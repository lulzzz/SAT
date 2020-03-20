using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas los Requerimientos
    /// </summary>
    public class Requerimiento : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_requerimiento_tr";

        private int _id_requerimiento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Requerimiento
        /// </summary>
        public int id_requerimiento { get { return this._id_requerimiento; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private string _descripcion_requerimiento;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción del Requerimiento
        /// </summary>
        public string descripcion_requerimiento { get { return this._descripcion_requerimiento; } }
        private int _id_tabla_objetivo;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Tabla Objetivo
        /// </summary>
        public int id_tabla_objetivo { get { return this._id_tabla_objetivo; } }
        private int _id_campo_objetivo;
        /// <summary>
        /// Atributo encargado de almacenar el Id Campo Objetivo
        /// </summary>
        public int id_campo_objetivo { get { return this._id_campo_objetivo; } }
        private string _valor_objetivo;
        /// <summary>
        /// Atributo encargado de almacenar el Id Valor Objetivo
        /// </summary>
        public string valor_objetivo { get { return this._valor_objetivo; } }
        private string _condicion;
        /// <summary>
        /// Atributo encargado de almacenar la Condición
        /// </summary>
        public string condicion { get { return this._condicion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encaragdo de Inicializar los Atributos por Defecto
        /// </summary>
        public Requerimiento()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encaragdo de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro_param">Id de Registro</param>
        public Requerimiento(int id_registro_param)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro_param);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Requerimiento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_requerimiento = 0;
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
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro_param">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro_param)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro_param, 0, 0, "", 0, 0, "", "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_requerimiento = id_registro_param;
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
                //Asignando Resultado Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="descripcion_requerimiento">Descripción del Requerimiento</param>
        /// <param name="id_tabla_objetivo">Id de Tabla Objetivo</param>
        /// <param name="id_campo_objetivo">Id de Campo Objetivo</param>
        /// <param name="valor_objetivo">Valor Objetivo</param>
        /// <param name="condicion">Condición</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_tabla, int id_registro, string descripcion_requerimiento, int id_tabla_objetivo, 
                                                int id_campo_objetivo, string valor_objetivo, string condicion, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_requerimiento, id_tabla, id_registro, descripcion_requerimiento, 
                               id_tabla_objetivo, id_campo_objetivo, valor_objetivo, condicion, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Requerimientos
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="descripcion_requerimiento">Descripción del Requerimiento</param>
        /// <param name="id_tabla_objetivo">Id de Tabla Objetivo</param>
        /// <param name="id_campo_objetivo">Id de Campo Objetivo</param>
        /// <param name="valor_objetivo">Valor Objetivo</param>
        /// <param name="condicion">Condición</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaRequerimiento(int id_tabla, int id_registro, string descripcion_requerimiento, int id_tabla_objetivo,
                                                int id_campo_objetivo, string valor_objetivo, string condicion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tabla, id_registro, descripcion_requerimiento, 
                               id_tabla_objetivo, id_campo_objetivo, valor_objetivo, condicion, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Requerimientos
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="descripcion_requerimiento">Descripción del Requerimiento</param>
        /// <param name="id_tabla_objetivo">Id de Tabla Objetivo</param>
        /// <param name="id_campo_objetivo">Id de Campo Objetivo</param>
        /// <param name="valor_objetivo">Valor Objetivo</param>
        /// <param name="condicion">Condición</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaRequerimiento(int id_tabla, int id_registro, string descripcion_requerimiento, int id_tabla_objetivo,
                                                int id_campo_objetivo, string valor_objetivo, string condicion, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tabla, id_registro, descripcion_requerimiento,id_tabla_objetivo, 
                               id_campo_objetivo, valor_objetivo, condicion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Requerimientos
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaRequerimiento(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tabla, this._id_registro, this._descripcion_requerimiento, this._id_tabla_objetivo,
                               this._id_campo_objetivo, this._valor_objetivo, this._condicion, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Requerimientos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRequerimiento()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_requerimiento);
        }

        #endregion
    }
}
