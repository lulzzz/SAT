using System;
using TSDK.Datos;
using TSDK.Base;
using System.Data;

namespace SAT_CL.Documentacion
{
    /// <summary>
    /// Clase que gestiona las acciones de la Importación de los Servicios
    /// </summary>
    public class ServicioImportacion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa los Estatus de la Importación de Servicios
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// 
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// 
            /// </summary>
            EnProgreso,
            /// <summary>
            /// 
            /// </summary>
            Terminado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo que almacena el SP
        /// </summary>
        private static string _nom_sp = "documentacion.sp_servicio_importacion_tsi";

        private int _id_servicio_importacion;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_servicio_importacion { get { return this._id_servicio_importacion; } }
        private int _id_servicio_maestro;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_servicio_maestro { get { return this._id_servicio_maestro; } }
        private int _id_compania;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private int _secuencia;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int secuencia { get { return this._secuencia; } }
        private DateTime _fecha_generacion;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public DateTime fecha_generacion { get { return this._fecha_generacion; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
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
        private int _id_transportista;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_transportista { get { return this._id_transportista; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Contructor que inicializa los Atributos por Defecto
        /// </summary>
        public ServicioImportacion()
        {
            //Inicializando Valores
            this._id_servicio_importacion =
            this._id_servicio_maestro =
            this._id_compania =
            this._secuencia = 0;
            this._id_estatus = 0;
            this._fecha_generacion =
            this._cita_carga =
            this._cita_descarga = DateTime.MinValue;
            this._id_transportista = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Contructor que inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_servicio_importacion">Identificador del Registro (PK)</param>
        public ServicioImportacion(int id_servicio_importacion)
        {
            cargaAtributosInstancia(id_servicio_importacion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ServicioImportacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_servicio_importacion"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_servicio_importacion)
        {
            //Declarando Variables Auxiliares
            bool retorno = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_servicio_importacion, 0, 0, 0, 0, null, null, null, 0, 0, false, "", "" };
            
            //Obteniendo Resultado de BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Inicializando Valores
                        this._id_servicio_importacion = id_servicio_importacion;
                        this._id_servicio_maestro = Convert.ToInt32(dr["IdServicioMaestro"]);
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._secuencia = Convert.ToInt32(dr["Secuencia"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        DateTime.TryParse(dr["FechaGeneracion"].ToString(), out this._fecha_generacion);
                        DateTime.TryParse(dr["CitaCarga"].ToString(), out this._cita_carga);
                        DateTime.TryParse(dr["CitaDescarga"].ToString(), out this._cita_descarga);
                        this._id_transportista = Convert.ToInt32(dr["IdTransportista"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    retorno = true;
                }
            }

            //Devolviendo Resultado
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="id_compania"></param>
        /// <param name="estatus"></param>
        /// <param name="fecha_generacion"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="id_transportista"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizandoRegistrosBD(int id_servicio_maestro, int secuencia, int id_compania, Estatus estatus, DateTime fecha_generacion,
                                    DateTime cita_carga, DateTime cita_descarga, int id_transportista, int id_usuario, bool habilitar)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 2, this._id_servicio_importacion, id_servicio_maestro, secuencia, id_compania, (byte)estatus, fecha_generacion, cita_carga, cita_descarga, id_transportista, id_usuario, habilitar, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="secuencia"></param>
        /// <param name="id_compania"></param>
        /// <param name="fecha_generacion"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="id_transportista"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioImportacion(int id_servicio_maestro, int secuencia, int id_compania, DateTime fecha_generacion,
                                                        DateTime cita_carga, DateTime cita_descarga, int id_transportista, int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 1, 0, id_servicio_maestro, secuencia, id_compania, (byte)Estatus.Registrado, fecha_generacion, cita_carga, cita_descarga, id_transportista, id_usuario, true, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="secuencia"></param>
        /// <param name="id_compania"></param>
        /// <param name="estatus"></param>
        /// <param name="fecha_generacion"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="id_transportista"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioImportacion(int id_servicio_maestro, int secuencia, int id_compania, Estatus estatus, DateTime fecha_generacion,
                                                        DateTime cita_carga, DateTime cita_descarga, int id_transportista, int id_usuario)
        {
            return this.actualizandoRegistrosBD(id_servicio_maestro, secuencia, id_compania, estatus, fecha_generacion, cita_carga, cita_descarga, id_transportista, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioImportacion(int id_usuario)
        {
            return this.actualizandoRegistrosBD(this._id_servicio_maestro, this._secuencia, this._id_compania, (Estatus)this._id_estatus, this._fecha_generacion, this._cita_carga, this._cita_descarga, this._id_transportista, id_usuario, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusServicioImportacion(Estatus estatus, int id_usuario)
        {
            return this.actualizandoRegistrosBD(this._id_servicio_maestro, this._secuencia, this._id_compania, estatus, this._fecha_generacion, this._cita_carga, this._cita_descarga, this._id_transportista, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicioImportacion()
        {
            return this.cargaAtributosInstancia(this._id_servicio_importacion);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_cliente"></param>
        /// <param name="id_transportista"></param>
        /// <returns></returns>
        public static DataTable ObtieneImportaciones(int id_compania, int id_cliente, int id_transportista, int id_importacion)
        {
            DataTable dtImportaciones = new DataTable();
            object[] param = { 4, id_compania, id_cliente, id_transportista, 0, 0, null, null, null, 0, id_importacion, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dtImportaciones = ds.Tables["Table"];
            }
            return dtImportaciones;
        }
        /// <summary>
        /// Método encargado de Obtener los Servicios Agrupados por Día
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_transportista"></param>
        /// <param name="cita_carga_inicio"></param>
        /// <param name="cita_carga_termino"></param>
        /// <returns></returns>
        public static DataTable ObtieneImportacionesControlAnticipo(int id_compania, int id_transportista, DateTime cita_carga_inicio, DateTime cita_carga_termino)
        {
            DataTable dtControlAnticipos = new DataTable();
            object[] param = { 5, 0, 0, 0, id_compania, 0, null, Fecha.ConvierteDateTimeObjeto(cita_carga_inicio), 
                               Fecha.ConvierteDateTimeObjeto(cita_carga_termino), id_transportista, 0, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dtControlAnticipos = ds.Tables["Table"];
            }
            return dtControlAnticipos;
        }

        #endregion
    }
}
