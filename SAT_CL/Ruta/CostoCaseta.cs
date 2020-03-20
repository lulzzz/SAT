using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Ruta
{
    /// <summary>
    /// Clase que permite realizar acciones basicas sobre los registros de la tabla CostoCaseta  (Insertar, Editar, Consultar)
    /// </summary>
    public class CostoCaseta:Disposable
    {
        #region Enumeracion
        /// <summary>
        /// Enumera los tipos de unidades motrices 
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            /// Cuando la unidad es un autobus
            /// </summary>
            Autobus = 1,
            /// <summary>
            /// Cuando la unidad es un camion
            /// </summary>
            Camion
        }

        /// <summary>
        /// Enumera los tipos de actualización de costo de caseta
        /// </summary>
        public enum TipoActualizacion
        {
            /// <summary>
            /// Actualización del costo de manera automática
            /// </summary>
            Automatico = 1,
            /// <summary>
            /// Actualización del costo de manera manual
            /// </summary>
            Manual
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla costo caseta
        /// </summary>
        private static string nom_sp = "ruta.sp_costo_caseta_tcc";
        private int _id_costo_caseta;
        /// <summary>
        /// Identificador del costo de una caseta
        /// </summary>
        public int id_costo_caseta
        {
            get { return _id_costo_caseta; }
        }
        private int _id_caseta;
        /// <summary>
        /// Identificador de una caseta
        /// </summary>
        public int id_caseta
        {
            get { return _id_caseta; }
        }
        private byte _id_tipo;
        /// <summary>
        /// Identificador de un tipo de unidad (Autobus, Camion, Auto,Motocicleta)
        /// </summary>
        public byte id_tipo
        {
            get { return _id_tipo; }
        }
        /// <summary>
        /// Permite el acceso a la enumeración de los tipos de unidades (Autobus,Camion, etc.)
        /// </summary>
        public Tipo tipo
        {
            get { return (Tipo)this._id_tipo; }
        }
        private int _no_ejes;
        /// <summary>
        /// Cantidad de ejes de una unidad automotriz
        /// </summary>
        public int no_ejes
        {
            get { return _no_ejes; }
        }
        private decimal _costo_caseta;
        /// <summary>
        /// Precio por uso de la caseta
        /// </summary>
        public decimal costo_caseta
        {
            get { return _costo_caseta; }
        }
        private byte _id_tipo_actualización;
        /// <summary>
        /// Tipo de actualización del precio de la caseta (Manual, Automático)
        /// </summary>
        public byte id_tipo_actualización
        {
            get { return _id_tipo_actualización; }
        }
        /// <summary>
        /// Permite el acceso a la enumeración de tipo de actualización
        /// </summary>
        public TipoActualizacion tipoActualizacion
        {
            get { return (TipoActualizacion)this._id_tipo_actualización; }
        }
        private DateTime _fecha_actualizacion;
        /// <summary>
        /// Almacena la fecha en la que se realiza la actualización del costo de la caseta
        /// </summary>
        public DateTime fecha_actualizacion
        {
            get { return _fecha_actualizacion; }
        }
        /// <summary>
        /// Define el estatus de uso del registro (Habilitado - Disponible, Deshabilitado - No Disponible)
        /// </summary>
        private bool _habilitar;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos a cero
        /// </summary>
        public CostoCaseta()
        {
            this._id_costo_caseta = 0;
            this._id_caseta = 0;
            this._id_tipo = 0;
            this._no_ejes = 0;
            this._costo_caseta = 0;
            this._id_tipo_actualización = 0;
            this._fecha_actualizacion = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro
        /// </summary>
        /// <param name="id_costo_caseta">Id que sirve como referencia para la consulta del registro</param>
        public CostoCaseta(int id_costo_caseta)
        {
            //Invoca al método encargado de realizar la busuqeda y asignación de valores a los atributos
            cargaAtributos(id_costo_caseta);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~CostoCaseta()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que busca y asigna a los atributos un registro de costo caseta
        /// </summary>
        /// <param name="id_costo_caseta">Id que sirve como referencia para la busqueda de registros</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_costo_caseta)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta de un registro
            object[] param = { 3, id_costo_caseta, 0, 0, 0, 0, 0, null, 0, false, "", "" };
            //Realiza la consulta del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas y almacena el valor de los campos a los atributos.
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_costo_caseta = id_costo_caseta;
                        this._id_caseta = Convert.ToInt32(r["IdCaseta"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._no_ejes = Convert.ToInt32(r["NoEjes"]);
                        this._costo_caseta = Convert.ToDecimal(r["CostoCaseta"]);
                        this._id_tipo_actualización = Convert.ToByte(r["IdTipoActualizacion"]);
                        this._fecha_actualizacion = Convert.ToDateTime(r["FechaActualizacion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno acorde a la validación del dataset
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de costo caseta
        /// </summary>
        /// <param name="id_caseta">Actualiza el identificador de una caseta</param>
        /// <param name="no_ejes">Actualiza la cantidad de ejes de una unidad</param>
        /// <param name="costo_caseta">Actualiza el precio de uso de una caseta</param>
        /// <param name="tipo">Actualiza el tipo de unidad automotriz (Autobus,Camion, etc.) </param>
        /// <param name="id_tipo_actualización">Actualiza el tipo de actualización de una caseta (Automático,Manual)</param>
        /// <param name="fecha_actualizacion">Actualiza la fecha de actualización del precio de la caseta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la edición del registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado -Disponible, Deshabilitado- No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarCostoCaseta(int id_caseta, Tipo tipo ,int no_ejes, decimal costo_caseta, TipoActualizacion id_tipo_actualización, DateTime fecha_actualizacion, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para la actualización del registro
            object[] param = { 2, this._id_costo_caseta, id_caseta,(Tipo) tipo ,no_ejes, costo_caseta, (TipoActualizacion)id_tipo_actualización, fecha_actualizacion, id_usuario, habilitar, "", "" };
            //Realiza la actualización del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta registro de costo caseta
        /// </summary>
        /// <param name="id_caseta">Inserta el identificador de una caseta</param>
        /// <param name="tipo">Inserta el tipo de unidad automotriz (Autobus,Camion, etc.) </param>
        /// <param name="no_ejes">Inserta la cantidad de ejes de una unidad</param>
        /// <param name="costo_caseta">Inserta el precio de uso de una caseta</param>
        /// <param name="id_tipo_actualización">Inserta el tipo de actualización de una caseta (Automatico,Manual)</param>
        /// <param name="fecha_actualizacion">Inserta la fecha de actualización del precio de la caseta</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizó la inserción del registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarCostoCaseta(int id_caseta, Tipo tipo , int no_ejes, decimal costo_caseta, TipoActualizacion id_tipo_actualización, DateTime fecha_actualizacion, int id_usuario)
        {
            //Creaación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para la actualización del registro
            object[] param = { 1, 0, id_caseta,(Tipo)tipo , no_ejes, costo_caseta, (TipoActualizacion)id_tipo_actualización, fecha_actualizacion, id_usuario, true, "", "" };
            //Realiza la actualización del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de costo caseta
        /// </summary>
        /// <param name="id_caseta">Actualiza el identificador de una caseta</param>
        /// <param name="tipo">Actualiza el tipo de unidad automotriz (Autobus,Camion, etc.) </param>
        /// <param name="no_ejes">Actualiza la cantidad de ejes de una unidad</param>
        /// <param name="costo_caseta">Actualiza el precio de uso de una caseta</param>
        /// <param name="id_tipo_actualización">Actualiza el tipo de actualización de una caseta (Automatico,Manual)</param>
        /// <param name="fecha_actualizacion">Actualiza la fecha de actualización del precio de la caseta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la edición del registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarCostoCaseta(int id_caseta,Tipo tipo , int no_ejes, decimal costo_caseta, TipoActualizacion id_tipo_actualización, DateTime fecha_actualizacion, int id_usuario)
        {
            //Retorna al método el método encargado de actualizar los campos de un registro
            return this.editarCostoCaseta(id_caseta, (Tipo)tipo , no_ejes, costo_caseta, (TipoActualizacion)id_tipo_actualización, fecha_actualizacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Métod que cambia el estado de uso del registro (Habilitado -Disponible, Deshabilitado- No Disponible).
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo el cambio de estado del registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCostoCaseta(int id_usuario)
        {
            //Retorna al método el método encargado de actualizar los campos de un registro
            return this.editarCostoCaseta(this._id_caseta, (Tipo)this._id_tipo, this._no_ejes, this._costo_caseta, (TipoActualizacion)this._id_tipo_actualización, this._fecha_actualizacion, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCostoCaseta()
        {
            //Invoca al método que asigna valores a los atributos de la clase
            return this.cargaAtributos(this._id_costo_caseta);
        }
        /// <summary>
        /// Método que carga los costos asignados a una caseta
        /// </summary>
        /// <param name="id_caseta">Id de rerefencia para inicializar la busqueda de costos de caseta</param>
        /// <returns></returns>
        public static DataTable ObtieneCostoCaseta(int id_caseta)
        {
            //Creación de la tabla que almacenara los costos de la caseta
            DataTable dtCostoCaseta = null;
            //Creación del areglo que realiza la consulta de los costos de una caseta
            object[] param = { 4, 0, id_caseta, 0, 0, 0, 0, null, 0, false, "", "" };
            //Almacena en un Dataset el resultado del método que realiza la busuqeda de costos de caseta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla loa valores del DS
                    dtCostoCaseta = DS.Tables["Table"];
            }
            //Devuelve el resultado al método
            return dtCostoCaseta;
        }
        #endregion
    }
}
