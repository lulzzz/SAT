using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// 
    /// </summary>
    public class OrdenTrabajo : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los posibles etstaus de una Orden de Trabajo
        /// </summary>
        public enum EstatusOrdenTrabajo
        {
            /// <summary>
            /// Estatus que indica que la orden esta en progreso
            /// </summary>
            Activa = 1,
            /// <summary>
            /// Estatus que indica que la orden esta activa sin embargo todas las actividades estan pausadas
            /// </summary>
            Pausada,
            /// <summary>
            /// Estatus que indica que la orden esta terminada
            /// </summary>
            Terminada
        }
        /// <summary>
        /// Define los tipos existentes de Orden de TRabajo
        /// </summary>
        public enum TipoOrdenTrabajo
        {
            /// <summary>
            /// Tipo estandar de orden de trabajo
            /// </summary>
            Estandar = 1
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nom_sp = "mantenimiento.sp_orden_trabajo_tot";

        private int _id_orden_trabajo;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_orden_trabajo { get { return _id_orden_trabajo; } }
        private int _no_orden_trabajo;
        /// <summary>
        /// Atributo que almacena el Número de Orden
        /// </summary>
        public int no_orden_trabajo { get { return _no_orden_trabajo; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_compania_emisora { get { return _id_compania_emisora; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public byte id_estatus { get { return _id_estatus; } }
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public EstatusOrdenTrabajo EstatusOrden { get { return (EstatusOrdenTrabajo)_id_estatus; } }
        private byte _id_tipo;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public byte id_tipo { get { return _id_tipo; } }
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public TipoOrdenTrabajo TipoOrden { get { return (TipoOrdenTrabajo)_id_tipo; } }
        private DateTime _fecha_recepcion;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public DateTime fecha_recepcion { get { return _fecha_recepcion; } }
        private DateTime _fecha_compromiso;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public DateTime fecha_compromiso { get { return _fecha_compromiso; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public DateTime fecha_inicio { get { return _fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public DateTime fecha_fin { get { return _fecha_fin; } }
        private bool _bit_unidad_externa;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public bool bit_unidad_externa { get { return _bit_unidad_externa; } }
        private int _id_compania_cliente;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_compania_cliente { get { return _id_compania_cliente; } }
        private int _id_compania_proveedor;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_compania_proveedor { get { return _id_compania_proveedor; } }
        private byte _id_tipo_unidad;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public byte id_tipo_unidad { get { return _id_tipo_unidad; } }
        private int _id_unidad;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_unidad { get { return _id_unidad; } }
        private byte _id_subtipo_unidad;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public byte id_subtipo_unidad { get { return _id_subtipo_unidad; } }
        private string _descripcion_unidad;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public string descripcion_unidad { get { return _descripcion_unidad; } }
        private byte _id_tipo_taller;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public byte id_tipo_taller { get { return _id_tipo_taller; } }
        private int _id_ubicacion_taller;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_ubicacion_taller { get { return _id_ubicacion_taller; } }
        private decimal _odometro;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public decimal odometro { get { return _odometro; } }
        private decimal _nivel_combustible;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public decimal nivel_combustible { get { return _nivel_combustible; } }
        private string _no_siniestro;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public string no_siniestro { get { return _no_siniestro; } }
        private string _entrega_unidad;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public string entrega_unidad { get { return _entrega_unidad; } }
        private string _recibe_unidad;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public string recibe_unidad { get { return _recibe_unidad; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public bool habilitar { get { return _habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public OrdenTrabajo()
        {
            //Asignando Valores
            this._id_orden_trabajo = 0;
            this._no_orden_trabajo = 0;
            this._id_compania_emisora = 0;
            this._id_estatus = 0; 
            this._id_tipo = 0; 
            this._fecha_recepcion = DateTime.MinValue;
            this._fecha_compromiso = DateTime.MinValue;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue; 
            this._bit_unidad_externa = false; 
            this._id_compania_cliente = 0; 
            this._id_compania_proveedor = 0; 
            this._id_tipo_unidad = 0; 
            this._id_unidad = 0; 
            this._id_subtipo_unidad = 0; 
            this._descripcion_unidad = ""; 
            this._id_tipo_taller = 0; 
            this._id_ubicacion_taller = 0; 
            this._odometro = 0; 
            this._nivel_combustible = 0; 
            this._no_siniestro = ""; 
            this._entrega_unidad = ""; 
            this._recibe_unidad = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        public OrdenTrabajo(int id_orden_trabajo)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_orden_trabajo);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~OrdenTrabajo()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            bool result = false;
            
            //Inicializamos el arreglo de parametros
            object[] param = { 3, id_orden_trabajo, 0, 0, 0, 0, null, null, null, null, false, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, "", "", "", 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_orden_trabajo = Convert.ToInt32(r["IdOrden"]);
                        this._no_orden_trabajo = Convert.ToInt32(r["NoOrden"]);
                        this._id_compania_emisora = Convert.ToInt32(r["IdCompaniaEmisora"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        DateTime.TryParse(r["FechaRecepcion"].ToString(), out this._fecha_recepcion);
                        DateTime.TryParse(r["FechaCompromiso"].ToString(), out this._fecha_compromiso);
                        DateTime.TryParse(r["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(r["FechaFin"].ToString(), out this._fecha_fin);
                        this._bit_unidad_externa = Convert.ToBoolean(r["BitUnidadExterna"]);
                        this._id_compania_cliente = Convert.ToInt32(r["IdCompaniaCliente"]);
                        this._id_compania_proveedor = Convert.ToInt32(r["IdCompaniaProveedor"]);
                        this._id_tipo_unidad = Convert.ToByte(r["IdTipoUnidad"]);
                        this._id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        this._id_subtipo_unidad = Convert.ToByte(r["IdSubtipoUnidad"]);
                        this._descripcion_unidad = r["Descripcion"].ToString();
                        this._id_tipo_taller = Convert.ToByte(r["IdTipoTaller"]);
                        this._id_ubicacion_taller = Convert.ToInt32(r["IdUbicacionTaller"]);
                        this._odometro = Convert.ToDecimal(r["Odometro"]);
                        this._nivel_combustible = Convert.ToDecimal(r["NivelCombustible"]);
                        this._no_siniestro = r["NoSiniestro"].ToString();
                        this._entrega_unidad = r["EntregaUnidad"].ToString();
                        this._recibe_unidad = r["RecibeUnidad"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Asignando Valor Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="id_tipo">Tipo</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="fecha_compromiso">Fecha de Compromiso</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="bit_unidad_externa">Indicador de Unidad Externa</param>
        /// <param name="id_compania_cliente">Compania de Cliente</param>
        /// <param name="id_compania_proveedor">Compania de Proveedor</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad</param>
        /// <param name="id_unidad">Referencia de la Unidad</param>
        /// <param name="id_subtipo_unidad">Subtipo de la Unidad</param>
        /// <param name="descripcion_unidad">Descripción de la Unidad</param>
        /// <param name="id_tipo_taller">Tipo de Taller</param>
        /// <param name="id_ubicacion_taller">Ubicación del Taller</param>
        /// <param name="odometro">Odometro</param>
        /// <param name="nivel_combustible">Nivel de Combustible</param>
        /// <param name="no_siniestro">Número de Siniestro</param>
        /// <param name="entrega_unidad">Quien Entrega la Unidad</param>
        /// <param name="recibe_unidad">Quien Recibe la Unidad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_compania_emisora, byte id_estatus, byte id_tipo, DateTime fecha_recepcion, DateTime fecha_compromiso, 
                    DateTime fecha_inicio, DateTime fecha_fin, bool bit_unidad_externa, int id_compania_cliente, int id_compania_proveedor, 
                    byte id_tipo_unidad, int id_unidad, byte id_subtipo_unidad, string descripcion_unidad, byte id_tipo_taller, int id_ubicacion_taller, 
                    decimal odometro, decimal nivel_combustible, string no_siniestro, string entrega_unidad, string recibe_unidad, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializamos el arreglo de parametros
            object[] param = { 2, this._id_orden_trabajo, this._no_orden_trabajo, id_compania_emisora, id_estatus, id_tipo, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_recepcion), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_compromiso),
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               bit_unidad_externa, id_compania_cliente, id_compania_proveedor, id_tipo_unidad, id_unidad, id_subtipo_unidad, 
                               descripcion_unidad, id_tipo_taller, id_ubicacion_taller, odometro, nivel_combustible, no_siniestro, entrega_unidad, 
                               recibe_unidad, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Ordenes de Trabajo
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="fecha_compromiso">Fecha de Compromiso</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="bit_unidad_externa">Indicador de Unidad Externa</param>
        /// <param name="id_compania_cliente">Compania de Cliente</param>
        /// <param name="id_compania_proveedor">Compania de Proveedor</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad</param>
        /// <param name="id_unidad">Referencia de la Unidad</param>
        /// <param name="id_subtipo_unidad">Subtipo de la Unidad</param>
        /// <param name="descripcion_unidad">Descripción de la Unidad</param>
        /// <param name="id_tipo_taller">Tipo de Taller</param>
        /// <param name="id_ubicacion_taller">Ubicación del Taller</param>
        /// <param name="odometro">Odometro</param>
        /// <param name="nivel_combustible">Nivel de Combustible</param>
        /// <param name="no_siniestro">Número de Siniestro</param>
        /// <param name="entrega_unidad">Quien Entrega la Unidad</param>
        /// <param name="recibe_unidad">Quien Recibe la Unidad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaOrdenTrabajo(int id_compania_emisora, DateTime fecha_recepcion, DateTime fecha_compromiso,
                    DateTime fecha_inicio, DateTime fecha_fin, bool bit_unidad_externa, int id_compania_cliente, int id_compania_proveedor,
                    byte id_tipo_unidad, int id_unidad, byte id_subtipo_unidad, string descripcion_unidad, byte id_tipo_taller, int id_ubicacion_taller,
                    decimal odometro, decimal nivel_combustible, string no_siniestro, string entrega_unidad, string recibe_unidad, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            int idOrdenTrabajo = 0;

            //Inicializamos el arreglo de parametros
            object[] param = { 1, 0, 0, id_compania_emisora, (byte)EstatusOrdenTrabajo.Activa, (byte)TipoOrdenTrabajo.Estandar, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_recepcion), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_compromiso), 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               bit_unidad_externa, id_compania_cliente, id_compania_proveedor, id_tipo_unidad, id_unidad, id_subtipo_unidad, 
                               descripcion_unidad, id_tipo_taller, id_ubicacion_taller, odometro, nivel_combustible, no_siniestro, entrega_unidad, 
                               recibe_unidad, id_usuario, true, "", "" };

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando Variable Auxiliar
                int id_ubicacion_unidad = 0;

                //Validando que sea una Unidad Externa
                if (!bit_unidad_externa)
                {
                    //Instanciando unidad
                    using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(id_unidad))
                    {
                        //Validando que este habilitado
                        if (unidad.habilitar)
                        {
                            //Obteniendo Estancia
                            using (SAT_CL.Despacho.EstanciaUnidad estancia = new SAT_CL.Despacho.EstanciaUnidad(unidad.id_estancia))
                            using (SAT_CL.Despacho.Parada parada = new SAT_CL.Despacho.Parada(estancia.id_parada))
                            {
                                //Validando que exista
                                if (estancia.habilitar && parada.habilitar)

                                    //Asignando Ubicación de la Unidad
                                    id_ubicacion_unidad = parada.id_ubicacion;
                                else
                                    //Asignando Ubicación Actual
                                    id_ubicacion_unidad = id_ubicacion_taller;
                            }
                        }
                    }
                }
                else
                    //Asignando Ubicación Actual
                    id_ubicacion_unidad = id_ubicacion_taller;
                
                //Validando Ubicación de la Unidad en caso de ser Propia
                if (id_ubicacion_unidad == id_ubicacion_taller)
                {
                    //Declarando Variable de Validación
                    bool validate = false;
                    decimal odometro_max = 0.00M;

                    //Si la Unidad es Interna
                    if (!bit_unidad_externa)
                    {
                        //Validando Tipo de Unidad
                        using (SAT_CL.Global.UnidadTipo tipo = new Global.UnidadTipo(id_tipo_unidad))
                        {
                            //Si la Unidad es Motriz
                            if (tipo.habilitar && tipo.bit_motriz)
                            {
                                //Obteniendo Odometro Máximo
                                odometro_max = ObtieneMaximoOdometroUnidad(id_unidad);

                                //Obteniendo Validación de odometro
                                validate = odometro > odometro_max ? true : false;
                            }
                            else
                                //Asignando Validación Positiva
                                validate = true;
                        }
                    }
                    else
                        //Asignando Validación Positiva
                        validate = true;

                    //Validando Resultado
                    if (validate)
                    
                        //Ejecutando SP
                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion(string.Format("El Valor del Odometro '{0:0.00}' debe ser mayor al Anterior '{1:0.00}'", odometro, odometro_max));

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Asignando Orden
                        idOrdenTrabajo = result.IdRegistro;

                        //Validando que no sea una Unidad Externa
                        if (!bit_unidad_externa)
                        {
                            //Instanciando Orden de Trabajo
                            using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(idOrdenTrabajo))
                            {
                                //Validando que exista la Orden de Trabajo
                                if (ot.habilitar)
                                {
                                    //Insertando Vencimiento
                                    result = SAT_CL.Global.Vencimiento.InsertaVencimientoOrdenTrabajo(19, ot.id_unidad, 1, "Orden de Trabajo No." + ot.no_orden_trabajo.ToString(),
                                                        Fecha.ObtieneFechaEstandarMexicoCentro(), DateTime.MinValue, 0, id_usuario);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Vencimiento
                                        int idVencimiento = result.IdRegistro;

                                        //Insertando Referencia de Vencimiento
                                        result = SAT_CL.Global.Referencia.InsertaReferencia(idOrdenTrabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Vencimiento", 0, "General"),
                                                        idVencimiento.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                                    }
                                }
                                else
                                    //Instanciando Orden
                                    result = new RetornoOperacion("No se puede Acceder a la Orden de Trabajo");
                            }
                        }
                        else
                            //Instanciando Orden
                            result = new RetornoOperacion(idOrdenTrabajo);
                    }
                }
                else
                    //Instanciando Orden
                    result = new RetornoOperacion("La Unidad no se encuentra en el Taller deseado.");

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Instanciando Registro
                    result = new RetornoOperacion(idOrdenTrabajo);

                    //Completando Transacción
                    trans.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Ordenes de Trabajo
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="fecha_compromiso">Fecha de Compromiso</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="bit_unidad_externa">Indicador de Unidad Externa</param>
        /// <param name="id_compania_cliente">Compania de Cliente</param>
        /// <param name="id_compania_proveedor">Compania de Proveedor</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad</param>
        /// <param name="id_unidad">Referencia de la Unidad</param>
        /// <param name="id_subtipo_unidad">Subtipo de la Unidad</param>
        /// <param name="descripcion_unidad">Descripción de la Unidad</param>
        /// <param name="id_tipo_taller">Tipo de Taller</param>
        /// <param name="id_ubicacion_taller">Ubicación del Taller</param>
        /// <param name="odometro">Odometro</param>
        /// <param name="nivel_combustible">Nivel de Combustible</param>
        /// <param name="no_siniestro">Número de Siniestro</param>
        /// <param name="entrega_unidad">Quien Entrega la Unidad</param>
        /// <param name="recibe_unidad">Quien Recibe la Unidad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaOrdenTrabajo(int id_compania_emisora, EstatusOrdenTrabajo estatus, TipoOrdenTrabajo tipo, DateTime fecha_recepcion, DateTime fecha_compromiso,
                    DateTime fecha_inicio, DateTime fecha_fin, bool bit_unidad_externa, int id_compania_cliente, int id_compania_proveedor,
                    byte id_tipo_unidad, int id_unidad, byte id_subtipo_unidad, string descripcion_unidad, byte id_tipo_taller, int id_ubicacion_taller,
                    decimal odometro, decimal nivel_combustible, string no_siniestro, string entrega_unidad, string recibe_unidad, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable de Validación
            bool validate = false;
            decimal odometro_max = 0.00M;

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que la Orden no se Encuentre Terminada
                if (this.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                {
                    //Si la Unidad es Interna
                    if (!bit_unidad_externa)
                    {
                        //Validando Tipo de Unidad
                        using (SAT_CL.Global.UnidadTipo tipoUnidad = new Global.UnidadTipo(id_tipo_unidad))
                        {
                            //Si la Unidad es Motriz
                            if (tipoUnidad.habilitar && tipoUnidad.bit_motriz)
                            {
                                //Obteniendo Odometro Máximo
                                odometro_max = ObtieneMaximoOdometroUnidad(id_unidad);

                                //Obteniendo Validación de odometro
                                validate = odometro > odometro_max ? true : false;
                            }
                            else
                                //Asignando Validación Positiva
                                validate = true;
                        }
                    }
                    else
                        //Asignando Validación Positiva
                        validate = true;

                    //Validando Odometro
                    if (validate)
                    {
                        //Obteniendo Requisiciones
                        using (DataTable dtRequisiciones = SAT_CL.Almacen.Requisicion.ObtieneRequisicionesOrdenTrabajo(this._id_orden_trabajo))
                        {
                            //Si existen Requisiciones Pendientes
                            if (Validacion.ValidaOrigenDatos(dtRequisiciones))

                                //Instanciando Excepción
                                result = new RetornoOperacion("La Orden de Trabajo tiene Requisiciones ligadas");
                            else
                                //Instanciando Orden de Trabajo
                                result = new RetornoOperacion(this._id_orden_trabajo);
                        }

                        //Validando que no existan Requisiciones
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Asignaciones
                            using (DataTable dtAsignaciones = SAT_CL.Mantenimiento.ActividadAsignacion.ObtieneAsignacionesOrdenTrabajo(this._id_orden_trabajo))
                            {
                                //Si existen Asignaciones Pendientes
                                if (Validacion.ValidaOrigenDatos(dtAsignaciones))

                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Orden de Trabajo tiene Asignaciones ligadas");
                                else
                                    //Instanciando Orden de Trabajo
                                    result = new RetornoOperacion(this._id_orden_trabajo);
                            }

                            //Validando que no existan Asignaciones
                            if (result.OperacionExitosa)
                            {
                                //Declarando Variable Auxiliar
                                int id_ubicacion_unidad = 0;

                                //Validando que sea una Unidad Externa
                                if (!bit_unidad_externa)
                                {
                                    //Instanciando unidad
                                    using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(id_unidad))
                                    {
                                        //Validando que este habilitado
                                        if (unidad.habilitar)
                                        {
                                            //Obteniendo Estancia
                                            using (SAT_CL.Despacho.EstanciaUnidad estancia = new SAT_CL.Despacho.EstanciaUnidad(unidad.id_estancia))
                                            using (SAT_CL.Despacho.Parada parada = new SAT_CL.Despacho.Parada(estancia.id_parada))
                                            {
                                                //Validando que exista
                                                if (estancia.habilitar && parada.habilitar)

                                                    //Asignando Ubicación de la Unidad
                                                    id_ubicacion_unidad = parada.id_ubicacion;
                                                else
                                                    //Asignando Ubicación Actual
                                                    id_ubicacion_unidad = id_ubicacion_taller;
                                            }
                                        }
                                    }
                                }
                                else
                                    //Asignando Ubicación Actual
                                    id_ubicacion_unidad = id_ubicacion_taller;

                                //Validando Ubicación de la Unidad en caso de ser Propia
                                if (id_ubicacion_unidad == id_ubicacion_taller)
                                {
                                    //Validando si se modificó de Unidad Interna a Externa
                                    if (this._bit_unidad_externa != bit_unidad_externa)
                                    {
                                        //Si la unidad es propia
                                        if (!this._bit_unidad_externa)
                                        {
                                            //Cargando Referencia de Vencimiento
                                            string idVencimiento = SAT_CL.Global.Referencia.CargaReferencia(this._id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Vencimiento", 0, "General"));

                                            //Obteniendo Referencias de la Orden de Trabajo
                                            using (SAT_CL.Global.Vencimiento vencimiento = new Global.Vencimiento(Convert.ToInt32(idVencimiento.Equals("") ? "0" : idVencimiento)))
                                            {
                                                //Validando que existan las Referencias
                                                if (vencimiento.habilitar)
                                                {
                                                    //Eliminando Vencimiento
                                                    result = vencimiento.DeshabilitaVencimiento(id_usuario);

                                                    //Validando Operación Exitosa
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Obteniendo Referencias
                                                        using (DataTable dtReferencias = SAT_CL.Global.Referencia.CargaReferencias(this._id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Vencimiento", 0, "General")))
                                                        {
                                                            //Validando que existan Referencias
                                                            if (Validacion.ValidaOrigenDatos(dtReferencias))
                                                            {
                                                                //Recorriendo Referencias
                                                                foreach (DataRow dr in dtReferencias.Rows)
                                                                {
                                                                    //Instanciando Referencia de Vencimiento
                                                                    using (SAT_CL.Global.Referencia ven = new Global.Referencia(Convert.ToInt32(dr["Id"])))
                                                                    {
                                                                        //Validando que exista
                                                                        if (ven.habilitar)

                                                                            //Eliminando Referencia
                                                                            result = SAT_CL.Global.Referencia.EliminaReferencia(ven.id_referencia, id_usuario);
                                                                        else
                                                                            //Instanciando Excepción
                                                                            result = new RetornoOperacion("No existe la Referencia de Vencimiento");

                                                                        //Validando si la Operación fue Incorrecta
                                                                        if (!result.OperacionExitosa)

                                                                            //Terminando Ciclo
                                                                            break;
                                                                    }
                                                                }

                                                                //Validando Operación Exitosa
                                                                if (result.OperacionExitosa)

                                                                    //Instanciando Orden de Trabajo
                                                                    result = new RetornoOperacion(this._id_orden_trabajo);
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion("No existe la Referencia de Vencimiento");
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No se encontro el Vencimiento de la Unidad");
                                            }
                                        }
                                        else
                                        {
                                            //Insertando Vencimiento
                                            result = SAT_CL.Global.Vencimiento.InsertaVencimiento(19, id_unidad, 1, 17, "Orden de Trabajo No." + no_orden_trabajo.ToString(),
                                                                Fecha.ObtieneFechaEstandarMexicoCentro(), DateTime.MinValue, 0, id_usuario);

                                            //Validando Operación Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Obteniendo Vencimiento
                                                int idVencimiento = result.IdRegistro;

                                                //Insertando Referencia de Vencimiento
                                                result = SAT_CL.Global.Referencia.InsertaReferencia(this._id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Vencimiento", 0, "General"),
                                                                idVencimiento.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Si la Unidad es propia
                                        if (!bit_unidad_externa)
                                        {
                                            //Validando que la Unidad sea distinta
                                            if (this._id_unidad != id_unidad)
                                            {
                                                //Cargando Referencia de Vencimiento
                                                string idVencimiento = SAT_CL.Global.Referencia.CargaReferencia(this._id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Vencimiento", 0, "General"));

                                                //Obteniendo Referencias de la Orden de Trabajo
                                                using (SAT_CL.Global.Vencimiento vencimiento = new Global.Vencimiento(Convert.ToInt32(idVencimiento.Equals("") ? "0" : idVencimiento)))
                                                {
                                                    //Validando que existan las Referencias
                                                    if (vencimiento.habilitar)

                                                        //Terminando Vencimiento
                                                        result = vencimiento.EditaVencimientoOrdenTrabajo(vencimiento.estatus, vencimiento.id_tabla, id_unidad, vencimiento.id_prioridad,
                                                                        vencimiento.id_tipo_vencimiento, vencimiento.descripcion, vencimiento.fecha_inicio, vencimiento.fecha_fin,
                                                                        vencimiento.valor_km, id_usuario);
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("No se encontro el Vencimiento de la Unidad");
                                                }
                                            }
                                            else
                                                //Instanciando Orden de Trabajo
                                                result = new RetornoOperacion(this._id_orden_trabajo);
                                        }
                                        else
                                            //Instanciando Orden de Trabajo
                                            result = new RetornoOperacion(this._id_orden_trabajo);
                                    }

                                    //Validando que no existan Asignaciones
                                    if (result.OperacionExitosa)
                                    {
                                        //Devolviendo Resultado Obtenido
                                        result = this.actualizaRegistrosBD(id_compania_emisora, (byte)estatus, (byte)tipo, fecha_recepcion, fecha_compromiso, fecha_inicio, fecha_fin,
                                                           bit_unidad_externa, id_compania_cliente, id_compania_proveedor, id_tipo_unidad, id_unidad, id_subtipo_unidad,
                                                           descripcion_unidad, id_tipo_taller, id_ubicacion_taller, odometro, nivel_combustible, no_siniestro, entrega_unidad,
                                                           recibe_unidad, id_usuario, this._habilitar);

                                        //Validando Operación Exitosa
                                        if (result.OperacionExitosa)

                                            //Completando Transacción
                                            trans.Complete();
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Unidad no se encuentra en el Taller");
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion(string.Format("El Valor del Odometro '{0:0.00}' debe ser mayor al Anterior '{1:0.00}'", odometro, odometro_max));
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Orden ha sido Terminada, Imposible su Edición");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Deshabilitar la Orden de Trabajo
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaOrdenTrabajo(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que la Orden no se Encuentre Terminada
            if (this.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Devolviendo Resultado Obtenido
                    result = this.actualizaRegistrosBD(this._id_compania_emisora, this._id_estatus, this._id_tipo, this._fecha_recepcion, this._fecha_compromiso, this._fecha_inicio, this._fecha_fin,
                                       this._bit_unidad_externa, this._id_compania_cliente, this._id_compania_proveedor, this._id_tipo_unidad, this._id_unidad, this._id_subtipo_unidad,
                                       this._descripcion_unidad, this._id_tipo_taller, this._id_ubicacion_taller, this._odometro, this._nivel_combustible, this._no_siniestro, this._entrega_unidad,
                                       this._recibe_unidad, id_usuario, false);

                    //Validando Operacion Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Validando si la Unidad es Externa o Interna
                        if (this._id_unidad > 0 && this.bit_unidad_externa == false)
                        {
                            //Cargando Referencia de Vencimiento
                            string idVencimiento = SAT_CL.Global.Referencia.CargaReferencia(this._id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Vencimiento", 0, "General"));

                            //Obteniendo Referencias de la Orden de Trabajo
                            using (SAT_CL.Global.Vencimiento vencimiento = new Global.Vencimiento(Convert.ToInt32(idVencimiento.Equals("") ? "0" : idVencimiento)))
                            {
                                //Validando que existan las Referencias
                                if (vencimiento.habilitar)

                                    //Terminando Vencimiento
                                    result = vencimiento.DeshabilitaVencimiento(id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se encontro el Vencimiento de la Unidad");
                            }
                        }
                        else
                            //Instanciando Orden de Trabajo
                            result = new RetornoOperacion(this._id_orden_trabajo);
                    }

                    //Validando Operaciones Exitosas
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Orden de Trabajo
                        result = new RetornoOperacion(this._id_orden_trabajo);

                        //Completando Transacción
                        trans.Complete();
                    }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("La Orden ha sido Terminada, Imposible su Eliminación");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar la Orden de Trabajo
        /// </summary>
        /// <returns></returns>
        public bool ActualizaOrdenTrabajo()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_orden_trabajo);
        }
        /// <summary>
        /// Método que carga las actividades ligadas a una orden de trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Id que serive como referencia para realizar la busqueda de actividades</param>
        /// <returns></returns>
        public static DataTable CargaActividadesOrdenTrabajo(int id_orden_trabajo)
        {
            //Creación de la tabla
            DataTable dtActividadOrdenTrabajo = null;
            //Creación del arreglo param
            object[] param = { 4, id_orden_trabajo, 0, 0, 0, 0, null, null, null, null, false, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, "", "", "", 0, false, "", "" }; 
            //Almacena en el DS el resultado del método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla loa valores del DS
                    dtActividadOrdenTrabajo = DS.Tables["Table"];

                //Devuelve el resultado al método
                return dtActividadOrdenTrabajo;
            }
        }
        /// <summary>
        /// Método encargado de Obtener el Valor Máximo del Odometro dada una Unidad
        /// </summary>
        /// <param name="id_unidad">Unidad Interna</param>
        /// <returns></returns>
        public static decimal ObtieneMaximoOdometroUnidad(int id_unidad)
        {
            //Declarando Objeto de Retorno
            decimal odometro = 0.00M;

            //Creación del arreglo param
            object[] param = { 8, 0, 0, 0, 0, 0, null, null, null, null, false, 0, 0, 0, id_unidad, 0, "", 0, 0, 0, 0, "", "", "", 0, false, "", "" }; ;
            //Almacena en el DS el resultado del método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in DS.Tables["Table"].Rows)
                    {
                        //Convirtiendo Valor a Decimal
                        odometro = Convert.ToDecimal(dr["Odometro"].ToString().Equals("") ? "0" : dr["Odometro"].ToString());

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return odometro;
        }
        /// <summary>
        /// Método que realiza la consulta del encabezado de la orden de trabajo para el formato de impresión de Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Id que permite identificar el registro de una orden de trabajo</param>
        /// <returns></returns>
        public static DataTable CargaEncabezadoOrdenTrabajo(int id_orden_trabajo)
        {
            //Creación de la tabla que almacenara los datos de la consulta
            DataTable dtEncabezadoOrdenTrabajo = null;
            //Creación del arreglo param
            object[] param = { 5, id_orden_trabajo, 0, 0, 0, 0, null, null, null, null, false, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, "", "", "", 0, false, "", "" };
            //Almacena el el dataset DS el resultado de invocar el método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna los valores del dataset DS a la tabla dtEncabezadoOrdenTrabajo
                    dtEncabezadoOrdenTrabajo = DS.Tables["Table"];

                //Devuelve el resultado al método
                return dtEncabezadoOrdenTrabajo;
            }
        }
        /// <summary>
        /// Método que realiza la consulta de los detalles de la orden de trabajo para el formato de impresión de Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Id que permite identificar el registro de una orden de trabajo</param>
        /// <returns></returns>
        public static DataTable CargaProductoOrdenTrabajo(int id_orden_trabajo)
        {
            //Creación de la tabla que almacenara los datos de la consulta
            DataTable dtEncabezadoOrdenTrabajo = null;
            //Creación del arreglo param
            object[] param = { 6, id_orden_trabajo, 0, 0, 0, 0, null, null, null, null, false, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, "", "", "", 0, false, "", "" };
            //Almacena el el dataset DS el resultado de invocar el método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna los valores del dataset DS a la tabla dtEncabezadoOrdenTrabajo
                    dtEncabezadoOrdenTrabajo = DS.Tables["Table"];

                //Devuelve el resultado al método
                return dtEncabezadoOrdenTrabajo;
            }
        }
        /// <summary>
        /// Método encargado de Terminar la Orden de Trabajo
        /// </summary>
        /// <param name="fecha_fin">Fecha de Termino</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion TerminaOrdenTrabajo(DateTime fecha_fin, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que la Orden no se Encuentre Terminada
            if (this.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Actualizando Registros
                    result = this.actualizaRegistrosBD(this._id_compania_emisora, (byte)EstatusOrdenTrabajo.Terminada, this._id_tipo, this._fecha_recepcion,
                                this._fecha_compromiso, this._fecha_inicio, fecha_fin, this._bit_unidad_externa, this._id_compania_cliente, this._id_compania_proveedor,
                                this._id_tipo_unidad, this._id_unidad, this._id_subtipo_unidad, this._descripcion_unidad, this._id_tipo_taller, this._id_ubicacion_taller,
                                this._odometro, this._nivel_combustible, this._no_siniestro, this._entrega_unidad, this._recibe_unidad, id_usuario, this._habilitar);

                    //Validando Operaciones
                    if (result.OperacionExitosa)
                    {
                        //Validando si la Unidad es Externa o Interna
                        if (this._id_unidad > 0 && this.bit_unidad_externa == false)
                        {
                            //Cargando Referencia de Vencimiento
                            string idVencimiento = SAT_CL.Global.Referencia.CargaReferencia(this._id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Vencimiento", 0, "General"));

                            //Obteniendo Referencias de la Orden de Trabajo
                            using (SAT_CL.Global.Vencimiento vencimiento = new Global.Vencimiento(Convert.ToInt32(idVencimiento.Equals("") ? "0" : idVencimiento)))
                            {
                                //Validando que existan las Referencias
                                if (vencimiento.habilitar)

                                    //Terminando Vencimiento
                                    result = vencimiento.TerminaVencimientoOrdenTrabajo(fecha_fin, id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se encontro el Vencimiento de la Unidad");
                            }
                        }
                        else
                            //Instanciando Orden de Trabajo
                            result = new RetornoOperacion(this._id_orden_trabajo);

                        //Validando Operación
                        if (result.OperacionExitosa)
                        {
                            //Declarando Validador de Requisiciones Pendientes
                            bool requisicion = false;

                            //Obteniendo Requisiciones
                            using (DataTable dtRequisiciones = SAT_CL.Mantenimiento.Reportes.ObtieneRequisicionesOrdenTrabajo(this._id_orden_trabajo))
                            {
                                //Si existen Requisiciones Pendientes
                                if (Validacion.ValidaOrigenDatos(dtRequisiciones))
                                    //Si no hay Requisiciones Pendientes
                                    requisicion = false;
                                else
                                    //Si no hay Requisiciones Pendientes
                                    requisicion = true;
                            }

                            //Validando si no existen Requisiciones Pendientes
                            if (requisicion)
                            {
                                //Obteniendo Actividades de la Orden de Trabajo
                                using (DataTable dtActividades = SAT_CL.Mantenimiento.OrdenTrabajoActividad.CargaOrdenTrabajoActividades(this._id_orden_trabajo))
                                {
                                    //Validando que existan Actividades
                                    if (Validacion.ValidaOrigenDatos(dtActividades))
                                    {
                                        //Recorriendo Actividades
                                        foreach (DataRow dr in dtActividades.Rows)
                                        {
                                            //Instanciando Actividad
                                            using (SAT_CL.Mantenimiento.OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(Convert.ToInt32(dr["Id"])))
                                            {
                                                //Validando que Exista la Actividad
                                                if (actividad.habilitar)
                                                {
                                                    //Terminando Actividad
                                                    result = actividad.TerminaActividadAsignaciones(fecha_fin, id_usuario);

                                                    //Si la Operación no fue Exitosa
                                                    if (!result.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No se puede Acceder a la Actividad");
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Orden de Trabajo
                                        result = new RetornoOperacion(this._id_orden_trabajo);
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("Existen Requisiciones Pendientes de la Orden");
                        }
                    }

                    //Actualizando Precio de Salida del Inventario en las Requisiciones ligadas a la Orden de Trabajo
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Requisiciones
                        using (DataTable dtRequisiciones = SAT_CL.Almacen.Requisicion.ObtieneRequisicionesOrdenTrabajo(this._id_orden_trabajo))
                        {
                            //Validando que existan Requisiciones
                            if (Validacion.ValidaOrigenDatos(dtRequisiciones))
                            {
                                //Recorriendo Requisiciones
                                foreach (DataRow dr in dtRequisiciones.Rows)
                                {
                                    //Instanciando Requisición
                                    using (SAT_CL.Almacen.Requisicion req = new Almacen.Requisicion(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista la Requisición
                                        if (req.habilitar)
                                        {
                                            //Obteniendo Detalles
                                            using (DataTable dtDetalles = Almacen.RequisicionDetalle.ObtieneDetallesRequisicion(req.id_requisicion))
                                            {
                                                //Validando que existan Requisiciones
                                                if (Validacion.ValidaOrigenDatos(dtDetalles))
                                                {
                                                    //Recorriendo Detalles
                                                    foreach (DataRow drt in dtDetalles.Rows)
                                                    {
                                                        //Instanciando Detalle de Requisición
                                                        using (Almacen.RequisicionDetalle rd = new Almacen.RequisicionDetalle(Convert.ToInt32(drt["NoDetalle"])))
                                                        {
                                                            //Validando que exista el Detalle
                                                            if (rd.Habilitar)
                                                            {
                                                                //Obteniendo Registros de Inventario
                                                                using (DataTable dtInventario = Almacen.Inventario.ObtieneInventarioRequisicionDetalle(rd.IdDetalleRequisicion))
                                                                {
                                                                    //Validando que existan Registros de Salida en el Inventario
                                                                    if (Validacion.ValidaOrigenDatos(dtInventario))
                                                                    {
                                                                        //Recorriendo Registros de Inventario
                                                                        foreach (DataRow dri in dtInventario.Rows)
                                                                        {
                                                                            //Instanciando Inventario
                                                                            using (Almacen.Inventario inv = new Almacen.Inventario(Convert.ToInt32(dri["Id"])))
                                                                            {
                                                                                //Validando que exista el Registro de Inventario
                                                                                if (inv.habilitar)
                                                                                {
                                                                                    //Instanciando Producto
                                                                                    using (Almacen.Producto prod = new Almacen.Producto(inv.id_producto))
                                                                                    {
                                                                                        //Validando que exista el Producto
                                                                                        if (prod.habilitar)

                                                                                            //Actualizando Precio de Salida del Inventario
                                                                                            result = inv.ActualizarPrecioSalida(prod.precio_salida, id_usuario);
                                                                                        else
                                                                                            //Instanciando Excepción
                                                                                            result = new RetornoOperacion("No se puede acceder al Producto Solicitado");
                                                                                    }
                                                                                }
                                                                                else
                                                                                    //Instanciando Excepción
                                                                                    result = new RetornoOperacion("No se puede acceder al registro de Inventario");
                                                                            }

                                                                            //Si la Operación no fue Exitosa
                                                                            if (!(result.OperacionExitosa))

                                                                                //Terminando Ciclo
                                                                                break;
                                                                        }
                                                                    }
                                                                    else
                                                                        //Instanciando Excepción
                                                                        result = new RetornoOperacion("No existen registros del Inventario");
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion("No existe la Requisición");

                                                            //Si la Operación no fue Exitosa
                                                            if (!(result.OperacionExitosa))

                                                                //Terminando Ciclo
                                                                break;
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No existen Detalles de la Requisición");
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No existe la Requisición");

                                        //Si la Operación no fue Exitosa
                                        if (!(result.OperacionExitosa))

                                            //Terminando Ciclo
                                            break;
                                    }
                                }
                            }
                        }

                        //Validando Operaciones
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Fecha
                            DateTime fecha_asignacion = DateTime.MinValue;
                            fecha_asignacion = ActividadAsignacion.ObtieneUltimaFechaAsignacionOrdenTrabajo(this._id_orden_trabajo);

                            //Validando que no exista una fecha de Asignación ó que la fecha de fin sea mayor la fecha de asignación
                            if (fecha_asignacion == DateTime.MinValue || fecha_fin >= fecha_asignacion)
                            {
                                //Instanciando Orden de Trabajo
                                result = new RetornoOperacion(this._id_orden_trabajo);

                                //Completando Transacción
                                trans.Complete();
                            }
                            else
                                //Instanciando Orden de Trabajo
                                result = new RetornoOperacion("La Fecha de Fin, debe ser mayor a la Fecha de la Ultima Asignación");
                        }
                    }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("La Orden de Trabajo se encuentra Terminada.");

            //Devolviendo Resultado Obtenido
            return result;
        }

          /// <summary>
        /// Método encargado de Actualizar la Fecha de Inicio de la Orden de Trabajo
        /// </summary>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaFechaInicioOrdenTrabajo(DateTime fecha_inicio, int id_usuario)
        {
            //Actualizando Registros
            return  this.actualizaRegistrosBD(this._id_compania_emisora, this._id_estatus, this._id_tipo, this._fecha_recepcion,
                        this._fecha_compromiso, fecha_inicio, this._fecha_fin, this._bit_unidad_externa, this._id_compania_cliente, this._id_compania_proveedor,
                        this._id_tipo_unidad, this._id_unidad, this._id_subtipo_unidad, this._descripcion_unidad, this._id_tipo_taller, this._id_ubicacion_taller,
                        this._odometro, this._nivel_combustible, this._no_siniestro, this._entrega_unidad, this._recibe_unidad, id_usuario, this._habilitar);

        }
        /// <summary>
        /// Método encargado de Facturar la Orden de Trabajo
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion FacturaOrdenTrabajo(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Declarando variables Auxiliares
            int idFacturaOtros = 0;
            int idFactura = 0;

            //Validando que la Unidad sea externa
            if (this._bit_unidad_externa)
            {
                //Validando que la Orden de Trabajo este Terminada
                if (this.EstatusOrden == EstatusOrdenTrabajo.Terminada)
                {
                    //Bloque transaccional
                    using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Obteniendo Resultado de la Inserción
                        result = SAT_CL.Facturacion.Facturado.InsertaFactura(0, Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 1, 1, id_usuario);

                        //Validando que se inserto la Factura
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Factura
                            idFactura = result.IdRegistro;

                            //Insertando Encabezado de Facturación
                            result = SAT_CL.Facturacion.FacturacionOtros.InsertaFacturacionOtros(idFactura,
                                            this._id_compania_emisora, this._id_compania_cliente, id_usuario);

                            //Validando que se inserto la Factura
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Facturación Otros
                                idFacturaOtros = result.IdRegistro;

                                //Instanciando Conceptos
                                using (SAT_CL.Tarifas.TipoCargo mano_obra = SAT_CL.Tarifas.TipoCargo.ObtieneTipoCargoDescripcion(this._id_compania_emisora,
                                                                            "Mano de Obra", 0),
                                                                refacciones = SAT_CL.Tarifas.TipoCargo.ObtieneTipoCargoDescripcion(this._id_compania_emisora,
                                                                            "Refacciones", 0))
                                {
                                    //Validando que existan los Conceptos de Cobro
                                    if (mano_obra.habilitar && refacciones.habilitar)
                                    {
                                        //Obteniendo Total de Mano de Obra
                                        decimal total_mano_obra = ActividadAsignacion.ObtieneMontoTotalEmpleados(this.id_orden_trabajo);
                                        decimal total_refacciones = Almacen.Requisicion.ObtieneTotalRefaccionRequisicion(this._id_orden_trabajo);

                                        //Insertando Concepto de Mano de Obra
                                        result = SAT_CL.Facturacion.FacturadoConcepto.InsertaFacturaConcepto(idFactura, 1, mano_obra.id_unidad, "", mano_obra.id_tipo_cargo,
                                                total_mano_obra, mano_obra.id_tipo_impuesto_retenido, mano_obra.tasa_impuesto_retenido, mano_obra.id_tipo_impuesto_trasladado,
                                                mano_obra.tasa_impuesto_trasladado, mano_obra.tipo_cargo, id_usuario);

                                        //Validando que se Inserto el Concepto de Mano de Obra
                                        if (result.OperacionExitosa)
                                        {
                                            //Insertando Concepto de Mano de Obra
                                            result = SAT_CL.Facturacion.FacturadoConcepto.InsertaFacturaConcepto(idFactura, 1, refacciones.id_unidad, "", refacciones.id_tipo_cargo,
                                                    total_refacciones, refacciones.id_tipo_impuesto_retenido, refacciones.tasa_impuesto_retenido, refacciones.id_tipo_impuesto_trasladado,
                                                    refacciones.tasa_impuesto_trasladado, refacciones.tipo_cargo, id_usuario);

                                            //Validando que se Inserto el Concepto de Refacciones
                                            if (result.OperacionExitosa)
                                            {
                                                //Insertando Referencia de Facturación
                                                result = SAT_CL.Global.Referencia.InsertaReferencia(this._id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Factura", 0, "General"),
                                                            idFacturaOtros.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("Los Conceptos de Cobro no estan registrados para esta compania");
                                }
                            }
                        }

                        //Validando que se inserto la Factura
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Orden de Trabajo
                            result = new RetornoOperacion(this._id_orden_trabajo);

                            //Completando Transacción
                            trans.Complete();
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Orden de Trabajo, tiene que estar Terminada para su Facturación");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No puede Facturar la Orden de una Unidad Propia");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargadp de cargar las asignaciones de las Actividades de la Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Id que permite identificar el registro de una orden de trabajo</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesOrdenTrabajo(int id_orden_trabajo)
        {
            //Creación de la tabla que almacenara los datos de la consulta
            DataTable dtAsignacionesOrdenTrabajo = null;
            //Creación del arreglo param
            object[] param = { 7, id_orden_trabajo, 0, 0, 0, 0, null, null, null, null, false, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, "", "", "", 0, false, "", "" };
            //Almacena el el dataset DS el resultado de invocar el método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna los valores del dataset DS a la tabla dtEncabezadoOrdenTrabajo
                    dtAsignacionesOrdenTrabajo = DS.Tables["Table"];

                //Devuelve el resultado al método
                return dtAsignacionesOrdenTrabajo;
            }
        }
        /// <summary>
        /// Método encargado de Obtener las Facturas Ligadas a la Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturaProveedorOrdenTrabajo(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = new DataTable();

            //Creación del arreglo param
            object[] param = { 9, id_orden_trabajo, 0, 0, 0, 0, null, null, null, null, false, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, "", "", "", 0, false, "", "" };
            
            //Almacena el el dataset DS el resultado de invocar el método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asignando Resultado Obtenido
                    dtFacturas = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacturas;
        }

        #endregion
    }
}
