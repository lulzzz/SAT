using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.CXP;
using System.Transactions;

namespace SAT_CL.CXP {
	/// <summary>
	/// Clase encargada de realizar las acciones sobre la tabla cxp.documento_pago_tdp
	/// </summary>
	public class DocumentoPago : Disposable {

		#region Enumeraciones
		#endregion

		#region Atributos
		/// <summary>
		/// Nombre del procedimiento almacenado que ejecuta las tareas de la entidad
		/// </summary>
		private static string _nombreSP = "cxp.sp_documento_pago_tdp";
		/// <summary>
		/// Llave primaria de la tabla
		/// </summary>
		public int idDocumentoPago { get { return _idDocumentoPago; } }
		private int _idDocumentoPago;
		/// <summary>
		/// Llave foranea a la tabla cxp.pago_facturado
		/// </summary>
		public int idPagoFacturado { get { return _idPagoFacturado; } }
		private int _idPagoFacturado;
		/// <summary>
		/// Identificador que liga el documento a la factura (facturado_proveedor)
		/// </summary>
		public string uuidDocumento { get { return _uuidDocumento; } }
		private string _uuidDocumento;
		/// <summary>
		/// Se puede registrar la serie del comprobante para control interno del contribuyente.
		/// </summary>
		public string serie { get { return _serie; } }
		private string _serie;
		/// <summary>
		/// Se puede registrar el folio del comprobante para control interno del contribuyente
		/// </summary>
		public string folio { get { return _folio; } }
		private string _folio;
		/// <summary>
		/// (Desde tabla catalogo fe33.moneda) Se debe registrar la clave de la moneda utilizada en los importes del documento relacionado
		/// </summary>
		public byte idMoneda { get { return _idMoneda; } }
		private byte _idMoneda;
		/// <summary>
		/// Es el tipo de cambio correspondiente a la moneda registrada en el documento relacionado. Este dato es requerido cuando la moneda del documento relacionado es distinta de la moneda
		/// </summary>
		public decimal tipoCambio { get { return _tipoCambio; } }
		private decimal _tipoCambio;
		/// <summary>
		/// (Desde catalogo 3195) Se debe registrar la clave PPD (Pago en parcialidades o diferido) que se registró en el campo.
		/// </summary>
		public byte metodoPago { get { return _metodoPago; } }
		private byte _metodoPago;
		/// <summary>
		/// Es el número de parcialidad que corresponde al pago. Es requerido cuando MetodoDePagoDR contiene Pago en parcialidades o diferido
		/// </summary>
		public byte numeroParcialidad { get { return _numeroParcialidad; } }
		private byte _numeroParcialidad;
		/// <summary>
		/// Es el monto del saldo insoluto de la parcialidad anterior. Es requerido cuando MetodoDePagoDR contiene Pago en parcialidades o diferido).
		/// </summary>
		public decimal importeSaldoAnterior { get { return _importeSaldoAnterior; } }
		private decimal _importeSaldoAnterior;
		/// <summary>
		/// Es el importe pagado que corresponde al documento relacionado.
		/// </summary>
		public decimal importePagado { get { return _importePagado; } }
		private decimal _importePagado;
		/// <summary>
		/// Es la diferencia entre el importe del saldo anterior y el monto del pago. Calculado mediante SQL
		/// </summary>
		public decimal importeSaldoInsoluto { get { return _importeSaldoInsoluto; } }
		private decimal _importeSaldoInsoluto;
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
		public DocumentoPago()
		{
			_idDocumentoPago = 0;
			_idPagoFacturado = 0;
			_uuidDocumento = "";
			_serie = "";
			_folio = "";
			_idMoneda = 0;
			_tipoCambio = 0M;
			_metodoPago = 0;
			_numeroParcialidad = 0;
			_importeSaldoAnterior = 0M;
			_importePagado = 0M;
			_importeSaldoInsoluto = 0M;
			_habilitar = false;
		}
		/// <summary>
		/// Constructor que envia un identificador primario para consultar un registro
		/// </summary>
		/// <param name="idDocumentoPago"></param>
		public DocumentoPago(int idDocumentoPago)
		{
			CargaAtributos(idDocumentoPago);
		}
		#endregion

		#region Destructor
		/// <summary>
		/// Destructor de la clase
		/// </summary>
		~DocumentoPago()
		{
			Dispose(false);
		}
		#endregion

