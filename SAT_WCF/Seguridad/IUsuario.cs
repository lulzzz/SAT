using System.ServiceModel;

namespace SAT_WCF.Seguridad
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IUsuario" en el código y en el archivo de configuración a la vez.
    [ServiceContract(Namespace = "http://www.tectos.com.mx/SAT_NEXTIA/Seguridad/"), XmlSerializerFormat]
    public interface IUsuario
    {
        /// <summary>
        /// Realiza las validaciones necesarias sobre la cuenta de usuario indicada y permite el acceso remoto a la plataforma.
        /// </summary>
        /// <param name="email">Email registrado en cuenta de usuario activa</param>
        /// <param name="contrasena">Contraseña asignada por el usuario para su inicio de sesión</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato XML</returns>
        [OperationContract]
        string AutenticaUsuario(string email, string contrasena);
        /// <summary>
        /// Realiza el firmado del usuario sobre una compañía en particular
        /// </summary>
        /// <param name="id_usuario">Id de Usuario Autenticado</param>
        /// <param name="id_compania">Id de Compañía donde se firmará el usuario</param>
        /// <param name="tipo_dispositivo">Tipo de dispositivo desde donde se accesa (Consultar TipoDispositivo en contrato de servicio)</param>
        /// <param name="nombre_dispositivo">Nombre o alias del dispositivo</param>
        /// <param name="direccion_ip_mac">Dirección ipV6 o MAC del dispositivo</param>
        /// <param name="codigo_aut">Código de Autenticación</param>
        /// <param name="token_fcm">Token de dispositivo, registrado para Firebase Cloud Messaging</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato XML</returns>        
        [OperationContract]
        string IniciaSesion(int id_usuario, int id_compania, string tipo_dispositivo, string nombre_dispositivo, string direccion_ip_mac, string codigo_aut, string token_fcm);
        /// <summary>
        /// Método encargado de Validar la Existencia de un Usuario
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato XML</returns>
        [OperationContract]
        string ValidaUsuarioContrasena(string email, string contrasena);
        /// <summary>
        /// Método encargado de Finalizar la Sesión de un Usuario
        /// </summary>
        /// <param name="id_usuario_sesion">Sesión de un Usuario</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato XML</returns>
        [OperationContract]
        string FinalizaSesion(int id_usuario_sesion);
        /// <summary>
        /// Obtiene las compañías a las que está adscrito el usuario
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string ObtieneCompaniasUsuario(int id_usuario);
        /// <summary>
        /// Método encargado de Actualizar la Ultima Actividad del Usuario
        /// </summary>
        /// <param name="id_usuario_sesion">Sesión Activa</param>
        /// <param name="direccion_mac">Dirección MAC</param>
        /// <param name="nombre_dispositivo">Nombre del Dispositivo</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato XML</returns>
        [OperationContract]
        string ActualizaUltimaActividad(int id_usuario_sesion, string direccion_mac, string nombre_dispositivo);
        /// <summary>
        /// Actualiza el token de dispositivo, asignado para servicios FCM
        /// </summary>
        /// <param name="id_usuario_sesion">Id de Usuario sesion </param>
        /// <param name="token">Token más reciente asignado para uso de FCM</param>
        /// <returns></returns>
        [OperationContract]
        string ActualizaTokenFireBaseCloudMessaging(int id_usuario_sesion, string token);
    }
}
