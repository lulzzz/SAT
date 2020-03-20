using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using SAT_CL.Tarifas;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Documentacion;
using SAT_CL.Global;
using TSDK.Datos;
using System.Transactions;

namespace SAT_CL.Facturacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Procesos de los Paquetes
    /// </summary>
    public class PaqueteProceso : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Expresa los Tipos de Proceso
        /// </summary>
        public enum TipoProceso
        {
            /// <summary>
            /// Proceso de Revisión
            /// </summary>
            Revision = 1,
            /// <summary>
            /// Proceso de Liberación
            /// </summary>
            Liberacion,
            /// <summary>
            /// Proceso de Liquidación
            /// </summary>
            Liquidacion
        }
        /// <summary>
        /// Expresa los Estatus del Proceso
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Entregado
            /// </summary>
            Entregado,
            /// <summary>
            /// Aceptado
            /// </summary>
            Aceptado,
            /// <summary>
            /// Rechazado
            /// </summary>
            Rechazado,
            /// <summary>
            /// Terminado
            /// </summary>
            Terminado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo que almacena el Nombre del SP
        /// </summary>
        private static string _nom_sp = "facturacion.sp_paquete_proceso_tpp";

        private int _id_paquete_proceso;
        /// <summary>
        /// Atributo que almacena el Id del Proceso del Paquete
        /// </summary>
        public int id_paquete_proceso { get { return this._id_paquete_proceso; } }
        private byte _id_tipo_proceso;
        /// <summary>
        /// Atributo que almacena el Tipo de Proceso
        /// </summary>
        public byte id_tipo_proceso { get { return this._id_tipo_proceso; } }
        /// <summary>
        /// Atributo que almacena el Tipo de Proceso (Enumeración)
        /// </summary>
        public TipoProceso tipo_proceso { get { return (TipoProceso)this._id_tipo_proceso; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo que almacena el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo que almacena el Estatus (Enumeración)
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private int _id_compania;
        /// <summary>
        /// Atributo que almacena la Compania Emisora
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private int _consecutivo_compania;
        /// <summary>
        /// Atributo que almacena el No. Consecutivo de la Compania
        /// </summary>
        public int consecutivo_compania { get { return this._consecutivo_compania; } }
        private int _id_cliente;
        /// <summary>
        /// Atributo que almacena el Cliente Receptor
        /// </summary>
        public int id_cliente { get { return this._id_cliente; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo que almacena la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo que almacena la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private int _id_usuario_responsable;
        /// <summary>
        /// Atributo que almacena el Usuario Responsable
        /// </summary>
        public int id_usuario_responsable { get { return this._id_usuario_responsable; } }
        private string _referencia;
        /// <summary>
        /// Atributo que almacena la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
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
        public PaqueteProceso()
        {
            //Asignando Valores
            this._id_paquete_proceso = 0;
            this._id_tipo_proceso = 0;
            this._id_estatus = 0;
            this._id_compania = 0;
            this._consecutivo_compania = 0;
            this._id_cliente = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._id_usuario_responsable = 0;
            this._referencia = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_paquete_proceso">Proceso de Paquete</param>
        public PaqueteProceso(int id_paquete_proceso)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_paquete_proceso);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~PaqueteProceso()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos de la Clase dado un Registro
        /// </summary>
        /// <param name="id_paquete_proceso">Proceso de Paquete</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_paquete_proceso)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_paquete_proceso, 0, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada uno de los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_paquete_proceso = id_paquete_proceso;
                        this._id_tipo_proceso = Convert.ToByte(dr["IdTipoProceso"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._consecutivo_compania = Convert.ToInt32(dr["ConsecutivoCompania"]);
                        this._id_cliente = Convert.ToInt32(dr["IdCliente"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        this._id_usuario_responsable = Convert.ToInt32(dr["IdUsuarioResponsable"]);
                        this._referencia = dr["Referencia"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="tipo_proceso">Tipo de Proceso</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="consecutivo_compania">Consecutivo por Compania</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario_responsable">Usuario Responsable del Proceso</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(TipoProceso tipo_proceso, Estatus estatus, int id_compania, int consecutivo_compania, 
                                    int id_cliente, DateTime fecha_inicio, DateTime fecha_fin, int id_usuario_responsable, string referencia, 
                                    int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_paquete_proceso, tipo_proceso, estatus, id_compania, consecutivo_compania, id_cliente, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               id_usuario_responsable, referencia, id_usuario, habilitar, "", "" };

            //Realizando Actualización
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Procesos de Paquete
        /// </summary>
        /// <param name="tipo_proceso">Tipo de Proceso</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="consecutivo_compania">Consecutivo por Compania</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario_responsable">Usuario Responsable del Proceso</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPaqueteProceso(TipoProceso tipo_proceso, int id_compania, int consecutivo_compania,
                                    int id_cliente, DateTime fecha_inicio, DateTime fecha_fin, int id_usuario_responsable, string referencia,
                                    int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, (byte)tipo_proceso, (byte)Estatus.Registrado, id_compania, consecutivo_compania, id_cliente, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               id_usuario_responsable, referencia, id_usuario, true, "", "" };

            //Realizando Actualización
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Procesos de Paquetes
        /// </summary>
        /// <param name="tipo_proceso">Tipo de Proceso</param>
        /// <param name="estatus">Estatus del Proceso</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="consecutivo_compania">Consecutivo por Compania</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario_responsable">Usuario Responsable del Proceso</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaPaqueteProceso(TipoProceso tipo_proceso, Estatus estatus, int id_compania, int consecutivo_compania,
                                    int id_cliente, DateTime fecha_inicio, DateTime fecha_fin, int id_usuario_responsable, string referencia,
                                    int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(tipo_proceso, estatus, id_compania, consecutivo_compania, id_cliente, fecha_inicio,
                               fecha_fin, id_usuario_responsable, referencia, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar el Proceso del Paquete
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPaqueteProceso(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Deshabilitando Registro
                result = this.actualizaRegistros((TipoProceso)this._id_tipo_proceso, (Estatus)this._id_estatus, this._id_compania, this._consecutivo_compania, this._id_cliente, this._fecha_inicio,
                               this._fecha_fin, this._id_usuario_responsable, this._referencia, id_usuario, false);

                //Validando Operación
                if (result.OperacionExitosa)
                {
                    //Obteniendo Paquete
                    int id_paquete = result.IdRegistro;
                    
                    //Obteniendo Detalles
                    using (DataTable dtDetallesPaquete = SAT_CL.Facturacion.PaqueteProcesoDetalle.ObtieneFacturacionPaqueteProceso(this._id_paquete_proceso))
                    {
                        //Validando que Existan Detalles
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesPaquete))
                        {
                            //Recorriendo Detalles
                            foreach (DataRow dr in dtDetallesPaquete.Rows)
                            {
                                //Instanciando Paquete
                                using (PaqueteProcesoDetalle ppd = new PaqueteProcesoDetalle(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando que Exista el Detalle
                                    if (ppd.id_paquete_proceso_detalle > 0)
                                    {
                                        //Deshabilitando Detalle
                                        result = ppd.DeshabilitaPaqueteProcesoDetalle(id_usuario);

                                        //Validando que la Operación no se haya Cumplido
                                        if (!result.OperacionExitosa)

                                            //Terminando Ciclo
                                            break;
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No Existe el Detalle");
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion(id_paquete);
                    }

                    //Validando Operación
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Registro
                        result = new RetornoOperacion(id_paquete);

                        //Completando Transacción
                        trans.Complete();
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Deshabilitar el Proceso del Paquete
        /// </summary>
        /// <param name="estatus">Estatus del Proceso</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusPaqueteProceso(Estatus estatus, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistros((TipoProceso)this._id_tipo_proceso, estatus, this._id_compania, this._consecutivo_compania, this._id_cliente, this._fecha_inicio,
                               TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), this._id_usuario_responsable, this._referencia, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPaqueteProceso()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_paquete_proceso);
        }
        /// <summary>
        /// Método encargado de Obtener los Datos del Proceso del Paquete
        /// </summary>
        /// <param name="id_paquete_proceso">Paquete</param>
        /// <returns></returns>
        public static DataSet ObtieneDatosPaquete(int id_paquete_proceso)
        {
            //Armando Arreglo de Parametros
            object[] param = { 4, id_paquete_proceso, 0, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            
                //Devolviendo Objeto de Retorno
                return ds;
        }

        /// <summary>
        /// Método encargado de Obtener el Reporte de Procter(Rechazos y Devoluciones de Servicios)
        /// </summary>
        /// <param name="id_paquete_proceso">Paquete</param>
        /// <returns></returns>
        public static DataSet ReportePROCTER(int id_paquete_proceso)
        {
            //Armando Arreglo de Parametros
            object[] param = { 5, id_paquete_proceso, 0, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))

                //Devolviendo Objeto de Retorno
                return ds;
        }
        /// <summary>
        /// Método que obtiene  el reporte de rechazos para el cliente colgate.
        /// </summary>
        /// <param name="id_paquete_proceso"></param>
        /// <returns></returns>
        public static DataSet ReporteColgate(int id_paquete_proceso)
        {
            //Creación del objeto param que almacena los parametros para la consulta de datos a la tabla PaqueteProceso
            object[] param = { 6, id_paquete_proceso, 0, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
            //Invoca al método EjecutaProcAlmacenadoDataSet y el resultado lo almacena el el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))            
                //Retorna el dataset DS al método
                return DS;                
        }
        /// <summary>
        /// Método que obtiene  el reporte de Schindler.
        /// </summary>
        /// <param name="id_paquete_proceso"></param>
        /// <returns></returns>
        public static DataSet ReporteSchindler(int id_paquete_proceso)
        {
            //Creación del objeto param que almacena los parametros para la consulta de datos a la tabla PaqueteProceso
            object[] param = { 7, id_paquete_proceso, 0, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
            //Invoca al método EjecutaProcAlmacenadoDataSet y el resultado lo almacena el el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                //Retorna el dataset DS al método
                return DS;
        }
        /// <summary>
        /// Método que obtiene  el reporte de ABC.
        /// </summary>
        /// <param name="id_paquete_proceso"></param>
        /// <returns></returns>
        public static DataTable ReporteABC(int id_paquete_proceso)
        {
            //Creación de la tabla
            DataTable dtReporteABC = null;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta de un paquete de proceso para el cliente ABC
            object[] param = { 8, id_paquete_proceso, 0, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
            //Invoca al método EjecutaProcAlmacenadoDataSet y el resultado lo almacena el el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna los valores del dataset a la tabla
                    dtReporteABC = DS.Tables["Table"];
            }
            //
                //Retorna el dataset DS al método
            return dtReporteABC;
        }
        /// <summary>
        /// Método que obtiene  el reporte de lili.
        /// </summary>
        /// <param name="id_paquete_proceso"></param>
        /// <returns></returns>
        public static DataTable Reportelili(int id_paquete_proceso)
        {
            //Creación de la tabla
            DataTable dtReporteLili = null;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta de un paquete de proceso para el cliente lili
            object[] param = { 9, id_paquete_proceso, 0, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
            //Invoca al método EjecutaProcAlmacenadoDataSet y el resultado lo almacena el el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna los valores del dataset a la tabla
                    dtReporteLili = DS.Tables["Table"];
            }
                //Retorna el dataset DS al método
            return dtReporteLili;
        }
        #endregion
    }
}
