using SAT_CL.Global;
using System;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;
using FEv33 = SAT_CL.FacturacionElectronica33;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Clase que muestra los Comportamientos y Estados del Emisor de la Addenda
    /// </summary>
    public class AddendaEmisor : Disposable
    {
        #region Atributos

        private static string nom_sp = "fe.sp_addenda_emisor_tea";

        private int _id_emisor_addenda;
        /// <summary>
        /// Atributo Encargado de almacenar el Id del Emisor de la Addenda
        /// </summary>
        public int id_emisor_addenda
        {
            get { return this._id_emisor_addenda; }
        }

        private int _id_emisor;
        /// <summary>
        /// Atributo Encargado de almacenar el Id del Emisor
        /// </summary>
        public int id_emisor
        {
            get { return this._id_emisor; }
        }

        private int _id_receptor;
        /// <summary>
        /// Atributo Encargado de almacenar el Id del Receptor
        /// </summary>
        public int id_receptor
        {
            get { return this._id_receptor; }
        }

        private int _id_addenda;
        /// <summary>
        /// Atributo Encargado de almacenar el Id de la Addenda
        /// </summary>
        public int id_addenda
        {
            get { return this._id_addenda; }
        }

        private XmlDocument _xml_predeterminado;
        /// <summary>
        /// Atributo Encargado de almacenar el archivo XML Predeterminado
        /// </summary>
        public XmlDocument xml_predeterminado
        {
            get { return this._xml_predeterminado; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo Encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Enumeraciones

        /// <summary>
        /// Define todos los receptores que solicitan addenda
        /// </summary>
        public enum Receptor
        {
            /// <summary>
            /// COLGATE PALMOLIVE S.A. DE C.V.
            /// </summary>
            ColgatePalmoliveTEM = 1078,
            /// <summary>
            /// MISSION HILL SA DE CV
            /// </summary>
            MissionHillsTEM = 1135
            /*// <summary>
            /// Cosbel
            /// </summary>
            Cosbel = 14,
            /// <summary>
            /// Kraft Foods de México
            /// </summary>
            KraftFoods = 158,
            /// <summary>
            /// Liverpool
            /// </summary>
            TiendasDepartamentalesLiverpool = 261,
            /// <summary>
            /// Liverpool
            /// </summary>
            OperadoraLiverpoolMexico = 262,
            /// <summary>
            /// Liverpool
            /// </summary>
            BodegasLiverpool = 544,
            /// <summary>
            /// Liverpool
            /// </summary>
            AlmacenadoraLiverpool = 545,
            /// <summary>
            /// Liverpool
            /// </summary>
            DistribuidoraLiverpool = 546,
            /// <summary>
            /// Liverpool
            /// </summary>
            ModaJovenSFERA = 547,
            /// <summary>
            /// Liverpool
            /// </summary>
            ImportacionesFACTUM = 548,
            /// <summary>
            /// Liverpool
            /// </summary>
            ImportadoraGLOBASTIC = 549,
            /// <summary>
            /// Liverpool
            /// </summary>
            ServiceTrading = 550,
            /// <summary>
            /// Liverpool
            /// </summary>
            PuertoLiverpool = 566,
            /// <summary>
            /// Liverpool
            /// </summary>
            OperadoraComercialLiverpool = 757,
            /// <summary>
            /// Proteinas y Oleicos
            /// </summary>
            ProteinasYOleicos = 166,
            /// <summary>
            /// Colgate Palmolive
            /// </summary>
            TEMColgatePalmolive = 1078,
            /// <summary>
            /// Colgate Palmolive
            /// </summary>
            MissionHills = 200,
            /// <summary>
            /// L'oreal
            /// </summary>
            Frabel = 357,
            /// <summary>
            /// Aluprint División Plegadizos
            /// </summary>
            AluprintPlegadizos = 630,
            /// <summary>
            /// Mabe
            /// </summary>
            Mabe = 673,
            /// <summary>
            /// Aluprint
            /// </summary>
            Aluprint = 856,
            /// <summary>
            /// Cerveceria Modelo de Guadalajara
            /// </summary>
            CerveceriaModeloGuadalajara = 878,
            /// <summary>
            /// Cerveceria Modelo de Torreon
            /// </summary>
            CerveceriaModeloTorreon = 891//*/
        }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicia los Valores por Default
        /// </summary>
        public AddendaEmisor()
        {   //Invoca el Método de CargarValoresInstancia
            cargaValoresInstancia();
        }
        /// <summary>
        /// Constructor que inicia los Valores dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public AddendaEmisor(int id_registro)
        {//Invoca el Método de CargarValoresInstancia
            cargaValoresInstancia(id_registro);
        }
        /// <summary>
        /// Constructor que inicia los Valores dado los Id's de Emisor, Receptor, Addenda
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        /// <param name="id_addenda">Id de Addenda</param>
        public AddendaEmisor(int id_emisor, int id_receptor, int id_addenda)
        {//Invoca el Método de CargarValoresInstancia
            cargaValoresInstancia(id_emisor, id_receptor, id_addenda);
        }

        /// <summary>
        /// Constructor que inicia los Valores dado los Id's de Emisor, Receptor, Versión del Complemento de Nómina
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        /// <param name="version">Versión del Complemento de Nómina</param>
        public AddendaEmisor(int id_emisor, int id_receptor, string  version)
        {//Invoca el Método de CargarValoresInstancia
            cargaValoresInstancia(id_emisor, id_receptor, version);
        }

        /// <summary>
        /// Constructor que inicia los Valores dado los Id's de Emisor, Receptor
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        public AddendaEmisor(int id_emisor, int id_receptor)
        {//Invoca el Método de CargarValoresInstancia
            cargaValoresInstancia(id_emisor, id_receptor);
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la Clase clAddenda
        /// </summary>
        ~AddendaEmisor()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método Privado encargado de cargar los Valores por Default
        /// </summary>
        private void cargaValoresInstancia()
        {   //Valores por Default
            this._id_emisor_addenda = 0;
            this._id_emisor = 0;
            this._id_receptor = 0;
            this._xml_predeterminado = null;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de cargar los Valores en base a un registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        private bool cargaValoresInstancia(int id_registro)
        {   //Declarando parametros
            object[] param = { 3, id_registro, 0, 0, 0, null, 0, false, "", "" };
            //Declarando Variables de Retorno
            bool result = false;
            //Utilizando DataSet para Ejecutar StoreProcedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando el Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Obteniendo cada Fila de la Tabla del DataSet
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_emisor_addenda = id_registro;
                        this._id_emisor = Convert.ToInt32(dr["IdEmisor"]);
                        this._id_receptor = Convert.ToInt32(dr["IdReceptor"]);
                        this._id_addenda = Convert.ToInt32(dr["IdAddenda"]);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(dr["XMLPredeterminado"].ToString());
                        this._xml_predeterminado = xmldoc;
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }
     
        /// <summary>
        /// Método encargado de Cargar los Valores dado el emisor, receptor y addenda
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        /// <param name="id_addenda">Id de Addenda</param>
        /// <returns></returns>
        private bool cargaValoresInstancia(int id_emisor, int id_receptor, int id_addenda)
        {   //Declarando parametros
            object[] param = { 4, 0, id_emisor, id_receptor, id_addenda, "", 0, false, "", "" };
            //Declarando Variables de Retorno
            bool result = false;
            //Utilizando DataSet para Ejecutar StoreProcedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando el Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table")) 
                {   //Obteniendo cada Fila de la Tabla del DataSet
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_emisor_addenda = Convert.ToInt32(dr["Id"]);
                        this._id_emisor = Convert.ToInt32(dr["IdEmisor"]);
                        this._id_receptor = Convert.ToInt32(dr["IdReceptor"]);
                        this._id_addenda = Convert.ToInt32(dr["IdAddenda"]);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(dr["XMLPredeterminado"].ToString());
                        this._xml_predeterminado = xmldoc;
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Método encargado de Cargar los Valores dado el emisor, receptor 
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        /// <returns></returns>
        private bool cargaValoresInstancia(int id_emisor, int id_receptor)
        {   //Declarando parametros
            object[] param = { 9, 0, id_emisor, id_receptor, 0, "", 0, false, "", "" };
            //Declarando Variables de Retorno
            bool result = false;
            //Utilizando DataSet para Ejecutar StoreProcedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando el Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Obteniendo cada Fila de la Tabla del DataSet
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_emisor_addenda = Convert.ToInt32(dr["Id"]);
                        this._id_emisor = Convert.ToInt32(dr["IdEmisor"]);
                        this._id_receptor = Convert.ToInt32(dr["IdReceptor"]);
                        this._id_addenda = Convert.ToInt32(dr["IdAddenda"]);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(dr["XMLPredeterminado"].ToString());
                        this._xml_predeterminado = xmldoc;
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Método encargado de Cargar los Valores dado el emisor, receptor y addenda
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        ///<param name="version">Versión del Complemento de Nómina</param>
        /// <returns></returns>
        private bool cargaValoresInstancia(int id_emisor, int id_receptor, string version)
        {   //Declarando parametros
            object[] param = { 10, 0, id_emisor, id_receptor, 0, "", 0, false, version, "" };
            //Declarando Variables de Retorno
            bool result = false;
            //Utilizando DataSet para Ejecutar StoreProcedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando el Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Obteniendo cada Fila de la Tabla del DataSet
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_emisor_addenda = Convert.ToInt32(dr["Id"]);
                        this._id_emisor = Convert.ToInt32(dr["IdEmisor"]);
                        this._id_receptor = Convert.ToInt32(dr["IdReceptor"]);
                        this._id_addenda = Convert.ToInt32(dr["IdAddenda"]);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(dr["XMLPredeterminado"].ToString());
                        this._xml_predeterminado = xmldoc;
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }
        /// Método encargado de la Actualizaciones de los Registros
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id de Receptor</param>
        /// <param name="id_addenda">Id de Addenda</param>
        /// <param name="xml">XML</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaAddendaEmisor(int id_emisor, int id_receptor,
            int id_addenda, XmlDocument xml, int id_usuario, bool habilitar)
        {   //Parametros del SP
            object[] param = { 2, this.id_emisor_addenda, id_emisor, id_receptor, id_addenda,
                             xml.InnerXml, id_usuario, habilitar, "", ""};
            //Regresando el Resultado del SP
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
        }

        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método encargado de Ingresar Registros
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        /// <param name="id_addenda">Id de Addenda</param>
        /// <param name="xml">XML</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion IngresarAddendaEmisor(int id_emisor, int id_receptor,
            int id_addenda, XmlDocument xml, int id_usuario)
        {
            //Declarando Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Parametros del SP
            object[] param = { 1, 0, id_emisor, id_receptor, id_addenda, xml.InnerXml, id_usuario, true, "", "" };
            //Regresando el resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Devolvemos Resultado
            return result;
        }

        /// <summary>
        /// Método Público encargado de Editar el registro actual
        /// </summary>
        /// <param name="id_emisor">Id Emisor</param>
        /// <param name="id_receptor">Id Receptor</param>
        /// <param name="id_addenda">Id de Addenda</param>
        /// <param name="xml">XML</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaAddendaEmisor(int id_emisor, int id_receptor,
            int id_addenda, XmlDocument xml, int id_usuario)
        {   //Regresa el Resultado del Método "editaAddendaEmisor"
            return this.editaAddendaEmisor(id_emisor, id_receptor, id_addenda, xml, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método encargado de Deshabilitar un Registro en base a un Id
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAddendaEmisor(int id_usuario)
        {   //Regresa el Resultado del Método "editaAddendaEmisor"
            return this.editaAddendaEmisor(this._id_emisor, this._id_receptor, this._id_addenda, this._xml_predeterminado,
            id_usuario, false);
        }

        /// <summary>
        /// Método encargado de Actualizar los valores del registro Actual
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public bool ActualizaAddendaEmisor(int id_registro)
        {   //Regresando el resultado de la Carga de Valores
            return this.cargaValoresInstancia(id_registro);
        }

        /// <summary>
        /// Carga los registros addenda que están configurados para el Emisor y Receptor indicados
        /// </summary>
        /// <param name="id_emisor">Id de Emisor</param>
        /// <param name="id_receptor">Id de Receptor</param>
        /// <returns></returns>
        public static DataTable CargaAddendasRequeridasEmisorReceptor(int id_emisor, int id_receptor)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parámetros
            object[] param = { 5, 0, id_emisor, id_receptor, 0, "", 0, false, "", "" };

            //Realziando búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando Origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }

        /// <summary>
        /// Carga los registros complemento que están configurados para el Emisor y Receptor indicados
        /// </summary>
        /// <param name="id_emisor">Id de Emisor</param>
        /// <param name="id_receptor">Id de Receptor</param>
        /// <returns></returns>
        public static DataTable CargaComplementosRequeridosEmisorReceptor(int id_emisor, int id_receptor)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parámetros
            object[] param = { 6, 0, id_emisor, id_receptor, 0, "", 0, false, "", "" };

            //Realziando búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando Origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }

        /// <summary>
        /// Carga las Addendas Emisores ligado a un Id de Addenda
        /// </summary>
        /// <param name="id_addenda"></param>
        /// <returns></returns>
        public static DataTable CargaAddendaEmisor(int id_addenda)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parámetros
            object[] param = { 7, 0, 0, 0, id_addenda, "", 0, false, "", "" };

            //Realziando búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando Origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }

        /// <summary>
        /// Carga las Addendas Emisores ligado a un Id de Addenda
        /// </summary>
        /// <param name="id_addenda">Id Addenda</param>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <returns></returns>
        public static DataTable CargaAddendaEmisor(int id_addenda, int id_compania_emisor)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parámetros
            object[] param = { 8, 0, id_compania_emisor, 0, id_addenda, "", 0, false, "", "" };

            //Realziando búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando Origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }

        /// <summary>
        /// Carga los registros complemento que están configurados para el Emisor y Receptor indicados
        /// </summary>
        /// <param name="id_emisor">Id de Emisor</param>
        /// <param name="id_receptor">Id de Receptor</param>
        /// <param name="version">Versión</param>
        /// <returns></returns>
        public static DataTable CargaComplementosRequeridosEmisorReceptor(int id_emisor, int id_receptor, string version)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parámetros
            object[] param = { 10, 0, id_emisor, id_receptor, 0, "", 0, false, version, "" };

            //Realziando búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando Origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }
        #endregion

        #region Customización de Addenda

        /// <summary>
        /// Realiza la transformación del elemento XML de la addenda Kraft
        /// </summary>
        /// <param name="xAddenda">Objeto addenda</param>
        /// <param name="addenda">Addenda a transformar</param>
        /// <returns></returns>
        private static RetornoOperacion transformaAddendaKraft(ref object xAddenda, XElement addenda)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Recorrinedo cada elemento
            foreach (XElement ex in addenda.Elements())
            {
                //Colocando inicio de línea
                xAddenda = string.Format("{0}##{1}", resultado, ex.Name.LocalName);

                //Determinando el tipo de manejo que recibira
                switch (ex.Name.LocalName)
                {
                    case "EGC":
                        xAddenda = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", xAddenda,
                                ex.Attribute("tipoFormato"), ex.Attribute("rfcEmisor"), ex.Attribute("rfcReceptor"),
                                ex.Attribute("folioUnicoEnvio"), ex.Attribute("estadoProcesoFacturacion"), ex.Attribute("tipoFactura"));
                        break;
                    case "EA":
                        xAddenda = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6:yyyMMdd}|{7}|{8}|{9}", xAddenda,
                                ex.Attribute("partnerFunction"), ex.Attribute("nombre"), ex.Attribute("calleNumero1"),
                                ex.Attribute("calleNumero2"), ex.Attribute("colonia"), ex.Attribute("ciudad"),
                                ex.Attribute("estado"), ex.Attribute("pais"), ex.Attribute("cp"));
                        break;
                    case "ER":
                        xAddenda = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", xAddenda,
                                ex.Attribute("serie"), ex.Attribute("folio"), ex.Attribute("numeroProveedor"),
                                ex.Attribute("codigoPagadorAlterno"), ex.Attribute("correoElectronicoComprador"), ex.Attribute("localidad"),
                                ex.Attribute("numeroNotaEntrada"), ex.Attribute("moneda"));
                        break;
                    case "DI":
                        xAddenda = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}", xAddenda,
                                ex.Attribute("numeroItem"), ex.Attribute("cantidad"), ex.Attribute("unidadMedida"), ex.Attribute("cantidadUnidadMedida"),
                                ex.Attribute("precioUnitario"), ex.Attribute("precioBruto"), ex.Attribute("codigoProducto"), ex.Attribute("descripcionProducto"),
                                ex.Attribute("tipoImpuesto"), ex.Attribute("porcentajeIVA"), ex.Attribute("importeIVA"), ex.Attribute("porcentajeIEPS"),
                                ex.Attribute("importeIEPS"), ex.Attribute("numeroOC"), ex.Attribute("fechaOC"), ex.Attribute("centroCosto"),
                                ex.Attribute("materialGroup"), ex.Attribute("cuentaContable"));
                        break;
                    case "TA":
                        xAddenda = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", xAddenda,
                                ex.Attribute("cantidadPartidas"), ex.Attribute("totalBruto"),
                                ex.Attribute("totalImpuestos"), ex.Attribute("importeTotal"));
                        break;

                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #region CFDI 3.2

        /// <summary>
        /// Personaliza el contenido del elemento addenda indicado conforme a los requerimientos del cliente
        /// </summary>
        /// <param name="addenda"></param>
        /// <param name="cfdi"></param>
        /// <param name="ns_w3c"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaReceptor(ref object addenda, Comprobante cfdi)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Determinando el emisor al que se generará 
            switch ((Receptor)cfdi.id_compania_receptor)
            {
                //Colgate Palmolive
                case Receptor.ColgatePalmoliveTEM:
                case Receptor.MissionHillsTEM:
                    //Creando addenda
                    resultado = CreaAddendaColgatePalmolive(ref addenda, cfdi);
                    break;

                /*/Liverpool
                case Receptor.OperadoraLiverpoolMexico:
                case Receptor.OperadoraComercialLiverpool:
                case Receptor.BodegasLiverpool:
                case Receptor.AlmacenadoraLiverpool:
                case Receptor.DistribuidoraLiverpool:
                case Receptor.ModaJovenSFERA:
                case Receptor.ImportacionesFACTUM:
                case Receptor.ImportadoraGLOBASTIC:
                case Receptor.ServiceTrading:
                case Receptor.PuertoLiverpool:
                case Receptor.TiendasDepartamentalesLiverpool:
                    //Creando addenda
                    resultado = CreaAddendaBodegasLiverpool(ref addenda, cfdi);
                    break;

                //Proteinas y Oleicos
                case Receptor.ProteinasYOleicos:
                    //Creando addenda
                    resultado = CreaAddendaProteinasYOleicos(ref addenda, cfdi);
                    break;

                //Cosbel (L'Oreal)
                case Receptor.Cosbel:
                //Frabel (L'Oreal)
                case Receptor.Frabel:
                    //Creando addenda
                    resultado = CreaAddendaLoreal(ref addenda, cfdi);
                    break;

                //Mabe
                case Receptor.Mabe:
                    resultado = CreaAddendaMabe(ref addenda, cfdi);
                    break;

                case Receptor.AluprintPlegadizos:
                case Receptor.Aluprint:
                    resultado = CreaAddendaAluprint(ref addenda, cfdi);
                    break;

                case Receptor.CerveceriaModeloGuadalajara:
                case Receptor.CerveceriaModeloTorreon:
                    resultado = CreaAddendaModelo(ref addenda, cfdi);
                    break;//*/

                //Cualquier Receptor
                default:
                    //Conserva addenda sin cambios
                    break;
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Colgate Palmolive
        /// </summary>
        /// <param name="xAaddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaColgatePalmolive(ref object xAddenda, Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace predeterminado
            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Número de folio asignado
                addenda.Element(ns + "requestForPaymentIdentification").Element(ns + "uniqueCreatorIdentification").SetValue(cfdi.serie + cfdi.folio.ToString());
                addenda.Element(ns + "AdditionalInformation").Element(ns + "referenceIdentification").SetValue(cfdi.serie + cfdi.folio.ToString());

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Bodegas Liverpool
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaBodegasLiverpool(ref object xAddenda, Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetNamespaceOfPrefix("detallista");

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Monto con Letra
                addenda.Element(ns + "specialInstruction").Element(ns + "text").SetValue(
                              Cadena.ConvierteMontoALetra(Math.Round(cfdi.total_moneda_nacional, 2).ToString()).ToUpper());

                //Número de Contrarecibo se asigna automáticamente igual al número de orden de compra o pedido
                addenda.Element(ns + "DeliveryNote").Element(ns + "referenceIdentification").SetValue(
                        addenda.Element(ns + "AdditionalInformation").Element(ns + "referenceIdentification").Value);

                //Fecha OC (Fija a la fecha actual)
                addenda.Element(ns + "orderIdentification").Element(ns + "ReferenceDate").SetValue(DateTime.Today.ToString("yyyy-MM-dd"));
                //Fecha Contrarecibo (Fija a la fecha actual)
                addenda.Element(ns + "DeliveryNote").Element(ns + "ReferenceDate").SetValue(DateTime.Today.ToString("yyyy-MM-dd"));

                //Montos de linea de detalle
                addenda.Element(ns + "lineItem").Element(ns + "grossPrice").Element(ns + "Amount").SetValue(cfdi.total_moneda_nacional.ToString("#########0.00"));
                addenda.Element(ns + "lineItem").Element(ns + "netPrice").Element(ns + "Amount").SetValue(cfdi.total_moneda_nacional.ToString("#########0.00"));
                addenda.Element(ns + "lineItem").Element(ns + "totalLineAmount").Element(ns + "grossAmount").Element(ns + "Amount").SetValue(cfdi.total_moneda_nacional.ToString("#########0.00"));
                addenda.Element(ns + "lineItem").Element(ns + "totalLineAmount").Element(ns + "netAmount").Element(ns + "Amount").SetValue(cfdi.total_moneda_nacional.ToString("#########0.00"));
                //Monto total de la Factura
                addenda.Element(ns + "totalAmount").Element(ns + "Amount").SetValue(cfdi.total_moneda_nacional.ToString("#########0.00"));

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el elemento addenda modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Proteinas y Oleicos
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaProteinasYOleicos(ref object xAddenda, Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Número de folio asignado
                addenda.Element(ns + "requestForPaymentIdentification").Element(ns + "uniqueCreatorIdentification").SetValue(cfdi.serie + cfdi.folio.ToString());

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el lelemento modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para L'Oreal y sus empresas
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaLoreal(ref object xAddenda, Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetNamespaceOfPrefix("if");

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //SubTotal
                addenda.Element(ns + "Encabezado").Attribute("SubTotal").SetValue(cfdi.subtotal_moneda_nacional.ToString("#############.00"));

                //Instanciando detalle de IVA
                using (Impuesto i = Impuesto.RecuperaImpuestoComprobante(cfdi.id_comprobante))
                {
                    //IVA
                    addenda.Element(ns + "Encabezado").Attribute("Iva").SetValue(i.total_trasladado_moneda_nacional.ToString("#############.00"));
                    //Retención
                    addenda.Element(ns + "Encabezado").Attribute("IvaRet").SetValue(i.total_retenido_moneda_nacional.ToString("#############.00"));
                    //Total antes de retención
                    addenda.Element(ns + "Encabezado").Attribute("TotalFactura").SetValue((cfdi.subtotal_moneda_nacional + i.total_trasladado_moneda_nacional).ToString("#############.00"));
                }

                //Total de Factura
                addenda.Element(ns + "Encabezado").Attribute("Total").SetValue(cfdi.total_moneda_nacional.ToString("#############.00"));

                //Fecha de Generación
                addenda.Element(ns + "Encabezado").Attribute("Fecha").SetValue(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("yyyy-MM-ddTHH:mm:ss"));

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el lelemento modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Mabe
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaMabe(ref object xAddenda, Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetNamespaceOfPrefix("mabe");

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Serie y Folio
                addenda.Attribute("folio").SetValue(cfdi.serie + cfdi.folio.ToString());
                //Fecha de Factura
                addenda.Attribute("fecha").SetValue(cfdi.fecha_expedicion.ToString("yyyy-MM-dd"));

                //Monto con Letra
                addenda.Element(ns + "Moneda").Attribute("importeConLetra").SetValue(
                                Cadena.ConvierteMontoALetra(Math.Round(cfdi.total_moneda_nacional, 2).ToString()).ToUpper());
                //SubTotal
                addenda.Element(ns + "Subtotal").Attribute("importe").SetValue(Math.Round(cfdi.subtotal_moneda_nacional, 2).ToString("########0.00"));
                //Importe Total
                addenda.Element(ns + "Total").Attribute("importe").SetValue(Math.Round(cfdi.total_moneda_nacional, 2).ToString("########0.00"));

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el elemento modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Aluprint
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaAluprint(ref object xAddenda, Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Fecha de Facturación
                addenda.Element(ns + "requestForPayment").Attribute(ns + "deliveryDate").SetValue(DateTime.Today.ToString("yyyyMMdd"));
                //Factura / Nota Crédito
                addenda.Element(ns + "requestForPayment").Element(ns + "entityType").SetValue(cfdi.tipo_comprobante == Comprobante.TipoComprobante.Traslado ? "" :
                                                                        cfdi.tipo_comprobante == Comprobante.TipoComprobante.Ingreso ? "Factura" : "Nota_Credito");

                //Retirando elemento raíz para ajustar a definición real de addenda usada por el cliente
                xAddenda = addenda.Document.Root.Elements().ToArray();
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Kraft Foods
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaKraftFoods(ref object xAddenda, Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                /*******************************************************************/
                //Llenando elementos de forma automática en base a necesidades
                /*******************************************************************/
                //RFC Emisor
                using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(cfdi.id_compania_emisor))
                {
                    if (em.id_compania_emisor_receptor > 0)
                        addenda.Element(ns + "EGC").Attribute("rfcEmisor").SetValue(em.rfc);
                    else
                        resultado = new RetornoOperacion("Error de llenado automático de addenda: Emisor no encontrado.");
                }
                //RFC Receptor
                using (CompaniaEmisorReceptor rc = new CompaniaEmisorReceptor(cfdi.id_compania_receptor))
                {
                    if (rc.id_compania_emisor_receptor > 0)
                        addenda.Element(ns + "EGC").Attribute("rfcReceptor").SetValue(rc.rfc);
                    else
                        resultado = new RetornoOperacion("Error de llenado automático de addenda: Receptor no encontrado.");
                }
                //Serie CFDI(interno)
                addenda.Element(ns + "EGC").Attribute("serieFactura").SetValue(cfdi.serie);
                //Folio CFDI(interno)
                addenda.Element(ns + "EGC").Attribute("folioFactura").SetValue(cfdi.folio);

                //Obteniendo total de partidas del elemento DI (secuencia manual)
                int totalPartidas = (from XElement di in addenda.Elements(ns + "DI")
                                     select Convert.ToInt32(di.Attribute("numeroItem"))).Max();

                //Validando que las partidas sean mayor o igual a uno
                if (totalPartidas > 0)
                {
                    //Total de partidas
                    addenda.Element(ns + "TA").Attribute("cantidadPartidas").SetValue(totalPartidas);
                    //Subtotal
                    addenda.Element(ns + "TA").Attribute("totalBruto").SetValue(cfdi.subtotal_moneda_nacional);
                    //Impuestos
                    addenda.Element(ns + "TA").Attribute("totalImpuestos").SetValue(cfdi.impuestos_moneda_nacional);
                    //Total
                    addenda.Element(ns + "TA").Attribute("importeTotal").SetValue(cfdi.total_moneda_nacional);

                    //Se aplica formato solicitado por ciente (texto plano, con separadores # y |)
                    resultado = transformaAddendaKraft(ref xAddenda, addenda);
                }
                else
                    resultado = new RetornoOperacion("Error de llenado automático de addenda: Partidas no detectadas.");
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaModelo(ref object xAddenda, Comprobante cfdi)
        {   //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            XNamespace ns = addenda.GetDefaultNamespace();

            using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(cfdi.id_comprobante))
            {
                try
                {   //Añadiendo los elementos especiales solicitados por el recepror
                    //Número de folio asignado
                    addenda.Element(ns + "modelo:requestForPayment").Element(ns + "modelo:requestForPaymentIdentification")
                        .Element(ns + "modelo:uniqueCreatorIdentification").SetValue(cfdi.serie + cfdi.folio.ToString() + "/" +
                        Cadena.TruncaCadena(tfd.UUID, 17, ""));

                    //Actualizando cambios sobre objeto de resultado
                    xAddenda = addenda;
                }
                catch (Exception ex)
                {
                    //Registrando error
                    resultado = new RetornoOperacion(ex.Message);
                }
            }
            //Devolviendo Resultado Obtenido
            return resultado;
        }

        #endregion

        #region CFDI 3.3

        /// <summary>
        /// Personaliza el contenido del elemento addenda indicado conforme a los requerimientos del cliente
        /// </summary>
        /// <param name="addenda"></param>
        /// <param name="cfdi"></param>
        /// <param name="ns_w3c"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaReceptor(ref object addenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Determinando el emisor al que se generará 
            switch ((Receptor)cfdi.id_compania_receptor)
            {
                //Colgate Palmolive
                case Receptor.ColgatePalmoliveTEM:
                case Receptor.MissionHillsTEM:
                    //Creando addenda
                    resultado = CreaAddendaColgatePalmolive(ref addenda, cfdi);
                    break;

                /*/Liverpool
                case Receptor.OperadoraLiverpoolMexico:
                case Receptor.OperadoraComercialLiverpool:
                case Receptor.BodegasLiverpool:
                case Receptor.AlmacenadoraLiverpool:
                case Receptor.DistribuidoraLiverpool:
                case Receptor.ModaJovenSFERA:
                case Receptor.ImportacionesFACTUM:
                case Receptor.ImportadoraGLOBASTIC:
                case Receptor.ServiceTrading:
                case Receptor.PuertoLiverpool:
                case Receptor.TiendasDepartamentalesLiverpool:
                    //Creando addenda
                    resultado = CreaAddendaBodegasLiverpool(ref addenda, cfdi);
                    break;

                //Proteinas y Oleicos
                case Receptor.ProteinasYOleicos:
                    //Creando addenda
                    resultado = CreaAddendaProteinasYOleicos(ref addenda, cfdi);
                    break;

                //Cosbel (L'Oreal)
                case Receptor.Cosbel:
                //Frabel (L'Oreal)
                case Receptor.Frabel:
                    //Creando addenda
                    resultado = CreaAddendaLoreal(ref addenda, cfdi);
                    break;

                //Mabe
                case Receptor.Mabe:
                    resultado = CreaAddendaMabe(ref addenda, cfdi);
                    break;

                case Receptor.AluprintPlegadizos:
                case Receptor.Aluprint:
                    resultado = CreaAddendaAluprint(ref addenda, cfdi);
                    break;

                case Receptor.CerveceriaModeloGuadalajara:
                case Receptor.CerveceriaModeloTorreon:
                    resultado = CreaAddendaModelo(ref addenda, cfdi);
                    break;//*/

                //Cualquier Receptor
                default:
                    //Conserva addenda sin cambios
                    break;
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Colgate Palmolive
        /// </summary>
        /// <param name="xAaddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaColgatePalmolive(ref object xAddenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace predeterminado
            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Número de folio asignado
                addenda.Element(ns + "requestForPaymentIdentification").Element(ns + "uniqueCreatorIdentification").SetValue(cfdi.serie + cfdi.folio.ToString());
                addenda.Element(ns + "AdditionalInformation").Element(ns + "referenceIdentification").SetValue(cfdi.serie + cfdi.folio.ToString());

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Bodegas Liverpool
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaBodegasLiverpool(ref object xAddenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetNamespaceOfPrefix("detallista");

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Monto con Letra
                addenda.Element(ns + "specialInstruction").Element(ns + "text").SetValue(
                              Cadena.ConvierteMontoALetra(Math.Round(cfdi.total_nacional, 2).ToString()).ToUpper());

                //Número de Contrarecibo se asigna automáticamente igual al número de orden de compra o pedido
                addenda.Element(ns + "DeliveryNote").Element(ns + "referenceIdentification").SetValue(
                        addenda.Element(ns + "AdditionalInformation").Element(ns + "referenceIdentification").Value);

                //Fecha OC (Fija a la fecha actual)
                addenda.Element(ns + "orderIdentification").Element(ns + "ReferenceDate").SetValue(DateTime.Today.ToString("yyyy-MM-dd"));
                //Fecha Contrarecibo (Fija a la fecha actual)
                addenda.Element(ns + "DeliveryNote").Element(ns + "ReferenceDate").SetValue(DateTime.Today.ToString("yyyy-MM-dd"));

                //Montos de linea de detalle
                addenda.Element(ns + "lineItem").Element(ns + "grossPrice").Element(ns + "Amount").SetValue(cfdi.total_nacional.ToString("#########0.00"));
                addenda.Element(ns + "lineItem").Element(ns + "netPrice").Element(ns + "Amount").SetValue(cfdi.total_nacional.ToString("#########0.00"));
                addenda.Element(ns + "lineItem").Element(ns + "totalLineAmount").Element(ns + "grossAmount").Element(ns + "Amount").SetValue(cfdi.total_nacional.ToString("#########0.00"));
                addenda.Element(ns + "lineItem").Element(ns + "totalLineAmount").Element(ns + "netAmount").Element(ns + "Amount").SetValue(cfdi.total_nacional.ToString("#########0.00"));
                //Monto total de la Factura
                addenda.Element(ns + "totalAmount").Element(ns + "Amount").SetValue(cfdi.total_nacional.ToString("#########0.00"));

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el elemento addenda modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Proteinas y Oleicos
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaProteinasYOleicos(ref object xAddenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Número de folio asignado
                addenda.Element(ns + "requestForPaymentIdentification").Element(ns + "uniqueCreatorIdentification").SetValue(cfdi.serie + cfdi.folio.ToString());

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el lelemento modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para L'Oreal y sus empresas
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaLoreal(ref object xAddenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetNamespaceOfPrefix("if");

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //SubTotal
                addenda.Element(ns + "Encabezado").Attribute("SubTotal").SetValue(cfdi.subtotal_nacional.ToString("#############.00"));

                //Instanciando detalle de IVA
                using (Impuesto i = Impuesto.RecuperaImpuestoComprobante(cfdi.id_comprobante33))
                {
                    //IVA
                    addenda.Element(ns + "Encabezado").Attribute("Iva").SetValue(i.total_trasladado_moneda_nacional.ToString("#############.00"));
                    //Retención
                    addenda.Element(ns + "Encabezado").Attribute("IvaRet").SetValue(i.total_retenido_moneda_nacional.ToString("#############.00"));
                    //Total antes de retención
                    addenda.Element(ns + "Encabezado").Attribute("TotalFactura").SetValue((cfdi.subtotal_nacional + i.total_trasladado_moneda_nacional).ToString("#############.00"));
                }

                //Total de Factura
                addenda.Element(ns + "Encabezado").Attribute("Total").SetValue(cfdi.subtotal_nacional.ToString("#############.00"));

                //Fecha de Generación
                addenda.Element(ns + "Encabezado").Attribute("Fecha").SetValue(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("yyyy-MM-ddTHH:mm:ss"));

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el lelemento modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Mabe
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaMabe(ref object xAddenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            //Recuperando Namespace del elemento
            XNamespace ns = addenda.GetNamespaceOfPrefix("mabe");

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Serie y Folio
                addenda.Attribute("folio").SetValue(cfdi.serie + cfdi.folio.ToString());
                //Fecha de Factura
                addenda.Attribute("fecha").SetValue(cfdi.fecha_expedicion.ToString("yyyy-MM-dd"));

                //Monto con Letra
                addenda.Element(ns + "Moneda").Attribute("importeConLetra").SetValue(
                                Cadena.ConvierteMontoALetra(Math.Round(cfdi.total_nacional, 2).ToString()).ToUpper());
                //SubTotal
                addenda.Element(ns + "Subtotal").Attribute("importe").SetValue(Math.Round(cfdi.total_nacional, 2).ToString("########0.00"));
                //Importe Total
                addenda.Element(ns + "Total").Attribute("importe").SetValue(Math.Round(cfdi.total_nacional, 2).ToString("########0.00"));

                //Actualizando cambios sobre objeto de resultado
                xAddenda = addenda;
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo el elemento modificado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Aluprint
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaAluprint(ref object xAddenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                //Añadiendo los elementos especiales solicitados por el recepror
                //Fecha de Facturación
                addenda.Element(ns + "requestForPayment").Attribute(ns + "deliveryDate").SetValue(DateTime.Today.ToString("yyyyMMdd"));
                //Factura / Nota Crédito
                addenda.Element(ns + "requestForPayment").Element(ns + "entityType").SetValue(cfdi.id_tipo_comprobante == 3 ? "" :
                                                                        cfdi.id_tipo_comprobante == 1 ? "Factura" : "Nota_Credito");

                //Retirando elemento raíz para ajustar a definición real de addenda usada por el cliente
                xAddenda = addenda.Document.Root.Elements().ToArray();
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método que crea el elemento addenda para Kraft Foods
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaKraftFoods(ref object xAddenda, FEv33.Comprobante cfdi)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            XNamespace ns = addenda.GetDefaultNamespace();

            try
            {
                /*******************************************************************/
                //Llenando elementos de forma automática en base a necesidades
                /*******************************************************************/
                //RFC Emisor
                using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(cfdi.id_compania_emisor))
                {
                    if (em.id_compania_emisor_receptor > 0)
                        addenda.Element(ns + "EGC").Attribute("rfcEmisor").SetValue(em.rfc);
                    else
                        resultado = new RetornoOperacion("Error de llenado automático de addenda: Emisor no encontrado.");
                }
                //RFC Receptor
                using (CompaniaEmisorReceptor rc = new CompaniaEmisorReceptor(cfdi.id_compania_receptor))
                {
                    if (rc.id_compania_emisor_receptor > 0)
                        addenda.Element(ns + "EGC").Attribute("rfcReceptor").SetValue(rc.rfc);
                    else
                        resultado = new RetornoOperacion("Error de llenado automático de addenda: Receptor no encontrado.");
                }
                //Serie CFDI(interno)
                addenda.Element(ns + "EGC").Attribute("serieFactura").SetValue(cfdi.serie);
                //Folio CFDI(interno)
                addenda.Element(ns + "EGC").Attribute("folioFactura").SetValue(cfdi.folio);

                //Obteniendo total de partidas del elemento DI (secuencia manual)
                int totalPartidas = (from XElement di in addenda.Elements(ns + "DI")
                                     select Convert.ToInt32(di.Attribute("numeroItem"))).Max();

                //Validando que las partidas sean mayor o igual a uno
                if (totalPartidas > 0)
                {
                    //Total de partidas
                    addenda.Element(ns + "TA").Attribute("cantidadPartidas").SetValue(totalPartidas);
                    //Subtotal
                    addenda.Element(ns + "TA").Attribute("totalBruto").SetValue(cfdi.subtotal_nacional);
                    //Impuestos
                    addenda.Element(ns + "TA").Attribute("totalImpuestos").SetValue(cfdi.impuestos_nacional);
                    //Total
                    addenda.Element(ns + "TA").Attribute("importeTotal").SetValue(cfdi.total_nacional);

                    //Se aplica formato solicitado por ciente (texto plano, con separadores # y |)
                    resultado = transformaAddendaKraft(ref xAddenda, addenda);
                }
                else
                    resultado = new RetornoOperacion("Error de llenado automático de addenda: Partidas no detectadas.");
            }
            catch (Exception ex)
            {
                //Registrando error
                resultado = new RetornoOperacion(ex.Message);
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xAddenda"></param>
        /// <param name="cfdi"></param>
        /// <returns></returns>
        public static RetornoOperacion CreaAddendaModelo(ref object xAddenda, FEv33.Comprobante cfdi)
        {   //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(cfdi.id_comprobante33);

            //Convirtiendo a XElement para facilitar uso de objeto
            XElement addenda = (XElement)xAddenda;

            XNamespace ns = addenda.GetDefaultNamespace();

            using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(cfdi.id_comprobante33))
            {
                try
                {   //Añadiendo los elementos especiales solicitados por el recepror
                    //Número de folio asignado
                    addenda.Element(ns + "modelo:requestForPayment").Element(ns + "modelo:requestForPaymentIdentification")
                        .Element(ns + "modelo:uniqueCreatorIdentification").SetValue(cfdi.serie + cfdi.folio.ToString() + "/" +
                        Cadena.TruncaCadena(tfd.UUID, 17, ""));

                    //Actualizando cambios sobre objeto de resultado
                    xAddenda = addenda;
                }
                catch (Exception ex)
                {
                    //Registrando error
                    resultado = new RetornoOperacion(ex.Message);
                }
            }
            //Devolviendo Resultado Obtenido
            return resultado;
        }

        #endregion

        #endregion
    }
}
