using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAT.MasterPage
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //Configurando enlaces dinámicos
            configuraEnlacesDinamicos();
            //Aplicando seguridad de página maestra
            SAT_CL.Seguridad.Forma.AplicaSeguridadPaginaMaestra(this, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        }
        /// <summary>
        /// Define los enlaces de páginas con contenido dinámico
        /// </summary>
        private void configuraEnlacesDinamicos()
        {
            //Historial de viajes
            lnkHistorialViajes.CommandName = string.Format("~/Accesorios/HistorialServicio.aspx?idRegistro={0}", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
        }
        /// <summary>
        /// Controla el direccionamiento del menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDireccionaModulo_Click(object sender, EventArgs e)
        {
            //Inicializando variables de sesión
            TSDK.ASP.Pagina.InicializaVariablesSesion();
            //referenciando al botón punlsado
            LinkButton lnk = (LinkButton)sender;

            //Determinando el tipo de apertura solicitada
            if (lnk.CommandArgument == "NuevaVentana")
            {
                //Obteniendo Ruta
                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta(this.Page.AppRelativeVirtualPath, lnk.CommandName);
                //Instanciando nueva ventana de navegador para apertura de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(urlReporte, "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1300,height=650", this.Page);
            }
            else
                //Direccionamos a la pagina destino
                Response.Redirect(lnk.CommandName);
        }
        /// <summary>
        /// Evento Producido al Seleccionar la Opción "Cambia Contraseña" del Control de Usuario "wucUsuarioAutenticado"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UsuarioAutenticado_ClickCambiarContrasena(object sender, EventArgs e)
        {
            //Inicializando Control
            ucCambioContrasena.InicializaControlUsuario();
            
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(UsuarioAutenticado, UsuarioAutenticado.GetType(), "CambioContrasena", "contenedorVentanaActualizaContrasena", "ventanaActualizaContrasena");
        }
        /// <summary>
        /// Evento Producido al Seleccionar la Opción "Cambia Perfil" del Control de Usuario "wucUsuarioAutenticado"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UsuarioAutenticado_ClickCambiaPerfil(object sender, EventArgs e)
        {
            //Inicializando Control
            ucPerfilUsuario.InicializaPerfilesUsuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(UsuarioAutenticado, UsuarioAutenticado.GetType(), "CambioPerfil", "contenedorVentanaActualizaPerfil", "ventanaActualizaPerfil");
        }
        /// <summary>
        /// Evento Producido al Seleccionar la Opción "Cambia Contraseña" del Control de Usuario "ucCambioContrasena"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucCambioContrasena_ClickCambiarContrasena(object sender, EventArgs e)
        {
            //Actualizando Contraseña
            ucCambioContrasena.ActualizaContrasenaUsuario();
        }
        /// <summary>
        /// Evento Producido al Cancelar el Cambio de Contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucCambioContrasena_ClickCancelar(object sender, EventArgs e)
        {
            //Cancelando Cambio de Contraseña
            ucCambioContrasena.CancelaContrasenaUsuario("CambioContrasena", "contenedorVentanaActualizaContrasena", "ventanaActualizaContrasena");
        }
        /// <summary>
        /// Evento Producido al Cancelar el Cambio de Perfil
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucPerfilUsuario_ClickCerrarVentana(object sender, EventArgs e)
        {
            //Cancelando Cambio de Contraseña
            ucPerfilUsuario.CancelaCambioPerfil("CambioPerfil", "contenedorVentanaActualizaPerfil", "ventanaActualizaPerfil");
        }
        
        #endregion

    }
}