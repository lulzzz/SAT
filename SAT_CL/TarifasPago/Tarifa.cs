using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.TarifasPago
{
    /// <summary>
    /// Proporciona los medios para la adminsitración de recursos Tarifa del módulo Tarifas de Pago
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
        /// <summary>
        /// Define el nivel de pago que tendrá una tarifa
        /// </summary>
        public enum NivelPago
        { 
            /// <summary>
            /// Pago por todos los movimientos de un servicio
            /// </summary>
            Servicio = 1,
            /// <summary>
            /// Pago para un movimiento ligado a un servicio
            /// </summary>
            Movimiento = 2,
            /// <summary>
            /// Pago para un movimiento sin servicio asociado
            /// </summary>
            Mov_Vacio = 3
        }
        /// <summary>
        /// Define a los perfiles de pago aplicables para un a tarifa
        /// </summary>
        public enum PerfilPago
        {
            /// <summary>
            /// Tarifa que paga a una Unidad de Permisionario
            /// </summary>
            Unidad = 1,
            /// <summary>
            /// Tarifa que paga a un operador de casa
            /// </summary>
            Operador = 2,
            /// <summary>
            /// Tarifa que paga a un Transportista
            /// </summary>
            Transportista = 3
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "tarifas_pago.sp_tarifa_tt";

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
        private byte _id_nivel_pago;
        /// <summary>
        /// Obtiene el Id de Nivel de pago al que se aplica (Servicio, Movimiento, Mov. Vacío)
        /// </summary>
        public byte id_nivel_pago { get { return this._id_nivel_pago; } }
        public NivelPago nivel_pago { get { return (NivelPago)this._id_nivel_pago; } }
        private byte _id_perfil_pago;
        /// <summary>
        /// Obtiene el Id de Perfil de asignación de tarifa
        /// </summary>
        public byte id_perfil_pago { get { return this._id_perfil_pago; } }
        public PerfilPago perfil_pago { get { return (PerfilPago)this._id_perfil_pago; } }
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
        private decimal _valor_unitario;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Unitario para tarifa cargado
        /// </summary>
        public decimal valor_unitario { get { return this._valor_unitario; } }
        private decimal _valor_unitario_vacio;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Unitario para tarifa vacío
        /// </summary>
        public decimal valor_unitario_vacio { get { return this._valor_unitario_vacio; } }
        private decimal _valor_unitario_tronco;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Unitario para tarifa tronco
        /// </summary>
        public decimal valor_unitario_tronco { get { return this._valor_unitario_tronco; } }
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
            this._id_nivel_pago = 0;
            this._id_perfil_pago = 0;
            this._id_compania_emisor = 0;
            this._id_cliente_receptor = 0;
            this._valor_unitario = 0;
            this._valor_unitario_vacio = 0;
            this._valor_unitario_tronco = 0;
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
            object[] param = { 3, id_registro, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, 0, false, "", "" };
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
                        this._id_nivel_pago = Convert.ToByte(dr["IdNivelPago"]);
                        this._id_perfil_pago = Convert.ToByte(dr["IdPerfilPago"]);
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_cliente_receptor = Convert.ToInt32(dr["IdClienteReceptor"]);
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]);
                        this._valor_unitario_vacio = Convert.ToDecimal(dr["ValorUnitarioVacio"]);
                        this._valor_unitario_tronco = Convert.ToDecimal(dr["ValorUnitarioTronco"]);
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
        /// <param name="nivel_pago">Nivel de pago</param>
        /// <param name="perfil_pago">Perfil de pago</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="valor_unitario_vacio">Valor Unitario Vacio</param>
        /// <param name="valor_unitario_tronco">Valor Unitario en Tronco</param>
        /// <param name="id_columna_filtro_col">Columna Filtro (Columna)</param>
        /// <param name="id_columna_filtro_row">Columna Filtro (Celda)</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion, int id_base_tarifa, NivelPago nivel_pago, PerfilPago perfil_pago, int id_compania_emisor, int id_cliente_receptor,
                                 decimal valor_unitario, decimal valor_unitario_vacio, decimal valor_unitario_tronco, int id_columna_filtro_col, int id_columna_filtro_row, DateTime fecha_inicio,
                                 DateTime fecha_fin, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tarifa, descripcion,id_base_tarifa, (byte)nivel_pago, (byte)perfil_pago, id_compania_emisor, id_cliente_receptor, 
                                 valor_unitario, valor_unitario_vacio, valor_unitario_tronco, id_columna_filtro_col, id_columna_filtro_row, fecha_inicio,
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
        /// <param name="nivel_pago">Nivel de Pago</param>
        /// <param name="perfil_pago">Perfil de Pago</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="valor_unitario_vacio">Valor Unitario Vacio</param>
        /// <param name="valor_unitario_tronco">Valor Unitario en Tronco</param>
        /// <param name="id_columna_filtro_col">Columna Filtro (Columna)</param>
        /// <param name="id_columna_filtro_row">Columna Filtro (Celda)</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTarifa(string descripcion, int id_base_tarifa, NivelPago nivel_pago, PerfilPago perfil_pago, int id_compania_emisor, int id_cliente_receptor,
                                 decimal valor_unitario, decimal valor_unitario_vacio, decimal valor_unitario_tronco, int id_columna_filtro_col, int id_columna_filtro_row, DateTime fecha_inicio,
                                 DateTime fecha_fin, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, descripcion, id_base_tarifa, (byte)nivel_pago, (byte)perfil_pago, id_compania_emisor, id_cliente_receptor, 
                                 valor_unitario, valor_unitario_vacio, valor_unitario_tronco, id_columna_filtro_col, id_columna_filtro_row, fecha_inicio,
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
        /// <param name="nivel_pago">Nivel de Pago</param>
        /// <param name="perfil_pago">Perfil de Pago</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="valor_unitario_vacio">Valor Unitario Vacio</param>
        /// <param name="valor_unitario_tronco">Valor Unitario en Tronco</param>
        /// <param name="id_columna_filtro_col">Columna Filtro (Columna)</param>
        /// <param name="id_columna_filtro_row">Columna Filtro (Celda)</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTarifa(string descripcion, int id_base_tarifa, NivelPago nivel_pago, PerfilPago perfil_pago, int id_compania_emisor, int id_cliente_receptor,
                                 decimal valor_unitario, decimal valor_unitario_vacio, decimal valor_unitario_tronco, int id_columna_filtro_col, int id_columna_filtro_row, DateTime fecha_inicio,
                                 DateTime fecha_fin, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion, id_base_tarifa, nivel_pago, perfil_pago, id_compania_emisor, id_cliente_receptor,
                                 valor_unitario, valor_unitario_vacio, valor_unitario_tronco, id_columna_filtro_col, id_columna_filtro_row, fecha_inicio,
                                 fecha_fin, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Tarifas
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTarifa(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion, this._id_base_tarifa, this.nivel_pago, this.perfil_pago, this._id_compania_emisor, this._id_cliente_receptor,
                                 this._valor_unitario, this._valor_unitario_vacio, this._valor_unitario_tronco, this._id_columna_filtro_col, this._id_columna_filtro_row, this._fecha_inicio,
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
        /// Obtiene el conjunto de tarifas de pago aplicables a los criterios señalados
        /// </summary>     
        /// <param name="nivel_pago">Nivel de Pago correspondiente (En base al Id de Servicio o Movimiento)</param>
        /// <param name="id_servicio_movimiento">Id de Servicio o movimiento</param>        
        /// <param name="perfil_pago">Perfil de Pago (En base al Id de Entidad de Pago)</param>
        /// <param name="id_entidad_pago">Id de Entidad a la que se debe pagar (Unidad, Operador o Transportista)</param>
        /// <returns></returns>
        private static DataTable obtieneTarifasPago(NivelPago nivel_pago, int id_servicio_movimiento, PerfilPago perfil_pago, int id_entidad_pago)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Determinando datos complementarios en base al nivel de pago
            if (nivel_pago == NivelPago.Movimiento)
            {
                //Instanciando movimiento indicado
                using (Despacho.Movimiento mov = new Despacho.Movimiento(id_servicio_movimiento))
                    //Si el movimiento existe y no es un movimiento vacío
                    if (mov.habilitar && mov.id_servicio > 0)
                        //Asignando Id de Servicio a registro de búsqueda
                        id_servicio_movimiento = mov.id_servicio;
            }

            //Creando conjunto de criterios de búsqueda en blanco
            object[] param = { 4, 0, "", 0, (byte)nivel_pago, (byte)perfil_pago, 0, 0, 0, 0, 0, 0, 0, null, null, 0, false, id_servicio_movimiento, id_entidad_pago };

            //Realizando carga de tarifas coincidentes
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obteniendo tabla de interés
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene el conjunto de tarifas de pago aplicables a los criterios señalados
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento</param>        
        /// <param name="perfil_pago">Perfil de Pago (En base al Id de Entidad de Pago)</param>
        /// <param name="id_entidad_pago">Id de Entidad a la que se debe pagar (Unidad, Operador o Transportista)</param>
        /// <param name="escalarAServicio">True para indicar que si no existen tarifas coincidentes a nivel de movimiento, se debe realizar una búsqueda a nivel de servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneTarifasPagoMovimiento(int id_movimiento, PerfilPago perfil_pago, int id_entidad_pago, bool escalarAServicio)
        {
            //Declarando objeto de retorno
            DataTable mit = null;
            //Declarando auxiliares de búsqueda (inicializadas a movimiento vacío)
            NivelPago nivel_pago = NivelPago.Mov_Vacio;
            int id_servicio_movimiento = id_movimiento;

            //Instanciando el movimiento solicitado
            using (Despacho.Movimiento mov = new Despacho.Movimiento(id_movimiento))
            {
                //Si el movimiento existe
                if (mov.habilitar)
                { 
                    //Determiando si el movimiento está ligado a un servicio
                    if (mov.id_servicio > 0)
                    { 
                        //Actualizando parámetros de búsqueda
                        nivel_pago = NivelPago.Movimiento;
                        id_servicio_movimiento = mov.id_servicio;
                    }
                }
            }
            
            //Realizando búsqueda por movimiento
            mit = obtieneTarifasPago(nivel_pago, id_servicio_movimiento, perfil_pago, id_entidad_pago);

            //Si no hay tarifas coincidentes y se ha solicitado búsqueda a otro nivel
            if (!Validacion.ValidaOrigenDatos(mit) && nivel_pago != NivelPago.Mov_Vacio && escalarAServicio)
                //Realizando búsqueda a nivel de servicio
                mit = obtieneTarifasPago(NivelPago.Servicio, id_servicio_movimiento, perfil_pago, id_entidad_pago);

            //Devolviendo resultado
            return mit;
        }
        /// <summary>
        /// Obtiene el conjunto de tarifas de pago aplicables a los criterios señalados
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>        
        /// <param name="perfil_pago">Perfil de Pago (En base al Id de Entidad de Pago)</param>
        /// <param name="id_entidad_pago">Id de Entidad a la que se debe pagar (Unidad, Operador o Transportista)</param>
        /// <param name="escalarAMovimiento">True para indicar que si no existen tarifas coincidentes a nivel de servicio, se debe realizar una búsqueda a nivel de movimiento</param>
        /// <returns></returns>
        public static DataTable ObtieneTarifasPagoServicio(int id_servicio, PerfilPago perfil_pago, int id_entidad_pago, bool escalarAMovimiento)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Realizando búsqueda por servicio
            mit = obtieneTarifasPago(NivelPago.Servicio, id_servicio, perfil_pago, id_entidad_pago);

            //Si no hay tarifas coincidentes y se ha solicitado búsqueda a otro nivel
            if (!Validacion.ValidaOrigenDatos(mit) && escalarAMovimiento)
                //Realizando búsqueda a nivel de movimiento
                mit = obtieneTarifasPago(NivelPago.Movimiento, id_servicio, perfil_pago, id_entidad_pago);

            //Devolviendo resultado
            return mit;
        }

        /*
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

        }*/

        #endregion

    }
}
