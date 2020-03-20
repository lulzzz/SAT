using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;

namespace SAT.UserControls
{
    public partial class wucMapaUbicacion : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Método encargado de Obtener la Clave de la API de Maps
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string _maps_api() { return ConfigurationManager.AppSettings["MapsEmbedAPI"]; }

        #endregion

        #region Estructuras

        /// <summary>
        /// Estructura de Composición de Ubicación Maps
        /// </summary>
        public struct Maps
        {
            public string tipo;
            public DateTime fecha;
            public string descripcion;
            public PointF punto;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Construye una lista con los puntos que contiene su dato geográfico
        /// </summary>
        /// <returns></returns>
        private List<PointF> recuperaPuntosUbicacion(SqlGeography geoubicacion)
        {
            //Definiendo objeto principal para listar los puntos (lat, long) que conforman el poligono
            List<PointF> puntos = new List<PointF>();
            //Si la ubicación geográfica está definida
            if (!geoubicacion.IsNull)
            {
                //Determinando el tipo de geometría que representa el valor geográfico
                switch (geoubicacion.STGeometryType().Value)
                {
                    case "Point":
                        puntos.Add(new PointF((float)geoubicacion.Lat.Value, (float)geoubicacion.Long.Value));
                        break;
                    case "LineString":
                    case "CompoundCurve":
                    case "Polygon":
                    case "CurvePolygon":
                        //Determinando cuantos puntos posee el valor geográfico
                        int total_puntos = geoubicacion.STNumPoints().Value;
                        //Para cada uno de los puntos existentes
                        for (int x = 1; x < total_puntos; x++)
                        {
                            //Añadiendo el punto a la lista
                            puntos.Add(new PointF((float)geoubicacion.STPointN(x).Lat.Value, (float)geoubicacion.STPointN(x).Long.Value));
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

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Mapa con una Ubicación
        /// </summary>
        /// <param name="geoubicacion">Ubicación Geografica </param>
        public void InicializaMapaUbicacion(string tipo_mapa, byte zoom_inicial, SqlGeography geoubicacion, string descripcion)
        {
            //Declarando Variable Auxiliares
            string superficie = "";
            List<PointF> puntos_ubicacion = recuperaPuntosUbicacion(geoubicacion);
            PointF centro_mapa;
            string direccion = geoubicacion.Lat.ToString() + ", " + geoubicacion.Long.ToString();

            //Si existe más de un punto a mostrar
            if (puntos_ubicacion.Count > 1)
            {
                //Obteniendo el centro de la ubicación
                centro_mapa = new PointF((float)geoubicacion.EnvelopeCenter().Lat.Value, (float)geoubicacion.EnvelopeCenter().Long.Value);

                //Color de la superficie
                Random r = new Random();
                string color = String.Format("#{0:X6}", r.Next(0x1000000));

                //La superficie será un poligono
                superficie =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficie = [ " + string.Join(",", (from PointF p in puntos_ubicacion
                                                                    select string.Format("new google.maps.LatLng({0}, {1})", p.X, p.Y))) + @"];

                // Creando superficie, con los puntos señalados y sobre el mapa ya definido
                superficie = new  google.maps.Polygon({
                    paths: coordenadasSuperficie,
                    map:map,
                    strokeColor: '" + color + @"',
                    strokeOpacity: 0.7,
                    strokeWeight: 3,
                    fillColor: '" + color + @"',
                    fillOpacity: 0.3
                });";
            }
            //Si es sólo un punto
            else
            {
                //El centro es el punto actual
                centro_mapa = puntos_ubicacion.DefaultIfEmpty().First();
                //La superficie será un marcador
                superficie =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficie = " + string.Format("new google.maps.LatLng({0}, {1})", centro_mapa.X, centro_mapa.Y) +
                @"
                // Creando superficie
                superficie = new  google.maps.Marker({
                position:coordenadasSuperficie,   
                map:map,             
                draggable:false
                });";
            }

            //Script principal del mapa
            string script = @"<script>
                                var map;
                                var informacion;
                                var superficie;

                                //Función de inicialización de contenido de mapa
                                function inicializaMapa() {

                                    //Configurando opciones generales de mapa
                                    var opcionesMapa = {
                                        zoom: " + zoom_inicial.ToString() + @",
                                        //Coordenada central del poligono
                                        center: new google.maps.LatLng(" + centro_mapa.X.ToString() + ", " + centro_mapa.Y.ToString() + @"),
                                        mapTypeId: google.maps.MapTypeId." + tipo_mapa + @"
                                    };
                                    //Aplicando configuración de mapa
                                    map = new google.maps.Map(document.getElementById('mapa'), opcionesMapa); "
                                    + superficie +
                                    @" //Agregando manejador de evento click dentro de la superficie mostrada.
                                    google.maps.event.addListener(superficie, 'click', mostrarInfo);

                                    informacion = new google.maps.InfoWindow();
                                }

                                /** @this {google.maps.Polygon/google.maps.Marker} */
                                function mostrarInfo(event) {
                                    //Mensaje de contenido
                                    var contentString = '<div><b>" + descripcion + @"</b>' +
                                        '<br>" + direccion + @"</div>';

                                    //Añadiendo información a mostrar
                                    informacion.setContent(contentString);
                                    informacion.setPosition(event.latLng);

                                    //Mostrando información
                                    informacion.open(map);
                                }

                                //Agregando manejador de evento carga de página
                                google.maps.event.addDomListener(window, 'load', inicializaMapa);

                            </script>";

            //Añadiendo script a la literal de representación en la forma
            ltr.Text = script;
        }
        /// <summary>
        /// Método encargado de Inicializar el Mapa con los Puntos especificos
        /// </summary>
        /// <param name="tipo_mapa">Tipo de Mapa a mostrar</param>
        /// <param name="zoom_inicial">Acercamiento Inicial del Mapa</param>
        /// <param name="puntos_descripcion">Estructura de Datos de las Ubicaciones</param>
        public void InicializaMapaPuntos(string tipo_mapa, byte zoom_inicial, List<Maps> puntos_descripcion)
        {
            //Declarando Variable Auxiliares
            string superficie = "";

            //Si existe más de un punto a mostrar
            if (puntos_descripcion.Count > 1)
            {
                //La superficie será un poligono
                superficie =
                @"//Declarando variable de marcadores de superficie a representar
                var markers = [ " + string.Join(",", (from Maps punto_desc in puntos_descripcion
                                                      select string.Format("['{0}, {1}', {2}, {3}]", 
                                                      punto_desc.tipo, punto_desc.descripcion, punto_desc.punto.X, punto_desc.punto.Y))) + @"];

                // Info Window Content
                var infoWindowContent = [" + string.Join(",", (from Maps punto_desc in puntos_descripcion
                                                               select string.Format("['<h3>{0}</h3>'+'<p>{1}</p>'+'<p>{2:dd/MM/yyyy HH:mm}</p>'+'<p>{3},{4}</p>']",
                                                               punto_desc.tipo, punto_desc.fecha, punto_desc.descripcion, punto_desc.punto.X, punto_desc.punto.Y))) + @"];";
            }

            //Script principal del mapa
            string script = @"<script>
                                var map;
                                var bounds = new google.maps.LatLngBounds();
                                var informacion;
                                var superficie;

                                //Función de inicialización de contenido de mapa
                                function inicializaMapa() {

                                    //Configurando opciones generales de mapa
                                    var opcionesMapa = {
                                        zoom: " + zoom_inicial.ToString() + @",
                                        //Coordenada central de los Puntos
                                        mapTypeId: google.maps.MapTypeId." + tipo_mapa + @"
                                    };

                                    //Aplicando configuración de mapa
                                    map = new google.maps.Map(document.getElementById('mapa'), opcionesMapa);
                                    
                                    " + superficie + @"
                                    
                                    // Display multiple markers on a map
                                    var infoWindow = new google.maps.InfoWindow(), marker, i;
    
                                    // Loop through our array of markers & place each one on the map  
                                    for( i = 0; i < markers.length; i++ ) {
                                        var position = new google.maps.LatLng(markers[i][1], markers[i][2]);
                                        bounds.extend(position);
                                        marker = new google.maps.Marker({
                                            position: position,
                                            map: map,
                                            title: markers[i][0]
                                        });
        
                                        // Allow each marker to have an info window    
                                        google.maps.event.addListener(marker, 'click', (function(marker, i) {
                                            return function() {
                                                infoWindow.setContent(infoWindowContent[i][0]);
                                                infoWindow.open(map, marker);
                                            }
                                        })(marker, i));

                                        // Automatically center the map fitting all markers on the screen
                                        map.fitBounds(bounds);
                                    }

                                    // Override our map zoom level once our fitBounds function runs (Make sure it only runs once)
                                    var boundsListener = google.maps.event.addListener((map), 'bounds_changed', function(event) {
                                        this.setZoom(14);
                                        google.maps.event.removeListener(boundsListener);
                                    });
                                }

                                //Agregando manejador de evento carga de página
                                google.maps.event.addDomListener(window, 'load', inicializaMapa);
                            </script>";

            //Añadiendo script a la literal de representación en la forma
            ltr.Text = script;
        }

        #endregion
    }
}