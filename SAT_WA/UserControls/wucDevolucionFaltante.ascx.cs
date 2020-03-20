using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAT_CL.Despacho;
using TSDK.ASP;
using TSDK.Datos;
using TSDK.Base;
using System.Transactions;

namespace SAT.UserControls
{
    public partial class wucDevolucionFaltante : System.Web.UI.UserControl
    {
        #region Atributos

        private DevolucionFaltante _objDevolucionFaltante;
        private int _idDetalle;
        private int _idServicio;
        private int _idCompaniaEmisora;
        private int _idMovimiento;
        private int _idParada;
        private DataSet _dS;

        /// <summary>
        /// Atributo encargado de obtener el Objeto de Devolución
        /// </summary>
        public DevolucionFaltante objDevolucionFaltante { get { return this._objDevolucionFaltante; } }
        /// <summary>
        /// Atributo encargado de Obtener el Detalle
        /// </summary>
        public int idDetalle { get { return this._idDetalle; } }
        /// <summary>
        /// Propiedad TabIndex del Control de Usuario
        /// </summary>
        public short TabIndex
        {
            set
            {   
                //Encabezado
                ddlTipo.TabIndex =
                ddlEstatus.TabIndex =
                txtFechaDevolucion.TabIndex =
                txtObservacion.TabIndex =
                btnGuardar.TabIndex =
                btnCancelar.TabIndex = 
                btnAgregarRef.TabIndex =

                //Referencias
                ddlTamano.TabIndex = 
                lkbExcel.TabIndex = 
                gvReferencias.TabIndex =
                
                //Detalle
                ddlEstatusDet.TabIndex =
                txtCantidad.TabIndex =
                ddlUnidad.TabIndex =
                txtCodProducto.TabIndex =
                txtDescripcionProd.TabIndex =
                ddlRazonDet.TabIndex =
                btnGuardarDetalle.TabIndex =
                btnCancelarDetalle.TabIndex =
                gvDetalles.TabIndex = value;
            }
            get { return ddlTipo.TabIndex; }
        }
        /// <summary>
        /// Propiedad Enabled del Control de Usuario
        /// </summary>
        public bool Enabled
        {
            set
            {   
                //Controles
                ddlTipo.Enabled =
                ddlEstatus.Enabled =
                txtFechaDevolucion.Enabled =
                txtObservacion.Enabled =
                btnGuardar.Enabled =
                btnCancelar.Enabled =
                btnAgregarRef.Enabled =

                //Referencias
                ddlTamano.Enabled =
                lkbExcel.Enabled =
                gvReferencias.Enabled =

                //Detalle
                ddlEstatusDet.Enabled =
                txtCantidad.Enabled =
                ddlUnidad.Enabled =
                txtCodProducto.Enabled =
                txtDescripcionProd.Enabled =
                ddlRazonDet.Enabled =
                btnGuardarDetalle.Enabled =
                btnCancelarDetalle.Enabled =
                gvDetalles.Enabled = value;
            }
            get { return ddlTipo.Enabled; }
        }
        /// <summary>
        /// Atributo encargado de Obtener el Contenedor de los Campos Autocompleta
        /// </summary>
        public string Contenedor;

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

        #region Manejadores de Eventos

        /// <summary>
        /// Declarando el Evento ClickGuardarFactura
        /// </summary>
        public event EventHandler ClickGuardarDevolucion;
        /// <summary>
        /// Método que manipula el Evento "Guardar Devolucion"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarDevolucion(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarDevolucion != null)

                //Invocando al Delegado
                ClickGuardarDevolucion(this, e);
        }

        /// <summary>
        /// Declarando el Evento ClickGuardarFactura
        /// </summary>
        public event EventHandler ClickGuardarDevolucionDetalle;
        /// <summary>
        /// Método que manipula el Evento "Guardar Devolucion Detalle"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarDevolucionDetalle(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarDevolucionDetalle != null)

