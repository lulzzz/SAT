using SAT_CL.FacturacionElectronica;
using SAT_CL.Global;
using SAT_CL.FacturacionElectronica33;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.CXP {
	/// <summary>
	/// Clase encargada de todas las Operaciones relacionadas con las Facturas del Proveedor
	/// </summary>
	public class FacturadoProveedor : Disposable {
		#region Enumeraciones
		/// <summary>
		/// Define los posibles estatus de factura 
		/// </summary>
		public enum EstatusFactura {
			/// <summary>
			/// en revision
			/// </summary>
			EnRevision = 1,
			/// <summary>
			/// aceptada
			/// </summary>
			Aceptada,
			/// <summary>
			/// Rechazada
			/// </summary>
			Rechazada,
			/// <summary>
			/// Aplicada Parcial
			/// </summary>
			AplicadaParcial,
			/// <summary>
			/// liquidada
			/// </summary>
			Liquidada,
			/// <summary>
			/// cancelada
			/// </summary>
			Cancelada,
			/// <summary>
			/// refacturación
			/// </summary>
			Refacturacion
		}
		/// <summary>
		/// Define los posibles Tipos de Comprobantes
		/// </summary>
		public enum TipoComprobante {
			/// <summary>
			/// Comprobante Fiscal Digital por Internet v3.2
			/// </summary>
			CFDI = 1,
			/// <summary>
			/// Código de Barras Bidimencional
			/// </summary>
			CBB,
			/// <summary>
			/// Comprobante Extranjero
			/// </summary>
			CExt,
			/// <summary>
			/// Comprobante Fiscal Digital por Internet v3.3
			/// </summary>
			CFDI3_3,
			/// <summary>
			/// Pagos conComprobante Fiscal Digital por Internet v3.3
			/// </summary>
			CFDI3_3Pago
		}
		/// <summary>
		/// Define los posibles estatus recepción
		/// </summary>
		public enum EstatusRecepion {
			/// <summary>
			/// Factura recibida por área distinta a contabilidad
			/// </summary>
			Recibida = 1,
			/// <summary>
			/// Factura entregada a contabilidad
			/// </summary>
			Entregada,
			/// <summary>
			/// Sin proceso
			/// </summary>
			SinProceso
		}
		/// <summary>
		/// Define los tipos existentes de facturas
		/// </summary>
		public enum TipoFactura {
			/// <summary>
			/// Factura tipo proovedor
			/// </summary>
			FacturaProovedor = 1,
			/// <summary>
			/// Factura Intercompañia
			/// </summary>
			FacturaIntercompania,
			/// <summary>
			/// Factura tipo liquidacion
			/// </summary>
			FacturaLiquidacion,
			/// <summary>
			/// Factura de monto negativo, que puede ser aplicada sobre otra factura del proveedor
			/// </summary>
			FacturaAplicable
		}
		/// <summary>
		/// Define los Estatus de Validación de la Factura
		/// </summary>
		public enum EstatusValidacion {
			/// <summary>
			/// Estatus que indica que el Documento de la Factura esta Validada
			/// </summary>
			ValidacionSintactica = 1,
			/// <summary>
			/// Estatus que indica que la Factura tiene Vigencia del SAT
			/// </summary>
			VigenciaSAT,
			/// <summary>
			/// Estatus que indica que la Factura esta Cancelaa por el SAT
			/// </summary>
			CancelacionSAT,
			/// <summary>
			/// Estatus que indica que la Factura se ingreso de forma Manual
			/// </summary>
			ValidacionManual
		}
		#endregion

		#region Atributos
		/// <summary>
		/// Atributo encargado de almacenar el Nombre del SP
		/// </summary>
		private static string _nom_sp = "cxp.sp_facturado_proveedor_tfp";
		/// <summary>
		/// Atributo encargado de Almacenar 
		/// </summary>
		public int id_factura { get { return this._id_factura; } }
		private int _id_factura;
		/// <summary>
		/// Atributo encargado de Almacenar la Compania Proveedora
		/// </summary>
		public int id_compania_proveedor { get { return this._id_compania_proveedor; } }
		private int _id_compania_proveedor;
		/// <summary>
		/// Atributo encargado de Almacenar la Compania Receptora
		/// </summary>
		public int id_compania_receptor { get { return this._id_compania_receptor; } }
		private int _id_compania_receptor;
		/// <summary>
		/// Atributo encargado de Almacenar el Servicio
		/// </summary>
		public int id_servicio { get { return this._id_servicio; } }
		private int _id_servicio;
		/// <summary>
		/// Atributo encargado de Almacenar la Serie
		/// </summary>
		public string serie { get { return this._serie; } }
		private string _serie;
		/// <summary>
		/// Atributo encargado de Almacenar el Folio
		/// </summary>
		public string folio { get { return this._folio; } }
		private string _folio;
		/// <summary>
		/// Atributo encargado de Almacenar el UUID
		/// </summary>
		public string uuid { get { return this._uuid; } }
		private string _uuid;
		/// <summary>
		/// Atributo encargado de Almacenar la Fecha de Factura
		/// </summary>
		public DateTime fecha_factura { get { return this._fecha_factura; } }
		private DateTime _fecha_factura;
		/// <summary>
		/// Atributo encargado de Almacenar el Tipo de la Factura
		/// </summary>
		public byte id_tipo_factura { get { return this._id_tipo_factura; } }
		private byte _id_tipo_factura;
		/// <summary>
		/// Representa la inicial del tipo de comprobante Ingreso, Egreso, Pago, Nómina, o Traslado
		/// </summary>
		public string tipo_comprobante { get { return this._tipo_comprobante; } }
		private string _tipo_comprobante;
		/// <summary>
		/// Atributo encargado de Almacenar la Naturaleza del CFDI
		/// </summary>
		public byte id_naturaleza_cfdi { get { return this._id_naturaleza_cfdi; } }
		private byte _id_naturaleza_cfdi;
		/// <summary>
		/// Atributo encargado de Almacenar el Estatus de la Factura
		/// </summary>
		public byte id_estatus_factura { get { return this._id_estatus_factura; } }
		private byte _id_estatus_factura;
		/// <summary>
		/// Atributo encargado de Almacenar el Estatus de la Recepción
		/// </summary>
		public byte id_estatus_recepcion { get { return this._id_estatus_recepcion; } }
		private byte _id_estatus_recepcion;
		/// <summary>
		/// Atributo encargado de Almacenar la Recepción
		/// </summary>
		public int id_recepcion { get { return this._id_recepcion; } }
		private int _id_recepcion;
		/// <summary>
		/// Atributo encargado de Almacenar el Segmento de Negocio
		/// </summary>
		public int id_segmento_negocio { get { return this._id_segmento_negocio; } }
		private int _id_segmento_negocio;
		/// <summary>
		/// Atributo encargado de Almacenar el Tipo de Servicio
		/// </summary>
		public int id_tipo_servicio { get { return this._id_tipo_servicio; } }
		private int _id_tipo_servicio;
		/// <summary>
		/// Atributo encargado de Almacenar el Total de la Factura
		/// </summary>
		public decimal total_factura { get { return this._total_factura; } }
		private decimal _total_factura;
		/// <summary>
		/// Atributo encargado de Almacenar el Subtotal de la Factura
		/// </summary>
		public decimal subtotal_factura { get { return this._subtotal_factura; } }
		private decimal _subtotal_factura;
		/// <summary>
		/// Atributo encargado de Almacenar el Descuento de la Factura
		/// </summary>
		public decimal descuento_factura { get { return this._descuento_factura; } }
		private decimal _descuento_factura;
		/// <summary>
		/// Atributo encargado de Almacenar el Importe Trasladado de la Factura
		/// </summary>
		public decimal trasladado_factura { get { return this._trasladado_factura; } }
		private decimal _trasladado_factura;
		/// <summary>
		/// Atributo encargado de Almacenar el Importe Retenido de la Factura
		/// </summary>
		public decimal retenido_factura { get { return this._retenido_factura; } }
		private decimal _retenido_factura;
		/// <summary>
		/// Atributo encargado de Almacenar la Moneda
		/// </summary>
		public string moneda { get { return this._moneda; } }
		private string _moneda;
		/// <summary>
		/// Atributo encargado de Almacenar el Monto del Tipo de Cambio
		/// </summary>
		public decimal monto_tipo_cambio { get { return this._monto_tipo_cambio; } }
		private decimal _monto_tipo_cambio;
		/// <summary>
		/// Atributo encargado de Almacenar la Fecha del Tipo de Cambio
		/// </summary>
		public DateTime fecha_tipo_cambio { get { return this._fecha_tipo_cambio; } }
		private DateTime _fecha_tipo_cambio;
		/// <summary>
		/// Atributo encargado de Almacenar el Total de la Factura en Pesos
		/// </summary>
		public decimal total_factura_pesos { get { return this._total_factura_pesos; } }
		private decimal _total_factura_pesos;
		/// <summary>
		/// Atributo encargado de Almacenar el Subtotal de la Factura en Pesos
		/// </summary>
		public decimal subtotal_pesos { get { return this._subtotal_pesos; } }
		private decimal _subtotal_pesos;
		/// <summary>
		/// Atributo encargado de Almacenar el Descuento de la Factura en Pesos
		/// </summary>
		public decimal descuento_factura_pesos { get { return this._descuento_factura_pesos; } }
		private decimal _descuento_factura_pesos;
		/// <summary>
		/// Atributo encargado de Almacenar el Importe Trasladado de la Factura en Pesos
		/// </summary>
		public decimal trasladado_pesos { get { return this._trasladado_pesos; } }
		private decimal _trasladado_pesos;
		/// <summary>
		/// Atributo encargado de Almacenar el Importe Retenido de la Factura en Pesos
		/// </summary>
		public decimal retenido_pesos { get { return this._retenido_pesos; } }
		private decimal _retenido_pesos;
		/// <summary>
		/// Atributo encargado de Almacenar el Estatus de Transferencia
		/// </summary>
		public bool bit_transferido { get { return this._bit_transferido; } }
		private bool _bit_transferido;
		/// <summary>
		/// Atributo encargado de Almacenar la Fecha del Transferencia
		/// </summary>
		public DateTime fecha_transferido { get { return this._fecha_transferido; } }
		private DateTime _fecha_transferido;
		/// <summary>
		/// Atributo encargado de Almacenar el Saldo de la Factura
		/// </summary>
		public decimal saldo { get { return this._saldo; } }
		private decimal _saldo;
		/// <summary>
		/// Atributo encargado de Almacenar la Condición de Pago
		/// </summary>
		public string condicion_pago { get { return this._condicion_pago; } }
		private string _condicion_pago;
		/// <summary>
		/// Atributo encargado de Almacenar los Dias de Credito
		/// </summary>
		public int dias_credito { get { return this._dias_credito; } }
		private int _dias_credito;
		/// <summary>
		/// Atributo encargado de Almacenar la Causa de la Falta de Pago
		/// </summary>
		public int id_causa_falta_pago { get { return this._id_causa_falta_pago; } }
		private int _id_causa_falta_pago;
		/// <summary>
		/// Atributo encargado de almacenar la Referencia del Resultado de Validación
		/// </summary>
		public byte id_resultado_validacion { get { return this._id_resultado_validacion; } }
		private byte _id_resultado_validacion;
		/// <summary>
		/// Atributo encargado de almacenar el Mensaje del Resultado de Validación
		/// </summary>
		public string resultado_validacion { get { return this._resultado_validacion; } }
		private string _resultado_validacion;
		/// <summary>
		/// Atributo encargado de Almacenar el Estatus Habilitar
		/// </summary>
		public bool habilitar { get { return this._habilitar; } }
		private bool _habilitar;
		#endregion

		#region Constructores
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos por Defecto
		/// </summary>
		public FacturadoProveedor()
		{
			this._id_factura = 0;
			this._id_compania_proveedor = 0;
			this._id_compania_receptor = 0;
			this._id_servicio = 0;
			this._serie = "";
			this._folio = "";
			this._uuid = "";
			this._fecha_factura = DateTime.MinValue;
			this._id_tipo_factura = 0;
			this._tipo_comprobante = "";
			this._id_naturaleza_cfdi = 0;
			this._id_estatus_factura = 0;
			this._id_estatus_recepcion = 0;
			this._id_recepcion = 0;
			this._id_segmento_negocio = 0;
			this._id_tipo_servicio = 0;
			this._total_factura = 0;
			this._subtotal_factura = 0;
			this._descuento_factura = 0;
			this._trasladado_factura = 0;
			this._retenido_factura = 0;
			this._moneda = "";
			this._monto_tipo_cambio = 0;
			this._fecha_tipo_cambio = DateTime.MinValue;
			this._total_factura_pesos = 0;
			this._subtotal_pesos = 0;
			this._descuento_factura_pesos = 0;
			this._trasladado_pesos = 0;
			this._retenido_pesos = 0;
			this._bit_transferido = false;
			this._fecha_transferido = DateTime.MinValue;
			this._saldo = 0;
			this._condicion_pago = "";
			this._dias_credito = 0;
			this._id_causa_falta_pago = 0;
			this._id_resultado_validacion = 0;
			this._resultado_validacion = "";
			this._habilitar = false;
		}
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_factura">Id de factura</param>
		public FacturadoProveedor(int id_factura)
		{
			cargaAtributosInstancia(id_factura);
		}
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos dado un Proveedor, Serie y Folio
		/// </summary>
		/// <param name="id_compania_proveedor">Compania Proveedora</param>
		/// <param name="serie">Serie</param>
		/// <param name="folio">Folio</param>
		public FacturadoProveedor(int id_compania_proveedor, string serie, string folio)
		{   //Invocando Método de Carga
			cargaAtributosInstancia(id_compania_proveedor, serie, folio);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="uuid"></param>
		/// <param name="idCompaniaReceptor"></param>
		public FacturadoProveedor(string uuid, int idCompaniaReceptor)
		{
			cargaAtributosInstancia(uuid, idCompaniaReceptor);
		}
		#endregion

		#region Destructores
		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~FacturadoProveedor()
		{
			Dispose(false);
		}
		#endregion

		#region Métodos Privados
		/// <summary>
		/// Método Privado encargado de Inicializar los Atributo dado un Registro
		/// </summary>
		/// <param name="id_registro">Id de Registro</param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_registro)
		{   //Declarando Objeto de Retorno
			bool result = false;
			object[] param = { 3, id_registro, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, 0, 0, 0, 0, "", 0, true, "", "" };
			//Instanciando Resultado del SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{   //Asignando Valores
						this._id_factura = id_registro;
						this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
						this._id_compania_receptor = Convert.ToInt32(dr["IdCompaniaReceptor"]);
						this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
						this._serie = dr["Serie"].ToString();
						this._folio = dr["Folio"].ToString();
						this._uuid = dr["UUID"].ToString();
						DateTime.TryParse(dr["FechaFactura"].ToString(), out this._fecha_factura);
						this._id_tipo_factura = Convert.ToByte(dr["IdTipoFactura"]);
						this._tipo_comprobante = dr["TipoComprobante"].ToString();
						this._id_naturaleza_cfdi = Convert.ToByte(dr["IdNaturalezaCFDI"]);
						this._id_estatus_factura = Convert.ToByte(dr["IdEstatusFactura"]);
						this._id_estatus_recepcion = Convert.ToByte(dr["IdEstatusRecepcion"]);
						this._id_recepcion = Convert.ToInt32(dr["IdRecepcion"]);
						this._id_segmento_negocio = Convert.ToInt32(dr["IdSegmentoNegocio"]);
						this._id_tipo_servicio = Convert.ToInt32(dr["IdTipoServicio"]);
						this._total_factura = Convert.ToDecimal(dr["TotalFactura"]);
						this._subtotal_factura = Convert.ToDecimal(dr["SubtotalFactura"]);
						this._descuento_factura = Convert.ToDecimal(dr["DescuentoFactura"]);
						this._trasladado_factura = Convert.ToDecimal(dr["TrasladadoFactura"]);
						this._retenido_factura = Convert.ToDecimal(dr["RetenidoFactura"]);
						this._moneda = dr["Moneda"].ToString();
						this._monto_tipo_cambio = Convert.ToDecimal(dr["MontoTipoCambio"]);
						DateTime.TryParse(dr["FechaTipoCambio"].ToString(), out this._fecha_tipo_cambio);
						this._total_factura_pesos = Convert.ToDecimal(dr["TotalFacturaPesos"]);
						this._subtotal_pesos = Convert.ToDecimal(dr["SubtotalPesos"]);
						this._descuento_factura_pesos = Convert.ToDecimal(dr["DescuentoFacturaPesos"]);
						this._trasladado_pesos = Convert.ToDecimal(dr["TrasladadoPesos"]);
						this._retenido_pesos = Convert.ToDecimal(dr["RetenidoPesos"]);
						this._bit_transferido = Convert.ToBoolean(dr["BitTransferido"]);
						DateTime.TryParse(dr["FechaTransferido"].ToString(), out this._fecha_transferido);
						this._saldo = Convert.ToDecimal(dr["Saldo"]);
						this._condicion_pago = dr["CondicionPago"].ToString();
						this._dias_credito = Convert.ToInt32(dr["DiasCredito"]);
						this._id_causa_falta_pago = Convert.ToInt32(dr["IdCausaFaltaPago"]);
						this._id_resultado_validacion = Convert.ToByte(dr["IdResultadoValidacion"]);
						this._resultado_validacion = dr["ResultadoValidacion"].ToString();
						this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
					}
					result = true;
				}
			}
			return result;
		}
		/// <summary>
		/// Método Privado encargado de Inicializar los Atributo dado un Proveedor, Serie y Folio
		/// </summary>
		/// <param name="id_compania_proveedor"></param>
		/// <param name="serie"></param>
		/// <param name="folio"></param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_compania_proveedor, string serie, string folio)
		{   //Declarando Objeto de Retorno
			bool result = false;
			object[] param = { 4, 0, id_compania_proveedor, 0, 0, serie, folio, uuid, null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, 0, 0, 0, 0, "", 0, true, "", "" };
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						this._id_factura = Convert.ToInt32(dr["Id"]);
						this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
						this._id_compania_receptor = Convert.ToInt32(dr["IdCompaniaReceptor"]);
						this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
						this._serie = dr["Serie"].ToString();
						this._folio = dr["Folio"].ToString();
						this._uuid = dr["UUID"].ToString();
						DateTime.TryParse(dr["FechaFactura"].ToString(), out this._fecha_factura);
						this._id_tipo_factura = Convert.ToByte(dr["IdTipoFactura"]);
						this._tipo_comprobante = dr["TipoComprobante"].ToString();
						this._id_naturaleza_cfdi = Convert.ToByte(dr["IdNaturalezaCFDI"]);
						this._id_estatus_factura = Convert.ToByte(dr["IdEstatusFactura"]);
						this._id_estatus_recepcion = Convert.ToByte(dr["IdEstatusRecepcion"]);
						this._id_recepcion = Convert.ToInt32(dr["IdRecepcion"]);
						this._id_segmento_negocio = Convert.ToInt32(dr["IdSegmentoNegocio"]);
						this._id_tipo_servicio = Convert.ToInt32(dr["IdTipoServicio"]);
						this._total_factura = Convert.ToDecimal(dr["TotalFactura"]);
						this._subtotal_factura = Convert.ToDecimal(dr["SubtotalFactura"]);
						this._descuento_factura = Convert.ToDecimal(dr["DescuentoFactura"]);
						this._trasladado_factura = Convert.ToDecimal(dr["TrasladadoFactura"]);
						this._retenido_factura = Convert.ToDecimal(dr["RetenidoFactura"]);
						this._moneda = dr["Moneda"].ToString();
						this._monto_tipo_cambio = Convert.ToDecimal(dr["MontoTipoCambio"]);
						DateTime.TryParse(dr["FechaTipoCambio"].ToString(), out this._fecha_tipo_cambio);
						this._total_factura_pesos = Convert.ToDecimal(dr["TotalFacturaPesos"]);
						this._subtotal_pesos = Convert.ToDecimal(dr["SubtotalPesos"]);
						this._descuento_factura_pesos = Convert.ToDecimal(dr["DesceuntoFacturaPesos"]);
						this._trasladado_pesos = Convert.ToDecimal(dr["TrasladadoPesos"]);
						this._retenido_pesos = Convert.ToDecimal(dr["RetenidoPesos"]);
						this._bit_transferido = Convert.ToBoolean(dr["BitTransferido"]);
						DateTime.TryParse(dr["FechaTransferido"].ToString(), out this._fecha_transferido);
						this._saldo = Convert.ToDecimal(dr["Saldo"]);
						this._condicion_pago = dr["CondicionPago"].ToString();
						this._dias_credito = Convert.ToInt32(dr["DiasCredito"]);
						this._id_causa_falta_pago = Convert.ToInt32(dr["IdCausaFaltaPago"]);
						this._id_resultado_validacion = Convert.ToByte(dr["IdResultadoValidacion"]);
						this._resultado_validacion = dr["ResultadoValidacion"].ToString();
						this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
					}
					result = true;
				}
			}
			return result;
		}
		/// <summary>
		/// Método Privado encargado de Inicializar los Atributo dado un Proveedor, Serie y Folio
		/// </summary>
		/// <param name="id_compania_proveedor"></param>
		/// <param name="serie"></param>
		/// <param name="folio"></param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(string uuid, int idCompaniaReceptor)
		{   //Declarando Objeto de Retorno
			bool result = false;
			object[] param = { 15, 0, 0, idCompaniaReceptor, 0, "", "", uuid, null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, 0, 0, 0, 0, "", 0, true, "", "" };
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						this._id_factura = Convert.ToInt32(dr["Id"]);
						this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
						this._id_compania_receptor = Convert.ToInt32(dr["IdCompaniaReceptor"]);
						this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
						this._serie = dr["Serie"].ToString();
						this._folio = dr["Folio"].ToString();
						this._uuid = dr["UUID"].ToString();
						DateTime.TryParse(dr["FechaFactura"].ToString(), out this._fecha_factura);
						this._id_tipo_factura = Convert.ToByte(dr["IdTipoFactura"]);
						this._tipo_comprobante = dr["TipoComprobante"].ToString();
						this._id_naturaleza_cfdi = Convert.ToByte(dr["IdNaturalezaCFDI"]);
						this._id_estatus_factura = Convert.ToByte(dr["IdEstatusFactura"]);
						this._id_estatus_recepcion = Convert.ToByte(dr["IdEstatusRecepcion"]);
						this._id_recepcion = Convert.ToInt32(dr["IdRecepcion"]);
						this._id_segmento_negocio = Convert.ToInt32(dr["IdSegmentoNegocio"]);
						this._id_tipo_servicio = Convert.ToInt32(dr["IdTipoServicio"]);
						this._total_factura = Convert.ToDecimal(dr["TotalFactura"]);
						this._subtotal_factura = Convert.ToDecimal(dr["SubtotalFactura"]);
						this._descuento_factura = Convert.ToDecimal(dr["DescuentoFactura"]);
						this._trasladado_factura = Convert.ToDecimal(dr["TrasladadoFactura"]);
						this._retenido_factura = Convert.ToDecimal(dr["RetenidoFactura"]);
						this._moneda = dr["Moneda"].ToString();
						this._monto_tipo_cambio = Convert.ToDecimal(dr["MontoTipoCambio"]);
						DateTime.TryParse(dr["FechaTipoCambio"].ToString(), out this._fecha_tipo_cambio);
						this._total_factura_pesos = Convert.ToDecimal(dr["TotalFacturaPesos"]);
						this._subtotal_pesos = Convert.ToDecimal(dr["SubtotalPesos"]);
						this._descuento_factura_pesos = Convert.ToDecimal(dr["DescuentoFacturaPesos"]);
						this._trasladado_pesos = Convert.ToDecimal(dr["TrasladadoPesos"]);
						this._retenido_pesos = Convert.ToDecimal(dr["RetenidoPesos"]);
						this._bit_transferido = Convert.ToBoolean(dr["BitTransferido"]);
						DateTime.TryParse(dr["FechaTransferido"].ToString(), out this._fecha_transferido);
						this._saldo = Convert.ToDecimal(dr["Saldo"]);
						this._condicion_pago = dr["CondicionPago"].ToString();
						this._dias_credito = Convert.ToInt32(dr["DiasCredito"]);
						this._id_causa_falta_pago = Convert.ToInt32(dr["IdCausaFaltaPago"]);
						this._id_resultado_validacion = Convert.ToByte(dr["IdResultadoValidacion"]);
						this._resultado_validacion = dr["ResultadoValidacion"].ToString();
						this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
					}
					result = true;
				}
			}
			return result;
		}
		/// <summary>
		/// Método Privado encargado de Actualizar los valores el BD
		/// </summary>
		/// <param name="id_compania_proveedor">Compania Proveedora (Emisor)</param>
		/// <param name="id_compania_receptor">Compania Receptora</param>
		/// <param name="id_servicio">Servicio</param>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="folio">Folio de la Factura</param>
		/// <param name="uuid">Universally Unique Identifier</param>
		/// <param name="fecha_factura">Fecha de Factura</param>
		/// <param name="id_tipo_factura">Tipo de Factura</param>
		/// <param name="id_estatus_factura">Estatus de Factura</param>
		/// <param name="id_estatus_recepcion">Estatus de Recepción</param>
		/// <param name="id_recepcion">Recepción</param>
		/// <param name="id_segmento_negocio">Segmento de Negocio</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio</param>
		/// <param name="total_factura">Total de la Factura</param>
		/// <param name="subtotal_factura">Subtotal de la Factura</param>
		/// <param name="descuento_factura">Descuento de la Factura</param>
		/// <param name="trasladado_factura">Importe Trasladado de la Factura</param>
		/// <param name="retenido_factura">Importe Retenido de la Factura</param>
		/// <param name="moneda">Moneda</param>
		/// <param name="monto_tipo_cambio">Monto del Tipo de Cambio</param>
		/// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
		/// <param name="total_factura_pesos">Total de la Factura en Pesos</param>
		/// <param name="subtotal_pesos">Subtotal de la Factura en Pesos</param>
		/// <param name="descuento_factura_pesos">Descuento de la Factura en Pesos</param>
		/// <param name="trasladado_pesos">Importe Trasladado de la Factura en Pesos</param>
		/// <param name="retenido_pesos">Importe Retenido de la Factura en Pesos</param>
		/// <param name="bit_transferido">Estatus de Transferencia</param>
		/// <param name="fecha_transferido">Fecha de Transferencia</param>
		/// <param name="saldo">Saldo de la Factura</param>
		/// <param name="condicion_pago">Condición de Pago</param>
		/// <param name="dias_credito">Dias de Credito</param>
		/// <param name="id_causa_falta_pago">Causa de Falta de Pago</param>
		/// <param name="id_resultado_validacion">Id de Resultado de Validación</param>
		/// <param name="resultado_validacion">mensaje del Resultado de Validación</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <param name="habilitar">Estatus Habilitar</param>
		/// <returns></returns>
		private RetornoOperacion actualizaRegistros(int id_compania_proveedor, int id_compania_receptor, int id_servicio, string serie, string folio, string uuid, DateTime fecha_factura, byte id_tipo_factura, string tipo_comprobante, byte id_naturaleza_cfdi, byte id_estatus_factura, byte id_estatus_recepcion, int id_recepcion, int id_segmento_negocio, int id_tipo_servicio, decimal total_factura, decimal subtotal_factura, decimal descuento_factura, decimal trasladado_factura, decimal retenido_factura, string moneda, decimal monto_tipo_cambio, DateTime fecha_tipo_cambio, decimal total_factura_pesos, decimal subtotal_pesos, decimal descuento_factura_pesos, decimal trasladado_pesos, decimal retenido_pesos, bool bit_transferido, DateTime fecha_transferido, decimal saldo, string condicion_pago, int dias_credito, int id_causa_falta_pago, byte id_resultado_validacion, string resultado_validacion, int id_usuario, bool habilitar)
		{
			RetornoOperacion result = new RetornoOperacion();

			object[] param = { 2, this._id_factura, id_compania_proveedor, id_compania_receptor, id_servicio, serie, folio, uuid, Fecha.ConvierteDateTimeObjeto(fecha_factura), id_tipo_factura, tipo_comprobante, id_naturaleza_cfdi, id_estatus_factura, id_estatus_recepcion, id_recepcion, id_segmento_negocio, id_tipo_servicio, total_factura, subtotal_factura, descuento_factura, trasladado_factura, retenido_factura, moneda, monto_tipo_cambio, Fecha.ConvierteDateTimeObjeto(fecha_tipo_cambio), total_factura_pesos, subtotal_pesos, descuento_factura_pesos, trasladado_pesos, retenido_pesos, bit_transferido, Fecha.ConvierteDateTimeObjeto(fecha_transferido), saldo, condicion_pago, dias_credito, id_causa_falta_pago, id_resultado_validacion, resultado_validacion, id_usuario, habilitar, "", "" };

			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			return result;
		}
		#endregion

		#region Métodos Públicos
		/// <summary>
		/// Método Público encargado de Insertar las Facturas del Proveedor
		/// </summary>
		/// <param name="id_compania_proveedor">Compania Proveedora (Emisor)</param>
		/// <param name="id_compania_receptor">Compania Receptora</param>
		/// <param name="id_servicio">Servicio</param>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="folio">Folio de la Factura</param>
		/// <param name="uuid">Universally Unique Identifier</param>
		/// <param name="fecha_factura">Fecha de Factura</param>
		/// <param name="id_tipo_factura">Tipo de Factura</param>
		/// <param name="tipo_comprobante"></param>
		/// <param name="id_naturaleza_cfdi"></param>
		/// <param name="id_estatus_factura">Estatus de Factura</param>
		/// <param name="id_estatus_recepcion">Estatus de Recepción</param>
		/// <param name="id_recepcion">Recepción</param>
		/// <param name="id_segmento_negocio">Segmento de Negocio</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio</param>
		/// <param name="total_factura">Total de la Factura</param>
		/// <param name="subtotal_factura">Subtotal de la Factura</param>
		/// <param name="descuento_factura">Descuento de la Factura</param>
		/// <param name="trasladado_factura">Importe Trasladado de la Factura</param>
		/// <param name="retenido_factura">Importe Retenido de la Factura</param>
		/// <param name="moneda">Moneda</param>
		/// <param name="monto_tipo_cambio">Monto del Tipo de Cambio</param>
		/// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
		/// <param name="total_factura_pesos">Total de la Factura en Pesos</param>
		/// <param name="subtotal_pesos">Subtotal de la Factura en Pesos</param>
		/// <param name="descuento_factura_pesos">Descuento de la Factura en Pesos</param>
		/// <param name="trasladado_pesos">Importe Trasladado de la Factura en Pesos</param>
		/// <param name="retenido_pesos">Importe Retenido de la Factura en Pesos</param>
		/// <param name="bit_transferido">Estatus de Transferencia</param>
		/// <param name="fecha_transferido">Fecha de Transferencia</param>
		/// <param name="saldo">Saldo de la Factura</param>
		/// <param name="condicion_pago">Condición de Pago</param>
		/// <param name="dias_credito">Dias de Credito</param>
		/// <param name="id_causa_falta_pago">Causa de Falta de Pago</param>
		/// <param name="id_resultado_validacion">Id de Resultado de Validación</param>
		/// <param name="resultado_validacion">Mensaje del Resultado de Validación</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaFacturadoProveedor(int id_compania_proveedor, int id_compania_receptor, int id_servicio, string serie, string folio, string uuid, DateTime fecha_factura, byte id_tipo_factura, string tipo_comprobante, byte id_naturaleza_cfdi, byte id_estatus_factura, byte id_estatus_recepcion, int id_recepcion, int id_segmento_negocio, int id_tipo_servicio, decimal total_factura, decimal subtotal_factura, decimal descuento_factura, decimal trasladado_factura, decimal retenido_factura, string moneda, decimal monto_tipo_cambio, DateTime fecha_tipo_cambio, decimal total_factura_pesos, decimal subtotal_pesos, decimal descuento_factura_pesos, decimal trasladado_pesos, decimal retenido_pesos, bool bit_transferido, DateTime fecha_transferido, decimal saldo, string condicion_pago, int dias_credito, int id_causa_falta_pago, byte id_resultado_validacion, string resultado_validacion, int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			object[] param = { 1, 0, id_compania_proveedor, id_compania_receptor, id_servicio, serie, folio, uuid, Fecha.ConvierteDateTimeObjeto(fecha_factura), id_tipo_factura, tipo_comprobante, id_naturaleza_cfdi, id_estatus_factura, id_estatus_recepcion, id_recepcion, id_segmento_negocio, id_tipo_servicio, total_factura, subtotal_factura, descuento_factura, trasladado_factura, retenido_factura, moneda, monto_tipo_cambio, Fecha.ConvierteDateTimeObjeto(fecha_tipo_cambio), total_factura_pesos, subtotal_pesos, descuento_factura_pesos, trasladado_pesos, retenido_pesos, bit_transferido, Fecha.ConvierteDateTimeObjeto(fecha_transferido), saldo, condicion_pago, dias_credito, id_causa_falta_pago, id_resultado_validacion, resultado_validacion, id_usuario, true, "", "" };

			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			return result;
		}
		/// <summary>
		/// Método Público encargado de Insertar las Facturas del Proveedor
		/// </summary>
		/// <param name="id_compania_proveedor">Compania Proveedora (Emisor)</param>
		/// <param name="id_compania_receptor">Compania Receptora</param>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="folio">Folio de la Factura</param>
		/// <param name="uuid">Universally Unique Identifier</param>
		/// <param name="fecha_factura">Fecha de Factura</param>
		/// <param name="id_tipo_factura">Tipo de Factura</param>
		/// <param name="tipo_comprobante"></param>
		/// <param name="id_naturaleza_cfdi"></param>
		/// <param name="id_estatus_factura">Estatus de Factura</param>
		/// <param name="descuento_factura">Descuento de Factura</param>
		/// <param name="id_estatus_recepcion">Estatus de Recepción</param>
		/// <param name="moneda">Moneda</param>
		/// <param name="monto_tipo_cambio">Monto del Tipo de Cambio</param>
		/// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
		/// <param name="descuento_factura_pesos">Descuento de Factura en Pesos</param>
		/// <param name="condicion_pago">Condición de Pago</param>
		/// <param name="dias_credito">Dias de Credito</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaFacturadoProveedor(int id_compania_proveedor, int id_compania_receptor, string serie, string folio, string uuid, DateTime fecha_factura, byte id_tipo_factura, string tipo_comprobante, byte id_naturaleza_cfdi, byte id_estatus_factura, decimal descuento_factura, byte id_estatus_recepcion, string moneda, decimal monto_tipo_cambio, DateTime fecha_tipo_cambio, decimal descuento_factura_pesos, string condicion_pago, int dias_credito, int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			object[] param = { 1, 0, id_compania_proveedor, id_compania_receptor, 0, serie, folio, uuid, Fecha.ConvierteDateTimeObjeto(fecha_factura), id_tipo_factura, tipo_comprobante, id_naturaleza_cfdi, id_estatus_factura, id_estatus_recepcion, 0, 0, 0, 0, 0, descuento_factura, 0, 0, moneda, monto_tipo_cambio, Fecha.ConvierteDateTimeObjeto(fecha_tipo_cambio), 0, 0, descuento_factura_pesos, 0, 0, false, null, 0, condicion_pago, dias_credito, 0, (byte)EstatusValidacion.ValidacionManual, "", id_usuario, true, "", "" };

			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			return result;
		}
		/// <summary>
		/// Método Público encargado de Editar las Facturas del Proveedor
		/// </summary>
		/// <param name="id_compania_proveedor">Compania Proveedora (Emisor)</param>
		/// <param name="id_compania_receptor">Compania Receptora</param>
		/// <param name="id_servicio">Servicio</param>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="folio">Folio de la Factura</param>
		/// <param name="uuid">Universally Unique Identifier</param>
		/// <param name="fecha_factura">Fecha de Factura</param>
		/// <param name="id_tipo_factura">Tipo de Factura</param>
		/// <param name="tipo_comprobante"></param>
		/// <param name="id_naturaleza_cfdi"></param>
		/// <param name="id_estatus_factura">Estatus de Factura</param>
		/// <param name="id_estatus_recepcion">Estatus de Recepción</param>
		/// <param name="id_recepcion">Recepción</param>
		/// <param name="id_segmento_negocio">Segmento de Negocio</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio</param>
		/// <param name="total_factura">Total de la Factura</param>
		/// <param name="subtotal_factura">Subtotal de la Factura</param>
		/// <param name="descuento_factura">Descuento de la Factura</param>
		/// <param name="trasladado_factura">Importe Trasladado de la Factura</param>
		/// <param name="retenido_factura">Importe Retenido de la Factura</param>
		/// <param name="moneda">Moneda</param>
		/// <param name="monto_tipo_cambio">Monto del Tipo de Cambio</param>
		/// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
		/// <param name="total_factura_pesos">Total de la Factura en Pesos</param>
		/// <param name="subtotal_pesos">Subtotal de la Factura en Pesos</param>
		/// <param name="descuento_factura_pesos">Descuento de la Factura en Pesos</param>
		/// <param name="trasladado_pesos">Importe Trasladado de la Factura en Pesos</param>
		/// <param name="retenido_pesos">Importe Retenido de la Factura en Pesos</param>
		/// <param name="bit_transferido">Estatus de Transferencia</param>
		/// <param name="fecha_transferido">Fecha de Transferencia</param>
		/// <param name="saldo">Saldo de la Factura</param>
		/// <param name="condicion_pago">Condición de Pago</param>
		/// <param name="dias_credito">Dias de Credito</param>
		/// <param name="id_causa_falta_pago">Causa de Falta de Pago</param>
		/// <param name="id_resultado_validacion">Id de Resultado de Validación</param>
		/// <param name="resultado_validacion">Mensaje del Resultado de Validación</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditaFacturadoProveedor(int id_compania_proveedor, int id_compania_receptor, int id_servicio, string serie, string folio, string uuid, DateTime fecha_factura, byte id_tipo_factura, string tipo_comprobante, byte id_naturaleza_cfdi, byte id_estatus_factura, byte id_estatus_recepcion, int id_recepcion, int id_segmento_negocio, int id_tipo_servicio, decimal total_factura, decimal subtotal_factura, decimal descuento_factura, decimal trasladado_factura, decimal retenido_factura, string moneda, decimal monto_tipo_cambio, DateTime fecha_tipo_cambio, decimal total_factura_pesos, decimal subtotal_pesos, decimal descuento_factura_pesos, decimal trasladado_pesos, decimal retenido_pesos, bool bit_transferido, DateTime fecha_transferido, decimal saldo, string condicion_pago, int dias_credito, int id_causa_falta_pago, byte id_resultado_validacion, string resultado_validacion, int id_usuario)
		{
			return this.actualizaRegistros(id_compania_proveedor, id_compania_receptor, id_servicio, serie, folio, uuid,fecha_factura, id_tipo_factura, tipo_comprobante, id_naturaleza_cfdi, id_estatus_factura,id_estatus_recepcion, id_recepcion, id_segmento_negocio, id_tipo_servicio, total_factura,subtotal_factura, descuento_factura, trasladado_factura, retenido_factura, moneda, monto_tipo_cambio,fecha_tipo_cambio, total_factura_pesos, subtotal_pesos, descuento_factura_pesos, trasladado_pesos,retenido_pesos, bit_transferido, fecha_transferido, saldo, condicion_pago, dias_credito,id_causa_falta_pago, id_resultado_validacion, resultado_validacion, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método Público encargado de Editar las Facturas del Proveedor
		/// </summary>
		/// <param name="id_estatus_factura">Estatus de Factura</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditaFacturadoProveedor(byte id_estatus_factura, int id_usuario)
		{
			return this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid,this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, id_estatus_factura,this._id_estatus_recepcion, this._id_recepcion, this._id_segmento_negocio, this._id_tipo_servicio, this._total_factura,this._subtotal_factura, this._descuento_factura, this._trasladado_factura, this._retenido_factura, this._moneda, this._monto_tipo_cambio,this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._descuento_factura_pesos, this._trasladado_pesos,this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo, this._condicion_pago, this._dias_credito,this._id_causa_falta_pago, this._id_resultado_validacion, this._resultado_validacion, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método Público encargado de Editar las Facturas del Proveedor
		/// </summary>
		/// <param name="id_compania_proveedor">Compania Proveedora (Emisor)</param>
		/// <param name="id_compania_receptor">Compania Receptora</param>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="folio">Folio de la Factura</param>
		/// <param name="uuid">Universally Unique Identifier</param>
		/// <param name="fecha_factura">Fecha de Factura</param>
		/// <param name="id_tipo_factura">Tipo de Factura</param>
		/// <param name="tipo_comprobante"></param>
		/// <param name="id_naturaleza_cfdi"></param>
		/// <param name="id_estatus_factura">Estatus de Factura</param>
		/// <param name="descuento_factura">Descuento de Factura</param>
		/// <param name="id_estatus_recepcion">Estatus de Recepción</param>
		/// <param name="id_segmento_negocio">Segmento de Negocio</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio</param>
		/// <param name="moneda">Moneda</param>
		/// <param name="monto_tipo_cambio">Monto del Tipo de Cambio</param>
		/// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
		/// <param name="descuento_factura_pesos">Descuento de Factura en Pesos</param>
		/// <param name="condicion_pago">Condición de Pago</param>
		/// <param name="dias_credito">Dias de Credito</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditaFacturadoProveedor(int id_compania_proveedor, int id_compania_receptor, string serie, string folio, string uuid,DateTime fecha_factura, byte id_tipo_factura, string tipo_comprobante, byte id_naturaleza_cfdi, byte id_estatus_factura, decimal descuento_factura, byte id_estatus_recepcion,int id_segmento_negocio, int id_tipo_servicio, string moneda, decimal monto_tipo_cambio,DateTime fecha_tipo_cambio, decimal descuento_factura_pesos, string condicion_pago, int dias_credito, int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			if (this._id_tipo_factura != (byte)TipoComprobante.CFDI)
			{
				if (!(this._bit_transferido))
					result = this.actualizaRegistros(id_compania_proveedor, id_compania_receptor, 0, serie, folio, uuid, fecha_factura, id_tipo_factura, tipo_comprobante, id_naturaleza_cfdi, id_estatus_factura, id_estatus_recepcion, id_segmento_negocio, id_tipo_servicio, 0, this._total_factura, this._subtotal_factura, descuento_factura, this.trasladado_factura, this._retenido_factura, moneda, monto_tipo_cambio, fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, descuento_factura_pesos, this._trasladado_pesos, this.retenido_pesos, false, fecha_tipo_cambio, this._saldo, condicion_pago, dias_credito, 0, (byte)EstatusValidacion.ValidacionManual, "", id_usuario, this._habilitar);
				else
					result = new RetornoOperacion("La Factura esta Transferida. Imposible su Edición");
			}
			return result;
		}
		/// <summary>
		/// Método Público que Actualiza los Valores Totales de la Factura
		/// </summary>
		/// <param name="total_factura">Total de la Factura</param>
		/// <param name="subtotal_factura">SubTotal de la Factura</param>
		/// <param name="trasladado_factura">Importe Trasladado de la Factura</param>
		/// <param name="retenido_factura">Importe Retenido de la Factura</param>
		/// <param name="total_factura_pesos">Total de la Factura en Pesos</param>
		/// <param name="subtotal_pesos">SubTotal de la Factura en Pesos</param>
		/// <param name="trasladado_pesos">Importe Trasladado de la Factura en Pesos</param>
		/// <param name="retenido_pesos">Importe Retenido de la Factura en Pesos</param>
		/// <param name="saldo">Saldo de la Factura</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaTotalesFacturadoProveedor(decimal total_factura, decimal subtotal_factura, decimal trasladado_factura,decimal retenido_factura, decimal total_factura_pesos, decimal subtotal_pesos,decimal trasladado_pesos, decimal retenido_pesos, decimal saldo, int id_usuario)
		{
			return this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid, this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, this._id_estatus_factura, (byte)EstatusRecepion.Entregada, this._id_recepcion, this._id_segmento_negocio, this._id_tipo_servicio, total_factura, subtotal_factura, this._descuento_factura, trasladado_factura, retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, saldo, this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion, this._resultado_validacion, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método Público encargado de Actualizar el Estatus de Recepción de la Factura
		/// </summary>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion EntregaFacturaProveedor(int id_usuario)
		{
			return this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid,this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, this._id_estatus_factura, (byte)EstatusRecepion.Entregada, this._id_recepcion, this._id_segmento_negocio,this._id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura,this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos,this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo,this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion,this._resultado_validacion, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método Público encargado de Actualizar el Estatus de la Factura, el Tipo de Servicio y el Segmento de Negocio
		/// </summary>
		/// <param name="estatus">Estatus de la Factura</param>
		/// <param name="id_segmento_negocio">Segmento de Negocio</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio</param>
		/// <param name="id_usuario">Id de usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditaFacturadoProveedor(EstatusFactura estatus, int id_segmento_negocio, int id_tipo_servicio, int id_usuario)
		{
			return this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid,this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, (byte)estatus, this._id_estatus_recepcion, this._id_recepcion, id_segmento_negocio,id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura,this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos,this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo,this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion,this._resultado_validacion, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Metodo encargado de Actualizar el Estatus de la Factura
		/// </summary>
		/// <param name="estatusFactura">Estatus de la Factura</param>
		/// <param name="id_usuario">Usuario que Actualzia el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaEstatusFacturadoProveedor(EstatusFactura estatusFactura, int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();
			//Si se va a Aceptar la Factura
			if (estatusFactura != EstatusFactura.Aceptada)
				result = this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid,this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, (byte)estatusFactura, this._id_estatus_recepcion, this._id_recepcion, this._id_segmento_negocio,this._id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura,this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos,this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo,this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion,this._resultado_validacion, id_usuario, this._habilitar);
			else
				result = AceptaFacturaProveedor(id_usuario);
			return result;
		}
		/// <summary>
		/// Metodo encargado de Actualizar el Estatus de la Recepción de la Factura
		/// </summary>
		/// <param name="estatusRecepcion">Estatus de la Recepción</param>
		/// <param name="id_recepcion">Referencia de la Recepción</param>
		/// <param name="id_usuario">Usuario que Actualzia el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaEstatusRecepcionFacturadoProveedor(EstatusRecepion estatusRecepcion, int id_recepcion, int id_usuario)
		{
			return this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid, this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, this._id_estatus_factura, (byte)estatusRecepcion, id_recepcion, this._id_segmento_negocio, this._id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura, this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo, this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion, this._resultado_validacion, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método Público encargado de Deshabilitar la Factura del Proveedor
		/// </summary>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaFacturadoProveedor(int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				result = this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid,this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, this._id_estatus_factura, this._id_estatus_recepcion, this._id_recepcion, this._id_segmento_negocio,this._id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura,this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos,this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo,this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion,this._resultado_validacion, id_usuario, false);

				if (result.OperacionExitosa)
				{
					//Obtener Conceptos
					using (DataTable dtConceptos = FacturadoProveedorConcepto.ObtieneConceptosFactura(this._id_factura))
					{
						if (Validacion.ValidaOrigenDatos(dtConceptos))
						{
							foreach (DataRow dr in dtConceptos.Rows)
							{
								using (FacturadoProveedorConcepto fpc = new FacturadoProveedorConcepto(Convert.ToInt32(dr["Id"])))
								{
									if (fpc.habilitar)
									{
										result = fpc.DeshabilitaFacturaProveedorConcepto(id_usuario);
										if (!result.OperacionExitosa)
											break;
									}
								}
							}
							if (result.OperacionExitosa)
								result = new RetornoOperacion(_id_factura);
						}
						else
							result = new RetornoOperacion(_id_factura);

						if (result.OperacionExitosa)
						{
							using (DataTable dtAnticipos = FacturadoProveedorRelacion.ObtieneFacturasRelacionAnticipos(this._id_factura))
							{
								if (Validacion.ValidaOrigenDatos(dtAnticipos))
								{
									foreach (DataRow dr in dtAnticipos.Rows)
									{
										using (FacturadoProveedorRelacion fpr = new FacturadoProveedorRelacion(Convert.ToInt32(dr["Id"])))
										{
											if (fpr.habilitar)
												result = fpr.DeshabilitarFacturaPoveedorRelacion(id_usuario);
											else
												result = new RetornoOperacion("No se puede Acceder a la Relación de la Factura");

											if (result.OperacionExitosa)
												//Terminando Ciclo
												break;
										}
									}
								}
								else
									result = new RetornoOperacion(_id_factura);
								
								//Si se realizaron las Operaciones
								if (result.OperacionExitosa)
								{
									//Instanciando Recepción
									result = new RetornoOperacion(_id_factura);
									//Completando Transacción
									trans.Complete();
								}
							}
						}
					}
				}
			}

			return result;
		}
		/// <summary>
		/// Método Público encargado de Actualizar la Factura del Proveedor
		/// </summary>
		/// <returns></returns>
		public bool ActualizarFacturadoProveedor()
		{   //Invocando Método de Carga
			return this.cargaAtributosInstancia(this._id_factura);
		}
		/// <summary>
		/// Metodo encargado de cargar el conjunto de facturas recibidas ligadas a un id de recepción
		/// </summary>
		/// <param name="id_recepcion">Id de Recepción de facturas</param>
		/// <returns></returns>
		public static DataTable CargaFacturasRecepcion(int id_recepcion)
		{
			DataTable mit = null;

			object[] param = { 5, 0, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, id_recepcion, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };

			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))			
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					mit = ds.Tables["Table"];			

			return mit;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EstatusFactura ObtieneEstatusFacturaAplicada()
		{
			EstatusFactura estatus = (EstatusFactura)this._id_estatus_factura;

			object[] param = { 7, this._id_factura, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };
			
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existen Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorriendo Filas
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Si los Pagos cubren la Factura
						if ((this.total_factura - Convert.ToDecimal(dr["MontoAplicado"])) == 0)
							estatus = EstatusFactura.Liquidada;
						//Si los Pagos no cubren la Factura
						else if (Convert.ToDecimal(dr["MontoAplicado"]) == 0)
							estatus = (EstatusFactura)this._id_estatus_factura;
						else
							estatus = EstatusFactura.AplicadaParcial;
					}
				}
			}

			return estatus;
		}
		/// <summary>
		/// Metodo Encargado Obtener el Reporte de las Facturas que estan Recibidas
		/// </summary>
		/// <param name="id_compania_proveedor">Compania que Provee al Factura</param>
		/// <param name="id_compania_receptor">Compania que recibe la Factura</param>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="folio">Folio de la Factura</param>
		/// <param name="uuid">UUID de la Factura</param>
		/// <param name="fecha_recepcion">Fecha de Recepción de la Factura</param>
		/// <returns></returns>
		public static DataTable ObtieneReporteFacturasRecepcion(int id_compania_proveedor, int id_compania_receptor, string serie, string folio, string uuid, DateTime fecha_recepcion)
		{
			DataTable dtFacturas = null;
			
			string fecha_inicio = "", fecha_fin = "";
			
			if (fecha_recepcion != DateTime.MinValue)
			{
				fecha_inicio = Convert.ToDateTime(fecha_recepcion.ToString("yyyy-MM-dd") + " 00:00").ToString("yyyy-MM-dd HH:mm");
				fecha_fin = Convert.ToDateTime(fecha_recepcion.ToString("yyyy-MM-dd") + " 00:00").ToString("yyyy-MM-dd HH:mm");
			}

			object[] param = { 6, 0, id_compania_proveedor, id_compania_receptor, 0, serie, folio, uuid, null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, 0, 0, 0, 0, "", 0, true, fecha_inicio, fecha_fin };

			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					dtFacturas = ds.Tables["Table"];
			}

			return dtFacturas;
		}
		/// <summary>
		/// Método que permite realizar la carga de datos (descripcion de recepcion facturas proveedor)
		/// </summary>
		/// <param name="id_recepcion">Identificador de una recepción de facturas proveedor</param>
		/// <returns></returns>
		public static DataTable CargaAcuseReciboFactura(int id_recepcion)
		{
			DataTable dtAcuseRecibo = null;

			object[] param = { 8, 0, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, id_recepcion, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					dtAcuseRecibo = DS.Tables["Table"];

			return dtAcuseRecibo;
		}
		/// <summary>
		/// Método encargado de Obtener las Facturas de Proveedor por Servicio
		/// </summary>
		/// <param name="id_servicio">Servicio</param>
		/// <returns></returns>
		public static DataTable ObtieneFacturasPorServicio(int id_servicio)
		{
			DataTable dtFacturasLigadas = null;

			object[] param ={ 9, 0, 0, 0, id_servicio, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "",0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					dtFacturasLigadas = DS.Tables["Table"];

			return dtFacturasLigadas;
		}
		/// <summary>
		/// Método encargado de Obtener el Monto Pendiente de Aplicación
		/// </summary>
		/// <param name="id_factura">Factura</param>
		/// <returns></returns>
		public static decimal ObtieneMontoPendienteAplicacion(int id_factura)
		{
			decimal result = 0;

			object[] param = { 10, id_factura, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };

			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					foreach (DataRow dr in ds.Tables["Table"].Rows)
						result = Convert.ToDecimal(dr["MontoPendiente"]);

			return result;
		}
		/// <summary>
		/// Método encargado de Validar si el Deposito tiene Facturas de Anticipos
		/// </summary>
		/// <param name="id_deposito">Deposito</param>
		/// <returns></returns>
		public static bool ValidaFacturasDeposito(int id_deposito)
		{
			bool result = false;

			object[] param = { 11, id_deposito, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };

			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Asignando Monto Pendiente Obtenido
						result = Convert.ToBoolean(dr["Validacion"]);
						break;
					}
				}
			}

			return result;
		}
		/// <summary>
		/// Método encargado de Obtener las Facturas de Proveedor por Deposito
		/// </summary>
		/// <param name="id_deposito">Deposito</param>
		/// <returns></returns>
		public static DataTable ObtieneFacturasPorDeposito(int id_deposito)
		{
			DataTable dtFacturasLigadas = null;
			
			object[] param = { 12, 0, 0, 0, id_deposito, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					dtFacturasLigadas = DS.Tables["Table"];

			return dtFacturasLigadas;
		}
		/// <summary>
		/// Método encargado de Actualizar el Servicio de la Factura
		/// </summary>
		/// <param name="id_servicio">Servicio</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaServicioFactura(int id_servicio, int id_usuario)
		{
			return this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, id_servicio, this._serie, this._folio, this._uuid, this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, this._id_estatus_factura, this._id_estatus_recepcion, this._id_recepcion, this._id_segmento_negocio, this._id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura, this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo, this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion, this._resultado_validacion, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método encargado de Aceptar la Factura del Proveedor
		/// </summary>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion AceptaFacturaProveedor(int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				result = this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid, this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, (byte)EstatusFactura.Aceptada, this._id_estatus_recepcion, this._id_recepcion, this._id_segmento_negocio, this._id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura, this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo, this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion, this._resultado_validacion, id_usuario, this._habilitar);				
				if (result.OperacionExitosa)
				{
					if (this._id_recepcion == 0)
					{
						result = Recepcion.InsertaRecepcion(this._id_compania_proveedor, this._id_compania_receptor,Fecha.ObtieneFechaEstandarMexicoCentro(), "", 2, id_usuario);
						if (result.OperacionExitosa)
						{
							ActualizarFacturadoProveedor();
							int idRecepcion = result.IdRegistro;
							result = ActualizaEstatusRecepcionFacturadoProveedor(EstatusRecepion.Recibida, idRecepcion, id_usuario);
						}
					}
					
					if (result.OperacionExitosa)
					{
						result = new RetornoOperacion(this._id_factura);
						trans.Complete();
					}
				}
			}

			return result;
		}
		/// <summary>
		/// Método encargado de Aceptar la Factura del Proveedor especificando su Segmento de Negocio y su Tipo de Servicio
		/// </summary>
		/// <param name="id_segmento_negocio">Segmento de Negocio</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion AceptaFacturaProveedor(int id_segmento_negocio, int id_tipo_servicio, int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				result = this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._id_servicio, this._serie, this._folio, this._uuid, this._fecha_factura, this._id_tipo_factura, this._tipo_comprobante, this._id_naturaleza_cfdi, (byte)EstatusFactura.Aceptada, this._id_estatus_recepcion, this._id_recepcion, id_segmento_negocio, id_tipo_servicio, this._total_factura, this._subtotal_factura, this._descuento_factura, this._trasladado_factura, this._retenido_factura, moneda, this._monto_tipo_cambio, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._descuento_factura_pesos, this._trasladado_pesos, this._retenido_pesos, this._bit_transferido, this._fecha_transferido, this._saldo, this._condicion_pago, this._dias_credito, this._id_causa_falta_pago, this._id_resultado_validacion, this._resultado_validacion, id_usuario, this._habilitar);
				if (result.OperacionExitosa)
				{
					if (this._id_recepcion == 0)
					{
						result = Recepcion.InsertaRecepcion(this._id_compania_proveedor, this._id_compania_receptor, Fecha.ObtieneFechaEstandarMexicoCentro(), "", 2, id_usuario);
						if (result.OperacionExitosa)
						{
							ActualizarFacturadoProveedor();
							int idRecepcion = result.IdRegistro;
							result = ActualizaEstatusRecepcionFacturadoProveedor(EstatusRecepion.Recibida, idRecepcion, id_usuario);
						}
					}
					if (result.OperacionExitosa)
					{
						result = new RetornoOperacion(this._id_factura);
						trans.Complete();
					}
				}
			}

			return result;
		}
		/// <summary>
		/// Método encargado de Obtener las Facturas Ligadas por una Entidad y un Registro
		/// </summary>
		/// <param name="id_tabla">Entidad por Consultar (25.- Deposito, 82.- Liquidación)</param>
		/// <param name="id_registro">Registro por Consultar</param>
		/// <returns></returns>
		public static DataTable ObtieneFacturasEntidad(int id_tabla, int id_registro)
		{
			DataTable dtFacturasLigadas = null;

			object[] param ={ 13, 0, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0,0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, id_tabla.ToString(), id_registro.ToString() };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					dtFacturasLigadas = DS.Tables["Table"];

			return dtFacturasLigadas;
		}
		/// <summary>
		/// Método encargado de obtener las aplicaciónes y/o relaciones de las Facturas
		/// </summary>
		/// <param name="id_factura_proveedor">Factura Proveedor</param>
		/// <returns></returns>
		public static DataTable ObtieneAplicacionesRelacionFacturasProveedor(int id_factura_proveedor)
		{
			DataTable dtFichasFacturas = null;

			object[] param = { 14, id_factura_proveedor, 0, 0, 0, "", "", "", null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "",0, null, 0, 0, 0, 0, 0, false, null, 0, "", 0, 0, 0, "", 0, true, "", "" };

			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					dtFichasFacturas = ds.Tables["Table"];

			return dtFichasFacturas;
		}
		/// <summary>
		/// Método encargado de Refacturar una Factura de Proveedor
		/// </summary>
		/// <param name="id_factura_anterior">Factura Anterior</param>
		/// <param name="id_factura_nueva">Factura Sustituyente</param>
		/// <param name="id_usuario">Usuario que actualiza el registro</param>
		/// <returns></returns>
		public static RetornoOperacion RefacturacionCXP(int id_factura_anterior, int id_factura_nueva, int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				using (FacturadoProveedor fac_ant = new FacturadoProveedor(id_factura_anterior),fac_nva = new FacturadoProveedor(id_factura_nueva))
				{
					//Validando que exista la Factura Anterior
					if (fac_ant.habilitar && ((EstatusFactura)fac_ant._id_estatus_factura == EstatusFactura.Aceptada ||(EstatusFactura)fac_ant._id_estatus_factura == EstatusFactura.EnRevision))
					{
						result = fac_ant.ActualizaEstatusFacturadoProveedor(EstatusFactura.Refacturacion, id_usuario);
						if (result.OperacionExitosa)
						{
							//Validando que exista la Nueva factura
							if (fac_nva.habilitar && ((EstatusFactura)fac_nva._id_estatus_factura == EstatusFactura.EnRevision))
							{
								if (fac_ant.id_compania_proveedor == fac_nva.id_compania_proveedor)
								{
									if (fac_ant.id_compania_receptor == fac_nva.id_compania_receptor)
									{
										result = fac_nva.ActualizaServicioFactura(fac_ant.id_servicio, id_usuario);
										if (result.OperacionExitosa)
										{
											using (DataTable dtReferencias = Referencia.CargaReferencias(fac_ant.id_factura, 72, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 72, "Deposito", 0, "Anticipos Proveedor")))
											{
												if (Validacion.ValidaOrigenDatos(dtReferencias))
												{
													foreach (DataRow dr in dtReferencias.Rows)
													{
														//Insertando Referencia de Deposito a la Nueva Factura
														result = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(fac_nva.id_factura, 51,Convert.ToInt32(dr["Valor"]), id_usuario);

														if (!result.OperacionExitosa)
															break;
													}
												}
												else
													result = new RetornoOperacion(fac_nva.id_factura);
											}
											//Obtiene Relaciones Activas
											using (DataTable dtRelaciones = FacturadoProveedorRelacion.ObtieneRelacionesFactura(fac_ant.id_factura))
											{
												if (Validacion.ValidaOrigenDatos(dtRelaciones))
												{
													foreach (DataRow dr in dtRelaciones.Rows)
													{
														//Instanciando Relación de Facturas
														using (FacturadoProveedorRelacion fpr = new FacturadoProveedorRelacion(Convert.ToInt32(dr["Id"])))
														{
															if (fpr.habilitar)
															{
																result = fpr.EditarFacturadoProveedorRelacion(fac_nva.id_factura, fpr.id_tabla, fpr.id_registro, id_usuario);
																if (!result.OperacionExitosa)
																	break;
															}
														}
													}
												}
												else
													result = new RetornoOperacion(fac_nva.id_factura);
											}
										}
									}
									else
										result = new RetornoOperacion("Los Receptores de la Factura no coinciden");
								}
								else
									result = new RetornoOperacion("Los Proveedores de la Factura no coinciden");
							}
							else
							{
								//Validando Excepción
								switch ((EstatusFactura)fac_nva.id_estatus_factura)
								{
									case EstatusFactura.AplicadaParcial:
									case EstatusFactura.Liquidada:
									{
										result = new RetornoOperacion("La Factura tiene Pagos Aplicados");
										break;
									}
									case EstatusFactura.Refacturacion:
									{
										result = new RetornoOperacion("La Factura ya fue Refacturada");
										break;
									}
								}
							}
						}

					}
					else
					{
						//Validando Excepción
						switch ((EstatusFactura)fac_ant.id_estatus_factura)
						{
							case EstatusFactura.AplicadaParcial:
							case EstatusFactura.Liquidada:
							{
								result = new RetornoOperacion("La Factura tiene Pagos Aplicados");
								break;
							}
							case EstatusFactura.Refacturacion:
							{
								result = new RetornoOperacion("La Factura ya fue Refacturada");
								break;
							}
						}
					}

					if (result.OperacionExitosa)
					{
						result = new RetornoOperacion(fac_nva.id_factura);
						transaccion.Complete();
					}
				}
			}

			return result;
		}

		#region Importación CFDI 3.2
		/// <summary>
		/// Método encargado de Importar el CFDI en la versión 3.2
		/// </summary>
		/// <param name="id_compania">Compania Receptora</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio del Proveedor</param>
		/// <param name="nombre_archivo">Nombre del Archivo XML</param>
		/// <param name="factura_xml">Archivo XML a Importar</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion ImportaComprobanteVersion32(int id_compania, int id_tipo_servicio, string nombre_archivo, XDocument factura_xml, int id_usuario)
		{
			//Recuperando Namespace del Timbre Fiscal
			XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");
			XNamespace ns = factura_xml.Root.GetNamespaceOfPrefix("cfdi");

			RetornoOperacion result = new RetornoOperacion();
			int idFactura = 0;
			string versionCFDI = factura_xml.Root.Attribute("version") != null ? factura_xml.Root.Attribute("version").Value : "";

			//Validando Versión del CFDI
			if (versionCFDI.Equals("3.2"))
			{
				result = validaFacturaXML32(id_compania, id_tipo_servicio, factura_xml, id_usuario);

				if (result.OperacionExitosa)
				{
					using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;
						obtieneCantidades32(factura_xml, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

						using (CompaniaEmisorReceptor emisor = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(factura_xml.Root.Element(ns + "Emisor").Attribute("rfc").Value, id_compania))
						using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(id_compania))
						{
							if (emisor.habilitar)
							{
								if (receptor.habilitar)
								{
									decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

									/** Retenciones **/
									//Validando que no exista el Nodo
									if (factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos") == null)
									{
										//Validando que existan Retenciones
										if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones") != null)
										{
											//Obteniendo Retenciones
											IEnumerable<XElement> retenciones = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").Elements();
											//Validando que existan Nodos
											if (retenciones != null)
											{
												//Recorriendo Retenciones
												foreach (XElement retencion in retenciones)
													totalImpRetenidos += Convert.ToDecimal(retencion.Attribute("importe").Value);
											}
										}
									}
									else
										//Asignando Total de Impuestos
										totalImpRetenidos = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos").Value);

									/** Traslados **/
									//Validando que no exista el Nodo
									if (factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados") == null)
									{
										//Validando que existan Traslados
										if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados") != null)
										{
											//Obteniendo Retenciones
											IEnumerable<XElement> traslados = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados").Elements();
											//Validando que existan Nodos
											if (traslados != null)
											{
												foreach (XElement traslado in traslados)
													//Sumando Impuestos Trasladados
													totalImpRetenidos += Convert.ToDecimal(traslado.Attribute("importe").Value);
											}
										}
									}
									else
										//Asignando Total de Impuestos
										totalImpTrasladados = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados").Value);

									//Insertando factura
									result = InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor,receptor.id_compania_emisor_receptor,0,factura_xml.Root.Attribute("serie") == null ? "" : factura_xml.Root.Attribute("serie").Value,factura_xml.Root.Attribute("folio") == null ? "" : factura_xml.Root.Attribute("folio").Value,factura_xml.Root.Element(ns + "Complemento").Element(tfd + "TimbreFiscalDigital").Attribute("UUID").Value,Convert.ToDateTime(factura_xml.Root.Attribute("fecha").Value),(byte)TipoComprobante.CFDI, "", 0,(byte)EstatusFactura.EnRevision,(byte)EstatusRecepion.Recibida,0, 0, 0,Convert.ToDecimal(factura_xml.Root.Attribute("total").Value),Convert.ToDecimal(factura_xml.Root.Attribute("subTotal").Value),factura_xml.Root.Attribute("descuentos") == null ? 0 : Convert.ToDecimal(factura_xml.Root.Attribute("descuentos").Value),totalImpTrasladados, totalImpRetenidos,factura_xml.Root.Attribute("Moneda") == null ? "" : factura_xml.Root.Attribute("Moneda").Value,monto_tc, Convert.ToDateTime(factura_xml.Root.Attribute("fecha").Value), total_p, subtotal_p, descuento_p, traslado_p,retenido_p, false, DateTime.MinValue, Convert.ToDecimal(factura_xml.Root.Attribute("total").Value),factura_xml.Root.Attribute("condicionesDePago") == null ? "" : factura_xml.Root.Attribute("condicionesDePago").Value,emisor.dias_credito, 1, (byte)EstatusValidacion.ValidacionSintactica, "", id_usuario);
								}
								else
									result = new RetornoOperacion("No se puede recuperar el Receptor de la Factura");
							}
							else
								result = new RetornoOperacion("No se puede recuperar el Emisor de la Factura");
						}

						if (result.OperacionExitosa)
						{
							IEnumerable<XElement> conceptos = factura_xml.Root.Element(ns + "Conceptos").Elements();
							decimal tasa_imp_ret, tasa_imp_tras;
							idFactura = result.IdRegistro;

							if (conceptos != null)
							{
								foreach (XElement concepto in conceptos)
								{
									obtieneCantidadesConcepto32(factura_xml, concepto, out tasa_imp_tras, out tasa_imp_ret);

									result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(concepto.Attribute("cantidad").Value),concepto.Attribute("unidad") == null ? "" : concepto.Attribute("unidad").Value, concepto.Attribute("noIdentificacion") == null ? "" : concepto.Attribute("noIdentificacion").Value,concepto.Attribute("descripcion").Value, 0, Convert.ToDecimal(concepto.Attribute("valorUnitario") == null ? "1" : concepto.Attribute("valorUnitario").Value),Convert.ToDecimal(concepto.Attribute("importe") == null ? "1" : concepto.Attribute("importe").Value),Convert.ToDecimal(concepto.Attribute("importe") == null ? "1" : concepto.Attribute("importe").Value) * monto_tc,tasa_imp_ret, tasa_imp_tras, id_usuario);

									if (!result.OperacionExitosa)
										break;
								}
							}
							else
								result = new RetornoOperacion("No existen conceptos registrados");

							if (result.OperacionExitosa)
							{
								//Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
								string ruta = string.Format(@"{0}{1}\{2}", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + nombre_archivo);

								result = ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + nombre_archivo.Replace(".xml", " ").Trim(), id_usuario,Encoding.UTF8.GetBytes(factura_xml.ToString()), ruta);
							}
						}

						if (result.OperacionExitosa)
						{
							result = new RetornoOperacion(idFactura);
							transaccion.Complete();
						}
					}
				}
			}
			else
				result = new RetornoOperacion("El CFDI no corresponde a la Versión '3.2' especificada");

			return result;
		}
        /// <summary>
		/// Método encargado de Importar el CFDI en la versión 3.2
		/// </summary>
		/// <param name="id_compania">Compania Receptora</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio del Proveedor</param>
		/// <param name="nombre_archivo">Nombre del Archivo XML</param>
		/// <param name="factura_xml">Archivo XML a Importar</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion ImportaComprobanteVersion32(int id_compania, int id_tipo_servicio, string nombre_archivo, XDocument factura_xml, bool estatus, int id_usuario)
        {
            //Recuperando Namespace del Timbre Fiscal
            XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");
            XNamespace ns = factura_xml.Root.GetNamespaceOfPrefix("cfdi");

            RetornoOperacion result = new RetornoOperacion();
            int idFactura = 0;
            string versionCFDI = factura_xml.Root.Attribute("version") != null ? factura_xml.Root.Attribute("version").Value : "";

            //Validando Versión del CFDI
            if (versionCFDI.Equals("3.2"))
            {
                result = validaFacturaXML32(id_compania, id_tipo_servicio, factura_xml, id_usuario);

                if (result.OperacionExitosa)
                {
                    using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;
                        obtieneCantidades32(factura_xml, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

                        using (CompaniaEmisorReceptor emisor = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(factura_xml.Root.Element(ns + "Emisor").Attribute("rfc").Value, id_compania))
                        using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(id_compania))
                        {
                            if (emisor.habilitar)
                            {
                                if (receptor.habilitar)
                                {
                                    decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                    /** Retenciones **/
                                    //Validando que no exista el Nodo
                                    if (factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos") == null)
                                    {
                                        //Validando que existan Retenciones
                                        if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones") != null)
                                        {
                                            //Obteniendo Retenciones
                                            IEnumerable<XElement> retenciones = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").Elements();
                                            //Validando que existan Nodos
                                            if (retenciones != null)
                                            {
                                                //Recorriendo Retenciones
                                                foreach (XElement retencion in retenciones)
                                                    totalImpRetenidos += Convert.ToDecimal(retencion.Attribute("importe").Value);
                                            }
                                        }
                                    }
                                    else
                                        //Asignando Total de Impuestos
                                        totalImpRetenidos = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos").Value);

                                    /** Traslados **/
                                    //Validando que no exista el Nodo
                                    if (factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados") == null)
                                    {
                                        //Validando que existan Traslados
                                        if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados") != null)
                                        {
                                            //Obteniendo Retenciones
                                            IEnumerable<XElement> traslados = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados").Elements();
                                            //Validando que existan Nodos
                                            if (traslados != null)
                                            {
                                                foreach (XElement traslado in traslados)
                                                    //Sumando Impuestos Trasladados
                                                    totalImpRetenidos += Convert.ToDecimal(traslado.Attribute("importe").Value);
                                            }
                                        }
                                    }
                                    else
                                        //Asignando Total de Impuestos
                                        totalImpTrasladados = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados").Value);

                                    //Insertando factura
                                    result = InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor, 0, factura_xml.Root.Attribute("serie") == null ? "" : factura_xml.Root.Attribute("serie").Value, factura_xml.Root.Attribute("folio") == null ? "" : factura_xml.Root.Attribute("folio").Value, factura_xml.Root.Element(ns + "Complemento").Element(tfd + "TimbreFiscalDigital").Attribute("UUID").Value, Convert.ToDateTime(factura_xml.Root.Attribute("fecha").Value), (byte)TipoComprobante.CFDI, "", 0, estatus == true ? (byte)EstatusFactura.Aceptada : (byte)EstatusFactura.EnRevision, (byte)EstatusRecepion.Recibida, 0, 0, 0, Convert.ToDecimal(factura_xml.Root.Attribute("total").Value), Convert.ToDecimal(factura_xml.Root.Attribute("subTotal").Value), factura_xml.Root.Attribute("descuentos") == null ? 0 : Convert.ToDecimal(factura_xml.Root.Attribute("descuentos").Value), totalImpTrasladados, totalImpRetenidos, factura_xml.Root.Attribute("Moneda") == null ? "" : factura_xml.Root.Attribute("Moneda").Value, monto_tc, Convert.ToDateTime(factura_xml.Root.Attribute("fecha").Value), total_p, subtotal_p, descuento_p, traslado_p, retenido_p, false, DateTime.MinValue, Convert.ToDecimal(factura_xml.Root.Attribute("total").Value), factura_xml.Root.Attribute("condicionesDePago") == null ? "" : factura_xml.Root.Attribute("condicionesDePago").Value, emisor.dias_credito, 1, (byte)EstatusValidacion.ValidacionSintactica, "", id_usuario);
                                }
                                else
                                    result = new RetornoOperacion("No se puede recuperar el Receptor de la Factura");
                            }
                            else
                                result = new RetornoOperacion("No se puede recuperar el Emisor de la Factura");
                        }

                        if (result.OperacionExitosa)
                        {
                            IEnumerable<XElement> conceptos = factura_xml.Root.Element(ns + "Conceptos").Elements();
                            decimal tasa_imp_ret, tasa_imp_tras;
                            idFactura = result.IdRegistro;

                            if (conceptos != null)
                            {
                                foreach (XElement concepto in conceptos)
                                {
                                    obtieneCantidadesConcepto32(factura_xml, concepto, out tasa_imp_tras, out tasa_imp_ret);

                                    result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(concepto.Attribute("cantidad").Value), concepto.Attribute("unidad") == null ? "" : concepto.Attribute("unidad").Value, concepto.Attribute("noIdentificacion") == null ? "" : concepto.Attribute("noIdentificacion").Value, concepto.Attribute("descripcion").Value, 0, Convert.ToDecimal(concepto.Attribute("valorUnitario") == null ? "1" : concepto.Attribute("valorUnitario").Value), Convert.ToDecimal(concepto.Attribute("importe") == null ? "1" : concepto.Attribute("importe").Value), Convert.ToDecimal(concepto.Attribute("importe") == null ? "1" : concepto.Attribute("importe").Value) * monto_tc, tasa_imp_ret, tasa_imp_tras, id_usuario);

                                    if (!result.OperacionExitosa)
                                        break;
                                }
                            }
                            else
                                result = new RetornoOperacion("No existen conceptos registrados");

                            if (result.OperacionExitosa)
                            {
                                //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                string ruta = string.Format(@"{0}{1}\{2}", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + nombre_archivo);

                                result = ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + nombre_archivo.Replace(".xml", " ").Trim(), id_usuario, Encoding.UTF8.GetBytes(factura_xml.ToString()), ruta);
                            }
                        }

                        if (result.OperacionExitosa)
                        {
                            result = new RetornoOperacion(idFactura);
                            transaccion.Complete();
                        }
                    }
                }
            }
            else
                result = new RetornoOperacion("El CFDI no corresponde a la Versión '3.2' especificada");

            return result;
        }
        /// <summary>
        /// Método encargado de Importar el CFDI en la versión 3.2
        /// </summary>
        /// <param name="id_compania">Compania Receptora</param>
        /// <param name="id_recepcion">Recepción de la Factura</param>
        /// <param name="id_tipo_servicio">Tipo de Servicio del Proveedor</param>
        /// <param name="nombre_archivo">Nombre del Archivo XML</param>
        /// <param name="factura_xml">Archivo XML a Importar</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion ImportaComprobanteVersion32(int id_compania, int id_recepcion, int id_tipo_servicio, string nombre_archivo, XDocument factura_xml, int id_usuario)
		{
			//Recuperando Namespace del Timbre Fiscal
			XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");
			XNamespace ns = factura_xml.Root.GetNamespaceOfPrefix("cfdi");
			RetornoOperacion result = new RetornoOperacion();
			int idFactura = 0;
			string versionCFDI = factura_xml.Root.Attribute("version") != null ? factura_xml.Root.Attribute("version").Value : "";

			if (versionCFDI.Equals("3.2"))
			{
				result = validaFacturaXML32(id_compania, id_tipo_servicio, factura_xml, id_usuario);

				if (result.OperacionExitosa)
				{
					//Inicializando Bloque Transaccional
					using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;
						obtieneCantidades32(factura_xml, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

						using (CompaniaEmisorReceptor emisor = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(factura_xml.Root.Element(ns + "Emisor").Attribute("rfc").Value, id_compania))
						using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(id_compania))
						{
							if (emisor.habilitar)
							{
								if (receptor.habilitar)
								{
									decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

									/** Retenciones **/
									//Validando que no exista el Nodo
									if (factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos") == null)
									{
										//Validando que existan Retenciones
										if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones") != null)
										{
											//Obteniendo Retenciones
											IEnumerable<XElement> retenciones = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").Elements();

											//Validando que existan Nodos
											if (retenciones != null)
											{
												//Recorriendo Retenciones
												foreach (XElement retencion in retenciones)

													//Sumando Impuestos Retenidos
													totalImpRetenidos += Convert.ToDecimal(retencion.Attribute("importe").Value);
											}
										}
									}
									else
										//Asignando Total de Impuestos
										totalImpRetenidos = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos").Value);


									/** Traslados **/
									//Validando que no exista el Nodo
									if (factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados") == null)
									{
										//Validando que existan Traslados
										if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados") != null)
										{
											//Obteniendo Retenciones
											IEnumerable<XElement> traslados = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados").Elements();

											if (traslados != null)
											{
												foreach (XElement traslado in traslados)
													totalImpRetenidos += Convert.ToDecimal(traslado.Attribute("importe").Value);
											}
										}
									}
									else
										totalImpTrasladados = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados").Value);

									//Insertando factura
									result = InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor, 0, factura_xml.Root.Attribute("serie") == null ? "" : factura_xml.Root.Attribute("serie").Value, factura_xml.Root.Attribute("folio") == null ? "" : factura_xml.Root.Attribute("folio").Value, factura_xml.Root.Element(ns + "Complemento").Element(tfd + "TimbreFiscalDigital").Attribute("UUID").Value, Convert.ToDateTime(factura_xml.Root.Attribute("fecha").Value), (byte)TipoComprobante.CFDI, "", 0, (byte)EstatusFactura.EnRevision, (byte)EstatusRecepion.Recibida, id_recepcion, 0, 0, Convert.ToDecimal(factura_xml.Root.Attribute("total").Value), Convert.ToDecimal(factura_xml.Root.Attribute("subTotal").Value), factura_xml.Root.Attribute("descuentos") == null ? 0 : Convert.ToDecimal(factura_xml.Root.Attribute("descuentos").Value), totalImpTrasladados, totalImpRetenidos, factura_xml.Root.Attribute("Moneda") == null ? "" : factura_xml.Root.Attribute("Moneda").Value, monto_tc, Convert.ToDateTime(factura_xml.Root.Attribute("fecha").Value), total_p, subtotal_p, descuento_p, traslado_p, retenido_p, false, DateTime.MinValue, Convert.ToDecimal(factura_xml.Root.Attribute("total").Value), factura_xml.Root.Attribute("condicionesDePago") == null ? "" : factura_xml.Root.Attribute("condicionesDePago").Value, emisor.dias_credito, 1, (byte)EstatusValidacion.ValidacionSintactica, "", id_usuario);
								}
								else
									result = new RetornoOperacion("No se puede recuperar el Receptor de la Factura");
							}
							else
								result = new RetornoOperacion("No se puede recuperar el Emisor de la Factura");
						}

						if (result.OperacionExitosa)
						{
							IEnumerable<XElement> conceptos = factura_xml.Root.Element(ns + "Conceptos").Elements();
							decimal tasa_imp_ret, tasa_imp_tras;
							idFactura = result.IdRegistro;

							if (conceptos != null)
							{
								foreach (XElement concepto in conceptos)
								{
									obtieneCantidadesConcepto32(factura_xml, concepto, out tasa_imp_tras, out tasa_imp_ret);

									result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(concepto.Attribute("cantidad").Value),concepto.Attribute("unidad") == null ? "" : concepto.Attribute("unidad").Value, concepto.Attribute("noIdentificacion") == null ? "" : concepto.Attribute("noIdentificacion").Value,concepto.Attribute("descripcion").Value, 0, Convert.ToDecimal(concepto.Attribute("valorUnitario") == null ? "1" : concepto.Attribute("valorUnitario").Value),Convert.ToDecimal(concepto.Attribute("importe") == null ? "1" : concepto.Attribute("importe").Value),Convert.ToDecimal(concepto.Attribute("importe") == null ? "1" : concepto.Attribute("importe").Value) * monto_tc,tasa_imp_ret, tasa_imp_tras, id_usuario);

									if (!result.OperacionExitosa)
										break;
								}
							}
							else
								result = new RetornoOperacion("No existen conceptos registrados");

							if (result.OperacionExitosa)
							{
								//Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
								string ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + nombre_archivo);
								result = ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + nombre_archivo.Replace(".xml", " ").Trim(), id_usuario,Encoding.UTF8.GetBytes(factura_xml.ToString()), ruta);
							}
						}

						if (result.OperacionExitosa)
						{
							result = new RetornoOperacion(idFactura);
							transaccion.Complete();
						}
					}
				}
			}
			else
				result = new RetornoOperacion("El CFDI no corresponde a la Versión '3.2' especificada");

			return result;
		}
		/// <summary>
		/// Método encargado de Validar los Requisitos de la Factura en Formato XML
		/// </summary>
		/// <param name="id_compania">Compania Receptora</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio del Proveedor</param>
		/// <param name="fac_xml">Archivo XML de la Factura</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		private static RetornoOperacion validaFacturaXML32(int id_compania, int id_tipo_servicio, XDocument fac_xml, int id_usuario)
		{
			RetornoOperacion result = new RetornoOperacion();

			if (fac_xml != null)
			{
				//Obteniendo Espacio de Nombres
				XNamespace ns = fac_xml.Root.GetNamespaceOfPrefix("cfdi");

				using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					using (CompaniaEmisorReceptor emi = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(fac_xml.Root.Element(ns + "Emisor").Attribute("rfc").Value.ToUpper(), id_compania))
					{
						int idProveedorEmisor = 0;

						if (emi.id_compania_emisor_receptor > 0)
						{
							idProveedorEmisor = emi.id_compania_emisor_receptor;
							result = new RetornoOperacion(idProveedorEmisor);
						}
						else
						{
							result = CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto("", fac_xml.Root.Element(ns + "Emisor").Attribute("rfc").Value.ToUpper(), fac_xml.Root.Element(ns + "Emisor").Attribute("nombre").Value.ToUpper(), "", 0, false, false, true, id_tipo_servicio, "", "", "", 0, 0, id_compania, 0, "FACTURAS DE PROVEEDOR", "", 0, 0, id_usuario);

							if (result.OperacionExitosa)
							{
								idProveedorEmisor = result.IdRegistro;

								using (CompaniaEmisorReceptor pro = new CompaniaEmisorReceptor(idProveedorEmisor))
								{
									if (pro.habilitar)
									{
										int idPais = 0, idEstado = 0;
										Direccion.ObtienePaisEstado(fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("pais").Value, fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("estado").Value, out idPais, out idEstado);

										if (idPais > 0 && idEstado > 0)
										{
											result = Direccion.InsertaDireccion(2, fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("calle").Value,fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("noExterior") == null ? "" : fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("noExterior").Value,fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("noInterior") == null ? "" : fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("noInterior").Value,fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("colonia") == null ? "" : fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("colonia").Value,fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("localidad") == null ? "" : fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("localidad").Value,fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("municipio").Value,fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("referencia") == null ? "" : fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("referencia").Value,idEstado, idPais, fac_xml.Root.Element(ns + "Emisor").Element(ns + "DomicilioFiscal").Attribute("codigoPostal").Value, id_usuario);
										}
										else
											result = new RetornoOperacion(pro.id_compania_emisor_receptor);

										if (result.OperacionExitosa)
										{
											using (ProveedorTipoServicio TipoServicio = new ProveedorTipoServicio(pro.id_tipo_servicio))
											{
												if (TipoServicio.id_proveedor_tipo_servicio > 0)
													result = AsignacionTipoServicio.InsertarAsignacionTipoServicio(idProveedorEmisor, TipoServicio.id_proveedor_tipo_servicio, id_usuario);
												else
													result = new RetornoOperacion(pro.id_compania_emisor_receptor);

												if (result.OperacionExitosa)
													result = new RetornoOperacion(pro.id_compania_emisor_receptor);
											}
										}
									}
								}
							}
						}

						if (idProveedorEmisor > 0)
						{
							using (CompaniaEmisorReceptor cer = new CompaniaEmisorReceptor(id_compania))
							{
								//Validando que exista el Receptor
								if (cer.rfc.ToUpper().Equals(fac_xml.Root.Element(ns + "Receptor").Attribute("rfc").Value.ToUpper()) && cer.habilitar)
								{
									//Instanciando XSD de validación
									using (EsquemasFacturacion EsquemaF = new EsquemasFacturacion(fac_xml.Root.Attribute("version").Value))
									{
										//Validando que exista el XSD
										if (EsquemaF.habilitar)
										{
											bool addenda;
											//Obteniendo XSD
											string[] esquemas = EsquemasFacturacion.CargaEsquemasPadreYComplementosCFDI(EsquemaF.version, id_compania, out addenda);

											//Validando que Existen Addendas
											if (fac_xml.Root.Element(ns + "Addenda") != null)

												//Removiendo Addendas
												fac_xml.Root.Element(ns + "Addenda").RemoveAll();

											//Obteniendo Validación
											string mensaje = "";
											result = new RetornoOperacion(mensaje, Xml.ValidaXMLSchema(fac_xml.ToString(), esquemas, out mensaje));
										}
									}
								}
								else
									//Instanciando Excepción
									result = new RetornoOperacion("El RFC de la factura no coincide con el Receptor");

								//Validando que las Operaciónes se Completaron
								if (result.OperacionExitosa)

									//Completando Transacción
									transaccion.Complete();
							}
						}
					}
				}
			}
			else
				//Instanciando Excepción
				result = new RetornoOperacion("La Factura XML no es valida");

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método Privado encargado de Obtener las Cantidades de la Factura
		/// </summary>
		/// <param name="document">Factura XML</param>
		/// <param name="total_p">Total en Pesos</param>
		/// <param name="subtotal_p">Subtotal en Pesos</param>
		/// <param name="descuento_p">Descuento en Pesos</param>
		/// <param name="traslado_p">Importe Trasladado en Pesos</param>
		/// <param name="retenido_p">Importe Retenido en Pesos</param>
		/// <param name="monto_tc">Monto del Tipo de Cambio</param>
		private static void obtieneCantidades32(XDocument document, out decimal total_p, out decimal subtotal_p, out decimal descuento_p, out decimal traslado_p, out decimal retenido_p, out decimal monto_tc)
		{
			//Obteniendo Espacio de Nombres
			XNamespace ns = document.Root.GetNamespaceOfPrefix("cfdi");

			//Validando si existe el Tipo de Cambio
			if (document.Root.Attribute("TipoCambio") == null)

				//Asignando Tipo de Cambio en 1
				monto_tc = 1;
			else
				//Asignando Tipo de Cambio del Comprobante
				monto_tc = Convert.ToDecimal(document.Root.Attribute("TipoCambio").Value);

			//Validando que el Monto no sea Negativo
			monto_tc = monto_tc < 0 ? 1 : monto_tc;

			//Asignando Atributo Obligatorios
			total_p = Convert.ToDecimal(document.Root.Attribute("total").Value) * monto_tc;
			subtotal_p = Convert.ToDecimal(document.Root.Attribute("subTotal").Value) * monto_tc;
			traslado_p = (document.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados") != null ?
					Convert.ToDecimal(document.Root.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados").Value) : 0) * monto_tc;
			retenido_p = (document.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos") != null ?
					Convert.ToDecimal(document.Root.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos").Value) : 0) * monto_tc;

			//Asignando Atributos Opcionales
			descuento_p = (document.Root.Attribute("descuento") == null ? 0 : Convert.ToDecimal(document.Root.Attribute("descuento").Value)) * monto_tc;
		}
		/// <summary>
		/// Método Privado encargado de Obtener las Cantidades de los Conceptos de la Factura
		/// </summary>
		/// <param name="cfdi"></param>
		/// <param name="concepto"></param>
		/// <param name="tasa_imp_tras"></param>
		/// <param name="tasa_imp_ret"></param>
		/// <param name="imp_ret"></param>
		/// <param name="imp_tras"></param>
		private static void obtieneCantidadesConcepto32(XDocument cfdi, XElement concepto, out decimal tasa_imp_tras, out decimal tasa_imp_ret)
		{
			//Obteniendo Espacio de Nombres
			XNamespace ns = cfdi.Root.GetNamespaceOfPrefix("cfdi");
			//Asignando Valores
			tasa_imp_tras = tasa_imp_ret = 0;

			//Validación de Retenciones
			if (cfdi.Root.Element(ns + "Impuestos").Element(ns + "Retenciones") != null)
			{
				//Obteniendo Retenciones
				IEnumerable<XElement> retenciones = cfdi.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").Elements();

				//Validando que existan Nodos
				if (retenciones != null)
				{
					//Recorriendo Retenciones
					foreach (XElement retencion in retenciones)
					{
						//Validando que el Importe no sea "0"
						if (Convert.ToDecimal(retencion.Attribute("importe").Value) > 0 && Convert.ToDecimal(concepto.Attribute("importe").Value) > 0)

							//Calculando Tasa de Retención
							tasa_imp_ret += (Convert.ToDecimal(retencion.Attribute("importe").Value) / Convert.ToDecimal(concepto.Attribute("importe").Value)) * 100;

					}
				}
			}

			//Validación de Traslados
			if (cfdi.Root.Element(ns + "Impuestos").Element(ns + "Traslados") != null)
			{
				//Obteniendo Traslados
				IEnumerable<XElement> traslados = cfdi.Root.Element(ns + "Impuestos").Element(ns + "Traslados").Elements();

				//Validando que existan Nodos
				if (traslados != null)
				{
					//Recorriendo Traslados
					foreach (XElement traslado in traslados)
					{
						//Validando que el Importe no sea "0"
						if (Convert.ToDecimal(traslado.Attribute("importe").Value) > 0 && Convert.ToDecimal(concepto.Attribute("importe").Value) > 0)

							//Calculando Tasa de Traslado
							tasa_imp_tras += (Convert.ToDecimal(traslado.Attribute("importe").Value) / Convert.ToDecimal(concepto.Attribute("importe").Value)) * 100;

					}
				}
			}
		}
		#endregion

		#region Importación CFDI 3.3
		/// <summary>
		/// Método encargado de Importar el CFDI en la versión 3.3
		/// </summary>
		/// <param name="id_compania">Compania Receptora</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio del Proveedor</param>
		/// <param name="nombre_archivo">Nombre del Archivo XML</param>
		/// <param name="factura_xml">Archivo XML a Importar</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion ImportaComprobanteVersion33(int id_compania, int id_tipo_servicio, string nombre_archivo, XDocument factura_xml, int id_usuario) 
		{
			//Declarando Objeto de Retorno
			RetornoOperacion resultado = new RetornoOperacion();
			//Recuperando Namespace del Timbre Fiscal
			XNamespace nsCFDI = factura_xml.Root.GetNamespaceOfPrefix("cfdi");
			XNamespace nsTFD = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");
			XNamespace nsPago = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace Pagos SAT");
			//Declarando Variable para Factura
			int idFacturadoProveedor = 0;

			//Obteniendo Version
			string versionCFDI = factura_xml.Root.Attribute("Version") != null ? factura_xml.Root.Attribute("Version").Value : "";

			if (versionCFDI.Equals("3.3"))
			{
				resultado = validaFacturaXML33(id_compania, id_tipo_servicio, factura_xml, id_usuario);

				if (resultado.OperacionExitosa)
				{
					using (TransactionScope transaccionFactura = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;
						obtieneCantidades33(factura_xml, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);
						//Usando Emisor y compania
						using (CompaniaEmisorReceptor emisor = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(factura_xml.Root.Element(nsCFDI + "Emisor").Attribute("Rfc").Value, id_compania))
						using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(id_compania))
						{
							if (emisor.habilitar)
							{
								if (receptor.habilitar)
								{
									decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

									if (factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("P"))
										totalImpRetenidos = totalImpRetenidos = 0;
									else
									{
                                        //Validando existencia de Impuestos
                                        if (factura_xml.Root.Element(nsCFDI + "Impuestos") != null)
                                        {
                                            /** Retenciones **/
                                            if (factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosRetenidos") == null)
                                            {
                                                //Validando que existan Retenciones
                                                if (factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Retenciones") != null)
                                                {
                                                    IEnumerable<XElement> retenciones = factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Retenciones").Elements();
                                                    if (retenciones != null)
                                                        foreach (XElement retencion in retenciones)
                                                            totalImpRetenidos += Convert.ToDecimal(retencion.Attribute("Importe").Value);
                                                }
                                            }
                                            else
                                                totalImpRetenidos = Convert.ToDecimal(factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosRetenidos").Value);
                                            /** Traslados **/
                                            if (factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosTrasladados") == null)
                                            {
                                                if (factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Traslados") != null)
                                                {
                                                    IEnumerable<XElement> traslados = factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Traslados").Elements();
                                                    if (traslados != null)
                                                        foreach (XElement traslado in traslados)
                                                            totalImpRetenidos += Convert.ToDecimal(traslado.Attribute("Importe").Value);
                                                }
                                            }
                                            else
                                                totalImpTrasladados = Convert.ToDecimal(factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosTrasladados").Value);
                                        }
                                        else
                                            totalImpRetenidos = totalImpRetenidos = 0;
                                    }

									//Insertando factura
									resultado = InsertaFacturadoProveedor(
											emisor.id_compania_emisor_receptor, 
											receptor.id_compania_emisor_receptor, 
											0, //Id Servicio
											factura_xml.Root.Attribute("Serie") == null ? "" : factura_xml.Root.Attribute("Serie").Value,
											factura_xml.Root.Attribute("Folio") == null ? "" : factura_xml.Root.Attribute("Folio").Value,
											factura_xml.Root.Element(nsCFDI + "Complemento").Element(nsTFD + "TimbreFiscalDigital").Attribute("UUID").Value,
											Convert.ToDateTime(factura_xml.Root.Attribute("Fecha").Value), 
											(byte)TipoComprobante.CFDI3_3,
											factura_xml.Root.Attribute("TipoDeComprobante").Value, 
											0, //Id Naturaleza CFDI
											(byte)EstatusFactura.EnRevision, (byte)EstatusRecepion.Recibida, 
											0, //Id Recepcion
											0, //Id Segmento Negocio
											0, //Id Tipo Servicio
											Convert.ToDecimal(factura_xml.Root.Attribute("Total").Value), Convert.ToDecimal(factura_xml.Root.Attribute("SubTotal").Value),
											factura_xml.Root.Attribute("Descuento") == null ? 0 : Convert.ToDecimal(factura_xml.Root.Attribute("Descuento").Value),
											totalImpTrasladados, totalImpRetenidos,
											factura_xml.Root.Attribute("Moneda") == null ? "" : factura_xml.Root.Attribute("Moneda").Value,
											monto_tc, 
											Convert.ToDateTime(factura_xml.Root.Attribute("Fecha").Value), 
											total_p, subtotal_p, descuento_p, traslado_p, retenido_p, 
											false, DateTime.MinValue, 
											Convert.ToDecimal(factura_xml.Root.Attribute("Total").Value),
											factura_xml.Root.Attribute("CondicionesDePago") == null ? "" : factura_xml.Root.Attribute("CondicionesDePago").Value,
											emisor.dias_credito, 
											1, //Id Causa Falta Pago
											(byte)EstatusValidacion.ValidacionSintactica, 
											"", //Resultado Validacion
											id_usuario);
								}
								else
									//Instanciando Excepción
									resultado = new RetornoOperacion("No se puede recuperar el Receptor de la Factura");
							}
							else
								//Instanciando Excepción
								resultado = new RetornoOperacion("No se puede recuperar el Emisor de la Factura");
						}

						if (resultado.OperacionExitosa)
						{
							decimal tasa_imp_ret = 0, tasa_imp_tras = 0;
							IEnumerable<XElement> conceptos = factura_xml.Root.Element(nsCFDI + "Conceptos").Elements();
							idFacturadoProveedor = resultado.IdRegistro;

							if (conceptos != null)
							{
								foreach (XElement concepto in conceptos)
								{
									if (factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("I") || factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("E"))
										obtieneCantidadesConcepto33(factura_xml, concepto, out tasa_imp_tras, out tasa_imp_ret);

									resultado = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(
										idFacturadoProveedor,
										Convert.ToDecimal(concepto.Attribute("Cantidad").Value),
										Cadena.TruncaCadena(string.Format("{0}-{1}", concepto.Attribute("ClaveUnidad").Value, concepto.Attribute("Unidad") == null ? "" : concepto.Attribute("Unidad").Value),7,"..."),
										concepto.Attribute("NoIdentificacion") == null ? "" : concepto.Attribute("NoIdentificacion").Value,
										string.Format("{0} - {1}",concepto.Attribute("ClaveProdServ").Value, concepto.Attribute("Descripcion").Value), 
										0, //Id Clasificacion Contable
										Convert.ToDecimal(concepto.Attribute("ValorUnitario") == null ? "1" : concepto.Attribute("ValorUnitario").Value),
										Convert.ToDecimal(concepto.Attribute("Importe") == null ? "1" : concepto.Attribute("Importe").Value),
										Convert.ToDecimal(concepto.Attribute("Importe") == null ? "1" : concepto.Attribute("Importe").Value) * monto_tc,
										tasa_imp_ret, tasa_imp_tras, id_usuario);
									//Si algo no salio bien
									if (!resultado.OperacionExitosa)
										break;
								}
							}
							else
								resultado = new RetornoOperacion("No existen conceptos registrados");

							if (resultado.OperacionExitosa)
							{
								if (factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("P"))
								{
									//Si el archivo XML contiene Pagos, insertarlos junto con los documentos que contengan
									IEnumerable<XElement> pagos = factura_xml.Root.Element(nsCFDI + "Complemento").Element(nsPago + "Pagos").Elements();
									if (pagos != null)
										resultado = ImportaComplementoPago10(pagos, idFacturadoProveedor, nsPago, id_usuario, id_compania);
									else
										resultado = new RetornoOperacion("No existen pagos registrados");
								}

								//Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
								string ruta = string.Format(@"{0}{1}\{2}", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFacturadoProveedor.ToString("0000000") + "-" + nombre_archivo);

								resultado = ArchivoRegistro.InsertaArchivoRegistro(72, idFacturadoProveedor, 8, "FACTURA: " + nombre_archivo.Replace(".xml", " ").Trim(), id_usuario, Encoding.UTF8.GetBytes(factura_xml.ToString()), ruta);
							}
						}

						if (resultado.OperacionExitosa)
						{
							resultado = new RetornoOperacion(idFacturadoProveedor);
							transaccionFactura.Complete();
						}
					}
				}
			}
			else
				resultado = new RetornoOperacion("El CFDI no corresponde a la Versión especificada");

			return resultado;
		}
        /// <summary>
		/// Método encargado de Importar el CFDI en la versión 3.3
		/// </summary>
		/// <param name="id_compania">Compania Receptora</param>
		/// <param name="id_tipo_servicio">Tipo de Servicio del Proveedor</param>
		/// <param name="nombre_archivo">Nombre del Archivo XML</param>
		/// <param name="factura_xml">Archivo XML a Importar</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion ImportaComprobanteVersion33(int id_compania, int id_tipo_servicio, string nombre_archivo, XDocument factura_xml, bool estatus, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Recuperando Namespace del Timbre Fiscal
            XNamespace nsCFDI = factura_xml.Root.GetNamespaceOfPrefix("cfdi");
            XNamespace nsTFD = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");
            XNamespace nsPago = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace Pagos SAT");
            //Declarando Variable para Factura
            int idFacturadoProveedor = 0;

            //Obteniendo Version
            string versionCFDI = factura_xml.Root.Attribute("Version") != null ? factura_xml.Root.Attribute("Version").Value : "";

            if (versionCFDI.Equals("3.3"))
            {
                resultado = validaFacturaXML33(id_compania, id_tipo_servicio, factura_xml, id_usuario);

                if (resultado.OperacionExitosa)
                {
                    using (TransactionScope transaccionFactura = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;
                        obtieneCantidades33(factura_xml, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);
                        //Usando Emisor y compania
                        using (CompaniaEmisorReceptor emisor = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(factura_xml.Root.Element(nsCFDI + "Emisor").Attribute("Rfc").Value, id_compania))
                        using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(id_compania))
                        {
                            if (emisor.habilitar)
                            {
                                if (receptor.habilitar)
                                {
                                    decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                    if (factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("P"))
                                        totalImpRetenidos = totalImpRetenidos = 0;
                                    else
                                    {
                                        //Validando existencia de Impuestos
                                        if (factura_xml.Root.Element(nsCFDI + "Impuestos") != null)
                                        {
                                            /** Retenciones **/
                                            if (factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosRetenidos") == null)
                                            {
                                                //Validando que existan Retenciones
                                                if (factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Retenciones") != null)
                                                {
                                                    IEnumerable<XElement> retenciones = factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Retenciones").Elements();
                                                    if (retenciones != null)
                                                        foreach (XElement retencion in retenciones)
                                                            totalImpRetenidos += Convert.ToDecimal(retencion.Attribute("Importe").Value);
                                                }
                                            }
                                            else
                                                totalImpRetenidos = Convert.ToDecimal(factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosRetenidos").Value);
                                            /** Traslados **/
                                            if (factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosTrasladados") == null)
                                            {
                                                if (factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Traslados") != null)
                                                {
                                                    IEnumerable<XElement> traslados = factura_xml.Root.Element(nsCFDI + "Impuestos").Element(nsCFDI + "Traslados").Elements();
                                                    if (traslados != null)
                                                        foreach (XElement traslado in traslados)
                                                            totalImpRetenidos += Convert.ToDecimal(traslado.Attribute("Importe").Value);
                                                }
                                            }
                                            else
                                                totalImpTrasladados = Convert.ToDecimal(factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosTrasladados").Value);
                                        }
                                        else
                                            totalImpRetenidos = totalImpRetenidos = 0;
                                    }

                                    //Insertando factura
                                    resultado = InsertaFacturadoProveedor(
                                            emisor.id_compania_emisor_receptor,
                                            receptor.id_compania_emisor_receptor,
                                            0, //Id Servicio
                                            factura_xml.Root.Attribute("Serie") == null ? "" : factura_xml.Root.Attribute("Serie").Value,
                                            factura_xml.Root.Attribute("Folio") == null ? "" : factura_xml.Root.Attribute("Folio").Value,
                                            factura_xml.Root.Element(nsCFDI + "Complemento").Element(nsTFD + "TimbreFiscalDigital").Attribute("UUID").Value,
                                            Convert.ToDateTime(factura_xml.Root.Attribute("Fecha").Value),
                                            (byte)TipoComprobante.CFDI3_3,
                                            factura_xml.Root.Attribute("TipoDeComprobante").Value,
                                            0, //Id Naturaleza CFDI
                                            estatus == true ? (byte)EstatusFactura.Aceptada : (byte)EstatusFactura.EnRevision, (byte)EstatusRecepion.Recibida,
                                            0, //Id Recepcion
                                            0, //Id Segmento Negocio
                                            0, //Id Tipo Servicio
                                            Convert.ToDecimal(factura_xml.Root.Attribute("Total").Value), Convert.ToDecimal(factura_xml.Root.Attribute("SubTotal").Value),
                                            factura_xml.Root.Attribute("Descuento") == null ? 0 : Convert.ToDecimal(factura_xml.Root.Attribute("Descuento").Value),
                                            totalImpTrasladados, totalImpRetenidos,
                                            factura_xml.Root.Attribute("Moneda") == null ? "" : factura_xml.Root.Attribute("Moneda").Value,
                                            monto_tc,
                                            Convert.ToDateTime(factura_xml.Root.Attribute("Fecha").Value),
                                            total_p, subtotal_p, descuento_p, traslado_p, retenido_p,
                                            false, DateTime.MinValue,
                                            Convert.ToDecimal(factura_xml.Root.Attribute("Total").Value),
                                            factura_xml.Root.Attribute("CondicionesDePago") == null ? "" : factura_xml.Root.Attribute("CondicionesDePago").Value,
                                            emisor.dias_credito,
                                            1, //Id Causa Falta Pago
                                            (byte)EstatusValidacion.ValidacionSintactica,
                                            "", //Resultado Validacion
                                            id_usuario);
                                }
                                else
                                    //Instanciando Excepción
                                    resultado = new RetornoOperacion("No se puede recuperar el Receptor de la Factura");
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No se puede recuperar el Emisor de la Factura");
                        }

                        if (resultado.OperacionExitosa)
                        {
                            decimal tasa_imp_ret = 0, tasa_imp_tras = 0;
                            IEnumerable<XElement> conceptos = factura_xml.Root.Element(nsCFDI + "Conceptos").Elements();
                            idFacturadoProveedor = resultado.IdRegistro;

                            if (conceptos != null)
                            {
                                foreach (XElement concepto in conceptos)
                                {
                                    if (factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("I") || factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("E"))
                                        obtieneCantidadesConcepto33(factura_xml, concepto, out tasa_imp_tras, out tasa_imp_ret);

                                    resultado = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(
                                        idFacturadoProveedor,
                                        Convert.ToDecimal(concepto.Attribute("Cantidad").Value),
                                        Cadena.TruncaCadena(string.Format("{0}-{1}", concepto.Attribute("ClaveUnidad").Value, concepto.Attribute("Unidad") == null ? "" : concepto.Attribute("Unidad").Value), 7, "..."),
                                        concepto.Attribute("NoIdentificacion") == null ? "" : concepto.Attribute("NoIdentificacion").Value,
                                        string.Format("{0} - {1}", concepto.Attribute("ClaveProdServ").Value, concepto.Attribute("Descripcion").Value),
                                        0, //Id Clasificacion Contable
                                        Convert.ToDecimal(concepto.Attribute("ValorUnitario") == null ? "1" : concepto.Attribute("ValorUnitario").Value),
                                        Convert.ToDecimal(concepto.Attribute("Importe") == null ? "1" : concepto.Attribute("Importe").Value),
                                        Convert.ToDecimal(concepto.Attribute("Importe") == null ? "1" : concepto.Attribute("Importe").Value) * monto_tc,
                                        tasa_imp_ret, tasa_imp_tras, id_usuario);
                                    //Si algo no salio bien
                                    if (!resultado.OperacionExitosa)
                                        break;
                                }
                            }
                            else
                                resultado = new RetornoOperacion("No existen conceptos registrados");

                            if (resultado.OperacionExitosa)
                            {
                                if (factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("P"))
                                {
                                    //Si el archivo XML contiene Pagos, insertarlos junto con los documentos que contengan
                                    IEnumerable<XElement> pagos = factura_xml.Root.Element(nsCFDI + "Complemento").Element(nsPago + "Pagos").Elements();
                                    if (pagos != null)
                                        resultado = ImportaComplementoPago10(pagos, idFacturadoProveedor, nsPago, id_usuario, id_compania);
                                    else
                                        resultado = new RetornoOperacion("No existen pagos registrados");
                                }

                                //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                string ruta = string.Format(@"{0}{1}\{2}", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFacturadoProveedor.ToString("0000000") + "-" + nombre_archivo);

                                resultado = ArchivoRegistro.InsertaArchivoRegistro(72, idFacturadoProveedor, 8, "FACTURA: " + nombre_archivo.Replace(".xml", " ").Trim(), id_usuario, Encoding.UTF8.GetBytes(factura_xml.ToString()), ruta);
                            }
                        }

                        if (resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion(idFacturadoProveedor);
                            transaccionFactura.Complete();
                        }
                    }
                }
            }
            else
                resultado = new RetornoOperacion("El CFDI no corresponde a la Versión especificada");

            return resultado;
        }
        /// <summary>
        /// Método encargado de insertar la informacion de los Pagos:Pago y Pago:DoctoRelacionado de la factura
        /// </summary>
        /// <param name="factura_xml"></param>
        /// <returns></returns>
        private static RetornoOperacion ImportaComplementoPago10(IEnumerable<XElement> pagos, int idFacturadoProveedor, XNamespace nsPago, int idUsuario, int idCompania)
		{
			RetornoOperacion resultado = new RetornoOperacion();
			int idPagoFacturado = 0;
			//Insertar cada pago y los documentos si los incluye
			using (TransactionScope transaccionPagosDocumentos = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				foreach (XElement pago in pagos)
				{
					resultado = PagoFacturado.Insertar(
						idFacturadoProveedor,
						Convert.ToDateTime(pago.Attribute("FechaPago").Value),
						FormaPago.ObtenerId(pago.Attribute("FormaDePagoP").Value),
						Moneda.ObtenerId(pago.Attribute("MonedaP").Value),
						Convert.ToDecimal(pago.Attribute("Monto").Value),
						pago.Attribute("NumOperacion").Value,
						pago.Attribute("RfcEmisorCtaOrd") != null ? pago.Attribute("RfcEmisorCtaOrd").Value : "", /* El SAT dice "Se puede" */
						pago.Attribute("CtaOrdenante").Value,/* El SAT dice "Se debe" */
						pago.Attribute("RfcEmisorCtaBen") != null ? pago.Attribute("RfcEmisorCtaBen").Value : "", /* El SAT dice "Se puede" */
						pago.Attribute("CtaBeneficiario") != null ? pago.Attribute("CtaBeneficiario").Value: "",/* El SAT dice "Se puede" */
						Convert.ToByte(pago.Attribute("TipoCadenaPago") != null ? Convert.ToByte(Catalogo.RegresaDescripcionValor(3206, pago.Attribute("TipoCadPago").Value)) : 0),
						pago.Attribute("TipoCambioP") != null ? Convert.ToDecimal(pago.Attribute("TipoCambioP").Value) : 1,
						pago.Attribute("MonedaP").Value == "MXN" ? Fecha.ObtieneFechaEstandarMexicoCentro() : Convert.ToDateTime(pago.Attribute("FechaPago").Value),
						0,/*Buscar banco*/
						idUsuario
						);
					//Si se inserta
					if (resultado.OperacionExitosa)
					{
						idPagoFacturado = resultado.IdRegistro;
						IEnumerable<XElement> documentos = pago.Elements();

						if (documentos != null)
						{
							foreach (XElement documento in documentos)
							{
								resultado = DocumentoPago.Insertar(
									idPagoFacturado,
									documento.Attribute("IdDocumento").Value,
									documento.Attribute("Serie") != null ? documento.Attribute("Serie").Value : "",
									documento.Attribute("Folio") != null ? documento.Attribute("Folio").Value : "",
									Moneda.ObtenerId(documento.Attribute("MonedaDR").Value),
									Convert.ToDecimal(documento.Attribute("TipoCambioDR") != null ? Convert.ToDecimal(documento.Attribute("TipoCambioDR").Value) : 1),
									Convert.ToByte(Catalogo.RegresaValorCadenaValor(3195, documento.Attribute("MetodoDePagoDR").Value)),
									Convert.ToByte(documento.Attribute("NumParcialidad").Value),
									Convert.ToDecimal(documento.Attribute("ImpSaldoAnt").Value),
									Convert.ToDecimal(documento.Attribute("ImpPagado").Value),
									Convert.ToDecimal(documento.Attribute("ImpSaldoInsoluto").Value),
									idUsuario
									);
								//Si no se logra insertar alguno, dejar de insertar el resto
								if (!resultado.OperacionExitosa)
									break;
							}
						}
						else resultado = new RetornoOperacion("No se encontraron Documentos Relacionados en la factura");
					}
					//Si no se logra insertar alguno, dejar de insertar el resto
					if (!resultado.OperacionExitosa)
						break;
				}
				//Terminar cuando Pagos y Documentos se inserten correctamente
				if (resultado.OperacionExitosa)
				{
					transaccionPagosDocumentos.Complete();
				}
			}
			//Despues de insertar todos los Documentos, actualizar estatus
			if (resultado.OperacionExitosa)
				using (PagoFacturado pagoFacturado = new PagoFacturado(idPagoFacturado))
					if (pagoFacturado.habilitar)
						resultado = pagoFacturado.ActualizaEstatusAutomatico(idCompania, idUsuario);
			return resultado;
		}
		/// <summary>
		/// Método encargado de Importar el CFDI en la versión 3.3
		/// </summary>
		/// <param name="id_compania">Compania Receptora</param>
		/// <param name="id_recepcion">Recepción de Factura</param>
		/// <param name="fecha_recepcion"></param>
		/// <param name="entregado_por"></param>
		/// <param name="id_medio_recepcion"></param>
		/// <param name="id_tipo_servicio">Tipo de Servicio del Proveedor</param>
		/// <param name="nombre_archivo">Nombre del Archivo XML</param>
		/// <param name="factura_xml">Archivo XML a Importar</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns>Devuelve el ID de Recepción</returns>
		public static RetornoOperacion ImportaComprobanteVersion33(int id_compania, int id_recepcion, DateTime fecha_recepcion, string entregado_por,byte id_medio_recepcion, int id_tipo_servicio, string nombre_archivo, XDocument factura_xml, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			XNamespace ns = factura_xml.Root.GetNamespaceOfPrefix("cfdi");
			//Recuperando Namespace del Timbre Fiscal
			XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");
			//Declarando Variable para Factura
			int idFactura = 0;
			int idRecepcion = 0;

			//Obteniendo Version
			string versionCFDI = factura_xml.Root.Attribute("Version") != null ? factura_xml.Root.Attribute("Version").Value : "";

			//Validando Versión del CFDI
			if (versionCFDI.Equals("3.3"))
			{
				//Obteniendo validación de Factura
				result = validaFacturaXML33(id_compania, id_tipo_servicio, factura_xml, id_usuario);

				//Validando Operación
				if (result.OperacionExitosa)
				{
					//Inicializando Bloque Transaccional
					using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						//Declarando variables de Montos
						decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

						//Obteniendo Valores
						obtieneCantidades33(factura_xml, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

						//Instanciando Emisor-Compania
						using (CompaniaEmisorReceptor emisor = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(factura_xml.Root.Element(ns + "Emisor").Attribute("Rfc").Value, id_compania))
						using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(id_compania))
						{
							//Validando que coincida el RFC del Emisor
							if (emisor.habilitar)
							{
								//Validando que coincida el RFC del Emisor
								if (receptor.habilitar)
								{
									//De acuerdo al estatus de la pagina
									if (id_recepcion == 0)

										//Insertando encabezado
										result = Recepcion.InsertaRecepcion(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
																										fecha_recepcion, entregado_por.ToUpper(), id_medio_recepcion, id_usuario);
									else
									{
										//Instanciamos la recepcion que se desea modificar 
										using (Recepcion recepcion = new Recepcion(id_recepcion))
										{
											//Validando Recepción
											if (recepcion.habilitar)

												//Editando el encabezado
												result = recepcion.EditaRecepcion(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor, recepcion.secuencia,
																										fecha_recepcion, entregado_por.ToUpper(), id_medio_recepcion, id_usuario);
											else
												//Instanciando Excepción
												result = new RetornoOperacion("No se puede recuperar la Recepción");
										}
									}

									//validando Operación Exitosa
									if (result.OperacionExitosa)
									{
										//Recuperando Recepción
										idRecepcion = result.IdRegistro;

										//Declarando Variables Auxiliares
										decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                        //Validando Impuestos Generales
                                        if (factura_xml.Root.Element(ns + "Impuestos") != null)
                                        {
                                            /** Retenciones **/
                                            //Validando que no exista el Nodo
                                            if (factura_xml.Root.Element(ns + "Impuestos").Attribute("TotalImpuestosRetenidos") == null)
                                            {
                                                //Validando que existan Retenciones
                                                if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones") != null)
                                                {
                                                    //Obteniendo Retenciones
                                                    IEnumerable<XElement> retenciones = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").Elements();

                                                    //Validando que existan Nodos
                                                    if (retenciones != null)
                                                    {
                                                        //Recorriendo Retenciones
                                                        foreach (XElement retencion in retenciones)

                                                            //Sumando Impuestos Retenidos
                                                            totalImpRetenidos += Convert.ToDecimal(retencion.Attribute("Importe").Value);
                                                    }
                                                }
                                            }
                                            else
                                                //Asignando Total de Impuestos
                                                totalImpRetenidos = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("TotalImpuestosRetenidos").Value);

                                            /** Traslados **/
                                            //Validando que no exista el Nodo
                                            if (factura_xml.Root.Element(ns + "Impuestos").Attribute("TotalImpuestosTrasladados") == null)
                                            {
                                                //Validando que existan Traslados
                                                if (factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados") != null)
                                                {
                                                    //Obteniendo Retenciones
                                                    IEnumerable<XElement> traslados = factura_xml.Root.Element(ns + "Impuestos").Element(ns + "Traslados").Elements();

                                                    //Validando que existan Nodos
                                                    if (traslados != null)
                                                    {
                                                        //Recorriendo Traslados
                                                        foreach (XElement traslado in traslados)

                                                            //Sumando Impuestos Trasladados
                                                            totalImpRetenidos += Convert.ToDecimal(traslado.Attribute("Importe").Value);
                                                    }
                                                }
                                            }
                                            else
                                                //Asignando Total de Impuestos
                                                totalImpTrasladados = Convert.ToDecimal(factura_xml.Root.Element(ns + "Impuestos").Attribute("TotalImpuestosTrasladados").Value);
                                        }

										//Insertando factura
										result = InsertaFacturadoProveedor(
												emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor, 0,
												factura_xml.Root.Attribute("Serie") == null ? "" : factura_xml.Root.Attribute("Serie").Value,
												factura_xml.Root.Attribute("Folio") == null ? "" : factura_xml.Root.Attribute("Folio").Value,
												factura_xml.Root.Element(ns + "Complemento").Element(tfd + "TimbreFiscalDigital").Attribute("UUID").Value,
												Convert.ToDateTime(factura_xml.Root.Attribute("Fecha").Value), (byte)TipoComprobante.CFDI3_3, "", 0,
												(byte)EstatusFactura.EnRevision, (byte)EstatusRecepion.Recibida,
												idRecepcion, 0, 0, Convert.ToDecimal(factura_xml.Root.Attribute("Total").Value), Convert.ToDecimal(factura_xml.Root.Attribute("SubTotal").Value),
												factura_xml.Root.Attribute("Descuento") == null ? 0 : Convert.ToDecimal(factura_xml.Root.Attribute("Descuento").Value),
												totalImpTrasladados, totalImpRetenidos,
												factura_xml.Root.Attribute("Moneda") == null ? "" : factura_xml.Root.Attribute("Moneda").Value,
												monto_tc, Convert.ToDateTime(factura_xml.Root.Attribute("Fecha").Value), total_p, subtotal_p, descuento_p, traslado_p,
												retenido_p, false, DateTime.MinValue, Convert.ToDecimal(factura_xml.Root.Attribute("Total").Value),
												factura_xml.Root.Attribute("CondicionesDePago") == null ? "" : factura_xml.Root.Attribute("CondicionesDePago").Value,
												emisor.dias_credito, 1, (byte)EstatusValidacion.ValidacionSintactica, "", id_usuario);
									}
								}
								else
									//Instanciando Excepción
									result = new RetornoOperacion("No se puede recuperar el Receptor de la Factura");
							}
							else
								//Instanciando Excepción
								result = new RetornoOperacion("No se puede recuperar el Emisor de la Factura");
						}

						//Validando que se inserto la Factura
						if (result.OperacionExitosa)
						{
							//Obteniendo Factura
							idFactura = result.IdRegistro;

							//Obteniendo Conceptos
							IEnumerable<XElement> conceptos = factura_xml.Root.Element(ns + "Conceptos").Elements();
							//Declarando Variables Auxiliares
							decimal tasa_imp_ret, tasa_imp_tras;

							//Si existen los Conceptos
							if (conceptos != null)
							{
								//Recorriendo Conceptos
								foreach (XElement concepto in conceptos)
								{
									//Obteniendo Cantidades del Concepto
									obtieneCantidadesConcepto33(factura_xml, concepto, out tasa_imp_tras, out tasa_imp_ret);

									//Insertando Cocepto de Factura
									result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(concepto.Attribute("Cantidad").Value),
																					concepto.Attribute("Unidad") == null ? "" : concepto.Attribute("Unidad").Value, concepto.Attribute("NoIdentificacion") == null ? "" : concepto.Attribute("NoIdentificacion").Value,
																					concepto.Attribute("Descripcion").Value, 0, Convert.ToDecimal(concepto.Attribute("ValorUnitario") == null ? "1" : concepto.Attribute("ValorUnitario").Value),
																					Convert.ToDecimal(concepto.Attribute("Importe") == null ? "1" : concepto.Attribute("Importe").Value),
																					Convert.ToDecimal(concepto.Attribute("Importe") == null ? "1" : concepto.Attribute("Importe").Value) * monto_tc,
																					tasa_imp_ret, tasa_imp_tras, id_usuario);

									//Si algo no salio bien
									if (!result.OperacionExitosa)
										//Terminando Ciclo
										break;
								}
							}
							else
								//Instanciando Excepción
								result = new RetornoOperacion("No existen conceptos registrados");

							//Validando resultado
							if (result.OperacionExitosa)
							{
								//Declarando Variables Auxiliares
								string ruta;
								//Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
								ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + nombre_archivo);
								//Insertamos Registro
								result = ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + nombre_archivo.Replace(".xml", " ").Trim(), id_usuario,
										Encoding.UTF8.GetBytes(factura_xml.ToString()), ruta);
							}
						}

						//Validando resultado
						if (result.OperacionExitosa)
						{
							//Instanciando Id de Factura
							result = new RetornoOperacion(idRecepcion);

							//Completando Transacción
							scope.Complete();
						}
					}
				}
			}
			else
				//Instanciando Excepción
				result = new RetornoOperacion("El CFDI no corresponde a la Versión especificada");

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Validar los Requisitos de la Factura en Formato XML
		/// </summary>
		/// <param name="idCompania">Compania Receptora</param>
		/// <param name="idTipoServicio">Tipo de Servicio del Proveedor</param>
		/// <param name="factura">Archivo XML de la Factura</param>
		/// <param name="idUsuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		private static RetornoOperacion validaFacturaXML33(int idCompania, int idTipoServicio, XDocument factura, int idUsuario)
		{
			RetornoOperacion resultado = new RetornoOperacion();

			if (factura != null)
			{
				XNamespace nsCFDI = factura.Root.GetNamespaceOfPrefix("cfdi");

				using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					using (CompaniaEmisorReceptor emisor = CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(factura.Root.Element(nsCFDI + "Emisor").Attribute("Rfc").Value.ToUpper(), idCompania))
					{
						int idProveedorEmisor = 0;
						//Validando que exista la Compania
						if (emisor.id_compania_emisor_receptor > 0)
						{
							idProveedorEmisor = emisor.id_compania_emisor_receptor;
							resultado = new RetornoOperacion(idProveedorEmisor);
						}
						else
						{
							//Insertando Compania
							resultado = CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto("", factura.Root.Element(nsCFDI + "Emisor").Attribute("Rfc").Value.ToUpper(), factura.Root.Element(nsCFDI + "Emisor").Attribute("Nombre").Value.ToUpper(), "", 0, false, false, true, idTipoServicio, "", "", "", 0, 0, idCompania, 0, "FACTURAS DE PROVEEDOR", "", 0, 0, idUsuario);
							//Validando que la Inserción haya sido Exitosa
							if (resultado.OperacionExitosa)
							{
								idProveedorEmisor = resultado.IdRegistro;
								//Instanciando Proveedor
								using (CompaniaEmisorReceptor proveedor = new CompaniaEmisorReceptor(idProveedorEmisor))
								{
									//Validando que Existe el Registro
									if (proveedor.habilitar)
									{
										//Instanciando Tipo de Servicio
										using (ProveedorTipoServicio tipoServicio = new ProveedorTipoServicio(proveedor.id_tipo_servicio))
										{
											//Si existe el tipo de servcio
											if (tipoServicio.id_proveedor_tipo_servicio > 0)
												//Inserta Asignación
												resultado = AsignacionTipoServicio.InsertarAsignacionTipoServicio(idProveedorEmisor, tipoServicio.id_proveedor_tipo_servicio, idUsuario);
											else
												resultado = new RetornoOperacion(proveedor.id_compania_emisor_receptor);

											if (resultado.OperacionExitosa)
												resultado = new RetornoOperacion(proveedor.id_compania_emisor_receptor);
										}
									}
								}
							}
						}

						//Validando que el RFC sea igual
						if (idProveedorEmisor > 0)
						{
							using (CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(idCompania))
							{
								//Validar RFC
								if (receptor.rfc.ToUpper().Equals(factura.Root.Element(nsCFDI + "Receptor").Attribute("Rfc").Value.ToUpper()) && receptor.habilitar)
								{
									using (EsquemasFacturacion esquemaFacturacion = new EsquemasFacturacion(factura.Root.Attribute("Version").Value))
									{
										if (esquemaFacturacion.habilitar)
										{
											bool addenda;
											//Obteniendo XSD
											string[] esquemas = EsquemasFacturacion.CargaEsquemasPadreYComplementosCFDI(esquemaFacturacion.version, idCompania, out addenda);
											//Validando que Existen Addendas
											if (factura.Root.Element(nsCFDI + "Addenda") != null)
												//Removiendo Addendas
												factura.Root.Element(nsCFDI + "Addenda").RemoveAll();
											//Obteniendo Validación
											string mensaje = "";
											resultado = new RetornoOperacion(mensaje, Xml.ValidaXMLSchema(factura.ToString(), esquemas, out mensaje));
										}
										else
											//Instanciando Proveedor
											resultado = new RetornoOperacion(idProveedorEmisor);
									}
								}
								else
									//Instanciando Excepción
									resultado = new RetornoOperacion("El RFC de la factura no coincide con el Receptor");
								//Validando que las Operaciónes se Completaron
								if (resultado.OperacionExitosa)
									transaccion.Complete();
							}
						}
					}
				}
			}
			else
				resultado = new RetornoOperacion("La Factura XML no es valida");

			return resultado;
		}
		/// <summary>
		/// Método Privado encargado de Obtener las Cantidades de la Factura
		/// </summary>
		/// <param name="factura_xml">Factura XML</param>
		/// <param name="total_p">Total en Pesos</param>
		/// <param name="subtotal_p">Subtotal en Pesos</param>
		/// <param name="descuento_p">Descuento en Pesos</param>
		/// <param name="traslado_p">Importe Trasladado en Pesos</param>
		/// <param name="retenido_p">Importe Retenido en Pesos</param>
		/// <param name="monto_tc">Monto del Tipo de Cambio</param>
		private static void obtieneCantidades33(XDocument factura_xml, out decimal total_p, out decimal subtotal_p, out decimal descuento_p, out decimal traslado_p, out decimal retenido_p, out decimal monto_tc)
		{
			//Obteniendo Espacio de Nombres
			XNamespace nsCFDI = factura_xml.Root.GetNamespaceOfPrefix("cfdi");

			//Validando si existe el Tipo de Cambio
			if (factura_xml.Root.Attribute("TipoCambio") == null)
				monto_tc = 1;
			else
				monto_tc = Convert.ToDecimal(factura_xml.Root.Attribute("TipoCambio").Value);

			//Validando que el Monto no sea Negativo
			monto_tc = monto_tc <= 0 ? 1 : monto_tc;

			//Asignando Atributo Obligatorios
			total_p = Convert.ToDecimal(factura_xml.Root.Attribute("Total").Value) * monto_tc;
			subtotal_p = Convert.ToDecimal(factura_xml.Root.Attribute("SubTotal").Value) * monto_tc;

			if (factura_xml.Root.Attribute("TipoDeComprobante").Value.Equals("P"))
				traslado_p = retenido_p = descuento_p = 0;
			else
			{
                if (factura_xml.Root.Element(nsCFDI + "Impuestos") != null)
                {
                    traslado_p = (factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosTrasladados") != null ? Convert.ToDecimal(factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosTrasladados").Value) : 0) * monto_tc;
                    retenido_p = (factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosRetenidos") != null ? Convert.ToDecimal(factura_xml.Root.Element(nsCFDI + "Impuestos").Attribute("TotalImpuestosRetenidos").Value) : 0) * monto_tc;
                    descuento_p = (factura_xml.Root.Attribute("Descuento") == null ? 0 : Convert.ToDecimal(factura_xml.Root.Attribute("Descuento").Value)) * monto_tc;
                }
                else
                    traslado_p = retenido_p = descuento_p = 0;
            }
		}
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de los Conceptos de la Factura
        /// </summary>
        /// <param name="cfdi"></param>
        /// <param name="concepto"></param>
        /// <param name="tasa_imp_tras"></param>
        /// <param name="tasa_imp_ret"></param>
        /// <param name="imp_ret"></param>
        /// <param name="imp_tras"></param>
        private static void obtieneCantidadesConcepto33(XDocument cfdi, XElement concepto, out decimal tasa_imp_tras, out decimal tasa_imp_ret)
        {
            //Obteniendo Espacio de Nombres
            XNamespace ns = cfdi.Root.GetNamespaceOfPrefix("cfdi");
            //Asignando Valores
            tasa_imp_tras = tasa_imp_ret = 0;

            //Validando IMpuestos Generales
            if (cfdi.Root.Element(ns + "Impuestos") != null)
            {
                //Validación de Retenciones
                if (cfdi.Root.Element(ns + "Impuestos").Element(ns + "Retenciones") != null)
                {
                    //Obteniendo Retenciones
                    IEnumerable<XElement> retenciones = cfdi.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").Elements();

                    //Validando que existan Nodos
                    if (retenciones != null)
                    {
                        //Recorriendo Retenciones
                        foreach (XElement retencion in retenciones)
                        {
                            //Validando que el Importe no sea "0"
                            if (Convert.ToDecimal(retencion.Attribute("Importe").Value) > 0 && Convert.ToDecimal(concepto.Attribute("Importe").Value) > 0)

                                //Calculando Tasa de Retención
                                tasa_imp_ret += (Convert.ToDecimal(retencion.Attribute("Importe").Value) / Convert.ToDecimal(concepto.Attribute("Importe").Value)) * 100;

                        }
                    }
                }

                //Validación de Traslados
                if (cfdi.Root.Element(ns + "Impuestos").Element(ns + "Traslados") != null)
                {
                    //Obteniendo Traslados
                    IEnumerable<XElement> traslados = cfdi.Root.Element(ns + "Impuestos").Element(ns + "Traslados").Elements();

                    //Validando que existan Nodos
                    if (traslados != null)
                    {
                        //Recorriendo Traslados
                        foreach (XElement traslado in traslados)
                        {
                            //Validando que el Importe no sea "0"
                            if (Convert.ToDecimal(traslado.Attribute("Importe").Value) > 0 && Convert.ToDecimal(concepto.Attribute("Importe").Value) > 0)

                                //Calculando Tasa de Traslado
                                tasa_imp_tras += (Convert.ToDecimal(traslado.Attribute("Importe").Value) / Convert.ToDecimal(concepto.Attribute("Importe").Value)) * 100;
                        }
                    }
                }
            }
        }

		public static DataTable ObtieneFacturaRelacionadaNC(string UUIDRelacionado)
		{
			DataTable dtFacturaRelacionadaNC = null;

			object[] param = { 15, 0, 0, 0, 0, "", "", UUIDRelacionado, null, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, null, 0, 0, 0, 0, 0, false, null, 0, 0, 0, 0, 0, "", 0, true, "", "" };

			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					dtFacturaRelacionadaNC = ds.Tables["Table"];
				return dtFacturaRelacionadaNC;
			}
		}

		#endregion

		#endregion
	}
}