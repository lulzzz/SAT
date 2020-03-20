using System;
using System.Data;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// 
    /// </summary>
    public class Producto : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Describe la moneda del producto
        /// </summary>
        public enum Moneda
        {
            /// <summary>
            /// Peso Mexicano
            /// </summary>
            Peso = 1,
            /// <summary>
            /// Dolar Americano
            /// </summary>
            Dolar
        }
        /// <summary>
        /// Describe el estatus de un producto
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// El producto puede estar en esatado activo
            /// </summary>
            Activo = 1,
            /// <summary>
            /// El producto puede estar en estatus Inactivo
            /// </summary>
            Inactivo = 2
        }
        #endregion

        #region Propiedades y atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD.
        /// </summary>
        public static string nombre_procedimiento_almacenado = "almacen.sp_producto_tpr";

        //Propiedades y atributos
        private int _id_producto;
        /// <summary>
        /// Identificador de un producto.
        /// </summary>
        public int id_producto
        {
            get { return _id_producto; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Permite identificar a una compañia  y asociarlo con el producto.
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private string _sku;
        /// <summary>
        /// Describe el codigo de un producto.
        /// </summary>
        public string sku
        {
            get { return _sku; }
        }
        private string _descripcion;
        /// <summary>
        /// Nombre o caracteristicas de un producto.
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private int _id_unidad;
        /// <summary>
        /// Unidad de medidad de un producto.
        /// </summary>
        public int id_unidad
        {
            get { return _id_unidad; }
        }
        private int _categoria;
        /// <summary>
        /// Esatablece la separacion de los productos.
        /// </summary>
        public int categoria
        {
            get { return _categoria; }
        }
        private string _fabricante;
        /// <summary>
        /// Nombre del fabricante del producto.
        /// </summary>
        public string fabricante
        {
            get { return _fabricante; }
        }
        private int _garantia;
        /// <summary>
        /// Tiempo en el que producto tiene para presentar quejas por defectos del mismo (meses).
        /// </summary>
        public int garantia
        {
            get { return _garantia; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Permite definir el estado de un producto Activo o Inactivo.
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        /// <summary>
        /// Accede a los elementos de la enumeración estatus.
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this._id_estatus; }
        }
        private byte _id_moneda_entrada;
        /// <summary>
        /// Define el tipo de moneda de entrada (Pesos o Dolar)
        /// </summary>
        public byte id_moneda_entrada
        {
            get { return _id_moneda_entrada; }
        }
        private decimal _precio_entrada;
        /// <summary>
        /// Cantidad monetaria que representa el precio de entrada de un producto.
        /// </summary>
        public decimal precio_entrada
        {
            get { return _precio_entrada; }
        }

        private byte _id_moneda_salida;
        /// <summary>
        /// Define el tipo de moneda de salida (Pesos o Dolar).
        /// </summary>
        public byte id_moneda_salida
        {
            get { return _id_moneda_salida; }
        }
        private decimal _precio_salida;
        /// <summary>
        /// Cantidad monetaria que reperesenta el precio de salida de un producto.
        /// </summary>
        public decimal precio_salida
        {
            get { return _precio_salida; }
        }      
        private decimal _cantidad_mayoreo;
        /// <summary>
        /// Cantidad de producto por mayoreo.
        /// </summary>
        public decimal cantidad_mayoreo
        {
            get { return _cantidad_mayoreo; }
        }
        private decimal _precio_salida_mayoreo;
        /// <summary>
        /// Costo del producto por mayoreo.
        /// </summary>
        public decimal precio_salida_mayoreo
        {
            get { return _precio_salida_mayoreo; }
        }
        private decimal _cantidad_minima;
        /// <summary>
        /// Cantidad minima de producto en existencia.
        /// </summary>
        public decimal cantidad_minima
        {
            get { return _cantidad_minima; }
        }
        private decimal _cantidad_maxima;
        /// <summary>
        /// Cantidad maxima de producto en existencia.
        /// </summary>
        public decimal cantidad_maxima
        {
            get { return _cantidad_maxima; }
            set { _cantidad_maxima = value; }
        }
        private int _id_producto_contenido;
        /// <summary>
        /// Producto derivado de un producto previo.
        /// </summary>
        public int id_producto_contenido
        {
            get { return _id_producto_contenido; }
        }
        private decimal _cantidad_contenido;
        /// <summary>
        /// Cantidad de producto contenido
        /// </summary>
        public decimal cantidad_contenido
        {
            get { return _cantidad_contenido; }
        }
        private bool _bit_sin_inventario;
        /// <summary>
        /// Define si el producto esta en inventario o no.
        /// </summary>
        public bool bit_sin_inventario
        {
            get { return _bit_sin_inventario; }
        }
        private bool _habilitar;
        /// <summary>
        /// Permite el cambio de estado de un registro (Habilitado / Deshabilitado)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor default
        /// </summary>
        public Producto()
        {
            this._id_producto = 0;
            this._id_compania_emisor = 0;
            this._sku = "";
            this._descripcion = "";
            this._id_unidad = 0;
            this._categoria = 0;
            this._fabricante = "";
            this._garantia = 0;
            this._id_estatus = 0;
            this._id_moneda_entrada = 0;
            this._precio_entrada = 0.0m;
            this._id_moneda_salida = 0;
            this._precio_salida = 0.0m;
            this._cantidad_mayoreo = 0.0m;
            this._precio_salida_mayoreo = 0.0m;
            this._cantidad_minima = 0.0m;
            this._cantidad_maxima = 0.0m;
            this._id_producto_contenido = 0;
            this._cantidad_contenido = 0.0m;
            this._bit_sin_inventario = false;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa la instancia en relacion al id de la cotizacion
        /// </summary>
        /// <param name="IdProducto"></param>
        public Producto(int IdProducto)
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, IdProducto, 0, "", "", 0, 0, "", 0, 0, 0, 0.0, 0, 0.0,  0.0, 0.0, 0.0, 0.0, 0, 0.0, false, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento_almacenado, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_producto = IdProducto;
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._sku = r["SKU"].ToString();
                        this._descripcion = r["Descripcion"].ToString();
                        this._id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        this._categoria = Convert.ToInt32(r["Categoria"]);
                        this._fabricante = r["Fabricante"].ToString();
                        this._garantia = Convert.ToInt32(r["Garantia"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_moneda_entrada = Convert.ToByte(r["IdMonedaEntrada"]);
                        this._precio_entrada = Convert.ToDecimal(r["PrecioEntrada"]);
                        this._id_moneda_salida = Convert.ToByte(r["IdMonedaSalida"]);
                        this._precio_salida = Convert.ToDecimal(r["PrecioSalida"]);
                        this._cantidad_mayoreo = Convert.ToDecimal(r["CantidadMayoreo"]);
                        this._precio_salida_mayoreo = Convert.ToDecimal(r["PrecioSalidaMayoreo"]);
                        this._cantidad_minima = Convert.ToDecimal(r["CantidadMinima"]);
                        this._cantidad_maxima = Convert.ToDecimal(r["CantidadMaxima"]);
                        this._id_producto_contenido = Convert.ToInt32(r["IdProductoContenido"]);
                        this._cantidad_contenido = Convert.ToDecimal(r["CantidadContenido"]);
                        this._bit_sin_inventario = Convert.ToBoolean(r["BitSinInventario"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }


                }
            }

        }


        #endregion

        #region Destructores

        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~Producto()
        {
            Dispose(false);
        }

        #endregion

        #region Metodos privados

        /// <summary>
        /// Método que permite actualizar los campos de un registro producto
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el identificador de una compañia</param>
        /// <param name="sku">Actualiza el codigo de un producto</param>
        /// <param name="descripcion">Actualiza las caracteristicas de un producto</param>
        /// <param name="id_unidad">Actualiza la unidad de medida del producto</param>
        /// <param name="categoria">Actualiza la categoria de un producto</param>
        /// <param name="fabricante">Actualiza el nombre del fabricante de un producto</param>
        /// <param name="garantia">Actualiza los meses de garantia de un producto</param>
        /// <param name="estatus">Actualiza el esattus del producto (Activo, Inactivo)</param>
        /// <param name="id_moneda_entrada">Actualiza el tipo de moneda entrada de un producto (Peso o Dolar)</param>
        /// <param name="precio_entrada">Actualiza el precio de entrada de un producto</param>
        /// <param name="id_moneda_salida">Actualiza el tipo de moneda salida de un producto (Peso o Dolar)</param>
        /// <param name="precio_salida">Actualiza el precio de salida de un producto</param>
        /// <param name="cantidad_mayoreo">Actualiza la mantidad por mayoreo de un producto</param>
        /// <param name="precio_salida_mayoreo">Actualiza el precio de salida por mayoreo de un producto</param>
        /// <param name="cantidad_minima">Actualiza la cantidad minima de existencia de un producto</param>
        /// <param name="cantidad_maxima">Actualiza la cantidad maxima de existencia de un producto</param>
        /// <param name="id_producto_contenido">Actualiza el identificador de la derivación de un producto</param>
        /// <param name="cantidad_contenido">Actualiza la cantidad del producto contenido</param>
        /// <param name="bit_sin_inventario">Actualiza si existe en inventario o no</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de habilitación de un registro(Habilitado / Deshabilitado)</param>
        /// <returns></returns>
        private RetornoOperacion editaRegistroProducto(int id_compania_emisor, string sku, string descripcion, int id_unidad, int categoria, string fabricante,
                                                        int garantia, Estatus estatus, byte id_moneda_entrada, decimal precio_entrada, byte id_moneda_salida,
                                                        decimal precio_salida,  decimal cantidad_mayoreo, decimal precio_salida_mayoreo, decimal cantidad_minima, 
                                                        decimal cantidad_maxima, int id_producto_contenido, decimal cantidad_contenido, bool bit_sin_inventario, 
                                                        int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Inicializando arreglo de parámetros
            object[] param = {2, this._id_producto,id_compania_emisor, sku, descripcion,id_unidad,categoria,fabricante,garantia,(byte) estatus, id_moneda_entrada,
                              precio_entrada, id_moneda_salida,precio_salida, cantidad_mayoreo,precio_salida_mayoreo,
                              cantidad_minima,cantidad_maxima,id_producto_contenido,cantidad_contenido,bit_sin_inventario, id_usuario, habilitar, "", ""};

            //Asigna valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento_almacenado, param);
            //Realizando actualizacion
            return retorno;
        }

        #endregion

        #region Metodos Publicos (interfaz)

        /// <summary>
        /// Método que permte insertar registros en la tabla producto.
        /// </summary>
        /// <param name="id_compania_emisor">Inserta el identificador de una compañia</param>
        /// <param name="sku">Inserta el codigo de un producto</param>
        /// <param name="descripcion">Inserta la descripcion de un producto</param>
        /// <param name="id_unidad">Inserta el tipo de unidad de medida</param>
        /// <param name="categoria">Inserta la categoria de un producto</param>
        /// <param name="fabricante">Inserta el nombre del fabricante del producto</param>
        /// <param name="garantia">Inserta los meses de garantia de un producto</param>
        /// <param name="estatus">Inserta el estatus de un producto(Activo / Inactivo)</param>
        /// <param name="id_moneda_entrada">Inserta el tipo de modeda de un producto entrada (Peso / Dolar)</param>
        /// <param name="precio_entrada">Inserta el precio de entrada de un producto</param>
        /// <param name="id_moneda_salida">Inserta el tipo de modena de un producto salida (Peso / Salida)</param>
        /// <param name="precio_salida">Inserta el precio de salida de un producto</param>
        /// <param name="cantidad_mayoreo">Inserta la cantida por mayoreo de un producto</param>
        /// <param name="precio_salida_mayoreo">Inserta el precio por mayoreo de un producto</param>
        /// <param name="cantidad_minima">Inserta la cantidad minima de un producto</param>
        /// <param name="cantidad_maxima">Inserta la cantidad maxima de un producto</param>
        /// <param name="id_producto_contenido">Inserta el identificador si es derivado de un producto</param>
        /// <param name="cantidad_contenido">Inserta la cantidad del producto contenido</param>
        /// <param name="bit_sin_inventario">Inserta un 1 si el producto existe en inventario o un 0 si no existe</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que relizo la inserción del registro.</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaProducto( int id_compania_emisor, string sku, string descripcion, int id_unidad, int categoria, string fabricante,
                                                        int garantia, Estatus estatus, byte id_moneda_entrada, decimal precio_entrada,byte id_moneda_salida, 
                                                        decimal precio_salida,  decimal cantidad_mayoreo,decimal precio_salida_mayoreo, decimal cantidad_minima, 
                                                        decimal cantidad_maxima, int id_producto_contenido,decimal cantidad_contenido,bool bit_sin_inventario,
                                                        int id_usuario)
        {
            //Creción del objeto retrono
            RetornoOperacion retorno = new RetornoOperacion();
            //Inicializamos el arreglo de parametros
            object[] param = { 1, 0, id_compania_emisor, sku, descripcion,id_unidad,categoria,fabricante,garantia,(byte)estatus,id_moneda_entrada,
                              precio_entrada,id_moneda_salida,precio_salida,cantidad_mayoreo,precio_salida_mayoreo,cantidad_minima,cantidad_maxima,
                              id_producto_contenido,cantidad_contenido,bit_sin_inventario,id_usuario, true, "", "" };

            //Realizamos la inserción del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento_almacenado, param);
            //Retorna el resultado al método
            return retorno;
        }
        /*// <summary>
        /// Metodo encargado de la insercion de un Producto
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="descripcion"></param>
        /// <param name="familia_1"></param>
        /// <param name="familia_2"></param>
        /// <param name="familia_3"></param>
        /// <param name="familia_4"></param>
        /// <param name="cantidad_contenido"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tipo_separacion"></param>
        /// <param name="cantidad_minima"></param>
        /// <param name="cantidad_maxima"></param>
        /// <param name="renovable"></param>
        /// <param name="separar_entrada_inventario"></param>
        /// <param name="precio_salida"></param>
        /// <param name="precio_mayoreo"></param>
        /// <param name="id_moneda"></param>
        /// <param name="cantidad_mayoreo"></param>
        /// <param name="bit_sin_inventario"></param>
        /// <param name="precio_referencia1"></param>
        /// <param name="precio_referencia2"></param>
        /// <param name="precio_referencia3"></param>
        /// <param name="id_usuario"></param>
        /// <param name="idProductoDerivado"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaProducto(string sku, string descripcion, int familia_1,
           int familia_2, int familia_3, int familia_4, int cantidad_contenido, int id_unidad, TipoSeparacion id_tipo_separacion,
           int cantidad_minima, int cantidad_maxima, bool renovable, bool separar_entrada_inventario, double precio_salida,
           double precio_mayoreo, Moneda id_moneda, int cantidad_mayoreo, bool bit_sin_inventario, double precio_referencia1,
           double precio_referencia2, double precio_referencia3, int id_usuario, int idProductoDerivado)
        {
            //Declaracion de objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion();
            //declarando metodo de transacción
            SqlTransaction transaccion = CapaDatos.m_capaDeDatos.InicializaTransaccionSQL(IsolationLevel.Serializable);

            //Inicializamos el arreglo de parametros
            object[] param = { 1, 0, sku, descripcion, familia_1, familia_2, familia_3, familia_4, cantidad_contenido, id_unidad, (int)id_tipo_separacion, cantidad_minima, cantidad_maxima, renovable, separar_entrada_inventario, precio_salida, precio_mayoreo, (int)id_moneda, cantidad_mayoreo, bit_sin_inventario, precio_referencia1, precio_referencia2, precio_referencia3, id_usuario, true, "", "" };

            //Realizamos la inserción del registro
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento_almacenado, param, transaccion);                        

            //Validamos que la edicion del producto haya sido exitosa y que haya registrado un producto derivado
            if (resultado.OperacionExitosa && idProductoDerivado != 0)
            {
                    resultado = clProductoDerivado.InsertaProductoDerivado(resultado.IdRegistro, idProductoDerivado, id_usuario, transaccion);                                                                                            
            }            
            //Terminamos la transaccion 
            CapaDatos.m_capaDeDatos.FinalizaTransaccionSQL(transaccion, resultado.OperacionExitosa);

            //Regresamos el resultado 
            return resultado;
        }
        /// <summary>
        /// Metodo encargado de insertar un producto, su producto derivado y su tipo referencia
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="descripcion"></param>
        /// <param name="familia_1"></param>
        /// <param name="familia_2"></param>
        /// <param name="familia_3"></param>
        /// <param name="familia_4"></param>
        /// <param name="cantidad_contenido"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tipo_separacion"></param>
        /// <param name="cantidad_minima"></param>
        /// <param name="cantidad_maxima"></param>
        /// <param name="renovable"></param>
        /// <param name="separar_entrada_inventario"></param>
        /// <param name="precio_salida"></param>
        /// <param name="precio_mayoreo"></param>
        /// <param name="id_moneda"></param>
        /// <param name="cantidad_mayoreo"></param>
        /// <param name="bit_sin_inventario"></param>
        /// <param name="precio_referencia1"></param>
        /// <param name="precio_referencia2"></param>
        /// <param name="precio_referencia3"></param>
        /// <param name="id_usuario"></param>
        /// <param name="idProductoDerivado"></param>
        /// <param name="tipoReferencia"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaProducto(string sku, string descripcion, int familia_1,
           int familia_2, int familia_3, int familia_4, int cantidad_contenido, int id_unidad, TipoSeparacion id_tipo_separacion,
           int cantidad_minima, int cantidad_maxima, bool renovable, bool separar_entrada_inventario, double precio_salida,
           double precio_mayoreo, Moneda id_moneda, int cantidad_mayoreo, bool bit_sin_inventario, double precio_referencia1,
           double precio_referencia2, double precio_referencia3, int id_usuario, int idProductoDerivado, TipoReferencia tipoReferencia)
        {
            //Declaracion de objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion();
            RetornoOperacion resultadoProducto = new RetornoOperacion();
            //declarando metodo de transacción
            SqlTransaction transaccion = CapaDatos.m_capaDeDatos.InicializaTransaccionSQL(IsolationLevel.Serializable);            
            //Inicializamos el arreglo de parametros
            object[] param = { 1, 0, sku, descripcion, familia_1, familia_2, familia_3, familia_4, cantidad_contenido, id_unidad, (int)id_tipo_separacion, cantidad_minima, cantidad_maxima, renovable, separar_entrada_inventario, precio_salida, precio_mayoreo, (int)id_moneda, cantidad_mayoreo, bit_sin_inventario, precio_referencia1, precio_referencia2, precio_referencia3, id_usuario, true, "", "" };

            //Realizamos la inserción del registro
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento_almacenado, param, transaccion);
            if (resultado.OperacionExitosa)
            {
                //guardando resultado de el registro del producto en objeto de retorno
                resultadoProducto = resultado;
                if (idProductoDerivado != 0)
                {
                    resultado = clProductoDerivado.InsertaProductoDerivado(resultado.IdRegistro, idProductoDerivado, id_usuario, transaccion);
                }
            }
            //Validamos que la edicion del producto haya sido exitosa y que haya registrado un producto derivado
            
            //Guardamos referencia es sim 
            if (resultado.OperacionExitosa && tipoReferencia != TipoReferencia.NoAplica)
            {
                //guardamos referencia
                resultado = BibliotecaClasesSICDB.clReferencia.Guarda(BibliotecaClasesSICDB.clReferencia.AccionReferencia.Registrar, Convert.ToInt32(tipoReferencia), 0, "true", resultadoProducto.IdRegistro, 33, id_usuario, transaccion);
            }
            //Terminamos la transaccion 
            CapaDatos.m_capaDeDatos.FinalizaTransaccionSQL(transaccion, resultado.OperacionExitosa);
            //Regresamos el resultado
            if (resultado.OperacionExitosa)
            { return resultadoProducto; }
            else { return resultado; }            
        }*/
        /// <summary>
        /// Método que permite actualizar los campos de un registro producto
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el identificador de una compañia</param>
        /// <param name="sku">Actualiza el codigo de un producto</param>
        /// <param name="descripcion">Actualiza las caracteristicas de un producto</param>
        /// <param name="id_unidad">Actualiza la unidad de medida del producto</param>
        /// <param name="categoria">Actualiza la categoria de un producto</param>
        /// <param name="fabricante">Actualiza el nombre del fabricante de un producto</param>
        /// <param name="garantia">Actualiza los meses de garantia de un producto</param>
        /// <param name="estatus">Actualiza el esattus del producto (Activo, Inactivo)</param>
        /// <param name="id_moneda_entrada">Actualiza el tipo de moneda entrada de un producto (Peso o Dolar)</param>
        /// <param name="precio_entrada">Actualiza el precio de entrada de un producto</param>
        /// <param name="id_moneda_salida">Actualiza el tipo de moneda salida de un producto (Peso o Dolar)</param>
        /// <param name="precio_salida">Actualiza el precio de salida de un producto</param>
        /// <param name="cantidad_mayoreo">Actualiza la mantidad por mayoreo de un producto</param>
        /// <param name="precio_salida_mayoreo">Actualiza el precio de salida por mayoreo de un producto</param>
        /// <param name="cantidad_minima">Actualiza la cantidad minima de existencia de un producto</param>
        /// <param name="cantidad_maxima">Actualiza la cantidad maxima de existencia de un producto</param>
        /// <param name="id_producto_contenido">Actualiza el identificador de la derivación de un producto</param>
        /// <param name="cantidad_contenido">Actualiza la cantidad del producto contenido</param>
        /// <param name="bit_sin_inventario">Actualiza si existe en inventario o no</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        public RetornoOperacion EditaProducto(int id_compania_emisor, string sku, string descripcion, int id_unidad, int categoria, string fabricante,
                                                int garantia, Estatus estatus, byte id_moneda_entrada, decimal precio_entrada, byte id_moneda_salida,
                                                decimal precio_salida, decimal cantidad_mayoreo,decimal precio_salida_mayoreo, decimal cantidad_minima,
                                                decimal cantidad_maxima, int id_producto_contenido,decimal cantidad_contenido,  bool bit_sin_inventario,
                                                int id_usuario)
                                                 
        {

            //Realizamos la edicion del registro
            return editaRegistroProducto(id_compania_emisor, sku, descripcion, id_unidad, categoria, fabricante, garantia, (Estatus)estatus, id_moneda_entrada, precio_entrada,
                                          id_moneda_salida, precio_salida,  cantidad_mayoreo, precio_salida_mayoreo, cantidad_minima,
                                         cantidad_maxima, id_producto_contenido, cantidad_contenido, bit_sin_inventario, id_usuario, this.habilitar);

        }
        /*// <summary>
        /// Metodo encargado de editar el producto y su producto derivado
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="descripcion"></param>
        /// <param name="familia_1"></param>
        /// <param name="familia_2"></param>
        /// <param name="familia_3"></param>
        /// <param name="familia_4"></param>
        /// <param name="cantidad_contenido"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tipo_separacion"></param>
        /// <param name="cantidad_minima"></param>
        /// <param name="cantidad_maxima"></param>
        /// <param name="renovable"></param>
        /// <param name="separar_entrada_inventario"></param>
        /// <param name="precio_salida"></param>
        /// <param name="precio_mayoreo"></param>
        /// <param name="id_moneda"></param>
        /// <param name="cantidad_mayoreo"></param>
        /// <param name="bit_sin_inventario"></param>
        /// <param name="precio_referencia1"></param>
        /// <param name="precio_referencia2"></param>
        /// <param name="precio_referencia3"></param>
        /// <param name="id_usuario"></param>
        /// <param name="idProductoHijo"></param>
        /// <returns></returns>
        public RetornoOperacion EditaProducto(string sku, string descripcion, int familia_1,
           int familia_2, int familia_3, int familia_4, int cantidad_contenido, int id_unidad, TipoSeparacion id_tipo_separacion,
           int cantidad_minima, int cantidad_maxima, bool renovable, bool separar_entrada_inventario, double precio_salida,
           double precio_mayoreo, Moneda id_moneda, int cantidad_mayoreo, bool bit_sin_inventario, double precio_referencia1,
           double precio_referencia2, double precio_referencia3, int id_usuario, int idProductoHijo)
        {
            //Declaracion de objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion(0);
            
            //declarando metodo de transacción
            SqlTransaction transaccion = CapaDatos.m_capaDeDatos.InicializaTransaccionSQL(IsolationLevel.Serializable);
            
            //Realizamos la edicion del registro
            resultado = editaRegistroProducto(sku, descripcion, familia_1, familia_2, familia_3, familia_4, cantidad_contenido, id_unidad, (int)id_tipo_separacion,
                cantidad_minima, cantidad_maxima, renovable, separar_entrada_inventario, precio_salida, precio_mayoreo, 
                (int)id_moneda, cantidad_mayoreo, bit_sin_inventario, precio_referencia1, precio_referencia2, precio_referencia3, 
                id_usuario, this.habilitar, transaccion);
            
            //Validamos que la edicion del producto haya sido exitosa 
            if (resultado.OperacionExitosa)
            {
                if (this.id_producto_derivado == 0)
                {
                    if( idProductoHijo != 0 )
                    //Insertamos producto derivado
                    resultado = clProductoDerivado.InsertaProductoDerivado(this.id_producto, idProductoHijo, id_usuario, transaccion);
                }
                else
                {
                    using(clProductoDerivado pd = new clProductoDerivado(0, this.id_producto))
                    {
                        if (idProductoHijo == 0)
                            resultado = pd.DeshabilitaProductoDerivado(id_usuario, transaccion);
                        else
                            if (this.id_producto_derivado != idProductoHijo)
                                //Editamos el producto derivado
                                resultado = pd.EditaProductoDerivado(this.id_producto, idProductoHijo, id_usuario, transaccion);
                    }
                }              
            }
            //Terminamos la transaccion 
            CapaDatos.m_capaDeDatos.FinalizaTransaccionSQL(transaccion, resultado.OperacionExitosa);
            //Regresamos el resultado 
            return resultado;
        }
        /// <summary>
        /// Metodo encargado de realizar la edición de un producto 
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="descripcion"></param>
        /// <param name="familia_1"></param>
        /// <param name="familia_2"></param>
        /// <param name="familia_3"></param>
        /// <param name="familia_4"></param>
        /// <param name="cantidad_contenido"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tipo_separacion"></param>
        /// <param name="cantidad_minima"></param>
        /// <param name="cantidad_maxima"></param>
        /// <param name="renovable"></param>
        /// <param name="separar_entrada_inventario"></param>
        /// <param name="precio_salida"></param>
        /// <param name="precio_mayoreo"></param>
        /// <param name="id_moneda"></param>
        /// <param name="cantidad_mayoreo"></param>
        /// <param name="bit_sin_inventario"></param>
        /// <param name="precio_referencia1"></param>
        /// <param name="precio_referencia2"></param>
        /// <param name="precio_referencia3"></param>
        /// <param name="id_usuario"></param>
        /// <param name="idProductoHijo"></param>
        /// <param name="tipoReferencia"></param>
        /// <returns></returns>
        public RetornoOperacion EditaProducto(string sku, string descripcion, int familia_1,
           int familia_2, int familia_3, int familia_4, int cantidad_contenido, int id_unidad, TipoSeparacion id_tipo_separacion,
           int cantidad_minima, int cantidad_maxima, bool renovable, bool separar_entrada_inventario, double precio_salida,
           double precio_mayoreo, Moneda id_moneda, int cantidad_mayoreo, bool bit_sin_inventario, double precio_referencia1,
           double precio_referencia2, double precio_referencia3, int id_usuario, int idProductoHijo, TipoReferencia tipoReferencia)
        {
            //Declaracion de objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion(0);
            //objeto para almacenar resultado de la operacion de la edicion del producto
            RetornoOperacion resultadoProducto = new RetornoOperacion(0);

            //declarando metodo de transacción
            SqlTransaction transaccion = CapaDatos.m_capaDeDatos.InicializaTransaccionSQL(IsolationLevel.Serializable);

            //Realizamos la edicion del registro
            resultado = editaRegistroProducto(sku, descripcion, familia_1, familia_2, familia_3, familia_4, cantidad_contenido, id_unidad, (int)id_tipo_separacion,
                cantidad_minima, cantidad_maxima, renovable, separar_entrada_inventario, precio_salida, precio_mayoreo,
                (int)id_moneda, cantidad_mayoreo, bit_sin_inventario, precio_referencia1, precio_referencia2, precio_referencia3,
                id_usuario, this.habilitar, transaccion);

            //Validamos que la edicion del producto haya sido exitosa 
            if (resultado.OperacionExitosa)
            {
                //Guardando resultado de producto guardado
                resultadoProducto = resultado;
                if (this.id_producto_derivado == 0)
                {
                    if (idProductoHijo != 0)
                        //Insertamos producto derivado
                        resultado = clProductoDerivado.InsertaProductoDerivado(this.id_producto, idProductoHijo, id_usuario, transaccion);
                }
                else
                {
                    using (clProductoDerivado pd = new clProductoDerivado(0, this.id_producto))
                    {
                        if (idProductoHijo == 0)
                            resultado = pd.DeshabilitaProductoDerivado(id_usuario, transaccion);
                        else
                            if (this.id_producto_derivado != idProductoHijo)
                                //Editamos el producto derivado
                                resultado = pd.EditaProductoDerivado(this.id_producto, idProductoHijo, id_usuario, transaccion);
                    }
                }
            }
            //Guardamos referencia
            //si se registro un tipo de referencia
            if (tipoReferencia != TipoReferencia.NoAplica)
            {                
                //si no tenia referencia
                if (!es_equipo_satelital && !es_sim)
                {
                    //guardamos referencia
                    resultado = BibliotecaClasesSICDB.clReferencia.Guarda(BibliotecaClasesSICDB.clReferencia.AccionReferencia.Registrar, Convert.ToInt32(tipoReferencia), 0, "true", this.id_producto, 33, id_usuario, transaccion);
                }
                //modificamos la referencia
                else
                {
                    //si hubo cambio en el tipo de referencia
                    if ((es_equipo_satelital && tipoReferencia == TipoReferencia.EsSim) || es_sim && tipoReferencia == TipoReferencia.EsEquipoSatelital)
                    {
                        //cambiamos variable tipoReferencia del metodo para poder cargar la referencia correspondiente dado el id de producto, el grupo de referencia y el tipo de referencia que vamos a modificar
                        //ocjeto de TipoReferencia que almacena el tipo de referencia a edshabilitar
                        TipoReferencia tipoReferenciaActual = TipoReferencia.NoAplica;
                        //si es equipo satelital, guardamos tipo referencia como tal
                        if (es_equipo_satelital) { tipoReferenciaActual = TipoReferencia.EsEquipoSatelital; }
                        //de lo contrario, guardamos tipoReferencia como sim
                        else if (es_sim) { tipoReferenciaActual = TipoReferencia.EsSim; }
                        //Editamos cambiose en referencia
                        //solo debe haber un tipo de referencia es sim o es equipo satelital
                        using (DataTable ds = clReferencia.CargaReferenciasRegistro(this.id_producto, 33, Convert.ToInt32(tipoReferenciaActual)))
                        {
                            //Validando origen de datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                            {
                                //modificando registro referencia
                                foreach (DataRow r in ds.Rows)
                                {
                                    //obtenemos id referencia
                                    int idReferencia = Convert.ToInt32(r["id_referencia"]);
                                    //eliminando referencia
                                    resultado = BibliotecaClasesSICDB.clReferencia.Guarda(BibliotecaClasesSICDB.clReferencia.AccionReferencia.Eliminar, Convert.ToInt32(tipoReferenciaActual), idReferencia, "true", this.id_producto, 33, id_usuario, transaccion);
                                }
                                if (resultado.OperacionExitosa)
                                {
                                    //Guardando nueva referencia
                                    resultado = BibliotecaClasesSICDB.clReferencia.Guarda(BibliotecaClasesSICDB.clReferencia.AccionReferencia.Registrar, Convert.ToInt32(tipoReferencia), 0, "true", this.id_producto, 33, id_usuario, transaccion);
                                }
                            }
                            else
                            {
                                resultado = new RetornoOperacion("No se pudo modificar la referencia del producto", false);
                            }
                        } 
                    }                  
                }
            }
            else
            {
                //si es equipo satelital o es sim, deshabilitamos referencia
                if (es_equipo_satelital || es_sim)
                {
                    //deshabilitando referencia
                    //cambiamos variable tipoReferencia del metodo para poder cargar la referencia correspondiente dado el id de producto, el grupo de referencia y el tipo de referencia que vamos a modificar
                    //si es equipo satelital, guardamos tipo referencia como tal
                    if (es_equipo_satelital) { tipoReferencia = TipoReferencia.EsEquipoSatelital; }
                    //de lo contrario, guardamos tipoReferencia como sim
                    else if (es_sim) { tipoReferencia = TipoReferencia.EsSim; }
                    //Editamos cambiose en referencia
                    //solo debe haber un tipo de referencia es sim o es equipo satelital
                    using (DataTable ds = clReferencia.CargaReferenciasRegistro(this.id_producto, 33, Convert.ToInt32(tipoReferencia)))
                    {
                        
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                        {
                            //modificando registro referencia
                            foreach (DataRow r in ds.Rows)
                            {
                                //obtenemos id referencia
                                int idReferencia = Convert.ToInt32(r["id_referencia"]);
                                resultado = BibliotecaClasesSICDB.clReferencia.Guarda(BibliotecaClasesSICDB.clReferencia.AccionReferencia.Eliminar, Convert.ToInt32(tipoReferencia), idReferencia, "false", this.id_producto, 33, id_usuario, transaccion);
                            }
                        }
                        else
                        {
                            resultado = new RetornoOperacion("No se pudo modificar la referencia del producto", false);
                        }
                    } 
                }
            }
            //Terminamos la transaccion 
            CapaDatos.m_capaDeDatos.FinalizaTransaccionSQL(transaccion, resultado.OperacionExitosa);
            if (resultado.OperacionExitosa) 
            { //regresamos retorno del producto
                return resultadoProducto; 
            }
            else 
            { //Regresamos el resultado de error
                return resultado;
            }
            
        }*/
        /// <summary>
        /// Metodo encargado de deshabilitar el producto
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaProducto(int id_usuario)
        {
            //Instanciamos el objeto resultado
            RetornoOperacion resultado = InventarioResumen.ValidaTransaccionProductoInventario(this._id_producto);

            //Validando existencia
            if (!resultado.OperacionExitosa)

                //Retorna al método la invocación del método editaRegistroProducto
                resultado = editaRegistroProducto(this._id_compania_emisor, this._sku, this._descripcion, this._id_unidad, this._categoria, this._fabricante, this._garantia, (Estatus)this._id_estatus,
                                             this._id_moneda_entrada, this._precio_entrada, this._id_moneda_salida, this._precio_salida, this._cantidad_mayoreo, this._precio_salida_mayoreo,
                                              this._cantidad_minima, this._cantidad_maxima, this._id_producto_contenido, this._cantidad_contenido, this._bit_sin_inventario, id_usuario, false);
            else
                //Instanciando Excepción
                resultado = new RetornoOperacion(resultado.Mensaje);

            //Devuelve Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Metodo encargado de regresar la descripcion en razon a un ID proporcionado
        /// </summary>
        /// <param name="id_producto"></param>
        /// <returns></returns>
        public static string ObtenDescripcionProducto(int id_producto)
        {
            //Instanciamos el objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializamos el arreglo de parametros 
            object[] param = { 4, id_producto, 0, "", "", 0, 0, "", 0, 0, 0, 0.0, 0, 0.0, 0.0, 0.0, 0.0, 0.0, 0, 0.0, false, 0, false, "", "" };

            //Ejecutamos el store procedure 
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento_almacenado, param);

            //Validamos el resultado
            if (resultado.Retorno != null)
                return resultado.Retorno.ToString();
            else return "";
        }

        /// <summary>
        /// Metodo encargado de retornar un buscar un producto en relacion a su codigo de producto
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static DataSet BuscaProducto(string codigo)
        {
            //Inicializamos el arreglo de parametros 
            object[] param = { 5, 0, 0, codigo, "", 0, 0, "", 0, 0, 0, 0.0, 0, 0.0, 0.0, 0.0, 0.0, 0.0, 0, 0.0, false, 0, false, "", "" };

            //Ejecutamos el store procedure 
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento_almacenado, param);

        }
        /// <summary>
        /// Metodo encargado de buscar un producto en relación a su descripcion
        /// </summary>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public static DataSet BuscaProductoDescripcion(string descripcion)
        {
            //Inicializamos el arreglo de parametros 
            object[] param = { 6, 0, 0, "", descripcion, 0, 0, "", 0, 0, 0, 0.0, 0, 0.0, 0.0, 0.0, 0.0, 0.0, 0, 0.0, false, 0, false, "", "" };

            //Ejecutamos el store procedure 
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento_almacenado, param);

        }
        /// <summary>
        /// Método encargado de Obtener los Productos Contenidos por Desempaquetar
        /// </summary>
        /// <param name="id_producto">Producto a Desempaquetar</param>
        /// <returns></returns>
        public static DataTable ObtieneProductoContenidoDesempaquetar(int id_producto)
        {
            //Declarando Objeto de Retorno
            DataTable dtProductos = null;

            //Inicializamos el arreglo de parametros 
            object[] param = { 7, id_producto, 0, "", "", 0, 0, "", 0, 0, 0, 0.0, 0, 0.0, 0.0, 0.0, 0.0, 0.0, 0, 0.0, false, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento_almacenado, param))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtProductos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtProductos;
        }
        /// <summary>
        /// Método encargado de Obtener los Totales 
        /// </summary>
        /// <param name="id_producto"></param>
        /// <param name="total_existencia">Existencia del Producto en el Inventario</param>
        /// <param name="total_requerido">Total del Producto Actualmente Requerido</param>
        /// <param name="total_por_entregar">Total del Producto que esta por Entregar</param>
        public static void ObtieneTotalesProductoInventario(int id_producto, out decimal total_existencia, out decimal total_requerido,
                                                             out decimal total_por_entregar)
        {
            //Inicializando Objetos de Salida
            total_existencia = total_requerido = total_por_entregar = 0.00M;

            //Inicializamos el arreglo de parametros 
            object[] param = { 8, id_producto, 0, "", "", 0, 0, "", 0, 0, 0, 0.0, 0, 0.0, 0.0, 0.0, 0.0, 0.0, 0, 0.0, false, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento_almacenado, param))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores Obtenidos
                        decimal.TryParse(dr["TotalExistencia"].ToString(), out total_existencia);
                        decimal.TryParse(dr["TotalRequerido"].ToString(), out total_requerido);
                        decimal.TryParse(dr["TotalPorEntregar"].ToString(), out total_por_entregar);
                    }
                }
            }
        }

        #endregion
    }
}
