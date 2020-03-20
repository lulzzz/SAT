using Microsoft.SqlServer.Types;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using TSDK.Base;

namespace SAT_CL.Maps
{
    /// <summary>
    /// 
    /// </summary>
    public class StaticMap
    {
        #region Estructuras

        /// <summary>
        /// Clase encargada de Crear un Marcador
        /// </summary>
        public class MarcadorMapa
        {
            private Color _color;
            /// <summary>
            /// Especifica el Color del Marcador
            /// </summary>
            public Color color { get { return this._color; } }
            private SqlGeography _ubicacion;
            /// <summary>
            /// Especifica la Ubicación del Marcador
            /// </summary>
            public SqlGeography ubicacion { get { return this._ubicacion; } }
            private char _etiqueta;
            /// <summary>
            /// Especifica la Etiqueta del Marcardor
            /// </summary>
            public char etiqueta { get { return this._etiqueta; } }

            /// <summary>
            /// Contructor que Inicializa los Valores del Marcador
            /// </summary>
            /// <param name="color"></param>
            /// <param name="ubicacion"></param>
            /// <param name="etiqueta"></param>
            public MarcadorMapa(Color color, SqlGeography ubicacion, char etiqueta)
            {
                //Asignando Atributos
                this._color = color;
                this._ubicacion = ubicacion;
                this._etiqueta = etiqueta;
            }
        }
        /// <summary>
        /// Clase encargada de Crear un Poligono 
        /// </summary>
        public class PoligonoMapa
        {
            private Color _color_camino;
            /// <summary>
            /// Especifica el Color del Camino
            /// </summary>
            public Color color { get { return this._color_camino; } }
            private Color _color_relleno;
            /// <summary>
            /// Especifica el Color de Relleno
            /// </summary>
            public Color color_relleno { get { return this._color_relleno; } }
            private byte _tamano;
            /// <summary>
            /// Especifica el Tamaño del Recorrido
            /// </summary>
            public byte tamano { get { return this._tamano; } }
            private SqlGeography _ubicacion;
            /// <summary>
            /// Especifica los datos de la Ubicación del Poligono en el Mapa
            /// </summary>
            public SqlGeography ubicacion { get { return this._ubicacion; } }
            private char _etiqueta;
            /// <summary>
            /// Especifica los datos de la Etiqueta en caso de ser un solo Punto
            /// </summary>
            public char etiqueta { get { return this._etiqueta; } }

            /// <summary>
            /// Constructor que Inicializa los Valores del Poligono
            /// </summary>
            /// <param name="color_camino"></param>
            /// <param name="color_relleno"></param>
            /// <param name="tamano"></param>
            /// <param name="ubicacion"></param>
            /// <param name="etiqueta"></param>
            public PoligonoMapa(Color color_camino, Color color_relleno, byte tamano, SqlGeography ubicacion, char etiqueta)
            {
                //Asignando Valores
                this._color_camino = color_camino;
                this._color_relleno = color_relleno;
                this._tamano = tamano;
                this._ubicacion = ubicacion;
                this._etiqueta = etiqueta;
            }
        }

        #endregion

        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa los Tipos de Mapas
        /// </summary>
        public enum TipoMapa
        {
            /// <summary>
            /// Expresa el Tipo de Mapa "Roadmap"
            /// </summary>
            Camino = 1,
            /// <summary>
            /// Expresa el Tipo de Mapa "Satellite"
            /// </summary>
            Satelite = 2,
            /// <summary>
            /// Expresa el Tipo de Mapa "Hybrid"
            /// </summary>
            Hibrido = 3,
            /// <summary>
            /// Expresa el Tipo de Mapa "Terrain"
            /// </summary>
            Terreno = 4
        }

        #endregion

        #region Atributos Privados

        /// <summary>
        /// Atributo que almacena el Objeto Instanciado por la Misma Clase
        /// </summary>
        private static StaticMap _staticMap;
        /// <summary>
        /// Atributo que almacena la Petición Web
        /// </summary>
        private HttpWebRequest _peticionWeb;
        /// <summary>
        /// Atributo que almacena la Respuesta Web
        /// </summary>
        private HttpWebResponse _respuestaWeb;
        /// <summary>
        /// Atributo que almacena la URL (API)
        /// </summary>
        private string _url;
        /// <summary>
        /// Atributo que almacena la Llave Web (API)
        /// </summary>
        private string _key;

