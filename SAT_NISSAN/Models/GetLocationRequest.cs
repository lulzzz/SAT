using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using RestSharp;
using TSDK.Base;
using Newtonsoft.Json.Linq;
using System.Text;

namespace SAT_NISSAN.Models
{
    public class GetLocationRequest
    {
        /// <summary>
        /// Parámetro opcional, este parámetro solo aplica cuando se desea que se realice un envío masivo de localización entre un rango de fechas, sin no se desea se enviara un cadena vacía
        /// </summary>
        public string StarDate { get; set; }
        /// <summary>
        /// Parámetro opcional, este parámetro solo aplica cuando se desea que se realice un envío masivo de localización entre un rango de fechas, sin no se desea se enviara un cadena vacía
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// Parámetro opcional que contiene valores de tipo numérico, donde se indica el total de minutos de la frecuencia con la que solicitan los datos cuando se han ingresado información en los parámetros starDate y EndDate
        /// </summary>
        public string Frecuency { get; set; }
        /// <summary>
        /// Parametro de Entrada que expresa el Folio Único de NISSAN
        /// </summary>
        public string[] TravelFolio { get; set; }
    }

    public class GetLocationResponse
    {
        public string FolioViaje { get; set; }
        public int IdPuntoEvento { get; set; }
        public string AETC { get; set; }
        public string NumeroContenedor1 { get; set; }
        public string NumeroContenedor2 { get; set; }
        public int NoConsecutivoContenedor { get; set; }
        public string CartaPorte { get; set; }
        public string FechaHoraInicioReal { get; set; }
        public string FechaHoraFinReal { get; set; }
        public string FechaHoraUltReporte { get; set; }
        public string UltUbicacionTransporte { get; set; }
        public string Tracto { get; set; }
        public string PlacasTracto { get; set; }
        public string NombreChofer { get; set; }
        public string ETA { get; set; }
        public double LatitudTracto { get; set; }
        public double LongitudTracto { get; set; }
        public double LatitudContenedor1 { get; set; }
        public double LongitudContenedor1 { get; set; }
        public double LatitudContenedor2 { get; set; }
        public double LongitudContenedor2 { get; set; }
        public bool EncendidoMotor { get; set; }
        public string StatusRuta { get; set; }
    }

