using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas las Unidades de Medida
    /// </summary>
    public class UnidadMedida : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_unidad_medida_tum";

        private int _id_unidad_medida;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Unidad de Medida
        /// </summary>
        public int id_unidad_medida { get { return this._id_unidad_medida; } }
        private byte _id_tipo_unidad_medida;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Unidad de Medida
        /// </summary>
        public byte id_tipo_unidad_medida { get { return this._id_tipo_unidad_medida; } }
        private byte _id_sistema_metrico;
        /// <summary>
        /// Atributo encargado de almacenar el Sistema Metrico
        /// </summary>
        public byte id_sistema_metrico { get { return this._id_sistema_metrico; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private string _abreviatura;
        /// <summary>
        /// Atributo encargado de almacenar la Abreviatura
        /// </summary>
        public string abreviatura { get { return this._abreviatura; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase 
        /// </summary>
        public UnidadMedida()
        {   //Invocando Método de carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_registro"></param>
        public UnidadMedida(int id_registro)
        {   //Invocando Método de carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~UnidadMedida()
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
            this._id_unidad_medida = 0;
            this._id_tipo_unidad_medida = 0;
            this._id_sistema_metrico = 0;
            this._descripcion = "";
            this._abreviatura = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, "", "", true, 0, "", "" };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_unidad_medida = id_registro;
                        this._id_tipo_unidad_medida = Convert.ToByte(dr["IdTipoUnidadMedida"]);
                        this._id_sistema_metrico = Convert.ToByte(dr["IdSistemaMetrico"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._abreviatura = dr["Abreviatura"].ToString();
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
        /// Método Privado encargado de Actualizar los registros en BD
        /// </summary>
        /// <param name="id_tipo_unidad_medida">Tipo de Unidad de la Medida</param>
        /// <param name="id_sistema_metrico">Sistema Metrico de la Unidad de Medida</param>
        /// <param name="descripcion">Descripción de la Unidad de Medida</param>
        /// <param name="abreviatura">Abreviatura de la Unidad de Medida</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(byte id_tipo_unidad_medida, byte id_sistema_metrico, string descripcion, 
                               string abreviatura, bool habilitar, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_unidad_medida, id_tipo_unidad_medida, id_sistema_metrico, descripcion, 
                               abreviatura, habilitar, id_usuario, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Unidades de Medida
        /// </summary>
        /// <param name="id_tipo_unidad_medida">Tipo de Unidad de la Medida</param>
        /// <param name="id_sistema_metrico">Sistema Metrico de la Unidad de Medida</param>
        /// <param name="descripcion">Descripción de la Unidad de Medida</param>
        /// <param name="abreviatura">Abreviatura de la Unidad de Medida</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUnidadMedida(byte id_tipo_unidad_medida, byte id_sistema_metrico, string descripcion,
                               string abreviatura, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tipo_unidad_medida, id_sistema_metrico, descripcion, 
                               abreviatura, true, id_usuario, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Unidades de Medida
        /// </summary>
        /// <param name="id_tipo_unidad_medida">Tipo de Unidad de la Medida</param>
        /// <param name="id_sistema_metrico">Sistema Metrico de la Unidad de Medida</param>
        /// <param name="descripcion">Descripción de la Unidad de Medida</param>
        /// <param name="abreviatura">Abreviatura de la Unidad de Medida</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaUnidadMedida(byte id_tipo_unidad_medida, byte id_sistema_metrico, string descripcion,
                               string abreviatura, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tipo_unidad_medida, id_sistema_metrico, descripcion,
                               abreviatura, this._habilitar, id_usuario);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Unidades de Medida
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUnidadMedida(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tipo_unidad_medida, this._id_sistema_metrico, this._descripcion,
                               this._abreviatura, false, id_usuario);
        }
        /// <summary>
        /// Método Público encargado de Actualizar las Unidades de Medida
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUnidadMedida()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_unidad_medida);
        }

        #endregion
    }
}
