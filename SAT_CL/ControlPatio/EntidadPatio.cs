using Microsoft.SqlServer.Types;
using System;
using System.Data;
using TSDK.Base;
using System.Configuration;
namespace SAT_CL.ControlPatio
{
    /// <summary>
    /// Clase encargada de las Operaciones correspondientes a las Entidades del Patio
    /// </summary>
    public class EntidadPatio : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que Expresa el Tipo de Entidad de Patio
        /// </summary>
        public enum TipoEntidad
        {   /// <summary>
            /// Expresa que la Entidad es un Anden
            /// </summary>
            Anden = 1,
            /// <summary>
            /// Expresa que la Entidad es un Cajon
            /// </summary>
            Cajon,
            /// <summary>
            /// Expresa que la Entidad es un Acceso
            /// </summary>
            Acceso
        }
        /// <summary>
        /// Enumeración que Expresa el Estatus de la Entidad
        /// </summary>
        public enum Estatus
        {   /// <summary>
            /// Expresa que la Entidad se encuentra Ocupada
            /// </summary>
            Ocupado = 1,
            /// <summary>
            /// Expresa que la Entidad se encuentra Vacia
            /// </summary>
            Vacio
        }
        /// <summary>
        /// Enumeración que Expresa el Estatus de Carga de la Entidad
        /// </summary>
        public enum EstatusCarga
        {   
            /// <summary>
            /// Expresa que la Entidad no hay Estatus de Carga
            /// </summary>
            Ninguno = 0,
            /// <summary>
            /// Expresa que la Entidad esta en el proceso de Carga
            /// </summary>
            Cargando,
            /// <summary>
            /// Expresa que la Entidad esta en el proceso de Descarga
            /// </summary>
            Descargando,
            /// <summary>
            /// Expresa que la Entidad esta en el proceso de Estacionado
            /// </summary>
            Estacionando
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_entidad_patio_tep";

