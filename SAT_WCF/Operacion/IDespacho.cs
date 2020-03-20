using System.ServiceModel;

namespace SAT_WCF.Operacion
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IDespacho" en el código y en el archivo de configuración a la vez.
    [ServiceContract(Namespace = "http://www.tectos.com.mx/SAT_NEXTIA/Operacion/"), XmlSerializerFormat]
    public interface IDespacho
    {
        /// <summary>
        /// Método encargado de Obtener los Datos del Encabezado del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneDatosEncabezadoServicio(int id_servicio);
        /// <summary>
        /// Obtiene los datos del operador y unidad asignada a una sesión determinada
        /// </summary>
        /// <param name="id_usuario_sesion">Id de Sesion de usuario</param>
        /// <returns></returns>
        [OperationContract]
        string ObtieneUnidadAsignadaOperadorSesion(int id_usuario_sesion);
        /// <summary>
        /// Método encargado de Obtener los Datos de las Paradas del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// </summary>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneParadasServicioActivas(int id_servicio);

        /// <summary>
        /// Método encargado de Obtener las Paradas del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        [OperationContract]
        string ObtieneParadasServicio(int id_servicio);
        /// <summary>
        /// Método encargado de Obtener las Referencias del Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneReferenciasServicio(int id_servicio);
        /// <summary>
        /// Método encargado de Obtener los Accesorios de un Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneAccesoriosViaje(int id_servicio);
        /// <summary>
        /// Método encargado de Obtener los Anticipos de un Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneAnticiposViaje(int id_servicio);
        /// <summary>
        /// Método encargado de Obtener los Datos del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// </summary>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneDatosServicio(int id_servicio);
        /// <summary>
        /// Método encargado de Cargar los Servicios sin Liquidar de un Operador para la Aplicación Móvil
        /// </summary>
        /// <param name="id_usuario">Usuario de la Aplicación Móvil</param>
        /// <param name="tiempo">Especifica el Valor de Tiempo a Filtrar</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneServiciosOperador(int id_usuario, int tiempo);
        /// <summary>
        /// Método encargado de Cargar el Servicio Actual de un Operador para la Aplicación Móvil
        /// </summary>
        /// <param name="id_usuario">Usuario de la Aplicación Móvil</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ServicioActualOperador(int id_usuario);
        /// <summary>
        /// Método encargado de Determinar si el Viaje esta Listo para Iniciarse
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string IniciarViaje(int id_servicio, double latitud, double longitud, int id_sesion);
        /// <summary>
        /// Método encargado de asignar la Llegada a una Parada
        /// </summary>
        /// <param name="id_parada">Parada</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string LlegadaParada(int id_parada, double latitud, double longitud, int id_sesion);
        /// <summary>
        /// Método encargado de asignar la Salida de una Parada
        /// </summary>
        /// <param name="id_parada">Parada</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string SalidaParada(int id_parada, double latitud, double longitud, int id_sesion);
        /// <summary>
        /// Método encargado de Determinar si el Viaje esta Listo para Iniciarse
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string TerminarViaje(int id_servicio, double latitud, double longitud, int id_sesion);
        /// <summary>
        /// Método encargado de Obtener los Documentos del Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneDocumentosViaje(int id_servicio);
        /// <summary>
        /// Método encargado de Obtener el Evento Actual
        /// </summary>
        /// <param name="id_parada">Parada del Servicio</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ObtieneEventoActualParada(int id_parada);
        /// <summary>
        /// Método encargado de Iniciar el Evento
        /// </summary>
        /// <param name="id_evento">Evento</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string IniciarEvento(int id_evento, int id_sesion);
        /// <summary>
        /// Método encargado de Terminar el Evento
        /// </summary>
        /// <param name="id_evento">Evento</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string TerminarEvento(int id_evento, int id_sesion);
        /// <summary>
        /// Método encargado de Insertar una Devolución
        /// </summary>
        /// <param name="id_parada">Parada</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string InsertaDevolucion(int id_parada, int id_sesion);

        /// <summary>
        /// Método encargado de Insertar Referencias del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <param name="id_tipo_referencia">Tipo de Referencia</param>
        /// <param name="valor_referencia">Valor de la Referencia</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string InsertaReferenciaServicio(int id_servicio, int id_tipo_referencia, string valor_referencia, int id_sesion);
        /// <summary>
        /// Método encargado de Cargar los Conceptos de los Depositos
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string CargaConceptosDeposito(int id_servicio, int id_sesion);
        /// <summary>
        /// Método encargado de Solicitar el Deposito del Servicio
        /// </summary>
        /// <param name="id_concepto_deposito">Concepto de Deposito</param>
        /// <param name="concepto_restriccion">Concepto de Restricción</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="monto">Monto</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>RetornoOperacion en Formato XML</returns>
        [OperationContract]
        string SolicitaDepositoServicio(int id_concepto_deposito, string concepto_restriccion, string referencia, decimal monto, int id_servicio, int id_sesion);
        /// <summary>
        /// Método que actualiza la fecha de llegada un servicio despacho
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <param name="noservicio">No servicio</param>
        /// <param name="nombrecompania">Nombre de compania</param>
        /// <param name="ubicacion">Ubicacion</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="secuencia">Comentario</param>
        /// <param name="usuario">Sesión del Usuario que Actualiza el Registro</param>
        /// <param name="contrasena">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ActualizaLlegada(string fecha, string no_servicio, string nombre_compania, string ubicacion, double latitud, double longitud, int secuencia, string usuario, string contrasena);
        /// <summary>
        /// Método que actualiza la fecha de llegada un servicio despacho
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <param name="noservicio">No servicio</param>
        /// <param name="nombrecompania">Nombre de compania</param>
        /// <param name="ubicacion">Ubicacion</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="secuencia">Comentario</param>
        /// <param name="usuario">Sesión del Usuario que Actualiza el Registro</param>
        /// <param name="contrasena">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>System.Data.DataTable en formato XML</returns>
        [OperationContract]
        string ActualizaSalida(string fecha, string no_servicio, string nombre_compania, string ubicacion, double latitud, double longitud, int secuencia, string usuario, string contrasena);
    }
}
