using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Bancos
{
    /// <summary>
    /// Clase de la tabla tipo_cambio que permite crear operaciones sobre la tabla (inserciones,actualizaciones,consultas,etc.).
    /// </summary>
    public class TipoCambio : Disposable
    {
        #region Enumeración
        /// <summary>
        /// Enumera los operaciones en las cuales se hara un tipo de cambio
        /// </summary>
        public enum OperacionUso
        {
            /// <summary>
            /// 
            /// </summary>
            Todos = 0,
            /// <summary>
            /// Se aplicara un tipo cambio en las facturas.
            /// </summary>
            Factura = 1
        };

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla
        /// </summary>
        private static string nom_sp = "bancos.sp_tipo_cambio_ttc";

        private int _id_tipo_cambio;
        /// <summary>
        /// Permite identificar el tipo de cambio de la moneda
        /// </summary>
        public int id_tipo_cambio
        {
            get { return _id_tipo_cambio; }
        }

        private int _id_compania;
        /// <summary>
        /// Permite identificar a una compañía
        /// </summary>
        public int id_compania
        {
            get { return _id_compania; }
        }

        private decimal _valor_tipo_cambio;
        /// <summary>
        /// Permite almacenar el valor de tipo de cambio
        /// </summary>
        public decimal valor_tipo_cambio
        {
            get { return _valor_tipo_cambio; }
        }

        private byte _id_moneda;
        /// <summary>
        /// Permite identificar el tipo de moneda 
        /// </summary>
        public byte id_moneda
        {
            get { return _id_moneda; }
        }

        private DateTime _fecha;
        /// <summary>
        /// Permite definir la fecha aplicable para un tipo de cambio.
        /// </summary>
        public DateTime fecha
        {
            get { return _fecha; }
        }

        private byte _id_operacion_uso;
        /// <summary>
        /// Permite identificar en que operación se aplica el tipo de cambio.
        /// </summary>
        public byte id_operacion_uso
        {
            get { return _id_operacion_uso; }
        }

        /// <summary>
        /// Permite acceder a los elementos de la enumeración OperacionUso
        /// </summary>
        public OperacionUso operacion_uso
        {
            get { return (OperacionUso)this._id_operacion_uso;}
        }

        private bool _habilitar;
        /// <summary>
        /// Permite saber el estado de habilitación de un registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Cosntructor por default que inicializa los atributos de la clase
        /// </summary>
        public TipoCambio()
        {
            this._id_tipo_cambio = 0;
            this._id_compania = 0;
            this._valor_tipo_cambio = 0.0M;
            this._id_moneda = 0;
            this._fecha = DateTime.MinValue;
            this._id_operacion_uso = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Cosntructor que inicializa los atributos a través de la busqueda de registros mediante un id
        /// </summary>
        /// <param name="id_tipo_cambio">Id que sirve como referencia para la busqueda de registros</param>
        public TipoCambio(int id_tipo_cambio)
        {
            //Invoca al método cargaAtributonstancia
            cargaAtributoInstancia(id_tipo_cambio);
        }

        /// <summary>
        /// Constructor que permite inicializar atributos en base a algunos parametros de busqueda
        /// </summary>
        /// <param name="id_compania">Id que permite identificar una compania</param>
        /// <param name="id_moneda">Id que permite identificar un tipo de moneda </param>
        /// <param name="fecha">Id que permite saber la fecha </param>
        /// <param name="id_operacion_uso">Id que me permite saber su operacion</param>
        public TipoCambio(int id_compania, byte id_moneda, DateTime fecha, byte id_operacion_uso)
        {
            cargaAtributoInstancia(id_compania, id_moneda, fecha, id_operacion_uso);

        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~TipoCambio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metódo privado que Carga los atributos dado un registro
        /// </summary>
        /// <param name="id_tipo_cambio"></param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_tipo_cambio)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación y asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 3, id_tipo_cambio, 0, 0.0M, 0, null, 0, 0, false, "", "" };
            //Invoca al sp de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos,que existan y que no sean nulos.
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas y almacenamiento de registros en la variable r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_tipo_cambio = id_tipo_cambio;
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._valor_tipo_cambio = Convert.ToDecimal(r["ValorTipoCambio"]);
                        this._id_moneda = Convert.ToByte(r["IdMoneda"]);
                        this._fecha = Convert.ToDateTime(r["Fecha"]);
                        this._id_operacion_uso = Convert.ToByte(r["IdOperacionUso"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno siempre y cuando cumpla las sentencias de validación de datos.
                    retorno = true;
                }
            }

            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite la carga de registros a los atributos mediante varios parametros de busqueda
        /// </summary>
        /// <param name="id_compania"> Permite realizar la busqueda de registro mediante el id_compania </param>
        /// <param name="id_moneda">Permite realizar la busqueda de registro mediante el id_moneda</param>
        /// <param name="fecha">Permite realizar la busqueda de registro mediante la fecha</param>
        /// <param name="id_operacion_uso">Permite realizar la busqueda de registro mediante el id_operacion_uso</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_compania, byte id_moneda, DateTime fecha, byte id_operacion_uso)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación y asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 4, 0, id_compania, 0.0M, id_moneda, fecha, id_operacion_uso, 0, false, "", "" };
            //Invoca al sp de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos,que existan y que no sean nulos.
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas y almacenamiento de registros en la variable r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_tipo_cambio = Convert.ToInt32(r["Id"]);
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._valor_tipo_cambio = Convert.ToDecimal(r["ValorTipoCambio"]);
                        this._id_moneda = Convert.ToByte(r["IdMoneda"]);
                        this._fecha = Convert.ToDateTime(r["Fecha"]);
                        this._id_operacion_uso = Convert.ToByte(r["IdOperacionUso"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno siempre y cuando cumpla las sentencias de validación de datos.
                    retorno = true;
                }
            }

            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite actualizar registros de tipo cambio
        /// </summary>
        /// <param name="id_compania">Permite actualizar el campo de id_compania</param>
        /// <param name="valor_tipo_cambio">Permite actualizar el campo de valor_tipo_cambio</param>
        /// <param name="id_moneda">Permite actualizar el campo de id_moneda</param>
        /// <param name="fecha">Permite actualizar el campo de fecha</param>
        /// <param name="id_operacion_uso">Permite actualizar el campo de id_operacion_uso</param>
        /// <param name="id_usuario">Permite actualizar el campo de id_usuario</param>
        /// <param name="habilitar">Permite actualizar el campo de habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarTipoCambio(int id_compania, decimal valor_tipo_cambio, byte id_moneda, DateTime fecha, OperacionUso operacion_uso, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 2, this.id_tipo_cambio, id_compania, valor_tipo_cambio, id_moneda, fecha, (byte)operacion_uso, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retrono del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que permite insertar registros a Tipo Cambio
        /// </summary>
        /// <param name="id_compania">Permite insertar un identificador de una compania</param>
        /// <param name="valor_tipo_cambio">Permite insertar el valor de la moneda en TipoCambio</param>
        /// <param name="id_moneda">Permite insertar el identificador de una moneda(peso,dollar,etc)</param>
        /// <param name="fecha">Permite insertar la fecha de un tipo de cambio</param>
        /// <param name="id_operacion_uso">Permite insertar id_operacion_uso</param>
        /// <param name="id_usuario">Permite insertar registros en el campo id_usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarTipoCambio(int id_compania, decimal valor_tipo_cambio, byte id_moneda, DateTime fecha, OperacionUso operacion_uso, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 1, 0, id_compania, valor_tipo_cambio, id_moneda, fecha, (byte)operacion_uso, id_usuario, true, "", "" };
            //Asiganción de valores la objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retrono del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite realizar actualizaciones a registros de tipo cambio
        /// </summary>
        /// <param name="id_compania">Permite actualizar el campo id_compania</param>
        /// <param name="valor_tipo_cambio">Permite actualizar el campo valor_tipo_cambio</param>
        /// <param name="id_moneda">Permite actualizar el campo id_moneda</param>
        /// <param name="fecha">Permite actualizar el campo fecha</param>
        /// <param name="id_operacion_uso">Permite actualizar el campo id_operacion_uso</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarTipoCambio(int id_compania, decimal valor_tipo_cambio, byte id_moneda, DateTime fecha,OperacionUso operacion_uso, int id_usuario)
        {
            //Retorna e Invoca al método privado editarTipoCambio
            return this.editarTipoCambio(id_compania, valor_tipo_cambio, id_moneda, fecha, this.operacion_uso, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite cambiar el estado de habilitación de un registro de Tipo Cambio
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarTipoCambio(int id_usuario)
        {
            //Retorna e Invoca el método editarTipoCambio
            return this.editarTipoCambio(this.id_compania, this.valor_tipo_cambio, this.id_moneda, this.fecha, this.operacion_uso, id_usuario, false);
        }

        #endregion
    }
}
