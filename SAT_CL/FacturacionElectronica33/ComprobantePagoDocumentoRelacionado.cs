using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Transactions;

namespace SAT_CL.FacturacionElectronica33 {
	/// <summary>
	/// Clase encargada de las operaciones relacionadas con ComprobantePagoDocumentoRelacionado_tcrd
	/// </summary>
	public class ComprobantePagoDocumentoRelacionado : Disposable {
		#region Atributos
		/// <summary>
		/// Nombre del método que realiza las funciones de la tabla.
		/// </summary>
		private static string _nom_sp = "fe33.sp_comprobante_pago_documento_relacionado_tcpd";

		/// <summary>
		/// Llave primaria de la tabla
		/// </summary>
		private int _id_comprobante_pago_documento_relacionado;
		public int id_comprobante_pago_documento_relacionado { get { return this._id_comprobante_pago_documento_relacionado; } }
		/// <summary>
		/// Identificador de la operación realizada
		/// </summary>
		private byte _id_tipo_operacion;
		public byte id_tipo_operacion { get { return this._id_tipo_operacion; } }
		/// <summary>
		/// Obtiene el tipo de operación del CFDI de Pago
		/// </summary>
		public TipoOperacion tipo_operacion { get { return (TipoOperacion)this._id_tipo_operacion; } }
		/// <summary>
		/// Identificador del comprobante de pago
		/// </summary>
		private int _id_comprobante_pago;
		public int id_comprobante_pago { get { return this._id_comprobante_pago; } }
		/// <summary>
		/// Identificador del documento del tipo de operación
		/// </summary>
		private int _id_tipo_operacion_doc;
		public int id_tipo_operacion_doc { get { return _id_tipo_operacion_doc; } }
		/// <summary>
		/// Obtiene el tipo de operación del CFDI (Documento Relacionado)
		/// </summary>
		public TipoOperacionDocumento tipo_operacion_documento { get { return (TipoOperacionDocumento)this._id_tipo_operacion_doc; } }
		/// <summary>
		/// Identificador del documento adjunto al movimiento
		/// </summary>
		private int _id_documento_relacionado;
		public int id_documento_relacionado { get { return this._id_documento_relacionado; } }
		/// <summary>
		/// Identificador del registro de egreso o ingreso
		/// </summary>
		private int _id_egreso_ingreso;
		public int id_egreso_ingreso { get { return this._id_egreso_ingreso; } }
		/// <summary>
		/// Identificador de la aplicación
		/// </summary>
		private int _id_aplicacion;
		public int id_aplicacion { get { return this._id_aplicacion; } }
		/// <summary>
		/// Saldo actual del adeudo
		/// </summary>
		private decimal _saldo_anterior;
		public decimal saldo_anterior { get { return this._saldo_anterior; } }
		/// <summary>
		/// Monto a pagar sobre el saldo actual
		/// </summary>
		private decimal _monto_pago;
		public decimal monto_pago { get { return this._monto_pago; } }
		/// <summary>
		/// Nuevo saldo actual, despues de realizar el pago
		/// </summary>
		private decimal _saldo_insoluto;
		public decimal saldo_insoluto { get { return this._saldo_insoluto; } }
		/// <summary>
		/// Número de parcialidad que representa ese pago
		/// </summary>
		private byte _no_parcialidad;
		public byte no_parcialidad { get { return this._no_parcialidad; } }
		/// <summary>
		/// Identificador de comprobante de egreso o ingreso
		/// </summary>
		private int _id_egreso_ingreso_comprobante;
		public int id_egreso_ingreso_comprobante { get { return this._id_egreso_ingreso_comprobante; } }
		/// <summary>
		/// Último usuario en modificar el registro
		/// </summary>
		private int _id_usuario;
		public int id_usuario { get { return this._id_usuario; } }
		/// <summary>
		/// Indica si el registro puede editarse o leerse
		/// </summary>
		private bool _habilitar;
		public bool habilitar { get { return this._habilitar; } }
		#endregion

