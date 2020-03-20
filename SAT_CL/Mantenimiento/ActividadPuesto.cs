using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// Clase que permite manipular los datos de la tabla Actividad Puesto(Consultar, Insertar, Editar)
    /// </summary>
    public class ActividadPuesto: Disposable
    {
        #region Enumeracion
        /// <summary>
        /// Enumeración de los direferentes tipos de puesto para actividades de mantenimietno a unidades de trasnporte
        /// </summary>
        public enum Puesto
        {
            /// <summary>
            /// Puesto de Mecanico tipo A
            /// </summary>
            MecanicoA = 1,
            /// <summary>
            /// Puesto de Mecanico tipo B
            /// </summary>
            MecanicoB = 2,
            /// <summary>
            /// Puesto de Mecanico tipo C
            /// </summary>
            MecanicoC = 3,
            /// <summary>
            /// Puesto de Mécanico tipo D
            /// </summary>
            MecanicoD = 4,
            /// <summary>
            /// Puesto de Electrico
            /// </summary>
            Electrico = 5,
            /// <summary>
            /// Puesto de Talachero.
            /// </summary>
            Talachero = 6
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla Activida Puesto
        /// </summary>
        private static string nom_sp = "mantenimiento.sp_actividad_puesto_tap";
        private int _id_actividad_puesto;
        /// <summary>
        /// Almacena el identificador del registro de una actividad puesto
        /// </summary>
        public int id_actividad_puesto
        {
            get { return _id_actividad_puesto; }
        }
        private int _id_actividad;
        /// <summary>
        /// Almacena el identificador de registro de una actividad de mantenimiento
        /// </summary>
        public int id_actividad
        {
            get { return _id_actividad; }
        }
        private byte _id_puesto;
        /// <summary>
        /// Almacena el identificador de los diferentes puestos de mantenimiento (Mecánico, eléctrico, talachero, etc.)
        /// </summary>
        public byte id_puesto
        {
            get { return _id_puesto; }
        }
        /// <summary>
        /// Permite tener acceso a los elementos de la enumeración Puesto
        /// </summary>
        private Puesto puesto
        {
            get { return (Puesto)this._id_puesto; }
        }
        private int _tiempo_actividad;
        /// <summary>
        /// Almacena la cantidad de días que dura la realización de una actividad.
        /// </summary>
        public int tiempo_actividad
        {
            get { return _tiempo_actividad; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacena el estado de habilitación de un registro.
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que inicializa los atributos a 0
        /// </summary>
        public ActividadPuesto()
        {
            this._id_actividad_puesto = 0;
            this._id_actividad = 0;
            this._id_puesto = 0;
            this._tiempo_actividad = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a patir de un registro dado.
        /// </summary>
        /// <param name="id_actividad_puesto">Id que identifica el registro con el que se inicializaran los atributos</param>
        public ActividadPuesto(int id_actividad_puesto)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_actividad_puesto);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ActividadPuesto()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro y almacena el resultado en los atributos
        /// </summary>
        /// <param name="id_actividad_puesto">Id que sirve como referencia para la busqueda del registro</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_actividad_puesto)
        {
            //Crea el objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesario para el sp.
            object[] param = { 3, id_actividad_puesto, 0, 0, 0, 0, false, "", "" };
            //Instancia al método EjecutaProcAlmacenadoDataSet y almacena el resultado en el datase DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan datos en el dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas del registro y los campos los almacena en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_actividad_puesto = id_actividad_puesto;
                        this._id_actividad = Convert.ToInt32(r["IdActividad"]);
                        this._id_puesto = Convert.ToByte(r["IdPuesto"]);
                        this._tiempo_actividad = Convert.ToInt32(r["TiempoActividad"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno siempre y cuando se cumplan la validación de datos
                    retorno = true;
                }
            }
            //Retorna el resultado al objeto retorno
            return retorno;
        }
        /// <summary>
        ///  Método que actualiza los campos de un registro de actividad puesto.
        /// </summary>
        /// <param name="id_actividad">Actualiza el campo de una actividad</param>
        /// <param name="id_puesto">Actualiza el campo de un puesto (mecánico, eléctrico, talachero)</param>
        /// <param name="tiempo_actividad">Actualiza los días de duración de una actividad</param>
        /// <param name="id_usuario">Actualiza el campo del identificador del usuario que realizó acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el campo del estado de habilitación de un registro</param>
        /// <returns></returns>
        private RetornoOperacion editarActividadPuesto(int id_actividad, Puesto id_puesto, int tiempo_actividad, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo param
            object[] param = { 2, this._id_actividad_puesto, id_actividad, (byte)id_puesto, tiempo_actividad, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que realiza la inserción de registro de Actividad Puesto
        /// </summary>
        /// <param name="id_actividad">Inserta el identificador del registro de una actividad</param>
        /// <param name="id_puesto">Inserta el identificador de un puesto (mecánico, eléctrico,talachero)</param>
        /// <param name="tiempo_actividad">Inserta la cantidad de días que dura la realización de una actividad</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarActividadPuesto(int id_actividad, Puesto id_puesto, int tiempo_actividad, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del objeto param
            object[] param = { 1, 0, id_actividad, (byte)id_puesto, tiempo_actividad, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        ///  Método que actualiza los campos de un registro de actividad puesto.
        /// </summary>
        /// <param name="id_actividad">Actualiza el campo de una actividad</param>
        /// <param name="id_puesto">Actualiza el campo de un puesto (mecánico, eléctrico, talachero)</param>
        /// <param name="tiempo_actividad">Actualiza los días de duración de una actividad</param>
        /// <param name="id_usuario">Actualiza el campo del identificador del usuario que realizo acciones sobre el registro</param>
        public RetornoOperacion EditarActividadPuesto(int id_actividad, Puesto id_puesto, int tiempo_actividad, int id_usuario)
        {
            //Retorna al método la instancia del método editarActividadPuesto
            return editarActividadPuesto(id_actividad, (Puesto)id_puesto, tiempo_actividad, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un regitro
        /// </summary>
        /// <param name="id_usuario">Identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarActividadRegistro(int id_usuario)
        {
            //Retorna al método la instancia del método editarActividadPuesto
            return editarActividadPuesto(this._id_actividad, (Puesto)this._id_puesto, this._tiempo_actividad, id_usuario, false);
        }
        /// <summary>
        /// Método que carga los puestos asignados a una actividad
        /// </summary>
        /// <param name="id_actividad"></param>
        /// <returns></returns>
        public static DataTable CargaPuestos(int id_actividad)
        {
            //Creacion de la tabla 
            DataTable dtCargaPuesto = null;
            //Creación del arreglo param
            object[] param = { 4, 0, id_actividad, 0, 0, 0, false, "", "" };
            //Almacena en el DS el resultado del método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla loa valores del DS
                    dtCargaPuesto = DS.Tables["Table"];

                //Devuelve el resultado al método
                return dtCargaPuesto;
            }

        }
        #endregion

    }
}
