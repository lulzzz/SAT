using System;
using System.Configuration;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Almacen
{
    public static class Reportes
    {
        #region Atributos

        private static string _nombre_stored_procedure = "almacen.sp_reporte_modulo";

        #endregion

        #region Métodos Públicos
                
        /// <summary>
        /// Obtiene la información detallada de existencias sobre un Id de Producto y Almacén
        /// </summary>
        /// <param name="id_producto">Id de Producto</param>
        /// <param name="id_almacen">Id de Almacén</param>
        /// <returns></returns>
        public static DataTable CargaExistenciasProducto(int id_producto, int id_almacen)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 1, id_producto, id_almacen, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Generando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene la información detallada de existencias sobre un Id de Producto en cualquier almacén
        /// </summary>
        /// <param name="id_producto">Id de Producto</param>
        /// <returns></returns>
        public static DataTable CargaExistenciasProducto(int id_producto)
        {
            //Devolviendo existencias
            return CargaExistenciasProducto(id_producto, 0);
        }
        /// <summary>
        /// Obtiene la información detallada de existencias sobre un Producto buscado por Número de Serie y almacén
        /// </summary>
        /// <param name="serie">Número de serie solicitado</param>
        /// <param name="id_almacen">Id de Almacén de interés</param>
        /// <returns></returns>
        public static DataTable CargaExistenciasProducto(string serie, int id_almacen)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 2, serie, id_almacen, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Generando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene la información detallada de existencias sobre un Producto buscado por Número de Serie en cualquier almacén
        /// </summary>
        /// <param name="serie">Número de serie solicitado</param>
        /// <returns></returns>
        public static DataTable CargaExistenciasProducto(string serie)
        {
            //Devolviendo resultado de existencias
            return CargaExistenciasProducto(serie, 0);
        }        
        /// <summary>
        /// Obtiene la información detallada de existencias sobre de productos con fecha de caducidad menor o igual a la solicitada dentro del almacén indicado
        /// </summary>
        /// <param name="fecha_caducidad">Fecha límite de caducidad</param>
        /// <param name="id_almacen">Id de Almacén de interés</param>
        /// <returns></returns>
        public static DataTable CargaExistenciasProducto(DateTime fecha_caducidad, int id_almacen)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 3, Fecha.ConvierteDateTimeString(fecha_caducidad, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), id_almacen, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Generando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene la información detallada de existencias sobre de productos con fecha de caducidad menor o igual a la solicitada dentro de cualquier almacén
        /// </summary>
        /// <param name="fecha_caducidad">Fecha límite de caducidad</param>
        /// <returns></returns>
        public static DataTable CargaExistenciasProducto(DateTime fecha_caducidad)
        {
            //Devolviendo resultado de existencias
            return CargaExistenciasProducto(fecha_caducidad, 0);
        }
        /// <summary>
        /// Método encargado de Obtener las Requisiciones Pendientes
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="fecha_ini_sol">Fecha de Inicio (Solicitud)</param>
        /// <param name="fecha_fin_sol">Fecha de Fin (Solicitud)</param>
        /// <param name="fecha_ini_ent">Fecha de Inicio (Entrega)</param>
        /// <param name="fecha_fin_ent">Fecha de Fin (Entrega)</param>
        /// <returns></returns>
        public static DataTable ObtieneRequisicionesPendientes(int id_compania_emisora, DateTime fecha_ini_sol, DateTime fecha_fin_sol, 
                                                                DateTime fecha_ini_ent, DateTime fecha_fin_ent, byte id_estatus,int id_almacen, int no_requisicion)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisicionesPend = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 4, id_compania_emisora, fecha_ini_sol == DateTime.MinValue ? "" : fecha_ini_sol.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_fin_sol == DateTime.MinValue ? "" : fecha_fin_sol.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_ini_ent == DateTime.MinValue ? "" : fecha_ini_ent.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_fin_ent == DateTime.MinValue ? "" : fecha_fin_ent.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 id_estatus, id_almacen, no_requisicion, "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtRequisicionesPend = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisicionesPend;
        }
        /// <summary>
        /// Método encargado de Obtener las Requisiciones de una Orden de Trabajo
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable ObtieneRequisicionesOrdenTrabajo(int id_compania_emisora, int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 5, id_compania_emisora, id_orden_trabajo, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtRequisiciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisiciones;
        }

        /// <summary>
        /// Método que realiza la busqueda de ordenes  de compra pendientes (en estatus Solicitar o Abastecido Parcial).
        /// </summary>
        /// <param name="id_compania_emisor">Identifica a la compañia a la que pertenece el registro deorden de compra.</param>
        /// <param name="fecha_ini_sol"> Fecha solicitud que determina el inicio de rango de busqueda por fecha.</param>
        /// <param name="fecha_fin_sol"> Fecha solicitud que determina el fin de rango de busqueda por fecha. </param>
        /// <param name="fecha_ini_ent">Fecha entrega que determina el Inicio de rango de busqueda por fecha.</param>
        /// <param name="fecha_fin_ent">Fecha entrega que determina el Fin de rango de busqueda por fecha.</param>
        /// <param name="id_proveedor">Identificador del proveedor de orden de compra.</param>
        /// <returns></returns>
        public static DataTable ObtieneOrdenesCompraPendientes(int id_compania_emisor,DateTime fecha_ini_sol, DateTime fecha_fin_sol,
                                                                DateTime fecha_ini_ent, DateTime fecha_fin_ent, int id_proveedor, string no_orden)
        {
            //Creación de la tabla dtOrdenCompraPendiente
            DataTable dtOrdenCompraPendiente = null;
            //Creación del arreglo que almacena los datos necesarios para la realización de la consulta
            object[] param = { 6, id_compania_emisor, fecha_ini_sol == DateTime.MinValue ? "" : fecha_ini_sol.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_fin_sol == DateTime.MinValue ? "" : fecha_fin_sol.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_ini_ent == DateTime.MinValue ? "" : fecha_ini_ent.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 fecha_fin_ent == DateTime.MinValue ? "" : fecha_fin_ent.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 id_proveedor, no_orden, "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //Invoca al método que realiza la consulta a base de datos y el resultado lo almacene en el dataset DS 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Valida los datos del DS
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))                
                    //Asigna a la tabla los valores del DS
                    dtOrdenCompraPendiente = DS.Tables["Table"];                
            }
            //Devuelve el resultado al método
            return dtOrdenCompraPendiente;
        }

        /// <summary>
        /// Método encargado de Obtener las Requisiciones de  una Actividad
        /// </summary>
        /// <param name="id_orden_trabajo_actividad">Id Orden Trabajo Actividad</param>
        /// <returns></returns>
        public static DataTable ObtieneRequisicionesActividad(int id_orden_trabajo_actividad)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 7, id_orden_trabajo_actividad, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtRequisiciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisiciones;
        }

        /// <summary>
        /// Método encargado de Obtener las Existencias de Almacen
        /// </summary>
        /// <param name="id_compania_emisora">Id compania Emisora</param>
        /// <param name="id_almacen">Id Almacen</param>
        /// <param name="producto">Producto</param>
        /// <param name="lote">Lote</param>
        /// <param name="serie">Serie</param>
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="fecha_fin">Fecha Fin</param>
        /// <returns></returns>
        public static DataTable ObtieneExistenciasAlmacen(int id_compania_emisora, int id_almacen, string producto, string lote, string serie, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 8, id_compania_emisora, id_almacen, producto, serie, 
                               Fecha.ConvierteDateTimeString(fecha_inicio, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), 
                               Fecha.ConvierteDateTimeString(fecha_fin, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), 
                               lote, "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtRequisiciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisiciones;
        }
        /// <summary>
        /// Método encargado de Obtener las Requisiciones
        /// </summary>
        /// <param name="id_compania_emisora"></param>
        /// <param name="no_requisicion"></param>
        /// <param name="id_almacen"></param>
        /// <param name="id_estatus"></param>
        /// <returns></returns>
        public static DataTable ObtieneRequisiciones(int id_compania_emisora, string no_requisicion, int id_almacen, byte id_estatus,
                                                     DateTime fecha_ini_sol, DateTime fecha_fin_sol, DateTime fecha_ini_ent, DateTime fecha_fin_ent)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 10, id_compania_emisora, no_requisicion, id_almacen, id_estatus, 
                               Fecha.ConvierteDateTimeString(fecha_ini_sol, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), 
                               Fecha.ConvierteDateTimeString(fecha_fin_sol, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), 
                               Fecha.ConvierteDateTimeString(fecha_ini_ent, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), 
                               Fecha.ConvierteDateTimeString(fecha_fin_ent, ConfigurationManager.AppSettings.Get("FormatoFechaReportes")), 
                               "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtRequisiciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisiciones;
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de Trazabilidad del Inventario
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_almacen">Almacen Deseado</param>
        /// <param name="lote">Lote por Buscar</param>
        /// <param name="id_producto">Producto Solicitado</param>
        /// <returns></returns>
        public static DataTable ObtieneReporteTrazabilidad(int id_compania_emisora, int id_almacen, string lote, int id_producto)
        {
            //Declarando Objeto de Retorno
            DataTable dtInventario = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 11, id_compania_emisora, id_almacen, lote, id_producto, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtInventario = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtInventario;
        }

        #endregion
    }
}
