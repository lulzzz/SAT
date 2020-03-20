using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using SAT_CL.Almacen;
using System.Transactions;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con las Actividades y Requisiciones Asignadas a la Orden de Trabajo
    /// </summary>
    public class OrdenTrabajoActividadRequisicion : Disposable
    {
        #region Atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        public static string nombre_store_procedure = "mantenimiento.sp_orden_trabajo_actividad_requisicion_toar";

        private int _id_orden_actividad_requisicion;
        public int id_orden_actividad_requisicion { get { return _id_orden_actividad_requisicion; } }
        private int _id_orden;
        public int id_orden { get { return _id_orden; } }
        private int _id_orden_actividad;
        public int id_orden_actividad { get { return _id_orden_actividad; } }
        private int _id_requisicion;
        public int id_requisicion { get { return _id_requisicion; } }
        private DateTime _fecha_asignacion;
        public DateTime fecha_asignacion { get { return _fecha_asignacion; } }
        private bool _habilitar;
        public bool habilitar { get { return _habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que inicializa la instancia por Defecto
        /// </summary>
        public OrdenTrabajoActividadRequisicion()
        {
            //Asignando Valores
            this._id_orden_actividad_requisicion = 0;
            this._id_orden = 0;
            this._id_orden_actividad = 0;
            this._id_requisicion = 0;
            this._fecha_asignacion = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa la instancia en relacion al id especifico
        /// </summary>
        /// <param name="IdOrdenActividadRequisicion"></param>
        public OrdenTrabajoActividadRequisicion(int IdOrdenActividadRequisicion)
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, IdOrdenActividadRequisicion, 0, 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "" };

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
                        this._id_orden_actividad_requisicion = Convert.ToInt32(r["Id"]);
                        this._id_orden = Convert.ToInt32(r["IdOrden"]);
                        this._id_orden_actividad = Convert.ToInt32(r["IdOrdenActividad"]);
                        this._id_requisicion = Convert.ToInt32(r["IdRequisicion"]);
                        this._fecha_asignacion = Convert.ToDateTime(r["FechaAsignacion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }

            }
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~OrdenTrabajoActividadRequisicion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metodo encargado de editar un registro orden-actividad-requisicion
        /// </summary>
        /// <param name="id_orden"></param>
        /// <param name="id_orden_actividad"></param>
        /// <param name="id_requisicion"></param>
        /// <param name="fecha_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaRegistroOrdenActividadRequisicion(int id_orden, int id_orden_actividad, int id_requisicion, DateTime fecha_asignacion, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this.id_orden_actividad_requisicion, id_orden, id_orden_actividad, id_requisicion, fecha_asignacion, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);
        }

        #endregion

        #region Metodos publicos (Interfaz)

        /// <summary>
        /// Metodo encargado de insertar un registro orden-actividad-requisicion
        /// </summary>
        /// <param name="id_orden"></param>
        /// <param name="id_orden_actividad"></param>
        /// <param name="id_requisicion"></param>
        /// <param name="fecha_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaOrdenActividadRequisicion(int id_orden, int id_orden_actividad, int id_requisicion, DateTime fecha_asignacion, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_orden, id_orden_actividad, id_requisicion, fecha_asignacion, id_usuario, true, "", "" };

            //Realizamos la inserción del registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);
        }
        /// <summary>
        /// Metodo encargado de insertar un registro orden - actividad - requisicion 
        /// </summary>
        /// <param name="id_orden"></param>
        /// <param name="id_orden_actividad"></param>
        /// <param name="id_requisicion"></param>
        /// <param name="fecha_asignacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaRegistroOrdenActividadRequisicion(int id_orden, int id_orden_actividad, int id_requisicion, DateTime fecha_asignacion, int id_usuario)
        {
            return this.editaRegistroOrdenActividadRequisicion(id_orden, id_orden_actividad, id_requisicion, fecha_asignacion, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Metodo encargado de deshabilitar el registro orden actividad requisicion
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que deshabilita</param>
        /// <param name="deshabilitaRequisicion">True para deshabilitar también la requisición, de lo contrario False para conservarla.</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaOrdenActividadRequisicion(int id_usuario, bool deshabilitaRequisicion)
        {
            //Declarando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Inicializando transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si es requerido deshabilitar también la requisición
                if (deshabilitaRequisicion)
                {
                    //Instanciando la requisición ligada
                    using (Requisicion r = new Requisicion(this._id_requisicion))
                    {
                        //Validamos Estatus de la Requisición
                        if ((Requisicion.Estatus)r.id_estatus == Requisicion.Estatus.Registrada)
                        {
                            //Si la requisición existe
                            if (r.id_requisicion > 0)
                                //Deshabilitando la requisición
                                resultado = r.DeshabilitaRequisicion(id_usuario);
                        }
                        else
                        {
                            //Establecemos Error
                            resultado = new RetornoOperacion("El estatus de la Requisición no permite su edicón.");
                        }
                    }
                }

                //Si no se ha producido ningún error previo
                if (resultado.OperacionExitosa)
                {
                    //Deshabilitando la liga entre la Requisición y la Actvividad
                    resultado = this.editaRegistroOrdenActividadRequisicion(this.id_orden, this.id_orden_actividad,
                                                this.id_requisicion, this.fecha_asignacion, id_usuario, false);

                    //Validando que la Operción fuese Exitosa
                    if (resultado.OperacionExitosa)

                        //Completando Transacción
                        trans.Complete();
                }
            }

            //Devolviendo el resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la deshabilitación de las requisiciónes y sus relaciones existentes con una actividad de Orden de Trabajo especifica.
        /// </summary>
        /// <param name="idOrdenActividad">Id de Actividad</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>    
        public static RetornoOperacion DeshabilitaRequisicionesActividadOrden(int idOrdenActividad, int idUsuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Cargando las relaciones existentes con Requisiciones 
            using (DataTable dt = CargaRequisicionesActividad(idOrdenActividad))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Instanciando transacción
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Para cada registro relación devuelto
                        foreach (DataRow f in dt.Rows)
                        {
                            //Instanciando la relación correspondiente
                            using (OrdenTrabajoActividadRequisicion r = new OrdenTrabajoActividadRequisicion(f.Field<int>("IdOrdenActividadRequisicion")))
                            {
                                //Si la relación existe
                                if (r.id_orden_actividad_requisicion > 0)
                                {
                                    //Realziando la deshabilitación
                                    resultado = r.DeshabilitaOrdenActividadRequisicion(idUsuario, true);

                                    //Si se produjo algún error
                                    if (!resultado.OperacionExitosa)
                                        //Saliendo del ciclo de deshabilitación
                                        break;
                                }
                            }
                        }

                        //Validando que la Operación fuese Exitosa
                        if (resultado.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
                //Si no contiene el esquema requerido
                else
                    resultado = new RetornoOperacion("Error al consultar las requisiciones de la actividad.");
            }

            //Devolvinedo el resultado obtenido
            return resultado;
        }
        /// <summary>
        /// Realiza la carga de relaciones existentes entre una actividad especifica y sus requisiciones
        /// </summary>
        /// <param name="idOrdenActividad">Id de Actividad</param>
        /// <returns></returns>
        public static DataTable CargaRequisicionesActividad(int idOrdenActividad)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;
            
            //Inicialziando los parámetros de consulta
            object[] parametros = { 4, 0, 0, idOrdenActividad, 0, null, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si se desea realizar la carga
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtRequisiciones = ds.Tables["Table"];

                //Devolviendo resultado
                return dtRequisiciones;
            }
        }

        /// <summary>
        /// Método encargado de Obtener las Requisiciones Pendientes x Termino de Actividad ligando una Orden
        /// </summary>
        /// <param name="idOrdenActividad">Id Orden Actividad</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaRequisicionesPendientexTerminarActividad(int idOrdenActividad)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicialziando los parámetros de consulta
            object[] parametros = { 5, 0, 0, idOrdenActividad, 0, null, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si se desea realizar la carga
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    resultado = new RetornoOperacion("Existen Requisiciones Pendientes", false);

                //Devolviendo resultado
                return resultado;
            }
        }
        /// <summary>
        /// Método encargado de Obtener las Requisiciones Pendientes x Termino de Actividad ligando una Orden
        /// </summary>
        /// <param name="idOrdenActividad">Id Orden Actividad</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaRequisicionesPendientexDeshabilitarActividad(int idOrdenActividad)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicialziando los parámetros de consulta
            object[] parametros = { 6, 0, 0, idOrdenActividad, 0, null, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si se desea realizar la carga
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    resultado = new RetornoOperacion("Existen Requisiciones Pendientes", false);

                //Devolviendo resultado
                return resultado;
            }
        }

        /// <summary>
        /// Método encargado de Obtener las Requisiciones Pendientes x Termino de Actividad ligando una Orden
        /// </summary>
        /// <param name="idOrdenActividad">Id Orden Actividad</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaRequisicionesPendientexCancelarActividad(int idOrdenActividad)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicialziando los parámetros de consulta
            object[] parametros = { 7, 0, 0, idOrdenActividad, 0, null, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si se desea realizar la carga
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    resultado = new RetornoOperacion("Existen Requisiciones Pendientes", false);

                //Devolviendo resultado
                return resultado;
            }
        }

        /// <summary>
        /// Método encargado de Obtener el Id de la Orden de Trabajo Actividad Requisición
        /// </summary>
        /// <param name="id_requisicion">Id Requisición</param>
        /// <returns></returns>
        public static int ObtieneOrdenTrabajActividadRequisicion(int id_requisicion)
        {
            //Declarando Objeto de Retorno
            int Id = 0;

            //Inicialziando los parámetros de consulta
            object[] parametros = { 8, 0, 0, 0, id_requisicion, null, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si se desea realizar la carga
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Obtenemos Parada Anterior
                    Id = (from DataRow r in ds.Tables[0].Rows
                                             select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return Id;
            }
            
        }
        #endregion
    }
}
