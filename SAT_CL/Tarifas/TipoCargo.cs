using System;
using System.Data;
using TSDK.Base;
using System.Linq;
namespace SAT_CL.Tarifas
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones de los Tipos de Carga
    /// </summary>
    public class TipoCargo : Disposable
    {
        #region Enumeracion
        /// <summary>
        /// Enumaración de lo tipos de impuesto trasladado
        /// </summary>
        public enum TipoImpTrasladado
        {
            /// <summary>
            /// Tipo de Impuesto trasladado IVA
            /// </summary>
            IVA = 1,
            /// <summary>
            /// Tipo de Impuesto trasladado IEPS
            /// </summary>
            IEPS = 2
        }
        /// <summary>
        /// Enumeración de los tipos de impuesto retenido
        /// </summary>
        public enum TipoImpRetenido
        {
            /// <summary>
            /// Tipo de Impuesto retenido IVA
            /// </summary>
            IVA = 1,
            /// <summary>
            /// Tipo de Impuesto ISR
            /// </summary>
            ISR =2
        }
        #endregion
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "tarifas.sp_tipo_cargo_ttc";

        private int _id_tipo_cargo;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Tipo de Cargo
        /// </summary>
        public int id_tipo_cargo { get { return this._id_tipo_cargo; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private byte _id_unidad;
        /// <summary>
        /// Obtiene el Id de la Unidad de Medida del Tipo de Cargo
        /// </summary>
        public byte id_unidad { get { return this._id_unidad; } }
        private byte _id_tipo_impuesto_trasladado;
        /// <summary>
        /// Atributo que almacena un tipo de impuesto traslado (IVA,IEPS)
        /// </summary>
        public byte id_tipo_impuesto_trasladado
        {
            get { return this._id_tipo_impuesto_trasladado; }
        }
        private decimal _tasa_impuesto_trasladado;
        /// <summary>
        /// Atributo encargado de almacenar la Tasa de Impuesto Trasladado
        /// </summary>
        public decimal tasa_impuesto_trasladado { get { return this._tasa_impuesto_trasladado; } }
        private byte _id_tipo_impuesto_retenido;

        public byte id_tipo_impuesto_retenido
        {
            get { return this._id_tipo_impuesto_retenido; }
        }
        private decimal _tasa_impuesto_retenido;
        /// <summary>
        /// Atributo encargado de almacenar la Tasa de Impuesto Retenido
        /// </summary>
        public decimal tasa_impuesto_retenido { get { return this._tasa_impuesto_retenido; } }
        private byte _tipo_cargo;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Cargo
        /// </summary>
        public byte tipo_cargo { get { return this._tipo_cargo; } }        
        private byte _id_moneda;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Cargo
        /// </summary>
        public byte id_moneda { get { return this._id_moneda; } }        
        private int _id_compania;
        /// <summary>
        /// Atributo encargado de almacenar la Compañia
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
        private string _cuenta_contable;
        /// <summary>
        /// Atributo encargado de almacenar la Cuenta Contable
        /// </summary>
        public string cuenta_contable { get { return this._cuenta_contable; } }        
        private int _id_base_tarifa;
        /// <summary>
        /// Atributo encargado de almacenar la Tarifa Base
        /// </summary>
        public int id_base_tarifa { get { return this._id_base_tarifa; } }
        private int _id_catalogo_sat;
        /// <summary>
        /// Atributo encargado de almacenar el Catalogo de SAT
        /// </summary>
        public int id_catalogo_sat { get { return this._id_catalogo_sat; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public TipoCargo()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public TipoCargo(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);            
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TipoCargo()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_tipo_cargo = 0;
            this._descripcion = "";
            this._id_unidad = 0;
            this._id_tipo_impuesto_trasladado=0;
            this._tasa_impuesto_trasladado = 0;
            this._id_tipo_impuesto_retenido = 0;
            this._tasa_impuesto_retenido = 0;
            this._tipo_cargo = 0;
            this._id_moneda = 0;
            this._id_compania = 0;
            this._tasa_impuesto1 = 0;
            this._tasa_impuesto2 = 0;
            this._cuenta_contable = "";
            this._id_base_tarifa = 0;
            this._id_catalogo_sat = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de parametros
            object[] param = { 3, id_registro, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo Filas
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_tipo_cargo = id_registro;
                        this._descripcion = dr["Descripcion"].ToString();
                        this._id_unidad = Convert.ToByte(dr["IdUnidad"]);
                        this._id_tipo_impuesto_trasladado = Convert.ToByte(dr["IdTipoImpuestoTrasladado"]);
                        this._tasa_impuesto_trasladado = Convert.ToDecimal(dr["TasaImpuestoTrasladado"]);
                        this._id_tipo_impuesto_retenido = Convert.ToByte(dr["IdImpuestoRetenido"]);
                        this._tasa_impuesto_retenido = Convert.ToDecimal(dr["TasaImpuestoRetenido"]);
                        this._tipo_cargo = Convert.ToByte(dr["TipoCargo"]);
                        this._id_moneda = Convert.ToByte(dr["IdMoneda"]);
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._tasa_impuesto1 = Convert.ToDecimal(dr["TasaImpuesto1"]);
                        this._tasa_impuesto2 = Convert.ToDecimal(dr["TasaImpuesto2"]);
                        this._cuenta_contable = dr["CuentaContable"].ToString();
                        this._id_base_tarifa = Convert.ToInt32(dr["IdBaseTarifa"]);
                        this._id_catalogo_sat = Convert.ToInt32(dr["IdCatalogoSAT"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo resultado obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="descripcion">Descripción del Tipo de Carga</param>
        /// <param name="id_unidad">Id de Unidad de Medida del Tipo de Cargo</param>
        /// <param name="trasladado">tipo de Impuesto Trasladado (IVA,IEPs)</param>        
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        ///  <param name="retenido">Tipo de Impuesto Retenido (IVA,ISR)</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="tipo_cargo">Tipo de Cargo</param>
        /// <param name="id_moneda">Moneda del Tipo de Cargo</param>
        /// <param name="id_compania">Compañia del Tipo de Cargo</param>
        /// <param name="tasa_impuesto1">Tasa de Impuesto 1</param>
        /// <param name="tasa_impuesto2">Tasa de Impuesto 2</param>
        /// <param name="cuenta_contable">Cuenta Contable</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion, byte id_unidad, TipoImpTrasladado trasladado, decimal tasa_impuesto_trasladado, TipoImpRetenido retenido ,
                                                    decimal tasa_impuesto_retenido, byte tipo_cargo, byte id_moneda, int id_compania, decimal tasa_impuesto1, decimal tasa_impuesto2, 
                                                    string cuenta_contable, int id_base_tarifa, int id_catalogo_sat, int id_usuario, bool habilitar)
        {   //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2, this._id_tipo_cargo, descripcion, id_unidad,(byte) trasladado ,tasa_impuesto_trasladado,(byte) retenido ,tasa_impuesto_retenido, tipo_cargo,
                               id_moneda, id_compania, tasa_impuesto1, tasa_impuesto2, cuenta_contable, id_base_tarifa, id_catalogo_sat, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Tipos de Cargo
        /// </summary>
        /// <param name="descripcion">Descripción del Tipo de Carga</param>
        /// <param name="id_unidad">Id de Unidad de Medida del Tipo de Cargo</param>
        /// <param name="trasladado">Tipo de Impuesto Trasladado (IVA,IEPS)</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="retenido">Tipo de Impuesto Retenido (IVA,ISR)</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="tipo_cargo">Tipo de Cargo</param>
        /// <param name="id_moneda">Moneda del Tipo de Cargo</param>
        /// <param name="id_compania">Compañia del Tipo de Cargo</param>
        /// <param name="tasa_impuesto1">Tasa de Impuesto 1</param>
        /// <param name="tasa_impuesto2">Tasa de Impuesto 2</param>
        /// <param name="cuenta_contable">Cuenta Contable</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_catalogo_sat"></param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTipoCargo(string descripcion, byte id_unidad, TipoImpTrasladado trasladado , decimal tasa_impuesto_trasladado, TipoImpRetenido retenido , 
                                                        decimal tasa_impuesto_retenido, byte tipo_cargo, byte id_moneda, int id_compania, decimal tasa_impuesto1, decimal tasa_impuesto2,
                                                        string cuenta_contable, int id_base_tarifa, int id_catalogo_sat, int id_usuario)
        {   //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1, 0, descripcion, id_unidad, (byte) trasladado , tasa_impuesto_trasladado, (byte) retenido , tasa_impuesto_retenido, tipo_cargo, id_moneda, id_compania, 
                               tasa_impuesto1, tasa_impuesto2, cuenta_contable, id_base_tarifa, id_catalogo_sat, id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Tipos de Cargo
        /// </summary>
        /// <param name="descripcion">Descripción del Tipo de Carga</param>
        /// <param name="id_unidad">Id de Unidad de Medida del Tipo de Cargo</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="tipo_cargo">Tipo de Cargo</param>
        /// <param name="id_moneda">Moneda del Tipo de Cargo</param>
        /// <param name="id_compania">Compañia del Tipo de Cargo</param>
        /// <param name="tasa_impuesto1">Tasa de Impuesto 1</param>
        /// <param name="tasa_impuesto2">Tasa de Impuesto 2</param>
        /// <param name="cuenta_contable">Cuenta Contable</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTipoCargo(string descripcion, byte id_unidad, TipoImpTrasladado trasladado , decimal tasa_impuesto_trasladado, 
                                        TipoImpRetenido retenido , decimal tasa_impuesto_retenido, byte tipo_cargo, byte id_moneda, int id_compania,
                                        decimal tasa_impuesto1, decimal tasa_impuesto2, string cuenta_contable, int id_base_tarifa, int id_catalogo_sat, 
                                        int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion, id_unidad, (TipoImpTrasladado)trasladado , tasa_impuesto_trasladado, (TipoImpRetenido) retenido, tasa_impuesto_retenido, tipo_cargo, id_moneda, id_compania,
                               tasa_impuesto1, tasa_impuesto2, cuenta_contable, id_base_tarifa, id_catalogo_sat, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar el Tipo de Carga
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTipoCargo(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion, this._id_unidad,(TipoImpTrasladado)this._id_tipo_impuesto_trasladado, this._tasa_impuesto_trasladado,(TipoImpRetenido) this._id_tipo_impuesto_retenido , 
                                           this._tasa_impuesto_retenido, this._tipo_cargo, this._id_moneda, this._id_compania, this._tasa_impuesto1, this._tasa_impuesto2, this._cuenta_contable, this._id_base_tarifa, 
                                           this._id_catalogo_sat, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos del Tipo de Carga
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTipoCargo()
        {   //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_tipo_cargo);
        }
        /// <summary>
        /// Recupera el Tipo de cargo principal, usado por la base de tarifa indicada y compañía
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía a Consultar</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa</param>
        /// <returns></returns>
        public static TipoCargo ObtieneTipoCargoBaseTarifa(int id_compania_emisor, int id_base_tarifa)
        { 
            //Declarando objeto decimal retorno
            TipoCargo tc = new TipoCargo();

            //Definiendo parámetros de consulta
            object[] param = { 4, 0, "", 0,0, 0, 0, 0, 0, 0, id_compania_emisor, 
                               0, 0, "", id_base_tarifa, 0, 0, false, "", ""  };

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
                        tc = new TipoCargo(Convert.ToInt32(r["Id"]));
                        //Terminando iteración
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return tc;
        }
        /// <summary>
        /// Método encargado de Obtener el Tipo de Cargo, usando la Compania, la Descripción
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <param name="id_base_tarifa">Tarifa Base</param>
        /// <returns></returns>
        public static TipoCargo ObtieneTipoCargoDescripcion(int id_compania_emisor, string descripcion, int id_base_tarifa)
        {
            //Declarando objeto decimal retorno
            TipoCargo tc = new TipoCargo();

            //Definiendo parámetros de consulta
            object[] param = { 5, 0, descripcion, 0, 0, 0, 0, 0, 0, 0, id_compania_emisor, 0, 0, "", id_base_tarifa, 0, 0, false, "", "" };

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
                        tc = new TipoCargo(Convert.ToInt32(r["Id"]));
                        //Terminando iteración
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return tc;
        }    
       
        /// <summary>
        /// Método encargado de Obtener el Id Tipo de Cargo, usando la Compania
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <returns></returns>
        public static int ObtieneTipoCargoDescripcion(int id_compania_emisor, string descripcion)
        {
            //Declarando objeto decimal retorno
            int id_tipo_cargo = 0;

            //Definiendo parámetros de consulta
            object[] param = { 6, 0, descripcion, 0, 0, 0, 0, 0, 0, 0, id_compania_emisor, 0, 0, "", 0, 0, 0, false, "", "" };

            //Realizando consulta de tipos de cargo
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    id_tipo_cargo = (from DataRow r in ds.Tables[0].Rows
                               select Convert.ToInt32(r["Id"])).FirstOrDefault();
                }
            }

            //Devolviendo resultado
            return id_tipo_cargo;
        }
        #endregion
    }
}
