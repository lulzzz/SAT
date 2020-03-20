using System.ServiceModel;

namespace SAT_WCF
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract(Namespace = "http://www.tectos.com.mx/SAT_NEXTIA/"), XmlSerializerFormat]
    public interface IHerramientasMovil
    {
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
        /// Método encargado de obtener el catálogo clasificado como general, solicitado a partir de su id
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
        /// Método Público encaragdo de Guardar los Archivos de cada Registro
        /// </summary>
        /// <param name="id_registro">Parametro que hace Referencia al Registro</param>
        /// <param name="id_tabla">Parametro que hace Referencia a la Tabla</param>
        /// <param name="id_compania">Compañia Emisora</param>
        /// <param name="id_archivo_tipo_configuracion">Tipo de Configuración del Archivo</param>
        /// <param name="referencia">Referencia del Archivo (Nombre)</param>
        /// <param name="archivo">Archivo en Formato Base 64</param>
        /// <param name="id_sesion">Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string GuardaArchivoRegistro(int id_registro, int id_tabla, int id_compania, int id_archivo_tipo_configuracion, string referencia, string archivo, int id_sesion);
        /// <summary>
        /// Método encargado de Obtener los Registros de Archivos Registro
        /// </summary>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Tipo de Configuración de Archivo</param>
        /// <returns></returns>
        [OperationContract]
        string CargaArchivosRegistro(string id_tabla, string id_registro, string id_archivo_tipo_configuracion);
        /// <summary>
        /// Método encargado de Actualizar el Archivo.
        /// </summary>
        /// <param name="id_archivo_registro">Parametro que hace referencia al Registro del Archivo</param>
        /// <param name="referencia">Referencia del Archivo (Nombre)</param>
        /// <param name="archivo">Archivo en Formato Base 64</param>
        /// <param name="id_sesion">Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string ActualizaArchivoRegistro(int id_archivo_registro, string referencia, string archivo, int id_sesion);
        /// <summary>
        /// Método encargado de Obtener el Archivo en Formato Base64 de un Registro
        /// </summary>
        /// <param name="id_archivo_registro">Registro del Archivo</param>
        /// <returns>Archivo en Formato XML</returns>
        [OperationContract]
        string ObtieneArchivoRegistroBase64(int id_archivo_registro);
        /// <summary>
        /// Método encargado de Eliminar el Archivo Registro
        /// </summary>
        /// <param name="id_archivo_registro">Registro del Archivo</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string EliminaArchivoRegistro(int id_archivo_registro, int id_sesion);
    }
}
