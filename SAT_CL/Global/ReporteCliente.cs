using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAT_CL;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;


namespace SAT_CL.Global
{
    public class ReporteCliente : Disposable
    {
        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nom_sp = "global.sp_reporte_cliente_trc";

        private int _id_reporte_cliente;
        /// <summary>
        /// Atributo encargado de Almacenar el Reporte Cliente
        /// </summary>
        public int id_reporte_cliente { get { return this._id_reporte_cliente; } }

        private int _id_compania;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Compania 
        /// </summary>
        public int id_compania { get { return this._id_compania; } }

        private int _id_compania_cliente;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Cliente
        /// </summary>
        public int id_compania_cliente { get { return this._id_compania_cliente; } }

        private int _id_compania_reporte;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Reporte
        /// </summary>
        public int id_compania_reporte { get { return this._id_compania_reporte; } }

        private int _id_tipo_reporte;
        /// <summary>
        /// Atributo encargado de Almacenar el Id Tipo de Reporte
        /// </summary>
        public int id_tipo_reporte { get { return this._id_tipo_reporte; } }

        private int _id_perfil;
        /// <summary>
        /// Atributo encargado de Almacenar el Id Perfil
        /// </summary>
        public int id_perfil { get { return this._id_perfil; } }

        private int _id_usuario;
        /// <summary>
        /// Atributo encargado de Almacenar el Id Usuario 
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar El Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ReporteCliente()
        {   //Asignando Valores
            this._id_reporte_cliente = 0;
            this._id_compania = 0;
            this._id_compania_cliente = 0;
            this._id_compania_reporte = 0;
            this._id_tipo_reporte = 0;
            this._id_perfil = 0;
            this._id_usuario = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_reporte_cliente">Id de Operador</param>
        public ReporteCliente(int id_reporte_cliente)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_reporte_cliente);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ReporteCliente()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_reporte_cliente">Id de Operador</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_reporte_cliente)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_reporte_cliente, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_reporte_cliente = id_reporte_cliente;
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._id_compania_cliente = Convert.ToInt32(dr["IdCliente"]);
                        this._id_compania_reporte = Convert.ToInt32(dr["IdReporte"]);
                        this._id_tipo_reporte = Convert.ToInt32(dr["IdTipoReporte"]);
                        this._id_perfil = Convert.ToInt32(dr["IdPerfil"]);
                        this._id_usuario = Convert.ToInt32(dr["IdUsuario"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Actualiza Operador
        /// </summary>
        /// <param name="id_compania">Id Compañia Emisor</param>
        /// <param name="id_compania_cliente">Id Cliente</param>
        /// <param name="id_compania_reporte">Id Reporte</param>
        /// <param name="id_tipo_reporte">Id Tipo Reporte</param>
        /// <param name="id_usuario">Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaRegistro(int id_compania, int id_compania_cliente, int id_compania_reporte, int id_tipo_reporte, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Armando Objeto de parametros
            object[] param = { 2, this._id_reporte_cliente, id_compania, id_compania_cliente, id_compania_reporte, id_tipo_reporte, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return resultado;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Reguistra un nuevo operador en la base de datos
        /// </summary>
        /// <param name="id_compania">Id Compañia Emisor</param>
        /// <param name="id_compania_cliente">Id Cliente</param>
        /// <param name="id_compania_reporte">Id Reporte</param>
        /// <param name="id_tipo_reporte">Id Tipo Reporte</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaOperador(int id_compania, int id_compania_cliente, int id_compania_reporte, int id_tipo_reporte, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 1, 0, id_compania, id_compania_cliente, id_compania_reporte, id_tipo_reporte, id_usuario, true, "", "" };

            //Creando nuevo usuario en BD
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Edita Operador Patio
        /// </summary>
        /// <param name="id_compania">Id Compañia Emisor</param>
        /// <param name="id_compania_cliente">Id Cliente</param>
        /// <param name="id_compania_reporte">Id Reporte</param>
        /// <param name="id_tipo_reporte">Id Tipo Reporte</param>
        /// <param name="id_usuario"> Id Usuario Actualiza </param>
        /// <returns></returns>
        public RetornoOperacion EditarReporteCliente(int id_compania, int id_compania_cliente, int id_compania_reporte, int id_tipo_reporte, int id_usuario)
        {
            return this.editaRegistro(id_compania, id_compania_cliente, id_compania_reporte, id_tipo_reporte, id_usuario, this.habilitar);

        }

        /// <summary>
        /// Deshabilita el registro operador
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaOperador(int id_usuario)
        {

            //Realizando actualización
            return this.editaRegistro(this._id_compania, this._id_compania_cliente, this._id_compania_reporte, this._id_tipo_reporte, id_usuario, false);

        }

        /// <summary>
        /// Realiza la actualización de los valores de atributos de la instancia volviendo a leer desde BD
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributos()
        {
            return cargaAtributosInstancia(this._id_reporte_cliente);
        }


        #endregion
    }
}
