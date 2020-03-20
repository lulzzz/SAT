using Microsoft.SqlServer.Types;
using SAT_CL.ControlEvidencia;
using SAT_CL.Documentacion;
using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Implementa los método para la administración de Paradas.
    /// </summary>
    public partial class Parada : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Estatus de la parada
        /// </summary>
        public enum EstatusParada
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
        /// Enumera el Tipo de  parada
        /// </summary>
        public enum TipoParada
        {
            /// <summary>
            /// Operativa
            /// </summary>
            Operativa = 1,
            /// <summary>
            /// Servicio
            /// </summary>
            Servicio,

        }

        /// <summary>
        /// Enumera el Tipo de Actualización Llegada 
        /// </summary>
        public enum TipoActualizacionLlegada
        {
            /// <summary>
            /// Sin Actualizar
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
        /// Enumera el Tipo de Actualización Salida 
        /// </summary>
        public enum TipoActualizacionSalida
        {
            /// <summary>
            /// Sin Actualizar
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
        private static string _nombre_stored_procedure = "despacho.sp_parada_tp";


        private int _id_parada;
        /// <summary>
        /// Describe el Id de Parada
        /// </summary>
        public int id_parada
        {
            get { return _id_parada; }
        }
        private int _id_servicio;
        /// <summary>
        /// Describe el Id de Sevicio
        /// </summary>
        public int id_servicio
        {
            get { return _id_servicio; }
        }
        private decimal _secuencia_parada_servicio;
        /// <summary>
        /// Describe la secuencia de Parada de Servicio
        /// </summary>
        public decimal secuencia_parada_servicio
        {
            get { return _secuencia_parada_servicio; }
        }
        private byte _id_tipo_parada;
        /// <summary>
        /// Describe el Tipo Parad
        /// </summary>
        public byte id_tipo_parada
        {
            get { return _id_tipo_parada; }
        }
        private byte _id_estatus_parada;
        /// <summary>
        /// Describe el Estatus de Parada
        /// </summary>
        public byte id_estatus_parada
        {
            get { return _id_estatus_parada; }
        }
        private int _id_ubicacion;
        /// <summary>
        /// Describe el Id de la Ubicacion
        /// </summary>
        public int id_ubicacion
        {
            get { return _id_ubicacion; }
        }
        private SqlGeography _geo_ubicacion;
        /// <summary>
        /// Describe la Geo Ubicacion
        /// </summary>
        public SqlGeography geo_ubicacion
        {
            get { return _geo_ubicacion; }
        }
        private string _descripcion;
        /// <summary>
        /// Describe la descripcion
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private DateTime _cita_parada;
        /// <summary>
        /// Describe la cita de Parda
        /// </summary>
        public DateTime cita_parada
        {
            get { return _cita_parada; }
        }
        private DateTime _fecha_llegada;
        /// <summary>
        /// Describe la Fecha de Llegada
        /// </summary>
        public DateTime fecha_llegada
        {
            get { return _fecha_llegada; }
        }
        private byte _id_tipo_actualizacion_llegada;
        /// <summary>
        /// Describe el Tipo de actualización de Llegada
        /// </summary>
        public byte id_tipo_actualizacion_llegada
        {
            get { return _id_tipo_actualizacion_llegada; }
        }
        private byte _id_razon_llegada_tarde;
        /// <summary>
        /// Describe el Id de  razon de llegada Tarde
        /// </summary>
        public byte id_razon_llegada_tarde
        {
            get { return _id_razon_llegada_tarde; }
        }
        private DateTime _fecha_salida;
        /// <summary>
        /// Describe la fecha de Salida
        /// </summary>
        public DateTime fecha_salida
        {
            get { return _fecha_salida; }
        }
        private byte _id_tipo_actualizacion_salida;
        /// <summary>
        /// Describe el tipo de actualizaciíon de Salida
        /// </summary>
        public byte id_tipo_actualizacion_salida
        {
            get { return _id_tipo_actualizacion_salida; }
        }
        private byte _id_razon_salida_tarde;
        /// <summary>
        /// Describe el Id de  razon de Salida Tarde
        /// </summary>
        public byte id_razon_salida_tarde
        {
            get { return _id_razon_salida_tarde; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        private byte[] _row_version;
        /// <summary>
        /// Describe la versión
        /// </summary>
        public byte[] row_version
        {
            get { return _row_version; }
        }

        /// <summary>
        /// Enumera el Estatus Parada
        /// </summary>
        public EstatusParada Estatus
        {
            get { return (EstatusParada)_id_estatus_parada; }
        }

        /// <summary>
        /// Enumera el Tipo Parada
        /// </summary>
        public TipoParada Tipo
        {
            get { return (TipoParada)_id_tipo_parada; }
        }

        /// <summary>
        /// Enumera el Tipo Actualizacion Inicio
        /// </summary>
        public TipoActualizacionLlegada TipoActualizacionLlegada_
        {
            get { return (TipoActualizacionLlegada)_id_tipo_actualizacion_llegada; }
        }


        /// <summary>
        ///  Enumera el Tipo Actualizacion Salida
        /// </summary>
        public TipoActualizacionSalida TipoActualizacionSalida_
        {
            get { return (TipoActualizacionSalida)_id_tipo_actualizacion_salida; }
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Parada()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public Parada()
        {

        }

        /// <summary>
        /// Genera una Instancia de Tipo Parada
        /// </summary>
        /// <param name="id_parada"></param>
        public Parada(int id_parada)
        {
            cargaAtributosInstancia(id_parada);
        }

        /// <summary>
        /// Genera una Instancia de Tipo Parada ligando una secuencia y un Id de Servicio
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="id_servicio"></param>
        public Parada(decimal secuencia, int id_servicio)
        {
            cargaAtributosInstancia(secuencia, id_servicio);
        }

        /// <summary>
        /// Genera una Instancia Parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        private bool cargaAtributosInstancia(int id_parada)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_parada, 0, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_parada = Convert.ToInt32(r["Id"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _secuencia_parada_servicio = Convert.ToDecimal(r["SecuenciaParadaServicio"]);
                        _id_tipo_parada = Convert.ToByte(r["IdTipoParada"]);
                        _id_estatus_parada = Convert.ToByte(r["IdEstatusParada"]);
                        _id_ubicacion = Convert.ToInt32(r["IdUbicacion"]);
                        _geo_ubicacion = (SqlGeography)r["GeoUbicacion"];
                        _descripcion = r["Descripcion"].ToString();
                        DateTime.TryParse(r["CitaParada"].ToString(), out _cita_parada);
                        DateTime.TryParse(r["FechaLLegada"].ToString(), out _fecha_llegada);
                        _id_tipo_actualizacion_llegada = Convert.ToByte(r["IdTipoActualizacionLlegada"]);
                        _id_razon_llegada_tarde = Convert.ToByte(r["IdRazonLLegadaTarde"]);
                        DateTime.TryParse(r["FechaSalida"].ToString(), out _fecha_salida);
                        _id_tipo_actualizacion_salida = Convert.ToByte(r["IdTipoActualizacionSalida"]);
                        _id_razon_salida_tarde = Convert.ToByte(r["IdRazonSalidaTarde"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _row_version = (byte[])r["RowVersion"];

                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Genera una Instancia de Tipo Parada ligando una secuencia y un Id de Servicio
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(decimal secuencia, int id_servicio)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 14, 0, id_servicio, secuencia, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_parada = Convert.ToInt32(r["Id"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _secuencia_parada_servicio = Convert.ToDecimal(r["SecuenciaParadaServicio"]);
                        _id_tipo_parada = Convert.ToByte(r["IdTipoParada"]);
                        _id_estatus_parada = Convert.ToByte(r["IdEstatusParada"]);
                        _id_ubicacion = Convert.ToInt32(r["IdUbicacion"]);
                        _geo_ubicacion = (SqlGeography)r["GeoUbicacion"];
                        _descripcion = r["Descripcion"].ToString();
                        DateTime.TryParse(r["CitaParada"].ToString(), out _cita_parada);
                        DateTime.TryParse(r["FechaLLegada"].ToString(), out _fecha_llegada);
                        _id_tipo_actualizacion_llegada = Convert.ToByte(r["IdTipoActualizacionLlegada"]);
                        _id_razon_llegada_tarde = Convert.ToByte(r["IdRazonLLegadaTarde"]);
                        DateTime.TryParse(r["FechaSalida"].ToString(), out _fecha_salida);
                        _id_tipo_actualizacion_salida = Convert.ToByte(r["IdTipoActualizacionSalida"]);
                        _id_razon_salida_tarde = Convert.ToByte(r["IdRazonSalidaTarde"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _row_version = (byte[])r["RowVersion"];

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
        ///  Método encargado de Editar una Parada
        /// </summary>
        /// <param name="id_servicio"> Id de Servicio ligado a la parada, puede ser 0 cuando no este ligado al Servicio</param>
        /// <param name="secuencia_parada_servicio">Secuaencia actual dado el Servicio</param>
        /// <param name="id_tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_estatus_parada">Estatus de la parada</param>
        /// <param name="id_ubicacion"> Id de Ubicación donde se está realizando la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="descripcion">Descripción del lugar donde la parada se está realizando.</param>
        /// <param name="cita_parada">Fecha y Hora de llegada de espera de la parada</param>
        /// <param name="fecha_llegada">Fecha y Hora de la Llegada</param>
        /// <param name="tipo_actualizacion_llegada">Medio  que se utilizó  para actualizar la llegada</param>
        /// <param name="id_razon_llegada_tarde">En caso de llegada Tarde, establece la causa</param>
        /// <param name="fecha_salida">Fecha y Hora de Salida de la Parada</param>
        /// <param name="tipo_actualizacion_salida">Medio  que se utilizó  para actualizar la  salida</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaParada(int id_servicio, decimal secuencia_parada_servicio, TipoParada id_tipo_parada, EstatusParada id_estatus_parada, int id_ubicacion,
                                             SqlGeography geo_ubicacion, string descripcion, DateTime cita_parada, DateTime fecha_llegada, TipoActualizacionLlegada tipo_actualizacion_llegada,
                                             byte id_razon_llegada_tarde, DateTime fecha_salida, TipoActualizacionSalida tipo_actualizacion_salida, byte id_razon_salida_tarde,
                                             int id_usuario, bool habilitar)
        {
            //Establecemos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Versión del Registro
            if (validaVersionRegistro())
            {

                //Inicializando arreglo de parámetros
                object[] param = { 2, this.id_parada, id_servicio, secuencia_parada_servicio, id_tipo_parada, id_estatus_parada, id_ubicacion, geo_ubicacion,
                                   descripcion, Fecha.ConvierteDateTimeObjeto(cita_parada), Fecha.ConvierteDateTimeObjeto(fecha_llegada), tipo_actualizacion_llegada, id_razon_llegada_tarde,
                                   Fecha.ConvierteDateTimeObjeto(fecha_salida), tipo_actualizacion_salida, id_razon_salida_tarde, id_usuario, habilitar, this._row_version,  "", ""};

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

            //inicializamos el arreglo de parametros
            object[] param = { 4, this.id_parada, 0, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, this._row_version, "", "" };

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
        /// Obtiene Ultima Secuencia de la Parada
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        private static decimal obtieneSecuenciaParada(int id_servicio)
        {
            //Declaramos Resultados
            decimal secuencia = 1;

            //inicializamos el arreglo de parametros
            object[] param = { 5, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Secuencia
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
        /// Actualizamos el estatus de la parada a Registrada
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private RetornoOperacion actualizaParadaAEstatusRegistradaParaReversa(int id_usuario)
        {
            //Devolvemos Resultado
            return this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, EstatusParada.Registrado, this._id_ubicacion,
                   this._geo_ubicacion, this._descripcion, this._cita_parada, DateTime.MinValue, (TipoActualizacionLlegada)0, 0, this._fecha_salida,
                   (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Actualizamos el estatus de la parada a Iniciada
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private RetornoOperacion actualizaParadaAEstatusIniciadaParaReversa(int id_usuario)
        {
            //Devolvemos Resultado
            return this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, EstatusParada.Iniciado, this._id_ubicacion,
                   this._geo_ubicacion, this._descripcion, this._cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, 0, DateTime.MinValue,
                   (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this._habilitar);
        }

        #endregion

        #region Metodos publicos


        /// <summary>
        /// Método encargado de Insertar el Viaje, a partir de las paradas de carga y descarga.
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada.</param>
        /// <param name="id_ubicacion_carga">Establece el lugar inicial en donde se realizara la carga  </param>
        /// <param name="cita_carga">Estableca la cita de carga del servicio</param>
        /// <param name="id_ubicacion_descarga">Establece el ultimo lugar de descarga del servicio</param>
        /// <param name="cita_descarga">Fecha y Hora donde se realizara la Descarga</param>
        /// <param name="id_ruta">Id Ruta que se utilizara</param>
        /// <param name="id_compania_emisor">Compañia al cual pertenece el Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="bit_copia"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicio(int id_servicio, int id_ubicacion_carga, DateTime cita_carga, int id_ubicacion_descarga, DateTime cita_descarga, int id_ruta,
                                                   int id_compania_emisor, int id_usuario, bool bit_copia)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Variables para Alamcenar el Id de Parada Carga y el Id Parada Descarga.
            int IdParadaCarga = 0;
            int IdParadaDescarga = 0;

            //Validando BIT de Copiado
            bool validacionCopia = bit_copia ? true : id_ubicacion_carga != id_ubicacion_descarga;

             //Validamos Ubicación de las paradas
            if (validacionCopia)
            {
                //Creamos la transacción 
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Insertamos Parada de Carga
                    resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_carga, cita_carga, SqlGeography.Null, id_usuario);

                    //Validamos Insercción Parada Carga
                    if (resultado.OperacionExitosa)
                    {
                        //Establecemos Id parada de Carga generado.
                        IdParadaCarga = resultado.IdRegistro;

                        //Insertamos Evento (Carga)
                        resultado = ParadaEvento.InsertaParadaEvento(id_servicio, IdParadaCarga, 0, 1, cita_carga.AddMinutes(1), id_usuario);

                        //Validamos Insercción del Evento
                        if (resultado.OperacionExitosa)
                        {
                            //Insertamos  Parada de Descarga
                            resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_descarga, cita_descarga, SqlGeography.Null, id_usuario);

                            //Validamos Insercción de la parada de Descarga
                            if (resultado.OperacionExitosa)
                            {
                                //Establecemos Id parada de Descarga generado.
                                IdParadaDescarga = resultado.IdRegistro;

                                //Insertamos Evento (Descarga)
                                resultado = ParadaEvento.InsertaParadaEvento(id_servicio, IdParadaDescarga, 0, 2,cita_descarga.AddMinutes(1), id_usuario);

                                //Validamos Insercción del evento
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertamos Segmento de Carga/Descarga
                                    resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, IdParadaCarga, IdParadaDescarga, id_ruta, id_usuario);

                                    //Validamos Insercción del Segmento
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Insertamos Movimiento de Carga/Descarga actualizando el Kilometraje Correspondiente
                                        resultado = Movimiento.InsertaMovimiento(id_servicio, resultado.IdRegistro, Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor, IdParadaCarga, IdParadaDescarga, id_usuario);

                                        //Validamos Insercción del Movimiento
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Instanciamos Servicio
                                            using (Servicio objServicio = new Servicio(id_servicio))
                                            {
                                                //Calculamos Kilometraje
                                                resultado = objServicio.CalculaKilometrajeServicio(id_usuario);
                                            }
                                            //Validamos Inserccion de Despacho
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Devolvemos Id de Servicio
                                                resultado = new RetornoOperacion(id_servicio);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Si la Operación fue exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Resultado transacción
                        scope.Complete();
                    }
                }//Fin Transacción
            }//Validamos Ubicaciòn
            else
            {
                //Establecemos Mensaje 
                resultado = new RetornoOperacion("No es posible la insercción del servicio con la misma ubicación de carga y descarga.");
            }
            return resultado;
        }

        /// <summary>
        /// Método encargado de Insertar el Viaje, a partir de las paradas de carga y descarga.
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada.</param>
        /// <param name="id_ubicacion_carga">Establece el lugar inicial en donde se realizara la carga  </param>
        /// <param name="cita_carga">Estableca la cita de carga del servicio</param>
        /// <param name="id_ubicacion_descarga">Establece el ultimo lugar de descarga del servicio</param>
        /// <param name="cita_descarga">Fecha y Hora donde se realizara la Descarga</param>
        /// <param name="id_ruta">Id Ruta que se utilizara</param>
        /// <param name="id_compania_emisor">Compañia al cual pertenece el Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="bit_copia"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicio(int id_servicio, int id_ubicacion_carga, DateTime cita_carga, int id_ubicacion_descarga, DateTime cita_descarga, int id_ruta,
                                                   int id_compania_emisor, int id_usuario, bool bit_copia, int id_parada, int id_parada_descarga)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Variables para Alamcenar el Id de Parada Carga y el Id Parada Descarga.
            int IdParadaCarga = 0;
            int IdParadaDescarga = 0;

            //Validando BIT de Copiado
            bool validacionCopia = bit_copia ? true : id_ubicacion_carga != id_ubicacion_descarga;

            //Validamos Ubicación de las paradas
            if (validacionCopia)
            {
                //Creamos la transacción 
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Insertamos Parada de Carga
                    resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_carga, cita_carga, SqlGeography.Null, id_usuario);

                    //Validamos Insercción Parada Carga
                    if (resultado.OperacionExitosa)
                    {
                        //Establecemos Id parada de Carga generado.
                        IdParadaCarga = resultado.IdRegistro;
                        //Foreach para eventos
                        //Cargando los los eventos 
                        using (DataTable tblParadaEventos = SAT_CL.Despacho.ParadaEvento.CargaEventos(id_parada))
                        {
                            //Validamos Vales
                            if (Validacion.ValidaOrigenDatos(tblParadaEventos))
                            {
                                //Por Cada Vale de Diesel
                                foreach (DataRow eventos in tblParadaEventos.Rows)
                                {
                                    //Validamos resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Insertamos Evento (Carga)
                                        resultado = ParadaEvento.InsertaParadaEvento(id_servicio, IdParadaCarga, 0, eventos.Field<int>("IdTipoEvento"), cita_carga.AddMinutes(1), id_usuario);
                                    }
                                    else
                                        //Finalizamos ciclo
                                        break;
                                }

                            }
                        }
                        //Validamos Insercción del Evento
                        if (resultado.OperacionExitosa)
                        {
                            //Insertamos  Parada de Descarga
                            resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_descarga, cita_descarga, SqlGeography.Null, id_usuario);

                            //Validamos Insercción de la parada de Descarga
                            if (resultado.OperacionExitosa)
                            {
                                //Establecemos Id parada de Descarga generado.
                                IdParadaDescarga = resultado.IdRegistro;

                                //Cargando los los eventos 
                                using (DataTable tblParadaEventos = SAT_CL.Despacho.ParadaEvento.CargaEventos(id_parada_descarga))
                                {
                                    //Validamos Vales
                                    if (Validacion.ValidaOrigenDatos(tblParadaEventos))
                                    {
                                        //Por Cada Vale de Diesel
                                        foreach (DataRow eventos in tblParadaEventos.Rows)
                                        {
                                            //Validamos resultado
                                            if (resultado.OperacionExitosa)
                                            {                                          
                                                //Insertamos Evento (Descarga)
                                                resultado = ParadaEvento.InsertaParadaEvento(id_servicio, IdParadaDescarga, 0, eventos.Field<int>("IdTipoEvento"), cita_descarga.AddMinutes(1), id_usuario);
                                            }
                                            else
                                                //Finalizamos ciclo
                                                break;
                                        }

                                    }
                                }
                                //Validamos Insercción del evento
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertamos Segmento de Carga/Descarga
                                    resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, IdParadaCarga, IdParadaDescarga, id_ruta, id_usuario);

                                    //Validamos Insercción del Segmento
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Insertamos Movimiento de Carga/Descarga actualizando el Kilometraje Correspondiente
                                        resultado = Movimiento.InsertaMovimiento(id_servicio, resultado.IdRegistro, Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor, IdParadaCarga, IdParadaDescarga, id_usuario);

                                        //Validamos Insercción del Movimiento
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Instanciamos Servicio
                                            using (Servicio objServicio = new Servicio(id_servicio))
                                            {
                                                //Calculamos Kilometraje
                                                resultado = objServicio.CalculaKilometrajeServicio(id_usuario);
                                            }
                                            //Validamos Inserccion de Despacho
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Devolvemos Id de Servicio
                                                resultado = new RetornoOperacion(id_servicio);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Si la Operación fue exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Resultado transacción
                        scope.Complete();
                    }
                }//Fin Transacción
            }//Validamos Ubicaciòn
            else
            {
                //Establecemos Mensaje 
                resultado = new RetornoOperacion("No es posible la insercción del servicio con la misma ubicación de carga y descarga.");
            }
            return resultado;
        }
        /// <summary>
        /// Método encargado de Insertar paradas principales de un servicio, con paradas de carga y descarga parcial.
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada.</param>
        /// <param name="id_ubicacion_carga">Establece el lugar inicial en donde se realizara la carga  </param>
        /// <param name="cita_carga">Estableca la cita de carga del servicio</param>
        /// <param name="id_ubicacion_descarga">Establece el ultimo lugar de descarga del servicio</param>
        /// <param name="cita_descarga">Fecha y Hora donde se realizara la Descarga</param>
        /// <param name="id_ruta">Id Ruta que se utilizara</param>
        /// <param name="id_compania_emisor">Compañia al cual pertenece el Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioDocumentacion(int id_servicio, int id_ubicacion_carga, DateTime cita_carga, int id_ubicacion_descarga, DateTime cita_descarga, int id_ruta,
                                                   int id_compania_emisor, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Variables para Alamcenar el Id de Parada Carga y el Id Parada Descarga.
            int IdParadaCarga = 0;
            int IdParadaDescarga = 0;

            //Validamos Ubicación de las paradas
            if (id_ubicacion_carga != id_ubicacion_descarga)
            {
                //Creamos la transacción 
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Insertamos Parada de Carga
                    resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_carga, cita_carga, SqlGeography.Null, id_usuario);

                    //Validamos Insercción Parada Carga
                    if (resultado.OperacionExitosa)
                    {
                        //Establecemos Id parada de Carga generado.
                        IdParadaCarga = resultado.IdRegistro;

                        //Insertamos Evento (Carga)
                        resultado = ParadaEvento.InsertaParadaEvento(id_servicio, IdParadaCarga, 0, 1, cita_carga.AddMinutes(1), id_usuario);

                        //Validamos Insercción del Evento
                        if (resultado.OperacionExitosa)
                        {
                            //Insertamos  Parada de Descarga
                            resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_descarga, cita_descarga, SqlGeography.Null, id_usuario);

                            //Validamos Insercción de la parada de Descarga
                            if (resultado.OperacionExitosa)
                            {
                                //Establecemos Id parada de Descarga generado.
                                IdParadaDescarga = resultado.IdRegistro;

                                //Insertamos Evento (Descarga)
                                resultado = ParadaEvento.InsertaParadaEvento(id_servicio, IdParadaDescarga, 0, 3,cita_descarga.AddMinutes(1), id_usuario);

                                //Validamos Insercción del evento
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertamos Segmento de Carga/Descarga
                                    resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, IdParadaCarga, IdParadaDescarga, id_ruta, id_usuario);

                                    //Validamos Insercción del Segmento
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Insertamos Movimiento de Carga/Descarga actualizando el Kilometraje Correspondiente
                                        resultado = Movimiento.InsertaMovimiento(id_servicio, resultado.IdRegistro, Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor, IdParadaCarga, IdParadaDescarga, id_usuario);

                                        //Validamos Insercción del Movimiento
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Instanciamos Servicio
                                            using (Servicio objServicio = new Servicio(id_servicio))
                                            {
                                                //Calculamos Kilometraje
                                                resultado = objServicio.CalculaKilometrajeServicio(id_usuario);
                                            }
                                            //Validamos Inserccion de Despacho
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Devolvemos Id de Servicio
                                                resultado = new RetornoOperacion(id_servicio);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Si la Operación fue exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Resultado transacción
                        scope.Complete();
                    }
                }//Fin Transacción
            }//Validamos Ubicaciòn
            else
            {
                //Establecemos Mensaje 
                resultado = new RetornoOperacion("No es posible la insercción del servicio con la misma ubicación de carga y descarga.");
            }
            return resultado;
        }
        /// <summary>
        /// Inserta las dos primeras paradas de un servicio, creando segmento y calculando su kilometraje
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada.</param>
        /// <param name="tipo_parada">Tipo de Parada Origen</param>
        /// <param name="id_ubicacion_principal">Establece el lugar de la primer parada del servicio </param>
        /// <param name="cita_principal">Estableca la cita de la primer parada del servicio</param>
        /// <param name="id_ubicacion_secundaria">Establece el lugar de la segunda parada del servicio</param>
        /// <param name="cita_secundaria">Establece la cita de la segunda parada del servicio</param>
        /// <param name="id_ruta">Id Ruta que se utilizara</param>
        /// <param name="id_compania_emisor">Compañia al cual pertenece el Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadasInicialesServicio(int id_servicio, int id_ubicacion_principal, DateTime cita_principal, int id_ubicacion_secundaria, DateTime cita_secundaria, int id_ruta,
                                                   int id_compania_emisor, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Variables para Alamcenar el Id de Parada Carga y el Id Parada Descarga.
            int idParadaPrincipal = 0;
            int idParadaSecundaria = 0;

            //Validamos Ubicación de las paradas
            if (id_ubicacion_principal != id_ubicacion_secundaria)
            {
                //Creamos la transacción 
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Insertamos Parada Inicial
                    resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_principal, cita_principal, SqlGeography.Null, id_usuario);

                    //Validamos Insercción Parada Inicial
                    if (resultado.OperacionExitosa)
                    {
                        //Recuperando Id de Primer parada
                        idParadaPrincipal = resultado.IdRegistro;

                        //Insertamos  Parada Secundaria
                        resultado = InsertaParada(id_servicio, TipoParada.Operativa, id_ubicacion_secundaria, cita_secundaria, SqlGeography.Null, id_usuario);

                        //Validamos Insercción de la parada secundaria
                        if (resultado.OperacionExitosa)
                        {
                            //Recuperando Id de Segunda parada
                            idParadaSecundaria = resultado.IdRegistro;

                            //Insertamos Segmento de Carga/Descarga
                            resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, idParadaPrincipal, idParadaSecundaria, id_ruta, id_usuario);

                            //Validamos Insercción del Segmento
                            if (resultado.OperacionExitosa)
                            {
                                //Insertamos Movimiento de Carga/Descarga actualizando el Kilometraje Correspondiente
                                resultado = Movimiento.InsertaMovimiento(id_servicio, resultado.IdRegistro, Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor, idParadaPrincipal, idParadaSecundaria, id_usuario);

                                //Validamos Insercción del Movimiento
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciamos Servicio
                                    using (Servicio objServicio = new Servicio(id_servicio))
                                        //Calculamos Kilometraje
                                        resultado = objServicio.CalculaKilometrajeServicio(id_usuario);

                                    //Validamos Inserccion de Despacho
                                    if (resultado.OperacionExitosa)
                                        //Devolvemos Id de Servicio
                                        resultado = new RetornoOperacion(id_servicio);
                                }
                            }
                        }
                    }
                    //Si la Operación fue exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Resultado transacción
                        scope.Complete();
                    }
                }//Fin Transacción
            }//Validamos Ubicaciòn
            else
            {
                //Establecemos Mensaje 
                resultado = new RetornoOperacion("No es posible la insercción de dos paradas consecutivas con la misma ubicación.");
            }
            return resultado;
        }
        /// <summary>
        /// Método en cargado de dar Reversa a la Fecha de llegada
        /// </summary>
        /// <param name="tipo_actualizacion_inicio_evento">Tipo actualización inicio del evento (Manual, GPS).</param>
        /// <param name="tipo_actualizacion_fin_evento">Tipo actualizacion inicio del evento (Manual, GPS).</param>
        /// <param name="id_usuario">Id usuario</param>
        /// <returns></returns>
        public RetornoOperacion ReversaParadaFechaLlegada(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_parada);

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //1. Validar que el estatus actual de la parada sea Iniciado  
                if (this.Estatus == EstatusParada.Iniciado)
                {
                    //2. Recuperar movimiento cuya parada de destino sea la parada a reversar (en caso de existir)
                    int idMovimientoAnterior = Movimiento.BuscamosMovimientoParadaDestino(this._id_servicio, this._id_parada);
                    Movimiento movimientoAnterior = new Movimiento();
                    //Si existe un movimiento que actualizar
                    if (idMovimientoAnterior > 0)
                    {
                        //Instanciando movimiento
                        movimientoAnterior = new Movimiento(idMovimientoAnterior);
                        //Si no se pudo recuperar el movimiento
                        if (movimientoAnterior.id_movimiento <= 0)
                            resultado = new RetornoOperacion("Error al recuperar movimiento previo a la llegada a esta parada.");
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {                        
                        //3. Determinar la secuencia de la parada
                        //3.1 Parada intermedia distinta a la última parada
                        if (this._secuencia_parada_servicio > 1)
                        {
                            //3.1.1 Realizando reversa para fecha de llegada del movimiento
                            resultado = movimientoAnterior.ReversaTerminaMovimiento(id_usuario);
                        }
                        //3.2 Si es la primer parada
                        else if (this._secuencia_parada_servicio == 1)
                        {
                            //Actualizando recursos y estancias
                            resultado = MovimientoAsignacionRecurso.ReversaMueveAsignacionesParadaInicial(this._id_parada, id_usuario);

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciando al servicio
                                using (Documentacion.Servicio servicio = new Servicio(this._id_servicio))
                                {
                                    //Actualizamos Estatus del Servicio a Documentado
                                    resultado = servicio.ActualizaEstatusServicio(Servicio.Estatus.Documentado, id_usuario);

                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Instanciamos Servicio Despacho
                                        using (ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                                            //Actualizar parada de origen en la información de su despacho
                                            resultado = objServicioDespacho.ActualizaParadaOrigenServicio(0, DateTime.MinValue, id_usuario);
                                    }
                                    //Si no se actualizó el estatus del servicio
                                    else
                                        resultado = new RetornoOperacion(string.Format("Error al iniciar servicio: {0}", resultado.Mensaje));
                                }
                            }
                        }

                        //Si la parada de destino del movimiento es una parada Operativa y no se encontraron errores previos
                        if (resultado.OperacionExitosa && this.Tipo == Parada.TipoParada.Operativa)
                        {
                            //Buscamos si la parada se encuentra como destino de un Segmento
                            using (SegmentoCarga segmento = new SegmentoCarga(SegmentoCarga.BuscamosSegmentoParadaDestino(this._id_servicio, this._id_parada)))
                            {
                                //Validamos que exista segmento
                                if (segmento.id_segmento_carga > 0)
                                    //Se reinicia el Segmento
                                    resultado = segmento.IniciaSegmento(id_usuario);
                            }
                        }

                        //Si no hay errores al reversar término de movimiento anterior
                        if (resultado.OperacionExitosa)
                            //Actualizando estatus de parada
                            resultado = reversaIniciaParada(id_usuario);
                    }
                }
                //Si no se encuentra iniciada la parada
                else
                    resultado = new RetornoOperacion("La parada que desea reversar no se encuentra en estatus 'Iniciada'.");

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();

            }

            //Devolvemos Resultado
            return resultado;
        }
        
        /// <summary>
        /// Reversa la fecha de Salida
        /// </summary>
        /// <param name="tipo_actualizacion_inicio_evento">Tipo actualización inicio del evento (Manual, GPS)</param>
        /// <param name="tipo_actualizacion_fin_evento">Tipo actualizacion fin del evento (Manual, GPS).</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ReversaParadaFechaSalida(int id_usuario)
        {
            //Declaramos Objeto Resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_parada);

            //1. Validar que no sea parada final
            //Buscamos si la parada se encuentra como origen de un Movimiento
            int id_movimiento = Movimiento.BuscamosMovimientoParadaOrigen(this._id_servicio, this._id_parada);

            //Determinando si la parada actual es la última del servicio
            if (id_movimiento > 0)
            {
                //2. Obtener parada siguiente
                using (Parada paradaSiguiente = new Parada(this._secuencia_parada_servicio + 1, this._id_servicio))
                {
                    //Si la parada fue encontrada
                    if (paradaSiguiente.id_parada > 0)
                    {
                        //3. Validar que la parada siguiente Tenga estatus Registrado
                        if (paradaSiguiente.Estatus == EstatusParada.Registrado)
                        {
                            //4. Validar que la parada actual se encuentre terminada
                            if (this.Estatus == EstatusParada.Terminado)
                            {
                                //5. Si la parada es operativa, validar si el el inicio de un segmento de carga
                                if (this.Tipo == TipoParada.Operativa)
                                {
                                    //Instanciar segmento correspondiente
                                    using (SegmentoCarga segmento = new SegmentoCarga(SegmentoCarga.BuscamosSegmentoParadaOrigen(this._id_servicio, this._id_parada)))
                                    {
                                        //Si el segmento existe
                                        if (segmento.id_segmento_carga > 0)
                                            //Actualziando segmento a estatus registrado
                                            resultado = segmento.ActualizaEstatusSegmento(SegmentoCarga.Estatus.Registrado, id_usuario);
                                    }
                                }

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciando movimiento
                                    using (Movimiento movimiento = new Movimiento(id_movimiento))
                                    {
                                        //Si se encontró el movimiento
                                        if (movimiento.id_movimiento > 0)
                                        {
                                            //6. Realizar reversa de inicio de movimiento
                                            resultado = movimiento.ReversaIniciaMovimiento(id_usuario);

                                            //Si no hay errores de reversa de movimiento
                                            if (resultado.OperacionExitosa)
                                                //7. Actualizar estatus de parada actual a Iniciada
                                                resultado = editaParada(this._id_servicio, this._secuencia_parada_servicio, this.Tipo, EstatusParada.Iniciado, this._id_ubicacion, this._geo_ubicacion,
                                                                        this._descripcion, this._cita_parada, this._fecha_llegada, this.TipoActualizacionLlegada_, this._id_razon_llegada_tarde,
                                                                        DateTime.MinValue, TipoActualizacionSalida.SinActualizar, this._id_razon_salida_tarde, id_usuario, this._habilitar);
                                        }
                                        //Si no se encontró el movimiento en bd
                                        else
                                            resultado = new RetornoOperacion(string.Format("Error al cargar movimiento '{0}'", id_movimiento));
                                    }
                                }
                            }
                            //Si no esta terminada
                            else
                                resultado = new RetornoOperacion("La parada que desea reversar no se encuentra en estatus 'Terminada'.");
                        }
                        //Indicando que la parada siguiente debe ser actualizada en primer instancia
                        else
                            resultado = new RetornoOperacion(string.Format("La parada con secuencia '{0:#####}' debe estar en estatus 'Registrada'.", paradaSiguiente.secuencia_parada_servicio));
                    }
                    //Si no se encontró la parada siguiente
                    else
                        resultado = new RetornoOperacion("No fue localizada la parada posterior a la parada que se quiere actualizar.");
                }
            }
            //Si existe una parada posterior en estatus distinto de Registrada
            else
                resultado = new RetornoOperacion("No es posible actualizar esta parada, ya que es la última del servicio. Utilice la funcionalidad de 'Reapertura de Servicio'.");

            //Devolviendo resultado 
            return resultado;
        }



        /// <summary>
        /// Realiza la búsqueda de aquella parada que involucra a la ubicación solicitada, pero que no está asociada a ningún servicio y no tiene secuencia
        /// </summary>
        /// <param name="id_ubicacion">Id de Ubicación</param>
        /// <param name="crear">True para crear un nuevo registro en caso de no existir alguno</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public static int ObtieneParadaComodinUbicacion(int id_ubicacion, bool crear, int id_usuario)
        { 
            //Declarando objeto de retorno
            int id_parada = 0;

            //Declarando el conjunto de parámetros de búsqueda
            object[] param = { 21, 0, 0, 0, 0, 0, id_ubicacion, null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Buscando parada coincidente
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            { 
                //Si hay coincidencia
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Para cada resultado
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Instanciando parada
                        id_parada = Convert.ToInt32(r["IdParada"]);
                        break;
                    }
                }
            }

            //Si la parada no existe 
            if(id_parada == 0 && crear)
            {
                RetornoOperacion resultado = new RetornoOperacion();

                try
                {
                    //Insertando nuevo registro parada alterna (comodin)
                    resultado = Parada.InsertaParadaAlternativaEstancia(0, Parada.EstatusParada.Iniciado, -1, Parada.TipoParada.Servicio, id_ubicacion, DateTime.MinValue, null,
                                        DateTime.MinValue, Parada.TipoActualizacionLlegada.SinActualizar, 0, DateTime.MinValue, Parada.TipoActualizacionSalida.SinActualizar, 0, id_usuario);
                }
                catch (Exception ex)
                {
                    //Instanciando Error
                    resultado = new RetornoOperacion(string.Format("Source:{0} - Message:{1} - Inner:{2}", ex.Source, ex.Message, ex.InnerException));
                }

                //Si se creó correctamente
                if (resultado.OperacionExitosa)
                    id_parada = resultado.IdRegistro;
            }

            //Devolviendo resultado
            return id_parada;
        }
        /// <summary>
        /// Realiza la búsqueda de aquella parada que involucra a la ubicación solicitada, pero que no está asociada a ningún servicio y no tiene secuencia
        /// </summary>
        /// <param name="id_ubicacion">Id de Ubicación</param>
        /// <returns></returns>
        public static int ObtieneParadaComodinUbicacion(int id_ubicacion)
        { 
            //Buscando parada
            return ObtieneParadaComodinUbicacion(id_ubicacion, false, 0);
        }
        /// <summary>
        /// Método encargado de Insertar una Parada, asignando el valor de la secuencia
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada, puede ser 0 cuando no este ligado al Servicio</param>
        /// <param name="secuencia">Asigna el numero de Secuencia por default a la parada</param>
        /// <param name="estatus"></param>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="fecha_llegada">Fecha Llegada de la parada</param>
        /// <param name="tipo_actualizacion_llegada">Tipo actualización llegada Tarde (Manual, GPS)</param>
        /// <param name="id_razon_llegada_tarde">Actualización Llegada Tarde</param>
        /// <param name="fecha_salida">Fecha de salida de la parada</param>
        /// <param name="tipo_actualizacion_salida">Tipo actualización Salida de la parada (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParadaAlternativaEstancia(int id_servicio, EstatusParada estatus, decimal secuencia, TipoParada tipo_parada, int id_ubicacion, DateTime cita_parada,
                                      SqlGeography geo_ubicacion, DateTime fecha_llegada, TipoActualizacionLlegada tipo_actualizacion_llegada, byte id_razon_llegada_tarde,
                                      DateTime fecha_salida, TipoActualizacionSalida tipo_actualizacion_salida, byte id_razon_salida_tarde, int id_usuario)
        {
            //Declaramos la variables a utilizar 
            string descripcion = "";
            //Validamos si existe ubicacion
            if (id_ubicacion == 0)
            {
                //En caso de no existir la ubicacion obtenemos el geocodificacion inversa de las coordenadas
                //TODO: Metodo geocodificacion inversa
                descripcion = "Valor metodo geocodificacion inversa";
            }
            else
            {
                //Instanciamos  la Ubicación para obtener la descripción.
                using (Ubicacion objUbicacion = new Ubicacion(id_ubicacion))
                {
                    //Obtenemos la descripcion de la ubicacion
                    descripcion = objUbicacion.descripcion.ToUpper();
                    //Instanciamos Ciudad
                    using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                    {
                        //Validamos que exista la Ciudad
                        if (objCiudad.id_ciudad > 0)
                        {
                            //Obtenemos la descripcion de la ubicacion
                            descripcion += " " + objCiudad.ToString();
                        }
                    }
                }
            }

            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_servicio, secuencia, tipo_parada, estatus, id_ubicacion, geo_ubicacion,
                                 descripcion, Fecha.ConvierteDateTimeObjeto(cita_parada),
                                 Fecha.ConvierteDateTimeObjeto(fecha_llegada) , tipo_actualizacion_llegada, id_razon_llegada_tarde,
                Fecha.ConvierteDateTimeObjeto(fecha_salida), tipo_actualizacion_salida, id_razon_salida_tarde, id_usuario,
                                 true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Inserta Nueva Parada
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="tipo_parada">Tipo Parada</param>
        /// <param name="id_ubicacion">Id Ubicacion</param>
        /// <param name="geo_ubicacion">GeoUbicacion</param>
        /// <param name="cita_parada">Cita Parada</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion NuevaParadaServicio(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_ubicacion, SqlGeography geo_ubicacion, DateTime cita_parada, int id_ruta, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos variables
            int id_parada;
            int id_segmento_nuevo = 0;
            int id_segmento_insercion = 0;
            int id_segmento_edicion = 0;
            decimal secuencia_anterior_carga = 0;
            decimal secuencia_posterior_carga = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //1. Instanciando servicio
                using (Servicio servicio = new Servicio(id_servicio))
                {
                    //Validando que el servicio esté cargado correctamente
                    if (servicio.id_servicio > 0)
                    {
                        //3. Actualizamos las secuencias de las paradas de acuerdo a la secuencia que se esta ingresando.
                        resultado = ActualizaSecuenciaParadas(id_servicio, secuencia, id_usuario);

                        //Validamos Actualizacion de Secuencias.
                        if (resultado.OperacionExitosa)
                        {
                            //4. Insertamo la nueva parada con la secuencia deseada
                            resultado = InsertaParada(id_servicio, secuencia, tipo_parada, id_ubicacion, cita_parada, geo_ubicacion, id_usuario);

                            //Validamos Insercion de Parada.
                            if (resultado.OperacionExitosa)
                            {
                                //Obtenemos el id de la parada recien insertada
                                id_parada = resultado.IdRegistro;

                                //5. Insertamos Evento 
                                //DONE: Se omite este paso, ya que esta sobrecarga de método no es solicitado el tipo de evento

                                //6.En caso de ser necesario modificamos los segmentos (Solo en inserción de paradas operativas)
                                if (tipo_parada == TipoParada.Operativa)
                                {
                                    //Buscamos la parada operativa anterior y la parada operativa posterior
                                    using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(id_servicio, secuencia)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(id_servicio, secuencia)))
                                    {
                                        //En caso de insertarse un segmento al inicio
                                        if (objParadaAnteriorCarga.id_parada == 0)
                                        {
                                            //6.1 Actualizamos todas las secuencias de los segmentos.
                                            resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, 0, id_usuario);

                                            //Validamos Actualizacion de la Secuencias
                                            if (resultado.OperacionExitosa)
                                            {
                                                //6.2 Insertamos el nuevo segmento con secuencia 1
                                                resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                //Obtenemos el id del segmneto  recien insertado
                                                id_segmento_nuevo = resultado.IdRegistro;
                                                secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                //Validamos Insercción Segmneto
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Instanciamos Servicio 
                                                    using (Documentacion.Servicio objServicio = new Documentacion.Servicio(id_servicio))
                                                    {
                                                        //6.3 Actualizamos la ubicación de Carga del Servicio
                                                        resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, id_ubicacion, cita_parada
                                                                                               , objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte
                                                                                               , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                               id_usuario);
                                                    }
                                                    //Validamos Actualización de la ubicación de Carga
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //6.4 Actualizamos el segmento de los Movimientos
                                                        resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                            //En caso de insertarse un segmento al final
                                            if (objParadaPosteriorCarga.id_parada == 0)
                                            {
                                                //6.1 No se actualizan secuencia de segmentos 
                                                //6.2 Insertamos el nuevo segmento con secuencia maxima mas 1
                                                resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);

                                                //Obtenemos el id del segmneto  recien insertado
                                                id_segmento_nuevo = resultado.IdRegistro;
                                                //Obtenemos la secuencia de la parada anterior de carga
                                                secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;

                                                //Validamos Insercción Segmneto
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Instanciamos Servicio
                                                    using (Documentacion.Servicio objServicio = new Documentacion.Servicio(id_servicio))
                                                    {
                                                        //6.3 Actualizamos la ubicación de Descarga del servicio
                                                        resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga
                                                                                               , id_ubicacion, cita_parada, objServicio.porte
                                                                                               , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                               id_usuario);
                                                    }
                                                    //Validamos Actualización
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //6.4 Actualizamos el segmento de los Movimientos
                                                        resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, objParadaAnteriorCarga.secuencia_parada_servicio, secuencia, id_segmento_nuevo, id_usuario);
                                                    }
                                                }
                                            }
                                            //En caso de insertarse un segmento intermedio
                                            else
                                            {
                                                //Obtenemos la secuencia del segmento que contiene a las paradas anterior y posterior
                                                decimal SecuenciaSegmento = SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, objParadaAnteriorCarga.id_parada, objParadaPosteriorCarga.id_parada);

                                                //6.1 Actualizamos todas las secuencias de los segmentos.
                                                resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, SecuenciaSegmento, id_usuario);

                                                //Validamos Actualización de las secuencias
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //6.2 Insertamos el nuevo segmento
                                                    resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, SecuenciaSegmento + 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                    //Obtenemos el id del segmneto  recien insertado
                                                    id_segmento_nuevo = resultado.IdRegistro;

                                                    //Obtenemos Secuencias para su edición de movimiento
                                                    secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                    secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                    //Validamos Insercción del segmento
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //6.3 Instanciamos segmento a partir de la secuencia obtenida
                                                        using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(id_servicio, SecuenciaSegmento))
                                                        {
                                                            //6.4 Actualizamos el destino del segmento original
                                                            resultado = objSegmentoCarga.EditaSegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia, objSegmentoCarga.EstatusSegmento,
                                                                        objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);

                                                            //Validamos Insercción Segmneto
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //6.5 Actualizamos el segmento de los Movimientos
                                                                resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                            }

                                                        }
                                                    }
                                                }
                                            }//If modificacion segmento/insercion segmento intermedio
                                    }//Using instancia parada carga anterior y posterior
                                }//Fin if Modificacion Segmento
                                //Validamos Actualización
                                if (resultado.OperacionExitosa)
                                {
                                    //7. Modificamos/Insertamos los movimientos 
                                    using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(id_servicio, secuencia)), objParadaPosterior = new Parada(BuscaParadaPosterior(id_servicio, secuencia)))
                                    {
                                        //En caso de insertarse un movimiento al inicio
                                        if (objParadaAnterior.id_parada == 0)
                                        {
                                            //7.1 Actualizamos todas las secuencias de los movimientos.
                                            resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, 0, id_usuario);

                                            //Validamos Actualizacion de la Secuencias
                                            if (resultado.OperacionExitosa)
                                            {
                                                //7.2 Insertamos el nuevo movimiento con secuencia 1
                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, 1, Movimiento.Tipo.Cargado, 0,
                                                                                         0, servicio.id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);
                                                //Validamos Insercción del Movimiento
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Instanciamos Servicio 
                                                    using (Servicio objServicio = new Servicio(id_servicio))
                                                    {
                                                        //7.4 Calculamos Kilometraje del Servicio
                                                        resultado = objServicio.CalculaKilometrajeServicio(id_usuario);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            //En caso de insertarse un movimiento al final
                                            if (objParadaPosterior.id_parada == 0)
                                            {
                                                //7.1 No actualizamos todas las secuencias de los movimientos
                                                //7.2 Insertamos Movimiento al Final
                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, Movimiento.Tipo.Cargado, 0,
                                                           0, servicio.id_compania_emisor, objParadaAnterior.id_parada, id_parada, id_usuario);

                                                //Validamos Actialización de Movimiento
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Instanciamos Servicio 
                                                    using (Servicio objServicio = new Servicio(id_servicio))
                                                    {
                                                        //7.4 Calculamos Kilometraje del Servicio
                                                        resultado = objServicio.CalculaKilometrajeServicio(id_usuario);
                                                    }
                                                }
                                            }
                                            //En caso de insertar un movimiento intermedio.
                                            else
                                            {
                                                // Obtenemos el movimiento coincidente que contiene la parada anterior y la parada posterior.
                                                using (Movimiento objMovimiento = new Movimiento(objParadaAnterior.id_parada, objParadaPosterior.id_parada, id_servicio))
                                                {
                                                    //7.1 Actualizamos la secuencia de los movimientos
                                                    resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, objMovimiento.secuencia_servicio, id_usuario);

                                                    //Validamos actualización de las secuencias
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validando Tipo de Parada Operativa
                                                        if ((Parada.TipoParada)tipo_parada == Parada.TipoParada.Operativa)
                                                        {
                                                            //De acuerdo a las paradas existentes de carga asignamos Segmento
                                                            //Si la secuencia es al final
                                                            if (secuencia_posterior_carga == 0)
                                                            {
                                                                id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                id_segmento_edicion = id_segmento_nuevo;

                                                            }
                                                            else
                                                            {
                                                                //Si la secuencia es al Inicio
                                                                if (secuencia_anterior_carga == 0)
                                                                {
                                                                    id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                    id_segmento_insercion = id_segmento_nuevo;
                                                                }
                                                                else
                                                                {
                                                                    //Si la secuencia es en medio
                                                                    id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                    id_segmento_insercion = id_segmento_nuevo;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Validando Tipo de Parada Servicio
                                                            id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                            id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                        }

                                                        //7.2 Insertamos Movimiento
                                                        resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_insercion, objMovimiento.secuencia_servicio + 1,
                                                                    Movimiento.Tipo.Cargado, 0, 0, servicio.id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);

                                                        //Validamos Inserción del movimiento.
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //7.3 Actualizamos el destino del movimiento obtenido
                                                            resultado = objMovimiento.EditaMovimiento(objMovimiento.id_servicio, id_segmento_edicion,
                                                                          objMovimiento.secuencia_servicio, (Movimiento.Estatus)objMovimiento.id_estatus_movimiento,
                                                                         (Movimiento.Tipo)objMovimiento.id_tipo_movimiento, 0, 0, objMovimiento.id_compania_emisor,
                                                                          objMovimiento.id_parada_origen, id_parada, id_usuario);

                                                            //Validamos Actualización del Destino
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Instanciamos Servicio 
                                                                using (Servicio objServicio = new Servicio(id_servicio))
                                                                {
                                                                    //7.4 Calculamos Kilometraje del Servicio
                                                                    resultado = objServicio.CalculaKilometrajeServicio(id_usuario);
                                                                }

                                                            }

                                                        }//fin insercción de movimientos
                                                    }//fin actualización de movimientos
                                                }//using movimiento
                                            }//if modificación del movimiento/intermedio, final
                                    }//using instancia parada anterior y posterior
                                    //Validmaos Actualización
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Devolmvemos Id Parada
                                        resultado = new RetornoOperacion(id_parada);
                                    }
                                }//fin actualizaión movimientos/insercción evento
                            }//fin if insercion parada
                        }//Fin Actualización de Secuencias
                    }//Fin Validación de carga de servicio
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Validamos Transacción
                    scope.Complete();
                }
            }//Fin Parda Transaccion
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza el arribo de unidades de interés a la parda, señalando la fecha, tipo de actualización y el posible motivo de su retraso
        /// </summary>
        /// <param name="fecha_llegada">Fecha de Llegada de la parada, será utilizada para inicializar las estancias en la misma</param>
        /// <param name="tipo_actualizacion_llegada">Tipo actualización Llegada de la Parada (Manual, GPS)</param>
        /// <param name="id_razon_llegada_tarde">Id Razón Llegada Tarde de la Parada</param>
        /// <param name="tipo_actualizacion_estancia_inicio">Tipo actualización estancia Inicio de la Parada (Manual,GPS)</param>
        /// <param name="id_servicio">Tipo idservicio</param>
        /// <param name="secuencia">Tipo idservicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizarFechaLlegada(DateTime fecha_llegada, TipoActualizacionLlegada tipo_actualizacion_llegada,
                                                     byte id_razon_llegada_tarde, EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion_estancia_inicio, int id_usuario)
        {
            //Declaramos Variable Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio
                using (Servicio objServicio = new Servicio(this._id_servicio))
                {
                    //Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Iniciado || objServicio.estatus == Servicio.Estatus.Documentado)
                    {
                        //Validamos Estatus de la Parada
                        if ((EstatusParada)this._id_estatus_parada == EstatusParada.Registrado)
                        {
                            //Buscamos si la parada se encuentra como origen de un Movimiento
                            int id_movimiento_origen = Movimiento.BuscamosMovimientoParadaOrigen(this._id_servicio, this._id_parada);

                            //Si es una parada posterior a la primera del servicio
                            if (this._secuencia_parada_servicio > 1)
                            {
                                //Instanciamos Parada Anterior
                                using (Parada objParadaAnterior = new Parada(this.secuencia_parada_servicio - 1, this._id_servicio))
                                {
                                    //Asignamos Fecha de Salida
                                    DateTime fecha_salida_parada_anterior = objParadaAnterior.fecha_salida;

                                    //Si la fecha de llegada es posterior a la fecha de salida de la parada anterior
                                    if (fecha_llegada <= fecha_salida_parada_anterior || fecha_salida_parada_anterior == DateTime.MinValue)
                                        resultado = new RetornoOperacion("La fecha de llegada " + fecha_llegada.ToString("dd/MM/yyyy HH:mm") + " debe ser mayor a la fecha de salida de la parada anterior " + fecha_salida_parada_anterior.ToString("dd/MM/yyyy HH:mm") + ".");
                                }

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Obteniendo el movimiento activo del servicio
                                    using (Movimiento movimientoActual = new Movimiento(Movimiento.BuscamosMovimientoParadaDestino(this._id_servicio, this._id_parada)))
                                    {
                                        //Si el movimiento existe
                                        if (movimientoActual.id_movimiento > 0)
                                            //Terminando el movimiento
                                            resultado = movimientoActual.TerminaMovimiento(fecha_llegada, tipo_actualizacion_estancia_inicio, id_usuario);
                                    }
                                }
                            }
                            //Si es la primer parada del servicio
                            else if (this._secuencia_parada_servicio == 1)
                            {
                                //Validando existencia de movimiento en vacío (resposicionamiento) que tenga como destino esta parada
                                using (Movimiento movimientoVacio = new Movimiento(this._id_servicio, 0))
                                {
                                    //Si el movimiento existe
                                    if (movimientoVacio.id_movimiento > 0)
                                        //Terminando movimiento y sus depenencias
                                        resultado = movimientoVacio.TerminaMovimientoVacio(fecha_llegada, tipo_actualizacion_estancia_inicio, id_usuario);
                                    //Si no hay movimiento
                                    else
                                        resultado = MovimientoAsignacionRecurso.MueveAsignacionesParadaInicial(this._id_parada, fecha_llegada, this._id_servicio, id_usuario);
                                }

                                //Si no hay errores por fin de movimiento previo o actualización de recursos para primer movimiento
                                if (resultado.OperacionExitosa)
                                {
                                    /* DONE: SE RETIRA VALIDACIÓN DE U. MOTRIZ O TERCERO PARA INICIAR SERVICIO (NO PERMITIA PRECARGAS DE REMOLQUE)
                                    //Validamos Existencia de Tercero o Unidad de Motriz
                                    resultado = MovimientoAsignacionRecurso.ValidacionRecursosLlegada(this._id_servicio, this._id_parada);

                                    //Si hay unidades motrices 
                                    if (resultado.OperacionExitosa)
                                    {*/
                                        //Actualizamos Estatus del Servicio a Iniciado
                                        resultado = objServicio.ActualizaEstatusServicio(Servicio.Estatus.Iniciado, id_usuario);

                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Instanciamos Servicio Despacho
                                            using (ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                                                //Actualizar parada de origen en la información de su despacho
                                                resultado = objServicioDespacho.ActualizaParadaOrigenServicio(this._id_parada, fecha_llegada, id_usuario);
                                        }
                                        //Si no se actualizó el estatus del servicio
                                        else
                                            resultado = new RetornoOperacion(string.Format("Error al iniciar servicio: {0}", resultado.Mensaje));
                                    //}
                                }
                            }

                            //Si la parada de destino del movimiento es una parada Operativa y no se encontraron errores previos
                            if (resultado.OperacionExitosa && this.Tipo == Parada.TipoParada.Operativa)
                            {
                                //Buscamos si la parada se encuentra como destino de un Segmento
                                using (SegmentoCarga segmento = new SegmentoCarga(SegmentoCarga.BuscamosSegmentoParadaDestino(this._id_servicio, this._id_parada)))
                                {
                                    //Validamos que exista segmento
                                    if (segmento.id_segmento_carga > 0)
                                        //Terminamos Segmento
                                        resultado = segmento.TerminaSegmento(id_usuario);
                                }
                            }

                            //Si no hay errores hasta este punto
                            if (resultado.OperacionExitosa)
                                //Iniciamos la Parada
                                resultado = IniciaParada(fecha_llegada, tipo_actualizacion_llegada, id_razon_llegada_tarde, id_usuario);
                           
                            //Validamos Resultado pa Envio  de Notificacion
                            if(resultado.OperacionExitosa)
                            {
                                //Instanciamos Ubicación Actual
                                using(Parada objParada = new Parada(this._id_parada))
                                {
                                    //Instanciamos Compañia
                                    using (CompaniaEmisorReceptor objCompaniaEmisor = new CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                                    {
                                        //Intanciamos Ubicacion
                                        using (Ubicacion objUbicacion = new Ubicacion(objParada.id_ubicacion))
                                        {
                                            //Obtenemos Datos del Servicio
                                            using (DataTable mitDatos = SAT_CL.Documentacion.Servicio.ObtieneInformacionLlegaParadaEMail(objServicio.id_servicio, objParada.id_parada))
                                            {
                                                //Enviamos Notificación
                                               // SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor, "NOTIFICA "+ objCompaniaEmisor.nombre_corto + ": ARRIBO A " + objUbicacion.descripcion,
                                               //     objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 1, "Notifica " +objCompaniaEmisor.nombre_corto+ ": Arribo a Parada " + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                               //  "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1,objServicio.id_servicio, "Referencia de Viaje", "No. viaje") +
                                               //"/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación") +
                                               //  SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación")
                                               //  + "/Carta Porte " + objServicio.porte,
                                               //  "", "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                               //  Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + ("RealizadoEMail.png"))),
                                               //  Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                               //  );
                                            }
                                        }
                                    }
                                }
                            }

                            if (resultado.OperacionExitosa)
                            {
                                if (this._secuencia_parada_servicio == 1)
                                {
                                    //Validacion para envio de metodo Genera Viaje MTC
                                    if (resultado.OperacionExitosa)
                                    {
                                        resultado = SAT_CL.Monitoreo.ProveedorWSUnidad.GenerarViajeMTC(this._id_servicio, Convert.ToInt32(this.id_parada));
                                    }
                                    ////Validamos que se realiza la operaciones anteriores para poder mandar los datos WEBSERVICE
                                    if (resultado.OperacionExitosa)
                                    {
                                        //metodo para verificar si cuenta con el proveedor 
                                        resultado = SAT_CL.Monitoreo.ProveedorWSUnidad.GenerarDespachoMTC(this._id_servicio, Convert.ToInt32(this.id_parada));
                                        //resultado = new RetornoOperacion(true);
                                    }
                                }
                            }
                        }
                        //Si el estatus de la parada no permite la edición
                        else
                            //Establecemos mensaje error
                            resultado = new RetornoOperacion("El estatus de la parada no permite su edición");
                    }
                    //Si el estatus del servicio no permite su edición
                    else
                        //Establecemos Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");

                }

                
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                    //Completamos transación
                    scope.Complete();
            }

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de actualizar la Fecha  de Salida de la parada
        /// </summary>
        /// <param name="fecha_salida">Fecha de Salida de la parada</param>
        /// <param name="tipo_actualizacion_parada_salida">Tipo actualización salida de la Parada (Manual, GPS)</param>
        /// <param name="tipo_actualizacion_estancia_fin">Tipo actualización fin de la estancia (Manual, GPS)</param>
        /// <param name="tipo_actualizacion_evento_fin">Tipo actualización fin del evento (Manual, GPS)</param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizarFechaSalida(DateTime fecha_salida, TipoActualizacionSalida tipo_actualizacion_parada_salida, EstanciaUnidad.TipoActualizacionFin tipo_actualizacion_estancia_fin,
                                                    ParadaEvento.TipoActualizacionFin tipo_actualizacion_evento_fin, byte id_razon_salida_tarde, int id_usuario)
        {
            //Declaramos variable Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio
                using (Servicio objServicio = new Servicio(this._id_servicio))
                {
                    //Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Iniciado)
                    {
                        //Validamos Estatus de la parada
                        if ((EstatusParada)this._id_estatus_parada == EstatusParada.Iniciado)
                        {
                            //Buscamos si la parada se encuentra como origen de un Movimiento
                            int id_movimiento = Movimiento.BuscamosMovimientoParadaOrigen(this._id_servicio, this._id_parada);

                            //Determinando si hay un movimiento con origen en la parada actual
                            if (id_movimiento > 0)
                            {
                                //Validamos recursos obligatorios para la salida de la parada (posibles combinaciones entre recursos).
                                resultado = MovimientoAsignacionRecurso.ValidaRecursosSalida(this._id_servicio, this._id_parada);

                                //Si no hay errores en recursos asignados
                                if (resultado.OperacionExitosa)
                                {
                                    //Validamos la Fecha de Salida sea mayor a la Fecha de Llegada 
                                    if (fecha_salida > this._fecha_llegada && this._fecha_llegada != DateTime.MinValue)
                                    {
                                        //Instanciando movimiento
                                        using (Movimiento movimientoActual = new Movimiento(id_movimiento))
                                        {
                                            //Si el movimiento fue encontrado
                                            if (movimientoActual.id_movimiento > 0)
                                            {
                                                //Iniciando movimiento
                                                resultado = movimientoActual.IniciaMovimiento(fecha_salida, tipo_actualizacion_estancia_fin, id_usuario);

                                                //Si se inicia correctamente el movimiento y es una parada operativa
                                                if (resultado.OperacionExitosa && this.Tipo == TipoParada.Operativa)
                                                {
                                                    //Instanciando Segmento
                                                    using (SegmentoCarga segmento = new SegmentoCarga(SegmentoCarga.BuscamosSegmentoParadaOrigen(this._id_servicio, this._id_parada)))
                                                    {
                                                        //Si el segmento se localizó 
                                                        if (segmento.id_segmento_carga > 0)
                                                            //Actualziando su estatus a iniciado
                                                            resultado = segmento.IniciaSegmento(id_usuario);
                                                        //De lo contrario
                                                        else
                                                            resultado = new RetornoOperacion("No pudo ser recuperado el segmento de la parada para su inicio.");
                                                    }                            
                                                }

                                                //Si no hay errores durante el proceso
                                                if (resultado.OperacionExitosa)
                                                    //Terminando la parada
                                                    resultado = TerminaParada(fecha_salida, tipo_actualizacion_parada_salida, tipo_actualizacion_evento_fin, id_razon_salida_tarde, id_usuario);
                                                  //Validamos resultado
                                                if(resultado.OperacionExitosa)
                                                {
                                                    //Instanciamos Ubicación Actual
                                                    using (Parada objParada = new Parada(this._id_parada))
                                                    {
                                                        //Intanciamos Ubicacion
                                                        using (Ubicacion objUbicacion = new Ubicacion(objParada.id_ubicacion))
                                                        {
                                                            //Instanciamos Compañia Emisor
                                                            using (CompaniaEmisorReceptor objCompaniaEmisor = new CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                                                            {
                                                                //Obtenemos Datos del Servicio
                                                                //using (DataTable mitDatos = SAT_CL.Documentacion.Servicio.ObtieneInformacionSalidaParadaEMail(objServicio.id_servicio, objParada.id_parada))
                                                                //{
                                                                //    //Enviamos Notificación
                                                                //    SAT_CL.Notificacion.Notificacion.EnviaCorreoNotificaciones(objServicio.id_compania_emisor,"NOTIFICA " + objCompaniaEmisor.nombre_corto + ": SALIDA DE " + objUbicacion.descripcion,
                                                                //        objServicio.id_cliente_receptor, 15, objParada.id_ubicacion, 2, "Notifica "+ objCompaniaEmisor.nombre_corto  +": Salida de la Parada " + objUbicacion.descripcion, "Servicio: " + objServicio.no_servicio,
                                                                //     "No. Viaje " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1,objServicio.id_servicio, "Referencia de Viaje", "No. viaje") +
                                                                //   "/Confirmación " + SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación") +
                                                                //     SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1,objServicio.id_servicio, "Referencia de Viaje", "Confirmación")
                                                                //     + "/Carta Porte " + objServicio.porte,
                                                                //     "", "", mitDatos, Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "ActualEMail.png")),
                                                                //     Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "RealizadoEMail.png")),
                                                                //     Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "SinRealizarEMail.png")), "idS=" + objServicio.id_servicio.ToString()
                                                                //     );
                                                                //}
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //Si no se pudo encontrar el movimiento
                                            else
                                                resultado = new RetornoOperacion("Error al recuperar movimiento a iniciar.");
                                        }
                                    }
                                    //La fecha de salida no es válida
                                    else
                                        //Establecemos Mesaje Error
                                        resultado = new RetornoOperacion("La fecha de salida " + fecha_salida.ToString("dd/MM/yyyy HH:mm") + " debe ser mayor a la fecha de llegada " + this._fecha_llegada.ToString("dd/MM/yyyy HH:mm") + ".");
                                }                        
                            }
                            //Si es la última parada no hay acciones, sólo una notificación
                            else
                                resultado = new RetornoOperacion("No puede actualizar esta parada ya que es la última del servicio, por favor utilice la funcionalidad para 'Terminar Servicio'.");
                        }
                        //Si la parada no se encuentra iniciada
                        else
                            //Establecemos mensaje error
                            resultado = new RetornoOperacion("La parada no se encuentrá iniciada.");
                    }
                    //Si el servicio no se encuentra iniciado
                    else
                        //Mostramos error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                    //Completamos transacción
                    scope.Complete();                
            }

            //Devolvemos Resultado
            return resultado;
        }
        
        /// <summary>
        /// Realiza la actualización de la fecha de cita de llegada a la parada
        /// </summary>
        /// <param name="cita">Fevha de Cita</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCitaLlegada(DateTime cita, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            int id_parada = 0;
             //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Si la parada permite su edición
                if (this.Estatus == Parada.EstatusParada.Registrado)
                {

                    //Actualizamos Valores correspondientes
                    resultado = this.editaParada(this._id_servicio, this._secuencia_parada_servicio, this.Tipo, (EstatusParada)this._id_estatus_parada, this._id_ubicacion, this._geo_ubicacion, this._descripcion,
                                               cita, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida,
                                               (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this.habilitar);

                    id_parada = resultado.IdRegistro;
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Actualizamos Evento Principal
                        using (ParadaEvento objParadaEvento = new ParadaEvento(ParadaEvento.ObtienerPrimerEvento(this._id_parada)))
                        {
                            //Actualizamos Cita
                            resultado = objParadaEvento.ActualizaCitaEvento(cita.AddMinutes(1), id_usuario);
                        }
                    }
                }
                //De lo contrario
                else
                    resultado = new RetornoOperacion(string.Format("La parada se encuentra en estatus '{0}', no es posible editar la cita de llegada a la misma.", this.Estatus));
                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    //Finalizamos Transaccion
                    scope.Complete();

                    //Establecemos Id Parada
                    resultado = new RetornoOperacion(id_parada);
                }
            }
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Edita una registró parada
        /// </summary>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="descripcion">Descripción del lugar donde la parada se está realizando</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParada(TipoParada tipo_parada, int id_ubicacion, SqlGeography geo_ubicacion, string descripcion, DateTime cita_parada, int id_usuario)
        {
            //Actualizamos Valores restantes
            return this.editaParada(this._id_servicio, this._secuencia_parada_servicio, tipo_parada, (EstatusParada)this._id_estatus_parada, id_ubicacion, geo_ubicacion, descripcion,
                                       cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida,
                                       (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this.habilitar);
        }

        /// <summary>
        /// Edita una registró parada
        /// </summary>
        /// <param name="secuencia">Define la secuencia</param>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="descripcion">Descripción del lugar donde la parada se está realizando</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParada(decimal secuencia, TipoParada tipo_parada, int id_ubicacion, SqlGeography geo_ubicacion, string descripcion, DateTime cita_parada, int id_usuario)
        {

            //Actualizamos Valores restantes
            return this.editaParada(this._id_servicio, secuencia, tipo_parada, (EstatusParada)this._id_estatus_parada, id_ubicacion, geo_ubicacion, descripcion,
                                       cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida,
                                       (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this.habilitar);
        }

        /// <summary>
        /// Edita una registró parada
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Define la secuencia</param>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="descripcion">Descripción del lugar donde la parada se está realizando</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParada(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_ubicacion, SqlGeography geo_ubicacion, string descripcion, DateTime cita_parada, int id_usuario)
        {

            //Actualizamos Valores restantes
            return this.editaParada(id_servicio, secuencia, tipo_parada, (EstatusParada)this._id_estatus_parada, id_ubicacion, geo_ubicacion, descripcion,
                                       cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida,
                                       (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Edita una registró parada
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="secuencia">Define la secuencia</param>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="descripcion">Descripción del lugar donde la parada se está realizando</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="tipo_actualizacion_salida">Tipo Actualización Salida (Manual)</param>
        /// <param name="fecha_salida">Fecha Salida</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParada(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_ubicacion, SqlGeography geo_ubicacion, string descripcion, DateTime cita_parada, TipoActualizacionSalida tipo_actualizacion_salida, DateTime fecha_salida ,int id_usuario)
        {

            //Actualizamos Valores restantes
            return this.editaParada(id_servicio, secuencia, tipo_parada, (EstatusParada)this._id_estatus_parada, id_ubicacion, geo_ubicacion, descripcion,
                                       cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, fecha_salida,
                                       tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this.habilitar);
        }

        /// <summary>
        /// Actualiza la Fecha de Salida
        /// </summary>
        /// <param name="fecha_salida">Fecha y Hora de Salida de la Parada</param>
        /// <param name="tipo_actualizacion_salida">Tipo actualización de la Salida (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaFechaSalida(DateTime fecha_salida, TipoActualizacionSalida tipo_actualizacion_salida, byte id_razon_salida_tarde, int id_usuario)
        {

            //Actualizamos Valores restantes
            return this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, (EstatusParada)this._id_estatus_parada, this._id_ubicacion, this._geo_ubicacion, this._descripcion,
                                       this._cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, fecha_salida,
                                       (TipoActualizacionSalida)tipo_actualizacion_salida, id_razon_salida_tarde, id_usuario, this.habilitar);
        }

        /// <summary>
        /// Cambiamos Estatus de la Parada a Iniciada
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CambiaParadaAIniciada(int id_usuario)
        {

            //Actualizamos Valores restantes
            return this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, EstatusParada.Iniciado, this._id_ubicacion, this._geo_ubicacion, this._descripcion,
                                       this._cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, DateTime.MinValue,
                                       (TipoActualizacionSalida)0, this._id_razon_salida_tarde, id_usuario, this.habilitar);
        }

        /// <summary>
        /// Cambiamos Estatus de la Parada a Registrada
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CambiaParadaARegistrada(int id_usuario)
        {

            //Actualizamos Valores restantes
            return this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, EstatusParada.Registrado, this._id_ubicacion, this._geo_ubicacion, this._descripcion,
                                       this._cita_parada, DateTime.MinValue, (TipoActualizacionLlegada)0, this._id_razon_llegada_tarde, DateTime.MinValue,
                                       (TipoActualizacionSalida)0, this._id_razon_salida_tarde, id_usuario, this.habilitar);
        }

        /// <summary>
        /// Edita una registró parada
        /// </summary>
        /// <param name="id_ubicacion">Id de Ubicación Fisica donde se está realizando la parada</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaParada(int id_ubicacion, DateTime cita_parada, int id_usuario)
        {
            //Declaramos Variable Retorno
            RetornoOperacion resultado = new RetornoOperacion("No pueden existir paradas continuas con la misma ubicación.");
            //Buscamos Parada Anterior y Posterior
            using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(this._id_servicio, this._secuencia_parada_servicio)), objParadaPosterior = new Parada(BuscaParadaPosterior(this._id_servicio, this._secuencia_parada_servicio)))
            {
                //Validamos que no existan paradas anterior con la misma ubicacion
                if (objParadaAnterior._id_ubicacion != id_ubicacion)
                {
                    //Validamos que no existan paradas posterior con la misma ubicacion
                    if (objParadaPosterior._id_ubicacion != id_ubicacion)
                    {
                        //Declaramos la variables a utilizar 
                        string descripcion = "";

                        //Instanciamos  la Ubicación para obtener la descripción.
                        using (Ubicacion objUbicacion = new Ubicacion(id_ubicacion))
                        {
                            //Obtenemos la descripcion de la ubicacion
                            descripcion = objUbicacion.descripcion.ToUpper();
                            //Instanciamos Ciudad
                            using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                            {
                                //Validamos que exista la Ciudad
                                if (objCiudad.id_ciudad > 0)
                                {
                                    //Obtenemos la descripcion de la ubicacion
                                    descripcion += " " + objCiudad.ToString();
                                }
                            }
                        }
                        //Creamos la transacción 
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciamos Servicio
                            using (Documentacion.Servicio objServicio = new Documentacion.Servicio(this._id_servicio))
                            {
                                //Validamos Estatus del Servicio
                                if (objServicio.estatus == Servicio.Estatus.Documentado)
                                {
                                    //Obtenemos resultado de la valiodación de las citas de Paradas
                                    resultado = validacionCitasParadasParaEdicion(this._secuencia_parada_servicio, this._id_servicio, ObtieneTotalParadas(this._id_servicio), cita_parada);

                                    //Validamos Cita de la Parada
                                    if (resultado.OperacionExitosa)
                                    {

                                        //Validamos la existencia de parada de inicio y fin para su edición en encabezado de Servicio
                                        if (ObtieneTotalParadas(this._id_servicio) == this.secuencia_parada_servicio || 1 == this._secuencia_parada_servicio)
                                        {

                                            //Si la parada a Actualizar es la 1 de Carga
                                            if (this._secuencia_parada_servicio == 1)
                                            {

                                                //Editamos Servicio valores de Carga(Ubicación Carga, Cita Carga)
                                                resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, id_ubicacion, cita_parada, objServicio.id_ubicacion_descarga,
                                                                                       objServicio.cita_descarga, objServicio.porte, objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                        id_usuario);
                                            }
                                            else
                                            {
                                                //Editamos Servicio valores de Descarga(Ubicación Descarga, Cita Descarga) 
                                                resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga, id_ubicacion,
                                                                                       cita_parada, objServicio.porte, objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                        id_usuario);
                                            }
                                        }
                                        //Validamos actualización de Servicio
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizamos Valores restantes
                                            resultado = this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, (EstatusParada)this._id_estatus_parada, id_ubicacion, this._geo_ubicacion, descripcion,
                                                                        cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida, 
                                                                        (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this.habilitar);
                                            //Validamos Actualización de la Parada
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Instanciamos Evento
                                                using (ParadaEvento objParadaEvento = new SAT_CL.Despacho.ParadaEvento(ParadaEvento.ObtienerPrimerEvento(this._id_parada)))
                                                {
                                                    //Actualizamos Cita del Primer Evento
                                                    resultado = objParadaEvento.ActualizaCitaEvento(cita_parada.AddMinutes(1), id_usuario);
                                                }
                                            }

                                            //Validamos Edición del Movimiento
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Instanciamos Servicio 
                                                if (objServicio.ActualizaServicio())
                                                {
                                                    // Calculamos Kilometraje del Servicio
                                                    resultado = objServicio.CalculaKilometrajeServicio(id_usuario);
                                                }
                                                else
                                                {
                                                    resultado = new RetornoOperacion("No se puede actualizar los atributos del servicio.");
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    //Establecemos Mensaje Error
                                    resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                                }
                            }
                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Completamos Transacción
                                scope.Complete();
                            }
                        }
                    }
                }
            }


            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Cambia a Estatus Iniciado
        /// </summary>
        /// <param name="fecha_llegada">Fecha y Hora de la Llegada</param>
        /// <param name="tipo_actualizacion_llegada">Medio  que se utilizó  para actualizar la llegada</param>
        /// <param name="id_razon_llegada_tarde">En caso de llegada Tarde, establece la causa</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion IniciaParada(DateTime fecha_llegada, TipoActualizacionLlegada tipo_actualizacion_llegada,
                                            byte id_razon_llegada_tarde, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos Estatus de la Parada
            if (EstatusParada.Iniciado != (EstatusParada)this._id_estatus_parada)
            {
                //Actualzaindo estatus a iniciada y asignando fecha de fin en valor mínimo
                resultado = this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, EstatusParada.Iniciado, this._id_ubicacion, this._geo_ubicacion,
                                            this._descripcion, this._cita_parada, fecha_llegada, (TipoActualizacionLlegada)tipo_actualizacion_llegada, id_razon_llegada_tarde, DateTime.MinValue,
                                            TipoActualizacionSalida.SinActualizar, this._id_razon_salida_tarde, id_usuario, this.habilitar);
            }
            else
                resultado = new RetornoOperacion("La parada ya se encuentra Iniciada");
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Cambia Edita la Fecha de Llegada de la parada
        /// </summary>
        /// <param name="fecha_llegada">Fecha y Hora de la Llegada</param>
        /// <param name="tipo_actualizacion_llegada">Medio  que se utilizó  para actualizar la llegada</param>
        /// <param name="id_razon_llegada_tarde">En caso de llegada Tarde, establece la causa</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaFechaLlegadaParada(DateTime fecha_llegada, TipoActualizacionLlegada tipo_actualizacion_llegada,
                                            byte id_razon_llegada_tarde, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Actualzaindo estatus a iniciada y asignando fecha de fin en valor mínimo
            resultado = this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, (EstatusParada)this._id_estatus_parada, this._id_ubicacion, this._geo_ubicacion,
                                        this._descripcion, this._cita_parada, fecha_llegada, (TipoActualizacionLlegada)tipo_actualizacion_llegada, id_razon_llegada_tarde, this._fecha_salida,
                                       (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, this.habilitar);
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Cambia a Estatus Terminado
        /// </summary>
        /// <param name="fecha_salida">Fecha y Hora de Salida de la Parada</param>
        /// <param name="tipo_actualizacion_salida_parada">Medio  que se utilizó  para actualizar la  salida de la Parada (manual, GPS)</param>
        /// <param name="tipo_actualizacion_fin_evento">Medio  que se utilizó  para actualizar el fin del evento (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaParada(DateTime fecha_salida, TipoActualizacionSalida tipo_actualizacion_salida_parada, ParadaEvento.TipoActualizacionFin tipo_actualizacion_fin_evento,
                                              byte id_razon_salida_tarde, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que la parada a terminar no sea una parada comodin
            if (this.id_servicio > 0 && this._secuencia_parada_servicio > 0)
            {
                // Validamos que el Estatus de la parada este Iniciada
                if ((EstatusParada)this._id_estatus_parada == EstatusParada.Iniciado)
                {
                    //Declaramos fecha fin para el último evento.
                    DateTime fecha_fin_evento = DateTime.MinValue;

                    //Obtenemos Fecha fin del último evento.
                    fecha_fin_evento = ParadaEvento.ObtieneFechaFinUltimoEvento(this._id_parada);

                    //Creamos la transacción 
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Terminamos Eventos de la Parada
                        resultado = TerminaEventosParada(fecha_salida, tipo_actualizacion_fin_evento, id_usuario);

                        //Validamos eventos terminados correctamente
                        if (resultado.OperacionExitosa)
                        {
                            //Si la de Salida es Minima
                            if (Fecha.EsFechaMinima(fecha_salida))
                            {
                                //Validamos fecha del Ultimo Evento
                                if (!Fecha.EsFechaMinima(fecha_fin_evento))
                                
                                    //Asignamos Fecha  Fin del Evento a la parada
                                    fecha_salida = fecha_fin_evento.AddMinutes(1);
                                else
                                    //Asignamos la Fecha Fin de la parada
                                    fecha_salida = fecha_llegada.AddMinutes(1);
                            }
                            else
                            {
                                //Validamos que la fecha fin del ultimo evento  no sea Minima
                                if (!Fecha.EsFechaMinima(fecha_fin_evento))

                                    //Validamos que la fecha de fin del evento sea menor a la fecha de salida de la parada
                                    resultado = Fecha.ValidaFechaEnRango(DateTime.MinValue, fecha_fin_evento, fecha_salida, "La fecha fin del evento '{1}' debe ser menor a la la fecha de salida de la parada '{2}'");
                                else
                                    //Valifamos que la fecha de de llegada de la parada sea menor a la fecha de salida de la parada
                                    resultado = Fecha.ValidaFechaEnRango(DateTime.MinValue, this._fecha_llegada, fecha_salida, "La fecha de llegada de la parada '{1}' debe ser menor a la la fecha de salida de la parada '{2}'");
                            }

                            //Validamos resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Editamos Parada
                                resultado = TerminaParada(fecha_salida, tipo_actualizacion_salida_parada, id_razon_salida_tarde, id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                
                                    //Finalizamos Transacción
                                    scope.Complete();
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("La parada no se encuentrá iniciada.");
            }
            //Si es una parada comodin
            else
                resultado = new RetornoOperacion("La parada no puede ser terminada ya que no pertenece a ningún servicio.");

            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Cambia a Estatus de la parada a Registrado
        /// </summary>
        /// <param name="fecha_salida">Fecha y Hora de Salida de la Parada</param>
        /// <param name="tipo_actualizacion_salida_parada">Medio  que se utilizó  para actualizar la  salida de la Parada (manual, GPS)</param>
        /// <param name="tipo_actualizacion_fin_evento">Medio  que se utilizó  para actualizar el fin del evento (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private RetornoOperacion reversaIniciaParada(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que la parada a reiniciar no sea una parada comodin
            if (this.id_servicio > 0 && this._secuencia_parada_servicio > 0)
            {
                // Validamos que el Estatus de la parada este Iniciada
                if ((EstatusParada)this._id_estatus_parada == EstatusParada.Iniciado)
                {
                    //Actualizando Eventos de la Parada a estatus Registrado
                    resultado = ParadaEvento.EditaParadaEventosParaReversa(this.id_parada, id_usuario);
                                        
                    //Validamos resultado
                    if (resultado.OperacionExitosa)
                        //Editamos Parada
                        resultado = editaParada(this._id_servicio, this._secuencia_parada_servicio, this.Tipo, EstatusParada.Registrado, this._id_ubicacion,
                                        this._geo_ubicacion, this._descripcion, this._cita_parada, DateTime.MinValue, TipoActualizacionLlegada.SinActualizar, 0,
                                        DateTime.MinValue, TipoActualizacionSalida.SinActualizar, 0, id_usuario, this._habilitar);
                    else
                        resultado = new RetornoOperacion(string.Format("Error al actualizar eventos de la parada: '{0}'", resultado.Mensaje));
                    
                }
                //Si el estatus no es válido
                else
                    resultado = new RetornoOperacion("La parada no se encuentrá iniciada.");
            }
            //Si es una parada comodin
            else
                resultado = new RetornoOperacion("La parada no puede ser reiniciada ya que no pertenece a ningún servicio.");

            //Obtenemos Resultado
            return resultado;
        }
        
        /// <summary>
        /// Cambia a Estatus Terminado
        /// </summary>
        /// <param name="fecha_salida">Fecha y Hora de Salida de la Parada</param>
        /// <param name="tipo_actualizacion_salida_parada">Medio  que se utilizó  para actualizar la  salida de la Parada (manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaParada(DateTime fecha_salida, TipoActualizacionSalida tipo_actualizacion_salida_parada, byte id_razon_salida_tarde, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            // Validamos que el Estatus de la parada este Iniciada
            if ((EstatusParada)this._id_estatus_parada == EstatusParada.Iniciado || (EstatusParada)this._id_estatus_parada == EstatusParada.Terminado)
            
                //Editamos Parada
                resultado = this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, EstatusParada.Terminado, this._id_ubicacion, this._geo_ubicacion, this._descripcion,
                                       this._cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, fecha_salida, tipo_actualizacion_salida_parada,
                                       id_razon_salida_tarde, id_usuario, this.habilitar);
            else
                //Instanciando Excepción
                resultado = new RetornoOperacion("No se puede terminar la parada ya que no se encuentrá iniciada.");
            
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Cambia a Estatus Terminado
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaParada(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            // Validamos que el Estatus de la parada este Iniciada
            if ((EstatusParada)this._id_estatus_parada == EstatusParada.Iniciado || (EstatusParada)this._id_estatus_parada == EstatusParada.Terminado)
            {
                //Editamos Parada
                resultado = this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, EstatusParada.Terminado, this._id_ubicacion, this._geo_ubicacion, this._descripcion,
                                       this._cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida, (TipoActualizacionSalida)this._id_tipo_actualizacion_salida,
                                       this._id_razon_salida_tarde, id_usuario, this.habilitar);
            }
            else
            {
                resultado = new RetornoOperacion("No se puede terminar la parada ya que no se encuentrá iniciada.");
            }
            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Deshabilita un Registro Parada en el módulo Documentación
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="TotalParadas">Total Registros Paradas</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParadaServicio(int id_usuario, int TotalParadas)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos Variables
            int id_segmento_actual = 0;
            decimal secuencia_parada_anterior_carga = 0;
            decimal secuencia_parada_posterior_carga = 0;
            int id_parada = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio
                using (Documentacion.Servicio objServicio = new Documentacion.Servicio(this._id_servicio))
                {
                    //Validamos Estatus
                    if (objServicio.estatus == Servicio.Estatus.Documentado)
                    {
                        // 1. Validamos la existencia de mas de 2 paradas para su deshabilitacón
                        if (ObtieneTotalParadas(this._id_servicio) > 2)
                        {
                            //2. Validamos Total de registros actual vs BD
                            if (TotalParadas == ObtieneTotalParadas(this._id_servicio))
                            {
                                //3. Validamos que el Estatus de la Parada este Registrado
                                if ((EstatusParada)this.id_estatus_parada == EstatusParada.Registrado)
                                {
                                    //4. En caso de ser necesario deshabilitamos los segmentos.
                                    if ((TipoParada)this._id_tipo_parada == TipoParada.Operativa)
                                    {
                                        //Buscamos la parada operativa anterior y la parada operativa posterior
                                        using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(id_servicio, this._secuencia_parada_servicio)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(id_servicio, this.secuencia_parada_servicio)))
                                        {
                                            //En caso de eliminar un segmento al inicio
                                            if (objParadaAnteriorCarga.id_parada == 0)
                                            {
                                                //4.1 Instanciamos el Segmento ligado al Id parada actual y la parada posterior actual
                                                using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, this._id_parada, objParadaPosteriorCarga.id_parada)))
                                                {
                                                    //4.2 Deshabilitamos el segmneto de carga
                                                    resultado = objSegmentoCarga.DeshabilitaSegmentoCarga(id_usuario);

                                                    //Validamos Eliminación del segmento 
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //4.3 Decrementamos las secuencia de los segmentos         
                                                        resultado = SegmentoCarga.DecrementaSecuenciaSegmentos(this._id_servicio, 0, id_usuario);

                                                        //Validamos la actualización de los segmentos
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //4.4 Actualizamos la ubicación de Carga del Servicio
                                                            resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objParadaPosteriorCarga.id_ubicacion, objParadaPosteriorCarga.cita_parada
                                                                                                   , objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte
                                                                                                   , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                   id_usuario);

                                                            //Validamos Actualización de Servicio
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //4.5 Instanciamos segmento con la secuencia actual definida para actualización de los movimientos.
                                                                using (SegmentoCarga objSegmento = new SegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia))
                                                                {
                                                                    //Asignamos Valor a las variables
                                                                    id_segmento_actual = objSegmento.id_segmento_carga;
                                                                    secuencia_parada_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                    secuencia_parada_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                    //4.6 Actualizamos el destino del segmento 

                                                                    //4.7 Actualizamos el segmento  de los Movimientos
                                                                }
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                            else
                                                //En caso de eliminar segmento al final
                                                if (objParadaPosteriorCarga.id_parada == 0)
                                                {
                                                    //4.1 Instanciamos el Segmento ligado el id de parada anterior y el Id de parada actual
                                                    using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, objParadaAnteriorCarga.id_parada, this._id_parada)))
                                                    {
                                                        //4.2 Deshabilitamos segmneto de carga
                                                        resultado = objSegmentoCarga.DeshabilitaSegmentoCarga(id_usuario);

                                                        //4.3 Decrementamos las secuencia de los segmentos 

                                                        //Validamos Deshabilitación del Segmneto
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //4.4 Actualizamos la ubicación de Descarga del servicio
                                                            resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga
                                                                                                   , objParadaAnteriorCarga.id_ubicacion, objParadaAnteriorCarga.cita_parada, objServicio.porte
                                                                                                   , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                   id_usuario);

                                                            //Validamos Actualización
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //4.5 Instanciamos segmento anterior para actualización de los movimientos.
                                                                using (SegmentoCarga objSegmento = new SegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia - 1))
                                                                {
                                                                    //Asignamos Valor 
                                                                    id_segmento_actual = objSegmento.id_segmento_carga;
                                                                    secuencia_parada_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                    secuencia_parada_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                    //4.6 Actualizamos el destino del segmento original

                                                                    //4.7 Actualizamos el segmento de los Movimientos
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //En caso de eliminar un segmento en medio

                                                    //4.1 Instanciamos el Segmento ligado el Id de parada actual y pa parada posterior
                                                    using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, this._id_parada, objParadaPosteriorCarga.id_parada)))
                                                    {
                                                        //4.2 Deshabilitamos segmneto de carga
                                                        resultado = objSegmentoCarga.DeshabilitaSegmentoCarga(id_usuario);

                                                        //Validamos Eliminación del segmento 
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //4.3 Decrementamos las secuencia de los segmentos         
                                                            resultado = SegmentoCarga.DecrementaSecuenciaSegmentos(this._id_servicio, objSegmentoCarga.secuencia, id_usuario);

                                                            //Validamos la actualización de segmentos
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //4.4 Actualizamos la ubicación de Descarga del servicio

                                                                //Instanciamos segmento a partir de la secuencia obtenida -1
                                                                using (SegmentoCarga objSegmento = new SegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia - 1))
                                                                {

                                                                    //4.6 Actualizamos el destino del segmento 
                                                                    resultado = objSegmento.EditaSegmentoCarga(objSegmento.id_servicio, objSegmento.secuencia, objSegmento.EstatusSegmento,
                                                                                objSegmento.id_parada_inicio, objParadaPosteriorCarga.id_parada, objSegmento.id_ruta, id_usuario);

                                                                    //Asignamos Valor 
                                                                    id_segmento_actual = objSegmento.id_segmento_carga;
                                                                    secuencia_parada_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                    secuencia_parada_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                    //4.7  Actualizamos el segmento de los Movimientos

                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                        }
                                    }

                                    //Validamos Actualización
                                    if (resultado.OperacionExitosa)
                                    {
                                        //5 Modificamos/Deahabilitamos los movimientos 
                                        using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(this._id_servicio, this._secuencia_parada_servicio)), objParadaPosterior = new Parada(BuscaParadaPosterior(this._id_servicio, this._secuencia_parada_servicio)))
                                        {
                                            //Validamos que no existan paradas anterior con la misma ubicacion
                                            if (objParadaAnterior._id_ubicacion != objParadaPosterior._id_ubicacion)
                                            {
                                                //En caso de deshabilitar un movimiento al inicio
                                                if (objParadaAnterior.id_parada == 0)
                                                {
                                                    //5.1 Instanciamos Movimiento para su eliminación
                                                    using (Movimiento objMovimiento = new Movimiento(this._id_parada, objParadaPosterior.id_parada, this._id_servicio))
                                                    {
                                                        //5.2 Deshabilitamos Movimiento.
                                                        resultado = objMovimiento.DeshabilitaMovimiento(id_usuario);

                                                        //Validamos Deshabilitación de los movimientos
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //5.3 Actaulizamos las secuencias
                                                            resultado = Movimiento.DecrementaSecuenciaMovimientos(id_servicio, 0, id_usuario);

                                                            //Validamos Actualización de Movimientos
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Asignamos Valor del Id de Segmento dependiendo del Tipo actualmente definido a la parada.
                                                                if (objParadaPosterior.Tipo == TipoParada.Servicio && (Parada.TipoParada)this.Tipo == TipoParada.Operativa)
                                                                {

                                                                    //5.4 Actualizamos Segmento de Movimiento 
                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, this.secuencia_parada_servicio, secuencia_parada_posterior_carga, 0, id_usuario);

                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //En caso de insertarse un movimiento al final
                                                    if (objParadaPosterior.id_parada == 0)
                                                    {
                                                        //5.1 Instanciamos Movimiento
                                                        using (Movimiento objMovimiento = new Movimiento(objParadaAnterior.id_parada, this._id_parada, this._id_servicio))
                                                        {
                                                            //5.2 Deshabilitamos Movimiento.
                                                            resultado = objMovimiento.DeshabilitaMovimiento(id_usuario);

                                                            //5.3 Actualizamos secuencias.

                                                            //Validamos Actualización de Movimientos
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Asignamos Valor del Id de Segmento dependiendo del Tipo actualmente definido a la parada.
                                                                if (objParadaAnterior.Tipo == TipoParada.Servicio && (Parada.TipoParada)this.Tipo == TipoParada.Operativa)
                                                                {
                                                                    id_segmento_actual = 0;
                                                                    //5.4 Actualizamos Segmento de Movimiento 
                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, secuencia_parada_anterior_carga, this._secuencia_parada_servicio, 0, id_usuario);


                                                                }
                                                            }
                                                        }
                                                    }
                                                    //En caso de eliminar un movimiento intermedio.
                                                    else
                                                    {
                                                        //5.1 Instanciamos Movimiento para su eliminación
                                                        using (Movimiento objMovimiento = new Movimiento(this.id_parada, objParadaPosterior.id_parada, id_servicio))
                                                        {
                                                            //5.2 Dehabilitamos Movimiento
                                                            resultado = objMovimiento.DeshabilitaMovimiento(id_usuario);

                                                            //Validamos la eliminación del Movimiento
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //5.3 Actualizamos Secuencias de los Movimientos
                                                                resultado = Movimiento.DecrementaSecuenciaMovimientos(objMovimiento.id_servicio, objMovimiento.secuencia_servicio, id_usuario);

                                                                //Validamos Actualización de secuencias
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //5.4 Instanciamos movimiento anterior para su edición
                                                                    using (Movimiento objMovimientoAnterior = new Movimiento(objParadaAnterior.id_parada, this._id_parada, this._id_servicio))
                                                                    {
                                                                        //5.5 Actualizamos destino del movimiento obtenido
                                                                        resultado = objMovimientoAnterior.EditaMovimiento(objMovimientoAnterior.id_servicio, objMovimientoAnterior.id_segmento_carga,
                                                                                        objMovimientoAnterior.secuencia_servicio, (Movimiento.Estatus)objMovimientoAnterior.id_estatus_movimiento,
                                                                                        (Movimiento.Tipo)objMovimientoAnterior.id_tipo_movimiento, 0, 0, objMovimientoAnterior.id_compania_emisor,
                                                                                        objMovimientoAnterior.id_parada_origen, objParadaPosterior.id_parada, id_usuario);

                                                                        //5.6 En caso de ser parada Operativa
                                                                        if ((TipoParada)this.Tipo == TipoParada.Operativa)
                                                                        {
                                                                            //Validamos existencia de parada anterior operativa
                                                                            if (secuencia_parada_anterior_carga == 0)
                                                                            {
                                                                                //Actualizamos el segmentos de los movimientos 0
                                                                                resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, this._secuencia_parada_servicio, secuencia_parada_posterior_carga, 0, id_usuario);
                                                                            }
                                                                            else
                                                                            {
                                                                                //Validamos Existencia de parada posterior operativa
                                                                                if (secuencia_parada_posterior_carga == 0)
                                                                                {
                                                                                    //Actualizamos el segmento de los Movimientos 0
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, secuencia_parada_anterior_carga, this._secuencia_parada_servicio + 1, 0, id_usuario);
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Actualizamos el segmento de los Movimientos con el segento actual definido
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, this._secuencia_parada_servicio, secuencia_parada_posterior_carga, id_segmento_actual, id_usuario);

                                                                                }

                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                            }
                                            else
                                            {
                                                //Establecmeos Error
                                                resultado = new RetornoOperacion("No es posible la eliminaciónn de la parada dado que, no puede existir paradas continuas con la misma ubicación.");
                                            }
                                        }
                                    }

                                    //Validamos Eliminación de Movimientos
                                    if (resultado.OperacionExitosa)
                                    {
                                        //6.Cargamos Eventos ligados a un Id de Parada
                                        resultado = ParadaEvento.DeshabilitaParadaEventos(this._id_parada, id_usuario);

                                        //Validamos Deshabilitación de Eventos.
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Deshabilitando relaciones de producto y la parada
                                            resultado = ServicioProducto.DeshabilitaProductosParada(this._id_parada, id_usuario);
                                            
                                            //Si no hay errores
                                            if (resultado.OperacionExitosa)
                                            {
                                                //7. Deshabilitamos parada 
                                                resultado = DeshabilitaParada(id_usuario);

                                                //Asignamos id de la parada
                                                id_parada = resultado.IdRegistro;

                                                //Validamos deshabilitación de parada
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //7.1 Actualizamos las secuencias
                                                    resultado = DecrementaSecuenciaParadas(this._id_servicio, this._secuencia_parada_servicio, id_usuario);

                                                    //Validamos Actualización Secuencias de las Paradas
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Instanciamos Servicio 
                                                        if (objServicio.ActualizaServicio())
                                                        {
                                                            //7.2 Calculamos Kilometraje del Servicio
                                                            resultado = objServicio.CalculaKilometrajeServicio(id_usuario);

                                                            //Validamos actualización de paradas
                                                            if (resultado.OperacionExitosa)
                                                            {

                                                                //Devolvemos id de parada
                                                                resultado = new RetornoOperacion(id_parada);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            resultado = new RetornoOperacion("No se puede actualizar los atributos del servicio.");
                                                        }

                                                    }
                                                }
                                            }
                                            //Si no se deshabilita el producto de la parada
                                            else
                                                resultado = new RetornoOperacion(string.Format("Error al eliminar productos de la parada {0}: {1}", this._id_parada, resultado.Mensaje));
                                        }
                                    }
                                }//Validación Estatus
                                else
                                {
                                    resultado = new RetornoOperacion("El estatus de la parada no permite su eliminación.");
                                }
                            }
                            else
                            {
                                //Establecmeos Error
                                resultado = new RetornoOperacion("No se puede eliminar la parada ya que fue modificada desde la última vez que fue consultada.");
                            }
                        }
                        else
                        {
                            //Establecmeos Error
                            resultado = new RetornoOperacion("El número de paradas mínimas es dos. Imposible eliminar la parada.");
                        }
                    }
                    else
                    {
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Instanciando servicio del movimiento
                    using (Documentacion.Servicio srv = new Documentacion.Servicio(this._id_servicio))
                    {
                        //Realizando actualización de plataforma de terceros (proveedor satelital)
                        srv.ActualizaPlataformaTerceros(id_usuario);
                    }

                    //Fin Transacción
                    scope.Complete();
                }

            }
            
            //Devolviendo resultado
            return resultado;
        }


        /// <summary>
        /// Deshabilita un Registro Parada
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="TotalParadas">Total Registros Paradas</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParadaDespacho(int id_usuario, int TotalParadas)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos Variables
            int id_segmento_actual = 0;
            decimal secuencia_parada_anterior_carga = 0;
            decimal secuencia_parada_posterior_carga = 0;
            int id_parada = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio 
                using (Documentacion.Servicio objServicio = new Documentacion.Servicio(this._id_servicio))
                {
                    //Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Iniciado)
                    {
                        //1. Validamos que la parada se encuentre en estatus Registrado
                        if ((EstatusParada)this._id_estatus_parada == EstatusParada.Registrado)
                        {
                            //Obtenemos Parada Anterior
                            using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(this._id_servicio, this._secuencia_parada_servicio)), objParadaPosterior = new Parada(BuscaParadaPosterior(this._id_servicio, this._secuencia_parada_servicio)))
                            {
                                //Validamos que no existan paradas anterior con la misma ubicacion
                                if (objParadaAnterior._id_ubicacion != objParadaPosterior.id_ubicacion)
                                {
                                    //2. Validamos que no exista fecha de salida de la PA para la parada final, en caso de ser parada inicial y en medio no válida la fecha de salida.
                                    if (((!Fecha.EsFechaMinima(objParadaAnterior.fecha_salida) && this._secuencia_parada_servicio != 1 && objParadaPosterior.id_parada != 0) || (Fecha.EsFechaMinima(objParadaAnterior.fecha_salida) && this._secuencia_parada_servicio != 1 && objParadaPosterior.id_parada != 0))
                                        || (Fecha.EsFechaMinima(objParadaAnterior.fecha_salida) && this._secuencia_parada_servicio == 1) || (objParadaPosterior.id_parada == 0 && Fecha.EsFechaMinima(objParadaAnterior.fecha_salida)))
                                    {
                                        //3. Instanciamos Movimiento 
                                        using (Movimiento objMovimientoFinal = new Movimiento(objParadaAnterior.id_parada, this._id_parada, this._id_servicio), objMovimientoInicio = new Movimiento(this._id_parada, objParadaPosterior.id_parada, this._id_servicio))
                                        {
                                            //4. Validamos Recursos asignados al Movimiento
                                            resultado = Movimiento.ValidacionAnticiposPorMovimiento(objServicio.id_servicio,objMovimientoInicio,
                                                                                             objMovimientoFinal, objParadaAnterior, objParadaPosterior, id_usuario);
                                            //Validamos Resultado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //5. Validamos Estancias Activas ligadas a la parada
                                                resultado = EstanciaUnidad.ValidaExistenciaEstanciasParada(this._id_parada);

                                                if (resultado.OperacionExitosa)
                                                {
                                                    //6. Validamos la existencia de mas de 2 paradas para su deshabilitacón
                                                    if (ObtieneTotalParadas(this._id_servicio) > 2)
                                                    {
                                                        //7. Validamos Total de registros actual vs BD
                                                        if (TotalParadas == ObtieneTotalParadas(this._id_servicio))
                                                        {
                                                            //8. Validamos que el Estatus de la Parada este Registrado
                                                            if ((EstatusParada)this.id_estatus_parada == EstatusParada.Registrado)
                                                            {
                                                                //9. En caso de ser necesario deshabilitamos los segmentos.
                                                                if ((TipoParada)this._id_tipo_parada == TipoParada.Operativa)
                                                                {
                                                                    //Buscamos la parada operativa anterior y la parada operativa posterior
                                                                    using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(id_servicio, this._secuencia_parada_servicio)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(id_servicio, this.secuencia_parada_servicio)))
                                                                    {
                                                                        //En caso de eliminar un segmento al inicio
                                                                        if (objParadaAnteriorCarga.id_parada == 0)
                                                                        {
                                                                            //9.1 Instanciamos el Segmento ligado al Id parada actual y la parada posterior actual
                                                                            using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, this._id_parada, objParadaPosteriorCarga.id_parada)))
                                                                            {
                                                                                //9.2 Deshabilitamos el segmneto de carga
                                                                                resultado = objSegmentoCarga.DeshabilitaSegmentoCarga(id_usuario);

                                                                                //Validamos Eliminación del segmento 
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Actualizamos Eviencias
                                                                                    resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaDeshabilitaParada(this._id_servicio, 0, resultado.IdRegistro, id_usuario);

                                                                                    //Validamos Resultado
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //9.3 Decrementamos las secuencia de los segmentos         
                                                                                        resultado = SegmentoCarga.DecrementaSecuenciaSegmentos(this._id_servicio, 0, id_usuario);

                                                                                        //Validamos la actualización de los segmentos
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //9.4 Actualizamos la ubicación de Carga del Servicio
                                                                                            resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objParadaPosteriorCarga.id_ubicacion, objParadaPosteriorCarga.cita_parada
                                                                                                                                   , objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte
                                                                                                                                   , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                                   id_usuario);

                                                                                            //Validamos Actualización de Servicio
                                                                                            if (resultado.OperacionExitosa)
                                                                                            {
                                                                                                //9.5 Instanciamos segmento con la secuencia actual definida para actualización de los movimientos.
                                                                                                using (SegmentoCarga objSegmento = new SegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia))
                                                                                                {
                                                                                                    //Asignamos Valor a las variables
                                                                                                    id_segmento_actual = objSegmento.id_segmento_carga;
                                                                                                    secuencia_parada_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                                                    secuencia_parada_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                                                    //9.6 Actualizamos el destino del segmento 

                                                                                                    //9.7 Actualizamos el segmento  de los Movimientos
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                            //En caso de eliminar segmento al final
                                                                            if (objParadaPosteriorCarga.id_parada == 0)
                                                                            {
                                                                                //9.1 Instanciamos el Segmento ligado el id de parada anterior y el Id de parada actual
                                                                                using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, objParadaAnteriorCarga.id_parada, this._id_parada)))
                                                                                {
                                                                                    //9.2 Deshabilitamos segmneto de carga
                                                                                    resultado = objSegmentoCarga.DeshabilitaSegmentoCarga(id_usuario);


                                                                                    //9.3 Decrementamos las secuencia de los segmentos 

                                                                                    //Validamos Deshabilitación del Segmneto
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //Actualizamos Control Evidencia
                                                                                        resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaDeshabilitaParada(this._id_servicio, resultado.IdRegistro, 0, id_usuario);

                                                                                        //Validamos Resultado
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //9.4 Actualizamos la ubicación de Descarga del servicio
                                                                                            resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga
                                                                                                                                   , objParadaAnteriorCarga.id_ubicacion, objParadaAnteriorCarga.cita_parada, objServicio.porte
                                                                                                                                   , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                                   id_usuario);
                                                                                            //Validamos Actualización
                                                                                            if (resultado.OperacionExitosa)
                                                                                            {
                                                                                                //9.5 Instanciamos segmento anterior para actualización de los movimientos.
                                                                                                using (SegmentoCarga objSegmento = new SegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia - 1))
                                                                                                {
                                                                                                    //Asignamos Valor 
                                                                                                    id_segmento_actual = objSegmento.id_segmento_carga;
                                                                                                    secuencia_parada_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                                                    secuencia_parada_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                                                    //9.6 Actualizamos el destino del segmento original

                                                                                                    //9.7 Actualizamos el segmento de los Movimientos
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                //En caso de eliminar un segmento en medio

                                                                                //9.1 Instanciamos el Segmento ligado el Id de parada actual y pa parada posterior
                                                                                using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, this._id_parada, objParadaPosteriorCarga.id_parada)))
                                                                                {
                                                                                    //9.2 Deshabilitamos segmneto de carga
                                                                                    resultado = objSegmentoCarga.DeshabilitaSegmentoCarga(id_usuario);

                                                                                    int idSegmentoPos = 0;
                                                                                    //Validamos Eliminación del segmento 
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        idSegmentoPos = resultado.IdRegistro;
                                                                                        //9.3 Decrementamos las secuencia de los segmentos         
                                                                                        resultado = SegmentoCarga.DecrementaSecuenciaSegmentos(this._id_servicio, objSegmentoCarga.secuencia, id_usuario);

                                                                                        //Validamos la actualización de segmentos
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //9.4 Actualizamos la ubicación de Descarga del servicio

                                                                                            //Instanciamos segmento a partir de la secuencia obtenida -1
                                                                                            using (SegmentoCarga objSegmento = new SegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia - 1))
                                                                                            {

                                                                                                //9.5 Actualizamos el destino del segmento 
                                                                                                resultado = objSegmento.EditaSegmentoCarga(objSegmento.id_servicio, objSegmento.secuencia, objSegmento.EstatusSegmento,
                                                                                                            objSegmento.id_parada_inicio, objParadaPosteriorCarga.id_parada, objSegmento.id_ruta, id_usuario);

                                                                                                //Asignamos Valor 
                                                                                                id_segmento_actual = objSegmento.id_segmento_carga;
                                                                                                secuencia_parada_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                                                secuencia_parada_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                                                //9.6  Actualizamos el segmento de los Movimientos

                                                                                                //Validamos Resultado
                                                                                                if (resultado.OperacionExitosa)
                                                                                                {
                                                                                                    //Actualizamos Secuencia
                                                                                                    resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaDeshabilitaParada(this._id_servicio, resultado.IdRegistro, idSegmentoPos, id_usuario);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                    }
                                                                }
                                                                //Validamos Actualización
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //10. En caso de deshabilitar un movimiento al inicio
                                                                    if (objParadaAnterior.id_parada == 0)
                                                                    {
                                                                        //10.1 Deshabilitamos Movimiento.
                                                                        resultado = objMovimientoInicio.DeshabilitaMovimiento(id_usuario);

                                                                        //Validamos Deshabilitación de los movimientos
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //10.2 Actaulizamos las secuencias
                                                                            resultado = Movimiento.DecrementaSecuenciaMovimientos(id_servicio, 0, id_usuario);

                                                                            //Validamos Actualización de Movimientos
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Asignamos Valor del Id de Segmento dependiendo del Tipo actualmente definido a la parada.
                                                                                if (objParadaPosterior.Tipo == TipoParada.Servicio && (Parada.TipoParada)this.Tipo == TipoParada.Operativa)
                                                                                {

                                                                                    //10.3 Actualizamos Segmento de Movimiento 
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, this.secuencia_parada_servicio, secuencia_parada_posterior_carga, 0, id_usuario);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                        //10 En caso de insertarse un movimiento al final
                                                                        if (objParadaPosterior.id_parada == 0)
                                                                        {
                                                                            //10.1 Deshabilitamos Movimiento.
                                                                            resultado = objMovimientoFinal.DeshabilitaMovimiento(id_usuario);

                                                                            //10.2 Actualizamos secuencias.

                                                                            //Validamos Actualización de Movimientos
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Asignamos Valor del Id de Segmento dependiendo del Tipo actualmente definido a la parada.
                                                                                if (objParadaAnterior.Tipo == TipoParada.Servicio && (Parada.TipoParada)this.Tipo == TipoParada.Operativa)
                                                                                {
                                                                                    id_segmento_actual = 0;
                                                                                    //10.3 Actualizamos Segmento de Movimiento 
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, secuencia_parada_anterior_carga, this._secuencia_parada_servicio, 0, id_usuario);
                                                                                }
                                                                            }
                                                                            //Validamos que la fecha de la parada anterior exista para la actualización de estatus de las Unidades
                                                                            if (Fecha.EsFechaMinima(objParadaAnterior.fecha_llegada))
                                                                            {
                                                                                //Obtenemos Movimiento Destino
                                                                                using (Movimiento objMovimientoDestino = new Movimiento(Movimiento.BuscamosMovimientoParadaDestino(this._id_servicio,
                                                                                                                       this._id_parada)))
                                                                                {

                                                                                }
                                                                            }
                                                                        }
                                                                        //En caso de eliminar un movimiento intermedio.
                                                                        else
                                                                        {
                                                                            //10.1 Dehabilitamos Movimiento
                                                                            resultado = objMovimientoInicio.DeshabilitaMovimiento(id_usuario);

                                                                            //Validamos la eliminación del Movimiento
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //10.2 Actualizamos Secuencias de los Movimientos
                                                                                resultado = Movimiento.DecrementaSecuenciaMovimientos(objMovimientoInicio.id_servicio,
                                                                                    objMovimientoInicio.secuencia_servicio, id_usuario);

                                                                                //Validamos Actualización de secuencias
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //10.3 Actualizamos destino del movimiento obtenido
                                                                                    resultado = objMovimientoFinal.EditaMovimiento(objMovimientoFinal.id_servicio, objMovimientoFinal.id_segmento_carga,
                                                                                                    objMovimientoFinal.secuencia_servicio, (Movimiento.Estatus)objMovimientoFinal.id_estatus_movimiento,
                                                                                                    (Movimiento.Tipo)objMovimientoFinal.id_tipo_movimiento, 0, 0, objMovimientoFinal.id_compania_emisor,
                                                                                                    objMovimientoFinal.id_parada_origen, objParadaPosterior.id_parada, id_usuario);

                                                                                    //10.4 En caso de ser parada Operativa
                                                                                    if ((TipoParada)this.Tipo == TipoParada.Operativa)
                                                                                    {
                                                                                        //Validamos existencia de parada anterior operativa
                                                                                        if (secuencia_parada_anterior_carga == 0)
                                                                                        {
                                                                                            //Actualizamos el segmentos de los movimientos 0
                                                                                            resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, this._secuencia_parada_servicio, secuencia_parada_posterior_carga, 0, id_usuario);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Validamos Existencia de parada posterior operativa
                                                                                            if (secuencia_parada_posterior_carga == 0)
                                                                                            {
                                                                                                //Actualizamos el segmento de los Movimientos 0
                                                                                                resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, secuencia_parada_anterior_carga, this._secuencia_parada_servicio + 1, 0, id_usuario);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                //Actualizamos el segmento de los Movimientos con el segento actual definido
                                                                                                resultado = Movimiento.ActualizaSegmentoDeMovimientos(this._id_servicio, this._secuencia_parada_servicio, secuencia_parada_posterior_carga, id_segmento_actual, id_usuario);

                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                }

                                                                //Validamos Eliminación de Movimientos
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //11.Cargamos Eventos ligados a un Id de Parada
                                                                    resultado = ParadaEvento.DeshabilitaParadaEventos(this._id_parada, id_usuario);

                                                                    //Validamos Deshabilitación de Eventos.
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Deshabilitando relaciones de producto y la parada
                                                                        resultado = ServicioProducto.DeshabilitaProductosParada(this._id_parada, id_usuario);

                                                                        //Si no hay errores
                                                                        if (resultado.OperacionExitosa)
                                                                        {

                                                                            //12. Deshabilitamos parada 
                                                                            resultado = this.editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, (EstatusParada)this.id_estatus_parada, this._id_ubicacion, this._geo_ubicacion,
                                                                                               this._descripcion, this._cita_parada, this._fecha_llegada, (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida,
                                                                                              (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, false);

                                                                            //Asignamos id de la parada
                                                                            id_parada = resultado.IdRegistro;

                                                                            //Validamos deshabilitación de parada
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //12.1 Actualizamos las secuencias
                                                                                resultado = DecrementaSecuenciaParadas(this._id_servicio, this._secuencia_parada_servicio, id_usuario);

                                                                                //Validamos Actualización Secuencias de las Paradas
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Actualizamos Servicio 
                                                                                    if (objServicio.ActualizaServicio())
                                                                                    {
                                                                                        //12.2 Calculamos Kilometraje del Servicio
                                                                                        resultado = objServicio.CalculaKilometrajeServicio(id_usuario);

                                                                                        //Validamos actualización de paradas
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {

                                                                                            //Devolvemos id de parada
                                                                                            resultado = new RetornoOperacion(id_parada);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Establecemos Error
                                                                                        resultado = new RetornoOperacion("Imposible actualizar los atributos del Servicio.");
                                                                                    }

                                                                                }
                                                                            }
                                                                        }
                                                                        //Si no se deshabilita el producto de la parada
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("Error al eliminar productos de la parada {0}: {1}", this._id_parada, resultado.Mensaje));
                                                                    }
                                                                }
                                                            }//Validacíón Estatus
                                                            else
                                                            {
                                                                resultado = new RetornoOperacion("El estatus de la parada no permite su eliminación.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Establecmeos Error
                                                            resultado = new RetornoOperacion("No se puede eliminar la parada ya que fue modificada desde la última vez que fue consultada.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Establecmeos Error
                                                        resultado = new RetornoOperacion("El número de paradas mínimas son 2. Imposible eliminar la parada.");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Establecemos Errors
                                        resultado = new RetornoOperacion("Es necesario quitar la fecha de salida de la parada anterior.");
                                    }
                                }
                                else
                                {
                                    //Establecmeos Error
                                    resultado = new RetornoOperacion("No es posible la eliminación de la parada dado que, no puede existir paradas continuas con la misma ubicación.");

                                }
                            }
                        }
                        else
                        {
                            //Establecemos Mensaje Error
                            resultado = new RetornoOperacion("Es necesario que la parada se encuentre en estatus Registrada.");
                        }
                    }//Validacion estatus del Servicio
                    else
                    {
                        //Establecemos Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Instanciando servicio del movimiento
                    using (Documentacion.Servicio srv = new Documentacion.Servicio(this._id_servicio))
                    {
                        //Realizando actualización de plataforma de terceros (proveedor satelital)
                        srv.ActualizaPlataformaTerceros(id_usuario);
                    }

                    //Validamos Transacción
                    scope.Complete();
                }
            }//Fin Transacción
            return resultado;
        }


        /// <summary>
        /// Carga Paradas a partir de un Id servicio y mayores a una secuencia
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <returns></returns>
        public static DataTable CargaParadasParaDecrementarSecuencias(int id_servicio, decimal secuencia)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 6, 0, id_servicio, secuencia, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };


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
        /// Carga Paradas a partir de un Id servicio y mayores o igual a una secuencia
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <returns></returns>
        public static DataTable CargaParadasParaActualizarSecuencias(int id_servicio, decimal secuencia)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 7, 0, id_servicio, secuencia, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };


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
        /// Búsca Parada Anterior Operativa  a partir de un servicio y una secuencia
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="secuencia">secuencia</param>
        /// <returns></returns>
        public static int BuscaParadaAnteriorOperativa(int id_servicio, decimal secuencia)
        {
            //Declaramos Resultados
            int IdParadaAnteriorCarga = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 8, 0, id_servicio, secuencia, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Parada Anterior
                    IdParadaAnteriorCarga = (from DataRow r in ds.Tables[0].Rows
                                             select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return IdParadaAnteriorCarga;
        }

        /// <summary>
        /// Búsca Parada Posterior Operativa  a partir de un servicio y una secuencia
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="secuencia">secuencia </param>
        /// <returns></returns>
        public static int BuscaParadaPosteriorOperativa(int id_servicio, decimal secuencia)
        {
            //Declaramos Resultados
            int IdParadaPosteriorCarga = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 9, 0, id_servicio, secuencia, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Parada Posterior
                    IdParadaPosteriorCarga = (from DataRow r in ds.Tables[0].Rows
                                              select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return IdParadaPosteriorCarga;
        }

        /// <summary>
        /// Búsca Parada Anterior a partir de un servicio y una secuencia
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="secuencia">secuencia</param>
        /// <returns></returns>
        public static int BuscaParadaAnterior(int id_servicio, decimal secuencia)
        {
            //Declaramos Resultados
            int IdParadaAnteriorActual = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 10, 0, id_servicio, secuencia, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Parada Anterior
                    IdParadaAnteriorActual = (from DataRow r in ds.Tables[0].Rows
                                              select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return IdParadaAnteriorActual;
        }

        /// <summary>
        /// Búsca Parada Posterior  a partir de un servicio y una secuencia 
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="secuencia">secuencia </param>
        /// <returns></returns>
        public static int BuscaParadaPosterior(int id_servicio, decimal secuencia)
        {
            //Declaramos Resultados
            int IdParadaPosteriorActual = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 11, 0, id_servicio, secuencia, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Parada Posterior
                    IdParadaPosteriorActual = (from DataRow r in ds.Tables[0].Rows
                                               select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }
            }
            //Obtenemos Resultado
            return IdParadaPosteriorActual;
        }

        /// <summary>
        /// Carga Paradas a partir de un Id servicio 
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaParadasParaVisualizacion(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 12, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };


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
        /// Carga Paradas a partir de un Id servicio, recuperando los datos necesarios para copiar a un nuevo registro
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaParadasParaCopia(int id_servicio)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 15, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de retorno
                DataTable mit = null;

                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de Validar si se puede Terminar el Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaTerminoServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion validacion = new RetornoOperacion();

            //inicializamos el arreglo de parametros
            object[] param = { 15, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obteniendo Paradas Iniciadas
                    List<DataRow> stops = (from DataRow dr in ds.Tables["Table"].AsEnumerable()
                                           where (EstatusParada)Convert.ToInt32(dr["IdEstatus"]) == EstatusParada.Iniciado ||
                                           (EstatusParada)Convert.ToInt32(dr["IdEstatus"]) == EstatusParada.Registrado
                                           select dr).ToList();

                    //Obteniendo Paradas Iniciadas
                    decimal ultima_sec = (from DataRow dr in ds.Tables["Table"].AsEnumerable()
                                          select Convert.ToDecimal(dr["Secuencia"])).Max();

                    //Obteniendo Ultima Parada
                    DataRow last_stop = (from DataRow dr in ds.Tables["Table"].AsEnumerable()
                                         where Convert.ToInt32(dr["Secuencia"]) == ultima_sec
                                         select dr).FirstOrDefault();

                    //Validando que existan Paradas
                    if (stops.Count > 0)
                    {
                        //Validando que existan Paradas
                        if (stops.Count == 1)
                        {
                            //Validando Ultima Parada
                            if (last_stop != null)
                            {
                                //Validando que sea la Ultima Parada
                                if (Convert.ToInt32(last_stop["Id"]) == Convert.ToInt32(stops[0]["Id"]))

                                    //Instanciando Resultado Positivo
                                    validacion = new RetornoOperacion(id_servicio, "Servicio listo para Terminarse", true);
                                else
                                    //Instanciando Excepción
                                    validacion = new RetornoOperacion("Tienes paradas pendientes por Terminar, aún no puedes terminar el Servicio.");
                            }
                            else
                                //Instanciando Excepción
                                validacion = new RetornoOperacion("No se puede obtener la ultima parada.");
                        }
                        else
                            //Instanciando Excepción
                            validacion = new RetornoOperacion("Tienes paradas pendientes por Terminar, aún no puedes terminar el Servicio.");
                    }
                    else
                        //Instanciando Excepción
                        validacion = new RetornoOperacion(id_servicio, "El Servicio no tiene paradas Iniciadas/Registradas.", true);
                }
            }

            //Devolviendo Resultado Obtenido
            return validacion;
        }

        /// <summary>
        /// Decrementa la secuencia de las Paradas.
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia que se utliliza como referencia para actualizar las secuencias  posteriores a la misma</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DecrementaSecuenciaParadas(int id_servicio, decimal secuencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Realizando la consulta
            using (DataTable mit = CargaParadasParaDecrementarSecuencias(id_servicio, secuencia))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Recorremos cada una de las paradas
                    foreach (DataRow r in mit.Rows)
                    {
                        //Validamos deshabilitación
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Parada
                            using (Parada objParada = new Parada(r.Field<int>("Id")))
                            {
                                resultado = objParada.EditaParada(objParada.secuencia_parada_servicio - 1, (TipoParada)objParada.Tipo, objParada.id_ubicacion,
                                                                 objParada.geo_ubicacion, objParada.descripcion, objParada.cita_parada, id_usuario);
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
        /// Terminamos Eventos de la Parada
        /// </summary>
        /// <param name="tipo_actualizacion_evento_fin">tipo actualización fin del evento (Manual, GPS)</param>
        /// <param name="fecha_salida">fecha salida de la parada</param>
        /// <param name="id_usuario">id usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaEventosParada(DateTime fecha_salida, ParadaEvento.TipoActualizacionFin tipo_actualizacion_evento_fin, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Obtenemos Evento ligado de la Parada
            using (DataTable mit = ParadaEvento.CargaEventosParaActualizacionParada(this._id_parada))
            {
                //Validamos que sea Parada operativa
                if ((TipoParada)this._id_tipo_parada == TipoParada.Operativa)
                {
                    //Validamos Origen Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recorremos cada una de los Eventos
                        foreach (DataRow r in mit.Rows)
                        {
                            //Validamos actaualización de evento
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos Evento
                                using (ParadaEvento objParadaEvento = new ParadaEvento(r.Field<int>("Id")))
                                {
                                    //Validamos Estatus del Evento
                                    if ((ParadaEvento.EstatusParadaEvento)objParadaEvento.id_estatus == ParadaEvento.EstatusParadaEvento.Iniciado)
                                    {
                                        //Terminamos Evento
                                        resultado = objParadaEvento.TerminaParadaEvento(objParadaEvento.inicio_evento, tipo_actualizacion_evento_fin,
                                                      0, objParadaEvento.inicio_evento.AddMinutes(1), id_usuario);
                                    }
                                    else
                                    {
                                        //Validamos Estatus del Evento
                                        if ((ParadaEvento.EstatusParadaEvento)objParadaEvento.id_estatus == ParadaEvento.EstatusParadaEvento.Terminado)
                                        {
                                            //Validamos que la fecha de fin del evento sea menor a la fecha de salida de la parada
                                            resultado =  Fecha.ValidaFechaEnRango(DateTime.MinValue, objParadaEvento.fin_evento, fecha_salida, "La fecha fin del evento "+ objParadaEvento.secuencia_evento_parada.ToString("#") +" '{1}' debe ser menor a la la fecha de salida de la parada '{2}'");
                                        }
                                        else
                                        {
                                            //Actualizamos estatus del Evento
                                            resultado = objParadaEvento.ActualizaEstatus(ParadaEvento.EstatusParadaEvento.Terminado, id_usuario);

                                        }
                                    }
                                }
                            }
                            else
                            //Salimos del ciclo
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("No existen eventos asignados a la parada [" + this._descripcion + "].");
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza Secuencia Paradas
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia que se utliliza como referencia para actualizar las secuencias  posteriores a la misma</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaSecuenciaParadas(int id_servicio, decimal secuencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Realizando la consulta
            using (DataTable mit = CargaParadasParaActualizarSecuencias(id_servicio, secuencia))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Recorremos cada una de las paradas
                    foreach (DataRow r in mit.Rows)
                    {
                        //Validamos deshabilitación
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Parada
                            using (Parada objParada = new Parada(r.Field<int>("Id")))
                            {
                                if (objParada.habilitar)
                                {
                                    string descripcion = "";
                                    //Instanciamos  la Ubicación para obtener la descripción.
                                    using (Ubicacion objUbicacion = new Ubicacion(objParada.id_ubicacion))
                                    {
                                        //Obtenemos la descripcion de la ubicacion
                                        descripcion = objUbicacion.descripcion.ToUpper();
                                        //Instanciamos Ciudad
                                        using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                                        {
                                            //Validamos que exista la Ciudad
                                            if (objCiudad.habilitar)
                                            {
                                                //Obtenemos la descripcion de la ubicacion
                                                descripcion += " " + objCiudad.ToString();
                                            }
                                            else
                                            {
                                                resultado = new RetornoOperacion("No se puede recuperar la Ciudad");
                                                break;
                                            }
                                        }
                                        //Actualizando Parada
                                        resultado = objParada.EditaParada(objParada.secuencia_parada_servicio + 1, (TipoParada)objParada.Tipo, objParada.id_ubicacion,
                                                                 objUbicacion.geoubicacion, descripcion, objParada.cita_parada, id_usuario);
                                    }
                                }
                                else
                                {
                                    resultado = new RetornoOperacion("No se puede recuperar la Parada");
                                    break;
                                }
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
        /// Obtiene el total de las paradas registradas
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static int ObtieneTotalParadas(int id_servicio)
        {
            //Declaramos Resultados
            int Total = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 13, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    Total = (from DataRow r in ds.Tables[0].Rows
                             select Convert.ToInt32(r["Total"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return Total;
        }

        /// <summary>
        /// Obtiene el Id de la última parada
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static int ObtieneUltimaParada(int id_servicio)
        {
            //Declaramos Resultados
            int IdParada = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 16, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    IdParada = (from DataRow r in ds.Tables[0].Rows
                                select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }
            }

            //Obtenemos Resultado
            return IdParada;
        }
        /// <summary>
        /// Valida las cita de parada a ingresar de acuerdo a las paradas ya definidas.
        /// </summary>
        /// <param name="secuencia">Secuencia a ingresar</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="total_paradas">Total de paradas actuales</param>
        /// <param name="cita_parada">Cita de Parada a ingresar</param>
        /// <param name="tipo_parada">Tipo Parada</param>
        /// <returns></returns>
        private static RetornoOperacion validacionCitasParadasParaInserccion(decimal secuencia, int id_servicio, int total_paradas, DateTime cita_parada, TipoParada tipo_parada)
        {
            //Declaramos Resultados
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos variable para almacenar la Cita
            DateTime cita_posterior = DateTime.MinValue;
            DateTime cita_anterior = DateTime.MinValue;

            //Validamos que sea Parada Operativa
            if (TipoParada.Operativa == tipo_parada)
            {
                //Instanciamos Paradas
                using (Parada objParadaActual = new Parada(secuencia, id_servicio), objParadaAnterior = new Parada(secuencia - 1, id_servicio))
                {
                    //Asignamos Valor
                    cita_posterior = objParadaActual.cita_parada;
                    cita_anterior = objParadaAnterior.cita_parada;

                    //Validamos que la parada sea Fecha Minima (es decir parada Servicio)
                    if (Fecha.EsFechaMinima(cita_posterior))
                    {
                        using (Parada objParada = new Parada(BuscaParadaPosteriorOperativa(id_servicio, secuencia - 1)))
                        {
                            //Asignamos Valor
                            cita_posterior = objParada.cita_parada;
                        }
                    }
                    //Validamos que la parada sea Fecha Minima (es decir parada  de Servicio)
                    if (Fecha.EsFechaMinima(cita_anterior))
                    {
                        using (Parada objParada = new Parada(BuscaParadaAnteriorOperativa(id_servicio, secuencia - 1)))
                        {
                            //Asignamos Valor
                            cita_anterior = objParada.cita_parada;
                        }
                    }

                    //Si la Parada esta al Inicio
                    if (secuencia == 1)
                    {
                        //Validamos que la cita de la parada a Ingresar sea Menor a la cita de la parada posterior.
                        if (cita_parada < cita_posterior)
                        {
                            //Devolvemos Resultado como correcto
                            resultado = new RetornoOperacion(0);
                        }
                        else
                        {
                            //Mostramos Mensaje Error
                            resultado = new RetornoOperacion("La cita de la parada debe ser menor a " + cita_posterior.ToString("dd/MM/yyyy HH:mm"));
                        }
                    }
                    else
                    {
                        //Si la Parada esta al Final
                        if (secuencia > total_paradas)
                        {
                            //Validamos que la cita de la parada a Ingresar sea Mayor  a la cita de la parada anterior
                            if (cita_parada > cita_anterior)
                            {
                                //Devolvemos Resultado como correcto
                                resultado = new RetornoOperacion(0);
                            }
                            else
                            {
                                //Mostramos Mensaje Error
                                resultado = new RetornoOperacion("La cita de la parada debe ser mayor a " + cita_anterior.ToString("dd/MM/yyyy HH:mm"));
                            }
                        }
                        else
                        {
                            //Si la Parada esta en Medio
                            //Validamos que la cita de la parada a Ingresar sea Mayor  a la cita de la parada anterior y menor a la parada Posterior
                            if (cita_parada > cita_anterior && cita_parada < cita_posterior)
                            {
                                //Devolvemos Resultado como correcto
                                resultado = new RetornoOperacion(0);
                            }
                            else
                            {
                                //Mostramos Mensaje Error
                                resultado = new RetornoOperacion("La cita de la parada debe ser mayor a " + cita_anterior.ToString("dd/MM/yyyy HH:mm") + " y menor a " +
                                           cita_posterior.ToString("dd/MM/yyyy HH:mm"));
                            }
                        }
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Valida las cita de parada a editar de acuerdo a las paradas ya definidas.
        /// </summary>
        /// <param name="secuencia">Secuencia a ingresar</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="total_paradas">Total de paradas actuales</param>
        /// <param name="cita_parada">Cita de Parada a ingresar</param>
        /// <returns></returns>
        private static RetornoOperacion validacionCitasParadasParaEdicion(decimal secuencia, int id_servicio, int total_paradas, DateTime cita_parada)
        {
            //Declaramos Resultados
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Secuencia Actual
            using (Parada objParadaPosterior = new Parada(secuencia + 1, id_servicio), objParadaAnterior = new Parada(secuencia - 1, id_servicio))
            {
                //Si la Parada esta al Inicio
                if (secuencia == 1)
                {
                    //Validamos que la cita de la parada a Ingresar sea Menor a la cita de la parada posterior.
                    if (cita_parada < objParadaPosterior.cita_parada)
                    {
                        //Devolvemos Resultado como correcto
                        resultado = new RetornoOperacion(0);
                    }
                    else
                    {
                        //Mostramos Mensaje Error
                        resultado = new RetornoOperacion("La cita de parada debe ser menor a " + objParadaPosterior.cita_parada.ToString("dd/MM/yyyy HH:mm"));
                    }
                }
                else
                {
                    //Si la Parada esta al Final
                    if (secuencia == total_paradas)
                    {
                        //Validamos que la cita de la parada a Ingresar sea Mayor  a la cita de la parada anterior
                        if (cita_parada > objParadaAnterior.cita_parada)
                        {
                            //Devolvemos Resultado como correcto
                            resultado = new RetornoOperacion(0);
                        }
                        else
                        {
                            //Mostramos Mensaje Error
                            resultado = new RetornoOperacion("La cita de la parada debe ser mayor a " + objParadaAnterior.cita_parada.ToString("dd/MM/yyyy HH:mm"));
                        }
                    }
                    else
                    {
                        //Si la Parada esta en Medio
                        //Validamos que la cita de la parada a Ingresar sea Mayor  a la cita de la parada anterior y menor a la parada Posterior
                        if (cita_parada > objParadaAnterior.cita_parada && cita_parada < objParadaPosterior.cita_parada)
                        {
                            //Devolvemos Resultado como correcto
                            resultado = new RetornoOperacion(0);
                        }
                        else
                        {
                            //Mostramos Mensaje Error
                            resultado = new RetornoOperacion("La cita de la parada debe ser mayor a " + objParadaAnterior.cita_parada.ToString("dd/MM/yyyy HH:mm") + " y menor a " +
                                        objParadaPosterior.cita_parada.ToString("dd/MM/yyyy HH:mm"));
                        }
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Carga Paradas para visualización en Despacho
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaParadasParaVisualizacionDespacho(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 17, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };


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
        /// Carga las paradas para su deshabilitación
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaParadasParaDeshabilitacion(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 18, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };


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
        /// Deshabilitamos la parada Alternativa 
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParadaAlternativaEstancia(int id_usuario)
        {
            //Declarams Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Validamos que sea parada Alternativa
            if (this._secuencia_parada_servicio == 0)
            {
                resultado = editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, (EstatusParada)this._id_estatus_parada,
                                  this._id_ubicacion, this._geo_ubicacion, this._descripcion, this._cita_parada, this._fecha_llegada,
                               (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida,
                               (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, false);
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Deshabilitamos la parada  
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParada(int id_usuario)
        {
            //Editamos Parada
            return editaParada(this._id_servicio, this._secuencia_parada_servicio, (TipoParada)this._id_tipo_parada, (EstatusParada)this._id_estatus_parada,
                                 this._id_ubicacion, this._geo_ubicacion, this._descripcion, this._cita_parada, this._fecha_llegada,
                              (TipoActualizacionLlegada)this._id_tipo_actualizacion_llegada, this._id_razon_llegada_tarde, this._fecha_salida,
                              (TipoActualizacionSalida)this._id_tipo_actualizacion_salida, this._id_razon_salida_tarde, id_usuario, false);
        }

        /// <summary>
        /// Validamos que la parada se encuentre activa.
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static bool ValidaExistenciaParada(int id_parada)
        {
            //Declaramos objeto Resultado
            bool validaParada = false;

            //inicializamos el arreglo de parametros
            object[] param = { 19, id_parada, 0, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Validacion
                    validaParada = (from DataRow r in ds.Tables[0].Rows
                                    select Convert.ToBoolean(r["ValidaParada"])).FirstOrDefault();

                }
            }
            //Declaramos Objeto Resultado
            return validaParada;
        }

        /// <summary>
        /// Validación de Documento Recibidos ligado al segmento de la parada por insertar
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidacionDocumentosRecibidosInsertaParada()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion(0);

             //Buscamos la parada operativa anterior y la parada operativa posterior
            using (Parada objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(this._id_servicio, this._secuencia_parada_servicio)))
            {
                //Obtenemos la secuencia del segmento que contiene a las paradas anterior y posterior
                decimal SecuenciaSegmento = SegmentoCarga.BuscaSecuenciaSegmento(this._id_servicio, this._id_parada, objParadaPosteriorCarga.id_parada);

                // Instanciamos segmento a partir de la secuencia obtenida
                using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SecuenciaSegmento))
                {
                //En caso de ser segmento en medio
                    if (objParadaPosteriorCarga.id_parada != 0)
                    {
                        //Instanciamos Control Evidencia Segmento
                        using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmento, objSegmentoCarga.id_segmento_carga))
                        {
                            //Cargamos Recepción de Documentos
                            using (DataTable mitDocumentos = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(objSegmentoControlEvidencia.id_segmento_control_evidencia))
                            {
                                //Validamos Origen de Datos
                                if (Validacion.ValidaOrigenDatos(mitDocumentos))
                                {
                                    //Recorremos Documentos
                                    foreach (DataRow r in mitDocumentos.Rows)
                                    {
                                        //Cargamos Detalle de Paquete
                                        using (DataTable mit = PaqueteEnvioDocumento.ObtieneDetallesPaqueteDocumento(r.Field<int>("Id")))
                                        {
                                            //Validamos Origen de Datos
                                            if (Validacion.ValidaOrigenDatos(mit))
                                            {
                                                //Establecemos Mensaje Error
                                                res = new RetornoOperacion(-3,"Ya existen procesos ligados a la Recepción de Paquetes para el Segmento, ¿Desea continuar?",false);

                                                //Salimos del ciclo
                                                break;
                                            }
                                        }
                                    }
                                    //Validamos Resultado
                                    if (res.OperacionExitosa)
                                    {
                                        //Establecemos mensaje Error
                                        res = new RetornoOperacion(-3,"Ya existen procesos ligados a la Recepción de Documentos  a continuación se cancelarán. ¿Desea continuar?", false);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            //Devolvemos Valor Retorno
            return res;
        }

        /// <summary>
        /// Validación de Documentos Recibidos ligando el segmento por deshabilitar
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidacionDocumentosRecibidosDeshabilitaParada()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion(0);

            //Buscamos la parada anterior y la parada posterior
            using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(this._id_servicio, this._secuencia_parada_servicio)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(this._id_servicio, this._secuencia_parada_servicio)))
            {
                //Instanciamos el Segmento ligado el Id de parada actual y parada posterior
                using (SegmentoCarga objSegmentoCargaPo = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(this._id_servicio, this._id_parada, objParadaPosteriorCarga.id_parada)))
                {
                    //Instanciamos segmento a partir de la secuencia obtenida -1
                    using (SegmentoCarga objSegmentoCargaAnte = new SegmentoCarga(objSegmentoCargaPo.id_servicio, objSegmentoCargaPo.secuencia - 1))
                    {
                        //En caso de ser segmento en medio
                        if (objParadaAnteriorCarga.id_parada != 0 && objParadaPosteriorCarga.id_parada != 0)
                        {
                            //Instanciamos Control Evidencia Segmento Posterior
                            using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmento, objSegmentoCargaPo.id_segmento_carga))
                            {
                                //Cargamos Recepción de Documentos
                                using (DataTable mitDocumentos = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(objSegmentoControlEvidencia.id_segmento_control_evidencia))
                                {
                                    //Validamos Origen de Datos
                                    if (Validacion.ValidaOrigenDatos(mitDocumentos))
                                    {
                                        //Recorremos Documentos
                                        foreach (DataRow r in mitDocumentos.Rows)
                                        {
                                            //Cargamos Detalle de Paquete
                                            using (DataTable mit = PaqueteEnvioDocumento.ObtieneDetallesPaqueteDocumento(r.Field<int>("Id")))
                                            {
                                                //Validamos Origen de Datos
                                                if (Validacion.ValidaOrigenDatos(mit))
                                                {
                                                    //Establecemos Mensaje Error en caso de existir el proceso de Recepción de Paquetes
                                                    res = new RetornoOperacion(-3, "Ya existen procesos ligados a la Recepción de Paquetes para el segmento, ¿Desea continuar?", false);

                                                    //Salimos del ciclo
                                                    break;
                                                }
                                            }
                                        }
                                        //Validamos Resultado
                                        if (res.OperacionExitosa)
                                        {
                                            //Establecemos mensaje Error
                                            res = new RetornoOperacion(-3, "Ya existen procesos ligados a la Recepción de Documentos  a continuación se cancelarán. ¿Desea continuar?", false);
                                        }
                                    }

                                }
                            }
                            //Validamos Resultado
                            if(res.OperacionExitosa)
                            {
                                //Validación del Segmento Anterior

                                //Instanciamos Control Evidencia Segmento Anterior
                                using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmento,objSegmentoCargaAnte.id_segmento_carga))
                                {
                                    //Cargamos Recepción de Documentos
                                    using (DataTable mitDocumentos = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(objSegmentoControlEvidencia.id_segmento_control_evidencia))
                                    {
                                        //Validamos Origen de Datos
                                        if (Validacion.ValidaOrigenDatos(mitDocumentos))
                                        {
                                            //Recorremos Documentos
                                            foreach (DataRow r in mitDocumentos.Rows)
                                            {
                                                //Cargamos Detalle de Paquete
                                                using (DataTable mit = PaqueteEnvioDocumento.ObtieneDetallesPaqueteDocumento(r.Field<int>("Id")))
                                                {
                                                    //Validamos Origen de Datos
                                                    if (Validacion.ValidaOrigenDatos(mit))
                                                    {
                                                        //Establecemos Mensaje Error
                                                        res = new RetornoOperacion(-3, "Ya existen procesos ligados a la Recepción de Paquetes para el segmento, ¿Desea continuar?", false);

                                                        //Salimos del ciclo
                                                        break;
                                                    }
                                                }
                                            }
                                            //Validamos Resultado
                                            if (res.OperacionExitosa)
                                            {
                                                //Establecemos mensaje Error
                                                res = new RetornoOperacion(-3, "Ya existen procesos ligados a la Recepción de Documento  a continuación se cancelarán. ¿Desea continuar?", false);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        //En caso de ser parada al Final
                        else if (objParadaPosteriorCarga.id_parada == 0)
                        {
                            //Instanciamos el Segmento ligado el id de parada anterior y el Id de parada actual
                            using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(this._id_servicio, SegmentoCarga.BuscaSecuenciaSegmento(this._id_servicio, objParadaAnteriorCarga.id_parada, this._id_parada)))
                            {
                                   //Instanciamos Control Evidencia Segmento
                                    using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmento, objSegmentoCarga.id_segmento_carga))
                                    {
                                        //Cargamos Recepción de Documentos
                                        using (DataTable mitDocumentos = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(objSegmentoControlEvidencia.id_segmento_control_evidencia))
                                        {
                                            //Validamos Origen de Datos
                                            if (Validacion.ValidaOrigenDatos(mitDocumentos))
                                            {
                                                //Recorremos Documentos
                                                foreach (DataRow r in mitDocumentos.Rows)
                                                {
                                                    //Cargamos Detalle de Paquete
                                                    using (DataTable mit = PaqueteEnvioDocumento.ObtieneDetallesPaqueteDocumento(r.Field<int>("Id")))
                                                    {
                                                        //Validamos Origen de Datos
                                                        if (Validacion.ValidaOrigenDatos(mit))
                                                        {
                                                            //Establecemos Mensaje Error
                                                            res = new RetornoOperacion(-3, "Ya existen procesos ligados a la Recepción de Paquetes para el Segmento, ¿Desea continuar?", false);

                                                            //Salimos del ciclo
                                                            break;
                                                        }
                                                    }
                                                }
                                                //Validamos Resultado
                                                if (res.OperacionExitosa)
                                                {
                                                    //Establecemos mensaje Error
                                                    res = new RetornoOperacion(-3, "Ya existen procesos ligados a la Recepción de Documento  a continuación se cancelarán. ¿Desea continuar?", false);
                                                }
                                            }

                                        }
                                    }
                            }

                        }
                    }
                }
            }
            
            //Devolvemos Valor Retorno
            return res;
        }
        /// <summary>
        /// Deshabilitamos las Paradas ligando un Id Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaParadas(int id_servicio, int id_usuario)
        {
            //Declarams Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos paradas ligando un Id Servicio
            using (DataTable mitParadas = CargaParadasParaDeshabilitacion(id_servicio))
            {
                //Validamos que existan paradas
                if (Validacion.ValidaOrigenDatos(mitParadas))
                {
                    // Recorremos cada una de las paradas
                    foreach (DataRow r in mitParadas.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Parada
                            using (Parada objParada = new Parada(r.Field<int>("Id")))
                            {
                                //Deshabilitamos eventos
                                resultado = ParadaEvento.DeshabilitaParadaEventos(objParada.id_parada, id_usuario);

                                //Validamos Deshabilitación de Eventos
                                if (resultado.OperacionExitosa)
                                {
                                    //Deshabilitamos Parada
                                    resultado = objParada.DeshabilitaParada(id_usuario);
                                }
                            }
                        }
                        else
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene la fecha suegerencia  de la ultima parada de acuerdo a la fecha fin de los evetos
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DateTime ObtieneFechaSalidaUltimaParada(int id_servicio)
        {
            //Declaramos variable fecha de última parada
            DateTime fechaUltima = DateTime.MinValue;
            //Declaramos Resultados
            int IdParada = ObtieneUltimaParada(id_servicio);

            //inicializamos el arreglo de parametros
            object[] param = { 20, IdParada, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    fechaUltima = (from DataRow r in ds.Tables[0].Rows
                                select Convert.ToDateTime(r["FechaSugerenciaTerminoParada"])).FirstOrDefault();

                }
            }

            //Obtenemos Resultado
            return fechaUltima;
        }

        /// <summary>
        /// Carga Paradas Para la Publicación de Unidades
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaParadasParaPublicacionDeUnidad(int id_servicio)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 23, 0, id_servicio, 0, 0, 0, 0, SqlGeography.Null, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de retorno
                DataTable mit = null;

                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Método  encargado de Actualizar los atributos de la Parada
        /// </summary>
        /// <returns></returns>
        public bool ActualizaParada()
        {
            return this.cargaAtributosInstancia(this._id_parada);
        }
        /// <summary>
        /// Método encargado de Obtener el  Movimiento dada una Parada de Destino
        /// </summary>
        /// <param name="id_parada">Parada de Destino</param>
        /// <returns></returns>
        public static int ObtieneMovimientoParadaDestino(int id_parada)
        {
            //Declarando Objeto de Retorno
            int idMovimiento = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 24, id_parada, 0, 0, 0, 0, 0, SqlGeography.Null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Obteniendo Movimiento
                        idMovimiento = Convert.ToInt32(dr["IdMovimiento"]);
                }
            }

            //Devolviendo Resultado Obtenido
            return idMovimiento;
        }

        #endregion
    }
}

