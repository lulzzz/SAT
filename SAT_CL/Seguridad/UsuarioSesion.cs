using System;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Proporciona los medios para la administración de entidades Usuario
    /// </summary>
    public class UsuarioSesion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los posibles estatus de la sesión 
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Activo
            /// </summary>
            Activo = 1,
            /// <summary>
            /// Terminado
            /// </summary>
            Terminado,
        }

        /// <summary>
        /// Define los posibles Tipos de Dispositivos desde los que se puede aperturar una sesión
        /// </summary>
        public enum TipoDispositivo
        {
            /// <summary>
            /// Todo tipo de dispositivo (Utilizar sólo en reportes)
            /// </summary>
            Indistinto = 0,
            /// <summary>
            /// Equipo de Escritorio
            /// </summary>
            Escritorio = 1,
            /// <summary>
            /// Equipo Portátil
            /// </summary>
            Portatil = 2,
            /// <summary>
            /// Dispositivo Android
            /// </summary>
            Android = 3,
            /// <summary>
            /// Dispositivo de conexion web service
            /// </summary>
            ServicioWeb = 4,
            /// <summary>
            /// Dispositivo desconocido
            /// </summary>
            Desconocido = 5,

        }

        #endregion

        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure usado por la  clase
        /// </summary>
        private static string _nombre_stored_procedure = "seguridad.sp_usuario_sesion_tus";

        private int _id_usuario_sesion;
        /// <summary>
        /// Describe el Id  de Usuario Sesión
        /// </summary>
        public int id_usuario_sesion
        {
            get { return _id_usuario_sesion; }
        }
        private int _id_usuario;
        /// <summary>
        /// Describe el Id de Usuraio 
        /// </summary>
        public int id_usuario
        {
            get { return _id_usuario; }
        }
        private int _id_compania_emisor_receptor;
        /// <summary>
        /// Describe la Compañia
        /// </summary>
        public int id_compania_emisor_receptor
        {
            get { return _id_compania_emisor_receptor; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Describe Estatus
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Describe la fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio
        {
            get { return _fecha_inicio; }
        }
        private DateTime _ultima_actividad;
        /// <summary>
        /// Describe la Ultima Actividad
        /// </summary>
        public DateTime ultima_actividad
        {
            get { return _ultima_actividad; }
        }
        private DateTime _fecha_expiracion;
        /// <summary>
        /// Describe la fecha de expiración
        /// </summary>
        public DateTime fecha_expiracion
        {
            get { return _fecha_expiracion; }
        }
        private byte _id_tipo_dispositivo;
        /// <summary>
        /// Describe el Tipo de Dispositico
        /// </summary>
        public byte id_tipo_dispositivo
        {
            get { return _id_tipo_dispositivo; }
        }
        private string _direccion_mac;
        /// <summary>
        /// Describe la Dirección Mac
        /// </summary>
        public string direccion_mac
        {
            get { return _direccion_mac; }
        }
        private string _nombre_dispositivo;
        /// <summary>
        /// Describe el nombre del dispositivo
        /// </summary>
        public string nombre_dispositivo
        {
            get { return _nombre_dispositivo; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        /// <summary>
        /// Enumera el Estatus de la Sesión
        /// </summary>
        public Estatus EstatusSesion
        {
            get { return (Estatus)_id_estatus; }
        }

        /// <summary>
        /// Enumera el Tipo de Dispositivo
        /// </summary>
        public TipoDispositivo TDispositivo
        {
            get { return (TipoDispositivo)_id_tipo_dispositivo; }
        }
        #endregion

        #region Constructores

        /// <summary>
        /// Crea una nueva instancia del tipo UsuarioSesion a partir del Id solicitado
        /// </summary>
        /// <param name="id_usuario_sesion">Id de Usuario Sesión</param>
        public UsuarioSesion(int id_usuario_sesion)
        {
            cargaAtributosInstancia(id_usuario_sesion);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Libera Recursos
        /// </summary>
        ~UsuarioSesion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Genera una Instancia dr Tipo Usuario Sesión ligado a un Id
        /// </summary>
        /// <param name="id_usuario_sesion">Id Usuario Sesión</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_usuario_sesion)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 3, id_usuario_sesion, 0, 0, 0, null, null, null, 0, "", "", 0, false, "", "" };

            //Realizando consulta hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando para cada registro devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores sobre atributos
                        _id_usuario_sesion = Convert.ToInt32(r["Id"]);
                        _id_usuario = Convert.ToInt32(r["IdUsuario"]);
                        _id_compania_emisor_receptor = Convert.ToInt32(r["IdCompania"]);
                        _id_estatus = Convert.ToByte(r["IdEstatus"]);
                        _fecha_inicio = Convert.ToDateTime(r["FechaInicio"]);
                        _ultima_actividad = Convert.ToDateTime(r["UltimaActividad"]);
                        DateTime.TryParse(r["FechaExpiracion"].ToString(), out _fecha_expiracion);
                        _id_tipo_dispositivo = Convert.ToByte(r["IdTipoDispositivo"]);
                        _direccion_mac = r["DireccionMAC"].ToString();
                        _nombre_dispositivo = r["NombreDispositivo"].ToString();
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        //Indicando resultado correcto de signación
                        resultado = true;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Edita un Usuario Sesión 
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="id_compania_emisor_receptor">Id Compania</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="ultima_actividad"> Ultima Actividad</param>
        /// <param name="fecha_expiracion">Fecha Expiracion</param>
        /// <param name="tipo_dispositivo">Tipo Dispositivo</param>
        /// <param name="direccion_mac">Dirección</param>
        /// <param name="nombre_dispositivo">Nombre Dispositivo</param>
        /// <param name="id_usuario_actualiza">Id Usuario actualiza</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaUsuarioSesion(int id_usuario, int id_compania_emisor_receptor, Estatus estatus, DateTime fecha_inicio, DateTime ultima_actividad,
                                                  DateTime fecha_expiracion, TipoDispositivo tipo_dispositivo, string direccion_mac, string nombre_dispositivo, int id_usuario_actualiza,
                                                 bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = {2, this._id_usuario_sesion, id_usuario, id_compania_emisor_receptor, estatus, fecha_inicio, ultima_actividad, 
                               Fecha.ConvierteDateTimeObjeto(fecha_expiracion), id_tipo_dispositivo, direccion_mac, nombre_dispositivo, id_usuario_actualiza, habilitar, "", ""};

            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }
        /// <summary>
        /// Obtenemos la Fecha de expiración de la Sesión
        /// </summary>
        /// <param name="tiempoExpiracion">Tiempo de Expiración de la Sesión</param>
        /// <param name="ultimo_inicio_sesion">Ultimo Inicio Sesion</param>
        /// <returns></returns>
        private static DateTime obtieneFechaExpiracionSesion(byte tiempoExpiracion, DateTime ultimo_inicio_sesion)
        {
            //Declaramos Fecha de Expiración de la sesión
            DateTime fecha_expiracion_sesion = DateTime.MinValue;

            //Si el Tiempo de Expiración es  != 0 sin limite de expiración
            if (tiempoExpiracion != 0)
            {
                //Obtenemos la Fecha de Expiración
                fecha_expiracion_sesion = ultimo_inicio_sesion.AddMinutes(tiempoExpiracion);
            }

            //Devolvemos Variable
            return fecha_expiracion_sesion;
        }

        #endregion

        #region Metodos publicos

        /// <summary>
        /// Inserta un Usuario Sesión 
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="id_compania_emisor_receptor">Compañia con la que inicio sesión</param>
        /// <param name="tipo_dispositivo">Tipo de Disposito  que utilizó para el Inicio de sesión de la Aplicació</param>
        /// <param name="direccion_mac">Dirección Mac</param>
        /// <param name="nombre_dispositivo">Nombre Dispositivo</param>
        /// <param name="id_usuario_actualiza">Id Usuario actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion IniciaSesion(int id_usuario, int id_compania_emisor_receptor, TipoDispositivo tipo_dispositivo, string direccion_mac, string nombre_dispositivo, int id_usuario_actualiza)
        {
            //Instanciamos Usuario para obtener el Tiempo de Expiración de la Sesión
            using (Usuario objUsuario = new Usuario(id_usuario))
            {
                //Inicializando arreglo de parámetros
                object[] param = {1, 0, id_usuario, id_compania_emisor_receptor, Estatus.Activo, null, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 
                                Fecha.ConvierteDateTimeObjeto(obtieneFechaExpiracionSesion(objUsuario.tiempo_expiracion, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro())), tipo_dispositivo, direccion_mac, nombre_dispositivo, id_usuario_actualiza, true, "", ""};

                //Realizando la actualización
                return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            }
        }
        /// <summary>
        /// Edita un Usuario Sesión 
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="id_compania_emisor_receptor">Id Compania</param>
        /// <param name="ultima_actividad"> Ultima Actividad</param>
        /// <param name="fecha_expiracion">Fecha Expiracion</param>
        /// <param name="tipo_dispositivo">Tipo Dispositivo</param>
        /// <param name="direccion_mac">Dirección</param>
        /// <param name="nombre_dispositivo">Nombre Dispositivo</param>
        /// <param name="id_usuario_actualiza">Id Usuario actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditaUsuarioSesion(int id_usuario, int id_compania_emisor_receptor, DateTime ultima_actividad,
                                                  DateTime fecha_expiracion, TipoDispositivo tipo_dispositivo, string direccion_mac, string nombre_dispositivo, int id_usuario_actualiza
                                                 )
        {

            return this.editaUsuarioSesion(id_usuario, id_compania_emisor_receptor, (Estatus)this._id_estatus, this._fecha_inicio, ultima_actividad, fecha_expiracion, tipo_dispositivo,
                                          direccion_mac, nombre_dispositivo, id_usuario_actualiza, this._habilitar);
        }
        /// <summary>
        /// Deshabilita Usuario Compania
        /// </summary>
        /// <param name="id_usuario_actualiza">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUsuarioSesion(int id_usuario_actualiza)
        {
            return this.editaUsuarioSesion(this._id_usuario, this._id_compania_emisor_receptor, (Estatus)this._id_estatus, this._fecha_inicio, this._ultima_actividad,
                                          this._fecha_expiracion, (TipoDispositivo)this._id_tipo_dispositivo, this._direccion_mac, this._nombre_dispositivo, id_usuario_actualiza, false);
        }
        /// <summary>
        /// Obtiene el No de Sesiones Activas actuales ligando al usuario
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que ha Iniciado sesión</param>
        /// <returns></returns>
        public static byte ObtieneNumeroSesionesActivas(int id_usuario)
        {
            //Declaramos Resultados
            byte NoSesionesActivas = 0;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 4, 0, id_usuario, 0, 0, null, null, null, 0, "", "", 0, false, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos el No. de Sesiones Activas
                    NoSesionesActivas = (from DataRow r in ds.Tables[0].Rows
                                         select Convert.ToByte(r["NoSesionesActivas"])).FirstOrDefault();
                }
            }

            //Obtenemos Resultado
            return NoSesionesActivas;
        }
        /// <summary>
        /// Finaliza todas las sesiones del usuario indicado que ya han expirado  
        /// </summary>
        /// <returns></returns>
        public static void FinalizaSesionesExpiradas(int id_usuario)
        {
            object[] param = { 5, 0, id_usuario, 0, 0, null, null, null, 0, "", "", 0, false, "", "" };

            //Realizando la actualización
            CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }
        /// <summary>
        /// Actualizamos última actividad de usuario 
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ActualizaUltimaActividad()
        {
            //Declaramos Objeto Resultante
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Usuario
            using (Usuario objUsuario = new Usuario(this._id_usuario))
            {
                //Actualizamos Última Actividad y Fecha Expiracion
                resultado = editaUsuarioSesion(this._id_usuario, this._id_compania_emisor_receptor, (Estatus)this._id_estatus,
                                               this._fecha_inicio, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), obtieneFechaExpiracionSesion(objUsuario.tiempo_expiracion, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()), (TipoDispositivo)this._id_tipo_dispositivo,
                                               this._direccion_mac, this._nombre_dispositivo, this._id_usuario, this._habilitar);
            }

            return resultado;
        }
        /// <summary>
        /// Método que Actualiza la Ultima Actividad del Usuario
        /// </summary>
        /// <param name="direccion_mac">Dirección MAC</param>
        /// <param name="nombre_dispositivo">Nombre del Dispositivo</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaUltimaActividad(string direccion_mac, string nombre_dispositivo)
        {
            //Declaramos Objeto Resultante
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Usuario
            using (Usuario objUsuario = new Usuario(this._id_usuario))
            {
                //Actualizamos Última Actividad y Fecha Expiracion
                resultado = editaUsuarioSesion(this._id_usuario, this._id_compania_emisor_receptor, (Estatus)this._id_estatus,
                                               this._fecha_inicio, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), obtieneFechaExpiracionSesion(objUsuario.tiempo_expiracion, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()), (TipoDispositivo)this._id_tipo_dispositivo,
                                               direccion_mac, nombre_dispositivo, this._id_usuario, this._habilitar);
            }

            return resultado;
        }
        /// <summary>
        /// Método encargado de Finalizar las Sesiones de un Usuario, excepto la Actual
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion FinalizaSesionesUsuario()
        {
            //Declaramos Objeto Resultante
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 6, this._id_usuario_sesion, this._id_usuario, 0, 0, null, null, null, 0, "", "", 0, false, "", "" };

            //Instanciando Usuario Sesión Excepción
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Recorriendo Ciclo
                        foreach (DataRow dr in ds.Tables["Table"].Rows)
                        {
                            //Instanciando Sesión
                            using (UsuarioSesion us = new UsuarioSesion(Convert.ToInt32(dr["Id"])))
                            {
                                //Validando que exista la Sesión
                                if (us.habilitar)
                                {
                                    //Terminando Sesión
                                    result = us.TerminarSesion();

                                    //Validando si la Operación no es Exitosa
                                    if (!result.OperacionExitosa)

                                        //Terminando Ciclo
                                        break;
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existe la Sesión");
                            }
                        }

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Positivo el Retorno
                            result = new RetornoOperacion(this._id_usuario_sesion);
                            
                            //Completando Transacción
                            trans.Complete();
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion(this._id_usuario_sesion, "No existen Sesiones Activas", true);
            }

            //Devolvemos resultado
            return result;
        }

        /*
        /// <summary>
        /// Método encargado de Finalizar las Sesiones de Usuario
        /// </summary>
        /// <param name="id_usuario_sesion_excepcion">Sesión Omitida</param>
        /// <returns></returns>
        public static RetornoOperacion FinalizaSesionesUsuario(int id_usuario_sesion_excepcion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando usuario Sesión
            using (UsuarioSesion us = new UsuarioSesion(id_usuario_sesion_excepcion))
            {
                //Validando que Exista la Sesión
                if (us.habilitar)

                    //Finalizando Sesiones
                    result = us.FinalizaSesionesUsuario();
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe la Sesión del Usuario");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        */

        /// <summary>
        /// Da por terminada la sesión actual
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion TerminarSesion()
        {
            return TerminarSesion(this._id_usuario);
        }
        /// <summary>
        /// Da por terminada la sesión actual
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que finaliza la sesión</param>
        /// <returns></returns>
        public RetornoOperacion TerminarSesion(int id_usuario)
        {
            //Declaramos Objeto Resultante
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que la Sesion se encuentre activa
            if ((Estatus)this._id_estatus == Estatus.Activo)
            {
                //Finalizando Sesión
                resultado = editaUsuarioSesion(this._id_usuario, this._id_compania_emisor_receptor, Estatus.Terminado, this._fecha_inicio, this._ultima_actividad, this._fecha_expiracion,
                                           this.TDispositivo, this._direccion_mac, this._nombre_dispositivo, id_usuario, this._habilitar);

                //Validando Operación Exitosa
                if (resultado.OperacionExitosa && this.TDispositivo == TipoDispositivo.Android)
                
                    //Cerrando Sesión 
                    Global.NotificacionPush.Instance.NotificacionCierreSesion(resultado.IdRegistro);
            }

            //Devolvemos resultado
            return resultado;
        }
        /// <summary>
        /// Actualiza atributos Usuario Sesion
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUsuarioSesion()
        {
            return this.cargaAtributosInstancia(this._id_usuario_sesion);
        }
        /// <summary>
        /// Recupera las sesiones activas para el usuario indicado en el tipo de dispositivo señalado
        /// </summary>
        /// <param name="id_usuario">Id de usuario a consultar</param>
        /// <param name="tipo_dispositivo">Tipo de Dispositivo a buscar</param>
        /// <returns></returns>
        public static DataTable ObtieneSesionesActivasUsuario(int id_usuario, TipoDispositivo tipo_dispositivo)
        { 
            //Declarando objeto de retorno
            DataTable mit = null;

            //Inicializando criterios de búsqueda
            object[] param = { 6, 0, id_usuario, 0, 0, null, null, null, (byte)tipo_dispositivo, "", "", 0, false, "", "" };

            //Realizando búsqueda
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }
        /// <summary>
        /// Método encargado de Obtener Todas las Sesiones Activas
        /// </summary>
        /// <param name="id_usuario">Usuario Deseado</param>
        /// <returns></returns>
        public static DataTable ObtieneSesionesActivasUsuario(int id_usuario)
        {
            //Declarando objeto de retorno
            DataTable dtSesionesActivas = null;

            //Inicializando criterios de búsqueda
            object[] param = { 8, 0, id_usuario, 0, 0, null, null, null, 0, "", "", 0, false, "", "" };

            //Realizando búsqueda
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Valores
                    dtSesionesActivas = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtSesionesActivas;
        }

        #endregion
    }
}
