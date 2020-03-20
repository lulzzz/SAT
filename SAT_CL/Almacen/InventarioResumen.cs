using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// Calse que permite realizar acciones de consulta, inserción y edición de registros sobre la tabla Inventario Resumen.
    /// </summary>
    public class InventarioResumen : Disposable
    {

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp de la tabla inventario resumen.
        /// </summary>
        private static string nom_sp = "almacen.sp_inventario_resumen_tir";
        private int _id_inventario_resumen;
        /// <summary>
        /// Almacena el identificador de un registro de inventario resumen.
        /// </summary>
        public int id_inventario_resumen
        {
            get { return _id_inventario_resumen; }
        }
        private int _id_inventario;
        /// <summary>
        /// Almacen el identificador que hace referencia a un registro de inventario.
        /// </summary>
        public int id_inventario
        {
            get { return _id_inventario; }
        }
        private int _id_producto;
        /// <summary>
        /// Almacena el identificador que hace referencia a un registros de un producto.
        /// </summary>
        public int id_producto
        {
            get { return _id_producto; }
        }
        private int _id_almacen;
        /// <summary>
        /// Almacena el identificador que hace referencia a un registro de almacen.
        /// </summary>
        public int id_almacen
        {
            get { return _id_almacen; }
        }
        private decimal _cantidad_actual;
        /// <summary>
        /// Almacena el numero actual de producto en almacen.
        /// </summary>
        public decimal cantidad_actual
        {
            get { return _cantidad_actual; }
        }
        private DateTime _fecha_inventario_resumen;
        /// <summary>
        /// Almacena la fecha de actualización del resumen inventario.
        /// </summary>
        public DateTime fecha_inventario_resumen
        {
            get { return _fecha_inventario_resumen; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacen el estado de habilitación de un registro (habilitado o deshabilitado)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que inicializa los atributos a 0.
        /// </summary>
        public InventarioResumen()
        {
            this._id_inventario_resumen = 0;
            this._id_inventario = 0;
            this._id_producto = 0;
            this._id_almacen = 0;
            this._cantidad_actual = 0.0m;
            this._fecha_inventario_resumen = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos de la clase con un registro determinado.
        /// </summary>
        /// <param name="id_inventario_resumen">Id que sirve para iniciar la busqueda de registro</param>
        public InventarioResumen(int id_inventario_resumen)
        {
            //Invoca el método cargaAtibutos.
            cargaAtributos(id_inventario_resumen);
        }
        /// <summary>
        /// Constructor que inicializa los atributos con un registro determinado por el producto y almacen
        /// </summary>
        /// <param name="id_producto">Identificador de un producto sirve para iniciar la busqueda de registro</param>
        /// <param name="id_almacen">Identificador de un almacen sirve para iniciar la busqueda de registro</param>
        public InventarioResumen(int id_producto, int id_almacen)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_producto, id_almacen);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~InventarioResumen()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que  realiza la busqueda de un registro y almacena el resultado en los atributos.
        /// </summary>
        /// <param name="id_inventario_resumen">Id que sirve como referencia para la busuqeda de registro</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_inventario_resumen)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para la consulta de registro InventarioResumen
            object[] param = { 3, id_inventario_resumen, 0, 0, 0, 0.0, null, 0, false, "", "" };
            //Invoca al método EjecutaProcAlmacenadoDataSet y asigna el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del DS (que existan y no sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas y almacena el resultado en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_inventario_resumen = id_inventario_resumen;
                        this._id_inventario = Convert.ToInt32(r["IdInventario"]);
                        this._id_producto = Convert.ToInt32(r["IdProducto"]);
                        this._id_almacen = Convert.ToInt32(r["IdAlmacen"]);
                        this._cantidad_actual = Convert.ToDecimal(r["CantidadActual"]);
                        this._fecha_inventario_resumen = Convert.ToDateTime(r["FechaInventario"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
                //Cambia de valor al objeto retorno siempre y cuando se cumpla de validación de datos.
                retorno = true;
            }
            //Retorna el método al resultado
            return retorno;
        }
        /// <summary>
        /// Método que realiza la busqueda de un registro dado un producto y almacen y almacena el resultado en los atributos.
        /// </summary>
        /// <param name="id_producto">Identificador de un producto que sirve como referencia para la busqueda de registro</param>
        /// <param name="id_almacen">Identificador de un almacen que sirve como referencia para la busqueda de registro</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_producto, int id_almacen)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para la consulta de registro InventarioResumen
            object[] param = { 4, 0, 0, id_producto, id_almacen, 0.0, null, 0, false, "", "" };
            //Invoca al método EjecutaProcAlmacenadoDataSet y asigna el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del DS (que existan y no sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas y almacena el resultado en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_inventario_resumen = Convert.ToInt32(r["Id"]);
                        this._id_inventario = Convert.ToInt32(r["IdInventario"]);
                        this._id_producto = id_producto;
                        this._id_almacen = id_almacen;
                        this._cantidad_actual = Convert.ToDecimal(r["CantidadActual"]);
                        this._fecha_inventario_resumen = Convert.ToDateTime(r["FechaInventario"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
                //Cambia de valor al objeto retorno siempre y cuando se cumpla de validación de datos.
                retorno = true;
            }
            //Retorna el método al resultado
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro Inventario Resumen
        /// </summary>
        /// <param name="id_inventario">Actualiza el identificador de un inventario</param>
        /// <param name="id_producto">Actualiza el identificador de producto</param>
        /// <param name="id_almacen">Actualiza el identificador de almacen</param>
        /// <param name="cantidad_actual">Actualiza la cantidad actual de producto en inventario</param>
        /// <param name="fecha_inventario_resumen">Actualiza la fecha de inventario resumen</param>
        /// <param name="id_usuario">Actualiza el identificador de un usuario</param>
        /// <param name="habilitar">Actualiza el estado de habilitación de un registro</param>
        /// <returns></returns>
        private RetornoOperacion editarInventarioResumen(int id_inventario, int id_producto, int id_almacen, decimal cantidad_actual, DateTime fecha_inventario_resumen, int id_usuario, bool habilitar)
        {
            //Crea el objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos para realizar la actualización de campos de un registro.
            object[] param = { 2, this._id_inventario_resumen, id_inventario, id_producto, id_almacen, cantidad_actual, fecha_inventario_resumen, id_usuario, habilitar, "", "" };
            //Asigna valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto al método
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta registros en la tabla Inventario Resumen
        /// </summary>
        /// <param name="id_inventario">Inserta el identificador de un inventario</param>
        /// <param name="id_producto">Inserta el identificador de un producto</param>
        /// <param name="id_almacen">Inserta el identificador de un almacen</param>
        /// <param name="cantidad_actual">Inserta la cantidad actual de un producto en inventario</param>
        /// <param name="fecha_inventario_resumen">Inserta la fecha de inventario resumen</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la inserción</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarInventarioResumen(int id_inventario, int id_producto, int id_almacen, decimal cantidad_actual, DateTime fecha_inventario_resumen, int id_usuario)
        {
            //Creación del objeto retorno.
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo param, que almacena los datos necesarios para realizar la inserción de registro.
            object[] param = { 1, 0, id_inventario, id_producto, id_almacen, cantidad_actual, fecha_inventario_resumen, id_usuario, true, "", "" };
            //Asigana valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro Inventario Resumen
        /// </summary>
        /// <param name="id_inventario">Actualiza el identificador de un inventario</param>
        /// <param name="id_producto">Actualiza el identificador de producto</param>
        /// <param name="id_almacen">Actualiza el identificador de almacen</param>
        /// <param name="cantidad_actual">Actualiza la cantidad actual de producto en inventario</param>
        /// <param name="fecha_inventario_resumen">Actualiza la fecha de inventario resumen</param>
        /// <param name="id_usuario">Actualiza el identificador de un usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarInventarioResumen(int id_inventario, int id_producto, int id_almacen, decimal cantidad_actual, DateTime fecha_inventario_resumen, int id_usuario)
        {
            //retorna al método el método editarInventarioResumen
            return editarInventarioResumen(id_inventario, id_producto, id_almacen, cantidad_actual, fecha_inventario_resumen, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Deshabilita registros de InventarioResumen
        /// </summary>
        /// <param name="id_usuario">Identificador de usuario que realiza la acción de deshabilitar registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarInventarioResumen(int id_usuario)
        {
            //Retorna al método el método editarInventarioResumen
            return editarInventarioResumen(this._id_inventario, this._id_producto, this._id_almacen, this._cantidad_actual, this._fecha_inventario_resumen, id_usuario, false);
        }
        /// <summary>
        /// Actualiza las existencias del producto y almacén al que pertenece este registro, incrementando o decrementando la cantidad indicada
        /// </summary>
        /// <param name="cantidad">Cantidad por incrementar o decrementar (uso de signo negativo para decremento) del inventario de este producto</param>
        /// <param name="fecha">Fecha de la última transacción que afecta al inventario de este producto</param>
        /// <param name="id_inventario">Id de Registro inventario que afecta por última vez a este producto</param>
        /// <param name="id_usuario">Id de Usuario que actualiza el inventario del producto</param>
        /// <returns></returns>
        public RetornoOperacion ActualizarExistenciasProducto(decimal cantidad, DateTime fecha, int id_inventario, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Actualizando cantidad actual del inventario (suma/resta)
            resultado = editarInventarioResumen(id_inventario, this._id_producto, this._id_almacen, (this._cantidad_actual + cantidad), fecha, id_usuario, this._habilitar);
            //Devolviendo resultados
            return resultado;
        }
        /// <summary>
        /// Método encargado de validar si el Producto tiene transacciones en el Inventario
        /// </summary>
        /// <param name="id_producto">Producto por Verificar</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaTransaccionProductoInventario(int id_producto)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Creación del arreglo param, que almacena los datos necesarios para realizar la inserción de registro.
            object[] param = { 5, 0, 0, id_producto, 0, 0.00M, null, 0, false, "", "" };

            //Invoca al método EjecutaProcAlmacenadoDataSet y asigna el resultado en el dataset DS
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del DS (que existan y no sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorre las filas y almacena el resultado en los atributos
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Validando Registro
                        if (Convert.ToInt32(r["Id"]) > 0)

                            //Instanciando Retorno
                            retorno = new RetornoOperacion(id_producto, "El Producto tiene Transacciones en el Inventario.", true);
                        else
                            //Instanciando Retorno
                            retorno = new RetornoOperacion("El Producto no tiene Transacciones en el Inventario.");
                    }
                }
                else
                    //Instanciando Retorno
                    retorno = new RetornoOperacion("El Producto no tiene Transacciones en el Inventario.");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        #endregion
    }
}
