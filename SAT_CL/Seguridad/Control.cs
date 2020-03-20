using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Clase encargada se Cargar todos los controles
    /// </summary>
   public class Control
   {
       #region Propiedades y atributos

       /// <summary>
       ///  Define el nombre del store procedure encargado de realizar las acciones en BD
       /// </summary>
       private static string _nom_sp = "seguridad.sp_control_tc";

       #endregion

       #region Método Publicos
       

       /// <summary>
       /// Carga todos los controles
       /// </summary>
       /// <returns></returns>
       public static DataTable CargaAcciones()
       {
           //Definiendo objeto de retorno
           DataTable mit = null;

           //Inicialziando los parámetros de consulta
           object[] param = {4, 0, 0, 0, "", "", 0, 0, false, "", ""  };

           //Realizando la consulta
           using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
           {
               //Validando origen de datos
               if (Validacion.ValidaOrigenDatos(ds, "Table"))
                   //Asignando a objeto de retorno
                   mit = ds.Tables["Table"];

               //Devolviendo resultado
               return mit;
           }
       }
       

       /// <summary>
       /// Carga todos los controles
       /// </summary>
       /// <param name="id_accion">Id Accion</param>
       /// <returns></returns>
       public static DataTable CargaControles(byte id_accion)
       {
           //Definiendo objeto de retorno
           DataTable mit = null;

           //Inicialziando los parámetros de consulta
           object[] param = { 5, 0, 0, 0, "", "", id_accion, 0, false, "", "" };

           //Realizando la consulta
           using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
           {
               //Validando origen de datos
               if (Validacion.ValidaOrigenDatos(ds, "Table"))
                   //Asignando a objeto de retorno
                   mit = ds.Tables["Table"];

               //Devolviendo resultado
               return mit;
           }
       }
       #endregion
   }
}
