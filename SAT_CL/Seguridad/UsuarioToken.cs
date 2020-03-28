using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Proporciona los medios para la administración de entidades UsuarioToken
    /// </summary>
    public class UsuarioToken : Disposable
    {
        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "seguridad.sp_usuario_token_tutk";

        private int _id_usuario_token;
        /// <summary>
        /// Atributo que almacena el Id del Usuario Token
        /// </summary>
        public int id_usuario_token { get { return this._id_usuario_token; } }

        private int _id_usuario_registra;
        /// <summary>
        /// Atributo que almacena el Id del Usuario Registra
        /// </summary>
        public int id_usuario_registra { get { return this._id_usuario_registra; } }

        private int _id_compania;
        /// <summary>
        /// Atributo que almacena el Id de la Compania
        /// </summary>
        public int id_compania { get { return this._id_compania; } }

        private int _secuencia;
        /// <summary>
        /// Atributo que almacena la secuencia
        /// </summary>
        public int secuencia { get { return this._secuencia; } }

        private int _id_clave_encriptacion;
        /// <summary>
        ///  Atributo que almacena el Id Clave Encriptacion
        /// </summary>
        public int id_clave_encriptacion { get { return this._id_clave_encriptacion; } }
        /// <summary>
        /// Atributo que almacena el Clave Encriptacion (Enumeración)
        /// </summary>
        public Encriptacion.MetodoDigestion clave_encriptacion { get { return (Encriptacion.MetodoDigestion)this._id_clave_encriptacion; } }

        private string _llave_encriptacion;
        /// <summary>
        /// Atributo que almacena la Llave de Encriptacion
        /// </summary>
        public string llave_encriptacion { get { return this._llave_encriptacion; } }

        private string _token;
        /// <summary>
        /// Atributo que almacena el Token
        /// </summary>
        public string token { get { return this._token; } }

        private DateTime _fecha_creacion;
        /// <summary>
        /// Atributo que almacena la Fecha de Creacion 
        /// </summary>
        public DateTime fecha_creacion { get { return this._fecha_creacion; } }

        private DateTime _fecha_termino;
        /// <summary>
        /// Atributo que almacena la Fecha de Creacion 
        /// </summary>
        public DateTime fecha_termino { get { return this._fecha_termino; } }

        private int _id_usuario;
        /// <summary>
        /// Atributo que guarda el identificador del último usuario en usar el registro.
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }

        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena la habilitacion.
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que se encarga de Inicializar los Atributos por Defecto
        /// </summary>
        public UsuarioToken()
        {
            this._id_usuario_token = 0;
            this._id_usuario_registra = 0;
            this._id_compania = 0;
            this._secuencia = 0;
            this._id_clave_encriptacion = 0;
            this._llave_encriptacion = "";
            this._token = "";
            this._fecha_creacion = DateTime.MinValue;
            this._fecha_termino = DateTime.MinValue;
            this._id_usuario = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado id autorizacion 
        /// </summary>
        /// <param name="id_usuario_token">Id Usuario Token</param>
        public UsuarioToken(int id_usuario_token)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_usuario_token);
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un TOKEN de Usuario
        /// </summary>
        /// <param name="token">Token de Usuario</param>
        public UsuarioToken(string token)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(token);
        }

        #endregion

        #region Destructores

        ~UsuarioToken()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Carga una Instancia UsuarioToken ligando un ID
        /// </summary>
        /// <param name="id_usuario_token"> </param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_usuario_token)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_usuario_token, 0, 0, 0, 0, "", "", null, null, 0, false, "", "" };

            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iniciando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_usuario_token = Convert.ToInt32(dr["Id"]);
                        this._id_usuario_registra = Convert.ToInt32(dr["IdUsuarioRegistra"]);
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._secuencia = Convert.ToInt32(dr["Secuencia"]);
                        this._id_clave_encriptacion = Convert.ToInt32(dr["IdClaveEncriptacion"]);
                        this._llave_encriptacion = dr["LlaveEncriptacion"].ToString();
                        this._token = dr["Token"].ToString();
                        DateTime.TryParse(dr["FechaCreacion"].ToString(), out this._fecha_creacion);
                        DateTime.TryParse(dr["FechaTermino"].ToString(), out this._fecha_termino);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Variables Positiva
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Carga una Instancia UsuarioToken proporcionando un TOKEN de Usuario
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(string token)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, 0, 0, 0, "", token, null, null, 0, false, "", "" };

            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iniciando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_usuario_token = Convert.ToInt32(dr["Id"]);
                        this._id_usuario_registra = Convert.ToInt32(dr["IdUsuarioRegistra"]);
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._secuencia = Convert.ToInt32(dr["Secuencia"]);
                        this._id_clave_encriptacion = Convert.ToInt32(dr["IdClaveEncriptacion"]);
                        this._llave_encriptacion = dr["LlaveEncriptacion"].ToString();
                        this._token = dr["Token"].ToString();
                        DateTime.TryParse(dr["FechaCreacion"].ToString(), out this._fecha_creacion);
                        DateTime.TryParse(dr["FechaTermino"].ToString(), out this._fecha_termino);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Variables Positiva
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_usuario_registra"> Id Usuario Registra </param>
        /// <param name="id_compania"> Id Compania </param>
        /// <param name="secuencia"> Secuencia </param>
        /// <param name="id_clave_encriptacion"> Id Clave Encriptacion </param>
        /// <param name="llave_encriptacion"> Llave Encriptacion </param>
        /// <param name="token"> Token </param>
        /// <param name="fecha_creacion"> Fecha Creacion </param>
        /// <param name="fecha_termino"> Fecha Termino</param>
        /// <param name="id_usuario"> Id Usuario Actualiza </param>
        /// <param name="habilitar"> Estatus Habilitar </param>
        /// <returns></returns>
        private RetornoOperacion editaUsuarioToken(int id_usuario_registra, int id_compania, int secuencia, int id_clave_encriptacion, string llave_encriptacion, string token, DateTime fecha_creacion, DateTime fecha_termino, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            //RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2,
                               this._id_usuario_token,
                               id_usuario_registra,
                               id_compania, secuencia,
                               id_clave_encriptacion,
                               llave_encriptacion,
                               token,
                               fecha_creacion,
                               fecha_termino,
                               id_usuario,
                               habilitar,
                               "",
                               "" };

            //Ejecutando SP
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo Resultado Obtenido
            //return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Registros en BD
        /// </summary>
        /// <param name="id_usuario_registra"> Id Usuario Registra </param>
        /// <param name="id_compania"> Id Compania </param>
        /// <param name="secuencia"> Secuencia </param>
        /// <param name="id_clave_encriptacion"> Id Clave Encriptacion </param>
        /// <param name="llave_encriptacion"> Llave Encriptacion </param>
        /// <param name="token"> Token </param>
        /// <param name="fecha_creacion"> Fecha Creacion </param>
        /// <param name="fecha_termino"> Fecha Termino</param>
        /// <param name="id_usuario"> Id Usuario Actualiza </param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUsuarioToken(int id_usuario_registra, int id_compania, int secuencia, int id_clave_encriptacion, string llave_encriptacion, string token, DateTime fecha_creacion, DateTime fecha_termino, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_usuario_registra, id_compania, secuencia, id_clave_encriptacion, llave_encriptacion, token, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_creacion), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_termino), id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Edita Usuario Token
        /// </summary>
        /// <param name="id_usuario_registra"> Id Usuario Registra </param>
        /// <param name="id_compania"> Id Compania </param>
        /// <param name="secuencia"> Secuencia </param>
        /// <param name="id_clave_encriptacion"> Id Clave Encriptacion </param>
        /// <param name="llave_encriptacion"> Llave Encriptacion </param>
        /// <param name="token"> Token </param>
        /// <param name="fecha_creacion"> Fecha Creacion </param>
        /// <param name="fecha_termino"> Fecha Termino</param>
        /// <param name="id_usuario"> Id Usuario Actualiza </param>
        /// <returns></returns>
        public RetornoOperacion EditarUsuarioToken(int id_usuario_registra, int id_compania, int secuencia, int id_clave_encriptacion, string llave_encriptacion, string token, DateTime fecha_creacion, DateTime fecha_termino, int id_usuario)
        {
            return this.editaUsuarioToken(id_usuario_registra, id_compania, secuencia, id_clave_encriptacion, llave_encriptacion, token, fecha_creacion, fecha_termino, id_usuario, this._habilitar);

        }
        /// <summary>
        /// Deshabilita Usuario Token
        /// </summary>
        /// <param name="id_usuario"> Id Usuario </param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUsuarioToken(int id_usuario)
        {
            return this.editaUsuarioToken(this._id_usuario_registra, this._id_compania, this._secuencia, this._id_clave_encriptacion, this.llave_encriptacion, this._token, this._fecha_creacion, this._fecha_termino, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Terminar el Token
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion TerminaUsuarioToken(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            if (this._fecha_termino == DateTime.MinValue)
                //Armando Arreglo de Parametros
                result = this.editaUsuarioToken(this._id_usuario_registra, this._id_compania, this._secuencia, this._id_clave_encriptacion, this._llave_encriptacion,
                                this._token, this._fecha_creacion, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, this._habilitar);
            else
                result = new RetornoOperacion("El Token ya esta terminado");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Actualiza atributos Usuario Token
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUsuarioCompania()
        {
            return this.cargaAtributosInstancia(this._id_usuario_token);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static UsuarioToken ObtieneTokenActivo(int id_usuario, int id_compania)
        {
            UsuarioToken token_activo = new UsuarioToken();
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_usuario, id_compania, 0, 0, "", "", null, null, 0, false, "", "" };
            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iniciando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        DateTime fec_creacion, fec_termino;
                        DateTime.TryParse(dr["FechaCreacion"].ToString(), out fec_creacion);
                        DateTime.TryParse(dr["FechaTermino"].ToString(), out fec_termino);
                        token_activo = new UsuarioToken
                        {
                            //Asignando Atributos
                            _id_usuario_token = Convert.ToInt32(dr["Id"]),
                            _id_usuario_registra = Convert.ToInt32(dr["IdUsuarioRegistra"]),
                            _id_compania = Convert.ToInt32(dr["IdCompania"]),
                            _secuencia = Convert.ToInt32(dr["Secuencia"]),
                            _id_clave_encriptacion = Convert.ToInt32(dr["IdClaveEncriptacion"]),
                            _llave_encriptacion = dr["LlaveEncriptacion"].ToString(),
                            _token = dr["Token"].ToString(),
                            _fecha_creacion = fec_creacion,
                            _fecha_termino = fec_termino,
                            _habilitar = Convert.ToBoolean(dr["Habilitar"])
                        };
                        break;
                    }
                }
            }
            return token_activo;
        }

        #endregion

        #region Métodos de Gestión TOKEN

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario_registro"></param>
        /// <param name="clave_encriptacion"></param>
        /// <param name="llave_encriptacion"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_usuario"></param>
        /// <param name="token_usuario"></param>
        /// <returns></returns>
        private static RetornoOperacion generaTokenUsuario(int id_usuario_registro, Encriptacion.MetodoDigestion clave_encriptacion, string llave_encriptacion, int id_compania, int id_usuario, out string token_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
            token_usuario = "";

            if (llave_encriptacion.Length <= 5)
            {
                string llave64 = Encriptacion.CalculaHashCadenaEnBase64(llave_encriptacion, clave_encriptacion);
                //Validando Usuario
                using (Usuario user = new Usuario(id_usuario_registro))
                {
                    if (user.habilitar)
                    {
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            using (UsuarioToken activo = UsuarioToken.ObtieneTokenActivo(id_usuario_registro, id_compania))
                            {
                                if (!activo.habilitar)
                                    //Generando Resultado Positivo
                                    retorno = new RetornoOperacion(id_usuario, "Listo para Crear el Token", true);
                                else
                                {
                                    //Terminando TOKEN
                                    retorno = activo.TerminaUsuarioToken(id_usuario);
                                    if (retorno.OperacionExitosa)
                                        //Generando Resultado Positivo
                                        retorno = new RetornoOperacion(id_usuario, "Listo para Crear el Token", true);
                                }
                            }

                            if (retorno.OperacionExitosa)
                            {
                                //Creando Clave para encriptación
                                string pre_token = string.Format("{0:0000}{1:yyyyMMdd_HHmm}", user.id_usuario, fecha_actual);
                                //Encriptando en Clave AES
                                token_usuario = Encriptacion.EncriptaCadenaAESEnBase64(pre_token, llave64);
                                if (!token_usuario.Equals(""))
                                    retorno = new RetornoOperacion(user.id_usuario, "Token generado exitosamente.", true);
                                else
                                    retorno = new RetornoOperacion("El token no se genero correctamente");

                                if (retorno.OperacionExitosa)
                                {
                                    retorno = UsuarioToken.InsertaUsuarioToken(id_usuario_registro, id_compania, 0, (int)clave_encriptacion, llave64, token_usuario, fecha_actual, DateTime.MinValue, id_usuario);
                                    if (retorno.OperacionExitosa)
                                    {
                                        retorno = new RetornoOperacion(retorno.IdRegistro, "Token Generado Exitosamente!", true);
                                        //Completando Transacción
                                        scope.Complete();
                                    }
                                }
                            }
                        }
                    }
                    else
                        retorno = new RetornoOperacion("El usuario que ingreso así como Clarita, es invalido");
                }
            }
            else
                retorno = new RetornoOperacion("La llave de encriptación debe de ser de 10 caracteres máximo");

            return retorno;
        }
        /// <summary>
        /// Método encargado de Generar un Nuevo Token de Autenticación de Usuario por Compania
        /// </summary>
        /// <param name="id_usuario_registro"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_usuario"></param>
        /// <param name="token_nvo"></param>
        /// <returns></returns>
        public static RetornoOperacion GeneraNuevoTokenUsuario(int id_usuario_registro, int id_compania, int id_usuario, out string token_nvo)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            token_nvo = "";

            //Obteniendo Clave de Encriptación (Aleatorio)
            string llave_encriptacion = Cadena.CadenaAleatoria(1, 2, 1);
            if (!llave_encriptacion.Equals(""))
            {
                //Obteniendo Método de Encriptación (Aleatorio)
                Random r = new Random();
                Encriptacion.MetodoDigestion clave_enc = (Encriptacion.MetodoDigestion)r.Next(1, 3);
                if (clave_enc != Encriptacion.MetodoDigestion.SinAsignar)

                    //Generando TOKEN
                    retorno = generaTokenUsuario(id_usuario_registro, clave_enc, llave_encriptacion, id_compania, id_usuario, out token_nvo);
                else
                    retorno = new RetornoOperacion("La clave de encriptacion correctamente");
            }
            else
                retorno = new RetornoOperacion("El token no se genero correctamente");

            return retorno;
        }
        /// <summary>
        /// Método encargado de Generar un Nuevo Token de Autenticación de Usuario por Compania
        /// </summary>
        /// <param name="id_usuario_registro"></param>
        /// <param name="clave_encriptacion"></param>
        /// <param name="llave_encriptacion"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_usuario"></param>
        /// <param name="token_nvo"></param>
        /// <returns></returns>
        public static RetornoOperacion GeneraNuevoTokenUsuario(int id_usuario_registro, Encriptacion.MetodoDigestion clave_encriptacion, string llave_encriptacion, int id_compania, int id_usuario, out string token_nvo)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            token_nvo = "";
            //Generando TOKEN
            retorno = generaTokenUsuario(id_usuario_registro, clave_encriptacion, llave_encriptacion, id_compania, id_usuario, out token_nvo);
            return retorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario_registro"></param>
        /// <param name="clave_encriptacion"></param>
        /// <param name="llave_encriptacion"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_usuario"></param>
        /// <param name="token_usuario"></param>
        /// <returns></returns>
        private static RetornoOperacion generaTokenUUID(int id_usuario_registro, string llave_encriptacion, int id_compania, int id_usuario, out string token_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
            token_usuario = "";

            if (llave_encriptacion.Length <= 5)
            {
                //Validando Usuario
                using (Usuario user = new Usuario(id_usuario_registro))
                {
                    if (user.habilitar)
                    {
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            using (UsuarioToken activo = UsuarioToken.ObtieneTokenActivo(id_usuario_registro, id_compania))
                            {
                                if (!activo.habilitar)
                                    //Generando Resultado Positivo
                                    retorno = new RetornoOperacion(id_usuario, "Listo para Crear el Token", true);
                                else
                                {
                                    //Terminando TOKEN
                                    retorno = activo.TerminaUsuarioToken(id_usuario);
                                    if (retorno.OperacionExitosa)
                                        //Generando Resultado Positivo
                                        retorno = new RetornoOperacion(id_usuario, "Listo para Crear el Token", true);
                                }
                            }

                            if (retorno.OperacionExitosa)
                            {
                                //CrearToken
                                token_usuario = Guid.NewGuid().ToString(); 
                                //Validando se genero el token
                                //if (retorno.OperacionExitosa)
                                //{
                                    retorno = SAT_CL.Seguridad.UsuarioToken.InsertaUsuarioToken(id_usuario_registro, id_compania, 0, 1, llave_encriptacion, token_usuario, fecha_actual, DateTime.MinValue, id_usuario);
                                    while (!retorno.OperacionExitosa)
                                    {
                                        //CrearToken
                                        token_usuario = Guid.NewGuid().ToString();
                                        retorno = UsuarioToken.InsertaUsuarioToken(id_usuario_registro, id_compania, 0, 1, llave_encriptacion, token_usuario, fecha_actual, DateTime.MinValue, id_usuario);
                                    if (retorno.OperacionExitosa)
                                        break;
                                    }

                                    if (retorno.OperacionExitosa)
                                    {
                                        retorno = new RetornoOperacion(retorno.IdRegistro, "Token Generado Exitosamente!", true);
                                        //Completando Transacción
                                        scope.Complete();
                                    }
                                //}
                            }
                        }
                    }
                    else
                        retorno = new RetornoOperacion("El usuario que ingreso, es invalido");
                }
            }
            else
                retorno = new RetornoOperacion("La llave de encriptación debe de ser de 10 caracteres máximo");

            return retorno;
        }
        /// <summary>
        /// Método encargado de Generar un Nuevo Token de Autenticación de Usuario por Compania
        /// </summary>
        /// <param name="id_usuario_registro"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_usuario"></param>
        /// <param name="token_nvo"></param>
        /// <returns></returns>
        public static RetornoOperacion GeneraNuevoTokenUUID(int id_usuario_registro, int id_compania, int id_usuario, out string token_nvo)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            token_nvo = "";

            //Obteniendo Clave de Encriptación (Aleatorio)
            string llave_encriptacion = Cadena.CadenaAleatoria(1, 5, 4);
            if (!llave_encriptacion.Equals(""))
            {
                //Generando TOKEN
                retorno = generaTokenUUID(id_usuario_registro, llave_encriptacion, id_compania, id_usuario, out token_nvo);
            }
            else
                retorno = new RetornoOperacion("El token no se genero correctamente");

            return retorno;
        }

        public string cifrarTextoAES(string textoCifrar, string palabraPaso,
          string valorRGBSalt, string algoritmoEncriptacionHASH,
          int iteraciones, string vectorInicial, int tamanoClave)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(vectorInicial);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(valorRGBSalt);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoCifrar);

                PasswordDeriveBytes password =
                    new PasswordDeriveBytes(palabraPaso, saltValueBytes,
                        algoritmoEncriptacionHASH, iteraciones);

                byte[] keyBytes = password.GetBytes(tamanoClave / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor =
                    symmetricKey.CreateEncryptor(keyBytes, InitialVectorBytes);

                MemoryStream memoryStream = new MemoryStream();

                CryptoStream cryptoStream =
                    new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] cipherTextBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();

                string textoCifradoFinal = Convert.ToBase64String(cipherTextBytes);

                return textoCifradoFinal;
            }
            catch
            {
                return null;
            }
        }

        public string descifrarTextoAES(string textoCifrado, string palabraPaso,
    string valorRGBSalt, string algoritmoEncriptacionHASH,
    int iteraciones, string vectorInicial, int tamanoClave)
        {
            try
            {
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(vectorInicial);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(valorRGBSalt);

                byte[] cipherTextBytes = Convert.FromBase64String(textoCifrado);

                PasswordDeriveBytes password =
                    new PasswordDeriveBytes(palabraPaso, saltValueBytes,
                        algoritmoEncriptacionHASH, iteraciones);

                byte[] keyBytes = password.GetBytes(tamanoClave / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, InitialVectorBytes);

                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();

                string textoDescifradoFinal = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                return textoDescifradoFinal;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
