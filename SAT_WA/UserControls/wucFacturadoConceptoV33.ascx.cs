using SAT_CL;
using SAT_CL.Facturacion;
using SAT_CL.Seguridad;
using System;
using System.Data;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucFacturadoConceptoV33 : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Id de Factura
        /// </summary>
        private int idFactura;

        private DataTable _dtConceptos;
        /// <summary>
        /// Propiedad TabIndex del Control de Usuario
        /// </summary>
        public short TabIndex
        {
            set
            {   //Concepto
                txtCantidadFacturaConcepto.TabIndex =
                ddlConceptoCobroFacturaConcepto.TabIndex =
                ddlUnidadFacturaConcepto.TabIndex =
                txtIdentificadorFacturaConcepto.TabIndex =
                txtValorUniFacturaConcepto.TabIndex =
                ddlImpuestoRetenido.TabIndex =
                txtTasaImpRetFacturaConcepto.TabIndex =
                ddlImpuestoTrasladado.TabIndex =
                txtTasaImpTraFacturaConcepto.TabIndex =
                btnAceptarFacturaConcepto.TabIndex =
                btnCancelar.TabIndex =
                ddlTamano.TabIndex =
                lnkExportarExcel.TabIndex =
                gvConceptosFacturaConcepto.TabIndex = value;
            }
            get { return txtCantidadFacturaConcepto.TabIndex; }
        }
        /// <summary>
        /// Propiedad Enabled del Control de Usuario
        /// </summary>
        public bool Enabled
        {
            set
            {   //Concepto
                txtCantidadFacturaConcepto.Enabled =
                ddlUnidadFacturaConcepto.Enabled =
                txtIdentificadorFacturaConcepto.Enabled =
                ddlConceptoCobroFacturaConcepto.Enabled =
                txtValorUniFacturaConcepto.Enabled =
                ddlImpuestoRetenido.Enabled =
                txtTasaImpRetFacturaConcepto.Enabled =
                ddlImpuestoTrasladado.Enabled =
                txtTasaImpTraFacturaConcepto.Enabled =
                btnAceptarFacturaConcepto.Enabled =
                ddlTamano.Enabled =
                lnkExportarExcel.Enabled =
                gvConceptosFacturaConcepto.Enabled = value;
            }
            get { return txtCantidadFacturaConcepto.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento desencadenado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando que se haya Producido un PostBack
            if (!(Page.IsPostBack))
                //Invocando Método de Carga
                cargaCatalogos();
            else//Recuperando Valor de los Atributos
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

        #region Eventos Concepto

        /// <summary>
        /// Declarando el Evento ClickGuardarFacturaConcepto
        /// </summary>
        public event EventHandler ClickGuardarFacturaConcepto;
        /// <summary>
        /// Método que manipula el Evento "Guardar Concepto"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarFacturaConcepto(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickGuardarFacturaConcepto != null)
                //Invocando al Delegado
                ClickGuardarFacturaConcepto(this, e);
        }
        /// <summary>
        /// Declarando el Evento ClickEliminarFacturaConcepto
        /// </summary>
        public event EventHandler ClickEliminarFacturaConcepto;
        /// <summary>
        /// Método que manipula el Evento "Eliminar Concepto"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarFacturaConcepto(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickEliminarFacturaConcepto != null)
                //Invocando al Delegado
                ClickEliminarFacturaConcepto(this, e);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Concepto de Cobro del Detalle de Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlConceptoCobroFacturaConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Instanciando Tipo de Carga
            using (SAT_CL.Tarifas.TipoCargo tc = new SAT_CL.Tarifas.TipoCargo(Convert.ToInt32(ddlConceptoCobroFacturaConcepto.SelectedValue)))
            {
                //Validando que exista un tipo de cargo
                if (tc.habilitar)
                {
                    //Asignando Unidad
                    ddlUnidadFacturaConcepto.SelectedValue = tc.id_unidad.ToString();

                    //Seleccionando elementos del catálogo de tipo de impuesto
                    ddlImpuestoRetenido.SelectedValue = tc.id_tipo_impuesto_retenido.ToString();
                    ddlImpuestoTrasladado.SelectedValue = tc.id_tipo_impuesto_trasladado.ToString();

                    //Cargando Tasas de Impuesto
                    txtTasaImpRetFacturaConcepto.Text = tc.tasa_impuesto_retenido.ToString();
                    txtTasaImpTraFacturaConcepto.Text = tc.tasa_impuesto_trasladado.ToString();
                }
            }
        }
        /// <summary>
        /// Evento producido al Presionar el Boton "Actualizar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarFacturaConcepto_Click(object sender, EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickGuardarFacturaConcepto != null)
                //Inicializando
                OnClickGuardarFacturaConcepto(e);
            return;
        }
        /// <summary>
        /// Evento producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Inicializando Indices del GV
            TSDK.ASP.Controles.InicializaIndices(gvConceptosFacturaConcepto);
            //Limpiando Controles
            limpiaControlesConcepto();
        }

        /// Evento generado al dar click en Bitácora 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacora(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvConceptosFacturaConcepto, sender, "lnk", false);
            //Validamos que existan Registros
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {
                //Mostramos Bitácora
                inicializaBitacora(gvConceptosFacturaConcepto.SelectedValue.ToString(), "12", "Bitácora Concepto");
            }
        }
        /// <summary>
        /// Evento generado al dar click en Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkReferenciasConcepto_Click(object sender, EventArgs e)
        {
            //Validamos que existan Registros
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvConceptosFacturaConcepto, sender, "lnk", false);

                //Inicializando Referencias
                inicializaReferenciaRegistro(gvConceptosFacturaConcepto.SelectedDataKey["Id"].ToString(), "12", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
            }
        }

        #region Eventos GridView Conceptos

        /// <summary>
        /// Evento Producido al Cambiar el tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando que existan los DataKeys
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {   //Limpiando Controles
                limpiaControlesConcepto();
                //Asignando Orden de Registros
                this._dtConceptos.DefaultView.Sort = lblOrdenado.Text;
                //Cambiando el Tamaño del GridView
                TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvConceptosFacturaConcepto, this._dtConceptos, Convert.ToInt32(ddlTamano.SelectedValue), true, 3);
                //Mostrando Totales
                escribeTotales();
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosFacturaConcepto_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {   //Validando que existan los DataKeys
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {   //Limpiando Controles
                limpiaControlesConcepto();
                //Asignando Orden de Registros
                this._dtConceptos.DefaultView.Sort = lblOrdenado.Text;
                //Cambiando Ordenamiento del GridView
                lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvConceptosFacturaConcepto, this._dtConceptos, e.SortExpression, true, 3);
                //Mostrando Totales
                escribeTotales();
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosFacturaConcepto_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {   //Validando que existan los DataKeys
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {   //Limpiando Controles
                limpiaControlesConcepto();
                //Asignando Orden de Registros
                this._dtConceptos.DefaultView.Sort = lblOrdenado.Text;
                //Cambiando Indice del GridView
                TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvConceptosFacturaConcepto, this._dtConceptos, e.NewPageIndex, true, 3);
                //Mostrando Totales
                escribeTotales();
            }
        }
        /// <summary>
        /// Evento Producido al presionar el Link Button "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarExcel_Click(object sender, EventArgs e)
        {   //Validando que existan Llaves
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
                //Exportando Excel
                Controles.ExportaContenidoGridView(this._dtConceptos, "IdUnidad", "IdConceptoCobro");
        }
        /// <summary>
        /// Evento Producido al presionar el Link Button "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarFacturaConcepto_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvConceptosFacturaConcepto, sender, "lnk", false);

                //Instanciando Concepto
                using (FacturadoConcepto objFacturadoConcepto = new FacturadoConcepto(Convert.ToInt32(gvConceptosFacturaConcepto.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Concepto
                    if (objFacturadoConcepto.id_detalle_facturado != 0)
                    {
                        //Asignando valores
                        txtCantidadFacturaConcepto.Text = objFacturadoConcepto.cantidad.ToString();
                        ddlUnidadFacturaConcepto.SelectedValue = objFacturadoConcepto.id_unidad.ToString();
                        txtIdentificadorFacturaConcepto.Text = objFacturadoConcepto.identificador;
                        ddlConceptoCobroFacturaConcepto.SelectedValue = objFacturadoConcepto.id_concepto_cobro.ToString();
                        txtValorUniFacturaConcepto.Text = objFacturadoConcepto.valor_unitario.ToString();
                        txtImporteFacturaConcepto.Text = string.Format("{0:#,###,###,##0.00}", objFacturadoConcepto.importe);
                        ddlImpuestoRetenido.SelectedValue = objFacturadoConcepto.id_impuesto_retenido.ToString();
                        txtTasaImpRetFacturaConcepto.Text = string.Format("{0:#,###,###,##0.00}", objFacturadoConcepto.tasa_impuesto_retenido);
                        ddlImpuestoTrasladado.SelectedValue = objFacturadoConcepto.id_impuesto_trasladado.ToString();
                        txtTasaImpTraFacturaConcepto.Text = string.Format("{0:#,###,###,##0.00}", objFacturadoConcepto.tasa_impuesto_trasladado);

                        //Cargando Tasas con respecto al Concepto
                        cargaTasasConcepto();
                    }
                }
                //Asignando foco en control principal
                txtCantidadFacturaConcepto.Focus();
            }
        }
        /// <summary>
        /// Evento Producido al presionar el Link Button "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFacturaConcepto_Click(object sender, EventArgs e)
        {   //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvConceptosFacturaConcepto, sender, "lnk", false);
            //Validando que este vacio el Evento
            if (ClickEliminarFacturaConcepto != null)
                //Inicializando
                OnClickEliminarFacturaConcepto(e);
            return;
        }

        #endregion

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Catalogos de los Conceptos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlUnidadFacturaConcepto, "", 25);
            //Catalogos de Impuesto retenido
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlImpuestoRetenido, "", 94, 1);
            //Catalogos Impuesto Trasladado
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlImpuestoTrasladado, "", 94, 2);
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConceptoCobroFacturaConcepto, 17, "", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Tasas con respecto al Concepto
            cargaTasasConcepto();
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Asignar el Valor de los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Valores a los ViewState
            ViewState["idFactura"] = this.idFactura;
            ViewState["DT"] = this._dtConceptos;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idFactura"]) != 0)
                //Asignando Valor al Atributo
                this.idFactura = Convert.ToInt32(ViewState["idFactura"]);
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                //Asignando Valor al Atributo
                this._dtConceptos = (DataTable)ViewState["DT"];
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles de Captura del Concepto
        /// </summary>
        private void limpiaControlesConcepto()
        {
            //Limpiando Controles
            txtIdentificadorFacturaConcepto.Text = "";
            txtCantidadFacturaConcepto.Text =
            txtValorUniFacturaConcepto.Text =
            txtImporteFacturaConcepto.Text = "0.00";
            ddlImpuestoRetenido.SelectedValue = "2";
            txtTasaImpRetFacturaConcepto.Text = "0.00";
            ddlImpuestoTrasladado.SelectedValue = "3";
            txtTasaImpTraFacturaConcepto.Text = "16.00";

            //Cargando Tasas por Concepto
            cargaTasasConcepto();
        }
        /// <summary>
        /// Método Privado encargado de Inicializar el Reporte de los Conceptos
        /// </summary>
        /// <param name="id_factura">Id de factura</param>
        private void inicializaReporteConceptos(int id_factura)
        {
            //Obteniendo Conceptos
            using (DataTable dtConceptos = FacturadoConcepto.ObtieneConceptosFactura(id_factura))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptos))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvConceptosFacturaConcepto, dtConceptos, "Id", "", true, 3);
                    //Añadiendo Tabla a ViewState
                    this._dtConceptos = dtConceptos;
                    //Mostrando Totales
                    escribeTotales();
                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvConceptosFacturaConcepto);
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvConceptosFacturaConcepto);
                    //Eliminando Tabla de ViewState
                    this._dtConceptos = null;
                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvConceptosFacturaConcepto);
                    //Mostrando Totales
                    escribeTotales();
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Escribir los Valores Totales de los Conceptos
        /// </summary>
        private void escribeTotales()
        {   //Validando Origen de datos
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._dtConceptos))
            {   //Mostrando Totales
                gvConceptosFacturaConcepto.FooterRow.Cells[6].Text = string.Format("{0:c}", this._dtConceptos.Compute("SUM(Importe)", ""));
                gvConceptosFacturaConcepto.FooterRow.Cells[7].Text = string.Format("{0:c}", this._dtConceptos.Compute("SUM(ImportePesos)", ""));
                gvConceptosFacturaConcepto.FooterRow.Cells[9].Text = string.Format("{0:c}", this._dtConceptos.Compute("SUM(ImporteRetenido)", ""));
                gvConceptosFacturaConcepto.FooterRow.Cells[11].Text = string.Format("{0:c}", this._dtConceptos.Compute("SUM(ImporteTrasladado)", ""));
            }
            else
            {   //Mostrando Totales en 0
                gvConceptosFacturaConcepto.FooterRow.Cells[6].Text = string.Format("{0:c}", 0);
                gvConceptosFacturaConcepto.FooterRow.Cells[7].Text = string.Format("{0:c}", 0);
                gvConceptosFacturaConcepto.FooterRow.Cells[9].Text = string.Format("{0:c}", 0);
                gvConceptosFacturaConcepto.FooterRow.Cells[11].Text = string.Format("{0:c}", 0);
            }
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
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucFacturadoConcepto.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método encargado de Cargar las Tasas de Traslado y Retención ligadas al Concepto
        /// </summary>
        private void cargaTasasConcepto()
        {
            //Instanciando Tipo de Carga
            using (SAT_CL.Tarifas.TipoCargo tc = new SAT_CL.Tarifas.TipoCargo(Convert.ToInt32(ddlConceptoCobroFacturaConcepto.SelectedValue)))
            {
                //Validando que exista un tipo de cargo
                if (tc.id_tipo_cargo != 0)
                {
                    //Validando que no Exista un Selección
                    if (gvConceptosFacturaConcepto.SelectedIndex == -1)
                    {
                        //Seleccionando elementos del catálogo de tipo de impuesto
                        ddlImpuestoRetenido.SelectedValue = tc.id_tipo_impuesto_retenido.ToString();
                        ddlImpuestoTrasladado.SelectedValue = tc.id_tipo_impuesto_trasladado.ToString();

                        //Cargando Tasas de Impuesto
                        txtTasaImpRetFacturaConcepto.Text = tc.tasa_impuesto_retenido.ToString();
                        txtTasaImpTraFacturaConcepto.Text = tc.tasa_impuesto_trasladado.ToString();
                    }
                }
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar Control por Defecto
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_factura">Id de Factura</param>
        /// </summary>
        public void InicializaControl(int id_factura)
        {   //Invocando Método de carga
            cargaCatalogos();
            //Inicializando Objetos
            this.idFactura = id_factura;
            //Limpiando Controles
            limpiaControlesConcepto();
            //Inicializando Reporte
            inicializaReporteConceptos(id_factura);
        }
        /// <summary>
        /// Método Público encargado de Guardar los Conceptos
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardarFacturaConcepto()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que exista una Factura
            if (this.idFactura != 0)
            {
                //Validando que exista una Relación en Facturación Electronica
                if (FacturadoFacturacion.ObtieneFacturacionElectronicaActivaV3_3(this.idFactura) == 0)
                {
                    //Validando que exista una Relación en Facturación Electronica
                    if (FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(this.idFactura) == 0)
                    {
                        //Validando que exista un Concepto
                        if (gvConceptosFacturaConcepto.SelectedIndex != -1)
                        {
                            //Instanciando Registro
                            using (FacturadoConcepto objFacturadoConcepto = new FacturadoConcepto(Convert.ToInt32(gvConceptosFacturaConcepto.SelectedDataKey["Id"])))
                            {   //Editando Valores del Concepto
                                result = objFacturadoConcepto.EditaFacturaConcepto(objFacturadoConcepto.id_factura, Convert.ToDecimal(txtCantidadFacturaConcepto.Text == "" ? "0" : txtCantidadFacturaConcepto.Text),
                                                                Convert.ToByte(ddlUnidadFacturaConcepto.SelectedValue), txtIdentificadorFacturaConcepto.Text, Convert.ToInt32(ddlConceptoCobroFacturaConcepto.SelectedValue),
                                                                Convert.ToDecimal(txtValorUniFacturaConcepto.Text == "" ? "0" : txtValorUniFacturaConcepto.Text), 0, Convert.ToByte(ddlImpuestoRetenido.SelectedValue),
                                                                Convert.ToDecimal(txtTasaImpRetFacturaConcepto.Text == "" ? "0" : txtTasaImpRetFacturaConcepto.Text), Convert.ToByte(ddlImpuestoTrasladado.SelectedValue),
                                                                Convert.ToDecimal(txtTasaImpTraFacturaConcepto.Text == "" ? "0" : txtTasaImpTraFacturaConcepto.Text),
                                                                objFacturadoConcepto.id_cargo_recurrente, ((Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        else
                            //Insertando Concepto
                            result = FacturadoConcepto.InsertaFacturaConcepto(this.idFactura, Convert.ToDecimal(txtCantidadFacturaConcepto.Text == "" ? "0" : txtCantidadFacturaConcepto.Text),
                                                                Convert.ToByte(ddlUnidadFacturaConcepto.SelectedValue), txtIdentificadorFacturaConcepto.Text, Convert.ToInt32(ddlConceptoCobroFacturaConcepto.SelectedValue),
                                                                Convert.ToDecimal(txtValorUniFacturaConcepto.Text == "" ? "0" : txtValorUniFacturaConcepto.Text), 0, Convert.ToByte(ddlImpuestoRetenido.SelectedValue),
                                                                Convert.ToDecimal(txtTasaImpRetFacturaConcepto.Text == "" ? "0" : txtTasaImpRetFacturaConcepto.Text), Convert.ToByte(ddlImpuestoTrasladado.SelectedValue),
                                                                Convert.ToDecimal(txtTasaImpTraFacturaConcepto.Text == "" ? "0" : txtTasaImpTraFacturaConcepto.Text),
                                                                0, ((Usuario)Session["usuario"]).id_usuario);
                    }
                    else
                        //Inicializando Contructor con Excepcion Personalizada
                        result = new RetornoOperacion("La Factura esta Registrada ó Timbrada en Facturación Electronica v3.2");
                }
                else
                    //Inicializando Contructor con Excepcion Personalizada
                    result = new RetornoOperacion("La Factura esta Registrada ó Timbrada en Facturación Electronica v3.3");
            }
            else
                //Inicializando Contructor con Excepcion Personalizada
                result = new RetornoOperacion("Debe existir la factura");
            
            //Validando que la Operación haya sido exitosa
            if (result.OperacionExitosa)
            {   //Inicializando Reporte
                inicializaReporteConceptos(this.idFactura);
                //Limpiando Controles
                limpiaControlesConcepto();
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Eliminar los Conceptos
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaFacturaConcepto()
        {
            //Declarando Objeto de Operacion
            RetornoOperacion resultCon = new RetornoOperacion();
            //Validando que existan Registros
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {
                //Instanciando Concepto
                using (FacturadoConcepto fc = new FacturadoConcepto(Convert.ToInt32(gvConceptosFacturaConcepto.SelectedDataKey["Id"])))
                {
                    //Validando que exista una Relación en Facturación Electronica 3.3
                    if (FacturadoFacturacion.ObtieneFacturacionElectronicaActivaV3_3(this.idFactura) == 0)
                    {
                        //Validando que exista una Relación en Facturación Electronica
                        if (FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(this.idFactura) == 0)
                            
                            //Deshabilitando Registro
                            resultCon = fc.DeshabilitaFacturaConcepto(((Usuario)Session["usuario"]).id_usuario);
                        else
                            //Inicializando Contructor con Excepcion Personalizada
                            resultCon = new RetornoOperacion("La Factura esta Registrada en Facturación Electronica v3.2");
                    }
                    else
                        //Inicializando Contructor con Excepcion Personalizada
                        resultCon = new RetornoOperacion("La Factura esta Registrada en Facturación Electronica v3.3");

                    //Validando que la Operacion fuera exitosa
                    if (resultCon.OperacionExitosa)
                    {
                        //Limpiando Controles
                        limpiaControlesConcepto();
                        //Obteniendo Conceptos
                        inicializaReporteConceptos(this.idFactura);
                    }
                }
            }
            else
                //Inicializando Parametros
                resultCon = new RetornoOperacion("No hay Conceptos");

            //Actualizando encabezado de Factura
            return resultCon;
        }

        #endregion
    }
}