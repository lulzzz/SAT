using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Autorizacion
{
    /// <summary>
    /// Proporciona los metodos para administrar las Autorizaciones
    /// </summary>
    public class Autorizacion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los tipo sde datos permitidos en una autorización
        /// </summary>
        public enum TipoDato
        {
            /// <summary>
            /// Moneda
            /// </summary>
            Money = 1
        }
        /// <summary>
        /// Define los tipos posibles de una autorización
        /// </summary>
        public enum TipoAutorizacion
        { 
            /// <summary>
            /// Monto del depósito de Sevicio
            /// </summary>
            MontoDepositoServicio = 1
        }

        #endregion
           
        #region Propiedades
        
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "autorizacion.sp_autorizacion_tau";

        private int _id_autorizacion;
        /// <summary>
        /// Id Autorizacion
        /// </summary>
        public int id_autorizacion
        {
            get { return _id_autorizacion; }
        }
        private byte _id_tipo_autorizacion;
        /// <summary>
        /// Obtiene el Id de Tipo de Autorización
        /// </summary>
        public byte id_tipo_autorizacion
        {
            get { return this._id_tipo_autorizacion; }
        }
        private int _id_tabla;
        /// <summary>
        /// Id tabla
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_registro;
        /// <summary>
        /// Obtiene el Id de registro
        /// </summary>
        public int id_registro
        { get { return this._id_registro; } }
        private string _descripcion;
        /// <summary>
        /// Descripcion de la Autorización 
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private byte _id_tipo_dato;
        /// <summary>
        /// Id Tipo de Dato
        /// </summary>
        public byte id_tipo_dato
        {
            get { return _id_tipo_dato; }
        }
        private bool _habilitar;
        /// <summary>
        ///  Estado de una Autorización
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        /// <summary>
        /// Enumeracion Tipo Dato
        /// </summary>
        public TipoDato TipoDeDato
        {
            get { return (TipoDato)_id_tipo_dato; }
        }
        /// <summary>
        /// Obtiene el tipo de autorización
        /// </summary>
        public TipoAutorizacion tipo_autorizacion
        { get { return (TipoAutorizacion)this._id_tipo_autorizacion; } }

        #endregion

        #region Constructor
        /// <summary>
        /// Genera una nueva instancia de tipo Autorización
        /// </summary> 
        public Autorizacion()
        {
            _id_autorizacion = 0;
            _id_tipo_autorizacion = 0;
            _id_tabla = 0;
            _id_registro = 0;
            _descripcion = "";
            _id_tipo_dato = 0;
            _habilitar = false;
        }
       
        /// <summary>
        /// Genera una nueva instancia de tipo Autorización dado un id 
        /// </summary>
        /// <param name="id_autorizacion"></param>
        public Autorizacion(int id_autorizacion)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 3, id_autorizacion, 0, 0, 0, "", 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_autorizacion = Convert.ToInt32(r["Id"]);
                        _id_tipo_autorizacion = Convert.ToByte(r["IdTipoAutorizacion"]);
                        _id_tabla = Convert.ToInt32(r["IdTabla"]);
                        _id_registro = Convert.ToInt32(r["IdRegistro"]);
                        _descripcion = r["descripcion"].ToString();
                        _id_tipo_dato = Convert.ToByte(r["IdTipoDato"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
            }
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Autorizacion()
        {
            Dispose(false);
        }
        #endregion

        #region Metodos privados
        /// <summary>
        /// Metodo encargado de editar una Autorizacion
        /// </summary>
        /// <param name="tipo_autorizacion"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="descripcion"></param>
        /// <param name="id_tipo_dato"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaAutorizacion(TipoAutorizacion tipo_autorizacion, int id_tabla, int id_registro, string descripcion, byte id_tipo_dato, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_autorizacion, (byte)tipo_autorizacion, id_tabla, id_registro, descripcion, (byte)id_tipo_dato, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }
        

       
        #endregion

        #region Metodos Publicos
       
        /// <summary>
        /// Metodo encargado de insertar una autorizacion
        /// </summary>
        /// <param name="tipo_autorizacion"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="descripcion"></param>
        /// <param name="id_tipo_dato"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAutorizacion(TipoAutorizacion tipo_autorizacion, int id_tabla, int id_registro, string descripcion, TipoDato id_tipo_dato, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, (byte)tipo_autorizacion, id_tabla, id_registro, descripcion, (byte)id_tipo_dato, id_usuario, true, "", "" };
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }
       
        
        /// <summary>
        /// Metodo encargado de editar una Autorizacion
        /// </summary>
        /// <param name="tipo_autorizacion"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="descripcion"></param>
        /// <param name="id_tipo_dato"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaAutorizacion(TipoAutorizacion tipo_autorizacion, int id_tabla, int id_registro, string descripcion, TipoDato id_tipo_dato, int id_usuario)
        {
            return this.editaAutorizacion(tipo_autorizacion, id_tabla, id_registro, descripcion, (byte)id_tipo_dato, id_usuario, this._habilitar);
        }

        
        /// <summary>
        /// Metodo encargado de deshabilitar una Autorizacion
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAutorizacion(int id_usuario)
        {
            return this.editaAutorizacion(this.tipo_autorizacion, this._id_tabla, this._id_registro, this._descripcion, this._id_tipo_dato, id_usuario, false);
        }
        
       
        /// <summary>
        /// Obtiene el detalle de autorización en el cual encaja el registro de la tabla solicitada en base al valor y tipo de autorización
        /// </summary>
        /// <param name="tipo_autorizacion"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static AutorizacionDetalle CargaAutorizacionesAplicablesRegistro(TipoAutorizacion tipo_autorizacion, int id_tabla, int id_registro, string valor)
        {
            //Declarando objeto de retorno
            AutorizacionDetalle resultado = new AutorizacionDetalle();

            //Declarando variable para indicar el tipo del stored procedura que se ejecutará
            int tipo = 0;

            //En base al tipo de dato, se determinará que tipo será ejecutado en el SP
            switch (tipo_autorizacion)
            { 
                case TipoAutorizacion.MontoDepositoServicio:
                    tipo = 4;
                    break;
            }

            //Declrando arreglo de criterios de consulta
            object[] param = { tipo, 0, 0, id_tabla, id_registro, "", 0, 0, false, valor, "" };

            //Ejecutando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {//Para cada uno de los registros encontrados
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Instanciando registro detalle
                        resultado = new AutorizacionDetalle(Convert.ToInt32(r["IdDetalleAutorizacion"]));
                    }
                }

                //Devolviendo resultado
                return resultado;
            }
        }
        
        #endregion
       
    }

}

       