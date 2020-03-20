using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones del Visor de Registros
    /// </summary>
    public class VisorRegistros
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del Store Procedure
        /// </summary>
        private static string _nom_sp = "global.sp_visorRegistros";

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público Encargado de Realizar la carga de registros de la Tabla solicitada conforme a los filtros Definidos
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Param1">Parametro Auxiliar 1</param>
        /// <param name="Param2">Parametro Auxiliar 2</param>
        /// <param name="Param3">Parametro Auxiliar 3</param>
        /// <param name="Param4">Parametro Auxiliar 4</param>
        /// <param name="Param5">Parametro Auxiliar 5</param>
        /// <param name="Param6">Parametro Auxiliar 6</param>
        /// <param name="Param7">Parametro Auxiliar 7</param>
        /// <returns></returns>
        public static DataSet CargaRegistrosTabla(int idTabla, string Param1, string Param2, string Param3, string Param4, string Param5, string Param6, string Param7)
        {   //Declarando Objeto de Parametros
            object[] param = { idTabla, Param1, Param2, Param3, Param4, Param5, Param6, Param7 };
            //Instanciando 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Método Público Encargado de Realizar la carga de registros de la Tabla solicitada conforme a los filtros Definidos
        /// </summary>
        /// <param name="filtros">Arreglo de Parametros por Filtrar</param>
        /// <returns></returns>
        public static DataSet CargaRegistrosTabla(object[] filtros)
        {  using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, filtros))
                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Método Público Encargado de Realizar la carga de registros de la Tabla solicitada conforme a los filtros Definidos
        /// </summary>
        /// <param name="tabla">Id de Tabla</param>
        /// <param name="param1">Parametro Auxiliar 1</param>
        /// <param name="param2">Parametro Auxiliar 2</param>
        /// <param name="param3">Parametro Auxiliar 3</param>
        /// <param name="param4">Parametro Auxiliar 4</param>
        /// <param name="param5">Parametro Auxiliar 5</param>
        /// <param name="param6">Parametro Auxiliar 6</param>
        /// <param name="param7">Parametro Auxiliar 7</param>
        /// <param name="gv">GridView donde se cargarán los resultados</param>
        /// <param name="datakeys">Colección de llaves separadas por el caracter '-'</param>
        /// <returns></returns>
        public static DataSet CargaRegistrosTabla(int tabla, string param1, string param2, string param3, string param4, string param5, string param6, string param7, GridView gv, string datakeys)
        {   //Inicializando parametros
            object[] param = { tabla, param1, param2, param3, param4, param5, param6, param7 };
            //Instanciado dataset con resultado de la consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Cargando el grid
                TSDK.ASP.Controles.CargaGridView(gv, DS, "Table", datakeys, "", true, 0);
                //Regresando el reusltado
                return DS;
            }
        }

        #endregion
    }
}
