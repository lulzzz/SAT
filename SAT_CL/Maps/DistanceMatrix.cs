using Microsoft.SqlServer.Types;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Xml.Linq;
using TSDK.Base;

namespace SAT_CL.Maps
{
    public sealed class DistanceMatrix
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración de Tipo de Petición
        /// </summary>
        public enum TipoPeticion
        {
            /// <summary>
            /// Expresa la Petición en Formato JavaScript Object Notation
            /// </summary>
            JSON = 1,
            /// <summary>
            /// Expresa la Petición en Formato XML
            /// </summary>
            XML = 2
        }        
        /// <summary>
        /// Enumeración de Unidades de Medida de la Solicitud
        /// </summary>
        public enum Unidades
        {
            /// <summary>
            /// Expresa las Unidades Solicitadas en Metros/Kilometros
            /// </summary>
            Metric = 1,
            /// <summary>
            /// Expresa las Unidades Solicitadas en Millas
            /// </summary>
            Imperial = 2
        }
        /// <summary>
        /// Enumeración que Define los Estatus de Respuesta de la Petición
        /// </summary>
        public enum EstatusRespuestaPeticion
        {
            /// <summary>
            /// Respuesta Correcta
            /// </summary>
            OK = 1,
            /// <summary>
            /// Petición Invalida
            /// </summary>
            INVALID_REQUEST = 2,
            /// <summary>
            /// Error Desconocido
            /// </summary>
            UNKNOWN_ERROR = 3,
            /// <summary>
            /// Maximo de Elementos Excedidos
            /// </summary>
            MAX_ELEMENTS_EXCEEDED = 4,
            /// <summary>
            /// Limite de Solicitudes Excedido
            /// </summary>
            OVER_QUERY_LIMIT = 5,
            /// <summary>
            /// Petición Denegada
            /// </summary>
            REQUEST_DENIED = 6
        }
        /// <summary>
        /// 
        /// </summary>
        public enum EstatusRespuestaElemento
        {
            /// <summary>
            /// Respuesta Correcta
            /// </summary>
            OK = 1,
            /// <summary>
            /// No Encontrado
            /// </summary>
            NOT_FOUND = 2,
            /// <summary>
            /// No hay Resultados
            /// </summary>
            ZERO_RESULTS = 3
        }

        #endregion

        #region Atributos Privados

