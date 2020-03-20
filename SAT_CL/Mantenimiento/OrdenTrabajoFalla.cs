using System;
using System.Data;
using System.Transactions;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// 
    /// </summary>
    public class OrdenTrabajoFalla : Disposable
    {
        #region Enumeraciones

        #endregion

        #region Propiedades y atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        public static string nombre_stored_procedure = "mantenimiento.sp_orden_trabajo_falla_tf";

        private int _id_falla;
        public int id_falla { get { return _id_falla; } }
        private int _id_orden_trabajo;
        public int id_orden_trabajo { get { return _id_orden_trabajo; } }
        private string _descripcion;
        public string descripcion { get { return _descripcion; } }
        private DateTime _fecha;
        public DateTime fecha { get { return _fecha; } }
        private bool _habilitar;
        public bool habilitar { get { return _habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una instancia con valore por default
        /// </summary>
        public OrdenTrabajoFalla()
        {
            this._id_falla = 0;
            this._id_orden_trabajo = 0;
            this._descripcion = "";
            this._fecha = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            this._habilitar = false;
        }

        /// <summary>
        /// Inicializa una instancia en razon al id proporcionado
        /// </summary>
        /// <param name="IdFalla"></param>
        public OrdenTrabajoFalla(int IdFalla)
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, IdFalla, 0, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_falla = Convert.ToInt32(r["IdFalla"]);
                        this._id_orden_trabajo = Convert.ToInt32(r["IdOrdenTrabajo"]);
                        this._descripcion = r["Descripcion"].ToString();
                        this._fecha = Convert.ToDateTime(r["Fecha"]);
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
        ~OrdenTrabajoFalla()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metodo encargado de editar un registro falla
        /// </summary>
        /// <param name="id_orden_trabajo"></param>
        /// <param name="descripcion"></param>
        /// <param name="fecha"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaRegistroFalla(int id_orden_trabajo, string descripcion, DateTime fecha, int id_usuario, bool habilitar)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la Orden de Trabajo a la cual se asociará
            using (OrdenTrabajo ot = new OrdenTrabajo(id_orden_trabajo))
            {
                //Si el estatus de la OT es distinto de terminado
                if (ot.EstatusOrden != OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                {
                    //Inicializando arreglo de parámetros
                    object[] param = { 2, this.id_falla, id_orden_trabajo, descripcion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha), id_usuario, habilitar, "", "" };

                    //Realizando actualizacion
                    return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, param);
                }
                
                else
                    //De lo contrario
                    resultado = new RetornoOperacion("No es posible modificar fallas de una Orden de Trabajo ya Terminada.");
            }

            //Devolvineod el resultado
            return resultado;
        }

        #endregion

        #region Metodos publicos (Interfaz)

        /// <summary>
        /// Metodo encargado de insertar un registro falla
        /// </summary>
        /// <param name="id_orden_trabajo"></param>
        /// <param name="descripcion"></param>
        /// <param name="fecha"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFalla(int id_orden_trabajo, string descripcion, DateTime fecha, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la Orden de Trabajo a la cual se asociará
            using (OrdenTrabajo ot = new OrdenTrabajo(id_orden_trabajo))
            {
                //Si el estatus de la OT es distinto de terminado
                if (ot.EstatusOrden != OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                {
                    //Inicializando arreglo de parámetros
                    object[] param = { 1, 0, id_orden_trabajo, descripcion, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha), id_usuario, true, "", "" };

                    //Realizamos la inserción del registro
                    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, param);
                }
                
                else
                    //De lo contrario
                    resultado = new RetornoOperacion("No es posible añadir fallas a una Orden de Trabajo ya Terminada.");
            }

            //Devolvineod el resultado
            return resultado;
        }
        /// <summary>
        /// Metodo encargado de editar el registro falla
        /// </summary>
        /// <param name="id_orden_trabajo"></param>
        /// <param name="descripcion"></param>
        /// <param name="fecha"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaRegistroFalla(int id_orden_trabajo, string descripcion, DateTime fecha, int id_usuario)
        {
            //Actualizando Registro
            return this.editaRegistroFalla(id_orden_trabajo, descripcion, fecha, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Metodo encargado de deshabilitar la falla
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFalla(int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizando la actualización solicitada
                resultado = this.editaRegistroFalla(this.id_orden_trabajo, this.descripcion, this.fecha, id_usuario, false);

                //Si la deshabilitación fue exitosa
                if (resultado.OperacionExitosa)
                {            
                    //Instancia a la clase actividad orden trabajo
                    using (DataTable dt = SAT_CL.Mantenimiento.OrdenTrabajoActividad.CargaActividadesAsignadas(this._id_orden_trabajo, this._id_falla))
                    {
                        //Valida los datos del datattable
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                        {
                            //Recorre las filas de la tabla
                            foreach (DataRow r in dt.Rows)
                            {
                                //Instancia a la clase OrdenTrabajoActividad
                                using(SAT_CL.Mantenimiento.OrdenTrabajoActividad ota = new SAT_CL.Mantenimiento.OrdenTrabajoActividad(Convert.ToInt32(r["Id"])))
                                    //Asigna al objeto resultado el resiltado del método DeshabilitaOrdenTrabajoActividad 
                                    resultado = ota.DeshabilitaOrdenTrabajoActividad(id_usuario);
                                //Valida si se realizo correctamente
                                if (!resultado.OperacionExitosa)
                                    //Cuando el retorno sea false agrega un break para salir del foreach
                                    break;
                            }
                        }
                    }
                }
                    //Si la deshabilitación fue exitosa
                    if (resultado.OperacionExitosa)
                        //Completando Transacción
                        trans.Complete();
                }
                //Si se realizó correctamente la transacción}
                if (resultado.OperacionExitosa)
                    //Reasignando Id de registro de interés
                    resultado = new RetornoOperacion(this._id_falla);            
            //Devolvineod el resultado
            return resultado;
        }

        /// <summary>
        /// Metodo encargado de deshabilitar las fallas de la orden deseada
        /// </summary>
        /// <param name="id_orden"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaFallasOrden(int id_orden, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Cargando los registros de interés
            using (DataTable dt = CargaFallasOrdenTrabajo(id_orden))
            {
                //Validamos el resultado
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Instanciando transacción
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Recorremos cada uno de los registros 
                        foreach (DataRow r in dt.Rows)
                        {
                            //Instanciamos al registro actual
                            using (OrdenTrabajoFalla falla = new OrdenTrabajoFalla(Convert.ToInt32(r["Id"])))
                            {
                                //Realizamos la deshabilitacion de la falla 
                                resultado = falla.DeshabilitaFalla(id_usuario);

                                //Validamos que haya sido realizada correctamente la deshabilitacion
                                if (!resultado.OperacionExitosa)
                                    //En caso de haber fallado la deshabilitacion 
                                    break;
                            }
                        }

                        //Si la deshabilitación fue exitosa
                        if (resultado.OperacionExitosa)
                            //Completando Transacción
                            trans.Complete();
                    }
                }
            }
            return resultado;
        }
        /// <summary>
        /// Carga los registros Fallas asociados a una Orden de Trabajo
        /// </summary>
        /// <param name="idOrdenTrabajo">Id de Orden de Trabajo</param>
        /// <param name="gv">GridView en que serán cargados los datos. NULL para omitir la carga</param>
        /// <param name="dataKeyNames">Colección de llaves que se aplicarán al GridView (separadas por el caracter '-')</param>
        /// <returns></returns>
        public static DataTable CargaFallasOrdenTrabajo(int idOrdenTrabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtFallas = null;
            
            //Inicialziando los parámetros de consulta
            object[] parametros = { 4, 0, idOrdenTrabajo, "", null, 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Si se desea realizar la carga
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                
                    //Asignando Resultado
                    dtFallas = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtFallas;
        }

        #endregion
    }
}