        #endregion

        #region Atributos Públicos

        /// <summary>
        /// Atributo que Obtiene la Unica Instancia de la Clase
        /// </summary>
        public static StaticMap objStaticMap
        {
            get
            {
                //Validando que no Exista la Instancia
                if (_staticMap == null)
                {
                    lock (new object())
                    {
                        //Si no existe
                        if (_staticMap == null)

                            //Creando una Nueva Instancia
                            _staticMap = new StaticMap();
                    }
                }

                //Devolviendo Resultado Obtenido
                return _staticMap;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        private StaticMap()
        {
            try
            {
                //Asignando Valores de Conexión
                this._url = ConfigurationManager.AppSettings["StaticMapAPI_URL"];
                this._key = ConfigurationManager.AppSettings["StaticMapAPI_SAT_DriverWebKey"];
            }
            catch (Exception) { }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Obtener la respuesta según la Configuración Solicitada
        /// </summary>
        /// <param name="url_parametros">URL con la Configuración Solicitada</param>
        /// <param name="imagen">Imagen de Salida (Resultado de la Petición)</param>
        /// <returns></returns>
        private RetornoOperacion obtieneRespuestaPeticion(string url_parametros, out Bitmap imagen)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            imagen = null;

            //Creando Objeto Uri
            Uri url_final = new Uri(url_parametros);

            //Creando Petición Web
            this._peticionWeb = (HttpWebRequest)WebRequest.Create(url_final);

            try
            {
                //Obteniendo Respuesta de la Petición
                this._respuestaWeb = (HttpWebResponse)this._peticionWeb.GetResponse();

                //Leyendo Respuesta
                Stream imageStream = this._respuestaWeb.GetResponseStream();

                //Creando Imagen
                imagen = new Bitmap(imageStream);

                //Asignando Resultado Positivo
                result = new RetornoOperacion(0, "La Imagen fue Obtenida con Exito", true);
            }
            catch (Exception e)
            {
                //Instanciando Excepción
                result = new RetornoOperacion(e.Message);
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Traducir los Tipos de Mapa
        /// </summary>
        /// <param name="tipoMapa"></param>
        /// <returns></returns>
        private string traduceTipoMapa(TipoMapa tipoMapa)
        {
            //Declarando Objeto de Retorno
            string tipo = "";

            //Evaluando Tipo de Petición
            switch (tipoMapa)
            {
                case TipoMapa.Camino:
                    tipo = "roadmap";
                    break;
                case TipoMapa.Satelite:
                    tipo = "satellite";
                    break;
                case TipoMapa.Hibrido:
                    tipo = "hybrid";
                    break;
                case TipoMapa.Terreno:
                    tipo = "terrain";
                    break;
            }

            //Devolviendo Resultado Obtenido
            return tipo;
        }
        /// <summary>
        /// Método encargado de Convertir un Objeto Poligono en Cadena para la Petición
        /// </summary>
        /// <param name="poligono">Poligono Deseado</param>
        /// <returns></returns>
        private string conviertePoligonoCadena(PoligonoMapa poligono)
        {
            //Declarando Objeto de Retorno
            string result = "";

            //Validando que exista una Ubicación
            if (poligono.ubicacion != SqlGeography.Null)
            {
                //Validando tipo de Ubicación
                switch(poligono.ubicacion.STGeometryType().Value)
                {
                    case "Point":
                        {
                            //Creando Marcador
                            result = string.Format("markers=color:0x{0}%7Clabel:{1}%7C{2},{3}",
                                          poligono.color.R.ToString("X2") + poligono.color.G.ToString("X2") + poligono.color.B.ToString("X2"),
                                          poligono.etiqueta.ToString().Equals("") ? "D" : poligono.etiqueta.ToString().ToUpper(), 
                                          poligono.ubicacion.Lat, poligono.ubicacion.Long);
                            break;
                        }
                    case "LineString":
                    case "CompoundCurve":
                    case "Polygon":
                    case "CurvePolygon":
                        {
                            //Obteniendo Conjunto de puntos
                            SqlGeography[] puntos_ubicacion = DatosEspaciales.ObtienePuntosPolygon(poligono.ubicacion, (int)poligono.ubicacion.STSrid.Value, out result);

                            //Validando Resultado Positivo
                            if (result.Equals(""))
                            {
                                //Asignando Atributo Generales
                                result = string.Format("path=color:0x{0}|weight:{1}|fillcolor:0x{2}",
                                                poligono.color.R.ToString("X2") + poligono.color.G.ToString("X2") + poligono.color.B.ToString("X2"),
                                                poligono.tamano,
                                                poligono.color_relleno.R.ToString("X2") + poligono.color_relleno.G.ToString("X2") + poligono.color_relleno.B.ToString("X2"));
                                
                                //Recorriendo Puntos
                                foreach (SqlGeography punto in puntos_ubicacion)
                                {
                                    //Validando que sea de Tipo Punto
                                    if (punto.STGeometryType().Value.Equals("Point"))

                                        //Asignando Ubicación en Punto
                                        result += string.Format("|{0},{1}", punto.Lat, punto.Long);
                                    else
                                        break;
                                }
                            }
                            else
                                //Limpiando Retorno
                                result = "";

                            break;
                        }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Convertir el Marcador en Cadena para la Petición
        /// </summary>
        /// <param name="marcador">Marcador Deseado</param>
        /// <returns></returns>
        private string convierteMarcadorCadena(MarcadorMapa marcador)
        {
            //Declarando Objeto de Retorno
            string result = "";

            //Validando que exista una Ubicación
            if (marcador.ubicacion != SqlGeography.Null)
            {
                //Validando tipo de Ubicación
                switch (marcador.ubicacion.STGeometryType().Value)
                {
                    case "Point":
                        {
                            //Creando Marcador
                            result = string.Format("markers=color:0x{0}%7Clabel:{1}%7C{2},{3}",
                                          marcador.color.R.ToString("X2") + marcador.color.G.ToString("X2") + marcador.color.B.ToString("X2"),
                                          marcador.etiqueta.ToString().ToUpper().Equals("") ? "A" : marcador.etiqueta.ToString().ToUpper(), 
                                          marcador.ubicacion.Lat, marcador.ubicacion.Long);
                            break;
                        }
                    case "LineString":
                    case "CompoundCurve":
                    case "Polygon":
                    case "CurvePolygon":
                        {
                            //Obteniendo Conjunto de puntos
                            SqlGeography[] puntos_ubicacion = DatosEspaciales.ObtienePuntosPolygon(marcador.ubicacion, (int)marcador.ubicacion.STSrid.Value, out result);

                            //Validando Resultado Positivo
                            if (result.Equals(""))
                            {
                                //Asignando Atributo Generales
                                result = string.Format("markers=color:0x{0}%7Clabel:{1}",
                                                marcador.color.R.ToString("X2") + marcador.color.R.ToString("X2") + marcador.color.R.ToString("X2"),
                                                marcador.etiqueta.ToString().ToUpper().Equals("") ? "A" : marcador.etiqueta.ToString().ToUpper());

                                //Recorriendo Puntos
                                foreach (SqlGeography punto in puntos_ubicacion)
                                {
                                    //Validando que sea de Tipo Punto
                                    if (punto.STGeometryType().Value.Equals("Point"))

                                        //Asignando Ubicación en Punto
                                        result += string.Format("%7C{0},{1}", punto.Lat, punto.Long);
                                    else
                                        break;
                                }
                            }
                            else
                                //Limpiando Retorno
                                result = "";

                            break;
                        }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Obtener el Mapa de la Ubicación. (Definiendo el Centro del Mapa, el Zoom, Marcando uno o varios Puntos en el Mapa)
        /// </summary>
        /// <param name="centro">Centro del Mapa</param>
        /// <param name="tipoMapa">Tipo de Mapa</param>
        /// <param name="zoom">Enfoque del Mapa</param>
        /// <param name="ancho">Ancho del Mapa</param>
        /// <param name="alto">Alto del Mapa</param>
        /// <param name="marcador">Marcador (Etiqueta, Color, Latitud, Longitud) del Mapa</param>
        /// <param name="imagen">Parametro de Imagen de Salida</param>
        /// <returns></returns>
        public RetornoOperacion ObtieneMapaUbicacion(SqlGeography centro, TipoMapa tipoMapa, int zoom, int ancho, int alto, MarcadorMapa marcador,
                                                     out Bitmap imagen)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            imagen = null;

            //Si el Zoom es mayor a 0
            if (zoom > 0)
            {
                //Validando que existan Alto y Ancho
                if (ancho > 0 && alto > 0)
                {
                    //Validando que exista la Ubicación del Marcador
                    if (marcador.ubicacion != SqlGeography.Null)
                    {
                        //Creando URL de Petición
                        string url_parametros = string.Format("{0}?center={1},{2}&zoom={3}&size={4}x{5}&maptype={6}&{7}&key={11}",
                                    this._url, centro.STGeometryType().Value.Equals("Point") ? centro.Lat : centro.EnvelopeCenter().Lat,
                                    centro.STGeometryType().Value.Equals("Point") ? centro.Long : centro.EnvelopeCenter().Long,
                                    zoom, ancho, alto, traduceTipoMapa(tipoMapa), convierteMarcadorCadena(marcador), this._key);

                        //Ejecutando Petición Web
                        result = obtieneRespuestaPeticion(url_parametros, out imagen);
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("Debe de Especificar una Ubicación");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Valores de Ancho y Alto Invalidos");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe de Especificar el Valor Zoom");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener el Mapa de la Ubicación. (Definiendo un Zoom, marcando una o varias Posiciones en el Mapa)
        /// </summary>
        /// <param name="tipoMapa">Tipo de Mapa</param>
        /// <param name="zoom">Enfoque del Mapa</param>
        /// <param name="ancho">Ancho del Mapa</param>
        /// <param name="alto">Alto del Mapa</param>
        /// <param name="marcador">Marcador (Etiqueta, Color, Latitud, Longitud) del Mapa</param>
        /// <param name="imagen">Parametro de Imagen de Salida</param>
        /// <returns></returns>
        public RetornoOperacion ObtieneMapaUbicacion(TipoMapa tipoMapa, int zoom, int ancho, int alto, MarcadorMapa marcador, 
                                                     out Bitmap imagen)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            imagen = null;

            //Si el Zoom es mayor a 0
            if (zoom > 0)
            {
                //Validando que existan Alto y Ancho
                if (ancho > 0 && alto > 0)
                {
                    //Validando Marcador
                    if (marcador.ubicacion != SqlGeography.Null)
                    {
                        //Creando URL de Petición
                        string url_parametros = string.Format("{0}?zoom={1}&size={2}x{3}&maptype={4}&{5}&key={6}",
                                                              this._url, zoom, ancho, alto, traduceTipoMapa(tipoMapa), convierteMarcadorCadena(marcador), this._key);

                        //Ejecutando Petición Web
                        result = obtieneRespuestaPeticion(url_parametros, out imagen);
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("Debe de Especificar una Ubicación");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Valores de Ancho y Alto Invalidos");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe de Especificar el Valor Zoom");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener el Mapa de la Ubicación. (Marcando una o Varias Posiciones en el Mapa)
        /// </summary>
        /// <param name="tipoMapa">Tipo de Mapa</param>
        /// <param name="ancho">Ancho del Mapa</param>
        /// <param name="alto">Alto del Mapa</param>
        /// <param name="marcador">Marcador (Etiqueta, Color, Latitud, Longitud) del Mapa</param>
        /// <param name="imagen">Parametro de Imagen de Salida</param>
        /// <returns></returns>
        public RetornoOperacion ObtieneMapaUbicacion(TipoMapa tipoMapa, int ancho, int alto, MarcadorMapa marcador, out Bitmap imagen)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            imagen = null;

            //Validando que existan Alto y Ancho
            if (ancho > 0 && alto > 0)
            {
                //Validando Marcador
                if (marcador.ubicacion != SqlGeography.Null)
                {
                    //Creando URL de Petición
                    string url_parametros = string.Format("{0}?size={1}x{2}&maptype={3}&{4}&key={5}",
                                                          this._url, ancho, alto, traduceTipoMapa(tipoMapa), convierteMarcadorCadena(marcador), this._key);

                    //Ejecutando Petición Web
                    result = obtieneRespuestaPeticion(url_parametros, out imagen);
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe de Especificar una Ubicación");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Valores de Ancho y Alto Invalidos");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener el Mapa de la Ubicación (Dando un Origen y un Destino).
        /// </summary>
        /// <param name="tipoMapa">Tipo de Mapa</param>
        /// <param name="ancho">Ancho del Mapa</param>
        /// <param name="alto">Alto del Mapa</param>
        /// <param name="marcador">Marcador Origen (Etiqueta, Color, Latitud, Longitud) del Mapa</param>
        /// <param name="ubicacion_destino">Poligono Destino (Color, Color Relleno, Tamano, Ubicación y Etiqueta) del Mapa</param>
        /// <param name="imagen">Parametro de Imagen de Salida</param>
        /// <returns></returns>
        public RetornoOperacion ObtieneMapaUbicacion(TipoMapa tipoMapa, int ancho, int alto, MarcadorMapa marcador_origen,
                                                     PoligonoMapa ubicacion_destino, out Bitmap imagen)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            string url_parametros = "";
            imagen = null;

            //Validando que existan Alto y Ancho
            if (ancho > 0 && alto > 0)
            {
                //Validando que exista la Ubicación de Destino
                if (ubicacion_destino.ubicacion != SqlGeography.Null)
                {
                    //Creando URL de Petición
                    url_parametros = string.Format("{0}?size={1}x{2}&maptype={3}&{4}&{5}&key={6}",
                                                    this._url, ancho, alto, traduceTipoMapa(tipoMapa),
                                                    convierteMarcadorCadena(marcador_origen), 
                                                    conviertePoligonoCadena(ubicacion_destino),
                                                    this._key);

                    //Ejecutando Petición Web
                    result = obtieneRespuestaPeticion(url_parametros, out imagen);
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe especificar un Destino");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Valores de Ancho y Alto Invalidos");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener el Mapa de la Ubicación (Dando un Origen, un Destino y el Centro del Destino).
        /// </summary>
        /// <param name="tipoMapa">Tipo de Mapa</param>
        /// <param name="ancho">Ancho del Mapa</param>
        /// <param name="alto">Alto del Mapa</param>
        /// <param name="marcador">Marcador Origen (Etiqueta, Color, Latitud, Longitud) del Mapa</param>
        /// <param name="ubicacion_destino">Poligono Destino (Color, Color Relleno, Tamano, Ubicación y Etiqueta) del Mapa</param>
        /// <param name="centro_destino">Definiendo el Centro del Mapa</param>
        /// <param name="imagen">Parametro de Imagen de Salida</param>
        /// <returns></returns>
        public RetornoOperacion ObtieneMapaUbicacion(TipoMapa tipoMapa, int ancho, int alto, MarcadorMapa marcador_origen,
                                                     PoligonoMapa ubicacion_destino, MarcadorMapa centro_destino, out Bitmap imagen)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            string url_parametros = "";
            imagen = null;

            //Validando que existan Alto y Ancho
            if (ancho > 0 && alto > 0)
            {
                //Validando que exista la Ubicación de Destino
                if (ubicacion_destino.ubicacion != SqlGeography.Null)
                {
                    //Creando URL de Petición
                    url_parametros = string.Format("{0}?size={1}x{2}&maptype={3}&{4}&{5}&{6}&key={7}",
                                                    this._url, ancho, alto, traduceTipoMapa(tipoMapa),
                                                    convierteMarcadorCadena(marcador_origen),
                                                    conviertePoligonoCadena(ubicacion_destino),
                                                    convierteMarcadorCadena(centro_destino),
                                                    this._key);

                    //Ejecutando Petición Web
                    result = obtieneRespuestaPeticion(url_parametros, out imagen);
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe especificar un Destino");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Valores de Ancho y Alto Invalidos");

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
