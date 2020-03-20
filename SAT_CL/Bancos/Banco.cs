using System;
using TSDK.Base;
using System.Data;

namespace SAT_CL.Bancos
{
    /// <summary>
    /// Clase de la tabla bancos que permite crear operaciones sobre la tabla (inserciones,actualizaciones,consultas,etc.).
    /// </summary>
    public class Banco : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo privado que almacena el nombre del store procedure de la tabla banco
        /// </summary>
        private static string nom_sp = "bancos.sp_banco_tb";

        private int _id_banco;
        /// <summary>
        /// ID que corresponde al identificador de un banco
        /// </summary>
        public int id_banco
        {
            get { return _id_banco; }
        }

        private string _clave;
        /// <summary>
        /// Corresponde a una identificador clave otorgada por el sat
        /// </summary>
        public string clave
        {
            get { return _clave; }
        }

        private string _nombre_corto;
        /// <summary>
        /// Corresponde a una descripcion reducida de un banco
        /// </summary>
        public string nombre_corto
        {
            get { return _nombre_corto; }
        }

        private string _razon_social;
        /// <summary>
        /// Corresponde a la razón social de un banco
        /// </summary>
        public string razon_social
        {
            get { return _razon_social; }
        }

        private string _rfc;
        /// <summary>
        /// Corresponde al RFC de un banco
        /// </summary>
        public string rfc
        {
            get { return _rfc; }
        }

        private bool _bit_nacional;
        /// <summary>
        /// Permite identificar si un banco si es nacional o no
        /// </summary>
        public bool bit_nacional
        {
            get { return _bit_nacional; }
        }

        private bool _habilitar;
        /// <summary>
        /// Corresponde al estado de habilitación de un registro de un banco
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default que inicializa a los atributos privados
        /// </summary>
        public Banco()
        {
            this._id_banco = 0;
            this._clave = 
            this._nombre_corto = 
            this._razon_social = 
            this._rfc = "";
            this._bit_nacional = 
            this._habilitar = false;
        }

        /// <summary>
        /// Cosntructor que inicializa los atributos a través de la busqueda de registros mediante un id
        /// </summary>
        /// <param name="id_banco">Id que sirve como referencia para la busqueda de registros</param>
        public Banco(int id_banco)
        {
            //Invoca al método privado cargaAtributoInstancia
            cargaAtributoInstancia(id_banco);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase banco
        /// </summary>
        ~Banco()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metódo privado que Carga los atributos dado un registro
        /// </summary>
        /// <param name="id_banco">Id de referencia para la asignación de registros</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_banco)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            //Creación y Asignación de valores al arreglo, necesarios para el SP de la tabla 
            object[] param = { 3, id_banco, "", "", "", "", false, 0, false, "", "" };
            //Invoca al store procedure de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos, que existan en la tabla y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas y almacenamiento de registros en la variable r 
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_banco = id_banco;
                        this._clave = Convert.ToString(r["Clave"]);
                        this._nombre_corto = Convert.ToString(r["NombreCorto"]);
                        this._razon_social = Convert.ToString(r["RazonSocial"]);
                        this._rfc = r["RFC"].ToString();
                        this._bit_nacional = Convert.ToBoolean(r["BitNacional"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno, siempre y cuando se cumpla la sentencia la validación de datos
                    retorno = true;
                }
            }
            //Retorno del objeto al método
            return retorno;

        }
        /// <summary>
        /// Método privado que permite actualizar  registros de banco
        /// </summary>
        /// <param name="clave">Permite actualizar el campo clave</param>
        /// <param name="nombre_corto">Permite actualizar el campo nombre_corto</param>
        /// <param name="razon_social">Permite actualizar el campo razon_social</param>
        /// <param name="rfc">Permite actualizar el campo rfc</param>
        /// <param name="bit_nacional">Permite actualizar el campo bit_nacional</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <param name="habilitar">Permite actualizar el campo habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarBanco(string clave, string nombre_corto, string razon_social, string rfc,
                                             bool bit_nacional, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 2, this.id_banco, clave, nombre_corto, razon_social, rfc, bit_nacional, 
                               id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método público que permite insertar registros en banco
        /// </summary>
        /// <param name="clave">Permite insertar una clave otorgada por el sat a un banco</param>
        /// <param name="nombre_corto">Permite insertar un nombre corto a un banco</param>
        /// <param name="razon_social">Permite insertar la razon social de un banco</param>
        /// <param name="rfc">Permite insertar el RFC de un Banco</param>
        /// <param name="bit_nacional">Permite insertar si un banco es nacional o extranjero</param>
        /// <param name="id_usuario">Permite identificar a un usuario que realiza acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarBanco(string clave, string nombre_corto, string razon_social, string rfc,
                                                    bool bit_nacional, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 1, 0, clave, nombre_corto, razon_social, rfc, bit_nacional, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite Actualizar los registros de banco
        /// </summary>
        /// <param name="clave">Permite actualizar el campo clave </param>
        /// <param name="nombre_corto">Permite actualizar el campo nombre_corto</param>
        /// <param name="razon_social">Permite actualizar el campo razon social</param>
        /// <param name="rfc">Permite actualizar el campo rfc</param>
        /// <param name="bit_nacional">Permite actualizar el campo bit_nacional</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarBanco(string clave, string nombre_corto, string razon_social, string rfc, bool bit_nacional, int id_usuario)
        {
            //Retorna e Invoca el método privado editarBanco
            return this.editarBanco(clave, nombre_corto, razon_social, rfc, bit_nacional, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que Actualiza el estado de habilitación de un registro de banco
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarBanco(int id_usuario)
        {
            //Retorna e Invoca al método privado editarBanco
            return this.editarBanco(this._clave, this._nombre_corto, this._razon_social, this._rfc, this._bit_nacional, id_usuario, false);
        }

        #endregion
    }
}

