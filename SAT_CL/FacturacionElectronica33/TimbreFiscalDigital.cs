using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Timbres Fiscales Digitales
    /// </summary>
    public class TimbreFiscalDigital : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_timbre_fiscal_digital_ttfd";

        private int _id_timbre_fiscal_digital;
        /// <summary>
        /// Atributo encargado de Almacenar el Identificador del Timbre Fiscal Digital
        /// </summary>
        public int id_timbre_fiscal_digital { get { return this._id_timbre_fiscal_digital; } }
        private int _id_comprobante;
        /// <summary>
        /// Atributo encargado de Almacenar el Comprobante del Timbre Fiscal Digital
        /// </summary>
        public int id_comprobante { get { return this._id_comprobante; } }
        private int _id_compania_proveedor;
        /// <summary>
        /// Atributo encargado de Almacenar el Proveedor Autorizado de Certificación (PAC) del Timbre Fiscal Digital
        /// </summary>
        public int id_compania_proveedor { get { return this._id_compania_proveedor; } }
        private string _rfc_pac;
        /// <summary>
        /// Atributo encargado de Almacenar el RFC del PAC
        /// </summary>
        public string rfc_pac { get { return this._rfc_pac; } }
        private string _leyenda;
        /// <summary>
        /// Atributo encargado de Almacenar la Leyenda del Timbre Fiscal Digital
        /// </summary>
        public string leyenda { get { return this._leyenda; } }
        private string _version;
        /// <summary>
        /// Atributo encargado de Almacenar la Versión del Timbre Fiscal Digital
        /// </summary>
        public string version { get { return this._version; } }
        private string _UUID;
        /// <summary>
        /// Atributo encargado de Almacenar el Identificador único de cada CFDI del Timbre Fiscal Digital
        /// </summary>
        public string UUID { get { return this._UUID; } }
        private DateTime _fecha_timbrado;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Timbrado del Comprobante
        /// </summary>
        public DateTime fecha_timbrado { get { return this._fecha_timbrado; } }
        private string _sello_CFD;
        /// <summary>
        /// Atributo encargado de Almacenar el Sello del CFDI del Timbre Fiscal Digital
        /// </summary>
        public string sello_CFD { get { return this._sello_CFD; } }
        private string _no_certificado;
        /// <summary>
        /// Atributo encargado de Almacenar Número del Certificado
        /// </summary>
        public string no_certificado { get { return this._no_certificado; } }
        private string _sello_SAT;
        /// <summary>
        /// Atributo encargado de Almacenar Sello 
        /// </summary>
        public string sello_SAT { get { return this._sello_SAT; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Construtores

        /// <summary>
        /// Constructor que inicializa los Atributos por Defecto
        /// </summary>
        public TimbreFiscalDigital()
        {
            //Asignando Valores
            this._id_timbre_fiscal_digital =
            this._id_comprobante =
            this._id_compania_proveedor = 0;
            this._rfc_pac =
            this._leyenda =
            this._version =
            this._UUID = "";
            this._fecha_timbrado = DateTime.MinValue;
            this._sello_CFD =
            this._no_certificado =
            this._sello_SAT = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_timbre_fiscal_digital">Timbre Fiscal Digital</param>
        public TimbreFiscalDigital(int id_timbre_fiscal_digital)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_timbre_fiscal_digital);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TimbreFiscalDigital()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_timbre_fiscal_digital"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_timbre_fiscal_digital)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_timbre_fiscal_digital, 0, 0, "", "", "", "", null, "", "", "", 0, false, "", "" };

            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Existencia de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_timbre_fiscal_digital = id_timbre_fiscal_digital;
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
                        this._rfc_pac = dr["RfcPac"].ToString();
                        this._leyenda = dr["Leyenda"].ToString();
                        this._version = dr["Version"].ToString();
                        this._UUID = dr["UUID"].ToString();
                        DateTime.TryParse(dr["FechaTimbrado"].ToString(), out this._fecha_timbrado);
                        this._sello_CFD = dr["SelloCFD"].ToString();
                        this._no_certificado = dr["NoCertificado"].ToString();
                        this._sello_SAT = dr["SelloSAT"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_comprobante">Comprobante del Timbre Fiscal Digital (TFD)</param>
        /// <param name="id_compania_proveedor">Proveedor Autorizado Certificador</param>
        /// <param name="rfc_pac">Regimen Fiscal de Contribuyentes del PAC</param>
        /// <param name="leyenda">Leyenda Opcional del TFD</param>
        /// <param name="version">Versión del TFD</param>
        /// <param name="UUID">Identificador del Folio Fiscal del Comprobante</param>
        /// <param name="fecha_timbrado">Fecha de Creación del TFD/param>
        /// <param name="sello_CFD">Sello del CFDI</param>
        /// <param name="no_certificado">Número del Certificado</param>
        /// <param name="sello_SAT">Sello Digital del SAT</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_comprobante, int id_compania_proveedor, string rfc_pac, string leyenda, string version,
                                                      string UUID, DateTime fecha_timbrado, string sello_CFD, string no_certificado, string sello_SAT,
                                                      int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_timbre_fiscal_digital, id_comprobante, id_compania_proveedor, rfc_pac, leyenda, version, UUID, 
                               fecha_timbrado, sello_CFD, no_certificado, sello_SAT, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Timbres Fiscales Digitales (TFD)
        /// </summary>
        /// <param name="id_comprobante">Comprobante del Timbre Fiscal Digital (TFD)</param>
        /// <param name="id_compania_proveedor">Proveedor Autorizado Certificador</param>
        /// <param name="rfc_pac">Regimen Fiscal de Contribuyentes del PAC</param>
        /// <param name="leyenda">Leyenda Opcional del TFD</param>
        /// <param name="version">Versión del TFD</param>
        /// <param name="UUID">Identificador del Folio Fiscal del Comprobante</param>
        /// <param name="fecha_timbrado">Fecha de Creación del TFD/param>
        /// <param name="sello_CFD">Sello del CFDI</param>
        /// <param name="no_certificado">Número del Certificado</param>
        /// <param name="sello_SAT">Sello Digital del SAT</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTimbreFiscalDigital(int id_comprobante, int id_compania_proveedor, string rfc_pac, string leyenda,
                                                           string version, string UUID, DateTime fecha_timbrado, string sello_CFD,
                                                           string no_certificado, string sello_SAT, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_comprobante, id_compania_proveedor, rfc_pac, leyenda, version, UUID, 
                               fecha_timbrado, sello_CFD, no_certificado, sello_SAT, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Timbres Fiscales Digitales (TFD)
        /// </summary>
        /// <param name="id_comprobante">Comprobante del Timbre Fiscal Digital (TFD)</param>
        /// <param name="id_compania_proveedor">Proveedor Autorizado Certificador</param>
        /// <param name="rfc_pac">Regimen Fiscal de Contribuyentes del PAC</param>
        /// <param name="leyenda">Leyenda Opcional del TFD</param>
        /// <param name="version">Versión del TFD</param>
        /// <param name="UUID">Identificador del Folio Fiscal del Comprobante</param>
        /// <param name="fecha_timbrado">Fecha de Creación del TFD/param>
        /// <param name="sello_CFD">Sello del CFDI</param>
        /// <param name="no_certificado">Número del Certificado</param>
        /// <param name="sello_SAT">Sello Digital del SAT</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaTimbreFiscalDigital(int id_comprobante, int id_compania_proveedor, string rfc_pac, string leyenda,
                                                           string version, string UUID, DateTime fecha_timbrado, string sello_CFD,
                                                           string no_certificado, string sello_SAT, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_comprobante, id_compania_proveedor, rfc_pac, leyenda, version, UUID,
                               fecha_timbrado, sello_CFD, no_certificado, sello_SAT, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Timbres Fiscales Digitales (TFD)
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTimbreFiscalDigital(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_comprobante, this._id_compania_proveedor, this._rfc_pac, this._leyenda, this._version,
                                             this._UUID, this._fecha_timbrado, this._sello_CFD, this._no_certificado, this._sello_SAT,
                                             id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos de los Timbres Fiscales Digitales (TFD)
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTimbreFiscalDigital()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_timbre_fiscal_digital);
        }
        /// <summary>
        /// Realiza la carga de los registros ligados a un Id de comprobante
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static DataTable CargaTimbresFiscalesComprobante(int id_comprobante)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_comprobante, 0, "", "", "", "", null, "", "", "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Busca e instancía el timbre fiscal digital del comprobante solicitado
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static TimbreFiscalDigital RecuperaTimbreFiscalComprobante(int id_comprobante)
        {
            //Definiendo objeto de retorno
            TimbreFiscalDigital timbre = new TimbreFiscalDigital();
            //Realizando la carga de los timbres del comprobante
            using (DataTable mit = CargaTimbresFiscalesComprobante(id_comprobante))
            {

                //Si el origen es válido
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Para cada uno de los registros
                    foreach (DataRow r in mit.Rows)

                        //Instanciando Timbre Fiscal Digital
                        timbre = new TimbreFiscalDigital(Convert.ToInt32(r["IdTimbreFiscalDigital"]));
                }
            }

            //Devolviendo resultado
            return timbre;
        }

        #endregion
    }
}
