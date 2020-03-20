using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;

namespace SAT.FacturacionElectronica
{
    public partial class ReporteIngresoServicioPeriodo : System.Web.UI.Page
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
            if (!IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Click en botón buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscaServicios();
        }
        /// <summary>
        /// Cambio de tamaño de página de gridview de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewIngreso_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewIngreso.SelectedValue), true, 0);
        }
        /// <summary>
        /// Exportación de de contenido de gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarIngreso_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "*".ToArray());
        }
        /// <summary>
        /// Cambio de página en gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvIngreso_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 0);
        }
        /// <summary>
        /// Cambio de criterio de orden en gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvIngreso_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewIngreso.Text = Controles.CambiaSortExpressionGridView(gvIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 0);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la página
        /// </summary>
        private void inicializaForma()
        {
            //Tamaño de Gridview (5-25)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewIngreso, "", 18);
            //Cargando catálogo de agrupación
            ListItemCollection elementos = new ListItemCollection();
            elementos.Add(new ListItem("Días", "DIA"));
            elementos.Add(new ListItem("Semana", "SEMANA"));
            elementos.Add(new ListItem("Meses", "MES"));
            elementos.Add(new ListItem("Años", "ANNO"));
            TSDK.ASP.Controles.CargaDropDownList(ddlAgrupador, elementos);

            //Inicializando GridView de servicios
            Controles.InicializaGridview(gvIngreso);
        }
        /// <summary>
        /// Realiza la búsqueda de servicios coincidentes a los filtros señalados para mostrar ingreso
        /// </summary>
        private void buscaServicios()
        {
            //Realizando la carga de los servicios coincidentes
            using (DataTable mit = SAT_CL.Facturacion.Reporte.CargaReporteIngresoServiciosPeriodo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text), ddlAgrupador.SelectedValue))
            {
                //Cargando Gridview
                Controles.CargaGridView(gvIngreso, mit, "Agrupador", lblCriterioGridViewIngreso.Text, true, 3);

                //Si no hay registros
                if (mit == null)
                    //Elimiando de sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                //Si existen registros, se sobrescribe
                else
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
            }
        }

        #endregion
    }
}