using Microsoft.Reporting.WebForms;
using SAT_CL.Bancos;
using SAT_CL.Facturacion;
using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
//Espacios de nombres para uso de archivos de transformación XSL
using System.Xml.Xsl;
using TSDK.Base;
using TSDK.Datos;
using Microsoft.Reporting.WebForms;
using System.Xml;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Proporciona los elementos requeridos para la administración de Comprobantes Fiscales Electrónicos (CFD y CFDI)
    /// </summary>
    public partial class Comprobante : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Tipo Comprobante  
        /// </summary>
        public enum TipoComprobante
        {
            /// <summary>
            /// Comprobante de Ingreso (Factura)
            /// </summary>
            Ingreso = 1,
            /// <summary>
            /// Comprobante de Egreso (Nota de Crédito)
            /// </summary>
            Egreso,
            /// <summary>
            /// Comprobante de Traslado (Carta Porte)
            /// </summary>
            Traslado
        }
        /// <summary>
        /// Estatus del comprobante
        /// </summary>
        public enum EstatusComprobante
        {
            /// <summary>
            /// Comprobante Vigente
            /// </summary>
            Vigente = 1,
            /// <summary>
            /// Comprobante Cancelado
            /// </summary>
            Cancelado
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
            ReciboNomina
        }
        /// <summary>
        /// Define los tipos de transferencia posibles para póliza de comprobante
        /// </summary>
        public enum TipoTransferenciaPoliza
        {
            /// <summary>
            /// Comprobantes nuevos
            /// </summary>
            ComprobanteNuevo = 1,
            /// <summary>
            /// Comprobantes cancelados
            /// </summary>
            ComprobanteCancelado
        }
        /// <summary>
        /// Define los tipos de movimientos existentes en póliza de comprobante
        /// </summary>
        protected enum tipoMovimientoPoliza
        {
            /// <summary>
            /// Movimiento a detalle por comprobante generado por el total de cada uno de los comprobantes
            /// </summary>
            Cliente = 1,
            /// <summary>
            /// Movimiento acumulado correspondiente a la retención del comprobante
            /// </summary>
            Retencion,
            /// <summary>
            /// Movimiento acumulado correspondiente al IVA del comprobante
            /// </summary>
            IVA,
            /// <summary>
            /// Movimiento acumulado correspondiente al subtotal del comprobante
            /// </summary>
            Ingreso
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        protected static string _nombre_stored_procedure = "fe.sp_comprobante_tcm";

        protected int _id_comprobante;
        /// <summary>
        /// Obtiene el identificador de la instancia
        /// </summary>
        public int id_comprobante { get { return _id_comprobante; } }
        protected string _serie;
        /// <summary>
        /// Obtiene la serie del comprobante (No es un dato Fiscal)
        /// </summary>
        public string serie { get { return this._serie; } }
        protected int _folio;
        /// <summary>
        /// Obtiene el Folio del comprobante (No es un dato Fiscal)
        /// </summary>
        public int folio { get { return this._folio; } }
        protected int _id_certificado;
        /// <summary>
        /// Obtiene el Id de Certificado con que se sella el comprobante
        /// </summary>
        public int id_certificado { get { return this._id_certificado; } }
        protected byte _id_tipo_comprobante;
        /// <summary>
        /// Obtiene el etipo de comprobante (ingreso/egreso/traslado)
        /// </summary>
        public TipoComprobante tipo_comprobante { get { return (TipoComprobante)this._id_tipo_comprobante; } }
        protected byte _id_origen_datos;
        /// <summary>
        /// Obtiene el Id de Origen de los datos del comprobante (Sistema o plataforma)
        /// </summary>
        public byte id_origen_datos { get { return this._id_origen_datos; } }
        /// <summary>
        /// Obtiene el origen de datos
        /// </summary>
        public OrigenDatos origen_datos { get { return (OrigenDatos)this._id_origen_datos; } }
        protected byte _id_estatus_comprobante;
        /// <summary>
        /// Obtiene el estatus del comprobante
        /// </summary>
        public EstatusComprobante estatus_comprobante { get { return (EstatusComprobante)this._id_estatus_comprobante; } }
        protected string _version;
        /// <summary>
        /// Obtiene la versión del comprobante
        /// </summary>
        public string version { get { return this._version; } }
        protected string _sello;
        /// <summary>
        /// Obtiene el sello del comprobante
        /// </summary>
        public string sello { get { return this._sello; } }
        protected byte _id_forma_pago;
        /// <summary>
        /// Obtiene la forma de pago del comprobante
        /// </summary>
        public byte id_forma_pago { get { return this._id_forma_pago; } }
        protected byte _id_condiciones_pago;
        /// <summary>
        /// Obtiene las condiciones de pago
        /// </summary>
        public byte id_condiciones_pago { get { return this._id_condiciones_pago; } }
        protected byte _id_metodo_pago;
        /// <summary>
        /// Obtiene el método de pago
        /// </summary>
        public byte id_metodo_pago { get { return this._id_metodo_pago; } }
        protected int _no_parcialidad;
        /// <summary>
        /// Obtiene el número de parcialidad al que ampara el comprobante (Facturas Parciales)
        /// </summary>
        public int no_parcialidad { get { return this._no_parcialidad; } }
        protected int _total_parcialidades;
        /// <summary>
        /// Obtiene el total de parcialidades (Facturas Parciales)
        /// </summary>
        public int total_parcialidades { get { return this._total_parcialidades; } }
        protected byte _id_moneda;
        /// <summary>
        /// Obtiene la Moneda
        /// </summary>
        public byte id_moneda { get { return this._id_moneda; } }
        protected DateTime _fecha_tipo_cambio;
        /// <summary>
        /// Obtiene la Fecha del Tipo de Cambio
        /// </summary>
        public DateTime fecha_tipo_cambio { get { return this._fecha_tipo_cambio; } }
        protected decimal _subtotal_moneda_captura;
        /// <summary>
        /// Obtiene el subtotal en moneda de captura
        /// </summary>
        public decimal subtotal_moneda_captura { get { return this._subtotal_moneda_captura; } }
        protected decimal _subtotal_moneda_nacional;
        /// <summary>
        /// Obtiene el subtotal en moneda nacional
        /// </summary>
        public decimal subtotal_moneda_nacional { get { return this._subtotal_moneda_nacional; } }
        protected decimal _descuento_moneda_captura;
        /// <summary>
        /// Obtiene el monto de descuento en moneda de captura
        /// </summary>
        public decimal descuento_moneda_captura { get { return this._descuento_moneda_captura; } }
        protected decimal _descuento_moneda_nacional;
        /// <summary>
        /// Obtiene el monto del descuento en moneda nacional
        /// </summary>
        public decimal descuento_moneda_nacional { get { return this._descuento_moneda_nacional; } }
        protected decimal _impuestos_moneda_captura;
        /// <summary>
        /// Obtiene el monto de impuestos en moneda de captura
        /// </summary>
        public decimal impuestos_moneda_captura { get { return this._impuestos_moneda_captura; } }
        protected decimal _impuestos_moneda_nacional;
        /// <summary>
        /// Obtiene el monto de impuestos en moneda nacional
        /// </summary>
        public decimal impuestos_moneda_nacional { get { return this._impuestos_moneda_nacional; } }
        protected decimal _total_moneda_captura;
        /// <summary>
        /// Obtiene el monto total del comprobante en moneda de captura
        /// </summary>
        public decimal total_moneda_captura { get { return this._total_moneda_captura; } }
        protected decimal _total_moneda_nacional;
        /// <summary>
        /// Obtiene el monto total del comprobante en moneda nacional
        /// </summary>
        public decimal total_moneda_nacional { get { return this._total_moneda_nacional; } }
        protected int _id_compania_emisor;
        /// <summary>
        /// Obtiene el Id del Emisor del Comprobante
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        protected int _id_direccion_emisor;
        /// <summary>
        /// Obtiene el Id de Ubicación del Emisor
        /// </summary>
        public int id_direccion_emisor { get { return this._id_direccion_emisor; } }
        protected int _id_sucursal;
        /// <summary>
        /// Obtiene el Id de Sucursal donde se generó el Comprobante
        /// </summary>
        public int id_sucursal { get { return this._id_sucursal; } }
        protected int _id_direccion_sucursal;
        /// <summary>
        /// Obtiene el Id de la Ubicación de la sucursal donde se generó el Comprobante
        /// </summary>
        public int id_direccion_sucursal { get { return this._id_direccion_sucursal; } }
        protected int _id_compania_receptor;
        /// <summary>
        /// Obtiene el Id de Receptor del Comprobante
        /// </summary>
        public int id_compania_receptor { get { return this._id_compania_receptor; } }
        protected int _id_direccion_receptor;
        /// <summary>
        /// Obtiene el Id de la Ubicación del Receptor del Comprobante
        /// </summary>
        public int id_direccion_receptor { get { return this._id_direccion_receptor; } }
        protected DateTime _fecha_captura;
        /// <summary>
        /// Obtiene ela fecha de captura del Comprobante
        /// </summary>
        public DateTime fecha_captura { get { return this._fecha_captura; } }
        protected DateTime _fecha_expedicion;
        /// <summary>
        /// Obtiene la fecha de expedición del Comprobante
        /// </summary>
        public DateTime fecha_expedicion { get { return this._fecha_expedicion; } }
        protected DateTime _fecha_cancelacion;
        /// <summary>
        /// Obtiene la fecha de Cancelación del Comprobante
        /// </summary>
        public DateTime fecha_cancelacion { get { return this._fecha_cancelacion; } }
        protected bool _generado;
        /// <summary>
        /// Obtiene el valor que determina si un Comprobante ya ha sido generado (Timbrado Fiscalmente)
        /// </summary>
        public bool generado { get { return this._generado; } }
        protected bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación de registro
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        protected bool _bit_transferido_nuevo;
        /// <summary>
        /// Obtiene el valor de Transferencia pendiente a sistema contable
        /// </summary>
        public bool bit_transferido_nuevo { get { return this._bit_transferido_nuevo; } }
        protected int _id_transferido_nuevo;
        /// <summary>
        /// Obtiene el Id de Transferencia pendiente a sistema contable
        /// </summary>
        public int id_transferido_nuevo { get { return this._id_transferido_nuevo; } }
        protected bool _bit_transferido_cancelado;
        /// <summary>
        /// Obtiene el valor de Transferencia pendiente de cancelación a sistema contable
        /// </summary>
        public bool bit_transferido_cancelado { get { return this._bit_transferido_cancelado; } }
        protected int _id_transferido_cancelado;
        /// <summary>
        /// Obtiene el valor de Id Transferencia pendiente de cancelación a sistema contable
        /// </summary>
        public int id_transferido_cancelado { get { return this._id_transferido_cancelado; } }
        protected string _lugar_expedicion;
        /// <summary>
        /// Obtiene el lugar de expedición del Comprobante
        /// </summary>
        public string lugar_expedicion { get { return this._lugar_expedicion; } }
        protected int _id_cuenta_pago;
        /// <summary>
        /// Obteiene el número de cuenta de pago del Comprobante
        /// </summary>
        public int id_cuenta_pago { get { return this._id_cuenta_pago; } }
        protected string _serie_folio_original;
        /// <summary>
        /// Obtiene el número de serie del Comprobante al que pertenece esta parcialidad
        /// </summary>
        public string serie_folio_original { get { return this._serie_folio_original; } }
        protected int _folio_original;
        /// <summary>
        /// Obtiene el número de folio del Comprobante al que pertenece esta parcialidad
        /// </summary>
        public int folio_original { get { return this._folio_original; } }
        protected DateTime _fecha_folio_original;
        /// <summary>
        /// Obtiene la fecha de expedición del Comprobante al que pertenece esta parcialidad
        /// </summary>
        public DateTime fecha_folio_original { get { return this._fecha_folio_original; } }
        protected decimal _monto_folio_original;
        /// <summary>
        /// Obtiene el monto total del Comprobante al que pertenece esta parcialidad
        /// </summary>
        public decimal monto_folio_original { get { return this._monto_folio_original; } }
        protected string _ruta_xml;
        /// <summary>
        /// Obtiene la ruta física de almacenamiento del archivo .xml del comprobante
        /// </summary>
        public string ruta_xml { get { return this._ruta_xml; } }

        protected string _ruta_codigo_bidimensional;
        /// <summary>
        /// Obtiene la ruta física de almacenamiento del archivo .xml del comprobante
        /// </summary>
        public string ruta_codigo_bidimensional { get { return this._ruta_codigo_bidimensional; } }


        #endregion

        #region Constructores

        /// <summary>
        /// Crea una instancia en blanco del tipo Comprobante
        /// </summary>
        protected Comprobante()
        {

        }
        /// <summary>
        /// Crea una instancia del tipo Comprobante a partir de un registro en BD
        /// </summary>
        /// <param name="id_comprobante">id comprobante</param>
        public Comprobante(int id_comprobante)
        {
            //cargando atributos de la instancia
            cargaAtributosInstancia(id_comprobante);
        }
        /// <summary>
        /// Crea una instancia del tipo Comprobante a partir de un emisor y número interno de comprobante
        /// </summary>
        /// <param name="rfc_emisor">RFC del Emisor</param>
        /// <param name="rfc_receptor">RFC del Receptor</param>
        /// <param name="serie">Serie</param>
        /// <param name="folio">Folio</param>
        public Comprobante(string rfc_emisor, string rfc_receptor, string serie, int folio)
        {
            //Inicializando parametros
            object[] parametros = { 10, 0, serie, folio, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0, null, null, null, 0, 0, 0, 0, 0, 0, 0, "", "", "", 0, null, 0, rfc_emisor, rfc_receptor };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorriendo las filas d ela tabla
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Llenando los campos
                        _id_comprobante = Convert.ToInt32(r["Id"]);
                        _serie = r["Serie"].ToString();
                        _folio = Convert.ToInt32(r["Folio"]);
                        _id_certificado = Convert.ToInt32(r["IdCertificado"]);
                        _id_tipo_comprobante = Convert.ToByte(r["IdTipo"]);
                        _id_origen_datos = Convert.ToByte(r["IdOrigen"]);
                        _id_estatus_comprobante = Convert.ToByte(r["IdEstatus"]);
                        _version = r["Version"].ToString();
                        _sello = r["Sello"].ToString();
                        _id_forma_pago = Convert.ToByte(r["IdFormaPago"]);
                        _id_condiciones_pago = Convert.ToByte(r["IdCondicionesPago"]);
                        _id_metodo_pago = Convert.ToByte(r["IdMetodoPago"]);
                        _no_parcialidad = Convert.ToInt32(r["NoParcialidad"]);
                        _total_parcialidades = Convert.ToInt32(r["TotalParcialidades"]);
                        _id_moneda = Convert.ToByte(r["IdMoneda"]);
                        _fecha_tipo_cambio = Convert.ToDateTime(r["FechaTipoCambio"]);
                        _subtotal_moneda_captura = Convert.ToDecimal(r["SubtotalMonedaCaptura"]);
                        _subtotal_moneda_nacional = Convert.ToDecimal(r["SubtotalMonedaNacional"]);
                        _descuento_moneda_captura = Convert.ToDecimal(r["DescuentoMonedaCaptura"]);
                        _descuento_moneda_nacional = Convert.ToDecimal(r["DescuentoMonedaNacional"]);
                        _impuestos_moneda_captura = Convert.ToDecimal(r["ImpuestosMonedaCaptura"]);
                        _impuestos_moneda_nacional = Convert.ToDecimal(r["ImpuestosMonedaNacional"]);
                        _total_moneda_captura = Convert.ToDecimal(r["TotalMonedaCaptura"]);
                        _total_moneda_nacional = Convert.ToDecimal(r["TotalMonedaNacional"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdEmisor"]);
                        _id_direccion_emisor = Convert.ToInt32(r["IdDireccionEmisor"]);
                        _id_sucursal = Convert.ToInt32(r["IdSucursal"]);
                        _id_direccion_sucursal = Convert.ToInt32(r["IdDireccionSucursal"]);
                        _id_compania_receptor = Convert.ToInt32(r["IdReceptor"]);
                        _id_direccion_receptor = Convert.ToInt32(r["IdDireccionReceptor"]);
                        DateTime.TryParse(r["FechaCaptura"].ToString(), out _fecha_captura);
                        DateTime.TryParse(r["FechaExpedicion"].ToString(), out _fecha_expedicion);
                        DateTime.TryParse(r["FechaCancelacion"].ToString(), out _fecha_cancelacion);
                        _generado = Convert.ToBoolean(r["Generado"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _bit_transferido_nuevo = Convert.ToBoolean(r["BitTransferidoNuevo"]);
                        _bit_transferido_cancelado = Convert.ToBoolean(r["BitTransferidoCancelado"]);
                        _id_transferido_nuevo = Convert.ToInt32(r["IdTransferidoNuevo"]);
                        _id_transferido_cancelado = Convert.ToInt32(r["IdTransferidoCancelado"]);
                        _lugar_expedicion = r["LugarExpedicion"].ToString();
                        _id_cuenta_pago = Convert.ToInt32(r["IdCuentaPago"]);
                        _serie_folio_original = r["SerieFolioFiscalOriginal"].ToString();
                        _folio_original = Convert.ToInt32(r["FolioFiscalOriginal"]);
                        DateTime.TryParse(r["FechaFolioFiscalOriginal"].ToString(), out _fecha_folio_original);
                        _monto_folio_original = Convert.ToDecimal(r["MontoFolioFiscalOriginal"]);
                        _ruta_xml = r["RutaArchivoXML"].ToString();
                    }
                }
            }
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Comprobante()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Carga los valores de la BD y los asigna a la instancia actual.
        /// </summary>
        /// <param name="id_comprobante">id comprobante</param>
        protected bool cargaAtributosInstancia(int id_comprobante)
        {
            //Definiendo objeto de retorno
            bool resultado = false;

            //Inicializando parametros
            object[] parametros = { 3, id_comprobante, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0, null, null, null,   false, false, 0,  false, 0,  "", 0, "", 0, null, 0, 0, false,"", "" };
             //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorriendo las filas d ela tabla
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Llenando los campos
                        _id_comprobante = Convert.ToInt32(r["Id"]);
                        _serie = r["Serie"].ToString();
                        _folio = Convert.ToInt32(r["Folio"]);
                        _id_certificado = Convert.ToInt32(r["IdCertificado"]);
                        _id_tipo_comprobante = Convert.ToByte(r["IdTipo"]);
                        _id_origen_datos = Convert.ToByte(r["IdOrigen"]);
                        _id_estatus_comprobante = Convert.ToByte(r["IdEstatus"]);
                        _version = r["Version"].ToString();
                        _sello = r["Sello"].ToString();
                        _id_forma_pago = Convert.ToByte(r["IdFormaPago"]);
                        _id_condiciones_pago = Convert.ToByte(r["IdCondicionesPago"]);
                        _id_metodo_pago = Convert.ToByte(r["IdMetodoPago"]);
                        _no_parcialidad = Convert.ToInt32(r["NoParcialidad"]);
                        _total_parcialidades = Convert.ToInt32(r["TotalParcialidades"]);
                        _id_moneda = Convert.ToByte(r["IdMoneda"]);
                        DateTime.TryParse(r["FechaTipoCambio"].ToString(), out _fecha_tipo_cambio);
                        _subtotal_moneda_captura = Convert.ToDecimal(r["SubtotalMonedaCaptura"]);
                        _subtotal_moneda_nacional = Convert.ToDecimal(r["SubtotalMonedaNacional"]);
                        _descuento_moneda_captura = Convert.ToDecimal(r["DescuentoMonedaCaptura"]);
                        _descuento_moneda_nacional = Convert.ToDecimal(r["DescuentoMonedaNacional"]);
                        _impuestos_moneda_captura = Convert.ToDecimal(r["ImpuestosMonedaCaptura"]);
                        _impuestos_moneda_nacional = Convert.ToDecimal(r["ImpuestosMonedaNacional"]);
                        _total_moneda_captura = Convert.ToDecimal(r["TotalMonedaCaptura"]);
                        _total_moneda_nacional = Convert.ToDecimal(r["TotalMonedaNacional"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdEmisor"]);
                        _id_direccion_emisor = Convert.ToInt32(r["IdDireccionEmisor"]);
                        _id_sucursal = Convert.ToInt32(r["IdSucursal"]);
                        _id_direccion_sucursal = Convert.ToInt32(r["IdDireccionSucursal"]);
                        _id_compania_receptor = Convert.ToInt32(r["IdReceptor"]);
                        _id_direccion_receptor = Convert.ToInt32(r["IdDireccionReceptor"]);
                        DateTime.TryParse(r["FechaCaptura"].ToString(), out _fecha_captura);
                        DateTime.TryParse(r["FechaExpedicion"].ToString(), out _fecha_expedicion);
                        DateTime.TryParse(r["FechaCancelacion"].ToString(), out _fecha_cancelacion);
                        _generado = Convert.ToBoolean(r["Generado"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _bit_transferido_nuevo = Convert.ToBoolean(r["BitTransferidoNuevo"]);
                        _bit_transferido_cancelado = Convert.ToBoolean(r["BitTransferidoCancelado"]);
                        _id_transferido_nuevo = Convert.ToInt32(r["IdTransferidoNuevo"]);
                        _id_transferido_cancelado = Convert.ToInt32(r["IdTransferidoCancelado"]);
                        _lugar_expedicion = r["LugarExpedicion"].ToString();
                        _id_cuenta_pago = Convert.ToInt32(r["IdCuentaPago"]);
                        _serie_folio_original = r["SerieFolioFiscalOriginal"].ToString();
                        _folio_original = Convert.ToInt32(r["FolioFiscalOriginal"]);
                        DateTime.TryParse(r["FechaFolioFiscalOriginal"].ToString(), out _fecha_folio_original);
                        _monto_folio_original = Convert.ToDecimal(r["MontoFolioFiscalOriginal"]);
                        _ruta_xml = r["RutaArchivoXML"].ToString();
                        _ruta_codigo_bidimensional = r["RutaCodigoBidimensional"].ToString();

                        resultado = true;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de todos los datos de un comprobante
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <param name="id_certificado"></param>
        /// <param name="tipo_comprobante"></param>
        /// <param name="id_origen_datos"></param>
        /// <param name="estatus_comprobante"></param>
        /// <param name="version"></param>
        /// <param name="sello"></param>
        /// <param name="id_forma_pago"></param>
        /// <param name="id_condiciones_pago"></param>
        /// <param name="id_metodo_pago"></param>
        /// <param name="no_parcialidad"></param>
        /// <param name="total_parcialidades"></param>
        ///<param name="id_moneda"></param>
        ///<param name="fecha_tipo_cambio"></param>
        /// <param name="subtotal_moneda_captura"></param>
        /// <param name="subtotal_moneda_nacional"></param>
        /// <param name="descuento_moneda_captura"></param>
        /// <param name="descuento_moneda_nacional"></param>
        /// <param name="impuestos_moneda_captura"></param>
        /// <param name="impuestos_moneda_nacional"></param>
        /// <param name="total_moneda_captura"></param>
        /// <param name="total_moneda_nacional"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_direccion_emisor"></param>
        /// <param name="id_sucursal"></param>
        /// <param name="id_direccion_sucursal"></param>
        /// <param name="id_compania_receptor"></param>
        /// <param name="id_direccion_receptor"></param>
        /// <param name="fecha_captura"></param>
        /// <param name="fecha_expedicion"></param>
        /// <param name="fecha_cancelacion"></param>
        /// <param name="generado"></param>
        /// <param name="bit_transferido_nuevo"></param>
        /// <param name="id_transferido_nuevo"></param>
        /// <param name="bit_transferido_cancelado"></param>
        /// <param name="id_transferido_cancelado"></param>
        /// <param name="lugar_expedicion"></param>
        /// <param name="id_cuenta_pago"></param>
        /// <param name="serie_folio_original"></param>
        /// <param name="folio_original"></param>
        /// <param name="fecha_folio_original"></param>
        /// <param name="monto_folio_original"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        protected RetornoOperacion editaComprobante(string serie, int folio, int id_certificado, TipoComprobante tipo_comprobante, byte id_origen_datos, EstatusComprobante estatus_comprobante, string version, string sello, byte id_forma_pago, byte id_condiciones_pago, byte id_metodo_pago,
                                                        int no_parcialidad, int total_parcialidades, byte id_moneda, DateTime  fecha_tipo_cambio, decimal subtotal_moneda_captura, decimal subtotal_moneda_nacional, decimal descuento_moneda_captura, decimal descuento_moneda_nacional,
                                                        decimal impuestos_moneda_captura, decimal impuestos_moneda_nacional, decimal total_moneda_captura, decimal total_moneda_nacional, int id_compania_emisor, int id_direccion_emisor, int id_sucursal, int id_direccion_sucursal,
                                                        int id_compania_receptor, int id_direccion_receptor, DateTime fecha_captura, DateTime fecha_expedicion, DateTime fecha_cancelacion, bool generado, bool bit_transferido_nuevo, int id_transferido_nuevo, bool bit_transferido_cancelado,
                                                        int id_transferido_cancelado, string lugar_expedicion, int id_cuenta_pago, string serie_folio_original, int folio_original, DateTime fecha_folio_original, decimal monto_folio_original, int id_usuario, bool habilitar)
        {
            //Definiendo resultado 
            RetornoOperacion resultado = new RetornoOperacion();

            //Realziando validación de generación de Comprobante
            if (!this._generado)
            {
                //Inicializando parametros
                object[] parametros = { 2, this._id_comprobante, serie, folio, id_certificado, (byte)tipo_comprobante, id_origen_datos, (byte)estatus_comprobante, version, sello, id_forma_pago, id_condiciones_pago, id_metodo_pago, no_parcialidad, total_parcialidades, id_moneda , Fecha.ConvierteDateTimeObjeto(fecha_tipo_cambio), 
                                      subtotal_moneda_captura, subtotal_moneda_nacional, descuento_moneda_captura, descuento_moneda_nacional, impuestos_moneda_captura, impuestos_moneda_nacional, total_moneda_captura, total_moneda_nacional, id_compania_emisor, id_direccion_emisor, 
                                      id_sucursal, id_direccion_sucursal, id_compania_receptor, id_direccion_receptor, fecha_captura,  Fecha.ConvierteDateTimeObjeto(fecha_expedicion), Fecha.ConvierteDateTimeObjeto(fecha_cancelacion), generado, 
                                      bit_transferido_nuevo, id_transferido_nuevo, bit_transferido_cancelado, id_transferido_cancelado, lugar_expedicion, id_cuenta_pago, serie_folio_original, folio_original, Fecha.ConvierteDateTimeObjeto(fecha_folio_original), monto_folio_original, id_usuario, habilitar ,"", "" };

                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, parametros);
            }
            else
                //Indicando que el estatus de generación impide actualizar
                resultado = new RetornoOperacion("Imposible editar, el comprobante ya ha sido timbrado.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza Transferencia Comprobante
        /// </summary>
        /// <param name="bit_actualiza_nuevo"></param>
        /// <param name="bit_actualiza_cancela"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        protected RetornoOperacion actualizaTransferenciaComprobante(bool bit_actualiza_nuevo, bool bit_actualiza_cancela,
                                                                        int id_usuario)
        {
            //Definiendo resultado 
            RetornoOperacion resultado = new RetornoOperacion();


            //Inicializando parametros
            object[] parametros = { 2, this._id_comprobante, this._serie, this._folio, this._id_certificado, this._id_tipo_comprobante, this._id_origen_datos, (byte)this._id_estatus_comprobante, this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, Fecha.ConvierteDateTimeObjeto(this._fecha_tipo_cambio),
                                      this._subtotal_moneda_captura, this._subtotal_moneda_nacional, this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, 
                                      this._id_sucursal, this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura, Fecha.ConvierteDateTimeObjeto(this._fecha_expedicion), Fecha.ConvierteDateTimeObjeto(this._fecha_cancelacion), this._generado, id_usuario, this._habilitar, 
                                      bit_actualiza_nuevo, bit_actualiza_cancela, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original, Fecha.ConvierteDateTimeObjeto(this._fecha_folio_original), this._monto_folio_original, "", "" };

            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, parametros);


            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza Transferencia Comprobante
        /// </summary>
        /// <param name="bit_transferido_nuevo"></param>
        /// <param name="id_transferido_nuevo"></param>
        /// <param name="bit_transferido_cancelado"></param>
        /// <param name="id_transferido_cancelado"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        protected RetornoOperacion actualizaTransferenciaComprobante(bool bit_transferido_nuevo, int id_transferido_nuevo, bool bit_transferido_cancelado, int id_transferido_cancelado,
                                                                        int id_usuario)
        {
            //Definiendo resultado 
            RetornoOperacion resultado = new RetornoOperacion();


            //Inicializando parametros
            object[] parametros = { 2, this._id_comprobante, this._serie, this._folio, this._id_certificado, this._id_tipo_comprobante, this._id_origen_datos, (byte)this._id_estatus_comprobante, this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, Fecha.ConvierteDateTimeObjeto(this._fecha_tipo_cambio),
                                      this._subtotal_moneda_captura, this._subtotal_moneda_nacional, this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, 
                                      this._id_sucursal, this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura, Fecha.ConvierteDateTimeObjeto(this._fecha_expedicion), Fecha.ConvierteDateTimeObjeto(this._fecha_cancelacion), this._generado, 
                                      bit_transferido_nuevo,  id_transferido_nuevo, bit_transferido_cancelado, id_transferido_cancelado, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original, Fecha.ConvierteDateTimeObjeto(this._fecha_folio_original), this._monto_folio_original,id_usuario, this._habilitar,"", "" };

            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, parametros);


            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene el número de folio por asignar a un comprobnate a apartir de el emisor y serie solicitada
        /// </summary>
        /// <param name="id_compania_emisor">Id de Emisor</param>
        /// <param name="serie">Serie del Folio</param>
        /// <returns></returns>
        protected static int obtieneFolioPorAsignar(int id_compania_emisor, string serie)
        {

            //Inicializando parametros
            object[] parametros = { 4, 0, serie, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0,0, id_compania_emisor, 0, 0, 0, 0, 0, null, null, null, false, false, 0, false, 0, "", 0, "", 0, null, 0, 0, false, "", "" };
                                 // 3, 0     , "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 0,                 0, 0, 0, 0,0, null, null, null,  false, false, 0, false, 0, "", 0, "", 0, null, 0, 0, false,"", "" };
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, parametros).IdRegistro;
        }
       
        /// <summary>
        /// Realiza el consumo de webservice para timbrar el documento xml solicitado
        /// </summary>
        /// <param name="documento">Documento XML del Comprobante a Timbrar</param>
        /// <param name="ns_SAT">Namespace al que pertenece el elemento Complemento, del cual se recuperará la información de timbrado</param>
        /// <param name="id_usuario">Id de Usuario que solicita el timbrado</param>
        /// <param name="transaccion">Transacción</param>
        /// <returns></returns>
        protected RetornoOperacion generaTimbreFiscalDigital(ref XDocument documento, XNamespace ns_SAT, int id_usuario)
        { 
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Recuperando bytes del comprobante
            byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);
            

            //Recuperando bytes del comprobante timbrado
            XDocument xml_comprobante_recuperado = null;
            int id_compania_pac = 0;

            //Traduciendo resultado
            resultado = PacCompaniaEmisor.GeneraTimbrePAC(documento, bytes_comprobante, out xml_comprobante_recuperado, out id_compania_pac, this._id_compania_emisor);

            //Si no existe error
            if (resultado.OperacionExitosa)
            {
                //Instanciando nuevo documento a partir del xml recuperado
                documento = xml_comprobante_recuperado;

                //Recuperando Namespace del Timbre Fiscal
                XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");

                //Recuperación de nodo Timbre Fiscal Digital
                XElement timbre = documento.Root.Element(ns_SAT + "Complemento").Element(ns + "TimbreFiscalDigital");

                //Si el nodo fue recuperado
                if (timbre != null)
                    //Realizando guardado de Timbre en BD
                    resultado = TimbreFiscalDigital.InsertarTimbreFiscal(this._id_comprobante, id_compania_pac, timbre.Attribute("version").Value, timbre.Attribute("UUID").Value,
                                                                Convert.ToDateTime(timbre.Attribute("FechaTimbrado").Value), timbre.Attribute("selloCFD").Value,
                                                                timbre.Attribute("noCertificadoSAT").Value, timbre.Attribute("selloSAT").Value, id_usuario);
                else
                    resultado = new RetornoOperacion("Imposible recuperar datos de timbre desde el comprobante.");
            }

            //Devolviendo resultado
            return resultado;
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
        protected RetornoOperacion validaRequisitosPreviosTimbrado(string serie, out int folio, out int id_certificado_activo,
                                        out string numero_certificado, out string certificado_base_64, out string contrasena_apertura,
                                        out byte[] bytes_certificado, SerieFolio.tipo tipoFolioSerie)
        {
            //Inicializando valores de parámetros de salida
            numero_certificado = certificado_base_64 = contrasena_apertura = "";
            folio = id_certificado_activo = 0;
            bytes_certificado = null;

            //Defiiniendo auxiliares para obtenciaón de certificado
            int id_compania_emisor = 0, id_sucursal = 0;

            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);

             //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que el comprobante no esté timbrado
                if (!this._generado)
                {
                    //Validando la fecha de cambio(a la fecha de timbrado)
                    if (this._id_moneda != 1)
                    {
                        //Innstanciamos Tipo de Cambio
                        using (TipoCambio tc = new TipoCambio(this._id_compania_emisor, this._id_moneda, DateTime.Today, 0))
                        {
                            //Si el tipo de cambio fue localizado
                            if (tc.id_tipo_cambio > 0)
                            {
                                //Validando fecha, en caso de ser distinta mandar error
                                if (tc.fecha.Date.CompareTo(DateTime.Today) != 0)
                                    resultado = new RetornoOperacion("La fecha del Tipo de Cambio debe ser igual a la fecha actual.");
                            }
                            else
                                resultado = new RetornoOperacion("Tipo de Cambio no encontrado.");
                        }
                    }

                    //Si no existen errores hasta aqui
                    if (resultado.OperacionExitosa)
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
                                            resultado = new RetornoOperacion("La sucursal no se encuentra activa.");

                                        //Asignando Id de Sucursal
                                        id_sucursal = s.id_sucursal;
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion("El emisor no se encuentra activo.");

                            //Asignando Id de Emisor
                            id_compania_emisor = em.id_compania_emisor_receptor;

                            //Si no existe error
                            if (resultado.OperacionExitosa)
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
                                                    resultado = new RetornoOperacion("Este certificado no pertence al emisor que sellará el comprobante.");
                                            }
                                            else
                                                resultado = new RetornoOperacion("Imposible recuperar los datos de Propietario del Certificado.");
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("No existe un Certificado de Sello Digital activo.");
                                }
                            }

                            //Si no hay erroes
                            if (resultado.OperacionExitosa)
                            {
                                //Validamos que exista referencia
                                if (em.FacturacionElectronica == null)
                                {
                                    resultado = new RetornoOperacion("El Emisor no tiene ningún regimen asignado.");
                                }
                                else
                                    //Validamos que exista la Clave
                                    if (!em.FacturacionElectronica.ContainsKey("Regimen Fiscal"))
                                    {
                                        resultado = new RetornoOperacion("El Emisor no tiene ningún regimen asignado.");
                                    }
                                    else if (em.FacturacionElectronica["Regimen Fiscal"] == "")
                                    {
                                        resultado = new RetornoOperacion("El Emisor no tiene ningún regimen asignado.");
                                    }
                            }

                            //Si no hay erroes
                            if (resultado.OperacionExitosa)
                            {
                                //Validando existencia de al menos un concepto
                                using (DataTable conceptos = Concepto.RecuperaConceptosComprobantes(this._id_comprobante))
                                {
                                    //SI NO existen registros
                                    if (!Validacion.ValidaOrigenDatos(conceptos))
                                    {
                                        resultado = new RetornoOperacion("No hay conceptos por facturar.");
                                    }
                                }
                            }

                            //Si no existe error
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos emisor y serie para obtener el tipo definido
                                using (SerieFolio objSerieFolio = new SerieFolio(serie, this._id_compania_emisor))
                                {
                                    ///Validamos el Tipo de Serie
                                    if (objSerieFolio.estatusTipo == tipoFolioSerie)
                                    {
                                        //Realizando búsqueda de folio por asignar
                                        folio = obtieneFolioPorAsignar(this._id_compania_emisor, serie);
                                        //Si no existe un folio disponible
                                        if (folio <= 0)
                                            resultado = new RetornoOperacion(string.Format("No existe un folio disponible para la serie {0}.", serie.ToUpper()));
                                    }
                                    else
                                    {
                                        resultado = new RetornoOperacion(string.Format("La serie no se encuentra disponible {0}.", serie));

                                    }

                                }
                            }
                        }
                    }
                }
                else
                    resultado = new RetornoOperacion("El comprobante ya se ha timbrado anteriormente.");
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
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
        protected RetornoOperacion validaRequisitosPreviosTimbradoNomina(string serie, out int folio, out int id_certificado_activo,
                                        out string numero_certificado, out string certificado_base_64, out string contrasena_apertura,
                                        out byte[] bytes_certificado, SerieFolio.tipo tipoFolioSerie)
        {
            //Inicializando valores de parámetros de salida
            numero_certificado = certificado_base_64 = contrasena_apertura = "";
            folio = id_certificado_activo = 0;
            bytes_certificado = null;

            //Defiiniendo auxiliares para obtenciaón de certificado
            int id_compania_emisor = 0, id_sucursal = 0;

            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que el comprobante no esté timbrado
                if (!this._generado)
                {
                    //Validando la fecha de cambio(a la fecha de timbrado)
                    if (this._id_moneda != 1)
                    {
                        //Innstanciamos Tipo de Cambio
                        using (TipoCambio tc = new TipoCambio(this._id_compania_emisor, this._id_moneda, DateTime.Today, 0))
                        {
                            //Si el tipo de cambio fue localizado
                            if (tc.id_tipo_cambio > 0)
                            {
                                //Validando fecha, en caso de ser distinta mandar error
                                if (tc.fecha.Date.CompareTo(DateTime.Today) != 0)
                                    resultado = new RetornoOperacion("La fecha del Tipo de Cambio debe ser igual a la fecha actual.");
                            }
                            else
                                resultado = new RetornoOperacion("Tipo de Cambio no encontrado.");
                        }
                    }

                    //Si no existen errores hasta aqui
                    if (resultado.OperacionExitosa)
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
                                            resultado = new RetornoOperacion("La sucursal no se encuentra activa.");

                                        //Asignando Id de Sucursal
                                        id_sucursal = s.id_sucursal;
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion("El emisor no se encuentra activo.");

                            //Asignando Id de Emisor
                            id_compania_emisor = em.id_compania_emisor_receptor;

                            //Si no existe error
                            if (resultado.OperacionExitosa)
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
                                                    //Validando fechas del certificado
                                                    if (cer.FinValidez.CompareTo(Fecha.ObtieneFechaEstandarMexicoCentro()) >= 0)
                                                    {
                                                        //Asignando parámteros de salida
                                                        id_certificado_activo = certificado.id_certificado_digital;
                                                        numero_certificado = cer.No_Serie;
                                                        certificado_base_64 = cer.CertificadoBase64;
                                                        contrasena_apertura = certificado.contrasena_desencriptada;
                                                        bytes_certificado = System.IO.File.ReadAllBytes(certificado.ruta_llave_privada);
                                                    }
                                                    else
                                                        resultado = new RetornoOperacion(string.Format("El certificado ha expirado: {0:dd/MM/yyyy}.", cer.FinValidez));
                                                }
                                                else
                                                    resultado = new RetornoOperacion("Este certificado no pertence al emisor que sellará el comprobante.");
                                            }
                                            else
                                                resultado = new RetornoOperacion("Imposible recuperar los datos de Propietario del Certificado.");
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("No existe un Certificado de Sello Digital activo.");
                                }
                            }

                            //Si no hay erroes
                            if (resultado.OperacionExitosa)
                            {
                                //Validamos que exista referencia
                                if (em.FacturacionElectronica == null)
                                {
                                    resultado = new RetornoOperacion("El Emisor no tiene ningún regimen asignado.");
                                }
                                else
                                    //Validamos que exista la Clave
                                    if (!em.FacturacionElectronica.ContainsKey("Regimen Fiscal Nomina(Clave)"))
                                    {
                                        resultado = new RetornoOperacion("El Emisor no tiene ningún regimen asignado.");
                                    }
                                    else if (em.FacturacionElectronica["Regimen Fiscal Nomina(Clave)"] == "")
                                    {
                                        resultado = new RetornoOperacion("El Emisor no tiene ningún regimen asignado.");
                                    }
                            }

                            //Si no hay erroes
                            if (resultado.OperacionExitosa)
                            {
                                //Validando existencia de al menos un concepto
                                using (DataTable conceptos = Concepto.RecuperaConceptosComprobantes(this._id_comprobante))
                                {
                                    //SI NO existen registros
                                    if (!Validacion.ValidaOrigenDatos(conceptos))
                                    {
                                        resultado = new RetornoOperacion("No hay conceptos por facturar.");
                                    }
                                }
                            }

                            //Si no existe error
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos emisor y serie para obtener el tipo definido
                                using (SerieFolio objSerieFolio = new SerieFolio(serie, this._id_compania_emisor))
                                {
                                    ///Validamos el Tipo de Serie
                                    if (objSerieFolio.estatusTipo == tipoFolioSerie)
                                    {
                                        //Realizando búsqueda de folio por asignar
                                        folio = obtieneFolioPorAsignar(this._id_compania_emisor, serie);
                                        //Si no existe un folio disponible
                                        if (folio <= 0)
                                            resultado = new RetornoOperacion(string.Format("No existe un folio disponible para la serie {0}.", serie.ToUpper()));
                                    }
                                    else
                                    {
                                        resultado = new RetornoOperacion(string.Format("La serie no se encuentra disponible {0}.", serie));

                                    }

                                }
                            }
                        }
                    }
                }
                else
                    resultado = new RetornoOperacion("El comprobante ya se ha timbrado anteriormente.");
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la modificacion del contenido en el archivo del CFD para omitir los acentos del documento
        /// </summary>
        /// <param name="xml_comprobante">Cadena xml con el contenido del comprobante</param>
        protected string suprimeCaracteresAcentuadosCFD(string xml_comprobante)
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
        /// Realiza la actualización de los datos Serie, Folio, Fecha de Expedición y Certificado utilizado para sellar comprobante
        /// </summary>
        /// <param name="serie">Serie del Comprobante</param>
        /// <param name="folio">Folio del Comprobante</param>
        /// <param name="id_certificado">Id de Certificado con que será sellado</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        protected RetornoOperacion asignaFolioComprobante(string serie, int folio, int id_certificado, int id_usuario)
        {
            //Actialziando datos
            return editaComprobante(serie, folio, id_certificado, this.tipo_comprobante, this._id_origen_datos, this.estatus_comprobante, this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, this._fecha_tipo_cambio, this._subtotal_moneda_captura, this._subtotal_moneda_nacional,
                    this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, this._id_sucursal, this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura,
                    Fecha.ObtieneFechaEstandarMexicoCentro(), this._fecha_cancelacion,this._generado,this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_cancelado, this._id_transferido_cancelado, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original, this._fecha_folio_original, this._monto_folio_original, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Actualiza el sello digital del comprobante
        /// </summary>
        /// <param name="sello_digital">Sello digital</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        protected RetornoOperacion actualizaSelloDigital(string sello_digital, int id_usuario)
        {
            return editaComprobante(this._serie, this._folio, this._id_certificado, this.tipo_comprobante, this._id_origen_datos, this.estatus_comprobante, this._version, sello_digital, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, this._fecha_tipo_cambio, this._subtotal_moneda_captura, this._subtotal_moneda_nacional,
                        this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, this._id_sucursal, this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura,
                        this._fecha_expedicion, this._fecha_cancelacion, true, true, this._id_transferido_nuevo, false,  this._id_transferido_cancelado, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original, this._fecha_folio_original, this._monto_folio_original , id_usuario, this._habilitar);
        }

        
        /// <summary>
        /// Método que obtiene el flujo de la transformación de un archivo CFD en formato .xml mediante un archivo de transformación .xslt
        /// </summary>
        /// <param name="flujoR">Flujo con el resultado de la transformación</param>
        /// <param name="ruta_xslt_cfdi">Ruta física del archivo de transformación que se aplicará al CFDI (.xslt)</param>
        /// <param name="ruta_xslt_co">Ruta física del archivo de transformación que se aplicará a la Cadena Original (.xslt)</param>
        /// <param name="ruta_xslt_co_alterna">Ruta física del archivo de transformación que se aplicará a la Candena Original Alterna (.xslt)</param>
        /// <param name="ruta_css_cfdi">Ruta física del archivo de Estilos en Cascada (CSS) que se aplicará a la transformación del CFDI</param>
        /// <returns></returns>
        protected RetornoOperacion transformaCFDXML(out MemoryStream flujoR, string ruta_xslt_cfdi, string ruta_xslt_co, string ruta_xslt_co_alterna, string ruta_css_cfdi)
        {
            //Creando un flujo en memoria para contener el resultado HTML de la transformación
            flujoR = null;
            //Declaramos Objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion();
            //Validando que el comprobante ya tenga un archivo XML generado
            if (this._ruta_xml != "")
            {
                //Instanciando al emisor del comprobante
                using (CompaniaEmisorReceptor emisor = new CompaniaEmisorReceptor(this._id_compania_emisor))
                {
                    //Instanciando un nuevo archivo de transformación
                    XslCompiledTransform archivoXSLT = new XslCompiledTransform();

                    try
                    {
                        //Declaramos Parametros 
                        XsltArgumentList parametros;

                        //Cargando el archivo de transformación a utilizar 
                        archivoXSLT.Load(ruta_xslt_cfdi);

                        //Inicialziando parámetros que se incluirán durante la transformación
                        resultado = inicializaParametrosXSLT(out parametros, emisor, ruta_css_cfdi, ruta_xslt_co, ruta_xslt_co_alterna);

                        //Validamos Resultado de la asignación de parametros
                        if (resultado.OperacionExitosa)
                        {
                            //Creando un flujo en memoria para contener el resultado HTML de la transformación
                            MemoryStream flujo = new MemoryStream();

                            //Se procesará el archivo .xml del CFD con el .xslt, y escribirá el resultado en el flujo de memoria
                            archivoXSLT.Transform(new XPathDocument(string.Format(@"{0}", this._ruta_xml)), parametros, flujo);
                            //Asignamos Valor
                            flujoR = flujo;

                        }
                    }
                    //Cachando excepciones por error en Hoja de Estilo
                    catch (XsltException exErrorHojaEstilos)
                    {
                        //Actualizando mensaje de resultado
                        resultado = new RetornoOperacion("Error Hojas de Estilo:" + exErrorHojaEstilos.ToString());


                    }
                    //Para cualquier otro tipo de excepción
                    catch (Exception ex)
                    {
                        //Actualizando mensaje de resultado
                        resultado = new RetornoOperacion("Error Hojas de Estilo:" + ex.ToString());

                    }
                }
            }
            else
            {

                //Asignando error de inexistancia de CFD
                resultado = new RetornoOperacion("No Existe Archivo XML");
            }

            //Devolvinedo resultado erroneo de generación
            return resultado;
        }
        
        /// <summary>
        /// Método que devuelve un arreglo de parámetros que serán asignados a un archivo de transformación XSLT
        /// </summary>
        /// <param name="emisor">Emisor</param>
        /// <param name="rutaArchivoCSS">Ruta relativa donde se localiza la hoja de estilos(.css) que se aplicará al documento HTML</param>
        ///<param name="rutaXSLTCadenaOriginal">Ruta relativa donde se localiza el archivo de transformación(.xslt) de la cadena original</param>
        ///<param name="rutaXSLTCadenaOriginalAlternativa">Ruta relativa donde se localiza el archivo de transformación(.xslt) de la cadena original en modo desconectado</param>
        /// <returns></returns>
        protected RetornoOperacion inicializaParametrosXSLT(out XsltArgumentList parametrosT, CompaniaEmisorReceptor emisor, string rutaArchivoCSS, string rutaXSLTCadenaOriginal, string rutaXSLTCadenaOriginalAlternativa)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            parametrosT = null;
            //Declarando variables para almacenar rutas requeridas para uso dentro de un flujo HTML 
            //Intsnaciamos Barra bidimensional 

            using (TimbreFiscalDigital objTimbre = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante))
            {
                //Si no existe el logo fisicamente
                if (File.Exists(this._ruta_codigo_bidimensional))
                {
                    string cadenaOriginal = "";

                    resultado = SelloDigitalSAT.CadenaCFD(this._ruta_xml, rutaXSLTCadenaOriginal,
                                                                     rutaXSLTCadenaOriginalAlternativa, out cadenaOriginal);

                    //Validamos Generacion de Cadena Original

                    if (resultado.OperacionExitosa)
                    {
                        //Obteniendo Cadena Original en formato adecuado para impresión
                        string co = Cadena.FragmentaCadena(cadenaOriginal, 127, "<br />");

                        //Obteniendo Cadena Original en formato adecuado para impresión
                        string seSAT = Cadena.FragmentaCadena(objTimbre.sello_SAT, 127, "<br />");

                        //Obteneindo Sello Digital en formato para impresión
                        string sd = Cadena.FragmentaCadena(this._sello, 127, "<br />");

                        //Obteniendo las referencias del registro
                        DataTable referencias = null;/* vane clReferencia.CargaReferenciasExtraCFD(this._id_comprobante);*/

                        //Obteniendo la Tasa de IVA Retenido desde el catálogo de variables en BD
                        string tasaIVARetenido = Impuesto.TasaIvaRetenido(this._id_comprobante);

                        //Declarando parámetros auxiliares para referencias, leyendas de impresión
                        string remitente = "", consignatario = "",
                                ref1 = "", ref2 = "", ref3 = "", ref4 = "",
                                ref5 = "", ref6 = "", ref7 = "", ref8 = "";

                        //Obteniendo las leyendas requeridas
                        string[] leyendasExtra = emisor.LeyendasImpresionCFD();
                        string textoLeyenda = "Este documento es una representación impresa de un CFDI.";

                        //Verificando que tanto la cadena original como el sello no sean cadena vacías
                        if (!string.IsNullOrEmpty(co) && !string.IsNullOrEmpty(sd))
                        {
                            //Añadiendo a listado de parámetros los elementos:            
                            //Definienedo variables de moneda a utilizar
                            string monedaE = "PESOS", monedaD = "MN";

                            //Si el CFD tiene un tipo de cambio asignado
                            /*vane if (this._id_tipo_cambio > 0)
                             {
                                 //Instanciando el TC
                                 using (TipoCambio tc = new TipoCambio(this._id_tipo_cambio))
                                 {
                                     if (tc.id_moneda > 0)
                                     { 
                                         //Recuperando descripción de moneda utilizada
                                         monedaE = Catalogo.RegresaDescripcionCatalogo(11, tc.id_moneda);
                                         monedaD = Catalogo.RegresaDescripcioValorCadena(11, tc.id_moneda);
                                         //Separando elementos de moneda
                                         monedaE = Cadena.RegresaCadenaSeparada(monedaE, " (", 0).ToUpper();
                                     }
                                 }
                             }*/
                            //Declarando listado de parámetros de retorno
                            XsltArgumentList parametrosTransformacion = new XsltArgumentList();

                            parametrosTransformacion.AddParam("montoLetra", "", Cadena.ConvierteMontoALetra(Math.Round(this._total_moneda_captura, 2).ToString(), monedaE, monedaD));
                            //Ruta absoluta(formato ../) de la Hoja de Estilos documento HTML 
                            parametrosTransformacion.AddParam("hojaEstilosCSS", "", "data:text/css;base64," + Convert.ToBase64String(File.ReadAllBytes(rutaArchivoCSS)));
                            //Ruta absoluta(formato ../) de la Imagen Logo Emisor creada
                            parametrosTransformacion.AddParam("imagenLogoEmisor", "", "data:image/jpg;base64," + Convert.ToBase64String(File.ReadAllBytes(this._ruta_codigo_bidimensional)));
                            //Serie y Folio Internos
                            parametrosTransformacion.AddParam("serie_folio_interno", "", string.Format("{0} - {1}", this._serie, this.folio));
                            //Cadena Original
                            parametrosTransformacion.AddParam("cadenaOriginal", "", co);
                            //Sello Digital
                            parametrosTransformacion.AddParam("selloDigital", "", sd);
                            //Sello Digital SAT
                            parametrosTransformacion.AddParam("selloSAT", "", seSAT);

                            //Si existen referencias que añadir
                            if (Validacion.ValidaOrigenDatos(referencias))
                            {
                                //Asignando los valores a sus variables correspondientes
                                remitente = (from DataRow f in referencias.Rows
                                             where f.Field<string>("IdTipoAlterno") == "SHPM"
                                             select f.Field<string>("Valor")).LastOrDefault<string>();

                                if (remitente == null)
                                    remitente = "";

                                consignatario = (from DataRow f in referencias.Rows
                                                 where f.Field<string>("IdTipoAlterno") == "CNSG"
                                                 select f.Field<string>("Valor")).LastOrDefault<string>();

                                if (consignatario == null)
                                    consignatario = "";

                                string[] otrasReferencias = (from DataRow f in referencias.Rows
                                                             where f.Field<string>("IdTipoAlterno") != "CNSG" && f.Field<string>("IdTipoAlterno") != "SHPM"
                                                             select f.Field<string>("Descripcion") + ": " + f.Field<string>("Valor")).ToArray<string>();


                                //Si existen más referencias
                                if (otrasReferencias != null)
                                {
                                    //Tratando de asignar los valores
                                    try { ref1 = otrasReferencias[0]; }
                                    catch (Exception) { }
                                    try { ref2 = otrasReferencias[1]; }
                                    catch (Exception) { }
                                    try { ref3 = otrasReferencias[2]; }
                                    catch (Exception) { }
                                    try { ref4 = otrasReferencias[3]; }
                                    catch (Exception) { }
                                    try { ref5 = otrasReferencias[4]; }
                                    catch (Exception) { }
                                    try { ref6 = otrasReferencias[5]; }
                                    catch (Exception) { }
                                    try { ref7 = otrasReferencias[6]; }
                                    catch (Exception) { }
                                    try { ref8 = otrasReferencias[7]; }
                                    catch (Exception) { }
                                }
                            }

                            //PARÁMETROS PARA REFERENCIAS EXTRA
                            //Origen
                            parametrosTransformacion.AddParam("lugarOrigen", "", remitente);
                            //Destino
                            parametrosTransformacion.AddParam("lugarDestino", "", consignatario);
                            //Referencia 1
                            parametrosTransformacion.AddParam("referencia1", "", ref1);
                            //Referencia 2
                            parametrosTransformacion.AddParam("referencia2", "", ref2);
                            //Referencia 3
                            parametrosTransformacion.AddParam("referencia3", "", ref3);
                            //Referencia 4
                            parametrosTransformacion.AddParam("referencia4", "", ref4);
                            //Referencia 5
                            parametrosTransformacion.AddParam("referencia5", "", ref5);
                            //Referencia 6
                            parametrosTransformacion.AddParam("referencia6", "", ref6);
                            //Referencia 7
                            parametrosTransformacion.AddParam("referencia7", "", ref7);
                            //Referencia 8
                            parametrosTransformacion.AddParam("referencia8", "", ref8);

                            //Añadiendo tasa de IVA Retenido (no aplica su uso en todos los casos)
                            parametrosTransformacion.AddParam("tasaIVARetenido", "", tasaIVARetenido);

                            //Determinando si la empresa que genera el CFD requiere añadir leyendas extra al pie de impresión
                            if (leyendasExtra != null)
                            {
                                //Recorriendo las leyendas devuletas
                                foreach (string l in leyendasExtra)
                                {
                                    textoLeyenda += " | " + l;
                                }
                            }

                            //Leyenda al pie de impresión
                            parametrosTransformacion.AddParam("leyendasExtra", "", textoLeyenda);
                            parametrosT = parametrosTransformacion;
                        }
                    }
                    else
                    {
                        resultado = new RetornoOperacion("No se puede generar cadenaOriginal");
                    }
                }


                else
                {
                    resultado = new RetornoOperacion("No existe Codigo Bidimensional asignado");
                }
            }

            //Devolvinedo arreglo de parámetros
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

                /*//Creando ruta de guardado del comprobante
                string ruta_codigo = string.Format(@"{0}{1}QR\{2}\{3}\{4}\{5}{6}.jpeg", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Unidad Almacenamiento CFD", 0),
                                                                         em.DirectorioAlmacenamiento, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year.ToString("0000"), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month.ToString("00"),
                                                                         TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Day.ToString("00"), serie, folio);*/
                //Creando ruta de guardado del comprobante
                string ruta_codigo = string.Format(@"{0}{1}\{2}\QR\{3}{4}.jpeg", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 119.ToString("0000"),
                                                          em.DirectorioAlmacenamiento, serie, folio);

                //Eliminando archivo si es que ya existe
                Archivo.EliminaArchivo(ruta_codigo);

                //Instanciamos Receptor
                using (CompaniaEmisorReceptor rec = new CompaniaEmisorReceptor(this._id_compania_receptor))
                {
                    using (TimbreFiscalDigital objTimbreFiscal = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante))
                    {
                        //Generamos Codigo Bidimensional
                        byte[] ArchivoBytes = Dibujo.GeneraCodigoBidimensional("?re=" + em.rfc + "&rr=" + rec.rfc + "&tt=" + this._total_moneda_captura + "&id=" + objTimbreFiscal.UUID, ImageFormat.Jpeg);

                        //Validamos Obtención de Bytes
                        if (ArchivoBytes != null)
                        {
                            //Añadimos Archivo
                            resultado = ArchivoRegistro.InsertaArchivoRegistro(119, this._id_comprobante, 17, "", id_usuario, ArchivoBytes, ruta_codigo);
                        }
                        else
                        {
                            resultado = new RetornoOperacion("No se puede cargar bytes (QR)");
                        }
                    }
                }
            }

            return resultado;

        }
        
        /// <summary>
        /// Añade las addendas definidas para el receptor y emisor del comprobante
        /// </summary>
        /// <param name="comprobante">Elemento xml a editar</param>
        /// <param name="ns_sat">Namespace del SAT para asociar elementos contenedores de complemento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        protected RetornoOperacion anadeElementoComplementoComprobante(ref XElement comprobante, XNamespace ns_sat, int id_usuario)
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante);

                //Cargando las addendas aplicables
                using (DataTable addendas = AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor))
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
                            RetornoOperacion r = new RetornoOperacion(this._id_comprobante);

                            //Recuperando la addenda aplicable al comprobante
                            using (AddendaComprobante ac = AddendaComprobante.RecuperaAddendaComprobante(Convert.ToInt32(a["Id"]), this._id_comprobante))
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
                                            using (Addenda ad = new Addenda(Convert.ToInt32(a["IdAddenda"])))
                                            {
                                                //Validando carga correcta de registro
                                                if (ad.id_addenda > 0)
                                                {
                                                    //Definiendo objeto para almacenamiento del contenido de addendas 
                                                    //(se añade debido a que el contenido de este nodo no está regulado bajo ningún estándar 
                                                    //y puede ser que un addenda no contenga un elemento raíz)
                                                    object obj_elemento_ac = elemento_ac;

                                                    //Creando addenda personalizada (en caso de estar definida para el receptor)
                                                    r = AddendaEmisor.CreaAddendaReceptor(ref obj_elemento_ac, this);

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
        /// Añade las addendas definidas para el receptor y emisor del comprobante
        /// </summary>
        /// <param name="comprobante">Elemento xml a editar</param>
        /// <param name="ns_sat">Namespace del SAT para asociar elementos contenedores de complemento</param>
        /// <returns></returns>
        protected RetornoOperacion anadeElementoComplementoReciboNomina(string version, ref XElement comprobante, XNamespace ns_sat)
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante);

            //Cargando las addendas aplicables
            using (DataTable addendas = AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor,"Nómina "+ version))
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
                        RetornoOperacion r = new RetornoOperacion(this._id_comprobante);

                        //Recuperando la addenda aplicable al comprobante
                        using (AddendaComprobante ac = AddendaComprobante.RecuperaAddendaComprobante(Convert.ToInt32(a["Id"]), this._id_comprobante))
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
                                        using (Addenda ad = new Addenda(Convert.ToInt32(a["IdAddenda"])))
                                        {
                                            //Validando carga correcta de registro
                                            if (ad.id_addenda > 0)
                                            {
                                                //Definiendo objeto para almacenamiento del contenido de addendas 
                                                //(se añade debido a que el contenido de este nodo no está regulado bajo ningún estándar 
                                                //y puede ser que un addenda no contenga un elemento raíz)
                                                object obj_elemento_ac = elemento_ac;

                                                //Creando addenda personalizada (en caso de estar definida para el receptor)
                                                r = AddendaEmisor.CreaAddendaReceptor(ref obj_elemento_ac, this);

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
        /// Añade las addendas definidas para el receptor y emisor del comprobante
        /// </summary>
        /// <param name="comprobante">Elemento xml a editar</param>
        /// <param name="ns_sat">Namespace del SAT para asociar elementos contenedores de addeenda</param>
        /// <param name="id_usuario">Usuario</param>
        /// <returns></returns>
        protected RetornoOperacion anadeElementoAddendaComprobante(ref XElement comprobante, XNamespace ns_sat, int id_usuario)
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante);


            //Cargando las addendas aplicables
            using (DataTable addendas = AddendaEmisor.CargaAddendasRequeridasEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor))
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
                        RetornoOperacion r = new RetornoOperacion(this._id_comprobante);

                        //Recuperando la addenda aplicable al comprobante
                        using (AddendaComprobante ac = AddendaComprobante.RecuperaAddendaComprobante(Convert.ToInt32(a["Id"]), this._id_comprobante))
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
                                        using (Addenda ad = new Addenda(Convert.ToInt32(a["IdAddenda"])))
                                        {
                                            //Validando carga correcta de registro
                                            if (ad.id_addenda > 0)
                                            {
                                                //Definiendo objeto para almacenamiento del contenido de addendas 
                                                //(se añade debido a que el contenido de este nodo no está regulado bajo ningún estándar 
                                                //y puede ser que un addenda no contenga un elemento raíz)
                                                object obj_elemento_ac = elemento_ac;

                                                //Creando addenda personalizada (en caso de estar definida para el receptor)
                                                r = AddendaEmisor.CreaAddendaReceptor(ref obj_elemento_ac, this);

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
                                                        resultado = ac.EditarAddendaComprobante( xmlDoc, id_usuario);
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
                        resultado = new RetornoOperacion("No se ha capturado addenda o bien no ha sido validada.");
                    }
                    else
                    {
                        //Si no existen errores y al menos una captura fue correcta
                        if (x.OperacionExitosa)
                        {
                            //Si addenda tiene contenido
                            if (elemento_addenda.HasElements)
                                comprobante.Add(elemento_addenda);
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
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        
        /// <summary>
        /// Verifica que el comprobante tenga al menos una addenda por añadir antes de realizar el timbrado, para evitar errores de guardado posteriores
        /// </summary>
        /// <returns></returns>
        protected RetornoOperacion validaAddendasCapturadas()
        {
            //Definiendo objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante);

             //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando las addendas aplicables
                using (DataTable addendas = AddendaEmisor.CargaAddendasRequeridasEmisorReceptor(this._id_compania_emisor, this._id_compania_receptor))
                {
                    //Si existen addendas configuradas
                    if (Validacion.ValidaOrigenDatos(addendas))
                    {
                        //Definiendo objeto de validación de captura de al menos un addenda
                        List<RetornoOperacion> resultado_addenda = new List<RetornoOperacion>();

                        //Para cada una de las addendas configuradas
                        foreach (DataRow a in addendas.Rows)
                        {
                            RetornoOperacion r = new RetornoOperacion(this._id_comprobante);

                            //Recuperando la addenda aplicable al comprobante
                            using (AddendaComprobante ac = AddendaComprobante.RecuperaAddendaComprobante(Convert.ToInt32(a["Id"]), this._id_comprobante))
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
                                resultado = new RetornoOperacion(this._id_comprobante);
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
                if(resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
            //Devolviendo resultado
            return resultado;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inserta un nuevo comprobante en BD
        /// </summary>
        /// <param name="tipo_comprobante"></param>
        /// <param name="id_origen_datos"></param>
        /// <param name="version"></param>
        /// <param name="id_forma_pago"></param>
        /// <param name="id_condiciones_pago"></param>
        /// <param name="id_metodo_pago"></param>
        /// <param name="no_parcialidad"></param>
        /// <param name="total_parcialidades"></param>
        /// <param name="id_moneda"></param>
        /// <param name="fecha_tipo_cambio"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_direccion_emisor"></param>
        /// <param name="id_sucursal"></param>
        /// <param name="id_direccion_sucursal"></param>
        /// <param name="id_compania_receptor"></param>
        /// <param name="id_direccion_receptor"></param>
        /// <param name="lugar_expedicion"></param>
        /// <param name="id_cuenta_pago"></param>
        /// <param name="serie_folio_original"></param>
        /// <param name="folio_original"></param>
        /// <param name="fecha_folio_original"></param>
        /// <param name="monto_folio_original"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaComprobante(TipoComprobante tipo_comprobante, byte id_origen_datos, string version, byte id_forma_pago, byte id_condiciones_pago, byte id_metodo_pago,
                                                int no_parcialidad, int total_parcialidades, byte id_moneda, DateTime fecha_tipo_cambio, int id_compania_emisor, int id_direccion_emisor, int id_sucursal, int id_direccion_sucursal,
                                                int id_compania_receptor, int id_direccion_receptor, string lugar_expedicion, string id_cuenta_pago, string serie_folio_original,
                                                int folio_original, DateTime fecha_folio_original, decimal monto_folio_original, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creando objeto con datos de registro por insertar
            object[] param = { 1, 0, "", 0, 0, (byte)tipo_comprobante, id_origen_datos, (byte)EstatusComprobante.Vigente, version, "", 
                                 id_forma_pago, id_condiciones_pago, id_metodo_pago, no_parcialidad, total_parcialidades, id_moneda, Fecha.ConvierteDateTimeObjeto(fecha_tipo_cambio),
                                 0, 0, 0, 0, 0, 0, 0, 0, id_compania_emisor, id_direccion_emisor, id_sucursal, id_direccion_sucursal, id_compania_receptor,
                                 id_direccion_receptor, Fecha.ObtieneFechaEstandarMexicoCentro(), null, null, false, false, 0, false, 0, lugar_expedicion, id_cuenta_pago, serie_folio_original,
                                 folio_original, Fecha.ConvierteDateTimeObjeto(fecha_folio_original), monto_folio_original, id_usuario, true, "", "" };
            

            //Realizando la inserción sobre la tabla
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de todos los datos de un comprobante
        /// </summary>
        /// <param name="tipo_comprobante"></param>
        /// <param name="id_forma_pago"></param>
        /// <param name="id_condiciones_pago"></param>
        /// <param name="id_metodo_pago"></param>
        /// <param name="no_parcialidad"></param>
        /// <param name="total_parcialidades"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_direccion_emisor"></param>
        /// <param name="id_sucursal"></param>
        /// <param name="id_direccion_sucursal"></param>
        /// <param name="id_compania_receptor"></param>
        /// <param name="id_direccion_receptor"></param>
        /// <param name="lugar_expedicion"></param>
        /// <param name="id_cuenta_pago"></param>
        /// <param name="serie_folio_original"></param>
        /// <param name="folio_original"></param>
        /// <param name="fecha_folio_original"></param>
        /// <param name="monto_folio_original"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaComprobante(TipoComprobante tipo_comprobante, byte id_forma_pago, byte id_condiciones_pago, byte id_metodo_pago, int no_parcialidad, int total_parcialidades, int id_compania_emisor, int id_direccion_emisor, int id_sucursal, int id_direccion_sucursal,
                                                        int id_compania_receptor, int id_direccion_receptor, string lugar_expedicion, int id_cuenta_pago, string serie_folio_original, int folio_original, DateTime fecha_folio_original, decimal monto_folio_original, int id_usuario)
        {
            return editaComprobante(this._serie, this._folio, this._id_certificado, tipo_comprobante, this._id_origen_datos, this.estatus_comprobante, this._version, this._sello, id_forma_pago, id_condiciones_pago, id_metodo_pago, no_parcialidad, total_parcialidades, this._id_moneda,this._fecha_tipo_cambio,
                this._subtotal_moneda_captura, this._subtotal_moneda_nacional, this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, id_compania_emisor, id_direccion_emisor,
                id_sucursal, id_direccion_sucursal, id_compania_receptor, id_direccion_receptor, this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, this._generado, this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_cancelado, this._id_transferido_cancelado,
                lugar_expedicion, id_cuenta_pago, serie_folio_original, folio_original, fecha_folio_original, monto_folio_original, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la cancelación del comprobante
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion CancelaComprobante(int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si el comprobante está generado y no ha sido cancelado aún
                if (this._generado && this.estatus_comprobante == EstatusComprobante.Vigente)
                {
                    //Inicializando parametros
                    object[] parametros = { 2, this._id_comprobante, this._serie, this._folio, this._id_certificado, this._id_tipo_comprobante, this._id_origen_datos, (byte)EstatusComprobante.Cancelado, this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, this._fecha_tipo_cambio, 
                                      this._subtotal_moneda_captura, this._subtotal_moneda_nacional, this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, 
                                      this._id_sucursal, this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura, Fecha.ConvierteDateTimeObjeto(this._fecha_expedicion), Fecha.ObtieneFechaEstandarMexicoCentro(), this._generado, 
                                      this._bit_transferido_nuevo, this._id_transferido_nuevo, true, this._id_transferido_cancelado, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original, Fecha.ConvierteDateTimeObjeto(this._fecha_folio_original), this._monto_folio_original, id_usuario, this._habilitar, "", "" };

                    //Ejecutando actualización
                    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, parametros);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Insertamos Bitácora de Cancelación de Comprobante
                        resultado = Bitacora.InsertaBitacora(119, this._id_comprobante, 8476, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos que no se au Recibo de Nómina
                            if ((OrigenDatos)this._id_origen_datos != OrigenDatos.ReciboNomina)
                            {
                                //Instanciamos Relacion Facturado facturacion
                                using (FacturadoFacturacion objFacturadoFacturacion = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFacturaElectronica(this._id_comprobante)))
                                {
                                    //Validamos que exista Relación
                                    if (objFacturadoFacturacion.id_facturado_facturacion > 0)
                                    {
                                        //Validamos el Origen de Datos
                                        if ((OrigenDatos)this._id_origen_datos == OrigenDatos.Facturado)
                                        {
                                            //Deshablitamos Relación
                                            resultado = objFacturadoFacturacion.ActualizaEstatusFacturadoFacturacion(FacturadoFacturacion.Estatus.Cancelada, id_usuario);
                                        }
                                        else if ((OrigenDatos)this._id_origen_datos == OrigenDatos.FacturaGlobal)
                                        {
                                            //Instanciamos Factura Global
                                            using (FacturaGlobal objFacturaGlobal = new FacturaGlobal(objFacturadoFacturacion.id_factura_global))
                                            {
                                                //Deshabilitamos Factura Global
                                                resultado = objFacturaGlobal.ActualizaACanceladoEstatusFacturaGlobal(id_usuario);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        resultado = new RetornoOperacion("Imposible recuperar datos complementarios Facturado Facturación.");
                                    }
                                }

                            }
                        }
                    }
                }
                else
                    resultado = new RetornoOperacion("El comprobante no se ha timbrado aún o ya se ha cancelado con aterioridad.");

                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }



        /// <summary>
        /// Actualiza Subtotal de Comprobante
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaSubtotalComprobante(int id_usuario)
        {
            //Inicializamos Vaariables
            decimal SubtotalCaptura = 0;
            decimal SubtotalNacional = 0;

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
              //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obtiene Total Conceptos
                using (DataTable mit = Concepto.RecuperaConceptosComprobantes(this._id_comprobante))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {

                        //Obtenemos Total  Conceptos
                        SubtotalCaptura = (from DataRow r in mit.Rows
                                           select Convert.ToDecimal(r["MonedaCaptura"])).Sum();


                        SubtotalNacional = (from DataRow r in mit.Rows
                                            select Convert.ToDecimal(r["MonedaNacional"])).Sum();
                    }
                }
                //Editamos Combrobante
                resultado = editaComprobante(this._serie, this._folio, this._id_certificado, this.tipo_comprobante, this._id_origen_datos, this.estatus_comprobante, this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, this._fecha_tipo_cambio, SubtotalCaptura, SubtotalNacional,
                        this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, this._id_sucursal, this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura,
                        this._fecha_expedicion, this._fecha_cancelacion, this._generado, this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_cancelado, this._id_transferido_cancelado, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original, this._fecha_folio_original, this._monto_folio_original, id_usuario, this._habilitar);

                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }
            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Devuelve los comprobantes sellados por un CSD
        /// </summary>
        /// <param name="id_certificado">Id de Certificado</param>
        /// <returns></returns>
        public static DataTable RecuperaComprobantesSellados(int id_certificado)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo arreglo de parámetros
            object[] param = { 5, 0, "", 0, id_certificado, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, null, false, false,0, false, 0,  "", 0, "", 0, null, 0, 0, false, "", "" };

    
    
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }

        /// <summary>
        /// Obtiene el Comprobante Vigente de un Recibo de Nómina
        /// </summary>
        /// <param name="id_liquidacion">Id Liquidación por Timbrar</param>
        /// <returns></returns>
        public static int ObtieneReciboNominaVigente(int id_liquidacion)
        {
            //Definiendo objeto de retorno
            int id_comprobante = 0;

            //Inicializando parametros
            object[] parametros = { 12, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, null, false, false, 0, false, 0, "", 0, "", 0, null, 0, 0, false, id_liquidacion, "" };
            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS))
                    id_comprobante = (from DataRow r in DS.Tables["Table"].Rows
                                    select r.Field<int>("Id")).FirstOrDefault();

                //Devolviendo resultado
                return id_comprobante;
            }
        }

        /// <summary>
        /// Devuelve los comprobantes vinculados a un id de ubicación
        /// </summary>
        /// <param name="id_ubicacion">Id de Ubicación</param>
        /// <returns></returns>
        public static DataTable RecuperaComprobantesPorUbicacion(int id_ubicacion)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo arreglo de parámetros
            object[] param = { 6, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_ubicacion, 0, id_ubicacion, 0, id_ubicacion, null, null, null, false, false, 0, false, 0, "", 0, "", 0, null, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }

        /// <summary>
        /// Devuelve los comprobantes ligado a una Sucursal
        /// </summary>
        /// <param name="id_sucursal">Id Sucursal </param>
        /// <returns></returns>
        public static DataTable RecuperaComprobantesPorSucursal(int id_sucursal)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo arreglo de parámetros
            object[] param = { 7, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_sucursal, 0, 0, 0, null, null, null, false, false, 0, false, 0, "", "", "", 0, null, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }

        /// <summary>
        /// Devuelve los comprobantes ligado a un Receptor
        /// </summary>
        /// <param name="id_compania_receptor">Id Receptor</param>
        /// <returns></returns>
        public static DataTable RecuperaComprobantesPorReceptor(int id_compania_receptor)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo arreglo de parámetros
            object[] param = { 8, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_compania_receptor, 0, null, null, null, false, false, 0, false, 0, "", "", "", 0, null, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }

        /// <summary>
        /// Devuelve los comprobantes ligado a un Emisor
        /// </summary>
        /// <param name="id_compania_emisor">Id Emisor</param>
        /// <returns></returns>
        public static DataTable RecuperaComprobantesPorEmisor(int id_compania_emisor)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo arreglo de parámetros
            object[] param = { 9, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_compania_emisor, 0, 0, 0, 0, 0, null, null, null, false, false, 0, false, 0, "", "", "", 0, null, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }


        /// <summary>
        /// Obtiene Decuento  a aplicar por cada uno de los Conceptos 
        /// </summary>
        /// <param name="concepto_importe_moneda_captura"></param>
        /// <param name="concepto_importe_moneda_nacional"></param>
        /// <param name="descuento_importe_moneda_captura"></param>
        /// <param name="descuento_importe_moneda_nacional"></param>
        public void ObtieneDescuentoPorConcepto(decimal concepto_importe_moneda_captura, decimal concepto_importe_moneda_nacional, out decimal descuento_importe_moneda_captura, out decimal descuento_importe_moneda_nacional)
        {
            //Inicializando variables de retorno
            descuento_importe_moneda_captura = 0; descuento_importe_moneda_nacional = 0;

            //Validamos Subtotal del Comprobante
            if (this.subtotal_moneda_captura != 0)
            {

                //Obtenemos el Total de cada uno de los Conceptos
                descuento_importe_moneda_captura = (concepto_importe_moneda_captura * this._descuento_moneda_captura) / this._subtotal_moneda_captura;
                descuento_importe_moneda_nacional = (concepto_importe_moneda_nacional * this._descuento_moneda_nacional) / this._subtotal_moneda_nacional;

            }

        }
        /// <summary>
        ///  Actualiza Impuesto del Comprobante  ligado a una transaccion
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaImpuestosComprobante(int id_impuesto, int id_usuario)
        {
            decimal impuesto_total_captura = 0;
            decimal impuesto_total_nacional = 0;

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
              //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
            //Obtenemos Impuesto ligado al comprobante
                using (Impuesto objImpuesto = new Impuesto(id_impuesto))
                {
                    //Validamos Existencia de Impuesto
                    if (objImpuesto.id_impuesto > 0)
                    {
                        //Calculamos Impuestos del comprobante
                        impuesto_total_captura = objImpuesto.total_trasladado_moneda_captura - objImpuesto.total_retenido_moneda_captura;
                        impuesto_total_nacional = objImpuesto.total_trasladado_moneda_nacional - objImpuesto.total_retenido_moneda_nacional;

                        resultado = editaComprobante(this._serie, this._folio, this._id_certificado, this.tipo_comprobante, this._id_origen_datos, this.estatus_comprobante, this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, this._fecha_tipo_cambio, this._subtotal_moneda_captura,
                                                this._subtotal_moneda_nacional, this._descuento_moneda_captura, this._descuento_moneda_nacional, impuesto_total_captura, impuesto_total_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, this._id_sucursal, this._id_direccion_sucursal,
                                                this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, this._generado, this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_cancelado, this._id_transferido_cancelado, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original,
                                                this._fecha_folio_original, this._monto_folio_original, id_usuario, this._habilitar);
                        //Validamos Resultado
                        if(resultado.OperacionExitosa)
                        {
                            //Finalizamos Transacción
                            scope.Complete();
                        }
                    }
                    else
                    {
                        resultado = new RetornoOperacion("No se encontro datos complementarios del Impuesto");
                    }
                }
            }

            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Carga los detalles del comprobante (conceptos, descuentos e impuestos)
        /// </summary>
        /// <param name="id_comprobante">id comprobante</param>
        public static DataTable CargaDetallesComprobante(int id_comprobante)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parametros
            object[] parametros = { 11, id_comprobante, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, null, false, false, 0, false, 0, "", 0, "", 0, null, 0, 0, false, "", "" };
            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    mit = DS.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
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


            //Editamos Descuentos Combrobante
            return editaComprobante(this._serie, this._folio, this._id_certificado, this.tipo_comprobante, this._id_origen_datos, this.estatus_comprobante,
                                                 this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad,
                                                 this._total_parcialidades, this._id_moneda, this._fecha_tipo_cambio, this._subtotal_moneda_captura, this._subtotal_moneda_nacional,
                                                 descuento_moneda_captura, descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional,
                                                 this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, this._id_sucursal,
                                                 this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura, this._fecha_expedicion,
                                                 this._fecha_cancelacion, this._generado, this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_cancelado, this._id_transferido_cancelado,
                                                 this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original,
                                                 this._fecha_folio_original, this._monto_folio_original, id_usuario, this._habilitar);


        }

        
        /* vane
        /// <summary>
        /// Método que exporta el archivo XML de un CFD a formato PDF
        /// </summary>
        /// <param name="bytesArchivoPDF">Retorna los Bytes del XML ya transformado</param>
        /// <param name="ruta_xslt_cfdi">Ruta física del archivo de transformación que se aplicará al CFDI (.xslt)</param>
        /// <param name="ruta_xslt_co">Ruta física del archivo de transformación que se aplicará a la Cadena Original (.xslt)</param>
        /// <param name="ruta_xslt_co_alterna">Ruta física del archivo de transformación que se aplicará a la Candena Original Alterna (.xslt)</param>
        /// <param name="ruta_css_cfdi">Ruta física del archivo de Estilos en Cascada (CSS) que se aplicará a la transformación del CFDI</param>
        /// <returns></returns>
        public RetornoOperacion GeneraComprobanteFiscalDigitalPDF(out byte[] bytesArchivoPDF, string ruta_xslt_cfdi, string ruta_xslt_co, string ruta_xslt_co_alterna, string ruta_css_cfdi)
        {
            //Declarando variable de resultado del proceso
            RetornoOperacion resultado = new RetornoOperacion();

            bytesArchivoPDF = null;

            //Declaramos Memory Stream
            MemoryStream flujoHTML = null;

            //Obteneiendo el flujo del archivo HTML creado a partir del CFD 
            resultado = transformaCFDXML(out flujoHTML, ruta_xslt_cfdi, ruta_xslt_co, ruta_xslt_co_alterna, ruta_css_cfdi);

            //Validamos Transformacion
            if (resultado.OperacionExitosa)
            {
                //Si el flujo es diferencte de vacío
                if (flujoHTML != null)
                {
                    //Instanciando un nuevo lector de flujos a partir del flujo del archivo HTML generado
                    StreamReader lector = new StreamReader(flujoHTML);

                    //Estableciendo la posición inicial de lectura
                    lector.BaseStream.Position = 0;

                    //Leyendo y escribiendo en una cadena el contenido del lector de flujo
                    string cadenaHTML = lector.ReadToEnd();

                    //Cerrando el lector
                    lector.Close();

                    //Cerrando el flujo HTML
                    flujoHTML.Close();

                    //Si se obtuvo una cadena a partir del lector
                    if (cadenaHTML != "")
                    { 
                        //Inicialziando conversor de documento
                        PdfConverter convertidor = PDF.CreaConversorHtmlAPdf(false, PdfPageSize.Letter, PDFPageOrientation.Portrait, false, false, 0, 0, 0, 0, PdfCompressionLevel.Normal);

                        //Obteniendo el arreglo de bytes de la cadena HTML, ligandola a la URL de la aplicación para el correcto enlace de complementos (CSS e imagenes)
                        bytesArchivoPDF = convertidor.GetPdfBytesFromHtmlString(cadenaHTML, AppDomain.CurrentDomain.BaseDirectory);

                        //Mostrando descarga de archivo PDF resultante
                        //clArchivos.DescargaArchivo(bytesArchivoPDF, this._numeroFolio + ".pdf", clArchivos.ContentType.binary_octetStream);

                        //Realizando la exportación a PDF
                        resultado = new RetornoOperacion("Se generó correctamente");
                    }
                }
            }

            //Devolvinado valor de error
            return resultado;
        }
       */
        
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
        public RetornoOperacion TimbraReciboNomina_V3_2(string serie, int id_usuario, string ruta_xslt_co, string ruta_xslt_co_local, bool omitir_addenda, DataTable nomina, DataTable percepion,
                                                                               DataTable percepciones, DataTable deduccion, DataTable deducciones, DataTable incapacidad, DataTable horasextra)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

             //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando variables auxiliares
                int id_certificado_activo = 0, folio = 0;
                string numero_certificado = "", certificado_base_64 = "", contrasena_certificado = "";
                byte[] bytes_certificado = null;
                serie = serie.ToUpper();

                //Validando requisitos previos a la genracióndel CFD (Serie solicitada existente y folio disponible, Timbrado previo, Emisor activo con certificado vigente y Addendas)
                resultado = validaRequisitosPreviosTimbrado(serie, out folio, out id_certificado_activo, out numero_certificado, out certificado_base_64, out contrasena_certificado, out bytes_certificado, SerieFolio.tipo.ReciboNomina);

                //Si cuenta con lo requerido para ser timbrado
                if (resultado.OperacionExitosa)
                {
                    //Actualizando Serie, Folio y Certificado utilizadso para sellado del comprobante
                    resultado = asignaFolioComprobante(serie, folio, id_certificado_activo, id_usuario);

                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recargando contenido de instancia
                        if (cargaAtributosInstancia(this._id_comprobante))
                        {
                            //Instanciando al emisor
                            using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                            {/*
                                //Creando ruta de guardado del comprobante
                                string ruta_xml = string.Format(@"{0}{1}CFDI_3_2\{2}\{3}\{4}\{5}{6}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Unidad Almacenamiento CFD", 0),
                                                                          em.DirectorioAlmacenamiento, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year.ToString("0000"), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month.ToString("00"),
                                                                          TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Day.ToString("00"), serie, folio);*/
                                //Creando ruta de guardado del comprobante
                                string ruta_xml = string.Format(@"{0}{1}\{2}\CFDI_3_2\{3}{4}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 119.ToString("0000"),
                                                                          em.DirectorioAlmacenamiento, serie, folio);

                                //Eliminando archivo si es que ya existe
                                Archivo.EliminaArchivo(ruta_xml);

                                //Declaración de namespaces a utilizar en el Comprobante
                                //SAT
                                XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT", 0);

                                //Declaración de namespaces a utilizar en el Comprobante
                                //SAT Nomina
                                XNamespace nsn = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT Nomina", 0);
                                //W3C
                                XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C", 0);
                                //Inicialziando el valor de schemaLocation del cfd
                                string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFD", 0);

                                //Creando Documento xml inicial y configurando la declaración de XML
                                XDocument documento = new XDocument();
                                documento.Declaration = new XDeclaration("1.0", "utf-8", "");

                                //Declaramos Objeto Resultato para validar Complemento de Nomina
                                RetornoOperacion resultadocomplementonomina = new RetornoOperacion();

                               
                                XElement comprobante = ComprobanteXML.CargaElementosArmadoReciboNomina_3_2(this._id_comprobante, ns, nsn, nsW3C, schemaLocation, this._id_compania_emisor, this._id_compania_receptor, nomina, percepion, percepciones,
                                                                                                                        deduccion, deducciones, incapacidad, horasextra, id_usuario, out resultadocomplementonomina);


                                //Validamos  que no exista errores en la creación del Complemento de Nómina
                                if (resultadocomplementonomina.OperacionExitosa)
                                {
                                    //Si no se ha requerido addenda/complementos
                                    if (!omitir_addenda)
                                        //Añadir addenda en caso necesario
                                        resultado = anadeElementoComplementoReciboNomina("Nómina",ref comprobante, ns);

                                    //Si no hubo errores
                                    if (resultado.OperacionExitosa)
                                        //Validando sólo para fines internos, que existan addendas
                                        resultado = validaAddendasCapturadas();

                                    //Si no hubo errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Añadiendo contenido del comprobante
                                        documento.Add(comprobante);

                                        //Definiendo bytes de XML
                                        byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                        //Verificando que se haya devuelto los bytes del XDocument
                                        if (!Conversion.EsArrayVacio(bytes_comprobante))
                                        {
                                            //Definiendo variable para almacenar cadena orignal
                                            string cadena_original = "";

                                            //Realizando transformación de cadena original
                                            resultado = SelloDigitalSAT.CadenaCFD(new System.IO.MemoryStream(bytes_comprobante), ruta_xslt_co, ruta_xslt_co_local, out cadena_original);

                                            //Si no hay errores
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Verificando Contenido de Cadena Original
                                                if (cadena_original != "")
                                                {
                                                    //Codificando Cadena Original a UTF-8
                                                    byte[] co_utf_8 = SelloDigitalSAT.CodificacionUTF8(cadena_original);
                                                    //Realizando sellado del Comprobante
                                                    string sello_digital = SelloDigitalSAT.FirmaCadenaSHA1(co_utf_8, bytes_certificado, contrasena_certificado);

                                                    //Si el sello digital fue generado correctamente
                                                    if (sello_digital != "")
                                                    {
                                                        //Actualizando el sello digital en BD y Marcando como 'Generado'
                                                        resultado = actualizaSelloDigital(sello_digital, id_usuario);

                                                        //Si se actualiza correctamente
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizando datos de sello digital
                                                            if (cargaAtributosInstancia(this._id_comprobante))
                                                            {
                                                                //Actualizando Sello y número de certificado en XML
                                                                documento.Root.SetAttributeValue("noCertificado", numero_certificado);
                                                                documento.Root.SetAttributeValue("certificado", certificado_base_64);
                                                                documento.Root.SetAttributeValue("sello", sello_digital);

                                                                //Conectando a Web Service del Proveedor de Timbre Fiscal y generando dicho registro
                                                                resultado = generaTimbreFiscalDigital(ref documento, ns, id_usuario);

                                                                //Si no existen errores en timbrado
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    if (!omitir_addenda)
                                                                    {
                                                                        //Actualizando contenido de comprobante
                                                                        comprobante = documento.Root;
                                                                        //Si se ha solicitado añadir elemento complemento
                                                                        resultado = anadeElementoAddendaComprobante(ref comprobante, ns, id_usuario);
                                                                    }
                                                                    //De lo contrario
                                                                    else
                                                                        //Registrando petición de timbrado sin addenda
                                                                        resultado = Bitacora.InsertaBitacora(119, this._id_comprobante, 6086, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                    //SI no hay errores
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Actualizando contenido de documento a arreglo de bytes
                                                                        bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                                        //Guardando archivo en unidad de almacenamiento
                                                                        resultado = ArchivoRegistro.InsertaArchivoRegistro(119, this._id_comprobante, 16, "", id_usuario, bytes_comprobante, ruta_xml);

                                                                        //Validmos Resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Generamos Barra Bidimensional
                                                                            resultado = this.generaCodigoBidimensionalComprobante(id_usuario);
                                                                        }
                                                                        //Si no existe error
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Asignando Id de Comprobante como Resultado general
                                                                            resultado = new RetornoOperacion(this._id_comprobante);
                                                                            //Finalizamos transacción
                                                                            scope.Complete();
                                                                        }
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

            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realzia el timbrado del comprobante, creando XML del mismo, Sellandolo y Concetandose con el proveedor de Timbrado
        /// </summary>
        /// <param name="version_nomina">Versión de la Nómina</param>
        /// <param name="serie"></param>
        /// <param name="id_usuario"></param>
        /// <param name="ruta_xslt_co"></param>
        /// <param name="ruta_xslt_co_local"></param>
        /// <param name="omitir_addenda"></param>
        /// <returns></returns>
        public RetornoOperacion TimbraReciboNomina_V3_2(string version_nomina, string serie, int id_usuario, string ruta_xslt_co, string ruta_xslt_co_local, int id_nomina_empleado, bool omitir_addenda)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando variables auxiliares
                int id_certificado_activo = 0, folio = 0;
                string numero_certificado = "", certificado_base_64 = "", contrasena_certificado = "";
                byte[] bytes_certificado = null;
                serie = serie.ToUpper();

                //Validando requisitos previos a la genracióndel CFD (Serie solicitada existente y folio disponible, Timbrado previo, Emisor activo con certificado vigente y Addendas)
                resultado = validaRequisitosPreviosTimbradoNomina(serie, out folio, out id_certificado_activo, out numero_certificado, out certificado_base_64, out contrasena_certificado, out bytes_certificado, SerieFolio.tipo.ReciboNomina);

                //Si cuenta con lo requerido para ser timbrado
                if (resultado.OperacionExitosa)
                {
                    //Actualizando Serie, Folio y Certificado utilizadso para sellado del comprobante
                    resultado = asignaFolioComprobante(serie, folio, id_certificado_activo, id_usuario);

                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recargando contenido de instancia
                        if (cargaAtributosInstancia(this._id_comprobante))
                        {
                            //Instanciando al emisor
                            using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                            {
                                //Creando ruta de guardado del comprobante
                                string ruta_xml = string.Format(@"{0}{1}\{2}\CFDI_3_2\{3}{4}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 119.ToString("0000"),
                                                                          em.DirectorioAlmacenamiento, serie, folio);

                                //Eliminando archivo si es que ya existe
                                Archivo.EliminaArchivo(ruta_xml);

                                //Declaración de namespaces a utilizar en el Comprobante
                                //SAT Nomina
                                XNamespace nsn = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT Nomina" + version_nomina.ToString(), 0);
                                //SAT
                                XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT ", 0);

                                //Esquema NOMIANA
                                XNamespace wn12 = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace WN12 ", 0);

                                //Declaración de namespaces a utilizar en el Comprobante
                               
                                //W3C
                                XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C", 0);
                                //Inicialziando el valor de schemaLocation del cfd
                                string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFD", 0);

                                //Creando Documento xml inicial y configurando la declaración de XML
                                XDocument documento = new XDocument();
                                documento.Declaration = new XDeclaration("1.0", "utf-8", "");

                                //Declaramos Objeto Resultato para validar Complemento de Nomina
                                RetornoOperacion resultadocomplementonomina = new RetornoOperacion();


                                //XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobante_3_2(this._id_comprobante, ns, nsW3C, schemaLocation, transaccion);
                                XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobanteReciboNominaActualizacion1_V_3_2(version_nomina,id_nomina_empleado, this._id_comprobante, ns,nsn, nsW3C,wn12, schemaLocation, this._id_compania_emisor, this._id_compania_receptor, id_usuario, out resultadocomplementonomina);

                                //Validamos  que no exista errores en la creación del Complemento de Nómina
                                if (resultadocomplementonomina.OperacionExitosa)
                                {
                                    //Si no se ha requerido addenda/complementos
                                    if (!omitir_addenda)
                                        //Añadir addenda en caso necesario
                                        resultado = anadeElementoComplementoReciboNomina(version_nomina,ref comprobante, ns);

                                    //Si no hubo errores
                                    if (resultado.OperacionExitosa)
                                        //Validando sólo para fines internos, que existan addendas
                                        resultado = validaAddendasCapturadas();

                                    //Si no hubo errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Añadiendo contenido del comprobante
                                        documento.Add(comprobante);

                                        //Definiendo bytes de XML
                                        byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                        //Verificando que se haya devuelto los bytes del XDocument
                                        if (!Conversion.EsArrayVacio(bytes_comprobante))
                                        {
                                            //Definiendo variable para almacenar cadena orignal
                                            string cadena_original = "";

                                            //Realizando transformación de cadena original
                                            resultado = SelloDigitalSAT.CadenaCFD(new System.IO.MemoryStream(bytes_comprobante), ruta_xslt_co, ruta_xslt_co_local, out cadena_original);

                                            //Si no hay errores
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Verificando Contenido de Cadena Original
                                                if (cadena_original != "")
                                                {
                                                    //Codificando Cadena Original a UTF-8
                                                    byte[] co_utf_8 = SelloDigitalSAT.CodificacionUTF8(cadena_original);
                                                    //Realizando sellado del Comprobante
                                                    string sello_digital = SelloDigitalSAT.FirmaCadenaSHA1(co_utf_8, bytes_certificado, contrasena_certificado);

                                                    //Si el sello digital fue generado correctamente
                                                    if (sello_digital != "")
                                                    {
                                                        //Actualizando el sello digital en BD y Marcando como 'Generado'
                                                        resultado = actualizaSelloDigital(sello_digital, id_usuario);

                                                        //Si se actualiza correctamente
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizando datos de sello digital
                                                            if (cargaAtributosInstancia(this._id_comprobante))
                                                            {
                                                                //Actualizando Sello y número de certificado en XML
                                                                documento.Root.SetAttributeValue("noCertificado", numero_certificado);
                                                                documento.Root.SetAttributeValue("certificado", certificado_base_64);
                                                                documento.Root.SetAttributeValue("sello", sello_digital);

                                                                //Conectando a Web Service del Proveedor de Timbre Fiscal y generando dicho registro
                                                                resultado = generaTimbreFiscalDigital(ref documento, ns, id_usuario);

                                                                //Si no existen errores en timbrado
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    if (!omitir_addenda)
                                                                    {
                                                                        //Actualizando contenido de comprobante
                                                                        comprobante = documento.Root;
                                                                        //Si se ha solicitado añadir elemento complemento
                                                                        resultado = anadeElementoAddendaComprobante(ref comprobante, ns, id_usuario);
                                                                    }
                                                                    //De lo contrario
                                                                    else
                                                                        //Registrando petición de timbrado sin addenda
                                                                        resultado = Bitacora.InsertaBitacora(119, this._id_comprobante, 6086, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                    //SI no hay errores
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Actualizando contenido de documento a arreglo de bytes
                                                                        bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                                        //Guardando archivo en unidad de almacenamiento
                                                                        resultado = ArchivoRegistro.InsertaArchivoRegistro(119, this._id_comprobante, 16, "", id_usuario, bytes_comprobante, ruta_xml);

                                                                        //Validmos Resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Generamos Barra Bidimensional
                                                                            resultado = this.generaCodigoBidimensionalComprobante(id_usuario);
                                                                        }
                                                                        //Si no existe error
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Asignando Id de Comprobante como Resultado general
                                                                            resultado = new RetornoOperacion(this._id_comprobante);
                                                                            //Finalizamos transacción
                                                                            scope.Complete();
                                                                        }
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

            }

            //Devolviendo resultado
            return resultado;
        }
        
        /// <summary>
        /// Realiza la recarga de los atributos de la instancia
        /// </summary>
        /// <returns></returns>
        public bool RefrescaAtributosInstancia()
        {
            return cargaAtributosInstancia(this._id_comprobante);
        }


        /// <summary>
        /// Metodo encargado de Timbrar un Comprobante de Acuerdo al Tipo de Comprobante
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="omitir_addenda"></param>
        /// <param name="ruta_xslt_co"></param>
        /// <param name="ruta_xslt_co_local"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion TimbraComprobanteFacturadoFacturaGlobal(string serie, bool omitir_addenda, string ruta_xslt_co,
                                                                 string ruta_xslt_co_local, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si el comprobante no está generado y no ha sido cancelado aún
                if (!this._generado && this.estatus_comprobante == EstatusComprobante.Vigente)
                {
                    //Validamos que no se au Recibo de Nómina
                    if ((OrigenDatos)this._id_origen_datos != OrigenDatos.ReciboNomina)
                    {
                        //Instanciamos Relacion Facturado facturacion
                        using (FacturadoFacturacion objFacturadoFacturacion = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFacturaElectronica(this._id_comprobante)))
                        {
                            //Validamos que exista Relación
                            if (objFacturadoFacturacion.id_facturado_facturacion > 0)
                            {
                                //Validamos el Origen de Datos
                                if ((OrigenDatos)this._id_origen_datos == OrigenDatos.Facturado)
                                {
                                    //Instanciamos Facturado
                                    using(Facturado objFacturado = new Facturado(objFacturadoFacturacion.id_factura))
                                    {
                                    //Timbra Facturación Otros
                                        resultado = objFacturado.TimbraFacturadoComprobante_V3_2(serie, omitir_addenda, ruta_xslt_co, ruta_xslt_co_local, id_usuario);
                                    }
                                }
                                else if ((OrigenDatos)this._id_origen_datos == OrigenDatos.FacturaGlobal)
                                {
                                    //Instanciamos Factura Global
                                    using (FacturaGlobal objFacturaGlobal = new FacturaGlobal(objFacturadoFacturacion.id_factura_global))
                                    {
                                        //Timbra Factura Global
                                        resultado = objFacturaGlobal.TimbraFacturaGlobal_V3_2(serie, omitir_addenda, ruta_xslt_co, ruta_xslt_co_local, id_usuario);
                                    }
                                }
                            }
                            else
                            {
                                resultado = new RetornoOperacion("Imposible recuperar datos complementarios Facturado Facturación.");
                            }
                        }
                    }
                    else
                        resultado = new RetornoOperacion("El comprobante es un Recibo de Nómina.");
                }
                    else
                        resultado = new RetornoOperacion("El comprobante  se ha timbrado o ya se ha cancelado con aterioridad.");

                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
            //Devolvemos Valor
            return resultado;
        }


        /// <summary>
        /// Realzia el timbrado del comprobante, creando XML del mismo, Sellandolo y Concetandose con el proveedor de Timbrado
        /// </summary>
        /// <param name="serie">Serie del folio solicitado(para control interno)</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="ruta_xslt_co">Ruta del Archivo de Transformación para cadena Original</param>
        /// <param name="ruta_xslt_co_local">Ruta del Archivo de Transformación para cadena Original (local, en caso de que la versíón en linea falle)</param>
        /// <param name="omitir_addenda">True para omitir addendas, de lo contrario false</param>
        /// <returns></returns>
        public RetornoOperacion TimbraComprobante(string serie, int id_usuario, string ruta_xslt_co, string ruta_xslt_co_local, bool omitir_addenda)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

             //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Declarando variables auxiliares
                int id_certificado_activo = 0, folio = 0;
                string numero_certificado = "", certificado_base_64 = "", contrasena_certificado = "";
                byte[] bytes_certificado = null;
                serie = serie.ToUpper();

                //Validando requisitos previos a la genracióndel CFD (Serie solicitada existente y folio disponible, Timbrado previo, Emisor activo con certificado vigente y Addendas)
                resultado = validaRequisitosPreviosTimbrado(serie, out folio, out id_certificado_activo, out numero_certificado, out certificado_base_64, out contrasena_certificado, out bytes_certificado, SerieFolio.tipo.FacturacionElectronica);

                //Si cuenta con lo requerido para ser timbrado
                if (resultado.OperacionExitosa)
                {
                    //Actualizando Serie, Folio y Certificado utilizadso para sellado del comprobante
                    resultado = asignaFolioComprobante(serie, folio, id_certificado_activo, id_usuario);

                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recargando contenido de instancia
                        if (cargaAtributosInstancia(this._id_comprobante))
                        {
                            //Instanciando al emisor
                            using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                            {
                                /*//Creando ruta de guardado del comprobante
                                string ruta_xml = string.Format(@"{0}{1}CFDI_3_2\{2}\{3}\{4}\{5}{6}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Unidad Almacenamiento CFD", 0),
                                                                          em.DirectorioAlmacenamiento, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year.ToString("0000"), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month.ToString("00"),
                                                                          TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Day.ToString("00"), serie, folio);*/
                                //Creando ruta de guardado del comprobante
                                string ruta_xml = string.Format(@"{0}{1}\{2}\CFDI_3_2\{3}{4}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 119.ToString("0000"),
                                                                          em.DirectorioAlmacenamiento, serie, folio);

                                //Eliminando archivo si es que ya existe
                                Archivo.EliminaArchivo(ruta_xml);

                                //Declaración de namespaces a utilizar en el Comprobante
                                //SAT
                                XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT");
                                //W3C
                                XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C");
                                //Inicialziando el valor de schemaLocation del cfd
                                string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFD");

                                //Creando Documento xml inicial y configurando la declaración de XML
                                XDocument documento = new XDocument();
                                documento.Document.Declaration = new XDeclaration("1.0", "utf-8", "");

                                //Generando xml base del comprobante
                                //XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobante_3_2(this._id_comprobante, ns, nsW3C, schemaLocation, transaccion);
                                XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobante_3_2(this._id_comprobante, ns, nsW3C, schemaLocation, this._id_compania_emisor, this._id_compania_receptor);

                                //Si no se ha omitido addenda/complementos
                                if (!omitir_addenda)
                                    //Añadir complemento en caso necesario
                                    resultado = anadeElementoComplementoComprobante(ref comprobante, ns, id_usuario);

                                //Si no hubo errores y no se pidio omitir addenda
                                if (resultado.OperacionExitosa && !omitir_addenda)
                                    //Validando sólo para fines internos, que existan addendas
                                    resultado = validaAddendasCapturadas();

                                //Si no hubo errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Añadiendo contenido del comprobante
                                    documento.Add(comprobante);

                                    //Instanciando al receptor
                                    using (CompaniaEmisorReceptor rec = new CompaniaEmisorReceptor(this._id_compania_receptor))
                                    {
                                        //Validamos que exista referencia
                                        if (rec.FacturacionElectronica != null)
                                        {
                                            //Validamos que exista la Clave
                                            if (rec.FacturacionElectronica.ContainsKey("Acepta Acentos Comptrobante"))
                                            {
                                                //Si es necesario realizar supresión de acentos en comprobante
                                                if (!Convert.ToBoolean(rec.FacturacionElectronica["Acepta Acentos Comptrobante"]))
                                                    documento = XDocument.Parse(suprimeCaracteresAcentuadosCFD(documento.ToString()));
                                            }
                                        }
                                    }

                                    //Definiendo bytes de XML
                                    byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                    //Verificando que se haya devuelto los bytes del XDocument
                                    if (!Conversion.EsArrayVacio(bytes_comprobante))
                                    {
                                        //Definiendo variable para almacenar cadena orignal
                                        string cadena_original = "";

                                        //Realizando transformación de cadena original
                                        resultado = SelloDigitalSAT.CadenaCFD(new System.IO.MemoryStream(bytes_comprobante), ruta_xslt_co, ruta_xslt_co_local, out cadena_original);

                                        //Si no hay errores
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Verificando Contenido de Cadena Original
                                            if (cadena_original != "")
                                            {
                                                //Codificando Cadena Original a UTF-8
                                                byte[] co_utf_8 = SelloDigitalSAT.CodificacionUTF8(cadena_original);
                                                //Realizando sellado del Comprobante
                                                string sello_digital = SelloDigitalSAT.FirmaCadenaSHA1(co_utf_8, bytes_certificado, contrasena_certificado);

                                                //Si el sello digital fue generado correctamente
                                                if (sello_digital != "")
                                                {
                                                    //Actualizando el sello digital en BD y Marcando como 'Generado'
                                                    resultado = actualizaSelloDigital(sello_digital, id_usuario);

                                                    //Si se actualiza correctamente
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Actualizando datos de sello digital
                                                        if (cargaAtributosInstancia(this._id_comprobante))
                                                        {
                                                            //Actualizando Sello y número de certificado en XML
                                                            documento.Root.SetAttributeValue("noCertificado", numero_certificado);
                                                            documento.Root.SetAttributeValue("certificado", certificado_base_64);
                                                            documento.Root.SetAttributeValue("sello", sello_digital);

                                                            //Conectando a Web Service del Proveedor de Timbre Fiscal y generando dicho registro
                                                            resultado = generaTimbreFiscalDigital(ref documento, ns, id_usuario);

                                                            //Si no existen errores en timbrado
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                if (!omitir_addenda)
                                                                {
                                                                    //Actualizando contenido de comprobante
                                                                    comprobante = documento.Root;
                                                                    //Si se ha solicitado añadir elemento complemento
                                                                    resultado = anadeElementoAddendaComprobante(ref comprobante, ns, id_usuario);
                                                                }
                                                                //De lo contrario
                                                                else
                                                                    //Registrando petición de timbrado sin addenda
                                                                    resultado = Bitacora.InsertaBitacora(119, this._id_comprobante, 6086, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                //SI no hay errores
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Actualizando contenido de documento a arreglo de bytes
                                                                    bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                                    //Guardando archivo en unidad de almacenamiento
                                                                    resultado = ArchivoRegistro.InsertaArchivoRegistro(119, this._id_comprobante, 16, "", id_usuario, bytes_comprobante, ruta_xml);

                                                                    //Validmos Resultado
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Generamos Barra Bidimensional
                                                                        resultado = this.generaCodigoBidimensionalComprobante(id_usuario);
                                                                    }
                                                                    //Si no existe error
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Asignando Id de Comprobante como Resultado general
                                                                        resultado = new RetornoOperacion(this._id_comprobante);

                                                                        //Finalizamos Transacción
                                                                        scope.Complete();
                                                                    }
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
                        }
                        else
                            resultado = new RetornoOperacion("Error al recuperar datos actualizados después de asignación de folio.");
                    }
                    else
                        resultado = new RetornoOperacion("Error al actualizar folio del comprobante.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }
    
        
        /// <summary>
        /// Método que exporta el archivo XML de un CFD a formato PDF
        /// </summary>
        /// <param name="bytesArchivoHTML">Retorna los Bytes del XML ya transformado</param>
        /// <param name="ruta_xslt_cfdi">Ruta física del archivo de transformación que se aplicará al CFDI (.xslt)</param>
        /// <param name="ruta_xslt_co">Ruta física del archivo de transformación que se aplicará a la Cadena Original (.xslt)</param>
        /// <param name="ruta_xslt_co_alterna">Ruta física del archivo de transformación que se aplicará a la Candena Original Alterna (.xslt)</param>
        /// <param name="ruta_css_cfdi">Ruta física del archivo de Estilos en Cascada (CSS) que se aplicará a la transformación del CFDI</param>
        /// <returns></returns>
        public RetornoOperacion GeneraComprobanteFiscalDigitalHTML(out byte[] bytesArchivoHTML, string ruta_xslt_cfdi, string ruta_xslt_co, string ruta_xslt_co_alterna, string ruta_css_cfdi)
        {
            //Declarando variable de resultado del proceso
            RetornoOperacion resultado = new RetornoOperacion();

            bytesArchivoHTML = null;

            //Declaramos Memory Stream
            MemoryStream flujoHTML = null;

            //Obteneiendo el flujo del archivo HTML creado a partir del CFD 
            resultado = transformaCFDXML(out flujoHTML, ruta_xslt_cfdi, ruta_xslt_co, ruta_xslt_co_alterna, ruta_css_cfdi);

            //Validamos Transformacion
            if (resultado.OperacionExitosa)
            {
                //Obteniendo arreglo de bytes en HTML
                bytesArchivoHTML = Flujo.ConvierteFlujoABytes(flujoHTML);

                //Si hay resultado
                if (bytesArchivoHTML.Length > 0)
                {
                    //Reemplazando rutas de CSS e Imagen QR
                    //str

                    //Indicando creación ncorrecta
                    resultado = new RetornoOperacion("Generación Correcta de versión HTML.");
                }
            }

            //Devolvinado valor de error
            return resultado;
        }
       
       /// <summary>
        /// Realiza la cancelación del Timbre Fiscal Digital ante el SAT, actualizando el acuse obtenido
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CancelaTimbreFiscalDigital(int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante);
            XDocument acuseXml = null;
              //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si el comprobante se ha timbrado
                if (this._generado)
                {
                    //Instanciando timbre del comprobante
                    using (TimbreFiscalDigital t = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante))
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
                                                resultado = PacCompaniaEmisor.CancelaTimbrePAC(em.rfc, cer.CertificadoBase64, p.ToXmlString(true), t.UUID, out acuseXml, this._id_compania_emisor);
                                                //Si no existe error
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Realizando inserción de registros acuse 
                                                    RetornoOperacion ra = AcuseCancelacion.InsertaAcuseCancelacion(resultado.IdRegistro.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), acuseXml, id_usuario);


                                                    //Si no hay error
                                                    if (ra.OperacionExitosa)
                                                        //Insertando detalle
                                                        ra = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(ra.IdRegistro, t.id_timbre_fiscal_digital, 1, id_usuario);

                                                    //Si no hay errores
                                                    if (ra.OperacionExitosa)
                                                         
                                                        ra = Bitacora.InsertaBitacora(119, this._id_comprobante, 6097, "", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

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
                    resultado = new RetornoOperacion("El comprobante no ha sido timbrado aún.");

            }

            //Devolviendo resultado
            return resultado;
        }
       
        #endregion

        #region Métodos Importación Registros
        
        /// <summary>
        /// Realiza la importación de un comprobante a partir del esquema valido ya definido
        /// </summary>
        /// <param name="comprobante"></param>
        /// <param name="conceptos"></param>
        /// <param name="descuentos"></param>
        /// <param name="impuestos"></param>
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
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion 
            ImportaComprobante_V3_2(DataTable comprobante, DataTable conceptos, DataTable descuentos, DataTable impuestos, DataTable impuestos_conceptos,
                                                       DataTable referencias, DataTable referencias_conceptos, OrigenDatos origen_datos, DataTable referencias_impresion_comprobante, 
                                                       DataTable referencias_impresion_concepto, int id_compania_uso, int  id_sucursal, bool impresion_detalle, DateTime fecha_cambio, int id_usuario)
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
                    using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(id_compania_uso, c["RFC_Emisor"].ToString()))
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
                                    using (CompaniaEmisorReceptor re = new CompaniaEmisorReceptor(id_compania_uso, c["RFC_Receptor"].ToString()))
                                    {
                                        //Si existe el receptor
                                        if (re.id_compania_emisor_receptor > 0)
                                        {
                                             //Instanciando receptor
                                            using (Sucursal su = new Sucursal(id_sucursal))
                                            {
                                                //Instanciamos dirección sucursañ
                                                using (Direccion direccionS = new Direccion(su.id_direccion))
                                                {
                                                    //Creamos la transacción 
                                                    using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                    {

                                                        //Realizando la inserción correspondiente
                                                        resultado = Comprobante.InsertaComprobante((TipoComprobante)Convert.ToInt32(c["Id_Tipo_Comprobante"]), (byte)origen_datos, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Versión Comprobante"), Convert.ToByte(c["Id_Forma_Pago"]),
                                                            Convert.ToByte(c["Id_Condiciones_Pago"]), Convert.ToByte(c["Id_Metodo_Pago"]), Convert.ToInt32(c["No_Parcialidad"]), Convert.ToInt32(c["Total_Parcialidades"]), Convert.ToByte(c["Id_Moneda"]), fecha_cambio == DateTime.MinValue ? DateTime.MinValue : fecha_cambio,
                                                            em.id_compania_emisor_receptor, em.id_direccion, direccionS.id_direccion != 0 ? su.id_sucursal: 0, direccionS.id_direccion, re.id_compania_emisor_receptor, re.id_direccion, string.Format("{0}, {1}", le.municipio, Catalogo.RegresaDescripcionCatalogo(16, le.id_estado)),
                                                                                        c["id_cuenta_pago"].ToString(), "", 0, DateTime.MinValue, 0, id_usuario);

                                                        //Guardando Id de registro
                                                        int id_comprobante = resultado.IdRegistro;

                                                        //Si se registró correctamente el encabezado
                                                        if (resultado.OperacionExitosa)
                                                            resultado = importaConceptosComprobante_V3_2(conceptos, referencias_conceptos, referencias_impresion_concepto, id_comprobante, impresion_detalle, id_usuario);

                                                        //Registrando Descuentos aplicables en caso de no encontrar errores
                                                        if (resultado.OperacionExitosa)
                                                            resultado = importaDescuentosComprobante_V3_2(descuentos, id_comprobante, id_usuario);

                                                        //Si no existen errores en descuentos
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Si hay impuestos que registrar
                                                            if (Validacion.ValidaOrigenDatos(impuestos))
                                                            {
                                                                //Realizando inserción de encabezado
                                                                resultado = Impuesto.InsertaImpuesto(id_comprobante, 0, 0, 0, 0, id_usuario);

                                                                //Si no existe ningún error
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Recuperando Id de Encabezado de Impuesto
                                                                    int id_impuesto = resultado.IdRegistro;

                                                                    //Para cada uno de los detalles
                                                                    foreach (DataRow di in impuestos.Rows)
                                                                    {
                                                                        //Realizando la inserción de detalles
                                                                        resultado = DetalleImpuesto.InsertaDetalleImpuesto(id_impuesto, Convert.ToInt32(di["Id_tipo_detalle"]), Convert.ToInt32(di["Id_impuesto_retenido"]),
                                                                                                            Convert.ToInt32(di["Id_impuesto_trasladado"]), Convert.ToDecimal(di["Tasa"]), Convert.ToDecimal(di["Importe_captura"]),
                                                                                                            Convert.ToDecimal(di["Importe_moneda_nacional"]), id_usuario);

                                                                        //Si no existe error
                                                                        if (resultado.OperacionExitosa)
                                                                            //Actualizando Id de Importación para registro posterior de detalles ligados a conceptos
                                                                            di.SetField<int>("Id_Importacion", resultado.IdRegistro);
                                                                        else
                                                                            //Saliendo de ciclo
                                                                            break;
                                                                    }

                                                                    //Si no existen errores en impuestos
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Validando la existencia de relaciones con conceptos
                                                                        if (Validacion.ValidaOrigenDatos(impuestos_conceptos))
                                                                        {
                                                                            //Para cada relación
                                                                            foreach (DataRow ic in impuestos_conceptos.Rows)
                                                                            {
                                                                                //Recuperando datos de Impuesto y concepto al que pertenece (Id de importación previa)
                                                                                int id_concepto = (from DataRow r in conceptos.Rows
                                                                                                   where r.Field<int>("Id_concepto_origen") == ic.Field<int>("Id_concepto_origen")
                                                                                                   select r.Field<int>("Id_Importacion")).FirstOrDefault();
                                                                                int id_detalle_impuesto = (from DataRow r in impuestos.Rows
                                                                                                           where r.Field<int>("Id_impuesto_origen") == ic.Field<int>("Id_impuesto_origen")
                                                                                                           select r.Field<int>("Id_Importacion")).FirstOrDefault();

                                                                                //Verificando que los valores se hayan recuperado correctamente
                                                                                if (id_concepto > 0 && id_detalle_impuesto > 0)
                                                                                {
                                                                                    //realziando inserción de relación
                                                                                    resultado = ConceptoDetalleImpuesto.InsertarConceptoDetalleImpuesto(id_concepto, id_detalle_impuesto, Convert.ToDecimal(ic["Importe_captura"]),
                                                                                                                                        Convert.ToDecimal(ic["Importe_moneda_nacional"]), id_usuario);
                                                                                }
                                                                                else
                                                                                    resultado = new RetornoOperacion("Imposible encontrar relación entre impuesto y concepto.");

                                                                                //Si existe algún error
                                                                                if (!resultado.OperacionExitosa)
                                                                                    //Saliendo de ciclo
                                                                                    break;
                                                                            }
                                                                        }
                                                                        else
                                                                            resultado = new RetornoOperacion("No se ha indicado la relación entre impuestos y conceptos.");
                                                                    }

                                                                    //Si no hay errores 
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Instanciando impuestos
                                                                        using (Impuesto i = new Impuesto(id_impuesto))
                                                                        {
                                                                            //Si el impuesto existe
                                                                            if (i.id_impuesto > 0)
                                                                            {
                                                                                //Actualziando encabezado de impuestos
                                                                                resultado = i.ActualizaTotalImpuesto(id_usuario);

                                                                                //Si no hay errores
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Instanciamos Comprobante
                                                                                    using (Comprobante objComprobante = new Comprobante(id_comprobante))
                                                                                    {
                                                                                        //Actualizamos Total Impuestos Comprobante
                                                                                        resultado = objComprobante.ActualizaImpuestosComprobante(id_impuesto, id_usuario);
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                                resultado = new RetornoOperacion("Encabezado de Impuesto no encontrado.");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        //Si no hay errores
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Armando Refencia de Concepto para Impreción de Comprobante
                                                            AgregaReferenciaComprobante(referencias_impresion_comprobante, referencias, 1, 119, 2102,true);
                                                           
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

                                                            //Validamos Resultado
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Finalizamos Transaccion
                                                                scope.Complete();
                                                            }
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
        ///  Realiza la deshabilitación del comprobante
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaComprobante(int id_usuario)
        {
            //Establecemos Mensaje error
            RetornoOperacion resultado = new RetornoOperacion();
            
             //Si el comprobante no se ha timbrado
            if (!this._generado)
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciamos Relacion Facturado facturacion
                    using (FacturadoFacturacion objFacturadoFacturacion = new FacturadoFacturacion(FacturadoFacturacion.ObtieneRelacionFacturaElectronica(this._id_comprobante)))
                    {
                        //Validamos que exista Relación
                        if (objFacturadoFacturacion.id_facturado_facturacion > 0)
                        {
                            //Validamos el Origen de Datos
                            if ((OrigenDatos)this._id_origen_datos == OrigenDatos.Facturado)
                            {
                                //Deshablitamos Relación
                                resultado = objFacturadoFacturacion.DeshabilitaFacturadoFacturacion(id_usuario);
                            }
                            else if ((OrigenDatos)this._id_origen_datos == OrigenDatos.FacturaGlobal)
                            {
                                //Instanciamos Factura Global
                                using (FacturaGlobal objFacturaGlobal = new FacturaGlobal(objFacturadoFacturacion.id_factura_global))
                                {
                                    //Deshabilitamos Factura Global
                                    resultado = objFacturaGlobal.ReversaFacturaGlobal(id_usuario);
                                }
                            }
                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                resultado = editaComprobante(this._serie, this._folio, this._id_certificado, this.tipo_comprobante, this._id_origen_datos, this.estatus_comprobante, this._version, this._sello, this._id_forma_pago, this._id_condiciones_pago, this._id_metodo_pago, this._no_parcialidad, this._total_parcialidades, this._id_moneda, this._fecha_tipo_cambio, this._subtotal_moneda_captura, this._subtotal_moneda_nacional,
                                    this._descuento_moneda_captura, this._descuento_moneda_nacional, this._impuestos_moneda_captura, this._impuestos_moneda_nacional, this._total_moneda_captura, this._total_moneda_nacional, this._id_compania_emisor, this._id_direccion_emisor, this._id_sucursal, this._id_direccion_sucursal, this._id_compania_receptor, this._id_direccion_receptor, this._fecha_captura,
                                    this._fecha_expedicion, this._fecha_cancelacion, this._generado, this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_cancelado, this._id_transferido_cancelado, this._lugar_expedicion, this._id_cuenta_pago, this._serie_folio_original, this._folio_original, this._fecha_folio_original, this._monto_folio_original, id_usuario, false);
                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Completamos Transacción
                                    scope.Complete();
                                }
                            }
                        }
                        else
                        {
                            //Establecmos mensaje
                            resultado = new RetornoOperacion("No se puede recuperar datos complementarios Facturado Facturación.");
                        }
                    }
                }
            }
            else
                        resultado = new RetornoOperacion("El comprobante ya se ha timbrado, no es posible editarlo.");
            //Devolvemos Resultado
            return resultado;
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
        public static RetornoOperacion ImportaReciboNomina_V3_2(DataTable comprobante, int id_compania, int id_sucursal, DataTable conceptos, DataTable referencias_conceptos, DataTable descuentos, DataTable impuestos, DataTable impuestos_conceptos, int id_usuario)
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
                        using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor( id_compania, c["RFC_Emisor"].ToString()))
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
                                                //Realizando la inserción correspondiente
                                                resultado = Comprobante.InsertaComprobante((TipoComprobante)Convert.ToInt32(c["Id_Tipo_Comprobante"]), (byte)Comprobante.OrigenDatos.ReciboNomina, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Versión Comprobante"), Convert.ToByte(c["Id_Forma_Pago"]),
                                                                                Convert.ToByte(c["Id_Condiciones_Pago"]), Convert.ToByte(c["Id_Metodo_Pago"]), Convert.ToInt32(c["No_Parcialidad"]), Convert.ToInt32(c["Total_Parcialidades"]), Convert.ToByte(c["Id_Moneda"]), DateTime.Today,
                                                                                em.id_compania_emisor_receptor, em.id_direccion, objSucursal.id_direccion!= 0? objSucursal.id_sucursal: 0, objSucursal.id_direccion, 0, 0, string.Format("{0}, {1}", le.municipio, Catalogo.RegresaDescripcionCatalogo(16, le.id_estado)),
                                                                                c["id_cuenta_pago"].ToString(), "", 0, DateTime.MinValue, 0, id_usuario);

                                                //Guardando Id de registro
                                                int id_comprobante = resultado.IdRegistro;

                                                //Si se registró correctamente el encabezado
                                                if (resultado.OperacionExitosa)
                                                   
                                                    resultado = importaConceptosComprobante_V3_2(conceptos, referencias_conceptos, null, id_comprobante, false, id_usuario);

                                                //Registrando Descuentos aplicables en caso de no encontrar errores
                                                if (resultado.OperacionExitosa)
                                                    resultado = importaDescuentosComprobante_V3_2(descuentos, id_comprobante, id_usuario);

                                                //Si no existen errores en descuentos
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Si hay impuestos que registrar
                                                    if (Validacion.ValidaOrigenDatos(impuestos))
                                                    {
                                                        //Realizando inserción de encabezado
                                                        resultado = Impuesto.InsertaImpuesto(id_comprobante, 0, 0, 0, 0, id_usuario);

                                                        //Si no existe ningún error
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Recuperando Id de Encabezado de Impuesto
                                                            int id_impuesto = resultado.IdRegistro;

                                                            //Para cada uno de los detalles
                                                            foreach (DataRow di in impuestos.Rows)
                                                            {
                                                                //Realizando la inserción de detalles
                                                                resultado = DetalleImpuesto.InsertaDetalleImpuesto(id_impuesto, Convert.ToInt32(di["Id_tipo_detalle"]), Convert.ToInt32(di["Id_impuesto_retenido"]),
                                                                                                    Convert.ToInt32(di["Id_impuesto_trasladado"]), Convert.ToDecimal(di["Tasa"]), Convert.ToDecimal(di["Importe_captura"]),
                                                                                                    Convert.ToDecimal(di["Importe_captura"]), id_usuario);

                                                                //Si no existe error
                                                                if (resultado.OperacionExitosa)
                                                                    //Actualizando Id de Importación para registro posterior de detalles ligados a conceptos
                                                                    di.SetField<int>("Id_Importacion", resultado.IdRegistro);
                                                                else
                                                                    //Saliendo de ciclo
                                                                    break;
                                                            }

                                                            //Si no existen errores en impuestos
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Validando la existencia de relaciones con conceptos
                                                                if (Validacion.ValidaOrigenDatos(impuestos_conceptos))
                                                                {
                                                                    //Para cada relación
                                                                    foreach (DataRow ic in impuestos_conceptos.Rows)
                                                                    {
                                                                        //Recuperando datos de Impuesto y concepto al que pertenece (Id de importación previa)
                                                                        int id_concepto = (from DataRow r in conceptos.Rows
                                                                                           where r.Field<int>("Id_concepto_origen") == ic.Field<int>("Id_concepto_origen")
                                                                                           select r.Field<int>("Id_Importacion")).FirstOrDefault();
                                                                        int id_detalle_impuesto = (from DataRow r in impuestos.Rows
                                                                                                   where r.Field<int>("Id_impuesto_origen") == ic.Field<int>("Id_impuesto_origen")
                                                                                                   select r.Field<int>("Id_Importacion")).FirstOrDefault();

                                                                        //Verificando que los valores se hayan recuperado correctamente
                                                                        if (id_concepto > 0 && id_detalle_impuesto > 0)
                                                                        {
                                                                            //realziando inserción de relación
                                                                            resultado = ConceptoDetalleImpuesto.InsertarConceptoDetalleImpuesto(id_concepto, id_detalle_impuesto, Convert.ToDecimal(ic["Importe_captura"]),
                                                                                                                                Convert.ToDecimal(ic["Importe_captura"]), id_usuario);
                                                                        }
                                                                        else
                                                                            resultado = new RetornoOperacion("Imposible encontrar relación entre impuesto y concepto.");

                                                                        //Si existe algún error
                                                                        if (!resultado.OperacionExitosa)
                                                                            //Saliendo de ciclo
                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                    resultado = new RetornoOperacion("No se ha indicado la relación entre impuestos y conceptos.");
                                                            }

                                                            //Si no hay errores 
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Instanciando impuestos
                                                                using (Impuesto i = new Impuesto(id_impuesto))
                                                                {
                                                                    //Si el impuesto existe
                                                                    if (i.id_impuesto > 0)
                                                                    {
                                                                        //Actualziando encabezado de impuestos
                                                                        resultado = i.ActualizaTotalImpuesto(id_usuario);

                                                                        //Si no hay errores
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Instanciamos Comprobante
                                                                            using (Comprobante objComprobante = new Comprobante(id_comprobante))
                                                                            {
                                                                                //Actualizamos Total Impuestos Comprobante
                                                                                resultado = objComprobante.ActualizaImpuestosComprobante(id_impuesto, id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion("Encabezado de Impuesto no encontrado.");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }


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
        public static RetornoOperacion ImportaReciboNominaActualizacion1_V3_2(DataTable comprobante, int id_compania, int id_sucursal, DataTable conceptos, DataTable referencias_conceptos, DataTable descuentos, DataTable impuestos, DataTable impuestos_conceptos, int id_usuario)
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
                        using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(id_compania, c["RFC_Emisor"].ToString()))
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
                                                //Realizando la inserción correspondiente
                                                resultado = Comprobante.InsertaComprobante((TipoComprobante)Convert.ToInt32(c["Id_Tipo_Comprobante"]), (byte)Comprobante.OrigenDatos.ReciboNomina, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Versión Comprobante"), Convert.ToByte(c["Id_Forma_Pago"]),
                                                                                Convert.ToByte(c["Id_Condiciones_Pago"]), Convert.ToByte(c["Id_Metodo_Pago"]), Convert.ToInt32(c["No_Parcialidad"]), Convert.ToInt32(c["Total_Parcialidades"]), Convert.ToByte(c["Id_Moneda"]), DateTime.Today,
                                                                                em.id_compania_emisor_receptor, em.id_direccion, objSucursal.id_direccion != 0 ? objSucursal.id_sucursal : 0, objSucursal.id_direccion, 0, 0, le.codigo_postal,
                                                                                c["id_cuenta_pago"].ToString(), "", 0, DateTime.MinValue, 0, id_usuario);

                                                //Guardando Id de registro
                                                int id_comprobante = resultado.IdRegistro;

                                                //Si se registró correctamente el encabezado
                                                if (resultado.OperacionExitosa)

                                                    resultado = importaConceptosComprobante_V3_2(conceptos, referencias_conceptos, null, id_comprobante, false, id_usuario);

                                                //Registrando Descuentos aplicables en caso de no encontrar errores
                                                if (resultado.OperacionExitosa)
                                                    resultado = importaDescuentosComprobante_V3_2(descuentos, id_comprobante, id_usuario);

                                                //Si no existen errores en descuentos
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Si hay impuestos que registrar
                                                    if (Validacion.ValidaOrigenDatos(impuestos))
                                                    {
                                                        //Realizando inserción de encabezado
                                                        resultado = Impuesto.InsertaImpuesto(id_comprobante, 0, 0, 0, 0, id_usuario);

                                                        //Si no existe ningún error
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Recuperando Id de Encabezado de Impuesto
                                                            int id_impuesto = resultado.IdRegistro;

                                                            //Para cada uno de los detalles
                                                            foreach (DataRow di in impuestos.Rows)
                                                            {
                                                                //Realizando la inserción de detalles
                                                                resultado = DetalleImpuesto.InsertaDetalleImpuesto(id_impuesto, Convert.ToInt32(di["Id_tipo_detalle"]), Convert.ToInt32(di["Id_impuesto_retenido"]),
                                                                                                    Convert.ToInt32(di["Id_impuesto_trasladado"]), Convert.ToDecimal(di["Tasa"]), Convert.ToDecimal(di["Importe_captura"]),
                                                                                                    Convert.ToDecimal(di["Importe_captura"]), id_usuario);

                                                                //Si no existe error
                                                                if (resultado.OperacionExitosa)
                                                                    //Actualizando Id de Importación para registro posterior de detalles ligados a conceptos
                                                                    di.SetField<int>("Id_Importacion", resultado.IdRegistro);
                                                                else
                                                                    //Saliendo de ciclo
                                                                    break;
                                                            }

                                                            //Si no existen errores en impuestos
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Validando la existencia de relaciones con conceptos
                                                                if (Validacion.ValidaOrigenDatos(impuestos_conceptos))
                                                                {
                                                                    //Para cada relación
                                                                    foreach (DataRow ic in impuestos_conceptos.Rows)
                                                                    {
                                                                        //Recuperando datos de Impuesto y concepto al que pertenece (Id de importación previa)
                                                                        int id_concepto = (from DataRow r in conceptos.Rows
                                                                                           where r.Field<int>("Id_concepto_origen") == ic.Field<int>("Id_concepto_origen")
                                                                                           select r.Field<int>("Id_Importacion")).FirstOrDefault();
                                                                        int id_detalle_impuesto = (from DataRow r in impuestos.Rows
                                                                                                   where r.Field<int>("Id_impuesto_origen") == ic.Field<int>("Id_impuesto_origen")
                                                                                                   select r.Field<int>("Id_Importacion")).FirstOrDefault();

                                                                        //Verificando que los valores se hayan recuperado correctamente
                                                                        if (id_concepto > 0 && id_detalle_impuesto > 0)
                                                                        {
                                                                            //realziando inserción de relación
                                                                            resultado = ConceptoDetalleImpuesto.InsertarConceptoDetalleImpuesto(id_concepto, id_detalle_impuesto, Convert.ToDecimal(ic["Importe_captura"]),
                                                                                                                                Convert.ToDecimal(ic["Importe_captura"]), id_usuario);
                                                                        }
                                                                        else
                                                                            resultado = new RetornoOperacion("Imposible encontrar relación entre impuesto y concepto.");

                                                                        //Si existe algún error
                                                                        if (!resultado.OperacionExitosa)
                                                                            //Saliendo de ciclo
                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                    resultado = new RetornoOperacion("No se ha indicado la relación entre impuestos y conceptos.");
                                                            }

                                                            //Si no hay errores 
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Instanciando impuestos
                                                                using (Impuesto i = new Impuesto(id_impuesto))
                                                                {
                                                                    //Si el impuesto existe
                                                                    if (i.id_impuesto > 0)
                                                                    {
                                                                        //Actualziando encabezado de impuestos
                                                                        resultado = i.ActualizaTotalImpuesto(id_usuario);

                                                                        //Si no hay errores
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Instanciamos Comprobante
                                                                            using (Comprobante objComprobante = new Comprobante(id_comprobante))
                                                                            {
                                                                                //Actualizamos Total Impuestos Comprobante
                                                                                resultado = objComprobante.ActualizaImpuestosComprobante(id_impuesto, id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion("Encabezado de Impuesto no encontrado.");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }


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
        /// Envía los archivos PDF y/o XML del CFDI solicitado como adjuntos en un mensaje de correo electrónico
        /// </summary>
        /// <param name="remitente">Correo electrónico desde el que se hará el envío del correo electrónico</param>
        /// <param name="asunto">Asunto que llevará el correo electrónico</param>
        /// <param name="mensaje">Mensaje del correo electrónico</param>
        /// <param name="destinatarios">Conjunto de destinatarios a los que será enviado el correo electrónico</param>
        /// <param name="pdf">True para adjuntar el archivo pdf</param>
        /// <param name="xml">True para adjuntar el archivo xml</param>
        /// <returns></returns>
        public RetornoOperacion EnviaArchivosEmail(string remitente, string asunto, string mensaje, string[] destinatarios, bool pdf, bool xml)
        { 
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante);

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
                    if((OrigenDatos)this._id_origen_datos== OrigenDatos.ReciboNomina)
                    {
                        //Obtenemos Bytes Comprobante de Nómina
                        //Obtenemos Id de Nómina de Empleado
                        int id_nomina_empleado = Nomina.NomEmpleado.ObtieneIdNomEmpleado(this._id_comprobante);

                        //Instanciamos Nómina Empleado
                        using (Nomina.NomEmpleado objNomEmpleado = new Nomina.NomEmpleado(id_nomina_empleado))
                        {
                            //Obtenmos Bytes de la Nómina Empleado
                            PDF = objNomEmpleado.GeneraPDFComprobanteNomina();
                        }
                    }
                    else
                    {
                        //Obtenemos Bytes de Comprobante
                        PDF = GeneraPDFComprobante();
                    }
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
                    SAT_CL.Global.Referencia.InsertaReferencia(this._id_comprobante, 119, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 119, "Envio E-mail", 0, "E-Mail (Enviados)"),  string.Join(";", destinatarios), Fecha.ObtieneFechaEstandarMexicoCentro(), 9, true);
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera el archivo .pdf del CFDI
        /// </summary>
        /// <returns></returns>
        public byte[] GeneraPDFComprobante()
        {
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Creación de la tabla para cargar el Logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));

            //Creando nuevo visor de reporte
            ReportViewer rvReporte = new Microsoft.Reporting.WebForms.ReportViewer();

            //Habilita las imagenes externas en reporte
            rvReporte.LocalReport.EnableExternalImages = true;

            //Asignación de la ubicación del reporte local
            rvReporte.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/RDLC/Comprobante.rdlc");
            //Carga Conceptos del Comprobante
            using (DataTable Concepto = SAT_CL.FacturacionElectronica.Concepto.CargaImpresionConceptos(this._id_comprobante))
            {
                //Agregar origen de datos 
                ReportDataSource rsComprobanteCFDI = new ReportDataSource("ComprobanteCFDI", Concepto);
                //Asigna los valores al conjunto de datos
                rvReporte.LocalReport.DataSources.Add(rsComprobanteCFDI);
            }

            //Valida el estatus del comprobante.
            if (this.estatus_comprobante.Equals(SAT_CL.FacturacionElectronica.Comprobante.EstatusComprobante.Cancelado))
            {
                //Si el estatus es cancelado  envia al parametro estatusComprobante la leyenda CANCELADO
                ReportParameter estatusComprobante = new ReportParameter("EstatusComprobante", "CANCELADO");
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusComprobante });
            }
            //En caso contrario no envia nada al parametro estatusComprobante
            else
            {
                ReportParameter estatusComprobante = new ReportParameter("EstatusComprobante", "");
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusComprobante });
            }

            //Intsanciamos Compania Emisor
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(this._id_compania_emisor))
            {
                //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                byte[] imagen = null;

                //Permite capturar errores en caso de que no exista una ruta para el archivo
                try
                {
                    //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                    imagen = System.IO.File.ReadAllBytes(this._ruta_codigo_bidimensional);
                }
                //En caso de que no exista una imagen, se devolvera un valor nulo.
                catch { imagen = null; }
                //Agrega a la tabla un registro con valor a la ruta de la imagen.
                dtCodigo.Rows.Add(imagen);

                //Creación del arreglo necesario para la carga de la ruta del logotipo y del 
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
                rvReporte.LocalReport.DataSources.Add(rvs);
                rvReporte.LocalReport.DataSources.Add(rvscod);

                //Asigna el origen de datos a los parametros, obtenidos de la instancia a la clase compañiaEmisorReceptor
                ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
                ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);

                //Instancia a la clase Direccion para obtener la dirección del emisor
                using (SAT_CL.Global.Direccion direm = new SAT_CL.Global.Direccion(this._id_direccion_emisor))
                {
                    //Asigna valores a los parametros obtendos de la instancia a la clase Dirección.
                    ReportParameter direccionEmisorMatriz = new ReportParameter("DireccionEmisorMatriz", direm.ObtieneDireccionCompleta());
                    ReportParameter direccionEmisorSucursal = new ReportParameter("DireccionEmisorSucursal", direm.ObtieneDireccionCompleta());
                    //Asigna valores a los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, direccionEmisorMatriz, direccionEmisorSucursal });
                }
            }

            //Instancia a la compania Receptor
            using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(this._id_compania_receptor))
            {   
                //Asigna valores a los parametros obtendos de la instancia a la clase companiaEmisorReceptor
                ReportParameter razonSocialReceptorCFDI = new ReportParameter("RazonSocialReceptorCFDI", receptor.nombre);
                ReportParameter rfcReceptorCFDI = new ReportParameter("RFCReceptorCFDI", receptor.rfc);
                //Obtiene la dirección del receptor
                using (SAT_CL.Global.Direccion dirre = new SAT_CL.Global.Direccion(this._id_direccion_receptor))
                {
                    //Asigna valores a los parametros obtenidos de la instancia a la clase Direccion
                    ReportParameter direccionReceptorCFDI = new ReportParameter("DireccionReceptorCFDI", dirre.ObtieneDireccionCompleta());
                    //Asigna valores a los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialReceptorCFDI, rfcReceptorCFDI, direccionReceptorCFDI });
                }
            }

            //Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
            SAT_CL.FacturacionElectronica.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante);
            //Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
            ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
            ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);

            //Generando cadena original
            string cadenaOriginal = "";
            SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(this._ruta_xml, System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"),
                                                        System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"), out cadenaOriginal);


            ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
            ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
            ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
            ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
            //Asigna valores a los parametros del reporteComprobante
            rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });


            //Instancia para la obtencion de los datos de sucursal
            using (SAT_CL.Global.Sucursal suc = new SAT_CL.Global.Sucursal(this._id_sucursal))
            {
                //Asigna los valores obtenidos de la instanca a la clase sucursal
                ReportParameter sucursalCFDI = new ReportParameter("SucursalCFDI", suc.nombre);
                //Asigna valores a los parametros del reporteComprobante
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { sucursalCFDI });
            }

            //Instanciamos a la clase Certificado
            using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(this._id_certificado))
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

            //Instancia a la clase Cuenta bancos 
            using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos(this._id_cuenta_pago))
            {
                if (this.id_cuenta_pago == 0 || cb.num_cuenta == "NO IDENTIFICADO")
                {
                    //Asigna los valores de la clase cuentaBancos a los parametros
                    ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", "No Identificado");
                    //Asigna valores de los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                }
                else
                {
                    //Obtiene el bit que mostrara la cuenta completa o no
                    string cuentaBancoCompleta = SAT_CL.Global.Referencia.CargaReferencia("0", 25, this.id_compania_receptor, "Leyendas Impresión CFD", "Bit Cuenta Banco Completa");
                    //Valida si la cuentaBancoCompleta es true, muestra la cuenta completa tal y como se dio de alta en el sistema
                    if (cuentaBancoCompleta == "TRUE")
                    {
                        //Asigna los valores de la clase cuentaBancos a los parametros
                        ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", cb.num_cuenta);
                        //Asigna valores de los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                    }
                    //En caso contrario solo muestra los ultimos 4 digitos
                    else
                    {
                        //Asigna los valores de la clase cuentaBancos a los parametros
                        ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.InvierteCadena(cb.num_cuenta).Substring(0, 4)));
                        //Asigna valores de los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                    }
                }
                //Si es Bit de banco Cunta Pagoes true
                string BitBanco = SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_receptor, "Leyendas Impresión CFD", "Bit Banco Cuenta Pago");
                if (BitBanco == "TRUE" || BitBanco == "true")
                {

                    //Instanciamos Cuenta
                    using (SAT_CL.Bancos.Banco objBanco = new SAT_CL.Bancos.Banco(cb.id_banco))
                    {
                        //Si la cadena es vacia muestra la descripción del método de págo
                        ReportParameter bancoCFDI = new ReportParameter("BancoCFDI", objBanco.nombre_corto);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { bancoCFDI });
                    }


                }
                //En caso de que el bit código método de págo no sea true mostrara la descripción del método de pago
                else
                {
                    ReportParameter bancoCFDI = new ReportParameter("BancoCFDI", " ");
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { bancoCFDI });
                }
            }

            //Asigna los valores de la clase comprobante a los parametros 
            ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", this._fecha_expedicion.ToString());
            ReportParameter serieCFDI = new ReportParameter("SerieCFDI", this._serie);
            ReportParameter folio = new ReportParameter("Folio", this._folio.ToString());

            //ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, this._id_metodo_pago) + "  -  " + SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(80, this._id_metodo_pago));
            ReportParameter formaPagoCFDI = new ReportParameter("FormaPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1099, this._id_forma_pago));
            ReportParameter totalCFDI = new ReportParameter("TotalCFDI", this._total_moneda_captura.ToString());
            ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_emisor, "Facturacion Electronica", "Regimen Fiscal"));
            ReportParameter leyendaImpresionCFDI1 = new ReportParameter("LeyendaImpresionCFDI1", SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD1"));
            ReportParameter leyendaImpresionCFDI2 = new ReportParameter("LeyendaImpresionCFDI2", SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_emisor, "Parametros Impresión CFD", "Total Comprobante") == "SI" ? SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2").Replace("TotalComprobante", this._total_moneda_nacional.ToString() + " (" + TSDK.Base.Cadena.ConvierteMontoALetra(this._total_moneda_nacional.ToString()) + ") ") : SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2"));
            ReportParameter comentario = new ReportParameter("Comentario", SAT_CL.Global.Referencia.CargaReferencia("0", 119, this._id_comprobante, "Facturacion Electrónica", "Comentario"));
            ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_emisor, "Color Empresa", "Color"));
            ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", this._subtotal_moneda_captura.ToString());

            //Asigna valores a los parametros del reporteComprobante                      
            rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,regimenFiscalCFDI,formaPagoCFDI,
                                                                                  totalCFDI,leyendaImpresionCFDI1, leyendaImpresionCFDI2, comentario, subtotalCFDI, color});
            //Obtiene el tipo de moneda con el cual se factura electronicamente
            int moneda = this.id_moneda;
            //Acorde al tipo de moneda Valida
            switch (moneda)
            {
                //Si es moneda Pesos Muestra
                case 1:
                    {
                        //Importe con letra en pesos, y muestra el signo de pesos
                        ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(this.total_moneda_captura.ToString()));
                        ReportParameter mon = new ReportParameter("Mon", "");
                        ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "");
                        ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                        ReportParameter figEuro = new ReportParameter("FigEuro", "");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, mon, tipoMoneda, figPeso, figEuro });   
                        break;
                    }
                //Si es Dolares
                case 2:
                    {
                        //Importe con letra en dolares, no muestra el signo de pesos y Muetsra el tipo de moneda en Dolares
                        ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(this.total_moneda_captura.ToString(), "DÓLARES", "USD"));
                        ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                        ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "USD");
                        ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                        ReportParameter figEuro = new ReportParameter("FigEuro", "");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, mon, tipoMoneda, figPeso, figEuro });   
                        break;
                    }
                //Se es Euro
                case 3:
                    {
                        //Importe con letra en Euros, no muestra el Signo de 
                        ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(this.total_moneda_captura.ToString(), "EUROS", "EUR"));
                        ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "EUR");
                        ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                        ReportParameter figPeso = new ReportParameter("FigPeso", "");
                        ReportParameter figEuro = new ReportParameter("FigEuro", "€");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, tipoMoneda, mon, figPeso, figEuro });
                        break;
                    }
            }
            //Obtiene la referencia bit código método de pago; permite definir si se mostrara el código o la descripción del método de págo definido por el sat.
            string codigo = SAT_CL.Global.Referencia.CargaReferencia("0", 25, this._id_compania_receptor, "Leyendas Impresión CFD", "Bit Código Método Pago");
            //Obtienen el valor cadena del método de págo (Codigo del método de págo definido por el SAT).
            string metodoPago = SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(80, this._id_metodo_pago);
            //Si es codigo es true
            if (codigo == "TRUE")
            {
                //Valida el valor de la cadadena   
                if (metodoPago == "")
                {
                    //Si la cadena es vacia muestra la descripción del método de págo
                    ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, this._id_metodo_pago));
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
                ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, this._id_metodo_pago));
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
            }
            //Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
            SAT_CL.FacturacionElectronica.Impuesto imp = (SAT_CL.FacturacionElectronica.Impuesto.RecuperaImpuestoComprobante(this._id_comprobante));

            //Instancia la clase DetalleImpuesto para obtener el desglose de los impuestos dado un id_impuesto
            using (DataTable detalleImp = SAT_CL.FacturacionElectronica.DetalleImpuesto.CargaDetallesImpuesto(imp.id_impuesto))
            {
                //Declarando variables auxiliares para recuperar impuestos
                decimal totalIvaR = 0, totalIsr = 0, totalIvaT = 0, totalIeps = 0, tasaIvaT = 0, tasaIeps = 0;

                //Si hay impuestos agregados al comprobante
                if (imp.id_impuesto > 0)
                {
                    //Asigna a las variablesel valor del desglose de impuesto 
                    totalIvaR = (from DataRow r in detalleImp.Rows
                                 where r.Field<int>("IdImpuestoRetenido") == 2
                                 select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                    totalIsr = (from DataRow r in detalleImp.Rows
                                where r.Field<int>("IdImpuestoRetenido") == 1
                                select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                    totalIvaT = (from DataRow r in detalleImp.Rows
                                 where r.Field<int>("IdImpuestoTrasladado") == 3
                                 select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                    totalIeps = (from DataRow r in detalleImp.Rows
                                 where r.Field<int>("IdImpuestoTrasladado") == 4
                                 select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                    tasaIeps = (from DataRow r in detalleImp.Rows
                                where r.Field<int>("IdImpuestoTrasladado") == 4
                                select r.Field<decimal>("Tasa")).FirstOrDefault();
                    tasaIvaT = (from DataRow r in detalleImp.Rows
                                where r.Field<int>("IdImpuestoTrasladado") == 3
                                select r.Field<decimal>("Tasa")).FirstOrDefault();
                }

                //Asignación de valores a los parametros                            
                ReportParameter EtiquetaTrasladado = new ReportParameter("EtiquetaTrasladado", totalIvaT.ToString() != "0" ? "I.V.A. Tras " + TSDK.Base.Cadena.TruncaCadena(tasaIvaT.ToString(), 3, "") + " %" : "IEPS " + TSDK.Base.Cadena.TruncaCadena(tasaIeps.ToString(), 3, "") + " %");
                ReportParameter EtiquetaRetenido = new ReportParameter("EtiquetaRetenido", totalIvaR.ToString() != "0" ? "I.V.A. Ret" : "ISR");
                ReportParameter Trasladado = new ReportParameter("Trasladado", totalIvaT.ToString() != "0" ? totalIvaT.ToString() : totalIeps.ToString());
                ReportParameter Retenido = new ReportParameter("Retenido", totalIsr.ToString() != "0" ? totalIsr.ToString() : totalIvaR.ToString());
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { EtiquetaTrasladado, EtiquetaRetenido, Trasladado, Retenido });
            }

            //Carga SubInforma
            rvReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);

            //Devolviendo resultado
            return rvReporte.LocalReport.Render("PDF");
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
            string[] _comprobantes = { this._id_comprobante.ToString() };
            errores = null;
            //Variable de Salida
            int descargasRest; DateTime fechaExp;
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos que la Fcatura se encuentre Timbrada
            if (this.generado)
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
                                        resultado = LinkDescarga.GeneraLinkDescarga(r.Field<int>("Id"), _comprobantes, out fechaExp, out descargasRest, this._id_compania_emisor, id_usuario);
                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Intsanciamos Usuario
                                            using (SAT_CL.Seguridad.Usuario usuario = new Seguridad.Usuario(id_usuario))
                                            {
                                                //Creamos mensaje
                                                resultado = creaMensajeMailDescargaCFDI("", asunto, "", mensaje, "", usuario.nombre,
                                                         CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Página Descarga CFDI") + "?id=" + resultado.IdRegistro.ToString(),
                                                         "Para descargar los CFDI solicitados, siga el siguiente enlace:'{0}'",
                                                         fechaExp.ToString("dd/MM/yyyy"), fechaExp.ToString("HH:mm"), descargasRest.ToString(),
                                                         "El link de descarga caduca el día {0} a las {1} hrs.<br />○ Sólo puede utilizar {2} veces este link.<br />○ Si requiere realizar más descargas, pongase en contacto con nosotros.");
                                            }
                                            //Validamos Resultado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Enviamos Email
                                                //Instanciando Correo Electronico
                                                using (Correo email = new Correo(e_mail_remitente, r.Field<string>("Valor"), asunto, resultado.Mensaje, true))
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
                        resultado = new RetornoOperacion("No hay contactos registrados");
                    }
                }
            }

            else
            {
                //Establecemos Error
                resultado = new RetornoOperacion("La factura aún no se ha timbrado.");
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Crea Mensaje E-mail para la descarga de Archivos
        /// </summary>
        /// <param name="asunto">Asunto</param>
        /// <param name="mensaje_asunto">Mensaje a retornar. Puede utilizar comodines de sustitución para incluir los valores con el siguiente orden: {0} Asunto</param>
        /// <param name="contenido">Contenido</param>
        /// <param name="mensaje_contenido">Mensaje a retornar. Puede utilizar comodines de sustitución para incluir los valores con el siguiente orden: {0} Contenido</param>
        /// <param name="saludo">Saludo</param>
        /// <param name="mensaje_saludo">Mensaje a retornar. Puede utilizar comodines de sustitución para incluir los valores con el siguiente orden: {0} Saludo</param>
        /// <param name="idLinkDes">IdLinkDescarga</param>
        /// <param name="mensaje_link">Mensaje a retornar. Puede utilizar comodines de sustitución para incluir los valores con el siguiente orden: {0} IdLinkDescarga</param>
        /// <param name="nota0">Nota0</param>
        /// <param name="nota1">Nota1</param>
        /// <param name="nota2">Nota2</param>
        /// <param name="mensaje_nota">Mensaje a retornar. Puede utilizar comodines de sustitución para incluir los valores con el siguiente orden: {0} Nota0, {1} Nota1, {2} Nota2</param>
        /// <returns></returns>
        private RetornoOperacion creaMensajeMailDescargaCFDI(string asunto, string mensaje_asunto, string contenido, string mensaje_contenido, string saludo, string mensaje_saludo, string idLinkDes, string mensaje_link,
                                             string nota0, string nota1, string nota2, string mensaje_nota)
        {
                 //Declaramos Variable contenido
                 RetornoOperacion resultado =  new RetornoOperacion();
                //Definiendo el asunto del mensaje de correo
                string _asunto = string.Format(mensaje_asunto, asunto);
                //Definiendo fecha
                string _fecMsj = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                //Mensaje escrito por el ejecutivo
                 string _msjPers = string.Format(mensaje_contenido, contenido);
                //Generando la URL de descarga(Página de descargas más el ID del CFD a descargar)
                string _urlDescarga = string.Format(mensaje_link, idLinkDes);
                string _nota = string.Format(mensaje_nota,nota0, nota1, nota2 ); //nota        
                string _saludo = string.Format(mensaje_saludo, saludo);//Saludo

                //Intsnaciamos el Archivo para la Firma Eletrónica
                using(DataTable mit = ArchivoRegistro.CargaArchivoRegistro(25, this._id_compania_emisor, 18, this._id_compania_emisor))
                {
                    //Validamos Origen
                    if(Validacion.ValidaOrigenDatos(mit))
                    {
                      //Obtenemos Id
                        int id = (from DataRow r in mit.Rows
                                  select r.Field<int>("Id")).FirstOrDefault();
                        //Instanciamos Arcchivo para obtener URL
                        using(ArchivoRegistro objArchivo = new ArchivoRegistro(id))
                        {
                            //Validamos que exista
                            if(objArchivo.id_archivo_registro > 0)
                            {
                                //Declaramos variable
                                string body = "";
                                //Obtenemos archivo
                                body = System.IO.File.ReadAllText(objArchivo.url);

                                //Remplazamos contenido en caso de existir referencias
                                body = _asunto != "" ? body.Replace("#Asunto#", _asunto) : body;
                                body = _fecMsj != "" ? body.Replace("#Fecha#", _fecMsj) : body;
                                body = _msjPers != "" ? body.Replace("#Mensaje#", _msjPers) : body;
                                body = _urlDescarga != "" ? body.Replace("#UrlDescarga#", _urlDescarga) : body;
                                body = _nota != "" ? body.Replace("#Nota#", _nota) : body;
                                body = _saludo != "" ? body.Replace("#Saludo#", _saludo) : body;
                                //Asignamos contenido del mensaje.
                                resultado = new RetornoOperacion(body, true);
                            }
                            else
                            {
                                //Establecemos error
                                resultado = new RetornoOperacion("No se encontró archivo para la firma de correo eletrónico.");
                            }
                        }
                    }
                }
            //Devolvemos resultado
            return resultado;
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
        private static RetornoOperacion importaConceptosComprobante_V3_2(DataTable conceptos, DataTable referencias_conceptos, DataTable referencia_impresion_concepto,
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
                        resultado = Concepto.InsertaConcepto(id_comprobante, Convert.ToDecimal(cn["Cantidad"]), Convert.ToInt32(cn["Id_unidad"]), Convert.ToInt32(cn["Descripcion"]), cn["No_identificacion"].ToString(),
                                                            Convert.ToDecimal(cn["Valor_unitario"]), Convert.ToDecimal(cn["Importe_captura"]), Convert.ToDecimal(cn["Importe_moneda_nacional"]), id_usuario);
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
                        AgregaReferenciaComprobante(referencia_impresion_concepto, referencias_conceptos, Convert.ToInt32(cn["Id_concepto_origen"]), 121, 2099, impresion_detalles);
                        
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
                    }
                    //Actualizamos Total Comprobante
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciamos Comprobante
                        using (Comprobante objComprobante = new Comprobante(id_comprobante))
                        {
                            //Actualizamos Total Comprobante
                            resultado = objComprobante.ActualizaSubtotalComprobante(id_usuario);
                            //Validamos Resultado
                            if(resultado.OperacionExitosa)
                            {
                                scope.Complete();
                            }
                        }
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
        /// Método encargado para Obtener el Armado de la Tabla de Referencias Comprobante para su posterior insercción
        /// </summary>
        /// <param name="referencia_comprobante">Referencia Comprobante</param>
        /// <param name="referencia_concepto">Referencia Concepto</param>
        /// <param name="id_concepto_origen">Id Concepto Origen</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_tipo">Id Tipo</param>
        private static void AgregaReferenciaComprobante(DataTable referencia_comprobante,DataTable referencia_concepto, int id_concepto_origen, int id_tabla, int id_tipo, bool detalles)
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
        /// Realiza la importación de registros descuento a partir de la definición de DataTable de Descuentos (Esquema de Importación)
        /// </summary>
        /// <param name="descuentos">Descuentos a Importar</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private static RetornoOperacion importaDescuentosComprobante_V3_2(DataTable descuentos, int id_comprobante, int id_usuario)
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
                        resultado = Descuento.InsertaDescuento(Convert.ToInt32(des["Id_motivo_descuento"]), id_comprobante, Convert.ToDecimal(des["Porcentaje"]),
                                                    Convert.ToDecimal(des["Importe_captura"]), Convert.ToDecimal(des["Importe_moneda_nacional"]), id_usuario);

                        //Actualizamos Descuentos Comprobante
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Comprobante
                            using (Comprobante objComprobante = new Comprobante(id_comprobante))
                            {
                                //Si el comprobante existe
                                if (objComprobante.id_comprobante > 0)
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
                    if(resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización de los datos de transferencia del nuevo comprobante
        /// </summary>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTransferenciaGeneracionComprobante(int id_usuario)
        {
            /*
             * Un comprobante marcado como pendiente por transferir al contpaq es marcado 
             * en su bit _bit_transferido_nuevo como 1, y se marca en 0 al ser tranferido
             */

            //instanciando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion("El comprobante ya ha sido transferido previamente.");

            //Validando que el estado del bit actualiza nuevo sea 1
            if (this._bit_transferido_nuevo)


                //Actualizando los datos de transferencia
                resultado = actualizaTransferenciaComprobante(false, 0, this._bit_transferido_cancelado, 0, id_usuario);


            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la actualización a transferido de un comprobante ya cancelado hacia el sistema contable
        /// </summary>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <param name="transaccion">Transacción</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTransferenciaCancelacionComprobante(int id_usuario)
        {
            /*
             * Un comprobante marcado como pendiente por cancelar al contpaq es marcado 
             * en su bit _bit_transferido_cancelado como 1, y se marca en 0 al ser cancelado.
             */

            //instanciando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion("El estatus del comprobante no permite su edición.");

            //Si el comprobante ya está cancelado pero se encuentra pendiente por transferir
            if ((EstatusComprobante)this._id_estatus_comprobante == EstatusComprobante.Cancelado)
            {
                //Si el comprobante no se ha actualizado aún
                if (this._bit_transferido_cancelado)
                    //Actualizando los datos de transferencia
                    resultado = actualizaTransferenciaComprobante(this._bit_transferido_nuevo, false, id_usuario);
                else
                    resultado = new RetornoOperacion("El comprobante ya se ha transferido previamente.");
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de Cencelación
        /// </summary>
        /// <param name="codigo_resultado">Código de resultado de cancelación devuelto</param>
        /// <returns></returns>
        protected static RetornoOperacion obtieneResultadoCancelacion(string codigo_resultado)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Determinando el resultado a partir del código del SAT
            switch (codigo_resultado)
            {

                case "201":
                    resultado = new RetornoOperacion("CFDI cancelado con éxito.", true);
                    break;
                case "202":
                    resultado = new RetornoOperacion("CFDI previamente cancelado.", true);
                    break;
                case "203":
                    resultado = new RetornoOperacion("CFDI no encontrado o no corresponde al emisor.");
                    break;
                case "204":
                    resultado = new RetornoOperacion("CFDI no aplicable para cancelación.");
                    break;
                case "205":
                    resultado = new RetornoOperacion("CFDI no existe.");
                    break;
                case "301":
                    resultado = new RetornoOperacion("XML mal formado.");
                    break;
                case "302":
                    resultado = new RetornoOperacion("Sello mal formado o erroneo.");
                    break;
                case "303":
                    resultado = new RetornoOperacion("Sello no corresponde al emisor.");
                    break;
                case "304":
                    resultado = new RetornoOperacion("Certificado revocado o caduco.");
                    break;
                default:
                    resultado = new RetornoOperacion("Error no identificado.");
                    break;
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Evento de cargar el Subreporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            //Obtenemos valores para realizar filtrado de parametros
            int id_comprobante = int.Parse(e.Parameters["IdComprobanteReferencia"].Values[0].ToString());
            int id_concepto = int.Parse(e.Parameters["IdConceptoReferencia"].Values[0].ToString());

            //Cargamos Tabla
            using (DataTable mitReferencias = SAT_CL.FacturacionElectronica.Concepto.CargaImpresionReferencias(id_comprobante, id_concepto))
            {
                //Asignamos Origen de Datos
                e.DataSources.Add(new ReportDataSource("Referencias", mitReferencias));
            }

        }
        #endregion

    }
}
