using Microsoft.SqlServer.Types;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_WCF.Operacion
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Despacho" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Despacho.svc o Despacho.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Despacho : IDespacho
    {
        /// <summary>
        /// Método encargado de Obtener los Datos del Encabezado del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public string ObtieneDatosEncabezadoServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("ServicioEncabezado"));

            //Obteniendo Datos
            using (DataTable dtEncabezado = Reporte.CargaEncabezadoViajeMovil(id_servicio))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEncabezado))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtEncabezado.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("ServicioEncabezado").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("ServicioEncabezado").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }

        /// <summary>
        /// Obtiene los datos del operador y unidad asignada a una sesión determinada
        /// </summary>
        /// <param name="id_usuario_sesion">Id de Sesion de usuario</param>
        /// <returns></returns>
        public string ObtieneUnidadAsignadaOperadorSesion(int id_usuario_sesion)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("UnidadAsignadaOperador"));

            //Definiendo tabla de interés
            DataTable mit = new DataTable("Table");
            mit.Columns.Add("Operador", typeof(String));
            mit.Columns.Add("UnidadMotriz", typeof(String));

            //Instanciando sesión
            using (SAT_CL.Seguridad.UsuarioSesion sesion = new SAT_CL.Seguridad.UsuarioSesion(id_usuario_sesion))
            {
                //Recuperando operador asignado
                using (SAT_CL.Global.Operador operador = SAT_CL.Global.Operador.ObtieneOperadorUsuario(sesion.id_usuario))
                {
                    //Recuperando unidad asignada
                    using (SAT_CL.Global.Unidad unidad = SAT_CL.Global.Unidad.ObtieneUnidadOperador(operador.id_operador))
                    {
                        //Añadiendo operador y unidad
                        mit.Rows.Add(operador.nombre, unidad.numero_unidad);

                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s, XmlWriteMode.WriteSchema);

                            //Obteniendo DataSet en XML
                            XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                            //Añadiendo Elemento al nodo de Publicaciones
                            document.Element("UnidadAsignadaOperador").Add(dataTableElement);
                        }
                    }
                }
            }

            //Devolviendo resultado
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener los Datos de las Paradas del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// </summary>
        /// <returns></returns>
        public string ObtieneParadasServicioActivas(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("ParadaServicio"));

            //Obteniendo Datos
            using (DataTable dtParadas = Reporte.CargaParadasViajeMovil(id_servicio))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtParadas))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtParadas.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("ParadaServicio").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("ParadaServicio").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener las Paradas del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public string ObtieneParadasServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("ParadaServicio"));

            //Obteniendo Datos
            using (DataTable dtParadas = Reporte.CargaParadasTotalesViajeMovil(id_servicio))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtParadas))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtParadas.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("ParadaServicio").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("ParadaServicio").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener las Referencias del Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns></returns>
        public string ObtieneReferenciasServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("Referencias"));

            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(id_servicio))
            {
                //Validando que existe el Servicio
                if (servicio.habilitar)
                {
                    //Creando Tabla Intermedia
                    using (DataTable dt = new DataTable("Table"))
                    {
                        //Definiendo Columnas
                        dt.Columns.Add("Id", typeof(int));
                        dt.Columns.Add("NombreTipo", typeof(string));
                        dt.Columns.Add("ValorReferencia", typeof(string));
                        dt.Columns.Add("Fecha", typeof(string));

                        //Obteniendo Datos
                        using (DataTable dtReferencias = SAT_CL.Global.Referencia.CargaReferenciasRegistroViaje(servicio.id_compania_emisor, id_servicio))
                        {
                            //Validando que existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtReferencias))
                            {
                                //Obteniendo Dia de Hoy
                                DateTime hoy = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();

                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtReferencias.Rows)
                                {
                                    //Obteniendo Fecha
                                    DateTime fecha;
                                    DateTime.TryParse(dr["Fecha"].ToString(), out fecha);

                                    //Insertando Fila en Tabla Intermedia
                                    dt.Rows.Add(Convert.ToInt32(dr["Id"]), dr["NombreTipo"].ToString(), dr["ValorReferencia"].ToString(),
                                        fecha.ToString(fecha.Date.CompareTo(hoy.Date) == 0 ? "HH:mm" : "dd MMM HH:mm", System.Globalization.CultureInfo.CreateSpecificCulture("es-MX")));
                                }
                                
                                //Creando flujo de memoria
                                using (System.IO.Stream s = new System.IO.MemoryStream())
                                {
                                    //Leyendo flujo de datos XML
                                    dt.WriteXml(s, XmlWriteMode.WriteSchema);

                                    //Obteniendo DataSet en XML
                                    XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                                    //Añadiendo Elemento al nodo de Publicaciones
                                    document.Element("Referencias").Add(dataTableElement);
                                }
                            }
                            else
                                //Añadiendo Resultado por Defecto
                                document.Element("Referencias").Add(new XElement("NewDataSet"));
                        }
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("Referencias").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener los Accesorios de un Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns></returns>
        public string ObtieneAccesoriosViaje(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("Accesorios"));

            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(id_servicio))
            {
                //Validando que existe el Servicio
                if (servicio.habilitar)
                {
                    //Obteniendo Datos
                    using (DataTable dtAccesorios = SAT_CL.ControlEvidencia.Reportes.ObtieneAccesoriosServicio(id_servicio))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAccesorios))
                        {
                            //Creando flujo de memoria
                            using (System.IO.Stream s = new System.IO.MemoryStream())
                            {
                                //Leyendo flujo de datos XML
                                dtAccesorios.WriteXml(s, XmlWriteMode.WriteSchema);

                                //Obteniendo DataSet en XML
                                XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                                //Añadiendo Elemento al nodo de Publicaciones
                                document.Element("Accesorios").Add(dataTableElement);
                            }
                        }
                        else
                            //Añadiendo Resultado por Defecto
                            document.Element("Accesorios").Add(new XElement("NewDataSet"));
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("Accesorios").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener los Anticipos de un Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns></returns>
        public string ObtieneAnticiposViaje(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("Anticipos"));

            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(id_servicio))
            {
                //Validando que existe el Servicio
                if (servicio.habilitar)
                {
                    //Obteniendo Datos
                    using (DataTable dtAnticipos = SAT_CL.EgresoServicio.Reportes.ReporteAnticipos(id_servicio))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAnticipos))
                        {
                            //Creando flujo de memoria
                            using (System.IO.Stream s = new System.IO.MemoryStream())
                            {
                                //Leyendo flujo de datos XML
                                dtAnticipos.WriteXml(s, XmlWriteMode.WriteSchema);

                                //Obteniendo DataSet en XML
                                XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                                //Añadiendo Elemento al nodo de Publicaciones
                                document.Element("Anticipos").Add(dataTableElement);
                            }
                        }
                        else
                            //Añadiendo Resultado por Defecto
                            document.Element("Anticipos").Add(new XElement("NewDataSet"));
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("Anticipos").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener los Datos del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// </summary>
        /// <returns></returns>
        public string ObtieneDatosServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("DatosServicio"));

            //Obteniendo Datos
            using (DataTable dtDatosServicio = Reporte.CargaDatosViajeMovil(id_servicio))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDatosServicio))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtDatosServicio.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("DatosServicio").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("DatosServicio").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Cargar los Servicios sin Liquidar de un Operador para la Aplicación Móvil
        /// </summary>
        /// <param name="id_usuario">Usuario de la Aplicación Móvil</param>
        /// <param name="id_tiempo">Valor de Tiempo por Filtrar</param>
        /// <returns></returns>
        public string ObtieneServiciosOperador(int id_usuario, int tiempo)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("ServiciosOperador"));

            //Obteniendo Datos
            using (DataTable dtServiciosOperador = Reporte.ObtieneServiciosOperadorMovil(id_usuario, tiempo))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtServiciosOperador))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtServiciosOperador.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("ServiciosOperador").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("ServiciosOperador").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Cargar el Servicio Actual de un Operador para la Aplicación Móvil
        /// </summary>
        /// <param name="id_usuario">Usuario de la Aplicación Móvil</param>
        /// <returns></returns>
        public string ServicioActualOperador(int id_usuario)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Obteniendo Datos
            using (DataTable dtServiciosOperador = Reporte.ObtieneServicioActivoOperadorMovil(id_usuario))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtServiciosOperador))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtServiciosOperador.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Add(new XElement("ServicioActual"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Determinar si el Viaje esta Listo para Iniciarse
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string IniciarViaje(int id_servicio, double latitud, double longitud, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Servicio
                        using (Servicio servicio = new Servicio(id_servicio))
                        {
                            //Validando que este Documentado
                            if (servicio.habilitar && servicio.estatus == SAT_CL.Documentacion.Servicio.Estatus.Documentado)
                            {
                                //Instanciando Primer Parada
                                using (Parada parada1 = new Parada(1.00M, servicio.id_servicio))
                                {
                                    //Validando que exista la Parada
                                    if (parada1.habilitar)
                                    {
                                        //Instanciando Ubicación
                                        using (SAT_CL.Global.Ubicacion ubicacion = new SAT_CL.Global.Ubicacion(parada1.id_ubicacion))
                                        {
                                            //Validando que exista la Ubicación
                                            if (ubicacion.habilitar)
                                            {
                                                //Obteniendo la Ubicación de la Unidad
                                                SqlGeography geoubicacion_unidad = DatosEspaciales.ConvierteLatLongPuntoSqlGeography(latitud, longitud, 4326);

                                                //Validando que exista la Geoubicación
                                                if (ubicacion.geoubicacion != SqlGeography.Null)
                                                {
                                                    //Obtiene Distancia Permitida
                                                    int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                    //Obteniendo Distancia por Ubicación
                                                    using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                                   SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(
                                                                                            0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                    {
                                                        //Validando que exista la Referencia
                                                        if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                        {
                                                            //Recorriendo Registro
                                                            foreach (DataRow dr in dtDistancia.Rows)

                                                                //Obteniendo Distancia Permitida
                                                                distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                        }
                                                    }

                                                    //Validando Tipo de Geometria
                                                    switch (ubicacion.geoubicacion.STGeometryType().Value)
                                                    {
                                                        case "Point":
                                                            {
                                                                //Validando que el Punto no exceda mas de 10 metros
                                                                if (DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, geoubicacion_unidad, distancia_permitida))

                                                                    //Instanciando Resultado Positivo
                                                                    result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                                else
                                                                    //Instanciando Excepción
                                                                    result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                                break;
                                                            }
                                                        case "LineString":
                                                        case "CompoundCurve":
                                                        case "Polygon":
                                                        case "CurvePolygon":
                                                            {
                                                                //Obtiene Punto mas Cercano
                                                                SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                                //Validando que exista el Punto mas Cercano
                                                                if (punto_cercano != SqlGeography.Null)
                                                                {
                                                                    //Validando que haya Intersección en las columnas
                                                                    if (DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, geoubicacion_unidad))
                                                                    {
                                                                        //Instanciando Resultado Positivo
                                                                        result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                                    }
                                                                    else
                                                                        //Instanciando Excepción
                                                                        result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                                }
                                                                else
                                                                    //Instanciando Excepción
                                                                    result = new RetornoOperacion(-2, "No se logro obtener el Punto mas Cercano", false);
                                                                break;
                                                            }
                                                    }

                                                    //Validando la Operación
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Realizando la actualización de la parada
                                                        result = parada1.ActualizarFechaLlegada(Fecha.ObtieneFechaEstandarMexicoCentro(), Parada.TipoActualizacionLlegada.APP, 0,
                                                                                                EstanciaUnidad.TipoActualizacionInicio.APP, user_sesion.id_usuario);

                                                        //Validando la Operación
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Actualizando Ultima Actividad
                                                            result = user_sesion.ActualizaUltimaActividad();

                                                            //Validando Operación Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Instanciando Servicio
                                                                result = new RetornoOperacion(id_servicio);

                                                                //Completando Transacción
                                                                trans.Complete();
                                                            }
                                                        }
                                                    }

                                                    /*/Validando que el Tipo de GeoUbicación se un Punto
                                                    if (ubicacion.geoubicacion.STGeometryType().Value.Equals("Point"))
                                                    {
                                                        //Validando que el Punto no exceda mas de 10 metros
                                                        if (DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, geoubicacion_unidad, 15))
                                                
                                                            //Instanciando Resultado Positivo
                                                            result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                    }
                                                    //Validando que el Tipo de GeoUbicación se un Poligono
                                                    else if (ubicacion.geoubicacion.STGeometryType().Value.Equals("Polygon"))
                                                    {
                                                        //Obteniendo Puntos de un Polygon
                                                        string mensaje = "";
                                                        SqlGeography[] puntos_referencia = DatosEspaciales.ObtienePuntosPolygon(ubicacion.geoubicacion, (int)ubicacion.geoubicacion.STSrid.Value, out mensaje);

                                                        //Obtiene Punto mas Cercano
                                                        SqlGeography punto_cercano = DatosEspaciales.ObtienePuntoMasCercano(geoubicacion_unidad, puntos_referencia);

                                                        //Validando que exista el Punto mas Cercano
                                                        if (punto_cercano != SqlGeography.Null)
                                                        {
                                                            //Validando que haya Intersección en las columnas
                                                            if (DatosEspaciales.ValidaDistanciaPermitida(geoubicacion_unidad, punto_cercano, 15))

                                                                //Instanciando Resultado Positivo
                                                                result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(-2, "No existe el Punto mas Cercano", false);
                                                    }//*/
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion(-2, "No existe la Ubicación Deseada", false);
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(-2, "No existe la Parada", false);
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion(-3, "El Servicio no esta Documentado", false);
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion(-2, "La Sesión no esta Activa", false);
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de asignar la Llegada a una Parada
        /// </summary>
        /// <param name="id_parada">Parada</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string LlegadaParada(int id_parada, double latitud, double longitud, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Primer Parada
                        using (Parada parada1 = new Parada(id_parada))
                        {
                            //Validando que exista la Parada
                            if (parada1.habilitar)
                            {
                                //Validando que es una Parada Operativa
                                if (parada1.Tipo == Parada.TipoParada.Operativa)
                                {
                                    //Instanciando Ubicación
                                    using (SAT_CL.Global.Ubicacion ubicacion = new SAT_CL.Global.Ubicacion(parada1.id_ubicacion))
                                    {
                                        //Validando que exista la Ubicación
                                        if (ubicacion.habilitar)
                                        {
                                            //Obteniendo la Ubicación de la Unidad
                                            SqlGeography geoubicacion_unidad = DatosEspaciales.ConvierteLatLongPuntoSqlGeography(latitud, longitud, 4326);

                                            //Obtiene Distancia Permitida
                                            int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                            //Obteniendo Distancia por Ubicación
                                            using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                           SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(
                                                                                    0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                            {
                                                //Validando que exista la Referencia
                                                if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                {
                                                    //Recorriendo Registro
                                                    foreach (DataRow dr in dtDistancia.Rows)

                                                        //Obteniendo Distancia Permitida
                                                        distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                }
                                            }

                                            //Validando Tipo de Geometria
                                            switch (ubicacion.geoubicacion.STGeometryType().Value)
                                            {
                                                case "Point":
                                                    {
                                                        //Validando que el Punto no exceda mas de 10 metros
                                                        if (DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, geoubicacion_unidad, distancia_permitida))

                                                            //Instanciando Resultado Positivo
                                                            result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                        break;
                                                    }
                                                case "LineString":
                                                case "CompoundCurve":
                                                case "Polygon":
                                                case "CurvePolygon":
                                                    {
                                                        //Obtiene Punto mas Cercano
                                                        SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                        //Validando que exista el Punto mas Cercano
                                                        if (punto_cercano != SqlGeography.Null)
                                                        {
                                                            //Validando que haya Intersección en las columnas
                                                            if (DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, geoubicacion_unidad))
                                                            {
                                                                //Instanciando Resultado Positivo
                                                                result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(-2, "No se logro obtener el Punto mas Cercano", false);
                                                        break;
                                                    }
                                            }



                                        }
                                        else
                                            //No se puede Acceder a la Parada
                                            result = new RetornoOperacion("No Existe la Ubicación");
                                    }
                                }
                                else if (parada1.Tipo == Parada.TipoParada.Servicio)
                                    
                                    //Instanciando Resultado Positivo
                                    result = new RetornoOperacion(0, "Es una Parada de Servicio", true);
                            }
                            else
                                //No se puede Acceder a la Parada
                                result = new RetornoOperacion("No Existe la Parada");

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Realizando la actualización de la parada
                                result = parada1.ActualizarFechaLlegada(Fecha.ObtieneFechaEstandarMexicoCentro(), Parada.TipoActualizacionLlegada.APP, 0,
                                                                        EstanciaUnidad.TipoActualizacionInicio.APP, user_sesion.id_usuario);

                                //Validando la Operación
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Parada
                                    int idParada = result.IdRegistro;

                                    //Actualizando Ultima Actividad
                                    result = user_sesion.ActualizaUltimaActividad();

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Servicio
                                        result = new RetornoOperacion(idParada);

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de asignar la Salida de una Parada
        /// </summary>
        /// <param name="id_parada">Parada</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string SalidaParada(int id_parada, double latitud, double longitud, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Primer Parada
                        using (Parada objParada = new Parada(id_parada))
                        {
                            //Validando que exista la Parada
                            if (objParada.habilitar)
                            {
                                //Validando el Tipo de Parada
                                if (objParada.Tipo == Parada.TipoParada.Operativa)
                                {
                                    //Instanciando Ubicación
                                    using (SAT_CL.Global.Ubicacion ubicacion = new SAT_CL.Global.Ubicacion(objParada.id_ubicacion))
                                    {
                                        //Validando que exista la Ubicación
                                        if (ubicacion.habilitar)
                                        {
                                            //Obteniendo la Ubicación de la Unidad
                                            SqlGeography geoubicacion_unidad = DatosEspaciales.ConvierteLatLongPuntoSqlGeography(latitud, longitud, 4326);

                                            //Obtiene Distancia Permitida
                                            int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                            //Obteniendo Distancia por Ubicación
                                            using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                           SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(
                                                                                    0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                            {
                                                //Validando que exista la Referencia
                                                if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                {
                                                    //Recorriendo Registro
                                                    foreach (DataRow dr in dtDistancia.Rows)

                                                        //Obteniendo Distancia Permitida
                                                        distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                }
                                            }

                                            //Validando Tipo de Geometria
                                            switch (ubicacion.geoubicacion.STGeometryType().Value)
                                            {
                                                case "Point":
                                                    {
                                                        //Validando que el Punto no exceda mas de 10 metros
                                                        if (DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, geoubicacion_unidad, distancia_permitida))

                                                            //Instanciando Resultado Positivo
                                                            result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                        break;
                                                    }
                                                case "LineString":
                                                case "CompoundCurve":
                                                case "Polygon":
                                                case "CurvePolygon":
                                                    {
                                                        //Obtiene Punto mas Cercano
                                                        SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                        //Validando que exista el Punto mas Cercano
                                                        if (punto_cercano != SqlGeography.Null)
                                                        {
                                                            //Validando que haya Intersección en las columnas
                                                            if (DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, geoubicacion_unidad))
                                                            {
                                                                //Instanciando Resultado Positivo
                                                                result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede iniciar el Viaje", true);
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible iniciar el Viaje", false);
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion(-2, "No se logro obtener el Punto mas Cercano", false);
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                            //No se puede Acceder a la Parada
                                            result = new RetornoOperacion("No Existe la Ubicación");
                                    }
                                }
                                else if (objParada.Tipo == Parada.TipoParada.Servicio)

                                    //Instanciando Resultado Positivo
                                    result = new RetornoOperacion(0, "Es una Parada de Servicio", true);
                            }
                            else
                                //No se puede Acceder a la Parada
                                result = new RetornoOperacion("No Existe la Parada");

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Realizando la actualización de la parada
                                result = objParada.ActualizarFechaSalida(Fecha.ObtieneFechaEstandarMexicoCentro(), Parada.TipoActualizacionSalida.APP,
                                                        EstanciaUnidad.TipoActualizacionFin.APP, ParadaEvento.TipoActualizacionFin.Manual, 0, user_sesion.id_usuario);

                                //Validando la Operación
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Parada
                                    int idParada = result.IdRegistro;

                                    //Actualizando Ultima Actividad
                                    result = user_sesion.ActualizaUltimaActividad();

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Servicio
                                        result = new RetornoOperacion(idParada);

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Determinar si el Viaje esta Listo para Iniciarse
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string TerminarViaje(int id_servicio, double latitud, double longitud, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Servicio
                        using (Servicio servicio = new Servicio(id_servicio))
                        {
                            //Validando que exista el Servicio
                            if (servicio.habilitar && servicio.estatus == Servicio.Estatus.Iniciado)
                            {
                                //Instanciando Primer Parada
                                using (Parada objParada = new Parada(Parada.ObtieneUltimaParada(id_servicio)))
                                {
                                    //Validando que exista la Parada
                                    if (objParada.habilitar)
                                    {
                                        //Instanciando Ubicación
                                        using (SAT_CL.Global.Ubicacion ubicacion = new SAT_CL.Global.Ubicacion(objParada.id_ubicacion))
                                        {
                                            //Validando que exista la Ubicación
                                            if (ubicacion.habilitar)
                                            {
                                                //Obteniendo la Ubicación de la Unidad
                                                SqlGeography geoubicacion_unidad = DatosEspaciales.ConvierteLatLongPuntoSqlGeography(latitud, longitud, 4326);

                                                //Obtiene Distancia Permitida
                                                int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                //Obteniendo Distancia por Ubicación
                                                using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                               SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(
                                                                                        0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                {
                                                    //Validando que exista la Referencia
                                                    if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                    {
                                                        //Recorriendo Registro
                                                        foreach (DataRow dr in dtDistancia.Rows)

                                                            //Obteniendo Distancia Permitida
                                                            distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                    }
                                                }

                                                //Validando Tipo de Geometria
                                                switch (ubicacion.geoubicacion.STGeometryType().Value)
                                                {
                                                    case "Point":
                                                        {
                                                            //Validando que el Punto no exceda mas de 10 metros
                                                            if (DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, geoubicacion_unidad, distancia_permitida))

                                                                //Instanciando Resultado Positivo
                                                                result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede terminar el Viaje", true);
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible terminar el Viaje", false);
                                                            break;
                                                        }
                                                    case "LineString":
                                                    case "CompoundCurve":
                                                    case "Polygon":
                                                    case "CurvePolygon":
                                                        {
                                                            //Obtiene Punto mas Cercano
                                                            SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                            //Validando que exista el Punto mas Cercano
                                                            if (punto_cercano != SqlGeography.Null)
                                                            {
                                                                //Validando que haya Intersección en las columnas
                                                                if (DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, geoubicacion_unidad))
                                                                {
                                                                    //Instanciando Resultado Positivo
                                                                    result = new RetornoOperacion(0, "La Unidad se encuentra en la Ubicación, puede terminar el Viaje", true);
                                                                }
                                                                else
                                                                    //Instanciando Excepción
                                                                    result = new RetornoOperacion(-4, "La Unidad no se encuentra en la Ubicación, imposible terminar el Viaje", false);
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                result = new RetornoOperacion(-2, "No se logro obtener el Punto mas Cercano", false);
                                                            break;
                                                        }
                                                }

                                                //Validando Operación Exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Realizando la actualización de la parada
                                                    result = servicio.TerminaServicio(Fecha.ObtieneFechaEstandarMexicoCentro(), Parada.TipoActualizacionSalida.APP, ParadaEvento.TipoActualizacionFin.APP, user_sesion.id_usuario);

                                                    //Validando la Operación
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Obteniendo Parada
                                                        int idServicio = result.IdRegistro;

                                                        //Actualizando Ultima Actividad
                                                        result = user_sesion.ActualizaUltimaActividad();

                                                        //Validando Operación Exitosa
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Instanciando Servicio
                                                            result = new RetornoOperacion(idServicio);

                                                            //Completando Transacción
                                                            trans.Complete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Validando Estatus
                                switch (servicio.estatus)
                                {
                                    case Servicio.Estatus.Documentado:
                                        {
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("El Servicio esta Documentado, no se puede Terminar");
                                            break;
                                        }
                                    case Servicio.Estatus.Terminado:
                                        {
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("El Servicio ya esta Terminado");
                                            break;
                                        }
                                    case Servicio.Estatus.Cancelado:
                                        {
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("El Servicio esta Cancelado, no se puede Terminar");
                                            break;
                                        }
                                }
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Obtener los Documentos del Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public string ObtieneDocumentosViaje(int id_servicio)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("DocumentosViaje"));

            //Obteniendo Datos
            using (DataSet dsDocumentosServicio = SAT_CL.ControlEvidencia.Reportes.ObtieneDocumentosServicio(id_servicio))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsDocumentosServicio))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dsDocumentosServicio.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("DocumentosViaje").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("DocumentosViaje").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener el Evento Actual
        /// </summary>
        /// <param name="id_parada">Parada del Servicio</param>
        /// <returns></returns>
        public string ObtieneEventoActualParada(int id_parada)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("EventoActual"));

            //Obteniendo Parada Actual
            using (DataTable dtEventoActual = SAT_CL.Despacho.Reporte.ObtieneEventoActual(id_parada))
            {
                //Validando que Exista el Evento Actual
                if (Validacion.ValidaOrigenDatos(dtEventoActual))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtEventoActual.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("EventoActual").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("EventoActual").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Iniciar el Evento
        /// </summary>
        /// <param name="id_evento">Evento</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string IniciarEvento(int id_evento, int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Evento
                        using (ParadaEvento evento = new ParadaEvento(id_evento))
                        {
                            //Validando que exista el Evento
                            if (evento.habilitar)
                            {
                                //Iniciando Parada Evento
                                result = evento.EditaParadaEventoEnDespacho(evento.id_tipo_evento, Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                                   ParadaEvento.TipoActualizacionInicio.APP, DateTime.MinValue, ParadaEvento.TipoActualizacionFin.APP,
                                                                   evento.id_motivo_retraso_evento, user_sesion.id_usuario);

                                //Validando la Operación
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Parada
                                    int idEvento = result.IdRegistro;

                                    //Actualizando Ultima Actividad
                                    result = user_sesion.ActualizaUltimaActividad();

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Servicio
                                        result = new RetornoOperacion(idEvento);

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe el Evento");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Terminar el Evento
        /// </summary>
        /// <param name="id_evento">Evento</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string TerminarEvento(int id_evento, int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Evento
                        using (ParadaEvento evento = new ParadaEvento(id_evento))
                        {
                            //Validando que exista el Evento
                            if (evento.habilitar)
                            {
                                //Instanciando Parada
                                using (Parada stop = new Parada(evento.id_parada))
                                {
                                    //Validando si existe la Parada
                                    if (stop.habilitar)
                                    {
                                        /*/Terminando Parada Evento
                                        result = evento.TerminaParadaEvento(Fecha.ObtieneFechaEstandarMexicoCentro(), ParadaEvento.TipoActualizacionFin.APP,
                                                            0, stop.fecha_salida, user_sesion.id_usuario);//*/

                                        //Terminando Parada Evento
                                        result = evento.EditaParadaEventoEnDespacho(evento.id_tipo_evento, evento.inicio_evento, ParadaEvento.TipoActualizacionInicio.APP,
                                                                           Fecha.ObtieneFechaEstandarMexicoCentro(), ParadaEvento.TipoActualizacionFin.APP,
                                                                           evento.id_motivo_retraso_evento, user_sesion.id_usuario);//*/

                                        //Validando la Operación
                                        if (result.OperacionExitosa)
                                        {
                                            //Obteniendo Parada
                                            int idEvento = result.IdRegistro;

                                            //Actualizando Ultima Actividad
                                            result = user_sesion.ActualizaUltimaActividad();

                                            //Validando Operación Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Instanciando Servicio
                                                result = new RetornoOperacion(idEvento);

                                                //Completando Transacción
                                                trans.Complete();
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No existe la Parada");
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe el Evento");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Insertar una Devolución
        /// </summary>
        /// <param name="id_parada">Parada</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string InsertaDevolucion(int id_parada, int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Parada
                        using (Parada stop = new Parada(id_parada))
                        {
                            //Validando que exista la Parada
                            if (stop.habilitar)
                            {
                                //Instanciando Servicio
                                using (Servicio serv = new Servicio(stop.id_servicio))
                                {
                                    //Validando que exista el Servicio
                                    if (serv.habilitar)
                                    {
                                        //Obtiene Movimiento de la Parada Destino
                                        int idMovimiento = Parada.ObtieneMovimientoParadaDestino(stop.id_parada);

                                        //Validando que exista el Movimeinto
                                        if (idMovimiento > 0)
                                        {
                                            //Insertando Devolucion
                                            result = SAT_CL.Despacho.DevolucionFaltante.InsertaDevolucionesFaltantes(serv.id_compania_emisor, 0, serv.id_servicio,
                                                        idMovimiento, stop.id_parada, DevolucionFaltante.TipoDevolucion.Devolucion, DevolucionFaltante.EstatusDevolucion.Registrado,
                                                        Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), "", user_sesion.id_usuario);

                                            //Validando la Operación
                                            if (result.OperacionExitosa)
                                            {
                                                //Obteniendo Parada
                                                int idRegistro = result.IdRegistro;

                                                //Actualizando Ultima Actividad
                                                result = user_sesion.ActualizaUltimaActividad();

                                                //Validando Operación Exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Instanciando Servicio
                                                    result = new RetornoOperacion(idRegistro);

                                                    //Completando Transacción
                                                    trans.Complete();
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No se puede registrar la Devolución si no hay Carga");
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No existe el Servicio");
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe la Parada");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Insertar Referencias del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <param name="id_tipo_referencia">Tipo de Referencia</param>
        /// <param name="valor_referencia">Valor de la Referencia</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string InsertaReferenciaServicio(int id_servicio, int id_tipo_referencia, string valor_referencia,  int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Servicio
                        using (Servicio servicio = new Servicio(id_servicio))
                        {
                            //Validando que exista el Servicio
                            if (servicio.habilitar)
                            {
                                //Insertando la Referencia
                                result = SAT_CL.Global.Referencia.InsertaReferencia(servicio.id_servicio, 1, id_tipo_referencia,
                                         valor_referencia, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), user_sesion.id_usuario, true);

                                //Validando la Operación
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Referencia
                                    int idReferencia = result.IdRegistro;

                                    //Actualizando Ultima Actividad
                                    result = user_sesion.ActualizaUltimaActividad();

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Referencia
                                        result = new RetornoOperacion(idReferencia);

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe el Servicio");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Cargar los Conceptos de los Depositos
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string CargaConceptosDeposito(int id_servicio, int id_sesion)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))

            //Instanciando Usuario Compania
            using (SAT_CL.Seguridad.UsuarioCompania user_company = new SAT_CL.Seguridad.UsuarioCompania(user_sesion.id_usuario, user_sesion.id_compania_emisor_receptor))
            {
                //Consultando catálogo solicitado
                using (DataTable mit = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(21, "-- Seleccione un Concepto", id_servicio, "2", 
                                                      user_company.id_departamento, user_sesion.id_compania_emisor_receptor.ToString()))
                {
                    //Validando que existan registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s);

                            //Obteniendo DataSet en XML
                            XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                            //Añadiendo Elemento al nodo de Publicaciones
                            document.Add(dataTableElement);
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Solicitar el Deposito del Servicio
        /// </summary>
        /// <param name="id_concepto_deposito"></param>
        /// <param name="concepto_restriccion"></param>
        /// <param name="referencia"></param>
        /// <param name="monto"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_sesion"></param>
        /// <returns></returns>
        public string SolicitaDepositoServicio(int id_concepto_deposito, string concepto_restriccion, string referencia, decimal monto, int id_servicio, int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Obteniendo Operador
                        using (SAT_CL.Global.Operador operador = SAT_CL.Global.Operador.ObtieneOperadorUsuario(user_sesion.id_usuario))

                        //Unidad
                        using (SAT_CL.Global.Unidad unidad = SAT_CL.Global.Unidad.ObtieneUnidadOperador(operador.id_operador))

                        //Servicio
                        using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(id_servicio))
                        {
                            //Validando que exista el Operador
                            if (operador.habilitar)
                            {
                                //Validando que exista la Unidad
                                if (unidad.habilitar)
                                {
                                    //Validando que exista el Servicio
                                    if (servicio.habilitar)
                                    {
                                        //Instanciando Asignación
                                        using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Reporte.ObtieneMovimientoAsignacionOperador(operador.id_operador, id_servicio)))
                                        {
                                            //Validando que exista y que no este liquidada
                                            if (mar.habilitar && mar.EstatusMovimientoAsignacion != MovimientoAsignacionRecurso.Estatus.Liquidado)
                                            {
                                                //Registramos Depósito
                                                result = SAT_CL.EgresoServicio.Deposito.InsertaDeposito(user_sesion.id_compania_emisor_receptor, unidad.numero_unidad,
                                                                            servicio.id_cliente_receptor, id_concepto_deposito, 0, 0, referencia.ToUpper(),
                                                                            SAT_CL.EgresoServicio.Deposito.TipoCargo.Depositante, false, Convert.ToInt32(Cadena.RegresaCadenaSeparada(concepto_restriccion, ':', 1)),
                                                                            0, operador.id_operador, 0, id_servicio, mar.id_movimiento_asignacion_recurso, monto, user_sesion.id_usuario);

                                                //Validando la Operación
                                                if (result.OperacionExitosa)
                                                {
                                                    //Obteniendo Parada
                                                    int idDeposito = result.IdRegistro;

                                                    //Actualizando Ultima Actividad
                                                    result = user_sesion.ActualizaUltimaActividad();

                                                    //Validando Operación Exitosa
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Instanciando Servicio
                                                        result = new RetornoOperacion(idDeposito);

                                                        //Completando Transacción
                                                        trans.Complete();
                                                    }
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("El Movimiento esta Liquidado");
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No se puede encontrar el Servicio");
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se puede encontrar la Unidad");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede encontrar el Operador");
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }

        /// <summary>
        /// Método que actualiza la fecha de llegada un servicio despacho
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <param name="noservicio">No servicio</param>
        /// <param name="nombrecompania">Nombre de compania</param>
        /// <param name="ubicacion">Ubicacion</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="secuencia">Comentario</param>
        /// <param name="usuario">Sesión del Usuario que Actualiza el Registro</param>
        ///  <param name="contrasena">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string ActualizaLlegada(string fecha, string no_servicio, string nombre_compania, string ubicacion, double latitud, double longitud, int secuencia, string usuario, string contrasena)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando credenciales usario
            using (Usuario objUsuario = new Usuario(usuario))
            {
                //Validando que la Sesión este Activa
                if (objUsuario.habilitar)
                {

                    result = objUsuario.AutenticaUsuario(contrasena);
                    if (result.OperacionExitosa)
                    {
                        //Determinando a cuantas empresas tiene acceso el usuario autenticado
                        using (DataTable mit = UsuarioCompania.ObtieneCompaniasUsuario(objUsuario.id_usuario))
                        {
                            //Validando el origen de datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                            {
                                //Si sólo existe un registro de resultado
                                if (mit.Rows.Count == 1)
                                {
                                    foreach (DataRow r in mit.Rows)
                                    {
                                        //Inicializando sesión en compañía registrada
                                        // iniciaSesion(Convert.ToInt32(r["IdCompaniaEmisorReceptor"]));
                                        //Insertamos Sesión del Usuario
                                        result = UsuarioSesion.IniciaSesion(objUsuario.id_usuario, Convert.ToInt32(r["IdCompaniaEmisorReceptor"]), UsuarioSesion.TipoDispositivo.ServicioWeb, "0", "0",
                                                                                                objUsuario.id_usuario);
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando la Sesión de Usuario
                                            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(result.IdRegistro))
                                            {
                                                //Declarando Punto Geografico
                                                SqlGeography point;

                                                try
                                                {
                                                    //Validando que exista un Punto Valido
                                                    if (latitud != 0 && longitud != 0)
                                                    {
                                                        //Convirtiendo Punto
                                                        point = SqlGeography.Point(latitud, longitud, 4326);

                                                        //Asignando Operación Exitosa
                                                        result = new RetornoOperacion(0, "", true);
                                                    }
                                                    else
                                                    {
                                                        //Inicializando Punto
                                                        point = SqlGeography.Null;

                                                        //Asignando Operación Exitosa
                                                        //result = new RetornoOperacion("Ubicación no Valida");
                                                        result = new RetornoOperacion(string.Format("TECTOS10004'{0}' Ubicacion no valida ", false));
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    //Inicializando Punto
                                                    point = SqlGeography.Null;
                                                    //Asignando Operación Exitosa
                                                    //result = new RetornoOperacion("No existe la Ubicación: " + e.Message);
                                                    result = new RetornoOperacion(string.Format("TECTOS10004'{0}' Ubicacion no valida " + e.Message, false));
                                                }

                                                //Validando Operación
                                                if (result.OperacionExitosa)
                                                {
                                                    //Declarando la variable datetime
                                                    //DateTime fechaUTC = Convert.ToDateTime(fecha);
                                                    DateTime fechaUTC = DateTime.ParseExact(fecha, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                                    //Validando que exista una Fecha Valida
                                                    if (fechaUTC != DateTime.MinValue)
                                                    {
                                                        //Validando que exista compañia, Instanciando Nombre corto
                                                        using (SAT_CL.Monitoreo.ProveedorWSDiccionario Nombrec = new SAT_CL.Monitoreo.ProveedorWSDiccionario(nombre_compania, user_sesion.id_compania_emisor_receptor))
                                                        {
                                                            //Validando que exista  el identificador
                                                            if (Nombrec.habilitar)
                                                            {
                                                                using (SAT_CL.Global.CompaniaEmisorReceptor Compania = new SAT_CL.Global.CompaniaEmisorReceptor(Nombrec.id_registro))
                                                                    //Validando que exista la compañia
                                                                    if (Compania.habilitar)
                                                                    {
                                                                        //Validando que noservicio
                                                                        using (SAT_CL.Documentacion.Servicio Servicio = new SAT_CL.Documentacion.Servicio(no_servicio, Compania.id_compania_emisor_receptor))
                                                                        {
                                                                            //Validando que exista la noservicio
                                                                            if (Servicio.habilitar)
                                                                            {
                                                                                //Validando descripcion
                                                                                using (SAT_CL.Global.Ubicacion Ubicacion = new SAT_CL.Global.Ubicacion(ubicacion, Compania.id_compania_emisor_receptor))
                                                                                {
                                                                                    //validando que exista la ubicacion
                                                                                    if (Ubicacion.habilitar)
                                                                                    {
                                                                                        //validando que secuencia sea mayor a 2
                                                                                        if (secuencia > 1)
                                                                                        {
                                                                                            //Metodo Actualiza fecha llegada
                                                                                            //Cargando las paradas asociadas al servicio maestro
                                                                                            using (DataTable tblParadas = SAT_CL.Despacho.Parada.CargaParadasParaCopia(Servicio.id_servicio))
                                                                                            {
                                                                                                //Validando existencia de paradas para copia
                                                                                                if (Validacion.ValidaOrigenDatos(tblParadas))
                                                                                                {
                                                                                                    //Seleccionando primer par de paradas del servicio
                                                                                                    List<DataRow> Paradas = (from DataRow dr in tblParadas.AsEnumerable()
                                                                                                                             where dr.Field<decimal>("Secuencia") == Convert.ToDecimal(secuencia)
                                                                                                                             select dr).ToList();
                                                                                                    if (Paradas.Count > 0)
                                                                                                    {
                                                                                                        foreach (DataRow dr in Paradas)
                                                                                                        {
                                                                                                            //Instanciando la parada actual
                                                                                                            //using (SAT_CL.Despacho.Parada objParada = new SAT_CL.Despacho.Parada(Convert.ToInt32(dr["Id"])))
                                                                                                            using (Parada objParada = new Parada(Convert.ToInt32(dr["Id"])))
                                                                                                            {
                                                                                                                //Realizando la actualización de la parada
                                                                                                                //result = objParada.ActualizarFechaLlegada(Convert.ToDateTime(fechaUTC.ToString("dd/MM/yyyy HH:mm")), Parada.TipoActualizacionLlegada.Manual, 0,
                                                                                                                //             EstanciaUnidad.TipoActualizacionInicio.Manual, user_sesion.id_usuario);
                                                                                                                result = objParada.ActualizarFechaLlegada(fechaUTC, Parada.TipoActualizacionLlegada.Manual, 0,
                                                                                                                             EstanciaUnidad.TipoActualizacionInicio.Manual, user_sesion.id_usuario);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                    result = new RetornoOperacion(string.Format("TECTOS10011'{0}'Invalidas las paradas del servicio.", false));
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                            result = new RetornoOperacion(string.Format("TECTOS10010'{0}' Secuencia inválida.", false));
                                                                                    }
                                                                                    else
                                                                                        result = new RetornoOperacion(string.Format("TECTOS10009'{0}' Descripcion ubicacion inválido.", false));
                                                                                }
                                                                            }
                                                                            else
                                                                                result = new RetornoOperacion(string.Format("TECTOS10008'{0}' Folio de servicio inválido.", false));
                                                                        }
                                                                    }
                                                                    else
                                                                        result = new RetornoOperacion(string.Format("TECTOS1007'{0}'El cliente no existe.", false));
                                                            }
                                                            else
                                                                result = new RetornoOperacion(string.Format("TECTOS1006'{0}'El cliente no existe en diccionario.", false));
                                                        }
                                                    }
                                                    else
                                                        result = new RetornoOperacion(string.Format("TECTOS10005'{0}'Fecha y hora inválida.", false));
                                                }

                                            }
                                        }
                                    }
                                }
                                //Si hay posibilidad de más de una compañía
                                else
                                    result = new RetornoOperacion(string.Format("TECTOS10003'{0}'Usario no contiene configuracion establecida", false));
                            }
                        }

                    }
                    //En caso de error
                    else
                        result = new RetornoOperacion(string.Format("TECTOS10002'{0}' Credenciales Invalidas", false));

                }
                else
                    //Instanciando Excepción
                    //result = new RetornoOperacion("La Sesión no esta Activa");
                    result = new RetornoOperacion(string.Format("TECTOS10001'{0}' Credenciales Invalidas", false));
            }
            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método que actualiza la fecha de llegada un servicio despacho
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <param name="noservicio">No servicio</param>
        /// <param name="nombrecompania">Nombre de compania</param>
        /// <param name="ubicacion">Ubicacion</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="secuencia">Comentario</param>
        /// <param name="usuario">Sesión del Usuario que Actualiza el Registro</param>
        ///  <param name="contrasena">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string ActualizaSalida(string fecha, string no_servicio, string nombre_compania, string ubicacion, double latitud, double longitud, int secuencia, string usuario, string contrasena)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando credenciales usario
            using (Usuario objUsuario = new Usuario(usuario))
            {
                //Validando que la Sesión este Activa
                if (objUsuario.habilitar)
                {

                    result = objUsuario.AutenticaUsuario(contrasena);
                    if (result.OperacionExitosa)
                    {
                        //Determinando a cuantas empresas tiene acceso el usuario autenticado
                        using (DataTable mit = UsuarioCompania.ObtieneCompaniasUsuario(objUsuario.id_usuario))
                        {
                            //Validando el origen de datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                            {
                                //Si sólo existe un registro de resultado
                                if (mit.Rows.Count == 1)
                                {
                                    foreach (DataRow r in mit.Rows)
                                    {
                                        //Inicializando sesión en compañía registrada
                                        // iniciaSesion(Convert.ToInt32(r["IdCompaniaEmisorReceptor"]));
                                        //Insertamos Sesión del Usuario
                                        result = UsuarioSesion.IniciaSesion(objUsuario.id_usuario, Convert.ToInt32(r["IdCompaniaEmisorReceptor"]), UsuarioSesion.TipoDispositivo.ServicioWeb, "0", "0",
                                                                                                objUsuario.id_usuario);
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando la Sesión de Usuario
                                            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(result.IdRegistro))
                                            {
                                                //Declarando Punto Geografico
                                                SqlGeography point;

                                                try
                                                {
                                                    //Validando que exista un Punto Valido
                                                    if (latitud != 0 && longitud != 0)
                                                    {
                                                        //Convirtiendo Punto
                                                        point = SqlGeography.Point(latitud, longitud, 4326);

                                                        //Asignando Operación Exitosa
                                                        result = new RetornoOperacion(0, "", true);
                                                    }
                                                    else
                                                    {
                                                        //Inicializando Punto
                                                        point = SqlGeography.Null;

                                                        //Asignando Operación Exitosa
                                                        //result = new RetornoOperacion("Ubicación no Valida");
                                                        result = new RetornoOperacion(string.Format("TECTOS10004'{0}' Ubicacion no valida ", false));
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    //Inicializando Punto
                                                    point = SqlGeography.Null;
                                                    //Asignando Operación Exitosa
                                                    //result = new RetornoOperacion("No existe la Ubicación: " + e.Message);
                                                    result = new RetornoOperacion(string.Format("TECTOS10004'{0}' Ubicacion no valida " + e.Message, false));
                                                }

                                                //Validando Operación
                                                if (result.OperacionExitosa)
                                                {
                                                    //Declarando la variable datetime
                                                    //DateTime fechaUTC = Convert.ToDateTime(fecha);
                                                    DateTime fechaUTC = DateTime.ParseExact(fecha, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                                    //Validando que exista una Fecha Valida
                                                    if (fechaUTC != DateTime.MinValue)
                                                    {
                                                        //Validando que exista compañia, Instanciando Nombre corto
                                                        using (SAT_CL.Monitoreo.ProveedorWSDiccionario Nombrec = new SAT_CL.Monitoreo.ProveedorWSDiccionario(nombre_compania, user_sesion.id_compania_emisor_receptor))
                                                        {
                                                            //Validando que exista  el identificador
                                                            if (Nombrec.habilitar)
                                                            {
                                                                using (SAT_CL.Global.CompaniaEmisorReceptor Compania = new SAT_CL.Global.CompaniaEmisorReceptor(Nombrec.id_registro))
                                                                    //Validando que exista la compañia
                                                                    if (Compania.habilitar)
                                                                    {
                                                                        //Validando que noservicio
                                                                        using (SAT_CL.Documentacion.Servicio Servicio = new SAT_CL.Documentacion.Servicio(no_servicio, Compania.id_compania_emisor_receptor))
                                                                        {
                                                                            //Validando que exista la noservicio
                                                                            if (Servicio.habilitar)
                                                                            {
                                                                                //Validando descripcion
                                                                                using (SAT_CL.Global.Ubicacion Ubicacion = new SAT_CL.Global.Ubicacion(ubicacion, Compania.id_compania_emisor_receptor))
                                                                                {
                                                                                    //validando que exista la ubicacion
                                                                                    if (Ubicacion.habilitar)
                                                                                    {
                                                                                        //validando que secuencia sea mayor a 2
                                                                                        if (secuencia > 0)
                                                                                        {
                                                                                            //Metodo Actualiza fecha llegada
                                                                                            //Cargando las paradas asociadas al servicio maestro
                                                                                            using (DataTable tblParadas = SAT_CL.Despacho.Parada.CargaParadasParaCopia(Servicio.id_servicio))
                                                                                            {
                                                                                                //Validando existencia de paradas para copia
                                                                                                if (Validacion.ValidaOrigenDatos(tblParadas))
                                                                                                {
                                                                                                    //Seleccionando primer par de paradas del servicio
                                                                                                    List<DataRow> Paradas = (from DataRow dr in tblParadas.AsEnumerable()
                                                                                                                             where dr.Field<decimal>("Secuencia") == Convert.ToDecimal(secuencia)
                                                                                                                             select dr).ToList();
                                                                                                    if (Paradas.Count > 0)
                                                                                                    {
                                                                                                        foreach (DataRow dr in Paradas)
                                                                                                        {
                                                                                                            //Instanciando la parada actual
                                                                                                            //using (SAT_CL.Despacho.Parada objParada = new SAT_CL.Despacho.Parada(Convert.ToInt32(dr["Id"])))
                                                                                                            using (Parada objParada = new Parada(Convert.ToInt32(dr["Id"])))
                                                                                                            {
                                                                                                                //Realizando la actualización de la parada
                                                                                                                //result = objParada.ActualizarFechaSalida(Convert.ToDateTime(fechaUTC.ToString("dd/MM/yyyy HH:mm")), Parada.TipoActualizacionSalida.Manual,
                                                                                                                //             EstanciaUnidad.TipoActualizacionFin.Manual, ParadaEvento.TipoActualizacionFin.Manual, user_sesion.id_usuario);
                                                                                                                result = objParada.ActualizarFechaSalida(fechaUTC, Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionFin.Manual, 
                                                                                                                             ParadaEvento.TipoActualizacionFin.Manual, 0, user_sesion.id_usuario);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                    result = new RetornoOperacion(string.Format("TECTOS10011'{0}'Invalidas las paradas del servicio.", false));
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                            result = new RetornoOperacion(string.Format("TECTOS10010'{0}' Secuencia inválida.", false));
                                                                                    }
                                                                                    else
                                                                                        result = new RetornoOperacion(string.Format("TECTOS10009'{0}' Descripción ubicacion inválido.", false));
                                                                                }
                                                                            }
                                                                            else
                                                                                result = new RetornoOperacion(string.Format("TECTOS10008'{0}' Folio de servicio inválido.", false));
                                                                        }
                                                                    }
                                                                    else
                                                                        result = new RetornoOperacion(string.Format("TECTOS1007'{0}'El cliente no existe.", false));
                                                            }
                                                            else
                                                                result = new RetornoOperacion(string.Format("TECTOS1006'{0}'El cliente no existe en diccionario.", false));
                                                        }
                                                    }
                                                    else
                                                        result = new RetornoOperacion(string.Format("TECTOS10005'{0}'Fecha y hora inválida.", false));
                                                }
                                            }
                                        }
                                    }
                                }
                                //Si hay posibilidad de más de una compañía
                                else
                                    result = new RetornoOperacion(string.Format("TECTOS10003'{0}'Usario no contiene configuracion establecida", false));
                            }
                        }

                    }
                    //En caso de error
                    else
                        result = new RetornoOperacion(string.Format("TECTOS10002'{0}' Credenciales Invalidas", false));

                }
                else
                    //Instanciando Excepción
                    //result = new RetornoOperacion("La Sesión no esta Activa");
                    result = new RetornoOperacion(string.Format("TECTOS10001'{0}' Credenciales Invalidas", false));
            }
            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
       }
}
