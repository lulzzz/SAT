using System;
using System.Data;
using System.Configuration;

namespace SAT_CL.CXP
{   
    /// <summary>
    /// Clase encargada de Almacenar todos los reportes del Modulo de CXP
    /// </summary>
    public static class Reportes
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "cxp.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Método Público que Obtiene las Facturas de Diesel
        /// </summary>
        /// <returns></returns>
        public static DataTable ObtieneReporteFacturasDiesel(string id_proveedor, string id_compania, string serie, int folio, string uuid)
        {   
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;
            
            //Armando Objeto de Parametros
            object[] param = { 1, id_proveedor, id_compania, serie, folio.ToString(), uuid, "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};
            
            //Obteniendo Reporte
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Reporte Obtenido
                    dtFacturas = ds.Tables["Table"];
            }
            
            //Devolviendo Objeto de Retorno
            return dtFacturas;
        }
        /// <summary>
        /// Método encargado de Obtener las Facturas por Aplicar uno o varios Pagos
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_proveedor">Proveedor de la Factura</param>
        /// <param name="uuid">UUID</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasPorAplicar(int id_compania, int id_proveedor, string uuid, int folio)
        {
            //Declarando objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Objeto de Parametros
            object[] param = { 2, id_proveedor, id_compania, uuid, folio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Reporte Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtFacturas;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Facturas por Pagar
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasPorPagar(int id_compania)
        {
            //Declarando objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Objeto de Parametros
            object[] param = { 3, id_compania, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Reporte Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtFacturas;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Facturas Pagadas
        /// </summary>
        /// <param name="id_compania">Compania Receptora</param>
        /// <param name="id_proveedor">Proveedor Emisor</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasPagadas(int id_compania, int id_proveedor)
        {
            //Declarando objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Objeto de Parametros
            object[] param = { 4, id_compania, id_proveedor, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Reporte Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtFacturas;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de los Saldos Globales
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="fecha_inicio">Fecha de Inicio del Pago</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteSaldoGlobal(int id_compania, int id_cliente, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtSaldosGlobales = null;

            //Declarando Variables Auxiliares
            string fec_ini = "", fec_fin = "";

            //Obteniendo Valores
            fec_ini = fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
            fec_fin = fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);

            //Inicializando los parámetros de consulta
            object[] param = { 5, id_compania, fec_ini, fec_fin, id_cliente, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Instanciando Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtSaldosGlobales = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtSaldosGlobales;
        }
        /// <summary>
        ///  Método encargado de Obtener el Reporte del Saldo por Detalle
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_cliente">Cliente del Servicio</param>
        /// <param name="fecha_inicio">Fecha de Inicio del Pago</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_estatus">Id de Estatus de pago de la factura</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteSaldoDetalle(int id_compania, int id_cliente, DateTime fecha_inicio, DateTime fecha_fin,
                                                           byte id_estatus, string serie, string folio, string uuid)
        {
            //Declarando Objeto de Retorno
            DataTable dtSaldosDetalle = null;

            //Declarando Variables Auxiliares
            string fec_ini = "", fec_fin = "";

            //Obteniendo Valores
            fec_ini = fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
            fec_fin = fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);

            //Inicializando los parámetros de consulta
            object[] param = { 6, id_cliente, fec_ini, fec_fin, id_compania, id_estatus, serie, folio, uuid, "", "", "", "", "", "", "", "", "", "", "", "" };

            //Instanciando Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtSaldosDetalle = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtSaldosDetalle;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de Saldos por Periodo
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteSaldosPeriodo(int id_compania, int id_cliente)
        {
            //Declarando Objeto de Retorno
            DataTable dtSaldosPeriodo = null;

            //Inicializando los parámetros de consulta
            object[] param = { 7, id_cliente, "", id_compania, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Instanciando Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtSaldosPeriodo = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtSaldosPeriodo;
        }
        /// <summary>
        /// Método encargado de Obtener las Facturas de Anticipos a Proveedores
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="serie">Serie</param>
        /// <param name="uuid">UUID</param>
        /// <param name="folio">Folio</param>
        /// <param name="id_entidad">Entidad Ligada a la Factura</param>
        /// <param name="id_registro">Registro Ligado a la factura</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasAnticipoProveedor(int id_compania, int id_proveedor, string serie, string uuid, int folio, int id_entidad, int id_registro)
        {
            //Declarando objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Objeto de Parametros
            object[] param = { 8, id_compania, id_proveedor, serie, uuid, folio, id_entidad, id_registro, "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Reporte Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtFacturas;
        }
        /// <summary>
        /// Método encargado de Obtener las Facturas Disponibles para Liquidación
        /// </summary>
        /// <param name="id_compania">Compania Receptora</param>
        /// <param name="id_proveedor">Proveedor de la Factura</param>
        /// <param name="serie">Serie de la Factura</param>
        /// <param name="folio">Folio de la Factura</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasDisponiblesEntidad(int id_compania, int id_proveedor, string serie, int folio, string uuid)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturasDisponibles = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la insercion de un registro a base de datos
            object[] param = { 9, id_compania, id_proveedor, serie, folio, uuid, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Instanciando Reporte de Facturas
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Tabla
                    dtFacturasDisponibles = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacturasDisponibles;
        }

        /// <summary>
        /// Obtiene el conjunto de facturas de proveedor y su detalle de aplicación de pagos
        /// </summary>
        /// <param name="id_compania">Id de Compañía Receptora de las facturas</param>
        /// <param name="id_proveedor">Id de proveedor que generó la factura</param>
        /// <param name="serie">No. de Serie utilizado por el proveedor</param>
        /// <param name="folio">No. de Folio utilizado por el proveedor</param>
        /// <param name="uuid">Alguno o todos los caracteres que conforman el Identificador ante el SAT para esta factura</param>
        /// <param name="fecha_inicio_fac">Fecha inicial del periodo de facturación del proveedor a consultar</param>
        /// <param name="fecha_fin_fac">Fecha final del periodo de facturación del proveedor a consultar</param>
        /// <param name="no_servicio">No. de Servicio</param>
        /// <param name="no_viaje">No. de Identificación del viaje del Cliente</param>
        /// <param name="carta_porte">No. de Carta Porte</param>
        /// <param name="no_anticipo">No. de Anticipo de Viaje</param>
        /// <param name="no_liquidacion">No. de Liquidación</param>
        /// <param name="estatus">Conjunto de estatus solicitados</param>
        /// <returns></returns>
        public static DataTable ObtieneIntegracionFacturasProveedor(int id_compania, int id_proveedor, string serie, string folio, string uuid, DateTime fecha_inicio_fac, DateTime fecha_fin_fac,
                                    string no_servicio, string no_viaje, string carta_porte, string no_anticipo, string no_liquidacion, string  estatus)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Generando arereglo de parámetros de consulta
            object[] param = { 10, id_compania, id_proveedor, serie, folio, uuid, TSDK.Base.Fecha.ConvierteDateTimeString(fecha_inicio_fac, ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 TSDK.Base.Fecha.ConvierteDateTimeString(fecha_fin_fac, ConfigurationManager.AppSettings["FormatoFechaReportes"]), no_servicio, no_viaje, carta_porte, no_anticipo, no_liquidacion, 
                                 estatus, "", "", "", "", "", "", "" };

            //Buscando coincidencias
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existen registros coincidentes
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    mit = ds.Tables["Table"];
                }
            }

            //Devolviendo resutado 
            return mit;
        }



        ///// <summary>
        ///// Obtiene el conjunto de facturas de proveedor y su detalle de aplicación de pagos
        ///// </summary>
        ///// <param name="id_proveedor">Id de proveedor que generó la factura</param>
        ///// <param name="serie">No. de Serie utilizado por el proveedor</param>
        ///// <param name="folio">No. de Folio utilizado por el proveedor</param>
        ///// <param name="uuid">Alguno o todos los caracteres que conforman el Identificador ante el SAT para esta factura</param>
        ///// <returns></returns>
        //public static DataTable ObtieneCuentasPorPagar(int id_proveedor, int serie, int folio, string uuid)
        //{
        //    //Definiendo objeto de retorno
        //    DataTable dtPruebas = null;

        //    //Generando arereglo de parámetros de consulta
        //    object[] param = { 11, id_proveedor, serie, folio, uuid, "", "", "",
        //                         "", "", "", "", "", "", "",
        //                         "", "", "", "", "", "", "", ""  };

        //    //Buscando coincidencias
        //    using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
        //    {
        //        //Si existen registros coincidentes
        //        if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
        //        {
        //            dtPruebas = ds.Tables["Table"];
        //        }
        //    }

        //    //Devolviendo resutado 
        //    return dtPruebas;
        //}


        /// <summary>
        /// Método encargado de Obtener las Facturas por Aplicar uno o varios Pagos
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_proveedor">Proveedor de la Factura</param>
        /// <param name="uuid">UUID</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasPorPagar(int id_proveedor, string uuid, string folio, string serie, string param5)
        {
            //Declarando objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Objeto de Parametros
            object[] param = { 11, id_proveedor, serie, uuid, folio, param5, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Reporte Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtFacturas;
        }
        /// <summary>
        /// Método Público que Obtiene las Facturas de IAVE
        /// </summary>
        /// <returns></returns>
        public static DataTable ObtieneReporteFacturasIAVE(string id_proveedor, string id_compania, string serie, int folio, string uuid)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Objeto de Parametros
            object[] param = { 12, id_proveedor, id_compania, serie, folio.ToString(), uuid, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Reporte Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtFacturas;
        }
        #endregion 

    }
}
