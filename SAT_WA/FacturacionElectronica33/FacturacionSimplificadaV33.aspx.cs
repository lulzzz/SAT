using SAT_CL.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.FacturacionElectronica33
{
    public partial class FacturacionSimplificadaV33 : System.Web.UI.Page
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
            //Validando que se produjo un PostBack
            if (!Page.IsPostBack)
                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            servicioxFacturar();
        }
        
        //Clasificacion de Eventos
        #region Eventos GridView "Servicio Por Facturar"
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView ""Servicio x Facturar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvServicioxFacturar, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);

            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar un Reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServiciosxFacturar_Click(object sender, EventArgs e)
        {

            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");

        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Servicios x  Facturar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicioxFacturar_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvServicioxFacturar, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);

            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Servicios x  Facturar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicioxFacturar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvServicioxFacturar, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);

            //Invocando Método de Suma
            sumaTotales();
        }
        #endregion

        #region Eventos: "Addenda"
        /// <summary>
        /// Evento Generado al dar click en Addenda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAddenda_Click(object sender, EventArgs e)
        {
            //Validamos Registros
            if (gvServicioxFacturar.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicioxFacturar, sender, "lnk", false);

                //Instanciando Comprobante
                using (FacturadoFacturacion Fac = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"]))))
                {
                    //Validando Relación
                    if (Fac.habilitar && Fac.id_factura_electronica > 0)
                    {
                        //Validamos que exista Relación
                        using (SAT_CL.FacturacionElectronica.Comprobante comp = new SAT_CL.FacturacionElectronica.Comprobante(Fac.id_factura_electronica))
                        {
                            //CargaCatalogo
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlAddenda, 49, "Ninguno", comp.id_compania_emisor, "", comp.id_compania_receptor, "");

                            //Mostrar Ventana Modal
                            alternaVentanaModal("ConfirmacionAddenda", gvServicioxFacturar);
                        }
                    }
                    //Validando Relación
                    else if (Fac.habilitar && Fac.id_factura_electronica33 > 0)
                    {
                        //Validamos que exista Relación
                        using (SAT_CL.FacturacionElectronica33.Comprobante comp = new SAT_CL.FacturacionElectronica33.Comprobante(Fac.id_factura_electronica33))
                        {
                            //CargaCatalogo
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlAddenda, 49, "Ninguno", comp.id_compania_emisor, "", comp.id_compania_receptor, "");

                            //Mostrar Ventana Modal
                            alternaVentanaModal("ConfirmacionAddenda", gvServicioxFacturar);
                        }
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe la Factura Electronica"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento generado al dar click en Aceptar Addenda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarAddenda_Click(object sender, EventArgs e)
        {
            //Validamos Addenda
            RetornoOperacion resultado = validaAddenda();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarAddenda, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion

        #region Eventos: "Facturación Electrónica"
        /// <summary>
        /// Evento producido al presionar el checkbox "TipoTodos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTipoTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros activos
            if (gvReferencias.DataKeys.Count > 0)
            {
                //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;

                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvReferencias.FooterRow.FindControl("lblContadorTipo"))
                    //Realizando la selección de todos los elementos del GridView
                    c.Text = Controles.SeleccionaFilasTodas(gvReferencias, "chkSeleccionTipo", todos.Checked).ToString();
            }
        }
        /// <summary>
        /// Evento producido al presionar el cada checkbox de la lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSeleccionTipo_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvReferencias.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Realizando des/selección del elemento
                Controles.SumaSeleccionadosFooter(d.Checked, gvReferencias, "lblContadorTipo");

                //Si retiró selección
                if (!d.Checked)
                {
                    //Referenciando control de encabezado
                    CheckBox t = (CheckBox)gvReferencias.HeaderRow.FindControl("chkTipoTodos");
                    //deshabilitando seleccion
                    t.Checked = d.Checked;
                }
            }
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoReferencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoReferencias.SelectedValue));
        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoReferencias.Text = Controles.CambiaSortExpressionGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
        }
        /// <summary>
        /// Evento generado al registrar la FE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegistrarFE_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Retorno
            RetornoOperacion resultado = RegistraFacturacionElectronica();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnRegistrarFE, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento generado al Aceptar la Cancelación un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            //Cancelamos Factura
            RetornoOperacion resultado = cancelaCFDI();

            //Validando Resultado
            if (resultado.OperacionExitosa)

                //Carga Viajes
                servicioxFacturar();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarCancelacionCFDI, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento generado al dar click en Registrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegistrarFacturaElectronica_Click(object sender, EventArgs e)
        {
            //Si existe la Tabla de Referencias
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Eliminamos Tabla
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }
            //Craga GV Referencias
            cargaReferencias();
            //Si existen Referencias
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Mostramos Venata Modal
                alternaVentanaModal("ReferenciasRegistro", btnRegistrarFacturaElectronica);
                //Cerramos Ventana Modal de Registro
                alternaVentanaModal("RegistrarFacturacionElectronica", btnRegistrarFacturaElectronica);

            }
            else
            {
                //Declaramos objeto Retorno
                RetornoOperacion resultado = RegistraFacturacionElectronica();
                //Mostrando resultado
                ScriptServer.MuestraNotificacion(btnRegistrarFacturaElectronica, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Genrado al dar click en Accesorios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccesorios_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;
            //Validamos Registros
            if (gvServicioxFacturar.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicioxFacturar, sender, "lnk", false);
                //De acuerdo al comando del botón
                switch (lnk.CommandName)
                {
                    case "PDF":
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FcaturacionElectronicaSimplificadaV33.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciamos Relación
                        using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))))
                        {
                            //Validamos que exista la facturación electrónica
                            if (objFacturaFacturacion.id_factura_electronica33 > 0)
                            {
                                //Instanciamos Comprobante
                                using (SAT_CL.FacturacionElectronica33.Comprobante objComprobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaFacturacion.id_factura_electronica33))
                                {
                                    //Validamos que exista el Comprobante
                                    if (objComprobante.id_comprobante33 > 0)
                                    {
                                        //Instanciando nueva ventana de navegador para apertura de registro
                                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteV33", objFacturaFacturacion.id_factura_electronica33), "Comprobante 3.3", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                    }
                                }
                            }

                        }
                        break;
                    case "XML":
                        //Descarga XML
                        descargarXML();
                        break;
                    case "VerComprobante":
                        //Instanciando Factura
                        using (Facturado facturado = new Facturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
                        {
                            //Instanciamos Relación
                            using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))))
                            {
                                //Validamos Factura Electrónica
                                if (objFacturaFacturacion.id_factura_electronica33 > 0)
                                {
                                    //Obteniendo Ruta
                                    string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturacionElectronicaSimplificadaV33.aspx", "~/FacturacionElectronica33/ComprobanteV33.aspx");

                                    //Estatus de Lectura
                                    Session["estatus"] = Pagina.Estatus.Lectura;

                                    //Instanciando nueva ventana de navegador para apertura de registro
                                    Response.Redirect(string.Format("{0}?idRegistro={1}", url, objFacturaFacturacion.id_factura_electronica33));
                                }
                                else
                                    //Mostrando Notificación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No existe la Factura Electrónica", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Evento Genrado al Registrar una Factura Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRegistroFE_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;
            //Validamos Registros
            if (gvServicioxFacturar.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicioxFacturar, sender, "lnk", false);

                //De acuerdo al comando del botón
                switch (lnk.CommandName)
                {
                    case "Registrar CFDI":
                        //Cerrando ventana modal 
                        alternaVentanaModal("RegistrarFacturacionElectronica", lnk);
                        //Inicializamos Valores
                        inicializaValoresRegistroFacturacionElectronica();
                        //Inicializando Indices
                        break;
                    case "Eliminar CFDI":
                        //Mostramos ventana modal 
                        alternaVentanaModal("ConfirmacionEliminacionCFDI", lnk);
                        break;
                }
            }
        }
        /// <summary>
        /// Evento Genrado al Timbrar una Factura Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbTimbrarFE_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;
            //Validamos Registros
            if (gvServicioxFacturar.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicioxFacturar, sender, "lnk", false);
                //De acuerdo al comando del botón
                switch (lnk.CommandName)
                {
                    case "Timbrar CFDI":
                        //Cerrando ventana modal 
                        alternaVentanaModal("TimbrarFacturacionElectronica", lnk);
                        //Inicializamos Valores
                        inicializaValoresTimbrarFacturacionElectronica();
                        break;
                    case "Cancelar CFDI":
                        //Cargamos Aplicaciones
                        cargaAplicaciones();
                        //Mostramos ventana modal 
                        alternaVentanaModal("CancelacionCancelarCFDI", lnk);
                        break;
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cerrar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;
            //De acuerdo al comando del botón
            switch (lnk.CommandName)
            {
                case "edicionConceptos":
                    //Cerrando ventana modal 
                    alternaVentanaModal("EdicionConceptos", lnk);
                    //Inicializando Indices
                    break;
                case "registrarFacturacionElectronica":
                    //Cerrando ventana modal 
                    alternaVentanaModal("RegistrarFacturacionElectronica", lnk);
                    break;
                case "referenciasServicio":
                    //Cerrando ventana modal 
                    alternaVentanaModal("ReferenciasServicio", lnk);
                    break;
                case "confirmacionEliminacionCFDi":
                    //Cerrando ventana modal 
                    alternaVentanaModal("ConfirmacionEliminacionCFDI", lnk);
                    break;
                case "timbrarFacturacionElectronica":
                    //Cerrando ventana modal 
                    alternaVentanaModal("TimbrarFacturacionElectronica", lnk);
                    break;
                case "confirmacionAddenda":
                    //Cerrando ventana modal 
                    alternaVentanaModal("ConfirmacionAddenda", lnk);
                    break;
                case "addenda":
                    //Cerrando ventana modal 
                    alternaVentanaModal("Addenda", lnk);
                    break;
                case "comentario":
                    //Cerrando ventana modal 
                    alternaVentanaModal("Comentario", lnk);
                    break;
                case "referenciasRegistro":
                    //Cerrando ventana modal 
                    alternaVentanaModal("ReferenciasRegistro", lnk);
                    break;
                case "producto":
                    //Cerrando ventana modal 
                    alternaVentanaModal("Producto", lnk);
                    break;
            }
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvServicioxFacturar);
            //Actualizando Reporte
            upgvServicioxFacturar.Update();
        }
        /// <summary>
        /// Evento generado al Cancelra CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminarCFDI_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            alternaVentanaModal("ConfirmacionEliminacionCFDI", btnCancelarEliminarCFDI);
        }
        /// <summary>
        /// Evento generado al Cancelar  la  Cancelación de un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            alternaVentanaModal("CancelacionCancelarCFDI", btnCancelarCancelacionCFDI);
        }
        /// <summary>
        /// Evento generado al timbrar la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTimbrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Factura
            using (Facturado objFacturado = new Facturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
            {
                //Registramos Factura
                resultado = objFacturado.TimbraFacturadoComprobante_V3_3(txtSerieFS.Text, chkOmitirAddenda.Checked, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                                         HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Carga Servicios
                    servicioxFacturar();
                }
            }

            //Cerrando ventana modal 
            alternaVentanaModal("TimbrarFacturacionElectronica", btnAceptarTimbrarFacturacionElectronica);
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarTimbrarFacturacionElectronica, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento generado al Eliminar un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarCFDI_Click(object sender, EventArgs e)
        {
            RetornoOperacion resultado = EliminaCFDI();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarEliminarCFDI, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Carga Viajes
            servicioxFacturar();
        }
        /// <summary>
        /// Evento generado al cambiar e método de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*/Si el Método de pago es efectivo
            if (ddlMetodoPago.SelectedValue == "9")
            {
                //Asignamos cuenta como NO IDENTIFICADO
                ddlUsoCFDI.SelectedValue = "0";
                //Deshabilitamos Control
                ddlUsoCFDI.Enabled = false;
            }
            else
            {
                //En caso de existir cuenta
                if (ddlUsoCFDI.Items.Count > 1)
                {
                    //Asignamos cuenta por default
                    ddlUsoCFDI.SelectedValue = ddlUsoCFDI.Items[1].Value;
                    //Habilitamos Control
                    ddlUsoCFDI.Enabled = true;
                }
            }//*/
        }

        #endregion

        #region Eventos: "Concepto"
        /// <summary>
        /// Guardar cargo
        /// </summary>
        protected void wucFacturadoConceptoV33_ClickGuardarFacturaConcepto(object sender, EventArgs e)
        {
            //Realizando guardado
            RetornoOperacion resultado = ucFacturadoConcepto.GuardarFacturaConcepto();
            //Si se guardó correctamente
            if (resultado.OperacionExitosa)
            {
                //Actualizando lista de servicios
                servicioxFacturar();
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Eliminar cargo
        /// </summary>
        protected void wucFacturadoConceptoV33_ClickEliminarFacturaConcepto(object sender, EventArgs e)
        {
            //Realizando guardado
            RetornoOperacion resultado = ucFacturadoConcepto.EliminaFacturaConcepto();
            //Si se guardó correctamente
            if (resultado.OperacionExitosa)
            {
                //Actualizando lista de servicios
                servicioxFacturar();
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Editar Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerConceptos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvServicioxFacturar.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvServicioxFacturar, sender, "lnk", false);
                //Validando que exista una Factura Seleccionada
                if (Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"]) > 0)
                {
                    //Inicializando Control
                    ucFacturadoConcepto.InicializaControl(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"]));
                    //Abriendo Ventana Modal
                    alternaVentanaModal("EdicionConceptos", upgvServicioxFacturar);
                }
            }
        }
        #endregion

        #region Eventos: "Referencias"
        /// <summary>
        /// Click Guardar Referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Realizando guardado de referencia
            RetornoOperacion resultado = wucReferenciaViaje.GuardaReferenciaViaje();
            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando Gridview
                servicioxFacturar();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click Eliminar Referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Realizando guardado de referencia
            RetornoOperacion resultado = wucReferenciaViaje.EliminaReferenciaViaje();
            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando Gridview
                servicioxFacturar();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento click sobre alguno de los botónes de acciones con ventanas modales del servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOtros_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvServicioxFacturar.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicioxFacturar, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;
                //Determinando el comando a ejecutar
                switch (lkb.CommandName)
                {
                    case "referenciasServicio":
                        //Inicializando control de referencias de servicio
                        wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["IdServicio"]));
                        //Realizando apertura de referencias de servicio
                        alternaVentanaModal("ReferenciasServicio", lkb);
                        break;
                    case "servicio":
                        //Inicializando Reerencias de Productos
                        wucProducto.InicializaControl(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["IdServicio"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        //Realizando apertura de referencias de servicio
                        alternaVentanaModal("Producto", lkb);
                        break;
                    case "porte":
                        //Mostramos Modal
                        alternaVentanaModal("encabezadoServicio", lkb);
                        //Inicializando control de referencias de servicio
                        wucEncabezadoServicio.InicializaEncabezadoServicio(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["IdServicio"]));
                        break;
                }
            }
        }
        /// <summary>
        /// Click en botón cerrar de ventanas modales de acciones de servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Determinando botón pulsado
            LinkButton lkbCerrar = (LinkButton)sender;
            //De acuerdo al comando del botón
            switch (lkbCerrar.CommandName)
            {
                case "EncabezadoServicio":
                    //Cerrando modal de referencias de servicio
                    alternaVentanaModal("encabezadoServicio", lkbCerrar);
                    break;
            }
        }
        #endregion

        #region Eventos: "Otros"

        #endregion

        #region Eventos: "Referencias Encabezado Servicio"
        /// <summary>
        /// Evento Producido al Guardar las Referencias del Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEncabezadoServicio_ClickGuardarReferencia(object sender, EventArgs e)
        {
            //Guardando Referencias
            wucEncabezadoServicio.GuardaEncabezadoServicio();
            //Cerrando Ventana Modal
            alternaVentanaModal("encabezadoServicio", this);
            //Cargando servicios
            servicioxFacturar();
        }
        #endregion

        #endregion

        #region Métodos

        //Clasificacion de métodos
        #region Métodos: "Addenda"
        /// <summary>
        /// Método encargado de validar si existe la Addenda
        /// </summary>
        private RetornoOperacion validaAddenda()
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Validamos que exista Addenda Seleccionada
            if (ddlAddenda.SelectedValue != "0")
            {
                //Instanciando Comprobante
                using (FacturadoFacturacion Fac = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"]))))
                {
                    //Validamos que exista Relación
                    using (SAT_CL.FacturacionElectronica.Comprobante comp = new SAT_CL.FacturacionElectronica.Comprobante(Fac.id_factura_electronica))
                    using (SAT_CL.FacturacionElectronica33.Comprobante cfdi33 = new SAT_CL.FacturacionElectronica33.Comprobante(Fac.id_factura_electronica33))
                    {
                        //Validamos Id Comprobante
                        if (comp.id_comprobante > 0)
                        {
                            //Inicializamos Control
                            wucAddendaComprobante.InicializaControl("3.2", comp.id_comprobante, Convert.ToInt32(ddlAddenda.SelectedValue));
                            //Cerrar Ventana Modal
                            alternaVentanaModal("ConfirmacionAddenda", btnAceptarAddenda);
                            //Cerrar Ventana Modal
                            alternaVentanaModal("Addenda", btnAceptarAddenda);
                        }
                        else if (cfdi33.id_comprobante33 > 0)
                        {
                            //Inicializamos Control
                            wucAddendaComprobante.InicializaControl("3.3", cfdi33.id_comprobante33, Convert.ToInt32(ddlAddenda.SelectedValue));
                            //Cerrar Ventana Modal
                            alternaVentanaModal("ConfirmacionAddenda", btnAceptarAddenda);
                            //Cerrar Ventana Modal
                            alternaVentanaModal("Addenda", btnAceptarAddenda);
                        }
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No se encontró registró de Factura Electrónica");
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        #endregion

        #region Métodos: "Facturación Electrónica"
        /// <summary>
        /// Eliminar CFDI
        /// </summary>
        private RetornoOperacion EliminaCFDI()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Relación
            using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"]))))
            {
                //Validamos que existan Relación
                if (objFacturaFacturacion.habilitar && objFacturaFacturacion.id_factura_electronica33 > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaFacturacion.id_factura_electronica33))
                    {
                        //Validando Comprobante
                        if (objCompobante.habilitar)
                        
                            //Enviamos link
                            resultado = objCompobante.DeshabilitaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No existe la Factura Electrónica v3.3.");
                    }
                }
                //Validamos que existan Relación
                else if (objFacturaFacturacion.habilitar && objFacturaFacturacion.id_factura_electronica > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica.Comprobante objCompobante = new SAT_CL.FacturacionElectronica.Comprobante(objFacturaFacturacion.id_factura_electronica))
                    {
                        //Validando Comprobante
                        if (objCompobante.habilitar)

                            //Enviamos link
                            resultado = objCompobante.DeshabilitaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No existe la Factura Electrónica v3.2.");
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("No existe la Factura Electrónica.");

                //Cerrar Ventana Modal
                alternaVentanaModal("ConfirmacionEliminacionCFDI", btnAceptarEliminarCFDI);
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Inicializa Valores para el Timbrado de la Facturación Electrónica
        /// </summary>
        private void inicializaValoresTimbrarFacturacionElectronica()
        {
            txtSerieFS.Text = "";
            lblTimbrarFacturacionElectronica.Text = "";
            chkOmitirAddenda.Checked = false;
        }
        /// <summary>
        /// Registro de la Facturación Electronica
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion RegistraFacturacionElectronica()
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciando Factura
            using (Facturado fac = new Facturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
            {
                //Validando Facturado
                if (fac.habilitar)
                {
                    //Declarando Nodos Relacionados
                    List<Tuple<int, byte, decimal, decimal>> cfdi_rel = new List<Tuple<int, byte, decimal, decimal>>();
                    //Registramos Factura
                    resultado = fac.ImportaFacturadoComprobante_V3_3(Convert.ToByte(ddlFormaPago.SelectedValue), Convert.ToInt32(ddlUsoCFDI.SelectedValue), Convert.ToByte(ddlMetodoPago.SelectedValue), Convert.ToByte(ddlTipoComrobante.SelectedValue), Convert.ToInt32(ddlSucursal.SelectedValue),
                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, 
                                obtieneReferencias().TrimEnd(','), cfdi_rel);
                }
                else
                    //Instanciando Exceción
                    resultado = new RetornoOperacion("No se puede recuperar la Factura");

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Si existen Referencias
                    if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                    {
                        //Cerramo Ventana Modal
                        alternaVentanaModal("ReferenciasRegistro", btnRegistrarFE);
                    }
                    else
                    {
                        //Cerramos Ventana Modal de Registro
                        alternaVentanaModal("RegistrarFacturacionElectronica", btnRegistrarFacturaElectronica);
                    }
                    //Carga Viajes
                    servicioxFacturar();
                }
            }
            return resultado;
        }
        /// <summary>
        /// Obtiene Referencias de Viaje
        /// </summary>
        /// <returns></returns>
        private string obtieneReferencias()
        {
            //Verificando que existan depósitos seleccionados
            GridViewRow[] Tipo = Controles.ObtenerFilasSeleccionadas(gvReferencias, "chkSeleccionTipo");
            //Declarando Arreglo para almacenar las Referencias
            string Referencias = "0";
            //Si existen 
            if (Tipo.Length > 0)
            {
                //Para cada uno de los controles marcados
                foreach (GridViewRow r in Tipo)
                {
                    //Seleccionando la fila
                    gvReferencias.SelectedIndex = r.RowIndex;
                    //Instanciando egreso por depósito
                    Referencias += gvReferencias.SelectedDataKey["Id"] + ",";
                }
            }
            //Retornamos Valor
            return Referencias != "0" ? Referencias.TrimStart('0') : Referencias;
        }
        /// <summary>
        /// Método encargado de Buscar las Referencias de Viaje
        /// </summary>
        private void cargaReferencias()
        {
            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtReferencia = SAT_CL.Facturacion.Reporte.ObtienesDatosReferenciasFacturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtReferencia))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvReferencias, dtReferencia, "Id", "", false, 1);
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtReferencia, "Table1");
                }
                else
                {
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Método encargado de Buscar las Aplicaciones
        /// </summary>
        private void cargaAplicaciones()
        {
            //Instanciando Aplicaciones
            using (DataTable dtAplicaciones = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(0, Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
            {
                //Validando que existan Aplicaciones
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAplicaciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvAplicaciones, dtAplicaciones, "Id", "", true, 2);
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAplicaciones, "Table2");
                    //Cambiando a Vista de Aplicaciones
                    mtvCancelacionCFDI.ActiveViewIndex = 1;
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvAplicaciones);
                    //Inicializando GridView
                    Controles.InicializaGridview(gvAplicaciones);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                    //Cambiando a Vista de Mensaje
                    mtvCancelacionCFDI.ActiveViewIndex = 0;
                }
            }
        }
        /// <summary>
        /// Inicializa Valores para el registro a Facturación Electrónica
        /// </summary>
        private void inicializaValoresRegistroFacturacionElectronica()
        {
            //Inicializamos Valores para Registrò de l FE
            ddlTipoComrobante.SelectedValue = "1";

            //Cargando Catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 108, "Ninguno", obtieneClienteFacturado(), "", 0, "");

            //Obteniendo Items
            ListItem liFormaPago = ddlFormaPago.Items.FindByText("Por definir [99]");
            ListItem liMetodoPago = ddlMetodoPago.Items.FindByText("Pago en parcialidades o diferido");
            ListItem liUsoCFDI = ddlUsoCFDI.Items.FindByText("[P01] Por definir");

            //Validando Item FP
            if (liFormaPago != null)
                //Asignando Forma de Pago
                ddlFormaPago.SelectedValue = liFormaPago.Value;
            //Validando Item MP
            if (liMetodoPago != null)
                //Asignando Forma de Pago
                ddlMetodoPago.SelectedValue = liMetodoPago.Value;
            //Validando Item Uso CFDI
            if (liUsoCFDI != null)
                //Asignando Uso del CFDI
                ddlUsoCFDI.SelectedValue = liUsoCFDI.Value;
        }
        /// <summary>
        /// Método encargado de Obtener el Cliente de un Facturado
        /// </summary>
        /// <returns></returns>
        private int obtieneClienteFacturado()
        {
            //Declarando Variable para el Cliente
            int idCliente = 0;

            //Validando que exista un Registro Seleccionado
            if (gvServicioxFacturar.SelectedIndex != -1)
            {
                //Instanciando Facturado
                using (Facturado facturado = new Facturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
                {
                    //Validando Facturado
                    if (facturado.habilitar)
                    {
                        //Validando Servicio
                        if (facturado.id_servicio > 0)
                        {
                            //Instancando Servicio
                            using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(facturado.id_servicio))
                            {
                                //Validando Existencia del Servicio
                                if (serv.habilitar)

                                    //Asignando Cliente
                                    idCliente = serv.id_cliente_receptor;
                            }
                        }
                        else
                        {
                            //Obteniendo Facturación Otros
                            using (SAT_CL.Facturacion.FacturacionOtros fo = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(facturado.id_factura))
                            {
                                //Validando Existencia del FO
                                if (fo.habilitar)

                                    //Asignando Cliente
                                    idCliente = fo.id_cliente_receptor;
                            }
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return idCliente;
        }
        /// <summary>
        /// Cancelar CFDI
        /// </summary>
        private RetornoOperacion cancelaCFDI()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();
            int id_comprobante = 0;
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Factura
                using (Facturado facturado = new Facturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
                {
                    //Instanciamos Relación
                    using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))))
                    {
                        //Validamos Factura Electrónica
                        if (objFacturaFacturacion.id_factura_electronica > 0)
                        {
                            //Instanciamos Comprobamte
                            using (SAT_CL.FacturacionElectronica.Comprobante objCompobante = new SAT_CL.FacturacionElectronica.Comprobante(objFacturaFacturacion.id_factura_electronica))
                            {
                                //Validando Comprobante
                                if (objCompobante.habilitar)
                                {
                                    //Enviamos link
                                    id_comprobante = objCompobante.id_comprobante;
                                    resultado = objCompobante.CancelaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                else
                                    //Establecemos Error
                                    resultado = new RetornoOperacion("No existe la Factura Electrónica v3.2");

                                //Validando Resultado
                                if (resultado.OperacionExitosa)

                                    //Insertando Referencia
                                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(objFacturaFacturacion.id_factura_electronica, 119, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 119, "Motivo Cancelación", 0, "Facturacion Electrónica"),
                                                txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                            }
                        }
                        //Validamos Factura Electrónica
                        else if (objFacturaFacturacion.id_factura_electronica33 > 0)
                        {
                            //Instanciamos Comprobamte
                            using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaFacturacion.id_factura_electronica33))
                            {
                                //Validando Comprobante
                                if (objCompobante.habilitar)
                                {
                                    //Enviamos link
                                    id_comprobante = objCompobante.id_comprobante33;
                                    resultado = objCompobante.CancelacionPendiente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                else
                                    //Establecemos Error
                                    resultado = new RetornoOperacion("No existe la Factura Electrónica v3.3");

                                //Validando Resultado
                                if (resultado.OperacionExitosa)

                                    //Insertando Referencia
                                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(objFacturaFacturacion.id_factura_electronica33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Motivo Cancelación", 0, "Facturacion Electrónica"),
                                                txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                            }
                        }
                        else
                            //Establecemos Error
                            resultado = new RetornoOperacion("No existe la Factura Electrónica");

                        //Validamos Resultado
                        if (resultado.OperacionExitosa && id_comprobante > 0)
                        {
                            //Deshabilitando Aplicaciones
                            //resultado = SAT_CL.CXC.FichaIngresoAplicacion.DeshabilitaAplicacionesFacturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"]), id_comprobante, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando Operación Exitosa
                            if (resultado.OperacionExitosa)
                            {
                                //Cerramo Ventana Modal
                                alternaVentanaModal("CancelacionCancelarCFDI", btnAceptarCancelacionCFDI);
                                //Completando Transacción
                                trans.Complete();
                            }
                        }
                    }
                }
            }
            //Devolvemos Valor
            return resultado;
        }

        #endregion

        #region Métodos: "Otros"
        /// <summary>
        /// Evento generado al dar click en Aceptar Comentario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarComentario_Click(object sender, EventArgs e)
        {
            //Actualizamos Comentario   
            RetornoOperacion resultado = actualizaComentario();
            //Cerramos Ventana Modal
            alternaVentanaModal("Comentario", btnAceptarComentario);
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarComentario, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Actualiza el Comentario de la Factura
        /// </summary>
        private RetornoOperacion actualizaComentario()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Factura
            using (Facturado facturado = new Facturado(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))
            {
                //Instanciamos Relación
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))))
                {
                    //Validamos Factura Electrónica
                    if (objFacturaFacturacion.id_factura_electronica33 > 0)
                    {
                        //Obteniendo Referencias
                        using (DataTable dtRef = SAT_CL.Global.Referencia.CargaReferencias(objFacturaFacturacion.id_factura_electronica33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Comentario", 0, "Facturacion Electrónica")))
                        {
                            //Valdiando que Existan
                            if (Validacion.ValidaOrigenDatos(dtRef))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtRef.Rows)
                                {
                                    //Validando que Exista el Registro
                                    if (Convert.ToInt32(dr["Id"]) > 0)
                                    {
                                        //Instanciando Observación
                                        using (SAT_CL.Global.Referencia comentario = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                        {
                                            //Validando que exista la Referencia
                                            if (comentario.habilitar)
                                            {
                                                //Editamos Referencia
                                                resultado = SAT_CL.Global.Referencia.EditaReferencia(comentario.id_referencia, txtComentario.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Terminamos el Ciclo
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                //Insertando Referencia
                                resultado = SAT_CL.Global.Referencia.InsertaReferencia(objFacturaFacturacion.id_factura_electronica33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Comentario", 0, "Facturacion Electrónica"),
                                            txtComentario.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                        }
                    }
                    //Validamos Factura Electrónica
                    else if (objFacturaFacturacion.id_factura_electronica > 0)
                    {
                        //Obteniendo Referencias
                        using (DataTable dtRef = SAT_CL.Global.Referencia.CargaReferencias(objFacturaFacturacion.id_factura_electronica, 119, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 119, "Comentario", 0, "Facturacion Electrónica")))
                        {
                            //Valdiando que Existan
                            if (Validacion.ValidaOrigenDatos(dtRef))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtRef.Rows)
                                {
                                    //Validando que Exista el Registro
                                    if (Convert.ToInt32(dr["Id"]) > 0)
                                    {
                                        //Instanciando Observación
                                        using (SAT_CL.Global.Referencia comentario = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                        {
                                            //Validando que exista la Referencia
                                            if (comentario.habilitar)
                                            {
                                                //Editamos Referencia
                                                resultado = SAT_CL.Global.Referencia.EditaReferencia(comentario.id_referencia, txtComentario.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Terminamos el Ciclo
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                //Insertando Referencia
                                resultado = SAT_CL.Global.Referencia.InsertaReferencia(objFacturaFacturacion.id_factura_electronica, 119, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 119, "Comentario", 0, "Facturacion Electrónica"),
                                            txtComentario.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                        }
                    }
                    else
                        //Establecemos Error
                        resultado = new RetornoOperacion("No existe Registró Facturación Electrónica");
                }
            }

            //Devolvemos Valor Return
            return resultado;
        }
        /// <summary>
        /// Evento generado al dar click en Comentario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbComentario_Click(object sender, EventArgs e)
        {
            if (gvServicioxFacturar.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicioxFacturar, sender, "lnk", false);
                //Limpiando comentario
                txtComentario.Text = "";
                uptxtComentario.Update();
                //Mostramos Ventana
                alternaVentanaModal("Comentario", gvServicioxFacturar);
            }
        }
        /// <summary>
        /// Realiza la descarga del XML del comprobante
        /// </summary>
        private void descargarXML()
        {
            //Instanciamos Relación
            using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(Convert.ToInt32(gvServicioxFacturar.SelectedDataKey["NoFactura"])))))
            {
                //Validamos que exista la facturación electrónica
                if (objFacturaFacturacion.id_factura_electronica33 > 0)
                {
                    //Instanciando registro en sesión
                    using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaFacturacion.id_factura_electronica33))
                    {
                        //Si existe y está generado
                        if (c.bit_generado)
                        {
                            //Obteniendo bytes del archivo XML
                            byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);
                            //Realizando descarga de archivo
                            if (cfdi_xml.Length > 0)
                            {
                                //Instanciando al emisor
                                using (SAT_CL.Global.CompaniaEmisorReceptor em = new SAT_CL.Global.CompaniaEmisorReceptor(c.id_compania_emisor))
                                    TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio), TSDK.Base.Archivo.ContentType.binary_octetStream);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Mostrando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            //Inicializando GridView
            Controles.InicializaGridview(gvServicioxFacturar);
            //Invocando Método de Carga
            cargaCatalogos();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            //TODO : Catalogos del Encabezado de Registro FE
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 3195);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPago, 185, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoComrobante, 186, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Sucursales
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Catalogos de Tamaño  Referencias de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoReferencias, "", 26);
        }
        /// <summary>
        /// Método encargado de Buscar los Serrvicio xFacturar
        /// </summary>
        private void servicioxFacturar()
        {
            //Obteniendo Fechas
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;
            //Validando que se Incluyan las Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }
            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtServiciosxFacturar = SAT_CL.CXC.Reporte.ObtieneReporteServiciosxFacturarV3_3(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtClienteFS.Text, "ID:", 1)), fec_ini, fec_fin,
                                                        Convert.ToInt32(Cadena.VerificaCadenaVacia(txtFolioFS.Text, "0")), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                        txtReferencia.Text, txtServicioFS.Text, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEconomico.Text, "ID:", 1)), txtPorte.Text, chkSinRegistrar.Checked, chkRegistradas.Checked, chkTimbradas.Checked))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtServiciosxFacturar))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvServicioxFacturar, dtServiciosxFacturar, "NoFactura-IdServicio", "", true, 1);
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServiciosxFacturar, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvServicioxFacturar);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Método encargado de Sumar los Totales
        /// </summary>
        private void sumaTotales()
        {
            //Validando que exista la Suma
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvServicioxFacturar.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SubTotal)", "")));
                gvServicioxFacturar.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Trasladado)", "")));
                gvServicioxFacturar.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Retenido)", "")));
                gvServicioxFacturar.FooterRow.Cells[15].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
            }
            else
            {
                //Mostrando Totales
                gvServicioxFacturar.FooterRow.Cells[12].Text =
                gvServicioxFacturar.FooterRow.Cells[13].Text =
                gvServicioxFacturar.FooterRow.Cells[14].Text =
                gvServicioxFacturar.FooterRow.Cells[15].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "EdicionConceptos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");
                    break;
                case "RegistrarFacturacionElectronica":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");
                    break;
                case "ConfirmacionEliminacionCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionEliminarCFDI", "confirmacionEliminarCFDI");
                    break;
                case "TimbrarFacturacionElectronica":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");
                    break;
                case "CancelacionCancelarCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");
                    break;
                case "ConfirmacionAddenda":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionAddenda", "confirmacionAddenda");
                    break;
                case "Addenda":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");
                    break;
                case "ReferenciasServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "referenciasServicioModal", "referenciasServicio");
                    break;
                case "Comentario":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoComentario", "confirmacionComentario");
                    break;
                case "ReferenciasRegistro":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaReferencias", "ventanaReferencias");
                    break;
                case "Producto":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoProducto", "Producto");
                    break;
                case "encabezadoServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "encabezadoServicioModal", "encabezadoServicio");
                    break;
            }
        }
        #endregion
    }
}