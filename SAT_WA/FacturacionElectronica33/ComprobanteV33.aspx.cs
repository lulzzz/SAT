using SAT_CL;
using SAT_CL.Bancos;
using SAT_CL.Global;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Web.Hosting;

namespace SAT.FacturacionElectronica33
{
    public partial class ComprobanteV33 : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento producido al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es una recraga de página
            if (!this.Page.IsPostBack)
            {
                //Inicializando contenido de página
                inicializaForma();
            }
        }
        /// <summary>
        /// Evento generado al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "EmisorReceptor":
                    mtvComprobante.ActiveViewIndex = 0;
                    btnEmisorReceptor.CssClass = "boton_pestana_activo";
                    btnDatosExpedicion.CssClass = "boton_pestana";
                    break;
                case "DatosExpedicion":
                    mtvComprobante.ActiveViewIndex = 1;
                    btnEmisorReceptor.CssClass = "boton_pestana";
                    btnDatosExpedicion.CssClass = "boton_pestana_activo";
                    break;
            }
        }
        /// <summary>
        /// Evento producido al dar click en algún elemento del menú desplegable de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Determinando el comando a ejecutar
            switch (((LinkButton)sender).CommandName)
            {
                case "Abrir":
                    inicializaAperturaRegistro();
                    break;
                case "Salir":
                    PilaNavegacionPaginas.DireccionaPaginaAnterior();
                    break;
                case "Eliminar":
                    eliminaComprobante();
                    break;
                case "Cancelar":
                    cancelaComprobante();
                    break;
                case "Bitacora":
                    inicializaBitacoraRegistro(Session["id_registro"].ToString(), "209", "Comprobante Fiscal Digital v3.3");
                    break;
                case "Referencias":
                    inicializaReferencias((Session["id_registro"]).ToString(), "209", "Comprobante Fiscal Digital v3.3");
                    break;
                case "VerTimbre":
                    //Validamos Existencia de comprobante
                    if (Convert.ToInt32(Session["id_registro"]) != 0)
                    {
                        //Instancia comprobante
                        using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validamos Si se Genero la Factura
                            if (c.bit_generado)
                            {
                                //Generamos Ruta para Abrir Timbre
                                string ruta_ventana = string.Format("TimbreFiscal.aspx?id_comprobante={0}", Session["id_registro"]);
                                //Abriendo ventana de detalle
                                ScriptServer.AbreNuevaVentana(ruta_ventana, "Timbre Fiscal", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=700,height=420", Page);
                            }
                        }
                    }
                    break;
                case "XML":
                    //Realizando descarga XML
                    descargarXML();
                    break;
                case "PDF":
                    //Obteniendo Ruta
                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteV33.aspx", "~/RDLC/Reporte.aspx");

                    //Instanciamos Comprobante
                    using (SAT_CL.FacturacionElectronica33.Comprobante objComprobante = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Validamos que exista el Comprobante
                        if (objComprobante.id_comprobante33 > 0)
                        {
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Comprobante", objComprobante.id_comprobante33), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        }
                    }
                    break;
                case "Timbrar":
                    //Abre Ventana Modal
                    ScriptServer.AlternarVentana(uplkbTimbrar, uplkbTimbrar.GetType(), "AbrirVentana", "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");
                    //Inicializa Valores
                    inicializaValoresTimbrarFacturacionElectronica();
                    break;
            }
        }
        /// <summary>
        /// Método encargado de Actualizar los Conceptos del Comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarConceptos_Click(object sender, EventArgs e)
        {
            //Cargando Detalles
            cargaDetallesComprobante();
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
        /// Evento producido al cambiar el tamaño de gridview de detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvConceptos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TResumen"), Convert.ToInt32(ddlTamañoGridViewConceptos.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento producido al pulsar el botón de exportación a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcel_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TResumen"));
        }
        /// <summary>
        /// Evento prodicido al cambiar el indice de página del Gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvConceptos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TResumen"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento producido al cambiar el orden del Gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Controles.CambiaSortExpressionGridView(gvConceptos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "TResumen"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferenciaConcepto_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvConceptos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvConceptos, sender, "lnk", false);
                //Inicializando Referencias
                inicializaReferenciaConcepto(gvConceptos.SelectedDataKey["IdRegistro"].ToString(), "215", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Inicializa el contenido de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Obteniendo Comprobante
            string id_comprobante33 = Request.QueryString["idRegistro"];
            //Validando que Exista
            if (id_comprobante33 != null)
                //Añadiendo Resultado a Session
                Session["id_registro"] = Convert.ToInt32(id_comprobante33);
            //Cargando catálogos requeridos
            cargaCatalogos();
            //Cargando valores sobre controles
            inicializaValoresRegistro();
            //Habilitando controles
            habilitaControles();
            //habilitando menú principal de forma
            inicializaMenu();
            //Cargando Detalles de factura
            cargaDetallesComprobante();
            //DONE: Implementar seguridad en controles         
            // BibliotecaClasesCentralDB.Seguridad.clControlPerfilUsuario.SeguridadControles(41, this.Page, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        }
        /// <summary>
        /// Carga los catálogos necesarios en la generación
        /// </summary>
        private void cargaCatalogos()
        {
            //Sucursales
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Métodos de Pago
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 3195);
            //Tipo Comprobante
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoComprobante,186,"Tipo de Comprobante",0,"",0,"");//Ya no es catálogo, es tabla
            //Estatus de Comprobante
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 3200);
            //Uso de CFDI
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlUsoCFDI, "", 3194);////
            //Forma de Pago
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlFormaPago, "", 1099);
            //Moneda
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMoneda, "", 11);
            //Tamaño de página en GridView
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewConceptos, "", 18);            
        }
        /// <summary>
        /// Carga valores de un registro determinado
        /// </summary>
        private void inicializaValoresRegistro()
        {
            //Determinando el tipo de carga a realizar en base al estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Nuevo registro
                case Pagina.Estatus.Nuevo:
                    //Id de Registro
                    lblID.Text = "ID";
                    //Serie 
                    txtSerie.Text =  "";
                    //Folio
                    txtFolio.Text = "";
                    //Emisor
                    using (SAT_CL.Global.CompaniaEmisorReceptor Emisor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        txtEmisor.Text = Emisor.nombre + " ID:" + Emisor.id_compania_emisor_receptor.ToString();
                    }
                    //Domicilio Emisor
                    cargaDomicilioRequerido("emisor");
                    //Sucursal
                    ddlSucursal.SelectedValue = "0";                    
                    //Receptor y Domicilio receptor
                    txtReceptor.Text = "";
                    //Método de Pago predeterminado "Transferencia Electrónica"
                    ddlMetodoPago.SelectedValue = "5";                    
                    //Tipo de Comprobante
                    ddlTipoComprobante.SelectedValue = "1";
                    //Estatus
                    ddlEstatus.SelectedValue = "1";
                    //Confirmacion
                    txtConfirmacion.Text = "";
                    //Régimen Fiscal
                    txtRegimenFiscal.Text = "";
                    //Uso CFDI
                    ddlUsoCFDI.SelectedValue = "";
                    //Condiciones de Pago
                    txtCondicionesPago.Text = "";
                    //Forma de Pago "Una sola exhibición"
                    ddlFormaPago.SelectedValue = "1";
                    //Moneda
                    ddlMoneda.SelectedValue = "1";
                    //Tipo de Cambio
                    txtTipoCambio.Text = "1.0000";
                    //Fechas de Captura
                    txtFechaCaptura.Text = "";
                    //Fecha de Expedición
                    txtFechaExpedicion.Text = "";
                    //Lugar de Expedición                     
                    cargaDomicilioRequerido("lugar_expedicion");
                    //Check timbrado
                    chkGenerado.Checked = false;
                    //Sello digital
                    txtSelloDigital.Text = "";
                    //fecha Cancelación
                    txtFechaCancelacion.Text = "";
                    //Totales de Comprobante
                    lblSubtotalCaptura.Text =
                    lblSubtotalNacional.Text =
                    lblDescuentosCaptura.Text =
                    lblDescuentosNacional.Text =
                    lblImpuestosCaptura.Text =
                    lblImpuestosNacional.Text =
                    lblTotalCaptura.Text =
                    lblTotalNacional.Text = string.Format("{0:c4}", 0);
                    break;
                //Lectura y edición de registro activo en sesión
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    //Instanciando registro comprobante
                    using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Asignando valores de registro
                        //Id de Registro
                        lblID.Text = c.id_comprobante33.ToString();
                        //Serie
                        txtSerie.Text = c.serie;
                        //Folio
                        txtFolio.Text = c.folio.ToString();
                        //Emisor
                        using (SAT_CL.Global.CompaniaEmisorReceptor Emisor = new SAT_CL.Global.CompaniaEmisorReceptor(c.id_compania_emisor))
                        {
                            txtEmisor.Text = Emisor.nombre + " ID:" + Emisor.id_compania_emisor_receptor.ToString();
                        }
                        //Domicilio Emisor
                        cargaDomicilioRequerido("emisor");
                        //Sucursal
                        ddlSucursal.SelectedValue = c.id_sucursal.ToString();                        
                        //Receptor y Domicilio receptor
                        using (CompaniaEmisorReceptor r = new CompaniaEmisorReceptor(c.id_compania_receptor))
                        {
                            txtReceptor.Text = r.nombre + "   ID:" + r.id_compania_emisor_receptor.ToString();
                        }                        
                        //Método de Pago predeterminado "Transferencia Electrónica"
                        ddlMetodoPago.SelectedValue = c.id_metodo_pago.ToString();                        
                        //Tipo de Comprobante
                        ddlTipoComprobante.SelectedValue = ((byte)c.id_tipo_comprobante).ToString();
                        //Estatus
                        ddlEstatus.SelectedValue = ((byte)c.id_estatus_vigencia).ToString();
                        //Condiciones de Pago
                        txtCondicionesPago.Text = c.condicion_pago.ToString();
                        //Forma de Pago "Una sola exhibición"
                        ddlFormaPago.SelectedValue = c.id_forma_pago.ToString();                        
                        //Moneda
                        ddlMoneda.SelectedValue = c.id_moneda.ToString();
                        //Tipo de Cambio
                        txtTipoCambio.Text = c.tipo_cambio.ToString();
                        //Fechas de Captura
                        txtFechaCaptura.Text = c.fecha_captura.ToString("yyyy/MM/dd hh:mm:ss tt");
                        //Fecha de Expedición
                        txtFechaExpedicion.Text = Fecha.ConvierteDateTimeString(c.fecha_expedicion, "yyyy/MM/dd hh:mm:ss tt");
                        //Lugar de Expedición
                        txtLugarExpedicion.Text = c.lugar_expedicion;
                        //Check Timbrado
                        chkGenerado.Checked = c.bit_generado;
                        //Sello Digital
                        txtSelloDigital.Text = c.sello;
                        //fecha Cancelación
                        txtFechaCancelacion.Text = Fecha.ConvierteDateTimeString(c.fecha_cancelacion, "yyyy/MM/dd hh:mm:ss tt");
                        
                        //Totales de Comprobante 
                        lblSubtotalCaptura.Text = string.Format("{0:c4}", c.subtotal_captura);
                        lblSubtotalNacional.Text = string.Format("{0:c4}", c.subtotal_nacional);
                        lblDescuentosCaptura.Text = string.Format("{0:c4}", c.descuentos_captura);
                        lblDescuentosNacional.Text = string.Format("{0:c4}", c.descuentos_nacional);
                        lblImpuestosCaptura.Text = string.Format("{0:c4}", c.impuestos_captura);
                        lblImpuestosNacional.Text = string.Format("{0:c4}", c.impuestos_nacional);
                        lblTotalCaptura.Text = string.Format("{0:c4}", c.total_captura);
                        lblTotalNacional.Text = string.Format("{0:c4}", c.total_nacional);
                    }
                    break;
            }
            //Estableciendo indice de tab por default
            //tbcComprobante.ActiveTabIndex = 0;
        }
        /// <summary>
        /// Habilitando controles de captura
        /// </summary>
        private void habilitaControles()
        {
            //Determinando el estatus que se aplicará sobre el registro
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    txtSerie.Enabled = true;
                    txtFolio.Enabled = true;
                    txtEmisor.Enabled = true;
                    txtDomicilioEmisor.Enabled = true;
                    ddlSucursal.Enabled = true;
                    txtReceptor.Enabled = true;
                    ddlMetodoPago.Enabled = true;
                    ddlTipoComprobante.Enabled = true;
                    ddlEstatus.Enabled = true;
                    txtConfirmacion.Enabled = true;
                    txtRegimenFiscal.Enabled = true;
                    ddlUsoCFDI.Enabled = true;
                    txtCondicionesPago.Enabled = true;
                    ddlFormaPago.Enabled = true;                    
                    ddlMoneda.Enabled = true;
                    txtTipoCambio.Enabled = true;
                    txtFechaCaptura.Enabled = true;
                    txtFechaExpedicion.Enabled = true;
                    txtLugarExpedicion.Enabled = true;
                    chkGenerado.Enabled = true;
                    txtSelloDigital.Enabled = true;
                    txtFechaCancelacion.Enabled = true;
                    break;
                case Pagina.Estatus.Lectura:
                    txtSerie.Enabled =
                    txtFolio.Enabled =
                    txtEmisor.Enabled =
                    txtDomicilioEmisor.Enabled =
                    ddlSucursal.Enabled =
                    txtReceptor.Enabled =
                    ddlMetodoPago.Enabled =
                    ddlTipoComprobante.Enabled =
                    ddlEstatus.Enabled =
                    txtConfirmacion.Enabled =
                    txtRegimenFiscal.Enabled =
                    ddlUsoCFDI.Enabled =
                    txtCondicionesPago.Enabled =
                    ddlFormaPago.Enabled =
                    ddlMoneda.Enabled =
                    txtTipoCambio.Enabled =
                    txtFechaCaptura.Enabled =
                    txtFechaExpedicion.Enabled =
                    txtLugarExpedicion.Enabled =
                    chkGenerado.Enabled =
                    txtSelloDigital.Enabled =
                    txtFechaCancelacion.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    txtSerie.Enabled = true;
                    txtFolio.Enabled = true;
                    txtEmisor.Enabled = true;
                    txtDomicilioEmisor.Enabled = true;
                    ddlSucursal.Enabled = true;
                    txtReceptor.Enabled = true;
                    ddlMetodoPago.Enabled = true;
                    ddlTipoComprobante.Enabled = true;
                    ddlEstatus.Enabled = true;
                    txtConfirmacion.Enabled = true;
                    txtRegimenFiscal.Enabled = true;
                    ddlUsoCFDI.Enabled = true;
                    txtCondicionesPago.Enabled = true;
                    ddlFormaPago.Enabled = true;                    
                    ddlMoneda.Enabled = false;//No se permite editar la moneda
                    txtTipoCambio.Enabled = false;//Ni el tipo de cambio
                    txtFechaCaptura.Enabled = true;
                    txtFechaExpedicion.Enabled = true;
                    txtLugarExpedicion.Enabled = true;
                    chkGenerado.Enabled = true;
                    txtSelloDigital.Enabled = true;
                    txtFechaCancelacion.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Metodo encargado de inicializar el menu de la forma
        /// </summary>
        private void inicializaMenu()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lkbAbrir.Enabled = true;
                        lkbXML.Enabled =
                        lkbPDF.Enabled =
                        lkbEliminar.Enabled = false;
                        lkbCancelar.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferenciasCFDI.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        lkbXML.Enabled =
                        lkbPDF.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbEliminar.Enabled = true;
                        lkbCancelar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferenciasCFDI.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        lkbXML.Enabled =
                        lkbPDF.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbEliminar.Enabled = false;
                        lkbCancelar.Enabled = false;
                        lkbBitacora.Enabled = true;
                        lkbReferenciasCFDI.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Cargando detalles de factura (conceptos/descuentos/impuestos)
        /// </summary>
        private void cargaDetallesComprobante()
        {
            //En base al estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    Controles.InicializaGridview(gvConceptos);
                    Session["DS"] = null;
                    break;
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Realizando carga de resumen de detalles
                        using (DataTable mit = SAT_CL.FacturacionElectronica33.Comprobante.CargaConceptosComprobante(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que Existan Datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                            {
                                //Llenando GridView
                                Controles.CargaGridView(gvConceptos, mit, "IdTipo-IdRegistro-IdAuxiliar", lblCriterioGridViewConceptos.Text, true, 1);
                                //Guardando en sesión
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "TResumen");
                            }
                            else
                            {
                                //Inicializando GridView
                                Controles.InicializaGridview(gvConceptos);
                                //Eliminando en sesión
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "TResumen");
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Realiza la carga del domicilio en base a la entidad que sea requedida
        /// </summary>
        /// <param name="entidad">Entidad que se actualizará ("emisor", "sucursal" y "receptor" son los valores aceptados)</param>
        private void cargaDomicilioRequerido(string entidad)
        {
            //Definiendo objeto domicilio
            Direccion u = new Direccion();
            //Determinando que entidad será consultada
            switch (entidad)
            {
                case "emisor":
                    //Instanciando emisor
                    using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        //Instanciando ubicación asignada
                        u = new Direccion(em.id_direccion);
                    //Indicando el control de texto que debe ser afectado
                    txtDomicilioEmisor.Text = u.ObtieneDireccionCompleta() + "   ID:" + u.id_direccion.ToString();
                    break;
                //case "sucursal":
                //    //Instanciando sucursal
                //    using (Sucursal suc = new Sucursal(Convert.ToInt32(ddlSucursal.SelectedValue)))
                //        //Instanciando ubicación asignada
                //        u = new Direccion(suc.id_direccion);
                //    //Indicando el control de texto que debe ser afectado
                //    txtDomicilioSucursal.Text = u.ObtieneDireccionCompleta() + "   ID:" + u.id_direccion.ToString();
                //    break;
                //case "receptor":
                //    //Instanciando receptor
                //    using (CompaniaEmisorReceptor rec = new CompaniaEmisorReceptor(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtReceptor.Text, "ID:", 1))))
                //        //Instanciando ubicación asignada
                //        u = new Direccion(rec.id_direccion);
                //    //Indicando el control de texto que debe ser afectado
                //    txtDomicilioReceptor.Text = u.ObtieneDireccionCompleta() + "   ID:" + u.id_direccion.ToString();
                //    break;
                case "lugar_expedicion":
                    //Determinando si el lugar estará dado por el domicilio de sucursal
                    if (ddlSucursal.SelectedValue != "0")
                    {
                        //Instanciando sucursal
                        using (Sucursal suc = new Sucursal(Convert.ToInt32(ddlSucursal.SelectedValue)))
                            //Instanciando ubicación asignada
                            u = new Direccion(suc.id_direccion);
                    }
                    else
                    {
                        //Instanciando emisor
                        using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            //Instanciando ubicación asignada
                            u = new Direccion(em.id_direccion);
                    }
                    //Indicando el control de texto que debe ser afectado
                    txtLugarExpedicion.Text = u.municipio + ", " + Catalogo.RegresaDescripcionCatalogo(16, u.id_estado);
                    //Actualizando panel, ya que al estar en tabs distintos no se puede añadir el trigger en diseño
                    uptxtLugarExpedicion.Update();
                    break;
            }
        }              
        /// <summary>
        /// Realiza la validación de edición del comprobante para el entorno de usuario
        /// </summary>
        /// <returns></returns>
        private bool validaEdicionComprobante()
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);
            //Instanciando el comprobante actual
            using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el registro existe
                if (c.id_comprobante33 > 0)
                {
                    //Si el comprobante no se ha timbrado
                    if (c.bit_generado)
                        resultado = new RetornoOperacion("El comprobante ya se ha timbrado, no es posible editarlo.");
                }
                else
                    resultado = new RetornoOperacion("El comprobante no fue encontrado.");
            }
            return resultado.OperacionExitosa;
        }
        /// <summary>
        /// Deshabilita el comprobante y sus dependencias
        /// </summary>
        /// <returns></returns>
        private void eliminaComprobante()
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);
            //Instanciando el comprobante actual
            using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el registro existe
                if (c.id_comprobante33 > 0)
                {
                    //Si el comprobante no se ha timbrado
                    if (!c.bit_generado)
                    {
                        //Realziando la deshabilitación
                        resultado = c.DeshabilitaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si no existe error
                        if (resultado.OperacionExitosa)
                        {
                            //Estableciendo estatus edición
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            inicializaForma();
                        }
                    }
                    else
                        resultado = new RetornoOperacion("El comprobante ya se ha timbrado, no es posible editarlo.");
                }
                else
                    resultado = new RetornoOperacion("El comprobante no fue encontrado.");
            }
            //Mostramos Mensaje
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Cancela el comprobante y sus dependencias
        /// </summary>
        /// <returns></returns>
        private void cancelaComprobante()
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);
            //Instanciando el comprobante actual
            using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el registro existe
                if (c.id_comprobante33 > 0)
                {
                    //Realziando la deshabilitación
                    //resultado = c.CancelaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                else
                    resultado = new RetornoOperacion("El comprobante no fue encontrado.");
            }
            //Si no existe error
            if (resultado.OperacionExitosa)
            {
                //Estableciendo estatus edición
                Session["estatus"] = Pagina.Estatus.Lectura;
                inicializaForma();
            }
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Método que inicializa el cuadro de dialogo para apertura de registros
        /// </summary>
        private void inicializaAperturaRegistro()
        {
            //Definiendo el Id de tabla por abrir
            Session["id_tabla"] = 209;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteV33.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Instanciando nueva ventana de navegador para apertura de registro
            ScriptServer.AbreNuevaVentana(url, "Abrir", configuracion, Page);
        }
        /// <summary>
        /// Método que inicializa el control bitácora del registro
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="titulo">TItulo a mostrar</param>
        private void inicializaBitacoraRegistro(string idRegistro, string idTabla, string titulo)
        {
            //Declarando variables para armado de URL
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteV33.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + titulo);
            //Instanciando nueva ventana de navegador para apertura de bitacora de registro
            ScriptServer.AbreNuevaVentana(urlDestino, "Bitacora", 700, 420, false, false, false, true, true, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteV33.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }
        /// <summary>
        /// Realiza la descarga del XML del comprobante
        /// </summary>
        private void descargarXML()
        {
            //Instanciando registro en sesión
            using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
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
                        using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(c.id_compania_emisor))
                            Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio), Archivo.ContentType.binary_octetStream);
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaConcepto(string id_registro, string id_tabla, string id_compania)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteV33.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Inicializa Valores para el Timbrado de la Facturación Electrónica
        /// </summary>
        private void inicializaValoresTimbrarFacturacionElectronica()
        {
            txtSerieTimbrar.Text = "";
            chkOmitirAddenda.Checked = false;
            lblTimbrarFacturacionElectronica.Text = "";
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
            //Instanciando registro comprobante
            using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(Session["id_registro"])))
            {
                /*/Registramos Factura
                resultado = c.TimbraComprobanteFacturadoFacturaGlobal(txtSerieTimbrar.Text, chkOmitirAddenda.Checked, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_3.xslt"),
                                                                         HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica33/cadenaoriginal_3_3_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);//*/
            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Valores
                inicializaValoresRegistro();
            }
            //Mostrando resultado
            lblTimbrarFacturacionElectronica.Text = resultado.Mensaje;
        }
        #endregion
    }
}