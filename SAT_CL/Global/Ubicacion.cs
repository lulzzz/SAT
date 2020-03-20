using System;
using System.Collections.Generic;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using Microsoft.SqlServer.Types;
using System.Drawing;


namespace SAT_CL.Global
{   
    /// <summary>
    /// Implementa los método para la administración de Ubicación
    /// </summary>
    public  class Ubicacion: Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Tipo Ubicación
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            /// Patios Cliente
            /// </summary>
            PatiosCliente = 1,
            /// <summary>
            ///Terminal
            /// </summary>
            Terminal,
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "global.sp_ubicacion_tu";

        private int _id_ubicacion;
        /// <summary>
        /// Describe el Id Ubicación
        /// </summary>
        public int id_ubicacion { get { return _id_ubicacion; } }
        private string _descripcion;
        /// <summary>
        /// Describe la descripción
        /// </summary>
        public string descripcion { get { return _descripcion; } }
        private byte _id_tipo_ubicacion;
        /// <summary>
        /// Describe el tipo de Ubicación
        /// </summary>
        public byte id_tipo_ubicacion { get { return _id_tipo_ubicacion; } }
        private bool _bit_especifico_compania;
        /// <summary>
        /// Describe el bit Especifico
        /// </summary>
        public bool bit_especifico_compania { get { return _bit_especifico_compania; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Describe el Id Compañia Emisor
        /// </summary>
        public int id_compania_emisor { get { return _id_compania_emisor; } }
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
        private string _direccion;
        /// <summary>
        /// Describe la dirección
        /// </summary>
        public string direccion { get { return _direccion; } }
        private int _id_ciudad;
        /// <summary>
        /// Describe el Id Ciudad
        /// </summary>
        public int id_ciudad { get { return _id_ciudad; } }
        private string _ciudad;
        /// <summary>
        /// Describe la Ciudad
        /// </summary>
        public string ciudad { get { return _ciudad; } }
        private string _codigo_postal;
        /// <summary>
        /// Describe el Codigo Postal
        /// </summary>
        public string codigo_postal { get { return _codigo_postal; } }
        private string _telefono;
        /// <summary>
        /// Describe el Telefono
        /// </summary>
        public string telefono { get { return _telefono; } }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar { get { return _habilitar; } }
        /// <summary>
        /// Enumera el Tipo Ubicación
        /// </summary>
        public Tipo TipoUbicacion { get { return (Tipo)_id_tipo_ubicacion; } }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Ubicacion()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public Ubicacion()
        {
            
        }
        /// <summary>
        /// Genera una Instancia Ubicación
        /// </summary>
        /// <param name="id_ubicacion"></param>
        public Ubicacion(int id_ubicacion)
        {
            cargaAtributosInstancia(id_ubicacion);
        }

        /// <summary>
        /// Genera una Instancia de Ubicación
        /// </summary>
        /// <param name="ubicacion"></param>
        /// /// <param name="compania"></param>
        public Ubicacion(string ubicacion, int compania)
        {
            cargaAtributosInstancia(ubicacion,compania);
        }
        /// <summary>
        /// Genera una Instancia Ubicación
        /// </summary>
        /// <param name="id_ubicacion">Establece Id de la Ubicación</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_ubicacion)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = {3, id_ubicacion, "", 0, false, 0, SqlGeography.Null, "", 0, "", "", 0, false, "", ""};

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_ubicacion =  Convert.ToInt32(r["Id"]);
                        _descripcion = r["Descripcion"].ToString();
                        _id_tipo_ubicacion = Convert.ToByte(r["IdTipoUbicacion"]);
                        _bit_especifico_compania = Convert.ToBoolean(r["BitEspecificoCompania"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _geoubicacion = (SqlGeography)r["Geoubicacion"];
                        _direccion = r["Direccion"].ToString();
                        _id_ciudad = Convert.ToInt32(r["IdCiudad"]);
                        _ciudad = r["Ciudad"].ToString();
                        _codigo_postal = r["CodigoPostal"].ToString();
                        _telefono = r["Telefono"].ToString();
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Genera una Instancia Ubicación ligando la descripción.
        /// </summary>
        /// <param name="ubicacion">Establece la descripción de la  Ubicación</param>
        /// <param name="Compania">Establece la descripción de la  Ubicación</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(string ubicacion, int compania)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 4, 0, ubicacion, 0, false, compania, SqlGeography.Null, "", 0, "", "", 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_ubicacion = Convert.ToInt32(r["Id"]);
                        _descripcion = r["Descripcion"].ToString();
                        _id_tipo_ubicacion = Convert.ToByte(r["IdTipoUbicacion"]);
                        _bit_especifico_compania = Convert.ToBoolean(r["BitEspecificoCompania"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _geoubicacion = (SqlGeography)r["Geoubicacion"];
                        _direccion = r["Direccion"].ToString();
                        _id_ciudad = Convert.ToInt32(r["IdCiudad"]);
                        _ciudad = r["Ciudad"].ToString();
                        _codigo_postal = r["CodigoPostal"].ToString();
                        _telefono = r["Telefono"].ToString();
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
        /// Método encargado de Editar una Ubicación
        /// </summary>
        /// <param name="descripcion">Descripción de la ubicación</param>
        /// <param name="tipo_ubicacion">Tipo de Ubicaión</param>
        /// <param name="bit_especifico_compania">En caso de ser ubicación especifica, establece bit especifico compañia</param>
        /// <param name="id_compania_emisor">En caso de ser ubicación especifica, compañia a la que pertenece</param>
        /// <param name="geoubicacion">Geoubicación</param>
        /// <param name="direccion">Dirección de la ubicación</param>
        /// <param name="id_ciudad">Ciudad de la Ubicación</param>
        /// <param name="codigo_postal">Codigo Postal de la Ubicación</param>
        /// <param name="telefono">Telefono de la ubicación</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaUbicacion(string descripcion, Tipo tipo_ubicacion,bool bit_especifico_compania, int id_compania_emisor, SqlGeography geoubicacion,
                                                string direccion, int id_ciudad, string codigo_postal,string telefono,int id_usuario, bool habilitar)
        {

            //Inicializando arreglo de parámetros
            object[] param = {2, this._id_ubicacion, descripcion, tipo_ubicacion, bit_especifico_compania, id_compania_emisor, geoubicacion,
                                 direccion, id_ciudad, codigo_postal, telefono, id_usuario, habilitar, "", ""};
            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        #endregion

        #region Metodos publicos

        /// <summary>
        /// Método encargado de Insertar una Ubicación
        /// </summary>
        /// <param name="descripcion">Descripción de la ubicación</param>
        /// <param name="tipo_ubicacion">Tipo de Ubicaión</param>
        /// <param name="bit_especifico_compania">En caso de ser ubicación especifica, establece bit especifico compañia</param>
        /// <param name="id_compania_emisor">En caso de ser ubicación especifica, compañia a la que pertenece</param>
        /// <param name="geo_ubicacion">Geoubicación</param>
        /// <param name="direccion">Dirección de la ubicación</param>
        /// <param name="id_ciudad">Ciudad de la Ubicación</param>
        /// <param name="codigo_postal">Codigo Postal de la Ubicación</param>
        /// <param name="telefono">Telefono de la ubicación</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUbicacion(string descripcion, Tipo tipo_ubicacion, bool bit_especifico_compania, int id_compania_emisor, SqlGeography geo_ubicacion,
                                string direccion, int id_ciudad, string codigo_postal, string telefono, int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, descripcion, tipo_ubicacion, bit_especifico_compania, id_compania_emisor, geo_ubicacion,
                                 direccion, id_ciudad, codigo_postal, telefono, id_usuario, true, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de Editar una Ubicación
        /// </summary>
        /// <param name="descripcion">Descripción de la ubicación</param>
        /// <param name="tipo_ubicacion">Tipo de Ubicaión</param>
        /// <param name="bit_especifico_compania">En caso de ser ubicación especifica, establece bit especifico compañia</param>
        /// <param name="id_compania_emisor">En caso de ser ubicación especifica, compañia a la que pertenece</param>
        /// <param name="geo_ubicacion">Geoubicación</param>
        /// <param name="direccion">Dirección de la ubicación</param>
        /// <param name="id_ciudad">Ciudad de la Ubicación</param>
        /// <param name="codigo_postal">Codigo Postal de la Ubicación</param>
        /// <param name="telefono">Telefono de la ubicación</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaUbicacion(string descripcion, Tipo tipo_ubicacion, bool bit_especifico_compania, int id_compania_emisor, SqlGeography geo_ubicacion,
                                            string direccion, int id_ciudad, string codigo_postal, string telefono, 
                                                       int id_usuario)
        {

            return this.editaUbicacion(descripcion, tipo_ubicacion, bit_especifico_compania, id_compania_emisor, geo_ubicacion, direccion, id_ciudad,
                                      codigo_postal, telefono, id_usuario, this._habilitar);
        }


        /// <summary>
        /// Deshabilita un Servicio Despacho
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUbicacion(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            return this.editaUbicacion(this._descripcion,(Tipo)this._id_tipo_ubicacion, this._bit_especifico_compania, this._id_compania_emisor, this._geoubicacion, 
                                      this._direccion, this._id_ciudad, this._codigo_postal, this._telefono, id_usuario, false);

        }

        /// <summary>
        /// Devuelve la dirección de la ubicación en un formato legíble
        /// </summary>
        /// <returns></returns>
        public string ObtieneDireccionCompleta()
        {   //Instanciando Ciudad
            using (Ciudad ciu = new Ciudad(this._id_ciudad))
                //Armando y devolviendo la dirección con todos los elementos de la ubicación
                return string.Format("{0} Del./Mun. {1}, {2} [{3}] C.P. {4}", this._direccion, this._ciudad, ciu.estado, ciu.pais, this._codigo_postal).ToUpper();
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
        /// Construye una lista con los puntos que contiene su dato geográfico
        /// </summary>
        /// <param name="geo"></param>
        /// <returns></returns>
        public static List<PointF> RecuperaPuntosUbicacion(SqlGeography geo)
        {
            //Definiendo objeto principal para listar los puntos (lat, long) que conforman el poligono
            List<PointF> puntos = new List<PointF>();
            //Si la ubicación geográfica está definida
            if (!geo.IsNull)
            {
                //Determinando el tipo de geometría que representa el valor geográfico
                switch (geo.STGeometryType().Value)
                {
                    case "Point":
                        puntos.Add(new PointF((float)geo.Lat.Value, (float)geo.Long.Value));
                        break;
                    case "LineString":
                    case "CompoundCurve":
                    case "Polygon":
                    case "CurvePolygon":
                        //Determinando cuantos puntos posee el valor geográfico
                        int total_puntos = geo.STNumPoints().Value;
                        //Para cada uno de los puntos existentes
                        for (int x = 1; x < total_puntos; x++)
                        {
                            //Añadiendo el punto a la lista
                            puntos.Add(new PointF((float)geo.STPointN(x).Lat.Value, (float)geo.STPointN(x).Long.Value));
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

        #endregion

    }
}
