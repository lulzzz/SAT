using System;
using System.Data;
using TSDK.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAT_CL.Asistencia
{
    public class AsistenciaHorario : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo que alamcena el nombre del store procedure
        /// </summary>
        private static string _nom_sp = "asistencia.sp_asistencia_horario_th";

        private int _id_horario;
        /// <summary>
        /// ID que corresponde al identificador de horario
        /// </summary>
        public int id_horario
        {
            get { return _id_horario; }
        }

        private TimeSpan _inicio_hrs;
        /// <summary>
        /// Corresponde al registro  de hr de inicio
        /// </summary>
        public TimeSpan inicio_hrs
        {
            get { return _inicio_hrs; }
        }

        private TimeSpan _fin_hrs;
        /// <summary>
        /// Corresponde al registro  de hr de fin 
        /// </summary>
        public TimeSpan fin_hrs
        {
            get { return _fin_hrs; }
        }

        private byte _tolerancia_entrada;
        /// <summary>
        /// Corresponde al registro  de tolerancia de entrada
        /// </summary>
        public byte tolerancia_entrada
        {
            get { return _tolerancia_entrada; }
        }

        private bool _bit_lunes;
        /// <summary>
        /// Corresponde al registro de bit lunes
        /// </summary>
        public bool bit_lunes
        {
            get { return _bit_lunes; }
        }

        private bool _bit_martes;
        /// <summary>
        /// Corresponde al registro de bit martes
        /// </summary>
        public bool bit_martes
        {
            get { return _bit_martes; }
        }

        private bool _bit_miercoles;
        /// <summary>
        /// Corresponde al registro de bit miercoles
        /// </summary>
        public bool bit_miercoles
        {
            get { return _bit_miercoles; }
        }

        private bool _bit_jueves;
        /// <summary>
        /// Corresponde al registro de bit jueves
        /// </summary>
        public bool bit_jueves
        {
            get { return _bit_jueves; }
        }

        private bool _bit_viernes;
        /// <summary>
        /// Corresponde al registro de bit viernes
        /// </summary>
        public bool bit_viernes
        {
            get { return _bit_viernes; }
        }

        private bool _bit_sabado;
        /// <summary>
        /// Corresponde al registro de bit sabado
        /// </summary>
        public bool bit_sabado
        {
            get { return _bit_sabado; }
        }

        private bool _bit_domingo;
        /// <summary>
        /// Corresponde al registro de bit domingo
        /// </summary>
        public bool bit_domingo
        {
            get { return _bit_domingo; }
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
        public AsistenciaHorario()
        {
            this._id_horario = 0;
            this._inicio_hrs = TimeSpan.MinValue;
            this._fin_hrs = TimeSpan.MinValue;
            this._tolerancia_entrada = 0;
            this._bit_lunes = false;
            this._bit_martes = false;
            this._bit_miercoles = false;
            this._bit_jueves = false;
            this._bit_viernes = false;
            this._bit_sabado = false;
            this._bit_domingo = false;
            this._habilitar = false;
        }

        /// <summary>
        /// Cosntructor que inicializa los atributos a través de la busqueda de registros mediante un id
        /// </summary>
        /// <param name="_id_horario">Id que sirve como referencia para la busqueda de registros</param>
        public AsistenciaHorario(int id_horario)
        {
            //Invoca al método privado cargaAtributoInstancia
            cargaAtributoInstancia(id_horario);
        }



        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~AsistenciaHorario()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metódo privado que Carga los atributos dado un registro de horario
        /// </summary>
        /// <param name="id_horario">Permite identificar el registro de horario</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_horario)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            //Creación y Asignación de valores al arreglo, necesarios para el SP de la tabla 
            object[] param = { 3, id_horario, null, null, 0, false, false, false, false, false, false, false, 0, false, "", "" };
            //Invoca al store procedure de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validación de los datos, que existan en la tabla y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas y almacenamiento de registros en la variable dr 
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        this._id_horario = id_horario;
                        TimeSpan.TryParse(dr["InicioHrs"].ToString(), out this._inicio_hrs);
                        TimeSpan.TryParse(dr["FinHrs"].ToString(), out this._fin_hrs);
                        this._tolerancia_entrada = Convert.ToByte(dr["ToleranciaEntrada"]);
                        this._bit_lunes = Convert.ToBoolean(dr["BitLunes"]);
                        this._bit_martes = Convert.ToBoolean(dr["BitMartes"]);
                        this._bit_miercoles = Convert.ToBoolean(dr["BitMiercoles"]);
                        this._bit_jueves = Convert.ToBoolean(dr["BitJueves"]);
                        this._bit_viernes = Convert.ToBoolean(dr["BitViernes"]);
                        this._bit_sabado = Convert.ToBoolean(dr["BitSabado"]);
                        this._bit_domingo = Convert.ToBoolean(dr["BitDomingo"]);
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
        /// Método privado que permite actualizar  registros de Asistencia horario
        /// </summary>
        /// <param name="inicio_hrs">Permite actualizar el registro de hora de inicio</param>
        /// <param name="fin_hrs">Permite actualizar el registro de hora de fin</param>
        /// <param name="tolerancia_entrada">Permite actualizar el registro de tolerancia de entrada</param>
        /// <param name="bit_lunes">Permite actualizar el registro de bit lunes</param>
        /// <param name="bit_martes">Permite actualizar el registro de bit martes</param>
        /// <param name="bit_miercoles">Permite actualizar el registro de bit miercoles</param>
        /// <param name="bit_jueves">Permite actualizar el registro de bit jueves</param>
        /// <param name="bit_viernes">Permite actualizar el registro de bit viernes</param>
        /// <param name="bit_sabado">Permite actualizar el registro de bit sabado</param>
        /// <param name="bit_domingo">Permite actualizar el registro de bit domingo</param>
        /// <param name="id_usuario">Permite actualizar el identificador del ultimo usuaruio que modifica</param>
        /// <param name="habilitar">Permite actualizar si el registro se encuentra activo</param>
        /// <returns></returns>
        private RetornoOperacion editaAsistenciaHorario(TimeSpan inicio_hrs, TimeSpan fin_hrs, byte tolerancia_entrada, bool bit_lunes, bool bit_martes, bool bit_miercoles,
                                             bool bit_jueves, bool bit_viernes, bool bit_sabado, bool bit_domingo, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 2, this._id_horario, TSDK.Base.Fecha.ConvierteTimeSpanObjeto(inicio_hrs), TSDK.Base.Fecha.ConvierteTimeSpanObjeto(fin_hrs), tolerancia_entrada, bit_lunes, bit_martes,
                               bit_miercoles, bit_jueves, bit_viernes, bit_sabado, bit_domingo, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que permite Actualizar los registros de Asistencia Horario
        /// </summary>
        /// <param name="inicio_hrs">Permite insertar el registro de hora de inicio</param>
        /// <param name="fin_hrs">Permite insertar el registro de hora de fin</param>
        /// <param name="tolerancia_entrada">Permite insertar el registro de tolerancia de entrada</param>
        /// <param name="bit_lunes">Permite insertar el registro de bit lunes</param>
        /// <param name="bit_martes">Permite insertar el registro de bit martes</param>
        /// <param name="bit_miercoles">Permite insertar el registro de bit miercoles</param>
        /// <param name="bit_jueves">Permite insertar el registro de bit jueves</param>
        /// <param name="bit_viernes">Permite insertar el registro de bit viernes</param>
        /// <param name="bit_sabado">Permite insertar el registro de bit sabado</param>
        /// <param name="bit_domingo">Permite insertar el registro de bit domingo</param>
        /// <param name="id_usuario">Permite insertar el identificador del ultimo usuaruio que modifica</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarAsistenciaHorario(TimeSpan inicio_hrs, TimeSpan fin_hrs, byte tolerancia_entrada, bool bit_lunes, bool bit_martes, bool bit_miercoles,
                                             bool bit_jueves, bool bit_viernes, bool bit_sabado, bool bit_domingo, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 1, 0, TSDK.Base.Fecha.ConvierteTimeSpanObjeto(inicio_hrs), TSDK.Base.Fecha.ConvierteTimeSpanObjeto(fin_hrs), tolerancia_entrada, bit_lunes, bit_martes, bit_miercoles, bit_jueves, bit_viernes, bit_sabado, bit_domingo, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método publico que permite Editar los registros de Asistencia Horario
        /// </summary>
        /// <param name="inicio_hrs">Permite Editar el registro de hora de inicio</param>
        /// <param name="fin_hrs">Permite Editar el registro de hora de fin</param>
        /// <param name="tolerancia_entrada">Permite Editar el registro de tolerancia de entrada</param>
        /// <param name="bit_lunes">Permite Editar el registro de bit lunes</param>
        /// <param name="bit_martes">Permite Editar el registro de bit martes</param>
        /// <param name="bit_miercoles">Permite Editar el registro de bit miercoles</param>
        /// <param name="bit_jueves">Permite Editar el registro de bit jueves</param>
        /// <param name="bit_viernes">Permite Editar el registro de bit viernes</param>
        /// <param name="bit_sabado">Permite Editar el registro de bit sabado</param>
        /// <param name="bit_domingo">Permite Editar el registro de bit domingo</param>
        /// <param name="id_usuario">Permite Editar el identificador del ultimo usuaruio que modifica</param>
        /// <returns></returns>
        public RetornoOperacion EditarAsistenciaHorario(TimeSpan inicio_hrs, TimeSpan fin_hrs, byte tolerancia_entrada, bool bit_lunes, bool bit_martes, bool bit_miercoles,
                                             bool bit_jueves, bool bit_viernes, bool bit_sabado, bool bit_domingo, int id_usuario)
        {
            //Retorna e Invoca el método privado editarAsistenciaHorario
            return this.editaAsistenciaHorario(inicio_hrs, fin_hrs, tolerancia_entrada, bit_lunes, bit_martes, bit_miercoles, bit_jueves, bit_viernes, bit_sabado, bit_domingo, id_usuario, this._habilitar);
        }

        public RetornoOperacion DeshabilitarAsistenciaHorario(int id_usuario)
        {
            //Retorna e Invoca al método privado editarAsistenciaEmpleado
            return this.editaAsistenciaHorario(this._inicio_hrs, this._fin_hrs, this._tolerancia_entrada, this._bit_lunes, this._bit_martes, this._bit_miercoles, this._bit_jueves, this._bit_viernes, this._bit_sabado, this._bit_domingo,id_usuario, false);
        }

        #endregion

    }
}
