using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL.CXP
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones de los Conceptos de las Facturas del Proveedor
    /// </summary>
    public class FacturadoProveedorConcepto : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo que almacena el Nombre del SP
        /// </summary>
        private static string _nom_sp = "cxp.sp_facturado_proveedor_concepto_tfpc";

        private int _id_facturado_proveedor_concepto;
        /// <summary>
        /// Atributo que almacena el Id del Concepto de la Factura del Proveedor
        /// </summary>
        public int id_facturado_proveedor_concepto { get { return this._id_facturado_proveedor_concepto; } }
        private int _id_factura_proveedor;
        /// <summary>
        /// Atributo que almacena la Factura del Proveedor
        /// </summary>
        public int id_factura_proveedor { get { return this._id_factura_proveedor; } }
        private decimal _cantidad;
        /// <summary>
        /// Atributo que almacena la Cantidad del Concepto
        /// </summary>
        public decimal cantidad { get { return this._cantidad; } }
        private string _unidad;
        /// <summary>
        /// Atributo que almacena la Unidad
        /// </summary>
        public string unidad { get { return this._unidad; } }
        private string _identificador;
        /// <summary>
        /// Atributo que almacena el Identificador
        /// </summary>
        public string identificador { get { return this._identificador; } }
        private string _concepto_cobro;
        /// <summary>
        /// Atributo que almacena el Concepto de Cobro
        /// </summary>
        public string concepto_cobro { get { return this._concepto_cobro; } }
        private int _id_clasificacion_contable;
        /// <summary>
        /// Atributo que almacena la Clasificación Contable
        /// </summary>
        public int id_clasificacion_contable { get { return this._id_clasificacion_contable; } }
        private decimal _valor_unitario;
        /// <summary>
        /// Atributo que almacena el Valor Unitario
        /// </summary>
        public decimal valor_unitario { get { return this._valor_unitario; } }
        private decimal _importe;
        /// <summary>
        /// Atributo que almacena el Importe
        /// </summary>
        public decimal importe { get { return this._importe; } }
        private decimal _importe_pesos;
        /// <summary>
        /// Atributo que almacena el Importe en Pesos
        /// </summary>
        public decimal importe_pesos { get { return this._importe_pesos; } }
        private decimal _tasa_impuesto_retenido;
        /// <summary>
        /// Atributo que almacena la Tasa de Impuesto Retenido
        /// </summary>
        public decimal tasa_impuesto_retenido { get { return this._tasa_impuesto_retenido; } }
        private decimal _importe_retenido;
        /// <summary>
        /// Atributo que almacena el Impuesto Retenido
        /// </summary>
        public decimal importe_retenido { get { return this._importe_retenido; } }
        private decimal _tasa_impuesto_trasladado;
        /// <summary>
        /// Atributo que almacena la Tasa de Impuesto Trasladado
        /// </summary>
        public decimal tasa_impuesto_trasladado { get { return this._tasa_impuesto_trasladado; } }
        private decimal _importe_trasladado;
        /// <summary>
        /// Atributo que almacena el Importe Trasladado
        /// </summary>
        public decimal importe_trasladado { get { return this._importe_trasladado; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public FacturadoProveedorConcepto()
        {   //Asignando Valores
            this._id_facturado_proveedor_concepto = 0;
            this._id_factura_proveedor = 0;
            this._cantidad = 0;
            this._unidad = "";
            this._identificador = "";
            this._concepto_cobro = "";
            this._id_clasificacion_contable = 0;
            this._valor_unitario = 0;
            this._importe = 0;
            this._importe_pesos = 0;
            this._tasa_impuesto_retenido = 0;
            this._importe_retenido = 0;
            this._tasa_impuesto_trasladado = 0;
            this._importe_trasladado = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_facturado_proveedor_concepto">Id del Concepto de la Factura del Proveedor</param>
        public FacturadoProveedorConcepto(int id_facturado_proveedor_concepto)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_facturado_proveedor_concepto);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~FacturadoProveedorConcepto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_facturado_proveedor_concepto">Id del Concepto de la Factura del Proveedor</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_facturado_proveedor_concepto)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arrego de Parametros
            object[] param = { 3, id_facturado_proveedor_concepto, 0, 0, "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Instanciando Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo los Registros
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_facturado_proveedor_concepto = id_facturado_proveedor_concepto;
                        this._id_factura_proveedor = Convert.ToInt32(dr["IdFacturaProveedor"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._unidad = dr["Unidad"].ToString();
                        this._identificador = dr["Identificador"].ToString();
                        this._concepto_cobro = dr["ConceptoCobro"].ToString();
                        this._id_clasificacion_contable = Convert.ToInt32(dr["IdClasificacionContable"]);
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]);
                        this._importe = Convert.ToDecimal(dr["Importe"]);
                        this._importe_pesos = Convert.ToDecimal(dr["ImportePesos"]);
                        this._tasa_impuesto_retenido = Convert.ToDecimal(dr["TasaImpuestoRetenido"]);
                        this._importe_retenido = Convert.ToDecimal(dr["ImporteRetenido"]);
                        this._tasa_impuesto_trasladado = Convert.ToDecimal(dr["TasaImpuestoTrasladado"]);
                        this._importe_trasladado = Convert.ToDecimal(dr["ImporteTrasladado"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Reusltado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_factura_proveedor">Id de la Factura del Proveedor</param>
        /// <param name="cantidad">Cantidad del Concepto</param>
        /// <param name="unidad">Unidad expresada en el Concepto</param>
        /// <param name="identificador">identificador del Concepto</param>
        /// <param name="concepto_cobro">Concepto de Cobro</param>
        /// <param name="id_clasificacion_contable">Id de Clasificación Contable</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe">Importe del Concepto</param>
        /// <param name="importe_pesos">Importe en Pesos del Concepto</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="importe_retenido">Importe Retenido</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="importe_trasladado">Importe Trasladado</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_factura_proveedor, decimal cantidad, string unidad, string identificador,
                                    string concepto_cobro, int id_clasificacion_contable, decimal valor_unitario, decimal importe,
                                    decimal importe_pesos, decimal tasa_impuesto_retenido, decimal importe_retenido, decimal tasa_impuesto_trasladado,
                                    decimal importe_trasladado, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_facturado_proveedor_concepto, id_factura_proveedor, cantidad, unidad, identificador, 
                               concepto_cobro, id_clasificacion_contable, valor_unitario, importe, importe_pesos, tasa_impuesto_retenido, 
                               importe_retenido, tasa_impuesto_trasladado, importe_trasladado, id_usuario, habilitar, "", "" };
            //Instanciando la Factura
            using (FacturadoProveedor fp = new FacturadoProveedor(id_factura_proveedor))
            {   
                //Validando que exista la Factura
                if (fp.id_factura != 0)
                {
                    //Validando que no este transferida
                    if (!fp.bit_transferido)
                        
                        //Obteniendo Resultado del SP
                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("Factura esta Transferida. No se puede editar el Concepto");
                }
                else//Instanciando Excepcion
                    result = new RetornoOperacion("No existe la Factura");
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Conceptos de las Facturas del Proveedor
        /// </summary>
        /// <param name="id_factura_proveedor">Id de la Factura del Proveedor</param>
        /// <param name="cantidad">Cantidad del Concepto</param>
        /// <param name="unidad">Unidad expresada en el Concepto</param>
        /// <param name="identificador">identificador del Concepto</param>
        /// <param name="concepto_cobro">Concepto de Cobro</param>
        /// <param name="id_clasificacion_contable">Id de Clasificación Contable</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe">Importe del Concepto</param>
        /// <param name="importe_pesos">Importe en Pesos del Concepto</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFacturaProveedorConcepto(int id_factura_proveedor, decimal cantidad, string unidad, string identificador,
                                    string concepto_cobro, int id_clasificacion_contable, decimal valor_unitario, decimal importe,
                                    decimal importe_pesos, decimal tasa_impuesto_retenido, decimal tasa_impuesto_trasladado, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_factura_proveedor, cantidad, unidad, identificador, 
                               concepto_cobro, id_clasificacion_contable, valor_unitario, importe, importe_pesos, tasa_impuesto_retenido, 
                               0, tasa_impuesto_trasladado, 0, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Conceptos de las Facturas del Proveedor
        /// </summary>
        /// <param name="id_factura_proveedor">Id de la Factura del Proveedor</param>
        /// <param name="cantidad">Cantidad del Concepto</param>
        /// <param name="unidad">Unidad expresada en el Concepto</param>
        /// <param name="identificador">identificador del Concepto</param>
        /// <param name="concepto_cobro">Concepto de Cobro</param>
        /// <param name="id_clasificacion_contable">Id de Clasificación Contable</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe">Importe del Concepto</param>
        /// <param name="importe_pesos">Importe en Pesos del Concepto</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaFacturaProveedorConcepto(int id_factura_proveedor, decimal cantidad, string unidad, string identificador,
                                    string concepto_cobro, int id_clasificacion_contable, decimal valor_unitario, decimal importe,
                                    decimal importe_pesos, decimal tasa_impuesto_retenido, decimal tasa_impuesto_trasladado, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_factura_proveedor, cantidad, unidad, identificador,
                               concepto_cobro, id_clasificacion_contable, valor_unitario, importe, importe_pesos, tasa_impuesto_retenido,
                               0, tasa_impuesto_trasladado, 0, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Actualizar la Clasificación de los Conceptos de las Facturas del Proveedor
        /// </summary>
        /// <param name="id_clasificacion_contable">Clasificación Contable</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaClasificacionConcepto(int id_clasificacion_contable,  int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_factura_proveedor, this._cantidad, this._unidad, this._identificador,
                               this._concepto_cobro, id_clasificacion_contable, this._valor_unitario, this._importe, this._importe_pesos, this._tasa_impuesto_retenido,
                               0, this._tasa_impuesto_trasladado, 0, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Conceptos de las Facturas del Proveedor
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFacturaProveedorConcepto(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_factura_proveedor, this._cantidad, this._unidad, identificador,
                               this._concepto_cobro, this._id_clasificacion_contable, this._valor_unitario, importe, this._importe_pesos, this._tasa_impuesto_retenido,
                               this._importe_retenido, this._tasa_impuesto_trasladado, this._importe_trasladado, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Conceptos de las Facturas del Proveedor
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFacturaProveedorConcepto()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_facturado_proveedor_concepto);
        }
        /// <summary>
        /// Método Público encargado de Obtener los Conceptos Ligados a la Factura
        /// </summary>
        /// <param name="id_facturado_proveedor">Factura del Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneConceptosFactura(int id_facturado_proveedor)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando arreglo de parametros
            object[] param = { 4, 0, id_facturado_proveedor, 0, "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Valores de la Factura
        /// </summary>
        /// <param name="id_facturado_proveedor">Id de Factura del Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneValoresTotalesFacturaProveedor(int id_facturado_proveedor)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando arreglo de parametros
            object[] param = { 5, 0, id_facturado_proveedor, 0, "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Conceptos de la Factura con su Clasificación
        /// </summary>
        /// <param name="id_facturado_proveedor">Factura del Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneConceptosClasificacionFactura(int id_facturado_proveedor)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando arreglo de parametros
            object[] param = { 6, 0, id_facturado_proveedor, 0, "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }

        #endregion
    }
}
