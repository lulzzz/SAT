using RestSharp;
using SAT_NISSAN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using TSDK.Base;

namespace SAT_NISSAN.Controllers
{
    [Authorize]
    [RoutePrefix("api/suat")]
    public class GetLocationController : ApiController
    {
        /// <summary>
        /// Método de Obtención de Posiciones por Viaje
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getLocation")]
        public IHttpActionResult getLocation(GetLocationRequest location)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;
            List<string> folios = new List<string>();
            List<DataRow> drsViajes = new List<DataRow>();
            string token_auth = @"", tipo_auth = @"", fecha_peticion = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("yyyy-MM-dd");
            int frecuencia = 0;

            //Validación de Datos Ingesados
            if (location != null)
            {
                //Validando Folios para obtenión de Datos por Viajes
                if (location.TravelFolio.Length > 0)
                {
                    if (location.TravelFolio.Length <= 20)
                    {
                        //Obteniendo Folios Invalidos
                        List<string> folios_invalidos = (from string fls in location.TravelFolio
                                                         where (fls.Trim().Length == 0 || fls.Length >= 50 || string.IsNullOrEmpty(fls))
                                                         select fls).ToList();
                        if (folios_invalidos != null)
                        {
                            if (folios_invalidos.Count == 0)
                            {
                                folios = (from string fls in location.TravelFolio
                                          select fls.Trim()).Distinct().ToList();
                                //Devolviendo resultado positivo
                                retorno = new RetornoOperacion(1, "Validaciones sin Problemas|Folios", true);
                            }
                            else
                                retorno = new RetornoOperacion("Existen folios invalidos en su petición. Solo puede consultar 20 Folios por Petición. Los Folios no deben exceder los 50 caracteres");
                        }
                        else
                            retorno = new RetornoOperacion(1, "Validaciones sin Problemas", true);
                    }
                    else
                        retorno = new RetornoOperacion("El Folio no debe de exceder los 500 caracteres");
                }
                else
                {
                    //Validando Fecha de Inicio
                    if (!string.IsNullOrEmpty(location.StarDate))
                    {
                        //Validando Fecha de Fin
                        if (!string.IsNullOrEmpty(location.EndDate))
                        {
                            if (location.StarDate.Length <= 20)
                            {
                                if (location.StarDate.Length <= 20)
                                {
                                    //Validando Fechas
                                    DateTime.TryParse(location.StarDate, out startDate);
                                    DateTime.TryParse(location.EndDate, out endDate);

                                    //Validando que la Fecha de Fin sea Mayor que la Fecha de Fin
                                    if (DateTime.Compare(startDate, endDate) < 0)
                                    {
                                        //Obteniendo Periodo de Frecuencia
                                        int.TryParse(location.Frecuency, out frecuencia);
                                        if (frecuencia > 0)
                                        {
                                            if (frecuencia <= 60)
                                                //Devolviendo resultado positivo
                                                retorno = new RetornoOperacion(1, "Validaciones sin Problemas|Periodos", true);
                                            else
                                                retorno = new RetornoOperacion(397, "Ingrese una frecuencia menor o igual a 60", false);
                                        }
                                        else
                                            //Devolviendo resultado positivo
                                            retorno = new RetornoOperacion(1, "Validaciones sin Problemas|Periodos", true);
                                    }
                                    //Si las Fechas de son iguales
                                    else if (DateTime.Compare(startDate, endDate) == 0)
                                        retorno = new RetornoOperacion(398, "Las Fechas son iguales", false);
                                    //Si la Fecha de Inicio es mayor que la de Fin
                                    else if (DateTime.Compare(startDate, endDate) > 0)
                                        retorno = new RetornoOperacion(399, "La Fecha de Inicio es mayor a la de Fin", false);
                                }
                                else
                                    retorno = new RetornoOperacion(420, "La Fecha de Fin no debe de exceder los 20 caracteres", false);
                            }
                            else
                                retorno = new RetornoOperacion(420, "La Fecha de Inicio no debe de exceder los 20 caracteres", false);
                        }
                        else
                            retorno = new RetornoOperacion(419, "No hay Fecha de Fin", false);
                    }
                    else
                        retorno = new RetornoOperacion(418, "No hay Fecha de Inicio", false);
                }
            }
            else
                retorno = new RetornoOperacion(397, "No hay Datos por Validar", false);

