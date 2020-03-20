using SAT_CL.Autorizacion;
using SAT_CL.Despacho;
using SAT_CL.Global;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;

namespace SAT_CL.EgresoServicio
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Depositos
    /// </summary>
    public class Deposito : Disposable
    {
        # region Enumerciones
        /// <summary>
        /// Enumera el Estatus del Deposito
        /// </summary>
        public enum EstatusDeposito
        {
            /// <summary>
            /// Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// En Autorizacion
            /// </summary>
            EnAutorizacion,
            /// <summary>
            /// Por Depositar
            /// </summary>
            PorDepositar,
            /// <summary>
            /// Por Liquidar
            /// </summary>
            PorLiquidar,
        }
        /// <summary>
        /// Enumera el tipo de Cargo del Deposito
        /// </summary>
        public enum TipoCargo
        {
            /// <summary>
            /// Depositante
            /// </summary>
            Depositante = 1,
            /// <summary>
            /// Operador
            /// </summary>
            Operador
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "egresos_servicio.sp_deposito_tde";

        private int _id_deposito;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Deposito
        /// </summary>
        public int id_deposito { get { return this._id_deposito; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compañia Emisor
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private int _no_deposito;
        /// <summary>
        /// Atributo encargado de Almacenar el No Deposito
        /// </summary>
        public int no_deposito { get { return this._no_deposito; } }
        private string _identificador_operador_unidad;
        /// <summary>
        /// Atributo encargado de Almacenar el Identificador Operador/Unidad asignado al recurso que se realizó el depósito
        /// </summary>
        public string identificador_operador_unidad { get { return this._identificador_operador_unidad; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        private int _id_cliente_receptor;
        /// <summary>
        /// Atributo encargado de Almacenar el Cliente Receptor
        /// </summary>
        public int id_cliente_receptor { get { return this._id_cliente_receptor; } }
        private int _id_concepto;
        /// <summary>
        /// Atributo encargado de Almacenar el Concepto
        /// </summary>
        public int id_concepto { get { return this._id_concepto; } }
        private int _id_ruta;
        /// <summary>
        /// Atributo encargado de Almacenar la Ruta
        /// </summary>
        public int id_ruta { get { return this._id_ruta; } }
        private int _id_concepto_recurrente;
        /// <summary>
        /// Atributo encargado de Almacenar el Concepto Recurrente
        /// </summary>
        public int id_concepto_recurrente { get { return this._id_concepto_recurrente; } }
        private DateTime _fecha_solicitud;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Solicitud
        /// </summary>
        public DateTime fecha_solicitud { get { return this._fecha_solicitud; } }
        private DateTime _fecha_autorizacion;
        /// <summary>
        /// Atributo encargado de Almacenar la fecha de autorización
        /// </summary>
        public DateTime fecha_autorizacion { get { return this._fecha_autorizacion; } }
        private DateTime _fecha_deposito;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Deósito
        /// </summary>
        public DateTime fecha_deposito { get { return this._fecha_deposito; } }
        private bool _bit_transferencia_contable;
        /// <summary>
        /// Atributo encargado de Almacenar  la transferecia a contabilidad
        /// </summary>
        public bool bit_transferencia_contable { get { return this._bit_transferencia_contable; } }
        private int _id_cuenta_origen;
        /// <summary>
        /// Atributo encargado de Almacenar  la Cuenta Origen
        /// </summary>
        public int id_cuenta_origen { get { return this._id_cuenta_origen; } }
        private int _id_cuenta_destino;
        /// <summary>
        /// Atributo encargado de Almacenar  la Cuenta Destino
        /// </summary>
        public int id_cuenta_destino { get { return this._id_cuenta_destino; } }
        private string _referencia;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private byte _id_tipo_cargo;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public byte id_tipo_cargo { get { return this._id_tipo_cargo; } }
        private bool _bit_efectivo;
        /// <summary>
        /// Atributo encargado de Almacenar si se realizó el depósito en efectivo
        /// </summary>
        public bool bit_efectivo { get { return this._bit_efectivo; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private DetalleLiquidacion _objDetalleLiquidacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Detalle de Liquidación
        /// </summary>
        public DetalleLiquidacion objDetalleLiquidacion { get { return this._objDetalleLiquidacion; } }
        /// <summary>
        /// Describe el Estatus Deposito
        /// </summary>
        public EstatusDeposito Estatus
        {
            get { return (EstatusDeposito)_id_estatus; }
        }
        /// <summary>
        /// Describe el Tipo de Cargo
        /// </summary>
        public TipoCargo Tipo
        {
            get { return (TipoCargo)_id_tipo_cargo; }
        }


        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Deposito()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id Registro</param>
        public Deposito(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Deposito()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_deposito = 0;
            this._id_compania_emisor = 0;
            this._no_deposito = 0;
            this._identificador_operador_unidad = "";
            this._id_estatus = 0;
            this._id_cliente_receptor = 0;
            this._id_concepto = 0;
            this._id_ruta = 0;
            this._id_concepto_recurrente = 0;
            this._fecha_solicitud = DateTime.MinValue;
            this._fecha_autorizacion = DateTime.MinValue;
            this._fecha_deposito = DateTime.MinValue;
            this._bit_transferencia_contable = false;
            this._id_cuenta_origen = 0;
            this._id_cuenta_destino = 0;
            this._referencia = "";
            this._id_tipo_cargo = 0;
            this._bit_efectivo = false;
            this._habilitar = false;
            //this._objDetalleLiquidacion = new DetalleLiquidacion();
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, "", 0, 0, 0, 0, 0, null, null, null, false, 0, 0, "", 0, false, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_deposito = id_registro;
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._no_deposito = Convert.ToInt32(dr["NoDeposito"]);
                        this._identificador_operador_unidad = dr["IdentificadorOperadorUnidad"].ToString();
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_cliente_receptor = Convert.ToInt32(dr["IdClienteReceptor"]);
                        this._id_concepto = Convert.ToInt32(dr["IdConcepto"]);
                        this._id_ruta = Convert.ToInt32(dr["IdRuta"]);
                        this._id_concepto_recurrente = Convert.ToInt32(dr["IdConceptoRecurrente"]);
                        DateTime.TryParse(dr["FechaSolicitud"].ToString(), out this._fecha_solicitud);
                        DateTime.TryParse(dr["FechaAutorizacion"].ToString(), out this._fecha_autorizacion);
                        DateTime.TryParse(dr["FechaDeposito"].ToString(), out this._fecha_deposito);
                        this._bit_transferencia_contable = Convert.ToBoolean(dr["BitTransferenciaContable"]);
                        this._id_cuenta_origen = Convert.ToInt32(dr["IdCuentaOrigen"]);
                        this._id_cuenta_destino = Convert.ToInt32(dr["IdCuentaDestino"]);
                        this._referencia = dr["Referencia"].ToString();
                        this._id_tipo_cargo = Convert.ToByte(dr["IdTipoCargo"]);
                        this._bit_efectivo = Convert.ToBoolean(dr["BitEfectivo"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                    //Inicializando Objeto Detalle de Liquidación
                    this._objDetalleLiquidacion = new DetalleLiquidacion(this._id_deposito, 51);
                }
                else
                    this._objDetalleLiquidacion = new DetalleLiquidacion();
            }
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="identificador_operador_unidad">Identifica el Operador/Unidad asignado al recurso que realizó el depósito</param>
        /// <param name="estatus">Estatus del Deposito</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_concepto">Concepto del Deposito</param>
        /// <param name="id_ruta">Ruta Marcada</param>
        /// <param name="id_concepto_recurrente">Concepto Recurrente de la Ruta</param>
        /// <param name="fecha_solicitud">Fecha de Solicitud</param>
        /// <param name="fecha_autorizacion">Fecha de Autorización</param>
        /// <param name="fecha_deposito">Fecha de Deposito</param>
        /// <param name="bit_transferencia_contable">Bit de Transferencia Contable</param>
        /// <param name="id_cuenta_destino">Cuenta Destino</param>
        /// <param name="id_cuenta_origen">Id Cuenta Origen</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo</param>
        /// <param name="bit_efectivo">Bit Efectivo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_compania_emisor, string identificador_operador_unidad, EstatusDeposito estatus, int id_cliente_receptor, int id_concepto,
                                            int id_ruta, int id_concepto_recurrente, DateTime fecha_solicitud, DateTime fecha_autorizacion, DateTime fecha_deposito,
                                            bool bit_transferencia_contable, int id_cuenta_origen, int id_cuenta_destino, string referencia, TipoCargo id_tipo_cargo, bool bit_efectivo, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_deposito, id_compania_emisor, 0, identificador_operador_unidad, estatus, id_cliente_receptor, id_concepto,
                               id_ruta, id_concepto_recurrente, Fecha.ConvierteDateTimeObjeto(fecha_solicitud), Fecha.ConvierteDateTimeObjeto(fecha_autorizacion),
                               Fecha.ConvierteDateTimeObjeto(fecha_deposito),
                               bit_transferencia_contable, id_cuenta_origen, id_cuenta_destino, referencia, id_tipo_cargo, bit_efectivo, id_usuario, habilitar,"","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="identificador_operador_unidad">Identifica el Operador/Unidad asignado al recurso que realizó el depósito</param>
        /// <param name="estatus">Estatus del Deposito</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_concepto">Concepto del Deposito</param>
        /// <param name="id_ruta">Ruta Marcada</param>
        /// <param name="id_concepto_recurrente">Concepto Recurrente de la Ruta</param>
        /// <param name="fecha_solicitud">Fecha de Solicitud</param>
        /// <param name="fecha_autorizacion">Fecha de Autorización</param>
        /// <param name="fecha_deposito">Fecha de Deposito</param>
        /// <param name="bit_transferencia_contable">Bit de Transferencia Contable</param>
        /// <param name="id_cuenta_origen">Id Cuenta Origen</param>
        /// <param name="id_cuenta_destino">Id Cuenta Destino</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo</param>
        /// <param name="bit_efectivo">Bit Efectivo</param>
        ///<param name="id_unidad">Id Unidad</param>
        ///<param name="id_operador">Id Operador</param>
        ///<param name="id_proveedor_compania">Id Proveedor Compania</param>
        ///<param name="id_servicio">Id Servicio</param>
        ///<param name="id_movimiento">Id movimiento</param>
        ///<param name="valor_unitario">Valor Unitario</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar"> Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosDetalleLiquidacion(int id_compania_emisor, string identificador_operador_unidad, EstatusDeposito estatus, int id_cliente_receptor, int id_concepto,
                                               int id_ruta, int id_concepto_recurrente, DateTime fecha_solicitud, DateTime fecha_autorizacion, DateTime fecha_deposito,
                                               bool bit_transferencia_contable, int id_cuenta_origen, int id_cuenta_destino, string referencia, TipoCargo id_tipo_cargo, bool bit_efectivo, int id_unidad,
                                               int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento, decimal valor_unitario, int id_usuario,
                                               bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            int id_deposito = 0;

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_deposito, id_compania_emisor, 0, identificador_operador_unidad, estatus, id_cliente_receptor, id_concepto,
                               id_ruta, id_concepto_recurrente, fecha_solicitud, fecha_autorizacion, fecha_deposito,
                               bit_transferencia_contable,  id_cuenta_origen, id_cuenta_destino, referencia, id_tipo_cargo, bit_efectivo, id_usuario, habilitar };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Almacenamos Id de Depósito
            id_deposito = result.IdRegistro;

            //Validando que la Operación fue exitosa
            if (result.OperacionExitosa)
            {
                //Editamos Detalle de Liquidación
                result = objDetalleLiquidacion.EditaDetalleLiquidacion(51, result.IdRegistro, objDetalleLiquidacion.id_estatus_liquidacion, id_unidad, id_operador, id_proveedor_compania,
                                   id_servicio, id_movimiento, DateTime.MinValue, 0, 1, 1, valor_unitario, id_usuario);

                //Si se Edito correctamente el Detalle de Liquidación
                if (result.OperacionExitosa)
                {
                    //Asignamos valor por Devolver
                    result = new RetornoOperacion(id_deposito);
                }
            }
            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Válida  las restricciones del Concepto Solicitante
        /// </summary>
        /// <param name="fecha_solicitud">Fecha Solicitud</param>
        /// <param name="id_deposito">Id Depositó Actual en caso de ser una Edición del Concepto</param>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="id_concepto_restriccion">Id Concepto Restriccion</param>
        /// <param name="monto">Monto</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_proveedor_compania">Id Proveedor Unidad</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <returns></returns>
        private static RetornoOperacion validaConceptoRestriccion(DateTime fecha_solicitud, int id_deposito, int id_concepto, int id_concepto_restriccion, decimal monto, int id_servicio, int id_movimiento,
                                                                      int id_proveedor_compania, int id_operador, int id_unidad)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Información del Depósito

            //Instanciando concepto para obtención de tipo de asignación
            using (ConceptoDeposito concepto = new ConceptoDeposito(id_concepto))
            {
                //Si el concepto existe Concepto
                if (concepto.id_concepto_deposito > 0)
                {
                    //Si el Concepto es Tipo General
                    if ((ConceptoDeposito.TipoConcepto)concepto.id_tipo_concepto == ConceptoDeposito.TipoConcepto.General)
                    {
                        //Validando que el concepto sea de compania
                        if (concepto.id_compania_emisor > 0)
                        {
                            //Definiendo contador de incidencias por Servicio y por Movimiento
                            int contadorServicio = 0, contadorMovimiento = 0;

                            //Obteniendo la cantidad de asignaciones ya existentes en BD del mismo concepto sobre movimiento y servicio
                            obtieneTotalAsignacionesConcepto(id_deposito, concepto.id_concepto_deposito, id_servicio, id_movimiento, out contadorServicio, out contadorMovimiento);

                            //Instanciando al detalle que corresponde en base a configuración de parámetros del Servicio
                            using (ConceptoRestriccion cr = new ConceptoRestriccion(id_concepto_restriccion))
                            {
                                //Si hay concepto restriccion 
                                if (cr.id_concepto_restriccion > 0)
                                {
                                    //Validando que las asignaciones actuales no superen los límites permitidos en base a su nivel de asignación (servicio / movimiento)
                                    if (((cr.incidencia_servicio != 0 && cr.incidencia_servicio > contadorServicio) || (cr.incidencia_servicio == 0)) ||
                                        ((cr.incidencia_movimiento != 0 && cr.incidencia_movimiento > contadorMovimiento) || (cr.incidencia_movimiento == 0)))
                                    {

                                        //Determinando si el concepto es válido para la asignación a operador, unidad y proveedor
                                        if ((id_proveedor_compania > 0 && id_unidad == 0 && concepto.bit_asigna_proveedor == true) || (id_operador > 0 && concepto.bit_asigna_operador) ||
                                            (id_proveedor_compania == 0 && id_unidad != 0 && concepto.bit_asigna_tractor == true))
                                        {
                                            //Verificando que el monto solicitado se encuentre dentro de los límites del concepto
                                            if (cr.minimo_monto <= monto && cr.maximo_monto >= monto)
                                                //Verificamos Fecha, Periodo y hora de uso del Concepto
                                                resultado = cr.ValidaFechaConceptoRestriccion(fecha_solicitud);
                                            else
                                                resultado = new RetornoOperacion(string.Format("El monto solicitado debe estar entre {0:c2} y {1:c2}.", cr.minimo_monto, cr.maximo_monto));
                                        }
                                        //Si el concepto no es asignable al operador, unidad o Proveedor
                                        else
                                            resultado = new RetornoOperacion(-2, "El concepto no es asignable a la unidad, operador o Proveedor.", false);
                                    }
                                    //De lo ciontrario, se indica que la cantidad de asignaciones del concepto se ha alcanzado
                                    else
                                        resultado = new RetornoOperacion("No es posible registrar el depósito, la cantidad máxima de asignaciones del concepto se ha alcanzado.");
                                }
                                //Si no existe, se debe a falta de configuración de concepto restriccion
                                else
                                    //Instanciando Excepción
                                    resultado = new RetornoOperacion("No se ha podido encontrar la configuración predeterminada para este concepto.");
                            }
                        }
                        else
                            //Si es concepto Global, no tiene restricciones
                            resultado = new RetornoOperacion(0, "", true);
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("Sólo puedes registrar depósitos de tipo general");
                }
                //De lo contrario indicando error
                else
                    resultado = new RetornoOperacion("No puede registrar un depósito por un concepto no existente.");
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene la cantidad de vaces que un concepto ha sido asignado a un servicio y/o movimientos específicos
        /// </summary>
        /// <param name="id_deposito">Id Depósito en caso de ser una Edición para ser discriminado</param>
        /// <param name="id_concepto">Id de concepto</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="total_servicio">Total de asignaciones del concepto dentro del servicio</param>
        /// <param name="total_movimiento">Total de asignaciones del concepto dentro del movimiento</param>
        private static void obtieneTotalAsignacionesConcepto(int id_deposito, int id_concepto, int id_servicio, int id_movimiento, out int total_servicio, out int total_movimiento)
        {
            //Asignando parámetros de salida
            total_servicio = total_movimiento = 0;

            //Armando Arreglo de Parametros
            object[] param = { 4, id_deposito, 0, 0, "", 0, 0, id_concepto, 0, 0, null, null, null, false, 0, 0, "", 0, false, 0, false, id_servicio, id_movimiento };

            //Realziando la consulta de conteos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table", "Table1"))
                {
                    //Obteniendo total del Servicio
                    total_servicio = ds.Tables["Table"].Rows[0].Field<int>("TotalAsignacionesServicio");
                    //Obteniendo total del movimiento
                    total_movimiento = ds.Tables["Table1"].Rows[0].Field<int>("TotalAsignacionesMovimiento");
                }
            }
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Inserta un depósito realizando las validaciones previas correspondientes
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía que realiza el depósito</param>
        /// <param name="identificador_operador_unidad">Identificador del operador o recurso al que se le realiza el depósito</param>
        /// <param name="id_cliente_receptor">Id de Cliente al que corresponde el depósito</param>
        /// <param name="id_concepto">Id de Concepto del depósito</param>
        /// <param name="id_ruta">Id de Ruta asociada</param>
        /// <param name="id_concepto_recurrente">Id de Concepto Recurrente al que se asocia este depósito</param>
        /// <param name="fecha_solicitud">Fecha de solicitud del depósito</param>
        /// <param name="tipo_cargo">Tipo de cargo</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="bit_efectivo">True para indicar que el pago se realiza en efectivo y no por transferencia electrónica</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDeposito(int id_compania_emisor, string identificador_operador_unidad, int id_cliente_receptor, int id_concepto,
                                                int id_ruta, int id_concepto_recurrente, DateTime fecha_solicitud, string referencia, TipoCargo tipo_cargo, bool bit_efectivo, int id_usuario)
        {
            //Armando parametros para inserción
            object[] param = { 1, 0, id_compania_emisor, 0, identificador_operador_unidad, (byte)EstatusDeposito.Registrado, id_cliente_receptor,
                            id_concepto, id_ruta, id_concepto_recurrente, fecha_solicitud, null, null, false, 0, 0, referencia, (byte)tipo_cargo, bit_efectivo, id_usuario, true, "", "" };

            //Realizando inserción de depósito en BD
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
        }
        /// <summary>
        /// Realiza la inserción de un depósito a operador, unidad o tercero; a su vez registra el detalle de liquidación correspondiente
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía que realiza el depósito</param>
        /// <param name="identificador_operador_unidad">Identificador adicional del operador o recurso beneficiado por el depósito</param>
        /// <param name="id_cliente_receptor">Id de Cliente asociado al depósito</param>
        /// <param name="id_concepto">Id de concepto del depósito</param>
        /// <param name="id_ruta">Id de Ruta de donde procede el depósito</param>
        /// <param name="id_concepto_recurrente">Id de Concepto Recurrente de donde proviene este depósito</param>
        /// <param name="tipo_cargo">Tipo de cargo del depósito (a quien será realizado el cargo del mismo)</param>
        /// <param name="bit_efectivo">True para indicar que el pago se realiza en efectivo y no vía transferencia electrónica</param>
        /// <param name="id_concepto_restriccion">Id de Restricción aplicable al concepto (define las limitantes de cantidad, monto y tiempo de uso del concepto del depósito)</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_proveedor_compania">Id de Tercero</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_movimiento_asignacion">Id de Asignación a Movimiento</param>
        /// <param name="monto_deposito">Monto a depositar</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDeposito(int id_compania_emisor, string identificador_operador_unidad, int id_cliente_receptor, int id_concepto,
                                                        int id_ruta, int id_concepto_recurrente, string referencia, TipoCargo tipo_cargo, bool bit_efectivo,
                                                        int id_concepto_restriccion, int id_unidad, int id_operador, int id_proveedor_compania,
                                                        int id_servicio, int id_movimiento_asignacion, decimal monto_deposito, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Declarando variable para recuperación de Id de Depósito
            int id_deposito = 0;
            //Instanciamos Asignacion
            using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
            {
                //Validamos que la asignación se encuentre iniciada
                if (objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Iniciado ||
                    objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Terminado)
                {
                    //Obteniendo fecha de solicitud de depósito
                    DateTime fecha_solicitud = Fecha.ObtieneFechaEstandarMexicoCentro();

                    //Validamos Restricciones sobre el concepto solicitado para este depósito
                    resultado = validaConceptoRestriccion(fecha_solicitud, 0, id_concepto, id_concepto_restriccion, monto_deposito, id_servicio,
                                                        objMovimientoAsignacion.id_movimiento, id_proveedor_compania, id_operador, id_unidad);
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializando Transacción
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {

                            //Validando que el Movimiento 
                            if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(objMovimientoAsignacion.id_movimiento))
                            {
                                //Insertando depósito
                                resultado = InsertaDeposito(id_compania_emisor, identificador_operador_unidad, id_cliente_receptor, id_concepto, id_ruta,
                                                            id_concepto_recurrente, fecha_solicitud, referencia, tipo_cargo, bit_efectivo, id_usuario);

                                //Validando que la Operación fue exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Recuperando Id de Depósito realizados
                                    id_deposito = resultado.IdRegistro;
                                    //Insertando Detalle de Liquidación
                                    resultado = DetalleLiquidacion.InsertaDetalleLiquidacion(51, id_deposito, id_unidad, id_operador, id_proveedor_compania,
                                                       id_servicio, objMovimientoAsignacion.id_movimiento, 0, 1, 32, monto_deposito, id_usuario);
                                }
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("El Movimiento ha sido Pagado");

                            //Si se Insertó correctamente el Detalle de Liquidación
                            if (resultado.OperacionExitosa)
                            {
                                //Asignamos valor por Devolver
                                resultado = new RetornoOperacion(id_deposito);
                                //Finalizando transacción
                                scope.Complete();
                            }
                        }
                    }
                }
                else
                {
                    //EStablecemos Resultado
                    resultado = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite el registro del depósito.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                }
            }
            //Devolviendo resultado Obtenido
            return resultado;
        }

        /// <summary>
        /// Realiza la inserción de un Depósito Programado a un operador, unidad o tercero; a su vez registra el detalle de liquidación correspondiente.
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía que realiza el depósito</param>
        /// <param name="identificador_operador_unidad">Identificador adicional del operador o recurso beneficiado por el depósito</param>
        /// <param name="id_cliente_receptor">Id de Cliente asociado al depósito</param>
        /// <param name="id_concepto">Id de concepto del depósito</param>
        /// <param name="id_ruta">Id de Ruta de donde procede el depósito</param>
        /// <param name="id_concepto_recurrente">Id de Concepto Recurrente de donde proviene este depósito</param>
        /// <param name="tipo_cargo">Tipo de cargo del depósito (a quien será realizado el cargo del mismo)</param>
        /// <param name="bit_efectivo">True para indicar que el pago se realiza en efectivo y no vía transferencia electrónica</param>
        /// <param name="id_concepto_restriccion">Id de Restricción aplicable al concepto (define las limitantes de cantidad, monto y tiempo de uso del concepto del depósito)</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_proveedor_compania">Id de Tercero</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_movimiento_asignacion">Id de Asignación a Movimiento</param>
        /// <param name="monto_deposito">Monto a depositar</param>
        /// <param name="fecha_ejecucion">Indicador de la sobrecarga para insertar depósitos programados en cualquier estado del viaje</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDeposito(int id_compania_emisor, string identificador_operador_unidad, int id_cliente_receptor, int id_concepto,
                                                        int id_ruta, int id_concepto_recurrente, string referencia, TipoCargo tipo_cargo, bool bit_efectivo,
                                                        int id_concepto_restriccion, int id_unidad, int id_operador, int id_proveedor_compania,
                                                        int id_servicio, int id_movimiento_asignacion, decimal monto_deposito, DateTime fecha_ejecucion,int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Declarando variable para recuperación de Id de Depósito
            int id_deposito = 0;
            //Instanciamos Asignacion
            using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
            {
                //Validamos que la asignación se encuentre iniciada
                if (objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Iniciado ||
                    objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Terminado ||
                    objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Registrado)
                {
                    //Obteniendo fecha de solicitud de depósito
                    DateTime fecha_solicitud = Fecha.ObtieneFechaEstandarMexicoCentro();

                    //Validamos Restricciones sobre el concepto solicitado para este depósito
                    resultado = validaConceptoRestriccion(fecha_solicitud, 0, id_concepto, id_concepto_restriccion, monto_deposito, id_servicio,
                                                        objMovimientoAsignacion.id_movimiento, id_proveedor_compania, id_operador, id_unidad);
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializando Transacción
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {

                            //Validando que el Movimiento 
                            if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(objMovimientoAsignacion.id_movimiento))
                            {
                                //Insertando depósito
                                resultado = InsertaDeposito(id_compania_emisor, identificador_operador_unidad, id_cliente_receptor, id_concepto, id_ruta,
                                                            id_concepto_recurrente, fecha_solicitud, referencia, tipo_cargo, bit_efectivo, id_usuario);

                                //Validando que la Operación fue exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Recuperando Id de Depósito realizados
                                    id_deposito = resultado.IdRegistro;
                                    //Insertando Detalle de Liquidación
                                    resultado = DetalleLiquidacion.InsertaDetalleLiquidacion(51, id_deposito, id_unidad, id_operador, id_proveedor_compania,
                                                       id_servicio, objMovimientoAsignacion.id_movimiento, 0, 1, 32, monto_deposito, id_usuario);
                                }
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("El Movimiento ha sido Pagado");

                            //Si se Insertó correctamente el Detalle de Liquidación
                            if (resultado.OperacionExitosa)
                            {
                                //Asignamos valor por Devolver
                                resultado = new RetornoOperacion(id_deposito);
                                //Finalizando transacción
                                scope.Complete();
                            }
                        }
                    }
                }
                else
                {
                    //EStablecemos Resultado
                    resultado = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite el registró del depósito.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                }
            }
            //Devolviendo resultado Obtenido
            return resultado;
        }


        /// <summary>
        /// Realiza la actualización del estatus del depósito de "Registrado" a : "Por Depositar" -> Si no es en efectivo y no hay autorizaciones pendientes. "En Autorización" -> Si no es efectivo y existen autorizaciones por confirmar. "Por Liquidar" -> Si es en efectivo.
        /// </summary>
        /// <param name="id_usuario">Id de usuario que realiza la solicitud del depósito</param>
        /// <param name="id_concepto_restriccion">Id Concepto Restricción</param>
        /// <returns></returns>
        public RetornoOperacion SolicitarDeposito(int id_concepto_restriccion, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(this._id_deposito);

            //Validando que el estatus actual del depósito sea registrado
            if (this.Estatus == EstatusDeposito.Registrado)
            {
                //Instanciando Concepto
                using (ConceptoDeposito concepto = new ConceptoDeposito(this._id_concepto))
                {
                    //Instanciando al detalle que corresponde en base a configuración de parámetros del Servicio
                    using (ConceptoRestriccion cr = new ConceptoRestriccion(id_concepto_restriccion))
                    {
                        //Si no existe un concepto para esta configuración
                        if (cr.id_concepto_restriccion > 0 || concepto.id_compania_emisor == 0)
                        {
                            //Determinado si es necesario que el depósito pase por un periodo de autorización
                            AutorizacionDetalle detalleAutorizacion = Autorizacion.Autorizacion.CargaAutorizacionesAplicablesRegistro(Autorizacion.Autorizacion.TipoAutorizacion.MontoDepositoServicio, 57, cr.id_concepto_restriccion, objDetalleLiquidacion.monto.ToString());

                            //Si no fue pagado en efectivo
                            if (!this._bit_efectivo)
                            {
                                //Validando la existencia de detalle de autorización
                                if (detalleAutorizacion.id_autorizacion_detalle > 0)
                                {
                                    //Realizando la inserción de autorizaciones requeridas
                                    resultado = detalleAutorizacion.InsertaAutorizacionesRequeridas(51, this._id_deposito, id_usuario);

                                    //Si existe error
                                    if (!resultado.OperacionExitosa)
                                        //Indicando error
                                        resultado = new RetornoOperacion("Error al solicitar autorizaciones.");
                                }
                            }

                            //Si se registraron correctamente, o no existen autorizaciones
                            if (resultado.OperacionExitosa)
                            {
                                //Definiendo el estatus que se asignará al depósito 
                                //Si es efectivo
                                if (this._bit_efectivo)
                                {
                                    //Por liquidar
                                    resultado = ActualizaEstatusAPorLiquidar(Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, id_usuario);
                                }
                                //Si no es efectivo y tiene autorización pendiente
                                else if (detalleAutorizacion.id_autorizacion_detalle > 0)
                                {
                                    //En autorización
                                    resultado = ActualizaEstatusAEnAutorizacion(id_usuario);
                                }
                                //Si no es efectivo y no tiene autorizaciones pendientes
                                else
                                {
                                    //Por depositar
                                    resultado = ActualizaEstatusAPorDepositar(DateTime.MinValue, id_usuario);
                                }
                            }

                        }
                        //De lo contrario se indica el error
                        else
                            resultado = new RetornoOperacion("No fue posible encontrar una configuración correcta para este concepto de acuerdo a la operación que lo solicita.");
                    }
                }
            }
            //De lo contrario se indica el error
            else
                resultado = new RetornoOperacion("El estatus del depósito debe ser 'Registrado' para realizar esta acción.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Edita un Depósito Solicitando
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="id_concepto_restriccion">Id Concepto Restricción</param>
        /// <param name="monto">Monto</param>
        /// <param name="referencia">Comentario del Depóstio</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo del Depósito</param>
        /// <param name="bit_efectivo">Efectivo</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaDeposito(int id_compania_emisor, int id_concepto, int id_concepto_restriccion, decimal monto, string referencia, TipoCargo id_tipo_cargo, bool bit_efectivo, int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos Estatus del Depósito
            if ((EstatusDeposito)this._id_estatus == EstatusDeposito.Registrado)
            {
                //Validamos que No Exista Ruta
                if (this._id_ruta == 0)
                {
                    //Instanciamos Concepto
                    using (SAT_CL.EgresoServicio.ConceptoDeposito objConcepto = new ConceptoDeposito(id_concepto))
                    {
                        //Validamos que no sea Depósito por Diesel
                        if (objConcepto.descripcion != "Diesel (Tractor)" || objConcepto.descripcion != "Diesel (Remolque)")
                            //Instanciamos Detalle Liquidacion
                            using (DetalleLiquidacion objDetalleLiquidacion = new DetalleLiquidacion(this._id_deposito, 51))
                            {
                                //De acuerdo al Tipo de Asignación
                                MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                                //Si la Asignación es para Unidad
                                if (objDetalleLiquidacion.id_unidad != 0)
                                {
                                    //Establecemos Asignación de Unidad
                                    tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
                                }
                                //Si la Asignación es para Tercero
                                else if (objDetalleLiquidacion.id_proveedor_compania != 0)
                                {
                                    //Establecemos Asignación de Tercero
                                    tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                                }
                                //Instanciamos Asignacion
                                using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objDetalleLiquidacion.id_movimiento, tipo)))
                                {
                                    //Validamos que la asignación se encuentre 
                                    if (objMovimientoAsignacion.id_movimiento_asignacion_recurso > 0)
                                    {
                                        //Obteniendo fecha de solicitud de depósito
                                        DateTime fecha_solicitud = Fecha.ObtieneFechaEstandarMexicoCentro();

                                        //Validamos Restricciones sobre el concepto solicitado para este depósito
                                        resultado = validaConceptoRestriccion(fecha_solicitud, this._id_deposito, id_concepto, id_concepto_restriccion, monto, objDetalleLiquidacion.id_servicio,
                                                                            objMovimientoAsignacion.id_movimiento, objDetalleLiquidacion.id_proveedor_compania, objDetalleLiquidacion.id_operador,
                                                                            objDetalleLiquidacion.id_unidad);
                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Inicializando Transacción
                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {

                                                //Validando que el Movimiento 
                                                if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(objMovimientoAsignacion.id_movimiento))
                                                {
                                                    //Editamos depósito
                                                    resultado = this.actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, (EstatusDeposito)this._id_estatus,
                                                                this._id_cliente_receptor, id_concepto, this._id_ruta, this._id_concepto_recurrente, fecha_solicitud, DateTime.MinValue,
                                                                DateTime.MinValue, this._bit_transferencia_contable, this._id_cuenta_origen, this._id_cuenta_destino, referencia,
                                                                id_tipo_cargo, bit_efectivo, id_usuario, this._habilitar);


                                                    //Validando que la Operación fue exitosa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Editamos Detalle de Liquidación
                                                        resultado = objDetalleLiquidacion.EditaDetalleLiquidacion(objDetalleLiquidacion.id_tabla, objDetalleLiquidacion.id_registro, objDetalleLiquidacion.id_estatus_liquidacion,
                                                                    objDetalleLiquidacion.id_unidad, objDetalleLiquidacion.id_operador, objDetalleLiquidacion.id_proveedor_compania, objDetalleLiquidacion.id_servicio,
                                                                    objDetalleLiquidacion.id_movimiento, objDetalleLiquidacion.fecha_liquidacion, objDetalleLiquidacion.id_liquidacion,
                                                                    objDetalleLiquidacion.cantidad, objDetalleLiquidacion.id_unidad_medida, monto, id_usuario);
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    resultado = new RetornoOperacion("El Movimiento ha sido Pagado");

                                                //Si se Insertó correctamente el Detalle de Liquidación
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Asignamos valor por Devolver
                                                    resultado = new RetornoOperacion(id_deposito);
                                                    //Finalizando transacción
                                                    scope.Complete();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //EStablecemos Resultado
                                        resultado = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite el registró del depósito.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                                    }
                                }
                            }
                    }

                }
                else
                    //Establecemos Mensaje Error
                    resultado = new RetornoOperacion("No se puede Editar el depósito ya que fue registrado por una ruta");
            }
            else
            {
                resultado = new RetornoOperacion("Soló puedes editar el depósito en estatus Registrado");
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Edita un Depósito Programado si la fecha de ejecución es mayor a la fecha en que se está editando.
        /// </summary>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_concepto"></param>
        /// <param name="id_concepto_restriccion"></param>
        /// <param name="monto"></param>
        /// <param name="referencia"></param>
        /// <param name="id_tipo_cargo"></param>
        /// <param name="bit_efectivo"></param>
        /// <param name="id_usuario"></param>
        /// <param name="fecha_ejecucion"></param>
        /// <returns></returns>
        public RetornoOperacion EditaDeposito(int id_compania_emisor, int id_concepto, int id_concepto_restriccion, decimal monto, string referencia, TipoCargo id_tipo_cargo, bool bit_efectivo, DateTime fecha_ejecucion, int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que No Exista Ruta
            if (this._id_ruta == 0)
            {
                //Instanciamos Concepto
                using (SAT_CL.EgresoServicio.ConceptoDeposito objConcepto = new ConceptoDeposito(id_concepto))
                {
                    //Validamos que no sea Depósito por Diesel
                    if (objConcepto.descripcion != "Diesel (Tractor)" || objConcepto.descripcion != "Diesel (Remolque)")
                        //Instanciamos Detalle Liquidacion
                        using (DetalleLiquidacion objDetalleLiquidacion = new DetalleLiquidacion(this._id_deposito, 51))
                        {
                            //De acuerdo al Tipo de Asignación
                            MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                            int recurso = 0;
                            //Si la Asignación es para Unidad
                            if (objDetalleLiquidacion.id_unidad != 0)
                            {
                                //Establecemos Asignación de Unidad
                                tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
                                recurso = objDetalleLiquidacion.id_unidad;
                            }
                            //Si la Asignación es para Tercero
                            else if (objDetalleLiquidacion.id_proveedor_compania != 0)
                            {
                                //Establecemos Asignación de Tercero
                                tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                                recurso = objDetalleLiquidacion.id_proveedor_compania;
                            }
                            else
                                recurso = objDetalleLiquidacion.id_operador;

                            /** EN CASO DE VIAJE DOCUMENTADO (ANTICIPO PROGRAMADO) **/
                            int idMovAsigR = 0;
                            idMovAsigR = MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objDetalleLiquidacion.id_movimiento, tipo);
                            if (idMovAsigR == 0)
                                idMovAsigR = MovimientoAsignacionRecurso.ObtieneAsignacionRegistradaRecurso(objDetalleLiquidacion.id_servicio, recurso, tipo);

                            //Instanciamos Asignacion
                            using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(idMovAsigR))
                            {
                                //Validamos que la asignación se encuentre 
                                if (objMovimientoAsignacion.id_movimiento_asignacion_recurso > 0)
                                {
                                    //Obteniendo fecha de solicitud de depósito
                                    DateTime fecha_solicitud = Fecha.ObtieneFechaEstandarMexicoCentro();

                                    //Validamos Restricciones sobre el concepto solicitado para este depósito
                                    resultado = validaConceptoRestriccion(fecha_solicitud, this._id_deposito, id_concepto, id_concepto_restriccion, monto, objDetalleLiquidacion.id_servicio,
                                                                        objMovimientoAsignacion.id_movimiento, objDetalleLiquidacion.id_proveedor_compania, objDetalleLiquidacion.id_operador,
                                                                        objDetalleLiquidacion.id_unidad);
                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Inicializando Transacción
                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {

                                            //Validando que el Movimiento 
                                            if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(objMovimientoAsignacion.id_movimiento))
                                            {
                                                //Editamos depósito
                                                resultado = this.actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, (EstatusDeposito)this._id_estatus,
                                                            this._id_cliente_receptor, id_concepto, this._id_ruta, this._id_concepto_recurrente, fecha_solicitud, DateTime.MinValue,
                                                            DateTime.MinValue, this._bit_transferencia_contable, this._id_cuenta_origen, this._id_cuenta_destino, referencia,
                                                            id_tipo_cargo, bit_efectivo, id_usuario, this._habilitar);


                                                //Validando que la Operación fue exitosa
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Editamos Detalle de Liquidación
                                                    resultado = objDetalleLiquidacion.EditaDetalleLiquidacion(objDetalleLiquidacion.id_tabla, objDetalleLiquidacion.id_registro, objDetalleLiquidacion.id_estatus_liquidacion,
                                                                objDetalleLiquidacion.id_unidad, objDetalleLiquidacion.id_operador, objDetalleLiquidacion.id_proveedor_compania, objDetalleLiquidacion.id_servicio,
                                                                objDetalleLiquidacion.id_movimiento, objDetalleLiquidacion.fecha_liquidacion, objDetalleLiquidacion.id_liquidacion,
                                                                objDetalleLiquidacion.cantidad, objDetalleLiquidacion.id_unidad_medida, monto, id_usuario);
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("El Movimiento ha sido Pagado");

                                            //Si se Insertó correctamente el Detalle de Liquidación
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Asignamos valor por Devolver
                                                resultado = new RetornoOperacion(id_deposito);
                                                //Finalizando transacción
                                                scope.Complete();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //EStablecemos Resultado
                                    resultado = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite el registró del depósito.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                                }
                            }
                        }
                }

            }
            else
                //Establecemos Mensaje Error
                resultado = new RetornoOperacion("No se puede Editar el depósito ya que fue registrado por una ruta");


            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Editar el Depósito
        /// </summary>
        /// <param name="id_cliente_receptor">Id Cliente</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaDeposito(int id_cliente_receptor, int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Invocando Método de Actualización
            resultado = this.actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, (EstatusDeposito)this._id_estatus, id_cliente_receptor, this._id_concepto,
                            this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud, this._fecha_autorizacion, this._fecha_deposito,
                            this._bit_transferencia_contable, this._id_cuenta_origen, this._id_cuenta_destino, this._referencia, (Deposito.TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, this._habilitar);

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Editar el Depósito
        /// </summary>
        /// <param name="id_cliente_receptor">Id Cliente</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaDepositoCuentaDestino(int id_cuenta_destino, int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Invocando Método de Actualización
            resultado = this.actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, (EstatusDeposito)this._id_estatus, this._id_cliente_receptor, this._id_concepto,
                            this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud, this._fecha_autorizacion, this._fecha_deposito,
                            this._bit_transferencia_contable, this._id_cuenta_origen, id_cuenta_destino, this._referencia, (Deposito.TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, this._habilitar);

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método Público encargado de Editar los Depositos
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="identificador_operador_unidad">Identifica el Operador/Unidad asignado al recurso que realizó el depósito</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_concepto">Concepto del Deposito</param>
        /// <param name="id_ruta">Ruta Marcada</param>
        /// <param name="id_concepto_recurrente">Concepto Recurrente de la Ruta</param>
        /// <param name="fecha_solicitud">Fecha de Solicitud</param>
        /// <param name="fecha_autorizacion">Fecha de Autorización</param>
        /// <param name="fecha_deposito">Fecha de Deposito</param>
        /// <param name="bit_transferencia_contable">Bit de Transferencia Contable</param>
        /// <param name="id_cuenta_origen">Id Cuenta Origen</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo</param>
        /// <param name="bit_efectivo">Bit Efectivo</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_proveedor_compania">Id Proveedor Compania</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaDepositoAsignadoDetalleLiquidacion(int id_compania_emisor, string identificador_operador_unidad, int id_cliente_receptor, int id_concepto,
                                               int id_ruta, int id_concepto_recurrente, DateTime fecha_solicitud, DateTime fecha_autorizacion, DateTime fecha_deposito,
                                               bool bit_transferencia_contable, int id_cuenta_origen, int id_cuenta_destino, string referencia, TipoCargo id_tipo_cargo, bool bit_efectivo, int id_unidad,
                                               int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento, decimal valor_unitario, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistrosDetalleLiquidacion(id_compania_emisor, identificador_operador_unidad, (EstatusDeposito)this._id_estatus, id_cliente_receptor, id_concepto,
                            id_ruta, id_concepto_recurrente, fecha_solicitud, fecha_autorizacion, fecha_deposito,
                            bit_transferencia_contable, id_cuenta_origen, id_cuenta_destino, referencia, id_tipo_cargo, bit_efectivo, id_unidad, id_operador,
                            id_proveedor_compania, id_servicio, id_movimiento, valor_unitario, id_usuario, true);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar los Depositos
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDeposito(int id_usuario)
        {
            //Declaramos Objeto Retorno 
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Estatus del deposito
                if (EstatusDeposito.Registrado == (EstatusDeposito)this._id_estatus)
                {
                    //Instanciamos el Detalle Liquidación
                    using (DetalleLiquidacion objDetalleLiquidacion = new DetalleLiquidacion(this._id_deposito, 51))
                    {
                        //Validando que exista el Detalle
                        if (objDetalleLiquidacion.habilitar)
                        {
                            //Declarando Variable Auxiliar
                            bool validaFacturaProveedor = false;

                            //Instanciando Concepto
                            using (ConceptoDeposito concepto = new ConceptoDeposito(this._id_concepto))
                            {
                                //Validando que exista el Concepto
                                if (concepto.habilitar)
                                {
                                    //Validando que el Concepto sea de "Anticipos de Proveedor"
                                    if (concepto.descripcion.Equals("Anticipo Proveedor") || concepto.descripcion.Equals("Finiquito Proveedor"))
                                    {
                                        //Obteniendo Validación
                                        validaFacturaProveedor = SAT_CL.CXP.FacturadoProveedor.ValidaFacturasDeposito(this._id_deposito);
                                    }
                                }
                            }

                            //Validando Resultado
                            if (!validaFacturaProveedor)
                            {
                                //Eliminamos Detalle Liquidacion
                                resultado = objDetalleLiquidacion.DeshabilitaDetalleLiquidacion(id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Invocando Método de Actualización
                                    resultado = this.actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, (EstatusDeposito)this._id_estatus, this._id_cliente_receptor, this._id_concepto,
                                                    this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud, this._fecha_autorizacion, this._fecha_deposito,
                                                    this._bit_transferencia_contable, this.id_cuenta_origen, this._id_cuenta_destino, this._referencia, (TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, false);

                                    //Validando Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Validando si el Concepto es de Diesel
                                        if (this._id_concepto == 9)
                                        {
                                            //Instanciando Vale de Diesel
                                            using (SAT_CL.EgresoServicio.AsignacionDiesel vale_diesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneValePorDeposito(resultado.IdRegistro))
                                            {
                                                //Validando que exista el Vale
                                                if (vale_diesel.id_asignacion_diesel > 0)
                                                {
                                                    //Deshabilitando Vale generado por Deposito
                                                    resultado = vale_diesel.DeshabilitaAsignacionDiesel(id_usuario);

                                                    //Validando operacion Exitosa
                                                    if (resultado.OperacionExitosa)

                                                        //Deshabilitando Detalle de Liquidación
                                                        resultado = vale_diesel.objDetalleLiquidacion.DeshabilitaDetalleLiquidacion(id_usuario);
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    resultado = new RetornoOperacion("No Existe el Vale de Diesel");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("El Deposito contiene Facturas Ligadas.");
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("Sólo puedes eliminar el depósito en estatus registrado.");

                //Validamos Resultado
                if (resultado.OperacionExitosa)

                    //Completando Transacción
                    scope.Complete();
            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }

        /// <summary>
        /// Deshabilita depósitos programados si su fecha de ejecución es mayor a la fecha actual.
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="fecha_ejecucion"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDeposito(int id_usuario, DateTime fecha_ejecucion)
        {
            //Declaramos Objeto Retorno 
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Estatus del deposito
                if (fecha_ejecucion >= DateTime.Today)
                {
                    //Instanciamos el Detalle Liquidación
                    using (DetalleLiquidacion objDetalleLiquidacion = new DetalleLiquidacion(this._id_deposito, 51))
                    {
                        //Validando que exista el Detalle
                        if (objDetalleLiquidacion.habilitar)
                        {
                            //Declarando Variable Auxiliar
                            bool validaFacturaProveedor = false;

                            //Instanciando Concepto
                            using (ConceptoDeposito concepto = new ConceptoDeposito(this._id_concepto))
                            {
                                //Validando que exista el Concepto
                                if (concepto.habilitar)
                                {
                                    //Validando que el Concepto sea de "Anticipos de Proveedor"
                                    if (concepto.descripcion.Equals("Anticipo Proveedor") || concepto.descripcion.Equals("Finiquito Proveedor"))
                                    {
                                        //Obteniendo Validación
                                        validaFacturaProveedor = SAT_CL.CXP.FacturadoProveedor.ValidaFacturasDeposito(this._id_deposito);
                                    }
                                }
                            }

                            //Validando Resultado
                            if (!validaFacturaProveedor)
                            {
                                //Eliminamos Detalle Liquidacion
                                resultado = objDetalleLiquidacion.DeshabilitaDetalleLiquidacion(id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Invocando Método de Actualización
                                    resultado = this.actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, (EstatusDeposito)this._id_estatus, this._id_cliente_receptor, this._id_concepto,
                                                    this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud, this._fecha_autorizacion, this._fecha_deposito,
                                                    this._bit_transferencia_contable, this.id_cuenta_origen, this._id_cuenta_destino, this._referencia, (TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, false);

                                    //Validando Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Validando si el Concepto es de Diesel
                                        if (this._id_concepto == 9)
                                        {
                                            //Instanciando Vale de Diesel
                                            using (SAT_CL.EgresoServicio.AsignacionDiesel vale_diesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneValePorDeposito(resultado.IdRegistro))
                                            {
                                                //Validando que exista el Vale
                                                if (vale_diesel.id_asignacion_diesel > 0)
                                                {
                                                    //Deshabilitando Vale generado por Deposito
                                                    resultado = vale_diesel.DeshabilitaAsignacionDiesel(id_usuario);

                                                    //Validando operacion Exitosa
                                                    if (resultado.OperacionExitosa)

                                                        //Deshabilitando Detalle de Liquidación
                                                        resultado = vale_diesel.objDetalleLiquidacion.DeshabilitaDetalleLiquidacion(id_usuario);
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    resultado = new RetornoOperacion("No Existe el Vale de Diesel");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("El Deposito contiene Facturas Ligadas.");
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("El depósito no puede ser eliminado porque ya fue ejecutado.");

                //Validamos Resultado
                if (resultado.OperacionExitosa)

                    //Completando Transacción
                    scope.Complete();
            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Atributos del Deposito
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDeposito()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_deposito);
        }
        /// <summary>
        /// Método Público encargado de Actualizar la Liquidación del Deposito
        /// </summary>
        /// <param name="id_liquidacion">Liquidación</param>
        /// <param name="estatus">Estatus de Actualización</param>
        /// <param name="id_usuario">Usuario que actualiza el Deposito</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaLiquidacionDeposito(int id_liquidacion, DetalleLiquidacion.Estatus estatus, int id_usuario)
        {
            //Editamos Detalle de Liquidación
            return objDetalleLiquidacion.EditaDetalleLiquidacion(51, this._id_deposito, (byte)estatus, this._objDetalleLiquidacion.id_unidad,
                                this._objDetalleLiquidacion.id_operador, this._objDetalleLiquidacion.id_proveedor_compania, this._objDetalleLiquidacion.id_servicio, this._objDetalleLiquidacion.id_movimiento,
                                DateTime.MinValue, id_liquidacion, this._objDetalleLiquidacion.cantidad, this._objDetalleLiquidacion.id_unidad_medida,
                                this._objDetalleLiquidacion.valor_unitario, id_usuario);
        }

        /// <summary>
        /// Actualiza el estatus del depósito a "Por Liquidar"
        /// </summary>   
        /// <param name="fecha_deposito">Fecha Depósito</param>
        /// <param name="id_cuenta_origen">Id Cuenta Origen</param>
        /// <param name="id_cuenta_destino">Id Cuenta Destino</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusAPorLiquidar(DateTime fecha_deposito, int id_cuenta_origen, int id_cuenta_destino, int id_usuario)
        {

            //Realizando la actualización
            return actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, EstatusDeposito.PorLiquidar, this._id_cliente_receptor,
                                                                this._id_concepto, this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud,
                                                                this._fecha_autorizacion, fecha_deposito, this._bit_transferencia_contable, id_cuenta_origen, id_cuenta_destino,
                                                                this._referencia, (TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Actualiza el estatus del depósito a "Por Depositar", actualizando la fecha de autorización del mismo
        /// </summary>
        /// <param name="fecha_autorizacion">Fecha de autorización</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusAPorDepositar(DateTime fecha_autorizacion, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando estatus permitido
            if (this.Estatus == EstatusDeposito.EnAutorizacion || this.Estatus == EstatusDeposito.Registrado)
            {
                //Inicializar transacción para realizar operaciones de solicitud de depósito
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Realizando la actualización
                    resultado = actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, EstatusDeposito.PorDepositar, this._id_cliente_receptor,
                                                                    this._id_concepto, this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud,
                                                                    fecha_autorizacion, this._fecha_deposito, this._bit_transferencia_contable,
                                                                    this._id_cuenta_origen, this._id_cuenta_destino, this._referencia,
                                                                    (TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, this._habilitar);

                    //Si no hay errores en actualización de depóstio
                    if (resultado.OperacionExitosa)

                        //Insertando egreso de banco para cubrir el depósito
                        resultado = Bancos.EgresoIngreso.InsertarEgresoDepositoOperadorYPermisionario(this._id_compania_emisor, this._id_deposito, this.objDetalleLiquidacion.monto, id_usuario);

                    //Si no hay errores en el proceso
                    if (resultado.OperacionExitosa)
                    {
                        //Actualziando resultado con el Id de depósito afectado
                        resultado = new RetornoOperacion(this._id_deposito);
                        //Confirmando cambios realizados
                        scope.Complete();
                    }
                }
            }
            else
                resultado = new RetornoOperacion(String.Format("El estatus actual del anticipo es '{0}'.", this.Estatus.ToString()));

            //Devolvinedo resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza el estatus del depósito a "En Autorización"
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusAEnAutorizacion(int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando la actualización
            resultado = actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, EstatusDeposito.EnAutorizacion, this._id_cliente_receptor,
                                                            this._id_concepto, this._id_ruta, this._id_concepto_recurrente, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                            this._fecha_autorizacion, this._fecha_deposito, this._bit_transferencia_contable,
                                                            this._id_cuenta_origen, this._id_cuenta_destino, this._referencia, (TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, this._habilitar);
            //Devolvinedo resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza el estatus del depósito a "Registrado".
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusARegistrado(int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando la actualización
            resultado = actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, EstatusDeposito.Registrado, this._id_cliente_receptor,
                                                            this._id_concepto, this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud,
                                                            this._fecha_autorizacion, this._fecha_deposito, this._bit_transferencia_contable, this._id_cuenta_origen, this._id_cuenta_destino,
                                                            this._referencia, (TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, this._habilitar);
            //Devolvinedo resultado
            return resultado;
        }

        /// <summary>
        ///  Carga registros por depósitar ligando un Id Compañia
        /// </summary>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static DataTable CargaRegistrosPorDepositar(int id_compania)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_compania, 0, "", 0, 0, 0, 0, 0, null, null, null, false, 0, 0, "", 0, false, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }


        /// <summary>
        /// Validamos que no existan depósitos asignados al movimiento.
        /// </summary>
        /// <param name="objMovimiento">Id Movimiento</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaRegistrosDeDepositos(Movimiento objMovimiento)
        {
            //Asignando parámetros de salida
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicialzaindo parámetros de consulta
            object[] param = { 6, 0, 0, 0, "", 0, 0, 0, 0, 0, null, null, null, false, 0, 0, "", 0, false, 0, false, objMovimiento.id_movimiento, "" };

            //Realziando la consulta de conteos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    //Obteniendo total del Servicio
                    int total_depositos = ds.Tables["Table"].Rows[0].Field<int>("TotalDepositos");

                    //Validamos existencia de depósitos
                    if (total_depositos > 0)
                    {
                        //Instanciamos Parada Origen y Destino
                        using (Parada objParadaOrigen = new Parada(objMovimiento.id_parada_origen), objParadaDestino = new Parada(objMovimiento.id_parada_destino))
                        {
                            //EStablecemos Resultado
                            resultado = new RetornoOperacion("Existen depósitos asignados al movimiento " + objParadaOrigen.descripcion + " - " + objParadaDestino.descripcion);
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Cargar los Depositos Asignados a los Movimientos
        /// </summary>
        /// <param name="id_movimiento">Referencia del Movimiento</param>
        /// <param name="id_entidad">Entidad de Busqueda</param>
        /// <param name="id_tipo_entidad">Tipo de la Entidad</param>
        /// <returns></returns>
        public static DataTable CargaDepositosMovimiento(int id_movimiento, int id_entidad, byte id_tipo_entidad)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 7, id_movimiento, 0, id_entidad, "", id_tipo_entidad, 0, 0, 0, 0, null, null, null, false, 0, 0, "", 0, false, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }


        /// <summary>
        /// Método Público encargado de Cargar los Depositos Asignados a los Movimientos del servicio señalado
        /// </summary>
        /// <param name="id_servicio">Referencia del Servicio a consultar</param>
        /// <param name="id_entidad">Entidad de Busqueda</param>
        /// <param name="id_tipo_entidad">Tipo de la Entidad</param>
        /// <returns></returns>
        public static DataTable CargaDepositosServicio(int id_servicio, int id_entidad, byte id_tipo_entidad)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Creando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo todos los movimientos del servicio
                using (DataTable mitMovimientos = Movimiento.CargaMovimientos(id_servicio))
                {
                    //Si existen movimientos
                    if (mitMovimientos != null)
                    {
                        //Para cada uno de los registros involucrados
                        foreach (DataRow m in mitMovimientos.Rows)
                        {
                            //Obteniendo sus vales asignados
                            using (DataTable mitVales = CargaDepositosMovimiento(m.Field<int>("Id"), id_entidad, id_tipo_entidad))
                            {
                                //Si hay vales se añaden al resultante
                                if (mitVales != null)
                                {
                                    //Si no hay vales agregados aun
                                    if (mit == null)
                                        mit = new DataTable();

                                    mit.Merge(mitVales, true, MissingSchemaAction.Add);
                                }
                            }
                        }
                    }
                }
                scope.Complete();
            }

            //Devolviendo resultado
            return mit;
        }
        /// <summary>
        ///  Carga registros por depósitar ligando un Id Compañia
        /// </summary>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static DataTable CargaDepositosPorLiquidar(int id_compania, int no_deposito, DateTime fecha_deposito, DateTime fecha_documentacion, int concepto, string referencia, int no_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 8, 0, id_compania, no_deposito, "", 0, 0, concepto, 0, 0, fecha_documentacion == DateTime.MinValue ? "" : fecha_documentacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                null, fecha_deposito == DateTime.MinValue ? "" : fecha_deposito.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), false, 0, 0, referencia, 0, false, 0, false, no_servicio, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de Editar el Depósito
        /// </summary>
        /// <param name="id_cliente_receptor">Id Cliente</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion RegresaDepositoTesoreria(int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Invocando Método de Actualización
            resultado = this.actualizaRegistro(this._id_compania_emisor, this._identificador_operador_unidad, EstatusDeposito.PorDepositar, this._id_cliente_receptor, this._id_concepto,
                            this._id_ruta, this._id_concepto_recurrente, this._fecha_solicitud, this._fecha_autorizacion, this._fecha_deposito,
                            this._bit_transferencia_contable, 0, 0, this._referencia, (Deposito.TipoCargo)this._id_tipo_cargo, this._bit_efectivo, id_usuario, this._habilitar);

            //Devolvemos Resultado
            return resultado;
        }
        #endregion
    }
}