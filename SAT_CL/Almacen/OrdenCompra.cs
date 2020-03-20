using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// Clase que perite la insercion; edicion y cambio de estado de un registro de orden de compra
    /// </summary>
    public class OrdenCompra : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeracion de estatus de una orden de compra
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// La OC sólo se ha capturado
            /// </summary>
            Registrada = 1,
            /// <summary>
            /// La OC se encuentra pendiente de abastecimiento
            /// </summary>
            Solicitada = 2,
            /// <summary>
            /// La OC se ha abastecido de forma parcial, puede continuar siendo abastecida
            /// </summary>
            AbastecidaParcial = 3,
            /// <summary>
            /// La OC se ha terminado de abastecer (parcial o totalmente)
            /// </summary>
            Cerrada = 4,            
            /// <summary>
            /// La OC no llegará a ser abastecida
            /// </summary>
            Cancelada = 5
        };
        /// <summary>
        /// Enumeración de tipo de orden de compra
        /// </summary>
        public enum TipoOrden
        {
            /// <summary>
            /// Tipo de orden de compra estandar
            /// </summary>
            Estandar = 1,
        };
        /// <summary>
        /// Enumeración de las formas de entrega de una orden de compra
        /// </summary>
        public enum FormaEntrega
        {
            /// <summary>
            /// La entrega es a domicilio
            /// </summary>
            ADomicilio = 1,
            /// <summary>
            /// La entrega se realiza en las instalaciones del proveedor
            /// </summary>
            EntregaOcurre = 2,
        };
        /// <summary>
        /// Enumeracion de las condiciones de pago de un orden de compra
        /// </summary>
        public enum CondicionesPago
        {
            /// <summary>
            /// Condiciones de Pago a credito
            /// </summary>
            Credito = 1,
            /// <summary>
            /// Condiciones de Pago a Contado
            /// </summary>
            Contado = 2,
        };

        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla orden compra
        /// </summary>
        private static string nom_sp = "almacen.sp_orden_compra_toc";

        private int _id_orden_compra;
        /// <summary>
        /// Id que almacena el identificador de una orden de compra
        /// </summary>
        public int id_orden_compra { get { return _id_orden_compra; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Id que almacena el identificador de la compañia que genera la orden de compra
        /// </summary>
        public int id_compania_emisor { get { return _id_compania_emisor; } }
        private int _no_orden_compra;
        /// <summary>
        /// Almacena el numero secuencial de una orden de compra
        /// </summary>
        public int no_orden_compra { get { return _no_orden_compra; } }
        private string _no_documento_proveedor;
        /// <summary>
        /// Almacena el numero del documento de una orden de compra
        /// </summary>
        public string no_documento_proveedor { get { return _no_documento_proveedor; } }
        private byte _id_estatus;
        /// <summary>
        /// Id que almacena el estatus de una orden de compra
        /// </summary>
        public byte id_estatus { get { return _id_estatus; } }
        /// <summary>
        /// Permite acceder a los elementos de la enumeración Estatus.
        /// </summary>
        public Estatus estatus { get { return (Estatus)this.id_estatus; } }
        private byte _id_tipo_orden;
        /// <summary>
        /// Id que almacena el tipo de orden de compra
        /// </summary>
        public byte id_tipo_orden { get { return _id_tipo_orden; } }
        /// <summary>
        /// Permite acceder a los elementos de la enumeración TipoOrden
        /// </summary>
        public TipoOrden tipo_orden { get { return (TipoOrden)this._id_tipo_orden; } }
        private int _id_proveedor;
        /// <summary>
        /// Id que amacena el identificador del proveedor 
        /// </summary>
        public int id_proveedor { get { return _id_proveedor; } }
        private int _id_almacen;
        /// <summary>
        /// Id que amacena el identificador el almacen
        /// </summary>
        public int id_almacen { get { return _id_almacen; } }
        private byte _id_forma_entrega;
        /// <summary>
        /// Id que permite almacenar la forma de pago 
        /// </summary>
        public byte id_forma_entrega { get { return _id_forma_entrega; } }
        /// <summary>
        /// Permite acceder a los elementos de la enumeración FormaEntrega
        /// </summary>
        public FormaEntrega forma_entrega { get { return (FormaEntrega)this._id_forma_entrega; } }
        private DateTime _fecha_solicitud;
        /// <summary>
        /// Permite almacenar la fecha de solicitud de una orden de compra
        /// </summary>
        public DateTime fecha_solicitud { get { return _fecha_solicitud; } }
        private DateTime _fecha_entrega;
        /// <summary>
        /// Permite almacenar la fecha de entrega de una orden de compra
        /// </summary>
        public DateTime fecha_entrega { get { return _fecha_entrega; } }
        private DateTime _fecha_compromiso;
        /// <summary>
        /// Permite almacenar la fecha compromiso de una orden de compra
        /// </summary>
        public DateTime fecha_compromiso { get { return _fecha_compromiso; } }
        private byte _id_condiciones_pago;
        /// <summary>
        /// Id que permite alamcenar el identificador de las condiciones de pago
        /// </summary>
        public byte id_condiciones_pago { get { return _id_condiciones_pago; } }
        /// <summary>
        /// Permite acceder a los elemetos de la enumeración CondicionesPago
        /// </summary>
        public CondicionesPago condiciones_pago { get { return (CondicionesPago)this._id_condiciones_pago; } }
        private int _id_factura_proveedor;
        /// <summary>
        /// Id que amacena el identificador de la factura de proveedor
        /// </summary>
        public int id_factura_proveedor { get { return _id_factura_proveedor; } }
        private byte _id_moneda;
        /// <summary>
        /// Id que permite definir el tipo de moneda (Peso,dollar)
        /// </summary>
        public byte id_moneda { get { return _id_moneda; } }
        private decimal _subtotal;
        /// <summary>
        /// Permite almacenar el valor del subtotal
        /// </summary>
        public decimal subtotal { get { return _subtotal; } }
        private decimal _imp_trasladado;
        /// <summary>
        /// Permite almacenar el valor total trasladado
        /// </summary>
        public decimal imp_trasladado { get { return _imp_trasladado; } }
        private decimal _imp_retenido;
        /// <summary>
        /// Permite almacenar el valor total retenido
        /// </summary>
        public decimal imp_retenido { get { return _imp_retenido; } }
        private decimal _total;
        /// <summary>
        /// Permite almacenar el valor del total
        /// </summary>
        public decimal total { get { return _total; } }
        private bool _habilitar;
        /// <summary>
        /// Permite cambiar el estado de habilitación de un registro de orden de compra
        /// </summary>
        public bool habilitar {  get { return _habilitar; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor default de la clase que inicializa los atributos.
        /// </summary>
        public OrdenCompra()
        {
            //Asignando Valores
            this._id_orden_compra = 0;
            this._id_compania_emisor = 0;
            this._no_orden_compra = 0;
            this._no_documento_proveedor = "";
            this._id_estatus = 0;
            this._id_tipo_orden = 0;
            this._id_proveedor = 0;
            this._id_almacen = 0;
            this._id_forma_entrega = 0;
            this._fecha_solicitud = DateTime.MinValue;
            this._fecha_entrega = DateTime.MinValue;
            this._fecha_compromiso = DateTime.MinValue;
            this._id_condiciones_pago = 0;
            this._id_factura_proveedor = 0;
            this._id_moneda = 0;
            this._subtotal = 0.00M;
            this._imp_trasladado = 0.00M;
            this._imp_retenido = 0.00M;
            this._total = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor de la clase que inicializa los atributos dado un id_orden_compra
        /// </summary>
        /// <param name="id_orden_compra">Identificador que sirve como referencia para la asignación de datos a los atributos </param>
        public OrdenCompra(int id_orden_compra)
        {
            //Invoca al método cargaAtributos.
            cargaAtributos(id_orden_compra);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destrucotr de la clase
        /// </summary>
        ~OrdenCompra()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que realiza la busqueda de un registro a partir de un id_orden_compra
        /// </summary>
        /// <param name="id_orden_compra">Id que permite la referencia de registros para su asignación a los atributos</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_orden_compra)
        {
            //Creacion del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para el sp 
            object[] param = { 3, id_orden_compra, 0, 0, "", 0, 0, 0, 0, 0, null, null, null, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, 0, false, "", "" };
            //Invoca y asigna los valores del arreglo y el atributo con el nombre del sp al metodo encargado de realizar las transacciones a la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos obtenidos de la transaccion, que existan y qe no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas del Dataset y asigna los valores a los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        //Asignando Valores
                        this._id_orden_compra = id_orden_compra;
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompania"]);
                        this._no_orden_compra = Convert.ToInt32(r["NoOrdenCompra"]);
                        this._no_documento_proveedor = r["NoDocumentoProveedor"].ToString();
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_tipo_orden = Convert.ToByte(r["IdTipoOrden"]);
                        this._id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        this._id_almacen = Convert.ToInt32(r["IdAlmacen"]);
                        this._id_forma_entrega = Convert.ToByte(r["IdFormaEntrega"]);
                        DateTime.TryParse(r["FechaSolicitud"].ToString(), out this._fecha_solicitud);
                        DateTime.TryParse(r["FechaEntrega"].ToString(), out this._fecha_entrega);
                        DateTime.TryParse(r["FechaCompromiso"].ToString(), out this._fecha_compromiso);
                        this._id_condiciones_pago = Convert.ToByte(r["IdCondicionesPago"]);
                        this._id_factura_proveedor = Convert.ToInt32(r["IdFacturaProveedor"]);
                        this._id_moneda = Convert.ToByte(r["IdMoneda"]);
                        this._subtotal = Convert.ToDecimal(r["SubTotal"]);
                        this._imp_trasladado = Convert.ToDecimal(r["ImpTrasladado"]);
                        this._imp_retenido = Convert.ToDecimal(r["ImpRetenido"]);
                        this._total = Convert.ToDecimal(r["Total"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la sentencia
                    retorno = true;
                }
            }
            //Retorno al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite la actualizacion de registros de una orden de compra
        /// </summary>
        /// <param name="id_compania_emisor">Id que permite la actualización de la compañia de una orden de compra</param>
        /// <param name="no_orden_compra">Permite la actualización del No. de Orden de Compra</param>
        /// <param name="no_documento_proveedor">Permite la Actualización del No. de Documento del Proveedor</param>
        /// <param name="id_estatus">Permite la actualización del estatus de una orden de compra</param>
        /// <param name="id_tipo_orden">Permite la actualizacion del tipo de orden</param>
        /// <param name="id_proveedor">Id que permite la actualizacion del proveedor</param>
        /// <param name="id_almacen">Id que permite la actualización del almacen</param>
        /// <param name="id_forma_entrega">Permite la actualizacion de la forma de entrega</param>
        /// <param name="fecha_solicitud">Permite la actualizacion de la fecha de solicitud</param>
        /// <param name="fecha_entrega">Permite la actualización de la fecha de entrega</param>
        /// <param name="fecha_compromiso">Pemite la actualizacion de la fecha comprmiso</param>
        /// <param name="id_condiciones_pago">Permite la actualizacion de las condiciones de pago</param>
        /// <param name="id_factura_proveedor">Id que permite la actualización de la Factura del Proveedor</param>
        /// <param name="id_moneda">Permite la actualizacion del tipo de moneda</param>
        /// <param name="subtotal">Permite la actualización del SubTotal</param>
        /// <param name="imp_trasladado">Permite la actualización del Impuesto Trasladado</param>
        /// <param name="imp_retenido">Permite la actualización del Impuesto Retenido</param>
        /// <param name="total">Permite la actualización del Total</param>
        /// <param name="id_usuario">Permite actualizar el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Permite cambiar el estado de habilitación de un registro</param>
        /// <returns></returns>
        private RetornoOperacion editarOrdenCompra(int id_compania_emisor, int no_orden_compra, string no_documento_proveedor, byte id_estatus,
                                    byte id_tipo_orden, int id_proveedor, int id_almacen, byte id_forma_entrega, DateTime fecha_solicitud,
                                    DateTime fecha_entrega, DateTime fecha_compromiso, byte id_condiciones_pago, int id_factura_proveedor,
                                    byte id_moneda, decimal subtotal, decimal imp_trasladado, decimal imp_retenido, decimal total,
                                    int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            
            //Creación del arreglo de tipo objeto que almacena los valores de los parametros del sp de la tabla Orden Compra.
            object[] param = { 2, this._id_orden_compra, id_compania_emisor, no_orden_compra, no_documento_proveedor, id_estatus,
                                    id_tipo_orden, id_proveedor, id_almacen, id_forma_entrega, Fecha.ConvierteDateTimeObjeto(fecha_solicitud),
                                    Fecha.ConvierteDateTimeObjeto(fecha_entrega), Fecha.ConvierteDateTimeObjeto(fecha_compromiso), 
                                    id_condiciones_pago, id_factura_proveedor, id_moneda, subtotal, imp_trasladado, imp_retenido, total,
                                    id_usuario, habilitar, "", "" };
            
            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            
            //Retorna al método el objeto retorno
            return retorno;
        }

        #endregion

        #region Método Públicos

        /// <summary>
        /// Método que permite la actualizacion de registros de una orden de compra
        /// </summary>
        /// <param name="id_compania_emisor">Id que permite la actualización de la compañia de una orden de compra</param>
        /// <param name="no_documento_proveedor">Permite la Actualización del No. de Documento del Proveedor</param>
        /// <param name="id_tipo_orden">Permite la actualizacion del tipo de orden</param>
        /// <param name="id_proveedor">Id que permite la actualizacion del proveedor</param>
        /// <param name="id_almacen">Id que permite la actualización del almacen</param>
        /// <param name="id_forma_entrega">Permite la actualizacion de la forma de entrega</param>
        /// <param name="fecha_solicitud">Permite la actualizacion de la fecha de solicitud</param>
        /// <param name="fecha_entrega">Permite la actualización de la fecha de entrega</param>
        /// <param name="fecha_compromiso">Pemite la actualizacion de la fecha comprmiso</param>
        /// <param name="id_condiciones_pago">Permite la actualizacion de las condiciones de pago</param>
        /// <param name="id_factura_proveedor">Id que permite la actualización de la Factura del Proveedor</param>
        /// <param name="id_moneda">Permite la actualizacion del tipo de moneda</param>
        /// <param name="subtotal">Permite la actualización del SubTotal</param>
        /// <param name="imp_trasladado">Permite la actualización del Impuesto Trasladado</param>
        /// <param name="imp_retenido">Permite la actualización del Impuesto Retenido</param>
        /// <param name="total">Permite la actualización del Total</param>
        /// <param name="id_usuario">Permite actualizar el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Permite cambiar el estado de habilitación de un registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarOrdenCompra(int id_compania_emisor, string no_documento_proveedor,
                                    TipoOrden tipo_orden, int id_proveedor, int id_almacen, FormaEntrega forma_entrega, DateTime fecha_solicitud,
                                    DateTime fecha_entrega, DateTime fecha_compromiso, CondicionesPago condiciones_pago, int id_factura_proveedor,
                                    byte id_moneda, decimal subtotal, decimal imp_trasladado, decimal imp_retenido, decimal total,
                                    int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Almacen
            if (id_almacen > 0)
            {
                //Creación del arreglo de tipo objeto que alamcena el valor de los parametros necesarios para el sp de la tabla OrdenCompra.
                object[] param = { 1, 0, id_compania_emisor, 0, no_documento_proveedor, (byte)Estatus.Registrada, (byte)tipo_orden, id_proveedor,
                                id_almacen, (byte)forma_entrega, Fecha.ConvierteDateTimeObjeto(fecha_solicitud), Fecha.ConvierteDateTimeObjeto(fecha_entrega),
                                Fecha.ConvierteDateTimeObjeto(fecha_compromiso), (byte)condiciones_pago, id_factura_proveedor, id_moneda,
                                subtotal, imp_trasladado, imp_retenido, total, id_usuario, true, "", "" };

                //Asignacion al objeto retorno del arreglo y el atributo con el nombre del sp, para la transacción a la base de datos.
                retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            }
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("Debe de ingresar un Almacen Valido.");
            
            //Retrono del objeto retrono al método
            return retorno;
        }
        /// <summary>
        /// Método que permite la actualiación de registros de una orden de compra
        /// </summary>
        /// <param name="id_compania_emisor">Id que permite la actualización de la compañia de una orden de compra</param>
        /// <param name="id_proveedor">Id que permite la actualizacion del proveedor</param>
        /// <param name="estatus">Permite la actualización del estatus de una orden de compra</param>
        /// <param name="id_tipo_orden">Premite la actualizacion del tipo de orden</param>
        /// <param name="id_forma_entrega">Permite la actualizacion de la forma de entrega</param>
        /// <param name="id_condiciones_pago">Permite la actualizacion de las condiciones de pago</param>
        /// <param name="id_moneda">Permite la actualizacion del tipo de moneda</param>
        /// <param name="fecha_solicitud">Permite la actualizacion de la fecha de solicitud </param>
        /// <param name="fecha_entrega">Permite la actualización de la fecha de entrega</param>
        /// <param name="fecha_compromiso">Pemite la actualizacion de la fecha comprmiso</param>
        /// <param name="dias_entrega">Permite la actualizacion de los dias de entrega de una orden de compra</param>
        /// <param name="id_usuario">Permite actualizar el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Permite cambiar ele estado de habilitación de un registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarOrdenCompra(int id_compania_emisor, string no_documento_proveedor, Estatus estatus,
                                    TipoOrden tipo_orden, int id_proveedor, int id_almacen, FormaEntrega forma_entrega, DateTime fecha_solicitud,
                                    DateTime fecha_entrega, DateTime fecha_compromiso, CondicionesPago condiciones_pago, int id_factura_proveedor,
                                    byte id_moneda, decimal subtotal, decimal imp_trasladado, decimal imp_retenido, decimal total,
                                    int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            bool validaCambioTaller = false;

            //Validando que este 'Abastecida Parcial' o 'Cerrada'
            if (estatus == Estatus.AbastecidaParcial || estatus == Estatus.Cerrada)
            {
                //Validando que sea el mismo taller
                if (this._id_almacen == id_almacen)

                    //Asignando Positiva la Validación
                    validaCambioTaller = true;
                else
                    //Asignando Negativa la Validación
                    validaCambioTaller = false;
            }
            else
                //Asignando Positiva la Validación
                validaCambioTaller = true;

            /*/Validando que la Orden estuviese Registrada
            if ((Estatus)this._id_estatus == Estatus.Registrada)
            {//*/

            //Validando Cambio de Taller
            if (validaCambioTaller)
            {
                //Validando Almacen
                if (id_almacen > 0)
                {
                    //Si ya posee una Factura de Proveedor
                    if (id_factura_proveedor == 0)

                        //Invoca y retorna el método editarOrdenCompra.
                        result = this.editarOrdenCompra(id_compania_emisor, this._no_orden_compra, no_documento_proveedor, (byte)estatus, (byte)tipo_orden, id_proveedor,
                                            id_almacen, (byte)forma_entrega, fecha_solicitud, fecha_entrega, fecha_compromiso, (byte)condiciones_pago, id_factura_proveedor,
                                            id_moneda, subtotal, imp_trasladado, imp_retenido, total, id_usuario, this._habilitar);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Orden de Compra ya Posee una Factura");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe de ingresar un Almacen Valido.");
            }
            else
            {   
                //Validando Estatus
                switch (estatus)
                {
                    case Estatus.AbastecidaParcial:
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Orden de Compra esta 'Abastecida Parcial', no puede cambiar el Almacen");
                        break;
                    case Estatus.Cerrada:
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Orden de Compra esta 'Cerrada', no puede cambiar el Almacen");
                        break;
                }
            }
            /*}else
                //Instanciando Excepción
                result = new RetornoOperacion("La Orden tiene que estar 'Registrada' para su Edición");//*/

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que permite el cambio de estado de un registro de una orden de compra
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo cambios sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarOrdenCompra(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que la Orden estuviese Registrada
            if ((Estatus)this._id_estatus == Estatus.Registrada)
            {
                //Si ya posee una Factura de Proveedor
                if (id_factura_proveedor == 0)

                    //Invoca y retorna el método editarOrdenCompra
                    result = this.editarOrdenCompra(this._id_compania_emisor, this._no_orden_compra, this._no_documento_proveedor, this._id_estatus, this._id_tipo_orden, this._id_proveedor,
                                        this._id_almacen, this._id_forma_entrega, this._fecha_solicitud, this._fecha_entrega, this._fecha_compromiso, this._id_condiciones_pago, this._id_factura_proveedor,
                                        this._id_moneda, this._subtotal, this._imp_trasladado, this._imp_retenido, this._total, id_usuario, false);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Orden de Compra ya Posee una Factura");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("La Orden tiene que estar 'Registrada' para su Eliminación");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que permite actualizar los valores de la orden de compra
        /// </summary>
        /// <returns></returns>
        public bool ActualizaOrdenCompra()
        {
            //Cargando Atributos de la Clase
            return this.cargaAtributos(this._id_orden_compra);
        }
        /// <summary>
        /// Método que permite actualizar el estado de una orden de compra.
        /// </summary>
        /// <param name="estatus">Permite accesar a la enumeración del estatus y poder modificarlo(solicitada,cancelada,cerrada,abastecida parcial.)</param>
        /// <param name="id_usuario">Permite identificar al usuario que realizo la ultima acción sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusOrdenCompra(Estatus estatus, int id_usuario)
        {
            //Retorna e Invoca el método de edición 
            return this.editarOrdenCompra(this._id_compania_emisor, this._no_orden_compra, this._no_documento_proveedor, (byte)estatus, this._id_tipo_orden, this._id_proveedor,
                                this._id_almacen, this._id_forma_entrega, this._fecha_solicitud, this._fecha_entrega, this._fecha_compromiso, this._id_condiciones_pago, this._id_factura_proveedor,
                                this._id_moneda, this._subtotal, this._imp_trasladado, this._imp_retenido, this._total, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Método que permite actualizar el estado de una orden de compra.
        /// </summary>
        /// <param name="estatus">Permite accesar a la enumeración del estatus y poder modificarlo(solicitada,cancelada,cerrada,abastecida parcial.)</param>
        /// <param name="id_usuario">Permite identificar al usuario que realizo la ultima acción sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusOrdenCompraDetalle(Estatus estatus, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Retorna e Invoca el método de edición 
                result = this.editarOrdenCompra(this._id_compania_emisor, this._no_orden_compra, this._no_documento_proveedor, (byte)estatus, this._id_tipo_orden, this._id_proveedor,
                                    this._id_almacen, this._id_forma_entrega, this._fecha_solicitud, this._fecha_entrega, this._fecha_compromiso, this._id_condiciones_pago, this._id_factura_proveedor,
                                    this._id_moneda, this._subtotal, this._imp_trasladado, this._imp_retenido, this._total, id_usuario, this.habilitar);

                //Validando Almacen -- En caso de no estarse cargando bien
                if (this._id_almacen > 0)
                {
                    //Validando Resultado Exitoso
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Detalles
                        using (DataTable dtDetalles = OrdenCompraDetalle.CargaDetallesOrdenCompra(this._id_orden_compra))
                        {
                            //Validando que existan Detalles
                            if (Validacion.ValidaOrigenDatos(dtDetalles))
                            {
                                //Recorriendo Detalles
                                foreach (DataRow dr in dtDetalles.Rows)
                                {
                                    //Instanciando Detalle
                                    using (OrdenCompraDetalle od = new OrdenCompraDetalle(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista el Detalle
                                        if (od.habilitar)
                                        {
                                            //Actualizando Detalles
                                            result = od.ActualizaEstatusDetalle((OrdenCompraDetalle.Estatus)((byte)estatus), id_usuario);

                                            //Validando que la Operación no fuese Exitosa
                                            if (!result.OperacionExitosa)

                                                //Terminando Ciclo
                                                break;
                                        }
                                        else
                                        {
                                            //Instanciando 
                                            result = new RetornoOperacion("No se puede Acceder a los Detalles");

                                            //Terminando Ciclo
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No Existen Detalles");
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe un Almacen en la Orden");

                //Validando Resultado Exitoso
                if (result.OperacionExitosa)
                {
                    //Instanciando Orden de Compra
                    result = new RetornoOperacion(this._id_orden_compra);

                    //Completando Transacción
                    trans.Complete();
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Actualiza el estatus de la OC a Cerrado, indicando la fecha de entrega de la OC como la fecha final de recepción de la misma
        /// </summary>
        /// <param name="fecha_entrega">Fecha de cierre de orden de compra</param>
        /// <param name="id_usuario">Permite identificar al usuario que realizo la ultima acción sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion CerrarOrdenCompra(DateTime fecha_entrega, int id_usuario)
        {
            //Retorna e Invoca el método de edición 
            return this.editarOrdenCompra(this._id_compania_emisor, this._no_orden_compra, this._no_documento_proveedor, (byte)Estatus.Cerrada, this._id_tipo_orden, this._id_proveedor,
                                this._id_almacen, this._id_forma_entrega, this._fecha_solicitud, fecha_entrega, this._fecha_compromiso, this._id_condiciones_pago, this._id_factura_proveedor,
                                this._id_moneda, this._subtotal, this._imp_trasladado, this._imp_retenido, this._total, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar los Totales de la Orden de Compra
        /// </summary>
        /// <param name="subtotal">SubTotal de la Orden</param>
        /// <param name="trasladado">Impuesto Trasladado de la Orden</param>
        /// <param name="retenido">Impuesto Retenido de la Orden</param>
        /// <param name="total">Total de la Orden</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTotalesOrdenCompra(decimal subtotal, decimal trasladado, decimal retenido, decimal total, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Retorna e Invoca el método de edición 
            result = this.editarOrdenCompra(this._id_compania_emisor, this._no_orden_compra, this._no_documento_proveedor, this._id_estatus, this._id_tipo_orden, this._id_proveedor,
                                this._id_almacen, this._id_forma_entrega, this._fecha_solicitud, fecha_entrega, this._fecha_compromiso, this._id_condiciones_pago, this._id_factura_proveedor,
                                this._id_moneda, subtotal, trasladado, retenido, total, id_usuario, this.habilitar);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que realiza la consulta del encabezado de la orden de compra a partir de un registro dado
        /// </summary>
        /// <param name="id_orden_compra">Identificador que sirve como referencia para la busqueda del registro de Orden de Compra</param>
        /// <returns></returns>
        public static DataTable EncabezadoOrdenCompra(int id_orden_compra)
        {
            //Creacion de la tabla dtencabezado
            DataTable dtEncabezado = null;
            //Creación del arreglo que almacena los datos necesario para realizar la consulta a base de datos
            object[] param = { 4, id_orden_compra, 0, 0, "", 0, 0, 0, 0, 0, null, null, null, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, 0, false, "", "" };
            //Invoca el método que reliza la consulta a base  de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos almacenados en el dataset
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(DS,"Table"))                
                    //Asigna los valores del dataset a la tabla dtEncabezado
                    dtEncabezado = DS.Tables["Table"];

                //Retorna el resultado al método
                return dtEncabezado;                
            }
        }
        /// <summary>
        /// Método que permite actualizar el estado de una orden de compra.
        /// </summary>
        /// <param name="estatus">Permite identificar la Factura de Proveedor de la Orden de Compra</param>
        /// <param name="id_usuario">Permite identificar al usuario que realizo la ultima acción sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaFacturaOrdenCompra(int id_factura_proveedor, int id_usuario)
        {
            //Retorna e Invoca el método de edición 
            return this.editarOrdenCompra(this._id_compania_emisor, this._no_orden_compra, this._no_documento_proveedor, this._id_estatus, this._id_tipo_orden, this._id_proveedor,
                                this._id_almacen, this._id_forma_entrega, this._fecha_solicitud, this._fecha_entrega, this._fecha_compromiso, this._id_condiciones_pago, id_factura_proveedor,
                                this._id_moneda, this._subtotal, this._imp_trasladado, this._imp_retenido, this._total, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Método encargado de Obtener la Factura Ligada a la Orden De Compra
        /// </summary>
        /// <param name="id_orden_compra">Orden de Compra</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturaOrdenCompra(int id_orden_compra)
        {
            //Declarando Objeto de Retorno
            DataTable dtFactura = null;

            //Creación del arreglo que almacena los datos necesario para realizar la consulta a base de datos
            object[] param = { 5, id_orden_compra, 0, 0, "", 0, 0, 0, 0, 0, null, null, null, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, 0, false, "", "" };

            //Invoca el método que reliza la consulta a base  de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos almacenados en el dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    
                    //Asigna los valores del dataset a la tabla dtEncabezado
                    dtFactura = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFactura;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public byte[] GeneraImpresionOrdenCompraPDF(int id_usuario)
        {
            //Creando nuevo visor de reporte
            ReportViewer rvReporte = new ReportViewer();

            //Variable qie almacena el identificador de una orden de compra
            int idOrdenCompra = this._id_orden_compra;

            //Declara la ubicación local del reporte orden de compra
            rvReporte.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/RDLC/OrdenCompra.rdlc");

            //Creación de la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            DataTable dtFirmaU = new DataTable();
            //Agrega una columna a la tabla 
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            dtFirmaU.Columns.Add("Imagen", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            rvReporte.LocalReport.DataSources.Clear();

            //Invoca a la clase Orden de Compra y obtiene los datos principales de la Orden de compra
            using (SAT_CL.Almacen.OrdenCompra orden = new SAT_CL.Almacen.OrdenCompra(idOrdenCompra))
            {
                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa  (Encabezado impresión)                              
                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(orden.id_compania_emisor))
                {
                    //Recorre las filas de la tabla
                    foreach (DataRow r in encabezado.Rows)
                    {
                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                        ReportParameter email = new ReportParameter("Email", r["email"].ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion, email });
                    }
                }
                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(orden.id_compania_emisor))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }

                //Instanciando Usuario
                using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(id_usuario))
                {
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] firma_img = null;

                    //Validando Usuario
                    if (user.habilitar)
                    {
                        //Obteniendo Archivos de Firma
                        using (DataTable dtFirma = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(30, user.id_usuario, 24, 0))
                        {
                            //Validando Firma
                            if (Validacion.ValidaOrigenDatos(dtFirma))
                            {
                                //Recorriendo Fila
                                foreach (DataRow dr in dtFirma.Rows)
                                {
                                    //Captura errores al momento de consultar la ubicación del logotipo.
                                    try
                                    {
                                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                                        firma_img = System.IO.File.ReadAllBytes(dr["URL"].ToString());
                                    }
                                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                                    catch { firma_img = null; }
                                }
                            }
                        }
                    }

                    //Añadiendo Fila
                    dtFirmaU.Rows.Add(firma_img);

                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("FirmaUsuario", dtFirmaU);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }

                //Encabezado de la orden de Compra
                using (DataTable encabezado = SAT_CL.Almacen.OrdenCompra.EncabezadoOrdenCompra(idOrdenCompra))
                {
                    //Valida que el datatable encabezado contenga valores vaidos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(encabezado))
                    {
                        //Recorre las filas del datatable y el resultado los almacena en los parametros del reporte
                        foreach (DataRow r in encabezado.Rows)
                        {
                            //Obteniendo Estatus
                            string tit = "ORDEN DE COMPRA";

                            //Validando Configuración por Compania
                            switch (orden.id_compania_emisor)
                            {
                                case 72:
                                    {
                                        //Si la Orden este Cerrada
                                        tit = orden.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Cerrada ? "ORDEN DE COMPRA" : "REQUISICIÓN DE COMPRA";
                                        break;
                                    }
                                case 76:
                                    {
                                        //Si la Orden este Cerrada
                                        tit = orden.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Cerrada ? "ORDEN DE COMPRA" : "REQUISICIÓN DE COMPRA";
                                        break;
                                    }
                            }

                            //Asignando Valores a los Atributos
                            ReportParameter titulo = new ReportParameter("TituloOrdenCompra", tit);
                            ReportParameter noCompra = new ReportParameter("NoCompra", r["NoCompra"].ToString());
                            ReportParameter documentoProveedor = new ReportParameter("DocumentoProveedor", r["DocumentoProveedor"].ToString());
                            ReportParameter proveedor = new ReportParameter("Proveedor", r["Proveedor"].ToString());
                            ReportParameter rfcProveedor = new ReportParameter("RFCProveedor", r["RFC"].ToString());
                            ReportParameter direccionProveedor = new ReportParameter("DireccionProveedor", r["Direccion"].ToString().ToUpper());
                            ReportParameter almacen = new ReportParameter("Almacen", r["Almacen"].ToString());
                            ReportParameter estatus = new ReportParameter("Estatus", r["Estatus"].ToString());
                            ReportParameter formaEntrega = new ReportParameter("FormaEntrega", r["FormaEntrega"].ToString());
                            ReportParameter fechaSolicitud = new ReportParameter("FechaSolicitud", r["FechaSolicitud"].ToString());
                            ReportParameter fechaCompromiso = new ReportParameter("FechaCompromiso", r["FechaCompromiso"].ToString());
                            ReportParameter condicionesPago = new ReportParameter("CondicionesPago", r["CondicionesPago"].ToString());
                            ReportParameter total = new ReportParameter("Total", r["Total"].ToString());
                            ReportParameter subTotal = new ReportParameter("SubTotal", r["SubTotal"].ToString());
                            ReportParameter ivaTrasladado = new ReportParameter("IvaTrasladado", r["IvaTrasladado"].ToString());
                            ReportParameter ivaRetenido = new ReportParameter("IvaRetenido", r["IvaRetenido"].ToString());
                            ReportParameter importeLetra = new ReportParameter("ImporteLetra", TSDK.Base.Cadena.ConvierteMontoALetra(r["Total"].ToString()));
                            ReportParameter direccionAlmacen = new ReportParameter("DireccionAlmacen", r["DireccionALmacen"].ToString());
                            ReportParameter telefonoProveedor = new ReportParameter("TelefonoProveedor", r["TelefonoProv"].ToString());
                            //Agrega los parametros al reporte
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { titulo, noCompra, documentoProveedor, proveedor, rfcProveedor, direccionProveedor, almacen, estatus, formaEntrega, fechaSolicitud, 
                                                                                        fechaCompromiso, condicionesPago, total, subTotal, ivaTrasladado, ivaRetenido, importeLetra,direccionAlmacen, telefonoProveedor });
                        }
                    }
                }
                //Carga las actividades y fallas de la orden de trabajo
                using (DataTable dtOrdenCompra = SAT_CL.Almacen.OrdenCompraDetalle.OrdenCompraImpresionDetalles(idOrdenCompra))
                {
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtOrdenCompra))
                    {
                        ReportDataSource rsOrdenCompra = new ReportDataSource("DetalleOrdenCompra", dtOrdenCompra);
                        rvReporte.LocalReport.DataSources.Add(rsOrdenCompra);
                    }
                    else
                    {
                        DataTable dtTem = new DataTable();
                        dtTem.Columns.Add("Estatus", typeof(string));
                        dtTem.Columns.Add("Codigo", typeof(string));
                        dtTem.Columns.Add("Producto", typeof(string));
                        dtTem.Columns.Add("Categoria", typeof(string));
                        dtTem.Columns.Add("CantidadInicial", typeof(string));
                        dtTem.Columns.Add("UnidadMedida", typeof(string));
                        dtTem.Columns.Add("PrecioUnitario", typeof(string));
                        dtTem.Columns.Add("Total", typeof(int));
                        dtTem.Rows.Add("", "", "", "", "", "", "", 0);
                        ReportDataSource rsOrdenCompra = new ReportDataSource("DetalleOrdenCompra", dtTem);
                        rvReporte.LocalReport.DataSources.Add(rsOrdenCompra);
                    }

                }
            }

            //Devolviendo resultado
            return rvReporte.LocalReport.Render("PDF");
        }

        #endregion
    }
}
