using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.CXP{
	/// <summary>
	/// Clase encargada de realizar las acciones sobre la tabla cxp.pago_facturado_tpf
	/// </summary>
	public class PagoFacturado : Disposable{
		
		#region Enumeraciones
		/// <summary>
		/// Opciones que el SAT da para Cadena de Pago
		/// </summary>
		public enum TipoCadenaPago {
			SPEI = 1
		}

		public enum EstatusPago {
			Registrado = 1,
			DocumentosPendientes = 2,
			DocumentosCompletos = 3,
			Ligado = 4,
			Cancelado = 5
		}
		#endregion

		#region Atributos
		/// <summary>
		/// Nombre del procedimiento almacenado que ejecuta las tareas de la entidad
		/// </summary>
		private static string _nombreSP = "cxp.sp_pago_facturado_tpf";
		/// <summary>
		/// Identificador primario de la entidad
		/// </summary>
		public int idPagoFacturado { get { return _idPagoFacturado; } }
		private int _idPagoFacturado;
		/// <summary>
		/// Identificador foraneo hacia la tabla cxp.facturado_proveedor_tfp
		/// </summary>
		public int idFacturadoProveedor { get { return _idFacturadoProveedor; } }
		private int _idFacturadoProveedor;
		/// <summary>
		/// Fecha de pago del nodo Pago, incluye horas
		/// </summary>
		public DateTime fechaPago { get { return _fechaPago; } }
		private DateTime _fechaPago;
		/// <summary>
		/// Llave foranea a la tabla catalogo fe33.forma_pago_tfp (205)
		/// </summary>
		public byte idFormaPagoP { get { return _idFormaPagoP; } }
		private byte _idFormaPagoP;
		/// <summary>
		/// Llave foranea a la tabla catalogo fe33.moneda_tm (207)
		/// </summary>
		public byte idMonedaP{ get { return _idMonedaP; } }
		private byte _idMonedaP;
		/// <summary>
		/// Es el valor unitario de la moneda, cuando esta es extranjera.
		/// </summary>
		public decimal tipoCambio { get { return _tipoCambio; } }
		private decimal _tipoCambio;
		/// <summary>
		/// Fecha en la que se obtiene el valor de la moneda extranjera
		/// </summary>
		public DateTime fechaTipoCambio { get { return _fechaTipoCambio; } }
		private DateTime _fechaTipoCambio;
		/// <summary>
		/// Es la sumatoria de los ImportePagado de cada Documento Relacionado dentro de este nodo
		/// </summary>
		public decimal monto { get { return _monto; } }
		private decimal _monto;
		/// <summary>
		/// El número de cheque, número de autorización, número de referencia, clave de rastreo en caso de ser SPEI, línea de captura o algún número de referencia o identificación análogo que permita identificar la operación correspondiente al pago efectuado.
		/// </summary>
		public string numeroOperacion { get { return _numeroOperacion; } }
		private string _numeroOperacion;
		/// <summary>
		/// Registrar la clave del RFC de la entidad emisora de la cuenta origen, es decir, la operadora, el banco, la institucion financiera, emisor de monedero electronico, etc.
		/// </summary>
		public string rfcEmisorCuentaOrdenante { get { return _rfcEmisorCuentaOrdenante; } }
		private string _rfcEmisorCuentaOrdenante;
		/// <summary>
		/// (Usando tabla catalogo banco.banco_tb) Se puede registrar el nombre del banco ordenante, es requerido en caso de ser extranjero,
		/// </summary>
		public byte idBancoOrdenanteExtranjero { get { return _idBancoOrdenanteExtranjero; } }
		private byte _idBancoOrdenanteExtranjero;
		/// <summary>
		/// Se debe registrar el número de la cuenta con la que se realizó el pago
		/// </summary>
		public string cuentaOrdenante { get { return _cuentaOrdenante; } }
		private string _cuentaOrdenante;
		/// <summary>
		/// Se debe registrar la clave en el RFC de la entidad operadora de la cuenta destino, es decir, la operadora, el banco, la institución financiera, emisor de monedero electrónico, etc.
		/// </summary>
		public string rfcEmisorCuentaBeneficiario { get { return _rfcEmisorCuentaBeneficiario; } }
		private string _rfcEmisorCuentaBeneficiario;
		/// <summary>
		/// Se debe registrar el número de cuenta en donde se recibió el pago.
		/// </summary>
		public string cuentaBeneficiario { get { return _cuentaBeneficiario; } }
		private string _cuentaBeneficiario;
		/// <summary>
		/// (Usando catalogo) Se debe registrar la clave del tipo de cadena de pago que genera la entidad receptora del pago.
		/// </summary>
		public byte idTipoCadenaPago { get { return _idTipoCadenaPago; } }
		private byte _idTipoCadenaPago;
		/// <summary>
		/// Determina el identificador del estatus, segun la enumeracion
		/// </summary>
		public byte idEstatus { get { return _idEstatus; } }
		private byte _idEstatus;
		/// <summary>
		/// Determina si el registro se puede usar o no
		/// </summary>
		public bool habilitar { get { return _habilitar; } }
		private bool _habilitar;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor con valores minimos
		/// </summary>
		public PagoFacturado()
		{
			_idPagoFacturado = 0;
			_idFacturadoProveedor = 0;
			_fechaPago = DateTime.MinValue;
			_idFormaPagoP = 0;
			_idMonedaP = 0;
			_tipoCambio = 0.00M;
			_fechaTipoCambio = DateTime.MinValue;
			_monto = 0.00M;
			_numeroOperacion = "";
			_rfcEmisorCuentaOrdenante = "";
			_idBancoOrdenanteExtranjero = 0;
			_cuentaOrdenante = "";
			_rfcEmisorCuentaBeneficiario = "";
			_cuentaBeneficiario = "";
			_idTipoCadenaPago = 0;
			_idEstatus = 0;
			_habilitar = false;
		}
		/// <summary>
		/// Constructor que envia un identificador primario para consultar un registro
		/// </summary>
		/// <param name="idPagoFacturado"></param>
		public PagoFacturado(int idPagoFacturado)
		{
			CargaAtributos(idPagoFacturado);
		}
		#endregion

		#region Destructores
		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~PagoFacturado()
		{
			Dispose(false);
		}
		#endregion

		#region Métodos privados
		/// <summary>
		/// Método encargado de traer un resitro gracias al identificador primario, para construir un objeto, si devuelve información
		/// </summary>
		/// <param name="idPagoFacturado">Identificador primario</param>
		/// <returns></returns>
		private bool CargaAtributos(int idPagoFacturado)
		{
			bool resultado = false;
			object[] param = { 3, idPagoFacturado, 0, null, 0, 0, 0M, null, 0M, "", "", 0, "", "", "", 0, 0, 0, false, "", "" };
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreSP, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					foreach(DataRow dr in ds.Tables["Table"].Rows)
					{
						_idPagoFacturado = idPagoFacturado;
						_idFacturadoProveedor = Convert.ToInt32(dr["idFacturadoProveedor"]);
						DateTime.TryParse(dr["fechaPago"].ToString(), out _fechaPago);
						_idFormaPagoP = Convert.ToByte(dr["idFormaPagoP"]);
						_idMonedaP = Convert.ToByte(dr["idMonedaP"]);
						_tipoCambio = Convert.ToInt32(dr["tipoCambio"]);
						DateTime.TryParse(dr["fechaTipoCambio"].ToString(), out _fechaTipoCambio);
						_monto = Convert.ToDecimal(dr["monto"]);
						_numeroOperacion = Convert.ToString(dr["numeroOperacion"]);
						_rfcEmisorCuentaOrdenante = Convert.ToString(dr["rfcEmisorCuentaOrdenante"]);
						_idBancoOrdenanteExtranjero = Convert.ToByte(dr["idBancoOrdenanteExtranjero"]);
						_cuentaOrdenante = Convert.ToString(dr["cuentaOrdenante"]);
						_rfcEmisorCuentaBeneficiario = Convert.ToString(dr["rfcEmisorCuentaBeneficiario"]);
						_cuentaBeneficiario = Convert.ToString(dr["cuentaBeneficiario"]);
						_idTipoCadenaPago = Convert.ToByte(dr["idTipoCadenaPago"]);
						_idEstatus = Convert.ToByte(dr["idEstatus"]);
						_habilitar = Convert.ToBoolean(dr["habilitar"]);
					}
					resultado = true;
				}
			}
			return resultado;
		}
		/// <summary>
		/// Método encargado de enviar los nuevos valores de todo el registro.
		/// </summary>
		/// <param name="idFacturadoProveedor"></param>
		/// <param name="fechaPago"></param>
		/// <param name="idFormaPagoP"></param>
		/// <param name="idMonedaP"></param>
		/// <param name="tipoCambio"></param>
		/// <param name="fechaTipoCambio"></param>
		/// <param name="monto"></param>
		/// <param name="numeroOperacion"></param>
		/// <param name="rfcEmisorCuentaOrdenante"></param>
		/// <param name="idBancoOrdenanteExtranjero"></param>
		/// <param name="cuentaOrdenante"></param>
		/// <param name="rfcEmisorCuentaBeneficiario"></param>
		/// <param name="cuentaBeneficiario"></param>
		/// <param name="idTipoCadenaPago"></param>
		/// <param name="id_usuario"></param>
		/// <param name="habilitar"></param>
		/// <returns></returns>
		private RetornoOperacion _Editar(int idFacturadoProveedor, DateTime fechaPago, byte idFormaPagoP, byte idMonedaP, decimal tipoCambio, DateTime fechaTipoCambio, decimal monto, string numeroOperacion, string rfcEmisorCuentaOrdenante, byte idBancoOrdenanteExtranjero, string cuentaOrdenante, string rfcEmisorCuentaBeneficiario, string cuentaBeneficiario, byte idTipoCadenaPago, EstatusPago estatusPago, int id_usuario, bool habilitar)
		{
			RetornoOperacion resultado = new RetornoOperacion();

			object[] param = { 2, this._idPagoFacturado, idFacturadoProveedor, fechaPago, idFormaPagoP, idMonedaP, tipoCambio, fechaTipoCambio, monto, numeroOperacion, rfcEmisorCuentaOrdenante, idBancoOrdenanteExtranjero, cuentaOrdenante, rfcEmisorCuentaBeneficiario, cuentaBeneficiario, idTipoCadenaPago, (byte)estatusPago, id_usuario, habilitar, "", "" };
			resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombreSP, param);

			return resultado;
		}
		#endregion

		#region Métodos públicos
		/// <summary>
		/// Método encargado de insertar un nuevo registro. Se puede no enviar datos de índole extranjero
		/// </summary>
		/// <param name="idFacturadoProveedor"></param>
		/// <param name="fechaPago"></param>
		/// <param name="idFormaPagoP"></param>
		/// <param name="idMonedaP"></param>
		/// <param name="tipoCambio"></param>
		/// <param name="fechaTipoCambio"></param>
		/// <param name="monto"></param>
		/// <param name="numeroOperacion"></param>
		/// <param name="rfcEmisorCuentaOrdenante"></param>
		/// <param name="idBancoOrdenanteExtranjero"></param>
		/// <param name="cuentaOrdenante"></param>
		/// <param name="rfcEmisorCuentaBeneficiario"></param>
		/// <param name="cuentaBeneficiario"></param>
		/// <param name="idTipoCadenaPago"></param>
		/// <returns></returns>
		public static RetornoOperacion Insertar(int idFacturadoProveedor, DateTime fechaPago, byte idFormaPagoP, byte idMonedaP, decimal monto, string numeroOperacion, string rfcEmisorCuentaOrdenante,string cuentaOrdenante,string rfcEmisorCuentaBeneficiario, string cuentaBeneficiario, byte idTipoCadenaPago, decimal tipoCambio, DateTime fechaTipoCambio, byte idBancoOrdenanteExtranjero, int id_usuario)
		{
			RetornoOperacion resultado = new RetornoOperacion();

			object[] param = { 1, 0, idFacturadoProveedor, fechaPago, idFormaPagoP, idMonedaP, tipoCambio, Fecha.ConvierteDateTimeObjeto(fechaTipoCambio), monto, numeroOperacion, rfcEmisorCuentaOrdenante, idBancoOrdenanteExtranjero, cuentaOrdenante, rfcEmisorCuentaBeneficiario, cuentaBeneficiario, idTipoCadenaPago, (byte)EstatusPago.Registrado, id_usuario, true, "", "" };
			resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombreSP, param);

			return resultado;
		}
		
		/// <summary>
		/// Método encargado de modificarlo los atributos del objeto/registro sin deshabilitarlo
		/// </summary>
		/// <param name="idFacturadoProveedor"></param>
		/// <param name="fechaPago"></param>
		/// <param name="idFormaPagoP"></param>
		/// <param name="idMonedaP"></param>
		/// <param name="tipoCambio"></param>
		/// <param name="fechaTipoCambio"></param>
		/// <param name="monto"></param>
		/// <param name="numeroOperacion"></param>
		/// <param name="rfcEmisorCuentaOrdenante"></param>
		/// <param name="idBancoOrdenanteExtranjero"></param>
		/// <param name="cuentaOrdenante"></param>
		/// <param name="rfcEmisorCuentaBeneficiario"></param>
		/// <param name="cuentaBeneficiario"></param>
		/// <param name="idTipoCadenaPago"></param>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion Editar(int idFacturadoProveedor, DateTime fechaPago, byte idFormaPagoP, byte idMonedaP, decimal tipoCambio, DateTime fechaTipoCambio, decimal monto, string numeroOperacion, string rfcEmisorCuentaOrdenante, byte idBancoOrdenanteExtranjero, string cuentaOrdenante, string rfcEmisorCuentaBeneficiario, string cuentaBeneficiario, byte idTipoCadenaPago, EstatusPago estatusPago, int id_usuario)
		{
			return _Editar(idFacturadoProveedor, fechaPago, idFormaPagoP, idMonedaP, tipoCambio, fechaTipoCambio, monto, numeroOperacion, rfcEmisorCuentaOrdenante, idBancoOrdenanteExtranjero, cuentaOrdenante, rfcEmisorCuentaBeneficiario, cuentaBeneficiario, idTipoCadenaPago, estatusPago, id_usuario, this._habilitar);
		}

		/// <summary>
		/// Método encargado de deshabiltar el objeto/registro, sin modificarlo
		/// </summary>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion Deshabiltiar(int id_usuario)
		{
			return _Editar(this._idFacturadoProveedor, this._fechaPago, this._idFormaPagoP, this._idMonedaP, this._tipoCambio, this._fechaTipoCambio, this._monto, this._numeroOperacion, this._rfcEmisorCuentaOrdenante, this._idBancoOrdenanteExtranjero, this._cuentaOrdenante, this._rfcEmisorCuentaBeneficiario, this._cuentaBeneficiario, this._idTipoCadenaPago, (EstatusPago)_idEstatus, id_usuario, false);
		}

		/// <summary>
		/// Recupera los elementos FI que se encuentran disponibles para agregar a un CFDI de Recepción de Pagos
		/// </summary>
		/// <param name="idCompania">Id de Compañía que emitirá el CFDI</param>
		/// <param name="id_cliente">Id de Cliente que realizó el pago</param>
		/// <returns></returns>
		public static DataTable ObtieneDocumentos(int idPago)
		{
			DataTable dtPagos = null;

			object[] param = { 4, idPago, 0, null, 0, 0, 0M, null, 0M, "", "", 0, "", "", "", 0, 0, 0, false, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreSP, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					dtPagos = DS.Tables["Table"];

			return dtPagos;
		}

		/// <summary>
		/// Método encargado de revisar si el Pago tiene todos sus Documentos Relacionados dados de alta en el sistema y cambiar en automático el estatus.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <returns></returns>
		public RetornoOperacion ActualizaEstatusAutomatico(int idCompania, int idUsuario)
		{
			RetornoOperacion resultado = new RetornoOperacion();
			byte nuevoEstatus = (byte)EstatusPago.DocumentosCompletos;
			using(DataTable dtDocumentos = ObtieneDocumentos(this._idPagoFacturado))
			{
				if (Validacion.ValidaOrigenDatos(dtDocumentos))
				{
					foreach (DataRow rowDocumento in dtDocumentos.Rows)
					{
						using (FacturadoProveedor factura = new FacturadoProveedor(rowDocumento["UUIDTDP"].ToString(), idCompania))
						{
							if (!factura.habilitar) //Si falta alguna factura
							{
								nuevoEstatus = (byte)EstatusPago.DocumentosPendientes;
								break; //Dejar de revisar
							}
						}
					}
					resultado = _Editar(_idFacturadoProveedor, _fechaPago, _idFormaPagoP, _idMonedaP, _tipoCambio, _fechaTipoCambio, _monto, _numeroOperacion, _rfcEmisorCuentaBeneficiario, _idBancoOrdenanteExtranjero, _cuentaOrdenante, _rfcEmisorCuentaBeneficiario, _cuentaBeneficiario, _idTipoCadenaPago, (EstatusPago)nuevoEstatus, idUsuario, this._habilitar);
				}
				else
				{
					resultado = new RetornoOperacion("No se encontraron Documentos Relacionados en el Pago.", false);
				}
			}
			return resultado;
		}

		public RetornoOperacion ActualizaEstatus (EstatusPago nuevoEstatus, int idUsuario)
		{
			return _Editar(_idFacturadoProveedor, _fechaPago, _idFormaPagoP, _idMonedaP, _tipoCambio, _fechaTipoCambio, _monto, _numeroOperacion, _rfcEmisorCuentaOrdenante, _idBancoOrdenanteExtranjero, _cuentaOrdenante, _rfcEmisorCuentaBeneficiario, _cuentaBeneficiario, _idTipoCadenaPago, nuevoEstatus, idUsuario, _habilitar);
		}
		#endregion
	}
}
