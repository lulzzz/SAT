using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de las operaciones de los Conceptos
    /// </summary>
    public class Concepto : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo que almacena el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_concepto_tcct";

        private int _id_concepto;
        /// <summary>
        /// Atributo que almacena el Identificador del Concepto
        /// </summary>
        public int id_concepto33 { get { return this._id_concepto; } }
        private int _id_comprobante;
        /// <summary>
        /// Atributo que almacena el Comprobante del Concepto
        /// </summary>
        public int id_comprobante { get { return this._id_comprobante; } }
        private int _id_concepto_padre;
        /// <summary>
        /// Atributo que almacena el Concepto que engloba al Concepto
        /// </summary>
        public int id_concepto_padre { get { return this._id_concepto_padre; } }
        private decimal _cantidad;
        /// <summary>
        /// Atributo que almacena la Cantidad del Concepto
        /// </summary>
        public decimal cantidad { get { return this._cantidad; } }
        private int _id_unidad;
        /// <summary>
        /// Atributo que almacena la Unidad de Medida del Concepto
        /// </summary>
        public int id_unidad { get { return this._id_unidad; } }
        private int _id_tipo_cargo;
        /// <summary>
        /// Atributo que almacena el Tipo de Cargo del Sistema SAT
        /// </summary>
        public int id_tipo_cargo { get { return this._id_tipo_cargo; } }
        private int _id_clave_sat;
        /// <summary>
        /// Atributo que almacena la Clave del Catalogo del SAT
        /// </summary>
        public int id_clave_sat { get { return this._id_clave_sat; } }
        private string _descripcion;
        /// <summary>
        /// Atributo que almacena la Descripción del Concepto
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private string _no_identificacion;
        /// <summary>
        /// Atributo que almacena el Número de Identificación del Concepto
        /// </summary>
        public string no_identificacion { get { return this._no_identificacion; } }
        private decimal _valor_unitario;
        /// <summary>
        /// Atributo que almacena el Valor Unitario del Concepto
        /// </summary>
        public decimal valor_unitario { get { return this._valor_unitario; } }
        private decimal _importe_moneda_captura;
        /// <summary>
        /// Atributo que almacena el Importe de Captura del Concepto
        /// </summary>
        public decimal importe_moneda_captura { get { return this._importe_moneda_captura; } }
        private decimal _importe_moneda_nacional;
        /// <summary>
        /// Atributo que almacena el Importe en Moneda Nacional del Concepto
        /// </summary>
        public decimal importe_moneda_nacional { get { return this._importe_moneda_nacional; } }
        private string _cuenta_predial;
        /// <summary>
        /// Atributo que almacena la Cuenta Predial del Concepto
        /// </summary>
        public string cuenta_predial { get { return this._cuenta_predial; } }
        private decimal _descuento;
        /// <summary>
        /// Atributo que almacena el Descuento del Concepto
        /// </summary>
        public decimal descuento { get { return this._descuento; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de inicializar los Atributos por Defecto
        /// </summary>
        public Concepto()
        {
            //Asignando Valores
            this._id_concepto = 0;
            this._id_comprobante = 0;
            this._id_concepto_padre = 0;
            this._cantidad = 0.00M;
            this._id_unidad = 0;
            this._id_tipo_cargo = 
            this._id_clave_sat = 0;
            this._descripcion = "";
            this._no_identificacion = "";
            this._valor_unitario = 
            this._importe_moneda_captura =
            this._importe_moneda_nacional = 0.00M;
            this._cuenta_predial = "";
            this._descuento = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_concepto33">Concepto en su versión 3.3</param>
        public Concepto(int id_concepto33)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_concepto33);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Concepto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar los Atributos por Defecto
        /// </summary>
        /// <param name="id_concepto33">Concepto en su versión 3.3</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_concepto33)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_concepto33, 0, 0, 0.00M, 0, 0, 0, "", "", 0.00M, 0.00M, 0.00M, "", 0.00M, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Valores
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_concepto = id_concepto33;
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._id_concepto_padre = Convert.ToInt32(dr["IdConceptoPadre"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._id_tipo_cargo = Convert.ToInt32(dr["IdTipoCargo"]);
                        this._id_clave_sat = Convert.ToInt32(dr["IdClaveSAT"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._no_identificacion = dr["NoIdentificacion"].ToString();
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]);
                        this._importe_moneda_captura = Convert.ToDecimal(dr["ImporteMonedaCaptura"]);
                        this._importe_moneda_nacional = Convert.ToDecimal(dr["ImporteMonedaNacional"]);
                        this._cuenta_predial = dr["CuentaPredial"].ToString();
                        this._descuento = Convert.ToDecimal(dr["Descuento"]);
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
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_comprobante">Comprobante del Concepto</param>
        /// <param name="id_concepto_padre">Concepto Padre del Concepto</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Unidad de Medida</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo (Sistema TECTOS_SAT)</param>
        /// <param name="id_clave_sat">Clave SAT</param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <param name="no_identificacion">No. de Identificación</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe_moneda_captura">Importe Total del Concepto en Moneda de Captura del Comprobante</param>
        /// <param name="importe_moneda_nacional">Importe Total del Concepto en Moneda Nacional del Comprobante</param>
        /// <param name="cuenta_predial">Cuenta Predial</param>
        /// <param name="descuento">Descuento</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_comprobante, int id_concepto_padre, decimal cantidad, int id_unidad, 
                                            int id_tipo_cargo, int id_clave_sat, string descripcion, string no_identificacion, 
                                            decimal valor_unitario, decimal importe_moneda_captura, decimal importe_moneda_nacional, 
                                            string cuenta_predial, decimal descuento, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_concepto, id_comprobante, id_concepto_padre, cantidad, id_unidad, id_tipo_cargo, id_clave_sat,
                               descripcion, no_identificacion, valor_unitario, importe_moneda_captura, importe_moneda_nacional, 
                               cuenta_predial, descuento, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Conceptos del Comprobante v3.3
        /// </summary>
        /// <param name="id_comprobante">Comprobante del Concepto</param>
        /// <param name="id_concepto_padre">Concepto Padre del Concepto</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Unidad de Medida</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo (Sistema TECTOS_SAT)</param>
        /// <param name="id_clave_sat"></param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <param name="no_identificacion">No. de Identificación</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe_moneda_captura">Importe Total del Concepto en Moneda de Captura del Comprobante</param>
        /// <param name="importe_moneda_nacional">Importe Total del Concepto en Moneda Nacional del Comprobante</param>
        /// <param name="cuenta_predial"></param>
        /// <param name="descuento">Descuento</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaConcepto(int id_comprobante, int id_concepto_padre, decimal cantidad, int id_unidad,
                                            int id_tipo_cargo, int id_clave_sat, string descripcion, string no_identificacion, 
                                            decimal valor_unitario, decimal importe_moneda_captura, decimal importe_moneda_nacional, 
                                            string cuenta_predial, decimal descuento, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_comprobante, id_concepto_padre, cantidad, id_unidad, id_tipo_cargo, id_clave_sat, descripcion + (!string.IsNullOrEmpty(no_identificacion) ? " - " + no_identificacion : ""), "", 
                               valor_unitario, importe_moneda_captura, importe_moneda_nacional, cuenta_predial, descuento, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Conceptos del Comprobante v3.3
        /// </summary>
        /// <param name="id_comprobante">Comprobante del Concepto</param>
        /// <param name="id_concepto_padre">Concepto Padre del Concepto</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Unidad de Medida</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo (Sistema TECTOS_SAT)</param>
        /// <param name="id_clave_sat"></param>
        /// <param name="descripcion">Descripción del Concepto</param>
        /// <param name="no_identificacion">No. de Identificación</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe_moneda_captura">Importe Total del Concepto en Moneda de Captura del Comprobante</param>
        /// <param name="importe_moneda_nacional">Importe Total del Concepto en Moneda Nacional del Comprobante</param>
        /// <param name="cuenta_predial"></param>
        /// <param name="descuento"></param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaConcepto(int id_comprobante, int id_concepto_padre, decimal cantidad, int id_unidad,
                                    int id_tipo_cargo, int id_clave_sat, string descripcion, string no_identificacion, decimal valor_unitario,
                                    decimal importe_moneda_captura, decimal importe_moneda_nacional, string cuenta_predial, 
                                    decimal descuento, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_comprobante, id_concepto_padre, cantidad, id_unidad, id_tipo_cargo, id_clave_sat, descripcion, no_identificacion, 
                               valor_unitario, importe_moneda_captura, importe_moneda_nacional, cuenta_predial, descuento, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Conceptos del Comprobante v3.3
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaConcepto(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_comprobante, this._id_concepto_padre, this._cantidad, this._id_unidad, this._id_tipo_cargo, this._id_clave_sat, 
                                        this._descripcion, this._no_identificacion, this._valor_unitario, this._importe_moneda_captura, this._importe_moneda_nacional, 
                                        this._cuenta_predial, this._descuento, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos del Concepto v3.3
        /// </summary>
        /// <returns></returns>
        public bool ActualizaConcepto()
        {
            //Devolviendo Resultado Obtenido
            return this.cargaAtributosInstancia(this._id_concepto);
        }
        /// <summary>
        /// Método encargado de Obtener los Conceptos del Comprobante
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <returns></returns>
        public static DataTable ObtieneConceptosComprobante(int id_comprobante)
        {
            //Declarando Objeto de Retorno
            DataTable dtConceptos = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_comprobante, 0, 0.00M, 0, 0, 0, "", "", 0.00M, 0.00M, 0.00M, "", 0.00M,0, false, "", "" };

            //Instanciando Resultados Obtenidos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtConceptos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtConceptos;
        }
        /// <summary>
        /// Método que obtiene subtotal e impuestos de traslado y retenidos del comprobante
        /// </summary>
        /// <param name="id_comprobante">id del compobante que se esta generando</param>
        /// <returns></returns>
        public static DataTable ObtineSubtotalComprobante(int id_comprobante)
        {
            DataTable dtConceptos = null;
            //Generando arreglo para obtener valor de los parametros
            object[] param = { 4, 0, id_comprobante, 0, 0.00M, 0, 0, 0, "", "", 0.00M, 0.00M, 0.00M, "", 0.00M, 0, false, "", "" };
            //Instanciando Resultado Obtenido 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table1"))
                    //Asigna resutado obtenido en la tabla
                    dtConceptos = ds.Tables["Table1"];
            }
            //Devuelve Resultado Obtenido
            return dtConceptos;
        }
        /// <summary>
        /// Metodo que carga datos de las referencias de impresion para el subreporte
        /// </summary>
        /// <param name="id_concepto"></param>
        /// <returns></returns>
        public static DataTable CargaImpresionReferencias(int id_concepto, int id_comprobante)
        {
            object[] param = { 5, id_concepto, id_comprobante, 0, 0.00M, 0, 0, 0, "", "", 0.00M, 0.00M, 0.00M, "", 0.00M, 0, false, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {
                DataTable mit = null;
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
                return mit;
            }
        }

        #endregion
    }
}
