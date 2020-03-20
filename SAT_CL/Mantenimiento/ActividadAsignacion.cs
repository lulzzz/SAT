using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using System.Linq;
using System.Collections;
using TSDK.Datos;

namespace SAT_CL.Mantenimiento
{
    public class ActividadAsignacion : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Especifica el estatus de la asignacion
        /// </summary>
        public enum EstatusAsignacionActividad
        {
            /// <summary>
            /// Estatus inicial de la asignacion
            /// </summary>
            Asignada = 1,
            /// <summary>
            /// La asignacion esta en proceso
            /// </summary>
            Iniciada,
            /// <summary>
            /// La asignacion se encuentra pausada
            /// </summary>
            Pausada,
            /// <summary>
            /// Las asignacion ha sido terminada
            /// </summary>
            Terminada,
            /// <summary>
            /// Las asignacion ha sido Cancelada
            /// </summary>
            Cancelada
        }
        /// <summary>
        /// Especifica el tipo de asignacion de la actividad
        /// </summary>
        public enum TipoAsignacionActividad
        {
            /// <summary>
            /// Se asigna a un empleado
            /// </summary>
            AsignacionInterna = 1,
            /// <summary>
            /// Se asigna a un proveedor
            /// </summary>
            AsignacionExterna
        }
        #endregion

        #region Propiedades y atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string nombre_store_procedure = "mantenimiento.sp_actividad_asignacion_taa";

        private int _id_asignacion_actividad;
        private int _id_orden_actividad;
        private int _id_tipo;
        private int _id_estatus;
        private int _id_empleado;
        private int _id_proveedor;
        private DateTime _inicio_asignacion;
        private DateTime _fin_asignacion;
        private int _duracion;
        private int _duracion_real;
        private bool _habilitar;
        private DateTime _fecha_periodo_minima;
        private DateTime _fecha_periodo_maxima;
        private int _periodos_abiertos;

        public int id_asignacion_actividad { get { return _id_asignacion_actividad; } }
        public int id_orden_actividad { get { return _id_orden_actividad; } }
        public int id_tipo { get { return _id_tipo; } }
        public TipoAsignacionActividad Tipo { get { return (TipoAsignacionActividad)_id_tipo; } }
        public int id_estatus { get { return _id_estatus; } }
        public EstatusAsignacionActividad Estatus { get { return (EstatusAsignacionActividad)_id_estatus; } }
        public int id_empleado { get { return _id_empleado; } }
        public int id_proveedor { get { return _id_proveedor; } }
        public DateTime inicio_asignacion { get { return _inicio_asignacion; } }
        public DateTime fin_asignacion { get { return _fin_asignacion; } }
        public int duracion { get { return _duracion; } }
        public int duracion_real { get { return this._duracion_real; } }
        public bool habilitar { get { return _habilitar; } }
        public DateTime fecha_periodo_minima { get { return _fecha_periodo_minima; } }
        public DateTime fecha_periodo_maxima { get { return _fecha_periodo_maxima; } }
        public int periodos_abiertos { get { return _periodos_abiertos; } }

        #endregion

        #region Constructores

        //Constructor default
        public ActividadAsignacion()
        {
            _id_asignacion_actividad = 0;
            _id_orden_actividad = 0;
            _id_tipo = 0;
            _id_estatus = 0;
            _id_empleado = 0;
            _id_proveedor = 0;
            _inicio_asignacion = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _fin_asignacion = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _duracion = 0;
            _habilitar = false;
            _fecha_periodo_maxima = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _fecha_periodo_minima = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _periodos_abiertos = 0;
        }

