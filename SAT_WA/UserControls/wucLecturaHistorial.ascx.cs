using SAT_CL.Global;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.ASP;
namespace SAT.UserControls
{
    public partial class wucLecturaHistorial : System.Web.UI.UserControl
    {
        #region Atributos

        /// Id de Lectura seleccionado
        /// </summary>
        private int _id_lectura;
        /// <summary>
        /// Id de la Unidad
        /// </summary>
        private int _id_unidad;
        /// <summary>
        /// Tabla con las Lecturas encontradas
        /// </summary>
        private DataTable _mitLecturas;
        /// <summary>
        /// Indica si se debe permitir la visualización de las columnas con los controles consultar 
        /// </summary>
        private bool _hab_consultar;
        /// <summary>
        /// Id de un Operador
        /// </summary>
        private int _id_operador;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {
                txtFechaInicio.TabIndex =
                txtFechaFin.TabIndex =
                ddlTamanoLecturaHistorial.TabIndex =
                lkbExportarLecturaHistorial.TabIndex =
                gvLecturaHistorial.TabIndex = value;
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
                ddlTamanoLecturaHistorial.Enabled =
                lkbExportarLecturaHistorial.Enabled =
                gvLecturaHistorial.Enabled = value;
            }
            get { return this.txtFechaInicio.Enabled; }
        }

        /// <summary>
        /// Obtiene el Id de Lectura seleccionado actualmente
        /// </summary>
        public int id_lectura { get { return this._id_lectura; } }
        /// <summary>
        /// Obtiene el Id de la unidad
        /// </summary>
        public int id_unidad  { get { return _id_unidad; } }
        /// <summary>
        /// Obtiene el Id del Operador
        /// </summary>
        public int id_operador { get { return this._id_operador; } }
        #endregion

        #region Manejadores de Eventos

