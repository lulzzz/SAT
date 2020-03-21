using SAT_CL;
using SAT_CL.CXP;
using SAT_CL.FacturacionElectronica;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasPagar
{
    public partial class NotasCreditoCxP : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando si se produjo un PostBack
            if (!Page.IsPostBack)
            {
                inicializaForma();
            }
        }
        /// <summary>
        /// Evento de click a boton importar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            RetornoOperacion result = new RetornoOperacion();

            //Creamos lita de registros
            List<KeyValuePair<string, byte[]>> registros = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista de errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene registros
            if (gvVistaPrevia.DataKeys.Count > 0)
            {
                //Obtiene filas seleccionadas 
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvVistaPrevia, "chkVarios");
                //Verificando que existan filas seleccionadas
                if (selected_rows.Length != 0)
                {
                    foreach (GridViewRow row in selected_rows)
                    {
                        gvVistaPrevia.SelectedIndex = row.RowIndex;
                        //Instanciando Compania Emisora (Proveedor)
                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(gvVistaPrevia.SelectedDataKey["rfcE"].ToString(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Validando que Exista el Proveedor
                            if (emi.id_compania_emisor_receptor == 0)
                            {
                                //Cargando Tipos de Servicio
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 28, "");

                                //Mostrando Proveedor por Ingresar
                                lblProveedorFactura.Text = gvVistaPrevia.SelectedDataKey["Emisor"].ToString();

                                //Mostrando ventana Modal
                                TSDK.ASP.ScriptServer.AlternarVentana(upbtnImportar, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                                result = new RetornoOperacion("", false);
                                //guardaXML();
                            }
                            else
                            {
                                result = new RetornoOperacion(true);
                            }
                        }
                    }
                    if (result.OperacionExitosa)
                        guardaXML();
                }
                else//Mostrando Mensaje
                    ScriptServer.MuestraNotificacion(this.Page, "Debe Seleccionar un Comprobante", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Click en botón vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            generarVistaPrevia();
        }
        /// <summary>
        /// Click en botón borrar archivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            //Limpiando archivos de sesión
            Session["id_registro_b"] = null;
            //Limpiando grid
            Controles.InicializaGridview(gvVistaPrevia);
            //Eliminando de Session
            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
        }
        /// <summary>
        /// Click en Validar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbValidacion_Click(object sender, ImageClickEventArgs e)
        {
            validaEstatusPublicacionSAT(imbValidacion);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarOperacion_Click(object sender, EventArgs e)
        {
            //Ocultando ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarOperacion, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
            guardaXML();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarOperacion_Click(object sender, EventArgs e)
        {
            //Limpiando XML de Sesión
            Session["id_registro_b"] = null;
            Session["id_registro"] = null;
            //Ocultando ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarOperacion, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbEliminar_Click(object sender, ImageClickEventArgs e)
        {
            if (gvVistaPrevia.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvVistaPrevia, sender, "imb", true);
                //Quitando Fila
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["TableImportacion"].Select("Cont = " + gvVistaPrevia.SelectedDataKey["Cont"].ToString()))
                    dr.Delete();

                ((DataSet)Session["DS"]).Tables["TableImportacion"].AcceptChanges();
                Controles.InicializaIndices(gvVistaPrevia);
                Controles.CargaGridView(gvVistaPrevia, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), "Cont-Id-xml-nombre-rfcE-rfcR-Emisor-UUID-Total-FechaFactura-TipoComprobante", lblOrdenarVistaPrevia.Text, true, 1);
            }
        }
        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {//Validando Si el GridView contiene Registros
            if (gvVistaPrevia.DataKeys.Count > 0)
            {   //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   //Caso para el CheckBox "Todos"
                    case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                        CheckBox chk = (CheckBox)gvVistaPrevia.HeaderRow.FindControl("chkTodos");
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvVistaPrevia, "chkVarios", chk.Checked);
                        break;
                }
            }
        }
        /// <summary>
        /// Cambia el tamaño del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVistaPrevia_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvVistaPrevia, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), Convert.ToInt32(ddlTamanoVistaPrevia.SelectedValue), true, 1);
        }
        /// <summary>
        /// Cambia el indice de página del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVistaPrevia_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvVistaPrevia, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Enlace a datos de cada fila del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVistaPrevia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validar el tipo de fila
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Castear controles desde la interfaz
                using (Label lblEstatusSAT = (Label)e.Row.FindControl("lblEstatusSAT"))
                using (Label lblEstatusSistema = (Label)e.Row.FindControl("lblEstatusSistema"))
                using (CheckBox chkVariosA = (CheckBox)e.Row.FindControl("chkVariosA"))
                {
                    if (chkVariosA.Checked)
                        lblEstatusSistema.Text = FacturadoProveedor.EstatusFactura.Aceptada.ToString();
                    else
                        lblEstatusSistema.Text = FacturadoProveedor.EstatusFactura.EnRevision.ToString();
                    //Validar que se recupera el control de kilometraje
                    if (lblEstatusSAT != null && lblEstatusSAT.Text != "")
                    {
                        if (lblEstatusSAT.Text.Contains("Cancelado") || lblEstatusSAT.Text.Contains("No Encontrado"))
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffb5c2");
                    }
                }
            }
        }
        /// <summary>
        /// Cambio de criterio de ordenamiento en gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVistaPrevia_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            lblOrdenarVistaPrevia.Text = Controles.CambiaSortExpressionGridView(gvVistaPrevia, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento producido al dar click en seleccionar la casilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkAceptadaE_CheckedChanged(object sender, EventArgs e)
        {
            //string estatus = "";
            //if (gvVistaPrevia.DataKeys.Count > 0)
            //{
            //Obtiene filas seleccionadas
            //GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvVistaPrevia, "chkVariosA");                    
            ////Verificando que existan filas seleccionadas
            //if (selected_rows.Length != 0)
            //{
            //    foreach (GridViewRow row in selected_rows)
            //    {
            //        //Para columnas de datos
            //        if (row.RowType == DataControlRowType.DataRow)
            //        {
            //            using (Label lblEstatusSistema = (Label)row.FindControl("lblEstatusSistema"))
            //            using (CheckBox chkVariosA = (CheckBox)row.FindControl("chkVariosA"))
            //            {
            //                if (chkVariosA.Checked)
            //                    lblEstatusSistema.Text = FacturadoProveedor.EstatusFactura.Aceptada.ToString();
            //                else
            //                    lblEstatusSistema.Text = FacturadoProveedor.EstatusFactura.EnRevision.ToString();
            //            }
            //        }
            //    }
            //}
            //}
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Inicializa contenido de formulario
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catálogos requeridos
            Session["id_registro_b"] = null;
            Session["id_registro"] = null;
            //Cargando lista de tamaño de GV de Vista Previa
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVistaPrevia, "", 26);
            //Carga catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 28, "");
            //Inicializando Contenido de GV
            Controles.InicializaGridview(gvVistaPrevia);
            //Eliminando de Session
            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
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

            //Si hay elementos
            if (!string.IsNullOrEmpty(archivoBase64))
            {
                //Validando tipo de archivo (mime type), debe ser .xml
                if (mimeType == "text/xml")
                {
                    List<byte[]> lista = new List<byte[]>();
                    List<string> archivos = new List<string>();
                    //Convietiendo archivo a bytes
                    byte[] array = Convert.FromBase64String(TSDK.Base.Cadena.RegresaCadenaSeparada(archivoBase64, "base64,", 1));
                    if (HttpContext.Current.Session["id_registro_b"] != null && HttpContext.Current.Session["id_registro"] != null)
                    {
                        try
                        {
                            lista = (List<byte[]>)HttpContext.Current.Session["id_registro_b"];
                            archivos = (List<string>)HttpContext.Current.Session["id_registro"];
                        }
                        catch (Exception ex) { }
                    }
                    if (lista.Count < 1)
                    {
                        //Añadiendo archivo
                        lista.Add(array);
                        archivos.Add(nombreArchivo);
                        //Salvando en sesión
                        HttpContext.Current.Session["id_registro_b"] = lista;
                        HttpContext.Current.Session["id_registro"] = archivos;
                        /*if (lista.Count > 1)
                            resultado = string.Format("'{0}' Archivos cargados correctamente!!!", lista.Count);
                        else*/
                        resultado = string.Format("Archivo '{0}' cargado correctamente.", nombreArchivo);
                    }
                    else
                    {
                        //Limpiando archivos de sesión
                        //HttpContext.Current.Session["id_registro_b"] = null;
                        //HttpContext.Current.Session["id_registro"] = null;
                        resultado = "'Solo puede cargar máximo 1 archivo.";
                    }
                }
                //Si el tipo de archivo no es válido
                else
                    resultado = "El archivo seleccionado no tiene un formato válido. Formato permitido '.xml'.";
            }
            //Archivo sin contenido
            else
                resultado = "No se encontró contenido en el archivo.";

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el proceso de visualización de cambios a realizar en base a la información recuperada desde el archivo
        /// </summary>
        private void generarVistaPrevia()
        {
            //Declarando objeto de resultado
            List<RetornoOperacion> resultados = new List<RetornoOperacion>();
            string archivo = "";
            bool res = true;

            //Validando Sesión Registro B
            if (Session["id_registro_b"] != null && Session["id_registro"] != null)
            {
                //Creando tabla concentradora de información
                DataTable dtImportacion = new DataTable();
                //Añadiendo columna para enumerar resultados
                DataColumn cID = new DataColumn("cont", typeof(int));
                cID.AutoIncrement = true;
                cID.AutoIncrementSeed = 1;
                cID.AutoIncrementStep = 1;
                dtImportacion.Columns.Add(cID);
                dtImportacion.Columns.Add("id", typeof(int));
                dtImportacion.Columns.Add("xml", typeof(string));
                dtImportacion.Columns.Add("nombre", typeof(string));
                dtImportacion.Columns.Add("rfcE", typeof(string));
                dtImportacion.Columns.Add("rfcR", typeof(string));
                dtImportacion.Columns.Add("TipoComprobante", typeof(string));
                dtImportacion.Columns.Add("Emisor", typeof(string));
                dtImportacion.Columns.Add("Receptor", typeof(string));
                dtImportacion.Columns.Add("Serie", typeof(string));
                dtImportacion.Columns.Add("Folio", typeof(string));
                dtImportacion.Columns.Add("UUID", typeof(string));
                dtImportacion.Columns.Add("FechaFactura", typeof(DateTime));
                dtImportacion.Columns.Add("SubTotal", typeof(decimal));
                dtImportacion.Columns.Add("Descuento", typeof(decimal));
                dtImportacion.Columns.Add("Trasladado", typeof(decimal));
                dtImportacion.Columns.Add("Retenido", typeof(decimal));
                dtImportacion.Columns.Add("Total", typeof(decimal));
                dtImportacion.Columns.Add("EstatusSistema", typeof(string));
                dtImportacion.Columns.Add("EstatusSAT", typeof(string));
                //Para cada uno de los archivos cargados
                foreach (byte[] b in (List<byte[]>)Session["id_registro_b"])
                {
                    //Inicializando resultado
                    RetornoOperacion resultado = new RetornoOperacion();

                    //Intentando Obtener campos del XML 
                    try
                    {
                        //Traduciendo a texto XML
                        XDocument doc = XDocument.Load(new MemoryStream(b));
                        //si se cargó correctamente
                        if (doc != null)
                        {
                            //Recuperando datos de interés desde el XML
                            XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                            string version = doc.Root.Attribute("version") != null ? doc.Root.Attribute("version").Value : doc.Root.Attribute("Version").Value;
                            //Obteniendo versión de CFDI
                            byte tipoCFDI;
                            if (version.Equals("3.2"))
                            {
                                switch (doc.Root.Attribute("tipoDeComprobante").Value.ToLower())
                                {
                                    case "ingreso":
                                        tipoCFDI = 1;
                                        break;
                                    case "egreso":
                                        tipoCFDI = 2;
                                        break;
                                    default:
                                        tipoCFDI = 3;
                                        break;
                                }
                            }
                            else
                            {
                                switch (doc.Root.Attribute("TipoDeComprobante").Value.ToUpper())
                                {
                                    case "I":
                                        tipoCFDI = 1;
                                        break;
                                    case "E":
                                        tipoCFDI = 2;
                                        break;
                                    default:
                                        tipoCFDI = 3;
                                        break;
                                }
                            }

                            if(tipoCFDI.Equals("ingreso") || tipoCFDI.Equals("E"))
                            {
                                //string IdByte = doc.ToString();
                                //int id_cfdi = 0;
                                string rfcE = version.Equals("3.2") ? doc.Root.Element(ns + "Emisor").Attribute("rfc").Value : doc.Root.Element(ns + "Emisor").Attribute("Rfc").Value;
                                string rfcR = version.Equals("3.2") ? doc.Root.Element(ns + "Emisor").Attribute("rfc").Value : doc.Root.Element(ns + "Emisor").Attribute("Rfc").Value;
                                string emisor = version.Equals("3.2") ? doc.Root.Element(ns + "Emisor").Attribute("nombre").Value : doc.Root.Element(ns + "Emisor").Attribute("Nombre").Value;
                                string receptor = version.Equals("3.2") ? doc.Root.Element(ns + "Receptor").Attribute("nombre").Value : doc.Root.Element(ns + "Receptor").Attribute("Nombre").Value;

                                string folio = "", serie = "";
                                if (doc.Root.Attribute("serie") != null || doc.Root.Attribute("Serie") != null)
                                {
                                    serie = version.Equals("3.2") ? doc.Root.Attribute("serie").Value : doc.Root.Attribute("Serie").Value;
                                }
                                if (doc.Root.Attribute("folio") != null || doc.Root.Attribute("Folio") != null)
                                {
                                    folio = version.Equals("3.2") ? doc.Root.Attribute("folio").Value : doc.Root.Attribute("Folio").Value;
                                }
                                DateTime fecha = DateTime.Parse(version.Equals("3.2") ? doc.Root.Attribute("fecha").Value : doc.Root.Attribute("Fecha").Value);
                                XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                                   where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                                   select el).DefaultIfEmpty(null).FirstOrDefault();
                                string uuid = timbre.Attribute("UUID").Value;
                                string tipoComprobante = doc.Root.Attribute("TipoDeComprobante").Value.ToUpper();
                                decimal subtotal = Convert.ToDecimal(version.Equals("3.2") ? doc.Root.Attribute("subTotal").Value : doc.Root.Attribute("SubTotal").Value);
                                decimal retenciones = Convert.ToDecimal(version.Equals("3.2") ? TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "totalImpuestosRetenidos", "0") : TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "TotalImpuestosRetenidos", "0"));
                                decimal traslados = Convert.ToDecimal(version.Equals("3.2") ? TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "totalImpuestosTrasladados", "0") : TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "TotalImpuestosTrasladados", "0"));
                                decimal descuento = Convert.ToDecimal(version.Equals("3.2") ? TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root, "descuento", "0") : TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root, "Descuento", "0"));
                                decimal total = Convert.ToDecimal(version.Equals("3.2") ? doc.Root.Attribute("total").Value : doc.Root.Attribute("Total").Value);
                                string estatus = FacturadoProveedor.EstatusFactura.EnRevision.ToString();
                                //Para cada uno de los archivos cargados
                                foreach (string a in (List<string>)Session["id_registro"])
                                {
                                    if (a.Contains(folio) || a.Contains(uuid))
                                        archivo = a.ToString();
                                }
                                dtImportacion.Rows.Add(null, null, doc, archivo, rfcE, rfcR, tipoComprobante, emisor, receptor, serie, folio, uuid, fecha, subtotal, descuento, traslados, retenciones, total, estatus);
                            }
                            else
                                resultado = new RetornoOperacion("El XML cargado no corresponde a un Egreso. Seleccione un XML de tipo Egreso.");

                        }
                        else
                            resultado = new RetornoOperacion("Error al leer contenido de archivo XML.");
                    }
                    catch (Exception ex)
                    {
                        resultado = new RetornoOperacion(string.Format("Excepción al importar archivo: {0}", ex.Message));
                    }
                    //Añadiendo resultado
                    resultados.Add(resultado);
                }
                //Almacenando resultados en sesión
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtImportacion, "TableImportacion");

                //Borrando archivo de memoria, una vez que se cargó a una tabla
                //Session["id_registro_b"] = null;
                //Session["id_registro"] = null;
                //Limpiando nombre de archivo
                //ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");

                //Llenando gridview de vista previa (Sin llaves de selección)
                Controles.CargaGridView(gvVistaPrevia, dtImportacion, "Cont-Id-xml-nombre-rfcE-rfcR-Emisor-UUID-Total-FechaFactura-TipoComprobante-EstatusSistema", lblOrdenarVistaPrevia.Text, true, 1);

                //Señalando resultado exitoso
                resultados.Add(new RetornoOperacion("Vista Previa generada con éxito.", true));
            }
            else
                //Instanciando Excepcion
                resultados.Add(new RetornoOperacion("Debe de cargar una Factura"));

            //Mostrando resultado general
            RetornoOperacion global = RetornoOperacion.ValidaResultadoOperacionMultiple(resultados, RetornoOperacion.TipoValidacionMultiple.Cualquiera, " | ");
            ScriptServer.MuestraNotificacion(this.Page, global, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza la validación del estatus de publicación del CFDI en servidores del SAT
        /// </summary>
        private void validaEstatusPublicacionSAT(System.Web.UI.Control control)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando auxiliares
            string rfc_emisor, rfc_receptor, UUID, estatus;
            decimal monto; DateTime fecha_expedicion;

            //Creamos lita de registros
            List<KeyValuePair<string, byte[]>> registros = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista de errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene registros
            if (gvVistaPrevia.DataKeys.Count > 0)
            {
                //Obtiene filas seleccionadas 
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvVistaPrevia, "chkVarios");
                //Verificando que existan filas seleccionadas
                if (selected_rows.Length != 0)
                {
                    foreach (GridViewRow row in selected_rows)
                    {
                        gvVistaPrevia.SelectedIndex = row.RowIndex;
                        //Obteniendo Documento XML
                        XDocument xDocument = XDocument.Parse(gvVistaPrevia.SelectedDataKey["xml"].ToString());
                        XNamespace ns = xDocument.Root.GetNamespaceOfPrefix("cfdi");
                        XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");

                        //Convirtiendo Documento
                        XmlDocument xmlDocument = new XmlDocument();
                        using (var xmlReader = xDocument.CreateReader())
                        {
                            //Cargando XML Document
                            xmlDocument.Load(xmlReader);
                        }

                        //Validando versión
                        switch (xDocument.Root.Attribute("version") != null ? xDocument.Root.Attribute("version").Value : xDocument.Root.Attribute("Version").Value)
                        {
                            case "3.2":
                            case "3.3":
                                {
                                    //Realizando validación de estatus en SAT
                                    result = Comprobante.ValidaEstatusPublicacionSAT(xmlDocument, out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);

                                    //Colocando resultado sobre controles
                                    //using (Label lblEstatusSAT = (Label)row.FindControl("lblEstatusSAT"))                                    
                                    //    lblEstatusSAT.Text = result.Mensaje;
                                    //Validar el tipo de fila
                                    if (row.RowType == DataControlRowType.DataRow)
                                    {
                                        //Castear controles desde la interfaz
                                        using (Label lblEstatusSAT = (Label)row.FindControl("lblEstatusSAT"))
                                        using (Label lblObservacion = (Label)row.FindControl("lblObservacion"))
                                        {
                                            estatus = Cadena.RegresaCadenaSeparada(result.Mensaje, " '", 1);
                                            lblEstatusSAT.Text = Cadena.RegresaCadenaSeparada(estatus, "': ", 0);
                                            lblObservacion.Text = Cadena.RegresaCadenaSeparada(estatus, "': ", 1);
                                            //Validar que se recupera el control de kilometraje
                                            if (lblEstatusSAT != null && lblEstatusSAT.Text != "")
                                            {
                                                if (lblEstatusSAT.Text.Contains("Cancelado") || lblEstatusSAT.Text.Contains("No Encontrado"))
                                                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffb5c2");
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Importar el Archvio XML
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion guardaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();
            int idfactura = 0;
            ////Obteniendo Archivo XML
            //if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion")))
            //{
            //    foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion").Select())
            //    {
            //Creamos lita de registros
            List<KeyValuePair<string, byte[]>> registros = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista de errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene registros
            if (gvVistaPrevia.DataKeys.Count > 0)
            {
                //Obtiene filas seleccionadas 
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvVistaPrevia, "chkVarios");
                //Verificando que existan filas seleccionadas
                if (selected_rows.Length != 0)
                {
                    foreach (GridViewRow row in selected_rows)
                    {
                        using (CheckBox chkVariosA = (CheckBox)row.FindControl("chkVariosA"))
                        {
                            gvVistaPrevia.SelectedIndex = row.RowIndex;
                            XDocument documento = XDocument.Parse(gvVistaPrevia.SelectedDataKey["xml"].ToString());
                            //Validando versión
                            switch (documento.Root.Attribute("version") != null ? documento.Root.Attribute("version").Value : documento.Root.Attribute("Version").Value)
                            {
                                case "3.2":
                                    {
                                        //Insertando CFDI 3.2
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                 Convert.ToInt32(ddlTipoServicio.SelectedValue), gvVistaPrevia.SelectedDataKey["nombre"].ToString(), documento, chkVariosA.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        idfactura = result.IdRegistro;
                                        break;
                                    }
                                case "3.3":
                                    {
                                        //Insertando CFDI 3.3
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion33(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                 Convert.ToInt32(ddlTipoServicio.SelectedValue), gvVistaPrevia.SelectedDataKey["nombre"].ToString(), documento, chkVariosA.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        idfactura = result.IdRegistro;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            //result = new RetornoOperacion("Debe Seleccionar un Comprobante");
            //    }
            //    ((DataSet)Session["DS"]).Tables["TableImportacion"].AcceptChanges();
            //}
            if (result.OperacionExitosa)
            {
                Controles.InicializaIndices(gvVistaPrevia);
                //Llenando gridview de vista previa (Sin llaves de selección)
                Controles.InicializaGridview(gvVistaPrevia);
                //Eliminando de Session
                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
                //Controles.CargaGridView(gvVistaPrevia, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), "Cont-Id-xml-nombre-rfcE-rfcR-Emisor-UUID-Total-FechaFactura-TipoComprobante", lblOrdenarVistaPrevia.Text, true, 1);
            }
            //Eliminando Contenido en Sessión del XML (Ya que existe un script que limpia el contenido del área de arrastre del xml después del click final de guardado)
            Session["id_registro_b"] = null;
            Session["id_registro"] = null;
            //Actualizamos la etiqueta de errores
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo resultado Obtenido
            return result;
        }
        #endregion
    }
}