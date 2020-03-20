using SAT_CL;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.EgresoServicio
{
    public partial class DepositosRechazados : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es un arecarga de página
            if (!IsPostBack)
                //Inicializando forma
                inicializaForma();
        }        
        /// <summary>
        /// Tamaño por página del gv de rechazos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDepositos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el tamaño del gv de rechazos
            Controles.CambiaTamañoPaginaGridView(gvDepositosRechazados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoDepositos.SelectedValue), true, 3);
        }
        /// <summary>
        /// Cambio de página en gv de rechazos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositosRechazados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvDepositosRechazados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Cambio de criterio de ordenamiento en gv de rechazos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositosRechazados_Sorting(object sender, GridViewSortEventArgs e)
        {
           lblCriterioGridDepositos.Text = Controles.CambiaSortExpressionGridView(gvDepositosRechazados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Click en botón buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarDepositosRechazados_Click(object sender, EventArgs e)
        {
            //Cargando el reporte solicitado
            cargaDepositosRechazados();
        }
        /// <summary>
        /// Click en botón de exportación de rechazos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcelExportarDepositosRechazados_Click(object sender, EventArgs e)
        {
            //Exportando a excel el contenido del gv
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Inicializndo contenido de forma
            cargaCatalogos();

            //Cargando rango de fechas inicial
            txtFechaSolicitudI.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFechaSolicitudF.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(1).AddMinutes(-1).ToString("dd/MM/yyyy HH:mm");

            //Asignando criterios iniciales de búsqueda (criterios pasados por url)
            //Tipo de Depósito
            if (Request.QueryString["idTipoDeposito"] != null)
                ddlTipoDeposito.SelectedValue = Request.QueryString["idTipoDeposito"].ToString();
            //Operador
            if (Request.QueryString["idOperador"] != null)
            {
                using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(Convert.ToInt32(Request.QueryString["idOperador"])))
                    txtOperador.Text = string.Format("{0}   ID:{1}", op.nombre, op.id_operador);
            }
            //Unidad
            if (Request.QueryString["idUnidad"] != null)
            {
                using (SAT_CL.Global.Unidad un = new SAT_CL.Global.Unidad(Convert.ToInt32(Request.QueryString["idUnidad"])))
                    txtUnidad.Text = string.Format("{0}   ID:{1}", un.numero_unidad, un.id_unidad);
            }
            //Proveedor
            if (Request.QueryString["idProveedor"] != null)
            {
                using (SAT_CL.Global.CompaniaEmisorReceptor com = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Request.QueryString["idProveedor"])))
                    txtProveedor.Text = string.Format("{0}   ID:{1}", com.nombre, com.id_compania_emisor_receptor);
            }

            //Inicializando el contenido del GV
            Controles.InicializaGridview(gvDepositosRechazados);

        }
        /// <summary>
        /// Carga el conjunto de catálogos necesarios
        /// </summary>
        private void cargaCatalogos()
        { 
            //Carga de tipos de operación
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoDeposito, 91, "Todos");
            //Tamaño de gv de rechazos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDepositos, "", 26);
        }
        /// <summary>
        /// Carga el conjunto de registros solicitado por los filtros
        /// </summary>
        private void cargaDepositosRechazados()
        { 
            DateTime fecha_inicial, fecha_final;
            DateTime.TryParse(txtFechaSolicitudI.Text, out fecha_inicial);
            DateTime.TryParse(txtFechaSolicitudF.Text, out fecha_final);

            //Cargando el conjunto de rechazos
            using (DataTable mit = SAT_CL.Bancos.Reporte.ObtieneReporteDepositosRechazados(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToByte(ddlTipoDeposito.SelectedValue),
                                fecha_inicial, fecha_final, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1)),
                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1))))
            { 
                //Inicializando indices de selección
                Controles.InicializaIndices(gvDepositosRechazados);
                //Cargando gv
                Controles.CargaGridView(gvDepositosRechazados, mit, "Id-IdTabla-IdRegistro", lblCriterioGridDepositos.Text, true, 3);

                //Si hay registros devueltos
                if (mit != null)
                    //Guardando en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                //Si no hay registros
                else
                    //Borrando el origen de datos anterior
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }
        }

        #endregion
                
    }
}