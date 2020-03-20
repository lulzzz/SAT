using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Proporciona los medios para la administración de entidades UsuarioCompania
    /// </summary>
    public class UsuarioCompania : Disposable
    {
        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "seguridad.sp_usuario_compania_tuc";


        private int _id_usuario_compania;
        /// <summary>
        /// Describe el Id de Usuraio Compañia
        /// </summary>
        public int id_usuario_compania
        {
            get { return _id_usuario_compania; }
        }

        private int _id_usuario;
        /// <summary>
        /// Describe el Id de Usuario
        /// </summary>
        public int id_usuario
        {
            get { return _id_usuario; }
        }

        private int _id_compania_emisor_receptor;
        /// <summary>
        /// Describe la Compañia Emisor Receptor
        /// </summary>
        public int id_compania_emisor_receptor
        {
            get { return _id_compania_emisor_receptor; }
        }
        private int _id_departamento;
        /// <summary>
        /// Describe el Departamento
        /// </summary>
        public int id_departamento
        {
            get { return _id_departamento; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~UsuarioCompania()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public UsuarioCompania()
        {

        }

        /// <summary>
        /// Genera una Instancia UsuarioCompania ligando un Id 
        /// </summary>
        /// <param name="id_usuario_compania">Id</param>
        public UsuarioCompania(int id_usuario_compania)
        {
            cargaAtributosInstancia(id_usuario_compania);
        }

        /// <summary>
        /// Genera una Instancia UsuarioCompania ligando un Id  Usuario, Id Compania
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="id_compania"></param>
        public UsuarioCompania(int id_usuario, int id_compania)
        {
            cargaAtributosInstancia(id_usuario, id_compania);
        }

        #endregion

        #region Metodos privados

        /// <summary>
        /// Genera una Instancia UsuarioCompania ligando un Id 
        /// </summary>
        /// <param name="id_usuario_compania"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_usuario_compania)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_usuario_compania, 0, 0, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_usuario_compania = Convert.ToInt32(r["Id"]);
                        _id_usuario = Convert.ToInt32(r["IdUsuario"]);
                        _id_compania_emisor_receptor = Convert.ToInt32(r["IdCompaniaReceptor"]);
                        _id_departamento = Convert.ToInt32(r["IdDepartamento"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);


                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Genera una Instancia UsuarioCompania ligando un Usuario y una Compañia
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_usuario, int id_compania)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 4, 0, id_usuario, id_compania, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_usuario_compania = Convert.ToInt32(r["Id"]);
                        _id_usuario = Convert.ToInt32(r["IdUsuario"]);
                        _id_compania_emisor_receptor = Convert.ToInt32(r["IdCompaniaReceptor"]);
                        _id_departamento = Convert.ToInt32(r["IdDepartamento"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);


                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Edita un Usuario Compañia
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="id_compania_emisor_receptor">Id Compañia</param>
        /// <param name="id_departamento">Id Departamento</param>
        /// <param name="id_usuario_actualiza">Id Usuario actualiza</param>
        /// <param name="habilitar">habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaUsuarioCompania(int id_usuario, int id_compania_emisor_receptor, int id_departamento, int id_usuario_actualiza, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_usuario_compania, id_usuario, id_compania_emisor_receptor, id_departamento, id_usuario_actualiza, habilitar, "", "" };

            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);


        }
        #endregion

        #region Metodos publicos

        /// <summary>
        /// Inserta Usuario Compania
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="id_compania_emisor_receptor">Id Compañia</param>
        /// <param name="id_departamento">Id Departamento</param>
        /// <param name="id_usuario_actualiza">Id Usuario actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUsuarioCompania(int id_usuario, int id_compania_emisor_receptor, int id_departamento, int id_usuario_actualiza)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_usuario, id_compania_emisor_receptor, id_departamento, id_usuario_actualiza, true, "", "" };

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }
        /// <summary>
        /// Edita Usuario Compania
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="id_compania_emisor_receptor">Id Compañia</param>
        /// <param name="id_departamento">Id Departamento</param>
        /// <param name="id_usuario_actualiza">Id Usuario actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditaUsuarioCompania(int id_usuario, int id_compania_emisor_receptor, int id_departamento, int id_usuario_actualiza)
        {
            return this.editaUsuarioCompania(id_usuario, id_compania_emisor_receptor, id_departamento, id_usuario_actualiza, this._habilitar);
        }
        /// <summary>
        /// Deshabilita Usuario Compania
        /// </summary>
        /// <param name="id_usuario_actualiza">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUsuarioCompania(int id_usuario_actualiza)
        {
            return this.editaUsuarioCompania(this._id_usuario, this._id_compania_emisor_receptor, this._id_departamento, id_usuario_actualiza, false);
        }
        /// <summary>
        /// Actualiza atributos Usuario Compania
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUsuarioCompania()
        {
            return this.cargaAtributosInstancia(this._id_usuario_compania);
        }
        /// <summary>
        /// Obtiene las compañías a las que está adscrito el usuario
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static DataTable ObtieneCompaniasUsuario(int id_usuario)
        { 
            //Declarando objeto de resultado
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, id_usuario, 0, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }            
        }

        #endregion
    }
}
