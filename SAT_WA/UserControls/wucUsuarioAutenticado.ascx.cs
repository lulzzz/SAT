using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAT_CL.Seguridad;
using SAT_CL.Global;
using TSDK.Base;
using TSDK.ASP;

namespace SAT.UserControls
{
    public partial class wucUsuarioAutenticado : System.Web.UI.UserControl
    {
        #region Eventos

        /// <summary>
        /// Evento generado al Iniciar la Páginal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es una recarga de página
            if (!Page.IsPostBack)
                inicializaForma();

            //Validamos que exista la Sesión
            if (Session["usuario_sesion"] != null)
                //Actualizamos Etiqueta Fin de Sesión
                lkbFinSesion.Text = DateTime.MinValue == ((UsuarioSesion)Session["usuario_sesion"]).fecha_expiracion ? "Sin Fin de Sesión " : "Finaliza Sesión:  " + ((UsuarioSesion)Session["usuario_sesion"]).fecha_expiracion.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Evento producido al inicializar el contro de Usuario para realizar las validaciones correspondientes de la sesión de usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //validamos si la sesión se encuentra activa
            validaSesionActiva();
        }

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickCambiarContrasena;
        /// <summary>
        /// Evento que Manipula el Manejador "Cambiar Contraseña"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCambiarContrasena(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickCambiarContrasena != null)
                //Iniciando Evento
                ClickCambiarContrasena(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Cambia Perfil"
        /// </summary>
        public event EventHandler ClickCambiaPerfil;
        /// <summary>
        /// Evento que Manipula el Manejador "Cambia Perfil"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCambiaPerfil(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickCambiaPerfil != null)
                //Iniciando Evento
                ClickCambiaPerfil(this, e);
        }


        #endregion

