using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Notificacion
{
    /// <summary>
    /// Clase que permite realizar inserción, edición y Consulta de registros de tipo Evento Notificación
    /// </summary>
    public class TipoEventoNotificacion:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp de la tabla TipoEventoNotificación
        /// </summary>
        private static string nom_sp = "notificacion.sp_tipo_evento_notificacion_tten";
        private int _id_tipo_evento_notificacion;
        /// <summary>
        /// Identifica el tipo de un evento de notificación
        /// </summary>
        public int id_tipo_evento_notificacion
        {
            get { return _id_tipo_evento_notificacion; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Identifica a la compañia a la que pertenece el evento de notificación
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private int _id_tabla;
        /// <summary>
        /// Identifica  la entidad a la que hace referencia el eventos
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private string _descripcion;
        /// <summary>
        /// Nombre o caracteristicas que permite identificar al tipo de evento notificación
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private string _mensaje;
        /// <summary>
        /// Texto del tipo de evento notificación 
        /// </summary>
        public string mensaje
        {
            get { return _mensaje; }
        }
        private bool _habilitar;
        /// <summary>
        /// Define la disponiblidad de uso de un registro (Habilitado - Disponible, Deshabilitado - No Disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos a cero
        /// </summary>
        public TipoEventoNotificacion()
        {
            this._id_tipo_evento_notificacion = 0;
            this._id_compania_emisor = 0;
            this._id_tabla = 0;
            this._descripcion = "";
            this._mensaje = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos 
        /// </summary>
        /// <param name="id_tipo_evento_notificacion">Identificador que sirve como referencia para inicializar los atributos de la clase</param>
        public TipoEventoNotificacion(int id_tipo_evento_notificacion)
        {
            //Invoca al métod que busca y asigna valores a los atributos
            cargaAtributos(id_tipo_evento_notificacion);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~TipoEventoNotificacion()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro y el resultado lo almacena en los atributos de la clase
        /// </summary>
        /// <param name="id_tipo_evento_notificacion">Identificador de referencia para realizar la busqueda</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_tipo_evento_notificacion)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta de registro
            object[] param = { 3, id_tipo_evento_notificacion, 0, 0, "", "", 0, false, "", "" };
            //Realiza la instancia a la clase TipoEventoNotificación
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que no sean nulos)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna las filas del dataset a los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_tipo_evento_notificacion = id_tipo_evento_notificacion;
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._mensaje = Convert.ToString(r["Mensaje"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorna el método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de Tipo Evento Notificación
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el identificador de la campañia a la que pertenece el registro de Tipo Evento Notificación</param>
        /// <param name="id_tabla">Actualiza el identificador de la entidad a la que pertenece el registro de Tipo Evento Notificación </param>
        /// <param name="descripcion">Actualiza el nombre o las caracteristicas descriptivas de un Tipo Evento Notificación</param>
        /// <param name="mensaje">Actualiza el texto del mésaje de Tipo Evento Notificación</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de habilitación de un registro</param>
        /// <returns></returns>
        private RetornoOperacion editarTipoEventoNotificacion(int id_compania_emisor, int id_tabla, string descripcion, string mensaje, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar los campos de un registro
            object[] param = { 2, this._id_tipo_evento_notificacion, id_compania_emisor, id_tabla, descripcion, mensaje, id_usuario, habilitar, "", "" };
            //Realiza la actualización  de los campos del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Regresa al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Método Públicos
        /// <summary>
        /// Método que inserta  un registro de Tipo Evento Notificación
        /// </summary>
        /// <param name="id_compania_emisor">Inserta el identificador de la campañia a la que pertenece el registro de Tipo Evento Notificación</param>
        /// <param name="id_tabla">Inserta el identificador de la entidad a la que pertenece el registro de Tipo Evento Notificación </param>
        /// <param name="descripcion">Inserta el nombre o las caracteristicas descriptivas de un Tipo Evento Notificación</param>
        /// <param name="mensaje">Inserta el texto del ménsaje de Tipo Evento Notificación</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarTipoEventoNotificacion(int id_compania_emisor, int id_tabla, string descripcion, string mensaje, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar los campos de un registro
            object[] param = { 1, 0, id_compania_emisor, id_tabla, descripcion, mensaje, id_usuario, true, "", "" };
            //Realiza la actualización  de los campos del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Regresa al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de Tipo Evento Notificación
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el identificador de la campañia a la que pertenece el registro de Tipo Evento Notificación</param>
        /// <param name="id_tabla">Actualiza el identificador de la entidad a la que pertenece el registro de Tipo Evento Notificación </param>
        /// <param name="descripcion">Actualiza el nombre o las caracteristicas descriptivas de un Tipo Evento Notificación</param>
        /// <param name="mensaje">Actualiza el texto del ménsaje de Tipo Evento Notificación</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarTipoEventoNotificacion(int id_compania_emisor, int id_tabla, string descripcion, string mensaje, int id_usuario)
        {
            //Retorna el método que realiza la actualización de campos de un registro
            return this.editarTipoEventoNotificacion(id_compania_emisor, id_tabla, descripcion, mensaje, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro 
        /// </summary>
        /// <param name="id_usuario">Permite identifiacr al usuario que realizo la acción de deshabilitar el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarTipoEventoNotificacion(int id_usuario)
        {
            //Retorna el método que realiza la actualización de campos de un registro
            return this.editarTipoEventoNotificacion(this._id_compania_emisor, this._id_tabla, this._descripcion, this._mensaje, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTipoEventoNotificacion()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_tipo_evento_notificacion);
        }
        #endregion
    }
}
