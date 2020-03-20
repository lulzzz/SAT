using Microsoft.SqlServer.Types;
using SAT_CL.Despacho;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Documentacion;


namespace SAT_WCF.Monitoreo
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Bitacora" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Bitacora.svc o Bitacora.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Bitacora : IBitacora
    {
        /// <summary>
        /// Método encargado de Insertar una Bitacora de Monitoreo
        /// </summary>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string ReportaPeticionMonitoreo(double latitud, double longitud, int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
            {
                //Validando que la Sesión este Activa
                if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
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
                            result = new RetornoOperacion("Ubicación no Valida");
                        }
                    }
                    catch (Exception e)
                    {
                        //Inicializando Punto
                        point = SqlGeography.Null;

                        //Asignando Operación Exitosa
                        result = new RetornoOperacion("No existe la Ubicación: " + e.Message);
                    }

                    //Validando si existe la Ubicación
                    if (point != SqlGeography.Null)
                    {
                        //Validando Operación
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Operador
                            using (Operador op = Operador.ObtieneOperadorUsuario(user_sesion.id_usuario))
                            {
                                //Validando que exista el Operador
                                if (op.habilitar)
                                {
                                    //Instanciando Movimiento y Parada
                                    using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(op.id_movimiento))
                                    using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(op.id_parada))
                                    {
                                        //Validando que exista el Movimiento
                                        if (mov.habilitar)
                                        {
                                            //Obteniendo Unidad
                                            int id_unidad = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionUnidadPropia(mov.id_movimiento);

                                            //Validando que exista la Unidad
                                            if (id_unidad > 0)

                                                //Registramos Bitácora Monitoreo
                                                result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.APP,
                                                           6, mov.id_servicio, 0, 0, mov.id_movimiento, 19, id_unidad, point, "",
                                                           "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0.00M, false, user_sesion.id_usuario);
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("El Operador no tiene Unidad Asignada");
                                        }
                                        else if (stop.habilitar)
                                        {
                                            //Obteniendo Unidad
                                            using (Unidad unidad = SAT_CL.Global.Unidad.ObtieneUnidadOperador(op.id_operador))
                                            {
                                                //Validando que exista la Unidad
                                                if (unidad.habilitar)
                                                {
                                                    //Registramos Bitácora Monitoreo
                                                    result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.APP,
                                                               6, stop.id_servicio, stop.id_parada, 0, 0, 19, unidad.id_unidad, point, "",
                                                               "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0.00M, false, user_sesion.id_usuario);
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("El Operador no tiene Unidad Asignada");
                                            }
                                        }
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existe el Operador");
                            }
                        }
                    }
                    else
                        //Asignando Operación Exitosa
                        result = new RetornoOperacion("No existe la Ubicación");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Sesión no esta Activa");
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Reportar una Notificación del Operador
        /// </summary>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string ReportaNotificacionMonitoreo(double latitud, double longitud, int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
            {
                //Validando que la Sesión este Activa
                if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
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
                            result = new RetornoOperacion("Ubicación no Valida");
                        }
                    }
                    catch (Exception e)
                    {
                        //Inicializando Punto
                        point = SqlGeography.Null;

                        //Asignando Operación Exitosa
                        result = new RetornoOperacion("No existe la Ubicación: " + e.Message);
                    }

                    //Validando si existe la Ubicación
                    if (point != SqlGeography.Null)
                    {
                        //Validando Operación
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Operador
                            using (Operador op = Operador.ObtieneOperadorUsuario(user_sesion.id_usuario))
                            {
                                //Validando que exista el Operador
                                if (op.habilitar)
                                {
                                    //Instanciando Movimiento y Parada
                                    using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(op.id_movimiento))
                                    using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(op.id_parada))
                                    {
                                        //Validando que exista el Movimiento
                                        if (mov.habilitar)
                                        {
                                            //Obteniendo Unidad
                                            int id_unidad = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionUnidadPropia(mov.id_movimiento);

                                            //Validando que exista la Unidad
                                            if (id_unidad > 0)

                                                //Registramos Bitácora Monitoreo
                                                result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.APP,
                                                           7, mov.id_servicio, 0, 0, mov.id_movimiento, 19, id_unidad, point, string.Format("{0}, {1}", point.Lat, point.Long),
                                                           "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0.00M, false, user_sesion.id_usuario);
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("El Operador no tiene Unidad Asignada");
                                        }
                                        else if (stop.habilitar)
                                        {
                                            //Obteniendo Unidad
                                            using (Unidad unidad = SAT_CL.Global.Unidad.ObtieneUnidadOperador(op.id_operador))
                                            {
                                                //Validando que exista la Unidad
                                                if (unidad.habilitar)
                                                {
                                                    //Registramos Bitácora Monitoreo
                                                    result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.APP,
                                                               7, stop.id_servicio, stop.id_parada, 0, 0, 19, unidad.id_unidad, point, string.Format("{0}, {1}", point.Lat, point.Long),
                                                               "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0.00M, false, user_sesion.id_usuario);
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("El Operador no tiene Unidad Asignada");
                                            }
                                        }
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existe el Operador");
                            }
                        }
                    }
                    else
                        //Asignando Operación Exitosa
                        result = new RetornoOperacion("No existe la Ubicación");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Sesión no esta Activa");
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método que inserta las Bitacoras de Monitoreo
        /// </summary>
        /// <param name="tipo">Tipo de Inidencia</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="comentario">Comentario</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string InsertaBitacoraMonitoreo(string tipo, double latitud, double longitud, string comentario, int id_sesion)
        {
            //Declaramos Mensaje
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
            {
                //Validando que la Sesión este Activa
                if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
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
                            result = new RetornoOperacion("Ubicación no Valida");
                        }
                    }
                    catch (Exception e)
                    {
                        //Inicializando Punto
                        point = SqlGeography.Null;

                        //Asignando Operación Exitosa
                        result = new RetornoOperacion("No existe la Ubicación: " + e.Message);
                    }

                    //Validando Operación
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Operador
                        using (Operador op = Operador.ObtieneOperadorUsuario(user_sesion.id_usuario))
                        {
                            //Validando que exista el Operador
                            if (op.habilitar)
                            {
                                //Instanciando Movimiento y Parada
                                using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(op.id_movimiento))
                                using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(op.id_parada))
                                {
                                    //Validando que exista el Movimiento
                                    if (mov.habilitar)
                                    {
                                        //Obteniendo Unidad
                                        int id_unidad = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionUnidadPropia(mov.id_movimiento);

                                        //Validando que exista la Unidad
                                        if (id_unidad > 0)

                                            //Registramos Bitácora Monitoreo
                                            result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.APP,
                                                       Convert.ToByte(tipo), mov.id_servicio, 0, 0, mov.id_movimiento, 19, id_unidad, point, "",
                                                       comentario, Fecha.ObtieneFechaEstandarMexicoCentro(), 0.00M, false, user_sesion.id_usuario);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("El Operador no tiene Unidad Asignada");
                                    }
                                    else if (stop.habilitar)
                                    {
                                        //Obteniendo Unidad
                                        using (Unidad unidad = SAT_CL.Global.Unidad.ObtieneUnidadOperador(op.id_operador))
                                        {
                                            //Validando que exista la Unidad
                                            if (unidad.habilitar)
                                            {
                                                //Registramos Bitácora Monitoreo
                                                result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.APP,
                                                           Convert.ToByte(tipo), stop.id_servicio, stop.id_parada, 0, 0, 19, unidad.id_unidad, point, "",
                                                           comentario, Fecha.ObtieneFechaEstandarMexicoCentro(), 0.00M, false, user_sesion.id_usuario);
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("El Operador no tiene Unidad Asignada");
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe el Operador");
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Sesión no esta Activa");
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de Incidencias
        /// </summary>
        /// <param name="id_sesion">Sesión del Usuario que solicita información</param>
        /// <returns></returns>
        public string ObtieneReporteIncidencias(int id_sesion)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
            {
                //Validando que la Sesión este Activa
                if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                {
                    //Instanciando Unidad
                    using (Unidad unidad = Unidad.ObtieneUnidadOperador(Operador.ObtieneOperadorUsuario(user_sesion.id_usuario).id_operador))
                    {
                        //Obteniendo Reporte
                        using (DataTable dtReporte = SAT_CL.Monitoreo.BitacoraMonitoreo.CargaReporteBitacoraMonitoreo(19, unidad.id_unidad))//*/
                        {
                            //Validando que existan Registros
                            if (Validacion.ValidaOrigenDatos(dtReporte))
                            {
                                //Creando flujo de memoria
                                using (System.IO.Stream s = new System.IO.MemoryStream())
                                {
                                    //Leyendo flujo de datos XML
                                    dtReporte.WriteXml(s, XmlWriteMode.WriteSchema);

                                    //Obteniendo DataSet en XML
                                    XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                                    //Añadiendo Elemento al nodo de Publicaciones
                                    document.Add(dataTableElement);
                                }
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    document.Add(XElement.Parse(new RetornoOperacion("La Sesión ha expirado").ToXMLString()));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }

        /// <summary>
        /// Método que actualiza la fecha de llegada un servicio despacho
        /// </summary>
        /// <param name="tipo_incidencia">Tipo incidencia</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="noservicio">No servicio</param>
        /// <param name="nombrecompania">Nombre de compania</param>
        /// <param name="ubicacion">Ubicacion</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="unidad">Comentario</param>
        /// <param name="usuario">Sesión del Usuario que Actualiza el Registro</param>
        /// <param name="contrasena">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string ActualizaIncidencia(string tipo_incidencia, string fecha, string no_servicio, string nombre_compania, string ubicacion, string unidad, int secuencia, double latitud, double longitud, string usuario, string contrasena)
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
                                                        result = new RetornoOperacion(string.Format("TECTOS10004'{0}' Ubicación  no valida ", false));
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    //Inicializando Punto
                                                    point = SqlGeography.Null;
                                                    //Asignando Operación Exitosa
                                                    //result = new RetornoOperacion("No existe la Ubicación: " + e.Message);
                                                    result = new RetornoOperacion(string.Format("TECTOS10004'{0}' Ubicación  no valida " + e.Message, false));
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
                                                        //Validando que exista unidad, Instanciando unidad
                                                        using (SAT_CL.Monitoreo.ProveedorWSDiccionario Nombreu = new SAT_CL.Monitoreo.ProveedorWSDiccionario(unidad, user_sesion.id_compania_emisor_receptor))
                                                        {
                                                            //Validando que exista unidad en diccionario
                                                            if (Nombreu.habilitar)
                                                            {
                                                                using (SAT_CL.Global.Unidad Unidad = new SAT_CL.Global.Unidad(Nombreu.id_registro))
                                                                    //Validando que exista la compañia
                                                                    if (Unidad.habilitar)
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
                                                                                                        //Recuperando tipo de incidencia "Por Definir"
                                                                                                        //int tipoincidencia = Convert.ToInt32(SAT_CL.Global.Catalogo.RegresaValorCadenaValor(3138, tipo_incidencia));
                                                                                                        int tipoincidencia;
                                                                                                        bool success = Int32.TryParse(SAT_CL.Global.Catalogo.RegresaValorCadenaValor(3138, tipo_incidencia), out tipoincidencia);
                                                                                                        if (success == true)
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
                                                                                                                        List<DataRow> ParadasTotal = (from DataRow dr in tblParadas.AsEnumerable()
                                                                                                                                                // where dr.Field<decimal>("Secuencia") == Convert.ToDecimal(secuencia)
                                                                                                                                                 select dr).ToList();
                                                                                                                        if (Paradas.Count > 0)
                                                                                                                        {
                                                                                                                            foreach (DataRow dr in Paradas)
                                                                                                                            {
                                                                                                                                //Validar si es el ultima parada
                                                                                                                                int secuenciamovimiento = 0;
                                                                                                                                if (ParadasTotal.Count == secuencia)
                                                                                                                                    secuenciamovimiento = (secuencia - 1);
                                                                                                                                else
                                                                                                                                    secuenciamovimiento = secuencia;                                                                                                                                    
                                                                                                                                using (DataTable tblMovimiento = SAT_CL.Despacho.Parada.CargaParadasParaVisualizacionDespacho(Servicio.id_servicio))
                                                                                                                                {
                                                                                                                                    //Validando existencia de paradas para copia
                                                                                                                                    if (Validacion.ValidaOrigenDatos(tblMovimiento))
                                                                                                                                    {
                                                                                                                                        //Seleccionando primer par de paradas del servicio
                                                                                                                                        List<DataRow> Movimiento = (from DataRow mr in tblMovimiento.AsEnumerable()
                                                                                                                                                                 where mr.Field<decimal>("Secuencia") == Convert.ToDecimal(secuenciamovimiento)
                                                                                                                                                                 select mr).ToList();
                                                                                                                                        if (Movimiento.Count > 0)
                                                                                                                                        {
                                                                                                                                            foreach (DataRow mr in Movimiento)
                                                                                                                                            {
                                                                                                                                                result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.APP,
                                                                                                                                                Convert.ToByte(tipoincidencia), Servicio.id_servicio, Convert.ToInt32(dr["Id"]), Convert.ToInt32(dr["IdEvento"]), Convert.ToInt32(mr["IdMovimiento"]), 19, Unidad.id_unidad, point, Convert.ToString(dr["Descripcion"]),
                                                                                                                                                "", fechaUTC, 0.00M, false, user_sesion.id_usuario);
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }                                                                                                                                  
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                        result = new RetornoOperacion(string.Format("TECTOS10011'{0}'Invalidas las paradas del servicio", false));
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                                result = new RetornoOperacion(string.Format("TECTOS10010'{0}' Secuencia inválida.", false));

                                                                                                        }
                                                                                                        else
                                                                                                            result = new RetornoOperacion(string.Format("TECTOS10014'{0}' Tipo de incidencia inválido.", false));
                                                                                                    }
                                                                                                    else
                                                                                                        result = new RetornoOperacion(string.Format("TECTOS10009'{0}' Descripción ubicación inválido.", false));
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                                result = new RetornoOperacion(string.Format("TECTOS10008'{0}' Folio de servicio inválido.", false));
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        result = new RetornoOperacion(string.Format("TECTOS10007'{0}'El cliente no existe.", false));
                                                                            }
                                                                            else
                                                                                result = new RetornoOperacion(string.Format("TECTOS10006'{0}'El cliente no existe en diccionario.", false));
                                                                        }
                                                                    }
                                                                    else
                                                                        result = new RetornoOperacion(string.Format("TECTOS10013'{0}'La unidad no existe.", false));
                                                            }
                                                            else
                                                            result = new RetornoOperacion(string.Format("TECTOS10012'{0}'La unidad no existe en diccionario.", false));
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
                                    result = new RetornoOperacion(string.Format("TECTOS10003'{0}'Usuario no contiene configuración establecida", false));
                            }
                        }

                    }
                    //En caso de error
                    else
                        result = new RetornoOperacion(string.Format("TECTOS10002'{0}' Password Inválido", false));
                }
                else
                    //Instanciando Excepción
                    //result = new RetornoOperacion("La Sesión no esta Activa");
                    result = new RetornoOperacion(string.Format("TECTOS10001'{0}' Usuario inválido", false));
            }
            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
    }
}
