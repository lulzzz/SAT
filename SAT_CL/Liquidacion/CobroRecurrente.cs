using SAT_CL.EgresoServicio;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Clase encargada de todas la Operaciones relacionadas con los Cobros Recurrentes
    /// </summary>
    public class CobroRecurrente : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Tipo de Entidad a la que se aplica el Cobro
        /// </summary>
        public enum TipoEntidadAplicación
        {   
            /// <summary>
            /// Expresa si el tipo de entidad es un Operador
            /// </summary>
            Unidad = 1,
            /// <summary>
            /// Expresa si el tipo de entidad es una Unidad
            /// </summary>
            Operador,
            /// <summary>
            /// Expresa si el tipo de entidad es un Proveedor
            /// </summary>
            Proveedor,
            /// <summary>
            /// Expresa si el tipo de entidad es un Empleado
            /// </summary>
            Empleado
        }
        /// <summary>
        /// Enumeración que expresa el Estatus de Termino del Cobro
        /// </summary>
        public enum EstatusTermino
        {
            /// <summary>
            /// Expresa que el Estatus del Cobro sigue Vigente
            /// </summary>
            Vigente = 1,
            /// <summary>
            /// 
            /// </summary>
            Terminado,
            /// <summary>
            /// 
            /// </summary>
            Pausa
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_cobro_recurrente_tcr";

        private int _id_cobro_recurrente;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Cobro Recurrente
        /// </summary>
        public int id_cobro_recurrente { get { return this._id_cobro_recurrente; } }
        private int _id_tipo_cobro_recurrente;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Cobro Recurrente
        /// </summary>
        public int id_tipo_cobro_recurrente { get { return this._id_tipo_cobro_recurrente; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Compania Emisor
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private decimal _total_deuda;
        /// <summary>
        /// Atributo encargado de Almacenar el Total de la Deuda
        /// </summary>
        public decimal total_deuda { get { return this._total_deuda; } }
        private decimal _total_cobrado;
        /// <summary>
        /// Atributo encargado de Almacenar el Total Cobrado
        /// </summary>
        public decimal total_cobrado { get { return this._total_cobrado; } }
        private decimal _saldo;
        /// <summary>
        /// Atributo encargado de Almacenar el Saldo
        /// </summary>
        public decimal saldo { get { return this._saldo; } }
        private decimal _monto_cobro;
        /// <summary>
        /// Atributo encargado de Almacenar el Monto de Cobro
        /// </summary>
        public decimal monto_cobro { get { return this._monto_cobro; } }
        private decimal _tasa_monto;
        /// <summary>
        /// Atributo encargado de Almacenar la Tasa del Monto de Cobro
        /// </summary>
        public decimal tasa_monto { get { return this._tasa_monto; } }
        private int _dias_cobro;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Cobro Recurrente
        /// </summary>
        public int dias_cobro { get { return this._dias_cobro; } }
        private byte _id_tipo_entidad_aplicacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Entidad de la Aplicación
        /// </summary>
        public byte id_tipo_entidad_aplicacion { get { return this._id_tipo_entidad_aplicacion; } }
        private int _id_unidad;
        /// <summary>
        /// Atributo encargado de Almacenar la Unidad
        /// </summary>
        public int id_unidad { get { return this._id_unidad; } }
        private int _id_operador;
        /// <summary>
        /// Atributo encargado de Almacenar el Operador
        /// </summary>
        public int id_operador { get { return this._id_operador; } }
        private int _id_proveedor_compania;
        /// <summary>
        /// Atributo encargado de Almacenar el Proveedor
        /// </summary>
        public int id_proveedor_compania { get { return this._id_proveedor_compania; } }
        private int _id_empleado;
        /// <summary>
        /// Atributo encargado de Almacenar el Empleado
        /// </summary>
        public int id_empleado { get { return this._id_empleado; } }
        private string _referencia;
        /// <summary>
        /// Atributo encargado de Almacenar la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private DateTime _fecha_inicial;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha Inicial
        /// </summary>
        public DateTime fecha_inicial { get { return this._fecha_inicial; } }
        private DateTime _fecha_ultimo_cobro;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha del Ultimo Cobro
        /// </summary>
        public DateTime fecha_ultimo_cobro { get { return this._fecha_ultimo_cobro; } }
        private byte _id_estatus_termino;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus de Termino del Cobro
        /// </summary>
        public byte id_estatus_termino { get { return this._id_estatus_termino; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de Almacenar la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Atributo encargado de Almacenar el Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private DetalleLiquidacion _objDetalleLiquidacionCR;
        /// <summary>
        /// Atributo encargado de Almacenar el Detalle de la Liquidación del Cobro Recurrente
        /// </summary>
        public DetalleLiquidacion objDetalleLiquidacionCR { get { return this._objDetalleLiquidacionCR; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public CobroRecurrente()
        {   //Asignando Valores
            this._id_cobro_recurrente = 0;
            this._id_tipo_cobro_recurrente = 0;
            this._id_compania_emisor = 0;
            this._total_deuda = 0;
            this._total_cobrado = 0;
            this._saldo = 0;
            this._monto_cobro = 0;
            this._tasa_monto = 0;
            this._dias_cobro = 0;
            this._id_tipo_entidad_aplicacion = 0;
            this._id_unidad = 0;
            this._id_operador = 0;
            this._id_proveedor_compania = 0;
            this._id_empleado = 0;
            this._referencia = "";
            this._fecha_inicial = DateTime.MinValue;
            this._fecha_ultimo_cobro = DateTime.MinValue;
            this._id_estatus_termino = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._habilitar = false;
            this._objDetalleLiquidacionCR = new DetalleLiquidacion();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_cobro_recurrente">Id de Cobro Recurrente</param>
        public CobroRecurrente(int id_cobro_recurrente)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_cobro_recurrente);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~CobroRecurrente()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encaragdo de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_cobro_recurrente">Id de Cobro Recurrente</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_cobro_recurrente)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_cobro_recurrente, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", null, null, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_cobro_recurrente = id_cobro_recurrente;
                        this._id_tipo_cobro_recurrente = Convert.ToInt32(dr["IdTipoCobroRecurrente"]);
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._total_deuda = Convert.ToDecimal(dr["TotalDeuda"]);
                        this._total_cobrado = Convert.ToDecimal(dr["TotalCobrado"]);
                        this._saldo = Convert.ToDecimal(dr["Saldo"]);
                        this._monto_cobro = Convert.ToDecimal(dr["MontoCobro"]);
                        this._tasa_monto = Convert.ToDecimal(dr["TasaMontoCobro"]);
                        this._dias_cobro = Convert.ToInt32(dr["DiasCobro"]);
                        this._id_tipo_entidad_aplicacion = Convert.ToByte(dr["IdTipoEntidadAplicacion"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._id_proveedor_compania = Convert.ToInt32(dr["IdProveedorCompania"]);
                        this._id_empleado = Convert.ToInt32(dr["IdEmpleado"]);
                        this._referencia = dr["Referencia"].ToString();
                        DateTime.TryParse(dr["FechaInicial"].ToString(), out this._fecha_inicial);
                        DateTime.TryParse(dr["FechaUltimoCobro"].ToString(), out this._fecha_ultimo_cobro);
                        this._id_estatus_termino = Convert.ToByte(dr["IdEstatusTermino"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._objDetalleLiquidacionCR = new DetalleLiquidacion(id_cobro_recurrente, 77);
                    }
                }
                //Asignando Resultado Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros
        /// </summary>
        /// <param name="id_tipo_cobro_recurrente">Tipo de Cobro Recurrente</param>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <param name="total_deuda">Total de la Deuda</param>
        /// <param name="total_cobrado">Total Cobrado</param>
        /// <param name="monto_cobro">Monto del Cobro</param>
        /// <param name="tasa_monto">Tasa del Monto de Cobro</param>
        /// <param name="dias_cobro">Dias de Cobro</param>
        /// <param name="id_tipo_entidad_aplicacion">Tipo de la Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor_compania">Proveedor</param>
        /// <param name="id_empleado">Empleado</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="fecha_inicial">Fecha Inicial</param>
        /// <param name="fecha_ultimo_cobro">Fecha Ultima de Cobro</param>
        /// <param name="id_estatus_termino">Estatus de Termino</param>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_tipo_cobro_recurrente, int id_compania_emisor, decimal total_deuda, decimal total_cobrado, 
                                            decimal monto_cobro, decimal tasa_monto, int dias_cobro, byte id_tipo_entidad_aplicacion, int id_unidad, int id_operador, 
                                            int id_proveedor_compania, int id_empleado, string referencia, DateTime fecha_inicial, DateTime fecha_ultimo_cobro, 
                                            byte id_estatus_termino, int id_tabla, int id_registro, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_cobro_recurrente, id_tipo_cobro_recurrente, id_compania_emisor, total_deuda, total_cobrado, monto_cobro, tasa_monto,
                               dias_cobro, id_tipo_entidad_aplicacion, id_unidad, id_operador, id_proveedor_compania, id_empleado, referencia, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicial), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_ultimo_cobro), 
                               id_estatus_termino, id_tabla, id_registro, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Cobros Recurrentes
        /// </summary>
        /// <param name="id_tipo_cobro_recurrente">Tipo de Cobro Recurrente</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="total_deuda">Total de la Deuda</param>
        /// <param name="total_cobrado">Total Cobrado</param>
        /// <param name="monto_cobro">Monto del Cobro</param>
        /// <param name="tasa_monto">Tasa del Monto de Cobro</param>
        /// <param name="dias_cobro">Dias de Cobro</param>
        /// <param name="id_tipo_entidad_aplicacion">Tipo de la Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor_compania">Proveedor</param>
        /// <param name="id_empleado">Empleado</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="fecha_inicial">Fecha Inicial</param>
        /// <param name="fecha_ultimo_cobro">Fecha Ultima de Cobro</param>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCobroRecurrente(int id_tipo_cobro_recurrente, int id_compania_emisor, decimal total_deuda, decimal total_cobrado,
                                            decimal monto_cobro, decimal tasa_monto, int dias_cobro, byte id_tipo_entidad_aplicacion, int id_unidad, int id_operador, 
                                            int id_proveedor_compania, int id_empleado, string referencia, DateTime fecha_inicial, DateTime fecha_ultimo_cobro,
                                            byte id_estatus_termino, int id_tabla, int id_registro, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tipo_cobro_recurrente, id_compania_emisor, total_deuda, total_cobrado, monto_cobro, tasa_monto, 
                               dias_cobro, id_tipo_entidad_aplicacion, id_unidad, id_operador, id_proveedor_compania, id_empleado, referencia, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicial), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_ultimo_cobro), 
                               id_estatus_termino, id_tabla, id_registro, id_usuario, true, "", "" };
            //Obteniendo Resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Cobros Recurrentes
        /// </summary>
        /// <param name="id_tipo_cobro_recurrente">Tipo de Cobro Recurrente</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="total_deuda">Total de la Deuda</param>
        /// <param name="total_cobrado">Total Cobrado</param>
        /// <param name="monto_cobro">Monto del Cobro</param>
        /// <param name="tasa_monto">Tasa del Monto de Cobro</param>
        /// <param name="dias_cobro">Dias de Cobro</param>
        /// <param name="id_tipo_entidad_aplicacion">Tipo de la Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor_compania">Proveedor</param>
        /// <param name="id_empleado">Empleado</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="fecha_inicial">Fecha Inicial</param>
        /// <param name="fecha_ultimo_cobro">Fecha Ultima de Cobro</param>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaCobroRecurrente(int id_tipo_cobro_recurrente, int id_compania_emisor, decimal total_deuda, decimal total_cobrado, 
                                            decimal monto_cobro, decimal tasa_monto, int dias_cobro, byte id_tipo_entidad_aplicacion, int id_unidad, int id_operador, 
                                            int id_proveedor_compania, int id_empleado, string referencia, DateTime fecha_inicial, DateTime fecha_ultimo_cobro,
                                            byte id_estatus_termino, int id_tabla, int id_registro, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tipo_cobro_recurrente, id_compania_emisor, total_deuda, total_cobrado, monto_cobro, tasa_monto,
                               dias_cobro, id_tipo_entidad_aplicacion, id_unidad, id_operador, id_proveedor_compania, id_empleado, referencia,
                               fecha_inicial, fecha_ultimo_cobro, id_estatus_termino, id_tabla, id_registro, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Cobros Recurrentes
        /// </summary>
        /// <param name="id_usuario">id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCobroRecurrente(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tipo_cobro_recurrente, this._id_compania_emisor, this._total_deuda, this._total_cobrado, this._monto_cobro, this._tasa_monto,
                               this._dias_cobro, this._id_tipo_entidad_aplicacion, this._id_unidad, this._id_operador, this._id_proveedor_compania, this._id_empleado, this._referencia,
                               this._fecha_inicial, this._fecha_ultimo_cobro, this._id_estatus_termino, this._id_tabla, this._id_registro, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus de Termino del Cobro Recurrente
        /// </summary>
        /// <param name="estatus">Estatus Deseado</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusTerminoCobroRecurrente(EstatusTermino estatus, int id_usuario)
        {   
            //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tipo_cobro_recurrente, this._id_compania_emisor, this._total_deuda, this._total_cobrado, this._monto_cobro, this._tasa_monto,
                               this._dias_cobro, this._id_tipo_entidad_aplicacion, this._id_unidad, this._id_operador, this._id_proveedor_compania, this._id_empleado, this._referencia,
                               this._fecha_inicial, this._fecha_ultimo_cobro, (byte)estatus, this._id_tabla, this._id_registro, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos del Cobro Recurrente
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCobroRecurrente()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_cobro_recurrente);
        }
        /// <summary>
        /// Método Público encargado de Calcular el Saldo actual calculando el monto dado con respecto a la tasa
        /// </summary>
        /// <param name="monto">Monto sin Cobros</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public decimal CalculaSaldoActual(decimal monto, DateTime fecha)
        {   //Declarando Objeto de Retorno
            decimal saldo_actual = 0;
            //Obtiene la Fecha de Actualización
            DateTime fecha_actualizacion = this._fecha_ultimo_cobro == DateTime.MinValue ? this._fecha_inicial : this._fecha_ultimo_cobro;
            //Resta de Fechas
            TimeSpan ts = fecha - fecha_actualizacion;
            //Validando que exista un Tasa
            if (this._tasa_monto > 0)
                //Calculando el Saldo Actual
                saldo_actual = (ts.Days / this._dias_cobro) * (monto * (this.tasa_monto / 100));
            else//Calculando Saldo Actual
                saldo_actual = (ts.Days / this._dias_cobro) * this._monto_cobro;
            //Devolviendo Ressultado Obtenido
            return saldo_actual;
        }
        /// <summary>
        /// Método Público encargado de Calcular el Saldo Actual
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public decimal CalculaSaldoActual(DateTime fecha)
        {   
            //Declarando Objeto de Retorno
            decimal saldo_actual = 0;
            
            //Obtiene la Fecha de Actualización
            DateTime fecha_actualizacion = this._fecha_ultimo_cobro == DateTime.MinValue ? this._fecha_inicial : this._fecha_ultimo_cobro;
            
            //Resta de Fechas
            TimeSpan ts = fecha - fecha_actualizacion;
            
            //Calculando Saldo Actual
            saldo_actual = (ts.Days / this._dias_cobro) * this._monto_cobro;
            
            //Instanciando Cobro Recurrente
            using(TipoCobroRecurrente tcr = new TipoCobroRecurrente(this._id_tipo_cobro_recurrente))
            {
                //Validando el Tipo de Cobro
                if((TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == TipoCobroRecurrente.TipoAplicacion.Descuento)

                    //Validando si el Saldo Actual es mayor que el calculado
                    saldo_actual = saldo_actual > this._saldo ? this._saldo : saldo_actual;
            }
            
            //Devolviendo Ressultado Obtenido
            return saldo_actual;
        }
        /// <summary>
        /// Método Público encaragdo de Actualizar el Cobro Recurrente
        /// </summary>
        /// <param name="id_liquidacion"></param>
        /// <param name="fecha_calculo"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTotalCobroRecurrente(int id_liquidacion, DateTime fecha_calculo, int id_unidad, int id_operador, 
                                                              int id_proveedor, int id_usuario)
        {   
            //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion();
            RetornoOperacion resultDetalleLiq = new RetornoOperacion();
            
            //Declarando Variables Auxiliares
            decimal saldo_actual = 0.00M;
            
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Tipo de Cobro Recurrente
                using (TipoCobroRecurrente tcr = new TipoCobroRecurrente(this._id_tipo_cobro_recurrente))
                {
                    //Obteniendo Dias
                    TimeSpan dias = fecha_calculo - this._fecha_ultimo_cobro;
                    //Asignando Estatus
                    byte id_estatus = this._id_estatus_termino;

                    //Declarando Cantidad
                    decimal cantidad = 0;

                    //Validando Estatus del Cobro Recurrente
                    switch ((EstatusTermino)this._id_estatus_termino)
                    {
                        case EstatusTermino.Vigente:
                            {
                                //Obteniendo Saldo Actual
                                saldo_actual = CalculaSaldoActual(fecha_calculo);
                                
                                //Obteniendo Cantidad
                                cantidad = dias.Days / this.dias_cobro;

                                //Validando que el Tipo de Aplicación sea Descuento
                                if (tcr.id_tipo_aplicacion == 2)
                                {
                                    //Total Actual Auxiliar
                                    saldo_actual = saldo_actual >= this._saldo ? this._saldo : saldo_actual;

                                    //Obteniendo Estatus
                                    id_estatus = saldo_actual == this._saldo ? (byte)EstatusTermino.Terminado : this._id_estatus_termino;

                                    //Calculando Cantidad
                                    cantidad = Convert.ToDecimal(saldo_actual / monto_cobro);
                                }
                                break;
                            }
                        case EstatusTermino.Pausa:
                            {
                                //Asignando Valores
                                cantidad = 0;
                                id_estatus = (byte)EstatusTermino.Vigente;
                                saldo_actual = 0;
                                break;
                            }
                    }
                    
                    //Insertando Detalle de Liquidación
                    resultDetalleLiq = DetalleLiquidacion.InsertaDetalleLiquidacion(77, this._id_cobro_recurrente, DetalleLiquidacion.Estatus.Liquidado, id_unidad, id_operador,
                                        id_proveedor, 0, 0, fecha_calculo, id_liquidacion, cantidad, 0, this._monto_cobro, id_usuario);

                    //Validando si la Operación fue exitosa
                    if (resultDetalleLiq.OperacionExitosa)
                    {
                        //Instanciando Detalle de Liquidación
                        using (DetalleLiquidacion dl = new DetalleLiquidacion(resultDetalleLiq.IdRegistro))
                        {
                            //Cantidad Entera
                            int cantidad_int = Decimal.ToInt32(cantidad);
                            
                            //Validando Estatus
                            if ((EstatusTermino)this._id_estatus_termino == EstatusTermino.Pausa)
                            
                                //Obteniendo Cantidad
                                cantidad_int = dias.Days / this.dias_cobro;
                            
                            //Calculando Fecha del Ultimo Cobro con respecto a la cantidad por los dias 
                            DateTime fecha_cobro = this._fecha_ultimo_cobro.AddDays(cantidad_int * this._dias_cobro);
                            
                            //Actualizando Registro
                            result = this.actualizaRegistros(this._id_tipo_cobro_recurrente, this._id_compania_emisor, this._total_deuda, this.total_cobrado + saldo_actual,
                                        this._monto_cobro, this._tasa_monto, this._dias_cobro, this._id_tipo_entidad_aplicacion, this._id_unidad, this._id_operador,
                                        this._id_proveedor_compania, this._id_empleado, this._referencia, this._fecha_inicial, fecha_cobro,
                                        id_estatus, this._id_tabla, this._id_registro, id_usuario, this._habilitar);

                            //Validando que la Operación haya sido Completada
                            if (result.OperacionExitosa)
                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion(resultDetalleLiq.Mensaje);
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Regresar el Cobro Recurrente en caso de Reabrir la Liquidación
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion RegresaTotalCobroRecurrente(int id_usuario)
        {
            //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion();
            RetornoOperacion resultDetalleLiq = new RetornoOperacion();

            //Declarando Variables Auxiliares
            DateTime fecha_cobro_anterior = DateTime.MinValue;
            decimal total = 0;

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo Cantidad
                int cantidad = 0;
                //Obteniendo Estatus
                byte id_estatus = this._total_deuda == this._total_cobrado ? (byte)EstatusTermino.Vigente : this._id_estatus_termino;

                //Instanciando Detalle de Liquidación
                using (DetalleLiquidacion dl = new DetalleLiquidacion(this._id_cobro_recurrente, 77))
                {
                    //Validando que exista el Detalle 
                    if (dl.id_detalle_liquidacion > 0)
                    {
                        //Obteniendo Cantidades
                        cantidad = Convert.ToInt32(dl.cantidad);
                        total = dl.cantidad * dl.valor_unitario;
                        
                        //Obteniendo Fecha Anterior
                        fecha_cobro_anterior = this._fecha_ultimo_cobro.AddDays(-1 * (cantidad * this._dias_cobro));
                        
                        //Deshabilitando Detalle del Cobro Recurrente
                        resultDetalleLiq = dl.DeshabilitaDetalleLiquidacion(id_usuario);
                        
                        //Validando la Eliminación del Detalle del Cobro
                        if (resultDetalleLiq.OperacionExitosa)
                        {
                            //Instanciando Cobro Recurrente
                            using (TipoCobroRecurrente tcr = new TipoCobroRecurrente(this._id_tipo_cobro_recurrente))
                            {
                                //Actualizando Registro
                                result = this.actualizaRegistros(this._id_tipo_cobro_recurrente, this._id_compania_emisor, this._total_deuda, this.total_cobrado - total,
                                            this._monto_cobro, this._tasa_monto, this._dias_cobro, this._id_tipo_entidad_aplicacion, this._id_unidad, this._id_operador,
                                            this._id_proveedor_compania, this._id_empleado, this._referencia, this._fecha_inicial, fecha_cobro_anterior,
                                            id_estatus, this._id_tabla, this._id_registro, id_usuario, this._habilitar);

                                //Validando que la Operación haya sido Completada
                                if (result.OperacionExitosa)
                                    //Completando Transacción
                                    trans.Complete();
                            }
                        }
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }


        /// <summary>
        /// Método Público encargado de Actualizar los Cobros Recurrentes
        /// </summary>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_entidad"></param>
        /// <param name="tipo_entidad"></param>
        /// <param name="id_liquidacion"></param>
        /// <param name="fecha_calculo"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static string[] ActualizaDetalleCobrosRecurrentes(int id_compania_emisor, int id_entidad, TipoEntidadAplicación tipo_entidad, 
                                                                 int id_liquidacion, DateTime fecha_calculo, int id_usuario)
        {   //Declarando Objeto del Retorno
            string[] results;
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, id_compania_emisor, 0, 0, 0, 0, 0, (byte)tipo_entidad, id_entidad, 0, 0, 0, "", 
                               null, null, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan 
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Asignando Limite Dinamico del Array
                    results = new string[ds.Tables["Table"].Rows.Count];
                    //Declarando Variable Auxiliar
                    int contador = 0;
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Instanciando Cobro Recurrente
                        using(CobroRecurrente cr = new CobroRecurrente(Convert.ToInt32(dr["Id"])))
                        {   //Obteniendo Entidades
                            int id_operador = tipo_entidad == TipoEntidadAplicación.Operador ? id_entidad : 0;
                            int id_proveedor = tipo_entidad == TipoEntidadAplicación.Proveedor ? id_entidad : 0;
                            int id_unidad = tipo_entidad == TipoEntidadAplicación.Unidad ? id_entidad : 0;
                            int id_empleado = tipo_entidad == TipoEntidadAplicación.Empleado ? id_entidad : 0;
                            //Invocando Método de Actualización del Cobro
                            result = cr.ActualizaTotalCobroRecurrente(id_liquidacion, fecha_calculo, id_unidad, id_operador, id_proveedor, id_usuario);
                            //Asignando Mensaje de Operación
                            results[contador] = result.IdRegistro.ToString() + ". " + result.Mensaje;
                            //Incrementando Contador
                            contador++;
                        }
                    }
                }
                else
                {   //Asignando Limite de Resultados
                    results = new string[1];
                    //Asignando Mensaje de Error
                    results[1] = "No se encontraron registros";
                }
            }
            //Devolviendo Resultado Obtenido
            return results;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Cobros Recurrentes de la Entidad
        /// </summary>
        /// <param name="id_tipo_entidad">Tipo de Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <returns></returns>
        public static DataTable ObtieneCobrosRecurrentesEntidad(byte id_tipo_entidad, int id_unidad, int id_operador, int id_proveedor, 
                                                int id_compania_emisor, DateTime fecha_liquidacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtCobros = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, id_compania_emisor, 0, 0, 0, 0, 0, id_tipo_entidad, id_unidad, id_operador, id_proveedor, 0, "", 
                               fecha_liquidacion, null, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Cargos Recurrentes
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtCobros = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtCobros;
        }
        /// <summary>
        /// Método Público encargado de Obtener Todos los Cobros Recurrentes que ha Tenido la Entidad
        /// </summary>
        /// <param name="id_cobro_recurrente">Referencia al Cobro Recurrente</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneHistorialCobrosRecurrentes(int id_cobro_recurrente, byte id_tipo_entidad, int id_unidad, int id_operador, int id_proveedor,
                                                int id_compania_emisor)
        {
            //Declarando Objeto de Retorno
            DataTable dtCobros = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, id_cobro_recurrente, 0, id_compania_emisor, 0, 0, 0, 0, 0, id_tipo_entidad, id_unidad, id_operador, id_proveedor, 0, "", 
                               null, null, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Cargos Recurrentes
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtCobros = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtCobros;
        }
        /// <summary>
        /// Método Público encargado de Obtener Todos los Cobros Recurrentes que ha Tenido la Entidad dada una Liquidación
        /// </summary>
        /// <param name="id_liquidacion">Referencia a la Liquidación Actual</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneCobrosRecurrentesTotales(int id_liquidacion, byte id_tipo_entidad, int id_unidad, int id_operador, int id_proveedor,
                                                int id_compania_emisor)
        {
            //Declarando Objeto de Retorno
            DataTable dtCobros = null;

            //Armando Arreglo de Parametros
            object[] param = { 7, 0, 0, id_compania_emisor, 0, 0, 0, 0, 0, id_tipo_entidad, id_unidad, id_operador, id_proveedor, 0, "", 
                               null, null, 0, 0, 0, 0, false, id_liquidacion.ToString(), "" };

            //Obteniendo Cargos Recurrentes
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtCobros = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtCobros;
        }

        #endregion
    }
}
