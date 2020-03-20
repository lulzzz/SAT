using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de las operaciones relacionadas con las Series y Folios de los CFDI's
    /// </summary>
    public class SerieFolioCFDI : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el 
        /// </summary>
        public enum TipoSerieFolioCFDI
        {
            /// <summary>
            /// Facturacion Electronica
            /// </summary>
            FacturacionElectronica = 1,
            /// <summary>
            /// Recibo dde Nomina
            /// </summary>
            ReciboNomina = 2, 
            /// <summary>
            /// Recibo de pagos
            /// </summary>
            ReciboPagos = 3
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Alamcenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_serie_folio_cfdi_tsfc";

        private int _id_folio_serie;
        /// <summary>
        /// Atributo que almacena el Identificador del Serie y/ó Folio por Compania
        /// </summary>
        public int id_folio_serie { get { return this._id_folio_serie; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo que almacena la Compania Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private string _version_cfdi;
        /// <summary>
        /// Atributo que almacena la Versión del CFDI
        /// </summary>
        public string version_cfdi { get { return this._version_cfdi; } }
        private string _serie;
        /// <summary>
        /// Atributo que almacena la Serie a Utilizar
        /// </summary>
        public string serie { get { return this._serie; } }
        private bool _activa;
        /// <summary>
        /// Atributo que almacena el Indicador de Activación
        /// </summary>
        public bool activa { get { return this._activa; } }
        private int _folio_inicial;
        /// <summary>
        /// Atributo que almacena el Folio Inicial de la Compania
        /// </summary>
        public int folio_inicial { get { return this._folio_inicial; } }
        private int _folio_final;
        /// <summary>
        /// Atributo que almacena el Folio Final de la Compania
        /// </summary>
        public int folio_final { get { return this._folio_final; } }
        private byte _id_tipo_folio_serie;
        /// <summary>
        /// Atributo que almacena el Tipo de Serie y/ó Folio
        /// </summary>
        public byte id_tipo_folio_serie { get { return this._id_tipo_folio_serie; } }
        /// <summary>
        /// Atributo que almacena el Tipo de Serie y/ó Folio (Enumeración)
        /// </summary>
        public TipoSerieFolioCFDI tipo_folio_serie { get { return (TipoSerieFolioCFDI)this._id_tipo_folio_serie; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que inicializa los Atributos por Defecto
        /// </summary>
        public SerieFolioCFDI()
        {
            //Asignando Valores
            this._id_folio_serie = 
            this._id_compania_emisor = 0;
            this._version_cfdi =
            this._serie = "";
            this._activa = false;
            this._folio_inicial =
            this._folio_final = 0;
            this._id_tipo_folio_serie = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_folio_serie">Folio/Serie del CFDI</param>
        public SerieFolioCFDI(int id_folio_serie)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_folio_serie);
        }
        /// <summary>
        /// Constructor que inicializa loa Atributos dada una Serie, Versión y Emisor
        /// </summary>
        /// <param name="serie">Serie del Comprobante</param>
        /// <param name="version">Versión del CFDI</param>
        /// <param name="id_compania_emisora">Emisor del Comprobante</param>
        public SerieFolioCFDI(string serie, string version, int id_compania_emisora)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(serie, version, id_compania_emisora);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~SerieFolioCFDI()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_folio_serie">Folio/Serie del CFDI</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_folio_serie)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_folio_serie, 0, "", "", false, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultados del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_folio_serie = id_folio_serie;
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._version_cfdi = dr["VersionCFDI"].ToString();
                        this._serie = dr["Serie"].ToString();
                        this._activa = Convert.ToBoolean(dr["Activa"]);
                        this._folio_inicial = Convert.ToInt32(dr["FolioInicial"]);
                        this._folio_final = Convert.ToInt32(dr["FolioFinal"]);
                        this._id_tipo_folio_serie = Convert.ToByte(dr["TipoFolioSerie"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Cargar los Atributos dado una Serie, una Versión y un Emisor
        /// </summary>
        /// <param name="serie">Serie del Comprobante</param>
        /// <param name="version">Versión del CFDI</param>
        /// <param name="id_compania_emisora">Emisor del Comprobante</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(string serie, string version, int id_compania_emisora)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_compania_emisora, version, serie, false, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultados del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_folio_serie = Convert.ToInt32(dr["Id"]);
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._version_cfdi = dr["VersionCFDI"].ToString();
                        this._serie = dr["Serie"].ToString();
                        this._activa = Convert.ToBoolean(dr["Activa"]);
                        this._folio_inicial = Convert.ToInt32(dr["FolioInicial"]);
                        this._folio_final = Convert.ToInt32(dr["FolioFinal"]);
                        this._id_tipo_folio_serie = Convert.ToByte(dr["TipoFolioSerie"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="version_cfdi">Versión del CFDI</param>
        /// <param name="serie">Serie Predeterminada</param>
        /// <param name="activa">Indicador de Serie/Folio Activas</param>
        /// <param name="folio_inicial">Folio Inicial</param>
        /// <param name="folio_final">Folio Final</param>
        /// <param name="tipo_folio_serie">Tipo de Serie/Folio</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_compania_emisor, string version_cfdi, string serie, bool activa, int folio_inicial, int folio_final, int tipo_folio_serie, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_folio_serie, id_compania_emisor, version_cfdi, serie, activa, folio_inicial, folio_final, 
                               tipo_folio_serie, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);


            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Series y Folios de los Emisores
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="version">Versión del CFDI</param>
        /// <param name="serie">Serie Predeterminada</param>
        /// <param name="activa">Indicador de Serie/Folio Activas</param>
        /// <param name="folio_inicial">Folio Inicial</param>
        /// <param name="folio_final">Folio Final</param>
        /// <param name="tipo_folio_serie">Tipo de Serie/Folio</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSerieFolioCFDI(int id_compania_emisor, string version, string serie, bool activa, int folio_inicial, int folio_final, byte tipo_folio_serie, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania_emisor, version.ToUpper(), serie.ToUpper(), activa, folio_inicial, folio_final, 
                               tipo_folio_serie, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);


            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Series y Folios de los Emisores
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="version">Versión del CFDI</param>
        /// <param name="serie">Serie Predeterminada</param>
        /// <param name="activa">Indicador de Serie/Folio Activas</param>
        /// <param name="folio_inicial">Folio Inicial</param>
        /// <param name="folio_final">Folio Final</param>
        /// <param name="tipo_folio_serie">Tipo de Serie/Folio</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaSerieFolioCFDI(int id_compania_emisor, string version, string serie, bool activa, int folio_inicial, int folio_final, byte tipo_folio_serie, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_compania_emisor, version.ToUpper(), serie.ToUpper(), activa, folio_inicial, folio_final,
                                             tipo_folio_serie, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar las Series y Folios de los Emisores
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaSerieFolioCFDI(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_compania_emisor, this._version_cfdi, this._serie, this._activa, this._folio_inicial, 
                                             this._folio_final, this._id_tipo_folio_serie, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar las Series y Folios de los Emisores
        /// </summary>
        /// <returns></returns>
        public bool ActualizaSerieFolioCFDI()
        {
            //Devolviendo Resultado Obtenido
            return this.cargaAtributosInstancia(this._id_folio_serie);
        }

        #endregion
    }
}
