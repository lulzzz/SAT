using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Autorizacion
{
    /// <summary>
    /// Proprrciona los metodos para Administrar las autorizaciones realizadas carga Pendiente de acuerdo a un idRegistro, idTabla.
    /// </summary>
    public  class AutorizacionRealizadaCargaPendientes : Disposable
    {

        #region Atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "autorizacion.autorizacion_realizada_carga_pendientes";

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AutorizacionRealizadaCargaPendientes()
        {
            Dispose(false);
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Genera una instancia de tipo Autorizacion Realizada Carga Pendiente
        /// </summary>
        private AutorizacionRealizadaCargaPendientes()
        {
            
        }
        
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Obtiene el detalle de las autorizaciones sin confirmar de Tipo Depósito
        /// </summary>
        /// <param name="tipo_autorizacion">Tipo de Autorización</param>
        /// <param name="id_usuario_responsable">Id de Usuario Responsable</param>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <returns></returns>
        public static DataTable CargaAutorizacionesPendientes(Autorizacion.TipoAutorizacion tipo_autorizacion, int id_usuario_responsable, int id_compania_emisor)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Definiendo criterios de consulta
            object[] param = { tipo_autorizacion, id_usuario_responsable, id_compania_emisor, "" };

            //Realizando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si existen registraos en el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultados
                return mit;
            }
        }
        
        #endregion
    }
}
