using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Tarifas
{
    /// <summary>
    /// Clase encargada de todas las operaciones de las Tarifas
    /// </summary>
    public class Tarifa : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los diferentes criterios de tabulación de tarifas disponibles
        /// </summary>
        public enum CriterioMatrizTarifa
        {
            /// <summary>
            /// Sin criterio de tabulación
            /// </summary>
            NoAplica = 0,
            /// <summary>
            /// Lugar de Carga Origen / Destino del servicio
            /// </summary>
            Ubicacion = 57,
            /// <summary>
            /// Producto transportado
            /// </summary>
            Producto = 60,
            /// <summary>
            /// Ciudad de Origen / Destino
            /// </summary>
            Ciudad = 1069,
            /// <summary>
            /// Kilometraje total recorrido
            /// </summary>
            Distancia = 1073,
            /// <summary>
            /// Número de paradas realizadas
            /// </summary>
            Paradas = 1074,
            /// <summary>
            /// Volumen total trasladado
            /// </summary>
            Volumen = 1075,
            /// <summary>
            /// Peso Total trasladado 
            /// </summary>
            Peso = 1076,
            /// <summary>
            /// Cantidad de piezas trasladadas
            /// </summary>
            Cantidad = 1077
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "tarifas.sp_tarifa_tt";

        private int _id_tarifa;
        /// <summary>
        /// Atributo encargado de Almacenar la Tarifa
        /// </summary>
        public int id_tarifa { get { return this._id_tarifa; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private int _id_base_tarifa;
        /// <summary>
        /// Atributo encargado de Almacenar la Tarifa Base
        /// </summary>
        public int id_base_tarifa { get { return this._id_base_tarifa; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private int _id_cliente_receptor;
        /// <summary>
        /// Atributo encargado de Almacenar el Cliente Receptor
        /// </summary>
        public int id_cliente_receptor { get { return this._id_cliente_receptor; } }
        private int _id_transportista;
        /// <summary>
        /// Atributo encargado de Almacenar el Transportista
        /// </summary>
        public int id_transportista { get { return this._id_transportista; } }
        private decimal _valor_unitario;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Unitario para tarifa fija cargado
        /// </summary>
        public decimal valor_unitario { get { return this._valor_unitario; } }
        private decimal _valor_unitario_vacio;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Unitario para tarifa fija vacío
        /// </summary>
        public decimal valor_unitario_vacio { get { return this._valor_unitario_vacio; } }
        private int _id_columna_filtro_col;
        /// <summary>
        /// Atributo encargado de Almacenar la Columna Filtro (Columna)
        /// </summary>
        public int id_columna_filtro_col { get { return this._id_columna_filtro_col; } }
        /// <summary>
        /// Tipo de filtrado para columna de matriz de tarifa
        /// </summary>
        public CriterioMatrizTarifa filtro_col { get { return (CriterioMatrizTarifa)this._id_columna_filtro_col; } }
        private int _id_columna_filtro_row;
        /// <summary>
        /// Atributo encargado de Almacenar la Columna Filtro (Celda)
        /// </summary>
        public int id_columna_filtro_row { get { return this._id_columna_filtro_row; } }
        /// <summary>
        /// Tipo de filtrado para fila de matriz de tarifa
        /// </summary>
        public CriterioMatrizTarifa filtro_row { get { return (CriterioMatrizTarifa)this._id_columna_filtro_row; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Tarifa()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public Tarifa(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Tarifa()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_tarifa = 0;
            this._descripcion = "";
            this._id_base_tarifa = 0;
            this._id_compania_emisor = 0;
            this._id_cliente_receptor = 0;
            this._id_transportista = 0;
            this._valor_unitario = 0;
            this._valor_unitario_vacio = 0;
            this._id_columna_filtro_col = 0;
            this._id_columna_filtro_row = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, "", 0, 0, 0, 0, 0, 0, 0, 0, null, null, 0, false, "", "" };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_tarifa = id_registro;
                        this._descripcion = dr["Descripcion"].ToString();
                        this._id_base_tarifa = Convert.ToInt32(dr["IdBaseTarifa"]);
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_cliente_receptor = Convert.ToInt32(dr["IdClienteReceptor"]);
                        this._id_transportista = Convert.ToInt32(dr["IdTransportista"]);
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]);
                        this._valor_unitario_vacio = Convert.ToDecimal(dr["ValorUnitarioVacio"]);
                        this._id_columna_filtro_col = Convert.ToInt32(dr["IdColumnaFiltroCol"]);
                        this._id_columna_filtro_row = Convert.ToInt32(dr["IdColumnaFiltroRow"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="descripcion">Descripción de la Tarifa</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="valor_unitario_vacio">Valor Unitario Vacio</param>
        /// <param name="id_columna_filtro_col">Columna Filtro (Columna)</param>
        /// <param name="id_columna_filtro_row">Columna Filtro (Celda)</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion, int id_base_tarifa, int id_compania_emisor, int id_cliente_receptor, int id_transportista,
                                 decimal valor_unitario, decimal valor_unitario_vacio, int id_columna_filtro_col, int id_columna_filtro_row, DateTime fecha_inicio,
                                 DateTime fecha_fin, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tarifa, descripcion,id_base_tarifa, id_compania_emisor, id_cliente_receptor, id_transportista, 
                                 valor_unitario, valor_unitario_vacio,id_columna_filtro_col, id_columna_filtro_row, fecha_inicio,
                                 fecha_fin, id_usuario, habilitar,"","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Tarifas
        /// </summary>
        /// <param name="descripcion">Descripción de la Tarifa</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="valor_unitario_vacio">Valor Unitario Vacio</param>
        /// <param name="id_columna_filtro_col">Columna Filtro (Columna)</param>
        /// <param name="id_columna_filtro_row">Columna Filtro (Celda)</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTarifa(string descripcion, int id_base_tarifa, int id_compania_emisor, int id_cliente_receptor, int id_transportista,
                                 decimal valor_unitario, decimal valor_unitario_vacio, int id_columna_filtro_col, int id_columna_filtro_row, DateTime fecha_inicio,
                                 DateTime fecha_fin, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, descripcion,id_base_tarifa, id_compania_emisor, id_cliente_receptor, id_transportista, 
                                 valor_unitario, valor_unitario_vacio,id_columna_filtro_col, id_columna_filtro_row, fecha_inicio,
                                 fecha_fin, id_usuario, true,"","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Tarifas
        /// </summary>
        /// <param name="descripcion">Descripción de la Tarifa</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="valor_unitario_vacio">Valor Unitario Vacio</param>
        /// <param name="id_columna_filtro_col">Columna Filtro (Columna)</param>
        /// <param name="id_columna_filtro_row">Columna Filtro (Celda)</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTarifa(string descripcion, int id_base_tarifa, int id_compania_emisor, int id_cliente_receptor, int id_transportista,
                                 decimal valor_unitario, decimal valor_unitario_vacio, int id_columna_filtro_col, int id_columna_filtro_row, DateTime fecha_inicio,
                                 DateTime fecha_fin, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion, id_base_tarifa, id_compania_emisor, id_cliente_receptor, id_transportista,
                                 valor_unitario, valor_unitario_vacio, id_columna_filtro_col, id_columna_filtro_row, fecha_inicio,
                                 fecha_fin, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Tarifas
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTarifa(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion, this._id_base_tarifa, this._id_compania_emisor, this._id_cliente_receptor, this._id_transportista,
                                 this._valor_unitario, this._valor_unitario_vacio, this._id_columna_filtro_col, this._id_columna_filtro_row, this._fecha_inicio,
                                 this._fecha_fin, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de la Tarifa
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTarifa()
        {   //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_tarifa);
        }
        /// <summary>
        /// Obtiene la primer tarifa de cobro aplicable a una factura de servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static Tarifa ObtieneTarifaCobroServicio(int id_servicio)
        { 
            //Definiendo objeto de retorno
            Tarifa tarifa = new Tarifa();

            //Cargando Tarifas coincidentes a descripción de servicio
            using (DataTable mit = ObtieneTarifasCobroServicio(id_servicio))
            {
                //Validando origen de datos
                if (mit != null)
                {
                    //Para cada resultado encontrado
                    foreach (DataRow t in mit.Rows)
                    {
                        tarifa = new Tarifa(Convert.ToInt32(t["IdTarifa"]));
                        break;
                    }
                }
            }

            //Devolviendo tarifa coincidente
            return tarifa;
        }
        /// <summary>
        /// Obtiene las tarifas de cobro aplicables a una factura de servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneTarifasCobroServicio(int id_servicio)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Creando arreglo de criterios de búsqueda
            object[] param = { 4, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, null, null, 0, false, id_servicio.ToString(), "" };

            //Cargando Tarifas coincidentes a descripción de servicio
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo tarifas coincidentes
                return mit;
            }
            
        }
        
        #endregion

    }
}
