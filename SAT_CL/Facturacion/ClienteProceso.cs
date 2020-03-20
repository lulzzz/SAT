using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Facturacion
{
    public class ClienteProceso : Disposable
    {
        #region Atributos

            /// <summary>
            /// Atributo encargado de Almacenar el Nombre del SP
            /// </summary>
        private static string _nom_sp = "facturacion.sp_cliente_proceso_tcp";

            /// <summary>
            /// Atributo encargado de Almacenar el Id del Registro
            /// </summary>
        private int _id_cliente_proceso;
        public int id_cliente_proceso { get {return this._id_cliente_proceso; }}

            /// <summary>
            /// Atributo encargado de Almacenar Id del Cliente
            /// </summary>
        private int _id_cliente;
        public int id_cliente { get { return this._id_cliente;} }

            /// <summary>
            /// Atributo encargado de Almacenar el Nombre del cliente con su Id Respectivo
            /// </summary>
        private string _nombreCliente;
        public string nombreCliente { get { return this._nombreCliente; } }


            /// <summary>
            /// Atributo encargado de Almacenar Id del Tipo de Proceso
            /// </summary>
        private int _id_tipo_proceso;
        public int id_tipo_proceso { get {return this._id_tipo_proceso; } }

            /// <summary>
            /// Atributo encargado de Almacenar Secuencia de Proceso
            /// </summary>
        private byte _secuencia;
        public byte secuencia { get { return this._secuencia; } }

            /// <summary>
            /// Atributo encargado de Almacenar Descripcion
            /// </summary>
        private string _descripcion;
        public string descripcion { get { return this._descripcion; } }

            /// <summary>
            /// Atributo encargado de Almacenar Contacto
            /// </summary>
        private string _contacto;
        public string contacto { get { return this._contacto; } }

            /// <summary>
            /// Atributo encargado de Almacenar Estatus Habilitar
            /// </summary>
        private bool _habilitar;
        public bool habilitar { get { return this._habilitar; } }

       

        #endregion

        #region Constructor

            /// <summary>
            /// Constructor encargado de Inicializar los Atributos por Defecto
            /// </summary>
        public ClienteProceso()
        {
            this._id_cliente_proceso = 0;
            this._id_cliente = 0;
            this._id_tipo_proceso = 0;
            this._secuencia = 0;
            this._descripcion = "";
            this._contacto = "";
            this._habilitar = false;
        }

            /// <summary>
            /// Constructor encargado de Inicializar los Atributos dado Un Id de un Registro
            /// </summary>
        public ClienteProceso(int id_cliente_proceso)
        {
            //Carga los atributos de la instancia
            cargaAtributosInstancia(id_cliente_proceso);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ClienteProceso()
        {
            Dispose(false);
        }

        #endregion 

        #region Metodos Privados

           /// <summary>
           /// Método encargado de Actualizar los Registros en BD
           /// </summary>
           /// <param name="id_cliente">Cliente del cual se actualizara registro</param>
           /// <param name="id_tipo_proceso">Proceso con el que se trabajara</param>
           /// <param name="id_usuario">Usuario Logueado</param>
           /// <param name="secuencia">Secuancia del proceso</param>
           /// <param name="descripcion">Descripcion</param>
           /// <param name="contacto">Contacto</param>
           /// <param name="habilitar">Estatus Habilitar</param>
           /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD( int id_cliente, int id_tipo_proceso, int id_usuario,
                                                     byte secuencia, string descripcion, string contacto , bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_cliente_proceso, id_cliente , id_tipo_proceso, id_usuario , secuencia, descripcion, contacto , habilitar };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return result;
        }

            /// <summary>
            /// Método encargado de Mostrar Registros de una tabla dado un ID especifico
            /// </summary>
            /// <param name="id_cliente">Cliente del cual se cargaran atributos</param>
            /// <returns></returns>
        private bool cargaAtributosInstancia(int id_cliente_proceso)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_cliente_proceso, 0, 0, 0, 0, "", "", false };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores

                        this._id_cliente_proceso = id_cliente_proceso;
                        this._id_cliente = Convert.ToInt32(dr["IdCliente"]);
                        this._nombreCliente = Convert.ToString(dr["NombreCliente"]) + " ID:" + this._id_cliente;
                        this._id_tipo_proceso = Convert.ToInt32(dr["IdTipoProceso"]);
                        this._secuencia = Convert.ToByte(dr["Secuencia"]);
                        this._descripcion = Convert.ToString(dr["Descripcion"]);
                        this._contacto = Convert.ToString(dr["Contacto"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Resultado Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
      
        #endregion 

        #region Metodos Publicos
            /// <summary>
            /// Método encargado de Insertar los Registros en BD
            /// </summary>
            /// <param name="id_cliente">Cliente con el que se trabajara el proceso</param>
            /// <param name="id_tipo_proceso">Proceso con el que se trabajara</param>
            /// <param name="id_usuario">Usuario Logueado</param>
            /// <param name="secuencia">Secuancia del proceso</param>
            /// <param name="descripcion">Descripcion</param>
            /// <param name="contacto">Contacto</param>
            /// <returns></returns>
        public static RetornoOperacion InsertaClienteProceso(int id_cliente, int id_tipo_proceso, int id_usuario, 
                                                         byte secuencia, string descripcion, string contacto)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_cliente, id_tipo_proceso , id_usuario , secuencia, descripcion, contacto , true };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Deshabilitar un registro en BD
        /// </summary>
        /// <param name="id_usuario">Usuario Logueado</param>
        /// <returns></returns>
        public  RetornoOperacion DeshabilitaClienteProceso(int id_usuario)
        {
            return actualizaRegistroBD(this._id_cliente, this._id_tipo_proceso, id_usuario, this._secuencia, this._descripcion, this._contacto, false);
        }

        /// <summary>
        /// Método encargado de Actualizar un registro en BD
        /// </summary>
        /// <param name="id_cliente">Cliente del cual se editara el registro</param>
        /// <param name="id_tipo_proceso">Proceso con el que se trabajara</param>
        /// <param name="id_usuario">Usuario Logueado</param>
        /// <param name="secuencia">Secuancia del proceso</param>
        /// <param name="descripcion">Descripcion</param>
        /// <param name="contacto">Contacto</param>
        public RetornoOperacion ActualizaRegistroBD (int id_cliente, int id_tipo_proceso, int id_usuario,
                                                         byte secuencia, string descripcion, string contacto)
        {
            return actualizaRegistroBD(id_cliente, id_tipo_proceso, id_usuario, secuencia, descripcion, contacto,this._habilitar);
        }

        #endregion 
    }
}
