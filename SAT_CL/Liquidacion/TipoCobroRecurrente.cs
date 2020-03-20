using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Clase encargada de todas la Operaciones de los Tipos de Cobros Recurrentes
    /// </summary>
    public class TipoCobroRecurrente : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Tipo de Aplicación del Cobro
        /// </summary>
        public enum TipoAplicacion
        {   /// <summary>
            /// Expresa cuando el Tipo de Aplicación es de Deducción por Impuestos
            /// </summary>
            Deduccion = 1,
            /// <summary>
            /// Expresa cuando el Tipo de Aplicación es un Descuento por algún motivo
            /// </summary>
            Descuento,
            /// <summary>
            /// Expresa cuando el Tipo de Aplicación es una Bonificación por algún motivo
            /// </summary>
            Bonificacion,
            /// <summary>
            /// Expresa cuando el Tipo de Aplicación es por la Percepción del Salario Diario
            /// </summary>
            Percepcion,
            /// <summary>
            /// Expresa cuando el Tipo de Aplicación es los Otros Pgos
            /// </summary>
            Otros
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_tipo_cobro_recurrente_ttcr";

        private int _id_tipo_cobro_recurrente;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Cobro Recurrente
        /// </summary>
        public int id_tipo_cobro_recurrente { get { return this._id_tipo_cobro_recurrente; } }
        private int _id_compania;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private byte _id_tipo_aplicacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Aplicación
        /// </summary>
        public byte id_tipo_aplicacion { get { return this._id_tipo_aplicacion; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private bool _bit_positivo;
        /// <summary>
        /// Atributo encargado de Almacenar el Indicador de Cantidad Positiva
        /// </summary>
        public bool bit_positivo { get { return this._bit_positivo; } }
        private bool _bit_sin_termino;
        /// <summary>
        /// Atributo encargado de Almacenar el Indicador de Termino del Cobro
        /// </summary>
        public bool bit_sin_termino { get { return this._bit_sin_termino; } }
        private string _clave_nomina;
        /// <summary>
        /// Atributo encargado de Almacenar la descripcion de la Clave de Nómina del catalogo del SAT
        /// </summary>
        public string clave_nomina { get { return this._clave_nomina; } }
        private byte _id_concepto_sat_nomina;
        /// <summary>
        /// Id que permite identificar la descripcion de la nonina del catalogo del sat
        /// </summary>
        public byte id_concepto_sat_nomina { get { return this._id_concepto_sat_nomina; } }
        private bool _gravado;
        /// <summary>
        /// Permite almacenar si en el tipo de cobro se aplicara un impuesto
        /// </summary>
        public bool gravado { get { return this._gravado; }}

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
        public TipoCobroRecurrente()
        {   
            //Asignando Valores
            this._id_tipo_cobro_recurrente = 0;
            this._id_compania = 0;
            this._id_tipo_aplicacion = 0;
            this._descripcion = "";
            this._bit_positivo = false;
            this._bit_sin_termino = false;
            this._id_concepto_sat_nomina = 0;
            this._clave_nomina = "";
            this._gravado = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_cobro_recurrente">Id de Tipo de Cobro Recurrente</param>
        public TipoCobroRecurrente(int id_tipo_cobro_recurrente)
        {   
            //Invocando Método de Carga
            cargaAtributosInstancia(id_tipo_cobro_recurrente);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TipoCobroRecurrente()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_cobro_recurrente">Tipo de Cobro Recurrente</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_tipo_cobro_recurrente)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_tipo_cobro_recurrente, 0, 0, "", false, false, "", 0, false, 0, false, "", "" };
            //Obteniendo Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existe el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_tipo_cobro_recurrente = id_tipo_cobro_recurrente;
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._id_tipo_aplicacion = Convert.ToByte(dr["IdTipoAplicacion"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._bit_positivo = Convert.ToBoolean(dr["BitPositivo"]);
                        this._bit_sin_termino = Convert.ToBoolean(dr["BitSinTermino"]);
                        this._clave_nomina = dr["ClaveNomina"].ToString();
                        this._id_concepto_sat_nomina = Convert.ToByte(dr["IdConceptoSatNomina"]);
                        this._gravado = Convert.ToBoolean(dr["Gravado"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
                //Asignando Resultado POsitivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_compania">Id de Compania</param>
        /// <param name="id_tipo_aplicacion">Tipo de Aplicación</param>
        /// <param name="descripcion">Descripción del Tipo nde Cobro</param>
        /// <param name="bit_positivo">Indicador de Cobro Positivo</param>
        /// <param name="bit_sin_termino">Indicador de Termino de Cobro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <param name="clave_nomina">Clave de Nómina</param>
        /// <param name="id_concepto_sat_nomina">Id que identifica el concepto de nomina del catalogo del sat</param>
        /// <param name="gravado">Permite almacenar si se aplica impuesto o no a un tipo de cobro recurrente</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_compania, byte id_tipo_aplicacion, string descripcion, bool bit_positivo, 
                                                    bool bit_sin_termino, string clave_nomina, byte id_concepto_sat_nomina, bool gravado, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tipo_cobro_recurrente, id_compania, id_tipo_aplicacion, descripcion, bit_positivo, bit_sin_termino, 
                              clave_nomina, id_concepto_sat_nomina, gravado, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Tipos de Cobros Recurrente
        /// </summary>
        /// <param name="id_compania">Id de Compania</param>
        /// <param name="id_tipo_aplicacion">Tipo de Aplicación</param>
        /// <param name="descripcion">Descripción del Tipo nde Cobro</param>
        /// <param name="bit_positivo">Indicador de Cobro Positivo</param>
        /// <param name="bit_sin_termino">Indicador de Termino de Cobro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTipoCobroRecurrente(int id_compania, byte id_tipo_aplicacion, string descripcion, bool bit_positivo,
                                                    bool bit_sin_termino, string clave_nomina, byte id_concepto_sat_nomina, bool gravado, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania, id_tipo_aplicacion, descripcion, bit_positivo, bit_sin_termino, clave_nomina, 
                                 id_concepto_sat_nomina, gravado, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Tipos de Cobros Recurrente
        /// </summary>
        /// <param name="id_compania">Id de Compania</param>
        /// <param name="id_tipo_aplicacion">Tipo de Aplicación</param>
        /// <param name="descripcion">Descripción del Tipo nde Cobro</param>
        /// <param name="bit_positivo">Indicador de Cobro Positivo</param>
        /// <param name="bit_sin_termino">Indicador de Termino de Cobro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTipoCobroRecurrente(int id_compania, byte id_tipo_aplicacion, string descripcion, bool bit_positivo,
                                                    bool bit_sin_termino, string clave_nomina, byte id_concepto_sat_nomina, bool gravado, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_compania, id_tipo_aplicacion, descripcion, bit_positivo, bit_sin_termino, clave_nomina, 
                                           id_concepto_sat_nomina, gravado, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Tipos de Cobros Recurrentes
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTipoCobroRecurrente(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_compania, this._id_tipo_aplicacion, this._descripcion, this._bit_positivo, this._bit_sin_termino,
                                           this._clave_nomina, this.id_concepto_sat_nomina, this.gravado, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualziar los Atributos de los Tipos de Cobros Recurrentes
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTipoCobroRecurrente()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_tipo_cobro_recurrente);
        }


        #endregion
    }
}
