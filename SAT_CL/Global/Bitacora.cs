using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones correspondientes con la Bitacora
    /// </summary>
    public class Bitacora
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_bitacora_tb";

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public Bitacora()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Privado encargado de Insertar la Bitacora
        /// </summary>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro de la Tabla</param>
        /// <param name="id_tipo_bitacora">Tipo de Bitacora</param>
        /// <param name="anterior">Registro Anterior</param>
        /// <param name="fecha">Fecha de Insercción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaBitacora(int id_tabla, int id_registro, int id_tipo_bitacora, string anterior, 
                                                       DateTime fecha, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de parametros
            object[] param = { 1, 0, id_tabla, id_registro, id_tipo_bitacora, anterior, fecha, id_usuario, habilitar, "", "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de la Edición de la Bitacora
        /// </summary>
        /// <param name="id_bitacora">Id de Bitacora</param>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro de la Tabla</param>
        /// <param name="id_tipo_bitacora">Tipo de Bitacora</param>
        /// <param name="anterior">Registro Anterior</param>
        /// <param name="fecha">Fecha de Insercción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        public static RetornoOperacion EditaBitacora(int id_bitacora, int id_tabla, int id_registro, int id_tipo_bitacora, string anterior,
                                                     DateTime fecha, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de parametros
            object[] param = { 2, id_bitacora, id_tabla, id_registro, id_tipo_bitacora, anterior, fecha, id_usuario, habilitar, "", "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de cargar los registros en bitácora de un registro 
        /// </summary>
        /// <param name="idTabla">Id de tabla a la que pertenece</param>
        /// <param name="idRegistro">Id de Registro a consultar</param>
        /// <param name="fechaInicial">Fecha Inicial a mostrar</param>
        /// <param name="fechaFinal">Fecha Final a mostrar</param>
        /// <param name="tipoBitacora">Id de Tipo de bitácora</param>
        /// <returns></returns>
        public static DataSet CargaBitacoraRegistro(int idTabla, int idRegistro, string fechaInicial, string fechaFinal, int tipoBitacora)
        {   //Inicializando parámetros de consulta
            object[] param = { 4, 0, idTabla, idRegistro, tipoBitacora, "", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, true, fechaInicial, fechaFinal, tipoBitacora.ToString() };
            //Obteniendo registros solicitados
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Devolviendo el DataSet obtenido
                return DS;
            }
        }

        #endregion

        #region Capa logica de forma web

        /// <summary>
        /// Método que carga la bitácora asociados al registro y tabla solicitados
        /// </summary>
        /// <param name="id_tabla">Id de tabla a la que pertenece</param>
        /// <param name="id_registro">Id de registro a cargar</param>
        /// <param name="inicio">Fecha de Inicio</param>
        /// <param name="fin">Fecha de Fin</param>
        /// <param name="tipo">Tipo de Bitacora</param>
        /// <param name="gv">Control GridView a Cargar</param>
        /// <param name="nombre_tabla_ds">Nombre de la Tabla en DataSet</param>
        /// <param name="llave_ds">Campos Llave</param>
        /// <param name="criterio_orden">Criterio de Ordenación</param>
        /// <returns></returns>
        public static DataSet CargaBitacoraControl(int id_tabla, int id_registro, DateTime inicio, DateTime fin, int tipo, GridView gv, string nombre_tabla_ds, string llave_ds, string criterio_orden)
        {   
            //Asignando atributo DataSet
            using (DataSet DS = Bitacora.CargaBitacoraRegistro(id_tabla, id_registro, inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), 
                                                                fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), tipo))
            {   
                //Cargando el GridView con los datops obtenidos
                TSDK.ASP.Controles.CargaGridView(gv, DS, nombre_tabla_ds, llave_ds, criterio_orden, true, 1);
                
                //Devolviendo Resultado Obtenido
                return DS;
            }
        }

        #endregion
    }
}
