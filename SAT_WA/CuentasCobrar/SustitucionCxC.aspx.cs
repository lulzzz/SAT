using SAT_CL;
using SAT_CL.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasCobrar
{
    public partial class SustitucionCxC : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Carga
                inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            //Inicializando Contenido de Facturación
            Controles.InicializaGridview(gvFacturacionOtros);
            Controles.InicializaIndices(gvFacturacionOtros);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "FacOtros":
                    {
                        //Exportando Facturas
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdFO", "IdFacturado");
                        break;
                    }
                case "Servicios":
                    {
                        //Exportando Facturas
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "IdServicio");
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAccionSustitucion_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            Button btn = (Button)sender;

            //Validando Comando
            switch (btn.CommandName)
            {
                case "Aceptar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;

                        /** Validando que Tanto la Factura como el Servicio esten seleccionado **/
                        //Facturas
                        if (gvFacturacionOtros.SelectedIndex != -1)
                        {
                            //Servicios
                            if (gvServicios.SelectedIndex != -1)
                            {
                                //Inicializando Bloque Transaccional
                                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Instanciando Servicio, Facturacion Otros, Comprobantes y Facturados
                                    using (FacturacionOtros fo = new FacturacionOtros(Convert.ToInt32(gvFacturacionOtros.SelectedDataKey["IdFO"])))
                                    using (Facturado fac_fo = new Facturado(Convert.ToInt32(gvFacturacionOtros.SelectedDataKey["IdFacturado"])), 
                                                     fac_serv = new Facturado(Convert.ToInt32(gvServicios.SelectedDataKey["IdFacturado"])))
                                    using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                                    using (SAT_CL.FacturacionElectronica33.Comprobante cfdi = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvFacturacionOtros.SelectedDataKey["IdCFDI"])))
                                    {
                                        //Validando Facturacion de Otros
                                        if (fo.habilitar && fac_fo.habilitar)
                                        {
                                            //Validando Servicio
                                            if (serv.habilitar && fac_serv.habilitar)
                                            {
                                                //Validando Facturacion Electronica
                                                if (cfdi.habilitar)
                                                {
                                                    //Validando Clientes
                                                    if (fo.id_cliente_receptor == serv.id_cliente_receptor)
                                                    {
                                                        //Deshabilitando Facturación de Otros
                                                        retorno = fo.DeshabilitaFacturacionOtros(idUsuario);

                                                        //Validando Operación
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Deshabilitando Facturado del Servicio
                                                            retorno = fac_serv.DeshabilitaFactura(idUsuario);
                                                            
                                                            //Validando Operación
                                                            if (retorno.OperacionExitosa)
                                                            {
                                                                //Actualizando Facturado (FO) en Facturado (Servicio)
                                                                retorno = fac_fo.ActualizaServicio(serv.id_servicio, idUsuario);

                                                                //Validando Operación
                                                                if (retorno.OperacionExitosa)
                                                                {
                                                                    //Actualizando Origen del CFDI
                                                                    retorno = cfdi.ActualizaOrigenComprobante(SAT_CL.FacturacionElectronica33.Comprobante.OrigenDatos.Facturado, idUsuario);
                                                                }
                                                            }
                                                        }    
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        retorno = new RetornoOperacion("Los Clientes no coinciden");
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    retorno = new RetornoOperacion("No se puede recuperar los Datos de Facturación Electrónica");
                                            }
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion("No se puede recuperar los Datos del Servicio");
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No se puede recuperar los Datos de la Facturación de Otros");
                                    }

                                    //Validando Operación Final
                                    if (retorno.OperacionExitosa)
                                    
                                        //Completando Operaciones
                                        scope.Complete();
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("Debe seleccionar un Servicio");
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("Debe seleccionar una Factura");

                        //Validando operación
                        if (retorno.OperacionExitosa)
                        {
                            //Ocultando Ventana Modal
                            alternaVentana(btn, "Sustitucion");

                            //Recargando Busquedas
                            buscaFacturacion();
                            buscaServicios();

                            //Inicializando Indices
                            Controles.InicializaIndices(gvFacturacionOtros);
                            Controles.InicializaIndices(gvServicios);
                        }

                        //Mostrando Mensaje de la Operación
                        ScriptServer.MuestraNotificacion(btn, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        break;
                    }
                case "Cancelar":
                    {
                        //Ocultando Ventana Modal
                        alternaVentana(btn, "Sustitucion");

                        //Inicializando Indices
                        Controles.InicializaIndices(gvFacturacionOtros);
                        Controles.InicializaIndices(gvServicios);
                        break;
                    }
            }
        }

        #region Eventos Facturación

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFO_Click(object sender, EventArgs e)
        {
            //Buscando Facturas
            buscaFacturacion();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFO_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del Grid
            Controles.CambiaTamañoPaginaGridView(gvFacturacionOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFO.SelectedValue), true, 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturacionOtros_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFO.Text = Controles.CambiaSortExpressionGridView(gvFacturacionOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturacionOtros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFacturacionOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturacionOtros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando Tipo de Fila
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbSeleccionarFO_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Datos
            if (gvFacturacionOtros.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturacionOtros, sender, "imb", false);

                //Mostrando confirmación
                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(1,"Seleccione el Servicio al que desea agregar la Factura",true), ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #region Eventos Servicios

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarServ_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda de Servicios
            buscaServicios();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoServ.Text = Controles.CambiaSortExpressionGridView(gvFacturacionOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando Tipo de Fila
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoServ_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del Grid
            Controles.CambiaTamañoPaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoServ.SelectedValue), true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbReemplazarServ_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Datos
            if (gvServicios.DataKeys.Count > 0)
            {
                //Validando Seleción de la Factura
                if (gvFacturacionOtros.SelectedIndex != -1)
                {
                    //Seleccionando Fila
                    Controles.SeleccionaFila(gvServicios, sender, "imb", false);

                    //Instanciando Facturación de Otros
                    using (SAT_CL.Facturacion.FacturacionOtros fo = new SAT_CL.Facturacion.FacturacionOtros(Convert.ToInt32(gvFacturacionOtros.SelectedDataKey["IdFO"])))
                    using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                    using (SAT_CL.FacturacionElectronica33.Comprobante cfdi = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvFacturacionOtros.SelectedDataKey["IdCFDI"])))
                    {
                        //Validando Servicio
                        if (serv.habilitar)
                        {
                            //Validando Facturacion de Otros
                            if (fo.habilitar)
                            {
                                //Validando Comprobante
                                if (cfdi.habilitar)
                                {
                                    //Personalizando Mensaje
                                    lblEncabezadoConfirmacion.Text = string.Format("Se añadira la Factura '{0}{1}' al Servicio '{2}'", cfdi.serie, cfdi.folio, serv.no_servicio);
                                    //Mostrando Modal
                                    alternaVentana(this.Page, "Sustitucion");
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar la Factura Electronica"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar la Factura"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Servicio"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
                else
                    //Instanciando Excepción
                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Debe seleccionar una Factura"), ScriptServer.PosicionNotificacion.AbajoDerecha);
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
            cargaCatalogo();
            //Inicializando controles
            inicializaControles();
        }
        /// <summary>
        /// 
        /// </summary>
        private void cargaCatalogo()
        {
            //Cargando Tamaño
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFO, "", 26);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServ, "", 26);
        }
        /// <summary>
        /// 
        /// </summary>
        private void inicializaControles()
        {
            //Limpiando Controles
            txtCliente.Text =
            txtSerie.Text =
            txtFolio.Text = "";
            txtFInicio.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFTermino.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            txtNoServicio.Text =
            txtReferencia.Text =
            txtLugarCarga.Text =
            txtLugarDescarga.Text = "";
            txtInicioServ.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtTerminoServ.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Asignando Enfoque
            txtCliente.Focus();

            //Inicializando Grid's
            Controles.InicializaGridview(gvFacturacionOtros);
            Controles.InicializaGridview(gvServicios);
        }
        /// <summary>
        /// Método encargado de Alternar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="comando"></param>
        private void alternaVentana(Control sender, string comando)
        {
            //Validando Comando
            switch (comando)
            {
                case "Sustitucion":
                    {
                        //Alternando ventana Modal
                        ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenedorVentanaConfirmacionSustitucion", "ventanaConfirmacionSustitucion");
                        break;
                    }
            }
        }

        #region Métodos Facturación

        /// <summary>
        /// Método encargado de Buscar las Facturas
        /// </summary>
        private void buscaFacturacion()
        {
            //Declarando Variables Auxiliares
            DateTime fecha_inicio, fecha_fin;
            fecha_inicio = fecha_fin = DateTime.MinValue;
            

            //Validando Inclusión
            if (chkIncluirFO.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFInicio.Text, out fecha_inicio);
                DateTime.TryParse(txtFTermino.Text, out fecha_fin);
            }

            //Obteniendo Datos del SP
            using (DataTable dtFacturas = SAT_CL.Facturacion.Reporte.CargaFacturacionOtrosCFDI(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), txtSerie.Text, Convert.ToInt32(txtFolio.Text.Equals("") ? "0" : txtFolio.Text),
                        txtUUID.Text, fecha_inicio, fecha_fin, txtReferenciaFO.Text))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando Datos
                    Controles.CargaGridView(gvFacturacionOtros, dtFacturas, "IdFO-IdFacturado-IdCFDI", "", true, 4);
                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table");
                }
                else
                {
                    //Inicilizando Datos
                    Controles.InicializaGridview(gvFacturacionOtros);
                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }


        #endregion

        #region Métodos Servicios

        /// <summary>
        /// Método encargado de Obtener los Servicios sin Facturar
        /// </summary>
        private void buscaServicios()
        {
            //Declarando Variables Auxiliares
            DateTime fecha_inicio, fecha_fin;
            fecha_inicio = fecha_fin = DateTime.MinValue;


            //Validando Inclusión
            if (chkIncluirServ.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtInicioServ.Text, out fecha_inicio);
                DateTime.TryParse(txtTerminoServ.Text, out fecha_fin);
            }

            //Obteniendo Datos del SP
            using (DataTable dtServicios = SAT_CL.Documentacion.Reportes.CargaServiciosSinFacturacionElectronica(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(txtNoServicio.Text.Equals("") ? "0" : txtNoServicio.Text), txtReferencia.Text,
                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtLugarCarga.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtLugarDescarga.Text, "ID:", 1)),
                        fecha_inicio, fecha_fin))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dtServicios))
                {
                    //Cargando Datos
                    Controles.CargaGridView(gvServicios, dtServicios, "IdServicio-IdFacturado", "", true, 3);
                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServicios, "Table1");
                }
                else
                {
                    //Inicilizando Datos
                    Controles.InicializaGridview(gvServicios);
                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        #endregion

        #endregion
    }
}