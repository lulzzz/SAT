using SAT_CL.Global;
using System;
using System.Data;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;


namespace SAT_CL.Monitoreo
{
    /// <summary>
    /// Clase que inserta, Actualiza y Consulta registros de proveedor de Web Service
    /// </summary>
    public class ProveedorWS : Disposable
    {
        #region Enumeracion
        /// <summary>
        /// Enumera los tipos de codificación de información de WS
        /// </summary>
        public enum Tipo
        {
            SOAP = 1,
            REST,
            RESTFUL,
            FTP,
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de proveedor de Web Service 
        /// </summary>
        private static string nom_sp = "monitoreo.sp_proveedor_ws_tpws";

        private int _id_proveedor_ws;
        /// <summary>
        /// Identifica el registro de un proveedor de web service
        /// </summary>
        public int id_proveedor_ws
        {
            get { return _id_proveedor_ws; }
        }
        private int _id_compania;
        /// <summary>
        /// Identifica a la compañia a la que pertenece el servicio de rastreo satelital
        /// </summary>
        public int id_compania
        {
            get { return _id_compania; }
        }
        private int _id_proveedor;
        /// <summary>
        /// Identifica al proveedor se servicio de rastreo satelital
        /// </summary>
        public int id_proveedor
        {
            get { return _id_proveedor; }
        }
        private byte _id_tipo;
        /// <summary>
        /// Define el tipo de codificación del información entre el Web Service y la aplicación (SOAP,REST, RESFUL)
        /// </summary>
        public byte id_tipo
        {
            get { return _id_tipo; }
        }
        /// <summary>
        /// Permite tener acceso a los elementos de la enumeración de tipo (SOAP,REST, RESTFUL);
        /// </summary>
        public Tipo tipo
        {
            get { return (Tipo)this._id_tipo; }
        }
        private string _identificador;
        /// <summary>
        /// Permite identificar rapidamente a un proveedor de servicio satelital
        /// </summary>
        public string identificador
        {
            get { return _identificador; }
        }
        private string _endpoin;
        /// <summary>
        /// Punto final de acceso a un web service
        /// </summary>
        public string endpoin
        {
            get { return _endpoin; }
        }
        private string _accion;
        /// <summary>
        /// Identifica la acción que se puede hacer dentro del web service
        /// </summary>
        public string accion
        {
            get { return _accion; }
        }
        private int _id_accion;
        /// <summary>
        /// Identifica el registro de id accion 
        /// </summary>
        public int id_accion
        {
            get { return _id_accion; }
        }
        private string _usuario;
        /// <summary>
        /// Nombre del usuario que tiene acceso al web service
        /// </summary>
        public string usuario
        {
            get { return _usuario; }
        }
        private string _contraseña;
        /// <summary>
        /// Contraseña de acceso al web service
        /// </summary>
        public string contraseña
        {
            get { return _contraseña; }
        }
        private bool _habilitar;
        /// <summary>
        /// Cambia el estado de habilitación de un registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        private bool _obtienePosicionesGeneral;
        /// <summary>
        /// 
        /// </summary>
        public bool obtienePosicionesGeneral
        {
            get { return _obtienePosicionesGeneral; }
        }

        #endregion

