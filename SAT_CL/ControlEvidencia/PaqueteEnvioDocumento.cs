using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Linq;
using System.Transactions;

namespace SAT_CL.ControlEvidencia
{
    /// <summary>
    /// Clase Encargada de las Operaciones de los Paquetes de Envio de Documentos
    /// </summary>
    public class PaqueteEnvioDocumento : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_paquete_envio_documento_tped";

        private int _id_paquete_envio_documento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Paquete de Envio
        /// </summary>
        public int id_paquete_envio_documento
        {
            get { return this._id_paquete_envio_documento; }
        }

        private int _id_control_evidencia_documento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Documento de Evidencia
        /// </summary>
        public int id_control_evidencia_documento
        {
            get { return this._id_control_evidencia_documento; }
        }

        private int _id_paquete_envio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Paquete de Envio
        /// </summary>
        public int id_paquete_envio
        {
            get { return this._id_paquete_envio; }
        }

        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus
        /// </summary>
        public EstatusPaqueteEnvioDocumento id_estatus
        {
            get { return (EstatusPaqueteEnvioDocumento)this._id_estatus; }
        }

        /// <summary>
        /// Atributo encargado de almacenar el Estatus del Paquete Documento
        /// </summary>
        public EstatusPaqueteEnvioDocumento estatus
        {
            get { return (EstatusPaqueteEnvioDocumento)this._id_estatus; }
        }

        private DateTime _fecha_recepcion;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Recepción
        /// </summary>
        public DateTime fecha_recepcion
        {
            get { return this._fecha_recepcion; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Enumeraciones
        /// <summary>
        /// Define los estatus que puede tener el control de evidencias de viaje
        /// </summary>
        public enum EstatusPaqueteEnvioDocumento
        {
            /// <summary>
            /// Paquete documento Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Paquete documento en transito
            /// </summary>
            Transito,
            /// <summary>
            /// Paquete domuneto Recibido
            /// </summary>
            Recibido,
            /// <summary>
            /// Paquete documento en Aclaración 
            /// </summary>
            En_Aclaracion,
            /// <summary>
            /// No se recibio paquete documento
            /// </summary>
            No_Recibidos,
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public PaqueteEnvioDocumento()
        {   //Invocando Método de carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public PaqueteEnvioDocumento(int id_registro)
        {   //Invocando Método de carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~PaqueteEnvioDocumento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Inicializando Valores
            this._id_paquete_envio_documento = 0;
            this._id_control_evidencia_documento = 0;
            this._id_paquete_envio = 0;
            this._id_estatus = 0;
            this._fecha_recepcion = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Variable de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, null, 0, false, "", "" };
            //Obteniendo Tabla de Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la Tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo las Filas de la Tabla
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_paquete_envio_documento = id_registro;
                        this._id_control_evidencia_documento = Convert.ToInt32(dr["IdControlEvidenciaDocumento"]);
                        this._id_paquete_envio = Convert.ToInt32(dr["IdPaqueteEnvio"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        DateTime.TryParse(dr["FechaRecepcion"].ToString(), out _fecha_recepcion);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Asignando Resultado como Verdadero
                    result = true;
                }
            }//Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="id_control_evidencia_documento">Documento de Evidencia</param>
        /// <param name="id_paquete_envio">Paquete de Envio</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_control_evidencia_documento,
                                            int id_paquete_envio, EstatusPaqueteEnvioDocumento id_estatus,
                                            DateTime fecha_recepcion, int id_usuario,
                                            bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2, this._id_paquete_envio_documento, id_control_evidencia_documento,
                                        id_paquete_envio,(byte) id_estatus, Fecha.ConvierteDateTimeObjeto(fecha_recepcion), 
                                        id_usuario, habilitar, "", "" };
            //Obteniendo Resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Privado encargado de Insertar los Registros
        /// </summary>
        /// <param name="id_control_evidencia_documento">Documento de Evidencia</param>
        /// <param name="id_paquete_envio">Paquete de Envio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPaqueteEnvioDocumento(int id_control_evidencia_documento,
                                                                int id_paquete_envio, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1, 0, id_control_evidencia_documento, id_paquete_envio,(byte)EstatusPaqueteEnvioDocumento.Registrado, 
                                        null, id_usuario, true, "", "" };
            //Obteniendo Resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Editar los Registros
        /// </summary>
        /// <param name="id_control_evidencia_documento">Documento de Evidencia</param>
        /// <param name="id_paquete_envio">Paquete de Envio</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaPaqueteEnvioDocumento(int id_control_evidencia_documento,
                                                               int id_paquete_envio, EstatusPaqueteEnvioDocumento id_estatus,
                                                               DateTime fecha_recepcion, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_control_evidencia_documento, id_paquete_envio,
                                       id_estatus, fecha_recepcion, id_usuario,
                                        this._habilitar);
        }

