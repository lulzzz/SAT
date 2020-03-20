using System;
using System.Data;
using TSDK.Datos;
using TSDK.Base;

namespace SAT_CL.Autorizacion
{
    /// <summary>
    /// Proporciona los metodos para Administrar los Detalles de las Autorizaciones Detalle Bloque Responsable.
    /// </summary>
    public class AutorizacionDetalleBloqueResponsable : Disposable
    {
        #region Propiedades
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "Autorizacion.sp_autorizacion_detalle_bloque_responsable";


        private int _id_autorizacion_detalle_bloque_responsable;
        /// <summary>
        /// Id Autorizacion Detalle Bloque Responsable
        /// </summary>
        public int id_autorizacion_detalle_bloque_responsable
        {
            get { return _id_autorizacion_detalle_bloque_responsable; }
        }

        private int _id_autorizacion_detalle_bloque;
        /// <summary>
        /// Id Autorizacion Detalle Bloque
        /// </summary>
        public int id_autorizacion_detalle_bloque
        {
            get { return _id_autorizacion_detalle_bloque; }

        }

        private int _id_usuario_responsable;
        /// <summary>
        /// Id del responsable de la autorizacion
        /// </summary>
        public int id_usuario_responsable
        {
            get { return _id_usuario_responsable; }
        }

        private bool _bit_sms;
        /// <summary>
        /// Activacion Sms
        /// </summary>
        public bool bit_sms
        {
            get { return _bit_sms; }
        }

        private bool _bit_email;
        /// <summary>
        /// Envío de Email
        /// </summary>
        public bool bit_email
        { get { return this._bit_email; } }

        private bool _habilitar;
        /// <summary>
        /// Estado de una Autorizacion Detalle
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AutorizacionDetalleBloqueResponsable()
        {
            Dispose(false);
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Genera una instancia de tipo Autorizacion Detalle Bloque Responsable
        /// </summary>
        public AutorizacionDetalleBloqueResponsable()
        {
            _id_autorizacion_detalle_bloque_responsable = 0;
            _id_autorizacion_detalle_bloque = 0;
            _id_usuario_responsable = 0;
            _bit_sms = false;
            _bit_email = false;
            _habilitar = false;
        }
        /// <summary>
        /// Genera una nueva instancia de tipo Autorización Detalle Bloque Responsable dado un id 
        /// </summary>
        /// <param name="id_autorizacion_detalle_bloque_responsable"></param>
        public AutorizacionDetalleBloqueResponsable(int id_autorizacion_detalle_bloque_responsable)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 3, id_autorizacion_detalle_bloque_responsable, 0, 0, false, false, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_autorizacion_detalle_bloque_responsable = Convert.ToInt32(r["IdAutorizacionDetalleBloqueResponsable"]);
                        _id_autorizacion_detalle_bloque = Convert.ToInt32(r["IdAutorizacionDetalleBloque"]);
                        _id_usuario_responsable = Convert.ToInt32(r["IdUsuarioResponsable"]);
                        _bit_sms = Convert.ToBoolean(r["BitSMS"]);
                        _bit_email = Convert.ToBoolean(r["BitEmail"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
            }
        }
        #endregion

        #region Metodos Privados
        /// <summary>
        /// Metodo encargado de Editar un Detalle Autoriazacion Bloque Responsable
        /// </summary>
        /// <param name="id_autorizacion_detalle_bloque"></param>
        /// <param name="id_usuario_responsable"></param>
        /// <param name="bit_sms"></param>
        /// <param name="bit_email"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaAutorizacionDetalleBloqueResponsable(int id_autorizacion_detalle_bloque, int id_usuario_responsable, bool bit_sms, bool bit_email, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_autorizacion_detalle_bloque_responsable, id_autorizacion_detalle_bloque, id_usuario_responsable, bit_sms, bit_email, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        #endregion

        #region Metodos Publicos
        /// <summary>
        /// Metodo encargado de insertar una Autorizacion Detalle Bloque Responsable
        /// </summary>
        /// <param name="id_autorizacion_detalle_bloque"></param>
        /// <param name="id_usuario_responsable"></param>
        /// <param name="bit_sms"></param>
        /// <param name="bit_email"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAutorizacionDetalleBloqueResponsable(int id_autorizacion_detalle_bloque, int id_usuario_responsable, bool bit_sms, bool bit_email, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_autorizacion_detalle_bloque, id_usuario_responsable, bit_sms, bit_email, id_usuario, true, "", "" };
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        /// <summary>
        /// Metodo encargado de editar una Autorizacion Detalle Bloque Responsable
        /// </summary>
        /// <param name="id_autorizacion_detalle_bloque"></param>
        /// <param name="id_usuario_responsable"></param>
        /// <param name="bit_sms"></param>
        /// <param name="bit_email"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaAutorizacionDetalleBloqueResponsable(int id_autorizacion_detalle_bloque, int id_usuario_responsable, bool bit_sms, bool bit_email, int id_usuario)
        {
            return this.editaAutorizacionDetalleBloqueResponsable(id_autorizacion_detalle_bloque, id_usuario_responsable, bit_sms, bit_email, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Deshabilita una Autorizacion Detalle Bloque Responsable
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAutorizacionDetalleBloqueResponsable(int id_usuario)
        {
            return this.editaAutorizacionDetalleBloqueResponsable(this._id_autorizacion_detalle_bloque, this._id_usuario_responsable, this._bit_sms, this._bit_email, id_usuario, false);

        }

        /// <summary>
        /// Realiza la carga de los responsables ligados a un bloque específico
        /// </summary>
        /// <param name="id_autorizacion_detalle_bloque">Id de Bloque de Detalle de Autorización</param>
        /// <returns></returns>
        public static DataTable CargaResponsablesAutorizacionDetalleBloque(int id_autorizacion_detalle_bloque)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Definiendo arreglo de objetos que contendrá al conjunto de criterios de consulta
            object[] param = { 4, 0, id_autorizacion_detalle_bloque, 0, false, false, 0, false, "", "" };

            //Realizando la búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando origen de datos
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga los valores de autorización asignadas por los responsables de un bloque
        /// </summary>
        /// <param name="id_autorizacion_detalle_bloque">Id de Bloque de Detalle de Autorización</param>
        /// <param name="id_tabla">Id de Tabla a consultar</param>
        /// <param name="id_registro">Id de Registro a consultar</param>
        /// <returns></returns>
        public static DataTable CargaAutorizacionResponsablesBloque(int id_autorizacion_detalle_bloque, int id_tabla, int id_registro)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Definiendo arreglo de objetos que contendrá al conjunto de criterios de consulta
            object[] param = { 5, 0, id_autorizacion_detalle_bloque, 0, false, false, 0, false, id_tabla.ToString(), id_registro.ToString() };

            //Realizando la búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando origen de datos
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Autorizaciones Pendientes Depositos
        /// </summary>
        /// <returns></returns>
        public static DataTable CargaAutorizacionesPendientesDepositos(string No_Orden)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Definiendo arreglo de objetos que contendrá al conjunto de criterios de consulta
            object[] param = { 6, 0, 0, 0, false, false, 0, false, No_Orden, "" };

            //Realizando la búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando origen de datos
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion
    }
}

