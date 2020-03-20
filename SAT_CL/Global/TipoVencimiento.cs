using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase de la tabla TipoVenciemietno que permite la manipulación de los registros en la base de datos(inserción,actualización, deshabilitar)
    /// </summary>
    public class TipoVencimiento : Disposable
    {
        #region Enumeración

        /// <summary>
        /// Enumera el tipo de aplicación de un TipoVencimiento
        /// </summary>
        public enum TipoAplicacion
        {
            /// <summary>
            /// Permite saber si un TipoVencimiento se aplicara a una unidad
            /// </summary>
            Unidad = 1,
            /// <summary>
            /// Permite saber si un TipoVencimiento se aplicara a un operador
            /// </summary>
            Operador = 2,
            /// <summary>
            /// Permite saber si un TipoVencimiento se aplicara a un transportista
            /// </summary>
            Transportista = 3,
            /// <summary>
            /// Permite saber si un TipoVencimiento se aplicara a un Servicio
            /// </summary>
            Servicio = 4
        };

        /// <summary>
        /// Enumera la prioridad de un TipoVencimiento
        /// </summary>
        public enum Prioridad
        {
            /// <summary>
            /// Determina si un TipoVencimiento es Obligatorio
            /// </summary>
            Obligatorio = 1,
            /// <summary>
            /// Determina su el TipoVencimiento es Opcional
            /// </summary>
            Opcional = 2
        };

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo que almacena el nombre del sp de la tabla TipoVencimiento
        /// </summary>
        private static string nom_sp = "global.sp_tipo_vencimiento_ttv";

        private int _id_tipo_vencimiento;
        /// <summary>
        /// Id que permite identificar el registro de TipoVencimeitno
        /// </summary>
        public int id_tipo_vencimiento
        {
            get { return _id_tipo_vencimiento; }
        }

        private byte _id_tipo_aplicacion;
        /// <summary>
        /// Id que permite identificar el tipo de aplicacion de un TipoVencimiento (Operador,Unidad)
        /// </summary>
        public byte id_tipo_aplicacion
        {
            get { return _id_tipo_aplicacion; }
        }
        /// <summary>
        /// Permite acceder a los valores de la enumeración de TipoAplicación
        /// </summary>
        public TipoAplicacion tipo_aplicacion
        {
            get { return (TipoAplicacion)this._id_tipo_aplicacion; }
        }

        private string _descripcion;
        /// <summary>
        /// Define las caracteristicas de un TipoEvento
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }

        private byte _id_prioridad;
        /// <summary>
        /// Id que identifica un nivel de prioridad
        /// </summary>
        public byte id_prioridad
        {
            get { return _id_prioridad; }
        }

        /// <summary>
        /// Permite acceder a los elementos de la enumeración Prioridad
        /// </summary>
        public Prioridad prioridad
        {
            get { return (Prioridad)this._id_prioridad; }
        }

        private bool _habilitar;
        /// <summary>
        /// Permite al cambio de habilitacion de un registro (Habilitar/Deshabiitar)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor default de la clase
        /// </summary>
        public TipoVencimiento()
        {
            this._id_tipo_vencimiento = 0;
            this._id_tipo_aplicacion = 0;
            this._descripcion = "";
            this._id_prioridad = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa los atributos de la clase a partir de un registro de TipoVencimiento
        /// </summary>
        /// <param name="id_tipo_vencimiento">Id que sierve como referencia para la inicialización de los atributos de la clase</param>
        public TipoVencimiento(int id_tipo_vencimiento)
        {
            //Invoca al método cargaAtributoInstancia().
            cargaAtributoInstancia(id_tipo_vencimiento);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~TipoVencimiento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite la busqueda de registros de tipoVencimiento a aprtir de un id de referencias y el resultado los asigna a los atributos de la clase.
        /// </summary>
        /// <param name="id_tipo_vencimiento">Id que sirve como referencia para la busqueda de registros.</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_tipo_vencimiento)
        {
            //Creación del objeto retrono
            bool retorno = false;
            //Creacioón y Asignación de valores al areglo utilizados para la manipulación de datos de la tabla TipoVencimiento
            object[] param = { 3, id_tipo_vencimiento, 0, "", 0, 0, false, "", "" };
            //Invoca al método de la capa de datos que permite realizar la consulta de registros de TipoVencimiento
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos de  TipoEvento existan y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre loas filas del Dataset, y almacena en r las coincidencias del id de busqueda(id_tipo_vencimiento)
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_tipo_vencimiento = id_tipo_vencimiento;
                        this._id_tipo_aplicacion = Convert.ToByte(r["IdTipoAplicacion"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._id_prioridad = Convert.ToByte(r["IdPrioridad"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor de retorno siempre y cuando se cumpla la validación de los datos
                    retorno = true;
                }
            }
            //Retorna el resultado al método.
            return retorno;
        }

        /// <summary>
        /// Método que permite la actualización de registros de TipoVencimiento
        /// </summary>
        /// <param name="id_tipo_aplicacion">Permite la actualización del tipo de aplicacion de un TipoVencimiento(operador,unidad)</param>
        /// <param name="descripcion">Permite la actualización de la descripcion de un TipoVencimiento</param>
        /// <param name="id_prioridad">Permite la actualización  del nivel de prioridad(obligatorio, opcional)</param>
        /// <param name="id_usuario">Permite la actualización del usuario que realizo acciones sobre el registro TipoVencimiento</param>
        /// <param name="habilitar">Permite la actualización del estado de habilitación de un registro de TipoVencimiento</param>
        /// <returns></returns>
        private RetornoOperacion editarTipoVencimiento(byte id_tipo_aplicacion, string descripcion, byte id_prioridad, int id_usuario, bool habilitar)
        {
            //Creación del objeto retrono
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al areglo,necesarios para el sp de la tabla TipoVencimiento
            object[] param = { 2, this.id_tipo_vencimiento, id_tipo_aplicacion, descripcion, id_prioridad, id_usuario, habilitar, "", "" };
            //asigana a la variable retorno los valores de la Invocación del método de la capa de datos que permite realizar la actualización de registros de TipoVencimiento. 
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos


        /// <summary>
        /// Método que permite la inserción de registros en la tabla TipoVencimiento
        /// </summary>
        /// <param name="id_tipo_aplicacion">Permite la insercion del tipo de aplicacion de un TipoVencimiento(operador,unidad)</param>
        /// <param name="descripcion">Permite la inserción de la descripcion de un Tipo de Vencimiento</param>
        /// <param name="id_prioridad">Permite la insercion de un nivel de Prioridad de un Tipo de Vencimiento(obligatorio,opcional)</param>
        /// <param name="id_usuario">Permite la inserción de  identificador del usuario que realizo la insercion del registro de TipoVencimiento</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarTipoVencimiento(byte id_tipo_aplicacion, string descripcion, byte id_prioridad, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al areglo con los datos a insertar en la Tabla TipoVencimiento
            object[] param = { 1, 0, id_tipo_aplicacion, descripcion, id_prioridad, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno de la Invocación del método de la capa de datos que permite realizar la inserción de registros de TipoVencimiento. 
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite la actualización de registros de TipoVencimiento
        /// </summary>
        /// <param name="tipo_aplicacion">Permite la actualización del tipo de aplicacion de un TipoVencimiento(operador,unidad)</param>
        /// <param name="descripcion">Permite la actualización de la descripcion de un TipoVencimiento</param>
        /// <param name="prioridad">Permite la actualización del nivel de prioridad (obligatoria, opcional)</param>
        /// <param name="id_usuario">Permite la actualización del usuario que realizo acciones sobre el registro TipoVencimiento</param>
        /// <returns></returns>
        public RetornoOperacion EditarTipoVencimiento(byte id_tipo_aplicacion, string descripcion,byte id_prioridad, int id_usuario)
        {
            //Retorna e Invoca el método editarTipoVencimiento().
            return this.editarTipoVencimiento(id_tipo_aplicacion, descripcion, id_prioridad, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite el cambio de habilitación de un registro de TipoVencimiento
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo el cambio de estado al registro de TipoVencimiento</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarTipoVencimiento(int id_usuario)
        {
            //Invoca y Retorna el método editarTipoVencimiento().
            return this.editarTipoVencimiento(this.id_tipo_aplicacion, this.descripcion, this.id_prioridad, id_usuario, false);
        }
        #endregion
    }
}
