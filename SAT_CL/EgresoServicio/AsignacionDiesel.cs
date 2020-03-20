using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Despacho;
using System.Configuration;
namespace SAT_CL.EgresoServicio
{
    /// <summary>
    /// Clase encargada de Todas las Operaciones de las Asignaciones de Diesel
    /// </summary>
    public class AsignacionDiesel : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración de Estatus
        /// </summary>
        public enum Estatus
        {   /// <summary>
            /// Estatus que Indica que los Litros del Vale estan en 0
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Estatus que Indica que los Litros del Vale son distintos de 0
            /// </summary>
            Actualizado,
            /// <summary>
            /// Estatus que Indica que el Vale ya fue facturado
            /// </summary>
            Facturado,
            /// <summary>
            /// Estatus que Indica que el Vale tiene un exceso de Litros y esta en Autorización
            /// </summary>
            EnAutorizacion,
            /// <summary>
            /// Estatus que Indica que el Vale fue Rechazado por un Usuario
            /// </summary>
            Rechazado
        }
        /// <summary>
        /// Enumeración de Tipos de Vale
        /// </summary>
        public enum TipoVale
        {   /// <summary>
            /// Tipo que Indica que el Vale es Original
            /// </summary>
            Original = 1,
            /// <summary>
            /// Tipo que Indica si existe una Diferencia de Litros con respecto a un Vale Original
            /// </summary>
            Dif_Litros,
            /// <summary>
            /// Tipo que Indica si existe una Diferencia del Monto con respecto a un Vale Original
            /// </summary>
            Dif_Monto,
            /// <summary>
            /// Tipo que Indica vale monedero
            /// </summary>
            Monedero
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "egresos_servicio.sp_asignacion_diesel_tad";

