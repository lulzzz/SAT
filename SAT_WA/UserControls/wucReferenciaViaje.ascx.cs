using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucReferenciaViaje : System.Web.UI.UserControl
    {
        #region Propiedades del control

        private int _id_registro;
        /// <summary>
        /// Define el registro al cual pertenecen las referencias.
        /// </summary>
        public int Registro
        {   //Obteniendo Valor
            get { return _id_registro; }
            //Asignando Valor
            set { _id_registro = value; }
        }

        private int _id_tabla;
        /// <summary>
        /// Define la tabla a la cual pertenecen las referencias.
        /// </summary>
        public int Tabla
        {   //Obteniendo Valor
            get { return _id_tabla; }
            //Asignando Valor
            set { _id_tabla = value; }
        }

        private int _id_compania;
        /// <summary>
        /// Define la compania a la cual pertenecen las referencias.
        /// </summary>
        public int Compania
        {   //Obteniendo Valor
            get { return _id_compania; }
            //Asignando Valor
            set { _id_compania = value; }
        }

        private int _id_cliente;
        /// <summary>
        /// Define el cliente a la cual pertenecen las referencias.
        /// </summary>
        public int Cliente
        {   //Obteniendo Valor
            get { return _id_cliente; }
            //Asignando Valor
            set { _id_cliente = value; }
        }

        private DataTable _dt;
        /// <summary>
        /// Propiedad encargada del Orden de Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   ddlTipoReferencia.TabIndex =
                txtReferencia.TabIndex =
                btnAgregar.TabIndex =
                btnCancelar.TabIndex = 
                ddlTamano.TabIndex =
                lnkExportar.TabIndex =
                gvReferencias.TabIndex = value;
            }
            get { return ddlTipoReferencia.TabIndex; }
        }
        /// <summary>
        /// Propiedad encargada de la Habilitacion de los Controles
        /// </summary>
        public bool Enable
        {
            set
            {   ddlTipoReferencia.Enabled =
                txtReferencia.Enabled =
                btnAgregar.Enabled =
                btnCancelar.Enabled = 
                ddlTamano.Enabled =
                lnkExportar.Enabled =
                gvReferencias.Enabled = value;
            }
            get { return ddlTipoReferencia.Enabled; }
        }

        #endregion
        
        #region Eventos del control

        /// <summary>
        /// Metodo ejecutado al cargar la pagina donde se encuentra contenido el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   
            //Si es una recarga de página
            if (Page.IsPostBack)
                //Leyendo viewstate
                leeViewState();
        }
        /// <summary>
        /// Evento disparado antes de guardar el Viewstate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Asignando el viewstate
            asignaViewState();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {   
            //Validando que el Evento no este Vacio
            if (ClickGuardarReferenciaViaje != null)
                //Invocando al Manejador de Evento
                OnClickGuardarReferenciaViaje(e);
            return;
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   
            //Limpiando Controles
            limipiaControles();
        }

        #region Manejador de Eventos

        /// <summary>
        /// Evento "ClickGuardarRefrenciaViaje"
        /// </summary>
        public event EventHandler ClickGuardarReferenciaViaje;
        /// <summary>
        /// Evento "ClickEliminarRefrenciaViaje"
        /// </summary>
        public event EventHandler ClickEliminarReferenciaViaje;
        /// <summary>
        /// Método que manipula el Evento "ClickGuardarRefrenciaViaje"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarReferenciaViaje(EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickGuardarReferenciaViaje != null)
                //Inicializando Evento
                ClickGuardarReferenciaViaje(this, e);
        }
        /// <summary>
        /// Método que manipula el Evento "ClickEliminarRefrenciaViaje"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarReferenciaViaje(EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickEliminarReferenciaViaje != null)
                //Inicializando Evento
                ClickEliminarReferenciaViaje(this, e);
        }

        #endregion

        #region Eventos GridView "Resultados"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Asignando Expresion de Ordenamiento
            this._dt.DefaultView.Sort = lblCriterio.Text;
            //Cambiando Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvReferencias, this._dt, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Asignando Expresion de Ordenamiento
            this._dt.DefaultView.Sort = lblCriterio.Text;
            //Cambiando Indice de Página del GridView
            Controles.CambiaIndicePaginaGridView(gvReferencias, this._dt, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_Sorting(object sender, GridViewSortEventArgs e)
        {   
            //Asignando Expresion de Ordenamiento
            this._dt.DefaultView.Sort = lblCriterio.Text;
            //Cambiando Indice de Página del GridView
            lblCriterio.Text = Controles.CambiaSortExpressionGridView(gvReferencias, this._dt, e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(this._dt, "Id");                
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {   
            //Validando que existan Registros
            if (gvReferencias.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvReferencias, sender, "lnk", false);
                //Instanciando Cargo Recurrente
                using (SAT_CL.Global.Referencia ref1 = new SAT_CL.Global.Referencia(Convert.ToInt32(gvReferencias.SelectedDataKey["Id"])))
                {   //Validando que exista el Registro
                    if (ref1.id_referencia != 0)
                    {   //Asignando Valores
                        ddlTipoReferencia.SelectedValue = ref1.id_referencia_tipo.ToString();
                        txtReferencia.Text = ref1.valor;
                    }
                    else
                    {   //Mostrando Error
                        lblError.Text = "No se pudo acceder al registro seleccionado, probablemente no existe";
                        //Quitando Selección
                        gvReferencias.SelectedIndex = -1;
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if (gvReferencias.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvReferencias, sender, "lnk", false);
                //Validando que el Evento no este Vacio
                if (ClickEliminarReferenciaViaje != null)
                    //Invocando al Manejador de Evento
                    OnClickEliminarReferenciaViaje(e);
                return;
            }
        }

        /// Evento generado al dar click en Bitácora 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacora(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvReferencias, sender, "lnk", false);
            //Validamos que existan Registros
            if (gvReferencias.DataKeys.Count > 0)
            {
                //Mostramos Bitácora
                inicializaBitacora(gvReferencias.SelectedValue.ToString(), "35", "Bitácora Parada");
            }
        }
        #endregion

        #endregion

        #region Metodos Privados del Control

        /// <summary>
        /// Metodo encargado de inicializar el control de usuario
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="compania"></param>
        /// <param name="cliente"></param>
        /// <param name="tabla"></param>
        public void InicializaControl(int registro, int compania, int cliente, int tabla)
        {   
            //Estableciendo registro y tabla
            this._id_registro = registro;
            this._id_compania = compania;
            this._id_cliente = cliente;
            this._id_tabla = tabla;

            //Limpiando Control
            txtReferencia.Text = "";

            //Cargando Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);

            //Validando que Exista la Tabla
            if (this._id_tabla == 1)
            {
                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoReferencia, 18, "", compania, "", cliente, "");
                
                //Cargando Referencias del Viaje
                cargaReferenciasViaje();
            }
            else
            {
                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoReferencia, 69, "", this._id_tabla, "", cliente, "");

                //Cargando Referencias
                cargaReferencias();
            }

            //Asignando viewstate
            asignaViewState();
        }
        /// <summary>
        /// Metodo encargado de inicializar el control de usuario
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        public void InicializaControl(int id_servicio)
        {
            //Instanciando servicio
            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(id_servicio))
            {
                //Validando que Exista el Registro
                if (servicio.habilitar)
                {
                    //Estableciendo registro y tabla
                    this._id_registro = servicio.id_servicio;
                    this._id_compania = servicio.id_compania_emisor;
                    this._id_cliente = servicio.id_cliente_receptor;
                    this._id_tabla = 1;

                    //Limpiando Control
                    txtReferencia.Text = "";

                    //Cargando Catalogo
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoReferencia, 18, "", this._id_compania, "", this._id_cliente, "");
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
                    //Cargando Referencias del Viaje
                    cargaReferenciasViaje();
                    //Asignando viewstate
                    asignaViewState();
                }
                
            }
        }
        /// <summary>
        /// Método encargado de Cargar las referencias del Viaje
        /// </summary>
        private void cargaReferenciasViaje()
        {   
            //Inicializamos el gridView
            using (DataTable dt = SAT_CL.Global.Referencia.CargaReferenciasRegistroViaje(this._id_compania, this._id_registro))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {   //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvReferencias, dt, "Id", "");
                    //Guardando Tabla al Atributo
                    this._dt = dt;
                }
                else
                {   //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvReferencias);
                    //Guardando Tabla al Atributo
                    this._dt = null;
                }
            }

            //Limpiando Etiqueta
            lblError.Text = "";
        }
        /// <summary>
        /// Método encargado de Cargar las referencias del Viaje
        /// </summary>
        private void cargaReferencias()
        {
            //Inicializamos el gridView
            using (DataTable dt = SAT_CL.Global.Referencia.CargaReferenciasRegistro(this._id_compania, this._id_registro, this._id_tabla))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {   //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvReferencias, dt, "Id", "");
                    //Guardando Tabla al Atributo
                    this._dt = dt;
                }
                else
                {   //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvReferencias);
                    //Guardando Tabla al Atributo
                    this._dt = null;
                }
            }

            //Limpiando Etiqueta
            lblError.Text = "";
        }
        /// <summary>
        /// Método encargado de asignar las variabkes viewstate
        /// </summary>
        private void asignaViewState()
        {   //Asignando los Valores
            ViewState["id_registro"] = _id_registro;
            ViewState["id_tabla"] = _id_tabla;
            ViewState["id_compania"] = _id_compania;
            ViewState["id_cliente"] = _id_cliente;
            ViewState["DT"] = _dt;
        }
        /// <summary>
        /// Método encargado de leer las variables viewstate
        /// </summary>
        private void leeViewState()
        {   //Recuperando los Valores
            this._id_registro = Convert.ToInt32(ViewState["id_registro"]);
            this._id_tabla = Convert.ToInt32(ViewState["id_tabla"]);
            this._id_compania = Convert.ToInt32(ViewState["id_compania"]);
            this._id_cliente = Convert.ToInt32(ViewState["id_cliente"]);
            this._dt = (DataTable)ViewState["DT"];
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles
        /// </summary>
        private void limipiaControles()
        {   
            //Limpiando Controles
            txtReferencia.Text = 
            lblError.Text = "";
            TSDK.ASP.Controles.InicializaIndices(gvReferencias);

            //Validando que sean Referencias de Servicio
            if(this._id_tabla == 1)
                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoReferencia, 18, "", this._id_compania, "", this._id_cliente, "");
            else
                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoReferencia, 69, "", this._id_tabla, "", this._id_cliente, "");
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
        /// Método Público encaragdo de Guardar las Referencias de Viaje
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaReferenciaViaje()
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando que exista un Registro
            if (this._id_registro != 0)
            {   
                //Validando que exista un Registro Seleccionado
                if (gvReferencias.SelectedIndex != -1)
                {
                    //Instanciando Referencia
                    using (SAT_CL.Global.Referencia ref1 = new SAT_CL.Global.Referencia(Convert.ToInt32(gvReferencias.SelectedDataKey["Id"])))
                    {
                        //Validando que la Referencia sea Valida
                        if (ref1.id_referencia != 0)

                            //Editando la Referencia
                            result = SAT_CL.Global.Referencia.EditaReferencia(ref1.id_referencia, Convert.ToInt32(ddlTipoReferencia.SelectedValue),
                                                        txtReferencia.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
                else
                {
                    //Insertando la Referencia
                    result = SAT_CL.Global.Referencia.InsertaReferencia(this._id_registro, this._id_tabla, Convert.ToInt32(ddlTipoReferencia.SelectedValue),
                                                txtReferencia.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                }
                //Validamos Resultado
                if (result.OperacionExitosa)
                {
                    //Si el tipo de Referencia es Viaje
                    if (Convert.ToInt32(ddlTipoReferencia.SelectedValue) == SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 1, "No. Viaje", 0, "Referencia de Viaje"))
                    {
                        //Instanciamos Servicio
                        using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(this._id_registro))
                        {
                            //Editamos referencia del Cliente de Servicio
                            result = objServicio.EditarServicio(txtReferencia.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }

            }
            else
                //Instanciando Exception
                result = new RetornoOperacion("No existe el registro");
            
            //Validando que la Operación haya sido exitosa
            if(result.OperacionExitosa)
            {   
                //Validando que Exista la Tabla
                if (this._id_tabla == 1)

                    //Cargando Referencias del Viaje
                    cargaReferenciasViaje();
                else
                    //Cargando Referencias de la Entidad
                    cargaReferencias();

                //Limpiando Controles
                limipiaControles();
            }
            
            //Mostrando Mensaje de Error
            lblError.Text = result.Mensaje;
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Eliminar las Referencias
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaReferenciaViaje()
        {   
            //Declarando Objeto de Retorno de Operacion
            RetornoOperacion result = new RetornoOperacion();
            
            //Instanciando Cargo Recurrente
            using (SAT_CL.Global.Referencia ref1 = new SAT_CL.Global.Referencia(Convert.ToInt32(gvReferencias.SelectedDataKey["Id"])))
            {   
                //Validando que exista el Registro
                if (ref1.id_referencia != 0)
                {   
                    //Asignando Valores
                    result = SAT_CL.Global.Referencia.EliminaReferencia(ref1.id_referencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validamos Resultado
                    if (result.OperacionExitosa)
                    {
                        //Si el tipo de Referencia es Viaje
                        if (ref1.id_referencia_tipo== SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 1, "No. Viaje", 0, "Referencia de Viaje"))
                        {
                            //Instanciamos Servicio
                            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(this._id_registro))
                            {
                                //Editamos referencia del Cliente de Servicio
                                result = objServicio.EditarServicio("", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                    }
                    //Validando que la Operación haya sido exitosa
                    if (result.OperacionExitosa)
                    {
                        //Validando que Exista la Tabla
                        if (this._id_tabla == 1)

                            //Cargando Referencias del Viaje
                            cargaReferenciasViaje();
                        else
                            //Cargando Referencias de la Entidad
                            cargaReferencias();

                        //Limpiando Controles
                        limipiaControles();
                    }
                }
                else
                {   
                    //Instanciando Error
                    result = new RetornoOperacion("No se pudo acceder al registro seleccionado, probablemente no existe");
                    
                    //Quitando Selección
                    gvReferencias.SelectedIndex = -1;
                }
                
                //Mostrando Mensaje
                lblError.Text = result.Mensaje;
            }
            
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}