                //Invocando al Delegado
                ClickGuardarDevolucionDetalle(this, e);
        }

        /// <summary>
        /// Declarando el Evento ClickGuardarFactura
        /// </summary>
        public event EventHandler ClickEliminarDevolucionDetalle;
        /// <summary>
        /// Método que manipula el Evento "Eliminar Devolucion Detalle"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarDevolucionDetalle(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickEliminarDevolucionDetalle != null)

                //Invocando al Delegado
                ClickEliminarDevolucionDetalle(this, e);
        }

        /// <summary>
        /// Declarando el Evento ClickAgregarReferenciasDevolucion
        /// </summary>
        public event EventHandler ClickAgregarReferenciasDevolucion;
        /// <summary>
        /// Método que manipula el Evento "Agregar Referencias Devolución"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickAgregarReferenciasDevolucion(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickAgregarReferenciasDevolucion != null)

                //Invocando al Delegado
                ClickAgregarReferenciasDevolucion(this, e);
        }

        /// <summary>
        /// Declarando el Evento ClickAgregarReferenciasDetalle
        /// </summary>
        public event EventHandler ClickAgregarReferenciasDetalle;
        /// <summary>
        /// Método que manipula el Evento "Agregar Referencias Detalle"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickAgregarReferenciasDetalle(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickAgregarReferenciasDetalle != null)

                //Invocando al Delegado
                ClickAgregarReferenciasDetalle(this, e);
        }

        #endregion

        /// <summary>
        /// Evento Producido al Guardar la Devolución
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarDevolucion != null)
                //Inicializando
                OnClickGuardarDevolucion(e);
            return;
        }
        /// <summary>
        /// Evento Producido al Cancelar la Devolución
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Inicializando Control
            inicializaControl();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarDetalle_Click(object sender, EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarDevolucionDetalle != null)
                //Inicializando
                OnClickGuardarDevolucionDetalle(e);
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarDetalle_Click(object sender, EventArgs e)
        {
            //Limpiando Controles
            limpiaControles();

            //Inicializando Indices
            Controles.InicializaIndices(gvDetalles);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarRef_Click(object sender, EventArgs e)
        {
            //Validando que exista la Devolución
            if (this._objDevolucionFaltante.habilitar)
            {
                //Validando que este vacio el Evento
                if (ClickAgregarReferenciasDevolucion != null)
                    //Inicializando
                    OnClickAgregarReferenciasDevolucion(e);
                return;
            }
            else
                //Mostrando Excepción
                ScriptServer.MuestraNotificacion(this.Page, "No Existe la Devolución", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Texto del Control "Código"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCodProducto_TextChanged(object sender, EventArgs e)
        {
            //Instanciando producto
            using(SAT_CL.Despacho.DevolucionFaltanteProducto prod = DevolucionFaltanteProducto.ObtieneProducto(txtCodProducto.Text))
            {
                //Validando si existe el producto
                if (prod.habilitar)

                    //Asignando Descripción
                    txtDescripcionProd.Text = prod.descripcion_producto;
                else
                    //Asignando Descripción
                    txtDescripcionProd.Text = "";
            }
        }

        #region Eventos GridView "Referencias"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que Exista la Tabla
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table")))
                //Asignando Expresión de Ordenamiento
                this._dS.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

            //Cambiando de Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcel_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch(lkb.CommandName)
            {
                case "Referencias":
                    {
                        //Exportando Contenido del GridView
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table"));
                        break;
                    }
                case "Detalles":
                    {
                        //Exportando Contenido del GridView
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table1"));
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Exista la Tabla
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table")))
                //Asignando Expresión de Ordenamiento
                this._dS.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

            //Cambiando Expresión de Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Exista la Tabla
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table")))
                //Asignando Expresión de Ordenamiento
                this._dS.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table"), e.NewPageIndex, true, 1);
        }

        #endregion

        #region Eventos GridView "Devolución Detalle"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Para columnas de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encontrando controles de interés de Servicio
                using (LinkButton lkbReferenciaDetalle = (LinkButton)e.Row.FindControl("lkbReferenciaDetalle"))
                {
                    //Si no existe el Servicio
                    if (lkbReferenciaDetalle.Text == "")
                    {
                        //Asignamos Valor Default
                        lkbReferenciaDetalle.Text = "Sin Referencia";
                        //Habilitamos link
                        lkbReferenciaDetalle.Enabled = true;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Exista la Tabla
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table1")))
                //Asignando Expresión de Ordenamiento
                this._dS.Tables["Table1"].DefaultView.Sort = lblOrdenadoDet.Text;

            //Cambiando Expresión de Ordenamiento
            lblOrdenadoDet.Text = Controles.CambiaSortExpressionGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table1"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Exista la Tabla
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table1")))
                //Asignando Expresión de Ordenamiento
                this._dS.Tables["Table1"].DefaultView.Sort = lblOrdenadoDet.Text;

            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table1"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDet_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que Exista la Tabla
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table1")))
                //Asignando Expresión de Ordenamiento
                this._dS.Tables["Table1"].DefaultView.Sort = lblOrdenadoDet.Text;

            //Cambiando de Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet(this._dS, "Table1"), Convert.ToInt32(ddlTamanoDet.SelectedValue), true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferenciaDetalle_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if(gvDetalles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDetalles, sender, "lnk", false);

                //Asignando Variable
                this._idDetalle = Convert.ToInt32(gvDetalles.SelectedDataKey["Id"]);
                
                //Validando que este vacio el Evento
                if (ClickAgregarReferenciasDetalle != null)
                    //Inicializando
                    OnClickAgregarReferenciasDetalle(e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEditarDetalle_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if(gvDetalles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDetalles, sender, "lnk", false);

                //Instanciando Detalle
                using (DevolucionFaltanteDetalle dfd = new DevolucionFaltanteDetalle(Convert.ToInt32(gvDetalles.SelectedDataKey["Id"])))
                {
                    //Validando que Exista el Registro
                    if(dfd.habilitar)
                    {
                        //Asignando Valores
                        this._idDetalle = Convert.ToInt32(gvDetalles.SelectedDataKey["Id"]);
                        ddlEstatusDet.SelectedValue = dfd.id_estatus.ToString();
                        txtCantidad.Text = dfd.cantidad.ToString();
                        ddlUnidad.SelectedValue = dfd.id_unidad.ToString();
                        txtCodProducto.Text = dfd.codigo_producto;
                        txtDescripcionProd.Text = dfd.descripcion_producto;
                        ddlRazonDet.SelectedValue = dfd.id_razon_detalle.ToString();
                    }
                    else
                    {
                        //Asignando Valores
                        this._idDetalle = 0;
                        txtCantidad.Text = "0.00";
                        txtCodProducto.Text = 
                        txtDescripcionProd.Text = "";
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminarDetalle_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvDetalles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDetalles, sender, "lnk", false);

                //Validando que este vacio el Evento
                if (ClickEliminarDevolucionDetalle != null)
                    //Inicializando
                    OnClickEliminarDevolucionDetalle(e);
                return;
            }
        }
        /// <summary>
        /// Evento que permite imprimir el formato de devoluciones de producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbImprimir_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el gridViewDetalles
            if (objDevolucionFaltante.id_devolucion_faltante > 0)
            {
                //Obteniendo Ruta            
                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucDevolucionFaltante.aspx", "~/RDLC/Reporte.aspx");
                //Instanciando nueva ventana de navegador para apertura de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "CajaDevolucion", objDevolucionFaltante.id_devolucion_faltante),"CajaDevolucion", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);                                                                                        
            }

        }
        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Asignar el Valor de los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Valores a los ViewState
            ViewState["objDevolucion"] = _objDevolucionFaltante == null ? 0 : _objDevolucionFaltante.id_devolucion_faltante;
            ViewState["idDetalle"] = _idDetalle;
            ViewState["idCompaniaEmisora"] = _idCompaniaEmisora;
            ViewState["idServicio"] = _idServicio;
            ViewState["idMovimiento"] = _idMovimiento;
            ViewState["idParada"] = _idParada;
            ViewState["DS"] = this._dS;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que existan los Valores
            if (ViewState["objDevolucion"] != null)
                //Asignando Valor al Contructor
                this._objDevolucionFaltante = new DevolucionFaltante(Convert.ToInt32(ViewState["objDevolucion"]));

            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idCompaniaEmisora"]) != 0)
                //Asignando Valor al Atributo
                this._idCompaniaEmisora = Convert.ToInt32(ViewState["idCompaniaEmisora"]);

            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idDetalle"]) != 0)
                //Asignando Valor al Atributo
                this._idDetalle = Convert.ToInt32(ViewState["idDetalle"]);
            
            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idServicio"]) != 0)
                //Asignando Valor al Atributo
                this._idServicio = Convert.ToInt32(ViewState["idServicio"]);

            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idMovimiento"]) != 0)
                //Asignando Valor al Atributo
                this._idMovimiento = Convert.ToInt32(ViewState["idMovimiento"]);

            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idParada"]) != 0)
                //Asignando Valor al Atributo
                this._idParada = Convert.ToInt32(ViewState["idParada"]);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos((DataSet)ViewState["DS"], "Table") || Validacion.ValidaOrigenDatos((DataSet)ViewState["DS"], "Table1"))
                //Asignando Valor
                this._dS = (DataSet)ViewState["DS"];
        }
        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        private void inicializaControl()
        {
            //Validando si Existe una Devolución
            if(_objDevolucionFaltante.habilitar)
            {
                //Asignando Valores
                lblNoDevolucion.Text = _objDevolucionFaltante.consecutivo_compania.ToString();
                ddlTipo.SelectedValue = _objDevolucionFaltante.id_tipo.ToString();
                ddlEstatus.SelectedValue = _objDevolucionFaltante.id_estatus.ToString();
                txtFechaDevolucion.Text = _objDevolucionFaltante.fecha_devolucion_faltante.ToString("dd/MM/yyyy HH:mm");
                txtObservacion.Text = _objDevolucionFaltante.observacion;

                //Asignando Atributos
                this._idServicio = _objDevolucionFaltante.id_servicio;
                this._idMovimiento = _objDevolucionFaltante.id_movimiento;
                this._idParada = _objDevolucionFaltante.id_parada;
                this._idCompaniaEmisora = _objDevolucionFaltante.id_compania_emisora;
            }
            else
            {
                //Instanciando Devolución
                _objDevolucionFaltante = new DevolucionFaltante(this._idServicio, this._idMovimiento, this._idParada);

                //Validando si Existe una Devolución
                if (_objDevolucionFaltante.habilitar)
                {
                    //Asignando Valores
                    lblNoDevolucion.Text = _objDevolucionFaltante.consecutivo_compania.ToString();
                    ddlTipo.SelectedValue = _objDevolucionFaltante.id_tipo.ToString();
                    ddlEstatus.SelectedValue = _objDevolucionFaltante.id_estatus.ToString();
                    txtFechaDevolucion.Text = _objDevolucionFaltante.fecha_devolucion_faltante.ToString("dd/MM/yyyy HH:mm");
                    txtObservacion.Text = _objDevolucionFaltante.observacion;

                    //Asignando Atributos
                    this._idServicio = _objDevolucionFaltante.id_servicio;
                    this._idMovimiento = _objDevolucionFaltante.id_movimiento;
                    this._idParada = _objDevolucionFaltante.id_parada;
                    this._idCompaniaEmisora = _objDevolucionFaltante.id_compania_emisora;
                }
                else
                {
                    //Cargando Catalogos
                    cargaCatalogos();
                    //Configurando Controles para Captura
                    lblNoDevolucion.Text = "Por Asignar";
                    txtFechaDevolucion.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                    txtObservacion.Text = "";
                }
            }

            //Cargando Referencias
            cargaReferenciasDevolucion();

            //Limpiando Controles
            limpiaControles();

            //Invocando Método de Carga de Detalles
            inicializaDetalleDevolucion();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos del Control
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Devolución
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3143);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 3144);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusDet, "", 3144);

            //Cargando Tamaño de Reportes
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDet, "", 18);

            //Cargando Catalogo de Unidades y Detalles de Devolución
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidad, 44, "", 5, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlRazonDet, "", 3145);
        }
        /// <summary>
        /// Método encargado de Cargar las Referencias de la Devolución
        /// </summary>
        private void cargaReferenciasDevolucion()
        {
            //Obteniendo Referencias
            using(DataTable dtReferencia = SAT_CL.Global.Referencia.CargaReferenciasRegistro(this._idCompaniaEmisora, this._objDevolucionFaltante.id_devolucion_faltante, 156))
            {
                //Validando que Existen Referencias
                if(Validacion.ValidaOrigenDatos(dtReferencia))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvReferencias, dtReferencia, "Id", "", true, 1);

                    //Añadiendo Tabla a Session
                    this._dS = OrigenDatos.AñadeTablaDataSet(this._dS, dtReferencia, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvReferencias);

                    //Añadiendo Tabla a Session
                    this._dS = OrigenDatos.EliminaTablaDataSet(this._dS, "Table");
                }
            }
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles dado una Devolución
        /// </summary>
        private void inicializaDetalleDevolucion()
        {
            //Obteniendo Detalles
            using(DataTable dtDetalles = DevolucionFaltanteDetalle.ObtieneDetallesDevolucion(_objDevolucionFaltante.id_devolucion_faltante))
            {
                //Validando que Existan los Registros
                if(Validacion.ValidaOrigenDatos(dtDetalles))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvDetalles, dtDetalles, "Id", "", true, 3);

                    //Añadiendo a Session
                    this._dS = OrigenDatos.AñadeTablaDataSet(this._dS, dtDetalles, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvDetalles);

                    //Eliminando de Session
                    this._dS = OrigenDatos.EliminaTablaDataSet(this._dS, "Table1");
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvDetalles);
            }
        }
        /// <summary>
        /// Método encargado de Limpiar los Controles del Detalle
        /// </summary>
        private void limpiaControles()
        {
            //Limpiando Controles
            txtCantidad.Text =
            txtCodProducto.Text =
            txtDescripcionProd.Text = "";
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_parada">Parada</param>
        public void InicializaDevolucion(int id_compania_emisora, int id_servicio, int id_movimiento, int id_parada)
        {
            //Asignando Atributos
            this._objDevolucionFaltante = new DevolucionFaltante();
            this._idDetalle = 0;
            this._idCompaniaEmisora = id_compania_emisora;
            this._idServicio = id_servicio;
            this._idMovimiento = id_movimiento;
            this._idParada = id_parada;

            //Invocando Método de Inicialización
            inicializaControl();
        }
        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_devolucion">Devolución</param>
        public void InicializaDevolucion(int id_devolucion)
        {
            //Asignando Atributos
            this._objDevolucionFaltante = new DevolucionFaltante(id_devolucion);
            this._idDetalle = 0;
            this._idCompaniaEmisora = 0;
            this._idServicio = 0;
            this._idMovimiento = 0;
            this._idParada = 0;

            //Invocando Método de Inicialización
            inicializaControl();
        }
        /// <summary>
        /// Método encargado de Guardar la Devolución
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaDevolucion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable Auxiliar
            DateTime fecha_dev = DateTime.MinValue;
            DateTime.TryParse(txtFechaDevolucion.Text, out fecha_dev);

            //Validando que Exista la Devolución
            if(_objDevolucionFaltante.habilitar)
            {
                //Editanto Devolución
                result = _objDevolucionFaltante.EditaDevolucionesFaltantes(this._idCompaniaEmisora, this._objDevolucionFaltante.consecutivo_compania, this._objDevolucionFaltante.id_servicio,
                                this._objDevolucionFaltante.id_movimiento, this._objDevolucionFaltante.id_parada, (DevolucionFaltante.TipoDevolucion)Convert.ToByte(ddlTipo.SelectedValue), 
                                (DevolucionFaltante.EstatusDevolucion)Convert.ToByte(ddlEstatus.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                fecha_dev, txtObservacion.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario); 
            }
            else
            {
                //Insertando Devolución
                result = DevolucionFaltante.InsertaDevolucionesFaltantes(this._idCompaniaEmisora, 0, this._idServicio, this._idMovimiento, this._idParada,
                                (DevolucionFaltante.TipoDevolucion)Convert.ToByte(ddlTipo.SelectedValue), (DevolucionFaltante.EstatusDevolucion)Convert.ToByte(ddlEstatus.SelectedValue),
                                TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), fecha_dev, txtObservacion.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Validando que la Operación haya Sido Exitosa
            if(result.OperacionExitosa)
            {
                //Instanciando Devolución
                this._objDevolucionFaltante = new DevolucionFaltante(result.IdRegistro);
                this._idDetalle = 0;

                //Asignando Valores
                this._idServicio = _objDevolucionFaltante.id_servicio;
                this._idMovimiento = _objDevolucionFaltante.id_movimiento;
                this._idParada = _objDevolucionFaltante.id_parada;

                //Invocando Método de Inicializanción
                inicializaControl();
            }

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Guardar el Detalle de la Devolución
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaDetalleDevolucion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            int id_producto_devolucion = 0;

            //Inicializando Transacción
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que exista el Registro
                if (_objDevolucionFaltante.habilitar)
                {
                    //Validando que exista un Selección
                    if (gvDetalles.SelectedIndex != -1)
                    {
                        //Instanciando Detalle
                        using (DevolucionFaltanteDetalle dfd = new DevolucionFaltanteDetalle(Convert.ToInt32(gvDetalles.SelectedDataKey["Id"])))
                        {
                            //Validando que exista un Detalle
                            if (dfd.id_devolucion_faltante_detalle > 0)
                            {
                                //Obteniendo Producto
                                using (DevolucionFaltanteProducto producto = DevolucionFaltanteProducto.ObtieneProducto(txtCodProducto.Text))
                                {
                                    //Validando que exista el Producto
                                    if (!producto.habilitar)

                                        //Insertando Producto
                                        result = DevolucionFaltanteProducto.InsertarDevolucionFaltanteProducto(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                            txtDescripcionProd.Text.ToUpper(), txtCodProducto.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    else
                                        //Asignando Resultado Positivo
                                        result = new RetornoOperacion(producto.id_devolucion_faltante_producto, "", true);
                                }
                                
                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Asignando Producto
                                    id_producto_devolucion = result.IdRegistro;

                                    //Editando Detalle
                                    result = dfd.EditaDevolucionFaltanteDetalle(dfd.id_devolucion_faltante, (DevolucionFaltanteDetalle.EstatusDevolucionDetalle)Convert.ToByte(ddlEstatusDet.SelectedValue),
                                                                            id_producto_devolucion, Convert.ToDecimal(txtCantidad.Text == "" ? "0" : txtCantidad.Text),
                                                                            Convert.ToByte(ddlUnidad.SelectedValue), txtCodProducto.Text.ToUpper(), txtDescripcionProd.Text.ToUpper(), 
                                                                            (DevolucionFaltanteDetalle.RazonDetalle)Convert.ToInt32(ddlRazonDet.SelectedValue),
                                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No Existe el Detalle");
                        }
                    }
                    else
                    {
                        //Obteniendo Producto
                        using (DevolucionFaltanteProducto producto = DevolucionFaltanteProducto.ObtieneProducto(txtCodProducto.Text))
                        {
                            //Validando que exista el Producto
                            if (!producto.habilitar)

                                //Insertando Producto
                                result = DevolucionFaltanteProducto.InsertarDevolucionFaltanteProducto(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                    txtDescripcionProd.Text.ToUpper(), txtCodProducto.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Asignando Resultado Positivo
                                result = new RetornoOperacion(producto.id_devolucion_faltante_producto, "", true);
                        }

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Asignando Producto
                            id_producto_devolucion = result.IdRegistro;
                            
                            //Insertando Devolución
                            result = DevolucionFaltanteDetalle.InsertaDevolucionFaltanteDetalle(_objDevolucionFaltante.id_devolucion_faltante, id_producto_devolucion, 
                                            Convert.ToDecimal(txtCantidad.Text == "" ? "0" : txtCantidad.Text), Convert.ToByte(ddlUnidad.SelectedValue), 
                                            Cadena.RegresaCadenaSeparada(txtCodProducto.Text, "ID:", 0, "0").ToUpper(), Cadena.RegresaCadenaSeparada(txtDescripcionProd.Text, "ID:", 0, "0").ToUpper(), 
                                            (DevolucionFaltanteDetalle.RazonDetalle)Convert.ToInt32(ddlRazonDet.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe la Devolución");

                //Validando que la Operación haya sido Exitosa
                if (result.OperacionExitosa)

                    //Completando Transacción
                    trans.Complete();
            }


            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Inicializando Devolución
                InicializaDevolucion(this._objDevolucionFaltante.id_devolucion_faltante);

                //Limpiando Controles
                limpiaControles();
            }

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Eliminar el Detalle de la Devolución
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaDetalleDevolucion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que exista un Selección
            if(gvDetalles.SelectedIndex != -1)
            {
                //Instanciando Detalle
                using(DevolucionFaltanteDetalle dfd = new DevolucionFaltanteDetalle(Convert.ToInt32(gvDetalles.SelectedDataKey["Id"])))
                {
                    //Validando que exista un Detalle
                    if (dfd.id_devolucion_faltante_detalle > 0)

                        //Deshabilitando Detalle
                        result = dfd.DeshabilitaDevolucionFaltanteDetalle(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No Existe el Detalle");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe Seleccionar un Registro");

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Inicializando Detalle
                inicializaDetalleDevolucion();

                //Limpiando Controles
                limpiaControles();
            }

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion


    }
}