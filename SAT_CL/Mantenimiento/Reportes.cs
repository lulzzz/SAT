using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TSDK.Datos;
using System.Configuration;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    ///  Clase encargada de Gestionar Todos los Reportes del Módulo de Mantenimiento
    /// </summary>
    public class Reportes
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el nombre de stored procedure 
        /// </summary>
        private static string _nom_sp = "mantenimiento.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Mètodo encargado de cargar las Actividades por nAsignar
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="no_orden">No Orden</param>
        /// <param name="id_estatus">Id Estatus</param>
        /// <param name="id_tipo_unidad">Id Tipo Unidad</param>
        /// <param name="id_sub_tipo_unidad">Id Sub Tipo Unidad</param>
        /// <param name="unidad">Unidad</param>
        /// <param name="id_familia">Id Familia</param>
        /// <param name="id_sub_familia">Id Sub Familia</param>
        /// <param name="fecha_inicio_orden_trabajo">Fecha de Inicio de Orden de Trabajo</param>
        /// <param name="fecha_fin_orden_trabajo">Fecha Fin de Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable CargaActividadesPorAsignar(int id_compania_emisor, string no_orden, string id_estatus, int id_tipo_unidad, byte id_sub_tipo_unidad,
                                              string  unidad, byte id_familia, byte id_sub_familia, DateTime fecha_inicio_orden_trabajo, DateTime fecha_fin_orden_trabajo)
        {
            //Declarando Objeto retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 1, id_compania_emisor, no_orden, id_estatus, id_tipo_unidad, id_sub_tipo_unidad,
                                  unidad, id_familia, id_sub_familia, fecha_inicio_orden_trabajo == DateTime.MinValue ? "" : fecha_inicio_orden_trabajo.ToString("dd/MM/yyyy HH:mm"), fecha_fin_orden_trabajo == DateTime.MinValue ? "" : fecha_fin_orden_trabajo.ToString("dd/MM/yyyy HH:mm"), "",  "", "", "", "", "", "", "", "", "", };
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return mit;
        }
        /// <summary>
        /// Método encargado de Obtener las Requisiciones ligadas a la Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable ObtieneRequisicionesOrdenTrabajo(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 2, id_orden_trabajo, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Valor Obtenido
                    dtRequisiciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisiciones;
        }
        /// <summary>
        /// Método encargado de Obtener las Asignaciones ligadas a la Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable ObtieneAsignacionesOrdenTrabajo(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtAsignaciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_orden_trabajo, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtAsignaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtAsignaciones;
        }
        /// <summary>
        /// Método que obtiene las ordenes de trabajo acorde a parametros de busqueda
        /// </summary>
        /// <param name="orden_trabajo">Realiza la busqueda por No de Orden Trabajo</param>
        /// <param name="estatus">Realiza la busqueda por estatus de la orden de trabajo</param>
        /// <param name="cliente">Realiza la busueda por el cliente al cual se brindo el servicio</param>
        /// <param name="proveedor">Realiza la busqueda por el proveedor que realizo el mantenimiento</param>
        /// <param name="empleado">Realiza la busqueda por el empleado que llevo acabo la actividad de mantenimiento</param>
        /// <param name="fecha_ini_sol">Realiza la busqueda de inicio de un rango de fechas de solicitud de Orden de Trabajo</param>
        /// <param name="fecha_fin_sol">Realiza la busqueda de fin de un rango de fechas de solicitud de Orden de Trabajo</param>
        /// <param name="fecha_ini_ent">Realiza la busqueda de inicio de un rango de fechas de entrega de Orden de Trabajo</param>
        /// <param name="fecha_fin_ent">Realiza la busqueda de fin de un rango de fechas de entrega de Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable OrdenTrabajo(int id_compania_emisor,int no_orden_trabajo, byte estatus, int id_cliente, int id_proveedor, int id_empleado, DateTime fecha_ini_sol, DateTime fecha_fin_sol, DateTime fecha_ini_ent, DateTime fecha_fin_ent,
                                             int id_unidad, string descripcion_unidad, int id_taller)
        {
            //Creación de la tabla Orden de tabajo
            DataTable dtOrdenTrabajo = null;
            //Creación del objeto param que alamcena los parametros necesarios para hacer la consulta de las ordenes de trabajo
            object[] param ={4,id_compania_emisor,no_orden_trabajo,estatus,id_cliente,id_proveedor,id_empleado,
                            fecha_ini_sol == DateTime.MinValue ? "" : fecha_ini_sol.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                            fecha_fin_sol == DateTime.MinValue ? "" : fecha_fin_sol.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                            fecha_ini_ent == DateTime.MinValue ? "" : fecha_ini_ent.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                            fecha_fin_ent == DateTime.MinValue ? "" : fecha_fin_ent.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),id_unidad,descripcion_unidad,id_taller,"","","","","","",""};
            //Invoca al método que realiza la consulta y el resultado lo almacena el un DataSet
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna los valores del dataset al datatble orden de trabajo
                    dtOrdenTrabajo = DS.Tables["Table"];
            }
            //Retorna el resultado al método
            return dtOrdenTrabajo;
        }
        #endregion
    }
}