            /**** Validando resultado ****/
            if (retorno.OperacionExitosa)
            {
                //Desencriptando Token
                int idUsuario = 0, idCompania = 0;
                string token_header = "";
                List<RetornoOperacion> errores_unidad = new List<RetornoOperacion>();
                //Obteniendo Encabezados
                System.Net.Http.HttpRequestMessage req = Request;
                System.Net.Http.Headers.HttpRequestHeaders headers = req.Headers;
                if (headers.Authorization != null)
                {
                    if (headers.Authorization.Scheme.Equals("Bearer"))
                        //Obteniendo Token de la Petición
                        token_header = headers.Authorization.Parameter;
                }
                    
                //Lista Principal de Datos
                List<GetLocationResponse> lista_posiciones = new List<GetLocationResponse>();

                string token_desencriptado = TokenGenerator.ObtenerUsuarioYCompaniaToken(token_header);
                if (!string.IsNullOrEmpty(token_desencriptado))
                {
                    idUsuario = Convert.ToInt32(Cadena.RegresaCadenaSeparada(token_desencriptado, "|", 0));
                    idCompania = Convert.ToInt32(Cadena.RegresaCadenaSeparada(token_desencriptado, "|", 1));
                }

                //Validando Compania
                if (idCompania > 0)
                {
                    switch (Cadena.RegresaCadenaSeparada(retorno.Mensaje, "|", 1))
                    {
                        case "Folios":
                            drsViajes = SAT_CL.Documentacion.Reportes.ObtieneViajesNISSAN(idCompania, folios);
                            break;
                        case "Periodos":
                            drsViajes = SAT_CL.Documentacion.Reportes.ObtieneViajesNISSAN(idCompania, startDate, endDate);
                            break;
                        default:
                            retorno = new RetornoOperacion(-2);
                            break;
                    }

                    //Validando Viajes
                    if (retorno.OperacionExitosa && drsViajes.Count > 0)
                    {
                        switch (Cadena.RegresaCadenaSeparada(retorno.Mensaje, "|", 1))
                        {
                            case "Folios":
                                {
                                    RetornoOperacion val_viajes_unidad = new RetornoOperacion(1);
                                    //Oteniendo Viajes no encontrados
                                    List<string> viajes_no_encontrados = (from string viaje_solicitado in folios
                                                                          join DataRow viaje_consulta in drsViajes
                                                                          on viaje_solicitado equals viaje_consulta["FolioViaje"].ToString()
                                                                          where !(viaje_solicitado.Equals(viaje_consulta["FolioViaje"].ToString()))
                                                                          select viaje_solicitado).Distinct().ToList();
                                    List<DataRow> viajes_encontrados = (from DataRow viaje_consulta in drsViajes
                                                                        join string viaje_solicitado in folios
                                                                        on viaje_consulta["FolioViaje"].ToString() equals viaje_solicitado
                                                                        select viaje_consulta).Distinct().ToList();

                                    if (viajes_encontrados != null)
                                    {
                                        /** Obtiene los Proveedores GPS **/
                                        //1.- IdProveedorWS
                                        //2.- EndPoint
                                        //3.- AccionGPS
                                        //4.- User
                                        //5.- Pass
                                        //6.- AuthUser
                                        //7.- AuthPass
                                        //8.- AuthType
                                        List<Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>>> proveedor_gps =
                                                            (from DataRow p in viajes_encontrados
                                                             where Convert.ToInt32(p["IdProveedorWS"]) > 0
                                                             select
                                                                new Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>>
                                                                (Convert.ToInt32(p["IdProveedorWS"]),
                                                                 p["EndPoint"].ToString(),
                                                                 p["AccionGPS"].ToString(),
                                                                 new Tuple<string, string>(p["User"].ToString(), p["Pass"].ToString()),
                                                                 new Tuple<string, string, string>(p["AuthUser"].ToString(), p["AuthPass"].ToString(), p["AuthType"].ToString()))).Distinct().ToList();
                                        if (proveedor_gps != null)
                                        {
                                            foreach (Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>> gps in proveedor_gps)
                                            {
                                                //Configurando Petición
                                                RestClient clienteGps = new RestClient();
                                                RestRequest peticionGps = new RestRequest();
                                                val_viajes_unidad = GetLocationMethods.ObtieneAccesoProveedorGPS(gps, out clienteGps, out token_auth, out tipo_auth);
                                                if (val_viajes_unidad.OperacionExitosa)
                                                    //Asignando Petición de Proveedor GPS
                                                    peticionGps = GetLocationMethods.ObtienePeticionGps(gps, clienteGps, token_auth, tipo_auth);

                                                /** Validando Obtención de Token para consumo GPS **/
                                                if (val_viajes_unidad.OperacionExitosa)
                                                {
                                                    //Agrupando Viajes
                                                    List<DataRow> viajes_x_gps = (from DataRow v in viajes_encontrados
                                                                                  where Convert.ToInt32(v["IdProveedorWS"]) == gps.Item1
                                                                                  select v).ToList();
                                                    List<string> v_x_gps = (from DataRow v in viajes_encontrados
                                                                            where Convert.ToInt32(v["IdProveedorWS"]) == gps.Item1
                                                                            select v["FolioViaje"].ToString()).Distinct().ToList();
                                                    if (viajes_x_gps.Count > 0 && v_x_gps.Count > 0)
                                                    {
                                                        //Recorriendo Viajes
                                                        foreach (string viaje in v_x_gps)
                                                        {
                                                            //Obteniendo Viajes por Folio
                                                            List<DataRow> vjs = (from DataRow v in viajes_x_gps
                                                                                 where v["FolioViaje"].Equals(viaje)
                                                                                 select v).ToList();

                                                            if (vjs.Count > 0)
                                                            {
                                                                //Obteniendo Unidades por Viaje
                                                                List<Tuple<int, string, string, string, int, string, string>>
                                                                        unidades = (from DataRow u in vjs
                                                                                    where Convert.ToInt32(u["IdProveedorUnidadWS"]) > 0
                                                                                    select new Tuple<int, string, string, string, int, string, string>
                                                                                    (Convert.ToInt32(u["IdTractor"]),
                                                                                    u["Tracto"].ToString(),
                                                                                    u["PlacasTracto"].ToString(),
                                                                                    u["AntenaGPS"].ToString(),
                                                                                    Convert.ToInt32(u["IdProveedorUnidadWS"]),
                                                                                    u["InicioViajeUnidad"].ToString(),
                                                                                    u["TerminoViajeUnidad"].ToString())).Distinct().ToList();
                                                                if (unidades.Count > 0)
                                                                {
                                                                    //Recorriendo Unidades
                                                                    foreach (Tuple<int, string, string, string, int, string, string> unidad in unidades)
                                                                    {
                                                                        //Obteniendo los Viajes por Unidad
                                                                        List<DataRow> viaje_unidad = (from DataRow vu in vjs
                                                                                                      where Convert.ToInt32(vu["IdTractor"]) == unidad.Item1
                                                                                                      select vu).ToList();
                                                                        if (viaje_unidad.Count > 0)
                                                                        {
                                                                            List<Tuple<double, double, string, DateTime, decimal, bool>> points = new List<Tuple<double, double, string, DateTime, decimal, bool>>();
                                                                            RetornoOperacion val_uni_pos = GetLocationMethods.ObtienePosicionesUnidad(unidad, peticionGps, clienteGps, 0, DateTime.MinValue, DateTime.MinValue, out points);

                                                                            if (val_uni_pos.OperacionExitosa && points.Count > 0)
                                                                            {
                                                                                List<GetLocationResponse> lts = new List<GetLocationResponse>();
                                                                                val_viajes_unidad = GetLocationMethods.ObtieneDatosUnidad(unidad, viaje_unidad, points, out lts);
                                                                                if (val_viajes_unidad.OperacionExitosa)
                                                                                {
                                                                                    foreach (GetLocationResponse l in lts)
                                                                                        lista_posiciones.Add(l);
                                                                                }
                                                                                else
                                                                                    //Asignando Error Gestionado
                                                                                    errores_unidad.Add(val_viajes_unidad);
                                                                            }
                                                                            else
                                                                                //Asignando Error Gestionado
                                                                                errores_unidad.Add(new RetornoOperacion(518, string.Format("No hay posiciones disponibles para la Unidad '{0}' para el periodo del '{1:yyyy-MM-dd HH:mm:ss}' hasta '{2:yyyy-MM-dd HH:mm:ss}'", unidad.Item2, Convert.ToDateTime(unidad.Item6), Convert.ToDateTime(unidad.Item7)), false));
                                                                        }
                                                                        else
                                                                            val_viajes_unidad = new RetornoOperacion(517, string.Format("No se puede recuperar el Viaje de la Unidad '{0}'", unidad.Item2), false);
                                                                    }
                                                                }
                                                                else
                                                                    val_viajes_unidad = new RetornoOperacion(516, "No hay Unidades configuradas para este Proveedor de GPS", false);
                                                            }
                                                            else
                                                                val_viajes_unidad = new RetornoOperacion(515, "No hay viajes para este proveedor", false);
                                                        }
                                                    }
                                                    else
                                                        val_viajes_unidad = new RetornoOperacion(514, "No hay viajes para este Proveedor de GPS", false);
                                                }
                                            }
                                        }
                                        else
                                            val_viajes_unidad = new RetornoOperacion(513, "No hay proveedores GPS disponibles", false);
                                    }

                                    if (viajes_no_encontrados != null)
                                    {
                                        foreach (string vj in viajes_no_encontrados)
                                        {
                                            lista_posiciones.Add
                                                (new GetLocationResponse
                                                {
                                                    FolioViaje = vj,
                                                    IdPuntoEvento = 0,
                                                    AETC = "",
                                                    NumeroContenedor1 = "",
                                                    NumeroContenedor2 = "",
                                                    NoConsecutivoContenedor = 0,
                                                    CartaPorte = "",
                                                    FechaHoraInicioReal = "",
                                                    FechaHoraFinReal = "",
                                                    FechaHoraUltReporte = "",
                                                    UltUbicacionTransporte = "",
                                                    Tracto = "",
                                                    PlacasTracto = "",
                                                    NombreChofer = "",
                                                    ETA = "", //Calcular ETA
                                                    LatitudTracto = 0,
                                                    LongitudTracto = 0,
                                                    LatitudContenedor1 = 0,
                                                    LongitudContenedor1 = 0,
                                                    LatitudContenedor2 = 0,
                                                    LongitudContenedor2 = 0,
                                                    EncendidoMotor = false,
                                                    StatusRuta = "OK"
                                                }
                                                );
                                            //Añadiendo el Error
                                            errores_unidad.Add(new RetornoOperacion(512, string.Format("El Folio '{0}'", vj),false));
                                        }
                                    }
                                    
                                    //Validaciones Generales de Consumo (ERRORES CRUCIALES QUE EVITAN EL CONSUMO)
                                    if (retorno.OperacionExitosa && val_viajes_unidad.OperacionExitosa)
                                    {
                                        List<Errors> errores = new List<Errors>();
                                        if (errores_unidad.Count > 0)
                                        {
                                            foreach (RetornoOperacion e in errores_unidad)
                                            {
                                                if (!e.OperacionExitosa)
                                                {
                                                    errores.Add(new Errors
                                                    {
                                                        code = e.IdRegistro,
                                                        source = new Source { pointer = "" },
                                                        detail = e.Mensaje,
                                                        title = ""
                                                    });
                                                }
                                            }
                                        }

                                        if (errores.Count == 0)
                                        {
                                            /** Armando Arreglo de Datos **/
                                            SuatResponse2 suatResponse = new SuatResponse2
                                            {
                                                success = true,
                                                data = lista_posiciones
                                            };
                                            return Ok(suatResponse);
                                        }
                                        else
                                        {
                                            /** Armando Arreglo de Datos **/
                                            SuatResponse2 suatResponse = new SuatResponse2
                                            {
                                                success = true,
                                                data = lista_posiciones,
                                                errors = errores
                                            };
                                            return Ok(suatResponse);
                                        }
                                    }
                                    else
                                    {
                                        SuatResponse2 failauthresponse;
                                        if (!retorno.OperacionExitosa)
                                        {
                                            failauthresponse = new SuatResponse2
                                            {
                                                success = false,
                                                data = { },
                                                errors = new List<Errors>()
                                                {
                                                    new Errors
                                                    {
                                                        code = retorno.IdRegistro,
                                                        detail = retorno.Mensaje,
                                                        title = "Informacion incorrecta"
                                                    }
                                                }
                                            };
                                        }
                                        else if (!val_viajes_unidad.OperacionExitosa)
                                        {
                                            failauthresponse = new SuatResponse2
                                            {
                                                success = false,
                                                data = { },
                                                errors = new List<Errors>()
                                                {
                                                    new Errors
                                                    {
                                                        code = val_viajes_unidad.IdRegistro,
                                                        detail = val_viajes_unidad.Mensaje,
                                                        title = "Error en Unidades"
                                                    }
                                                }
                                            };
                                        }
                                        else
                                            failauthresponse = null;

                                        //string json = JsonConvert.SerializeObject(failauthresponse);
                                        return Json(failauthresponse);
                                    }
                                }
                            case "Periodos":
                                {
                                    RetornoOperacion val_viajes_fechas = new RetornoOperacion(1);
                                    /** Obtiene los Proveedores GPS **/
                                    //1.- IdProveedorWS
                                    //2.- EndPoint
                                    //3.- AccionGPS
                                    //4.- User
                                    //5.- Pass
                                    //6.- AuthUser
                                    //7.- AuthPass
                                    //8.- AuthType
                                    List<Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>>> proveedor_gps =
                                                        (from DataRow p in drsViajes
                                                         where Convert.ToInt32(p["IdProveedorWS"]) > 0
                                                         select
                                                            new Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>>
                                                            (Convert.ToInt32(p["IdProveedorWS"]),
                                                             p["EndPoint"].ToString(),
                                                             p["AccionGPS"].ToString(),
                                                             new Tuple<string, string>(p["User"].ToString(), p["Pass"].ToString()),
                                                             new Tuple<string, string, string>(p["AuthUser"].ToString(), p["AuthPass"].ToString(), p["AuthType"].ToString()))).Distinct().ToList();
                                    if (proveedor_gps != null)
                                    {
                                        foreach (Tuple<int, string, string, Tuple<string, string>, Tuple<string, string, string>> gps in proveedor_gps)
                                        {
                                            //Configurando Petición
                                            RestClient clienteGps = new RestClient();
                                            RestRequest peticionGps = new RestRequest();
                                            val_viajes_fechas = GetLocationMethods.ObtieneAccesoProveedorGPS(gps, out clienteGps, out token_auth, out tipo_auth);
                                            if (val_viajes_fechas.OperacionExitosa)
                                                //Asignando Petición de Proveedor GPS
                                                peticionGps = GetLocationMethods.ObtienePeticionGps(gps, clienteGps, token_auth, tipo_auth);

                                            /** Validando Obtención de Token para consumo GPS **/
                                            if (val_viajes_fechas.OperacionExitosa)
                                            {
                                                //Agrupando Viajes
                                                List<DataRow> viajes_x_gps = (from DataRow v in drsViajes
                                                                              where Convert.ToInt32(v["IdProveedorWS"]) == gps.Item1
                                                                              select v).ToList();
                                                List<string> v_x_gps = (from DataRow v in drsViajes
                                                                        where Convert.ToInt32(v["IdProveedorWS"]) == gps.Item1
                                                                        select v["FolioViaje"].ToString()).Distinct().ToList();
                                                if (viajes_x_gps.Count > 0 && v_x_gps.Count > 0)
                                                {
                                                    //Recorriendo Viajes
                                                    foreach (string viaje in v_x_gps)
                                                    {
                                                        //Obteniendo Viajes por Folio
                                                        List<DataRow> vjs = (from DataRow v in viajes_x_gps
                                                                             where v["FolioViaje"].Equals(viaje)
                                                                             select v).ToList();

                                                        if (vjs.Count > 0)
                                                        {
                                                            //Obteniendo Unidades por Viaje
                                                            List<Tuple<int, string, string, string, int, string, string>>
                                                                    unidades = (from DataRow u in vjs
                                                                                where Convert.ToInt32(u["IdProveedorUnidadWS"]) > 0
                                                                                select new Tuple<int, string, string, string, int, string, string>
                                                                                (Convert.ToInt32(u["IdTractor"]),
                                                                                u["Tracto"].ToString(),
                                                                                u["PlacasTracto"].ToString(),
                                                                                u["AntenaGPS"].ToString(),
                                                                                Convert.ToInt32(u["IdProveedorUnidadWS"]),
                                                                                u["InicioViajeUnidad"].ToString(),
                                                                                u["TerminoViajeUnidad"].ToString())).Distinct().ToList();
                                                            if (unidades.Count > 0)
                                                            {
                                                                //Recorriendo Unidades
                                                                foreach (Tuple<int, string, string, string, int, string, string> unidad in unidades)
                                                                {
                                                                    //Obteniendo los Viajes por Unidad
                                                                    List<DataRow> viaje_unidad = (from DataRow vu in vjs
                                                                                                  where Convert.ToInt32(vu["IdTractor"]) == unidad.Item1
                                                                                                  select vu).ToList();
                                                                    if (viaje_unidad.Count > 0)
                                                                    {
                                                                        List<Tuple<double, double, string, DateTime, decimal, bool>> points = new List<Tuple<double, double, string, DateTime, decimal, bool>>();
                                                                        RetornoOperacion val_uni_pos = GetLocationMethods.ObtienePosicionesUnidad(unidad, peticionGps, clienteGps, frecuencia, startDate, endDate, out points);

                                                                        if (val_uni_pos.OperacionExitosa)
                                                                        {
                                                                            if (points.Count > 0)
                                                                            {
                                                                                List<GetLocationResponse> lts = new List<GetLocationResponse>();
                                                                                val_viajes_fechas = GetLocationMethods.ObtieneDatosUnidad(unidad, viaje_unidad, points, out lts);
                                                                                if (val_viajes_fechas.OperacionExitosa)
                                                                                {
                                                                                    foreach (GetLocationResponse l in lts)
                                                                                        lista_posiciones.Add(l);
                                                                                }
                                                                                else
                                                                                    //Asignando Error Gestionado
                                                                                    errores_unidad.Add(val_viajes_fechas);
                                                                            }
                                                                            else
                                                                                //Asignando Error Gestionado
                                                                                errores_unidad.Add(new RetornoOperacion(518, string.Format("No hay posiciones disponibles para la Unidad '{0}' para el periodo del '{1:yyyy-MM-dd HH:mm:ss}' hasta '{2:yyyy-MM-dd HH:mm:ss}'", unidad.Item2, Convert.ToDateTime(unidad.Item6), Convert.ToDateTime(unidad.Item7)), false));
                                                                        }
                                                                        else
                                                                            //Asignando Error Gestionado
                                                                            errores_unidad.Add(val_uni_pos);
                                                                    }
                                                                    else
                                                                        val_viajes_fechas = new RetornoOperacion(517, string.Format("No se puede recuperar el Viaje de la Unidad '{0}'", unidad.Item2), false);
                                                                }
                                                            }
                                                            else
                                                                val_viajes_fechas = new RetornoOperacion(516, "No hay Unidades configuradas para este Proveedor de GPS", false);
                                                        }
                                                        else
                                                            val_viajes_fechas = new RetornoOperacion(515, "No hay viajes para este proveedor", false);
                                                    }
                                                }
                                                else
                                                    val_viajes_fechas = new RetornoOperacion(514, "No hay viajes para este Proveedor de GPS", false);
                                            }
                                        }
                                    }
                                    else
                                        val_viajes_fechas = new RetornoOperacion(513, "No hay proveedores GPS disponibles", false);

                                    //Validaciones Generales de Consumo (ERRORES CRUCIALES QUE EVITAN EL CONSUMO)
                                    if (retorno.OperacionExitosa && val_viajes_fechas.OperacionExitosa)
                                    {
                                        List<Errors> errores = new List<Errors>();
                                        if (errores_unidad.Count > 0)
                                        {
                                            foreach (RetornoOperacion e in errores_unidad)
                                            {
                                                if (!e.OperacionExitosa)
                                                {
                                                    errores.Add(new Errors
                                                    {
                                                        code = e.IdRegistro,
                                                        source = new Source { pointer = "" },
                                                        detail = e.Mensaje,
                                                        title = ""
                                                    });
                                                }
                                            }
                                        }

                                        if (errores.Count == 0)
                                        {
                                            /** Armando Arreglo de Datos **/
                                            SuatResponse2 suatResponse = new SuatResponse2
                                            {
                                                success = true,
                                                data = lista_posiciones
                                            };
                                            return Ok(suatResponse);
                                        }
                                        else
                                        {
                                            /** Armando Arreglo de Datos **/
                                            SuatResponse2 suatResponse = new SuatResponse2
                                            {
                                                success = true,
                                                data = lista_posiciones,
                                                errors = errores
                                            };
                                            return Ok(suatResponse);
                                        }
                                    }
                                    else
                                    {
                                        SuatResponse2 failauthresponse;
                                        if (!retorno.OperacionExitosa)
                                        {
                                            failauthresponse = new SuatResponse2
                                            {
                                                success = false,
                                                data = { },
                                                errors = new List<Errors>()
                                                {
                                                    new Errors
                                                    {
                                                        code = retorno.IdRegistro,
                                                        detail = retorno.Mensaje,
                                                        title = "Informacion incorrecta"
                                                    }
                                                }
                                            };
                                        }
                                        else if (!val_viajes_fechas.OperacionExitosa)
                                        {
                                            failauthresponse = new SuatResponse2
                                            {
                                                success = false,
                                                data = { },
                                                errors = new List<Errors>()
                                                {
                                                    new Errors
                                                    {
                                                        code = val_viajes_fechas.IdRegistro,
                                                        detail = val_viajes_fechas.Mensaje,
                                                        title = "Error en Unidades"
                                                    }
                                                }
                                            };
                                        }
                                        else
                                            failauthresponse = null;

                                        //string json = JsonConvert.SerializeObject(failauthresponse);
                                        return Json(failauthresponse);
                                    }
                                }
                        }
                    }
                    else
                        retorno = new RetornoOperacion(452, string.Format("No hay Viajes Registrados {0}", Cadena.RegresaCadenaSeparada(retorno.Mensaje, "|", 1).Equals("Folios") ? "para los Folios especificados" : "para el periodo de Fechas Ingresadas"), false);
                }
                else
                    retorno = new RetornoOperacion(453, "No se puede recuperar la Compania", false);
            }

            /** Armando Arreglo de Datos **/
            SuatResponse2 respuestaFinal = new SuatResponse2
            {
                success = false,
                data = {},
                errors = new List<Errors>() 
                { 
                    new Errors 
                    { 
                        code = retorno.IdRegistro,
                        detail = retorno.Mensaje,
                        title = "Informacion incorrecta"
                    } 
                }
            };
            return Json(respuestaFinal);
        }

    }
}
