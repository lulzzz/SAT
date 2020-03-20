using SAT_CL.Seguridad;
using System;
using System.Data;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Web.UI.WebControls;
namespace SAT.UserControls
{
    public partial class ActividadOrdenTrabajo : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id Orden Trabajo Actividad
        /// </summary>
        private int _id_orden_trabajo_actividad;
        /// <summary>
        /// Id Orden Trabajo
        /// </summary>
        private int _id_orden_trabajo;
        /// <summary>
        /// Id Falla
        /// </summary>
        private int _id_falla;
        /// <summary>
        /// DataTable de Actividades
        /// </summary>
        private DataTable _dtActividades;
        /// <summary>
        /// DataTable de ActividadesAsignadas
        /// </summary>
        private DataTable _dtActividadesAsignadas;
        /// <summary>
        /// Declaración de Evento ClickGuardar
        /// </summary>
        public event EventHandler ClickRegistrar;
        /// <summary>
        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Tab
                ddlArea.TabIndex = 
                ddlSubArea.TabIndex =  value;
            }
            get { return btnBuscar.TabIndex; }
        }
        /// <summary>
        /// Atributo encargado de Almacenar el Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Eventos
        /// <summary>
        /// Manipula Evento ClickRegistrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickRegistrar(EventArgs e)
        {
            if (ClickRegistrar != null)
                ClickRegistrar(this, e);

        }

        /// Evento disparado al presionar el boton Registrar Actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnRegistrar(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvActividades, sender, "lnk", false);
            //Validando que exista un Evento
            if (ClickRegistrar != null)
                OnClickRegistrar(new EventArgs());
        }

        /// <summary>
        /// Evento generado al Cancelar una Actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            inicializaValores();
        }

        /// <summary>
        /// Evento generado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            
                //Inicializando Forma
                inicializaForma();
            else
                //Recupera Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento producido previo a la carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Almacenando valores viewstate
            asignaAtributos();
        }


        #endregion 

        #region Eventos GridView 

        /// <summary>
        /// Evento generado al Buscar una Actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Método encargado de Buscar una Actividad
            cargaActividades();
        }
        /// <summary>
        /// Evento disparado al cambiar el Tamaño del GridView Actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewActividades_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividades.DataKeys.Count > 0) 
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvActividades, this._dtActividades, Convert.ToInt32(ddlTamañoGridViewActividades.SelectedValue), true, 3);
            }
        }

        /// <summary>
        /// Evento disparado cambiar el criterio de Ordenamiento de GridView Actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_OnSorting(object sender, GridViewSortEventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividades.DataKeys.Count > 0)
            {   //Asignando al Label el Criterio de Ordenamiento
                lblCriterioGridViewActividades.Text = Controles.CambiaSortExpressionGridView(gvActividades, this._dtActividades, e.SortExpression, true, 3);
            }
        }
        /// <summary>
        /// Evento disparado al cambiar el Indice de pagina del GridView Actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividades.DataKeys.Count > 0)
            {   //Cambiando el Indice de Pagina
                Controles.CambiaIndicePaginaGridView(gvActividades, this._dtActividades, e.NewPageIndex, true, 3);

            }
        }
        /// <summary>
        /// Evento disparado al cambiar el Tamaño del GridView Actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewgvActividades_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividades.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvActividades, this._dtActividades, Convert.ToInt32(ddlTamañoGridViewActividades.SelectedValue), true, 3);
            }
        }
        /// <summary>
        /// Eventyo generado al exportar las actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelgvActividades_Click(object sender, EventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividades.DataKeys.Count > 0)
            {
                //Exporta el contenido de la tabla cargada en el gridview
                Controles.ExportaContenidoGridView(this._dtActividades);
            }
        }

        /// <summary>
        /// Evento Generado al Cambiar el Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cargando Catalogo Sub Area
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubArea, "Todos", 1122, Convert.ToInt32(ddlArea.SelectedValue));
        }

        /// <summary>
        /// Evento generado al cambiar la Selección de Tamaño de Actividades  Asignadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanogvActividadesAsignadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividadesAsignadas.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvActividadesAsignadas, this._dtActividadesAsignadas, Convert.ToInt32(ddlTamanogvActividadesAsignadas.SelectedValue), true, 4);
            }
        }

        /// <summary>
        ///  Evento generado al Exportar en Excel de Actividades Asignadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportargvActividadesAsignadas_Click(object sender, EventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividadesAsignadas.DataKeys.Count > 0)
            {
                //Exporta el contenido de la tabla cargada en el gridview
                Controles.ExportaContenidoGridView(this._dtActividadesAsignadas);
            }
        }

        /// <summary>
        ///  Evento disparado al cambiar el Indice de pagina del GridView Actividades Asignadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividadesAsignadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvActividadesAsignadas que existan registro en el GridView
            if (gvActividadesAsignadas.DataKeys.Count > 0)
            {   //Cambiando el Indice de Pagina
                Controles.CambiaIndicePaginaGridView(gvActividadesAsignadas, this._dtActividadesAsignadas, e.NewPageIndex, true, 4);

            }
        }

        /// <summary>
        /// Evento disparado cambiar el criterio de Ordenamiento de GridView Actividades Asignadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividadesAsignadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvActividadesAsignadas.DataKeys.Count > 0)
            {   //Asignando al Label el Criterio de Ordenamiento
                lblOrdenadogvActividadesAsignadas.Text = Controles.CambiaSortExpressionGridView(gvActividadesAsignadas, this._dtActividadesAsignadas, e.SortExpression, true, 4);
            }
        }


        #endregion
     

        #region Métodos
        
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo de Fallas
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFallasActividad, 92, "Ninguno", this._id_orden_trabajo, "", 0, "");
            //Cargando Catalogo Area
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlArea, "Todos", 1121);
            //Cargando Catalogo Sub Area
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubArea, "Todos", 1122, Convert.ToInt32(ddlArea.SelectedValue));
            //Cargando Catalogo Tamaño Actividades
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewActividades, "", 56);
            //Cargando Catalogo Tamaño Actividades Asignadas
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanogvActividadesAsignadas, "", 56);
        }
        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdOrdenTrabajoActividad"]) != 0)
                this._id_orden_trabajo_actividad = Convert.ToInt32(ViewState["IdOrdenTrabajoActividad"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdOrdenTrabajo"]) != 0)
                this._id_orden_trabajo = Convert.ToInt32(ViewState["IdOrdenTrabajo"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdFalla"]) != 0)
                this._id_falla = Convert.ToInt32(ViewState["IdFalla"]);
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DTActividades"]))
                //Asignando Valor al Atributo
                this._dtActividades = (DataTable)ViewState["DTActividades"];
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DTActividadesAsignadas"]))
                //Asignando Valor al Atributo
                this._dtActividadesAsignadas = (DataTable)ViewState["DTActividadesAsignadas"];
        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdOrdenTrabajoActividad"] = this._id_orden_trabajo_actividad;
            ViewState["IdOrdenTrabajo"] = this._id_orden_trabajo;
            ViewState["IdFalla"] = this._id_falla;
            ViewState["DTActividades"] = this._dtActividades;
            ViewState["DTActividadesAsignadas"] = this._dtActividadesAsignadas;
           
        }
        /// <summary>
        /// Inicializa Valores Forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga Catalogos
            cargaCatalogos();
        }
        /// <summary>
        /// Inicializa Valores
        /// </summary>
        private void inicializaValores()
        {
                //Limpiando Valores
                ddlArea.SelectedValue =
                ddlSubArea.SelectedValue = "0";
                ddlFallasActividad.SelectedValue = this._id_falla.ToString();
               //Inicializa Grid View Actividades
                Controles.InicializaGridview(gvActividades);
            
        }
        /// <summary>
        /// Inicializa los Controles de la Orden de Trabajo Actividad
        /// </summary>
        /// <param name="id_orden_trabajo_actividad">Id Orden Trabajo Actividad</param>
        /// <param name="id_orden_trabajo">Id Orden de Trabajo</param>
        /// <param name="id_falla">Id Falla</param>
        public void InicializaControl(int id_orden_trabajo_actividad, int id_orden_trabajo, int id_falla)
        {
            //Asignando Atributos
            this._id_orden_trabajo_actividad = id_orden_trabajo_actividad;
            this._id_falla = id_falla;
            this._id_orden_trabajo = id_orden_trabajo;
            //Si la Orden de Trabajo es 0
            if (id_orden_trabajo == 0)
            {
                //Instanciamos Falla
                using (SAT_CL.Mantenimiento.OrdenTrabajoFalla objOrdenTrabajoFalla = new SAT_CL.Mantenimiento.OrdenTrabajoFalla(id_falla))
                {
                    //Asignamos Orden de Trabajo
                    this._id_orden_trabajo = objOrdenTrabajoFalla.id_orden_trabajo;
                }
            }
            //Carga Catalogos
            cargaCatalogos();
            //Inicializa Valores
            inicializaValores();
            //Carga Actividades Asignadas
            cargaActividadesAsignadas();

        }

        /// <summary>
        /// Método encargado de Cargar las Actividades
        /// </summary>
        private void cargaActividades()
        {
            //Instanciamos Orden de Trabajo para Obtener la Actividad
            using (SAT_CL.Mantenimiento.OrdenTrabajo objOrdenTrabajo = new SAT_CL.Mantenimiento.OrdenTrabajo(this._id_orden_trabajo))
            {
                //Declaramos Variables para almacenar el Tipo de Unidad Y Subtipo de Unidad
                int id_tipo_unidad =objOrdenTrabajo.id_tipo_unidad;
                byte id_subtipo_unidad = objOrdenTrabajo.id_subtipo_unidad;

                //Validamos que la Unidad no sea Externa
                if(objOrdenTrabajo.bit_unidad_externa == false)
                {
                    //Intsnaciamos Unidad
                    using(SAT_CL.Global.Unidad objUnidad = new SAT_CL.Global.Unidad(objOrdenTrabajo.id_unidad))
                    {
                        //Asignamos Valores
                        id_tipo_unidad = objUnidad.id_tipo_unidad;
                        id_subtipo_unidad = objUnidad.id_sub_tipo_unidad;
                    }
                }

                //Carga las Actividades de acuerdo a los filtros correspondientes
                using (DataTable mit = SAT_CL.Mantenimiento.OrdenTrabajoActividad.CargaActividades(
                                      Convert.ToInt32(ddlArea.SelectedValue), Convert.ToInt32(ddlSubArea.SelectedValue), id_tipo_unidad,
                                       id_subtipo_unidad, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvActividades, mit, "Id", "", true, 3);

                        //Añadiendo Tabla  
                        this._dtActividades = mit;
                    }

                    else
                    {
                        //Eliminando Tabla de Session
                        this._dtActividades = null;
                        Controles.InicializaGridview(gvActividades);
                    }
                }
            }
        }

        /// <summary>
        /// Método encargado de Cargar las Actividades ligadas a la Orden de Trabajo
        /// </summary>
        private void cargaActividadesAsignadas()
        {
            //Carga las Actividades de acuerdo a los filtros correspondientes
            using (DataTable mit = SAT_CL.Mantenimiento.OrdenTrabajoActividad.CargaResumenActividadesAsignadas(this._id_orden_trabajo))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvActividadesAsignadas, mit, "Id", "", true, 4);

                    //Añadiendo Tabla  
                    this._dtActividadesAsignadas = mit;
                }

                else
                {
                    //Eliminando Tabla de Session
                    this._dtActividadesAsignadas = null;
                    Controles.InicializaGridview(gvActividadesAsignadas);
                }
            }
        }
    
        /// <summary>
        /// Método encargado de Registrar la Orden de Trabajo de la Actividad
        /// </summary>
        public RetornoOperacion RegistraOtrdenTrabajoActividad()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que se encuentre la Actividad
            if (gvActividades.DataKeys.Count > 0)
            {
                //Insertamos Orde Trabajo Actividad
                resultado = SAT_CL.Mantenimiento.OrdenTrabajoActividad.InsertaOrdenActividad(Convert.ToInt32(gvActividades.SelectedValue), this._id_orden_trabajo,  Convert.ToInt32(ddlFallasActividad.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Control
                    this.InicializaControl(this._id_orden_trabajo_actividad, this._id_orden_trabajo, this._id_falla);

                    //Cargamos Actividades Asignadas
                    cargaActividadesAsignadas();
                    //Cargamos Actividaes
                    cargaActividades();
                }
            }
                
               

            //Devolvemos resultado
            return resultado;
        }
       
        #endregion

       

        
    }
}