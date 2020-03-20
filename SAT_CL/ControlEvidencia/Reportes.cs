using System.Data;
using TSDK.Datos;
using TSDK.Base;
using System;
using System.Configuration;
using System.Linq;

namespace SAT_CL.ControlEvidencia
{
    /// <summary>
    /// Clase encargada de Cargar todos los Reportes del Modulo de Control de Evidencias
    /// </summary>
    public static class Reportes
    {
        #region Atributos
        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_reporte_modulo";

        #endregion

        /// <summary>
        /// Carga el reporte de paquetes existentes, con la posibilidad de filtrado por todos los criterios del mismo
        /// </summary>
        /// <param name="id_compania">Id de Compañía</param>
        /// <param name="id_lugar_origen">Id de Origen del envío</param>
        /// <param name="id_lugar_destino">Id de Destino del envío</param>
        /// <param name="id_estatus">Id de estatus</param>
        /// <param name="id_medio_envio">Id de Medio de Envío</param>
        /// <param name="referencia">Referencia de envío</param>
        /// <param name="inicio_salida">Inicio de periodo de fecha de salida</param>
        /// <param name="fin_salida">Fin de periodo de fecha de salida</param>
        /// <param name="inicio_llegada">Inicio de periodo de fecha de llegada</param>
        /// <param name="fin_llegada">Fin de periodo de fecha de llegada</param>
        /// <returns></returns>
        public static DataTable CargaReportePaquetesEnvio(int id_compania, int id_lugar_origen, int id_lugar_destino, int id_estatus, int id_medio_envio, string referencia,
                                            DateTime inicio_salida, DateTime fin_salida, DateTime inicio_llegada, DateTime fin_llegada)
        {   //Declarando Tabla de Retorno
            DataTable dt = null;
            //Armando Objeto de Parametros
            object[] param = { 1, id_compania.ToString(), id_lugar_origen.ToString(), id_lugar_destino.ToString(), id_estatus.ToString(), id_medio_envio.ToString(), referencia, 
                                 Fecha.ConvierteDateTimeString( inicio_salida, "dd/MM/yyyy HH:mm:ss"), 
                                 Fecha.ConvierteDateTimeString( fin_salida, "dd/MM/yyyy HH:mm:ss"), 
                                 Fecha.ConvierteDateTimeString( inicio_llegada, "dd/MM/yyyy HH:mm:ss"), 
                                 Fecha .ConvierteDateTimeString( fin_llegada, "dd/MM/yyyy HH:mm:ss"), 
                                "", "", "", "", "", "", "", "", "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la Tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dt = ds.Tables["Table"];
                //Devolviendo Resultado Obtenido
                return dt;
            }
        }

        /// <summary>
        /// Método Público encargado de generar el Reporte de las Hojas de Instrucción
        /// </summary>
        /// <param name="id_compania">Compania</param>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="id_remitente">Remitente</param>
        /// <param name="id_destinatario">Destinatario</param>
        /// <param name="descripcion">Descripción</param>
        /// <returns></returns>
        public static DataTable CargaReporteHojaInstruccion(int id_compania, int id_cliente, int id_remitente, int id_destinatario, string descripcion)
        {   //Declarando Tabla de Retorno
            DataTable dt = null;
            //Armando Objeto de Parametros
            object[] param = { 2, id_compania.ToString(), id_cliente.ToString(), id_remitente.ToString(), id_destinatario.ToString(), descripcion.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la Tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dt = ds.Tables["Table"];
            }//Devolviendo Resultado Obtenido
            return dt;
        }

