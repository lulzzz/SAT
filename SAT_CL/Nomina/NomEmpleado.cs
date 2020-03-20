using Microsoft.Reporting.WebForms;
using SAT_CL.Bancos;
using FEv32 = SAT_CL.FacturacionElectronica;
using FEv33 = SAT_CL.FacturacionElectronica33;
using SAT_CL.Global;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
//Espacios de nombres para uso de archivos de transformación XSL
namespace SAT_CL.Nomina {
	/// <summary>
	///  Implementa los método para la administración  de la  Nómina del Empleado
	/// </summary> 
	public class NomEmpleado : Disposable {
		#region Enumeraciones
		/// <summary>
		/// Enumeración que define a los Estatus de la Nomina del Empleado
		/// </summary>
		public enum Estatus {
			/// <summary>
			/// Estatus Registrado
			/// </summary>
			Registrado = 1,
			/// <summary>
			/// Estatus Timbrado
			/// </summary>
			Timbrado,
			/// <summary>
			/// Estatus Cancelado
			/// </summary>
			Cancelado
		}
		#endregion
		#region Atributos
		/// <summary>
		/// Atributo encargado de Almacenar el Nombre del SP
		/// </summary>
		private static string _nom_sp = "nomina.sp_nom_empleado_tne";
		private int _id_nomina_empleado;
		/// <summary>
		/// Atributo que almacena el Identificador de la Nomina del Empleado
		/// </summary>
		public int id_nomina_empleado { get { return this._id_nomina_empleado; } }
		private int _id_nomina;
		/// <summary>
		/// Atributo que almacena la Nomina
		/// </summary>
		public int id_nomina { get { return this._id_nomina; } }
		private int _id_empleado;
		/// <summary>
		/// Atributo que almacena el Empleado de la Nomina
		/// </summary>
		public int id_empleado { get { return this._id_empleado; } }
		private byte _id_estatus;
		/// <summary>
		/// Atributo que almacena el Estatus de la Nomina
		/// </summary>
		public byte id_estatus { get { return this._id_estatus; } }
		/// <summary>
		/// Atributo que almacena el Estatus de la Nomina (Enumeración)
		/// </summary>
		public Estatus estatus { get { return (Estatus)this._id_estatus; } }
		private int _id_comprobante;
		/// <summary>
		/// Atributo que almacena el Comprobante
		/// </summary>
		public int id_comprobante { get { return this._id_comprobante; } }
		private int _id_comprobante33;
		/// <summary>
		/// Numero de comprobante de la version 3.3
		/// </summary>
		public int id_comprobante33 { get { return _id_comprobante33; } }
		private bool _habilitar;
		/// <summary>
		/// Atributo que almacena el Estatus Habilitar
		/// </summary>
		public bool habilitar { get { return this._habilitar; } }
		#endregion
		#region Constructores
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos por Defectos
		/// </summary>
		public NomEmpleado()
		{
			//Asignando Atributos
			this._id_nomina_empleado = 0;
			this._id_nomina = 0;
			this._id_empleado = 0;
			this._id_estatus = 0;
			this._id_comprobante = 0;
			this._id_comprobante33 = 0;
			this._habilitar = false;
		}
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_nomina_empleado"></param>
		public NomEmpleado(int id_nomina_empleado)
		{
			//Invocando Método de Carga
			cargaAtributosInstancia(id_nomina_empleado);
		}
		#endregion
		#region Destructores
		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~NomEmpleado()
		{
			Dispose(false);
		}
		#endregion
		#region Métodos Privados
		/// <summary>
		/// Método encargado de Cargar los Atributos
		/// </summary>
		/// <param name="id_nomina"></param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			bool result = false;
			//Armando Arreglo de Parametros
			object[] param = { 3, id_nomina_empleado, 0, 0, 0, 0, 0, 0, false, "", "" };
			//Obteniendo resultado del SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan datos
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorriendo Cada Fila
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Asignando Atributos
						this._id_nomina_empleado = id_nomina_empleado;
						this._id_nomina = Convert.ToInt32(dr["IdNomina"]);
						this._id_empleado = Convert.ToInt32(dr["IdEmpleado"]);
						this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
						this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
						this._id_comprobante33 = Convert.ToInt32(dr["IdComprobante33"]);
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
		/// Método encargado de Actualizar los Atributos en BD
		/// </summary>
		/// <param name="id_nomina">Nomina</param>
		/// <param name="id_empleado">Empleado de la Nomina</param>
		/// <param name="id_estatus">Estatus de la Nomina del Empleado</param>
		/// <param name="id_comprobante">Comprobante (CFDI)</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <param name="habilitar">Estatus Habilitar del Registro</param>
		/// <returns></returns>
		private RetornoOperacion actualizaAtributosBD(int id_nomina, int id_empleado, byte id_estatus, int id_comprobante, int id_comprobante33, int id_usuario, bool habilitar)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Armando Arreglo de Parametros
			object[] param = { 2, this._id_nomina_empleado, id_nomina, id_empleado, id_estatus, id_comprobante, id_comprobante33, id_usuario, habilitar, "", "" };
			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
			//Devolviendo Resultado Obtenido
			return result;
		}
		#endregion
		#region Métodos Públicos
		/// <summary>
		/// Método encargado de Insertar la Nomina del Empleado añadiendo los esquema registros correspondientes a la nómina
		/// </summary>
		/// <param name="id_nomina">Nomina</param>
		/// <param name="id_empleado">Empleado de la Nomina</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaNominaEmpleado(int id_nomina, int id_empleado, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			int id_nomina_empleado = 0;
			//Creamos la transacción 
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Instanciando Nomina
				using (Nomina12 nomina = new Nomina12(id_nomina))
				{
					//Validando Nomina
					if (nomina.habilitar)
					{
						//Validando Nomina Timbrada
						result = nomina.ValidaNominaTimbrada();
						//Operación Exitosa?
						if (!result.OperacionExitosa)
						{
							//Armando Arreglo de Parametros
							object[] param = { 1, 0, id_nomina, id_empleado, Estatus.Registrado, 0, 0, id_usuario, true, "", "" };
							//Ejecutando SP
							result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
							//Asignamos Valor
							id_nomina_empleado = result.IdRegistro;
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Insertamos Encabezado de la Nómina//
								result = ActualizaEncabezadoNominaEsquema(id_nomina, id_nomina_empleado, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Insertamos Detalle de la Nómina
									result = ActualizaReceptorNominaEsquema(nomina, id_empleado, id_nomina_empleado, id_usuario);
								}
							}
						}
					}
				}
				//Validamos Resultado
				if (result.OperacionExitosa)
				{
					//Finalizamos Transacción
					scope.Complete();
				}
				//Devolcemos Nomina Empleado
				if (result.OperacionExitosa)
					//Instanciando Nomina Empleado
					result = new RetornoOperacion(id_nomina_empleado);
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Actualiza los Valores del Encabezado de la Nómina
		/// </summary>
		/// <param name="id_nomina">Nómina actual</param>
		/// <param name="id_nomina_empleado">Id Nómina de Empleado</param>
		/// <param name="id_usuario">Id usuario actualiza</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaEncabezadoNominaEsquema(int id_nomina, int id_nomina_empleado, int id_usuario)
		{
			//Declaramos objeto Resultado
			RetornoOperacion result = new RetornoOperacion();
			//Creamos la transacción 
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Instanciamos Nómina
				using (Nomina12 nomina = new Nomina12(id_nomina))
				{
					//Version
					result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Version", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Version", "Nomina"), id_nomina_empleado, nomina.version, 0, 0, 0, id_usuario);
					//Validamos Resultado
					if (result.OperacionExitosa)
					{
						//Tipo de Nómina
						result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "TipoNomina", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "TipoNomina", "Nomina"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(3185, nomina.id_tipo_nomina), 0, 3185, nomina.id_tipo_nomina, id_usuario);
						//Validamos Resultado
						if (result.OperacionExitosa)
						{
							//Fecha Pago
							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "FechaPago", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "FechaPago", "Nomina"), id_nomina_empleado, nomina.fecha_pago.ToString("yyyy-MM-dd"), 0, 0, 0, id_usuario);
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Fecha Inicial dePago
								result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "FechaInicialPago", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "FechaInicialPago", "Nomina"), id_nomina_empleado, nomina.fecha_inicial_pago.ToString("yyyy-MM-dd"), 0, 0, 0, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Fecha Feinal dePago
									result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "FechaFinalPago", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "FechaFinalPago", "Nomina"), id_nomina_empleado, nomina.fecha_final_pago.ToString("yyyy-MM-dd"), 0, 0, 0, id_usuario);
								}
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Numero de Dias de Pago
									result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "NumDiasPagados", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "NumDiasPagados", "Nomina"), id_nomina_empleado, nomina.dias_pago.ToString(), 0, 0, 0, id_usuario);
									//Validamos Resultado
									if (result.OperacionExitosa)
									{
										//Periodicidad de Pago
										result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "PeriodicidadPago", "Receptor", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "PeriodicidadPago", "Receptor", "Nomina"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(3186, nomina.id_periodicidad_pago), 0, 3186, nomina.id_periodicidad_pago, id_usuario);
										//Validamos Resultado
										if (result.OperacionExitosa)
										{
											//Registro Patronal
											result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "RegistroPatronal", "Emisor", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "RegistroPatronal", "Emisor", "Nomina"), id_nomina_empleado, nomina.registro_patronal, 0, 0, 0, id_usuario);
											//Validamos Resultado
											if (result.OperacionExitosa)
											{
												//Instanciamos Nómina Emeplado
												using (NomEmpleado objNomEmpleado = new NomEmpleado(id_nomina_empleado))
												{
													//Instanciamos Operador
													using (Operador objOperador = new Operador(objNomEmpleado.id_empleado))
													{
														//Declaramos Variable para Antiguedad
														string mensaje = "";
														//Declaramos Varible para Antuguedad
														if (Referencia.CargaReferencia("0", 76, objNomEmpleado.id_empleado, "Recibo Nómina", "ISSSTE") != "")
														{
															//En Caso de ser ISSTE
															mensaje = "P" + Cadena.ObtieneFormatoFecha(objOperador.fecha_ingreso, nomina.fecha_final_pago).Replace(" meses", "M").Replace(" días", "D").Replace(" ", "").Replace(" año(s)", "Y").Replace(" mes", "M").Replace(" día", "D");
														}
														else
														{
															//Obtenemos en Semanas
															TimeSpan diferencia = (nomina.fecha_final_pago - objOperador.fecha_ingreso);
															int semanas = (diferencia.Days + 1) / 7;
															mensaje = "P" + semanas.ToString() + "W";
														}
														// Antiguedad
														result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Antigüedad", "Receptor", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Antigüedad", "Receptor", "Nomina"), id_nomina_empleado, mensaje.Replace("-", ""), 0, 0, 0, id_usuario);
													}
												}
											}
										}
									}
								}
							}
						}
					}
					//Validamos Resultado
					if (result.OperacionExitosa)
					{
						scope.Complete();
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Actualiza los Valores del  nodo Recptor de la Nómina
		/// </summary>
		/// <param name="nomina">Nómina actual</param>
		/// <param name="id_empleado">Id de Empleado de la Nómina</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_usuario">Id usuario actualiza</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaReceptorNominaEsquema(Nomina12 nomina, int id_empleado, int id_nomina_empleado, int id_usuario)
		{
			//Declaramos objeto Resultado
			RetornoOperacion result = new RetornoOperacion();
			//Instanciamos Empleado
			using (Operador objOperador = new Operador(id_empleado))
			{
				//Creamos la transacción 
				using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					//Validamos Exista el Empleado
					if (objOperador.id_operador > 0)
					{
						//Curp
						result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "Curp", "Receptor", "Nomina"), id_nomina_empleado, objOperador.curp, 0, 0, 0, id_usuario);
						//Validamos Resultado
						if (result.OperacionExitosa)
						{
							//Número de Seguridad Social
							result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "NumSeguridadSocial", "Receptor", "Nomina"), id_nomina_empleado, objOperador.nss, 0, 0, 0, id_usuario);
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Fecha de Ingreso
								result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "FechaInicioRelLaboral", "Receptor", "Nomina"), id_nomina_empleado, objOperador.fecha_ingreso.ToString("yyyy-MM-dd"), 0, 0, 0, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Tipo Contrato
									result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "TipoContrato", "Receptor", "Nomina"), id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Tipo Contrato"), 0, 0, 0, id_usuario);
								}
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Sindicalizado
									result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "Sindicalizado", "Receptor", "Nomina"), id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Sindicalizado"), 0, 0, 0, id_usuario);
									//Validamos Resultado
									if (result.OperacionExitosa)
									{
										//Tipo Jornada
										result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "TipoJornada", "Receptor", "Nomina"), id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Tipo Jornada"), 0, 0, 0, id_usuario);
										//Validamos Resultado
										if (result.OperacionExitosa)
										{
											//Tipo Regimen
											result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "TipoRegimen", "Receptor", "Nomina"), id_nomina_empleado, Referencia.CargaReferencia("0", 25, nomina.id_compania_emisor, "Recibo Nómina", "Tipo Regimen"), 0, 0, 0, id_usuario);
											//Validamos Resultado
											if (result.OperacionExitosa)
											{
												//Num Empleado
												result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "NumEmpleado", "Receptor", "Nomina"), id_nomina_empleado, objOperador.id_operador.ToString(), 0, 0, 0, id_usuario);
												//Validamos Resultado
												if (result.OperacionExitosa)
												{
													//Departamento
													result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "Departamento", "Receptor", "Nomina"), id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Departamento"), 0, 0, 0, id_usuario);
													//Validamos Resultado
													if (result.OperacionExitosa)
													{
														//Puesto
														result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "Puesto", "Receptor", "Nomina"), id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Contratos", "Puesto"), 0, 0, 0, id_usuario);
														//Validamos Resultado
														if (result.OperacionExitosa)
														{
															//Riesgo Puesto
															result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "RiesgoPuesto", "Receptor", "Nomina"), id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Riesgo Puesto"), 0, 0, 0, id_usuario);
															//Validamos Resultado
															if (result.OperacionExitosa)
															{
																//Banco
																//Obtenemos Cuenta
																CuentaBancos objCuentaBancos = CuentaBancos.ObtieneCuentaBanco(76, objOperador.id_operador, CuentaBancos.TipoCuenta.Default);
																//Validamos Exista Cuenta
																if (objCuentaBancos != null)
																{
																	//Si Existe la Cuenta
																	if (objCuentaBancos.id_cuenta_bancos > 0)
																	{
																		//Instanciamos Banco
																		using (Banco objBanco = new Banco(objCuentaBancos.id_banco))
																		{
																			result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "Banco", "Receptor", "Nomina"), id_nomina_empleado, objBanco.clave, 98, 0, objBanco.id_banco, id_usuario);
																		}
																		//Validamos Resultado
																		if (result.OperacionExitosa)
																		{
																			//Instanciamos Banco
																			using (Banco objBanco = new Banco(objCuentaBancos.id_banco))
																			{
																				result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "CuentaBancaria", "Receptor", "Nomina"), id_nomina_empleado, objCuentaBancos.num_cuenta, 0, 0, 0, id_usuario);
																			}
																		}
																	}
																}
																//Validamos Resultado
																if (result.OperacionExitosa)
																{
																	//Asignamos Variable
																	decimal SBC = 0.00M;
																	if (Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Base Cotizacion Apor") != "")
																	{
																		//Asignamos Valor
																		SBC = Convert.ToDecimal(Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Base Cotizacion Apor"));
																	}
																	//Salario Base CotApor
																	result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "SalarioBaseCotApor", "Receptor", "Nomina"), id_nomina_empleado, SBC.ToString(), 0, 0, 0, id_usuario);
																	//Validamos Resultado
																	if (result.OperacionExitosa)
																	{
																		//Asignamos Variable
																		decimal SDI = 0.00M;
																		if (Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Diario Integrado") != "")
																		{
																			//Asignamos Valor
																			SDI = Convert.ToDecimal(Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Diario Integrado"));
																		}
																		//Salario Diario Integrado
																		result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "SalarioDiarioIntegrado", "Receptor", "Nomina"), id_nomina_empleado, SDI.ToString(), 0, 0, 0, id_usuario);
																		//Validamos Resultado
																		if (result.OperacionExitosa)
																		{
																			//Intsnaciamos Compañia
																			using (Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(nomina.id_compania_emisor))
																			{
																				//Instanciamos Direccion
																				using (Direccion objDireccion = new Direccion(objCompania.id_direccion))
																				{
																					//Clave Entidad Federativa
																					result = EsquemaRegistro.ActualizaEsquemaRegistro(0, 0, Esquema.ObtieneIdEsquema(nomina.version, "ClaveEntFed", "Receptor", "Nomina"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(16, objDireccion.id_estado), 0, 16, objDireccion.id_estado, id_usuario);
																				}
																			}
																		}
																	}
																}
															}
														}
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
						//Mostramos Error
						result = new RetornoOperacion("No se encontró datos complementario del Empleado.");
					//Validamos Resultado
					if (result.OperacionExitosa)
					{
						scope.Complete();
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Actualiza los Atributos principales del Operador
		/// </summary>
		/// <param name="nomina">Nomina</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion RecargaReceptorNominaEsquema(int id_usuario)
		{
			//Declaramos objeto Resultado
			RetornoOperacion result = new RetornoOperacion();
			//Creamos la transacción 
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Instanciamos Nómina
				using (Nomina12 nomina = new Nomina12(this._id_nomina))
				{
					//Instanciamos Operador
					using (Operador objOperador = new Operador(this._id_empleado))
					{
						//Validamos Exista el Empleado
						if (objOperador.id_operador > 0)
						{
							//Curp
							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Curp", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Curp", "Receptor", "Nomina"), this._id_nomina_empleado, objOperador.curp, 0, 0, 0, id_usuario);
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Número de Seguridad Social
								result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "NumSeguridadSocial", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "NumSeguridadSocial", "Receptor", "Nomina"), this._id_nomina_empleado, objOperador.nss, 0, 0, 0, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Fecha de Ingreso
									result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "FechaInicioRelLaboral", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "FechaInicioRelLaboral", "Receptor", "Nomina"), this._id_nomina_empleado, objOperador.fecha_ingreso.ToString("yyyy-MM-dd"), 0, 0, 0, id_usuario);
									//Validamos Resultado
									if (result.OperacionExitosa)
									{
										//Tipo Contrato
										result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "TipoContrato", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "TipoContrato", "Receptor", "Nomina"), this._id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Tipo Contrato"), 0, 0, 0, id_usuario);
									}
									//Validamos Resultado
									if (result.OperacionExitosa)
									{
										//Sindicalizado
										result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Sindicalizado", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Sindicalizado", "Receptor", "Nomina"), this._id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Sindicalizado"), 0, 0, 0, id_usuario);
										//Validamos Resultado
										if (result.OperacionExitosa)
										{
											//Tipo Jornada
											result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "TipoJornada", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "TipoJornada", "Receptor", "Nomina"), this._id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Tipo Jornada"), 0, 0, 0, id_usuario);
											//Validamos Resultado
											if (result.OperacionExitosa)
											{
												//Tipo Regimen
												result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "TipoRegimen", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "TipoRegimen", "Receptor", "Nomina"), this._id_nomina_empleado, Referencia.CargaReferencia("0", 25, nomina.id_compania_emisor, "Recibo Nómina", "Tipo Regimen"), 0, 0, 0, id_usuario);
												//Validamos Resultado
												if (result.OperacionExitosa)
												{
													//Num Empleado
													result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "NumEmpleado", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "NumEmpleado", "Receptor", "Nomina"), this._id_nomina_empleado, objOperador.id_operador.ToString(), 0, 0, 0, id_usuario);
													//Validamos Resultado
													if (result.OperacionExitosa)
													{
														//Departamento
														result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Departamento", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Departamento", "Receptor", "Nomina"), this._id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Departamento"), 0, 0, 0, id_usuario);
														//Validamos Resultado
														if (result.OperacionExitosa)
														{
															//Puesto
															result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Puesto", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Puesto", "Receptor", "Nomina"), this._id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Contratos", "Puesto"), 0, 0, 0, id_usuario);
															//Validamos Resultado
															if (result.OperacionExitosa)
															{
																//Riesgo Puesto
																result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "RiesgoPuesto", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "RiesgoPuesto", "Receptor", "Nomina"), this._id_nomina_empleado, Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Riesgo Puesto"), 0, 0, 0, id_usuario);
																//Validamos Resultado
																if (result.OperacionExitosa)
																{
																	//Banco
																	//Obtenemos Cuenta
																	CuentaBancos objCuentaBancos = CuentaBancos.ObtieneCuentaBanco(76, objOperador.id_operador, CuentaBancos.TipoCuenta.Default);
																	//Validamos Exista Cuenta
																	if (objCuentaBancos != null)
																	{
																		//Si Existe la Cuenta
																		if (objCuentaBancos.id_cuenta_bancos > 0)
																		{
																			//Instanciamos Banco
																			using (Banco objBanco = new Banco(objCuentaBancos.id_banco))
																			{
																				result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Banco", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Banco", "Receptor", "Nomina"), this._id_nomina_empleado, objBanco.clave, 98, 0, objBanco.id_banco, id_usuario);
																			}
																			//Validamos Resultado
																			if (result.OperacionExitosa)
																			{
																				//Instanciamos Banco
																				using (Banco objBanco = new Banco(objCuentaBancos.id_banco))
																				{
																					result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "CuentaBancaria", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "CuentaBancaria", "Receptor", "Nomina"), this._id_nomina_empleado, objCuentaBancos.num_cuenta.Length < 11 ? "" : objCuentaBancos.num_cuenta, 0, 0, 0, id_usuario);
																				}
																			}
																		}
																	}
																	else
																	{
																		//Insertamos Banco
																		result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Banco", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Banco", "Receptor", "Nomina"), this._id_nomina_empleado, "", 0, 0, 0, id_usuario);
																		//Validamos Resulatdo
																		if (result.OperacionExitosa)
																		{
																			//
																			result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "CuentaBancaria", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "CuentaBancaria", "Receptor", "Nomina"), this._id_nomina_empleado, "", 0, 0, 0, id_usuario);
																		}
																	}
																	//Validamos Resultado
																	if (result.OperacionExitosa)
																	{
																		//Asignamos Variable
																		decimal SBC = 0.00M;
																		if (Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Base Cotizacion Apor") != "")
																		{
																			//Asignamos Valor
																			SBC = Convert.ToDecimal(Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Base Cotizacion Apor"));
																		}
																		//Salario Base CotApor
																		result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "SalarioBaseCotApor", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "SalarioBaseCotApor", "Receptor", "Nomina"), this._id_nomina_empleado, SBC.ToString(), 0, 0, 0, id_usuario);
																		//Validamos Resultado
																		if (result.OperacionExitosa)
																		{
																			//Asignamos Variable
																			decimal SDI = 0.00M;
																			if (Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Diario Integrado") != "")
																			{
																				//Asignamos Valor
																				SDI = Convert.ToDecimal(Referencia.CargaReferencia("0", 76, objOperador.id_operador, "Recibo Nómina", "Salario Diario Integrado"));
																			}
																			//Salario Diario Integrado
																			result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "SalarioDiarioIntegrado", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "SalarioDiarioIntegrado", "Receptor", "Nomina"), this._id_nomina_empleado, SDI.ToString(), 0, 0, 0, id_usuario);
																			//Validamos Resultado
																			if (result.OperacionExitosa)
																			{
																				//Intsnaciamos Compañia
																				using (Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(nomina.id_compania_emisor))
																				{
																					//Instanciamos Direccion
																					using (Direccion objDireccion = new Direccion(objCompania.id_direccion))
																					{
																						//Clave Entidad Federativa
																						result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "ClaveEntFed", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "ClaveEntFed", "Receptor", "Nomina"), this._id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(16, objDireccion.id_estado), 0, 16, objDireccion.id_estado, id_usuario);
																						//Validamos Resultado
																						if (result.OperacionExitosa)
																						{
																							//Declaramos Variable para Antiguedad
																							string mensaje = "";
																							//Declaramos Varible para Antuguedad
																							if (Referencia.CargaReferencia("0", 76, this._id_empleado, "Recibo Nómina", "ISSSTE") != "")
																							{
																								//En Caso de ser ISSTE
																								mensaje = "P" + Cadena.ObtieneFormatoFecha(objOperador.fecha_ingreso, nomina.fecha_final_pago).Replace(" meses", "M").Replace(" días", "D").Replace(" ", "").Replace(" año(s)", "Y").Replace(" mes", "M").Replace(" día", "D");
																							}
																							else
																							{
																								//Obtenemos en Semanas
																								TimeSpan diferencia = (nomina.fecha_final_pago - objOperador.fecha_ingreso);
																								int semanas = (diferencia.Days + 1) / 7;
																								mensaje = "P" + semanas.ToString() + "W";
																							}
																							// Antiguedad
																							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(nomina.version, "Antigüedad", "Receptor", "Nomina"), this._id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(nomina.version, "Antigüedad", "Receptor", "Nomina"), this._id_nomina_empleado, mensaje.Replace("-", ""), 0, 0, 0, id_usuario);
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
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
							//Mostramos Error
							result = new RetornoOperacion("No se encontró datos complementario del Empleado.");
					}
					//Validamos Resultado
					if (result.OperacionExitosa)
					{
						scope.Complete();
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Método encargado de Actualizar los Valores
		/// </summary>
		/// <returns></returns>
		public bool ActualizaNomEmpleado()
		{
			//Invocando Método de Carga
			return this.cargaAtributosInstancia(this._id_nomina_empleado);
		}
		/// <summary>
		/// Método encargado de Obtener los Totales del Empleado
		/// </summary>
		/// <param name="id_nomina_empleado">Nomina del Empleado</param>
		/// <returns></returns>
		public static DataTable ObtieneTotalesEmpleado(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			DataTable dtNominasEmpleados = null;
			//Armando Arreglo de Parametros
			object[] param = { 6, id_nomina_empleado, 0, 0, 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dtNominasEmpleados = ds.Tables["Table"];
			}
			//Devolviendo Resultado
			return dtNominasEmpleados;
		}
		/// <summary>
		/// Método encargado de Obtener las Nominas de los Empleados en estatus Registrado para su Timbrado
		/// </summary>
		/// <param name="id_nomina">Nomina</param>
		/// <returns></returns>
		public static DataTable ObtieneNominasEmpleadoRegistrados(int id_nomina)
		{
			//Declarando Objeto de Retorno
			DataTable dtNominasEmpleados = null;
			//Armando Arreglo de Parametros
			object[] param = { 8, 0, id_nomina, 0, 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dtNominasEmpleados = ds.Tables["Table"];
			}
			//Devolviendo Resultado
			return dtNominasEmpleados;
		}
		/// <summary>
		/// Método encargado de Obtener las Nominas de los Empleados 
		/// </summary>
		/// <param name="id_nomina">Nomina</param>
		/// <returns></returns>
		public static DataTable ObtieneIdsNominasEmpleado(int id_nomina)
		{
			//Declarando Objeto de Retorno
			DataTable dtNominasEmpleados = null;
			//Armando Arreglo de Parametros
			object[] param = { 9, 0, id_nomina, 0, 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dtNominasEmpleados = ds.Tables["Table"];
			}
			//Devolviendo Resultado
			return dtNominasEmpleados;
		}
		#region CFDI3.2
		/// <summary>
		/// Método encargado de Obtener el Id de Nomina Empleado
		/// </summary>
		/// <param name="idComprobante">Id Comprobante</param>
		/// <returns></returns>
		public static int ObtieneIdNomEmpleado(int idComprobante)
		{
			//Declarando Objeto de Retorno
			int id_nom_empleado = 0;
			//Armando Arreglo de Parametros
			object[] param = { 10, 0, 0, 0, 0, idComprobante, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						id_nom_empleado = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			//Devolviendo Resultado Obtenido
			return id_nom_empleado;
		}
		/// <summary>
		/// Método encargado de Editar la Nomina del Empleado
		/// </summary>
		/// <param name="id_nomina">Nomina</param>
		/// <param name="id_empleado">Empleado de la Nomina</param>
		/// <param name="estatus">Estatus de la Nomina del Empleado</param>
		/// <param name="id_comprobante">Comprobante (CFDI)</param>
		/// <param name="id_comprobante33">Comprobante (CFDI3.3)</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion EditaNominaEmpleado(int id_nomina, int id_empleado, Estatus estatus, int id_comprobante, int id_comprobante33, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Validando que exista un Comprobante
			if (!(this._id_comprobante != 0 && this._id_comprobante33 != 0 && this.estatus == Estatus.Timbrado))
				//Devolviendo Resultado Obtenido
				result = this.actualizaAtributosBD(id_nomina, id_empleado, (byte)estatus, id_comprobante, id_comprobante33, id_usuario, this._habilitar);
			else
				//Instanciando Excepción
				result = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		// Método encargado de Actualizar el Estatus de la Nómina de Empleado
		/// </summary>
		/// <param name="estatus">Estatus de la Nómina Empleado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaEstatus(Estatus estatus, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Declarando Bloque Transaccional
			using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Devolviendo Resultado Obtenido
				result = this.actualizaAtributosBD(this._id_nomina, this._id_empleado, (byte)estatus, this._id_comprobante, this._id_comprobante33,id_usuario, this._habilitar);
				//Operación Exitosa?
				if (result.OperacionExitosa)
				{
					//Validando estatus Ingresado
					if (estatus == Estatus.Cancelado)
					{
						//Validando Comprobante
						if (this._id_comprobante > 0)
						{
							//Instanciando Compania
							using (FEv32.Comprobante cmp = new FEv32.Comprobante(this._id_comprobante))
							{
								//Validando comprobante
								if (cmp.habilitar)
									//Aztualizamos el Estatus del Comprobante a Cancelado
									result = cmp.CancelaComprobante(id_usuario);
								else
									//Instanciando Excepción
									result = new RetornoOperacion("No se puede recuperar el Comprobante");
							}
						}
						//Validando Comprobante 3.3
						if (this._id_comprobante33 > 0)
						{
							//Instanciando Compania
							using (FEv33.Comprobante cmp = new FEv33.Comprobante(this._id_comprobante33))
							{
								//Validando comprobante
								if (cmp.habilitar)
								{
                                    //Aztualizamos el Estatus del Comprobante a Cancelado
                                    result = cmp.CancelacionPendiente(id_usuario);
									//result = cmp.CancelaComprobante(id_usuario);
								}
								else
									//Instanciando Excepción
									result = new RetornoOperacion("No se puede recuperar el Comprobante (v3.3)");
							}
						}
					}
					//Operación Exitosa?
					if (result.OperacionExitosa)
					{
						//Inicializando Nomina de Empleado
						result = new RetornoOperacion(this._id_nomina_empleado);
						//Completando Transacción
						trans.Complete();
					}
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Deshabilitar la Nomina de Empleado
		/// </summary>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaNomEmpleado(int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Declarando Ambiente Transaccional
			using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Validando que exista un Comprobante
				if (!(this._id_comprobante != 0 && this._id_comprobante33 != 0 && this.estatus == Estatus.Timbrado))
				{
					//Obteniendo Detalles
					using (DataTable dtDetallesEmp = EsquemaRegistro.ObtieneDetalles(this._id_nomina_empleado))
					{
						//Validando que Existen Nominas
						if (Validacion.ValidaOrigenDatos(dtDetallesEmp))
						{
							//Iniciando Ciclo de Detalle
							foreach (DataRow drDE in dtDetallesEmp.Rows)
							{
								//Instanciando Detalle
								using (EsquemaRegistro dne = new EsquemaRegistro(Convert.ToInt32(drDE["Id"])))
								{
									//Validando Registro
									if (dne.habilitar)
									{
										//Insertando Detalle
										result = dne.DeshabilitaEsquemaRegistro(id_usuario);
										//Si la Operación no fue Exitosa
										if (!result.OperacionExitosa)
											//Terminando Ciclo
											break;
									}
									else
									{
										//Instanciando Nomina
										result = new RetornoOperacion("No Existe el Detalle de la Nomina");
										//Terminando Ciclo
										break;
									}
								}
							}
						}
						else
							//Instanciando Nomina
							result = new RetornoOperacion(this._id_nomina_empleado);
						//Validando Operaciones
						if (result.OperacionExitosa)
						{
							//Devolviendo Resultado Obtenido
							result = this.actualizaAtributosBD(this._id_nomina, this._id_empleado, this._id_estatus, this._id_comprobante, this._id_comprobante33, id_usuario, false);
							//Validando Operaciones
							if (result.OperacionExitosa)
								//Completando Transacción
								trans.Complete();
						}
					}
				}
				else
					//Instanciando Excepción
					result = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Obtener las Nominas de los Empleados dada un Nomina
		/// </summary>
		/// <param name="id_nomina">Nomina</param>
		/// <returns></returns>
		public static DataTable ObtieneNominasEmpleado(int id_nomina)
		{
			//Declarando Objeto de Retorno
			DataTable dtNominasEmpleados = null;
			//Armando Arreglo de Parametros
			object[] param = { 5, 0, id_nomina, 0, 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dtNominasEmpleados = ds.Tables["Table"];
			}
			//Devolviendo Resultado
			return dtNominasEmpleados;
		}
		/// <summary>
		/// Método que devuelve el ultimo sueldo pagado a un determinado empleado
		/// </summary>
		/// <param name="idEmpleado"></param>
		/// <returns></returns>
		public static decimal ObtieneSueldoNominaAnterior(int idNominaEmpleado, int idEmpleado)
		{
			//Variable retorno
			decimal ultimoSueldo = 0;
			//Armar arreglo de parámetros
			object[] param = { 13, idNominaEmpleado, 0, idEmpleado, 0, 0, 0, 0, false, "", "" };
			//Ejecutar consulta
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validar resultado de consulta
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorriendo Cada Fila
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Asignando Atributos
						ultimoSueldo = Convert.ToDecimal(dr["UltimoSueldo"]);
					}
				}
			}
			return ultimoSueldo;
		}
		/// <summary>
		/// Método encargado de Obtener los Datos de la Nomina de Empleado
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		private static DataSet obtienesDatosFacturaElectronica(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			DataSet dsNomina = null;
			//Armando Arreglo de Parametros
			object[] param = { 4, id_nomina_empleado, 0, 0, 0, 0, 0, 0, false, "", "" };
			//Obteniendo Liquidaciones
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds))
					//Asignando Valor Obtenido
					dsNomina = ds;
			}
			//Devolviendo Resultado Obtenido
			return dsNomina;
		}
		/// <summary>
		/// Carga Armado XML de Recibo de Nómina
		/// </summary>
		/// <param name="id_nomina_empleado"> Id Nómina del Empleado</param>
		/// <returns></returns>
		private static DataSet cargaArmadoXMLReciboNomina(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			DataSet dsNomina = null;
			//Armando Arreglo de Parametros
			object[] param = { 7, id_nomina_empleado, 0, 0, 0, 0, 0, 0, false, "", "" };
			//Obteniendo Liquidaciones
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds))
					//Asignando Valor Obtenido
					dsNomina = ds;
			}
			//Devolviendo Resultado Obtenido
			return dsNomina;
		}
		/// <summary>
		/// Genera el archivo .pdf del CFDI de Nómina
		/// </summary>
		/// <returns></returns>
		public byte[] GeneraPDFComprobanteNomina()
		{
			//Creando nuevo visor de reporte
			ReportViewer rvReporte = new Microsoft.Reporting.WebForms.ReportViewer();
			//Habilita las imagenes externas en reporte
			rvReporte.LocalReport.EnableExternalImages = true;
			//Asignación de la ubicación del reporte local
			rvReporte.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/RDLC/ComprobanteNomina12.rdlc");
			//Creacion del la variable tipo tabla dtLogo.
			DataTable dtLogo = new DataTable();
			//Creación de la tabla para cargar el QR
			DataTable dtCodigo = new DataTable();
			//Agrega una columna a la table donde almacenara el parametro logotipo
			dtLogo.Columns.Add("Logotipo", typeof(byte[]));
			//Agrega una columna a la table donde almacenara el parametro Imagen
			dtCodigo.Columns.Add("Imagen", typeof(byte[]));
			//Habilita la consulta de imagenes externas
			rvReporte.LocalReport.EnableExternalImages = true;
			//Limpia el reporte
			rvReporte.LocalReport.DataSources.Clear();
			//Invoca a la casle Nomina empleado para obtener los datos de nomina empleado
			using (SAT_CL.Nomina.NomEmpleado nomOperador = new SAT_CL.Nomina.NomEmpleado(this._id_nomina_empleado))
			{
				//Invoca a la clase operador y obtiene los datos del empleado
				using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(nomOperador.id_empleado))
				{
					ReportParameter nombreEmpleado = new ReportParameter("NombreEmpleado", op.nombre);
					ReportParameter rfcEmpleado = new ReportParameter("RFCEmpleado", op.rfc);
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreEmpleado, rfcEmpleado });
					//Invoca a la clase Direccion apra obtener la direccion del empleado
					using (SAT_CL.Global.Direccion dirEmpleado = new SAT_CL.Global.Direccion(op.id_direccion))
					{
						ReportParameter direccionEmpleado = new ReportParameter("DireccionEmpleado", dirEmpleado.ObtieneDireccionCompleta());
						rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmpleado });
					}
				}
				//Invoca a la clase comprobante y obtiene los datos referentes a la facturacion electronica del comprobante de nomina
				using (SAT_CL.FacturacionElectronica.Comprobante comprobante = new SAT_CL.FacturacionElectronica.Comprobante(nomOperador.id_comprobante))
				{
					//Creació del arreglo necesario para la carga de la ruta del logotipo y del 
					byte[] imagen = null;
					//Permite capturar errores en caso de que no exista una ruta para el archivo
					try
					{
						//Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
						imagen = System.IO.File.ReadAllBytes(comprobante.ruta_codigo_bidimensional);
					}
					//En caso de que no exista una imagen, se devolvera un valor nulo.
					catch { imagen = null; }
					//Agrega a la tabla un registro con valor a la ruta de la imagen.
					dtCodigo.Rows.Add(imagen);
					ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
					rvReporte.LocalReport.DataSources.Add(rvscod);
					//Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
					SAT_CL.FacturacionElectronica.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(comprobante.id_comprobante);
					//Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
					ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
					ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);
					string cadenaOriginal = "";
					RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(comprobante.ruta_xml, System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"), System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"), out cadenaOriginal);
					ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
					ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
					ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
					ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
					//Asigna valores a los parametros del reporteComprobante
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });
					//Instanciamos a la clase Certificado
					using (CertificadoDigital certificado = new CertificadoDigital(comprobante.id_certificado))
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
					ReportParameter banco = new ReportParameter("Banco", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "Banco", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { banco });
					//Asigna los valores de la clase cuentaBancos a los parametros
					ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "CuentaBancaria", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
					//Asigna valores de los parametros del reporteComprobante
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
					//Asigna los valores de la clase comprobante a los parametros 
					ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", comprobante.fecha_expedicion.ToString());
					ReportParameter serieCFDI = new ReportParameter("SerieCFDI", comprobante.serie);
					ReportParameter folio = new ReportParameter("Folio", comprobante.folio.ToString());
					ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, comprobante.id_metodo_pago) + "  -  " + SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(80, comprobante.id_metodo_pago));
					ReportParameter formaPagoCFDI = new ReportParameter("FormaPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1099, comprobante.id_forma_pago));
					ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(comprobante.total_moneda_nacional.ToString()));
					ReportParameter totalCFDI = new ReportParameter("TotalCFDI", comprobante.total_moneda_nacional.ToString());
					ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", comprobante.subtotal_moneda_nacional.ToString());
					ReportParameter importeDeduccion = new ReportParameter("ImporteDeduccion", comprobante.descuento_moneda_nacional.ToString());
					//Asigna valores a los parametros del reporteComprobante                      
					rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,metodoPagoCFDI,formaPagoCFDI,importeLetrasCFDI,totalCFDI, subtotalCFDI,importeDeduccion});
					//Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
					SAT_CL.FacturacionElectronica.Impuesto imp = (SAT_CL.FacturacionElectronica.Impuesto.RecuperaImpuestoComprobante(comprobante.id_comprobante));
					//Instancia la clase DetalleImpuesto para obtener el desglose de los impuestos dado un id_impuesto
					using (DataTable detalleImp = SAT_CL.FacturacionElectronica.DetalleImpuesto.CargaDetallesImpuesto(imp.id_impuesto))
					{
						//Declarando variables auxiliares para recuperar impuestos
						decimal totalIsr = 0;
						//Si hay impuestos agregados al comprobante
						if (imp.id_impuesto > 0)
						{
							totalIsr = (from DataRow r in detalleImp.Rows where r.Field<int>("IdImpuestoRetenido") == 1 select r.Field<decimal>("ImporteMonedaNacional")).FirstOrDefault();
						}
						ReportParameter ISR = new ReportParameter("ISR", totalIsr.ToString());
						rvReporte.LocalReport.SetParameters(new ReportParameter[] { ISR });
					}
				}
				//Invoca a la clase Nomina para obtener los datos necesarios para la nomina
				using (SAT_CL.Nomina.Nomina12 nomina = new SAT_CL.Nomina.Nomina12(nomOperador.id_nomina))
				{
					ReportParameter diasPagados = new ReportParameter("DiasPagados", nomina.dias_pago.ToString());
					ReportParameter fechaPago = new ReportParameter("FechaPago", nomina.fecha_pago.ToString());
					ReportParameter fechaInicioPeriodicidad = new ReportParameter("FechaInicioPeriodicidad", nomina.fecha_inicial_pago.ToString());
					ReportParameter fechaFinPeriodicidad = new ReportParameter("FechaFinPeriodicidad", nomina.fecha_final_pago.ToString());
					ReportParameter periodicidad = new ReportParameter("Periodicidad", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3186, nomina.id_periodicidad_pago).ToString().ToUpper());
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPagados, fechaPago, fechaInicioPeriodicidad, fechaFinPeriodicidad, periodicidad });
					//Invoca a la clase compania y obtiene los datos de la compania emisora del comprobante
					using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(nomina.id_compania_emisor))
					{
						ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", Referencia.CargaReferencia("0", 25, nomina.id_compania_emisor, "Facturacion Electronica", "Regimen Fiscal"));
						ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
						ReportParameter color = new ReportParameter("Color", Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
						ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
						rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, color, regimenFiscalCFDI });
						//Invoca a la clase direccion para obtener la direcciond e la compañia
						using (SAT_CL.Global.Direccion dirEmisor = new SAT_CL.Global.Direccion(emisor.id_direccion))
						{
							ReportParameter direccionEmisorMatriz = new ReportParameter("DireccionEmisorMatriz", dirEmisor.ObtieneDireccionCompleta());
							rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmisorMatriz });
						}
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
						//Asigna al reporte el datasource con los valores asignado al conjunto de datos.
						rvReporte.LocalReport.DataSources.Add(rvs);
					}
				}
				//Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de deducción de una nomina
				using (DataTable dtDetalleNominaDeduccion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaDeduccionNuevaV(this._id_nomina_empleado))
				{
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(dtDetalleNominaDeduccion))
					{
						ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", dtDetalleNominaDeduccion);
						rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
					}
					else
					{
						//Creamos Tabla
						using (DataTable mit = new DataTable())
						{
							//Añadimos columnas
							mit.Columns.Add("Clave", typeof(string));
							mit.Columns.Add("Concepto", typeof(string));
							mit.Columns.Add("Importe", typeof(decimal));
							//Añadimos registro
							DataRow row = mit.NewRow();
							row["Clave"] = "000";
							row["Concepto"] = " ";
							row["Importe"] = 0.00;
							mit.Rows.Add(row);
							ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", mit);
							rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
						}
					}
				}
				//Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de percepción de una nomina
				using (DataTable dtDetalleNominaPercepcion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaPercepcionNuevaV(this._id_nomina_empleado))
				{
					ReportDataSource rsComprobanteNominaPercepcion = new ReportDataSource("ComprobanteNominaPercepcion", dtDetalleNominaPercepcion);
					rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaPercepcion);
				}
			}
			//Devolviendo resultado
			return rvReporte.LocalReport.Render("PDF");
		}
		/// <summary>
		/// Importación y Timbrado del recibo de Nómina 1.2, Comprobante 3.2
		/// </summary>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="ruta_xslr_co"></param>
		/// <param name="ruta_xslr_co_local"></param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion ImportaTimbraNominaEmpleadoComprobante_V3_2(string version, string serie, string ruta_xslr_co, string ruta_xslr_co_local, int id_usuario)
		{
			//Declaramos Objeto Retorno
			RetornoOperacion resultado = new RetornoOperacion();
			//Validando Timbrado en Esquema nuevo
			if (this._id_comprobante33 == 0)
			{
				//Creamos la transacción 
				using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					//Validamos Estatus de la Nómina 
					if ((Estatus)this._id_estatus == Estatus.Registrado)
					{
						//Validamos Totales Horas Extras
						resultado = EsquemaRegistro.ValidaTotalesHorasExtras(this._id_nomina_empleado);
						//Validamos Totales de Horas Extras
						if (resultado.OperacionExitosa)
						{
							//Validamos Totales de Incapacidades
							resultado = EsquemaRegistro.ValidaTotalesIncapacidades(this._id_nomina_empleado);
							//Validamos Resultado
							if (resultado.OperacionExitosa)
							{
								//Quitamos Total de Impuestos Retenidos
								resultado = EsquemaRegistro.QuitaTotalImpuestoRetenido(version, this._id_nomina_empleado, id_usuario);
								//Validamos Resultado
								if (resultado.OperacionExitosa)
								{
									//Quitamos Total Deducciones
									resultado = EsquemaRegistro.QuitaTotalDeducciones(version, this._id_nomina_empleado, id_usuario);
									{
										//Validamos Incapacidades
										if (resultado.OperacionExitosa)
										{
											//Obtenemos los Datos de Nomina Empleado de acuerdo al esquema de Facturación Electrónica
											using (DataSet dsNomina = obtienesDatosFacturaElectronica(this._id_nomina_empleado))
											{
												//Validamos Registros
												if (Validacion.ValidaOrigenDatos(dsNomina))
												{
													//Intsanciamos Nómina
													using (Nomina12 objNomina = new Nomina12(this._id_nomina))
													{
														//Recargamos Atributos del Empleado
														resultado = RecargaReceptorNominaEsquema(id_usuario);

														//Validamos Resultado
														if (resultado.OperacionExitosa)
														{
															//Importamos Factura a Factura Electrónica
															resultado = FEv32.Comprobante.ImportaReciboNominaActualizacion1_V3_2(dsNomina.Tables["Table"], objNomina.id_compania_emisor, objNomina.id_sucursal, dsNomina.Tables["Table1"], null, dsNomina.Tables["Table3"], dsNomina.Tables["Table2"], dsNomina.Tables["Table5"], id_usuario);
														}
													}
													//Validamos Resultaod de Timbrado
													if (resultado.OperacionExitosa)
													{
														//Instaciamos Comprobante
														using (FEv32.Comprobante objComprobante = new FEv32.Comprobante(resultado.IdRegistro))
														{
															//Validamos Id Comprobante
															if (objComprobante.habilitar)
															{
																//Actualizmos Id de Comprobantes
																resultado = EditaNominaEmpleado(this._id_nomina, this._id_empleado, (Estatus)this._id_estatus, resultado.IdRegistro, this._id_comprobante33, id_usuario);
																//Validamos Resultado
																if (resultado.OperacionExitosa)
																{
																	//Timbramos
																	resultado = objComprobante.TimbraReciboNomina_V3_2(version, serie, id_usuario, ruta_xslr_co, ruta_xslr_co_local, this._id_nomina_empleado, false);
																	//Actualizamos Estatus de la Nómina de Empleado
																	if (resultado.OperacionExitosa)
																	{
																		//Recargamos Atributos
																		if (this.ActualizaNomEmpleado())
																		{
																			//Actualizamos Estatus
																			resultado = ActualizaEstatus(Estatus.Timbrado, id_usuario);
																			//Validamos Resultado
																			if (resultado.OperacionExitosa)
																			{
																				resultado = new RetornoOperacion(this._id_nomina_empleado, "La Nómina del Empleado se ha Timbrado " + objComprobante.serie + objComprobante.folio.ToString(), true);
																				//Finalizamos transacción
																				scope.Complete();
																			}
																		}
																		else
																			//Mostramos Mensaje Error
																			resultado = new RetornoOperacion("Error al refrescar Atributos.");
																	}
																}
															}
														}
													}
												}
												else
												{
													resultado = new RetornoOperacion("No se encontró Información para exportación de la FE.");
												}
											}
										}
									}
								}
							}
						}
					}
					else
						//Mostramos Mensaje Error
						resultado = new RetornoOperacion("El estatus del Nómina no permite su edición.");
				}
			}
			else
				//Mostramos Mensaje Error
				resultado = new RetornoOperacion("La Nómina del Empleado fue Timbrada en el esquema anterior (v3.3), debe cancelarla para poderla Timbrar en el Nuevo Esquema.");
			//Devolvemos Resultado
			return resultado;
		}
		#endregion
		#region CFDI3.3
		/// <summary>
		/// Método encargado de Obtener el Id de Nomina Empleado
		/// </summary>
		/// <param name="idComprobante33">Id Comprobante</param>
		/// <returns></returns>
		public static int ObtieneIdNomEmpleadoV3_3(int idComprobante33)
		{
			//Declarando Objeto de Retorno
			int id_nom_empleado = 0;
			//Armando Arreglo de Parametros
			object[] param = { 12, 0, 0, 0, 0, 0, idComprobante33, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
						//Obtenemos Validacion
						id_nom_empleado = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
			}
			//Devolviendo Resultado Obtenido
			return id_nom_empleado;
		}
		/// <summary>
		/// Método encargado de Obtener los Datos de la Nomina de Empleado
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static DataSet ObtienesDatosFacturaElectronicaV3_3(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			DataSet dsNomina = null;
			//Armando Arreglo de Parametros
			object[] param = { 11, id_nomina_empleado, 0, 0, 0, 0, 0, 0, true, "", "" };
			//Obteniendo Liquidaciones
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds))
					//Asignando Valor Obtenido
					dsNomina = ds;
			}
			//Devolviendo Resultado Obtenido
			return dsNomina;
		}
		/// <summary>
		/// Importación y Timbrado del recibo de Nómina 1.2, Comprobante 3.3
		/// </summary>
		/// <param name="serie">Serie de la Factura</param>
		/// <param name="ruta_xslr_co"></param>
		/// <param name="ruta_xslr_co_local"></param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion ImportaTimbraNominaEmpleadoComprobante_V3_3(string version, string serie, string ruta_xslr_co, string ruta_xslr_co_local, int id_usuario)
		{
			//Declaramos Objeto Retorno
			RetornoOperacion resultado = new RetornoOperacion();
			//Validando Registro en esquema Anterior
			if (this._id_comprobante == 0)
			{
				//Creamos la transacción 
				using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					//Validamos Estatus de la Nómina 
					if ((Estatus)this._id_estatus == Estatus.Registrado)
					{
						//Validamos Totales Horas Extras
						resultado = EsquemaRegistro.ValidaTotalesHorasExtras(this._id_nomina_empleado);
						//Validamos Totales de Horas Extras
						if (resultado.OperacionExitosa)
						{
							//Validamos Totales de Incapacidades
							resultado = EsquemaRegistro.ValidaTotalesIncapacidades(this._id_nomina_empleado);
							//Validamos Resultado
							if (resultado.OperacionExitosa)
							{
								//Quitamos Total de Impuestos Retenidos
								resultado = EsquemaRegistro.QuitaTotalImpuestoRetenido(version, this._id_nomina_empleado, id_usuario);
								//Validamos Resultado
								if (resultado.OperacionExitosa)
								{
									//Quitamos Total Deducciones
									resultado = EsquemaRegistro.QuitaTotalDeducciones(version, this._id_nomina_empleado, id_usuario);
									{
										//Validamos Incapacidades
										if (resultado.OperacionExitosa)
										{
											//Obtenemos los Datos de Nomina Empleado de acuerdo al esquema de Facturación Electrónica
											using (DataSet dsNomina = ObtienesDatosFacturaElectronicaV3_3(this._id_nomina_empleado))
											{
												//Validamos Registros
												if (Validacion.ValidaOrigenDatos(dsNomina))
												{
													//Intsanciamos Nómina
													using (Nomina12 objNomina = new Nomina12(this._id_nomina))
													{
														//Recargamos Atributos del Empleado
														resultado = RecargaReceptorNominaEsquema(id_usuario);
														//Validamos Resultado
														if (resultado.OperacionExitosa)
															//Importamos Factura a Factura Electrónica
															resultado = FEv33.Comprobante.ImportaReciboNominaActualizacion_V3_3(dsNomina.Tables["Table"], objNomina.id_compania_emisor, objNomina.id_sucursal, dsNomina.Tables["Table1"], null, dsNomina.Tables["Table2"], id_usuario);
													}
													//Validamos Resultaod de Timbrado
													if (resultado.OperacionExitosa)
													{
														//Instaciamos Comprobante
														using (FEv33.Comprobante objComprobante = new FEv33.Comprobante(resultado.IdRegistro))
														{
															//Validamos Id Comprobante
															if (objComprobante.id_comprobante33 > 0)
															{
																//Actualizmos Id de Comprobantes
																resultado = EditaNominaEmpleado(this._id_nomina, this._id_empleado, (Estatus)this._id_estatus, this._id_comprobante, resultado.IdRegistro, id_usuario);
																//Validamos Resultado
																if (resultado.OperacionExitosa)
																{
																	//Timbramos
																	resultado = objComprobante.TimbraReciboNomina_V3_3(version, serie, id_usuario, ruta_xslr_co, ruta_xslr_co_local, this._id_nomina_empleado, false);
																	//Actualizamos Estatus de la Nómina de Empleado
																	if (resultado.OperacionExitosa)
																	{
																		//Recargamos Atributos
																		if (this.ActualizaNomEmpleado())
																		{
																			//Actualizamos Estatus
																			resultado = ActualizaEstatus(Estatus.Timbrado, id_usuario);
																			//Validamos Resultado
																			if (resultado.OperacionExitosa)
																			{
																				resultado = new RetornoOperacion(this._id_nomina_empleado, "La Nómina del Empleado se ha Timbrado " + objComprobante.serie + objComprobante.folio, true);
																				//Finalizamos transacción
																				scope.Complete();
																			}
																		}
																		else
																			//Mostramos Mensaje Error
																			resultado = new RetornoOperacion("Error al refrescar Atributos.");
																	}
																}
															}
														}
													}
												}
												else
												{
													resultado = new RetornoOperacion("No se encontró Información para exportación de la FE.");
												}
											}
										}
									}
								}
							}
						}
					}
					else
						//Mostramos Mensaje Error
						resultado = new RetornoOperacion("El estatus del Nómina no permite su edición.");
				}
			}
			else
				//Mostramos Mensaje Error
				resultado = new RetornoOperacion("La Nómina del Empleado fue Timbrada en el esquema anterior (v3.2), debe cancelarla para poderla Timbrar en el Nuevo Esquema.");
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Genera el archivo .pdf del CFDI de Nómina
		/// </summary>
		/// <returns></returns>
		public byte[] GeneraPDFComprobanteNomina33()
		{
			//Creando nuevo visor de reporte
			ReportViewer rvReporte = new Microsoft.Reporting.WebForms.ReportViewer();
			//Habilita las imagenes externas en reporte
			rvReporte.LocalReport.EnableExternalImages = true;
			//Almacena el identificador de un comprobante de nomina
			int idEmpleadoNomina = this._id_nomina_empleado;
			//Declara la ubicación Local del RDLC de una Nomina
			rvReporte.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/RDLC/CFDINomina12_33.rdlc");
			//Creacion del la variable tipo tabla dtLogo.
			DataTable dtLogo = new DataTable();
			//Creación de la tabla para cargar el QR
			DataTable dtCodigo = new DataTable();
			//Agrega una columna a la table donde almacenara el parametro logotipo
			dtLogo.Columns.Add("Logotipo", typeof(byte[]));
			//Agrega una columna a la table donde almacenara el parametro Imagen
			dtCodigo.Columns.Add("Imagen", typeof(byte[]));
			//Habilita la consulta de imagenes externas
			rvReporte.LocalReport.EnableExternalImages = true;
			//Limpia el reporte
			rvReporte.LocalReport.DataSources.Clear();
			//Invoca a la casle Nomina empleado para obtener los datos de nomina empleado
			using (SAT_CL.Nomina.NomEmpleado nomOperador = new SAT_CL.Nomina.NomEmpleado(idEmpleadoNomina))
			{
				//Invoca a la clase operador y obtiene los datos del empleado
				using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(nomOperador.id_empleado))
				{
					ReportParameter nombreEmpleado = new ReportParameter("NombreEmpleado", op.nombre);
					ReportParameter rfcEmpleado = new ReportParameter("RFCEmpleado", op.rfc);
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreEmpleado, rfcEmpleado });
					//Invoca a la clase Direccion apra obtener la direccion del empleado
					using (SAT_CL.Global.Direccion dirEmpleado = new SAT_CL.Global.Direccion(op.id_direccion))
					{
						ReportParameter direccionEmpleado = new ReportParameter("DireccionEmpleado", dirEmpleado.ObtieneDireccionCompleta());
						rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmpleado });
					}
				}
				//Invoca a la clase comprobante y obtiene los datos referentes a la facturacion electronica del comprobante de nomina
				using (SAT_CL.FacturacionElectronica33.Comprobante comprobante = new SAT_CL.FacturacionElectronica33.Comprobante(nomOperador.id_comprobante33))
				{
					//Creació del arreglo necesario para la carga de la ruta del logotipo y del 
					byte[] imagen = null;
					//Permite capturar errores en caso de que no exista una ruta para el archivo
					try
					{
						//Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
						imagen = System.IO.File.ReadAllBytes(comprobante.ruta_codigo_bidimensional);
					}
					//En caso de que no exista una imagen, se devolvera un valor nulo.
					catch { imagen = null; }
					//Agrega a la tabla un registro con valor a la ruta de la imagen.
					dtCodigo.Rows.Add(imagen);
					ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
					rvReporte.LocalReport.DataSources.Add(rvscod);
					//Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
					SAT_CL.FacturacionElectronica33.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(comprobante.id_comprobante33);
					//Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
					ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
					ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);
					string cadenaOriginal = "";
					RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(comprobante.ruta_xml, System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"), System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), out cadenaOriginal);
					ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
					ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
					ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
					ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
					//Asigna valores a los parametros del reporteComprobante
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });
					//Instanciamos a la clase Certificado
					using (CertificadoDigital certificado = new CertificadoDigital(comprobante.id_certificado))
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
					ReportParameter banco = new ReportParameter("Banco", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "Banco", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { banco });
					//Asigna los valores de la clase cuentaBancos a los parametros
					ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "CuentaBancaria", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
					//Asigna valores de los parametros del reporteComprobante
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
					//Asigna los valores de la clase comprobante a los parametros 
					ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", comprobante.fecha_expedicion.ToString());
					ReportParameter serieCFDI = new ReportParameter("SerieCFDI", comprobante.serie);
					ReportParameter folio = new ReportParameter("Folio", comprobante.folio.ToString());
					string uso_cfdi = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3194, comprobante.id_uso_receptor);
					ReportParameter usoCFDI = new ReportParameter("UsoCFDI", uso_cfdi);
					ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, comprobante.id_metodo_pago) + "  -  " + SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3195, comprobante.id_metodo_pago));
					ReportParameter formaPagoCFDI;
					//Instanciando Forma de Pago.
					using (SAT_CL.FacturacionElectronica33.FormaPago fp = new SAT_CL.FacturacionElectronica33.FormaPago(comprobante.id_forma_pago))
						//Asignando Forma de Pago
						formaPagoCFDI = new ReportParameter("FormaPagoCFDI", fp.descripcion);
					//Asignando Valores
					ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(comprobante.total_nacional.ToString()));
					ReportParameter totalCFDI = new ReportParameter("TotalCFDI", comprobante.total_nacional.ToString());
					ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", comprobante.subtotal_nacional.ToString());
					ReportParameter importeDeduccion = new ReportParameter("ImporteDeduccion", comprobante.descuentos_nacional.ToString());
					//Asigna valores a los parametros del reporteComprobante                      
					rvReporte.LocalReport.SetParameters(new ReportParameter[] { fechaComprobanteCFDI, serieCFDI, folio, metodoPagoCFDI, formaPagoCFDI, importeLetrasCFDI, totalCFDI, subtotalCFDI, importeDeduccion });
					//Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
					SAT_CL.FacturacionElectronica33.Impuesto imp = (SAT_CL.FacturacionElectronica33.Impuesto.ObtieneImpuestoComprobante(comprobante.id_comprobante33));
					//Instancia la clase DetalleImpuesto para obtener el desglose de los impuestos dado un id_impuesto
					using (DataTable detalleImp = SAT_CL.FacturacionElectronica33.ImpuestoDetalle.ObtieneDetallesImpuesto(imp.id_impuesto))
					{
						//Declarando variables auxiliares para recuperar impuestos
						decimal totalIsr = 0;
						//Si hay impuestos agregados al comprobante
						if (imp.id_impuesto > 0)
						{
							totalIsr = (from DataRow r in detalleImp.Rows where r.Field<string>("Detalle").Equals("Retencion") select r.Field<decimal>("ImporteNacional")).Sum();
						}
						ReportParameter ISR = new ReportParameter("ISR", totalIsr.ToString());
						rvReporte.LocalReport.SetParameters(new ReportParameter[] { ISR });
					}

					//Invoca a la clase Nomina para obtener los datos necesarios para la nomina
					using (SAT_CL.Nomina.Nomina12 nomina = new SAT_CL.Nomina.Nomina12(nomOperador.id_nomina))
					{
						ReportParameter diasPagados = new ReportParameter("DiasPagados", nomina.dias_pago.ToString());
						ReportParameter fechaPago = new ReportParameter("FechaPago", nomina.fecha_pago.ToString());
						ReportParameter fechaInicioPeriodicidad = new ReportParameter("FechaInicioPeriodicidad", nomina.fecha_inicial_pago.ToString());
						ReportParameter fechaFinPeriodicidad = new ReportParameter("FechaFinPeriodicidad", nomina.fecha_final_pago.ToString());
						ReportParameter periodicidad = new ReportParameter("Periodicidad", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3186, nomina.id_periodicidad_pago).ToString().ToUpper());
						rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPagados, fechaPago, fechaInicioPeriodicidad, fechaFinPeriodicidad, periodicidad });
						//Invoca a la clase compania y obtiene los datos de la compania emisora del comprobante
						using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(nomina.id_compania_emisor))
						{
							//Asignando Parametros
							ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCadenaValor(3197, comprobante.regimen_fiscal));
							ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
							ReportParameter color = new ReportParameter("Color", Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
							ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
							rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, color, regimenFiscalCFDI });
							//Invoca a la clase direccion para obtener la direcciond e la compañia
							using (SAT_CL.Global.Direccion dirEmisor = new SAT_CL.Global.Direccion(emisor.id_direccion))
							{
								ReportParameter direccionEmisorMatriz = new ReportParameter("DireccionEmisorMatriz", dirEmisor.codigo_postal);
								rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmisorMatriz });
							}
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
							//Asigna al reporte el datasource con los valores asignado al conjunto de datos.
							rvReporte.LocalReport.DataSources.Add(rvs);
						}
					}
					//Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de deducción de una nomina
					using (DataTable dtDetalleNominaDeduccion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaDeduccionNuevaV(idEmpleadoNomina))
					{
						//Validamos Origen de Datos
						if (Validacion.ValidaOrigenDatos(dtDetalleNominaDeduccion))
						{
							ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", dtDetalleNominaDeduccion);
							rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
						}
						else
						{
							//Creamos Tabla
							using (DataTable mit = new DataTable())
							{
								//Añadimos columnas
								mit.Columns.Add("Clave", typeof(string));
								mit.Columns.Add("Concepto", typeof(string));
								mit.Columns.Add("Importe", typeof(decimal));
								//Añadimos registro
								DataRow row = mit.NewRow();
								row["Clave"] = "000";
								row["Concepto"] = " ";
								row["Importe"] = 0.00;
								mit.Rows.Add(row);
								ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", mit);
								rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
							}
						}
					}
					//Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de percepción de una nomina
					using (DataTable dtDetalleNominaPercepcion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaPercepcionNuevaV(idEmpleadoNomina))
					{
						//Declara Variable que obtendra las filas que contengan datos.                     
						DataRow[] percepciones = (from DataRow r in dtDetalleNominaPercepcion.Rows where r.Field<string>("CLAVE") != null select r).ToArray();
						ReportDataSource rsComprobanteNominaPercepcion = new ReportDataSource("ComprobanteNominaPercepcion", TSDK.Datos.OrigenDatos.ConvierteArregloDataRowADataTable(percepciones));
						rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaPercepcion);
					}
				}
			}
			//Devolviendo resultado
			return rvReporte.LocalReport.Render("PDF");
		}
		#endregion
		#endregion
	}
}