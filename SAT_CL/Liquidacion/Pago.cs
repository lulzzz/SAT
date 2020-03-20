using SAT_CL.EgresoServicio;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.Liquidacion
{   
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Pagos
    /// </summary>
    public class Pago : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_pago_tp";

        private int _id_pago;
        /// <summary>
        /// Atributo encargado de almacenar el Pago
        /// </summary>
        public int id_pago { get { return this._id_pago; } }
        private int _id_tipo_pago;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Pago
        /// </summary>
        public int id_tipo_pago { get { return this._id_tipo_pago; } }
        private int _id_tarifa;
        /// <summary>
        /// Atributo encargado de almacenar la Tarifa
        /// </summary>
        public int id_tarifa { get { return this._id_tarifa; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private string _referencia;
        /// <summary>
        /// Atributo encargado de almacenar la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private DetalleLiquidacion _objDetallePago;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle del Pago
        /// </summary>
        public DetalleLiquidacion objDetallePago { get { return this._objDetallePago; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Pago()
        {   //Asignando Valores
            this._id_pago = 0;
            this._id_tipo_pago = 0;
            this._id_tarifa = 0;
            this._descripcion = "";
            this._referencia = "";
            this._objDetallePago = new DetalleLiquidacion();
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_pago">Id de Pago</param>
        public Pago(int id_pago)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_pago);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Pago()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_pago">Id de Pago</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_pago)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_pago, 0, 0, "", "", 0, false, "", "" };
            //Obteniendo Registro del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_pago = id_pago;
                        this._id_tipo_pago = Convert.ToInt32(dr["IdTipoPago"]);
                        this._id_tarifa = Convert.ToInt32(dr["IdTarifa"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._referencia = dr["Referencia"].ToString();
                        this._objDetallePago = new DetalleLiquidacion(id_pago, 79);
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
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_tipo_pago">Tipo de Pago</param>
        /// <param name="id_tarifa">Tarifa</param>
        /// <param name="descripcion">Descripcion del Pago</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_tipo_pago, int id_tarifa, string descripcion, string referencia, 
                                                    int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_pago, id_tipo_pago, id_tarifa, descripcion, referencia, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Pagos
        /// </summary>
        /// <param name="id_tipo_pago">Tipo de Pago</param>
        /// <param name="id_tarifa">Tarifa</param>
        /// <param name="descripcion">Descripcion del Pago</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPago(int id_tipo_pago, int id_tarifa, string descripcion, string referencia,
                                                   int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            RetornoOperacion resultDetalleLiq = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tipo_pago, id_tarifa, descripcion, referencia, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_tipo_pago"></param>
        /// <param name="id_tarifa"></param>
        /// <param name="descripcion"></param>
        /// <param name="referencia"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_proveedor_compania"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_movimiento"></param>
        /// <param name="id_liquidacion"></param>
        /// <param name="cantidad"></param>
        /// <param name="id_unidad_medida"></param>
        /// <param name="valor_unitario"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPago(int id_tipo_pago, int id_tarifa, string descripcion, string referencia, int id_unidad, 
                                            int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento, int id_liquidacion, 
                                            decimal cantidad, int id_unidad_medida, decimal valor_unitario, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            RetornoOperacion resultDetalleLiq = new RetornoOperacion();
            
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tipo_pago, id_tarifa, descripcion, referencia, id_usuario, true, "", "" };
            
            //Validando que exista el tipo de pago
            if (id_tipo_pago != 0)
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Resultado del SP
                    result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                    //Validando que la Operación haya sido Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Insertando Detalle de Liquidación
                        resultDetalleLiq = SAT_CL.EgresoServicio.DetalleLiquidacion.InsertaDetalleLiquidacion(79, result.IdRegistro, id_unidad, id_operador,
                                                id_proveedor_compania, id_servicio, id_movimiento, id_liquidacion, cantidad, id_unidad_medida,
                                                valor_unitario, id_usuario);

                        //Validando que la Operación haya sido Exitosa
                        if (resultDetalleLiq.OperacionExitosa)
                            //Completando Transaccion
                            trans.Complete();
                    }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe Seleccionar un Tipo de Pago");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Pagos
        /// </summary>
        /// <param name="id_tipo_pago">Tipo de Pago</param>
        /// <param name="id_tarifa">Tarifa</param>
        /// <param name="descripcion">Descripcion del Pago</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_proveedor_compania">Id de Proveedor</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento al que esta Ligado el Pago</param>
        /// <param name="id_liquidacion">Liquidación del Pago</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaPago(int id_tipo_pago, int id_tarifa, string descripcion, string referencia, int id_unidad, 
                                            int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento, int id_liquidacion, 
                                            decimal cantidad, int id_unidad_medida, decimal valor_unitario, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            RetornoOperacion resultDetallePago = new RetornoOperacion();
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {   //Invocando Método de Actualización
                result = this.actualizaRegistros(id_tipo_pago, id_tarifa, descripcion, referencia, id_usuario, this._habilitar);
                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                {   //Actualizando el Detalle del Pago
                    resultDetallePago = this._objDetallePago.EditaDetalleLiquidacion(79, result.IdRegistro, this._objDetallePago.id_estatus_liquidacion, id_unidad, id_operador,
                                            id_proveedor_compania, id_servicio, id_movimiento, this._objDetallePago.fecha_liquidacion, id_liquidacion, cantidad, id_unidad_medida,
                                            valor_unitario, id_usuario);
                    //Validando que la Operación haya sido Exitosa
                    if (resultDetallePago.OperacionExitosa)
                        //Completando Transaccion
                        trans.Complete();
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Pagos
        /// </summary>
        /// <param name="id_tipo_pago">Tipo de Pago</param>
        /// <param name="id_tarifa">Tarifa</param>
        /// <param name="descripcion">Descripcion del Pago</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaPago(int id_tipo_pago, int id_tarifa, string descripcion, string referencia,
                                                   int id_usuario)
        {   
            //Invocando Método de Actualización
            return this.actualizaRegistros(id_tipo_pago, id_tarifa, descripcion, referencia, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encaragdo de Deshabilitar los Pagos
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPago(int id_usuario)
        {   
            //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tipo_pago, this._id_tarifa, this._descripcion, this._referencia, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPago()
        {   
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_pago);
        }
        /// <summary>
        /// Método Plúbico encargado de Obtener las Comprobaciones Ligadas a un Movimiento
        /// </summary>
        /// <param name="id_movimiento">Referencia al Movimiento</param>
        /// <param name="id_liquidacion">Referencia a la Liquidación</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignacion (Unidad, Operador, Tercero)</param>
        /// <param name="id_recurso">Referencia del Recurso</param>
        /// <param name="id_estatus_liq">Estatus de la Liquidación</param>
        /// <returns></returns>
        public static DataTable ObtienePagosMovimiento(int id_movimiento, int id_liquidacion, int id_tipo_asignacion, int id_recurso, byte id_estatus_liq, bool pagos_otros)
        {
            //Declarando Objeto de Retorno
            DataTable dtComprobaciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, id_movimiento, id_liquidacion, id_estatus_liq, 0, 0, 0, pagos_otros, id_tipo_asignacion.ToString(), id_recurso.ToString() };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtComprobaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtComprobaciones;
        }
        /// <summary>
        /// Genera el pago correspondiente al movimiento indicado, aplicando los criterios de la tarifa solicitada
        /// </summary>
        /// <param name="id_liquidacion">Id de Liquidación</param>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_transportista">Id de Transportista</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public static RetornoOperacion AplicaTarifaPagoMovimiento(int id_liquidacion, int id_tarifa, int id_movimiento, int id_unidad, int id_operador, int id_transportista, int id_usuario)
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_movimiento);

            //Creando bloque transaccional
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando id de pago por realizar
                int id_pago = 0;

                //Instanciando movimiento de interés
                using (Despacho.Movimiento mov = new Despacho.Movimiento(id_movimiento))
                {
                    //Si el movimiento está activo
                    if (mov.habilitar)
                    {
                        //Instanciando la Tarifa correspondiente
                        using (TarifasPago.Tarifa tarifa = new TarifasPago.Tarifa(id_tarifa))
                        {
                            //Si la tarifa existe
                            if (tarifa.habilitar)
                            {
                                //Declarando variable para almacenar valor de tarifa a utilizar
                                decimal valor_unitario_tarifa = 0, cantidad_tarifa = 0;
                                int id_tipo_pago_tarifa = 0;

                                //Se determina el tipo de tarifa por aplicar (fija o variable)
                                //Si existen detalles de matriz (variable)
                                if (tarifa.filtro_col != TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica || tarifa.filtro_row != TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica)
                                {
                                    //Declarando variables necesarias para almacenar criterios de búsqueda en matríz (valores columna / fila)
                                    string descipcion_col = "", descipcion_fila = "", operador_col = "=", operador_row = "=";

                                    //Extrayendo los datos requeridos del servicio, para realizar búsqueda sobre la matriz de la tarifa
                                    resultado = mov.ExtraeCriteriosMatrizTarifaPago(tarifa.filtro_col, tarifa.filtro_row, tarifa.id_base_tarifa, out descipcion_col, out descipcion_fila, out operador_col, out operador_row);

                                    //Si no hay errores en recuperación de criterios de búsqueda
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Localizando valor en matríz mediante los filtros encontrados
                                        using (TarifasPago.TarifaMatriz detalle_tarifa = TarifasPago.TarifaMatriz.ObtieneDetalleMatrizTarifa(tarifa.id_tarifa, descipcion_col, descipcion_fila, operador_col, operador_row))
                                        {
                                            //Si fue encontrado un elemento en la matríz de tarifa
                                            if (detalle_tarifa.habilitar)
                                            {
                                                //Recuperando valor de matriz acorde a estatus de carga (cargado, vacío o tronco)
                                                switch (mov.TipoMovimiento)
                                                {
                                                    case Despacho.Movimiento.Tipo.Cargado:
                                                        valor_unitario_tarifa = detalle_tarifa.tarifa_cargado;
                                                        break;
                                                    case Despacho.Movimiento.Tipo.EnTronco:
                                                        valor_unitario_tarifa = detalle_tarifa.tarifa_tronco;
                                                        break;
                                                    case Despacho.Movimiento.Tipo.Vacio:
                                                        valor_unitario_tarifa = detalle_tarifa.tarifa_vacio;
                                                        break;
                                                }
                                            }
                                            //Si no hay coincidencia en la matríz
                                            else
                                                resultado = new RetornoOperacion(string.Format("La tarifa '{0}' no poseé una coincidencia '{1}' - '{2}' para este movimiento.", tarifa.descripcion, tarifa.filtro_row.ToString(), tarifa.filtro_col.ToString())); ;
                                        }
                                    }
                                }
                                //Si es una tarifa fija
                                else
                                {
                                    //Determianndo el tipo de movimiento realizado
                                    switch (mov.TipoMovimiento)
                                    {
                                        case Despacho.Movimiento.Tipo.Cargado:
                                            valor_unitario_tarifa = tarifa.valor_unitario;
                                            break;
                                        case Despacho.Movimiento.Tipo.EnTronco:
                                            valor_unitario_tarifa = tarifa.valor_unitario_tronco;
                                            break;
                                        case Despacho.Movimiento.Tipo.Vacio:
                                            valor_unitario_tarifa = tarifa.valor_unitario_vacio;
                                            break;
                                    }
                                }

                                //Si no hay errores de obtención de tarifa
                                if (resultado.OperacionExitosa)
                                {
                                    //Calculando la cantidad sobre la que aplicará el valor unitario de tarifa
                                    cantidad_tarifa = mov.ExtraeCriterioBaseTarifaPago(tarifa.id_base_tarifa, out id_tipo_pago_tarifa);

                                    //Instanciando tipo de pago principal
                                    using (TipoPago tp = new TipoPago(id_tipo_pago_tarifa))
                                    {
                                        //Validando existencia de tipo de pago
                                        if (tp.id_tipo_pago > 0)
                                        {
                                            //Insertando pago principal de de tarifa
                                            resultado = InsertaPago(tp.id_tipo_pago, id_tarifa, mov.descripcion, string.Format("Movimiento '{0}'", mov.id_movimiento), id_unidad, id_operador, id_transportista,
                                                                    mov.id_servicio, mov.id_movimiento, id_liquidacion, cantidad_tarifa, tp.id_unidad_medida,
                                                                    valor_unitario_tarifa, id_usuario);

                                            //Si se registró correctamente el pago
                                            if(resultado.OperacionExitosa)
                                            {
                                                //Conservando id de pago
                                                id_pago = resultado.IdRegistro;
                                                //Insertando relación del pago con el movimiento
                                                resultado = PagoMovimiento.InsertaPagoMovimiento(id_pago, id_movimiento, id_usuario);
                                            }
                                        }
                                        //De lo contrario
                                        else
                                            resultado = new RetornoOperacion(string.Format("El tipo de pago base para la tarifa '{0}' no pudo ser encontrado, favor de revisar la configuración.", tarifa.descripcion));
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion(string.Format("Error al recuperar los datos de la tarifa Id '{0}'", id_tarifa));
                        }
                    }
                    //Reportando error
                    else
                        resultado = new RetornoOperacion(string.Format("El Movimiento '{0}' no pudo ser recuperado.", id_movimiento));
                }

                //Si no hay errores, se confirma la transacción
                if (resultado.OperacionExitosa)
                {
                    resultado = new RetornoOperacion(id_pago);
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera el pago correspondiente al movimiento indicado, aplicando los criterios de la tarifa solicitada
        /// </summary>
        /// <param name="id_liquidacion">Id de Liquidación</param>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_transportista">Id de Transportista</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public static RetornoOperacion AplicaTarifaPagoServicio(int id_liquidacion, int id_tarifa, int id_servicio, int id_unidad, int id_operador, int id_transportista, int id_usuario)
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_servicio);

            //Creando bloque transaccional
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando id de pago por realizar
                int id_recurso = 0;
                Despacho.MovimientoAsignacionRecurso.Tipo tipo_recurso;

                //Determinando el tipo de recurso
                if (id_unidad > 0)
                {
                    tipo_recurso = Despacho.MovimientoAsignacionRecurso.Tipo.Unidad;
                    id_recurso = id_unidad;
                }
                else if (id_operador > 0)
                {
                    tipo_recurso = Despacho.MovimientoAsignacionRecurso.Tipo.Operador;
                    id_recurso = id_operador;
                }
                else
                {
                    tipo_recurso = Despacho.MovimientoAsignacionRecurso.Tipo.Tercero;
                    id_recurso = id_transportista;
                }

                //Validando que la Tarifa sea Secundaria
                if (SAT_CL.TarifasPago.TarifaCompuesta.ValidaTarifaSecundaria(id_tarifa))
                {
                    //Obteniendo conjunto de movimientos asociados al servicio, cuya asignación pertenece al mismo recurso (id_unidad, id_operador o transportista)
                    using (DataTable mitMovimientos = Despacho.Movimiento.ObtieneMovimientosServcioRecurso(id_servicio, tipo_recurso, id_recurso))
                    {
                        //Validando que existan movimientos pendientes
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(mitMovimientos))
                        {
                            //Instanciando la Tarifa correspondiente
                            using (TarifasPago.Tarifa tarifa = new TarifasPago.Tarifa(id_tarifa))
                            {
                                //Si la tarifa existe
                                if (tarifa.habilitar)
                                {
                                    //En base al nivel de aplicación de la tarifa
                                    switch (tarifa.nivel_pago)
                                    {
                                        case TarifasPago.Tarifa.NivelPago.Servicio:

                                            //Instanciando servicio
                                            using (Documentacion.Servicio servicio = new Documentacion.Servicio(id_servicio))
                                            {
                                                //Si el servicio existe
                                                if (servicio.habilitar)
                                                {
                                                    //Declarando variable para almacenar valor de tarifa a utilizar
                                                    decimal valor_unitario_tarifa = 0, cantidad_tarifa = 0;
                                                    int id_tipo_pago_tarifa = 0;

                                                    //Se determina el tipo de tarifa por aplicar (fija o variable)
                                                    //Si existen detalles de matriz (variable)
                                                    if (tarifa.filtro_col != TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica || tarifa.filtro_row != TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica)
                                                    {
                                                        //Declarando variables necesarias para almacenar criterios de búsqueda en matríz (valores columna / fila)
                                                        string descipcion_col = "", descipcion_fila = "", operador_col = "=", operador_row = "=";

                                                        //Extrayendo los datos requeridos del servicio, para realizar búsqueda sobre la matriz de la tarifa
                                                        resultado = servicio.ExtraeCriteriosMatrizTarifaPago(tarifa.filtro_col, tarifa.filtro_row, tarifa.id_base_tarifa, out descipcion_col, out descipcion_fila, out operador_col, out operador_row);

                                                        //Si no hay errores en recuperación de criterios de búsqueda
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Localizando valor en matríz mediante los filtros encontrados
                                                            using (TarifasPago.TarifaMatriz detalle_tarifa = TarifasPago.TarifaMatriz.ObtieneDetalleMatrizTarifa(tarifa.id_tarifa, descipcion_col, descipcion_fila, operador_col, operador_row))
                                                            {
                                                                //Si fue encontrado un elemento en la matríz de tarifa
                                                                if (detalle_tarifa.habilitar)
                                                                    //Recuperando valor de matriz acorde a estatus
                                                                    valor_unitario_tarifa = detalle_tarifa.tarifa_cargado;
                                                                //Si no hay coincidencia en la matríz
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("La tarifa '{0}' no poseé una coincidencia '{1}' - '{2}' para este servicio.", tarifa.descripcion, tarifa.filtro_row.ToString(), tarifa.filtro_col.ToString())); ;
                                                            }
                                                        }
                                                    }
                                                    //Si no hay matriz configurada
                                                    else
                                                        //Se considera el valor general de encabezado de tarifa
                                                        valor_unitario_tarifa = tarifa.valor_unitario;

                                                    //Si no hay errores de obtención de tarifa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Calculando la cantidad sobre la que aplicará el valor unitario de tarifa
                                                        cantidad_tarifa = servicio.ExtraeCriterioBaseTarifaPago(tarifa.id_base_tarifa, out id_tipo_pago_tarifa);

                                                        //Instanciando tipo de pago principal
                                                        using (TipoPago tp = new TipoPago(id_tipo_pago_tarifa))
                                                        {
                                                            //Validando existencia de tipo de pago
                                                            if (tp.id_tipo_pago > 0)
                                                            {
                                                                //Instanciando Origen y Destino de servicio
                                                                using (Global.Ubicacion origen = new Global.Ubicacion(servicio.id_ubicacion_carga), destino = new Global.Ubicacion(servicio.id_ubicacion_descarga))
                                                                {
                                                                    //Insertando pago principal de de tarifa
                                                                    resultado = InsertaPago(tp.id_tipo_pago, id_tarifa, string.Format("{0} - {1}", origen.descripcion, destino.descripcion), string.Format("Servicio '{0}'", servicio.no_servicio), id_unidad, id_operador, id_transportista,
                                                                                            servicio.id_servicio, 0, id_liquidacion, cantidad_tarifa, tp.id_unidad_medida, valor_unitario_tarifa, id_usuario);
                                                                }

                                                                //Si se registró correctamente el pago
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Conservando id de pago
                                                                    int id_pago = resultado.IdRegistro;

                                                                    //Para cada uno de los movimientos encontrados
                                                                    foreach (DataRow m in mitMovimientos.Rows)
                                                                    {
                                                                        //Insertando relación del pago con el movimiento
                                                                        resultado = PagoMovimiento.InsertaPagoMovimiento(id_pago, m.Field<int>("Id"), id_usuario);

                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                            //De lo contrario
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("El tipo de pago base para la tarifa '{0}' no pudo ser encontrado, favor de revisar la configuración.", tarifa.descripcion));
                                                        }
                                                    }
                                                }
                                                else
                                                    resultado = new RetornoOperacion("La información del servicio no pudo ser recuperada.");
                                            }

                                            break;
                                        case TarifasPago.Tarifa.NivelPago.Movimiento:

                                            //Para cada uno de los movimientos encontrados
                                            foreach (DataRow m in mitMovimientos.Rows)
                                            {
                                                //Realizando registro de pago para el movimiento
                                                resultado = AplicaTarifaPagoMovimiento(id_liquidacion, id_tarifa, m.Field<int>("Id"), id_unidad, id_operador, id_transportista, id_usuario);

                                                //Si hay algún error
                                                if (!resultado.OperacionExitosa)
                                                {
                                                    resultado = new RetornoOperacion(string.Format("Error en pago de Mov. '{0}': {1}", m.Field<int>("Id"), resultado.Mensaje));
                                                    //Interrumpiando ciclo
                                                    break;
                                                }
                                            }

                                            break;
                                    }
                                }
                                else
                                    resultado = new RetornoOperacion(string.Format("Error al recuperar los datos de la tarifa Id '{0}'", id_tarifa));
                            }
                        }
                        //Si no hay movimientos pendientes
                        else
                            resultado = new RetornoOperacion("No existen movimientos pendientes por liquidar en este servicio.");
                    }
                }
                //Si es una Tarifa Principal
                else
                {
                    //Obteniendo conjunto de movimientos asociados al servicio, cuya asignación pertenece al mismo recurso (id_unidad, id_operador o transportista)
                    using (DataTable mitMovimientos = Despacho.Movimiento.ObtieneMovimientosSinPagoServcioRecurso(id_servicio, tipo_recurso, id_recurso))
                    { 
                        //Validando que existan movimientos pendientes
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(mitMovimientos))
                        {
                            //Instanciando la Tarifa correspondiente
                            using (TarifasPago.Tarifa tarifa = new TarifasPago.Tarifa(id_tarifa))
                            {
                                //Si la tarifa existe
                                if (tarifa.habilitar)
                                {
                                    //En base al nivel de aplicación de la tarifa
                                    switch (tarifa.nivel_pago)
                                    {
                                        case TarifasPago.Tarifa.NivelPago.Servicio:

                                            //Instanciando servicio
                                            using (Documentacion.Servicio servicio = new Documentacion.Servicio(id_servicio))
                                            {
                                                //Si el servicio existe
                                                if (servicio.habilitar)
                                                {
                                                    //Declarando variable para almacenar valor de tarifa a utilizar
                                                    decimal valor_unitario_tarifa = 0, cantidad_tarifa = 0;
                                                    int id_tipo_pago_tarifa = 0;

                                                    //Se determina el tipo de tarifa por aplicar (fija o variable)
                                                    //Si existen detalles de matriz (variable)
                                                    if (tarifa.filtro_col != TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica || tarifa.filtro_row != TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica)
                                                    {
                                                        //Declarando variables necesarias para almacenar criterios de búsqueda en matríz (valores columna / fila)
                                                        string descipcion_col = "", descipcion_fila = "", operador_col = "=", operador_row = "=";

                                                        //Extrayendo los datos requeridos del servicio, para realizar búsqueda sobre la matriz de la tarifa
                                                        resultado = servicio.ExtraeCriteriosMatrizTarifaPago(tarifa.filtro_col, tarifa.filtro_row, tarifa.id_base_tarifa, out descipcion_col, out descipcion_fila, out operador_col, out operador_row);

                                                        //Si no hay errores en recuperación de criterios de búsqueda
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Localizando valor en matríz mediante los filtros encontrados
                                                            using (TarifasPago.TarifaMatriz detalle_tarifa = TarifasPago.TarifaMatriz.ObtieneDetalleMatrizTarifa(tarifa.id_tarifa, descipcion_col, descipcion_fila, operador_col, operador_row))
                                                            {
                                                                //Si fue encontrado un elemento en la matríz de tarifa
                                                                if (detalle_tarifa.habilitar)
                                                                    //Recuperando valor de matriz acorde a estatus
                                                                    valor_unitario_tarifa = detalle_tarifa.tarifa_cargado;
                                                                //Si no hay coincidencia en la matríz
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("La tarifa '{0}' no poseé una coincidencia '{1}' - '{2}' para este servicio.", tarifa.descripcion, tarifa.filtro_row.ToString(), tarifa.filtro_col.ToString())); ;
                                                            }
                                                        }
                                                    }
                                                    //Si no hay matriz configurada
                                                    else
                                                        //Se considera el valor general de encabezado de tarifa
                                                        valor_unitario_tarifa = tarifa.valor_unitario;

                                                    //Si no hay errores de obtención de tarifa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Calculando la cantidad sobre la que aplicará el valor unitario de tarifa
                                                        cantidad_tarifa = servicio.ExtraeCriterioBaseTarifaPago(tarifa.id_base_tarifa, out id_tipo_pago_tarifa);

                                                        //Instanciando tipo de pago principal
                                                        using (TipoPago tp = new TipoPago(id_tipo_pago_tarifa))
                                                        {
                                                            //Validando existencia de tipo de pago
                                                            if (tp.id_tipo_pago > 0)
                                                            {
                                                                //Instanciando Origen y Destino de servicio
                                                                using (Global.Ubicacion origen = new Global.Ubicacion(servicio.id_ubicacion_carga), destino = new Global.Ubicacion(servicio.id_ubicacion_descarga))
                                                                {
                                                                    //Insertando pago principal de de tarifa
                                                                    resultado = InsertaPago(tp.id_tipo_pago, id_tarifa, string.Format("{0} - {1}", origen.descripcion, destino.descripcion), string.Format("Servicio '{0}'", servicio.no_servicio), id_unidad, id_operador, id_transportista,
                                                                                            servicio.id_servicio, 0, id_liquidacion, cantidad_tarifa, tp.id_unidad_medida, valor_unitario_tarifa, id_usuario);
                                                                }

                                                                //Si se registró correctamente el pago
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Conservando id de pago
                                                                    int id_pago = resultado.IdRegistro;

                                                                    //Para cada uno de los movimientos encontrados
                                                                    foreach (DataRow m in mitMovimientos.Rows)
                                                                    {
                                                                        //Insertando relación del pago con el movimiento
                                                                        resultado = PagoMovimiento.InsertaPagoMovimiento(id_pago, m.Field<int>("Id"), id_usuario);

                                                                        //Si hay error
                                                                        if (!resultado.OperacionExitosa)
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                            //De lo contrario
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("El tipo de pago base para la tarifa '{0}' no pudo ser encontrado, favor de revisar la configuración.", tarifa.descripcion));
                                                        }
                                                    }
                                                }
                                                else
                                                    resultado = new RetornoOperacion("La información del servicio no pudo ser recuperada.");
                                            }

                                            break;
                                        case TarifasPago.Tarifa.NivelPago.Movimiento:

                                            //Para cada uno de los movimientos encontrados
                                            foreach (DataRow m in mitMovimientos.Rows)
                                            {
                                                //Realizando registro de pago para el movimiento
                                                resultado = AplicaTarifaPagoMovimiento(id_liquidacion, id_tarifa, m.Field<int>("Id"), id_unidad, id_operador, id_transportista, id_usuario);

                                                //Si hay algún error
                                                if (!resultado.OperacionExitosa)
                                                {
                                                    resultado = new RetornoOperacion(string.Format("Error en pago de Mov. '{0}': {1}", m.Field<int>("Id"), resultado.Mensaje));
                                                    //Interrumpiando ciclo
                                                    break;
                                                }
                                            }

                                            break;
                                    }
                                }
                                else
                                    resultado = new RetornoOperacion(string.Format("Error al recuperar los datos de la tarifa Id '{0}'", id_tarifa));
                            }
                        }
                        //Si no hay movimientos pendientes
                        else
                            resultado = new RetornoOperacion("No existen movimientos pendientes por liquidar en este servicio.");
                    }
                }


                //Si no hay errores, se confirma la transacción
                if (resultado.OperacionExitosa)
                {
                    resultado = new RetornoOperacion(id_servicio);
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Obtener los Pagos de la Liquidación que no Pertenecen a un Servicio y/o Movimiento
        /// </summary>
        /// <param name="id_liquidacion">Liquidación</param>
        /// <returns></returns>
        public static DataTable ObtienePagosLiquidacion(int id_liquidacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtPagos = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, id_liquidacion, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtPagos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtPagos;
        }
        /// <summary>
        /// Método encargado de Validar si el Servicio Posee Pagos Aplicados.
        /// </summary>
        /// <param name="id_servicio">Servicio a Validar</param>
        /// <returns></returns>
        public static bool ValidaPagoServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 6, id_servicio, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignamos Valor
                    result = true;
            }

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar si el Servicio Posee Pagos Aplicados de Forma general (Solo Servicio).
        /// </summary>
        /// <param name="id_servicio">Servicio a Validar</param>
        /// <returns></returns>
        public static bool ValidaPagoServicioGeneral(int id_servicio)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 7, id_servicio, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignamos Valor
                    result = true;
            }

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar si el Servicio Posee Pagos Aplicados.
        /// </summary>
        /// <param name="id_servicio">Servicio a Validar</param>
        /// <param name="id_recurso">Recurso Asignado al Viaje</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación</param>
        /// <returns></returns>
        public static bool ValidaPagoServicio(int id_servicio, int id_recurso, int id_tipo_asignacion)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 8, id_servicio, id_tipo_asignacion, id_recurso, 0, 0, 0, false, "", "" };

            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignamos Valor
                    result = true;
            }

            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion
    }
}
