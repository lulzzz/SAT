using System;
using System.Data;
using TSDK.Base;
using System.Configuration;

namespace SAT_CL.ControlPatio
{
    /// <summary>
    /// Clase encargada de Obtener los Reportes del Modulo Control de Patios
    /// </summary>
    public static class Reporte
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Método Público encargado de Obtener el Reporte de las Unidades Dentro del Patio
        /// </summary>
        /// <param name="descripcion">Descripcion (Nombre Operador / No. Unidad)</param>
        /// <param name="identificador">Identificador (Identificación / Placas)</param>
        /// <param name="id_patio">Ubicacion de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneUnidadesDentro(string descripcion, string identificador, int id_patio)
        {   //Declarando Objeto de Retorno
            DataTable dtUnidades = null;
            //Armando Arreglo de Parametros
            object[] param = { 1, descripcion, identificador, id_patio.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
            //Obteniendo Reporte
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtUnidades = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades Dentro con su Evento Actual
        /// </summary>
        /// <param name="descripcion">Descripcion (Nombre Operador / No. Unidad)</param>
        /// <param name="identificador">Identificador (Identificación / Placas)</param>
        /// <param name="id_patio">Ubicacion de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneUnidadesEventoActual(string descripcion, string identificador, int id_patio)
        {   //Declarando Objeto de Retorno
            DataTable dtUnidades = null;
            //Armando Arreglo de Parametros
            object[] param = { 2, descripcion, identificador, id_patio.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtUnidades = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Reportes del Historial de las Unidades
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad/Unidad</param>
        /// <param name="descripcion">Descripción de las Entidades</param>
        /// <param name="identificacion">Identificaciones de las Entidades</param>
        /// <param name="id_estatus_acceso">Estatus de Acceso de las Entidades</param>
        /// <param name="id_estatus_patio">Estatus de Patio de las Entidades</param>
        /// <param name="fec_ini">Fecha de Inicio</param>
        /// <param name="fec_fin">Fecha de Fin</param>
        /// <returns></returns>
        public static DataSet ObtieneHistorialUnidades(int id_ubicacion_patio, int id_transportista, byte id_tipo_entidad, string descripcion,
                                                        string identificacion, int id_estatus_acceso, int id_estatus_patio, DateTime fec_ini, DateTime fec_fin)
        {   
            //Armando Arreglo de Parametros
            object[] param = { 3, id_ubicacion_patio, id_transportista, id_tipo_entidad, descripcion, identificacion, id_estatus_acceso, id_estatus_patio, 
                                 fec_ini == DateTime.MinValue ? "" : fec_ini.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), fec_fin == DateTime.MinValue ? "" : fec_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                 "", "", "", "", "", "", "", "", "", "", "", };
            
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))

                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Método Público encargado de Obtener el Reporte de las Unidades Dentro del Patio (Interfaz Móvil)
        /// </summary>
        /// <param name="descripcion">Descripcion (Nombre Operador / No. Unidad)</param>
        /// <param name="id_patio">Ubicacion de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneUnidadesDentroMovil(string descripcion, int id_patio)
        {   
            //Declarando Objeto de Retorno
            DataTable dtUnidades = null;
            
            //Armando Arreglo de Parametros
            object[] param = { 4, id_patio.ToString(), descripcion, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
            
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Resultado Obtenido
                    dtUnidades = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Entidades(Anden/Cajon) junto con su Estatus Actual
        /// </summary>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <returns></returns>
        public static DataSet CargaEstatusEntidadesGenerales(SAT_CL.ControlPatio.EntidadPatio.TipoEntidad tipo_entidad, int id_ubicacion_patio, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Armando Arreglo de Parametros
            object[] param = { 5, id_ubicacion_patio, (byte)tipo_entidad, fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                fecha_fin == DateTime.MinValue ? "" : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", };
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))

                //Devolviendo Resultado Obtenido
                return ds;
        }

        #endregion
    }
}
