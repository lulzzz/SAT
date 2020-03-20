using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Base.CertificadoDigital;
using TSDK.Datos;
using FEv32 = SAT_CL.FacturacionElectronica;
using System.Linq;
using SAT_CL.Facturacion;
using SAT_CL.Bancos;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Comprobante : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Estatus de la Vigencia del Comprobante
        /// </summary>
        public enum EstatusVigencia
        {
            /// <summary>
            /// Expresa que el Comprobante esta Vigente
            /// </summary>
            Vigente = 1,
            /// <summary>
            /// Expresa que el Comprobante esta Cancelado
            /// </summary>
            Cancelado = 2,
            /// <summary>
            /// Expresa que el Comprobante esta Pendiente de Cancelación
            /// </summary>
            PendienteCancelacion = 3
        }
        /// <summary>
        /// OrigenDatos
        /// </summary>
        public enum OrigenDatos
        {
            /// <summary>
            /// Facturado
            /// </summary>
            Facturado = 1,
            /// <summary>
            /// Factura Global
            /// </summary>
            FacturaGlobal = 2,
            /// <summary>
            /// Liquidacion
            /// </summary>
            ReciboNomina,
            /// <summary>
            /// FI o Comprobante de Pago de Cliente
            /// </summary>
            ReciboPagoCliente
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del Store Procedure
        /// </summary>
        private static string _nom_sp = "fe33.sp_comprobante_tcp";
        private int _id_comprobante33;
        /// <summary>
        /// Atributo que almacena el Identificador del Comprobante v3.3
        /// </summary>
        public int id_comprobante33 { get { return this._id_comprobante33; } }
        private byte _id_estatus_vigencia;
        /// <summary>
        /// Atributo que almacena el Estatus
        /// </summary>
        public byte id_estatus_vigencia { get { return this._id_estatus_vigencia; } }
        /// <summary>
        /// 
        /// </summary>
        public EstatusVigencia estatusVigencia { get { return (EstatusVigencia)this._id_estatus_vigencia; } }
        private int _id_tipo_comprobante;
        /// <summary>
        /// Atributo que almacena el Tipo de Comprobante
        /// </summary>
        public int id_tipo_comprobante { get { return this._id_tipo_comprobante; } }
        private int _id_origen_datos;
        /// <summary>
        /// Atributo que almacena el Origen de Datos
        /// </summary>
        public int id_origen_datos { get { return this._id_origen_datos; } }
        private int _id_certificado;
        /// <summary>
        /// Atributo que almacena el Certificado
        /// </summary>
        public int id_certificado { get { return this._id_certificado; } }
        private string _version;
        /// <summary>
        /// Atributo que almacena la Serie del Comprobante v3.3
        /// </summary>
        public string version { get { return this._version; } }
        private string _serie;
        /// <summary>
        /// Atributo que almacena la Serie del Comprobante v3.3
        /// </summary>
        public string serie { get { return this._serie; } }
        private string _folio;
        /// <summary>
        /// Atributo que almacena el Folio del Comprobante v3.3
        /// </summary>
        public string folio { get { return this._folio; } }
        private string _sello;
        /// <summary>
        /// Atributo que almacena el Sello del Comprobante v3.3
        /// </summary>
        public string sello { get { return this._sello; } }
        private int _id_forma_pago;
        /// <summary>
        /// Atributo que almacena la Forma de Pago
        /// </summary>
        public int id_forma_pago { get { return this._id_forma_pago; } }
        private int _id_metodo_pago;
        /// <summary>
        /// Atributo que almacena el Método de Pago
        /// </summary>
        public int id_metodo_pago { get { return this._id_metodo_pago; } }
        private string _condicion_pago;
        /// <summary>
        /// Atributo que almacena la Condición de Pago
        /// </summary>
        public string condicion_pago { get { return this._condicion_pago; } }
        private int _id_moneda;
        /// <summary>
        /// Atributo que almacena la Moneda
        /// </summary>
        public int id_moneda { get { return this._id_moneda; } }
        private decimal _subtotal_captura;
        /// <summary>
        /// Atributo que almacena el SubTotal de Captura
        /// </summary>
        public decimal subtotal_captura { get { return this._subtotal_captura; } }
        private decimal _impuestos_captura;
        /// <summary>
        /// Atributo que almacena los Impuestos de Captura
        /// </summary>
        public decimal impuestos_captura { get { return this._impuestos_captura; } }
        private decimal _descuentos_captura;
        /// <summary>
        /// Atributo que almacena los Descuentos de Captura
        /// </summary>
        public decimal descuentos_captura { get { return this._descuentos_captura; } }
        private decimal _total_captura;
        /// <summary>
        /// Atributo que almacena el Total de Captura
        /// </summary>
        public decimal total_captura { get { return this._total_captura; } }
        private decimal _subtotal_nacional;
        /// <summary>
        /// Atributo que almacena el SubTotal (Nacional MXN)
        /// </summary>
        public decimal subtotal_nacional { get { return this._subtotal_nacional; } }
        private decimal _impuestos_nacional;
        /// <summary>
        /// Atributo que almacena los Impuestos (Nacional MXN)
        /// </summary>
        public decimal impuestos_nacional { get { return this._impuestos_nacional; } }
        private decimal _descuentos_nacional;
        /// <summary>
        /// Atributo que almacena los Descuentos (Nacional MXN)
        /// </summary>
        public decimal descuentos_nacional { get { return this._descuentos_nacional; } }
        private decimal _total_nacional;
        /// <summary>
        /// Atributo que almacena el Total (Nacional MXN)
        /// </summary>
        public decimal total_nacional { get { return this._total_nacional; } }
        private decimal _tipo_cambio;
        /// <summary>
        /// Atributo que almacena el Tipo de Cambio
        /// </summary>
        public decimal tipo_cambio { get { return this._tipo_cambio; } }
        private string _lugar_expedicion;
        /// <summary>
        /// Atributo que almacena el Lugar de Expedición (Cógido Postal)
        /// </summary>
        public string lugar_expedicion { get { return this._lugar_expedicion; } }
        private int _id_direccion_lugar_expedicion;
        /// <summary>
        /// Atributo que almacena la Dirección del Lugar de Expedición
        /// </summary>
        public int id_direccion_lugar_expedicion { get { return this._id_direccion_lugar_expedicion; } }
        private string _confirmacion;
        /// <summary>
        /// Atributo que almacena la Confirmación
        /// </summary>
        public string confirmacion { get { return this._confirmacion; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo que almacena la Compania Emisora del Comprobante
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private string _regimen_fiscal;
        /// <summary>
        /// Atributo que almacena el Regimen Fiscal del Emisor
        /// </summary>
        public string regimen_fiscal { get { return this._regimen_fiscal; } }
        private int _id_sucursal;
        /// <summary>
        /// Atributo que almacena la Sucursal de Compania Emisora del Comprobante
        /// </summary>
        public int id_sucursal { get { return this._id_sucursal; } }
        private int _id_compania_receptor;
        /// <summary>
        /// Atributo que almacena la Compania Receptora del Comprobante
        /// </summary>
        public int id_compania_receptor { get { return this._id_compania_receptor; } }
        private int _id_uso_receptor;
        /// <summary>
        /// Atributo que almacena el Uso del CFDI del Receptor
        /// </summary>
        public int id_uso_receptor { get { return this._id_uso_receptor; } }
        private DateTime _fecha_captura;
        /// <summary>
        /// Atributo que almacena la Fecha de Captura del Comprobante
        /// </summary>
        public DateTime fecha_captura { get { return this._fecha_captura; } }
        private DateTime _fecha_expedicion;
        /// <summary>
        /// Atributo que almacena la Fecha de Expedición del Comprobante
        /// </summary>
        public DateTime fecha_expedicion { get { return this._fecha_expedicion; } }
        private DateTime _fecha_cancelacion;
        /// <summary>
        /// Atributo que almacena la Fecha de Cancelación del Comprobante
        /// </summary>
        public DateTime fecha_cancelacion { get { return this._fecha_cancelacion; } }
        private bool _bit_generado;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool bit_generado { get { return this._bit_generado; } }
        private bool _bit_transferido_nuevo;
        /// <summary>
        /// Atributo que almacena el Indicador de Transferencia Nueva
        /// </summary>
        public bool bit_transferido_nuevo { get { return this._bit_transferido_nuevo; } }
        private int _id_transferencia_nuevo;
        /// <summary>
        /// Atributo que almacena el Identificador del la Nueva Transferencia
        /// </summary>
        public int id_transferencia_nuevo { get { return this._id_transferencia_nuevo; } }
        private bool _bit_transferido_cancelar;
        /// <summary>
        /// Atributo que almacena el Indicador de Cancelación de la Transferencia
        /// </summary>
        public bool bit_transferido_cancelar { get { return this._bit_transferido_cancelar; } }
        private int _id_transferencia_cancelar;
        /// <summary>
        /// Atributo que almacena el Identificador de la Transferencia Cancelada
        /// </summary>
        public int id_transferencia_cancelar { get { return this._id_transferencia_cancelar; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private string _ruta_xml;
        /// <summary>
        /// Atributo que almacena la Ruta del Comprobante en Formato XML
        /// </summary>
        public string ruta_xml { get { return this._ruta_xml; } }
        /// <summary>
        /// Atributo que almacena la Ruta del Codigo QR
        /// </summary>
        private string _ruta_codigo_bidimensional;
        public string ruta_codigo_bidimensional { get { return this._ruta_codigo_bidimensional; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Contructor que inicializa los Valores por Defecto
        /// </summary>
        public Comprobante()
        {
            //Asignando Valores
            this._id_comprobante33 = 0;
            this._id_estatus_vigencia = 0;
            this._id_tipo_comprobante = 
            this._id_origen_datos = 
            this._id_certificado = 0;
            this._version = 
            this._serie = 
            this._folio = 
            this._sello = "";
            this._id_forma_pago = 0;
            this._id_metodo_pago = 0;
            this._condicion_pago = "";
            this._id_moneda = 0;
            this._subtotal_captura = 
            this._impuestos_captura = 
            this._descuentos_captura = 
            this._total_captura = 
            this._subtotal_nacional = 
            this._impuestos_nacional = 
            this._descuentos_nacional = 
            this._total_nacional = 0.00M;
            this._tipo_cambio = 0.00M;
            this._lugar_expedicion = "";
            this._id_direccion_lugar_expedicion = 0;
            this._confirmacion = "";
            this._id_compania_emisor = 0;
            this._regimen_fiscal = "";
            this._id_sucursal =
            this._id_compania_receptor = 0;
            this._id_uso_receptor = 0;
            this._fecha_captura = 
            this._fecha_expedicion = 
            this._fecha_cancelacion = DateTime.MinValue;
            this._bit_generado = 
            this._bit_transferido_nuevo = false;
            this._id_transferencia_nuevo = 0;
            this._bit_transferido_cancelar = false;
            this._id_transferencia_cancelar = 0;
            this._habilitar = false;
            this._ruta_xml = "";
            this._ruta_codigo_bidimensional = "";
        }
        /// <summary>
        /// Constructor que inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_comprobante33">Comprobante v3.3</param>
        public Comprobante(int id_comprobante33)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_comprobante33);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Comprobante()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_comprobante33">Comprobante v3.3</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_comprobante33)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Declarando Arreglo de Parametros
            object[] param = { 3, id_comprobante33, 0, 0, 0, 0, "", "", "", "", 0, 0, "", 0, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 
                               0.00M, 0.00M, "", 0, "", 0, "", 0, 0, 0, null, null, null, false, false, 0, false, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_comprobante33 = id_comprobante33;
                        this._id_estatus_vigencia = Convert.ToByte(dr["IdEstatusVigencia"]);
                        this._id_tipo_comprobante = Convert.ToInt32(dr["IdTipoComprobante"]);
                        this._id_origen_datos = Convert.ToInt32(dr["IdOrigenDatos"]);
                        this._id_certificado = Convert.ToInt32(dr["IdCertificado"]);
                        this._version = dr["Version"].ToString();
                        this._serie = dr["Serie"].ToString();
                        this._folio = dr["Folio"].ToString();
                        this._sello = dr["Sello"].ToString();
                        this._id_forma_pago = Convert.ToInt32(dr["IdFormaPago"]);
                        this._id_metodo_pago = Convert.ToInt32(dr["IdMetodoPago"]);
                        this._condicion_pago = dr["CondicionPago"].ToString();
                        this._id_moneda = Convert.ToInt32(dr["IdMoneda"]);
                        this._subtotal_captura = Convert.ToDecimal(dr["SubTotalCaptura"]);
                        this._impuestos_captura = Convert.ToDecimal(dr["ImpuestosCaptura"]);
                        this._descuentos_captura = Convert.ToDecimal(dr["DescuentosCaptura"]);
                        this._total_captura = Convert.ToDecimal(dr["TotalCaptura"]);
                        this._subtotal_nacional = Convert.ToDecimal(dr["SubTotalNacional"]);
                        this._impuestos_nacional = Convert.ToDecimal(dr["ImpuestosNacional"]);
                        this._descuentos_nacional = Convert.ToDecimal(dr["DescuentosNacional"]);
                        this._total_nacional = Convert.ToDecimal(dr["TotalNacional"]);
                        this._tipo_cambio = Convert.ToDecimal(dr["TipoCambio"]);
                        this._lugar_expedicion = dr["LugarExpedicion"].ToString();
                        this._id_direccion_lugar_expedicion = Convert.ToInt32(dr["IdDireccionLugarExpedicion"]);
                        this._confirmacion = dr["Confirmacion"].ToString();
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._regimen_fiscal = dr["RegimenFiscal"].ToString();
                        this._id_sucursal = Convert.ToInt32(dr["IdSucursal"]);
                        this._id_compania_receptor = Convert.ToInt32(dr["IdCompaniaReceptor"]);
                        this._id_uso_receptor = Convert.ToByte(dr["IdUsoCfdiReceptor"]);
                        DateTime.TryParse(dr["FechaCaptura"].ToString(), out this._fecha_captura);
                        DateTime.TryParse(dr["FechaExpedicion"].ToString(), out this._fecha_expedicion);
                        DateTime.TryParse(dr["FechaCancelacion"].ToString(), out this._fecha_cancelacion);
                        this._bit_generado = Convert.ToBoolean(dr["BitGenerado"]);
                        this._bit_transferido_nuevo = Convert.ToBoolean(dr["BitTransferidoNuevo"]);
                        this._id_transferencia_nuevo = Convert.ToInt32(dr["IdTransferenciaNuevo"]);
                        this._bit_transferido_cancelar = Convert.ToBoolean(dr["BitTransferidoCancelar"]);
                        this._id_transferencia_cancelar = Convert.ToInt32(dr["IdTransferenciaCancelar"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._ruta_xml = dr["RutaXML"].ToString();
                        this._ruta_codigo_bidimensional = dr["RutaCodigoBidimensional"].ToString();
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos en la BD
        /// </summary>
        /// <param name="estatus_vigencia">Estatus de la Vigencia</param>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante (I,E,T,P,N)</param>
        /// <param name="id_origen_datos">Origen de Datos</param>
        /// <param name="id_certificado">Certificado del Timbrado</param>
        /// <param name="version">Versión del Comprobante</param>
        /// <param name="serie">Serie</param>
        /// <param name="folio">Folio</param>
        /// <param name="sello">Sello Fiscal Digital</param>
        /// <param name="id_forma_pago">Forma de Pago (Para el Complemento de Pago)</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="condicion_pago">Condición de Pago</param>
        /// <param name="id_moneda">Moneda</param>
        /// <param name="subtotal_captura">SubTotal de Captura</param>
        /// <param name="impuestos_captura">Impuestos de Captura</param>
        /// <param name="descuentos_captura">Descuentos de Captura</param>
        /// <param name="total_captura">Total de Captura</param>
        /// <param name="subtotal_nacional">SubTotal (Nacional MXN)</param>
        /// <param name="impuestos_nacional">Impuestos (Nacional MXN)</param>
        /// <param name="descuentos_nacional">Descuentos (Nacional MXN)</param>
        /// <param name="total_nacional">Total (Nacional MXN)</param>
        /// <param name="tipo_cambio">Tipo de Cambio</param>
        /// <param name="lugar_expedicion">Lugar de Expedición (Código Postal)</param>
        /// <param name="id_direccion_lugar_expedicion">Dirección del Lugar de Exp.</param>
        /// <param name="confirmacion">No. de Confirmación del PAC</param>
        /// <param name="id_compania_emisor">Emisor del Comprobante</param>
        /// <param name="id_regimen_fiscal">Regimen Fiscal</param>
        /// <param name="id_sucursal">Sucursal</param>
        /// <param name="id_compania_receptor">Receptor del Comprobante</param>
        /// <param name="id_uso_cdfi_receptor">Uso del CFDI del Receptor</param>
        /// <param name="fecha_captura">Fecha de Captura del >Comprobante</param>
        /// <param name="fecha_expedicion">Fecha de Expedición del Comprobante</param>
        /// <param name="fecha_cancelacion">Fecha de Cancelación del Comprobante</param>
        /// <param name="bit_generado">Indicador de Generación del Comprobante</param>
        /// <param name="bit_transferido_nuevo">Indicador de Nueva Transferencia</param>
        /// <param name="id_transferencia_nuevo">Identificador de Nueva Transferencia</param>
        /// <param name="bit_transferido_cancelar">Indicador de Transferencia Cancelada</param>
        /// <param name="id_transferencia_cancelar">Identificador de Transferencia Cancelada</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaAtributosBD(EstatusVigencia estatus_vigencia, int id_tipo_comprobante, int id_origen_datos, 
                        int id_certificado, string version, string serie, string folio, string sello, int id_forma_pago, int id_metodo_pago, 
                        string condicion_pago, int id_moneda, decimal subtotal_captura, decimal impuestos_captura, decimal descuentos_captura, 
                        decimal total_captura, decimal subtotal_nacional, decimal impuestos_nacional, decimal descuentos_nacional, 
                        decimal total_nacional, decimal tipo_cambio, string lugar_expedicion, int id_direccion_lugar_expedicion, 
                        string confirmacion, int id_compania_emisor, string regimen_fiscal, int id_sucursal, int id_compania_receptor, 
                        int id_uso_cdfi_receptor, DateTime fecha_captura, DateTime fecha_expedicion, 
                        DateTime fecha_cancelacion, bool bit_generado, bool bit_transferido_nuevo, int id_transferencia_nuevo, 
                        bool bit_transferido_cancelar, int id_transferencia_cancelar, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Arreglo de Parametros
            object[] param = { 2, this._id_comprobante33, (byte)estatus_vigencia, id_tipo_comprobante, id_origen_datos, id_certificado, version, 
                               serie, folio, sello, id_forma_pago, id_metodo_pago, condicion_pago, id_moneda, subtotal_captura, impuestos_captura,
                               descuentos_captura, total_captura, subtotal_nacional, impuestos_nacional, descuentos_nacional, total_nacional, 
                               tipo_cambio, lugar_expedicion, id_direccion_lugar_expedicion, confirmacion, id_compania_emisor, regimen_fiscal,
                               id_sucursal, id_compania_receptor, id_uso_cdfi_receptor, Fecha.ConvierteDateTimeObjeto(fecha_captura), 
                               Fecha.ConvierteDateTimeObjeto(fecha_expedicion), Fecha.ConvierteDateTimeObjeto(fecha_cancelacion), bit_generado, bit_transferido_nuevo, 
                               id_transferencia_nuevo, bit_transferido_cancelar, id_transferencia_cancelar, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Realiza la modificacion del contenido en el archivo del CFD para omitir los acentos del documento
        /// </summary>
        /// <param name="xml_comprobante">Cadena xml con el contenido del comprobante</param>
        private string suprimeCaracteresAcentuadosCFD(string xml_comprobante)
        {
            //Definiendo objeto de retorno
            string xml_sin_acentos = "";

            //Realizando las modificaciones al contenido
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_comprobante, "[á|à]", "a");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[é|è]", "e");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[í|ì]", "i");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[ó|ò]", "o");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[ú|ù]", "u");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[Á|À]", "A");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[É|È]", "E");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[Í|Ì]", "I");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[Ó|Ò]", "O");
            xml_sin_acentos = Cadena.SustituyePatronCadena(xml_sin_acentos, "[Ú|Ù]", "U");

            return xml_sin_acentos;
        }
        /// <summary>
        /// Obtiene el número de folio por asignar a un comprobante a apartir de el emisor y serie solicitada
        /// </summary>
        /// <param name="id_compania_emisor">Id de Emisor</param>
        /// <param name="version">Versión del Comprobante (3.3)</param>
        /// <param name="serie">Serie del Folio</param>
        /// <returns></returns>
        private static string obtieneFolioPorAsignar(int id_compania_emisor, string version, string serie)
        {
            //Declarando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, 0, 0, version, serie, "", "", 0, 0, "", 0, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 
                               0.00M, 0.00M, "", 0, "", id_compania_emisor, "", 0, 0, 0, null, null, null, false, false, 0, false, 0, 0, false, "", "" };
            //Ejecuta SP
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param).IdRegistro.ToString();
        }
        /// <summary>
        /// Realiza la actualización de los datos Serie, Folio, Fecha de Expedición y Certificado utilizado para sellar comprobante
        /// </summary>
        /// <param name="version">Versión del Comprobante</param>
        /// <param name="serie">Serie del Comprobante</param>
        /// <param name="folio">Folio del Comprobante</param>
        /// <param name="id_certificado">Id de Certificado con que será sellado</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private RetornoOperacion asignaFolioComprobante(string version, string serie, string folio, int id_certificado, int id_usuario)
        {
            //Actialziando datos
            return this.actualizaAtributosBD((EstatusVigencia)this._id_estatus_vigencia, this._id_tipo_comprobante, this._id_origen_datos, id_certificado, this._version,
                               serie, folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                               this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                               this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                               this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, Fecha.ObtieneFechaEstandarMexicoCentro(), this._fecha_cancelacion, this._bit_generado, this._bit_transferido_nuevo,
                               this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Validar los Requisitos Previos al Timbrado
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <param name="id_certificado_activo">Identificador de Certificado Activo</param>
        /// <param name="numero_certificado">Número de Certificado</param>
        /// <param name="certificado_base_64">Certificado en Base64</param>
        /// <param name="contrasena_apertura">Contraseña de Apertura</param>
        /// <param name="bytes_certificado">Certificado en Bytes</param>
        /// <param name="tipoFolioSerie">Tipo de Serie y/ó Folio por Calcular (FE, Nómina)</param>
        /// <returns></returns>
        private RetornoOperacion validaRequisitosPreviosTimbrado(string serie, out string folio, out int id_certificado_activo,
                                        out string numero_certificado, out string certificado_base_64, out string contrasena_apertura,
                                        out byte[] bytes_certificado, SerieFolioCFDI.TipoSerieFolioCFDI tipoFolioSerie)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(1);

            //Inicializando valores de Retorno
            id_certificado_activo = 0;
            folio = numero_certificado = certificado_base_64 = contrasena_apertura = "";
            bytes_certificado = null;            

            //Inicializando Bloque Transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que el Comprobante no este Timbrado
                if (!this._bit_generado)
                {
                    //Si la Moneda es Distinta de MXN
                    if (this._id_moneda != 1)
                    {
                        //Validando el Tipo de Cambio
                        if (!(this._tipo_cambio > 0))

                            //Instanciando Excepción
                            result = new RetornoOperacion("El Comprobante debe de tener un Tipo de Cambio Valido");
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("El Comprobante se encuentra Timbrado");

                //Validando Validación Exitosa
                if (result.OperacionExitosa)
                {
                    //Validando que el Emisor este Vigente
                    using (Global.CompaniaEmisorReceptor emisor = new Global.CompaniaEmisorReceptor(this._id_compania_emisor))
                    {
                        //Validando Existencia
                        if (emisor.habilitar)
                        {
                            //Validando sucursal
                            if (this._id_sucursal > 0)
                            {
                                //Instanciando sucursal
                                using (Global.Sucursal s = new Global.Sucursal(this._id_sucursal))
                                {
                                    //Validando que no exista la Sucursal
                                    if (!s.habilitar)
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("La sucursal no se encuentra activa.");
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("El Emisor no se encuentra Activo.");

                        //Validando Validación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo el certificado activo para el emisor y/o sucursal
                            using (CertificadoDigital certificado = CertificadoDigital.RecuperaCertificadoEmisorSucursal(this._id_compania_emisor, this._id_sucursal, CertificadoDigital.TipoCertificado.CSD))
                            {
                                //Validando Existencia del Certificado
                                if (certificado.Habilitar)
                                {
                                    //Obteniendo CER
                                    using (Certificado cer = new Certificado(certificado.ruta_llave_publica))
                                    {
                                        //Comprobando Carga del Certificado
                                        if (cer.Subject != null)
                                        {
                                            //Validando RFC del Emisor contra el del Certificado
                                            if (cer.Subject.RFCPropietario == emisor.rfc)
                                            {
                                                //Asignando parámteros de salida
                                                id_certificado_activo = certificado.id_certificado_digital;
                                                numero_certificado = cer.No_Serie;
                                                certificado_base_64 = cer.CertificadoBase64;
                                                contrasena_apertura = certificado.contrasena_desencriptada;
                                                bytes_certificado = System.IO.File.ReadAllBytes(certificado.ruta_llave_privada);
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("Imposible recuperar los datos de Propietario del Certificado.");
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existe un Certificado de Sello Digital activo.");
                            }
                        }

                        //Si no hay erroes
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Conceptos
                            using (DataTable dtConceptos = Concepto.ObtieneConceptosComprobante(this._id_comprobante33))
                            {
                                //Validando que no existan Conceptos
                                if (!Validacion.ValidaOrigenDatos(dtConceptos))

                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existen Conceptos en el Comprobante");
                            }
                        }

                        //Si no existe error
                        if (result.OperacionExitosa)
                        {
                            //Instanciamos emisor y serie para obtener el tipo definido
                            using (SerieFolioCFDI objSerieFolio = new SerieFolioCFDI(serie, version, this._id_compania_emisor))
                            {
                                ///Validamos el Tipo de Serie
                                if (objSerieFolio.tipo_folio_serie == tipoFolioSerie)
                                {
                                    //Realizando búsqueda de folio por asignar
                                    folio = obtieneFolioPorAsignar(this._id_compania_emisor, version, serie);
                                    //Si no existe un folio disponible
                                    if (Convert.ToInt32(folio) <= 0)
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(string.Format("No existe un folio disponible para la serie {0}.", serie.ToUpper()));
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion(string.Format("La serie no se encuentra disponible {0}.", serie));
                            }
                        }
                    }
                }

                //Validamos Resultado
                if (result.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar el Sello Digital en el Comprobante
        /// </summary>
        /// <param name="sello_digital">Sello Digital del CFDI</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaSelloDigital(string sello_digital, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaAtributosBD((EstatusVigencia)this._id_estatus_vigencia, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                               this._serie, this._folio, sello_digital, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                               this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                               this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                               this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, true, true,
                               this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Añade las addendas definidas para el receptor y emisor del comprobante
        /// </summary>
        /// <param name="comprobante">Elemento xml a editar</param>
        /// <param name="ns_sat">Namespace del SAT para asociar elementos contenedores de complemento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private RetornoOperacion anadeElementoComplementoComprobante(ref XDocument comprobante, XNamespace ns_sat, int id_usuario)
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante33);

            //Cargando las addendas aplicables
            using (DataTable addendas = FEv32.AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor))
            {
                //Si existen addendas configuradas
                if (Validacion.ValidaOrigenDatos(addendas))
                {
                    //Si no existe el elemento Complemento
                    if (comprobante.Element(ns_sat + "Complemento") == null)
                        //Añadiendolo
                        comprobante.Add(new XElement(ns_sat + "Complemento"));

                    //Creando elementos que pueden contener addenda
                    //Se recupera, ya que este existe por ser obligatorio para timbre
                    XElement elemento_complemento = comprobante.Root.Element(ns_sat + "Complemento");

                    //Definiendo objeto de validación de captura de al menos un addenda
                    List<RetornoOperacion> resultado_addenda = new List<RetornoOperacion>();

                    //Para cada una de las addendas configuradas
                    foreach (DataRow a in addendas.Rows)
                    {
                        RetornoOperacion r = new RetornoOperacion(this._id_comprobante33);

                        //Recuperando la addenda aplicable al comprobante
                        using (FEv32.AddendaComprobante ac = FEv32.AddendaComprobante.RecuperaAddendaComprobanteV33(Convert.ToInt32(a["Id"]), this._id_comprobante33))
                        {
                            //Si la addenda se ha capturado y es válida
                            if (ac.id_addenda_comprobante > 0 && ac.habilitar && ac.bit_validacion)
                            {
                                //Declarando elemento por añadir
                                XElement elemento_ac = null;

                                try
                                {
                                    //Cargando XML correspondiente
                                    elemento_ac = XDocument.Parse(ac.xml_addenda.OuterXml).Root;
                                }
                                catch (Exception ex)
                                {
                                    r = new RetornoOperacion(ex.Message);
                                }

                                //Si el elemento no está vacío
                                if (r.OperacionExitosa)
                                {
                                    //Si existe un esquema adicional que actualizar
                                    if (elemento_ac.Attribute("schemaLocation") != null)
                                    {
                                        //Localizando atributo 'schemaLocation', para añadir el prefijo del espacio de nombres de la W3C
                                        string valor_schemaLocation = elemento_ac.Attribute("schemaLocation").Value;

                                        //Si se localizó
                                        if (valor_schemaLocation != "")
                                        {
                                            //Eliminando atributo, añadiendo el prefijo correspondiente
                                            elemento_ac.Attribute("schemaLocation").Remove();
                                            //Creando nuevo atributo con prefijo correspondiente y el valor del atributo anterior
                                            elemento_ac.Add(new XAttribute(comprobante.Root.GetNamespaceOfPrefix("xsi") + "schemaLocation", valor_schemaLocation));
                                        }
                                        else
                                            r = new RetornoOperacion("Error en addenda, el atributo 'schemaLocation' no tiene ningún valor.");
                                    }

                                    //Si no hay errores
                                    if (r.OperacionExitosa)
                                    {
                                        //Instanciando addenda para recuperar 
                                        using (FEv32.Addenda ad = new FEv32.Addenda(Convert.ToInt32(a["IdAddenda"])))
                                        {
                                            //Validando carga correcta de registro
                                            if (ad.id_addenda > 0)
                                            {
                                                //Definiendo objeto para almacenamiento del contenido de addendas 
                                                //(se añade debido a que el contenido de este nodo no está regulado bajo ningún estándar 
                                                //y puede ser que un addenda no contenga un elemento raíz)
                                                object obj_elemento_ac = elemento_ac;

                                                //Creando addenda personalizada (en caso de estar definida para el receptor)
                                                r = FEv32.AddendaEmisor.CreaAddendaReceptor(ref obj_elemento_ac, this);

                                                //Si no existe error
                                                if (r.OperacionExitosa)
                                                    elemento_complemento.Add(obj_elemento_ac);

                                                //Covertimos XElement a XmlDocumento para edición de la Addenda
                                                using (XmlReader xmlReader = System.Xml.Linq.XElement.Parse(obj_elemento_ac.ToString()).CreateReader())
                                                {
                                                    XmlDocument xmlDoc = new XmlDocument();
                                                    xmlDoc.Load(xmlReader);

                                                    //Guradamos Addenda en BD
                                                    resultado = ac.EditarAddendaComprobante(xmlDoc, id_usuario);
                                                }
                                            }
                                            else
                                                r = new RetornoOperacion("Error en addenda, no fue posible recuperar configuración de addenda.");
                                        }
                                    }
                                }
                                else
                                    r = new RetornoOperacion("Error en addenda, el elemento principal no fue encontrado.");
                            }
                            else
                                //Actualizando error
                                r = new RetornoOperacion("Error en addenda, addenda sin validación de esquema.");
                        }

                        //Añadiendo resultado
                        resultado_addenda.Add(r);
                    }

                    //Si existe al menos una addenda exitosa
                    RetornoOperacion x = (from RetornoOperacion r in resultado_addenda
                                          where r.OperacionExitosa == true
                                          select r).FirstOrDefault();

                    //SI NO HAY RESULTADOS SATISFACTORIOS (SÓLO ERRORES)
                    if (x == null)
                    {
                        resultado = new RetornoOperacion("No se ha capturado addenda o bien no ha sido validada.");
                    }
                    else
                    {
                        //Si existen errores y al menos una captura fue correcta
                        if (!x.OperacionExitosa)
                        {
                            //Mostrando erores
                            string mensaje_error = "";
                            foreach (RetornoOperacion r in resultado_addenda)
                                mensaje_error += r.Mensaje + "<br/>";
                            resultado = new RetornoOperacion(mensaje_error, false);
                        }
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Verifica que el comprobante tenga al menos una addenda por añadir antes de realizar el timbrado, para evitar errores de guardado posteriores
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaAddendasCapturadas()
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante33);

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando las addendas aplicables
                using (DataTable addendas = FEv32.AddendaEmisor.CargaAddendasRequeridasEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor))
                {
                    //Si existen addendas configuradas
                    if (Validacion.ValidaOrigenDatos(addendas))
                    {
                        //Definiendo objeto de validación de captura de al menos un addenda
                        List<RetornoOperacion> resultado_addenda = new List<RetornoOperacion>();

                        //Para cada una de las addendas configuradas
                        foreach (DataRow a in addendas.Rows)
                        {
                            RetornoOperacion r = new RetornoOperacion(this._id_comprobante33);

                            //Recuperando la addenda aplicable al comprobante
                            using (FEv32.AddendaComprobante ac = FEv32.AddendaComprobante.RecuperaAddendaComprobanteV33(Convert.ToInt32(a["Id"]), this._id_comprobante33))
                            {
                                //Si la addenda se ha capturado y es válida
                                if (ac.id_addenda_comprobante > 0 && ac.habilitar && ac.bit_validacion)
                                {
                                    //No aplica error
                                }
                                else
                                    //Actualizando error
                                    r = new RetornoOperacion("Error en addenda, addenda sin validación de esquema.");
                            }

                            //Añadiendo resultado
                            resultado_addenda.Add(r);
                        }

                        //SI existe al menos una addenda exitosa
                        RetornoOperacion x = (from RetornoOperacion r in resultado_addenda
                                              where r.OperacionExitosa == true
                                              select r).FirstOrDefault();
                        //Si no hay addendas capturadas
                        if (x != null)
                        {
                            //Si no existen errores y al menos una captura fue correcta
                            if (x.OperacionExitosa)
                            {
                                resultado = new RetornoOperacion(this._id_comprobante33);
                            }
                            else
                            {
                                //Mostrando erores
                                string mensaje_error = "";
                                foreach (RetornoOperacion r in resultado_addenda)
                                    mensaje_error += r.Mensaje + "<br/>";
                                resultado = new RetornoOperacion(mensaje_error, false);
                            }
                        }
                        else
                            resultado = new RetornoOperacion("Ninguna Addenda se ha capturado aún.");
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Generar el Timbre Fiscal Digital
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="ns_SAT"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        private RetornoOperacion generaTimbreFiscalDigital(ref XDocument documento, XNamespace ns_SAT, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Recuperando bytes del comprobante
            byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

            //Recuperando bytes del comprobante timbrado
            XDocument xml_comprobante_recuperado = null;
            int id_compania_pac = 0;
            
            //Traduciendo resultado
            result = FEv32.PacCompaniaEmisor.GeneraTimbrePAC3_3(documento, bytes_comprobante, out xml_comprobante_recuperado, out id_compania_pac, this._id_compania_emisor);

            //Si no existe error
            if (result.OperacionExitosa)
            {
                //Instanciando nuevo documento a partir del xml recuperado
                documento = xml_comprobante_recuperado;

                //Recuperando Namespace del Timbre Fiscal
                XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");

                //Recuperación de nodo Timbre Fiscal Digital
                XElement timbre = documento.Root.Element(ns_SAT + "Complemento").Element(ns + "TimbreFiscalDigital");

                //Si el nodo fue recuperado
                if (timbre != null)
                {   
                    //Instanciando Datos del PAC
                    using (CompaniaEmisorReceptor pac = new CompaniaEmisorReceptor(id_compania_pac))
                    {
                        //Validando Existencia del PAC
                        if (pac.habilitar)

                            //Realizando guardado de Timbre en BD
                            result = TimbreFiscalDigital.InsertaTimbreFiscalDigital(this._id_comprobante33, pac.id_compania_emisor_receptor, pac.rfc,
                                timbre.Attribute("Leyenda") != null ? timbre.Attribute("Leyenda").Value : "", timbre.Attribute("Version").Value,
                                timbre.Attribute("UUID").Value, Convert.ToDateTime(timbre.Attribute("FechaTimbrado").Value), timbre.Attribute("SelloCFD").Value,
                                timbre.Attribute("NoCertificadoSAT").Value, timbre.Attribute("SelloSAT").Value, id_usuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede recuperar el PAC");
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Imposible recuperar datos de timbre desde el comprobante.");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Genera Codigo Bidimensional Comprobante
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        protected RetornoOperacion generaCodigoBidimensionalComprobante(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Intsanciamos Emisor
            using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
            {
                //Creando ruta de guardado del comprobante
                string ruta_codigo = string.Format(@"{0}{1}\{2}\QR\{3}{4}.jpeg", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 209.ToString("0000"),
                                                          em.DirectorioAlmacenamiento, serie, folio);

                //Eliminando archivo si es que ya existe
                Archivo.EliminaArchivo(ruta_codigo);

                //Instanciamos Receptor
                using (CompaniaEmisorReceptor rec = new CompaniaEmisorReceptor(this._id_compania_receptor))
                {
                    //Obteniendo Timbre Fiscal
                    using (TimbreFiscalDigital objTimbreFiscal = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante33))
                    {
                        //Generamos Codigo Bidimensional
                        string cadena_codigo = string.Format("?re={0}&rr={1}&tt={2:0.000000}&id={3}", em.rfc, rec.rfc, this._total_captura, objTimbreFiscal.UUID);
                        byte[] ArchivoBytes = Dibujo.GeneraCodigoBidimensional(cadena_codigo, System.Drawing.Imaging.ImageFormat.Jpeg);

                        //Validamos Obtención de Bytes
                        if (ArchivoBytes != null)
                        
                            //Añadimos Archivo
                            resultado = ArchivoRegistro.InsertaArchivoRegistro(209, this._id_comprobante33, 23, "", id_usuario, ArchivoBytes, ruta_codigo);
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No se puede cargar bytes (QR)");
                    }
                }
            }

            //Devolviendo Resultado
            return resultado;
        }
        /// <summary>
        /// Añade las addendas definidas para el receptor y emisor del comprobante
        /// </summary>
        /// <param name="comprobante">Elemento xml a editar</param>
        /// <param name="ns_sat">Namespace del SAT para asociar elementos contenedores de addeenda</param>
        /// <param name="id_usuario">Usuario</param>
        /// <returns></returns>
        protected RetornoOperacion anadeElementoAddendaComprobante(ref XDocument comprobante, XNamespace ns_sat, int id_usuario)
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion result = new RetornoOperacion(this._id_comprobante33);

            //Cargando las addendas aplicables
            using (DataTable addendas = FEv32.AddendaEmisor.CargaAddendasRequeridasEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor))
            {
                //Si existen addendas configuradas
                if (Validacion.ValidaOrigenDatos(addendas))
                {
                    //Se crea al no ser obligatorio
                    XElement elemento_addenda = new XElement(ns_sat + "Addenda");

                    //Definiendo objeto de validación de captura de al menos un addenda
                    List<RetornoOperacion> resultado_addenda = new List<RetornoOperacion>();

                    //Para cada una de las addendas configuradas
                    foreach (DataRow a in addendas.Rows)
                    {
                        RetornoOperacion r = new RetornoOperacion(this._id_comprobante33);

                        //Recuperando la addenda aplicable al comprobante
                        using (FEv32.AddendaComprobante ac = FEv32.AddendaComprobante.RecuperaAddendaComprobanteV33(Convert.ToInt32(a["Id"]), this._id_comprobante33))
                        {
                            //Si la addenda se ha capturado y es válida
                            if (ac.id_addenda_comprobante > 0 && ac.habilitar && ac.bit_validacion)
                            {
                                //Declarando elemento por añadir
                                XElement elemento_ac = null;

                                try
                                {
                                    //Cargando XML correspondiente
                                    elemento_ac = XDocument.Parse(ac.xml_addenda.OuterXml).Root;
                                }
                                catch (Exception ex)
                                {
                                    r = new RetornoOperacion(ex.Message);
                                }

                                //Si el elemento no está vacío
                                if (r.OperacionExitosa)
                                {
                                    //Si existe un esquema adicional que actualizar
                                    if (elemento_ac.Attribute("schemaLocation") != null)
                                    {
                                        //Localizando atributo 'schemaLocation', para añadir el prefijo del espacio de nombres de la W3C
                                        string valor_schemaLocation = elemento_ac.Attribute("schemaLocation").Value;

                                        //Si se localizó
                                        if (valor_schemaLocation != "")
                                        {
                                            //Eliminando atributo, añadiendo el prefijo correspondiente
                                            elemento_ac.Attribute("schemaLocation").Remove();
                                            //Creando nuevo atributo con prefijo correspondiente y el valor del atributo anterior
                                            elemento_ac.Add(new XAttribute(comprobante.Root.GetNamespaceOfPrefix("xsi") + "schemaLocation", valor_schemaLocation));
                                        }
                                        else
                                            r = new RetornoOperacion("Error en addenda, el atributo 'schemaLocation' no tiene ningún valor.");
                                    }

                                    //Si no hay errores
                                    if (r.OperacionExitosa)
                                    {
                                        //Instanciando addenda para recuperar 
                                        using (FEv32.Addenda ad = new FEv32.Addenda(Convert.ToInt32(a["IdAddenda"])))
                                        {
                                            //Validando carga correcta de registro
                                            if (ad.id_addenda > 0)
                                            {
                                                //Definiendo objeto para almacenamiento del contenido de addendas 
                                                //(se añade debido a que el contenido de este nodo no está regulado bajo ningún estándar 
                                                //y puede ser que un addenda no contenga un elemento raíz)
                                                object obj_elemento_ac = elemento_ac;

                                                //Creando addenda personalizada (en caso de estar definida para el receptor)
                                                r = FEv32.AddendaEmisor.CreaAddendaReceptor(ref obj_elemento_ac, this);

                                                //Si no existe error
                                                if (r.OperacionExitosa)
                                                {
                                                    //Añadimos Addenda a Complemento
                                                    elemento_addenda.Add(obj_elemento_ac);

                                                    //Covertimos XElement a XmlDocumento para edición de la Addenda
                                                    using (XmlReader xmlReader = System.Xml.Linq.XElement.Parse(obj_elemento_ac.ToString()).CreateReader())
                                                    {
                                                        XmlDocument xmlDoc = new XmlDocument();
                                                        xmlDoc.Load(xmlReader);

                                                        //Guradamos Addenda en BD
                                                        result = ac.EditarAddendaComprobante(xmlDoc, id_usuario);
                                                    }
                                                }
                                            }
                                            else
                                                r = new RetornoOperacion("Error en addenda, no fue posible recuperar configuración de addenda.");
                                        }
                                    }
                                }
                                else
                                    r = new RetornoOperacion("Error en addenda, el elemento principal no fue encontrado.");
                            }
                            else
                                //Actualizando error
                                r = new RetornoOperacion("Error en addenda, addenda sin validación de esquema.");
                        }

                        //Añadiendo resultado
                        resultado_addenda.Add(r);
                    }

                    //SI existe al menos una addenda exitosa
                    RetornoOperacion x = (from RetornoOperacion r in resultado_addenda
                                          where r.OperacionExitosa == true
                                          select r).FirstOrDefault();

                    //SI NO HAY RESULTADOS SATISFACTORIOS (SÓLO ERRORES)
                    if (x == null)
                    {
                        result = new RetornoOperacion("No se ha capturado addenda o bien no ha sido validada.");
                    }
                    else
                    {
                        //Si no existen errores y al menos una captura fue correcta
                        if (x.OperacionExitosa)
                        {
                            //Si addenda tiene contenido
                            if (elemento_addenda.HasElements)
                                comprobante.Root.Add(elemento_addenda);
                        }
                        else
                        {
                            //Mostrando erores
                            string mensaje_error = "";
                            foreach (RetornoOperacion r in resultado_addenda)
                                mensaje_error += r.Mensaje + "<br/>";
                            result = new RetornoOperacion(mensaje_error, false);
                        }
                    }
                }
            }

            //Devolviendo resultado
            return result;
        }
        /// <summary>
        /// Realiza la importación de registros concepto a partir de la definición de DataTable de Conceptos (Esquema de Importación)
        /// </summary>
        /// <param name="conceptos">Conceptos a importar</param>
        /// <param name="referencias_conceptos">Referencias de Conceptos</param>
        /// <param name="referencia_comprobante">Referencia de Comprobante</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private static RetornoOperacion importaConceptosComprobante_V3_3(DataTable conceptos, DataTable concepto_impuestos, DataTable referencias_conceptos, DataTable referencia_impresion_concepto,
            int id_comprobante, bool impresion_detalles, int id_usuario)
        {
            //Definiendo resultado (es obligatorio tener conceptos)
            RetornoOperacion resultado = new RetornoOperacion();
            //variable para Almacenar el Id de Concepto
            int id_concepto = 0;
            //Si hay conceptos
            if (Validacion.ValidaOrigenDatos(conceptos))
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Realizando lectura de conceptos
                    foreach (DataRow cn in conceptos.Rows)
                    {
                        //Realizando la inserción correspondinte
                        resultado = Concepto.InsertaConcepto(id_comprobante, Convert.ToInt32(cn["Id_concepto_Padre"]), Convert.ToDecimal(cn["Cantidad"]),
                                                            Convert.ToInt32(cn["Id_unidad"]), Convert.ToInt32(cn["Id_tipo_cargo"]), Convert.ToInt32(cn["Id_Clave_SAT"]), cn["Descripcion"].ToString(), cn["No_identificacion"].ToString(),
                                                            Convert.ToDecimal(cn["Valor_unitario"]), Convert.ToDecimal(cn["Importe_captura"]), Convert.ToDecimal(cn["Importe_moneda_nacional"]),
                                                            cn["Cuenta_Predial"].ToString(), Convert.ToDecimal(cn["Descuento"]), id_usuario);
                        //Asignavos valor
                        id_concepto = resultado.IdRegistro;
                        //Si no hay error
                        if (resultado.OperacionExitosa)
                            //Actualizando Id de Importación para inserción posterior de detalle de impuesto
                            cn.SetField<int>("Id_Importacion", resultado.IdRegistro);
                        else
                            //De lo contrario terminando ciclo de conceptos
                            break;

                        //Armando Refencia de Concepto para Impreción de Comprobante
                        AgregaReferenciaComprobante(referencia_impresion_concepto, referencias_conceptos, Convert.ToInt32(cn["Id_concepto_origen"]), 215, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 215, "Referencias", 0, "Facturacion Electrónica"), impresion_detalles);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Validando existencia de referencias
                            if (Validacion.ValidaOrigenDatos(referencias_conceptos))
                            {
                                //Para cada una de las referencias encontradas
                                foreach (DataRow r in referencias_conceptos.Rows)
                                {
                                    //Validamos el Origen del Concepto
                                    if (Convert.ToInt32(cn["Id_concepto_origen"]) == Convert.ToInt32(r["id_concepto_origen"]))
                                    {
                                        //realzaindo la inserción correspondiente
                                        resultado = Referencia.InsertaReferencia(id_concepto, Convert.ToInt32(r["id_tabla"]), Convert.ToInt32(r["id_tipo"]), r["valor"].ToString(),
                                                                        Convert.ToDateTime(r["fecha"]), id_usuario, true);

                                        //Si existe algún error
                                        if (!resultado.OperacionExitosa)
                                        {
                                            resultado = new RetornoOperacion(string.Format("Error al registrar Referencia '{0}'; {1}", r["valor"], resultado.Mensaje));
                                            break;
                                        }
                                    }

                                }
                            }
                        }

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Declarando Impuesto
                            int id_impuesto = 0;

                            //Validando existencia de referencias
                            if (Validacion.ValidaOrigenDatos(concepto_impuestos))
                            {
                                //Validando Impuesto del Comprobante
                                using (Impuesto imp = Impuesto.ObtieneImpuestoComprobante(id_comprobante))
                                {
                                    //Validando Impuesto
                                    if (imp.habilitar)

                                        //Resultado Positivo
                                        resultado = new RetornoOperacion(imp.id_impuesto);
                                    else
                                        //Insertando Impuesto del Comprobante
                                        resultado = Impuesto.InsertaImpuesto(id_comprobante, 0, 0, 0, 0, id_usuario);
                                }

                                //Validando Operación Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Obteniendo Impuesto
                                    id_impuesto = resultado.IdRegistro;

                                    //Para cada relación
                                    foreach (DataRow ic in concepto_impuestos.Select("Id_concepto_origen = " + cn.Field<int>("Id_concepto_origen").ToString()))
                                    {
                                        //Recuperando datos de Impuesto y concepto al que pertenece (Id de importación previa)
                                        int id_conc = (from DataRow r in conceptos.Rows
                                                       where r.Field<int>("Id_concepto_origen") == ic.Field<int>("Id_concepto_origen")
                                                       select r.Field<int>("Id_Importacion")).FirstOrDefault();
                                        decimal imp_base = (from DataRow r in conceptos.Rows
                                                            where r.Field<int>("Id_concepto_origen") == ic.Field<int>("Id_concepto_origen")
                                                            select r.Field<decimal>("Importe_captura")).FirstOrDefault();
                                        decimal imp_mon_cap = (from DataRow r in conceptos.Rows
                                                               where r.Field<int>("Id_concepto_origen") == ic.Field<int>("Id_concepto_origen")
                                                               select r.Field<decimal>("Importe_moneda_nacional")).FirstOrDefault();

                                        //Verificando que los valores se hayan recuperado correctamente
                                        if (id_conc > 0 && imp_base > 0.00M && imp_mon_cap > 0.00M)
                                        {
                                            //Insertando Detalle del Concepto
                                            resultado = ImpuestoDetalle.InsertaImpuestoDetalle(id_impuesto, (ImpuestoDetalle.TipoImpuestoDetalle)ic.Field<byte>("Id_tipo_detalle"), id_conc,
                                                                                    1, imp_base, ic.Field<byte>("Id_imp_ret"), ic.Field<byte>("Id_imp_tra"), ic.Field<decimal>("Tasa_impuesto"),
                                                                                    ic.Field<decimal>("Importe_captura"), ic.Field<decimal>("Importe_moneda_nacional"), id_usuario);

                                            //Si existe algún error
                                            if (!resultado.OperacionExitosa)
                                                //Saliendo de ciclo
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Actualizamos Total Comprobante
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando Comprobante
                        using (Comprobante cmp = new Comprobante(id_comprobante))
                        {
                            //Validando Existencia
                            if (cmp.habilitar)

                                //Actualizando SubTotal del Comprobante
                                resultado = cmp.ActualizaSubtotalComprobante(id_usuario);
                        }
                        
                        //Validando Operación Final
                        if (resultado.OperacionExitosa)

                            //Completando Transacción
                            scope.Complete();
                    }
                    else
                        resultado = new RetornoOperacion("No se han podido guardar todos los conceptos.");
                }
            }
            else
                resultado = new RetornoOperacion("No existen conceptos que registrar.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la importación de registros descuento a partir de la definición de DataTable de Descuentos (Esquema de Importación)
        /// </summary>
        /// <param name="descuentos">Descuentos a Importar</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private static RetornoOperacion importaDescuentosComprobante_V3_3(DataTable descuentos, int id_comprobante, int id_usuario)
        {
            //Declarando objeto de retorno sin error (No es obligatorio tener descuentos)
            RetornoOperacion resultado = new RetornoOperacion(id_comprobante);

            //Si hay descuentos
            if (Validacion.ValidaOrigenDatos(descuentos))
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Para cada uno de los descuentos
                    foreach (DataRow des in descuentos.Rows)
                    {
                        //Insertando descuento
                        resultado = Descuento.InsertaDescuento(id_comprobante, Convert.ToDecimal(des["Porcentaje"]),
                                        Convert.ToDecimal(des["Importe_captura"]), Convert.ToDecimal(des["Importe_moneda_nacional"]), id_usuario);

                        //Actualizamos Descuentos Comprobante
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Comprobante
                            using (Comprobante objComprobante = new Comprobante(id_comprobante))
                            {
                                //Si el comprobante existe
                                if (objComprobante.id_comprobante33 > 0)
                                    //Actualizamos Total Comprobante
                                    resultado = objComprobante.ActualizaDescuento(Convert.ToDecimal(des["Importe_captura"]), Convert.ToDecimal(des["Importe_moneda_nacional"]), id_usuario);
                                else
                                    resultado = new RetornoOperacion("No pudo ser recuperado el comprobante.");
                            }
                        }
                        else
                            resultado = new RetornoOperacion("No se han podido guardar el descuento.");

                        //Saliendo de ciclo (sólo se permite un descuento)
                        break;
                    }
                    //Validamos resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referencia_comprobante"></param>
        /// <param name="referencia_concepto"></param>
        /// <param name="id_concepto_origen"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_tipo"></param>
        /// <param name="detalles"></param>
        private static void AgregaReferenciaComprobante(DataTable referencia_comprobante, DataTable referencia_concepto, int id_concepto_origen, int id_tabla, int id_tipo, bool detalles)
        {
            //Declaramos Variable
            DataRow[] re = null;
            //Validamos que exista Referencia
            if (Validacion.ValidaOrigenDatos(referencia_comprobante))
            {

                //Obtenemos La Referencias del Concepto Origen
                re = (from DataRow r in referencia_comprobante.Rows
                      where Convert.ToInt32(r["id_origen"]) == id_concepto_origen
                      select r).ToArray();
                //Validamos que exista elementos
                if (re.Length > 0)
                {
                    //Obtenemos Datatable Referencias solo del Concepto Origen
                    DataTable dtReferenciasConcepto = TSDK.Datos.OrigenDatos.ConvierteArregloDataRowADataTable(re);

                    if (detalles)
                    {
                        //Añadimos Registro
                        referencia_concepto.Rows.Add(id_concepto_origen, id_tabla, id_tipo, SAT_CL.Global.Referencia.ObtieneReferenciaDetalles(dtReferenciasConcepto), Fecha.ObtieneFechaEstandarMexicoCentro());
                    }
                    else
                    {

                        SAT_CL.Global.Referencia.ObtieneReferenciaGlobal(dtReferenciasConcepto, referencia_concepto, id_concepto_origen, id_tabla, id_tipo);

                    }
                }
            }
        }
        /// <summary>
        /// Realiza la validación de los requerimientos previos al timbrado de comprobante
        /// </summary>
        /// <param name="serie">Serie del Comprobante</param>
        /// <param name="folio">Folio del Comprobante</param>
        /// <param name="id_certificado_activo">Id de Certificado de Sello Digital que tiene activo el emisor del comprobante</param>
        /// <param name="numero_certificado">Número de Certificado de Sello Digital a Utilizar</param>
        /// <param name="certificado_base_64">Contenido del Certificado en Base64</param>
        /// <param name="contrasena_apertura">Contraseña de Apertura</param>
        /// <param name="bytes_certificado">Bytes del Certificado .cer Activo</param>
        /// <returns></returns>
        protected RetornoOperacion validaRequisitosPreviosTimbradoNomina(string serie, out string folio, out int id_certificado_activo,
                                        out string numero_certificado, out string certificado_base_64, out string contrasena_apertura,
                                        out byte[] bytes_certificado, SerieFolioCFDI.TipoSerieFolioCFDI tipoFolioSerie)
        {
            //Inicializando valores de parámetros de salida
            numero_certificado = certificado_base_64 = contrasena_apertura = folio = "";
            id_certificado_activo = 0;
            bytes_certificado = null;

            //Defiiniendo auxiliares para obtenciaón de certificado
            int id_compania_emisor = 0, id_sucursal = 0;

            //Definiendo objeto de retorno
            RetornoOperacion result = new RetornoOperacion(1);

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que el comprobante no esté timbrado
                if (!this._bit_generado)
                {
                    //Validando la fecha de cambio(a la fecha de timbrado)
                    if (this._id_moneda != 1)
                    {
                        //Si la Moneda es Distinta de MXN
                        if (this._id_moneda != 1)
                        {
                            //Validando el Tipo de Cambio
                            if (!(this._tipo_cambio > 0))

                                //Instanciando Excepción
                                result = new RetornoOperacion("El Comprobante debe de tener un Tipo de Cambio Valido");
                        }
                    }

                    //Si no existen errores hasta aqui
                    if (result.OperacionExitosa)
                    {
                        //Verificando que el emisor continue activo
                        using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                        {
                            if (em.id_compania_emisor_receptor > 0 && em.habilitar)
                            {
                                //Validando sucursal
                                if (this._id_sucursal > 0)
                                {
                                    //Instanciando sucursal
                                    using (Sucursal s = new Sucursal(this._id_sucursal))
                                    {
                                        if (s.id_sucursal <= 0 || !s.habilitar)
                                            result = new RetornoOperacion("La sucursal no se encuentra activa.");

                                        //Asignando Id de Sucursal
                                        id_sucursal = s.id_sucursal;
                                    }
                                }
                            }
                            else
                                result = new RetornoOperacion("El emisor no se encuentra activo.");

                            //Asignando Id de Emisor
                            id_compania_emisor = em.id_compania_emisor_receptor;

                            //Si no existe error
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo el certificado activo para el emisor y/o sucursal
                                using (CertificadoDigital certificado = CertificadoDigital.RecuperaCertificadoEmisorSucursal(id_compania_emisor, id_sucursal, CertificadoDigital.TipoCertificado.CSD))
                                {
                                    //Si el certificado no fue encontrado
                                    if (certificado.id_certificado_digital > 0 && certificado.Habilitar)
                                    {
                                        //Cargando certificado (.cer)
                                        using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                                        {
                                            //Comprobando carga de certificado
                                            if (cer.Subject != null)
                                            {
                                                //Verificando pertenencia de certificado
                                                if (cer.Subject.RFCPropietario == em.rfc)
                                                {
                                                    //Asignando parámteros de salida
                                                    id_certificado_activo = certificado.id_certificado_digital;
                                                    numero_certificado = cer.No_Serie;
                                                    certificado_base_64 = cer.CertificadoBase64;
                                                    contrasena_apertura = certificado.contrasena_desencriptada;
                                                    bytes_certificado = System.IO.File.ReadAllBytes(certificado.ruta_llave_privada);
                                                    //ltado = new RetornoOperacion(string.Format("El certificado ha expirado: {0:dd/MM/yyyy}.", cer.FinValidez));
                                                }
                                                else
                                                    result = new RetornoOperacion("Este certificado no pertence al emisor que sellará el comprobante.");
                                            }
                                            else
                                                result = new RetornoOperacion("Imposible recuperar los datos de Propietario del Certificado.");
                                        }
                                    }
                                    else
                                        result = new RetornoOperacion("No existe un Certificado de Sello Digital activo.");
                                }
                            }

                            //Si no hay erroes
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Conceptos
                                using (DataTable dtConceptos = Concepto.ObtieneConceptosComprobante(this._id_comprobante33))
                                {
                                    //Validando que no existan Conceptos
                                    if (!Validacion.ValidaOrigenDatos(dtConceptos))

                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No existen Conceptos en el Comprobante");
                                }
                            }

                            //Si no existe error
                            if (result.OperacionExitosa)
                            {
                                //Instanciamos emisor y serie para obtener el tipo definido
                                using (SerieFolioCFDI objSerieFolio = new SerieFolioCFDI(serie, version, id_compania_emisor))
                                {
                                    ///Validamos el Tipo de Serie
                                    if (objSerieFolio.tipo_folio_serie == tipoFolioSerie)
                                    {
                                        //Realizando búsqueda de folio por asignar
                                        folio = obtieneFolioPorAsignar(this._id_compania_emisor, version, serie);
                                        //Si no existe un folio disponible
                                        if (Convert.ToInt32(folio) <= 0)
                                            //Instanciando Excepción
                                            result = new RetornoOperacion(string.Format("No existe un folio disponible para la serie {0}.", serie.ToUpper()));
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(string.Format("La serie no se encuentra disponible {0}.", serie));
                                }
                            }
                        }
                    }
                }
                else
                    result = new RetornoOperacion("El comprobante ya se ha timbrado anteriormente.");

                //Validamos Resultado
                if (result.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return result;
        }
        /// <summary>
        /// Añade las addendas definidas para el receptor y emisor del comprobante
        /// </summary>
        /// <param name="comprobante">Elemento xml a editar</param>
        /// <param name="ns_sat">Namespace del SAT para asociar elementos contenedores de complemento</param>
        /// <returns></returns>
        protected RetornoOperacion anadeElementoComplementoReciboNominaV3_3(string version, ref XElement comprobante, XNamespace ns_sat)
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante33);

            //Cargando las addendas aplicables
            using (DataTable addendas = FEv32.AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor, "Nómina " + version))
            {
                //Si existen addendas configuradas
                if (Validacion.ValidaOrigenDatos(addendas))
                {
                    //Si no existe el elemento Complemento
                    if (comprobante.Element(ns_sat + "Complemento") == null)
                        //Añadiendolo
                        comprobante.Add(new XElement(ns_sat + "Complemento"));

                    //Creando elementos que pueden contener addenda
                    //Se recupera, ya que este existe por ser obligatorio para timbre
                    XElement elemento_complemento = comprobante.Element(ns_sat + "Complemento");

                    //Definiendo objeto de validación de captura de al menos un addenda
                    List<RetornoOperacion> resultado_addenda = new List<RetornoOperacion>();

                    //Para cada una de las addendas configuradas
                    foreach (DataRow a in addendas.Rows)
                    {
                        RetornoOperacion r = new RetornoOperacion(this._id_comprobante33);

                        //Recuperando la addenda aplicable al comprobante
                        using (FEv32.AddendaComprobante ac = FEv32.AddendaComprobante.RecuperaAddendaComprobante(Convert.ToInt32(a["Id"]), this._id_comprobante33))
                        {
                            //Si la addenda se ha capturado y es válida
                            if (ac.id_addenda_comprobante > 0 && ac.habilitar && ac.bit_validacion)
                            {
                                //Declarando elemento por añadir
                                XElement elemento_ac = null;

                                try
                                {
                                    //Cargando XML correspondiente
                                    elemento_ac = XDocument.Parse(ac.xml_addenda.OuterXml).Root;
                                }
                                catch (Exception ex)
                                {
                                    r = new RetornoOperacion(ex.Message);
                                }

                                //Si el elemento no está vacío
                                if (r.OperacionExitosa)
                                {
                                    //Si existe un esquema adicional que actualizar
                                    if (elemento_ac.Attribute("schemaLocation") != null)
                                    {
                                        //Localizando atributo 'schemaLocation', para añadir el prefijo del espacio de nombres de la W3C
                                        string valor_schemaLocation = elemento_ac.Attribute("schemaLocation").Value;

                                        //Si se localizó
                                        if (valor_schemaLocation != "")
                                        {
                                            //Eliminando atributo, añadiendo el prefijo correspondiente
                                            elemento_ac.Attribute("schemaLocation").Remove();
                                            //Creando nuevo atributo con prefijo correspondiente y el valor del atributo anterior
                                            elemento_ac.Add(new XAttribute(comprobante.GetNamespaceOfPrefix("xsi") + "schemaLocation", valor_schemaLocation));
                                        }
                                        else
                                            r = new RetornoOperacion("Error en addenda, el atributo 'schemaLocation' no tiene ningún valor.");
                                    }

                                    //Si no hay errores
                                    if (r.OperacionExitosa)
                                    {
                                        //Instanciando addenda para recuperar 
                                        using (FEv32.Addenda ad = new FEv32.Addenda(Convert.ToInt32(a["IdAddenda"])))
                                        {
                                            //Validando carga correcta de registro
                                            if (ad.id_addenda > 0)
                                            {
                                                //Definiendo objeto para almacenamiento del contenido de addendas 
                                                //(se añade debido a que el contenido de este nodo no está regulado bajo ningún estándar 
                                                //y puede ser que un addenda no contenga un elemento raíz)
                                                object obj_elemento_ac = elemento_ac;

                                                //Creando addenda personalizada (en caso de estar definida para el receptor)
                                                r = FEv32.AddendaEmisor.CreaAddendaReceptor(ref obj_elemento_ac, this);

                                                //Si no existe error
                                                if (r.OperacionExitosa)
                                                    elemento_complemento.Add(obj_elemento_ac);
                                            }
                                            else
                                                r = new RetornoOperacion("Error en addenda, no fue posible recuperar configuración de addenda.");
                                        }
                                    }
                                }
                                else
                                    r = new RetornoOperacion("Error en addenda, el elemento principal no fue encontrado.");
                            }
                            else
                                //Actualizando error
                                r = new RetornoOperacion("Error en addenda, addenda sin validación de esquema.");
                        }

                        //Añadiendo resultado
                        resultado_addenda.Add(r);
                    }

                    //Si existe al menos una addenda exitosa
                    RetornoOperacion x = (from RetornoOperacion r in resultado_addenda
                                          where r.OperacionExitosa == true
                                          select r).FirstOrDefault();

                    //SI NO HAY RESULTADOS SATISFACTORIOS (SÓLO ERRORES)
                    if (x == null)
                    {
                        resultado = new RetornoOperacion("No se ha capturado addenda o bien no ha sido validada.");
                    }
                    else
                    {
                        //Si existen errores y al menos una captura fue correcta
                        if (!x.OperacionExitosa)
                        {
                            //Mostrando erores
                            string mensaje_error = "";
                            foreach (RetornoOperacion r in resultado_addenda)
                                mensaje_error += r.Mensaje + "<br/>";
                            resultado = new RetornoOperacion(mensaje_error, false);
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera el mensaje del contenido de correo en base a la plantilla predefinida para esta opción
        /// </summary>
        /// <param name="mensaje_personalizado">Mensaje de texto definido por el usuario</param>
        /// <returns></returns>
        protected string generaMensajeEmailCFDI(string mensaje_personalizado)
        {
            //Formato predeterminado
            string formato = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/FormatosHTML/CFDI_Email_1.html"));

            //Instanciando los elementos requeridos para esta acción
            using (CompaniaEmisorReceptor emisor = new CompaniaEmisorReceptor(this._id_compania_emisor))
            {
                //Obteniendo las imagenes a incluir en base64
                string logoTectosBase64 = Convert.ToBase64String(File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Image/IT logo version 3 (non-editable web-ready file).png")));
                string logoEmpresaBase64 = Convert.ToBase64String(File.ReadAllBytes(emisor.ruta_logotipo));


                //Declarando objeto de retorno
                return formato.Replace("{0}", emisor.nombre).Replace("{1}", this.serie + this.folio.ToString()).Replace("{2}", logoEmpresaBase64).Replace("{3}", mensaje_personalizado).Replace("{4}", logoTectosBase64);
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Comprobantes en su versión 3.3
        /// </summary>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante (I,E,T,P,N)</param>
        /// <param name="id_origen_datos">Origen de Datos</param>
        /// <param name="id_certificado">Certificado del Timbrado</param>
        /// <param name="version">Versión del Comprobante</param>
        /// <param name="serie">Serie</param>
        /// <param name="folio">Folio</param>
        /// <param name="sello">Sello Fiscal Digital</param>
        /// <param name="id_forma_pago">Forma de Pago (Para el Complemento de Pago)</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="condicion_pago">Condición de Pago</param>
        /// <param name="id_moneda">Moneda</param>
        /// <param name="subtotal_captura">SubTotal de Captura</param>
        /// <param name="impuestos_captura">Impuestos de Captura</param>
        /// <param name="descuentos_captura">Descuentos de Captura</param>
        /// <param name="total_captura">Total de Captura</param>
        /// <param name="subtotal_nacional">SubTotal (Nacional MXN)</param>
        /// <param name="impuestos_nacional">Impuestos (Nacional MXN)</param>
        /// <param name="descuentos_nacional">Descuentos (Nacional MXN)</param>
        /// <param name="total_nacional">Total (Nacional MXN)</param>
        /// <param name="tipo_cambio">Tipo de Cambio</param>
        /// <param name="lugar_expedicion">Lugar de Expedición (Código Postal)</param>
        /// <param name="id_direccion_lugar_expedicion">Dirección del Lugar de Exp.</param>
        /// <param name="confirmacion">No. de Confirmación del PAC</param>
        /// <param name="id_compania_emisor">Emisor del Comprobante</param>
        /// <param name="regimen_fiscal">Regimen Fiscal</param>
        /// <param name="id_sucursal">Sucursal</param>
        /// <param name="id_compania_receptor">Receptor del Comprobante</param>
        /// <param name="id_uso_cfdi_receptor">Uso del CFDI</param>
        /// <param name="fecha_captura">Fecha de Captura del >Comprobante</param>
        /// <param name="fecha_expedicion">Fecha de Expedición del Comprobante</param>
        /// <param name="fecha_cancelacion">Fecha de Cancelación del Comprobante</param>
        /// <param name="bit_generado">Indicador de Generación del Comprobante</param>
        /// <param name="bit_transferido_nuevo">Indicador de Nueva Transferencia</param>
        /// <param name="id_transferencia_nuevo">Identificador de Nueva Transferencia</param>
        /// <param name="bit_transferido_cancelar">Indicador de Transferencia Cancelada</param>
        /// <param name="id_transferencia_cancelar">Identificador de Transferencia Cancelada</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaComprobante(int id_tipo_comprobante, int id_origen_datos,
                        int id_certificado, string version, string serie, string folio, string sello, int id_forma_pago, int id_metodo_pago,
                        string condicion_pago, int id_moneda, decimal subtotal_captura, decimal impuestos_captura, decimal descuentos_captura,
                        decimal total_captura, decimal subtotal_nacional, decimal impuestos_nacional, decimal descuentos_nacional,
                        decimal total_nacional, decimal tipo_cambio, string lugar_expedicion, int id_direccion_lugar_expedicion,
                        string confirmacion, int id_compania_emisor, string regimen_fiscal, int id_sucursal, int id_compania_receptor, 
                        int id_uso_cfdi_receptor, DateTime fecha_captura, DateTime fecha_expedicion,
                        DateTime fecha_cancelacion, bool bit_generado, bool bit_transferido_nuevo, int id_transferencia_nuevo,
                        bool bit_transferido_cancelar, int id_transferencia_cancelar, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Arreglo de Parametros
            object[] param = { 1, 0, (byte)EstatusVigencia.Vigente, id_tipo_comprobante, id_origen_datos, id_certificado, version, 
                               serie, folio, sello, id_forma_pago, id_metodo_pago, condicion_pago, id_moneda, subtotal_captura, impuestos_captura,
                               descuentos_captura, total_captura, subtotal_nacional, impuestos_nacional, descuentos_nacional, total_nacional, 
                               tipo_cambio, lugar_expedicion, id_direccion_lugar_expedicion, confirmacion, id_compania_emisor, regimen_fiscal, id_sucursal,
                               id_compania_receptor, id_uso_cfdi_receptor, fecha_captura, fecha_expedicion, fecha_cancelacion, bit_generado, bit_transferido_nuevo, 
                               id_transferencia_nuevo, bit_transferido_cancelar, id_transferencia_cancelar, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_tipo_comprobante"></param>
        /// <param name="id_origen_datos"></param>
        /// <param name="version"></param>
        /// <param name="id_forma_pago"></param>
        /// <param name="id_metodo_pago"></param>
        /// <param name="condicion_pago"></param>
        /// <param name="id_moneda"></param>
        /// <param name="tipo_cambio"></param>
        /// <param name="lugar_expedicion"></param>
        /// <param name="id_direccion_lugar_expedicion"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="regimen_fiscal"></param>
        /// <param name="id_sucursal"></param>
        /// <param name="id_compania_receptor"></param>
        /// <param name="id_uso_cfdi_receptor"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaComprobante(int id_tipo_comprobante, int id_origen_datos,
                        string version, int id_forma_pago, int id_metodo_pago,
                        string condicion_pago, int id_moneda, decimal tipo_cambio, string lugar_expedicion, int id_direccion_lugar_expedicion,
                        int id_compania_emisor, string regimen_fiscal, int id_sucursal, int id_compania_receptor,
                        int id_uso_cfdi_receptor, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Arreglo de Parametros
            object[] param = { 1, 0, (byte)EstatusVigencia.Vigente, id_tipo_comprobante, id_origen_datos, 0, version, "", "", "", 
                               id_forma_pago, id_metodo_pago, condicion_pago, id_moneda, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 
                               tipo_cambio, lugar_expedicion, id_direccion_lugar_expedicion, "", id_compania_emisor, regimen_fiscal, id_sucursal,
                               id_compania_receptor, id_uso_cfdi_receptor, Fecha.ObtieneFechaEstandarMexicoCentro(), null, null, false, false, 
                               0, false, 0, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_tipo_comprobante"></param>
        /// <param name="id_origen_datos"></param>
        /// <param name="version"></param>
        /// <param name="id_forma_pago"></param>
        /// <param name="id_metodo_pago"></param>
        /// <param name="condicion_pago"></param>
        /// <param name="id_moneda"></param>
        /// <param name="tipo_cambio"></param>
        /// <param name="lugar_expedicion"></param>
        /// <param name="id_direccion_lugar_expedicion"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="regimen_fiscal"></param>
        /// <param name="id_sucursal"></param>
        /// <param name="id_compania_receptor"></param>
        /// <param name="id_uso_cfdi_receptor"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaComprobante(int id_tipo_comprobante, int id_origen_datos,
                        string version, int id_forma_pago, int id_metodo_pago,
                        string condicion_pago, int id_moneda, decimal tipo_cambio, string lugar_expedicion, int id_direccion_lugar_expedicion,
                        int id_compania_emisor, string regimen_fiscal, int id_sucursal, int id_compania_receptor,
                        int id_uso_cfdi_receptor, DateTime fecha, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Arreglo de Parametros
            object[] param = { 1, 0, (byte)EstatusVigencia.Vigente, id_tipo_comprobante, id_origen_datos, 0, version, "", "", "", 
                               id_forma_pago, id_metodo_pago, condicion_pago, id_moneda, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 
                               tipo_cambio, lugar_expedicion, id_direccion_lugar_expedicion, "", id_compania_emisor, regimen_fiscal, id_sucursal,
                               id_compania_receptor, id_uso_cfdi_receptor, Fecha.ConvierteDateTimeObjeto(fecha == DateTime.MinValue ? Fecha.ObtieneFechaEstandarMexicoCentro() : fecha), null, null, false, false, 
                               0, false, 0, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Comprobantes en su versión 3.3
        /// </summary>
        /// <param name="estatus_vigencia">Estatus de la Vigencia</param>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante (I,E,T,P,N)</param>
        /// <param name="id_origen_datos">Origen de Datos</param>
        /// <param name="id_certificado">Certificado del Timbrado</param>
        /// <param name="version">Versión del Comprobante</param>
        /// <param name="serie">Serie</param>
        /// <param name="folio">Folio</param>
        /// <param name="sello">Sello Fiscal Digital</param>
        /// <param name="id_forma_pago">Forma de Pago (Para el Complemento de Pago)</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="condicion_pago">Condición de Pago</param>
        /// <param name="id_moneda">Moneda</param>
        /// <param name="subtotal_captura">SubTotal de Captura</param>
        /// <param name="impuestos_captura">Impuestos de Captura</param>
        /// <param name="descuentos_captura">Descuentos de Captura</param>
        /// <param name="total_captura">Total de Captura</param>
        /// <param name="subtotal_nacional">SubTotal (Nacional MXN)</param>
        /// <param name="impuestos_nacional">Impuestos (Nacional MXN)</param>
        /// <param name="descuentos_nacional">Descuentos (Nacional MXN)</param>
        /// <param name="total_nacional">Total (Nacional MXN)</param>
        /// <param name="tipo_cambio">Tipo de Cambio</param>
        /// <param name="lugar_expedicion">Lugar de Expedición (Código Postal)</param>
        /// <param name="id_direccion_lugar_expedicion">Dirección del Lugar de Exp.</param>
        /// <param name="confirmacion">No. de Confirmación del PAC</param>
        /// <param name="id_compania_emisor">Emisor del Comprobante</param>
        /// <param name="regimen_fiscal">Regimen Fiscal</param>
        /// <param name="id_sucursal">Sucursal</param>
        /// <param name="id_compania_receptor">Receptor del Comprobante</param>
        /// <param name="id_uso_cfdi_receptor">Uso del CFDI</param>
        /// <param name="fecha_captura">Fecha de Captura del >Comprobante</param>
        /// <param name="fecha_expedicion">Fecha de Expedición del Comprobante</param>
        /// <param name="fecha_cancelacion">Fecha de Cancelación del Comprobante</param>
        /// <param name="bit_generado">Indicador de Generación del Comprobante</param>
        /// <param name="bit_transferido_nuevo">Indicador de Nueva Transferencia</param>
        /// <param name="id_transferencia_nuevo">Identificador de Nueva Transferencia</param>
        /// <param name="bit_transferido_cancelar">Indicador de Transferencia Cancelada</param>
        /// <param name="id_transferencia_cancelar">Identificador de Transferencia Cancelada</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaComprobante(EstatusVigencia estatus_vigencia, int id_tipo_comprobante, int id_origen_datos,
                        int id_certificado, string version, string serie, string folio, string sello, int id_forma_pago, int id_metodo_pago,
                        string condicion_pago, int id_moneda, decimal subtotal_captura, decimal impuestos_captura, decimal descuentos_captura,
                        decimal total_captura, decimal subtotal_nacional, decimal impuestos_nacional, decimal descuentos_nacional,
                        decimal total_nacional, decimal tipo_cambio, string lugar_expedicion, int id_direccion_lugar_expedicion,
                        string confirmacion, int id_compania_emisor, string regimen_fiscal, int id_sucursal, int id_compania_receptor, 
                        int id_uso_cfdi_receptor, DateTime fecha_captura, DateTime fecha_expedicion,
                        DateTime fecha_cancelacion, bool bit_generado, bool bit_transferido_nuevo, int id_transferencia_nuevo,
                        bool bit_transferido_cancelar, int id_transferencia_cancelar, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaAtributosBD(estatus_vigencia, id_tipo_comprobante, id_origen_datos, id_certificado, version,
                               serie, folio, sello, id_forma_pago, id_metodo_pago, condicion_pago, id_moneda, subtotal_captura, impuestos_captura,
                               descuentos_captura, total_captura, subtotal_nacional, impuestos_nacional, descuentos_nacional, total_nacional,
                               tipo_cambio, lugar_expedicion, id_direccion_lugar_expedicion, confirmacion, id_compania_emisor, regimen_fiscal, id_sucursal,
                               id_compania_receptor, id_uso_cfdi_receptor, fecha_captura, fecha_expedicion, fecha_cancelacion, bit_generado, bit_transferido_nuevo,
                               id_transferencia_nuevo, bit_transferido_cancelar, id_transferencia_cancelar, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar el Origen del Comprobante
        /// </summary>
        /// <param name="origenDatos"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaOrigenComprobante(OrigenDatos origenDatos, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaAtributosBD((EstatusVigencia)this._id_estatus_vigencia, this._id_tipo_comprobante, (int)origenDatos, this._id_certificado, this._version,
                               this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                               this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                               this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                               this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, this._bit_generado, this._bit_transferido_nuevo,
                               this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Comprobantes en su versión 3.3
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaComprobante(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion result = new RetornoOperacion();
            
            //Si el comprobante no se ha timbrado
            if (!this._bit_generado)
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciamos Relacion Facturado facturacion
                    using (FacturadoFacturacion objFacturadoFacturacion = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFacturaElectronicav3_3(this._id_comprobante33)))
                    {
                        //Validamos que exista Relación
                        if (objFacturadoFacturacion.habilitar)
                        {
                            //Validamos el Origen de Datos
                            if ((OrigenDatos)this._id_origen_datos == OrigenDatos.Facturado)
                            {
                                //Deshablitamos Relación
                                result = objFacturadoFacturacion.DeshabilitaFacturadoFacturacion(id_usuario);
                            }
                            else if ((OrigenDatos)this._id_origen_datos == OrigenDatos.FacturaGlobal)
                            {
                                //Instanciamos Factura Global
                                using (FacturaGlobal objFacturaGlobal = new FacturaGlobal(objFacturadoFacturacion.id_factura_global))
                                {
                                    //Deshabilitamos Factura Global
                                    result = objFacturaGlobal.ReversaFacturaGlobal_V3_3(id_usuario);
                                }
                            }

                            //Validando Operaciones
                            if (result.OperacionExitosa)
                            {
                                //Obtiene Conceptos Comprobante
                                using (DataTable dtConceptos = Concepto.ObtieneConceptosComprobante(this._id_comprobante33))
                                {
                                    //Validando Conceptos
                                    if (Validacion.ValidaOrigenDatos(dtConceptos))
                                    {
                                        //Recorriendo Conceptos
                                        foreach (DataRow dr in dtConceptos.Rows)
                                        {
                                            //Instanciando Concepto
                                            using (Concepto con = new Concepto(Convert.ToInt32(dr["Id"])))
                                            {
                                                //Validando Concepto
                                                if (con.habilitar)
                                                {
                                                    //Deshabilitando Concepto
                                                    result = con.DeshabilitaConcepto(id_usuario);

                                                    //Validando Resultado
                                                    if (!result.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }
                                                else
                                                {
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No se puede recuperar el Concepto");
                                                    //Terminando Ciclo
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    //Validando Operaciones
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Impuesto del Comprobante
                                        using (Impuesto imp = Impuesto.ObtieneImpuestoComprobante(this._id_comprobante33))
                                        {
                                            //Validando Existencia
                                            if (imp.habilitar)
                                            
                                                //Deshabilitando Impuesto
                                                result = imp.DeshabilitaImpuesto(id_usuario);
                                        }
                                    }

                                    //Validando Operaciones
                                    if (result.OperacionExitosa)
                                    {
                                        //Devolviendo Resultado Obtenido
                                        result = this.actualizaAtributosBD((EstatusVigencia)this._id_estatus_vigencia, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                                                           this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                                                           this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                                                           this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                                                           this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, this._bit_generado, this._bit_transferido_nuevo,
                                                           this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, false);

                                        //Validamos Resultado
                                        if (result.OperacionExitosa)

                                            //Completamos Transacción
                                            scope.Complete();
                                    }
                                }
                            }
                        }
                        else
                            //Establecmos mensaje
                            result = new RetornoOperacion("No se puede recuperar datos complementarios Facturado Facturación.");
                    }
                }
            }

            else
                result = new RetornoOperacion("El comprobante ya se ha timbrado, no es posible editarlo.");

            //Devolvemos Resultado
            return result;
        }
        /// <summary>
        /// Método encargado de enviar el Email de Comprobante
        /// </summary>
        /// <param name="id_archivo_tipo_configuracion">Tipo de Archivo para la estructura de la Firma Electrónica</param>
        /// <param name="e_mail_remitente">E-mail remitente</param>
        /// <param name="asunto">Asunto</param>
        /// <param name="mensaje">Mensaje</param>
        /// <param name="errores">Errores generados</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EnviaEmail(int id_archivo_tipo_configuracion, string e_mail_remitente, string asunto, string mensaje, out List<string> errores, int id_usuario)
        {
            //Declaramos variable para almacenar los comprobantes
            string[] _comprobantes = { this._id_comprobante33.ToString() };
            errores = null;
            //Variable de Salida
            int descargasRest; DateTime fechaExp;
            //Declaramos objeto resultado
            RetornoOperacion result = new RetornoOperacion();
            //Validamos que la Fcatura se encuentre Timbrada
            if (this._bit_generado)
            {
                //Cargamos Contactos
                using (DataTable mit = Referencia.CargaReferencias(id_compania_receptor, 25, 2058))
                {
                    //Validamos Contactos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recorremos cada uno de los contactos
                        foreach (DataRow r in mit.Rows)
                        {
                            //Validamos Correo
                            if (Cadena.ValidaEmail(r.Field<string>("Valor")))
                            {
                                //Validamos Tipo de Archivo
                                switch (id_archivo_tipo_configuracion)
                                {
                                    case 18: //Descarga CFDI
                                        //Obteniendo Resultado
                                        result = SAT_CL.FacturacionElectronica.LinkDescarga.GeneraLinkDescarga(r.Field<int>("Id"), _comprobantes, out fechaExp, out descargasRest, this._id_compania_emisor, id_usuario);
                                        //Validamos Resultado
                                        if (result.OperacionExitosa)
                                        {
                                            //Intsanciamos Usuario
                                            using (SAT_CL.Seguridad.Usuario usuario = new Seguridad.Usuario(id_usuario))
                                            {
                                                /*/Creamos mensaje
                                                result = creaMensajeMailDescargaCFDI("", asunto, "", mensaje, "", usuario.nombre,
                                                         CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Página Descarga CFDI") + "?id=" + result.IdRegistro.ToString(),
                                                         "Para descargar los CFDI solicitados, siga el siguiente enlace:'{0}'",
                                                         fechaExp.ToString("dd/MM/yyyy"), fechaExp.ToString("HH:mm"), descargasRest.ToString(),
                                                         "El link de descarga caduca el día {0} a las {1} hrs.<br />○ Sólo puede utilizar {2} veces este link.<br />○ Si requiere realizar más descargas, pongase en contacto con nosotros.");//*/
                                            }
                                            //Validamos Resultado
                                            if (result.OperacionExitosa)
                                            {
                                                //Enviamos Email
                                                //Instanciando Correo Electronico
                                                using (Correo email = new Correo(e_mail_remitente, r.Field<string>("Valor"), asunto, result.Mensaje, true))
                                                {
                                                    //Enviando Correo Electronico
                                                    bool enviar = email.Enviar();

                                                    //Si no se envío el mensaje
                                                    if (!enviar)
                                                    {   //Recorriendo los errores del envío
                                                        foreach (string error in email.Errores)
                                                            //Añadiendo errores a la lista 
                                                            errores.Add(error + "<br />");
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                }

                            }
                        }
                    }
                    else
                    {
                        //Establecemos mensaje error
                        result = new RetornoOperacion("No hay contactos registrados");
                    }
                }
            }
            else
            {
                //Establecemos Error
                result = new RetornoOperacion("La factura aún no se ha timbrado.");
            }
            //Devolvemos Resultado
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos del Comprobante en su versión 3.3
        /// </summary>
        /// <returns></returns>
        public bool ActualizaComprobante()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_comprobante33);
        }
        /// <summary>
        /// Actualiza Subtotal de Comprobante
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaSubtotalComprobante(int id_usuario)
        {
            //Inicializamos Vaariables
            decimal SubtotalCaptura = 0.00M, SubtotalNacional = 0.00M, ImpuestosCaptura = 0.00M, ImpuestosNacional = 0.00M;
            
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obtiene Total Conceptos
                using (DataTable mit = Concepto.ObtieneConceptosComprobante(this._id_comprobante33))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Obtenemos Total  Conceptos
                        SubtotalCaptura = (from DataRow r in mit.Rows
                                           select Convert.ToDecimal(r["ImporteMonedaCaptura"])).Sum();
                        SubtotalNacional = (from DataRow r in mit.Rows
                                            select Convert.ToDecimal(r["ImporteMonedaNacional"])).Sum();

                        //Instanciando Retorno Positivo
                        resultado = new RetornoOperacion(this._id_comprobante33);
                    }
                }

                //Obtiene Total Conceptos
                using (DataTable mit = Comprobante.ObtieneTotalImpuestos(this._id_comprobante33))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Obtenemos Total  Conceptos
                        ImpuestosCaptura = (from DataRow r in mit.Rows
                                            select Convert.ToDecimal(r["TotalTrasladoCaptura"]) - Convert.ToDecimal(r["TotalRetenidoCaptura"])).Sum();


                        ImpuestosNacional = (from DataRow r in mit.Rows
                                             select Convert.ToDecimal(r["TotalTrasladoNacional"]) - Convert.ToDecimal(r["TotalRetenidoNacional"])).Sum();

                        //Instanciando Impuesto
                        using (Impuesto imp = Impuesto.ObtieneImpuestoComprobante(this._id_comprobante33))
                        {
                            //Validando Existencia
                            if (imp.habilitar)
                            
                                //Editando Impuesto
                                resultado = imp.EditaImpuesto(this._id_comprobante33, (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalTrasladoCaptura"])).Sum(),
                                                             (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalTrasladoNacional"])).Sum(),
                                                             (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalRetenidoCaptura"])).Sum(),
                                                             (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalRetenidoNacional"])).Sum(),
                                                             id_usuario);
                            else
                                //Insertando Impuesto
                                resultado = Impuesto.InsertaImpuesto(this._id_comprobante33, (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalTrasladoCaptura"])).Sum(),
                                                                    (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalTrasladoNacional"])).Sum(),
                                                                    (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalRetenidoCaptura"])).Sum(),
                                                                    (from DataRow r in mit.Rows select Convert.ToDecimal(r["TotalRetenidoNacional"])).Sum(),
                                                                    id_usuario);
                        }
                    }
                }
                
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Editamos Combrobante
                    resultado = this.actualizaAtributosBD(EstatusVigencia.Vigente, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                            this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda,
                            SubtotalCaptura, ImpuestosCaptura, this._descuentos_captura, this._total_captura, SubtotalNacional, ImpuestosNacional,
                            this._descuentos_nacional, this._total_nacional, this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion,
                            this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal, this._id_compania_receptor, this._id_uso_receptor,
                            this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, this._bit_generado, this._bit_transferido_nuevo, this._id_transferencia_nuevo,
                            this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Finalizamos transacción
                        scope.Complete();
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Actualizamos Descuento
        /// </summary>
        /// <param name="descuento_moneda_captura"></param>
        /// <param name="descuento_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaDescuento(decimal descuento_moneda_captura, decimal descuento_moneda_nacional, int id_usuario)
        {

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Editamos Combrobante
            resultado = this.actualizaAtributosBD(EstatusVigencia.Vigente, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                    this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda,
                    this._subtotal_captura, this._impuestos_captura, descuento_moneda_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional,
                    descuento_moneda_nacional, this._total_nacional, this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion,
                    this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal, this._id_compania_receptor, this._id_uso_receptor,
                    this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, this._bit_generado, this._bit_transferido_nuevo, this._id_transferencia_nuevo,
                    this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);

            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_cliente"></param>
        /// <param name="comprobante"></param>
        /// <param name="conceptos"></param>
        /// <param name="descuentos"></param>
        /// <param name="impuestos_conceptos"></param>
        /// <param name="referencias"></param>
        /// <param name="referencias_conceptos"></param>
        /// <param name="origen_datos"></param>
        /// <param name="referencias_impresion_comprobante"></param>
        /// <param name="referencias_impresion_concepto"></param>
        /// <param name="id_compania_uso"></param>
        /// <param name="id_sucursal"></param>
        /// <param name="impresion_detalle"></param>
        /// <param name="fecha_cambio"></param>
        /// <param name="fecha"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion ImportaComprobante_V3_3(int id_cliente, DataTable comprobante, DataTable conceptos, DataTable descuentos, DataTable impuestos_conceptos,
                                                       DataTable referencias, DataTable referencias_conceptos, OrigenDatos origen_datos, DataTable referencias_impresion_comprobante,
                                                       DataTable referencias_impresion_concepto, int id_compania_uso, int id_sucursal, bool impresion_detalle, DateTime fecha, DateTime fecha_cambio, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando origen de datos
            if (Validacion.ValidaOrigenDatos(comprobante))
            {
                //Realizando lectura de encabezado de comprobante
                foreach (DataRow c in comprobante.Rows)
                {
                    //Instanciando emisor del comprobante
                    using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(id_compania_uso))
                    {
                        //Si el smisor existe
                        if (em.id_compania_emisor_receptor > 0)
                        {
                            //Instancinado ubicación del emisor para obtener lugar de expedición
                            using (Direccion le = new Direccion(em.id_direccion))
                            {
                                //si la ubicación no existe
                                if (le.id_direccion > 0)
                                {
                                    //Instanciando receptor
                                    using (CompaniaEmisorReceptor re = new CompaniaEmisorReceptor(id_cliente))
                                    {
                                        //Si existe el receptor
                                        if (re.id_compania_emisor_receptor > 0)
                                        {
                                            //Instanciando receptor
                                            using (Sucursal su = new Sucursal(id_sucursal))
                                            {
                                                //Instanciamos dirección sucursal
                                                using (Direccion direccionS = new Direccion(su.id_direccion))
                                                {
                                                    //Creamos la transacción 
                                                    using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                    {
                                                        //Obteniendo Regimen Fiscal
                                                        string reg_fis = SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3197, em.id_regimen_fiscal);

                                                        //Realizando la inserción correspondiente
                                                        resultado = Comprobante.InsertaComprobante(Convert.ToInt32(c["Id_Tipo_Comprobante"]), (byte)origen_datos, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Versión CFDI"), Convert.ToByte(c["Id_Forma_Pago"]),
                                                            Convert.ToByte(c["Id_Metodo_Pago"]), c["Condiciones_Pago"].ToString(), Convert.ToInt32(c["Id_Moneda"]), Convert.ToDecimal(c["Tipo_Cambio"]), le.codigo_postal, le.id_direccion, em.id_compania_emisor_receptor, reg_fis,
                                                            su.id_sucursal, re.id_compania_emisor_receptor, Convert.ToInt32(c["Id_Uso_CFDI"]), fecha, id_usuario);

                                                        //Guardando Id de registro
                                                        int id_comprobante = resultado.IdRegistro;

                                                        //Si se registró correctamente el encabezado
                                                        if (resultado.OperacionExitosa)
                                                            resultado = importaConceptosComprobante_V3_3(conceptos, impuestos_conceptos, referencias_conceptos, referencias_impresion_concepto, id_comprobante, impresion_detalle, id_usuario);

                                                        //Registrando Descuentos aplicables en caso de no encontrar errores
                                                        if (resultado.OperacionExitosa)
                                                            resultado = importaDescuentosComprobante_V3_3(descuentos, id_comprobante, id_usuario);

                                                        //Si no hay errores
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Armando Refencia de Concepto para Impreción de Comprobante
                                                            AgregaReferenciaComprobante(referencias_impresion_comprobante, referencias, 1, 209, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Referencias", 0, "General"), true);

                                                            //Validando existencia de referencias
                                                            if (Validacion.ValidaOrigenDatos(referencias))
                                                            {
                                                                //Para cada una de las referencias encontradas
                                                                foreach (DataRow r in referencias.Rows)
                                                                {
                                                                    //Validamos el Origen del Concepto
                                                                    if (1 == Convert.ToInt32(r["id_concepto_origen"]))
                                                                    {
                                                                        //realzaindo la inserción correspondiente
                                                                        resultado = Referencia.InsertaReferencia(id_comprobante, Convert.ToInt32(r["id_tabla"]), Convert.ToInt32(r["id_tipo"]), r["valor"].ToString(),
                                                                                                        Convert.ToDateTime(r["fecha"]), id_usuario, true);
                                                                    }
                                                                    //Si existe algún error
                                                                    if (!resultado.OperacionExitosa)
                                                                    {
                                                                        resultado = new RetornoOperacion(string.Format("Error al registrar Referencia '{0}'; {1}", r["valor"], resultado.Mensaje));
                                                                        break;
                                                                    }

                                                                }
                                                            }
                                                        }

                                                        //Si no hay errores
                                                        if (resultado.OperacionExitosa && id_comprobante > 0)
                                                        {
                                                            //reasignando Id de Importaciónde COmprobante como resultado
                                                            resultado = new RetornoOperacion(id_comprobante);

                                                            //Finalizamos Transaccion
                                                            scope.Complete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            resultado = new RetornoOperacion("El receptor no se encuentra registrado en la aplicación");
                                    }
                                }
                                else
                                    resultado = new RetornoOperacion("El lugar de expedición no fue recuperado.");
                            }
                        }
                        else
                            resultado = new RetornoOperacion("El emisor no se encuentra registrado en la aplicación.");
                    }

                    //Terminando ciclo de importación de comprobante
                    break;
                }
            }
            else
                resultado = new RetornoOperacion("No se encontró comprobante por registrar.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="serie"></param>
        /// <param name="id_usuario"></param>
        /// <param name="ruta_xslt_co33"></param>
        /// <param name="ruta_xslt_co_local33"></param>
        /// <param name="omitir_addenda"></param>
        /// <returns></returns>
        public RetornoOperacion TimbraComprobante(string version, string serie, int id_usuario, string ruta_xslt_co33, string ruta_xslt_co_local33, bool omitir_addenda)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            DateTime fecha_mexico = Fecha.ObtieneFechaEstandarMexicoCentro();

            //Validando que el Comprobante no este generado
            if (!this._bit_generado)
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Declarando variables auxiliares
                    int id_certificado_activo = 0;
                    string folio = "", numero_certificado = "", certificado_base_64 = "", contrasena_certificado = "";
                    byte[] bytes_certificado = null;
                    serie = serie.ToUpper();

                    //Validando Requisitos Previos
                    result = validaRequisitosPreviosTimbrado(serie, out folio, out id_certificado_activo, out numero_certificado,
                                                      out certificado_base_64, out contrasena_certificado, out bytes_certificado,
                                                      SerieFolioCFDI.TipoSerieFolioCFDI.FacturacionElectronica);

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Asignando Folio al Comprobante
                        result = asignaFolioComprobante(version, serie, folio, id_certificado_activo, id_usuario);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Actualizando Atributos
                            if (this.cargaAtributosInstancia(this._id_comprobante33))
                            {
                                //Instanciando al emisor
                                using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                                {
                                    //Validando Emisor
                                    if (em.habilitar)
                                    {
                                        //Creando ruta de guardado del comprobante
                                        string ruta_xml = string.Format(@"{0}{1}\{2}\CFDI_3_3\{3}{4}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 209.ToString("0000"),
                                                                                  em.DirectorioAlmacenamiento, serie, folio);

                                        //Eliminando archivo si es que ya existe
                                        Archivo.EliminaArchivo(ruta_xml);

                                        /**** Declaración de namespaces a utilizar en el Comprobante ****/
                                        //SAT
                                        XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT");
                                        //W3C
                                        XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C");
                                        //Inicialziando el valor de schemaLocation del cfd
                                        string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFDI 3.3");

                                        //Creando Documento xml inicial y configurando la declaración de XML
                                        XDocument documento = new XDocument();
                                        documento.Document.Declaration = new XDeclaration("1.0", "utf-8", "");

                                        //Obteniendo Comprobante en Formato XML
                                        documento = ComprobanteXML.CargaElementosArmadoComprobante3_3(this._id_comprobante33, ns, nsW3C, schemaLocation);

                                        //Validando que exista el Comprobante
                                        if (documento != null)
                                        {
                                            //Si no se ha omitido addenda/complementos
                                            if (!omitir_addenda)
                                                //Añadir complemento en caso necesario
                                                result = anadeElementoComplementoComprobante(ref documento, ns, id_usuario);

                                            //Si no hubo errores
                                            if (result.OperacionExitosa)
                                            {
                                                /**** Validando Acentos por Receptor ****/
                                                //Instanciando al receptor
                                                using (CompaniaEmisorReceptor rec = new CompaniaEmisorReceptor(this._id_compania_receptor))
                                                {
                                                    //Validamos que exista referencia
                                                    if (rec.habilitar && rec.FacturacionElectronica33 != null)
                                                    {
                                                        //Validamos que exista la Clave
                                                        if (rec.FacturacionElectronica33.ContainsKey("Acepta Acentos Comptrobante"))
                                                        {
                                                            //Si es necesario realizar supresión de acentos en comprobante
                                                            if (!Convert.ToBoolean(rec.FacturacionElectronica33["Acepta Acentos Comptrobante"]))
                                                                documento = XDocument.Parse(suprimeCaracteresAcentuadosCFD(documento.ToString()));
                                                        }
                                                    }
                                                }

                                                //Asignando Valores
                                                documento.Root.SetAttributeValue("NoCertificado", numero_certificado);
                                                documento.Root.Attribute("Fecha").Value = fecha_mexico.AddMinutes(-2).ToString("yyyy-MM-ddTHH:mm:ss");

                                                //Definiendo bytes de XML
                                                byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                //Verificando que se haya devuelto los bytes del XDocument
                                                if (!Conversion.EsArrayVacio(bytes_comprobante))
                                                {
                                                    //Definiendo variable para almacenar cadena orignal
                                                    string cadena_original = "";

                                                    //Realizando transformación de cadena original
                                                    result = FEv32.SelloDigitalSAT.CadenaCFD(new System.IO.MemoryStream(bytes_comprobante), ruta_xslt_co33, ruta_xslt_co_local33, out cadena_original);
                                                    //result = FEv32.SelloDigitalSAT.CadenaCFD(new System.IO.MemoryStream(bytes_comprobante), ruta_xslt_co_local33, ruta_xslt_co33, out cadena_original);

                                                    //Validando Operación Exitosa
                                                    if (result.OperacionExitosa && !cadena_original.Equals(""))
                                                    {
                                                        //Codificando Cadena Original a UTF-8
                                                        byte[] co_utf_8 = FEv32.SelloDigitalSAT.CodificacionUTF8(cadena_original);

                                                        //Realizando sellado del Comprobante
                                                        string sello_digital = FEv32.SelloDigitalSAT.FirmaCadenaSHA256(co_utf_8, bytes_certificado, contrasena_certificado);

                                                        //Si el sello digital fue generado correctamente
                                                        if (sello_digital != "")
                                                        {
                                                            //Actualizando el sello digital en BD y Marcando como 'Generado'
                                                            result = actualizaSelloDigital(sello_digital, id_usuario);

                                                            //Si se actualiza correctamente
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Actualizando datos de sello digital
                                                                if (cargaAtributosInstancia(this._id_comprobante33))
                                                                {
                                                                    //Actualizando Sello y número de certificado en XML
                                                                    documento.Root.SetAttributeValue("Certificado", certificado_base_64);
                                                                    documento.Root.SetAttributeValue("Sello", sello_digital);

                                                                    //Conectando a Web Service del Proveedor de Timbre Fiscal y generando dicho registro
                                                                    result = generaTimbreFiscalDigital(ref documento, ns, id_usuario);

                                                                    //Validando Operación Exitosa
                                                                    if (result.OperacionExitosa)
                                                                    {
                                                                        //Validando si se Omitira la Addenda
                                                                        if (!omitir_addenda)

                                                                            //Si se ha solicitado añadir elemento complemento
                                                                            result = anadeElementoAddendaComprobante(ref documento, ns, id_usuario);
                                                                        else
                                                                            //Registrando petición de timbrado sin addenda
                                                                            result = Bitacora.InsertaBitacora(209, this._id_comprobante33, 8886, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                        //Validando Operación
                                                                        if (result.OperacionExitosa)
                                                                        {
                                                                            //Actualizando contenido de documento a arreglo de bytes
                                                                            bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                                            //Guardando archivo en unidad de almacenamiento
                                                                            result = ArchivoRegistro.InsertaArchivoRegistro(209, this._id_comprobante33, 22, "", id_usuario, bytes_comprobante, ruta_xml);

                                                                            //Validmos Resultado
                                                                            if (result.OperacionExitosa)

                                                                                //Generamos Barra Bidimensional
                                                                                result = this.generaCodigoBidimensionalComprobante(id_usuario);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("Error, sello digital en blanco.");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Validando Operaciones Exitosas
                    if (result.OperacionExitosa)
                    {
                        //Asignando Comprobante
                        result = new RetornoOperacion(this._id_comprobante33);

                        //Completando Transacción
                        scope.Complete();
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener el Total de los Impuestos
        /// </summary>
        /// <param name="id_comprobante33"></param>
        /// <returns></returns>
        public static DataTable ObtieneTotalImpuestos(int id_comprobante33)
        {
            //Declarando Objeto de Retorno
            DataTable dtImpuestos = null;

            //Declarando Arreglo de Parametros
            object[] param = { 5, id_comprobante33, 0, 0, 0, 0, "", "", "", "", 0, 0, "", 0, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 
                               0.00M, 0.00M, "", 0, "", 0, "", 0, 0, 0, null, null, null, false, false, 0, false, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores
                    dtImpuestos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtImpuestos;
        }
        /// <summary>
        ///  Realiza la importación de un comprobante a partir del esquema valido ya definido
        /// </summary>
        /// <param name="comprobante"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_sucursal"></param>
        /// <param name="conceptos"></param>
        /// <param name="referencias_conceptos"></param>
        /// <param name="descuentos"></param>
        /// <param name="impuestos"></param>
        /// <param name="impuestos_conceptos"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion ImportaReciboNominaActualizacion_V3_3(DataTable comprobante, int id_compania, int id_sucursal, DataTable conceptos, DataTable referencias_conceptos, DataTable descuentos, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(comprobante))
                {
                    //Realizando lectura de encabezado de comprobante
                    foreach (DataRow c in comprobante.Rows)
                    {
                        //Instanciando emisor del comprobante
                        using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(id_compania))
                        {
                            //Si el smisor existe
                            if (em.id_compania_emisor_receptor > 0)
                            {
                                //Instancinado ubicación del emisor para obtener lugar de expedición
                                using (Direccion le = new Direccion(em.id_direccion))
                                {
                                    //si la ubicación no existe
                                    if (le.id_direccion > 0)
                                    {
                                        //Instanciamos Sucursal
                                        using (Sucursal objSucursal = new Sucursal(id_sucursal))
                                        {
                                            //Instancinado ubicación del emisor para obtener lugar de expedición
                                            using (Direccion leS = new Direccion(objSucursal.id_direccion))
                                            {
                                                //Obteniendo Regimen Fiscal
                                                string reg_fis = SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3197, em.id_regimen_fiscal);

                                                //Realizando la inserción correspondiente
                                                resultado = Comprobante.InsertaComprobante(Convert.ToInt32(c["Id_Tipo_Comprobante"]), (byte)Comprobante.OrigenDatos.ReciboNomina, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Versión CFDI"), Convert.ToByte(c["Id_Forma_Pago"]),
                                                                                Convert.ToByte(c["Id_Metodo_Pago"]), c["Condiciones_Pago"].ToString(), Convert.ToInt32(c["Id_Moneda"]), 0.00M, le.codigo_postal, le.id_direccion, em.id_compania_emisor_receptor, reg_fis,
                                                                                objSucursal.id_sucursal, 0, Convert.ToInt32(c["Id_Uso_CFDI"]), DateTime.MinValue, id_usuario);

                                                //Guardando Id de registro
                                                int id_comprobante = resultado.IdRegistro;

                                                //Si se registró correctamente el encabezado
                                                if (resultado.OperacionExitosa)
                                                    resultado = importaConceptosComprobante_V3_3(conceptos, null, referencias_conceptos, null, id_comprobante, false, id_usuario);

                                                //Registrando Descuentos aplicables en caso de no encontrar errores
                                                if (resultado.OperacionExitosa)
                                                    resultado = importaDescuentosComprobante_V3_3(descuentos, id_comprobante, id_usuario);

                                                //Si no hay errores
                                                if (resultado.OperacionExitosa && id_comprobante > 0)
                                                {
                                                    //reasignando Id de Importaciónde COmprobante como resultado
                                                    resultado = new RetornoOperacion(id_comprobante);
                                                    //Finalizamos transacción
                                                    scope.Complete();
                                                }
                                            }
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("El lugar de expedición no fue recuperado.");
                                }
                            }
                            else
                                resultado = new RetornoOperacion("El emisor no se encuentra registrado en la aplicación.");
                        }

                        //Terminando ciclo de importación de comprobante
                        break;
                    }
                }
                else
                    resultado = new RetornoOperacion("No se encontró comprobante por registrar.");
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realzia el timbrado del comprobante, creando XML del mismo, Sellandolo y Concetandose con el proveedor de Timbrado
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="id_usuario"></param>
        /// <param name="ruta_xslt_co"></param>
        /// <param name="ruta_xslt_co_local"></param>
        /// <param name="omitir_addenda"></param>
        /// <param name="nomina"></param>
        /// <param name="percepion"></param>
        /// <param name="percepciones"></param>
        /// <param name="deduccion"></param>
        /// <param name="deducciones"></param>
        /// <param name="incapacidad"></param>
        /// <param name="horasextra"></param>
        /// <returns></returns>
        public RetornoOperacion TimbraReciboNomina_V3_3(string version_nomina, string serie, int id_usuario, string ruta_xslt_co, string ruta_xslt_co_local, int id_nomina_empleado, bool omitir_addenda)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando variables auxiliares
                int id_certificado_activo = 0;
                string numero_certificado = "", certificado_base_64 = "", contrasena_certificado = "", folio = "";
                byte[] bytes_certificado = null;
                serie = serie.ToUpper();

                //Validando requisitos previos a la genracióndel CFD (Serie solicitada existente y folio disponible, Timbrado previo, Emisor activo con certificado vigente y Addendas)
                resultado = validaRequisitosPreviosTimbradoNomina(serie, out folio, out id_certificado_activo, out numero_certificado, out certificado_base_64, out contrasena_certificado, out bytes_certificado, SerieFolioCFDI.TipoSerieFolioCFDI.ReciboNomina);

                //Si cuenta con lo requerido para ser timbrado
                if (resultado.OperacionExitosa)
                {
                    //Actualizando Serie, Folio y Certificado utilizadso para sellado del comprobante
                    resultado = asignaFolioComprobante(VERSION_CFDI, serie, folio, id_certificado_activo, id_usuario);

                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recargando contenido de instancia
                        if (cargaAtributosInstancia(this._id_comprobante33))
                        {
                            //Instanciando al emisor
                            using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                            {
                                //Creando ruta de guardado del comprobante
                                string ruta_xml = string.Format(@"{0}{1}\{2}\CFDI_3_3\{3}{4}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 209.ToString("0000"),
                                                                          em.DirectorioAlmacenamiento, serie, folio);

                                //Eliminando archivo si es que ya existe
                                Archivo.EliminaArchivo(ruta_xml);

                                //Declaración de namespaces a utilizar en el Comprobante
                                //SAT
                                XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT", 0);
                                //Declaración de namespaces a utilizar en el Comprobante
                                //SAT Nomina 1.2
                                XNamespace nsn = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT Nomina1.2", 0);
                                //W3C
                                XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C", 0);
                                //Esquema NOMIANA
                                XNamespace wn12 = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace WN12 ", 0);
                                //Inicialziando el valor de schemaLocation del cfd
                                string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation Nomina 1.2 CFDI 3.3");

                                //Creando Documento xml inicial y configurando la declaración de XML
                                XDocument documento = new XDocument();
                                documento.Declaration = new XDeclaration("1.0", "utf-8", "");

                                //Declaramos Objeto Resultato para validar Complemento de Nomina
                                RetornoOperacion resultadocomplementonomina = new RetornoOperacion();

                                //Cargando Elementos del Armado
                                XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobanteReciboNomina_V_3_3(version_nomina, 
                                                            id_nomina_empleado, this._id_comprobante33, ns, nsn, nsW3C, wn12, schemaLocation, 
                                                            this._id_compania_emisor, this._id_compania_receptor, id_usuario, out resultadocomplementonomina);


                                //Validamos  que no exista errores en la creación del Complemento de Nómina
                                if (resultadocomplementonomina.OperacionExitosa)
                                {
                                    //Si no se ha requerido addenda/complementos
                                    if (!omitir_addenda)
                                        //Añadir addenda en caso necesario
                                        resultado = anadeElementoComplementoReciboNominaV3_3("1.2", ref comprobante, ns);

                                    //Si no hubo errores
                                    if (resultado.OperacionExitosa)
                                        //Validando sólo para fines internos, que existan addendas
                                        resultado = validaAddendasCapturadas();

                                    //Si no hubo errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Añadiendo contenido del comprobante
                                        documento.Add(comprobante);

                                        //Asignando Valores
                                        documento.Root.SetAttributeValue("NoCertificado", numero_certificado);

                                        //Definiendo bytes de XML
                                        byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                        //Verificando que se haya devuelto los bytes del XDocument
                                        if (!Conversion.EsArrayVacio(bytes_comprobante))
                                        {
                                            //Definiendo variable para almacenar cadena orignal
                                            string cadena_original = "";

                                            //Realizando transformación de cadena original
                                            resultado = FEv32.SelloDigitalSAT.CadenaCFD(new System.IO.MemoryStream(bytes_comprobante), ruta_xslt_co, ruta_xslt_co_local, out cadena_original);

                                            //Si no hay errores
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Verificando Contenido de Cadena Original
                                                if (cadena_original != "")
                                                {
                                                    //Codificando Cadena Original a UTF-8
                                                    byte[] co_utf_8 = FEv32.SelloDigitalSAT.CodificacionUTF8(cadena_original);
                                                    //Realizando sellado del Comprobante
                                                    string sello_digital = FEv32.SelloDigitalSAT.FirmaCadenaSHA256(co_utf_8, bytes_certificado, contrasena_certificado);

                                                    //Si el sello digital fue generado correctamente
                                                    if (sello_digital != "")
                                                    {
                                                        //Actualizando el sello digital en BD y Marcando como 'Generado'
                                                        resultado = actualizaSelloDigital(sello_digital, id_usuario);

                                                        //Si se actualiza correctamente
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizando datos de sello digital
                                                            if (cargaAtributosInstancia(this._id_comprobante33))
                                                            {
                                                                //Actualizando Sello y número de certificado en XML
                                                                documento.Root.SetAttributeValue("Certificado", certificado_base_64);
                                                                documento.Root.SetAttributeValue("Sello", sello_digital);

                                                                //Conectando a Web Service del Proveedor de Timbre Fiscal y generando dicho registro
                                                                resultado = generaTimbreFiscalDigital(ref documento, ns, id_usuario);

                                                                //Si no existen errores en timbrado
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Validando si se Omitira la Addenda
                                                                    if (!omitir_addenda)

                                                                        //Si se ha solicitado añadir elemento complemento
                                                                        resultado = anadeElementoAddendaComprobante(ref documento, ns, id_usuario);
                                                                    else
                                                                        //Registrando petición de timbrado sin addenda
                                                                        resultado = Bitacora.InsertaBitacora(209, this._id_comprobante33, 8886, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                    //Validando Operación
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Actualizando contenido de documento a arreglo de bytes
                                                                        bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                                        //Guardando archivo en unidad de almacenamiento
                                                                        resultado = ArchivoRegistro.InsertaArchivoRegistro(209, this._id_comprobante33, 22, "", id_usuario, bytes_comprobante, ruta_xml);

                                                                        //Validmos Resultado
                                                                        if (resultado.OperacionExitosa)

                                                                            //Generamos Barra Bidimensional
                                                                            resultado = this.generaCodigoBidimensionalComprobante(id_usuario);
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion("Error al crear addenda.");
                                                                }
                                                            }
                                                            else
                                                                resultado = new RetornoOperacion("Error al recuperar datos actualizados después de sellado.");
                                                        }
                                                        else
                                                            resultado = new RetornoOperacion("Error al actualizar sello digital.");
                                                    }
                                                    else
                                                        resultado = new RetornoOperacion("Error, sello digital en blanco.");
                                                }
                                                else
                                                    resultado = new RetornoOperacion("Error al crear cadena original. Existen problemas de conexión al sitio de internet, intentelo más tarde.");
                                            }
                                            else
                                                resultado = new RetornoOperacion("Error al crear cadena original.");
                                        }
                                        else
                                            resultado = new RetornoOperacion("Error al leer bytes del xml construido.");
                                    }
                                }
                                else
                                {
                                    resultado = new RetornoOperacion(resultadocomplementonomina.Mensaje);
                                }
                            }
                        }
                        else
                            resultado = new RetornoOperacion("Error al recuperar datos actualizados después de asignación de folio.");
                    }
                    else
                        resultado = new RetornoOperacion("Error al actualizar folio del comprobante.");
                }

                //Validando Operaciones Exitosas
                if (resultado.OperacionExitosa)
                {
                    //Asignando Comprobante
                    resultado = new RetornoOperacion(this._id_comprobante33);

                    //Completando Transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Colocar el Comprobante como Pendiente de Cancelación
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion CancelacionPendiente(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Si el comprobante está generado y no ha sido cancelado aún
            if (this._bit_generado && this.id_estatus_vigencia == (byte)EstatusVigencia.Vigente)
            {
                //Inicializando Bloque
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validando Comprobante de Pago
                    if (!ComprobantePagoDocumentoRelacionado.ValidaComprobantePagoCxC(this._id_comprobante33))
                    {
                        //Validando Nota de Credito
                        retorno = Facturacion.FacturadoEgresoRelacion.ValidaNotaCreditoCFDI(this._id_comprobante33);

                        //Validando Operación
                        if (!retorno.OperacionExitosa)
                        {
                            //Inicializando parametros
                            retorno = this.actualizaAtributosBD(EstatusVigencia.PendienteCancelacion, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                                           this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                                           this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                                           this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                                           this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, DateTime.MinValue,
                                           this._bit_generado, this._bit_transferido_nuevo, this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);

                            //Validando Operación
                            if (retorno.OperacionExitosa)
                            {
                                //Obteniendo Origen del Comprobante
                                OrigenDatos origen; int id_registro = 0;
                                retorno = ObtieneOrigenDatosComprobante(this._id_comprobante33, out origen, out id_registro);

                                //Validando Operación
                                if (retorno.OperacionExitosa)
                                {
                                    //Cancelando CFDI
                                    CancelacionCDFI.EstatusUUID estatusUUID = CancelacionCDFI.EstatusUUID.SinEstatus;
                                    CancelacionCDFI.TipoCancelacion tipo = CancelacionCDFI.TipoCancelacion.SinAsignar;
                                    XDocument acuse = new XDocument();
                                    retorno = CancelacionCDFI.objCancelacion.CancelacionComprobanteCxC(this._id_comprobante33, out estatusUUID, out tipo, out acuse, id_usuario);

                                    //Validando Operación
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Instanciando Retorno Positivo
                                        retorno = new RetornoOperacion(this._id_comprobante33);
                                        //Completando Transaccion
                                        scope.Complete();
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Retorno
                            retorno = new RetornoOperacion(retorno.Mensaje);
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El Comprobante está relacionado a un CFDI de Recepción de Pagos, imposible su cancelación.");
                }
            }
            else
            {   
                //Validando Estatus
                switch ((EstatusVigencia)this.id_estatus_vigencia)
                {
                    case EstatusVigencia.Cancelado:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El comprobante ya se ha cancelado con aterioridad.");
                            break;
                        }
                    case EstatusVigencia.PendienteCancelacion:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El comprobante se encuentra Pendiente de Cancelación.");
                            break;
                        }
                }
            }

            //Devovliendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion CancelacionRechazada(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            if (this._bit_generado && this.id_estatus_vigencia == (byte)EstatusVigencia.PendienteCancelacion)
            {
                //Validando Comprobante de Pago
                if (!ComprobantePagoDocumentoRelacionado.ValidaComprobantePagoCxC(this._id_comprobante33))
                {
                    //Validando Nota de Credito
                    retorno = Facturacion.FacturadoEgresoRelacion.ValidaNotaCreditoCFDI(this._id_comprobante33);

                    //Validando Operación
                    if (!retorno.OperacionExitosa)

                        ////Inicializando Bloque
                        //using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        //{
                        //Inicializando parametros
                        retorno = this.actualizaAtributosBD(EstatusVigencia.Vigente, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                                       this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                                       this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                                       this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                                       this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, DateTime.MinValue,
                                       this._bit_generado, this._bit_transferido_nuevo, this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);

                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion(retorno.Mensaje);

                        ////Validando Operación
                        //if (retorno.OperacionExitosa)
                        //{
                        //    //Obteniendo Timbre Fiscal
                        //    using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante33))
                        //    //Instanciando Detalle del Acuse
                        //    using (AcuseCancelacionDetalle detalle = AcuseCancelacionDetalle.ObtieneAcuseTimbre(tfd.id_timbre_fiscal_digital))
                        //    {
                        //        //Validando detalle
                        //        if (tfd.habilitar)
                        //        {
                        //            //Validando detalle
                        //            if (detalle.habilitar_tacd)
                        //            {
                        //                //Actualizando Estatus
                        //                retorno = detalle.ActualizaEstatusDetalle(AcuseCancelacionDetalle.EstatusCancelacion.Rechazado, id_usuario);
                        //            }
                        //            else
                        //                //Instanciando Excepción
                        //                retorno = new RetornoOperacion("No se puede recuperar el detalle del Acuse");
                        //        }
                        //        else
                        //            //Instanciando Excepción
                        //            retorno = new RetornoOperacion("No se puede recuperar el Timbre Fiscal Digital");
                        //    }

                        //    //Validando Operación
                        //    if (retorno.OperacionExitosa)
                        //    {
                        //        //Instanciando CFDI
                        //        retorno = new RetornoOperacion(this._id_comprobante33);
                        //        //Completando Transacción
                        //        scope.Complete();
                        //    }
                        //}
                    //}
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("El Comprobante está relacionado a un CFDI de Recepción de Pagos, imposible su cancelación.");
            }
            else
            {
                //Validando Estatus
                switch ((EstatusVigencia)this.id_estatus_vigencia)
                {
                    case EstatusVigencia.Cancelado:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El comprobante ya se ha cancelado con aterioridad.");
                            break;
                        }
                    case EstatusVigencia.Vigente:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El comprobante se encuentra vigente.");
                            break;
                        }
                }
            }

            //Devovliendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        [Obsolete]
        public RetornoOperacion CancelacionRechazadaABC(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Si el comprobante está generado y no ha sido cancelado aún
            if (this._bit_generado && this.id_estatus_vigencia == (byte)EstatusVigencia.PendienteCancelacion)
            {
                //Validando Comprobante de Pago
                if (!ComprobantePagoDocumentoRelacionado.ValidaComprobantePagoCxC(this._id_comprobante33))
                {
                    //Inicializando Bloque
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Inicializando parametros
                        retorno = this.actualizaAtributosBD(EstatusVigencia.Vigente, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                                       this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                                       this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                                       this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                                       this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, DateTime.MinValue,
                                       this._bit_generado, this._bit_transferido_nuevo, this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);

                        //Validando Operación
                        if (retorno.OperacionExitosa)
                        {
                            //Obteniendo Timbre Fiscal
                            using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante33))
                            //Instanciando Detalle del Acuse
                            using (AcuseCancelacionDetalle detalle = AcuseCancelacionDetalle.ObtieneAcuseTimbre(tfd.id_timbre_fiscal_digital))
                            {
                                //Validando TFD
                                if (tfd.habilitar && detalle.habilitar_tacd)
                                {
                                    //Actualizando Estatus
                                    retorno = detalle.ActualizaEstatusDetalle(AcuseCancelacionDetalle.EstatusCancelacion.Rechazado, id_usuario);

                                    //Validando Operación
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Validando origen
                                        if ((OrigenDatos)this._id_origen_datos == OrigenDatos.ReciboPagoCliente)
                                        {
                                            //Recuperando relaciones de pagos del CFDI previo para su actualización a cancelado
                                            using (DataTable mitPagosCFDI = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(this._id_comprobante33))
                                            {
                                                //Si hay elementos por actualizar
                                                if (mitPagosCFDI != null)
                                                {
                                                    //Para cada elemento
                                                    foreach (DataRow p in mitPagosCFDI.Rows)
                                                    {
                                                        //Instanciando Relación
                                                        using (EgresoIngresoComprobante eic = new EgresoIngresoComprobante(p.Field<int>("IdEgresoIngresoComp")))
                                                        {
                                                            //Si se localizó la relación
                                                            if (eic.habilitar)

                                                                //Actualizando estatus de relación
                                                                retorno = eic.ActualizaEstatusEgresoIngresoComprobante(EgresoIngresoComprobante.Estatus.Timbrado, id_usuario);
                                                            else
                                                                //Instanciando Excepción
                                                                retorno = new RetornoOperacion(string.Format("Error al Recuperar Relación Pagos - CFDI (Para sustitución) ID: '{0}'", p.Field<int>("IdEgresoIngresoComp")));

                                                            //Si hay errores se interrumpe ciclo
                                                            if (!retorno.OperacionExitosa)
                                                                break;
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    retorno = new RetornoOperacion("Error al Recuperar Pagos de CFDI Sustituido para su Actualización a Cancelado.");
                                            }
                                        }

                                        //Validando Operación
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Instanciando CFDI
                                            retorno = new RetornoOperacion(this._id_comprobante33);
                                            //Completando Transacción
                                            scope.Complete();
                                        }
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    retorno = new RetornoOperacion("No se puede recuperar el Timbre Fiscal Digital");
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("El Comprobante está relacionado a un CFDI de Recepción de Pagos, imposible su cancelación.");
            }
            else
            {
                //Validando Estatus
                switch ((EstatusVigencia)this.id_estatus_vigencia)
                {
                    case EstatusVigencia.Cancelado:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El comprobante ya se ha cancelado con aterioridad.");
                            break;
                        }
                    case EstatusVigencia.Vigente:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El comprobante se encuentra vigente.");
                            break;
                        }
                }
            }

            //Devovliendo Resultado Obtenido
            return retorno;
        }

        /// <summary>
        /// Método encargado de Cancelar el Comprobante
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion CancelaComprobante(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si el comprobante está generado y no ha sido cancelado aún
                if (this._bit_generado && this.id_estatus_vigencia == (byte)EstatusVigencia.PendienteCancelacion)
                {
                    //Validando Comprobante de Pago
                    if (!ComprobantePagoDocumentoRelacionado.ValidaComprobantePagoCxC(this._id_comprobante33))
                    {
                        //Validando Nota de Credito
                        retorno = Facturacion.FacturadoEgresoRelacion.ValidaNotaCreditoCFDI(this._id_comprobante33);

                        //Validando Operación
                        if (!retorno.OperacionExitosa)
                        {
                            //Inicializando parametros
                            retorno = this.actualizaAtributosBD(EstatusVigencia.Cancelado, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                                           this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                                           this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                                           this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                                           this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, Fecha.ObtieneFechaEstandarMexicoCentro(),
                                           this._bit_generado, this._bit_transferido_nuevo, this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);

                            //Validamos Resultado
                            if (retorno.OperacionExitosa)
                            {
                                //Insertamos Bitácora de Cancelación de Comprobante
                                retorno = Bitacora.InsertaBitacora(209, this._id_comprobante33, 8913, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                //Validamos Resultado
                                if (retorno.OperacionExitosa)
                                {
                                    //Validamos que no se au Recibo de Nómina
                                    if ((OrigenDatos)this._id_origen_datos != OrigenDatos.ReciboNomina && (OrigenDatos)this._id_origen_datos != OrigenDatos.ReciboPagoCliente)
                                    {
                                        //Instanciamos Relacion Facturado facturacion
                                        using (FacturadoFacturacion objFacturadoFacturacion = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFacturaElectronicav3_3(this._id_comprobante33)))
                                        {
                                            //Validamos que exista Relación
                                            if (objFacturadoFacturacion.habilitar)
                                            {
                                                //Validamos el Origen de Datos
                                                if ((OrigenDatos)this._id_origen_datos == OrigenDatos.Facturado)
                                                {
                                                    //Deshablitamos Relación
                                                    retorno = objFacturadoFacturacion.ActualizaEstatusFacturadoFacturacion(FacturadoFacturacion.Estatus.Cancelada, id_usuario);
                                                }
                                                else if ((OrigenDatos)this._id_origen_datos == OrigenDatos.FacturaGlobal)
                                                {
                                                    //Instanciamos Factura Global
                                                    using (FacturaGlobal objFacturaGlobal = new FacturaGlobal(objFacturadoFacturacion.id_factura_global))
                                                    {
                                                        //Deshabilitamos Factura Global
                                                        retorno = objFacturaGlobal.ActualizaACanceladoEstatusFacturaGlobal_V3_3(id_usuario);
                                                    }
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion("Imposible recuperar datos complementarios Facturado Facturación.");

                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion(retorno.Mensaje);
                    }
                    else
                        retorno = new RetornoOperacion("El Comprobante está relacionado a un CFDI de Recepción de Pagos, imposible su cancelación.");

                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("El comprobante no se ha timbrado aún o ya se ha cancelado con aterioridad.");

                //Validamos resultado
                if (retorno.OperacionExitosa)
                {
                    //Instanciando Comprobante
                    retorno = new RetornoOperacion(this._id_comprobante33);
                    //Completando Scope
                    scope.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Realiza la cancelación del Timbre Fiscal Digital ante el SAT, actualizando el acuse obtenido
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        [Obsolete]
        public RetornoOperacion CancelaTimbreFiscalDigital(int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante33);
            XDocument acuseXml = null;
            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si el comprobante se ha timbrado
                if (this._bit_generado)
                {
                    //Validando Comprobante de Pago
                    if (!ComprobantePagoDocumentoRelacionado.ValidaComprobantePagoCxC(this._id_comprobante33))
                    {
                        //Instanciando timbre del comprobante
                        using (TimbreFiscalDigital t = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante33))
                        {
                            //Si el Timbre existe
                            if (t.id_timbre_fiscal_digital > 0 && t.habilitar)
                            {
                                //Instanciando emisor del comprobante
                                using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                                {
                                    //Si el emisor se carga correctamente
                                    if (em.id_compania_emisor_receptor > 0 && em.habilitar)
                                    {
                                        //Obteniendo el certificado activo para el emisor y/o sucursal
                                        using (CertificadoDigital certificado = CertificadoDigital.RecuperaCertificadoEmisorSucursal(em.id_compania_emisor_receptor, this._id_sucursal, CertificadoDigital.TipoCertificado.CSD))
                                        {
                                            //Cargando certificado (.cer)
                                            using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                                            {
                                                //Obtenemos XML de la Llave privada
                                                System.Security.Cryptography.RSACryptoServiceProvider p = TSDK.Base.CertificadoDigital.CertificadosOpenSSLKey.DecodeEncryptedPrivateKeyInfo(File.ReadAllBytes(certificado.ruta_llave_privada), TSDK.Base.Cadena.CadenaSegura(certificado.contrasena_desencriptada));

                                                //Cancelamos Timbre
                                                resultado = FEv32.PacCompaniaEmisor.CancelaTimbrePAC3_3(em.rfc, cer.CertificadoBase64, p.ToXmlString(true), t.UUID, out acuseXml, this._id_compania_emisor);
                                                //Si no existe error
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Declarando variable Auxiliar
                                                    RetornoOperacion ra = new RetornoOperacion();
                                                    
                                                    //Validando Código de Acierto
                                                    switch (resultado.IdRegistro)
                                                    {
                                                        case 202:
                                                            {
                                                                //Obteniendo Acuse de Cancelación
                                                                using(AcuseCancelacionDetalle ac = AcuseCancelacionDetalle.ObtieneAcuseTimbre(t.id_timbre_fiscal_digital))
                                                                {
                                                                    //Validando Existencia
                                                                    if (!ac.habilitar_tacd)
                                                                    {
                                                                        //Realizando inserción de registros acuse 
                                                                        ra = AcuseCancelacion.InsertaAcuseCancelacion(resultado.IdRegistro.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), acuseXml, "3.3", id_usuario);

                                                                        //Si no hay error
                                                                        if (ra.OperacionExitosa)
                                                                            //Insertando detalle
                                                                            ra = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(ra.IdRegistro, t.id_timbre_fiscal_digital, 1, id_usuario);

                                                                        //Si no hay errores
                                                                        if (ra.OperacionExitosa)
                                                                            //Insertando Bitacora
                                                                            ra = Bitacora.InsertaBitacora(209, this._id_comprobante33, 8887, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                                                                    }
                                                                    else
                                                                        //Instanciando Acuse de Detalle
                                                                        ra = new RetornoOperacion(ac.id_acuse_cancelacion_detalle_tacd);
                                                                }
                                                                break;
                                                            }
                                                        case 201:
                                                            {
                                                                //Realizando inserción de registros acuse 
                                                                ra = AcuseCancelacion.InsertaAcuseCancelacion(resultado.IdRegistro.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), acuseXml, "3.3", id_usuario);

                                                                //Si no hay error
                                                                if (ra.OperacionExitosa)
                                                                    //Insertando detalle
                                                                    ra = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(ra.IdRegistro, t.id_timbre_fiscal_digital, 1, id_usuario);

                                                                //Si no hay errores
                                                                if (ra.OperacionExitosa)
                                                                    //Insertando Bitacora
                                                                    ra = Bitacora.InsertaBitacora(209, this._id_comprobante33, 8887, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                                                                break;
                                                            }
                                                    }

                                                    //Si existe algún error al actualizar BD
                                                    if (!ra.OperacionExitosa)
                                                    {
                                                        //Sobreescribiendo resultado
                                                        resultado = ra;
                                                    }
                                                    else
                                                    {
                                                        //Finalizamos transacción
                                                        scope.Complete();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("La información del Emisor no pudo ser recuperada.");
                                }
                            }
                            else
                                resultado = new RetornoOperacion("No es posible recuperar la información de timbrado.");
                        }
                    }
                    else
                        resultado = new RetornoOperacion("El comprobante esta relacionado a un pago, imposible su cancelación.");
                }
                else
                    resultado = new RetornoOperacion("El comprobante no ha sido timbrado aún.");

            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera el archivo .pdf del CFDI
        /// </summary>
        /// <returns></returns>
        public byte[] GeneraPDFComprobanteV33()
        {
            //Creando nuevo visor de reporte
            ReportViewer rvReporte = new Microsoft.Reporting.WebForms.ReportViewer();
            
            //Declaramos variables para armar el nombre del archivo
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Creación d ela tabla para cargar el Logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));

            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Creación de la variable idComprobante
            int idComprobante = this._id_comprobante33;
            //Asignación de la ubicación del reporte local
            rvReporte.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/RDLC/Comprobante_CFDI33.rdlc");

            //Carga Conceptos del Comprobante
            using (DataTable Concepto = SAT_CL.FacturacionElectronica33.Concepto.ObtieneConceptosComprobante(idComprobante))
            {
                //Validando Concepto
                if (Validacion.ValidaOrigenDatos(Concepto))
                {
                    //Agregar origen de datos 
                    ReportDataSource rsComprobanteCFDI = new ReportDataSource("ComprobanteCFDIV33", Concepto);
                    //Asigna los valores al conjunto de datos
                    rvReporte.LocalReport.DataSources.Add(rsComprobanteCFDI);
                }
                else
                    using (DataTable tabla = new DataTable())
                    {
                        tabla.Columns.Add("Id", typeof(int));
                        tabla.Columns.Add("IdComprobante", typeof(int));
                        tabla.Columns.Add("Cantidad", typeof(decimal));
                        tabla.Columns.Add("ClaveUnidad", typeof(int));
                        tabla.Columns.Add("Unidad", typeof(string));
                        tabla.Columns.Add("ClaveServProd", typeof(int));
                        tabla.Columns.Add("Concepto", typeof(string));
                        tabla.Columns.Add("NoIdentificacion", typeof(string));
                        tabla.Columns.Add("ValorUnitario", typeof(decimal));
                        tabla.Columns.Add("ImporteMonedaCaptura", typeof(decimal));
                        tabla.Columns.Add("ImporteMonedaNacional", typeof(decimal));
                        tabla.Columns.Add("Descuento", typeof(decimal));
                        tabla.Columns.Add("Impuesto", typeof(string));
                        //Insertar Registros
                        DataRow row = tabla.NewRow();
                        row["Id"] = 0;
                        row["IdComprobante"] = 0;
                        row["Cantidad"] = 0;
                        row["ClaveUnidad"] = 0;
                        row["Unidad"] = "";
                        row["ClaveServProd"] = 0;
                        row["Concepto"] = "";
                        row["NoIdentificacion"] = "";
                        row["ValorUnitario"] = 0;
                        row["ImporteMonedaCaptura"] = 0;
                        row["ImporteMonedaNacional"] = 0;
                        row["Descuento"] = 0;
                        row["Impuesto"] = "";
                        tabla.Rows.Add(row);
                        ReportDataSource rsComprobanteCFDI = new ReportDataSource("ComprobanteCFDIV33", tabla);
                        rvReporte.LocalReport.DataSources.Add(rsComprobanteCFDI);
                    }
            }            
            //Instanciar el Comprobante
            using (SAT_CL.FacturacionElectronica33.Comprobante objComprobante = new SAT_CL.FacturacionElectronica33.Comprobante(idComprobante))
            {           
                //Valida el estatus del comprobante.
                if (objComprobante.id_estatus_vigencia.Equals((byte)SAT_CL.FacturacionElectronica33.Comprobante.EstatusVigencia.Cancelado))
                {
                    //Si el estatus es cancelado  envia al parametro estatusComprobante la leyenda CANCELADO
                    ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "CANCELADO");
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                }
                //En caso contrario no envia nada al parametro estatusComprobante
                else
                {
                    ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "");
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                }
                //Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
                using (SAT_CL.FacturacionElectronica33.Impuesto imp = (SAT_CL.FacturacionElectronica33.Impuesto.ObtieneImpuestoComprobante(objComprobante.id_comprobante33)))
                using (SAT_CL.FacturacionElectronica33.FormaPago fp = new SAT_CL.FacturacionElectronica33.FormaPago(objComprobante.id_forma_pago))
                using (SAT_CL.FacturacionElectronica33.TipoComprobante tc = new SAT_CL.FacturacionElectronica33.TipoComprobante(objComprobante.id_tipo_comprobante))
                {
                    //Asignando Parametros
                    ReportParameter formaPago = new ReportParameter("FormaPagoCFDI", string.Format("{0} - {1}", fp.clave, fp.descripcion));
                    ReportParameter tipoCFDI = new ReportParameter("TipoComprobante", string.Format("{0} - {1}", tc.clave, tc.descripcion));                    
                    //Creando Parametros de Totales
                    ReportParameter subTotal = new ReportParameter("SubTotal", objComprobante.subtotal_captura.ToString("C2"));
                    ReportParameter impTras = new ReportParameter("ImpTrasladado", imp.total_trasladado_captura.ToString("C2"));
                    ReportParameter impRet = new ReportParameter("ImpRetenido", imp.total_retenido_captura.ToString("C2"));
                    ReportParameter descuento = new ReportParameter("Descuento", objComprobante.descuentos_captura.ToString("C2"));
                    ReportParameter total = new ReportParameter("Total", objComprobante.total_captura.ToString("C2"));
                    //Asignando Valores de Parametros
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { formaPago, tipoCFDI, subTotal, impTras, impRet, descuento, total });
                }

                /*/Carga Subtotal e impuestos del Comprobante
                using (DataTable SubTotal = SAT_CL.FacturacionElectronica33.Concepto.ObtineSubtotalComprobante(idComprobante))
                {
                    //Validando Subtotal(es)
                    if (Validacion.ValidaOrigenDatos(SubTotal))
                    {
                        //Agregar origen de datos 
                        ReportDataSource rsTotalesCFDI = new ReportDataSource("TotalesGeneralesCFDIV33", SubTotal);
                        //Asigna los valores al conjunto de datos
                        rvReporte.LocalReport.DataSources.Add(rsTotalesCFDI);
                    }
                    else
                        using (DataTable tabla = new DataTable())
                        {
                            tabla.Columns.Add("SubTotal", typeof(decimal));
                            tabla.Columns.Add("Descuento", typeof(decimal));
                            tabla.Columns.Add("ImpuestoTrasladado", typeof(decimal));
                            tabla.Columns.Add("ImpuestoRetenido", typeof(decimal));
                            tabla.Columns.Add("Total", typeof(decimal));
                            //Insertar Registros
                            DataRow row = tabla.NewRow();
                            row["SubTotal"] = 0;
                            row["Descuento"] = 0;
                            row["ImpuestoTrasladado"] = 0;
                            row["ImpuestoRetenido"] = 0;
                            row["Total"] = 0;
                            tabla.Rows.Add(row);
                            ReportDataSource rsTotalesCFDI = new ReportDataSource("TotalesGeneralesCFDIV33", tabla);
                            rvReporte.LocalReport.DataSources.Add(rsTotalesCFDI);
                        }
                }//*/

                //Asignamos valor a las variables
                serie_descargapdf = objComprobante.serie;
                folio_descargapdf = objComprobante.folio.ToString();
                //Intsanciamos Compania Emisor
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objComprobante.id_compania_emisor))
                {
                    //Asignamos valor a las variables
                    rfc_descargapdf = emisor.rfc;
                    nombrecorto_descargapdf = emisor.nombre_corto;
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] imagen = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagen = System.IO.File.ReadAllBytes(objComprobante.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagen = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagen);
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                    rvReporte.LocalReport.DataSources.Add(rvscod);

                    //Asigna el origen de datos a los parametros, obtenidos de la instancia a la clase compañiaEmisorReceptor
                    ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
                    ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
                    //Instancia a la clase Direccion para obtener la dirección del emisor
                    using (SAT_CL.Global.Direccion direm = new SAT_CL.Global.Direccion(objComprobante.id_direccion_lugar_expedicion))
                    {
                        //Asigna valores a los parametros obtendos de la instancia a la clase Dirección.
                        ReportParameter direccionEmisorSucursal = new ReportParameter("DireccionEmisorSucursal", direm.codigo_postal);
                        //Asigna valores a los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, direccionEmisorSucursal });
                    }
                }
                //Instancia a la compania Receptor
                using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(objComprobante.id_compania_receptor))
                using (SAT_CL.Global.Direccion dirRec = new SAT_CL.Global.Direccion(receptor.id_direccion))
                {
                    //Variable Auxiliar
                    string razon_social = "";
                    string variable = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Código Postal CFDIv3.3", objComprobante.id_compania_emisor);

                    //Validando Configuración de Código Postal
                    if (Convert.ToInt32(variable.Equals("") ? "0" : variable) > 0)

                        //Asignando Valor a la Razón Social
                        razon_social = string.Format("{0} CP:{1}", receptor.nombre, dirRec.codigo_postal);
                    else
                        //Asignando solo Razón Social
                        razon_social = receptor.nombre;

                    //Asigna valores a los parametros obtendos de la instancia a la clase companiaEmisorReceptor
                    ReportParameter razonSocialReceptorCFDI = new ReportParameter("RazonSocialReceptorCFDI", razon_social);
                    ReportParameter rfcReceptorCFDI = new ReportParameter("RFCReceptorCFDI", receptor.rfc);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialReceptorCFDI, rfcReceptorCFDI });
                }
                //Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
                SAT_CL.FacturacionElectronica33.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(objComprobante.id_comprobante33);
                //Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
                ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
                ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);

                string cadenaOriginal = "";

                TSDK.Base.RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(objComprobante.ruta_xml, System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                                 System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), out cadenaOriginal);


                ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
                ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
                ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
                ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
                //Asigna valores a los parametros del reporteComprobante
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });

                //Instanciamos a la clase Certificado
                using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(objComprobante.id_certificado))
                {
                    //Cargando certificado (.cer)
                    using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                    {
                        //Asigna los valores instanciados a los parametros
                        ReportParameter certificadoDigitalEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                        //Asigna valores a los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { certificadoDigitalEmisor });
                    }
                }
                //Asigna los valores de la clase comprobante a los parametros 
                ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", objComprobante.fecha_expedicion.ToString());
                ReportParameter serieCFDI = new ReportParameter("SerieCFDI", objComprobante.serie);
                ReportParameter folio = new ReportParameter("Folio", objComprobante.folio.ToString());
                ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCadenaValor(3197, objComprobante.regimen_fiscal));
                ReportParameter leyendaImpresionCFDI1 = new ReportParameter("LeyendaImpresionCFDI1", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD1"));
                ReportParameter leyendaImpresionCFDI2 = new ReportParameter("LeyendaImpresionCFDI2", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Parametros Impresión CFD", "Total Comprobante") == "SI" ? SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2").Replace("TotalComprobante", objComprobante.total_nacional.ToString() + " (" + TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_nacional.ToString()) + ") ") : SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2"));
                ReportParameter comentario = new ReportParameter("Comentario", SAT_CL.Global.Referencia.CargaReferencia("0", 209, objComprobante.id_comprobante33, "Facturacion Electrónica", "Comentario"));
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Color Empresa", "Color"));                
                string uso_cfdi = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3194, objComprobante.id_uso_receptor);
                ReportParameter usoCFDI = new ReportParameter("UsoCFDI", uso_cfdi);
                //Asigna valores a los parametros del reporteComprobante
                rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,regimenFiscalCFDI,
                                                                          leyendaImpresionCFDI1, leyendaImpresionCFDI2, comentario, 
                                                                          usoCFDI, color});

                //Obtiene la referencia bit código método de pago; permite definir si se mostrara el código o la descripción del método de págo definido por el sat.
                string codigo = SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_receptor, "Leyendas Impresión CFD", "Bit Código Método Pago");
                //Obtienen el valor cadena del método de págo (Codigo del método de págo definido por el SAT).
                string metodoPago = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, objComprobante.id_metodo_pago);
                int moneda = objComprobante.id_moneda;
                //
                switch (moneda)
                {
                    case 1:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_captura.ToString()));
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "MXN");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "");
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, mon, tipoMoneda, figPeso, figEuro });
                            break;
                        }
                    case 2:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_captura.ToString(), "DÓLARES", "USD"));
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "USD");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "");
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, tipoMoneda, mon, figPeso, figEuro });
                            break;
                        }
                    case 3:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_captura.ToString(), "EUROS", "EUR"));
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "EUR");
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "€");
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, tipoMoneda, mon, figPeso, figEuro });
                            break;
                        }
                }
                //Si es codigo es true
                if (codigo == "TRUE" || codigo == "true")
                {
                    //Valida el valor de la cadadena
                    if (metodoPago == "")
                    {
                        //Si la cadena es vacia muestra la descripción del método de págo
                        ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", string.Format("{0} - {1}", SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3195, objComprobante.id_metodo_pago), SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, objComprobante.id_metodo_pago)));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                    }
                    //En caso Contrario Muetra el Código 
                    else
                    {
                        ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", metodoPago);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                    }
                }
                //En caso de que el bit código método de págo no sea true mostrara la descripción del método de pago
                else
                {
                    ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", string.Format("{0} - {1}", SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3195, objComprobante.id_metodo_pago), SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, objComprobante.id_metodo_pago)));
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                }
            }
            //Carga SubInforma
            ReportParameter idcomprobante = new ReportParameter("IdComprobante", idComprobante.ToString());
            rvReporte.LocalReport.SetParameters(new ReportParameter[] { idcomprobante });
            rvReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandlerCompRel);
            
            //ReportViewer1.LocalReport.SetParameters(reportParameterCollection);

            //Carga SubInforma
            rvReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler33);                

            //Devolviendo resultado
            return rvReporte.LocalReport.Render("PDF");
        }
        /// <summary>
        /// Genera el archivo .pdf del CFDI
        /// </summary>
        /// <returns></returns>
        public byte[] GeneraPDFComprobantePagoV33()
        {
            //Creando nuevo visor de reporte
            ReportViewer rvReporte = new Microsoft.Reporting.WebForms.ReportViewer();

            //Creación d ela tabla para cargar el Logotipo de la compañia
            DataTable dtLogo = new DataTable();
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Creando Columnas
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Datos del Nombre del Archivo de Descarga
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";

            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;

            //Creación de la variable idComprobante
            int idComprobante = this._id_comprobante33;

            //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
            ReportDataSource rvs, rvscod, rdsPagos;

            //Asignación de la ubicación del reporte local
            rvReporte.Reset();
            rvReporte.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/RDLC/ComprobantePagoCFDI.rdlc");

            //Instanciando Comprobante 3.3
            using (SAT_CL.FacturacionElectronica33.Comprobante comp = new SAT_CL.FacturacionElectronica33.Comprobante(idComprobante))
            using (SAT_CL.FacturacionElectronica33.TimbreFiscalDigital tfd = SAT_CL.FacturacionElectronica33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(comp.id_comprobante33))
            {
                //Validando que el Comprobante este Habilitado
                if (comp.habilitar && tfd.habilitar)
                {
                    //Asignando parametro
                    ReportParameter cfdi_pago = new ReportParameter("IdCfdiPago", comp.id_comprobante33.ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { cfdi_pago });

                    //Valida el estatus del comprobante.
                    if (comp.id_estatus_vigencia.Equals(SAT_CL.FacturacionElectronica33.Comprobante.EstatusVigencia.Cancelado))
                    {
                        //Si el estatus es cancelado  envia al parametro estatusComprobante la leyenda CANCELADO
                        ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "CANCELADO");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                    }
                    //En caso contrario no envia nada al parametro estatusComprobante
                    else
                    {
                        ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                    }
                    
                    //Obteniendo Pagos
                    using (DataTable dtPagos = SAT_CL.FacturacionElectronica33.EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(comp.id_comprobante33))
                    {
                        //Validando Datos
                        if (Validacion.ValidaOrigenDatos(dtPagos))

                            //Asignando Pagos
                            rdsPagos = new ReportDataSource("Pagos", dtPagos);
                        else
                        {
                            //Declarando Tabla Vacia
                            using (DataTable dtP = new DataTable("Pagos"))
                            {
                                //Definiendo Columnas
                                dtP.Columns.Add("Id", typeof(int));
                                dtP.Columns.Add("FormaPago", typeof(string));
                                dtP.Columns.Add("Fecha", typeof(string));
                                dtP.Columns.Add("Monto", typeof(decimal));
                                dtP.Columns.Add("Moneda", typeof(string));

                                //Añadiendo Fila en Vacio
                                dtP.Rows.Add(0, "", "", 0.00M, "");

                                //Asignando Pagos
                                rdsPagos = new ReportDataSource("Pagos", dtP);
                            }
                        }
                    }

                    //Declarando Parametros
                    ReportParameter razonSocial, rfcEmisor, ColorImp, regimenFiscal, fechaComprobante, fechaCFDI, serieFolio,
                                    lugarExpedicion, uuid, certificadoSerieSAT, certificadoEmisor,
                                    cadenaOriginal, selloDigital, selloDigitalSAT, razonSocialRec, rfcReceptor, usoCFDI;

                    //Creació del arreglo necesario para la carga de la ruta del Código QR
                    byte[] imagenQR = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagenQR = System.IO.File.ReadAllBytes(comp.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagenQR = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagenQR);

                    //Asignando Atributos del Comprobante
                    fechaComprobante = new ReportParameter("FechaComprobanteCFDI", comp.fecha_expedicion.ToString("dd/MM/yyyy HH:mm"));
                    fechaCFDI = new ReportParameter("FechaCFDI", tfd.fecha_timbrado.ToString("dd/MM/yyyy HH:mm"));
                    serieFolio = new ReportParameter("SerieFolioCFDI", string.Format("{0}-{1}", comp.serie, comp.folio));
                    uuid = new ReportParameter("UUID", tfd.UUID);
                    certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", tfd.no_certificado);
                    serie_descargapdf = comp.serie;
                    folio_descargapdf = comp.folio;

                    //Obteniendo Cadena Orignal
                    string cad_ori = "";

                    //Validando Timbrado
                    if (tfd.habilitar)
                    {
                        //Obteniendo Datos
                        SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(comp.ruta_xml, System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                                 System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), out cad_ori);

                        //Instanciamos a la clase Certificado
                        using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(comp.id_certificado))
                        {
                            //Cargando certificado (.cer)
                            using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))

                                //Asigna los valores instanciados a los parametros
                                certificadoEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                        }
                    }
                    else
                        //Asigna los valores instanciados a los parametros
                        certificadoEmisor = new ReportParameter("CertificadoDigitalEmisor", "");

                    //Asignando Valores
                    cadenaOriginal = new ReportParameter("CadenaOriginalCFDI", cad_ori);
                    selloDigital = new ReportParameter("SelloDigitalCFDI", comp.sello);
                    selloDigitalSAT = new ReportParameter("SelloDigitalSatCFDI", tfd.sello_SAT);

                    //Instanciamos a la clase Certificado
                    using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(comp.id_certificado))
                    {
                        //Cargando certificado (.cer)
                        using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))

                            //Asigna los valores instanciados a los parametros
                            certificadoEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                    }

                    //Instanciando Emisor y Receptor System.Web.HttpContext.Current.
                    using (SAT_CL.Global.CompaniaEmisorReceptor emi = new SAT_CL.Global.CompaniaEmisorReceptor(comp.id_compania_emisor),
                            rec = new SAT_CL.Global.CompaniaEmisorReceptor(comp.id_compania_receptor))
                    using (SAT_CL.Global.Direccion dirEmi = new SAT_CL.Global.Direccion(emi.id_direccion))
                    using (SAT_CL.Global.Direccion dirRec = new SAT_CL.Global.Direccion(rec.id_direccion))
                    {
                        //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                        byte[] logotipo = null;
                        //Permite capturar errores en caso de que no exista una ruta para el archivo
                        try
                        {
                            //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                            logotipo = System.IO.File.ReadAllBytes(emi.ruta_logotipo);
                        }
                        //En caso de que no exista una imagen, se devolvera un valor nulo.
                        catch { logotipo = null; }
                        //Agrega a la tabla un registro con valor a la ruta de la imagen.
                        dtLogo.Rows.Add(logotipo);

                        //Asignando Emisor
                        razonSocial = new ReportParameter("RazonSocialEmisorCFDI", emi.nombre);
                        rfcEmisor = new ReportParameter("RFCEmisorCFDI", emi.rfc);
                        ColorImp = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emi.id_compania_emisor_receptor, "Color Empresa", "Color"));
                        regimenFiscal = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3197, emi.id_regimen_fiscal));
                        lugarExpedicion = new ReportParameter("LugarExpedicion", dirEmi.codigo_postal);
                        nombrecorto_descargapdf = emi.nombre_corto;

                        //Asignando Receptor
                        razonSocialRec = new ReportParameter("RazonSocialReceptorCFDI", rec.nombre);
                        rfcReceptor = new ReportParameter("RFCReceptorCFDI", rec.rfc);
                        usoCFDI = new ReportParameter("UsoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3194, emi.id_uso_cfdi));
                    }

                    //Asignación de Parametros
                    rvReporte.LocalReport.SetParameters(new ReportParameter[]{ razonSocial, rfcEmisor, ColorImp, regimenFiscal, fechaComprobante, 
                                                                               fechaCFDI, serieFolio, lugarExpedicion, uuid, certificadoSerieSAT, 
                                                                               certificadoEmisor, cadenaOriginal, selloDigital, selloDigitalSAT, razonSocialRec, 
                                                                               rfcReceptor, usoCFDI });

                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen
                    rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                    rvReporte.LocalReport.DataSources.Add(rvscod);
                    rvReporte.LocalReport.DataSources.Add(rdsPagos);
                }
            }

            //Carga SubInforma
            rvReporte.LocalReport.SubreportProcessing += new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            rvReporte.LocalReport.Refresh();

            //Devolviendo resultado
            return rvReporte.LocalReport.Render("PDF");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //Obteniendo Pago
            int idPago = int.Parse(e.Parameters["IdPagoDocumento"].Values[0].ToString());
            int idCFDI = int.Parse(e.Parameters["IdCfdiPago"].Values[0].ToString());

            //Obteniendo Pagos 
            using (DataTable dtDocumentosPago = SAT_CL.FacturacionElectronica33.ComprobantePagoDocumentoRelacionado.ObtieneComprobantesPago(idPago, idCFDI))
            {
                //Añadiendo Origen de Datos
                e.DataSources.Add(new ReportDataSource("DocumentosPago", dtDocumentosPago));
            }//*/
        }
        /// <summary>
        /// Envía los archivos PDF y/o XML del CFDI solicitado como adjuntos en un mensaje de correo electrónico
        /// </summary>
        /// <param name="remitente">Correo electrónico desde el que se hará el envío del correo electrónico</param>
        /// <param name="asunto">Asunto que llevará el correo electrónico</param>
        /// <param name="mensaje">Mensaje del correo electrónico</param>
        /// <param name="destinatarios">Conjunto de destinatarios a los que será enviado el correo electrónico</param>
        /// <param name="pdf">True para adjuntar el archivo pdf</param>
        /// <param name="xml">True para adjuntar el archivo xml</param>
        /// <returns></returns>
        public RetornoOperacion EnviaArchivosEmailV3_3(string remitente, string asunto, string mensaje, string[] destinatarios, bool pdf, bool xml)
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante33);

            //Creando mensaje personalizado para CFDI
            string mensajeCFDI = generaMensajeEmailCFDI(mensaje);

            //Enviamos Email
            //Instanciando Correo Electronico
            using (Correo email = new Correo(remitente, destinatarios, asunto, mensajeCFDI, true))
            {
                //Si se solicita adjuntar archivo xml
                if (xml)
                    //Adjuntando archivo xml del  comprobante
                    email.ArchivosAdjuntos.Add(new System.Net.Mail.Attachment(this._ruta_xml));

                //Si se solicita archivo pdf
                if (pdf)
                {
                    //Declaramos variable para Almacenar los Bytes del PDF
                    byte[] PDF = null;

                    //De acuerdo al Tipo solicitado
                    //Combrobante
                    if ((OrigenDatos)this._id_origen_datos == OrigenDatos.ReciboNomina)
                    {
                        //Obtenemos Bytes Comprobante de Nómina
                        //Obtenemos Id de Nómina de Empleado
                        int id_nomina_empleado = Nomina.NomEmpleado.ObtieneIdNomEmpleadoV3_3(this._id_comprobante33);

                        //Instanciamos Nómina Empleado
                        using (Nomina.NomEmpleado objNomEmpleado = new Nomina.NomEmpleado(id_nomina_empleado))
                        {
                            //Validando Empleado
                            if (objNomEmpleado.habilitar && objNomEmpleado.id_comprobante33 > 0)
                            
                                //Obtenmos Bytes de la Nómina Empleado
                                PDF = objNomEmpleado.GeneraPDFComprobanteNomina33();
                        }
                    }
                    else
                        //Obtenemos Bytes de Comprobante
                        PDF = GeneraPDFComprobanteV33();

                    //Creando representación impresa (pdf)
                    MemoryStream flujoPDF = new MemoryStream(PDF);
                    //Adjuntando archivo pdf
                    email.ArchivosAdjuntos.Add(new System.Net.Mail.Attachment(flujoPDF, string.Format("{0}{1}{2}", this._serie, this._folio, ".pdf")));
                }

                //Enviando Correo Electronico
                bool enviar = email.Enviar();

                //Si no se envío el mensaje
                if (!enviar)
                {
                    string errores = "";
                    //Recorriendo los errores del envío
                    foreach (string error in email.Errores)
                        //Añadiendo errores a la lista
                        errores = errores + error + "<br />";
                    resultado = new RetornoOperacion(errores);
                }
                else
                {
                    //En caso de ser Exitoso el Envió, Insertamos Referencia
                    SAT_CL.Global.Referencia.InsertaReferencia(this._id_comprobante33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Envio E-mail", 0, "E-Mail (Enviados)"), string.Join(";", destinatarios), Fecha.ObtieneFechaEstandarMexicoCentro(), 9, true);
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Evento de carga para el Subreporte versión 3.3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubreportProcessingEventHandler33(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath.ToString() == "ComprobanteReferencias")
            {
                int id_comprobante33 = int.Parse(e.Parameters["IdComprobanteReferencia"].Values[0].ToString());
                int id_concepto33 = int.Parse(e.Parameters["IdConceptoReferencia"].Values[0].ToString());
                using (DataTable mitReferencias = SAT_CL.FacturacionElectronica33.Concepto.CargaImpresionReferencias(id_concepto33, id_comprobante33))
                {
                    e.DataSources.Add(new ReportDataSource("Referencias", mitReferencias));
                }
            }
        }
        /// <summary>
        /// Evento de carga para el Subreporte de Comprobante Relacionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubreportProcessingEventHandlerCompRel(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath.ToString() == "ComprobanteRelacionado")
            {
                int id_comprobante = int.Parse(e.Parameters["IdComprobante"].Values[0].ToString());
                //Carga CFDI Relacionados
                using (DataTable CFDIRelacionados = SAT_CL.FacturacionElectronica33.ComprobanteRelacion.ObtieneRelacionesComprobante(id_comprobante))
                {
                    //Agregar origen de datos 
                    //ReportDataSource rsCFDIRelacionado;
                    //Validando Concepto
                    if (Validacion.ValidaOrigenDatos(CFDIRelacionados))
                    {
                        e.DataSources.Add(new ReportDataSource("ComprobanteRelacionado", CFDIRelacionados));
                    }
                    else
                        using (DataTable tablaCFDI = new DataTable())
                        {
                            tablaCFDI.Columns.Add("Id", typeof(string));
                            tablaCFDI.Columns.Add("Secuencia", typeof(string));
                            tablaCFDI.Columns.Add("TipoRelacion", typeof(string));
                            tablaCFDI.Columns.Add("Relacion", typeof(string));
                            tablaCFDI.Columns.Add("UUID", typeof(string));
                            //Insertar Registros
                            DataRow row = tablaCFDI.NewRow();
                            row["Id"] = "";
                            row["Secuencia"] = "";
                            row["TipoRelacion"] = "";
                            row["Relacion"] = "";
                            row["UUID"] = "";
                            tablaCFDI.Rows.Add(row);
                            e.DataSources.Add(new ReportDataSource("ComprobanteRelacionado", tablaCFDI));
                        }
                    //Asigna los valores al conjunto de datos
                    //rvReporte.LocalReport.DataSources.Add(rsCFDIRelacionado);
                }
            }
        }
        /// <summary>
        /// Método encargado de devolver la tabla con los conceptos del comprobante.
        /// </summary>
        /// <param name="id_comprobante33">Llave primaria de fe33.comprobante_tcp</param>
        /// <returns></returns>
        public static DataTable CargaConceptosComprobante(int id_comprobante33)
        {
            //Definiendo objeto de retorno
            DataTable dtConcComp = null;
            //Declarando Arreglo de parámetros
            object[] param = {
                                    6,//tipo
                                    id_comprobante33,//id_comprobante_tcp
                                    0,//id_estatus_vigencia_tcp
                                    0,//id_tipo_comprobante_tcp
                                    0,//id_origen_datos_tcp
                                    0,//id_certificado_tcp
                                    "",//version_tcp
                                    "",//serie_tcp
                                    "",//folio_tcp
                                    "",//sello_tcp
                                    0,//id_forma_pago_tcp
                                    0,//id_metodo_pago_tcp
                                    "",//condicion_pago_tcp
                                    0,//id_moneda_tcp
                                    0.00M,//subtotal_captura_tcp
                                    0.00M,//impuestos_captura_tcp
                                    0.00M,//descuentos_captura_tcp
                                    0.00M,//total_captura_tcp
                                    0.00M,//subtotal_nacional_tcp
                                    0.00M,//impuestos_nacional_tcp
                                    0.00M,//descuentos_nacional_tcp
                                    0.00M,//total_nacional_tcp
                                    0,//tipo_cambio_tcp
                                    "",//lugar_expedicion_tcp
                                    0,//id_direccion_lugar_expedicion_tcp
                                    "",//confirmacion_tcp
                                    0,//id_compania_emisor_tcp
                                    "",//regimen_fiscal_tcp
                                    0,//id_sucursal_tcp
                                    0,//id_compania_receptor_tcp
                                    0,//id_uso_cfdi_receptor_tcp
                                    null,//fecha_captura_tcp
                                    null,//fecha_expedicion_tcp
                                    null,//fecha_cancelacion_tcp
                                    false,//bit_generado_tcp
                                    false,//bit_transferido_nuevo_tcp
                                    0,//id_transferencia_nuevo_tcp
                                    false,//bit_transferido_cancelar_tcp
                                    0,//id_transferencia_cancelar_tcp
                                    0,//id_usuario_tcp
                                    false,//habilitar_tcp
                                    "",//param1
                                    ""//param2
                                    };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores
                    dtConcComp = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtConcComp;
        }

        #endregion

        #region Nota de Credito 3.3

        /// <summary>
        /// Método encargado de Crear la Nota de Credito
        /// </summary>
        /// <param name="id_compania_emisor">Compania (Emisor)</param>
        /// <param name="id_compania_receptor">Cliente (Receptor)</param>
        /// <param name="id_forma_pago">Forma de Pago</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="id_uso_cfdi">Uso del CFDI</param>
        /// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
        /// <param name="tipo_cambio">Tipo de Cambio</param>
        /// <param name="comprobantes">Saldo Pendiente</param>
        /// <param name="serie">Serie de la Nota de Credito</param>
        /// <param name="aplica_facturados"></param>
        /// <param name="ruta_xslt_co">Ruta del Archivo de Transformación</param>
        /// <param name="ruta_xslt_co_local">Ruta del Archivo de Transformación (Local)</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion CreaNotaCreditoCxC(int id_compania_emisor, int id_compania_receptor, int id_forma_pago, int id_metodo_pago,
                                                          int id_uso_cfdi, DateTime fecha_tipo_cambio, decimal tipo_cambio, List<Tuple<int,decimal>> comprobantes,
                                                          string serie,
                                                          bool aplica_facturados, string ruta_xslt_co, string ruta_xslt_co_local, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            int idNC = 0, idFacturadoNC = 0, idFacOtros = 0, idMoneda = 0;
            List<Tuple<int, int, decimal, decimal, int>> facturados = new List<Tuple<int, int, decimal, decimal, int>>();
            DateTime fec_fac = Fecha.ObtieneFechaEstandarMexicoCentro(), fec_tc = DateTime.MinValue;
            decimal saldo_pendiente = 0.00M;
            List<Tuple<int, int>> cfdi_i_aplicacion = new List<Tuple<int, int>>();
            List<Tuple<int, byte, decimal, decimal>> cfdi_rel = new List<Tuple<int, byte, decimal, decimal>>();
            decimal tol_mon = 0.00M;

            //Validando que existan Comprobantes
            if (comprobantes.Count > 0)
            {
                try
                {
                    //Obteniendo Montos
                    tol_mon = Convert.ToDecimal(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Monto Tolerancia Saldo Pago Cliente", id_compania_emisor));
                }
                catch { tol_mon = 0.00M; }

                /** Obteniendo Saldo de los Comprobantes **/
                //Recorriendo Comprobantes
                foreach (Tuple<int,decimal> cmp in comprobantes)
                {
                    //Instanciando Comprobante
                    using (Comprobante cfdi = new Comprobante(cmp.Item1))
                    {
                        //Validando Comprobante
                        if (cfdi.habilitar)
                        {
                            //Validando Comprobante
                            if (cfdi.id_tipo_comprobante == 1)
                            {
                                //Obteniendo Saldo del Comprobante
                                decimal saldo_cmp = FacturacionElectronica33.Reporte.ObtieneSaldoPendienteComprobante(cfdi.id_comprobante33);

                                //Validando Saldo
                                if (saldo_cmp > 0)
                                {
                                    //Validando Saldo del Comprobante
                                    if (cmp.Item2 <= saldo_cmp)
                                    {
                                        //Añadiendo a lista de relaciones
                                        cfdi_rel.Add(new Tuple<int, byte, decimal, decimal>(cfdi.id_comprobante33, 1, cfdi.total_captura, cmp.Item2));

                                        //Incrementando Saldo Total
                                        saldo_pendiente += cmp.Item2;

                                        //Validando Origen del Comprobante
                                        switch ((Comprobante.OrigenDatos)cfdi.id_origen_datos)
                                        {
                                            case OrigenDatos.Facturado:
                                                {
                                                    //Obteniendo Facturado del Comprobante v3.3
                                                    using (FacturadoFacturacion ff = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFacturaElectronicav3_3(cfdi.id_comprobante33)))
                                                    {
                                                        //Validando Relación
                                                        if (ff.id_factura > 0 && ff.habilitar)
                                                        {
                                                            //Obteniendo Monto Pendiente por Aplicar
                                                            decimal monto_pend = Facturado.ObtieneMontoPendienteAplicacion(ff.id_factura), monto_por_aplicar = 0.00M;

                                                            //Si el saldo de mi Cfdi es mayor o igual a mi monto pendiente
                                                            if (cmp.Item2 >= monto_pend)
                                                                monto_por_aplicar = monto_pend;
                                                            //Si el saldo de mi Cfdi es menor a mi monto pendiente
                                                            else if (cmp.Item2 < monto_pend)
                                                                monto_por_aplicar = cmp.Item2;

                                                            //Añadiendo a la lista de Facturados
                                                            facturados.Add(new Tuple<int, int, decimal, decimal, int>(ff.id_factura, cmp.Item1, monto_pend, monto_por_aplicar, 0));
                                                            //Instanciando Resultado Positivo
                                                            result = new RetornoOperacion(0, "Resultado Positivo", true);
                                                        }
                                                        else
                                                            //Instanciando Resultado Positivo
                                                            result = new RetornoOperacion(string.Format("No se puede recuperar la Factura del Comprobante '{0}{1}'", cfdi.serie, cfdi.folio));
                                                    }
                                                    break;
                                                }
                                            case OrigenDatos.FacturaGlobal:
                                                {
                                                    //Obteniendo Detalles de la Factura Global
                                                    using (DataTable dtFF = FacturadoFacturacion.ObtieneDetallesFacturaGlobalV3_3(cfdi.id_comprobante33))
                                                    {
                                                        //Validando Detalles
                                                        if (Validacion.ValidaOrigenDatos(dtFF))
                                                        {
                                                            decimal sld_cmp = cmp.Item2;
                                                            //Recorriendo Relaciones
                                                            foreach (DataRow dr in dtFF.Rows)
                                                            {
                                                                //Obteniendo Facturado del Comprobante v3.3
                                                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                                                {
                                                                    //Validando Relación
                                                                    if (ff.id_factura > 0 && ff.habilitar)
                                                                    {
                                                                        //Validando que exista saldo en el Comprobante
                                                                        if (sld_cmp > 0)
                                                                        {
                                                                            //Obteniendo Monto Pendiente por Aplicar
                                                                            decimal monto_pend = Facturado.ObtieneMontoPendienteAplicacion(ff.id_factura), monto_por_aplicar = 0.00M;

                                                                            //Si el saldo de mi Cfdi es mayor o igual a mi monto pendiente
                                                                            if (sld_cmp >= monto_pend)
                                                                                monto_por_aplicar = monto_pend;
                                                                            //Si el saldo de mi Cfdi es menor a mi monto pendiente
                                                                            else if (sld_cmp < monto_pend)
                                                                                monto_por_aplicar = sld_cmp;

                                                                            //Añadiendo a la lista de Facturados
                                                                            facturados.Add(new Tuple<int, int, decimal, decimal, int>(ff.id_factura, cmp.Item1, monto_pend, monto_por_aplicar, ff.id_factura_global));
                                                                            //Instanciando Resultado Positivo
                                                                            result = new RetornoOperacion(0, "Resultado Positivo", true);
                                                                            //Restando al Saldo del Comprobante
                                                                            sld_cmp = sld_cmp - monto_por_aplicar;
                                                                        }
                                                                        else
                                                                            break;
                                                                    }
                                                                    else
                                                                        //Instanciando Resultado Positivo
                                                                        result = new RetornoOperacion(string.Format("No se puede recuperar las Facturas del Comprobante '{0}{1}'", cfdi.serie, cfdi.folio));
                                                                }

                                                                //Validando Resultado
                                                                if (!result.OperacionExitosa)

                                                                    //Terminando Ciclo
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Resultado Positivo
                                                            result = new RetornoOperacion(string.Format("No se puede recuperar las Facturas del Comprobante '{0}{1}'", cfdi.serie, cfdi.folio));
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    //Instanciando Resultado Positivo
                                                    result = new RetornoOperacion("No se puede crear una Nota de Credito de este Comprobante");
                                                    break;
                                                }
                                        }

                                        //Validando Resultado
                                        if (!result.OperacionExitosa)

                                            //Terminando Ciclo
                                            break;
                                    }
                                    else
                                    {
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(string.Format("El monto del Comprobante '{0}{1}' excede la Cantidad Permitida '{2}'", cfdi.serie, cfdi.folio, saldo_cmp));
                                        //Terminando Ciclo
                                        break;
                                    }
                                }
                                else
                                {
                                    //Instanciando Excepción
                                    result = new RetornoOperacion(string.Format("El Comprobante '{0}{1}' ya no tiene Saldo Pendiente", cfdi.serie, cfdi.folio));
                                    //Terminando Ciclo
                                    break;
                                }
                            }
                            else
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion(string.Format("El Comprobante '{0}{1}' no es de Ingreso, imposible crear una Nota de Credito", cfdi.serie, cfdi.folio));
                                //Terminando Ciclo
                                break;
                            }
                        }
                        else
                        {
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede recuperar el Comprobante");
                            //Terminando Ciclo
                            break;
                        }
                    }
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Validando Saldo Pendiente
                    if (saldo_pendiente > 0)
                    {
                        //Validando Facturados
                        if (facturados.Count > 0)
                        {
                            //Instanciando Compania Emisora
                            using (CompaniaEmisorReceptor emisor = new CompaniaEmisorReceptor(id_compania_emisor))
                            using (Direccion dir_em = new Direccion(emisor.id_direccion))
                            using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(id_compania_receptor))
                            {
                                //Validando Datos del Emisor
                                if (emisor.habilitar && dir_em.habilitar)
                                {
                                    //Validando Datos del Receptor
                                    if (receptor.habilitar)
                                    {
                                        //Declarando Variables Auxiliares
                                        int id_cta_origen = 0, id_cta_destino = 0;

                                        //Instanciando Forma de Pago
                                        using (FormaPago fp = new FormaPago(id_forma_pago))
                                        {
                                            //Validando Forma de Pago
                                            if (fp.habilitar)
                                            {
                                                //TODO: Aplicar habilitación y borrado de contenido ACORDE A CATÁLOGO FORMA PAGO DEL SAT
                                                switch (fp.clave)
                                                {
                                                    //_02_Cheque
                                                    case "02":
                                                    //_03_TransaferenciaElectronica
                                                    case "03":
                                                    //_04_TarjetaCredito
                                                    case "04":
                                                    //_05_MonederoElectronico
                                                    case "05":
                                                    //_28_TarjetaDebito
                                                    case "28":
                                                    //_29_TarjetaServicios
                                                    case "29":
                                                        //Cargando Cuentas de Origen -- Donde se va a Obtener el Dinero
                                                        using (SAT_CL.Bancos.CuentaBancos cta_ori = SAT_CL.Bancos.CuentaBancos.ObtieneCuentaBanco(25, receptor.id_compania_emisor_receptor, CuentaBancos.TipoCuenta.Default),
                                                                                          cta_des = SAT_CL.Bancos.CuentaBancos.ObtieneCuentaBanco(25, emisor.id_compania_emisor_receptor, CuentaBancos.TipoCuenta.Default))
                                                        {
                                                            //Validando Existencia
                                                            if (cta_ori.habilitar)
                                                            {
                                                                //Asignando Cuenta Origen
                                                                id_cta_origen = cta_ori.id_cuenta_bancos;

                                                                //Validando Existencia
                                                                if (cta_des.habilitar)

                                                                    //Asignando Cuenta Origen
                                                                    id_cta_destino = cta_des.id_cuenta_bancos;

                                                                else
                                                                    //Instanciando Excepción
                                                                    result = new RetornoOperacion("No se puede recuperar la Cuenta de Destino");
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion("No se puede recuperar la Cuenta de Origen");
                                                        }
                                                        break;
                                                    //_06_DineroElectronico
                                                    case "06":
                                                        //Cargando Cuentas de Origen -- Donde se va a Obtener el Dinero
                                                        using (SAT_CL.Bancos.CuentaBancos cta_ori = SAT_CL.Bancos.CuentaBancos.ObtieneCuentaBanco(25, receptor.id_compania_emisor_receptor, CuentaBancos.TipoCuenta.Default))
                                                        {
                                                            //Validando Existencia
                                                            if (cta_ori.habilitar)

                                                                //Asignando Cuenta Origen
                                                                id_cta_origen = cta_ori.id_cuenta_bancos;

                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion("No se puede recuperar la Cuenta de Origen");
                                                        }

                                                        //Inicializando Cuenta Destino
                                                        id_cta_destino = 0;
                                                        break;
                                                    default:
                                                        //Asignando Cuentas en 0
                                                        id_cta_origen = id_cta_destino = 0;
                                                        break;
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("No existe la Forma de Pago");
                                        }

                                        //Validando Resultado
                                        if (result.OperacionExitosa)
                                        {
                                            //Obteniendo Lista de Comprobantes
                                            List<int> idCmp = (from Tuple<int, decimal> tp in comprobantes
                                                               select tp.Item1).ToList();
                                            //Validando Lista de Comprobantes
                                            if (idCmp != null)
                                            {
                                                if (idCmp.Count > 0)
                                                {
                                                    //Validando Moneda
                                                    result = ValidaMonedaComprobantes(string.Join(",", idCmp.ToArray()), out idMoneda);

                                                    //Validando Moneda Comprobante(s)
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Creando Ambiente Transaccional
                                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                        {
                                                            //Validando Tipo de Cambio en Caso de Necesitarse
                                                            if (idMoneda != 1)
                                                            {
                                                                //Validando parametros especiales
                                                                if (fecha_tipo_cambio != DateTime.MinValue && (tipo_cambio != 0 && tipo_cambio != 1))
                                                                {
                                                                    //Validando Tipo de Cambio Valido
                                                                    using (TipoCambio tc = new TipoCambio(emisor.id_compania_emisor_receptor, (byte)idMoneda, fecha_tipo_cambio, 0))
                                                                    {
                                                                        //Validando Tipo de Cambio
                                                                        if (tc.habilitar)
                                                                        {
                                                                            //Validando que el Valor sea Distinto
                                                                            if (tc.valor_tipo_cambio != tipo_cambio)

                                                                                //Actualizando el Tipo de Cambio
                                                                                result = tc.EditarTipoCambio(id_compania_emisor, tipo_cambio, (byte)idMoneda, fecha_tipo_cambio, TipoCambio.OperacionUso.Todos, id_usuario);
                                                                        }
                                                                        else
                                                                            //Insertando Tipo de Cambio (Actualizado)
                                                                            result = TipoCambio.InsertarTipoCambio(id_compania_emisor, tipo_cambio, (byte)idMoneda, fecha_tipo_cambio, TipoCambio.OperacionUso.Todos, id_usuario);

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //Validando Excepciones
                                                                    /** Fecha Tipo de Cambio **/
                                                                    if (fecha_tipo_cambio == DateTime.MinValue)

                                                                        //Instanciando la Excepción
                                                                        result = new RetornoOperacion("Debe de especificar una Fecha de Tipo de Cambio");
                                                                    /** Valor Tipo de Cambio '0' **/
                                                                    else if (tipo_cambio == 0)

                                                                        //Instanciando la Excepción
                                                                        result = new RetornoOperacion("Debe de especificar una Valor de Tipo de Cambio distinto de '0'");
                                                                    /** Valor Tipo de Cambio '1' **/
                                                                    else if (tipo_cambio == 1)

                                                                        //Instanciando la Excepción
                                                                        result = new RetornoOperacion("Debe de especificar una Valor de Tipo de Cambio distinto de '1'");
                                                                }
                                                            }

                                                            //Obteniendo Saldo pendiente de los Facturados
                                                            decimal saldo_facturados = facturados.Sum(s => s.Item4);//s.Item3

                                                            /** SE COMENTA DIFERENCIA DE MONTOS PERMITIDOS POR VALIDACIÓN DE SALDOS IGUAL O MENORES **/
                                                            //Obteniendo Valor Absoluto de la Diferencia de los Montos
                                                            bool res = true;// false;

                                                            //decimal diff = Math.Abs(saldo_facturados - saldo_pendiente);

                                                            ////Validando Si la diferencia es ta dentro del Rango Permitido
                                                            //res = diff == 0 ? true : (diff <= tol_mon ? true : false);

                                                            //Validando Resultado de Montos Diferentes
                                                            if (res)
                                                            {
                                                                //Insertando Nota de Credito (CxC)
                                                                result = SAT_CL.Bancos.EgresoIngreso.InsertaFichaIngreso(id_compania_emisor, 25, receptor.id_compania_emisor_receptor, receptor.nombre,
                                                                                        3, (EgresoIngreso.FormaPagoSAT)id_forma_pago, id_cta_origen, id_cta_destino, "", saldo_pendiente, (byte)idMoneda,
                                                                                        saldo_pendiente, fec_fac, id_usuario);

                                                                //Validando Operación
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Obteniendo Ficha de Ingreso
                                                                    idNC = result.IdRegistro;

                                                                    //Validando Solicitud de Aplicación
                                                                    if (aplica_facturados)
                                                                    {
                                                                        //Recorriendo Facturados
                                                                        foreach (Tuple<int, int, decimal, decimal, int> facturado in facturados)
                                                                        {
                                                                            //Insertando Aplicación
                                                                            result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(9, facturado.Item1,
                                                                                                                                    idNC, facturado.Item4, fec_fac, false,
                                                                                                                                    0, false, 0, id_usuario);

                                                                            //Validando Operación
                                                                            if (result.OperacionExitosa)
                                                                            {
                                                                                //Añadiendo a Lista de Aplicaciones VS Cfdi's de Ingreso
                                                                                cfdi_i_aplicacion.Add(new Tuple<int, int>(result.IdRegistro, facturado.Item2));

                                                                                //Instanciando Facturado
                                                                                using (SAT_CL.Facturacion.Facturado fc = new Facturado(facturado.Item1))
                                                                                {
                                                                                    //Validando Facturado
                                                                                    if (fc.habilitar)
                                                                                    {
                                                                                        //Validando Estatus
                                                                                        Facturado.EstatusFactura est = Facturado.EstatusFactura.Registrada;
                                                                                        if (facturado.Item4 == facturado.Item3)
                                                                                            est = Facturado.EstatusFactura.Liquidada;
                                                                                        else if (facturado.Item4 < facturado.Item3)
                                                                                            est = Facturado.EstatusFactura.AplicadaParcial;

                                                                                        //Actualizando Estatus
                                                                                        result = fc.ActualizaEstatusFactura(est, id_usuario);

                                                                                        //Validando Operación
                                                                                        if (!result.OperacionExitosa)

                                                                                            //Terminando Ciclo
                                                                                            break;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Instanciando Excepción
                                                                                        result = new RetornoOperacion("No se puede recuperar la Factura");
                                                                                        //Terminando Ciclo
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    //Validando Operación
                                                                    if (result.OperacionExitosa)
                                                                    {
                                                                        //Validando Solicitud de Aplicación 
                                                                        if (aplica_facturados)
                                                                        {
                                                                            //Instanciando Nota de Credito (Ficha de Ingreso)
                                                                            using (EgresoIngreso nc = new EgresoIngreso(idNC))
                                                                            {
                                                                                //Validando la existencia de la Nota
                                                                                if (nc.habilitar)

                                                                                    //Actualizando Estatus
                                                                                    result = nc.ActualizaFichaIngresoEstatus(EgresoIngreso.Estatus.Aplicada, id_usuario);
                                                                                else
                                                                                    //Instanciando Excepción
                                                                                    result = new RetornoOperacion("No se puede recuperar la Nota de Credito");
                                                                            }
                                                                        }

                                                                        //Validando Operación
                                                                        if (result.OperacionExitosa)
                                                                        {
                                                                            //Insertando Facturado
                                                                            result = SAT_CL.Facturacion.Facturado.InsertaNotaCreditoCxC(fec_fac, fecha_tipo_cambio, id_compania_emisor, idMoneda, id_usuario);

                                                                            //Validando Operación
                                                                            if (result.OperacionExitosa)
                                                                            {
                                                                                //Obteniendo Facturado
                                                                                idFacturadoNC = result.IdRegistro;

                                                                                //Instanciando Facturado
                                                                                using (SAT_CL.Facturacion.Facturado fac = new Facturado(idFacturadoNC))
                                                                                {
                                                                                    //Validando Operación
                                                                                    if (result.OperacionExitosa)
                                                                                    {
                                                                                        //Insertando Factura de Otros
                                                                                        result = SAT_CL.Facturacion.FacturacionOtros.InsertaFacturacionOtros(idFacturadoNC, id_compania_emisor, id_compania_receptor, id_usuario);

                                                                                        //Validando Operación
                                                                                        if (result.OperacionExitosa)
                                                                                        {
                                                                                            //Instanciando Tipo de Cargo
                                                                                            using (SAT_CL.Tarifas.TipoCargo tc = new Tarifas.TipoCargo(Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Concepto Cobro Nota de Credito v3.3", id_compania_emisor))))
                                                                                            {
                                                                                                //Validando Tipo de Cargo
                                                                                                if (tc.habilitar)
                                                                                                {
                                                                                                    //Obteniendo Bases
                                                                                                    decimal imp_base = saldo_pendiente / (1 + (tc.tasa_impuesto_trasladado == 0.00M ? tc.tasa_impuesto_trasladado : tc.tasa_impuesto_trasladado / 100) - (tc.tasa_impuesto_retenido == 0.00M ? tc.tasa_impuesto_retenido : tc.tasa_impuesto_retenido / 100));

                                                                                                    //Insertando Concepto
                                                                                                    result = SAT_CL.Facturacion.FacturadoConcepto.InsertaFacturaConcepto(idFacturadoNC, 1, 1, "", tc.id_tipo_cargo,
                                                                                                                                                    imp_base, tc.id_tipo_impuesto_retenido, tc.tasa_impuesto_retenido,
                                                                                                                                                    tc.id_tipo_impuesto_trasladado, tc.tasa_impuesto_trasladado, 0, id_usuario);
                                                                                                }
                                                                                                else
                                                                                                    //Instanciando Excepción
                                                                                                    result = new RetornoOperacion("No se puede recuperar el Concepto de la Nota de Credito ");
                                                                                            }

                                                                                            //Validando Operación
                                                                                            if (result.OperacionExitosa && fac.ActualizaFactura())
                                                                                            {
                                                                                                //Obteniendo Facturación Otros
                                                                                                idFacOtros = result.IdRegistro;

                                                                                                //Validando que existe un Uso de CFDI valido
                                                                                                if (id_uso_cfdi > 0)
                                                                                                {
                                                                                                    //Importando Comprobante
                                                                                                    result = fac.ImportaFacturadoComprobante_V3_3(id_forma_pago, id_uso_cfdi, (byte)id_metodo_pago, 2, 0, emisor.id_compania_emisor_receptor, id_usuario, "", cfdi_rel);

                                                                                                    //Validando Operación
                                                                                                    if (result.OperacionExitosa)
                                                                                                    {
                                                                                                        //Obteniendo Comprobante del Facturado
                                                                                                        using (Comprobante cfdi_nc = new Comprobante(Facturacion.FacturadoFacturacion.ObtieneFacturacionElectronicaActivaV3_3(fac.id_factura)))
                                                                                                        {
                                                                                                            //Validando Comprobante Recuperado
                                                                                                            if (cfdi_nc.habilitar)
                                                                                                            {
                                                                                                                //Obteniendo Relación
                                                                                                                using (FacturadoFacturacion ff = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFactura(fac.id_factura)))
                                                                                                                {
                                                                                                                    //Validando Existencia
                                                                                                                    if (ff.habilitar)
                                                                                                                    {
                                                                                                                        //Validando Operación
                                                                                                                        if (result.OperacionExitosa && cfdi_i_aplicacion.Count > 0)
                                                                                                                        {
                                                                                                                            //Insertando Relación Principal
                                                                                                                            result = Facturacion.FacturadoEgreso.InsertaFacturadoEgreso(cfdi_nc.id_compania_emisor, fac.id_factura, idNC, cfdi_nc.id_comprobante33, VERSION_CFDI, id_usuario);

                                                                                                                            //Validando Operación
                                                                                                                            if (result.OperacionExitosa)
                                                                                                                            {
                                                                                                                                //Obteniendo Relación Principal
                                                                                                                                int idFacEgreso = result.IdRegistro;

                                                                                                                                //Recorriendo Aplicaciones
                                                                                                                                foreach (Tuple<int, int> ap_cfdi in cfdi_i_aplicacion)
                                                                                                                                {
                                                                                                                                    //Insertando Relación de Ingreso
                                                                                                                                    result = Facturacion.FacturadoEgresoRelacion.InsertaFacturadoEgresoRelacion(idFacEgreso, ap_cfdi.Item2, ap_cfdi.Item1, id_usuario);

                                                                                                                                    //validando Errores
                                                                                                                                    if (!result.OperacionExitosa)

                                                                                                                                        //Terminando Ciclo
                                                                                                                                        break;
                                                                                                                                }

                                                                                                                                //Validando Operación
                                                                                                                                if (result.OperacionExitosa)
                                                                                                                                {
                                                                                                                                    //Recorriendo Relaciones para saber si se Requiere refacturación
                                                                                                                                    foreach (Tuple<int, byte, decimal, decimal> comp in cfdi_rel)
                                                                                                                                    {
                                                                                                                                        //Obteniendo Diferencia
                                                                                                                                        decimal d = Math.Abs(comp.Item3 - comp.Item4);

                                                                                                                                        //Validando que se Compruebe el Total del Comprobante
                                                                                                                                        if (d <= tol_mon)
                                                                                                                                        {
                                                                                                                                            //Obteniendo Facturas Globales
                                                                                                                                            List<Tuple<int, int, decimal, decimal, int>> facs_globales = (from Tuple<int, int, decimal, decimal, int> fgs in facturados
                                                                                                                                                                                                          where fgs.Item5 > 0
                                                                                                                                                                                                          && fgs.Item2 == comp.Item1
                                                                                                                                                                                                          select fgs).ToList();
                                                                                                                                            //Obteniendo Facturados
                                                                                                                                            List<Tuple<int, int, decimal, decimal, int>> facs = (from Tuple<int, int, decimal, decimal, int> fcs in facturados
                                                                                                                                                                                                 where fcs.Item5 == 0
                                                                                                                                                                                                 && fcs.Item2 == comp.Item1
                                                                                                                                                                                                 select fcs).ToList();

                                                                                                                                            //Validando Fac. Globales
                                                                                                                                            if (facs_globales != null)
                                                                                                                                            {
                                                                                                                                                //Validando minimo 1
                                                                                                                                                if (facs_globales.Count > 0)
                                                                                                                                                {
                                                                                                                                                    //Agrupando Globales
                                                                                                                                                    List<int> fgs = (from Tuple<int, int, decimal, decimal, int> f in facs_globales
                                                                                                                                                                     select f.Item5).Distinct().ToList();

                                                                                                                                                    //Validando Fac. Globales
                                                                                                                                                    if (fgs != null)
                                                                                                                                                    {
                                                                                                                                                        //Recorriendo FG's
                                                                                                                                                        foreach (int fg in fgs)
                                                                                                                                                        {
                                                                                                                                                            //Copiando Factura Global
                                                                                                                                                            result = Facturado.CopiaFacturado(OrigenDatos.FacturaGlobal, fg, new List<Tuple<int, int, int>>(), id_usuario);

                                                                                                                                                            //Validando Resultado
                                                                                                                                                            if (!result.OperacionExitosa)

                                                                                                                                                                //Termiando Ciclo
                                                                                                                                                                break;
                                                                                                                                                        }
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                        //Instanciando Excepción
                                                                                                                                                        result = new RetornoOperacion("No hay facturas Globales");
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                    //Instanciando Excepción
                                                                                                                                                    result = new RetornoOperacion(0, "No hay facturas Globales", true);
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                                //Instanciando Excepción
                                                                                                                                                result = new RetornoOperacion(0, "No hay facturas Globales", true);

                                                                                                                                            //Validando Operación
                                                                                                                                            if (result.OperacionExitosa)
                                                                                                                                            {
                                                                                                                                                //Validando Facturados
                                                                                                                                                if (facs != null)
                                                                                                                                                {
                                                                                                                                                    //Validando minimo 1
                                                                                                                                                    if (facs.Count > 0)
                                                                                                                                                    {
                                                                                                                                                        //Agrupando Globales
                                                                                                                                                        List<int> fcs = (from Tuple<int, int, decimal, decimal, int> f in facs
                                                                                                                                                                         select f.Item1).Distinct().ToList();

                                                                                                                                                        //Validando Fac. Globales
                                                                                                                                                        if (fcs != null)
                                                                                                                                                        {
                                                                                                                                                            //Recorriendo FG's
                                                                                                                                                            foreach (int fc in fcs)
                                                                                                                                                            {
                                                                                                                                                                //Copiando Factura Global
                                                                                                                                                                result = Facturado.CopiaFacturado(OrigenDatos.Facturado, fc, new List<Tuple<int, int, int>>(), id_usuario);

                                                                                                                                                                //Validando Resultado
                                                                                                                                                                if (!result.OperacionExitosa)

                                                                                                                                                                    //Termiando Ciclo
                                                                                                                                                                    break;
                                                                                                                                                            }
                                                                                                                                                        }
                                                                                                                                                        else
                                                                                                                                                            //Instanciando Excepción
                                                                                                                                                            result = new RetornoOperacion("No hay facturas Globales");
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                        //Instanciando Excepción
                                                                                                                                                        result = new RetornoOperacion(0, "No hay facturas Globales", true);
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                    //Instanciando Excepción
                                                                                                                                                    result = new RetornoOperacion(0, "No hay facturas Globales", true);
                                                                                                                                            }

                                                                                                                                            //Validando Resultado
                                                                                                                                            if (!result.OperacionExitosa)

                                                                                                                                                //Termiando Ciclo
                                                                                                                                                break;
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }

                                                                                                                        //Validando Operación Final
                                                                                                                        if (result.OperacionExitosa)
                                                                                                                        {
                                                                                                                            //Actualizando Estatus
                                                                                                                            result = ff.ActualizaEstatusFacturadoFacturacion(FacturadoFacturacion.Estatus.Facturada, id_usuario);

                                                                                                                            //Validando Operación Final
                                                                                                                            if (result.OperacionExitosa)

                                                                                                                                //Timbrando Comprobante
                                                                                                                                result = cfdi_nc.TimbraComprobante(VERSION_CFDI, serie, id_usuario, ruta_xslt_co, ruta_xslt_co_local, true);
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                    //Instanciando Excepción
                                                                                                    result = new RetornoOperacion("No existe un Uso de CFDI valido");
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion(string.Format("Existe una diferencia en los Saldos de '{0:C2}'", Math.Abs(saldo_pendiente - saldo_facturados)));

                                                            //Validando Resultado Final
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Instanciando Factura Otros
                                                                result = new RetornoOperacion(idFacturadoNC);

                                                                //Completando Transacción
                                                                scope.Complete();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No se pueden recuperar los Comprobantes");
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("No se pueden recuperar los Comprobantes");
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No se pueden recuperar los Datos del Receptor");
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se pueden recuperar los Datos del Emisor");
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se pueden recuperar las Facturas de los Comprobantes");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe Saldo Pendiente en los Comprobantes");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe de Seleccionar al menos un Comprobante");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar las Monedas de los Comprobantes
        /// </summary>
        /// <param name="comprobantes">Comprobantes por Validar</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaMonedaComprobantes(string comprobantes, out int id_moneda)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            id_moneda = 0;

            //Declarando Arreglo de Parametros
            object[] param = { 7, 0, 0, 0, 0, 0, "", "", "", comprobantes.Trim().Equals("") ? "0" : comprobantes.Trim(), 0, 0, "", 0, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M,
                               0.00M, 0.00M, "", 0, "", 0, "", 0, 0, 0, null, null, null, false, false, 0, false, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Validando si hay mas de una Moneda
                    if ((from DataRow dr in ds.Tables["Table"].Rows
                         select dr).Count() == 1)
                    {
                        //Obteniendo Moneda
                        id_moneda = (from DataRow dr in ds.Tables["Table"].Rows
                                     select Convert.ToInt32(dr["IdMoneda"])).FirstOrDefault();

                        //Asignando Retorno Positivo
                        result = new RetornoOperacion(id_moneda);
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No todas las Monedas de Comprobantes Coinciden");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se puede recuperar la Moneda del Comprobante");
            }

            //Devolviendo resultado Obtenido
            return result;
        }

        public RetornoOperacion CancelaNotaCreditoCxC(string motivo, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Vigencia
            if (this.bit_generado && (EstatusVigencia)this._id_estatus_vigencia == EstatusVigencia.Vigente)
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Cancelando CFDI
                    CancelacionCDFI.EstatusUUID estatusUUID = CancelacionCDFI.EstatusUUID.SinEstatus;
                    CancelacionCDFI.TipoCancelacion tipo = CancelacionCDFI.TipoCancelacion.SinAsignar;
                    XDocument acuse = new XDocument();
                    retorno = CancelacionCDFI.objCancelacion.CancelacionComprobanteCxC(this._id_comprobante33, out estatusUUID, out tipo, out acuse, id_usuario);
                }
            }
            else
            {
                //Validando Estatus 
                switch ((EstatusVigencia)this._id_estatus_vigencia)
                {
                    case EstatusVigencia.Cancelado:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El Comprobante está Cancelado");
                            break;
                        }
                    case EstatusVigencia.PendienteCancelacion:
                        {
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El Comprobante está Pendiente de Cancelación");
                            break;
                        }
                    default:
                        {
                            //Validando Generación
                            if (!this._bit_generado)
                            
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("El Comprobante no esta Timbrado");
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("Error desconocido, contacte a su Proveedor de Servicio");
                            break;
                        }
                }
            }

            //Devolviendo resultado Obtenido
            return retorno;
        }


        #endregion
    }
}
