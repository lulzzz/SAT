using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Clase que permite realiza acciones sobre la tabla DevolucionFaltanteProducto(Actualiza,Consulta e Inserta registros)
    /// </summary>
    public class DevolucionFaltanteProducto: Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp de la tabla DevolucionFaltanteProducto
        /// </summary>
        private static string nom_sp = "despacho.sp_devolucion_faltante_producto_tdfp";
        private int _id_devolucion_faltante_producto;
        /// <summary>
        /// Almacena el identificador del registro de una devolucion de producto faltante
        /// </summary>
        public int id_devolucion_faltante_producto
        {
            get { return _id_devolucion_faltante_producto; }
        }
        private int _id_compania_origen;
        /// <summary>
        /// Almacena el identificador de la compañia a la que pertenece el producto devolucion faltante
        /// </summary>
        public int id_compania_origen
        {
            get { return _id_compania_origen; }
        }
        private string _descripcion_producto;
        /// <summary>
        /// Almacena las caracteristicas de un producto
        /// </summary>
        public string descripcion_producto
        {
            get { return _descripcion_producto; }
        }
        private string _codigo_producto;
        /// <summary>
        /// Almacena una secuencia numerica que identifica como unico a un producto
        /// </summary>
        public string codigo_producto
        {
            get { return _codigo_producto; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacena el estatus de habilitación de un registro (Habilitado/Deshabilitado)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que incializa los atributos a 0.
        /// </summary>
        public DevolucionFaltanteProducto()
        {
            this._id_devolucion_faltante_producto = 0;
            this._id_compania_origen = 0;
            this._descripcion_producto = "";
            this._codigo_producto = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa loas atributos a partir de un registro de DevoluciónFaltanteProducto
        /// </summary>
        /// <param name="id_devolucion_faltante_producto"></param>
        public DevolucionFaltanteProducto(int id_devolucion_faltante_producto)
        {
            //Invoca al método cargaAtributos().
            cargaAtributos(id_devolucion_faltante_producto);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~DevolucionFaltanteProducto()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la consulta de un registro de DevoluciónFaltanteProducto y los alamcena en los atributos de la clase
        /// </summary>
        /// <param name="id_devolucion_faltante_producto">Id que sirve como referencia para realizar la consulta de registro de DevoluciónFaltanteProducto </param>
        /// <returns></returns>
        private bool cargaAtributos(int id_devolucion_faltante_producto)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta del registro de DevoluciónFaltanteProducto
            object[] param = { 3, id_devolucion_faltante_producto, 0, "", "", 0, false, "", "" };
            //Realiza la consulta a base de datos y almacena el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos asignados al DS. (que existan o sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS))
                {
                    //Recorre las filas del dataset y almacena el resultado en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_devolucion_faltante_producto = id_devolucion_faltante_producto;
                        this._id_compania_origen = Convert.ToInt32(r["IdCompaniaOrigen"]);
                        this._descripcion_producto = Convert.ToString(r["DescripcionProducto"]);
                        this._codigo_producto = Convert.ToString(r["CodigoProducto"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la validación de datos
                    retorno = true;
                }
            }
            //Devuelve el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de DevolucionFaltanteProducto
        /// </summary>
        /// <param name="id_compania_origen">Actualiza el identificador de una compañia a la que pertenece el registro</param>
        /// <param name="descripcion_producto">Actualiza la descripción del producto</param>
        /// <param name="codigo_producto">Actualiza el codigo del producto</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estatus de habilitación del registro</param>
        /// <returns></returns>
        private RetornoOperacion editarDevolucionFaltanteProducto(int id_compania_origen, string descripcion_producto, string codigo_producto, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la actualización del registro
            object[] param = { 2, this._id_devolucion_faltante_producto, id_compania_origen, descripcion_producto, codigo_producto, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado de la actualización
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que realiza inserciones de registros de DevoluciónFaltanteProducto
        /// </summary>
        /// <param name="id_compania_origen">Inserta el identificador de un acompañia a la que pertenece el producto de devolución faltante</param>
        /// <param name="descripcion_producto">Inserta la descripcion de un producto de devolución faltante</param>
        /// <param name="codigo_producto">Inserta el codigo de un producto de devolución faltante</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la acción de inserción</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarDevolucionFaltanteProducto(int id_compania_origen, string descripcion_producto, string codigo_producto, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del parametro que almacena los datos necesarios para realizar la insercion del registro de DevolucionFaltanteProducto
            object[] param = { 1, 0, id_compania_origen, descripcion_producto, codigo_producto, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno al método del objeto retrono
            return retorno;
        }

        /// <summary>
        /// Método que actualiza los campos de un registro de DevolucionFaltanteProducto
        /// </summary>
        /// <param name="id_compania_origen">Actualiza el identificador de una compañia a la que pertenece el registro</param>
        /// <param name="descripcion_producto">Actualiza la descripción del producto</param>
        /// <param name="codigo_producto">Actualiza el codigo del producto</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarDevolucionFaltanteProducto(int id_compania_origen, string descripcion_producto, string codigo_producto, int id_usuario)
        {
            //Retorno del método editarDevolucionFaltanteProducto().
            return editarDevolucionFaltanteProducto(id_compania_origen, descripcion_producto, codigo_producto, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un registro de DevolucionFaltanteProducto
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDevolucionFaltanteProducto(int id_usuario)
        {
            //Retorno del método editarDevolucionFaltanteProducto().
            return editarDevolucionFaltanteProducto(this._id_compania_origen, this._descripcion_producto, this._codigo_producto, id_usuario, false);
        }
        /// <summary>
        /// Método que actualiza los atributos de DevolucionFaltanteProducto
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDevolucionFaltanteProducto()
        {
            //Retorno del método cargaAtributos
            return cargaAtributos(this._id_devolucion_faltante_producto);
        }
        /// <summary>
        /// Método encargado de Obtener el Producto dado un Código
        /// </summary>
        /// <param name="codigo_producto">Código del Producto</param>
        /// <returns></returns>
        public static DevolucionFaltanteProducto ObtieneProducto(string codigo_producto)
        {
            //Declarando Objeto de Retorno
            DevolucionFaltanteProducto producto = new DevolucionFaltanteProducto();

            //Creación del parametro que almacena los datos necesarios para realizar la insercion del registro de DevolucionFaltanteProducto
            object[] param = { 4, 0, 0, "", codigo_producto, 0, false, "", "" };

            //Realiza la consulta a base de datos y almacena el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos asignados al DS. (que existan o sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas del dataset y almacena el resultado en los atributos
                    foreach (DataRow dr in DS.Tables["Table"].Rows)
                    
                        //Asignando producto
                        producto = new DevolucionFaltanteProducto(Convert.ToInt32(dr["Id"].ToString().Equals("") ? "0" : dr["Id"].ToString()));
                    
                }
            }

            //Devolviendo Resultado Obtenido
            return producto;
        }


        #endregion
    }
}
