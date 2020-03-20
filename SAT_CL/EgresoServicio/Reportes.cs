using System.Data;
using System;
using System.Configuration;

namespace SAT_CL.EgresoServicio
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
        private static string nombre_store_procedure = "egresos_servicio.sp_reporte_modulo";

        #endregion

        #region Metodos estaticos


        /// <summary>
        /// Carga los ultimos depósitos es Estatus Por Liquidar y Liquidado
        /// </summary>
        /// <param name="id_operador"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static DataTable VisorUltimosDepositos(int id_operador, int id_unidad, int id_proveedor, int id_compania)
        {
            //Inicializando los parámetros de consulta
            object[] parametros = { 1, id_operador, id_unidad, id_proveedor, id_compania, "", "", "", "", "",
                                      "", "","", "", "","","","","","","" };

            //Cargando los registros de interés
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros).Tables[0])
                return mit;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Vales de Diesel
        /// </summary>
        /// <param name="id_compania">Compania</param>
        /// <param name="no_vale">Número de Vale</param>
        /// <param name="id_estacion_combustible">Estación de Combustible</param>
        /// <param name="fecha_carga_inicio">Fecha de Carga Inicial</param>
        /// <param name="fecha_carga_fin">Fecha de Fin </param>
        /// <returns></returns>
        public static DataTable ObtieneValesDiesel(int id_compania, string no_vale, int id_estacion_combustible, DateTime fecha_carga_inicio, DateTime fecha_carga_fin)
        {   //Declarando Objeto de Retorno
            DataTable dtVales = null;
            //Declarando Objeto de parametros
            object[] param = { 2, id_compania, no_vale, id_estacion_combustible, fecha_carga_inicio == DateTime.MinValue ? "" :fecha_carga_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                  fecha_carga_fin == DateTime.MinValue ? "":fecha_carga_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //Instanciando 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtVales = ds.Tables["Table"];
            }
            //Devolviendo Reporte Obtenido
            return dtVales;
        }

        /// <summary>
        /// Carga los anticipos pendientes (Vale de Diesel y Depósitos).
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor">Id Proveedor</param>
        /// <returns></returns>
        public static DataTable CargaAnticiposPendientes(int id_operador, int id_unidad, int id_proveedor)
        {
            //Inicializando los parámetros de consulta
            object[] parametros = { 3, id_operador, id_unidad, id_proveedor, "", "", "", "", "", "",
                                      "", "","", "", "","","","","","","" };

            //Cargando los registros de interés
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros).Tables[0])
                return mit;
        }


        /// <summary>
        /// Carga los anticipos de un recurso y viaje definido (Vales de Diesel y Depósitos).
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor">Id Proveedor</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaAnticiposRecursoServicio(int id_operador, int id_unidad, int id_proveedor, int id_servicio)
        {
            //Inicializando los parámetros de consulta
            object[] parametros = { 4, id_operador, id_unidad, id_proveedor, id_servicio, "", "", "", "", "",
                                      "", "","", "", "","","","","","","" };

            //Cargando los registros de interés
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros).Tables[0])
                return mit;
        }
        /// <summary>
        /// Carga los anticipos de un recurso y movimiento definido (Vales de Diesel y Depósitos).
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor">Id Proveedor</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <returns></returns>
        public static DataTable CargaAnticiposRecursoMovimiento(int id_operador, int id_unidad, int id_proveedor, int id_movimiento)
        {
            //Inicializando los parámetros de consulta
            object[] parametros = { 7, id_operador, id_unidad, id_proveedor, id_movimiento, "", "", "", "", "",
                                      "", "","", "", "","","","","","","" };

            //Cargando los registros de interés
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros).Tables[0])
                return mit;
        }
        /// <summary>
        /// Genera el Reporte de los Depósitos
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="no_deposito">No Depósito</param>
        /// <param name="id_estatus">Estatus del depósito</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_tercero">Id Tercero</param>
        /// <param name="identificacion">Identificación</param>
        /// <param name="efectivo">Efectivo</param>
        /// <param name="no_servicio">No Servicio</param>
        /// <param name="fecha_inicio_registro">Fecha de inicio del registró</param>
        /// <param name="fecha_fin_registro">Fecha fin del registró</param>
        /// <param name="fecha_inicio_depositado">Fecha de Inicio del depósito</param>
        /// <param name="fecha_fin_depositado">Fecha fin del depósito</param>
        /// <param name="id_estatus_liquidacion">Estatus Liquidación</param>
        /// <param name="id_comprobacion">Id Comprobacion</param>
        /// <returns></returns>
        public static DataSet ReporteDepositos(int id_compania_emisor,int no_deposito, byte id_estatus, int id_unidad, int id_operador, int id_tercero, string identificacion,
                                               int efectivo, string no_servicio, DateTime fecha_inicio_registro, DateTime fecha_fin_registro, DateTime fecha_inicio_depositado, 
                                               DateTime fecha_fin_depositado, byte id_estatus_liquidacion, int id_comprobacion, int id_concepto_deposito,string carta_porte,string referencias)
        {
            //Inicializando los parámetros de consulta
            object[] parametros = { 5, id_compania_emisor, no_deposito, id_estatus, id_unidad, id_operador, id_tercero, identificacion, efectivo, no_servicio, 
                                     fecha_inicio_registro == DateTime.MinValue ? "": fecha_inicio_registro.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     fecha_fin_registro == DateTime.MinValue? "" :  fecha_fin_registro.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                     fecha_inicio_depositado== DateTime.MinValue ? "": fecha_inicio_depositado.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     fecha_fin_depositado == DateTime.MinValue ? "": fecha_fin_depositado.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                     id_estatus_liquidacion, id_comprobacion, id_concepto_deposito,carta_porte,referencias,"","" };
                        
            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
                return ds;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de Vales de Diesel
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <param name="no_vale">Número de Vale</param>
        /// <param name="no_viaje">Número de Viaje</param>
        /// <param name="no_servicio">Número de Servicio</param>
        /// <param name="fecha_inicio_solicitud">Fecha de Inicio (Solicitud)</param>
        /// <param name="fecha_fin_solicitud">Fecha de Fin (Solicitud)</param>
        /// <param name="fecha_inicio_carga">Fecha de Inicio (Carga)</param>
        /// <param name="fecha_fin_carga">Fecha de Fin (Carga)</param>
        /// <param name="id_estacion_combustible">Estación de Combustible</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="unidad">Unidad Asignación de Cargo</param>
        /// <param name="unidad_diesel">Unidad que se le Asigno el Diesel</param>
        /// <returns></returns>
        public static DataTable ReporteValesDiesel(int id_compania_emisor, int id_cliente, string no_vale, string no_servicio, DateTime fecha_inicio_solicitud, DateTime fecha_fin_solicitud,
                                                    DateTime fecha_inicio_carga, DateTime fecha_fin_carga, int id_estacion_combustible, int id_unidad, int id_operador, 
                                                    int id_proveedor, byte id_estatus, string unidad, string unidad_diesel, string no_viaje)
        {
            //Declarando Objeto de Retorno
            DataTable dtValesDiesel = null;
            
            //Inicializando los parámetros de consulta 
            object[] parametros = { 6, id_compania_emisor, id_cliente, no_vale, no_servicio,
                                     fecha_inicio_solicitud == DateTime.MinValue ? "": fecha_inicio_solicitud.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     fecha_fin_solicitud == DateTime.MinValue ? "": fecha_fin_solicitud.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                     fecha_inicio_carga == DateTime.MinValue ? "": fecha_inicio_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                     fecha_fin_carga == DateTime.MinValue ? "": fecha_fin_carga.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                     id_estacion_combustible, id_unidad, id_operador, id_proveedor, id_estatus, unidad, unidad_diesel, no_viaje, "", "", "", "" };
            
            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando que Existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))

                    //Asignando Resultado Obtenido
                    dtValesDiesel = ds.Tables["Table"];
            }
                
            //Devolviendo Resultado Obtenido
            return dtValesDiesel;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de los Anticipos
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable ReporteAnticipos(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtAnticipos = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 8, id_servicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtAnticipos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtAnticipos;
        }

        /// <summary>
        /// Carga Anticipos Ligado a un Movimiento
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <returns></returns>
        public static DataTable CargaAnticiposMovimiento(int id_movimiento)
        {
            //Declarando Objeto de Retorno
            DataTable dtAnticipos = null;

            //Inicializando los parámetros de consulta
            object[] parametros = { 9, id_movimiento, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtAnticipos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtAnticipos;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Vales de Diesel
        /// </summary>
        /// <param name="id_compania">Compania</param>
        /// <param name="no_casetas">Número de Vale</param>
        /// <param name="id_estacion_combustible">Estación de Combustible</param>
        /// <param name="fecha_carga_inicio">Fecha de Carga Inicial</param>
        /// <param name="fecha_carga_fin">Fecha de Fin </param>
        /// <returns></returns>
        public static DataTable ObtieneCrucesIave(int id_compania, string no_casetas, string tag, DateTime fecha_carga_inicio, DateTime fecha_carga_fin)
        {   //Declarando Objeto de Retorno
            DataTable dtVales = null;
            //Declarando Objeto de parametros
            object[] param = { 10, id_compania, no_casetas, tag ,fecha_carga_inicio == DateTime.MinValue ? "" :fecha_carga_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                                  fecha_carga_fin == DateTime.MinValue ? "":fecha_carga_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //Instanciando 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtVales = ds.Tables["Table"];
            }
            //Devolviendo Reporte Obtenido
            return dtVales;
        }

        public static DataSet CargaGastosGenerales(int IdServicio)
        {
            //Inicializando los parámetros de consulta
            object[] parametros = { 11, IdServicio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Cargando los registros de interés
            using (DataSet dsGastosGenerales = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
                return dsGastosGenerales;
        }
        #endregion
    }
}
