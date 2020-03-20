using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
using SAT_CL.Seguridad;

namespace SAT.Soporte
{
    public partial class ReporteSoporteTecnico : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Si no es una recarga de página
            if (!this.IsPostBack)
            {
                //Inicializando la forma
                inicializaForma();
                //Asignamos Focus
                txtSolicitante.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }
            //TSDK.ASP.Controles.InicializaGridview(gvSoportes);
        }
        /// <summary>
        /// Evento generado al dar clic en Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Cargando Depositos
            buscaSoporte();
        }
        #region Eventos Soporte

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSoportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Registros           
            Controles.CambiaTamañoPaginaGridView(gvSoportes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoSoportes.SelectedValue), true, 1);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Orden de los Datos del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSoportes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvSoportes.DataKeys.Count > 0)
            {

                //Cambiando Ordenamiento
                lblOrdenarSoportes.Text = Controles.CambiaSortExpressionGridView(gvSoportes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            }
        }

        /// <summary>
        /// Evento Generado al cambiar indice de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSoportes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando paginacion al Grid View
            Controles.CambiaIndicePaginaGridView(gvSoportes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
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
                case "Soportes":
                    //Exporta Grid View
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
                    break;
            }
        }
        #endregion
        #endregion

        #region Metodos
        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga catalogos
            cargaCatalogos();
            //Inicializa controles
            inicializaControles();
            TSDK.ASP.Controles.InicializaGridview(gvSoportes);
        }
        /// <summary>
        /// Método privado de cargar los catalogos de la forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSoportes, "", 26);
            //Cargando Catalogo de Soporte
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSoportes, "--Seleccione--", 3203);
            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConceptoDeposito, 71, "TODOS", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
        }
        /// <summary>
        /// Método privado de inicializar los controles
        /// </summary>
        private void inicializaControles()
        {
            txtSolicitante.Text = "";
            txtObser.Text = "";
            //Inicializando Fechas
            //txtFechaIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            //txtFechaFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }

        private void buscaSoporte()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini = DateTime.MinValue;
            DateTime fec_fin = DateTime.MinValue;
            if (txtFechaIni.Text != "" && txtFechaFin.Text != "")
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFechaIni.Text, out fec_ini);
                DateTime.TryParse(txtFechaFin.Text, out fec_fin);
            }
            //Invoca al dataset para inicializar los valores del gridview si existe en relación a una orden de compra
            using (DataTable dtSoporte = SAT_CL.Soporte.SoporteTecnico.ObtieneSoporte(txtSolicitante.Text, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(ddlSoportes.SelectedValue), fec_ini, fec_fin, txtObser.Text))
            {
                //Valida si existen los datos del datase
                if (Validacion.ValidaOrigenDatos(dtSoporte))
                {
                    //Si existen, carga los valores del datatable al gridview
                    Controles.CargaGridView(gvSoportes, dtSoporte, "Id", "");
                    //Asigna a la variable de sesion los datos del dataset invocando al método AñadeTablaDataSet
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSoporte, "Table");
                }
                //Si no existen
                else
                {
                    //Inicializa el gridView 
                    Controles.InicializaGridview(gvSoportes);
                    //Elimina los datos del dataset si se realizo una consulta anterior
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        #endregion
    }
}