        private int _id_asignacion_diesel;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Asignación de Diesel
        /// </summary>
        public int id_asignacion_diesel { get { return this._id_asignacion_diesel; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        private int _conteo_impresion;
        /// <summary>
        /// Atributo encargado de Almacenar el Conteo de las Impresiones
        /// </summary>
        public int conteo_impresion { get { return this._conteo_impresion; } }
        private string _nombre_operador_proveedor;
        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del Operador y/ó Proveedor
        /// </summary>
        public string nombre_operador_proveedor { get { return this._nombre_operador_proveedor; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private string _no_vale;
        /// <summary>
        /// Atributo encargado de Almacenar el Número del Vale
        /// </summary>
        public string no_vale { get { return this._no_vale; } }
        private int _id_ubicacion_estacion;
        /// <summary>
        /// Atributo encargado de Almacenar la Ubicación de la Estación
        /// </summary>
        public int id_ubicacion_estacion { get { return this._id_ubicacion_estacion; } }
        private DateTime _fecha_solicitud;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Solicitud
        /// </summary>
        public DateTime fecha_solicitud { get { return this._fecha_solicitud; } }
        private DateTime _fecha_carga;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Carga
        /// </summary>
        public DateTime fecha_carga { get { return this._fecha_carga; } }
        private int _id_costo_diesel;
        /// <summary>
        /// Atributo encargado de Almacenar el Costo del Diesel
        /// </summary>
        public int id_costo_diesel { get { return this._id_costo_diesel; } }
        private byte _id_tipo_combustible;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Combustible
        /// </summary>
        public byte id_tipo_combustible { get { return this._id_tipo_combustible; } }
        private int _id_factura;
        /// <summary>
        /// Atributo encargado de Almacenar la Factura
        /// </summary>
        public int id_factura { get { return this._id_factura; } }
        private bool _bit_transferencia_contable;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus de Transferencia Contable
        /// </summary>
        public bool bit_transferencia_contable { get { return this._bit_transferencia_contable; } }
        private string _referencia;
        /// <summary>
        /// Atributo encargado de Almacenar la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private int _id_lectura;
        /// <summary>
        /// Atributo encargado de Almacenar la Lectura del Diesel
        /// </summary>
        public int id_lectura { get { return this._id_lectura; } }
        private int _id_deposito;
        /// <summary>
        /// Atributo encargado de Almacenar el Deposito
        /// </summary>
        public int id_deposito { get { return this._id_deposito; } }
        private byte _tipo_vale;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Vale
        /// </summary>
        public byte tipo_vale { get { return this._tipo_vale; } }
        /// <summary>
        /// Atributo encargado de Devolver la Enumeración con respecto al Tipo de Vale
        /// </summary>
        public TipoVale tipo_vale_enum { get { return (TipoVale)this.tipo_vale; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private DetalleLiquidacion _objDetalleLiquidacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Detalle de Liquidación
        /// </summary>
        public DetalleLiquidacion objDetalleLiquidacion { get { return this._objDetalleLiquidacion; } }
        private int _id_unidad_diesel;

        /// <summary>
        /// Atributo encargado de Almacenar el Id de Unidad a quie se le asignara el diesel
        /// </summary>
        public int id_unidad_diesel { get { return this._id_unidad_diesel; } }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que inicializa los Atributos por Defecto
        /// </summary>
        public AsignacionDiesel()
        {   //Asignando de valores
            this._id_asignacion_diesel = 0;
            this._id_estatus = 0;
            this._conteo_impresion = 0;
            this._nombre_operador_proveedor = "";
            this._id_compania_emisor = 0;
            this._no_vale = "";
            this._id_ubicacion_estacion = 0;
            this._fecha_solicitud = DateTime.MinValue;
            this._fecha_carga = DateTime.MinValue;
            this._id_costo_diesel = 0;
            this._id_tipo_combustible = 0;
            this._id_factura = 0;
            this._bit_transferencia_contable = false;
            this._referencia = "";
            this._id_lectura = 0;
            this._id_deposito = 0;
            this._tipo_vale = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_asignacion_diesel">Id de Registro</param>
        public AsignacionDiesel(int id_asignacion_diesel)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_asignacion_diesel);
        }
        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~AsignacionDiesel()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos
        /// </summary>
        /// <param name="id_asignacion_diesel">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_asignacion_diesel)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_asignacion_diesel, 0, 0, "", 0, 0, 0, null, null, 0, 0, 0, false, "", 0, 0, 0, 0, 0, false, "", "" };
            //Instanciando registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando de valores
                        this._id_asignacion_diesel = id_asignacion_diesel;
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._conteo_impresion = Convert.ToInt32(dr["ConteoImpresion"]);
                        this._nombre_operador_proveedor = dr["NombreOperadorProveedor"].ToString();
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._no_vale = dr["NoVale"].ToString();
                        this._id_ubicacion_estacion = Convert.ToInt32(dr["IdUbicacionEstacion"]);
                        DateTime.TryParse(dr["FechaSolicitud"].ToString(), out this._fecha_solicitud);
                        DateTime.TryParse(dr["FechaCarga"].ToString(), out this._fecha_carga);
                        this._id_costo_diesel = Convert.ToInt32(dr["IdCostoDiesel"]);
                        this._id_tipo_combustible = Convert.ToByte(dr["IdTipoCombustible"]);
                        this._id_factura = Convert.ToInt32(dr["IdFactura"]);
                        this._bit_transferencia_contable = Convert.ToBoolean(dr["BitTransferenciaContable"]);
                        this._referencia = dr["Referencia"].ToString();
                        this._id_lectura = Convert.ToInt32(dr["IdLectura"]);
                        this._id_deposito = Convert.ToInt32(dr["IdDeposito"]);
                        this._tipo_vale = Convert.ToByte(dr["TipoVale"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._id_unidad_diesel = Convert.ToInt32(dr["IdUnidadDiesel"]);
                        this._objDetalleLiquidacion = new DetalleLiquidacion(id_asignacion_diesel, 69);
                    }
                }
                //Asignando Retorno Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_estatus">Estatus de la Asignación</param>
        /// <param name="conteo_impresion">Conteo de Impresiones del Vale</param>
        /// <param name="nombre_operador_proveedor">nombre del Operador y/ó Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_ubicacion_estacion">Ubicación de la Estación</param>
        /// <param name="fecha_solicitud">Fecha de la Solicitud</param>
        /// <param name="fecha_carga">Fecha de Carga</param>
        /// <param name="id_costo_diesel">Costo del Diesel</param>
        /// <param name="id_factura">Factura del Vale</param>
        /// <param name="bit_transferencia_contable">Estatus de Transferencia</param>
        /// <param name="referencia">Referencia del Vale</param>
        /// <param name="id_lectura">Lectura de Diesel</param>
        /// <param name="id_deposito">Deposito del Vale</param>
        /// <param name="tipo_vale">Tipo del Vale</param>
        /// <param name="id_unidad_diesel">Unidad Diesel</param>
        /// <param name="id_usuario">Usuario que realiza la Actualización</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(byte id_estatus, int conteo_impresion, string nombre_operador_proveedor, int id_compania_emisor,
                               int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, int id_costo_diesel, byte id_tipo_combustible, int id_factura,
                               bool bit_transferencia_contable, string referencia, int id_lectura, int id_deposito, byte tipo_vale, int id_unidad_diesel, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_asignacion_diesel, id_estatus, conteo_impresion, nombre_operador_proveedor, id_compania_emisor, 
                               this._no_vale, id_ubicacion_estacion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solicitud), 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_carga), id_costo_diesel, id_tipo_combustible, id_factura, 
                               bit_transferencia_contable, referencia, id_lectura, id_deposito, tipo_vale, id_unidad_diesel, id_usuario, habilitar, "", "" };
            //Validando el Estatus del Vale
            if (this._id_estatus != (byte)Estatus.Facturado)
                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Validando si existe un incremento en la Impresión
            else if (this._conteo_impresion != conteo_impresion)
                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            else//Instanciando Excepcion del Vale Facturado
                result = new RetornoOperacion("El Vale ha sido Facturado, Imposible su Edición");
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Actualizamos los Litros de la Unidad que se añade el diesel
        /// </summary>
        /// <param name="id_unidad_disel">Id Unidad donde se asignara el Diesel</param>
        /// <param name="litros">litros a Cargar de la Unidad</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private static RetornoOperacion actualizaAsignacionDieselUnidad(int id_unidad_disel, decimal litros, int id_usuario)
        {
            //Declarando objeto de resultado 
            RetornoOperacion resultado = new RetornoOperacion();

            //Si hay unidad de permisionario
            if (id_unidad_disel > 0)
            {
                //Instanciando unidad indicada
                using (SAT_CL.Global.Unidad u = new Global.Unidad(id_unidad_disel))
                {
                    //Si la unidad existe
                    if (u.habilitar)
                        //Actualizando litros
                        resultado = u.ActualizaCombustibleAsignado(litros, id_usuario);
                    else
                        resultado = new RetornoOperacion("Unidad de permisionario no localizada.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la actualización de la cantidad de litros asignados a la unidad indicada o bien a la unidad asignada al operador
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="litros">Litros por asignar</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private static RetornoOperacion actualizaAsignacionDieselUnidad(int id_unidad, int id_operador, int id_movimiento, decimal litros, int id_usuario)
        {
            //Declarando objeto de resultado 
            RetornoOperacion resultado = new RetornoOperacion();

            //Creando bloque transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si hay unidad de permisionario
                if (id_unidad > 0)
                {
                    //Instanciando unidad indicada
                    using (SAT_CL.Global.Unidad u = new Global.Unidad(id_unidad))
                    {
                        //Si la unidad existe
                        if (u.habilitar)
                            //Actualizando litros
                            resultado = u.ActualizaCombustibleAsignado(litros, id_usuario);
                        else
                            resultado = new RetornoOperacion("Unidad de permisionario no localizada.");
                    }
                }
                //Si es operador de casa
                else
                {
                    //Obteniendo la unidad asignada al operador
                    using (SAT_CL.Global.Unidad u = new Global.Unidad(SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionUnidadPropia(id_movimiento)))
                    {
                        //Si la unidad existe
                        if (u.habilitar)
                            //Actualizando litros
                            resultado = u.ActualizaCombustibleAsignado(litros, id_usuario);
                        else
                            resultado = new RetornoOperacion("No fue posible obtener la unidad asignada al operador.");
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando cambios
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Asignaciones de Diesel
        /// </summary>
        /// <param name="nombre_operador_proveedor">nombre del Operador y/ó Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="no_vale">Número de Vale</param>
        /// <param name="id_ubicacion_estacion">Ubicación de la Estación</param>
        /// <param name="fecha_solicitud">Fecha de la Solicitud</param>
        /// <param name="fecha_carga">Fecha de Carga</param>
        /// <param name="id_costo_diesel">Costo del Diesel</param>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="id_factura">Factura del Vale</param>
        /// <param name="bit_transferencia_contable">Estatus de Transferencia</param>
        /// <param name="referencia">Referencia del Vale</param>
        /// <param name="id_lectura">Lectura de Diesel</param>
        /// <param name="id_deposito">Deposito del Vale</param>
        /// <param name="tipo_vale">Tipo del Vale</param>
        /// <param name="litros">Cantidad de Litros por Vale</param>
        /// <param name="id_unidad_diesel">Unidad Diesel</param>
        /// <param name="id_usuario">usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAsignacionDiesel(string nombre_operador_proveedor, int id_compania_emisor,
                               int no_vale, int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, int id_costo_diesel, byte id_tipo_combustible, int id_factura,
                               bool bit_transferencia_contable, string referencia, int id_lectura, int id_deposito, byte tipo_vale, decimal litros, int id_unidad_diesel, int id_usuario)
        {   //Validando Estatus de la Asignación
            byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, estatus, 0, nombre_operador_proveedor, id_compania_emisor, 
                                no_vale, id_ubicacion_estacion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solicitud), 
                                TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_carga), id_costo_diesel, id_tipo_combustible, id_factura, 
                                bit_transferencia_contable, referencia, id_lectura, id_deposito, tipo_vale, id_unidad_diesel, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        ///  Método Público encargado de Insertar las Asignaciones de Diesel y su detalle de liquidación, a la vez actualiza el combustible asignado sobre la unidad involucrada
        /// </summary>
        /// <param name="nombre_operador_proveedor">nombre del Operador y/ó Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="no_vale">Número de Vale</param>
        /// <param name="id_ubicacion_estacion">Ubicación de la Estación</param>
        /// <param name="fecha_solicitud">Fecha de la Solicitud</param>
        /// <param name="fecha_carga">Fecha de Carga</param>
        /// <param name="id_costo_diesel">Costo del Diesel</param>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="id_factura">Factura del Vale</param>
        /// <param name="bit_transferencia_contable">Estatus de Transferencia</param>
        /// <param name="referencia">Referencia del Vale</param>
        /// <param name="id_lectura">Lectura de Diesel</param>
        /// <param name="id_deposito">Deposito del Vale</param>
        /// <param name="tipo_vale">Tipo del Vale</param>
        /// <param name="litros">Cantidad de Litros por Vale</param>
        /// <param name="costo_combustible">Costo de Combustible</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_unidad_asignacion_diesel">Id de Unidad que se le Asigna el Diesel</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor_compania">Compania Proveedora</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento_asignacion">Movimiento Asignacion</param>
        /// <param name="id_usuario">usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAsignacionDiesel(string nombre_operador_proveedor, int id_compania_emisor,
                               int no_vale, int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, int id_costo_diesel,
                               byte id_tipo_combustible, int id_factura, bool bit_transferencia_contable, string referencia, int id_lectura, int id_deposito,
                               byte tipo_vale, decimal litros, decimal costo_combustible, int id_unidad, int id_unidad_asignacion_diesel,
                               int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento_asignacion, int id_usuario)
        {
            //Validando Estatus del Vale de diesel
            byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Unidad de Diesel
                using (SAT_CL.Global.Unidad objUnidadDisel = new SAT_CL.Global.Unidad(id_unidad_asignacion_diesel))
                {
                    //Validamos Unidad de Diesel
                    if (objUnidadDisel.id_unidad > 0)
                    {
                        //Validando que los Litros no excedan la capacidad del Combustible
                        if (litros <= objUnidadDisel.capacidad_combustible)
                        {
                            //Validamos el Estatus de la Asignacion
                            using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
                            {
                                //Validamos Estatus de la Asignacion
                                if (objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Iniciado ||
                                    objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Terminado)
                                {
                                    //Armando Arreglo de Parametros
                                    object[] param = { 1, 0, estatus, 0, nombre_operador_proveedor, id_compania_emisor, 
                                   no_vale, id_ubicacion_estacion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solicitud), 
                                   TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_carga), id_costo_diesel, id_tipo_combustible, id_factura, 
                                   bit_transferencia_contable, referencia, id_lectura, id_deposito, tipo_vale, id_unidad_asignacion_diesel, id_usuario, true, "", "" };

                                    //Validando que el Movimiento 
                                    if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(objMovimientoAsignacion.id_movimiento))
                                    {
                                        //Obteniendo Resultado del SP
                                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                                        //Validando que se haya Insertando
                                        if (result.OperacionExitosa)
                                        {
                                            //Recuperando id de asignación de diesel
                                            int id_asignacion_diesel = result.IdRegistro;
                                            //Actualizando datos de asignación de combustible
                                            result = actualizaAsignacionDieselUnidad(id_unidad_asignacion_diesel, litros, id_usuario);

                                            //Si no hay errores
                                            if (result.OperacionExitosa)
                                            {
                                                //Instanciando Costo Combustible
                                                using (SAT_CL.EgresoServicio.CostoCombustible cc = new SAT_CL.EgresoServicio.CostoCombustible(id_costo_diesel))
                                                {
                                                    //Validando que exista el Costo
                                                    if (cc.id_costo_combustible > 0)

                                                        //Insertando Detalle de Liquidación
                                                        result = DetalleLiquidacion.InsertaDetalleLiquidacion(69, id_asignacion_diesel, id_unidad, id_operador, id_proveedor_compania, id_servicio,
                                                                                            objMovimientoAsignacion.id_movimiento, 0, litros, 29, cc.costo_combustible, id_usuario);
                                                    else
                                                        //Insertando Detalle de Liquidación
                                                        result = DetalleLiquidacion.InsertaDetalleLiquidacion(69, id_asignacion_diesel, id_unidad, id_operador, id_proveedor_compania, id_servicio,
                                                                                            objMovimientoAsignacion.id_movimiento, 0, litros, 29, costo_combustible, id_usuario);

                                                    //Validando que se Insertara el Detalle
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Enviamos Notificación de Diesel
                                                        Global.NotificacionPush.Instance.NotificacionDepositoAnticiposYDiesel(id_movimiento_asignacion,
                                                            SAT_CL.Global.NotificacionPush.TipoNotificacionServicio.Diesel, Fecha.EsFechaMinima(fecha_carga) ? Fecha.ObtieneFechaEstandarMexicoCentro() : fecha_carga, litros);

                                                        //Asignando Vale
                                                        result = new RetornoOperacion(id_asignacion_diesel);
                                                        //Completando Transacción
                                                        trans.Complete();
                                                    }
                                                }
                                            }
                                            else
                                                result = new RetornoOperacion(string.Format("Error al actualizar combustible asignado a unidad: {0}", result.Mensaje));
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("El Movimiento se encuentra Pagado");
                                }
                                else
                                    //EStablecemos Resultado
                                    result = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite asignar combustible.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                            }
                        }
                        else
                            //Mostrando resultado
                            result = new RetornoOperacion("Los Litros asignados no deben de exceder la capacidad de la Unidad");
                    }
                    else
                        //Establecemos Mensaje Error
                        result = new RetornoOperacion("No es posible encontrar la Unidad para la Asignación de Diesel.");
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método público encargado de insertar las asignaciones de diésel que son programadas, así como su detalle de liquidación; a la vez, actualiza el combustible asignado sobre la unidad involucrada.
        /// </summary>
        /// <param name="nombre_operador_proveedor"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="no_vale"></param>
        /// <param name="id_ubicacion_estacion"></param>
        /// <param name="fecha_solicitud"></param>
        /// <param name="fecha_carga"></param>
        /// <param name="id_costo_diesel"></param>
        /// <param name="id_tipo_combustible"></param>
        /// <param name="id_factura"></param>
        /// <param name="bit_transferencia_contable"></param>
        /// <param name="referencia"></param>
        /// <param name="id_lectura"></param>
        /// <param name="id_deposito"></param>
        /// <param name="tipo_vale"></param>
        /// <param name="litros"></param>
        /// <param name="costo_combustible"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_unidad_asignacion_diesel"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_proveedor_compania"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_movimiento_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAsignacionDieselProgramado(string nombre_operador_proveedor, int id_compania_emisor,
                               int no_vale, int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, int id_costo_diesel,
                               byte id_tipo_combustible, int id_factura, bool bit_transferencia_contable, string referencia, int id_lectura, int id_deposito,
                               byte tipo_vale, decimal litros, decimal costo_combustible, int id_unidad, int id_unidad_asignacion_diesel,
                               int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento_asignacion, int id_usuario)
        {
            //Validando Estatus del Vale de diesel
            byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Unidad de Diesel
                using (SAT_CL.Global.Unidad objUnidadDisel = new SAT_CL.Global.Unidad(id_unidad_asignacion_diesel))
                {
                    //Validamos Unidad de Diesel
                    if (objUnidadDisel.id_unidad > 0)
                    {
                        //Validando que los Litros no excedan la capacidad del Combustible
                        bool val = (TipoVale)tipo_vale == TipoVale.Monedero ? true : litros <= objUnidadDisel.capacidad_combustible;
                        if (val)
                        {
                            //Validamos el Estatus de la Asignacion
                            using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
                            {
                                //Validamos Estatus de la Asignacion
                                if (objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Registrado ||
                                    objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Iniciado ||
                                    objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Terminado)
                                {
                                    //Armando Arreglo de Parametros
                                    object[] param = { 1, 0, estatus, 0, nombre_operador_proveedor, id_compania_emisor,
                                   no_vale, id_ubicacion_estacion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solicitud),
                                   TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_carga), id_costo_diesel, id_tipo_combustible, id_factura,
                                   bit_transferencia_contable, referencia, id_lectura, id_deposito, tipo_vale, id_unidad_asignacion_diesel, id_usuario, true, "", "" };

                                    //Validando que el Movimiento 
                                    if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(objMovimientoAsignacion.id_movimiento))
                                    {
                                        //Obteniendo Resultado del SP
                                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                                        //Validando que se haya Insertando
                                        if (result.OperacionExitosa)
                                        {
                                            //Recuperando id de asignación de diesel
                                            int id_asignacion_diesel = result.IdRegistro;
                                            //Actualizando datos de asignación de combustible
                                            result = actualizaAsignacionDieselUnidad(id_unidad_asignacion_diesel, litros, id_usuario);

                                            //Si no hay errores
                                            if (result.OperacionExitosa)
                                            {
                                                //Instanciando Costo Combustible
                                                using (SAT_CL.EgresoServicio.CostoCombustible cc = new SAT_CL.EgresoServicio.CostoCombustible(id_costo_diesel))
                                                {
                                                    //Validando que exista el Costo
                                                    if (cc.id_costo_combustible > 0)

                                                        //Insertando Detalle de Liquidación
                                                        result = DetalleLiquidacion.InsertaDetalleLiquidacion(69, id_asignacion_diesel, id_unidad, id_operador, id_proveedor_compania, id_servicio,
                                                                                            objMovimientoAsignacion.id_movimiento, 0, litros, 29, cc.costo_combustible, id_usuario);
                                                    else
                                                        //Insertando Detalle de Liquidación
                                                        result = DetalleLiquidacion.InsertaDetalleLiquidacion(69, id_asignacion_diesel, id_unidad, id_operador, id_proveedor_compania, id_servicio,
                                                                                            objMovimientoAsignacion.id_movimiento, 0, litros, 29, costo_combustible, id_usuario);

                                                    //Validando que se Insertara el Detalle
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Enviamos Notificación de Diesel
                                                        Global.NotificacionPush.Instance.NotificacionDepositoAnticiposYDiesel(id_movimiento_asignacion,
                                                            SAT_CL.Global.NotificacionPush.TipoNotificacionServicio.Diesel, Fecha.EsFechaMinima(fecha_carga) ? Fecha.ObtieneFechaEstandarMexicoCentro() : fecha_carga, litros);

                                                        //Asignando Vale
                                                        result = new RetornoOperacion(id_asignacion_diesel);
                                                        //Completando Transacción
                                                        trans.Complete();
                                                    }
                                                }
                                            }
                                            else
                                                result = new RetornoOperacion(string.Format("Error al actualizar combustible asignado a unidad: {0}", result.Mensaje));
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("El Movimiento se encuentra Pagado");
                                }
                                else
                                    //EStablecemos Resultado
                                    result = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite asignar combustible.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                            }
                        }
                        else
                            //Mostrando resultado
                            result = new RetornoOperacion("Los Litros asignados no deben de exceder la capacidad de la Unidad");
                    }
                    else
                        //Establecemos Mensaje Error
                        result = new RetornoOperacion("No es posible encontrar la Unidad para la Asignación de Diesel.");
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        ///Encargado de Insertar un Vale de Diesel por Depósito
        /// </summary>
        /// <param name="nombre_operador_proveedor"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="no_vale"></param>
        /// <param name="fecha_solicitud"></param>
        /// <param name="fecha_carga"></param>
        /// <param name="costo_combustible"></param>
        /// <param name="bit_transferencia_contable"></param>
        /// <param name="referencia"></param>
        /// <param name="id_deposito"></param>
        /// <param name="tipo_vale"></param>
        /// <param name="litros"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_proveedor_compania"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_movimiento_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaValeDieselPorDeposito(string nombre_operador_proveedor, int id_compania_emisor,
                               int no_vale, DateTime fecha_solicitud, DateTime fecha_carga, decimal costo_combustible,
                               bool bit_transferencia_contable, string referencia, int id_deposito, byte tipo_vale, decimal litros, int id_unidad,
                               int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento_asignacion, int id_usuario)
        {
            //Validando Estatus del Vale de diesel
            byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;

            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos el Estatus de la Asignacion
                using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
                {
                    //Validamos Estatus de la Asignacion
                    if (objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Iniciado ||
                        objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Terminado)
                    {
                        //Validando que exista el Costo de Diesel
                        if (costo_combustible > 0)
                        {
                            //Instanciamos Deposito
                            using (SAT_CL.EgresoServicio.Deposito objDeposito = new Deposito(Convert.ToInt32(id_deposito)))
                            {
                                //Validamos Existencia de Deposito
                                if (objDeposito.id_deposito > 0)
                                {
                                    //Instanciamos Concepto
                                    using (SAT_CL.EgresoServicio.ConceptoDeposito objConcepto = new ConceptoDeposito(objDeposito.id_concepto))
                                    {
                                        //Obtenomos Unidad Diesel
                                        int id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimientoAsignacion.id_movimiento);
                                        //Unuidad a Actualizar el Depósito
                                        if (objConcepto.descripcion == "Diesel (Remolque)")
                                        //Validamos Tipo de Depósito registrado
                                        {
                                            id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadArrasteDiesel(objMovimientoAsignacion.id_movimiento);
                                        }
                                        //Validamos que Exista Unidad
                                        if (id_unidad_diesel > 0)
                                        {
                                            //Instanciamos Unidad de Diesel
                                            using (SAT_CL.Global.Unidad objUnidad = new Global.Unidad(id_unidad_diesel))
                                            {
                                                //Validando que los Litros no excedan la capacidad del Combustible
                                                if (litros < objUnidad.capacidad_combustible)
                                                {
                                                    //Armando Arreglo de Parametros
                                                    object[] param = { 1, 0, estatus, 0, nombre_operador_proveedor, id_compania_emisor, 
                                                                       no_vale, 0, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solicitud), 
                                                                       TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_carga), 0, 1, 0, 
                                                                       bit_transferencia_contable, referencia, 0, id_deposito, tipo_vale, 
                                                                       id_unidad_diesel, id_usuario, true, "", "" };

                                                    //Obteniendo Resultado del SP
                                                    result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                                                    //Validando que se haya Insertando
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Recuperando id de asignación de diesel
                                                        int id_asignacion_diesel = result.IdRegistro;
                                                        //Actualizando datos de asignación de combustible
                                                        result = actualizaAsignacionDieselUnidad(id_unidad_diesel, litros, id_usuario);

                                                        //Si no hay errores
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Insertando Detalle de Liquidación
                                                            result = DetalleLiquidacion.InsertaDetalleLiquidacion(69, id_asignacion_diesel, id_unidad, id_operador, id_proveedor_compania, id_servicio,
                                                                                                objMovimientoAsignacion.id_movimiento, 0, litros, 29, costo_combustible, id_usuario);

                                                            //Validando que se Insertara el Detalle
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Asignando Id de Asignación al Retorno
                                                                result = new RetornoOperacion(id_asignacion_diesel);
                                                                //Completando Transacción
                                                                trans.Complete();
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(string.Format("Error al actualizar combustible asignado a unidad: {0}", result.Mensaje));
                                                    }
                                                }
                                                else
                                                    //Mostrando resultado
                                                    result = new RetornoOperacion("Los Litros asignados no deben de exceder la capacidad de la Unidad");
                                            }
                                        }
                                        else
                                            //Mostrando resultado
                                            result = new RetornoOperacion("No se puede encontrar la Unidad para la Asignación de Diesel.");
                                    }
                                }
                                else
                                    //Mostramos Mensaje Resultado
                                    result = new RetornoOperacion("No existe depósito registrado para Vale de Diesel.");
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("El Costo de Diesel no puede ser 0.00");
                    }
                    else
                        //Establecemos Resultado
                        result = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite el registró del depósito.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Asignaciones de Diesel
        /// </summary>
        /// <param name="nombre_operador_proveedor">nombre del Operador y/ó Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_ubicacion_estacion">Ubicación de la Estación</param>
        /// <param name="fecha_solicitud">Fecha de la Solicitud</param>
        /// <param name="fecha_carga">Fecha de Carga</param>
        /// <param name="id_costo_diesel">Costo del Diesel</param>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="id_factura">Factura del Vale</param>
        /// <param name="bit_transferencia_contable">Estatus de Transferencia</param>
        /// <param name="referencia">Referencia del Vale</param>
        /// <param name="id_lectura">Lectura de Diesel</param>
        /// <param name="id_deposito">Deposito del Vale</param>
        /// <param name="tipo_vale">Tipo del Vale</param>
        /// <param name="id_unidad_diesel">Unidad Diesel</param>
        /// <param name="litros">Cantidad de Litros por Vale</param>
        /// <param name="id_usuario">Usuario que realiza la Actualización</param>
        /// <returns></returns>
        public RetornoOperacion EditaAsignacionDiesel(string nombre_operador_proveedor, int id_compania_emisor,
                                   int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, int id_costo_diesel, byte id_tipo_combustible, int id_factura,
                                   bool bit_transferencia_contable, string referencia, int id_lectura, int id_deposito, byte tipo_vale, int id_unidad_diesel, decimal litros, int id_usuario)
        {   //Validando Estatus de la Asignación
            byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;

            //Invocando Método de actualizacion
            return this.actualizaRegistros(estatus, this._conteo_impresion, nombre_operador_proveedor, id_compania_emisor,
                               id_ubicacion_estacion, fecha_solicitud, fecha_carga, id_costo_diesel, id_tipo_combustible, id_factura,
                               bit_transferencia_contable, referencia, id_lectura, id_deposito, tipo_vale, id_unidad_diesel, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Editar las Asignaciones de Diesel
        /// </summary>
        /// <param name="nombre_operador_proveedor">nombre del Operador y/ó Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_ubicacion_estacion">Ubicación de la Estación</param>
        /// <param name="fecha_solicitud">Fecha de la Solicitud</param>
        /// <param name="fecha_carga">Fecha de Carga</param>
        /// <param name="id_costo_diesel">Costo del Diesel</param>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="id_factura">Factura del Vale</param>
        /// <param name="bit_transferencia_contable">Estatus de Transferencia</param>
        /// <param name="referencia">Referencia del Vale</param>
        /// <param name="id_lectura">Lectura de Diesel</param>
        /// <param name="id_deposito">Deposito del Vale</param>
        /// <param name="tipo_vale">Tipo del Vale</param>
        /// <param name="litros">Cantidad de Litros por Vale</param>
        /// <param name="costo_combustible">Costo del Combustible</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor_compania">Compania Proveedora</param>
        /// <param name="id_servicio">Servicio Ligado</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_unidad_diesel">Unidad de Diesel (Remolque)</param>
        /// <param name="id_usuario">Usuario que realiza la Actualización</param>
        /// <returns></returns>
        public RetornoOperacion EditaAsignacionDiesel(string nombre_operador_proveedor, int id_compania_emisor,
                                   int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, int id_costo_diesel, byte id_tipo_combustible, int id_factura,
                                   bool bit_transferencia_contable, string referencia, int id_lectura, int id_deposito, byte tipo_vale, decimal litros, decimal costo_combustible,
                                   int id_unidad, int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento, int id_unidad_diesel, int id_usuario)
        {
            //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obtenemos Unidad Anterior para Restar
                int id_unidad_anterior = this._id_unidad_diesel;
                //Validando Estatus de la Asignación
                byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;
                // Validamos que no sea Vale de Ruta
                if (Cadena.RegresaCadenaSeparada(referencia, "[", 0) != "DIESEL RUTA")
                {

                    //Instanciamos Unidad de Diesel
                    using (SAT_CL.Global.Unidad objUnidadDisel = new SAT_CL.Global.Unidad(id_unidad_diesel))
                    {
                        //Validamos Unidad de Diesel
                        if (objUnidadDisel.id_unidad > 0)
                        {
                            //Validando que los Litros no excedan la capacidad del Combustible
                            if (litros <= objUnidadDisel.capacidad_combustible)
                            {
                                //Invocando Método de actualizacion
                                result = this.actualizaRegistros(estatus, this._conteo_impresion, nombre_operador_proveedor, id_compania_emisor,
                                                   id_ubicacion_estacion, fecha_solicitud, fecha_carga, id_costo_diesel, id_tipo_combustible, id_factura,
                                                   bit_transferencia_contable, referencia, id_lectura, id_deposito, tipo_vale, id_unidad_diesel, id_usuario, this._habilitar);
                                //Validando que se haya 
                                if (result.OperacionExitosa)
                                {
                                    //Preservando id de asignación de diesel
                                    int id_asignacion = result.IdRegistro;

                                    //Instanciando Detalle
                                    using (DetalleLiquidacion dl = new DetalleLiquidacion(id_asignacion, 69))
                                    {
                                        //Validando que exista un Registro detalle de liquidación
                                        if (dl.id_detalle_liquidacion != 0)
                                        {
                                            //Obtenemos Diferencia de litros
                                            decimal diferencia = litros - dl.cantidad;

                                            //Si Existe Diferencia de Unidad
                                            if (id_unidad_anterior != id_unidad_diesel)
                                            {
                                                //Restamos los litros de la Unidad Anterior
                                                result = actualizaAsignacionDieselUnidad(id_unidad_anterior, -dl.cantidad, id_usuario);

                                                //Validamos Resultado
                                                if (result.OperacionExitosa)
                                                {
                                                    //Suma litros a la Nueva Unidad
                                                    //Restamos los litros de la Unidad Anterior
                                                    result = actualizaAsignacionDieselUnidad(id_unidad_diesel, litros, id_usuario);
                                                }
                                            }
                                            else
                                            {
                                                //Obteniendo diferencia de litros (en caso de existir)
                                                if (dl.cantidad != litros)
                                                {
                                                    //Si es la Unidad Actual 
                                                    //Actualizando diferencia
                                                    result = actualizaAsignacionDieselUnidad(id_unidad_diesel, diferencia, id_usuario);
                                                }
                                            }


                                            //Si no hay errores hasta este punto
                                            if (result.OperacionExitosa)
                                            {
                                                //Instanciando Costo del Combustible
                                                using (CostoCombustible cc = new CostoCombustible(id_costo_diesel))
                                                {
                                                    //Validando si existe el Costo
                                                    if (cc.habilitar)

                                                        //Editando Detalle
                                                        result = dl.EditaDetalleLiquidacion(dl.id_tabla, id_asignacion, dl.id_estatus_liquidacion, id_unidad, id_operador,
                                                                            id_proveedor_compania, id_servicio, id_movimiento, dl.fecha_liquidacion, dl.id_liquidacion,
                                                                            litros, dl.id_unidad_medida, cc.costo_combustible, id_usuario);
                                                    else
                                                        //Editando Detalle
                                                        result = dl.EditaDetalleLiquidacion(dl.id_tabla, id_asignacion, dl.id_estatus_liquidacion, id_unidad, id_operador,
                                                                            id_proveedor_compania, id_servicio, id_movimiento, dl.fecha_liquidacion, dl.id_liquidacion,
                                                                            litros, dl.id_unidad_medida, costo_combustible, id_usuario);
                                                }
                                            }
                                            else
                                                result = new RetornoOperacion(string.Format("Error al actualizar combustible asignado a unidad: {0}", result.Mensaje));

                                        }
                                    }

                                    //Validando Operaciones
                                    if (result.OperacionExitosa)
                                    {
                                        result = new RetornoOperacion(id_asignacion);
                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                            else
                                //Mostrando resultado
                                result = new RetornoOperacion("Los Litros asignados no deben de exceder la capacidad de la Unidad");
                        }
                        else
                            //Establecemos Mensaje Error
                            result = new RetornoOperacion("No es posible encontrar la Unidad para la Asignación de Diesel.");
                    }
                }
                else
                    //Mostrando Error
                    result = new RetornoOperacion("Imposible editar el vale ya que se realizó al calcular la ruta.");
            }
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar la Lectura de Diesel
        /// </summary>
        /// <param name="id_lectura">Lectura Obtenido</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaLecturaDiesel(int id_lectura, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(this._id_estatus, this._conteo_impresion, this._nombre_operador_proveedor, this._id_compania_emisor,
                               this._id_ubicacion_estacion, this._fecha_solicitud, fecha_carga, this._id_costo_diesel, this._id_tipo_combustible, this._id_factura,
                               this._bit_transferencia_contable, this._referencia, id_lectura, this._id_deposito, this._tipo_vale, this._id_unidad_diesel, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Liquidar el Vale de Diesel, actualizando su Detalle de Liquidación
        /// </summary>
        /// <param name="id_liquidacion"></param>
        /// <param name="fecha_liquidacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion LiquidaValeDiesel(int id_liquidacion, DateTime fecha_liquidacion, int id_usuario)
        {
            //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion(this._id_asignacion_diesel);
            RetornoOperacion resultDetalle = new RetornoOperacion();

            //Instanciando Detalle
            using (DetalleLiquidacion dl = new DetalleLiquidacion(result.IdRegistro, 69))
            {
                //Validando que exista un Registro
                if (dl.id_detalle_liquidacion != 0)
                {
                    //Instanciando Costo del Combustible
                    using (CostoCombustible cc = new CostoCombustible(id_costo_diesel))
                        //Editando Detalle
                        resultDetalle = dl.EditaDetalleLiquidacion(dl.id_tabla, result.IdRegistro, (byte)DetalleLiquidacion.Estatus.Liquidado, dl.id_unidad,
                                            dl.id_operador, dl.id_proveedor_compania, dl.id_servicio, dl.id_movimiento, fecha_liquidacion,
                                            id_liquidacion, dl.cantidad, dl.id_unidad_medida, dl.valor_unitario, id_usuario);
                }
            }

            //Validando Operaciones
            if (!resultDetalle.OperacionExitosa)
                //Instanciando Excepcion
                result = new RetornoOperacion("No se ha podido Liquidar el Vale");

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Asignar los Vales de Diesel a una Factura
        /// </summary>
        /// <param name="id_factura">Factura Asignada</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion AsignaFacturaValeDiesel(int id_factura, int id_usuario)
        {   //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Invocando Método de actualizacion
            result = this.actualizaRegistros(this._id_estatus, this._conteo_impresion, this._nombre_operador_proveedor, this._id_compania_emisor,
                               this._id_ubicacion_estacion, this._fecha_solicitud, this._fecha_carga, this._id_costo_diesel, this._id_tipo_combustible, id_factura,
                               this._bit_transferencia_contable, this._referencia, this._id_lectura, this._id_deposito, this._tipo_vale, this._id_unidad_diesel, id_usuario, this._habilitar);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Quitar el Vale de Diesel de la Factura
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion QuitaFacturaValeDiesel(int id_usuario)
        {   //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Invocando Método de actualizacion
            result = this.actualizaRegistros(this._id_estatus, this._conteo_impresion, this._nombre_operador_proveedor, this._id_compania_emisor,
                               this._id_ubicacion_estacion, this._fecha_solicitud, this._fecha_carga, this._id_costo_diesel, this._id_tipo_combustible, 0,
                               this._bit_transferencia_contable, this._referencia, this._id_lectura, this._id_deposito, this._tipo_vale, this._id_unidad_diesel, id_usuario, this._habilitar);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Asignaciones de Diesel
        /// </summary>
        /// <param name="id_usuario">Usuario que realiza la Actualización</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAsignacionDiesel(int id_usuario)
        {   //Invocando Método de actualizacion
            return this.actualizaRegistros(this._id_estatus, this._conteo_impresion, this._nombre_operador_proveedor, this._id_compania_emisor,
                               this._id_ubicacion_estacion, this._fecha_solicitud, this._fecha_carga, this._id_costo_diesel, this._id_tipo_combustible, this._id_factura,
                               this._bit_transferencia_contable, this._referencia, this._id_lectura, this._id_deposito, this._tipo_vale, this._id_unidad_diesel, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Datos de la Asignación
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAsignacionDiesel()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_asignacion_diesel);
        }
        /// <summary>
        /// Método Público encaragdo de Incrementar el Contador de Impresiones
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion IncrementaConteoImpresion(int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos que no sea vale de Depósito
            //if (this._id_deposito == 0)
            //{
                //Invocando Método de actualizacion
                resultado = this.actualizaRegistros(this._id_estatus, this._conteo_impresion + 1, this._nombre_operador_proveedor, this._id_compania_emisor,
                                   this._id_ubicacion_estacion, this._fecha_solicitud, this._fecha_carga, this._id_costo_diesel, this._id_tipo_combustible, this._id_factura,
                                   this._bit_transferencia_contable, this._referencia, this._id_lectura, this._id_deposito, this._tipo_vale, this._id_unidad_diesel, id_usuario, this._habilitar);
            //}
            //else
                //Mostrando mensaje
                //resultado = new RetornoOperacion("Imposible su impresión ya que fue diesel en efectivo.");
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Vales Ligados a una Factura
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <returns></returns>
        public static DataTable ObtieneValesPorFactura(int id_factura)
        {   //Declarando Objeto de Retorno
            DataTable dtVales = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, "", 0, 0, 0, null, null, 0, 0, id_factura, false, "", 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Reporte Obtenido
                    dtVales = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtVales;
        }


        /// <summary>
        /// Obtiene el total de vales de diesel asignados al recurso para la liquidación y servicio especificados
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_liquidacion">Id de liquidación</param>
        /// <param name="id_entidad">Id de Entidad</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad</param>
        /// <returns></returns>
        public static DataTable ObtieneAsignacionesDieselServicio(int id_servicio, int id_liquidacion, int id_entidad, byte id_tipo_entidad)
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
                            using (DataTable mitVales = ObtieneAsignacionesDieselMovimiento(m.Field<int>("Id"), id_liquidacion, id_entidad, id_tipo_entidad))
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
        /// Método Público encargado de Obtener el Reporte de Vales por Movimiento
        /// </summary>
        /// <param name="id_movimiento">Movimiento del Vale</param>
        /// <param name="id_liquidacion">Liquidación a la que Pertenece el Vale</param>
        /// <param name="id_entidad">Entidad a la que esta Asignado el Vale</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad del Vale (1.-Unidad, 2.-Operador, 3.-Tercero)</param>
        /// <returns></returns>
        public static DataTable ObtieneAsignacionesDieselMovimiento(int id_movimiento, int id_liquidacion, int id_entidad, byte id_tipo_entidad)
        {
            //Declarando Objeto de Retorno
            DataTable dtValesDiesel = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, id_movimiento, 0, id_liquidacion, "", 0, 0, 0, null, null, 0, 0, 0, false, "", 0, 0, 0, 0, 0, false, id_tipo_entidad.ToString(), id_entidad.ToString() };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtValesDiesel = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtValesDiesel;
        }
        /// <summary>
        /// Método Público encargado de Actualizar la Liquidación del Vale
        /// </summary>
        /// <param name="id_liquidacion">Referencia de la Liquidación</param>
        /// <param name="estatus">Estatus de la Liquidación</param>
        /// <param name="id_usuario">Usuario que actualzia el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaLiquidacionVale(int id_liquidacion, DetalleLiquidacion.Estatus estatus, int id_usuario)
        {
            //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion(this._id_asignacion_diesel);
            RetornoOperacion resultDetalle = new RetornoOperacion();

            //Instanciando Detalle
            using (DetalleLiquidacion dl = new DetalleLiquidacion(result.IdRegistro, 69))
            {
                //Validando que exista un Registro
                if (dl.id_detalle_liquidacion > 0)

                    //Editando Detalle
                    resultDetalle = dl.EditaDetalleLiquidacion(dl.id_tabla, result.IdRegistro, (byte)estatus, dl.id_unidad,
                                        dl.id_operador, dl.id_proveedor_compania, dl.id_servicio, dl.id_movimiento, dl.fecha_liquidacion,
                                        id_liquidacion, dl.cantidad, dl.id_unidad_medida, dl.valor_unitario, id_usuario);
            }

            //Validando Operaciones
            if (!resultDetalle.OperacionExitosa)

                //Instanciando Excepcion
                result = new RetornoOperacion("No se ha podido Liquidar el Vale");

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que permite realizar la carga de datos de AsignacionDiesel a una tabla 
        /// </summary>
        /// <param name="id_asignacion_diesel">Permite identificar al registro al cual va hacer consultado para su almacenamiento en la tabla CargaImpresionValeDiesel</param>
        /// <returns></returns>
        public static DataTable CargaImpresionValeDiesel(int id_asignacion_diesel)
        {
            //Creacion del objeto retorno 
            DataTable dtValeDiesel = null;
            //Creaciòn del arreglo param, con los valores para la obtencion de datos de impresiòn de vale
            object[] param = { 7, id_asignacion_diesel, 0, 0, "", 0, 0, 0, null, null, 0, 0, 0, false, "", 0, 0, 0, 0, 0, false, "", "" };
            //Valida que los datos obtenidos de asignaciòn de vale
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que exista el regitro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla cargaVale los valores encontrados del registro
                    dtValeDiesel = DS.Tables["Table"];
            }
            //Devuelve el resultado al método
            return dtValeDiesel;
        }
        /// <summary>
        /// Método encargado de Obtener la Instancia del Vale dado un Deposito
        /// </summary>
        /// <param name="id_deposito">Deposito del Vale</param>
        /// <returns></returns>
        public static AsignacionDiesel ObtieneValePorDeposito(int id_deposito)
        {
            //Declarando Objeto de Retorno
            AsignacionDiesel vale = new AsignacionDiesel();

            //Armando Arreglo de Parametros
            object[] param = { 8, 0, 0, 0, "", 0, 0, 0, null, null, 0, 0, 0, false, "", 0, id_deposito, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Instanciando vale de Diesel
                        vale = new AsignacionDiesel(Convert.ToInt32(dr["Id"]));
                }
            }

            //Devolviendo Resultado Obtenido
            return vale;
        }
        /// <summary>
        /// Obtiene una tabla con los detalles de vales para combustible (Diesel, G.Magna, G.Premium)
        /// </summary>
        /// <returns></returns>
        public static DataTable ObtieneValesCombustible()
        {
            //Crear objeto retorno
            DataTable dtValesCombustible = null;
            //Crear arreglo de parámetros para el stored procedure
            object[] param = { 9, 0, 0, 0, "", 0, 0, 0, null, null, 0, 0, 0, false, "", 0, 0, 0, 0, 0, false, "", "" };
            //Ejecutar consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validar que devuelva registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Vaciar resultados al objeto retorno
                    dtValesCombustible = DS.Tables["Table"];
                }
            }
            //Devolver el objeto
            return dtValesCombustible;
        }

        public RetornoOperacion EliminaValeDiesel(int id_asignacion_diesel, int id_unidad_diesel, int id_compania_emisor, int id_detalle_liquidacion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invoca a la clase AsignacionDiesel
                using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new SAT_CL.EgresoServicio.AsignacionDiesel(id_asignacion_diesel))
                {
                    //Valida que exista el registro
                    if (ad.id_asignacion_diesel > 0)
                        //Asigna valores al objeto retorno
                        retorno = ad.DeshabilitaAsignacionDiesel(id_usuario);
                    //Instancia a la clase requisición
                    //Valida si la inserción a la base de datos se realizo correctamente
                    if (retorno.OperacionExitosa)//si se deshabilito
                    {
                        using (SAT_CL.Global.Unidad u = new SAT_CL.Global.Unidad(id_unidad_diesel))
                        {
                            //Valida que exista el registro
                            if (u.id_unidad > 0)
                                using (SAT_CL.EgresoServicio.DetalleLiquidacion dl = new SAT_CL.EgresoServicio.DetalleLiquidacion(id_detalle_liquidacion))
                                    retorno = u.RestaCombustibleAsignado(u.combustible_asignado - dl.cantidad, id_usuario);
                        }
                        if (retorno.OperacionExitosa)//si se desconto combustible
                        {
                            //Preservando id de asignación de diesel
                            int id_asignacion = ad.id_asignacion_diesel;
                            using (SAT_CL.EgresoServicio.DetalleLiquidacion dl = new SAT_CL.EgresoServicio.DetalleLiquidacion(id_asignacion, 69))
                            {
                                //Validad que exista el registro
                                if (dl.id_detalle_liquidacion > 0)
                                    //Asigna valores al objeto retorno
                                    retorno = dl.DeshabilitaDetalleLiquidacion(id_usuario);
                            }
                        }

                    }

                }
                //Valida si la inserción a la base de datos se realizo correctamente
                if (retorno.OperacionExitosa)

                    trans.Complete();
                    

            }
            return retorno;
        }
        /// <summary>
        ///Encargado de Insertar un Vale de Diesel por Depósito
        /// </summary>
        /// <param name="nombre_operador_proveedor"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="no_vale"></param>
        /// <param name="fecha_solicitud"></param>
        /// <param name="fecha_carga"></param>
        /// <param name="costo_combustible"></param>
        /// <param name="bit_transferencia_contable"></param>
        /// <param name="referencia"></param>
        /// <param name="id_deposito"></param>
        /// <param name="tipo_vale"></param>
        /// <param name="litros"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_proveedor_compania"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_movimiento_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaValeDieselPorDeposito(string nombre_operador_proveedor, int id_compania_emisor,
                               int no_vale, int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, decimal costo_combustible,
                               bool bit_transferencia_contable, string referencia, int id_deposito, byte tipo_vale, decimal litros, int id_unidad,
                               int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento_asignacion, int id_usuario)
        {
            //Validando Estatus del Vale de diesel
            byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;

            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos el Estatus de la Asignacion
                using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
                {
                    //Validamos Estatus de la Asignacion
                    if (objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Iniciado ||
                        objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Terminado)
                    {
                        //Validando que exista el Costo de Diesel
                        if (costo_combustible > 0)
                        {
                            //Instanciamos Deposito
                            using (SAT_CL.EgresoServicio.Deposito objDeposito = new Deposito(Convert.ToInt32(id_deposito)))
                            {
                                //Validamos Existencia de Deposito
                                if (objDeposito.id_deposito > 0)
                                {
                                    //Instanciamos Concepto
                                    using (SAT_CL.EgresoServicio.ConceptoDeposito objConcepto = new ConceptoDeposito(objDeposito.id_concepto))
                                    {
                                        //Obtenomos Unidad Diesel
                                        int id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimientoAsignacion.id_movimiento);
                                        //Unuidad a Actualizar el Depósito
                                        if (objConcepto.descripcion == "Diesel (Remolque)")
                                        //Validamos Tipo de Depósito registrado
                                        {
                                            id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadArrasteDiesel(objMovimientoAsignacion.id_movimiento);
                                        }
                                        //Validamos que Exista Unidad
                                        if (id_unidad_diesel > 0)
                                        {
                                            //Instanciamos Unidad de Diesel
                                            using (SAT_CL.Global.Unidad objUnidad = new Global.Unidad(id_unidad_diesel))
                                            {
                                                //Validando que los Litros no excedan la capacidad del Combustible
                                                if (litros < objUnidad.capacidad_combustible)
                                                {
                                                    //Armando Arreglo de Parametros
                                                    object[] param = { 1, 0, estatus, 0, nombre_operador_proveedor, id_compania_emisor,
                                                                       no_vale, id_ubicacion_estacion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solicitud),
                                                                       TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_carga), 0, 1, 0,
                                                                       bit_transferencia_contable, referencia, 0, id_deposito, tipo_vale,
                                                                       id_unidad_diesel, id_usuario, true, "", "" };

                                                    //Obteniendo Resultado del SP
                                                    result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                                                    //Validando que se haya Insertando
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Recuperando id de asignación de diesel
                                                        int id_asignacion_diesel = result.IdRegistro;
                                                        //Actualizando datos de asignación de combustible
                                                        result = actualizaAsignacionDieselUnidad(id_unidad_diesel, litros, id_usuario);

                                                        //Si no hay errores
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Insertando Detalle de Liquidación
                                                            result = DetalleLiquidacion.InsertaDetalleLiquidacion(69, id_asignacion_diesel, id_unidad, id_operador, id_proveedor_compania, id_servicio,
                                                                                                objMovimientoAsignacion.id_movimiento, 0, litros, 29, costo_combustible, id_usuario);

                                                            //Validando que se Insertara el Detalle
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Asignando Id de Asignación al Retorno
                                                                result = new RetornoOperacion(id_asignacion_diesel);
                                                                //Completando Transacción
                                                                trans.Complete();
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(string.Format("Error al actualizar combustible asignado a unidad: {0}", result.Mensaje));
                                                    }
                                                }
                                                else
                                                    //Mostrando resultado
                                                    result = new RetornoOperacion("Los Litros asignados no deben de exceder la capacidad de la Unidad");
                                            }
                                        }
                                        else
                                            //Mostrando resultado
                                            result = new RetornoOperacion("No se puede encontrar la Unidad para la Asignación de Diesel.");
                                    }
                                }
                                else
                                    //Mostramos Mensaje Resultado
                                    result = new RetornoOperacion("No existe depósito registrado para Vale de Diesel.");
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("El Costo de Diesel no puede ser 0.00");
                    }
                    else
                        //Establecemos Resultado
                        result = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite el registró del depósito.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        ///Encargado de Insertar un Vale de Diesel por Depósito
        /// </summary>
        /// <param name="nombre_operador_proveedor"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="no_vale"></param>
        /// <param name="fecha_solicitud"></param>
        /// <param name="fecha_carga"></param>
        /// <param name="costo_combustible"></param>
        /// <param name="bit_transferencia_contable"></param>
        /// <param name="referencia"></param>
        /// <param name="id_deposito"></param>
        /// <param name="tipo_vale"></param>
        /// <param name="litros"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_proveedor_compania"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_movimiento_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaValeDieselPorDepositoProgramado(string nombre_operador_proveedor, int id_compania_emisor,
                               int no_vale, int id_ubicacion_estacion, DateTime fecha_solicitud, DateTime fecha_carga, decimal costo_combustible,
                               bool bit_transferencia_contable, string referencia, int id_deposito, byte tipo_vale, decimal litros, int id_unidad,
                               int id_operador, int id_proveedor_compania, int id_servicio, int id_movimiento_asignacion, int id_usuario)
        {
            //Validando Estatus del Vale de diesel
            byte estatus = litros == 0 ? (byte)Estatus.Registrado : (byte)Estatus.Actualizado;

            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos el Estatus de la Asignacion
                using (MovimientoAsignacionRecurso objMovimientoAsignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
                {
                    //Validamos Estatus de la Asignacion
                    if (objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Iniciado ||
                        objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Terminado ||
                        objMovimientoAsignacion.EstatusMovimientoAsignacion == MovimientoAsignacionRecurso.Estatus.Registrado)
                    {
                        //Validando que exista el Costo de Diesel
                        if (costo_combustible > 0)
                        {
                            //Instanciamos Deposito
                            using (SAT_CL.EgresoServicio.Deposito objDeposito = new Deposito(Convert.ToInt32(id_deposito)))
                            {
                                //Validamos Existencia de Deposito
                                if (objDeposito.id_deposito > 0)
                                {
                                    //Instanciamos Concepto
                                    using (SAT_CL.EgresoServicio.ConceptoDeposito objConcepto = new ConceptoDeposito(objDeposito.id_concepto))
                                    {
                                        //Obtenomos Unidad Diesel
                                        int id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimientoAsignacion.id_movimiento);
                                        //Unuidad a Actualizar el Depósito
                                        if (objConcepto.descripcion == "Diesel (Remolque)")
                                        //Validamos Tipo de Depósito registrado
                                        {
                                            id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadArrasteDiesel(objMovimientoAsignacion.id_movimiento);
                                        }
                                        //Validamos que Exista Unidad
                                        if (id_unidad_diesel > 0)
                                        {
                                            //Instanciamos Unidad de Diesel
                                            using (SAT_CL.Global.Unidad objUnidad = new Global.Unidad(id_unidad_diesel))
                                            {
                                                //Validando que los Litros no excedan la capacidad del Combustible
                                                if (litros < objUnidad.capacidad_combustible)
                                                {
                                                    //Armando Arreglo de Parametros
                                                    object[] param = { 1, 0, estatus, 0, nombre_operador_proveedor, id_compania_emisor,
                                                                       no_vale, id_ubicacion_estacion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solicitud),
                                                                       TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_carga), 0, 1, 0,
                                                                       bit_transferencia_contable, referencia, 0, id_deposito, tipo_vale,
                                                                       id_unidad_diesel, id_usuario, true, "", "" };

                                                    //Obteniendo Resultado del SP
                                                    result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                                                    //Validando que se haya Insertando
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Recuperando id de asignación de diesel
                                                        int id_asignacion_diesel = result.IdRegistro;
                                                        //Actualizando datos de asignación de combustible
                                                        result = actualizaAsignacionDieselUnidad(id_unidad_diesel, litros, id_usuario);

                                                        //Si no hay errores
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Insertando Detalle de Liquidación
                                                            result = DetalleLiquidacion.InsertaDetalleLiquidacion(69, id_asignacion_diesel, id_unidad, id_operador, id_proveedor_compania, id_servicio,
                                                                                                objMovimientoAsignacion.id_movimiento, 0, litros, 29, costo_combustible, id_usuario);

                                                            //Validando que se Insertara el Detalle
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Asignando Id de Asignación al Retorno
                                                                result = new RetornoOperacion(id_asignacion_diesel);
                                                                //Completando Transacción
                                                                trans.Complete();
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(string.Format("Error al actualizar combustible asignado a unidad: {0}", result.Mensaje));
                                                    }
                                                }
                                                else
                                                    //Mostrando resultado
                                                    result = new RetornoOperacion("Los Litros asignados no deben de exceder la capacidad de la Unidad");
                                            }
                                        }
                                        else
                                            //Mostrando resultado
                                            result = new RetornoOperacion("No se puede encontrar la Unidad para la Asignación de Diesel.");
                                    }
                                }
                                else
                                    //Mostramos Mensaje Resultado
                                    result = new RetornoOperacion("No existe depósito registrado para Vale de Diesel.");
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("El Costo de Diesel no puede ser 0.00");
                    }
                    else
                        //Establecemos Resultado
                        result = new RetornoOperacion(string.Format("El estatus '{0}' de la asignación no permite el registró del depósito.", (SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion));
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
