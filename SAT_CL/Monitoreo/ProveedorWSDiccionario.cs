using System;
using System.Data;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Monitoreo
{
    public class ProveedorWSDiccionario:Disposable
    {

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de ProveedorWSDiccionario 
        /// </summary>
        private static string nom_sp = "monitoreo.sp_proveedor_ws_diccionario_tpwsd";

        private int _id_proveedor_ws_diccionario;
        /// <summary>
        /// Identifica el registro de un ProveedorWSDiccionario
        /// </summary>
        public int id_proveedor_ws_diccionario
        {
            get { return _id_proveedor_ws_diccionario; }
        }
        private int _id_proveedor_ws;
        /// <summary>
        ///Identifica el registro de un proveedor web service
        /// </summary>
        public int id_proveedor_ws
        {
            get { return _id_proveedor_ws; }
        }
        /// <summary>
        /// Identifica la tabla
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_tabla;
        /// <summary>
        /// Identifica el registro
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private int _id_registro;
        /// <summary>
        /// Identifica el estatus
        /// </summary>
        public int id_estatus
        {
            get { return _id_estatus; }
        }
        private int _id_estatus;
       
        private string _identificador;
        /// <summary>
        /// Permite identificar proveedor web service
        /// </summary>
        public string identificador
        {
            get { return _identificador; }
        }
        /// <summary>
        /// Identifica tipo 
        /// </summary>
        public int tipo_identificador
        {
            get { return _tipo_identificador; }
        }
        private int _tipo_identificador;
        private string _alias;
        /// <summary>
        /// atributo alias
        /// </summary>
        public string alias
        {
            get { return _alias; }
        }     
        private string _serie;
        /// <summary>
        /// atributo serie 
        /// </summary>
        public string serie
        {
            get { return _serie; }
        }   
        private bool _habilitar;
        /// <summary>
        /// Cambia el estado de habilitación de un registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Contructores
        /// <summary>
        /// Constructor que inicializa los atributos en cero
        /// </summary>
        public ProveedorWSDiccionario()
        {
            this._id_proveedor_ws_diccionario = 0;
            this._id_proveedor_ws = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._id_estatus = 0;
            this._identificador = "";
            this._tipo_identificador = 0;
            this._alias = "";
            this._serie = "";
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro de ProveedorWSDiccionario
        /// </summary>
        /// <param name="id_proveedor_ws_diccionario"></param>
        public ProveedorWSDiccionario(int id_proveedor_ws_diccionario)
        {
            //Invoca al método que realiza la busqueda y asignación de regsitros de ProveedorWSDiccionario
            cargaAtributos(id_proveedor_ws_diccionario);
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro de ProveedorWSDiccionario
        /// </summary>
        /// <param name="identificador"></param>
        /// <param name="idcompania"></param>
        public ProveedorWSDiccionario(string identificador , int idcompania)
        {   //Invocando Método de Cargado
            cargaAtributosIdenteficador(identificador, idcompania);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase 
        /// </summary>
        ~ProveedorWSDiccionario()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro de proveedor y los asigna a los atributos de la clase
        /// </summary>
        /// <param name="id_proveedor_ws_diccionario">Identifica a un registro de ProveedorWSDiccionario</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_proveedor_ws_diccionario)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios pra realizar la busqueda de un registro
            object[] param = { 3, id_proveedor_ws_diccionario, 0, 0, 0, 0, "", 0, "", "", 0, false, "", "" };
            //Ínvoca al método que realiza la busqueda del registro y lo alamcena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre los campos del dataset y los alamcena en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_proveedor_ws_diccionario = id_proveedor_ws_diccionario;
                        this._id_proveedor_ws= Convert.ToInt32(r["IdProveedor"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._id_estatus = Convert.ToInt32(r["Estatus"]);
                        this._identificador = Convert.ToString(r["Identificador"]);
                        this._tipo_identificador = Convert.ToInt32(r["TipoIdentificador"]);
                        this._alias = Convert.ToString(r["Alias"]);
                        this._serie = Convert.ToString(r["Serie"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retrono
            return retorno;
        }
        /// <summary>
        /// Método que realiza la busqueda de un registro de proveedor y los asigna a los atributos de la clase
        /// </summary>
        /// <param name="identificador">Identifica a un registro de identificador</param>
        /// <param name="idcompania">Identifica a un registro de idcompania</param>
        /// <returns></returns>
        private bool cargaAtributosIdenteficador(string identificador, int idcompania)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios pra realizar la busqueda de un registro
            object[] param = { 5, 0, idcompania, 0, 0, 0, identificador, 0, "", "", 0, false, "", "" };
            //Ínvoca al método que realiza la busqueda del registro y lo alamcena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre los campos del dataset y los alamcena en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_proveedor_ws_diccionario = id_proveedor_ws_diccionario;
                        this._id_proveedor_ws = Convert.ToInt32(r["IdProveedor"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._id_estatus = Convert.ToInt32(r["Estatus"]);
                        this._identificador = Convert.ToString(r["Identificador"]);
                        this._tipo_identificador = Convert.ToInt32(r["TipoIdentificador"]);
                        this._alias = Convert.ToString(r["Alias"]);
                        this._serie = Convert.ToString(r["Serie"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retrono
            return retorno;
        }
        /// <summary>
        /// Método que realiza la actualización de un registro de ProveedorWSDiccionario
        /// </summary>
        /// <param name="id_proveedor_ws">Id ProvedorWS</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_estatus">Id Estutas</param>
        /// <param name="identificador">Id Identificador Proveedor</param>
        /// <param name="tipo_identificador">Tipo de dato Identificador</param>
        /// <param name="alias">Alias</param>
        /// <param name="serie">Serie</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro</param>
        /// <param name="habilitar">Actualiza el estado de disponibilidad del regsitro</param>
        /// <returns></returns>
        private RetornoOperacion editarProveedorWSDiccionario(int id_proveedor_ws, int id_tabla, int id_registro, int id_estatus, string identificador, int tipo_identificador, string alias, string serie,
                                                   int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que alamcena los datos necesarios para actualizar un registro
            object[] param = { 2, this._id_proveedor_ws_diccionario, id_proveedor_ws, id_tabla, id_registro, id_estatus, identificador, tipo_identificador, alias, serie, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método que actualiza el registro de ProveedorWSDiccionario
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;

        }
        #endregion

        #region Método Públicos
        /// <summary>
        /// Método que realiza la actualización de un registro de ProveedorWSDiccionario
        /// </summary>
        /// <param name="id_proveedor_ws">Id ProvedorWS</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_estatus">Id Estutas</param>
        /// <param name="identificador">Id Identificador Proveedor</param>
        /// <param name="tipo_identificador">Tipo de dato Identificador</param>
        /// <param name="alias">Alias</param>
        /// <param name="serie">Serie</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro</param>
        /// <param name="habilitar">Actualiza el estado de disponibilidad del regsitro</param>
        /// <returns></returns>
        public  RetornoOperacion InsertarProveedorWSDiccionario(int id_proveedor_ws, int id_tabla, int id_registro, int id_estatus, string identificador, int tipo_identificador, string alias, string serie,
                                                   int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que alamcena los datos necesarios para insertar un registro
            object[] param = { 1, 0, id_proveedor_ws, id_tabla, id_registro, id_estatus, identificador, tipo_identificador, alias, serie, id_usuario, true, "", "" };
            //Asigna al objeto retorno el resultado del método que inserta el registro de ProveedorWSDiccionario
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que realiza la actualización de un registro de ProveedorWSDiccionario
        /// </summary>
        /// <param name="id_proveedor_ws">Id ProvedorWS</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_estatus">Id Estutas</param>
        /// <param name="identificador">Id Identificador Proveedor</param>
        /// <param name="tipo_identificador">Tipo de dato Identificador</param>
        /// <param name="alias">Alias</param>
        /// <param name="serie">Serie</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro</param>
        /// <param name="habilitar">Actualiza el estado de disponibilidad del regsitro</param>
        /// <returns></returns>
        public RetornoOperacion EditarProveedorWSDiccionario(int id_proveedor_ws, int id_tabla, int id_registro, int id_estatus, string identificador, int tipo_identificador, string alias, string serie,
                                                   int id_usuario)
        {
            //Invoca al método que realiza la edición de un registro
            return editarProveedorWSDiccionario(id_proveedor_ws, id_tabla, id_registro, id_estatus, identificador, tipo_identificador, alias, serie, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un registro de proveedor WS
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo la accioón</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarProveedorWSDiccionario(int id_usuario)
        {
            //Invoca al método que realiza la edición de un registro
            return editarProveedorWSDiccionario(this._id_proveedor_ws, this.id_tabla, this._id_registro, this._id_estatus, this._identificador, this._tipo_identificador, this._alias, this._serie, id_usuario, false);
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
        /// Método encargado de Obtener los Parametros del Web Service del Proveedor
        /// </summary>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <returns></returns>
        public static DataTable ObtieneRegistrosDiccionarioWS(int id_tabla, int id_registro)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando Arreglo de Parametros
            //object[] param = { 0, "", "", "", "" };
            object[] param = { 4, 0, 0, id_tabla, id_registro, 0, "", 0, "", "", 0, true, "", "" };

            //Obteniendo Parametros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        #endregion 
    }
}
