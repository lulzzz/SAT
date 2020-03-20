using System;
using SAT_CL.Global;
using TSDK.Base;
using SAT_CL.Documentacion;

namespace SAT.UserControls
{
    public partial class Clasificacion : System.Web.UI.UserControl
    {
        #region Atributos

        private SAT_CL.Global.Clasificacion objClasificacion;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Clasificación
        /// </summary>
        public int idClasificacion { get { return objClasificacion.id_clasificacion; } }

        private int _id_compania;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compañia
        /// </summary>
        public int idCompania { get { return this._id_compania; } }

        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Tabla
        /// </summary>
        public int idTabla { get { return this._id_tabla; } }

        private int _id_registro;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Registro
        /// </summary>
        public int idRegistro { get { return this._id_registro; } }

        /// <summary>
        /// Obtiene el Valor de Tabulación del Control
        /// </summary>
        public short TabIndex
        {   //Asignando Valor de tabulación
            set
            {
                ddlFlota.TabIndex =
                ddlRegion.TabIndex =
                ddlTipoServicio.TabIndex =
                ddlUbicacionTerminal.TabIndex =
                ddlAlcanceServicio.TabIndex =
                ddlDetalleNegocio.TabIndex =
                ddlClasificacion1.TabIndex =
                ddlClasificacion2.TabIndex =
                btnGuardar.TabIndex =
                btnCancelar.TabIndex = value;
            }
            //Devolviendo Valor de tabulacion del Primer Control
            get { return ddlFlota.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor de habilitación de los elementos del control
        /// </summary>
        public bool Enabled
        {
            set
            {
                ddlFlota.Enabled =
                ddlRegion.Enabled =
                ddlTipoServicio.Enabled =
                ddlUbicacionTerminal.Enabled =
                ddlAlcanceServicio.Enabled =
                ddlDetalleNegocio.Enabled =
                ddlClasificacion1.Enabled =
                ddlClasificacion2.Enabled =
                btnGuardar.Enabled =
                btnCancelar.Enabled = value;
            }
            get { return this.ddlFlota.Enabled; }
        }

        #endregion

        #region Manejadores de Evento Públicos

        /// <summary>
        /// Declaración del evento ClickGuardar, cuyo delegado será OnClickGuardar
        /// </summary>
        public event EventHandler ClickGuardar;
        /// <summary>
        /// Declaración del evento ClickCancelar, cuyo delegado será OnClickCancelar
        /// </summary>
        public event EventHandler ClickCancelar;

        /// <summary>
        /// Método que manipula al Evento OnClickGuardar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardar(EventArgs e)
        {   //Validando que el evento no este vacio
            if (ClickGuardar != null)
                //Invocación al delegado
                ClickGuardar(this, e);
        }
        /// <summary>
        /// Método que manipula al Evento OnClickCancelar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCancelar(EventArgs e)
        {   //Validando que el evento no este vacio
            if (ClickCancelar != null)
                //Invocación al delegado
                ClickCancelar(this, e);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento disparado al producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando que se haya producido un PostBack
            if (!(Page.IsPostBack))
            {   //cargando catalogos
                asignaAtributos();
            }
            else
                recuperaAtributos();
        }
        /// <summary>
        /// Evento producido previo a la renderización del control web
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            asignaAtributos();
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Validando que el evento "Guardar" este Vacio
            if (ClickGuardar != null)
                OnClickGuardar(new EventArgs());
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Validando que el evento "Cancelar" este Vacio
            if (ClickCancelar != null)
                OnClickCancelar(new EventArgs());
        }

        /// Evento generado al dar click en Bitácora 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacora(object sender, EventArgs e)
        {
            //Validamos que existan Registros
            if (this.objClasificacion.id_clasificacion > 0)
            {
                //Mostramos Bitácora
                inicializaBitacora(this.objClasificacion.id_clasificacion.ToString(), "3", "Bitácora Clasificación");
            }
        }
        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {   //Asignando Atributos ViewState
            ViewState["idClasificacion"] = this.objClasificacion != null ? this.objClasificacion.id_clasificacion : 0;
            ViewState["idCompania"] = this._id_compania;
            ViewState["idTabla"] = this._id_tabla;
            ViewState["idRegistro"] = this._id_registro;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   //Recuperando Valores
            if (ViewState["idClasificacion"] != null)
                this.objClasificacion = new SAT_CL.Global.Clasificacion(Convert.ToInt32(ViewState["idClasificacion"]));
            if (ViewState["idCompania"] != null)
                this._id_compania = Convert.ToInt32(ViewState["idCompania"]);
            if (ViewState["idTabla"] != null)
                this._id_tabla = Convert.ToInt32(ViewState["idTabla"]);
            if (ViewState["idRegistro"] != null)
                this._id_registro = Convert.ToInt32(ViewState["idRegistro"]);
        }
        
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos del Control
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando Flotas
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFlota, 3, "Ninguna", idCompania, "", 1, "");
            /**Catalogos Faltantes**/
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRegion, 3, "Ninguna", idCompania, "", 2, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUbicacionTerminal, 9, "Ninguna", idCompania, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 3, "Ninguna", idCompania, "", 4, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlAlcanceServicio, 3, "Ninguna", idCompania, "", 5, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDetalleNegocio, 3, "Ninguna", idCompania, "", 6, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlClasificacion1, 3, "Ninguna", idCompania, "", 7, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlClasificacion2, 3, "Ninguna", idCompania, "", 8, "");
        }

        /// <summary>
        /// Configura la ventana de visualización de bitácora del registro solicitado
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucParada.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="idTabla">Id de la Tabla</param>
        /// <param name="idRegistro">Id del Registro</param>
        /// <param name="idCompania">Id de Compania</param>
        public void InicializaControl(int idTabla, int idRegistro, int idCompania)
        {
            //Instanciando clasificación del registro solicitado
            this.objClasificacion = new SAT_CL.Global.Clasificacion(idTabla, idRegistro, 0);
            //Almacenando compañía configurada
            this._id_compania = idCompania;
            //Tabla y registro
            this._id_tabla = idTabla;
            this._id_registro = idRegistro;

            //Cargando Catalogos
            cargaCatalogos();

            //Si el registro ya existe
            if (this.objClasificacion.id_clasificacion > 0)
            {
                //Asignando Valores
                lblId.Text = objClasificacion.id_clasificacion.ToString();
                ddlFlota.SelectedValue = objClasificacion.id_flota.ToString();
                ddlRegion.SelectedValue = objClasificacion.id_region.ToString();
                ddlTipoServicio.SelectedValue = objClasificacion.id_tipo_servicio.ToString();
                ddlUbicacionTerminal.SelectedValue = objClasificacion.id_ubicacion_terminal.ToString();
                ddlAlcanceServicio.SelectedValue = objClasificacion.id_alcance_servicio.ToString();
                ddlDetalleNegocio.SelectedValue = objClasificacion.id_detalle_negocio.ToString();
                ddlClasificacion1.SelectedValue = objClasificacion.id_clasificacion1.ToString();
                ddlClasificacion2.SelectedValue = objClasificacion.id_clasificacion2.ToString();
            }
            //Si no hay clasificación regstrada aún
            else
                //Asignando Valores
                lblId.Text = "Por Asignar";
            //Limpiando Mensaje
            lblError.Text = "";
        }
        /// <summary>
        /// Guarda los cambios realizados en la clasificación
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaCambiosClasificacion()
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();            
            //Validando que exista un Registro
            if(objClasificacion.id_clasificacion != 0)
            {   //Actualizando los Datos
                result = objClasificacion.EditaClasificacion(this._id_tabla, this._id_registro, objClasificacion.id_tipo, Convert.ToInt32(ddlFlota.SelectedValue),
                                                            Convert.ToInt32(ddlRegion.SelectedValue), Convert.ToInt32(ddlUbicacionTerminal.SelectedValue),
                                                            Convert.ToInt32(ddlTipoServicio.SelectedValue), Convert.ToInt32(ddlAlcanceServicio.SelectedValue),
                                                            Convert.ToInt32(ddlDetalleNegocio.SelectedValue), Convert.ToInt32(ddlClasificacion1.SelectedValue),
                                                            Convert.ToInt32(ddlClasificacion2.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            else
            {   //Insertando Clasificacion
                result = SAT_CL.Global.Clasificacion.InsertaClasificacion(this._id_tabla, this._id_registro, 0, Convert.ToInt32(ddlFlota.SelectedValue),
                                                                         Convert.ToInt32(ddlRegion.SelectedValue), Convert.ToInt32(ddlUbicacionTerminal.SelectedValue),
                                                                         Convert.ToInt32(ddlTipoServicio.SelectedValue), Convert.ToInt32(ddlAlcanceServicio.SelectedValue),
                                                                         Convert.ToInt32(ddlDetalleNegocio.SelectedValue), Convert.ToInt32(ddlClasificacion1.SelectedValue),
                                                                         Convert.ToInt32(ddlClasificacion2.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Validando que la operacion fuera exitosa
            if (result.OperacionExitosa)
            {   //Creando Objeto
                objClasificacion = new SAT_CL.Global.Clasificacion(result.IdRegistro);
                //Inicializando Controles
                InicializaControl(this.objClasificacion.id_tabla, this.objClasificacion.id_registro, this._id_compania);
            }
            //Mostrando Mensaje de Operacion
            lblError.Text = result.Mensaje;
            //Devolviendo Objeto de Retorno
            return result;
        }

        /// <summary>
        /// Guarda los cambios realizados en la clasificación del Servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaCambiosClasificacionServicio()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que exista un Registro
            if (objClasificacion.id_clasificacion == 0)
            {
                //Insertando Clasificacion
                result = SAT_CL.Global.Clasificacion.InsertaClasificacion(this._id_tabla, this._id_registro, 0, Convert.ToInt32(ddlFlota.SelectedValue),
                                                                         Convert.ToInt32(ddlRegion.SelectedValue), Convert.ToInt32(ddlUbicacionTerminal.SelectedValue),
                                                                         Convert.ToInt32(ddlTipoServicio.SelectedValue), Convert.ToInt32(ddlAlcanceServicio.SelectedValue),
                                                                         Convert.ToInt32(ddlDetalleNegocio.SelectedValue), Convert.ToInt32(ddlClasificacion1.SelectedValue),
                                                                         Convert.ToInt32(ddlClasificacion2.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            else
            {
                //Instanciamos Servicio
                using (Servicio objServicio = new Servicio(this._id_registro))
                {
                    //Actualizando los Datos
                    result = objClasificacion.EditaClasificacion(this._id_tabla, this._id_registro, objClasificacion.id_tipo, Convert.ToInt32(ddlFlota.SelectedValue),
                                                                        Convert.ToInt32(ddlRegion.SelectedValue), Convert.ToInt32(ddlUbicacionTerminal.SelectedValue),
                                                                        Convert.ToInt32(ddlTipoServicio.SelectedValue), Convert.ToInt32(ddlAlcanceServicio.SelectedValue),
                                                                        Convert.ToInt32(ddlDetalleNegocio.SelectedValue), Convert.ToInt32(ddlClasificacion1.SelectedValue),
                                                                        Convert.ToInt32(ddlClasificacion2.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            //Validando que la operacion fuera exitosa
            if (result.OperacionExitosa)
            {   //Creando Objeto
                objClasificacion = new SAT_CL.Global.Clasificacion(result.IdRegistro);
                //Inicializando Controles
                InicializaControl(this.objClasificacion.id_tabla, this.objClasificacion.id_registro, this._id_compania);
            }
            //Mostrando Mensaje de Operacion
            lblError.Text = result.Mensaje;
            //Devolviendo Objeto de Retorno
            return result;
        }

        /// <summary>
        /// Descarta cualquier cambio no guardado en el contenido del control
        /// </summary>
        /// <returns></returns>
        public void CancelaCambiosClasificacion()
        {
            InicializaControl(this._id_tabla, this._id_registro, this._id_compania);
        }

        #endregion
    }
}