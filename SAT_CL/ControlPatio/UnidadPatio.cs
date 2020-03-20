using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{
    public class UnidadPatio : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_unidad_patio_tupa";
        /// <summary>
        /// Atributo encargado de Almacenar el Id Primario del Registro
        /// </summary>
        public int id_unidad_patio { get { return this._id_unidad_patio; } }
        private int _id_unidad_patio;
        /// <summary>
        /// Atributo encargado de Almacenar el no economico
        /// </summary>
        public string no_economico { get { return this._no_economico; } }
        private string _no_economico;
        /// <summary>
        /// Atributo encargado de Almacenar placas
        /// </summary>
        public string placas { get { return this._placas; } }
        private string _placas;
        /// <summary>
        /// Atributo encargado de Almacenar color
        /// </summary>
        public string color { get { return this._color; } }
        private string _color;
        /// <summary>
        /// Atributo encargado de Almacenar transportista
        /// </summary>
        public int id_transportista { get { return this._id_transportista; } }
        private int _id_transportista;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private bool _habilitar;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public UnidadPatio()
        {   //Asignando Valores
            this._id_unidad_patio = 0;
            this._no_economico = "";
            this._placas = "";
            this._color = "";
            this._id_transportista = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_unidad_patio">Id de Unidad de Patio</param>
        public UnidadPatio(int id_unidad_patio)
        {   //Invocando Método de Carga
            cargaAtributoInstancia(id_unidad_patio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~UnidadPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_unidad_patio"></param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_unidad_patio)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_unidad_patio, "", "", "", 0, true, 0, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_unidad_patio = Convert.ToInt32(dr["Id"]); ;
                        this._no_economico = Convert.ToString(dr["NoEconomico"]);
                        this._placas = Convert.ToString(dr["Placas"]);
                        this._color = Convert.ToString(dr["Color"]);
                        this._id_transportista = Convert.ToInt32(dr["IdTransportista"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Valor Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar el Registro en BD
        /// </summary>
        /// <param name="id_unidad_patio">Unidad Asignado</param>
        /// <param name="no_economico">No economico</param>
        /// <param name="placas">Indicador de placas</param>
        /// <param name="color">Indicador de placas</param>
        /// <param name="id_transportista">Indicador de id transportista param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(string no_economico, string placas, string color, int id_transportista,
                                                   int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_unidad_patio, no_economico, placas, color, id_transportista, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado de la Actualización
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Relaciones de los Unidades con los Patios
        /// </summary>
        /// <param name="id_unidad_patio">Unidad Asignado</param>
        /// <param name="no_economico">No economico</param>
        /// <param name="placas">Indicador de placas</param>
        /// <param name="color">Indicador de placas</param>
        /// <param name="id_transportista">Indicador de id transportista param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUnidadPatio(string no_economico, string placas, string color, int id_transportista,
                                                   int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, no_economico, placas, color, id_transportista, id_usuario, true, "", "" };
            //Obteniendo Resultado de la Actualización
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Relaciones de los Usuarios con los Patios
        /// </summary>
        /// <param name="id_usuario_asignado">Usuario Asignado</param>
        /// <param name="id_patio">Patio</param>
        /// <param name="bit_patio_default">Indicador de Patio Predeterminado</param>
        /// <param name="id_acceso_default">Acceso Predeterminado del Patio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaUnidadPatio(string no_economico, string placas, string color, int id_transportista,
                                                   int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(no_economico, placas, color, id_transportista,
                                          id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Relaciones de los Usuarios con los Patios
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUnidadPatio(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(this._no_economico, this._placas, this._color, this._id_transportista,
                                          id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos del Registro Actual
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUnidadPatio()
        {   //Invocando Método de Carga
            return this.cargaAtributoInstancia(this._id_unidad_patio);
        }
        #endregion
    }
}
