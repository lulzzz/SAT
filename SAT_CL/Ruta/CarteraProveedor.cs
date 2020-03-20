using System;
using System.Configuration;
using System.Data;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Ruta
{
    class CarteraProveedor : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "ruta.sp_cartera_proveedor_tcp";

        private int _id_cartera_proveedor;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Cartera de Proveedor
        /// </summary>
        public int id_cartera_proveedor { get { return this._id_cartera_proveedor; } }
        private int _id_proveedor;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Proveedor
        /// </summary>
        public int id_proveedor { get { return this._id_proveedor; } }
        private int _id_producto;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Producto
        /// </summary>
        public int id_producto { get { return this._id_producto; } }
        private int _id_tipo;
        /// <summary>
        /// Atributo encargado de almacenar el Id tipo 
        /// </summary>
        public int id_tipo { get { return this._id_tipo; } }
        private decimal _litros;
        /// <summary>
        /// Atributo encargado de almacenar los litros
        /// </summary>
        public decimal litros { get { return this._litros; } }
        private decimal _precio_unitario;
        /// <summary>
        /// Atributo encargado de almacenar el Precio Unitario
        /// </summary>
        public decimal precio_unitario { get { return this._precio_unitario; } }
        private decimal _monto;
        /// <summary>
        /// Atributo encargado de almacenar el monto
        /// </summary>
        public decimal monto { get { return this._monto; } }
        private int _id_anticipo;
        /// <summary>
        /// Atributo encargado de almacenar el id de anticipo
        /// </summary>
        public int id_anticipo { get { return this._id_anticipo; } }
        private string _referencia_proveedor;
        /// <summary>
        /// Atributo encargado de almacenar la referencia de proveedor
        /// </summary>
        public string referencia_proveedor { get { return this._referencia_proveedor; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de almacenar el Id de servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private int _id_segmento;
        /// <summary>
        /// Atributo de almacenar el Id de segmento
        /// </summary>
        public int id_segmento { get { return this._id_segmento; } }
        private int _id_ruta;
        /// <summary>
        /// tributo de almacenar el Id de la ruta
        /// </summary>
        public int id_ruta { get { return this._id_ruta; } }
        private DateTime _fecha;
        /// <summary>
        /// Atributo encargado de almacenar la fecha
        /// </summary>
        public DateTime fecha { get { return this._fecha; } }
        private int _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el id de estatus
        /// </summary>
        public int id_estatus { get { return this._id_estatus; } }
        private int _id_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Factura
        /// </summary>
        public int id_factura { get { return this._id_factura; } }        
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public CarteraProveedor()
        {
            _id_cartera_proveedor = 0;
            _id_proveedor = 0;
            _id_producto = 0;
            _id_tipo = 0;
            _litros = 0.0M;
            _precio_unitario = 0.0M;
            _monto = 0.0M;
            _id_anticipo = 0;
            _referencia_proveedor = "";
            _id_servicio = 0;
            _id_segmento = 0;
            _id_ruta = 0;
            _fecha = DateTime.MinValue;
            _id_estatus = 0;
            _id_factura = 0;
            _habilitar = false;

        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public CarteraProveedor(int id_registro)
        {
            //Invocando Método de Carga
            cargaAtributos(id_registro);
        }
        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~CarteraProveedor()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de registros, y almacena el resultado en los atributos
        /// </summary>
        /// <param name="id_reporte_unidad_foranea">Identificador de reporte de unidades foraneas</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_cartera_proveedor)
        {
            //Creacion del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para el store procedure
            object[] param = { 3, id_cartera_proveedor, 0, 0, 0, 0.00M, 0.00M, 0.00M, 0, "", 0, 0, 0, null, 0, 0, 0, false, "", "" };
            //Crea un dataset y almacena el resultado del método EjecutaProcAlmacenadoDataSet 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que existan datos en el dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas del registro encontrado y almacena el resultado en cada actributo
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_cartera_proveedor = id_cartera_proveedor;
                        this._id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        this._id_producto = Convert.ToInt32(r["IProducto"]);
                        this._id_tipo = Convert.ToInt32(r["IdTipo"]);
                        this._litros = Convert.ToDecimal(r["Litros"]);
                        this._precio_unitario = Convert.ToDecimal(r["PrecioUnitario"]);
                        this._monto = Convert.ToDecimal(r["Monto"]);
                        this._id_anticipo = Convert.ToInt32(r["IdAnticipo"]);
                        this._referencia_proveedor = Convert.ToString(r["ReferenciaProveedor"]);
                        this._id_servicio = Convert.ToInt32(r["IdServicio"]);
                        this._id_segmento = Convert.ToInt32(r["IdSegmento"]);
                        this._id_ruta = Convert.ToInt32(r["IdRuta"]);
                        this._fecha = Convert.ToDateTime(r["Fecha"]);
                        this._id_estatus = Convert.ToInt32(r["IdEstatus"]);
                        this._id_factura = Convert.ToInt32(r["IdFactura"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorno el objeto retorno al método
            return retorno;
        }

        /// <summary>
        /// Método que edita los registros de cartera proveedor
        /// </summary>
        /// <param name="id_proveedor"></param>
        /// <param name="id_producto"></param>
        /// <param name="id_tipo"></param>
        /// <param name="litros"></param>
        /// <param name="precio_unitario"></param>
        /// <param name="monto"></param>
        /// <param name="id_anticipo"></param>
        /// <param name="referencia_proveedor"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_segmento"></param>
        /// <param name="id_ruta"></param>
        /// <param name="fecha"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_factura"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaCarteraProveedor(int id_proveedor, int id_producto, int id_tipo, decimal litros, decimal precio_unitario,
                                                       decimal monto, int id_anticipo, string referencia_proveedor, int id_servicio, int id_segmento, 
                                                       int id_ruta, DateTime fecha, int id_estatus, int id_factura, int id_usuario, bool habilitar)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_cartera_proveedor, id_proveedor, id_producto, id_tipo, litros, precio_unitario, monto, id_anticipo, referencia_proveedor, id_servicio, id_segmento, id_ruta, fecha, id_estatus, id_factura,
                                id_usuario, habilitar, "", "" };
            //Realizando actualizacion
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado obtenido
            return resultado;
        }
        #endregion

        #region Metodos públicos
        /// <summary>
        /// Metodo encargado de insertar
        /// </summary>
        /// <param name="id_proveedor"></param>
        /// <param name="id_producto"></param>
        /// <param name="id_tipo"></param>
        /// <param name="litros"></param>
        /// <param name="precio_unitario"></param>
        /// <param name="monto"></param>
        /// <param name="id_anticipo"></param>
        /// <param name="referencia_proveedor"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_segmento"></param>
        /// <param name="id_ruta"></param>
        /// <param name="fecha"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_factura"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCarteraProveedor(int id_proveedor, int id_producto, int id_tipo, decimal litros, decimal precio_unitario,
                                                       decimal monto, int id_anticipo, string referencia_proveedor, int id_servicio, int id_segmento,
                                                       int id_ruta, DateTime fecha, int id_estatus, int id_factura, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para el store procedure
            object[] param = { 1, 0, id_proveedor, id_producto, id_tipo, litros, precio_unitario, monto, id_anticipo, referencia_proveedor, id_servicio,
                id_segmento, id_ruta, fecha, id_estatus, id_factura, id_usuario, true, "", "" };
            //Asigna valores al objeto retorno
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno el resultado al objeto retorno
            return resultado;
        }

        /// <summary>
        /// Metodo que actualiza los campos de un registro
        /// </summary>
        /// <param name="id_proveedor"></param>
        /// <param name="id_producto"></param>
        /// <param name="id_tipo"></param>
        /// <param name="litros"></param>
        /// <param name="precio_unitario"></param>
        /// <param name="monto"></param>
        /// <param name="id_anticipo"></param>
        /// <param name="referencia_proveedor"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_segmento"></param>
        /// <param name="id_ruta"></param>
        /// <param name="fecha"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_factura"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaCarteraProveedor(int id_proveedor, int id_producto, int id_tipo, decimal litros, decimal precio_unitario,
                                                       decimal monto, int id_anticipo, string referencia_proveedor, int id_servicio, int id_segmento,
                                                       int id_ruta, DateTime fecha, int id_estatus, int id_factura, int id_usuario)
        {
            return this.editaCarteraProveedor(id_proveedor, id_producto, id_tipo, litros, precio_unitario, monto, id_anticipo, referencia_proveedor, id_servicio,
                id_segmento, id_ruta, fecha, id_estatus, id_factura, id_usuario, true);
        }
        /// <summary>
        /// Método que realiza una busqueda si un registro de actualizó correctamente
        /// </summary>
        /// <param name="id_reporte_unidad_foranea">Identificador principal para la busqueda de la actualizacion sobre los registros </param>
        /// <returns></returns>
        public bool ActualizaCarteraProveedor()
        {
            return this.cargaAtributos(this._id_cartera_proveedor);
        }

        /// <summary>
        /// Método encargado de editar el atributo "Habilitar" del registro, manteniendo el valor del resto de los atributos
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCarteraProveedor(int id_usuario)
        {
            return this.editaCarteraProveedor(this.id_proveedor, this.id_producto, this.id_tipo, this.litros, this.precio_unitario, this.monto, this.id_anticipo, 
                this.referencia_proveedor, this.id_servicio, this.id_segmento, this.id_ruta, this.fecha, this.id_estatus, this.id_factura, id_usuario, false);
        }
        #endregion
    }
}
