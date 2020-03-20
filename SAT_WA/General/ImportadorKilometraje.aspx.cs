using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using UBI = SAT_CL.Global;

namespace SAT.General
{
    public partial class ImportadorKilometraje : System.Web.UI.Page
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
            //Si no es recarga de página
            if (!Page.IsPostBack)
            {
                //Inicializa pagina
                inicializaPagina();
            }
        }
        /// <summary>
        /// Click en link de descarga de esquema de importación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDescargaEsquema_Click(object sender, EventArgs e)
        {
            //Realizando descarga de formaro de importación correspondiente
            TSDK.Base.Archivo.DescargaArchivo(File.ReadAllBytes(Server.MapPath("~/FormatosImportacion/KMS_DEFINICION.xlsx")), "Kilometraje.xlsx", TSDK.Base.Archivo.ContentType.ms_excel);
        }
        /// <summary>
        /// Click en botón vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            AgrupaUbicaciones();
        }

        /// <summary>
        /// Click en botón importar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            ImportacionKilometrajes();
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza el proceso de visualización de cambios a realizar en base a la información recuperada desde el archivo
        /// </summary>
        private void AgrupaUbicaciones()
        {
            //Declarando resultao de carga de vista previa
            RetornoOperacion resultado = new RetornoOperacion("Primero debe cargar un archivo .xls o .xlsx.");

            //Validando existencia de archivo en sesión
            if (Session["ArchivoImportacionKilometraje"] != null)
            {
                //Leyendo primer tabla
                using (DataTable mitExcel = Excel.DataTableDeExcelBytes((byte[])Session["ArchivoImportacionKilometraje"], "KILOMETRAJES"))
                {
                    //Si hay datos
                    if (mitExcel != null)
                    {
                        //Creando tabla concentradora de información
                        DataTable mitImportacion = new DataTable();

                        //Añadiendo columna para enumerar resultados
                        DataColumn cID = new DataColumn("Id", typeof(int));
                        cID.AutoIncrement = true;
                        cID.AutoIncrementSeed = 1;
                        cID.AutoIncrementStep = 1;
                        mitImportacion.Columns.Add(cID);
                        mitImportacion.Columns.Add("No", typeof(int));
                        mitImportacion.Columns.Add("IdO", typeof(int));
                        mitImportacion.Columns.Add("Origen", typeof(string));
                        mitImportacion.Columns.Add("IdD", typeof(int));
                        mitImportacion.Columns.Add("Destino", typeof(string));
                        mitImportacion.Columns.Add("KMS", typeof(decimal));
                        mitImportacion.Columns.Add("Tiempo", typeof(decimal));
                        mitImportacion.Columns.Add("KMSPago", typeof(decimal));
                        mitImportacion.Columns.Add("KMSCobro", typeof(decimal));
                        mitImportacion.Columns.Add("Observacion", typeof(string));
                        mitImportacion.Columns.Add("KMSActual", typeof(decimal));
                        mitImportacion.Columns.Add("IdKMS", typeof(int));

                        //Creando tabla concentradora de información
                        DataTable mitKilometrajes = new DataTable();

                        //Añadiendo columna para enumerar resultados
                        DataColumn cIK = new DataColumn("Id", typeof(int));
                        cIK.AutoIncrement = true;
                        cIK.AutoIncrementSeed = 1;
                        cIK.AutoIncrementStep = 1;
                        mitKilometrajes.Columns.Add(cIK);
                        mitKilometrajes.Columns.Add("No", typeof(int));
                        mitKilometrajes.Columns.Add("IdO", typeof(int));
                        mitKilometrajes.Columns.Add("Ori", typeof(string));
                        mitKilometrajes.Columns.Add("IdD", typeof(int));
                        mitKilometrajes.Columns.Add("Dest", typeof(string));
                        mitKilometrajes.Columns.Add("KMS", typeof(decimal));
                        mitKilometrajes.Columns.Add("Tiempo", typeof(decimal));
                        mitKilometrajes.Columns.Add("KMSPago", typeof(decimal));
                        mitKilometrajes.Columns.Add("KMSCobro", typeof(decimal));
                        mitKilometrajes.Columns.Add("Observacion", typeof(string));
                        mitKilometrajes.Columns.Add("KMSActual", typeof(decimal));
                        mitKilometrajes.Columns.Add("IdKMS", typeof(int));

                        //Creando tabla concentradora de información
                        DataTable dtUbicacionesSI = new DataTable();
                        dtUbicacionesSI.Columns.Add("No", typeof(int));
                        dtUbicacionesSI.Columns.Add("IdUbicacion", typeof(int));
                        dtUbicacionesSI.Columns.Add("Descripcion", typeof(string));
                        dtUbicacionesSI.Columns.Add("Geoubicacion", typeof(string));

                        //Agruparemos las ubicaciones 
                        using (DataTable dtAgrubicaciones = mitExcel)
                        {
                            //Cargando Reporte
                            using (DataTable dtKilometraje = SAT_CL.Global.Kilometraje.ObtieneKilometrajes(0, 0, 0, 0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            {
                                List<string> ODAgrupados = ((from DataRow ori in dtAgrubicaciones.Rows
                                                             select ori["ORIGEN"].ToString().Trim().ToUpper()).Union(
                                                              from DataRow des in dtAgrubicaciones.Rows
                                                              select des["DESTINO"].ToString().Trim().ToUpper())).ToList();
                                if (ODAgrupados.Count > 0)
                                {
                                    foreach (string dr in ODAgrupados)
                                    {
                                        if (dr != null)
                                        {
                                            //string Descripcion = dr.ToString().Trim().ToUpper() == "" ? dr : Cadena.RegresaCadenaSeparada(dr, "(", 0);
                                            string Descripcion = dr.Trim().ToUpper();
                                            //Instanciando concepto
                                            using (UBI.Ubicacion Ubicacion = new UBI.Ubicacion(Descripcion, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                            {
                                                //Validacion
                                                if (Ubicacion.habilitar)
                                                    //Ubicaciones que existen en la tabla global.ubicaciones
                                                    dtUbicacionesSI.Rows.Add(-1, Ubicacion.id_ubicacion, dr, Ubicacion.geoubicacion);
                                                else
                                                    //Ubicaciones que no se encuentran en la tabla y seran dadas de altas
                                                    dtUbicacionesSI.Rows.Add(-2, 0, dr, 0);
                                            }
                                        }
                                    }
                                }

                                //Obtiene los kilometrajes que se encuentran en excel 
                                List<DataRow> Kilometrajes = (from DataRow r in dtAgrubicaciones.Rows
                                                              select r).ToList();
                                if (Kilometrajes.Count > 0)
                                {
                                    foreach (DataRow dr in Kilometrajes)
                                    {
                                        string Origen = Convert.ToString(dr["ORIGEN"]);
                                        string Destino = Convert.ToString(dr["DESTINO"]);
                                        List<DataRow> IdUbicacionesOrigen = (from DataRow ori in dtUbicacionesSI.Rows
                                                                             where ori["Descripcion"].ToString().Trim().ToUpper().Contains(Origen.ToString().Trim().ToUpper())
                                                                             select ori).ToList();
                                        List<DataRow> IdUbicacionesDestino = (from DataRow des in dtUbicacionesSI.Rows
                                                                              where des["Descripcion"].ToString().Trim().ToUpper().Contains(Destino.ToString().Trim().ToUpper())
                                                                              select des).ToList();
                                        foreach (DataRow ori in IdUbicacionesOrigen)
                                        {
                                            foreach (DataRow des in IdUbicacionesDestino)
                                            {
                                                //Obtiene los kilometrajes a importar general
                                                List<DataRow> KilometrajesBD = (from DataRow kil in dtKilometraje.Rows
                                                                                where kil["IdOrigen"].ToString().Contains(Convert.ToString(ori["IdUbicacion"])) &&
                                                                                kil["IdDestino"].ToString().Contains(Convert.ToString(des["IdUbicacion"]))
                                                                                select kil).ToList();
                                                if (KilometrajesBD.Count == 1)
                                                    //resgistro -1 update
                                                    foreach (DataRow kil in KilometrajesBD)
                                                    {
                                                        //Tabla con origen y destino general 
                                                        decimal KmsActual = Convert.ToDecimal(kil["KmsReales"]);
                                                        decimal KmsNuevo = Convert.ToDecimal(dr["KMS REALES"]);
                                                        int errorU = 0;
                                                        if (KmsNuevo == KmsActual)
                                                        {
                                                            errorU = -7;
                                                        }
                                                        else if (KmsNuevo > KmsActual)
                                                        {
                                                            errorU = -8;
                                                        }
                                                        else if (KmsNuevo < KmsActual)
                                                        {
                                                            errorU = -9;
                                                        }
                                                        mitKilometrajes.Rows.Add(null, errorU, ori["IdUbicacion"], dr["ORIGEN"], des["IdUbicacion"], dr["DESTINO"], dr["KMS REALES"], dr["TIEMPO (MIN)"], dr["KMS (PAGO)"], dr["KMS (COBRO)"], "", kil["KmsReales"], kil["Id"]);
                                                        // mitImportacion.Rows.Add(null, error, ori["IdUbicacion"], dr["ORIGEN"], des["IdUbicacion"], dr["DESTINO"], dr["KMS REALES"], dr["TIEMPO (MIN)"], dr["KMS (PAGO)"], dr["KMS (COBRO)"],"Se realizara la modificacion actual", kil["KmsReales"]);
                                                    }
                                                else
                                                {
                                                    int error = (Convert.ToInt32(ori["No"]) + Convert.ToInt32(des["No"]));
                                                    mitKilometrajes.Rows.Add(null, error, ori["IdUbicacion"], dr["ORIGEN"], des["IdUbicacion"], dr["DESTINO"], dr["KMS REALES"], dr["TIEMPO (MIN)"], dr["KMS (PAGO)"], dr["KMS (COBRO)"], "Se realizara la modificacion actual", 0, 0);

                                                }
                                            }
                                        }
                                    }
                                    foreach (DataRow dp in mitKilometrajes.Rows)
                                    {
                                        List<DataRow> ag = (from DataRow r in mitKilometrajes.Rows
                                                            where Convert.ToInt32(r["IdO"]) == dp.Field<int>("IdO") &&
                                                            Convert.ToInt32(r["IdD"]) == dp.Field<int>("IdD") &&
                                                            r["Ori"].ToString().Equals(dp["Ori"].ToString()) &&
                                                            r["Dest"].ToString().Equals(dp["Dest"].ToString())
                                                            select r).ToList();

                                        //Validacion de kilometraje no se repite
                                        if (ag.Count == 1)
                                        {
                                            int error = (Convert.ToInt32(dp["No"]));
                                            string observacion = "";
                                            if (error == -1)
                                                observacion = "Este kilometraje se actualizara";
                                            if (error == -2)
                                                observacion = "Kilometraje listo para importación";
                                            else if (error == -3)
                                                observacion = "No existe";
                                            else if (error == -4)
                                                observacion = "No existe";
                                            else if (error == -7)
                                                observacion = "Se realizara la modificacion actual Los kilometrajes son iguales";
                                            else if (error == -8)
                                                observacion = "Se realizara la modificacion actual el kilometraje es mayor";
                                            else if (error == -9)
                                                observacion = "Se realizara la modificacion actual el kilometraje es menor";
                                            mitImportacion.Rows.Add(null, dp["No"], dp["IdO"], dp["Ori"], dp["IdD"], dp["Dest"], dp["KMS"], dp["Tiempo"], dp["KMSPago"], dp["KMSCobro"], observacion, dp["KMSActual"], dp["IdKMS"]);
                                        }
                                        //Validacion de kilometraje que se repite
                                        else if (ag.Count > 1)
                                        {
                                            int No = (Convert.ToInt32(dp["No"]));
                                            int error = 0;
                                            if (No == -2 || No == -3 || No == -4)
                                                error = -5;
                                            else
                                                error = -6;
                                            mitImportacion.Rows.Add(null, error, dp["IdO"], dp["Ori"], dp["IdD"], dp["Dest"], dp["KMS"], dp["Tiempo"], dp["KMSPago"], dp["KMSCobro"], "Kilometraje Repetido", dp["KMSActual"], dp["IdKMS"]);
                                        }
                                        else
                                        {
                                            //Error
                                        }
                                    }
                                }
                            }
                            List<DataRow> kg = (from DataRow k in mitImportacion.Rows
                                                where k.Field<int>("No") == -2 || k.Field<int>("No") == -3 || k.Field<int>("No") == -4 || k.Field<int>("No") == -5
                                                select k).ToList();
                            //Valida que el list tenga datos nuevos
                            if (kg.Count > 1)
                            {
                                DataTable KMSNew = (from DataRow k in mitImportacion.Rows
                                                    where k.Field<int>("No") == -2 || k.Field<int>("No") == -3 || k.Field<int>("No") == -4 || k.Field<int>("No") == -5
                                                    orderby k.Field<int>("No") ascending
                                                    select k).CopyToDataTable();
                                //Si hay datos
                                if (KMSNew != null)
                                {
                                    //Almacenando resultados en sesión
                                    Session["DSNew"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DSNew"], KMSNew, "TableNew");
                                    //Llenando gridview de vista previa (Sin llaves de selección)
                                    Controles.CargaGridView(gvKmsNuevos, KMSNew, "Id-No", lblOrdenarVistaPreviaKmsNuevos.Text, true, 1);
                                }
                            }
                            else
                                Controles.InicializaGridview(gvKmsNuevos);

                            List<DataRow> ug = (from DataRow u in mitImportacion.Rows
                                                where u.Field<int>("No") == -1 || u.Field<int>("No") == -6 || u.Field<int>("No") == -7 || u.Field<int>("No") == -8 || u.Field<int>("No") == -9
                                                select u).ToList();
                            //Valida que exista datos para modificacion
                            if (ug.Count > 1)
                            {
                                //Obtiene los kilometrajes update a importar general
                                DataTable KMSUpdate = (from DataRow u in mitImportacion.Rows
                                                       where u.Field<int>("No") == -1 || u.Field<int>("No") == -6 || u.Field<int>("No") == -7 || u.Field<int>("No") == -8 || u.Field<int>("No") == -9
                                                       orderby u.Field<int>("No") ascending
                                                       select u).CopyToDataTable();
                                //Si hay datos
                                if (KMSUpdate != null)
                                {
                                    //Almacenando resultados en sesión
                                    Session["DSUpdate"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DSUpdate"], KMSUpdate, "TableUpdate");
                                    //Llenando gridview de vista previa (Sin llaves de selección)
                                    Controles.CargaGridView(gvKmsExistentes, KMSUpdate, "Id-No", lblOrdenarVistaPreviaKmsExistentes.Text, true, 1);
                                }
                            }
                            else
                                Controles.InicializaGridview(gvKmsExistentes);
                            //Almacenando resultados en sesión
                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitImportacion, "TableImportacion");
                            //Borrando archivo de memoria, una vez que se cargó a una tabla
                            Session["ArchivoImportacionKilometraje"] = null;
                            //Limpiando nombre de archivo
                            ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");
                            //Señalando resultado exitoso
                            resultado = new RetornoOperacion("Vista Previa generada con éxito.", true);
                        }
                    }
                    //De lo contrario señalando error
                    else
                    {
                        resultado = new RetornoOperacion("No fue posible encontrar la hoja 'CARGOS' en este archivo, por favor valide que sea el archivo correcto y tenga el formato permitido.");
                    }
                }
            }
            //Notificando resultado obtenido
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza la actualización de montos solicitados (insertando, editano o eliminando conceptos), una vez actualizados se realiza la agrupación de los registros
        /// </summary>
        private void ImportacionKilometrajes()
        {
            //Declarando resultao de carga de vista previa
            RetornoOperacion resultado = new RetornoOperacion("Primero debe cargar un archivo .xls o .xlsx.");
            //Validando que existan registros que modificar
            using (DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"))
            {
                //Obtiene los kilometrajes a importar general
                List<DataRow> Validacion = (from DataRow r in mit.Rows
                                            where r.Field<int>("No") != -1 && r.Field<int>("No") != -2 && r.Field<int>("No") != -7 && r.Field<int>("No") != -8 && r.Field<int>("No") != -9
                                            select r).ToList();
                if (Validacion.Count > 1)

                    //Señalando resultado exitoso
                    resultado = new RetornoOperacion("No se puede importar el documento, validar errores", false);
                else
                {
                    //Obtiene los registros que importaran sin ningun problema los cuales se identifican por no = -2
                    List<DataRow> Insertar = (from DataRow r in mit.Rows
                                                  //where r.Field<int>("No") == -2
                                              select r).ToList();
                    foreach (DataRow dr in Insertar)
                    {
                        int Opcion = Convert.ToInt32(dr["No"]);
                        if (Opcion == -7 || Opcion == -8 || Opcion == -9)
                            Opcion = -1;
                        switch (Opcion)
                        {
                            case -1:
                                //Modificacion de registo
                                using (SAT_CL.Global.Kilometraje km = new SAT_CL.Global.Kilometraje(Convert.ToInt32(dr["IdKMS"])))
                                {
                                    //Validando que exista el Kilometraje
                                    if (km.habilitar)
                                    {
                                        if (km.id_kilometraje > 0)
                                        {
                                            decimal KmsActual = Convert.ToDecimal(dr["KMSActual"]);
                                            decimal KmsNuevo = Convert.ToDecimal(dr["KMS"]);
                                            if (KmsActual == KmsNuevo)
                                            {
                                                resultado = new RetornoOperacion(true);
                                            }
                                            else
                                                //Editando Kilometraje
                                                resultado = km.EditaKilometrajeImportacion(Convert.ToInt32(dr["IdO"]), Convert.ToInt32(dr["IdD"]),
                                                Convert.ToDecimal(dr["KMS"]), Convert.ToDecimal(Cadena.VerificaCadenaVacia(Convert.ToString(dr["Tiempo"]), "0")),
                                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(Convert.ToString(dr["KMSPago"]), "0")), Convert.ToDecimal(Cadena.VerificaCadenaVacia(Convert.ToString(dr["KMSCobro"]), "0")),
                                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        }
                                    }
                                }
                                break;
                            //Inserta nuevo registro
                            case -2:
                                resultado = SAT_CL.Global.Kilometraje.InsertaKilometraje(Convert.ToInt32(dr["IdO"]), Convert.ToInt32(dr["IdD"])
                                , SqlGeography.Null, SqlGeography.Null, Convert.ToDecimal(dr["KMS"]), 0.00M, Convert.ToDecimal(Cadena.VerificaCadenaVacia(Convert.ToString(dr["Tiempo"]), "0")), 0.00M, 0,
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(Convert.ToString(dr["KMSPago"]), "0")), Convert.ToDecimal(Cadena.VerificaCadenaVacia(Convert.ToString(dr["KMSCobro"]), "0")),
                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                break;
                        }
                    }
                }
            }
            //inicializa pagina
            inicializaPagina();
            //Notificando resultado obtenido
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaPagina()
        {

            //Limpiando sesión
            Session["ArchivoImportacionKilometraje"] = null;
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
            Session["DSNew"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DSNew"], "TableNew");
            Session["DSUpdate"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DSUpdate"], "TableUpdate");
            //Inicializando Contenido de GV
            Controles.InicializaGridview(gvKmsNuevos);
            Controles.InicializaGridview(gvKmsExistentes);
            //Invocando Método de Carga
            cargaCatalogos();
        }

        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando lista de tamaño de GV de Vista Previa
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVistaPreviaKmsExistentes, "", 26);
            //Cargando lista de tamaño de GV de Vista Previa
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVistaPreviaKmsNuevos, "", 26);
        }

        #endregion

        #region Métodos Públicos

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
                //Validando tipo de archivo (mime type), debe ser .xlsx o .xls
                if (mimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                    mimeType == "application/vnd.ms-excel")
                {
                    //Convietiendo archivo a bytes
                    byte[] array = Convert.FromBase64String(TSDK.Base.Cadena.RegresaCadenaSeparada(archivoBase64, "base64,", 1));

                    //Almacenando en variable de sesión
                    HttpContext.Current.Session["ArchivoImportacionKilometraje"] = array;
                    resultado = string.Format("Archivo '{0}' cargado correctamente!!!", nombreArchivo);
                }
                //Si el tipo de archivo no es válido
                else
                    resultado = "El archivo seleccionado no tiene un formato válido. Formatos permitidos '.xls' / '.xlsx'.";
            }
            //Archivo sin contenido
            else
                resultado = "No se encontró contenido en el archivo.";

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Eventos GridView "KmsNuevos"
        /// <summary>
        /// Cambia el tamaño del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVistaPreviaKmsNuevos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvKmsNuevos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSNew"], "TableNew"), Convert.ToInt32(ddlTamanoVistaPreviaKmsNuevos.SelectedValue), true, 1);
        }
        /// <summary>
        /// Cambia el indice de página del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKmsNuevos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvKmsNuevos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSNew"], "TableNew"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Enlace a datos de cada fila del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKmsNuevos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //Si hay datos que mostrar
            if (gvKmsNuevos.DataKeys.Count > 0)
            {
                //Si es una fila de datos
                if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
                {
                    //Recuperando información de la fila actual
                    DataRow fila = ((DataRowView)e.Row.DataItem).Row;

                    /*
                     APLICANDO FORMATO DE VISUALIZACIÓN ACORDE A LA INFORMACIÓN DE LA FILA
                     */

                    //Si no se encontró un servicio
                    if (Convert.ToInt32(fila["No"]) == -2)
                    {
                        //Se marca la fila en color verde los que se importaran sin ninguna problema
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ACD453");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    //Si hubo errores de generación de vista previa
                    else if (Convert.ToInt32(fila["No"]) == -3 || Convert.ToInt32(fila["No"]) == -4 || Convert.ToInt32(fila["No"]) == -5)
                    {
                        //Fondo rojo el kilometraje lo que no se importaran 
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }

                }
            }
        }
        /// <summary>
        /// Cambio de criterio de ordenamiento en gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKmsNuevos_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            lblOrdenarVistaPreviaKmsNuevos.Text = Controles.CambiaSortExpressionGridView(gvKmsNuevos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSNew"], "TableNew"), e.SortExpression, true, 1);
        }
        #endregion

        #region Eventos GridView "KmsExistentes"
        /// <summary>
        /// Cambia el tamaño del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVistaPreviaKmsExistentes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvKmsExistentes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSUpdate"], "TableUpdate"), Convert.ToInt32(ddlTamanoVistaPreviaKmsExistentes.SelectedValue), true, 1);
        }

        /// <summary>
        /// Cambia el indice de página del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKmsExistentes_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvKmsExistentes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSUpdate"], "TableUpdate"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Enlace a datos de cada fila del gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKmsExistentes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //Si hay datos que mostrar
            if (gvKmsExistentes.DataKeys.Count > 0)
            {
                //Si es una fila de datos
                if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
                {
                    //Recuperando información de la fila actual
                    DataRow fila = ((DataRowView)e.Row.DataItem).Row;
                    /*
                     APLICANDO FORMATO DE VISUALIZACIÓN ACORDE A LA INFORMACIÓN DE LA FILA
                     */
                    if (Convert.ToInt32(fila["No"]) == -6)
                    {
                        //Se marca la fila en color gris
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#909090");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    else if (Convert.ToInt32(fila["No"]) == -8)
                    {
                        //Se marca la fila en color verde  kilometraje mayor
                        e.Row.Cells[4].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
                    }
                    //Si hubo errores de generación de vista previa
                    else if (Convert.ToInt32(fila["No"]) == -7)
                    {
                        //Se marca la fila en color negro kilometraje igual
                        e.Row.Cells[4].BackColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        e.Row.Cells[4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    //Si hay servicio
                    else if (Convert.ToInt32(fila["No"]) == -9)
                    {
                        //Fondo rojo el kilometraje es menor
                        e.Row.Cells[4].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                    }
                }
            }
        }
        /// <summary>
        /// Cambio de criterio de ordenamiento en gv de vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKmsExistentes_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            lblOrdenarVistaPreviaKmsExistentes.Text = Controles.CambiaSortExpressionGridView(gvKmsNuevos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSUpdate"], "TableUpdate"), e.SortExpression, true, 1);
        }

        /// <summary>
        /// Evento Producido al Exportar un Reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch (lnk.CommandName)
            {
                case "ExportarExistentes":
                    //Exportando Contenido
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSUpdate"], "TableUpdate"), "Id", "No", "IdO", "IdD", "KMSActual", "IdKMS");
                    break;
                case "ExportarNuevos":
                    //Exportando Contenido
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DSNew"], "TableNew"), "Id", "No", "IdO", "IdD", "KMSActual", "IdKMS");
                    break;
            }
        }
        #endregion

    }
}