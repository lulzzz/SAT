using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Collections.Generic;
using System.Linq;

namespace SAT_CL.Mantenimiento
{
     /// <summary>
    /// Proporciona métodos para la administración de registros Actividad de Orden de Trabajo.
    /// </summary>
   public  class AsignacionPeriodo :Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Especifica el tipo de asignacion de periodo asignado a la actividad
        /// </summary>
        public enum TipoPeriodoActividad
        {
            /// <summary>
            /// Periodo asignado a la actividad  de la Orden de Trabajo
            /// </summary>
            OrdenDeTrabajo = 1,
        }
        #endregion

        #region Propiedades y atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        public static string nombre_store_procedure = "mantenimiento.sp_orden_trabajo_actividad_asignacion_periodo_taap";

        private int _id_actividad_asignacion_periodo;
        private int _id_actividad_asignacion;
        private int _id_tipo_periodo;
        private DateTime _inicio_periodo;
        private DateTime _fin_periodo;
        private int _duracion;
        private bool _habilitar;

        public int id_actividad_asignacion_periodo { get { return _id_actividad_asignacion_periodo; } }
        public int id_actividad_asignacion { get { return _id_actividad_asignacion; } }
        public int id_tipo_periodo { get { return _id_tipo_periodo; } }
        public TipoPeriodoActividad Tipo { get { return (TipoPeriodoActividad)_id_tipo_periodo; } }
        public DateTime inicio_periodo { get { return _inicio_periodo; } }
        public DateTime fin_periodo { get { return _fin_periodo; } }
        public int duracion { get { return _duracion; } }
        public bool habilitar { get { return _habilitar; } }


        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public AsignacionPeriodo()
        {
            _id_actividad_asignacion_periodo = 0;
            _id_actividad_asignacion = 0;
            _id_tipo_periodo = 0;
            _inicio_periodo = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _fin_periodo = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _duracion = 0;
            _habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa la instancia en relacion al id de la cotizacion
        /// </summary>
        /// <param name="IdActividadPeriodo">Id de actividad periodo</param>
        public AsignacionPeriodo(int IdActividadPeriodo)
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, IdActividadPeriodo, 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Verificamos que existan datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        _id_actividad_asignacion_periodo = Convert.ToInt32(r["IdActividadAsignacionPeriodo"]);
                        _id_actividad_asignacion = Convert.ToInt32(r["IdActividadAsignacion"]);
                        _id_tipo_periodo = Convert.ToInt32(r["IdTipo"]);
                        _inicio_periodo = Convert.ToDateTime(r["InicioPeriodo"]);
                        DateTime.TryParse(r["FinPeriodo"].ToString(), out _fin_periodo);
                        _duracion = Convert.ToInt32(r["Duracion"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
            }
        }
     
        #endregion

        #region Destructores

        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~AsignacionPeriodo()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados


        /// <summary>
        /// Metodo encargado de editar un registro actividad - periodo
        /// </summary>
        /// <param name="id_actividad_asignacion"></param>
        /// <param name="id_tipo_periodo"></param>
        /// <param name="inicio_periodo"></param>
        /// <param name="fin_periodo"></param>
        /// <param name="duracion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaActividadPeriodo(int id_actividad_asignacion, int id_tipo_periodo, DateTime inicio_periodo, DateTime fin_periodo, int duracion, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this.id_actividad_asignacion_periodo, id_actividad_asignacion, id_tipo_periodo, inicio_periodo, Fecha.ConvierteDateTimeObjeto(fin_periodo), duracion, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);
        }


        #endregion

        #region Metodos publicos (Interfaz)

        /// <summary>
        /// Metodo encargado de insertar un registro periodo - actividad
        /// </summary>
        /// <param name="id_actividad_asignacion"></param>
        /// <param name="id_tipo_periodo"></param>
        /// <param name="inicio_periodo"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaActividadPeriodo(int id_actividad_asignacion, TipoPeriodoActividad id_tipo_periodo, DateTime inicio_periodo, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_actividad_asignacion, (int)id_tipo_periodo, inicio_periodo, null, 0, id_usuario, true, "", "" };

