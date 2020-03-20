using Microsoft.SqlServer.Types;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;


namespace SAT_CL.Ruta
{
    /// <summary>
    /// Clase que realiza las acciones sobre los registros de caseta (Insertar,Editar,Consultar)
    /// </summary>
    public class Caseta : Disposable
    {
        #region Enumeración
        /// <summary>
        /// Enumera los tipos de caseta
        /// </summary>
        public enum TipoCaseta
        {
            /// <summary>
            /// Tipo de caseta Fitosanitaria
            /// </summary>
            Fitosanitaria = 1,
            /// <summary>
            /// Caseta de Acceso a carretera
            /// </summary>
            Acceso,
            /// <summary>
            /// Caseta de Cobro (estacionamiento)
            /// </summary>
            Cobro
        }
        public enum RedCarretera
        {
            CAPUFE = 1
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla caseta.
        /// </summary>
        private static string nom_sp = "ruta.sp_caseta_tc";
        private int _id_caseta;
        /// <summary>
        /// Identificador de una caseta
        /// </summary>
        public int id_caseta
        {
            get { return _id_caseta; }
        }
        private int _id_compania;
        /// <summary>
        /// Identificador de la compañía a la que pertenece el registro de una caseta
        /// </summary>
        public int id_compania
        {
            get { return _id_compania; }
        }
        private int _no_caseta;
        /// <summary>
        /// Número consecutivo que permite enumerar por compañía las casetas registradas.
        /// </summary>
        public int no_caseta
        {
            get { return _no_caseta; }
        }
        private string _descripcion_caseta;
        /// <summary>
        /// Nombre o caracteristicas de una caseta.
        /// </summary>
        public string descripcion_caseta
        {
            get { return _descripcion_caseta; }
        }
        private string _alias_caseta;
        /// <summary>
        /// Nnombre corto de una caseta.
        /// </summary>
        public string alias_caseta
        {
            get { return _alias_caseta; }
        }
        private SqlGeography _posicion_caseta;
        /// <summary>
        /// Posición geográfica de la caseta.
        /// </summary>
        public SqlGeography posicion_caseta
        {
            get { return _posicion_caseta; }
        }
        private byte _id_tipo_caseta;
        /// <summary>
        /// Permite saber el tipo de caseta (Cobro, Fitosanitaria,Acceso).
        /// </summary>
        public byte id_tipo_caseta
        {
            get { return _id_tipo_caseta; }
        }
        /// <summary>
        /// Permite tener acceso a la enumeración de tipo de casetas
        /// </summary>
        public TipoCaseta tipo
        {
            get { return (TipoCaseta)this._id_tipo_caseta; }
        }
        private int _id_red_carretera;
        /// <summary>
        /// Permite identificar una red de carretera a la que pertence la caseta.
        /// </summary>
        public int id_red_carretera
        {
            get { return _id_red_carretera; }
        }
        /// <summary>
        /// Permite tener acceso a la enumeración de tipo de casetas
        /// </summary>
        public RedCarretera redCarretera
        {
            get { return (RedCarretera)this._id_red_carretera; }
        }
        private bool _bit_iave;
        /// <summary>
        /// Identifica si la casete hace uso de tarjeta IAVE o no
        /// </summary>
        public bool bit_iave
        {
            get { return _bit_iave; }
            set { _bit_iave = value; }
        }
        /// <summary>
        /// Habilita o Deshabilita el uso del registro
        /// </summary>
        private bool _habilitar;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que inicializa los atributos a cero
        /// </summary>
        public Caseta()
        {
            this._id_caseta = 0;
            this._id_compania = 0;
            this._no_caseta = 0;
            this._descripcion_caseta = "";
            this._alias_caseta = "";
            this._posicion_caseta = SqlGeography.Null;
            this._id_tipo_caseta = 0;
            this._id_red_carretera = 0;
            this._bit_iave = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los valores de los atributos a partir de la consulta de un registro
        /// </summary>
        /// <param name="id_caseta">ID que sirve como referencia para la consulta de registro.</param>
        public Caseta(int id_caseta)
        {
            //Invoca al método que realiza la consulta y asignación de valores a los atributos.
            cargaAtributos(id_caseta);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~Caseta()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registros y el resultado lo asigna a los atributos
        /// </summary>
        /// <param name="id_caseta">Id que sirve como referencia para la busqueda del registro</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_caseta)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta a base de datos
            object[] param = { 3, id_caseta, 0, 0, "", "", null, 0, 0, false, 0, false, "", "" };
            //Crea instancia con el método que realiza la consulta a base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recore las filas del dataset y asigna el valor a los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_caseta = id_caseta;
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._no_caseta = Convert.ToInt32(r["NoCaseta"]);
                        this._descripcion_caseta = Convert.ToString(r["Descripcion"]);
                        this._alias_caseta = Convert.ToString(r["Alias"]);
                        this._posicion_caseta = (SqlGeography)r["PosicionCaseta"];
                        this._id_tipo_caseta = Convert.ToByte(r["IdTipoCaseta"]);
                        this._id_red_carretera = Convert.ToInt32(r["IdRedCarretera"]);
                        this._bit_iave = Convert.ToBoolean(r["BitIAVE"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
                //Cambia el valor del objeto retrono Si se cumple la validación del DS
                retorno = true;
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los campos de un registro de caseta
        /// </summary>
        /// <param name="id_compania">Actualiza el identificador de una compañía a la que pertenece una caseta</param>
        /// <param name="descripcion_caseta">Actualiza el nombre o la descripción de una caseta</param>
        /// <param name="alias_caseta">Actualiza el nombre corto de la caseta</param>
        /// <param name="posicion_caseta">Actualiza la posición geográfica de la caseta</param>
        /// <param name="tipo_caseta">Actualiza el tipo de caseta (Cobro, Acceso, Fitosanitaria)</param>
        /// <param name="id_red_carretera">Actualiza la red de carretera a la que pertenece una caseta</param>
        /// <param name="bit_iave">Actualiza si la caseta hace uso de una Tarjeta IAVE o no</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización al registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado-Disponible,Deshabilitado-No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarCaseta(int id_compania, string descripcion_caseta, string alias_caseta, SqlGeography posicion_caseta,
                                              TipoCaseta tipo_caseta, RedCarretera id_red_carretera, bool bit_iave, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesario para realizar la actualización del registro
            object[] param = { 2, this._id_caseta, id_compania, this._no_caseta, descripcion_caseta, alias_caseta, posicion_caseta, (TipoCaseta)tipo_caseta,(RedCarretera) id_red_carretera, bit_iave, id_usuario, habilitar, "", "" };
            //Realiza la actualización de campos del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método que permite insertar un registro de caseta
        /// </summary>
        /// <param name="id_compania">Inserta el identificador de una compañía a la que pertenece una caseta</param>
        /// <param name="descripcion_caseta">Inserta el nombre o la descripción de una caseta</param>
        /// <param name="alias_caseta">Inserta el nombre corto de la caseta</param>
        /// <param name="posicion_caseta">Inserta la posición geográfica de la caseta</param>
        /// <param name="tipo_caseta">Inserta el tipo de caseta (Cobro, Acceso, Fitosanitaria)</param>
        /// <param name="id_red_carretera">Inserta la red de carretera a la que pertenece una caseta</param>
        /// <param name="bit_iave">Inserta si la caseta hace uso de una Tarjeta IAVE o no</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo acciones sobre el registro</param>        
        /// <returns></returns>
        public static RetornoOperacion InsertarCaseta(int id_compania, string descripcion_caseta, string alias_caseta, SqlGeography posicion_caseta,
                                                      TipoCaseta tipo_caseta, RedCarretera id_red_carretera, bool bit_iave, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del areglo que almacena los datos necesarios para realizar la inserción de una caseta
            object[] param = { 1, 0, id_compania, 0, descripcion_caseta, alias_caseta, posicion_caseta, (TipoCaseta)tipo_caseta,(RedCarretera) id_red_carretera, bit_iave, id_usuario, true, "", "" };
            //Realiza la inserción de una caseta
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los campos de un registro de caseta
        /// </summary>
        /// <param name="id_compania">Actualiza el identificador de una compañía a la que pertenece una caseta</param>        
        /// <param name="descripcion_caseta">Actualiza el nombre o la descripción de una caseta</param>
        /// <param name="alias_caseta">Actualiza el nombre corto de la caseta</param>
        /// <param name="posicion_caseta">Actualiza la posición geográfica de la caseta</param>
        /// <param name="tipo_caseta">Actualiza el tipo de caseta (Cobro, Acceso, Fitosanitaria)</param>
        /// <param name="id_red_carretera">Actualiza la red de carretera a la que pertenece una caseta</param>
        /// <param name="bit_iave">Actualiza si la caseta hace uso de una Tarjeta IAVE o no</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>        
        /// <returns></returns>
        public RetornoOperacion EditarCaseta(int id_compania, string descripcion_caseta, string alias_caseta, SqlGeography posicion_caseta,
                                                      TipoCaseta tipo_caseta, RedCarretera id_red_carretera, bool bit_iave, int id_usuario)
        {
            //Retorna al método el resultado del método que realiza la actualización de campos de un registro
            return editarCaseta(id_compania, descripcion_caseta, alias_caseta, posicion_caseta, (TipoCaseta)tipo_caseta,(RedCarretera) id_red_carretera, bit_iave, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita los registros de una caseta
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizó la acción</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCaseta(int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Inicia bloque transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            { 
                retorno = editarCaseta(this._id_compania, this._descripcion_caseta, this._alias_caseta, this._posicion_caseta, (TipoCaseta)this._id_tipo_caseta,(RedCarretera) this._id_red_carretera, this._bit_iave, id_usuario, false);
                //Valida la operación de deshabilitar registro
                if (retorno.OperacionExitosa)
                {
                    //Obtiene los costos de las casetas
                    using (DataTable dtCostoCaseta = SAT_CL.Ruta.CostoCaseta.ObtieneCostoCaseta(this._id_caseta))
                    {
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCostoCaseta))
                        {
                            //Recorre los registros de la tabla de costo caseta
                            foreach (DataRow r in dtCostoCaseta.Rows)
                            {
                                //Instancia a la clase costo caseta para obtener el método de deshabilitar registro
                                using (SAT_CL.Ruta.CostoCaseta costo = new SAT_CL.Ruta.CostoCaseta(Convert.ToInt32(r["Id"])))
                                {
                                    //Deshabilita los costos de la caseta
                                    retorno = costo.DeshabilitaCostoCaseta(id_usuario);
                                    //Valida la operación
                                    if (!retorno.OperacionExitosa)
                                        break;
                                }
                            }
                        }
                    }
                }
                //Valida la transacción
                if (retorno.OperacionExitosa)
                    //Termina el bloque transaccional
                    trans.Complete();
            }
            //Retorna al método el resultado del método que realiza la actualización de campos de un registro
            return retorno;
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCaseta()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_caseta);
        }
        #endregion
    }
}