    public class GetLocationMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gps"></param>
        /// <param name="clienteGps"></param>
        /// <param name="token_auth"></param>
        /// <param name="tipo_auth"></param>
        /// <returns></returns>
        public static RetornoOperacion ObtieneAccesoProveedorGPS(
            Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>> gps, 
            out RestClient clienteGps, out string token_auth, out string tipo_auth)
        {
            RetornoOperacion retorno = new RetornoOperacion(1, "Datos Obtenidos Exitosamente", true);
            //Obteniendo URL de Autenticación(TOKEN)
            token_auth = tipo_auth = "";
            string url_token = @"" + gps.Item2 + "/oauth/token";
            string url_gps = @"" + gps.Item2 + gps.Item3;
            RestClient clienteToken = new RestClient(url_token);
                       clienteGps = new RestClient(url_gps);


            //Configurando Datos del Cliente y de la Petición
            RestRequest peticionToken = new RestRequest(Method.POST);
            clienteToken.Timeout = -1;
            peticionToken.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(gps.Item4.Item1 + ":" + gps.Item4.Item2)));
            peticionToken.AddHeader("Content-Type", "multipart/form-data");
            peticionToken.AlwaysMultipartFormData = true;
            //Añadiendo Parametros
            peticionToken.AddParameter("grant_type", gps.Item5.Item3);
            peticionToken.AddParameter("username", gps.Item5.Item1);
            peticionToken.AddParameter("password", gps.Item5.Item2);

            //Consumiendo Petición
            IRestResponse respuestaToken = clienteToken.Execute(peticionToken);
            if (respuestaToken.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject json_token = JObject.Parse(respuestaToken.Content);
                if (json_token != null)
                {
                    //Obteniendo Token de Autorización
                    token_auth = (string)json_token["access_token"];
                    tipo_auth = (string)json_token["token_type"];
                    clienteGps.Timeout = -1;

                    //Asignando Retorno Positivo
                    retorno = new RetornoOperacion(1, "Token Obtenido Exitosamente!", true);
                }
                else
                    retorno = new RetornoOperacion("No se puede acceder al Servicio GPS");
            }
            else
            {
                if (Convert.ToInt32(respuestaToken.StatusCode) == 0)
                    retorno = new RetornoOperacion(string.Format("{0} - {1}", respuestaToken.StatusCode, respuestaToken.StatusDescription));
                else
                    retorno = new RetornoOperacion(string.Format("{0} - {1}", respuestaToken.StatusCode, respuestaToken.ErrorMessage));
            }

            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gps"></param>
        /// <param name="clienteGps"></param>
        /// <param name="token_auth"></param>
        /// <param name="tipo_auth"></param>
        /// <returns></returns>
        public static RestRequest ObtienePeticionGps(
            Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>> gps,
            RestClient clienteGps, string token_auth, string tipo_auth)
        {
            //Configurando Petición GPS
            RestRequest peticionGps = new RestRequest(Method.POST);
            peticionGps.AddHeader("Content-Type", "application/json");
            peticionGps.AddHeader("Authorization", tipo_auth + " " + token_auth);

            return peticionGps;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unidad"></param>
        /// <param name="peticionGps"></param>
        /// <param name="clienteGps"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static RetornoOperacion ObtienePosicionesUnidad(Tuple<int, string, string, string, int, string, string> unidad,
            RestRequest peticionGps, RestClient clienteGps, int frecuencia, DateTime inicio_serv, DateTime fin_servicio,
            out List<Tuple<double, double, string, DateTime, decimal, bool>> points)
        {
            RetornoOperacion retorno = new RetornoOperacion(1, "Datos Obtenidos Exitosamente", true);
            points = new List<Tuple<double, double, string, DateTime, decimal, bool>>();

            DateTime inicio, termino;
            DateTime.TryParse(unidad.Item6, out inicio);
            DateTime.TryParse(unidad.Item7, out termino);

            //Validando Fechas
            if (inicio != DateTime.MinValue && termino != DateTime.MinValue)
            {
                //Si pasa más de un día
                double dias = (termino.Date - inicio.Date).TotalDays, cont = 0;
                if (dias == 0)
                {
                    //Armando Parametros de Petición
                    string gps_params = @"{ 'fecha' : '" + inicio.ToString("yyyy-MM-dd") + "', 'idUnit' : '" + unidad.Item4 + "' }";
                    JObject json_params = JObject.Parse(gps_params);
                    peticionGps.AddParameter("application/json", json_params.ToString(Newtonsoft.Json.Formatting.None), ParameterType.RequestBody);

                    //Consumiendo Petición
                    IRestResponse respuestaGps = clienteGps.Execute(peticionGps);
                    if (respuestaGps.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string posiciones_gps = @"{ 'posiciones' : " + respuestaGps.Content + " }";
                        JObject json_gps = JObject.Parse(posiciones_gps);
                        if (json_gps != null)
                        {
                            JArray psGps = (JArray)json_gps["posiciones"];

                            if (psGps != null)
                            {
                                //Declarando Variable que almacena los puntos para cualquier tipo de petición
                                List<Tuple<double, double, string, DateTime, decimal, bool>> pnts = new List<Tuple<double, double, string, DateTime, decimal, bool>>();

                                if (frecuencia > 0)
                                {
                                    if (inicio_serv != DateTime.MinValue)
                                    {
                                        if (fin_servicio != DateTime.MinValue)
                                        {
                                            double contador_incremento = (double)frecuencia;
                                            DateTime fecha_pivote = inicio_serv;

                                            //Iniciando Ciclo
                                            while (DateTime.Compare(fecha_pivote, fin_servicio) < 0 ||
                                                    DateTime.Compare(fecha_pivote, fin_servicio) == 0)
                                            {
                                                //Validando que la Fecha de inicio, este dentro del rango de las fechas de operacion de la Unidad
                                                if (fecha_pivote >= inicio)
                                                {
                                                    //Validando que la Fecha de inicio, este dentro del rango de las fechas de operacion de la Unidad
                                                    if (fecha_pivote <= termino)
                                                    {
                                                        //Obtener puntos especificos
                                                        List<Tuple<double, double, string, DateTime, decimal, bool>> pf =
                                                        (from JToken p in psGps
                                                         where p["idUnit"].ToString().Equals(unidad.Item4)
                                                         && (Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()).ToString("yyyy-MM-dd HH:mm").Equals(fecha_pivote.ToString("yyyy-MM-dd HH:mm")))
                                                         select new Tuple<double, double, string, DateTime, decimal, bool>
                                                             (Convert.ToDouble(p["lat"]),
                                                              Convert.ToDouble(p["lng"]),
                                                              p["address"].ToString(),
                                                              Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()),
                                                              Convert.ToDecimal(p["speed"]),
                                                              p["speed"].ToString().Equals("0.0") ? false : true)).ToList();
                                                        if (pf.Count > 0)
                                                        {
                                                            foreach (Tuple<double, double, string, DateTime, decimal, bool> p in pf)
                                                                pnts.Add(p);
                                                        }

                                                        //Incrementando Contador
                                                        fecha_pivote = fecha_pivote.AddMinutes(contador_incremento);
                                                    }
                                                    else
                                                        //Terminando Ciclo
                                                        break;
                                                }
                                                else
                                                    fecha_pivote = fecha_pivote.AddMinutes(contador_incremento);
                                            }

                                            //Validando Lista
                                            if (pnts.Count > 0)
                                            {
                                                foreach (Tuple<double, double, string, DateTime, decimal, bool> p in pnts)
                                                    points.Add(p);
                                            }
                                            else
                                                retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                                        }
                                        else
                                            retorno = new RetornoOperacion("No existe una fecha de fin valida del servicio para esta Unidad");
                                    }
                                    else
                                        retorno = new RetornoOperacion("No existe una fecha de inicio valida del servicio para esta Unidad");
                                    List<DateTime> periodo_x_frecuencia = new List<DateTime>();
                                    
                                }
                                else
                                {
                                    //Obteniendo Datos de los Elementos por el Rango de Fechas de las Unidades
                                    pnts = (from JToken p in psGps
                                            where p["idUnit"].ToString().Equals(unidad.Item4)
                                            && (Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()) >= inicio &&
                                               Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()) <= termino)
                                            select new Tuple<double, double, string, DateTime, decimal, bool>
                                                (Convert.ToDouble(p["lat"]),
                                                 Convert.ToDouble(p["lng"]),
                                                 p["address"].ToString(),
                                                 Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()),
                                                 Convert.ToDecimal(p["speed"]),
                                                 p["speed"].ToString().Equals("0.0") ? false : true)
                                            ).ToList();
                                    //Validando Lista
                                    if (pnts.Count > 0)
                                    {
                                        foreach (Tuple<double, double, string, DateTime, decimal, bool> p in pnts)
                                            points.Add(p);
                                    }
                                    else
                                        retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                                }
                            }
                            else
                                retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                        }
                        else
                            retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                    }
                    else
                        retorno = new RetornoOperacion(520, string.Format("{0} - {1}", respuestaGps.StatusCode, respuestaGps.StatusDescription), false);
                }
                else if (dias > 0)
                {
                    while (cont <= dias)
                    {
                        //Armando Parametros de Petición
                        string gps_params = @"{ 'fecha' : '" + inicio.AddDays(cont).ToString("yyyy-MM-dd") + "', 'idUnit' : '" + unidad.Item4 + "' }";
                        JObject json_params = JObject.Parse(gps_params);
                        peticionGps.AddParameter("application/json", json_params.ToString(Newtonsoft.Json.Formatting.None), ParameterType.RequestBody);

                        //Consumiendo Petición
                        IRestResponse respuestaGps = clienteGps.Execute(peticionGps);
                        if (respuestaGps.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string posiciones_gps = @"{ 'posiciones' : " + respuestaGps.Content + " }";
                            JObject json_gps = JObject.Parse(posiciones_gps);
                            if (json_gps != null)
                            {
                                JArray psGps = (JArray)json_gps["posiciones"];
                                if (psGps != null)
                                {
                                    //Declarando Variable que almacena los puntos para cualquier tipo de petición
                                    List<Tuple<double, double, string, DateTime, decimal, bool>> pnts = new List<Tuple<double, double, string, DateTime, decimal, bool>>();

                                    if (frecuencia > 0)
                                    {
                                        if (inicio_serv != DateTime.MinValue)
                                        {
                                            if (fin_servicio != DateTime.MinValue)
                                            {
                                                double contador_incremento = (double)frecuencia;
                                                DateTime fecha_pivote = inicio_serv;

                                                //Iniciando Ciclo
                                                while (DateTime.Compare(fecha_pivote, fin_servicio) < 0 ||
                                                        DateTime.Compare(fecha_pivote, fin_servicio) == 0)
                                                {
                                                    //Validando que la Fecha de inicio, este dentro del rango de las fechas de operacion de la Unidad
                                                    if (fecha_pivote >= inicio)
                                                    {
                                                        //Validando que la Fecha de inicio, este dentro del rango de las fechas de operacion de la Unidad
                                                        if (fecha_pivote <= termino)
                                                        {
                                                            //Obtener puntos especificos
                                                            List<Tuple<double, double, string, DateTime, decimal, bool>> pf =
                                                            (from JToken p in psGps
                                                             where p["idUnit"].ToString().Equals(unidad.Item4)
                                                             && (Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()).ToString("yyyy-MM-dd HH:mm").Equals(fecha_pivote.ToString("yyyy-MM-dd HH:mm")))
                                                             select new Tuple<double, double, string, DateTime, decimal, bool>
                                                                 (Convert.ToDouble(p["lat"]),
                                                                  Convert.ToDouble(p["lng"]),
                                                                  p["address"].ToString(),
                                                                  Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()),
                                                                  Convert.ToDecimal(p["speed"]),
                                                                  p["speed"].ToString().Equals("0.0") ? false : true)).ToList();
                                                            if (pf.Count > 0)
                                                            {
                                                                foreach (Tuple<double, double, string, DateTime, decimal, bool> p in pf)
                                                                    pnts.Add(p);
                                                            }

                                                            //Incrementando Contador
                                                            fecha_pivote = fecha_pivote.AddMinutes(contador_incremento);
                                                        }
                                                        else
                                                            //Terminando Ciclo
                                                            break;
                                                    }
                                                    else
                                                        fecha_pivote = fecha_pivote.AddMinutes(contador_incremento);
                                                }

                                                //Validando Lista
                                                if (pnts.Count > 0)
                                                {
                                                    foreach (Tuple<double, double, string, DateTime, decimal, bool> p in pnts)
                                                        points.Add(p);
                                                }
                                                else
                                                    retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                                            }
                                            else
                                                retorno = new RetornoOperacion("No existe una fecha de fin valida del servicio para esta Unidad");
                                        }
                                        else
                                            retorno = new RetornoOperacion("No existe una fecha de inicio valida del servicio para esta Unidad");
                                        List<DateTime> periodo_x_frecuencia = new List<DateTime>();

                                    }
                                    else
                                    {
                                        //Obteniendo Datos de los Elementos por el Rango de Fechas de las Unidades
                                        pnts = (from JToken p in psGps
                                                where p["idUnit"].ToString().Equals(unidad.Item4)
                                                && (Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()) >= inicio &&
                                                   Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()) <= termino)
                                                select new Tuple<double, double, string, DateTime, decimal, bool>
                                                    (Convert.ToDouble(p["lat"]),
                                                     Convert.ToDouble(p["lng"]),
                                                     p["address"].ToString(),
                                                     Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()),
                                                     Convert.ToDecimal(p["speed"]),
                                                     p["speed"].ToString().Equals("0.0") ? false : true)
                                                ).ToList();
                                        //Añadiendo a Lista
                                        if (pnts.Count > 0)
                                        {
                                            foreach (Tuple<double, double, string, DateTime, decimal, bool> p in pnts)
                                                points.Add(p);
                                        }
                                        else
                                            retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                                    }
                                }
                                else
                                    retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                            }
                            else
                                retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'", unidad.Item2), false);
                        }
                        else
                            retorno = new RetornoOperacion(520, string.Format("{0} - {1}", respuestaGps.StatusCode, respuestaGps.StatusDescription), false);

                        //Incrementando Contador
                        cont++;
                    }
                }

                //Validando Resultado Final
                if (points.Count > 0)
                    retorno = new RetornoOperacion(1, "Datos Obtenidos Exitosamente", true);
                else
                    retorno = new RetornoOperacion(519, string.Format("No hay posiciones para la Unidad '{0}'{1}", unidad.Item2, frecuencia == 0? "" : string.Format(" con la frecuencia de '00:{0}:00'", frecuencia)), false);
            }
            else
                retorno = new RetornoOperacion(521, "No hay fechas de operación para esta Unidad", false);

            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unidad"></param>
        /// <param name="viaje_unidad"></param>
        /// <param name="points"></param>
        /// <param name="posiciones_encontradas"></param>
        /// <returns></returns>
        public static RetornoOperacion ObtieneDatosUnidad(Tuple<int, string, string, string, int, string, string> unidad,
                    List<DataRow> viaje_unidad, List<Tuple<double, double, string, DateTime, decimal, bool>> points,
                    out List<GetLocationResponse> posiciones_encontradas)
        {
            RetornoOperacion retorno = new RetornoOperacion(1, "Datos Obtenidos Exitosamente", true);
            posiciones_encontradas = new List<GetLocationResponse>();

            //Recoriendo Linea del Viaje
            foreach (DataRow v in viaje_unidad)
            {
                //Anidando Posiciones por Linea de Viaje
                foreach (Tuple<double, double, string, DateTime, decimal, bool> point in points)
                {
                    try
                    {
                        posiciones_encontradas.Add
                            (new GetLocationResponse
                            {
                                FolioViaje = v["FolioViaje"].ToString(),
                                IdPuntoEvento = Convert.ToInt32(v["IdPuntoEvento"]),
                                AETC = v["AETC"].ToString(),
                                NumeroContenedor1 = v["NumeroContenedor1"].ToString(),
                                NumeroContenedor2 = v["NumeroContenedor2"].ToString(),
                                NoConsecutivoContenedor = Convert.ToInt32(v["NoConsecutivoContenedor"]),
                                CartaPorte = v["CartaPorte"].ToString(),
                                FechaHoraInicioReal = v["FechaHoraInicioReal"].ToString(),
                                FechaHoraFinReal = v["FechaHoraFinReal"].ToString(),
                                FechaHoraUltReporte = point.Item4.ToString("yyyy-MM-dd HH:mm:ss"),
                                UltUbicacionTransporte = point.Item3,
                                Tracto = unidad.Item2,
                                PlacasTracto = unidad.Item3,
                                NombreChofer = v["NombreChofer"].ToString(),
                                ETA = "", //Calcular ETA
                                LatitudTracto = point.Item1,
                                LongitudTracto = point.Item2,
                                LatitudContenedor1 = Convert.ToInt32(v["IdContenedor1"]) > 0 ? point.Item1 : 0,
                                LongitudContenedor1 = Convert.ToInt32(v["IdContenedor1"]) > 0 ? point.Item2 : 0,
                                LatitudContenedor2 = Convert.ToInt32(v["IdContenedor2"]) > 0 ? point.Item1 : 0,
                                LongitudContenedor2 = Convert.ToInt32(v["IdContenedor2"]) > 0 ? point.Item2 : 0,
                                EncendidoMotor = point.Item6,
                                StatusRuta = "OK"
                            }
                            );
                    }
                    catch (Exception ex)
                    {
                        retorno = new RetornoOperacion(599, ex.Message, false);
                        break;
                    }

                    if (!retorno.OperacionExitosa)
                        break;
                }
            }

            return retorno;
        }
    }
}