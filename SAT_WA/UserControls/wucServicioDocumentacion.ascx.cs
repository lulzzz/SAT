using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucServicioDocumentacion : System.Web.UI.UserControl
    {
        #region Enumeraciones

        /// <summary>
        /// Define las vistas posibles del control
        /// </summary>
        public enum VistaDocumentacion
        {
            /// <summary>
            /// Vista del encabezado (Edición)
            /// </summary>
            Encabezado = 0,
            /// <summary>
            /// Vista Paradas (Edición)
            /// </summary>
            Paradas,
            /// <summary>
            /// Clasificación del Servicio (Edición)
            /// </summary>
            Clasificacion,
            /// <summary>
            /// Resumen de Servicio
            /// </summary>
            Resumen
        }

        #endregion

        #region Atributos

        private int _id_servicio;
        private int _id_parada;
        private bool _nueva_documentacion;
        private bool _hab_edicion;
        private VistaDocumentacion _vista_inicial;

        /// <summary>
        /// Obtiene el Id de Servicio activo en el control
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        /// <summary>
        /// Obtiene el Id de Parada seleccionada en el GV de Paradas
        /// </summary>
        public int id_parada { get { return this._id_parada; } }
        /// <summary>
        /// Obtiene el valor que indica si se ha realizado una nueva documentación
        /// </summary>
        public bool nueva_documentacion { get { return this._nueva_documentacion; } }

        private DataTable _mit_parada;
        private DataTable _mit_evento;
        private DataTable _mit_producto;
        private DataTable _mit_resumen;

        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Manejadores de Eventos

        /// <summary>
        /// Manejador de Evento Click en Botón Agregar parada
        /// </summary>
        public event EventHandler ImbAgregarParada_Click;
        /// <summary>
        /// Manejador de Evento Click en Botón Agregar producto
        /// </summary>
        public event EventHandler ImbAgregarProducto_Click;
        /// <summary>
        /// Manejador de Evento Click en Botón Eliminar parada
        /// </summary>
        public event EventHandler LkbEliminarParada_Click;
        /// <summary>
        /// Manejador de evento Click en botón eliminar producto
        /// </summary>
        public event EventHandler LkbEliminarProducto_Click;
        /// <summary>
        /// Manejador de evento CLick en botón Aceptar del encabezado de servicio
        /// </summary>
        public event EventHandler BtnAceptarEncabezado_Click;
        /// <summary>
        /// Manejador de evento Click en botón Citas de Eventos
        /// </summary>
        public event EventHandler LkbCitasEventos_Click;


        /// <summary>
        /// Maneja Click en botón AgregarParada
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnImbAgregarParada_Click(EventArgs e)
        {
            if (this.ImbAgregarParada_Click != null)
                this.ImbAgregarParada_Click(this, e);
        }
        /// <summary>
        /// Maneja Click en botón Agregar producto
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnImbAgregarProducto_Click(EventArgs e)
        {
            if (this.ImbAgregarProducto_Click != null)
                this.ImbAgregarProducto_Click(this, e);
        }
        /// <summary>
        /// Maneja Click en botón Eliminar parada
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnLkbEliminarParada_Click(EventArgs e)
        {
            if (this.LkbEliminarParada_Click != null)
                this.LkbEliminarParada_Click(this, e);
        }
        /// <summary>
        /// Maneja Click en botón Eliminar Producto
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnLkbEliminarProducto_Click(EventArgs e)
        {
            if (this.LkbEliminarProducto_Click != null)
                this.LkbEliminarProducto_Click(this, e);
        }
        /// <summary>
        /// Maneja Click en botón Aceptar del Encabezado
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnBtnAceptarEncabezado_Click(EventArgs e)
        {
            if (this.BtnAceptarEncabezado_Click != null)
                this.BtnAceptarEncabezado_Click(this, e);
        }
        /// <summary>
        /// Maneja Click en botón Citas de Eventos
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnLkbCitasEventos_Click(EventArgs e)
        {
            if (this.LkbCitasEventos_Click != null)
                this.LkbCitasEventos_Click(this, e);
        }

        #endregion

        #region Eventos Generales

        /// <summary>
        /// Carga del control web de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (Page.IsPostBack)
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Recuperando Atributos
            asignaAtributos();
        }
        /// <summary>
        /// Click en pestañas de vista de control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVistaServicioDocumentacion_OnClick(object sender, EventArgs e)
        {
            //Determinando que vista debe ser mostrada en base al botón pulsado
            switch (((Button)sender).CommandName)
            {
                case "Encabezado":
                    mtvDocumentacion.SetActiveView(vwEncabezado);
                    break;
                case "Paradas":
                    mtvDocumentacion.SetActiveView(vwParadas);
                    break;
                case "Resumen":
                    mtvDocumentacion.SetActiveView(vwResumen);
                    //Actualziando contenido de pestaña
                    inicializaResumenServicio();
                    break;
                case "Clasificacion":
                    mtvDocumentacion.SetActiveView(vwClasificacion);
                    wucClasificacion.InicializaControl(1, this._id_servicio, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                    break;
            }

            //Aplicando estilo correspondiente
            asignaEstiloTabs();
        }

        #endregion

        #region Eventos Encabezado

        /// <summary>
        /// Click en botón Aceptar en el encabezado de documentación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEncabezadoDocumentacion_Click(object sender, EventArgs e)
        {
            //Determinando el comando a realizar
            switch (((Button)sender).CommandName)
            {
                //Gardado de encabezado
                case "Aceptar":
                    if (BtnAceptarEncabezado_Click != null)
                        OnBtnAceptarEncabezado_Click(e);
                    break;
            }
        }

        #endregion

        #region Eventos Paradas

        /// <summary>
        /// Click en botón Agregar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbAgregar_Click(object sender, EventArgs e)
        {
            //Obteniendo la referencia del botón pulsado y determinando la acción a realizar
            switch (((LinkButton)sender).CommandName)
            {
                case "Parada":
                    //Validando datos requeridos
                    //Generando evento click del control correspondiente
                    if (this.imbAgregarParada != null)
                        OnImbAgregarParada_Click(e);
                    break;
                case "Producto":
                    //Generando evento click del control correspondiente
                    if (this.imbAgregarProducto != null)
                        OnImbAgregarProducto_Click(e);
                    break;
            }
        }
        /// <summary>
        /// Cambio de indice de página de GV de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadasDocumentacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando indice de página
            Controles.CambiaIndicePaginaGridView(gvParadasDocumentacion, this._mit_parada, e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Cambio de indice de página de GV de Productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductoEvento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando indice de página
            Controles.CambiaIndicePaginaGridView(gvProductoEvento, this._mit_producto, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbParada_Click(object sender, ImageClickEventArgs e)
        {
            //Si hay elementos en el Gridview
            if (gvParadasDocumentacion.DataKeys.Count > 0)
            {
                //Seleccionando fila 
                Controles.SeleccionaFila(gvParadasDocumentacion, sender, "imb", false);

                //Asignando id de parada seleccionada
                this._id_parada = Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"]);

                //Determinando el comando a ejecutar
                switch (((ImageButton)sender).CommandName)
                {
                    case "Editar":
                        //Configurando controles para edición
                        inicializaControlesParadaEdicion();
                        //limpiando productos
                        this._mit_evento.Clear();
                        this._mit_producto.Clear();
                        Controles.InicializaGridview(gvProductoEvento);
                        inicializaControlesProducto();
                        //Asignando foco en control de cita
                        txtCitaParada.Focus();
                        break;
                    case "Eliminar":
                        //Generando evento
                        if (this.LkbEliminarParada_Click != null)
                            OnLkbEliminarParada_Click(e);
                        break;
                    case "Referencias":
                        //Si hay un registro en sesión
                        if (this._id_parada.ToString() != "0")
                            inicializaReferenciaRegistro(this._id_parada.ToString(), "5", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        else
                            ScriptServer.MuestraNotificacion(this.Page, "Para agregar referencias a la parada, es necesario guardar el servicio.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                }
            }
        }
        /// <summary>
        /// Click en botón de GV de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbParada_Click(object sender, EventArgs e)
        {
            //Si hay elementos en el Gridview
            if (gvParadasDocumentacion.DataKeys.Count > 0)
            {
                //Seleccionando fila 
                Controles.SeleccionaFila(gvParadasDocumentacion, sender, "lnk", false);

                //Asignando id de parada seleccionada
                this._id_parada = Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"]);

                //Determinando el comando a ejecutar
                switch (((LinkButton)sender).CommandName)
                {
                    case "Seleccionar":
                        //Configurando controles para captura
                        inicializaControlesParada();
                        //Cargando conjunto de productos y eventos de la parada
                        cargaProductos();
                        //limpiando productos
                        inicializaControlesProducto();
                        txtProductoEventoParada.Focus();
                        break;
                    case "CitasEventos":
                        //Si hay manejador de eventos asignado y existe un Id de Servicio activo
                        if (this.LkbCitasEventos_Click != null && this._id_servicio > 0)
                            //Produciendo evento
                            OnLkbCitasEventos_Click(e);
                        break;
                }
            }
        }
        /// <summary>
        /// Click en botón de GV de Productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbProducto_Click(object sender, ImageClickEventArgs e)
        {
            //Si hay elementos en el Gridview
            if (gvProductoEvento.DataKeys.Count > 0)
            {
                //Seleccionando fila 
                Controles.SeleccionaFila(gvProductoEvento, sender, "imb", false);

                //Determinando el comando a ejecutar
                switch (((ImageButton)sender).CommandName)
                {
                    case "Editar":
                        //Cargando datos de producto para su edición
                        inicializaControlesProductoEdicion();
                        break;
                    case "Eliminar":
                        //Generando evento
                        if (this.LkbEliminarParada_Click != null)
                            OnLkbEliminarProducto_Click(e);
                        break;
                }
            }
        }
        /// <summary>
        /// Cambio de tipo de evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoEventoParada_SelectedIndexChanged(object sender, EventArgs e)
        {
            visualizaProductoTipoEvento();

            //Limpiando producto para evitar errores de visualización
            txtProductoEventoParada.Text = "";
        }        

        #endregion

        #region Eventos Resumen Servicio

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Paradas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvParadas, this._mit_resumen, e.NewPageIndex, true, 3);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño  Paradas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoParadas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvParadas, this._mit_resumen,
                                        Convert.ToInt32(ddlTamanoParadas.SelectedValue), true, 3);
        }


        /// <summary>
        /// Evento Producido al pulsar el botón "Exportar Paradas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarParadas_OnClick(object sender, EventArgs e)
        {
            //Exportando de Servicios
            Controles.ExportaContenidoGridView(this._mit_resumen);
        }

        /// <summary>
        /// Evento generado al enlzar los Datos de la Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Paradas
                if (gvParadas.DataKeys.Count > 0)
                {
                    //Buscamos Grid View de Productos
                    using (GridView gvProductos = (GridView)e.Row.FindControl("gvProductos"))
                    {
                        //Carga Eventos para cada una de las Paradas
                        using (DataTable mit = SAT_CL.Documentacion.ServicioProducto.ObtieneProductosParada(Convert.ToInt32(gvParadas.DataKeys[e.Row.RowIndex].Value)))
                        {
                            //Validamos Origen de Datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                            {
                                //Cargamos Grid View Eventos
                                TSDK.ASP.Controles.CargaGridView(gvProductos, mit, "Id", "");

                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Aplica el estilo (css) a los botónes tab del control en base a la vista activa actual
        /// </summary>
        private void asignaEstiloTabs()
        { 
            //Determinando la vista activa
            switch ((mtvDocumentacion.GetActiveView().ID))
            {
                case "vwEncabezado":
                    btnVistaEcabezadoServicio.CssClass = "boton_pestana_activo";
                    btnVistaParadasServicio.CssClass = "boton_pestana";
                    btnVistaResumenServicio.CssClass = "boton_pestana";
                    btnVistaClasificacion.CssClass = "boton_pestana";

                    //Asignando foco predeterminado
                    txtClienteServicio.Focus();
                    break;
                case "vwParadas":
                    btnVistaEcabezadoServicio.CssClass = "boton_pestana";
                    btnVistaParadasServicio.CssClass = "boton_pestana_activo";
                    btnVistaResumenServicio.CssClass = "boton_pestana";
                    btnVistaClasificacion.CssClass = "boton_pestana";

                    //Asignando foco predeterminado
                    txtUbicacionParada.Focus();
                    break;
                case "vwResumen":
                    btnVistaEcabezadoServicio.CssClass = "boton_pestana";
                    btnVistaParadasServicio.CssClass = "boton_pestana";
                    btnVistaResumenServicio.CssClass = "boton_pestana_activo";
                    btnVistaClasificacion.CssClass = "boton_pestana";
                    //Asignando foco predeterminado
                    gvParadas.Focus();
                    break;
                case "vwClasificacion":
                    btnVistaEcabezadoServicio.CssClass = "boton_pestana";
                    btnVistaParadasServicio.CssClass = "boton_pestana";
                    btnVistaResumenServicio.CssClass = "boton_pestana";
                    btnVistaClasificacion.CssClass = "boton_pestana_activo";

                    //Asignando foco predeterminado
                    wucClasificacion.Focus();
                    break;
            }           
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["_id_servicio"] = this._id_servicio;
            ViewState["_id_parada"] = this._id_parada;
            ViewState["_hab_edicion"] = this._hab_edicion;
            ViewState["_vista_inicial"] = this._vista_inicial;
            ViewState["_nueva_documentacion"] = this._nueva_documentacion;
            ViewState["_mit_parada"] = this._mit_parada;
            ViewState["_mit_evento"] = this._mit_evento;
            ViewState["_mit_producto"] = this._mit_producto;
            ViewState["_mit_resumen"] = this._mit_resumen;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["_id_servicio"] != null && ViewState["_nueva_documentacion"] != null &&
                ViewState["_hab_edicion"] != null && ViewState["_vista_inicial"] != null)
            {
                this._id_servicio = Convert.ToInt32(ViewState["_id_servicio"]);
                this._id_parada = Convert.ToInt32(ViewState["_id_parada"]);
                this._nueva_documentacion = Convert.ToBoolean(ViewState["_nueva_documentacion"]);
                this._hab_edicion = Convert.ToBoolean(ViewState["_hab_edicion"]);
                this._vista_inicial = (VistaDocumentacion)ViewState["_vista_inicial"];

                if (ViewState["_mit_parada"] != null)
                    this._mit_parada = (DataTable)ViewState["_mit_parada"];
                if (ViewState["_mit_evento"] != null)
                    this._mit_evento = (DataTable)ViewState["_mit_evento"];
                if (ViewState["_mit_producto"] != null)
                    this._mit_producto = (DataTable)ViewState["_mit_producto"];
                if (ViewState["_mit_resumen"] != null)
                    this._mit_resumen = (DataTable)ViewState["_mit_resumen"];
            }
        }
        /// <summary>
        /// Inicializa el contenido del control
        /// </summary>
        private void inicializaControl()
        {
            //Creando esquema de tablas temporales
            creaEsquemaTablasTemporales();
            //Cargando catálogos requeridos
            cargaCatalogo();                       

            //Si se desea capturar un nuevo servicio
            if (this._id_servicio == 0)
                //Configurando contenido de control para captura inicial
                configuraNuevoServicio();
            //Si ya es un servicio existente
            else
                //Configurando consulta
                configuraConsultaServicio();

            //Habilitando edición en control
            habilitaTabsControl();

            //Asignando estilo a tabs
            asignaEstiloTabs();

            //Actualziando contenido de pestaña
            inicializaResumenServicio();
        }
        /// <summary>
        /// Carga el conjunto de catálogos requeridos en el control
        /// </summary>
        private void cargaCatalogo()
        {
            //Tipo de Parada
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoParadaDocumentacion, "", 19);
            //Tipo de Evento
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEventoParada, 40, "", Convert.ToInt32(ddlTipoParadaDocumentacion.SelectedValue), "", 0, "");
            //Unidad de Medida (Cantidad)
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadCantidad, 2, "Otro", 5, "", 0, "");
            //Unidad de Medida (Peso)
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadPeso, 2, "Otros", 2, "", 4, "");

            //Tamaño Resumen Paradas
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoParadas, "", 56);
        }
        /// <summary>
        /// Configura la captura de un nuevo servicio sobre los elementos del control de usuario
        /// </summary>
        private void configuraNuevoServicio()
        {
            //Titulo de Control
            h2EncabezadoServicioDocumentacion.InnerText = "Nueva Documentación de Servicio";

            //limpiando controles de encabezado
            txtClienteServicio.Text =
            txtReferencia.Text =
            txtObservacion.Text =
            txtCartaPorte.Text = "";

            //Ocultando botón Guardar encabezado
            btnAceptarEncabezadoDocumentacion.Visible = false;

            //Inicializando control de temperaturas
            wucTemperaturaServicio.InicializaTemperaturasServicio();

            //Inicializando control de clasificación
            wucClasificacion.InicializaControl(1, 0, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

            //Inicializando Contenido de GridView
            Controles.InicializaGridview(gvParadasDocumentacion);
            Controles.InicializaGridview(gvProductoEvento);

            //Inicializando controles de captura de parada
            inicializaControlesParada();
            //Inicializando controles de captura de producto
            inicializaControlesProducto();
        }
        /// <summary>
        /// Construye el esquema de tablas temporales
        /// </summary>
        private void creaEsquemaTablasTemporales()
        {
            //Paradas
            this._mit_parada = new DataTable();
            this._mit_parada.Columns.Add("Id", typeof(Int32));
            this._mit_parada.Columns.Add("Secuencia", typeof(Decimal));
            this._mit_parada.Columns.Add("IdTipoParada", typeof(Byte));
            this._mit_parada.Columns.Add("TipoParada", typeof(String));
            this._mit_parada.Columns.Add("IdUbicacion", typeof(Int32));
            this._mit_parada.Columns.Add("Ubicacion", typeof(String));
            this._mit_parada.Columns.Add("Cita", typeof(DateTime));

            //Eventos
            this._mit_evento = new DataTable();
            this._mit_evento.Columns.Add("Id", typeof(Int32));
            this._mit_evento.Columns.Add("IdParada", typeof(Int32));
            this._mit_evento.Columns.Add("Secuencia", typeof(Decimal));
            this._mit_evento.Columns.Add("IdTipoEvento", typeof(Int32));

            //Producto
            this._mit_producto = new DataTable();
            this._mit_producto.Columns.Add("Id", typeof(Int32));
            this._mit_producto.Columns.Add("SecuenciaEvento", typeof(Decimal));
            this._mit_producto.Columns.Add("SecuenciaProducto", typeof(Decimal));
            this._mit_producto.Columns.Add("IdParada", typeof(Int32));
            this._mit_producto.Columns.Add("IdTipoEvento", typeof(Byte));
            this._mit_producto.Columns.Add("TipoEvento", typeof(String));
            this._mit_producto.Columns.Add("IdProducto", typeof(Int32));
            this._mit_producto.Columns.Add("Producto", typeof(String));
            this._mit_producto.Columns.Add("Cantidad", typeof(Decimal));
            this._mit_producto.Columns.Add("IdUnidadCantidad", typeof(Byte));
            this._mit_producto.Columns.Add("UnidadCantidad", typeof(String));
            this._mit_producto.Columns.Add("Peso", typeof(Decimal));
            this._mit_producto.Columns.Add("IdUnidadPeso", typeof(Byte));
            this._mit_producto.Columns.Add("UnidadPeso", typeof(String));
        }
        /// <summary>
        /// Configura la edición y/o consulta del servicio sobre los elementos del control de usuario
        /// </summary>
        private void configuraConsultaServicio()
        {            
            //Instanciando servicio
            using (Servicio servicio = new Servicio(this._id_servicio))
            {
                //Titulo de Control
                h2EncabezadoServicioDocumentacion.InnerText = string.Format("Documentación de Servicio '{0}'", servicio.no_servicio);

                //Cargando información en controles de encabezado
                using (SAT_CL.Global.CompaniaEmisorReceptor cliente = new SAT_CL.Global.CompaniaEmisorReceptor(servicio.id_cliente_receptor))
                    txtClienteServicio.Text = string.Format("{0} ID:{1}", cliente.nombre, cliente.id_compania_emisor_receptor);
                txtReferencia.Text = servicio.referencia_cliente;
                txtObservacion.Text = servicio.observacion_servicio;
                txtCartaPorte.Text = servicio.porte;

                //Mostrando botón Guardar encabezado
                btnAceptarEncabezadoDocumentacion.Visible = true;

                //Inicializando control de temperaturas
                wucTemperaturaServicio.InicializaTemperaturasServicio(this._id_servicio);

                //Inicializando control de clasificación
                wucClasificacion.InicializaControl(1, this._id_servicio, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                //Cargando paradas 
                cargaParadas();
                //Inicializando contenido de Gridview de productos
                Controles.InicializaGridview(gvProductoEvento);

                //Inicializando controles de captura de parada
                inicializaControlesParada();
                //Inicializando controles de captura de producto
                inicializaControlesProducto();
            }
        }
        /// <summary>
        /// Inicializa los controles de captura de parada
        /// </summary>
        private void inicializaControlesParada()
        {
            txtSecuenciaParada.Text = obtineSecuenciaParada().ToString();
            ddlTipoParadaDocumentacion.SelectedValue = "1";
            txtUbicacionParada.Text = "";

            //Definiendo valor predeterminado de fecha de cita
            DateTime fecha_cita = Fecha.ObtieneFechaEstandarMexicoCentro();

            //Si existen paradas registradas en la tabla correspondiente
            if (this._mit_parada.Rows.Count > 0)
            { 
                //Obteniendo cita estimada
                fecha_cita = (from DataRow r in this._mit_parada.Rows
                              orderby Convert.ToDecimal(r["Secuencia"]) descending
                              select Convert.ToDateTime(r["Cita"])).FirstOrDefault().AddHours(3);
            }

            //Asignando fecha correspondiente de cita
            txtCitaParada.Text = fecha_cita.ToString("dd/MM/yyyy HH:mm");

            //Indicando el comando de guardado
            imbAgregarParada.CommandArgument = "Nuevo";

            //Habilitando controles
            habilitaControlesParada();
        }
        /// <summary>
        /// Inicializa los controles de parada para su edición
        /// </summary>
        private void inicializaControlesParadaEdicion()
        {
            //Obteniendo identificadores de parada
            int id_parada = Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"]);
            decimal secuencia = Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Secuencia"]);

            //Si es temporal
            if (id_parada == 0)
            {
                //Localizando producto solicitado
                DataRow r = (from DataRow p in this._mit_parada.Rows
                             where Convert.ToDecimal(p["Secuencia"]) == secuencia
                             select p).FirstOrDefault();

                //Asignando valores sobre controles de edición
                txtSecuenciaParada.Text = Convert.ToInt32(r["Secuencia"]).ToString();
                ddlTipoParadaDocumentacion.SelectedValue = r["IdTipoParada"].ToString();
                txtUbicacionParada.Text = string.Format("{0} ID:{1}", r["Ubicacion"], r["IdUbicacion"]);
                txtCitaParada.Text = string.Format("{0:dd/MM/yyyy HH:mm}", r["Cita"]);
            }
            //Si es de bd
            else
            {
                //Instanciando registro parada
                using (Parada parada = new Parada(id_parada))
                {
                    //Instanciando ubicacion
                    using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(parada.id_ubicacion))
                    {
                        //Asignando valores sobre controles de edición
                        txtSecuenciaParada.Text = Convert.ToInt32(parada.secuencia_parada_servicio).ToString();
                        ddlTipoParadaDocumentacion.SelectedValue = parada.id_tipo_parada.ToString();
                        txtUbicacionParada.Text = string.Format("{0} ID:{1}", u.descripcion, u.id_ubicacion);
                        txtCitaParada.Text = parada.cita_parada.ToString("dd/MM/yyyy HH:mm");
                    }
                }
            }
            //Indicando el comando de guardado
            imbAgregarParada.CommandArgument = "Edicion";

            //Habilitando controles
            habilitaControlesParada();
        }
        /// <summary>
        /// Inicializa los controles de captura de producto y evento
        /// </summary>
        private void inicializaControlesProducto()
        {
            //Si existe una parada seleccionada
            if (gvParadasDocumentacion.SelectedIndex != -1)
                //Cargando catálogo Tipo de Evento
                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEventoParada, 40, "", Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["IdTipoParada"]), "", 0, "");
            else
                //Cargando catálogo Tipo de Evento
                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEventoParada, 40, "", Convert.ToInt32(ddlTipoParadaDocumentacion.SelectedValue), "", 0, "");          
            
            //DONE: No se asigna evento predeterminado, ya que está en función del tipo de parada
            //ddlTipoEventoParada.SelectedValue = "1";

            txtProductoEventoParada.Text = "";
            txtCantidadProducto.Text = "";
            ddlUnidadCantidad.SelectedValue = "23";
            txtPesoProducto.Text = "";
            ddlUnidadPeso.SelectedValue = "18";
            imbAgregarProducto.CommandArgument = "Nuevo";

            //Habilitando controles
            habilitaControlesProducto();

            visualizaProductoTipoEvento();
        }
        /// <summary>
        /// Coloca los valores del registro producto a editar sobre los controles correspondientes
        /// </summary>
        private void inicializaControlesProductoEdicion()
        {
            //Determinando el tipo de carga a realizar (temporal o Bd)
            int id_producto = Convert.ToInt32(gvProductoEvento.SelectedDataKey["Id"]);
            decimal secuencia_producto = Convert.ToInt32(gvProductoEvento.SelectedDataKey["SecuenciaProducto"]);

            //Si es temporal
            if (id_producto == 0)
            {
                //Localizando producto solicitado
                DataRow r = (from DataRow p in this._mit_producto.Rows
                             where Convert.ToDecimal(p["SecuenciaProducto"]) == secuencia_producto
                             select p).FirstOrDefault();

                //Asignando valores sobre controles de edición
                ddlTipoEventoParada.SelectedValue = r["IdTipoEvento"].ToString();
                //Determinando el tipo de evento (carga 1, descarga 2/3), carga se obtiene Id de Producto de textbox, descargas de Dropdown 
                if (ddlTipoEventoParada.SelectedValue == "1")
                    txtProductoEventoParada.Text = string.Format("{0} ID:{1}", r["Producto"], r["IdProducto"]);
                else
                {
                    //TODO: cargando catálogo con productos correspondientes
                    //Asignando valor default
                }                
                txtCantidadProducto.Text = r["Cantidad"].ToString();
                ddlUnidadCantidad.SelectedValue = r["IdUnidadCantidad"].ToString();
                txtPesoProducto.Text = r["Peso"].ToString();
                ddlUnidadPeso.SelectedValue = r["IdUnidadPeso"].ToString();
            }
            //Si es de bd
            else
            {
                //Instanciando registro producto servicio
                using (ServicioProducto producto = new ServicioProducto(id_producto))
                {
                    //Instanciando producto
                    using (SAT_CL.Global.Producto p = new SAT_CL.Global.Producto(producto.id_producto))
                    {
                        //Asignando valores de registro sobre controles
                        ddlTipoEventoParada.SelectedValue = producto.id_tipo.ToString();

                        //Determinando el tipo de evento (carga 1, descarga 2/3), carga se obtiene Id de Producto de textbox, descargas de Dropdown 
                        if (ddlTipoEventoParada.SelectedValue == "1")
                            txtProductoEventoParada.Text = string.Format("{0} ID:{1}", p.descripcion, p.id_producto);
                        else
                        {
                            //Para evitar error de validación
                            txtProductoEventoParada.Text = string.Format("{0} ID:{1}", p.descripcion, p.id_producto);

                            //Productos cargados
                            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProductoEventoParada, 67, "-- Seleccione un Producto --", this._id_servicio, "", Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Secuencia"]), "");
                            //Asignando valor default
                            ddlProductoEventoParada.SelectedValue = p.id_producto.ToString();
                        }
                    }

                    txtCantidadProducto.Text = producto.cantidad.ToString();
                    ddlUnidadCantidad.SelectedValue = producto.id_unidad.ToString();
                    txtPesoProducto.Text = producto.peso.ToString();
                    ddlUnidadPeso.SelectedValue = producto.id_unidad_peso.ToString();
                }
            }

            //Definiendo comando de guardado
            imbAgregarProducto.CommandArgument = "Edicion";

            //Habilitando controles
            habilitaControlesProducto();
            visualizaProductoTipoEvento();
        }
        /// <summary>
        /// Realiza la carga del catálogo de productos con los productos existentes en la tabla temporal correspondiente
        /// </summary>
        private void cargaCatalogoProductosTemporal()
        {
            Controles.CargaDropDownList(ddlProductoEventoParada, this._mit_producto, "IdProducto", "Producto");
        }
        /// <summary>
        /// Obtiene la secuencia de la parada por insertar
        /// </summary>
        /// <returns></returns>
        private int obtineSecuenciaParada()
        {
            //Declarando variable de conteo de registros parada
            int secuencia = 1;

            //Si hay registros parada
            if (Validacion.ValidaOrigenDatos(this._mit_parada))
                //Obteneindo secuencia
                secuencia = (from DataRow r in this._mit_parada.Rows
                             select Convert.ToInt32(r["Secuencia"])).Max() + 1;

            //Devolviendo resultado
            return secuencia;
        }
        /// <summary>
        /// Realiza la carga de las paradas almacenadas temporalmente o desde BD
        /// </summary>
        private void cargaParadas()
        {
            //Inicializando indices de selección
            Controles.InicializaIndices(gvParadasDocumentacion);
            //Limpiando selección de parada
            this._id_parada = 0;

            //Si ya existe un Id de Servicio en el control
            if (this._id_servicio > 0)
            {
                //Actualizando origen de datos con registros de BD
                using (DataTable mit = Parada.CargaParadasServicioVisualziacionControlDocumentacion(this._id_servicio))
                {
                    //Si hay registros
                    if (mit != null)
                        //Añadiendo resultados de consulta
                        this._mit_parada = mit;
                    else
                        this._mit_parada.Clear();
                }

                //Limpiando el resto del esquema temporal
                this._mit_evento.Clear();
                this._mit_producto.Clear();
            }

            //Cargando Control GridView correspondiente
            Controles.CargaGridView(gvParadasDocumentacion, this._mit_parada, "Id-Secuencia-IdTipoParada", "", true, 2);
        }
        /// <summary>
        /// Guarda una nueva parada asociada al servicio en esquema de tablas temporales
        /// </summary>
        private RetornoOperacion guardaParadaTemporalServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            try
            {
                //creando nueva fila con estructura de tabla destino
                DataRow nr = this._mit_parada.NewRow();
                //Asignando valores de campos
                nr.SetField("Id", 0);
                nr.SetField("Secuencia", 1);
                nr.SetField("IdTipoParada", Convert.ToByte(ddlTipoParadaDocumentacion.SelectedValue));
                nr.SetField("TipoParada", ddlTipoParadaDocumentacion.SelectedItem.Text);
                nr.SetField("IdUbicacion", Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionParada.Text, "ID:", 1)));
                nr.SetField("Ubicacion", Cadena.RegresaCadenaSeparada(txtUbicacionParada.Text, "ID:", 0));
                nr.SetField("Cita", txtCitaParada.Text.Trim() != "" ? Convert.ToDateTime(txtCitaParada.Text) : DateTime.MinValue);
                //Añadiendo la parada solicitada
                this._mit_parada.Rows.Add(nr);
            }
            catch (Exception ex) { resultado = new RetornoOperacion(ex.Message); }

            //Guardando evento y producto predeterminado
            insertaProductoTemporalPredeterminado();

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Edita una nueva parada asociada al servicio en esquema de tablas temporales
        /// </summary>
        private RetornoOperacion editaParadaTemporalServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Recuperando secuencia de fila seleccionada
            decimal secuencia = Convert.ToDecimal(gvParadasDocumentacion.SelectedDataKey["Secuencia"]);

            //Obteniendo fila de datos
            DataRow parada = (from DataRow r in this._mit_parada.Rows
                              where Convert.ToDecimal(r["Secuencia"]) == secuencia
                              select r).FirstOrDefault();

            //Si la fila fue recuperada
            if(parada != null)
            {
                //Actualizando información
                parada.SetField("IdUbicacion", Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionParada.Text, "ID:", 1)));
                parada.SetField("Ubicacion", Cadena.RegresaCadenaSeparada(txtUbicacionParada.Text, "ID:", 0));
                parada.SetField("Cita", txtCitaParada.Text.Trim() != "" ? Convert.ToDateTime(txtCitaParada.Text) : DateTime.MinValue);
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el guardado de una nueva parada en BD, ligada al servicio actual
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion guardaParadaBDServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Si no existe un servicio activo
            if (this._id_servicio == 0)
                //Insertando nuevo servicio
                resultado = insertaServicio();
            //Si ya existe un servicio
            else
            {
                //Determinando secuencia en base a existencia de paradas
                decimal secuencia = Convert.ToDecimal(txtSecuenciaParada.Text);
                //Si la secuancia mínima y máxima permiten el consecutivo
                decimal sec1 = (from DataRow r in this._mit_parada.Rows
                                orderby Convert.ToDecimal(r["Secuencia"]) ascending
                                select Convert.ToDecimal(r["Secuencia"])).FirstOrDefault();
                decimal sec2 = (from DataRow r in this._mit_parada.Rows
                                orderby Convert.ToDecimal(r["Secuencia"]) descending
                                select Convert.ToDecimal(r["Secuencia"])).FirstOrDefault();
                //Si la secuencia nueva es menor a la primer parada
                if (secuencia < sec1)
                    secuencia = 1;
                else if (secuencia > sec2)
                    secuencia = sec2 + 1;

                //Insertando parada correspondiente
                resultado = Parada.NuevaParadaDocumentacion(this._id_servicio, secuencia, (Parada.TipoParada)Convert.ToByte(ddlTipoParadaDocumentacion.SelectedValue),
                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionParada.Text, "ID:", 1)), Microsoft.SqlServer.Types.SqlGeography.Null,
                    txtCitaParada.Text.Trim() != "" ? Convert.ToDateTime(txtCitaParada.Text) : DateTime.MinValue, 0, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                    this._mit_parada.Rows.Count, ((Usuario)Session["usuario"]).id_usuario);
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de una parada en BD
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion editaParadaBDServicio()
        {
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_parada);

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando parada seleccionada, parada siguiente y parada anterior
                using (Parada p = new Parada(Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"])),
                    pAnterior = new Parada(Parada.BuscaParadaAnteriorOperativa(this._id_servicio, p.secuencia_parada_servicio)),
                    pSiguiente = new Parada(Parada.BuscaParadaPosteriorOperativa(this._id_servicio, p.secuencia_parada_servicio)))
                {
                    //Si la parada se encuentra aún registrada
                    if (p.Estatus == Parada.EstatusParada.Registrado)
                    {
                        //Obteniendo fecha por establecer
                        DateTime fecha = txtCitaParada.Text.Trim() != "" ? Convert.ToDateTime(txtCitaParada.Text) : DateTime.MinValue;

                        //Si es la primer parada
                        if (p.secuencia_parada_servicio == 1)
                        {
                            //Si la siguiente parada posee fecha de cita menor o igual a la solicitada
                            if (pSiguiente.cita_parada.CompareTo(fecha) <= 0)
                                resultado = new RetornoOperacion(string.Format("La fecha por actualizar debe ser menor a '{0:dd/MM/yyyy HH:mm}'", pSiguiente.cita_parada));
                            //De lo contrario se realiza la actualización de la cita en el encabezado de servicio
                            else
                            {
                                //Instanciando servicio
                                using (SAT_CL.Documentacion.Servicio srv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                                    resultado = srv.ActualizaCitaCarga(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                if (!resultado.OperacionExitosa)
                                    resultado = new RetornoOperacion(string.Format("Error al actualizar cita de carga en encabezado de servicio: {0}", resultado.Mensaje));
                            }
                        }
                        //Si es la última
                        else if (p.secuencia_parada_servicio == new Parada(Parada.ObtieneUltimaParada(this._id_servicio)).secuencia_parada_servicio)
                        {
                            //Si la parada anterior posee fecha de cita mayor o igual a la solicitada
                            if (pAnterior.cita_parada.CompareTo(fecha) >= 0)
                                resultado = new RetornoOperacion(string.Format("La fecha por actualizar debe ser mayor a '{0:dd/MM/yyyy HH:mm}'", pAnterior.cita_parada));
                            //De lo contrario se actualiza la fecha de descarga en el encabezado de servicio
                            else
                            {
                                using (SAT_CL.Documentacion.Servicio srv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                                    resultado = srv.ActualizaCitaDescarga(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                if (!resultado.OperacionExitosa)
                                    resultado = new RetornoOperacion(string.Format("Error al actualizar cita de descarga en encabezado de servicio: {0}", resultado.Mensaje));
                            }
                        }
                        //Si es una intermedia
                        else
                        {
                            //Si la cita de parada apor actualizar no se encuentra entre la cita de la parada anterior y siguiente
                            if (pAnterior.cita_parada.CompareTo(fecha) >= 0 || pSiguiente.cita_parada.CompareTo(fecha) <= 0)
                                resultado = new RetornoOperacion(string.Format("La fecha por actualizar debe ser mayor a '{0:dd/MM/yyyy HH:mm}' y menor que '{1:dd/MM/yyyy HH:mm}'.", pAnterior.cita_parada, pSiguiente.cita_parada));
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                            //Actualizando parada
                            resultado = p.ActualizaCitaLlegada(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                            //Confirmando transacción
                            scope.Complete();
                    }
                    else
                        resultado = new RetornoOperacion(string.Format("No es posible editar la parada, su estatus actual es '{0}'.", p.Estatus));
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el borrado de la parada solicitada y sus dependencias temporales
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion eliminaParadaTemporal()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Borrando tablas temporales
            creaEsquemaTablasTemporales();

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el guardado del servicio temporal hacia un esquema de BD
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Recuperando datos de primier parada
            DataRow p1 = this._mit_parada.Rows[0];

            //Inicializando bloque transaccional
            using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Insertando nuevo encabezado de servicio
                resultado = Servicio.InsertarServicio("", 1, 0, false, 0, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtClienteServicio.Text, "ID:", 1)), p1.Field<int>("IdUbicacion"), p1.Field<DateTime>("Cita"),
                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionParada.Text, "ID:", 1)), txtCitaParada.Text.Trim() != "" ? Convert.ToDateTime(txtCitaParada.Text) : DateTime.MinValue,
                                    txtCartaPorte.Text.ToUpper(), txtReferencia.Text.ToUpper(), Fecha.ObtieneFechaEstandarMexicoCentro(), txtObservacion.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);

                //Validando Operación Exitosa
                if (resultado.OperacionExitosa)
                {
                    //Conservando Id de Servicio nuevo
                    this._id_servicio = resultado.IdRegistro;

                    //Validando Operación Exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciamos Servicio
                        using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                        {
                            //Actualizamos Referencia de Viaje
                            resultado = objServicio.ActualizacionReferenciaViaje(txtReferencia.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }
                
                //Si no hay error al registrar nuevo servicio
                if (resultado.OperacionExitosa)
                {
                    //Inicializando conjunto de paradas de servicio con eventos predeterminados de carga y descarga parcial
                    resultado = Parada.InsertaServicioDocumentacion(this._id_servicio, p1.Field<int>("IdUbicacion"), p1.Field<DateTime>("Cita"),
                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionParada.Text, "ID:", 1)), txtCitaParada.Text.Trim() != "" ? Convert.ToDateTime(txtCitaParada.Text) : DateTime.MinValue, 0,
                                    ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((Usuario)Session["usuario"]).id_usuario);

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Realizando inserción de Eventos y Productos de Parada 1
                        resultado = insertaEventosYProductosParadaTemporalBD();
                    //Si hay error de creación de primer segmento (dos paradas)
                    else
                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al crear primer segmento de servicio: {0}", resultado.Mensaje), resultado.OperacionExitosa);
                }
                //Error al crear encabezado de servicio
                else
                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al registrar encabezado de servicio: {0}", resultado.Mensaje), resultado.OperacionExitosa);

                //Si no hay errores encontrados (paradas, eventos y productos)
                if (resultado.OperacionExitosa)
                {
                    //Creando clasificación
                    resultado = SAT_CL.Global.Clasificacion.InsertaClasificacion(1, this._id_servicio, ((Usuario)Session["usuario"]).id_usuario);                 

                    //Personalizando resultado en caso de error
                    if (!resultado.OperacionExitosa)
                        resultado = new RetornoOperacion(resultado.IdRegistro, "Error al crear Clasificación predeterminada.", resultado.OperacionExitosa);
                }

                //Si no hay errores encontrados (paradas, eventos, productos y clasificación )
                if (resultado.OperacionExitosa)
                {
                    //Realizando inserción de encabezado de factura
                    resultado = SAT_CL.Facturacion.Facturado.InsertaFactura(this._id_servicio, ((Usuario)Session["usuario"]).id_usuario);

                    //Si hay errores
                    if (!resultado.OperacionExitosa)
                        //Personalizando error
                        resultado = new RetornoOperacion(resultado.IdRegistro, "Error al crear Encabezado de Factura.", resultado.OperacionExitosa);
                }

                //Si no hay errores operativos
                if (resultado.OperacionExitosa)
                    //Guardando referencias
                    resultado = wucTemperaturaServicio.GuardaTemperaturas(this._id_servicio);

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Instanciando parada con secuencia 2 del servicio creado
                    using (Parada p2 = new Parada(2, this._id_servicio))
                        //Crenado resultado con Id de ultima parada
                        resultado = new RetornoOperacion(p2.id_parada);

                    using (Servicio servicio = new Servicio(this._id_servicio))
                        //Titulo de Control
                        h2EncabezadoServicioDocumentacion.InnerText = string.Format("Documentación de Servicio '{0}'", servicio.no_servicio);

                    //Mostrando botón de guardado general
                    btnAceptarEncabezadoDocumentacion.Visible = true;

                    //Señalando nueva documentación realizada
                    this._nueva_documentacion = true;

                    //Completando transacción
                    transaccion.Complete();
                }
                //Si hubo errores
                else
                    //Indicando que no hay servicio insertado
                    this._id_servicio = 0;
            }

            //Inicializando control de clasificación
            wucClasificacion.InicializaControl(1, this._id_servicio, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

            //Habilitando pestaña de resumen
            btnVistaResumenServicio.Enabled = this._id_servicio > 0 ? true : false;

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la inserción de los eventos y productos de la parada temporal hacia bd
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaEventosYProductosParadaTemporalBD()
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Instanciando Parada con secuencia 1 del servicio actual
            using (Parada p1 = new Parada(1, this._id_servicio))
            {
                //Parada cada uno de los eventos existentes
                foreach (DataRow evento in this._mit_evento.Rows)
                {
                    //Validando existencia del tipo de evento requerido
                    using (DataTable mitEventosParada = ParadaEvento.CargaEventos(p1.id_parada))
                    {
                        //Indicador de existencia de evento
                        bool existe_evento = false;
                        //Si hay elementos retornados
                        if (mitEventosParada != null)
                        {
                            //Buscando Tipo de Evento, Si no existe
                            if ((from DataRow r in mitEventosParada.Rows
                                 where Convert.ToInt32(r["IdTipoEvento"]) == Convert.ToInt32(evento["IdTipoEvento"])
                                 select Convert.ToInt32(r["Id"])).FirstOrDefault() > 0)
                            {
                                //Señalando existencia de evento
                                existe_evento = true;
                            }
                        }

                        //Si no existe el evento solicitado
                        if (!existe_evento)
                        {
                            //Realizando la inserción hacia BD
                            resultado = ParadaEvento.InsertaParadaEvento(this._id_servicio, p1.id_parada, 0,
                                                    Convert.ToInt32(evento["IdTipoEvento"]), ((Usuario)Session["usuario"]).id_usuario);
                        }
                    }

                    //Si hay errores
                    if (!resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion(string.Format("Error al registrar Evento '{0}': {1}", evento["TipoEvento"].ToString(), resultado.Mensaje));
                        //terminando iteraciones
                        break;
                    }
                }

                //Si no hay errores en inserción de eventos
                if (resultado.OperacionExitosa)
                {
                    //Parada cada uno de los productos existentes
                    foreach (DataRow producto in this._mit_producto.Rows)
                    {
                        //Realizando la inserción hacia BD
                        resultado = SAT_CL.Documentacion.ServicioProducto.InsertaServicioProducto(this._id_servicio, p1.id_parada,
                                                    Convert.ToByte(producto["IdTipoEvento"]), Convert.ToInt32(producto["IdProducto"]),
                                                    Convert.ToDecimal(producto["Cantidad"]), Convert.ToInt32(producto["IdUnidadCantidad"]),
                                                    Convert.ToDecimal(producto["Peso"]), Convert.ToInt32(producto["IdUnidadPeso"]),
                                                    ((Usuario)Session["usuario"]).id_usuario);

                        //Si hay errores
                        if (!resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion(string.Format("Error al registrar Producto '{0}': {1}", producto["Producto"].ToString(), resultado.Mensaje));
                            //terminando iteraciones
                            break;
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la carga de los productos y eventos almacenadas temporalmente o desde BD
        /// </summary>
        private void cargaProductos()
        {
            //Inicializando indices de selección de producto
            Controles.InicializaIndices(gvProductoEvento);

            //Si ya existe un Id de Servicio en el control
            if (this._id_servicio > 0)
            {
                //Actualizando origen de datos con registros de BD
                using (DataSet ds = ServicioProducto.CargaEventosYProductosParadaVisualizacionControlDocumentacion(Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"])))
                {
                    //Validando existencia de elementos en tablas requeridas
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                        this._mit_evento = ds.Tables["Table"].Copy();
                    else
                        this._mit_evento.Clear();

                    if (Validacion.ValidaOrigenDatos(ds, "Table1"))
                        this._mit_producto = ds.Tables["Table1"].Copy();
                    else
                        this._mit_producto.Clear();
                }
            }

            //Cargando Control GridView correspondiente
            Controles.CargaGridView(gvProductoEvento, this._mit_producto, "Id-SecuenciaProducto-SecuenciaEvento", "", true, 1);
        }
        /// <summary>
        /// Realiza el salvado del nuevo evento y/o producto asociado a la parada inicial en el esquema temporal
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaProductoTemporalPredeterminado()
        {
            //Indicando resultado sin Id, para efecto de devolución re resultado temporal
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Considerando que se acaba de insertar la primer parada en esquema temporal, se inserta el primer evento temporal (Carga)
            decimal secuencia_evento = 1;

            try
            {
                //Creando nueva fila de tabla de eventos
                DataRow nr = this._mit_evento.NewRow();
                //Añadiendo atributos de evento
                nr.SetField("Id", 0);
                nr.SetField("IdParada", 0);
                nr.SetField("Secuencia", secuencia_evento);
                nr.SetField("IdTipoEvento", 1);
                //Insertando evento en tabla temporal
                this._mit_evento.Rows.Add(nr);
            }
            catch (Exception ex) { resultado = new RetornoOperacion(string.Format("Error al insertar evento: {0}", ex.Message)); }
            
            //Si se insertó correctamente el evento
            if (resultado.OperacionExitosa)
            {
                //Indicando resultado sin Id, para efecto de devolución re resultado temporal
                resultado = new RetornoOperacion(0);
                //Añadiendo producto
                try
                {
                    //Declarando variable para almacenar secuencia de guardado temporal de producto
                    decimal secuencia_producto = 1;

                    //Nueva fila con estructura de tabla destino
                    DataRow nr = this._mit_producto.NewRow();
                    //Asignando valores de campos
                    nr.SetField("Id", 0);
                    nr.SetField("SecuenciaEvento", secuencia_evento);
                    nr.SetField("SecuenciaProducto", secuencia_producto);
                    nr.SetField("IdParada", 0);
                    //Evento Predeterminado
                    nr.SetField("IdTipoEvento", 1);
                    nr.SetField("TipoEvento", "Carga");
                    // Producto Predeterminado
                    nr.SetField("IdProducto", 1);
                    nr.SetField("Producto", "ABARROTES");
                    //Cantidad y Unidad de Medida Predeterminados (Tarimas)
                    nr.SetField("Cantidad", 1);
                    nr.SetField("IdUnidadCantidad", 23);
                    nr.SetField("UnidadCantidad", "Tarima(s)");
                    nr.SetField("Peso", 0);
                    nr.SetField("IdUnidadPeso", 18);
                    nr.SetField("UnidadPeso", "Tonelada(s)");
                    //Añadiendo la parada solicitada
                    this._mit_producto.Rows.Add(nr);
                }
                catch (Exception ex) { resultado = new RetornoOperacion(string.Format("Error al insertar producto predeterminado: {0}", ex.Message)); }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el salvado del nuevo evento y/o producto asociado a la parada en el esquema temporal
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaProductoTemporal()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declarando variable para almacenar secuencia de guardado temporal de evento
            decimal secuencia_evento = (from DataRow r in this._mit_evento.Rows
                                        where Convert.ToInt32(r["IdTipoEvento"]) == Convert.ToInt32(ddlTipoEventoParada.SelectedValue)
                                        select Convert.ToDecimal(r["Secuencia"])).DefaultIfEmpty(0).FirstOrDefault();
            //Si no existe aún el tabla temporal
            if (secuencia_evento == 0)
            {
                resultado = insertaEventoTemporal();
                secuencia_evento = (decimal)resultado.IdRegistro;
            }

            //Si se insertó correctamente el evento
            if (resultado.OperacionExitosa)
            {
                //Indicando resultado sin Id, para efecto de devolución re resultado temporal
                resultado = new RetornoOperacion(0);
                //Añadiendo producto
                try
                {
                    //Declarando variable para almacenar secuencia de guardado temporal de producto
                    decimal secuencia_producto = (from DataRow r in this._mit_producto.Rows
                                                  select Convert.ToDecimal(r["SecuenciaProducto"])).DefaultIfEmpty(0).Max() + 1;

                    //Nueva fila con estructura de tabla destino
                    DataRow nr = this._mit_producto.NewRow();
                    //Asignando valores de campos
                    nr.SetField("Id", 0);
                    nr.SetField("SecuenciaEvento", secuencia_evento);
                    nr.SetField("SecuenciaProducto", secuencia_producto);
                    nr.SetField("IdParada", 0);
                    nr.SetField("IdTipoEvento", Convert.ToByte(ddlTipoEventoParada.SelectedValue == "1" ? 1 : 2));
                    nr.SetField("TipoEvento", ddlTipoEventoParada.SelectedValue == "1" ? "Carga" : "Descarga");
                    //Determinando el tipo de evento (carga 1, descarga 2/3), carga se obtiene Id de Producto de textbox, descargas de Dropdown 
                    if (ddlTipoEventoParada.SelectedValue == "1")
                    {
                        nr.SetField("IdProducto", Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProductoEventoParada.Text, "ID:", 1)));
                        nr.SetField("Producto", Cadena.RegresaCadenaSeparada(txtProductoEventoParada.Text, "ID:", 0));
                    }
                    else
                    {
                        nr.SetField("IdProducto", Convert.ToInt32(ddlProductoEventoParada.SelectedValue));
                        nr.SetField("Producto", ddlProductoEventoParada.SelectedItem.Text);
                    }
                    nr.SetField("Cantidad", Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtCantidadProducto.Text, "0")));
                    nr.SetField("IdUnidadCantidad", Convert.ToByte(ddlUnidadCantidad.SelectedValue));
                    nr.SetField("UnidadCantidad", ddlUnidadCantidad.SelectedItem.Text);
                    nr.SetField("Peso", Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtPesoProducto.Text, "0")));
                    nr.SetField("IdUnidadPeso", Convert.ToByte(ddlUnidadPeso.SelectedValue));
                    nr.SetField("UnidadPeso", ddlUnidadPeso.SelectedItem.Text);
                    //Añadiendo la parada solicitada
                    this._mit_producto.Rows.Add(nr);
                }
                catch (Exception ex) { resultado = new RetornoOperacion(string.Format("Error al insertar producto: {0}", ex.Message)); }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el guardado en modo de edición del producto y/o evento solicitado sobre la tabla temporal
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion editaProductoTemporal()
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Editando producto
            try
            {
                //Obteniendo la fila en el origen de datos del producto seleccionado
                DataRow r = (from DataRow p in this._mit_producto.Rows
                             where Convert.ToDecimal(p["SecuenciaProducto"]) == Convert.ToInt32(gvProductoEvento.SelectedDataKey["SecuenciaProducto"])
                             select p).FirstOrDefault();

                //Asignando nuevos valores de campos editables
                //Determinando el tipo de evento (carga 1, descarga 2/3), carga se obtiene Id de Producto de textbox, descargas de Dropdown 
                if (ddlTipoEventoParada.SelectedValue == "1")
                {
                    r.SetField("IdProducto", Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProductoEventoParada.Text, "ID:", 1)));
                    r.SetField("Producto", Cadena.RegresaCadenaSeparada(txtProductoEventoParada.Text, "ID:", 0));
                }
                else
                {
                    r.SetField("IdProducto", Convert.ToInt32(ddlProductoEventoParada.SelectedValue));
                    r.SetField("Producto", ddlProductoEventoParada.SelectedItem.Text);
                }
                r.SetField("Cantidad", Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtCantidadProducto.Text, "0")));
                r.SetField("IdUnidadCantidad", Convert.ToByte(ddlUnidadCantidad.SelectedValue));
                r.SetField("Peso", Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtPesoProducto.Text, "0")));
                r.SetField("IdUnidadPeso", Convert.ToByte(ddlUnidadPeso.SelectedValue));
            }
            catch (Exception ex) { resultado = new RetornoOperacion(string.Format("Error al editar producto: {0}", ex.Message)); }


            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la eliminación del evento y/o producto solicitados
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion eliminaProductoTemporal()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Recuperando secuencias temporales de evento y producto
            decimal secuencia_producto = Convert.ToDecimal(gvProductoEvento.SelectedDataKey["SecuenciaProducto"]);
            decimal secuencia_evento = Convert.ToDecimal(gvProductoEvento.SelectedDataKey["SecuenciaEvento"]);

            //Determinando si ya no existen más productos con el mismo tipo de evento
            if ((from DataRow p in this._mit_producto.Rows
                 where Convert.ToDecimal(p["SecuenciaEvento"]) == secuencia_evento &&
                 Convert.ToDecimal(p["SecuenciaProducto"]) != secuencia_producto
                 select Convert.ToDecimal(p["SecuenciaProducto"])).DefaultIfEmpty(0).Count(p => p > 0) == 0)
            {
                //Eliminando evento temporal
                resultado = eliminaEventoTemporal(secuencia_evento);
            }


            try
            {
                //Localizando producto solicitado
                DataRow producto = (from DataRow p in this._mit_producto.Rows
                                    where Convert.ToDecimal(p["SecuenciaProducto"]) == secuencia_producto
                                    select p).FirstOrDefault();

                //Eliminando producto de la tabla
                this._mit_producto.Rows.Remove(producto);
            }
            catch (Exception ex)
            {
                resultado = new RetornoOperacion(string.Format("No fue posible remover el producto: {0}", ex.Message));
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Inserta un evento en la tabla temporal
        /// </summary>
        private RetornoOperacion insertaEventoTemporal()
        {
            //Obteniendo secuencia para nuevo evento
            decimal secuencia_evento = (from DataRow r in this._mit_evento.Rows
                                        select Convert.ToDecimal(r["Secuencia"])).DefaultIfEmpty(0).Max() + 1;

            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(secuencia_evento);

            try
            {
                //Creando nueva fila de tabla de eventos
                DataRow nr = this._mit_evento.NewRow();
                //Añadiendo atributos de evento
                nr.SetField("Id", 0);
                nr.SetField("IdParada", 0);
                nr.SetField("Secuencia", secuencia_evento);
                nr.SetField("IdTipoEvento", Convert.ToInt32(ddlTipoEventoParada.SelectedValue));
                //Insertando evento en tabla temporal
                this._mit_evento.Rows.Add(nr);
            }
            catch (Exception ex) { resultado = new RetornoOperacion(string.Format("Error al insertar evento: {0}", ex.Message)); }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el borrado del evento con la secuencia solicitada de la tabla temporal
        /// </summary>
        /// <param name="secuencia"></param>
        /// <returns></returns>
        private RetornoOperacion eliminaEventoTemporal(decimal secuencia)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            try
            {
                //Localizando evento solicitado
                DataRow ev = (from DataRow e in this._mit_evento.Rows
                              where Convert.ToDecimal(e["Secuencia"]) == secuencia
                              select e).FirstOrDefault();

                //Eliminando la fila del evento solicitado
                this._mit_evento.Rows.Remove(ev);
            }
            catch (Exception ex)
            {
                resultado = new RetornoOperacion(string.Format("No fue posible remover el evento: {0}", ex.Message));
            }

            //Recuperando eventos con secuencia posterior al elemento recien eliminado
            IEnumerable<DataRow> eventos = (from DataRow e in this._mit_evento.Rows
                                            where Convert.ToDecimal(e["Secuencia"]) > secuencia
                                            orderby Convert.ToDecimal(e["Secuencia"]) ascending
                                            select e).DefaultIfEmpty();

            try
            {
                //A cada evento se le restará en una unidad el valor de su secuencia actual y se actualizará dicha secuencia en la tabla de productos asociados
                foreach (DataRow r in eventos)
                {
                    //Si no hay valores nulos
                    if (r != null)
                    {
                        //Actualizando productos asociados
                        foreach (DataRow p in (from DataRow producto in this._mit_producto.Rows
                                               where Convert.ToDecimal(producto["SecuenciaEvento"]) == r.Field<decimal>("Secuencia")
                                               select producto).DefaultIfEmpty())
                        {
                            //Si hay registro que actualizar
                            if (p != null)
                            {
                                //Actualizando secuencia de evento asociado al producto
                                p.SetField("SecuenciaEvento", p.Field<decimal>("SecuenciaEvento") - 1);
                            }
                        }

                        //Actualizando secuencia de evento
                        r.SetField("Secuencia", r.Field<decimal>("Secuencia") - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = new RetornoOperacion(string.Format("No fue posible actualizar las secuencias de evento en los productos existentes: {0}", ex.Message));
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la inserción de un nuevo evento y/o producto hacia la BD
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaProductoBD()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //inicializando bloque transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Recuperando Id de Parada seleccioanda
                int id_parada = Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"]);

                //Determinando el tipo de parada (si es operativa)
                if (gvParadasDocumentacion.SelectedDataKey["IdTipoParada"].ToString() == "1")
                {
                    //Verificando si ya existe un evento del tipo solicitado
                    bool carga, descarga, parcial;
                    ParadaEvento.CargaTipoEventos(id_parada, out carga, out descarga, out parcial);

                    //Si el evento requerido no existe
                    if ((ddlTipoEventoParada.SelectedValue == "1" && !carga) ||
                        (ddlTipoEventoParada.SelectedValue == "2" && !descarga) ||
                        (ddlTipoEventoParada.SelectedValue == "3" && !parcial))
                    {
                        //Se inserta un nuevo servicio
                        resultado = ParadaEvento.InsertaParadaEventoEnDocumentacion(this._id_servicio, id_parada, Convert.ToInt32(ddlTipoEventoParada.SelectedValue),
                                                                                    this._mit_evento.Rows.Count, ((Usuario)Session["usuario"]).id_usuario);
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Se inserta el producto solicitado
                        resultado = ServicioProducto.InsertaServicioProducto(this._id_servicio, id_parada, Convert.ToByte(ddlTipoEventoParada.SelectedValue == "1" ? 1 : 2),
                            //Determinando el tipo de evento (carga 1, descarga 2/3), carga se obtiene Id de Producto de textbox, descargas de Dropdown 
                                                    ddlTipoEventoParada.SelectedValue == "1" ? Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProductoEventoParada.Text, "ID:", 1)) : Convert.ToInt32(ddlProductoEventoParada.SelectedValue),
                                                                            Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtCantidadProducto.Text, "0")), Convert.ToInt32(ddlUnidadCantidad.SelectedValue),
                                                                            Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtPesoProducto.Text, "0")), Convert.ToInt32(ddlUnidadPeso.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);
                    }
                }
                //Si es de servicio
                else
                {
                    //Si no existe el evento en esta parada
                    if (ParadaEvento.ObtieneTotalEventos(Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"]), Convert.ToInt32(ddlTipoEventoParada.SelectedValue)) == 0)
                    {
                        //Se inserta un nuevo evento de servicio
                        resultado = ParadaEvento.InsertaParadaEventoEnDocumentacion(this._id_servicio, id_parada, Convert.ToInt32(ddlTipoEventoParada.SelectedValue),
                                                                                    this._mit_evento.Rows.Count, ((Usuario)Session["usuario"]).id_usuario);
                        //Si no hay error
                        if (resultado.OperacionExitosa)
                            resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Evento '{0}': {1} (Puede ser visualizado desde la lista de eventos cuando la unidad se encuentre en dicha parada)", ddlTipoEventoParada.SelectedItem.Text, resultado.Mensaje), resultado.OperacionExitosa);
                        else
                            resultado = new RetornoOperacion(string.Format("Evento '{0}': {1}", ddlTipoEventoParada.SelectedItem.Text, resultado.Mensaje));
                    }
                    else
                        resultado = new RetornoOperacion(string.Format("El Evento '{0}' ya se ha registrado previamenmte para esta parada.", ddlTipoEventoParada.SelectedItem.Text));
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Se confirma transacción
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de un producto hacia la BD
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion editaProductoBD()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Si el producto se va a descargar
            if (ddlTipoEventoParada.SelectedValue != "1")
            {
                //Validando que exista selección de producto
                if (ddlProductoEventoParada.SelectedValue == "0")
                    resultado = new RetornoOperacion("Debe seleccionar un producto de la lista de productos cargados previamente.");
            }

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Instanciando producto en edición
                using (ServicioProducto producto = new ServicioProducto(Convert.ToInt32(gvProductoEvento.SelectedDataKey["Id"])))
                {
                    //Si el producto existe
                    if (producto.id_servicio_producto > 0)
                        //Realizando actualización
                        resultado = producto.EditaServicioProducto(producto.id_servicio, producto.id_parada, producto.id_tipo,
                            //Determinando el tipo de evento (carga 1, descarga 2/3), carga se obtiene Id de Producto de textbox, descargas de Dropdown 
                                                    ddlTipoEventoParada.SelectedValue == "1" ? Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProductoEventoParada.Text, "ID:", 1)) : Convert.ToInt32(ddlProductoEventoParada.SelectedValue),
                                                    Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtCantidadProducto.Text, "0")), Convert.ToInt32(ddlUnidadCantidad.SelectedValue),
                                                    Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtPesoProducto.Text, "0")), Convert.ToInt32(ddlUnidadPeso.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);
                    else
                        resultado = new RetornoOperacion("El producto ya no existe en la BD. Favor de actualizar la vista de edición.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la eliminación de un producto en la BD
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion eliminaProductoBD()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Instanciando producto en edición
            using (ServicioProducto producto = new ServicioProducto(Convert.ToInt32(gvProductoEvento.SelectedDataKey["Id"])))
            {
                //Si el producto existe
                if (producto.id_servicio_producto > 0)
                    //Realizando actualización
                    resultado = producto.DeshabilitaServicioProducto(((Usuario)Session["usuario"]).id_usuario);
                else
                    resultado = new RetornoOperacion("El producto ya no existe en la BD. Favor de actualizar la vista de edición.");
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la habilitación de los controles de captura de parada
        /// </summary>
        private void habilitaControlesParada()
        {
            //Determinando si existe o no servicio
            if (this._id_servicio == 0)
            {
                //Deshabilitando tipo de parada
                ddlTipoParadaDocumentacion.Enabled =
                    //Secuencia
                txtSecuenciaParada.Enabled = false;

                //Ubicación y cita
                txtUbicacionParada.Enabled =
                    txtCitaParada.Enabled = true;
            }
            //Si hay servicio
            else
            {
                //Determinando que tipo de acción se quiere realizar al guardar
                if (imbAgregarParada.CommandArgument == "Nuevo")
                {
                    //Habilitando tipo de parada
                    ddlTipoParadaDocumentacion.Enabled =
                        //Secuencia
                    txtSecuenciaParada.Enabled =

                    //Ubicación y cita
                    txtUbicacionParada.Enabled =
                    txtCitaParada.Enabled = true;
                }
                else if (imbAgregarParada.CommandArgument == "Edicion")
                {
                    //Habilitando tipo de parada
                    ddlTipoParadaDocumentacion.Enabled =
                        //Secuencia
                    txtSecuenciaParada.Enabled =

                    //Ubicación y cita
                    txtUbicacionParada.Enabled = false;
                    txtCitaParada.Enabled = true;
                }
            }
        }
        /// <summary>
        /// Realiza la habilitación de los controles de captura de producto
        /// </summary>
        private void habilitaControlesProducto()
        {
            //Determinando estado de captura (nuevo/edición)
            if (imbAgregarProducto.CommandArgument == "Nuevo")
            {
                //Habilitando tipo de evento
                ddlTipoEventoParada.Enabled = true;
            }
            //Si es edición
            else if (imbAgregarProducto.CommandArgument == "Edicion")
            {
                //Habilitando tipo de evento
                ddlTipoEventoParada.Enabled = false;
            }

            //Declarando auxiliar de habilitación de controles de producto
            bool hab_producto = true;

            //Si existe una parada seleccionada
            if (gvParadasDocumentacion.SelectedIndex != -1)
            {
                //Determinando el tipo de parada, si es operativa
                if (gvParadasDocumentacion.SelectedDataKey["IdTipoParada"].ToString() != "1")
                    hab_producto = false;
            }
            else
            {
                //Determinando el tipo de parada, si es operativa
                if (ddlTipoParadaDocumentacion.SelectedValue != "1")
                    hab_producto = false;
            }

            //Se aplica habilitación de controles de producto
            txtProductoEventoParada.Enabled =
                ddlProductoEventoParada.Enabled =
                txtCantidadProducto.Enabled =
                ddlUnidadCantidad.Enabled =
                txtPesoProducto.Enabled =
                ddlUnidadPeso.Enabled = hab_producto;
        }
        /// <summary>
        /// Habilita o deshabilita los tabs del control en base al atributo de edición 
        /// </summary>
        private void habilitaTabsControl()
        {
            //Si se permite la edición
            if (this._hab_edicion)
            {
                //Habilitando todos los Tabs
                btnVistaEcabezadoServicio.Enabled =
                btnVistaParadasServicio.Enabled = true;
                btnVistaResumenServicio.Enabled = this._id_servicio > 0 ? true : false;

                //Asignando Vista activa predeterminada
                mtvDocumentacion.ActiveViewIndex = (int)this._vista_inicial;
            }
            //Si no se permite habilitación
            else
            {
                //Deshabilitando los Tabs de edición
                btnVistaEcabezadoServicio.Enabled =
                btnVistaParadasServicio.Enabled = false;
                btnVistaResumenServicio.Enabled = this._id_servicio > 0 ? true : false;

                //Asignando Vista activa predeterminada
                mtvDocumentacion.SetActiveView(vwResumen);
            }
        }
        /// <summary>
        /// Inicializamos Valores de pestaña de resumen
        /// </summary>
        private void inicializaResumenServicio()
        {
            //Si hay servicio
            if (this._id_servicio > 0)
            {
                /* CONFIGURACIÓN DE ENCABEZADO DE PESTAÑA */
                //Instanciamos Servicio
                using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                {
                    //Instanciamos Cliente
                    using (SAT_CL.Global.CompaniaEmisorReceptor objCliente = new SAT_CL.Global.CompaniaEmisorReceptor(objServicio.id_cliente_receptor))
                    {
                        lblCliente.Text = objCliente.nombre;
                    }

                    lblCartaPorte.Text = objServicio.porte;
                    lblRefCliente.Text = objServicio.referencia_cliente;
                    lblObservacion.Text = objServicio.observacion_servicio;
                }

                /* CONFIGURACIÓN DE GRIDVIEW DE DETALLE DE PARADAS */
                //Obtenemos Paradas ligados al Id de Servicio
                using (DataTable mit = Parada.CargaParadasParaVisualizacionDespacho(this._id_servicio))
                {
                    //Cargamos Grid View
                    Controles.CargaGridView(gvParadas, mit, "IdOrigen", "", true, 3);
                    //Se almacena en tabla temporal
                    this._mit_resumen = mit;
                }
            }
            //Si no hay servicio
            else
            {
                lblCliente.Text = 
                lblCartaPorte.Text =
                lblRefCliente.Text = 
                lblObservacion.Text = "";
                Controles.InicializaGridview(gvParadas);
            }
        }
        /// <summary>
        /// Muestra u oculta controles de producto en base al tipo de evento
        /// </summary>
        private void visualizaProductoTipoEvento()
        {
            //Determinando si el evento es carga
            if (ddlTipoEventoParada.SelectedValue == "1")
            {
                //Mostrando Catálogo autocompletable
                txtProductoEventoParada.Visible = true;
                //Ocultando lista de existentes
                ddlProductoEventoParada.Visible = false;
            }
            //Si es una descarga (total/parcial)
            else
            {
                //Actualizando lista de productos
                //Si no hay un servicio activo aún
                if (this._id_servicio == 0)
                    cargaCatalogoProductosTemporal();
                //Si ya hay un servicio
                else
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProductoEventoParada, 67, "-- Seleccione un Producto --", this._id_servicio, "", Convert.ToInt32(gvParadasDocumentacion.SelectedIndex != -1 ? gvParadasDocumentacion.SelectedDataKey["Secuencia"] : "0"), "");

                //Ocultando Catálogo autocompletable
                txtProductoEventoParada.Visible = false;
                //Mostrando lista de existentes
                ddlProductoEventoParada.Visible = true;
            }
        }
        /// <summary>
        /// Realiza la validación de un producto seleccionado antes de guardar o editar un producto
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaSeleccionProducto()
        { 
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Si el tipo de parada es operativa
            if (gvParadasDocumentacion.SelectedDataKey["IdTipoParada"].ToString() == "1")
            {
                //Determinando el origen de la validación en base al tipo de evento a realizar
                switch (ddlTipoEventoParada.SelectedValue)
                {
                    case "1":
                        if (Cadena.RegresaCadenaSeparada(txtProductoEventoParada.Text, "ID:", 1) == "0")
                            resultado = new RetornoOperacion("Debe seleccionar un producto de la lista de sugerencias.");
                        break;
                    default:
                        if (ddlProductoEventoParada.SelectedValue == "0")
                            resultado = new RetornoOperacion("Debe seleccionar un producto de la lista de productos previamente cargados.");
                        break;
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Configura la ventana de referencias del registro solicitado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Planeacion.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencias", 800, 500, false, false, false, true, true, Page);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inicializa el control para captura de nuevo servicio
        /// </summary>
        public void InicializaControl()
        {
            InicializaControl(0, true, VistaDocumentacion.Encabezado);
        }
        /// <summary>
        /// Inicializa el control para captura de nuevo servicio
        /// </summary>
        /// <param name="id_ubicacion">Id de Ubicacion Inicial</param>
        public void InicializaControl(int id_ubicacion)
        {
            InicializaControl(0, true, VistaDocumentacion.Encabezado);
            //Asignando ubicación predeterminada
            if (id_ubicacion > 0)
            { 
                //Instanciando ubicación
                using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(id_ubicacion))
                {
                    txtUbicacionParada.Text = string.Format("{0} ID:{1}", u.descripcion, u.id_ubicacion);
                }
            }
        }
        /// <summary>
        /// Inicializa el control a partir de un Id de Servicio (Consulta / Edición)
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="hab_edicion">True para Indicar que deberá estár activa la edición del servicio</param>
        public void InicializaControl(int id_servicio, bool hab_edicion, VistaDocumentacion vista)
        {
            //Configurando atributos de control
            this._id_servicio = id_servicio;
            this._id_parada = 0;
            this._hab_edicion = hab_edicion;
            this._nueva_documentacion = false;
            this._vista_inicial = vista;
            //inicializando contenido de control
            inicializaControl();
        }
        /// <summary>
        /// Realiza el guardado de parada de servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaParadaServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando existencia de Cliente seleccionado
            if (Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtClienteServicio.Text, "ID:", 1)) > 0)
            {
                //Si no hay paradas registradas (primer parada se guarda temporalmente
                if (this._mit_parada.Rows.Count == 0)
                    resultado = guardaParadaTemporalServicio();
                //Si hay una parada
                else if (this._mit_parada.Rows.Count == 1)
                {
                    //Determinando el comando de guardado
                    switch (imbAgregarParada.CommandArgument)
                    {
                        case "Nuevo":
                            resultado = guardaParadaBDServicio();
                            break;
                        case "Edicion":
                            resultado = editaParadaTemporalServicio();
                            break;
                    }
                }
                //Si hay mas de una parada
                else
                {
                    //Determinando el comando de guardado
                    switch (imbAgregarParada.CommandArgument)
                    {
                        case "Nuevo":
                            resultado = guardaParadaBDServicio();
                            break;
                        case "Edicion":
                            resultado = editaParadaBDServicio();
                            break;
                    }                    
                }
            }
            //Si no hay un cliente
            else
                resultado = new RetornoOperacion("Debe Seleccionar un Cliente de la Lista que se despliega en la sección 'Encabezado'.");

            //Si no hay error en inserción de parada
            if (resultado.OperacionExitosa)
            {
                //Cargando Paradas
                cargaParadas();
                //Seleccionando parada insertada
                Controles.MarcaFila(gvParadasDocumentacion, resultado.IdRegistro.ToString(), "Id", "Id-Secuencia-IdTipoParada", this._mit_parada, "", 10, true, 2);
                //Limpiando controles de captura de parada
                inicializaControlesParada();
                //Cargando productos de la parada
                cargaProductos();
                //Limpiando controles de captura de producto
                inicializaControlesProducto();
                //Colocando foco en productos
                txtProductoEventoParada.Focus();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el borrado de la parada de servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaParadaServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando selección de parada
            if (gvParadasDocumentacion.SelectedIndex != -1)
            {
                //Recuperando Id de Parada y Secuencia
                int id_parada = Convert.ToInt32(gvParadasDocumentacion.SelectedDataKey["Id"]);

                //Si hay Parada en BD
                if (id_parada > 0)
                {
                    //Instanciando parada actual
                    using (Parada p = new Parada(id_parada))
                    {
                        //Instanciando servicio actual
                        using (Servicio servicio = new Servicio(this._id_servicio))
                        {
                            //Si el servicio está documentado
                            if (servicio.estatus == Servicio.Estatus.Documentado)
                                //Deshabilitando conforme reglas de documentación de servicio
                                resultado = p.DeshabilitaParadaServicio(((Usuario)Session["usuario"]).id_usuario, this._mit_parada.Rows.Count);
                            //Para cualquier otro estatus
                            else
                                //Deshabilitando conforme reglas de despacho
                                resultado = p.DeshabilitaParadaDespacho(((Usuario)Session["usuario"]).id_usuario, this._mit_parada.Rows.Count);
                        }
                    }
                }
                //Si es temporal
                else
                    //Se realiza el borrado de productos y eventos temporales
                    resultado = eliminaParadaTemporal();
            }
            else
                resultado = new RetornoOperacion("Debe seleccionar una parada de la lista.");

            //Si no hay error en eliminación de parada
            if (resultado.OperacionExitosa)
            {
                //Cargando Paradas
                cargaParadas();
                //Limpiando controles de captura de parada
                inicializaControlesParada();

                //Cargando Productos en blanco
                Controles.InicializaGridview(gvProductoEvento);
                this._mit_evento.Clear();
                this._mit_producto.Clear();

                //Limpiando controles de captura de producto
                inicializaControlesProducto();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el guardado del producto solicitado, asociandolo a la parada seleccionada
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaProductoEvento()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando selección de parada de servicio
            if (gvParadasDocumentacion.SelectedIndex != -1)
            {
                //Validando existencia de producto seleccionado
                resultado = validaSeleccionProducto();
                //Si no hay problema con la selección del producto
                if (resultado.OperacionExitosa)
                {
                    //Determinando la existencia de servicio, si no lo hay, se guarda en esquema temporal
                    if (this._id_servicio == 0)
                    {
                        //Determinando argumento de guardado
                        switch (imbAgregarProducto.CommandArgument)
                        {
                            case "Nuevo":
                                resultado = insertaProductoTemporal();
                                break;
                            case "Edicion":
                                resultado = editaProductoTemporal();
                                break;
                        }
                    }
                    //Si existe un servicio
                    else
                    {
                        //Determinando argumento de guardado
                        switch (imbAgregarProducto.CommandArgument)
                        {
                            case "Nuevo":
                                resultado = insertaProductoBD();
                                break;
                            case "Edicion":
                                resultado = editaProductoBD();
                                break;
                        }
                    }
                }
            }
            else
                resultado = new RetornoOperacion("Debe seleccionar una parada de la lista.");

            //Si no hay error en actualización de producto
            if (resultado.OperacionExitosa)
            {
                //Limpiando controles de captura de producto
                inicializaControlesProducto();
                //Cargando productos de la parada
                cargaProductos();
                //Colocando foco en producto
                txtProductoEventoParada.Focus();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el borrado del producto solicitado
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaProductoEvento()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando selección de producto
            if (gvProductoEvento.SelectedIndex != -1)
            {
                //Recuperando Id de Producto y Secuencia
                int id_producto = Convert.ToInt32(gvProductoEvento.SelectedDataKey["Id"]);

                //Si hay Producto en BD
                if (id_producto > 0)
                {
                    //Deshabilitando el producto
                    resultado = eliminaProductoBD();
                }
                //Si es temporal
                else
                    //Se realiza el borrado de productos y eventos temporales
                    resultado = eliminaProductoTemporal();
            }
            else
                resultado = new RetornoOperacion("Debe seleccionar un producto de la lista.");

            //Si no hay error en actualización de producto
            if (resultado.OperacionExitosa)
            {
                //Limpiando controles de captura de producto
                inicializaControlesProducto();
                //Cargando productos de la parada
                cargaProductos();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Guarda el encabezado de servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaEncabezadoServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Si no hay servicio activo
            if (this._id_servicio > 0)
            {
                //Inicialiando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando servicio
                    using (Servicio servicio = new Servicio(this._id_servicio))
                    {
                        //Actualizando servicio
                        resultado = servicio.ActualizaEncabezadoServicio(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtClienteServicio.Text, "ID:", 1, "0")), txtCartaPorte.Text.ToUpper(), txtReferencia.Text.ToUpper(), txtObservacion.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                        /*/Si no hay errores operativos
                        if (resultado.OperacionExitosa)
                            //Guardando referencias
                            resultado = wucTemperaturaServicio.GuardaTemperaturas(this._id_servicio);//*/
                        //Validando Operación Exitosa
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Referencia de Viaje
                            resultado = servicio.ActualizacionReferenciaViaje(txtReferencia.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                        }
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion(this._id_servicio);
                        scope.Complete();
                    }
                }
            }
            else
                resultado = new RetornoOperacion("No existe un servicio por actualizar.");

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Temperatura

        protected void wucTemperaturaServicio_ClickGuardarTemperaturas(object sender, EventArgs e)
        {

        }

        #endregion

        #region Clasificaciones

        /// <summary>
        /// Click guardar clasificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucClasificacion_ClickGuardar(object sender, EventArgs e)
        {
            //Validando existencia de un servicio activo
            if (this._id_servicio > 0)
                //Realizando guardado de clasificación
                wucClasificacion.GuardaCambiosClasificacionServicio();
        }
        /// <summary>
        /// Click cancelar clasificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucClasificacion_ClickCancelar(object sender, EventArgs e)
        {
            wucClasificacion.CancelaCambiosClasificacion();
        }

        #endregion
    }
}