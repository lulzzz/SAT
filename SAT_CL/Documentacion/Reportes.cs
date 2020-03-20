using System.Data;
using System;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;
using System.Collections.Generic;

namespace SAT_CL.Documentacion
{
    /// <summary>
    /// Genera una clase para Reportes
    /// </summary>
    public static class Reportes
    {
        #region Nombre stored procedure

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string nombre_store_procedure = "documentacion.sp_reporte_modulo";

        #endregion

        #region Metodos estaticos

        /// <summary>
        /// Realiza la búsqueda de servicios coincidentes con los criterios establecidos
        /// </summary>
        /// <param name="id_compania">Id de Compañía</param>
        /// <param name="no_servicio">Número de Servicio</param>
        /// <param name="id_estatus">Id de Estatus de Servicio</param>
        /// <param name="id_ubicacion_carga">Id de Ubicación de Carga</param>
        /// <param name="id_ubicacion_descarga">Id de Ubicación de Descarga</param>
        /// <param name="id_cliente">Id de Cliente</param>
        /// <param name="porte">Carta Porte</param>
        /// <param name="referencia">Referencia del Cliente</param>
        /// <param name="id_region">Id de Región</param>
        /// <param name="id_tipo_servicio">Id de Tipo de Servicio</param>
        /// <param name="id_alcance_servicio">Id de Alcance del Servicio</param>
        /// <param name="fecha_inicial_cita_carga">Fecha Inicial de Cita de Carga</param>
        /// <param name="fecha_final_cita_carga">Fecha Final de Cita de Carga</param>
        /// <param name="fecha_inicial_cita_descarga">Fecha Inicial de Cita de Descarga</param>
        /// <param name="fecha_final_cita_descarga">Fecha Final de Cita de Descarga</param>
        /// <param name="fecha_inicial_inicio_servicio">Fecha Inicial de Comienzo de Servicio</param>
        /// <param name="fecha_final_inicio_servicio">Fecha Final de Comienzo de Servicio</param>
        /// <param name="fecha_inicial_fin_servicio">Fecha Inicial de Fin de Servicio</param>
        /// <param name="fecha_final_fin_servicio">Fecha Final de Fin de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaReporteServicios(int id_compania, string no_servicio, int id_estatus, int id_ubicacion_carga, int id_ubicacion_descarga, int id_cliente, string porte, string referencia, int id_region,
                                                    int id_tipo_servicio, int id_alcance_servicio, DateTime fecha_inicial_cita_carga, DateTime fecha_final_cita_carga, DateTime fecha_inicial_cita_descarga, DateTime fecha_final_cita_descarga,
                                                    DateTime fecha_inicial_inicio_servicio, DateTime fecha_final_inicio_servicio, DateTime fecha_inicial_fin_servicio, DateTime fecha_final_fin_servicio)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 1, id_compania, no_servicio, id_estatus, id_ubicacion_carga, id_ubicacion_descarga, id_cliente, porte, referencia, id_region, id_tipo_servicio, id_alcance_servicio,
                                      Fecha.ConvierteDateTimeString(fecha_inicial_cita_carga,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_cita_carga, ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      Fecha.ConvierteDateTimeString(fecha_inicial_cita_descarga,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_cita_descarga,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      Fecha.ConvierteDateTimeString(fecha_inicial_inicio_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_inicio_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      Fecha.ConvierteDateTimeString(fecha_inicial_fin_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_fin_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables[0];

                //Devolviendo resultados
                return mit;
            }
        }
        /// <summary>
        /// Carga Sevicios  documentados
        /// </summary>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_ciudad_origen">Id Ciudad Origen</param>
        /// <param name="id_ciudad_destino">Id Ciudad Destino</param>
        /// <param name="cita_carga">Inicio Cita Carga</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <returns></returns>
        public static DataTable CargaServiciosSinIniciar(int id_cliente, int id_ciudad_origen, int id_ciudad_destino, string cita_carga, int id_compania_emisor)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 2,id_cliente, id_ciudad_origen, id_ciudad_destino, cita_carga!=""? Fecha.ConvierteDateTimeString(Convert.ToDateTime(cita_carga), "yyyy-dd-MM HH:mm:ss") :"",
                                     cita_carga != ""?  Fecha.ConvierteDateTimeString(Convert.ToDateTime(cita_carga).AddHours(23).AddMinutes(59), "yyyy-dd-MM HH:mm:ss") : "", id_compania_emisor, "", "", "",
                                      "", "", "", "", "","","","","","","" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Sevicios que esten Pendientes para Despacho
        /// </summary>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_ciudad_origen">Id Ciudad Origen</param>
        /// <param name="id_ciudad_destino">Id Ciudad Destino</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="no_servicio">No Servicio</param>
        /// <param name="terminados">Si desea obtener servicios terminados asigna valor a true de lo contrario se obtienen servicios en estatus Iniciados y Documentados.</param>
        /// <param name="referencia">Referencia de viaje a buscar</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <returns></returns>
        public static DataTable CargaServiciosParaDespacho(int id_cliente, int id_ciudad_origen, int id_ciudad_destino, int id_compania_emisor, string no_servicio,
                                                           bool terminados, string referencia, string porte)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 3, id_cliente, id_ciudad_origen, id_ciudad_destino, id_compania_emisor, no_servicio, Convert.ToByte(terminados), referencia, porte, "",
                                      "", "", "", "", "","","","","","","" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga el movimiento o servicio asignado a cada unidad motriz o de arrastre de la compañía indicada
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía</param>
        /// <param name="no_unidad">Número de Unidad</param>
        /// <param name="id_estatus_unidad">Id de Estatus de Unidad</param>
        /// <param name="id_ubicacion">Id de Ubicación</param>
        /// <param name="tipo_unidades">Tipo de Unidades a Mostrar como principales (Primer Columna Izquierda)</param>
        /// <param name="unidades_propias">Permite visuaizar  las unidades que sean propias</param>
        /// <param name="unidades_no_propias">Permite visuaizar las unidades que no sean propias</param>
        /// <param name="id_cliente">Cliente de las Unidades (Servicios)</param>
        /// <returns></returns>
        public static DataTable CargaDespachoSimplificadoUnidades(int id_compania_emisor, string no_unidad, string id_estatus_unidad, int id_ubicacion, string tipo_unidades, bool unidades_propias, bool unidades_no_propias, int id_cliente, string id_flotas)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 4,id_compania_emisor, no_unidad, id_estatus_unidad, id_ubicacion, tipo_unidades, unidades_propias,unidades_no_propias, id_cliente, id_flotas,
                                      "", "", "", "", "","","","","","","" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga los datos de requeridos del servicio para su envío a Omnitracs
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_proveedor_ftp">Proveedor FTP (GPS)</param>
        /// <returns></returns>
        public static DataTable CargaInformacionServicioOmnitracs(int id_servicio, int id_proveedor_ftp)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 5, id_servicio, "", "", id_proveedor_ftp, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de Obtener los Servicios, para asignarles una Requisición
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="no_servicio">No. de Servicio</param>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="id_estatus">Estatus</param>
        /// <returns></returns>
        public static DataTable ObtieneServiciosRequisicion(int id_compania, string no_servicio, int id_cliente, byte id_estatus,
                                                            DateTime cita_ini_carga, DateTime cita_fin_carga, DateTime cita_ini_descarga,
                                                            DateTime cita_fin_descarga)
        {
            //Declarando Objeto de Retorno
            DataTable dtServicios = null;

            //Inicializando los parámetros de consulta
            object[] param = { 6, id_compania, no_servicio, id_cliente, id_estatus,
                               Fecha.ConvierteDateTimeString(cita_ini_carga, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")),
                               Fecha.ConvierteDateTimeString(cita_fin_carga, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")),
                               Fecha.ConvierteDateTimeString(cita_ini_descarga, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")),
                               Fecha.ConvierteDateTimeString(cita_fin_descarga, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")),
                               "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Validando que existan Servicios
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Datos Recuperados
                    dtServicios = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtServicios;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de los Movimientos Generales
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_servicio">No. Servicio</param>
        /// <param name="id_estatus_mov">Estatus Movimiento</param>
        /// <param name="id_tipo_mov">Tipo de Movimiento</param>
        /// <param name="id_parada_origen">Parada Origen</param>
        /// <param name="id_parada_destino">Parada Destino</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación</param>
        /// <param name="id_recurso_asignado">Recurso Asignado</param>
        /// <returns></returns>
        public static DataTable BuscarMovimiento(int id_compania_emisora, int no_servicio, string no_viaje, decimal secuencia, int id_parada_origen, int id_parada_destino)
        {
            //Declarando Objeto de Retorno
            DataTable dtMovimientos = null;

            //Armando Arreglo de Parametros
            object[] param = { 7, id_compania_emisora, no_servicio, no_viaje, secuencia, id_parada_origen, id_parada_destino, "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando a objeto de retorno
                    dtMovimientos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtMovimientos;
        }
        /// <summary>
        /// Método encargad de Obtener los Servicios Terminados que no han sido Facturados
        /// </summary>
        /// <param name="id_compania_emisora"></param>
        /// <param name="id_cliente"></param>
        /// <param name="no_servicio"></param>
        /// <param name="referencia"></param>
        /// <param name="id_parada_origen"></param>
        /// <param name="id_parada_destino"></param>
        /// <param name="inicio"></param>
        /// <param name="termino"></param>
        /// <returns></returns>
        public static DataTable CargaServiciosSinFacturacionElectronica(int id_compania_emisora, int id_cliente, int no_servicio, string referencia,
                                            int id_parada_origen, int id_parada_destino, DateTime inicio, DateTime termino)
        {
            //Declarando Objeto de Retorno
            DataTable dtServicios = null;

            //Armando Arreglo de Parametros
            object[] param = { 8, id_compania_emisora, id_cliente, no_servicio, referencia, id_parada_origen, id_parada_destino,
                               inicio == DateTime.MinValue ? "" : inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               termino == DateTime.MinValue ? "" : termino.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando a objeto de retorno
                    dtServicios = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtServicios;
        }
        /// <summary>
        /// Carga 
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="id_parada"></param>
        /// <param name="tipo"></param>
        /// <param name="id_evento"></param>
        /// <returns></returns>
        public static DataTable CargaEventosParaDespacho(int id_servicio, int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 9, id_servicio, id_parada, "", 0, 0, "", "", "", "",
                                      "", "", "", "", "","","","","","","" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga 
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="id_parada"></param>
        /// <param name="tipo"></param>
        /// <param name="id_evento"></param>
        /// <returns></returns>
        public static DataTable CreaciondeViaje(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 10, id_servicio, "", "", "", "", "", "", "", "",
                                      "", "", "", "", "","","","","","","" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Carga 
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="id_parada"></param>
        /// <param name="tipo"></param>
        /// <param name="id_evento"></param>
        /// <returns></returns>
        public static DataTable CreacionArregloViaje(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 11, id_servicio, "", "", "", "", "", "", "", "",
                                      "", "", "", "", "","","","","","","" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Realiza la búsqueda de servicios coincidentes con los criterios establecidos
        /// </summary>
        /// <param name="id_compania">Id de Compañía</param>
        /// <param name="no_servicio">Número de Servicio</param>
        /// <param name="id_estatus">Id de Estatus de Servicio</param>
        /// <param name="id_ubicacion_carga">Id de Ubicación de Carga</param>
        /// <param name="id_ubicacion_descarga">Id de Ubicación de Descarga</param>
        /// <param name="id_cliente">Id de Cliente</param>
        /// <param name="fecha_inicial_cita_carga">Fecha Inicial de Cita de Carga</param>
        /// <param name="fecha_final_cita_carga">Fecha Final de Cita de Carga</param>
        /// <param name="fecha_inicial_cita_descarga">Fecha Inicial de Cita de Descarga</param>
        /// <param name="fecha_final_cita_descarga">Fecha Final de Cita de Descarga</param>
        /// <param name="fecha_inicial_inicio_servicio">Fecha Inicial de Comienzo de Servicio</param>
        /// <param name="fecha_final_inicio_servicio">Fecha Final de Comienzo de Servicio</param>
        /// <param name="fecha_inicial_fin_servicio">Fecha Inicial de Fin de Servicio</param>
        /// <param name="fecha_final_fin_servicio">Fecha Final de Fin de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaReporteServiciosAnticiposProgramados(int id_compania, string no_servicio, int no_economico, int id_ubicacion_carga, int id_ubicacion_descarga, int id_cliente, DateTime fecha_inicial_cita_carga, DateTime fecha_final_cita_carga, DateTime fecha_inicial_cita_descarga, DateTime fecha_final_cita_descarga,
                                                    DateTime fecha_inicial_inicio_servicio, DateTime fecha_final_inicio_servicio, DateTime fecha_inicial_fin_servicio, DateTime fecha_final_fin_servicio)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 12, id_compania, no_servicio, no_economico, id_ubicacion_carga, id_ubicacion_descarga, id_cliente, "", "", "", "", "",
                                      Fecha.ConvierteDateTimeString(fecha_inicial_cita_carga,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_cita_carga, ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      Fecha.ConvierteDateTimeString(fecha_inicial_cita_descarga,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_cita_descarga,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      Fecha.ConvierteDateTimeString(fecha_inicial_inicio_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_inicio_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      Fecha.ConvierteDateTimeString(fecha_inicial_fin_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]), Fecha.ConvierteDateTimeString(fecha_final_fin_servicio,ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                      ""};

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables[0];

                //Devolviendo resultados
                return mit;
            }
        }
        /// <summary>
        /// Método encaragdo de Obtener los Viajes que tienen FOLIO NISSAN
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="viajes"></param>
        /// <returns></returns>
        public static List<DataRow> ObtieneViajesNISSAN(int id_compania, List<string> viajes)
        {
            //Declarando objeto de retorno
            List<DataRow> mit = new List<DataRow>();

            if (viajes.Count > 0)
            {
                string noViajes = @"";
                foreach (string vj in viajes)
                {
                    //Validando Cadena Vacia
                    if (noViajes.Equals(""))
                        noViajes = @"'" + vj.Trim() + @"'";
                    else
                        noViajes += @",'" + vj.Trim() + @"'";
                }

                //Inicializando los parámetros de consulta
                object[] parametros = { 13, id_compania, "1", "", "", "", "", "", "", "",
                                        "", "", "", "", "","","","","","",noViajes };

                //Cargando los registros de interés
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
                {
                    //Si hay registros
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        foreach (DataRow dr in ds.Tables["Table"].Rows)
                            mit.Add(dr);
                    }
                }
            }

            //Devolviendo resultados
            return mit;
        }
        /// <summary>
        /// Método encaragdo de Obtener los Viajes que tienen FOLIO NISSAN
        /// </summary>
        /// <param name="id_compania">Compania</param>
        /// <param name="periodo_inicio"></param>
        /// <param name="periodo_fin"></param>
        /// <returns></returns>
        public static List<DataRow> ObtieneViajesNISSAN(int id_compania, DateTime periodo_inicio, DateTime periodo_fin)
        {
            //Declarando objeto de retorno
            List<DataRow> mit = new List<DataRow>();

            //Inicializando los parámetros de consulta
            object[] parametros = { 13, id_compania, "2", "", "", "", "", "", "", "", "", "", "", "", "","","","","","",
                                    string.Format("'{0:yyyyMMdd HH:mm}' AND '{1:yyyyMMdd HH:mm}'", periodo_inicio, periodo_fin) };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        mit.Add(dr);
                }
            }

            //Devolviendo resultados
            return mit;
        }

        #endregion
    }
}