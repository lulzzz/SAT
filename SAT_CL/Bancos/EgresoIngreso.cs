using System;
using System.Configuration;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Despacho;
using System.Collections.Generic;
using fe33 = SAT_CL.FacturacionElectronica33;
using System.Xml.Linq;
using System.Linq;

namespace SAT_CL.Bancos {
	/// <summary>
	/// Clase de la tabla egreso_ingreso que permite crear operaciones sobre la tabla (inserciones,actualizaciones,consultas,etc.).
	/// </summary>
	public class EgresoIngreso : Disposable {
		#region Enumeracion

		/// <summary>
		/// Permite enumerar los métodos de pago de un egreso-ingreso
		/// </summary>
		public enum FormaPago {
			/// <summary>
			/// Método de pago mediante cheques
			/// </summary>
			Cheque = 1,

			/// <summary>
			/// Métodos de pago en efectivo
			/// </summary>
			Efectivo = 2,

			/// <summary>
			/// Método de pago mediante transferencia bancaria
			/// </summary>
			Transferida = 3,

			/// <summary>
			/// Cualquier tipo de documento que tiene un valor monetario
			/// </summary> 
			Documento = 4,

		};

		/// <summary>
		/// Permite enumerar los estatus de un egreso-ingreso
		/// </summary>
		public enum Estatus {
			/// <summary>
			/// Identifica el estatus de un egreso-ingreso si esta capturada
			/// </summary>
			Capturada = 1,

			/// <summary>
			/// Permite identificar el estatus de un  egreso-ingreso si esta aplicado
			/// </summary>
			Aplicada = 2,

			/// <summary>
			/// Permite identificar el estatus de un si un egreso-ingreso si esta aplicado parcialmente
			/// </summary>
			AplicadaParcial = 3,

			/// <summary>
			/// Permite identificar el estatus de un egreso-ingreso si esta cancelada
			/// </summary>
			Cancelada = 4,

			/// <summary>
			/// Permite indentificar el estatus de un Egreso-Ingreso si esta Depositado
			/// </summary>
			Depositado = 5
		};

		/// <summary>
		/// Permite enumerar los tipos de operacion (egreso,ingreso)
		/// </summary>
		public enum TipoOperacion {
			/// <summary>
			/// Permite identificar si el registro es un egreso
			/// </summary>
			Egreso = 1,

			/// <summary>
			/// Permite identificar si el registro es un ingreso
			/// </summary>
			Ingreso = 2
		};
		/// <summary>
		/// Formas de Pago válidas ante el SAT (Usado para validar datos requeridos en CFDI de Pagos)
		/// </summary>
		public enum FormaPagoSAT {
            _00_Ninguna = 0,
			_03_TransferenciaElectronica = 1,
			_01_Efectivo,
			_02_Cheque,
			_04_TarjetaCredito,
			_05_MonederoElectronico,
			_06_DineroElectronico,
			_08_ValesDespensa,
			_28_TarjetaDebito,
			_29_TarjetaServicios,
			_99_PorDefinir,
			_12_Dacion,
			_13_PagoSubrogacion,
			_14_PagoConsignacion,
			_15_Condonacion,
			_17_Compensacion,
			_23_Novacion,
			_24_Confusion,
			_25_RemisionDeuda,
			_26_PrescipcionCaducidad,
			_27_ASatisfaccionAcreedor
		}

		#endregion

		#region Atributos

		/// <summary>
		/// Atributo  que almacena el nombre del store procedure de la tabla
		/// </summary>
		private static string nom_sp = "bancos.sp_egreso_ingreso_tei";

		private int _id_egreso_ingreso;
		/// <summary>
		/// Id que permite identificar una ficha de ingreso aplicada
		/// </summary>
		public int id_egreso_ingreso {
			get { return _id_egreso_ingreso; }
		}

		private byte _id_tipo_operacion;
		/// <summary>
		/// Permite identificar el tipo de uso del registro (Egreso - Ingreso)
		/// </summary>
		public byte id_tipo_operacion {
			get { return _id_tipo_operacion; }
		}

		/// <summary>
		/// Permite almacenar la enumeración de tipo operación
		/// </summary>
		public TipoOperacion tipo_operacion {
			get { return (TipoOperacion)this._id_tipo_operacion; }
		}

		private int _secuencia_compania;
		/// <summary>
		/// Permite tener una secuencia de companias para cada egreso - ingreso
		/// </summary>
		public int secuencia_compania {
			get { return _secuencia_compania; }
		}

		private int _id_compania;
		/// <summary>
		/// ID que permite identificar una compania
		/// </summary>
		public int id_compania {
			get { return _id_compania; }
		}

		private int _id_tabla;
		/// <summary>
		/// Id que permite identificar a una entidad que referencia a un egreso - ingreso
		/// </summary>
		public int id_tabla {
			get { return _id_tabla; }
		}

		private int _id_registro;
		/// <summary>
		/// Id que permite identificar un registro que hace referencia a un egreso - ingreso
		/// </summary>
		public int id_registro {
			get { return _id_registro; }
		}

		private string _nombre_depositante;
		/// <summary>
		/// Describe el nombre del depositante de  un egreso - ingreso
		/// </summary>
		public string nombre_depositante {
			get { return _nombre_depositante; }
		}

		private byte _id_estatus;
		/// <summary>
		/// ID que permite identificar el estado de un egreso - ingreso
		/// </summary>
		public byte id_estatus {
			get { return _id_estatus; }
		}

		/// <summary>
		/// Permite acceder a los elementos de la enumeracion de estatus de un egreso-Ingreso
		/// </summary>
		public Estatus estatus {
			get { return (Estatus)this._id_estatus; }
		}

		private int _id_egreso_ingreso_concepto;
		/// <summary>
		/// Id que permite identificar el concepto de un egreso - ingreso
		/// </summary>
		public int id_egreso_ingreso_concepto {
			get { return _id_egreso_ingreso_concepto; }
		}

		private byte _id_forma_pago;
		/// <summary>
		/// Id que permite identificar la forma de pago de un egreso - ingreso
		/// </summary>
		public byte id_forma_pago {
			get { return _id_forma_pago; }
		}

		/// <summary>
		/// Permite ácceder a los elementos de la enumeración Método Pago.
		/// </summary>
		public FormaPagoSAT forma_pago {
			get { return (FormaPagoSAT)this._id_forma_pago; }
		}

		private int _id_cuenta_destino;
		/// <summary>
		/// Id que permite Identificar el número de cuenta que recibe un egreso - ingreso.
		/// </summary>
		public int id_cuenta_destino {
			get { return _id_cuenta_destino; }

		}
		private int _id_cuenta_origen;
		/// <summary>
		/// Id que permite Identificar el número de cuenta que envia un egreso - ingreso.
		/// </summary>
		public int id_cuenta_origen {
			get { return _id_cuenta_origen; }
		}

		private string _num_cheque;
		/// <summary>
		/// Número de cheque de un egreso - ingreso 
		/// </summary>
		public string num_cheque {
			get { return _num_cheque; }
		}

		private decimal _monto;
		/// <summary>
		/// Describe el monto monetario de un egreso - ingreso 
		/// </summary>
		public decimal monto {
			get { return _monto; }
		}

		private byte _id_moneda;
		/// <summary>
		/// Permite identificar la moneda que utiliza un egreso - ingreso
		/// </summary>
		public byte id_moneda {
			get { return _id_moneda; }
		}

		private decimal _monto_pesos;
		/// <summary>
		/// Describe el monto en pesos de un egreso - ingreso
		/// </summary>
		public decimal monto_pesos {
			get { return _monto_pesos; }
		}

		private DateTime _fecha_egreso_ingreso;
		/// <summary>
		/// Fecha en que se efectúa el egreso - ingreso 
		/// </summary>
		public DateTime fecha_egreso_ingreso {
			get { return _fecha_egreso_ingreso; }
		}

		private DateTime _fecha_captura;
		/// <summary>
		/// Fecha de captura de un egreso - ingreso
		/// </summary>
		public DateTime fecha_captura {
			get { return _fecha_captura; }
		}

		private bool _bit_transferido_nuevo;
		/// <summary>
		/// Permite identificar el estado de nuevo de un egreso - ingreso para su envio al sistema contable.
		/// </summary>
		public bool bit_transferido_nuevo {
			get { return _bit_transferido_nuevo; }
		}

		private int _id_transferido_nuevo;
		/// <summary>
		/// Identificar el estado de nuevo de un egreso - ingreso.
		/// </summary>
		public int id_transferido_nuevo {
			get { return _id_transferido_nuevo; }
		}

		private bool _bit_transferido_edicion;
		/// <summary>
		/// Permite identificar el estado de edición de un egreso - ingreso para su envio al sistema contable.
		/// </summary>
		public bool bit_transferido_edicion {
			get { return _bit_transferido_edicion; }
		}

		private int _id_transferido_edicion;
		/// <summary>
		/// Identifica el estado de edición de un egreso - ingreso.
		/// </summary>
		public int id_transferido_edicion {
			get { return _id_transferido_edicion; }
		}

		private bool _habilitar;
		/// <summary>
		/// Corresponde al estado de habilitacion de un registro de un banco
		/// </summary>
		public bool habilitar {
			get { return _habilitar; }
		}

		/// <summary>
		/// Obtiene Transferencia Bancaria
		/// </summary>
		private int _id_transferencia_bancaria;
		/// <summary>
		/// Corresponde a la Transferenci Bancaria
		/// </summary>
		public int id_transferencia_bancaria {
			get { return _id_transferencia_bancaria; }
		}

		/// <summary>
		/// Obtiene Transferencia Bancaria
		/// </summary>
		private int _id_rechazo_transferencia;
		/// <summary>
		/// Corresponde a la Transferenci Bancaria
		/// </summary>
		public int id_rechazo_transferencia {
			get { return _id_rechazo_transferencia; }
		}

		#endregion

		#region Contructores

		/// <summary>
		/// Constructor por default que inicializa los atributos de la clase.
		/// </summary>
		public EgresoIngreso()
		{
			this._id_egreso_ingreso = 0;
			this._id_tipo_operacion = 0;
			this._secuencia_compania = 0;
			this._id_compania = 0;
			this._id_tabla = 0;
			this._id_registro = 0;
			this._nombre_depositante = "";
			this._id_estatus = 0;
			this._id_egreso_ingreso_concepto = 0;
			this._id_forma_pago = 0;
			this._id_cuenta_destino = 0;
			this._id_cuenta_origen = 0;
			this._num_cheque = "";
			this._monto = 0.0M;
			this._id_moneda = 0;
			this._monto_pesos = 0.0M;
			this._fecha_egreso_ingreso = DateTime.MinValue;
			this._fecha_captura = DateTime.MinValue;
			this._bit_transferido_nuevo = false;
			this._id_transferido_nuevo = 0;
			this._bit_transferido_edicion = false;
			this._id_transferido_edicion = 0;
			this._habilitar = false;
			this._id_transferencia_bancaria = 0;
			this._id_rechazo_transferencia = 0;
		}

		/// <summary>
		/// Cosntructor que inicializa los atributos a partir de un registro de referencia.
		/// </summary>
		/// <param name="id_egreso_ingreso">Id que sirve como referencia para la busqueda de registros</param>
		public EgresoIngreso(int id_egreso_ingreso)
		{
			//Invoca al método cargaAtributoInstancia
			cargaAtributoInstancia(id_egreso_ingreso);
		}
		/// <summary>
		/// Constructor que inicializa los Atributos apartir de una Entidad y un Registro
		/// </summary>
		/// <param name="id_tabla">Entidad del Egreso</param>
		/// <param name="id_registro">Registro del Egreso</param>
		public EgresoIngreso(int id_tabla, int id_registro)
		{
			//Invoca al método cargaAtributoInstancia
			cargaAtributoInstancia(id_tabla, id_registro);
		}

		#endregion

		#region Destructor

		/// <summary>
		/// Destructor de la clase
		/// </summary>
		~EgresoIngreso()
		{
			Dispose(false);
		}

		#endregion

		#region Métodos Privados

