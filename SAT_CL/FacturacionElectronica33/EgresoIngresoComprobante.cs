using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SAT_CL.FacturacionElectronica33 {
	public class EgresoIngresoComprobante : Disposable {

		#region Enumeraciones
		/// <summary>
		/// Define los tipos de operaciones válidos para esta entidad
		/// </summary>
		public enum TipoOperacion {
			/// <summary>
			/// CFDI Emitido a un Cliente por concepto de Comprobante de Recepción de Pagos
			/// </summary>
			ComprobanteCliente = 1,
			/// <summary>
			/// CFDI Emitido por un Proveedor por concepto de un Pago Realizado
			/// </summary>
			ComprobanteProveedor = 2
		}
		/// <summary>
		/// Define los estatus posibles de esta entidad
		/// </summary>
		public enum Estatus {
			/// <summary>
			/// Relación activa (de un CFDI sin timbrar) y un Egreso o Ingreso
			/// </summary>
			Registrado = 1,
			/// <summary>
			/// Relación con un CFDI Timbrado
			/// </summary>
			Timbrado,
			/// <summary>
			/// Relación con un CFDI que será cancelado pero aún se encuentra activo
			/// </summary>
			PorCancelar,
			/// <summary>
			/// Relación con un CFDI ya cancelado
			/// </summary>
			Cancelado
		}
		#endregion

		#region Atributos
		/// <summary>
		/// Atributo encargado de Almacenar el Nombre del SP de Egreso Ingreso Comprobante
		/// </summary>
		private static string _nom_sp = "fe33.sp_egreso_ingreso_comprobante_teic";

		private int _id_egreso_ingreso_comprobante;
		/// <summary>
		/// Atributo que almacena el Identificador de egreso ingreso comprobante
		/// </summary>
		public int id_egreso_ingreso_comprobante { get { return this._id_egreso_ingreso_comprobante; } }

		private int _id_egreso_ingreso;
		/// <summary>
		/// Atributo que almacena el Identificador de egreso ingreso 
		/// </summary>
		public int id_egreso_ingreso { get { return this._id_egreso_ingreso; } }

		private byte _id_tipo_operacion;
		/// <summary>
		/// Atributo que almacena el Identificador de tipo de operacion
		/// </summary>
		public byte id_tipo_operacion { get { return this._id_tipo_operacion; } }
		/// <summary>
		/// Obtiene el tipo de operación asignada (CLiente/Proveedor)
		/// </summary>
		public TipoOperacion tipo_operacion { get { return (TipoOperacion)this._id_tipo_operacion; } }
		private int _id_comprobante_pago;
		/// <summary>
		/// Atributo que almacena el Identificador de comprobante de operacion
		/// </summary>
		public int id_comprobante_pago { get { return this._id_comprobante_pago; } }

		private byte _id_estatus;
		/// <summary>
		/// Atributo que almacena el Identificador de estatus(registrado,timbrado,por cancelar, cancelado) 
		/// </summary>
		public byte id_estatus { get { return this._id_estatus; } }

		private int _id_egreso_ingreso_comprobante_reemplazado;
		/// <summary>
		/// Atributo que almacena el Identificador de comprobante pago reemplazado
		/// </summary>
		public int id_egreso_ingreso_comprobante_reemplazado { get { return this._id_egreso_ingreso_comprobante_reemplazado; } }

		private bool _habilitar;
		/// <summary>
		/// Atributo que almacena el Identificador de comprobante pago reemplazado
		/// </summary>
		public bool habilitar { get { return this._habilitar; } }
		#endregion

		#region Constructores
		/// <summary>
		/// Constructor que inicializa los Atributos por Defecto
		/// </summary>
		public EgresoIngresoComprobante()
		{
			//Asignando Valores
			this._id_egreso_ingreso_comprobante = 0;
			this._id_egreso_ingreso = 0;
			this._id_tipo_operacion = 0;
			this._id_comprobante_pago = 0;
			this._id_estatus = 0;
			this._id_egreso_ingreso_comprobante_reemplazado = 0;
			this._habilitar = false;
		}

		/// <summary>
		/// Constructor que inicializa los Atributos dado un Registro
		/// </summary>
		/// <param name="id_egreso_ingreso_comprobante">parametro dado de Egreso Ingreso Comprobante</param>
		public EgresoIngresoComprobante(int id_egreso_ingreso_comprobante)
		{
			//Invocando Método de Carga
			cargaAtributosInstancia(id_egreso_ingreso_comprobante);
		}
		#endregion

		#region Destructores
		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~EgresoIngresoComprobante()
		{
			Dispose(false);
		}
		#endregion

		#region Métodos Privados
		/// <summary>
		/// Método encargado de Cargar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_egreso_ingreso_comprobante">Parametro dado de Egreso Ingreso Comprobante</param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_egreso_ingreso_comprobante)
		{
			//Declarando Objeto de Retorno
			bool result = false;
			//Armando Arreglo de Parametros
			object[] param = { 3, id_egreso_ingreso_comprobante, 0, 0, 0, 0, 0, 0, false, "", "" };
			//Obteniendo Resultado del SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorriendo Registro
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Asignando Valores
						this._id_egreso_ingreso_comprobante = id_egreso_ingreso_comprobante;
						this._id_egreso_ingreso = Convert.ToInt32(dr["IdEgresoIngreso"]);
						this._id_tipo_operacion = Convert.ToByte(dr["IdTipoOperacion"]);
						this._id_comprobante_pago = Convert.ToInt32(dr["IdComprobantePago"]);
						this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
						this._id_egreso_ingreso_comprobante_reemplazado = Convert.ToInt32(dr["IdEgresoIngresoComprobanteReemplazado"]);
						this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
						//Terminando Ciclo
						break;
					}
				}
			}
			//Devolviendo Resultado Obtenido
			return result;
		}

		/// <summary>
		/// Método encargado de Actualizar los Registros de la BD
		/// </summary>
		/// <param name="id_egreso_ingreso">Permite Actualizar el identificador del registro de egreso ingreso</param>
		/// <param name="id_tipo_operacion">Permite Actualizar el identificador del registro tipo operacion</param>
		/// <param name="id_comprobante_pago">Permite Actualizar el identificador del registro de comprobante pago</param>
		/// <param name="id_estatus">Permite Actualizar el identificador del registro de estatus</param>
		/// <param name="id_egreso_ingreso_comprobante_reemplazado">Permite Actualizar el identificador del registro de comprobante pago reemplazado</param>
		/// <param name="id_usuario">Permite Actualizar el identificador del registro de usuario</param>
		/// <param name="habilitar">Permite Actualizar si el registro se encuentra activo</param>
		/// <returns></returns>
		private RetornoOperacion actualizaRegistrosBD(int id_egreso_ingreso, byte id_tipo_operacion, int id_comprobante_pago, byte id_estatus, int id_egreso_ingreso_comprobante_reemplazado, int id_usuario, bool habilitar)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Armando Arreglo de Parametros
			object[] param = { 2, this._id_egreso_ingreso_comprobante, id_egreso_ingreso, id_tipo_operacion, id_comprobante_pago, id_estatus, id_egreso_ingreso_comprobante_reemplazado, id_usuario, habilitar, "", "" };
			//Obteniendo Resultado del SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
			//Devolviendo Resultado Obtenido
			return result;
		}
		#endregion

		#region Métodos Públicos
		/// <summary>
		/// Método encargado de Insertar los registros de EgresoIngresoComprobante
		/// </summary>
		/// <param name="id_egreso_ingreso">Permite Insertar el identificador del registro de egreso ingreso</param>
		/// <param name="id_tipo_operacion">Permite Insertar el identificador del registro tipo operacion</param>
		/// <param name="id_comprobante_pago">Permite Insertar el identificador del registro de comprobante pago</param>
		/// <param name="id_estatus">Permite Insertar el identificador del registro de estatus</param>
		/// <param name="id_egreso_ingreso_comprobante_reemplazado">Permite Insertar el identificador del registro de comprobante pago reemplazado</param>
		/// <param name="id_usuario">Permite Insertar el identificador del registro de usuario</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaEgresoIngresoComprobante(int id_egreso_ingreso, byte id_tipo_operacion, int id_comprobante_pago, byte id_estatus, int id_egreso_ingreso_comprobante_reemplazado, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Armando Arreglo de Parametros
			object[] param = { 1, 0, id_egreso_ingreso, id_tipo_operacion, id_comprobante_pago, id_estatus, id_egreso_ingreso_comprobante_reemplazado, id_usuario, true, "", "" };
			//Obteniendo Resultado del SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
			//Devolviendo Resultado Obtenido
			return result;
		}

		/// <summary>
		/// Método encargado de Editar los registros de IngresoEgresoComprobante
		/// </summary>
		/// <param name="id_egreso_ingreso">Permite Editar el identificador del registro de egreso ingreso</param>
		/// <param name="id_tipo_operacion">Permite Editar el identificador del registro tipo operacion</param>
		/// <param name="id_comprobante_pago">Permite Editar el identificador del registro de comprobante pago</param>
		/// <param name="id_estatus">Permite Editar el identificador del registro de estatus</param>
		/// <param name="id_egreso_ingreso_comprobante_reemplazado">Permite Editar el identificador del registro de comprobante pago reemplazado</param>
		/// <param name="id_usuario">Permite Editar el identificador del registro de usuario</param>
		/// <returns></returns>
		public RetornoOperacion EditaEgresoIngresoComprobante(int id_egreso_ingreso, byte id_tipo_operacion, int id_comprobante_pago, byte id_estatus, int id_egreso_ingreso_comprobante_reemplazado, int id_usuario)
		{
			//Devolviendo Resultado Obtenido
			return this.actualizaRegistrosBD(id_egreso_ingreso, id_tipo_operacion, id_comprobante_pago, id_estatus, id_egreso_ingreso_comprobante_reemplazado, id_usuario, this._habilitar);
		}

		/// <summary>
		/// Realiza el cambio de estatus de la relación Pago - CFDI
		/// </summary>
		/// <param name="estatus">Nuevo estatus a colocar</param>
		/// <param name="id_usuario">Id de Usuario que realiza la operación</param>
		/// <returns></returns>
		public RetornoOperacion ActualizaEstatusEgresoIngresoComprobante(Estatus estatus, int id_usuario)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion();

			//SI el cambio de estatus es válido
			if (((Estatus)this.id_estatus == Estatus.Registrado && estatus == Estatus.Timbrado) ||
				 ((Estatus)this.id_estatus == Estatus.Timbrado && estatus == Estatus.PorCancelar) ||
				 ((Estatus)this.id_estatus == Estatus.PorCancelar && estatus == Estatus.Cancelado))
			{
				//Devolviendo Resultado Obtenido
				return this.actualizaRegistrosBD(this._id_egreso_ingreso, this._id_tipo_operacion, this._id_comprobante_pago, (byte)estatus, this._id_egreso_ingreso_comprobante_reemplazado, id_usuario, this._habilitar);
			}
			else
				resultado = new RetornoOperacion(string.Format("Las relaciones de Pago y CFDI de Pago no pueden pasar de '{0}' a '{1}'.", ((Estatus)this.id_estatus), estatus));
			//Devolviendo resultado
			return resultado;
		}

		/// <summary>
		/// Método encargado de Deshabilitar registros de Egreso Ingreso Comprobante
		/// </summary>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaEgresoIngresoComprobante(int id_usuario)
		{
			//Devolviendo Resultado Obtenido
			return this.actualizaRegistrosBD(this._id_egreso_ingreso, this._id_tipo_operacion, this._id_comprobante_pago, this._id_estatus, this._id_egreso_ingreso_comprobante_reemplazado, id_usuario, false);
		}
		
		/// <summary>
		/// Método encargado de Actualizar el registro de EgresoIngreso Comprobante
		/// </summary>
		/// <returns></returns>
		public bool ActualizaEgresoIngresoComprobante()
		{
			//Invocando Método de Carga
			return this.cargaAtributosInstancia(this._id_egreso_ingreso_comprobante);
		}
		
		/// <summary>
		/// Realiza la búsqueda y determina que comprobante (CFDI de Recepción de Pagos) activo corresponde al egreso o ingreso
		/// </summary>
		/// <param name="id_egreso_ingreso">Id de Egreso / Ingreso</param>
		/// <returns></returns>
		public static EgresoIngresoComprobante ObtieneComprobanteActivoEgresoIngreso(int id_egreso_ingreso)
		{
			//Declarando objeto predeterminado
			EgresoIngresoComprobante obj = null;
			//Definiendo conjunto de valores para realizar la busqueda desde el SP
			object[] param = { 4, 0, id_egreso_ingreso, 0, 0, 0, 0, 0, true, "", "" };
			//Realizando búsqueda
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Si hay resultados
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Se crea objeto
					obj = new EgresoIngresoComprobante();
					//Se asignan atributos del mismo
					obj._id_egreso_ingreso_comprobante = Convert.ToInt32(ds.Tables["Table"].Rows[0]["Id"]);
					obj._id_egreso_ingreso = Convert.ToInt32(ds.Tables["Table"].Rows[0]["IdEgresoIngreso"]);
					obj._id_comprobante_pago = Convert.ToInt32(ds.Tables["Table"].Rows[0]["IdComprobantePago"]);
					obj._id_tipo_operacion = Convert.ToByte(ds.Tables["Table"].Rows[0]["IdTipoOperacion"]);
					obj._id_estatus = Convert.ToByte(ds.Tables["Table"].Rows[0]["IdEstatus"]);
					obj._id_egreso_ingreso_comprobante_reemplazado = Convert.ToInt32(ds.Tables["Table"].Rows[0]["IdEgresoIngresoComprobanteReemplazado"]);
					obj._habilitar = Convert.ToBoolean(ds.Tables["Table"].Rows[0]["Habilitar"]);
				}
			}
			//Devolviendo objeto resultante
			return obj;
		}

		/// <summary>
		/// Obtiene el conjunto de pagos asignados a un CFDI de Recepción de Pagos específico
		/// </summary>
		/// <param name="id_comprobante_pago">Id del CFDI de Comprobante de Recepción de Pagos</param>
		/// <returns></returns>
		public static DataTable ObtienePagosCFDIRecepcionPagos(int id_comprobante_pago)
		{
			//Declarando Objeto de Retorno
			DataTable mitPagosCFDI = null;
			//Creación del arreglo param
			object[] param = { 5, 0, 0, 0, id_comprobante_pago, 0, 0, 0, true, "", "" };
			//Crea dataset y almacena el resultado de método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Valida los datos del dataset
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					//Asignando Resultado Obtenido
					mitPagosCFDI = DS.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return mitPagosCFDI;
		}

		/// <summary>
		/// Añade un conjunto de Pagos (FI) a un CFDI de Recepción de Pagos
		/// </summary>
		/// <param name="lista_fi">Lista de Pagos y su Id de Relación previa existente</param>
		/// <param name="id_usuario">Id de Usuario que realiza la operación</param>
		/// <param name="id_comprobante_pago">Id de CFDI de Recepción de Pagos</param>
		/// <returns></returns>
		public static RetornoOperacion AgregarIngresosACFDIPagos(List<KeyValuePair<int, int>> lista_fi, int id_usuario, int id_comprobante_pago)
		{
			//Declarando objeto de Retorno
			RetornoOperacion resultado = new RetornoOperacion(id_comprobante_pago);
			//Inicializando bloque transaccional
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Instanciando CFDI de Pagos
				using (Comprobante cfdi = new Comprobante(id_comprobante_pago))
				{
					//SI no se ha generado
					if (cfdi.habilitar && !cfdi.bit_generado)
					{
						//Si la lista contiene elementos válidos
						if (lista_fi.Count(fi => fi.Key == 0) == 0)
						{
							//Se realiza la inserción de la(s) relacion(es) entre el Ingreso y el CFDI registrado
							foreach (KeyValuePair<int, int> fi in lista_fi)
							{
								//Instanciando pago
								using (Bancos.EgresoIngreso ingreso = new Bancos.EgresoIngreso(fi.Key))
								{
									//Validando conjunto de aplicaciones realizadas sobre la FI
									if (ingreso.estatus == Bancos.EgresoIngreso.Estatus.Aplicada)
									{
										//Recuperando aplicaciones de FI
										using (DataTable mitAplicaciones = Reporte.ObtienesDoctoRelacionadoFIFacturaElectronicaRecepcionPago(ingreso.id_egreso_ingreso))
										{
											//Si existe al menos una aplicación
											if (mitAplicaciones != null)
											{
												//Insertando relación de CFDI de pago y FI
												resultado = InsertaEgresoIngresoComprobante(fi.Key, (byte)TipoOperacion.ComprobanteCliente, id_comprobante_pago, (byte)Estatus.Registrado, fi.Value, id_usuario);
												//Preservando Id de Relación FI - Comprobante de Pago
												int id_ingreso_comprobante_pago = resultado.IdRegistro;
												//Si no hay errores
												if (resultado.OperacionExitosa)
												{
													//Para cada aplicación
													foreach (DataRow apl in mitAplicaciones.Rows)
													{
														//Validando que el saldo del Documento Relacionado no sea negativo
														if ((Convert.ToDecimal(apl["SaldoAnterior"]) - Convert.ToDecimal(apl["MontoPago"])) >= 0)
														{
															//Realizando inserción de Documentos Relacionados (CFDI Aplicados en el pago del Cliente - FI)
															resultado = ComprobantePagoDocumentoRelacionado.InsertarComprobantePagoDocumentoRelacionado(ComprobantePagoDocumentoRelacionado.TipoOperacion.Ingreso, id_comprobante_pago, ComprobantePagoDocumentoRelacionado.TipoOperacionDocumento.Ingreso, Convert.ToInt32(apl["IdDocumentoRelacionado"]), fi.Key, Convert.ToInt32(apl["IdAplicacion"]), Convert.ToDecimal(apl["SaldoAnterior"]), Convert.ToDecimal(apl["MontoPago"]), Convert.ToByte(apl["Secuencia"]), id_ingreso_comprobante_pago, id_usuario);
															//Si hay algún error
															if (!resultado.OperacionExitosa)
															{
																//Instanciando CFDI (Docto. Relacionado) con problemas
																using (Comprobante cfdiDoctoRel = new Comprobante(Convert.ToInt32(apl["IdDocumentoRelacionado"])))
																	//Indicando que CFDI tuvo el problema
																	resultado = new RetornoOperacion(string.Format("Error al Registrar Docto. Relacionado '{0}{1}': Este CFDI ya contiene una relación con este documento o bien ya existe el número de parcialidad del mismo.", cfdiDoctoRel.serie, cfdiDoctoRel.folio));
																break;
															}
														}
														else
															resultado = new RetornoOperacion("Esta acción no se puede completar debido a que alguno de los Documentos Relacionados (UUID) se reportaría con Saldo negativo.");
													}
												}
											}
										}
									}
									//Si el estatus no es válido (es distinto de aplicación parcial o total)
									else
										resultado = new RetornoOperacion(string.Format("El estatus actual del pago del cliente es '{0}', no es posible generar su CFDI.", ingreso.estatus));
								}
							}
						}
						else
							resultado = new RetornoOperacion("Uno o más Pagos tienen un identificador '0' (o no hay elementos especificados), es imposible agregar al CFDI.");
					}
					else
						resultado = new RetornoOperacion("El Comprobante ya fue Timbrado. No es posible agregar o quitar Pagos.");
					//Si no hay errores, se confirman cambios realizados
					if (resultado.OperacionExitosa)
					{
						resultado = new RetornoOperacion(id_comprobante_pago);
						scope.Complete();
					}
				}
			}
			return resultado;
		}

		/// <summary>
		/// Añade un conjunto de Pagos (FI) a un CFDI de Recepción de Pagos
		/// </summary>
		/// <param name="lista_egreso_ingreso_comprobante">Lista de Pagos</param>
		/// <param name="id_usuario">Id de Usuario que realiza la operación</param>
		/// <param name="id_comprobante_pago">Id de CFDI de Recepción de Pagos</param>
		/// <returns></returns>
		public static RetornoOperacion EliminarIngresosDeCFDIPagos(List<int> lista_egreso_ingreso_comprobante, int id_usuario, int id_comprobante_pago)
		{
			//Declarando objeto de Retorno
			RetornoOperacion resultado = new RetornoOperacion(id_comprobante_pago);
			//Inicializando bloque transaccional
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Instanciando CFDI de Pagos
				using (Comprobante cfdi = new Comprobante(id_comprobante_pago))
				{
					//SI no se ha generado
					if (cfdi.habilitar && !cfdi.bit_generado)
					{
						//Si la lista contiene elementos válidos
						if (lista_egreso_ingreso_comprobante.Count(fi => fi == 0) == 0)
						{
							//Se realiza la inserción de la(s) relacion(es) entre el Ingreso y el CFDI registrado
							foreach (int fi in lista_egreso_ingreso_comprobante)
							{
								//Deshabilitando relación
								resultado = DeshabilitaEgresoIngresoComprobante(fi, id_usuario);
								//Si hay algún error
								if (!resultado.OperacionExitosa)
									break;
							}
						}
						else
							resultado = new RetornoOperacion("Uno o más Pagos tienen un identificador '0' (o no hay elementos especificados), es imposible agregar al CFDI.");
					}
					else
						resultado = new RetornoOperacion("El Comprobante ya fue Timbrado. No es posible agregar o quitar Pagos.");
					//Si no hay errores, se confirman cambios realizados
					if (resultado.OperacionExitosa)
					{
						resultado = new RetornoOperacion(id_comprobante_pago);
						scope.Complete();
					}
				}
			}
			return resultado;
		}

		/// <summary>
		/// Deshabilita la relación Egreso/Ingreso - CFDI de Pagos, así como los Documentos Relacionados
		/// </summary>
		/// <param name="id_egreso_ingreso_comprobante">Id de Realción Egreso/Ingreso CFDI de Pago</param>
		/// <param name="id_usuario">Id de Usuario que realiza la operación</param>
		/// <returns></returns>
		public static RetornoOperacion DeshabilitaEgresoIngresoComprobante(int id_egreso_ingreso_comprobante, int id_usuario)
		{
			//Declarando objeto de retorno
			RetornoOperacion resultado = new RetornoOperacion(string.Format("Error al Deshabilitar relación Egreso/Ingreso - CFDI. Id: '{0}'", id_egreso_ingreso_comprobante));
			//Realizando transacción
			using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
			{
				//Instanciando Registro
				using (EgresoIngresoComprobante eic = new EgresoIngresoComprobante(id_egreso_ingreso_comprobante))
				{
					//Si el registro existe
					if (eic.habilitar)
					{
						//Deshabilitando
						resultado = eic.DeshabilitaEgresoIngresoComprobante(id_usuario);
						//Si no hay errores
						if (resultado.OperacionExitosa)
							//Deshabilitando relaciones de CFDI (documentos relacionados)
							resultado = ComprobantePagoDocumentoRelacionado.DeshabilitarDocumentosRelacionadosIngresoEgresoComprobantePago(eic.id_egreso_ingreso_comprobante, id_usuario);
					}
				}
				//Si no hay errores
				if (resultado.OperacionExitosa)
					//Confirmando cambios realizados
					scope.Complete();
			}
			//Devolviendo resultado
			return resultado;
		}
		
		/// <summary>
		/// Método encagado de traer consulta para la forma FacturadoRecepcionPagosV10
		/// </summary>
		/// <returns></returns>
		public static DataTable ObtieneEgresos()
		{
			//Declarar retorno
			DataTable dtEgresos = null;
			//Crer arreglo de parametros
			object[] param = { 6, 0, 0, 0, 0, 0, 0, 0, true, "", "" };
			//Consultar BD
			using(DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Valida los datos obtenidos
				if (Validacion.ValidaOrigenDatos(DS, "Table")) dtEgresos = DS.Tables["Table"];
			}
			//Devolver retorno
			return dtEgresos;
		}
		#endregion
	}
}
