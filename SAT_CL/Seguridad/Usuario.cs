using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
using System.Collections.Generic;
namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Proporciona los medios para la administración de entidades Usuario
    /// </summary>
    public class Usuario : Disposable
    {
        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure usado por la  clase
        /// </summary>
        private static string _nombre_stored_procedure = "seguridad.sp_usuario_tu";

        private int _id_usuario;
        /// <summary>
        /// Obtiene el Id de usuario
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }
        private string _nombre;
        /// <summary>
        /// Obtiene el nombre del usuario
        /// </summary>
        public string nombre { get { return this._nombre; } }
        private string _email;
        /// <summary>
        /// Obtiene el email del usuario
        /// </summary>
        public string email { get { return this._email; } }
        private string _contrasena;
        private DateTime _fecha_contrasena;
        /// <summary>
        /// Obtiene la fecha de la última modificación de contraseña
        /// </summary>
        public DateTime fecha_contrasena { get { return this._fecha_contrasena; } }
        private byte _dias_vigencia;
        /// <summary>
        /// Obtiene los días de vigencia de la contraseña sobre la fecha de última modificación de la misma
        /// </summary>
        public byte dias_vigencia { get { return this._dias_vigencia; } }
        private byte _tiempo_expiracion;
        /// <summary>
        /// Obtiene  el tiempo de expiración
        /// </summary>
        public byte tiempo_expiracion { get { return this._tiempo_expiracion; } }
        private string _pregunta_secreta;
        /// <summary>
        /// Obtiene la pregunta secreta del usuario
        /// </summary>
        public string pregunta_secreta { get { return this._pregunta_secreta; } }
        private string _respuesta_secreta;
        private byte _sesiones_disponibles;
        /// <summary>
        /// Obtiene el número de sesiones totales disponibles para el usuario
        /// </summary>
        public byte sesiones_disponibles { get { return this._sesiones_disponibles; } }
        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del usuario
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        public string notificaciones;

        /// <summary>
        /// Obtiene la configuración del usuario
        /// </summary>
        public Dictionary<string, string> Configuracion;
        #endregion

        #region Constructores

        /// <summary>
        /// Crea una nueva instancia del tipo Usuario a partir del Id solicitado
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        public Usuario(int id_usuario)
        {
            cargaAtributosInstancia(id_usuario);
        }

        /// <summary>
        /// Crea una nueva instancia del tipo Usuario a partir de un email solicitado
        /// </summary>
        /// <param name="email">Email</param>
        public Usuario(string email)
        {
            cargaAtributosInstancia(email);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Libera los recursos utilizados por la instancia
        /// </summary>
        ~Usuario()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la carga de los atributos de la instncia en base al Id de registro solicitado
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_usuario)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 3, id_usuario, "", "", "", null, 0, 0, "", "", 0, 0, false, "", "" };

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
                        this._id_usuario = Convert.ToInt32(r["Id"]);
                        this._nombre = r["Nombre"].ToString();
                        this._email = r["Email"].ToString();
                        this._contrasena = r["Contrasena"].ToString();
                        DateTime.TryParse(r["FechaContrasena"].ToString(), out this._fecha_contrasena);
                        this._dias_vigencia = Convert.ToByte(r["DiasVigencia"]);
                        this._tiempo_expiracion = Convert.ToByte(r["TiempoExpiracion"]);
                        this._pregunta_secreta = r["PreguntaSecreta"].ToString();
                        this._respuesta_secreta = r["RespuestaSecreta"].ToString();
                        this._sesiones_disponibles = Convert.ToByte(r["SesionesDisponibles"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                        this.notificaciones = r["Notificaciones"].ToString();

                        //Realizando la carga de su configuración
                        cargaReferenciasConfiguracionUsuario();
                        //Indicando resultado correcto de signación
                        resultado = true;
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la carga de los atributos de la instncia en base al email de registro solicitado
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(string email)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 4, 0, "", email, "", null, 0,0, "", "", 0, 0, false, "", "" };

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
                        this._id_usuario = Convert.ToInt32(r["Id"]);
                        this._nombre = r["Nombre"].ToString();
                        this._email = r["Email"].ToString();
                        this._contrasena = r["Contrasena"].ToString();
                        DateTime.TryParse(r["FechaContrasena"].ToString(), out this._fecha_contrasena);
                        this._dias_vigencia = Convert.ToByte(r["DiasVigencia"]);
                        this._tiempo_expiracion = Convert.ToByte(r["TiempoExpiracion"]);
                        this._pregunta_secreta = r["PreguntaSecreta"].ToString();
                        this._respuesta_secreta = r["RespuestaSecreta"].ToString();
                        this._sesiones_disponibles = Convert.ToByte(r["SesionesDisponibles"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                        this.notificaciones = r["Notificaciones"].ToString();

                        //Realizando la carga de su configuración
                        cargaReferenciasConfiguracionUsuario();
                        //Indicando resultado correcto de signación
                        resultado = true;
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Reguistra un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="email">Email</param>
        /// <param name="contrasena">Contraseña deseada</param>
        /// <param name="fecha_contrasena">Fecha de última modificación de la contraseña</param>
        /// <param name="dias_vigencia">Días de vigencia de la contraseña (0 = Sin límite de vigencia)</param>
        /// <param name="tiempo_expiracion">Tiempo de expiracion de la sesion</param>
        /// <param name="pregunta_secreta">Pregunta secreta para recuperación de cuenta</param>
        /// <param name="respuesta_secreta">Respuesta a la pregunta secreta</param>
        /// <param name="sesiones_disponibles">Número de sesiones totales permitidas para el usuario (0 = Sin límite de sesiones)</param>
        /// <param name="id_usuario_actualiza">Id de usuario que registra al nuevo usuario</param>
        /// <param name="habilitar">Valor de habilitación del usuario</param>
        /// <returns></returns>
        private RetornoOperacion editaUsuario(string nombre, string email, string contrasena, DateTime fecha_contrasena, byte dias_vigencia, byte tiempo_expiracion, string pregunta_secreta, string respuesta_secreta, byte sesiones_disponibles, int id_usuario_actualiza, bool habilitar)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 2, this._id_usuario, nombre, email, contrasena, fecha_contrasena, dias_vigencia, tiempo_expiracion, pregunta_secreta, respuesta_secreta, sesiones_disponibles, id_usuario_actualiza, habilitar, "", "" };

            //Creando nuevo usuario en BD
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Reguistra un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="email">Email</param>
        /// <param name="contrasena">Contraseña deseada</param>
        /// <param name="fecha_contrasena">Fecha de edición de contraseña</param>
        /// <param name="dias_vigencia">Días de vigencia de la contraseña (0 = Sin límite de vigencia)</param>
        /// <param name="tiempo_expiracion">Tiempo de expiración de la sesión</param>
        /// <param name="pregunta_secreta">Pregunta secreta para recuperación de cuenta</param>
        /// <param name="respuesta_secreta">Respuesta a la pregunta secreta</param>
        /// <param name="sesiones_disponibles">Número de sesiones totales permitidas para el usuario (0 = Sin límite de sesiones)</param>
        /// <param name="id_usuario_actualiza">Id de usuario que registra al nuevo usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUsuario(string nombre, string email, string contrasena, DateTime fecha_contrasena, byte dias_vigencia, byte tiempo_expiracion, string pregunta_secreta, string respuesta_secreta, byte sesiones_disponibles, int id_usuario_actualiza)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 1, 0, nombre, email, Encriptacion.CalculaHashCadenaEnBase64(contrasena, Encriptacion.MetodoDigestion.SHA1), fecha_contrasena,
                                dias_vigencia, tiempo_expiracion, pregunta_secreta, Encriptacion.CalculaHashCadenaEnBase64(respuesta_secreta, Encriptacion.MetodoDigestion.SHA1), sesiones_disponibles, id_usuario_actualiza, true, "", "" };

            //Creando nuevo usuario en BD
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Reguistra un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="email">Email</param>
        /// <param name="dias_vigencia">Días de vigencia de la contraseña (0 = Sin límite de vigencia)</param>
        /// <param name="tiempo_expiracion">Tiempo de expiraión de la sesión</param>
        /// <param name="sesiones_disponibles">Número de sesiones totales permitidas para el usuario (0 = Sin límite de sesiones)</param>
        /// <param name="id_usuario_actualiza">Id de usuario que registra al nuevo usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUsuario(string nombre, string email, byte dias_vigencia,  byte tiempo_expiracion, byte sesiones_disponibles, int id_usuario_actualiza)
        {
            //Devolviendo resultado
            return InsertaUsuario(nombre, email, "", DateTime.Today, dias_vigencia, tiempo_expiracion, "", "", sesiones_disponibles, id_usuario_actualiza);
        }
        /// <summary>
        /// Realiza la edición de los datos no confidenciales del usuario
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="email">Email</param>
        /// <param name="sesiones_disponibles">Número de sesiones totales permitidas para el usuario</param>
        /// <param name="id_usuario_actualiza">Id de usuario que registra al nuevo usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaInformacionGeneral(string nombre, string email, byte sesiones_disponibles, int id_usuario_actualiza)
        {
            return this.editaUsuario(nombre, email, this._contrasena, this._fecha_contrasena, this._dias_vigencia, this._tiempo_expiracion, this._pregunta_secreta, this._respuesta_secreta, sesiones_disponibles, id_usuario_actualiza, this._habilitar);
        }

        /// <summary>
        /// Método que realiza la actualización de los campos generales de un usuario
        /// </summary>
        /// <param name="nombre">Permite la actualización del nombre del usuario</param>
        /// <param name="email">Permite actualizar el correo electronico con la cual se dio de alta el usuario</param>
        /// <param name="sesiones_disponibles">Permite actualizar el número de sesiones que dispondra el usuario</param>
        /// <param name="tiempo_expiracion">Permite actualizar el rango de expiración de inicio de sesion del usuario</param>
        /// <param name="dias_vigencia">Permite actualizar los dias habilies en las que podra iniciar sesión con su cuenta de usuario</param>
        /// <param name="id_usuario_actualiza">Permite actualizar al usuario que realizo actualizaciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaInformaciónGeneral(string nombre, string email, byte sesiones_disponibles,
                                                        byte tiempo_expiracion, byte dias_vigencia, int id_usuario_actualiza)
        {
            return this.editaUsuario(nombre, email, this._contrasena, this._fecha_contrasena, dias_vigencia, tiempo_expiracion,
                                     this._pregunta_secreta, this._respuesta_secreta, sesiones_disponibles, id_usuario_actualiza,
                                     this._habilitar);
        }

        /// <summary>
        /// Realiza la edición de la contraseña de usuario
        /// </summary>
        /// <param name="contrasena">Nueva contraseña asignada</param>
        /// <param name="id_usuario_actualiza">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditaContrasena(string contrasena, int id_usuario_actualiza)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Obteniendo hash de nueva contraseña
            string hashContrasena = Encriptacion.CalculaHashCadenaEnBase64(contrasena, Encriptacion.MetodoDigestion.SHA1);
            //Si se ha modificado la contraseña, se asigna nueva fecha
            DateTime fechaContrasena = hashContrasena != this._contrasena ? DateTime.Today : this._fecha_contrasena;
            //Realizando actualización
            resultado = this.editaUsuario(this._nombre, this._email, hashContrasena, fechaContrasena, this._dias_vigencia, this._tiempo_expiracion, this._pregunta_secreta, this._respuesta_secreta, this._sesiones_disponibles, id_usuario_actualiza, this._habilitar);
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Editar la Contraseña del Usuario
        /// </summary>
        /// <param name="contrasena_nueva"></param>
        /// <param name="contrasena_actual"></param>
        /// <param name="id_usuario_actualiza"></param>
        /// <returns></returns>
        public RetornoOperacion EditaContrasena(string contrasena_nueva, string contrasena_actual, int id_usuario_actualiza)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Encriptando Contraseña Ingresada
            string contrasena_actual_encriptada = Encriptacion.CalculaHashCadenaEnBase64(contrasena_actual, Encriptacion.MetodoDigestion.SHA1);
            string contrasena_nueva_encriptada = Encriptacion.CalculaHashCadenaEnBase64(contrasena_nueva, Encriptacion.MetodoDigestion.SHA1);

            //Validando que la Contraseña Actual
            if (contrasena_actual_encriptada.Equals(this._contrasena))
            {
                //Validando la Contraseña Nueva con la Actual
                if (!contrasena_nueva_encriptada.Equals(this._contrasena))

                    //Actualizando Contraseña
                    resultado = this.editaUsuario(this._nombre, this._email, contrasena_nueva_encriptada, Fecha.ObtieneFechaEstandarMexicoCentro(), this._dias_vigencia,
                                    this._tiempo_expiracion, this._pregunta_secreta, this._respuesta_secreta, this._sesiones_disponibles, id_usuario_actualiza, this._habilitar);
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("Ingrese una Contraseña Diferente a la Actual");
            }
            else
                //Instanciando Excepción
                resultado = new RetornoOperacion("Verifique la contraseña actual");

            //Devolviendo Resultado Obtenido
            return resultado;
        }

        /// <summary>
        /// Realiza la edición de los datos de recuperación de contraseña
        /// </summary>
        /// <param name="pregunta_secreta">Pregunta secreta</param>
        /// <param name="respuesta_secreta">Respuesta secreta</param>
        /// <param name="id_usuario_actualiza">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditaPreguntaSecreta(string pregunta_secreta, string respuesta_secreta, int id_usuario_actualiza)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Obteniendo hash de nueva respuesta secreta
            string hashRespuesta = Encriptacion.CalculaHashCadenaEnBase64(respuesta_secreta, Encriptacion.MetodoDigestion.SHA1);

            //Realizando actualización
            resultado = this.editaUsuario(this._nombre, this._email, this._contrasena, this._fecha_contrasena, this._dias_vigencia, this._tiempo_expiracion,
                        pregunta_secreta, hashRespuesta, this._sesiones_disponibles, id_usuario_actualiza, this._habilitar);
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Deshabilita el registro usuario
        /// </summary>
        /// <param name="id_usuario_actualiza">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUsuario(int id_usuario_actualiza)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando actualización
            resultado = this.editaUsuario(this._nombre, this._email, this._contrasena, this._fecha_contrasena, this._dias_vigencia, this._tiempo_expiracion,
                      this._pregunta_secreta, this._respuesta_secreta, this._sesiones_disponibles, id_usuario_actualiza, false);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Inicia Sesión el Usuario
        /// </summary>
        /// <param name="contrasena">Contraseña proporcionada por el Usuario</param>
        /// <returns></returns>
        public RetornoOperacion AutenticaUsuario(string contrasena)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que exista Usuario
            if(this._id_usuario > 0)
            {
                //Validando asignación de un perfil de usuario activo
                using (PerfilSeguridadUsuario perfilActivo = PerfilSeguridadUsuario.ObtienePerfilActivo(this._id_usuario))
                {
                    //Si hay perfil activo
                    if (perfilActivo.id_perfil > 0)
                    {
                        //Obtenemos Contraseña proporcionada
                        contrasena = Encriptacion.CalculaHashCadenaEnBase64(contrasena, Encriptacion.MetodoDigestion.SHA1);

                        //Comparamos contraseña actual con la proporcionada
                        if (this._contrasena == contrasena)
                        {

                            //Actualizamos sesiones que ya han expirado
                            UsuarioSesion.FinalizaSesionesExpiradas(this.id_usuario);
                            /*Validamos el No. de Sesiones Disponibles para el Usuario comparando las que se encuentran actualmente Activas.
                                                Sesiones Disponible = 0 sin limite de sesiones */
                            if ((UsuarioSesion.ObtieneNumeroSesionesActivas(this._id_usuario) < this._sesiones_disponibles) || (this.sesiones_disponibles == 0))
                                //Regresamos Id de Usuario
                                resultado = new RetornoOperacion(this._id_usuario);
                            else
                            {
                                //Mostrando Mensaje 
                                resultado = new RetornoOperacion("Ha alcanzado el número de sesiones permitadas o no ha cerrado correctamente su última sesión.");
                            }
                        }
                        else
                        {
                            //Mostramos Error
                            resultado = new RetornoOperacion("Verifique usuario y/o contraseña.");
                        }
                    }
                    //Si no hay perfil de seguridad activo
                    else
                    {
                        resultado = new RetornoOperacion("No cuenta con un perfil de seguridad activo, contacte con el administrador del sitio.");
                    }
                }
            }
            else
            {
                resultado = new RetornoOperacion("No existe un usuario con los datos proporcionados.");
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la carga de las referencias usadas en la configuración del usuario
        /// </summary>
        /// <returns></returns>
        private void cargaReferenciasConfiguracionUsuario()
        {
            //Inicializando arreglo de parámetros para consulta
            object[] param = { 5, this._id_usuario, "", "", "", null, 0, 0, "", "", 0, 0, false, "", "" };

            //Realziando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si existe el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando conjunto de elementos al diccionario de salida
                    this.Configuracion = (from DataRow r in ds.Tables["Table"].Rows
                                          select r).ToDictionary(c => c.Field<string>("Descripcion"), c => c.Field<string>("Valor"));
                }
                else
                {   //Añade valores por Default
                   this.Configuracion =new  Dictionary<string, string>();

                   this.Configuracion.Add("0", "Sin Asignar");
                }
            }
        }

        /// <summary>
        /// Realiza la actualización de los valores de atributos de la instancia volviendo a leer desde BD
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributos()
        {
            return cargaAtributosInstancia(this._id_usuario);
        }


        #endregion

        #region Métodos Android

        /// <summary>
        /// Obtiene el Usuario asociado al operador indicado
        /// </summary>
        /// <param name="id_operador">Id de Operador</param>
        /// <returns></returns>
        public static Usuario ObtieneUsuarioAsignadoOperador(int id_operador)
        {
            //Inicializando retorno
            Usuario usuario = new Usuario(0);

            //Instanciando Operador
            using (Global.Operador op = new Global.Operador(id_operador))
            {
                //Si hay operador
                if (op.id_operador > 0)
                {
                    //Recuperando Id de Usuario
                    using (Global.Referencia vinculo = new Global.Referencia(op.rfc, Global.ReferenciaTipo.ObtieneIdReferenciaTipo(op.id_compania_emisor, 30, "Vinculo Operador", 0, "Configuración")))
                    {
                        //Instanciando usuario
                        usuario = new Usuario(vinculo.id_registro);
                    }
                }
            }

            //Devolviendo objeto de resultado
            return usuario;
        }

        #endregion
    }
}
