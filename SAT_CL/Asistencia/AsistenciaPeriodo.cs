using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Asistencia
{
    public class AsistenciaPeriodo : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que alamcena el nombre del store procedure
        /// </summary>
        private static string _nom_sp = "asistencia.sp_asistencia_periodo_tap";

        private int _id_asistencia_periodo;
        /// <summary>
        /// ID que corresponde al identificador de asistencia periodo
        /// </summary>
        public int id_asistencia_periodo
        {
            get { return _id_asistencia_periodo; }
        }

        private int _id_compania;
        /// <summary>
        /// ID que corresponde al identificador de compania 
        /// </summary>
        public int id_compania
        {
            get { return _id_compania; }
        }

        private byte _id_estatus;
        /// <summary>
        /// ID que corresponde al identificador de estatus
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }

        private int _id_empleado;
        /// <summary>
        /// ID que corresponde al identificador de empleado
        /// </summary>
        public int id_empleado
        {
            get { return _id_empleado; }
        }

        private byte _id_periodo_tipo;
        /// <summary>
        /// ID que corresponde al identificador de periodo tipo
        /// </summary>
        public byte id_periodo_tipo
        {
            get { return _id_periodo_tipo; }
        }

        private DateTime _inicio;
        /// <summary>
        /// Corresponde al registro de fecha de inicio
        /// </summary>
        public DateTime inicio
        {
            get { return _inicio; }
        }

        private DateTime _fin;
        /// <summary>
        /// Corresponde al registro de fecha de fin
        /// </summary>
        public DateTime fin
        {
            get { return _fin; }
        }

        private int _id_usuario_autorizador;
        /// <summary>
        /// Id que Corresponde al usuario autorizador
        /// </summary>
        public int id_usuario_autorizador
        {
            get { return _id_usuario_autorizador; }
        }

        private string _comentarios;
        /// <summary>
        /// Corresponde al registro de comentarios
        /// </summary>
        public string comentarios
        {
            get { return _comentarios; }
        }

        private bool _habilitar;
        /// <summary>
        /// Corresponde al estado de habilitación de un registro de asistencia periodo
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
        public AsistenciaPeriodo()
        {
            this._id_asistencia_periodo = 0;
            this._id_compania = 0;
            this._id_estatus = 0;
            this._id_empleado = 0;
            this._id_periodo_tipo = 0;
            this._inicio = DateTime.MinValue;
            this._fin = DateTime.MinValue;
            this._id_usuario_autorizador = 0;
            this._comentarios ="";
            this._habilitar = false;
        }

        /// <summary>
        /// Cosntructor que inicializa los atributos a través de la busqueda de registros mediante un id
        /// </summary>
        /// <param name="id_asistencia_periodo">Id que sirve como referencia para la busqueda de registros</param>
        public AsistenciaPeriodo(int id_asistencia_periodo)
        {
            //Invoca al método privado cargaAtributoInstancia
            cargaAtributoInstancia(id_asistencia_periodo);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metódo privado que Carga los atributos dado un registro de asistencia periodo
        /// </summary>
        /// <param name="id_asistencia_periodo">Permite identificar el registro de asistencia periodo</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_asistencia_periodo)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            //Creación y Asignación de valores al arreglo, necesarios para el SP de la tabla 
            object[] param = { 3, id_asistencia_periodo, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
            //Invoca al store procedure de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validación de los datos, que existan en la tabla y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas y almacenamiento de registros en la variable dr 
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        this._id_asistencia_periodo = id_asistencia_periodo;
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_empleado = Convert.ToInt32(dr["IdEmpleado"]);
                        this._id_periodo_tipo = Convert.ToByte(dr["IdPeriodoTipo"]);
                        DateTime.TryParse(dr["Inicio"].ToString(), out _inicio);
                        DateTime.TryParse(dr["Fin"].ToString(), out _fin);
                        this._id_usuario_autorizador = Convert.ToInt32(dr["IdUsuarioAutorizador"]);
                        this._comentarios = Convert.ToString(dr["Comentarios"]);
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
        /// Método privado que permite actualizar  registros de Asistencia Periodo
        /// </summary>
        /// <param name="id_compania">Permite actualizar el identificador de compania</param>
        /// <param name="id_estatus">Permite actualizar el identificador de estatus</param>
        /// <param name="id_empleado">Permite actualizar el identificador de empleado</param>
        /// <param name="id_periodo_tipo">Permite actualizar el identificador de periodo tipo</param>
        /// <param name="inicio">Permite actualizar el registro de fecha de inicio</param>
        /// <param name="fin">Permite actualizar el registro de fecha fin</param>
        /// <param name="id_usuario_autorizador">Permite actualizar el identificador de usuario autorizador</param>
        /// <param name="comentarios">Permite actualizar el regsitro de comentario</param>
        /// <param name="id_usuario">Permite actualizar el identificador de usuario que modifica</param>
        /// <param name="habilitar">Permite actualizar si el regsitro se encunetra activo</param>
        /// <returns></returns>
        private RetornoOperacion editarAsistenciaPeriodo(int id_compania, byte id_estatus, int id_empleado, byte id_periodo_tipo, DateTime inicio,
                                                          DateTime fin, int id_usuario_autorizador, string comentarios, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 2, this._id_asistencia_periodo, id_compania,  id_estatus, id_empleado, id_periodo_tipo, TSDK.Base.Fecha.ConvierteDateTimeObjeto(inicio),
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fin), id_usuario_autorizador, comentarios,id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método público que permite insertar registros de Asistencia Periodo
        /// </summary>
        /// <param name="id_compania">Permite Insertar el identificador de compania</param>
        /// <param name="id_estatus">Permite Insertar el identificador de estatus</param>
        /// <param name="id_empleado">Permite Insertar el identificador de empleado</param>
        /// <param name="id_periodo_tipo">Permite Insertar el identificador de periodo tipo</param>
        /// <param name="inicio">Permite Insertar el registro de fecha de inicio</param>
        /// <param name="fin">Permite Insertar el registro de fecha fin</param>
        /// <param name="id_usuario_autorizador">Permite Insertar el identificador de usuario autorizador</param>
        /// <param name="comentarios">Permite Insertar el registro de comentario</param>
        /// <param name="id_usuario">Permite Insertar el identificador de usuario que modifica</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarAsistenciaPeriodo(int id_compania, byte id_estatus, int id_empleado, byte id_periodo_tipo, DateTime inicio,
                                                          DateTime fin, int id_usuario_autorizador, string comentarios, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 1, 0, id_compania, id_estatus, id_empleado, id_periodo_tipo, TSDK.Base.Fecha.ConvierteDateTimeObjeto(inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fin), id_usuario_autorizador, comentarios, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite Actualizar los registros de Asistencia Periodo
        /// </summary>
        /// <param name="id_compania">Permite Editar el identificador de compania</param>
        /// <param name="id_estatus">Permite Editar el identificador de estatus</param>
        /// <param name="id_empleado">Permite Editar el identificador de empleado</param>
        /// <param name="id_periodo_tipo">Permite Editar el identificador de periodo tipo</param>
        /// <param name="inicio">Permite Editar el registro de fecha de inicio</param>
        /// <param name="fin">Permite Editar el registro de fecha fin</param>
        /// <param name="id_usuario_autorizador">Permite Editar el identificador de usuario autorizador</param>
        /// <param name="comentarios">Permite Editar el registro de comentario</param>
        /// <param name="id_usuario">Permite Editar el identificador de usuario que modifica</param>
        /// <returns></returns>
        public RetornoOperacion EditarAsistenciaPeriodo(int id_compania, byte id_estatus, int id_empleado, byte id_periodo_tipo, DateTime inicio,
                                                          DateTime fin, int id_usuario_autorizador, string comentarios, int id_usuario)
        {
            //Retorna e Invoca el método privado editarAsistenciaPeriodo
            return this.editarAsistenciaPeriodo(id_compania, id_estatus, id_empleado, id_periodo_tipo, inicio, fin, id_usuario_autorizador, comentarios, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que Actualiza el estado de habilitación de un registro de Asistencia Periodo
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarAsistenciaPeriodo(int id_usuario)
        {
            //Retorna e Invoca al método privado editarAsistenciaPeriodo
            return this.editarAsistenciaPeriodo(this._id_compania, this._id_estatus, this._id_empleado, this._id_periodo_tipo, this._inicio, this._fin, this._id_usuario_autorizador, this._comentarios, id_usuario, false);
        }

        #endregion











    }
}