        /// <summary>
        /// Atributo que almacena el Objeto Instanciado por la Misma Clase
        /// </summary>
        private static DistanceMatrix _distanceMatrix;
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
        public static DistanceMatrix objDistanceMatrix
        {
            get
            {
                //Validando que no Exista la Instancia
                if (_distanceMatrix == null)
                {
                    lock (new object())
                    {
                        //Si no existe
                        if (_distanceMatrix == null)

                            //Creando una Nueva Instancia
                            _distanceMatrix = new DistanceMatrix();
                    }
                }

                //Devolviendo Resultado Obtenido
                return _distanceMatrix;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        private DistanceMatrix()
        {
            try
            {
                //Asignando Valores de Conexión
                this._url = ConfigurationManager.AppSettings["DistanceMatrixAPI_URL"];
                this._key = ConfigurationManager.AppSettings["DistanceMatrixAPI_SAT_DriverWebKey"];
            }
            catch (Exception) { }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url_parametros"></param>
        /// <param name="tipoPeticion"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneRespuestaPeticion(string url_parametros, TipoPeticion tipoPeticion)
        {
            //Declarando Objeto de Validación
            RetornoOperacion result = new RetornoOperacion();
            
            //Creando Petición Web
            this._peticionWeb = (HttpWebRequest)WebRequest.Create(url_parametros);

            //Configurando Petición Web
            this._peticionWeb.ContentType = "application/" + traduceTipoPeticion(tipoPeticion);
            this._peticionWeb.Method = "POST";

            try
            {
                //Inicializando Petición
                this._peticionWeb.GetRequestStream();

                //Obteniendo Respuesta de la Petición
                this._respuestaWeb = (HttpWebResponse)this._peticionWeb.GetResponse();

                //Leyendo Respuesta
                using (StreamReader streamReader = new StreamReader(this._respuestaWeb.GetResponseStream()))
                {
                    //Escribiendo Respuesta
                    result = new RetornoOperacion(streamReader.ReadToEnd(), true);
                }
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
        /// Método que Traduce los Tipos de Petición
        /// </summary>
        /// <param name="tipoPeticion">Enumeración de Tipo de Petición</param>
        /// <returns></returns>
        private string traduceTipoPeticion(TipoPeticion tipoPeticion)
        {
            //Declarando Objeto de Retorno
            string tipo = "";

            //Evaluando Tipo de Petición
            switch (tipoPeticion)
            {
                case TipoPeticion.XML:
                    tipo = "xml";
                    break;
                case TipoPeticion.JSON:
                    tipo = "json";
                    break;
            }

            //Devolviendo Resultado Obtenido
            return tipo;
        }
        /// <summary>
        /// Método encargado de Traducir las Unidades
        /// </summary>
        /// <param name="unidades">Enumeración de Tipos de Unidad</param>
        /// <returns></returns>
        private string traduceUnidades(Unidades unidades)
        {
            //Declarando Objeto de Retorno
            string tipo = "";

            //Evaluando Tipo de Petición
            switch (unidades)
            {
                case Unidades.Metric:
                    tipo = "metric";
                    break;
                case Unidades.Imperial:
                    tipo = "imperial";
                    break;
            }

            //Devolviendo Resultado Obtenido
            return tipo;
        }
        /// <summary>
        /// Método encargado de Personalizar las Excepciónes de la Petición
        /// </summary>
        /// <param name="estatus">Estatus de la Petición</param>
        /// <returns></returns>
        private RetornoOperacion defineExcepcionPeticion(string estatus)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus
            switch (estatus)
            {
                case "OK":
                    //Personalizando Excepción
                    result = new RetornoOperacion("Operación Exitosa", true);
                    break;
                case "INVALID_REQUEST":
                    //Personalizando Excepción
                    result = new RetornoOperacion("Petición Invalida", false);
                    break;
                case "MAX_ELEMENTS_EXCEEDED":
                    //Personalizando Excepción
                    result = new RetornoOperacion("Maximo de Elementos Excedidos", false);
                    break;
                case "OVER_QUERY_LIMIT":
                    //Personalizando Excepción
                    result = new RetornoOperacion("Limite de Solicitudes Excedido", false);
                    break;
                case "REQUEST_DENIED":
                    //Personalizando Excepción
                    result = new RetornoOperacion("Petición Denegada", false);
                    break;
                case "UNKNOWN_ERROR":
                default:
                    //Personalizando Excepción
                    result = new RetornoOperacion("Error Desconocido", false);
                    break;
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Personalizar las Excepciónes del Elemento
        /// </summary>
        /// <param name="estatus">Estatus del Elemento</param>
        /// <returns></returns>
        private RetornoOperacion defineExcepcionElemento(string estatus)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus
            switch (estatus)
            {
                case "OK":
                    //Personalizando Excepción
                    result = new RetornoOperacion("Operación Exitosa", true);
                    break;
                case "NOT_FOUND":
                    //Personalizando Excepción
                    result = new RetornoOperacion("Alguna de las Ubicaciones no fueron encontradas", false);
                    break;
                case "ZERO_RESULTS":
                    //Personalizando Excepción
                    result = new RetornoOperacion("No hubo resultados", false);
                    break;
                default:
                    //Personalizando Excepción
                    result = new RetornoOperacion("Error Desconocido", false);
                    break;
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos XML

        /// <summary>
        /// Método encargado de Obtener la Distancia y el Tiempo dado un Origen y un Destino
        /// </summary>
        /// <param name="origen">Origen</param>
        /// <param name="destino">Destino</param>
        /// <param name="unidades">Unidad de Retorno</param>
        /// <returns></returns>
        public RetornoOperacion ObtieneDistanciaOrigenDestinoXML(SqlGeography origen, SqlGeography destino, Unidades unidades, out XDocument distanciaMatrix)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            distanciaMatrix = new XDocument();

            //Declarando Variables Axuliares
            SqlGeography ori = SqlGeography.Null, dest = SqlGeography.Null;

            //Validando Origen
            if (origen != SqlGeography.Null)
            {
                //Validando Origen
                if (destino != SqlGeography.Null)
                {
                    //Validando que el Origen sea de Tipo "Point"
                    if (origen.STGeometryType().Value.Equals("Point"))
                    
                        //Asignando Valores
                        ori = origen;
                    else
                        //Obteniendo Puntos Centrales
                        ori = origen.EnvelopeCenter();

                    //Validando que el Destino sea de Tipo "Point"
                    if (destino.STGeometryType().Value.Equals("Point"))
                    
                        //Asignando Valores
                        dest = destino;
                    else
                        //Obteniendo Puntos Centrales
                        dest = destino.EnvelopeCenter();
                    

                    //Construyendo URL con Parametros
                    string url_final = string.Format("{0}{1}?units={2}&origins={3},{4}&destinations={5},{6}&key={7}",
                                                        this._url, traduceTipoPeticion(TipoPeticion.XML), traduceUnidades(unidades),
                                                        ori.Lat, ori.Long, dest.Lat, dest.Long, this._key);

                    //Ejecutando Petición Web
                    result = obtieneRespuestaPeticion(url_final, TipoPeticion.XML);
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe el Destino");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No existe el Origen");

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                try
                {
                    //Convirtiendo Resultado en XML
                    distanciaMatrix = XDocument.Parse(result.Mensaje);
                }
                catch (Exception e)
                {
                    //Instanciando Excepción
                    result = new RetornoOperacion(e.Message);
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener la Distancia y la Duración de un Origen y un Destino
        /// </summary>
        /// <param name="origen">Origen</param>
        /// <param name="destino">Destino</param>
        /// <param name="unidades">Unidad de Retorno</param>
        /// <param name="distancia">Distancia en Metros (Parametro Output)</param>
        /// <param name="duracion">Duración en Minutos (Parametro Output)</param>
        public RetornoOperacion ObtieneDistanciaOrigenDestinoXML(SqlGeography origen, SqlGeography destino, Unidades unidades, 
                                                                 out int distancia, out int duracion)
        {
            //Declarando Variables Auxiliares
            RetornoOperacion result = new RetornoOperacion();
            XDocument distanciaMatrix = new XDocument();
            
            //Inicializando Valores
            distancia = duracion = 0;

            //Obteniendo Distancia|Duración
            result = ObtieneDistanciaOrigenDestinoXML(origen, destino, unidades, out distanciaMatrix);

            //Validando si existe 
            if (result.OperacionExitosa)
            {
                try
                {
                    //Obteniendo Estatus de la Petición
                    result = defineExcepcionPeticion(distanciaMatrix.Root.Element("status").Value);
                    
                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Elemento
                        XElement elemento = distanciaMatrix.Root.Element("row").Element("element");

                        //Validando que exista el Elemento
                        if (elemento != null)
                        {
                            //Validando Estatus del Elemento
                            result = defineExcepcionElemento(elemento.Element("status").Value);

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Valor de Distancia
                                distancia = Convert.ToInt32(elemento.Element("distance").Element("value").Value);
                                //Obteniendo Valor de Duración
                                duracion = Convert.ToInt32(elemento.Element("duration").Element("value").Value);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    //Instanciando Excepción
                    result = new RetornoOperacion(e.Message);
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
