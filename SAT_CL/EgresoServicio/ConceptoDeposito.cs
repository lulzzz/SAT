using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
namespace SAT_CL.EgresoServicio
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionados con los Conceptos del Deposito
    /// </summary>
    public class ConceptoDeposito : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Enumera el tipo de Concepto
        /// </summary>
        public enum TipoConcepto
        {
            /// <summary>
            /// General
            /// </summary>
            General = 1,
            /// <summary>
            /// Asignación de Diesel
            /// </summary>
            AsignacionDeDiesel
        }
        #endregion
        #region Atributos

        /// <summary>
        /// Atributos encaragdo de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "egresos_servicio.sp_concepto_deposito_tcd";

        private int _id_concepto_deposito;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Concepto del Deposito
        /// </summary>
        public int id_concepto_deposito { get { return this._id_concepto_deposito; } }
	    private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
	    private string _descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar la Descripción del Concepto
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
	    private string _nombre_corto;
        /// <summary>
        /// Atributo encargado de Almacenar el Nombre Corto del Concepto
        /// </summary>
        public string nombre_corto { get { return this._nombre_corto; } }
        private byte _id_tipo_concepto;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo Concepto
        /// </summary>
        public byte id_tipo_concepto { get { return this._id_tipo_concepto; } }
	    private bool _bit_asigna_tractor;
        /// <summary>
        /// Atributo encargado de Almacenar Indicador que Asigna el Tractor
        /// </summary>
        public bool bit_asigna_tractor { get { return this._bit_asigna_tractor; } }
	    private bool _bit_asigna_operador;
        /// <summary>
        /// Atributo encargado de Almacenar Indicador que Asigna el Operador
        /// </summary>
        public bool bit_asigna_operador { get { return this._bit_asigna_operador; } }
	    private bool _bit_asigna_proveedor;
        /// <summary>
        /// Atributo encargado de Almacenar Indicador que Asigna el Proveedor
        /// </summary>
        public bool bit_asigna_proveedor { get { return this._bit_asigna_proveedor; } }
	    private string _cuenta_contable;
        /// <summary>
        /// Atributo encargado de Almacenar la Cuenta Contable
        /// </summary>
        public string cuenta_contable { get { return this._cuenta_contable; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        /// <summary>
        /// Describe el Tipo Concepto
        /// </summary>
        public TipoConcepto Tipo
        {
            get { return (TipoConcepto)_id_tipo_concepto; }
        }

        #endregion

        #region Contructores

        /// <summary>
        /// Constructor encargado de Inicializar los Conceptos por Defecto
        /// </summary>
        public ConceptoDeposito()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Conceptos dado un Registro
        /// </summary>
        /// <param name="id_registro">Registro</param>
        public ConceptoDeposito(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ConceptoDeposito()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando valores
            this._id_concepto_deposito = 0;
            this._id_compania_emisor = 0;
            this._descripcion = "";
            this._nombre_corto = "";
            this._id_tipo_concepto = 0;
            this._bit_asigna_tractor = false;
            this._bit_asigna_operador = false;
            this._bit_asigna_proveedor = false;
            this._cuenta_contable = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, "", "",0, false, false, false, "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada Fila
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando valores
                        this._id_concepto_deposito = id_registro;
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._nombre_corto = dr["NombreCorto"].ToString();
                        this._id_tipo_concepto = Convert.ToByte(dr["IdTipoConcepto"]);
                        this._bit_asigna_tractor = Convert.ToBoolean(dr["BitAsignaTractor"]);
                        this._bit_asigna_operador = Convert.ToBoolean(dr["BitAsignaOperador"]);
                        this._bit_asigna_proveedor = Convert.ToBoolean(dr["BitAsignaProveedor"]);
                        this._cuenta_contable = dr["CuentaContable"].ToString();
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
        /// Método Privado encargado de Actualizar los Registros
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <param name="nombre_corto">Nombre Corto del Concepto</param>
        /// <param name="tipo_concepto">TipoConcepto</param>
        /// <param name="bit_asigna_tractor">Asignador del Tractor</param>
        /// <param name="bit_asigna_operador">Asignador del Operador</param>
        /// <param name="bit_asigna_proveedor">Asignador del Proveedor</param>
        /// <param name="cuenta_contable">Cuenta Contable</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_compania_emisor, string descripcion, string nombre_corto, TipoConcepto tipo_concepto, bool bit_asigna_tractor,
                                    bool bit_asigna_operador, bool bit_asigna_proveedor, string cuenta_contable, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_concepto_deposito, id_compania_emisor, descripcion, nombre_corto, tipo_concepto, bit_asigna_tractor, 
                               bit_asigna_operador, bit_asigna_proveedor, cuenta_contable, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Conceptos
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <param name="nombre_corto">Nombre Corto del Concepto</param>
        /// <param name="tipo_concepto">Tipo Concepto</param>
        /// <param name="bit_asigna_tractor">Asignador del Tractor</param>
        /// <param name="bit_asigna_operador">Asignador del Operador</param>
        /// <param name="bit_asigna_proveedor">Asignador del Proveedor</param>
        /// <param name="cuenta_contable">Cuenta Contable</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaConceptoDeposito(int id_compania_emisor, string descripcion, string nombre_corto,  TipoConcepto tipo_concepto, bool bit_asigna_tractor,
                                            bool bit_asigna_operador, bool bit_asigna_proveedor, string cuenta_contable, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania_emisor, descripcion, nombre_corto,  tipo_concepto,  bit_asigna_tractor, 
                               bit_asigna_operador, bit_asigna_proveedor, cuenta_contable, id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Conceptos
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <param name="nombre_corto">Nombre Corto del Concepto</param>
        /// <param name="tipo_concepto">Tipo Concepto</param>
        /// <param name="bit_asigna_tractor">Asignador del Tractor</param>
        /// <param name="bit_asigna_operador">Asignador del Operador</param>
        /// <param name="bit_asigna_proveedor">Asignador del Proveedor</param>
        /// <param name="cuenta_contable">Cuenta Contable</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaConceptoDeposito(int id_compania_emisor, string descripcion, string nombre_corto, TipoConcepto tipo_concepto, bool bit_asigna_tractor,
                                            bool bit_asigna_operador, bool bit_asigna_proveedor, string cuenta_contable, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_compania_emisor, descripcion, nombre_corto, tipo_concepto, bit_asigna_tractor, 
                               bit_asigna_operador, bit_asigna_proveedor, cuenta_contable, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Conceptos
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaConceptoDeposito(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_compania_emisor, this._descripcion, this._nombre_corto, (TipoConcepto)this._id_tipo_concepto, this._bit_asigna_tractor,
                               this._bit_asigna_operador, this._bit_asigna_proveedor, this._cuenta_contable, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Conceptos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRegistros()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_concepto_deposito);
        }

        /// <summary>
        /// Obtiene Concepto lignado una Descripción
        /// </summary>
        /// <param name="descripcion">Descrición</param>
        /// <param name="id_compania_emisor">Compañia Emisor</param>
        /// <returns></returns>
        public static int ObtieneConcepto(string descripcion, int id_compania_emisor)
        {
            //Declaramos Resultados
            int IdConcepto = 0;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_compania_emisor, descripcion, "", 0, false, false, false, "", 0, false, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Parada Anterior
                    IdConcepto = (from DataRow r in ds.Tables[0].Rows
                                             select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }
            }
           //Obtenemos Resultado
            return IdConcepto;
        }

        #endregion
    }
}
