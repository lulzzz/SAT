using SAT_CL;
using SAT_CL.Documentacion;
using System;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base; 
using SAT_CL.Despacho;
using SAT_CL.Seguridad;

namespace SAT.UserControls
{
    public partial class wucProducto : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de Obtener el Id de Servicio
        /// </summary>
        public int idServicio { get { return this._id_servicio; } }
        private int _id_compania;
        /// <summary>
        /// Atributo encargado de Obtener el Id de Compania
        /// </summary>
        public int idCompania { get { return this._id_compania; } }
        private DataSet dsProductos;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                ddlParadas.TabIndex =
                ddlTipo.TabIndex =
                txtProductoCarga.TabIndex =
                txtProductoDescarga.TabIndex =
                txtCantidad.TabIndex =
                ddlUnidad.TabIndex =
                txtPeso.TabIndex =
                ddlUnidadPeso.TabIndex =
                btnGuardarProducto.TabIndex =
                ddlTamanoReqDisp.TabIndex =
                lnkExportar.TabIndex =
                gvServicioProductos.TabIndex = value;
            }
            get { return ddlParadas.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor de habilitación de los controles contenidos
        /// </summary>
        public bool Enabled
        {
            set {
                //Asignando Habilitación
                ddlParadas.Enabled =
                ddlTipo.Enabled =
                txtProductoCarga.Enabled =
                txtProductoDescarga.Enabled =
                txtCantidad.Enabled =
                ddlUnidad.Enabled =
                txtPeso.Enabled =
                ddlUnidadPeso.Enabled =
                btnGuardarProducto.Enabled =
                lnkExportar.Enabled =
                gvServicioProductos.Enabled = value;
            }
            get { return ddlParadas.Enabled; }
        }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento disparado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando que se produzca un PostBack
            if (!(Page.IsPostBack))
                //Asignando Atributos
                asignaAtributos();
            else//Recuperando Atributos
                recuperaAtributos();

            //Invocando Carga de Confirguracion de Scripts
            //cargaScriptConfiguracionProducto();
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

        /*// <summary>
        /// Método Privado encargado de Cargar el Script que Actualiza los Catalogos de Autocompletado de los Productos
        /// </summary>
        private void cargaScriptConfiguracionProducto()
        {
            //Declarando Script de Configuración
            string script = @"<script type='text/javascript'>
                                //Obteniendo instancia actual de la página y añadiendo manejador de evento
                                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                                //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
                                function EndRequestHandler(sender, args) {
                                if (args.get_error() == undefined) {
                                ConfiguraJQueryControlProducto();
                                }
                                }
                                //Creando función para configuración de jquery en control de usuario
                                function ConfiguraJQueryControlProducto() {
                                    $(document).ready(function () {
                                        
                                        //Función de validación de campos
                                        var validacionProducto = function () {
                                        var isValidP1;
                                        
                                        //Validando la Visibilidad del Control
                                        if($('#"+ this.txtProductoCarga.ClientID + @"').is(':visible') == true){
                                            //Validando Producto de Carga
                                            isValidP1 = !$('#" + this.txtProductoCarga.ClientID + @"').validationEngine('validate');
                                        }
                                        else {
                                            //Validando Producto de Descarga
                                            isValidP1 = !$('#" + this.txtProductoDescarga.ClientID + @"').validationEngine('validate');
                                        }
                                        var isValidP2 = !$('#" + this.txtCantidad.ClientID + @"').validationEngine('validate');
                                        var isValidP3 = !$('#" + this.txtPeso.ClientID + @"').validationEngine('validate');
                                        return isValidP1 && isValidP2 && isValidP3;
                                        };

                                        //Función de validación de campos
                                        var validacionCalculoCantidadProducto = function () 
                                        {
                                        var isValidP1;
                                        
                                        //Validando Producto de Descarga
                                        isValidP1 = !$('#" + this.txtProductoDescarga.ClientID + @"').validationEngine('validate');
                                        
                                        return isValidP1;
                                        };
                                        
                                        //Validación de campos requeridos
                                        $('#" + this.lnkProducto.ClientID + @"').click(validacionCalculoCantidadProducto); 

                                        //Validación de campos requeridos
                                        $('#" + this.btnGuardarProducto.ClientID + @"').click(validacionProducto);

                                        //Serializando Control
                                        $('#" + this.ddlTipo.ClientID + @"').serialize();
                                
                                        //Sugerencias de producto
                                        $('#" + this.txtProductoCarga.ClientID + @"').autocomplete('../WebHandlers/AutoCompleta.ashx?id=5&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"&param2=1&param3=" + this.idServicio + @"');
                                        //Sugerencias de producto
                                        $('#" + this.txtProductoDescarga.ClientID + @"').autocomplete('../WebHandlers/AutoCompleta.ashx?id=5&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"&param2=2&param3=" + this.idServicio + @"');
                                    
                                    });
                                }
                                //Invocación Inicial de método de configuración JQuery
                                ConfiguraJQueryControlProducto();
                            </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaConfiguracionProductos", script, false);
        }//*/

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarProducto;
        /// <summary>
        /// Manejador de Evento "Eliminar"
        /// </summary>
        public event EventHandler ClickEliminarProducto;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarProducto(EventArgs e)
        {   //Validando que exista el Evento
            if (ClickGuardarProducto != null)
                //Iniciando Evento
                ClickGuardarProducto(this, e);
        }
        /// <summary>
        /// Evento que Manipula el Manejador "Eliminar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarProducto(EventArgs e)
        {   //Validando que exista el Evento
            if (ClickEliminarProducto != null)
                //Iniciando Evento
                ClickEliminarProducto(this, e);
        }

        #endregion

        /// <summary>
        /// Evento disparado al presionar el boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Validando que exista un Evento
            if (ClickGuardarProducto != null)
                //Iniciando Manejador
                OnClickGuardarProducto(e);
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvServicioProductos);
            //Limpiando Controles
            limpiaControles();
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Control
            ddlTipo.Enabled = true;
        }

       
        /// <summary>
        /// Evento disparado al cambiar el Indice del DropDownList "Paradas - Lugar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlParadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obteniendo Tipos
            using (DataTable dtTipos = CapaNegocio.m_capaNegocio.CargaCatalogo(11, "Ninguna", Convert.ToInt32(ddlParadas.SelectedValue), "", 0, ""))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTipos))
                {
                    //Cargando DropDownList
                    TSDK.ASP.Controles.CargaDropDownList(ddlTipo, dtTipos, "id", "descripcion");

                    //Validando que existe un unico evento en la parada, (Registro Ninguna y Descarga)
                    if (dtTipos.Rows.Count == 2)
                    {
                        //Recorremos la tabla con el conjunto de eventos 
                        foreach (DataRow dr in dtTipos.Rows)
                        {
                            //Asignamos de forma automatica el evento existente
                            if (Convert.ToInt32(dr["id"]) != 0)
                                ddlTipo.SelectedValue = dr["id"].ToString();

                            //Validando que exista un registro del tipo diferente a "Carga" es decir "Descarga"
                            if (Convert.ToInt32(dr["id"]) > 1)
                            {                    

                                //Validando que existan productos
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsProductos, "Table"))
                                {
                                    //Validamos que solo exista un solo tipo de producto de carga
                                    if (calculaTotalProductosCarga() == 1)
                                    {
                                        //Recorremos el DataSet de producto
                                        foreach (DataRow drP in dsProductos.Tables["Table"].Rows)
                                        {
                                            //Buscamos un evento de carga
                                            if (drP["IdTipo"].ToString() == "1")
                                            {
                                                //Cargamos el producto a descargar
                                                txtProductoDescarga.Text = drP["Producto"].ToString() + " ID:" + drP["IdProducto"].ToString();
                                                //Cargamos la cantidad sobrante a descargar
                                                txtCantidad.Text = calculaCantidadDescarga(Convert.ToInt32(drP["IdProducto"])).ToString();
                                                //Cargamos el peso sobrante a descargar
                                                txtPeso.Text = calculaPesoDescarga(Convert.ToInt32(drP["IdProducto"])).ToString();
                                                //Cargamos unidades de cantidad y peso
                                                ddlUnidad.SelectedValue = drP["IdUniCant"].ToString();
                                                ddlUnidadPeso.SelectedValue = drP["IdUniPeso"].ToString();
                                            }
                                        }
                                    }
                                    else
                                        inicializaControlesProducto();

                                }
                                else
                                    inicializaControlesProducto();
                            }
                        }
                    }

                    //Invocando Método de Configuración
                    configuraControlesProducto();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraControlesProducto();
        }

        /// <summary>
        /// Evento disparado al dar click en el vinculo Producto para calcular los totales pendientes por descargar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkProducto_Click(object sender, EventArgs e)
        {
            calculaDescargaProducto();
        }
        
        #region Eventos GridView "gvServicioProducto"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "gvServicioProductos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoReqDisp_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando que existan Llaves
            if (gvServicioProductos.DataKeys.Count > 0)
            {   //Cambiando Tamaño de Registros
                Controles.CambiaTamañoPaginaGridView(gvServicioProductos, this.dsProductos.Tables["Table"], Convert.ToInt32(ddlTamanoReqDisp.SelectedValue));
                //Mostrando Totales
                calculaTotales();
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Orden de los Datos del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicioProductos_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando que existan Llaves
            if (gvServicioProductos.DataKeys.Count > 0)
            {   //Asignando Ordenamiento
                this.dsProductos.Tables["Table"].DefaultView.Sort = lblOrdenarReqDisp.Text;
                //Cambiando Ordenamiento
                lblOrdenarReqDisp.Text = Controles.CambiaSortExpressionGridView(gvServicioProductos, this.dsProductos.Tables["Table"], e.SortExpression);
                //Mostrando Totales
                calculaTotales();
                //Limpiando Controles
                limpiaControles();
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Paginación del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicioProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando que existan Llaves
            if (gvServicioProductos.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this.dsProductos.Tables["Table"].DefaultView.Sort = lblOrdenarReqDisp.Text;
                //Cambiando el Tamaño de la Página
                Controles.CambiaIndicePaginaGridView(gvServicioProductos, this.dsProductos.Tables["Table"], e.NewPageIndex);
                //Mostrando totales al pie
                calculaTotales();
                //Limpiando Controles
                limpiaControles();
            }
        }
        /// Evento generado al dar click en Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacora(object sender, EventArgs e)
        {

            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvServicioProductos, sender, "lnk", false);
            //Validamos que existan Registros
            if (gvServicioProductos.DataKeys.Count > 0)
            {
                //Mostramos Bitácora
                inicializaBitacora(gvServicioProductos.SelectedValue.ToString(), "2", "Bitácora Produccto");
                gvServicioProductos.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Excel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando que existan Llaves
            if (gvServicioProductos.DataKeys.Count > 0)
                //Exportando Excel
                Controles.ExportaContenidoGridView(this.dsProductos.Tables["Table"], "IdUniCant", "IdUniPeso");
        }
        /// <summary>
        /// Evento disparado al presionar el LinkButton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {   //Validando que existan Llaves
            if (gvServicioProductos.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvServicioProductos, sender, "lnk", false);
                //Instanciando Producto de Servicio
                using (ServicioProducto sp = new ServicioProducto(Convert.ToInt32(gvServicioProductos.SelectedDataKey["Id"])))
                {   //Validando que el registro sea Valido
                    if (sp.id_servicio_producto != 0)
                    {   //Asignando Valores
                        ddlParadas.SelectedValue = sp.id_parada.ToString();
                        CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 11, "Ninguna", Convert.ToInt32(ddlParadas.SelectedValue), "", 0, "");
                        ddlTipo.Enabled = false;
                        ddlTipo.SelectedValue = sp.id_tipo.ToString();
                        txtCantidad.Text = (sp.cantidad < 0 ? sp.cantidad * -1 : sp.cantidad).ToString();
                        cargaCatalogos();
                        ddlUnidad.SelectedValue = sp.id_unidad.ToString();
                        txtPeso.Text = (sp.peso < 0 ? sp.peso * -1 : sp.peso).ToString();
                        ddlUnidadPeso.SelectedValue = sp.id_unidad_peso.ToString();
                        
                        //Instanciando Producto
                        using (SAT_CL.Global.Producto pro = new SAT_CL.Global.Producto(sp.id_producto))
                        {    
                            //Validando el Tipo
                            if (ddlTipo.SelectedValue == "1")
                            {
                                //Valor de Producto
                                txtProductoCarga.Text = pro.descripcion + " ID:" + pro.id_producto.ToString();
                                txtProductoDescarga.Text = "";
                            }
                            else
                            {
                                //Valor de Producto
                                txtProductoCarga.Text = "";
                                txtProductoDescarga.Text = pro.descripcion + " ID:" + pro.id_producto.ToString();
                            }
                        }

                        //Invocando Método de Configuración
                        configuraControlesProducto();
                    }
                }
            }
        }
        /// <summary>
        /// Evento disparado al presionar el LinkButton "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {   //Validando que existan registros en el grid
            if (gvServicioProductos.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvServicioProductos, sender, "lnk", false);
                //Validando que exista un Evento
                if (ClickEliminarProducto != null)
                    //Iniciando Manejador
                    OnClickEliminarProducto(e);
            }
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {   //Asignando Atributos
            ViewState["idServicio"] = this._id_servicio;
            ViewState["idCompania"] = this._id_compania;
            ViewState["DS"] = this.dsProductos;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {   //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["idServicio"]);
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)
                this._id_compania = Convert.ToInt32(ViewState["idCompania"]);
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)ViewState["DS"], "Table"))
                this.dsProductos = (DataSet)ViewState["DS"];
        }
        /// <summary>
        /// Método encargado de Configurar los Controles Depediendo de la Actividad
        /// </summary>
        private void configuraControlesProducto()
        {
            //Validando el Tipo del Control
            switch (ddlTipo.SelectedValue)
            {
                    //Para eventos de carga
                case "1":
                    {
                        //Asignando Visualización
                        txtProductoCarga.Visible = true;
                        txtProductoDescarga.Visible = false;
                        lnkProducto.Enabled = false;
                        break;
                    }
                    //Para cualquier evento diferente a cargas
                default:
                    {
                        //Asignando Visualización
                        txtProductoCarga.Visible = false;
                        txtProductoDescarga.Visible = true;
                        lnkProducto.Enabled = true;
                        break;
                    }
                
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {   
            //Cargando Catalogos de la Pagina
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlParadas, 1, "Ninguna", this._id_servicio, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 11, "Ninguna", Convert.ToInt32(ddlParadas.SelectedValue), "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidad, 2, "", 5, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadPeso, 2, "Otros", 2, "", 4, "");
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoReqDisp, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Productos del Servicio
        /// </summary>
        private void cargaProductosServicios()
        {   
            //Obteniendo Productos ligados al Servicio
            using (DataTable dt = ServicioProducto.ObtieneProductosServicio(this._id_servicio))
            {   
                //Validando que la Tabla contenga registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {   
                    //Inicializando indices de selección
                    TSDK.ASP.Controles.InicializaIndices(gvServicioProductos);
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvServicioProductos, dt, "Id", "");
                    //Añadiendo Tabla a Session
                    this.dsProductos = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this.dsProductos, dt, "Table");
                    //Mostrando Totales
                    calculaTotales();
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvServicioProductos);
                    //Eliminando Tabla de Sesion
                    this.dsProductos = TSDK.Datos.OrigenDatos.EliminaTablaDataSet(this.dsProductos, "Table");
                    //Mostrando Totales
                    gvServicioProductos.FooterRow.Cells[7].Text = string.Format("{0:#,###,###,###.00}", 0);
                    gvServicioProductos.FooterRow.Cells[9].Text = string.Format("{0:#,###,###,###.00}", 0);
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles de Captura
        /// </summary>
        private void limpiaControles()
        {   
            //Limpiando Valores
            ddlParadas.SelectedValue = "0";
            txtProductoCarga.Text = "";
            txtProductoDescarga.Text = "";
            txtCantidad.Text = "";
            txtPeso.Text = "";
            lblError.Text = "";
            ddlTipo.Enabled = true;
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucProducto.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }

        /// <summary>
        /// Metodo encargado de limpiar los controles de producto
        /// </summary>
        private void inicializaControlesProducto()
        {
            //Asignando Valores
            txtProductoCarga.Text =
            txtProductoDescarga.Text = "";
            txtCantidad.Text =
            txtPeso.Text = "0.00";
            //Cargando Catalogo
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidad, 2, "", 5, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadPeso, 2, "Otros", 2, "", 4, "");

        }
        /// <summary>
        /// Calcula el producto por descargar
        /// </summary>
        private void calculaDescargaProducto()
        {
            //Declaramos variables a utilizar
            int id_producto = 0;
            //Validamos que exista tipo de evento seleccionado
            if (ddlTipo.SelectedValue != null)
            {
                //Validamos que el tipo de evento seleccionado sea un evento de descarga
                if (Convert.ToInt32(ddlTipo.SelectedValue) > 1)
                {
                    //Obtenemos el id de producto
                    id_producto = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProductoDescarga.Text, "ID:", 1));
                    //Validando que existan productos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsProductos, "Table"))
                    {
                        //Recorremos el DataSet de producto
                        foreach (DataRow drP in dsProductos.Tables["Table"].Rows)
                        {
                            //En caso de encontrar el id de producto deseado
                            if (Convert.ToInt32(drP["IdProducto"]) == id_producto)
                            {
                                //Cargamos la cantidad sobrante a descargar
                                txtCantidad.Text = calculaCantidadDescarga(id_producto).ToString();
                                //Cargamos el peso sobrante a descargar
                                txtPeso.Text = calculaPesoDescarga(id_producto).ToString();
                                //Cargamos unidades de cantidad y peso
                                ddlUnidad.SelectedValue = drP["IdUniCant"].ToString();
                                ddlUnidadPeso.SelectedValue = drP["IdUniPeso"].ToString();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Metodo encargado de obtener el numero de productos que existen a la carga
        /// </summary>
        /// <returns></returns>
        private int calculaTotalProductosCarga()
        {
            //Obtenemos el numero de productos que existen en eventos de carga
            int i = (from DataRow r in dsProductos.Tables["Table"].Rows
                     where r.Field<byte>("IdTipo") == 1
                     select r.Field<int>("IdProducto")).DefaultIfEmpty(0).Distinct().Count(id_producto => id_producto > 0);

            return i;
        }
        /// <summary>
        /// Metodo encargado de calcular la cantidad sobrante por descargar
        /// </summary>
        /// <returns></returns>
        private decimal calculaCantidadDescarga(int id_producto)
        {
            //Obtenemos el numero de productos que existen en eventos de carga
            decimal i = (from DataRow r in dsProductos.Tables["Table"].Rows
                         where r.Field<int>("IdProducto") == id_producto
                         select r.Field<decimal>("Cantidad")).Sum();

            if (i < 0)
                return 0;
            return i;
        }

        /// <summary>
        /// Metodo encargado de clacular el peso sobrante por descargar
        /// </summary>
        /// <returns></returns>
        private decimal calculaPesoDescarga(int id_producto)
        {
            //Obtenemos el numero de productos que existen en eventos de carga
            decimal i = (from DataRow r in dsProductos.Tables["Table"].Rows
                         where r.Field<int>("IdProducto") == id_producto
                         select r.Field<decimal>("Peso")).Sum();
            if (i < 0)
                return 0;
            return i;
        }

        
        private bool validaUnidadesCantidadPeso(int id_producto, int id_unidad_cantidad, int id_unidad_peso)
        {
            //Validando que Existan Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(this.dsProductos, "Table"))
            {
                //Obtenemos el numero de productos que existen en eventos de carga
                byte peso = (from DataRow r in dsProductos.Tables["Table"].Rows
                                where r.Field<int>("IdProducto") == id_producto
                                select r.Field<byte>("IdUniPeso")).DefaultIfEmpty<byte>(0).Distinct().First<byte>();
                
                //Obtenemos el numero de productos que existen en eventos de carga
                byte cantidad = (from DataRow r in dsProductos.Tables["Table"].Rows
                                where r.Field<int>("IdProducto") == id_producto
                                 select r.Field<byte>("IdUniCant")).DefaultIfEmpty<byte>(0).Distinct().First<byte>();

                //Si el peso y la cantidad del producto son iguales
                if (cantidad == id_unidad_cantidad && peso == id_unidad_peso)
                    return true;

                //Si no existe el peso y la cantidad
                if(cantidad == 0 && peso == 0)
                    return true;

                return false;
            }
            return true;
        }

        /// <summary>
        /// Metodo encargado de validar antes de realizar actualizacion o insercion de productos servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaGuardaProductoServicio(int id_producto_servicio)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declaramos variables auxiliares
            decimal cantidad_edicion = 0;
            decimal peso_edicion = 0;

            //Validamos si existe producto servicio
            if (id_producto_servicio != 0)
            {
                //Instanciamos el producto servicio
                using (ServicioProducto sp = new ServicioProducto(id_producto_servicio))
                {
                    //Obtenemos los valores de interes
                    cantidad_edicion = -1 * sp.cantidad;
                    peso_edicion = -1 * sp.peso;
                }
            }
            
            //Asignamos el producto de acuerdo al proceso a realizar
            string producto = txtProductoCarga.Visible ? txtProductoCarga.Text : txtProductoDescarga.Text;
            
            //Validando que se haya seleccionado una parada
            if (ddlParadas.SelectedValue != "" && ddlParadas.SelectedValue != "0")
            {
                //Validando que se haya seleccionado un tipo de operación 
                if (ddlTipo.SelectedValue != "" && ddlTipo.SelectedValue != "0")
                {
                    //Validamos si las unidades de carga y descarga son iguales en cantidad y peso
                    if (validaUnidadesCantidadPeso(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(producto, "ID:", 1)), Convert.ToInt32(ddlUnidad.SelectedValue), Convert.ToInt32(ddlUnidadPeso.SelectedValue)))
                    {
                        //En los casos en los que el evento sea un evento distinto a una carga
                        if (Convert.ToInt32(ddlTipo.SelectedValue) > 1)
                        {
                            //Validamos que las cantidades de producto por descargar no excedan el maximo de producto cargado
                            if ((calculaCantidadDescarga(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(producto, "ID:", 1))) + cantidad_edicion) >= Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidad.Text, "0")))
                            {
                                //Validamos que los pesos de producto por descargar no excedan el maximo de producto cargado
                                if ((calculaPesoDescarga(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(producto, "ID:", 1))) + peso_edicion) >= Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPeso.Text, "0")))
                                {
                                    result = new RetornoOperacion("", true);
                                }
                                else
                                    //Personalizando Excepcion
                                    result = new RetornoOperacion("Los pesos de descarga no deben de exceder el peso cargado", false);
                            }
                            else
                                //Personalizando Excepcion
                                result = new RetornoOperacion("Las cantidades de descarga no deben de exceder la cantidad cargada", false);
                        }
                        else
                            //Asignando el retorno positivo
                            result = new RetornoOperacion("", true);
                    }
                    else
                        //Personalizando Excepcion
                        result = new RetornoOperacion("Las unidades de cantidad y peso deben coincidir en carga y descarga", false);

                }
                else
                    //Personalizando Excepcion
                    result = new RetornoOperacion("Debe de Seleccionar un Tipo Valido", false);
            }
            else
                //Personalizando Excepcion
                result = new RetornoOperacion("Debe de seleccionar una Parada", false);

            //Retornamos el resultado de la validacion
            return result;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_compania">Id de Compania</param>
        public void InicializaControl(int id_servicio, int id_compania)
        {   //Asignando Atributos
            this._id_servicio = id_servicio;
            this._id_compania = id_compania;
            //Limpiando Controles de texto
            limpiaControles();
            //Cargando Catalogos
            cargaCatalogos();
            //Cargando Productos Ligados al Servicio
            cargaProductosServicios();            
        }

        
        
        /// <summary>
        /// Método Público encargado de Guardar los Cambios en los Productos
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaProductoServicio()
        {

            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Declarando variables Auxiliares
            string producto = txtProductoCarga.Visible ? txtProductoCarga.Text : txtProductoDescarga.Text;

            int id_producto_servicio = 0;

            //Validando que exista un producto seleccionado para conocer si es una edicion o insercion
            if (gvServicioProductos.SelectedIndex != -1)
                //Obtenemos el id de producto servicio
                id_producto_servicio = Convert.ToInt32(gvServicioProductos.SelectedDataKey["Id"]);                

            //Asignamos el resultado de la validacion
            result = validaGuardaProductoServicio(id_producto_servicio);

            //Realizamos la validación de actualizacion 
            if (result.OperacionExitosa)
            {
                //Validando que exista un producto seleccionado para conocer si es una edicion o insercion
                if (id_producto_servicio != 0)
                {
                    //Instanciando Producto de Servicio
                    using (ServicioProducto sp = new ServicioProducto(id_producto_servicio))
                    {
                        //Validando que el registro sea Valido
                        if (sp.id_servicio_producto != 0)
                        {
                            //Actualizando Producto
                            result = sp.EditaServicioProducto(sp.id_servicio, Convert.ToInt32(ddlParadas.SelectedValue), Convert.ToByte(ddlTipo.SelectedValue),
                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(producto, "ID:", 1)),
                                Convert.ToInt32(ddlTipo.SelectedValue) == 2 || Convert.ToInt32(ddlTipo.SelectedValue) == 3 ? Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidad.Text, "0")) * -1 : Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidad.Text, "0")),
                                Convert.ToInt32(ddlUnidad.SelectedValue), Convert.ToInt32(ddlTipo.SelectedValue) == 2 || Convert.ToInt32(ddlTipo.SelectedValue) == 3 ? Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPeso.Text, "0")) * -1 : Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPeso.Text, "0")),
                                Convert.ToInt32(ddlUnidadPeso.SelectedValue),
                                ((Usuario)Session["usuario"]).id_usuario);
                        }
                        else
                            //Personalizando Excepcion
                            result = new RetornoOperacion("El producto no pudo ser recuperado desde la BD, puede ser que ya no exista.");
                    }
                }
                else                
                    //Insertando Registro
                    result = ServicioProducto.InsertaServicioProducto(this._id_servicio, Convert.ToInt32(ddlParadas.SelectedValue),
                                Convert.ToByte(ddlTipo.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(producto, "ID:", 1)),
                                Convert.ToInt32(ddlTipo.SelectedValue) == 2 || Convert.ToInt32(ddlTipo.SelectedValue) == 3 ? Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidad.Text, "0")) * -1 : Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidad.Text, "0")),
                                Convert.ToInt32(ddlUnidad.SelectedValue), Convert.ToInt32(ddlTipo.SelectedValue) == 2 || Convert.ToInt32(ddlTipo.SelectedValue) == 3 ? Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPeso.Text, "0")) * -1 : Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPeso.Text, "0")),
                                Convert.ToInt32(ddlUnidadPeso.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);             

            }
            //Validando que la operacion haya sido exitosa
            if (result.OperacionExitosa)

                //Inicializando Control
                InicializaControl(this._id_servicio, this._id_compania);

            //Mostrando Mensaje de Error
            lblError.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Eliminar los Productos
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaProductoServicio()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que exista un registro seleccionado
            if (gvServicioProductos.SelectedIndex != -1)
            {   //Instanciando Producto de Servicio
                using (ServicioProducto sp = new ServicioProducto(Convert.ToInt32(gvServicioProductos.SelectedDataKey["Id"])))
                {   //Validando que el registro sea Valido
                    if (sp.id_servicio_producto != 0)
                    {   //Deshabilitando el registro
                        result = sp.DeshabilitaServicioProducto(((Usuario)Session["usuario"]).id_usuario);
                        //Validando que la operacion haya sido exitosa
                        if (result.OperacionExitosa)
                            //Inicializando Control
                            InicializaControl(this._id_servicio, this._id_compania);
                        else//Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvServicioProductos);
                        //Mostrando Mensaje de Error
                        lblError.Text = result.Mensaje;
                    }
                    else//Inicializando Indices
                        TSDK.ASP.Controles.InicializaIndices(gvServicioProductos);
                }
            }
            else//Instanciando Exception
                result = new RetornoOperacion("No existen registros que eliminar.");
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Calcula y muestra los totales al pie de GridView
        /// </summary>
        private void calculaTotales()
        {   //Mostrando Totales
            gvServicioProductos.FooterRow.Cells[7].Text = string.Format("{0:0.00}", this.dsProductos.Tables["Table"].Compute("SUM(Cantidad)", ""));
            gvServicioProductos.FooterRow.Cells[9].Text = string.Format("{0:0.00}", this.dsProductos.Tables["Table"].Compute("SUM(Peso)", ""));
        }

        #endregion
    }
}