using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Documentacion
{
    public partial class VisorDocumentacion : System.Web.UI.Page
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
            if (!IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Cambio se selección para uso de filtro de fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Si se ha marcado el filtro
            if (chkRangoFechas.Checked)
            {
                //Habilitando controles de filtrado
                rdbCitaCarga.Enabled =
                rdbCitaDescarga.Enabled =
                rdbInicioServicio.Enabled =
                rdbFinServicio.Enabled =
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = true;

                //Colocando fechas predeterminadas (una semana)
                txtFechaFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(1).AddMinutes(-1).ToString("dd/MM/yyyy HH:mm");
                txtFechaInicio.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            }
            //Si no se ha marcado
            else
            {
                //Deshabilitando controles de filtrado
                rdbCitaCarga.Enabled =
                rdbCitaDescarga.Enabled =
                rdbInicioServicio.Enabled =
                rdbFinServicio.Enabled =
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = false;

                //Limpiando fechas
                txtFechaInicio.Text = txtFechaFin.Text = "";
            }
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferencias_Click(object sender, EventArgs e)
        {
            //Si hay registros servicio mostrados
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccioanndo fila actual
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

                //Referencias de Viaje
                wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["Id"]));

                //ALternando Ventana
                ScriptServer.AlternarVentana(this.Page, "ReferenciasViaje", "contenedorVentanaReferenciaViaje", "ventanaReferenciaViaje");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //ALternando Ventana
            ScriptServer.AlternarVentana(this.Page, "ReferenciasViaje", "contenedorVentanaReferenciaViaje", "ventanaReferenciaViaje");
            //Recargando Grid
            buscaServicios();
        }
        /// <summary>
        /// Evento Producido al Guardar una Referencia del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Guardando Referencia
            resultado = wucReferenciaViaje.GuardaReferenciaViaje();

            //Validamos Resultado
            if (resultado.OperacionExitosa)

                //Recargando Servicios
                buscaServicios();
        }
        /// <summary>
        /// Evento Producido al Eliminar una Referencia del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Eliminando Referencia
            resultado = wucReferenciaViaje.EliminaReferenciaViaje();
            
            //Validamos Resultado
            if (resultado.OperacionExitosa)
                
                //Recargando Servicios
                buscaServicios();
        }

        /// <summary>
        /// Cambio de tamaño de página de gridview de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewServicios.SelectedValue), true, 3);
        }
        /// <summary>
        /// Exportación de de contenido de gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServicios_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "*".ToArray());
        }
        /// <summary>
        /// Cambio de página en gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Cambio de criterio de orden en gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewServicios.Text = Controles.CambiaSortExpressionGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Click en botón bitácora de servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Si hay registros servicio mostrados
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccioanndo fila actual
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

                //Construyendo URL 
                string url = Cadena.RutaRelativaAAbsoluta("~/Documentacion/ReporteServicios.aspx", string.Format("~/Accesorios/BitacoraRegistro.aspx?idT={0}&idR={1}&tB={2}", 1, gvServicios.SelectedDataKey.Value, "Servicio"));
                //Definiendo Configuracion de la Ventana
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                //Abriendo Nueva Ventana
                ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la página
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catálogos requeridos
            cargaCatalogos();
            //Inicializando GridView de servicios
            Controles.InicializaGridview(gvServicios);
        }
        /// <summary>
        /// Realiza la carga de los catálogos utilizados en la forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando estatus de servicio más la opción todos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "Todos", 6);
            //Región del servicio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRegion, 3, "Todos", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 2, "");
            //Tipo de Servicio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 3, "Todos", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 4, "");
            //Alcance de Servicio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlAlcance, 3, "Todos", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 5, "");
            //Tamaño de Gridview (5-25)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewServicios, "", 26);
        }
        /// <summary>
        /// Realiza la búsqueda de servicios coincidentes a los filtros señalados
        /// </summary>
        private void buscaServicios()
        {
            //Declarando variables para rangos de fecha
            DateTime inicial_cita_carga = DateTime.MinValue;
            DateTime final_cita_carga = DateTime.MinValue;
            DateTime inicial_cita_descarga = DateTime.MinValue;
            DateTime final_cita_descarga = DateTime.MinValue;
            DateTime inicial_inicio_servicio = DateTime.MinValue;
            DateTime final_inicio_servicio = DateTime.MinValue;
            DateTime inicial_fin_servicio = DateTime.MinValue;
            DateTime final_fin_servicio = DateTime.MinValue;

            //Determinando que criterio será utilizado
            if (chkRangoFechas.Checked)
            {
                //Cita carga
                if (rdbCitaCarga.Checked)
                {
                    inicial_cita_carga = Convert.ToDateTime(txtFechaInicio.Text);
                    final_cita_carga = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Cita Descarga
                else if (rdbCitaDescarga.Checked)
                {
                    inicial_cita_descarga = Convert.ToDateTime(txtFechaInicio.Text);
                    final_cita_descarga = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Inicio de Servicio
                else if (rdbInicioServicio.Checked)
                {
                    inicial_inicio_servicio = Convert.ToDateTime(txtFechaInicio.Text);
                    final_inicio_servicio = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Fin de Servicio
                else
                {
                    inicial_fin_servicio = Convert.ToDateTime(txtFechaInicio.Text);
                    final_fin_servicio = Convert.ToDateTime(txtFechaFin.Text);
                }
            }

            //Realizando la carga de los servicios coincidentes
            using (DataTable mit = SAT_CL.Documentacion.Reportes.CargaReporteServicios(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoServicio.Text, Convert.ToInt32(ddlEstatus.SelectedValue),
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOrigen.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDestino.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                txtPorte.Text, txtReferencia.Text, Convert.ToInt32(ddlRegion.SelectedValue), Convert.ToInt32(ddlTipoServicio.SelectedValue), Convert.ToInt32(ddlAlcance.SelectedValue), inicial_cita_carga, final_cita_carga, inicial_cita_descarga, final_cita_descarga,
                inicial_inicio_servicio, final_inicio_servicio, inicial_fin_servicio, final_fin_servicio))
            {
                //Cargando Gridview
                Controles.CargaGridView(gvServicios, mit, "Id", lblCriterioGridViewServicios.Text, true, 3);

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