using System.ServiceModel;

namespace SAT_WCF.ControlEvidencia
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IRecepcionDocumento" en el código y en el archivo de configuración a la vez.
    [ServiceContract(Namespace = "http://www.tectos.com.mx/SAT_NEXTIA/ControlEvidencia/"), XmlSerializerFormat]
    public interface IRecepcionDocumento
    {
        /// <summary>
        /// Método encargado de Insertar el Servicio Control Evidencia de Todos los Segmentos
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string InsertaServicioControlEvidencia(int id_servicio, int id_sesion);
        /// <summary>
        /// Método encargado de Insertar la Evidencia del Documento
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_servicio_ce">Servicio Control Evidencia</param>
        /// <param name="id_segmento">Segmento</param>
        /// <param name="id_segmento_ce">Segmento Control Evidencia</param>
        /// <param name="id_hoja_instruccion_doc">Documento de la Hoja de Instrucción</param>
        /// <param name="id_lugar_cobro">Lugar de Cobro</param>
        /// <param name="documento">Documento</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        [OperationContract]
        string InsertaControlEvidenciaDocumento(int id_servicio, int id_servicio_ce, int id_segmento, int id_segmento_ce,
                                                       int id_hoja_instruccion_doc, int id_lugar_cobro, string documento,
                                                       int id_sesion);
    }
}
