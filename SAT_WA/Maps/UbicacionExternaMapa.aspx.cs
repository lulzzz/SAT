using Microsoft.SqlServer.Types;
using SAT.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Maps
{
    public partial class UbicacionExternaMapa : System.Web.UI.Page
    {
        #region Enumeraciones

        /// <summary>
        /// Expresa el Tipo de Ubicación de donde Viene
        /// </summary>
        public enum TipoUbicacion
        {
            /// <summary>
            /// Expresa la Incidencia de la Bitacora de Monitoreo
            /// </summary>
            Incidencia = 1,
            /// <summary>
            /// Expresa el Historial de Incidencias de la Bitacora
            /// </summary>
            HistorialIncicencias,
            /// <summary>
            /// Expresa la Posición del Proveedor del GPS
            /// </summary>
            ProveedorGPS
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
            //Obteniendo Campos del Request
            TipoUbicacion param1 = (TipoUbicacion)Convert.ToByte(Request.QueryString["P1"]);
            string param2 = Request.QueryString["P2"];
            string param3 = Request.QueryString["P3"];
            string param4 = Request.QueryString["P4"];
            string param5 = Request.QueryString["P5"];
            string param6 = Request.QueryString["P6"];
            string param7 = Request.QueryString["P7"];

            //Validando Tipo de Ubicación
            switch (param1)
            {
                case TipoUbicacion.Incidencia:
                    {
                        //Validando que los Valores no esten vacios
                        if (!string.IsNullOrEmpty(param2))
                        {
                            //Instanciando Incidencia
                            using (SAT_CL.Monitoreo.BitacoraMonitoreo incidencia = new SAT_CL.Monitoreo.BitacoraMonitoreo(Convert.ToInt32(param2)))
                            {
                                //Validando que exista la Incidencia
                                if (incidencia.habilitar)
                                {
                                    //Inicializando Mapa
                                    ucMapaUbicacionExterna.InicializaMapaUbicacion("ROAD", 22, incidencia.geoubicacion, incidencia.comentario);
                                }
                            }
                        }
                        break;
                    }
                case TipoUbicacion.HistorialIncicencias:
                    {
                        //Obteniendo Fechas
                        DateTime inicio, fin;
                        DateTime.TryParse(Cadena.RegresaCadenaSeparada(param4.Replace("T", " "), "|", 0), out inicio);
                        DateTime.TryParse(Cadena.RegresaCadenaSeparada(param4.Replace("T", " "), "|", 1), out fin);
                        
                        //Obteniendo Incidencias
                        using (DataTable dtIncidencias = SAT_CL.Monitoreo.BitacoraMonitoreo.CargaHistorialBitacoraMonitoreo(Convert.ToInt32(param5),
                                                            Convert.ToInt32(param6), inicio, fin, Convert.ToByte(param2), ""))
                        {
                            //Validando si hay incidencias
                            if (Validacion.ValidaOrigenDatos(dtIncidencias))
                            {
                                //Declarando Contador
                                int contador = 0;

                                //Definiendo objeto principal para listar los puntos (lat, long) que conforman el poligono
                                List<wucMapaUbicacion.Maps> ubicaciones = new List<wucMapaUbicacion.Maps>();

                                //Recorriendo 
                                foreach (DataRow dr in dtIncidencias.Rows)
                                {
                                    //Instanciando Incidencia
                                    using (SAT_CL.Monitoreo.BitacoraMonitoreo incidencia = new SAT_CL.Monitoreo.BitacoraMonitoreo(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Valdiando que exista una Incidencia
                                        if (incidencia.habilitar)
                                        {
                                            //Validando que exista el Punto
                                            if (incidencia.geoubicacion != SqlGeography.Null)
                                            {
                                                //Determinando el tipo de geometría que representa el valor geográfico
                                                switch (incidencia.geoubicacion.STGeometryType().Value)
                                                {
                                                    case "Point":
                                                        {
                                                            //Creando Punto
                                                            ubicaciones.Add(new wucMapaUbicacion.Maps
                                                            {
                                                                //Asignando Valores
                                                                tipo = dr["Tipo"].ToString(),
                                                                fecha = incidencia.fecha_bitacora,
                                                                descripcion = incidencia.comentario,
                                                                punto = new PointF((float)incidencia.geoubicacion.Lat.Value, (float)incidencia.geoubicacion.Long.Value)
                                                            });
                                                            break;
                                                        }
                                                }
                                            }
                                        }
                                    }

                                    //Si hay 10 puntos o mas
                                    if (contador >= 10)

                                        //Termina Ciclo
                                        break;

                                    //Incrementando Contador
                                    contador++;
                                }

                                //Inicializando Mapa
                                ucMapaUbicacionExterna.InicializaMapaPuntos("roadMap", 22, ubicaciones);
                            }
                        }


                        break;
                    }
                case TipoUbicacion.ProveedorGPS:
                    {
                        //Creando Punto
                        SqlGeography point = SqlGeography.Point(Convert.ToDouble(param2), Convert.ToDouble(param3), 4326);
                        
                        //Inicializando Mapa
                        ucMapaUbicacionExterna.InicializaMapaUbicacion("ROAD", 22, point, param4);
                        break;
                    }
            }
        }

        #endregion
    }
}