using System;
using System.Data;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{
    /// <summary>
    /// Clase encargada de las Operaciones correspondientes a los Eventos de los Detalles de Acceso
    /// </summary>
    public class EventoDetalleAcceso : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Tipo de Actualización 
        /// </summary>
        public enum TipoActualizacion
        {   /// <summary>
            /// Expresa que la Actualización fue hecha por la Interfaz Web
            /// </summary>
            Web = 1,
            /// <summary>
            /// Expresa que la Actualización fue hecha por la Interfaz Mobil
            /// </summary>
            Mobil
        }
        /// <summary>
        /// Enumeración que expresa el Tipo de Confirmación del Evento
        /// </summary>
        public enum TipoConfirmacion
        {   /// <summary>
            /// Expresa que la Confirmación es de Inicio del Evento
            /// </summary>
            Inicio = 1,
            /// <summary>
            /// Expresa que la Confirmación es de Fin del Evento
            /// </summary>
            Fin
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_evento_detalle_acceso_teda";

        private int _id_evento_detalle_acceso;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Evento del Detalle
        /// </summary>
        public int id_evento_detalle_acceso { get { return this._id_evento_detalle_acceso; } }
        private int _id_detalle_acceso_patio;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public int id_detalle_acceso_patio { get { return this._id_detalle_acceso_patio; } }
        private int _id_tipo_evento;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Evento
        /// </summary>
        public int id_tipo_evento { get { return this._id_tipo_evento; } }
        private int _id_entidad_patio;
        /// <summary>
        /// Atributo encargado de almacenar la Entidad del Patio
        /// </summary>
        public int id_entidad_patio { get { return this._id_entidad_patio; } }
        private int _id_zona_patio;
        /// <summary>
        /// Atributo encargado de almacenar la Zona del Patio
        /// </summary>
        public int id_zona_patio { get { return this._id_zona_patio; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private string _referencia;
        /// <summary>
        /// Atributo encargado de almacenar la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private byte _id_tipo_actualizacion_inicio;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Actualización de Inicio
        /// </summary>
        public byte id_tipo_actualizacion_inicio { get { return this._id_tipo_actualizacion_inicio; } }
        private byte _id_tipo_actualizacion_fin;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Actualización de Fin
        /// </summary>
        public byte id_tipo_actualizacion_fin { get { return this._id_tipo_actualizacion_fin; } }
        private bool _bit_confirmacion_asignacion_ini;
        /// <summary>
        /// Atributo encargado de almacenar el Indicador de Confirmación de Inicio de la Asignación
        /// </summary>
        public bool bit_confirmacion_asignacion_ini { get { return this._bit_confirmacion_asignacion_ini; } }
        private bool _bit_confirmacion_asignacion_fin;
        /// <summary>
        /// Atributo encargado de almacenar el Indicador de Confirmación de Fin de la Asignación
        /// </summary>
        public bool bit_confirmacion_asignacion_fin { get { return this._bit_confirmacion_asignacion_fin; } }
        private int _id_operador_asignado;
        /// <summary>
        /// Atributo encargado de almacenar el Id Operador Asignado
        /// </summary>
        public int id_operador_asignado { get { return this._id_operador_asignado; } }
        private DateTime _fecha_asignacion;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public DateTime fecha_asignacion { get { return this._fecha_asignacion; } }
        private DateTime _fecha_confirmacion;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public DateTime fecha_confirmacion { get { return this._fecha_confirmacion; } }
        private bool _bit_inicio_anden;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public bool bit_inicio_anden { get { return this._bit_inicio_anden; } }
        private bool _bit_fin_anden;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public bool bit_fin_anden { get { return this._bit_fin_anden; } }
        private DateTime _fecha_inicio_anden;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public DateTime fecha_inicio_anden { get { return this._fecha_inicio_anden; } }
        private DateTime _fecha_fin_anden;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public DateTime fecha_fin_anden { get { return this._fecha_fin_anden; } }
        private int _id_operador_fin;
        /// <summary>
        /// Atributo encargado de almacenar el Detalle de Acceso
        /// </summary>
        public int id_operador_fin { get { return this._id_operador_fin; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private byte[] _row_version;
        /// <summary>
        /// Atributo encargado de Almacenar la Versión del Registro
        /// </summary>
        public byte[] row_version { get { return this._row_version; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public EventoDetalleAcceso()
        {   //Asignando Valores
            this._id_evento_detalle_acceso = 0;
            this._id_detalle_acceso_patio = 0;
            this._id_tipo_evento = 0;
            this._id_entidad_patio = 0;
            this._id_zona_patio = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._referencia = "";
            this._id_tipo_actualizacion_inicio = 0;
            this._id_tipo_actualizacion_fin = 0;
            this._bit_confirmacion_asignacion_ini = false;
            this._bit_confirmacion_asignacion_fin = false;
            this._id_operador_asignado = 0;
            this._fecha_asignacion = DateTime.MinValue;
            this._fecha_confirmacion = DateTime.MinValue;
            this._bit_inicio_anden = false;
            this._bit_fin_anden = false;
            this._fecha_inicio_anden = DateTime.MinValue;
            this._fecha_fin_anden = DateTime.MinValue;
            this._id_operador_fin = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_evento_detalle_acceso">Id de Registro</param>
        public EventoDetalleAcceso(int id_evento_detalle_acceso)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_evento_detalle_acceso);
        }
        public EventoDetalleAcceso(string id_detalle_acceso_patio)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_detalle_acceso_patio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~EventoDetalleAcceso()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_evento_detalle_acceso">Id de Evento de Detalle de Acceso</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_evento_detalle_acceso)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_evento_detalle_acceso, 0, 0, 0, 0, null, null, "", 0, 0, false, false, 0, null, null,false,false,null,null,0,0,false, null, "", "" };
            //Instanciando Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_evento_detalle_acceso = id_evento_detalle_acceso;
                        this._id_detalle_acceso_patio = Convert.ToInt32(dr["IdDetalleAccesoPatio"]);
                        this._id_tipo_evento = Convert.ToInt32(dr["IdTipoEvento"]);
                        this._id_entidad_patio = Convert.ToInt32(dr["IdEntidadPatio"]);
                        this._id_zona_patio = Convert.ToInt32(dr["IdZonaPatio"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        this._referencia = dr["Referencia"].ToString();
                        this._id_tipo_actualizacion_inicio = Convert.ToByte(dr["IdTipoActualizacionInicio"]);
                        this._id_tipo_actualizacion_fin = Convert.ToByte(dr["IdTipoActualizacionFin"]);
                        this._bit_confirmacion_asignacion_ini = Convert.ToBoolean(dr["BitConfirmacionAsignacionIni"]);
                        this._bit_confirmacion_asignacion_fin = Convert.ToBoolean(dr["BitConfirmacionAsignacionFin"]);
                        this._id_operador_asignado = Convert.ToInt32(dr["IdOperadorAsignado"]);
                        DateTime.TryParse(dr["FechaAsignacion"].ToString(), out this._fecha_asignacion);
                        DateTime.TryParse(dr["FechaConfirmacion"].ToString(), out this._fecha_confirmacion);
                        this._bit_inicio_anden = Convert.ToBoolean(dr["BitInicioAnden"]);
                        this._bit_fin_anden = Convert.ToBoolean(dr["BitFinAnden"]); ;
                        DateTime.TryParse(dr["FechaInicioAnden"].ToString(), out this._fecha_inicio_anden);
                        DateTime.TryParse(dr["FechaFinAnden"].ToString(), out this._fecha_fin_anden);
                        this._id_operador_fin = Convert.ToInt32(dr["IdOperadorFin"]);

                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._row_version = (byte[])dr["RowVersion"];
                    }
                    //Asignando Valor Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        private bool cargaAtributosInstancia(string id_detalle_acceso_patio)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 10, 0, Convert.ToInt32(id_detalle_acceso_patio), 0, 0, 0, null, null, "", 0, 0, false, false, 0, null, null, false, false, null, null, 0, 0, false, null, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_evento_detalle_acceso = Convert.ToInt32(dr["Id"]);
                        this._id_detalle_acceso_patio = Convert.ToInt32(id_detalle_acceso_patio);
                        this._id_tipo_evento = Convert.ToInt32(dr["IdTipoEvento"]);
                        this._id_entidad_patio = Convert.ToInt32(dr["IdEntidadPatio"]);
                        this._id_zona_patio = Convert.ToInt32(dr["IdZonaPatio"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        this._referencia = dr["Referencia"].ToString();
                        this._id_tipo_actualizacion_inicio = Convert.ToByte(dr["IdTipoActualizacionInicio"]);
                        this._id_tipo_actualizacion_fin = Convert.ToByte(dr["IdTipoActualizacionFin"]);
                        this._bit_confirmacion_asignacion_ini = Convert.ToBoolean(dr["BitConfirmacionAsignacionIni"]);
                        this._bit_confirmacion_asignacion_fin = Convert.ToBoolean(dr["BitConfirmacionAsignacionFin"]);
                        this._id_operador_asignado = Convert.ToInt32(dr["IdOperadorAsignado"]);
                        DateTime.TryParse(dr["FechaAsignacion"].ToString(), out this._fecha_asignacion);
                        DateTime.TryParse(dr["FechaConfirmacion"].ToString(), out this._fecha_confirmacion);
                        this._bit_inicio_anden = Convert.ToBoolean(dr["BitInicioAnden"]);
                        this._bit_fin_anden = Convert.ToBoolean(dr["BitFinAnden"]); ;
                        DateTime.TryParse(dr["FechaInicioAnden"].ToString(), out this._fecha_inicio_anden);
                        DateTime.TryParse(dr["FechaFinAnden"].ToString(), out this._fecha_fin_anden);
                        this._id_operador_fin = Convert.ToInt32(dr["IdOperadorFin"]);

                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._row_version = (byte[])dr["RowVersion"];
                    }
                    //Asignando Valor Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Validar si el Registro se puede Modificar
        /// </summary>
        /// <returns></returns>
        private bool validaRegistro()
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros            
            object[] param = { 4, id_evento_detalle_acceso, 0, 0, 0, 0, null, null, "", 0, 0, false, false, 0, null, null, false, false, null, null, 0, 0, false, this._row_version, "", "" };


            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Validación en Positivo
                    result = true;
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_detalle_acceso_patio">Detalle del Acceso de Patio</param>
        /// <param name="id_tipo_evento">Tipo de Evento</param>
        /// <param name="id_entidad_patio">Entidad de Patio</param>
        /// <param name="id_zona_patio">Zona de Patio</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo_actualizacion_inicio">Tipo de Actualización de Inicio</param>
        /// <param name="id_tipo_actualizacion_fin">Tipo de Actualización de Fin</param>
        /// <param name="bit_confirmacion_asignacion_ini">Indicador de Confirmación de Inicio de la Asignación</param>
        /// <param name="bit_confirmacion_asignacion_fin">Indicador de Confirmación de Fin de la Asignación</param>
        /// <param name="id_operador_asignado">Operador asignado</param>
        /// <param name="fecha_asignacion">Fecha de asignación</param>
        /// <param name="fecha_confirmacion">Fecha de confirmación</param>
        /// <param name="bit_inicio_anden">Indicador de Inicio en Andén</param>
        /// <param name="bit_fin_anden">Indicador de Fin en Andén</param>
        /// <param name="fecha_inicio_anden">Fecha de Inicio en Andén</param>
        /// <param name="fecha_fin_anden">Fecha de Fin en Andén</param>
        /// <param name="id_operador_fin">Operador Fin</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_detalle_acceso_patio, int id_tipo_evento, int id_entidad_patio, int id_zona_patio, 
                               DateTime fecha_inicio, DateTime fecha_fin, string referencia, byte id_tipo_actualizacion_inicio, byte id_tipo_actualizacion_fin, 
                               bool bit_confirmacion_asignacion_ini, bool bit_confirmacion_asignacion_fin, int id_operador_asignado, DateTime fecha_asignacion,
                               DateTime fecha_confirmacion, bool bit_inicio_anden, bool bit_fin_anden, DateTime fecha_inicio_anden, DateTime fecha_fin_anden,
                               int id_operador_fin, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que el Registro se pueda Actualizar
            if (validaRegistro())
            {   //Armando Arreglo de Parametros
                object[] param = { 2, this._id_evento_detalle_acceso, id_detalle_acceso_patio, id_tipo_evento, id_entidad_patio, id_zona_patio, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), referencia, id_tipo_actualizacion_inicio, id_tipo_actualizacion_fin, 
                               bit_confirmacion_asignacion_ini, bit_confirmacion_asignacion_fin, id_operador_asignado, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_asignacion),
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_confirmacion), bit_inicio_anden, bit_fin_anden, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio_anden),
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin_anden), id_operador_fin, id_usuario, habilitar, this._row_version, "", "" };
                //Obteniendo Resultado Obtenidofecha_inicio_anden
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }
            else//Instanciando Excepcion
                result = new RetornoOperacion("El registro fue modificado en BD desde la última vez que fue consultado.");
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Eventos de los Detalles de Acceso
        /// </summary>
        /// <param name="id_detalle_acceso_patio">Detalle del Acceso de Patio</param>
        /// <param name="id_tipo_evento">Tipo de Evento</param>
        /// <param name="id_entidad_patio">Entidad de Patio</param>
        /// <param name="id_zona_patio">Zona de Patio</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo_actualizacion_inicio">Tipo de Actualización de Inicio</param>
        /// <param name="id_tipo_actualizacion_fin">Tipo de Actualización de Fin</param>
        /// <param name="bit_confirmacion_asignacion_ini">Indicador de Confirmación de Inicio de la Asignación</param>
        /// <param name="bit_confirmacion_asignacion_fin">Indicador de Confirmación de Fin de la Asignación</param>
        /// <param name="id_operador_asignado">Operador asignado</param>
        /// <param name="fecha_asignacion">Fecha de asignación</param>
        /// <param name="fecha_confirmacion">Fecha de confirmación</param>
        /// <param name="bit_inicio_anden">Indicador de Inicio en Andén</param>
        /// <param name="bit_fin_anden">Indicador de Fin en Andén</param>
        /// <param name="fecha_inicio_anden">Fecha de Inicio en Andén</param>
        /// <param name="fecha_fin_anden">Fecha de Fin en Andén</param>
        /// <param name="id_operador_fin">Operador Fin</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaEventoDetalleAcceso(int id_detalle_acceso_patio, int id_tipo_evento, int id_entidad_patio, int id_zona_patio,
                               DateTime fecha_inicio, DateTime fecha_fin, string referencia, byte id_tipo_actualizacion_inicio, byte id_tipo_actualizacion_fin,
                               bool bit_confirmacion_asignacion_ini, bool bit_confirmacion_asignacion_fin, int id_operador_asignado, DateTime fecha_asignacion,
                               DateTime fecha_confirmacion, bool bit_inicio_anden, bool bit_fin_anden, DateTime fecha_inicio_anden, DateTime fecha_fin_anden,
                               int id_operador_fin, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_detalle_acceso_patio, id_tipo_evento, id_entidad_patio, id_zona_patio, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               referencia, id_tipo_actualizacion_inicio, id_tipo_actualizacion_fin, bit_confirmacion_asignacion_ini, 
                               bit_confirmacion_asignacion_fin, id_operador_asignado, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_asignacion),
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_confirmacion), bit_inicio_anden, bit_fin_anden, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio_anden), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin_anden),
                               id_operador_fin, id_usuario, true, null, "", "" };
            //Obteniendo Resultado Obtenido
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Eventos de los Detalles de Acceso
        /// </summary>
        /// <param name="id_detalle_acceso_patio">Detalle del Acceso de Patio</param>
        /// <param name="id_tipo_evento">Tipo de Evento</param>
        /// <param name="id_entidad_patio">Entidad de Patio</param>
        /// <param name="id_zona_patio">Zona de Patio</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo_actualizacion_inicio">Tipo de Actualización de Inicio</param>
        /// <param name="id_tipo_actualizacion_fin">Tipo de Actualización de Fin</param>
        /// <param name="bit_confirmacion_asignacion_ini">Indicador de Confirmación de Asignación de Inicio</param>
        /// <param name="bit_confirmacion_asignacion_fin">Indicador de Confirmación de Asignación de Fin</param>
        /// <param name="id_operador_asignado">Operador asignado</param>
        /// <param name="fecha_asignacion">Fecha de asignación</param>
        /// <param name="fecha_confirmacion">Fecha de confirmación</param>
        /// <param name="bit_inicio_anden">Indicador de Inicio en Andén</param>
        /// <param name="bit_fin_anden">Indicador de Fin en Andén</param>
        /// <param name="fecha_inicio_anden">Fecha de Inicio en Andén</param>
        /// <param name="fecha_fin_anden">Fecha de Fin en Andén</param>
        /// <param name="id_operador_fin">Operador Fin</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaEventoDetalleAcceso(int id_detalle_acceso_patio, int id_tipo_evento, int id_entidad_patio, int id_zona_patio,
                               DateTime fecha_inicio, DateTime fecha_fin, string referencia, byte id_tipo_actualizacion_inicio, byte id_tipo_actualizacion_fin,
                               bool bit_confirmacion_asignacion_ini, bool bit_confirmacion_asignacion_fin, int id_operador_asignado, DateTime fecha_asignacion,
                               DateTime fecha_confirmacion, bool bit_inicio_anden, bool bit_fin_anden, DateTime fecha_inicio_anden, DateTime fecha_fin_anden,
                               int id_operador_fin, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_detalle_acceso_patio, id_tipo_evento, id_entidad_patio, id_zona_patio,
                               fecha_inicio, fecha_fin, referencia, id_tipo_actualizacion_inicio, id_tipo_actualizacion_fin,
                               bit_confirmacion_asignacion_ini, bit_confirmacion_asignacion_fin, id_operador_asignado, fecha_asignacion,
                               fecha_confirmacion, bit_inicio_anden, bit_fin_anden, fecha_inicio_anden,
                               fecha_fin_anden, id_operador_fin, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Eventos de los Detalles de Acceso
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaEventoDetalleAcceso(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_detalle_acceso_patio, this._id_tipo_evento, this._id_entidad_patio, this._id_zona_patio,
                               this._fecha_inicio, this._fecha_fin, this._referencia, this._id_tipo_actualizacion_inicio, this._id_tipo_actualizacion_fin,
                               this._bit_confirmacion_asignacion_ini, this._bit_confirmacion_asignacion_fin, this._id_operador_asignado, this._fecha_asignacion,
                               this._fecha_confirmacion, this._bit_inicio_anden, this._bit_fin_anden, this._fecha_inicio_anden, this._fecha_fin_anden,
                               this._id_operador_fin, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de los Eventos de los Detalles de Acceso
        /// </summary>
        /// <returns></returns>
        public bool ActualizaEventoDetalleAcceso()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_evento_detalle_acceso);
        }
        /// <summary>
        /// Método Público encargado de Terminar el Evento del Detalle
        /// </summary>
        /// <param name="fecha_fin">Fecha de Termino del Evento</param>
        /// <param name="tipo_actualizacion_fin">Tipo de Actualización de Termino</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion TerminaEventoDetalle(DateTime fecha_fin, TipoActualizacion tipo_actualizacion_fin, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_detalle_acceso_patio, this._id_tipo_evento, this._id_entidad_patio, this._id_zona_patio,
                               this._fecha_inicio, fecha_fin, this._referencia, this._id_tipo_actualizacion_inicio, (byte)tipo_actualizacion_fin,
                               this._bit_confirmacion_asignacion_ini, this._bit_confirmacion_asignacion_fin, this._id_operador_asignado, this._fecha_asignacion,
                               this._fecha_confirmacion, this._bit_inicio_anden, this._bit_fin_anden, this._fecha_inicio_anden, this._fecha_fin_anden,
                               this._id_operador_fin, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Actualizar la Confirmación del Evento
        /// </summary>
        /// <param name="tipo">Tipo de Confirmación</param>
        /// <param name="fecha">Fecha de Actualización</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaConfirmacionEvento(TipoConfirmacion tipo, DateTime fecha, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invocando Método de Actualización
                result = this.actualizaRegistros(this._id_detalle_acceso_patio, this._id_tipo_evento, this._id_entidad_patio, this._id_zona_patio,
                                   tipo == TipoConfirmacion.Inicio ? fecha : this._fecha_inicio, tipo == TipoConfirmacion.Fin ? fecha : this._fecha_fin,
                                   this._referencia, tipo == TipoConfirmacion.Inicio ? (byte)TipoActualizacion.Mobil : this._id_tipo_actualizacion_inicio,
                                   tipo == TipoConfirmacion.Fin ? (byte)TipoActualizacion.Mobil : this._id_tipo_actualizacion_fin,
                                   tipo == TipoConfirmacion.Inicio ? true : this._bit_confirmacion_asignacion_ini,
                                   tipo == TipoConfirmacion.Fin ? true : this._bit_confirmacion_asignacion_fin, this._id_operador_asignado, this._fecha_asignacion,
                                   this._fecha_confirmacion, this._bit_inicio_anden, this._bit_fin_anden, this._fecha_inicio_anden, this._fecha_fin_anden,
                                   this._id_operador_fin, id_usuario, this._habilitar);

                //Validando que se actualizara el Registro
                if(result.OperacionExitosa)
                {
                    //Instanciando Entidad Patio
                    using(EntidadPatio ep = new EntidadPatio(this._id_entidad_patio))
                    {
                        //Instanciando Entidad de Patio(Anden/Cajon)
                        if(ep.id_entidad_patio > 0)

                            //Actualizando Entidad 
                            result = ep.ActualizaEventoActualEntidadPatio(ep.estatus, ep.estatus_carga, fecha, ep.id_evento, id_usuario);

                        //Validando que la Operación haya sido Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Detalle Acceso
                            using (DetalleAccesoPatio dap = new DetalleAccesoPatio(this._id_detalle_acceso_patio))
                            {
                                //Validando que exista el Registro
                                if (dap.id_detalle_acceso_patio > 0)
                                {
                                    //Validando que sea una Entidad de Tipo Anden
                                    if (ep.tipo_entidad == EntidadPatio.TipoEntidad.Anden)

                                        result = dap.ActualizaAndenCargaDetalle(dap.id_entidad_anden, dap.bit_cargado, (DetalleAccesoPatio.EstatusPatio)dap.id_estatus_patio, id_usuario);
                                    
                                    //Validando que sea una Entidad de Tipo Cajon
                                    if (ep.tipo_entidad == EntidadPatio.TipoEntidad.Cajon)

                                        result = dap.ActualizaCajonCargaDetalle(dap.id_entidad_cajon, dap.bit_cargado, (DetalleAccesoPatio.EstatusPatio)dap.id_estatus_patio, id_usuario);
                                    //Completando Transacción
                                    trans.Complete();
                                }
                            }
                        }
                    }
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Público encargado de Actualizar la Confirmación del Evento
        /// </summary>
        /// <param name="tipo">Tipo de Confirmación</param>
        /// <param name="fecha">Fecha de Actualización</param>
        /// <param name="tipo_actualizacion_fin">Tipo de Actualización</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaConfirmacionEvento(TipoConfirmacion tipo, DateTime fecha, TipoActualizacion tipo_actualizacion_fin, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invocando Método de Actualización
                result = this.actualizaRegistros(this._id_detalle_acceso_patio, this._id_tipo_evento, this._id_entidad_patio, this._id_zona_patio,
                                   tipo == TipoConfirmacion.Inicio ? fecha : this._fecha_inicio, tipo == TipoConfirmacion.Fin ? fecha : this._fecha_fin,
                                   this._referencia, this._id_tipo_actualizacion_inicio, (byte)tipo_actualizacion_fin,
                                   tipo == TipoConfirmacion.Inicio ? true : this._bit_confirmacion_asignacion_ini,
                                   tipo == TipoConfirmacion.Fin ? true : this._bit_confirmacion_asignacion_fin, this._id_operador_asignado, this._fecha_asignacion,
                               this._fecha_confirmacion, this._bit_inicio_anden, this._bit_fin_anden, this._fecha_inicio_anden, this._fecha_fin_anden,
                               this._id_operador_fin, id_usuario, this._habilitar);

                //Validando que se actualizara el Registro
                if (result.OperacionExitosa)
                {
                    //Obteniendo Evento
                    int id_evt = result.IdRegistro;

                    //Instanciando Entidad Patio
                    using (EntidadPatio ep = new EntidadPatio(this._id_entidad_patio))
                    {
                        //Instanciando Entidad de Patio(Anden/Cajon)
                        if (ep.id_entidad_patio > 0)

                            //Actualizando Entidad 
                            result = ep.ActualizaEventoActualEntidadPatio(ep.estatus, ep.estatus_carga, fecha, ep.id_evento, id_usuario);

                        //Validando que la Operación haya sido Exitosa
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Eventos sin Terminar de un Detalle de Acceso
        /// </summary>
        /// <param name="id_detalle_acceso">Detalle de Acceso</param>
        /// <returns></returns>
        public static DataTable ObtieneEventosDetalle(int id_detalle_acceso)
        {   //Declarando Objeto de Retorno
            DataTable dtEventos = null;
            //Armando Arreglo de Parametros            
            object[] param = { 5, 0, id_detalle_acceso, 0, 0, 0, null, null, "", 0, 0, false, false, 0, null, null, false, false, null, null, 0, 0, false, null, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtEventos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtEventos;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Eventos Terminados de un Detalle de Acceso
        /// </summary>
        /// <param name="id_detalle_acceso">Detalle de Acceso</param>
        /// <returns></returns>
        public static DataTable ObtieneEventosDetalleTerminados(int id_detalle_acceso)
        {   //Declarando Objeto de Retorno
            DataTable dtEventos = null;
            //Armando Arreglo de Parametros
            object[] param = { 6, 0, id_detalle_acceso, 0, 0, 0, null, null, "", 0, 0, false, false, 0, null, null, false, false, null, null, 0, 0, false, null, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtEventos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtEventos;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Eventos Terminados en el Dia de una Entidad de Patio (Anden/Cajon)
        /// </summary>
        /// <param name="id_entidad">Entidad de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneEventosEntidadTerminadosDia(int id_entidad)
        {   //Declarando Objeto de Retorno
            DataTable dtEventos = null;
            //Armando Arreglo de Parametros
            object[] param = { 7, 0, 0, 0, id_entidad, 0, null, null, "", 0, 0, false, false, 0, null, null, false, false, null, null, 0, 0, false, null, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtEventos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtEventos;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Eventos Terminados de una Entidad de Patio (Anden/Cajon)
        /// </summary>
        /// <param name="id_entidad">Entidad de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneEventosEntidadTerminadosHistorial(int id_entidad)
        {   //Declarando Objeto de Retorno
            DataTable dtEventos = null;
            //Armando Arreglo de Parametros
            object[] param = { 8, 0, 0, 0, id_entidad, 0, null, null, "", 0, 0, false, false, 0, null, null, false, false, null, null, 0, 0, false, null, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtEventos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtEventos;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Eventos 
        /// </summary>
        /// <param name="id_detalle_acceso">Detalle de Acceso(Unidad)</param>
        /// <returns></returns>
        public static DataTable ObtieneEventosPorDetalleInstruccion(int id_detalle_acceso)
        {   //Declarando Objeto de Retorno
            DataTable dtEventos = null;
            //Armando Arreglo de Parametros
            object[] param = { 9, 0, id_detalle_acceso, 0, 0, id_detalle_acceso, null, null, "", 0, 0, false, false, 0, null, null, false, false, null, null, 0, 0, false, null, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtEventos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtEventos;
        }

        #endregion
    }
}