        public event EventHandler lkbConsultar;
        public event EventHandler btnNuevaLectura;
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
        /// Evento Click en Botón nuevo lectura
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnbtnNuevoLectura(EventArgs e)
        {
            //Si hay manejador asignado
            if (btnNuevaLectura != null)
                btnNuevaLectura(this, e);
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
        /// Evento producido al dar click sobre algún botón del GridView de Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLecturaHistorialClick(object sender, EventArgs e)
        {
            //Si existen registros en el gridview
            if (gvLecturaHistorial.DataKeys.Count > 0)
            {
                //Seleccionando fila del gv
                TSDK.ASP.Controles.SeleccionaFila(gvLecturaHistorial, sender, "lnk", false);
                //Indicando la bitácora señalado
                this._id_lectura = Convert.ToInt32(gvLecturaHistorial.SelectedDataKey.Value);
                //Determinando que botón fue pulsado
                switch (((LinkButton)sender).CommandName)
                {
                    case "Consultar":
                        if (lkbConsultar != null)
                            OnlkbConsultar(e);
                        break;
                }

                //Inicializando indices de selección
                TSDK.ASP.Controles.InicializaIndices(gvLecturaHistorial);
            }
        } 
        /// <summary>
        /// Evento corting de gridview de Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLecturaHistorial_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvLecturaHistorial.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitLecturas.DefaultView.Sort = lblOrdenadoLecturaHistorial.Text;
                //Cambiando Ordenamiento
                lblOrdenadoLecturaHistorial.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvLecturaHistorial, this._mitLecturas, e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Bitácora Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLecturaHistorial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvLecturaHistorial, this._mitLecturas, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoLecturaHistorial_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvLecturaHistorial, this._mitLecturas, Convert.ToInt32(ddlTamanoLecturaHistorial.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Bitácora Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarLecturaHistorial_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitLecturas, "");
        }
        /// <summary>
        /// Evento click en botón nuevo Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoLecturaHistorial_Click(object sender, EventArgs e)
        {
            if (btnNuevoLectura != null)
                OnbtnNuevoLectura(e);
        }
        /// <summary>
        /// Evento que permite la busqueda de Lecturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBusca_Click(object sender, EventArgs e)
        {
            //Invoca al método carga Lecturas
            cargaHistorialLecturas();
        }
        /// <summary>
        /// Evento de enlace a datos de cada fila del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLecturaHistorial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si no se ha solicitado ocultar columnas de acciones
            if (!this._hab_consultar)
            {
                //Si es un fila de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //obteniendo controles linkbutton para consulta y término de Bitácora Monitoreo
                    using (LinkButton lkbConsultar = (LinkButton)e.Row.FindControl("lkbConsultarLectura"))
                    {
                        //Deshabilitando controles
                        lkbConsultar.Enabled = false;
                    }
                }
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Método que permite asignar los valores a los controles DropDownList.
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando catálogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoLecturaHistorial, "", 18);
            SAT_CL.Seguridad.Forma.AplicaSeguridadControlusuarioWeb(this, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        }

        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdLectura"] = this._id_lectura;
            ViewState["IdUnidad"] = this._id_unidad;
            ViewState["mitLectura"] = this._mitLecturas;
            ViewState["HabConsultar"] = this._hab_consultar;
            ViewState["IdOperador"] = this._id_operador;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["IdUnidad"] != null && ViewState["HabConsultar"] != null )
            {
                this._id_lectura = Convert.ToInt32(ViewState["IdLectura"]);
                this._id_unidad = Convert.ToInt32(ViewState["IdUnidad"]);
                this._hab_consultar = Convert.ToBoolean(ViewState["HabConsultar"]);
                if (ViewState["mitLectura"] != null)
                    this._mitLecturas = (DataTable)ViewState["mitLectura"];
                this._id_operador = Convert.ToInt32(ViewState["IdOperador"]);
            }

        }

        /// <summary>
        /// Inicializa Control de Usuario
        /// </summary>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="hab_consultar">Habiliotar la Consulta de Lec turas</param>
        public void InicializaControl(int id_unidad, bool hab_consultar)
        {
            //Asignando a atributos privados
            this._id_unidad = id_unidad;
            this._hab_consultar = hab_consultar;
            //Método que inicializa los controles del WUC
            cargaValores();
        }
        /// <summary>
        /// Inicializa Control de Usuario
        /// </summary>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="hab_consultar">Habiliotar la Consulta de Lec turas</param>
        public void InicializaControl(int id_unidad, bool hab_consultar, int id_operador)
        {
            //Asignando a atributos privados
            this._id_unidad = id_unidad;
            this._hab_consultar = hab_consultar;
            this._id_operador = id_operador;
            //Método que inicializa los controles del WUC
            cargaValores();
        }
        /// <summary>
        /// Método que asigna valores iniciales al control de Usuario Lectura Historial
        /// </summary>
        private void cargaValores()
        {
            //Carga catálogo 
            cargaCatalogos();
            //Asigna el valor de inicio a los controles txtFechaInicio (menos un dia) y txtFechaFin el valor de la fecha actual
            txtFechaInicio.Text = Convert.ToString(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddHours(-24).ToString("dd/MM/yyyy HH:mm"));
            txtFechaFin.Text = Convert.ToString(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMinutes(5).ToString("dd/MM/yyyy HH:mm"));

            //Realizando carga de Lectura
            cargaHistorialLecturas();
        }
        /// <summary>
        /// Carga las Lecturas aplicables a los criterios de búsqueda solicitados
        /// </summary>
        private void cargaHistorialLecturas()
        {
            //Creación del objeto retono
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al método valida Fecha
            retorno = validaFechas();
            //Valida si las fechas.
            if (retorno.OperacionExitosa)
            {
                //Indicando que no hay selección de bitácora aún
                this._id_lectura = 0;
                //Cargando Lecturas
                this._mitLecturas = SAT_CL.Mantenimiento.Lectura.CargaHistorialLectura(this._id_unidad, Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text));
                //Si no hay registros
                if (this._mitLecturas == null)
                    TSDK.ASP.Controles.InicializaGridview(gvLecturaHistorial);
                else
                    //Mostrandolos en gridview
                    TSDK.ASP.Controles.CargaGridView(gvLecturaHistorial, this._mitLecturas, "Id", lblOrdenadoLecturaHistorial.Text, true, 1);

                //Si se ha solicitado ocultar columnas de consulta y término
                if (!this._hab_consultar)
                    gvLecturaHistorial.Columns[6].Visible = false;
            }
            //Encaso de existir Error
            if (!retorno.OperacionExitosa)
            {
                //Mostrando resultado
                ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
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

        #endregion

    }
}