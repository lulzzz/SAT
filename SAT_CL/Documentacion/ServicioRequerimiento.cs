using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Documentacion
{   
    /// <summary>
    /// Clase encargada de todas las operaciones de los Requerimientos de los Servicios
    /// </summary>
    public class ServicioRequerimiento : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "documentacion.sp_servicio_requerimiento_tsr";

        private int _id_requerimiento_servicio;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Requerimiento del Servicio
        /// </summary>
        public int id_requerimiento_servicio { get { return this._id_requerimiento_servicio; } }
        private int _id_requerimiento;
        /// <summary>
        /// Atributo encargado de Almacenar el Requerimiento
        /// </summary>
        public int id_requerimiento { get { return this._id_requerimiento; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de Almacenar el Servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de Almacenar la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Atributo encargado de Almacenar el Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private string _descripcion_requerimiento;
        /// <summary>
        /// Atributo encargado de Almacenar la Descripción del Requerimiento
        /// </summary>
        public string descripcion_requerimiento { get { return this._descripcion_requerimiento; } }
        private int _id_tabla_objetivo;
        /// <summary>
        /// Atributo encargado de Almacenar la Tabla Objetivo
        /// </summary>
        public int id_tabla_objetivo { get { return this._id_tabla_objetivo; } }
        private int _id_campo_objetivo;
        /// <summary>
        /// Atributo encargado de Almacenar el Campo Objetivo
        /// </summary>
        public int id_campo_objetivo { get { return this._id_campo_objetivo; } }
        private string _valor_objetivo;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Objetivo
        /// </summary>
        public string valor_objetivo { get { return this._valor_objetivo; } }
        private string _condicion;
        /// <summary>
        /// Atributo encargado de Almacenar la Condición
        /// </summary>
        public string condicion { get { return this._condicion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ServicioRequerimiento()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ServicioRequerimiento(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ServicioRequerimiento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_requerimiento_servicio = 0;
            this._id_requerimiento = 0;
            this._id_servicio = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._descripcion_requerimiento = "";
            this._id_tabla_objetivo = 0;
            this._id_campo_objetivo = 0;
            this._valor_objetivo = "";
            this._condicion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, "", 0, 0, "", "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {   //Validando que la Tabla contenga registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_requerimiento_servicio = id_registro;
                        this._id_requerimiento = Convert.ToInt32(dr["IdRequerimiento"]);
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._descripcion_requerimiento = dr["DescripcionRequerimiento"].ToString();
                        this._id_tabla_objetivo = Convert.ToInt32(dr["IdTablaObjetivo"]);
                        this._id_campo_objetivo = Convert.ToInt32(dr["IdCampoObjetivo"]);
                        this._valor_objetivo = dr["ValorObjetivo"].ToString();
                        this._condicion = dr["Condicion"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="id_requerimiento">Id de Requerimiento</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="descripcion_requerimiento">Descripción de Requerimiento</param>
        /// <param name="id_tabla_objetivo">Tabla Objetivo</param>
        /// <param name="id_campo_objetivo">Campo Objetivo</param>
        /// <param name="valor_objetivo">Valor Obejtivo</param>
        /// <param name="condicion">Condición</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_requerimiento, int id_servicio, int id_tabla, int id_registro, 
                                            string descripcion_requerimiento, int id_tabla_objetivo, int id_campo_objetivo,
                                            string valor_objetivo, string condicion, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_requerimiento_servicio, id_requerimiento, id_servicio, id_tabla, id_registro, 
                                 descripcion_requerimiento, id_tabla_objetivo, id_campo_objetivo, valor_objetivo, condicion, 
                                 id_usuario, habilitar,	"",	"" };
            //Obteniendo Resultado de Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Publico encargado de Ingresar los Requerimientos de Servicio
        /// </summary>
        /// <param name="id_requerimiento">Id de Requerimiento</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="descripcion_requerimiento">Descripción de Requerimiento</param>
        /// <param name="id_tabla_objetivo">Tabla Objetivo</param>
        /// <param name="id_campo_objetivo">Campo Objetivo</param>
        /// <param name="valor_objetivo">Valor Obejtivo</param>
        /// <param name="condicion">Condición</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioRequerimiento(int id_requerimiento, int id_servicio, int id_tabla, int id_registro,
                                            string descripcion_requerimiento, int id_tabla_objetivo, int id_campo_objetivo,
                                            string valor_objetivo, string condicion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_requerimiento, id_servicio, id_tabla, id_registro, 
                                 descripcion_requerimiento, id_tabla_objetivo, id_campo_objetivo, valor_objetivo, condicion, 
                                 id_usuario, true,	"",	"" };
            //Obteniendo Resultado de Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Publico encargado de Editar los Requerimientos de Servicio
        /// </summary>
        /// <param name="id_requerimiento">Id de Requerimiento</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="descripcion_requerimiento">Descripción de Requerimiento</param>
        /// <param name="id_tabla_objetivo">Tabla Objetivo</param>
        /// <param name="id_campo_objetivo">Campo Objetivo</param>
        /// <param name="valor_objetivo">Valor Obejtivo</param>
        /// <param name="condicion">Condición</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioRequerimiento(int id_requerimiento, int id_servicio, int id_tabla, int id_registro,
                                            string descripcion_requerimiento, int id_tabla_objetivo, int id_campo_objetivo,
                                            string valor_objetivo, string condicion, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(id_requerimiento, id_servicio, id_tabla, id_registro,
                                 descripcion_requerimiento, id_tabla_objetivo, id_campo_objetivo, valor_objetivo, condicion,
                                 id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Publico encargado de Deshabilitar los Requerimientos de Servicio
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioRequerimiento(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(this._id_requerimiento, this._id_servicio, this._id_tabla, this._id_registro,
                                 this._descripcion_requerimiento, this._id_tabla_objetivo, this._id_campo_objetivo, this._valor_objetivo, this._condicion,
                                 id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de los Requerimientos de Servicio
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicioRequerimiento()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_requerimiento_servicio);
        }
        /// <summary>
        /// Método Público encaragdo de Obtener los Requerimientos Ligados al Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneRequerimientosServicio(int id_servicio)
        {   //Declarando Objeto de Retorno
            DataTable dtRequerimientos = null;
            //Armando Areglo de Parametros
            object[] param = { 4, 0, 0, id_servicio, 0, 0, "", 0, 0, "", "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Objeto de Retorno
                    dtRequerimientos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtRequerimientos;
        }
        /// <summary>
        /// Devuelve los Requerimientos ligados al Servicio solicitado, sin formato para visualización (sólo información directa de registro)
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaRequerimientosServicio(int id_servicio)
        {   //Declarando Objeto de Retorno
            DataTable dtRequerimientos = null;
            //Armando Areglo de Parametros
            object[] param = { 5, 0, 0, id_servicio, 0, 0, "", 0, 0, "", "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Objeto de Retorno
                    dtRequerimientos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtRequerimientos;
        }

        /// <summary>
        /// Devuelve los Requerimientos ligados al Servicio para su deshabilitación.
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaRequerimientosServicioParaDeshabilitacion(int id_servicio)
        {   
            //Declarando Objeto de Retorno
            DataTable dtRequerimientos = null;
           
            //Armando Areglo de Parametros
            object[] param = { 6, 0, 0, id_servicio, 0, 0, "", 0, 0, "", "", 0, false, "", "" };
            
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Objeto de Retorno
                    dtRequerimientos = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtRequerimientos;
        }

        /// <summary>
        /// Deshabilitamos Requerimientos ligados al servicio.
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_usuario"> Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaRequerimientos(int id_servicio, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Requerimientos
            using (DataTable mitRequerimientos = CargaRequerimientosServicioParaDeshabilitacion(id_servicio))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitRequerimientos))
                {
                    //Recorremos cada uno de los requerimientos
                    foreach (DataRow r in mitRequerimientos.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Requerimiento
                            using (ServicioRequerimiento objServicioRequerimiento = new ServicioRequerimiento(r.Field<int>("Id")))
                            {
                                //Deshabulitamos Requerimiento
                                resultado = objServicioRequerimiento.DeshabilitaServicioRequerimiento(id_usuario);
                            }
                        }
                        else
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;

        }
        #endregion
    }
}
