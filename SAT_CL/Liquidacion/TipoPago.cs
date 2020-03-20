using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Tipos de Pago
    /// </summary>
    public class TipoPago : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que Expresa los Niveles de Aplicación del Tipo de Pago
        /// </summary>
        public enum NivelAplicacion
        {   /// <summary>
            /// Expresa que el Nivel de Aplicación del Tipo de Pago es para un Servicio
            /// </summary>
            Servicio = 1,
            /// <summary>
            /// Expresa que el Nivel de Aplicación del Tipo de Pago es para un Movimiento
            /// </summary>
            Movimiento
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_tipo_pago_ttp";

        private int _id_tipo_pago;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Pago
        /// </summary>
        public int id_tipo_pago { get { return this._id_tipo_pago; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private int _id_unidad_medida;
        /// <summary>
        /// Atributo encargado de almacenar la Unidad de Medida
        /// </summary>
        public int id_unidad_medida { get { return this._id_unidad_medida; } }
        private decimal _tasa_impuesto_trasladado;
        /// <summary>
        /// Atributo encargado de almacenar la Tasa de Impuesto Trasladado
        /// </summary>
        public decimal tasa_impuesto_trasladado { get { return this._tasa_impuesto_trasladado; } }
        private decimal _tasa_impuesto_retenido;
        /// <summary>
        /// Atributo encargado de almacenar la Tasa de Impuesto Retenido
        /// </summary>
        public decimal tasa_impuesto_retenido { get { return this._tasa_impuesto_retenido; } }
        private byte _id_nivel_aplicacion;
        /// <summary>
        /// Atributo encargado de almacenar el Nivel de Aplicación
        /// </summary>
        public byte id_nivel_aplicacion { get { return this._id_nivel_aplicacion; } }
        private byte _id_moneda;
        /// <summary>
        /// Atributo encargado de almacenar la Moneda
        /// </summary>
        public byte id_moneda { get { return this._id_moneda; } }
        private int _id_compania;
        /// <summary>
        /// Atributo encargado de almacenar la Compania
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private decimal _tasa_impuesto1;
        /// <summary>
        /// Atributo encargado de almacenar la Tasa de Impuesto 1
        /// </summary>
        public decimal tasa_impuesto1 { get { return this._tasa_impuesto1; } }
        private decimal _tasa_impuesto2;
        /// <summary>
        /// Atributo encargado de almacenar la Tasa de Impuesto 2
        /// </summary>
        public decimal tasa_impuesto2 { get { return this._tasa_impuesto2; } }
        private int _id_base_tarifa;
        /// <summary>
        /// Atributo encargado de almacenar la Tarifas Base
        /// </summary>
        public int id_base_tarifa { get { return this._id_base_tarifa; } }

        private byte _id_concepto_sat_nomina;
        /// <summary>
        /// Id que permite identificar la descripción de nomina del catálogo del sat
        /// </summary>
        public byte id_concepto_sat_nomina { get { return this._id_concepto_sat_nomina; } }

        private bool _gravado;
        /// <summary>
        /// Permite almacena si se aplicara o no un impuesto a un tipo de pago
        /// </summary>
        public bool gravado { get { return this._gravado; } }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de alamcenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public TipoPago()
        {   //Asignando Valores
            this._id_tipo_pago = 0;
            this._descripcion = "";
            this._id_unidad_medida = 0;
            this._tasa_impuesto_trasladado = 0;
            this._tasa_impuesto_retenido = 0;
            this._id_nivel_aplicacion = 0;
            this._id_moneda = 0;
            this._id_compania = 0;
            this._tasa_impuesto1 = 0;
            this._tasa_impuesto2 = 0;
            this._id_base_tarifa = 0;
            this._id_concepto_sat_nomina = 0;
            this._gravado=false;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_pago"></param>
        public TipoPago(int id_tipo_pago)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_tipo_pago);
        }


        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TipoPago()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_pago">Id Tipo de Pago</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_tipo_pago)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_tipo_pago, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, false, "", "" };
            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_tipo_pago = id_tipo_pago;
                        this._descripcion = dr["Descripcion"].ToString();
                        this._id_unidad_medida = Convert.ToInt32(dr["IdUnidadMedida"]);
                        this._tasa_impuesto_trasladado = Convert.ToDecimal(dr["TasaImpuestoTrasladado"]);
                        this._tasa_impuesto_retenido = Convert.ToDecimal(dr["TasaImpuestoRetenido"]);
                        this._id_nivel_aplicacion = Convert.ToByte(dr["IdNivelAplicacion"]);
                        this._id_moneda = Convert.ToByte(dr["IdMoneda"]);
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._tasa_impuesto1 = Convert.ToDecimal(dr["TasaImpuesto1"]);
                        this._tasa_impuesto2 = Convert.ToDecimal(dr["TasaImpuesto2"]);
                        this._id_base_tarifa = Convert.ToInt32(dr["IdBaseTarifa"]);
                        this._id_concepto_sat_nomina = Convert.ToByte(dr["IdConceptoSatNomina"]);
                        this._gravado = Convert.ToBoolean(dr["Gravado"]);
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
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="descripcion">Descripción del Tipo de Pago</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="id_nivel_aplicacion">Nivel de Aplicación</param>
        /// <param name="id_moneda">Moneda</param>
        /// <param name="id_compania">Compania</param>
        /// <param name="tasa_impuesto1">Tasa de Impuesto 1</param>
        /// <param name="tasa_impuesto2">Tasa de Impuesto 2</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_concepto_sat_nomina">Permite actualizar el identificador del concepto de nomina.</param>
        /// <param name="habilitar">Permite actualizar si a un tipo de pago se le aplicaran impuestos o no,</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion, int id_unidad_medida, decimal tasa_impuesto_trasladado, decimal tasa_impuesto_retenido,
                                                    byte id_nivel_aplicacion, byte id_moneda, int id_compania, decimal tasa_impuesto1, decimal tasa_impuesto2, int id_base_tarifa, 
                                                    byte id_concepto_sat_nomina, bool gravado, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tipo_pago, descripcion, id_unidad_medida, tasa_impuesto_trasladado, tasa_impuesto_retenido, 
                               id_nivel_aplicacion, id_moneda, id_compania, tasa_impuesto1, tasa_impuesto2, id_base_tarifa,
                               id_concepto_sat_nomina,gravado, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }        

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Tipos de Pago
        /// </summary>
        /// <param name="descripcion">Descripción del Tipo de Pago</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="id_nivel_aplicacion">Nivel de Aplicación</param>
        /// <param name="id_moneda">Moneda</param>
        /// <param name="id_compania">Compania</param>
        /// <param name="tasa_impuesto1">Tasa de Impuesto 1</param>
        /// <param name="tasa_impuesto2">Tasa de Impuesto 2</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_concepto_sat_nomina">Almacena el identificador de la descripción de nomina</param>
        /// <param name="gravado">Almacena si un impuesto se aplicara a un TipoPago (Gravado = 1, No Gravado =0)</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTipoPago(string descripcion, int id_unidad_medida, decimal tasa_impuesto_trasladado, decimal tasa_impuesto_retenido,
                                                    byte id_nivel_aplicacion, byte id_moneda, int id_compania, decimal tasa_impuesto1, decimal tasa_impuesto2, int id_base_tarifa,
                                                    byte id_concepto_sat_nomina, bool gravado, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, descripcion, id_unidad_medida, tasa_impuesto_trasladado, tasa_impuesto_retenido, 
                               id_nivel_aplicacion, id_moneda, id_compania, tasa_impuesto1, tasa_impuesto2, id_base_tarifa,
                               id_concepto_sat_nomina, gravado, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Tipos de Pago
        /// </summary>
        /// <param name="descripcion">Descripción del Tipo de Pago</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="id_nivel_aplicacion">Nivel de Aplicación</param>
        /// <param name="id_moneda">Moneda</param>
        /// <param name="id_compania">Compania</param>
        /// <param name="tasa_impuesto1">Tasa de Impuesto 1</param>
        /// <param name="tasa_impuesto2">Tasa de Impuesto 2</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_concepto_sat_nomina">Actualiza el campo id_concepto_sat_nomina</param>
        /// <param name="gravado">Actualiza el campo gravado </param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTipoPago(string descripcion, int id_unidad_medida, decimal tasa_impuesto_trasladado, decimal tasa_impuesto_retenido,
                                              byte id_nivel_aplicacion, byte id_moneda, int id_compania, decimal tasa_impuesto1, decimal tasa_impuesto2, int id_base_tarifa,
                                              byte id_concepto_sat_nomina,bool gravado, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion, id_unidad_medida, tasa_impuesto_trasladado, tasa_impuesto_retenido,
                               id_nivel_aplicacion, id_moneda, id_compania, tasa_impuesto1, tasa_impuesto2, id_base_tarifa,
                               id_concepto_sat_nomina, gravado, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Tipos de Pago
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTipoPago(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion, this._id_unidad_medida, this._tasa_impuesto_trasladado, this._tasa_impuesto_retenido,
                               this._id_nivel_aplicacion, this._id_moneda, this._id_compania, this._tasa_impuesto1, this._tasa_impuesto2, this._id_base_tarifa,
                               this.id_concepto_sat_nomina, this.gravado, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Tipo de Pago
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTipoPago()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_tipo_pago);
        }
        
        /// <summary>
        /// Recupera el Tipo de cargo principal, usado por la base de tarifa indicada y compañía
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía a Consultar</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa</param>
        /// <returns></returns>
        public static TipoPago ObtieneTipoPagoBaseTarifa(int id_compania_emisor, int id_base_tarifa)
        {
            //Declarando objeto decimal retorno
            TipoPago tc = new TipoPago();

            //Definiendo parámetros de consulta
            object[] param = { 4, 0, "", 0, 0, 0, 0, 0, id_compania_emisor, 0, 0, id_base_tarifa, 0, false, 0, false, "", "" };

            //Realizando consulta de tipos de cargo
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Para cada resultado
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Instanciando tipo de cargo
                        tc = new TipoPago(Convert.ToInt32(r["Id"]));
                        //Terminando iteración
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return tc;
        }

        #endregion
    }
}
