using System;
using TSDK.Datos;
using TSDK.Base;
using System.Data;
using System.Transactions;

namespace SAT_CL.Documentacion
{
    public class ServicioImportacionDetalle : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo que almacena el SP
        /// </summary>
        private static string _nom_sp = "documentacion.sp_servicio_importacion_detalle_tsid";

        private int _id_servicio_importacion_detalle;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_servicio_importacion_detalle { get { return this._id_servicio_importacion_detalle; } }
        private int _id_servicio_importacion;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_servicio_importacion { get { return this._id_servicio_importacion; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private string _no_viaje;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public string no_viaje { get { return this._no_viaje; } }
        private int _secuencia;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int secuencia { get { return this._secuencia; } }
        private DateTime _cita_carga;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public DateTime cita_carga { get { return this._cita_carga; } }
        private DateTime _cita_descarga;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public DateTime cita_descarga { get { return this._cita_descarga; } }
        private string _operador;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public string operador { get { return this._operador; } }
        private string _unidad;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public string unidad { get { return this._unidad; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public ServicioImportacionDetalle()
        {
            //Inicializando Variables
            this._id_servicio_importacion_detalle = 
            this._id_servicio_importacion = 
            this._id_servicio = 0;
            this._no_viaje = "";
            this._secuencia = 0;
            this._cita_carga = 
            this._cita_descarga = DateTime.MinValue;
            this._operador = 
            this._unidad = "";
            this._habilitar = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion_detalle"></param>
        public ServicioImportacionDetalle(int id_servicio_importacion_detalle)
        {
            cargaAtributosInstancia(id_servicio_importacion_detalle);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ServicioImportacionDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion_detalle"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_servicio_importacion_detalle)
        {
            bool retorno = false;
            object[] param = { 3, id_servicio_importacion_detalle, 0, 0, "", 0, null, null, "", "", 0, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Inicializando Variables
                        this._id_servicio_importacion_detalle = id_servicio_importacion_detalle;
                        this._id_servicio_importacion = Convert.ToInt32(dr["IdServicioImportacion"]);
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._no_viaje = dr["NoViaje"].ToString();
                        this._secuencia = Convert.ToInt32(dr["Secuencia"]);
                        DateTime.TryParse(dr["CitaCarga"].ToString(), out this._cita_carga);
                        DateTime.TryParse(dr["CitaDescarga"].ToString(), out this._cita_descarga);
                        this._operador = dr["Operador"].ToString();
                        this._unidad = dr["Unidad"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    retorno = true;
                }
            }

            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion"></param>
        /// <param name="id_servicio"></param>
        /// <param name="no_viaje"></param>
        /// <param name="secuencia"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="operador"></param>
        /// <param name="unidad"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizandoRegistrosBD(int id_servicio_importacion, int id_servicio, string no_viaje, int secuencia, 
                                                DateTime cita_carga, DateTime cita_descarga, string operador, string unidad, 
                                                int id_usuario, bool habilitar)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 2, this._id_servicio_importacion_detalle, id_servicio_importacion, id_servicio, no_viaje, secuencia, cita_carga, cita_descarga, operador, unidad, id_usuario, habilitar, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion"></param>
        /// <param name="id_servicio"></param>
        /// <param name="no_viaje"></param>
        /// <param name="secuencia"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="operador"></param>
        /// <param name="unidad"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioImportacionDetalle(int id_servicio_importacion, int id_servicio, string no_viaje, int secuencia,
                                                        DateTime cita_carga, DateTime cita_descarga, string operador, string unidad,
                                                        int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 1, 0, id_servicio_importacion, id_servicio, no_viaje, secuencia, cita_carga, cita_descarga, operador, unidad, id_usuario, true, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion"></param>
        /// <param name="id_servicio"></param>
        /// <param name="no_viaje"></param>
        /// <param name="secuencia"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="operador"></param>
        /// <param name="unidad"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioImportacionDetalle(int id_servicio_importacion, int id_servicio, string no_viaje, int secuencia,
                                                        DateTime cita_carga, DateTime cita_descarga, string operador, string unidad,
                                                        int id_usuario)
        {
            return this.actualizandoRegistrosBD(id_servicio_importacion, id_servicio, no_viaje, secuencia, cita_carga, cita_descarga, operador, unidad, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioImportacionDetalle(int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                retorno = this.actualizandoRegistrosBD(this._id_servicio_importacion, this._id_servicio, this._no_viaje, this._secuencia, this._cita_carga, this._cita_descarga, this._operador, this._unidad, id_usuario, false);
                if (retorno.OperacionExitosa)
                {
                    using (DataTable dtDetalles = ServicioImportacionDetalle.ObtieneImportaciones(this._id_servicio_importacion))
                    {
                        if (Validacion.ValidaOrigenDatos(dtDetalles))
                        
                            retorno = ActualizaEstatusImportacionGeneral(this._id_servicio_importacion, id_usuario);
                        else
                        {
                            using (ServicioImportacion si = new ServicioImportacion(this._id_servicio_importacion))
                            {
                                if (si.habilitar)
                                    retorno = si.DeshabilitaServicioImportacion(id_usuario);
                                else
                                    retorno = new RetornoOperacion("No se puede recuperar la Importación del Servicio");
                            }
                        }

                        if (retorno.OperacionExitosa)
                        {
                            retorno = new RetornoOperacion(this._id_servicio_importacion);
                            scope.Complete();
                        }
                    }
                }
            }
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="operador"></param>
        /// <param name="unidad"></param>
        /// <param name="no_viaje"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaServicioProduccion(int id_servicio, DateTime cita_carga, DateTime cita_descarga, string operador, string unidad, string no_viaje, int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();

            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                retorno = this.actualizandoRegistrosBD(this._id_servicio_importacion, id_servicio, no_viaje, this._secuencia, cita_carga, cita_descarga, operador, unidad, id_usuario, this._habilitar);
                if (retorno.OperacionExitosa)
                {
                    retorno = ServicioImportacionDetalle.ActualizaEstatusImportacionGeneral(this._id_servicio_importacion, id_usuario);
                    if (retorno.OperacionExitosa)
                    {
                        retorno = new RetornoOperacion(this.id_servicio_importacion_detalle);
                        scope.Complete();
                    }
                }
            }
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicioImportacionDetalle()
        {
            return this.cargaAtributosInstancia(this._id_servicio_importacion_detalle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_cliente"></param>
        /// <param name="id_transportista"></param>
        /// <returns></returns>
        public static DataTable ObtieneImportaciones(int id_servicio_importacion)
        {
            DataTable dtImportaciones = new DataTable();
            object[] param = { 4, 0, id_servicio_importacion, 0, "", 0, null, null, "", "", 0, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dtImportaciones = ds.Tables["Table"];
            }
            return dtImportaciones;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaEstatusImportacionGeneral(int id_servicio_importacion, int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            byte id_estatus = 0;
            object[] param = { 5, 0, id_servicio_importacion, 0, "", 0, null, null, "", "", 0, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        break;
                    }

                    //Validando existencia
                    if (id_estatus > 0)
                    {
                        using (ServicioImportacion si = new ServicioImportacion(id_servicio_importacion))
                        {
                            if (si.habilitar)
                                //Actualizando Estatus
                                retorno = si.ActualizaEstatusServicioImportacion((ServicioImportacion.Estatus)id_estatus, id_usuario);
                            else
                                retorno = new RetornoOperacion("No se pudo recuperar la Importación");
                        }
                    }
                }
            }
            
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_transportista"></param>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="fecha_operacion"></param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesImportacion(int id_compania, int id_transportista, int id_servicio_maestro, DateTime fecha_operacion)
        {
            DataTable dtDetallesImportaciones = new DataTable();
            object[] param = { 6, id_compania, id_transportista, id_servicio_maestro, "", 0, fecha_operacion, null, "", "", 0, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dtDetallesImportaciones = ds.Tables["Table"];
            }
            return dtDetallesImportaciones;
        }
        /// <summary>
        /// Método encargado de Obtener el Detalle de Importación dado un Servicio
        /// </summary>
        /// <param name="id_servicio">Identificador del Servicio</param>
        /// <returns></returns>
        public static ServicioImportacionDetalle ObtieneDetalleServicio(int id_servicio)
        {
            ServicioImportacionDetalle sid = new ServicioImportacionDetalle();
            object[] param = { 7, 0, 0, id_servicio, "", 0, null, null, "", "", 0, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Citas
                        DateTime cc, cd;
                        cc = cd = DateTime.MinValue;
                        DateTime.TryParse(dr["CitaCarga"].ToString(), out cc);
                        DateTime.TryParse(dr["CitaDescarga"].ToString(), out cd);
                        sid = new ServicioImportacionDetalle
                        {
                            //Inicializando Variables
                            _id_servicio_importacion_detalle = Convert.ToInt32(dr["Id"]),
                            _id_servicio_importacion = Convert.ToInt32(dr["IdServicioImportacion"]),
                            _id_servicio = Convert.ToInt32(dr["IdServicio"]),
                            _no_viaje = dr["NoViaje"].ToString(),
                            _secuencia = Convert.ToInt32(dr["Secuencia"]),
                            _cita_carga = cc,
                            _cita_descarga = cd,
                            _operador = dr["Operador"].ToString(),
                            _unidad = dr["Unidad"].ToString(),
                            _habilitar = Convert.ToBoolean(dr["Habilitar"])
                        };
                        break;
                    }
                }
            }
            return sid;
        }

        #endregion
    }
}
