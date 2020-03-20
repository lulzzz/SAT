using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones correspondientes con las Columnas Filtro de las Tablas
    /// </summary>
    public class TablaColumnaFiltro : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_tabla_columna_filtro_tcf";
        
        private int _id_columna_filtro;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Columna Filtro
        /// </summary>
        public int id_columna_filtro { get { return this._id_columna_filtro; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_tipo;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Tipo
        /// </summary>
        public int id_tipo { get { return this._id_tipo; } }
        private string _columna_filtro;
        /// <summary>
        /// Atributo encargado de Almacenar el Campo de Columna Filtro
        /// </summary>
        public string columna_filtro { get { return this._columna_filtro; } }
        private string _alias;
        /// <summary>
        /// Atributo encargado de Almacenar el Alias
        /// </summary>
        public string alias { get { return this._alias; } }
        private int _id_tabla_catalogo;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Tabla Catalogo
        /// </summary>
        public int id_tabla_catalogo { get { return this._id_tabla_catalogo; } }
        private string _campo_descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar el Campo Descripción
        /// </summary>
        public string campo_descripcion { get { return this._campo_descripcion; } }
		private int _id_tipo_catalogo;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Tipo del Catalogo
        /// </summary>
        public int id_tipo_catalogo { get { return this._id_tipo_catalogo; } }
        private string _id_campo;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Campo Objetivo
        /// </summary>
        public string id_campo { get { return this._id_campo; } }
        private string _operador;
        /// <summary>
        /// Obtiene el operador que filtra al campo objetivo
        /// </summary>
        public string operador { get { return this._operador; } }
        private bool _bit_clasificacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Bit de Clasificación
        /// </summary>
        public bool bit_clasificacion { get { return this._bit_clasificacion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Contructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public TablaColumnaFiltro()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Contructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public TablaColumnaFiltro(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TablaColumnaFiltro()
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
            this._id_columna_filtro = 0;
            this._id_tabla = 0;
            this._id_tipo = 0;
            this._columna_filtro = "";
            this._alias = "";
            this._id_tabla_catalogo = 0;
            this._campo_descripcion = "";
            this._id_tipo_catalogo = 0;
            this._id_campo = "";
            this._operador = "";
            this._bit_clasificacion = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, 0, "", "", 0, "", 0, "", "", false, 0, false, "", "" };
            //Instanciando Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Filas
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_columna_filtro = id_registro;
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_tipo = Convert.ToInt32(dr["IdTipo"]);
                        this._columna_filtro = dr["ColumnaFiltro"].ToString();
                        this._alias = dr["Alias"].ToString();
                        this._id_tabla_catalogo = Convert.ToInt32(dr["IdTablaCatalogo"]);
                        this._campo_descripcion = dr["CampoDescripcion"].ToString();
                        this._id_tipo_catalogo = Convert.ToInt32(dr["IdTipoCatalogo"]);
                        this._id_campo = dr["IdCampo"].ToString();
                        this._operador = dr["Operador"].ToString();
                        this._bit_clasificacion = Convert.ToBoolean(dr["BitClasificacion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando valor Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
