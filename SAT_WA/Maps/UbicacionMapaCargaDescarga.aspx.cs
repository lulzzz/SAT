using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SAT.Maps
{
    public partial class UbicacionMapaCargaDescarga : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Producido al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Recuperando parámetros enviados(id de ubicación carga, id ubicacion descarga)
            int id_ubicacion_carga = Convert.ToInt32(Request.QueryString["id_ubicacion_carga"] != null ? Request.QueryString["id_ubicacion_carga"] : "0");
            int id_ubicacion_descarga = Convert.ToInt32(Request.QueryString["id_ubicacion_descarga"] != null ? Request.QueryString["id_ubicacion_descarga"] : "0");
            //Instanciando ubicación
            using (SAT_CL.Global.Ubicacion uc = new SAT_CL.Global.Ubicacion(id_ubicacion_carga), ud = new SAT_CL.Global.Ubicacion(id_ubicacion_descarga))
            {
                //Si la ubicación existe
                if (uc.id_ubicacion > 0 && ud.id_ubicacion >0 )
                {
                    //Creando script de mapa
                    construyeScriptMapa("ROAD", 17, uc, ud);
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
        /// <param name="ubicacion_carga"></param>
        /// <param name="ubicacion_descarga">
        private void construyeScriptMapa(string tipo_mapa, byte zoom_inicial, SAT_CL.Global.Ubicacion ubicacion_carga, SAT_CL.Global.Ubicacion ubicacion_descarga)
        {
            //Script que determinará el tipo de superficie a representar (punto o poligono)
            string superficieCarga = "";
            string superficieDescarga = "";
            //Puntos a trazar
            List<PointF> puntos_carga = ubicacion_carga.RecuperaPuntosUbicacion();
            List<PointF> puntos_descarga = ubicacion_descarga.RecuperaPuntosUbicacion();
            //Centro del mapa (punto medio de la lista de puntos)
            PointF centro_mapa, centro_mapa_carga, centro_mapa_descarga;

            // Obtenemos Superficie para la Ubicación de Carga
            //Si existe más de un punto a mostrar
            if (puntos_carga.Count > 1)
            {
                //Obteniendo el centro de la ubicación
                centro_mapa_carga = new PointF((float)ubicacion_carga.geoubicacion.EnvelopeCenter().Lat.Value, (float)ubicacion_carga.geoubicacion.EnvelopeCenter().Long.Value);
                centro_mapa_descarga = new PointF((float)ubicacion_descarga.geoubicacion.EnvelopeCenter().Lat.Value, (float)ubicacion_descarga.geoubicacion.EnvelopeCenter().Long.Value);
                //Obtenemos el Promedio para establecer el centro del  Mapa
                float centerX = (centro_mapa_carga.X + centro_mapa_descarga.X)/2;
                float centerY = (centro_mapa_carga.Y + centro_mapa_descarga.Y)/2;
                //Establecemos Centro del Mapa
                centro_mapa = new PointF(centerX, centerY);

                //Color de la superficie
                Random r = new Random();
                string color = String.Format("#{0:X6}", r.Next(0x1000000));

                //La superficie será un poligono
                superficieCarga =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficieCarga = [ " + string.Join(",", (from PointF p in puntos_carga
                                                                    select string.Format("new google.maps.LatLng({0}, {1})", p.X, p.Y))) + @"];

                // Creando superficie, con los puntos señalados y sobre el mapa ya definido
                superficieCarga = new  google.maps.Polygon({
                    paths: coordenadasSuperficieCarga,
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
                centro_mapa_carga = puntos_carga.DefaultIfEmpty().First();
                centro_mapa_descarga = puntos_descarga.DefaultIfEmpty().First();
                //Obtenemos el Promedio para establecer el centro del  Mapa
                float centerX = (centro_mapa_carga.X + centro_mapa_descarga.X) / 2;
                float centerY = (centro_mapa_carga.Y + centro_mapa_descarga.Y) / 2;
                //Establecemos Centro del Mapa
                centro_mapa = new PointF(centerX, centerY);

                //La superficie será un marcador
                superficieCarga =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficieCarga = " + string.Format("new google.maps.LatLng({0}, {1})", centro_mapa_carga.X, centro_mapa_carga.Y) +
                @"
                // Creando superficie
                superficieCarga = new  google.maps.Marker({
                position:coordenadasSuperficieCarga,   
                map:map,             
                draggable:false
                });";
            }

            //Obtenemos Superficie para la ubicación de Descarga
            //Si existe más de un punto a mostrar
            if (puntos_descarga.Count > 1)
            {

                //Color de la superficie
                Random r = new Random();
                string color = String.Format("#{0:X6}", r.Next(0x1000000));

                //La superficie será un poligono
                superficieDescarga =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficieDescarga = [ " + string.Join(",", (from PointF p in puntos_descarga
                                                                         select string.Format("new google.maps.LatLng({0}, {1})", p.X, p.Y))) + @"];

                // Creando superficie, con los puntos señalados y sobre el mapa ya definido
                superficieDescarga = new  google.maps.Polygon({
                    paths: coordenadasSuperficieDescarga,
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
                //La superficie será un marcador
                superficieDescarga =
                @"//Declarando variable de coordenadas de superficie a representar           
                var coordenadasSuperficieDescarga = " + string.Format("new google.maps.LatLng({0}, {1})", centro_mapa_descarga.X, centro_mapa_descarga.Y) +
                @"
                // Creando superficie
                superficieDescarga = new  google.maps.Marker({
                position:coordenadasSuperficieDescarga,   
                map:map,             
                draggable:false
                });";
            }

            //Script principal del mapa
            string script = @"<script>
            var map;
            var informacionCarga;
            var superficieCarga;
            var informacionDescarga;
            var superficieDescarga;


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
            + superficieCarga + superficieDescarga +
            @" //Agregando manejador de evento click dentro de la superficie mostrada.
            google.maps.event.addListener(superficieCarga, 'click', mostrarInfoCarga);
            google.maps.event.addListener(superficieDescarga, 'click', mostrarInfoDescarga);

            informacionCarga = new google.maps.InfoWindow();
            informacionDescarga = new google.maps.InfoWindow();
        }

        /** @this {google.maps.Polygon/google.maps.Marker} (Ubicación Carga) */
        function mostrarInfoCarga(event) {
            //Mensaje de contenido
            var contentStringCarga = '<div><b>" + ubicacion_carga.descripcion + @"</b>' +
                '<br>" + ubicacion_carga.ObtieneDireccionCompleta() + @"</div>';

            //Añadiendo información a mostrar
            informacionCarga.setContent(contentStringCarga);
            informacionCarga.setPosition(event.latLng);

            //Mostrando información
            informacionCarga.open(map);
        }
        
          /** @this {google.maps.Polygon/google.maps.Marker} (Ubicación Carga) */
        function mostrarInfoDescarga(event) {
            //Mensaje de contenido
            var contentStringDescarga = '<div><b>" + ubicacion_descarga.descripcion + @"</b>' +
                '<br>" + ubicacion_descarga.ObtieneDireccionCompleta() + @"</div>';

            //Añadiendo información a mostrar
            informacionDescarga.setContent(contentStringDescarga);
            informacionDescarga.setPosition(event.latLng);

            //Mostrando información
            informacionDescarga.open(map);
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