		#region Métodos privados
		/// <summary>
		/// Método encargado de traer un resitro gracias al identificador primario, para construir un objeto, si devuelve información
		/// </summary>
		/// <param name="idDocumentoPago"></param>
		/// <returns></returns>
		private bool CargaAtributos(int idDocumentoPago)
		{
			bool resultado = false;
			object[] param = { 3, idDocumentoPago, 0, "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreSP, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						_idDocumentoPago = Convert.ToInt32(dr["idDocumentoPago"]);
						_idPagoFacturado = Convert.ToInt32(dr["idPagoFacturado"]);
						_uuidDocumento = Convert.ToString(dr["uuidDocumento"]);
						_serie = Convert.ToString(dr["serie"]);
						_folio = Convert.ToString(dr["folio"]);
						_idMoneda = Convert.ToByte(dr["idMoneda"]);
						_tipoCambio = Convert.ToDecimal(dr["tipoCambio"]);
						_metodoPago = Convert.ToByte(dr["metodoPago"]);
						_numeroParcialidad = Convert.ToByte(dr["numeroParcialidad"]);
						_importeSaldoAnterior = Convert.ToDecimal(dr["importeSaldoAnterior"]);
						_importePagado = Convert.ToDecimal(dr["importePagado"]);
						_importeSaldoInsoluto = Convert.ToDecimal(dr["importeSaldoInsoluto"]);
						_habilitar = Convert.ToBoolean(dr["habilitar"]);

					}
				}
			}
			return resultado;
		}
		/// <summary>
		/// Método encargado de enviar los nuevos valores de todo el registro
		/// </summary>
		/// <returns></returns>
		private RetornoOperacion _Editar(int idPagoFacturado, string uuidDocumento, string serie, string folio, byte idMoneda, decimal tipoCambio, byte metodoPago, byte numeroParcialidad, decimal importeSaldoAnterior, decimal importePagado, decimal importeSaldoInsoluto, int idUsuario, bool habilitar)
		{
			RetornoOperacion resultado = new RetornoOperacion();
			object[] param = { 2, this._idDocumentoPago, idPagoFacturado, uuidDocumento, serie, folio, idMoneda, tipoCambio, metodoPago, numeroParcialidad, importeSaldoAnterior, importePagado, importeSaldoInsoluto, idUsuario, habilitar, "", "" };
			//Editar Documento
			resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombreSP, param);

			return resultado;
		}
		#endregion

		#region Métodos públicos
		/// <summary>
		/// Método encargado de insertar un nuevo registro|
		/// </summary>
		/// <param name="idPagoFacturado"></param>
		/// <param name="uuidDocumento"></param>
		/// <param name="serie"></param>
		/// <param name="folio"></param>
		/// <param name="idMoneda"></param>
		/// <param name="tipoCambio"></param>
		/// <param name="metodoPago"></param>
		/// <param name="numeroParcialidad"></param>
		/// <param name="importeSaldoAnterior"></param>
		/// <param name="importePagado"></param>
		/// <param name="importeSaldoInsoluto"></param>
		/// <param name="idUsuario"></param>
		/// <returns></returns>
		public static RetornoOperacion Insertar(int idPagoFacturado, string uuidDocumento, string serie, string folio, byte idMoneda, decimal tipoCambio, byte metodoPago, byte numeroParcialidad, decimal importeSaldoAnterior, decimal importePagado, decimal importeSaldoInsoluto, int idUsuario)
		{
			RetornoOperacion resultado = new RetornoOperacion();

			object[] param = { 1, 0, idPagoFacturado, uuidDocumento, serie, folio, idMoneda, tipoCambio, metodoPago, numeroParcialidad, importeSaldoAnterior, importePagado, importeSaldoInsoluto, idUsuario, true, "", "" };
			resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombreSP, param);

			return resultado;
		}
		/// <summary>
		/// Método encargado de modificar los atributos del objeto/registro sin deshabilitarlo. El saldo insoluto se calcula desde SQL
		/// </summary>
		/// <param name="idPagoFacturado"></param>
		/// <param name="uuidDocumento"></param>
		/// <param name="serie"></param>
		/// <param name="folio"></param>
		/// <param name="idMoneda"></param>
		/// <param name="tipoCambio"></param>
		/// <param name="metodoPago"></param>
		/// <param name="numeroParcialidad"></param>
		/// <param name="importeSaldoAnterior"></param>
		/// <param name="importePagado"></param>
		/// <param name="idUsuario"></param>
		/// <returns></returns>
		public RetornoOperacion Editar(int idPagoFacturado, string uuidDocumento, string serie, string folio, byte idMoneda, decimal tipoCambio, byte metodoPago, byte numeroParcialidad, decimal importeSaldoAnterior, decimal importePagado, decimal importeSaldoInsoluto, int idUsuario)
		{
			return this._Editar(idPagoFacturado, uuidDocumento, serie, folio, idMoneda, tipoCambio, metodoPago, numeroParcialidad, importeSaldoAnterior, importePagado, importeSaldoInsoluto, idUsuario, this._habilitar);
		}
		/// <summary>
		/// Método encargado de deshabilitar el objeto/registro, sin modificarlo.
		/// </summary>
		/// <param name="idUsuario"></param>
		/// <returns></returns>
		public RetornoOperacion Deshabilitar(int idUsuario)
		{
			return this._Editar(_idPagoFacturado, _uuidDocumento, _serie, _folio, _idMoneda, _tipoCambio, _metodoPago, _numeroParcialidad, _importeSaldoAnterior, _importePagado, _importeSaldoInsoluto, idUsuario, false);
		}
		#endregion
	}
}
