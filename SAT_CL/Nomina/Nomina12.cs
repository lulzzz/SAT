using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Nomina {
	/// <summary>
	///  Implementa los método para la administración  de la  Nómina
	/// </summary>
	public class Nomina12 : Disposable {
		#region Atributos

		/// <summary>
		/// Atributo encargado de Almacenar el Nombre del SP
		/// </summary>
		private static string _nom_sp = "nomina.sp_nomina12_tn";

		private int _id_nomina;
		/// <summary>
		/// Atributo que almacena el Registro de Nomina
		/// </summary>
		public int id_nomina { get { return this._id_nomina; } }
		private int _id_compania_emisor;
		/// <summary>
		/// Atributo que almacena la Compania Emisora
		/// </summary>
		public int id_compania_emisor { get { return this._id_compania_emisor; } }
		private int _no_consecutivo;
		/// <summary>
		/// Atributo que almacena el Consecutivo de la Nómina
		/// </summary>
		public int no_consecutivo { get { return this._no_consecutivo; } }
		private int _id_nomina_origen;
		/// <summary>
		/// Atributo que almacena el Id de Nómina Origen
		/// </summary>
		public int id_nomina_origen { get { return this._id_nomina_origen; } }
		private string _version;
		/// <summary>
		/// Atributo que almacena la versión de la Nómina
		/// </summary>
		public string version { get { return this._version; } }
		private byte _id_tipo_nomina;
		/// <summary>
		/// Atributo que almacena el Tipo de Nómina
		/// </summary>
		public byte id_tipo_nomina { get { return this._id_tipo_nomina; } }
		private DateTime _fecha_pago;
		/// <summary>
		/// Atributo que almacena la Fecha de Pago
		/// </summary>
		public DateTime fecha_pago { get { return this._fecha_pago; } }
		private DateTime _fecha_inicial_pago;
		/// <summary>
		/// Atributo que almacena la Fecha Inicial de Pago
		/// </summary>
		public DateTime fecha_inicial_pago { get { return this._fecha_inicial_pago; } }
		private DateTime _fecha_final_pago;
		/// <summary>
		/// Atributo que almacena la Fecha Final de Pago    
		/// </summary>
		public DateTime fecha_final_pago { get { return this._fecha_final_pago; } }
		private DateTime _fecha_nomina;
		/// <summary>
		/// Atributo que almacena la Fecha de Nómina
		/// </summary>
		public DateTime fecha_nomina { get { return this._fecha_nomina; } }
		private decimal _dias_pago;
		/// <summary>
		/// Atributo que almacena los Dias de Pago
		/// </summary>
		public decimal dias_pago { get { return this._dias_pago; } }
		private int _id_sucursal;
		/// <summary>
		/// Atributo que almacena la Sucursal
		/// </summary>
		public int id_sucursal { get { return this._id_sucursal; } }
		private byte _id_periodicidad_pago;
		/// <summary>
		/// Atributo que almacena la Periodicidad de Pago
		/// </summary>
		public byte id_periodicidad_pago { get { return this._id_periodicidad_pago; } }
		private byte _id_metodo_pago;
		/// <summary>
		/// Atributo que almacena el Método de Pago
		/// </summary>
		public byte id_metodo_pago { get { return this._id_metodo_pago; } }
		private string _registro_patronal;
		/// <summary>
		/// Atributo que almacena eL Registro Patronal
		/// </summary>
		public string registro_patronal { get { return this._registro_patronal; } }
		private string _curp;
		/// <summary>
		/// Atributo que almacena el Curp
		/// </summary>
		public string curp { get { return this._curp; } }
		private string _rfc_patron;
		/// <summary>
		/// Atributo que almacena el RFC del Patron
		/// </summary>
		public string rfc_patron { get { return this._rfc_patron; } }
		private bool _habilitar;
		/// <summary>
		/// Atributo que almacena el Estatus Habilitar
		/// </summary>
		public bool habilitar { get { return this._habilitar; } }
		#endregion

		#region Constructores

		/// <summary>
		/// Constructor encargado de Inicializar los Atributos por Defecto
		/// </summary>
		public Nomina12()
		{
			//Asignando Atributos
			this._id_nomina = 0;
			this._id_compania_emisor = 0;
			this._no_consecutivo = 0;
			this._id_nomina_origen = 0;
			this._version = "";
			this._id_tipo_nomina = 0;
			this._fecha_pago = DateTime.MinValue;
			this._fecha_inicial_pago = DateTime.MinValue;
			this._fecha_final_pago = DateTime.MinValue;
			this._fecha_nomina = DateTime.MinValue;
			this._dias_pago = 0.00M;
			this._id_sucursal = 0;
			this._id_periodicidad_pago = 0;
			this._id_metodo_pago = 0;
			this._registro_patronal = "";
			this._curp = "";
			this._rfc_patron = "";
			this._habilitar = false;
		}
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_nomina">Id de Nomina</param>
		public Nomina12(int id_nomina)
		{
			//Invocando Método de Carga
			cargaAtributosInstancia(id_nomina);
		}

		#endregion

		#region Destructores

		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~Nomina12()
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
		private bool cargaAtributosInstancia(int id_nomina)
		{
			//Declarando Objeto de Retorno
			bool result = false;

			//Armando Arreglo de Parametros
			object[] param = { 3, id_nomina, 0, 0, 0, "", 0, null, null, null, null, 0, 0, 0, 0, "", "", "", 0, false, "", "" };

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
						this._id_nomina = id_nomina;
						this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
						this._no_consecutivo = Convert.ToInt32(dr["NoConsecutivo"]);
						this._id_nomina_origen = Convert.ToByte(dr["IdNominaOrigen"]);
						this._version = dr["Version"].ToString();
						this._id_tipo_nomina = Convert.ToByte(dr["IdTipoNomina"]);
						DateTime.TryParse(dr["FechaPago"].ToString(), out this._fecha_pago);
						DateTime.TryParse(dr["FechaInicialPago"].ToString(), out this._fecha_inicial_pago);
						DateTime.TryParse(dr["FechaFinalPago"].ToString(), out this._fecha_final_pago);
						DateTime.TryParse(dr["FechaNomina"].ToString(), out this._fecha_nomina);
						this._dias_pago = Convert.ToDecimal(dr["DiasPago"]);
						this._id_sucursal = Convert.ToInt32(dr["IdSucursal"]);
						this._id_periodicidad_pago = Convert.ToByte(dr["IdPeriodicidadPago"]);
						this._id_metodo_pago = Convert.ToByte(dr["IdMetodoPago"]);
						this._registro_patronal = dr["RegistroPattronal"].ToString();
						this._curp = dr["Curp"].ToString();
						this._rfc_patron = dr["RfcPatron"].ToString();
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
		/// <param name="id_compania_emisora">Compania Emisora</param>
		/// <param name="no_consecutivo">No. Consecutivo por Compania</param>
		/// <param name="id_nomina_origen">Nomina de Origen</param>
		/// <param name="version">Versión de la Nómina</param>
		/// <param name="id_tipo_nomina">Tipo de Nómina (Oridnaria, Extraordinaria)</param>
		/// <param name="fecha_pago">Fecha de Pago</param>
		/// <param name="fecha_inicial_pago">Fecha de Inicio del Pago</param>
		/// <param name="fecha_final_pago">Fecha de de Fin del Pago</param>
		/// <param name="fecha_nomina">Fecha de Nomina</param>
		/// <param name="dias_pago">Dias de Pago</param>
		/// <param name="id_sucursal">Sucursal</param>
		/// <param name="id_periodicidad_pago">Periodicidad de Pago(Quincenal, Semanal, Mensual, etc...)</param>
		/// <param name="id_metodo_pago">Método de Pago(Efectivo, Transferencia, Cheque, etc...)</param>
		/// <param name="registro_patronal">Registro Patronal</param>
		/// <param name="curp">Curp del Emisor</param>
		/// <param name="rfc_patron">RFC del Patron</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <param name="habilitar">Estatus Habilitar del Registro</param>
		/// <returns></returns>
		private RetornoOperacion actualizaAtributosBD(int id_compania_emisor, int no_consecutivo, int id_nomina_origen, string version, byte id_tipo_nomina, DateTime fecha_pago, DateTime fecha_inicial_pago, DateTime fecha_final_pago, DateTime fecha_nomina, decimal dias_pago, int id_sucursal, byte id_periodicidad_pago, byte id_metodo_pago, string registro_patronal, string curp, string rfc_patron, int id_usuario, bool habilitar)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Armando Arreglo de Parametros
			object[] param = { 2, this._id_nomina, id_compania_emisor, no_consecutivo, id_nomina_origen, version, id_tipo_nomina, fecha_pago, fecha_inicial_pago,
																 fecha_final_pago, fecha_nomina, dias_pago, id_sucursal, id_periodicidad_pago, id_metodo_pago, registro_patronal, curp, rfc_patron,
																 id_usuario, habilitar, "", ""};

			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Validar si la Nomina Tiene Nominas de Empleado Timbradas
		/// </summary>
		/// <param name="id_nomina">Nomina</param>
		/// <returns></returns>
		private RetornoOperacion validaNominaTimbrada(int id_nomina)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Armando Arreglo de Parametros
			object[] param = { 4, id_nomina, 0, 0, 0, "", 0, null, null, null, null, 0, 0, 0, 0, "", "", "", 0, false, "", "" };
			//Obteniendo resultado del SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan datos
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorriendo Cada Fila
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Validando si esta Timbrada
						if (Convert.ToInt32(dr["Validacion"]) > 0)
							//Instanciando Excepción
							result = new RetornoOperacion(this._id_nomina, "La Nomina ha sido Timbrada", true);
						break;
					}
				}
			}

			//Devolviendo Resultado Obtenido
			return result;
		}
		#endregion

		#region Métodos Públicos
		/// <summary>
		/// Método encargado de Insertar una Nómina
		/// </summary>
		/// <param name="id_compania_emisor">Compania Emisora</param>
		/// <param name="no_consecutivo">No. Consecutivo por Compania</param>
		/// <param name="id_nomina_origen">Nomina de Origen</param>
		/// <param name="version">Versión de la Nómina</param>
		/// <param name="id_tipo_nomina">Tipo de Nómina (Oridnaria, Extraordinaria)</param>
		/// <param name="fecha_pago">Fecha de Pago</param>
		/// <param name="fecha_inicial_pago">Fecha de Inicio del Pago</param>
		/// <param name="fecha_final_pago">Fecha de de Fin del Pago</param>
		/// <param name="fecha_nomina">Fecha de Nomina</param>
		/// <param name="dias_pago">Dias de Pago</param>
		/// <param name="id_sucursal">Sucursal</param>
		/// <param name="id_periodicidad_pago">Periodicidad de Pago(Quincenal, Semanal, Mensual, etc...)</param>
		/// <param name="id_metodo_pago">Método de Pago(Efectivo, Transferencia, Cheque, etc...)</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaNomina(int id_compania_emisor, int no_consecutivo, int id_nomina_origen, string version, byte id_tipo_nomina, DateTime fecha_pago, DateTime fecha_inicial_pago, DateTime fecha_final_pago, DateTime fecha_nomina, decimal dias_pago, int id_sucursal, byte id_periodicidad_pago, byte id_metodo_pago, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Obtenemos Valores
			string RegistroPatronal = Global.Referencia.CargaReferencia("0", 25, id_compania_emisor, "Recibo Nómina", "Registro Patronal");
			string Curp = Global.Referencia.CargaReferencia("0", 25, id_compania_emisor, "Recibo Nómina", "Curp");
			string RfcPatron = "";
			//Armando Arreglo de Parametros
			object[] param = { 1, 0, id_compania_emisor, no_consecutivo, id_nomina_origen, version, id_tipo_nomina, fecha_pago, fecha_inicial_pago, fecha_final_pago, fecha_nomina, dias_pago, id_sucursal, id_periodicidad_pago, id_metodo_pago, RegistroPatronal, Curp, RfcPatron, id_usuario, true, "", "" };

			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Actualizar los Atributos en BD
		/// </summary>
		/// <param name="id_compania_emisora">Compania Emisora</param>
		/// <param name="no_consecutivo">No. Consecutivo por Compania</param>
		/// <param name="id_nomina_origen">Nomina de Origen</param>
		/// <param name="version">Versión de la Nómina</param>
		/// <param name="id_tipo_nomina">Tipo de Nómina (Oridnaria, Extraordinaria)</param>
		/// <param name="fecha_pago">Fecha de Pago</param>
		/// <param name="fecha_inicial_pago">Fecha de Inicio del Pago</param>
		/// <param name="fecha_final_pago">Fecha de de Fin del Pago</param>
		/// <param name="fecha_nomina">Fecha de Nomina</param>
		/// <param name="dias_pago">Dias de Pago</param>
		/// <param name="id_sucursal">Sucursal</param>
		/// <param name="id_periodicidad_pago">Periodicidad de Pago(Quincenal, Semanal, Mensual, etc...)</param>
		/// <param name="id_metodo_pago">Método de Pago(Efectivo, Transferencia, Cheque, etc...)</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion EditaNomina(int id_compania_emisor, int no_consecutivo, int id_nomina_origen, string version, byte id_tipo_nomina, DateTime fecha_pago,DateTime fecha_inicial_pago, DateTime fecha_final_pago, DateTime fecha_nomina, decimal dias_pago, int id_sucursal, byte id_periodicidad_pago, byte id_metodo_pago, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Obtenemos Valores
			string RegistroPatronal = Global.Referencia.CargaReferencia("0", 25, id_compania_emisor, "Recibo Nómina", "Registro Patronal");
			string Curp = Global.Referencia.CargaReferencia("0", 25, id_compania_emisor, "Recibo Nómina", "Curp");
			string RfcPatron = "";
			//Creamos la transacción 
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Validando Nomina Timbrada
				result = this.validaNominaTimbrada(this._id_nomina);

				//Operación Exitosa?
				if (!result.OperacionExitosa)
				{
					//Devolviendo Resultado Obtenido
					result = this.actualizaAtributosBD(id_compania_emisor, no_consecutivo, id_nomina_origen, version, id_tipo_nomina, fecha_pago, fecha_inicial_pago, fecha_final_pago, fecha_nomina, dias_pago, id_sucursal, id_periodicidad_pago, id_metodo_pago, RegistroPatronal, Curp, RfcPatron, id_usuario, true);
					//Validamos  Resultado
					if (result.OperacionExitosa)
					{
						//Cargamos Empleado ligado a la Nómina
						using (DataTable mit = NomEmpleado.ObtieneNominasEmpleadoRegistrados(this._id_nomina))
						{
							//Validamos Orifen de Datos
							if (Validacion.ValidaOrigenDatos(mit))
							{
								//Recorremos Empleados
								foreach (DataRow r in mit.Rows)
								{
									//Actualizamos Tipo de Nómina
									result = NomEmpleado.ActualizaEncabezadoNominaEsquema(this._id_nomina, r.Field<int>("Id"), id_usuario);
								}
							}
						}
					}
				}
				else
					//Instanciando Excepción
					result = new RetornoOperacion(result.Mensaje);
				//Validamos Resultado
				if (result.OperacionExitosa)
				{
					//Asignamos Valor de Resultado
					result = new RetornoOperacion(this._id_nomina, result.Mensaje, result.OperacionExitosa);
					//Terminamos Transacción
					scope.Complete();
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Deshabilitar la Nomina
		/// </summary>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaNomina(int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Declarando Ambiente Transaccional
			using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Validando Nomina Timbrada
				result = this.validaNominaTimbrada(this._id_nomina);
				//Operación Exitosa?
				if (!result.OperacionExitosa)
				{
					//Obteniendo Nomina de Empleados
					using (DataTable dtNominasEmpleado = NomEmpleado.ObtieneNominasEmpleadoRegistrados(this._id_nomina))
					{
						//Validando que existan Nominas de Empleados
						if (Validacion.ValidaOrigenDatos(dtNominasEmpleado))
						{
							//Recorriendo Nomina de Empleados
							foreach (DataRow dr in dtNominasEmpleado.Rows)
							{
								//Instanciando Nomina de Empleados
								using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(dr["Id"])))
								{
									//Validando que exista el Registro
									if (ne.habilitar)
									{
										//Deshabilitando Nomina de Empleado
										result = ne.DeshabilitaNomEmpleado(id_usuario);
										//Si la Operación no fue Correcta
										if (!result.OperacionExitosa)
											//Terminando Ciclo
											break;
									}
								}
							}
						}
						else
							//Instanciando Nomina
							result = new RetornoOperacion(this._id_nomina);
					}
					//Validando Operación
					if (result.OperacionExitosa)
					{
						//Devolviendo Resultado Obtenido
						result = this.actualizaAtributosBD(this._id_compania_emisor, this._no_consecutivo, this._id_nomina_origen, this._version, this._id_tipo_nomina, this._fecha_pago, this._fecha_inicial_pago, this._fecha_final_pago, this._fecha_nomina, this._dias_pago, this._id_sucursal, this._id_periodicidad_pago, this._id_metodo_pago, this._registro_patronal, this._curp, this._rfc_patron, id_usuario, false);
						//Validando Operación
						if (result.OperacionExitosa)
							//Completando Transacción
							trans.Complete();
					}
				}
				else
					//Instanciando Excepción
					result = new RetornoOperacion(result.Mensaje);
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Actualizar los Valores
		/// </summary>
		/// <returns></returns>
		public bool ActualizaNom()
		{
			//Invocando Método de Carga
			return this.cargaAtributosInstancia(this._id_nomina);
		}
		/// <summary>
		/// Método encargado de Copiar la Nomina
		/// </summary>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion CopiaNomina(int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Declarando Variables Auxiliares
			int idNomina = 0, idNominaEmpleado = 0;
			//Declarando Ambiente Transaccional
			using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				string RegistroPatronal = Global.Referencia.CargaReferencia("0", 25, id_compania_emisor, "Recibo Nómina", "Registro Patronal");
				string Curp = Global.Referencia.CargaReferencia("0", 25, id_compania_emisor, "Recibo Nómina", "Curp");
				string RfcPatron = "";
				object[] param = { 1, 0, this._id_compania_emisor, 0, 0, "1.2", this._id_tipo_nomina, null, null, null, null, this._dias_pago, this._id_sucursal, this._id_periodicidad_pago, this._id_metodo_pago, RegistroPatronal, Curp, RfcPatron, id_usuario, true, "", "" };
				//Ejecutando SP
				result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
				//Validando Operación Correcta
				if (result.OperacionExitosa)
				{
					//Guardando Nomina
					idNomina = result.IdRegistro;
					//Obteniendo Nominas de Empleado
					using (DataTable dtNominaEmpleado = NomEmpleado.ObtieneIdsNominasEmpleado(this._id_nomina))
					{
						//Validando que Existen Nominas
						if (Validacion.ValidaOrigenDatos(dtNominaEmpleado))
						{
							//Iniciando Ciclo de Nomina de Empleados
							foreach (DataRow drNE in dtNominaEmpleado.Rows)
							{
								//Instanciando Nomina de Empleado
								using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(drNE["Id"])))
								{
									//Validando que existe el Registro
									if (ne.habilitar)
									{
										//Insertando Nomina de Empleado
										result = NomEmpleado.InsertaNominaEmpleado(idNomina, ne.id_empleado, id_usuario);
										//Validando Operación Correcta
										if (result.OperacionExitosa)
										{
											//Guardando Nomina
											idNominaEmpleado = result.IdRegistro;
											//Obteniendo los Encabezados Percepcion, Deduccion,Horas Extras, Incapacidades,
											using (DataTable dtDetallesEmp = EsquemaRegistro.ObtieneEncabezados(ne.id_nomina_empleado))
											{
												//Validando que Existen Nominas
												if (Validacion.ValidaOrigenDatos(dtDetallesEmp))
												{
													//Iniciando Ciclo de Detalle
													foreach (DataRow drDE in dtDetallesEmp.Rows)
													{
														//Instanciando Esquema Registro
														using (EsquemaRegistro dne = new EsquemaRegistro(Convert.ToInt32(drDE["Id"])))
														{
															//Validando Registro
															if (dne.habilitar)
															{
																//Insertando Detalle
																result = EsquemaRegistro.InsertaEsquemaRegistro(dne.id_esquema, 0, idNominaEmpleado, dne.valor, dne.id_tabla_catalogo, dne.id_tipo_catalogo, dne.id_valor, id_usuario);
																int id_registro_superior = result.IdRegistro;
																//Validamos Resultado
																if (result.OperacionExitosa)
																{
																	//Método encargado de Registrar Sub Nodos
																	result = CopiaRegistroSuperior(id_usuario, idNominaEmpleado, dne.id_esquema_registro, id_registro_superior);
																}
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
													result = new RetornoOperacion(idNomina);
											}
											//Ligamos Horas Extras
											if (result.OperacionExitosa)
											{
												//Instnacimoas Percepcion de Tipo Horas Extras
												using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoPercepcion", "Percepcion", "Percepciones"), idNominaEmpleado, "019")))
												{
													//Obtiene Detalles Horas Extras
													using (DataTable mitDetalles = EsquemaRegistro.ObtieneDetalleHorasExtras(idNominaEmpleado, 0))
													{
														//Validando que Existen Nominas
														if (Validacion.ValidaOrigenDatos(mitDetalles))
														{
															//Iniciando Ciclo de Detalle
															foreach (DataRow det in mitDetalles.Rows)
															{
																using (EsquemaRegistro objHorasExtras = new EsquemaRegistro(Convert.ToInt32(det["Id"])))
																{
																	//Validando Registro
																	if (objHorasExtras.habilitar)
																	{
																		//Insertando Encabezado de Horas Extras
																		result = objHorasExtras.EditaEsquemaRegistro(objHorasExtras.id_esquema, objEsquemaRegistro.id_esquema_superior, objHorasExtras.id_nomina_empleado, objHorasExtras.valor, objHorasExtras.id_tabla_catalogo, objHorasExtras.id_tipo_catalogo, objHorasExtras.id_valor, id_usuario);
																		//Si la Operación no fue Exitosa
																		if (!result.OperacionExitosa)
																			//Terminando Ciclo
																			break;
																	}
																}
															}
														}
													}
												}
											}
										}
										else
											//Terminando Ciclo
											break;
									}
									else
									{
										//Instanciando Nomina
										result = new RetornoOperacion("No Existe la Nómina del Empleado");
										//Terminando Ciclo
										break;
									}
								}
							}
						}
						else
							//Instanciando Nomina
							result = new RetornoOperacion(idNomina);
						//Validando Operaciones
						if (result.OperacionExitosa)
						{
							//Instanciando Nomina
							result = new RetornoOperacion(idNomina);
							//Completando Transacción
							trans.Complete();
						}
					}
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id_usuario"></param>
		/// <param name="id_nomina_empleado"></param>
		/// <param name="id_esquema_registro"></param>
		/// <param name="id_registro_superior"></param>
		/// <returns></returns>
		private static RetornoOperacion CopiaRegistroSuperior(int id_usuario, int id_nomina_empleado, int id_esquema_registro, int id_registro_superior)
		{
			//Declaramos Objeto Resultado
			RetornoOperacion result = new RetornoOperacion(0);
			//Obteniendo los Atributos
			using (DataTable dtDetalles = EsquemaRegistro.ObtieneDetallesEsquemaSuperiorAtributos(id_esquema_registro))
			{
				//Validando que Existen Nominas
				if (Validacion.ValidaOrigenDatos(dtDetalles))
				{
					//Iniciando Ciclo de Detalle
					foreach (DataRow det in dtDetalles.Rows)
					{
						using (EsquemaRegistro objdet = new EsquemaRegistro(Convert.ToInt32(det["Id"])))
						{
							//Validando Registro
							if (objdet.habilitar)
							{
								//Insertando Detalle
								result = EsquemaRegistro.InsertaEsquemaRegistro(objdet.id_esquema, id_registro_superior, id_nomina_empleado, objdet.valor, objdet.id_tabla_catalogo, objdet.id_tipo_catalogo, objdet.id_valor, id_usuario);
								//Si la Operación no fue Exitosa
								if (!result.OperacionExitosa)
									//Terminando Ciclo
									break;
							}
						}
					}
				}
			}
			//Validamos resultado
			if (result.OperacionExitosa)
			{
				//Cargamos Elementos
				using (DataTable dtElementos = EsquemaRegistro.ObtieneDetallesEsquemaSuperiorElementos(id_esquema_registro))
				{
					//Validando que Existen Nominas
					if (Validacion.ValidaOrigenDatos(dtElementos))
					{
						//Iniciando Ciclo de Detalle
						foreach (DataRow dtElem in dtElementos.Rows)
						{
							//Instanciamos Registro
							using (EsquemaRegistro objElem = new EsquemaRegistro(Convert.ToInt32(dtElem["Id"])))
							{
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Validando Registro
									if (objElem.habilitar)
									{
										//Insertando Detalle
										result = EsquemaRegistro.InsertaEsquemaRegistro(objElem.id_esquema, id_registro_superior, id_nomina_empleado, objElem.valor, objElem.id_tabla_catalogo,
															objElem.id_tipo_catalogo, objElem.id_valor, id_usuario);
										int id_registro_superior_detalle = result.IdRegistro;
										//Validamos Resulaltado
										if (result.OperacionExitosa)
										{
											//Método en cargado de regiitrar subNodos
											result = CopiaRegistroSuperior(id_usuario, id_nomina_empleado, objElem.id_esquema_registro, id_registro_superior_detalle);
										}
										//Si la Operación no fue Exitosa
										if (!result.OperacionExitosa)
											//Terminando Ciclo
											break;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		#region CFDI 3.2

		/// <summary>
		/// Método encargado de Validar la Nomina
		/// </summary>
		/// <returns></returns>
		public RetornoOperacion ValidaNominaTimbrada()
		{
			//Invocando Método de Validación
			return this.validaNominaTimbrada(this._id_nomina);
		}
		/// <summary>
		/// Método encargado de Timbrar Toda la Nómina
		/// </summary>
		/// <param name="serie">Serie</param>
		/// <param name="id_cuenta_pago">Cuenta Pago del Emisor</param>
		/// <param name="ruta_xslr_co">Ruta para la cadena Original en Linea</param>
		/// <param name="ruta_xslr_co_local">Ruta para la Cadena Original desconectado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion TimbraNomina_V3_2(string serie, string ruta_xslr_co, string ruta_xslr_co_local, int id_usuario)
		{
			//Declaramos Objeto Resultado
			RetornoOperacion result = new RetornoOperacion(0);
			//Declaramos Objeto Resultado
			RetornoOperacion ResultadoMensaje = new RetornoOperacion(0);

			//Guardamos Mensaje
			string mensaje = "";
			//Instanciando Nomina de Empleados
			using (DataTable dtNominaEmpleados = SAT_CL.Nomina.NomEmpleado.ObtieneNominasEmpleadoRegistrados(this._id_nomina))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(dtNominaEmpleados))
				{
					//Recorriendo Nominas de Empleados
					foreach (DataRow dr in dtNominaEmpleados.Rows)
					{
						//Instanciando Nómina Empleado
						using (SAT_CL.Nomina.NomEmpleado ne = new SAT_CL.Nomina.NomEmpleado(Convert.ToInt32(dr["Id"])))
						{
							//Validando que exista el Registro
							if (ne.habilitar)
							{
								//Validando Esquema de Timbrado
								if (ne.id_comprobante33 == 0)

									//Timbrando Nómina del Empleado
									result = ne.ImportaTimbraNominaEmpleadoComprobante_V3_2(this._version, serie, ruta_xslr_co, ruta_xslr_co_local, id_usuario);
								else
									//Instanciando Excepción
									result = new RetornoOperacion("La Nómina del Empleado fue timbrada en el esquema 3.3");
							}
							else
								//Instanciando Excepción
								result = new RetornoOperacion("No Existe la Nómina del Empleado");
						}
						//Construyendo mensaje de este Timbrado de Nómina de Empleado
						ResultadoMensaje = new RetornoOperacion(mensaje += string.Format("{0}, ", result.Mensaje), true);

					}
				}
			}
			//Devolvemos Resultado
			return ResultadoMensaje;
		}

		#endregion

		#region CFDI 3.3

		/// <summary>
		/// Método encargado de Timbrar Toda la Nómina
		/// </summary>
		/// <param name="serie">Serie</param>
		/// <param name="id_cuenta_pago">Cuenta Pago del Emisor</param>
		/// <param name="ruta_xslr_co">Ruta para la cadena Original en Linea</param>
		/// <param name="ruta_xslr_co_local">Ruta para la Cadena Original desconectado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion TimbraNomina_V3_3(string serie, string ruta_xslr_co, string ruta_xslr_co_local, int id_usuario)
		{
			//Declaramos Objeto Resultado
			RetornoOperacion result = new RetornoOperacion(0);
			//Declaramos Objeto Resultado
			RetornoOperacion ResultadoMensaje = new RetornoOperacion(0);

			//Guardamos Mensaje
			string mensaje = "";
			//Instanciando Nomina de Empleados
			using (DataTable dtNominaEmpleados = SAT_CL.Nomina.NomEmpleado.ObtieneNominasEmpleadoRegistrados(this._id_nomina))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(dtNominaEmpleados))
				{
					//Recorriendo Nominas de Empleados
					foreach (DataRow dr in dtNominaEmpleados.Rows)
					{
						//Instanciando Nómina Empleado
						using (SAT_CL.Nomina.NomEmpleado ne = new SAT_CL.Nomina.NomEmpleado(Convert.ToInt32(dr["Id"])))
						{
							//Validando que exista el Registro
							if (ne.habilitar && ne.estatus == NomEmpleado.Estatus.Registrado)
							{
								//Validando Timbrado Anterior
								if (ne.id_comprobante == 0)

									//Timbrando Nómina del Empleado
									result = ne.ImportaTimbraNominaEmpleadoComprobante_V3_3(this._version, serie, ruta_xslr_co, ruta_xslr_co_local, id_usuario);
								else
									//Instanciando Excepción
									result = new RetornoOperacion("La Nómina del Empleado fue timbrada en el esquema 3.2");
							}
							else
								//Instanciando Excepción
								result = new RetornoOperacion("No Existe la Nómina del Empleado");
						}
						//Construyendo mensaje de este Timbrado de Nómina de Empleado
						ResultadoMensaje = new RetornoOperacion(mensaje += string.Format("{0}, ", result.Mensaje), true);

					}
				}
			}
			//Devolvemos Resultado
			return ResultadoMensaje;
		}

		#endregion

		#endregion
	}
}
