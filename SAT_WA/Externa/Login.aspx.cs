using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using SAT_CL.Seguridad;
using TSDK.ASP;
using TSDK.Base;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;
namespace SAT.Externa
{
    /// <summary>
    /// Clase encargada de todas las funcionalidades del Login de Acceso
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento genrado al carga la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Establecemos Boton Default
            Form.DefaultButton = btnAceptar.ClientID;
            //Asignamos Foco a control de nombre de usuario (email)
            txtUsuario.Focus();
            //autenticaUsuario("test123@tectos.com.mx", "123");


        }

        /// <summary>
        /// Evento generado al Iniciar Sesión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIniciaSesion_Click(object sender, EventArgs e)
        {
            ////
            //string URLacortada = "";
            ////Declarando Objeto de Retorno
            //RetornoOperacion result = new RetornoOperacion();
            //result = SAT_CL.Seguridad.UsuarioToken.GeneraNuevoTokenUUID(2015, 1126, 2015, out string tokenen);
            //URLacortada = TSDK.Google.Firebase.AcortarUrl(ConfigurationManager.AppSettings["SystemURI"].ToString() + "Externa/Login.aspx?ustk=" + tokenen);


            ////Determinando tipo de autenticación solicitada
            //switch (((Button)sender).CommandName)
            //{
            //    case "Usuario":
            //        //Autentica Usuario
            //        autenticaUsuario(txtUsuario.Text.Trim(), txtContrasena.Text);
            //        break;
            //}
        }
        /// <summary>
        /// Evento Generado al dar click en Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Iniciamos Sesión
            validaSeleccionCompania();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Autentica Usuario
        /// </summary>
        /// <param name="username"></param>
        /// <param name="contrasena"></param>
        private void autenticaUsuario(string username, string contrasena)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Usuario de acuerdo user name
            using (Usuario objUsuario = new Usuario(username))
            {
                //Autenticando Usuario
                resultado = objUsuario.AutenticaUsuario(contrasena);

                //Si la Autenticación es correcta
                if (resultado.OperacionExitosa)
                {
                    //Asignando variable de usuario en sesión
                    Session["usuario"] = objUsuario;

                    //Configurando tiempo de duración de la sesión 
                    Session.Timeout = objUsuario.tiempo_expiracion > 0 ? objUsuario.tiempo_expiracion : 255;

                    //Determinando a cuantas empresas tiene acceso el usuario autenticado
                    using (DataTable mit = UsuarioCompania.ObtieneCompaniasUsuario(objUsuario.id_usuario))
                    {
                        //Validando el origen de datos
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                        {
                            //Si sólo existe un registro de resultado
                            if (mit.Rows.Count == 1)
                            {
                                foreach (DataRow r in mit.Rows)
                                    //Inicializando sesión en compañía registrada
                                    resultado = iniciaSesion(Convert.ToInt32(r["IdCompaniaEmisorReceptor"]));
                            }
                            //Si hay posibilidad de más de una compañía
                            else
                            {
                                //Cargamos Catalogo de Compañia
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCompania, 13, "..........", resultado.IdRegistro, "", 0, "");
                                //Mostramos Vista para la Selección de Compañia
                                mtvInicioSesion.SetActiveView(vwCompania);
                                //Establecemos Botón Default
                                Form.DefaultButton = btnIniciaSesion.ClientID;
                                //Foco a control de compañías
                                ddlCompania.Focus();
                            }
                        }
                    }
                }

                //En caso de error
                if (!resultado.OperacionExitosa)
                    ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Iniciamos Sesion 
        /// </summary>
        private void validaSeleccionCompania()
        {
            //Declaramos Objeto Resultado con error
            RetornoOperacion resultado = new RetornoOperacion("Seleccione la Compañia");

            //Validamos Selección de Compañia
            if (ddlCompania.SelectedValue != "0")
                resultado = iniciaSesion(Convert.ToInt32(ddlCompania.SelectedValue));

            if (!resultado.OperacionExitosa)
                //Mostrando resultado con error
                ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Inicia sesión para compañía especificada
        /// </summary>
        /// <param name="id_compania">Id de Compania</param>
        /// <returns></returns>
        private RetornoOperacion iniciaSesion(int id_compania)
        {
            // Recuperamos la IP de la máquina del cliente
            // Primero comprobamos si se accede desde un proxy
            string ipAddress1 = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            // Acceso desde una máquina particular
            string ipAddress2 = Request.ServerVariables["REMOTE_ADDR"];
            //Recuperando la ip correspondiente
            string ipAddress = Cadena.RegresaElementoNoVacio(ipAddress1, ipAddress2);
            //Nombre de dispositivo
            string nomDispositivo = Request.ServerVariables["REMOTE_HOST"];
            //Recuperando información del navegador
            UsuarioSesion.TipoDispositivo tipoDispositivo = Request.Browser.IsMobileDevice ? UsuarioSesion.TipoDispositivo.Portatil : UsuarioSesion.TipoDispositivo.Escritorio;

            //Insertamos Sesión del Usuario
            RetornoOperacion resultado = UsuarioSesion.IniciaSesion(((Usuario)Session["usuario"]).id_usuario, id_compania, tipoDispositivo, ipAddress, nomDispositivo,
                                                                    ((Usuario)Session["usuario"]).id_usuario);

            //Si se Insertó Sessión 
            if (resultado.OperacionExitosa)
            {
                //Instanciamos Usuario Sesión para Asignar al Sesión
                using (UsuarioSesion objUsuarioSesion = new UsuarioSesion(resultado.IdRegistro))
                {
                    //Asignando variable de usuario
                    Session["usuario_sesion"] = objUsuarioSesion;

                    try
                    {
                        //Creando cookie con datos de inicio de sesión
                        HttpCookie Cookie = new HttpCookie("Login");

                        //Asignando llave de sesión
                        Cookie["ID"] = objUsuarioSesion.id_usuario_sesion.ToString();

                        //Añadiendo cookie
                        Response.Cookies.Add(Cookie);
                    }
                    catch (Exception ex)
                    {
                        //Registrando en log de eventos de la aplicacion
                        Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                    }

                    //Inicializamos Variables de Sesión
                    Pagina.InicializaVariablesSesion();

                    //Obteniendo Acceso por Defecto
                    string acceso = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Externa/Login.aspx", PerfilSeguridadUsuario.ObtieneAccesoPerfilActivo(objUsuarioSesion.id_usuario));

                    //Validando que exista
                    if (!acceso.Equals(""))
                    {
                        //Redireccionando a forma  por Default
                        FormsAuthentication.SetAuthCookie(((Usuario)Session["usuario"]).email, false);
                        Response.Redirect(acceso);
                    }
                    else
                        //Redireccionando a forma  por Default
                        FormsAuthentication.RedirectFromLoginPage(((Usuario)Session["usuario"]).email, false);
                }
            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }


        #endregion
    }
}