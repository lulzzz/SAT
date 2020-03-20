using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Asistencia
{
    public class AsistenciaEmpleado : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que alamcena el nombre del store procedure
        /// </summary>
        private static string _nom_sp = "asistencia.sp_asistencia_empleado_tae";

        private int _id_asistencia_empleado;
        /// <summary>
        /// ID que corresponde al identificador de asistencia empleado
        /// </summary>
        public int id_asistencia_empleado
        {
            get { return _id_asistencia_empleado; }
        }

        private int _id_compania;
        /// <summary>
        /// ID que corresponde al identificador de compania 
        /// </summary>
        public int id_compania
        {
            get { return _id_compania; }
        }

        private int _id_empleado;
        /// <summary>
        /// ID que corresponde al identificador de empleado
        /// </summary>
        public int id_empleado
        {
            get { return _id_empleado; }
        }


        private byte _id_estatus_entrada;
        /// <summary>
        /// ID que corresponde al identificador de estatus entrada
        /// </summary>
        public byte id_estatus_entrada
        {
            get { return _id_estatus_entrada; }
        }

        private byte _id_estatus_salida;
        /// <summary>
        /// ID que corresponde al identificador de estatus salida
        /// </summary>
        public byte id_estatus_salida
        {
            get { return _id_estatus_salida; }
        }

        private DateTime _fecha_entrada;
        /// <summary>
        ///  corresponde a la fecha de entrada 
        /// </summary>
        public DateTime fecha_entrada
        {
            get { return _fecha_entrada; }
        }

        private DateTime _fecha_salida;
        /// <summary>
        /// corresponde a la fecha de salida 
        /// </summary>
        public DateTime fecha_salida
        {
            get { return _fecha_salida; }
        }

        private int _id_horario;
        /// <summary>
        /// Id que corresponde al horario
        /// </summary>
        public int id_horario
        {
            get { return _id_horario; }
        }

        private int _periodo_laborado;
        /// <summary>
        /// Corresponde al periodo laborado
        /// </summary>
        public int periodo_laborado
        {
            get { return _periodo_laborado; }
        }

        private int _id_asistencia_periodo;
        /// <summary>
        /// ID que Corresponde al id de asistencia periodo
        /// </summary>
        public int id_asistencia_periodo
        {
            get { return _id_asistencia_periodo; }
        }

        private bool _habilitar;
        /// <summary>
        /// Corresponde al estado de habilitación de un registro de asistencia empleado
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default que inicializa a los atributos privados
        /// </summary>
        public AsistenciaEmpleado()
        {
            this._id_asistencia_empleado = 0;
            this._id_compania = 0;
            this._id_empleado = 0;
            this._id_estatus_entrada = 0;
            this._id_estatus_salida = 0;
            this._fecha_entrada = DateTime.MinValue;
            this._fecha_salida = DateTime.MinValue;
            this._id_horario = 0;
            this._id_asistencia_periodo = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Cosntructor que inicializa los atributos a través de la busqueda de registros mediante un id
        /// </summary>
        /// <param name="id_asistencia_empleado">Id que sirve como referencia para la busqueda de registros</param>
        public AsistenciaEmpleado(int id_asistencia_empleado)
        {
            //Invoca al método privado cargaAtributoInstancia
            cargaAtributoInstancia(id_asistencia_empleado);
        }

        #endregion

         #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~AsistenciaEmpleado()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados


        /// <summary>
        /// Metódo privado que Carga los atributos dado un registro de asistencia empleado
        /// </summary>
        /// <param name="id_asistencia_empleado">Permite identificar el registro de asistencia empleado</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_asistencia_empleado)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            //Creación y Asignación de valores al arreglo, necesarios para el SP de la tabla 
            object[] param = { 3, id_asistencia_empleado, 0, 0, 0, 0, null, null, 0, 0, 0, 0, false, "", "" };
            //Invoca al store procedure de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validación de los datos, que existan en la tabla y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas y almacenamiento de registros en la variable dr 
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        this._id_asistencia_empleado = id_asistencia_empleado;
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._id_empleado = Convert.ToInt32(dr["IdEmpleado"]);
                        this._id_estatus_entrada = Convert.ToByte(dr["IdEstatusEntrada"]);
                        this._id_estatus_salida = Convert.ToByte(dr["IdEstatusSalida"]);
                        DateTime.TryParse(dr["FechaEntrada"].ToString(), out _fecha_entrada);
                        DateTime.TryParse(dr["FechaSalida"].ToString(), out _fecha_salida);
                        this._id_horario = Convert.ToInt32(dr["IdHorario"]);
                        this._id_asistencia_periodo = Convert.ToInt32(dr["IdAsistenciaPeriodo"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno, siempre y cuando se cumpla la sentencia la validación de datos
                    retorno = true;
                }
            }
            //Retorno del objeto al método
            return retorno;
        }

        /// <summary>
        /// Método privado que permite actualizar  registros de Asistencia Empleado
        /// </summary>
        /// <param name="id_compania">Permite actualizar el identificador de compania</param>
        /// <param name="id_empleado">Permite actualizar el identificador de empleado</param>
        /// <param name="id_estatus_entrada">Permite actualizar el identificador de estatus entrada</param>
        /// <param name="id_estatus_salida">Permite actualizar el identificador de salida</param>
        /// <param name="fecha_entrada">Permite actualizar la fecha de entrada</param>
        /// <param name="fecha_salida">Permite actualizar la fecha de salida</param>
        /// <param name="id_horario">Permite actualizar el identificador de horario</param>
        /// <param name="id_asistencia_periodo">Permite actualizar el identificador de asistencia periodo</param>
        /// <param name="id_usuario">Permite actualizar el identificador de campo</param>
        /// <param name="habilitar">Permite actualizar si el registro se encuentra activo</param>
        /// <returns></returns>
        private RetornoOperacion editarAsistenciaEmpleado(int id_compania, int id_empleado, byte id_estatus_entrada, byte id_estatus_salida,
                                             DateTime fecha_entrada, DateTime fecha_salida, int id_horario, int id_asistencia_periodo,int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 2, this._id_asistencia_empleado, id_compania, id_empleado, id_estatus_entrada, id_estatus_salida, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_entrada),
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_salida), id_horario, 0, id_asistencia_periodo,id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos


        /// <summary>
        /// Método público que permite insertar registros de Asistencia Empleado
        /// </summary>
        /// <param name="id_compania">Permite insertar el identificador de compania</param>
        /// <param name="id_empleado">Permite insertar el identificador de empleado</param>
        /// <param name="id_estatus_entrada">Permite insertar el identificador de estatus entrada</param>
        /// <param name="id_estatus_salida">Permite insertar el identificador de salida</param>
        /// <param name="fecha_entrada">Permite insertar la fecha de entrada</param>
        /// <param name="fecha_salida">Permite insertar la fecha de salida</param>
        /// <param name="id_horario">Permite insertar el identificador de horario</param>
        /// <param name="id_asistencia_periodo">Permite insertar el identificador de asistencia periodo</param>
        /// <param name="id_usuario">Permite insertar el identificador de campo</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarAsistenciaEmpleado(int id_compania, int id_empleado, byte id_estatus_entrada, byte id_estatus_salida,
                                             DateTime fecha_entrada, DateTime fecha_salida, int id_horario, int id_asistencia_periodo, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 1, 0, id_compania, id_empleado, id_estatus_entrada, id_estatus_salida, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_entrada), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_salida), id_horario, 0, id_asistencia_periodo, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }


        /// <summary>
        /// Método que permite Actualizar los registros de Asistencia Empleado
        /// </summary>
        /// <param name="id_compania">Permite editar el identificador de compania</param>
        /// <param name="id_empleado">Permite editar el identificador de empleado</param>
        /// <param name="id_estatus_entrada">Permite editar el identificador de estatus entrada</param>
        /// <param name="id_estatus_salida">Permite editar el identificador de salida</param>
        /// <param name="fecha_entrada">Permite editar la fecha de entrada</param>
        /// <param name="fecha_salida">Permite editar la fecha de salida</param>
        /// <param name="id_horario">Permite editar el identificador de horario</param>
        /// <param name="id_asistencia_periodo">Permite editar el identificador de asistencia periodo</param>
        /// <param name="id_usuario">Permite editar el identificador de campo</param>
        /// <returns></returns>
        public RetornoOperacion EditarAsistenciaEmpleado(int id_compania, int id_empleado, byte id_estatus_entrada, byte id_estatus_salida,
                                             DateTime fecha_entrada, DateTime fecha_salida, int id_horario, int id_asistencia_periodo, int id_usuario)
        {
            //Retorna e Invoca el método privado editarAsistenciaEmpleado
            return this.editarAsistenciaEmpleado(id_compania, id_empleado, id_estatus_entrada, id_estatus_salida, fecha_entrada, fecha_salida, id_horario, id_asistencia_periodo, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que Actualiza el estado de habilitación de un registro de Asistencia Empleado
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarAsistenciaEmpleado(int id_usuario)
        {
            //Retorna e Invoca al método privado editarAsistenciaEmpleado
            return this.editarAsistenciaEmpleado(this._id_compania, this._id_empleado, this._id_estatus_entrada, this._id_estatus_salida, this._fecha_entrada, this._fecha_salida, this._id_horario, this._id_asistencia_periodo, id_usuario, false);
        }

        #endregion
    }
}
