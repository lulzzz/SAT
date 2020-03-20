using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;

namespace SAT_CL.Monitoreo
{
    /// <summary>
    /// Implementa los método para la administración de Bitácora de Monitoreo
    /// </summary>
    public class BitacoraMonitoreo : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Tipo Ubicación
        /// </summary>
        public enum OrigenBitacoraMonitoreo
        {
            /// Dispositivo no reconocido
            /// </summary>
            Desconocido = 0,
            /// <summary>
            /// Equipo de Escritorio
            /// </summary>
            Escritorio = 1,
            /// <summary>
            /// Equipo Portátil
            /// </summary>
            AntenaGPS = 2,
            /// <summary>
            /// Aplicación Movil
            /// </summary>
            APP = 3,
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "monitoreo.sp_bitacora_monitoreo_tbm";

        private int _id_bitacora_monitoreo;
        /// <summary>
        /// Describe la Bitácora de Monitoreo
        /// </summary>
        public int id_bitacora_monitoreo { get { return _id_bitacora_monitoreo; } }
        private byte _id_origen_bitacora_monitoreo;
        /// <summary>
        /// Describe el Origen de la Bitácora
        /// </summary>
        public byte id_origen_bitacora_monitoreo { get { return _id_origen_bitacora_monitoreo; } }
        private byte _id_tipo_bitacora_monitoreo;
        /// <summary>
        /// Describe el Tipo de Bitácora
        /// </summary>
        public byte id_tipo_bitacora_monitoreo { get { return _id_tipo_bitacora_monitoreo; } }
        private int _id_servicio;
        /// <summary>
        /// Describe el Servicio
        /// </summary>
        public int id_servicio { get { return _id_servicio; } }
        private int _id_parada;
        /// <summary>
        /// Describe la Parada
        /// </summary>
        public int id_parada { get { return _id_parada; } }
        private int _id_evento;
        /// <summary>
        /// Describe el Evento
        /// </summary>
        public int id_evento { get { return _id_evento; } }
        private int _id_movimiento;
        /// <summary>
        /// Describe el Movimiento
        /// </summary>
        public int id_movimiento { get { return _id_movimiento; } }
        private int _id_tabla;
        /// <summary>
        /// Describe la tabla
        /// </summary>
        public int id_tabla { get { return _id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Describe el Registró
        /// </summary>
        public int id_registro { get { return _id_registro; } }
        private SqlGeography _geoubicacion;
        /// <summary>
        /// Describe la geoubicación
        /// </summary>
        public SqlGeography geoubicacion { get { return _geoubicacion; } }
        /// <summary>
        /// Obtiene el valor de la longitud de esta ubicación
        /// </summary>
        public double latitud { get { return !this._geoubicacion.IsNull ? this._geoubicacion.EnvelopeCenter().Lat.Value : 0; } }
        /// <summary>
        /// Obtiene el valor de la longitud de esta ubicación
        /// </summary>
        public double longitud { get { return !this._geoubicacion.IsNull ? this._geoubicacion.EnvelopeCenter().Long.Value : 0; } }
        private string _nombre_ubicacion;
        /// <summary>
        /// Describe el nombre de la Ubicación
        /// </summary>
        public string nombre_ubicacion { get { return _nombre_ubicacion; } }
        private string _comentario;
        /// <summary>
        /// Describe el comentario
        /// </summary>
        public string comentario { get { return _comentario; } }
        private DateTime _fecha_bitacora;
        /// <summary>
        /// Describe la Fecha de Bitácora
        /// </summary>
        public DateTime fecha_bitacora { get { return _fecha_bitacora; } }
        private decimal _velocidad;
        /// <summary>
        /// Describe la Velocidad de la Unidad
        /// </summary>
        public decimal velocidad { get { return _velocidad; } }
        private bool _bit_encendido;
        /// <summary>
        /// Describe el Indicador de Encendido
        /// </summary>
        public bool bit_encendido { get { return _bit_encendido; } }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar { get { return _habilitar; } }
        /// <summary>
        /// Enumera el Origen
        /// </summary>
        public OrigenBitacoraMonitoreo Origen { get { return (OrigenBitacoraMonitoreo)_id_origen_bitacora_monitoreo; } }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~BitacoraMonitoreo()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public BitacoraMonitoreo()
        {

        }
        /// <summary>
        /// Genera una Instancia Bitácora Monitoreo
        /// </summary>
        /// <param name="id_bitacora_monitoreo"></param>
        public BitacoraMonitoreo(int id_bitacora_monitoreo)
        {
            cargaAtributosInstancia(id_bitacora_monitoreo);
        }
        /// <summary>
        /// Genera una Instancia Bitácora Monitoreo
        /// </summary>
        /// <param name="id_bitacora_monitoreo">Establece Id de Bitácora de Monitoreo</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_bitacora_monitoreo)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_bitacora_monitoreo, 0, 0, 0, 0, 0, 0, 0, 0, SqlGeography.Null, "", "", null, 0.00M, false, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_bitacora_monitoreo = Convert.ToInt32(r["Id"]);
                        _id_origen_bitacora_monitoreo = Convert.ToByte(r["OrigenBitacora"]);
                        _id_tipo_bitacora_monitoreo = Convert.ToByte(r["TipoBitacora"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _id_parada = Convert.ToInt32(r["IdParada"]);
                        _id_evento = Convert.ToInt32(r["IdEvento"]);
                        _id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        _id_tabla = Convert.ToInt32(r["IdTabla"]);
                        _id_registro = Convert.ToInt32(r["IdRegistro"]);
                        _geoubicacion = (SqlGeography)r["Ubicacion"];
                        _nombre_ubicacion = r["NombreUbicacion"].ToString();
                        _comentario = r["Comentario"].ToString();
                        _fecha_bitacora = Convert.ToDateTime(r["FechaBitacora"]);
                        _velocidad = Convert.ToDecimal(r["Velocidad"]);
                        _bit_encendido = Convert.ToBoolean(r["BitEncendido"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
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
        /// Método encargado de Editar la Bitácora Monitoreo
        /// </summary>
        /// <param name="id_origen_bitacora_monitoreo">Origen de la Bitácora (Escritorio, Portatil)</param>
        /// <param name="id_tipo_bitacora_monitoreo">Tipo de Bitácora</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_evento">Id Evento</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="ubicacion">Geo Ubicación</param>
        /// <param name="nombre_ubicacion">Nombre de la Ubicación</param>
        /// <param name="comentario">Comentario</param>
        /// <param name="fecha_bitacora">Fecha de la Bitácora</param>
        /// <param name="velocidad">Velocidad</param>
        /// <param name="bit_encendido">Indicador de Encendido</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaBitacoraMonitoreo(OrigenBitacoraMonitoreo id_origen_bitacora_monitoreo, byte id_tipo_bitacora_monitoreo, int id_servicio,
                                                        int id_parada, int id_evento, int id_movimiento, int id_tabla, int id_registro, SqlGeography ubicacion,
                                                        string nombre_ubicacion, string comentario, DateTime fecha_bitacora, decimal velocidad, bool bit_encendido,
                                                        int id_usuario, bool habilitar)
        {

            //Inicializando arreglo de parámetros
            object[] param = {2, this._id_bitacora_monitoreo, id_origen_bitacora_monitoreo, id_tipo_bitacora_monitoreo, id_servicio, 
                                 id_parada, id_evento, id_movimiento, id_tabla, id_registro, ubicacion, nombre_ubicacion, comentario, 
                                 fecha_bitacora, velocidad, bit_encendido, 
                                 id_usuario, habilitar, "", ""};
            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        #endregion

        #region Metodos publicos

        /// <summary>
        /// Método encargado de Insertar la Bitácora Monitoreo
        /// </summary>
        /// <param name="id_origen_bitacora_monitoreo">Origen de la Bitácora (Escritorio, Portatil)</param>
        /// <param name="id_tipo_bitacora_monitoreo">Tipo de Bitácora</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_evento">Id Evento</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="ubicacion">Geo Ubicación</param>
        /// <param name="nombre_ubicacion">Nombre de la Ubicación</param>
        /// <param name="comentario">Comentario</param>
        /// <param name="fecha_bitacora">Fecha de la Bitácora</param>
        /// <param name="velocidad">Velocidad</param>
        /// <param name="bit_encendido">Indicador de Encendido</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaBitacoraMonitoreo(OrigenBitacoraMonitoreo id_origen_bitacora_monitoreo, byte id_tipo_bitacora_monitoreo, int id_servicio,
                                                     int id_parada, int id_evento, int id_movimiento, int id_tabla, int id_registro, SqlGeography ubicacion, 
                                                     string nombre_ubicacion, string comentario, DateTime fecha_bitacora, decimal velocidad, bool bit_encendido, 
                                                     int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_origen_bitacora_monitoreo, id_tipo_bitacora_monitoreo, id_servicio, id_parada, id_evento, 
                                 id_movimiento, id_tabla, id_registro, ubicacion, nombre_ubicacion, comentario, Fecha.ConvierteDateTimeObjeto(fecha_bitacora), 
                                 velocidad, bit_encendido, id_usuario, true, "", ""};
            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }
        /// <summary>
        /// Método encargado de Editar la Bitácora Monitoreo
        /// </summary>
        /// <param name="id_tipo_bitacora_monitoreo">Tipo de Bitácora</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="ubicacion">Geo Ubicación</param>
        /// <param name="nombre_ubicacion">Nombre de la Ubicación</param>
        /// <param name="comentario">Comentario</param>
        /// <param name="fecha_bitacora">Fecha de la Bitácora</param>
        /// <param name="velocidad">Velocidad</param>
        /// <param name="bit_encendido">Indicador de Encendido</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaBitacoraMonitoreo(byte id_tipo_bitacora_monitoreo, int id_servicio, int id_parada, int id_evento,
                                        int id_movimiento, int id_tabla, int id_registro, SqlGeography ubicacion, string nombre_ubicacion,
                                        string comentario, DateTime fecha_bitacora, decimal velocidad, bool bit_encendido, int id_usuario)
        {
            //Establecemos Resultado
            return this.editaBitacoraMonitoreo((OrigenBitacoraMonitoreo)this._id_origen_bitacora_monitoreo, id_tipo_bitacora_monitoreo, id_servicio, id_parada,
                                            id_evento, id_movimiento, id_tabla, id_registro, ubicacion, nombre_ubicacion, comentario,
                                            fecha_bitacora, velocidad, bit_encendido, id_usuario, habilitar);
        }
        /// <summary>
        /// Deshabilita una Bitacora Monitoreo
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaBitacoraMonitoreo(int id_usuario)
        {
            //Declaramos Objeto Resultado
            return this.editaBitacoraMonitoreo((OrigenBitacoraMonitoreo)this._id_origen_bitacora_monitoreo, this._id_tipo_bitacora_monitoreo, this._id_servicio,
                                              this._id_parada, this._id_evento, this._id_movimiento, this._id_tabla, this._id_registro, this._geoubicacion, this._nombre_ubicacion, this._comentario, this._fecha_bitacora,
                                              this._velocidad, this._bit_encendido, id_usuario, false);
        }
        /// <summary>
        /// Método  encargado de Actualizar  la Ubicación
        /// </summary>
        /// <returns></returns>
        public bool ActualizaBitacoraMonitoreo()
        {
            //Invocando Método de Retorno
            return this.cargaAtributosInstancia(this._id_bitacora_monitoreo);
        }
        /// <summary>
        /// Construye una lista con los puntos que contiene su dato geográfico
        /// </summary>
        /// <returns></returns>
        public List<PointF> RecuperaPuntosUbicacion()
        {
            //Definiendo objeto principal para listar los puntos (lat, long) que conforman el poligono
            List<PointF> puntos = new List<PointF>();
            //Si la ubicación geográfica está definida
            if (!this._geoubicacion.IsNull)
            {
                //Determinando el tipo de geometría que representa el valor geográfico
                switch (this._geoubicacion.STGeometryType().Value)
                {
                    case "Point":
                        puntos.Add(new PointF((float)this._geoubicacion.Lat.Value, (float)this._geoubicacion.Long.Value));
                        break;
                    case "LineString":
                    case "CompoundCurve":
                    case "Polygon":
                    case "CurvePolygon":
                        //Determinando cuantos puntos posee el valor geográfico
                        int total_puntos = this._geoubicacion.STNumPoints().Value;
                        //Para cada uno de los puntos existentes
                        for (int x = 1; x < total_puntos; x++)
                        {
                            //Añadiendo el punto a la lista
                            puntos.Add(new PointF((float)this._geoubicacion.STPointN(x).Lat.Value, (float)this._geoubicacion.STPointN(x).Long.Value));
                        }
                        break;
                    // TODO: Definir si la tabla de ubicaciones puede tener más de un polígono definido para el mismo lugar (areas separadas por avenidas o lotes no consecutivos).
                    /*
                case "GeometryCollection":
                case "MultiPoint":
                case "MultiLineString":
                case "MultiPolygon":
                    break;
                    */
                    default:
                        break;
                }
            }

            //Devolviendo listado de puntos
            return puntos;
        }

        /// <summary>
        /// Carga Historial de Bitácora de acuerdo a una Entidad
        /// </summary>
        /// <param name="id_tabla">Id Tabla entidad</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="fecha_inicio">Permite definir un parametro de busqueda entre una fecha inicial</param>
        /// <param name="fecha_fin">Perite definir un limite de busqueda por fecha</param>
        /// <param name="id_tipo_bitacora_monitoreo">Permite definir un tipo de busqueda(Posicionamiento, robo, Accidente,etc)</param>
        /// <param name="no_servicio">No. de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaHistorialBitacoraMonitoreo(int id_tabla, int id_registro, DateTime fecha_inicio, DateTime fecha_fin, byte id_tipo_bitacora_monitoreo, string no_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 4, 0, 0, id_tipo_bitacora_monitoreo, 0, 0, 0, 0, id_tabla, id_registro, SqlGeography.Null, no_servicio, "", null, 0.00M, false, 0, false, fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) };

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
        /// Método encargado de Carga el Reporte de Bitacora por Entidad
        /// </summary>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro</param>
        /// <returns></returns>
        public static DataTable CargaReporteBitacoraMonitoreo(int id_tabla, int id_registro)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, 0, 0, 0, 0, 0, 0, id_tabla, id_registro, SqlGeography.Null, "", "", null, 0.00M, false, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }
        /// <summary>
        /// Método encargado de Cargar las Imagenes de Incidencia del Monitoreo
        /// </summary>
        /// <param name="id_bitacora_monitoreo">Bitacora de Monitoreo</param>
        /// <returns></returns>
        public static DataTable CargaImagenesBitacoraMonitoreo(int id_bitacora_monitoreo)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 6, id_bitacora_monitoreo, 0, 0, 0, 0, 0, 0, 0, 0, SqlGeography.Null, "", "", null, 0.00M, false, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }

        #endregion
    }
}


