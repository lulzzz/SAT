using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{   /// <summary>
    /// Clase encargada de las Operaciones de los Usuarios y los Patios Asignados
    /// </summary>
    public class UsuarioPatio : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_usuario_patio_tup";

        private int _id_usuario_patio;
        /// <summary>
        /// Atributo encargado de Almacenar el Id Primario del Registro
        /// </summary>
        public int id_usuario_patio { get { return this._id_usuario_patio; } }
        private int _id_usuario_asignado;
        /// <summary>
        /// Atributo encargado de Almacenar el Usuario Asignado
        /// </summary>
        public int id_usuario_asignado { get { return this._id_usuario_asignado; } }
        private int _id_patio;
        /// <summary>
        /// Atributo encargado de Almacenar el Patio
        /// </summary>
        public int id_patio { get { return this._id_patio; } }
        private bool _bit_patio_default;
        /// <summary>
        /// Atributo encargado de Almacenar el Indicador de Patio Predeterminado
        /// </summary>
        public bool bit_patio_default { get { return this._bit_patio_default; } }
        private int _id_acceso_default;
        /// <summary>
        /// Atributo encargado de Almacenar el Acceso Predeterminado del Patio
        /// </summary>
        public int id_acceso_default { get { return this._id_acceso_default; } }
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
        public UsuarioPatio()
        {   //Asignando Valores
            this._id_usuario_patio = 0;
            this._id_usuario_asignado = 0;
            this._id_patio = 0;
            this._bit_patio_default = false;
            this._id_acceso_default = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_usuario_patio">Id de Usuario de Patio</param>
        public UsuarioPatio(int id_usuario_patio)
        {   //Invocando Método de Carga
            cargaAtributoInstancia(id_usuario_patio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~UsuarioPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_usuario_patio"></param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_usuario_patio)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_usuario_patio, 0, 0, false, 0, 0, false, "", "" };
            //Instanciando Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_usuario_patio = id_usuario_patio;
                        this._id_usuario_asignado = Convert.ToInt32(dr["IdUsuarioAsignado"]);
                        this._id_patio = Convert.ToInt32(dr["IdPatio"]);
                        this._bit_patio_default = Convert.ToBoolean(dr["BitPatioDefault"]);
                        this._id_acceso_default = Convert.ToInt32(dr["IdAccesoDefault"]);
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
        /// <param name="id_usuario_asignado">Usuario Asignado</param>
        /// <param name="id_patio">Patio</param>
        /// <param name="bit_patio_default">Indicador de Patio Predeterminado</param>
        /// <param name="id_acceso_default">Acceso Predeterminado del Patio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_usuario_asignado, int id_patio, bool bit_patio_default, int id_acceso_default, 
                                                   int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_usuario_patio, id_usuario_asignado, id_patio, bit_patio_default, id_acceso_default, 
                               id_usuario, habilitar, "", "" };
            //Obteniendo Resultado de la Actualización
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Relaciones de los Usuarios con los Patios
        /// </summary>
        /// <param name="id_usuario_asignado">Usuario Asignado</param>
        /// <param name="id_patio">Patio</param>
        /// <param name="bit_patio_default">Indicador de Patio Predeterminado</param>
        /// <param name="id_acceso_default">Acceso Predeterminado del Patio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUsuarioPatio(int id_usuario_asignado, int id_patio, bool bit_patio_default, int id_acceso_default,
                                                           int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_usuario_asignado, id_patio, bit_patio_default, id_acceso_default, 
                               id_usuario, true, "", "" };
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
        public RetornoOperacion EditaUsuarioPatio(int id_usuario_asignado, int id_patio, bool bit_patio_default, int id_acceso_default,
                                                  int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(id_usuario_asignado, id_patio, bit_patio_default, id_acceso_default,
                                          id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Relaciones de los Usuarios con los Patios
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUsuarioPatio(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(this._id_usuario_asignado, this._id_patio, this._bit_patio_default, this._id_acceso_default,
                                          id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos del Registro Actual
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUsuarioPatio()
        {   //Invocando Método de Carga
            return this.cargaAtributoInstancia(this._id_usuario_patio);
        }
        /// <summary>
        /// Método Público encargado de Obtener la Instancia por Defecto del Usuario
        /// </summary>
        /// <param name="id_usuario">Usuario</param>
        /// <returns></returns>
        public static UsuarioPatio ObtieneInstanciaDefault(int id_usuario)
        {   //Declarando Objeto de Retorno
            UsuarioPatio result = new UsuarioPatio();
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_usuario, 0, false, 0, 0, false, "", "" };
            //Obteniendo instancia
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Valdiando que existe un Registro
                        if (Convert.ToInt32(dr["Id"]) > 0)
                            //Instanciando Registro
                            result = new UsuarioPatio(Convert.ToInt32(dr["Id"]));
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
