using System;
using System.Data;
using TSDK.Base;
using System.Transactions;
using TSDK.Datos;

namespace SAT_CL.Facturacion
{
   public class ProcesoDias : Disposable
    {
        #region Atributos
            /// <summary>
            /// Atributo encargado de Almacenar el Nombre del SP
            /// </summary>
        private static string _nom_sp = "facturacion.sp_proceso_dias_tpd";

            /// <summary>
            /// Atributo encargado de Almacenar la Relacion Entre Procesos y Dias
            /// </summary>
        private int _id_proceso_dias;
        public int id_proceso_dias { get { return this._id_proceso_dias; } }

            /// <summary>
            /// Atributo encargado de Almacenar la Relacion Entre Cliente y Proceso
            /// </summary>
        private int _id_cliente_proceso;
        public int id_cliente_proceso { get { return this._id_cliente_proceso; } }

            /// <summary>
            /// Atributo encargado de Almacenar ID del Dia
            /// </summary>
        private int _id_dia;
        public int id_dia { get { return this._id_dia; } }

            /// <summary>
            /// Atributo encargado de Almacenar Hora de Inicio
            /// </summary>
        private TimeSpan _hora_inicio;
        public TimeSpan hora_inicio { get { return this._hora_inicio; } }

            /// <summary>
            /// Atributo encargado de Almacenar Hora de Termino
            /// </summary>
        private TimeSpan _hora_termino;
        public TimeSpan hora_termino { get { return this._hora_termino; } }

            /// <summary>
            /// Atributo encargado de Almacenar Numero de dias
            /// </summary>
        private int _numero_dias;
        public int numero_dias { get { return this._numero_dias; } }

            /// <summary>
            /// Atributo encargado de Almacenar Habilitar
            /// </summary>
        private bool _habilitar;
        public bool habilitar { get { return this._habilitar; } }

        #endregion 

        #region Constructor
       //Constructor para inicializar atributos la clase con valores default
        public ProcesoDias()
        {
            this._id_proceso_dias = 0;
            this._id_cliente_proceso = 0;
            this._id_dia = 0;
            this._hora_inicio = new TimeSpan(00, 00, 00);
            this._hora_termino = new TimeSpan(00, 00, 00);
            this._numero_dias = 0;
            this._habilitar = false;
        }

       //Constructor para inicializar atribuos de la clase con valores de un ID especifico
        public ProcesoDias(int id_proceso_dias)
        {
            //Invoca el metodo cargaAtributosInstancia pasandole por parametro un ID especifico
            cargaAtributosInstancia(id_proceso_dias);
      
        }
        #endregion 

        #region Destructor

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ProcesoDias()
        {
            Dispose(false);
        }

        #endregion 

        #region Metodos Privados
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_cliente_proceso">Id del cliente que manejara el proceso</param>
        /// <param name="id_dia">Id del dia de semana</param>
        /// <param name="id_usuario">Usuario logueado</param>
        /// <param name="hora_inicio">Hora inicio</param>
        /// <param name="hora_termino">Hora Termino</param>
        /// <param name="numero_dias">Numero dias mes</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_cliente_proceso, int id_dia, int id_usuario,
                                                     TimeSpan hora_inicio, TimeSpan hora_termino, int numero_dias, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_proceso_dias, id_cliente_proceso, id_dia , id_usuario, hora_inicio, hora_termino, numero_dias, habilitar};

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de recuperar un registro y asignar valores a atributos de la clase dado un ID especifico
        /// </summary>
        /// <param name="id_proceso_dias">Id de registro para recuperar atributos</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_proceso_dias)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_proceso_dias, 0, 0, 0, new TimeSpan(00, 00, 00) , new TimeSpan(00, 00, 00), 0, false };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores

                        this._id_proceso_dias = id_proceso_dias;
                        this._id_cliente_proceso = Convert.ToInt32(dr["IdClienteProceso"]);
                        this._id_dia = Convert.ToInt32(dr["IdDia"]);
                        this._hora_inicio = TimeSpan.Parse(dr["HoraInicio"].ToString());
                        this._hora_termino = TimeSpan.Parse(dr["HoraTermino"].ToString());
                        this._numero_dias = Convert.ToInt32(dr["NumeroDeDias"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Resultado Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion 

        #region Metodos Publicos

        /// <summary>
        /// Método encargado de insertarregistros en en BD
        /// </summary>
        /// <param name="id_cliente_proceso">Id del cliente que manejara el proceso</param>
        /// <param name="id_dia">Id del dia de semana</param>
        /// <param name="id_usuario">Usuario logueado</param>
        /// <param name="hora_inicio">Hora inicio</param>
        /// <param name="hora_termino">Hora Termino</param>
        /// <param name="numero_dias">Numero dias mes</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaProcesoDias(int id_cliente_proceso, int id_dia, int id_usuario,
                                                         TimeSpan hora_inicio, TimeSpan hora_termino, int numero_dias)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_cliente_proceso, id_dia, id_usuario, hora_inicio , hora_termino, numero_dias, true };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de deshabilitar un registro en BD
        /// </summary>
        /// <param name="id_usuario">Usuario logueado</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaProcesoDias(int id_usuario)
        {
            return actualizaRegistroBD(this._id_cliente_proceso, this._id_dia, id_usuario, this.hora_inicio, this.hora_termino, this.numero_dias, false);
        }

