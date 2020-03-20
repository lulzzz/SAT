using System.ServiceModel;

namespace SAT_WCF.Monitoreo
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IBitacora" en el código y en el archivo de configuración a la vez.
    [ServiceContract(Namespace = "http://www.tectos.com.mx/SAT_NEXTIA/Monitoreo/"), XmlSerializerFormat]
    public interface IBitacora
    {
        /// <summary>
        /// Método encargado de Insertar una Bitacora de Monitoreo
        /// </summary>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string ReportaPeticionMonitoreo(double latitud, double longitud, int id_sesion);
        /// <summary>
        /// Método encargado de Reportar una Notificación del Operador
        /// </summary>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string ReportaNotificacionMonitoreo(double latitud, double longitud, int id_sesion);
        /// <summary>
        /// Método que inserta las Bitacoras de Monitoreo
        /// </summary>
        /// <param name="tipo">Tipo de Inidencia</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="comentario">Comentario</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string InsertaBitacoraMonitoreo(string tipo, double latitud, double longitud, string comentario, int id_sesion);
        /// <summary>
        /// Método encargado de Obtener el Reporte de Incidencias
        /// </summary>
        /// <param name="id_sesion">Sesión del Usuario que solicita información</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneReporteIncidencias(int id_sesion);
        /// <summary>
        /// Método que actualiza la fecha de llegada un servicio despacho
        /// </summary>
        /// <param name="tipo_incidencia">Tipo incidencia</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="noservicio">No servicio</param>
        /// <param name="nombrecompania">Nombre de compania</param>
        /// <param name="ubicacion">Ubicacion</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="unidad">Comentario</param>
        /// <param name="usuario">Sesión del Usuario que Actualiza el Registro</param>
        /// <param name="contrasena">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ActualizaIncidencia(string tipo_incidencia, string fecha, string no_servicio, string nombre_compania, string ubicacion, string unidad, int secuencia, double latitud, double longitud, string usuario, string contrasena);
    }
}
