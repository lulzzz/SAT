using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Facturacion
{   
    /// <summary>
    /// Clase encargada de todas las operaciones de los Conceptos de las Facturas
    /// </summary>
    public class FacturadoConcepto : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "facturacion.sp_facturado_concepto_tfc";

        private int _id_detalle_facturado;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Detalle del Facturado (Concepto)
        /// </summary>
        public int id_detalle_facturado { get { return this._id_detalle_facturado; } }
        private int _id_factura;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Id de Factura
        /// </summary>
        public int id_factura { get { return this._id_factura; } }
        private decimal _cantidad;
        /// <summary>
        /// Atributo encargado de almacenar el valor de la Cantidad
        /// </summary>
        public decimal cantidad { get { return this._cantidad; } }
        private byte _id_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el valor de Tipo de Unidad
        /// </summary>
        public byte id_unidad { get { return this._id_unidad; } }
        private string _identificador;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Identificador
        /// </summary>
        public string identificador { get { return this._identificador; } }
        private int _id_concepto_cobro;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Concepto de Cobro
        /// </summary>
        public int id_concepto_cobro { get { return this._id_concepto_cobro; } }
        private decimal _valor_unitario;
        /// <summary>
        /// Atributo encargado de almacenar el Valor Unitario
        /// </summary>
        public decimal valor_unitario { get { return this._valor_unitario; } }
        private decimal _importe;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Importe
        /// </summary>
        public decimal importe { get { return this._importe; } }
        private decimal _importe_pesos;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Importe en Pesos
        /// </summary>
        public decimal importe_pesos { get { return this._importe_pesos; } }
        private byte _id_impuesto_retenido;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Id de Impuesto Retenido
        /// </summary>
        public byte id_impuesto_retenido { get { return this._id_impuesto_retenido; } }
        private decimal _tasa_impuesto_retenido;
        /// <summary>
        /// Atributo encargado de almacenar el valor de la Tasa de Impuesto Retenido
        /// </summary>
        public decimal tasa_impuesto_retenido { get { return this._tasa_impuesto_retenido; } }
        private decimal _importe_retenido;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Importe Retenido
        /// </summary>
        public decimal importe_retenido { get { return this._importe_retenido; } }
        private byte _id_impuesto_trasladado;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Id Impuesto Trasladado
        /// </summary>
        public byte id_impuesto_trasladado { get { return this._id_impuesto_trasladado; } }
        private decimal _tasa_impuesto_trasladado;
        /// <summary>
        /// Atributo encargado de almacenar el valor de la Tasa de Impuesto Trasladado
        /// </summary>
        public decimal tasa_impuesto_trasladado { get { return this._tasa_impuesto_trasladado; } }
        private decimal _importe_trasladado;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Importe Trasladado
        /// </summary>
        public decimal importe_trasladado { get { return this._importe_trasladado; } }
        private int _id_cargo_recurrente;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Id del Cargo Recurrente
        /// </summary>
        public int id_cargo_recurrente { get { return this._id_cargo_recurrente; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el valor del Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Construtor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public FacturadoConcepto()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Construtor de la Clase encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public FacturadoConcepto(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~FacturadoConcepto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando valores
            this._id_detalle_facturado = 0; 
            this._id_factura = 0;
            this._cantidad = 0;
            this._id_unidad = 0;
            this._identificador = ""; 
            this._id_concepto_cobro = 0;
            this._valor_unitario = 0;
            this._importe = 0;
            this._importe_pesos = 0;
            this._id_impuesto_retenido = 0;
            this._tasa_impuesto_retenido = 0;
            this._importe_retenido = 0;
            this._id_impuesto_trasladado = 0;
            this._tasa_impuesto_trasladado = 0;
            this._importe_trasladado = 0;
            this._id_cargo_recurrente = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de parametros
            object[] param = { 3, id_registro, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0,0,0, 0, 0, 0, false, "", "" };
            //Obteniendo el Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada uno de los Registros
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando valores
                        this._id_detalle_facturado = id_registro;
                        this._id_factura = Convert.ToInt32(dr["IdFactura"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._id_unidad = Convert.ToByte(dr["IdUnidad"]);
                        this._identificador = dr["Identificador"].ToString(); ;
                        this._id_concepto_cobro = Convert.ToInt32(dr["IdConceptoCobro"]); ;
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]);
                        this._importe = Convert.ToDecimal(dr["Importe"]);
                        this._importe_pesos = Convert.ToDecimal(dr["ImportePesos"]);
                        this._id_impuesto_retenido = Convert.ToByte(dr["IdImpuestoRetenido"]);
                        this._tasa_impuesto_retenido = Convert.ToDecimal(dr["TasaImpuestoRetenido"]);
                        this._importe_retenido = Convert.ToDecimal(dr["ImporteRetenido"]);
                        this._id_impuesto_trasladado = Convert.ToByte(dr["IdImpuestoTrasladado"]);
                        this._tasa_impuesto_trasladado = Convert.ToDecimal(dr["TasaImpuestoTrasladado"]);
                        this._importe_trasladado = Convert.ToDecimal(dr["ImporteTrasladado"]);
                        this._id_cargo_recurrente = Convert.ToInt32(dr["IdCargoRecurrente"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo resultado de operacion
            return result;
        }
        /// <summary>
        ///  Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Tipo de Unidad</param>
        /// <param name="identificador">Identificador</param>
        /// <param name="id_concepto_cobro">Concepto del Cobro</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe_pesos">Importe en Pesos</param>
        /// <param name="id_impuesto_retenido">Id Impuesto Retenido</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="id_impuesto_trasladado">Id Impuesto Trasladado</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="id_cargo_recurrente">Cargo Recurrente</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_factura, decimal cantidad, byte id_unidad, string identificador, int id_concepto_cobro, 
                                                    decimal valor_unitario, decimal importe_pesos, byte id_impuesto_retenido, decimal tasa_impuesto_retenido,
                                                    byte id_impuesto_trasladado, decimal tasa_impuesto_trasladado, int id_cargo_recurrente, int id_usuario, bool habilitar)
        {   
            //Declarando Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando arreglo de parametros
            object[] param = { 2, this._id_detalle_facturado, id_factura, cantidad, id_unidad, identificador, id_concepto_cobro, 
                               valor_unitario, 0, importe_pesos, id_impuesto_retenido, tasa_impuesto_retenido, 0, id_impuesto_trasladado, 
                               tasa_impuesto_trasladado, 0, id_cargo_recurrente, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar si la el Concepto esta ligado a un Factura Global Activa
        /// </summary>
        /// <returns></returns>
        private bool validaConceptoFacturaGlobal()
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando arreglo de parametros
            object[] param = { 11, this._id_detalle_facturado, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo el Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo cada uno de los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    
                        //Asignando Resultado Obtenido
                        result = Convert.ToInt32(dr["Indicador"]) == 0 ? false : true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Conceptos de las Facturas
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Tipo de Unidad</param>
        /// <param name="identificador">Identificador</param>
        /// <param name="id_concepto_cobro">Concepto del Cobro</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe_pesos">Importe en Pesos</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="id_cargo_recurrente"></param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFacturaConcepto(int id_factura, decimal cantidad, byte id_unidad, string identificador, int id_concepto_cobro,
                                                    decimal valor_unitario, decimal importe_pesos, byte id_impuesto_retenido,  decimal tasa_impuesto_retenido,
                                                   byte id_impuesto_trasladado, decimal tasa_impuesto_trasladado, int id_cargo_recurrente, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            int idFacturaConcepto = 0;

            //Inicializando Transac
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {   
                //Instanciando factura
                using (Facturado fac = new Facturado(id_factura))
                {
                    //validando que exista la Factura
                    if (fac.id_factura != 0)
                    {
                        //Validando la Moneda
                        if (fac.moneda != 1)
                        {
                            //Instanciando Servicio
                            using (Documentacion.Servicio serv = new Documentacion.Servicio(fac.id_servicio))

                            //Instanciando Facturación Otros
                            using (FacturacionOtros fo = FacturacionOtros.ObtieneInstanciaFactura(id_factura))

                            //Instanciando Tipo de Cambio
                            using (Bancos.TipoCambio tc = new Bancos.TipoCambio(serv.id_compania_emisor == 0 ? fo.id_compania_emisora : serv.id_compania_emisor, (byte)fac.moneda, fac.fecha_tipo_cambio, 0))
                            using (SAT_CL.FacturacionElectronica33.Moneda mon = new FacturacionElectronica33.Moneda(fac.moneda))
                            {
                                //Validando que existe el Tipo de Cambio
                                if (tc.habilitar && mon.habilitar)
                                {
                                    //Armando arreglo de parametros
                                    object[] param = { 1, 0, id_factura, cantidad, id_unidad, identificador, id_concepto_cobro, 
                                                        valor_unitario, 0, System.Math.Round((cantidad * valor_unitario), 2,MidpointRounding.ToEven) * tc.valor_tipo_cambio, id_impuesto_retenido, tasa_impuesto_retenido, 0, 
                                                        id_impuesto_trasladado, tasa_impuesto_trasladado, 0, id_cargo_recurrente,  id_usuario, true, "", "" };

                                    //Obteniendo Resultado del SP
                                    result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                                }
                                else
                                    //Instanciando Error
                                    result = new RetornoOperacion(string.Format("No existe el Tipo de Cambio del dia '{0:dd/MM/yyyy HH:mm}' de la Moneda '{1}'", fac.fecha_tipo_cambio, mon.clave));
                            }
                        }
                        else
                        {
                            //Armando arreglo de parametros
                            object[] param = { 1, 0, id_factura, cantidad, id_unidad, identificador, id_concepto_cobro, 
                                                valor_unitario, 0, System.Math.Round((cantidad * valor_unitario), 2,MidpointRounding.ToEven), id_impuesto_retenido, tasa_impuesto_retenido, 0, 
                                                id_impuesto_trasladado, tasa_impuesto_trasladado, 0, id_cargo_recurrente,  id_usuario, true, "", "" };

                            //Obteniendo Resultado del SP
                            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                        }

                        //Validando que la Operacion haya sido exitosa
                        if (result.OperacionExitosa)
                        {
                            //Guardando Id del Concepto
                            idFacturaConcepto = result.IdRegistro;

                            //Obteniendo Valores Totales
                            using (DataTable dt = FacturadoConcepto.ObtieneValoresTotalesFactura(id_factura))
                            {
                                //Validando que la tabla contenga Valores
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                                {
                                    //Recorriendo cada Fila
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        //Editando Factura
                                        result = fac.EditaFactura(Convert.ToDecimal(dr["TotalFactura"]), Convert.ToDecimal(dr["SubTotalFactura"]),
                                                            Convert.ToDecimal(dr["TrasladadoFactura"]), Convert.ToDecimal(dr["RetenidoFactura"]), 1);
                                    }
                                }
                            }

                            //Validando que las Operaciones hayan sido exitosas
                            if (result.OperacionExitosa)
                            {
                                //Instanciando Concepto
                                result = new RetornoOperacion(idFacturaConcepto);

                                //Completando la Transaccion
                                scope.Complete();
                            }
                        }
                    }
                    else
                        //Instanciando Error
                        result = new RetornoOperacion("No existe la factura");
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Insertar los Conceptos de las Facturas
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Tipo de Unidad</param>
        /// <param name="identificador">Identificador</param>
        /// <param name="id_concepto_cobro">Concepto del Cobro</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="id_impuesto_retenido">Id Impuesto Retenido</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="id_impuesto_trasladado">Id Impuesto Trasladado</param>
        /// <param name="id_cargo_recurrente"></param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFacturaConcepto(int id_factura, decimal cantidad, byte id_unidad, string identificador, int id_concepto_cobro,
                                                    decimal valor_unitario,  byte id_impuesto_retenido, decimal tasa_impuesto_retenido,  byte id_impuesto_trasladado, decimal tasa_impuesto_trasladado, int id_cargo_recurrente, 
                                                    int id_usuario)
        {
            return InsertaFacturaConcepto(id_factura, cantidad, id_unidad, identificador, id_concepto_cobro, valor_unitario, System.Math.Round((cantidad * valor_unitario), 2, MidpointRounding.ToEven), id_impuesto_retenido, tasa_impuesto_retenido, id_impuesto_trasladado,
                                         tasa_impuesto_trasladado, id_cargo_recurrente, id_usuario);
        }
        /// <summary>
        /// Método Público encargado de Editar los Conceptos de las Facturas
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Tipo de Unidad</param>
        /// <param name="identificador">Identificador</param>
        /// <param name="id_concepto_cobro">Concepto del Cobro</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe_pesos">Importe en Pesos</param>
        /// <param name="id_impuesto_retenido">Id Impuesto retenido</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="id_impuesto_trasladado">Id Impuesto Trasladado</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="id_cargo_recurrente">Id de cargo recurrente</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaImportePesos(decimal tipo_cambio, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_factura, this._cantidad, this._id_unidad, this._identificador, this._id_concepto_cobro, this._valor_unitario, 
                                    System.Math.Round((this._cantidad * this._valor_unitario), 2, MidpointRounding.ToEven) * tipo_cambio, this.id_impuesto_retenido, this._tasa_impuesto_retenido,
                                    this._id_impuesto_trasladado, this._tasa_impuesto_trasladado, this._id_cargo_recurrente, id_usuario, 
                                    this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Editar los Conceptos de las Facturas
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Tipo de Unidad</param>
        /// <param name="identificador">Identificador</param>
        /// <param name="id_concepto_cobro">Concepto del Cobro</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="importe_pesos">Importe en Pesos</param>
        /// <param name="id_impuesto_retenido">Id Impuesto retenido</param>
        /// <param name="tasa_impuesto_retenido">Tasa de Impuesto Retenido</param>
        /// <param name="id_impuesto_trasladado">Id Impuesto Trasladado</param>
        /// <param name="tasa_impuesto_trasladado">Tasa de Impuesto Trasladado</param>
        /// <param name="id_cargo_recurrente">Id de cargo recurrente</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaFacturaConcepto(int id_factura, decimal cantidad, byte id_unidad, string identificador, int id_concepto_cobro,
                                                    decimal valor_unitario, decimal importe_pesos, byte id_impuesto_retenido, decimal tasa_impuesto_retenido,
                                                     byte id_impuesto_trasladado, decimal tasa_impuesto_trasladado, int id_cargo_recurrente, int id_usuario)
        {
            //Declarando Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloques
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando factura
                using (Facturado fac = new Facturado(this._id_factura))
                {
                    //validando que exista la Factura
                    if (fac.id_factura != 0)
                    {
                        //Validando que la Moneda sea dinstinto de Pesos
                        if (fac.moneda != 1)
                        {
                            //Instanciando Servicio
                            using (Documentacion.Servicio serv = new Documentacion.Servicio(fac.id_servicio))

                            //Instanciando Facturación Otros
                            using (FacturacionOtros fo = FacturacionOtros.ObtieneInstanciaFactura(this._id_factura))

                            //Instanciando Tipo de Cambio
                            using (Bancos.TipoCambio tc = new Bancos.TipoCambio(serv.id_compania_emisor == 0 ? fo.id_compania_emisora : serv.id_compania_emisor, (byte)fac.moneda, fac.fecha_tipo_cambio, 0))
                            {
                                //Validando que exista el Tipo de Cambio
                                if (tc.id_tipo_cambio > 0)

                                    //Actualizando Registros
                                    result = this.actualizaRegistros(id_factura, cantidad, id_unidad, identificador, id_concepto_cobro, valor_unitario,
                                                            System.Math.Round((cantidad * valor_unitario), 2, MidpointRounding.ToEven) * tc.valor_tipo_cambio, id_impuesto_retenido,
                                                            tasa_impuesto_retenido, id_impuesto_trasladado, tasa_impuesto_trasladado, id_cargo_recurrente,
                                                            id_usuario, this._habilitar);
                                else
                                    //Instanciando Error
                                    result = new RetornoOperacion("No existe el Tipo de Cambio");
                            }
                        }
                        else
                            //Actualizando Registros
                            result = this.actualizaRegistros(id_factura, cantidad, id_unidad, identificador, id_concepto_cobro, valor_unitario, 
                                                    System.Math.Round((cantidad * valor_unitario), 2, MidpointRounding.ToEven), id_impuesto_retenido, 
                                                    tasa_impuesto_retenido, id_impuesto_trasladado, tasa_impuesto_trasladado, id_cargo_recurrente, 
                                                    id_usuario, this._habilitar);

                        //Validando que se Actualizara
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Valores Totales
                            using (DataTable dt = FacturadoConcepto.ObtieneValoresTotalesFactura(this._id_factura))
                            {
                                //Validando que la tabla contenga Valores
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                                {
                                    //Recorriendo cada Fila
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        //Editando Factura
                                        result = fac.EditaFactura(Convert.ToDecimal(dr["TotalFactura"]), Convert.ToDecimal(dr["SubTotalFactura"]),
                                                            Convert.ToDecimal(dr["TrasladadoFactura"]), Convert.ToDecimal(dr["RetenidoFactura"]), id_usuario);
                                    }
                                }
                            }
                        }
                    }
                    else
                        //Instanciando Error
                        result = new RetornoOperacion("No existe la factura");
                }

                //Validando que las Operaciones hayan sido exitosas
                if (result.OperacionExitosa)
                {
                    //Instanciando Factura Concepto
                    result = new RetornoOperacion(this._id_detalle_facturado);

                    //Completando Transaccion
                    scope.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Conceptos de las Facturas
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFacturaConcepto(int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando si esta ligada a una Factura Global
            if (!validaConceptoFacturaGlobal())
            {
                //Inicializando Bloques
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando factura
                    using (Facturado fac = new Facturado(this._id_factura))
                    {
                        //validando que exista la Factura
                        if (fac.habilitar)
                        {
                            //Invocando Método de Actualización
                            result = this.actualizaRegistros(this._id_factura, this._cantidad, this._id_unidad, this._identificador, this._id_concepto_cobro,
                                                 this._valor_unitario, this._importe_pesos, this.id_impuesto_retenido, this._tasa_impuesto_retenido,
                                                 this._id_impuesto_trasladado, this._tasa_impuesto_trasladado, this._id_cargo_recurrente, id_usuario, false);

                            //Validando que se Actualizara
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Valores Totales
                                using (DataTable dt = FacturadoConcepto.ObtieneValoresTotalesFactura(this._id_factura))
                                {
                                    //Validando que la tabla contenga Valores
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                                    {
                                        //Recorriendo cada Fila
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            //Editando Factura
                                            result = fac.EditaFactura(Convert.ToDecimal(dr["TotalFactura"]), Convert.ToDecimal(dr["SubTotalFactura"]),
                                                                Convert.ToDecimal(dr["TrasladadoFactura"]), Convert.ToDecimal(dr["RetenidoFactura"]), id_usuario);
                                        }
                                    }
                                }
                            }

                            //Validando que las Operaciones hayan sido exitosas
                            if (result.OperacionExitosa)
                            {
                                //Instanciando Factura Concepto
                                result = new RetornoOperacion(this._id_detalle_facturado);

                                //Completando Transaccion
                                scope.Complete();
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No existe la Factura");
                    }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El detalle se encuentra en una Factura Global, Imposible su Eliminación");

            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Conceptos de las Facturas
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaFacturaConceptos(int id_factura, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Conceptos
                using (DataTable mitConceptos = CargaFacturadoConceptosParaDeshabilitacion(id_factura))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mitConceptos))
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Recorremos los conceptos
                            foreach (DataRow r in mitConceptos.Rows)
                            {
                                //Validamos Origen de Datos
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciamos Facturado Concepto
                                    using (FacturadoConcepto objFacturadoConcepto = new FacturadoConcepto(r.Field<int>("Id")))
                                    {
                                        //Deshabilitamos Concepto
                                        resultado = objFacturadoConcepto.DeshabilitaFacturaConcepto(id_usuario);
                                    }
                                }
                                else
                                    //Salimos del ciclo
                                    break;
                            }

                            //Si las Operaciones fueron exitosas
                            if (resultado.OperacionExitosa)

                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                }
            

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Conceptos de las Facturas
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFacturaConcepto()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_detalle_facturado);
        }
        /// <summary>
        /// Método Público encargado de Obtener los Conceptos de una Factura
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <returns></returns>
        public static DataTable ObtieneConceptosFactura(int id_factura)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando arreglo de parametros
            object[] param = { 4, 0, id_factura, 0, 0, "", 0, 0, 0, 0, 0, 0, 0,0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Valores de la Factura
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <returns></returns>
        public static DataTable ObtieneValoresTotalesFactura(int id_factura)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando arreglo de parametros
            object[] param = { 5, 0, id_factura, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
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
        /// Método Público encargado de los Conceptos de la Factura
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <returns></returns>
        public static DataTable CargaFacturadoConceptosParaDeshabilitacion(int id_factura)
        {   
            //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando arreglo de parametros
            object[] param = { 6, 0, id_factura, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
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
        /// Método Privado encargado de Cargar las Facturas Disponibles y las que estan Ligadas a una Factura Global
        /// </summary>
        /// <param name="id_factura_global">Referencia a la Factura Global Actual</param>
        /// <param name="id_cliente">Cliente al que pertenecen las Facturas</param>
        /// <param name="id_compania">Compania Emisora de las Facturas</param>
        /// <param name="referencia_viaje">Referencia del Servicio (Encabezado-Referencias)</param>
        /// <returns></returns>
        public static DataSet CargaFacturasTodas(int id_factura_global, int id_cliente, int id_compania, string referencia_viaje)
        {
            //Armando arreglo de parametros
            object[] param = { 7, id_factura_global, id_cliente, 0, 0, "", id_compania, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, referencia_viaje, "" };
            
            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
               
                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Método Público encargado de Cargar los Conceptos Disponibles de la Factura
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <param name="id_cliente">Cliente al que pertenece la Factura</param>
        /// <returns></returns>
        public static DataTable CargaConceptosDisponibleFactura(int id_factura, int id_cliente)
        {   
            //Declarando Objeto de Retorno
            DataTable dt = null;
            
            //Armando arreglo de parametros
            object[] param = { 8, id_cliente, id_factura, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            
            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Público encargado de Cargar los Conceptos Ligados de la Factura
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <param name="id_factura">Factura</param>
        /// <param name="id_cliente">Cliente al que pertenece la Factura</param>
        /// <returns></returns>
        public static DataTable CargaConceptosLigadosFactura(int id_factura_global, int id_factura, int id_cliente)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando arreglo de parametros
            object[] param = { 9, id_cliente, id_factura, 0, 0, "", id_factura_global, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método encargado de Cargar los Detalles de la Factura de Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable CargaDetallesFacturaServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando arreglo de parametros
            object[] param = { 10, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, id_servicio.ToString(), "" };

            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método encargado de Cargar los Detalles de la Factura de Servicio
        /// con regularizacion de Cart Porte-Traslado 16/12/2015
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        public static DataTable CargaDetallesFacturaServicioR16122015(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando arreglo de parametros
            object[] param = { 12, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, id_servicio.ToString(), "" };

            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
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
