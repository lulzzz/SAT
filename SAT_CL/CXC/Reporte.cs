using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SAT_CL.CXC
{
    public class Reporte
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "cxc.sp_reporte_modulo";


        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Obtener el Reporte de los Saldos Globales
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_cliente">Cliente del Servicio</param>
        /// <param name="fecha_inicio">Fecha de Inicio del Pago</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="facturacion_electronica">Filtrado Solo la facturado Electrónicamente</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteSaldoGlobal(int id_compania, int id_cliente, DateTime fecha_inicio, DateTime fecha_fin, byte facturacion_electronica)
        {
            //Declarando Objeto de Retorno
            DataTable dtSaldosGlobales = null;

            //Declarando Variables Auxiliares
            string fec_ini = "", fec_fin = "";

            //Obteniendo Valores
            fec_ini = fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
            fec_fin = fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);

            //Inicializando los parámetros de consulta
            object[] param = { 1, id_compania, fec_ini, fec_fin, id_cliente, facturacion_electronica, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Instanciando Reporte
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        public static DataTable ObtieneReporteSaldoDetalle(int id_compania, int id_cliente, DateTime fecha_inicio, DateTime fecha_fin, string referencia, bool solo_servicio, bool sin_proceso_revision, bool en_revision, bool revision_terminada, byte factura_electronica,
                                                          int folio, string id_estatus_cobro, string no_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtSaldosDetalle = null;

            //Declarando Variables Auxiliares
            string fec_ini = "", fec_fin = "";

            //Obteniendo Valores
            fec_ini = fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
            fec_fin = fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);

            //Inicializando los parámetros de consulta
            object[] param = { 2, id_cliente, fec_ini, fec_fin, id_compania, referencia, solo_servicio ? 1 : 0, sin_proceso_revision ? 1 : 0, en_revision ? 1 : 0, revision_terminada ? 1 : 0, factura_electronica, folio, id_estatus_cobro, no_servicio, "", "", "", "", "", "", "" };

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
            object[] param = { 3, id_cliente, "", id_compania, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

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
        ///  Método encargado de Obtener el Reporte de Servicios por Facturar
        /// </summary>
        /// <param name="id_cliente">Cliente de la Compania</param>
        /// <param name="fecha_inicio">Fecha Inicio de la Factura</param>
        /// <param name="fecha_fin">Fecha Fin de la Factura</param>
        /// <param name="folio">Folio</param>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="no_servicio">No Servicio</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="sin_registrar">Servicios sin registrar en FE</param>
        /// <param name="registrado">Servicios registrados en FE</param>
        /// <param name="timbrado">Servicios timbrados</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteServiciosxFacturar(int id_cliente, DateTime fecha_inicio, DateTime fecha_fin, int folio, int id_compania_emisor, 
                                                    string referencia, string no_servicio, string porte, bool sin_registrar, bool registrado, bool timbrado)
        {
            //Declarando Objeto de Retorno
            DataTable dtSaldosDetalle = null;

            //Declarando Variables Auxiliares
            string fec_ini = "", fec_fin = "";

            //Obteniendo Valores
            fec_ini = fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
            fec_fin = fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);

            //Inicializando los parámetros de consulta
            object[] param = { 4, id_cliente, fec_ini, fec_fin, folio, id_compania_emisor, referencia, no_servicio, porte, sin_registrar ? 1 : 0, registrado ? 1 : 0, timbrado ? 1 : 0, 
                                "", "", "", "", "", "", "", "", "" };

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
        ///  Método encargado de Obtener el Reporte de Servicios por Facturar v3.3
        /// </summary>
        /// <param name="id_cliente">Cliente de la Compania</param>
        /// <param name="fecha_inicio">Fecha Inicio de la Factura</param>
        /// <param name="fecha_fin">Fecha Fin de la Factura</param>
        /// <param name="folio">Folio</param>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="no_servicio">No Servicio</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="sin_registrar">Servicios sin registrar en FE</param>
        /// <param name="registrado">Servicios registrados en FE</param>
        /// <param name="timbrado">Servicios timbrados</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteServiciosxFacturarV3_3(int id_cliente, DateTime fecha_inicio, DateTime fecha_fin, int folio, int id_compania_emisor,
                                                    string referencia, string no_servicio, int no_economico, string porte, bool sin_registrar, bool registrado, bool timbrado)
        {
            //Declarando Objeto de Retorno
            DataTable dtSaldosDetalle = null;

            //Declarando Variables Auxiliares
            string fec_ini = "", fec_fin = "";

            //Obteniendo Valores
            fec_ini = fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);
            fec_fin = fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]);

            //Inicializando los parámetros de consulta
            object[] param = { 5, id_cliente, fec_ini, fec_fin, folio, id_compania_emisor, referencia, no_servicio, porte, sin_registrar ? 1 : 0, registrado ? 1 : 0, timbrado ? 1 : 0, 
                                "", "", no_economico, "", "", "", "", "", "" };

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


        #endregion
    }
}
