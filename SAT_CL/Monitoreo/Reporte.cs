using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Datos;

namespace SAT_CL.Monitoreo
{
    /// <summary>
    /// Clase encargada de Obtener los Reportes del Modulo de Monitoreo.
    /// </summary>
    public class Reporte
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "monitoreo.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Obtener el Historial de Evaluaciones de una Entidad
        /// </summary>
        /// <param name="id_tabla">Entidad (19.- Unidad)</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="fec_bit_ini">Fecha de Inicio (Bitacora)</param>
        /// <param name="fec_bit_fin">Fecha de Termino (Bitacora)</param>
        /// <param name="fec_eva_ini">Fecha de Inicio (Evaluacion)</param>
        /// <param name="fec_eva_fin">Fecha de Termino (Evaluacion)</param>
        /// <returns></returns>
        public static DataTable ObtieneHistorialEvaluaciones(int id_tabla, int id_registro, DateTime fec_bit_ini, DateTime fec_bit_fin,
                                                             DateTime fec_eva_ini, DateTime fec_eva_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtEvaluaciones = null;

            //Inicializando los parámetros de consulta
            object[] param = { 1, id_tabla, id_registro, 
                               fec_bit_ini == DateTime.MinValue ? "" : fec_bit_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fec_bit_fin == DateTime.MinValue ? "" : fec_bit_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fec_eva_ini == DateTime.MinValue ? "" : fec_eva_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               fec_eva_fin == DateTime.MinValue ? "" : fec_eva_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                               "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores
                    dtEvaluaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtEvaluaciones;
        }

        #endregion
    }
}
