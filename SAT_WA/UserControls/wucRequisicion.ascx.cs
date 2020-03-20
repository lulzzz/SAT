using System;
using System.Data;
using System.Transactions;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using SAT_CL.Almacen;
using SAT_CL.Mantenimiento;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucRequisicion : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_requisicion;
        private int _id_orden;
        private int _id_orden_actividad;
        private int _id_servicio;
        private bool _maestro;
        private DataTable _dt;
        private bool _muestra_encabezado;
        /// <summary>
        /// Atributo que almacena la Requisición
        /// </summary>
        public int idRequisicion { get { return this._id_requisicion; } }
        /// <summary>
        /// Expresa el orden de Tabulación
        /// </summary>
        public short TabIndex
        {
            set
            {
                txtReferencia.TabIndex =
                txtAlmacen.TabIndex =
                txtFechaEntReq.TabIndex = value;
            }
            get { return txtAlmacen.TabIndex; }
        }
        /// <summary>
        /// Expresa la habilitación
        /// </summary>
        public bool Enabled
        {
            set
            {
                txtReferencia.Enabled =
                txtAlmacen.Enabled =
                txtFechaEntReq.Enabled = value;
            }
            get { return this.txtAlmacen.Enabled; }
 
        }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarRequisicion;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarRequisicion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickGuardarRequisicion != null)

                //Iniciando Evento
                ClickGuardarRequisicion(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Solicitar"
        /// </summary>
        public event EventHandler ClickSolicitarRequisicion;
        /// <summary>
        /// Evento que Manipula el Manejador "Solicitar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickSolicitarRequisicion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickSolicitarRequisicion != null)

                //Iniciando Evento
                ClickSolicitarRequisicion(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Referenciar"
        /// </summary>
        public event EventHandler ClickReferenciarRequisicion;
        /// <summary>
        /// Evento que Manipula el Manejador "Referenciar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickReferenciarRequisicion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickReferenciarRequisicion != null)

                //Iniciando Evento
                ClickReferenciarRequisicion(this, e);
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
            if (!Page.IsPostBack)

                //Asignando Atributos
                asignaAtributos();
            else
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Asignando Valores
            asignaAtributos();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarRequisicion != null)

                //Iniciando Manejador
                OnClickGuardarRequisicion(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSolicitar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickSolicitarRequisicion != null)

                //Iniciando Manejador
                OnClickSolicitarRequisicion(e);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Imprimir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            //Instancia a la clase requisicion
            using (SAT_CL.Almacen.Requisicion req = new SAT_CL.Almacen.Requisicion(this._id_requisicion))
            {
                //Valida que existan registros
                if (req.id_requisicion > 0)
                {
                    //Obteniendo Ruta
                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucRequisicion.ascx", "~/RDLC/Reporte.aspx");
                    //Instanciando nueva ventana de navegador para apertura de registro
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Requisicion", this._id_requisicion), "Requisicion", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                }
                else
                    //Instanciando Excepción
                    ScriptServer.MuestraNotificacion(btnImprimir, "No existe la Requisición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReferencias_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickReferenciarRequisicion != null)

                //Iniciando Manejador
                OnClickReferenciarRequisicion(e);
        }

        #region Eventos Detalles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarDet_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaDetalle();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarDet_Click(object sender, EventArgs e)
        {
            //Limpiando Controles
            limpiaControlesDetalles();

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvRequisicionDetalles);
        }

        #region Eventos GridView "Detalles"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Registros
            Controles.CambiaTamañoPaginaGridView(gvRequisicionDetalles, this._dt, Convert.ToInt32(ddlTamano.SelectedValue));

            //Sumando Totales
            sumaTotalesRequisicion();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Excel
            Controles.ExportaContenidoGridView(this._dt, "Id");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionDetalles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existen Registros
            if (gvRequisicionDetalles.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._dt.DefaultView.Sort = lblOrdenar.Text;

                //Cambiando Ordenamiento
                lblOrdenar.Text = Controles.CambiaSortExpressionGridView(gvRequisicionDetalles, this._dt, e.SortExpression);

                //Sumando Totales
                sumaTotalesRequisicion();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionDetalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existen Registros
            if (gvRequisicionDetalles.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._dt.DefaultView.Sort = lblOrdenar.Text;

                //Cambiando el Tamaño de la Página
                Controles.CambiaIndicePaginaGridView(gvRequisicionDetalles, this._dt, e.NewPageIndex);

                //Sumando Totales
                sumaTotalesRequisicion();
            }
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos en el GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Creando Menucontextual
            //Controles.CreaMenuContextual(e, "menuRequisicion", "menuRequisicionOpciones", "MostrarMenuReqDet", true, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Validando que Existen Registros
            if (gvRequisicionDetalles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvRequisicionDetalles, sender, "lnk", false);
                
                //Instanciando Detalle
                using(RequisicionDetalle rd = new RequisicionDetalle(Convert.ToInt32(gvRequisicionDetalles.SelectedDataKey["NoDetalle"])))
                {
                    //Validando que Exista el Registro
                    if(rd.IdDetalleRequisicion > 0)
                    {
                        //Asignando Valores
                        lblNoDetalle.Text = rd.IdDetalleRequisicion.ToString();
                        ddlEstatusDetalle.SelectedValue = rd.IdEstatus.ToString();
                        txtCantidadDet.Text = rd.Cantidad.ToString();

                        //Instanciando Producto
                        using(Producto pro = new Producto(rd.IdProducto))
                        {
                            //Validando que Exista el Producto
                            if (pro.id_producto > 0)

                                //Asignando Valor
                                txtProductoDet.Text = pro.descripcion + "[" + pro.sku + "]" + " ID:" + pro.id_producto.ToString();
                            else
                                //Limpiando Valor
                                txtProductoDet.Text = "";
                        }
                    }
                    else
                    {
                        //Asignando Valores
                        lblNoDetalle.Text = "Por Asignar";
                        txtCantidadDet.Text = "0.00";
                        
                        //Mostrando Mensaje
                        ScriptServer.MuestraNotificacion(this, "No se pudo acceder al Registro", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Limpiando Controles
                        limpiaControlesDetalles();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando que Existen Registros
            if (gvRequisicionDetalles.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvRequisicionDetalles, sender, "lnk", false);
                
                //Instanciando Detalle
                using (RequisicionDetalle rd = new RequisicionDetalle(Convert.ToInt32(gvRequisicionDetalles.SelectedDataKey["NoDetalle"])))
                {
                    //Validando que Exista el Registro
                    if (rd.IdDetalleRequisicion > 0)

                        //Deshabilitando Detalle
                        result = rd.DeshabilitaDetalleRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }

                //Validando que la Operación haya sido Exitosa
                if(result.OperacionExitosa)
                {
                    //Limpiando Controles
                    limpiaControlesDetalles();

                    //Cargando Detalles
                    cargaDetallesRequisicion(this._id_requisicion);
                }

                //Mostrando Mensaje
                ScriptServer.MuestraNotificacion(gvRequisicionDetalles, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar los Valores y Controles del Control
        /// </summary>
        private void inicializaControlUsuario()
        {
            //Cargando Catalogos
            cargaCatalogos();

            //Validando que sea Verdadero
            if (this._muestra_encabezado)

                //Mostrando Contenido del Encabezado
                divEncabezado.Visible = true;
            else
                //Ocultando Contenido del Encabezado
                divEncabezado.Visible = false;

            //Instanciando Requisición
            using (Requisicion req = new Requisicion(this._id_requisicion))
            {
                //Validando que Exista la Requisición
                if (req.id_requisicion > 0)
                {
                    if (req.id_tipo == 1)

                        //Valida el ddltipo
                        habilitaControlesDetalles(false);
                    else
                        //Asignando Tipo de Trabajo
                        ddlTipo.SelectedValue = "2";

                    //Asignando Servicio
                    this._id_servicio = req.id_servicio;

                    //Asignando Valores
                    lblNoRequisicion.Text = req.no_requisicion.ToString();
                    txtFechaSolicitud.Text = req.fecha_solitud.ToString("dd/MM/yyyy HH:mm");
                    txtFechaEntReq.Text = req.fecha_entrega_requerida == DateTime.MinValue ? "" : req.fecha_entrega_requerida.ToString("dd/MM/yyyy HH:mm");
                    txtFechaEntrega.Text = req.fecha_entrega == DateTime.MinValue ? "" : req.fecha_entrega.ToString("dd/MM/yyyy HH:mm");
                    ddlEstatus.SelectedValue = req.id_estatus.ToString();
                    txtReferencia.Text = req.referencia;
                    ddlTipo.SelectedValue = req.id_tipo.ToString();

                    //Instanciando Compania
                    using (SAT_CL.Global.CompaniaEmisorReceptor com = new SAT_CL.Global.CompaniaEmisorReceptor(req.id_compania_emisora))
                    {
                        //Validando que Exista la Compania
                        if (com.id_compania_emisor_receptor > 0)

                            //Asignando Compania
                            txtCompaniaEmisora.Text = com.nombre;
                        else
                            //Limpiando Control
                            txtCompaniaEmisora.Text = "";
                    }
                    
                    //Validando que sea de Tipo Maestro
                    if (req.id_tipo == 1)
                    
                        //Asignando Almacen
                        txtAlmacen.Text = "ALMACEN GENERAL ID:0";
                    else
                    {
                        //Instanciando Almacen
                        using (SAT_CL.Almacen.Almacen al = new SAT_CL.Almacen.Almacen(req.id_almacen))
                        {
                            //Validando que exista el Almacen
                            if (al.id_almacen > 0)

                                //Asignando Almacen
                                txtAlmacen.Text = al.descripcion + " ID:" + al.id_almacen.ToString();
                            else
                                //Limpiando Control
                                txtAlmacen.Text = "";
                        }
                    }

                    //Instanciando Usuario Solicitante
                    using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(req.id_usuario_solicitante))
                    {
                        //Validando que Exista el Usuario Solicitante
                        if (user.id_usuario > 0)

                            //Asignado Usuario Solicitante
                            txtUsuarioSolicitante.Text = user.nombre;
                        else
                            //Asignado Usuario Solicitante
                            txtUsuarioSolicitante.Text = "";
                    }

                    //Cargando Detalles
                    cargaDetallesRequisicion(req.id_requisicion);
                }
                else
                {
                    //Validando que sea de Tipo Maestro
                    if (this._maestro)
                    {
                        ddlTipo.SelectedValue = "1";
                        ddlTipo.Enabled =
                        txtAlmacen.Enabled = false;
                        txtAlmacen.Text = "ALMACEN GENERAL ID:0";
                    }
                    else
                    {
                        ddlTipo.SelectedValue = "2";
                        txtAlmacen.Text = "";
                    }

                    //Asignando Valores
                    lblNoRequisicion.Text = "Por Asignar";
                    txtReferencia.Text = "";
                    txtFechaSolicitud.Text =
                    txtFechaEntReq.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                    txtFechaEntrega.Text = "";
                    
                    //Instanciando Compania
                    using (SAT_CL.Global.CompaniaEmisorReceptor com = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        //Validando que Exista la Compania
                        if (com.id_compania_emisor_receptor > 0)

                            //Asignando Compania
                            txtCompaniaEmisora.Text = com.nombre;
                        else
                            //Limpiando Control
                            txtCompaniaEmisora.Text = "";
                    }
                    //Instanciando Usuario Solicitante
                    using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
                    {
                        //Validando que Exista el Usuario Solicitante
                        if (user.id_usuario > 0)

                            //Asignado Usuario Solicitante
                            txtUsuarioSolicitante.Text = user.nombre;
                        else
                            //Asignado Usuario Solicitante
                            txtUsuarioSolicitante.Text = "";
                    }

                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvRequisicionDetalles);

                    //Añadiendo Tabla
                    this._dt = null;
                }

                //Configurando Controles de Detalles
                limpiaControlesDetalles();
                habilitaControlesDetalles(req.id_requisicion > 0? true : false);
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            //Tipo y Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 2125);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 2126);
            //Estatus (Detalle)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusDetalle, "", 3157);
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Guardando Atributo en ViewState
            ViewState["IdRequisicion"] = this._id_requisicion;
            ViewState["IdOrden"] = this._id_orden;
            ViewState["IdOrdenActividad"] = this._id_orden_actividad;
            ViewState["IdServicio"] = this._id_servicio;
            ViewState["Maestro"] = this._maestro;
            ViewState["DT"] = this._dt;
            ViewState["MuestraEncabezado"] = this._muestra_encabezado;
        }
        /// <summary>
        /// Método encaragdo de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Exista el Atributo
            if (Convert.ToInt32(ViewState["IdRequisicion"]) != 0)
                //Asignando Valor al Atributo
                this._id_requisicion = Convert.ToInt32(ViewState["IdRequisicion"]);
            //Validando que Exista el Atributo
            if (Convert.ToInt32(ViewState["IdOrden"]) != 0)
                //Asignando Valor al Atributo
                this._id_orden = Convert.ToInt32(ViewState["IdOrden"]);
            //Validando que Exista el Atributo
            if (Convert.ToInt32(ViewState["IdOrdenActividad"]) != 0)
                //Asignando Valor al Atributo
                this._id_orden_actividad = Convert.ToInt32(ViewState["IdOrdenActividad"]);
            //Validando que Exista el Atributo
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)
                //Asignando Valor al Atributo
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);
            //Validando que Exista el Atributo
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                //Asignando Valor al Atributo
                this._dt = (DataTable)ViewState["DT"];
            //Valida que exista el atributo
            if (Convert.ToBoolean(ViewState["Maestro"]) != false)
                //Asigna valor al atributo
                this._maestro = Convert.ToBoolean(ViewState["Maestro"]);
            //Valida que exista el atributo
            if (ViewState["MuestraEncabezado"] != null)
                //Asigna valor al atributo
                this._muestra_encabezado = Convert.ToBoolean(ViewState["MuestraEncabezado"]);
        }
        /// <summary>
        /// Método encargado de Cargar los Detalles de las Requisiciones
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        private void cargaDetallesRequisicion(int id_requisicion)
        {
            //Obteniendo Detalles
            using (DataTable dtDetallesReq = RequisicionDetalle.ObtieneDetallesRequisicion(this._id_requisicion))
            {
                //Validando que Existen Detalles
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesReq))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvRequisicionDetalles, dtDetallesReq, "NoDetalle", "");

                    //Añadiendo Tabla
                    this._dt = dtDetallesReq;
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvRequisicionDetalles);

                    //Añadiendo Tabla
                    this._dt = null;
                }
            }

            //Sumando Totales
            sumaTotalesRequisicion();
        }
        /// <summary>
        /// Método encargado de Sumar los Totales de la Requisición
        /// </summary>
        private void sumaTotalesRequisicion()
        {
            //Validando que existan Registros
            if(Validacion.ValidaOrigenDatos(this._dt))
            {
                //Sumando Totales
                gvRequisicionDetalles.FooterRow.Cells[4].Text = string.Format("{0:0.00}", Convert.ToDecimal(this._dt.Compute("SUM(Cantidad)", "")));
                gvRequisicionDetalles.FooterRow.Cells[7].Text = string.Format("{0:C2}", Convert.ToDecimal(this._dt.Compute("SUM(Total)", "")));
            }
            else
            {
                //Sumando Totales
                gvRequisicionDetalles.FooterRow.Cells[4].Text = string.Format("{0:0:00}", 0);
                gvRequisicionDetalles.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método encargado de Habilitar los Controles
        /// </summary>
        /// <param name="enable"></param>
        private void habilitaControlesDetalles(bool enable)
        {
            //Habilitando Controles
            txtProductoDet.Enabled =
            txtCantidadDet.Enabled = 
            ddlTamano.Enabled =
            lnkExportar.Enabled =
            gvRequisicionDetalles.Enabled = enable;
        }
        /// <summary>
        /// Método encargado de Limpiar lo Controles
        /// </summary>
        private void limpiaControlesDetalles()
        {
            //Limpiando Controles
            lblNoDetalle.Text = "Por Asignar";
            txtProductoDet.Text =
            txtCantidadDet.Text = "";
            gvRequisicionDetalles.SelectedIndex = -1;
        }
        /// <summary>
        /// Método encargado de Guardar el Detalle de la Requisición
        /// </summary>
        private void guardaDetalle()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que Exista un Registro Seleccionado
            if(gvRequisicionDetalles.SelectedIndex != -1)
            {
                //Instanciando Detalle
                using (RequisicionDetalle rd = new RequisicionDetalle(Convert.ToInt32(gvRequisicionDetalles.SelectedDataKey["NoDetalle"])))
                {
                    //Validando que exista el Detalle
                    if (rd.Habilitar && (RequisicionDetalle.EstatusDetalle)rd.IdEstatus == RequisicionDetalle.EstatusDetalle.Registrado)
                    {
                        //Instancia a la clase producto de almacen
                        using (SAT_CL.Almacen.Producto prod = new SAT_CL.Almacen.Producto(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProductoDet.Text, "ID:", 1, "0"))))
                        {
                            //Validando que exista el producto
                            if (prod.id_producto > 0)
                            {
                                //Editando Detalle
                                result = rd.EditaDetalleRequisicion((RequisicionDetalle.EstatusDetalle)rd.IdEstatus, Convert.ToDecimal(txtCantidadDet.Text),
                                                        prod.id_unidad, prod.id_producto, prod.sku, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("El Detalle debe de estar 'Registrado' para su Edición");
                }
            }
            else
            {
                //Instancia a la clase requisicion
                using (SAT_CL.Almacen.Requisicion req = new SAT_CL.Almacen.Requisicion(this._id_requisicion))
                {
                    //Si el estatus del registro de requisición es:capturado, por autorizar, pendiente almacen, abastecido, abastecido parcial y cancelas
                    if (req.id_estatus != 3)
                        //Instancia a la claso producto de alamcen
                        using(SAT_CL.Almacen.Producto prod = new SAT_CL.Almacen.Producto(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProductoDet.Text, "ID:", 1, "0"))))
                        {
                            //Insertando Detalle
                            result = RequisicionDetalle.InsertaDetalleRequisicion(this._id_requisicion, Convert.ToDecimal(txtCantidadDet.Text), prod.id_unidad,
                                                    prod.id_producto, prod.sku, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }

                    //En caso contrario
                    else
                        //Envia un mensaje de error
                        result = new RetornoOperacion("No se puede agregar producto ya que la requisición fue solicitada.");
                }
            }
 
            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Limpiando Controles
                limpiaControlesDetalles();

                //Habilitando Controles
                habilitaControlesDetalles(true);

                //Cargando Detalles
                cargaDetallesRequisicion(this._id_requisicion);
            }

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(btnGuardarDet, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Requisición
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        /// <param name="id_orden">Orden de Trabajo</param>
        /// <param name="id_orden_actividad">Actividad de la Orden de Trabajo</param>
        /// <param name="id_servicio">Servicio</param>
        public void InicializaRequisicion(int id_requisicion, int id_orden, int id_orden_actividad, int id_servicio)
        {
            //Asignando Atributos
            this._id_requisicion = id_requisicion;
            this._id_orden = id_orden;
            this._id_orden_actividad = id_orden_actividad;
            this._id_servicio = id_servicio;
            this._maestro = false;
            this._muestra_encabezado = true;
            
            //Invocando Método de Inicialización
            inicializaControlUsuario();
        }
        /// <summary>
        /// Método encargado de Inicializar los Valores de la Requisición
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        /// <param name="id_orden">Orden de Trabajo</param>
        /// <param name="id_orden_actividad">Actividad de la Orden de Trabajo</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="maestro">Indicador de Requisición Maestra</param>
        public void InicializaRequisicion(int id_requisicion, int id_orden, int id_orden_actividad, int id_servicio, bool maestro)
        {
            //Asignando Atributos
            this._id_requisicion = id_requisicion;
            this._id_orden = id_orden;
            this._id_orden_actividad = id_orden_actividad;
            this._id_servicio = id_servicio;
            this._maestro = maestro;
            this._muestra_encabezado = true;

            //Invocando Método de Inicialización
            inicializaControlUsuario();
        }
        /// <summary>
        /// Método encargado de Inicializar los Valores de la Requisición
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        /// <param name="id_orden">Orden de Trabajo</param>
        /// <param name="id_orden_actividad">Actividad de la Orden de Trabajo</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="maestro">Indicador de Requisición Maestra</param>
        /// <param name="muestra_encabezado">Indicador que Oculta o Muestra el Encabezado</param>
        public void InicializaRequisicion(int id_requisicion, int id_orden, int id_orden_actividad, int id_servicio, bool maestro, bool muestra_encabezado)
        {
            //Asignando Atributos
            this._id_requisicion = id_requisicion;
            this._id_orden = id_orden;
            this._id_orden_actividad = id_orden_actividad;
            this._id_servicio = id_servicio;
            this._maestro = maestro;
            this._muestra_encabezado = muestra_encabezado;

            //Invocando Método de Inicialización
            inicializaControlUsuario();
        }
        /// <summary>
        /// Método encargado de Guardar la Requisición
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaRequisicion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha
            DateTime fecha_entrega_req = DateTime.MinValue;
            DateTime.TryParse(txtFechaEntReq.Text, out fecha_entrega_req);

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que Exista la Requisición
                if (this._id_requisicion > 0)
                {
                    //Instanciando Requisición
                    using (Requisicion req = new Requisicion(this._id_requisicion))
                    {
                        //Validando que Exista la Requisición
                        if (req.id_requisicion > 0)

                            //Editando Requisición
                            result = req.EditaRequisicion(req.no_requisicion, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                          this._id_servicio, (Requisicion.Estatus)Convert.ToByte(ddlEstatus.SelectedValue), txtReferencia.Text, Convert.ToByte(ddlTipo.SelectedValue),
                                                          Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtAlmacen.Text, "ID:", 1, "0")),
                                                          req.id_usuario_solicitante, req.fecha_solitud, fecha_entrega_req, req.fecha_entrega,
                                                          ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando que la Operación fuese Exitosa
                        if(result.OperacionExitosa)
                            //Completando Transacción
                            trans.Complete();
                    }
                }
                else
                {
                    //Insertando la Requisición
                    result = Requisicion.InsertaRequisicion(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                            this._id_servicio, Convert.ToByte(ddlEstatus.SelectedValue), txtReferencia.Text, Convert.ToByte(ddlTipo.SelectedValue),
                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtAlmacen.Text, "ID:", 1, "0")),
                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 
                                                            fecha_entrega_req, DateTime.MinValue, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validando que la Operación fuese Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Requisición
                        int idRequisicion = result.IdRegistro;

                        //Validando que Existan Orden y Actividad
                        if (this._id_orden_actividad > 0 && this._id_orden > 0)
                        {
                            //Insertando Requisición de Actividad de la Orden de Trabajo
                            result = OrdenTrabajoActividadRequisicion.InsertaOrdenActividadRequisicion(this._id_orden, this._id_orden_actividad,
                                        idRequisicion, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                                //Instanciando Requisición
                                result = new RetornoOperacion(idRequisicion);
                        }

                        //Validando que la Operación fuese Exitosa
                        if (result.OperacionExitosa)
                            //Completando Transacción
                            trans.Complete();
                    }
                }
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Valores
                this._id_requisicion = result.IdRegistro;

                //Inicializando Control
                inicializaControlUsuario();
            }

            //Mostrando Mensaje de Operación
            //ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Solicitar la Requisición
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion SolicitaRequisicion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Requisición
            using (Requisicion req = new Requisicion(this._id_requisicion))
            {
                //Validando que Exista la Requisición
                if (req.id_requisicion > 0)

                    //Editando Requisición
                    result = req.SolicitaRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se puede Acceder al Registro");
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Requisición
                this._id_requisicion = result.IdRegistro;
                
                //Inicializando Control
                inicializaControlUsuario();
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}