using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucRequerimiento : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_compania;

        private int _id_servicio;

        public string ControlClientID;

        private DataTable _dtRequerimientos;

        /// <summary>
        /// Atributo Público encargado de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   ddlTabla.TabIndex =
                ddlRegistro.TabIndex =
                txtDescripcionReq.TabIndex =
                ddlTablaReq.TabIndex = 
                ddlFiltro.TabIndex =
                ddlCondicion.TabIndex =
                ddlValor.TabIndex =
                btnGuardar.TabIndex =
                btnCancelar.TabIndex = 
                ddlTamanoReqDisp.TabIndex =
                lnkExportarDisponibles.TabIndex =
                gvReqDisponibles.TabIndex = value;
            }
            get { return ddlTabla.TabIndex; }
        }
        /// <summary>
        /// Atributo Público encargado de la Habilitación de los Controles
        /// </summary>
        public bool Enabled
        {
            set
            {   ddlTabla.Enabled =
                ddlRegistro.Enabled =
                txtDescripcionReq.Enabled =
                ddlTablaReq.Enabled =
                ddlFiltro.Enabled =
                ddlCondicion.Enabled =
                ddlValor.Enabled = 
                btnCancelar.Enabled =
                btnGuardar.Enabled =
                ddlTamanoReqDisp.Enabled =
                lnkExportarDisponibles.Enabled =
                gvReqDisponibles.Enabled = value;
            }
            get { return ddlTabla.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento desencadenado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando si se produjo un PostBack
            if (!(Page.IsPostBack))
            {   //Cargando Catalogos de la Forma
                cargaCatalogos();
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoReqDisp, "", 18);
            }
            else//Recuperando Atributos del Control
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
        /// <summary>
        /// Evento descencadenado al cambiar el Indice del Control "ddlTabla"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTabla_SelectedIndexChanged(object sender, EventArgs e)
        {   //Actualizando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRegistro, 6, "", Convert.ToInt32(ddlTabla.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
            //Mostrando Enfoque en el Siguiente Control
            ddlRegistro.Focus();
        }
        /// <summary>
        /// Evento descencadenado al cambiar el Indice del Control "ddlTablaReq"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTablaReq_SelectedIndexChanged(object sender, EventArgs e)
        {   //Actualizando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFiltro, 5, "", Convert.ToInt32(ddlTablaReq.SelectedValue), "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlValor, 7, "", Convert.ToInt32(ddlFiltro.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
            //Mostrando Enfoque en el Siguiente Control
            ddlFiltro.Focus();
        }
        /// <summary>
        /// Evento descencadenado al cambiar el Indice del Control "ddlFiltro"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {   //Actualizando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlValor, 7, "", Convert.ToInt32(ddlFiltro.SelectedValue), "", 0, "");
            //Mostrando Enfoque en el Siguiente Control
            ddlValor.Focus();
        }

        #region Manejador de Eventos

        /// <summary>
        /// Evento "ClickSeleccionarRequerimiento"
        /// </summary>
        public event EventHandler ClickGuardarRequerimiento;
        /// <summary>
        /// Evento "ClickEliminarRequerimiento"
        /// </summary>
        public event EventHandler ClickEliminarRequerimiento;
        /// <summary>
        /// Método que manipula el Evento "ClickSeleccionarRequerimiento"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarRequerimiento(EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickGuardarRequerimiento != null)
                //Inicializando Evento
                ClickGuardarRequerimiento(this, e);
        }
        /// <summary>
        /// Método que manipula el Evento "ClickEliminarRequerimiento"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarRequerimiento(EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickEliminarRequerimiento != null)
                //Inicializando Evento
                ClickEliminarRequerimiento(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickGuardarRequerimiento != null)
                //Invocando al Manejador de Evento
                OnClickGuardarRequerimiento(e);
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Inicializando Indices
            Controles.InicializaIndices(gvReqDisponibles);
            //Limpiando Controles
            cargaCatalogos();
            txtDescripcionReq.Text = "";
        }

        #endregion

        #region Eventos GridView "Requerimientos Disponibles"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "gvReqDisponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoReqDisp_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando que existan Llaves
            if (gvReqDisponibles.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._dtRequerimientos.DefaultView.Sort = lblOrdenarReqDisp.Text;
                //Cambiando Tamaño de Registros
                Controles.CambiaTamañoPaginaGridView(gvReqDisponibles, this._dtRequerimientos, Convert.ToInt32(ddlTamanoReqDisp.SelectedValue), true, 1);
                //Limpiando Control
                txtDescripcionReq.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Orden de los Datos del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReqDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando que existan Llaves
            if (gvReqDisponibles.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._dtRequerimientos.DefaultView.Sort = lblOrdenarReqDisp.Text;
                //Cambiando Ordenamiento
                lblOrdenarReqDisp.Text = Controles.CambiaSortExpressionGridView(gvReqDisponibles, this._dtRequerimientos, e.SortExpression, true, 1);
                //Limpiando Control
                txtDescripcionReq.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Paginación del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReqDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando que existan Llaves
            if (gvReqDisponibles.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._dtRequerimientos.DefaultView.Sort = lblOrdenarReqDisp.Text;
                //Cambiando el Tamaño de la Página
                Controles.CambiaIndicePaginaGridView(gvReqDisponibles, this._dtRequerimientos, e.NewPageIndex, true, 1);
                //Limpiando Control
                txtDescripcionReq.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {   //Seleccionando Fila
            Controles.SeleccionaFila(gvReqDisponibles, sender, "lnk", false);
            //Validando que no este Vacio
            if (ClickEliminarRequerimiento != null)
                //Invocando Manejador de Evento
                OnClickEliminarRequerimiento(new EventArgs());
            return;
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {   //Validando que existan Llaves
            if (gvReqDisponibles.DataKeys.Count > 0)
            {   //Seleccionando Fila
                Controles.SeleccionaFila(gvReqDisponibles, sender, "lnk", false);
                //Instanciando Requerimiento del Servicio
                using (SAT_CL.Documentacion.ServicioRequerimiento serReq = new SAT_CL.Documentacion.ServicioRequerimiento(Convert.ToInt32(gvReqDisponibles.SelectedDataKey["Id"])))
                {   //Validando que el Registro sea Valido
                    if (serReq.id_requerimiento_servicio != 0)
                    {   //Asignando Valores
                        ddlTabla.SelectedValue = serReq.id_tabla.ToString();
                        ddlTablaReq.SelectedValue = serReq.id_tabla_objetivo.ToString();
                        txtDescripcionReq.Text = serReq.descripcion_requerimiento;
                        //Invocando Método de Carga de Catalogos
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRegistro, 6, "", Convert.ToInt32(ddlTabla.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
                        ddlRegistro.SelectedValue = serReq.id_registro.ToString();
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFiltro, 5, "", Convert.ToInt32(ddlTablaReq.SelectedValue), "", 0, "");
                        ddlFiltro.SelectedValue = serReq.id_campo_objetivo.ToString();
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlValor, 7, "", Convert.ToInt32(ddlFiltro.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
                        ddlCondicion.SelectedItem.Text = serReq.condicion;
                        ddlValor.SelectedValue = serReq.valor_objetivo;
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Excel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarDisponibles_Click(object sender, EventArgs e)
        {   //Validando que existan Llaves
            if (gvReqDisponibles.DataKeys.Count > 0)
                //Exportando Excel
                Controles.ExportaContenidoGridView(((DataTable)ViewState["DT"]));
        }

        #endregion

        #endregion


        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Ingresar las coincidencias de los Requerimientos Globales
        /// </summary>
        private void ingresaRequerimientosGlobal()
        {   /*
             * To Do: Crear Método que en base a los Requerimientos de la Tabla Global 
             *        cree los Requerimientos Ligados al Servicio
             */
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTabla, 4, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRegistro, 6, "", Convert.ToInt32(ddlTabla.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTablaReq, 4, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFiltro, 5, "", Convert.ToInt32(ddlTablaReq.SelectedValue), "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlValor, 7, "", Convert.ToInt32(ddlFiltro.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlCondicion, "", 14);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Requerimientos asignados a un Servicio
        /// </summary>
        private void cargaRequerimientosServicio()
        {   //Obteniendo Requerimientos
            using (DataTable dtReqOcupados = SAT_CL.Documentacion.ServicioRequerimiento.ObtieneRequerimientosServicio(this._id_servicio))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtReqOcupados))
                {   //Cargando GridView
                    Controles.CargaGridView(gvReqDisponibles, dtReqOcupados, "Id", "", true, 1);
                    //Añadiendo a Vista
                    this._dtRequerimientos = dtReqOcupados;
                    //Inicializando Indices
                    Controles.InicializaIndices(gvReqDisponibles);
                }
                else
                {   //Inicializando GridView
                    Controles.InicializaGridview(gvReqDisponibles);
                    //Añadiendo a Vista
                    this._dtRequerimientos = null;
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {   //Asignando Atributos
            ViewState["idCompania"] = this._id_compania;
            ViewState["idServicio"] = this._id_servicio;
            ViewState["DT"] = this._dtRequerimientos;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   //Validando que exista el Valor de la Compania
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)
                //Asignando Valor
                this._id_compania = Convert.ToInt32(ViewState["idCompania"]);
            //Validando que exista el Valor del Servicio
            if (Convert.ToInt32(ViewState["idServicio"]) != 0)
                //Asignando Valor
                this._id_servicio = Convert.ToInt32(ViewState["idServicio"]);
            //Validando que exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                //Asignando Valor
                this._dtRequerimientos = (DataTable)ViewState["DT"];
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_compania">Id de Compania</param>
        /// <param name="id_servicio">Id de Servicio</param>
        public void InicializaControl(int id_compania, int id_servicio)
        {   //Asignando Valor a los Atributos
            this._id_compania = id_compania;
            this._id_servicio = id_servicio;
            //Cargando de Catalogos
            cargaCatalogos();
            txtDescripcionReq.Text = "";
            //Ingresando Catalogos
            ingresaRequerimientosGlobal();
            //Cargando Requerimientos
            cargaRequerimientosServicio();
        }
        /// <summary>
        /// Método Público encargado de Seleccionar el Requerimiento a Añadir
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaRequerimiento()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Valdiando que exista un Registro Seleccionado
            if (gvReqDisponibles.SelectedIndex != -1)
            {   //Instanciando Requerimiento
                using (SAT_CL.Documentacion.ServicioRequerimiento serReq = new SAT_CL.Documentacion.ServicioRequerimiento(Convert.ToInt32(gvReqDisponibles.SelectedDataKey["Id"])))
                {   //Validando que exista el Requerimiento
                    if (serReq.id_requerimiento_servicio != 0)
                        //Editando Requerimeinto del Servicio
                        result = serReq.EditaServicioRequerimiento(serReq.id_requerimiento, this._id_servicio, Convert.ToInt32(ddlTabla.SelectedValue),
                                                        Convert.ToInt32(ddlRegistro.SelectedValue), txtDescripcionReq.Text.ToUpper(), Convert.ToInt32(ddlTablaReq.SelectedValue),
                                                        Convert.ToInt32(ddlFiltro.SelectedValue), ddlValor.SelectedValue, ddlCondicion.SelectedItem.ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            else
            {   //Insertando Requerimiento del Servicio
                result = SAT_CL.Documentacion.ServicioRequerimiento.InsertaServicioRequerimiento(0, this._id_servicio, Convert.ToInt32(ddlTabla.SelectedValue),
                                                        Convert.ToInt32(ddlRegistro.SelectedValue), txtDescripcionReq.Text.ToUpper(), Convert.ToInt32(ddlTablaReq.SelectedValue),
                                                        Convert.ToInt32(ddlFiltro.SelectedValue), ddlValor.SelectedValue, ddlCondicion.SelectedItem.ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Validando que la Operacion sea Exitosa
            if (result.OperacionExitosa)
            {    //Invocando Método que Inicializa el Control
                InicializaControl(this._id_compania, this._id_servicio);
                //Mostrando enfoque en el primer control
                txtDescripcionReq.Focus();
            }
            //Mostrando Mensaje de Operacion
            lblError.Text = result.Mensaje;
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Eliminar el Requerimiento del Servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminarRequerimientoServicio()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que existan registros
            if (gvReqDisponibles.DataKeys.Count > 0)
            {   //Instanciando el Requerimiento Ligado al Servicio
                using (SAT_CL.Documentacion.ServicioRequerimiento serReq = new SAT_CL.Documentacion.ServicioRequerimiento(Convert.ToInt32(gvReqDisponibles.SelectedDataKey["Id"])))
                {   //Validando que exista un registro
                    if (serReq.id_requerimiento_servicio != 0)
                        //Deshabilitando Requerimiento del Servicio
                        result = serReq.DeshabilitaServicioRequerimiento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                //Validando que la Operación haya Sido exitosa
                if (result.OperacionExitosa)
                    //Inicializando Control
                    InicializaControl(this._id_compania, this._id_servicio);
            }
            else
            {   //Instanciando Error
                result = new RetornoOperacion("No se encontraron registros");
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvReqDisponibles);
            }
            //Mostrando Mensaje de la Operacion
            lblError.Text = result.Mensaje;
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}