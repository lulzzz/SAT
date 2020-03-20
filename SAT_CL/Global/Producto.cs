using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Implementa los método para la administración del Producto.
    /// </summary>
    public class Producto : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_producto_tp";

        private int _id_producto;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Producto
        /// </summary>
        public int id_producto { get { return this._id_producto; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Compañia Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private bool _bit_peligroso;
        /// <summary>
        /// Atributo encargado de Almacenar el Bit Peligroso
        /// </summary>
        public bool bit_peligroso { get { return this._bit_peligroso; } }
        private bool _bit_flamable;
        /// <summary>
        /// Atributo encargado de Almacenar  el Bit Flamable
        /// </summary>
        public bool bit_flamable { get { return this._bit_flamable; } }
        private bool _bit_perecedero;
        /// <summary>
        /// Atributo encargado de Almacenar  el Bit Perecedero
        /// </summary>
        public bool bit_perecedero { get { return this._bit_perecedero; } }
        private bool _bit_fluido;
        /// <summary>
        /// Atributo encargado de Almacenar el Bit Fluido
        /// </summary>
        public bool bit_fluido { get { return this._bit_fluido; } }
        private int _minima_temperatura;
        /// <summary>
        /// Atributo encargado de Almacenar la Minima Temperatura
        /// </summary>
        public int minima_temperatura { get { return this._minima_temperatura; } }
        private int _maxima_temperatura;
        /// <summary>
        /// Atributo encargado de Almacenar la Maxima Temperatura
        /// </summary>
        public int maxima_temperatura { get { return this._maxima_temperatura; } }
        private int _id_unidad_temperatura;
        /// <summary>
        /// Atributo encargado de Almacenar la Unidad de Temperatura
        /// </summary>
        public int id_unidad_temperatura { get { return this._id_unidad_temperatura; } }
        private string _informacion_adicional1;
        /// <summary>
        /// Atributo encargado de Almacenar la Información Adicional 1
        /// </summary>
        public string informacion_adicional1 { get { return this._informacion_adicional1; } }
        private string _informacion_adicional2;
        /// <summary>
        /// Atributo encargado de Almacenar la Información Adicional 2
        /// </summary>
        public string informacion_adicional2 { get { return this._informacion_adicional2; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Producto()
        {   //Invocando Metodo de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro"></param>
        public Producto(int id_registro)
        {   //Invocando Metodo de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Producto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_producto = 0; 
            this._id_compania_emisor = 0;
            this._descripcion = "";
            this._bit_peligroso = false;
            this._bit_flamable = false;
            this._bit_perecedero = false;
            this._bit_fluido = false;
            this._minima_temperatura = 0;
            this._maxima_temperatura = 0;
            this._id_unidad_temperatura = 0;
            this._informacion_adicional1 = "";
            this._informacion_adicional2 = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando objeto de retorno
            bool result = false;
            //Armando arreglo de parametros
            object[] param = { 3, id_registro, 0, "", false, false, false, false, 0, 0, 0, "", "", 0, false, "", "" };
            //Obteniendo resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {   //Validando Origen de Datos
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada una de las Filas
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_producto = id_registro;
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._bit_peligroso = Convert.ToBoolean(dr["BitPeligroso"]);
                        this._bit_flamable = Convert.ToBoolean(dr["BitFlamable"]);
                        this._bit_perecedero = Convert.ToBoolean(dr["BitPerecedero"]);
                        this._bit_fluido = Convert.ToBoolean(dr["BitFluido"]);
                        this._minima_temperatura = Convert.ToInt32(dr["MinimaTemperatura"]); ;
                        this._maxima_temperatura = Convert.ToInt32(dr["MaximaTemperatura"]); ;
                        this._id_unidad_temperatura = Convert.ToInt32(dr["IdUnidadTemperatura"]); ;
                        this._informacion_adicional1 = dr["InformacionAdicional1"].ToString();
                        this._informacion_adicional2 = dr["InformacionAdicional2"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }//Asignando Positivo el Objeto de Retorno
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registro en BD
        /// </summary>
        /// <param name="id_compania_emisor">Compañia Emisora</param>
        /// <param name="descripcion">Descripcion</param>
        /// <param name="bit_peligroso">Indicador Peligroso</param>
        /// <param name="bit_flamable">Indicador Flamable</param>
        /// <param name="bit_perecedero">Indicador Perecedero</param>
        /// <param name="bit_fluido">Indicador Fluido</param>
        /// <param name="minima_temperatura">Minima Temperatura</param>
        /// <param name="maxima_temperatura">Maxima Temperatura</param>
        /// <param name="id_unidad_temperatura">Unidad de Temperatura</param>
        /// <param name="informacion_adicional1">Información Adicional 1</param>
        /// <param name="informacion_adicional2">Información Adicional 2</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_compania_emisor, string descripcion, bool bit_peligroso, 
                                        bool bit_flamable, bool bit_perecedero, bool bit_fluido, int minima_temperatura, 
                                        int maxima_temperatura, int id_unidad_temperatura, string informacion_adicional1,
                                        string informacion_adicional2, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 2, this._id_producto, id_compania_emisor, descripcion, bit_peligroso, bit_flamable, bit_perecedero, 
                                 bit_fluido, minima_temperatura, maxima_temperatura, id_unidad_temperatura, informacion_adicional1, 
                                 informacion_adicional2, id_usuario, habilitar, "", "" };
            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Productos
        /// </summary>
        /// <param name="id_compania_emisor">Compañia Emisora</param>
        /// <param name="descripcion">Descripcion</param>
        /// <param name="bit_peligroso">Indicador Peligroso</param>
        /// <param name="bit_flamable">Indicador Flamable</param>
        /// <param name="bit_perecedero">Indicador Perecedero</param>
        /// <param name="bit_fluido">Indicador Fluido</param>
        /// <param name="minima_temperatura">Minima Temperatura</param>
        /// <param name="maxima_temperatura">Maxima Temperatura</param>
        /// <param name="id_unidad_temperatura">Unidad de Temperatura</param>
        /// <param name="informacion_adicional1">Información Adicional 1</param>
        /// <param name="informacion_adicional2">Información Adicional 2</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaProducto(int id_compania_emisor, string descripcion, bool bit_peligroso, 
                                        bool bit_flamable, bool bit_perecedero, bool bit_fluido, int minima_temperatura, 
                                        int maxima_temperatura, int id_unidad_temperatura, string informacion_adicional1,
                                        string informacion_adicional2, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 1, 0, id_compania_emisor, descripcion, bit_peligroso, bit_flamable, bit_perecedero, 
                                 bit_fluido, minima_temperatura, maxima_temperatura, id_unidad_temperatura, informacion_adicional1, 
                                 informacion_adicional2, id_usuario, true, "", "" };
            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Productos
        /// </summary>
        /// <param name="id_compania_emisor">Compañia Emisora</param>
        /// <param name="descripcion">Descripcion</param>
        /// <param name="bit_peligroso">Indicador Peligroso</param>
        /// <param name="bit_flamable">Indicador Flamable</param>
        /// <param name="bit_perecedero">Indicador Perecedero</param>
        /// <param name="bit_fluido">Indicador Fluido</param>
        /// <param name="minima_temperatura">Minima Temperatura</param>
        /// <param name="maxima_temperatura">Maxima Temperatura</param>
        /// <param name="id_unidad_temperatura">Unidad de Temperatura</param>
        /// <param name="informacion_adicional1">Información Adicional 1</param>
        /// <param name="informacion_adicional2">Información Adicional 2</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaProducto(int id_compania_emisor, string descripcion, bool bit_peligroso,
                                        bool bit_flamable, bool bit_perecedero, bool bit_fluido, int minima_temperatura,
                                        int maxima_temperatura, int id_unidad_temperatura, string informacion_adicional1,
                                        string informacion_adicional2, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_compania_emisor, descripcion, bit_peligroso, bit_flamable, bit_perecedero,
                                 bit_fluido, minima_temperatura, maxima_temperatura, id_unidad_temperatura, informacion_adicional1,
                                 informacion_adicional2, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Productos
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaProducto(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_compania_emisor, this._descripcion, this._bit_peligroso, this._bit_flamable, this._bit_perecedero,
                                 this._bit_fluido, this._minima_temperatura, this._maxima_temperatura, this._id_unidad_temperatura, this._informacion_adicional1,
                                 this._informacion_adicional2, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los atributos de los Produtos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaProducto()
        {   //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_producto);
        }


        #endregion

    }
}
