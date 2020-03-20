using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SAT.Maps
{
    public partial class UbicacionMapa : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Producido al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Recuperando parámetros enviados(id de ubicación)
            int id_ubicacion = Convert.ToInt32(Request.QueryString["id_ubicacion"] != null ? Request.QueryString["id_ubicacion"] : "0");
            //Instanciando ubicación
            using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(id_ubicacion))
            { 
                //Si la ubicación existe
                if (u.id_ubicacion > 0)
                { 
                    //Creando script de mapa
                    construyeScriptMapa("ROAD", 22, u);
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Construye el script de inicialización y carga del mapa
        /// </summary>
        /// <param name="tipo_mapa">Tipo de Mapa solicitado (ROAD, SATELLITE, HYBRID, TERRAIN)</param>
        /// <param name="zoom_inicial"></param>
        /// <param name="ubicacion"></param>
        private void construyeScriptMapa(string tipo_mapa, byte zoom_inicial, SAT_CL.Global.Ubicacion ubicacion)
        {
            //Script que determinará el tipo de superficie a representar (punto o poligono)
            string superficie = "";
            //Puntos a trazar
            List<PointF> puntos = ubicacion.RecuperaPuntosUbicacion();
            //Centro del mapa (punto medio de la lista de puntos)
            PointF centro_mapa;
                        
            //Si existe más de un punto a mostrar
            if (puntos.Count > 1)
            {
                //Obteniendo el centro de la ubicación
                centro_mapa = new PointF((float)ubicacion.geoubicacion.EnvelopeCenter().Lat.Value, (float)ubicacion.geoubicacion.EnvelopeCenter().Long.Value);

                //Color de la superficie
                Random r = new Random();
                string color = String.Format("#{0:X6}", r.Next(0x1000000));

                //La superficie será un poligono
                superficie =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficie = [ " + string.Join(",", (from PointF p in puntos
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
                centro_mapa = puntos.DefaultIfEmpty().First();
                //La superficie será un marcador
                superficie =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficie = " +string.Format("new google.maps.LatLng({0}, {1})", centro_mapa.X, centro_mapa.Y) +
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
            var contentString = '<div><b>" + ubicacion.descripcion + @"</b>' +
                '<br>" + ubicacion.ObtieneDireccionCompleta() + @"</div>';

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

        #endregion
    }
}