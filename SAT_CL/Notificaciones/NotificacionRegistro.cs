using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Notificaciones
{
    public class NotificacionRegistro : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "notificaciones.sp_notificacion_registro_tnr";

        private int _id_notificacion_registro;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_notificacion_registro { get { return this._id_notificacion_registro; } }
        private int _id_lista_distribucion_detalle_contacto;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_lista_distribucion_detalle_contacto { get { return this._id_lista_distribucion_detalle_contacto; } }
        private int _id_registro;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private int _id_estatus;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_estatus { get { return this._id_estatus; } }
        private int _secuencia;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int secuencia { get { return this._secuencia; } }
        private DateTime _fecha_inicio_notificacion;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public DateTime fecha_inicio_notificacion { get { return this._fecha_inicio_notificacion; } }
        private int _estatus_respuesta;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int estatus_respuesta { get { return this._estatus_respuesta; } }
        private DateTime _fecha_termino_notificacion;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public DateTime fecha_termino_notificacion { get { return this._fecha_termino_notificacion; } }
        private int _id_usuario_respuesta;
        /// <summary>
        /// Atributo que almacena
        /// </summary>
        public int id_usuario_respuesta { get { return this._id_usuario_respuesta; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public NotificacionRegistro()
        {
            //Asignando Atributos
            this._id_notificacion_registro = 0;
            this._id_lista_distribucion_detalle_contacto = 0;
            this._id_registro = 0;
            this._id_estatus = 0;
            this._secuencia = 0;
            this._fecha_inicio_notificacion = DateTime.MinValue;
            this._estatus_respuesta = 0;
            this._fecha_termino_notificacion = DateTime.MinValue;
            this._id_usuario_respuesta = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_notificacion_registro"></param>
        public NotificacionRegistro(int id_notificacion_registro)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_notificacion_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~NotificacionRegistro()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos del Registro
        /// </summary>
        /// <param name="id_notificacion_registro"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_notificacion_registro)
        {
            //Declarando Objeto de Retorno
            bool retorno = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_notificacion_registro, 0, 0, 0, 0, null, 0, null, 0, 0, false, "", "" };

            //Obteniendo Datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Datos
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_notificacion_registro = id_notificacion_registro;
                        this._id_lista_distribucion_detalle_contacto = Convert.ToInt32(dr["ListaDetalleContacto"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._id_estatus = Convert.ToInt32(dr["IdEstatus"]);
                        this._secuencia = Convert.ToInt32(dr["Secuencia"]);
                        this._fecha_inicio_notificacion = Convert.ToDateTime(dr["FechaInicio"]);
                        this._estatus_respuesta = Convert.ToInt32(dr["EstatusRespuesta"]);
                        this._fecha_termino_notificacion = Convert.ToDateTime(dr["FechaTermino"]);
                        this._id_usuario_respuesta = Convert.ToInt32(dr["UsuarioRespuesta"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado
            return retorno;
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos del Registro
        /// </summary>
        /// <param name="id_lista_distribucion_detalle_contacto"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_estatus"></param>
        /// <param name="secuencia"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="id_estatus_respuesta"></param>
        /// <param name="fecha_termino"></param>
        /// <param name="id_usuario_respuesta"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_lista_distribucion_detalle_contacto, int id_registro, int id_estatus,
                                                int secuencia, DateTime fecha_inicio, int id_estatus_respuesta, DateTime fecha_termino, int id_usuario_respuesta, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_notificacion_registro, id_lista_distribucion_detalle_contacto, id_registro, id_estatus, secuencia, fecha_inicio, id_estatus_respuesta, fecha_termino, id_usuario_respuesta, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar
        /// </summary>
        /// <param name="id_lista_distribucion_detalle_contacto"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_estatus"></param>
        /// <param name="secuencia"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="id_estatus_respuesta"></param>
        /// <param name="fecha_termino"></param>
        /// <param name="id_usuario_respuesta"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaNotificacionRegistro(int id_lista_distribucion_detalle_contacto, int id_registro, int id_estatus,
                                                int secuencia, DateTime fecha_inicio, int id_estatus_respuesta, DateTime fecha_termino, int id_usuario_respuesta, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_lista_distribucion_detalle_contacto, id_registro, id_estatus, secuencia, fecha_inicio, id_estatus_respuesta, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_termino), id_usuario_respuesta, id_usuario, true, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Editar
        /// </summary>
        /// <param name="id_lista_distribucion_detalle_contacto"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_estatus"></param>
        /// <param name="secuencia"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="id_estatus_respuesta"></param>
        /// <param name="fecha_termino"></param>
        /// <param name="id_usuario_respuesta"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaNotificacionRegistro(int id_lista_distribucion_detalle_contacto, int id_registro, int id_estatus,
                                                int secuencia, DateTime fecha_inicio, int id_estatus_respuesta, DateTime fecha_termino, int id_usuario_respuesta, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(id_lista_distribucion_detalle_contacto, id_registro, id_estatus,
                               secuencia, fecha_inicio, id_estatus_respuesta, fecha_termino, id_usuario_respuesta, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaNotificacionRegistro(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(this._id_lista_distribucion_detalle_contacto, this._id_registro, this._id_estatus,
                               this._secuencia, this._fecha_inicio_notificacion, this._estatus_respuesta, this._fecha_termino_notificacion, this._id_usuario_respuesta, id_usuario, false);
        }
        /// <summary>
		/// Método encargado de Obtener los Detalles 
		/// </summary>
		/// <param name="id_requisicion">Requisición</param>
		/// <returns></returns>
		public static DataTable ObtieneNotificaciones(int id_lista_distribucion_detalle_contacto)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;
            //Inicializando parámetros de inserción           
            object[] parametros = { 4, 0, id_lista_distribucion_detalle_contacto, 0, 0, 0, null, 0, null, 0, 0, false, "", "" };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtDetalles = ds.Tables["Table"];
            }
            //Devovliendo Objeto de Retorno
            return dtDetalles;
        }
        #endregion
    }
}