        /// <summary>
        /// Método encargado de actualizar un registro en BD
        /// </summary>
        /// <param name="id_cliente_proceso">Id del cliente que manejara el proceso</param>
        /// <param name="id_dia">Id del dia de semana</param>
        /// <param name="id_usuario">Usuario logueado</param>
        /// <param name="hora_inicio">Hora inicio</param>
        /// <param name="hora_termino">Hora Termino</param>
        /// <param name="numero_dias">Numero dias mes</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaRegistroBD(int id_cliente_proceso, int id_dia, int id_usuario,
                                                     TimeSpan hora_inicio, TimeSpan hora_termino, int numero_dias){
            return actualizaRegistroBD(id_cliente_proceso, id_dia, id_usuario, hora_inicio, hora_termino, numero_dias, this.habilitar);

        }


        /// <summary>
        /// Método encargado de recuperar registros de una tabla dandole un id especifco
        /// </summary>
        /// <param name="id_cliente_proceso">Id del cliente que manejara el proceso</param>
        /// <returns></returns>
        public static DataTable ObtieneProcesoDias(int id_cliente_proceso)
        {
            //Creación de la tabla que almacenara los registros obtenidos
            DataTable dtProcesoDias = null;
            //Creación del areglo que realiza la consulta 
            object[] param = { 4, 0, id_cliente_proceso, 0, 0, new TimeSpan(00, 00, 00), new TimeSpan(00, 00, 00), 0, false };
            //Almacena en un Dataset el resultado de la consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla loa valores del DS
                    dtProcesoDias = DS.Tables["Table"];
            }
            //Devuelve el resultado al método
            return dtProcesoDias;
        }


        /// <summary>
        /// Método encargado de verificar como se esta guardando un proceso de revision, si es po dia de la semana o dia del mes
        /// </summary>
        /// <param name="id_cliente_proceso">Id del cliente que manejara el proceso</param>
        /// <returns></returns>
        public static int VerificaDiaSemanaODiaMes(int id_cliente_proceso)
        {
            //Declarando Objeto de Retorno
            int resultado = 0;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_cliente_proceso, 0, 0, new TimeSpan(00, 00, 00), new TimeSpan(00, 00, 00), 0, false };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        
                        //Si dia de la semana esta en cero a la variable resultado le asignamos un 1
                        if (Convert.ToInt32(dr["DiaSemana"]) == 0)
                        {
                            resultado = 1;
                        }

                        //Si dia del mes esta en cero a la variable resultado le asignamos un 1
                        if (Convert.ToInt32(dr["DiaMes"]) == 0)
                        {
                            resultado = 2;
                        }

                    }
                }
             
            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }

        /// <summary>
        /// Metodo encargado de deshabilitar todos los registros ligados a un cliente con un determinado proceso de revision
        /// </summary>
        /// <param name="id_cliente_proceso">Id del cliente que manejara el proceso</param>
        /// <param name="id_cliente_proceso">Usuario logueado</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaRegistrosLigadosAUnCliente(int Id_Cliente_Proceso, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos Registros Para Deshabilitar
                using (DataTable mitConceptos = CargarRegistrosParaDeshabilitar(Id_Cliente_Proceso))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mitConceptos))
                    {
                        //Recorremos los registros
                        foreach (DataRow r in mitConceptos.Rows)
                        {
                            //Validamos Origen de Datos
                            if (resultado.OperacionExitosa)
                            {

                                //Instanciamos la clase Proceso 
                                using (ProcesoDias pd = new ProcesoDias(r.Field<int>("Id")))
                                {
                                    //Deshabilitamos Registro
                                    resultado = pd.DeshabilitaProcesoDias(id_usuario);
                                }
                               
                            }
                            else
                                //Salimos del ciclo
                                break;
                        }

                        //Si las Operaciones fueron exitosas
                        if (resultado.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Metodo encargado cargar los registros que se van a deshabilitar
        /// </summary>
        /// <param name="id_cliente_proceso">Id del cliente que manejara el proceso</param>
        /// <returns></returns>
        public static DataTable CargarRegistrosParaDeshabilitar(int Id_Cliente_Proceso)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando arreglo de parametros
            object[] param = { 6, 0, Id_Cliente_Proceso, 0, 0, new TimeSpan(00, 00, 00), new TimeSpan(00, 00, 00), 0, false };
            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
       
        #endregion 

    }
}
