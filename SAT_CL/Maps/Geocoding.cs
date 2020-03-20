using System;
using System.Configuration;
using System.Net;
using System.Xml.Linq;
using TSDK.Base;

namespace SAT_CL.Maps
{
    /// <summary>
    /// Proporciona elementos para realizar conversiones de Goecodificacion y/o Geocodificación Inversa mediante Geocoding API de Google
    /// </summary>
    public sealed class Geocoding
    {
        #region Constructor

        /// <summary>
        /// Constructor no accesible
        /// </summary>
        private Geocoding()
        {
            try
            {
                //Inicialziando elementos requeridos de objeto
                _url = ConfigurationManager.AppSettings["GeocodingAPI_URL"];
                _key = ConfigurationManager.AppSettings["GeocodingAPI_SAT_DriverWebKey"];
            }
            catch (Exception ex)
            {
                _url = _key = "";
            }
        }
        #endregion

        #region Enumeraciones
        
        /// <summary>
        /// Idiomas aceptados en respuestas
        /// </summary>
        public enum IdiomaRespuesta
        {
            /// <summary>
            /// Inglés
            /// </summary>
            EN,
            /// <summary>
            /// Español
            /// </summary>
            ES
        }

        /// <summary>
        /// Tipos de Resultado solicitados en respuesta de Geocodificación
        /// </summary>
        public enum TipoResultado
        {
            /// <summary>
            /// Ningún filtro de tipo de ubicación
            /// </summary>
            def,
            /// <summary>
            /// Indica una dirección exacta.
            /// </summary>
            street_address,
            /// <summary>
            /// Indica la denominación de una carretera (como "US 101").
            /// </summary>
            route,
            /// <summary>
            /// Indica una intersección principal, generalmente de dos calles importantes.
            /// </summary>
            intersection,
            /// <summary>
            /// Indica una entidad política. Generalmente, este tipo indica un polígono de alguna administración pública.
            /// </summary>
            political,
            /// <summary>
            /// Indica la entidad política nacional, y es generalmente el tipo de orden más alto que devuelve el geocodificador.
            /// </summary>
            country,
            /// <summary>
            /// Indica una entidad civil de primer orden por debajo del nivel de país. En Estados Unidos, estos niveles administrativos son los estados. No todos los países poseen estos niveles administrativos. En la mayoría de los casos, los nombres cortos de administrative_area_level_1 coincidirán considerablemente con las subdivisiones de ISO 3166-2 y otras listas conocidas; sin embargo, no podemos garantizarlo debido a que nuestros resultados de geocodificación están basados en diferentes señales y datos de ubicación.
            /// </summary>
            administrative_area_level_1,
            /// <summary>
            /// Indica una entidad civil de segundo orden por debajo del nivel de país. En Estados Unidos, estos niveles administrativos son los condados. No todos los países poseen estos niveles administrativos.
            /// </summary>
            administrative_area_level_2,
            /// <summary>
            /// Indica una entidad civil de tercer orden por debajo del nivel de país. Este tipo indica una división civil inferior. No todos los países poseen estos niveles administrativos.I
            /// </summary>
            administrative_area_level_3,
            /// <summary>
            /// Indica una entidad civil de cuarto orden por debajo del nivel de país. Este tipo indica una división civil inferior. No todos los países poseen estos niveles administrativos.
            /// </summary>
            administrative_area_level_4,
            /// <summary>
            /// Indica una entidad civil de quinto orden por debajo del nivel de país. Este tipo indica una división civil inferior. No todos los países poseen estos niveles administrativos.
            /// </summary>
            administrative_area_level_5,
            /// <summary>
            /// Indica un nombre alternativo de uso frecuente para la entidad.
            /// </summary>
            colloquial_area,
            /// <summary>
            /// Indica una entidad política constituida de una ciudad o un pueblo.
            /// </summary>
            locality,
            /// <summary>
            /// Indica un tipo específico de localidad japonesa para facilitar la distinción entre los múltiples componentes de localidad en una dirección japonesa.
            /// </summary>
            ward,
            /// <summary>
            /// Indica una entidad civil de primer orden por debajo de una localidad. Algunas ubicaciones pueden recibir uno de los tipos adicionales: sublocality_level_1 a sublocality_level_5. Cada nivel de sublocalidad es una entidad civil. Los números más altos indican un área geográfica más pequeña.
            /// </summary>
            sublocality,
            /// <summary>
            /// Indica un barrio determinado.
            /// </summary>
            neighborhood,
            /// <summary>
            /// Indica una ubicación determinada, generalmente un edificio o un conjunto de edificios con un nombre en común.
            /// </summary>
            premise,
            /// <summary>
            /// Indica una entidad de primer orden por debajo de una ubicación determinada; generalmente un edificio en particular en un conjunto de edificios con un nombre en común.
            /// </summary>
            subpremise,
            /// <summary>
            /// Indica un código postal tal como se usa para identificar una dirección de correo postal dentro del país.
            /// </summary>
            postal_code,
            /// <summary>
            /// Indica una atracción natural destacada.
            /// </summary>
            natural_feature,
            /// <summary>
            /// Indica un aeropuerto.
            /// </summary>
            airport,
            /// <summary>
            /// Indica un parque determinado.
            /// </summary>
            park,
            /// <summary>
            /// Indica un punto de interés determinado. Generalmente, estos "PI" son entidades locales destacadas que no pueden incluirse fácilmente en otra categoría, como el edificio "Empire State" o la "Estatua de la libertad".
            /// </summary>
            point_of_interest
        }
        /// <summary>
        /// Datos adicionales de búsqueda
        /// </summary>
        public enum TipoUbicacion
        {
            /// <summary>
            /// Ningún filtro de tipo de ubicación
            /// </summary>
            DEF,
            /// <summary>
            /// Indica que el resultado devuelto es un geocódigo exacto para el cual contamos con información de ubicación precisa que puede delimitarse hasta la dirección.
            /// </summary>
            ROOFTOP,
            /// <summary>
            /// Indica que el resultado devuelto refleja una aproximación (generalmente en una calle) interpolada entre dos puntos precisos (como intersecciones). Generalmente se devuelven resultados interpolados cuando no se encuentran disponibles geocódigos exactos para una dirección.
            /// </summary>
            RANGE_INTERPOLATED,
            /// <summary>
            /// Indica que el resultado devuelto es el centro geométrico de un resultado como una polilínea (por ejemplo, una calle) o un polígono (región).
            /// </summary>
            GEOMETRIC_CENTER,
            /// <summary>
            /// Indica que el resultado devuelto es aproximado.
            /// </summary>
            APPROXIMATE
        }

