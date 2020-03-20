using System;
using System.Data;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Monitoreo
{
    /// <summary>
    /// Clase que permite insertar, actualizar y consultar datos de Proveedor WS Parametros
    /// </summary>
    public class ProveedorWSParametros :Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla proveedor parametros
        /// </summary>
        private static string nom_sp = "monitoreo.sp_proveedor_ws_parametros_tpwsp";
        private int _id_proveedor_ws_parametros;
        /// <summary>
        /// Identificador de un registro de proveedor ws parametros
        /// </summary>
        public int id_proveedor_ws_parametros
        {
            get { return _id_proveedor_ws_parametros; }
        }
        private int _id_proveedor_ws;
        /// <summary>
        /// Identificador del proveedor de web service
        /// </summary>
        public int id_proveedor_ws
        {
            get { return _id_proveedor_ws; }
        }
        private byte _secuencia;
        /// <summary>
        /// Número secuencial de los parametros por proveedor de web service
        /// </summary>
        public byte secuencia
        {
            get { return _secuencia; }
        }
        private string _nombre_parametro;
        /// <summary>
        /// Nombre del paramaetro de web service
        /// </summary>
        public string nombre_parametro
        {
            get { return _nombre_parametro; }
        }
        private string _valor_parametro;
        /// <summary>
        /// Valor asignado a un parametro de web service
        /// </summary>
        public string valor_parametro
        {
            get { return _valor_parametro; }
        }
        private bool _es_valor_fijo;
        /// <summary>
        /// Permite determinar si el parametro se contemplara en el consumo de web service
        /// </summary>
        public bool es_valor_fijo
        {
            get { return _es_valor_fijo; }
        }
        private bool _habilitar;
        /// <summary>
        /// Permite el cambio de estado de un registro de habilitado a deshabilitado
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos en cero
        /// </summary>
        public ProveedorWSParametros()
        {
            this._id_proveedor_ws_parametros = 0;
            this._id_proveedor_ws = 0;
            this._secuencia = 0;
            this._nombre_parametro = "";
            this._valor_parametro = "";
            this._es_valor_fijo = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro Proveedor Parametros
        /// </summary>
        /// <param name="id_proveedor_ws_parametros"></param>
        public ProveedorWSParametros(int id_proveedor_ws_parametros)
        {
            //Invoca al método que realiza la busaqueda y asignación de valores a los atributos
            cargaAtributos(id_proveedor_ws_parametros);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ProveedorWSParametros()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de registros y el resultado los asigna a los atributos
        /// </summary>
        /// <param name="id_proveedor_ws_parametros">Identifica un registro de parametros de proveedor de ws</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_proveedor_ws_parametros)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro de parametros de proveedor
            object[] param = { 3, id_proveedor_ws_parametros, 0, 0, "", "", false, 0, false, "", "" };
            //Invoca al método que realiza la busqueda de registros y los almacena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el DS contenga datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y almacena en los atributo el registro del dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_proveedor_ws_parametros = id_proveedor_ws_parametros;
                        this._id_proveedor_ws = Convert.ToInt32(r["IdProveedor"]);
                        this._secuencia = Convert.ToByte(r["Secuencia"]);
                        this._nombre_parametro = Convert.ToString(r["NombreParametro"]);
                        this._valor_parametro = Convert.ToString(r["ValorParametro"]);
                        this._es_valor_fijo = Convert.ToBoolean(r["EsFijo"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cuando termine el recorrido de las filas del DS cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorna al mètodo el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los parametros de proveedor de web service
        /// </summary>
        /// <param name="id_proveedor_ws">Actualiza el proveedor de web service</param>
        /// <param name="secuencia">Actualiza el número de secuencia de parametros de proveedor de web service </param>
        /// <param name="nombre_parametro">Actualiza el nombre del parametro </param>
        /// <param name="valor_parametro">Actualiza el valor asignado a un parametro</param>
        /// <param name="es_valor_fijo">Actualiza si el parametro es necesario o no para el consumo del web service</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización de datos del registro</param>
        /// <param name="habilitar">Actualiza el estado de habilitación de un registro</param>
        /// <returns></returns>
        private RetornoOperacion editarProveedorParametros(int id_proveedor_ws, byte secuencia, string nombre_parametro, string valor_parametro, bool es_valor_fijo, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del parametro que almacena los datos necesarios para realizar la actualización de un registro
            object[] param = { 2, this._id_proveedor_ws_parametros, id_proveedor_ws, this._secuencia, nombre_parametro, valor_parametro, es_valor_fijo, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el método que que realiza la actualización de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al metodo el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta los parametros de proveedor de web service
        /// </summary>
        /// <param name="id_proveedor_ws">Inserta el proveedor de web service</param>
        /// <param name="secuencia">Inserta el número de secuencia de parametros de proveedor de web service </param>
        /// <param name="nombre_parametro">Inserta el nombre del parametro </param>
        /// <param name="valor_parametro">Inserta el valor asignado a un parametro</param>
        /// <param name="es_valor_fijo">Inserta si el parametro es necesario o no para el consumo del web service</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la actualización de datos del registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarProveedorParametros(int id_proveedor_ws, byte secuencia, string nombre_parametro, string valor_parametro, bool es_valor_fijo, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del parametro que almacena los datos necesarios para realizar la inserción de un registro
            object[] param = { 1, 0, id_proveedor_ws, 0, nombre_parametro, valor_parametro, es_valor_fijo, id_usuario, true, "", "" };
            //Asigna al objeto retorno el método que que realiza la inserción de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los parametros de proveedor de web service
        /// </summary>
        /// <param name="id_proveedor_ws">Actualiza el proveedor de web service</param>
        /// <param name="secuencia">Actualiza el número de secuencia de parametros de proveedor de web service </param>
        /// <param name="nombre_parametro">Actualiza el nombre del parametro </param>
        /// <param name="valor_parametro">Actualiza el valor asignado a un parametro</param>
        /// <param name="es_valor_fijo">Actualiza si el parametro es necesario o no para el consumo del web service</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización de datos del registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarProveedorParametros(int id_proveedor_ws, string nombre_parametro, string valor_parametro, bool es_valor_fijo, int id_usuario)
        {
            //Invoca al método que actualiza un registro
            return this.editarProveedorParametros(id_proveedor_ws, this._secuencia, nombre_parametro, valor_parametro, es_valor_fijo, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un registro de Proveedor Parametros
        /// </summary>
        /// <param name="id_usuario">Identificador del usuario que deshabilito el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaProveedorParametros(int id_usuario)
        {
            //Invoca al método que actualiza un registro
            return this.editarProveedorParametros(this._id_proveedor_ws, this._secuencia, this._nombre_parametro, this._valor_parametro, this._es_valor_fijo, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaProveedorWSParametros()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_proveedor_ws_parametros);
        }
        /// <summary>
        /// Método encargado de Obtener los Parametros del WS
        /// </summary>
        /// <param name="id_proveedor_ws">Proveedor del Servicio Web</param>
        /// <returns></returns>
        public static DataTable ObtieneParametrosProveedorWS(int id_proveedor_ws)
        {
            //Declarando Objeto de Retorno
            DataTable dtParametros = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro de parametros de proveedor
            object[] param = { 4, 0, id_proveedor_ws, 0, "", "", false, 0, false, "", "" };

            //Obteniendo Parametros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan los Parametros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtParametros = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtParametros;
        }


        #endregion
    }
}
