using SAT_CL.FacturacionElectronica;
using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{
    /// <summary>
    /// Representa un registro Sucursal almacenado en BD
    /// </summary>
    public class Sucursal : Disposable
    {

        #region Atributos


        private static string _nombre_stored_procedure = "global.sp_sucursal_ts";

        /// <summary>
        /// Id de Registro Sucursal
        /// </summary>
        private int _id_sucursal;
        /// <summary>
        /// Obtiene el Id de registro sucursal al cual representa la instancia
        /// </summary>
        public int id_sucursal
        {
            get { return this._id_sucursal; }
        }

        /// <summary>
        /// Id de Emisor a la que pertenece
        /// </summary>
        private int _id_compania_emisor;
        /// <summary>
        /// Obtiene el Id de emisor al que pertenece la instancia
        /// </summary>
        public int id_compania_emisor
        {
            get { return this._id_compania_emisor; }
        }

        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        private string _nombre;
        /// <summary>
        /// Obtiene el nombre de la sucursal de la instancia
        /// </summary>
        public string nombre
        {
            get { return this._nombre; }
        }

        /// <summary>
        /// Id de Ubicación
        /// </summary>
        private int _id_direccion;
        /// <summary>
        /// Obtiene el Id de Ubicación de la sucursal
        /// </summary>
        public int id_direccion
        {
            get { return this._id_direccion; }
        }

        /// <summary>
        /// Valor de Habilitación de registro
        /// </summary>
        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del registro en la instancia
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }


        #endregion

        #region Contructores

        /// <summary>
        /// Genera una nueva instancia del tipo Sucursal
        /// </summary>
        public Sucursal()
        {
            this._id_sucursal = 0;
            this._id_direccion = 0;
            this._id_compania_emisor = 0;
            this._nombre = "";
            this._id_direccion = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Genera una nueva instancia del tipo Sucursal
        /// </summary>
        /// <param name="id_sucursal"></param>
        public Sucursal(int id_sucursal)
        {
            //Incializando instancia
            cargaAtributosInstancia(id_sucursal);
        }


        /// <summary>
        /// Método que inicializa los atributos de la instancia
        /// </summary>
        /// <param name="id_sucursal"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_sucursal)
        {
            //Declarando variable de retorno
            bool resultado = false;

            //Inicialziando parámetros para ejecución de SP
            object[] parametros = { 3, id_sucursal, 0, "", 0, 0, false, "", "" };

            //Cargando registro desde BD
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando el origen de datos obtenido 
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {
                    //Recorriendo las filas devueltas
                    foreach (DataRow fila in DS.Tables["Table"].Rows)
                    {
                        this._id_sucursal = id_sucursal;
                        //Inicializando atributos
                        this._id_compania_emisor = Convert.ToInt32(fila["IdEmisor"]);
                        this._nombre = fila["Nombre"].ToString();
                        this._id_direccion = Convert.ToInt32(fila["IdDireccion"]);
                        this._habilitar = Convert.ToBoolean(fila["Habilitar"]);

                        resultado = true;
                    }
                }

            }

            return resultado;
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~Sucursal()
        {
            Dispose(false);
        }
        #endregion


        #region Métodos Privados


        /// <summary>
        /// Edita una Sucursal
        /// </summary>
        /// <param name="id_compania_emisor"></param>
        /// <param name="nombre"></param>
        /// <param name="id_direccion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaSucursal(int id_compania_emisor, string nombre, int id_direccion, int id_usuario, bool habilitar)
        {
            //Declaramos Objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion();

             //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamo existencia de ubicación 
                if (!Validacion.ValidaOrigenDatos(Comprobante.RecuperaComprobantesPorSucursal(this._id_sucursal)))
                {
                    //Inicializando arreglo de parámetros
                    object[] param = { 2, this._id_sucursal, id_compania_emisor, nombre, id_direccion, id_usuario, habilitar, "", "" };

                    //Realizando actualizacion
                    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
                }
                else
                {
                    resultado = new RetornoOperacion("No puede ser editado el registro debido a que está ligado a un CFD.");
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }
            }

            //Devolvemos Resultado
            return resultado;

        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método que permite la inserción de datos en la tabla Sucursal
        /// </summary>
        /// <param name="id_compania_emisor">Permite la inserción de un identificador para una compañia emisora</param>
        /// <param name="nombre">Permite la inserción del nombre de una Sucursal</param>
        ///<param name="id_direccion">Direccion de la Sucursal</param>
        /// <param name="id_usuario">Permite la inserción del identificador del usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarSucursal(int id_compania_emisor, string nombre, int id_direccion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 1, 0, id_compania_emisor, nombre, id_direccion ,id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite la actualización de registros de Sucursal
        /// </summary>
        /// <param name="id_compania_emisor">Permite la actualización del identificador de una compañia</param>
        /// <param name="nombre">Permite la actualizacion de la nombre de una Sucursal</param>
        /// <param name="id_direccion">Dirrección de la Sucursal </param>
        /// <param name="id_usuario">Permite la actualización de un identificador del usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarSucursal(int id_compania_emisor, string nombre, int id_direccion, int id_usuario)
        {
            //Invoca y retorna el método editarSucursal().
            return this.editaSucursal(id_compania_emisor, nombre, id_direccion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite actualizar estado de habilitción o deshabilitación de un registro de Sucursal
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario quien realizo el cambio de estado del registro(Habilo/Deshabilito)</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarSucursal(int id_usuario)
        {
            //Invoca y retorna el método editaSucursal()-
            return this.editaSucursal(this._id_compania_emisor, this.nombre, id_usuario, this._id_direccion, false);
        }


        /// <summary>
        /// Método encargado de cargar la Compania Emisor
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <returns></returns>
        public static DataTable CargaSucursales(int id_compania_emisor)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Creación y asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 4, 0, id_compania_emisor, "", 0, 0, true, "", "" };
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dt;
        }
        #endregion
    }
}
