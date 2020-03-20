using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// Clase de Entrada Salida que permite realizar las acciones de Inserción, Actualización y Deshabilitar registros. 
    /// </summary>
    public class EntradaSalida : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Enumera los tipos de operacion sobre la tabla TipooOperacion
        /// </summary>
        public enum TipoOperacion
        { 
            /// <summary>
            /// Define si es una entrada de producto
            /// </summary>
            Entrada = 1,
            /// <summary>
            /// Define si es una salida de producto
            /// </summary>
            Salida = 2
        }

        #endregion
    

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp de la tabla EntradaSalida
        /// </summary>
        private static string nom_sp = "almacen.sp_entrada_salida_tes";
        private int _id_entrada_salida;
        /// <summary>
        /// Identificador de una entrada o salida de un producto.
        /// </summary>
        public int id_entrada_salida
        {
            get { return _id_entrada_salida; }
        }
        private byte _id_tipo_operacion;
        /// <summary>
        /// Permite identificar si el registro se refiere a una entrada o a una salida de producto.
        /// </summary>
        public byte id_tipo_operacion
        {
            get { return _id_tipo_operacion; }
        }
        /// <summary>
        /// Permite acceder a los elementos de la enumeración TipooOperacion
        /// </summary>
        public TipoOperacion tipo_operacion
        {
            get { return (TipoOperacion)this._id_tipo_operacion; }
        }
        private int _secuencia_compania;
        /// <summary>
        /// Consecutivo numerico que permite identificar la secuencia de registros por empresa.
        /// </summary>
        public int secuencia_compania
        {
            get { return _secuencia_compania; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Identificador de una empresa
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private int _id_tabla;
        /// <summary>
        /// Identificador de la tabla a la cual pertenece el registro de entrada/salida.
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_registro;
        /// <summary>
        /// Identificador de un registro asociado con la entrada/salida.
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private DateTime _fecha_entrada_salida;
        /// <summary>
        /// Fecha en la que se realizo la entrada/salida de producto.
        /// </summary>
        public DateTime fecha_entrada_salida
        {
            get { return _fecha_entrada_salida; }
        }
        private byte _id_moneda;
        /// <summary>
        /// Identificador de un tipo de moneda (Pesos, Dolar o Euro)
        /// </summary>
        public byte id_moneda
        {
            get { return _id_moneda; }
        }
        private int _id_almacen;
        /// <summary>
        /// Identificador de un almacen al que pertenece el producto que se dara entrada o salida.
        /// </summary>
        public int id_almacen
        {
            get { return _id_almacen; }
        }
        private bool _habilitar;
        /// <summary>
        /// Permite cambiar el estdo de habilitación de un registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa en 0 los atributos de la clase.
        /// </summary>
        public EntradaSalida()
        {
            this._id_entrada_salida = 0;
            this._id_tipo_operacion = 0;
            this._secuencia_compania = 0;
            this._id_compania_emisor = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._fecha_entrada_salida = DateTime.MinValue;
            this._id_moneda = 0;
            this._id_almacen = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atibutos a partir de un rgistro determinado.
        /// </summary>
        /// <param name="id_entrada_salida">Identificador de un registro de entrada/salida</param>
        public EntradaSalida(int id_entrada_salida)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_entrada_salida);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~EntradaSalida()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que carga con valores a los atributos a partir de un registro determinado.
        /// </summary>
        /// <param name="id_entrada_salida">Id que sirve como referencia para la busqueda del registro</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_entrada_salida)
        {
            //Creación del objeto retorno
            bool retorno;
            //Creación del arreglo que almacena los dtos necesarios para realizar la consulta a base de datos.
            object[] param = { 3, id_entrada_salida, 0, 0, 0, 0, 0, null, 0, 0, 0, false, "", "" };
            //Asigana al DataSet DS el resultado del método EjecutaProcAlmacenadoDataSet
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del DS(Que existan y no sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas de DS y almacena el resultado del registro en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_entrada_salida = id_entrada_salida;
                        this._id_tipo_operacion = Convert.ToByte(r["IdTipoOperacion"]);
                        this._secuencia_compania = Convert.ToInt32(r["SecuenciaCompania"]);
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._fecha_entrada_salida = Convert.ToDateTime(r["FechaEntradaSalida"]);
                        this._id_moneda = Convert.ToByte(r["IdMoneda"]);
                        this._id_almacen = Convert.ToInt32(r["IdAlmacen"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
                //Cambia de valor al objeto retorno siempre y cando se cumplan la validación de datos
                retorno = true;
            }
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro Entrada/Salida
        /// </summary>
        /// <param name="tipo_operacion">Actualiza el tipo de operación (Entrada o Salida)</param>
        /// <param name="secuencia_compania">Actualiza la secuencia de la compañia</param>
        /// <param name="id_compania_emisor">Actualiza el identificador de la compania</param>
        /// <param name="id_tabla">Actualiza el identificador de la tabla donde pertenece el registro</param>
        /// <param name="id_registro">Actualiza el identificador del registro al que hace referencia la entrada-salida</param>
        /// <param name="fecha_entrada_salida">Actualiza la fecha en que se realizo la entrada-salida</param>
        /// <param name="id_moneda">Actualiza el tipo de moneda(pesos, dolar, euro)</param>
        /// <param name="id_almacen">Actualiza el identificador de un almacen al que pertenece el producto</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la ultima acción sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de habilitación del registro</param>
        /// <returns></returns>
        private RetornoOperacion editarEntradaSalida(TipoOperacion tipo_operacion, int secuencia_compania, int id_compania_emisor, int id_tabla, int id_registro,
                                                    DateTime fecha_entrada_salida, byte id_moneda, int id_almacen, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos para realizar la actualización del registro
            object[] param = { 2, this._id_entrada_salida, (byte)tipo_operacion, secuencia_compania, id_compania_emisor, id_tabla, id_registro, fecha_entrada_salida, id_moneda, id_almacen, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método EjecutaProcAlmacenadoObjeto.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el método al objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método que inserta registros de entrada salida
        /// </summary>
        /// <param name="tipo_operacion">Inserta el tipo de operacion a registrar (Entrada o Salida)</param>
        /// <param name="secuencia_compania">Inserta consecutivo de registros</param>
        /// <param name="id_compania_emisor">Inserta el identificador de una compañia</param>
        /// <param name="id_tabla">Inserta el identificador de la tabla donde pertenece el registro </param>
        /// <param name="id_registro">Inserta el identificador del registro que hace referencia a una Entrada o Salida de producto</param>
        /// <param name="fecha_entrada_salida">Inserta la fecha entrada o salida de un producto</param>
        /// <param name="id_moneda">Inserta  tipo de moneda (pesos, dolar, euro)</param>        
        /// <param name="id_almacen">Inserta el identificador de un almacen donde proviene el producto</param>
        /// <param name="id_usuario">Inserta el identificador de usuario que realizo la acción de inserción</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarEntradaSalida(TipoOperacion tipo_operacion, int id_compania_emisor, int id_tabla, int id_registro,
                                                              DateTime fecha_entrada_salida, byte id_moneda, int id_almacen, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos para realizar la inserción del registro.
            object[] param = { 1, 0, (byte)tipo_operacion, 0, id_compania_emisor, id_tabla, id_registro, fecha_entrada_salida, id_moneda, id_almacen, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro Entrada/Salida
        /// </summary>
        /// <param name="tipo_operacion">Actualiza el tipo de operación (Entrada o Salida)</param>
        /// <param name="secuencia_compania">Actualiza la secuencia de la compania</param>
        /// <param name="id_compania_emisor">Actualiza el identificador de la compania</param>
        /// <param name="id_tabla">Actualiza el identificador de la tabla donde pertenece el registro</param>
        /// <param name="id_registro">Actualiza el identificador del registro al que hace referencia la entrada-salida</param>
        /// <param name="fecha_entrada_salida">Actualiza la fecha en que se realizo la entrada-salida</param>
        /// <param name="id_moneda">Actualiza el tipo de moneda(pesos, dolar, euro)</param>        
        /// <param name="id_almacen">Actualiza el idntificador de un almacen</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la ultima accion sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de habilitacion del registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarEntradaSalida(TipoOperacion tipo_operacion, int secuencia_compania, int id_compania_emisor, int id_tabla, int id_registro,
                                                    DateTime fecha_entrada_salida, byte id_moneda, int id_almacen, int id_usuario)
        {
            //Retorna al método la edicion de registros
            return editarEntradaSalida((TipoOperacion)tipo_operacion, secuencia_compania, id_compania_emisor, id_tabla, id_registro, fecha_entrada_salida, id_moneda,
                                       id_almacen, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un registro
        /// </summary>
        /// <param name="id_usuario">Id que permite identifiacr al usuario que realizo la accion de deshabilitar el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarEntradaSalida(int id_usuario)
        {
            //Retorna al método la edicion de registros
            return editarEntradaSalida((TipoOperacion)this._id_tipo_operacion, this._secuencia_compania, this.id_compania_emisor, this._id_tabla,
                this._id_registro, this._fecha_entrada_salida, this._id_moneda,this._id_almacen, id_usuario, false);
        }
        #endregion


    }
}
