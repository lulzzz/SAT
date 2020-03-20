using SAT_CL.Documentacion;
using SAT_CL.Tarifas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using FEv32 = SAT_CL.FacturacionElectronica;
using FEv33 = SAT_CL.FacturacionElectronica33;
using System.Linq;

namespace SAT_CL.Facturacion
{
    /// <summary>
    /// Clase encargada de todas las Operaciones de la Tabla "Facturado"
    /// </summary>
    public class Facturado : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeracion para el Estatus de la Factura
        /// </summary>
        public enum EstatusFactura
        {   /// <summary>
            /// Estatus que indica que la Factura esta Registrada
            /// </summary>
            Registrada = 1,
            /// <summary>
            /// Estatus que indica que la Factura esta Aplicada Parcial
            /// </summary>
            AplicadaParcial,
            /// <summary>
            /// Estatus que indica que la Factura esta Liquidada
            /// </summary>
            Liquidada,
            /// <summary>
            /// Estatus que indica que la Factura esta Cancelada
            /// </summary>
            Cancelada,
            /// <summary>
            /// Estatus que indica que la Factura no es Facturable para ningún proceso
            /// </summary>
            NoFacturable
        }

        #endregion
        
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "facturacion.sp_facturado_tf";

        private int _id_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Factura
        /// </summary>
        public int id_factura { get { return this._id_factura; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private string _no_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Número de Factura
        /// </summary>
        public string no_factura { get { return this._no_factura; } }
        private DateTime _fecha_factura;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Factura
        /// </summary>
        public DateTime fecha_factura { get { return this._fecha_factura; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de almacenar el Estatus (Enumeración)
        /// </summary>
        public EstatusFactura estatus { get { return (EstatusFactura)this._id_estatus; } }
        private int _id_causa_falta_pago;
        /// <summary>
        /// Atributo encargado de almacenar la Causa de Falta de Pago
        /// </summary>
        public int id_causa_falta_pago { get { return this._id_causa_falta_pago; } }
        private int _id_tarifa_cobro;
        /// <summary>
        /// Atributo encargado de almacenar la Tarifa de Cobro
        /// </summary>
        public int id_tarifa_cobro { get { return this._id_tarifa_cobro; } }
        private decimal _total_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Total de la Factura
        /// </summary>
        public decimal total_factura { get { return this._total_factura; } }
        private decimal _subtotal_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Sub-Total de la Factura
        /// </summary>
        public decimal subtotal_factura { get { return this._subtotal_factura; } }
        private decimal _trasladado_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Trasladado de la Factura
        /// </summary>
        public decimal trasladado_factura { get { return this._trasladado_factura; } }
        private decimal _retenido_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Retenido de la Factura
        /// </summary>
        public decimal retenido_factura { get { return this._retenido_factura; } }
        private int _moneda;
        /// <summary>
        /// Atributo encargado de almacenar la  Moneda
        /// </summary>
        public int moneda { get { return this._moneda; } }
        private DateTime _fecha_tipo_cambio;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Tipo de Cambio
        /// </summary>
        public DateTime fecha_tipo_cambio { get { return this._fecha_tipo_cambio; } }
        private decimal _total_factura_pesos;
        /// <summary>
        /// Atributo encargado de almacenar el Total de la Factura en Pesos
        /// </summary>
        public decimal total_factura_pesos { get { return this._total_factura_pesos; } }
        private decimal _subtotal_pesos;
        /// <summary>
        /// Atributo encargado de almacenar el Sub-Total en Pesos
        /// </summary>
        public decimal subtotal_pesos { get { return this._subtotal_pesos; } }
        private decimal _trasladado_pesos;
        /// <summary>
        /// Atributo encargado de almacenar el Trasladado en Pesos
        /// </summary>
        public decimal trasladado_pesos { get { return this._trasladado_pesos; } }
        private decimal _retenido_pesos;
        /// <summary>
        /// Atributo encargado de almacenar el Retenido en Pesos
        /// </summary>
        public decimal retenido_pesos { get { return this._retenido_pesos; } }
        private int _id_condicion_pago;
        /// <summary>
        /// Atributo encargado de almacenar la Condición de pago
        /// </summary>
        public int id_condicion_pago { get { return this._id_condicion_pago; } }
        private int _dias_credito;
        /// <summary>
        /// Atributo encargado de almacenar los Dias de Credito
        /// </summary>
        public int dias_credito { get { return this._dias_credito; } }
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
        public Facturado()
        {   //Invocando Método de Cargado
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public Facturado(int id_registro)
        {   //Invocando Método de Cargado
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Facturado()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   
            //Asignando Valores
            this._id_factura = 0;
            this._no_factura = "";
            this._id_servicio = 0;
            this._fecha_factura = DateTime.MinValue;
            this._id_estatus = 0;
            this._id_causa_falta_pago = 0;
            this._id_tarifa_cobro = 0;
            this._total_factura = 0;
            this._subtotal_factura = 0;
            this._trasladado_factura = 0;
            this._retenido_factura = 0;
            this._moneda = 0;
            this._fecha_tipo_cambio = DateTime.MinValue;
            this._total_factura_pesos = 0;
            this._subtotal_pesos = 0;
            this._trasladado_pesos = 0;
            this._retenido_pesos = 0;
            this._id_condicion_pago = 0;
            this._dias_credito = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   
            //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de Parametros
            object[] param = { 3, id_registro, 0, "", null, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   
                    //Recorriendo cada uno de los Registros
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Asignando Valores
                        this._id_factura = id_registro;
                        this._no_factura = dr["NoFactura"].ToString();
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        DateTime.TryParse(dr["FechaFactura"].ToString(), out this._fecha_factura);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_causa_falta_pago = Convert.ToInt32(dr["IdCausaFaltaPago"]); ;
                        this._id_tarifa_cobro = Convert.ToInt32(dr["IdTarifaCobro"]); ;
                        this._total_factura = Convert.ToDecimal(dr["TotalFactura"]);
                        this._subtotal_factura = Convert.ToDecimal(dr["SubtotalFactura"]);
                        this._trasladado_factura = Convert.ToDecimal(dr["TrasladadoFactura"]);
                        this._retenido_factura = Convert.ToDecimal(dr["RetenidoFactura"]);
                        this._moneda = Convert.ToInt32(dr["Moneda"]);
                        DateTime.TryParse(dr["FechaTipoCambio"].ToString(), out this._fecha_tipo_cambio);
                        this._total_factura_pesos = Convert.ToDecimal(dr["TotalFacturaPesos"]);
                        this._subtotal_pesos = Convert.ToDecimal(dr["SubtotalPesos"]);
                        this._trasladado_pesos = Convert.ToDecimal(dr["TrasladadoPesos"]);
                        this._retenido_pesos = Convert.ToDecimal(dr["RetenidoPesos"]);
                        this._id_condicion_pago = Convert.ToInt32(dr["IdCondicionPago"]); ;
                        this._dias_credito = Convert.ToInt32(dr["DiasCredito"]); ;
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
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="no_factura">No. de Factura</param>
        /// <param name="fecha_factura">Fecha de Factura</param>
        /// <param name="id_estatus">Id de Estatus</param>
        /// <param name="id_causa_falta_pago">Causa de Falta de Pago</param>
        /// <param name="id_tarifa_cobro">Tarifa de Cobro</param>
        /// <param name="total_factura">Total de Factura</param>
        /// <param name="subtotal_factura">Subtotal de Factura</param>
        /// <param name="trasladado_factura">Traslado de Factura</param>
        /// <param name="retenido_factura">Retenido de Factura</param>
        /// <param name="moneda">Moneda de la Factura</param>
        /// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
        /// <param name="total_factura_pesos">Total de Factura en Pesos</param>
        /// <param name="subtotal_pesos">Subtotal en Pesos</param>
        /// <param name="trasladado_pesos">Trasladado en Pesos</param>
        /// <param name="retenido_pesos">Retenido en Pesos</param>
        /// <param name="id_condicion_pago">Condición de Pago</param>
        /// <param name="dias_credito">Dias de Credito</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_servicio, string no_factura, DateTime fecha_factura, byte id_estatus, 
                                                    int id_causa_falta_pago, int id_tarifa_cobro, decimal total_factura, 
                                                    decimal subtotal_factura, decimal trasladado_factura, decimal retenido_factura, 
                                                    int moneda, DateTime fecha_tipo_cambio, decimal total_factura_pesos, 
                                                    decimal subtotal_pesos, decimal trasladado_pesos, decimal retenido_pesos, 
                                                    int id_condicion_pago, int dias_credito, int id_usuario, bool habilitar)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 2, this._id_factura, id_servicio, no_factura, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_factura), id_estatus, id_causa_falta_pago, 
                                 id_tarifa_cobro, total_factura, subtotal_factura, trasladado_factura, retenido_factura, moneda, 
                                 TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_tipo_cambio), total_factura_pesos, subtotal_pesos, trasladado_pesos, retenido_pesos, 
                                 id_condicion_pago, dias_credito, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Facturas
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="no_factura">No. de Factura</param>
        /// <param name="fecha_factura">Fecha de Factura</param>
        /// <param name="id_causa_falta_pago">Causa de Falta de Pago</param>
        /// <param name="id_tarifa_cobro">Tarifa de Cobro</param>
        /// <param name="total_factura">Total de Factura</param>
        /// <param name="subtotal_factura">Subtotal de Factura</param>
        /// <param name="trasladado_factura">Traslado de Factura</param>
        /// <param name="retenido_factura">Retenido de Factura</param>
        /// <param name="moneda">Moneda de la Factura</param>
        /// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
        /// <param name="id_condicion_pago">Condición de Pago</param>
        /// <param name="dias_credito">Dias de Credito</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFactura(int id_servicio, string no_factura, DateTime fecha_factura,
                                                    int id_causa_falta_pago, int id_tarifa_cobro, decimal total_factura, 
                                                    decimal subtotal_factura, decimal trasladado_factura, decimal retenido_factura, 
                                                    int moneda, DateTime fecha_tipo_cambio, int id_condicion_pago, int dias_credito, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            decimal valor_tc = 0.00M;
            
            //Validando que la Moneda sea Distinto a MXN
            if (moneda != 1)
            {
                //Instanciando Servicio
                using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(id_servicio))
                {
                    //Instanciando Tipo de Cambio
                    using (Bancos.TipoCambio tc = new Bancos.TipoCambio(ser.id_compania_emisor, (byte)moneda, fecha_factura, 0))
                    {
                        //Validando que exista el Tipo de Cambio
                        if (tc.id_tipo_cambio > 0)
                        {
                            //Instanciando Valor
                            result = new RetornoOperacion(tc.id_tipo_cambio);

                            //Asignando Valor del Tipo de Cambio
                            valor_tc = tc.valor_tipo_cambio;
                        }
                        else
                        {   
                            //Obteniendo Resultado del SP
                            result = new RetornoOperacion("No existe el Tipo de Cambio");

                            //Asignando Valor
                            valor_tc = 1;
                        }
                    }
                }
            }
            else
            {
                //Instanciando Valor
                result = new RetornoOperacion(1);

                //Asignando Valor del Tipo de Cambio
                valor_tc = 1;
            }

            //Validando la Operación
            if (result.OperacionExitosa)
            {
                //Armando arreglo de Parametros
                object[] param = { 1, 0, id_servicio, no_factura, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_factura), EstatusFactura.Registrada, id_causa_falta_pago, 
                                         id_tarifa_cobro, total_factura, subtotal_factura, trasladado_factura, retenido_factura, moneda, 
                                         TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_tipo_cambio), total_factura * valor_tc, 
                                         subtotal_factura * valor_tc, trasladado_factura * valor_tc, 
                                         retenido_factura * valor_tc, id_condicion_pago, dias_credito, id_usuario, true, "", "" };

                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Público encargado de Insertar las Facturas en Ceros
        /// </summary>
        /// <param name="FecFac">Fecha de la Factura</param>
        /// <param name="FecTC">Fecha del Tipo de Cambio</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_moneda">Moneda de la Operación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaNotaCreditoCxC(DateTime FecFac, DateTime FecTC, int id_compania, int id_moneda, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            decimal valor_tc = 0.00M;

            //Validando que la Moneda sea Distinto a MXN
            if (id_moneda != 1)
            {
                //Instanciando Tipo de Cambio
                using (Bancos.TipoCambio tc = new Bancos.TipoCambio(id_compania, (byte)id_moneda, FecTC, 0))
                {
                    //Validando que exista el Tipo de Cambio
                    if (tc.id_tipo_cambio > 0)
                    {
                        //Instanciando Valor
                        result = new RetornoOperacion(tc.id_tipo_cambio);

                        //Asignando Valor del Tipo de Cambio
                        valor_tc = tc.valor_tipo_cambio;
                    }
                    else
                    {
                        //Obteniendo Resultado del SP
                        result = new RetornoOperacion("No existe el Tipo de Cambio");

                        //Asignando Valor
                        valor_tc = 0;
                    }
                }
            }
            else
            {
                //Instanciando Valor
                result = new RetornoOperacion(1);

                //Asignando Valor del Tipo de Cambio
                valor_tc = 1;
            }

            //Validando Operación
            if (result.OperacionExitosa)
            {
                //Armando arreglo de Parametros
                object[] param = { 1, 0, 0, "0", TSDK.Base.Fecha.ConvierteDateTimeObjeto(FecFac), (byte)EstatusFactura.Registrada, 0, 
                                0, 0, 0, 0, 0, id_moneda, TSDK.Base.Fecha.ConvierteDateTimeObjeto(FecTC), 0, 0, 0,
                                0, 2, 30, id_usuario, true, "","" };
                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Insertar las Facturas en Ceros
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="FecFac">Fecha de Factura</param>
        /// <param name="id_tarifa_cobro">Tarifa de Cobro</param>
        /// <param name="id_moneda">Moneda a Utilizar</param>
        /// <param name="id_condicion_pago">Condición de Pago</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFactura(int id_servicio, DateTime FecFac, DateTime FecTC, int id_tarifa_cobro, int id_moneda, 
                                                      int id_condicion_pago, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            decimal valor_tc = 0.00M;

            //Validando que la Moneda sea Distinto a MXN
            if (id_moneda != 1)
            {
                //Instanciando Servicio
                using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(id_servicio))
                {
                    //Instanciando Tipo de Cambio
                    using (Bancos.TipoCambio tc = new Bancos.TipoCambio(ser.id_compania_emisor, (byte)id_moneda, FecTC, 0))
                    {
                        //Validando que exista el Tipo de Cambio
                        if (tc.id_tipo_cambio > 0)
                        {
                            //Instanciando Valor
                            result = new RetornoOperacion(tc.id_tipo_cambio);

                            //Asignando Valor del Tipo de Cambio
                            valor_tc = tc.valor_tipo_cambio;
                        }
                        else
                        {
                            //Obteniendo Resultado del SP
                            result = new RetornoOperacion("No existe el Tipo de Cambio");

                            //Asignando Valor
                            valor_tc = 0;
                        }
                    }
                }
            }
            else
            {
                //Instanciando Valor
                result = new RetornoOperacion(1);

                //Asignando Valor del Tipo de Cambio
                valor_tc = 1;
            }

            //Validando Operación
            if (result.OperacionExitosa)
            {
                //Armando arreglo de Parametros
                object[] param = { 1, 0, id_servicio, "0", TSDK.Base.Fecha.ConvierteDateTimeObjeto(FecFac), (byte)EstatusFactura.Registrada, 0, 
                                id_tarifa_cobro, 0, 0, 0, 0, id_moneda, TSDK.Base.Fecha.ConvierteDateTimeObjeto(FecTC), 0, 0, 0,
                                0, id_condicion_pago, 30, id_usuario, true, "","" };
                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Insertar las Facturas por Defecto
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFactura(int id_servicio, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 1, 0, id_servicio, "0", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), (byte)EstatusFactura.Registrada, 0, 
                                0, 0, 0, 0, 0, 1, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0,
                                0, 1, 30, id_usuario, true, "","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Facturas
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="no_factura">No. de Factura</param>
        /// <param name="fecha_factura">Fecha de Factura</param>
        /// <param name="id_estatus">Id de Estatus</param>
        /// <param name="id_causa_falta_pago">Causa de Falta de Pago</param>
        /// <param name="id_tarifa_cobro">Tarifa de Cobro</param>
        /// <param name="total_factura">Total de Factura</param>
        /// <param name="subtotal_factura">Subtotal de Factura</param>
        /// <param name="trasladado_factura">Traslado de Factura</param>
        /// <param name="retenido_factura">Retenido de Factura</param>
        /// <param name="moneda">Moneda de la Factura</param>
        /// <param name="fecha_tipo_cambio">Fecha del Tipo de Cambio</param>
        /// <param name="total_factura_pesos">Total de Factura en Pesos</param>
        /// <param name="subtotal_pesos">Subtotal en Pesos</param>
        /// <param name="trasladado_pesos">Trasladado en Pesos</param>
        /// <param name="retenido_pesos">Retenido en Pesos</param>
        /// <param name="id_condicion_pago">Condición de Pago</param>
        /// <param name="dias_credito">Dias de Credito</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaFactura(int id_servicio, string no_factura, DateTime fecha_factura, byte id_estatus, 
                                             int id_causa_falta_pago, int id_tarifa_cobro, decimal total_factura, 
                                             decimal subtotal_factura, decimal trasladado_factura, decimal retenido_factura, 
                                             int moneda, DateTime fecha_tipo_cambio, decimal total_factura_pesos, 
                                             decimal subtotal_pesos, decimal trasladado_pesos, decimal retenido_pesos, 
                                             int id_condicion_pago, int dias_credito, int id_usuario)
        {   
            //Invocando Método de Actualización
            return this.actualizaRegistros(id_servicio, no_factura, fecha_factura, id_estatus, id_causa_falta_pago,
                                 id_tarifa_cobro, total_factura, subtotal_factura, trasladado_factura, retenido_factura, moneda,
                                 fecha_tipo_cambio, total_factura_pesos, subtotal_pesos, trasladado_pesos, retenido_pesos,
                                 id_condicion_pago, dias_credito, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar el Servicio del Facturado
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaServicio(int id_servicio, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistros(id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago,
                                 this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura, this._moneda,
                                 this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._trasladado_pesos, this._retenido_pesos,
                                 this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Editar los Valores Totales de las Facturas
        /// </summary>
        /// <param name="total_factura">Total de Factura</param>
        /// <param name="subtotal_factura">Subtotal de Factura</param>
        /// <param name="trasladado_factura">Trasladado de Factura</param>
        /// <param name="retenido_factura">Retenido de Factura</param>
        /// <param name="total_fac_pesos">Total de Factura en Pesos</param>
        /// <param name="subtotal_fac_pesos">Subtotal de Factura en Pesos</param>
        /// <param name="trasladado_fac_pesos">Trasladado de Factura en Pesos</param>
        /// <param name="retenido_fac_pesos">Retenido de Factura en Pesos</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaFactura(decimal total_factura, decimal subtotal_factura, decimal trasladado_factura,
                                        decimal retenido_factura, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion("El estatus de la Factura no permite su Edición");
            
            //Validamos que la Factura se encuentre en Estatus Registrado
            if ((EstatusFactura)this._id_estatus == EstatusFactura.Registrada)
            {
                //Instanciamos Relación de Facturación Electrónica
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(this._id_factura))))
                {
                    //Validamos que existan Relación
                    if (objFacturaFacturacion.id_facturado_facturacion <= 0)
                    {
                        //Validando que la Moneda sea MXN
                        if (this._moneda == 1)
                        {
                            //Invocando Método de Actualización
                            result = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago,
                                                        this._id_tarifa_cobro, total_factura, subtotal_factura, trasladado_factura, retenido_factura,
                                                        this._moneda, this._fecha_tipo_cambio, total_factura, subtotal_factura, trasladado_factura, retenido_factura,
                                                        this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);
                        }
                        else
                        {
                            //Instanciando Servicio
                            using (Documentacion.Servicio serv = new Documentacion.Servicio(this._id_servicio))
                            {
                                //Instanciando Facturación Otros
                                using (FacturacionOtros fo = FacturacionOtros.ObtieneInstanciaFactura(this._id_factura))
                                {
                                    //Instanciando Tipo de Cambio
                                    using (Bancos.TipoCambio tc = new Bancos.TipoCambio(serv.id_compania_emisor == 0 ? fo.id_compania_emisora : serv.id_compania_emisor, (byte)this._moneda, this._fecha_tipo_cambio, 0))
                                    {
                                        //Validando que existe el Tipo de Cambio
                                        if (tc.id_tipo_cambio > 0)
                                        {
                                            //Invocando Método de Actualización
                                            result = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago,
                                                                        this._id_tarifa_cobro, total_factura, subtotal_factura, trasladado_factura, retenido_factura,
                                                                        this._moneda, this._fecha_tipo_cambio, total_factura * tc.valor_tipo_cambio, subtotal_factura * tc.valor_tipo_cambio,
                                                                        trasladado_factura * tc.valor_tipo_cambio, retenido_factura * tc.valor_tipo_cambio,
                                                                        this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);
                                        }
                                        else
                                        {
                                            //Validando que la Moneda sea MXN
                                            if (this._moneda == 1)
                                            {
                                                //Invocando Método de Actualización
                                                result = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago,
                                                                            this._id_tarifa_cobro, total_factura, subtotal_factura, trasladado_factura, retenido_factura,
                                                                            this._moneda, this._fecha_tipo_cambio, total_factura, subtotal_factura, trasladado_factura, retenido_factura,
                                                                            this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("No Existe el Tipo de Cambio");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        //Establecemos Mensaje Error
                        result = new RetornoOperacion("Imposible editar la Factura ya que esta ligada a una Factura Electrónica.");
                    }
                }
            }
            
            //Devovliendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Valores del Encabezado de Factura
        /// </summary>
        /// <param name="fecha_fac">Fecha de la Factura</param>
        /// <param name="id_tarifa_cobro">Tarifa de Cobro</param>
        /// <param name="moneda">Moneda de la Factura</param>
        /// <param name="id_condicion_pago">Condición de Pago</param>
        /// <param name="id_usuario">id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaFactura(DateTime fecha_fac, DateTime fecha_tc, int id_tarifa_cobro, int moneda, int id_condicion_pago, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion("El estatus de la Factura no permite su edición");
            decimal valor_tc = 0;

            //Validamos que la Factura se encuentre en Estatus Registrado
            if ((EstatusFactura)this._id_estatus == EstatusFactura.Registrada)
            {
                 //Instanciamos Relación de Facturación Electrónica
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(this._id_factura))))
                {
                    //Validamos que existan Relación
                    if (objFacturaFacturacion.id_facturado_facturacion <= 0)
                    {
                        //Instanciando Servicio
                        using (Documentacion.Servicio serv = new Documentacion.Servicio(this._id_servicio))
                        {
                            //Instanciando Facturación Otros
                            using (FacturacionOtros fo = FacturacionOtros.ObtieneInstanciaFactura(this._id_factura))
                            {
                                //Validando que la Moneda
                                if (moneda != 1)
                                {
                                    //Instanciando Tipo de Cambio
                                    using (Bancos.TipoCambio tc = new Bancos.TipoCambio(serv.id_compania_emisor == 0 ? fo.id_compania_emisora : serv.id_compania_emisor, (byte)moneda, fecha_tc, 0))
                                    {
                                        //Validando que existe el Tipo de Cambio
                                        if (tc.id_tipo_cambio > 0)
                                        {
                                            //Asignando Positivo el Retorno
                                            result = new RetornoOperacion(1);

                                            //Asignando Valor de Tipo de Cambio
                                            valor_tc = tc.valor_tipo_cambio;
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No Existe el Tipo de Cambio");
                                    }
                                }
                                else
                                {
                                    //Asignando Positivo el Retorno
                                    result = new RetornoOperacion(1);

                                    //Valor a 1
                                    valor_tc = 1;
                                }
                            }

                            //Validando que fuese correcta la validación
                            if (result.OperacionExitosa)
                            {
                                //Inicializando Bloque Transaccional
                                using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Invocando Método de Actualización
                                    result = this.actualizaRegistros(this._id_servicio, this._no_factura, fecha_fac, this._id_estatus, this._id_causa_falta_pago,
                                                                    id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura,
                                                                    moneda, fecha_tc, this._total_factura * valor_tc, this._subtotal_factura * valor_tc,
                                                                    this._trasladado_factura * valor_tc, this._retenido_factura * valor_tc,
                                                                    id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Cargamos Conceptos
                                        using (DataTable mitConceptos = FacturadoConcepto.ObtieneConceptosFactura(this._id_factura))
                                        {
                                            //Validamos Origen de Datos
                                            if (Validacion.ValidaOrigenDatos(mitConceptos))
                                            {
                                                //Recorremos los conceptos
                                                foreach (DataRow dr in mitConceptos.Rows)
                                                {
                                                    //Instanciamos Facturado Concepto
                                                    using (FacturadoConcepto objFacturadoConcepto = new FacturadoConcepto(dr.Field<int>("Id")))
                                                    {
                                                        //Validando que exista el Concepto
                                                        if (objFacturadoConcepto.habilitar)
                                                        {
                                                            //Actualizando Importe en Pesos
                                                            result = objFacturadoConcepto.ActualizaImportePesos(valor_tc, id_usuario);

                                                            //Validando Operación Exitosa
                                                            if (!result.OperacionExitosa)

                                                                //Terminando Ciclo
                                                                break;
                                                        }
                                                        else
                                                            //Terminando Ciclo
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                                //Instanciando Factura
                                                result = new RetornoOperacion(this._id_factura);
                                        }

                                        //Validando Operación Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Completando Transacción
                                            trans.Complete();

                                            //Instanciando Factura
                                            result = new RetornoOperacion(this._id_factura);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Establecemos Mensaje Error
                        result = new RetornoOperacion("Imposible editar la Factura ya que esta ligada a una Factura Electrónica.");
                    }
                }
            }

            //Invocando Método de Actualización
            return result;
        }
        /// <summary>
        /// Método encargado de cancelar la Factura
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CancelaFactura(int id_usuario)
        {  
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Validamos Estatus de la Factura
            if (estatus == EstatusFactura.Registrada || estatus== EstatusFactura.NoFacturable)
            {
                //Instanciando Comprabante
                using (FacturacionElectronica.Comprobante cmp = new FEv32.Comprobante(FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(this._id_factura)))
                {
                    //Validando que exista el Comprobante
                    if (cmp.habilitar)
                    {
                        //Validando que exista una Factura Cancelada
                        if (cmp.estatus_comprobante == FEv32.Comprobante.EstatusComprobante.Cancelado)
                        {
                            //Invocando Método de Actualización
                            resultado = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, (byte)EstatusFactura.Cancelada, this._id_causa_falta_pago,
                                                        this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura,
                                                        this._moneda, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos,
                                                        this._trasladado_pesos, this._retenido_pesos, this._id_condicion_pago,
                                                        this._dias_credito, id_usuario, this._habilitar);
                        }
                        else
                        {
                            resultado = new RetornoOperacion(string.Format("No se ha Cancelado el CFDI."));
                        }

                    }
                    else
                        //Invocando Método de Actualización
                        resultado = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, (byte)EstatusFactura.Cancelada, this._id_causa_falta_pago,
                                                    this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura,
                                                    this._moneda, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos,
                                                    this._trasladado_pesos, this._retenido_pesos, this._id_condicion_pago,
                                                    this._dias_credito, id_usuario, this._habilitar);
                }
            }
            else
                resultado = new RetornoOperacion(string.Format("El estatus del cobro del servicio es '{0}'.", this.estatus));

            //Devolvemos resultado
            return resultado;

        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus de la Factura
        /// </summary>
        /// <param name="estatus">Estatus por Actualizar</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusFactura(EstatusFactura estatus, int id_usuario)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Invocando Método de Actualización
            resultado = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, (byte)estatus, this._id_causa_falta_pago,
                                        this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura,
                                        this._moneda, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos,
                                        this._trasladado_pesos, this._retenido_pesos, this._id_condicion_pago,
                                        this._dias_credito, id_usuario, this._habilitar);

            //Devolvemos resultado
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Factura
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFactura(int id_usuario)
        {   
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Estatus de la Factura
                if ((EstatusFactura)this.id_estatus == EstatusFactura.Registrada)
                {
                    //Validando si existe una Factura Global
                    if (ValidaFacturaGlobal().OperacionExitosa)
                    {
                        //Instanciando Comprabante
                        using (FacturacionElectronica.Comprobante cmp = new FEv32.Comprobante(FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(this._id_factura)))
                        {
                            //Validando que exista el Comprobante
                            if (cmp.habilitar)
                            {
                                //Validando Estatus
                                switch (cmp.estatus_comprobante)
                                {
                                    case FEv32.Comprobante.EstatusComprobante.Vigente:
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("El Comprobante se encuentra Vigente, Imposible su Eliminación");
                                        break;
                                    case FEv32.Comprobante.EstatusComprobante.Cancelado:
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("El Comprobante se encuentra Cancelado, Imposible su Eliminación");
                                        break;
                                    default:
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("El Comprobante se encuentra Registrado, Imposible su Eliminación");
                                        break;
                                }
                            }
                            else
                            {
                                //Deshabilitamos Conceptos
                                resultado = FacturadoConcepto.DeshabilitaFacturaConceptos(this._id_factura, id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Invocando Método de Actualización
                                    resultado = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago,
                                                         this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura, this._moneda,
                                                         this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._trasladado_pesos, this._retenido_pesos,
                                                         this._id_condicion_pago, this._dias_credito, id_usuario, false);
                                }
                            }
                        }
                    }
                    else
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("La Factura se encuentra contenida en una Factura Global, Imposible su Eliminación");
                }
                else
                    //Establecemos Mensaje Error
                    resultado = new RetornoOperacion("El estatus de la factura no permite su eliminación.");
                

                //Si las Operaciones fueron exitosas
                if (resultado.OperacionExitosa)

                    //Completando Transacción
                    trans.Complete();
            }
            //Devolvemos Resultado 
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Valor de los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFactura()
        {   
            //Invocando Método de Carga de Atributos
            return this.cargaAtributosInstancia(this._id_factura);
        }
        /// <summary>
        /// Método Público encargado de Obtener la Factura Ligada al Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static Facturado ObtieneFacturaServicio(int id_servicio)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 4, 0, id_servicio, "0", null, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, true, "","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return new Facturado(result.IdRegistro);
        }
        /// <summary>
        /// Asigna el Id de Tarifa de cobro que aplica sobre la factura (actualizando cargos vinculados), determinada a aprtir de la información del servicio a la que pertenece la tarifa
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTarifa(int id_usuario)
        { 
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_factura);

            //Validando estatus de factura
            if ((EstatusFactura)this._id_estatus == EstatusFactura.Registrada)
            {
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(this._id_factura))))
                {
                    //Validamos que existan Relación
                    if (objFacturaFacturacion.id_facturado_facturacion <= 0)
                    {
                        //Si no existen aplicaciones a la Factura
                        if (!SAT_CL.CXC.FichaIngresoAplicacion.ValidaFacturaAplicada(this._id_factura))
                        {
                            //Buscando la tarifa aplicable a la configuración del servicio
                                using (Tarifa tarifa = Tarifa.ObtieneTarifaCobroServicio(this._id_servicio))
                                {
                                    //Si la tarifa fue encontrada
                                    if (tarifa.id_tarifa > 0)
                                    {
                                        //Deshabilitando Conceptos
                                        resultado = FacturadoConcepto.DeshabilitaFacturaConceptos(this._id_factura, id_usuario);

                                        //Inicializando bloque transaccional
                                        using (System.Transactions.TransactionScope transaccion = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {
                                            //Validando que la Operación haya sido Exitosa
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Declarando variable para almacenar valor de tarifa a utilizar
                                                decimal valor_unitario_tarifa = 0, cantidad_tarifa = 0;
                                                int id_tipo_cargo_tarifa = 0, id_tarifa_matriz = 0;

                                                //Instanciando servicio involucrado
                                                using (Documentacion.Servicio servicio = new Documentacion.Servicio(this._id_servicio))
                                                {
                                                    //Determinando si la tarifa aplica mediante búsqueda en matriz o mediante valores directos de encabezado de tarifa
                                                    if (tarifa.filtro_col != Tarifa.CriterioMatrizTarifa.NoAplica || tarifa.filtro_row != Tarifa.CriterioMatrizTarifa.NoAplica)
                                                    {
                                                        //Declarando variables necesarias para almacenar criterios de búsqueda en matríz (valores columna / fila)
                                                        string descipcion_col = "", descipcion_fila = "", operador_col = "=", operador_row = "=";

                                                        //Extrayendo los datos requeridos del servicio, para realizar búsqueda sobre la matriz de la tarifa
                                                        resultado = servicio.ExtraeCriteriosMatrizTarifa(tarifa.filtro_col, tarifa.filtro_row, tarifa.id_base_tarifa, out descipcion_col, out descipcion_fila, out operador_col, out operador_row);

                                                        //Si no hay errores en recuperación de criterios de búsqueda
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Localizando valor en matríz mediante los filtros encontrados
                                                            using (TarifaMatriz detalle_tarifa = TarifaMatriz.ObtieneDetalleMatrizTarifa(tarifa.id_tarifa, descipcion_col, descipcion_fila, operador_col, operador_row))
                                                            {
                                                                //Si fue encontrado un elemento en la matríz de tarifa
                                                                if (detalle_tarifa.id_tarifa_matriz > 0)
                                                                {
                                                                    //Conservando Id de Detalle de matriz, para carga posterior de cargos recurrentes
                                                                    id_tarifa_matriz = detalle_tarifa.id_tarifa_matriz;
                                                                    //Recuperando valor de matriz acorde a estatus de carga (cargado/vacío)
                                                                    valor_unitario_tarifa = detalle_tarifa.tarifa_cargado;
                                                                }
                                                                //Si no hay coincidencia en la matríz
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("La tarifa '{0}' no poseé una coincidencia '{1}' - '{2}' para este servicio.", tarifa.descripcion, tarifa.filtro_row.ToString(), tarifa.filtro_col.ToString())); ;
                                                            }
                                                        }
                                                    }
                                                    //Si no hay matriz configurada
                                                    else
                                                        //Se considera el valor general de encabezado de tarifa
                                                        valor_unitario_tarifa = tarifa.valor_unitario;

                                                    //Si no hay errores de obtención de tarifa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Calculando la cantidad sobre la que aplicará el valor unitario de tarifa
                                                        cantidad_tarifa = servicio.ExtraeCriterioBaseTarifa(tarifa.id_base_tarifa, out id_tipo_cargo_tarifa);

                                                        //Actualizando el Id de Tarifa en la Factura
                                                        resultado = actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago, tarifa.id_tarifa, this._total_factura, this._subtotal_factura,
                                                                                    this._trasladado_factura, this._retenido_factura, this._moneda, this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._trasladado_pesos,
                                                                                    this._retenido_pesos, this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);

                                                        //Si no hay error al actualizar tarifa aplicada
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Instanciando tipo de cargo principal
                                                            using (TipoCargo tc = new TipoCargo(id_tipo_cargo_tarifa))
                                                            {
                                                                //Validando existencia de tipo de cargo
                                                                if (tc.id_tipo_cargo > 0)
                                                                {
                                                                    //Insertando cargo principal de de tarifa
                                                                    resultado = FacturadoConcepto.InsertaFacturaConcepto(this._id_factura, 1, tc.id_unidad, "", tc.id_tipo_cargo, cantidad_tarifa * valor_unitario_tarifa, 2, tc.tasa_impuesto_retenido, 3, tc.tasa_impuesto_trasladado, 0, id_usuario);

                                                                    //Si no hay errores
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Buscando cargos recurrentes por añadir (ligados a la tarifa en general y al elemento de tarifa correspondiente)
                                                                        using (DataTable cargos_recurrentes = CargoRecurrente.ObtieneCargosRecurrentes(tarifa.id_tarifa, id_tarifa_matriz))
                                                                        {
                                                                            //Validando existencia de cargos recurrentes
                                                                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(cargos_recurrentes))
                                                                            {
                                                                                //Para cada cargo encontrado
                                                                                foreach (DataRow cr in cargos_recurrentes.Rows)
                                                                                {
                                                                                    //Instanciando cargo recurrente
                                                                                    using (TipoCargo c = new TipoCargo(Convert.ToInt32(cr["IdTipoCargo"])))
                                                                                    {
                                                                                        //Si el cargo fue encontrado
                                                                                        if (c.id_tipo_cargo > 0)
                                                                                        {
                                                                                            //Realizando inserción de cargo sobre la factura actual
                                                                                            resultado = FacturadoConcepto.InsertaFacturaConcepto(this._id_factura, Convert.ToDecimal(cr["Cantidad"]), c.id_unidad, "", c.id_tipo_cargo, Convert.ToDecimal(cr["ValorU"]),
                                                                                                                                    2, c.tasa_impuesto_retenido, 3, c.tasa_impuesto_trasladado, Convert.ToInt32(cr["Id"]), id_usuario);

                                                                                            //Si hay errores
                                                                                            if (!resultado.OperacionExitosa)
                                                                                            {
                                                                                                resultado = new RetornoOperacion(string.Format("Error al registrar cargo recurrente '{0}'.", c.descripcion));
                                                                                                //Terminando ciclo de inserción
                                                                                                break;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                            resultado = new RetornoOperacion(string.Format("No es posible recuperar el detalle del tipo de cargo Id '{0}'.", cr["IdTipoCargo"]));
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion("Error al registrar el cálculo de tarifa asignada.");

                                                                    //Si no hay errores
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Finalziando transacción con exito
                                                                        transaccion.Complete();
                                                                        //Asignando resultado final
                                                                        resultado = new RetornoOperacion(this._id_factura);
                                                                    }
                                                                }
                                                                //De lo contrario
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("El tipo de cargo base para la tarifa '{0}' no pudo ser encontrado, favor de revisar la configuración.", tarifa.descripcion));
                                                            }
                                                        }
                                                        //Señalando error al actualizar Tarifa
                                                        else
                                                            resultado = new RetornoOperacion("La Tarifa no pudo ser actualizada en la Factura.");

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                        //
                                        resultado = new RetornoOperacion("No hay tarifas coincidentes.");
                                
                                
                            }
                        }
                        //Si ya hay tarifa previa
                        else
                            resultado = new RetornoOperacion("Imposible editar la Factura ya que ha sido aplicada");
                    }
                    else
                    {
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("Imposible editar la Factura ya que esta ligada a una Factura Electrónica.");
                    }
                }
            }
            else
                resultado = new RetornoOperacion("El estatus de la factura no permite actualizar la tarifa.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Validamos si ya exite una Factura Electrónica activa
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidaFacturaElectronica()
        {
            //Declaramos Variable
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos
            if (FacturadoFacturacion.ObtieneRelacionFactura(this._id_factura) > 0)
            {
                //Mostramos Error
                resultado = new RetornoOperacion("Ya existe un registró Facturación Electrónica.");
            }
            //Devolvemos Resultado
            return resultado;
        }

        #region Métodos CFDI 3.2

        /// <summary>
        /// Importa Facturado a Facturación Electrónica
        /// </summary>
        /// <param name="id_forma_pago">Forma de Pago de la Factura</param>
        /// <param name="no_cuenta_pago">Cuenta de Pago</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="tipo_comprobante">Tipo de Comprobante</param>
        /// <param name="id_sucursal">Id de Sucursal</param>
        /// <param name="id_compania">Id Compania Emisor</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="referencias_viaje">Envió de Referencia de Viaje a mostrar</param>
        /// <returns></returns>
        public RetornoOperacion ImportaFacturadoComprobante_V3_2(byte id_forma_pago, int no_cuenta_pago, byte id_metodo_pago, byte tipo_comprobante, int id_sucursal, int id_compania, int id_usuario, string referencias_viaje)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos fecha para el tipo de cambio
            DateTime fecha_tc = DateTime.MinValue;

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos estatus del servicio
                using (Servicio objServicio = new Servicio(this._id_servicio))
                {
                    //Validamos Estatus del Servicio
                    if (objServicio.estatus != Servicio.Estatus.Cancelado)
                    {
                        //Validamos que no exista registró activo
                        resultado = ValidaFacturaElectronica();

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos asignaición a la Factura Global
                            resultado = ValidaFacturaGlobal();

                            //Validamos resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Asignamos valor a la Fecha de Tipo de Cambio
                                fecha_tc = this._fecha_tipo_cambio;

                                //Recalcula Montos de la Factura
                                resultado = ActualizaMontosTipoCambio(id_compania, fecha_tc, (byte)this._moneda, id_usuario);

                                //Validamos Recalculo de Tipo de Cambio
                                if (resultado.OperacionExitosa && ActualizaFacturado())
                                {
                                    //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                                    using (DataSet dsFactura = SAT_CL.Facturacion.Reporte.ObtienesDatosFacturadoFacturaElectronica(this._id_factura, id_forma_pago, no_cuenta_pago, id_metodo_pago, tipo_comprobante, referencias_viaje))
                                    {
                                        //Validamos Registros
                                        if (Validacion.ValidaOrigenDatos(dsFactura))
                                        {
                                            //Importamos Factura a Factura Electrónica
                                            resultado = FEv32.Comprobante.ImportaComprobante_V3_2(dsFactura.Tables["Table"], dsFactura.Tables["Table1"], null, dsFactura.Tables["Table2"], dsFactura.Tables["Table3"], dsFactura.Tables["Table4"], dsFactura.Tables["Table5"], FEv32.Comprobante.OrigenDatos.Facturado, dsFactura.Tables["Table6"], dsFactura.Tables["Table7"], id_compania, id_sucursal, true, fecha_tc, id_usuario);

                                            //Validamos Resultaod de Timbrado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Insertamos Relación de la Factura y Facturación Electrónica
                                                resultado = FacturadoFacturacion.InsertarFacturadoFacturacion(this._id_factura, 0, 0, resultado.IdRegistro, 0, id_usuario);

                                                //Validamos Rsultado
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Devolvemos Id Factura
                                                    resultado = new RetornoOperacion(this._id_factura);

                                                    //Completamos Transacción
                                                    scope.Complete();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            resultado = new RetornoOperacion("No se encontró Información para exportación de la Factura.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                        resultado = new RetornoOperacion("El estatus del Servicio no permite la Importación a Facturación Electrónica.");
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Timbra Facturación Electrónica
        /// </summary>
        /// <param name="serie">Serie para asignar la Factura</param>
        /// <param name="omitir_addenda">Si se desea Ommitir la Addenda</param>
        /// <param name="ruta_xslt_co">URI del archivo de transformación</param>
        /// <param name="ruta_xslt_co_local">URI del archivo de transformación a utilizar si el primero no puede ser utilizado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TimbraFacturadoComprobante_V3_2(string serie, bool omitir_addenda, string ruta_xslt_co,
                                                                 string ruta_xslt_co_local, int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Relación
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(this._id_factura))))
                {
                    //Validamos que existan Relación
                    if (objFacturaFacturacion.id_facturado_facturacion > 0)
                    {
                        //Instanciamos Comprobante
                        using (FEv32.Comprobante objComprobante = new FEv32.Comprobante(objFacturaFacturacion.id_factura_electronica))
                        {
                            //Validamos existencia de comprobante
                            if (objComprobante.id_comprobante > 0)
                            {
                                //Timbramos Fctura
                                resultado = objComprobante.TimbraComprobante(serie, id_usuario, ruta_xslt_co, ruta_xslt_co_local, omitir_addenda);

                                //Validamos Resultaod de Timbrado
                                if (resultado.OperacionExitosa)
                                {
                                    //Validamos Existencia del registro Facturado Facturación
                                    if (objFacturaFacturacion.id_factura > 0)
                                    {
                                        //Editamos Estatus a Timbrado
                                        resultado = objFacturaFacturacion.ActualizaEstatusFacturadoFacturacion(FacturadoFacturacion.Estatus.Facturada, id_usuario);

                                        //Validamos Rsultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizamos atributos del Comprobante
                                            objComprobante.RefrescaAtributosInstancia();
                                            //Devolvemos Id Factura
                                            resultado = new RetornoOperacion(this._id_factura, "La factura ha sido Timbrada " + objComprobante.serie + objComprobante.folio + " .", true);

                                            //Completamos Transacción
                                            scope.Complete();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Establecemos mensaje Resultado
                                resultado = new RetornoOperacion("No se encontró registró Factura Electrónica");
                            }
                        }
                    }
                    else
                    {
                        //Establecemos mensaje Resultado
                        resultado = new RetornoOperacion("No se puede recuperar datos complementarios Facturado Facturación.");
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        #endregion

        #region Métodos CFDI 3.3

        /// <summary>
        /// Importa Facturado a Facturación Electrónica
        /// </summary>
        /// <param name="id_forma_pago">Forma de Pago de la Factura</param>
        /// <param name="id_uso_cfdi">Uso del CFDI</param>
        /// <param name="id_metodo_pago">Método de Pago</param>
        /// <param name="tipo_comprobante">Tipo de Comprobante</param>
        /// <param name="id_sucursal">Id de Sucursal</param>
        /// <param name="id_compania">Id Compania Emisor</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="referencias_viaje">Envió de Referencia de Viaje a mostrar</param>
        /// <returns></returns>
        public RetornoOperacion ImportaFacturadoComprobante_V3_3(int id_forma_pago, int id_uso_cfdi, byte id_metodo_pago, int tipo_comprobante, int id_sucursal, 
                                                int id_compania, int id_usuario, string referencias_viaje, List<Tuple<int, byte, decimal, decimal>> cfdi_relacionados)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();
            int id_cliente = 0;
            //Declaramos fecha para el tipo de cambio
            DateTime fecha_tc = DateTime.MinValue;

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando Existencia del USO
                if (id_uso_cfdi > 0)
                {
                    //Validando Origen de la Facturación
                    if (this._id_servicio == 0)
                    {
                        //Instanciando Facturación Otros
                        using (FacturacionOtros objFacOtros = FacturacionOtros.ObtieneInstanciaFactura(this._id_factura))
                        {
                            //Validando Existencia
                            if (objFacOtros.habilitar)
                            {
                                //Asignando Cliente
                                id_cliente = objFacOtros.id_cliente_receptor;
                                //Instanciando Resultado
                                resultado = new RetornoOperacion(id_cliente);
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No se puede recuperar el Cliente");
                        }
                    }
                    else
                    {
                        //Instanciando Servicio
                        using (Servicio objServicio = new Servicio(this._id_servicio))
                        {
                            //Validando Existencia
                            if (objServicio.habilitar)
                            {
                                //Validamos Estatus del Servicio
                                if (objServicio.estatus != Servicio.Estatus.Cancelado)
                                {
                                    //Asignando Cliente
                                    id_cliente = objServicio.id_cliente_receptor;
                                    //Instanciando Resultado
                                    resultado = new RetornoOperacion(id_cliente);
                                }
                                else
                                    //Instanciando Excepción
                                    resultado = new RetornoOperacion("El estatus del Servicio no permite la Importación a Facturación Electrónica.");
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No se puede recuperar el Cliente del Servicio");
                        }
                    }

                    //Validamos Estatus del Servicio
                    if (resultado.OperacionExitosa)
                    {
                        //Validamos que no exista registró activo
                        resultado = ValidaFacturaElectronica();

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos asignaición a la Factura Global
                            resultado = ValidaFacturaGlobal();

                            //Validamos resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Asignamos valor a la Fecha de Tipo de Cambio
                                fecha_tc = this._fecha_tipo_cambio;

                                //Recalcula Montos de la Factura
                                resultado = ActualizaMontosTipoCambio(id_compania, fecha_tc, (byte)this._moneda, id_usuario);

                                //Validamos Recalculo de Tipo de Cambio
                                if (resultado.OperacionExitosa && ActualizaFacturado())
                                {
                                    //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                                    using (DataSet dsFactura = SAT_CL.FacturacionElectronica33.Reporte.ObtienesDatosFacturadoFacturaElectronica(this._id_factura,
                                                                  id_forma_pago, id_uso_cfdi, id_metodo_pago, tipo_comprobante, "", referencias_viaje))
                                    {
                                        //Validamos Registros
                                        if (Validacion.ValidaOrigenDatos(dsFactura))
                                        {
                                            //Importamos Factura a Factura Electrónica
                                            resultado = FEv33.Comprobante.ImportaComprobante_V3_3(id_cliente, dsFactura.Tables["Table"], dsFactura.Tables["Table1"], null, dsFactura.Tables["Table2"], dsFactura.Tables["Table3"], dsFactura.Tables["Table4"], FEv33.Comprobante.OrigenDatos.Facturado, dsFactura.Tables["Table5"], dsFactura.Tables["Table6"], id_compania, id_sucursal, true, this._fecha_factura, fecha_tc, id_usuario);

                                            //Validamos Resultaod de Timbrado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Obteniendo Comprobante
                                                int idComprobante = resultado.IdRegistro;

                                                //Configurando Relaciones de Comprobantes Cancelados
                                                resultado = FEv33.ComprobanteRelacion.ConfiguraRelacionComprobanteCancelacionFacturado(this._id_factura, idComprobante, id_usuario);

                                                //Validamos Rsultado
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Validando Existencia de relaciones
                                                    if (cfdi_relacionados.Count > 0)
                                                    {
                                                        //Recorriendo Relaciones
                                                        foreach (Tuple<int, byte, decimal, decimal> rel in cfdi_relacionados)
                                                        {
                                                            //Insertando relación
                                                            resultado = FEv33.ComprobanteRelacion.InsertaComprobanteRelacion(idComprobante, rel.Item1, rel.Item2, 0, 0, id_usuario);

                                                            //Validando Operación Erronea
                                                            if (!resultado.OperacionExitosa)

                                                                //Terminando Ciclo
                                                                break;
                                                        }
                                                    }

                                                    //Validamos Rsultado
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Insertamos Relación de la Factura y Facturación Electrónica
                                                        resultado = FacturadoFacturacion.InsertarFacturadoFacturacion(this._id_factura, 0, 0, 0, idComprobante, id_usuario);

                                                        //Validamos Rsultado
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Devolvemos Id Factura
                                                            resultado = new RetornoOperacion(this._id_factura);

                                                            //Completamos Transacción
                                                            scope.Complete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    resultado = new RetornoOperacion("No se puede Recuperar el Uso del CFDI del Cliente.");
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Timbra Facturación Electrónica
        /// </summary>
        /// <param name="serie">Serie para asignar la Factura</param>
        /// <param name="omitir_addenda">Si se desea Ommitir la Addenda</param>
        /// <param name="ruta_xslt_co">URI del archivo de transformación</param>
        /// <param name="ruta_xslt_co_local">URI del archivo de transformación a utilizar si el primero no puede ser utilizado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TimbraFacturadoComprobante_V3_3(string serie, bool omitir_addenda, string ruta_xslt_co,
                                                                 string ruta_xslt_co_local, int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Relación
                using (FacturadoFacturacion objFacturaFacturacion = new FacturadoFacturacion((FacturadoFacturacion.ObtieneRelacionFactura(this._id_factura))))
                {
                    //Validamos que existan Relación
                    if (objFacturaFacturacion.id_facturado_facturacion > 0)
                    {
                        //Instanciamos Comprobante
                        using (FEv33.Comprobante objComprobante = new FEv33.Comprobante(objFacturaFacturacion.id_factura_electronica33))
                        {
                            //Validamos existencia de comprobante
                            if (objComprobante.id_comprobante33 > 0)
                            {
                                //Timbramos Fctura
                                resultado = objComprobante.TimbraComprobante(objComprobante.version, serie, id_usuario, ruta_xslt_co, ruta_xslt_co_local, omitir_addenda);

                                //Validamos Resultaod de Timbrado
                                if (resultado.OperacionExitosa)
                                {
                                    //Validamos Existencia del registro Facturado Facturación
                                    if (objFacturaFacturacion.id_factura > 0)
                                    {
                                        //Editamos Estatus a Timbrado
                                        resultado = objFacturaFacturacion.ActualizaEstatusFacturadoFacturacion(FacturadoFacturacion.Estatus.Facturada, id_usuario);

                                        //Validamos Rsultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizamos atributos del Comprobante
                                            if (objComprobante.ActualizaComprobante())
                                            {
                                                //Devolvemos Id Factura
                                                resultado = new RetornoOperacion(this._id_factura, "La factura ha sido Timbrada " + objComprobante.serie + objComprobante.folio + " .", true);

                                                //Completamos Transacción
                                                scope.Complete();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Establecemos mensaje Resultado
                                resultado = new RetornoOperacion("No se encontró registró Factura Electrónica");
                            }
                        }//*/
                    }
                    else
                    {
                        //Establecemos mensaje Resultado
                        resultado = new RetornoOperacion("No se puede recuperar datos complementarios Facturado Facturación.");
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        #endregion

        /// <summary>
        /// Validamos si ya exite una Factura Global activa
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidaFacturaGlobal()
        {
            //Declaramos Variable
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos
            if (FacturadoFacturacion.ObtieneRelacionFacturaGlobal(this._id_factura)>0)
            {
                //Mostramos Error
                resultado = new RetornoOperacion("La factura ya se encuentrá asignada a una Factura Global");
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Validamos si ya exite una Proceso de Revisión Activo
        /// </summary>
        /// <returns></returns>
        public bool ValidaProcesoRevision()
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando arreglo de parametros
            object[] param = { 6, this._id_factura, 0, "", null, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

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

            //Devolvemos Resultado
            return result;
        }
        /// <summary>
        /// Método encargado de Recalcular los Montos de la Factura conforme al Tipo de Cambio
        /// </summary>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="fecha">Fecha de Tipo de Cambio</param>
        /// <param name="id_moneda">Moneda de la Operación</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaMontosTipoCambio(int id_compania, DateTime fecha, byte id_moneda, int id_usuario)
        {
            //Declarando Variables Auxiliares
            DateTime fecha_tc;
            DateTime.TryParse(fecha.ToString("dd/MM/yyyy"), out fecha_tc);
            
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(this._id_factura);
            
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que la Moneda sea distinta
                if (this._moneda != 1)
                {
                    //Instanciando Tipo de Cambio
                    using (Bancos.TipoCambio tc = new Bancos.TipoCambio(id_compania, id_moneda, fecha_tc, 0))
                    {
                        //Validando que exista el Tipo de Cambio
                        if (tc.id_tipo_cambio > 0)
                        {
                            //Actualizando Totales de la Factura
                            result = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago,
                                         this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura, this._moneda,
                                         fecha_tc, this._total_factura * tc.valor_tipo_cambio, this._subtotal_factura * tc.valor_tipo_cambio,
                                         this._trasladado_factura * tc.valor_tipo_cambio, this._retenido_factura * tc.valor_tipo_cambio,
                                         this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Conceptos
                                using (DataTable dtConceptos = FacturadoConcepto.ObtieneConceptosFactura(this._id_factura))
                                {
                                    //Validando que existan Conceptos
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptos))
                                    {
                                        //Recorriendo Conceptos
                                        foreach (DataRow dr in dtConceptos.Rows)
                                        {
                                            //Instanciando Concepto
                                            using (FacturadoConcepto fc = new FacturadoConcepto(Convert.ToInt32(dr["Id"])))
                                            {
                                                //Validando que exista el Concepto
                                                if (fc.id_detalle_facturado > 0)
                                                {
                                                    //Actualizando Importe en Pesos del Concepto
                                                    result = fc.EditaFacturaConcepto(fc.id_factura, fc.cantidad, fc.id_unidad, fc.identificador, fc.id_concepto_cobro,
                                                                                fc.valor_unitario, (fc.cantidad * fc.valor_unitario) * tc.valor_tipo_cambio, fc.id_impuesto_retenido, fc.tasa_impuesto_retenido,
                                                                               fc.id_impuesto_trasladado, fc.tasa_impuesto_trasladado, fc.id_cargo_recurrente, id_usuario);

                                                    //Validando que la Operación no hubiese sido Exitosa
                                                    if (!result.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }
                                                else
                                                {
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No Existe Concepto");

                                                    //Terminando Ciclo
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No Existen Conceptos para esta Factura");
                                }
                            }
                        }
                        else
                        {
                            //Instanciando Excepción
                            result = new RetornoOperacion("No Existe el Tipo de Cambio");
                        }
                    }
                }
                else
                {
                    //Actualizando Totales de la Factura
                    result = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, this._id_estatus, this._id_causa_falta_pago,
                                 this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura, this._moneda,
                                 fecha_tc, this._total_factura_pesos, this._subtotal_pesos, this._trasladado_pesos, this._retenido_pesos,
                                 this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);
                }

                //Validando que las Operaciones fuesen exitosas
                if (result.OperacionExitosa)
                
                    //Completando Transacción
                    trans.Complete();
            }

            //Devovliendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener el Monto Pendiente de Aplicación
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <returns></returns>
        public static decimal ObtieneMontoPendienteAplicacion(int id_factura)
        {
            //Declarando Objeto de Retorno
            decimal result = 0;
            
            //Armando arreglo de Parametros
            object[] param = { 5, id_factura, 0, "0", null, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, true, "", "" };
            
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Inicializando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Asignando Monto Pendiente Obtenido
                        result = Convert.ToDecimal(dr["MontoPendiente"]);
                }
            }
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus de la Factura a "No Facturable"
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza la Factura</param>
        /// <param name="motivo">Motivo del Cambio de Estatus</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusNoFacturable(int id_usuario, string motivo)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que la Factura este Registrada
            if (this.estatus == EstatusFactura.Registrada)
            {
                //Intsnaciamos Servicio
                using (SAT_CL.Documentacion.Servicio objServicio = new Servicio(this._id_servicio))
                {
                    //Validando que exista el Servicio
                    if (objServicio.habilitar)
                    {
                        //Validando Operación Exitosa (FG)
                        if (ValidaFacturaGlobal().OperacionExitosa)
                        {
                            //Validando Operación Exitosa (FE)
                            if (ValidaFacturaElectronica().OperacionExitosa)
                            {
                                //Validando Operación Exitosa (PR)
                                if (!ValidaProcesoRevision())
                                {
                                    //Inicializando Bloque Transaccional
                                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                    {
                                        //Invocando Método de Actualización
                                        result = this.actualizaRegistros(this._id_servicio, this._no_factura, this._fecha_factura, (byte)EstatusFactura.NoFacturable, this._id_causa_falta_pago,
                                                             this._id_tarifa_cobro, this._total_factura, this._subtotal_factura, this._trasladado_factura, this._retenido_factura, this._moneda,
                                                             this._fecha_tipo_cambio, this._total_factura_pesos, this._subtotal_pesos, this._trasladado_pesos, this._retenido_pesos,
                                                             this._id_condicion_pago, this._dias_credito, id_usuario, this._habilitar);

                                        //Validando Operación Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Insertando Notivo No Facturable
                                            result = SAT_CL.Global.Referencia.InsertaReferencia(objServicio.id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(objServicio.id_compania_emisor, 1, "Motivo (Servicio No Facturable)", 0, "Referencia de Viaje"),
                                                        motivo, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                            //Validando Operación Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Instanciando Excepción
                                                result = new RetornoOperacion(this._id_factura);
                                                //Completando Transacción
                                                scope.Complete();
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("La Factura se encuentra Ligada a un Proceso de Revisión");
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("La Factura se encuentra Timbrada Electronicamente");
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("La Factura se encuentra Ligada a un Factura Global");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Factura no es de Servicio");
                }
            }
            //Si la Factura esta Aplicada o Liquidada
            else if (this.estatus == EstatusFactura.AplicadaParcial || this.estatus == EstatusFactura.Liquidada)
                
                //Instanciando Excepción
                result = new RetornoOperacion("La Factura se encuentra Aplicada, Imposible su Edición");
            else
                //Instanciando Excepción
                result = new RetornoOperacion(string.Format("El Estatus '{0}' de la Factura no permite su Edición", this.estatus));

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Cargar las Refacturaciones ligando un Id de Factura
        /// </summary>
        /// <param name="id_factura"></param>
        /// <returns></returns>
        public static DataTable CargaRefacturacion(int id_factura)
        {
            //Declarando objeto Resultado
            DataTable mit = null;
            //Armando arreglo de Parametros
            object[] param = { 7, id_factura, 0, "", null, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))

                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

            //Realziando consulta 
            return mit;
        }
        /// <summary>
        /// Método  encargado de Actualizar los atributos del Facturado
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFacturado()
        {
            return this.cargaAtributosInstancia(this._id_factura);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_factura_electronica"></param>
        /// <param name="objFacturado"></param>
        /// <returns></returns>
        public static Bancos.EgresoIngreso ObtieneNotaCredito(int id_factura_electronica, out Facturado objFacturado)
        {
            //Declarando Objeto de Retorno
            Bancos.EgresoIngreso notaCredito = new Bancos.EgresoIngreso();
            objFacturado = new Facturado();

            //Armando arreglo de Parametros
            object[] param = { 8, id_factura_electronica, 0, "", null, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, false, "", "" };



            //Devolviendo Resultado Obtenido
            return notaCredito;
        }
        /// <summary>
        /// Método encargado de Realizar la Copia del Facturado para Re-facturar el Facturado
        /// </summary>
        /// <param name="origen_datos"></param>
        /// <param name="id_registro_origen"></param>
        /// <param name="conceptos_relacion">Relación de los Conceptos de Facturación (1.- Facturado, 2.- Fac. Concepto, 3.- Fac. Global)</param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion CopiaFacturado(FacturacionElectronica33.Comprobante.OrigenDatos origen_datos, int id_registro_origen, List<Tuple<int, int, int>> conceptos_relacion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            int idFacNvo = 0;

            //Validando Origen
            switch (origen_datos)
            {
                case FEv33.Comprobante.OrigenDatos.FacturaGlobal:
                    {
                        //Instanciando Factura Global
                        using (FacturaGlobal fg = new FacturaGlobal(id_registro_origen))
                        {
                            //Validando Existencia
                            if (fg.habilitar)
                            {
                                //Validando Estatus
                                if (fg.estatus == FacturaGlobal.Estatus.Facturado && fg.id_factura_electronica33 > 0)
                                {
                                    //Obteniendo Detalles
                                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesFacturacionFacturaGlobal(fg.id_factura_global))
                                    {
                                        //Validando Datos
                                        if (Validacion.ValidaOrigenDatos(dtDetalles))
                                        {
                                            //Instanciando Bloque Transaccional
                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {
                                                //Recoriendo Detalles
                                                foreach (DataRow det in dtDetalles.Rows)
                                                {
                                                    //Instanciando Detalle
                                                    using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(det["Id"])))
                                                    {
                                                        //Validando Registro
                                                        if (ff.habilitar)

                                                            //Actualizando Estatus
                                                            retorno = ff.ActualizaEstatusFacturadoFacturacion(FacturadoFacturacion.Estatus.NotaCredito, id_usuario);
                                                        else
                                                            //Instanciando Excepción
                                                            retorno = new RetornoOperacion("No se puede obtener los Datos del Detalle");

                                                        //Validando Operación
                                                        if (!retorno.OperacionExitosa)

                                                            //Termiando Ciclo
                                                            break;
                                                    }
                                                }

                                                //Validando Operación
                                                if (retorno.OperacionExitosa)
                                                {
                                                    //Agrupando Facturados
                                                    List<int> facs = (from DataRow det in dtDetalles.Rows
                                                                      select Convert.ToInt32(det["IdFactura"])).Distinct().ToList();

                                                    //Validando Conceptos obtenidos
                                                    if (facs.Count > 0)
                                                    {
                                                        //Recorriendo Filas
                                                        foreach (int idFac in facs)
                                                        {
                                                            //Obteniendo Detalles de Un facturado
                                                            List<Tuple<int, int, int>> relaciones = new List<Tuple<int, int, int>>();

                                                            //Obteniendo Conceptos Agrupados
                                                            List<int> concs = (from DataRow det in dtDetalles.Rows
                                                                               select Convert.ToInt32(det["IdFacturaConcepto"])).Distinct().ToList();

                                                            //Validando Conceptos obtenidos
                                                            if (concs.Count > 0)
                                                            {
                                                                //Recorriendo Filas
                                                                foreach (int idCon in concs)

                                                                    //Añadiendo Linea de Relaciones
                                                                    relaciones.Add(new Tuple<int, int, int>(idFac, idCon, fg.id_factura_global));

                                                                //Invocando Método Recursivo
                                                                retorno = Facturado.CopiaFacturado(FEv33.Comprobante.OrigenDatos.Facturado, idFac, relaciones, id_usuario);
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                retorno = new RetornoOperacion("No existen facturados en la factura global.");

                                                            //Validando Operación
                                                            if (!retorno.OperacionExitosa)

                                                                //Termiando Ciclo
                                                                break;
                                                        }
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        retorno = new RetornoOperacion("No existen facturados en la factura global.");

                                                    //Validando Operación Final
                                                    if (retorno.OperacionExitosa)
                                                    {
                                                        //Editando FG
                                                        retorno = fg.ReestableceFacturaGlobal(id_usuario);

                                                        //Validando Operación Final
                                                        if (retorno.OperacionExitosa)

                                                            //Completando Transacción
                                                            scope.Complete();
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion(string.Format("No se pueden recuperar los Detalles de la Factura Global '{0}'.", fg.no_factura_global));
                                    }
                                }
                                else
                                {
                                    //Validando Estatis
                                    switch (fg.estatus)
                                    {
                                        case FacturaGlobal.Estatus.Cancelada:
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion(string.Format("La Factura Global '{0}' esta Cancelada", fg.no_factura_global));
                                            break;
                                        case FacturaGlobal.Estatus.Registrada:
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion(string.Format("La Factura Global '{0}' solo esta Registrada", fg.no_factura_global));
                                            break;
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar la Factura Global.");
                        }
                        break;
                    }
                case FEv33.Comprobante.OrigenDatos.Facturado:
                    {
                        //Instanciando Facturado por Copiar
                        using (Facturado fac_ant = new Facturado(id_registro_origen))
                        {
                            //Validando Registro
                            if (fac_ant.habilitar)
                            {
                                //Obteniendo Conceptos por Copiar
                                using (DataTable dtConceptos = FacturadoConcepto.ObtieneConceptosFactura(fac_ant.id_factura))
                                {
                                    //Validando Datos
                                    if (Validacion.ValidaOrigenDatos(dtConceptos))
                                    {
                                        //Inicializando Bloque Transaccional
                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {
                                            //Validando Origen
                                            if (fac_ant.id_servicio > 0)
                                            {
                                                //Guardando Servicio
                                                int idServ = fac_ant.id_servicio;
                                                //Actualizando Facturado
                                                retorno = fac_ant.ActualizaServicio(-1, id_usuario);

                                                //Validando Operación
                                                if (retorno.OperacionExitosa)
                                                {
                                                    //Insertando Facturado (Copia - Nuevo)
                                                    retorno = Facturado.InsertaFactura(idServ, fac_ant.no_factura, fac_ant.fecha_factura, fac_ant.id_causa_falta_pago,
                                                                    fac_ant.id_tarifa_cobro, fac_ant.total_factura, fac_ant.subtotal_factura, fac_ant.trasladado_factura,
                                                                    fac_ant.retenido_factura, fac_ant.moneda, fac_ant.fecha_tipo_cambio, fac_ant.id_condicion_pago,
                                                                    fac_ant.dias_credito, id_usuario);

                                                    //Validando Operación
                                                    if (retorno.OperacionExitosa)

                                                        //Obteniendo Facturado
                                                        idFacNvo = retorno.IdRegistro;
                                                }
                                            }
                                            else
                                            {
                                                //Obteniendo facturación de Otros
                                                using (FacturacionOtros fo = FacturacionOtros.ObtieneInstanciaFactura(fac_ant.id_factura))
                                                {
                                                    //Validando Existencia
                                                    if (fo.habilitar)
                                                    {
                                                        //Actualizando Facturado
                                                        retorno = fac_ant.ActualizaServicio(-1, id_usuario);

                                                        //Validando Operación
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Insertando Facturado (Copia - Nuevo)
                                                            retorno = Facturado.InsertaFactura(0, fac_ant.no_factura, fac_ant.fecha_factura, fac_ant.id_causa_falta_pago,
                                                                            fac_ant.id_tarifa_cobro, fac_ant.total_factura, fac_ant.subtotal_factura, fac_ant.trasladado_factura,
                                                                            fac_ant.retenido_factura, fac_ant.moneda, fac_ant.fecha_tipo_cambio, fac_ant.id_condicion_pago,
                                                                            fac_ant.dias_credito, id_usuario);

                                                            //Validando Operación
                                                            if (retorno.OperacionExitosa)
                                                            {
                                                                //Obteniendo Facturado
                                                                idFacNvo = retorno.IdRegistro;

                                                                //Actualizando Nuevo Facturado
                                                                retorno = fo.EditaFacturacionOtros(idFacNvo, fo.id_compania_emisora, fo.id_cliente_receptor, id_usuario);
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        retorno = new RetornoOperacion("No se puede recuperar la Facturación de Otros.");
                                                }
                                            }

                                            //Validando Operación
                                            if (retorno.OperacionExitosa)
                                            {
                                                //Recorriendo Conceptos
                                                foreach (DataRow dr in dtConceptos.Rows)
                                                {
                                                    //Instanciando Concepto
                                                    using (FacturadoConcepto fc_ant = new FacturadoConcepto(Convert.ToInt32(dr["Id"])))
                                                    {
                                                        //Validando Concepto
                                                        if (fc_ant.habilitar)
                                                        {
                                                            //Insertando Copia de Concepto
                                                            retorno = FacturadoConcepto.InsertaFacturaConcepto(idFacNvo, fc_ant.cantidad, fc_ant.id_unidad, fc_ant.identificador,
                                                                            fc_ant.id_concepto_cobro, fc_ant.valor_unitario, fc_ant.id_impuesto_retenido, fc_ant.tasa_impuesto_retenido,
                                                                            fc_ant.id_impuesto_trasladado, fc_ant.tasa_impuesto_trasladado, fc_ant.id_cargo_recurrente, id_usuario);

                                                            //Validando Operación
                                                            if (retorno.OperacionExitosa)
                                                            {
                                                                //Validando Lista
                                                                if (conceptos_relacion.Count > 0)
                                                                {
                                                                    //Obteniendo Nuevo Concepto
                                                                    int idFacConNvo = retorno.IdRegistro;
                                                                    //Obteniendo Elemento Coincidente
                                                                    Tuple<int, int, int> concepto = (from Tuple<int, int, int> c in conceptos_relacion
                                                                                                     where c.Item1 == fac_ant.id_factura
                                                                                                     && c.Item2 == fc_ant.id_detalle_facturado
                                                                                                     && c.Item3 > 0
                                                                                                     select c).FirstOrDefault();

                                                                    //Validando Existencia del Concepto
                                                                    if (concepto != null)
                                                                    
                                                                        //Insertando Relación
                                                                        retorno = FacturadoFacturacion.InsertarFacturadoFacturacion(idFacNvo, idFacConNvo, concepto.Item3, 0, 0, id_usuario);
                                                                }
                                                            }

                                                            //Validando Operación negativa
                                                            if (!retorno.OperacionExitosa)
                                                            
                                                                //Validando Lista
                                                                break;
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            retorno = new RetornoOperacion("No se pudo recuperar uno de los Conceptos.");
                                                    }

                                                    //Validando Operación
                                                    if (!retorno.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }

                                                //Validando Operación
                                                if (retorno.OperacionExitosa)
                                                {
                                                    //Validando Lista
                                                    if (conceptos_relacion.Count > 0)
                                                    {
                                                        //Obteniendo Relaciones
                                                        List<Tuple<int, int, int>> relaciones = (from Tuple<int, int, int> c in conceptos_relacion
                                                                                                 where c.Item1 == fac_ant.id_factura
                                                                                                 && c.Item2 == 0
                                                                                                 && c.Item3 > 0
                                                                                                 select c).ToList();

                                                        //Validando Relaciones
                                                        if (relaciones.Count > 0)
                                                        {
                                                            //Recorriendo Lista
                                                            foreach (Tuple<int, int, int> relacion in relaciones)
                                                            {
                                                                //Insertando Relación
                                                                retorno = FacturadoFacturacion.InsertarFacturadoFacturacion(idFacNvo, 0, relacion.Item3, 0, 0, id_usuario);

                                                                //Validando Operación
                                                                if (!retorno.OperacionExitosa)
                                                                    
                                                                    //Terminando Ciclo
                                                                    break;
                                                            }
                                                        }
                                                    }

                                                    //Obteniendo Relación, solo cuando es por Facturado
                                                    int id_ff = FacturadoFacturacion.ObtieneRelacionFactura(fac_ant.id_factura);

                                                    //Validando Relación
                                                    if (id_ff > 0)
                                                    {
                                                        //Obteniendo Relación con facturación Electronica
                                                        using (FacturadoFacturacion ff = new FacturadoFacturacion(id_ff))
                                                        {
                                                            //Validando Existencia
                                                            if (ff.habilitar)

                                                                //Actualizando Estatus
                                                                retorno = ff.ActualizaEstatusFacturadoFacturacion(FacturadoFacturacion.Estatus.NotaCredito, id_usuario);
                                                            else
                                                                //Instanciando Excepción
                                                                retorno = new RetornoOperacion("No se puede recuperar la relación.");
                                                        }
                                                    }

                                                    //Validando Operación
                                                    if (retorno.OperacionExitosa)
                                                    {
                                                        //Instanciando Facturado en retorno
                                                        retorno = new RetornoOperacion(idFacNvo, "El Facturado fue copiado y Actualizado Exitosamente.", true);
                                                        //Completando Excepción
                                                        scope.Complete();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        retorno = new RetornoOperacion("No se pudieron recuperar los Conceptos.");
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar la Factura.");
                        }


                        break;
                    }
                default:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No existe el Facturado o alguna relación con el Comprobante");
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }


        #endregion
    }
}

