using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
namespace SAT.General
{
    public partial class ReporteVencimientos : System.Web.UI.Page
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
                ddlTipo.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }

        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Vencimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void
            gvVencimiento_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvVencimiento, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Vencimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVencimiento_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvVencimiento, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewVencimiento.SelectedValue), true, 1);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Vencimiento a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelVencimiento_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");

        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Vencimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVencimiento_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewVencimiento.Text = Controles.CambiaSortExpressionGridView(gvVencimiento, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }



        /// <summary>
        /// Evento generado al buscar los Vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Carga los Vencimientos
            cargaVencimientos();
        }

        /// <summary>
        /// Evento generado al dar click en la Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvVencimiento.DataKeys.Count > 0)
            {
                //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvVencimiento, sender, "lnk", false);
                //Inicializamos Bitacora
                inicializaBitacora(Convert.ToInt32(gvVencimiento.SelectedValue), 129, "Vencimiento");
                //Carga grafica
                Controles.CargaGrafica(ChtVencimiento, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                      "DiaProximo", "Total", true);
            }
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
            TSDK.ASP.Controles.InicializaGridview(gvVencimiento);
        }
        /// <summary>
        /// 
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "TODOS", 1104);
            //Cargando Catalogo Tipo de Vencimiento
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 60, "TODOS", 0, "", 0, "");
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewVencimiento, "", 26);

        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControles()
        {
            ddlTipo.Text = "0";
            ddlEstatus.SelectedValue = "0";
            txtIdentificador.Text = "";
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/ReporteVencimiento.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de cargar los Vencimientos
        /// </summary>
        private void cargaVencimientos()
        {

            //Obtenemos Vencimientos
            using (DataSet ds = SAT_CL.Global.Reporte.ReporteVencimientos(Convert.ToInt32(ddlTipo.SelectedValue), txtIdentificador.Text, Convert.ToByte(ddlEstatus.SelectedValue)))
            {
                //Cargando los GridView
                Controles.CargaGridView(gvVencimiento, ds.Tables["Table"], "Id", "", true, 1);
                Controles.CargaGridView(gvResumen, ds.Tables["Table1"], "DiaProximo", "", true, 1);
                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table") && Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Añadiendo Tablas al DataSet de Session 
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                    //Carga grafica
                    Controles.CargaGrafica(ChtVencimiento, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                          "DiaProximo", "Total", true);
                    //Calculamos Total
                    gvResumen.FooterRow.Cells[1].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")).ToString();
                }

                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvVencimiento);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        #endregion
    }
}