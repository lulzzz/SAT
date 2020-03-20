using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucHistorialViajes : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_compania;
        private DataTable _dt;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                txtNoServicio.TabIndex = 
                txtCliente.TabIndex =
                ddlEstatus.TabIndex =
                txtReferencia.TabIndex =
                chkCitaCarga.TabIndex =
                chkCitaDescarga.TabIndex =
                chkDocumentacion.TabIndex =
                txtFecIni.TabIndex =
                txtFecFin.TabIndex =
                btnBuscar.TabIndex =
                ddlTamano.TabIndex =
                lnkExportar.TabIndex =
                gvViajes.TabIndex = value;
            }
            get { return txtNoServicio.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor de habilitación de los controles contenidos
        /// </summary>
        public bool Enabled
        {
            set
            {
                //Asignando Habilitación
                txtNoServicio.Enabled =
                txtCliente.Enabled =
                ddlEstatus.Enabled =
                txtReferencia.Enabled =
                chkCitaCarga.Enabled =
                chkCitaDescarga.Enabled =
                chkDocumentacion.Enabled =
                txtFecIni.Enabled =
                txtFecFin.Enabled =
                btnBuscar.Enabled =
                ddlTamano.Enabled =
                lnkExportar.Enabled =
                gvViajes.Enabled = value;
            }
            get { return txtNoServicio.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Generarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se produzca un PostBack
            if (!(Page.IsPostBack))
            {
                //Cargando Catalogos
                cargaCatalogos();
                
                //Asignando Atributos
                asignaAtributos();
            }
            else
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al Buscar el Historial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaViajes();
        }

        #region Eventos GridView "Viajes"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvViajes.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._dt.DefaultView.Sort = lblOrdenado.Text;

                //Cambiando tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvViajes, this._dt, Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
            }
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(this._dt, "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existan Registros
            if (gvViajes.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._dt.DefaultView.Sort = lblOrdenado.Text;

                //Cambiando Expresión del Ordenamiento
                lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvViajes, this._dt, e.SortExpression, true, 2);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existan Registros
            if (gvViajes.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._dt.DefaultView.Sort = lblOrdenado.Text;

                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvViajes, this._dt, e.NewPageIndex, true, 2);
            }
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        private void inicializaControl()
        {
            //Asignando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Inicializando GridView
            Controles.InicializaGridview(gvViajes);
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando estatus de servicio más la opción todos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "Todos", 6);

            //Tamaño de Gridview (25-100)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {   
            //Asignando Atributos
            ViewState["idCompania"] = this._id_compania;
            ViewState["DT"] = this._dt;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {   
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)
                this._id_compania = Convert.ToInt32(ViewState["idCompania"]);
            
            //Validando que existan Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                this._dt = (DataTable)ViewState["DT"];
        }
        /// <summary>
        /// Método encargado de Buscar los Viajes
        /// </summary>
        private void buscaViajes()
        {
            //Declarando Variables de Fecha
            DateTime cita_carga_ini, cita_carga_fin, cita_descarga_ini, cita_descarga_fin, fec_doc_ini, fec_doc_fin;
            cita_carga_ini = cita_carga_fin = cita_descarga_ini = cita_descarga_fin = fec_doc_ini = fec_doc_fin = DateTime.MinValue;
            
            //Validando Fechas Solicitadas
            if (chkCitaCarga.Checked)
            {
                //Obteniendo Fecha
                DateTime.TryParse(txtFecIni.Text, out cita_carga_ini);
                DateTime.TryParse(txtFecFin.Text, out cita_carga_fin);
            }
            if (chkCitaDescarga.Checked)
            {
                //Obteniendo Fecha
                DateTime.TryParse(txtFecIni.Text, out cita_descarga_ini);
                DateTime.TryParse(txtFecFin.Text, out cita_descarga_fin);
            }
            if (chkDocumentacion.Checked)
            {
                //Obteniendo Fecha
                DateTime.TryParse(txtFecIni.Text, out fec_doc_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_doc_fin);
            }
            
            
            //Obteniendo Reporte
            using (DataTable dtServicios = SAT_CL.Despacho.Reporte.CargaHistorialServicios(this._id_compania, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, "0")), txtNoServicio.Text, txtCartaPorte.Text,
                                                            Convert.ToInt32(ddlEstatus.SelectedValue), txtReferencia.Text, cita_carga_ini, cita_carga_fin, cita_descarga_ini, cita_descarga_fin, fec_doc_ini, fec_doc_fin))
            {
                //Validando que existan registros
                if (Validacion.ValidaOrigenDatos(dtServicios))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvViajes, dtServicios, "id_servicio", lblOrdenado.Text, true, 2);

                    //Añadiendo Tabla a ViewState
                    this._dt = dtServicios;
                }
                else
                {
                    //Cargando GridView
                    Controles.InicializaGridview(gvViajes);

                    //Añadiendo Tabla a ViewState
                    this._dt = null;
                }
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicialziar el Control
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        public void InicializaHistorialViajes(int id_compania)
        {
            //Asignando Atributo
            this._id_compania = id_compania;

            //Inicializando Control
            inicializaControl();
        }

        #endregion
    }
}