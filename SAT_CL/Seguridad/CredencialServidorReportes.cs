using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Implementación de IReportServerCredentials para Autenticación Impersonal de Usuario en Servidor de Reportes (SSRS)
    /// </summary>
    public class CredencialServidorReportes : IReportServerCredentials
    {
        #region Atributos

        private string _userName;
        private string _password;
        private string _domain;

        /// <summary>
        /// Obtiene Interpretación de usuario
        /// </summary>
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                //Identidad Predeterminada
                return null;
            }
        }
        /// <summary>
        /// Obtiene las Credenciales de la Instancia
        /// </summary>
        public ICredentials NetworkCredentials
        {
            get
            {
                //Creando credencial de red a partir de usuario, dominio y contraseña
                return new NetworkCredential(_userName, _password, _domain);
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Crea una nueva instancia de autenticación con los criterios definidos
        /// </summary>
        /// <param name="usuario">Nombre de Usuario</param>
        /// <param name="contrasena">Contraseña</param>
        /// <param name="dominio">Dominio</param>
        public CredencialServidorReportes(string usuario, string contrasena, string dominio)
        {
            _userName = usuario;
            _password = contrasena;
            _domain = dominio;
        }

        #endregion

        #region Métodos Públicos
        
        /// <summary>
        /// Recupera las credenciales de autenticación
        /// </summary>
        /// <param name="cookie_autenticacion"></param>
        /// <param name="usuario"></param>
        /// <param name="contrasena"></param>
        /// <param name="autoridad"></param>
        /// <returns></returns>
        public bool GetFormsCredentials(out Cookie cookie_autenticacion, out string usuario, out string contrasena, out string autoridad)
        {
            // Do not use forms credentials to authenticate.
            cookie_autenticacion = null;
            usuario = contrasena = autoridad = null;
            return false;
        }

        #endregion
    }
}
