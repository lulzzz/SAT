using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// Proporciona métodos para la administración de registros Actividad de Orden de Trabajo.
    /// </summary>
    public class OrdenTrabajoActividad : Disposable
    {

        #region Enumeraciones

        /// <summary>
        /// Define los posibles estaus que puede adoptar una Actividad asignada a una Orden de Trabajo.
        /// </summary>
        public enum EstatusOrdenActividad
        {
            /// <summary>
            /// Estatus que indica que la actividad no se ha iniciado, sólo está registrada.
            /// </summary>
            Registrada = 1,
            /// <summary>
            /// Estatus que indica que la actividad esta en progreso
            /// </summary>
            Iniciada,
            /// <summary>
            /// Estatus que indica que la actividad esta activa sin embargo todas las actividades estan pausadas
            /// </summary>
            Pausada,
            /// <summary>
            /// Estatus que indica que la actividad esta terminada
            /// </summary>
            Terminada,
            /// <summary>
            /// Estatus que indica que la actividad esta Cancelada
            /// </summary>
            Cancelada
        }

        #endregion

        #region Propiedades y atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        public static string nombre_stored_procedure = "mantenimiento.sp_orden_trabajo_actividad_toa";

        private int _id_orden_actividad;
        /// <summary>
        /// Id Orden Actividad
        /// </summary>
        public int id_orden_actividad { get { return _id_orden_actividad; } }
        private int _id_actividad;
        /// <summary>
        /// Id Actividad
        /// </summary>
        public int id_actividad { get { return _id_actividad; } }
        private int _id_orden;
        /// <summary>
        /// Id Orden
        /// </summary>
        public int id_orden { get { return _id_orden; } }
        private int _id_falla;
        /// <summary>
        /// Id Falla
        /// </summary>
        public int id_falla { get { return _id_falla; } }
        private int _id_estatus;
        /// <summary>
        /// Id Estatus
        /// </summary>
        public int id_estatus { get { return _id_estatus; } }
        /// <summary>
        /// Estatus Orden Actividad
        /// </summary>
        public EstatusOrdenActividad EstatusActividad { get { return (EstatusOrdenActividad)_id_estatus; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// 
        /// </summary>
        public DateTime fecha_inicio { get { return _fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// 
        /// </summary>
        public DateTime fecha_fin { get { return _fecha_fin; } }
        private int _duracion;
        /// <summary>
        /// 
        /// </summary>
        public int duracion { get { return _duracion; } }
        /// <summary>
        /// 
        /// </summary>
        public string duracion_cadena { get { return Cadena.ConvierteMinutosACadena(this._duracion); } }
        private bool _habilitar;
        /// <summary>
        /// 
        /// </summary>
        public bool habilitar { get { return _habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public OrdenTrabajoActividad()
        {
            _id_orden_actividad = 0;
            _id_actividad = 0;
            _id_orden = 0;
            _id_falla = 0;
            _id_estatus = 0;
            _fecha_inicio = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _fecha_fin = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _duracion = 0;
            _habilitar = false;

        }

        /// <summary>
        /// Constructor que inicializa la instancia en relacion al id especifico
        /// </summary>
        /// <param name="IdOrdenActividad"></param>
        public OrdenTrabajoActividad(int IdOrdenActividad)
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, IdOrdenActividad, 0, 0, 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Verificamos que existan datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        _id_orden_actividad = Convert.ToInt32(r["IdOrdenActividad"]);
                        _id_actividad = Convert.ToInt32(r["IdActividad"]);
                        _id_orden = Convert.ToInt32(r["IdOrden"]);
                        _id_falla = Convert.ToInt32(r["IdFalla"]);
                        _id_estatus = Convert.ToInt32(r["IdEstatus"]);
                        DateTime.TryParse(r["Inicio"].ToString(), out _fecha_inicio);
                        DateTime.TryParse(r["Fin"].ToString(), out _fecha_fin);
                        try
                        {
                            _duracion = Convert.ToInt32(r["DuracionTotal"]);
                        }
                        catch (Exception)
                        {
                        }
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
        ~OrdenTrabajoActividad()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metodo encargado de editar el registro relacion actividad-orden
        /// </summary>
        /// <param name="id_actividad"></param>
        /// <param name="id_orden"></param>
        /// <param name="id_falla"></param>
        /// <param name="id_estatus"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <param name="duracion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaOrdenTrabajoActividad(int id_actividad, int id_orden, int id_falla, EstatusOrdenActividad  id_estatus, DateTime fecha_inicio, DateTime fecha_fin, int duracion, int id_usuario, bool habilitar)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
           
            //Inicializando arreglo de parámetros
            object[] param = { 2, this.id_orden_actividad, id_actividad, id_orden, id_falla, id_estatus, Fecha.ConvierteDateTimeObjeto(fecha_inicio), Fecha.ConvierteDateTimeObjeto(fecha_fin), duracion, id_usuario, habilitar, "", "" };
                      
            //Realizando actualizacion
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, param);
                        
            //Devolvinedo resultado obtenido
            return resultado;
        }


        /// <summary>
        /// Metodo encargado de modificar el estatus de la orden actividad
        /// </summary>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        private RetornoOperacion cambiaEstatusOrdenTrabajoActividad(OrdenTrabajoActividad.EstatusOrdenActividad estatus, int id_usuario)
        {
            return this.editaOrdenTrabajoActividad(this.id_actividad, this.id_orden, this.id_falla, estatus, this.fecha_inicio, this.fecha_fin, this._duracion, id_usuario, this.habilitar);
        }

        #endregion

        #region Metodos publicos (Interfaz)

        /// <summary>
        /// Metodo encargado de insertar un registro orden actividad
        /// </summary>
        /// <param name="id_actividad"></param>
        /// <param name="id_orden"></param>
        /// <param name="id_falla"></param>
        /// <param name="id_estatus"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <param name="duracion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaOrdenActividad(int id_actividad, int id_orden, int id_falla,  int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Orden de Trabajo
            using (OrdenTrabajo  ot = new OrdenTrabajo(id_orden))
            {
                //Si la OT no se encuentra cerrada aún
                if ((OrdenTrabajo.EstatusOrdenTrabajo)ot.id_estatus != OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                {
                    //Inicializando arreglo de parámetros
                    object[] param = { 1, 0, id_actividad, id_orden, id_falla, EstatusOrdenActividad.Registrada, null, null, 0, id_usuario, true, "", "" };
                    
                // Realizando actualizacion
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, param);
                }
                //De lo contrario
                else
                    resultado = new RetornoOperacion("Imposible añadir actividades a una Orden de Trabajo ya Terminada.");
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Metodo encargado de realizar la edicion de registros actividad - orden
        /// </summary>
        /// <param name="id_actividad"></param>
        /// <param name="id_orden"></param>
        /// <param name="id_falla"></param>
        /// <param name="id_estatus"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaOrdenTrabajoActividad(int id_actividad, int id_orden, int id_falla, EstatusOrdenActividad estatus, DateTime fecha_inicio, DateTime fecha_fin, int id_usuario)
        {
            return this.editaOrdenTrabajoActividad(id_actividad, id_orden, id_falla, estatus, fecha_inicio, fecha_fin, this._duracion, id_usuario, this.habilitar);

        }

        /// <summary>
        /// Método encargado de Cambiar el Estatus de la Actividad
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion AbrirActividad(int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion("Sólo es posible Abrir Actividade(s) Terminada(s).");

            //Validamos Estatus
            if ((EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Terminada)
            {
                resultado = editaOrdenTrabajoActividad(this._id_actividad, this._id_orden, this._id_falla, EstatusOrdenActividad.Iniciada,
                            this._fecha_inicio, DateTime.MinValue, 0, id_usuario, this._habilitar);
            }

            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Cancelamos Actividad
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CancelaOrdenTrabajoActividad(int id_usuario)
        {
            //Establecemos resultado
            RetornoOperacion res = new RetornoOperacion();

            //Validamos que el estatus de la Actividad 
            if((EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Terminada)
            {
            //Validamos que todas las asignaciones se encuentren Canceladas
            if (!Validacion.ValidaOrigenDatos(ActividadAsignacion.CargaAsignacionesDifEstatus(this._id_orden_actividad,ActividadAsignacion.EstatusAsignacionActividad.Cancelada)))
            {
                //Validamos Requisiciones Pendientes
                res = OrdenTrabajoActividadRequisicion.ValidaRequisicionesPendientexTerminarActividad(this._id_orden_actividad);
                   //Validamos Resultado
                   if(res.OperacionExitosa)
                   {
                       //Cancelamos la Actividad
                       res = editaOrdenTrabajoActividad(this._id_actividad, this._id_orden, this._id_falla, EstatusOrdenActividad.Cancelada, this._fecha_inicio, this._fecha_fin, this._duracion, id_usuario, this.habilitar);
                   }
            }
            else
                //Mostramos mensaje Resultado
            {
                res = new RetornoOperacion("Las asignaciones deben estar en estatus Cancelada.");
            }
            }
            //Establecemos Mesaje Error
            else
            {
               //Mostramos  Mensaje Error
                res = new RetornoOperacion("El estatus de la Actividad no permite su edición. ");
            }

            //Devolvemos resultado
            return res;

        }

        /// <summary>
        /// Terminamos Actividad
        /// </summary>
        /// <param name="fecha">fecha</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaActividad(DateTime fecha, int id_usuario)
        {
            //Establecemos resultado
            RetornoOperacion res = new RetornoOperacion();

            //Validamos Estatus de la Actividad
            if ((EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Iniciada)
            {
                //Validamos que todas las asignaciones se encuentren Terminadas
                if (!Validacion.ValidaOrigenDatos(ActividadAsignacion.CargaAsignacionesPendientesTermino(this._id_orden_actividad)))
                {
                    //Validamos Fecha de la Actividad vs fecha de termino de la última Asignación
                    using (ActividadAsignacion objAsignacion = new ActividadAsignacion(ActividadAsignacion.ObtieneUltimaAsignacionTerminada(this._id_orden_actividad)))
                    {

                        if (fecha >= objAsignacion.fin_asignacion)
                        {
                            //Validamos Requisiciones Pendientes
                            res = OrdenTrabajoActividadRequisicion.ValidaRequisicionesPendientexTerminarActividad(this._id_orden_actividad);

                                //Validamos Resultado
                                if(res.OperacionExitosa)
                                {
                            
                                //Terminamos Actividad
                                res = editaOrdenTrabajoActividad(this._id_actividad, this._id_orden, this._id_falla, EstatusOrdenActividad.Terminada, this._fecha_inicio, fecha, ActividadAsignacion.ObtieneDuracionAsignacion(this._id_orden_actividad), id_usuario, this.habilitar);
                                }
                        }
                        else
                            //Establecemos Mesaje Error
                            res = new RetornoOperacion("La fecha de termino de la actividad " + fecha.ToString("dd/MM/yyyy HH:mm") + " debe ser mayor a la fecha de termino de la última asignación " + objAsignacion.fin_asignacion.ToString("dd/MM/yyyy HH:mm") + ".");

                    }
                }
                else
                //Mostramos mensaje Resultado
                {
                    res = new RetornoOperacion("Las asignaciones deben estar en estatus Terminada.");
                }
            }
            else
                res = new RetornoOperacion("El estatus de la actividad no permite su edición.");
            //Devolvemos resultado
            return res;

        }
        /// <summary>
        /// Método encargado de Deshabilitar la Orden de Trabajo
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaOrdenTrabajoActividad(int id_usuario)
        {
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos que las asignaciones se encuentran como registradas
            if ((EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Registrada)
            {
                //Validamos Estatus de la Requisiciones
                resultado = OrdenTrabajoActividadRequisicion.ValidaRequisicionesPendientexDeshabilitarActividad(this._id_orden_actividad);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Devolviendo Resultado Obtenido
                    resultado = editaOrdenTrabajoActividad(this._id_actividad, this._id_orden, this._id_falla, (EstatusOrdenActividad)this._id_estatus, this._fecha_inicio, this._fecha_fin, this._duracion, id_usuario, false);
                }
            }
            else
                resultado = new RetornoOperacion("El estatus de la actividad do permite su eliminación");
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Carga Actividades ligando una Orden de Trnbajo
        /// </summary>
        /// <param name="id_orde_trabajo">Id Orden Trabajo</param>
        /// <returns></returns>
        public static DataTable CargaOrdenTrabajoActividades(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;


            //Inicializando arreglo de parámetros
            object[] param = { 4, 0, 0, id_orden_trabajo, 0, 0,null, null, 0, 0, true, "", "" };
                      
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dt;
        }
        /// <summary>
        /// Carga las Actividades dfe acuerdo a los filtros
        /// </summary>
        /// <param name="id_actividad">Id Actividad</param>
        /// <param name="id_familia">Id Familia</param>
        /// <param name="id_sub_familia">Id Subfamilia</param>
        /// <param name="id_tipo_unidad">Id Tipo Unidad</param>
        /// <param name="id_subtipo_unidad">Id SubtipoUnidad</param>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <returns></returns>
        public static DataTable CargaActividades(int id_familia, int id_sub_familia, int id_tipo_unidad, int id_subtipo_unidad, int id_compania_emisor)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;


            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, 0, id_familia, id_sub_familia, id_tipo_unidad, null, null, 0, id_subtipo_unidad, true, id_compania_emisor, "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Objeto de Retorno
            return dt;
        }
        /// <summary>
        /// Metodo encargado de actualizar el estatus de la actividad en razón al estatus de sus asignaciones
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public RetornoOperacion AsignaEstatusOrdenActividad(int id_usuario, DateTime fecha)
        {
            //Instancia una variable de retorno
            RetornoOperacion retorno = new RetornoOperacion(1);

            //Declaramos variables auxiliares
            bool A = false, I = false, P = false, T = false , C = false;
            OrdenTrabajoActividad.EstatusOrdenActividad estatus = EstatusOrdenActividad.Registrada;

            //Inicializando arreglo de parámetros
            object[] param = { 6, this.id_orden_actividad, 0, 0, 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, false, "", "" };

            //Realizamos la consulta para conocer el estatus de las asignaciones de la actividad
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Validamos el resultado
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos el conjunto de registros
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        switch (Convert.ToInt32(r["IdEstatus"]))
                        {
                            //Asignada
                            case 1:
                                {
                                    A = Convert.ToBoolean(r["Valor"]);
                                    break;
                                }
                            //Iniciada
                            case 2:
                                {
                                    I = Convert.ToBoolean(r["Valor"]);
                                    break;
                                }
                            //Pausada
                            case 3:
                                {
                                    P = Convert.ToBoolean(r["Valor"]);
                                    break;
                                }
                            //Terminada
                            case 4:
                                {
                                    T = Convert.ToBoolean(r["Valor"]);
                                    break;
                                }
                            //Cancelada
                            case 5:
                                {
                                    C = Convert.ToBoolean(r["Valor"]);
                                    break;
                                }
                        }
                    }
                        if (!I && P)
                            estatus = EstatusOrdenActividad.Pausada;
                        else
                            if (!I && !P && !T)
                               estatus = EstatusOrdenActividad.Registrada;
                                else
                                estatus = EstatusOrdenActividad.Iniciada;

                    

                    //Si el estatus determinado es diferente al estatus actual
                    if (estatus != this.EstatusActividad)
                    {

                        //Cuando la actividad va a iniciar por primera vez 
                        if (estatus == EstatusOrdenActividad.Iniciada && this.EstatusActividad == EstatusOrdenActividad.Registrada)
                            //Editamos el estatus asi como la fecha de inicio
                            retorno = this.editaOrdenTrabajoActividad(this.id_actividad, this.id_orden, id_falla, estatus, fecha, this.fecha_fin, this._duracion, id_usuario, this.habilitar);
                        else
                            //Para cualquier otro estatus distinto a iniciar por primera vez o terminar
                            if (estatus != EstatusOrdenActividad.Terminada)
                                //Si la actividad no se va a terminar entonces unicamente modificamos el estatus
                                retorno = this.cambiaEstatusOrdenTrabajoActividad(estatus, id_usuario);
                    }
                }

            }
            //Regresamos el resultado 
            return retorno;
        }
        /// <summary>
        /// Método que permite cargar las actividades asignadas a una orden de trabajo
        /// </summary>
        /// <param name="id_orden">Permite identificar a que registro de orden de trabajo pertenece la actividad</param>
        /// <param name="id_falla">Permite identificar a que registro de falla  pertenece la actividad</param>
        /// <returns></returns>
        public static DataTable CargaActividadesAsignadas(int id_orden, int id_falla)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;
            //Creación del objeto param que almacena los datos necesario para realizar la consulta a la tabla OrdenTrabajoActividad
            object[] param = { 7, 0, 0, id_orden, id_falla, 0, null, null, 0, 0, true, "", "" };
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Objeto de Retorno
            return dt;

        }
        /// <summary>
        /// Método encaragdo de Terminar la Actividad y sus Asignaciones Ligadas
        /// </summary>
        /// <param name="estatus">Estatus de la Actividad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion TerminaActividadAsignaciones(DateTime fecha_fin, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que la Actividad este Iniciada ó Pausada
                if ((EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Iniciada ||
                        (EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Pausada)
                {
                    //Obteniendo Asignaciones de la Actividad
                    using (DataTable dtAsignaciones = ActividadAsignacion.CargaAsignacionesActividadOT(this._id_orden_actividad))
                    {
                        //Validando que existan Asignaciones
                        if (Validacion.ValidaOrigenDatos(dtAsignaciones))
                        {
                            //Recorriendo Asignaciones
                            foreach (DataRow dr in dtAsignaciones.Rows)
                            {
                                //Instanciando Asignación
                                using (ActividadAsignacion asignacion = new ActividadAsignacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando que exista la Asignación
                                    if (asignacion.habilitar)
                                    {
                                        //Validando Estatus
                                        switch (asignacion.Estatus)
                                        {
                                            case ActividadAsignacion.EstatusAsignacionActividad.Iniciada:
                                            case ActividadAsignacion.EstatusAsignacionActividad.Pausada:
                                                {
                                                    //Terminando Asignación
                                                    result = asignacion.TerminaAsignacion(fecha_fin, id_usuario);
                                                    break;
                                                }
                                            case ActividadAsignacion.EstatusAsignacionActividad.Terminada:
                                            case ActividadAsignacion.EstatusAsignacionActividad.Cancelada:
                                                {
                                                    //Instanciando Retorno
                                                    result = new RetornoOperacion(asignacion.id_asignacion_actividad);
                                                    break;
                                                }
                                            case ActividadAsignacion.EstatusAsignacionActividad.Asignada:
                                                {
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("La Asignación debe de estar Iniciada ó Pausada para poder Terminarla");
                                                    break;
                                                }
                                        }

                                        //Validando Operación Exitosa
                                        if (!result.OperacionExitosa)

                                            //Terminando Ciclo
                                            break;
                                    }
                                    else
                                    {
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No se puede Acceder a la Asignación");

                                        //Terminando Ciclo
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                //Si esta Registrada o Cancelada
                else if ((EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Registrada ||
                        (EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Cancelada)

                    //Instanciando Excepción
                    result = new RetornoOperacion("La Actividad debe de estar Iniciada ó Pausada para poder Terminarla");
                //Si esta Terminada
                else if ((EstatusOrdenActividad)this._id_estatus == EstatusOrdenActividad.Terminada)
                    //Instanciando Asignación
                    result = new RetornoOperacion(this._id_orden_actividad);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Editamos el estatus asi como la fecha de inicio
                    result = this.editaOrdenTrabajoActividad(this.id_actividad, this.id_orden, this.id_falla, EstatusOrdenActividad.Terminada, this.fecha_inicio, fecha_fin, this._duracion, id_usuario, this.habilitar);

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Registro
                        result = new RetornoOperacion(this._id_orden_actividad);

                        //Completando Transacción
                        trans.Complete();
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Carga el Resumen de las Actividades ligando una Orden de Trabajo
        /// </summary>
        /// <param name="id_orden">Id Orden</param>
        /// <returns></returns>
        public static DataTable CargaResumenActividadesAsignadas(int id_orden)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;


            //Inicializando arreglo de parámetros
            object[] param = { 8, 0, 0, id_orden, 0, 0, null, null, 0, 0, true, "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Objeto de Retorno
            return dt;
        }


        /// <summary>
        /// Carga Actividades Iniciadas de la OT
        /// </summary>
        /// <param name="id_orden">Id Orden</param>
        /// <returns></returns>
        public static bool ValidaActividadesIniciadas(int id_orden)
        {
            //Declarando Objeto de Retorno
            bool valor = true;


            //Inicializando arreglo de parámetros
            object[] param = { 9, 0, 0, id_orden, 0, 0, null, null, 0, 0, true, "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                   valor = false;
            }
            //Devolviendo Objeto de Retorno
            return valor;
        }
        #endregion
    }
}


