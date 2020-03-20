using SAT_CL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.FacturacionElectronica33
{
    public partial class CancelacionCFDI : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //validando PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando Página
                inicializaPagina();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFacturas_Click(object sender, EventArgs e)
        {
            //Determinando la pestaña pulsada
            switch (((Button)sender).CommandName)
            {
                case "FacturasDisponibles":
                    //Cambiando estilos de pestañas
                    btnFacturasCxC.CssClass = "boton_pestana_activo";
                    btnFacturasCxP.CssClass = "boton_pestana";
                    //Asignando vista activa de la forma
                    mtvCfdiCancelacion.SetActiveView(vwCxC);
                    break;
                case "FacturasLigadas":
                    //Cambiando estilos de pestañas
                    btnFacturasCxC.CssClass = "boton_pestana";
                    btnFacturasCxP.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvCfdiCancelacion.SetActiveView(vwCxP);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCFDI_Click(object sender, EventArgs e)
        {
            //Cargando CFDI's Pendientes
            cargaPendientesCancelacion();
            //Inicializando Vista
            mtvCfdiCancelacion.ActiveViewIndex = 0;
            //Cambiando estilos de pestañas
            btnFacturasCxC.CssClass = "boton_pestana_activo";
            btnFacturasCxP.CssClass = "boton_pestana";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarCFDI_Click(object sender, EventArgs e)
        {
            //Obteniendo Filas Marcadas
            GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvCfdiPendientes, "chkVarios");

            //Validando Filas
            if (gvr.Length > 0)
            {
                //Validamos Màximo permitido de Comprobantes
                if (Convert.ToInt32(gvr.Length) <= Convert.ToInt32(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Maximo Cancelacion CFDI")))
                {
                    //Recorriendo Filas
                    foreach (GridViewRow gv in gvr)
                    {
                        //Seleccionando Fila
                        gvCfdiPendientes.SelectedIndex = gv.RowIndex;

                        //Mostrando Resultado
                        ScriptServer.MuestraNotificacion(btnActualizarCFDI, actualizaComprobantes(), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }

                    //Inicializando Indices
                    Controles.InicializaIndices(gvCfdiPendientes);
                    //Recargando Consulta
                    cargaPendientesCancelacion();
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(btnActualizarCFDI, new RetornoOperacion(string.Format("Sólo es posible la cancelación de {0} Comprobantes.", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Maximo Cancelacion CFDI"))), ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            else
                //Mostrando Excepción
                ScriptServer.MuestraNotificacion(btnActualizarCFDI, new RetornoOperacion("No existen filas seleccionadas"), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            using (LinkButton lkb = (LinkButton)sender)
            {
                //Validando Comando
                switch (lkb.CommandName)
                {
                    case "ConsultaCFDI":
                        {
                            //Gestionando ventana Modal
                            gestionaVentanaModal(lkb, lkb.CommandName);
                            break;
                        }
                }
            }
        }

        #region Eventos GridView "CFDI's Pendientes Cancelación"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCFDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            Controles.CambiaTamañoPaginaGridView(gvCfdiPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoCFDI.SelectedValue), true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            using (LinkButton lkb = (LinkButton)sender)
            {
                //Validando Comando
                switch (lkb.CommandName)
                {
                    case "CfdiPendientes":
                        {
                            //Exporta el contenido de la tabla cargada en el gridview
                            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdCFDI-IdTFD-IdTabla-IdRegistro-IdAcuseTFD-AccionAcuse");
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Validando Si el GridView contiene Registros
            if (gvCfdiPendientes.DataKeys.Count > 0)
            {   
                //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   
                    case "chkTodos":
                        //Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                        CheckBox chk = (CheckBox)gvCfdiPendientes.HeaderRow.FindControl("chkTodos");
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvCfdiPendientes, "chkVarios", chk.Checked);
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbAccionCancelacion_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Datos
            if (gvCfdiPendientes.DataKeys.Count > 0)
            {
                //Selecionando Fila
                Controles.SeleccionaFila(gvCfdiPendientes, sender, "imb", false);

                //Declarando Objeto de Retorno
                RetornoOperacion retorno = actualizaComprobantes();

                //Validando Retorno
                if (retorno.OperacionExitosa)
                {
                    //Recargando Datos
                    cargaPendientesCancelacion();
                    //Inicializando indices
                    Controles.InicializaIndices(gvCfdiPendientes);
                }

                //Mostrando Notificación
                ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCfdiPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Tamaño de Página
            Controles.CambiaIndicePaginaGridView(gvCfdiPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCfdiPendientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Asignando Expresión de Ordenamiento
            lblOrdenadoCFDI.Text = Controles.CambiaSortExpressionGridView(gvCfdiPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCfdiPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando Tipo de Fila
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando fila a enlazar
                DataRow dr = ((DataRowView)e.Row.DataItem).Row;

                //Instanciando Image Button
                using (CheckBox chk = (CheckBox)e.Row.FindControl("chkVarios"))
                using (ImageButton imb = (ImageButton)e.Row.FindControl("imbAccionCancelacion"))
                {
                    //validando Existencia
                    if (imb != null && chk != null)
                    {
                        //Validando Datos
                        if (dr["AccionAcuse"] != null)
                        {
                            //Validando Acción
                            switch (dr["AccionAcuse"].ToString())
                            {
                                case "Solicitar Cancelación":
                                    {
                                        //Asignando Imagen
                                        imb.ImageUrl = "~/Image/cfdi_cancelacion.png";
                                        break;
                                    }
                                case "Consultar Cancelación":
                                    {
                                        //Asignando Imagen
                                        imb.ImageUrl = "~/Image/cfdi_consulta.png";
                                        break;
                                    }
                            }
                        }
                        else
                            //Asignando Imagen
                            imb.ImageUrl = "~/Image/cfdi_consulta.png";

                        //Validando Datos
                        if (dr["Tipo"] != null)
                        {
                            //Validando Acción
                            switch (dr["Tipo"].ToString())
                            {
                                case "PAGO [P]":
                                    {
                                        //Asignando Imagen
                                        imb.Visible = 
                                        chk.Visible = false;
                                        break;
                                    }
                                default:
                                    {
                                        //Asignando Imagen
                                        imb.Visible =
                                        chk.Visible = true;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();
            //Inicializando Controles
            inicializaControles();
        }
        /// <summary>
        /// 
        /// </summary>
        private void inicializaControles()
        {
            //Inicializando Vista
            mtvCfdiCancelacion.ActiveViewIndex = 0;
            //Cambiando estilos de pestañas
            btnFacturasCxC.CssClass = "boton_pestana_activo";
            btnFacturasCxP.CssClass = "boton_pestana";

            //Asignando Emisor
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando Existencia
                if (cer.habilitar)

                    //Asignando Valor del Emisor
                    lblEmisor.Text = string.Format("{0} Rfc:{1}", cer.nombre, cer.rfc);
            }

            //Inicializando grid
            Controles.InicializaGridview(gvCfdiPendientes);
        }
        /// <summary>
        /// 
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCFDI, "", 56);
        }
        /// <summary>
        /// 
        /// </summary>
        private void cargaPendientesCancelacion()
        {
            //Obteniendo Comprobantes Pendientes de Cancelación
            using (DataTable dtCfdiPend = SAT_CL.FacturacionElectronica33.Reporte.ObtieneComprobantesPendientesCancelacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtReceptor.Text, "ID:", 1)), txtSerie.Text, Convert.ToInt32(txtFolio.Text.Equals("") ? "0" : txtFolio.Text)))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dtCfdiPend))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvCfdiPendientes, dtCfdiPend, "IdCFDI-IdTFD-IdTabla-IdRegistro-IdAcuseTFD-AccionAcuse", "", true, 3);
                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCfdiPend, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvCfdiPendientes);
                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método encargado de Actualizar los Comprobantes
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion actualizaComprobantes()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            SAT_CL.FacturacionElectronica33.CancelacionCDFI.EstatusUUID estatusUUID = SAT_CL.FacturacionElectronica33.CancelacionCDFI.EstatusUUID.SinEstatus;
            SAT_CL.FacturacionElectronica33.CancelacionCDFI.TipoCancelacion tipo = SAT_CL.FacturacionElectronica33.CancelacionCDFI.TipoCancelacion.SinAsignar;
            XDocument consulta = new XDocument();
            int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;

            //Validando que este seleccionada la Fila
            if (gvCfdiPendientes.SelectedIndex != -1)
            {
                //Validando Comando
                switch (gvCfdiPendientes.SelectedDataKey["AccionAcuse"].ToString())
                {
                    case "Solicitar Cancelación":
                        {
                            //Enviando Petición de Cancelación
                            retorno = SAT_CL.FacturacionElectronica33.CancelacionCDFI.objCancelacion.CancelacionComprobanteCxC(Convert.ToInt32(gvCfdiPendientes.SelectedDataKey["IdCFDI"]), out estatusUUID, out tipo, out consulta, idUsuario);
                            break;
                        }
                    case "Consultar Cancelación":
                        {
                            //Consulta de Cancelación
                            retorno = SAT_CL.FacturacionElectronica33.Comprobante.ConsultaCancelacionComprobante(Convert.ToInt32(gvCfdiPendientes.SelectedDataKey["IdCFDI"]), idUsuario);
                            break;
                        }
                }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="comando"></param>
        private void gestionaVentanaModal(Control sender, string comando)
        {
            //Validando Comando
            switch (comando)
            {
                case "ConsultaCFDI":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, comando, "contenedorVentanaConsultaCFDI", "ventanaConsultaCFDI");
                        break;
                    }
            }
        }

        #endregion
    }
}