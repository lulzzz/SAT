using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAT_CL.Global
{
    /// <summary>
    /// Proporciona los medios para la carga de un reporte de compañía
    /// </summary>
    public static class CompaniaReporteConsulta
    {
        #region Atributos

        /// <summary>
        /// Nombre del SP que ejecutará los reportes de compañía
        /// </summary>
        private static string _nombre_stored_procedure = "global.sp_compania_reporte_consulta_tcr";

        #endregion


        #region Métodos

        /// <summary>
        /// Ejecuta el reporte solicitado
        /// </summary>
        /// <param name="id_reporte">Id de Reporte</param>
        /// <param name="fecha_inicio">Fecha inicial del reporte</param>
        /// <param name="fecha_fin">Fecha final del reporte</param>
        /// <returns></returns>
        public static DataTable EjecutaReporteCompania(int id_reporte, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Creando arreglo de parámetros
            object[] param = { id_reporte, TSDK.Base.Fecha.ConvierteDateTimeString(fecha_inicio, System.Configuration.ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                             TSDK.Base.Fecha.ConvierteDateTimeString(fecha_fin, System.Configuration.ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                             "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si existen datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return mit;
        }

        #endregion
    }
}
