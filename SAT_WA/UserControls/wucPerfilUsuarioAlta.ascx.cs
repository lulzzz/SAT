using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucPerfilUsuarioAlta : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_usuario;
        private int _id_compania;
        private DataTable _dtPerfiles;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                ddlPerfil.TabIndex =
                txtUsuario.TabIndex = 
                btnGuardar.TabIndex =
                btnCancelar.TabIndex =
                ddlTamano.TabIndex =
                lnkExportar.TabIndex =
                gvPerfilesUsuario.TabIndex = value;
            }
            get { return ddlPerfil.TabIndex; }
        }
        /// <summary>
        /// Propiedad que establece la Habilitación de los Controles
        /// </summary>
        public bool Enabled
        {
            set
            {   //Asignando Orden
                ddlPerfil.Enabled =
                txtUsuario.Enabled =
                btnGuardar.Enabled =
                btnCancelar.Enabled =
                ddlTamano.Enabled =
                lnkExportar.Enabled =
                gvPerfilesUsuario.Enabled = value;
            }
            get { return ddlPerfil.Enabled; }
        }

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
                cargaCatalogos();
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
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarPerfilUsuario != null)

                //Iniciando Manejador
                OnClickGuardarPerfilUsuario(e);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Inicializando Control
            inicializaControlUsuario(this._id_compania, this._id_usuario);
        }

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Eliminar"
        /// </summary>
        public event EventHandler ClickEliminarPerfilUsuario;
        /// <summary>
        /// Evento que Manipula el Manejador "Eliminar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarPerfilUsuario(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickEliminarPerfilUsuario != null)
                //Iniciando Evento
                ClickEliminarPerfilUsuario(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarPerfilUsuario;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarPerfilUsuario(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickGuardarPerfilUsuario != null)
                //Iniciando Evento
                ClickGuardarPerfilUsuario(this, e);
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
            //Validando que Existan Registros
            if (gvPerfilesUsuario.DataKeys.Count > 0)
            {
                //Asignando Expresión de Ordenamiento
                this._dtPerfiles.DefaultView.Sort = lblOrdenado.Text;
                
                //Cambiando Expresión del Ordenamiento
                lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvPerfilesUsuario, this._dtPerfiles, e.SortExpression);
            }
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
        /// Eventos Producido al Eliminar el Perfil de Usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if(gvPerfilesUsuario.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvPerfilesUsuario, sender, "lnk", false);

                //Validando que exista un Evento
                if (ClickEliminarPerfilUsuario != null)
                    
                    //Iniciando Manejador
                    OnClickEliminarPerfilUsuario(e);
            }
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar el Control de Usuario
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_usuario">Usuario</param>
        private void inicializaControlUsuario(int id_compania, int id_usuario)
        {
            //Asignando Atributos
            this._id_compania = id_compania;
            this._id_usuario = id_usuario;

            //Habilitando Controles
            habilitaControles(true);

            //Cargando Catalogos
            cargaCatalogos();

            //Instanciando Usuario
            using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(this._id_usuario))
            {
                //Validando que exista
                if (user.id_usuario > 0)

                    //Asignando Valor
                    txtUsuario.Text = user.nombre + " ID:" + user.id_usuario.ToString();
                else
                    //Limpiando Valor
                    txtUsuario.Text = "";
            }

            //Limpiando Mensaje de Error
            lblError.Text = "";

            //Cargando Perfiles de Usuario
            cargaPerfilesUsuario();
        }
        /// <summary>
        /// Método encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles(bool enable)
        {
            //Asignando Orden
            ddlPerfil.Enabled =
            //chkActivo.Enabled =
            //txtUsuario.Enabled =
            btnGuardar.Enabled =
            btnCancelar.Enabled =
            ddlTamano.Enabled =
            lnkExportar.Enabled =
            gvPerfilesUsuario.Enabled = enable;
        }

        /// <summary>
        /// Método encargado de Cargar los Perfiles del Usuario
        /// </summary>
        private void cargaPerfilesUsuario()
        {
            //Obteniendo Perfiles
            using (DataTable dtPerfiles = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilesUsuario(this._id_usuario))
            {
                //Validando que Existan Perfiles
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPerfiles))
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

                //Inicializando Indices
                Controles.InicializaIndices(gvPerfilesUsuario);
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Tipos de Método de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPerfil, 61, "-- Seleccione un Perfil de la Lista", this._id_compania, "", 0, "");

            //Cargando Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Asignar el Valor de los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Valores a los ViewState
            ViewState["idCompania"] = this._id_compania;
            ViewState["idUsuario"] = this._id_usuario;
            ViewState["PerfilesUsuario"] = this._dtPerfiles;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que existan los Valores
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)

                //Asignando Valor al Atributo
                this._id_compania = Convert.ToInt32(ViewState["idCompania"]);
            
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
        /// <param name="id_compania">Compania Emisora</param>
        public void InicializaPerfilesUsuarioAlta(int id_usuario, int id_compania)
        {
            //Invocando Método de Inicialización
            inicializaControlUsuario(id_compania, id_usuario);
        }
        /// <summary>
        /// Método encargado de Guardar el Perfil de Usuario
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaPerfilUsuario()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que exista un Perfil
            if (ddlPerfil.SelectedValue != "0")
            {
                //Insertando Perfil
                result = SAT_CL.Seguridad.PerfilSeguridadUsuario.InsertaPerfilSeguridadUsuario(Convert.ToInt32(ddlPerfil.SelectedValue),
                                               Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUsuario.Text, "ID:", 1, "0")),
                                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("* Seleccione un Perfil de la Lista");

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Inicialización
                inicializaControlUsuario(this._id_compania, this._id_usuario);

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Eliminar los Perfiles del Usuario
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaPerfilUsuario()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que existan Perfiles
                if (this._dtPerfiles.Rows.Count > 1)
                {
                    //Instanciando Perfil de Usuario
                    using (SAT_CL.Seguridad.PerfilSeguridadUsuario psu = new SAT_CL.Seguridad.PerfilSeguridadUsuario(Convert.ToInt32(gvPerfilesUsuario.SelectedDataKey["IdPerfilUsuario"])))
                    {
                        //Validando que Exista el Perfil
                        if (psu.id_perfil_usuario > 0)
                        {
                            //Editando Perfil
                            result = psu.DeshabilitaPerfilSeguridadUsuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa && psu.perfil_activo)
                            {
                                //Obteniendo Perfiles
                                using (DataTable dtPerfiles = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilesUsuario(this._id_usuario))
                                {
                                    //Validando que Existan Perfiles
                                    if (Validacion.ValidaOrigenDatos(dtPerfiles))
                                    {
                                        //Recorriendo Perfil
                                        foreach (DataRow dr in dtPerfiles.Rows)
                                        {
                                            //Instanciando Primer Perfil Encontrado
                                            using (SAT_CL.Seguridad.PerfilSeguridadUsuario p = new SAT_CL.Seguridad.PerfilSeguridadUsuario(Convert.ToInt32(dr["IdPerfilUsuario"])))
                                            {
                                                //Validando si esta Habilitado
                                                if(p.habilitar)
                                                
                                                    //Actualizando Perfil 
                                                    result = p.EditaPerfilSeguridadUsuario(p.id_perfil, p.id_usuario, true, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No Existen el Perfil");
                                            }

                                            //Terminando Ciclo
                                            break;
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No Existen Perfiles");
                                }
                            }

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Instanciando Perfil
                                result = new RetornoOperacion(psu.id_perfil_usuario);

                                //Completando Transacción
                                trans.Complete();
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe de existir un Perfil Activo");
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            
                //Invocando Método de Inicialización
                inicializaControlUsuario(this._id_compania, this._id_usuario);

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvPerfilesUsuario);

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}