        /// <summary>
        /// Constructor que inicializa la instancia en relacion al id de la asignación de actividad
        /// </summary>
        /// <param name="IdActividadAsignacion"></param>
        public ActividadAsignacion(int IdActividadAsignacion)
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, IdActividadAsignacion, 0, 0, 0, 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_asignacion_actividad = Convert.ToInt32(r["IdAsignacionActividad"]);
                        this._id_orden_actividad = Convert.ToInt32(r["IdOrdenActividad"]);
                        this._id_tipo = Convert.ToInt32(r["IdTipo"]);
                        this._id_estatus = Convert.ToInt32(r["IdEstatus"]);
                        this._id_empleado = Convert.ToInt32(r["IdEmpleado"]);
                        this._id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        DateTime.TryParse(r["InicioAsignacion"].ToString(), out _inicio_asignacion);
                        DateTime.TryParse(r["FinAsignacion"].ToString(), out _fin_asignacion);
                        this._duracion = Convert.ToInt32(r["Duracion"]);
                        this._duracion_real = Convert.ToInt32(r["DuracionReal"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                        DateTime.TryParse(r["PeriodoMin"].ToString(), out _fecha_periodo_minima);
                        DateTime.TryParse(r["PeriodoMax"].ToString(), out _fecha_periodo_maxima);
                        this._periodos_abiertos = Convert.ToInt32(r["PeriodosAbiertos"]);

                    }
                }
            }
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~ActividadAsignacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados


        /// <summary>
        /// Metodo encargado de editar un registro asignacion de actividad
        /// </summary>
        /// <param name="id_orden_actividad"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_empleado"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="inicio_asignacion"></param>
        /// <param name="fin_asignacion"></param>
        /// <param name="duracion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaRegistroActividadAsignacion(int id_orden_actividad, int id_tipo, int id_estatus, int id_empleado, int id_proveedor, DateTime inicio_asignacion, DateTime fin_asignacion,
            int duracion, int id_usuario, bool habilitar)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la actividad a la que corresponde
            using (OrdenTrabajoActividad a = new OrdenTrabajoActividad(this._id_orden_actividad))
            {
                //Si el estatus de la actividad es distinto de terminado
                if (a.EstatusActividad != OrdenTrabajoActividad.EstatusOrdenActividad.Terminada)
                {
                    //Si el estatus propio es distinto de terminado
                    if (this.Estatus != EstatusAsignacionActividad.Terminada)
                    {
                        //Inicializando arreglo de parámetros
                        object[] param = { 2, this.id_asignacion_actividad, id_orden_actividad, id_tipo, id_estatus, id_empleado, id_proveedor, Fecha.ConvierteDateTimeObjeto(inicio_asignacion), Fecha.ConvierteDateTimeObjeto(fin_asignacion), duracion, id_usuario, habilitar, "", "" };

                        //Realizando actualizacion
                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);
                    }
                    //Si ya se terminó la asignación
                    else
                        //Personalizando error
                        resultado = new RetornoOperacion("El estatus de la Asignación no permite su edición.");
                }
                //De lo contrario
                else
                    resultado = new RetornoOperacion("El estatus de la Actividad no permite su edición.");
            }

            //Devolvinedo valor
            return resultado;
        }

        #endregion

        #region Metodos publicos (Interfaz)

        /// <summary>
        /// Realiza la recarga de valores para atributos desde la BD
        /// </summary>
        public void RecargaAtributosInstancia()
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, this._id_asignacion_actividad, 0, 0, 0, 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_asignacion_actividad = Convert.ToInt32(r["IdAsignacionActividad"]);
                        this._id_orden_actividad = Convert.ToInt32(r["IdOrdenActividad"]);
                        this._id_tipo = Convert.ToInt32(r["IdTipo"]);
                        this._id_estatus = Convert.ToInt32(r["IdEstatus"]);
                        this._id_empleado = Convert.ToInt32(r["IdEmpleado"]);
                        this._id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        this._inicio_asignacion = Convert.ToDateTime(r["InicioAsignacion"]);
                        DateTime.TryParse(r["FinAsignacion"].ToString(), out _fin_asignacion);
                        this._duracion = Convert.ToInt32(r["Duracion"]);
                        this._duracion_real = Convert.ToInt32(r["DuracionReal"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                        DateTime.TryParse(r["PeriodoMin"].ToString(), out _fecha_periodo_minima);
                        DateTime.TryParse(r["PeriodoMax"].ToString(), out _fecha_periodo_maxima);
                        this._periodos_abiertos = Convert.ToInt32(r["PeriodosAbiertos"]);

                    }
                }
            }
        }
        /// <summary>
        /// Metodo encargado de insertar un registro asignacion de actividad
        /// </summary>
        /// <param name="id_orden_actividad"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_empleado"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="inicio_asignacion"></param>
        /// <param name="fin_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaActividadAsignacion(int id_orden_actividad, TipoAsignacionActividad id_tipo, EstatusAsignacionActividad id_estatus, int id_empleado, int id_proveedor, DateTime inicio_asignacion, DateTime fin_asignacion, int id_usuario)
        {
            //Objeto resultado
            RetornoOperacion retorno = new RetornoOperacion();
            int idAsignacion = 0;
            //Instanciamos la actividad 
            using (OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(id_orden_actividad))
            {
                //Si el estatus de la actividad es distinto de terminado
                if (actividad.EstatusActividad != OrdenTrabajoActividad.EstatusOrdenActividad.Terminada)
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Inicializando arreglo de parámetros
                        object[] param = { 1, 0, id_orden_actividad, (int)id_tipo, (int)id_estatus, id_empleado, id_proveedor, Fecha.ConvierteDateTimeObjeto(inicio_asignacion), Fecha.ConvierteDateTimeObjeto(fin_asignacion), 0, id_usuario, true, "", "" };

                        //Realizamos la inserción del registro
                        retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);

                        //Se alamacena Id de asignación
                        idAsignacion = retorno.IdRegistro;

                        //Si ha sido exitosa la actualizacion
                        if (retorno.OperacionExitosa)
                        {
                            //Realizamos la actualizacion del estatus de la actividad
                            retorno = actividad.AsignaEstatusOrdenActividad(id_usuario, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro());

                            //Si se ha realizado una actualización correcta
                            if (retorno.OperacionExitosa)
                            {
                                //Asignando Valor
                                retorno = new RetornoOperacion(idAsignacion);

                                //Completando Transacción
                                trans.Complete();
                            }

                        }
                    }
                }
                //Si ya se encuentra terminada su actividad
                else
                    retorno = new RetornoOperacion("El estatus de la Actividad no permite su edición.");
            }

            //Retornamos el resultado
            return retorno;
        }
        /// <summary>
        /// Metodo encargado de editar un registro orden de trabajo
        /// </summary>
        /// <param name="id_orden_actividad"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_empleado"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="inicio_asignacion"></param>
        /// <param name="fin_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaRegistroActividadAsignacion(int id_orden_actividad, TipoAsignacionActividad id_tipo, EstatusAsignacionActividad id_estatus, int id_empleado, int id_proveedor, DateTime inicio_asignacion, DateTime fin_asignacion, int id_usuario)
        {
            //Validando Estatus
            if (this.Estatus == EstatusAsignacionActividad.Asignada)

                //Invocando Método de Actualización
                return this.editaRegistroActividadAsignacion(id_orden_actividad, (int)id_tipo, (int)id_estatus, id_empleado, id_proveedor, inicio_asignacion, fin_asignacion, 0,id_usuario, this.habilitar);
            else
                //Instanciando Excepción
                return new RetornoOperacion("No se puede actualizar la actividad");
        }

        /// <summary>
        /// Método encargado de Cancelar la Asignación
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion CancelaActividadAsignacion( int id_usuario)
        {
            
                //Invocando Método de Actualización
                return this.editaRegistroActividadAsignacion(this._id_orden_actividad, (int)id_tipo, (int)EstatusAsignacionActividad.Cancelada, this._id_empleado, this._id_proveedor, this._inicio_asignacion, 
                                                            this._fin_asignacion, 0,id_usuario, this.habilitar);
        }

        /// <summary>
        /// Metodo encargado de deshabilitar la asignacion de la actividad
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaActividadAsignacion(int id_usuario)
        {
            //Variable retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando que el estatus de la asignación no sea pausado
            if (this.Estatus == EstatusAsignacionActividad.Asignada )
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Deshabilitando los registros periodo
                    retorno = AsignacionPeriodo.DeshabilitaPeriodosAsignacionActividad(this._id_asignacion_actividad, id_usuario);

                    //Si los periodos fueron deshabilitados correctamente
                    if (retorno.OperacionExitosa)
                    {
                        //Realizamos la modificacion del estatus 
                        retorno = this.editaRegistroActividadAsignacion(this.id_orden_actividad, this.id_tipo, this.id_estatus, this.id_empleado, this.id_proveedor, this.inicio_asignacion, this.fin_asignacion,0, id_usuario, false);

                        //Si ha sido exitosa la actualizacion
                        if (retorno.OperacionExitosa)
                        {
                            //Instanciamos la actividad 
                            using (OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(this.id_orden_actividad))
                            {
                                //Realizamos la actualizacion del estatus de la actividad
                                retorno = actividad.AsignaEstatusOrdenActividad(id_usuario, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro());

                                //Si se actualizó correctamente
                                if (retorno.OperacionExitosa)
                                    //Asignando Id de registro de interés
                                    retorno = new RetornoOperacion(this._id_asignacion_actividad);
                            }
                        }
                    }
                    //Termina la transacción
                    if (retorno.OperacionExitosa)
                        trans.Complete();
                }
            }
            //De lo contrario
            else
                retorno = new RetornoOperacion("El estatus de la asignación no permite su eliminación.");

            //Retornamos el resultado
            return retorno;

        }
        /// <summary>
        /// Deshabilita todas las asignaciones ligadas a una actividad
        /// </summary>
        /// <param name="idOrdenActividad"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaAsignacionesActividad(int idOrdenActividad, int idUsuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Cargando las asignaciones de la Actividad
            using (DataTable dt = CargaAsignacionesActividadOT(idOrdenActividad))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Para cada registro devuelto
                        foreach (DataRow f in dt.Rows)
                        {
                            //Instanciando asignación
                            using (ActividadAsignacion a = new ActividadAsignacion(f.Field<int>("IdAsignacionActividad")))
                            {
                                //Si asignacion existe
                                if (a.id_asignacion_actividad > 0)
                                    //Desahbilitando registro
                                    resultado = a.DeshabilitaActividadAsignacion(idUsuario);
                                //De lo contrario
                                else
                                    //Indicando el error
                                    resultado = new RetornoOperacion("Error al leer registro asignación.");

                                //Si existe algún error
                                if (!resultado.OperacionExitosa)
                                    //Saliendo del ciclo
                                    break;
                            }
                        }

                        //¿Operación Exitosa?
                        if (resultado.OperacionExitosa)
                            //Completando Transacción
                            trans.Complete();
                    }
                }
                //De lo contrario
                else
                    resultado = new RetornoOperacion("Error al cargar las asignaciones de la actividad.");
            }

            //Devolvinedo resultado
            return resultado;
        }
        /// <summary>
        /// Metodo encargado de cambiar el estatus de la asignacion de actividad
        /// </summary>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion CambiaEstatusActividadAsignacion(EstatusAsignacionActividad estatus, int id_usuario)
        {
            //Variable retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizamos la modificacion del estatus 
                retorno = this.editaRegistroActividadAsignacion(this.id_orden_actividad, this.id_tipo, (int)estatus, this.id_empleado, this.id_proveedor, this.inicio_asignacion, this.fin_asignacion, 0, id_usuario, this.habilitar);

                //Si ha sido exitosa la actualizacion
                if (retorno.OperacionExitosa)
                {
                    //Instanciamos la actividad 
                    using (OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(this.id_orden_actividad))
                    {
                        //Realizamos la actualizacion del estatus de la actividad
                        retorno = actividad.AsignaEstatusOrdenActividad(id_usuario, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro());

                        //Si se actualizó correctamente
                        if (retorno.OperacionExitosa)
                        {   
                            //Asignando Id de registro de interés
                            retorno = new RetornoOperacion(this._id_asignacion_actividad);

                            //Completando Transacción
                            trans.Complete();
                        }
                    }
                }
            }

            //Retornamos el resultado
            return retorno;
        }
        
        /// <summary>
        /// Metodo encargado de pausar una asignacion
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion IniciaAsignacion(DateTime inicio, int id_usuario)
        {

            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Instanciamos la actividad 
            using (OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(this.id_orden_actividad))
            {
                //Validamos que la Fecha de Inicio de la asignación  sea mayor a la fecha de Inicio de la Actividad
                if (inicio >= actividad.fecha_inicio || Fecha.EsFechaMinima(actividad.fecha_inicio))
                {
                    //Validamos que no cuente con periodos abiertos
                    if (this._periodos_abiertos == 0)
                    {
                        //Validamos fechas de los periodos
                        if (Fecha.EsFechaMinima(this._fecha_periodo_maxima) || inicio >= this._fecha_periodo_maxima)
                        {
                            //Instanciamos una transaccion de BD
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Modificamos el estatus y fecha de la asignacion 
                                retorno = this.editaRegistroActividadAsignacion(this.id_orden_actividad, this.id_tipo, (int)ActividadAsignacion.EstatusAsignacionActividad.Iniciada, this.id_empleado, this.id_proveedor,
                                    this._inicio_asignacion == DateTime.MinValue ? inicio : this._inicio_asignacion, this.fin_asignacion, 0, id_usuario, this.habilitar);


                                //Validamos Resultado
                                if (retorno.OperacionExitosa)
                                {
                                    //Insertamos el periodo ligado a la actividad
                                    retorno = AsignacionPeriodo.InsertaActividadPeriodo(this.id_asignacion_actividad, AsignacionPeriodo.TipoPeriodoActividad.OrdenDeTrabajo, inicio, id_usuario);

                                    //Si ha sido exitosa la actualizacion
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Validamos que sea la Primera Asignacion Por Iniciar
                                        if(OrdenTrabajoActividad.ValidaActividadesIniciadas(actividad.id_orden))
                                        {
                                            //Intsnaciamos Orden T
                                            using(OrdenTrabajo objOrdenTrabajo = new OrdenTrabajo(actividad.id_orden))
                                            {
                                            
                                            //Actualizamos Fecha de Inicio de la Orden
                                                retorno = objOrdenTrabajo.ActualizaFechaInicioOrdenTrabajo(inicio, id_usuario);
                                            }
                                        }

                                        //Validamos Resultado
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Realizamos la actualizacion del estatus de la actividad
                                            retorno = actividad.AsignaEstatusOrdenActividad(id_usuario, inicio);


                                            //Si se actualizó correctamenete
                                            if (retorno.OperacionExitosa)
                                            {
                                                retorno = new RetornoOperacion(this._id_asignacion_actividad);
                                                //Completamos transacción
                                                trans.Complete();
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        else
                            retorno = new RetornoOperacion("La fecha de Inicio debe ser mayor a la última fecha del periodo " + this._fecha_periodo_maxima.ToString("dd/MM/yyyy HH:mm") + ".");
                    }

                    else
                        retorno = new RetornoOperacion("Ya ha sido iniciada la Asignación.");
                }
                else
                    //Establecemos Mesaje Error
                    retorno = new RetornoOperacion("La fecha de inicio de la asignación " + inicio.ToString("dd/MM/yyyy HH:mm") + " debe ser mayor a la fecha de inicio de la actividad " + actividad.fecha_inicio.ToString("dd/MM/yyyy HH:mm") + ".");

            }

            //Retornamos el resultado
            return retorno;
        }

        /// <summary>
        /// Metodo encargado de iniciar una asignacion
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion PausaAsignacion(DateTime fecha, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validamos que cuente con periodos abiertos
            if (this._periodos_abiertos > 0)
            {
                //Instanciamos Periodo Abierto
                using (AsignacionPeriodo objAsignacionPeriodo = new AsignacionPeriodo(AsignacionPeriodo.ObtienePeriodoAbiertoAsignacionActividad(this._id_asignacion_actividad)))
                {
                    //Validamos que exista Periodo
                    if (objAsignacionPeriodo.id_actividad_asignacion_periodo > 0)
                    {
                        //Instanciamos una transaccion de BD
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Terminamos Periodo
                            retorno = objAsignacionPeriodo.TerminaActividadPeriodo(fecha, id_usuario);
                            //Validamos resultado
                            if (retorno.OperacionExitosa)
                            {
                                //Modificamos el estatus y fecha de la asignacion 
                                retorno = this.editaRegistroActividadAsignacion(this.id_orden_actividad, this.id_tipo, (int)ActividadAsignacion.EstatusAsignacionActividad.Pausada, this.id_empleado, this.id_proveedor,
                                           this._inicio_asignacion, this.fin_asignacion, 0, id_usuario, this.habilitar);

                                //Si ha sido exitosa la actualizacion
                                if (retorno.OperacionExitosa)
                                {
                                    //Instanciamos la actividad 
                                    using (OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(this.id_orden_actividad))
                                    {
                                        //Realizamos la actualizacion del estatus de la actividad
                                        retorno = actividad.AsignaEstatusOrdenActividad(id_usuario, fecha);
                                        //Si se actualizó correctamenete
                                        if (retorno.OperacionExitosa)
                                        {
                                            retorno = new RetornoOperacion(this._id_asignacion_actividad);
                                            //Completamos Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        retorno = new RetornoOperacion("No se puede recuperar datos complentarios del periodo.");
                    }

                }
            }

            else
                retorno = new RetornoOperacion("Imposible pausar la asignación ya que aún no se ha iniciado.");

            //Retornamos el resultado
            return retorno;
        }

        /// <summary>
        /// Metodo encargado de modificar la fecha de incio de la asignacion 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ModificaInicioAsignacion(DateTime fecha, int id_usuario)
        {
            //Declaramos e instanciamos la variable de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validamos que la fecha que deseamos actualizar sea valida para los periodos de la asignacion
            if (this._fecha_periodo_minima.CompareTo(DateTime.MinValue) == 0 || (this._fecha_periodo_minima.CompareTo(DateTime.MinValue) > 0 && this._fecha_periodo_minima.CompareTo(fecha) >= 0))
            {
                //Instanciamos una transaccion de BD
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Editamos la fecha de inicio de la asignacion 
                    retorno = this.editaRegistroActividadAsignacion(this.id_orden_actividad, this.id_tipo, this.id_estatus, this.id_empleado, this.id_proveedor, fecha, this.fin_asignacion,0, id_usuario, this.habilitar);

                    //Si la operación se realizo correctamente 
                    if (retorno.OperacionExitosa)
                    {
                        //Instanciamos una actividad
                        using (OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(this.id_orden_actividad))
                        {
                            //Validamos si la fecha de inicio de actividad se ve afectada
                            if (actividad.fecha_inicio.CompareTo(fecha) > 0)
                            {
                                //Editamos la fecha de inicio de la actividad
                                retorno = actividad.EditaOrdenTrabajoActividad(actividad.id_actividad, actividad.id_orden, actividad.id_falla, actividad.EstatusActividad, fecha, actividad.fecha_fin, id_usuario);

                                //¿Operación Exitosa?
                                if (retorno.OperacionExitosa)
                                    //Completando Transacción
                                    trans.Complete();
                            }
                        }
                    }
                }
            }
            else
                //Personalizando error
                retorno = new RetornoOperacion("La fecha de inicio que desea asignar es mayor a la mínima permitida por los periodos.");

            //Retornamos el resultado de la operacion 
            return retorno;
        }
        /// <summary>
        /// Metodo encargado de terminar una asignacion de actividad
        /// </summary>
        /// <param name="fecha_fin">Fcha Termino de la Asignación</param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion TerminaAsignacion(DateTime fecha, int id_usuario)
        {
            //Instanciamos la variable de retorno 
            RetornoOperacion retorno = new RetornoOperacion();

                //Validamos Peiodos abierto
                if (this._periodos_abiertos > 0)
                {
                    //Obtenemos Periodo Abierto
                    using (AsignacionPeriodo objPeriodo = new AsignacionPeriodo(AsignacionPeriodo.ObtienePeriodoAbiertoAsignacionActividad(this._id_asignacion_actividad)))
                    {
                        //Validando que la fecha de inicio sea menor o igual a la minima de los periodos de la asignación, o no existe fecha limitante
                        if ( fecha  >= objPeriodo.inicio_periodo)
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Terminamos Periodo
                                retorno = objPeriodo.TerminaActividadPeriodo(fecha, id_usuario);

                                //Validamos Resultado
                                if (retorno.OperacionExitosa)
                                {
                                    //Realizamos el termino de la asignacion 
                                    retorno = this.editaRegistroActividadAsignacion(this.id_orden_actividad, this.id_tipo, (int)ActividadAsignacion.EstatusAsignacionActividad.Terminada,
                                             this.id_empleado, this.id_proveedor, this.inicio_asignacion, fecha, AsignacionPeriodo.ObtieneDuracionPeriodo(this._id_asignacion_actividad), id_usuario, this.habilitar);

                                    //Si la operacion se realizo correctamente 
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Instanciamos la actividad 
                                        using (OrdenTrabajoActividad actividad = new OrdenTrabajoActividad(this.id_orden_actividad))
                                        {
                                            //Realizamos la actualizacion del estatus de la actividad
                                            retorno = actividad.AsignaEstatusOrdenActividad(id_usuario, fecha);

                                            //Si se actualizó correctamenete
                                            if (retorno.OperacionExitosa)
                                            {
                                                //Instanciando el Id
                                                retorno = new RetornoOperacion(this._id_asignacion_actividad);

                                                //Completando Transacción
                                                trans.Complete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                            retorno = new RetornoOperacion("La fecha de fin debe ser mayor a la última fecha del periodo " + this._fecha_periodo_minima.ToString("dd/MM/yyyy HH:mm") + ".");
                    }

                }
                else
                    retorno = new RetornoOperacion("La asignación  aún no ha sido Iniciada.");


            //Retornamos el resultado
            return retorno;
        }
        /// <summary>
        /// Metodo encargado de asignar un nuevo periodo a la actividad
        /// </summary>
        /// <param name="TipoPeriodo"></param>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion RegistraPeriodoActividad(AsignacionPeriodo.TipoPeriodoActividad TipoPeriodo, DateTime inicio, DateTime fin, int id_usuario)
        {
            //Instanciamos el objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Instanciamos al tipo de periodo
            using (TipoPeriodoAsignacion tipo = new TipoPeriodoAsignacion((int)TipoPeriodo))
            {
                //Validamos que la fecha del periodo se encuentre dentro del rango de la actividad (sólo para pausas de tiempo)
                //Al igual es requerido para registros pausa, que el estatus sea iniciado
                if ((this.inicio_asignacion.CompareTo(inicio) <= 0
                    && tipo.signo < 0 && this.Estatus == EstatusAsignacionActividad.Iniciada)
                    || (tipo.signo > 0 && this.Estatus != EstatusAsignacionActividad.Asignada))
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Insertamos el periodo ligado a la actividad
                        //retorno = AsignacionPeriodo.InsertaActividadPeriodo(this.id_asignacion_actividad, TipoPeriodo, inicio, fin, id_usuario);

                        //Validamos que la insercion haya sido exitosa y si el tipo de periodo requiere interrupcion
                        if (retorno.OperacionExitosa)
                        {
                            //En caso de ser un periodo negativo y que la fecha de fin sea iagual al valor minimo (fecha no asignada), pausamos la actividad
                            if (tipo.signo == -1 && fin.CompareTo(DateTime.MinValue) == 0)
                            {   
                                //Cambiamos el estatus de la actividad, pausamos 
                                retorno = this.CambiaEstatusActividadAsignacion(EstatusAsignacionActividad.Pausada, id_usuario);

                                //¿Operacion Exitosa?
                                if (retorno.OperacionExitosa)

                                    //Completando Transacción
                                    trans.Complete();
                            }
                        }
                    }
                }
                else
                    retorno = new RetornoOperacion(@"Es necesario que el inicio del periodo sea mayor al inicio de asignación y que el estatus de la asignación sea 'Iniciado' (para periodos de pausa).<br />
                                                     Es necesario que el estatus sea distinto de 'Asignado' (periodos para incremento de tiempo).");

            }

            //Retornamos el resultado de la operacion
            return retorno;
        }

        /// <summary>
        /// Carga las asignaciones de empleados a una actividad de Orden de Trabajo
        /// </summary>
        /// <param name="idActividadOrdenTrabajo">Id de Actividad asignada a la OT</param>
        /// <param name="gv">GridView que será llenado. NULL para omitir.</param>
        /// <param name="dataKeyNames">Llaves que se aplicarán a cada fila del GridView</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesActividadOT(int idActividadOrdenTrabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtAsignaciones = null;
            
            //Inicialziando los parámetros de consulta
            object[] parametros = { 4, 0, idActividadOrdenTrabajo, 0, 0, 0, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtAsignaciones = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtAsignaciones;
        }

        /// <summary>
        /// Carga las asignaciones que no se encuentrán en el estatus deseado
        /// </summary>
        /// <param name="id_orden_actividad">Id Acividad</param>
        /// <param name="estatus">Estatus</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesDifEstatus( int id_orden_actividad,EstatusAsignacionActividad estatus)
        {
            //Declarando Objeto de Retorno
            DataTable dtAsignaciones = null;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 5, 0, id_orden_actividad, 0,estatus, 0, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtAsignaciones = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtAsignaciones;
        }

        /// <summary>
        /// Carga las asignaciones que no se encuentrán pendientes por terminar
        /// </summary>
        /// <param name="id_orden_actividad">Id Acividad</param>
        /// <param name="estatus">Estatus</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesPendientesTermino(int id_orden_actividad)
        {
            //Declarando Objeto de Retorno
            DataTable dtAsignaciones = null;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 6, 0, id_orden_actividad, 0, 0, 0, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtAsignaciones = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtAsignaciones;
        }

        /// <summary>
        /// Obtiene ultima asignación Terminada
        /// </summary>
        /// <param name="id_orden_actividad">Id Actiividad</param>
        /// <returns></returns>
        public static int ObtieneUltimaAsignacionTerminada(int id_orden_actividad)
        {
            //Declarando Objeto de Retorno
            int id = 0;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 7, 0, id_orden_actividad, 0, 0,0, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando Resultado
                    id = (from DataRow r in ds.Tables["Table"].Rows
                          select Convert.ToInt32(r["Id"])).FirstOrDefault();
                }
            }

            //Devolviendo resultado
            return id;
        }

        /// <summary>
        /// Método encargado de cargar las Asignaciones requeridas para la Actividad
        /// </summary>
        /// <param name="idActividadOrdenTrabajo"> Id Actividad de la Orden de Trabajo</param>
        /// <param name="id_actividad">Id Actividad</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesRequeridas(int idActividadOrdenTrabajo, int id_actividad)
        {
            //Declarando Objeto de Retorno
            DataTable dtAsignaciones = null;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 8, 0, idActividadOrdenTrabajo, 0, 0, 0, 0, null, null, 0, 0, false, id_actividad, "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtAsignaciones = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtAsignaciones;
        }

        /// <summary>
        /// Obtiene el Periodo de las Asignaciones ligando una Actividad
        /// </summary>
        /// <param name="idActividad"></param>
        /// <returns></returns>
        public static int ObtieneDuracionAsignacion(int idActividad)
        {
            //Declaramos variable
            int total_duracion = 0;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 9, 0, idActividad, 0, 0, 0, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros).Tables[0])
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    total_duracion = (from DataRow r in mit.Rows
                                          select Convert.ToInt32(r["TotalDuracion"])).FirstOrDefault();

                }
                //Devolviendo resultado
                return total_duracion;
            }
        }
        /// <summary>
        /// Método encargado de Obtener el Monto Total del Empleado
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static decimal ObtieneMontoTotalEmpleados(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            decimal monto_total_empleado = 0.00M;

            //Inicialziando los parámetros de consulta
            object[] param = { 10, 0, id_orden_trabajo, 0, 0, 0, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Obteniendo Monto Total de los Empleados
                        monto_total_empleado = (from DataRow r in ds.Tables["Table"].Rows
                                          select Convert.ToDecimal(r["MontoTotalEmpleados"])).FirstOrDefault();

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return monto_total_empleado;
        }

        /// <summary>
        /// Método encargado de Cargar la Mano de Obra
        /// </summary>
        /// <param name="idOrden">Id Orden</param>
        /// <returns></returns>
        public static DataTable CargaReporteManoObra(int idOrden)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 11, 0, 0, 0, 0, 0, 0, null, null, 0, 0, false, idOrden, "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dt;
        }
        /// <summary>
        /// Método encargado de Obtener la Ultima Fecha de Termino 
        /// </summary>
        /// <param name="id_orden_trabajo"></param>
        /// <returns></returns>
        public static DateTime ObtieneUltimaFechaAsignacionOrdenTrabajo(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DateTime fecha_ultima_asignacion = DateTime.MinValue;

            //Inicialziando los parámetros de consulta
            object[] param = { 12, id_orden_trabajo, 0, 0, 0, 0, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Validando registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Ciclos
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    
                        //Obteniendo Fecha de la Ultima Asignación
                        DateTime.TryParse(dr["FechaUltimaAsignacion"].ToString(), out fecha_ultima_asignacion);
                }
            }

            //Devolviendo Resultado Obtenido
            return fecha_ultima_asignacion;
        }
        /// <summary>
        /// Método encargado de Obtener las Asignaciones ligadas a una Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable ObtieneAsignacionesOrdenTrabajo(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtAsignaciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 13, id_orden_trabajo, 0, 0, 0, 0, 0, null, null, 0, 0, false, "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtAsignaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtAsignaciones;
        }


        #endregion
    }
}
