using SAT_CL.Global;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAT.UserControls
{
    public partial class wucVencimientosHistorial : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id de Tipo  de Aplicación de Vencimientos
        /// </summary>
        private TipoVencimiento.TipoAplicacion _tipo_aplicacion;
        /// <summary>
        /// Id de Recurso al que corresponden los vencimientos
        /// </summary>
        private int _id_recurso;
        /// <summary>
        /// Id de Vencimiento seleccionado
        /// </summary>
        private int _id_vencimiento;
        /// <summary>
        /// Tabla con los vencimientos encontrados
        /// </summary>
        private DataTable _mitVencimientos;
        /// <summary>
        /// Indica si se debe permitir la visualización de las columnas con los controles consultar y terminar
        /// </summary>
        private bool _hab_consultar_terminar;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {
                ddlEstatus.TabIndex =
                ddlTipo.TabIndex =
                ddlPrioridad.TabIndex =
                chkRangoFechas.TabIndex =
                rdbInicioVencimiento.TabIndex =
                rdbFinVenciamiento.TabIndex =
                txtFechaInicio.TabIndex =
                txtFechaFin.TabIndex =
                ddlTamanoVencimientoHistorial.TabIndex =
                lkbExportarVencimientoHistorial.TabIndex =
                gvVencimientos.TabIndex = value;
            }
            get
            {
                return ddlTamanoVencimientoHistorial.TabIndex;
            }
        }
        /// <summary>
        /// Obtiene el Id de Vencimiento seleccionado actualmente
        /// </summary>
        public int id_vencimiento { get { return this._id_vencimiento; } }
        /// <summary>
        /// Obtiene el Id de recurso
        /// </summary>
        public int id_recurso { get { return this._id_recurso; } }
        /// <summary>
        /// Obtiene el Tipo de Aplicación
        /// </summary>
        public TipoVencimiento.TipoAplicacion tipo_aplicacion { get { return this._tipo_aplicacion; } }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Manejadores de Eventos

        public event EventHandler lkbConsultar;
        public event EventHandler lkbTerminar;
        public event EventHandler btnNuevoVencimiento;
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
        /// Evento Click en botón Terminar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnlkbTerminar(EventArgs e)
        {
            
            //Si hay manejador asignado
            if (lkbTerminar != null)
                lkbTerminar(this, e);
        }
        /// <summary>
        /// Evento Click en Botón nuevo vencimiento
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnbtnNuevoVencimiento(EventArgs e)
        { 
            //Si hay manejador asignado
            if (btnNuevoVencimiento != null)
                btnNuevoVencimiento(this, e);
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
            if (Page.IsPostBack)
            {
                //Recuperando Atributos
                recuperaAtributos();
            }   

            //Implementado seguridad de este recurso
            SAT_CL.Seguridad.Forma.AplicaSeguridadControlusuarioWeb(this, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
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
        /// Evento generado al Buscar los Vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Cargando vencimientos
            cargaHistorialVencimientos();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoEntidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraControlesBusqueda();
        }
        /// <summary>
        /// Evento generado al cambiar rango de Fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Habilitamos Controles
            txtFechaInicio.Enabled =
            txtFechaFin.Enabled = chkRangoFechas.Checked;
        }
        /// <summary>
        /// Evento click en botón nuevo vencimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoVencimientoHistorial_Click(object sender, EventArgs e)
        {
            if (btnNuevoVencimiento != null)
                OnbtnNuevoVencimiento(e);
        }

        #region Eventos GridView "Vencimientos"

        /// <summary>
        /// Evento producido al dar click sobre algún botón del GridView de Vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVencimientoOperacion_Click(object sender, EventArgs e)
        {
            //Si existen registros en el gridview
            if (gvVencimientos.DataKeys.Count > 0)
            {
                //Seleccionando fila del gv
                TSDK.ASP.Controles.SeleccionaFila(gvVencimientos, sender, "lnk", false);
                //Indicando vencimiento señalado
                this._id_vencimiento = Convert.ToInt32(gvVencimientos.SelectedDataKey.Value);
                //Determinando que botón fue pulsado
                switch (((LinkButton)sender).CommandName)
                {
                    case "Consultar":
                        if (lkbConsultar != null)
                            OnlkbConsultar(e);
                        break;
                    case "Terminar":
                        if (lkbTerminar != null)
                            OnlkbTerminar(e);
                        break;
                }

                //Inicializando indices de selección
                TSDK.ASP.Controles.InicializaIndices(gvVencimientos);
            }
        }
        /// <summary>
        /// Evento corting de gridview de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVencimientos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvVencimientos.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitVencimientos.DefaultView.Sort = lblOrdenadoVencimientoHistorial.Text;
                //Cambiando Ordenamiento
                lblOrdenadoVencimientoHistorial.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvVencimientos, this._mitVencimientos, e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVencimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvVencimientos, this._mitVencimientos, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVencimientoHistorial_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvVencimientos, this._mitVencimientos, Convert.ToInt32(ddlTamanoVencimientoHistorial.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarVencimientoHistorial_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitVencimientos, "");
        }
        /// <summary>
        /// Evento de enlace a datos de cada fila del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVencimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si no se ha solicitado ocultar columnas de acciones
            if (this._hab_consultar_terminar)
            {
                //Si es un fila de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;
                    //Validando existencia de datos requeridos para esta funcionalidad
                    if (row.Table.Columns.Contains("Estatus") && row.Table.Columns.Contains("IdPrioridad"))
                    {
                        //Determinando si el vencimiento se encuentra en estatus completado
                        if (row["Estatus"].ToString() == "Completado")
                        {
                            //obteniendo controles linkbutton para consulta y término de vencimiento
                            using (LinkButton lkbConsultar = (LinkButton)e.Row.FindControl("lkbConsultarVencimiento"),
                                            lkbTerminar = (LinkButton)e.Row.FindControl("lkbTerminarVencimiento"))
                            {
                                //Deshabilitando controles
                                lkbConsultar.Enabled = lkbTerminar.Enabled = false;
                            }
                        }
                        //Determinando prioridad del vencimiento
                        if ((TipoVencimiento.Prioridad)Convert.ToByte(row["IdPrioridad"]) == TipoVencimiento.Prioridad.Obligatorio)
                        {
                            //Cambiando color de forndo de la fila
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "TODOS", 1104);
            //Catálogo de Tipo de Vencimientos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 48, "TODOS", (int)this._tipo_aplicacion, "", 0, "");
            //Cargando Catalogo de Prioridad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPrioridad, "TODOS", 1103);
            //Cargando catálogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVencimientoHistorial, "", 18);
            //Cargando Tipo de Entidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEntidad, "", 1102);
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdRecurso"] = this._id_recurso;
            ViewState["TipoAplicacion"] = this._tipo_aplicacion;
            ViewState["mitVencimientos"] = this._mitVencimientos;
            ViewState["HabConsultarTerminar"] = this._hab_consultar_terminar;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["IdRecurso"] != null && ViewState["TipoAplicacion"] != null
                && ViewState["HabConsultarTerminar"] != null)
            {
                this._id_recurso = Convert.ToInt32(ViewState["IdRecurso"]);
                this._tipo_aplicacion = (TipoVencimiento.TipoAplicacion)ViewState["TipoAplicacion"];
                this._hab_consultar_terminar = Convert.ToBoolean(ViewState["HabConsultarTerminar"]);
                if (ViewState["mitVencimientos"] != null)
                    this._mitVencimientos = (DataTable)ViewState["mitVencimientos"];
            }
        }
        /// <summary>
        /// Realiza la configuración incial del control de usuario respecto al registro solicitado y las caracteristicas adicionales de visualización
        /// </summary>
        /// <param name="tipo_aplicacion">Tipo de Aplicación del vencimiento</param>
        /// <param name="id_recurso">Id de recurso (unidad/operador)</param>
        public void InicializaControl(TipoVencimiento.TipoAplicacion tipo_aplicacion, int id_recurso)
        {
            InicializaControl(tipo_aplicacion, id_recurso, true, (byte)Vencimiento.Estatus.Activo, 0, 0, false, true, DateTime.MinValue, DateTime.MinValue);
        }

        /// <summary>
        /// Realiza la configuración incial del control de usuario respecto al registro solicitado y las caracteristicas adicionales de visualización
        /// </summary>
        /// <param name="tipo_aplicacion">Tipo de Aplicación del vencimiento</param>
        /// <param name="id_recurso">Id de recurso (unidad/operador)</param>
        /// <param name="hab_consultar_terminar">True para mostrar columnas de opciones Consultar y Terminar, de lo contrario False</param>
        public void InicializaControl(TipoVencimiento.TipoAplicacion tipo_aplicacion, int id_recurso, bool hab_consultar_terminar)
        {
            InicializaControl(tipo_aplicacion, id_recurso, hab_consultar_terminar, (byte)Vencimiento.Estatus.Activo, 0, 0, false, true, DateTime.MinValue, DateTime.MinValue);
        }

        /// <summary>
        /// Realiza la configuración incial del control de usuario respecto al registro solicitado y las caracteristicas adicionales de visualización
        /// </summary>
        /// <param name="tipo_aplicacion">Tipo de Aplicación del vencimiento</param>
        /// <param name="id_recurso">Id de recurso (unidad/operador)</param>
        /// <param name="ver_terminados">True para mostrar vencimientos en estatus terminado</param>
        /// <param name="hab_consultar_terminar">True para mostrar columnas de opciones Consultar y Terminar, de lo contrario False</param>
        /// <param name="estatus">Estatus del vencimiento que se desea mostrar en el gv</param>
        /// <param name="id_tipo_vencimiento">Tipo de Vencimiento que se desea mostrar en el gv</param>
        /// <param name="id_prioridad">Id Prioridad del Vencimiento que se desea mostrar en el gv</param>
        /// <param name="rango_fechas">Valor que identifica si se requiere realizar la búsqueda por fecha,</param>
        /// <param name="fecha_inicio_vencimiento">tipo de fechas que se desea realizar la búsqueda en caso de ser false se realizara por fecha de fin de vencimiento,</param>
        /// <param name="fecha_fin">Fecha Inicio que se desea mostrar</param>
        /// <param name="fecha_inicio">Fecha fin que se desea  mostrar</param>
        public void InicializaControl(TipoVencimiento.TipoAplicacion tipo_aplicacion, int id_recurso, bool hab_consultar_terminar, byte id_estatus,
                                      int id_tipo_vencimiento, byte id_prioridad, bool rango_fechas, bool fecha_inicio_vencimiento, DateTime fecha_inicio, DateTime fecha_fin)
        { 
            //Asignando a atributos privados
            this._tipo_aplicacion = tipo_aplicacion;
            this._id_recurso = id_recurso;
            this._hab_consultar_terminar = hab_consultar_terminar;

            //Carga Catalogo
            cargaCatalogos();

            //Asignando Tipo de Entidad
            ddlTipoEntidad.SelectedValue = Convert.ToInt32(this._tipo_aplicacion).ToString();

            //Invocando Método de Configuración
            configuraControlesBusqueda();

            //Actualizando rótulo de entidad consultada
            switch (this._tipo_aplicacion)
            { 
                case TipoVencimiento.TipoAplicacion.Unidad:
                    //Instanciando unidad
                    using (Unidad u = new Unidad(this._id_recurso))
                    {
                        //Validando Unidad
                        if (u.habilitar)
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = string.Format("Vencimientos de Unidad '{0}'", u.numero_unidad);

                            //Asignando Valor
                            txtUnidad.Text = u.numero_unidad + " ID:" + u.id_unidad.ToString();
                        }                        
                        else
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = "Vencimientos de Unidad";

                            //Asignando Valor
                            txtUnidad.Text = "";
                        }
                    }
                    break;
                case TipoVencimiento.TipoAplicacion.Operador:
                    //Instanciando operador
                    using (Operador o = new Operador(this._id_recurso))
                    {
                        //Validando Operador
                        if (o.habilitar)
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = string.Format("Vencimientos de Operador '{0}'", o.nombre);

                            //Asignando Valor
                            txtOperador.Text = o.nombre + " ID:" + o.id_operador.ToString();
                        }
                        else
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = "Vencimientos de Operador";

                            //Asignando Valor
                            txtOperador.Text = "";
                        }
                    }
                    break;
                case TipoVencimiento.TipoAplicacion.Transportista:
                    //Instanciando transportista
                    using (CompaniaEmisorReceptor t = new CompaniaEmisorReceptor(this._id_recurso))
                    {
                        //Validando Transportista
                        if (t.habilitar)
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = string.Format("Vencimientos de Transportista '{0}'", t.nombre_corto);

                            //Asignando Valor
                            txtProveedor.Text = t.nombre + " ID:" + t.id_compania_emisor_receptor.ToString();
                        }
                        else
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = "Vencimientos de Transportista";

                            //Asignando Valor
                            txtProveedor.Text = "";
                        }
                    }
                    break;
                case TipoVencimiento.TipoAplicacion.Servicio:
                    //Instanciando transportista
                    using (SAT_CL.Documentacion.Servicio s = new SAT_CL.Documentacion.Servicio(this._id_recurso))
                    {
                        //Validando Servicio
                        if (s.habilitar)
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = string.Format("Vencimientos del Servicio '{0}'", s.no_servicio);

                            //Asignando Valor
                            txtServicio.Text = "Servicio No." + s.no_servicio + " ID:" + s.id_servicio.ToString();
                        }
                        else
                        {
                            //Asignando Valor al Encabezado
                            h2EntidadConsultada.InnerText = "Vencimientos del Servicio";

                            //Asignando Valor
                            txtServicio.Text = "";
                        }
                    }
                    break;
            }
            
            //Inicializamos Controles de acuerdo a los paramteros obtenidos
            ddlEstatus.SelectedValue = id_estatus.ToString();
            ddlTipo.SelectedValue = id_tipo_vencimiento.ToString();
            ddlPrioridad.SelectedValue = id_prioridad.ToString();

            //Inicializamos Controles
            txtFechaInicio.Enabled = txtFechaFin.Enabled  = chkRangoFechas.Checked = rango_fechas;
            rdbInicioVencimiento.Checked = fecha_inicio_vencimiento != true ? false : true;
            rdbFinVenciamiento.Checked = fecha_inicio_vencimiento != true ? true : false;
            //Validamos que exista Filtro de Unidades
            if (rango_fechas)
            {
                //Inicializamos Controles
                txtFechaInicio.Text = fecha_inicio.ToString("dd/MM/yyyy hh:mm");
                txtFechaFin.Text = fecha_fin.ToString("dd/MM/yyyy hh:mm");
            }
            else
            {
                //Obtenemos fecha de Default
                DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
                DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
                txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy") + " 00:00";
                txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";
            }

            //Realizando carga de vencimientos
            cargaHistorialVencimientos();
        }
        /// <summary>
        /// Carga los vencimientos aplicables a los criterios de búsqueda solicitados
        /// </summary>
        private void cargaHistorialVencimientos()
        {
            //Indicando que no hay selección de vencimiento aún
            this._id_vencimiento = 0;

            //Declaramos variables de Fechas 
            DateTime inicioVencimientoFechaInicio = DateTime.MinValue, inicioVencimientoFechaFin = DateTime.MinValue, finVencimientoFechaInicio = DateTime.MinValue, finVencimientoFechaFin = DateTime.MinValue;

            //De acuerdo al chek box de fechas 
            if (chkRangoFechas.Checked)
            {
                //De acuerdo al Tipo De Fecha
                if(rdbInicioVencimiento.Checked)
                {
                //Declaramos variables de Fechas de Inicio de Vencimiento
                inicioVencimientoFechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                inicioVencimientoFechaFin = Convert.ToDateTime(txtFechaFin.Text);
                }
                else
                {
                //Declaramos variables de Fechas de Inicio de Vencimiento
                finVencimientoFechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                finVencimientoFechaFin = Convert.ToDateTime(txtFechaFin.Text);
                }
            }

            //Obteniendo Recurso
            int idRecurso = ddlTipoEntidad.SelectedValue == "1" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1, "0")) :
                                (ddlTipoEntidad.SelectedValue == "2" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtOperador.Text, "ID:", 1, "0")) : 
                                (ddlTipoEntidad.SelectedValue == "3" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1, "0")) :
                                 Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtServicio.Text, "ID:", 1, "0"))));

            //Cargando vencimientos
            this._mitVencimientos = Vencimiento.CargaVencimientosRecurso((TipoVencimiento.TipoAplicacion)Convert.ToInt32(ddlTipoEntidad.SelectedValue), idRecurso, Convert.ToByte(ddlEstatus.SelectedValue),
                Convert.ToInt32(ddlTipo.SelectedValue), Convert.ToByte(ddlPrioridad.SelectedValue), inicioVencimientoFechaInicio, inicioVencimientoFechaFin, finVencimientoFechaInicio, finVencimientoFechaFin) ;
            //Si no hay registros
            if (this._mitVencimientos == null)
                TSDK.ASP.Controles.InicializaGridview(gvVencimientos);
            else
                //Mostrandolos en gridview
                TSDK.ASP.Controles.CargaGridView(gvVencimientos, this._mitVencimientos, "Id", lblOrdenadoVencimientoHistorial.Text, true, 1);

            //Si se ha solicitado ocultar columnas de consulta y término
            if (!this._hab_consultar_terminar)
                gvVencimientos.Columns[7].Visible = gvVencimientos.Columns[8].Visible = false;
        }
        /// <summary>
        /// Método encargado de Configurar los Controles de la Entidad de Busqueda
        /// </summary>
        private void configuraControlesBusqueda()
        {
            //Actualizando rótulo de entidad consultada
            switch ((TipoVencimiento.TipoAplicacion)Convert.ToInt32(ddlTipoEntidad.SelectedValue))
            {
                case TipoVencimiento.TipoAplicacion.Unidad:
                    //Configurando Controles
                    txtUnidad.Visible = true;
                    txtOperador.Visible =
                    txtProveedor.Visible = 
                    txtServicio.Visible = false;

                    //Asignando Valor al Encabezado
                    h2EntidadConsultada.InnerText = "Vencimientos de Unidad";
                    break;
                case TipoVencimiento.TipoAplicacion.Operador:
                    //Configurando Controles
                    txtUnidad.Visible = false;
                    txtOperador.Visible = true;
                    txtProveedor.Visible =
                    txtServicio.Visible = false;

                    //Asignando Valor al Encabezado
                    h2EntidadConsultada.InnerText = "Vencimientos de Operador";
                    break;
                case TipoVencimiento.TipoAplicacion.Transportista:
                    //Configurando Controles
                    txtUnidad.Visible = 
                    txtOperador.Visible = false;
                    txtProveedor.Visible = true;
                    txtServicio.Visible = false;

                    //Asignando Valor al Encabezado
                    h2EntidadConsultada.InnerText = "Vencimientos de Transportista";
                    break;
                case TipoVencimiento.TipoAplicacion.Servicio:
                    //Configurando Controles
                    txtUnidad.Visible =
                    txtOperador.Visible = 
                    txtProveedor.Visible = false;
                    txtServicio.Visible = true;

                    //Asignando Valor al Encabezado
                    h2EntidadConsultada.InnerText = "Vencimientos de Servicio";
                    break;
            }

            //Configurando Controles
            txtUnidad.Text =
            txtOperador.Text = 
            txtProveedor.Text = 
            txtServicio.Text = "";
        }

        #endregion
    }
}