		#region Enumeraciones

		/// <summary>
		/// Enumera las posibles operaciones del Comprobante de Recepción de Pago (Origen del CFDI)
		/// </summary>
		public enum TipoOperacion {
			/// <summary>
			/// CFDI emitido a un Cliente
			/// </summary>
			Ingreso = 1,
			/// <summary>
			/// CFDI emitido por un Proveedor
			/// </summary>
			Egreso = 2,
		}
		/// <summary>
		/// Enumera las posibles operaciones del Documento Relacionado (Origen del CFDI - Documento Relacionado)
		/// </summary>
		public enum TipoOperacionDocumento {
			/// <summary>
			/// CFDI emitido a un Cliente
			/// </summary>
			Ingreso = 1,
			/// <summary>
			/// CFDI emitido por un Proveedor
			/// </summary>
			Egreso = 2,
		}

		#endregion

		#region Costructores
		/// <summary>
		/// Constructor encargado de Inicializar los atributos en su valor mínimo
		/// </summary>
		public ComprobantePagoDocumentoRelacionado()
		{
			//Asignando valores
			this._id_comprobante_pago_documento_relacionado = 0;
			this._id_tipo_operacion = 0;
			this._id_comprobante_pago = 0;
			this._id_tipo_operacion_doc = 0;
			this._id_documento_relacionado = 0;
			this._id_egreso_ingreso = 0;
			this._id_aplicacion = 0;
			this._saldo_anterior = 0.0M;
			this._monto_pago = 0.0M;
			this._saldo_insoluto = 0.0M;
			this._no_parcialidad = 0;
			this._id_egreso_ingreso_comprobante = 0;
			this._id_usuario = 0;
			this._habilitar = false;
		}
		/// <summary>
		/// Constructor encargado de inicar los atributos dado una PK
		/// </summary>
		/// <param name="id_comprobante_relacionado"></param>
		public ComprobantePagoDocumentoRelacionado(int id_comprobante_relacionado)
		{
			cargaAtributosInstancia(id_comprobante_relacionado);
		}
		#endregion

		#region Destructores
		/// <summary>
		/// Destructor de la clase
		/// </summary>
		~ComprobantePagoDocumentoRelacionado()
		{
			Dispose(false);
		}
		#endregion