        private int _id_entidad_patio;
        /// <summary>
        /// Atributo encargado de Almacenar la Entidad de Patio
        /// </summary>
        public int id_entidad_patio { get { return this._id_entidad_patio; } }
        private int _id_ubicacion_patio;
        /// <summary>
        /// Atributo encargado de Almacenar la Ubicación del Patio
        /// </summary>
        public int id_ubicacion_patio { get { return this._id_ubicacion_patio; } }
        private byte _id_tipo_entidad;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Entidad
        /// </summary>
        public byte id_tipo_entidad { get { return this._id_tipo_entidad; } }
        /// <summary>
        /// Atributo encargado de Almacenar la Enumeración del Tipo de Entidad
        /// </summary>
        public TipoEntidad tipo_entidad { get { return (TipoEntidad)this._id_tipo_entidad; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de Almacenar la Enumeración del Estatus de la Entidad
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private byte _id_estatus_carga;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus de Carga
        /// </summary>
        public byte id_estatus_carga { get { return this._id_estatus_carga; } }
        /// <summary>
        /// Atributo encargado de Almacenar la Enumeración del Estatus de Carga de la Entidad
        /// </summary>
        public EstatusCarga estatus_carga { get { return (EstatusCarga)this._id_estatus_carga; } }
        private SqlGeography _geoubicacion;
        /// <summary>
        /// Atributo encargado de Almacenar la Ubicación Geografica
        /// </summary>
        public SqlGeography geoubicacion { get { return this._geoubicacion; } }
        private string _color_hxd;
        /// <summary>
        /// Atributo encargado de Almacenar el Color HXD
        /// </summary>
        public string color_hxd { get { return this._color_hxd; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private int _id_zona_patio;
        /// <summary>
        /// Atributo encargado de Almacenar la Zona del Patio
        /// </summary>
        public int id_zona_patio { get { return this._id_zona_patio; } }
        private DateTime _fecha_estatus;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Actualización de Estatus
        /// </summary>
        public DateTime fecha_estatus { get { return this._fecha_estatus; } }
        private decimal _coordenada_x;
        /// <summary>
        /// Atributo encargado de Almacenar la Coordenada X
        /// </summary>
        public decimal coordenada_x { get { return this._coordenada_x; } }
        private decimal _coordenada_y;
        /// <summary>
        /// Atributo encargado de Almacenar la Coordenada Y
        /// </summary>
        public decimal coordenada_y { get { return this._coordenada_y; } }
        private int _tiempo_carga;
        /// <summary>
        /// Atributo encargado de Almacenar el Tiempo de Carga
        /// </summary>
        public int tiempo_carga { get { return this._tiempo_carga; } }
        private int _tiempo_descarga;
        /// <summary>
        /// Atributo encargado de Almacenar el Tiempo de Descarga
        /// </summary>
        public int tiempo_descarga { get { return this._tiempo_descarga; } }
        private int _tiempo_libre;
        /// <summary>
        /// Atributo encargado de Almacenar el Tiempo Libre
        /// </summary>
        public int tiempo_libre { get { return this._tiempo_libre; } }
        private int _id_evento;
        /// <summary>
        /// Atributo encargado de Almacenar el Evento Actual
        /// </summary>
        public int id_evento { get { return this._id_evento; } }
        private int _tamano_icono;
        /// <summary>
        /// Atributo encargado de Almacenar el Tamaño del Icono
        /// </summary>
        public int tamano_icono { get { return this._tamano_icono; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Construccion

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public EntidadPatio()
        {   //Asignando Valores
            this._id_entidad_patio = 0;
            this._id_ubicacion_patio = 0;
            this._id_tipo_entidad = 0;
            this._id_estatus = 0;
            this._id_estatus_carga = 0;
            this._geoubicacion = null;
            this._color_hxd = "";
            this._descripcion = "";
            this._id_zona_patio = 0;
            this._fecha_estatus = DateTime.MinValue;
            this._coordenada_x = 0;
            this._coordenada_y = 0;
            this._tiempo_carga = 0;
            this._tiempo_descarga = 0;
            this._tiempo_libre = 0;
            this._id_evento = 0;
            this._tamano_icono = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        /// <param name="id_entidad_patio">Entidad de Patio</param>
        public EntidadPatio(int id_entidad_patio)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_entidad_patio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~EntidadPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos de la Instancia dado un Registro
        /// </summary>
        /// <param name="id_entidad_patio">Entidad de Patio</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_entidad_patio)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_entidad_patio, 0, 0, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_entidad_patio = id_entidad_patio;
                        this._id_ubicacion_patio = Convert.ToInt32(dr["IdUbicacionPatio"]);
                        this._id_tipo_entidad = Convert.ToByte(dr["IdTipoEntidad"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_estatus_carga = Convert.ToByte(dr["IdEstatusCarga"]);
                        this._geoubicacion = (SqlGeography)dr["Geoubicacion"];
                        this._color_hxd = dr["ColorHXD"].ToString();
                        this._descripcion = dr["Descripcion"].ToString();
                        this._id_zona_patio = Convert.ToInt32(dr["IdZonaPatio"]);
                        DateTime.TryParse(dr["FechaEstatus"].ToString(), out this._fecha_estatus);
                        this._coordenada_x = Convert.ToDecimal(dr["CoordenadaX"]);
                        this._coordenada_y = Convert.ToDecimal(dr["CoordenadaY"]);
                        this._tiempo_carga = Convert.ToInt32(dr["TiempoCarga"]);
                        this._tiempo_descarga = Convert.ToInt32(dr["TiempoDescarga"]);
                        this._tiempo_libre = Convert.ToInt32(dr["TiempoLibre"]);
                        this._id_evento = Convert.ToInt32(dr["IdEvento"]);
                        this._tamano_icono = Convert.ToInt32(dr["TamanoIcono"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Retorno a Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="estatus_carga">Estatus de Carga</param>
        /// <param name="geoubicacion">Geoubicación de la Entidad</param>
        /// <param name="color_hxd">Color en Formato Hexadecimal</param>
        /// <param name="descripcion">Descripción de la Entidad</param>
        /// <param name="id_zona_patio">Zona del Patio a la que pretenece la Entidad</param>
        /// <param name="fecha_estatus">Fecha de Actualización del Estatus</param>
        /// <param name="coordenada_x">Coordenada X</param>
        /// <param name="coordenada_y">Coordenada Y</param>
        /// <param name="tiempo_carga">Tiempo de Carga</param>
        /// <param name="tiempo_descarga">Tiempo de Descarga</param>
        /// <param name="tiempo_libre">Tiempo Libre</param>
        /// <param name="id_evento">Evento Actual de la Entidad</param>
        /// <param name="tamano_icono">Tamaño del Icono</param>
        /// <param name="id_usuario">usuario que actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_ubicacion_patio, TipoEntidad tipo_entidad, Estatus estatus, EstatusCarga estatus_carga, SqlGeography geoubicacion, 
                                        string color_hxd, string descripcion, int id_zona_patio, DateTime fecha_estatus, decimal coordenada_x, decimal coordenada_y, int tiempo_carga, 
                                        int tiempo_descarga, int tiempo_libre, int id_evento, int tamano_icono, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_entidad_patio, id_ubicacion_patio, (byte)tipo_entidad, (byte)estatus, (byte)estatus_carga, geoubicacion, 
                                 color_hxd, descripcion, id_zona_patio, fecha_estatus, coordenada_x, coordenada_y, tiempo_carga, 
                                 tiempo_descarga, tiempo_libre, id_evento, tamano_icono, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Entidades de Patio
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="geoubicacion">Geoubicación de la Entidad</param>
        /// <param name="color_hxd">Color en Formato Hexadecimal</param>
        /// <param name="descripcion">Descripción de la Entidad</param>
        /// <param name="id_zona_patio">Zona del Patio a la que pretenece la Entidad</param>
        /// <param name="fecha_estatus">Fecha de Actualización del Estatus</param>
        /// <param name="coordenada_x">Coordenada X</param>
        /// <param name="coordenada_y">Coordenada Y</param>
        /// <param name="tiempo_carga">Tiempo de Carga</param>
        /// <param name="tiempo_descarga">Tiempo de Descarga</param>
        /// <param name="tiempo_libre">Tiempo Libre</param>
        /// <param name="id_evento">Evento Actual de la Entidad</param>
        /// <param name="tamano_icono">Tamaño del Icono</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaEntidadPatio(int id_ubicacion_patio, TipoEntidad tipo_entidad, SqlGeography geoubicacion,
                                        string color_hxd, string descripcion, int id_zona_patio, DateTime fecha_estatus, decimal coordenada_x, decimal coordenada_y, int tiempo_carga,
                                        int tiempo_descarga, int tiempo_libre, int id_evento, int tamano_icono, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_ubicacion_patio, (byte)tipo_entidad, (byte)Estatus.Vacio, (byte)EstatusCarga.Ninguno, geoubicacion, 
                                 color_hxd, descripcion, id_zona_patio, fecha_estatus, coordenada_x, coordenada_y, tiempo_carga, 
                                 tiempo_descarga, tiempo_libre, id_evento, tamano_icono, id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Entidades de Patio
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="estatus_carga">Estatus de Carga</param>
        /// <param name="geoubicacion">Geoubicación de la Entidad</param>
        /// <param name="color_hxd">Color en Formato Hexadecimal</param>
        /// <param name="descripcion">Descripción de la Entidad</param>
        /// <param name="id_zona_patio">Zona del Patio a la que pretenece la Entidad</param>
        /// <param name="fecha_estatus">Fecha de Actualización del Estatus</param>
        /// <param name="coordenada_x">Coordenada X</param>
        /// <param name="coordenada_y">Coordenada Y</param>
        /// <param name="tiempo_carga">Tiempo de Carga</param>
        /// <param name="tiempo_descarga">Tiempo de Descarga</param>
        /// <param name="tiempo_libre">Tiempo Libre</param>
        /// <param name="id_evento">Evento Actual de la Entidad</param>
        /// <param name="tamano_icono">Tamaño del Icono</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaEntidadPatio(int id_ubicacion_patio, TipoEntidad tipo_entidad, Estatus estatus, EstatusCarga estatus_carga, SqlGeography geoubicacion,
                                        string color_hxd, string descripcion, int id_zona_patio, DateTime fecha_estatus, decimal coordenada_x, decimal coordenada_y, int tiempo_carga,
                                        int tiempo_descarga, int tiempo_libre, int id_evento, int tamano_icono, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_ubicacion_patio, tipo_entidad, estatus, estatus_carga, geoubicacion,
                                 color_hxd, descripcion, id_zona_patio, fecha_estatus, coordenada_x, coordenada_y, tiempo_carga,
                                 tiempo_descarga, tiempo_libre, id_evento, tamano_icono, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Entidades de Patio
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaEntidadPatio(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_ubicacion_patio, (TipoEntidad)this._id_tipo_entidad, (Estatus)this._id_estatus, (EstatusCarga)this._id_estatus_carga, this._geoubicacion,
                                 this._color_hxd, this._descripcion, this._id_zona_patio, this._fecha_estatus, this._coordenada_x, this._coordenada_y, this._tiempo_carga,
                                 this._tiempo_descarga, this._tiempo_libre, this._id_evento, this._tamano_icono, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de las Entidades de Patio
        /// </summary>
        /// <returns></returns>
        public bool ActualizaEntidadPatio()
        {   //Invocando Método de Carga de Atributos
            return this.cargaAtributosInstancia(this._id_entidad_patio);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Evento Actual de la Entidad de Patio
        /// </summary>
        /// <param name="estatus">Estatus de la Entidad (Ocupado, Vacio)</param>
        /// <param name="estatus_carga">Estatus de Carga de la Entidad (Ninguno, Cargando, Descargando, Estacionando)</param>
        /// <param name="fecha_estatus">Fecha de Actualización del Estatus</param>
        /// <param name="id_evento">Evento Actual</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEventoActualEntidadPatio(Estatus estatus, EstatusCarga estatus_carga, DateTime fecha_estatus, int id_evento, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_ubicacion_patio, (TipoEntidad)this._id_tipo_entidad, estatus, estatus_carga, this._geoubicacion,
                                 this._color_hxd, this._descripcion, this._id_zona_patio, fecha_estatus, this._coordenada_x, this._coordenada_y, this._tiempo_carga,
                                 this._tiempo_descarga, this._tiempo_libre, id_evento, this._tamano_icono, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Cargar las Entidades en la Zona según su Tipo
        /// </summary>
        /// <param name="id_zona_patio">Zona de Patio</param>
        /// <returns></returns>
        public static DataTable CargaEntidadesZona(int id_zona_patio)
        {   //Declarando Objeto de Retorno
            DataTable dtEntidades = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, 0, 0, null, "", "", id_zona_patio, null, 0, 0, null, null, null, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Entidades
                    dtEntidades = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtEntidades;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Entidades(Anden/Cajon) junto con su Estatus Actual
        /// </summary>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public static DataTable CargaEstatusEntidadesActuales(TipoEntidad tipo_entidad, int id_ubicacion_patio)
        {
            //Declaramos un objeto datatable
            DataTable t = null;
            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, "", "" };

            //Obtenemos la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que exista la tabla deseada
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Entidades
                    t = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return t;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Entidades(Anden/Cajon) junto con su Estatus Actual
        /// </summary>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <returns></returns>
        public static DataSet CargaEstatusEntidadesGenerales(TipoEntidad tipo_entidad, int id_ubicacion_patio, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Armando Arreglo de Parametros
            object[] param = { 6, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, 
                                 fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))

                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Método Público encargado de Cargar los Indicadores de las Entidades de Patio
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public static DataTable RetornaIndicadoresEntidad(int id_ubicacion_patio)
        {   
            //Declarando Objeto de Retorno
            DataTable dtEntidades = null;
            
            //Armando Arreglo de Parametros
            object[] param = { 7, 0, id_ubicacion_patio, 0, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, "", "" };
            
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Entidades
                    dtEntidades = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtEntidades;
        }
        /// <summary>
        /// Método Público encargado de Cargar los Indicadores de las Entidades de Patio
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <returns></returns>
        public static DataTable RetornaIndicadoresEntidad(int id_ubicacion_patio, DateTime fecha_inicio, DateTime fecha_fin)
        {   
            //Declarando Objeto de Retorno
            DataTable dtEntidades = null;
            
            //Armando Arreglo de Parametros
            object[] param = { 8, 0, id_ubicacion_patio, 0, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, 
                                 fecha_inicio != DateTime.MinValue ? fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) : "", 
                                 fecha_fin != DateTime.MinValue ? fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) : "" };
            
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Entidades
                    dtEntidades = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtEntidades;
        }
        /// <summary>
        /// Método Publico encargado de Obtener los Indicadores dadas las Coordenadas X - Y
        /// </summary>
        /// <param name="coordenada_x">Coordenada X</param>
        /// <param name="coordenada_y">Coordenada Y</param>
        /// <returns></returns>
        public static DataTable ObtieneIndicadoresPorEntidad(decimal coordenada_x, decimal coordenada_y)
        {
            //Declarando Objeto de Retorno
            DataTable dtEntidades = null;

            //Armando Arreglo de Parametros
            object[] param = { 9, 0, 0, 0, 0, 0, null, "", "", 0, null, coordenada_x, coordenada_y, null, null, null, 0, 0, 0, false, "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Entidades
                    dtEntidades = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtEntidades;
        }

        /// <summary>
        /// Metodo encargado de realizar la consulta que muestra la cantidad de eventos realizados por hora por entidad
        /// </summary>
        /// <param name="tipo_entidad"></param>
        /// <param name="id_ubicacion_patio"></param>
        /// <returns></returns>
        public static DataSet CargaOcupacionEntidadHora(TipoEntidad tipo_entidad, int id_ubicacion_patio)
        {
            //Armando Arreglo de Parametros
            object[] param = { 10, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, 
                                 "", ""};

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Devolviendo Resultado Obtenido
                return ds;
            }               
        }
       
        /// <summary>
        /// Metodo encargado de realizar la consulta que muestra la cantidad de eventos de carga/descarga realizados por hora por entidad
        /// </summary>
        /// <param name="tipo_entidad"></param>
        /// <param name="id_ubicacion_patio"></param>
        /// <param name="tipo_evento"></param>
        /// <returns></returns>
        public static DataSet CargaOcupacionEntidadHora(TipoEntidad tipo_entidad, int id_ubicacion_patio, int tipo_evento)
        {
            //Armando Arreglo de Parametros
            object[] param = { 14, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, 
                                 tipo_evento, ""};

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Devolviendo Resultado Obtenido
                return ds;
            }
        }

        /// <summary>
        /// Consulta que muestra la dispersion de eventos realizados el dia de hoy de acuerdo al tiempo de ejecucion
        /// </summary>
        /// <param name="tipo_entidad"></param>
        /// <param name="id_ubicacion_patio"></param>
        /// <returns></returns>
        public static DataTable CargaResumenEventosTiempo(TipoEntidad tipo_entidad, int id_ubicacion_patio)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando Arreglo de Parametros
            object[] param = { 11, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, 
                                 "", ""};

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Entidades
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }
        
        /// <summary>
        /// Carga la dispersion de eventos actuales por tiempo de ejecucion.
        /// </summary>
        /// <param name="tipo_entidad"></param>
        /// <param name="id_ubicacion_patio"></param>
        /// <param name="estatus_carga"></param>
        /// <returns></returns>
        public static DataTable CargaResumenEventosTiempo(TipoEntidad tipo_entidad, int id_ubicacion_patio, EstatusCarga estatus_carga)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando Arreglo de Parametros
            object[] param = { 13, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, (byte)estatus_carga, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, 
                                 DateTime.MinValue.ToString(), DateTime.MinValue.ToString()};

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Entidades
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }
       
        /// <summary>
        /// Metodo encargado de carga el resumen de entidades agrupadas por estatus
        /// </summary>
        /// <param name="tipo_entidad"></param>
        /// <param name="id_ubicacion_patio"></param>
        /// <returns></returns>
        public static DataTable CargaResumenEstatusEntidades(TipoEntidad tipo_entidad, int id_ubicacion_patio)
        {
            //Declaramos un objeto datatable
            DataTable t = null;
            //Armando Arreglo de Parametros
            object[] param = { 12, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, "", "" };

            //Obtenemos la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista la tabla deseada
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Entidades
                    t = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return t;
        }

        /// <summary>
        /// Carga el resumen de minutos ocupados y disponibles de acuerdo al tipo de entidad
        /// </summary>
        /// <param name="tipo_entidad"></param>
        /// <param name="id_ubicacion_patio"></param>
        /// <returns></returns>
        public static DataTable CargaResumenTiempoEntidad(TipoEntidad tipo_entidad, int id_ubicacion_patio)
        {
            //Declaramos un objeto datatable
            DataTable t = null;
            //Armando Arreglo de Parametros
            object[] param = { 15, 0, id_ubicacion_patio, (byte)tipo_entidad, 0, 0, null, "", "", 0, null, 0, 0, null, null, null, 0, 0, 0, false, "", "" };

            //Obtenemos la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista la tabla deseada
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Entidades
                    t = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return t;
        }
        #endregion
    }
}
