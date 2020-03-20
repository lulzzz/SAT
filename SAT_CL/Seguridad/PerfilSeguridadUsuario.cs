using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;
using System.Transactions;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Clase encargada de Todas las Operaciones relacionadas con la Relación de los Perfiles de los Usuarios
    /// </summary>
    public class PerfilSeguridadUsuario : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del Store Procedure
        /// </summary>
        private static string _nom_sp = "seguridad.sp_perfil_seguridad_usuario_tpsu";
        
        private int _id_perfil_usuario;
        /// <summary>
        /// Atributo encargado de almacenar el Perfil de Seguridad del Usuario
        /// </summary>
        public int id_perfil_usuario { get { return this._id_perfil_usuario; } }
        private int _id_perfil;
        /// <summary>
        /// Atributo encargado de almacenar el Perfil
        /// </summary>
        public int id_perfil { get { return this._id_perfil; } }
        private int _id_usuario;
        /// <summary>
        /// Atributo encargado de almacenar el Usuario
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }
        private bool _perfil_activo;
        /// <summary>
        /// Atributo encargado de almacenar el Indicador del Perfil Activo
        /// </summary>
        public bool perfil_activo { get { return this._perfil_activo; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public PerfilSeguridadUsuario()
        {
            //Asignando Atributos
            this._id_perfil_usuario = 0;
            this._id_perfil = 0;
            this._id_usuario = 0;
            this._perfil_activo = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Perfil de Seguridad
        /// </summary>
        /// <param name="id_perfil_seguridad">Perfil de Seguridad</param>
        public PerfilSeguridadUsuario(int id_perfil_seguridad)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_perfil_seguridad);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~PerfilSeguridadUsuario()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar los Atributos de la Clase
        /// </summary>
        /// <param name="id_perfil_usuario">Perfil de Seguridad del Usuario</param>
        private bool cargaAtributosInstancia(int id_perfil_usuario)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_perfil_usuario, 0, 0, false, 0, false, "", "" };

            //Obteniendo Registro del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iniciando Ciclo
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_perfil_usuario = id_perfil_usuario;
                        this._id_perfil = Convert.ToInt32(dr["IdPerfil"]);
                        this._id_usuario = Convert.ToInt32(dr["IdUsuario"]);
                        this._perfil_activo = Convert.ToBoolean(dr["PerfilActivo"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Variable Positiva
                    result = true;
                }
            }
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_perfil">Perfil de Seguridad</param>
        /// <param name="id_usuario">Usuario</param>
        /// <param name="perfil_activo">Indicador de Perfil Activo</param>
        /// <param name="id_usuario_act">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_perfil, int id_usuario, bool perfil_activo, int id_usuario_act, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_perfil_usuario, id_perfil, id_usuario, perfil_activo, id_usuario_act, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Perfiles de Seguridad
        /// </summary>
        /// <param name="id_perfil">Perfil de Seguridad</param>
        /// <param name="id_usuario">Usuario</param>
        /// <param name="id_usuario_act">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPerfilSeguridadUsuario(int id_perfil, int id_usuario, int id_usuario_act)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_perfil, id_usuario, true, id_usuario_act, true, "", "" };

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Perfil de Usuario
                using (PerfilSeguridadUsuario psu = PerfilSeguridadUsuario.ObtienePerfilActivo(id_usuario))
                {
                    //Validando que Existe el Perfil
                    if(psu._id_perfil_usuario > 0)
                    {
                        //Editando Perfil
                        result = psu.EditaPerfilSeguridadUsuario(psu.id_perfil, psu.id_usuario, false, id_usuario_act);

                        //Validando que la Operación fuese Exitosa
                        if(result.OperacionExitosa)

                            //Ejecutando SP
                            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                    }
                    else
                        //Ejecutando SP
                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                    //Validando que las Operaciones fueran Exitosas
                    if (result.OperacionExitosa)

                        //Completando Transacción
                        trans.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Perfiles de Seguridad
        /// </summary>
        /// <param name="id_perfil">Perfil de Seguridad</param>
        /// <param name="id_usuario">Usuario</param>
        /// <param name="perfil_activo">Indicador de Perfil Activo</param>
        /// <param name="id_usuario_act">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaPerfilSeguridadUsuario(int id_perfil, int id_usuario, bool perfil_activo, int id_usuario_act)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistroBD(id_perfil, id_usuario, perfil_activo, id_usuario_act, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Perfiles de Seguridad
        /// </summary>
        /// <param name="id_usuario_act">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPerfilSeguridadUsuario(int id_usuario_act)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistroBD(this._id_perfil, this._id_usuario, this._perfil_activo, id_usuario_act, false);
        }
        /// <summary>
        /// Método encargado de Actualizar el Perfil de Seguridad
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPerfilSeguridadUsuario()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_perfil_usuario);
        }
        /// <summary>
        /// Método encargado de Obtener la Instancia del Perfil Activo, dado un Usuario
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static PerfilSeguridadUsuario ObtienePerfilActivo(int id_usuario)
        {
            //Declarando Objeto de Retorno
            PerfilSeguridadUsuario psu = new PerfilSeguridadUsuario();

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, id_usuario, true, 0, false, "", "" };

            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iniciando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    
                        //Instanciando Perfil de Usuario
                        psu = new PerfilSeguridadUsuario(Convert.ToInt32(dr["Id"]));
                }
            }

            //Devolviendo Objeto de Retorno
            return psu;
        }
        /// <summary>
        /// Método encargado de Obtener los Perfiles del Usuario
        /// </summary>
        /// <param name="id_usuario">Usuario</param>
        /// <returns></returns>
        public static DataTable ObtienePerfilesUsuario(int id_usuario)
        {
            //Declarando Objeto de Retorno
            DataTable dtPerfiles = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, id_usuario, false, 0, false, "", "" };

            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtPerfiles = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtPerfiles;
        }

        #endregion
    }
}
