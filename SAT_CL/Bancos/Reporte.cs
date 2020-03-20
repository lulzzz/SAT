using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;
namespace SAT_CL.Bancos
{
    /// <summary>
    /// Implementa la clase de Reportes
    /// </summary>
    public class Reporte
    {
        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nom_sp = "bancos.sp_reporte_modulo";

        #endregion

        #region Metodos estaticos


        /// <summary>
        /// Reporte Egresos
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="no_egreso">No Egreso</param>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="beneficiario">Beneficiario</param>
        /// <param name="id_cuenta_origen">Id Cuenta Origen</param>
        /// <param name="id_estatus">Id Estatus</param>
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="fecha_fin">Fecha Fin</param>
        /// <returns></returns>
        public static DataSet ReporteEgresos(int id_compania_emisor, int no_egreso, int id_concepto, string beneficiario, int id_cuenta_origen, byte id_estatus, DateTime fecha_inicio,
                                                   DateTime fecha_fin)
        {
            //Inicializando los parámetros de consulta
            object[] parametros = { 1, id_compania_emisor, no_egreso, id_concepto, beneficiario, id_cuenta_origen, id_estatus, 
                                     fecha_inicio == DateTime.MinValue ? "": fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     fecha_fin == DateTime.MinValue ?"": fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                     "", "", "", "", "", "","","","","","","" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
                return ds;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de las Fichas de Ingreso
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <param name="no_ficha">No. de Ficha</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="id_concepto">Concepto</param>
        /// <param name="id_cta_origen">Cuenta de Origen</param>
        /// <param name="id_cta_destino">Cuenta de Destino</param>
        /// <param name="id_moneda">Moneda</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteFichasIngreso(int id_compania_emisor, int no_ficha, byte id_estatus, int id_tipo_entidad, int id_registro,
                                            byte id_metodo_pago, int id_concepto, int id_cta_origen, int id_cta_destino, byte id_moneda,
                                            DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasIngreso = null;

            //Inicializando los parámetros de consulta
            object[] param = { 2, id_compania_emisor, id_estatus, no_ficha, id_tipo_entidad, id_registro, id_metodo_pago, id_concepto, id_cta_origen, id_cta_destino, id_moneda,
                               fecha_inicio == DateTime.MinValue ? "": fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fecha_fin == DateTime.MinValue ?"": fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               "", "", "", "", "", "","","" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFichasIngreso = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFichasIngreso;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de Aplicación de Fichas
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="no_ficha">No Ficha</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteAplicacionFichas(int id_compania_emisor, DateTime fecha_inicio, DateTime fecha_fin, string no_ficha)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasIngreso = null;

            //Inicializando los parámetros de consulta
            object[] param = { 3, id_compania_emisor, fecha_inicio == DateTime.MinValue ? "": fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fecha_fin == DateTime.MinValue ?"": fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), no_ficha, "", "", "", "", "", "",
                               "", "", "", "", "", "", "", "","","" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFichasIngreso = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFichasIngreso;
        }

        /// <summary>
        /// Método encargado de Obtener el Reporte de Egreso e Ingresos de una Cuenta.
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_cuenta">Cuenta</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteEgresoIngreso(int id_compania_emisora, int id_cuenta, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasIngreso = null;

            //Inicializando los parámetros de consulta
            object[] param = { 4, id_compania_emisora, id_cuenta, fecha_inicio == DateTime.MinValue ? "": fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fecha_fin == DateTime.MinValue ?"": fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "",
                               "", "", "", "", "", "", "", "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFichasIngreso = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFichasIngreso;
        }

        /// <summary>
        /// Método encargado de Obtener el Reporte de Egreso Rechazados por tipo y beneficiario.
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_tipo_deposito">Id de Tipo de Depósito</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_proveedor">Id de Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteDepositosRechazados(int id_compania_emisora, byte id_tipo_deposito, DateTime fecha_inicio, DateTime fecha_fin, int id_operador, int id_unidad, int id_proveedor)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasIngreso = null;

            //Inicializando los parámetros de consulta
            object[] param = { 5, id_compania_emisora, id_tipo_deposito, fecha_inicio == DateTime.MinValue ? "": fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fecha_fin == DateTime.MinValue ?"": fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), id_operador, id_unidad, id_proveedor, "", "", "", "",
                               "", "", "", "", "", "", "", "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFichasIngreso = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFichasIngreso;
        }

        #endregion
    }
}