        /// <summary>
        /// Evento Generado al hacer click en cualquier link Opciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_MenuOpciones(object sender, EventArgs e)
        {
            //Referenciamos al objeto que disparo el evento 
            LinkButton boton = (LinkButton)sender;

            //De acuerdo al nombre de comando
            switch (boton.CommandName)
            {
                //Cerramos Sesion
                case "CerrarSesion":
                    //Declaramos Objeto Resultante
                    RetornoOperacion resultado = new RetornoOperacion();
                    //Instanciamos la Sesion del Usuario
                    using (UsuarioSesion objUsuarioSesion = ((UsuarioSesion)Session["usuario_sesion"]))
                    {
                        //Validamos que exista Sesion
                        if (objUsuarioSesion.id_usuario_sesion > 0)
                        {
                            //Cerramos Sesion en BD
                            resultado = objUsuarioSesion.TerminarSesion();

                            //Si se Termino la Sesión correctamnete
                            if(resultado.OperacionExitosa)
                                //Cerramos Session
                                Sesion.CierraSesionActual(FormsAuthentication.LoginUrl);
                        }
                    }
                    break;

                case "CambiaContrasena":
                    {
                        //Validando que exista un Evento
                        if (ClickCambiarContrasena != null)
                            //Iniciando Manejador
                            OnClickCambiarContrasena(e);

                        break;
                    }
                case "CambiaPerfil":
                    {
                        //Validando que exista un Evento
                        if (ClickCambiaPerfil != null)
                            //Iniciando Manejador
                            OnClickCambiaPerfil(e);

                        break;
                    }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Forma
        /// </summary>
        private void inicializaForma()
        {
            /*TODO: DETERMINAR COMO SE HABILITARÁ O DESHABILITARÁ EL TIMER DE BUSQUEDA DE PENDIENTES*/

            //Instanciando usuario
            using (Usuario usuario = (Usuario)Session["usuario"])
            {
                //Si el usuario es el responsable de realizar depósitos
                if (usuario.habilitar && usuario.notificaciones.Equals("Si"))
                
                    //Activando Notificaciones
                    tmrNotificaciones.Enabled = true;
            }

            //Instanciamos la Sesin del Usuario
            using (UsuarioSesion objUsuarioSesion = ((UsuarioSesion)Session["usuario_sesion"]))
            {
                //Validamos que exista Sesion
                if (objUsuarioSesion.id_usuario_sesion > 0)
                {
                    //Refrescamos atributos de la Sesion
                    objUsuarioSesion.ActualizaUsuarioSesion();
                    //Inicializamos Valores
                    inicializamosValores(objUsuarioSesion);
                    
                    if (tmrNotificaciones.Enabled)
                        //Buscando notificaciones
                        muestraPendientesUsuario();
                }
            }
        }         
         /// <summary>
         /// Inicializamos Valores
         /// </summary>
        private void inicializamosValores(UsuarioSesion objUsuarioSesion)
        {
            //Referenciando al usuario de la sesión actual
            using (Usuario u = (Usuario)Session["usuario"])
            {
                //Nombre de Usuario
                lkbUsuario.Text = u.nombre;
                //Fecha de Inicio de Sesión
                lkbInicioSesion.Text = "Inicio Sesión: " + objUsuarioSesion.fecha_inicio.ToString("dd/MM/yyyy HH:mm");

                //Obteniendo el perfil de seguridad actual del usuario
                SAT_CL.Seguridad.PerfilSeguridadUsuario perfilActivo = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilActivo(u.id_usuario);
                using (SAT_CL.Seguridad.PerfilSeguridad perfil = new PerfilSeguridad(perfilActivo.id_perfil))
                {
                    //Perfil
                    lkbPerfil.Text = string.Format("Perfil: {0}", perfil.descripcion);
                }

                //Instanciamos Compañia de la sesión actual
                using (CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {
                    lkbCompania.Text = "Compañía: " + objCompania.nombre_corto;
                }

                //Validamos exista dias de Vigencia (0 = Sin vigencia, es permanente)
                if (u.dias_vigencia > 0)
                {
                    //Si  la vigencia ha caducado
                    if (DateTime.Today.CompareTo(u.fecha_contrasena.Date.AddDays(u.dias_vigencia)) > 0)
                    {
                        //Redireccionamos a pagina Cambio de Contraseña
                        Response.Redirect("~/CambioContrasena.aspx");
                    }
                }
            }
        }
        /// <summary>
        /// Validamos Si la Sesión se encuentra activa
        /// </summary>
        private void validaSesionActiva()
        {
            //Verificando que exista una sesión de BD en la sesión de ASP
            if (Session["usuario_sesion"] == null)
            {
                //Recuperando la cookie
                HttpCookie c = Request.Cookies.Get("Login");

                //Instanciando al usuario sesión  para cerrar la sesión 
                using (UsuarioSesion objUsuarioSesion = new UsuarioSesion(Convert.ToInt32(c["ID"])))
                {
                    //Validamos que exista Sesion
                    if (objUsuarioSesion.id_usuario_sesion > 0)
                        //Cerramos Sesion BD                          
                        objUsuarioSesion.TerminarSesion();
                }

                //Cerramos Session desde asp.net
                Sesion.CierraSesionActual(FormsAuthentication.LoginUrl);
            }
            else
            {
                //Instanciamos Usuario Sesion
                using (UsuarioSesion objUsuarioSesion = (UsuarioSesion)Session["usuario_sesion"])
                {
                    //Refresca Atributos
                    if (objUsuarioSesion.ActualizaUsuarioSesion())
                    {
                        //Validando si la sesión ha finalizado
                        if (objUsuarioSesion.EstatusSesion == UsuarioSesion.Estatus.Activo)
                        {                            
                            //Referenciando al usuario de la sesión actual
                            using (Usuario u = (Usuario)Session["usuario"])
                            {
                                //Actualziando información desde BD
                                if (u.ActualizaAtributos())
                                {
                                    //Validando posible cambio de contraseña no reflejado en esta sesión
                                    if (u.fecha_contrasena < objUsuarioSesion.ultima_actividad)
                                        //Actualizamos Ultima Actividad
                                        objUsuarioSesion.ActualizaUltimaActividad();
                                    //Si la contraseña se modificó
                                    else
                                    {
                                        //Direccionando a página de inicio
                                        Sesion.CierraSesionActual(FormsAuthentication.LoginUrl);
                                    }
                                }
                            }
                        }
                        else
                            //Direccionando a página de inicio
                            Sesion.CierraSesionActual(FormsAuthentication.LoginUrl);
                    }
                }
            }
        }

        #endregion

        #region Notificaciones

        /// <summary>
        /// Click en botón de pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNotificacionesUsuario_Click(object sender, EventArgs e)
        {
            muestraPendientesUsuario();
        }
        /// <summary>
        /// Temporizador de notificaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tmrNotificaciones_Tick(object sender, EventArgs e)
        {
            muestraPendientesUsuario();
        }
        /// <summary>
        /// Realiza la búsqueda de pendientes para el usuario autenticado, sobre la compañía en la que inició sesión
        /// </summary>
        private void muestraPendientesUsuario()
        { 
            //Creando Lista de pendientes
            List<string> notificaciones = new List<string>();
            
            //TODO: REALIZAR LA CLASIFICACIÓN O SEPARACIÓN ADECUADA DE CARGA DE PENDIENTES ACORDE A NECESIDADES (PERFILES DE USUARIO, DEPARTAMENTO, ETC.)

            /**
             * DEPÓSITOS PENDIENTES (ANTICIPOS OPERADOR, PAGOS PROVEEDOR, LIQUIDACIONES, ETC.)
             */
            //Instanciando usuario
            using (Usuario usuario = (Usuario)Session["usuario"])
            {
                //Si el usuario es el responsable de realizar depósitos
                if (usuario.habilitar && usuario.notificaciones.Equals("Si"))
                {
                    //Anticipos
                    using (DataTable mit = SAT_CL.EgresoServicio.Deposito.CargaRegistrosPorDepositar(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        //Si hay registros
                        if (mit != null)
                            //Agregando pendiente
                            notificaciones.Add(string.Format("{0} Depósito(s) a Operador.", mit.Rows.Count));
                    }
                    //Liquidaciones
                    using (DataTable mit = SAT_CL.Liquidacion.Liquidacion.ObtieneLiquidacionesPorDepositar(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        //Si hay registros
                        if (mit != null)
                            //Agregando pendiente
                            notificaciones.Add(string.Format("{0} Pago(s) de Liquidación a Operador.", mit.Rows.Count));
                    }
                    //Pagos Proveedores
                    using (DataTable mit = SAT_CL.CXP.Reportes.ObtieneFacturasPorPagar((((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor)))
                    {
                        //Si hay registros
                        if (mit != null)
                            //Agregando pendiente
                            notificaciones.Add(string.Format("{0} Pago(s) a Proveedor.", mit.Rows.Count));
                    }
                }
            }

            //Si hay notificaciones
            if (notificaciones.Count > 0)
            {
                //Asignando total de notificaciones
                lblNotificacionesUsuario.Text = string.Format("{0} {1}", notificaciones.Count, notificaciones.Count > 1 ? "Notificaciones" : "Notificación");
                //Habilitando link
                lkbNotificacionesUsuario.Enabled = true;
                //Estilo de mensaje de notificaciones pendientes
                lblNotificacionesUsuario.CssClass = "label_negrita";
                //Mostrando mensaje final
                ScriptServer.MuestraNotificacion(this.Page, string.Join(@"   |   ", notificaciones), ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.Arriba);
            }
            //Si no hay notificaciones
            else
            {
                //Asignando total de notificaciones
                lblNotificacionesUsuario.Text = "Sin Notificaciones";
                //Estilo de mensaje de notificaciones pendientes
                lblNotificacionesUsuario.CssClass = "label";
                //Deshabilitando link
                lkbNotificacionesUsuario.Enabled = false;
            }

        }

        #endregion
    }
}