		/// <summary>
		/// Método que asigna valores a los atributos de registros de un egreso - ingreso
		/// </summary>
		/// <param name="id_egreso_ingreso">Id de referencia para la asignación de registros </param>
		/// <returns></returns>
		private bool cargaAtributoInstancia(int id_egreso_ingreso)
		{
			//Creación del método retorno
			bool retorno = false;
			//Creación y Asignación  de valores al arreglo necesarios para el sp de la tabla
			object[] param = { 3, id_egreso_ingreso, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m,
															 null, null, false, 0, false, 0, 0, false, "", "" };
			//Invoca al  al sp de la tabla
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Validación de los datos del dataset (que existan y que no sean nulos)
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
				{
					//Recorrido de las filas y almacenamiento de registros en la variable r
					foreach (DataRow r in DS.Tables[0].Rows)
					{
						this._id_egreso_ingreso = id_egreso_ingreso;
						this._id_tipo_operacion = Convert.ToByte(r["IdTipoOperacion"]);
						this._secuencia_compania = Convert.ToInt32(r["SecuenciaCompania"]);
						this._id_compania = Convert.ToInt32(r["IdCompania"]);
						this._id_tabla = Convert.ToInt32(r["IdTabla"]);
						this._id_registro = Convert.ToInt32(r["IdRegistro"]);
						this._nombre_depositante = Convert.ToString(r["NombreDespositante"]);
						this._id_estatus = Convert.ToByte(r["IdEstatus"]);
						this._id_egreso_ingreso_concepto = Convert.ToInt32(r["IdEgresoIngresoConcepto"]);
						this._id_forma_pago = Convert.ToByte(r["IdMetodoPago"]);
						this._id_cuenta_destino = Convert.ToInt32(r["IdCuentaDestino"]);
						this._id_cuenta_origen = Convert.ToInt32(r["IdCuentaOrigen"]);
						this._num_cheque = Convert.ToString(r["NumCheque"]);
						this._monto = Convert.ToDecimal(r["Monto"]);
						this._id_moneda = Convert.ToByte(r["IdMoneda"]);
						this._monto_pesos = Convert.ToDecimal(r["MontoPesos"]);
						DateTime.TryParse(r["FechaEgresoIngreso"].ToString(), out this._fecha_egreso_ingreso);
						this._fecha_captura = Convert.ToDateTime(r["FechaCaptura"]);
						this._bit_transferido_nuevo = Convert.ToBoolean(r["BitTransferidoNuevo"]);
						this._id_transferido_nuevo = Convert.ToInt32(r["IdTransferidoNuevo"]);
						this._bit_transferido_edicion = Convert.ToBoolean(r["BitTransferidoEdicion"]);
						this._id_transferido_edicion = Convert.ToInt32(r["IdTRansferidoEdicion"]);
						this._habilitar = Convert.ToBoolean(r["Habilitar"]);
						this._id_transferencia_bancaria = Convert.ToInt32(r["IdTransferenciaBancaria"]);
						this._id_rechazo_transferencia = Convert.ToInt32(r["IdRazonRechazo"]);
					}
					//Cambio de valor a retorno siempre y cuando cumpla la sentencia de validación de datos
					retorno = true;
				}
			}
			//Retrono del resultado al método
			return retorno;
		}
		/// <summary>
		/// Método que asigna valores a los atributos de registros de un egreso - ingreso
		/// </summary>
		/// <param name="id_tabla">Entidad del Registro</param>
		/// <param name="id_registro">Registro del Egreso</param>
		/// <returns></returns>
		private bool cargaAtributoInstancia(int id_tabla, int id_registro)
		{
			//Creación del método retorno
			bool retorno = false;
			//Creación y Asignación  de valores al arreglo necesarios para el sp de la tabla
			object[] param = { 8, 0, 0, 0, 0, id_tabla, id_registro, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m,
															 null, null, false, 0, false, 0, 0, false, "", "" };
			//Invoca al  al sp de la tabla
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Validación de los datos del dataset (que existan y que no sean nulos)
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
				{
					//Recorrido de las filas y almacenamiento de registros en la variable r
					foreach (DataRow r in DS.Tables["Table"].Rows)
					{
						//Asignando Atributos
						this._id_egreso_ingreso = Convert.ToInt32(r["Id"]);
						this._id_tipo_operacion = Convert.ToByte(r["IdTipoOperacion"]);
						this._secuencia_compania = Convert.ToInt32(r["SecuenciaCompania"]);
						this._id_compania = Convert.ToInt32(r["IdCompania"]);
						this._id_tabla = Convert.ToInt32(r["IdTabla"]);
						this._id_registro = Convert.ToInt32(r["IdRegistro"]);
						this._nombre_depositante = Convert.ToString(r["NombreDespositante"]);
						this._id_estatus = Convert.ToByte(r["IdEstatus"]);
						this._id_egreso_ingreso_concepto = Convert.ToInt32(r["IdEgresoIngresoConcepto"]);
						this._id_forma_pago = Convert.ToByte(r["IdMetodoPago"]);
						this._id_cuenta_destino = Convert.ToInt32(r["IdCuentaDestino"]);
						this._id_cuenta_origen = Convert.ToInt32(r["IdCuentaOrigen"]);
						this._num_cheque = Convert.ToString(r["NumCheque"]);
						this._monto = Convert.ToDecimal(r["Monto"]);
						this._id_moneda = Convert.ToByte(r["IdMoneda"]);
						this._monto_pesos = Convert.ToDecimal(r["MontoPesos"]);
						DateTime.TryParse(r["FechaEgresoIngreso"].ToString(), out this._fecha_egreso_ingreso);
						this._fecha_captura = Convert.ToDateTime(r["FechaCaptura"]);
						this._bit_transferido_nuevo = Convert.ToBoolean(r["BitTransferidoNuevo"]);
						this._id_transferido_nuevo = Convert.ToInt32(r["IdTransferidoNuevo"]);
						this._bit_transferido_edicion = Convert.ToBoolean(r["BitTransferidoEdicion"]);
						this._id_transferido_edicion = Convert.ToInt32(r["IdTRansferidoEdicion"]);
						this._habilitar = Convert.ToBoolean(r["Habilitar"]);
						this._id_transferencia_bancaria = Convert.ToInt32(r["IdTransferenciaBancaria"]);
						this._id_rechazo_transferencia = Convert.ToInt32(r["IdRazonRechazo"]);
					}
					//Cambio de valor a retorno siempre y cuando cumpla la sentencia de validación de datos
					retorno = true;
				}
			}
			//Retrono del resultado al método
			return retorno;
		}

		/// <summary>
		/// Método que permite actualizar campos de ficha ingreso
		/// </summary>
		/// <param name="tipo_operacion">Permiso que permite actualizar el campo id_tipo_operacion</param>
		/// <param name="secuencia_compania">Permiso que permite actualizar el campo secuencia_compania </param>
		/// <param name="id_compania">Permiso que permite actualizar el campo id_compania</param>
		/// <param name="id_tabla">Permiso que permite actualizar el campo id_tabla</param>
		/// <param name="id_registro">Permiso que permite actualizar el campo id_registro</param>
		/// <param name="nombre_depositante">Permiso que permite actualizar el campo nombre_depositante</param>
		/// <param name="estatus">Estatus</param>
		/// <param name="id_egreso_ingreso_concepto">Permiso que permite actualizar el campo id_egreso_ingreso_concepto</param>
		/// <param name="forma_pago">Permiso que permite actualizar el campo id_metodo_pago</param>
		/// <param name="id_cuenta_destino">Permiso que permite actualizar el campo id_cuenta_destino</param>
		/// <param name="id_cuenta_origen">Permiso que permite actualizar el campo id_cuenta_origen</param>
		/// <param name="num_cheque">Permiso que permite actualizar el campo num_cheque</param>
		/// <param name="monto">Permiso que permite actualizar el campo monto</param>
		/// <param name="id_moneda">Permiso que permite actualizar el campo id_moneda</param>
		/// <param name="monto_pesos">Permiso que permite actualizar el campo monto_pesos</param>
		/// <param name="fecha_egreso_ingreso">Permiso que permite actualizar el campo fecha_egreso_ingreso</param>
		/// <param name="fecha_captura">Permiso que permite actualizar el campo fecha_captura</param>
		/// <param name="bit_transferido_nuevo">Permiso que permite actualizar el campo bit_transferiso_nuevo</param>
		/// <param name="id_transferido_nuevo">Permiso que permite actualizar el campo id_transferido_nuevo</param>
		/// <param name="bit_transferido_edicion">Permiso que permite actualizar el campo bit_trasnferido_edicion</param>
		/// <param name="id_transferido_edicion">Permiso que permite actualizar el campo id_trnasferido_edicion</param>
		/// <param name="transferencia_bancaria">No Transferencia Bancaria</param>
		/// <param name="razon_rechazo">Razón del rechazo del egreso</param>
		/// <param name="id_usuario">Permiso que permite actualizar el campo id_usuario</param>
		/// <param name="habilitar">Permiso que permite actualizar el campo habilitar</param>
		/// <returns></returns>
		private RetornoOperacion editarEgresoIngreso(TipoOperacion tipo_operacion, int secuencia_compania, int id_compania, int id_tabla,
																								 int id_registro, string nombre_depositante, Estatus estatus, int id_egreso_ingreso_concepto,
                                                                                                 FormaPagoSAT forma_pago, int id_cuenta_destino, int id_cuenta_origen, string num_cheque,
																								 decimal monto, byte id_moneda, decimal monto_pesos, DateTime fecha_egreso_ingreso, DateTime fecha_captura,
																								 bool bit_transferido_nuevo, int id_transferido_nuevo, bool bit_transferido_edicion,
																								 int id_transferido_edicion, string transferencia_bancaria, string razon_rechazo, int id_usuario, bool habilitar)
		{
			//Creación del obejto retorno
			RetornoOperacion retorno = new RetornoOperacion(0);

			//Inicializando Transacción
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Si existe la Referencia de transferencia bancara en el egreso
				if (!string.IsNullOrEmpty(transferencia_bancaria))
				{
					//SI el monto es mayor a 0
					if (monto > 0)
					{
						//Instancia Referencia de Transferencia Bancaria
						using (SAT_CL.Global.Referencia objReferenciaTrans = new SAT_CL.Global.Referencia(this._id_transferencia_bancaria))
						{
							//Si existe la Refrencia la editamos
							if (objReferenciaTrans.id_referencia > 0)
							{
								//Editamos Referencia
								retorno = SAT_CL.Global.Referencia.EditaReferencia(objReferenciaTrans.id_referencia, transferencia_bancaria, id_usuario);
							}
							else
							{
								//Insertamos Referencia del No Transferencia
								retorno = Global.Referencia.InsertaReferencia(this._id_egreso_ingreso, 101, Global.ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 101, "No. Transferencia", 0, "Datos Contables"), transferencia_bancaria, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
							}
						}
					}
					else
						retorno = new RetornoOperacion("Se debe especificar un monto mayor a 0.");
				}
				//Si se añadirá referencia por rechazo de egreso
				else if (!string.IsNullOrEmpty(razon_rechazo))
				{
					//Instancia Referencia de Transferencia Bancaria
					using (SAT_CL.Global.Referencia objReferenciaRechazo = new Global.Referencia(this._id_rechazo_transferencia))
					{
						//Si existe la Refrencia la editamos
						if (objReferenciaRechazo.id_referencia > 0)
						{
							//Editamos Referencia
							retorno = SAT_CL.Global.Referencia.EditaReferencia(objReferenciaRechazo.id_referencia, razon_rechazo, id_usuario);
						}
						else
						{
							//Insertamos Referencia del No Transferencia
							retorno = Global.Referencia.InsertaReferencia(this._id_egreso_ingreso, 101, Global.ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 101, "Razón Rechazo", 0, "Datos Contables"), razon_rechazo, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
						}
					}
				}

				//Validamos Resultado
				if (retorno.OperacionExitosa)
				{
					//TODO: CONTEMPLAR ACTUALIZACIÓN DE BITS DE EDICIÓN PARA TRANSFERENCIAS

					//Creación y asignación  de valores al arreglo necesarios para el SP de la tabla
					object[] param = { 2, this.id_egreso_ingreso, (byte)tipo_operacion, secuencia_compania, id_compania, id_tabla, id_registro, nombre_depositante,
															 (byte)estatus, id_egreso_ingreso_concepto, (byte)forma_pago, id_cuenta_destino, id_cuenta_origen, num_cheque, monto, id_moneda,
															 monto_pesos, Fecha.ConvierteDateTimeObjeto(fecha_egreso_ingreso), fecha_captura, bit_transferido_nuevo, id_transferido_nuevo, bit_transferido_edicion, id_transferido_edicion,
															 id_usuario, habilitar, "", "" };
					//Asignación de valores al objeto retorno
					retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
				}

				//Validamos Resultado
				if (retorno.OperacionExitosa)
				{
					//Confirmando transacción
					scope.Complete();
				}
			}

			//Retorno del resultado al método
			return retorno;
		}
		/// <summary>
		/// Método que permite insertar registros a un egreso - ingreso
		/// </summary>
		/// <param name="tipo_operacion">Campo id_tipo_operacion</param>
		/// <param name="id_compania">Campo id_compania</param>
		/// <param name="id_tabla">Campo id_tabla</param>
		/// <param name="id_registro">Campo id_registro</param>
		/// <param name="nombre_depositante">Campo nombre_depositante</param>
		/// <param name="id_egreso_ingreso_concepto">Campo id_egreso_ingreso_concepto</param>
		/// <param name="id_metodo_pago">Campo id_metodo_pago</param>
		/// <param name="id_cuenta_destino">Campo id_cuenta_destino</param>
		/// <param name="id_cuenta_origen">Campo id_cuenta_origen</param>
		/// <param name="num_cheque">Campo num_cheque</param>
		/// <param name="monto">Campo monto</param>
		/// <param name="id_moneda">Campo id_moneda</param>
		/// <param name="monto_pesos">Campo monto_pesos</param>
		/// <param name="fecha_egreso_ingreso">Campo fecha_egreso_ingreso</param>
		/// <param name="transferencia_bancaria">Transferencia Bancaria</param>
		/// <param name="id_usuario">Campo id_usuario</param>
		/// <returns></returns>
		private static RetornoOperacion insertarEgresoIngreso(TipoOperacion tipo_operacion, int id_compania, int id_tabla, int id_registro, string nombre_depositante,
																												 int id_egreso_ingreso_concepto, FormaPagoSAT forma_pago, int id_cuenta_destino, int id_cuenta_origen,
																												 string num_cheque, decimal monto, byte id_moneda, decimal monto_pesos, DateTime fecha_egreso_ingreso, string transferencia_bancaria,
																												 int id_usuario)
		{
			//Declaramos Objeto Retorno
			RetornoOperacion retorno = new RetornoOperacion(0);

			//Declaramos Objeto Resultado
			int id_egreso_ingreso = 0;

			//Inicializando Transacción
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Si el monto a registrar es mayor a 0
				if (monto > 0 || (monto < 0 && id_tabla == 82))
				{
					//Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
					object[] param = { 1, 0, (byte)tipo_operacion, 0, id_compania, id_tabla, id_registro, nombre_depositante,(byte)Estatus.Capturada, id_egreso_ingreso_concepto,
															 (byte)forma_pago, id_cuenta_destino, id_cuenta_origen, num_cheque, monto, id_moneda, monto_pesos, fecha_egreso_ingreso, Fecha.ObtieneFechaEstandarMexicoCentro(),
															 false, 0, false, 0, id_usuario, true, "", "" };

					//Asignación  de valores del objeto retorno
					retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

					//Establecmos Mensaje
					id_egreso_ingreso = retorno.IdRegistro;

					//Si existe la Referencia
					if (transferencia_bancaria != "")
					{
						//Validamos Resultado
						if (retorno.OperacionExitosa)
						{
							//Insertamos Referencia del No Transferencia
							retorno = Global.Referencia.InsertaReferencia(retorno.IdRegistro, 101, Global.ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 101, "No. Transferencia", 0, "Datos Contables"), transferencia_bancaria, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
						}
					}

					//Validamos Resultado
					if (retorno.OperacionExitosa)
					{
						//Finalizamos Transacción
						scope.Complete();

						//Establemeos Resultado
						retorno = new RetornoOperacion(id_egreso_ingreso);
					}
				}
				else
					retorno = new RetornoOperacion("Se debe especificar un monto mayor a 0.");
			}

			//Retorno del resultado al método
			return retorno;
		}

		/// <summary>
		/// Método encargado de Validar las Aplicaciones de las Fichas
		/// </summary>
		/// <returns></returns>
		private RetornoOperacion validaAplicacionFicha()
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
			object[] param = { 5, this._id_egreso_ingreso, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0, 0, 0, null, null, false, 0, false, 0, 0, false, "", "" };

			//Instanciando Resultado
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Validando que existan Registros
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Iniciando Ciclo
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Validando que Exista un Indicador
						if (Convert.ToInt32(dr["Aplicaciones"]) > 0)

							//Instanciando Resultado Negativo
							result = new RetornoOperacion("La Ficha ya ha sido Aplicada. Imposible su Edición");
						else
							//Instanciando Resultado Positivo
							result = new RetornoOperacion(this._id_egreso_ingreso, "", true);
					}
				}
			}

			//Devolviendo Resultado Obtenido
			return result;
		}

		#endregion

		#region Métodos Públicos

		/// <summary>
		/// Método Público encargado de Insertar el Pago de una Liquidación
		/// </summary>
		/// <param name="id_compania">Compania que Realiza la Liquidación</param>
		/// <param name="id_registro_liquidacion">Referencia a la Liquidación</param>
		/// <param name="monto">Monto de la Liquidación</param>
		/// <param name="monto_pesos">Monto de la Liquidación en Pesos</param>
		/// <param name="fecha_egreso_ingreso">Fecha de la Operación (Egreso - Ingreso)</param>
		/// <param name="id_usuario">Usuario que Inserta el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaPagoLiquidacion(int id_compania, int id_registro_liquidacion, decimal monto, decimal monto_pesos, DateTime fecha_egreso_ingreso, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Instanciando Liqudiación
			using (Liquidacion.Liquidacion liq = new Liquidacion.Liquidacion(id_registro_liquidacion))
			{
				//Validando que exista la Liquidación
				if (liq.habilitar)
				{
					//Validando que sea una Liqudiación de Proveedor
					if (liq.id_proveedor != 0)
					{
						//Validando que no sea una Liquidación en 0's
						if (monto.Equals(0.00M))
						{
							object[] param = { 1, 0, (byte)TipoOperacion.Egreso, 0, id_compania, 82, id_registro_liquidacion, "", (byte)Estatus.Aplicada, 18, (byte)FormaPagoSAT._03_TransferenciaElectronica, 0, 0, "", monto, 1, monto_pesos, fecha_egreso_ingreso, Fecha.ObtieneFechaEstandarMexicoCentro(), false, 0, false, 0, id_usuario, true, "", "" };
							result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
						}
						else
							//Retorno del resultado al método
							result = insertarEgresoIngreso(TipoOperacion.Egreso, id_compania, 82, id_registro_liquidacion, "", 18, FormaPagoSAT._03_TransferenciaElectronica, 0, 0, "", monto, 1, monto, fecha_egreso_ingreso, "", id_usuario);
					}
					else
					{
						//Validando que no sea una Liquidación en 0's
						if (monto.Equals(0.00M))
						{
							//Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
							object[] param = { 1, 0, (byte)TipoOperacion.Egreso, 0, id_compania, 82, id_registro_liquidacion, "", (byte)Estatus.Aplicada, 1, (byte)FormaPagoSAT._03_TransferenciaElectronica, 0, 0, "", monto, 1, monto_pesos, fecha_egreso_ingreso, Fecha.ObtieneFechaEstandarMexicoCentro(), false, 0, false, 0, id_usuario, true, "", "" };

							//Asignación  de valores del objeto retorno
							result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
						}
						else
							//Retorno del resultado al método
							result = insertarEgresoIngreso(TipoOperacion.Egreso, id_compania, 82, id_registro_liquidacion, "", 1, FormaPagoSAT._03_TransferenciaElectronica, 0, 0, "", monto, 1, monto,
																	fecha_egreso_ingreso, "", id_usuario);
					}
				}
			}

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método que permite insertar un egreso por concepto de depósito a operador o permisionario
		/// </summary>
		/// <param name="id_compania">Campo id_compania</param>
		/// <param name="id_registro_deposito">Campo id_registro</param>
		/// <param name="monto">Campo monto</param>
		/// <param name="id_usuario">Campo id_usuario</param>
		/// <returns></returns>
		public static RetornoOperacion InsertarEgresoDepositoOperadorYPermisionario(int id_compania, int id_registro_deposito, decimal monto, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Instanciando Deposito
			using (EgresoServicio.Deposito dep = new EgresoServicio.Deposito(id_registro_deposito))
			//Instanciando Concepto
			using (EgresoServicio.ConceptoDeposito cd = new EgresoServicio.ConceptoDeposito(dep.id_concepto))
			{
				//Validando Registros
				if (dep.habilitar && cd.habilitar)
				{
					//Validando si es un Concepto de Proveedor
					if (cd.descripcion.Equals("Anticipo Proveedor") || cd.descripcion.Equals("Finiquito Proveedor"))

						//Retorno del resultado al método
						result = insertarEgresoIngreso(TipoOperacion.Egreso, id_compania, 51, id_registro_deposito, "", 17, FormaPagoSAT._03_TransferenciaElectronica, 0, 0, "", monto, 1, monto,
																		DateTime.MinValue, "", id_usuario);
					else
						//Retorno del resultado al método
						result = insertarEgresoIngreso(TipoOperacion.Egreso, id_compania, 51, id_registro_deposito, "", 1, FormaPagoSAT._03_TransferenciaElectronica, 0, 0, "", monto, 1, monto,
																		DateTime.MinValue, "", id_usuario);
				}
				else
					//Instanciando Excepción
					result = new RetornoOperacion("No existe el Deposito");
			}

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método que Permite Insertar las Fichas de Ingreso
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_tipo_entidad">Tipo de Entidad de la Ficha de Ingreso</param>
		/// <param name="id_registro">Entidad de la Ficha de Ingreso</param>
		/// <param name="nombre_dep">Nombre del Depositante</param>
		/// <param name="id_concepto">Concepto de la Ficha</param>
		/// <param name="forma_pago">Método de Pago de la Ficha</param>
		/// <param name="id_cta_origen">No. de Cuenta de Origen (De la que se Obtiene el Dinero)</param>
		/// <param name="id_cta_destino">No. de Cuenta de Destino (En la que se Recibe el Dinero)</param>
		/// <param name="num_cheque">Número de Cheque</param>
		/// <param name="monto">Monto de la Ficha</param>
		/// <param name="id_moneda">Moneda</param>
		/// <param name="monto_pesos">Monto de la Ficha en Pesos</param>
		/// <param name="fecha_egreso_ingreso">Fecha de la Ficha de Ingreso</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaFichaIngreso(int id_compania, int id_tipo_entidad, int id_registro, string nombre_dep, int id_concepto, FormaPagoSAT forma_pago,
																											 int id_cta_origen, int id_cta_destino, string num_cheque, decimal monto, byte id_moneda, decimal monto_pesos,
																											 DateTime fecha_egreso_ingreso, int id_usuario)
		{
			//Retorno del resultado al método
			return insertarEgresoIngreso(TipoOperacion.Ingreso, id_compania, id_tipo_entidad, id_registro, nombre_dep, id_concepto, forma_pago, id_cta_destino, id_cta_origen, num_cheque, monto,
																	id_moneda, monto_pesos, fecha_egreso_ingreso, "", id_usuario);
		}
		/// <summary>
		/// Método que Permite Insertar Egresos Varios
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_tipo_entidad">Tipo de Entidad de la Ficha de Ingreso</param>
		/// <param name="id_registro">Entidad del Egreso</param>
		/// <param name="nombre_dep">Nombre del Depositante</param>
		/// <param name="id_concepto">Concepto del Egreso</param>
		/// <param name="metodo_pago">Método de Pago del Egreso</param>
		/// <param name="id_cta_origen">No. de Cuenta de Origen (De la que se Obtiene el Dinero)</param>
		/// <param name="id_cta_destino">No. de Cuenta de Destino (En la que se Recibe el Dinero)</param>
		/// <param name="num_cheque">Número de Cheque</param>
		/// <param name="monto">Monto del Egreso</param>
		/// <param name="id_moneda">Moneda</param>
		/// <param name="monto_pesos">Monto del Egreso en Pesos</param>
		/// <param name="fecha_egreso_ingreso">Fecha del Egreso</param>
		/// <param name="transferencia_bancaria">Transferencia Bancaria</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaEgresosVarios(int id_compania, int id_tipo_entidad, int id_registro, string nombre_dep, int id_concepto, FormaPagoSAT forma_pago,
                                                                                                             int id_cta_origen, int id_cta_destino, string num_cheque, decimal monto, byte id_moneda, decimal monto_pesos,
																											 DateTime fecha_egreso_ingreso, string transferencia_bancaria, int id_usuario)
		{
			//Retorno del resultado al método
			return insertarEgresoIngreso(TipoOperacion.Egreso, id_compania, id_tipo_entidad, id_registro, nombre_dep, id_concepto, forma_pago, id_cta_destino, id_cta_origen, num_cheque, monto,
																	id_moneda, monto_pesos, fecha_egreso_ingreso, transferencia_bancaria, id_usuario);
		}
		/// <summary>
		/// Método que Permite Insertar los Pagos a las Facturas de los Proveedores
		/// </summary>
		/// <param name="fecha_deposito">Fecha Deposito de la Factura</param>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_tipo_entidad">Tipo de Entidad de la Ficha de Ingreso</param>
		/// <param name="id_registro">Entidad de la Ficha de Ingreso</param>
		/// <param name="nombre_dep">Nombre del Depositante</param>
		/// <param name="id_cta_origen">No. de Cuenta de Origen (De la que se Obtiene el Dinero)</param>
		/// <param name="id_cta_destino">No. de Cuenta de Destino (En la que se Recibe el Dinero)</param>
		/// <param name="num_cheque">Número de Cheque</param>
		/// <param name="monto">Monto de la Ficha</param>
		/// <param name="id_moneda">Moneda</param>
		/// <param name="monto_pesos">Monto de la Ficha en Pesos</param>
		/// <param name="fecha_egreso_ingreso">Fecha de la Ficha de Ingreso</param>
		/// <param name="transferencia_bancaria">No Tranferencia Bancaria</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaPagoFactura(DateTime fecha_deposito, int id_compania, int id_tipo_entidad, int id_registro, string nombre_dep, int id_cta_origen, int id_cta_destino,
																											string num_cheque, decimal monto, byte id_moneda, decimal monto_pesos, DateTime fecha_egreso_ingreso,
																										 string transferencia_bancaria, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Declaramos Objeto Resultado
			int id_egreso_ingreso = 0;
			//Inicializando Transacción
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{


				//Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
				object[] param = { 1, 0, (byte)TipoOperacion.Egreso, 0, id_compania, id_tipo_entidad, id_registro, nombre_dep,(byte)Estatus.Aplicada, 15,
															 (byte)FormaPagoSAT._03_TransferenciaElectronica, id_cta_destino, id_cta_origen, num_cheque, monto, id_moneda, monto_pesos, fecha_egreso_ingreso, fecha_deposito,
															 false, 0, false, 0, id_usuario, true, "", "" };

				//Asignación  de valores del objeto retorno
				result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

				//Asignamos Valor
				id_egreso_ingreso = result.IdRegistro;

				//Si existe la Referencia
				if (transferencia_bancaria != "")
				{
					//Validamos Resultado
					if (result.OperacionExitosa)
					{
						//Insertamos Referencia del No Transferencia
						result = Global.Referencia.InsertaReferencia(id_egreso_ingreso, 101, Global.ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 101, "No. Transferencia", 0, "Datos Contables"), transferencia_bancaria, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
					}
				}
				//Validamos Resultado
				if (result.OperacionExitosa)
				{
					//Finalizamos Transacción
					scope.Complete();

					result = new RetornoOperacion(id_egreso_ingreso);
				}
			}
			//Devolviendo Objeto de Retorno
			return result;
		}

		/// <summary>
		/// Método que permite actualizar los registros de un egreso - ingreso
		/// </summary>
		/// <param name="tipo_operacion">Campo que permite actualizar id_tipo_operacion</param>
		/// <param name="secuencia_compania">Campo que permite actualizar secuencia_compania</param>
		/// <param name="id_compania">Campo que permite actualizar id_compania</param>
		/// <param name="id_tabla">Campo que permite actualizar id_tabla</param>
		/// <param name="id_registro">Campo que permite actualizar id_registro</param>
		/// <param name="nombre_depositante">Campo que permite actualizar nombre_depositante</param>
		/// <param name="estatus">Campo que permite actualizar id_estatus</param>
		/// <param name="id_egreso_ingreso_concepto">Campo que permite actualizar id-concepto_ficha</param>
		/// <param name="forma_pago">Campo que permite actualizar id_metodo_pago</param>
		/// <param name="id_cuenta_destino">Campo que permite actualizar id_cuenta_destino</param>
		/// <param name="id_cuenta_origen">Campo que permite actualizar id_cuenta_origen</param>
		/// <param name="num_cheque">Campo que permite actualizar num_cheque</param>
		/// <param name="monto">Campo que permite actualizar monto</param>
		/// <param name="id_moneda">Campo que permite actualizar id_moneda</param>
		/// <param name="monto_pesos">Campo que permite actualizar monto_pesos</param>
		/// <param name="fecha_egreso_ingreso">Campo que permite actualizar fecha_egreso_ingreso</param>
		/// <param name="fecha_captura">Campo que permite actualizar fecha_captura</param>
		/// <param name="bit_transferido_nuevo">Campo que permite actualizar bit_transferido_nuevo</param>
		/// <param name="id_transferido_nuevo">Campo que permite actualizar id_transferido_nuevo</param>
		/// <param name="bit_transferido_edicion">Campo que permite actualizar bit_transferido_edicion</param>
		/// <param name="id_transferido_edicion">Campo que permite actualizar id_transferido_edicion</param>
		/// <param name="transferencia_bancaria">No de Transferencia Bancaria</param>
		/// <param name="razon_rechazo"></param>
		/// <param name="id_usuario">Campo que permite actualizar id_usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditarEgresoIngreso(TipoOperacion tipo_operacion, int secuencia_compania, int id_compania, int id_tabla, int id_registro,
																								string nombre_depositante, Estatus estatus, int id_egreso_ingreso_concepto, FormaPagoSAT forma_pago,
																								int id_cuenta_destino, int id_cuenta_origen, string num_cheque, decimal monto, byte id_moneda,
																								decimal monto_pesos, DateTime fecha_egreso_ingreso, DateTime fecha_captura, bool bit_transferido_nuevo,
																								int id_transferido_nuevo, bool bit_transferido_edicion, int id_transferido_edicion,
																								string transferencia_bancaria, string razon_rechazo, int id_usuario)
		{
			//Retorna e Invoca al método editarEgresoIngreso
			return this.editarEgresoIngreso(tipo_operacion, secuencia_compania, id_compania, id_tabla, id_registro, nombre_depositante, estatus,
																			id_egreso_ingreso_concepto, forma_pago, id_cuenta_destino, id_cuenta_origen, num_cheque, monto, id_moneda,
																			monto_pesos, fecha_egreso_ingreso, fecha_captura, bit_transferido_nuevo, id_transferido_nuevo, bit_transferido_edicion,
																			id_transferido_edicion, transferencia_bancaria, razon_rechazo, id_usuario, this._habilitar);

		}
		/// <summary>
		/// Método que Permite Insertar las Fichas de Ingreso
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_tipo_entidad">Tipo de Entidad de la Ficha de Ingreso</param>
		/// <param name="id_registro">Entidad de la Ficha de Ingreso</param>
		/// <param name="nombre_dep">Nombre del Depositante</param>
		/// <param name="id_concepto">Concepto de la Ficha</param>
		/// <param name="forma_pago">Método de Pago de la Ficha</param>
		/// <param name="id_cta_origen">No. de Cuenta de Origen (De la que se Obtiene el Dinero)</param>
		/// <param name="id_cta_destino">No. de Cuenta de Destino (En la que se Recibe el Dinero)</param>
		/// <param name="num_cheque">Número de Cheque</param>
		/// <param name="monto">Monto de la Ficha</param>
		/// <param name="id_moneda">Moneda</param>
		/// <param name="monto_pesos">Monto de la Ficha en Pesos</param>
		/// <param name="fecha_egreso_ingreso">Fecha de la Ficha de Ingreso</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion EditaFichaIngreso(int id_compania, int id_tipo_entidad, int id_registro, string nombre_dep, int id_concepto, FormaPagoSAT forma_pago,
																						int id_cta_origen, int id_cta_destino, string num_cheque, decimal monto, byte id_moneda, decimal monto_pesos,
																						DateTime fecha_egreso_ingreso, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Validando Aplicacion
			result = validaAplicacionFicha();

			//Validando Resultado
			if (result.OperacionExitosa)

				//Retorno del resultado al método
				result = this.editarEgresoIngreso(TipoOperacion.Ingreso, this._secuencia_compania, id_compania, id_tipo_entidad, id_registro, nombre_dep, (Estatus)this._id_estatus, id_concepto,
											forma_pago, id_cta_destino, id_cta_origen, num_cheque, monto, id_moneda, monto_pesos, fecha_egreso_ingreso, Fecha.ObtieneFechaEstandarMexicoCentro(),
											this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, "", "", id_usuario, this._habilitar);

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Deshabilitar las Fichas de Ingreso
		/// </summary>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitarFichaIngreso(int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Validando Aplicacion
			result = validaAplicacionFicha();

			//Validando Resultado
			if (result.OperacionExitosa)

				//Retorna e invoca el metodo privado editarEgresoIngreso
				result = this.editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante,
																				this.estatus, this._id_egreso_ingreso_concepto, this.forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque,
																				this._monto, this._id_moneda, this._monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura, this._bit_transferido_nuevo,
																				this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, "", "", id_usuario, false);

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método que Permite Editar Egresos Varios
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_tipo_entidad">Tipo de Entidad de la Ficha de Ingreso</param>
		/// <param name="id_registro">Entidad del Egreso</param>
		/// <param name="nombre_dep">Nombre del Depositante</param>
		/// <param name="id_concepto">Concepto del Egreso</param>
		/// <param name="forma_pago">Método de Pago del Egreso</param>
		/// <param name="id_cta_origen">No. de Cuenta de Origen (De la que se Obtiene el Dinero)</param>
		/// <param name="id_cta_destino">No. de Cuenta de Destino (En la que se Recibe el Dinero)</param>
		/// <param name="num_cheque">Número de Cheque</param>
		/// <param name="monto">Monto del Egreso</param>
		/// <param name="id_moneda">Moneda</param>
		/// <param name="monto_pesos">Monto del Egreso en Pesos</param>
		/// <param name="fecha_egreso_ingreso">Fecha del Egreso</param>
		/// <param name="transferencia_bancaria">Transferencia Bancaria</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		public RetornoOperacion EditaEgresosVarios(int id_compania, int id_tipo_entidad, int id_registro, string nombre_dep, int id_concepto, FormaPagoSAT forma_pago,
																						int id_cta_origen, int id_cta_destino, string num_cheque, decimal monto, byte id_moneda, decimal monto_pesos,
																						DateTime fecha_egreso_ingreso, string transferencia_bancaria, int id_usuario)
		{
			//Retorno del resultado al método
			return this.editarEgresoIngreso(TipoOperacion.Egreso, this._secuencia_compania, id_compania, id_tipo_entidad, id_registro, nombre_dep, (Estatus)this._id_estatus, id_concepto,
					forma_pago, id_cta_destino, id_cta_origen, num_cheque, monto, id_moneda, monto_pesos, fecha_egreso_ingreso, Fecha.ObtieneFechaEstandarMexicoCentro(),
																	this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, transferencia_bancaria, "", id_usuario, this._habilitar);
		}

		/// <summary>
		/// Método encargado de Depositar el Egreso-Ingreso
		/// </summary>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion DepositaEgresosVarios(int id_usuario)
		{
			//Retorno del resultado al método
			return this.editarEgresoIngreso(TipoOperacion.Egreso, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante, Estatus.Depositado, this._id_egreso_ingreso_concepto,
																	forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto, this._id_moneda, monto_pesos, this._fecha_egreso_ingreso, Fecha.ObtieneFechaEstandarMexicoCentro(),
																	this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, new Global.Referencia(this._id_transferencia_bancaria).valor,
																	"", id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método Público encargado de Actualizar el Estatus de las Fichas de Ingreso
		/// </summary>
		/// <param name="estatus">Estatus a Actualizar</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaFichaIngresoEstatus(Estatus estatus, int id_usuario)
		{
			//Retorno del resultado al método
			return this.editarEgresoIngreso(TipoOperacion.Ingreso, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante, estatus, this._id_egreso_ingreso_concepto,
																	forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto, this._id_moneda, monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura,
																	this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, "", "", id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método Público encargado de Actualizar el Estatus de las Fichas de Ingreso a Aplicado
		/// </summary>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizarEstatusManualFichaIngresoAplicado(int id_usuario)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion(this._id_egreso_ingreso);

			//Inicializando bloque transaccional
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Recuperando valor límite tolerado para actualización de estatus
				decimal tolerancia = 0;
				Decimal.TryParse(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Monto Tolerancia Saldo Pago Cliente", this._id_compania), out tolerancia);

				//Si el saldo restante de la ficha es menor o igual al monto tolerado para la compañía
				if (ObtieneSaldoEgreso() <= tolerancia || tolerancia == 0)
				{
					//Actualizando estatus a Aplicado
					resultado = this.editarEgresoIngreso(TipoOperacion.Ingreso, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante, Estatus.Aplicada, this._id_egreso_ingreso_concepto,
																			forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto, this._id_moneda, monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura,
																			this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, "", "", id_usuario, this._habilitar);

					//Si no hubo problemas 
					if (resultado.OperacionExitosa)
						//Registrando bitácora de actualización Manual
						resultado = Global.Bitacora.InsertaBitacora(101, this._id_egreso_ingreso, 8938, ((byte)this.estatus).ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
				}
				else
					resultado = new RetornoOperacion(string.Format("El monto máximo permitido (saldo del pago) para esta acción es {0:c}", tolerancia));

				//Si no hay errores
				if (resultado.OperacionExitosa)
				{
					resultado = new RetornoOperacion(this._id_egreso_ingreso);
					scope.Complete();
				}
			}

			//Devolviendo resultado
			return resultado;
		}
		/// <summary>
		/// Método Público encargado de Actualizar el Estatus de las Fichas de Ingreso
		/// </summary>
		/// <param name="tipo_operacion">Tipo de Operación</param>
		/// <param name="estatus">Estatus a Actualizar</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaFichaIngresoEstatus(TipoOperacion tipo_operacion, Estatus estatus, int id_usuario)
		{
			//Retorno del resultado al método
			return this.editarEgresoIngreso(tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante, estatus, this._id_egreso_ingreso_concepto,
																	forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto, this._id_moneda, monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura,
																	this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, "", "", id_usuario, this._habilitar);
		}
		/// <summary>
		/// Realiza la actualización del egreso por depósito a estatus Aplicado y las actualizaciones subsecuentes.
		/// </summary>
		/// <param name="fecha_deposito">Fecha Depósito</param>
		/// <param name="id_cuenta_origen">Id de Cuenta de Origen</param>
		/// <param name="id_cuenta_destino">Id de Cuenta de Destino</param>
		/// <param name="transferencia_bancaria">Transferencia Bancaria </param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion ActualizarEstatusEgresoDepositado(DateTime fecha_deposito, int id_cuenta_origen, int id_cuenta_destino, string transferencia_bancaria, FormaPagoSAT forma_pago, int id_usuario)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion();

			//Validando que el registro pertenezca a un depósito y su estatus permita la actualización
			if (this._id_tabla == 51)
			{
				//Validando estatus adecuado
				if (this.estatus == Estatus.Capturada)
				{
					//Inicializando bloque transaccional
					using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						//Instanciando depósito
						using (SAT_CL.EgresoServicio.Deposito deposito = new EgresoServicio.Deposito(this._id_registro))
						{
							//Intsanciamos detalle liq
							using (SAT_CL.EgresoServicio.DetalleLiquidacion objDetalleLiq = new EgresoServicio.DetalleLiquidacion(deposito.id_deposito, 51))
							{
								//Si el deposito existe
								if (deposito.id_deposito > 0)
								{
									//Validando que el estatus se encuentre en Por Depositar
									if (deposito.Estatus == EgresoServicio.Deposito.EstatusDeposito.PorDepositar)
									{
										//Actualizando estatus
										resultado = deposito.ActualizaEstatusAPorLiquidar(fecha_deposito, id_cuenta_origen, id_cuenta_destino, id_usuario);
										//Si no hay errores
										if (resultado.OperacionExitosa)
										{
                                            //Actualizando información de registro egreso
                                            resultado = editarEgresoIngreso((TipoOperacion)this._id_tipo_operacion, this._secuencia_compania, this._id_compania, 51, deposito.id_deposito,
                                                            this._nombre_depositante, Estatus.Depositado, this._id_egreso_ingreso_concepto, forma_pago, id_cuenta_destino, id_cuenta_origen,
                                                            transferencia_bancaria, this._monto, this._id_moneda, this._monto_pesos, fecha_deposito, this._fecha_captura, this._bit_transferido_nuevo,
                                                            this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, transferencia_bancaria, "", id_usuario, this._habilitar);
											//resultado = actualizaEstatusEgresoIngreso(Estatus.Depositado, fecha_deposito, id_cuenta_destino, id_cuenta_origen, transferencia_bancaria, "", id_usuario);
										}
										else
											resultado = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", deposito.no_deposito, resultado.Mensaje));
									}
									//Si el etatus del depósito nopermite su actualización
									else
										resultado = new RetornoOperacion("El estatus del depósito no permite la actualización del mismo.");
								}
								else
									resultado = new RetornoOperacion("Depósito no encontrado.");


								//Si no hay errores
								if (resultado.OperacionExitosa)
								{
									//Actualizando información del depósito
									deposito.ActualizaDeposito();

									//Validando que sea Concepto de Diesel
									if (deposito.id_concepto == 9)
									{
										//Instanciando Vale
										using (SAT_CL.EgresoServicio.AsignacionDiesel vale_diesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneValePorDeposito(deposito.id_deposito))
										{
											//Validando que exista el Vale
											if (vale_diesel.habilitar)
											
												//Actualizando fecha de Carga Asignación de Diesel
												resultado = vale_diesel.EditaAsignacionDiesel(vale_diesel.nombre_operador_proveedor, vale_diesel.id_compania_emisor, vale_diesel.id_ubicacion_estacion,
																		vale_diesel.fecha_solicitud, deposito.fecha_deposito, vale_diesel.id_costo_diesel, vale_diesel.id_tipo_combustible, vale_diesel.id_factura,
																		vale_diesel.bit_transferencia_contable, vale_diesel.referencia, vale_diesel.id_lectura, vale_diesel.id_deposito, vale_diesel.tipo_vale,
																		vale_diesel.id_unidad_diesel, vale_diesel.objDetalleLiquidacion.cantidad, id_usuario);
											else
												//Instanciando Excepción
												resultado = new RetornoOperacion("No se puede Acceder al Vale de Diesel");
										}
									}


                                            //Si no hay errores
                                            if (resultado.OperacionExitosa)
									{
										//Instanciando Deposito
										resultado = new RetornoOperacion(deposito.id_deposito);

										//Declaramos Tipo
										MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;
										//De acuerdo aal tipo de asignación de deposito es Unidad
										if (objDetalleLiq.id_unidad != 0)
										
											//Asignamos Tipo de Unidad
											tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
										
										//Si la Asignación es a tercero
										else if (objDetalleLiq.id_proveedor_compania != 0)
										
											//Asignamos Tercero
											tipo = MovimientoAsignacionRecurso.Tipo.Tercero;

										//Enviamos Notificación de confirmación del mismo
										Global.NotificacionPush.Instance.NotificacionDepositoAnticiposYDiesel(MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objDetalleLiq.id_movimiento, tipo), 
                                                                        SAT_CL.Global.NotificacionPush.TipoNotificacionServicio.AnticipoDepositado, deposito.fecha_deposito, objDetalleLiq.monto);

                                        using (SAT_CL.EgresoServicio.AnticipoProgramado anticipoProgramado = new EgresoServicio.AnticipoProgramado(deposito.id_deposito, deposito.id_compania_emisor))
                                        {
                                            if (anticipoProgramado.habilitar)
                                            {
                                                resultado = anticipoProgramado.AtualizaReferenciaAnticipoProgramado(transferencia_bancaria, id_usuario);
                                            }
                                        }
                                        //Finalizando transacción
                                        scope.Complete();
									}
								}
							}
						}
					}
				}
				else
					resultado = new RetornoOperacion("El estatus del egreso no permite su actualización.");
			}
			else
				resultado = new RetornoOperacion("El registro no corresponde a un depósito.");

			//Devolviendo resultado
			return resultado;
		}

		/// <summary>
		/// Realiza la actualización del egreso por depósito a estatus Cancelado y las actualizaciones subsecuentes.
		/// </summary>
		/// <param name="razon_rechazo">Motivo del rechazo del depósito</param>
		/// <param name="id_usuario">Id de Usuario</param>
		/// <returns></returns>
		public RetornoOperacion ActualizarEstatusEgresoDepositoCancelado(string razon_rechazo, int id_usuario)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion();

			//Validando que el registro pertenezca a un depósito y su estatus permita la actualización
			if (this._id_tabla == 51)
			{
				//Validando estatus
				if (this.estatus != Estatus.Cancelada)
				{
					//Inicializando bloque transaccional
					using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						//Instanciando depósito
						using (SAT_CL.EgresoServicio.Deposito deposito = new EgresoServicio.Deposito(this._id_registro))
						{
							//Si el deposito existe
							if (deposito.habilitar)
							{
								//Validando que el estatus se encuentre en Por Depositar
								if (deposito.Estatus == EgresoServicio.Deposito.EstatusDeposito.PorDepositar)
								{
                                    //Validando Concepto de Proveedor
                                    using (EgresoServicio.ConceptoDeposito concepto = new EgresoServicio.ConceptoDeposito(deposito.id_concepto))
                                    {
                                        if (concepto.habilitar)
                                        {
                                            switch (concepto.descripcion.ToUpper())
                                            {
                                                case "ANTICIPO PROVEEDOR":
                                                    {
                                                        List<EgresoServicio.ServicioImportacionAnticipos> anticipos = EgresoServicio.ServicioImportacionAnticipos.ObtieneImportacionesAnticiposPrevios(deposito.id_deposito);
                                                        if (anticipos.Count > 0)
                                                        {
                                                            resultado = new RetornoOperacion(deposito.id_deposito);
                                                            //Recorriendo Ciclo
                                                            foreach (EgresoServicio.ServicioImportacionAnticipos anticipo in anticipos)
                                                            {
                                                                if (anticipo.habilitar && (anticipo.id_anticipo_finiquito_cc > 0 || anticipo.id_anticipo_finiquito_sc > 0))
                                                                
                                                                    //Quitando Anticipo Previo de la Relación - respetando el Finiquito que tenga
                                                                    resultado = anticipo.ActualizaDepositoPrevioImportacionAnticipos(0, id_usuario);
                                                                
                                                                else if (anticipo.habilitar && anticipo.id_anticipo_finiquito_cc == 0)
                                                                    
                                                                    //Quitando Anticipo Previo de la Relación
                                                                    resultado = anticipo.DeshabilitaServicioImportacionAnticipos(id_usuario);
                                                                else
                                                                    resultado = new RetornoOperacion("No se puede recuperar la relación del Anticipo");

                                                                //Validando Errores
                                                                if (!resultado.OperacionExitosa)
                                                                    break;
                                                            }

                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Actualizando estatus
                                                                resultado = deposito.ActualizaEstatusARegistrado(id_usuario);
                                                                if (resultado.OperacionExitosa && deposito.ActualizaDeposito())
                                                                {
                                                                    resultado = deposito.DeshabilitaDeposito(id_usuario);
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Deshabilitamos Autorizaciones
                                                                        resultado = Autorizacion.AutorizacionRealizada.DeshabilitaAutorizacionRealizada(51, deposito.id_deposito, id_usuario);
                                                                        //Si no hay errores
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Actualizando información de registro egreso
                                                                            resultado = editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante,
                                                                                            Estatus.Cancelada, this._id_egreso_ingreso_concepto, this.forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto,
                                                                                            this._id_moneda, this._monto_pesos, Fecha.ObtieneFechaEstandarMexicoCentro(), this._fecha_captura, this._bit_transferido_nuevo, this._id_transferido_nuevo,
                                                                                            this._bit_transferido_edicion, this._id_transferido_edicion, "", razon_rechazo, id_usuario, this._habilitar);
                                                                        }
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", deposito.no_deposito, resultado.Mensaje));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Actualizando estatus
                                                            resultado = deposito.ActualizaEstatusARegistrado(id_usuario);

                                                            //Validamos Resultado
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Deshabilitamos Autorizaciones
                                                                resultado = Autorizacion.AutorizacionRealizada.DeshabilitaAutorizacionRealizada(51, deposito.id_deposito, id_usuario);
                                                                //Si no hay errores
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Actualizando información de registro egreso
                                                                    resultado = editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante,
                                                                                    Estatus.Cancelada, this._id_egreso_ingreso_concepto, this.forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto,
                                                                                    this._id_moneda, this._monto_pesos, Fecha.ObtieneFechaEstandarMexicoCentro(), this._fecha_captura, this._bit_transferido_nuevo, this._id_transferido_nuevo,
                                                                                    this._bit_transferido_edicion, this._id_transferido_edicion, "", razon_rechazo, id_usuario, this._habilitar);
                                                                }
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", deposito.no_deposito, resultado.Mensaje));
                                                            }
                                                        }

                                                        break;
                                                    }
                                                case "FINIQUITO PROVEEDOR":
                                                    {
                                                        List<EgresoServicio.ServicioImportacionAnticipos> finiquitos = EgresoServicio.ServicioImportacionAnticipos.ObtieneImportacionesFiniquito(deposito.id_deposito);
                                                        if (finiquitos.Count > 0)
                                                        {
                                                            resultado = new RetornoOperacion(deposito.id_deposito);
                                                            //Recorriendo Ciclo
                                                            foreach (EgresoServicio.ServicioImportacionAnticipos finiquito in finiquitos)
                                                            {
																if (finiquito.habilitar)
																{
																	//Validando Finiquito CC
																	if (finiquito.id_anticipo_finiquito_cc > 0)
																		//Quitando el Finiquito de la Relación - respetando el Anticipo Previo que tenga
																		resultado = finiquito.ActualizaFiniquitoServicioImportacionAnticiposCC(0, id_usuario);
																	//Validando Finiquito CC
																	else if (finiquito.id_anticipo_finiquito_sc > 0)
																		//Quitando el Finiquito de la Relación - respetando el Anticipo Previo que tenga
																		resultado = finiquito.ActualizaFiniquitoServicioImportacionAnticiposSC(0, id_usuario);

																	//Validando Existencia de Anticipos
																	if (finiquito.ActualizaServicioImportacionAnticipos() && finiquito.id_anticipo_previo == 0)
																	{
																		if (finiquito.id_anticipo_finiquito_cc == 0 && finiquito.id_anticipo_finiquito_sc == 0)
																			//Quitando el Finiquito de la Relación
																			resultado = finiquito.DeshabilitaServicioImportacionAnticipos(id_usuario);
																	}
																}
																else
																	resultado = new RetornoOperacion("No se puede recuperar la relación del Finiquito");

                                                                //Validando Errores
                                                                if (!resultado.OperacionExitosa)
                                                                    break;
                                                            }

                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Actualizando estatus
                                                                resultado = deposito.ActualizaEstatusARegistrado(id_usuario);
                                                                if (resultado.OperacionExitosa && deposito.ActualizaDeposito())
                                                                {
                                                                    resultado = deposito.DeshabilitaDeposito(id_usuario);
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Deshabilitamos Autorizaciones
                                                                        resultado = Autorizacion.AutorizacionRealizada.DeshabilitaAutorizacionRealizada(51, deposito.id_deposito, id_usuario);
                                                                        //Si no hay errores
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Actualizando información de registro egreso
                                                                            resultado = editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante,
                                                                                            Estatus.Cancelada, this._id_egreso_ingreso_concepto, this.forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto,
                                                                                            this._id_moneda, this._monto_pesos, Fecha.ObtieneFechaEstandarMexicoCentro(), this._fecha_captura, this._bit_transferido_nuevo, this._id_transferido_nuevo,
                                                                                            this._bit_transferido_edicion, this._id_transferido_edicion, "", razon_rechazo, id_usuario, this._habilitar);
                                                                        }
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", deposito.no_deposito, resultado.Mensaje));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        //Actualizando estatus
                                                        resultado = deposito.ActualizaEstatusARegistrado(id_usuario);

                                                        //Validamos Resultado
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Deshabilitamos Autorizaciones
                                                            resultado = Autorizacion.AutorizacionRealizada.DeshabilitaAutorizacionRealizada(51, deposito.id_deposito, id_usuario);
                                                            //Si no hay errores
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Actualizando información de registro egreso
                                                                resultado = editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante,
                                                                                Estatus.Cancelada, this._id_egreso_ingreso_concepto, this.forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto,
                                                                                this._id_moneda, this._monto_pesos, Fecha.ObtieneFechaEstandarMexicoCentro(), this._fecha_captura, this._bit_transferido_nuevo, this._id_transferido_nuevo,
                                                                                this._bit_transferido_edicion, this._id_transferido_edicion, "", razon_rechazo, id_usuario, this._habilitar);
                                                            }
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", deposito.no_deposito, resultado.Mensaje));
                                                        }
                                                        break;
                                                    }
                                            }
                                            using (SAT_CL.EgresoServicio.AnticipoProgramado anticipoProgramado = new EgresoServicio.AnticipoProgramado(deposito.id_deposito, deposito.id_compania_emisor))
                                            {
                                                if (anticipoProgramado.habilitar)
                                                {
                                                    resultado = anticipoProgramado.DeshabilitaAnticipoProgramado(id_usuario);
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        using (SAT_CL.EgresoServicio.Deposito objDeposito = new EgresoServicio.Deposito(this._id_registro))
                                                        {
                                                            //resultado = anticipoProgramado.EditarAnticipoProgramado(deposito.id_deposito, anticipoProgramado.id_compania, anticipoProgramado.id_servicio, anticipoProgramado.id_concepto, anticipoProgramado.monto, anticipoProgramado.fecha_ejecucion, razon_rechazo, id_usuario);
                                                            resultado = objDeposito.DeshabilitaDeposito(id_usuario);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            resultado = new RetornoOperacion("No se puede recuperar el Concepto");
                                    }
								}
								//Si el etatus del depósito nopermite su actualización
								else
									resultado = new RetornoOperacion("El estatus del depósito no permite la actualización del mismo.");
							}
							else
								resultado = new RetornoOperacion("Depósito no encontrado.");
						}

						//Si no hay errores
						if (resultado.OperacionExitosa)
                            //Finalizando transacción                            
                        scope.Complete();
					}
				}
				else
					resultado = new RetornoOperacion("El estatus del egreso no permite su actualización.");
			}
			else
				resultado = new RetornoOperacion("El registro no corresponde a un depósito.");

			//Devolviendo resultado
			return resultado;
		}
		/// <summary>
		///Actualiza la Liquidación a Estatus Depósitado
		/// </summary>
		/// <param name="fecha_deposito">Fecha en la que se Depósita la Liquidación</param>
		/// <param name="id_cuenta_origen">Id Cuenta Origen</param>
		/// <param name="id_cuenta_destino">Id Cuenta Destino</param>
		/// <param name="transferencia_bancaria">Referencia del depósito, generalmente es el número de transferencia bancaria asociada</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion ActualizarEstatusEgresoLiquidacionAceptada(DateTime fecha_deposito, int id_cuenta_origen, int id_cuenta_destino, string transferencia_bancaria,
                                                                        FormaPagoSAT forma_pago, int id_usuario)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion();

			//Validando que el registro pertenezca a un depósito y su estatus permita la actualización
			if (this._id_tabla == 82)
			{
				//Validando estatus adecuado
				if (this.estatus == Estatus.Capturada)
				{
					//Inicializando Bloque Transacional
					using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						//Instanciando Liquidación
						using (SAT_CL.Liquidacion.Liquidacion liq = new Liquidacion.Liquidacion(this.id_registro))
						{
							//Validando que exista la Liquidación
							if (liq.id_liquidacion > 0)
							{
								//Validando que la Liquidación se encuentre Cerrada
								if (liq.estatus == Liquidacion.Liquidacion.Estatus.Liquidado)
								{
									//Actualizando Estatus de la Liquidación
									resultado = liq.ActualizaEstatusADepositado(id_usuario);

									//Validando que se haya actualizado la Liquidación
									if (resultado.OperacionExitosa)
									{
                                        //Actualizando Estatus del Egreso - Ingreso
                                        //Actualizando información de registro egreso
                                        resultado = editarEgresoIngreso((TipoOperacion)this._id_tipo_operacion, this._secuencia_compania, this._id_compania, 82, liq.id_liquidacion,
                                                        this._nombre_depositante, Estatus.Depositado, this._id_egreso_ingreso_concepto, forma_pago, id_cuenta_destino, id_cuenta_origen,
                                                        transferencia_bancaria, this._monto, this._id_moneda, this._monto_pesos, fecha_deposito, this._fecha_captura, this._bit_transferido_nuevo,
                                                        this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, transferencia_bancaria, "", id_usuario, this._habilitar);
                                        //resultado = actualizaEstatusEgresoIngreso(Estatus.Depositado, fecha_deposito, id_cuenta_destino, id_cuenta_origen, transferencia_bancaria, "", id_usuario);

										//Validando que se Actualizo el Egreso - Ingreso
										if (resultado.OperacionExitosa)

											//Completando Transacción
											trans.Complete();
									}

								}
								else
									//Instanciando Excepcion
									resultado = new RetornoOperacion("La Liquidación debe de Estar Cerrada");
							}
							else
								//Instanciando Excepcion
								resultado = new RetornoOperacion("No se puede acceder a la Liquidación");
						}
					}
				}
				else
					//Instanciando Excepcion
					resultado = new RetornoOperacion("El Estatus no Permite su Edición");
			}
			else
				//Instanciando Excepcion
				resultado = new RetornoOperacion("El Registro no es de una Liquidación");

			//Devolviendo Resultado Obtenidos
			return resultado;
		}
		/// <summary>
		/// Actualiza el Egreso
		/// </summary>
		/// <param name="estatus">Estatus de Egreso</param>
		/// <param name="fecha_egreso_ingreso">fecha de Egreso</param>
		/// <param name="id_cuenta_destino">Cuenta Destino</param>
		/// <param name="id_cuenta_origen">Cuenta Origen</param>
		/// <param name="transferencia_bancaria">Referencia del depósito, generalmente es el número de transferencia bancaria asociada</param>
		/// <param name="razon_rechazo">Razón de rechazo del egreso</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		private RetornoOperacion actualizaEstatusEgresoIngreso(Estatus estatus, DateTime fecha_egreso_ingreso, int id_cuenta_destino, int id_cuenta_origen, string transferencia_bancaria, string razon_rechazo, int id_usuario)
		{
			//Actualizando información de registro egreso
			return editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante,
																										estatus, this._id_egreso_ingreso_concepto, this.forma_pago, id_cuenta_destino, id_cuenta_origen, this._num_cheque, this._monto,
																										this._id_moneda, this._monto_pesos, fecha_egreso_ingreso, this._fecha_captura, this._bit_transferido_nuevo, this._id_transferido_nuevo,
																										this._bit_transferido_edicion, this._id_transferido_edicion, transferencia_bancaria, razon_rechazo, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Actualiza el Egreso a Cancelado
		/// </summary>
		/// <param name="razon_rechazo">Razón de rechazo del pago de liquidación</param>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion ActualizarEstatusEgresoLiquidacionCancelada(string razon_rechazo, int id_usuario)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion();

			//Validando que el registro pertenezca a un depósito y su estatus permita la actualización
			if (this._id_tabla == 82)
			{
				//Validando estatus adecuado
				if (this.estatus == Estatus.Capturada)
				{
					//Inicializando Bloque Transacional
					using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
					{
						//Instanciando Liquidación
						using (SAT_CL.Liquidacion.Liquidacion liq = new Liquidacion.Liquidacion(this.id_registro))
						{
							//Validando que exista la Liquidación
							if (liq.id_liquidacion > 0)
							{
								//Actualizando Estatus de la Liquidación
								resultado = liq.AbreLiquidacion(id_usuario);

								//Validando que se haya actualizado la Liquidación
								if (resultado.OperacionExitosa)
								{
									//Actualizando Estatus del Egreso - Ingreso
									this.editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro,
																					this._nombre_depositante, Estatus.Cancelada, this._id_egreso_ingreso_concepto, this.forma_pago,
																					this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto, this._id_moneda,
																					this._monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura, this._bit_transferido_nuevo,
																					this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, "", razon_rechazo, id_usuario, this._habilitar);

									//Validando que se Actualizo el Egreso - Ingreso
									if (resultado.OperacionExitosa)

										//Completando Transacción
										trans.Complete();
								}
							}
							else
								//Instanciando Excepcion
								resultado = new RetornoOperacion("No se puede acceder a la Liquidación");
						}
					}
				}
				else
					//Instanciando Excepcion
					resultado = new RetornoOperacion("El Estatus no Permite su Edición");
			}
			else
				//Instanciando Excepcion
				resultado = new RetornoOperacion("El Registro no es de una Liquidación");

			//Devolviendo Resultado Obtenidos
			return resultado;
		}

		/// <summary>
		/// Método que permite deshabilitar un registro de un egreso - ingreso
		/// </summary>
		/// <param name="id_usuario">Id que permite identificar al usuario ue realizo acciones sobre el registro</param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitarEgresoIngreso(int id_usuario)
		{
			//Retorna e invoca el metodo privado editarEgresoIngreso
			return this.editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante,
																			this.estatus, this._id_egreso_ingreso_concepto, this.forma_pago, this._id_cuenta_destino, this._id_cuenta_origen, this._num_cheque,
																			this._monto, this._id_moneda, this._monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura, this._bit_transferido_nuevo,
																			this._id_transferido_nuevo, this._bit_transferido_edicion, this._id_transferido_edicion, new Global.Referencia(this._id_transferencia_bancaria).valor, "", id_usuario, false);
		}
		/// <summary>
		/// Método encargado de Actualizar los Atributos
		/// </summary>
		/// <returns></returns>
		public bool ActualizaEgresoIngreso()
		{
			//Devolviendo Resultado Obtenido
			return cargaAtributoInstancia(this._id_egreso_ingreso);
		}
		/// <summary>
		/// Método Público encargado de Obtener las Fichas de Ingreso
		/// </summary>
		/// <param name="id_cliente">Cliente</param>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="no_ficha">No. de Ficha</param>
		/// <param name="referencia">Referencia Interbancaria</param>
		/// <returns></returns>
		public static DataTable ObtieneFichasIngresoConSaldo(int id_cliente, int id_compania, int no_ficha, string referencia)
		{
			//Declarando Objeto de Retorno
			DataTable dtFichasIngreso = null;

			//Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
			object[] param = { 4, 0, 0, no_ficha, id_compania, 0, id_cliente, "", 0, 0, 0, 0, 0, referencia, 0.0m, 0, 0.0m,
															 null, null, false, 0, false, 0, 0, false, "", "" };

			//Instanciando Resultado
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Validando que existan Registros
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

					//Asignando Valor Obtenido
					dtFichasIngreso = ds.Tables["Table"];
			}

			//Devolviendo Resultado Obtenido
			return dtFichasIngreso;
		}
		/// <summary>
		/// Método que obtiene el encabezado de una ficha de ingreso
		/// </summary>
		/// <param name="id_egreso_ingreso">Identificador que permite realizar la busqueda de la ficha de ingreso</param>
		/// <returns></returns>
		public static DataTable EncabezadoFichaIngreso(int id_egreso_ingreso)
		{
			//Creación de una datatable
			DataTable dtEncabezadoFichaIngreso = null;
			//Creación del arreglo param
			object[] param ={ 6, id_egreso_ingreso, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m,
															 null, null, false, 0, false, 0, 0, false, "", "" };
			//Crea dataset y almacena el resultado de método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Valida los datos del dataset
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
					dtEncabezadoFichaIngreso = DS.Tables["Table"];
			}
			//Retorno de la tabla al método 
			return dtEncabezadoFichaIngreso;
		}
		/// <summary>
		/// Método encargado de Obtener los Egresos de Anticipos a Proveedor
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_proveedor">Proveedor</param>
		/// <returns></returns>
		public static DataTable ObtieneAnticiposYNotasCreditoProveedor(int id_compania, int id_proveedor)
		{
			//Declarando Objeto de Retorno
			DataTable dtAnticiposProveedor = null;

			//Creación del arreglo param
			object[] param = { 7, 0, 0, 0, id_compania, 0, id_proveedor, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m, null, null, false, 0, false, 0, 0, false, "", "" };

			//Crea dataset y almacena el resultado de método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Valida los datos del dataset
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

					//Asignando Resultado Obtenido
					dtAnticiposProveedor = DS.Tables["Table"];
			}

			//Devolviendo Resultado Obtenido
			return dtAnticiposProveedor;
		}
		/// <summary>
		/// Método encargado de Obtener los Egresos de Anticipos a Proveedor
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_proveedor">Proveedor</param>
		/// <param name="depositos">Factura de Proveedor</param>
		/// <returns></returns>
		public static DataTable ObtieneAnticiposProveedor(int id_compania, int id_proveedor, string depositos)
		{
			//Declarando Objeto de Retorno
			DataTable dtAnticiposProveedor = null;

			//Creación del arreglo param
			object[] param = { 9, 0, 0, 0, id_compania, 0, id_proveedor, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m, null, null, false, 0, false, 0, 0, false, depositos, "" };

			//Crea dataset y almacena el resultado de método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Valida los datos del dataset
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

					//Asignando Resultado Obtenido
					dtAnticiposProveedor = DS.Tables["Table"];
			}

			//Devolviendo Resultado Obtenido
			return dtAnticiposProveedor;
		}
		/// <summary>
		/// Método encargado de Obtener el Saldo Disponible del Egreso
		/// </summary>
		/// <returns></returns>
		public decimal ObtieneSaldoEgreso()
		{
			//Declarando Objeto de Retorno
			decimal saldo_actual = 0.00M;

			//Creación del arreglo param
			object[] param = { 10, this._id_egreso_ingreso, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m, null, null, false, 0, false, 0, 0, false, "", "" };

			//Crea dataset y almacena el resultado de método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
				{
					//Recorriendo Filas
					foreach (DataRow dr in DS.Tables["Table"].Rows)
					{
						//Asignando Valor
						saldo_actual = Convert.ToDecimal(dr["SaldoDisponible"]);

						//Terminando Ciclo
						break;
					}
				}
			}

			//Devolviendo Resultado Obtenido
			return saldo_actual;
		}
		/// <summary>
		/// Método encargado de Depositar el Anticipo (Y aplicar las Facturas que tiene ligadas)
		/// </summary>
		/// <param name="id_cuenta_origen">Cuenta Origen</param>
		/// <param name="id_cuenta_destino">Cuenta Destino</param>
		/// <param name="fecha_deposito">Fecha de Deposito</param>
		/// <param name="referencia">Referencia</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaEstatusDepositoEgreso(int id_cuenta_origen, int id_cuenta_destino, DateTime fecha_deposito,
															   FormaPagoSAT forma_pago, string referencia, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Declarando Variables Auxiliares
			decimal monto_x_aplicar = 0.00M, monto_disp_factura = 0.00M, monto_disp_egreso = 0.00M;
			SAT_CL.CXP.FacturadoProveedor.EstatusFactura estatus_factura;
			SAT_CL.Bancos.EgresoIngreso.Estatus estatus_egreso;

			//Validamos  que exista Cuenta
			if (id_cuenta_destino != 0)
			{
				//Inicializando Transacción
				using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					//Depositando Egreso
					result = this.ActualizarEstatusEgresoDepositado(fecha_deposito, id_cuenta_origen, id_cuenta_destino, referencia.ToUpper(), forma_pago, id_usuario);

					//Validando Operación Exitosa
					if (result.OperacionExitosa)
					{
						//Obteniendo Facturas Ligadas a la Entidad
						using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(51, this._id_registro, false))
						{
							//Validando que existan Registros
							if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
							{
								//Recorriendo Facturas Ligadas
								foreach (DataRow dr in dtFacturasLigadas.Rows)
								{
									//Actualizando Atributos
									this.ActualizaEgresoIngreso();

									//Instanciando Relación de la Factura
									using (SAT_CL.CXP.FacturadoProveedorRelacion fpr = new SAT_CL.CXP.FacturadoProveedorRelacion(Convert.ToInt32(dr["IdFPR"])))
									{
										//Validando que exista la relación
										if (fpr.habilitar)
										{
											//Instanciando Factura
											using (SAT_CL.CXP.FacturadoProveedor fp = new SAT_CL.CXP.FacturadoProveedor(fpr.id_factura_proveedor))
											{
												//Validando que exista la factura
												if (fp.habilitar && ((SAT_CL.CXP.FacturadoProveedor.EstatusFactura)fp.id_estatus_factura == SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Aceptada
																						|| (SAT_CL.CXP.FacturadoProveedor.EstatusFactura)fp.id_estatus_factura == SAT_CL.CXP.FacturadoProveedor.EstatusFactura.AplicadaParcial))
												{
													//Inicializando Variables
													monto_x_aplicar = 0.00M;
													monto_disp_egreso = this.ObtieneSaldoEgreso();
													monto_disp_factura = SAT_CL.CXP.FacturadoProveedor.ObtieneMontoPendienteAplicacion(fp.id_factura);
													estatus_factura = (CXP.FacturadoProveedor.EstatusFactura)fp.id_estatus_factura;
													estatus_egreso = (Estatus)this._id_estatus;

													//Si el saldo de la Factura es menor al del deposito
													if (monto_disp_factura < monto_disp_egreso)
													{
														//Actualizando Variables
														estatus_factura = SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Liquidada;
														estatus_egreso = Estatus.AplicadaParcial;
														monto_x_aplicar = monto_disp_factura;
													}
													//Si el saldo de la Factura es igual al del deposito
													else if (monto_disp_factura == monto_disp_egreso)
													{
														//Actualizando Variables
														estatus_factura = SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Liquidada;
														estatus_egreso = Estatus.Aplicada;
														monto_x_aplicar = monto_disp_factura;
													}
													//Si el saldo de la factura es mayor al del deposito
													else if (monto_disp_factura > monto_disp_egreso)
													{
														//Actualizando Variables
														estatus_factura = SAT_CL.CXP.FacturadoProveedor.EstatusFactura.AplicadaParcial;
														estatus_egreso = Estatus.Aplicada;
														monto_x_aplicar = monto_disp_egreso;
													}

													//Insertando Aplicación
													result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(72, fp.id_factura, this._id_egreso_ingreso,
																																							monto_x_aplicar, fecha_deposito, false, 0, false, 0, id_usuario);

													//Validando Operación Exitosa
													if (result.OperacionExitosa)
													{
														//Actualizando Estatus de Factura
														result = fp.ActualizaEstatusFacturadoProveedor(estatus_factura, id_usuario);

														//Validando Operación Exitosa
														if (result.OperacionExitosa)
														{
															//Actualizando Estatus de la Ficha
															result = this.ActualizaFichaIngresoEstatus(TipoOperacion.Egreso, estatus_egreso, id_usuario);

															//Si el resultado no fue exitoso
															if (!result.OperacionExitosa)

																//Terminando Ciclo
																break;
														}
													}
													else
														//Terminando Ciclo
														break;

												}
												else
												{
													//Instanciando Excepción
													result = new RetornoOperacion("La Factura no esta Aceptada");

													//Terminando Ciclo
													break;
												}
											}
										}
										else
										{
											//Instanciando Excepción
											result = new RetornoOperacion("No se puede acceder a la relación de la Factura");

											//Terminando Ciclo
											break;
										}
									}
								}
							}
							else
								//Instanciando Excepción
								result = new RetornoOperacion("El Deposito no tiene Facturas Ligadas", true);
						}
					}

					//Validando Operación Exitosa
					if (result.OperacionExitosa)
					{
						//Instanciando Resultado Exitoso
						result = new RetornoOperacion(this._id_egreso_ingreso);

						//Completando Transacción
						trans.Complete();
					}
				}
			}
			else
				//Instanciando Excepción
				result = new RetornoOperacion("La Cuenta es obligatoria, actualice y vuelva a intentarlo.");

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Depositar la Liquidación (y aplicar las facturas ligadas)
		/// </summary>
		/// <param name="id_cuenta_origen">Cuenta Origen</param>
		/// <param name="id_cuenta_destino">Cuenta Destino</param>
		/// <param name="fecha_deposito">Fecha de Deposito</param>
		/// <param name="referencia">Referencia</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaEstatusLiquidacionEgreso(int id_cuenta_origen, int id_cuenta_destino, DateTime fecha_deposito,
															FormaPagoSAT forma_pago, string referencia, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Declarando Variables Auxiliares
			decimal monto_x_aplicar = 0.00M, monto_disp_factura = 0.00M, monto_disp_egreso = 0.00M;
			SAT_CL.CXP.FacturadoProveedor.EstatusFactura estatus_factura;
			SAT_CL.Bancos.EgresoIngreso.Estatus estatus_egreso;

			//Validamos  que exista Cuenta
			if (id_cuenta_destino != 0)
			{
				//Inicializando Transacción
				using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					//Validando que el monto sea mayor a 0
					if (this._monto > 0)

						//Depositando Egreso
						result = this.ActualizarEstatusEgresoLiquidacionAceptada(fecha_deposito, _id_cuenta_origen, id_cuenta_destino, referencia.ToUpper(), forma_pago, id_usuario);
					else
						//Instanciando Excepción
						result = new RetornoOperacion("No se puede Depositar una Liquidación en menor a '0'");

					//Validando Operación Exitosa
					if (result.OperacionExitosa)
					{
						//Obteniendo Facturas Ligadas a la Entidad
						using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(82, this._id_registro, false))
						{
							//Validando que existan Registros
							if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
							{
								//Recorriendo Facturas Ligadas
								foreach (DataRow dr in dtFacturasLigadas.Rows)
								{
									//Actualizando Atributos
									this.ActualizaEgresoIngreso();

									//Instanciando Relación de la Factura
									using (SAT_CL.CXP.FacturadoProveedorRelacion fpr = new SAT_CL.CXP.FacturadoProveedorRelacion(Convert.ToInt32(dr["IdFPR"])))
									{
										//Validando que exista la relación
										if (fpr.habilitar)
										{
											//Instanciando Factura
											using (SAT_CL.CXP.FacturadoProveedor fp = new SAT_CL.CXP.FacturadoProveedor(fpr.id_factura_proveedor))
											{
												//Validando que exista la factura
												if (fp.habilitar && ((SAT_CL.CXP.FacturadoProveedor.EstatusFactura)fp.id_estatus_factura == SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Aceptada
																						|| (SAT_CL.CXP.FacturadoProveedor.EstatusFactura)fp.id_estatus_factura == SAT_CL.CXP.FacturadoProveedor.EstatusFactura.AplicadaParcial))
												{
													//Inicializando Variables
													monto_x_aplicar = 0.00M;
													monto_disp_egreso = this.ObtieneSaldoEgreso();
													monto_disp_factura = SAT_CL.CXP.FacturadoProveedor.ObtieneMontoPendienteAplicacion(fp.id_factura);
													estatus_factura = (CXP.FacturadoProveedor.EstatusFactura)fp.id_estatus_factura;
													estatus_egreso = (Estatus)this._id_estatus;

													//Si el saldo de la Factura es menor al del deposito
													if (monto_disp_factura < monto_disp_egreso)
													{
														//Actualizando Variables
														estatus_factura = SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Liquidada;
														estatus_egreso = Estatus.AplicadaParcial;
														monto_x_aplicar = monto_disp_factura;
													}
													//Si el saldo de la Factura es igual al del deposito
													else if (monto_disp_factura == monto_disp_egreso)
													{
														//Actualizando Variables
														estatus_factura = SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Liquidada;
														estatus_egreso = Estatus.Aplicada;
														monto_x_aplicar = monto_disp_factura;
													}
													//Si el saldo de la factura es mayor al del deposito
													else if (monto_disp_factura > monto_disp_egreso)
													{
														//Actualizando Variables
														estatus_factura = SAT_CL.CXP.FacturadoProveedor.EstatusFactura.AplicadaParcial;
														estatus_egreso = Estatus.Aplicada;
														monto_x_aplicar = monto_disp_egreso;
													}

													//Insertando Aplicación
													result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(72, fp.id_factura, this._id_egreso_ingreso,
																																							monto_x_aplicar, fecha_deposito, false, 0, false, 0, id_usuario);

													//Validando Operación Exitosa
													if (result.OperacionExitosa)
													{
														//Actualizando Estatus de Factura
														result = fp.ActualizaEstatusFacturadoProveedor(estatus_factura, id_usuario);

														//Validando Operación Exitosa
														if (result.OperacionExitosa)
														{
															//Actualizando Estatus de la Ficha
															result = this.ActualizaFichaIngresoEstatus(TipoOperacion.Egreso, estatus_egreso, id_usuario);

															//Si el resultado no fue exitoso
															if (!result.OperacionExitosa)

																//Terminando Ciclo
																break;
														}
													}
													else
														//Terminando Ciclo
														break;

												}
												else
												{
													//Instanciando Excepción
													result = new RetornoOperacion("La Factura no esta Aceptada");

													//Terminando Ciclo
													break;
												}
											}
										}
										else
										{
											//Instanciando Excepción
											result = new RetornoOperacion("No se puede acceder a la relación de la Factura");

											//Terminando Ciclo
											break;
										}
									}
								}
							}
							else
								//Instanciando Excepción
								result = new RetornoOperacion("La Liquidación no tiene Facturas Ligadas", true);
						}
					}

					//Validando Operación Exitosa
					if (result.OperacionExitosa)
					{
						//Instanciando Resultado Exitoso
						result = new RetornoOperacion(this._id_egreso_ingreso);

						//Completando Transacción
						trans.Complete();
					}
				}
			}
			else
				//Instanciando Excepción
				result = new RetornoOperacion("La Cuenta es obligatoria, actualice y vuelva a intentarlo.");

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Obtener los Servicios por Entidad
		/// </summary>
		/// <param name="id_egreso_ingreso"></param>
		/// <returns></returns>
		public static DataTable ObtieneServiciosEntidad(int id_entidad, int id_registro)
		{
			//Declarando Objeto de Retorno
			DataTable dtServicios = null;

			//Creación del arreglo param
			object[] param = { 11, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m, null, null, false, 0, false, 0, 0, false, id_entidad, id_registro };

			//Crea dataset y almacena el resultado de método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Valida los datos del dataset
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

					//Asignando Resultado Obtenido
					dtServicios = DS.Tables["Table"];
			}

			//Devolviendo Resultado Obtenido
			return dtServicios;
		}
		/// <summary>
		/// Recupera los elementos FI que se encuentran disponibles para agregar a un CFDI de Recepción de Pagos
		/// </summary>
		/// <param name="id_compania">Id de Compañía que emitirá el CFDI</param>
		/// <param name="id_cliente">Id de Cliente que realizó el pago</param>
		/// <returns></returns>
		public static DataTable ObtieneFIDisponiblesParaCFDIRecepcionPagos(int id_compania, int id_cliente)
		{
			DataTable mitPagosDisponibles = null;

			object[] param = { 12, 0, 0, 0, id_compania, 0, id_cliente, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m, null, null, false, 0, false, 0, 0, false, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
			{
				//Valida los datos del dataset
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					mitPagosDisponibles = DS.Tables["Table"];
			}
			return mitPagosDisponibles;
		}
		
		/// <summary>
		/// Recupera los elementos FI que se encuentran disponibles para agregar a un CFDI de Recepción de Pagos
		/// </summary>
		/// <param name="idCompania">Id de Compañía que emitirá el CFDI</param>
		/// <param name="id_cliente">Id de Cliente que realizó el pago</param>
		/// <returns></returns>
		public static DataTable ObtienePagos(int idEgreso, int idCompania)
		{
			DataTable dtPagos = null;

			object[] param = { 13, idEgreso, 0, 0, idCompania, 0, 0, "", 0, 0, 0, 0, 0, "", 0.0m, 0, 0.0m, null, null, false, 0, false, 0, 0, false, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					dtPagos = DS.Tables["Table"];
			
			return dtPagos;
		}

		/// <summary>
		/// Método encargado de Editar el Depósito
		/// </summary>
		/// <param name="id_cliente_receptor">Id Cliente</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditaCuentaDestino(int id_cuenta_destino, int id_usuario)
		{
			RetornoOperacion resultado = new RetornoOperacion();
			resultado = this.editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante, estatus,this._id_egreso_ingreso_concepto, this.forma_pago, id_cuenta_destino, this._id_cuenta_origen, this._num_cheque, this._monto, this._id_moneda,this._monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura, this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion,this._id_transferido_edicion, "", "", id_usuario, this._habilitar);
			return resultado;
		}

		/// <summary>
		/// Método encargado de Editar el Depósito
		/// </summary>
		/// <param name="id_cliente_receptor">Id Cliente</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion RegresaDeposito(int id_usuario)
		{
			RetornoOperacion resultado = new RetornoOperacion();
			resultado = this.editarEgresoIngreso(this.tipo_operacion, this._secuencia_compania, this._id_compania, this._id_tabla, this._id_registro, this._nombre_depositante, Estatus.Capturada,this._id_egreso_ingreso_concepto, this.forma_pago, 0, 0, this._num_cheque, this._monto, this._id_moneda,this._monto_pesos, this._fecha_egreso_ingreso, this._fecha_captura, this._bit_transferido_nuevo, this._id_transferido_nuevo, this._bit_transferido_edicion,this._id_transferido_edicion, "", "", id_usuario, this._habilitar);
			return resultado;
		}
		#endregion

		#region CFDI V3.3 (Comprobante Recepción Pagos)

		/// <summary>
		/// Realiza la transferencia de la información necesaria de una FI (o conjunto de las mismas) hacia el esquema de CFDI V3.3 para Comprobante de Recepción de Pagos V1.0
		/// </summary>
		/// <param name="lista_FI">Conjunto de FI (pagos de cliente) que serán contemplados en el CFDI</param>
		/// <param name="id_sucursal">Id de Sucursal</param>
		/// <param name="id_uso_cfdi_receptor">Id de Uso CFDI Receptor</param>
		/// <param name="id_usuario">Id de Usuario que realiza la operación</param>
		/// <returns></returns>
		public static RetornoOperacion ImportarFIComprobanteV3_3(List<KeyValuePair<int, int>> lista_FI, int id_sucursal, int id_uso_cfdi_receptor, int id_usuario)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion(1);
			int id_compania_uso = 0, id_cliente = 0;

			//Validando que existan elementos por transferir
			if (lista_FI != null)
			{
				if (lista_FI.Count > 0)
				{
					//Si no hay elementos en 0
					if (lista_FI.Count(fi => fi.Key > 0) > 0)
					{
						//Inicializando transacción
						using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
						{
							//Valida que las FI sean todas del tipo Ingreso y cobranza
							foreach (KeyValuePair<int, int> fi in lista_FI)
							{
								//Instanciando FI
								using (EgresoIngreso i = new EgresoIngreso(fi.Key))
								{

									//Si el registro NO es del tipo ingreso
									if (i.tipo_operacion != TipoOperacion.Ingreso)
										resultado = new RetornoOperacion(string.Format("El elemento '{0}' no es un 'Ingreso'.", i.secuencia_compania));

									//Si no hay errores con el tipo de elemento (FI)
									if (resultado.OperacionExitosa)
									{
										//Instanciando concepto de ingreso
										using (EgresoIngresoConcepto ic = new EgresoIngresoConcepto(i.id_egreso_ingreso_concepto))
											//Si no es del tipo cobranza
											if (!ic.descripcion_concepto.ToUpper().Contains("COBRANZA"))
												resultado = new RetornoOperacion(string.Format("El Ingreso '{0}' no es del tipo 'Cobranza'.", i.secuencia_compania));
									}

									//Si no hay problemas con el concepto del ingreso
									if (resultado.OperacionExitosa)
									{
										//Validando estatus de Timbrado de esta FI
										using (fe33.EgresoIngresoComprobante ic = fe33.EgresoIngresoComprobante.ObtieneComprobanteActivoEgresoIngreso(i.id_egreso_ingreso))
										{
											//Si hay un elemento activo
											if (ic != null)
											{
												//Instanciando el CFDI
												using (fe33.Comprobante cfdiPagoActivo = new fe33.Comprobante(ic.id_comprobante_pago))
												{
													if (cfdiPagoActivo.id_comprobante33 > 0)
														//Señalando el error
														resultado = new RetornoOperacion(string.Format("El Ingreso '{0}' ya se encuentra en el CFDI de Recepción de Pagos '{1}{2}'. Primero debe cancelar ese CFDI.", i.secuencia_compania, cfdiPagoActivo.serie, cfdiPagoActivo.folio));
												}
											}
										}
									}

									//Asignando Id de Compañía EMisor del CFDI y su Receptor
									id_cliente = i.id_registro;
									id_compania_uso = i.id_compania;
								}
								//Si hay errores
								if (!resultado.OperacionExitosa)
									//Terminando iteración
									break;
							}

							//Si no hay errores de validación en el conjunto de FI
							if (resultado.OperacionExitosa)
							{
								//Recuperando el conjunto de datos generales del CFDI de Comprobante de Pagos
								using (DataSet ds = fe33.Reporte.ObtienesDatosEncabezadoFIFacturaElectronicaRecepcionPago(lista_FI[0].Key))
								{
									//Si hay datos
									if (Validacion.ValidaOrigenDatos(ds))
									{
										//Realizando operaciones de importación
										resultado = fe33.Comprobante.ImportaComprobante_V3_3_ReciboPago_V1_0(id_cliente, ds.Tables["Table"], ds.Tables["Table1"], lista_FI, id_compania_uso, id_sucursal, id_uso_cfdi_receptor, id_usuario);

										//Si no hay errores
										if (resultado.OperacionExitosa)
											//Confirmando cambios realizados
											scope.Complete();
									}
								}
							}
						}
					}
					else
						resultado = new RetornoOperacion("La lista de pagos contiene uno o más elementos con referencia '0'.");

				}
				else
					resultado = new RetornoOperacion("No se ha definido un pago o conjunto de pagos por asociar.");
			}
			else
				resultado = new RetornoOperacion("No se ha definido un pago o conjunto de pagos por asociar.");

			//Devolviendo resultado
			return resultado;
		}

		#endregion

	}
}