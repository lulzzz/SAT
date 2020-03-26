using System.Runtime.Serialization;
using System.ServiceModel;

namespace SAT_WCF
{
    /// <summary>
    /// Interfáz para implementación de Herramientas de comunicación a plataforma principal SAT.
    /// </summary>
    [ServiceContract(Namespace = "http://www.tectos.com.mx/SAT_NEXTIA/"), XmlSerializerFormat]
    public interface IMobileService
    {
        /// <summary>
        /// Realiza las validaciones necesarias sobre la cuenta de usuario indicada y permite el acceso remoto a la plataforma.
        /// </summary>
        /// <param name="email">Email registrado en cuenta de usuario activa</param>
        /// <param name="contrasena">Contraseña asignada por el usuario para su inicio de sesión</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
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
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        [OperationContract]
        string IniciaSesion(int id_usuario, int id_compania, string tipo_dispositivo, string nombre_dispositivo, string direccion_ip_mac);
        /// <summary>
        /// Obtiene las compañías a las que está adscrito el usuario
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string ObtieneCompaniasUsuario(int id_usuario);
        /// <summary>
        /// Obtiene el catálogo solicitado a partir de su id y los criterios deseados
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string CargaCatalogo(int id_catalogo, string opcion_inicial);
        /// <summary>
        /// Obtiene el catálogo solicitado a partir de su id y los criterios deseados
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <param name="param1">Parámetro 1</param>
        /// <param name="param2">Parámetro 2</param>
        /// <param name="param3">Parámetro 3</param>
        /// <param name="param4">Parámetro 4</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string CargaCatalogoParametros(int id_catalogo, string opcion_inicial, int param1, string param2, int param3, string param4);
        /// <summary>
        /// Obtiene el catálogo clasificado como general, solicitado a partir de su id
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string CargaCatalogoGeneral(int id_catalogo, string opcion_inicial);
        /// <summary>
        /// Obtiene el catálogo clasificado como general, solicitado a partir de su id y un valor de un catálogo superior
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <param name="valor_superior">Valor del catálogo superior</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string CargaCatalogoGeneralParametros(int id_catalogo, string opcion_inicial, int valor_superior);
        /// <summary>
        /// Realiza el registro de un encabezado de acceso y sus detalles (entidades que accesan)
        /// </summary>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <param name="id_acceso">Id de Acceso del Patio (elemento de patio)</param>
        /// <param name="fecha_hora">Fecha y hora de acceso en formato yyyy-MM-ddTHH:mm:ss</param>
        /// <param name="entidades_xml">Conjunto de entidades que accesan al patio. Validar contra esquema correspondiente</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        [OperationContract]
        string RegistraEntradaPatio(int id_patio, int id_acceso, string fecha_hora, string entidades_xml, int id_usuario);
        /// <summary>
        /// Método Público encargado de Cargar las Posibles Sugerencias
        /// </summary>
        /// <param name="contextKey">Indica el Catalogo a Obtener</param>
        /// <param name="prefix">Contiene el Indicio de las Opciones a Elegir</param>
        /// <param name="complement1">Indica el Primer complemento en caso de requerirse</param>
        /// <param name="complement2">Indica el Segundo complemento en caso de requerirse</param>
        /// <param name="complement3">Indica el Tercer complemento en caso de requerirse</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string CargaConsultaSugerencia(int contextKey, string prefix, string complement1, string complement2, string complement3);
        /// <summary>
        /// Obtiene el Reporte de las Unidades que se encuentran en Patio
        /// </summary>
        /// <param name="descripcion">Descripción de la Unidad</param>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string ObtieneUnidadesDentro(string descripcion, int id_patio);
        /// <summary>
        /// Realiza el registro de un encabezado de acceso y actualizar sus detalles (entidades que salen)
        /// </summary>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <param name="id_acceso">Id de Acceso del Patio (elemento de patio)</param>
        /// <param name="fecha_hora">Fecha y hora de salida en formato yyyy-MM-ddTHH:mm:ss</param>
        /// <param name="entidades_xml">Conjunto de entidades que accesan al patio. Validar contra esquema correspondiente</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        [OperationContract]
        string RegistraSalidaPatio(int id_patio, int id_acceso, string fecha_hora, string entidades_xml, int id_usuario);
        /// <summary>
        /// Método Público encargado de Obtener la Instancia por Defecto del Usuario Y Compania
        /// </summary>
        /// <param name="id_usuario">Usuario</param>
        /// <returns>SAT_CL.ControlPatio.UsuarioPatio en formato xml Personalizado</returns>
        [OperationContract]
        string ObtienePatioDefaultUsuario(int id_usuario, int id_compania);
        /// <summary>
        /// Metodo que regresa los indicadores relacionados con las unidades en patio
        /// </summary>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string ObtieneIndicadoresAccesoPatio(int id_patio);
        /// <summary>
        /// Método Público encargado de Obtener las Unidades con Evento Pendientes de Confirmación
        /// </summary>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string ObtieneUnidadesEventosPendientes(int id_patio);
        /// <summary>
        /// Método Público encargado de Obtener los Eventos dado un Detalle de Acceso
        /// </summary>
        /// <param name="id_detalle_acceso">Detalle de Acceso</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        [OperationContract]
        string ObtieneEventosDetalleAcceso(int id_detalle_acceso);
        /// <summary>
        /// Método Público encargado de Confirmar el Evento
        /// </summary>
        /// <param name="id_evento">Id de Evento</param>
        /// <param name="tipo">Tipo de Confirmacion</param>
        /// <param name="fecha">Fecha y hora de actualizacion en formato yyyy-MM-ddTHH:mm:ss</param>
        /// <param name="id_usuario">Usuario que Actualiza el Evento</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        [OperationContract]
        string ActualizaConfirmacionEvento(int id_evento, byte tipo, string fecha, int id_usuario);
        /// <summary>
        /// Método Público encaragdo de Guardar los Archivos de cada Registro
        /// </summary>
        /// <param name="id_registro">Parametro que hace Referencia al Registro</param>
        /// <param name="id_tabla">Parametro que hace Referencia a la Tabla</param>
        /// <param name="id_compania">Compañia Emisora</param>
        /// <param name="id_archivo_tipo">Tipo de Archivo</param>
        /// <param name="referencia">Referencia del Archivo (Nombre)</param>
        /// <param name="archivo">Archivo en Formato Base 64</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        [OperationContract]
        string GuardaArchivoRegistro(int id_registro, int id_tabla, int id_compania, int id_archivo_tipo, string referencia, string archivo, int id_usuario);
    }

    /// <summary>
    /// Define los posibles Tipos de Dispositivos desde los que se puede aperturar una sesión. Definidos por SAT_CL.Seguridad.UsuarioSesion.TipoDispositivo.
    /// </summary>
    [DataContract(Namespace = "http://www.tectos.com.mx/")]
    public enum TipoDispositivo
    {
        /// <summary>
        /// Dispositivo no reconocido
        /// </summary>
        [EnumMember]
        Desconocido = 0,
        /// <summary>
        /// Equipo de Escritorio
        /// </summary>
        [EnumMember]
        Escritorio = 1,
        /// <summary>
        /// Equipo Portátil
        /// </summary>
        [EnumMember]
        Portatil,
        /// <summary>
        /// Dispositivo Android
        /// </summary>
        [EnumMember]
        Android
    }
}
