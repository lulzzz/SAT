using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Clase encargada de Obtener los Reportes del Modulo de Despacho
    /// </summary>
    public static class Reporte
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "despacho.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Método Público encargado de Obtener los Viajes por Entidad
        /// </summary>
        /// <param name="id_tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_entidad">Entidad</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <param name="id_estatus_liquidacion">Estatus de la Liquidación</param>
        /// <returns></returns>
        public static DataTable ObtieneViajesEntidad(byte id_tipo_entidad, int id_entidad, DateTime fecha_liquidacion, byte id_estatus_liquidacion)
        {   
            //Declarando Objeto de Retorno
            DataTable dtViajes = null;
            
            //Armando Arreglo de Parametros
            object[] param = { 1, id_entidad.ToString(), id_tipo_entidad.ToString(), fecha_liquidacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 id_estatus_liquidacion.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            
            //Instanciando Reporte del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignado Resultado Obtenido
                    dtViajes = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtViajes;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Movimientos dado un Viaje
        /// </summary>
        /// <param name="id_viaje">Viaje</param>
        /// <param name="id_liquidacion">Liquidación Actual</param>
        /// <param name="id_entidad">Entidad</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_estatus_liquidacion">Estatus de la Liquidación</param>
        /// <param name="pagos_otros">Indicador de Pagos Otros</param>
        /// <returns></returns>
        public static DataSet ObtieneMovimientosYPagosPorViaje(int id_viaje, int id_liquidacion, int id_entidad, byte id_tipo_entidad, byte id_estatus_liquidacion, bool pagos_otros)
        {   
            //Armando Arreglo de Parametros
            object[] param = { 2, id_viaje.ToString(), id_liquidacion.ToString(), id_entidad.ToString(), id_tipo_entidad.ToString(), id_estatus_liquidacion.ToString(), pagos_otros ? "1" : "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            
            //Instanciando Reporte del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                
                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Método Público encargado de Obtener la Fecha del Primer Viaje Asignado
        /// </summary>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="id_tipo_entidad">Tipo de la Entidad</param>
        /// <returns></returns>
        public static DateTime ObtieneFechaPrimerViajeAsignado(int id_entidad, int id_tipo_entidad)
        {   
            //Declarando Objeto de Retorno
            DateTime fecha_primer_viaje = DateTime.MinValue;
            
            //Armando Arreglo de Parametros
            object[] param = { 3, id_entidad.ToString(), id_tipo_entidad.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            
            //Instanciando Reporte del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Iniciando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        
                        //Asignado Resultado Obtenido
                        DateTime.TryParse(dr["FechaPrimerViaje"].ToString(), out fecha_primer_viaje);
                }
            }
            
            //Devolviendo Resultado Obtenido
            return fecha_primer_viaje;
        }
        /// <summary>
        /// Método Público encargado de Obtener la Fecha del Ultimo Viaje Asignado
        /// </summary>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="id_tipo_entidad">Tipo de la Entidad</param>
        /// <returns></returns>
        public static DateTime ObtieneFechaUltimoViajeAsignado(int id_entidad, int id_tipo_entidad)
        {   
            //Declarando Objeto de Retorno
            DateTime fecha_ultimo_viaje = DateTime.MinValue;
            
            //Armando Arreglo de Parametros
            object[] param = { 4, id_entidad.ToString(), id_tipo_entidad.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            
            //Instanciando Reporte del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Iniciando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        
                        //Asignado Resultado Obtenido
                        DateTime.TryParse(dr["FechaUltimoViaje"].ToString(), out fecha_ultimo_viaje);
                }
            }
            
            //Devolviendo Resultado Obtenido
            return fecha_ultimo_viaje;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Movimientos que estan ligados a Pagos
        /// </summary>
        /// <param name="id_pago">Id de Pago</param>
        /// <param name="id_liquidacion">Id de Liquidación</param>
        /// <returns></returns>
        public static DataTable ObtieneMovimientosDePago(int id_pago, int id_liquidacion)
        {   
            //Declarando Objeto de Retorno
            DataTable dtMovimientosPagos = null;
            
            //Armando Arreglo de Parametros
            object[] param = { 5, id_pago.ToString(), id_liquidacion.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            
            //Instanciando Reporte del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignado Resultado Obtenido
                    dtMovimientosPagos = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtMovimientosPagos;
        }

        /// <summary>
        /// Mètodo encargado de cargar los Movimientos en Vacio
        /// </summary>
        /// <param name="id_ciudad_origen">Id Ciudad Origen</param>
        /// <param name="id_ciudad_destino">Id Ciudad Destino</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignacion(Unidad, Operador, Tercero-9</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <param name="id_estatus">Id Esatatus Movimiento Vacio</param>
        /// <param name="inicio_fecha_llegada">Inicio Fecha de Llegada</param>
        /// <param name="fin_fecha_llegada">Fin Fecha de Llegada</param>
        /// <param name="inicio_fecha_salida">Inicio Fecha de Salida</param>
        /// <param name="fin_fecha_salida">Fin Fecha de Salida</param>
        /// <param name="id_movimiento">Id de movimiento a buscar</param>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <returns></returns>
        public static DataTable CargaMovimientoVacio(int id_ciudad_origen, int id_ciudad_destino, byte id_tipo_asignacion, int id_recurso, byte id_estatus,
                                                     DateTime inicio_fecha_llegada, DateTime fin_fecha_llegada, DateTime inicio_fecha_salida, DateTime fin_fecha_salida, int id_movimiento, 
                                                     int id_compania_emisor)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, id_ciudad_origen, id_ciudad_destino, id_tipo_asignacion, id_recurso, id_estatus,
                                  inicio_fecha_llegada == DateTime.MinValue ? "": inicio_fecha_llegada.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                  fin_fecha_llegada == DateTime.MinValue ? "": fin_fecha_llegada.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                  inicio_fecha_salida == DateTime.MinValue ? "": inicio_fecha_salida.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                  fin_fecha_salida == DateTime.MinValue ? "": fin_fecha_salida.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), id_movimiento, id_compania_emisor, "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion. ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga todas las Estancias de la Unidades
        /// </summary>
        /// <param name="id_ubicacion">Id Ubicación</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad (Tarctor, Remolque)</param>
        /// <param name="id_recurso">Id Unidad</param>
        /// <param name="inicio_fecha_inicio">fecha inicial del Inicio de la Estancia</param>
        /// <param name="inicio_fecha_fin">fecha final del Inicio de la Estancia</param>
        /// <param name="termino_fecha_inicio">fecha inicial del Termino de la Estancia</param>
        /// <param name="termino_fecha_fin">fecha final del Termino de la Estancia</param>
        /// <returns></returns>
        public static DataTable CargaEstancias(int id_ubicacion, byte id_tipo_unidad, int id_recurso, DateTime inicio_fecha_inicio, DateTime inicio_fecha_fin, DateTime termino_fecha_inicio,
                                              DateTime termino_fecha_fin)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 7, id_ubicacion, id_tipo_unidad, id_recurso, Fecha.ConvierteDateTimeString(inicio_fecha_inicio, "dd/MM/yyyy HH:mm:ss"),
                              Fecha.ConvierteDateTimeString(inicio_fecha_fin, "dd/MM/yyyy HH:mm:ss"), 
                           Fecha.ConvierteDateTimeString(termino_fecha_inicio, "dd/MM/yyyy HH:mm:ss"), Fecha.ConvierteDateTimeString(termino_fecha_fin, "dd/MM/yyyy HH:mm:ss"),
                             "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        /// Carga todas las Estancias de la Unidades
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <param name="no_servicio">No Servicio</param>
        /// <param name="id_ubicacion">Id Ubicación</param>
        /// <param name="id_tipo_evento">Tipo de Evento</param>
        /// <param name="id_estatus_evento">Estatus Evento</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public static DataSet ReporteEventos(int id_compania_emisor, string no_servicio, int id_ubicacion, byte id_tipo_evento, byte estatus_evento,
                                              DateTime fecha, string referencia)
        {
            //Definiendo objeto de retorno
            DataSet ds = null;

            //Armando Arreglo de Parametros
            object[] param = { 8, id_compania_emisor, no_servicio, id_ubicacion, id_tipo_evento,estatus_evento, 
                               fecha == DateTime.MinValue ? "" : fecha.ToString((ConfigurationManager.AppSettings["FormatoFechaReportes"])),
                               "", referencia, "","", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(DS))
                    //Asignando a objeto de retorno
                    ds = DS;

                //Devolviendo resultado
                return ds;
            }
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
        public static DataTable ReporteMovimientosGeneral(int id_compania_emisora, string id_servicio, byte id_estatus_mov, byte id_tipo_mov, 
                                        int id_parada_origen, int id_parada_destino, byte id_tipo_asignacion, int id_recurso_asignado,
                                        DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtMovimientos = null;

            //Armando Arreglo de Parametros
            object[] param = { 9, id_compania_emisora, id_servicio, id_estatus_mov, id_tipo_mov, id_parada_origen, id_parada_destino, id_tipo_asignacion, id_recurso_asignado,
                               fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        /// Método encargado de Generar el Reporte de Unidades
        /// </summary>
        /// <param name="id_compania_emisora"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <returns></returns>
        public static DataTable ReporteGeneralUnidades(int id_compania_emisora, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtUnidades = null;

            //Armando Arreglo de Parametros
            object[] param = { 10, id_compania_emisora, fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               "", "", "", "", "", "","", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando a objeto de retorno
                    dtUnidades = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método encargado de Obtener el Hsitorial de los Movimientos según su Asignación
        /// </summary>
        /// <param name="id_recurso_asignado">Recurso Asignado</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación</param>
        /// <returns></returns>
        public static DataTable ObtieneHistorialMovimiento(int id_recurso_asignado, int id_tipo_asignacion, DateTime fec_ini, DateTime fec_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtHistorial = null;

            //Armando Arreglo de Parametros
            object[] param = { 11, id_tipo_asignacion, id_recurso_asignado, fec_ini == DateTime.MinValue ? "" : fec_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fec_fin == DateTime.MinValue ? "" : fec_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               "", "", "", "", "","", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtHistorial = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtHistorial;
        }

        /// <summary>
        /// Método encargado de Obtener el Balance de Unidades
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_estatus">Id Estatus Unidad</param>
        /// <param name="id_tipo_unidad">Id Tipo Unidad</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_ubicacion">Id Ubicacion</param>
        /// <returns></returns>
        public static DataSet BalanceUnidades( int id_compania_emisor, byte id_estatus, int id_tipo_unidad, int id_unidad, string no_servicio, int id_ubicacion)
        {
            //Definiendo objeto de retorno
            DataSet ds = null;

            //Armando Arreglo de Parametros
            object[] param = { 12, id_compania_emisor, id_estatus, id_tipo_unidad, id_unidad, no_servicio, id_ubicacion,
                             "", "", "","", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(DS))
                    //Asignando a objeto de retorno
                    ds = DS;

                //Devolviendo resultado
                return ds;
            }
        }
        /// <summary>
        /// Obtine los servicios documentados e iniciados para su planeación de recursos
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía que realiza los servicios</param>
        /// <param name="id_cliente_receptor">Id deCliente (al que se realzia el servicio)</param>
        /// <param name="no_servicio">Número de Servicio</param>
        /// <param name="ver_documentados">True para ver servicios documentados</param>
        /// <param name="ver_iniciados">True para ver servicios iniciados</param>
        /// <param name="fecha_ini_carga">Fecha de Inicio (Carga)</param>
        /// <param name="fecha_fin_carga">Fecha de Fin (Carga)</param>
        /// <param name="fecha_ini_descarga">Fecha de Inicio (Descarga)</param>
        /// <param name="fecha_fin_descarga">Fecha de Fin (Descarga)</param>
        /// <param name="referencia">Referencia de Viaje</param>
        /// <returns></returns>
        public static DataTable CargaPlaneacionServicios(int id_compania_emisor, int id_cliente_receptor, string no_servicio, bool ver_documentados, bool ver_iniciados,
                                                DateTime fecha_ini_carga, DateTime fecha_fin_carga, DateTime fecha_ini_descarga, DateTime fecha_fin_descarga, string referencia,
                                                string id_unidad, string id_operador)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 13, id_compania_emisor, id_cliente_receptor, no_servicio, ver_documentados ? 1 : 0, ver_iniciados ? 1 : 0, 
                                 fecha_ini_carga == DateTime.MinValue ? "" : fecha_ini_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_fin_carga == DateTime.MinValue ? "" : fecha_fin_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_ini_descarga == DateTime.MinValue ? "" : fecha_ini_descarga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_fin_descarga == DateTime.MinValue ? "" : fecha_fin_descarga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 referencia, "", "", "", id_unidad == "0" ? "" : id_unidad, id_operador == "0" ? "": id_operador, "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        /// Obtine los servicios documentados e iniciados para su planeación de recursos
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía que realiza los servicios</param>
        /// <param name="id_cliente_receptor">Id deCliente (al que se realzia el servicio)</param>
        /// <param name="no_servicio">Número de Servicio</param>
        /// <param name="ver_documentados">True para ver servicios documentados</param>
        /// <param name="ver_iniciados">True para ver servicios iniciados</param>
        /// <param name="operacion">Operación Servicio</param>
        /// <param name="terminal">Terminal</param>
        /// <param name="alcance">Alcance del  Servicio</param>
        /// <param name="inicio_cita_carga">Fecha Inicio (Cita Carga)</param>
        /// <param name="fin_cita_carga">Fecha Fin (Cita Carga)</param>
        /// <param name="inicio_fec_doc">Fecha Inicio (Documentación)</param>
        /// <param name="fin_fec_doc">Fecha Fin (Documentación)</param>
        /// <returns></returns>
        public static DataTable CargaPlaneacionServicios(int id_compania_emisor, int id_cliente_receptor, string no_servicio, 
                                        bool ver_documentados, bool ver_iniciados, string operacion, string terminal,
                                        string alcance, DateTime fecha_ini_carga, DateTime fecha_fin_carga, 
                                        DateTime fecha_ini_descarga, DateTime fecha_fin_descarga, string id_unidad, string id_operador)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 13, id_compania_emisor, id_cliente_receptor, no_servicio, ver_documentados ? 1 : 0, ver_iniciados ? 1 : 0,
                               fecha_ini_carga == DateTime.MinValue ? "" : fecha_ini_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_fin_carga == DateTime.MinValue ? "" : fecha_fin_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_ini_descarga == DateTime.MinValue ? "" : fecha_ini_descarga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_fin_descarga == DateTime.MinValue ? "" : fecha_fin_descarga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               "", operacion, terminal, alcance, id_unidad == "0" ? "" : id_unidad, id_operador == "0" ? "": id_operador, "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        /// Método encargado de Obtener el Reporte de Devoluciones
        /// </summary>
        /// <param name="consecutivo_compania"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_estatus"></param>
        /// <param name="fecha_captura_ini"></param>
        /// <param name="fecha_captura_fin"></param>
        /// <param name="fecha_devolucion_faltante_ini"></param>
        /// <param name="fecha_devolucion_faltante_fin"></param> 
        /// <param name="observacion"></param>
        /// <param name="referencia"></param>
        /// <param name="id_compania_emisora"></param>
        /// <returns></returns>
        public static DataSet ReporteDevolucionesDetalle(int consecutivo_compania, byte id_tipo, byte id_estatus, DateTime fecha_captura_ini, DateTime fecha_captura_fin, DateTime fecha_devolucion_faltante_ini, 
                                                        DateTime fecha_devolucion_faltante_fin, string observacion, string referencia, int id_compania_emisora, string no_viaje, int id_cliente)
        {
            //Definiendo objeto de retorno
            DataSet ds = null;

            //Armando Arreglo de Parametros
            object[] param = { 14, consecutivo_compania, id_tipo, id_estatus, 
                               fecha_captura_ini == DateTime.MinValue ? "" : fecha_captura_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fecha_captura_fin == DateTime.MinValue ? "" : fecha_captura_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fecha_devolucion_faltante_ini == DateTime.MinValue ? "" : fecha_devolucion_faltante_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_devolucion_faltante_fin == DateTime.MinValue ? "" : fecha_devolucion_faltante_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               observacion, referencia, id_compania_emisora.ToString(), no_viaje, id_cliente, "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(DS))
                    //Asignando a objeto de retorno
                    ds = DS;

                //Devolviendo resultado
                return ds;
            }
        }
        /// <summary>
        /// Obtiene las paradas con evento de carga/descarga en la semana actual
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía que realiza los servicios</param>
        /// <param name="id_cliente_receptor">Id deCliente (al que se realzia el servicio)</param>
        /// <param name="no_servicio">Número de Servicio</param>
        /// <param name="solo_pendientes">True para visualizar sólo eventos de paradas en estatus Registrado</param>
        /// <returns></returns>
        public static DataSet ObtieneParadasCargaDescargaSemanaActual(int id_compania_emisor, bool solo_pendientes)
        {
            //Armando Arreglo de Parametros
            object[] param = { 15, id_compania_emisor, solo_pendientes ? "1" : "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                //Devolviendo resultado
                return ds;
        }

        /// <summary>
        /// Método que permite la carga de valores para el reporte bitacora eventos
        /// </summary>
        /// <param name="id_compania_emisor">Identificador de la empresa</param>
        /// <param name="id_cliente">Identificador del cliente</param>
        /// <param name="no_servicio">Numero consecutivo referente a un servicio</param>
        /// <param name="referencias">Referencias de servicio</param>
        /// <param name="estatus_inicio">Estatus de un evento (Iniciado)</param>
        /// <param name="estatus_fin">Estatus de un evento (Terminado)</param>
        /// <param name="fecha_inicio">Fecha de cita de descarga</param>
        /// <param name="fecha_fin">Fecha de cita de descarga</param>
        /// <param name="tipo_evento">Tipo de evento carga, descarga,etc. definida por el tipo de parada(operativa, servicio)</param>
        /// <returns></returns>
        public static DataTable ReporteBitacoraEvento(int id_compania_emisor, int id_cliente, string no_servicio, string referencias, 
                                                      byte estatus_inicio,byte estatus_fin , DateTime fecha_inicio, DateTime fecha_fin,byte tipo_evento)
        {
            //Creación del dataset dsBitacora
            DataTable dtBitacora = null;
            //Creacion del arreglo param que permite almacenar los datos necesarios para el reporte.
            object[] param = { 16, id_compania_emisor, id_cliente, no_servicio, referencias, estatus_inicio, estatus_fin, 
                                 fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) ,
                                 fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                tipo_evento, "", "", "", "", "", "", "", "", "", "", "" };
            //Invoca al método encargado de hacer la consulta a base de datos y el resultado lo almacena en el dataset ds
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que existan los datos en el dataset
                if (Validacion.ValidaOrigenDatos(ds,"Table"))               
                    //Asigna al dataset dsBitacora los valores del dataset ds
                    dtBitacora = ds.Tables["Table"];
            }
            //Retorna el dataset dsBitacora al método
            return dtBitacora;
        }
        /// <summary>
        /// Método encargado de Obtener el Historial de los Servicios
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="no_servicio">No. de Servicio</param>
        /// <param name="carta_porte">Número de Carta Porte</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="cita_carga_ini">Cita de Carga (Inicio)</param>
        /// <param name="cita_carga_fin">Cita de Carga (Fin)</param>
        /// <param name="cita_descarga_ini">Cita de Descarga (Inicio)</param>
        /// <param name="cita_descarga_fin">Cita de Descarga (Fin)</param>
        /// <param name="fecha_doc_ini">Fecha de Documentación (Inicio)</param>
        /// <param name="fecha_doc_fin">Fecha de Documentación (Fin)</param>
        /// <returns></returns>
        public static DataTable CargaHistorialServicios(int id_compania_emisor, int id_cliente_receptor, string no_servicio, string carta_porte, int id_estatus,
                                                string referencia, DateTime cita_carga_ini, DateTime cita_carga_fin, DateTime cita_descarga_ini, DateTime cita_descarga_fin,
                                                DateTime fecha_doc_ini, DateTime fecha_doc_fin)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 17, id_compania_emisor, id_cliente_receptor, no_servicio, id_estatus, referencia, 
                                 cita_carga_ini == DateTime.MinValue ? "" : cita_carga_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 cita_carga_fin == DateTime.MinValue ? "" : cita_carga_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                 cita_descarga_ini == DateTime.MinValue ? "" : cita_descarga_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 cita_descarga_fin == DateTime.MinValue ? "" : cita_descarga_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_doc_ini == DateTime.MinValue ? "" : fecha_doc_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_doc_fin == DateTime.MinValue ? "" : fecha_doc_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 carta_porte, "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        /// Carga los datos de requeridos del movimiento en vacío para su envío a Omnitracs
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento en vacío</param>
        /// <param name="id_proveedor_ftp">Id Proveedor de FTP</param>
        /// <returns></returns>
        public static DataTable CargaInformacionMovimientoVacioFTP(int id_movimiento, int id_proveedor_ftp)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 18, id_movimiento, id_proveedor_ftp, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
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
        /// Método encargado de Cargar el Encabezado del Viaje para la Aplicación Móvil
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns></returns>
        public static DataTable CargaEncabezadoViajeMovil(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtEncabezado = null;
            
            //Inicializando los parámetros de consulta
            object[] parametros = { 19, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Validando si existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtEncabezado = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtEncabezado;
        }
        /// <summary>
        /// Método encargado de Cargar las Paradas del Viaje para la Aplicación Móvil
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns></returns>
        public static DataTable CargaParadasViajeMovil(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtParadas = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 20, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Validando si existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtParadas = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtParadas;
        }
        /// <summary>
        /// Método encargado de Cargar los Datos de Viaje para la Aplicación Móvil
        /// </summary>
        /// <param name="id_servicio">Servicio Deseado</param>
        /// <returns></returns>
        public static DataTable CargaDatosViajeMovil(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtDatosServicio = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 21, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Validando si existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtDatosServicio = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtDatosServicio;
        }
        /// <summary>
        /// Método encargado de Cargar los Servicios sin Liquidar de un Operador para la Aplicación Móvil
        /// </summary>
        /// <param name="id_usuario">Usuario de la Aplicación Móvil</param>
        /// <param name="tiempo">Especifica el Valor del Tiempo a Filtrar</param>
        /// <returns></returns>
        public static DataTable ObtieneServiciosOperadorMovil(int id_usuario, int tiempo)
        {
            //Declarando Objeto de Retorno
            DataTable dtServiciosOperador = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 22, id_usuario, tiempo, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Validando si existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtServiciosOperador = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtServiciosOperador;
        }
        /// <summary>
        /// Método encargado de Cargar el Servicio Activo de un Operador para la Aplicación Móvil
        /// </summary>
        /// <param name="id_usuario">Usuario de la Aplicación Móvil</param>
        /// <returns></returns>
        public static DataTable ObtieneServicioActivoOperadorMovil(int id_usuario)
        {
            //Declarando Objeto de Retorno
            DataTable dtServiciosOperador = new DataTable("ServicioActual");
            dtServiciosOperador.Columns.Add("IdServicio", typeof(int));            

            //Inicializando los parámetros de consulta
            object[] parametros = { 26, id_usuario, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Operador
            using (SAT_CL.Global.Operador operador = SAT_CL.Global.Operador.ObtieneOperadorUsuario(id_usuario))
            {
                //Realizando la consulta
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
                {
                    //Validando si existen Registros
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Fila
                        foreach (DataRow dr in ds.Tables["Table"].Rows)
                        {
                            //Asignando Primer Registro
                            dtServiciosOperador.Rows.Add(Convert.ToInt32(dr["IdServicio"]));

                            //Terminando Ciclo en la Primera Iteración
                            break;
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return dtServiciosOperador;
        }
        /// <summary>
        /// Obtiene las paradas con evento de carga/descarga en la semana actual
        /// </summary>
        /// <param name="inicio">Inicio de Semana</param>
        /// <param name="termino">Termino de Semana</param>
        /// <param name="id_compania_emisor">Id de Compañía que realiza los servicios</param>
        /// <param name="solo_pendientes">True para visualizar sólo eventos de paradas en estatus Registrado</param>
        /// <returns></returns>
        public static DataSet ObtieneParadasCargaDescargaSemanaActual(DateTime inicio, DateTime termino, int id_compania_emisor, bool solo_pendientes)
        {
            //Armando Arreglo de Parametros
            object[] param = { 23, id_compania_emisor, solo_pendientes ? "1" : "0", 
                               inicio == DateTime.MinValue ? "" : inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               termino == DateTime.MinValue ? "" : termino.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                //Devolviendo resultado
                return ds;
        }
        /// <summary>
        /// Método encargado de Cargar Todas las Paradas del Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable CargaParadasTotalesViajeMovil(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtParadas = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 24, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Validando si existen Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtParadas = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtParadas;
        }
        /// <summary>
        /// Método encargado de Obtener el Evento Actual
        /// </summary>
        /// <param name="id_parada">Parada</param>
        /// <returns></returns>
        public static DataTable ObtieneEventoActual(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 25, id_parada, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        /// Método encargado de Obtener la Asignación Actual del Servicio de un Operador
        /// </summary>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static int ObtieneMovimientoAsignacionOperador(int id_operador, int id_servicio)
        {
            //Declarando Objeto de Retorno
            int idMovAR = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 27, id_operador, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando a objeto de retorno
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Obteniendo Asignación
                        idMovAR = Convert.ToInt32(dr["MovimientoAR"]);

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return idMovAR;
        }
        /// <summary>
        /// Obtine los servicios documentados e iniciados para su planeación de recursos
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía que realiza los servicios</param>
        /// <param name="id_cliente_receptor">Id deCliente (al que se realzia el servicio)</param>
        /// <param name="no_servicio">Número de Servicio</param>
        /// <param name="ver_documentados">True para ver servicios documentados</param>
        /// <param name="ver_iniciados">True para ver servicios iniciados</param>
        /// <param name="operacion">Operación Servicio</param>
        /// <param name="terminal">Terminal</param>
        /// <param name="alcance">Alcance del  Servicio</param>
        /// <param name="inicio_cita_carga">Fecha Inicio (Cita Carga)</param>
        /// <param name="fin_cita_carga">Fecha Fin (Cita Carga)</param>
        /// <param name="inicio_fec_doc">Fecha Inicio (Documentación)</param>
        /// <param name="fin_fec_doc">Fecha Fin (Documentación)</param>
        /// <returns></returns>
        public static DataTable CargaServiciosImprimir(int id_compania_emisor, int id_cliente_receptor, string no_servicio,
                                        bool ver_documentados, bool ver_iniciados, string operacion, string alcance, DateTime fecha_ini_carga, DateTime fecha_fin_carga,
                                        DateTime fecha_ini_descarga, DateTime fecha_fin_descarga, DateTime fecha_ini_ini_viaje, DateTime fecha_fin_ini_viaje, DateTime fecha_ini_fin_viaje,
                                        DateTime fecha_fin_fin_viaje, string no_viaje, string id_operador, string id_unidad)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 28, id_compania_emisor, id_cliente_receptor, no_servicio, ver_documentados ? 1 : 0, ver_iniciados ? 1 : 0,
                               fecha_ini_carga == DateTime.MinValue ? "" : fecha_ini_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_fin_carga == DateTime.MinValue ? "" : fecha_fin_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_ini_descarga == DateTime.MinValue ? "" : fecha_ini_descarga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_fin_descarga == DateTime.MinValue ? "" : fecha_fin_descarga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_ini_ini_viaje == DateTime.MinValue ? "" : fecha_ini_ini_viaje.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_fin_ini_viaje == DateTime.MinValue ? "" : fecha_fin_ini_viaje.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_ini_fin_viaje == DateTime.MinValue ? "" : fecha_ini_fin_viaje.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_fin_fin_viaje == DateTime.MinValue ? "" : fecha_fin_fin_viaje.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               operacion, alcance, no_viaje, id_operador == "0" ? "" : id_operador, id_unidad == "0" ? "" : id_unidad, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion
    }
}