            //Realizamos la inserción del registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);
        }

        /// <summary>
        /// Metodo encargado de editar un registro actividad periodo
        /// </summary>
        /// <param name="id_actividad_asignacion"></param>
        /// <param name="id_tipo_periodo"></param>
        /// <param name="inicio_periodo"></param>
        /// <param name="fin_periodo"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaActividadPeriodo(int id_actividad_asignacion, TipoPeriodoActividad id_tipo_periodo, DateTime inicio_periodo, DateTime fin_periodo, int id_usuario)
        {
            //Declarando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la actividad correspondiente
            using (ActividadAsignacion a = new ActividadAsignacion(this._id_actividad_asignacion))
            {
                //Instanciando al tipo de periodo
                using (TipoPeriodoAsignacion t = new TipoPeriodoAsignacion(this._id_tipo_periodo))
                {
                    //Si la asignación NO se ha terminado y el periodo no la afecta
                    //O si el periodo es extemporaneo
                    if ((a.Estatus != ActividadAsignacion.EstatusAsignacionActividad.Terminada
                        && t.signo < 0) || t.signo > 0)
                        //Realizando la actualización del registro
                        resultado = this.editaActividadPeriodo(id_actividad_asignacion, (int)id_tipo_periodo, inicio_periodo, fin_periodo, 0, id_usuario, this.habilitar);
                    //Si no es válida la edición
                    else
                        resultado = new RetornoOperacion("El estatus de la asignación no permite la actualización del periodo.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Metodo encargado de deshabilitar el periodo ligado a la actividad
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaActividadPeriodo(int id_usuario)
        {
            //Declarando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos al tipo de periodo
            using (TipoPeriodoAsignacion t = new TipoPeriodoAsignacion(this._id_tipo_periodo))
            {
                //Instanciamos la actividad
                using (ActividadAsignacion actividad = new ActividadAsignacion(id_actividad_asignacion))
                {
                    //Si la actividad no se ha cerrado aún y el periodo es negativo (pausa)
                    //O si el periodo no implica actualización sobre la asignación y la actividad
                    if ((actividad.Estatus != ActividadAsignacion.EstatusAsignacionActividad.Terminada && t.signo < 0)
                        || t.signo > 0)
                    {
                           //Creamos la transacción 
                        using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {

                            //Deshabilitando el periodo
                            resultado = this.editaActividadPeriodo(this.id_actividad_asignacion, this.id_tipo_periodo, this.inicio_periodo, this.fin_periodo, 0, id_usuario, false);

                            //Si el resultado de la actualizacion es correcto
                            if (resultado.OperacionExitosa)
                            {
                                //Si el tipo de periodo es negativo
                                if (t.signo == -1 && this._fin_periodo.CompareTo(DateTime.MinValue) == 0)
                                {

                                    //Realizamos la activacion de la actividad
                                    resultado = actividad.CambiaEstatusActividadAsignacion(ActividadAsignacion.EstatusAsignacionActividad.Iniciada, id_usuario);

                                    //Validamos Resultado
                                    if(resultado.OperacionExitosa)
                                    {
                                        scope.Complete();
                                    }
                                }
                            }

                        }
                    }
                    //De lo contrario
                    else
                        resultado = new RetornoOperacion("El estatus de la asignación de actividad no permite esta actualización.");

                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Metodo encargado de terminar el periodo ligado a la actividad
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion TerminaActividadPeriodo(DateTime fecha, int id_usuario)
        {
            //Declarando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos la actividad
            using (ActividadAsignacion actividad = new ActividadAsignacion(_id_actividad_asignacion))
            {
                //Validando que la actividad no se encuentre terminada aún
                if (actividad.Estatus != ActividadAsignacion.EstatusAsignacionActividad.Terminada)
                {
                                   
                    //Validamos Resultado
                    if (this._inicio_periodo > this._fin_periodo)
                    {
                        //Instanciamos al tipo de periodo
                        using (TipoPeriodoAsignacion tipo = new TipoPeriodoAsignacion(this._id_tipo_periodo))
                        {
                            //Creamos la transacción 
                            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {

                                //Terminamos el periodo
                                resultado = this.editaActividadPeriodo(this.id_actividad_asignacion, this.id_tipo_periodo, this.inicio_periodo, fecha, 0, id_usuario, this.habilitar);

                                //Si el resultado de la actualizacion es correcto
                                if (resultado.OperacionExitosa)
                                {
                                    //Si el tipo de periodo es negativo
                                    if (tipo.signo == -1)
                                        //Realizamos la activacion de la actividad
                                        resultado = actividad.CambiaEstatusActividadAsignacion(ActividadAsignacion.EstatusAsignacionActividad.Iniciada, id_usuario);

                                    //Validamos Resultado
                                    if(resultado.OperacionExitosa)
                                    {
                                        //Terminamos transacción
                                        scope.Complete();
                                    }
                                }
                            }
                        }
                    }
                    else    //Establecemos Mesaje Error
                        resultado = new RetornoOperacion("La fecha de inicio del periodo " + this._inicio_periodo.ToString("dd/MM/yyyy HH:mm") + " debe ser mayor a la fecha de fin del periodo " + this._fin_periodo.ToString("dd/MM/yyyy HH:mm") + ".");
                }
                else
                    resultado = new RetornoOperacion("Imposible actualizar, la asignación ya se encuentra terminada.");
            }

            //Devolvinedo resultado
            return resultado;
        }
        /// <summary>
        /// Carga los registros Periodo ligados a una asignación de actividad
        /// </summary>
        /// <param name="idAsignacionActividad">Id de Asignación de Actividad</param>
        /// <returns></returns>
        public static DataTable CargaPeriodosAsignacionActividad(int idAsignacionActividad)
        {
            //Declaramos Tabla
            DataTable mit = null;
            //Inicialziando los parámetros de consulta
            object[] parametros = { 4, 0, idAsignacionActividad, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Carga el Periodo abierto ligado a una asignación de actividad
        /// </summary>
        /// <param name="idAsignacionActividad">Id de Asignación de Actividad</param>
        /// <returns></returns>
        public static int ObtienePeriodoAbiertoAsignacionActividad(int idAsignacionActividad)
        {
            //Declaramos variable
            int id_periodo_abierto = 0;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 5, 0, idAsignacionActividad, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros).Tables[0])
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    id_periodo_abierto = (from DataRow r in mit.Rows
                                          select Convert.ToInt32(r["IdActividadAsignacionPeriodo"])).FirstOrDefault();

                }
                //Devolviendo resultado
                return id_periodo_abierto;
            }
        }
        /// <summary>
        /// Carga el Periodo abierto ligado a una asignación de actividad
        /// </summary>
        /// <param name="idAsignacionActividad">Id de Asignación de Actividad</param>
        /// <returns></returns>
        public static int ObtieneDuracionPeriodo(int idAsignacionActividad)
        {
            //Declaramos variable
            int id_periodo_abierto = 0;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 6, 0, idAsignacionActividad, 0, null, null, 0, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros).Tables[0])
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    id_periodo_abierto = (from DataRow r in mit.Rows
                                          select Convert.ToInt32(r["TotalDuracion"])).FirstOrDefault();

                }
                //Devolviendo resultado
                return id_periodo_abierto;
            }
        }

        /// <summary>
        /// Deshabilita todos los periodos asociados a una asignación de actividad
        /// </summary>
        /// <param name="idAsignacionActividad">Id de </param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaPeriodosAsignacionActividad(int idAsignacionActividad, int id_usuario)
        {
            //Declarando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);

              //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Cargando los periodos ligados a la asignación
                using (DataTable  mit = AsignacionPeriodo.CargaPeriodosAsignacionActividad(idAsignacionActividad))
                {
                    //Validando que el origen de datos exista
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Para cada registro devuelto
                        foreach (DataRow f in mit.Rows)
                        {
                            //Instanciando registro
                            using (AsignacionPeriodo p = new AsignacionPeriodo(f.Field<int>("IdActividadAsignacionPeriodo")))
                            {
                                //Si el periodo existe
                                if (p.id_actividad_asignacion_periodo > 0)
                                    //Deshabilitando el registro
                                    resultado = p.DeshabilitaActividadPeriodo(id_usuario);
                                else
                                    resultado = new RetornoOperacion("Error al leer registro del periodo.");

                                //En caso de error, saliendo del ciclo
                                if (!resultado.OperacionExitosa)
                                    break;
                            }
                        }
                    }
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        

        
    }
}
