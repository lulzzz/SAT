using SAT_CL.Documentacion;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using System.IO;
using SAT_CL.Global;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Implementa los métodos para la administración de Paradas Eventos
    /// </summary>
    public class ParadaEvento : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Estatus del Evento
        /// </summary>
        public enum EstatusParadaEvento
        {
            /// <summary>
            /// Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Iniciado
            /// </summary>
            Iniciado,
            /// <summary>
            /// Terminado
            /// </summary>
            Terminado
        }

        /// <summary>
        /// Enumera el Tipo de Actualización Inicio 
        /// </summary>
        public enum TipoActualizacionInicio
        {
            /// <summary>
            /// Sin Actualización
            /// </summary>
            SinActualizar = 0,
            /// <summary>
            ///  Manual
            /// </summary>
            Manual = 1,
            /// <summary>
            /// GPS
            /// </summary>
            GPS,
            /// <summary>
            /// APP
            /// </summary>
            APP,

        }

        /// <summary>
        /// Enumera el Tipo de Actualización Fin 
        /// </summary>
        public enum TipoActualizacionFin
        {
            /// <summary>
            /// Sin Actualización
            /// </summary>
            SinActualizar = 0,
            /// <summary>
            ///  Manual
            /// </summary>
            Manual = 1,
            /// <summary>
            /// GPS
            /// </summary>
            GPS,
            /// <summary>
            /// APP
            /// </summary>
            APP,

        }
        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "despacho.sp_parada_evento_tpe";


        private int _id_evento;
        /// <summary>
        /// Describe el Id Evento
        /// </summary>
        public int id_evento
        {
            get { return _id_evento; }
        }
        private int _id_servicio;
        /// <summary>
        /// Describe el Id de Servicio
        /// </summary>
        public int id_servicio
        {
            get { return _id_servicio; }
        }
        private int _id_parada;
        /// <summary>
        /// Describe el Id de Parada
        /// </summary>
        public int id_parada
        {
            get { return _id_parada; }
        }
        private decimal _secuencia_evento_parada;
        /// <summary>
        /// Describe la secuencia
        /// </summary>
        public decimal secuencia_evento_parada
        {
            get { return _secuencia_evento_parada; }
        }
        private int _id_elemento_ubicacion;
        /// <summary>
        /// Describe el elemento ubicación
        /// </summary>
        public int id_elemento_ubicacion
        {
            get { return _id_elemento_ubicacion; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Describe el estatus
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        private int _id_tipo_evento;
        /// <summary>
        /// Describe el tipo de evento
        /// </summary>
        public int id_tipo_evento
        {
            get { return _id_tipo_evento; }
        }
        private DateTime _cita_evento;
        /// <summary>
        /// Describe la cita del Evento
        /// </summary>
        public DateTime cita_evento
        {
            get { return _cita_evento; }
        }
        private DateTime _inicio_evento;
        /// <summary>
        /// Describe el inicio del Evento
        /// </summary>
        public DateTime inicio_evento
        {
            get { return _inicio_evento; }
        }
        private byte _id_tipo_actualizacion_inicio;
        /// <summary>
        /// Describe el tipo de actualización inicio
        /// </summary>
        public byte id_tipo_actualizacion_inicio
        {
            get { return _id_tipo_actualizacion_inicio; }
        }
        private DateTime _fin_evento;
        /// <summary>
        /// Describe el fin del evento
        /// </summary>
        public DateTime fin_evento
        {
            get { return _fin_evento; }
        }
        private byte _id_tipo_actualizacion_fin;
        /// <summary>
        /// Describe el tipo de actualización fin 
        /// </summary>
        public byte id_tipo_actualizacion_fin
        {
            get { return _id_tipo_actualizacion_fin; }
        }
        private byte _id_motivo_retraso_evento;
        /// <summary>
        /// Describe el motivo retraso evento
        /// </summary>
        public byte id_motivo_retraso_evento
        {
            get { return _id_motivo_retraso_evento; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        private string _descripcion_tipo_evento;
        /// <summary>
        /// Describe el Tipo de Evento
        /// </summary>
        public string descripcion_tipo_evento { get { return this._descripcion_tipo_evento; } }
        private byte[] _row_version;
        /// <summary>
        /// Describe la version del evento
        /// </summary>
        public byte[] row_version
        {
            get { return _row_version; }
        }
        /// <summary>
        /// Enumera el Estatus Evento
        /// </summary>
        public EstatusParadaEvento Estatus
        {
            get { return (EstatusParadaEvento)_id_estatus; }
        }
        /// <summary>
        /// Define el Tipo Actualizacion Inicio
        /// </summary>
        public TipoActualizacionInicio TipoActualizacionInicio_
        {
            get { return (TipoActualizacionInicio)_id_tipo_actualizacion_inicio; }
        }
        /// <summary>
        /// Define el Tipo Actualizacion Fin
        /// </summary>
        public TipoActualizacionFin TipoActualizacionFin_
        {
            get { return (TipoActualizacionFin)_id_tipo_actualizacion_fin; }
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ParadaEvento()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public ParadaEvento()
        {
        }

        /// <summary>
        ///  Genera una Instancia de Tipo Parada Evento
        /// </summary>
        /// <param name="id_evento">Id Evento</param>
        public ParadaEvento(int id_evento)
        {
            cargaAtributosInstancia(id_evento);
        }

        /// <summary>
        /// Genera una Instancia Parada Evento
        /// </summary>
        /// <param name="id_evento">Id Evento</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_evento)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_evento, 0, 0, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_evento = Convert.ToInt32(r["Id"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _id_parada = Convert.ToInt32(r["IdParada"]);
                        _secuencia_evento_parada = Convert.ToDecimal(r["SecuenciaEventoParada"]);
                        _id_elemento_ubicacion = Convert.ToInt32(r["IdElementoUbicacion"]);
                        _id_estatus = Convert.ToByte(r["IdEstatus"]);
                        _id_tipo_evento = Convert.ToInt32(r["IdTipoEvento"]);
                        DateTime.TryParse(r["CitaEvento"].ToString(), out _cita_evento);
                        DateTime.TryParse(r["InicioEvento"].ToString(), out _inicio_evento);
                        _id_tipo_actualizacion_inicio = Convert.ToByte(r["IdTipoActualizacionInicio"]);
                        DateTime.TryParse(r["FinEvento"].ToString(), out _fin_evento);
                        _id_tipo_actualizacion_fin = Convert.ToByte(r["IdTipoActualizacionFin"]);
                        _id_motivo_retraso_evento = Convert.ToByte(r["IdMotivoRetrasoEvento"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _row_version = (byte[])r["RowVersion"];
                        _descripcion_tipo_evento = r["DescripcionTipoEvt"].ToString();

                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }



        #endregion

        #region Metodos privados

        /// <summary>
        /// Método encargado de editar una Parada evento
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="secuencia_evento_parada">Secuencia del Evento</param>
        /// <param name="id_elemento_ubicacion">Id de elemento de patio, en caso de que este ligado.</param>
        /// <param name="estatus">Estatus del Evento</param>
        /// <param name="id_tipo_evento">Tipo de Evento al que pertenece</param>
        /// <param name="cita_evento">Cita del Evento</param>
        /// <param name="inicio_evento">Fecha de Inicio del Evento</param>
        /// <param name="tipo_actualizacion_inicio">Medio  que se utilizó  para actualizar el Inicio del Evento</param>
        /// <param name="fin_evento">Fecha de Inicio del Evento</param>
        /// <param name="tipo_actualizacion_fin">Medio  que se utilizó  para actualizar el Fin del Evento</param>
        /// <param name="id_motivo_retraso_evento">En caso de que el evento se realizó Tarde, establece la causa</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaParadaEvento(int id_servicio, int id_parada, decimal secuencia_evento_parada, int id_elemento_ubicacion, EstatusParadaEvento estatus,
                                                 int id_tipo_evento, DateTime cita_evento, DateTime inicio_evento, TipoActualizacionInicio tipo_actualizacion_inicio, DateTime fin_evento, TipoActualizacionFin tipo_actualizacion_fin,
                                                 byte id_motivo_retraso_evento, int id_usuario, bool habilitar)
        {
            //Establecemos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Versión del Registro
            if (validaVersionRegistro())
            {
                //Inicializando arreglo de parámetros
                object[] param = {2, this._id_evento, id_servicio, id_parada, secuencia_evento_parada, id_elemento_ubicacion, estatus,
                                 id_tipo_evento, Fecha.ConvierteDateTimeObjeto(cita_evento), Fecha.ConvierteDateTimeObjeto(inicio_evento), 
                                 tipo_actualizacion_inicio, Fecha.ConvierteDateTimeObjeto(fin_evento), tipo_actualizacion_fin,
                                 id_motivo_retraso_evento, id_usuario, habilitar, this._row_version, "", ""};

                //Establecemos Resultado
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            }
            else
            {
                //Establecmeos Error
                resultado = new RetornoOperacion("El registro fue modificado en BD desde la última vez que fue consultado.");
            }

            return resultado;
        }

        /// <summary>
        /// Validamos versión de Registro desde la Base de Datos y Instancia creada
        /// </summary>
        /// <returns></returns>
        private bool validaVersionRegistro()
        {
            //Declaramos Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 4, this._id_evento, 0, 0, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, this._row_version, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Establecemos Resultado correcto
                    resultado = true;
            }

            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        ///  Obtiene Ultima Secuencia del Evento
        /// </summary>
        /// <param name="id_parada">Id de la Parada.</param>
        /// <returns></returns>
        private static decimal obtieneSecuenciaEvento(int id_parada)
        {
            //Declaramos Resultados
            decimal secuencia = 1;

            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Secuencia
                    decimal secuenciaBD = (from DataRow r in ds.Tables[0].Rows
                                           select Convert.ToDecimal(r["Secuencia"])).FirstOrDefault();

                    //Asignamos Secuencia Correspondiente
                    secuencia = secuenciaBD + 1;
                }

            }

            //Obtenemos Resultado
            return secuencia;
        }

        /// <summary>
        /// Valimos existencia de productos ligando el tipo de evento
        /// </summary>
        private RetornoOperacion validaExistenciaProductos()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos que exista producto ligado al evento de la parada
            if (ServicioProducto.ObtieneTotalProductosDeParadaEvento(this._id_parada, (byte)this._id_tipo_evento) > 0)
            {
                //Validamos que exista otro evento ligado a la parada
                if (ParadaEvento.ObtieneTotalEventos(this._id_parada, this._id_tipo_evento) <= 1)
                {
                    //Establecemos Mensaje Error
                    resultado = new RetornoOperacion("Existen productos ligados al evento.");
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtenemos Total de Eventos Sin Iniciar
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada">Id de la Parada</param>
        /// <returns></returns>
        private static int obtieneTotalEventosSinIniciar(int id_servicio, int id_parada)
        {
            //Declaramos Resultados
            int Total = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 16, 0, id_servicio, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Total
                    Total = (from DataRow r in ds.Tables[0].Rows
                             select Convert.ToInt32(r["Total"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return Total;
        }

        #endregion

        #region Metodos publicos

        /// <summary>
        /// Método encargado de Insertar una Parada Evento
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_elemento_ubicacion">Id de elemento de patio, en caso de que este ligado.</param>
        /// <param name="id_tipo_evento">Tipo de Evento al que pertenece</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadaEvento(int id_servicio, int id_parada, int id_elemento_ubicacion,
                                                 int id_tipo_evento, int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_servicio, id_parada, obtieneSecuenciaEvento(id_parada), id_elemento_ubicacion, EstatusParadaEvento.Registrado,
                                 id_tipo_evento, null, null, 0, null, 0, 0, id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de Insertar una Parada Evento
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_elemento_ubicacion">Id de elemento de patio, en caso de que este ligado.</param>
        /// <param name="id_tipo_evento">Tipo de Evento al que pertenece</param>
        /// <param name="cita_evento">Cita Evento</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadaEvento(int id_servicio, int id_parada, int id_elemento_ubicacion,
                                                 int id_tipo_evento,DateTime cita_evento, int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_servicio, id_parada, obtieneSecuenciaEvento(id_parada), id_elemento_ubicacion, EstatusParadaEvento.Registrado,
                                 id_tipo_evento, Fecha.ConvierteDateTimeObjeto(cita_evento), null, 0, null, 0, 0, id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }
        /// <summary>
        /// Método encargado de Insertar una Parada Evento
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_elemento_ubicacion">Id de elemento de patio, en caso de que este ligado.</param>
        /// <param name="id_tipo_evento">Tipo de Evento al que pertenece</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadaEvento(int id_servicio, decimal secuencia, EstatusParadaEvento estatus, int id_parada, int id_elemento_ubicacion,
                                             int id_tipo_evento, int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_servicio, id_parada, secuencia, id_elemento_ubicacion, estatus,
                                 id_tipo_evento, null, null, 0, null, 0, 0, id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de Insertar una Parada Evento en el Módulo de Despacho
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="TotalEventos">Total Eventos</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadaEventoEnDespacho(int id_servicio, int id_parada,
                                                  int TotalEventos, int id_usuario)
        {
            //Declaramos tipo de evento default para uso de paradas de servicio.
            int id_tipo_evento = 4;
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Servicio
            using (Servicio objServicio = new Servicio(id_servicio))
            {
                /*/Validamos Estatus del Servicio
                if (objServicio.estatus == Servicio.Estatus.Documentado || objServicio.estatus == Servicio.Estatus.Iniciado)
                {//*/
                //Instanciamos Parada
                using (Parada objParada = new Parada(id_parada))
                {
                    /*/Validamos Estatus
                    if ((Parada.EstatusParada)objParada.id_estatus_parada != Parada.EstatusParada.Terminado)
                    {//*/
                    //Validamos Total de registros actual vs BD
                    if (TotalEventos == ObtieneTotalEventos(id_parada))
                    {
                        //En casó de ser parada Operativa
                        if (objParada.Tipo == Parada.TipoParada.Operativa)
                        {
                            //Obtenemos el Tipo de Evento actualmente Dedinido a la Parada
                            id_tipo_evento = obtieneTipoEvento(id_parada);

                            //Validamos Si el Tipo de Evento es Carga
                            if (id_tipo_evento == 1)
                            {
                                //Asignamos Nuevo Evento Descarga
                                id_tipo_evento = 2;
                            }
                            else
                            {
                                //Asignamos Nuevo Evento Carga
                                id_tipo_evento = 1;
                            }
                        }

                        //Inicializando arreglo de parámetros
                        object[] param = {1, 0, id_servicio, id_parada, obtieneSecuenciaEvento(id_parada), 0, EstatusParadaEvento.Registrado,
                                 id_tipo_evento, null, null, 0, null, 0, 0, id_usuario, true, null, "", ""};

                        //Realizando la actualización
                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                    }//Fin Validación Total Eventos
                    else
                    {
                        //Establecemos mensaje Error
                        resultado = new RetornoOperacion("No se puede agregar el evento, ya que el total de los mismos se ha modificado desde la última vez que fueron consultados.");
                    }
                    /*}
                    else
                    {
                        //Mostramos Mensaje
                        resultado = new RetornoOperacion("El estatus de la parada no permite su edición.");
                    }//*/
                }
                /*}
                else
                {
                    //Establecemos Resultados
                    resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                }//*/
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Insertar una Parada Evento en el Módulo de Servicio
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="TotalEventos">Total Eventos</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadaEventoEnServicio(int id_servicio, int id_parada,
                                                  int TotalEventos, int id_usuario)
        {
            //Declaramos Variable
            int id_tipo_evento = 0;
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Servicio
            using (Servicio objServicio = new Servicio(id_servicio))
            {
                /*/Validamos Estatus del Servicio
                if (objServicio.estatus == Servicio.Estatus.Documentado)
                {//*/
                //Instanciamos Parada
                using (Parada objParada = new Parada(id_parada))
                {
                    /*/Validamos Estatus
                    if ((Parada.EstatusParada)objParada.id_estatus_parada != Parada.EstatusParada.Terminado)
                    {//*/
                    //Validamos Total de registros actual vs BD
                    if (TotalEventos == ObtieneTotalEventos(id_parada))
                    {
                        //Obtenemos el Tipo de Evento actualmente Dedinido a la Parada
                        id_tipo_evento = obtieneTipoEvento(id_parada);

                        //Validamos Si el Tipo de Evento es Carga
                        if (id_tipo_evento == 1)
                        {
                            //Asignamos Nuevo Evento Descarga
                            id_tipo_evento = 2;
                        }
                        else
                        {
                            //Asignamos Nuevo Evento Carga
                            id_tipo_evento = 1;
                        }

                        //Inicializando arreglo de parámetros
                        object[] param = {1, 0, id_servicio, id_parada, obtieneSecuenciaEvento(id_parada), 0, EstatusParadaEvento.Registrado,
                                 id_tipo_evento, null, null, 0, null, 0, 0, id_usuario, true, null, "", ""};

                        //Realizando la actualización
                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                    }//Fin Validación Total Eventos
                    else
                    {
                        //Establecemos mensaje Error
                        resultado = new RetornoOperacion("No se puede eliminar el evento ya que fue modificado desde la última vez que fue consultado.");
                    }
                    /*}
                    else
                    {
                        //Mostramos Mensaje
                        resultado = new RetornoOperacion("El estatus de la parada no permite su edición.");
                    }//*/
                }
                /*}
                else
                {
                    //Establecemos Resultados
                    resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                }//*/
            }
            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Insertar una Parada Evento en el Módulo de Despacho
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_tipo_evento">Id de Tipo de Evento</param>
        /// <param name="TotalEventos">Total Eventos</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadaEventoEnDocumentacion(int id_servicio, int id_parada, int id_tipo_evento,
                                                  int TotalEventos, int id_usuario)
        {

            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Servicio
            using (Servicio objServicio = new Servicio(id_servicio))
            {
                /*/Validamos Estatus del Servicio
                if (objServicio.estatus == Servicio.Estatus.Documentado || objServicio.estatus == Servicio.Estatus.Iniciado)
                {//*/
                //Instanciamos Parada
                using (Parada objParada = new Parada(id_parada))
                {
                    /*/Validamos Estatus
                    if ((Parada.EstatusParada)objParada.id_estatus_parada != Parada.EstatusParada.Terminado)
                    {//*/
                    //Validamos Total de registros actual vs BD
                    if (TotalEventos == ObtieneTotalEventos(id_parada))
                    {
                        //Inicializando arreglo de parámetros
                        object[] param = {1, 0, id_servicio, id_parada, obtieneSecuenciaEvento(id_parada), 0, EstatusParadaEvento.Registrado,
                                 id_tipo_evento, null, null, 0, null, 0, 0, id_usuario, true, null, "", ""};

                        //Realizando la actualización
                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                    }//Fin Validación Total Eventos
                    else
                    {
                        //Establecemos mensaje Error
                        resultado = new RetornoOperacion("No se puede agregar el evento, ya que el total de los mismos se ha modificado desde la última vez que fueron consultados.");
                    }
                    /*}
                    else
                    {
                        //Mostramos Mensaje
                        resultado = new RetornoOperacion("El estatus de la parada no permite su edición.");
                    }//*/
                }
                /*}
                else
                {
                    //Establecemos Resultados
                    resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                }//*/
            }
            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Edita una registró parada evento 
        /// </summary>
        /// <param name="id_servicio">Id del servicio al que pertence</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="secuencia_evento_parada">Secuencia del Evento</param>
        /// <param name="id_elemento_ubicacion">Id de elemento de patio, en caso de que este ligado.</param>
        /// <param name="id_tipo_evento">Tipo de Evento al que pertenece</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParadaEvento(int id_servicio, int id_parada, decimal secuencia_evento_parada, int id_elemento_ubicacion,
                                                 int id_tipo_evento, int id_usuario)
        {

            //Actualizamos Valores restantes
            return this.editaParadaEvento(id_servicio, id_parada, secuencia_evento_parada, id_elemento_ubicacion, (EstatusParadaEvento)this._id_estatus, id_tipo_evento, this._cita_evento,
                                          this._inicio_evento, (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, this._fin_evento, (TipoActualizacionFin)this._id_tipo_actualizacion_fin,
                                          this._id_motivo_retraso_evento, id_usuario, this._habilitar);

        }

        /// <summary>
        /// Actualizamos las fechas de los eventos a Null
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EditaParadaEventoFechasParaReversa(int id_parada, TipoActualizacionInicio tipo_actualizacion_inicio, TipoActualizacionFin tipo_actualizacion_fin, int id_usuario)
        {
            //Establecemos Mensaje Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Eventos
            using (DataTable mitEventos = CargaEventos(id_parada))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitEventos))
                {
                    //Recorremos cada uno de los eventos
                    foreach (DataRow r in mitEventos.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Evento
                            using (ParadaEvento objParadaEvento = new ParadaEvento(r.Field<int>("Id")))
                            {
                                //Actualizamos Fechas y Estatus
                                resultado = objParadaEvento.editaParadaEvento(objParadaEvento.id_servicio, objParadaEvento.id_parada, objParadaEvento.secuencia_evento_parada,
                                                                            objParadaEvento.id_elemento_ubicacion, EstatusParadaEvento.Registrado, objParadaEvento.id_tipo_evento,
                                                                            objParadaEvento.cita_evento, DateTime.MinValue, tipo_actualizacion_inicio, DateTime.MinValue,
                                                                            tipo_actualizacion_fin, objParadaEvento.id_motivo_retraso_evento, id_usuario, objParadaEvento.habilitar);
                            }
                        }
                    }
                }
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Actualizamos las fechas de los eventos a Null
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EditaParadaEventosParaReversa(int id_parada, int id_usuario)
        {
            //Establecemos Mensaje Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Eventos
            using (DataTable mitEventos = CargaEventos(id_parada))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitEventos))
                {
                    //Recorremos cada uno de los eventos
                    foreach (DataRow r in mitEventos.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Evento
                            using (ParadaEvento objParadaEvento = new ParadaEvento(r.Field<int>("Id")))
                            {
                                //Actualizamos Fechas y Estatus
                                resultado = objParadaEvento.editaParadaEvento(objParadaEvento.id_servicio, objParadaEvento.id_parada, objParadaEvento.secuencia_evento_parada,
                                            objParadaEvento.id_elemento_ubicacion, EstatusParadaEvento.Registrado, objParadaEvento.id_tipo_evento, objParadaEvento.cita_evento,
                                            DateTime.MinValue, TipoActualizacionInicio.SinActualizar, DateTime.MinValue, TipoActualizacionFin.SinActualizar,
                                            objParadaEvento.id_motivo_retraso_evento, id_usuario, objParadaEvento.habilitar);
                            }
                        }
                    }
                }
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Edita las Fechas de los Eventos y el Tipo de evento en el Módulo de Despacho.
        /// </summary>
        /// <param name="id_tipo_evento">Tipo Evento (Paradas operativas: Carga, Descarga / Paradas de servicio: mantenimiento)</param>
        /// <param name="inicio_evento">Fecha de Inicio del Evento</param>
        /// <param name="tipo_actualizacion_inicio">Tipo de Actualización Inicio (Manual,GPS)</param>
        /// <param name="fin_evento">Fecha Fin del Evento</param>
        /// <param name="tipo_actualizacion_fin">Tipo Sctualización Fin (Manual, GPS)</param>
        /// <param name="id_motivo_retraso_evento">Motivo de Retraso</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParadaEventoEnDespacho(int id_tipo_evento, DateTime inicio_evento, TipoActualizacionInicio tipo_actualizacion_inicio, DateTime fin_evento,
                                                       TipoActualizacionFin tipo_actualizacion_fin, byte id_motivo_retraso_evento, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Si la parada se encuentrá terminada no se actualiza el tipo de evento
            int idTipoEvento = this._id_tipo_evento;

            //Instanciamos Parada
            using (Parada objParada = new Parada(this._id_parada))
            {
                //Validamos estus de la parada
                if (objParada.Estatus != Parada.EstatusParada.Registrado)
                {
                    //Validamo existencia de Fecha de Inicio en caso  tener de Fecha Fin
                    if ((!Fecha.EsFechaMinima(inicio_evento) && Fecha.EsFechaMinima(fin_evento))
                        || (!Fecha.EsFechaMinima(inicio_evento) && !Fecha.EsFechaMinima(fin_evento))
                        || (Fecha.EsFechaMinima(inicio_evento) && Fecha.EsFechaMinima(fin_evento)))
                    {
                        //Validamos estatus de la Parada
                        if (objParada.Estatus == Parada.EstatusParada.Iniciado)
                            //Asignamo tipo de evento de acuerdo al que se desea actualizar
                            idTipoEvento = id_tipo_evento;

                        //Si el evento está inciado
                        if (this.Estatus == EstatusParadaEvento.Iniciado)
                        {
                            //Validamos existencia de fecha de Inicio si se ha expresado fecha de fin
                            if (Fecha.EsFechaMinima(inicio_evento) && !Fecha.EsFechaMinima(fin_evento))
                                //Mostramos Mensaje Error
                                resultado = new RetornoOperacion("La fecha de Inicio es obligatoria.");
                        }
                        //Si está terminado y la parada está terminada
                        else if (this.Estatus == EstatusParadaEvento.Terminado && objParada.Estatus == Parada.EstatusParada.Terminado)
                        {
                            //Validamos Existencia de Fecha Fin
                            if (Fecha.EsFechaMinima(inicio_evento) || Fecha.EsFechaMinima(fin_evento))
                                //Mostramos Mensaje
                                resultado = new RetornoOperacion("Las fechas de inicio y fin son obligatorias.");
                        }

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos Fecha Fin evento, si no hay fecha de fin
                            if (Fecha.EsFechaMinima(fin_evento))
                            {
                                //Si hay fecha de inicio
                                if (!Fecha.EsFechaMinima(inicio_evento))
                                    //Validamos Rango de la Fecha Inicio Evento, Fecha Fin Evento, Fecha LLegada Parada
                                    resultado = Fecha.ValidaFechaEnRango(objParada.fecha_llegada, inicio_evento, fin_evento, "La fecha inicio del evento '{1}' debe ser un valor entre la fecha de inicio del servicio ó llegada a la parada '{0}' y la fecha de fin de evento '{2}'");

                                //Si no hay problemas
                                if (resultado.OperacionExitosa)
                                {
                                    //Si el Estatus es Registrado o Terminado y hay fecha de inicio
                                    if ((this.Estatus == EstatusParadaEvento.Registrado || this.Estatus == EstatusParadaEvento.Terminado) && !Fecha.EsFechaMinima(inicio_evento))
                                        //Actualizamos Estatus a  Iniciado
                                        resultado = IniciaParadaEvento(idTipoEvento, inicio_evento, tipo_actualizacion_inicio, id_usuario);
                                    //Si el estatus es Iniciado o Terminado y no hay fecha de inicio
                                    else if ((this.Estatus == EstatusParadaEvento.Iniciado || this.Estatus == EstatusParadaEvento.Terminado) && Fecha.EsFechaMinima(inicio_evento))
                                        //Regresando evento a Registrado
                                        resultado = ActualizaEstatusRegistradoParadaEvento(idTipoEvento, id_usuario);
                                    else
                                        //Editamos Evento
                                        resultado = editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                                                   this.Estatus, idTipoEvento, this._cita_evento, inicio_evento, tipo_actualizacion_inicio,
                                                                   fin_evento, tipo_actualizacion_fin, id_motivo_retraso_evento, id_usuario, this._habilitar);
                                }
                            }
                            //Si hay fecha de fin de evento
                            else
                            {
                                //Validamos Rango de Fechas vs Fecha de Salida de la Parada
                                resultado = Fecha.ValidaFechaEnRango(inicio_evento, fin_evento, objParada.fecha_salida, "La fecha fin del evento '{1}' debe ser un valor entre la fecha de inicio de evento '{0}' y la fecha de salida de la parada ó término del servicio '{2}'.");

                                //validamos Rango
                                if (resultado.OperacionExitosa)
                                {
                                    //Editamos Evento
                                    resultado = editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                               (EstatusParadaEvento)EstatusParadaEvento.Terminado, idTipoEvento, this._cita_evento, inicio_evento, tipo_actualizacion_inicio, fin_evento, tipo_actualizacion_fin,
                                                id_motivo_retraso_evento, id_usuario, this._habilitar);
                                }
                            }
                        }
                    }
                    else
                        //Mostramos Mensaje
                        resultado = new RetornoOperacion("La fecha de inicio es obligatoria");
                }
                //Si la parada está en estatus registrado
                else
                {
                    //Editamos sólo el tipo de evento y motivo de retraso
                    resultado = editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                               this.Estatus, id_tipo_evento, this._cita_evento, this._inicio_evento, (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio,
                                               this._fin_evento, (TipoActualizacionFin)this._id_tipo_actualizacion_fin, id_motivo_retraso_evento, id_usuario, this._habilitar);

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        resultado = new RetornoOperacion(resultado.IdRegistro, "La parada actual se encuentra en estatus 'Registrado', sólo se actualizarón los datos: Tipo de Evento, Cita y Motivo de Retraso.", resultado.OperacionExitosa);
                }
                //Enviamos Correo, si no existe Error
                if (resultado.OperacionExitosa)
                {
                    //Enviamos E-Mail
                    enviaEmailNotificacion(objParada, id_tipo_evento, inicio_evento, fin_evento);
                }
            }

           

            //Devolvemos Reultado
            return resultado;
        }


        /// <summary>
        /// Enviamos E-Mail de Notificación de Evento
        /// </summary>
        /// <param name="objParada"></param>
        /// <param name="id_evento"></param>
        /// <param name="id_tipo_evento"></param>
        /// <param name="inicio_evento"></param>
        /// <param name="fin_evento"></param>
        private void enviaEmailNotificacion(Parada  objParada, int id_tipo_evento, DateTime inicio_evento, DateTime fin_evento)
        {
            //Instanciamos Servicio
            using (Documentacion.Servicio objServicio = new Servicio(objParada.id_servicio))
            {
                //Intanciamos Ubicacion
                using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(objParada.id_ubicacion))
                {
                    //Instanciamos Compañia Emisor
                    using (CompaniaEmisorReceptor objCompaniaEmisor = new CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                    {
                        //Obtenemos Informacion de los Eventos
                        using (DataTable mitDatos = ParadaEvento.CargaEventosEMail(objParada.id_parada, this._id_evento))
                        {
                            //Validamos si la Fecha de Inicio no es Minima
                            if (Fecha.EsFechaMinima(this._inicio_evento) && !Fecha.EsFechaMinima(inicio_evento))
                            {
                                //Deacuerdo al Tipo de Evento
                                switch (id_tipo_evento.ToString())
                                {
                                    //Carga
                                    case "1":

                                        //Enviamos Notificación
                                        SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor,"NOTIFICA "+ objCompaniaEmisor.nombre_corto +": INICIO CARGA EN " + objUbicacion.descripcion,
                                            objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 3, "Notifica " + objCompaniaEmisor.nombre_corto + ": Inicio de Carga en " + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                         "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(),1, objServicio.id_servicio, "Referencia de Viaje", "No. viaje") +
                                       "/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación") +
                                         SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación")
                                         + "/Carta Porte " + objServicio.porte,
                                          "UBICACIÓN ACTUAL: "+objUbicacion.descripcion + " <BR> Cita a las: " + Fecha.ConvierteDateTimeString(objParada.cita_parada, "dd/MM/yyy HH:mm") + " <BR>Llegada a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_llegada, "dd/MM/yyy HH:mm") + " <BR>Salida a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_salida, "dd/MM/yyyy HH:mm"),
                                         "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "RealizadoEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                         );
                                        break;

                                    //Descarga
                                    case "2":

                                        //Enviamos Notificación
                                        SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor,"NOTIFICA " + objCompaniaEmisor.nombre_corto+": INICIO DESCARGA EN " + objUbicacion.descripcion,
                                         objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 5, "Notifica " + objCompaniaEmisor.nombre_corto + ": Inicio de Descarga en " + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                         "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "No. viaje") +
                                       "/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación") +
                                         SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación")
                                         + "/Carta Porte " +objServicio.porte,
                                          "UBICACIÓN ACTUAL: "+objUbicacion.descripcion + " <BR> Cita a las: " + Fecha.ConvierteDateTimeString(objParada.cita_parada, "dd/MM/yyy HH:mm") + " <BR>Llegada a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_llegada, "dd/MM/yyy HH:mm") + " <BR>Salida a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_salida, "dd/MM/yyyy HH:mm"),
                                         "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "RealizadoEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                         );
                                        break;
                                    //Descarga Parcial
                                    case "3":

                                        //Enviamos Notificación
                                        SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor,"NOTIFICA "+ objCompaniaEmisor.nombre_corto+": INICIO DESCARGA EN " + objUbicacion.descripcion,
                                         objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 5, "Notifica "+ objCompaniaEmisor.nombre_corto + ": Inicio de Descarga en" + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                         "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1,objServicio.id_servicio, "Referencia de Viaje", "No. viaje") +
                                       "/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio,  "Referencia de Viaje", "Confirmación") +
                                         SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación")
                                         + "/Carta Porte " + objServicio.porte,
                                          "UBICACIÓN ACTUAL: "+objUbicacion.descripcion + " <BR> Cita a las: " + Fecha.ConvierteDateTimeString(objParada.cita_parada, "dd/MM/yyy HH:mm") + " <BR>Llegada a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_llegada, "dd/MM/yyy HH:mm") + " <BR>Salida a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_salida, "dd/MM/yyyy HH:mm"),
                                         "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "RealizadoEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                         );
                                        break;
                                }
                            }
                            //Validamos Si la Fecha de Fin no es Minima
                            if (Fecha.EsFechaMinima(this._fin_evento) && !Fecha.EsFechaMinima(fin_evento))
                            {
                                //Deacuerdo al Tipo de Evento
                                switch (id_tipo_evento.ToString())
                                {
                                    //Carga
                                    case "1":

                                        //Enviamos Notificación
                                        SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor,"NOTIFICA" + objCompaniaEmisor.nombre_corto+ ": TERMINÓ CARGA EN " + objUbicacion.descripcion,
                                         objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 4, "Notifica "+ objCompaniaEmisor.nombre_corto +": Terminó de Carga en " + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                         "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio,  "Referencia de Viaje", "No. viaje") +
                                       "/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación") +
                                         SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio,  "Referencia de Viaje", "Confirmación")
                                         + "/Carta Porte " + objServicio.porte,
                                          "UBICACIÓN ACTUAL: "+objUbicacion.descripcion + " <BR> Cita a las: " + Fecha.ConvierteDateTimeString(objParada.cita_parada, "dd/MM/yyy HH:mm") + " <BR>Llegada a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_llegada, "dd/MM/yyy HH:mm") + " <BR>Salida a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_salida, "dd/MM/yyyy HH:mm"),
                                         "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "RealizadoEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                         );
                                        break;

                                    //Descarga
                                    case "2":

                                        //Enviamos Notificación
                                        SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor,"NOTIFICA " + objCompaniaEmisor.nombre_corto+ ": TERMINÓ DESCARGA EN " + objUbicacion.descripcion,
                                         objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 6, "Notifica " + objCompaniaEmisor.nombre_corto + ": Terminó de Descarga en " + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                         "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "No. viaje") +
                                       "/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación") +
                                         SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación")
                                         + "/Carta Porte " + objServicio.porte,
                                         "UBICACIÓN ACTUAL: "+objUbicacion.descripcion + " <BR> Cita a las: " + Fecha.ConvierteDateTimeString(objParada.cita_parada, "dd/MM/yyy HH:mm") + " <BR>Llegada a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_llegada, "dd/MM/yyy HH:mm") + " <BR>Salida a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_salida, "dd/MM/yyyy HH:mm"),
                                         "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "RealizadoEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                         );
                                        break;

                                    //Descarga Parcial
                                    case "3":

                                        //Enviamos Notificación
                                        SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor, "NOTIFICA "+ objCompaniaEmisor.nombre_corto +": TERMINÓ DESCARGA EN " + objUbicacion.descripcion,
                                         objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 6, "Notifica"+ objCompaniaEmisor.nombre_corto+": Terminó de Descarga en  " + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                         "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1,objServicio.id_servicio, "Referencia de Viaje", "No. viaje") +
                                       "/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio,  "Referencia de Viaje", "Confirmación") +
                                         SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio,  "Referencia de Viaje", "Confirmación")
                                         + "/Carta Porte " + objServicio.porte,
                                         "UBICACIÓN ACTUAL: "+objUbicacion.descripcion + " <BR> Cita a las: " + Fecha.ConvierteDateTimeString(objParada.cita_parada, "dd/MM/yyy HH:mm") + " <BR>Llegada a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_llegada, "dd/MM/yyy HH:mm") + " <BR>Salida a las: " + Fecha.ConvierteDateTimeString(objParada.fecha_salida, "dd/MM/yyyy HH:mm"),
                                         "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "RealizadoEMail.png")),
                                         Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                         );
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Edita una registró  evento  en el Módulo de Documentación
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParadaEventoEnServicio(int id_usuario)
        {
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos variable para almacenar evento
            int id_tipo_evento = 0;

            ///Instanciamos Servicio
            using (Servicio objServicio = new Servicio(this._id_servicio))
            {
                /*/Validamos Estatus del Servicio
                if (objServicio.estatus == Servicio.Estatus.Documentado)
                {//*/
                //Instanciamos parada
                using (Parada objParada = new Parada(this._id_parada))
                {
                    /*/Validamos Estatus de parada no se encuentre terminada.
                    if ((Parada.EstatusParada)objParada.id_estatus_parada != Parada.EstatusParada.Terminado)
                    {//*/
                    //Validamos Si el Tipo de Evento es Carga
                    if (this._id_tipo_evento == 1)
                    {
                        //Asignamos Nuevo Evento Descarga
                        id_tipo_evento = 2;
                    }
                    else
                    {
                        //Si el Tipo es Descarga
                        if (this._id_tipo_evento == 2)
                        {
                            //Asignamos Nuevo Evento Descarga Parcial
                            id_tipo_evento = 3;
                        }
                        else if(this._id_tipo_evento == 3)
                        {
                            //Asignamos Nuevo Evento Descarga Parcial
                            id_tipo_evento = 77;
                        }
                        else if (this._id_tipo_evento == 77)
                        {
                            //Asignamos Nuevo Evento Descarga Parcial
                            id_tipo_evento = 78;
                        }
                        else if (this._id_tipo_evento == 78)
                        {
                            //Asignamos Nuevo Evento Descarga Parcial
                            id_tipo_evento = 79;
                        }
                        else
                        {
                            //Asignamos Nuevo Evento Carga
                            id_tipo_evento = 1;
                        }
                    }

                    //Validamos existencia de productos ligado al tipo de evento (Carga/Descarga)
                    resultado = validaExistenciaProductos();
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Actualizamos Valores restantes
                        resultado = this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion, (EstatusParadaEvento)this._id_estatus, id_tipo_evento,
                                                        this._cita_evento, this._inicio_evento, (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, this._fin_evento,
                                                        (TipoActualizacionFin)this._id_tipo_actualizacion_fin, this._id_motivo_retraso_evento, id_usuario, this._habilitar);

                    }
                    /*}
                    else
                    {
                        //Establecemos mensaje error
                        resultado = new RetornoOperacion("El estatus de la parada no permite su edición.");
                    }//*/
                }
                /*}
                else
                {
                    //Establecemos Error
                    resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                }//*/
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Inicia un evento
        /// </summary>
        /// <param name="inicio_evento">Fecha de Inicio del Evento</param>
        /// <param name="tipo_actualizacion_inicio">Medio  que se utilizó  para actualizar el Inicio del Evento</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion IniciaParadaEvento(int id_tipo_evento, DateTime inicio_evento, TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Actualizando información de evento de la parada
            resultado = this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                               EstatusParadaEvento.Iniciado, id_tipo_evento, this._cita_evento, inicio_evento, tipo_actualizacion_inicio,
                                               DateTime.MinValue, TipoActualizacionFin.SinActualizar, this._id_motivo_retraso_evento, id_usuario,
                                               this._habilitar);

            return resultado;
        }
        /// <summary>
        /// Devuelve el estatus del evento a Registrado
        /// </summary>
        /// <param name="id_tipo_evento">Id de Tipo de Evento</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusRegistradoParadaEvento(int id_tipo_evento, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Actualizando información de evento de la parada
            resultado = this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                               EstatusParadaEvento.Registrado, id_tipo_evento, this._cita_evento, DateTime.MinValue, TipoActualizacionInicio.SinActualizar,
                                               DateTime.MinValue, TipoActualizacionFin.SinActualizar, this._id_motivo_retraso_evento, id_usuario,
                                               this._habilitar);
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Actualizamos Estatus del Evento
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatus(EstatusParadaEvento estatus, int id_usuario)
        {

            //Editamos Evento
            return this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                                     estatus, this._id_tipo_evento, this._cita_evento, this._inicio_evento,
                                                     (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, this._fin_evento,
                                                     (TipoActualizacionFin)this._id_tipo_actualizacion_fin, this._id_motivo_retraso_evento,
                                                     id_usuario, this._habilitar);
        }

        /// <summary>
        /// Termina un Evento
        /// </summary>
        /// <param name="fin_evento">Fecha de Inicio del Evento</param>
        /// <param name="tipo_actualizacion_fin">Medio  que se utilizó  para actualizar el Fin del Evento</param>
        /// <param name="id_motivo_retraso_evento">En caso de que el evento se realizó Tarde, establece la causa</param>
        /// <param name="fecha_salida_parada">Fecha Salida de la Parada</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaParadaEvento(DateTime fin_evento, TipoActualizacionFin tipo_actualizacion_fin, byte id_motivo_retraso_evento, DateTime fecha_salida_parada, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Fecha Fin evento vs Fecha Termino de la Parada
            resultado = Fecha.ValidaFechaEnRango(fin_evento, fecha_salida_parada, DateTime.MinValue, "La fecha de salida de la parada o término del servicio '{1}' debe ser mayor que la fecha de fin del evento " + this.secuencia_evento_parada.ToString());

            //Validamos Rengo de Fechas
            if (resultado.OperacionExitosa)
            {
                resultado = this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                                   EstatusParadaEvento.Terminado, this._id_tipo_evento, this._cita_evento, this._inicio_evento,
                                                   (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, fin_evento, tipo_actualizacion_fin,
                                                   id_motivo_retraso_evento, id_usuario, this._habilitar);
            }

            //Obtenemos Resultado
            return resultado;
        }


        /// <summary>
        /// Deshabilita un Registro Parada Evento en el Módulo de Servicio
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="TotalEventos">Total Eventos</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParadaEventoEnServicio(int id_usuario, int TotalEventos)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            int totaleventos = 0;
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio
                using (Servicio objServicio = new Servicio(this._id_servicio))
                {
                    /*/Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Documentado)
                    {//*/
                    //Asignamos Total de Eventos
                    totaleventos = ObtieneTotalEventos(this._id_parada);

                    //Validamos Total de registros actual vs BD
                    if (TotalEventos == totaleventos)
                    {
                        //Validamos que el Estatus de la Parada este Registrado
                        if ((EstatusParadaEvento)this._id_estatus == EstatusParadaEvento.Registrado)
                        {
                            //Validamos que exista mas de 1 evento
                            if (totaleventos > 1)
                            {
                                resultado = validaExistenciaProductos();
                                //Validamos existencia de Productos
                                if (resultado.OperacionExitosa)
                                {
                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Deshabilitamos Evento
                                        resultado = this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                                                          (EstatusParadaEvento)this._id_estatus, this._id_tipo_evento, this._cita_evento, this._inicio_evento,
                                                                            (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, this._fin_evento,
                                                                            (TipoActualizacionFin)this._id_tipo_actualizacion_fin, this._id_motivo_retraso_evento,
                                                                            id_usuario, false);


                                        //Validamos deshabilitación del Evento
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizamos las secuencia de los Eventos
                                            resultado = ActualizaSecuenciaEventos(this._id_parada, this._secuencia_evento_parada, id_usuario);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Establecemos Mensaje Error
                                resultado = new RetornoOperacion("Debe existir por lo menos un evento.");
                            }
                        }
                        else
                        {
                            resultado = new RetornoOperacion("El estatus del evento no permite su eliminación.");
                        }

                    }//Fin Validación Total Eventos
                    else
                    {
                        //Establecmeos Error
                        resultado = new RetornoOperacion("No se puede eliminar el evento ya que fue modificado desde la última vez que fue consultado.");
                    }
                    /*}
                    else
                    {
                        //Establecemos Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                    }//*/
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Asignando Id de evento deshabilitado
                    resultado = new RetornoOperacion(this._id_evento);
                    //Completamos Transacción
                    scope.Complete();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Deshabilita un Registro Parada Evento en el Módulo de Despacho
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="TotalEventos">Total Eventos</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParadaEventoEnDespacho(int id_usuario, int TotalEventos)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            int totaleventos = 0;
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio
                using (Servicio objServicio = new Servicio(this._id_servicio))
                {
                    /*/Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Iniciado)
                    {//*/
                    //Asignamos Total de Eventos
                    totaleventos = ObtieneTotalEventos(this._id_parada);

                    //Validamos Total de registros actual vs BD
                    if (TotalEventos == totaleventos)
                    {

                        //Validamos que el Estatus de la Parada este Registrado
                        if ((EstatusParadaEvento)this._id_estatus == EstatusParadaEvento.Registrado)
                        {
                            //Validamos que exista mas de 1 evento
                            if (totaleventos > 1)
                            {
                                //Deshabilitamos Evento
                                resultado = this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                                                  (EstatusParadaEvento)this._id_estatus, this._id_tipo_evento, this._cita_evento, this._inicio_evento,
                                                                  (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, this._fin_evento,
                                                                  (TipoActualizacionFin)this._id_tipo_actualizacion_fin, this._id_motivo_retraso_evento, id_usuario, false);

                                //Validamos deshabilitación del Evento
                                if (resultado.OperacionExitosa)
                                {
                                    //Actualizamos las secuencia de los Eventos
                                    resultado = ActualizaSecuenciaEventos(this._id_parada, this._secuencia_evento_parada, id_usuario);
                                }
                            }
                            else
                            {
                                //Establecemos Mensaje Error
                                resultado = new RetornoOperacion("Debe existir por lo menos un evento.");
                            }
                        }
                        else
                        {
                            resultado = new RetornoOperacion("El estatus del evento no permite su eliminación.");
                        }

                    }//Fin Validación Total Eventos
                    else
                    {
                        //Establecmeos Error
                        resultado = new RetornoOperacion("No se puede eliminar el evento ya que fue modificado desde la última vez que fue consultado.");
                    }
                    /*}
                    else
                    {
                        //Establecemos Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                    }//*/
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Asignando Id de evento deshabilitado
                    resultado = new RetornoOperacion(this._id_evento);
                    //Completamos Transacción
                    scope.Complete();
                }
            }
            return resultado;
        }
        /// <summary>
        /// Deshabilita un Registro Parada Evento
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParadaEvento(int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que el Estatus de la Parada este Registrado
            if ((EstatusParadaEvento)this._id_estatus == EstatusParadaEvento.Registrado)
            {


                //Deshabilitamos Evento
                resultado = this.editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion,
                                                    (EstatusParadaEvento)this._id_estatus, this._id_tipo_evento, this._cita_evento, this._inicio_evento,
                                                    (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, this._fin_evento,
                                                    (TipoActualizacionFin)this._id_tipo_actualizacion_fin, this._id_motivo_retraso_evento, id_usuario, false);

                //Validamos deshabilitación del Evento
                if (resultado.OperacionExitosa)
                {
                    //Actualizamos las secuencia de los Eventos
                    resultado = ActualizaSecuenciaEventos(this._id_parada, this._secuencia_evento_parada, id_usuario);
                }
            }
            else
            {
                resultado = new RetornoOperacion("El estatus del evento no permite su eliminación.");
            }
            return resultado;
        }
        /// <summary>
        /// Actualiza Secuencia de Eventos
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaSecuenciaEventos(int id_parada, decimal secuencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Realizando la consulta
            using (DataTable mit = CargaEventosParaActualizarSecuencias(id_parada, secuencia))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Recorremos cada una de las paradas
                    foreach (DataRow r in mit.Rows)
                    {
                        //Validamos actualización
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Parada
                            using (ParadaEvento objParadaEvento = new ParadaEvento(r.Field<int>("Id")))
                            {
                                resultado = objParadaEvento.EditaParadaEvento(objParadaEvento.id_servicio, objParadaEvento.id_parada, objParadaEvento.secuencia_evento_parada - 1,
                                            objParadaEvento.id_elemento_ubicacion, objParadaEvento.id_tipo_evento, id_usuario);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Carga Eventos ligando un Id de Parada ordenados por secuencia Asc
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="secuencia">Secuencia</param>
        /// <returns></returns>
        public static DataTable CargaEventosParaActualizarSecuencias(int id_parada, decimal secuencia)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 6, 0, 0, id_parada, secuencia, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Carga Eventos ligando un Id de Parada 
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static DataTable CargaEventos(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 7, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Carga Eventos ligando un Id de Parada 
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static DataTable CargaEventosParaVisualizacion(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 8, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Obtiene el Tipo de Evento actualmente definido a la parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        private static int obtieneTipoEvento(int id_parada)
        {
            //Declaramos Resultados
            int tipoEvento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 9, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Secuencia
                    tipoEvento = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["TipoEvento"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return tipoEvento;
        }

        /// <summary>
        ///  Obtiene Total Eventos registrados
        /// </summary>
        /// <param name="id_parada">Id de la Parada</param>
        /// <returns></returns>
        public static int ObtieneTotalEventos(int id_parada)
        {
            //Declaramos Resultados
            int Total = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 10, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Total
                    Total = (from DataRow r in ds.Tables[0].Rows
                             select Convert.ToInt32(r["Total"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return Total;
        }

        /// <summary>
        /// Obtiene el Primer Tipo de Evento actualmente definido a la parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static int ObtienerPrimerTipoEvento(int id_parada)
        {
            //Declaramos Resultados
            int tipoEvento = 1;

            //Inicializando arreglo de parámetros
            object[] param = { 9, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Secuencia
                    tipoEvento = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["TipoEvento"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return tipoEvento;
        }

        /// <summary>
        /// Carga Eventos ligando un Id de Parada  para actualización 
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static DataTable CargaEventosParaActualizacionFechas(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 12, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Carga los eventos ligando un Id de Parada para actualización de la Fecha de Salida de la Parada.
        /// </summary>
        /// <param name="id_parada"></param>
        /// <returns></returns>
        public static DataTable CargaEventosParaActualizacionParada(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 13, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Carga los tipos de Eventos ligado a una Parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="Carga">Evento de Carga</param>
        /// <param name="Descarga">Evento de Descarga</param>
        /// <param name="DescargaParcial">Evento de Descarga Parcial</param>
        public static void CargaTipoEventos(int id_parada, out bool Carga, out bool Descarga, out bool DescargaParcial)
        {
            //Declaramos variables
            Carga = Descarga = DescargaParcial = false;

            //Inicializamos arreglo de parametros
            object[] param = { 14, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };


            //Realizando carga de registros
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param).Tables[0])
            {

                Carga = (from DataRow r in mit.Rows
                         where Convert.ToByte(r["Tipo"]) == 1
                         select Convert.ToBoolean(r["Valor"])).FirstOrDefault();
                Descarga = (from DataRow r in mit.Rows
                            where Convert.ToByte(r["Tipo"]) == 2
                            select Convert.ToBoolean(r["Valor"])).FirstOrDefault();
                DescargaParcial = (from DataRow r in mit.Rows
                                   where Convert.ToByte(r["Tipo"]) == 3
                                   select Convert.ToBoolean(r["Valor"])).FirstOrDefault();

            }
        }
        /// <summary>
        /// Copia los eventos de carga, descarga desde a parada origen indicada hacia la parada destino solicitada
        /// </summary>
        /// <param name="id_servicio">Id de Servicio a donde se asocian los nuevos eventos</param>
        /// <param name="id_parada_origen">Id de Parada a Copiar</param>
        /// <param name="id_parada_destino">Id de Parada destino de la Copia</param>
        /// <param name="id_usuario">Id de Usuario que realiza la copia</param>
        /// <param name="cita_parada_destino">Cita de la Parada Destino</param>
        /// <returns></returns>
        public static RetornoOperacion CopiaEventosParadaServicio(int id_servicio, int id_parada_origen, int id_parada_destino, DateTime cita_parada_destino, int id_usuario)
        {
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_parada_destino);

            //Inicializando transacción
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando eventos ligados a la parda de origen
                using (DataTable tblEventos = ParadaEvento.CargaEventos(id_parada_origen))
                {
                    //Validando existencia de eventos de parada para copia
                    if (Validacion.ValidaOrigenDatos(tblEventos))
                    {
                        //Para cada uno de los eventos asociados
                        foreach (DataRow rEvento in tblEventos.Rows)
                        {
                            //Realizando copia de evento hacia nueva parada
                            resultado = ParadaEvento.InsertaParadaEvento(id_servicio, id_parada_destino, 0, Convert.ToInt32(rEvento["IdTipoEvento"]), cita_parada_destino, id_usuario);

                            //Si existe algún error
                            if (!resultado.OperacionExitosa)
                            {
                                //Personalizando error
                                resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar evento {0}; {1}", rEvento["Id"], resultado.Mensaje), resultado.OperacionExitosa);
                                //Se interrumpe el ciclo de copia de eventos
                                break;
                            }
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Se confirman cambios realziados
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene la Fecha Fin del Último Evento
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static DateTime ObtieneFechaFinUltimoEvento(int id_parada)
        {
            //Declaramos Resultados
            DateTime fechaFin = DateTime.MinValue;

            //Inicializando arreglo de parámetros
            object[] param = { 15, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Secuencia
                    int idEvento = (from DataRow r in ds.Tables[0].Rows
                                    select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
                    //Si el evento existe
                    if (idEvento != 0)
                    {
                        //Instanciamos Evento
                        using (ParadaEvento objEvento = new ParadaEvento(idEvento))
                        {
                            //Asignamos Fecha
                            fechaFin = objEvento.fin_evento;
                        }
                    }
                }
            }

            //Obtenemos Resultado
            return fechaFin;
        }

        /// <summary>
        /// Carga Eventos para su deshabilitación
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static DataTable CargaEventosParaDeshabilitacion(int id_parada, int id_usuario)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 17, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Valida eventos sin Iniciar para apertura de ventana modal
        /// </summary>
        /// <param name="id_servicio">Id Servcio</param>
        /// <param name="id_parada"> Id Parada</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaEventosSinIniciar(int id_servicio, int id_parada)
        {
            //Declaramos Variable Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            if (obtieneTotalEventosSinIniciar(id_servicio, id_parada) != 0)
            {
                //Establecemos Mensaje Error
                resultado = new RetornoOperacion("Existen eventos sin actualizar. ¿Desea actualizar dichos eventos?");
            }
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Deshabilita los eventos ligando un Id de Parada
        /// </summary>
        /// <param name="id_parada">Id parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaParadaEventos(int id_parada, int id_usuario)
        {
            //Declarams Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(id_parada);

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos eventos ligados a la parada
                using (DataTable mitEventos = CargaEventosParaDeshabilitacion(id_parada, id_usuario))
                {
                    //Validamos que existan eventos
                    if (Validacion.ValidaOrigenDatos(mitEventos))
                    {
                        // Recorremos cada uno de los eventos
                        foreach (DataRow r in mitEventos.Rows)
                        {
                            //Instanciamos Eventos
                            using (ParadaEvento objParadaEvento = new ParadaEvento(r.Field<int>("Id")))
                                //Deshabilitamos Evento
                                resultado = objParadaEvento.DeshabilitaParadaEvento(id_usuario);

                            //Si hay errores
                            if (!resultado.OperacionExitosa)
                                break;
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Terminando con éxito la transacción
                    scope.Complete();
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Validamos que el evento se encuentre activo.
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_tipo_evento">Id Tipo Evento</param>
        /// <returns></returns>
        public static bool ValidaExistenciaEvento(int id_parada, int id_tipo_evento)
        {
            //Declaramos objeto Resultado
            bool validaEvento = false;

            //Inicializando arreglo de parámetros
            object[] param = { 18, 0, 0, id_parada, 0, 0, 0, id_tipo_evento, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Validacion
                    validaEvento = (from DataRow r in ds.Tables[0].Rows
                                    select Convert.ToBoolean(r["ValidaEvento"])).FirstOrDefault();

                }
            }
            //Declaramos Objeto Resultado
            return validaEvento;
        }

        /// <summary>
        /// Validamos que el evento se encuentre activo.
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_tipo_evento">Id Tipo Evento</param>
        /// <returns></returns>
        public static int ObtieneTotalEventos(int id_parada, int id_tipo_evento)
        {
            //Declaramos objeto Resultado
            int totalEventos = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 19, 0, 0, id_parada, 0, 0, 0, id_tipo_evento, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Validacion
                    totalEventos = (from DataRow r in ds.Tables[0].Rows
                                    select Convert.ToInt32(r["TotalEventos"])).FirstOrDefault();

                }
            }
            //Declaramos Objeto Resultado
            return totalEventos;
        }
        /// <summary>
        /// Realiza la actualización de la fecha de cita del evento
        /// </summary>
        /// <param name="cita">Fecha de Cita de Inicio del Evento</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCitaEvento(DateTime cita, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando actualización de fecha de cita
            resultado = editaParadaEvento(this._id_servicio, this._id_parada, this._secuencia_evento_parada, this._id_elemento_ubicacion, 
                                this.Estatus, this._id_tipo_evento, cita, this._inicio_evento, this.TipoActualizacionInicio_, this._fin_evento, 
                                this.TipoActualizacionFin_, this._id_motivo_retraso_evento, id_usuario, this._habilitar);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Método  encargado de Actualizar los atributos de la Parada Evento
        /// </summary>
        /// <returns></returns>
        public bool ActualizaParadaEvento()
        {
            return this.cargaAtributosInstancia(this._id_evento);
        }

        /// <summary>
        /// Carga Eventos ligando un Id de Parada 
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_evento">Id Evento</param>
        /// <returns></returns>
        public static DataTable CargaEventosEMail(int id_parada, int id_evento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 20, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, id_evento, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Obtiene el Primer  Evento actualmente definido a la parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static int ObtienerPrimerEvento(int id_parada)
        {
            //Declaramos Resultados
            int Id = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 21, 0, 0, id_parada, 0, 0, 0, 0, null, null, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Secuencia
                    Id = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return Id;
        }

        #endregion
    }
}

