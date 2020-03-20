using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Operadores
    /// </summary>
    public class Operador : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Enumera el Estatus 
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            ///  Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Disponible
            /// </summary>
            Disponible,
            /// <summary>
            /// Ocupado
            /// </summary>
            Ocupado,
            /// <summary>
            /// Transito
            /// </summary>
            Transito,
            /// <summary>
            /// Baja
            /// </summary>
            Baja
        }
        /// <summary>
        /// Expresa el Tipo de Entidad
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            /// Indica que la Entidad es un Operador
            /// </summary>
            Operador = 1,
            /// <summary>
            /// Indica que la Entidad es un Empleado
            /// </summary>
            Empleado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_operador_to";

        private int _id_operador;
        /// <summary>
        /// Atributo encargado de Almacenar el Operador
        /// </summary>
        public int id_operador { get { return this._id_operador; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Obtiene el estatus actual del operador
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private byte _id_tipo;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo
        /// </summary>
        public byte id_tipo { get { return this._id_tipo; } }
        /// <summary>
        /// Obtiene el Tipo del operador
        /// </summary>
        public Tipo tipo { get { return (Tipo)this._id_tipo; } }
        private string _nombre;
        /// <summary>
        /// Atributo encargado de Almacenar el Nombre
        /// </summary>
        public string nombre { get { return this._nombre; } }
        private DateTime _fecha_nacimiento;
        /// <summary>
        /// Obtiene la fecha de nacimiento del operador
        /// </summary>
        public DateTime fecha_nacimiento { get { return this._fecha_nacimiento; } }
        private string _rfc;
        /// <summary>
        /// Obtiene el RFC del operador
        /// </summary>
        public string rfc { get { return this._rfc; } }
        private string _curp;
        /// <summary>
        /// Obtiene la CURP del operador
        /// </summary>
        public string curp { get { return this._curp; } }
        private string _nss;
        /// <summary>
        /// Obtiene el Número de Segudad Social del Operador
        /// </summary>
        public string nss { get { return this._nss; } }
        private string _r_control;
        /// <summary>
        /// Obtiene la clave de Recurso Confiable del Operador
        /// </summary>
        public string r_control { get { return this._r_control; } }
        private string _telefono;
        /// <summary>
        /// Atributo encargado de Almacenar el Telefono
        /// </summary>
        public string telefono { get { return this._telefono; } }
        private string _telefono_casa;
        /// <summary>
        /// Obtiene el número de Teléfono de casa del operador
        /// </summary>
        public string telefono_casa { get { return this._telefono_casa; } }
        private int _id_direccion;
        /// <summary>
        /// Atributo encargado de Almacenar la Dirección
        /// </summary>
        public int id_direccion { get { return this._id_direccion; } }
        private DateTime _fecha_ingreso;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Ingreso
        /// </summary>
        public DateTime fecha_ingreso { get { return this._fecha_ingreso; } }
        private DateTime _fecha_baja;
        /// <summary>
        /// Obtiene la fecha de la baja del operador
        /// </summary>
        public DateTime fecha_baja { get { return this._fecha_baja; } }
        private byte _id_tipo_licencia;
        /// <summary>
        /// Obtiene el Id de Tipo de Licencia del Operador
        /// </summary>
        public byte id_tipo_licencia { get { return this._id_tipo_licencia; } }
        private string _numero_licencia;
        /// <summary>
        /// Obtiene el número de licencia del operador
        /// </summary>
        public string numero_licencia { get { return this._numero_licencia; } }
        private int _id_parada;
        /// <summary>
        /// Obtiene el Id de la parada actual del operador
        /// </summary>
        public int id_parada { get { return this._id_parada; } }
        private int _id_movimiento;
        /// <summary>
        /// Obtiene el Id de movimiento actual activo del operador
        /// </summary>
        public int id_movimiento { get { return this._id_movimiento; } }
        private DateTime _fecha_actualizacion;
        /// <summary>
        /// Obtiene la fecha de la última actualización de el Id de Parada o Movimiento
        /// </summary>
        public DateTime fecha_actualizacion { get { return this._fecha_actualizacion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar El Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private string _cod_authenticacion;
        /// <summary>
        /// Obtiene el Código de Authenticación del operador
        /// </summary>
        public string cod_authenticacion { get { return this._cod_authenticacion; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Operador()
        {   //Asignando Valores
            this._id_operador = 0;
            this._id_compania_emisor = 0;
            this._id_estatus = 0;
            this._id_tipo = 0;
            this._nombre = "";
            this._fecha_nacimiento = DateTime.MinValue;
            this._rfc = "";
            this._curp = "";
            this._nss = "";
            this._r_control = "";
            this._telefono = "";
            this._telefono_casa = "";
            this._id_direccion = 0;
            this._fecha_ingreso = DateTime.MinValue;
            this._fecha_baja = DateTime.MinValue;
            this._id_tipo_licencia = 0;
            this._numero_licencia = "";
            this._id_parada = 0;
            this._id_movimiento = 0;
            this._fecha_actualizacion = DateTime.MinValue;
            this._habilitar = false;
            this._cod_authenticacion = "";
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_operador">Id de Operador</param>
        public Operador(int id_operador)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_operador);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Operador()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_operador">Id de Operador</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_operador)
        {   
            //Declarando Objeto de Retorno
            bool result = false;
            
            //Armando Arreglo de Parametros
            object[] param = { 3, id_operador, 0, 0, 0, "", null, "", "", "", "", "", "", 0, null, null, 0, "", 0, 0, null, 0, false, "", "" };
            
            //Instanciando 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_operador = id_operador;
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_tipo = Convert.ToByte(dr["IdTipo"]);
                        this._nombre = dr["Nombre"].ToString();
                        this._fecha_nacimiento = Convert.ToDateTime(dr["FechaNacimiento"]);
                        this._rfc = dr["Rfc"].ToString();
                        this._curp = dr["Curp"].ToString();
                        this._nss = dr["Nss"].ToString();
                        this._r_control = dr["RControl"].ToString();
                        this._telefono = dr["Telefono"].ToString();
                        this._telefono_casa = dr["TelefonoCasa"].ToString();
                        this._id_direccion = Convert.ToInt32(dr["IdDireccion"]);
                        this._fecha_ingreso = Convert.ToDateTime(dr["FechaIngreso"]);
                        DateTime.TryParse(dr["FechaBaja"].ToString(), out this._fecha_baja);
                        this._id_tipo_licencia = Convert.ToByte(dr["IdTipoLicencia"]);
                        this._numero_licencia = dr["NumeroLicencia"].ToString();
                        this._id_parada = Convert.ToInt32(dr["IdParada"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        DateTime.TryParse(dr["FechaActualizacion"].ToString(), out this._fecha_actualizacion);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._cod_authenticacion = dr["CodigoAuth"].ToString();
                    }
                    //Asignando Retorno Positivo
                    result = true;
                }
            }
            
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Actualiza Operador
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="fecha_nacimiento">Fecha de Nacimiento</param>
        /// <param name="rfc">RFC</param>
        /// <param name="curp">CURP</param>
        /// <param name="nss">NSS</param>
        /// <param name="r_control">Número de Recurso Confiable</param>
        /// <param name="telefono">Telefono</param>
        /// <param name="telefono_casa">Número Telefónico de casa</param>
        /// <param name="id_direccion">Id Dirección</param>
        /// <param name="fecha_ingreso">Fecha Ingreso</param>
        /// <param name="fecha_baja">Fecha de Baja del Operador</param>
        /// <param name="id_tipo_licencia">Tipo de Licencia</param>
        /// <param name="numero_licencia">Número de Licencia</param>
        /// <param name="id_parada">Id de Parada actual</param>
        /// <param name="id_movimiento">Id de Movimiento actual</param>
        /// <param name="fecha_actualizacion">Fecha de Ultima actualización de Parada o Id de Movimiento</param>
        /// <param name="id_usuario">Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaRegistro(int id_compania_emisor, Estatus estatus, Tipo tipo, string nombre, DateTime fecha_nacimiento, string rfc, string curp, string nss, string r_control,
                                            string telefono, string telefono_casa, int id_direccion, DateTime fecha_ingreso, DateTime fecha_baja, byte id_tipo_licencia, string numero_licencia,
                                            int id_parada, int id_movimiento, DateTime fecha_actualizacion, int id_usuario, bool habilitar)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Armando Objeto de parametros
            object[] param = { 2, this._id_operador, id_compania_emisor, (byte)estatus, (byte)tipo, nombre, fecha_nacimiento, rfc, curp, nss, r_control, telefono, telefono_casa, id_direccion, fecha_ingreso, 
                                Fecha.ConvierteDateTimeObjeto(fecha_baja), id_tipo_licencia, numero_licencia, id_parada, id_movimiento, Fecha.ConvierteDateTimeObjeto(fecha_actualizacion), id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            
            //Devolviendo resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Determina si el operador está listo para su baja, eveluando existencia de anticipos, vales y asignaciones pendientes
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaAsignacionesOperadorBaja()
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_operador);

            //Obteniendo el conjunto de anticipos pendientes
            using (DataTable mitAnticipos = EgresoServicio.DetalleLiquidacion.ObtieneDetallesSinLiquidarOperador(this._id_operador))
                //Si hay registros
                if (mitAnticipos != null)
                    resultado = new RetornoOperacion("Existen elementos pendientes por liquidar.");

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Obteniendo asignaciones a movimientos pendientes
                using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.ObtieneAsignacionesPendientesRecurso(MovimientoAsignacionRecurso.Tipo.Operador, this._id_operador))
                {
                    //Si hay registros
                    if (mitAsignaciones != null)
                        resultado = new RetornoOperacion("Existen asignaciones inconclusas para este operador.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Añade un nuevo registro Operador a la tabla correspondiente en BD, con los datos proporcionados
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía a la que se suscribe el operador</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="nombre">Nombre del Operador</param>
        /// <param name="fecha_nacimiento">Fecha de Nacimiento</param>
        /// <param name="rfc">RFC</param>
        /// <param name="curp">CURP</param>
        /// <param name="nss">NSS</param>
        /// <param name="r_control">R Control</param>
        /// <param name="telefono">Telefóno móvil o personal</param>
        /// <param name="telefono_casa">Teléfono de casa o emergencia</param>
        /// <param name="id_direccion">Id de Dirección del operador</param>
        /// <param name="fecha_ingreso">Fecha de Ingreso</param>
        /// <param name="id_tipo_licencia">Tipo de Licencia de conducir</param>
        /// <param name="numero_licencia">Número de Licencia</param>
        /// <param name="id_usuario">Id de Usuario que lo registra</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaOperador(int id_compania_emisor, Tipo tipo, string nombre, DateTime fecha_nacimiento, string rfc, string curp, string nss, string r_control,
                                                    string telefono, string telefono_casa, int id_direccion, DateTime fecha_ingreso, byte id_tipo_licencia, string numero_licencia, int id_usuario)
        {
            //Creando arreglo de parametros para ejecución de SP
            object[] param =  { 1, 0, id_compania_emisor, (byte)Estatus.Registrado, (byte)tipo, nombre, fecha_nacimiento, rfc, curp, nss, r_control, telefono, telefono_casa, id_direccion, fecha_ingreso, null, 
                                id_tipo_licencia, numero_licencia, 0, 0, null, id_usuario, true, "", "" };

            //Realizando inserción de nuevo operador
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
        }
        /// <summary>
        /// Edita el registro Operador de la tabla correspondiente en BD, con los datos proporcionados
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía a la que se suscribe el operador</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="nombre">Nombre del Operador</param>
        /// <param name="fecha_nacimiento">Fecha de Nacimiento</param>
        /// <param name="rfc">RFC</param>
        /// <param name="curp">CURP</param>
        /// <param name="nss">NSS</param>
        /// <param name="r_control">R Control</param>
        /// <param name="telefono">Telefóno móvil o personal</param>
        /// <param name="telefono_casa">Teléfono de casa o emergencia</param>
        /// <param name="id_direccion">Id de Dirección del operador</param>
        /// <param name="fecha_ingreso">Fecha de Ingreso</param>
        /// <param name="id_tipo_licencia">Tipo de Licencia de conducir</param>
        /// <param name="numero_licencia">Número de Licencia</param>
        /// <param name="id_usuario">Id de Usuario que lo registra</param>
        /// <returns></returns>
        public RetornoOperacion EditaOperador(int id_compania_emisor, Tipo tipo, string nombre, DateTime fecha_nacimiento, string rfc, string curp, string nss, string r_control,
                                                    string telefono, string telefono_casa, int id_direccion, DateTime fecha_ingreso, byte id_tipo_licencia, string numero_licencia, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_operador);

            //Validando que la fecha de ingreso y fecha de baja
            //Si hay fecha de baja, la fecha de ingreso debe ser menor
            if (this._fecha_baja.CompareTo(DateTime.MinValue) > 0)
                if (this._fecha_baja.CompareTo(this._fecha_ingreso) < 0)
                    resultado = new RetornoOperacion("La fecha de ingreso no puede ser mayor a la fecha de baja.");

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Realizando inserción de nuevo operador
                resultado = editaRegistro(id_compania_emisor, this.estatus, tipo, nombre, fecha_nacimiento, rfc, curp, nss, r_control, telefono, telefono_casa, id_direccion, fecha_ingreso, this._fecha_baja,
                                    id_tipo_licencia, numero_licencia, this._id_parada, this._id_movimiento, this._fecha_actualizacion, id_usuario, this._habilitar);

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la deshabilitación del operador
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que deshabilita</param>
        /// <returns></returns>
        [Obsolete("El método no debe ser utilizado, en su lugar utilizar 'ActualizaEstatusABaja' para exclusión de operadores.", true)]
        public RetornoOperacion DeshabilitaOperador(int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando estatus actual de operador
            if (this.estatus == Estatus.Registrado || this.estatus == Estatus.Disponible)
            {
                //TODO: Validar pendientes (anticipos, liquidación, fecha de ultimo servicio, ultimo pago)
                //Realizando deshabilitación de nuevo operador
                resultado = editaRegistro(this._id_compania_emisor, this.estatus, this.tipo, this._nombre, this._fecha_nacimiento, this._rfc, this._curp, this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso, this._fecha_baja,
                                    this._id_tipo_licencia, this._numero_licencia, this._id_parada, this._id_movimiento, this._fecha_actualizacion, id_usuario, false);
            }
            else
                resultado = new RetornoOperacion("El estatus actual del operador no permite su deshabilitación.");

            return resultado;
        }
        /// <summary>
        /// Carga los Operadores para Asignación de Recurso
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_estatus">Estatus</param>
        /// <returns></returns>
        public static DataTable CargaOperadoresParaAsignacionRecurso(int id_compania_emisor, int id_operador, byte id_estatus)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Declarando arreglo de parámetros para consulta en BD
            //Armando Objeto de Parametros
            object[] param = { 4, id_operador, id_compania_emisor, id_estatus, 0, "", null, "", "", "", "", "", "", 0, null, null, 0, "", 0, 0, null, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Operadores para asignación en despacho
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="nombre">Nombre del Operador</param>
        /// <param name="id_ubicacion">Id ubicación</param>
        /// <param name="estancia">Valida existencia de estancia del recurso</param>
        /// <returns></returns>
        public static DataTable CargaOperadoresParaAsignacionEnDespacho(int id_compania_emisor, string nombre, int id_ubicacion, byte estancia)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 5, 0, id_compania_emisor, 0, 0, nombre, null, "", "", "", "", "", "", 0, null, null, 0, "", 0, 0, null, 0, false, id_ubicacion, estancia };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Actualizamos el Estatus del Operador a Parada Ocupado.
        /// </summary>
        /// <param name="id_parada_a_mover">Id parada a mover</param>
        /// <param name="id_usuario">Id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaOperadorAParadaOcupado(int id_parada_a_mover, int id_usuario)
        {
            //Estable Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //ValidamosEstatus
            if ((Estatus)this._id_estatus == Estatus.Disponible || (Estatus)this._id_estatus == Estatus.Registrado)
            {
                //Validamos Estatus
                if ((Estatus)this._id_estatus != Estatus.Ocupado)
                {
                    resultado = editaRegistro(this._id_compania_emisor, Estatus.Ocupado, (Tipo)this.id_tipo, this._nombre, this._fecha_nacimiento, this._rfc, this._curp,
                                              this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso,
                                              this._fecha_baja, this._id_tipo_licencia, this._numero_licencia, this._id_parada, this._id_movimiento,
                                              this._fecha_actualizacion, id_usuario, this._habilitar);
                }
                else
                {
                    //Obtenemos el Id de Servicio que se encuentra actualmente al Operador
                    using (Servicio objServicio = new Servicio(MovimientoAsignacionRecurso.ObtieneServicioOperadorIniciado(this._id_operador)))
                    {
                        using (Parada objParadaAMover = new Parada(id_parada_a_mover))
                        {
                            //Si el servicio es igual al actual
                            if (objServicio.id_servicio == objParadaAMover.id_servicio)
                            {
                                //Actualizamos Estatus
                                resultado = editaRegistro(this._id_compania_emisor, Estatus.Ocupado, (Tipo)this.id_tipo, this._nombre, this._fecha_nacimiento, this._rfc, this._curp,
                                              this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso,
                                              this._fecha_baja, this._id_tipo_licencia, this._numero_licencia, this._id_parada, this._id_movimiento,
                                              this._fecha_actualizacion, id_usuario, this._habilitar);
                            }
                            //Si es distinto
                            else
                            {
                                //Establecemos Mensaje Error 
                                resultado = new RetornoOperacion("El Operador " + this._nombre + " se encuentra asignado al servicio " + objServicio.no_servicio + ".");
                            }
                        }
                    }
                }
            }
            else
            {
                //Establecemos Resultado
                resultado = new RetornoOperacion("El operador " + this.nombre + " no se encuentra disponible.");
            }
            //Devolvemos Valor 
            return resultado;
        }

        /// <summary>
        /// Actualizamos el Estatus del Operador a Disponible.
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusADisponible(int id_usuario)
        {
            //Estable Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos Resultado
            if ((Estatus)this._id_estatus != Estatus.Transito)
            {
                resultado = editaRegistro(this._id_compania_emisor, Estatus.Disponible, (Tipo)this.id_tipo, this._nombre, this._fecha_nacimiento, this._rfc, this._curp,
                                              this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso,
                                              DateTime.MinValue, this._id_tipo_licencia, this._numero_licencia, this._id_parada, this._id_movimiento,
                                              this._fecha_actualizacion, id_usuario, this._habilitar);
            }
            else
            {
                //Establecemos Resultado
                resultado = new RetornoOperacion("El estatus del operador " + this.nombre + " no permite su edición");
            }
            //Devolvemos Valor 
            return resultado;
        }
        /// <summary>
        /// Realiza la baja del operador
        /// </summary>
        /// <param name="fecha_baja">Fecha de Baja del operador</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusABaja(DateTime fecha_baja, int id_usuario)
        {
            //Estable Resultado 
            RetornoOperacion resultado = new RetornoOperacion(this.id_operador);

            //Validamos estatus actual de operador
            if (this.estatus == Estatus.Disponible || this.estatus == Estatus.Registrado)
            {
                //inicializando transacción
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validando fecha de baja contra fecha de adquisición
                    if (this._fecha_ingreso.CompareTo(fecha_baja) <= 0)
                    {
                        //Obteniendo ultima asignación del operador
                        using (AsignacionOperadorUnidad a = AsignacionOperadorUnidad.ObtieneAsignacionActiva(AsignacionOperadorUnidad.TipoBusquedaAsignacion.Operador, this._id_operador))
                        {
                            //SI hay asignación
                            if (a.habilitar)
                                //Terminando última asignación del operador
                                resultado = a.TerminaAsignacionOperadorUnidad(fecha_baja, id_usuario);
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                            //Actualizando estatus a baja
                            resultado = editaRegistro(this._id_compania_emisor, Estatus.Baja, (Tipo)this.id_tipo, this._nombre, this._fecha_nacimiento, this._rfc, this._curp,
                                                          this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso,
                                                          fecha_baja, this._id_tipo_licencia, this._numero_licencia, this._id_parada, this._id_movimiento,
                                                          this._fecha_actualizacion, id_usuario, this._habilitar);
                    }
                    //Si la baja es previa al ingreso
                    else
                        resultado = new RetornoOperacion(string.Format("La fecha de baja '{0:dd/MM/yyyy}' debe ser posterior a la fecha de ingreso '{1:dd/MM/yyyy}' del operador.", fecha_baja, this._fecha_ingreso));

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Finalizando transacción
                        scope.Complete();
                }
            }
            else
                //Instanciando Excepción
                resultado = new RetornoOperacion("El estatus actual del operador no permite la baja del mismo.");

            /*
            //Validamos estatus actual de operador
            if (this.estatus == Estatus.Disponible || this.estatus == Estatus.Registrado)
            {
                //inicializando transacción
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validar pendientes (anticipos, liquidación, fecha de ultimo servicio, ultimo pago)
                    resultado = validaAsignacionesOperadorBaja();

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Cargando liquidaciones depositadas
                        using (DataTable mitLiquidaciones = Liquidacion.Liquidacion.ObtieneLiquidacionesEntidad(this._id_compania_emisor, Liquidacion.Liquidacion.TipoAsignacion.Operador, this._id_operador))
                        {
                            //Si hay registros
                            if (Validacion.ValidaOrigenDatos(mitLiquidaciones))
                            {
                                //Validando fecha de baja contra fecha de última liquidación
                                DateTime ultimaLiquidacion = (from DataRow r in mitLiquidaciones.Rows
                                                              where r.Field<DateTime>("FechaLiquidacion").CompareTo(fecha_baja) > 0
                                                              orderby r.Field<DateTime>("FechaLiquidacion") descending
                                                              select r.Field<DateTime>("FechaLiquidacion")).FirstOrDefault();
                                //Si hay liquidaciones 
                                if (ultimaLiquidacion.CompareTo(DateTime.MinValue) > 0)
                                    resultado = new RetornoOperacion(string.Format("La fecha de baja no puede ser menor a la fecha de la última liquidación '{0:dd/MM/yyyy HH:mm}'.", ultimaLiquidacion));
                            }
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //Validando fecha de baja contra fecha de adquisición
                            if (this._fecha_ingreso.CompareTo(fecha_baja) <= 0)
                            {
                                //Obteniendo ultima asignación del operador
                                using (AsignacionOperadorUnidad a = AsignacionOperadorUnidad.ObtieneAsignacionActiva(AsignacionOperadorUnidad.TipoBusquedaAsignacion.Operador, this._id_operador))
                                {
                                    //SI hay asignación
                                    if (a.habilitar)
                                        //Terminando última asignación del operador
                                        resultado = a.TerminaAsignacionOperadorUnidad(fecha_baja, id_usuario);
                                }

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                    //Actualizando estatus a baja
                                    resultado = editaRegistro(this._id_compania_emisor, Estatus.Baja, this._nombre, this._fecha_nacimiento, this._rfc, this._curp,
                                                                  this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso,
                                                                  fecha_baja, this._id_tipo_licencia, this._numero_licencia, this._id_parada, this._id_movimiento,
                                                                  this._fecha_actualizacion, id_usuario, this._habilitar);
                            }
                        }
                        //Si la baja es previa al ingreso
                        else
                            resultado = new RetornoOperacion(string.Format("La fecha de baja '{0:dd/MM/yyyy}' debe ser posterior a la fecha de ingreso '{1:dd/MM/yyyy}' del operador.", fecha_baja, this._fecha_ingreso));
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Finalizando transacción
                        scope.Complete();
                }
            }
            else
                resultado = new RetornoOperacion("El estatus actual del operador no permite la baja del mismo.");*/

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Actualizamos el Estatus del Operador.
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatus(Estatus estatus, int id_usuario)
        {
            //Estable Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            resultado = editaRegistro(this._id_compania_emisor, estatus, (Tipo)this.id_tipo, this._nombre, this._fecha_nacimiento, this._rfc, this._curp,
                                               this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso,
                                               this._fecha_baja, this._id_tipo_licencia, this._numero_licencia, this._id_parada, this._id_movimiento,
                                               this._fecha_actualizacion, id_usuario, this._habilitar);

            //Devolvemos Valor 
            return resultado;
        }
        /// <summary>
        /// Actualizamos Estatus de los Operadores
        /// </summary>
        /// <param name="mitOperadores">Tabla de Operadores</param>
        /// <param name="estatus">Estatus  de la Unidad</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaEstatusOperadores(DataTable mitOperadores, Estatus estatus, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos origen de datos
            if (Validacion.ValidaOrigenDatos(mitOperadores))
            {
                //Por cada una de la Unidad
                foreach (DataRow r in mitOperadores.Rows)
                {
                    //Instanciamos Unidades
                    using (Operador objOperador = new Operador(r.Field<int>("Id")))
                    {
                        //Validamos que el estatus se diferente al que se desea actualizar
                        if (objOperador.estatus != estatus)
                            //Actualiza estatus de la Unidad
                            resultado = objOperador.ActualizaEstatus(estatus, id_usuario);
                    }

                    //Si hay errores
                    if (!resultado.OperacionExitosa)
                        break;
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización del id de parada y movimiento actuales del operador
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="fecha_actualizacion">Fecha de actualización</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaParadaYMovimiento(int id_parada, int id_movimiento, DateTime fecha_actualizacion, int id_usuario)
        {
            //Realizando actualización
            return editaRegistro(this._id_compania_emisor, this.estatus, (Tipo)this.id_tipo, this._nombre, this._fecha_nacimiento, this._rfc, this._curp,
                            this._nss, this._r_control, this._telefono, this._telefono_casa, this._id_direccion, this._fecha_ingreso, this._fecha_baja,
                            this._id_tipo_licencia, this._numero_licencia, id_parada, id_movimiento, fecha_actualizacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la recarga de los atributos propios de la instancia
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributosInstancia()
        {
            return cargaAtributosInstancia(this._id_operador);
        }
        /// <summary>
        /// Obtiene el conjunto de registros operador que coincida con los filtros solicitados
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía</param>
        /// <param name="nombre">Nombre del operador</param>
        /// <param name="id_estatus">Id de Estatus</param>
        /// <param name="id_ubicacion_actual">Id de la Ubicación actual del operador</param>
        /// <returns></returns>
        public static DataTable CargaReporteOperadores(int id_compania_emisor, string nombre, byte id_estatus, int id_ubicacion_actual)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Declarando arreglo de parámetros para consulta en BD
            //Armando Objeto de Parametros
            object[] param = { 6, 0, id_compania_emisor, id_estatus, 0, nombre, null, "", "", "", "", "", "", 0, null, null, 0, "", 0, 0, null, 0, false, id_ubicacion_actual, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Método encargado de Liberar al Operador
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion LiberarOperador(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Obtenemos la Unidad Asignada al Operador
            int id_unidad = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(this._id_operador);
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Actualizamos Estatus del Operador
                resultado = this.ActualizaEstatusADisponible(id_usuario);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Validamos que exista la Unidad
                    if (id_unidad > 0)
                    {
                        //Instanciamos la Unidad 
                        using (Unidad objUnidad = new Unidad(id_unidad))
                        {
                            //Validamos Unidad
                            if (objUnidad.id_unidad > 0)
                            {
                                //Instanciamos Tipo de Unidad
                                using (UnidadTipo objUnidadTipo = new UnidadTipo(objUnidad.id_tipo_unidad))
                                {
                                    //Validamos que exista el Tipo de Unidad
                                    if (objUnidadTipo.id_tipo_unidad > 0)
                                    {
                                        //Validamos que el Tipo de Unidad sea Motriz y permita Arrastre
                                        if (objUnidadTipo.bit_motriz == true && objUnidadTipo.bit_permite_arrastre == true)
                                        {
                                            //Declaramos Variable para obtener la estancia actual de la unidad
                                            int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_unidad);

                                            //Instanciamos Estancia de la Unidad
                                            using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(id_estancia))
                                            {
                                                //Validamos Estancia
                                                if (objEstanciaUnidad.id_estancia_unidad > 0)
                                                {
                                                    //Declaramos variable para la parada comodin
                                                    int idParadaNuevo = 0;
                                                    //Instanciamos la parada de la Estancia
                                                    using (Parada objParada = new Parada(objEstanciaUnidad.id_parada))
                                                    {
                                                        //Validamos Ultima Parada
                                                        if (objParada.id_parada > 0)
                                                        {
                                                            //Verificando existencia de parada alterna en la ubicación actual
                                                            idParadaNuevo = Parada.ObtieneParadaComodinUbicacion(objParada.id_ubicacion, true, id_usuario);

                                                            //Validamos que exista Parada Comodin
                                                            if (idParadaNuevo != 0)
                                                            {
                                                                //Editamos Estancia de la Unidad
                                                                resultado = objEstanciaUnidad.CambiaParadaEstanciaUnidad(idParadaNuevo, id_usuario);

                                                                //Validamos Resultado
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Actualizamos Estatus de la Unidad
                                                                    resultado = objUnidad.ActualizaEstatusADisponible(id_usuario);

                                                                    //Validamos Resultado
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Refrescamos Atributos
                                                                        if (objUnidad.ActualizaAtributosInstancia())
                                                                        {
                                                                            //Actualizamos Atributos principales de la Unidad
                                                                            resultado = objUnidad.ActualizaEstanciaYMovimiento(objEstanciaUnidad.id_estancia_unidad, 0, objParada.fecha_llegada, id_usuario);

                                                                            //Validamos Resultado
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Obtenemos Asignación Iniciada de la Unidad
                                                                                int IdAsignacionUnidad = MovimientoAsignacionRecurso.ObtieneMovimientoAsignacionIniciada(MovimientoAsignacionRecurso.Tipo.Unidad, objUnidad.id_unidad);

                                                                                //Validamos Existencia de Asignación
                                                                                if (IdAsignacionUnidad != 0)
                                                                                {
                                                                                    //Instanciamos Asignación
                                                                                    using (MovimientoAsignacionRecurso objAsignacionUnidad = new MovimientoAsignacionRecurso(IdAsignacionUnidad))
                                                                                    {
                                                                                        //Cancelamos Asignación
                                                                                        resultado = objAsignacionUnidad.CancelaMovimientoAsignacionRecurso(id_usuario);
                                                                                    }
                                                                                }

                                                                                //Validamos Resultado en caso de tener Asignación Iniciada
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Refrescamos Atributos del Operador
                                                                                    if (this.ActualizaAtributosInstancia())
                                                                                    {
                                                                                        //Actualizamos Operador
                                                                                        resultado = this.ActualizaParadaYMovimiento(idParadaNuevo, 0, objParada.fecha_llegada, id_usuario);

                                                                                        //Validamos Resultado
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //Obtenemos Asignación Iniciada del Operador
                                                                                            int IdAsignacionOperador = MovimientoAsignacionRecurso.ObtieneMovimientoAsignacionIniciada(MovimientoAsignacionRecurso.Tipo.Operador, this._id_operador);

                                                                                            //Validamos Existencia de Asignación
                                                                                            if (IdAsignacionOperador != 0)
                                                                                            {
                                                                                                //Instanciamos Asignación del Operador
                                                                                                using (MovimientoAsignacionRecurso objAsignacionOperador = new MovimientoAsignacionRecurso(IdAsignacionOperador))
                                                                                                {
                                                                                                    ////Declaramos Variable para Validación de Viaje Activo
                                                                                                    //bool viajeActivo = MovimientoAsignacionRecurso.ValidaViajeActivo(objAsignacionOperador.id_tipo_asignacion, objAsignacionOperador.id_recurso_asignado, objAsignacionOperador.id_movimiento);
                                                                                                  

                                                                                                    //Cancelamos Asignación
                                                                                                    resultado = objAsignacionOperador.CancelaMovimientoAsignacionRecurso(id_usuario);

                                                                                                  ////Validamos Resultado
                                                                                                  //  if(resultado.OperacionExitosa)
                                                                                                  //  {
                                                                                                  //   //Realizando envio de notificación al recurso asignado
                                                                                                  //  Global.NotificacionPush.Instance.EliminaAsignacionServicio(objAsignacionOperador.id_movimiento_asignacion_recurso,viajeActivo);
                                                                                                  //  }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        //Establecemos Mensaje Resultado
                                                                                        resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad");
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Mostramos Error
                                                                            resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad " + objUnidad.numero_unidad + ".");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //Establecemos Mensaje Resultado
                                                                resultado = new RetornoOperacion("No se encontró datos complementarios de la Parada Comodin");

                                                        }
                                                        else
                                                            //Establecemos Mensaje Resultado
                                                            resultado = new RetornoOperacion("No se encontró datos complementarios de la parada.");
                                                    }
                                                }
                                                else
                                                    //Establecemos Mesnaje Resultado;
                                                    resultado = new RetornoOperacion("Error al Obtener datos complementarios de la Estancia de la Unidad");
                                            }
                                        }
                                        else
                                            //Establecemos Mensaje Resultado
                                            resultado = new RetornoOperacion("Sólo es posible la liberación de Unidades Motrices que aceptan Unidades de Arrastre.");
                                    }
                                    else
                                        //Establecemos Mensaje Resultado
                                        resultado = new RetornoOperacion("No se encontró datos complementarios del Tipo de Unidad");
                                }
                            }
                            else
                                //Establcemos Mensaje Resultado
                                resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad");
                        }
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {

                    //Finalizamos Transacción
                    scope.Complete();
                }
            }
            //Devolvemos Mensaje Retorno
            return resultado;
        }

        /// <summary>
        /// Obtiene la Ultima Asignación del Operador
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static int ObtieneIdUltimoOperador(int id_servicio)
        {
            //Definiendo objeto de retorno
            int id = 0;

            //Armando Arreglo de Parametros
            object[] param = { 7, 0, 0, 0, 0, "", null, "", "", "", "", "", "", 0, null, null, 0, "", 0, 0, null, 0, false, id_servicio, "" };
        


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos la Asignación
                    id = (from DataRow r in ds.Tables[0].Rows
                          select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id;
            }

        }
        /// <summary>
        /// Método encargado de Obtener el Operador Ligado al Usuario
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static Operador ObtieneOperadorUsuario(int id_usuario)
        {
            //Declarando Objeto de Retorno
            Operador op = new Operador();

            //Armando Arreglo de Parametros
            object[] param = { 8, 0, 0, 0, 0, "", null, "", "", "", "", "", "", 0, null, null, 0, "", 0, 0, null, 0, false, id_usuario, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Obtenemos la Asignación
                    op = new Operador((from DataRow r in ds.Tables[0].Rows
                                       select Convert.ToInt32(r["IdOperador"])).FirstOrDefault());
            }

            //Devolviendo Resultado Obtenido
            return op;
        }

        #endregion
    }
}
