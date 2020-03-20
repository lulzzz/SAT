using System.Data;
using System;
using System.Configuration;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Genera la clase para Reportes de la Liquidación
    /// </summary>
    public static class Reportes
    {
        #region Nombre stored procedure

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string nombre_store_procedure = "liquidacion.sp_reporte_modulo";

        #endregion

        #region Metodos estaticos


        /// <summary>
        /// Genera el Reporte de la liquidaciones
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="no_liquidacion">No Liquidación</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_tercero">Id Terrcero</param>
        /// <param name="id_estatus">Id Estatus</param>
        /// <param name="fecha_inicio_liquidacion">Fecha Inicio Liquidación</param>
        /// <param name="fecha_fin_liquidacion">Fecha Fin Liquidación</param>
        /// <param name="id_tipo_asignacion">Id Tipo Asignación</param>
        /// <param name="id_tipo_operador">Tipo de Operador</param>
        /// <returns></returns>
        public static DataSet ReporteLiquidaciones(int id_compania_emisor, int no_liquidacion, int id_unidad, int id_operador, int id_tercero, byte id_estatus,
                                                  DateTime fecha_inicio_liquidacion, DateTime fecha_fin_liquidacion, byte id_tipo_asignacion, byte id_tipo_operador)
        {
            //Inicializando los parámetros de consulta 
            object[] parametros = { 1, id_compania_emisor, no_liquidacion, id_unidad, id_operador, id_tercero, id_estatus, 
                                     fecha_inicio_liquidacion== DateTime.MinValue ? "": fecha_inicio_liquidacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     fecha_fin_liquidacion == DateTime.MinValue ? "": fecha_fin_liquidacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     id_tipo_asignacion, id_tipo_operador, "","","","","", "", "", "", "", "" };
            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
                return ds;
        }
        /// <summary>
        /// Genera el reporte de Movimientos y Viajes pagados a un operador en el intervalo de tiempo solicitado
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía</param>
        /// <param name="fecha_inicio">Fecha de Inicio de la búsqueda</param>
        /// <param name="fecha_fin">Fecha de Fin de la búsqueda</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <returns></returns>
        public static DataSet ReportePagoMovimientosOperador(int id_compania_emisor, DateTime fecha_inicio, DateTime fecha_fin, int id_operador)
        {
            //Inicializando los parámetros de consulta 
            object[] parametros = { 2, id_compania_emisor, fecha_inicio== DateTime.MinValue ? "": fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                      fecha_fin== DateTime.MinValue ? "": fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), id_operador, "", 
                                     "", "", "", "", "", "","","","","", "", "", "", "", "" };
            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
                return ds;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de los Servicios y Movimientos
        /// </summary>
        /// <param name="id_recurso">Recurso Asignado</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación (Operador, Unidad, Proveedor)</param>
        /// <param name="fecha_liquidacion">Fecha de la Liquidación</param>
        /// <param name="id_estatus_liq">Estatus de la Asignación</param>
        /// <param name="id_liquidacion">Liquidación Actual</param>
        /// <returns></returns>
        public static DataTable ReporteServiciosMovimientosLiquidacion(int id_recurso, int id_tipo_asignacion, DateTime fecha_liquidacion, int id_estatus_asignacion, int id_liquidacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtServiciosMovimientos = null;
            
            //Inicializando los parámetros de consulta 
            object[] parametros = { 3, id_recurso, id_tipo_asignacion, fecha_liquidacion == DateTime.MinValue ? "": fecha_liquidacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     id_estatus_asignacion, id_liquidacion, "", "", "", "", "", "","","","","", "", "", "", "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores Obtenidos
                    dtServiciosMovimientos = ds.Tables["Table"];
            }    
                
            //Devolviendo Resultado Obtenido
            return dtServiciosMovimientos;
        }

        /// <summary>
        /// Genera el Reporte de la liquidaciones para la nueva Actualización de Nómina 1.2 
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="no_liquidacion">No Liquidación</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_tercero">Id Terrcero</param>
        /// <param name="id_estatus">Id Estatus</param>
        /// <param name="fecha_inicio_liquidacion">Fecha Inicio Liquidación</param>
        /// <param name="fecha_fin_liquidacion">Fecha Fin Liquidación</param>
        /// <param name="id_tipo_asignacion">Id Tipo Asignación</param>
        /// <param name="id_tipo_operador">Tipo de Operador</param>
        /// <returns></returns>
        public static DataSet ReporteLiquidaciones12(int id_compania_emisor, int no_liquidacion, int id_unidad, int id_operador, int id_tercero, byte id_estatus,
                                                  DateTime fecha_inicio_liquidacion, DateTime fecha_fin_liquidacion, byte id_tipo_asignacion, byte id_tipo_operador)
        {
            //Inicializando los parámetros de consulta 
            object[] parametros = { 4, id_compania_emisor, no_liquidacion, id_unidad, id_operador, id_tercero, id_estatus, 
                                     fecha_inicio_liquidacion== DateTime.MinValue ? "": fecha_inicio_liquidacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     fecha_fin_liquidacion == DateTime.MinValue ? "": fecha_fin_liquidacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     id_tipo_asignacion, id_tipo_operador, "","","","","", "", "", "", "", "" };
            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
                return ds;
        }
        /// <summary>
        /// Método que permite obtener las lecturas de Diesel y el calculo de Diesel 
        /// </summary>
        /// <param name="id_liquidacion">Identificador de una liquidación</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="fechaLiquidacion">Fecha de la liquidación</param>
        /// <returns></returns>
        public static DataSet ControlDiesel(int id_liquidacion, int id_operador, DateTime fechaLiquidacion)
        {            
            //Inicializando los parámetros de consulta 
            object[] parametros = { 5, id_liquidacion, id_operador, fechaLiquidacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))            
                return ds;
        }
        /// <summary>
        ///  Método encargado de Obtener el Reporte del Saldo por Detalle
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_cliente">Cliente del Servicio</param>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="fecha_inicio">Fecha de Inicio del Pago</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="solo_servicio">True para sólo mostrar detalles de Servicio</param>
        /// <param name="sin_proceso_revision">True para incluir detalles sin proceso de revisión registrado</param>
        /// <param name="en_revision">True para incluir detalles con proceso de revisión en curso</param>
        /// <param name="revision_terminada">True para incluir detalles con proceso de revisión terminado</param>
        /// <param name="factura_electronica">True para filtrar solo detalles facturados electronicamente</param>
        /// <param name="folio">Folio de la FE</param>
        /// <param name="id_estatus_cobro">Estatus de Cobro del registro facturado</param>
        /// <param name="no_servicio">No Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteControlAnticipos(int id_cliente, int id_servicio_maestro, DateTime fecha_inicio, DateTime fecha_fin, string no_servicio, string serie, int id_compania, int folio,
                                                               int id_proveedor, string serie_proveedor, string folio_proveedor,string UUID)
        {
            //Declarando Objeto de Retorno
            DataTable dtControlAnticipos = null;

            //Declarando Variables Auxiliares
            string fec_ini = "", fec_fin = "";
            //Obteniendo Valores
            fec_ini = fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
            fec_fin = fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
        
            //Inicializando los parámetros de consulta
            object[] param = { 6, id_cliente, fec_ini, fec_fin, no_servicio, serie, id_compania, folio, id_proveedor, 
                               serie_proveedor, folio_proveedor, UUID, id_servicio_maestro, "", "", "", "", "", "", "", ""};
            //Instanciando Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Validando que existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtControlAnticipos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtControlAnticipos;
        }
        #endregion

    }
}
