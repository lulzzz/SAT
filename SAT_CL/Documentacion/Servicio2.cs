using Microsoft.Reporting.WebForms;
using SAT_CL.Global;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Documentacion
{
    /// <summary>
    /// Proporciona los medios para la adminsitración de Servicios
    /// </summary>
    public partial class Servicio
    {
        #region Métodos de Interfaz a Plataformas de Terceros

        /// <summary>
        /// Recupera información de interés del servicio y la envía en un archivo a un servidor FTP especificado
        /// </summary>
        /// <param name="secuencia">Secuencia de notificación del servicio</param>
        /// <param name="codificacion">Codificación del archivo</param>
        /// <returns></returns>
        private RetornoOperacion enviaInformacionServicioFTP(int secuencia, System.Text.Encoding codificacion)
        {
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);

            //Obteniendo Proveedores FTP
            using (DataTable dtProveedoresFTP = SAT_CL.Monitoreo.ProveedorWS.ObtieneProveedoresFTP(this._id_compania_emisor))
            {
                //Validando Proveedores
                if (Validacion.ValidaOrigenDatos(dtProveedoresFTP))
                {
                    //Recorriendo Datos
                    foreach (DataRow dr in dtProveedoresFTP.Rows)
                    {
                        //Validando la Compania
                        if (this._id_compania_emisor == 76)
                        {
                            switch (dr["Identificador"].ToString())
                            {
                                case "TEM - Omnitracs FTP":
                                    {
                                        //Realizando la recuperación de información
                                        using (DataTable mit = Reportes.CargaInformacionServicioOmnitracs(this._id_servicio, Convert.ToInt32(dr["Id"])))
                                        {
                                            //Si hay elementos que utilizar
                                            if (mit != null)
                                            {
                                                //Instanciando compañía a la que pertenece el servicio
                                                using (CompaniaEmisorReceptor comp = new CompaniaEmisorReceptor(this._id_compania_emisor))
                                                {
                                                    //Definiendo variables auxiliares
                                                    string no_viaje = "";
                                                    byte[] bytesArchivo = null;

                                                    //Creando archivo en memoria
                                                    using (MemoryStream archivo = new MemoryStream())
                                                    {
                                                        //Creando escritos de texto en flujo
                                                        StreamWriter escritor = new StreamWriter(archivo, codificacion);

                                                        //Añadiendo encabezado
                                                        escritor.Write("OPERACION|FECHA VIAJE|DT|PI ORIGEN|CITA LLEGADA ORIGEN|PI DESTINO|CITA LLEGADA DESTINO|SHIPPER|RUTA MAESTRA|EJE TRANSITO|NIVEL SEGURIDAD|SCAC|OPERADOR|VEHICULO|PLATPORTABLE1|PORTABLE1|TIPO REMOLQUE|REMOLQUE1|PLATPORTABLE2|PORTABLE2|REMOLQUE2|PLATPORTABLE3|PORTABLE3|SHIPPER2|DESCRIPCION|EVENTO LOG1|EVENTO LOG2");

                                                        //Dando el formato solicitado para el contenido del archivo
                                                        foreach (DataRow r in mit.Rows)
                                                        {
                                                            //Extrayendo número de viaje
                                                            no_viaje = Cadena.TruncaCadena(string.Format("{0}{1}", r["DT"].ToString(), secuencia > 0 ? "-" + secuencia.ToString() : ""), 15, "");

                                                            //Nueva linea de texto
                                                            string linea = string.Format("{0}|{1}|NS{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}",
                                                                                        Cadena.TruncaCadena(r["Operacion"].ToString(), 3, ""), r.Field<DateTime>("FechaViaje").ToString("dd/MM/yyyy HH:mm"), no_viaje,
                                                                                        Cadena.TruncaCadena(r["PiOrigen"].ToString(), 31, ""), r.Field<DateTime>("CitaLlegadaOrigen").ToString("dd/MM/yyyy HH:mm"), Cadena.TruncaCadena(r["PiDestino"].ToString(), 31, ""),
                                                                                        r.Field<DateTime>("CitaLlegadaDestino").ToString("dd/MM/yyyy HH:mm"), Cadena.TruncaCadena(r["Shipper"].ToString(), 30, ""), Cadena.TruncaCadena(r["RutaMaestra"].ToString(), 30, ""),
                                                                                        Cadena.TruncaCadena(r["EjeTransito"].ToString(), 30, ""), Cadena.TruncaCadena(r["NivelSeguridad"].ToString(), 8, ""), Cadena.TruncaCadena(r["SCAC"].ToString(), 4, ""),
                                                                                        Cadena.TruncaCadena(r["Operador"].ToString(), 40, ""), Cadena.TruncaCadena(r["Vehiculo"].ToString(), 30, ""), Cadena.TruncaCadena(r["PlatPortable1"].ToString(), 10, ""),
                                                                                        Cadena.TruncaCadena(r["Portable1"].ToString(), 15, ""), Cadena.TruncaCadena(r["TipoRemolque"].ToString(), 30, ""), Cadena.TruncaCadena(r["Remolque1"].ToString(), 30, ""),
                                                                                        Cadena.TruncaCadena(r["PlatPortable2"].ToString(), 10, ""), Cadena.TruncaCadena(r["Portable2"].ToString(), 15, ""), Cadena.TruncaCadena(r["Remolque2"].ToString(), 30, ""),
                                                                                        Cadena.TruncaCadena(r["PlatPortable3"].ToString(), 10, ""), Cadena.TruncaCadena(r["Portable3"].ToString(), 15, ""), Cadena.TruncaCadena(r["Shipper2"].ToString(), 30, ""),
                                                                                        Cadena.TruncaCadena(r["Descripcion"].ToString(), 30, ""), Cadena.TruncaCadena(r["EventoLog1"].ToString(), 15, ""), Cadena.TruncaCadena(r["EventoLog2"].ToString(), 15, ""));

                                                            //Añadiendo linea creada
                                                            if (!string.IsNullOrEmpty(linea))
                                                                escritor.Write(Environment.NewLine + linea);
                                                        }

                                                        //Confirmando cambios en flujo y liberando recursos de escritor
                                                        escritor.Flush();

                                                        //Obteniendo arreglo de bytes del flujo de archivo
                                                        bytesArchivo = TSDK.Base.Flujo.ConvierteFlujoABytes(archivo);
                                                    }

                                                    //Recuperando datos de autenticación en servidor FTP
                                                    string servidorFTP = dr["Endpoint"].ToString();
                                                    string usuarioFTP = dr["Usuario"].ToString();
                                                    string contrasenaFTP = dr["Contraseña"].ToString();

                                                    try
                                                    {
                                                        //Definiendo nombre de archivo
                                                        string nombre_archivo = string.Format("{0}-DT{1}_{2:ddMMyyyy_HHmmss}.csv", comp.nombre_corto, no_viaje, Fecha.ObtieneFechaEstandarMexicoCentro());
                                                        //Creando peticioón FTP
                                                        FtpWebRequest peticionFTP = FTP.CreaPeticionFTP(string.Format("{0}/{1}", servidorFTP, nombre_archivo), WebRequestMethods.Ftp.UploadFile, usuarioFTP, contrasenaFTP);
                                                        //Dimensionando archivo por transferir en la petición
                                                        peticionFTP.ContentLength = bytesArchivo.Length;

                                                        //Recuperando flujo de petición ftp
                                                        Stream flujoPeticionFTP = peticionFTP.GetRequestStream();
                                                        //Añadiendo bytes del archivo creado a flujo de petición (escribiendo)
                                                        flujoPeticionFTP.Write(bytesArchivo, 0, bytesArchivo.Length);
                                                        //Cerrando flujo de escritura de archivo
                                                        flujoPeticionFTP.Close();
                                                    }
                                                    //Si no hubo petición devuelta
                                                    catch (NullReferenceException)
                                                    {
                                                        //En caso de error
                                                        resultado = new RetornoOperacion(string.Format("Error al crear petición al servidor FTP '{0}'.", servidorFTP));

                                                        using (EventLog eventLog = new EventLog("Application"))
                                                        {
                                                            eventLog.Source = "Application";
                                                            eventLog.WriteEntry(resultado.Mensaje, EventLogEntryType.Information, 101, 1);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //En caso de error
                                                        resultado = new RetornoOperacion(ex.Message);

                                                        using (EventLog eventLog = new EventLog("Application"))
                                                        {
                                                            eventLog.Source = "Application";
                                                            eventLog.WriteEntry(resultado.Mensaje, EventLogEntryType.Information, 101, 1);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                resultado = new RetornoOperacion("No pudo ser recuperada la información del servicio para su envío.");
                                        }
                                        break;
                                    }
                                case "TEM - Unicomm FTP":
                                    {
                                        //Realizando la recuperación de información
                                        using (DataTable mit = Reportes.CargaInformacionServicioOmnitracs(this._id_servicio, Convert.ToInt32(dr["Id"])))
                                        {
                                            //Si hay elementos que utilizar
                                            if (mit != null)
                                            {
                                                //Definiendo variables auxiliares
                                                string no_viaje = "";
                                                byte[] bytesArchivo = Excel.BytesDataTableExcel(mit, "Reporte UNICOMM FTP", new string[] { "FechaViaje", "CitaLlegadaDestino", "CitaLlegadaOrigen" });
                                                //Recuperando datos de autenticación en servidor FTP
                                                string servidorFTP = dr["Endpoint"].ToString();
                                                string usuarioFTP = dr["Usuario"].ToString();
                                                string contrasenaFTP = dr["Contraseña"].ToString();

                                                //Instanciando compañía a la que pertenece el servicio
                                                using (CompaniaEmisorReceptor comp = new CompaniaEmisorReceptor(this._id_compania_emisor))
                                                {

                                                    try
                                                    {
                                                        //Definiendo nombre de archivo
                                                        string nombre_archivo = string.Format("{0}-DT{1}_{2:ddMMyyyy_HHmmss}.xlsx", comp.nombre_corto, no_viaje, Fecha.ObtieneFechaEstandarMexicoCentro());
                                                        //Creando peticioón FTP
                                                        FtpWebRequest peticionFTP = FTP.CreaPeticionFTP(string.Format("{0}/{1}", servidorFTP, nombre_archivo), WebRequestMethods.Ftp.UploadFile, usuarioFTP, contrasenaFTP);
                                                        //Dimensionando archivo por transferir en la petición
                                                        peticionFTP.ContentLength = bytesArchivo.Length;
                                                        //Recuperando flujo de petición ftp
                                                        Stream flujoPeticionFTP = peticionFTP.GetRequestStream();
                                                        //Añadiendo bytes del archivo creado a flujo de petición (escribiendo)
                                                        flujoPeticionFTP.Write(bytesArchivo, 0, bytesArchivo.Length);
                                                        //Cerrando flujo de escritura de archivo
                                                        flujoPeticionFTP.Close();
                                                        //Guardamos el Archivo en la Ruta Especifica
                                                        Archivo.GuardaArchivoCreandoRuta(bytesArchivo, string.Format(@"{0}UnicommTEM\{1}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), nombre_archivo), false);
                                                    }
                                                    //Si no hubo petición devuelta
                                                    catch (NullReferenceException)
                                                    {
                                                        //En caso de error
                                                        resultado = new RetornoOperacion(string.Format("Error al crear petición al servidor FTP '{0}'.", servidorFTP));

                                                        using (EventLog eventLog = new EventLog("Application"))
                                                        {
                                                            eventLog.Source = "Application";
                                                            eventLog.WriteEntry(resultado.Mensaje, EventLogEntryType.Information, 101, 1);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //En caso de error
                                                        resultado = new RetornoOperacion(ex.Message);

                                                        using (EventLog eventLog = new EventLog("Application"))
                                                        {
                                                            eventLog.Source = "Application";
                                                            eventLog.WriteEntry(resultado.Mensaje, EventLogEntryType.Information, 101, 1);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza actualizaciones en plataformas de terceros sin afectar el flujo operativo de la aplicación
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento de interés</param>
        /// <param name="id_usuario">Id de Usuario</param>
        public RetornoOperacion ActualizaPlataformaTerceros(int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Asignando id de servicio
            resultado = new RetornoOperacion(this._id_servicio);

            //Si se requiere reporte a OMNITRACS (Ref. Servidor FTP)
            if (Referencia.CargaReferencia(this._id_compania_emisor, 25, ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 25, "Servidor", 0, "FTP Omnitracs")) != "")
            {

                //Recuperando referencia de secuencia de notificación a proveedor satelital
                using (Referencia rs = new Referencia(Referencia.CargaRegistroReferencia("0", 1, this._id_servicio, "Rastreo Satelital", "Notificación Proveedor Satelital")))
                {
                    //Reportando actualización de servicio
                    resultado = enviaInformacionServicioFTP(Convert.ToInt32(Cadena.RegresaElementoNoVacio(rs.valor, "0")), System.Text.Encoding.UTF8);

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Si ya existia un registro de secuencia de notificación
                        if (rs.id_referencia > 0)
                            //Actualizando su valor
                            resultado = Referencia.EditaReferencia(rs.id_referencia, (Convert.ToInt32(rs.valor) + 1).ToString(), id_usuario);
                        //Si no existe
                        else
                            resultado = Referencia.InsertaReferencia(this._id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Notificación Proveedor Satelital", 0, "Rastreo Satelital"), "1", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                    }
                }
            }
        

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Método para descargar Gastos generales de un servicio
        public static byte[] GeneraPDFGastosGenerales(int IdServicio)
        {
            //Creando nuevo visor de reporte
            ReportViewer rvReporte = new Microsoft.Reporting.WebForms.ReportViewer();
            
            //Obtenemos el registro completo del servicio proporcionado
            using (SAT_CL.Documentacion.Servicio Viaje = new SAT_CL.Documentacion.Servicio(IdServicio))
            {
                //Obtenemos los datos de la compañía emisora
                using (SAT_CL.Global.CompaniaEmisorReceptor Compania = new SAT_CL.Global.CompaniaEmisorReceptor(Viaje.id_compania_emisor))
                {
                    //Declaramos la ubicación reporte
                    rvReporte.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/RDLC/GastosGenerales.rdlc");
                    //Habilita la consulta de imagenes externas
                    rvReporte.LocalReport.EnableExternalImages = true;
                    //Limpia el reporte
                    rvReporte.LocalReport.DataSources.Clear();
                    //Declarando arreglo auxiliar
                    byte[] logo = null;
                    //Declarando la tabla para almacenar al logo
                    using (DataTable dtLogo = new DataTable())
                    {
                        //Añadiendo la única columna
                        dtLogo.Columns.Add("Logotipo", typeof(byte[]));
                        try { logo = System.IO.File.ReadAllBytes(Compania.ruta_logotipo); }
                        catch { logo = null; }

                        //Agregando imagen
                        dtLogo.Rows.Add(logo);

                        //Agregamos al origen de datos
                        ReportDataSource rdsLogo = new ReportDataSource("Logotipo", dtLogo);
                        rvReporte.LocalReport.DataSources.Add(rdsLogo);
                    }

                    //Creación de variables que almacenan los datos consultados de un registro
                    ReportParameter NoServicio = new ReportParameter("NoServicio", Viaje.no_servicio);
                    //Asignamos al reporte las variables creadas
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { NoServicio });
                    //Cargando todos los gastos generales del servicio especificado
                    using (DataSet DSGastosGenerales = SAT_CL.EgresoServicio.Reportes.CargaGastosGenerales(IdServicio))
                    {
                        //Valida que existan los registros del DataSet
                        if (Validacion.ValidaOrigenDatos(DSGastosGenerales))
                        {
                            //Separamos las tablas obtenidas del DataSet
                            DataTable MitCasetas = DSGastosGenerales.Tables[0];
                            DataTable MitConceptos = DSGastosGenerales.Tables[1];
                            DataTable MitDiesel = DSGastosGenerales.Tables[2];

                            if (MitCasetas.Rows.Count > 0)
                            {
                                //Declarando tabla
                                using (DataTable dtCasetas = new DataTable())
                                {
                                    dtCasetas.Columns.Add("Ruta", typeof(string));
                                    dtCasetas.Columns.Add("Caseta", typeof(string));
                                    dtCasetas.Columns.Add("TipoCaseta", typeof(string));
                                    dtCasetas.Columns.Add("RedCarretera", typeof(string));
                                    dtCasetas.Columns.Add("IAVE", typeof(string));
                                    dtCasetas.Columns.Add("Ejes", typeof(int));
                                    dtCasetas.Columns.Add("MontoIAVE", typeof(decimal));
                                    dtCasetas.Columns.Add("MontoEfectivo", typeof(decimal));
                                    dtCasetas.Columns.Add("Deposito", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    foreach (DataRow r in MitCasetas.Rows)
                                    {
                                        dtCasetas.Rows.Add(r.ItemArray);
                                    }

                                    ReportDataSource rdsCasetas = new ReportDataSource("Casetas", dtCasetas);
                                    rvReporte.LocalReport.DataSources.Add(rdsCasetas);
                                }
                            }
                            else
                            {
                                //Declarando tabla
                                using (DataTable dtCasetas = new DataTable())
                                {
                                    dtCasetas.Columns.Add("Ruta", typeof(string));
                                    dtCasetas.Columns.Add("Caseta", typeof(string));
                                    dtCasetas.Columns.Add("TipoCaseta", typeof(string));
                                    dtCasetas.Columns.Add("RedCarretera", typeof(string));
                                    dtCasetas.Columns.Add("IAVE", typeof(string));
                                    dtCasetas.Columns.Add("Ejes", typeof(int));
                                    dtCasetas.Columns.Add("MontoIAVE", typeof(decimal));
                                    dtCasetas.Columns.Add("MontoEfectivo", typeof(decimal));
                                    dtCasetas.Columns.Add("Deposito", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    dtCasetas.Rows.Clear();

                                    ReportDataSource rdsCasetas = new ReportDataSource("Casetas", dtCasetas);
                                    rvReporte.LocalReport.DataSources.Add(rdsCasetas);
                                }
                            }

                            if (MitConceptos.Rows.Count > 0)
                            {
                                //Declarando la tabla
                                using (DataTable dtConceptos = new DataTable())
                                {
                                    dtConceptos.Columns.Add("Id", typeof(string));
                                    dtConceptos.Columns.Add("Concepto", typeof(string));
                                    dtConceptos.Columns.Add("Cantidad", typeof(int));
                                    dtConceptos.Columns.Add("Precio", typeof(decimal));
                                    dtConceptos.Columns.Add("Monto", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    foreach (DataRow r in MitConceptos.Rows)
                                    {
                                        dtConceptos.Rows.Add(r.ItemArray);
                                    }

                                    ReportDataSource rdsConceptos = new ReportDataSource("Conceptos", dtConceptos);
                                    rvReporte.LocalReport.DataSources.Add(rdsConceptos);
                                }
                            }
                            else
                            {
                                //Declarando la tabla
                                using (DataTable dtConceptos = new DataTable())
                                {
                                    dtConceptos.Columns.Add("Id", typeof(string));
                                    dtConceptos.Columns.Add("Concepto", typeof(string));
                                    dtConceptos.Columns.Add("Cantidad", typeof(int));
                                    dtConceptos.Columns.Add("Precio", typeof(decimal));
                                    dtConceptos.Columns.Add("Monto", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    dtConceptos.Rows.Clear();

                                    ReportDataSource rdsConceptos = new ReportDataSource("Conceptos", dtConceptos);
                                    rvReporte.LocalReport.DataSources.Add(rdsConceptos);
                                }
                            }

                            if (MitDiesel.Rows.Count > 0)
                            {
                                //Declarando la tabla
                                using (DataTable dtDiesel = new DataTable())
                                {
                                    dtDiesel.Columns.Add("Id", typeof(string));
                                    dtDiesel.Columns.Add("Concepto", typeof(string));
                                    dtDiesel.Columns.Add("Cantidad", typeof(int));
                                    dtDiesel.Columns.Add("Precio", typeof(decimal));
                                    dtDiesel.Columns.Add("Monto", typeof(decimal));
                                    dtDiesel.Columns.Add("EstacionCombustible", typeof(string));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    foreach (DataRow r in MitDiesel.Rows)
                                    {
                                        dtDiesel.Rows.Add(r.ItemArray);
                                    }

                                    ReportDataSource rdsDiesel = new ReportDataSource("Diesel", dtDiesel);
                                    rvReporte.LocalReport.DataSources.Add(rdsDiesel);
                                }
                            }
                            else
                            {
                                //Declarando la tabla
                                using (DataTable dtDiesel = new DataTable())
                                {
                                    dtDiesel.Columns.Add("Id", typeof(string));
                                    dtDiesel.Columns.Add("Concepto", typeof(string));
                                    dtDiesel.Columns.Add("Cantidad", typeof(int));
                                    dtDiesel.Columns.Add("Precio", typeof(decimal));
                                    dtDiesel.Columns.Add("Monto", typeof(decimal));
                                    dtDiesel.Columns.Add("EstacionCombustible", typeof(string));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    dtDiesel.Rows.Clear();

                                    ReportDataSource rdsDiesel = new ReportDataSource("Diesel", dtDiesel);
                                    rvReporte.LocalReport.DataSources.Add(rdsDiesel);
                                }
                            }
                            //Devolviendo resultado
                            return rvReporte.LocalReport.Render("PDF");
                        }
                        else
                            return null;
                    }
                }
            }
            //SAT_CL.Documentacion.Servicio Serv = new SAT_CL.Documentacion.Servicio(IdServicio);
            //Generando flujo del reporte 
            //byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            //TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("Gastos_generales_servicio_{0}.pdf", Serv.no_servicio), TSDK.Base.Archivo.ContentType.application_PDF);
            
            
        }
        #endregion
    }
}
