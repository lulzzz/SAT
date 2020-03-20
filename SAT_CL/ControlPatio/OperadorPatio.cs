using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.ControlPatio
{
    public class OperadorPatio : Disposable
    {

        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure usado por la  clase
        /// </summary>
        private static string _nombre_stored_procedure = "control_patio.sp_operador_patio_top";

        private int _id_operador_patio;
        /// <summary>
        /// Obtiene el Id de operador
        /// </summary>
        public int id_operador_patio { get { return this._id_operador_patio; } }

        private string _nombre;
        /// <summary>
        /// Obtiene el nombre del operador
        /// </summary>
        public string nombre { get { return this._nombre; } }

        private string _nombre_corto;
        /// <summary>
        /// Obtiene el nombre corto del operador
        /// </summary>
        public string nombre_corto { get { return this._nombre_corto; } }

        private int _id_usuario_sistema;
        /// <summary>
        /// Obtiene el Id de operador
        /// </summary>
        public int id_usuario_sistema { get { return this._id_usuario_sistema; } }

        private int _id_patio;
        /// <summary>
        /// Obtiene el Id de patio
        /// </summary>
        public int id_patio { get { return this._id_patio; } }

        private bool _bit_activo;
        /// <summary>
        /// Obtiene el valor de habilitación del usuario
        /// </summary>
        public bool bit_activo { get { return this._bit_activo; } }

        private int _id_usuario;
        /// <summary>
        /// Obtiene el Id de usuario
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }

        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del operador
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que se encarga de Inicializar los Atributos por Defecto
        /// </summary>
        public OperadorPatio()
        {
            this._id_operador_patio = 0;
            this._nombre= "";
            this._nombre_corto = "";
            this._id_usuario_sistema =  0;
            this._id_patio = 0;
            this._bit_activo = false;
            this._id_usuario = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Crea una nueva instancia del tipo Operador a partir del Id solicitado
        /// </summary>
        /// <param name="id_operador">Id de Usuario</param>
        public OperadorPatio(int id_operador)
        {
            cargaAtributosInstancia(id_operador);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Libera los recursos utilizados por la instancia
        /// </summary>
        ~OperadorPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la carga de los atributos de la instncia en base al Id de registro solicitado
        /// </summary>
        /// <param name="id_operador">Id de usuario</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_operador)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 3, id_operador, "", "", 0, 0, false, 0, false, "", "" };

            //Realizando consulta hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando para cada registro devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores sobre atributos
                        this._id_operador_patio = Convert.ToInt32(r["Id"]);
                        this._nombre = r["Nombre"].ToString();
                        this._nombre_corto = r["NombreCorto"].ToString();
                        this._id_usuario_sistema = Convert.ToInt32(r["IdUsuarioSistema"]);
                        this._id_patio = Convert.ToInt32(r["IdPatio"]);
                        this._bit_activo = Convert.ToBoolean(r["BitActivo"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Asignando Variables Positiva
                        resultado = true;
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Reguistra un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del operador</param>
        /// <param name="nombre_corto">Nombre corto del operador</param>
        /// <param name="id_usuario_sistema">Id Usuario Sistema</param>
        /// <param name="id_patio">Id Patio</param>
        /// <param name="bit_activo">Bit Activo estatus dentro del patio</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <param name="habilitar">Valor de habilitación del operador</param>
        /// <returns></returns>
        private RetornoOperacion editaOperador(string nombre, string nombre_corto, int id_usuario_sistema ,int id_patio, bool bit_activo, int id_usuario, bool habilitar)
        {

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 2, this._id_operador_patio, nombre, nombre_corto, id_usuario_sistema, id_patio, bit_activo, id_usuario, habilitar, "", "" };

            //Creando nuevo usuario en BD
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Reguistra un nuevo operador en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del operador</param>
        /// <param name="nombre_corto">Nombre corto del operador</param>
        /// <param name="id_usuario_sistema">Id Usuario Sistema</param>
        /// <param name="id_patio">Id Patio</param>
        /// <param name="bit_activo">Bit Activo estatus dentro del patio</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaOperador(string nombre, string nombre_corto, int id_usuario_sistema, int id_patio, bool bit_activo, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 1, 0, nombre, nombre_corto, id_usuario_sistema, id_patio, bit_activo, id_usuario, true, "", "" };

            //Creando nuevo usuario en BD
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Edita Operador Patio
        /// </summary>
        /// <param name="nombre"> Nombre del operador </param>
        /// <param name="nombre_corto"> Nombre corto del operador </param>
        /// <param name="id_usuario_sistema">Id Usuario Sistema</param>
        /// <param name="id_patio"> Id Patio </param>
        /// <param name="bit_activo"> Bit Activo estatus dentro del patio </param>
        /// <param name="id_usuario"> Id Usuario Actualiza </param>
        /// <returns></returns>
        public RetornoOperacion EditarOperadorPatio(string nombre, string nombre_corto, int id_usuario_sistema, int id_patio, bool bit_activo, int id_usuario)
        {
            return this.editaOperador(nombre, nombre_corto, id_usuario_sistema, id_patio, bit_activo, id_usuario, this.habilitar);

        }

        /// <summary>
        /// Deshabilita el registro operador
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaOperador(int id_usuario)
        {

            //Realizando actualización
            return this.editaOperador(this._nombre, this._nombre_corto, this._id_usuario_sistema, this._id_patio, this._bit_activo, id_usuario, false);

        }

        /// <summary>
        /// Realiza la actualización de los valores de atributos de la instancia volviendo a leer desde BD
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributos()
        {
            return cargaAtributosInstancia(this._id_operador_patio);
        }


        #endregion
    }
}