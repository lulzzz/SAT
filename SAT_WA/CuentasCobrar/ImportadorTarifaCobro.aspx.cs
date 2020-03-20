using SAT_CL.Documentacion;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using FAC = SAT_CL.Facturacion;
using TAR = SAT_CL.Tarifas;

namespace SAT.CuentasPagar
{
    public partial class ImportadorTarifaCobro : System.Web.UI.Page
    {

        #region Eventos

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es recarga de página
            if (!Page.IsPostBack)
            {
                //Cargando lista de tamaño de GV de Vista Previa
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVistaPrevia, "", 26);
                //Limpiando sesión
                Session["ArchivoImportacionTarifa"] = null;
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
                //Inicializando Contenido de GV
                Controles.InicializaGridview(gvVistaPrevia);
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
            TSDK.Base.Archivo.DescargaArchivo(File.ReadAllBytes(Server.MapPath("~/FormatosImportacion/TARIFA_COBRO_SERVICIOS.xlsx")), "TARIFA_COBRO_SERVICIOS.xlsx", TSDK.Base.Archivo.ContentType.ms_excel);
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
        protected void gvVistaPrevia_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //Si hay datos que mostrar
            if (gvVistaPrevia.DataKeys.Count > 0)
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
                    if (Convert.ToInt32(fila["IdServicio"]) == 0)
                    {
                        //Se marca la fila en color gris
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#909090");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    //Si hubo errores de generación de vista previa
                    else if (Convert.ToInt32(fila["IdServicio"]) == -1)
                    {
                        //Se marca la fila en color blanco
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#505050");
                    }
                    //Si no se ha terminado el servicio
                    else if (fila["FechaFin"] == DBNull.Value)
                    {
                        //Se marca la fila en color gris
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#909090");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }                    
                    //Si se pretende facturar electrónicamente pero no está disponible
                    else if (Convert.ToInt32(fila["IdFacturaGlobal"]) > 0 && !Convert.ToBoolean(fila["DisponibleFacturaGlobal"]))
                    {
                        //Se marca la fila en color gris
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#909090");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    //Si está facturado electrónicamente y se pretende añadir a paquete
                    else if (Convert.ToBoolean(fila["FE"]) && Convert.ToInt32(fila["IdPaqueteRevision"]) > 0 && Convert.ToBoolean(fila["DisponibleRevision"]))
                    {
                            //Se marca la fila en color azul
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFC2");
                            e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#505050");
                    }                    
                    //Si sólo está facturado y no se requiere agruparlo (FG o paquete revisión)
                    else if (Convert.ToBoolean(fila["FE"]))
                    {
                        //Se marca la fila en color gris
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#909090");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    //Si NO se puede añadir a paquete
                    else if (Convert.ToInt32(fila["IdPaqueteRevision"]) > 0 && !Convert.ToBoolean(fila["DisponibleRevision"]))
                    {
                        //Se marca la fila en color gris
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#909090");
                        e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    //Si hay servicio
                    else
                    {
                        //Fondo gris suave predeterminado
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FCFCFC");

                        //Determinando cambios en montos
                        decimal flete_actual = Convert.ToDecimal(fila["FleteActual"]);
                        decimal flete_nuevo = Convert.ToDecimal(fila["Flete"]);
                        decimal maniobras_actual = Convert.ToDecimal(fila["ManiobrasActual"]);
                        decimal maniobras_nuevo = Convert.ToDecimal(fila["Maniobras"]);
                        decimal casetas_actual = Convert.ToDecimal(fila["CasetasActual"]);
                        decimal casetas_nuevo = Convert.ToDecimal(fila["Casetas"]);
                        decimal varios_actual = Convert.ToDecimal(fila["VariosActual"]);
                        decimal varios_nuevo = Convert.ToDecimal(fila["Varios"]);
                        decimal renta_actual = Convert.ToDecimal(fila["RentaActual"]);
                        decimal renta_nuevo = Convert.ToDecimal(fila["Renta"]);
                        decimal estadias_actual = Convert.ToDecimal(fila["EstadiasActual"]);
                        decimal estadias_nuevo = Convert.ToDecimal(fila["Estadias"]);
                        //Flete actual mayor al nuevo
                        if (flete_actual > flete_nuevo)
                        {
                            e.Row.Cells[8].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                        //Flete actual menor que el nuevo
                        else if (flete_actual < flete_nuevo)
                        {
                            e.Row.Cells[8].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
                        }

                        //Varios actual mayor al nueva
                        if (varios_actual > varios_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                        //Varios actual menor que la nueva
                        else if (varios_actual < varios_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
                        }

                        //Maniobras actual mayor al nueva
                        if (maniobras_actual > maniobras_nuevo)
                        {
                            e.Row.Cells[10].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                        //Maniobras actual menor que la nueva
                        else if (maniobras_actual < maniobras_nuevo)
                        {
                            e.Row.Cells[10].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
                        }

                        //Casetas actual mayor al nueva
                        if (casetas_actual > casetas_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                        //Maniobras actual menor que la nueva
                        else if (casetas_actual < casetas_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
                        }
                        //Varios actual mayor al nueva
                        if (varios_actual > varios_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                        //Varios actual menor que la nueva
                        else if (varios_actual < varios_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
                        }
                        //Renta actual mayor al nueva
                        if (renta_actual > renta_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                        //Renta actual menor que la nueva
                        else if (renta_actual < renta_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
                        }
                        //Renta actual mayor al nueva
                        if (estadias_actual > estadias_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                        //Renta actual menor que la nueva
                        else if (estadias_actual < estadias_nuevo)
                        {
                            e.Row.Cells[13].BackColor = System.Drawing.ColorTranslator.FromHtml("#A5D16F");
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
        protected void gvVistaPrevia_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            lblOrdenarVistaPrevia.Text = Controles.CambiaSortExpressionGridView(gvVistaPrevia, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Click en botón importar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            actualizarMontosYAgrupar();
        }
        /// <summary>
        /// Click en Finalizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrarActualizar_Click(object sender, EventArgs e)
        {
            //Cerrando y actualizando padre
            ScriptServer.ActualizaVentanaPadre(this, true);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza el proceso de visualización de cambios a realizar en base a la información recuperada desde el archivo
        /// </summary>
        private void generarVistaPrevia()
        {
            //Declarando resultao de carga de vista previa
            RetornoOperacion resultado = new RetornoOperacion("Primero debe cargar un archivo .xls o .xlsx.");

            //Validando existencia de archivo en sesión
            if (Session["ArchivoImportacionTarifa"] != null)
            {
                //Leyendo primer tabla
                using (DataTable mitExcel = Excel.DataTableDeExcelBytes((byte[])Session["ArchivoImportacionTarifa"], "CARGOS"))
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

                        mitImportacion.Columns.Add("IdServicio", typeof(int));
                        mitImportacion.Columns.Add("IdFacturado", typeof(int));
                        mitImportacion.Columns.Add("SerieFolio", typeof(string));
                        mitImportacion.Columns.Add("NoServicio", typeof(string));
                        mitImportacion.Columns.Add("Referencia", typeof(string));
                        mitImportacion.Columns.Add("FE", typeof(bool));
                        mitImportacion.Columns.Add("Cliente", typeof(string));
                        mitImportacion.Columns.Add("Origen", typeof(string));
                        mitImportacion.Columns.Add("Destino", typeof(string));
                        DataColumn cFin = new DataColumn("FechaFin", typeof(DateTime));
                        cFin.AllowDBNull = true;
                        mitImportacion.Columns.Add(cFin);
                        mitImportacion.Columns.Add("IdsFlete", typeof(int));
                        mitImportacion.Columns.Add("FleteActual", typeof(decimal));
                        mitImportacion.Columns.Add("Flete", typeof(decimal));
                        mitImportacion.Columns.Add("IdsVarios", typeof(int));
                        mitImportacion.Columns.Add("VariosActual", typeof(decimal));
                        mitImportacion.Columns.Add("Varios", typeof(decimal));
                        mitImportacion.Columns.Add("IdsManiobra", typeof(int));
                        mitImportacion.Columns.Add("ManiobrasActual", typeof(decimal));
                        mitImportacion.Columns.Add("Maniobras", typeof(decimal));
                        mitImportacion.Columns.Add("MRetencion", typeof(bool));
                        mitImportacion.Columns.Add("IdsCaseta", typeof(int));
                        mitImportacion.Columns.Add("CasetasActual", typeof(decimal));
                        mitImportacion.Columns.Add("Casetas", typeof(decimal));
                        mitImportacion.Columns.Add("CRetencion", typeof(bool));
                        mitImportacion.Columns.Add("IdsRenta", typeof(int));
                        mitImportacion.Columns.Add("RentaActual", typeof(decimal));
                        mitImportacion.Columns.Add("Renta", typeof(decimal));
                        mitImportacion.Columns.Add("RRetencion", typeof(bool));
                        mitImportacion.Columns.Add("IdsEstadias", typeof(int));
                        mitImportacion.Columns.Add("EstadiasActual", typeof(decimal));
                        mitImportacion.Columns.Add("Estadias", typeof(decimal));
                        mitImportacion.Columns.Add("ERetencion", typeof(bool));
                        mitImportacion.Columns.Add("Nota", typeof(string));
                        mitImportacion.Columns.Add("DisponibleRevision", typeof(bool));
                        mitImportacion.Columns.Add("DisponibleFacturaGlobal", typeof(bool));
                        mitImportacion.Columns.Add("IdPaqueteRevision", typeof(int));
                        mitImportacion.Columns.Add("IdFacturaGlobal", typeof(int));  

                        //Obteniendo agrupación de FG o paquete de revisión activos acorde a solicitud
                        int idPaquete = 0, idFG = 0, idCliente = 0;
                        if (Request.QueryString["idPaqueteRevision"] != null)
                            //Recuperando Id de paquete
                            idPaquete = Convert.ToInt32(Request.QueryString["idPaqueteRevision"]);
                        if (Request.QueryString["idFacturaGlobal"] != null)
                            idFG = Convert.ToInt32(Request.QueryString["idFacturaGlobal"]);

                        //Recuperando el Id de cliente
                        if(idPaquete > 0){
                            using(FAC.PaqueteProceso pr = new FAC.PaqueteProceso(idPaquete))
                            {
                                if (pr.habilitar)
                                    idCliente = pr.id_cliente;
                                else
                                    idCliente = 0;
                            }
                        }
                        else if (idFG > 0)
                        {
                            using (FAC.FacturaGlobal fg = new FAC.FacturaGlobal(idFG))
                            {
                                if (fg.habilitar)
                                    idCliente = fg.id_compania_cliente;
                                else
                                    idCliente = 0;
                            }
                        }

                        if (idCliente > 0)
                        {
                            //Obteniendo Facturas
                            using (DataSet ds = FAC.FacturadoConcepto.CargaFacturasTodas(idFG, idCliente, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ""))
                            {
                                //Para cada elemento del archivo excel
                                foreach (DataRow rExcel in mitExcel.Rows)
                                {
                                    //Cargando información del servicio en cuestión
                                    int no_servicio = 0;
                                    int.TryParse(rExcel["NO_SERVICIO"].ToString(), out no_servicio);
                                    using (DataTable mitInfoServicio = SAT_CL.Facturacion.Reporte.ObtenerCargosPrincipalesServicio(no_servicio, rExcel["Referencia"].ToString(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                    {
                                        //Si se encontró la información requerida
                                        if (mitInfoServicio != null)
                                        {
                                            try
                                            {
                                                //Validando existencia de un solo registro servicio con esta referencia
                                                if ((from DataRow s in mitInfoServicio.Rows
                                                     select s["IdServicio"]).Distinct().Count() == 1)
                                                {
                                                    //Nota opcional para indicar facturación o servicio no concluido aún
                                                    string nota = "Listo para editar.";
                                                    bool disponibleRevision = false, disponibleFactura = false;

                                                    //Si el servicio está facturado electrónicamente
                                                    if (Convert.ToBoolean(mitInfoServicio.Rows[0]["FE"]))
                                                    {
                                                        nota = string.Format("Servicio Facturado en '{0}'. No es posible editar montos y sólo estará disponible para agregar en la modalidad de Paquete de Revisión.", mitInfoServicio.Rows[0]["SerieFolio"].ToString());
                                                    }
                                                    //Si el servicio no está terminado
                                                    else if (mitInfoServicio.Rows[0]["FechaFin"] == DBNull.Value)
                                                        nota = "El servicio no se ha dado por terminado aún. No es posible editar ni agrupar en Factura Global o Paquete de Revisión.";
                                                    //Si hay más de un concepto de alguno de los tipos (flete, maniobras, casetas)
                                                    else if (Convert.ToInt32(mitInfoServicio.Rows[0]["IdsFlete"]) > 1 || Convert.ToInt32(mitInfoServicio.Rows[0]["IdsManiobra"]) > 1
                                                            || Convert.ToInt32(mitInfoServicio.Rows[0]["IdsCaseta"]) > 1 || Convert.ToInt32(mitInfoServicio.Rows[0]["IdsVarios"]) > 1
                                                            || Convert.ToInt32(mitInfoServicio.Rows[0]["IdsRenta"]) > 1 || Convert.ToInt32(mitInfoServicio.Rows[0]["IdsEstadias"]) > 1)
                                                        nota += " Existen conceptos con más de una aparición, estos serán reemplazados por sólo un concepto con el monto solicitado.";

                                                    //Validando si es requerido incluir a factura global o paquete
                                                    if (idPaquete > 0 && idCliente > 0)
                                                    {
                                                        //Obteniendo Facturas Disponibles
                                                        List<DataRow> facturas_disponibles = (from DataRow fd in ds.Tables["Table"].Rows
                                                                                              where fd["NoViaje"].ToString().Trim().ToUpper().Contains(rExcel["Referencia"].ToString().Trim().ToUpper())
                                                                                              || fd["NoServicio"].ToString().Trim().ToUpper().Contains(rExcel["NO_SERVICIO"].ToString().Trim().ToUpper())
                                                                                              select fd).ToList();

                                                        //Validando disponibilidad
                                                        if (facturas_disponibles.Count == 0)
                                                            nota = (nota == "Listo para editar.") ? "Este servicio no pertenece al cliente o fue discriminado por algún criterio del Proceso de Revisión." : nota;
                                                        else
                                                            disponibleRevision = true;

                                                        //Validando disponibilidad
                                                        //if (!Validacion.ValidaOrigenDatos(FAC.PaqueteProcesoDetalle.CargaFacturasDisponibles(idPaquete, idCliente, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, mitInfoServicio.Rows[0]["Referencia"].ToString(), "")))
                                                        //nota = (nota == "Listo para editar.") ? "Este servicio no pertenece al cliente o fue discriminado por algún criterio del Proceso de Revisión." : nota;
                                                        //else
                                                        //disponibleRevision = true;
                                                    }
                                                    if (idFG > 0 && idCliente > 0 && !Convert.ToBoolean(mitInfoServicio.Rows[0]["FE"]))
                                                    {
                                                        //Obteniendo Facturas Todas
                                                        List<DataRow> facturas_todas = (from DataRow fd in ds.Tables["Table"].Rows
                                                                                        where fd["NoViaje"].ToString().Trim().ToUpper().Contains(rExcel["Referencia"].ToString().Trim().ToUpper())
                                                                                        || fd["NoServicio"].ToString().Trim().ToUpper().Contains(rExcel["NO_SERVICIO"].ToString().Trim().ToUpper())
                                                                                        select fd).ToList();

                                                        //Validando disponibilidad
                                                        if (facturas_todas.Count == 0)
                                                            nota = (nota == "Listo para editar.") ? "Este servicio no pertenece al cliente o fue discriminado por algún criterio de Facturación Global." : nota;
                                                        else
                                                            disponibleFactura = true;

                                                        //Validando disponibilidad
                                                        //if (!Validacion.ValidaOrigenDatos(FAC.FacturadoConcepto.CargaFacturasTodas(idFG, idCliente, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, mitInfoServicio.Rows[0]["Referencia"].ToString())))
                                                        //    nota = (nota == "Listo para editar.") ? "Este servicio no pertenece al cliente o fue discriminado por algún criterio de Facturación Global." : nota;
                                                        //else
                                                        //    disponibleFactura = true;
                                                    }

                                                    //Insertando registro coincidente con montos actuales y sugeridos desde archivo
                                                    mitImportacion.Rows.Add(null, Convert.ToInt32(mitInfoServicio.Rows[0]["IdServicio"]),
                                                                            Convert.ToInt32(mitInfoServicio.Rows[0]["IdFacturado"]),
                                                                            mitInfoServicio.Rows[0]["SerieFolio"].ToString(),
                                                                            mitInfoServicio.Rows[0]["NoServicio"].ToString(),
                                                                            mitInfoServicio.Rows[0]["Referencia"].ToString(),
                                                                            Convert.ToBoolean(mitInfoServicio.Rows[0]["FE"]),
                                                                            mitInfoServicio.Rows[0]["Cliente"].ToString(),
                                                                            mitInfoServicio.Rows[0]["Origen"].ToString(),
                                                                            mitInfoServicio.Rows[0]["Destino"].ToString(),
                                                                            mitInfoServicio.Rows[0]["FechaFin"],
                                                                            Convert.ToInt32(mitInfoServicio.Rows[0]["IdsFlete"]), Convert.ToDecimal(mitInfoServicio.Rows[0]["Flete"]), Convert.ToDecimal(rExcel["FLETE"].ToString() != "" ? rExcel["FLETE"] : 0),
                                                                             Convert.ToInt32(mitInfoServicio.Rows[0]["IdsVarios"]), Convert.ToDecimal(mitInfoServicio.Rows[0]["Varios"]), Convert.ToDecimal(rExcel["VARIOS"].ToString() != "" ? rExcel["VARIOS"] : 0),
                                                                            Convert.ToInt32(mitInfoServicio.Rows[0]["IdsManiobra"]), Convert.ToDecimal(mitInfoServicio.Rows[0]["Maniobras"]), Convert.ToDecimal(rExcel["MANIOBRAS"].ToString() != "" ? rExcel["MANIOBRAS"] : 0), rExcel["RETENCION_MANIOBRAS"].ToString().ToUpper() == "SI" ? true : false,
                                                                            Convert.ToInt32(mitInfoServicio.Rows[0]["IdsCaseta"]), Convert.ToDecimal(mitInfoServicio.Rows[0]["Casetas"]), Convert.ToDecimal(rExcel["CASETAS"].ToString() != "" ? rExcel["CASETAS"] : 0), rExcel["RETENCION_CASETAS"].ToString().ToUpper() == "SI" ? true : false,
                                                                            Convert.ToInt32(mitInfoServicio.Rows[0]["IdsRenta"]), Convert.ToDecimal(mitInfoServicio.Rows[0]["Renta"]), Convert.ToDecimal(rExcel["RENTA"].ToString() != "" ? rExcel["RENTA"] : 0), rExcel["RETENCION_RENTA"].ToString().ToUpper() == "SI" ? true : false,
                                                                            Convert.ToInt32(mitInfoServicio.Rows[0]["IdsEstadias"]), Convert.ToDecimal(mitInfoServicio.Rows[0]["Estadias"]), Convert.ToDecimal(rExcel["ESTADIAS"].ToString() != "" ? rExcel["ESTADIAS"] : 0), rExcel["RETENCION_ESTADIAS"].ToString().ToUpper() == "SI" ? true : false,
                                                                            nota, disponibleRevision, disponibleFactura, idPaquete, idFG);
                                                }
                                                //Si hay mas de uno
                                                else
                                                {
                                                    //Recuperando referencias repetidas
                                                    string repeticiones = string.Join("; ", (from DataRow r in mitInfoServicio.Rows
                                                                                             select string.Format("Serv. {0} -> {1} {2}", r["NoServicio"], r["TipoReferencia"], r["Referencia"])).ToArray());
                                                    //Insertando registro con error de búsqueda en concentrado
                                                    mitImportacion.Rows.Add(null, 0, 0, "", "", rExcel["Referencia"].ToString(), false, "", "", "", null, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, string.Format("Existe más de una coincidencia con esta Referencia: {0}", repeticiones), false, false, idPaquete, idFG);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                resultado = new RetornoOperacion(string.Format("Referencia '{0}': {1}", rExcel["Referencia"], ex.Message), false);
                                                mitImportacion.Rows.Add(null, -1, 0, "", "", rExcel["Referencia"].ToString(), false, "", "", "", null, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, string.Format("Error al generar vista previa: {0}", ex.Message), false, false, idPaquete, idFG);
                                            }
                                        }
                                        //Si no se encontró
                                        else
                                        {
                                            //Insertando registro con error de búsqueda en concentrado
                                            mitImportacion.Rows.Add(null, 0, 0, "", "", rExcel["Referencia"].ToString(), false, "", "", "", null, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, "Referencia no encontrada.", false, false, idPaquete, idFG);
                                        }
                                    }
                                }
                            }
                        }
                        //Si no se encontró
                        else
                        {
                            //Insertando registro con error de búsqueda en concentrado
                            mitImportacion.Rows.Add(null, 0, 0, "", "", "", false, "", "", "", null, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0, false, "No se puede recuperar el Cliente de la Operación", false, false, idPaquete, idFG);
                        }

                        //Almacenando resultados en sesión
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitImportacion, "TableImportacion");

                        //Borrando archivo de memoria, una vez que se cargó a una tabla
                        Session["ArchivoImportacionTarifa"] = null;
                        //Limpiando nombre de archivo
                        ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");

                        //Llenando gridview de vista previa (Sin llaves de selección)
                        Controles.CargaGridView(gvVistaPrevia, mitImportacion, "IdServicio", lblOrdenarVistaPrevia.Text, true, 1);

                        //Señalando resultado exitoso
                        resultado = new RetornoOperacion("Vista Previa generada con éxito.", true);
                    }
                    //De lo contrario
                    else
                    {
                        //Señalando error
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
        private void actualizarMontosYAgrupar()
        {
            //Validando que existan registros que modificar
            using (DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TableImportacion"))
            {
                //Si hay registros
                if (mit != null)
                {
                    //Obteniendo Validación Global de Preservación de Montos
                    string cat_variable = SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Validación Importador CxC", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                    bool preservaMontos = cat_variable.Equals("1") ? true : false;

                    //Recuperando sólo aquellos elementos que no poseen ningún impedimento de actualización (encontrados por referencia, servicios terminados)
                    IEnumerable<DataRow> servicios = (from DataRow r in mit.Rows
                                                      where r.Field<int>("IdServicio") > 0 && r["FechaFin"] != DBNull.Value
                                                      select r);

                    //Obteniendo agrupación de FG o paquete de revisión activos acorde a solicitud
                    int idPaquete = 0, idFG = 0, idCliente = 0;
                    if (Request.QueryString["idPaqueteRevision"] != null)
                        //Recuperando Id de paquete
                        idPaquete = Convert.ToInt32(Request.QueryString["idPaqueteRevision"]);
                    if (Request.QueryString["idFacturaGlobal"] != null)
                        idFG = Convert.ToInt32(Request.QueryString["idFacturaGlobal"]);

                    //Recuperando el Id de cliente
                    if(idPaquete > 0)
                    {
                        using(FAC.PaqueteProceso pr = new FAC.PaqueteProceso(idPaquete))
                            //Asignando Cliente
                            idCliente = pr.id_cliente;
                    }
                    if (idFG > 0)
                    {
                        using (FAC.FacturaGlobal fg = new FAC.FacturaGlobal(idFG))
                            //Asignando Cliente
                            idCliente = fg.id_compania_cliente;
                    }
                    
                    //Instanciando Facturas Todas
                    using (DataSet ds = FAC.FacturadoConcepto.CargaFacturasTodas(idPaquete != 0 ? idPaquete : idFG, idCliente, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ""))
                    {
                        //Para cada elemento
                        foreach (DataRow r in servicios)
                        {
                            //Inicializando bloque transaccional 
                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Actualización permitida
                                RetornoOperacion resultado = new RetornoOperacion(r.Field<int>("IdServicio"));

                                //Si se requiere facturar y ya se encontraba facturado
                                if (!r.Field<bool>("DisponibleFacturaGlobal") && r.Field<int>("IdFacturaGlobal") > 0)
                                {
                                    //Señalando que no es posible actualizar
                                    resultado = new RetornoOperacion("No es posible añadir a Factura Global, puede ser que no pertenezca al Cliente o que ya se encuentre en otra factura.");
                                }
                                //Si no hay factura electronica
                                else if (!r.Field<bool>("FE"))
                                {
                                    //Validando tanto el estatus del servicio como el estatus de la ultima facturación
                                    using (Servicio srv = new Servicio(r.Field<int>("IdServicio")))
                                    {
                                        using (FAC.Facturado fac = new FAC.Facturado(r.Field<int>("IdFacturado")))
                                        {
                                            //Si el servicio se encuentra documentado, iniciado, cancelado, no facturable o deshabilitado
                                            if ((srv.id_servicio > 0 && fac.id_factura > 0) &&
                                                (srv.estatus != Servicio.Estatus.Terminado || !srv.habilitar ||
                                                fac.estatus == FAC.Facturado.EstatusFactura.Cancelada ||
                                                fac.estatus == FAC.Facturado.EstatusFactura.NoFacturable))
                                            {
                                                //Indicando que no se realizará la actualización
                                                resultado = new RetornoOperacion("El Servicio fue modificado desde la generación de la vista previa. No fue posible editarlo.");
                                            }

                                            //Si no hay errores
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Obteniendo último registro facturado electrónico
                                                //Si este registro es mayor a 0 (cambió desde la generación de vista previa a la fecha)
                                                if (FAC.FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(fac.id_factura) > 0)
                                                {
                                                    //Indicando error
                                                    resultado = new RetornoOperacion("El Servicio fue facturado desde la generación de la vista previa. No fue posible editarlo.");
                                                }
                                            }

                                            //Si no hay errores de validación
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Auxiliar de filtrado de conceptos
                                                IEnumerable<DataRow> conceptosInteres = null;
                                                int idConceptoInteres = 0;
                                                
                                                //Cargando los conceptos asociados a la factura
                                                using (DataTable mitConceptos = FAC.FacturadoConcepto.ObtieneConceptosFactura(fac.id_factura))
                                                {
                                                    //Validando si los Montos actuales se preservaran
                                                    if (!preservaMontos)
                                                    {
                                                        //Validando existencia de conceptos
                                                        if (mitConceptos != null)
                                                            //Filtrando conceptos flete
                                                            conceptosInteres = (from DataRow c in mitConceptos.Rows
                                                                                where c.Field<string>("ConceptoCobro").Contains("FLETE")
                                                                                select c);

                                                        //SI hubo conceptos de interés
                                                        if (conceptosInteres != null)
                                                        {
                                                            //Preservando id de concepto principal
                                                            idConceptoInteres = conceptosInteres.Count() > 0 ? conceptosInteres.FirstOrDefault().Field<int>("Id") : 0;
                                                            //Si hay más de un concepto
                                                            if (conceptosInteres.Count() > 1)
                                                            {
                                                                //Recorriendo lista de conceptos de interés
                                                                foreach (DataRow c in (from DataRow cI in conceptosInteres
                                                                                       where cI.Field<int>("Id") != idConceptoInteres
                                                                                       select cI))
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto flete = new FAC.FacturadoConcepto(c.Field<int>("Id")))
                                                                    {
                                                                        //Se deshabilitan los conceptos involucrados
                                                                        resultado = flete.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                        {
                                                                            //Complementando mensaje de error
                                                                            resultado = new RetornoOperacion(string.Format("Error al deshabilitar fletes duplicados: {0}", resultado.Mensaje));
                                                                            //Saliendo de iteración
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validando si los Montos actuales se preservaran
                                                        if (!preservaMontos)
                                                        {
                                                            /* FLETE */
                                                            //Obteniendo datos requeridos del tipo de concepto
                                                            using (TAR.TipoCargo tcFlete = new TAR.TipoCargo(TAR.TipoCargo.ObtieneTipoCargoDescripcion(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "FLETE")))
                                                            {
                                                                //Si hay registro existente
                                                                if (idConceptoInteres > 0)
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto con = new FAC.FacturadoConcepto(idConceptoInteres))
                                                                    {
                                                                        //Si se recuperó sin problemas y sigue activo
                                                                        if (con.habilitar)
                                                                        {
                                                                            //Si se requiere actualizar
                                                                            if (r.Field<decimal>("Flete") > 0)
                                                                            {
                                                                                //Actualizando monto
                                                                                resultado = con.EditaFacturaConcepto(con.id_factura, 1, tcFlete.id_unidad, "", tcFlete.id_tipo_cargo, r.Field<decimal>("Flete"), r.Field<decimal>("Flete"),
                                                                                                                tcFlete.id_tipo_impuesto_retenido, tcFlete.tasa_impuesto_retenido, tcFlete.id_tipo_impuesto_trasladado, tcFlete.tasa_impuesto_trasladado,
                                                                                                                0, ((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                            //Si se requiere eliminar
                                                                            else if (r.Field<decimal>("Flete") == 0)
                                                                            {
                                                                                //Deshabilitando registro
                                                                                resultado = con.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Si no lo hay y se requiere crearlo
                                                                else if (r.Field<decimal>("Flete") > 0)
                                                                {
                                                                    //Se inserta nuevo concepto con unidad de medida (servicio)
                                                                    resultado = FAC.FacturadoConcepto.InsertaFacturaConcepto(fac.id_factura, 1, tcFlete.id_unidad, "", tcFlete.id_tipo_cargo,
                                                                                                        r.Field<decimal>("Flete"), r.Field<decimal>("Flete"), tcFlete.id_tipo_impuesto_retenido, tcFlete.tasa_impuesto_retenido,
                                                                                                        tcFlete.id_tipo_impuesto_trasladado, tcFlete.tasa_impuesto_trasladado, 0, ((Usuario)Session["usuario"]).id_usuario);
                                                                }

                                                                //Si hubo error
                                                                if (!resultado.OperacionExitosa)
                                                                    //Personalizando error
                                                                    resultado = new RetornoOperacion(resultado.IdRegistro, "Error al actualizar monto de Flete.", resultado.OperacionExitosa);
                                                            }
                                                        }
                                                    }

                                                    //Validando si los Montos actuales se preservaran
                                                    if (!preservaMontos)
                                                    {
                                                        //Reiniciando id de concepto
                                                        idConceptoInteres = 0;
                                                        //Validando existencia de conceptos
                                                        if (mitConceptos != null)
                                                            //Filtrando conceptos Varios
                                                            conceptosInteres = (from DataRow c in mitConceptos.Rows
                                                                                where c.Field<string>("ConceptoCobro").Contains("VARIOS")
                                                                                select c);

                                                        //SI hubo conceptos de interés
                                                        if (conceptosInteres != null)
                                                        {
                                                            //Preservando id de concepto principal
                                                            idConceptoInteres = conceptosInteres.Count() > 0 ? conceptosInteres.FirstOrDefault().Field<int>("Id") : 0;
                                                            //Si hay más de un concepto
                                                            if (conceptosInteres.Count() > 1)
                                                            {
                                                                //Recorriendo lista de conceptos de interés
                                                                foreach (DataRow c in (from DataRow cI in conceptosInteres
                                                                                       where cI.Field<int>("Id") != idConceptoInteres
                                                                                       select cI))
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto varios = new FAC.FacturadoConcepto(c.Field<int>("Id")))
                                                                    {
                                                                        //Se deshabilitan los conceptos involucrados
                                                                        resultado = varios.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                        {
                                                                            //Complementando mensaje de error
                                                                            resultado = new RetornoOperacion(string.Format("Error al deshabilitar varios duplicados: {0}", resultado.Mensaje));
                                                                            //Saliendo de iteración
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validando si los Montos actuales se preservaran
                                                        if (!preservaMontos)
                                                        {
                                                            /* VARIOS */
                                                            //Obteniendo datos requeridos del tipo de concepto
                                                            using (TAR.TipoCargo tcVarios = new TAR.TipoCargo(TAR.TipoCargo.ObtieneTipoCargoDescripcion(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "VARIOS")))
                                                            {
                                                                //Si hay registro existente
                                                                if (idConceptoInteres > 0)
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto con = new FAC.FacturadoConcepto(idConceptoInteres))
                                                                    {
                                                                        //Si se recuperó sin problemas y sigue activo
                                                                        if (con.habilitar)
                                                                        {
                                                                            //Si se requiere actualizar
                                                                            if (r.Field<decimal>("Varios") > 0)
                                                                            {
                                                                                //Actualizando monto
                                                                                resultado = con.EditaFacturaConcepto(con.id_factura, 1, con.id_unidad, "", tcVarios.id_tipo_cargo, r.Field<decimal>("Varios"), r.Field<decimal>("Varios"),
                                                                                                                tcVarios.id_tipo_impuesto_retenido, tcVarios.tasa_impuesto_retenido, tcVarios.id_tipo_impuesto_trasladado, tcVarios.tasa_impuesto_trasladado,
                                                                                                                0, ((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                            //Si se requiere eliminar
                                                                            else if (r.Field<decimal>("Varios") == 0)
                                                                            {
                                                                                //Deshabilitando registro
                                                                                resultado = con.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Si no lo hay y se requiere crearlo
                                                                else if (r.Field<decimal>("Varios") > 0)
                                                                {
                                                                    //Se inserta nuevo concepto con unidad de medida (servicio)
                                                                    resultado = FAC.FacturadoConcepto.InsertaFacturaConcepto(fac.id_factura, 1, tcVarios.id_unidad, "", tcVarios.id_tipo_cargo,
                                                                                                        r.Field<decimal>("Varios"), r.Field<decimal>("Varios"), tcVarios.id_tipo_impuesto_retenido, tcVarios.tasa_impuesto_retenido,
                                                                                                        tcVarios.id_tipo_impuesto_trasladado, tcVarios.tasa_impuesto_trasladado, 0, ((Usuario)Session["usuario"]).id_usuario);
                                                                }

                                                                //Si hubo error
                                                                if (!resultado.OperacionExitosa)
                                                                    //Personalizando error
                                                                    resultado = new RetornoOperacion(resultado.IdRegistro, "Error al actualizar monto de Varios.", resultado.OperacionExitosa);
                                                            }
                                                        }
                                                    }

                                                    //Validando si los Montos actuales se preservaran
                                                    if (!preservaMontos)
                                                    {
                                                        //Reiniciando id de concepto
                                                        idConceptoInteres = 0;

                                                        //Validando existencia de conceptos
                                                        if (mitConceptos != null)
                                                            //Filtrando conceptos Maniobras
                                                            conceptosInteres = (from DataRow c in mitConceptos.Rows
                                                                                where c.Field<string>("ConceptoCobro").Contains("MANIOBRAS")
                                                                                select c);

                                                        if (conceptosInteres != null)
                                                        {
                                                            //Preservando id de concepto principal
                                                            idConceptoInteres = conceptosInteres.Count() > 0 ? conceptosInteres.FirstOrDefault().Field<int>("Id") : 0;
                                                            //Si hay más de un concepto
                                                            if (conceptosInteres.Count() > 1)
                                                            {
                                                                //Recorriendo lista de conceptos de interés
                                                                foreach (DataRow c in (from DataRow cI in conceptosInteres
                                                                                       where cI.Field<int>("Id") != idConceptoInteres
                                                                                       select cI))
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto maniobra = new FAC.FacturadoConcepto(c.Field<int>("Id")))
                                                                    {
                                                                        //Se deshabilitan los conceptos involucrados
                                                                        resultado = maniobra.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                        {
                                                                            //Complementando mensaje de error
                                                                            resultado = new RetornoOperacion(string.Format("Error al deshabilitar maniobras duplicadas: {0}", resultado.Mensaje));
                                                                            //Saliendo de iteración
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validando si los Montos actuales se preservaran
                                                        if (!preservaMontos)
                                                        {
                                                            /* MANIOBRAS */
                                                            //Obteniendo datos requeridos del tipo de concepto
                                                            using (TAR.TipoCargo tcManiobras = new TAR.TipoCargo(TAR.TipoCargo.ObtieneTipoCargoDescripcion(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "MANIOBRAS")))
                                                            {
                                                                //Si hay registro existente
                                                                if (idConceptoInteres > 0)
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto con = new FAC.FacturadoConcepto(idConceptoInteres))
                                                                    {
                                                                        //Si se recuperó sin problemas y sigue activo
                                                                        if (con.habilitar)
                                                                        {
                                                                            //Si se requiere actualizar
                                                                            if (r.Field<decimal>("Maniobras") > 0)
                                                                            {
                                                                                //Actualizando monto (Se utiliza el criterio de retención para el concepto en base a la información del archivo)
                                                                                resultado = con.EditaFacturaConcepto(con.id_factura, 1, con.id_unidad, "", tcManiobras.id_tipo_cargo, r.Field<decimal>("Maniobras"), r.Field<decimal>("Maniobras"),
                                                                                                        r.Field<bool>("MRetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcManiobras.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("MRetencion") ? (decimal)4 : tcManiobras.tasa_impuesto_retenido, tcManiobras.id_tipo_impuesto_trasladado, tcManiobras.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                            //Si se requiere eliminar
                                                                            else if (r.Field<decimal>("Maniobras") == 0)
                                                                            {
                                                                                //Deshabilitando registro
                                                                                resultado = con.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Si no lo hay y se requiere crearlo
                                                                else if (r.Field<decimal>("Maniobras") > 0)
                                                                {
                                                                    //Se inserta nuevo concepto con unidad de medida (servicio)
                                                                    resultado = FAC.FacturadoConcepto.InsertaFacturaConcepto(fac.id_factura, 1, tcManiobras.id_unidad,
                                                                                                        "", tcManiobras.id_tipo_cargo, r.Field<decimal>("Maniobras"), r.Field<decimal>("Maniobras"),
                                                                                                        r.Field<bool>("MRetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcManiobras.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("MRetencion") ? (decimal)4 : tcManiobras.tasa_impuesto_retenido, tcManiobras.id_tipo_impuesto_trasladado, tcManiobras.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                }

                                                                //Si hubo error
                                                                if (!resultado.OperacionExitosa)
                                                                    //Personalizando error
                                                                    resultado = new RetornoOperacion(resultado.IdRegistro, "Error al actualizar monto de Maniobras.", resultado.OperacionExitosa);
                                                            }
                                                        }
                                                    }

                                                    //Validando si los Montos actuales se preservaran
                                                    if (!preservaMontos)
                                                    {
                                                        //Reiniciando id de concepto
                                                        idConceptoInteres = 0;

                                                        //Validando existencia de conceptos
                                                        if (mitConceptos != null)
                                                            //Filtrando conceptos Renta
                                                            conceptosInteres = (from DataRow c in mitConceptos.Rows
                                                                                where c.Field<string>("ConceptoCobro").Contains("RENTA")
                                                                                select c);

                                                        if (conceptosInteres != null)
                                                        {
                                                            //Preservando id de concepto principal
                                                            idConceptoInteres = conceptosInteres.Count() > 0 ? conceptosInteres.FirstOrDefault().Field<int>("Id") : 0;
                                                            //Si hay más de un concepto
                                                            if (conceptosInteres.Count() > 1)
                                                            {
                                                                //Recorriendo lista de conceptos de interés
                                                                foreach (DataRow c in (from DataRow cI in conceptosInteres
                                                                                       where cI.Field<int>("Id") != idConceptoInteres
                                                                                       select cI))
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto renta = new FAC.FacturadoConcepto(c.Field<int>("Id")))
                                                                    {
                                                                        //Se deshabilitan los conceptos involucrados
                                                                        resultado = renta.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                        {
                                                                            //Complementando mensaje de error
                                                                            resultado = new RetornoOperacion(string.Format("Error al deshabilitar maniobras duplicadas: {0}", resultado.Mensaje));
                                                                            //Saliendo de iteración
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validando si los Montos actuales se preservaran
                                                        if (!preservaMontos)
                                                        {
                                                            /* RENTA */
                                                            //Obteniendo datos requeridos del tipo de concepto
                                                            using (TAR.TipoCargo tcRenta = new TAR.TipoCargo(TAR.TipoCargo.ObtieneTipoCargoDescripcion(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "RENTA")))
                                                            {
                                                                //Si hay registro existente
                                                                if (idConceptoInteres > 0)
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto con = new FAC.FacturadoConcepto(idConceptoInteres))
                                                                    {
                                                                        //Si se recuperó sin problemas y sigue activo
                                                                        if (con.habilitar)
                                                                        {
                                                                            //Si se requiere actualizar
                                                                            if (r.Field<decimal>("Renta") > 0)
                                                                            {
                                                                                //Actualizando monto (Se utiliza el criterio de retención para el concepto en base a la información del archivo)
                                                                                resultado = con.EditaFacturaConcepto(con.id_factura, 1, con.id_unidad, "", tcRenta.id_tipo_cargo, r.Field<decimal>("Renta"), r.Field<decimal>("Renta"),
                                                                                                        r.Field<bool>("RRetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcRenta.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("RRetencion") ? (decimal)4 : tcRenta.tasa_impuesto_retenido, tcRenta.id_tipo_impuesto_trasladado, tcRenta.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                            //Si se requiere eliminar
                                                                            else if (r.Field<decimal>("Renta") == 0)
                                                                            {
                                                                                //Deshabilitando registro
                                                                                resultado = con.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Si no lo hay y se requiere crearlo
                                                                else if (r.Field<decimal>("Renta") > 0)
                                                                {
                                                                    //Se inserta nuevo concepto con unidad de medida (servicio)
                                                                    resultado = FAC.FacturadoConcepto.InsertaFacturaConcepto(fac.id_factura, 1, tcRenta.id_unidad,
                                                                                                        "", tcRenta.id_tipo_cargo, r.Field<decimal>("Renta"), r.Field<decimal>("Renta"),
                                                                                                        r.Field<bool>("RRetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcRenta.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("RRetencion") ? (decimal)4 : tcRenta.tasa_impuesto_retenido, tcRenta.id_tipo_impuesto_trasladado, tcRenta.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                }

                                                                //Si hubo error
                                                                if (!resultado.OperacionExitosa)
                                                                    //Personalizando error
                                                                    resultado = new RetornoOperacion(resultado.IdRegistro, "Error al actualizar monto de Maniobras.", resultado.OperacionExitosa);
                                                            }
                                                        }
                                                    }

                                                    //Validando si los Montos actuales se preservaran
                                                    if (!preservaMontos)
                                                    {
                                                        //Reiniciando id de concepto
                                                        idConceptoInteres = 0;

                                                        //Validando existencia de conceptos
                                                        if (mitConceptos != null)
                                                            //Filtrando conceptos Casetas
                                                            conceptosInteres = (from DataRow c in mitConceptos.Rows
                                                                                where c.Field<string>("ConceptoCobro").Contains("CASETAS")
                                                                                select c);

                                                        if (conceptosInteres != null)
                                                        {
                                                            //Preservando id de concepto principal
                                                            idConceptoInteres = conceptosInteres.Count() > 0 ? conceptosInteres.FirstOrDefault().Field<int>("Id") : 0;
                                                            //Si hay más de un concepto
                                                            if (conceptosInteres.Count() > 1)
                                                            {
                                                                //Recorriendo lista de conceptos de interés
                                                                foreach (DataRow c in (from DataRow cI in conceptosInteres
                                                                                       where cI.Field<int>("Id") != idConceptoInteres
                                                                                       select cI))
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto caseta = new FAC.FacturadoConcepto(c.Field<int>("Id")))
                                                                    {
                                                                        //Se deshabilitan los conceptos involucrados
                                                                        resultado = caseta.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                        {
                                                                            //Complementando mensaje de error
                                                                            resultado = new RetornoOperacion(string.Format("Error al deshabilitar casetas duplicadas: {0}", resultado.Mensaje));
                                                                            //Saliendo de iteración
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validando si los Montos actuales se preservaran
                                                        if (!preservaMontos)
                                                        {
                                                            /* CASETAS */
                                                            //Obteniendo datos requeridos del tipo de concepto
                                                            using (TAR.TipoCargo tcCasetas = new TAR.TipoCargo(TAR.TipoCargo.ObtieneTipoCargoDescripcion(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "CASETAS")))
                                                            {
                                                                //Si hay registro existente
                                                                if (idConceptoInteres > 0)
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto con = new FAC.FacturadoConcepto(idConceptoInteres))
                                                                    {
                                                                        //Si se recuperó sin problemas y sigue activo
                                                                        if (con.habilitar)
                                                                        {
                                                                            //Si se requiere actualizar
                                                                            if (r.Field<decimal>("Casetas") > 0)
                                                                            {
                                                                                //Actualizando monto (Se utiliza el criterio de retención para el concepto en base a la información del archivo)
                                                                                resultado = con.EditaFacturaConcepto(con.id_factura, 1, con.id_unidad, "", tcCasetas.id_tipo_cargo, r.Field<decimal>("Casetas"), r.Field<decimal>("Casetas"),
                                                                                                        r.Field<bool>("CRetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcCasetas.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("CRetencion") ? (decimal)4 : tcCasetas.tasa_impuesto_retenido, tcCasetas.id_tipo_impuesto_trasladado, tcCasetas.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                            //Si se requiere eliminar
                                                                            else if (r.Field<decimal>("Casetas") == 0)
                                                                            {
                                                                                //Deshabilitando registro
                                                                                resultado = con.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Si no lo hay y se requiere crearlo
                                                                else if (r.Field<decimal>("Casetas") > 0)
                                                                {
                                                                    //Se inserta nuevo concepto con unidad de medida (servicio)
                                                                    resultado = FAC.FacturadoConcepto.InsertaFacturaConcepto(fac.id_factura, 1, tcCasetas.id_unidad,
                                                                                                        "", tcCasetas.id_tipo_cargo, r.Field<decimal>("Casetas"), r.Field<decimal>("Casetas"),
                                                                                                        r.Field<bool>("CRetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcCasetas.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("CRetencion") ? (decimal)4 : tcCasetas.tasa_impuesto_retenido, tcCasetas.id_tipo_impuesto_trasladado, tcCasetas.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                }

                                                                //Si hubo error
                                                                if (!resultado.OperacionExitosa)
                                                                    //Personalizando error
                                                                    resultado = new RetornoOperacion(resultado.IdRegistro, "Error al actualizar monto de Casetas.", resultado.OperacionExitosa);
                                                            }
                                                        }
                                                    }

                                                    //Validando si los Montos actuales se preservaran
                                                    if (!preservaMontos)
                                                    {
                                                        //Reiniciando id de concepto
                                                        idConceptoInteres = 0;

                                                        //Validando existencia de conceptos
                                                        if (mitConceptos != null)
                                                            //Filtrando conceptos Casetas
                                                            conceptosInteres = (from DataRow c in mitConceptos.Rows
                                                                                where c.Field<string>("ConceptoCobro").Contains("ESTADIAS")
                                                                                select c);

                                                        if (conceptosInteres != null)
                                                        {
                                                            //Preservando id de concepto principal
                                                            idConceptoInteres = conceptosInteres.Count() > 0 ? conceptosInteres.FirstOrDefault().Field<int>("Id") : 0;
                                                            //Si hay más de un concepto
                                                            if (conceptosInteres.Count() > 1)
                                                            {
                                                                //Recorriendo lista de conceptos de interés
                                                                foreach (DataRow c in (from DataRow cI in conceptosInteres
                                                                                       where cI.Field<int>("Id") != idConceptoInteres
                                                                                       select cI))
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto estadia = new FAC.FacturadoConcepto(c.Field<int>("Id")))
                                                                    {
                                                                        //Se deshabilitan los conceptos involucrados
                                                                        resultado = estadia.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                        {
                                                                            //Complementando mensaje de error
                                                                            resultado = new RetornoOperacion(string.Format("Error al deshabilitar casetas duplicadas: {0}", resultado.Mensaje));
                                                                            //Saliendo de iteración
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validando si los Montos actuales se preservaran
                                                        if (!preservaMontos)
                                                        {
                                                            /* ESTADIAS */
                                                            //Obteniendo datos requeridos del tipo de concepto
                                                            using (TAR.TipoCargo tcEstadias = new TAR.TipoCargo(TAR.TipoCargo.ObtieneTipoCargoDescripcion(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "ESTADIAS")))
                                                            {
                                                                //Si hay registro existente
                                                                if (idConceptoInteres > 0)
                                                                {
                                                                    //Instanciando concepto
                                                                    using (FAC.FacturadoConcepto con = new FAC.FacturadoConcepto(idConceptoInteres))
                                                                    {
                                                                        //Si se recuperó sin problemas y sigue activo
                                                                        if (con.habilitar)
                                                                        {
                                                                            //Si se requiere actualizar
                                                                            if (r.Field<decimal>("Estadias") > 0)
                                                                            {
                                                                                //Actualizando monto (Se utiliza el criterio de retención para el concepto en base a la información del archivo)
                                                                                resultado = con.EditaFacturaConcepto(con.id_factura, 1, con.id_unidad, "", tcEstadias.id_tipo_cargo, r.Field<decimal>("Estadias"), r.Field<decimal>("Estadias"),
                                                                                                        r.Field<bool>("ERetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcEstadias.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("ERetencion") ? (decimal)4 : tcEstadias.tasa_impuesto_retenido, tcEstadias.id_tipo_impuesto_trasladado, tcEstadias.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                            //Si se requiere eliminar
                                                                            else if (r.Field<decimal>("Estadias") == 0)
                                                                            {
                                                                                //Deshabilitando registro
                                                                                resultado = con.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Si no lo hay y se requiere crearlo
                                                                else if (r.Field<decimal>("Estadias") > 0)
                                                                {
                                                                    //Se inserta nuevo concepto con unidad de medida (servicio)
                                                                    resultado = FAC.FacturadoConcepto.InsertaFacturaConcepto(fac.id_factura, 1, tcEstadias.id_unidad,
                                                                                                        "", tcEstadias.id_tipo_cargo, r.Field<decimal>("Estadias"), r.Field<decimal>("Estadias"),
                                                                                                        r.Field<bool>("ERetencion") ? Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(94, 1, "IVA")) : tcEstadias.id_tipo_impuesto_retenido,
                                                                                                        r.Field<bool>("ERetencion") ? (decimal)4 : tcEstadias.tasa_impuesto_retenido, tcEstadias.id_tipo_impuesto_trasladado, tcEstadias.tasa_impuesto_trasladado,
                                                                                                        0, ((Usuario)Session["usuario"]).id_usuario);
                                                                }

                                                                //Si hubo error
                                                                if (!resultado.OperacionExitosa)
                                                                    //Personalizando error
                                                                    resultado = new RetornoOperacion(resultado.IdRegistro, "Error al actualizar monto de Estadias.", resultado.OperacionExitosa);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //Si no existen errores de actualización de montos (o validaciones previas)
                                if (resultado.OperacionExitosa)
                                {
                                    //Si se requiere integración a paquete de revisión
                                    if (r.Field<int>("IdPaqueteRevision") > 0)
                                    {
                                        //Instanciando paquete de revisión
                                        using (FAC.PaqueteProceso pr = new FAC.PaqueteProceso(r.Field<int>("IdPaqueteRevision")))
                                        {
                                            //Si su estatus es registrado
                                            if (pr.estatus == FAC.PaqueteProceso.Estatus.Registrado)
                                            {
                                                //Obteniendo Facturas Disponibles
                                                List<DataRow> facturas_disponibles = (from DataRow fd in ds.Tables["Table"].Rows
                                                                                      where fd.Field<string>("NoViaje").Contains(r.Field<string>("Referencia")) 
                                                                                      || fd.Field<string>("NoServicio").Contains(r.Field<string>("NoServicio"))
                                                                                      select fd).ToList();
                                                
                                                //Validando disponibilidad de factura para inclusión en nuevo paquete
                                                if (facturas_disponibles.Count > 0)
                                                
                                                    //Asociando factura a su paquete
                                                    resultado = FAC.PaqueteProcesoDetalle.InsertaPaqueteProcesoDetalle(pr.id_paquete_proceso, r.Field<int>("IdFacturado"), 0,
                                                                                            false, false, DateTime.MinValue, "", ((Usuario)Session["usuario"]).id_usuario);
                                                //Si ya no está disponible
                                                else
                                                    resultado = new RetornoOperacion("Error al integrar servicio a paquete de revisión; el servicio ya no se encuentra disponible.");
                                            }
                                            //Si el paquete ya no está en estatus registrado
                                            else
                                                resultado = new RetornoOperacion("Error al integrar servicio a paquete de revisión; el paquete ya no se encuentra en estatus 'Registrado'.");
                                        }
                                    }
                                    //Si se requiere integrar a Factura Global
                                    else if (r.Field<int>("IdFacturaGlobal") > 0)
                                    {
                                        //Instanciando factura y validando su estatus
                                        using (FAC.FacturaGlobal fg = new FAC.FacturaGlobal(r.Field<int>("IdFacturaGlobal")))
                                        {
                                            //Si está en construcción (Registrada)
                                            if (fg.estatus == FAC.FacturaGlobal.Estatus.Registrada)
                                            {
                                                //Obteniendo Facturas Disponibles
                                                List<DataRow> facturas_todas = (from DataRow fd in ds.Tables["Table"].Rows
                                                                                where fd.Field<string>("NoViaje").Contains(r.Field<string>("Referencia"))
                                                                                || fd.Field<string>("NoServicio").Contains(r.Field<string>("NoServicio"))
                                                                                select fd).ToList();
                                                
                                                //Validando disponibilidad
                                                if (facturas_todas.Count > 0)
                                                
                                                    //Insertando nueva relación de facturado
                                                    resultado = FAC.FacturadoFacturacion.InsertarFacturadoFacturacion(r.Field<int>("IdFacturado"), 0, fg.id_factura_global, 0, 0,
                                                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                else
                                                    resultado = new RetornoOperacion("Error al integrar servicio a factura global; el servicio ya no se encuentra disponible.");
                                            }
                                            else
                                                resultado = new RetornoOperacion("Error al integrar servicio a factura global; la factura ya no se encuentra en estatus 'Registrada'.");
                                        }
                                    }
                                }

                                //Si no hubo errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Confirmando transacción
                                    scope.Complete();
                                }

                                //Mostrando resultado de la transacción
                                ScriptServer.MuestraNotificacion(this, r["Id"].ToString(), string.Format("Serv. {0} [Ref. {1}]: {2}", r["NoServicio"].ToString(), r["Referencia"].ToString(), resultado.Mensaje), resultado.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                    }
                }
                else
                {
                    //Limpiando sesión
                    Session["ArchivoImportacionTarifa"] = null;
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");
                    //Mostrando notificación
                    ScriptServer.MuestraNotificacion(this, "Vuelva a cargar el archivo y presione 'Vista Previa' antes que esta opción.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    //Limpiando nombre de archivo
                    ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");
                }

            }

            //Limpiando sesión
            Session["ArchivoImportacionTarifa"] = null;
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TableImportacion");

            //Borrando contenido de gv
            Controles.InicializaGridview(gvVistaPrevia);
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
                    HttpContext.Current.Session["ArchivoImportacionTarifa"] = array;
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
        
    }
}