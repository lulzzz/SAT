using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de las operaciones correspondientes al Contacto
    /// </summary>
    public  class Contacto:Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_contacto_tc";

        private int _id_contacto;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Contacto
        /// </summary>
        public int id_contacto { get { return this._id_contacto; } }
        
         private string _nombre;
        /// <summary>
        /// Atributo encargado de Almacenar el Nombre
        /// </summary>
        public string nombre { get { return this._nombre; } }
        
        private string _telefono;
        /// <summary>
        /// Atributo encargado de Almacenar el Telefono
        /// </summary>
        public string telefono { get { return this._telefono; } }

         private string _email;
        /// <summary>
        /// Atributo encargado de Almacenar el E-Mail
        /// </summary>
        public string email { get { return this._email; } }

         private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compañia Emisor
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }       

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estado del Registro
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }


        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Contacto()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id Registro</param>
        public Contacto(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Contacto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {
            //Asignando Valores
            this._id_contacto = 0; 
            this._nombre = ""; 
            this._telefono = "";
            this._email =""; 
            this._id_compania_emisor = 0;
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
            //Armando Arreglo de Parametros
            object[] param = {3, id_registro, "", "", "", 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        this._id_contacto = Convert.ToInt32(dr["Id"]);
                        this._nombre = dr["Nombre"].ToString();
                        this._telefono = dr["Telefono"].ToString();
                        this._email = dr["EMail"].ToString();
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        //Asignando Resultado Positivo
                        result = true;
                    }
                }
                //Devolviendo resultado Obtenido
                return result;
            }
        }

        /// <summary>
        /// Actualiza Registros del Contacto
        /// </summary>
        /// <param name="nombre">Nombre</param>
        /// <param name="telefono">Telefono</param>
        /// <param name="email">E-Mail</param>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string nombre, string telefono, string email,int id_compania_emisor,int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_contacto, nombre, telefono, email, id_compania_emisor, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inserta un Contacto
        /// </summary>
        /// <param name="nombre">Nombre</param>
        /// <param name="telefono">Telefono</param>
        /// <param name="email">E-Mail</param>
        /// <param name="id_compania_emisor">Compañia Emisor</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaContacto(string nombre, string telefono, string email, int id_compania_emisor, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, nombre, telefono, email, id_compania_emisor, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Editar un Contacto
        /// </summary>
        /// <param name="nombre">Nombre</param>
        /// <param name="telefono">Telefono</param>
        /// <param name="email">E-Mail</param>
        /// <param name="id_compania_emisor">Compañia Emisor</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaContacto(string nombre, string telefono, string email, int id_compania_emisor, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(nombre,  telefono,  email,  id_compania_emisor,  id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar un Contacto
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaContacto(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._nombre, this._telefono, this._email, this._id_compania_emisor, id_usuario, false);
        }

        /// <summary>
        /// Método Público encargado de Actualizar el Contacto
        /// </summary>
        /// <returns></returns>
        public bool ActualizaContacto()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_contacto);
        }



        #endregion
    }

}
