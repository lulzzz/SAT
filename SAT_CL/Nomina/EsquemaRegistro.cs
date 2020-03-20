using System;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Global;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT_CL.Nomina {
	/// <summary>
	///  Implementa los método para la administración  del Esquema de Registro
	/// </summary> 
	public class EsquemaRegistro : Disposable {
		#region Atributos
		/// <summary>
		/// Atributo encargado de Almacenar el Nombre del SP
		/// </summary>
		private static string _nom_sp = "nomina.sp_esquema_registro_ter";
		private int _id_esquema_registro;
		/// <summary>
		/// Atributo que almacena el  Id de Esquema Registro
		/// </summary>
		public int id_esquema_registro { get { return this._id_esquema_registro; } }
		private int _id_esquema;
		/// <summary>
		/// Atributo que almacena el  Id de Esquema
		/// </summary>
		public int id_esquema { get { return this._id_esquema; } }
		private int _id_esquema_superior;
		/// <summary>
		/// Atributo que almacena el  Id de Esquema
		/// </summary>
		public int id_esquema_superior { get { return this._id_esquema_superior; } }
		private int _id_nomina_empleado;
		/// <summary>
		/// Atributo que almacena el  Id de Nomina Empelado
		/// </summary>
		public int id_nomina_empleado { get { return this._id_nomina_empleado; } }
		private string _valor;
		/// <summary>
		/// Atributo que almacena el Valor
		/// </summary>
		public string valor { get { return this._valor; } }
		private int _id_tabla_catalogo;
		/// <summary>
		/// Atributo que almacena el Id Tabla Catalogo
		/// </summary>
		public int id_tabla_catalogo { get { return this._id_tabla_catalogo; } }
		private int _id_tipo_catalogo;
		/// <summary>
		/// Atributo que almacena el  Id Tipo de Catalogo
		/// </summary>
		public int id_tipo_catalogo { get { return this._id_tipo_catalogo; } }
		private int _id_valor;
		/// <summary>
		/// Atributo que almacena el  Id Valor
		/// </summary>
		public int id_valor { get { return this._id_valor; } }
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
		public EsquemaRegistro()
		{
			//Asignando Atributos
			this._id_esquema_registro = 0;
			this._id_esquema = 0;
			this._id_esquema_superior = 0;
			this._id_nomina_empleado = 0;
			this._valor = "";
			this._id_tabla_catalogo = 0;
			this._id_tipo_catalogo = 0;
			this._id_valor = 0;
			this._habilitar = false;
		}
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_esquema_registro"></param>
		public EsquemaRegistro(int id_esquema_registro)
		{
			//Invocando Método de Carga
			cargaAtributosInstancia(id_esquema_registro);
		}
		#endregion
		#region Destructores
		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~EsquemaRegistro()
		{
			Dispose(false);
		}
		#endregion
		#region Métodos Privados
		/// <summary>
		/// Método encargado de Cargar los Atributos
		/// </summary>
		/// <param name="id_esquema_registro"></param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_esquema_registro)
		{
			//Declarando Objeto de Retorno
			bool result = false;
			//Armando Arreglo de Parametros
			object[] param = { 3, id_esquema_registro, 0, 0, 0, "", 0, 0, 0, 0, false, "", "" };
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
						this._id_esquema = id_esquema;
						//Asignando Atributos
						this._id_esquema_registro = id_esquema_registro;
						this._id_esquema = Convert.ToInt32(dr["IdEsquema"]);
						this._id_esquema_superior = Convert.ToInt32(dr["IdEsquemaSuperior"]);
						this._id_nomina_empleado = Convert.ToInt32(dr["IdNominaEmpleado"]);
						this._valor = dr["Valor"].ToString();
						this._id_tabla_catalogo = Convert.ToInt32(dr["IdTablaCatalogo"]);
						this._id_tipo_catalogo = Convert.ToInt32(dr["IdTipoCatalogo"]);
						this._id_valor = Convert.ToInt32(dr["IdValor"]); ;
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
		/// Actualiza el Esquema de Registro
		/// </summary>
		/// <param name="id_esquema">Id Esquema</param>
		/// <param name="id_esquema_supeior">Esquema Superior</param>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <param name="valor">Valor</param>
		/// <param name="id_tabla_catalogo">Id Tabla Catalogo</param>
		/// <param name="id_tipo_catalogo">Id Tipo Catalogo</param>
		/// <param name="id_valor">Id Valor de acuerdo ala Tabla Catalogo o Tipo</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <param name="habilitar">Habilitar</param>
		/// <returns></returns>
		private RetornoOperacion actualizaAtributosBD(int id_esquema, int id_esquema_supeior, int id_nomina_empleado, string valor, int id_tabla_catalogo, int id_tipo_catalogo, int id_valor, int id_usuario, bool habilitar)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Armando Arreglo de Parametros
			object[] param = { 2, this._id_esquema_registro, id_esquema, id_esquema_supeior, id_nomina_empleado, valor, id_tabla_catalogo, id_tipo_catalogo, id_valor, id_usuario, habilitar, "", "" };
			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
			//Devolviendo Resultado Obtenido
			return result;
		}
		#endregion
		
		#region Métodos Públicos
		
		#region RetornoOperacion
		/// <summary>
		/// Método encargado de actualizar solo el atributo Valor del registro
		/// </summary>
		/// <param name="nuevoValor"></param>
		/// <param name="idUsuario"></param>
		/// <returns></returns>
		public RetornoOperacion ActualizaValor(string nuevoValor, int idUsuario)
		{
			return this.actualizaAtributosBD(this._id_esquema, this._id_esquema_superior, this._id_nomina_empleado, nuevoValor, this._id_tabla_catalogo, this._id_tipo_catalogo, this._id_valor, idUsuario, this._habilitar);
		}
		/// <summary>
		/// Método encargado de Editar un Esquema Registro
		/// </summary>
		/// <param name="id_esquema">Id Esquema</param>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="valor">Valor</param>
		/// <param name="id_tabla_catalogo">Id Tabla Catalogo</param>
		/// <param name="id_tipo_catalogo">Id Tipo de Catalogo</param>
		/// <param name="id_valor">Id Valor de acuerdo ala Tabla Catalogo o Tipo</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditaEsquemaRegistro(int id_esquema, int id_esquema_superior, int id_nomina_empleado, string valor, int id_tabla_catalogo, int id_tipo_catalogo, int id_valor, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Devolviendo Resultado Obtenido
			result = this.actualizaAtributosBD(id_esquema, id_esquema_superior, id_nomina_empleado, valor, id_tabla_catalogo, id_tipo_catalogo, id_valor, id_usuario, this._habilitar);
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Deshabilitar un Esquema Registro
		/// </summary>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaEsquemaRegistro(int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Devolviendo Resultado Obtenido
			result = this.actualizaAtributosBD(this._id_esquema, this._id_esquema_superior, this._id_nomina_empleado, this._valor, this._id_tabla_catalogo, this._id_tipo_catalogo, this._id_valor, id_usuario, false);
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Deshabilitar un Esquema Registro (deshabilitando los registros conincidentes con el Id Agrupador)
		/// </summary>
		/// <param name="id_usuario"></param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaEsquemaRegistroSuperior(string version, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion(0);
			//Declarando Ambiente Transaccional
			using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Intsnaciamos Nòmina Empleado
				using (NomEmpleado objNominaEmpleado = new NomEmpleado(this._id_nomina_empleado))
				{
					//Validando que exista un Comprobante
					if (!(objNominaEmpleado.id_comprobante != 0 && (SAT_CL.Nomina.NomEmpleado.Estatus)objNominaEmpleado.id_estatus == SAT_CL.Nomina.NomEmpleado.Estatus.Timbrado))
					{
						//Instnacimoas Percepcion de Tipo Horas Extras
						using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoPercepcion", "Percepcion", "Percepciones"), id_nomina_empleado, "019")))
						{
							//Si el Concepto es Horas Extras
							if (this._id_esquema_registro == objEsquemaRegistro.id_esquema_superior)
							{
								//Validamos que no existan detalla
								if (Validacion.ValidaOrigenDatos(ObtieneDetalleHorasExtras(this._id_nomina_empleado, this._id_esquema_registro)))
								{
									//Mostramos Error
									result = new RetornoOperacion("Elimine los detalle de Horas Extras.");
								}
							}
							//Validamos resultado
							if (result.OperacionExitosa)
							{
								//Carga Registros Agrupadores
								using (DataTable mit = ObtieneDetallesEsquemaSuperiorAtributos(this._id_esquema_registro))
								{
									//Validamos Origen de Datos
									if (Validacion.ValidaOrigenDatos(mit))
									{
										//Recorriendo Nomina de Empleados
										foreach (DataRow dr in mit.Rows)
										{
											//Instanciando Nomina de Empleados
											using (EsquemaRegistro ne = new EsquemaRegistro(Convert.ToInt32(dr["Id"])))
											{
												//Validando que exista el Registro
												if (ne.habilitar)
												{
													//Deshabilitando Nomina de Empleado
													result = ne.DeshabilitaEsquemaRegistro(id_usuario);
													//Si la Operación no fue Correcta
													if (!result.OperacionExitosa)
														//Terminando Ciclo
														break;
												}
											}
										}
									}
									//Validamos Resultado
									if (result.OperacionExitosa)
									{
										//Obtenemos Elemento ligados
										//Carga Registros Agrupadores
										using (DataTable mitElemen = ObtieneDetallesEsquemaSuperiorElementos(this._id_esquema_registro))
										{
											//Valida Origen de Datos
											if (Validacion.ValidaOrigenDatos(mitElemen))
											{
												//Recorriendo Nomina de Empleados
												foreach (DataRow dr in mitElemen.Rows)
												{
													//Instanciando Nomina de Empleados
													using (EsquemaRegistro el = new EsquemaRegistro(Convert.ToInt32(dr["Id"])))
													{
														//Validando que exista el Registro
														if (el.habilitar)
														{
															//Deshabilitando Nomina de Empleado
															result = el.DeshabilitaEsquemaRegistroSuperior(version, id_usuario);
															//Si la Operación no fue Correcta
															if (!result.OperacionExitosa)
																//Terminando Ciclo
																break;
														}
													}
												}
											}
										}
										//Validamos Resultado
										if (result.OperacionExitosa)
										{
											//Devolviendo Resultado Obtenido
											result = this.actualizaAtributosBD(this._id_esquema, this._id_esquema_superior, this._id_nomina_empleado, this._valor, this._id_tabla_catalogo, this._id_tipo_catalogo, this._id_valor, id_usuario, false);
											//Validamos Resultado
											if (result.OperacionExitosa)
											{
												//Actualizamos Totales de Percepciones
												result = ActualizaTotalesPercepciones(version, this._id_nomina_empleado, id_usuario);
												//Validamos Resultado
												if (result.OperacionExitosa)
												{
													//Actualizamos Totales Deducciones
													result = ActualizaTotalesDeducciones(version, id_nomina_empleado, id_usuario);
													//Validamos Resultado
													if (result.OperacionExitosa)
													{
														//Actualizamos Totales de la Nómina
														result = ActualizaTotalesNomina(version, id_nomina_empleado, id_usuario);
													}
												}
											}
										}
									}
									//Validando Operación
									if (result.OperacionExitosa)
										//Completando Transacción
										trans.Complete();
								}
							}
						}
					}
					else
						//Instanciando Excepción
						result = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}

		/// <summary>
		/// Actualizamos la Percepción de la Nómina
		/// </summary>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <param name="id_esquema_superior_subsidio">Id Esquema supeiorior Subsidio</param>
		/// <param name="version">Versión de la Nómina</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_tipo_pago">Id tipo de Percepción</param>
		/// <param name="importe_gravado">Importe Gravado de la Percecpción</param>
		/// <param name="importe_exento">Importe Exento de la percepción</param>
		/// <param name="importe">Importe de la Deducción</param>
		///<param name="bit_subsidio_causado">Validamos si es necesario agregar el Importe Causado</param>
		/// <param name="importe_causado">Añadimos Importe Causado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaDetalleNomina(int id_esquema_superior, int id_esquema_superior_subsidio, string version, int id_nomina_empleado, int id_tipo_pago, decimal importe_gravado, decimal importe_exento, decimal importe, bool bit_subsidio_causado, decimal importe_causado, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion(0);
			//Instanciamos Cobro Recurrente
			using (Liquidacion.TipoCobroRecurrente objTipoCobro = new Liquidacion.TipoCobroRecurrente(id_tipo_pago))
			{
				//Creamos la transacción 
				using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
				{
					//Validamos tipo de Pago Percepcion
					if ((Liquidacion.TipoCobroRecurrente.TipoAplicacion)objTipoCobro.id_tipo_aplicacion == Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion || (Liquidacion.TipoCobroRecurrente.TipoAplicacion)objTipoCobro.id_tipo_aplicacion == Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion)
					{
						//Insertamos Esquema Percepcion  en caso de no existir
						if (id_esquema_superior == 0)
						{
							//Armando Arreglo de Parametros
							object[] param = { 1, 0, 41, 0, id_nomina_empleado, "", 0, 0, 0, id_usuario, true, "", "" };
							//Ejecutando SP
							result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
							//Asignamos Esuqema Superior
							id_esquema_superior = result.IdRegistro; ;
						}
						//Validamos Resultado
						if (result.OperacionExitosa)
						{
							//Actualizamos Tipo Percepción
							result = ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoPercepcion", "Percepcion", "Percepciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "TipoPercepcion", "Percepcion", "Percepciones"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(92, objTipoCobro.id_concepto_sat_nomina), 78, 0, id_tipo_pago, id_usuario);
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Actualizamos Concepto
								result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Concepto", "Percepcion", "Percepciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Concepto", "Percepcion", "Percepciones"), id_nomina_empleado, objTipoCobro.descripcion, 78, 0, id_tipo_pago, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Actualizamos Clave
									result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Clave", "Percepcion", "Percepciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Clave", "Percepcion", "Percepciones"), id_nomina_empleado, objTipoCobro.clave_nomina, 78, 0, id_tipo_pago, id_usuario);
								}
							}
						}
						if (result.OperacionExitosa)
						{
							//Actualizamos Importe Gravado
							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "ImporteGravado", "Percepcion", "Percepciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "ImporteGravado", "Percepcion", "Percepciones"), id_nomina_empleado, importe_gravado.ToString(), 0, 0, 0, id_usuario);
							if (result.OperacionExitosa)
							{
								//Actualizamos Importe Gravado
								result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "ImporteExento", "Percepcion", "Percepciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "ImporteExento", "Percepcion", "Percepciones"), id_nomina_empleado, importe_exento.ToString(), 0, 0, 0, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Actualizamos Totales de Percepciones
									result = ActualizaTotalesPercepciones(version, id_nomina_empleado, id_usuario);
								}
							}
						}
					}
					else
							//Validamos tipo de Pago Deducción
							if ((Liquidacion.TipoCobroRecurrente.TipoAplicacion)objTipoCobro.id_tipo_aplicacion == Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion || (Liquidacion.TipoCobroRecurrente.TipoAplicacion)objTipoCobro.id_tipo_aplicacion == Liquidacion.TipoCobroRecurrente.TipoAplicacion.Descuento)
					{
						//Insertamos Esquema Percepcion  en caso de no existir
						if (id_esquema_superior == 0)
						{
							//Armando Arreglo de Parametros
							object[] param = { 1, 0, 74, 0, id_nomina_empleado, "", 0, 0, 0, id_usuario, true, "", "" };
							//Ejecutando SP
							result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
							//Asignamos Esuqema Superior
							id_esquema_superior = result.IdRegistro;
						}
						//Validamos Resultado
						if (result.OperacionExitosa)
						{
							//Actualizamos Tipo Deducción
							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoDeduccion", "Deduccion", "Deducciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "TipoDeduccion", "Deduccion", "Deducciones"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(91, objTipoCobro.id_concepto_sat_nomina), 78, 0, id_tipo_pago, id_usuario);
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Actualizamos Concepto
								result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Concepto", "Deduccion", "Deducciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Concepto", "Deduccion", "Deducciones"), id_nomina_empleado, objTipoCobro.descripcion, 78, 0, id_tipo_pago, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Actualizamos Clave
									result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Clave", "Deduccion", "Deducciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Clave", "Deduccion", "Deducciones"), id_nomina_empleado, objTipoCobro.clave_nomina, 78, 0, id_tipo_pago, id_usuario);
									if (result.OperacionExitosa)
									{
										//Actualizamos Importe 
										result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Importe", "Deduccion", "Deducciones"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Importe", "Deduccion", "Deducciones"), id_nomina_empleado, importe.ToString(), 0, 0, 0, id_usuario);
										//Validamos Resultado
										if (result.OperacionExitosa)
										{
											//Actualizamos Totales Deducciones
											result = ActualizaTotalesDeducciones(version, id_nomina_empleado, id_usuario);
										}
									}
								}
							}
						}
					}
					//Otros Pagos
					else if ((Liquidacion.TipoCobroRecurrente.TipoAplicacion)objTipoCobro.id_tipo_aplicacion == Liquidacion.TipoCobroRecurrente.TipoAplicacion.Otros)
					{
						//Insertamos Esquema Percepcion  en caso de no existir
						if (id_esquema_superior == 0)
						{
							//Armando Arreglo de Parametros
							object[] param = { 1, 0, 82, 0, id_nomina_empleado, "", 0, 0, 0, id_usuario, true, "", "" };
							//Ejecutando SP
							result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
							//Asignamos Esuqema Superior
							id_esquema_superior = result.IdRegistro;
						}
						//Validamos Resultado
						if (result.OperacionExitosa)
						{
							//Actualizamos Tipo OtroPago
							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoOtroPago", "OtroPago", "OtrosPagos"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "TipoOtroPago", "OtroPago", "OtrosPagos"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(3188, objTipoCobro.id_concepto_sat_nomina), 78, 0, id_tipo_pago, id_usuario);
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Actualizamos Concepto
								result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Concepto", "OtroPago", "OtrosPagos"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Concepto", "OtroPago", "OtrosPagos"), id_nomina_empleado, objTipoCobro.descripcion, 78, 0, id_tipo_pago, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Actualizamos Clave
									result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Clave", "OtroPago", "OtrosPagos"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Clave", "OtroPago", "OtrosPagos"), id_nomina_empleado, objTipoCobro.clave_nomina, 78, 0, id_tipo_pago, id_usuario);
									if (result.OperacionExitosa)
									{
										//Actualizamos Importe 
										result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Importe", "OtroPago", "OtrosPagos"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Importe", "OtroPago", "OtrosPagos"), id_nomina_empleado, importe.ToString(), 0, 0, 0, id_usuario);
										//Validamos Resultado
										if (result.OperacionExitosa)
										{
											//Encaso de se Requerido
											if (bit_subsidio_causado)
											{
												//Insertamos Esquema Percepcion  en caso de no existir
												if (id_esquema_superior_subsidio == 0)
												{
													//Armando Arreglo de Parametros
													object[] param = { 1, 0, 83, id_esquema_superior, id_nomina_empleado, "", 0, 0, 0, id_usuario, true, "", "" };
													//Ejecutando SP
													result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
													//Asignamos Esuqema Superior
													id_esquema_superior_subsidio = result.IdRegistro; ;
												}
												//Actualizamos Subsidio Causado
												result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "SubsidioCausado", "SubsidioAlEmpleo", "OtroPago"), id_nomina_empleado, id_esquema_superior_subsidio), id_esquema_superior_subsidio, Esquema.ObtieneIdEsquema(version, "SubsidioCausado", "SubsidioAlEmpleo", "OtroPago"), id_nomina_empleado, importe_causado.ToString(), 0, 0, 0, id_usuario);
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
						//Actualizamos Totales de la Nómina
						result = ActualizaTotalesNomina(version, id_nomina_empleado, id_usuario);
					}
					//Validamos resultado
					if (result.OperacionExitosa)
					{
						scope.Complete();
						result = new RetornoOperacion(id_esquema_superior);
					}
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Insertar o Actualizar un Esquema Registro 
		/// </summary>
		/// <param name="id_esquema_registro">Id Esquema Registro (si es mayor a 0 es edición de lo contrario se Inserta</param>
		/// <param name="id_esquema">Id Esquema</param>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="valor">Valor</param>
		/// <param name="id_tabla_catalogo">Id Tabla Catalogo</param>
		/// <param name="id_tipo_catalogo">Id Tipo de Catalogo</param>
		/// <param name="id_valor">Id Valor de acuerdo ala Tabla Catalogo o Tipo</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaEsquemaRegistro(int id_esquema_registro, int id_esquema_superior, int id_esquema, int id_nomina_empleado, string valor, int id_tabla_catalogo, int id_tipo_catalogo, int id_valor, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion(0);
			//Si Existe el Esquema 
			if (id_esquema_registro > 0)
			{
				//Intsanciamos Esquema Registro
				using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(id_esquema_registro))
				{
					//Validamos Exista el Esquema Registro
					if (objEsquemaRegistro.id_esquema_registro > 0)
					{
						//Edita Esquema
						result = objEsquemaRegistro.EditaEsquemaRegistro(id_esquema, id_esquema_superior, id_nomina_empleado, valor, id_tabla_catalogo, id_tipo_catalogo, id_valor, id_usuario);
					}
					else
					{
						//Mostrando Resultado
						result = new RetornoOperacion("No se encontró datos complementarios del Esquema Registro.");
					}
				}
			}
			else
			{
				//Inserta Esquema
				result = InsertaEsquemaRegistro(id_esquema, id_esquema_superior, id_nomina_empleado, valor, id_tabla_catalogo, id_tipo_catalogo, id_valor, id_usuario);
			}
			//Devolvemos Resultado
			return result;
		}
		/// <summary>
		/// Actualizamos Nodo de Horas Extras
		/// </summary>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="importe_pagado">Importe Pagado</param>
		/// <param name="horas_extras">Horas Extra</param>
		/// <param name="tipo_horas">Tipo Horas</param>
		/// <param name="dias">Dias</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaHorasExtras(int id_esquema_superior, int id_nomina_empleado, string version, int dias, byte id_tipo_horas, int horas_extras, decimal importe_pagado, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion(0);
			//Creamos la transacción 
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Instnacimoas Percepcion de Tipo Horas Extras
				using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoPercepcion", "Percepcion", "Percepciones"), id_nomina_empleado, "019")))
				{
					//Validamos que Exista
					if (objEsquemaRegistro.id_esquema_registro > 0)
					{
						//Insertamos Esquema Percepcion  en caso de no existir
						if (id_esquema_superior == 0)
						{
							//Armando Arreglo de Parametros
							object[] param = { 1, 0, 45, objEsquemaRegistro.id_esquema_superior, id_nomina_empleado, "", 0, 0, 0, id_usuario, true, "", "" };
							//Ejecutando SP
							result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
							//Asignamos Esuqema Superior
							id_esquema_superior = result.IdRegistro; ;
						}
						//Validamos Resultado
						if (result.OperacionExitosa)
						{
							//Actualizamos Dias
							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "Dias", "HorasExtra", "Percepcion"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "Dias", "HorasExtra", "Percepcion"), id_nomina_empleado, dias.ToString(), 0, 0, 0, id_usuario);
							//Validamos Resultado
							if (result.OperacionExitosa)
							{
								//Actualizamos Tipo Horas
								result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoHoras", "HorasExtra", "Percepcion"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "TipoHoras", "HorasExtra", "Percepcion"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(3150, id_tipo_horas), 0, 3150, id_tipo_horas, id_usuario);
								//Validamos Resultado
								if (result.OperacionExitosa)
								{
									//Actualizamos Horas Extras
									result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "HorasExtra", "HorasExtra", "Percepcion"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "HorasExtra", "HorasExtra", "Percepcion"), id_nomina_empleado,
											 horas_extras.ToString(), 0, 0, 0, id_usuario);
									//Validamos Resultado
									if (result.OperacionExitosa)
									{
										//Actualizamos Importe Pagado
										result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "ImportePagado", "HorasExtra", "Percepcion"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "ImportePagado", "HorasExtra", "Percepcion"), id_nomina_empleado, importe_pagado.ToString(), 0, 0, 0, id_usuario);
									}
								}
							}
						}
					}
					else
						result = new RetornoOperacion("Es necesario registrar la Percepción Horas Extras.");
				}
				//Validamos resultado
				if (result.OperacionExitosa)
				{
					scope.Complete();
					result = new RetornoOperacion(id_esquema_superior);
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Actualizamos Incapacidades
		/// </summary>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="version">Versión</param>
		/// <param name="dias">Dias de Incapacidad</param>
		/// <param name="id_tipo">Tipo de Incapacidad</param>
		/// <param name="importe_pagado">Importe Oagado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaIncapacidades(int id_esquema_superior, int id_nomina_empleado, string version, int dias, byte id_tipo, decimal importe_pagado, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion(0);
			//Creamos la transacción 
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Insertamos Esquema Percepcion  en caso de no existir
				if (id_esquema_superior == 0)
				{
					//Armando Arreglo de Parametros
					object[] param = { 1, 0, 94, 0, id_nomina_empleado, "", 0, 0, 0, id_usuario, true, "", "" };
					//Ejecutando SP
					result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
					//Asignamos Esuqema Superior
					id_esquema_superior = result.IdRegistro; ;
				}
				//Validamos Resultado
				if (result.OperacionExitosa)
				{
					//Actualizamos Dias
					result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "DiasIncapacidad", "Incapacidad", "Incapacidades"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "DiasIncapacidad", "Incapacidad", "Incapacidades"), id_nomina_empleado, dias.ToString(), 0, 0, 0, id_usuario);
					//Validamos Resultado
					if (result.OperacionExitosa)
					{
						//Actualizamos Tipo Incapacidad
						result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TipoIncapacidad", "Incapacidad", "Incapacidades"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "TipoIncapacidad", "Incapacidad", "Incapacidades"), id_nomina_empleado, Global.Catalogo.RegresaDescripcioValorCadena(3150, id_tipo), 0, 3150, id_tipo, id_usuario);
						//Validamos Resultado
						if (result.OperacionExitosa)
						{
							//Actualizamos Importe Pagado
							result = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "ImporteMonetario", "Incapacidad", "Incapacidades"), id_nomina_empleado, id_esquema_superior), id_esquema_superior, Esquema.ObtieneIdEsquema(version, "ImporteMonetario", "Incapacidad", "Incapacidades"), id_nomina_empleado, importe_pagado.ToString(), 0, 0, 0, id_usuario);
						}
					}
				}
				//Validamos resultado
				if (result.OperacionExitosa)
				{
					scope.Complete();
					result = new RetornoOperacion(id_esquema_superior);
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Actualizamos Totales de la Deducción
		/// </summary>
		/// <param name="version">versi+ón</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaTotalesDeducciones(string version, int id_nomina_empleado, int id_usuario)
		{
			//DFeclaramos objeto Resultado
			RetornoOperacion resultado = new RetornoOperacion();
			decimal total_impuestos_retenidos = 0.00M;
			decimal total_otras_deducciones = 0.00M;
			//Obtenemos Importe Exentos y Gravados
			ObtieneTotalesDeducciones(id_nomina_empleado, out total_impuestos_retenidos, out total_otras_deducciones);
			//Actualizamos Total Impuestos Retenidos 
			resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalImpuestosRetenidos", "Deducciones"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalImpuestosRetenidos", "Deducciones"), id_nomina_empleado, total_impuestos_retenidos.ToString(), 0, 0, 0, id_usuario);
			//Validamos RFesultado
			if (resultado.OperacionExitosa)
			{
				//Actualizamos Total  Gravado
				resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalOtrasDeducciones", "Deducciones"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalOtrasDeducciones", "Deducciones"), id_nomina_empleado, total_otras_deducciones.ToString(), 0, 0, 0, id_usuario);
			}
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Actualizamos Totales de la Percepción
		/// </summary>
		/// <param name="version">versi+ón</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaTotalesPercepciones(string version, int id_nomina_empleado, int id_usuario)
		{
			//DFeclaramos objeto Resultado
			RetornoOperacion resultado = new RetornoOperacion();
			decimal total_exento = 0.00M;
			decimal total_gravado = 0.00M;
			decimal total_sueldos = 0.00M;
			//Obtenemos Importe Exentos y Gravados
			ObtieneTotalesPercepciones(id_nomina_empleado, out total_exento, out total_gravado, out total_sueldos);
			//Actualizamos Total Exento 
			resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalExento", "Percepciones"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalExento", "Percepciones"), id_nomina_empleado, total_exento.ToString(), 0, 0, 0, id_usuario);
			//Validamos RFesultado
			if (resultado.OperacionExitosa)
			{
				//Actualizamos Total  Gravado
				resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalGravado", "Percepciones"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalGravado", "Percepciones"), id_nomina_empleado, total_gravado.ToString(), 0, 0, 0, id_usuario);
				//Validamos RFesultado
				if (resultado.OperacionExitosa)
				{
					//Actualizamos Total  Sueldos
					resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalSueldos", "Percepciones"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalSueldos", "Percepciones"), id_nomina_empleado, total_sueldos.ToString(), 0, 0, 0, id_usuario);
				}
			}
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Actualizamos Totales de la Deducción
		/// </summary>
		/// <param name="version">versi+ón</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion ActualizaTotalesNomina(string version, int id_nomina_empleado, int id_usuario)
		{
			//DFeclaramos objeto Resultado
			RetornoOperacion resultado = new RetornoOperacion();
			decimal total_percepciones = 0.00M;
			decimal total_deducciones = 0.00M;
			decimal total_otros_pagos = 0.00M;
			//Obtenemos Importe Exentos y Gravados
			ObtieneTotalesNomina(id_nomina_empleado, out total_percepciones, out total_deducciones, out total_otros_pagos);
			//Actualizamos Total Percepciones
			resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalPercepciones", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalPercepciones", "Nomina"), id_nomina_empleado, total_percepciones.ToString(), 0, 0, 0, id_usuario);
			//Validamos RFesultado
			if (resultado.OperacionExitosa)
			{
				//Actualizamos Total  Deducciones
				resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalDeducciones", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalDeducciones", "Nomina"), id_nomina_empleado, total_deducciones.ToString(), 0, 0, 0, id_usuario);
				//Validamos RFesultado
				if (resultado.OperacionExitosa)
				{
					//Actualizamos Total  Otros Pagos
					resultado = EsquemaRegistro.ActualizaEsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalOtrosPagos", "Nomina"), id_nomina_empleado, 0), 0, Esquema.ObtieneIdEsquema(version, "TotalOtrosPagos", "Nomina"), id_nomina_empleado, total_otros_pagos.ToString(), 0, 0, 0, id_usuario);
				}
			}
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Método encargado de Insertar un Esquema Registro
		/// </summary>
		/// <param name="id_esquema">Id Esquema</param>
		/// <param name="id_esquema_superior">Esquema Superior</param>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="valor">Valor</param>
		/// <param name="id_tabla_catalogo">Id Tabla Catalogo</param>
		/// <param name="id_tipo_catalogo">Id Tipo de Catalogo</param>
		/// <param name="id_valor">Id Valor de acuerdo ala Tabla Catalogo o Tipo</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaEsquemaRegistro(int id_esquema, int id_esquema_superior, int id_nomina_empleado, string valor, int id_tabla_catalogo, int id_tipo_catalogo, int id_valor, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Armando Arreglo de Parametros
			object[] param = { 1, 0, id_esquema, id_esquema_superior, id_nomina_empleado, valor, id_tabla_catalogo, id_tipo_catalogo, id_valor, id_usuario, true, "", "" };
			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de englobar la inserción de informacion sobre SeparacionIndemnizacion. Para su uso en la forma Nomina1233.aspx
		/// </summary>
		/// <param name="idNomina">Identificador de registro en session de la forma Nomina1233.aspx</param>
		/// <param name="idNominaEmpleado">Identificador del registro seleccionado del gridview principal.</param>
		/// <param name="idValor">Identificador obtenido del ddlSIConcepto de la ventana modal SeparacionIndemnizacion, correspondiente a [022]Prima por antiguedad, [023]Pagos por separacion o [025]Indemnizaciones</param>
		/// <param name="importeGravado">Cantidad obtenida desde el txtSIIngresoGravado de la ventana modal SeparacionIndemnizacion</param>
		/// <param name="idUsuario">Identificador del usuario actual en session</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaSeparacionIndemnizacion(int idNomina, int idNominaEmpleado,   int idValor, decimal importeGravado,  int idEmpleado, int idUsuario)
		{
			//Crear variables auxiliares que ayudan a obtener y conservar informacion
			RetornoOperacion resultado = new RetornoOperacion(); //Almacena el resultado de las inserciones
			int idEsquemaSuperior = 0; //
			decimal totalSeparacionIndemnizacion = ObtieneTotalSeparacionIndemnizacion(idNominaEmpleado);
			decimal ultimoSueldo = NomEmpleado.ObtieneSueldoNominaAnterior(idNominaEmpleado, idEmpleado);
			int anosServicio = 0;
			using (Operador operador = new Operador(idEmpleado)) anosServicio = Fecha.ObtieneAnosDiferenciaRedondeado(operador.fecha_ingreso, operador.fecha_baja);
			//Insertar nodo para el esquema [41]Percepcion, que será nuestro agrupador
			resultado = InsertaEsquemaRegistro(41, 0, idNominaEmpleado, "", 0, 0, 0, idUsuario);
			//Si se insertó el esquema [41]
			if (resultado.OperacionExitosa)
			{
				//Almacenar el id obtenido para usarlo como agrupador
				idEsquemaSuperior = resultado.IdRegistro;
				//Englobar las inserciones del Nodo Percepcion
				resultado = InsertaNodoPercepcion(idNominaEmpleado, idEsquemaSuperior, importeGravado, idValor, idUsuario);
				//Si la informacion para el Nodo Percepcion se inserta, con el esquema [50]ImporteExento como ultimo
				if (resultado.OperacionExitosa)
				{
					//Actualiza el TotalPagado
					totalSeparacionIndemnizacion = ObtieneTotalSeparacionIndemnizacion(idNominaEmpleado);
					/*Comienza la insercion de la informacion para el Nodo SeparacionIndemnizacion*/
					resultado = InsertaNodoSeparacion(idNominaEmpleado, idEsquemaSuperior, totalSeparacionIndemnizacion, ultimoSueldo, anosServicio, idUsuario);
					//
					if (resultado.OperacionExitosa)
					{
						using (Nomina12 nom = new Nomina12(idNomina))
						{
							resultado = ActualizaTotalesNomina(nom.version, idNominaEmpleado, idUsuario);
							if (resultado.OperacionExitosa)
								resultado = ActualizaTotalesPercepciones(nom.version, idNominaEmpleado, idUsuario);
						}
					}
				}
			}
			return resultado;
		}
		/// <summary>
		/// Insercion de atributos parael Nodo Percepcion
		/// </summary>
		/// <param name="idEsquemaSuperior"></param>
		/// <param name="idNominaEmpleado"></param>
		/// <param name="idValor"></param>
		/// <param name="idUsuario"></param>
		/// <returns></returns>
		private static RetornoOperacion InsertaNodoPercepcion(int idNominaEmpleado, int idEsquemaSuperior, decimal importeGravado, int idValor, int idUsuario)
		{
			RetornoOperacion resultado;
			//Instanciar el TipoCobroRecurrente para obtener la clave del SAT. Ejs: "022", "023", "025". Dado el elemento escogido del ddlSIConcepto, que puede devolver [82]PrimaAntiguedad, [83]Indemnizacion y []PagosPorSeparacion
			using (Liquidacion.TipoCobroRecurrente tipoCobroR = new Liquidacion.TipoCobroRecurrente(idValor))
			{
				//Validando que se instanció correctamente
				if (tipoCobroR.habilitar)
				{
					//Tabla[78]:TipoCobroRecurrente->IdValor:[82]PrimaAntigüedad/[83]Indemnizaciones/[PorRegistrarID]PagosPorIndemnizacion
					//Insertar el registro para el esquema [46]TipoPercepcion
					resultado = InsertaEsquemaRegistro(46, idEsquemaSuperior, idNominaEmpleado, tipoCobroR.clave_nomina, 78, 0, idValor, idUsuario);
					//Si se insertó el esquema [46]
					if (resultado.OperacionExitosa)
					{
						//Insertar el registro para el esquema [47]Clave
						resultado = InsertaEsquemaRegistro(47, idEsquemaSuperior, idNominaEmpleado, tipoCobroR.clave_nomina, 78, 0, idValor, idUsuario);
						//Si se insertó el esquema [47]
						if (resultado.OperacionExitosa)
						{
							//Insertar el registro para el esquema [48]Concepto
							resultado = InsertaEsquemaRegistro(48, idEsquemaSuperior, idNominaEmpleado, tipoCobroR.descripcion, 78, 0, idValor, idUsuario);
							//Si se insertó el esquema [48]
							if (resultado.OperacionExitosa)
							{
								//Insertar el registro para el esquema [49]ImporteGravado
								resultado = InsertaEsquemaRegistro(49, idEsquemaSuperior, idNominaEmpleado, importeGravado.ToString(), 0, 0, 0, idUsuario);
								//Si se insertó el esquema [49]
								if (resultado.OperacionExitosa)
								{
									//Insertar el registro para el esquema [50]ImporteExento. Al tratarse de informacion sobre SeparacionIndemnizacion, SIEMPRE ENVIAR CERO EN DECIMAL
									resultado = InsertaEsquemaRegistro(50, idEsquemaSuperior, idNominaEmpleado, "0.00", 0, 0, 0, idUsuario);
									/*Hasta aqui termina la inserción de la informacion para el Nodo Percepcion*/
								}
							}
						}
					}
				}
				//Si no se logra instanciar el TipoCobroRecurrente, impedir que siga la transaccion
				else
				{
					resultado = new RetornoOperacion("Imposible obtener el Cobro Recurrente");
				}
			}
			return resultado;
		}
		/// <summary>
		/// Insercion de atributos para el Nodo SeparacionIndemnizacion
		/// </summary>
		/// <param name="idNominaEmpleado"></param>
		/// <param name="idEsquemaSuperior"></param>
		/// <param name="idUsuario"></param>
		/// <param name="totalSeparacionIndemnizacion"></param>
		/// <param name="ultimoSueldo"></param>
		/// <returns></returns>
		private static RetornoOperacion InsertaNodoSeparacion(int idNominaEmpleado, int idEsquemaSuperior, decimal totalSeparacionIndemnizacion, decimal ultimoSueldo, int anosServicio, int idUsuario)
		{
			RetornoOperacion resultado = new RetornoOperacion("", true);
			/*Pueden existir hasta 3 nodos de Percepcion, relacionadas uno a cada clave de Separacion Indemnizacion [022], [023], [025]*/
			/*Por otro lado, el nodo SeparacionIndemnizacion debe englobar la informacion que puedan traer los nodos Percepcion. Es decir, no debe repetirse*/
			//Revisar si ya existe el registro para el esquema [61]SeparacionIndemnizacion. Si existe, edita; si no, inserta
			using (EsquemaRegistro esq61 = new EsquemaRegistro(ObtieneIdEsquemaRegistrado(idNominaEmpleado, 61)))
			{
				if (esq61.habilitar) { /*No se requiere actualizar el valor del esquema 61, solo tomar el agrupador de éste*/ idEsquemaSuperior = esq61.id_esquema_registro; }
				//Insertar el registro para el esquema [61]SeparacionIndemnizacion
				else { resultado = InsertaEsquemaRegistro(61, 0, idNominaEmpleado, "", 0, 0, 0, idUsuario); idEsquemaSuperior = resultado.IdRegistro; }
			}
			//Si se insertó el esquema [61]
			if (resultado.OperacionExitosa)
			{
				//Revisar si ya existe el registro para el esquema 62. Si existe, edita; si no, inserta
				using (EsquemaRegistro esq62 = new EsquemaRegistro(ObtieneIdEsquemaRegistrado(idNominaEmpleado, 62, "", idEsquemaSuperior)))
				{
					if (esq62.habilitar) esq62.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
					//Insertar el esquema [62]TotalPagado
					else resultado = InsertaEsquemaRegistro(62, idEsquemaSuperior, idNominaEmpleado, totalSeparacionIndemnizacion.ToString(), 0, 0, 0, idUsuario);
				}
				//Si se insertó el esquema [62]
				if (resultado.OperacionExitosa)
				{
					using (EsquemaRegistro esq63 = new EsquemaRegistro(ObtieneIdEsquemaRegistrado(idNominaEmpleado, 63, "", idEsquemaSuperior)))
					{
						if (esq63.habilitar) {/*No se requiere actualizar el esquema 63*/ }
						//Insertar el esquema [63]NumAñosServicio
						else resultado = InsertaEsquemaRegistro(63, idEsquemaSuperior, idNominaEmpleado, anosServicio.ToString(), 0, 0, 0, idUsuario);
					}
					//Si se insertó el esquema [63]
					if (resultado.OperacionExitosa)
					{
						using (EsquemaRegistro esq64 = new EsquemaRegistro(ObtieneIdEsquemaRegistrado(idNominaEmpleado, 64, "", idEsquemaSuperior)))
						{
							if (esq64.habilitar) { /*No se requiere editar el esquema 64*/ }
							//Insertar el esquema [64]UltimoSueldoMesord
							else resultado = InsertaEsquemaRegistro(64, idEsquemaSuperior, idNominaEmpleado, ultimoSueldo.ToString(), 0, 0, 0, idUsuario);
						}
						//Si se insertó el esquema [64]
						if (resultado.OperacionExitosa)
						{
							using (EsquemaRegistro esq65 = new EsquemaRegistro(ObtieneIdEsquemaRegistrado(idNominaEmpleado, 65, "", idEsquemaSuperior)))
							{
								if (esq65.habilitar) resultado = esq65.ActualizaValor((totalSeparacionIndemnizacion < ultimoSueldo ? totalSeparacionIndemnizacion : ultimoSueldo).ToString(), idUsuario);
								//Insertar el esquema [65]IngresoAcumulable
								else resultado = InsertaEsquemaRegistro(65, idEsquemaSuperior, idNominaEmpleado, (totalSeparacionIndemnizacion < ultimoSueldo ? totalSeparacionIndemnizacion : ultimoSueldo).ToString(), 0, 0, 0, idUsuario);
							}
							//Si se insertó el esquema [65]
							if (resultado.OperacionExitosa)
							{
								using (EsquemaRegistro esq66 = new EsquemaRegistro(ObtieneIdEsquemaRegistrado(idNominaEmpleado, 66, "", idEsquemaSuperior)))
								{
									if (esq66.habilitar) resultado = esq66.ActualizaValor((totalSeparacionIndemnizacion - ultimoSueldo).ToString(), idUsuario);
									//Insertar el esquema [66]IngresoNoAcumulable
									else resultado = InsertaEsquemaRegistro(66, idEsquemaSuperior, idNominaEmpleado, (totalSeparacionIndemnizacion - ultimoSueldo).ToString(), 0, 0, 0, idUsuario);
								}
								/*Hasta aquí termina la inserción de la información del Nodo SeparacionIndemnizacion*/
								//Si se insertó el esquema [66]
								if (resultado.OperacionExitosa)
								{
									//Buscar si el esquema 68 ya fue insertado, e instanciarlo
									using (EsquemaRegistro esq68 = new EsquemaRegistro(ObtieneIdEsquemaRegistrado(idNominaEmpleado, 68)))
									{
										//Si ya existe y se obtiene el registro, actualizar solo su atributo Valor.
										if (esq68.habilitar)
											resultado = esq68.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
										//Si no existe, insertarlo con el valor actualizado
										else
											resultado = InsertaEsquemaRegistro(68, 0, idNominaEmpleado, totalSeparacionIndemnizacion.ToString(), 0, 0, 0, idUsuario);
									}
								}
							}
						}
					}
				}
			}
			return resultado;
		}
		/// <summary>
		/// Obtiene el Total de Encabezado Deducciones de Incapacidades
		/// </summary>
		/// <param name="version">Versión de Nómina</param>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion QuitaTotalImpuestoRetenido(string version, int id_nomina_empleado, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion resultado = new RetornoOperacion(0);
			//Validamos Totales 
			if (ObtieneImpuestoRetenido(id_nomina_empleado) == 0)
			{
				int id_registro = ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalImpuestosRetenidos", "Deducciones"), id_nomina_empleado, 0);
				//Obtenemos Id de Impuestos Retenidos
				if (id_registro > 0)
				{
					//Intsnacimos Registro para su eliminación
					using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(id_registro))
					{
						//Deshabilitamos
						resultado = objEsquemaRegistro.DeshabilitaEsquemaRegistro(id_usuario);
					}
				}
			}
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Obtiene el Total de Encabezado Deducciones de Incapacidades
		/// </summary>
		/// <param name="version">Versión de Nómina</param>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <param name="id_usuario">Id Usuario</param>
		/// <returns></returns>
		public static RetornoOperacion QuitaTotalDeducciones(string version, int id_nomina_empleado, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion resultado = new RetornoOperacion(0);
			//Creamos la transacción 
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Validamos Totales 
				if (!ValidaDeducciones(id_nomina_empleado))
				{
					//Eliminamos Total OtrasDeducciones
					int id_registro = ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalOtrasDeducciones", "Deducciones"), id_nomina_empleado, 0);
					//Obtenemos Id de Impuestos Retenidos
					if (id_registro > 0)
					{
						//Intsnacimos Registro para su eliminación
						using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(id_registro))
						{
							//Deshabilitamos
							resultado = objEsquemaRegistro.DeshabilitaEsquemaRegistro(id_usuario);
						}
					}
					int id_registro_encabezado = ObtieneIdEsquemaRegistro(Esquema.ObtieneIdEsquema(version, "TotalDeducciones", "Nomina"), id_nomina_empleado, 0);
					//Validamos Existencia de Registro
					if (id_registro_encabezado > 0 && ObtieneImpuestoRetenido(id_nomina_empleado) == 0)
					{
						//Validamos resultado
						if (resultado.OperacionExitosa)
						{
							//Intsnacimos Registro para su eliminación
							using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(id_registro_encabezado))
							{
								//Deshabilitamos
								resultado = objEsquemaRegistro.DeshabilitaEsquemaRegistro(id_usuario);
							}
						}
					}
				}
				//Validamos Resultado
				if (resultado.OperacionExitosa)
				{
					scope.Complete();
				}
			}
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Obtiene el Total de Encabezado Percepciones de Horas Extras
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static RetornoOperacion ValidaTotalesHorasExtras(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion resultado = new RetornoOperacion(0);
			//Validamos Totales 
			if (ObtieneTotalEncabezadoHorasExtras(id_nomina_empleado) != ObtieneTotalDetallesHorasExtras(id_nomina_empleado))
			{
				//En caso de no ser iguales mostramos mensaje erro
				resultado = new RetornoOperacion("Los detalles de las Horas Extras no coinciden con el Encabezado. ");
			}
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Obtiene el Total de Encabezado Deducciones de Incapacidades
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static RetornoOperacion ValidaTotalesIncapacidades(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion resultado = new RetornoOperacion(0);
			//Validamos Totales 
			if (ObtieneTotalEncabezadoIncapacidades(id_nomina_empleado) != ObtieneTotalDetallesIncapacidades(id_nomina_empleado))
			{
				//En caso de no ser iguales mostramos mensaje erro
				resultado = new RetornoOperacion("Los detalles de las Incapacidades no coinciden con el Encabezado. ");
			}
			//Devolvemos Resultado
			return resultado;
		}
		#endregion

		#region BOOL
		/// <summary>
		/// Método encargado de Actualizar los Valores
		/// </summary>
		/// <returns></returns>
		public bool ActualizaEsquemaRegistro()
		{
			//Invocando Método de Carga
			return this.cargaAtributosInstancia(this._id_esquema);
		}
		/// <summary>
		/// Validación de Existencia de Deducciones
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static bool ValidaDeducciones(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			bool validacion = false;
			//Armando Arreglo de Parametros
			object[] param = { 25, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Asignamos Validación
						validacion = true;
					}
			}
			return validacion;
		}
		/// <summary>
		/// Método que busca si existen Percepciones relacionadas a Separacion Indeminzacion, con Clave [022], [023], [025]
		/// </summary>
		/// <param name="idNominaEmpleado"></param>
		/// <param name="claveConceptoSAT"></param>
		/// <param name="tipoAplicacion"></param>
		/// <returns></returns>
		public static bool ValidaExistenciasSeparacionIndmnizacion(int idNominaEmpleado, string claveConceptoSAT, int tipoAplicacion)
		{
			bool existencias = false;
			existencias = Validacion.ValidaOrigenDatos(ObtieneDetallesSeparacionIndemnizacion(idNominaEmpleado, claveConceptoSAT, tipoAplicacion));
			return existencias;
		}
		#endregion

		#region DECIMAL
		/// <summary>
		/// Método que obtiene la suma de los totalesPagados para las claves [022], [023] y [025]
		/// </summary>
		/// <param name="idNominaEmpleado"></param>
		/// <returns></returns>
		public static decimal ObtieneTotalSeparacionIndemnizacion(int idNominaEmpleado)
		{
			decimal totalSeparacionIndemnizacion = 0;
			//Armando Arreglo de Parametros
			object[] param = { 29, 0, 0, 0, idNominaEmpleado, "", 0, 0, 0, 0, false, "", "" };
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
						totalSeparacionIndemnizacion = Convert.ToDecimal(dr["TotalSeparacionIndemnizacion"]);
					}
				}
			}
			return totalSeparacionIndemnizacion;
		}
		/// <summary>
		/// Obtiene el Total de  Detalles de las Incapacidades
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static decimal ObtieneTotalDetallesIncapacidades(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			decimal total = 0;
			//Armando Arreglo de Parametros
			object[] param = { 22, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						total = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["Total"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			return total;
		}
		/// <summary>
		/// Obtiene el Total de Encabezado Deducciones de Incapacidades
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static decimal ObtieneTotalEncabezadoIncapacidades(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			decimal total = 0;
			//Armando Arreglo de Parametros
			object[] param = { 23, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						total = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["Total"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			return total;
		}
		/// <summary>
		/// Obtiene La Deducción de Tipo 002 ISR
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static decimal ObtieneImpuestoRetenido(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			int id_registro = 0;
			//Armando Arreglo de Parametros
			object[] param = { 24, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						id_registro = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			return id_registro;
		}
		/// <summary>
		/// Obtiene el Total de  Detalles de las Horas Extras
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static decimal ObtieneTotalDetallesHorasExtras(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			decimal total = 0;
			//Armando Arreglo de Parametros
			object[] param = { 20, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						total = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["Total"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			return total;
		}
		/// <summary>
		/// Obtiene el Total de Encabezado Percepciones de Horas Extras
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <returns></returns>
		public static decimal ObtieneTotalEncabezadoHorasExtras(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			decimal total = 0;
			//Armando Arreglo de Parametros
			object[] param = { 21, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						total = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Total"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			return total;
		}
		#endregion

		#region INT
		/// <summary>
		/// Método encargado de buscar un Esquema para una determinada Nomina de Empleado, y devolver el id primario si lo encuentra.
		/// </summary>
		/// <param name="idNominaEmpleado"></param>
		/// <param name="idEsquema"></param>
		/// <returns></returns>
		public static int ObtieneIdEsquemaRegistrado(int idNominaEmpleado, int idEsquema, string valor = "", int idEsquemaSuperior = 0)
		{
			int idEsquemaRegistro = 0;
			//Armando Arreglo de Parametros
			object[] param = { 30, 0, idEsquema, idEsquemaSuperior, idNominaEmpleado, valor, 0, 0, 0, 0, false, "", "" };
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
						idEsquemaRegistro = Convert.ToInt32(dr["IdEsquemaRegistro"]);
					}
				}
			}
			return idEsquemaRegistro;
		}
		/// <summary>
		/// Obtiene el Id de Esquema Registro
		/// </summary>
		/// <param name="id_esquema">Id Esquema</param>
		/// <param name="id_nomina_empleado">Id Nomina de Empleado</param>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <returns></returns>
		public static int ObtieneIdEsquemaRegistro(int id_esquema, int id_nomina_empleado, int id_esquema_superior)
		{
			//Declarando Objeto de Retorno
			int id_esquema_registro = 0;
			//Armando Arreglo de Parametros
			object[] param = { 4, 0, id_esquema, id_esquema_superior, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						id_esquema_registro = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			//Devolviendo Resultado Obtenido
			return id_esquema_registro;
		}
		/// <summary>
		/// Obtiene el Id de Esquema Registro
		/// </summary>
		/// <param name="id_esquema">Id Esquema</param>
		/// <param name="id_nomina_empleado">Id Nomina de Empleado</param>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <param name="valor">Valor Coincidente</param>
		/// <returns></returns>
		public static int ObtieneIdEsquemaRegistro(int id_esquema, int id_nomina_empleado, string valor)
		{
			//Declarando Objeto de Retorno
			int id_esquema_registro = 0;
			//Armando Arreglo de Parametros
			object[] param = { 18, 0, id_esquema, 0, id_nomina_empleado, valor, 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						id_esquema_registro = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
			return id_esquema_registro;
		}
		#endregion

		#region STRING
		/// <summary>
		/// Obtiene el Id de Esquema Registro
		/// </summary>
		/// <param name="id_esquema">Id Esquema</param>
		/// <param name="id_nomina_empleado">Id Nomina de Empleado</param>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <returns></returns>
		public static string ObtieneValorEsquemaRegistro(int id_esquema, int id_nomina_empleado, int id_esquema_superior)
		{
			//Declarando Objeto de Retorno
			string valor = "";
			//Armando Arreglo de Parametros
			object[] param = { 17, 0, id_esquema, id_esquema_superior, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						valor = (from DataRow r in ds.Tables[0].Rows select r["Valor"].ToString()).DefaultIfEmpty().FirstOrDefault();
					}
			}
			//Devolviendo Resultado Obtenido
			return valor;
		}
		#endregion

		#region VOID
		/// <summary>
		/// Totales de la Deducción
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="total_impuestos_retenidos">Total Impuestos Retenidos</param>
		/// <param name="total_otras_deducciones">Total Otras Deducciones</param>
		public static void ObtieneTotalesDeducciones(int id_nomina_empleado, out decimal total_impuestos_retenidos, out decimal total_otras_deducciones)
		{
			//Declarando Objeto de Retorno
			total_impuestos_retenidos = 0.00M;
			total_otras_deducciones = 0.00M;
			//Armando Arreglo de Parametros
			object[] param = { 6, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						total_impuestos_retenidos = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalImpuestosRetenidos"])).DefaultIfEmpty().FirstOrDefault();
						//Obtenemos Validacion
						total_otras_deducciones = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalOtrasDeducciones"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
		}
		/// <summary>
		/// Obttiene Totales de la nómina
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="total_percepciones">Total Percepciones</param>
		/// <param name="total_deducciones">Total Deducciones</param>
		/// <param name="total_otro_pagos">Total Otros Pagos</param>
		public static void ObtieneTotalesNomina(int id_nomina_empleado, out decimal total_percepciones, out decimal total_deducciones, out decimal total_otro_pagos)
		{
			//Declarando Objeto de Retorno
			total_percepciones = 0.00M;
			total_deducciones = 0.00M;
			total_otro_pagos = 0.00M;
			//Armando Arreglo de Parametros
			object[] param = { 7, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						total_percepciones = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalPercepciones"])).DefaultIfEmpty().FirstOrDefault();
						//Obtenemos Validacion
						total_deducciones = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalDeducciones"])).DefaultIfEmpty().FirstOrDefault();
						//Obtenemos Validacion
						total_otro_pagos = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalOtrosPagos"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
		}
		/// <summary>
		/// Obtiene los Totales de las Percepciones
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nomina Empleado</param>
		/// <param name="total_exento"> Total Exento</param>
		/// <param name="total_gravado">Total Gravado</param>
		/// <param name="total_sueldos">Total Sueldos</param>
		/// <returns></returns>
		public static void ObtieneTotalesPercepciones(int id_nomina_empleado, out decimal total_exento, out decimal total_gravado, out decimal total_sueldos)
		{
			//Declarando Objeto de Retorno
			total_exento = 0.00M;
			total_gravado = 0.00M;
			total_sueldos = 0.00M;
			//Armando Arreglo de Parametros
			object[] param = { 5, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Validamos Origen de Datos
					if (Validacion.ValidaOrigenDatos(ds, "Table"))
					{
						//Obtenemos Validacion
						total_exento = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalExento"])).DefaultIfEmpty().FirstOrDefault();
						//Obtenemos Validacion
						total_gravado = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalGravado"])).DefaultIfEmpty().FirstOrDefault();
						//Obtenemos Validacion
						total_sueldos = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["TotalSueldos"])).DefaultIfEmpty().FirstOrDefault();
					}
			}
		}		
		/// <summary>
		/// Obttiene los Valores de las Horas Extras
		/// </summary>
		/// <param name="id_nomina_empleado"></param>
		/// <param name="id_agrupador"></param>
		/// <param name="dias"></param>
		/// <param name="id_tipo_horas"></param>
		/// <param name="horas_extras"></param>
		/// <param name="importe_pagado"></param>
		public static void ObtieneDetalleHorasExtras(int id_nomina_empleado, int id_agrupador, out int dias, out byte id_tipo_horas, out int horas_extras, out decimal importe_pagado)
		{
			//Declarando Objeto de Retorno
			dias = 0;
			id_tipo_horas = 0;
			horas_extras = 0;
			importe_pagado = 00M;
			//Armando Arreglo de Parametros
			object[] param = { 11, 0, 0, id_agrupador, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Obtenemos Dias
					dias = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Dias"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Tipo Horas
					id_tipo_horas = (from DataRow r in ds.Tables[0].Rows select Convert.ToByte(r["TipoHoras"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Horas Extras
					horas_extras = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["HorasExtras"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Importe
					importe_pagado = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["ImportePagado"])).DefaultIfEmpty().FirstOrDefault();
				}
			}
		}
		/// <summary>
		/// Obtiene Valores de Incapacidad
		/// </summary>
		/// <param name="id_nomina_empleado"></param>
		/// <param name="id_agrupador"></param>
		/// <param name="dias"></param>
		/// <param name="id_tipo"></param>
		/// <param name="importe_pagado"></param>
		public static void ObtieneDetalleIncapacidades(int id_nomina_empleado, int id_agrupador, out int dias, out byte id_tipo, out decimal importe_pagado)
		{
			//Declarando Objeto de Retorno
			dias = 0;
			id_tipo = 0;
			importe_pagado = 00M;
			//Armando Arreglo de Parametros
			object[] param = { 12, 0, 0, id_agrupador, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Obtenemos Dias
					dias = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["Dias"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Tipo Horas
					id_tipo = (from DataRow r in ds.Tables[0].Rows select Convert.ToByte(r["Tipo"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Importe
					importe_pagado = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["ImportePagado"])).DefaultIfEmpty().FirstOrDefault();
				}
			}
		}
		/// <summary>
		/// Obtenemos los Detalles de la Nómina
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_agrupador">Id Agrupadot</param>
		///<param name="id_concepto">Id Concepto</param>
		///<param name="importe">Importe </param>
		/// <returns></returns>
		public static void ObtieneDetalleNominaDeduccion(int id_nomina_empleado, int id_agrupador, out int id_concepto, out decimal importe)
		{
			//Declarando Objeto de Retorno
			id_concepto = 0;
			importe = 00M;
			//Armando Arreglo de Parametros
			object[] param = { 10, 0, 0, id_agrupador, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Obtenemos Id Concepto
					id_concepto = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["IdConcepto"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Importe Gravado
					importe = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["Importe"])).DefaultIfEmpty().FirstOrDefault();
				}
			}
		}
		/// <summary>
		/// Método encargado de Obtener  los Valores de Nóminas Otros
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_agrupador">Id Agrupador</param>
		/// <param name="id_concepto">Id Concepto</param>
		/// <param name="importe">Importe</param>
		/// <param name="importe_subsidio_causado">Importe del Subsidio Causado</param>
		/// <param name="valor_subsidio_causado">Valor del Subsidio Causado</param>
		public static void ObtieneDetalleNominaOtros(int id_nomina_empleado, int id_agrupador, out int id_concepto, out decimal importe, out decimal importe_subsidio_causado, out bool valor_subsidio_causado)
		{
			//Declarando Objeto de Retorno
			id_concepto = 0;
			importe = 00M;
			importe_subsidio_causado = 00M;
			valor_subsidio_causado = false;
			//Armando Arreglo de Parametros
			object[] param = { 26, 0, 0, id_agrupador, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Obtenemos Id Concepto
					id_concepto = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["IdConcepto"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Importe 
					importe = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["Importe"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Importe del Subsidio Causado
					importe_subsidio_causado = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["ImporteSubsidioCausado"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Valor Subsidio Causado
					valor_subsidio_causado = (from DataRow r in ds.Tables[0].Rows select Convert.ToBoolean(r["ValorSubsidioCausado"])).DefaultIfEmpty().FirstOrDefault();
				}
			}
		}
		/// <summary>
		/// Obtenemos los Detalles de la Nómina
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_agrupador">Id Agrupadot</param>
		///<param name="id_concepto">Id Concepto</param>
		///<param name="importe_exento">Importe Exento</param>
		///<param name="importe_gravado">Importe Gravado</param>
		/// <returns></returns>
		public static void ObtieneDetalleNominaPercepcion(int id_nomina_empleado, int id_agrupador, out int id_concepto, out decimal importe_gravado, out decimal importe_exento)
		{
			//Declarando Objeto de Retorno
			id_concepto = 0;
			importe_gravado = 00M;
			importe_exento = 00M;
			//Armando Arreglo de Parametros
			object[] param = { 9, 0, 0, id_agrupador, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Obtenemos Id Concepto
					id_concepto = (from DataRow r in ds.Tables[0].Rows select Convert.ToInt32(r["IdConcepto"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos Importe Gravado
					importe_gravado = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["ImporteGravado"])).DefaultIfEmpty().FirstOrDefault();
					//Obtenemos ImporteExento
					importe_exento = (from DataRow r in ds.Tables[0].Rows select Convert.ToDecimal(r["ImporteExento"])).DefaultIfEmpty().FirstOrDefault();
				}
			}
		}
		#endregion

		#region DATATABLE
		/// <summary>
		/// Carga Horas Extras o Incapaciodad de acuerdo al tipo
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_tipo_otros">Id Tipo Otros(Horas Extras, Incapacidad)</param>
		public static DataTable CargaHorasExtraIncapacidad(int id_nomina_empleado, int id_tipo_otros)
		{
			//Declarando Objeto de Retorno
			DataTable dt = null;
			//Armando Arreglo de Parametros
			object[] param = { 13, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, id_tipo_otros, "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dt = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dt;
		}
		/// <summary>
		/// Obtiene los Encabezados para la copia de Detalles
		/// </summary>
		/// <param name="id_nomina_empleado"></param>
		/// <returns></returns>
		public static DataTable ObtieneEncabezados(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			DataTable dt = null;
			//Armando Arreglo de Parametros
			object[] param = { 16, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dt = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dt;
		}
		/// <summary>
		/// Obtienes todos los registros
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina de Empleado</param>
		/// <returns></returns>
		public static DataTable ObtieneDetalles(int id_nomina_empleado)
		{
			//Declarando Objeto de Retorno
			DataTable dt = null;
			//Armando Arreglo de Parametros
			object[] param = { 14, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dt = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dt;
		}
		/// <summary>
		/// Obtienes los registros ligados a un Registro Superior
		/// </summary>
		/// <param name="id_registro_superior"></param>
		/// <returns></returns>
		public static DataTable ObtieneDetallesEsquemaSuperiorAtributos(int id_registro_superior)
		{
			//Declarando Objeto de Retorno
			DataTable dt = null;
			//Armando Arreglo de Parametros
			object[] param = { 15, 0, 0, id_registro_superior, 0, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dt = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dt;
		}
		/// <summary>
		/// Obtienes los  elementos ligados a un Registro Superior
		/// </summary>
		/// <param name="id_registro_superior"></param>
		/// <returns></returns>
		public static DataTable ObtieneDetallesEsquemaSuperiorElementos(int id_registro_superior)
		{
			//Declarando Objeto de Retorno
			DataTable dt = null;
			//Armando Arreglo de Parametros
			object[] param = { 27, 0, 0, id_registro_superior, 0, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dt = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dt;
		}
		/// <summary>
		/// Obtiene los encabezados de las Nóminas Extras
		/// </summary>
		/// <param name="id_nomina_empleado">Id Nómina Empleado</param>
		/// <param name="id_esquema_superior">Id Esquema Superior</param>
		/// <returns></returns>
		public static DataTable ObtieneDetalleHorasExtras(int id_nomina_empleado, int id_esquema_superior)
		{
			//Declarando Objeto de Retorno
			DataTable dt = null;
			//Armando Arreglo de Parametros
			object[] param = { 19, 0, 0, id_esquema_superior, id_nomina_empleado, "", 0, 0, 0, 0, false, "", "" };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dt = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dt;
		}
		/// <summary>
		/// Método encargado de Obtener los Detalles de Nomina del Empleado
		/// </summary>
		/// <param name="id_nomina_empleado">Nomina del Empleado</param>
		///<param name="clave_concepto_SAT">Clave de la Nómina</param>
		/// <param name="tipo_aplicacion">Tipo de Aplicación (Percepción, Deducción, Bonificación)</param>
		/// <returns></returns>
		public static DataTable ObtieneDetalleNominaEmpleado(int id_nomina_empleado, string clave_concepto_SAT, int id_tipo_aplicacion)
		{
			//Declarando Objeto de Retorno
			DataTable dtDetallesNomina = null;
			//Armando Arreglo de Parametros
			object[] param = { 8, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, id_tipo_aplicacion, clave_concepto_SAT };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dtDetallesNomina = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dtDetallesNomina;
		}
		/// <summary>
		/// Método encargado de Obtener los Detalles de Nomina del Empleado SOLO DE SEPARACION E INDEMNIZACION
		/// </summary>
		/// <param name="id_nomina_empleado">Nomina del Empleado</param>
		///<param name="clave_concepto_SAT">Clave de la Nómina</param>
		/// <param name="tipo_aplicacion">Tipo de Aplicación (Percepción, Deducción, Bonificación)</param>
		/// <returns></returns>
		public static DataTable ObtieneDetallesSeparacionIndemnizacion(int id_nomina_empleado, string clave_concepto_SAT, int id_tipo_aplicacion)
		{
			//Declarando Objeto de Retorno
			DataTable dtDetallesNominaSI = null;
			//Armando Arreglo de Parametros
			object[] param = { 28, 0, 0, 0, id_nomina_empleado, "", 0, 0, 0, 0, false, id_tipo_aplicacion, clave_concepto_SAT };
			//Instanciando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dtDetallesNominaSI = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dtDetallesNominaSI;
		}
		#endregion
		#endregion
	}
}