        /// <summary>
        /// Método Público encargado Deshabilitar los Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPaqueteEnvioDocumento(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_control_evidencia_documento, this._id_paquete_envio,
                                          (EstatusPaqueteEnvioDocumento)this._id_estatus, this._fecha_recepcion, id_usuario,
                                           false);
        }

        /// <summary>
        /// Deshabilita todos los documentos ligados a la HI indicada
        /// </summary>
        /// <param name="id_paquete_envio">Id de Paquete envío</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaPaqueteEnvioDocumentos(int id_paquete_envio, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_paquete_envio);

             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando documentos
                using (DataTable mitDoc = CargaDocumentoDelPaquete(id_paquete_envio))
                {
                    //Si existen resultados
                    if (Validacion.ValidaOrigenDatos(mitDoc))
                    {
                        //Para cada uno de los registros
                        foreach (DataRow d in mitDoc.Rows)
                        {
                            //instanciando registro
                            using (PaqueteEnvioDocumento dp = new PaqueteEnvioDocumento(Convert.ToInt32(d["Id"])))
                            {
                                //Si el registro existe
                                if (dp.id_paquete_envio_documento > 0)
                                    //Deshabilitando Documento
                                    resultado = dp.DeshabilitaPaqueteEnvioDocumento(id_usuario);
                                else
                                    resultado = new RetornoOperacion(string.Format("Detalle 'ID: {0}' no encontrado.", Convert.ToInt32(d["Id"])));
                            }

                            //Si existe error
                            if (!resultado.OperacionExitosa)
                                //Saliendo de ciclo
                                break;
                        }
                    }
                }

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Validamos Transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPaqueteEnvioDocumento()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_paquete_envio_documento);
        }



        /// <summary>
        /// Actualiza Estatus del Paquete Envio Documento 
        /// </summary>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusPaqueteEnvioDocumento(EstatusPaqueteEnvioDocumento estatus, int id_usuario)
        {
              //Declaramos Variable resultado
               RetornoOperacion resultado = new RetornoOperacion();

              //Creamos la transacción 
               using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
               {
                   //Declaramos Id del Documento
                   int idDocumento = 0;

                   //Declaramos Variable para fecha de recepción del Documento
                   DateTime fecha_recepcion = this._fecha_recepcion;

                   //Validamos que el estatus sea Recibido o en Aclaración para la asignación de la fecha de recepción
                   if (estatus == EstatusPaqueteEnvioDocumento.Recibido || estatus == EstatusPaqueteEnvioDocumento.En_Aclaracion)
                   {
                       fecha_recepcion = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
                   }
                   //Actualiza Estatus del Paquete Envío documento
                   resultado = this.actualizaRegistros(this._id_control_evidencia_documento, this._id_paquete_envio, estatus,
                               fecha_recepcion, id_usuario, this._habilitar);

                   //Establecemos Id del Documento
                   idDocumento = resultado.IdRegistro;

                   //Validamos Actalizacion del Paquete Documento
                   if (resultado.OperacionExitosa)
                   {
                       //Instanciamos Paquete 
                       using (PaqueteEnvio objPaqueteEnvio = new PaqueteEnvio(this._id_paquete_envio))
                       {
                           //Actualizamos Estatus del Paquete
                           resultado = objPaqueteEnvio.ActualizaEstatusGeneralPaquete(id_usuario);

                           //Si se realizo correctamente las actualizaciones
                           if (resultado.OperacionExitosa)
                           {
                               resultado = new RetornoOperacion(idDocumento);
                           }

                       }
                   }

                   //Validamos Resultado
                   if (resultado.OperacionExitosa)
                   {
                       //Validamos Transacción
                       scope.Complete();
                   }
               }

            return resultado;

        }

    
        /// <summary>
        /// Carga Estatus Paquete Envio Documento
        /// </summary>
        public static void CargaEstatusPaqueteEnvioDocumento(int id_paquete_envio, out bool Registrado, out bool Transito, out bool Recibido,
                                                      out bool EnAclaracion, out bool NoRecibido)
        {

            Registrado = Transito = Recibido = EnAclaracion = NoRecibido = false;
            //Inicializamos arreglo de parametros
            object[] param = { 4, 0, 0, id_paquete_envio, 0, null, 0, false, "", "" };

            //Realizando carga de registros
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {

                Registrado = (from DataRow r in mit.Rows
                              where Convert.ToByte(r["Estatus"]) == 1
                              select Convert.ToBoolean(r["Valida"])).FirstOrDefault();
                Transito = (from DataRow r in mit.Rows
                            where Convert.ToByte(r["Estatus"]) == 2
                            select Convert.ToBoolean(r["Valida"])).FirstOrDefault();
                Recibido = (from DataRow r in mit.Rows
                            where Convert.ToByte(r["Estatus"]) == 3
                            select Convert.ToBoolean(r["Valida"])).FirstOrDefault();
                EnAclaracion = (from DataRow r in mit.Rows
                                where Convert.ToByte(r["Estatus"]) == 4
                                select Convert.ToBoolean(r["Valida"])).FirstOrDefault();
                NoRecibido = (from DataRow r in mit.Rows
                              where Convert.ToByte(r["Estatus"]) == 5
                              select Convert.ToBoolean(r["Valida"])).FirstOrDefault();
            }
        }


        /// <summary>
        /// Carga Documentos de un Paquete
        /// </summary>
        /// <param name="id_paquete"></param>
        /// <returns></returns>
        public static DataTable CargaDocumentoDelPaquete(int id_paquete)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 5, 0, 0, id_paquete, 0, null, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }


        /// <summary>
        /// Devuelve el último detalle de paquete asignado al documento de viaje solicitado
        /// </summary>
        /// <param name="id_control_evidencia_documento">Id de Documento de viaje</param>
        /// <returns></returns>
        public static PaqueteEnvioDocumento ObtieneUltimoDetallePaqueteDocumento(int id_control_evidencia_documento)
        { 
            //Definiendo objeto de retorno
            PaqueteEnvioDocumento dp = new PaqueteEnvioDocumento();

            //Definiendo parámetros de búsqueda
            object[] param = { 6, 0, id_control_evidencia_documento, 0, 0, null, 0, false, "", "" };

            //Realizando búsqueda
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            { 
                //Si existen resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                { 
                    //Para el resultado
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    { 
                        //Instanciando detalle de paquete
                        dp = new PaqueteEnvioDocumento(Convert.ToInt32(r["Id"]));
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return dp;
        }

        /// <summary>
        /// Devuelve los detalles de paquete asignado al documento 
        /// </summary>
        /// <param name="id_control_evidencia_documento">Id de Documento de viaje</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesPaqueteDocumento(int id_control_evidencia_documento)
        {
            //Definiendo objeto de retorno
            DataTable mit = new DataTable();

            //Definiendo parámetros de búsqueda
            object[] param = { 7, 0, id_control_evidencia_documento, 0, 0, null, 0, false, "", "" };

            //Realizando búsqueda
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existen resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Validando origen de datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                        //Asignando a objeto de retorno
                        mit = ds.Tables["Table"];
                }
            }

            //Devolviendo resultado
            return mit;
        }
        /// <summary>
        /// Método encargado de Validar si el Documento esta dentro de un Paquete
        /// </summary>
        /// <param name="id_control_evidencia_documento">Id de Documento de viaje</param>
        /// <returns></returns>
        public static bool ValidaDocumentoPaqueteDetalle(int id_control_evidencia_documento)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Definiendo parámetros de búsqueda
            object[] param = { 7, 0, id_control_evidencia_documento, 0, 0, null, 0, false, "", "" };

            //Realizando búsqueda
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existen resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Positivo
                    result = true;
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
