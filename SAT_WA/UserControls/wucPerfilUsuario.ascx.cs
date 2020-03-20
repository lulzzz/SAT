using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Seguridad;

namespace SAT.UserControls
{
    public partial class wucPerfilUsuario : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_usuario;
        private DataTable _dtPerfiles;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
            if (!(Page.IsPostBack))
            
                //Cargando Tamaño del GridView
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            else
                //Recuperando Valor de los Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento Producido Despues de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCerrarVentana != null)

                //Iniciando Manejador
                OnClickCerrarVentana(e);
        }

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Cerrar Ventana"
        /// </summary>
        public event EventHandler ClickCerrarVentana;
        /// <summary>
        /// Evento que Manipula el Manejador "Cerrar Ventana"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCerrarVentana(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickCerrarVentana != null)
                //Iniciando Evento
                ClickCerrarVentana(this, e);
        }

        #endregion

        #region Eventos GridView "Perfiles Usuario"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Perfiles Usuario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvPerfilesUsuario, this._dtPerfiles, Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Perfiles Usuario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(this._dtPerfiles, "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Perfiles Usuario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPerfilesUsuario_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Asignando criterio de ordenamiento actual
            if (this._dtPerfiles != null)
                this._dtPerfiles.DefaultView.Sort = lblOrdenado.Text;

            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvPerfilesUsuario, this._dtPerfiles, e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Perfiles Usuario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPerfilesUsuario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvPerfilesUsuario, this._dtPerfiles, e.NewPageIndex);
        }

        /// <summary>
        /// Eventos Producido al Seleccionar el Perfil de Usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionar_Click(object sender, EventArgs e)
        {
            //Validando que Existan Perfiles
            if (gvPerfilesUsuario.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPerfilesUsuario, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando Perfil de Usuario
                    using (SAT_CL.Seguridad.PerfilSeguridadUsuario psu = new SAT_CL.Seguridad.PerfilSeguridadUsuario(Convert.ToInt32(gvPerfilesUsuario.SelectedDataKey["IdPerfilUsuario"])))
                    {
                        //Validando que Exista el Perfil
                        if (psu.id_perfil_usuario > 0)
                        {
                            //Instanciando Perfil de Usuario
                            using (SAT_CL.Seguridad.PerfilSeguridadUsuario pActivo = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilActivo(this._id_usuario))
                            {
                                //Validando que Exista un Perfil Activo
                                if (pActivo.id_perfil_usuario > 0)
                                {
                                    //Validando que sean Distintos
                                    if (pActivo.id_perfil_usuario != psu.id_perfil_usuario)
                                    {
                                        //Actualizando Perfil Activo
                                        result = pActivo.EditaPerfilSeguridadUsuario(pActivo.id_perfil, pActivo.id_usuario, false, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Exitosa
                                        if(result.OperacionExitosa)
                                            
                                            //Actualizando Perfil Actual
                                            result = psu.EditaPerfilSeguridadUsuario(psu.id_perfil, psu.id_usuario, true, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                    else
                                        //Actualizando Perfil Actual
                                        result = psu.EditaPerfilSeguridadUsuario(psu.id_perfil, psu.id_usuario, true, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                }
                                else
                                    //Actualizando Perfil Actual
                                    result = psu.EditaPerfilSeguridadUsuario(psu.id_perfil, psu.id_usuario, true, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                
                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Instanciando Usuario
                                    using (SAT_CL.Seguridad.Usuario user = (SAT_CL.Seguridad.Usuario)Session["usuario"])
                                    {
                                        //Actualizando Atributos
                                        if(user.ActualizaAtributos())
                                        {
                                            //Actualizando Perfil de Usuario
                                            psu.ActualizaPerfilSeguridadUsuario();
                                            
                                            //Instanciando Perfil
                                            using (SAT_CL.Seguridad.PerfilSeguridad ps = new PerfilSeguridad(psu.id_perfil))
                                            {
                                                //Instanciando Perfil de Seguridad
                                                if (ps.id_perfil_seguridad > 0)
                                                {
                                                    //Instanciando Forma
                                                    using (SAT_CL.Seguridad.Forma form = new SAT_CL.Seguridad.Forma(ps.id_forma_inicio))
                                                    {
                                                        //Completando Transacción
                                                        trans.Complete();

                                                        //Inicializando variables de Sesión
                                                        inicializaVariablesSesion(user);

                                                        //Obteniendo Ruta Relativa
                                                        string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta(Page.AppRelativeVirtualPath, form.ruta_relativa);

                                                        //Redireccionando a la Forma Inicio del Perfil
                                                        Response.Redirect(url);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Asignar el Valor de los Atributos
        /// </summary>
        private void asignaAtributos()
        {   
            //Asignando Valores a los ViewState
            ViewState["idUsuario"] = this._id_usuario;
            ViewState["PerfilesUsuario"] = this._dtPerfiles;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   
            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idUsuario"]) != 0)                
                //Asignando Valor al Atributo
                this._id_usuario = Convert.ToInt32(ViewState["idUsuario"]);
            
            //Validando que Exista el Origen de Datos
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["PerfilesUsuario"]))
                //Asignando Valor al Atributo
                this._dtPerfiles = (DataTable)ViewState["PerfilesUsuario"];
        }
        /// <summary>
        /// Método encaragdo de inicializar las variables de sesión
        /// </summary>
        /// <param name="usuario">Objeto usuario actual</param>
        private void inicializaVariablesSesion(SAT_CL.Seguridad.Usuario usuario)
        {
            //INICIALIZANDO VALORES DE VARIABLES DE SESION
            //Objeto usuario
            Session["usuario"] = usuario;
            //Id de Registro
            Session["id_registro"] = 0;
            //Id de registro b
            Session["id_registro_b"] = 0;
            //Id de registro c
            Session["id_registro_c"] = 0;
            //Variable de sesion estatus siempre a nuevo registro
            Session["estatus"] = Pagina.Estatus.Nuevo;
            //Vaciando el DataSet de sesion
            Session["DS"] = null;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar los Perfiles de Usuario
        /// </summary>
        /// <param name="id_usuario">Usuario</param>
        public void InicializaPerfilesUsuario(int id_usuario)
        {
            //Asignando Atributo
            this._id_usuario = id_usuario;

            //Obteniendo Perfiles
            using(DataTable dtPerfiles = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilesUsuario(this._id_usuario))
            {
                //Validando que Existan Perfiles
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtPerfiles))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvPerfilesUsuario, dtPerfiles, "IdPerfilUsuario", "", true, 1);

                    //Asignando Perfiles
                    this._dtPerfiles = dtPerfiles;
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvPerfilesUsuario);

                    //Eliminando Perfiles
                    this._dtPerfiles = null;
                }
            }
        }
        /// <summary>
        /// Método encargado de Cancelar el Cambio de Perfil
        /// </summary>
        /// <param name="identificador_script"></param>
        /// <param name="nombre_panel"></param>
        public void CancelaCambioPerfil(string identificador_script, params string[] nombre_panel)
        {
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarImagen, uplnkCerrarImagen.GetType(), identificador_script, nombre_panel);
        }

        #endregion
    }
}