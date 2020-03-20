using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using TSDK.Base;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con los Esquemas de Facturación
    /// </summary>
    public class EsquemasFacturacion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe.sp_esquemas_facturacion_tef";

        private int _id_esquema_facturacion;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Esquema de Facturación
        /// </summary>
        public int id_esquema_facturacion { get { return this._id_esquema_facturacion; } }
        private string _version;
        /// <summary>
        /// Atributo encargado de almacenar la Versión del CDFI
        /// </summary>
        public string version { get { return this._version; } }
        private XmlDocument _xsd;
        /// <summary>
        /// Atributo encargado de almacenar el XML Schema
        /// </summary>
        public XmlDocument xsd { get { return this._xsd; } }
        private int _id_compania_receptora;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compania Receptora
        /// </summary>
        public int id_compania_receptora { get { return this._id_compania_receptora; } }
        private byte _id_tipo_xsd;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de XSD
        /// </summary>
        public byte id_tipo_xsd { get { return this._id_tipo_xsd; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public EsquemasFacturacion()
        {   //Asignando Valores
            this._id_esquema_facturacion = 0;
            this._version = "";
            this._xsd = null;
            this._id_compania_receptora = 0;
            this._id_tipo_xsd = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_esquema_facturacion">Id de Esquema de Facturación</param>
        public EsquemasFacturacion(int id_esquema_facturacion)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_esquema_facturacion);
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dada la Version
        /// </summary>
        /// <param name="version"></param>
        public EsquemasFacturacion(string version)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(version);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~EsquemasFacturacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_esquema_facturacion">Id de Esquema de Facturación</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_esquema_facturacion)
        {   //Declarando Objeto de retorno
            bool result = false;
            //Armando Arreglo de parametros
            object[] param = { 3, id_esquema_facturacion, "", null, 0, 0, 0, false, "", "" };
            //Instanciando Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_esquema_facturacion = id_esquema_facturacion;
                        this._version = dr["Version"].ToString();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(dr["XSD"].ToString());
                        this._xsd = doc;
                        this._id_compania_receptora = Convert.ToInt32(dr["IdCompaniaReceptora"]);
                        this._id_tipo_xsd = Convert.ToByte(dr["IdTipoXSD"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="version">Versión del Esquema de Facturación</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(string version)
        {   //Declarando Objeto de retorno
            bool result = false;
            //Armando Arreglo de parametros
            object[] param = { 4, 0, version, null, 0, 0, 0, false, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_esquema_facturacion = Convert.ToInt32(dr["Id"]);
                        this._version = dr["Version"].ToString();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(dr["XSD"].ToString());
                        this._xsd = doc;
                        this._id_compania_receptora = Convert.ToInt32(dr["IdCompaniaReceptora"]);
                        this._id_tipo_xsd = Convert.ToByte(dr["IdTipoXSD"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Cargar los Esquemas de los Complementos de los CFDI's
        /// </summary>
        /// <param name="version_cfdi_padre">Versión del CFDI Padre</param>
        /// <param name="id_compania_receptor">Compania Emisora para Obtener Addendas Ligadas</param>
        /// <param name="tiene_addenda">Estatus que Verifica si la Factura Tiene Addenda</param>
        /// <returns></returns>
        public static string[] CargaEsquemasPadreYComplementosCFDI(string version_cfdi_padre, int id_compania_receptor, out bool tiene_addenda)
        {   //Declarando Objeto de Retorno
            string[] esquemas;
            //Asignando Valor
            tiene_addenda = false;
            //Armando Arreglo de Parametros
            object[] param = { 5, 0, version_cfdi_padre, null, id_compania_receptor, 0, 0, false, "", "" };
            //Instanciando Valores
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Declarando Contador
                    int contador = 0;
                    //Creando Arreglo Dinamico
                    esquemas = new string[ds.Tables["Table"].Rows.Count];
                    //Iniciando Ciclo
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Añadiendo Esquemas
                        esquemas[contador] = dr["XSD"].ToString();
                        //Incrementando Contador
                        contador++;
                        //validando si ya existe una Addenda
                        if (!tiene_addenda)
                            //Validando si el Tipo es Addenda
                            tiene_addenda = Convert.ToByte(dr["Tipo"]) == 3 ? true : false;
                    }
                }
                else//Asignando Resultado Nulo
                    esquemas = null;
            }
            //Devolviendo 
            return esquemas;
        }

        #endregion
    }
}
