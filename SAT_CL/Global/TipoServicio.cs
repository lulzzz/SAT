using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase de la tabla ProveedorTipoServicio que permite realizar operaciones sobre la tabla(Inserciones,Consultas,Actualizaciones,etc).
    /// </summary>
    public class ProveedorTipoServicio : Disposable
    {

        #region Atributos

        /// <summary>
        /// Atributo que almacena el nombre del sp de la tabla ProveedorTipoServicio
        /// </summary>
        private static string nom_sp = "global.sp_proveedor_tipo_servicio_tpts";

        private int _id_proveedor_tipo_servicio;
        /// <summary>
        /// Identificador del proveedor de un tipo servicio
        /// </summary>
        public int id_proveedor_tipo_servicio
        {
            get { return _id_proveedor_tipo_servicio; }
        }

        private int _id_compania_emisor;
        /// <summary>
        /// Identificador de la compañía.
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }

        private string _descripcion;
        /// <summary>
        /// Almacena la descripción de un ProveedorTipoServicio
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }

        private bool _habilitar;
        /// <summary>
        /// Permite almacenar el cambio de estado (habilitado/Deshabilitado) de un registro ProveedorTipoServicio.
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor default de la clase que inicializa los atributos 
        /// </summary>
        public ProveedorTipoServicio()
        {
            this._id_proveedor_tipo_servicio = 0;
            this._id_compania_emisor = 0;
            this._descripcion = "";
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro ProveedorTipoServicio
        /// </summary>
        /// <param name="id_proveedor_tipo_servicio">Id que sirve como referencia para la asignacion de registros los atributos de la clase ProveedorTipoServicio</param>
        public ProveedorTipoServicio(int id_proveedor_tipo_servicio)
        {
            //Invoca al método cargaAtributoInstancia();
            cargaAtributoInstancia(id_proveedor_tipo_servicio);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ProveedorTipoServicio()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite la asignación de datos a los atributos de la clase a partir de un registro. 
        /// </summary>
        /// <param name="id_proveedor_tipo_servicio">Id que sirve como referencia para la busqueda de registros de ProveedorTipoServicio</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_proveedor_tipo_servicio)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación y asignación de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 3, id_proveedor_tipo_servicio, 0, "", 0, false, "", "" };
            //Invoca al sp de la Tabla ProveedorTipoServicio
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que en el dataset existan registros y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas y almacena en la variable r los datos encontrados
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_proveedor_tipo_servicio = id_proveedor_tipo_servicio;
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno solo si se cumple la validación de datos.
                    retorno = true;
                }
            }
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite la actualización de registro de ProveedorTipoServicio
        /// </summary>
        /// <param name="id_compania_emisor">Permite la actualización del identificador de una campañía</param>
        /// <param name="descripcion">Permite la actualización de descripción del ProveedorTipoServicio</param>
        /// <param name="id_usuario">Permite la actualización del identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Permite actualizar el estado de habilitacion de un registro (Deshabilitado)</param>
        /// <returns></returns>
        private RetornoOperacion editarProveedorTipoServicio(int id_compania_emisor, string descripcion, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 2, this.id_proveedor_tipo_servicio, id_compania_emisor, descripcion, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método que permite la inserción de datos en la tabla ProveedorTipoServicio
        /// </summary>
        /// <param name="id_compania_emisor">Permite la inserción de un identificador para una compañia emisora</param>
        /// <param name="descripcion">Permite la inserción de la descripción de un ProveedorTipoServicio</param>
        /// <param name="id_usuario">Permite la inserción del identificador del usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarProveedorTipoServicio(int id_compania_emisor, string descripcion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 1, 0, id_compania_emisor, descripcion, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite la actualización de registros de ProveedorTipoServicio
        /// </summary>
        /// <param name="id_compania_emisor">Permite la actualización del identificador de una compañia</param>
        /// <param name="descripcion">Permite la actualizacion de la descripcion de un ProveedorTipoServicio</param>
        /// <param name="id_usuario">Permite la actualización de un identificador del usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarProveedorTipoServicio(int id_compania_emisor, string descripcion, int id_usuario)
        {
            //Invoca y retorna el método editarProveedorTipoServicio().
            return this.editarProveedorTipoServicio(id_compania_emisor, descripcion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite actualizar estado de habilitción o deshabilitación de un registro de ProveedorTipoServicio
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario quien realizo el cambio de estado del registro(Habilo/Deshabilito)</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarProveedorTipoServicio(int id_usuario)
        {
            //Invoca y retorna el método editaProveedorTipoServicio()-
            return this.editarProveedorTipoServicio(this.id_compania_emisor, this.descripcion, id_usuario, false);
        }

        #endregion

    }
}
