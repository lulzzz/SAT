using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL
{
    /// <summary>
    /// Proporciona métodos para conectividad con la Base de Datos InterfazContpaqi2014.
    /// </summary>
    public sealed class CapaDatos
    {
        #region Atributos Privados

        /// <summary>
        /// Objeto capa de datos instanciado por la misma clase
        /// </summary>
        private static CapaDatos capaDeDatos = null;

        /// <summary>
        /// Cadena de conexión de la única instancia permitida
        /// </summary>
        private string _strCon;

        #endregion

        #region Atributos Públicos

        /// <summary>
        /// Accede a la única instancia permitida de la clase
        /// </summary>
        public static CapaDatos m_capaDeDatos
        {
            get
            {
                if (capaDeDatos == null)
                {
                    lock (new object())
                    {
                        if (capaDeDatos == null)
                            capaDeDatos = new CapaDatos();
                    }
                }
                return capaDeDatos;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        private CapaDatos()
        {
            //Asignando cadena de conexión sincrona correspondiente
            try
            {
                _strCon = ConfigurationManager.ConnectionStrings["TECTOS_SAT_db"].ConnectionString;
            }
            catch (Exception)
            {
                //Registrando en log de eventos de la aplicacion
                //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
            }
        }

        #endregion

        #region Configuración de Conexión y Comando para Stored Procedure

        /// <summary>
        /// Genera el comando correspondiente requerido para la ejecución del Stored Procedure solicitado
        /// </summary>
        /// <param name="db">Instancia de Base de Datos a la pertenece el Stored Procedure</param>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <returns></returns>
        private DbCommand generaComandoStoredProcedure(Database db, string nombreStoredProcedure, object[] valoresParametro)
        {
            //Declarando e inicilizando objeto de retorno
            DbCommand cmd = null;

            //Validando los datos necesarios
            if (db != null && !string.IsNullOrEmpty(nombreStoredProcedure) && valoresParametro != null)
            {
                try
                {
                    //Obteniendo los datos del Stored Procedure 
                    cmd = db.GetStoredProcCommand(nombreStoredProcedure, valoresParametro);
                    //Haciendo compatible los parametros del comando con tipo definidos por usuario(Geography)
                    compatibilidadUDT(cmd);
                    cmd.CommandTimeout = 600;
                }
                //Excepción producida por diferencia en No. de parámetros del SP y los proporcionados
                catch (InvalidOperationException ex)
                {
                    //Registrando excepción correspondiente
                    ex.ToString();
                }
                catch (Exception e)
                {
                    //Registrando en el EventLog la excepción producida al generar el comando de ejecución del SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                    //Registrando excepción correspondiente
                    e.ToString();
                }
            }

            //Devolvinedo el comendo generado
            return cmd;
        }
        /// <summary>
        /// Realiza la asignación de valores de los parámetros de un comando
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="valorParametros"></param>
        private void asignaValorParametrosComando(DbCommand cmd, object[] valorParametros)
        {
            //Verificando que el comando exista
            if (cmd != null && valorParametros != null)
            {
                //Definiendo indice de asignacón de valores (Se inicio en 1, dado que el indice 0 está reservado por SQL para el parametro @ReturnValue)
                int indice = 1;

                //Si existen más valores o la cantidad es igual a la cantidad de parámetros
                if (cmd.Parameters.Count - 1 <= valorParametros.Length)
                {
                    //Iterando hasta la cantidad de parámetros
                    for (indice = 1; indice < cmd.Parameters.Count; indice++)
                    {
                        //Asignando valor a parámetro
                        cmd.Parameters[indice].Value = valorParametros[indice - 1];
                    }
                }
                //Si no hace falta
                else
                {
                    //Iterando hasta la cantidad de valores
                    for (indice = 1; indice - 1 < valorParametros.Length; indice++)
                    {
                        //Asignando valor a parámetro
                        cmd.Parameters[indice].Value = valorParametros[indice - 1];
                    }
                    //Para el resto de parámetros
                    for (int i = indice; i < cmd.Parameters.Count; i++)
                    {
                        //Asignando valor nulo
                        cmd.Parameters[i].Value = DBNull.Value;//BibliotecaClasesBase.Herramientas.ObtieneValorDefaultTipoDeDato(cmd.Parameters[i].DbType.GetType());
                    }
                }
            }
        }


        /// <summary>
        /// Genera el comando correspondiente requerido para la ejecución del Stored Procedure solicitado
        /// </summary>
        /// <param name="db">Instancia de Base de Datos a la pertenece el Stored Procedure</param>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <param name="autocompletarParametros">True para indicar que el arreglo de objetos de valor de parámetro debe ser igualado a la cantidad de parámetros del SP.</param>
        /// <returns></returns>
        private DbCommand generaComandoStoredProcedure(Database db, string nombreStoredProcedure, object[] valoresParametro, bool autocompletarParametros)
        {
            //Declarando e inicilizando objeto de retorno
            DbCommand cmd = null;

            //Validando los datos necesarios
            if (db != null && !string.IsNullOrEmpty(nombreStoredProcedure) && valoresParametro != null)
            {
                try
                {
                    //Determinando si es requerido el ajuste automático de parámetros
                    if (autocompletarParametros)
                    {
                        //Recuperando listado de parámetros requeridos
                        cmd = db.GetStoredProcCommand(nombreStoredProcedure);
                        db.DiscoverParameters(cmd);
                        //Asignando valores de forma manual
                        asignaValorParametrosComando(cmd, valoresParametro);
                    }
                    else
                        //Obteniendo los datos del Stored Procedure y llenando parámetros
                        cmd = db.GetStoredProcCommand(nombreStoredProcedure, valoresParametro);
                }
                //Excepción producida por diferencia en No. de parámetros del SP y los proporcionados
                catch (InvalidOperationException)
                {
                    //Registrando excepción correspondiente

                }
                catch (Exception)
                {
                    //Registrando en el EventLog la excepción producida al generar el comando de ejecución del SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                }
            }

            //Devolvinedo el comendo generado
            return cmd;
        }

        /// <summary>
        /// Metodo encargado de hacer compatible el arreglo de parametros con Tipos Definidos por el usuario
        /// </summary>
        /// <param name="cmd"></param>
        private void compatibilidadUDT(DbCommand cmd)
        {
            //Recorremos el conjunto de parametros 
            foreach (DbParameter p in cmd.Parameters)
            {
                if (p.Value != null)
                {
                    switch (p.Value.GetType().Name)
                    {
                        //En caso de encontrar un parametro de tipo Geography
                        case "SqlGeography":
                            {
                                //Referenciamos el parametro a tipo SQL 
                                SqlParameter psql = (SqlParameter)p;
                                //Modificamos la propiedad UdtTypeName
                                psql.UdtTypeName = "Geography";
                                break;
                            }
                        //En caso de encontrar un parametro de tipo Geography
                        case "SqlGeometry":
                            {
                                //Referenciamos el parametro a tipo SQL 
                                SqlParameter psql = (SqlParameter)p;
                                //Modificamos la propiedad UdtTypeName
                                psql.UdtTypeName = "Geometry";
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Genera un arreglo de objetos a partir de los valores contenidos en el arreglo de parámetros de entrada
        /// </summary>
        /// <param name="parametrosComando">Parámetros de entrada</param>
        /// <returns></returns>
        private object[] valoresParametroComando(DbParameterCollection parametrosComando)
        {
            //Declarando e inicilizando objeto de retorno
            object[] valorParametros = null;

            //Si el arreglo de entrada no es nulo
            if (parametrosComando != null)
            {
                //Asignando dimensiones de arreglo de salida en base a la cantidad de parametros de entrada
                //Se omite el primer elemento de la lista de entrada dado que este elemento es el retornado por default
                valorParametros = new object[parametrosComando.Count - 1];

                //Declarando variable indexadora de arreglo de salida
                int indice = 0;

                //Para cada parámetro del arreglo de entrada
                for (int x = 1; x <= valorParametros.Length; x++)
                {
                    //Asignando su valor a su localidad correspondiente en el arreglo de salida
                    valorParametros[indice] = parametrosComando[x].Value;

                    //Incrementando indice de arreglo de salida
                    indice++;
                }
            }

            //Devolviendo valor nulo
            return valorParametros;
        }

        #endregion

        #region Transacciones SQL

        /// <summary>
        /// Método que inicializa una transacción de Transact-SQL
        /// </summary>
        /// <param name="nivelAislamiento">Nivel de aislamiento que regirá durante la ejecución de la transacción</param>
        /// <returns></returns>
        public SqlTransaction InicializaTransaccionSQL(IsolationLevel nivelAislamiento)
        {
            try
            {
                //Instanciando nueva cadena de conexion
                SqlConnection conexionBD = new SqlConnection(this._strCon);
                //Abriendo la conexión actual
                conexionBD.Open();

                //Declarando nueva transacción
                SqlTransaction transaccion = conexionBD.BeginTransaction(nivelAislamiento);

                //Devolviendo transacción inicializada
                return transaccion;
            }
            catch (Exception)
            {
                //Registrando en el EventLog la excepción producida al inicilizar la transacción
                //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);

                //En caso de algún error
                return null;
            }
        }

        /// <summary>
        /// Método que finaliza una transacción, estableciendo si fue o no exitosa
        /// </summary>
        /// <param name="transaccion">Transacción por finalizar</param>
        /// <param name="transaccionExitosa">Define si se aplicará un commit o rollback sobre la transacción</param>
        public void FinalizaTransaccionSQL(SqlTransaction transaccion, bool transaccionExitosa)
        {
            //Instanciando nueva conexión
            SqlConnection conexionBD = new SqlConnection();

            try
            {
                //Obteniendo la conexión utilizada por la transacción
                conexionBD = transaccion.Connection;

                //Validando exito de transacción
                //Si fue erronea
                if (!transaccionExitosa)
                    //Revocando todas las acciónes realizadas
                    transaccion.Rollback();
                //Si fue exitosa
                else
                    transaccion.Commit();
            }
            catch (Exception)
            {
                //Registrando en el EventLog la excepción producida al finalizar la transacción
                //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);

            }
            finally
            {
                //Cerrando conexión a BD
                conexionBD.Close();
            }
        }

        #endregion

        #region Ejecuta SP - DataSet

        /// <summary>
        /// Ejecuta el Stored Procedure solicitado de manera Sincrónica
        /// </summary>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <returns></returns>
        public DataSet EjecutaProcAlmacenadoDataSet(string nombreStoredProcedure, object[] valoresParametro)
        {
            //Referenciando a la BD indicada en la cadena de conexión
            Database db = new SqlDatabase(_strCon);

            //Generando el comando definido por el Stored Procedure
            DbCommand cmd = generaComandoStoredProcedure(db, nombreStoredProcedure, valoresParametro);

            //Si el commando fue creado correctamente
            if (cmd != null)
            {
                try
                {
                    //Devolvinedo DataSet obtenido
                    return db.ExecuteDataSet(cmd);
                }
                catch (Exception ex)
                {
                    //Registrando en el EventLog la excepción producida al ejecutar SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                    string excepcion = ex.Message;
                }
            }

            //Retornando valor nulo
            return null;
        }

        /// <summary>
        /// Ejecuta el Stored Procedure solicitado de manera Sincrónica, ligando la ejecución del SP a una transacción
        /// </summary>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <param name="transaccion">Transacción a la que será asociada la ejecución del Stored Procedure</param>
        /// <returns></returns>
        public DataSet EjecutaProcAlmacenadoDataSet(string nombreStoredProcedure, object[] valoresParametro, DbTransaction transaccion)
        {
            //Referenciando a la BD indicada en la cadena de conexión
            Database db = new SqlDatabase(_strCon);

            //Generando el comando definido por el Stored Procedure
            DbCommand cmd = generaComandoStoredProcedure(db, nombreStoredProcedure, valoresParametro);

            //Si el commando fue creado correctamente
            if (cmd != null)
            {
                try
                {
                    //Devolvinedo DataSet obtenido
                    return db.ExecuteDataSet(cmd, transaccion);
                }
                catch (Exception)
                {
                    //Registrando en el EventLog la excepción producida al ejecutar SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                }
            }

            //Retornando valor nulo
            return null;
        }

        /// <summary>
        /// Ejecuta el Stored Procedure solicitado de manera Sincrónica, autocompletando el arreglo de parámetros requeridos por el SP con valores por default en caso de ser necesario.
        /// </summary>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <returns></returns>
        public DataSet EjecutaProcAlmacenadoDataSetParametrosDefault(string nombreStoredProcedure, object[] valoresParametro)
        {
            //Referenciando a la BD indicada en la cadena de conexión
            Database db = new SqlDatabase(_strCon);

            //Generando el comando definido por el Stored Procedure
            DbCommand cmd = generaComandoStoredProcedure(db, nombreStoredProcedure, valoresParametro, true);

            //Si el commando fue creado correctamente
            if (cmd != null)
            {
                try
                {
                    //Devolvinedo DataSet obtenido
                    return db.ExecuteDataSet(cmd);
                }
                catch (Exception)
                {
                    //Registrando en el EventLog la excepción producida al ejecutar SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                }
            }

            //Retornando valor nulo
            return null;
        }

        /// <summary>
        /// Ejecuta el Stored Procedure solicitado de manera Sincrónica
        /// </summary>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <param name="actualizaParametrosSalida">True para reasignar arreglo de valores (parámetros de salida en SP), de lo contrario False</param>
        /// <returns></returns>
        public DataSet EjecutaProcAlmacenadoDataSet(string nombreStoredProcedure, object[] valoresParametro, bool actualizaParametrosSalida)
        {
            //Declarando e inicilizando DataSet de retorno
            DataSet ds = null;

            //Referenciando a la BD indicada en la cadena de conexión
            Database db = new SqlDatabase(_strCon);

            //Generando el comando definido por el Stored Procedure
            DbCommand cmd = generaComandoStoredProcedure(db, nombreStoredProcedure, valoresParametro);

            //Si el commando fue creado correctamente
            if (cmd != null)
            {
                try
                {
                    //Ejecutando el Stored Procedure
                    ds = db.ExecuteDataSet(cmd);

                    //Si se desea asignar parámetros de salida
                    if (actualizaParametrosSalida)
                        //Leyendo los parametros del commando y asignandolos al arreglo de salida
                        valoresParametro = valoresParametroComando(cmd.Parameters);
                }
                catch (Exception)
                {
                    //Registrando en el EventLog la excepción producida al ejecutar SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                }
            }

            //Retornando valor nulo
            return ds;
        }

        #endregion

        #region Ejecuta SP - Scalar

        /// <summary>
        /// Ejecuta el Stored Procedure solicitado. Devolvinedo un valor escalar. El SP se ejecuta de manera Sincrónica
        /// </summary>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <returns></returns>
        public RetornoOperacion EjecutaProcAlmacenadoObjeto(string nombreStoredProcedure, object[] valoresParametro)
        {
            //Referenciando a la BD indicada en la cadena de conexión
            Database db = new SqlDatabase(_strCon);

            //Declarando objeto de resultado
            RetornoOperacion resultado = null;

            //Generando el comando definido por el Stored Procedure
            DbCommand cmd = generaComandoStoredProcedure(db, nombreStoredProcedure, valoresParametro);

            //Si el commando fue creado correctamente
            if (cmd != null)
            {
                try
                {
                    //Devolvinedo DataSet obtenido
                    resultado = new RetornoOperacion(db.ExecuteScalar(cmd));
                }
                catch (Exception ex)
                {
                    //Instanciando Excepción Personalizada
                    resultado = new RetornoOperacion(string.Format("SP: {0}, Type: {1}, DB Exception: {2}", nombreStoredProcedure, valoresParametro[0], ex.Message));
                }
                finally
                {
                    //Validando Resultado Obtenido
                    if (!resultado.OperacionExitosa)
                    {
                        //Declarando Objeto de Gestión
                        string gest = "";

                        //Obteniendo Tipo de Consulta
                        switch ((int)valoresParametro[0])
                        {
                            case 1:
                                gest = "Insercción";
                                break;
                            case 2:
                                gest = "Actualización";
                                break;
                            default:
                                gest = "Consulta";
                                break;
                        }

                        //Instanciando Excepción Personalizada
                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("SP: {0}, Type: [{1}], Exception: {2}", nombreStoredProcedure, gest, resultado.Mensaje), false);
                    }
                }
            }

            //Retornando valor nulo
            return resultado;
        }

        /// <summary>
        /// Ejecuta el Stored Procedure solicitado, ligandolo a una transacción. Devolvinedo un valor escalar.
        /// </summary>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <param name="transaccion">Transacción a la que será asociada la ejecución del Stored Procedure</param>
        /// <returns></returns>
        public RetornoOperacion EjecutaProcAlmacenadoObjeto(string nombreStoredProcedure, object[] valoresParametro, DbTransaction transaccion)
        {
            //Referenciando a la BD indicada en la cadena de conexión
            Database db = new SqlDatabase(_strCon);

            //Declarando objeto de retorno
            RetornoOperacion resultado = null;

            //Generando el comando definido por el Stored Procedure
            DbCommand cmd = generaComandoStoredProcedure(db, nombreStoredProcedure, valoresParametro);

            //Si el commando fue creado correctamente
            if (cmd != null)
            {
                try
                {
                    //Devolvinedo DataSet obtenido
                    resultado = new RetornoOperacion(db.ExecuteScalar(cmd, transaccion));
                }
                catch (Exception)
                {
                    //Registrando en el EventLog la excepción producida al ejecutar SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                }
            }

            //Retornando valor nulo
            return resultado;
        }

        /// <summary>
        /// Ejecuta el Stored Procedure solicitado de manera Sincrónica. Devolvinedo un valor escalar.
        /// </summary>
        /// <param name="nombreStoredProcedure">Nombre del Stored Procedure</param>
        /// <param name="valoresParametro">Arreglo de valores que se asignarán a los parámetros del SP</param>
        /// <param name="actualizaParametrosSalida">True para reasignar arreglo de valores (parámetros de salida en SP), de lo contrario False</param>
        /// <returns></returns>
        public RetornoOperacion EjecutaProcAlmacenadoObjeto(string nombreStoredProcedure, object[] valoresParametro, bool actualizaParametrosSalida)
        {
            //Referenciando a la BD indicada en la cadena de conexión
            Database db = new SqlDatabase(_strCon);

            //Declarando objeto de retorno
            RetornoOperacion resultado = null;

            //Generando el comando definido por el Stored Procedure
            DbCommand cmd = generaComandoStoredProcedure(db, nombreStoredProcedure, valoresParametro);

            //Si el commando fue creado correctamente
            if (cmd != null)
            {
                try
                {
                    //Ejecutando el Stored Procedure
                    resultado = new RetornoOperacion(db.ExecuteScalar(cmd));

                    //Si se desea asignar parámetros de salida
                    if (actualizaParametrosSalida)
                        //Leyendo los parametros del commando y asignandolos al arreglo de salida
                        valoresParametro = valoresParametroComando(cmd.Parameters);
                }
                catch (Exception)
                {
                    //Registrando en el EventLog la excepción producida al ejecutar SP
                    //Excepcion.RegistraExcepcionEventLog(ConfigurationManager.AppSettings["EventLogSource"], ex);
                }
            }
            
            //Retornando valor nulo
            return resultado;
        }

        #endregion
    }
}

