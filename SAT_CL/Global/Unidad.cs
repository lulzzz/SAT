using Microsoft.SqlServer.Types;
using SAT_CL.Despacho;
using SAT_CL.Maps;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con las Unidades
    /// </summary>
    public class Unidad : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Enumera el Estatus 
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            ///  Parada Disponible
            /// </summary>
            ParadaDisponible = 1,
            /// <summary>
            /// Parada Ocupado
            /// </summary>
            ParadaOcupado,
            /// <summary>
            /// Transito
            /// </summary>
            Transito,
            /// <summary>
            /// Baja
            /// </summary>
            Baja,

        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_unidad_tu";

        private int _id_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Unidad
        /// </summary>
        public int id_unidad { get { return this._id_unidad; } }
        private string _numero_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Número de la Unidad
        /// </summary>
        public string numero_unidad { get { return this._numero_unidad; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private byte _id_estatus_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de la Unidad
        /// </summary>
        public byte id_estatus_unidad { get { return this._id_estatus_unidad; } }
        private int _id_tipo_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Unidad
        /// </summary>
        public int id_tipo_unidad { get { return this._id_tipo_unidad; } }
        private byte _id_sub_tipo_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Sub Tipo
        /// </summary>
        public byte id_sub_tipo_unidad { get { return this._id_sub_tipo_unidad; } }
        private byte _ejes;
        /// <summary>
        /// Atributo encargado de almacenar la Ejes
        /// </summary>
        public byte ejes { get { return this._ejes; } }
        private int _id_dimension;
        /// <summary>
        /// Atributo encargado de almacenar la Dimension
        /// </summary>
        public int id_dimension { get { return this._id_dimension; } }
        private bool _bit_no_propia;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de Propiedad
        /// </summary>
        public bool bit_no_propia { get { return this._bit_no_propia; } }
        private DateTime _fecha_adquisicion;
        /// <summary>
        /// Obtiene la fecha de adquisición de la unidad
        /// </summary>
        public DateTime fecha_adquisicion { get { return this._fecha_adquisicion; } }
        private DateTime _fecha_baja;
        /// <summary>
        /// Obtiene la fecha de baja de la unidad
        /// </summary>
        public DateTime fecha_baja { get { return this._fecha_baja; } }
        private int _id_compania_proveedor;
        /// <summary>
        /// Atributo encargado de almacenar el compania_proveedor
        /// </summary>
        public int id_compania_proveedor { get { return this._id_compania_proveedor; } }
        private byte _id_marca;
        /// <summary>
        /// Atributo encargado de almacenar la Marca
        /// </summary>
        public byte id_marca { get { return this._id_marca; } }
        private string _modelo;
        /// <summary>
        /// Atributo encargado de almacenar el Modelo
        /// </summary>
        public string modelo { get { return this._modelo; } }
        private int _ano;
        /// <summary>
        /// Atributo encargado de almacenar el Ano
        /// </summary>
        public int ano { get { return this._ano; } }
        private string _serie;
        /// <summary>
        /// Atributo encargado de almacenar la Serie
        /// </summary>
        public string serie { get { return this._serie; } }
        private byte _id_marca_motor;
        /// <summary>
        /// Atributo encargado de almacenar la Marca del Motor
        /// </summary>
        public byte id_marca_motor { get { return this._id_marca_motor; } }
        private string _modelo_motor;
        /// <summary>
        /// Atributo encargado de almacenar el Modelo del Motor
        /// </summary>
        public string modelo_motor { get { return this._modelo_motor; } }
        private string _serie_motor;
        /// <summary>
        /// Atributo encargado de almacenar la Serie del Motor
        /// </summary>
        public string serie_motor { get { return this._serie_motor; } }
        private byte _id_estado_placas;
        /// <summary>
        /// Atributo encargado de almacenar las Placas
        /// </summary>
        public byte id_estado_placas { get { return this._id_estado_placas; } }
        private string _placas;
        /// <summary>
        /// Atributo encargado de almacenar las Placas
        /// </summary>
        public string placas { get { return this._placas; } }
        private decimal _peso_tara;
        /// <summary>
        /// Atributo encargado de almacenar el Peso
        /// </summary>
        public decimal peso_tara { get { return this._peso_tara; } }
        private byte _id_unidad_medida_peso;
        /// <summary>
        /// Atributo encargado de almacenar la Unidad de Medida de Peso
        /// </summary>
        public byte id_unidad_medida_peso { get { return this._id_unidad_medida_peso; } }
        private decimal _kilometraje_asignado;
        /// <summary>
        /// Atributo encargado de almacenar el Kilometraje Asignado
        /// </summary>
        public decimal kilometraje_asignado { get { return this._kilometraje_asignado; } }
        private decimal _capacidad_combustible;
        /// <summary>
        /// Atributo encargado de almacenar la Capacidad del Combustible
        /// </summary>
        public decimal capacidad_combustible { get { return this._capacidad_combustible; } }
        private decimal _combustible_asignado;
        /// <summary>
        /// Atributo encargado de almacenar Combustible Asignado
        /// </summary>
        public decimal combustible_asignado { get { return this._combustible_asignado; } }
        private int _id_operador;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Operador
        /// </summary>
        public int id_operador { get { return this._id_operador; } }
        private string _antena_gps_principal;
        /// <summary>
        /// Atributo encargado de almacenar la Antena GPS Principal
        /// </summary>
        public string antena_gps_principal { get { return this._antena_gps_principal; } }
        private int _id_estancia;
        /// <summary>
        /// Obtiene el Id de la estancia actual (estatus disponible y ocupado)
        /// </summary>
        public int id_estancia { get { return this._id_estancia; } }
        private int _id_movimiento;
        /// <summary>
        /// Obtiene el Id de movimiento actual (estatus tránsito)
        /// </summary>
        public int id_movimiento { get { return this._id_movimiento; } }
        private int _id_configuracion;
        /// <summary>
        /// Obtiene el Id de Configuración de una Llanta
        /// </summary>
        public int id_configuracion { get { return this._id_configuracion; } }
        private DateTime _fecha_actualizacion;
        /// <summary>
        /// Obtiene la fecha de actualización del Id de estancia o Movimiento actuales
        /// </summary>
        public DateTime fecha_actualizacion { get { return this._fecha_actualizacion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        /// <summary>
        /// Enumera el Estatus de la Unidad
        /// </summary>
        public Estatus EstatusUnidad
        {
            get { return (Estatus)id_estatus_unidad; }
        }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Unidad()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public Unidad(int id_registro)
        {   
            //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado Número Económico y la Compania
        /// </summary>
        /// <param name="no_economico">Número Económico</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        public Unidad(string no_economico, int id_compania_emisora)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(no_economico, id_compania_emisora);
        }

        public Unidad(int id_compania_emisora, string tag)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_compania_emisora, tag);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Unidad()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_unidad = 0;
            this._numero_unidad = "";
            this._id_compania_emisor = 0;
            this._id_estatus_unidad = 0;
            this._id_tipo_unidad = 0;
            this._id_sub_tipo_unidad = 0;
            this._ejes = 0;
            this._id_dimension = 0;
            this._bit_no_propia = false;
            this._fecha_adquisicion = DateTime.MinValue;
            this._fecha_baja = DateTime.MinValue;
            this._id_compania_proveedor = 0;
            this._id_marca = 0;
            this._modelo = "";
            this._ano = 0;
            this._serie = "";
            this._id_marca_motor = 0;
            this._modelo_motor = "";
            this._serie_motor = "";
            this._id_estado_placas = 0;
            this._placas = "";
            this._peso_tara = 0;
            this._id_unidad_medida_peso = 0;
            this._kilometraje_asignado = 0;
            this._capacidad_combustible = 0;
            this._combustible_asignado = 0;
            this._id_operador = 0;
            this._antena_gps_principal = "";
            this._id_estancia = 0;
            this._id_movimiento = 0;
            this._fecha_actualizacion = DateTime.MinValue;
            this._id_configuracion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, "", 0, 0, 0, 0, 0, 0, false, null, null, 0, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, null, 0,0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_unidad = id_registro;
                        this._numero_unidad = dr["NumeroUnidad"].ToString();
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_estatus_unidad = Convert.ToByte(dr["IdEstatusUnidad"]);
                        this._id_tipo_unidad = Convert.ToInt32(dr["IdTipoUnidad"]);
                        this._id_sub_tipo_unidad = Convert.ToByte(dr["IdSubTipo"]);
                        this._ejes = Convert.ToByte(dr["Ejes"]);
                        this._id_dimension = Convert.ToInt32(dr["IdDimension"]);
                        this._bit_no_propia = Convert.ToBoolean(dr["BitNoPropia"]);
                        this._fecha_adquisicion = Convert.ToDateTime(dr["FechaAdquisicion"]);
                        DateTime.TryParse(dr["FechaBaja"].ToString(), out this._fecha_baja);
                        this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
                        this._id_marca = Convert.ToByte(dr["IdMarca"]);
                        this._modelo = dr["Modelo"].ToString();
                        this._ano = Convert.ToInt32(dr["Ano"]);
                        this._serie = dr["Serie"].ToString();
                        this._id_marca_motor = Convert.ToByte(dr["IdMarcaMotor"]);
                        this._modelo_motor = dr["ModeloMotor"].ToString();
                        this._serie_motor = dr["SerieMotor"].ToString();
                        this._id_estado_placas = Convert.ToByte(dr["IdEstadoPlacas"]);
                        this._placas = dr["Placas"].ToString();
                        this._peso_tara = Convert.ToDecimal(dr["PesoTara"]);
                        this._id_unidad_medida_peso = Convert.ToByte(dr["IdUnidadMedidaPeso"]);
                        this._kilometraje_asignado = Convert.ToDecimal(dr["KilometrajeAsignado"]);
                        this._capacidad_combustible = Convert.ToDecimal(dr["CapacidadCombustible"]);
                        this._combustible_asignado = Convert.ToDecimal(dr["CombustibleAsignado"]);
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._antena_gps_principal = dr["AntenaGpsPrincipal"].ToString();
                        this._id_estancia = Convert.ToInt32(dr["IdEstancia"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        DateTime.TryParse(dr["FechaActualizacion"].ToString(), out this._fecha_actualizacion);
                        this._id_configuracion = Convert.ToInt32(dr["IdConfiguracion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos dado un No. Económico y una Compania
        /// </summary>
        /// <param name="no_economico">Número Económico</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(string no_economico, int id_compania_emisora)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 10, 0, no_economico, id_compania_emisora, 0, 0, 0, 0, 0, false, null, null, 0, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, null, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_unidad = Convert.ToInt32(dr["Id"]);
                        this._numero_unidad = dr["NumeroUnidad"].ToString();
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_estatus_unidad = Convert.ToByte(dr["IdEstatusUnidad"]);
                        this._id_tipo_unidad = Convert.ToInt32(dr["IdTipoUnidad"]);
                        this._id_sub_tipo_unidad = Convert.ToByte(dr["IdSubTipo"]);
                        this._ejes = Convert.ToByte(dr["Ejes"]);
                        this._id_dimension = Convert.ToInt32(dr["IdDimension"]);
                        this._bit_no_propia = Convert.ToBoolean(dr["BitNoPropia"]);
                        this._fecha_adquisicion = Convert.ToDateTime(dr["FechaAdquisicion"]);
                        DateTime.TryParse(dr["FechaBaja"].ToString(), out this._fecha_baja);
                        this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
                        this._id_marca = Convert.ToByte(dr["IdMarca"]);
                        this._modelo = dr["Modelo"].ToString();
                        this._ano = Convert.ToInt32(dr["Ano"]);
                        this._serie = dr["Serie"].ToString();
                        this._id_marca_motor = Convert.ToByte(dr["IdMarcaMotor"]);
                        this._modelo_motor = dr["ModeloMotor"].ToString();
                        this._serie_motor = dr["SerieMotor"].ToString();
                        this._id_estado_placas = Convert.ToByte(dr["IdEstadoPlacas"]);
                        this._placas = dr["Placas"].ToString();
                        this._peso_tara = Convert.ToDecimal(dr["PesoTara"]);
                        this._id_unidad_medida_peso = Convert.ToByte(dr["IdUnidadMedidaPeso"]);
                        this._kilometraje_asignado = Convert.ToDecimal(dr["KilometrajeAsignado"]);
                        this._capacidad_combustible = Convert.ToDecimal(dr["CapacidadCombustible"]);
                        this._combustible_asignado = Convert.ToDecimal(dr["CombustibleAsignado"]);
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._antena_gps_principal = dr["AntenaGpsPrincipal"].ToString();
                        this._id_estancia = Convert.ToInt32(dr["IdEstancia"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        DateTime.TryParse(dr["FechaActualizacion"].ToString(), out this._fecha_actualizacion);
                        this._id_configuracion = Convert.ToInt32(dr["IdConfiguracion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Tag y una Compania
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="tag">Tag</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_compania_emisora, string tag)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 11, 0, "", id_compania_emisora, 0, 0, 0, 0, 0, false, null, null, 0, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, tag, 0, 0, null, 0, 0, true, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_unidad = Convert.ToInt32(dr["Id"]);
                        this._numero_unidad = dr["NumeroUnidad"].ToString();
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_estatus_unidad = Convert.ToByte(dr["IdEstatusUnidad"]);
                        this._id_tipo_unidad = Convert.ToInt32(dr["IdTipoUnidad"]);
                        this._id_sub_tipo_unidad = Convert.ToByte(dr["IdSubTipo"]);
                        this._ejes = Convert.ToByte(dr["Ejes"]);
                        this._id_dimension = Convert.ToInt32(dr["IdDimension"]);
                        this._bit_no_propia = Convert.ToBoolean(dr["BitNoPropia"]);
                        this._fecha_adquisicion = Convert.ToDateTime(dr["FechaAdquisicion"]);
                        DateTime.TryParse(dr["FechaBaja"].ToString(), out this._fecha_baja);
                        this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
                        this._id_marca = Convert.ToByte(dr["IdMarca"]);
                        this._modelo = dr["Modelo"].ToString();
                        this._ano = Convert.ToInt32(dr["Ano"]);
                        this._serie = dr["Serie"].ToString();
                        this._id_marca_motor = Convert.ToByte(dr["IdMarcaMotor"]);
                        this._modelo_motor = dr["ModeloMotor"].ToString();
                        this._serie_motor = dr["SerieMotor"].ToString();
                        this._id_estado_placas = Convert.ToByte(dr["IdEstadoPlacas"]);
                        this._placas = dr["Placas"].ToString();
                        this._peso_tara = Convert.ToDecimal(dr["PesoTara"]);
                        this._id_unidad_medida_peso = Convert.ToByte(dr["IdUnidadMedidaPeso"]);
                        this._kilometraje_asignado = Convert.ToDecimal(dr["KilometrajeAsignado"]);
                        this._capacidad_combustible = Convert.ToDecimal(dr["CapacidadCombustible"]);
                        this._combustible_asignado = Convert.ToDecimal(dr["CombustibleAsignado"]);
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._antena_gps_principal = dr["AntenaGpsPrincipal"].ToString();
                        this._id_estancia = Convert.ToInt32(dr["IdEstancia"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        DateTime.TryParse(dr["FechaActualizacion"].ToString(), out this._fecha_actualizacion);
                        this._id_configuracion = Convert.ToInt32(dr["IdConfiguracion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="numero_unidad">Número de Unidad</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_estatus_unidad">Estatus de la Unidad</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad</param>
        /// <param name="id_sub_tipo_unidad">Id SubTipo</param>
        /// <param name="ejes">Ejes</param>
        /// <param name="id_dimension">Dimension de la Unidad</param>
        /// <param name="bit_no_propia">Estatus de Propiedad</param>
        /// <param name="fecha_adquisicion">Fecha de adquisición de la unidad</param>
        /// <param name="fecha_baja">Fecha de baja de la unidad</param>
        /// <param name="id_compania_proveedor">compania_proveedor de la Unidad</param>
        /// <param name="id_marca">Marca de la Unidad</param>
        /// <param name="modelo">Modelo de la Unidad</param>
        /// <param name="ano">Ano de la Unidad</param>
        /// <param name="serie">Serie de la Unidad</param>
        /// <param name="id_marca_motor">Marca del Motor</param>
        /// <param name="modelo_motor">Modelo del Motor</param>
        /// <param name="serie_motor">Serie del Motor</param>
        /// <param name="id_estado_placas">Estado Placas</param>
        /// <param name="placas">Placas de la Unidad</param>
        /// <param name="peso_tara">Peso de la Unidad</param>
        /// <param name="id_unidad_medida_peso">Unidad de Medida de Peso</param>
        /// <param name="kilometraje_asignado">Kilometraje Asignado</param>
        /// <param name="capacidad_combustible">Capacidad de Combustible</param>
        /// <param name="combustible_asignado">Combustible Asignado</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="antena_gps_principal">Antena GPS Principal</param>/
        /// <param name="id_estancia">Id de estancia actual (Estatus ocupado y disponible)</param>
        /// <param name="id_movimiento">Id de movimiento actual (estatus tránsito)</param>
        /// <param name="fecha_actualizacion">Fecha de actualización de la estancia o movimiento actuales</param>
        /// <param name="id_configuracion">Configuración de Llanta</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaUnidad(string numero_unidad, int id_compania_emisor, Estatus id_estatus_unidad, int id_tipo_unidad, byte id_sub_tipo_unidad, byte ejes, int id_dimension,
                                    bool bit_no_propia, DateTime fecha_adquisicion, DateTime fecha_baja, int id_compania_proveedor, byte id_marca, string modelo, int ano, string serie,
                                    byte id_marca_motor, string modelo_motor, string serie_motor, byte id_estado_placas, string placas, decimal peso_tara, byte id_unidad_medida_peso,
                                    decimal kilometraje_asignado, decimal capacidad_combustible, decimal combustible_asignado, int id_operador, string antena_gps_principal, int id_estancia,
                                    int id_movimiento, DateTime fecha_actualizacion, int id_configuracion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de parametros
            object[] param = { 2, this._id_unidad, numero_unidad, id_compania_emisor, id_estatus_unidad, id_tipo_unidad, id_sub_tipo_unidad, ejes, id_dimension, bit_no_propia, 
                                 fecha_adquisicion, Fecha.ConvierteDateTimeObjeto(fecha_baja), id_compania_proveedor, id_marca, modelo, ano, serie, id_marca_motor, modelo_motor, serie_motor, 
                                 id_estado_placas, placas, peso_tara, id_unidad_medida_peso, kilometraje_asignado, capacidad_combustible, combustible_asignado, id_operador, antena_gps_principal, 
                                 id_estancia, id_movimiento, Fecha.ConvierteDateTimeObjeto(fecha_actualizacion), id_configuracion, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Determina si la unidad está lista para su baja, eveluando existencia de anticipos, vales y asignaciones pendientes
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaAsignacionesUnidadBaja()
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_unidad);

            //Obteniendo el conjunto de anticipos pendientes
            using (DataTable mitAnticipos = EgresoServicio.DetalleLiquidacion.ObtieneDetallesSinLiquidarUnidad(this._id_unidad))
                //Si hay registros
                if (mitAnticipos != null)
                    resultado = new RetornoOperacion("Existen elementos pendientes por liquidar.");

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Obteniendo asignaciones a movimientos pendientes
                using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.ObtieneAsignacionesPendientesRecurso(MovimientoAsignacionRecurso.Tipo.Unidad, this._id_unidad))
                {
                    //Si hay registros
                    if (mitAsignaciones != null)
                        resultado = new RetornoOperacion("Existen asignaciones inconclusas para esta unidad.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Unidades
        /// </summary>
        /// <param name="numero_unidad">Número de Unidad</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad</param>
        /// <param name="id_sub_tipo_unidad">Id Sub_Tipo</param>
        /// <param name="ejes">Ejes</param>
        /// <param name="id_dimension">Dimension de la Unidad</param>
        /// <param name="bit_no_propia">Estatus de Propiedad</param>
        /// <param name="fecha_adquisicion">Fecha de Adquisición de la unidad</param>
        /// <param name="id_compania_proveedor">compania_proveedor de la Unidad</param>
        /// <param name="id_marca">Marca de la Unidad</param>
        /// <param name="modelo">Modelo de la Unidad</param>
        /// <param name="ano">Ano de la Unidad</param>
        /// <param name="serie">Serie de la Unidad</param>
        /// <param name="id_marca_motor">Marca del Motor</param>
        /// <param name="modelo_motor">Modelo del Motor</param>
        /// <param name="serie_motor">Serie del Motor</param>
        /// <param name="id_estado_placas">Estado Placas</param>
        /// <param name="placas">Placas de la Unidad</param>
        /// <param name="peso_tara">Peso de la Unidad</param>
        /// <param name="id_unidad_medida_peso">Unidad de Medida de Peso</param>
        /// <param name="km_inicial">Kilometros iniciales de la unidad (mismo que será incrementando con cada movimiento terminado)</param>
        /// <param name="capacidad_combustible">Capacidad de Combustible</param>
        /// <param name="antena_gps_principal">Antena GPS Principal</param>
        /// <param name="id_configuracion">Configuración de Llanta</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUnidad(string numero_unidad, int id_compania_emisor, int id_tipo_unidad, byte id_sub_tipo_unidad, byte ejes, int id_dimension, bool bit_no_propia,
                                            DateTime fecha_adquisicion, int id_compania_proveedor, byte id_marca, string modelo, int ano, string serie, byte id_marca_motor, string modelo_motor,
                                            string serie_motor, byte id_estado_placas, string placas, decimal peso_tara, byte id_unidad_medida_peso, decimal km_inicial, 
                                            decimal capacidad_combustible, string antena_gps_principal, int id_configuracion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de parametros
            object[] param = { 1, 0, numero_unidad, id_compania_emisor, Estatus.ParadaDisponible, id_tipo_unidad, id_sub_tipo_unidad, ejes, id_dimension, bit_no_propia, 
                                 fecha_adquisicion, null, id_compania_proveedor, id_marca, modelo, ano, serie, id_marca_motor, modelo_motor, serie_motor,  
                                 id_estado_placas, placas, peso_tara, id_unidad_medida_peso, km_inicial, capacidad_combustible, 0, 0, antena_gps_principal, 
                                 0, 0, fecha_adquisicion, id_configuracion, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los datos generales de la unidad
        /// </summary>
        /// <param name="numero_unidad">Número de Unidad</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad</param>
        /// <param name="id_sub_tipo_unidad">Id Sub Tipo</param>
        /// <param name="ejes">Ejes</param>
        /// <param name="id_dimension">Dimension de la Unidad</param>
        /// <param name="bit_no_propia">Estatus de Propiedad</param>
        ///<param name="fecha_adquisicion">Fecha de Adquisición de la Unidad</param>
        /// <param name="id_compania_proveedor">compania_proveedor de la Unidad</param>
        /// <param name="id_marca">Marca de la Unidad</param>
        /// <param name="modelo">Modelo de la Unidad</param>
        /// <param name="ano">Ano de la Unidad</param>
        /// <param name="serie">Serie de la Unidad</param>
        /// <param name="id_marca_motor">Marca del Motor</param>
        /// <param name="modelo_motor">Modelo del Motor</param>
        /// <param name="serie_motor">Serie del Motor</param>
        /// <param name="id_estado_placas">Id Estado Placas</param>
        /// <param name="placas">Placas de la Unidad</param>
        /// <param name="peso_tara">Peso de la Unidad</param>
        /// <param name="id_unidad_medida_peso">Unidad de Medida de Peso</param>
        /// <param name="capacidad_combustible">Capacidad de Combustible</param>
        /// <param name="antena_gps_principal">Antena GPS Principal</param>
        /// <param name="id_configuracion">Configuración de Llanta</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaUnidad(string numero_unidad, int id_compania_emisor, int id_tipo_unidad, byte id_sub_tipo_unidad, byte ejes, int id_dimension, bool bit_no_propia,
                                    DateTime fecha_adquisicion, int id_compania_proveedor, byte id_marca, string modelo, int ano, string serie, byte id_marca_motor,
                                    string modelo_motor, string serie_motor, byte id_estado_placas, string placas, decimal peso_tara, byte id_unidad_medida_peso,
                                    decimal capacidad_combustible, string antena_gps_principal, int id_configuracion, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_unidad);

            //Si hay fecha de baja especificada, esta debe ser mayor a la fecha de ingreso
            if (this._fecha_baja.CompareTo(DateTime.MinValue) > 0)
                if (this._fecha_baja.CompareTo(this._fecha_adquisicion) < 0)
                    resultado = new RetornoOperacion("La fecha de adquisición no puede ser mayor a fecha de baja de la unidad.");

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Realizando actualización de registro
                resultado = this.editaUnidad(numero_unidad, id_compania_emisor, this.EstatusUnidad, id_tipo_unidad, id_sub_tipo_unidad, ejes, id_dimension, bit_no_propia,
                                fecha_adquisicion, this._fecha_baja, id_compania_proveedor, id_marca, modelo, ano, serie, id_marca_motor, modelo_motor, serie_motor,
                                id_estado_placas, placas, peso_tara, id_unidad_medida_peso, this._kilometraje_asignado, capacidad_combustible, this._combustible_asignado,
                                this._id_operador, antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion,
                                id_usuario, this._habilitar);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Actualizamos Estatus de la Unidad
        /// </summary>
        /// <param name="estatus">Estatus de la Unidad</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusUnidad(Estatus estatus, int id_usuario)
        {

            //Actualiza Estatus
            return this.editaUnidad(this._numero_unidad, this._id_compania_emisor, estatus, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                                    this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor,
                                    this._modelo_motor, this._serie_motor, this._id_estado_placas, this._placas, this._peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado,
                                    this._capacidad_combustible, this._combustible_asignado, this._id_operador, this._antena_gps_principal, this._id_estancia, this._id_movimiento,
                                    this._fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar la unidad
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        [Obsolete("El método no debe ser utilizado, en su lugar utilizar 'ActualizaEstatusABaja' para exclusión de unidades.", true)]
        public RetornoOperacion DeshabilitaUnidad(int id_usuario)
        {
            //Invocando Método de Actualización
            return this.editaUnidad(this._numero_unidad, this._id_compania_emisor, this.EstatusUnidad, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                            this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                            this._id_estado_placas, this._placas, peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado, this._capacidad_combustible, this._combustible_asignado, this._id_operador,
                            this._antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar las Unidades
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributosInstancia()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_unidad);
        }

        /// <summary>
        /// Carga de Unidades para Asignación de Recurso
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="no_unidad">Número de Unidad</param>
        /// <param name="id_tipo_unidad">Id Tipo Unidad</param>
        /// <param name="id_estatus_unidad">Estatus</param>
        /// <param name="id_compania_proveedor">Compania Proveedor</param>
        /// <param name="bit_no_propia">Bit No Propia</param>
        /// <param name="id_ubicacion">Ubicación de la Unidad</param>
        /// <returns></returns>
        public static DataTable CargaUnidadesParaAsignacionRecurso(int id_compania_emisor, string no_unidad, byte id_tipo_unidad, byte id_estatus_unidad, int id_compania_proveedor,
                                                                  bool bit_no_propia, int id_ubicacion)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Declarando arreglo de parámetros para consulta en BD
            //Armando Objeto de Parametros
            object[] param = { 4, 0, no_unidad, id_compania_emisor, id_estatus_unidad, id_tipo_unidad, 0, 0, 0, bit_no_propia, null, null, id_compania_proveedor, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, "", id_ubicacion, 0, null, 0, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Actualizamos Estatus de la Unidades
        /// </summary>
        /// <param name="mitUnidades">Tabla de Unidades</param>
        /// <param name="estatus">Estatus  de la Unidad</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaEstatusUnidades(DataTable mitUnidades, Estatus estatus, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos origen de datos
            if (Validacion.ValidaOrigenDatos(mitUnidades))
            {
                //Por cada una de la Unidad
                foreach (DataRow r in mitUnidades.Rows)
                {
                    //Instanciamos Unidades
                    using (Unidad objUnidad = new Unidad(r.Field<int>("Id")))
                    {
                        //Validamos que el estatus se diferente al que se desea actualizar
                        if ((Estatus)objUnidad._id_estatus_unidad != estatus)
                            //Actualiza estatus de la Unidad
                            resultado = objUnidad.ActualizaEstatusUnidad(estatus, id_usuario);
                    }

                    //Validamos Resultado
                    if (!resultado.OperacionExitosa)
                        //Salimos del Ciclo
                        break;
                }
            }
            else
            {
                resultado = new RetornoOperacion("No existe asignación de Recursos para actualización de Estatus de las Unidades");
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza el estatus de la Unidad a Parada Ocupado
        /// </summary>
        /// <param name="id_parada_a_mover">Id Parada para la nueva asignación de la unidad</param>
        /// <param name="id_estancia_actual">Estancia actual</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusAParadaOcupado(int id_parada_a_mover, int id_estancia_actual, int id_usuario)
        {
            //Establecemos Variable 
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Instanciamos Movimiento la cual se encuentra la Unidad
            using (Movimiento objMovimiento = new Movimiento(MovimientoAsignacionRecurso.ObtieneMovimientoDeAsignacionIniciada(MovimientoAsignacionRecurso.Tipo.Unidad, this._id_unidad)))
            {
                //Instanciamos Estancia actual 
                using (EstanciaUnidad objEstancia = new EstanciaUnidad(id_estancia_actual))
                {
                    //Validamos Estatus
                    if ((Estatus)this._id_estatus_unidad == Estatus.ParadaDisponible || (Estatus)this._id_estatus_unidad == Estatus.ParadaOcupado)
                    {
                        //Instanciamos Parada
                        using (Parada objParadaEstancia = new Parada(objEstancia.id_parada), objParadaAMover = new Parada(id_parada_a_mover))
                        {
                            //Validamos Ubicación
                            if (objParadaEstancia.id_ubicacion == objParadaAMover.id_ubicacion)
                            {
                                //Validamos Servicio
                                if (objMovimiento.id_servicio == objParadaAMover.id_servicio || objMovimiento.id_servicio == 0)
                                    //Actualizamos Estatus
                                    resultado = ActualizaEstatusUnidad(Estatus.ParadaOcupado, id_usuario);
                                else
                                {
                                    //Instanciamos Servicio
                                    using (Documentacion.Servicio objServicio = new Documentacion.Servicio(objMovimiento.id_servicio))
                                    {
                                        //Establecemos Mensaje Error 
                                        resultado = new RetornoOperacion("La Unidad " + this._numero_unidad + " se encuentrá asignada al servicio " + objServicio.no_servicio + ".");
                                    }
                                }
                            }
                            else
                                //Establecemos Mensaje Error 
                                resultado = new RetornoOperacion("La Unidad " + this._numero_unidad + " no está disponible ya que se encuentrá en la ubicación " + objParadaEstancia.descripcion + ".");
                        }
                    }
                    else
                        //Establcemos Mensaje
                        resultado = new RetornoOperacion("La Unidad " + this._numero_unidad + " no se encuentra disponible.");
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza el estatus de la Unidad a Disponible
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusADisponible(int id_usuario)
        {
            //Establecemos Variable 
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Estatus
            if ((Estatus)this._id_estatus_unidad == Estatus.ParadaOcupado || (Estatus)this._id_estatus_unidad == Estatus.Baja)
                //Actualiza Estatus
                resultado = ActualizaEstatusUnidad(Estatus.ParadaDisponible, id_usuario);
            else
                //Establcemos Mensaje
                resultado = new RetornoOperacion("El estatus de la Unidad " + this._numero_unidad + " no permite su edición");
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la baja del operador
        /// </summary>
        /// <param name="fecha_baja">Fecha de Baja del operador</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusABaja(DateTime fecha_baja, int id_usuario)
        {
            //Estable Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos estatus actual de unidad
            if (this.EstatusUnidad == Estatus.ParadaDisponible)
            {
                //inicializando transacción
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validar pendientes (anticipos, liquidación, fecha de ultimo servicio, ultimo pago)
                    resultado = validaAsignacionesUnidadBaja();

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Cargando liquidaciones depositadas
                        using (DataTable mitLiquidaciones = Liquidacion.Liquidacion.ObtieneLiquidacionesEntidad(this._id_compania_emisor, Liquidacion.Liquidacion.TipoAsignacion.Unidad, this._id_unidad))
                        {
                            //Si hay registros
                            if (Validacion.ValidaOrigenDatos(mitLiquidaciones))
                            {
                                //Validando fecha de baja contra fecha de última liquidación
                                DateTime ultimaLiquidacion = (from DataRow r in mitLiquidaciones.Rows
                                                              where r.Field<DateTime>("FechaLiquidacion").CompareTo(fecha_baja) > 0
                                                              orderby r.Field<DateTime>("FechaLiquidacion") descending
                                                              select r.Field<DateTime>("FechaLiquidacion")).FirstOrDefault();
                                //Si hay liquidaciones 
                                if (ultimaLiquidacion.CompareTo(DateTime.MinValue) > 0)
                                    resultado = new RetornoOperacion(string.Format("La fecha de baja no puede ser menor a la fecha de la última liquidación '{0:dd/MM/yyyy HH:mm}'.", ultimaLiquidacion));
                            }
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //Validando fecha de baja contra fecha de adquisición
                            if (this._fecha_adquisicion.CompareTo(fecha_baja) <= 0)
                            {
                                //Instanciando tipo unidad
                                using (UnidadTipo tipo_unidad = new UnidadTipo(this._id_tipo_unidad))
                                {
                                    //Si la unidad es motriz
                                    if (tipo_unidad.bit_motriz)
                                        //Finalizando asignación de operador actual
                                        resultado = ReemplazaOperadorAsignado(0, fecha_baja, id_usuario);
                                }

                                //Si se actualizó correctamente la asignación de operador
                                if (resultado.OperacionExitosa)
                                    //Actualizando estatus de unidad a baja
                                    resultado = editaUnidad(this._numero_unidad, this._id_compania_emisor, Estatus.Baja, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                                        this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                                        this._id_estado_placas, this._placas, peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado, this._capacidad_combustible, this._combustible_asignado, this._id_operador,
                                        this._antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
                            }
                            //Si la baja es previa a la adquisición
                            else
                                resultado = new RetornoOperacion(string.Format("La fecha de baja '{0:dd/MM/yyyy}' debe ser posterior a la fecha de adquisición '{1:dd/MM/yyyy}' de la unidad.", fecha_baja, this._fecha_adquisicion));
                        }
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Finalizando transacción
                        scope.Complete();
                }
            }
            else
                resultado = new RetornoOperacion("El estatus actual de la unidad no permite la baja de la misma.");
            
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Carga Unidades para asignación de recurso en despacho
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="no_unidad">Id Unidad</param>
        ///<param name="id_ubicacion_actual">En caso de cargar unidades en la misma ubicacion asignamos Id Ubicacion y asignamos id ubicacion diferente = 0</param>
        ///<param name="id_ubicacion_diferente">En caso de cargar unidades en diferente ubicación asignamos Id ubicación diferente y asignamos id ubicacion = 0</param>
        /// <returns></returns>
        public static DataTable CargaUnidadesParaAsignacionEnDespacho(int id_compania_emisor, string no_unidad, int id_ubicacion_actual, int id_ubicacion_deferente)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 5, 0, no_unidad, id_compania_emisor, 0, 0, 0, 0, 0, false, null, null, 0, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, null,0, 0, false, id_ubicacion_actual, id_ubicacion_deferente };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Validamos que la Unidad llego al final del Servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidaUnidadParaTerminoServicio()
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que el estatus se encuentre Ocupado

            if (this.EstatusUnidad == Estatus.ParadaOcupado)
            {
                //Validamos que no exista alguna asignación Iniciada
                if (MovimientoAsignacionRecurso.ObtieneMovimientoAsignacionIniciada(MovimientoAsignacionRecurso.Tipo.Unidad, this._id_unidad) == 0)
                {
                    //Establecemos Resultado
                    resultado = new RetornoOperacion(0);
                }
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de liberar la Unidad  y el operador asignado
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion LiberarUnidad(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos el Estatus de la Unidad se encuntre Ocupado
            if ((Estatus)this._id_estatus_unidad == Estatus.ParadaOcupado)
            {
                //Instanciamos Tipo de Unidad
                using (UnidadTipo objUnidadTipo = new UnidadTipo(this._id_tipo_unidad))
                {
                    //Validamos que exista el Tipo de Unidad
                    if (objUnidadTipo.id_tipo_unidad > 0)
                    {
                        //Validamos que el Tipo de Unidad sea Motriz y permita Arrastre
                        if (objUnidadTipo.bit_motriz == true && objUnidadTipo.bit_permite_arrastre == true)
                        {
                            //Creamos la transacción 
                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Declaramos Variable para obtener la estancia actual de la unidad
                                int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(this._id_unidad);

                                //Instanciamos Estancia de la Unidad
                                using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(id_estancia))
                                {
                                    //Validamos Estancia
                                    if (objEstanciaUnidad.id_estancia_unidad > 0)
                                    {
                                        //Declaramos variable para la parada comodin
                                        int idParadaNuevo = 0;
                                        //Instanciamos la parada de la Estancia
                                        using (Parada objParada = new Parada(objEstanciaUnidad.id_parada))
                                        {
                                            //Validamos Ultima Parada
                                            if (objParada.id_parada > 0)
                                            {
                                                //Verificando existencia de parada alterna en la ubicación actual
                                                idParadaNuevo = Parada.ObtieneParadaComodinUbicacion(objParada.id_ubicacion, true, id_usuario);

                                                //Validamos que exista Parada Comodin
                                                if (idParadaNuevo != 0)
                                                {
                                                    //Editamos Estancia de la Unidad
                                                    resultado = objEstanciaUnidad.CambiaParadaEstanciaUnidad(idParadaNuevo, id_usuario);

                                                    //Validamos Resultado
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Actualizamos Estatus de la Unidad
                                                        resultado = this.ActualizaEstatusADisponible(id_usuario);

                                                        //Validamos Resultado
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Refrescamos Atributos
                                                            if (this.ActualizaAtributosInstancia())
                                                            {
                                                                //Actualizamos Atributos principales de la Unidad
                                                                resultado = this.ActualizaEstanciaYMovimiento(objEstanciaUnidad.id_estancia_unidad, 0, objParada.fecha_llegada, id_usuario);

                                                                //Validamos Resultado
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Obtenemos Asignación Iniciada de la Unidad
                                                                    int IdAsignacionUnidad = MovimientoAsignacionRecurso.ObtieneMovimientoAsignacionIniciada(MovimientoAsignacionRecurso.Tipo.Unidad, this._id_unidad);

                                                                    //Validamos Existencia de Asignación
                                                                    if (IdAsignacionUnidad != 0)
                                                                    {
                                                                        //Instanciamos Asignación
                                                                        using (MovimientoAsignacionRecurso objAsignacionUnidad = new MovimientoAsignacionRecurso(IdAsignacionUnidad))
                                                                        {
                                                                            //Cancelamos Asignación
                                                                            resultado = objAsignacionUnidad.CancelaMovimientoAsignacionRecurso(id_usuario);
                                                                        }
                                                                    }

                                                                    //Si existe Operador Asignado a la Unidad
                                                                    if (this._id_operador != 0)
                                                                    {
                                                                        //Validamos Resultado de Cancelación de Asignación e Caso de Existir
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Instanciamos Operador
                                                                            using (Operador objOperador = new Operador(this._id_operador))
                                                                            {
                                                                                //Actualizamos Estatus
                                                                                resultado = objOperador.ActualizaEstatusADisponible(id_usuario);

                                                                                //Validamos Resultado 
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Refrecamos Atributos del Operador
                                                                                    if (objOperador.ActualizaAtributosInstancia())
                                                                                    {
                                                                                        //Actualizamos Operador
                                                                                        resultado = objOperador.ActualizaParadaYMovimiento(idParadaNuevo, 0, objParada.fecha_llegada, id_usuario);

                                                                                        //Validamos Resultado
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //Obtenemos Asignación Iniciada del Operador
                                                                                            int IdAsignacionOperador = MovimientoAsignacionRecurso.ObtieneMovimientoAsignacionIniciada(MovimientoAsignacionRecurso.Tipo.Operador, this._id_operador);

                                                                                            //Validamos Existencia de Asignación
                                                                                            if (IdAsignacionOperador != 0)
                                                                                            {
                                                                                                //Instanciamos Asignación del Operador
                                                                                                using (MovimientoAsignacionRecurso objAsignacionOperador = new MovimientoAsignacionRecurso(IdAsignacionOperador))
                                                                                                {
                                                                                                    //Validamos Viaje Activo
                                                                                                    //bool viajeActivo = MovimientoAsignacionRecurso.ValidaViajeActivo(objAsignacionOperador.id_tipo_asignacion, objAsignacionOperador.id_recurso_asignado, objAsignacionOperador.id_movimiento);                                                                                              
                                                                                                    //Cancelamos Asignación
                                                                                                    resultado = objAsignacionOperador.CancelaMovimientoAsignacionRecurso(id_usuario);
                                                                                                    ////Validamos Resultado
                                                                                                    //if(resultado.OperacionExitosa)
                                                                                                    //{
                                                                                                    //  //Realizando envio de notificación al recurso asignado
                                                                                                    //Global.NotificacionPush.Instance.EliminaAsignacionServicio(objAsignacionOperador.id_movimiento_asignacion_recurso, viajeActivo);
                                                                                                    //}
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Establecemos Mensaje Error
                                                                                        resultado = new RetornoOperacion("No se encontró datos complementarios del Operador " + objOperador.nombre + ".");
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Mostramos Error
                                                                resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad " + this._numero_unidad + ".");
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Establecemos Mensaje Resultado
                                                    resultado = new RetornoOperacion("No se ecnontró datos complementarios de la Parada Comodin");

                                            }
                                            else
                                                //Establecemos Mensaje Resultado
                                                resultado = new RetornoOperacion("No se encontró datos complementarios de la parada.");
                                        }
                                    }
                                    else
                                    //Establecemos Mesnaje Resultado;
                                    resultado = new RetornoOperacion("Error al Obtener datos complementarios de la Estancia de la Unidad");
                                }
                                //Validamos Resultado
                                if(resultado.OperacionExitosa)
                                {
                                    //Finalizamos transacción
                                    scope.Complete();
                                }

                            }
                        }
                        else
                            //Establecemos Mensaje Resultado
                            resultado = new RetornoOperacion("Sólo es posible la liberación de Unidades Motrices que aceptan Unidades de Arrastre.");
                    }
                    else
                        //Establecemos Mensaje Resultado
                        resultado = new RetornoOperacion("No se encontró datos complementarios del Tipo de Unidad");
                }
            }
            else
                //Establecemos mensaje Resultado
                resultado = new RetornoOperacion("El estatus de la Unidad no permite su liberación");

            //Devolvemos Resultado
            return resultado;
        }


        /// <summary>
        /// Metodo encargado de cargar indicadores ligados con las unidades de acuerdo a la compañia de interes
        /// </summary>
        /// <param name="id_compania_emisor"></param>
        /// <returns></returns>
        public static DataTable CargaIndicadoresUnidades(int id_compania_emisor)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 6, 0, "", id_compania_emisor, 0, 0, 0, 0, 0, false, null, null, 0, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, null, 0, 0, false, 0, 0 };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Realiza la actualización de la estancia o movimiento actuales de la unidad, asi como la fecha de actualización de los mismos.
        /// </summary>
        /// <param name="id_estancia">Id de Estancia actual (Estatus Disponible y Ocupado)</param>
        /// <param name="id_movimiento">Id de Movimiento actual (Estatus Tránsito)</param>
        /// <param name="fecha_actualizacion">Fecha de actualización de estancia o movimiento</param>
        /// <param name="id_usuario">Id de Usuario que realiza la actualización</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstanciaYMovimiento(int id_estancia, int id_movimiento, DateTime fecha_actualizacion, int id_usuario)
        {
            //Realizando actualización
            return editaUnidad(this._numero_unidad, this._id_compania_emisor, this.EstatusUnidad, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                this._id_estado_placas, this._placas, this._peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado, this._capacidad_combustible, this._combustible_asignado, this._id_operador,
                this._antena_gps_principal, id_estancia, id_movimiento, fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Realiza la actualización del operador asignado actualmente a la unidad
        /// </summary>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="fecha_asignacion">Fecha de Asignación de unidades</param>
        /// <param name="id_usuario">Id de Usuario que realiza la actualización</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaOperadorAsignado(int id_operador, DateTime fecha_asignacion, int id_usuario)
        {
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_unidad);

            //Validando que el estatus de la unidad permita su actualización
            if (this.EstatusUnidad != Estatus.Baja)
            {
                //Si el id de operador es mayor a 0
                if (id_operador > 0)
                {
                    //Instanciando al operador
                    using (Operador o = new Operador(id_operador))
                    {
                        //Si el operador se encontró
                        if (o.habilitar)
                        {
                            //Instanciando estancia actual de la unidad
                            using (EstanciaUnidad e = new EstanciaUnidad(this._id_estancia))
                            {
                                //Si la estancia se encontró
                                if (e.habilitar)
                                    //Actualizando Información de Parada y Movimiento sobre el operador
                                    resultado = o.ActualizaParadaYMovimiento(e.id_parada, 0, fecha_asignacion, id_usuario);
                                else
                                    resultado = new RetornoOperacion("La Estancia actual de la unidad no pudo ser localizada.");
                            }
                        }
                        else
                            resultado = new RetornoOperacion("Datos del operador no recuperados.");
                    }
                }
                //Si no hay error de actualización en operador
                if (resultado.OperacionExitosa)
                    //Realizando actualización
                    resultado = editaUnidad(this._numero_unidad, this._id_compania_emisor, this.EstatusUnidad, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                        this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                        this._id_estado_placas, this._placas, this._peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado, this._capacidad_combustible, this._combustible_asignado, id_operador,
                        this._antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
            }
            //De lo contrario, se señala el error
            else
                resultado = new RetornoOperacion("La unidad se encuentra dada de baja, no es posible asignar un operador.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la sustitución del operador actual asignado a la unidad, reemplazandolo por el solicitado. Termina asignaciones previas de unidad y nuevo operador en caso de ser requerido 
        /// </summary>
        /// <param name="id_nvo_operador">Id de Nuevo Operador</param>
        /// <param name="fecha_inicial">Fecha de Inicio de nueva asignación de operador (Fecha de Fin de las asignaciones anteriores)</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ReemplazaOperadorAsignado(int id_nvo_operador, DateTime fecha_inicial, int id_usuario)
        { 
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(this._id_unidad);

            //Inicializando transacción
            using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que la unidad actual sea una unidad motriz
                using (UnidadTipo tipo = new UnidadTipo(this._id_tipo_unidad))
                {
                    if (tipo.bit_motriz)
                    {
                        //Obteniendo asignación actual de la unidad
                        using (AsignacionOperadorUnidad a = AsignacionOperadorUnidad.ObtieneAsignacionActiva(AsignacionOperadorUnidad.TipoBusquedaAsignacion.Unidad, this._id_unidad))
                        {
                            //Si hay asignación activa
                            if (a.habilitar)
                                //Terminar asignación
                                resultado = a.TerminaAsignacionOperadorUnidad(fecha_inicial, id_usuario);
                        }

                        //Si se ha solicitado que se asigne un nuevo operador (ID > 0)
                        if (resultado.OperacionExitosa && id_nvo_operador > 0)
                        {
                            //Obteniendo última asignación del nuevo operador
                            using (AsignacionOperadorUnidad a = AsignacionOperadorUnidad.ObtieneAsignacionActiva(AsignacionOperadorUnidad.TipoBusquedaAsignacion.Operador, id_nvo_operador))
                            {
                                //Si hay asignación activa
                                if (a.habilitar)
                                    //Terminar asignación
                                    resultado = a.TerminaAsignacionOperadorUnidad(fecha_inicial, id_usuario);
                            }

                            //Si no hay errores hasta este punto
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciando operador por asignar
                                using (Operador operador = new Operador(id_nvo_operador))
                                {
                                    //Validando coherencia entre fechas de alta de unidad, alta de operador y fecha de asignación 
                                    if (operador.fecha_ingreso.CompareTo(fecha_inicial) <= 0 && this.fecha_adquisicion.CompareTo(fecha_inicial) <= 0)
                                        //Insertando nueva asignación
                                        resultado = AsignacionOperadorUnidad.InsertaAsignacionOperadorAUnidad(id_nvo_operador, this._id_unidad, fecha_inicial, id_usuario);
                                    else
                                        resultado = new RetornoOperacion("La fecha de asignación de la unidad y el operador debe ser mayor o igual a la fecha de ingreso de ambos.");
                                }
                            }
                        }
                    }
                    else
                        resultado = new RetornoOperacion(string.Format("La unidad '{0}' no permite la asignación de un operador debido a su tipo de unidad.", this.numero_unidad));
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    resultado = new RetornoOperacion(this._id_unidad);
                    //Confirmando cambios realizados
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización de la cantidad de kilometros que se han asignado a la unidad
        /// </summary>
        /// <param name="kilometros_asignados">Cantidad de Kilometros asignados que se incrementarán al total actual</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaKilometrosAsignados(decimal kilometros_asignados, int id_usuario)
        {
            //Realizando actualización
            return editaUnidad(this._numero_unidad, this._id_compania_emisor, this.EstatusUnidad, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                this._id_estado_placas, this._placas, this._peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado + kilometros_asignados, this._capacidad_combustible, this._combustible_asignado, this._id_operador,
                this._antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la actualización de la cantidad de kilometros que se han asignado a la unidad
        /// </summary>
        /// <param name="kilometraje_odometro">Cantidad de Kilometros asignados que se incrementarán al total actual</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaOdometroUnidad(decimal kilometraje_odometro, int id_usuario)
        {
            //Realizando actualización
            return editaUnidad(this._numero_unidad, this._id_compania_emisor, this.EstatusUnidad, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                this._id_estado_placas, this._placas, this._peso_tara, this._id_unidad_medida_peso, kilometraje_odometro, this._capacidad_combustible, this._combustible_asignado, this._id_operador,
                this._antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la actualización de la cantidad de kilometros que se han asignado a la unidad
        /// </summary>
        /// <param name="combustible_asignado">Litros de combustible asignado que se incrementarán al total actual</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCombustibleAsignado(decimal combustible_asignado, int id_usuario)
        {
            //Realizando actualización
            return editaUnidad(this._numero_unidad, this._id_compania_emisor, this.EstatusUnidad, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                this._id_estado_placas, this._placas, this._peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado, this._capacidad_combustible, this._combustible_asignado + combustible_asignado, this._id_operador,
                this._antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Obtiene el conjunto de registros unidad que coincida con los filtros solicitados
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía</param>
        /// <param name="numero_unidad">Número económico de la unidad</param>
        /// <param name="id_estatus_unidad">Id de Estatus de Unidad</param>
        /// <param name="id_tipo_unidad">Id de Tipo de Unidad</param>
        /// <param name="id_compania_proveedor">Id de Compañía Proveedor</param>
        /// <param name="bit_no_propia">Bit No Propio</param>
        /// <param name="id_ubicacion_actual">Id de Ubicación actual de la unidad</param>
        /// <returns></returns>
        public static DataTable CargaReporteUnidades(int id_compania_emisor, string numero_unidad, byte id_estatus_unidad, int id_tipo_unidad, int id_compania_proveedor, bool bit_no_propia, int id_ubicacion_actual)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Declarando arreglo de parámetros para consulta en BD
            //Armando Objeto de Parametros
            object[] param = { 7, 0, numero_unidad, id_compania_emisor, id_estatus_unidad, id_tipo_unidad, 0, 0, 0, bit_no_propia, null, null, id_compania_proveedor, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, null, 0 , 0, false, id_ubicacion_actual, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de Obtener la Unidad actual asignada a un Operador
        /// </summary>
        /// <param name="id_operador">Operador</param>
        /// <returns></returns>
        public static Unidad ObtieneUnidadOperador(int id_operador)
        {
            //Declarando Objeto de Retorno
            Unidad unidad = new Unidad();

            //Armando Objeto de Parametros
            object[] param = { 8, 0, "", 0, 0, 0, 0, 0, 0, false, null, null, 0, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, id_operador, "", 0, 0, null, 0, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Obtenemos la Asignación
                    unidad = new Unidad((from DataRow r in ds.Tables[0].Rows
                                         select Convert.ToInt32(r["IdUnidad"])).FirstOrDefault());
            }

            //Devolviendo resultado Obtenido
            return unidad;
        }

        /// <summary>
        /// Obtiene el Kms Total de una Unidad a partir de una Fecha
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="fecha_inicial">Fecha Inicial de Carga</param>
        /// <param name="fecha_ultima">Fecha Ultima Carga</param>
        /// <returns></returns>
        public static decimal ObtieneKmsTotalesUltimacarga(int id_unidad, DateTime fecha_inicial, DateTime fecha_ultima)
        {
            //Declaramos variable fecha de última parada
            decimal kmsTotales = 0;

            //Armando Objeto de Parametros
            object[] param = { 9, id_unidad, "", 0, 0, 0, 0, 0, 0, false, null, null, 0, 0, "", 0, "", 0, "", "", 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, null, 0, 0, false,
                               fecha_inicial == DateTime.MinValue ? "":fecha_inicial.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                               fecha_ultima == DateTime.MinValue ? "":fecha_ultima.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) };
          

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    kmsTotales = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToDecimal(r["KmsTotal"])).DefaultIfEmpty().FirstOrDefault();

                }
            }

            //Obtenemos Resultado
            return kmsTotales;
        }
        /// <summary>
        /// Método encargado de Evaluar la Unidad por su Posicionamiento contra el Sistema
        /// </summary>
        /// <param name="id_proveedor_ws_unidad">Antena Proveedor de Servicios GPS</param>
        /// <param name="id_servicio">Servicio Actual de la Unidad</param>
        /// <param name="id_parada">Parada Actual de la Unidad</param>
        /// <param name="id_movimiento">Movimiento Actual de la Unidad</param>
        /// <param name="id_parada_destino">Parada de Destino</param>
        /// <param name="ubicacion_gps_unidad"></param>
        /// <param name="id_evaluacion"></param>
        /// <param name="id_usuario">Usuario que Actualiza los Registros</param>
        /// <returns></returns>
        public RetornoOperacion EvaluaUnidadGPS(int id_proveedor_ws_unidad, int id_servicio, int id_parada, int id_movimiento, int id_parada_destino, 
                                                out SqlGeography ubicacion_gps_unidad, out int id_evaluacion,
                                                int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora resultado = Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok;
            DateTime fecha_gps;
            string ubicacion_desc = "";
            double latitud = 0.00, longitud = 0.00;
            decimal velocidad = 0.00M, cant_combustible = 0.00M;
            bool bit_encendido = false;
            int distancia = 0, duracion = 0, id_bitacora = 0;
            int id_destino = id_parada_destino;
            int tiempo_excedido = 0;
            id_evaluacion = 0;
                        
            //Inicializando Variables de Salida
            ubicacion_gps_unidad = SqlGeography.Null;

            //Fecha de Petición
            DateTime fecha_peticion = Fecha.ObtieneFechaEstandarMexicoCentro();

            //Obteniendo Proveedor GPS
            using (SAT_CL.Monitoreo.ProveedorWSUnidad pro_uni = new Monitoreo.ProveedorWSUnidad(id_proveedor_ws_unidad))
            {
                //Validando que exista
                if (pro_uni.habilitar)
                {
                    //Obtiene Posición Actual
                    result = SAT_CL.Monitoreo.ProveedorWSUnidad.ObtienePosicionActualUnidad(pro_uni.id_proveedor_ws, pro_uni.id_proveedor_ws_unidad, 
                                                    out ubicacion_desc, out latitud, out longitud, out fecha_gps, out velocidad, 
                                                    out bit_encendido, out cant_combustible);
                    //Posición Unidad Sistema
                    ubicacion_gps_unidad = SqlGeography.Point(latitud, longitud, 4326);
                    //Validando que haya una Respuesta
                    if (result.OperacionExitosa)
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Insertando Bitacora de Monitoreo
                            result = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.AntenaGPS,
                                        6, id_servicio, id_parada, SAT_CL.Despacho.ParadaEvento.ObtienerPrimerEvento(id_parada), id_movimiento, 19, this._id_unidad,
                                        ubicacion_gps_unidad, ubicacion_desc, "Petición de Ubicación GPS", fecha_gps, velocidad, bit_encendido,
                                        id_usuario);

                            //Validando que haya una Respuesta
                            if (result.OperacionExitosa)
                            {
                                //Guardando Bitacora
                                id_bitacora = result.IdRegistro;

                                //Tiempo Tolerancia
                                int tolerancia = bit_encendido ? pro_uni.tiempo_encendido : pro_uni.tiempo_apagado;

                                //Calculando Hora Tolerada
                                DateTime fecha_gps_tolerada = fecha_gps.AddMinutes(tolerancia);

                                //Validando Comparación
                                switch (fecha_gps_tolerada.CompareTo(fecha_peticion))
                                {
                                    //Si es Mayor
                                    case 1:
                                    //Si es Igual
                                    case 0:
                                        {
                                            //Asignando Tiempo Excedido
                                            tiempo_excedido = 0;
                                            break;
                                        }
                                    //Si es Menor
                                    case -1:
                                        //Obteniendo Tiempo Excedido
                                        TimeSpan tiempo_ex = fecha_peticion - fecha_gps;
                                        
                                        //Asignando Tiempo Excedido
                                        tiempo_excedido = (int)(tiempo_ex.TotalMinutes < 0 ? tiempo_ex.TotalMinutes * -1 : tiempo_ex.TotalMinutes) - tolerancia;
                                        break;
                                }

                                //Validando Estatus de la Unidad
                                switch (this.EstatusUnidad)
                                {
                                    case Unidad.Estatus.ParadaDisponible:
                                    case Unidad.Estatus.ParadaOcupado:
                                        {
                                            //Instanciando Estancia, Parada y Ubicación
                                            using (EstanciaUnidad estancia = new EstanciaUnidad(this._id_estancia))
                                            using (Parada stop = new Parada(estancia.id_parada))
                                            using (Ubicacion ubicacion = new Ubicacion(stop.id_ubicacion))
                                            {
                                                //Validando que existan Registros
                                                if (estancia.habilitar && stop.habilitar && ubicacion.habilitar)
                                                {
                                                    //Validando que exista la Ubicación
                                                    if (ubicacion.geoubicacion != SqlGeography.Null)
                                                    {
                                                        //Obtiene Distancia Permitida
                                                        int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                        //Obteniendo Distancia por Ubicación
                                                        using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                                        SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(
                                                                                                0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                        {
                                                            //Validando que exista la Referencia
                                                            if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                            {
                                                                //Recorriendo Registro
                                                                foreach (DataRow dr in dtDistancia.Rows)

                                                                    //Obteniendo Distancia Permitida
                                                                    distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                            }
                                                        }

                                                        //Validando Tipo de Geometria
                                                        switch (ubicacion.geoubicacion.STGeometryType().Value)
                                                        {
                                                            case "Point":
                                                                {
                                                                    //Validando que el Punto no exceda mas de 10 metros
                                                                    if (!DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, ubicacion_gps_unidad, distancia_permitida))

                                                                        //Instanciando Resultado Positivo
                                                                        resultado = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente;

                                                                    //Instanciando Excepción
                                                                    result = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                    break;
                                                                }
                                                            case "LineString":
                                                            case "CompoundCurve":
                                                            case "Polygon":
                                                            case "CurvePolygon":
                                                                {
                                                                    //Obtiene Punto mas Cercano
                                                                    SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                                    //Validando que exista el Punto mas Cercano
                                                                    if (punto_cercano != SqlGeography.Null)
                                                                    {
                                                                        //Validando que haya Intersección en las columnas
                                                                        if (!DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, ubicacion_gps_unidad))

                                                                            //Instanciando Resultado Positivo
                                                                            resultado = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente;

                                                                        //Instanciando Excepción
                                                                        result = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                    }
                                                                    else
                                                                        //Instanciando Excepción
                                                                        result = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                    break;
                                                                }
                                                        }
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                            }
                                            break;
                                        }
                                    case Unidad.Estatus.Transito:
                                        {
                                            //Instanciando Movimiento y Parada Destino
                                            using (Movimiento mov = new Movimiento(this._id_movimiento))
                                            using (Parada stop = new Parada(mov.id_parada_destino))
                                            using (Ubicacion ubicacion = new Ubicacion(stop.id_ubicacion))
                                            {
                                                //Validando que existan Registros
                                                if (mov.habilitar && stop.habilitar && ubicacion.habilitar)
                                                {
                                                    //Asignando Ubicación
                                                    id_destino = stop.id_parada;

                                                    //Validando si la Unidad esta en Movimiento
                                                    if (velocidad > 0)

                                                        //Instanciando Resultado Correcto
                                                        result = new RetornoOperacion(0, "", true);
                                                    else
                                                    {
                                                        //Validando que exista la Ubicación
                                                        if (ubicacion.geoubicacion != SqlGeography.Null)
                                                        {
                                                            //Obtiene Distancia Permitida
                                                            int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                            //Obteniendo Distancia por Ubicación
                                                            using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                                            SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                            {
                                                                //Validando que exista la Referencia
                                                                if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                                {
                                                                    //Recorriendo Registro
                                                                    foreach (DataRow dr in dtDistancia.Rows)

                                                                        //Obteniendo Distancia Permitida
                                                                        distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                                }
                                                            }

                                                            //Validando Tipo de Geometria
                                                            switch (ubicacion.geoubicacion.STGeometryType().Value)
                                                            {
                                                                case "Point":
                                                                    {
                                                                        //Validando que el Punto no exceda mas de 10 metros
                                                                        if (!DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, ubicacion_gps_unidad, distancia_permitida))

                                                                            //Instanciando Resultado Positivo
                                                                            resultado = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida;

                                                                        //Instanciando Excepción
                                                                        result = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                        break;
                                                                    }
                                                                case "LineString":
                                                                case "CompoundCurve":
                                                                case "Polygon":
                                                                case "CurvePolygon":
                                                                    {
                                                                        //Obtiene Punto mas Cercano
                                                                        SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                                        //Validando que exista el Punto mas Cercano
                                                                        if (punto_cercano != SqlGeography.Null)
                                                                        {
                                                                            //Validando que haya Intersección en las columnas
                                                                            if (!DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, ubicacion_gps_unidad))
                                                                            {

                                                                                //Instanciando Resultado Positivo
                                                                                resultado = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida;
                                                                            }

                                                                            //Instanciando Excepción
                                                                            result = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                        }
                                                                        else
                                                                            //Instanciando Excepción
                                                                            result = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }

                                //Validando Resultado Posicionamiento
                                if (result.OperacionExitosa)
                                {
                                    //Instanciando Parada de Destino
                                    using (Parada destino = new Parada(id_destino))
                                    using (Ubicacion dest = new Ubicacion(destino.id_ubicacion))
                                    {
                                        //Validando que exista el Destino
                                        if (destino.habilitar && dest.habilitar)
                                        {
                                            //Obtiene Distancia Permitida
                                            SqlGeography d_point = SqlGeography.Null;
                                            //Validando Tipo de Geometria
                                            switch (dest.geoubicacion.STGeometryType().Value)
                                            {
                                                case "LineString":
                                                case "CompoundCurve":
                                                case "Polygon":
                                                case "CurvePolygon":
                                                    {
                                                        d_point = dest.geoubicacion.EnvelopeCenter();
                                                        break;
                                                    }
                                                case "Point":
                                                    {
                                                        d_point = dest.geoubicacion;
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        d_point = SqlGeography.Null;
                                                        break;
                                                    }
                                            }

                                            if (d_point != SqlGeography.Null && !(d_point.Lat == 0 && d_point.Long == 0))
                                            {
                                                //Obteniendo Datos API
                                                result = DistanceMatrix.objDistanceMatrix.ObtieneDistanciaOrigenDestinoXML(ubicacion_gps_unidad, dest.geoubicacion,
                                                                                                    DistanceMatrix.Unidades.Metric, out distancia, out duracion);

                                                //Validando Resultado Posicionamiento
                                                if (result.OperacionExitosa)
                                                {
                                                    //Validando si no hay errores
                                                    if (resultado == Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok && this.EstatusUnidad == Estatus.Transito)

                                                        //Obteniendo Resultado de Cercania
                                                        resultado = Monitoreo.EvaluacionBitacora.ObtieneCercaniaEvaluacion(id_bitacora, distancia, fecha_peticion);
                                                }
                                            }
                                            else
                                                resultado = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido;
                                        }

                                        //Validando Resultado Posicionamiento
                                        if (result.OperacionExitosa)
                                        {
                                            //Validando Estatus de Evaluación
                                            if (resultado == Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok && tiempo_excedido > 0)

                                                //Asignando Resultado
                                                resultado = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido;

                                            //Insertando Evaluación de la Bitacora
                                            result = SAT_CL.Monitoreo.EvaluacionBitacora.InsertaEvaluacionBitacora(id_bitacora, fecha_peticion, resultado, tiempo_excedido, distancia,
                                                    duracion, dest.habilitar ? fecha_peticion.AddSeconds((double)duracion) : DateTime.MinValue, destino.cita_parada, id_usuario);

                                            //Validando Resultado Posicionamiento
                                            if (result.OperacionExitosa)
                                            {
                                                //Asignando Evaluación
                                                id_evaluacion = result.IdRegistro;

                                                //Instanciando Unidad
                                                result = new RetornoOperacion(this._id_unidad);

                                                //Completando Transacción
                                                trans.Complete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe la Antena");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Realiza la actualización de la cantidad de kilometros que se han asignado a la unidad
        /// </summary>
        /// <param name="combustible_asignado">Litros de combustible asignado que se incrementarán al total actual</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion RestaCombustibleAsignado(decimal combustible_asignado, int id_usuario)
        {
            //Realizando actualización
            return editaUnidad(this._numero_unidad, this._id_compania_emisor, this.EstatusUnidad, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._ejes, this._id_dimension, this._bit_no_propia,
                this._fecha_adquisicion, this._fecha_baja, this._id_compania_proveedor, this._id_marca, this._modelo, this._ano, this._serie, this._id_marca_motor, this._modelo_motor, this._serie_motor,
                this._id_estado_placas, this._placas, this._peso_tara, this._id_unidad_medida_peso, this._kilometraje_asignado, this._capacidad_combustible, combustible_asignado, this._id_operador,
                this._antena_gps_principal, this._id_estancia, this._id_movimiento, this._fecha_actualizacion, this._id_configuracion, id_usuario, this._habilitar);
        }
        #endregion
    }
}
