using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Mantenimiento
{
    public partial class ImportacionLecturasDiesel : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse una Petición al Servidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando PostBack
            if (!Page.IsPostBack)

                //Inicializando Forma
                inicializaForma();
        }
        /// <summary>
        /// Almacena un archivo en memoria de sesión
        /// </summary>
        /// <param name="archivoBase64">Contenido del archivo en formato Base64</param>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="mimeType">Tipo de contendio del archivo</param>
        /// <returns></returns>
        [WebMethod]
        public static string LecturaArchivo(string archivoBase64, string nombreArchivo, string mimeType)
        {
            //Definiendo objeto de retorno
            string resultado = "";
            RetornoOperacion result = new RetornoOperacion(1);

            //Si hay elementos
            if (!string.IsNullOrEmpty(archivoBase64))
            {
                //Validando tipo de archivo (mime type), debe ser .xml
                if (mimeType == "text/xml")
                {
                    try
                    {
                        //Declarando Lista de Lecturas
                        List<XDocument> lista = new List<XDocument>();
                        //Convietiendo archivo a bytes
                        byte[] responseData = Convert.FromBase64String(TSDK.Base.Cadena.RegresaCadenaSeparada(archivoBase64, "base64,", 1));
                        //Declarando Documento XML
                        XmlDocument doc = new XmlDocument();
                        XDocument responseXml = new XDocument();

                        //Obteniendo XML en cadena
                        using (MemoryStream ms = new MemoryStream(responseData))
                            //Cargando Documento XML
                            doc.Load(ms);

                        //Convirtiendo XML
                        responseXml = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);

                        //Validando Sesión
                        if (HttpContext.Current.Session["id_registro_b"] != null)
                        {
                            try
                            {
                                //Obteniendo Lista de Archivos
                                lista = (List<XDocument>)HttpContext.Current.Session["id_registro_b"];

                                //Obteniendo Documento repetido
                                XDocument rep = (from XDocument file in lista
                                                 where file.ToString().Equals(responseXml.ToString())
                                                 select file).FirstOrDefault();

                                //Validando documento
                                if (rep != null)

                                    //Instanciando Excepción
                                    result = new RetornoOperacion("Archivo cargado previamente");
                            }
                            catch (Exception ex)
                            {
                                //Instanciando Excepción
                                resultado = string.Format("Error al Cargar el Archivo: '{0}'", ex.Message);
                            }
                        }

                        //Validando Resultado
                        if (result.OperacionExitosa)
                        {
                            //Añadiendo Elemento a la Lista
                            lista.Add(responseXml);

                            //Almacenando en variables de sesión
                            HttpContext.Current.Session["id_registro_b"] = lista;
                        }

                        //Validando Lista
                        if (lista.Count > 1)

                            //Personalizando Respuesta
                            resultado = string.Format("'{0}' Archivos cargados correctamente!!!", lista.Count);
                        else
                            //Instanciando Resultado Positivo
                            resultado = string.Format("Archivo '{0}' cargado correctamente!!!", nombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        //Limpiando en variables de sesión
                        HttpContext.Current.Session["id_registro_b"] = null;
                        //Instanciando Excepción
                        resultado = string.Format("Error al Cargar el Archivo: '{0}'", ex.Message);
                    }
                }
                //Si el tipo de archivo no es válido
                else
                    resultado = "El archivo seleccionado no tiene un formato válido. Formatos permitidos '.xml'.";
            }
            //Archivo sin contenido
            else
                resultado = "No se encontró contenido en el archivo.";

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Click en botón vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            //Invocando Método de Vista Previa
            generarVistaPrevia();
        }
        /// <summary>
        /// Cambia el tamaño del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVistaPrevia_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvLecturasImportacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), Convert.ToInt32(ddlTamanoVistaPrevia.SelectedValue), true, 1);
        }
        /// <summary>
        /// Cambia el indice de página del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLecturasImportacion_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvLecturasImportacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Enlace a datos de cada fila del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLecturasImportacion_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //Si hay datos que mostrar
            if (gvLecturasImportacion.DataKeys.Count > 0)
            {
                //Si es una fila de datos
                if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
                {
                    //Recuperando información de la fila actual
                    DataRow fila = ((DataRowView)e.Row.DataItem).Row;

                    //Validando que exista el Dato
                    if (!fila["IdEstatus"].ToString().Equals(""))
                    {
                        //Obteniendo Control
                        using (LinkButton lkbEliminar = (LinkButton)e.Row.FindControl("lnkEliminar"))
                        {
                            //Validando el Estatus del Ingreso
                            if (Convert.ToInt32(fila["IdEstatus"]) > 0)
                            {
                                //Validando Control
                                if (lkbEliminar != null)

                                    //Ocultando Control
                                    lkbEliminar.Visible = false;

                                //Validando Resultado sin Errores
                                if (Convert.ToInt32(fila["IdEstatus"]) == 1)
                                {
                                    //Se marca la fila en color gris
                                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FCFCFC");
                                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#505050");
                                }
                                else if (Convert.ToByte(fila["IdEstatus"]) == 2)
                                {
                                    //Se marca la fila en color gris
                                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#C3EBFF");
                                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#505050");
                                }
                            }
                            else
                            {
                                //Se marca la fila en color blanco
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                                e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#505050");

                                //Validando Control
                                if (lkbEliminar != null)

                                    //Ocultando Control
                                    lkbEliminar.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Cambio de criterio de ordenamiento en gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLecturasImportacion_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            lblOrdenarVistaPrevia.Text = Controles.CambiaSortExpressionGridView(gvLecturasImportacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvLecturasImportacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvLecturasImportacion, sender, "lnk", false);

                //Declarando OBjeto de retorno
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Tabla de Sesión
                using (DataTable dtImportacion = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"))
                {
                    //Validando Registros
                    if (Validacion.ValidaOrigenDatos(dtImportacion))
                    {
                        //Obteniendo Fila Seleccionada
                        foreach (DataRow dr in dtImportacion.Select(string.Format("Id = {0}", gvLecturasImportacion.SelectedDataKey["Id"])))
                        {
                            //Eliminando Fila
                            dtImportacion.Rows.Remove(dr);
                            //Aceptando Cambios
                            dtImportacion.AcceptChanges();
                            //Instanciando Resultado
                            result = new RetornoOperacion(Convert.ToInt32(gvLecturasImportacion.SelectedDataKey["Id"]));
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existen datos por Eliminar");

                    //Validando operación
                    if (result.OperacionExitosa)
                    {
                        //Validando Registros
                        if (Validacion.ValidaOrigenDatos(dtImportacion))
                        {
                            //Llenando gridview de vista previa (Sin llaves de selección)
                            Controles.CargaGridView(gvLecturasImportacion, dtImportacion, "Id", lblOrdenarVistaPrevia.Text, true, 1);
                            //Almacenando resultados en sesión
                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtImportacion, "TableImportacion");
                        }
                        else
                        {
                            //Llenando gridview de vista previa (Sin llaves de selección)
                            Controles.InicializaGridview(gvLecturasImportacion);
                            //Almacenando resultados en sesión
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
                        }
                    }
                }

                //Mostrando Resultado
                ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Click en botón importar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            agregarLecturaUnidad();
        }
        /// <summary>
        /// Click en Finalizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrarActualizar_Click(object sender, EventArgs e)
        {
            //Inicializando Forma
            inicializaForma();
            //Cerrando y actualizando padre
            ScriptServer.MuestraNotificacion(btnCerrarActualizar, new RetornoOperacion(1, "Agregue sus Lecturas", true), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Forma
        /// </summary>
        private void inicializaForma()
        {
            //Cargando lista de tamaño de GV de Vista Previa
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVistaPrevia, "", 26);
            //Limpiando sesión
            Session["id_registro_b"] = null;
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
            //Inicializando Contenido de GV
            Controles.InicializaGridview(gvLecturasImportacion);
        }
        /// <summary>
        /// Realiza el proceso de visualización de cambios a realizar en base a la información recuperada desde el archivo
        /// </summary>
        private void generarVistaPrevia()
        {
            //Declarando resultao de carga de vista previa
            RetornoOperacion resultado = new RetornoOperacion("Primero debe cargar un archivo .xml.");

            //Validando existencia de archivo en sesión
            if (Session["id_registro_b"] != null)
            {
                //Obteniendo Lista de Elementos XML
                List<XDocument> lista = new List<XDocument>();

                try
                { lista = (List<XDocument>)Session["id_registro_b"]; }
                catch (Exception e) { }

                //Validando Lista de Elementos
                if (lista.Count > 0)
                {
                    //Declarando Tabla Temporal
                    DataTable dtLecturas = configuraTablaLecturas();

                    //Recorriendo Lista de XML
                    foreach (XDocument xml in lista)
                    {
                        //Validando Origen de la Lectura
                        switch (determinaOrigenLectura(xml))
                        {
                            case "DDEC":
                                {
                                    //Obteniendo Lecturas ISX
                                    configuraLecturaDDEC(xml, ref dtLecturas);
                                    break;
                                }
                            case "ISX":
                                {
                                    //Obteniendo Lecturas ISX
                                    configuraLecturaISX(xml, ref dtLecturas);
                                    break;
                                }
                            default:
                                {
                                    //Instanciando Excepción
                                    resultado = new RetornoOperacion("Debe solicitar el desarrollo de la importación de su archivo de Lecturas.");
                                    break;
                                }
                        }
                    }

                    //Validando Registros
                    if (Validacion.ValidaOrigenDatos(dtLecturas))
                    {
                        //Borrando archivo de memoria, una vez que se cargó a una tabla
                        Session["id_registro_b"] = null;

                        //Limpiando nombre de archivo
                        ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");

                        //Llenando gridview de vista previa (Sin llaves de selección)
                        Controles.CargaGridView(gvLecturasImportacion, dtLecturas, "Id", lblOrdenarVistaPrevia.Text, true, 1);

                        //Almacenando resultados en sesión
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtLecturas, "TableImportacion");

                        //Señalando resultado exitoso
                        resultado = new RetornoOperacion("Vista Previa generada con éxito.", true);
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("No se recuperaron la(s) Lectura(s) de (los) Archivo(s)");

                    //Limpiando Espacio en Memoria
                    dtLecturas = null;
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("Debe de agregar al menos un archivo");
            }

            //Notificando resultado obtenido
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Determinar el Origen de la Lectura
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        private string determinaOrigenLectura(XDocument xmlFile)
        {
            //Declarando Objeto de Retorno
            string origen = "";

            //Declarando variable Auxiliar
            RetornoOperacion result = new RetornoOperacion();

            try
            {
                //Validando Nodo
                result = new RetornoOperacion(xmlFile.Root.Element("DataFile").Attribute("VehicleID").Value != null ? 1 : -2);
            }
            catch (Exception e)
            {
                //Instanciando Excepción
                result = new RetornoOperacion(-2, e.Message, false);
            }

            //Validando Operación Exitosa
            if (result.OperacionExitosa)

                //Asignando Origen
                origen = "DDEC";
            else
            {
                try
                {
                    //Validando Nodo
                    result = new RetornoOperacion(xmlFile.Root.Element("DeviceInfo").Attribute("EngineModel").Value != null ? 1 : -2);
                }
                catch (Exception e)
                {
                    //Instanciando Excepción
                    result = new RetornoOperacion(-2, e.Message, false);
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)

                    //Asignando Origen
                    origen = "ISX";
            }

            //Devolviendo Resultado Obtenido
            return origen;
        }
        /// <summary>
        /// Método encargado de Configurar la tabla de Lecturas
        /// </summary>
        /// <returns></returns>
        private DataTable configuraTablaLecturas()
        {
            //Validación de Datos de Sesión


            //Declarando Tabla Temporal
            using (DataTable dtLecturas = new DataTable("Table"))
            {
                //Declarando Columnas
                DataColumn cID = new DataColumn("Id", typeof(int));
                cID.AutoIncrement = true;
                cID.AutoIncrementSeed = 1;
                cID.AutoIncrementStep = 1;
                dtLecturas.Columns.Add(cID);
                dtLecturas.Columns.Add("IdEstatus", typeof(int));
                dtLecturas.Columns.Add("IdUnidad", typeof(int));
                dtLecturas.Columns.Add("IdOperador", typeof(int));
                dtLecturas.Columns.Add("Unidad", typeof(string));
                dtLecturas.Columns.Add("Operador", typeof(string));
                dtLecturas.Columns.Add("FechaLectura", typeof(DateTime));
                dtLecturas.Columns.Add("KmsLectura", typeof(decimal));
                dtLecturas.Columns.Add("KmsSistema", typeof(decimal));
                dtLecturas.Columns.Add("HorasLectura", typeof(decimal));
                dtLecturas.Columns.Add("HrsLectura", typeof(string));
                dtLecturas.Columns.Add("LitrosLectura", typeof(decimal));
                dtLecturas.Columns.Add("Referencia", typeof(string));
                dtLecturas.Columns.Add("OdometroActual", typeof(string));
                dtLecturas.Columns.Add("EconomiaCombustible", typeof(string));
                dtLecturas.Columns.Add("VelocidadPromedio", typeof(string));
                dtLecturas.Columns.Add("ConsumoCombustible", typeof(string));
                dtLecturas.Columns.Add("Observaciones", typeof(string));

                //Devolviendo Tabla Inicial
                return dtLecturas;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="dtLecturas"></param>
        /// <returns></returns>
        private void configuraLecturaDDEC(XDocument xml, ref DataTable dtLecturas)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Variable de Unidad
            string unidad = "";

            /** OBTENIENDO CAMPOS DE LA LECTURA **/
            //Declarando Variables Auxiliares
            int horas_lectura = 0;
            DateTime fecha_lectura = DateTime.MinValue;
            DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
            TimeSpan horas = new TimeSpan();
            decimal kms_sistema = 0.00M, kms_lectura = 0.00M, litros_lectura = 0.00M;
            string ecoCombustible = "", velPromedio = "", consumo = "", odometro = "";
            XElement datos_generales = null, actividad_viaje = null, configuracion = null,
                     distancia = null, tiempo = null, combustible = null,
                     eco_comb = null, vel_pro = null, con_comb = null, odom = null;

            try
            {
                //Obteniendo Archivos
                IEnumerable<XElement> unidades = xml.Root.Elements("DataFile");

                //Validando Unidades Obtenidas
                if (unidades != null)
                {
                    //Recorriendo Unidades
                    foreach (XElement tr in unidades)
                    {
                        //Recuperando Unidad
                        unidad = tr.Attribute("VehicleID").Value;

                        //Instanciando Unidad
                        using (SAT_CL.Global.Unidad eco = new SAT_CL.Global.Unidad(unidad, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        using (SAT_CL.Global.Operador ope = new SAT_CL.Global.Operador(eco.id_operador))
                        {
                            //Validando Unidad
                            if (eco.habilitar)
                            {
                                //Inicializando Variables Auxiliares
                                datos_generales = actividad_viaje = configuracion =
                                distancia = tiempo = combustible = eco_comb =
                                vel_pro = con_comb = odom = null;

                                //Obteniendo Nodos Principales
                                datos_generales = tr;
                                actividad_viaje = tr.Element("TripActivity");
                                configuracion = tr.Element("Configuration");

                                //Obteniendo Parametros Especificos
                                distancia = (from XElement param in actividad_viaje.Elements("Parameter")
                                             where param.Attribute("Name").Value.Equals("Trip Distance")
                                             select param).FirstOrDefault();
                                tiempo = (from XElement param in actividad_viaje.Elements("Parameter")
                                          where param.Attribute("Name").Value.Equals("Trip Time")
                                          select param).FirstOrDefault();
                                combustible = (from XElement param in actividad_viaje.Elements("Parameter")
                                               where param.Attribute("Name").Value.Equals("Trip Fuel")
                                               select param).FirstOrDefault();

                                /**** Asignando Valores ****/
                                //Obteniendo Fecha
                                bool success = DateTime.TryParseExact(datos_generales.Attribute("PC_Date").Value, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fecha_lectura);
                                if (!success)
                                    DateTime.TryParse(datos_generales.Attribute("PC_Date").Value, out fecha_lectura);

                                //Validando Fecha de la Lectura
                                if (fecha_lectura != DateTime.MinValue)
                                {
                                    //Validando Fecha Actual contra la Fecha de Lectura
                                    if (DateTime.Compare(fecha_lectura, fecha_actual) <= 0)
                                    {
                                        //Calculando Horas
                                        horas = TimeSpan.FromHours(Convert.ToDouble(tiempo.Value));
                                        horas_lectura = (int)horas.TotalMinutes;

                                        //Obteniendo Kilometros
                                        kms_lectura = Math.Round(Convert.ToDecimal(distancia.Value), 2);

                                        //Obteniendo Litros
                                        litros_lectura = Math.Round(Convert.ToDecimal(combustible.Value), 2);

                                        //Instanciando Resultado Positivo
                                        resultado = new RetornoOperacion(1);
                                    }
                                    else
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("No puede Ingresar Lecturas con una Fecha posterior a la Fecha Actual");
                                }
                                else
                                    //Instanciando Excepción
                                    resultado = new RetornoOperacion("El Archivo no posee una Fecha Valida");

                                //Validando Elementos
                                if (resultado.OperacionExitosa)
                                {
                                    /**** Obteniendo Valores Opcionales ****/
                                    //Economía de Combustible
                                    try
                                    {
                                        eco_comb = (from XElement param in actividad_viaje.Elements("Parameter")
                                                    where param.Attribute("Name").Value.Equals("Trip Economy")
                                                    select param).FirstOrDefault();
                                        ecoCombustible = Math.Round(Convert.ToDecimal(eco_comb.Value), 2).ToString();
                                    }
                                    catch (Exception ex) { }
                                    //Velocidad del Vehículo Promedio
                                    try
                                    {
                                        vel_pro = (from XElement param in actividad_viaje.Elements("Parameter")
                                                   where param.Attribute("Name").Value.Equals("Avg Vehicle Speed")
                                                   select param).FirstOrDefault();
                                        velPromedio = Math.Round(Convert.ToDecimal(vel_pro.Value), 2).ToString();
                                    }
                                    catch (Exception ex) { }
                                    //Consumo de Combustible
                                    try
                                    {
                                        con_comb = (from XElement param in actividad_viaje.Elements("Parameter")
                                                    where param.Attribute("Name").Value.Equals("Fuel Consumption")
                                                    select param).FirstOrDefault();
                                        consumo = string.Format("{0:0.00}", Convert.ToDecimal(con_comb.Value));
                                    }
                                    catch (Exception ex) { }
                                    //Odómetro Actual
                                    try
                                    {
                                        odom = (from XElement param in configuracion.Elements("Parameter")
                                                where param.Attribute("Name").Value.Equals("Current Odometer")
                                                select param).FirstOrDefault();
                                        odometro = odom.Value;
                                    }
                                    catch (Exception ex) { }

                                    //Insertando Registro
                                    dtLecturas.Rows.Add(null, 1, eco.id_unidad, ope.id_operador, eco.numero_unidad, string.IsNullOrEmpty(ope.nombre) ? "NO TIENE OPERADOR" : ope.nombre, fecha_lectura,
                                                        kms_lectura, kms_sistema, horas_lectura, Cadena.ConvierteMinutosACadena(horas_lectura),
                                                        litros_lectura, "Importado desde Archivo DDEC", odometro, ecoCombustible, velPromedio, consumo,
                                                        "Lectura obtenida exitosamente");
                                }
                                else
                                    //Agregando Excepción
                                    dtLecturas.Rows.Add(null, -1, 0, 0, unidad, "", fecha_lectura, 0, 0, 0, "", 0, "", "", "", "", "", resultado.Mensaje);
                            }
                            else
                                //Agregando Excepción
                                dtLecturas.Rows.Add(null, -1, 0, 0, unidad, "", null, 0, 0, 0, "", 0, "", "", "", "", "", string.Format("No se puede recuperar la Unidad con el Económico '{0}'", unidad));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Agregando Excepción
                dtLecturas.Rows.Add(null, -1, 0, 0, unidad, "", null, 0, 0, 0, "", 0, "", "", "", "", "", ex.Message);
                //Instanciando Excepción
                resultado = new RetornoOperacion(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private void configuraLecturaISX(XDocument xml, ref DataTable dtLecturas)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Unidad
            string unidad = "";

            //Recuperando Unidad
            unidad = xml.Root.Element("DeviceInfo").Attribute("UnitNumber").Value;

            //Instanciando Unidad
            using (SAT_CL.Global.Unidad eco = new SAT_CL.Global.Unidad(unidad.Trim(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            using (SAT_CL.Global.Operador ope = new SAT_CL.Global.Operador(eco.id_operador))
            {
                //Validando Unidad
                if (eco.habilitar)
                {
                    /** OBTENIENDO CAMPOS DE LA LECTURA **/
                    //Declarando Variables Auxiliares
                    int horas_lectura = 0;
                    DateTime fecha_lectura = DateTime.MinValue;
                    DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
                    TimeSpan horas = new TimeSpan();
                    decimal kms_sistema = 0.00M, kms_lectura = 0.00M, litros_lectura = 0.00M;
                    string ecoCombustible = "", velPromedio = "", consumo = "", odometro = "";
                    XElement datos_generales = null, actividad_viaje = null,
                             distancia = null, tiempo = null, combustible = null,
                             eco_comb = null, vel_pro = null, con_comb = null, odom = null;

                    try
                    {
                        //Obteniendo Nodos Principales
                        datos_generales = xml.Root.Element("DeviceInfo");
                        actividad_viaje = xml.Root.Element("TripInfoParameters");

                        //Obteniendo Parametros Especificos
                        distancia = (from XElement param in actividad_viaje.Elements("TripInfo")
                                     where param.Attribute("Name").Value.Equals("Trip Distance")
                                     select param).FirstOrDefault();
                        tiempo = (from XElement param in actividad_viaje.Elements("TripInfo")
                                  where param.Attribute("Name").Value.Equals("Trip Time")
                                  select param).FirstOrDefault();
                        combustible = (from XElement param in actividad_viaje.Elements("TripInfo")
                                       where param.Attribute("Name").Value.Equals("Trip Fuel Used")
                                       select param).FirstOrDefault();

                        /**** Asignando Valores ****/
                        //Obteniendo Fecha
                        bool success = DateTime.TryParseExact(datos_generales.Attribute("ReportDate").Value, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fecha_lectura);
                        if (!success)
                            DateTime.TryParse(datos_generales.Attribute("ReportDate").Value, out fecha_lectura);

                        //Validando Fecha de la Lectura
                        if (fecha_lectura != DateTime.MinValue)
                        {
                            //Validando Fecha Actual contra la Fecha de Lectura
                            if (DateTime.Compare(fecha_lectura, fecha_actual) <= 0)
                            {
                                //Calculando Horas
                                horas = TimeSpan.FromHours(Convert.ToDouble(tiempo.Attribute("Value").Value));
                                horas_lectura = (int)horas.TotalMinutes;

                                //Obteniendo Kilometros
                                kms_lectura = Math.Round(Convert.ToDecimal(distancia.Attribute("Value").Value), 2);

                                //Obteniendo Litros
                                litros_lectura = Math.Round(Convert.ToDecimal(combustible.Attribute("Value").Value), 2);

                                //Instanciando Resultado Positivo
                                resultado = new RetornoOperacion(1);
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No puede Ingresar Lecturas con una Fecha posterior a la Fecha Actual");
                        }
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("El Archivo no posee una Fecha Valida");
                    }
                    catch (Exception ex)
                    {
                        //Agregando Excepción
                        dtLecturas.Rows.Add(null, -1, 0, 0, unidad, "", null, 0, 0, 0, "", 0, "", "", "", "", "", ex.Message);
                        //Instanciando Excepción
                        resultado = new RetornoOperacion(ex.Message);
                    }

                    //Validando Elementos
                    if (resultado.OperacionExitosa)
                    {
                        /**** Obteniendo Valores Opcionales ****/
                        //Economía de Combustible
                        try
                        {
                            eco_comb = (from XElement param in actividad_viaje.Elements("TripInfo")
                                        where param.Attribute("Name").Value.Equals("Trip Average Fuel Economy")
                                        select param).FirstOrDefault();
                            ecoCombustible = Math.Round(Convert.ToDecimal(eco_comb.Attribute("Value").Value), 2).ToString();
                        }
                        catch (Exception ex) { }
                        //Velocidad del Vehículo Promedio
                        try
                        {
                            vel_pro = (from XElement param in actividad_viaje.Elements("TripInfo")
                                       where param.Attribute("Name").Value.Equals("Average Vehicle Speed")
                                       select param).FirstOrDefault();
                            velPromedio = Math.Round(Convert.ToDecimal(vel_pro.Attribute("Value").Value), 2).ToString();
                        }
                        catch (Exception ex) { }
                        //Consumo de Combustible
                        try
                        {
                            consumo = Math.Round((litros_lectura / Convert.ToDecimal(horas.TotalHours)), 2).ToString();
                        }
                        catch (Exception ex) { }
                        //Odómetro Actual
                        try
                        {
                            odom = (from XElement param in actividad_viaje.Elements("TripInfo")
                                    where param.Attribute("Name").Value.Equals("Total ECM Distance")
                                    select param).FirstOrDefault();
                            odometro = odom.Attribute("Value").Value;
                        }
                        catch (Exception ex) { }

                        //Insertando Registro
                        dtLecturas.Rows.Add(null, 1, eco.id_unidad, ope.id_operador, eco.numero_unidad, string.IsNullOrEmpty(ope.nombre) ? "NO TIENE OPERADOR" : ope.nombre, fecha_lectura,
                                            kms_lectura, kms_sistema, horas_lectura, Cadena.ConvierteMinutosACadena(horas_lectura),
                                            litros_lectura, "Importado desde Archivo ISX", odometro, ecoCombustible, velPromedio, consumo,
                                            "Lectura obtenida exitosamente");
                    }
                    else
                        //Agregando Excepción
                        dtLecturas.Rows.Add(null, -1, 0, 0, unidad, "", fecha_lectura, 0, 0, 0, "", 0, "", "", "", "", "", resultado.Mensaje);
                }
                else
                    //Agregando Excepción
                    dtLecturas.Rows.Add(null, -1, 0, 0, unidad, "", null, 0, 0, 0, "", 0, "", "", "", "", "", string.Format("No se puede recuperar la Unidad con el Económico '{0}'", unidad));
            }
        }
        /// <summary>
        /// Realiza la insercción de Lecturas a las Unidades
        /// </summary>
        private void agregarLecturaUnidad()
        {
            //Declarando objeto de resultado
            List<RetornoOperacion> resultados = new List<RetornoOperacion>();
            RetornoOperacion global = new RetornoOperacion();
            bool res = true;

            //Validando que existan registros que modificar
            using (DataTable dtLecturasDiesel = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(dtLecturasDiesel))
                {
                    //Validando Registros con Errores
                    if ((from DataRow row in dtLecturasDiesel.Rows
                         where Convert.ToInt32(row["IdEstatus"]) == -1
                         select row).Count() == 0)
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtLecturasDiesel.Rows)
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Obteniendo Fecha de Lectura
                                DateTime fecha_lec;
                                DateTime.TryParse(dr["FechaLectura"].ToString(), out fecha_lec);
                                int idLectura = 0;

                                //Declarando Objeto de Retorno
                                RetornoOperacion lectura = SAT_CL.Mantenimiento.Lectura.InsertarLectura(fecha_lec, Convert.ToInt32(dr["IdUnidad"]),
                                        Convert.ToInt32(dr["IdOperador"]), "", Convert.ToDecimal(dr["KmsSistema"]), Convert.ToDecimal(dr["KmsLectura"]),
                                        Convert.ToInt32(dr["HorasLectura"]), Convert.ToDecimal(dr["LitrosLectura"]), dr["Referencia"].ToString(),
                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando Resultado Positivo
                                if (lectura.OperacionExitosa)
                                {
                                    //Guardando Lectura
                                    idLectura = lectura.IdRegistro;

                                    //Validando EconomiaCombustible
                                    if (!dr["EconomiaCombustible"].ToString().Equals(""))

                                        //Insertando Referencia
                                        lectura = SAT_CL.Global.Referencia.InsertaReferencia(idLectura, 151, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0,
                                                        151, "Economía de Combustible", 0, "Reportes - Actividades del Viaje"), dr["EconomiaCombustible"].ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación
                                    if (lectura.OperacionExitosa)
                                    {
                                        //Validando EconomiaCombustible
                                        if (!dr["VelocidadPromedio"].ToString().Equals(""))

                                            //Insertando Referencia
                                            lectura = SAT_CL.Global.Referencia.InsertaReferencia(idLectura, 151, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0,
                                                            151, "Velocidad del Vehículo Promedio", 0, "Reportes - Actividades del Viaje"), dr["VelocidadPromedio"].ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }

                                    //Validando Operación
                                    if (lectura.OperacionExitosa)
                                    {
                                        //Validando EconomiaCombustible
                                        if (!dr["OdometroActual"].ToString().Equals(""))
                                        {
                                            //Insertando Referencia
                                            lectura = SAT_CL.Global.Referencia.InsertaReferencia(idLectura, 151, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0,
                                                            151, "Odómetro Actual", 0, "Reportes - Actividades del Viaje"), dr["OdometroActual"].ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            decimal odometro = 0.00M;
                                            decimal.TryParse(dr["OdometroActual"].ToString(), out odometro);

                                            //Validando operación y Odometro
                                            if (lectura.OperacionExitosa && odometro > 0.00M)
                                            {
                                                //Instanciando Unidad
                                                using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(dr["IdUnidad"])))
                                                {
                                                    //Validando Unidad
                                                    if (unidad.habilitar)

                                                        //Actualizando Odometro en KMS Asignado
                                                        lectura = unidad.ActualizaOdometroUnidad(odometro, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                }
                                            }
                                        }
                                    }

                                    //Validando Operación
                                    if (lectura.OperacionExitosa)
                                    {
                                        //Validando EconomiaCombustible
                                        if (!dr["ConsumoCombustible"].ToString().Equals(""))

                                            //Insertando Referencia
                                            lectura = SAT_CL.Global.Referencia.InsertaReferencia(idLectura, 151, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0,
                                                            151, "Consumo de Combustible", 0, "Reportes - Actividades del Viaje"), dr["ConsumoCombustible"].ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }

                                    //Completando Transacción
                                    if (idLectura > 0 && lectura.OperacionExitosa)
                                    {
                                        //Instanciando Lectura
                                        lectura = new RetornoOperacion(idLectura);
                                        //Completando Transacción
                                        scope.Complete();
                                    }

                                    //Añadiendo Resultado a Lista
                                    resultados.Add(lectura.OperacionExitosa ? new RetornoOperacion(lectura.IdRegistro, "Lectura Importada con Exito", true) : lectura);
                                }
                            }
                        }

                        //Mostrando resultado general
                        global = RetornoOperacion.ValidaResultadoOperacionMultiple(resultados, RetornoOperacion.TipoValidacionMultiple.Cualquiera, " | ");
                    }
                    else
                        //Instanciando Excepción
                        global = new RetornoOperacion("Existen registros con errores");
                }
                else
                    //Instanciando Excepción
                    global = new RetornoOperacion("Existen registros con errores");
            }

            //Mostrando Resultado general
            ScriptServer.MuestraNotificacion(btnImportar, global, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Limpiando nombre de archivo
            if (global.OperacionExitosa)
            {
                //Limpiando sesión
                Session["id_registro_b"] = null;
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
                //Mostrando notificación
                ScriptServer.MuestraNotificacion(this, global, ScriptServer.PosicionNotificacion.AbajoDerecha);
                //Borrando nombres de archivos cargados
                ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");
            }
            else
            {
                //Limpiando sesión
                Session["id_registro_b"] = null;
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
                //Mostrando notificación
                ScriptServer.MuestraNotificacion(this, "Vuelva a cargar el archivo y presione 'Vista Previa' antes que esta opción.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                //Limpiando nombre de archivo
                ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");
            }

            //Borrando contenido de gv
            Controles.InicializaGridview(gvLecturasImportacion);
        }

        #endregion
    }
}