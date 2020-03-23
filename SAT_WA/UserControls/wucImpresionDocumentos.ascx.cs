using SAT_CL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucImpresionDocumentos : System.Web.UI.UserControl
    {
        #region Atributos
        /// <summary>
        /// Id de Compania a consultar
        /// </summary>
        private int _idCompania;
        /// <summary>
        /// Id de Compania a consultar
        /// </summary>
        private int _idUsuario;
        /// <summary>
        /// DataSet con los registros a mostrar
        /// </summary>
        private DataSet _ds;
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {//Validando si se Produjo un PostBack
            if (!(Page.IsPostBack))
                //Invocando  Método de Asignación
                asignaAtributos();
            else//Invocando  Método de Recuperación
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al dar click en boton Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarModal_Click(object sender, EventArgs e)
        {
            ScriptServer.AlternarVentana(lkbCerrarModal, "impresionPorte", "contenedorVentanaImpresionPorte", "ventanaImpresionPorte");
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {//Validando si existen Registros
            if (gvServicios.DataKeys.Count > 0)
                //Cambiando Tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {//Validando si existen Registros
            if (gvServicios.DataKeys.Count > 0)
                //Cambiando Indice de Página del GridView
                Controles.CambiaIndicePaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {//Validando si existen Registros
            if (gvServicios.DataKeys.Count > 0)
                //Cambiando Indice de Página del GridView
                lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido a Presionar el LinkButton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando si existen Registros
            if (gvServicios.DataKeys.Count > 0)
                //Exportando Contenido del GridView
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id", "IdRegistro");
        }
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando si existen Registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Validar el tipo de fila
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    using (ImageButton imbPorte = (ImageButton)e.Row.FindControl("imbPorte"),
                                   imbViajera = (ImageButton)e.Row.FindControl("imbViajera"),
                                   imbInstruccion = (ImageButton)e.Row.FindControl("imbInstruccion"),
                                   imbGastos = (ImageButton)e.Row.FindControl("imbGastos"))
                    {
                        //Recuperar informacion de la fila actual
                        DataRow fila = ((DataRowView)e.Row.DataItem).Row;
                        if (fila["ImpresionCP"].ToString() == "1")
                        {
                            imbPorte.Visible = false;
                        }
                        if (fila["ImpresionCPV"].ToString() == "1")
                        {
                            imbViajera.Visible = false;
                        }
                        if (fila["ImpresionHI"].ToString() == "1")
                        {
                            imbInstruccion.Visible = false;
                        }
                        if (fila["ImpresionGastos"].ToString() == "1")
                        {
                            imbGastos.Visible = false;
                        }
                    }
                }
            }
        }
        protected void lkbCalculaTarifa_Click(object sender, EventArgs e)
        {
            //Validamos que existan Servicios
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);
                //Aplicando tarifa
                aplicaTarifaServicio();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbImprimir_Click(object sender, ImageClickEventArgs e)
        {
            //Recuperando el botón pulsado
            ImageButton imb = (ImageButton)sender;
            //Validamos que existan Servicios
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvServicios, sender, "imb", false);
                //En base al comando definido para el botón
                switch (imb.CommandName)
                {
                    case "Porte":
                        {
                            //Invocando Método de Validación en la Carta Porte
                            if (SAT_CL.Documentacion.Servicio.ValidaImpresionEspecificaCartaPorte(Convert.ToInt32(gvServicios.SelectedValue)).OperacionExitosa)
                            {
                                //Mostramos Modal
                                ScriptServer.AlternarVentana(gvServicios, "impresionPorte", "contenedorVentanaImpresionPorte", "ventanaImpresionPorte");
                                //Inicializando control
                                wucImpresionPorte.InicializaImpresionCartaPorte(Convert.ToInt32(gvServicios.SelectedValue), UserControls.wucImpresionPorte.TipoImpresion.CartaPorte);
                                SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(gvServicios.SelectedValue), 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Conteo Impresión CP", 0, "General"), "1", Fecha.ObtieneFechaEstandarMexicoCentro(), this._idUsuario);
                            }
                            else
                            {
                                //Construyendo URL
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Accesorios/ImpresionDocumentos.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Porte", Convert.ToInt32(gvServicios.SelectedValue)), "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(gvServicios.SelectedValue), 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Conteo Impresión CP", 0, "General"), "1", Fecha.ObtieneFechaEstandarMexicoCentro(), this._idUsuario);
                            }
                            break;
                        }
                    case "Viajera":
                        {
                            //Validamos Selección
                            if (gvServicios.SelectedIndex != -1)
                            {
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Accesorios/ImpresionDocumentos.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "PorteViajera", Convert.ToInt32(gvServicios.SelectedValue)), "PorteViajera", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(gvServicios.SelectedValue), 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Conteo Impresión CPV", 0, "General"), "1", Fecha.ObtieneFechaEstandarMexicoCentro(), this._idUsuario);
                            }
                            break;
                        }
                    case "Instruccion":
                        {
                            //Validamos Selección
                            if (gvServicios.SelectedIndex != -1)
                            {
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Accesorios/ImpresionDocumentos.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "HojaDeInstruccion", Convert.ToInt32(gvServicios.SelectedValue)), "HojaDeInstruccion", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(gvServicios.SelectedValue), 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Conteo Impresión HI", 0, "General"), "1", Fecha.ObtieneFechaEstandarMexicoCentro(), this._idUsuario);
                            }
                            break;
                        }
                    case "Gastos":
                        {
                            //Validamos Selección
                            if (gvServicios.SelectedIndex != -1)
                            {
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Accesorios/ImpresionDocumentos.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "GastosGenerales", Convert.ToInt32(gvServicios.SelectedValue)), "GastosGenerales", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(gvServicios.SelectedValue), 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Conteo Impresión Gastos", 0, "General"), "1", Fecha.ObtieneFechaEstandarMexicoCentro(), this._idUsuario);
                            }
                            break;
                        }
                }
                cargaServicios();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargaServicios();
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {   //Asignando Valores
            ViewState["idCompania"] = this._idCompania;
            ViewState["idUsuario"] = this._idUsuario;
            ViewState["DS"] = this._ds;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   //Recuperando Valores
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)
                this._idCompania = Convert.ToInt32(ViewState["idCompania"]);
            if (Convert.ToInt32(ViewState["idUsuario"]) != 0)
                this._idUsuario = Convert.ToInt32(ViewState["idUsuario"]);
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)ViewState["DS"], "Table"))
                this._ds = (DataSet)ViewState["DS"];
        }
        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaControl()
        {
            Controles.InicializaGridview(gvServicios);
            //Inicializando Fechas
            txtFechaInicio.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            chkIncluir.Checked = true;
            //Cargando Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxOperacionServicio, 3, "TODOS", this._idCompania, "", 6, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxAlcance, 3, "TODOS", this._idCompania, "", 5, "");
        }
        /// <summary>
        /// Realiza la búsqueda y muestra los servicios activos
        /// </summary>
        private void cargaServicios()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_cita_carga = DateTime.MinValue, fec_fin_cita_carga = DateTime.MinValue,
                     fec_ini_cita_descarga = DateTime.MinValue, fec_fin_cita_descarga = DateTime.MinValue,
                     fec_ini_ini_viaje = DateTime.MinValue, fec_fin_ini_viaje = DateTime.MinValue,
                     fec_ini_fin_viaje = DateTime.MinValue, fec_fin_fin_viaje = DateTime.MinValue;

            //Validando si se Requieren las Fechas
            if (chkIncluir.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rbCarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFechaInicio.Text, out fec_ini_cita_carga);
                    DateTime.TryParse(txtFechaFin.Text, out fec_fin_cita_carga);
                }
                else if (rbDescarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFechaInicio.Text, out fec_ini_cita_descarga);
                    DateTime.TryParse(txtFechaFin.Text, out fec_fin_cita_descarga);
                }
                else if (rbInicioViaje.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFechaInicio.Text, out fec_ini_ini_viaje);
                    DateTime.TryParse(txtFechaFin.Text, out fec_fin_ini_viaje);
                }
                else if (rbFinViaje.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFechaInicio.Text, out fec_ini_fin_viaje);
                    DateTime.TryParse(txtFechaFin.Text, out fec_fin_fin_viaje);
                }
            }

            //Obteniendo Filtros de Clasificación
            string operacion = Controles.RegresaSelectedValuesListBox(lbxOperacionServicio, "{0},", true, false);
            string alcance = Controles.RegresaSelectedValuesListBox(lbxAlcance, "{0},", true, false);

            //Cargando Servicio
            using (DataTable mit = SAT_CL.Despacho.Reporte.CargaServiciosImprimir(this._idCompania, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), 
                txtServicio.Text, chkDocumentados.Checked, chkIniciados.Checked, operacion.Length > 1 ? operacion.Substring(0, operacion.Length - 1) : "",
                alcance.Length > 1 ? alcance.Substring(0, alcance.Length - 1) : "", fec_ini_cita_carga, fec_fin_cita_carga, fec_ini_cita_descarga, fec_fin_cita_descarga, fec_ini_ini_viaje,
                fec_ini_fin_viaje, fec_fin_ini_viaje, fec_fin_fin_viaje, txtViaje.Text, Cadena.RegresaCadenaSeparada(txtOperador.Text, " ID:", 1), Cadena.RegresaCadenaSeparada(txtUnidad.Text, " ID:", 1)))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Obtenemos Arreglos de Id
                    int[] id = (from DataRow r in mit.Rows
                                select r.Field<int>("id_servicio")).ToArray();

                    //Cargamos Publicaciones Activas de Nuestros Servicios
                    //cargaPublicacionesActivasServicios(id);
                }

                Controles.CargaGridView(gvServicios, mit, "id_servicio-movimiento-id_parada_actual-IdParadaInicial-NoServicio-ImpresionCP-ImpresionCPV-ImpresionHI-ImpresionGastos", lblOrdenado.Text, true, 4);
                //Guardando en sesión el origen de datos
                if (mit != null)
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                else
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Quitando selecciones de fila existentes
            Controles.InicializaIndices(gvServicios);
        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="id_tabla"></param>
        /// <param name="nombre_tabla"></param>
        public void InicializaControlUsuario(int idCompania, int idUsuario)
        {   //Asignando Id de registro
            this._idCompania = idCompania;
            this._idUsuario = idUsuario;
            //Inicializamos el control
            inicializaControl();
        }
        /// <summary>
        /// 
        /// </summary>
        private void aplicaTarifaServicio()
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando factura del servicio seleccionado
            using (SAT_CL.Facturacion.Facturado objFacturado = SAT_CL.Facturacion.Facturado.ObtieneFacturaServicio(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"])))
            {
                //Validando que exista un Id de factura
                if (objFacturado.id_factura != 0)
                    //Obteniendo Resultado de la Edición
                    resultado = objFacturado.ActualizaTarifa(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    resultado = new RetornoOperacion("El servicio no tiene un encabezado de factura o no pudo ser recuperado.");
            }

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {   
                //Actualizando listado de servicios
                cargaServicios();
                Controles.InicializaIndices(gvServicios);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Imprimir el la Carta Porte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucImpresionPorte_ClickImprimirCartaPorte(object sender, EventArgs e)
        {
            //Invocando Método de Impresión
            wucImpresionPorte.ImprimeCartaPorte();
        }
        #endregion        
    }
}