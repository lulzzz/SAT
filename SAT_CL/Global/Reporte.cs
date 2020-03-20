using System.Data;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargado de administrar los Reportes
    /// </summary>
    public static class Reporte
    {
        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nom_sp = "global.sp_reporte_modulo";

        #endregion

        #region Metodos estaticos


        /// <summary>
        /// Reporte vencimientos
        /// </summary>
        /// <param name="id_tipo">Tipo de Vencimientp</param>
        /// <param name="dentificador">Descripción del registro</param>
        /// <param name="id_estatus">Id estatus del Vencimiento</param>
        /// <returns></returns>
        public static DataSet ReporteVencimientos(int id_tipo, string identificador, byte id_estatus)
        {
           
            //Inicializando los parámetros de consulta
            object[] parametros = { 1, id_tipo, id_estatus, identificador, "", "", "", 
                                     "", "", 
                                     "", "", "", "", "", "","","","","","","" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
                return ds;
        }

        /// <summary>
        /// Reporte vencimientos
        /// </summary>
        /// <param name="id_tipo">Tipo de Vencimientp</param>
        /// <param name="dentificador">Descripción del registro</param>
        /// <param name="id_estatus">Id estatus del Vencimiento</param>
        /// <returns></returns>
        public static DataSet CargaKilometrajes(int id_compania, int id_servicio_bandera, int id_unidad, int id_ubicacion, int id_servicio)
        {

            //Inicializando los parámetros de consulta
            object[] parametros = { 2, id_compania, id_servicio_bandera, id_unidad, id_ubicacion,id_servicio,"",
                                     "", "", "", "", "", "", "", "","","","","","","" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
                return ds;
        }

        #endregion
    }
}
