using Microsoft.SqlServer.Types;
using SAT_CL.Global;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucBitacoraMonitoreoHistorial : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id de Bitácora seleccionado
        /// </summary>
        private int _id_bitacora_monitoreo;
        /// <summary>
        /// Id de tabla de la bitácora 
        /// </summary>
        private int _id_tabla;
        /// <summary>
        /// Id de registo de la bitácora 
        /// </summary>
        private int _id_registro;
        /// <summary>
        /// Tabla con los bitácoras encontradas
        /// </summary>
        private DataTable _mitBitacora;
        /// <summary>
        /// Indica si se debe permitir la visualización de las columnas con los controles consultar 
        /// </summary>
        private bool _hab_consultar;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {
                txtFechaInicio.TabIndex =
                txtFechaFin.TabIndex =
                txtNoServicio.TabIndex =
                ddlTipo.TabIndex =
                ddlTamanoBitacoraMonitoreo.TabIndex =
                lkbExportarBitacoraMonitoreo.TabIndex =
                gvBitacoraMonitoreo.TabIndex = value;
            }
            get
            {
                return txtFechaInicio.TabIndex;
            }
        }
        /// <summary>
        /// Permite obtener el valor de disponibilidad de los controles (true/false).
        /// </summary>
        public bool Enabled
        {
            set
            {
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled =
                ddlTipo.Enabled =
                ddlTamanoBitacoraMonitoreo.Enabled =
                lkbExportarBitacoraMonitoreo.Enabled =
                gvBitacoraMonitoreo.Enabled = value;
            }
            get { return this.txtFechaInicio.Enabled; }
        }

        /// <summary>
        /// Obtiene el Id de Bitácora Monitoreo seleccionado actualmente
        /// </summary>
        public int id_bitacora_monitoreo { get { return this._id_bitacora_monitoreo; } }

        #endregion

        #region Manejadores de Eventos

        public event EventHandler lkbConsultar;
        public event EventHandler btnNuevoBitacora;
        /// <summary>
        /// Evento Click en botón Consultar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnlkbConsultar(EventArgs e)
        {
            //Si hay manejador asignado
            if (lkbConsultar != null)
                lkbConsultar(this, e);
        }
        /// <summary>
        /// Evento Click en Botón nuevo Bitácora Monitoreo
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnbtnNuevoBitacora(EventArgs e)
        {
            //Si hay manejador asignado
            if (btnNuevoBitacora != null)
                btnNuevoBitacora(this, e);
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento producido al  cargar el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!(Page.IsPostBack))
            {
                //Implementado seguridad de este recurso               
            }
            else
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
            //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento producido al dar click sobre algún botón del GridView de Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacoraMonitoreoClick(object sender, EventArgs e)
        {
            //Si existen registros en el gridview
            if (gvBitacoraMonitoreo.DataKeys.Count > 0)
            {
                //Seleccionando fila del gv
                TSDK.ASP.Controles.SeleccionaFila(gvBitacoraMonitoreo, sender, "lnk", false);
                //Indicando la bitácora señalado
                this._id_bitacora_monitoreo = Convert.ToInt32(gvBitacoraMonitoreo.SelectedDataKey.Value);
                //Determinando que botón fue pulsado
                switch (((LinkButton)sender).CommandName)
                {
                    case "Consultar":
                        if (lkbConsultar != null)
                            OnlkbConsultar(e);
                        break;
                }

                //Inicializando indices de selección
                TSDK.ASP.Controles.InicializaIndices(gvBitacoraMonitoreo);
            }
        }
        /// <summary>
        /// Evento corting de gridview de Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacoraMonitoreo_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvBitacoraMonitoreo.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitBitacora.DefaultView.Sort = lblOrdenadoBitacoraMonitoreo.Text;
                //Cambiando Ordenamiento
                lblOrdenadoBitacoraMonitoreo.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvBitacoraMonitoreo, this._mitBitacora, e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacoraMonitoreo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvBitacoraMonitoreo, this._mitBitacora, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoBitacoraMonitoreo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvBitacoraMonitoreo, this._mitBitacora, Convert.ToInt32(ddlTamanoBitacoraMonitoreo.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarBitacoraMonitoreo_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitBitacora, "");
        }
        /// <summary>
        /// Evento click en botón nuevo Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoBitacoraMonitoreo_Click(object sender, EventArgs e)
        {
            if (btnNuevoBitacora != null)
                OnbtnNuevoBitacora(e);
        }
        /// <summary>
        /// Evento que permite la busqueda de bitacora monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBusca_Click(object sender, EventArgs e)
        {
            //Invoca al método cargaHistorialBitacora
            cargaHistorialBitacora();
        }
        /// <summary>
        /// Evento de enlace a datos de cada fila del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacoraMonitoreo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si no se ha solicitado ocultar columnas de acciones
            if (!this._hab_consultar)
            {
                //Si es un fila de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //obteniendo controles linkbutton para consulta y término de Bitácora Monitoreo
                    using (LinkButton lkbConsultar = (LinkButton)e.Row.FindControl("lkbConsultarVencimiento"))
                    {
                        //Deshabilitando controles
                        lkbConsultar.Enabled = false;
                    }
                }
            }

            //Si es un fila de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo HyperLink
                using (HyperLink hylMaps = (HyperLink)e.Row.FindControl("hylMaps"))
                {
                    //Obteniendo Origen de la Fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    //Validando si existe el Id
                    if (!row["Id"].ToString().Equals(""))

                        //Construyendo URL 
                        hylMaps.NavigateUrl = string.Format("{0}?P1={1}&P2={2}", "~/Maps/UbicacionExternaMapa.aspx", 1, row.Field<int>("Id"));
                }
            }
        }
        /// <summary>
        /// Click en botón Pedir Ubicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSolicitarUbicacionMovil_Click(object sender, EventArgs e)
        {
            //Solicitando ubicación acorde al tipo de entidad
            solicitaUbicacion();
        }
        /// <summary>
        /// Evento Producido al Dar Click al Link 'Ver Mapas'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbMapsTipo_Click(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraNavegacion();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Proveedor GPS"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProveedorGPS_Click(object sender, EventArgs e)
        {
            //Validando que la Entidad sea una Unidad
            if (this._id_tabla == 19)
            {
                //Validando que exista la Unidad
                if (this._id_registro > 0)
                {
                    //Cargando los Proveedores Disponibles por Unidad
                    using (DataTable dtServiciosGPS = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(101, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", this._id_registro, ""))
                    {
                        //Validando que existan Registros
                        if (Validacion.ValidaOrigenDatos(dtServiciosGPS))
                        {
                            //Validando que solo Exista un Servicio GPS
                            if (dtServiciosGPS.Rows.Count == 1)
                            {
                                //Recorriendo Registros
                                foreach (DataRow dr in dtServiciosGPS.Rows)
                                {
                                    //Guargando Incidencia de GPS
                                    RetornoOperacion result = guardaIncidenciaGPS(Convert.ToInt32(dr["id"]));

                                    //Mostrando Notificación
                                    ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                    //Terminando Ciclo
                                    break;
                                }
                            }
                            else if (dtServiciosGPS.Rows.Count > 1)
                            {
                                //Cargando Control
                                Controles.CargaDropDownList(ddlServicioGPS, dtServiciosGPS, "id", "descripcion");

                                //Mostrando Ventana Modal
                                gestionaVentana(btnProveedorGPS, "ProveedorGPS");
                            }
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(btnProveedorGPS, new RetornoOperacion("No Existen Servicios GPS para esta Unidad"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
                else
                    //Instanciando Excepción
                    ScriptServer.MuestraNotificacion(btnProveedorGPS, new RetornoOperacion("No Existe la Unidad"), ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            else
                //Instanciando Excepción
                ScriptServer.MuestraNotificacion(btnProveedorGPS, new RetornoOperacion("Solo las Unidades tienen Proveedor de GPS"), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarProveedorGPS_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Cerrando Ventana
            gestionaVentana(lkb, lkb.CommandName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Invocando Método de Guardado
            result = guardaIncidenciaGPS(Convert.ToInt32(ddlServicioGPS.SelectedValue));

            //Validando Resultado
            if (result.OperacionExitosa)

                //Ocultando Ventana
                gestionaVentana(btnSeleccionar, "ProveedorGPS");

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que permite asignar los valores a los controles DropDownList.
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando catálogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoBitacoraMonitoreo, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "Todo", 3138);
            SAT_CL.Seguridad.Forma.AplicaSeguridadControlusuarioWeb(this, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        }

        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdTabla"] = this._id_tabla;
            ViewState["IdRegistro"] = this._id_registro;
            ViewState["IdBitacora"] = this._id_bitacora_monitoreo;
            ViewState["mitBitacora"] = this._mitBitacora;
            ViewState["HabConsultar"] = this._hab_consultar;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["IdTabla"] != null && ViewState["IdRegistro"] != null
                && ViewState["IdBitacora"] != null && ViewState["HabConsultar"] != null
               )
            {
                this._id_tabla = Convert.ToInt32(ViewState["IdTabla"]);
                this._id_registro = Convert.ToInt32(ViewState["IdRegistro"]);
                this._id_bitacora_monitoreo = Convert.ToInt32(ViewState["IdBitacora"]);
                this._hab_consultar = Convert.ToBoolean(ViewState["HabConsultar"]);
                if (ViewState["mitBitacora"] != null)
                    this._mitBitacora = (DataTable)ViewState["mitBitacora"];
            }

        }
        /// <summary>
        /// Inicializa control de Usuario
        /// </summary>
        /// <param name="id_tabla">Tabla de la bitácora (Unidad =19)</param>
        /// <param name="id_registro">Id Registro</param>
        public void InicializaControl(int id_tabla, int id_registro)
        {
            InicializaControl(id_tabla, id_registro, true);
        }

        /// <summary>
        /// Inicializa control de Usuario
        /// </summary>
        /// <param name="id_tabla">Tabla de la bitácora (Unidad =19)</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="hab_consultar">Habilitación de la Edición</param>
        public void InicializaControl(int id_tabla, int id_registro, bool hab_consultar)
        {
            //Asignando a atributos privados
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;
            this._hab_consultar = hab_consultar;

            //Habilitando petición de ubicación sólo si la compañía tiene configurada aplicación móvil
            btnSolicitarUbicacionMovil.Visible = CompaniaEmisorReceptor.ValidaConfiguracionUsoAplicacionMovil(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor).OperacionExitosa;

            //Carga catálogo de tipos de bitácora
            cargaCatalogos();
            //Asigna el valor de inicio a los controles txtFechaInicio (menos un dia) y txtFechaFin el valor de la fecha actual
            txtFechaInicio.Text = Convert.ToString(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Date.ToString("dd/MM/yyyy HH:mm"));
            txtFechaFin.Text = Convert.ToString(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(1).AddMinutes(-1).ToString("dd/MM/yyyy HH:mm"));

            //Cargando catálogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoBitacoraMonitoreo, "", 18);
            //Realizando carga de Bitácora
            cargaHistorialBitacora();
        }
        /// <summary>
        /// Carga la Bitácora Monitoreo aplicables a los criterios de búsqueda solicitados
        /// </summary>
        private void cargaHistorialBitacora()
        {
            //Creación del objeto retono
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al método valida Fecha
            retorno = validaFechas();
            //Valida si las fechas.
            if (retorno.OperacionExitosa)
            {
                //Indicando que no hay selección de bitácora aún
                this._id_bitacora_monitoreo = 0;
                //Cargando Bitácora Monitoreo
                this._mitBitacora = SAT_CL.Monitoreo.BitacoraMonitoreo.CargaHistorialBitacoraMonitoreo(this._id_tabla, this._id_registro, Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text), Convert.ToByte(ddlTipo.SelectedValue), txtNoServicio.Text);
                //Si no hay registros
                if (this._mitBitacora == null)
                    TSDK.ASP.Controles.InicializaGridview(gvBitacoraMonitoreo);
                else
                    //Mostrandolos en gridview
                    TSDK.ASP.Controles.CargaGridView(gvBitacoraMonitoreo, this._mitBitacora, "Id-IdServicio-IdMovimiento", lblOrdenadoBitacoraMonitoreo.Text, true, 1);

                //Si se ha solicitado ocultar columnas de consulta y término
                if (!this._hab_consultar)
                    gvBitacoraMonitoreo.Columns[6].Visible = false;
                //Se las fechas son corectas el valor del objeto retorno el vacio, (no mandara mensaje de operaciòn exitosa)
                retorno = new RetornoOperacion(" ");
            }
            //Manda un mensaje acorde a la ejecusion exitosa o no de la operación
            lblError.Text = retorno.Mensaje;
        }
        /// <summary>
        /// Método que valida la fecha de inicio sea menor a la Fecha Fin
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaFechas()
        {
            //Creación del objeto retorno con valor 1 al constructor de la clase
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Compara los datos encontrados en los controles de fecha inicio y fecha fin(si la fechaInicio es menor a fechaFin y el resultado de la comparacion es a 0)
            if (Convert.ToDateTime(txtFechaInicio.Text).CompareTo(Convert.ToDateTime(txtFechaFin.Text)) > 0)
                //  Al objeto retorno se le asigna un mensaje de error en la validación de las fechas.
                retorno = new RetornoOperacion(" Fecha Inicio debe ser MENOR que Fecha Fin.");
            //Retorna el resultado al método 
            return retorno;
        }
        /// <summary>
        /// Realiza la petición de ubicación actual a la entidad actual en la bitácora
        /// </summary>
        private void solicitaUbicacion()
        {
            //Realizando petición
            RetornoOperacion resultado = new RetornoOperacion("No es posible completar esta acción con el recurso actual.");

            //Determinando el tipo de entidad en base a la tabla de procedencia
            switch (_id_tabla)
            {
                //Operador
                case 76:
                    resultado = NotificacionPush.Instance.NotificacionPeticionUbicacion(SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Operador, _id_registro);
                    break;
                //Unidad
                case 19:
                    resultado = NotificacionPush.Instance.NotificacionPeticionUbicacion(SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Unidad, _id_registro);
                    break;
                //Transportista
                case 25:
                    //TODO: implementar según aplique
                    break;
            }

            //Mostrando resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnSolicitarUbicacionMovil, resultado, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Configurar
        /// </summary>
        private void configuraNavegacion()
        {
            //Declarando URL
            string url = "";

            //Declarando Objeto de Retorno
            RetornoOperacion result = validaFechas();

            //Validando que existan fechas
            if (result.OperacionExitosa)
            {
                //Obteniendo Fechas
                DateTime inicio, fin;
                DateTime.TryParse(txtFechaInicio.Text, out inicio);
                DateTime.TryParse(txtFechaFin.Text, out fin);

                //Construyendo URL 
                url = string.Format("{0}?P1={1}&P2={2}&P3={2}&P4={3:yyyy-MM-ddTHH:mm}|{4:yyyy-MM-ddTHH:mm}&P5={5}&P6={6}",
                            TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControl/wucBitacoraMonitoreoHistorial.ascx",
                            "~/Maps/UbicacionExternaMapa.aspx"), 2, ddlTipo.SelectedValue, inicio, fin, this._id_tabla, this._id_registro);

                //Definiendo Configuracion de la Ventana
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width='+screen.width+',height='+screen.height+',fullscreen=YES";
                //Abriendo Nueva Ventana
                TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Ubicación Incidencia", configuracion, Page);
            }
            else
                //Mostrando Excepción
                ScriptServer.MuestraNotificacion(lkbMapsTipo, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Gestinar la Ventana 
        /// </summary>
        /// <param name="sender">Control que Invoca la Ventana</param>
        /// <param name="nombre_ventana">Nombre de la Ventana</param>
        private void gestionaVentana(Control sender, string nombre_ventana)
        {
            //Validando Ventana
            switch (nombre_ventana)
            {
                case "ProveedorGPS":
                    {
                        //Gestionando Ventana
                        ScriptServer.AlternarVentana(sender, nombre_ventana, "contenedorVentanaProveedorGPS", "ventanaProveedorGPS");
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Guardar la Incidencia de GPS a la Bitacora de Monitoreo
        /// </summary>
        /// <param name="id_proveedor_ws">Proveedor de GPS</param>
        private RetornoOperacion guardaIncidenciaGPS(int id_proveedor_ws)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Unidad
            using (Unidad unidad = new Unidad(this._id_registro))
            using (SAT_CL.Monitoreo.ProveedorWSUnidad pu = SAT_CL.Monitoreo.ProveedorWSUnidad.ObtieneAntenaPredeterminada(this._id_registro))
            {
                //Instanciando Unidad
                if (unidad.habilitar)
                {
                    //Declarando Objetos de Retorno
                    DateTime fecha_gps;
                    double latitud = 0.00, longitud = 0.00;
                    decimal velocidad = 0.00M, cant_combustible = 0.00M;
                    string ubicacion_desc = "";
                    bool bit_encendido = false;

                    //Obtiene Posición Actual
                    result = SAT_CL.Monitoreo.ProveedorWSUnidad.ObtienePosicionActualUnidad(id_proveedor_ws, this._id_registro,
                                                    out ubicacion_desc, out latitud, out longitud, out fecha_gps, out velocidad, 
                                                    out bit_encendido, out cant_combustible);

                    //Validando que haya una Respuesta
                    if (result.OperacionExitosa)
                    {
                        //Declarando Variables Auxiliares
                        int id_servicio = 0, id_parada = 0, id_movimiento = 0;

                        //Si existe la Estancia
                        if (unidad.id_estancia > 0)
                        {
                            //Instanciando Estancia
                            using (SAT_CL.Despacho.EstanciaUnidad estancia = new SAT_CL.Despacho.EstanciaUnidad(unidad.id_estancia))
                            {
                                //Si existe
                                if (estancia.habilitar)
                                {
                                    //Instanciando Parada
                                    using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(estancia.id_parada))
                                    {
                                        //Si existe la Parada
                                        if (stop.habilitar)
                                        {
                                            //Asignando Valores
                                            id_servicio = stop.id_servicio;
                                            id_parada = stop.id_parada;
                                            id_movimiento = 0;
                                        }
                                    }
                                }
                            }
                        }
                        else if (unidad.id_movimiento > 0)
                        {
                            //Instanciando Movimiento
                            using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(unidad.id_movimiento))
                            {
                                //Si existe el Movimiento
                                if (mov.habilitar)
                                {
                                    //Asignando Valores
                                    id_servicio = mov.id_servicio;
                                    id_parada = 0;
                                    id_movimiento = mov.id_movimiento;
                                }
                            }
                        }

                        //Insertando Bitacora de Monitoreo
                        result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.AntenaGPS,
                                    6, id_servicio, id_parada, SAT_CL.Despacho.ParadaEvento.ObtienerPrimerEvento(id_parada), id_movimiento, this._id_tabla, this._id_registro,
                                    SqlGeography.Point(latitud, longitud, 4326), ubicacion_desc, "Petición de Ubicación GPS", fecha_gps, velocidad, bit_encendido,
                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe la Unidad");
            }

            //Validando que haya una Respuesta
            if (result.OperacionExitosa)

                //Invoca al método cargaHistorialBitacora
                cargaHistorialBitacora();

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}