        #endregion

        #region Atributos Privados

        /// <summary>
        /// Atributo que almacena el Objeto Instanciado por la Misma Clase (sólo cuando este sea requerido)
        /// </summary>
        private static readonly Lazy<Geocoding> _instance = new Lazy<Geocoding>(() => new Geocoding());
        /// <summary>
        /// Atributo que almacena la URL (API)
        /// </summary>
        private string _url;
        /// <summary>
        /// Atributo que almacena la Llave Web (API)
        /// </summary>
        private string _key;

        #endregion

        #region Atributo Público

        /// <summary>
        /// Obtiene la instancia única de la clase
        /// </summary>
        public static Geocoding Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Realiza la petición de geocodificación inversa devolviendo el resultado en formato XML
        /// </summary>
        /// <param name="lat">Latitud</param>
        /// <param name="lng">Longitud</param>
        /// <param name="respuesta_predeterminada">En caso de existir algún incidente en la obtención de la geocodificación inversa, se utilizará este valor como retorno predeterminado</param>
        /// <returns></returns>
        public RetornoOperacion ObtenerGeocofificacionInversa(double lat, double lng, string respuesta_predeterminada)
        {
            //Devolviendo respuesta
            return ObtenerGeocofificacionInversa(lat, lng, TipoUbicacion.DEF, TipoResultado.def, IdiomaRespuesta.ES,respuesta_predeterminada);
        }

        /// <summary>
        /// Realiza la petición de geocodificación inversa devolviendo el resultado en formato XML
        /// </summary>
        /// <param name="lat">Latitud</param>
        /// <param name="lng">Longitud</param>
        /// <param name="tipo_ubicacion">Tipo de Ubicación a buscar</param>
        /// <param name="tipo_resultado">Tipo de resultado deseado</param>
        /// <param name="idioma_respuesta">Idioma de la respuesta esperada</param>
        /// <param name="respuesta_predeterminada">En caso de existir algún incidente en la obtención de la geocodificación inversa, se utilizará este valor como retorno predeterminado</param>
        /// <returns></returns>
        public RetornoOperacion ObtenerGeocofificacionInversa(double lat, double lng, TipoUbicacion tipo_ubicacion, TipoResultado tipo_resultado, IdiomaRespuesta idioma_respuesta, string respuesta_predeterminada)
        {
            //Filtros de búsqueda
            string tipoResultado = tipo_resultado.Equals(TipoResultado.def) ? "" : "&result_type=" + tipo_resultado.ToString().ToLower();
            string tipoUbicacion = tipo_ubicacion.Equals(TipoUbicacion.DEF) ? "" : "&location_type=" + tipo_ubicacion.ToString().ToUpper();
            
            //Declarando objeto de retorno y generando consumo de API
            RetornoOperacion resultado = HTTPWeb.CrearPeticionYRespuesta(string.Format("{0}/xml?latlng={1},{2}&key={3}&language={4}{5}{6}", this._url, lat.ToString(), lng.ToString(), this._key, idioma_respuesta.ToString().ToLower(), tipoResultado, tipoUbicacion), HTTPWeb.Metodo.POST);

            //Si no hay errores en petición
            if (resultado.OperacionExitosa)
            {
                try
                {
                    //Convirtiendo respuesta a XML
                    XDocument doc = XDocument.Parse(resultado.Mensaje);

                    if (doc.Root.Element("status").Value.Equals("OK"))
                    {
                        //Validando contenido de respuesta
                        string res = doc.Root.Element("result").Element("formatted_address").Value;
                        resultado = new RetornoOperacion(res, true);
                    }          
                    else
                        resultado = new RetornoOperacion(respuesta_predeterminada, false);
                }
                catch (Exception)
                {
                    resultado = new RetornoOperacion(respuesta_predeterminada, false);
                }
            }

            //Devolviendo respuesta
            return resultado;
        }

        #endregion
    }
}
