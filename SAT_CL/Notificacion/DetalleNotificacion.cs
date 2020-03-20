using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;

namespace SAT_CL.Notificacion
{
    /// <summary>
    /// Clase que permite realizar inserción, edición y consulta de registros de Detalle Notificación
    /// </summary>
    public class DetalleNotificacion : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla detalle notificación
        /// </summary>
        private static string nom_sp = "notificacion.sp_detalle_notificacion_tdn";
        private int _id_detalle_notificacion;
        /// <summary>
        /// Identificador del detalle de una notificación
        /// </summary>
        public int id_detalle_notificacion
        {
            get { return _id_detalle_notificacion; }
        }
        private int _id_notificacion;
        /// <summary>
        /// Identificador de una notificación
        /// </summary>
        public int id_notificacion
        {
            get { return _id_notificacion; }
        }
        private int _id_tipo_evento_notificacion;
        /// <summary>
        /// Idntificador de un tipo de evento notificación
        /// </summary>
        public int id_tipo_evento_notificacion
        {
            get { return _id_tipo_evento_notificacion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacena el estado de habilitación de un registro
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
        public DetalleNotificacion()
        {
            this._id_detalle_notificacion = 0;
            this._id_notificacion = 0;
            this._id_tipo_evento_notificacion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de la consulta de un registro
        /// </summary>
        /// <param name="id_detalle_notificacion">Identificador de referencia de un registro detalle notificación</param>
        public DetalleNotificacion(int id_detalle_notificacion)
        {
            //Invoca al método que busca y asigna a los atributos de la clase los campos de un registro
            cargaAtributos(id_detalle_notificacion);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~DetalleNotificacion()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que busca registros y el resultado lo almacena en los atributos de la clase
        /// </summary>
        /// <param name="id_detalle_notificacion">Identificador del registro a buscar</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_detalle_notificacion)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda del registro DetalleNotificación
            object[] param = { 3, id_detalle_notificacion, 0, 0, 0, false, "", "" };
            //Realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que no sean nulos)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las campos de las filas del dataset y almacena cada campo en los atributos correspondientes
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_detalle_notificacion = id_detalle_notificacion;
                        this._id_notificacion = Convert.ToInt32(r["IdNotificacion"]);
                        this._id_tipo_evento_notificacion = Convert.ToInt32(r["IdTipoEventoNotificacion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro detalle notificación
        /// </summary>
        /// <param name="id_notificacion">Actualiza el identificador de una notificación</param>
        /// <param name="id_tipo_evento_notificacion">Actualiza el identificador de un tipo de evento notificación</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizó la actualización del registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro</param>
        /// <returns></returns>
        private RetornoOperacion editarDetalleNotificacion(int id_notificacion, int id_tipo_evento_notificacion, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos para realizar la actualización del registro
            object[] param = { 2, this._id_detalle_notificacion, id_notificacion, id_tipo_evento_notificacion, id_usuario, habilitar, "", "" };
            //Realiza la actualización de los campos del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que Inserta un registro detalle notificación
        /// </summary>
        /// <param name="id_notificacion">Inserta el identificador de una notificación</param>
        /// <param name="id_tipo_evento_notificacion">Inserta el identificador de un tipo de evento notificación</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizó la actualización del registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarDetalleNotificacion(int id_notificacion, int id_tipo_evento_notificacion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos para realizar la inserción del registro detalle notificación
            object[] param = { 1, 0, id_notificacion, id_tipo_evento_notificacion, id_usuario, true, "", "" };
            //Realiza la inserción de los campos del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro detalle notificación
        /// </summary>
        /// <param name="id_notificacion">Actualiza el identificador de una notificación</param>
        /// <param name="id_tipo_evento_notificacion">Actualiza el identificador de un tipo de evento notificación</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarDetalleNotificacion(int id_notificacion, int id_tipo_evento_notificacion, int id_usuario)
        {
            //Invoca al método que realiza la busqueda de registros y asigna los atributos los valores encontrados
            return this.editarDetalleNotificacion(id_notificacion, id_tipo_evento_notificacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que modifica el estado de uso del registro
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo el cambio de estado al registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarDetallleNotificacion(int id_usuario)
        {
            //Invoca al método que realiza la busqueda de registros y asigna a los atributos los valores encontrados
            return this.editarDetalleNotificacion(this._id_notificacion, this._id_tipo_evento_notificacion, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDetalleNotificacion()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_detalle_notificacion);
        }

        /// <summary>
        /// Obtenemos los Detalles de Notificación ,ligando un Id Notificación
        /// </summary>
        /// <param name="id_notificacion">Id Notificación</param>
        /// <returns></returns>
        public static DataTable CargaDetalleNotificacion(int id_notificacion)
        {
            //Declaramos variable 
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = {4, 0, id_notificacion, 0, 0, false, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos los Detalles
                    mit = ds.Tables["Table"];
                    

                }
            }
            //Devolvemos Resultado
            return mit;
        }
        #endregion
    }
}
