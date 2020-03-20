using SAT_CL;
using SAT_CL.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucFacturadoV3_3 : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Objeto "Facturado"
        /// </summary>
        private Facturado objFacturado;
        /// <summary>
        /// Atributo Público encargado de Almacenar el Id de la Factura
        /// </summary>
        public int idFactura { get { return this.objFacturado.id_factura; } }

        private int idServicio;
        private DataTable _dT;
        private DataTable _dTReferenciasViaje;
        private DataTable _dtRefacturaciones;

        /// <summary>
        /// Propiedad TabIndex del Control de Usuario
        /// </summary>
        public short TabIndex
        {
            set
            {   //Encabezado
                txtNofactura.TabIndex =
                ddlEstatus.TabIndex =
                txtFechaTC.TabIndex =
                txtFechaFactura.TabIndex =
                txtTotalFactura.TabIndex =
                ddlMonedaFactura.TabIndex =
                txtTotalPesosFactura.TabIndex =
                ddlCondicionPagoFactura.TabIndex =
                btnAceptarFactura.TabIndex =
                btnCancelarFactura.TabIndex = value;
            }
            get { return txtNofactura.TabIndex; }
        }
        /// <summary>
        /// Propiedad Enabled del Control de Usuario
        /// </summary>
        public bool Enabled
        {
            set
            {   //Controles
                ddlMonedaFactura.Enabled =
                ddlCondicionPagoFactura.Enabled =
                    //Botones
                btnAceptarFactura.Enabled =
                btnCancelarFactura.Enabled = value;
            }
            get { return txtNofactura.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento desencadenado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
            if (!(Page.IsPostBack))

                //Invocando Método de Carga
                cargaCatalogos();
            else
                //Recuperando Valor de los Atributos
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
        /// Evento producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarFactura_Click(object sender, EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarFactura != null)

                //Inicializando
                OnClickGuardarFactura(new EventArgs());
            return;
        }
        /// <summary>
        /// Evento producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarFactura_Click(object sender, EventArgs e)
        {
            //Validando que exista un Id
            if (objFacturado.id_factura != 0)

                //Inicializando Control con Id
                inicializaControles(objFacturado.id_factura, this.idServicio);
            else
                //Inicializando Control
                InicializaControl(this.idServicio);
        }
        /// <summary>
        /// Declarando el Evento ClickGuardarFactura
        /// </summary>
        public event EventHandler ClickGuardarFactura;
        /// <summary>
        /// Método que manipula el Evento "Guardar Factura"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarFactura(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarFactura != null)

                //Invocando al Delegado
                ClickGuardarFactura(this, e);
        }
        /// <summary>
        /// Delegado de evento Click en botón de aplicar tarifa
        /// </summary>
        public event EventHandler ClickAplicarTarifa;
        /// <summary>
        /// Evento Click Aplicar tarifa
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickAplicarTarifa(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickAplicarTarifa != null)

                //Invocando al Delegado
                ClickAplicarTarifa(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAplicarTarifa_Click(object sender, EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickAplicarTarifa != null)

                //Inicializando
                OnClickAplicarTarifa(new EventArgs());
        }
        /// <summary>
        /// Evento Producido al dar click en el LinkButton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarFactura_Click(object sender, EventArgs e)
        {
            //Validando que exista un Id
            if (objFacturado.id_factura != 0)
            {
                //Inicializando Control
                inicializaControles(objFacturado.id_factura, idServicio);

                //Habilitando Controles
                habilitaControles(true);
            }
            else
                //Mostrando Mensaje
                lblErrorFactura.Text = "Debe de existir la Factura";
        }
        /// Evento generado al dar click en Bitácora 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacora(object sender, EventArgs e)
        {
            //Validamos que existan Registros
            if (this.objFacturado.id_factura > 0)
            {
                //Mostramos Bitácora
                inicializaBitacora(this.objFacturado.id_factura.ToString(), "9", "Bitácora Facturado");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMonedaFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Declarando Variables Auxiliares
            decimal valor_tc = 0;
            DateTime fecha = DateTime.MinValue;

            //Validando que la Moneda no sea MXN
            if (ddlMonedaFactura.SelectedValue != "1")
            {
                //Instanciando Factura
                using (Facturado fac = new Facturado(this.idFactura))
                {
                    //Validando que exista la Factura
                    if (fac.id_factura > 0)
                    {
                        //Obteniendo Fecha sin Hora
                        DateTime.TryParse(fac.fecha_tipo_cambio.ToString("dd/MM/yyyy"), out fecha);

                        //Instanciando Tipo de Cambio
                        using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                            Convert.ToByte(ddlMonedaFactura.SelectedValue), fecha, 0))
                        {
                            //Validando que exista el Tipo de Cambio
                            if (tc.id_tipo_cambio > 0)

                                //Asignando Valor de Cambio
                                valor_tc = tc.valor_tipo_cambio;
                            else
                                //Asignando Valor de Cambio
                                valor_tc = 0;
                        }
                    }
                    else
                    {
                        //Obteniendo Fecha sin Hora
                        DateTime.TryParse(txtFechaTC.Text, out fecha);

                        //Instanciando Tipo de Cambio
                        using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                            Convert.ToByte(ddlMonedaFactura.SelectedValue), fecha, 0))
                        {
                            //Validando que exista el Tipo de Cambio
                            if (tc.id_tipo_cambio > 0)

                                //Asignando Valor de Cambio
                                valor_tc = tc.valor_tipo_cambio;
                            else
                                //Asignando Valor de Cambio
                                valor_tc = 0;
                        }
                    }
                }
            }

            //Mostrando Total en Pesos
            txtTotalPesosFactura.Text = string.Format("{0:#,###,###,###.00}", Convert.ToDecimal(txtTotalFactura.Text == "" ? "0" : txtTotalFactura.Text) * valor_tc);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Fecha de Tipo de Cambio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFechaTC_TextChanged(object sender, EventArgs e)
        {
            //Declarando Variables Auxiliares
            decimal valor_tc = 0;
            DateTime fecha = DateTime.MinValue;

            //Validando que la Moneda no sea MXN
            if (ddlMonedaFactura.SelectedValue != "1")
            {
                //Instanciando Factura
                using (Facturado fac = new Facturado(this.idFactura))
                {
                    //Validando que exista la Factura
                    if (fac.id_factura > 0)
                    {
                        //Obteniendo Fecha sin Hora
                        DateTime.TryParse(fac.fecha_tipo_cambio.ToString("dd/MM/yyyy"), out fecha);

                        //Instanciando Tipo de Cambio
                        using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                            Convert.ToByte(ddlMonedaFactura.SelectedValue), fecha, 0))
                        {
                            //Validando que exista el Tipo de Cambio
                            if (tc.id_tipo_cambio > 0)

                                //Asignando Valor de Cambio
                                valor_tc = tc.valor_tipo_cambio;
                            else
                                //Asignando Valor de Cambio
                                valor_tc = 0;
                        }
                    }
                    else
                    {
                        //Obteniendo Fecha sin Hora
                        DateTime.TryParse(txtFechaTC.Text, out fecha);

                        //Instanciando Tipo de Cambio
                        using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                            Convert.ToByte(ddlMonedaFactura.SelectedValue), fecha, 0))
                        {
                            //Validando que exista el Tipo de Cambio
                            if (tc.id_tipo_cambio > 0)

                                //Asignando Valor de Cambio
                                valor_tc = tc.valor_tipo_cambio;
                            else
                                //Asignando Valor de Cambio
                                valor_tc = 0;
                        }
                    }
                }
            }

            //Mostrando Total en Pesos
            txtTotalPesosFactura.Text = string.Format("{0:#,###,###,###.00}", Convert.ToDecimal(txtTotalFactura.Text == "" ? "0" : txtTotalFactura.Text) * valor_tc);
        }
        /// <summary>
        /// Evento generado al Cambiar el Tipo de Comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoComrobante_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*/De acuerdo al Tipo de Cmprobante es Ingreso 
            switch (Convert.ToByte(ddlTipoComrobante.SelectedValue))
            {
                //El tipo de Comprobante es Ingreso
                case 1:
                    //Instanciando Factura
                    using (Facturado fac = new Facturado(this.idFactura))
                    {
                        //Si es una Factura de Servicios
                        if (fac.id_servicio != 0)
                        {
                            //Instanciamos Servicio
                            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(objFacturado.id_servicio))
                            {
                                //Cargando Cuentas Pago 
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 42, "NO IDENTIFICADO", 25, "", objServicio.id_cliente_receptor, "");
                            }
                        }
                        else
                        {
                            //Instanciamos Facturación otros
                            using (SAT_CL.Facturacion.FacturacionOtros objFacturacionOtros = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(this.idFactura))
                            {
                                //Cargando Cuentas Pago 
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 42, "NO IDENTIFICADO", 25, "", objFacturacionOtros.id_cliente_receptor, "");
                            }
                        }
                    }

                    break;
                default:
                    //Instanciando Factura
                    using (Facturado fac = new Facturado(this.idFactura))
                    {
                        //Si es una Factura de Servicios
                        if (fac.id_servicio != 0)
                        {
                            //Instanciamos Servicio
                            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(objFacturado.id_servicio))
                            {
                                //Cargando Cuentas Pago 
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 42, "NO IDENTIFICADO", 25, "", objServicio.id_compania_emisor, "");
                            }
                        }
                        else
                        {
                            //Instanciamos Facturación otros
                            using (SAT_CL.Facturacion.FacturacionOtros objFacturacionOtros = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(this.idFactura))
                            {
                                //Cargando Cuentas Pago 
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 42, "NO IDENTIFICADO", 25, "", objFacturacionOtros.id_compania_emisora, "");
                            }
                        }
                    }
                    break;

            }//*/
        }
        /// <summary>
        /// Evento generado al  Mostrar la Refacturaciones de un Comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerRefacturacion_Click(object sender, EventArgs e)
        {
            //Mostramos Venata Modal
            alternaVentanaModal("Refacturacion", lkbVerRefacturacion);

            //Mostramos Refacturaciones
            cargaRefacturaciones();
        }

        #endregion

        #region Eventos Facturación Electrónica

        /// <summary>
        /// Evento generado al dar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferencias_Click(object sender, EventArgs e)
        {

            //Inicializamos Ventana de Referencia
            inicializaReferencias(objFacturado.id_factura.ToString(), "9", "Facturado");

        }
        /// <summary>
        /// Evento generado al dar clik en link de Registrar Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkRegistrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Abre Ventana Modal
            ScriptServer.AlternarVentana(lkbRegistrarFacturaElectronica, uplkbRegistrarFacturaElectronica.GetType(), "AbrirVentana", "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");

            /*/Instanciando Factura
            using (Facturado fac = new Facturado(this.idFactura))
            {
                //Si es una Factura de Servicios
                if (fac.id_servicio != 0)
                {
                    //Instanciamos Servicio
                    using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(objFacturado.id_servicio))
                    {
                        //Cargando Cuentas Pago 
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 42, "NO IDENTIFICADO", 25, "", objServicio.id_cliente_receptor, "");
                    }
                }
                else
                {
                    //Instanciamos Facturación otros
                    using (SAT_CL.Facturacion.FacturacionOtros objFacturacionOtros = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(this.idFactura))
                    {
                        //Cargando Cuentas Pago 
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 42, "NO IDENTIFICADO", 25, "", objFacturacionOtros.id_cliente_receptor, "");
                    }
                }
            }//*/
            //Inicializamos Valores
            inicializaValoresRegistroFacturacionElectronica();
        }

        /// <summary>
        ///  Evento generado al dar clik en link en Timbrar Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkTimbrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Abre Ventana Modal
            ScriptServer.AlternarVentana(lkbTimbrarFacturaElectronica, uplkbTimbrarFacturaElectronica.GetType(), "AbrirVentana", "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");

            //Inicializa Valores
            inicializaValoresTimbrarFacturacionElectronica();
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
                case "refacturacion":
                    //Cerrando ventana modal 
                    alternaVentanaModal("Refacturacion", lnk);
                    break;
            }
        }
        /// <summary>
        /// Evento generado al dar click en Addenda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAddendaFacturaElectronica_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbAddendaFacturaElectronica, uplkbAddendaFacturaElectronica.GetType(), "CerrarVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");

            //Instanciando Comprobante
            using (FacturadoFacturacion Fac = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura)))
            {
                //Validando Relación
                if (Fac.habilitar && Fac.id_factura_electronica > 0)
                {
                    //Validamos que exista Relación
                    using (SAT_CL.FacturacionElectronica.Comprobante comp = new SAT_CL.FacturacionElectronica.Comprobante(Fac.id_factura_electronica))
                    {
                        //CargaCatalogo
                        CapaNegocio.m_capaNegocio.CargaCatalogo
                             (ddlAddenda, 49, "Ninguno", comp.id_compania_emisor, "", comp.id_compania_receptor, "");
                    }
                }
                //Validando Relación
                else if (Fac.habilitar && Fac.id_factura_electronica33 > 0)
                {
                    //Validamos que exista Relación
                    using (SAT_CL.FacturacionElectronica33.Comprobante comp = new SAT_CL.FacturacionElectronica33.Comprobante(Fac.id_factura_electronica33))
                    {
                        //CargaCatalogo
                        CapaNegocio.m_capaNegocio.CargaCatalogo
                             (ddlAddenda, 49, "Ninguno", comp.id_compania_emisor, "", comp.id_compania_receptor, "");
                    }
                }
                else
                    //Instanciando Excepción
                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe la Factura Electronica"), ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
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
        /// Evebto generado al Registrar la Factura Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegistrarFacturaElectronica_Click(object sender, EventArgs e)
        {

            //Si existe la Tabla de Referencias
            if (this._dTReferenciasViaje != null)
            {
                //Eliminamos Tabla
                this._dTReferenciasViaje = null;
            }
            //Craga GV Referencias
            cargaReferencias();
            //Si existen Referencias
            if (this._dTReferenciasViaje != null)
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
        /// Registro de la Facturación Electronica
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion RegistraFacturacionElectronica()
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que exista la Factura
            if (objFacturado.id_factura > 0)
            {
                //Declarando Nodos Relacionados
                List<Tuple<int, byte, decimal, decimal>> cfdi_rel = new List<Tuple<int, byte, decimal, decimal>>();
                //Registramos Factura
                resultado = objFacturado.ImportaFacturadoComprobante_V3_3(Convert.ToByte(ddlFormaPago.SelectedValue), Convert.ToInt32(ddlUsoCFDI.SelectedValue), Convert.ToByte(ddlMetodoPago.SelectedValue), Convert.ToByte(ddlTipoComrobante.SelectedValue), Convert.ToInt32(ddlSucursal.SelectedValue),
                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, 
                            obtieneReferencias().TrimEnd(','), cfdi_rel);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Si existen Referencias
                    if (this._dTReferenciasViaje != null)
                    {
                        //Cerramo Ventana Modal
                        alternaVentanaModal("ReferenciasRegistro", btnRegistrarFE);
                    }
                    else
                    {
                        //Cerramos Ventana Modal de Registro
                        alternaVentanaModal("RegistrarFacturacionElectronica", btnRegistrarFacturaElectronica);
                    }
                }
            }

            return resultado;
        }
        /// <summary>
        /// Método encargado de Buscar las Referencias de Viaje
        /// </summary>
        private void cargaReferencias()
        {

            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtReferencia = SAT_CL.Facturacion.Reporte.ObtienesDatosReferenciasFacturado(objFacturado.id_factura))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtReferencia))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvReferencias, dtReferencia, "Id", "", false, 1);

                    //Añadiendo Tabla  
                    this._dTReferenciasViaje = dtReferencia;
                }
                else
                {
                    //Eliminando Tabla de Session
                    this._dTReferenciasViaje = null;
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
            Controles.CambiaTamañoPaginaGridView(gvReferencias, this._dTReferenciasViaje, Convert.ToInt32(ddlTamanoReferencias.SelectedValue));
        }

        /// <summary>
        /// Evento producido al cambiar el indice de página del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvReferencias, this._dTReferenciasViaje, e.NewPageIndex);
        }

        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoReferencias.Text = Controles.CambiaSortExpressionGridView(gvReferencias, this._dTReferenciasViaje, e.SortExpression);
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
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "RegistrarFacturacionElectronica":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");
                    break;
                case "Comentario":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoComentario", "confirmacionComentario");
                    break;
                case "ReferenciasRegistro":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaReferencias", "ventanaReferencias");
                    break;
                case "Refacturacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoVentanaRefacturaciones", "ventanaRefacturaciones");
                    break;
                case "TimbrarFacturaElectronica":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");
                    break;

            }
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

            //Registramos Factura
            resultado = objFacturado.TimbraFacturadoComprobante_V3_3(txtSerie.Text, chkOmitirAddenda.Checked, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                                     HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validamos Resultado
            if (resultado.OperacionExitosa)
                //Cerramos Ventana Modal
                alternaVentanaModal("TimbrarFacturaElectronica", btnAceptarTimbrarFacturacionElectronica);

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(btnAceptarTimbrarFacturacionElectronica, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento generado al cerrar la ventana de Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarRegistarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Cerramo Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarRegistarFacturacionElectronica, uplkbCerrarRegistarFacturacionElectronica.GetType(), "CerrarVentanaModal", "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");
        }

        /// <summary>
        /// Evento generdo al cerrar la venta de Timbrar Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarTimbrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarTimbrarFacturacionElectronica, uplkbCerrarTimbrarFacturacionElectronica.GetType(), "CerrarVentana", "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");
        }
        /// <summary>
        /// Exportación de de contenido de gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarRefacturacion_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(this._dtRefacturaciones, "*".ToArray());
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRefacturacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvRefacturacion, this._dtRefacturaciones, Convert.ToInt32(ddlTamanoReferencias.SelectedValue));
        }

        /// <summary>
        /// Evento producido al cambiar el indice de página del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRefacturacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvRefacturacion, this._dtRefacturaciones, e.NewPageIndex);
        }

        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRefacturacion_Sorting(object sender, GridViewSortEventArgs e)
        {

            lblOrdenadoRefacturacion.Text = Controles.CambiaSortExpressionGridView(gvRefacturacion, this._dtRefacturaciones, e.SortExpression);
        }
        /// <summary>
        /// Evento generdo al cerrar la venta de Timbrar Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAddendaComprobante_ClickEliminar(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(this, this.GetType(), "CerrarVentana", "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");
        }

        /// <summary>
        /// Evento generado al dar click en Aceptar Addenda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarAddenda_Click(object sender, EventArgs e)
        {
            //Validamos que exista Addenda Seleccionada
            if (ddlAddenda.SelectedValue != "0")
            {
                //Instanciando Comprobante
                using (FacturadoFacturacion Fac = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura)))
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
                            //Cerrando Ventana Modal
                            ScriptServer.AlternarVentana(btnAceptarAddenda, btnAceptarAddenda.GetType(), "CerrarVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");
                            //Abrir Ventana Modal
                            ScriptServer.AlternarVentana(btnAceptarAddenda, upbtnAceptarAddenda.GetType(), "AbrirVentana", "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");
                        }
                        else if (cfdi33.id_comprobante33 > 0)
                        {
                            //Inicializamos Control
                            wucAddendaComprobante.InicializaControl("3.3", cfdi33.id_comprobante33, Convert.ToInt32(ddlAddenda.SelectedValue));
                            //Cerrando Ventana Modal
                            ScriptServer.AlternarVentana(btnAceptarAddenda, btnAceptarAddenda.GetType(), "CerrarVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");
                            //Abrir Ventana Modal
                            ScriptServer.AlternarVentana(btnAceptarAddenda, upbtnAceptarAddenda.GetType(), "AbrirVentana", "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se encontró registró de Factura Electrónica"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }

        /// <summary>
        /// Evento generado al Cerrar Addenda Comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarwucAddendaComprobante_Click(object sender, EventArgs e)
        {
            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarwucAddendaComprobante, uplkbCerrarwucAddendaComprobante.GetType(), "AbrirVentana", "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");
        }

        /// <summary>
        /// Evento generado al Cerrar Addenda Comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarAddendaComprobante_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarAddenda, lkbCerrarAddenda.GetType(), "AbrirVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");
        }
        /// <summary>
        /// Evento generado al cambiar al Forma de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        /// <summary>
        /// Evento generado al Abrir la venta E-mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEmail_Click(object sender, EventArgs e)
        {
            //Inicializando contenido de controles de envío de correo
            //Instanciando comprobante de interés
            using (SAT_CL.FacturacionElectronica33.Comprobante comp = new SAT_CL.FacturacionElectronica33.Comprobante(FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(objFacturado.id_factura)))
            {
                //Si hay comprobante timbrado
                if (comp.bit_generado)
                {
                    //Instanciando compañía de interés
                    using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(comp.id_compania_emisor))
                    {
                        string destinatarios = "";
                        //Cargando contactos (Destinatarios)
                        using (DataTable mitContactos = SAT_CL.Global.Referencia.CargaReferencias(comp.id_compania_receptor, 25, 2058))
                        {
                            //Si hay elementos
                            if (mitContactos != null)
                            {
                                foreach (DataRow r in mitContactos.Rows)
                                {
                                    //Si ya existe contenido en el control
                                    if (destinatarios != "")
                                        destinatarios = destinatarios + ";\r\n" + r.Field<string>("Valor");
                                    //De lo contrario
                                    else
                                        destinatarios = r.Field<string>("Valor");
                                }
                            }
                        }

                        //Inicializando control de envío de comprobante
                        wucEmailCFDI.InicializaControl(((SAT_CL.Seguridad.Usuario)Session["usuario"]).email, string.Format("CFDI {0} [{1}]", comp.serie + comp.folio.ToString(), c.rfc), destinatarios,
                            "Los archivos se encuentran adjuntos en este mensaje. Si usted no ha solicitado el envío de este comprobante, por favor contacte a su ejecutivo de cuenta.", comp.id_comprobante33);
                    }
                }
                else
                    ScriptServer.MuestraNotificacion(lkbEmail, "El comprobante no se ha timbrado.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }

            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbEmail, "EnvioEmail", "contenidoConfirmacionEmail", "confirmacionEmail");
        }

        /// Evento generado al cerrar la ventana Moda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEmailCFDI_LkbCerrarEmail_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(this.Page, "EnvioEmail", "contenidoConfirmacionEmail", "confirmacionEmail");

        }
        /// <summary>
        /// Evento generado al descargar el xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbXml_Click(object sender, EventArgs e)
        {
            //Descarga XML
            descargarXML();
        }

        /// <summary>
        /// Evento generado al dar click en Aceptar E-mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEmailCFDI_BtnEnviarEmail_Click(object sender, EventArgs e)
        {
            //Enviando email
            RetornoOperacion resultado = wucEmailCFDI.EnviaEmail(true, true);

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Cerrando ventana modal
                ScriptServer.AlternarVentana(this.Page, "EnvioEmail", "contenidoConfirmacionEmail", "confirmacionEmail");

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento generado al Eliminar un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarCFDI_Click(object sender, EventArgs e)
        {
            eliminaCFDI();
        }

        /// <summary>
        /// Evento generado al Cancelra CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminarCFDI_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnCancelarEliminarCFDI, upbtnCancelarEliminarCFDI.GetType(), "CerrarVentanaModal", "contenidoConfirmacionEliminarCFDI", "confirmacionEliminarCFDI");
        }

        /// <summary>
        /// Evento generado al Aceptar la Cancelación un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            CancelaCFDI();
        }

        /// <summary>
        /// Evento generado al Cancelar  la  Cancelación de un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnCancelarCancelacionCFDI, upbtnCancelarCancelacionCFDI.GetType(), "CerrarVentanaModal", "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");

        }

        /// <summary>
        /// Evento generado al Eliminar un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminarCFDI_Click(object sender, EventArgs e)
        {
            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbEliminarCFDI, uplkbEliminarCFDI.GetType(), "AbrirVentanaModal", "contenidoConfirmacionEliminarCFDI", "confirmacionEliminarCFDI");

        }

        /// <summary>
        /// Evento generado al cancelar CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCancelarCFDI_Click(object sender, EventArgs e)
        {
            //Instanciando Aplicaciones
            using (DataTable dtAplicaciones = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(0, this.idFactura))
            {
                //Validando que existan Aplicaciones
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAplicaciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvAplicaciones, dtAplicaciones, "Id", "", true, 2);

                    //Guardando Datos en ViewState
                    this._dT = dtAplicaciones;

                    //Cambiando a Vista de Aplicaciones
                    mtvCancelacionCFDI.ActiveViewIndex = 1;
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvAplicaciones);

                    //Guardando Datos en ViewState
                    this._dT = null;

                    //Cambiando a Vista de Mensaje
                    mtvCancelacionCFDI.ActiveViewIndex = 0;
                }
            }

            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbCancelarCFDI, uplkbCancelarCFDI.GetType(), "AbrirVentanaModal", "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");
        }

        /// <summary>
        /// Evento generado al exportar a PDF un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbPDF_Click(object sender, EventArgs e)
        {
            //Obteniendo Ruta
            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucFacturadoV3_3.ascx", "~/RDLC/Reporte.aspx");
            //Instanciamos Relación
            using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura))))
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
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteV33", objFacturaFacturacion.id_factura_electronica33), "ComprobanteV33", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        }
                    }
                }

            }
        }
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
        /// Evento generado al dar click en Comentario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbComentario_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana
            alternaVentanaModal("Comentario", lkbComentario);
        }

        /// <summary>
        /// Actualiza el Comentario de la Factura
        /// </summary>
        private RetornoOperacion actualizaComentario()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Factura
            using (Facturado facturado = new Facturado(objFacturado.id_factura))
            {
                //Instanciamos Relación
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura))))
                {
                    //Validamos Factura Electrónica
                    if (objFacturaFacturacion.id_facturado_facturacion > 0)
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
                            {
                                //Insertando Referencia
                                resultado = SAT_CL.Global.Referencia.InsertaReferencia(objFacturaFacturacion.id_factura_electronica33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Comentario", 0, "Facturacion Electrónica"),
                                            txtComentario.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                            }
                        }
                    }
                    else
                    {
                        //Establecemos Error
                        resultado = new RetornoOperacion("No existe Registró Facturación Electrónica");
                    }
                }
            }
            //Devolvemos Valor Return
            return resultado;
        }
        /// <summary>
        /// Evento Generado al Dar Click en el link "Ver Comprobante"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerComprobante_Click(object sender, EventArgs e)
        {
            //Instanciamos Relación
            using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura))))
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
                            //Obteniendo Ruta
                            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucFacturadoV3_3.ascx", "~/FacturacionElectronica33/ComprobanteV33.aspx");

                            //Estatus de Lectura
                            Session["estatus"] = Pagina.Estatus.Lectura;

                            //Instanciando nueva ventana de navegador para apertura de registro
                            Response.Redirect(string.Format("{0}?idRegistro={1}", urlReporte, objComprobante.id_comprobante33));
                        }
                    }
                }
                else
                    //Mostrando Notificación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No existe la Factura Electrónica", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Método encargado de Obtener el Cliente dependiendo del Origen
        /// </summary>
        /// <returns></returns>
        private int obtieneClienteFacturado()
        {
            //Declarando Objeto de Retorno
            int idCliente = 0;

            //Validando Servicio
            if (this.idServicio > 0)
            {
                //Instancando Servicio
                using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(this.idServicio))
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
                using (SAT_CL.Facturacion.FacturacionOtros fo = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(this.idFactura))
                {
                    //Validando Existencia del FO
                    if (fo.habilitar)

                        //Asignando Cliente
                        idCliente = fo.id_cliente_receptor;
                }
            }

            //Devolviendo Resultado Obtenido
            return idCliente;
        }

        #endregion

        #region Métodos Facturación Electrónica

        /// <summary>
        /// Inicializa Valores para el registro a Facturación Electrónica
        /// </summary>
        private void inicializaValoresRegistroFacturacionElectronica()
        {
            using (SAT_CL.Global.CompaniaEmisorReceptor cliente = new SAT_CL.Global.CompaniaEmisorReceptor(obtieneClienteFacturado()))
            {
                if (cliente.habilitar)
                {
                    //Cargando Catalogo
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 108, "Ninguno", cliente.id_compania_emisor_receptor, "", 0, "");

                    //Inicializamos Valores para Registrò de l FE
                    ddlTipoComrobante.SelectedValue = "1";
                    lblErrorFacturacionElectronica.Text = "";

                    //Obteniendo Items
                    ListItem liFormaPago = ddlFormaPago.Items.FindByText("Por definir [99]");
                    ListItem liMetodoPago = ddlMetodoPago.Items.FindByText("Pago en parcialidades o diferido");
                    ListItem liUsoCFDI = cliente.id_uso_cfdi > 0 ? ddlUsoCFDI.Items.FindByValue(cliente.id_uso_cfdi.ToString()) : ddlUsoCFDI.Items.FindByText("[P01] Por definir");

                    //Validando Item FP
                    if (liFormaPago != null)
                        //Asignando Forma de Pago
                        ddlFormaPago.SelectedValue = liFormaPago.Value;
                    //Validando Item MP
                    if (liMetodoPago != null)
                        //Asignando Método de Pago
                        ddlMetodoPago.SelectedValue = liMetodoPago.Value;
                    //Validando Item Uso CFDI
                    if (liUsoCFDI != null)
                        //Asignando Uso del CFDI
                        ddlUsoCFDI.SelectedValue = liUsoCFDI.Value;
                }
            }
            
        }

        /// <summary>
        /// Inicializa Valores para el Timbrado de la Facturación Electrónica
        /// </summary>
        private void inicializaValoresTimbrarFacturacionElectronica()
        {
            txtSerie.Text = "";
        }

        /// <summary>
        /// Cancelar CFDI
        /// </summary>
        private void CancelaCFDI()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Relación
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura))))
                {
                    //Validamos Factura Electrónica
                    if (objFacturaFacturacion.habilitar && objFacturaFacturacion.id_factura_electronica33 > 0)
                    {
                        //Instanciamos Comprobamte
                        using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaFacturacion.id_factura_electronica33))
                        {
                            //Validando Comprobante
                            if (objCompobante.habilitar)
                            {
                                //Enviamos link
                                resultado = objCompobante.CancelacionPendiente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando Operación
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertando Referencia
                                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(objFacturaFacturacion.id_factura_electronica33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Motivo Cancelación", 0, "Facturacion Electrónica"),
                                                txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                                }
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No se puede recuperar la Factura Electronica v3.3");
                        }
                    }
                    else if (objFacturaFacturacion.habilitar && objFacturaFacturacion.id_factura_electronica > 0)
                    {
                        //Instanciamos Comprobamte
                        using (SAT_CL.FacturacionElectronica.Comprobante objCompobante = new SAT_CL.FacturacionElectronica.Comprobante(objFacturaFacturacion.id_factura_electronica))
                        {
                            //Validando Comprobante
                            if (objCompobante.habilitar)
                            {
                                //Enviamos link
                                resultado = objCompobante.CancelaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando Operación
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertando Referencia
                                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(objFacturaFacturacion.id_factura_electronica, 119, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 119, "Motivo Cancelación", 0, "Facturacion Electrónica"),
                                                txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                                }
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No se puede recuperar la Factura Electronica v3.2");
                        }
                    }
                    else
                        //Establecemos Error
                        resultado = new RetornoOperacion("No existe la Factura Electrónica");

                    //Validando Operación Exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando Aplicaciones
                        using (DataTable dtAplicaciones = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(0, this.idFactura))
                        {
                            //Validando que existan Aplicaciones
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAplicaciones))
                            {
                                //Recorriendo Registros
                                foreach (DataRow dr in dtAplicaciones.Rows)
                                {
                                    //Instanciando Aplicacion de la Factura
                                    using (SAT_CL.CXC.FichaIngresoAplicacion fia = new SAT_CL.CXC.FichaIngresoAplicacion(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista la Aplicación
                                        if (fia.id_ficha_ingreso_aplicacion > 0)
                                        {
                                            //Deshabilitando Ficha de Ingreso
                                            resultado = fia.DeshabilitarFichaIngresoAplicacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando Operación Exitosa
                                            if (!resultado.OperacionExitosa)

                                                //Terminando Ciclo
                                                break;
                                            else
                                            {
                                                //Instanciando Ficha de Ingreso
                                                using (SAT_CL.Bancos.EgresoIngreso fi = new SAT_CL.Bancos.EgresoIngreso(fia.id_egreso_ingreso))
                                                {
                                                    //Validando que exista el Registro
                                                    if (fi.habilitar)
                                                    {
                                                        //Obteniendo Facturas Aplicadas
                                                        using (DataTable dtAplicacionesFicha = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(fi.id_egreso_ingreso, 0))
                                                        {
                                                            //Si no existen Aplicaciones
                                                            if (!TSDK.Datos.Validacion.ValidaOrigenDatos(dtAplicacionesFicha))
                                                            {
                                                                //Actualizando Estatus de la Ficha
                                                                resultado = fi.ActualizaFichaIngresoEstatus(SAT_CL.Bancos.EgresoIngreso.Estatus.Capturada, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando Operación Correcta
                                                                if (resultado.OperacionExitosa)

                                                                    //Terminando Ciclo
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //Validando Desaplicación Exitosa
                                if (resultado.OperacionExitosa)

                                    //Actualizando Estatus de la Factura
                                    resultado = objFacturado.ActualizaEstatusFactura(Facturado.EstatusFactura.Registrada, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                            else
                                //Instanciando Factura
                                resultado = new RetornoOperacion(this.idFactura);

                            //Validando Operación Exitosa
                            if (resultado.OperacionExitosa)

                                //Completando Transacción
                                trans.Complete();
                        }
                    }

                    //Cerrar Ventana Modal
                    ScriptServer.AlternarVentana(btnAceptarCancelacionCFDI, upbtnAceptarCancelacionCFDI.GetType(), "CerrarVentanaModal", "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");

                }
            }

            //Mostrando Mensaje de Operación
            lblErrorFactura.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Eliminar CFDI
        /// </summary>
        private void eliminaCFDI()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Relación
            using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura))))
            {
                //Validamos que existan Relación
                if (objFacturaFacturacion.id_factura_electronica33 > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaFacturacion.id_factura_electronica33))
                    {
                        //Validando Comprobante v3.3
                        if (objCompobante.habilitar)

                            //Enviamos link
                            resultado = objCompobante.DeshabilitaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No se puede recuperar el Comprobante v3.3");
                    }
                }
                else if (objFacturaFacturacion.id_factura_electronica > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica.Comprobante objCompobante = new SAT_CL.FacturacionElectronica.Comprobante(objFacturaFacturacion.id_factura_electronica))
                    {
                        //Validando Comprobante v3.3
                        if (objCompobante.habilitar)

                            //Enviamos link
                            resultado = objCompobante.DeshabilitaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No se puede recuperar el Comprobante v3.2");
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("No existe la Factura Electrónica.");
                
                //Cerrar Ventana Modal
                ScriptServer.AlternarVentana(btnAceptarEliminarCFDI, upbtnAceptarEliminarCFDI.GetType(), "CerrarVentanaModal", "contenidoConfirmacionEliminarCFDI", "confirmacionEliminarCFDI");

            }

            //Mostrando Excepción
            lblErrorFactura.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Realiza la descarga del XML del comprobante
        /// </summary>
        private void descargarXML()
        {
            //Instanciamos Relación
            using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura))))
            {
                //Validamos que exista la facturación electrónica
                if (objFacturaFacturacion.habilitar && objFacturaFacturacion.id_factura_electronica33 > 0)
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
                                    TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", !em.nombre_corto.Equals("") ? em.nombre_corto : em.rfc, c.serie, c.folio), TSDK.Base.Archivo.ContentType.binary_octetStream);
                                //Actualizamos Update Panel
                                ulkbXml.Update();
                            }
                        }
                    }
                }
                else //Validamos que exista la facturación electrónica
                    if (objFacturaFacturacion.habilitar && objFacturaFacturacion.id_factura_electronica > 0)
                    {
                        //Instanciando registro en sesión
                        using (SAT_CL.FacturacionElectronica.Comprobante c = new SAT_CL.FacturacionElectronica.Comprobante(objFacturaFacturacion.id_factura_electronica))
                        {
                            //Si existe y está generado
                            if (c.generado)
                            {
                                //Obteniendo bytes del archivo XML
                                byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);

                                //Realizando descarga de archivo
                                if (cfdi_xml.Length > 0)
                                {
                                    //Instanciando al emisor
                                    using (SAT_CL.Global.CompaniaEmisorReceptor em = new SAT_CL.Global.CompaniaEmisorReceptor(c.id_compania_emisor))
                                        TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio), TSDK.Base.Archivo.ContentType.binary_octetStream);
                                    //Actualizamos Update Panel
                                    ulkbXml.Update();
                                }
                            }
                        }
                    }
            }
        }
        /// <summary>
        /// Método encargado de Cargar las Refacturaciones 
        /// </summary>
        private void cargaRefacturaciones()
        {

            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtRefacturaciones = SAT_CL.Facturacion.Facturado.CargaRefacturacion(objFacturado.id_factura))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtRefacturaciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvRefacturacion, dtRefacturaciones, "Id", "", false, 1);

                    //Añadiendo Tabla  
                    this._dtRefacturaciones = dtRefacturaciones;
                }
                else
                {
                    //Eliminando Tabla de Session
                    this._dTReferenciasViaje = null;
                }
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Público encargado de Inicializar Control dado un Id de Registro
        /// </summary>
        /// <param name="idFactura">Id de Factura</param>
        /// <param name="id_servicio">Id de Servicio</param>
        private void inicializaControles(int idFactura, int id_servicio)
        {
            //Invocando Método de carga
            cargaCatalogos();

            /*/Declarando Variable para el Cliente
            int idCliente = 0;

            //Validando Servicio
            if (id_servicio > 0)
            {
                //Instancando Servicio
                using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(id_servicio))
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
                using (SAT_CL.Facturacion.FacturacionOtros fo = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(idFactura))
                {
                    //Validando Existencia del FO
                    if (fo.habilitar)

                        //Asignando Cliente
                        idCliente = fo.id_cliente_receptor;
                }
            }

            //Cargando Catalogo
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 108, "Ninguno", idCliente, "", 0, "");//*/

            //Invocando Método de Habilitacion de Controles
            habilitaControles(false);

            //Inicializando Objetos
            this.objFacturado = new Facturado(idFactura);
            this.idServicio = id_servicio;

            //Validando que Exista un registro
            if (objFacturado.id_factura != 0)
            {
                //Instancia a la clase FacturadoFcturacion dado un id de Factura
                using (FacturadoFacturacion facturado = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(objFacturado.id_factura))))
                {
                    //Valida que exista una factura electronica
                    if (facturado.id_factura_electronica33 > 0)
                    {
                        //Instancia a la clase comprobante para obtener la serie y folio 
                        using (SAT_CL.FacturacionElectronica33.Comprobante comprobante = new SAT_CL.FacturacionElectronica33.Comprobante(facturado.id_factura_electronica33))
                        {
                            //Validando Comprobante
                            if (comprobante.habilitar)
                            {
                                //Validando Contenido del Comprobante
                                if (comprobante.serie.Equals("") && comprobante.folio.Equals(""))

                                    //Inicializando Control en Cero '0'
                                    txtNofactura.Text = "0";
                                else
                                    //Asigna al control txtNofactura los valores de Serie y Folio de la factura electronica 
                                    txtNofactura.Text = comprobante.serie + ' ' + comprobante.folio;
                            }
                            else
                                //Inicializando Control en Vacio
                                txtNofactura.Text = "";
                        }
                    }
                    //Valida que exista una factura electronica
                    else if (facturado.id_factura_electronica > 0)
                    {
                        //Instancia a la clase comprobante para obtener la serie y folio 
                        using (SAT_CL.FacturacionElectronica.Comprobante comprobante = new SAT_CL.FacturacionElectronica.Comprobante(facturado.id_factura_electronica))
                        {
                            //Validando Comprobante
                            if (comprobante.habilitar)
                            {
                                //Validando Contenido del Comprobante
                                if (comprobante.serie.Equals("") && comprobante.folio == 0)

                                    //Inicializando Control en Cero '0'
                                    txtNofactura.Text = "0";
                                else
                                    //Asigna al control txtNofactura los valores de Serie y Folio de la factura electronica 
                                    txtNofactura.Text = comprobante.serie + ' ' + comprobante.folio.ToString();
                            }
                            else
                                //Inicializando Control en Vacio
                                txtNofactura.Text = "";
                        }
                    }
                }
                //Asignando Valores
                txtTarifaCobroFactura.Text = "ID:" + objFacturado.id_tarifa_cobro.ToString();
                ddlEstatus.SelectedValue = objFacturado.id_estatus.ToString();
                txtFechaFactura.Text = objFacturado.fecha_factura.ToString("dd/MM/yyyy HH:mm");
                txtFechaTC.Text = objFacturado.fecha_tipo_cambio.ToString("dd/MM/yyyy");
                txtTotalFactura.Text = string.Format("{0:#,###,###,###.00}", objFacturado.total_factura);
                ddlMonedaFactura.SelectedValue = objFacturado.moneda.ToString();
                txtTotalPesosFactura.Text = string.Format("{0:#,###,###,###.00}", objFacturado.total_factura_pesos);
                ddlCondicionPagoFactura.SelectedValue = objFacturado.id_condicion_pago.ToString();
                txtSubTotal.Text = string.Format("{0:#,###,###,###.00}", objFacturado.subtotal_factura);
                txtImpRet.Text = string.Format("{0:#,###,###,###.00}", objFacturado.retenido_factura);
                txtImpTra.Text = string.Format("{0:#,###,###,###.00}", objFacturado.trasladado_factura);

                //Instanciando tarifa de cobro aplicada
                using (SAT_CL.Tarifas.Tarifa tarifa = new SAT_CL.Tarifas.Tarifa(objFacturado.id_tarifa_cobro))
                {
                    //Asignando nombre de tarifa
                    lblTarifaCobro.Text = Cadena.TruncaCadena(tarifa.descripcion, 30, "...");
                    lblTarifaCobro.ToolTip = tarifa.descripcion;
                }
            }
            else
            {
                //Asignando Valores
                txtNofactura.Text =
                txtTarifaCobroFactura.Text = "";
                txtFechaTC.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                txtFechaFactura.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                txtTotalFactura.Text =
                txtTotalPesosFactura.Text =
                txtSubTotal.Text =
                txtImpRet.Text =
                txtImpTra.Text = string.Format("{0:#,###,###,###.00}", 0);
                lblTarifaCobro.Text = "----";
                lblTarifaCobro.ToolTip = "No Aplica Tarifa";
            }

            //Limpiando Mensaje de Operación
            lblErrorFactura.Text = "";
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Catalogos del Encabezado
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlCondicionPagoFactura, "", 23);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 3195);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 2129);
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlMonedaFactura, 184, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPago, 185, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoComrobante, 186, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Sucursales
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Catalogos de Tamaño  Referencias de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoReferencias, "", 26);
            //Cargando Catalogos de Tamaño  Refacturacion de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRefacturacion, "", 26);
        }
        /// <summary>
        /// Método Privado encargado de Asignar el Valor de los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Valores a los ViewState
            ViewState["objFacturado"] = objFacturado == null ? 0 : objFacturado.id_factura;
            ViewState["idServicio"] = idServicio;
            ViewState["DT"] = this._dT;
            ViewState["DTReferenciasViaje"] = this._dTReferenciasViaje;
            ViewState["DTRefacturaciones"] = this._dtRefacturaciones;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que existan los Valores
            if (ViewState["objFacturado"] != null)
                //Asignando Valor al Contructor
                this.objFacturado = new Facturado(Convert.ToInt32(ViewState["objFacturado"]));
            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idServicio"]) != 0)
                //Asignando Valor al Atributo
                this.idServicio = Convert.ToInt32(ViewState["idServicio"]);
            //Validando que Existan Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                //Asignando Valor
                this._dT = (DataTable)ViewState["DT"];
            //Validando que Existan Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DTReferenciasViaje"]))
                //Asignando Valor
                this._dTReferenciasViaje = (DataTable)ViewState["DTReferenciasViaje"];
            //Validando que Existan Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DTRefacturaciones"]))
                //Asignando Valor
                this._dtRefacturaciones = (DataTable)ViewState["DTRefacturaciones"];
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles para Edicion
        /// </summary>
        /// <param name="result">Estatus de Habilitación</param>
        private void habilitaControles(bool result_enabled)
        {
            //Controles
            ddlMonedaFactura.Enabled =
            ddlCondicionPagoFactura.Enabled =
            txtFechaFactura.Enabled =
            txtFechaTC.Enabled =
                //Botones
            btnAceptarFactura.Enabled =
            btnCancelarFactura.Enabled = result_enabled;
        }

        /// <summary>
        /// Configura la ventana de visualización de bitácora del registro solicitado
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucParada.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Visualizar los Controles de Facturación Electronica
        /// </summary>
        /// <param name="visualiza">Indica si se mostrarán los Controles de Facturación Electronica</param>
        private void visualizaControlesFE(bool visualiza)
        {
            //Asignando Visibilidad a los Controles
            lkbBitacora.Visible =
            lkbTimbrarFacturaElectronica.Visible =
                //lnkEditarFactura.Visible = 
            lkbAddendaFacturaElectronica.Visible =
            lkbRegistrarFacturaElectronica.Visible =
            lkbAplicarTarifa.Visible = visualiza;
        }

        /// <summary>
        /// Configura y muestra ventana de referencias de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla</param>
        private void inicializaReferencias(string id_registro, string id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/Comprobante.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar Control por Defecto
        /// <param name="id_servicio">Id de Servicio</param>
        /// </summary>
        public void InicializaControl(int id_servicio)
        {
            //Invocando Método de Carga de Catalogos
            cargaCatalogos();

            //Obteniendo la factura ligada al Servicio
            int idFac = Facturado.ObtieneFacturaServicio(id_servicio).id_factura;

            //Inicializando Controles
            inicializaControles(idFac, id_servicio);

            //Visualizando Control
            visualizaControlesFE(true);
        }
        /// <summary>
        /// Método encargado de Inicializar el Control 
        /// </summary>
        /// <param name="id_factura">Factura a la que pertenece</param>
        /// <param name="tieneServicio">Indicador si la Factura pertenece a un Servicio</param>
        public void InicializaControl(int id_factura, bool tieneServicio)
        {
            //Invocando Método de Carga de Catalogos
            cargaCatalogos();

            //Obteniendo la factura ligada al Servicio
            int idFac = id_factura;

            //Inicializando Controles
            inicializaControles(idFac, 0);

            //Visualizando Control
            visualizaControlesFE(tieneServicio);
        }
        /// <summary>
        /// Método Público encaragdo de Guardar los Cambios de las Facturas
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaFactura()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Obteniendo Fecha de Factura
            DateTime FecFac, FecTC;
            DateTime.TryParse(txtFechaFactura.Text, out FecFac);
            DateTime.TryParse(txtFechaTC.Text, out FecTC);

            //Validando que exista un Id
            if (objFacturado.id_factura != 0)
            {
                //Actualizando Concepto
                objFacturado.ActualizaFactura();

                //Validando el Estatus de la Factura
                if ((SAT_CL.Facturacion.Facturado.EstatusFactura)objFacturado.id_estatus == Facturado.EstatusFactura.Registrada)
                {
                    //Validando que exista una Relación en Facturación Electronica
                    if (FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(this.idFactura) == 0)

                        //Obteniendo Resultado de la Edición
                        result = objFacturado.EditaFactura(FecFac, FecTC, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTarifaCobroFactura.Text, "ID:", 1)),
                                                            Convert.ToInt32(ddlMonedaFactura.SelectedValue), Convert.ToInt32(ddlCondicionPagoFactura.SelectedValue),
                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Inicializando Contructor con Excepcion Personalizada
                        result = new RetornoOperacion("La Factura esta Registrada ó Timbrada en Facturación Electronica");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("El Estatus de la Factura no permite su Edición");
            }
            else
                //Obteniendo Resultado de la Inserción
                result = Facturado.InsertaFactura(this.idServicio, FecFac, FecTC, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTarifaCobroFactura.Text, "ID:", 1)),
                                                  Convert.ToInt32(ddlMonedaFactura.SelectedValue), Convert.ToInt32(ddlCondicionPagoFactura.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando que la operación haya sido exitosa
            if (result.OperacionExitosa)

                //Inicializando Control
                inicializaControles(result.IdRegistro, this.idServicio);

            //Mostrando Mensaje
            lblErrorFactura.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Realiza la búsqueda y aplicación de la tarifa correspondiente
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion AplicaTarifaFactura()
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que exista un Id de factura
            if (objFacturado.id_factura != 0)
            {
                //Validando que exista una Relación en Facturación Electronica
                if (FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(this.idFactura) == 0)
                {
                    //Cargando últimos cambios posibles
                    if (objFacturado.ActualizaFactura())
                    {
                        //Obteniendo Resultado de la Edición
                        resultado = objFacturado.ActualizaTarifa(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando que la operación haya sido exitosa
                        if (resultado.OperacionExitosa)
                        {
                            //Inicializando Control
                            inicializaControles(resultado.IdRegistro, this.idServicio);

                            //Mostrando Mensaje
                            lblErrorFactura.Text = resultado.Mensaje + "<br />* Nota: Al calcular la tarifa, se borraran los detalles.";
                        }
                        else
                            //Instanciando Excepción
                            lblErrorFactura.Text = resultado.Mensaje;
                    }
                }
                else
                    //Inicializando Contructor con Excepcion Personalizada
                    resultado = new RetornoOperacion("La Factura esta Registrada en Facturación Electronica");
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}