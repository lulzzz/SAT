using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.FacturacionElectronica;

namespace SAT_CL.Facturacion
{
    /// <summary>
    /// Clase encargada de Gestionar Todos los Reportes del Módulo de Cuentas Por Cobrar
    /// </summary>
    public class Reporte : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        private static string _nom_sp = "facturacion.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Método Público encargado de Obtener las Facturas por Pagar
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <param name="descripcion_fac_global">Descripción</param>
        /// <param name="uuid">UUID</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="folio">Folio de CFDI</param>
        /// <param name="id_moneda">Id de Moneda Utilizada</param>
        /// <param name="bit_solo_facturas_timbradas">True para indicar que solo se debe mostrar elementos timbrados actualmente</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasPorPagar(int id_compania, int id_cliente, string descripcion_fac_global, string uuid, string referencia, int folio, int id_moneda, bool bit_solo_facturas_timbradas)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Arreglo de Parametros
            object[] param = { 1, id_compania, id_cliente, descripcion_fac_global, uuid, referencia, folio, id_moneda, Convert.ToByte(bit_solo_facturas_timbradas), "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                
                    //Asignando Resultado Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtFacturas;
        }


        /// <summary>
        /// Carga los datos de Facturado a partir del esquema de facturación electrónica para su importación.
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <param name="id_forma_pago">Forma de Pago</param>
        /// <param name="no_cta_pago">Cuenta de Pafo</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="tipo_comprobante">Tipo de Comprobante</param>
        /// <param name="referencias_viaje">Obtiene Referencias de Viaje a Mostrar</param>
        /// <returns></returns>
        public static DataSet ObtienesDatosFacturadoFacturaElectronica(int id_factura, byte id_forma_pago, int no_cta_pago, byte id_metodo_pago, byte tipo_comprobante, string referencias_viaje )
        {
            //Declarando Objeto de Retorno
            DataSet dsFactura = null;

            //Armando Arreglo de Parametros
            object[] param = { 2, id_factura, id_forma_pago, no_cta_pago, id_metodo_pago, referencias_viaje, tipo_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando  Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dsFactura = ds;
            }

            //Devolviendo Objeto de Retorno
            return dsFactura; 
        }
        /// <summary>
        /// Carga los datos de la Factura Global a detalle por concepto a partir del esquema de facturación electrónica para su importación.
        /// </summary>
        /// <param name="id_factura_global">Id Factura Global</param>
        /// <param name="id_forma_pago">Id Forma Pago</param>
        /// <param name="no_cta_pago">No Cuenta Pago</param>
        /// <param name="id_metodo_pago">Metodo de Pago</param>
        /// <param name="id_condiciones_pago">Condiciones de Pago</param>
        /// <param name="id_moneda">Id Moneda</param>
        /// <param name="fecha_cambio">Fecha de Cambio</param>
        /// <param name="referencias">Id Referencias de Viaje</param>
        /// <returns></returns>
        public static DataSet ObtienesDatosFacturaGlobalDetalleFacturaElectronica(int id_factura_global, byte id_forma_pago, int no_cta_pago, byte id_metodo_pago, byte id_condiciones_pago, byte id_moneda,
                                                                           DateTime fecha_cambio, string referencias)
        {
            //Declarando Objeto de Retorno
            DataSet dsFactura = null;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_factura_global, id_forma_pago, no_cta_pago, id_metodo_pago, id_condiciones_pago, id_moneda, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), referencias, "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando  Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dsFactura = ds;
            }

            //Devolviendo Objeto de Retorno
            return dsFactura; ;
        }
        /// <summary>
        /// Obtiene el reporte de ingreso por servicios en el periodo de tiempo señalado, agrupando por días, semanas, meses o años
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="fecha_inicial">Fecha inciial del periodo a consultar</param>
        /// <param name="fecha_final">Fecha final del periodo a consultar</param>
        /// <param name="agrupador">Criterio de agrupación del ingreso. Valores permitidos: "DIA", "SEMANA", "MES", "ANNO"</param>
        /// <returns></returns>
        public static DataTable CargaReporteIngresoServiciosPeriodo(int id_compania, DateTime fecha_inicial, DateTime fecha_final, string agrupador)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, id_compania, fecha_inicial.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), fecha_final.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), agrupador.ToUpper(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtFacturas = ds.Tables["Table"];

                //Devolviendo Objeto de Retorno
                return dtFacturas;
            }            
        }
        /// <summary>
        /// Carga los datos de la Factura Global general por concepto a partir del esquema de facturación electrónica para su importación.
        /// </summary>
        /// <param name="id_factura_global">Id Factura Global</param>
        /// <param name="id_forma_pago">Id Forma Pago</param>
        /// <param name="no_cta_pago">No Cuenta Pago</param>
        /// <param name="id_metodo_pago">Metodo de Pago</param>
        /// <param name="id_condiciones_pago">Condiciones de Pago</param>
        /// <param name="id_moneda">Id Moneda</param>
        /// <param name="fecha_cambio">Fecha de Cambio</param>
        /// <param name="rerferencias">Referencias</param>
        /// <returns></returns>
        public static DataSet ObtienesDatosFacturaGlobalGeneralFacturaElectronica(int id_factura_global, byte id_forma_pago, int no_cta_pago, byte id_metodo_pago, byte id_condiciones_pago, byte id_moneda,
                                                                           DateTime fecha_cambio, string referencias)
        {
            //Declarando Objeto de Retorno
            DataSet dsFactura = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, id_factura_global, id_forma_pago, no_cta_pago, id_metodo_pago, id_condiciones_pago, id_moneda, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), referencias, "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando  Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dsFactura = ds;
            }

            //Devolviendo Objeto de Retorno
            return dsFactura; ;
        }
        /// <summary>
        /// Carga los datos de la Factura Global general por concepto sin distinción de montos a partir del esquema de facturación electrónica para su importación.
        /// </summary>
        /// <param name="id_factura_global">Id Factura Global</param>
        /// <param name="id_forma_pago">Id Forma Pago</param>
        /// <param name="no_cta_pago">No Cuenta Pago</param>
        /// <param name="id_metodo_pago">Metodo de Pago</param>
        /// <param name="id_condiciones_pago">Condiciones de Pago</param>
        /// <param name="id_moneda">Id Moneda</param>
        /// <param name="fecha_cambio">Fecha de Cambio</param>
        /// <param name="rerferencias">Referencias</param>
        /// <returns></returns>
        public static DataSet ObtienesDatosFacturaGlobalGeneralConceptoFacturaElectronica(int id_factura_global, byte id_forma_pago, int no_cta_pago, byte id_metodo_pago, byte id_condiciones_pago, byte id_moneda,
                                                                           DateTime fecha_cambio, string referencias)
        {
            //Declarando Objeto de Retorno
            DataSet dsFactura = null;

            //Armando Arreglo de Parametros
            object[] param = { 10, id_factura_global, id_forma_pago, no_cta_pago, id_metodo_pago, id_condiciones_pago, id_moneda, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), referencias, "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando  Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dsFactura = ds;
            }

            //Devolviendo Objeto de Retorno
            return dsFactura; ;
        }

        /// <summary>
        /// Carga los datos de la Factura Global general por un solo concepto a partir del esquema de facturación electrónica para su importación.
        /// </summary>
        /// <param name="id_factura_global">Id Factura Global</param>
        /// <param name="id_forma_pago">Id Forma Pago</param>
        /// <param name="no_cta_pago">No Cuenta Pago</param>
        /// <param name="id_metodo_pago">Metodo de Pago</param>
        /// <param name="id_condiciones_pago">Condiciones de Pago</param>
        /// <param name="id_moneda">Id Moneda</param>
        /// <param name="fecha_cambio">Fecha de Cambio</param>
        /// <param name="no_identificacion">Identificación de los Conceptos</param>
        /// <returns></returns>
        public static DataSet ObtienesDatosFacturaGlobalUnConceptoFacturaElectronica(int id_factura_global, byte id_forma_pago, int no_cta_pago, byte id_metodo_pago, byte id_condiciones_pago, byte id_moneda,
                                                                           DateTime fecha_cambio, string no_identificacion)
        {
            //Declarando Objeto de Retorno
            DataSet dsFactura = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, id_factura_global, id_forma_pago, no_cta_pago, id_metodo_pago, id_condiciones_pago, id_moneda, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), no_identificacion, "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando  Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    DataRow[] noIdentificacion =
                    ds.Tables[1].Select("No_identificacion = ' '");
                    noIdentificacion[0]["No_identificacion"] = no_identificacion;
                    //Obtenemos 
                    //Asignando Resultado Obtenido
                    dsFactura = ds;
                }
            }

            //Devolviendo Objeto de Retorno
            return dsFactura; ;
        }


        /// <summary>
        /// Obtiene las Referencias de Viaje de Facturado
        /// </summary>
        /// <param name="id_factura"></param>
        /// <returns></returns>
        public static DataTable ObtienesDatosReferenciasFacturado(int id_factura)
        {
            //Declarando Objeto de Retorno
            DataTable dtReferencias = null;

            //Armando Arreglo de Parametros
            object[] param = { 7, id_factura, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtReferencias = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtReferencias; 
        }

        /// <summary>
        /// Obtiene las Referencias de Viajes de una Factura Global
        /// </summary>
        /// <param name="id_factura"></param>
        /// <returns></returns>
        public static DataTable ObtienesDatosReferenciasFacturaGlobal(int id_factura, int id_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtReferencias = null;

            //Armando Arreglo de Parametros
            object[] param = { 8, id_factura, id_compania, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtReferencias = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtReferencias;
        }

        /// <summary>
        /// Obtiene las montos de conceptos principales de cobro del servicio solicitado por una referencia dada
        /// </summary>
        /// <param name="referencia">Número de referencia a buscar</param>
        /// <param name="id_compania">Id de Compañía a la que pertenece la referencia</param>
        /// <returns></returns>
        public static DataTable ObtenerCargosPrincipalesServicio(int no_servicio, string referencia, int id_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtReferencias = null;

            //Armando Arreglo de Parametros
            object[] param = { 9, id_compania, referencia, no_servicio.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtReferencias = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtReferencias;
        }
        /// <summary>
        /// Método encargado de Carga las Facturas de Otros registradas y timbradas en Facturación Electronica
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="serie">Serie</param>
        /// <param name="folio">Folio</param>
        /// <param name="uuid">UUID (Folio Fiscal)</param>
        /// <param name="inicio">Inicio de Expedición</param>
        /// <param name="termino">Termino de Expedición</param>
        /// <param name="referencia">Datos Adicionales</param>
        /// <returns></returns>
        public static DataTable CargaFacturacionOtrosCFDI(int id_compania, int id_cliente, string serie, int folio, string uuid, DateTime inicio, DateTime termino, string referencia)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacOtros = null;

            //Armando Arreglo de Parametros
            object[] param = { 11, id_compania, id_cliente, serie, folio, uuid,
                               inicio == DateTime.MinValue ? "" : inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               termino == DateTime.MinValue ? "" : termino.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               referencia, "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtFacOtros = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtFacOtros;
        }
        /// <summary>
        /// Método encargado de Obtener los Comprobantes Timbrados como Nota de Credito
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_cliente">Cliente por Filtrar</param>
        /// <param name="serie">Serie</param>
        /// <param name="folio">Folio</param>
        /// <param name="fecha_exp_ini">Fecha de Expedición (Inicio)</param>
        /// <param name="fecha_exp_fin">Fecha de Expedición (Termino)</param>
        /// <returns></returns>
        public static DataTable ObtieneCfdiNotasCredito(int id_compania, int id_cliente, string serie, int folio, DateTime fecha_exp_ini, DateTime fecha_exp_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtNotasCredito = null;

            //Armando Arreglo de Parametros
            object[] param = { 12, id_compania, id_cliente, serie, folio, Fecha.ConvierteDateTimeString(fecha_exp_ini, "yyyyMMdd HH:mm"),
                               Fecha.ConvierteDateTimeString(fecha_exp_fin, "yyyyMMdd HH:mm"), "", "", "", "", "", "", "",
                               "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtNotasCredito = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtNotasCredito;
        }
        /// <summary>
        /// Método encargado de 
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <returns></returns>
        public static DataTable ObtieneCfdiAmparadosNC(int id_comprobante)
        {
            //Declarando Objeto de Retorno
            DataTable dtCfdisAmparados = null;

            //Armando Arreglo de Parametros
            object[] param = { 13, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtCfdisAmparados = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtCfdisAmparados;
        }

        #endregion
    }
}