        #region Contructores
        /// <summary>
        /// Constructor que inicializa los atributos en cero
        /// </summary>
        public ProveedorWS()
        {
            this._id_proveedor_ws = 0;
            this._id_compania = 0;
            this._id_proveedor = 0;
            this._id_tipo = 0;
            this._identificador = "";
            this._endpoin = "";
            this._accion = "";
            this._id_accion = 0;
            this._usuario = "";
            this._contraseña = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro de Proveedor de web service
        /// </summary>
        /// <param name="id_proveedor_ws"></param>
        public ProveedorWS(int id_proveedor_ws)
        {
            //Invoca al método que realiza la busqueda y asignación de regsitros de proveedor de web service
            cargaAtributos(id_proveedor_ws);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase 
        /// </summary>
        ~ProveedorWS()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro de proveedor y los asigna a los atributos de la clase
        /// </summary>
        /// <param name="id_proveedor_ws">Identifica a un registro de Proveedor de Web service</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_proveedor_ws)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios pra realizar la busqueda de un registro
            object[] param = { 3, id_proveedor_ws, 0, 0, 0, "", "", "", 0, "", "", 0, false, "", "" };
            //Ínvoca al método que realiza la busqueda del registro y lo alamcena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre los campos del dataset y los alamcena en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_proveedor_ws = id_proveedor_ws;
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._identificador = Convert.ToString(r["Identificador"]);
                        this._endpoin = Convert.ToString(r["Endpoint"]);
                        this._accion = Convert.ToString(r["Accion"]);
                        this._id_accion = Convert.ToInt32(r["IdAccion"]);
                        this._usuario = Convert.ToString(r["Usuario"]);
                        this._contraseña = Convert.ToString(r["Contraseña"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                        this._obtienePosicionesGeneral = r["ObtienePosicionesGeneral"].ToString().Equals("1") ? true : false;
                    }
                    //Cambia el valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retrono
            return retorno;
        }
        /// <summary>
        /// Método que realiza la actualización de un registro de proveedor de web service
        /// </summary>
        /// <param name="id_compania">Actualiza la compañia a la que pertenece el servicio de proveedor de rastreo satelital</param>
        /// <param name="id_proveedor">Actualiza el proveedor de servicio satelital</param>
        /// <param name="tipo">Actualiza el tipo de codificación de informacion para el web service</param>
        /// <param name="identificador">Actualiza el nombre con el que se identifica rapidamente a un proveedor de web service</param>
        /// <param name="endpoin">Actualiza el enpoint del web service</param>
        /// <param name="accion">Actualiza la acción del web service</param>
        /// <param name="id_accion">Actualiza el idacción del web service</param>
        /// <param name="usuario">Actualiza el usuario que tiene acceso al web service</param>
        /// <param name="contraseña">Actualiza la contraseña de acceso al web service</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro</param>
        /// <param name="habilitar">Actualiza el estado de disponibilidad del regsitro</param>
        /// <returns></returns>
        private RetornoOperacion editarProveedorWS(int id_compania, int id_proveedor, Tipo tipo, string identificador, string endpoin, string accion, int id_accion, string usuario,
                                                   string contraseña, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que alamcena los datos necesarios para actualizar un registro
            object[] param = { 2, this._id_proveedor_ws, id_compania, id_proveedor, tipo, identificador, endpoin, accion, id_accion, usuario, contraseña, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método que actualiza el registro de proveedor de web service
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;

        }
        #endregion

        #region Método Públicos
        /// <summary>
        /// Método que realiza la inserción de un registro de proveedor de web service
        /// </summary>
        /// <param name="id_compania">Inserta la compañia a la que pertenece el servicio de proveedor de rastreo satelital</param>
        /// <param name="id_proveedor">Inserta el proveedor de servicio satelital</param>
        /// <param name="tipo">Inserta el tipo de codificación de informacion para el web service</param>
        /// <param name="identificador">Inserta el nombre con el que se identifica rapidamente a un proveedor de web service</param>
        /// <param name="endpoin">Inserta el enpoint del web service</param>
        /// <param name="accion">Inserta la acción del web service</param>
        /// <param name="id_accion">Actualiza el idacción del web service</param>
        /// <param name="usuario">Inserta el usuario que tiene acceso al web service</param>
        /// <param name="contraseña">Inserta la contraseña de acceso al web service</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la actualización del registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarProveedorWS(int id_compania, int id_proveedor, Tipo tipo, string identificador, string endpoin, string accion, int id_accion, string usuario,
                                                   string contraseña, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que alamcena los datos necesarios para insertar un registro
            object[] param = { 1, 0, id_compania, id_proveedor, tipo, identificador, endpoin, accion, id_accion, usuario, contraseña, id_usuario, true, "", "" };
            //Asigna al objeto retorno el resultado del método que inserta el registro de proveedor de web service
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que realiza la actualización de un registro de proveedor de web service
        /// </summary>
        /// <param name="id_compania">Actualiza la compañia a la que pertenece el servicio de proveedor de rastreo satelital</param>
        /// <param name="id_proveedor">Actualiza el proveedor de servicio satelital</param>
        /// <param name="tipo">Actualiza el tipo de codificación de informacion para el web service</param>
        /// <param name="identificador">Actualiza el nombre con el que se identifica rapidamente a un proveedor de web service</param>
        /// <param name="endpoin">Actualiza el enpoint del web service</param>
        /// <param name="accion">Actualiza la acción del web service</param>
        /// <param name="id_accion">Actualiza el idacción del web service</param>
        /// <param name="usuario">Actualiza el usuario que tiene acceso al web service</param>
        /// <param name="contraseña">Actualiza la contraseña de acceso al web service</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarProveedorWS(int id_compania, int id_proveedor, Tipo tipo, string identificador, string endpoin, string accion, int id_accion, string usuario,
                                                   string contraseña, int id_usuario)
        {
            //Invoca al método que realiza la edición de un registro
            return editarProveedorWS(id_compania, id_proveedor, (Tipo)tipo, identificador, endpoin, accion, id_accion, usuario, contraseña, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un registro de proveedor WS
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo la accioón</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarProveedorWS(int id_usuario)
        {
            //Invoca al método que realiza la edición de un registro
            return editarProveedorWS(this._id_compania, this._id_proveedor, (Tipo)this._id_tipo, this._identificador, this._endpoin, this._accion, this._id_accion, this._usuario, this._contraseña, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaProveedorWS()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_proveedor_ws);
        }
        /// <summary>
        /// Método encargado de Consumir el Servicio Web del Proveedor
        /// </summary>
        /// <param name="ensobretado">Datos Ensobretados para Petición</param>
        /// <param name="datosObtenidos">Resultado del Web Service</param>
        /// <returns></returns>
        public RetornoOperacion ConsumeProveedorWS(XDocument ensobretado, out XDocument datosObtenidos)
        {
            //Creación del objeto retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Tipo de WS
            switch ((Tipo)this._id_tipo)
            {
                case Tipo.SOAP:
                    {
                        //Obteniendo Elemento Root
                        //XElement soapEnvelope = ProveedorWSCargaParametros.ObtieneXMLParametrosProveedorWS(this._id_proveedor_ws).Root;

                        //Obteniendo Respuesta del WS
                        result = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._endpoin), ensobretado);
                        break;
                    }
            }

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            
                //Asignando Datos
                datosObtenidos = XDocument.Parse(result.Mensaje);
            else
                //Instanciando Excepción
                datosObtenidos = XDocument.Parse(result.ToXMLString());
            
            //Retorna al método el objeto retorno
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener los Proveedores de FTP
        /// </summary>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static DataTable ObtieneProveedoresFTP(int id_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtProveedoresFTP = null;

            //Creación del arreglo que alamcena los datos necesarios para insertar un registro
            object[] param = { 4, 0, id_compania, 0, 0, "", "", "", 0, "", "", 0, false, "", "" };

            //Obteniendo Datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando Resultado
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores Obtenidos
                    dtProveedoresFTP = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtProveedoresFTP;
        }

        #endregion 
    }
}
