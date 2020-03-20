using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class ReporteBalanceUnidades : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento generado al cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando  la forma
                inicializaForma();
                //Asignamos Focus
                //.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }

        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvUnidades, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 4);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewUnidades.SelectedValue), true, 4);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Unidades a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelUnidades_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");

        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewUnidades.Text = Controles.CambiaSortExpressionGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 4);
        }


        /// <summary>
        /// Evento generado al cambiar de Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvHisOperador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvHisOperador, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, false, 0);
        }

        /// <summary>
        /// Evento generado al buscar las Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Carga las Unidades
            cargaUnidades();
        }

        /// <summary>
        /// Evento generado al dar click en la Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando la fila 
                Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);
                //Inicializamos Bitacora
                inicializaBitacora(Convert.ToInt32(gvUnidades.SelectedValue), 19, "Unidad");
                Controles.CargaGrafica(ChtEstatus, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "Estatus", "Total", true);
                gvResumen.FooterRow.Cells[1].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")).ToString();

            }
        }

        /// Método encargado de dar click en Operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOperador_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);

                //Mostramos ventana 
                alternaVentanaModal(upgvHisOperador, "HistorialOperadores");

                //Carga Operadores
                cargaHistorialOperador();
            }
        }

        /// <summary>
        /// Evento generado al Cerrar eñ Historial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarHistorialOperadores_Click(object sender, EventArgs e)
        {
            //Mostramos ventana 
            alternaVentanaModal(upgvHisOperador, "HistorialOperadores");
        }

        /// <summary>
        /// Evento generado al dar click en la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNumUnidad_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);

                //Inicializando contenido 
                wucBitacoraMonitoreoH.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedValue), true);

                //Mostrando ventana 
                alternaVentanaModal(upgvUnidades, "BitacoraMonitoreoHistorial");
            }
        }

        #endregion

        #region Eventos Control de Usuario

        /// <summary>
        /// Evento click sobre botón nuevo Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreoH_btnNuevoBitacora(object sender, EventArgs e)
        {
            //Abriendo ventana de nuevo registro
            wucBitMonitoreo.InicializaControl(0, Convert.ToInt32(gvUnidades.SelectedDataKey["IdServicio"]), Convert.ToInt32(gvUnidades.SelectedDataKey["IdParada"]),
                Convert.ToInt32(gvUnidades.SelectedDataKey["IdEvento"]), Convert.ToInt32(gvUnidades.SelectedDataKey["NoMovimiento"]), 19, Convert.ToInt32(gvUnidades.SelectedDataKey["IdUnidad"]));
            
            //Cerramos ventana 
            alternaVentanaModal(this, "BitacoraMonitoreoHistorial");

            //Mostramos ventana 
            alternaVentanaModal(this, "BitacoraMonitoreo");
        }

        /// <summary>
        /// Evento generado al Registrar la Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitMonitoreo_ClickRegistrar(object sender, EventArgs e)
        {
            //Realizando guardado de Bitácora Monitoreo
            RetornoOperacion resultado = wucBitMonitoreo.RegistraBitacoraMonitoreo();
            //Sino hay error
            if (resultado.OperacionExitosa)
            {
                //Inicializando contenido 
                wucBitacoraMonitoreoH.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedValue), true);
                //Mostramos ventana 
                alternaVentanaModal(this, "BitacoraMonitoreoHistorial");

                //Cerramos ventana 
                alternaVentanaModal(this, "BitacoraMonitoreo");
            }
        }

        /// <summary>
        /// Evento generado al Eliminar la Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitMonitoreo_ClickEliminar(object sender, EventArgs e)
        {
            //Realizando guardado de Bitácora Monitoreo
            RetornoOperacion resultado = wucBitMonitoreo.DeshabilitaBitacoraMonitoreo();
            //Sino hay error
            if (resultado.OperacionExitosa)
            {
                //Inicializando contenido 
                wucBitacoraMonitoreoH.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedValue), true);
                //Mostramos ventana 
                alternaVentanaModal(this, "BitacoraMonitoreoHistorial");

                //Cerramos ventana 
                alternaVentanaModal(this, "BitacoraMonitoreo");
            }
        }
        /// <summary>
        /// Evento generado al Cerrar la Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarBitMonitoreo_Click(object sender, EventArgs e)
        {
            //Inicializando contenido 
            wucBitacoraMonitoreoH.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedValue), true);
            //Mostramos ventana 
            alternaVentanaModal(lkbCerrarBitMonitoreo, "BitacoraMonitoreoHistorial");

            //Cerramos ventana 
            alternaVentanaModal(lkbCerrarBitMonitoreo, "BitacoraMonitoreo");
        }

        /// <summary>
        /// Evento generado al Cerrar el Historial de Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarBitacoraMonitoreoH_Click(object sender, EventArgs e)
        {
            //Cerramos ventana 
            alternaVentanaModal(lkbCerrarBitacoraMonitoreoH, "BitacoraMonitoreoHistorial");
        }

        /// <summary>
        /// Evento geenrado al dar click el Consultar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreoH_lkbConsultar(object sender, EventArgs e)
        {
            //Inicializando contenido 
            //Abriendo ventana de nuevo registro
            wucBitMonitoreo.InicializaControl(wucBitacoraMonitoreoH.id_bitacora_monitoreo, 0, 0, 0, 0, 19, Convert.ToInt32(gvUnidades.SelectedDataKey["IdUnidad"]));
            
            //Cerramos ventana 
            alternaVentanaModal(this, "BitacoraMonitoreoHistorial");

            //Mostramos ventana 
            alternaVentanaModal(this, "BitacoraMonitoreo");
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga controles
            cargaCatalogos();
            //Inicializa Controles
            inicializaControles();
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvUnidades);
        }
        /// <summary>
        /// 
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {

            //Cargando Catalogos Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstatusUnidad, 65, "TODOS");
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewUnidades, "", 26);
            //Cargando Catalogos Tipo Evento
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "TODOS");

        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControles()
        {
            ddlEstatusUnidad.SelectedValue = "0";
            ddlTipoUnidad.SelectedValue = "0";
            txtNoUnidad.Text = "";
            txtUbicacion.Text = "";
        }

        /// <summary>
        /// Configura y muestra ventana de bitácora de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla (Titulo de bitácora)</param>
        private void inicializaBitacora(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/ReporteEventos.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de cargar las Unidades
        /// </summary>
        private void cargaUnidades()
        {

            //Obtenemos Depósito
            using (DataSet ds = SAT_CL.Despacho.Reporte.BalanceUnidades(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                   Convert.ToByte(ddlEstatusUnidad.SelectedValue),
                                                                   Convert.ToInt32(ddlTipoUnidad.SelectedValue),
                                                                   Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtNoUnidad.Text, ':', 1, "0")),
                                                                    Cadena.VerificaCadenaVacia(txtNoServicio.Text, ""),
                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, ':', 1, "0"))))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table") && Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvUnidades, ds.Tables["Table"], "IdUnidad-IdServicio-NoMovimiento-IdParada", "", true, 4);
                    Controles.CargaGridView(gvResumen, ds.Tables["Table1"], "Estatus", "", false, 0);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                    //Carga grafica
                    Controles.CargaGrafica(ChtEstatus, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                          "Estatus", "Total", true);
                    gvResumen.FooterRow.Cells[1].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")).ToString();

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvUnidades);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }

        }

        
        /// <summary>
        /// Método encargado de cargar los Operadores ligado a una Unidad
        /// </summary>
        private void cargaHistorialOperador()
        {

            //Obtenemos Operadores
            using (DataTable mit = SAT_CL.Global.AsignacionOperadorUnidad.CargaOperadores(Convert.ToInt32(gvUnidades.SelectedDataKey["IdUnidad"])))
            {
               //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvHisOperador, mit, "Id", "", false, 0);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table2");
                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvHisOperador);

                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        /// <summary>
        /// Administra la visualización de ventanas modales en la página (muestra/oculta)
        /// </summary>
        /// <param name="control">Control que afecta a la ventana</param>
        /// <param name="nombre_script_ventana">Nombre del script de la ventana</param>
        private void alternaVentanaModal(Control control, string nombre_script_ventana)
        {
            //Determinando que ventana será afectada (mostrada/ocultada)
            switch (nombre_script_ventana)
            {
                case "BitacoraMonitoreo":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalBitacoraM", "BitacoraM");
                    break;
                case "BitacoraMonitoreoHistorial":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalBitacoraMonitoreoH", "BitacoraMonitoreoH");
                    break;
                    case "HistorialOperadores":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalHistorialOp", "HistorialOp");
                    break;
            }
        }

        #endregion

    }
}