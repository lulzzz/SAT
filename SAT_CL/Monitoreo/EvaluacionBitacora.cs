using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Monitoreo
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con la Evaluación de la Bitacora
    /// </summary>
    public class EvaluacionBitacora : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Expresa los Resultados de la Evaluación de la Bitacora
        /// </summary>
        public enum ResultadoBitacora
        {
            /// <summary>
            /// Expresa que el Resultado de la Bitacora es correcto.
            /// </summary>
            Ok = 1,
            /// <summary>
            /// Expresa que la Ubicación de la Unidad no Coincide con la del Sistema
            /// </summary>
            UbicacionNoCoincidente = 2,
            /// <summary>
            /// Expresa que la Unidad esta Detenida
            /// </summary>
            UnidadDetenida = 3,
            /// <summary>
            /// Expresa que la Unidad se esta Alejando del Destino
            /// </summary>
            UnidadAlejandose = 4,
            /// <summary>
            /// Expresa que la Unidad ha posicionado fuera de la Tolerancia
            /// </summary>
            UnidadTiempoExcedido = 5,
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "monitoreo.sp_evaluacion_bitacora_teb";

        private int _id_evaluacion_bitacora;
        /// <summary>
        /// Atributo que Almacena el Identificador Unico del Registro
        /// </summary>
        public int id_evaluacion_bitacora { get { return this._id_evaluacion_bitacora; } }
        private int _id_bitacora;
        /// <summary>
        /// Atributo que Almacena la Bitacora
        /// </summary>
        public int id_bitacora { get { return this._id_bitacora; } }
        private DateTime _fecha_bitacora;
        /// <summary>
        /// Atributo que Almacena la Fecha de la Bitacora
        /// </summary>
        public DateTime fecha_bitacora { get { return this._fecha_bitacora; } }
        private byte _id_resultado_bitacora;
        /// <summary>
        /// Atributo que Almacena el Resultado de la Bitacora
        /// </summary>
        public byte id_resultado_bitacora { get { return this._id_resultado_bitacora; } }
        /// <summary>
        /// Atributo que Almacena el Resultado de la Bitacora (Enumeración)
        /// </summary>
        public ResultadoBitacora resultado_bitacora { get { return (ResultadoBitacora)this._id_resultado_bitacora; } }
        private int _tiempo_excedido;
        /// <summary>
        /// Atributo que Almacena el Tiempo Excedido de Posicionamiento GPS
        /// </summary>
        public int tiempo_excedido { get { return this._tiempo_excedido; } }
        private int _distancia;
        /// <summary>
        /// Atributo que Almacena la Distancia Restante entre la Unidad y su Destino
        /// </summary>
        public int distancia { get { return this._distancia; } }
        private decimal _tiempo;
        /// <summary>
        /// Atributo que Almacena el Tiempo Restante entre la Unidad y su Destino
        /// </summary>
        public decimal tiempo { get { return this._tiempo; } }
        private DateTime _hora_llegada;
        /// <summary>
        /// Atributo que Almacena la Hora de Llegada entre la Unidad y su Destino
        /// </summary>
        public DateTime hora_llegada { get { return this._hora_llegada; } }
        private DateTime _cita;
        /// <summary>
        /// Atributo que Almacena la Cita de la Unidad y su Destino
        /// </summary>
        public DateTime cita { get { return this._cita; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que Almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public EvaluacionBitacora()
        {
            //Asignando Valores
            this._id_evaluacion_bitacora = 0;
            this._id_bitacora = 0;
            this._fecha_bitacora = DateTime.MinValue;
            this._id_resultado_bitacora = 0;
            this._tiempo_excedido = 0;
            this._distancia = 0;
            this._tiempo = 0.00M;
            this._hora_llegada = DateTime.MinValue;
            this._cita = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_evaluacion_bitacora">Evaluación de la Bitacora</param>
        public EvaluacionBitacora(int id_evaluacion_bitacora)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_evaluacion_bitacora);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~EvaluacionBitacora()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_evaluacion_bitacora">Evaluación de la Bitacora</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_evaluacion_bitacora)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Creando Arreglo de Parametros
            object[] param = { 3, id_evaluacion_bitacora, 0, null, 0, 0, 0, 0, null, null, 0, false, "", "" };

            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo el Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_evaluacion_bitacora = id_evaluacion_bitacora;
                        this._id_bitacora = Convert.ToInt32(dr["IdBitacora"]);
                        DateTime.TryParse(dr["FechaBitacora"].ToString(), out this._fecha_bitacora);
                        this._id_resultado_bitacora = Convert.ToByte(dr["IdResultadoBitacora"]);
                        this._tiempo_excedido = Convert.ToInt32(dr["TiempoExcedido"]);
                        this._distancia = Convert.ToInt32(dr["Distancia"]);
                        this._tiempo = Convert.ToDecimal(dr["Tiempo"]);
                        DateTime.TryParse(dr["HoraLlegada"].ToString(), out this._hora_llegada);
                        DateTime.TryParse(dr["Cita"].ToString(), out this._cita);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Valor Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_bitacora">Bitacora de Monitoreo</param>
        /// <param name="fecha_bitacora">Fecha de la Evaluación de la Bitacora</param>
        /// <param name="resultado_bitacora">Resultado de la Bitacora (Enumeración)</param>
        /// <param name="tiempo_excedido">Tiempo Excedido de Posicionamiento GPS</param>
        /// <param name="distancia">Distancia Restante de la Unidad con su Destino</param>
        /// <param name="tiempo">Tiempo Restante entre la Unidad y su Destino</param>
        /// <param name="hora_llegada">Hora de Llegada entre la Unidad y su Destino</param>
        /// <param name="cita">Cita del Destino de la Unidad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_bitacora, DateTime fecha_bitacora, ResultadoBitacora resultado_bitacora, int tiempo_excedido, 
                                                      int distancia, decimal tiempo, DateTime hora_llegada, DateTime cita, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Creando Arreglo de Parametros
            object[] param = { 2, this._id_evaluacion_bitacora, id_bitacora, fecha_bitacora, (byte)resultado_bitacora, tiempo_excedido, 
                               distancia, tiempo, Fecha.ConvierteDateTimeObjeto(hora_llegada), Fecha.ConvierteDateTimeObjeto(cita), 
                               id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Evaluación de las Bitacora
        /// </summary>
        /// <param name="id_bitacora">Bitacora de Monitoreo</param>
        /// <param name="fecha_bitacora">Fecha de la Evaluación de la Bitacora</param>
        /// <param name="resultado_bitacora">Resultado de la Bitacora (Enumeración)</param>
        /// <param name="tiempo_excedido">Tiempo Excedido de Posicionamiento GPS</param>
        /// <param name="distancia">Distancia Restante de la Unidad con su Destino</param>
        /// <param name="tiempo">Tiempo Restante entre la Unidad y su Destino</param>
        /// <param name="hora_llegada">Hora de Llegada Estimada</param>
        /// <param name="cita">Cita del Destino de la Unidad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaEvaluacionBitacora(int id_bitacora, DateTime fecha_bitacora, ResultadoBitacora resultado_bitacora, int tiempo_excedido,
                                                                 int distancia, decimal tiempo, DateTime hora_llegada, DateTime cita, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Creando Arreglo de Parametros
            object[] param = { 1, 0, id_bitacora, fecha_bitacora, (byte)resultado_bitacora, tiempo_excedido, distancia, 
                               tiempo, Fecha.ConvierteDateTimeObjeto(hora_llegada), Fecha.ConvierteDateTimeObjeto(cita), 
                               id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Evaluación de la Bitacora
        /// </summary>
        /// <param name="id_bitacora">Bitacora de Monitoreo</param>
        /// <param name="fecha_bitacora">Fecha de la Evaluación de la Bitacora</param>
        /// <param name="resultado_bitacora">Resultado de la Bitacora (Enumeración)</param>
        /// <param name="distancia">Distancia Restante de la Unidad con su Destino</param>
        /// <param name="tiempo">Tiempo Restante entre la Unidad y su Destino</param>
        /// <param name="cita">Cita del Destino de la Unidad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaEvaluacionBitacora(int id_bitacora, DateTime fecha_bitacora, ResultadoBitacora resultado_bitacora, int tiempo_excedido,
                                                        int distancia, decimal tiempo, DateTime hora_llegada, DateTime cita, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_bitacora, fecha_bitacora, resultado_bitacora, tiempo_excedido, distancia, 
                                             tiempo, hora_llegada, cita, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar la Evaluación de la Bitacora
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaEvaluacionBitacora(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_bitacora, this._fecha_bitacora, (ResultadoBitacora)this._id_resultado_bitacora, this._tiempo_excedido,
                                             this._distancia, this._tiempo, this._hora_llegada, this._cita, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaEvaluacionBitacora()
        {
            //Devolviendo Resultado Obtenido
            return this.cargaAtributosInstancia(this._id_evaluacion_bitacora);
        }
        /// <summary>
        /// Método encargado de Obtener el Resultado de la Evaluación
        /// </summary>
        /// <param name="id_eta">Evaluación</param>
        /// <returns></returns>
        public static DataTable ObtieneResultadoEvaluacion(int id_eta)
        {
            //Declarando Objeto de Retorno
            DataTable dtETA = null;

            //Creando Arreglo de Parametros
            object[] param = { 4, id_eta, 0, null, 0, 0, 0, 0, null, null, 0, false, "", "" };

            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores
                    dtETA = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtETA;
        }
        /// <summary>
        /// Método encargado de Obtener el Resultado de la Cercania
        /// </summary>
        /// <param name="id_bitacora">Bitacora</param>
        /// <param name="distancia">Distancia</param>
        /// <returns></returns>
        public static ResultadoBitacora ObtieneCercaniaEvaluacion(int id_bitacora, int distancia, DateTime fecha_peticion)
        {
            //Declarando Objeto de Retorno
            ResultadoBitacora result = ResultadoBitacora.Ok;

            //Creando Arreglo de Parametros
            object[] param = { 5, 0, id_bitacora, null, 0, 0, distancia, 0, null, fecha_peticion, 0, false, "", "" };

            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Resultado
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Instanciando Evaluación Anterior
                        using (SAT_CL.Monitoreo.EvaluacionBitacora eta_ant = new SAT_CL.Monitoreo.EvaluacionBitacora(Convert.ToInt32(dr["Id"])))
                        {
                            //Validando que exista
                            if (eta_ant.habilitar)
                            {
                                //Validando si la Unidad se esta Acercando
                                if (eta_ant.distancia >= distancia)

                                    //Intanciando Resultado Positivo
                                    result = ResultadoBitacora.Ok;

                                //Validando si la Unidad se esta Alejando
                                else if (eta_ant.distancia < distancia)

                                    //Intanciando Resultado Positivo
                                    result = ResultadoBitacora.UnidadAlejandose;
                            }
                        }

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
