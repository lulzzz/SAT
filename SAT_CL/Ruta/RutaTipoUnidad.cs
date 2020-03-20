using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Transactions;
namespace SAT_CL.Ruta
{
    /// <summary>
    /// Clase que permite realizar acciones sobre una RutaTipoUbnidad (Insertar, Editar y Consultar)
    /// </summary>
    public class RutaTipoUnidad:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla RutaTipoUnidad
        /// </summary>
        private static string nom_sp = "ruta.sp_ruta_tipo_unidad_trtu";
        private int _id_ruta_tipo_unidad;
        /// <summary>
        /// Identifica el tipo de unidad asignada a una ruta.
        /// </summary>
        public int id_ruta_tipo_unidad
        {
            get { return _id_ruta_tipo_unidad; }
        }
        private int _id_ruta;
        /// <summary>
        /// Identifica a una ruta
        /// </summary>
        public int id_ruta
        {
            get { return _id_ruta; }
        }
        private byte _id_tipo_unidad;
        /// <summary>
        /// Define el tipo de unidad (Tractor,Caja,Pipa,Rabo, etc.).
        /// </summary>
        public byte id_tipo_unidad
        {
            get { return _id_tipo_unidad; }
        }
        private byte _id_configuracion;
        /// <summary>
        /// Define las diferentes combinaciones de unidades y dimensiones por tipo de unidad (Tractor de 43 pies con Caja de 28 pies, Tractor 28 pies,..., etc.)
        /// </summary>
        public byte id_configuracion
        {
            get { return _id_configuracion; }
        }
        private decimal _rendimiento;
        /// <summary>
        /// Almacena el calculo de kilometros recorridos por litros  de diesel
        /// </summary>
        public decimal rendimiento
        {
            get { return _rendimiento; }
        }
        private bool _habilitar;
        /// <summary>
        /// Define el estado de uso del registro (Habilitado-Disponible, Deshabilitado-No Disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos de la clase en cero
        /// </summary>
        public RutaTipoUnidad()
        {
            this._id_ruta_tipo_unidad = 0;
            this._id_ruta = 0;
            this._id_tipo_unidad = 0;
            this._id_configuracion = 0;
            this._rendimiento = 0.0m;
            this._habilitar = false;
        }
        /// <summary>
        /// Método que inicializa los atributos de la clase a partir de un registro de RutaTipoUnidad.
        /// </summary>
        /// <param name="id_ruta_tipo_unidad">Identificador del registro a consultar</param>
        public RutaTipoUnidad(int id_ruta_tipo_unidad)
        {
            //Invoca al método que realiza la consulta y asignación de valores a los atributos
            cargaAtributos(id_ruta_tipo_unidad);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~RutaTipoUnidad()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que inicializa los atributos a partir de la busqueda de un registro de RutaTipoUnidad
        /// </summary>
        /// <param name="id_ruta_tipo_unidad">Identificador que sirve como referencia para la busqueda del registro RutatipoUnidad </param>
        /// <returns></returns>
        private bool cargaAtributos(int id_ruta_tipo_unidad)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda del registro RutaTipoUnidad
            object[] param = { 3, id_ruta_tipo_unidad, 0, 0, 0, 0, 0, false, "", "" };
            //Realiza la Busuqeda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas del dataset y asigna a los atributos el resultado
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_ruta_tipo_unidad = id_ruta_tipo_unidad;
                        this._id_ruta = Convert.ToInt32(r["IdRuta"]);
                        this._id_tipo_unidad = Convert.ToByte(r["IdTipoUnidad"]);
                        this._id_configuracion = Convert.ToByte(r["IdConfiguracion"]);
                        this._rendimiento = Convert.ToDecimal(r["Rendimiento"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno al termino del recorrido del dataset
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de RutaTipoUnidad
        /// </summary>
        /// <param name="id_ruta">Actualiza el identificador de una ruta</param>
        /// <param name="id_tipo_unidad">Actualiza el tipo de uniadd (tractor, pipa,caja, etc.)</param>
        /// <param name="id_configuracion">Actualiza el tipo de configuración de una unidad</param>
        /// <param name="rendimiento">Actualiza el rendimiento de la unidad (kilometros recorridos por litro asignado)</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizó actualizaciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado-Disponible, Deshabilitado-No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarRutaTipoUnidad(int id_ruta, byte id_tipo_unidad, byte id_configuracion, decimal rendimiento, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos del registro a actualizar
            object[] param = { 2, this._id_ruta_tipo_unidad, id_ruta, id_tipo_unidad, id_configuracion, rendimiento, id_usuario, habilitar, "", "" };
            //Realiza la actualización del registro RutaTipoUnidad
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }

        #endregion

        #region Método Públicos
        /// <summary>
        /// Método que inserta un registro de RutaTipoUnidad
        /// </summary>
        /// <param name="id_ruta">Inserta el identificador de una ruta</param>
        /// <param name="id_tipo_unidad">Inserta el tipo de uniadd (tractor, pipa,caja, etc.)</param>
        /// <param name="id_configuracion">Inserta el tipo de configuración de una unidad</param>
        /// <param name="rendimiento">Inserta el rendimiento de la unidad (kilometros recorridos por litro asignado)</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizó actualizaciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarRutaTipoUnidad(int id_ruta, byte id_tipo_unidad, byte id_configuracion, decimal rendimiento, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos del registro a insertar
            object[] param = { 1, 0, id_ruta, id_tipo_unidad, id_configuracion, rendimiento, id_usuario, true, "", "" };
            //Realiza la insercion del registro RutaTipoUnidad
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de RutaTipoUnidad
        /// </summary>
        /// <param name="id_ruta">Actualiza el identificador de una ruta</param>
        /// <param name="id_tipo_unidad">Actualiza el tipo de unidad (tractor, pipa,caja, etc.)</param>
        /// <param name="id_configuracion">Actualiza el tipo de configuración de una unidad</param>
        /// <param name="rendimiento">Actualiza el rendimiento de la unidad (kilometros recorridos por litro asignado)</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo actualizaciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarRutaTipoUnidad(int id_ruta, byte id_tipo_unidad, byte id_configuracion, decimal rendimiento, int id_usuario)
        {
            //Retorna al método el resultado del método que actualiza los registro RutaTipoUnidad
            return this.editarRutaTipoUnidad(id_ruta, id_tipo_unidad, id_configuracion, rendimiento, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro (Habilitado-Disponible, Deshabilitado-No Disponible)
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo la acción</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaRutaTipoUnidad(int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
           
             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos Vales de Diesel
                using (DataTable mit = SAT_CL.Ruta.RutaUnidadDiesel.CargaVales(this._id_ruta_tipo_unidad))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recorremos Vales
                        foreach (DataRow r in mit.Rows)
                        {
                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos Vale
                                using (RutaUnidadDiesel objVale = new RutaUnidadDiesel(r.Field<int>("Id")))
                                {
                                    //Deshabilitamos Vale
                                    resultado = objVale.DeshabilitaRutaUnidadDiesel(id_usuario);
                                }
                            }
                            else
                            {
                                //Salimos del Ciclo
                                break;
                            }

                        }
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Retorna al método el resultado del método que actualiza los registro RutaTipoUnidad
                    resultado = editarRutaTipoUnidad(this._id_ruta, this._id_tipo_unidad, this._id_configuracion, this._rendimiento, id_usuario, false);
                }
                //Validamos resultado
                if(resultado.OperacionExitosa)
                {
                    //Finaizamos Transaccion
                    scope.Complete();
                }
            }
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRutaTipoUnidad()
        {
            //Invoca al método que asigna valores a los atributos de la clase
            return this.cargaAtributos(this._id_ruta_tipo_unidad);
        }

        /// <summary>
        ///  Carga Tipo de Unidad
        /// </summary>
        /// <param name="id_ruta"></param>
        /// <returns></returns>
        public static DataTable CargaTipoUnidad(int id_ruta)
        {
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda del registro RutaTipoUnidad
            object[] param = { 4, 0, id_ruta, 0, 0, 0, 0, false, "", "" };
        
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Definiendo objeto de retorno
                DataTable mit = null;

                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }


        /// <summary>
        /// Carga los Vales Ligando Un Segmento, Ruta, Tipo Unidad, Configuracion
        /// </summary>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_segmento">Id Segmento</param>
        /// <param name="tipo_unidad">Tipo Unidad</param>
        /// <param name="configuracion">Configuracion</param>
        /// <returns></returns>
        public static DataTable CargaValesRutaSegmento(int id_ruta, int id_segmento, string tipo_unidad, string configuracion)
        {
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda del registro RutaTipoUnidad
            object[] param = { 5, 0, id_ruta, 0, 0, 0, id_segmento, false, configuracion, tipo_unidad };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Definiendo objeto de retorno
                DataTable mit = null;

                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        #endregion
    }
}
