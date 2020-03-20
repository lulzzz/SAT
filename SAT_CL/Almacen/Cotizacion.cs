using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// Clase que permite realizar inserciones; actualizaciones sobre los registros de base de datos pertenecientes a la tabla cotizacion
    /// </summary>
    public class Cotizacion:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributoq ue almacena el nombre del store procedure de la tabla cotización
        /// </summary>
        private static string nom_sp = "almacen.sp_cotizacion_tc";
        private int _id_cotizacion;
        /// <summary>
        /// Id que identifica una cotización
        /// </summary>
        public int id_cotizacion
        {
            get { return _id_cotizacion; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Id que permite identifiacr a una compañia emisora
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private int _no_requisicion;
        /// <summary>
        /// Permite identificar un numero de requisisción de una cotización.
        /// </summary>
        public int no_requisicion
        {
            get { return _no_requisicion; }
        }
        private int _id_proveedor;
        /// <summary>
        /// Id que permite identificar al proveedor de productos de cada compañia
        /// </summary>
        public int id_proveedor
        {
            get { return _id_proveedor; }
        }
        private int _id_producto;
        /// <summary>
        /// Permte identificar el producto de una cotización
        /// </summary>
        public int id_producto
        {
            get { return _id_producto; }
        }
        private decimal _cantidad;
        /// <summary>
        /// Permite definir el numero de productos de una cotización.
        /// </summary>
        public decimal cantidad
        {
            get { return _cantidad; }
        }
        private decimal _precio;
        /// <summary>
        /// Permite alamcenar el precio de una cotización del producto.
        /// </summary>
        public decimal precio
        {
            get { return _precio; }
        }
        private DateTime _fecha_cotizacion;
        /// <summary>
        /// Permite almacenar la fecha en la que se realizo la cotizacion
        /// </summary>
        public DateTime fecha_cotizacion
        {
            get { return _fecha_cotizacion; }
        }
        private byte _id_moneda;
        /// <summary>
        /// Id que permite definir un tipo de moneda (pesos, dolar,etc.)
        /// </summary>
        public byte id_moneda
        {
            get { return _id_moneda; }
        }
        private int _dias_vigencia;
        /// <summary>
        /// Permite almacenar los dias de vigencia de una cotización
        /// </summary>
        public int dias_vigencia
        {
            get { return _dias_vigencia; }
        }
        private int _dias_entrega;
        /// <summary>
        /// Almacena los dias en los que estara disponible el producto.
        /// </summary>
        public int dias_entrega
        {
            get { return _dias_entrega; }
        }
        private string _comentario;
        /// <summary>
        /// Alamcena descripciones referentes a la cotización.
        /// </summary>
        public string comentario
        {
            get { return _comentario; }
        }
        private bool _habilitar;
        /// <summary>
        /// Permite almacenar el cambio de estado de habilitación de un registro. 
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// cosntructor por default que inicializa los atributos en 0.
        /// </summary>
        public Cotizacion()
        {
            this._id_cotizacion = 0;
            this._id_compania_emisor = 0;
            this._no_requisicion = 0;
            this._id_proveedor = 0;
            this._id_producto = 0;
            this._cantidad = 0.0m;
            this._precio = 0.0m;
            this._fecha_cotizacion = DateTime.MinValue;
            this._id_moneda = 0;
            this._dias_vigencia = 0;
            this._dias_entrega = 0;
            this._comentario = "";
            this._habilitar = false;

        }
        /// <summary>
        /// Costructor que inicializara los atributos a aprtir de un id_cotizacion
        /// </summary>
        /// <param name="id_cotizacion">Id que perite identificar un numero de cotizacion en la tabla cotización</param>
        public Cotizacion(int id_cotizacion)
        {
            //Invoca al método carga atributos. 
            cargaAtributos(id_cotizacion);
        }
        /// <summary>
        /// Constructor que permte la consulta de una cotización a partir de un id_compania_emisor y un Id_producto
        /// </summary>
        /// <param name="id_compania_emisor">Id que permite identificar a una compania</param>
        /// <param name="id_producto">Id que permite identificar a un producto de la cotización.</param>
        public Cotizacion(int id_compania_emisor, int id_producto)
        {
            //Invoca al método cargaatrubutos
            cargaAtributos(id_compania_emisor, id_producto);
        }
        #endregion

        #region Destructor
        ~Cotizacion()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que permite la inicialización de los atributos a partir de un id_cotización
        /// </summary>
        /// <param name="id_cotización">Permite refereciar al registro de cotización para su asignación a los atributos</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_cotizacion)
        {
            //creación del onjeto retorno
            bool retorno = false;
            //Creación del arreglo de tipo objeto, con los parametros necesarios para consultar a la base de datos y estraer el registro requerido.
            object[] param = { 3, id_cotizacion, 0, 0, 0, 0, 0.0m, 0.0m, null, 0, 0, 0, "", 0, false, "", "" };
            //Invoca y asigna los valores del arreglo y el atributo con el nombre del sp al metodo encargado de realizar las transacciones a la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos almacenados en el dataset y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre cada fila del dataset y asigna a los atributos el valor de variable r con los datos encontrados
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_cotizacion = id_cotizacion;
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._no_requisicion = Convert.ToInt32(r["NoRequisicion"]);
                        this._id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        this._id_producto = Convert.ToInt32(r["IdProducto"]);
                        this._cantidad = Convert.ToInt32(r["Cantidad"]);
                        this._precio = Convert.ToInt32(r["Precio"]);
                        this._fecha_cotizacion = Convert.ToDateTime(r["FechaCotizacion"]);
                        this._id_moneda = Convert.ToByte(r["IdMoneda"]);
                        this._dias_vigencia = Convert.ToInt32(r["DiasVigencia"]);
                        this._dias_entrega = Convert.ToInt32(r["DiasEntrega"]);
                        this._comentario = Convert.ToString(r["Comentario"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la validación de los datos.
                    retorno = true;
                }
            }
            //Retornal el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que permitira la consulta de una cotización dado el id_compania_emisor y el id_producto
        /// </summary>
        /// <param name="id_compania_emisor">Id que permite identificar a la compañia </param>
        /// <param name="id_producto">Id que permite identificar al producto de una cotización</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_compania_emisor, int id_producto)
        {
            //creación del onjeto retorno
            bool retorno = false;
            //Creación del arreglo de tipo objeto, con los parametros necesarios para consultar a la base de datos y estraer el registro requerido.
            object[] param = { 3, 0, id_compania_emisor, 0, 0, id_producto, 0.0m, 0.0m, null, 0, 0, 0, "", 0, false, "", "" };
            //Invoca y asigna los valores del arreglo y el atributo con el nombre del sp al metodo encargado de realizar las transacciones a la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos almacenados en el dataset y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre cada fila del dataset y asigna a los atributos el valor de variable r con los datos encontrados
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_cotizacion = 0; 
                        _id_compania_emisor = id_compania_emisor;
                        _no_requisicion = Convert.ToInt32(r["NoRequisicion"]);
                        _id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        _id_producto = id_producto;
                        _cantidad = Convert.ToInt32(r["Cantidad"]);
                        _precio = Convert.ToInt32(r["Precio"]);
                        _fecha_cotizacion = Convert.ToDateTime(r["FechaCotizacion"]);
                        _id_moneda = Convert.ToByte(r["IdMoneda"]);
                        _dias_vigencia = Convert.ToInt32(r["DiasVigencia"]);
                        _dias_entrega = Convert.ToInt32(r["DiasEntrega"]);
                        _comentario = Convert.ToString(r["Comentarios"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la validación de los datos.
                    retorno = true;
                }
            }
            //Retornal el objeto retorno al método
            return retorno;

        }
        /// <summary>
        /// Método que permite la actualizacion de los campos de un registro que pertenece a una cotización.
        /// </summary>        
        /// <param name="no_requisicion">Permite la actualización del número de requisición de una cotización</param>
        /// <param name="id_proveedor">Permite la actualización del identifiacdor del proveedor de productos</param>
        /// <param name="id_producto">Permite la actualización del identificador del producto </param>
        /// <param name="cantidad">Permite la actualización de la cantidad de productos de una cotización</param>
        /// <param name="precio">Permite la actualización del precio de una cotización</param>
        /// <param name="fecha_cotizacion">Permite la actualización de la fecha de cotización</param>
        /// <param name="id_moneda">Permite la actualización del tipo de moneda de la cotización (pesos,dolar,etc.)</param>
        /// <param name="dias_vigencia">Permite actualizar los dias de vigencia de una cotización </param>
        /// <param name="dias_entrega">Permite actualizar los dias de entrega de una cotización</param>
        /// <param name="comentario">Permite actualizar los camentarios de una cotización.</param>
        /// <param name="id_usuario">Permite alctualizar el identificador del usuario que realizo la ultima accion sobre el registro</param>
        /// <param name="habilitar">Permite actualizar el estado de un registro (Habilitado/Deshabilitado)</param>
        /// <returns></returns>
        private RetornoOperacion editarCotizacion(int no_requisicion, int id_proveedor, int id_producto,
                                                  decimal cantidad, decimal precio, DateTime fecha_cotizacion, byte id_moneda,
                                                  int dias_vigencia, int dias_entrega, string comentario, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo de tipo objeto que almacena los valores de los parametros del sp de la tabla cotización.
            object[] param ={2,this._id_cotizacion,0,no_requisicion,id_proveedor,id_producto,cantidad,precio,fecha_cotizacion,id_moneda,
                              dias_vigencia, dias_entrega, comentario, id_usuario, habilitar,"",""};

            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Métodoq ue permite la inserción de registros a la tabla cotización.
        /// </summary>
        /// <param name="id_compania_emisor">Permite la inserción de un identificador de una compañia</param>
        /// <param name="no_requisicion">Pemite la inserción de un numero de requisición de una cotización</param>
        /// <param name="id_proveedor">Permite la inserción de un identificador de un proveedor de producto</param>
        /// <param name="id_producto">Permite la insercion de un identificador de productos</param>
        /// <param name="cantidad">Permite la insercion de la cantidad de productos a cotizar</param>
        /// <param name="precio">Permite la insercion del precio de la cotización de productos</param>
        /// <param name="fecha_cotizacion">Permite la insercion de la fecha de cotizacion</param>
        /// <param name="id_moneda">Permite la inserción de un tipo de moneda(peso,dollar)</param>
        /// <param name="dias_vigencia">Permite la inserción de los dias de vigencia de la cotización</param>
        /// <param name="dias_entrega">Permite la inserción de los dias en los que estara disponible el producto</param>
        /// <param name="comentario">Permite la insercion de referencias de una cotización.</param>
        /// <param name="id_usuario">Permite la inserción del usuario que realizó la última modificacion al registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarCotizacion(int id_compania_emisor, int no_requisicion, int id_proveedor, int id_producto,
                                                  decimal cantidad, decimal precio, DateTime fecha_cotizacion, byte id_moneda,
                                                  int dias_vigencia, int dias_entrega, string comentario, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo de tipo objeto que alamcena el valor de los parametros necesarios para el sp de la tabla Cotización.
            object[] param = { 1, 0, id_compania_emisor, no_requisicion, id_proveedor, id_producto, cantidad, precio, fecha_cotizacion,
                               id_moneda, dias_vigencia, dias_entrega, comentario, id_usuario, true, "", "" };
            //Asignacion al objeto retorno del arreglo y el atributo con el nombre del sp, para la transacción a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retrono del objeto retrono al método
            return retorno;
        }
        /// <summary>
        /// Método que permite la actualización de los campos de una cotización.
        /// </summary>
        /// <param name="no_requisicion">Permite la actualización del número de requisición de una cotización</param>
        /// <param name="id_proveedor">Permite la actualización del identifiacdor del proveedor de productos</param>
        /// <param name="id_producto">Permite la actualización del identificador del producto </param>
        /// <param name="cantidad">Permite la actualización de la cantidad de productos de una cotización</param>
        /// <param name="precio">Permite la actualización del precio de una cotización</param>
        /// <param name="fecha_cotizacion">Permite la actualización de la fecha de cotización</param>
        /// <param name="id_moneda">Permite la actualización del tipo de moneda de la cotización (pesos,dolar,etc.)</param>
        /// <param name="dias_vigencia">Permite actualizar los dias de vigencia de una cotización </param>
        /// <param name="dias_entrega">Permite actualizar los dias de entrega de una cotización</param>
        /// <param name="comentario">Permite actualizar los camentarios de una cotización.</param>
        /// <param name="id_usuario">Permite alctualizar el identificador del usuario que realizo la ultima accion sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarCotizacion(int no_requisicion, int id_proveedor, int id_producto,
                                                  decimal cantidad, decimal precio, DateTime fecha_cotizacion, byte id_moneda,
                                                  int dias_vigencia, int dias_entrega, string comentario, int id_usuario)
        {
            //Retorna e invoca al método editarCotizacion.
            return this.editarCotizacion(no_requisicion, id_proveedor, id_producto, cantidad, precio, fecha_cotizacion, id_moneda, dias_vigencia, dias_entrega,
                                         comentario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que permite El cambio de estado de habilitación de un registro(Habilitado/Deshabilitado)
        /// </summary>
        /// <param name="id_usuario">Permite Identificar al ultimo usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCotizacion(int id_usuario)
        {
            //Invoca y retorna  al método editarCotizacón.
            return this.editarCotizacion(this.no_requisicion, this.id_proveedor, this.id_producto, this.cantidad, this.precio, this.fecha_cotizacion,
                                         this.id_moneda, this._dias_vigencia, this.dias_entrega, this.comentario, id_usuario, false);
        }
        #endregion

    }
}