		#region Métodos Privados
		/// <summary>
		/// Método encargado realizar una búsqueda y dar valores a los atributos
		/// </summary>
		/// <param name="id_comprobante_relacionado"></param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_comprobante_relacionado)
		{
			//Declarar objeto a devolver
			bool retorno = false;

			//Armando arreglo de parámetros
			object[] param = {
																 3, // Tipo
                                 id_comprobante_relacionado,// IdComprobantePagoDocumentoRelacionado
                                 0,// IdTipoOperacion
                                 0,// IdComprobantePago
                                 0,// IdTipoOperacionDoc
                                 0,// IdDocumentoRelacionado
                                 0,// IdEgresoIngreso
                                 0,// IdAplicacion
                                 0.0M,// SaldoAnterior
                                 0.0M,// MontoPago
                                 0.0M,// SaldoInsoluto
                                 0,// NoParcialidad
                                 0,// IdEgresoIngresoComprobante
                                 0,// IdUsuario
                                 false,// Habilitar
                                 "",// Param1
                                 ""// Param2
                             };

			//Obteniendo resultado del SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorrer registro
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Asignar valores
						this._id_comprobante_pago_documento_relacionado = id_comprobante_relacionado;
						this._id_tipo_operacion = Convert.ToByte(dr["IdTipoOperacion"]);
						this._id_comprobante_pago = Convert.ToInt32(dr["IdComprobantePago"]);
						this._id_tipo_operacion_doc = Convert.ToInt32(dr["IdTipoOperacionDoc"]);
						this._id_documento_relacionado = Convert.ToInt32(dr["IdDocumentoRelacionado"]);
						this._id_egreso_ingreso = Convert.ToInt32(dr["IdEgresoIngreso"]);
						this._id_aplicacion = Convert.ToInt32(dr["IdAplicacion"]);
						this._saldo_anterior = Convert.ToDecimal(dr["SaldoAnterior"]);
						this._monto_pago = Convert.ToDecimal(dr["MontoPago"]);
						this._saldo_insoluto = Convert.ToDecimal(dr["SaldoInsoluto"]);
						this._no_parcialidad = Convert.ToByte(dr["NoParcialidad"]);
						this._id_egreso_ingreso_comprobante = Convert.ToInt32(dr["IdEgresoIngresoComprobante"]);
						this._id_usuario = Convert.ToInt32(dr["IdUsuario"]);
						this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
						//Terminar ciclo
						break;
					}
				}
			}

			//Devolver retorno
			return retorno;
		}
		/// <summary>
		/// Método encargado de editar TODOS los campos de la tabla en un registro
		/// </summary>
		/// <param name="tipo_operacion"></param>
		/// <param name="id_comprobante_pago"></param>
		/// <param name="id_tipo_operacion_doc"></param>
		/// <param name="id_documento_relacionado"></param>
		/// <param name="id_egreso_ingreso"></param>
		/// <param name="id_aplicacion"></param>
		/// <param name="saldo_anterior"></param>
		/// <param name="monto_pago"></param>
		/// <param name="no_parcialidad"></param>
		/// <param name="id_egreso_ingreso_comprobante"></param>
		/// <param name="id_usuario"></param>
		/// <param name="habilitar"></param>
		/// <returns></returns>
		private RetornoOperacion editaComprobantePagoDocumentoRelacionado(byte tipo_operacion, int id_comprobante_pago, int id_tipo_operacion_doc, int id_documento_relacionado, int id_egreso_ingreso, int id_aplicacion, decimal saldo_anterior, decimal monto_pago, byte no_parcialidad, int id_egreso_ingreso_comprobante, int id_usuario, bool habilitar)
		{
			//Crear objeto retorno
			RetornoOperacion retorno = new RetornoOperacion();

			//Crear arreglo de parámetros
			object[] param = {
																 2, // Tipo
                                 this._id_comprobante_pago_documento_relacionado,// IdComprobantePagoDocumentoRelacionado
                                 tipo_operacion,// IdTipoOperacion
                                 id_comprobante_pago,// IdComprobantePago
                                 id_tipo_operacion_doc,// IdTipoOperacionDoc
                                 id_documento_relacionado,// IdDocumentoRelacionado
                                 id_egreso_ingreso,// IdEgresoIngreso
                                 id_aplicacion,// IdAplicacion
                                 saldo_anterior,// SaldoAnterior
                                 monto_pago,// MontoPago
                                 0,// SaldoInsoluto (calculada)
                                 no_parcialidad,// NoParcialidad
                                 id_egreso_ingreso_comprobante,// IdEgresoIngresoComprobante
                                 id_usuario,// IdUsuario
                                 habilitar,// Habilitar
                                 "",// Param1
                                 ""// Param2
                             };

			//Ejecutar SP
			retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			//Devolver retorno
			return retorno;
		}
		#endregion

		#region Métodos Públicos
		/// <summary>
		/// Método encargado de insertar un nuevo registo en la tabla
		/// </summary>
		/// <param name="tipo_operacion"></param>
		/// <param name="id_comprobante_pago"></param>
		/// <param name="tipo_operacion_doc"></param>
		/// <param name="id_documento_relacionado"></param>
		/// <param name="id_egreso_ingreso"></param>
		/// <param name="id_aplicacion"></param>
		/// <param name="saldo_anterior"></param>
		/// <param name="monto_pago"></param>
		/// <param name="no_parcialidad"></param>
		/// <param name="id_egreso_ingreso_comprobante"></param>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public static RetornoOperacion InsertarComprobantePagoDocumentoRelacionado(TipoOperacion tipo_operacion, int id_comprobante_pago, TipoOperacionDocumento tipo_operacion_doc, int id_documento_relacionado, int id_egreso_ingreso, int id_aplicacion, decimal saldo_anterior, decimal monto_pago, byte no_parcialidad, int id_egreso_ingreso_comprobante, int id_usuario)
		{
			//Crear objeto retorno
			RetornoOperacion retorno = new RetornoOperacion();
			//Armando arreglo de parámetros
			object[] param = {
				1, // Tipo
				0,// IdComprobantePagoDocumentoRelacionado
				(byte)tipo_operacion,// IdTipoOperacion
				id_comprobante_pago,// IdComprobantePago
				(byte)tipo_operacion_doc,// IdTipoOperacionDoc
				id_documento_relacionado,// IdDocumentoRelacionado
				id_egreso_ingreso,// IdEgresoIngreso
				id_aplicacion,// IdAplicacion
				saldo_anterior,// SaldoAnterior
				monto_pago,// MontoPago
				0,// SaldoInsoluto (calculada)
				no_parcialidad,// NoParcialidad
				id_egreso_ingreso_comprobante,// IdEgresoIngresoComprobante
				id_usuario,// IdUsuario
				true,// Habilitar
				"",// Param1
				""// Param2
      };
			//Ejecutar SP
			retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
			//Devolver retorno
			return retorno;
		}

		/// <summary>
		/// Método encargado de editar los atributos de un registro, excepto habilitar.
		/// </summary>
		/// <param name="tipo_operacion"></param>
		/// <param name="id_comprobante_pago"></param
		/// <param name="tipo_operacion_doc"></param>
		/// <param name="id_documento_relacionado"></param>
		/// <param name="id_egreso_ingreso"></param>
		/// <param name="id_aplicacion"></param>
		/// <param name="saldo_anterior"></param>
		/// <param name="monto_pago"></param>
		/// <param name="no_parcialidad"></param>
		/// <param name="id_egreso_ingreso_comprobante"></param>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion EditaComprobantePagoDocumentoRelacionado(TipoOperacion tipo_operacion, int id_comprobante_pago, TipoOperacionDocumento tipo_operacion_doc, int id_documento_relacionado, int id_egreso_ingreso, int id_aplicacion, decimal saldo_anterior, decimal monto_pago, byte no_parcialidad, int id_egreso_ingreso_comprobante, int id_usuario)
		{
			return this.editaComprobantePagoDocumentoRelacionado((byte)tipo_operacion, id_comprobante_pago, (byte)tipo_operacion_doc, id_documento_relacionado, id_egreso_ingreso, id_aplicacion, saldo_anterior, monto_pago, no_parcialidad, id_egreso_ingreso_comprobante, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método encargado de modificar los atributos IdUsuario y Habilitar de un registro, manteniendo el resto.
		/// </summary>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaComprobantePagoDocumentoRelacionado(int id_usuario)
		{
			return this.editaComprobantePagoDocumentoRelacionado(this._id_tipo_operacion, this._id_comprobante_pago, this._id_tipo_operacion_doc, this._id_documento_relacionado, this._id_egreso_ingreso, this._id_aplicacion, this._saldo_anterior, this._monto_pago, this._no_parcialidad, this._id_egreso_ingreso_comprobante, id_usuario, false);
		}
		/// <summary>
		/// Método encargado de Validar si el Comprobante (Doc. Rel.) tiene alguna relación con un Comprobante de Pago (CxC)
		/// </summary>
		/// <param name="id_documento_relacionado">CFDI Relacionado</param>
		/// <returns></returns>
		public static bool ValidaComprobantePagoCxC(int id_documento_relacionado)
		{
			//Declarando Objeto de Retorno
			bool result = false;

			//Armando arreglo de parámetros
			object[] param = {   4, // Tipo
                                 0,// IdComprobantePagoDocumentoRelacionado
                                 0,// IdTipoOperacion
                                 0,// IdComprobantePago
                                 (byte)TipoOperacionDocumento.Ingreso,// IdTipoOperacionDoc
                                 id_documento_relacionado,// IdDocumentoRelacionado
                                 0,// IdEgresoIngreso
                                 0,// IdAplicacion
                                 0.00M,// SaldoAnterior
                                 0.00M,// MontoPago
                                 0,// SaldoInsoluto (calculada)
                                 0,// NoParcialidad
                                 0,// IdEgresoIngresoComprobante
                                 0,// IdUsuario
                                 false,// Habilitar
                                 "",// Param1
                                 ""// Param2 
                             };

			//Obteniendo resultado del SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorriendo Registros
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Validando Existencia
						result = Convert.ToInt32(dr["Id"]) > 0 ? true : false;
						//Terminando Ciclo
						break;
					}
				}
			}

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Deshabilita todos los documentos relacionados a una asociación egreso-ingreso-cfdi de pago
		/// </summary>
		/// <param name="id_egreso_ingreso_comprobante">Id de Egreso-Ingreso-Pago</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion DeshabilitarDocumentosRelacionadosIngresoEgresoComprobantePago(int id_egreso_ingreso_comprobante, int id_usuario)
		{
			//Declarando objeto de retorno
			RetornoOperacion resultado = new RetornoOperacion();

			//Creando arreglo de parámetros para deshabilitación
			object[] param = { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_egreso_ingreso_comprobante, id_usuario, 0, "", "" };

			//Realizando transacción
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Realizando actualización de los registros
				using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
					//Validando resultados
					resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);

				//Si no hay errores
				if (resultado.OperacionExitosa)
					//Confirmando cambios realizados
					scope.Complete();
			}

			//Devolviendo resultado
			return resultado;
		}

		/// <summary>
		/// Determina si es posible eliminar una aplicación de pago de Cliente, en base a su relación con un CFDI de Recepción de Pagos Activo
		/// </summary>
		/// <param name="id_tabla_origen_aplicacion"></param>
		/// <param name="id_aplicacion_pago"></param>
		/// <returns></returns>
		public static RetornoOperacion ValidarAplicacionEnCFDIRecepcionPagoActivo(int id_tabla_origen_aplicacion, int id_aplicacion_pago)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion(id_aplicacion_pago);

			//Creando arreglo de parámetros para deshabilitación
			object[] param = { 6, 0, Convert.ToByte(id_tabla_origen_aplicacion == 9 ? TipoOperacion.Ingreso : TipoOperacion.Egreso), 0, 0, 0, 0, id_aplicacion_pago, 0, 0, 0, 0, 0, 0, 0, "", "" };

			//Realizando búsqueda de los registros que impidan elimiar Aplicación de Pago
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando resultados
				if (Validacion.ValidaOrigenDatos(ds, true, true))
					//Si hay resultados
					resultado = new RetornoOperacion("Existe una Relación Activa a un CFDI de Recepción de Pagos Vigente.");
			}

			//Devolviendo resultado
			return resultado;
		}
		/// <summary>
		/// Método encargado de Obtener los Comprobantes de un Pago
		/// </summary>
		/// <param name="id_pago"></param>
        /// <param name="id_cfdi"></param>
		/// <returns></returns>
		public static DataTable ObtieneComprobantesPago(int id_pago, int id_cfdi)
		{
			//Declarando Variable de Retorno
			DataTable dtPagos = null;

			//Creando arreglo de parámetros para deshabilitación
			object[] param = { 7, 0, 0, id_cfdi, 0, 0, id_pago, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

			//Realizando búsqueda de los registros que impidan elimiar Aplicación de Pago
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando resultados
				if (Validacion.ValidaOrigenDatos(ds, "Table"))

					//Asignando Valores
					dtPagos = ds.Tables["Table"];
			}

			//Devolviendo Resultado Obtenido
			return dtPagos;
		}

		#endregion
	}
}
