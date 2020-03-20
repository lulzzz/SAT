using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con los Perfiles de Seguridad
    /// </summary>
    public class PerfilSeguridad : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del Store Procedure
        /// </summary>
        private static string _nom_sp = "seguridad.sp_perfil_seguridad_tps";
        
        private int _id_perfil_seguridad;
        /// <summary>
        /// Atributo encargado de almacenar el Perfil de Seguridad
        /// </summary>
        public int id_perfil_seguridad { get { return this._id_perfil_seguridad; } }
        private int _id_compania;
        /// <summary>
        /// Atributo encargado de almacenar la Compania Emisora
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción del Perfil
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private string _detalles;
        /// <summary>
        /// Atributo encargado de almacenar los Detalles del Perfil
        /// </summary>
        public string detalles { get { return this._detalles; } }
        private int _id_forma_inicio;
        /// <summary>
        /// Atributo encargado de almacenar la Forma de Inicio
        /// </summary>
        public int id_forma_inicio { get { return this._id_forma_inicio; } }
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
        public PerfilSeguridad()
        {
            //Asignando Atributos
            this._id_perfil_seguridad = 0;
            this._id_compania = 0;
            this._descripcion = "";
            this._detalles = "";
            this._id_forma_inicio = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Perfil de Seguridad
        /// </summary>
        /// <param name="id_perfil_seguridad">Perfil de Seguridad</param>
        public PerfilSeguridad(int id_perfil_seguridad)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_perfil_seguridad);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~PerfilSeguridad()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar los Atributos de la Clase
        /// </summary>
        /// <param name="id_perfil_seguridad">Perfil de Seguridad</param>
        private bool cargaAtributosInstancia(int id_perfil_seguridad)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_perfil_seguridad, 0, "", "", 0, 0, false, "", "" };

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
                        this._id_perfil_seguridad = id_perfil_seguridad;
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._detalles = dr["Detalles"].ToString();
                        this._id_forma_inicio = Convert.ToInt32(dr["IdFormaInicio"]);
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
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Perfil</param>
        /// <param name="detalles">Detalles del Perfil</param>
        /// <param name="id_forma_inicio">Referencia de Forma de Inicio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_compania, string descripcion, string detalles, int id_forma_inicio, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_perfil_seguridad, id_compania, descripcion, detalles, id_forma_inicio, id_usuario, habilitar, "", "" };

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
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Perfil</param>
        /// <param name="detalles">Detalles del Perfil</param>
        /// <param name="id_forma_inicio">Referencia de Forma de Inicio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPerfilSeguridad(int id_compania, string descripcion, string detalles, int id_forma_inicio, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania, descripcion, detalles, id_forma_inicio, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Perfiles de Seguridad
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Perfil</param>
        /// <param name="detalles">Detalles del Perfil</param>
        /// <param name="id_forma_inicio">Referencia de Forma de Inicio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public  RetornoOperacion EditaPerfilSeguridad(int id_compania, string descripcion, string detalles, int id_forma_inicio, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistroBD(id_compania, descripcion, detalles, id_forma_inicio, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Perfiles de Seguridad
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPerfilSeguridad(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistroBD(this._id_compania, this._descripcion, this._detalles, this._id_forma_inicio, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar el Perfil de Seguridad
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPerfilSeguridad()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_perfil_seguridad);
        }

        #endregion
    }
}
