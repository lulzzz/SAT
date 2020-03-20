using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Almacen
{
    public partial class RequisicionesPendientes : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Buscando Requisiciones
            buscarRequisicionesPendientes();
        }

        #region Eventos GridView "Requisiciones Pendientes"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvRequisicionesPendiente, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionesPendiente_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvRequisicionesPendiente, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionesPendiente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvRequisicionesPendiente, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Dar Click en las Opciones de los Productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbProductos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvRequisicionesPendiente.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;

                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisicionesPendiente, sender, "lnk", false);

                //Validando Comando
                switch(lnk.CommandName)
                {
                    case "Disponibles":
                        {
                            //Asignando Requisición a Sesión
                            Session["id_registro"] = Convert.ToInt32(gvRequisicionesPendiente.SelectedDataKey["Id"]);

                            //Cargando Ruta de Destino
                            string url = Cadena.RutaRelativaAAbsoluta("~/Almacen/RequisicionesPendientes.aspx", "~/Almacen/RequisicionSurtido.aspx");

                            //Redireccionando a la Nueva Forma
                            Response.Redirect(url);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCancelar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvRequisicionesPendiente.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisicionesPendiente, sender, "lnk", false);

                //Instanciando Requisición
                using (SAT_CL.Almacen.Requisicion requision = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisicionesPendiente.SelectedDataKey["Id"])))
                {
                    //Validando que exista la Requisición
                    if (requision.habilitar)
                    {
                        //Validando que la Requisición este Solicitada
                        if(requision.estatus == SAT_CL.Almacen.Requisicion.Estatus.Solicitada)
                        
                            //Actualizando Estatus de la Requisición
                            result = requision.ActualizaEstatusRequisicionDetalles(SAT_CL.Almacen.Requisicion.Estatus.Cancelada, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                        {
                            //Validando Estatus
                            switch(requision.estatus)
                            {
                                case SAT_CL.Almacen.Requisicion.Estatus.AbastecidaParcial:
                                    {
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("La Requisición se encuentra Abastecida Parcialmente, Imposible su Cancelación");
                                        break;
                                    }
                                case SAT_CL.Almacen.Requisicion.Estatus.Cerrada:
                                    {
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("La Requisición se encuentra Cerrada, Imposible su Cancelación");
                                        break;
                                    }
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se encuentra la Requisición");

                    //Validando Operación
                    if (result.OperacionExitosa)

                        //Recargando requisiciones
                        buscarRequisicionesPendientes();

                    //Inicializando Indices
                    Controles.InicializaIndices(gvRequisicionesPendiente);

                    //Mostrando Mensaje de la Operación
                    ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializando Fechas
            txtFecIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Inicializando GridView
            Controles.InicializaGridview(gvRequisicionesPendiente);

            //Cargando Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "TODOS", 2126);
        }
        /// <summary>
        /// Método encargado de Buscar las Requisiciones Pendientes
        /// </summary>
        private void buscarRequisicionesPendientes()
        {   
            //Declarando Variables Auxiliares
            DateTime fec_ini_sol, fec_fin_sol, fec_ini_ent, fec_fin_ent;
            fec_ini_sol = fec_fin_sol = fec_ini_ent = fec_fin_ent = DateTime.MinValue;

            //Validando que se encuentre 
            if (chkSolicitud.Checked)
            {
                //Asignando Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini_sol);
                DateTime.TryParse(txtFecFin.Text, out fec_fin_sol);
            }
            if (chkEntrega.Checked)
            {
                //Asignando Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini_ent);
                DateTime.TryParse(txtFecFin.Text, out fec_fin_ent);
            }
            
            //Obteniendo Requisiciones
            using (DataTable dtRequisiciones = SAT_CL.Almacen.Reportes.ObtieneRequisicionesPendientes(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                fec_ini_sol, fec_fin_sol, fec_ini_ent, fec_fin_ent,Convert.ToByte(ddlEstatus.SelectedValue),Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtAlmacen.Text,"ID:",1)),Convert.ToInt32(Cadena.VerificaCadenaVacia(txtNoRequisicio.Text,"0"))))
            {
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(dtRequisiciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvRequisicionesPendiente, dtRequisiciones, "Id", lblOrdenado.Text, true, 2);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtRequisiciones, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvRequisicionesPendiente);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        #endregion
    }
}