        /// <summary>
        /// Método Público encargado de generar el Reporte de los Documentos
        /// </summary>
        /// <param name="no_viaje">Número de Viaje</param>
        /// <param name="id_compania">Compania</param>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="referencia">Referencia de Servicio</param>
        /// <param name="fecha_inicial">Fecha inicial del rango de búsqueda (fecha inicio de viaje)</param>
        /// <param name="fecha_final">Fecha final del rango de búsqueda (fecha inicio de viaje)</param>
        /// <param name="porte">Porte</param>
        /// <returns></returns>
        public static DataSet CargaReporteDocumentos(string no_viaje, int id_compania, int id_cliente, byte id_estatus, string referencia, DateTime fecha_inicial, DateTime fecha_final, string porte)
        {   //Armando Objeto de Parametros
            object[] param = { 3, no_viaje, id_compania, id_cliente, id_estatus, referencia , Fecha.ConvierteDateTimeString(fecha_inicial, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), 
                                 Fecha.ConvierteDateTimeString(fecha_final, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), porte, "", "", "", "", "", "", "", "", "", "", "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds1 = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                //Devolviendo Objeto de Retorno
                return ds1;
        }

        /// <summary>
        /// Método encargado de Obtener los Viajes con control de evidencia sin recepción
        /// </summary>
        /// <param name="no_viaje">Identificador alfanumérico del servicio</param>
        /// <param name="id_compania">Compañia que realizo el  servicio</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="referencia">Referencia del Servicio</param>
        /// <param name="solo_servicios">True para indicar que la busqueda sólo aplicará sobre Servicios y no sobre Movimientos en Vacío. De lo contrario False</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <param name="porte">Porte</param>
        /// <param name="fecha_inicial_cita_carga">Inicio de Carga</param>
        /// <param name="fecha_final_cita_carga">Fin de Carga</param>
        /// <param name="fecha_inicial_cita_descarga">Inicio de Descarga</param>
        /// <param name="fecha_final_cita_descarga">Fin de Descarga</param>
        /// <param name="fecha_inicial_inicio_servicio"></param>
        /// <param name="fecha_final_inicio_servicio"></param>
        /// <param name="fecha_inicial_fin_servicio"></param>
        /// <param name="fecha_final_fin_servicio"></param>
        /// <param name="operacion">Operación Servicio (Clasificación)</param>
        /// <param name="alcance">Alcance Servicio (Clasificación)</param>
        /// <param name="terminal">Terminal (Clasificación)</param>
        /// <returns></returns>
        public static DataTable ObtienePendientesRecepcionEvidencia(string no_viaje, int id_compania, int id_operador, int id_unidad, string referencia, bool solo_servicios, 
                                    int id_cliente, string porte, DateTime fecha_inicial_cita_carga, DateTime fecha_final_cita_carga, DateTime fecha_inicial_cita_descarga, DateTime fecha_final_cita_descarga,
                                    DateTime fecha_inicial_inicio_servicio, DateTime fecha_final_inicio_servicio, DateTime fecha_inicial_fin_servicio, DateTime fecha_final_fin_servicio, 
                                    string operacion, string alcance, string terminal)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;
            //Declarando Arreglo de Parametros
            object[] param = { 4, no_viaje, id_compania, id_operador, id_unidad, referencia, solo_servicios, id_cliente, porte,
                               Fecha.ConvierteDateTimeString(fecha_inicial_cita_carga,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_cita_carga, ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               Fecha.ConvierteDateTimeString(fecha_inicial_cita_descarga,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_cita_descarga,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               Fecha.ConvierteDateTimeString(fecha_inicial_inicio_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_inicio_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               Fecha.ConvierteDateTimeString(fecha_inicial_fin_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_fin_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               operacion, alcance, terminal, "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando si la Tabla contiene registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Recuperando Tabla
                    dt = ds.Tables["Table"];

                //Devolviendo Resultado Obtenido
                return dt;
            }
        }
        /// <summary>
        /// Método encargado de Obtener los Accesorios dado un Servicio
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        public static DataTable ObtieneAccesoriosServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtAccesorios = null;

            //Declarando Arreglo de Parametros
            object[] param = { 5, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando si la Tabla contiene registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Recuperando Tabla
                    dtAccesorios = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtAccesorios;
        }
        /// <summary>
        /// Método encargado de Obtener los Documentos dado un Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataSet ObtieneDocumentosServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtDocucmentos = null;

            //Declarando Arreglo de Parametros
            object[] param = { 6, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))

                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Método encargado de Validar si el Operador no debe Evidencias para asignarlo a algún Viaje
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_operador"></param>
        /// <returns></returns>
        public static RetornoOperacion ValidaViajesSinEvidenciasOperador(int id_compania, int id_operador)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Obteniendo Variable Catalogo
            using (DataTable dtVariableBD = CapaNegocio.m_capaNegocio.RegresaRegistroVariableCatalogoBD("Validación Evidencias (Máx | Fec. Ini)", id_compania))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dtVariableBD))
                {
                    //Recorriendo Resultado
                    foreach (DataRow dr in dtVariableBD.Rows)
                    {
                        //Validando Valor
                        if (Convert.ToInt32(dr["Valor"]) > 0)
                        {
                            //Declarando Arreglo de Parametros
                            object[] param = { 7, "", id_compania, id_operador, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

                            //Obteniendo Resultado del SP
                            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                            {
                                //Validando Datos
                                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                                {
                                    //Obteniendo Total de Viajes
                                    int total_servicios = ds.Tables["Table"].Rows.Count;
                                    decimal documentos = (from DataRow r in ds.Tables["Table"].Rows
                                                          select Convert.ToDecimal(r["Documentos"])).Sum();
                                    
                                    //Validando Limite Máximo Permitido
                                    if (total_servicios > Convert.ToInt32(dr["Valor"]))
                                    
                                        //Instanciando retorno positivo
                                        retorno = new RetornoOperacion(id_compania, string.Format("El Operador tiene '{0}' viajes sin evidencias, de los cuales tiene '{1}' documento(s) por recibir.", total_servicios, documentos), true);
                                    else
                                        //Instanciando retorno positivo
                                        retorno = new RetornoOperacion(string.Format("El Operador tiene '{0}' viajes sin evidencias.", total_servicios));
                                }
                                else
                                    //Instanciando retorno positivo
                                    retorno = new RetornoOperacion("El Operador no tiene viajes sin evidencias.");
                            }
                        }
                        else
                            //Instanciando retorno positivo
                            retorno = new RetornoOperacion("Esta compania no tiene la validación de evidencias habilitada.");

                        //Terminando Ciclo
                        break;
                    }
                }
                else
                    //Instanciando retorno positivo
                    retorno = new RetornoOperacion("Esta compania no tiene configuración de validación de evidencias.");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        public static DataTable ObtieneServiciosPaqueteEnvio(int IdPaquete, int IdCompania)
        {
            //Declarando tabla de retorno
            DataTable dt = null;

            //Armando objeto de parámetros
            object[] param = { 8, IdCompania, IdPaquete, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};

            //Obteniendo el resultado dep SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que la tabla contenga registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dt = ds.Tables["Table"];

                //Devolviendo resultado obtenido
                return dt;
            }
        }

        /// <summary>
        /// Método encargado de Obtener los Accesorios dado un Servicio
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        public static DataTable ObtieneEvidenciasParadas(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtParadas = null;

            //Declarando Arreglo de Parametros
            object[] param = { 9, "" , id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando si la Tabla contiene registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Recuperando Tabla
                    dtParadas = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtParadas;
        }
    }
}
