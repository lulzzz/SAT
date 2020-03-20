using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// Clase que permite realizar acciones sobre la tabla inventario (Inserción, Actualización y Deshabilitación de registros).
    /// </summary>
    public class Inventario : Disposable
    {
        #region Enumeracion

        /// <summary>
        /// Enumeracion de estatus de un producto
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// El producto del elemento inventario se encuentra disponible en almacén.
            /// </summary>
            Disponible = 1,
            /// <summary>
            /// El producto del elemento inventario se encuentra dañado en almacén.
            /// </summary>
            Dañado = 2,
            /// <summary>
            /// El producto del elemento inventario se encuentra caduco en almacén.
            /// </summary>
            Caduco = 3,
            /// <summary>
            /// El producto del elemento inventario ya no existe en esta presentación dentro del inventario, se extrajo su contenido.
            /// </summary>
            Desmontado = 4,
            /// <summary>
            /// El elemento del inventario agotó sus existencias
            /// </summary>
            Agotado = 5
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo privado que almacena el nombre del store procedure de la tabla Inventario.
        /// </summary>
        private static string nom_sp = "almacen.sp_inventario_ti";

        private int _id_inventario;
        /// <summary>
        /// Id que permite identificar un registro de inventario.
        /// </summary>
        public int id_inventario
        {
            get { return _id_inventario; }
        }
        private int _id_producto;
        /// <summary>
        /// Id que permite identificar el registros de un producto.
        /// </summary>
        public int id_producto
        {
            get { return _id_producto; }
        }
        private decimal _cantidad;
        /// <summary>
        /// Cantidad de un producto en inventario.
        /// </summary>
        public decimal cantidad
        {
            get { return _cantidad; }
        }
        private int _id_entrada_salida;
        /// <summary>
        /// Id que Identifica la entrada o salida de producto del almacén.
        /// </summary>
        public int id_entrada_salida
        {
            get { return _id_entrada_salida; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Id que permite determina el estatus del producto (Disponible, Dañado,Caduco).
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        /// <summary>
        /// Permite acceder a los elementos de la enumeración Estatus.
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this._id_estatus; }
        }
        private string _lote;
        /// <summary>
        /// Lote de un producto.
        /// </summary>
        public string lote
        {
            get { return _lote; }
        }
        private string _serie;
        /// <summary>
        /// Secuencia numerica de un producto.
        /// </summary>
        public string serie
        {
            get { return _serie; }
        }
        private DateTime _fecha_caducidad;
        /// <summary>
        /// Fecha de caducidad de un producto.
        /// </summary>
        public DateTime fecha_caducidad
        {
            get { return _fecha_caducidad; }
        }
        private int _id_tabla;
        /// <summary>
        /// Id que permite identificar a que tabla corresponde un registro de inventario.
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_registro;
        /// <summary>
        /// Id que permite identificar el registro de inventario.
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private int _id_inventario_origen;
        /// <summary>
        /// Id que permite identificar si el registro es derivado de un registro previo de inventario.
        /// </summary>
        public int id_inventario_origen
        {
            get { return _id_inventario_origen; }
        }
        private string _posicion_x;
        /// <summary>
        /// Permite definir la posición en coordenas x de un producto.
        /// </summary>
        public string posicion_x
        {
            get { return _posicion_x; }
        }
        private string _posicion_y;
        /// <summary>
        /// Permite definir la posición en coordenas y de un producto.
        /// </summary>
        public string posicion_y
        {
            get { return _posicion_y; }
        }
        private string _posicion_z;
        /// <summary>
        /// Permite definir la posición en coordenadas z de un producto.
        /// </summary>
        public string posicion_z
        {
            get { return _posicion_z; }
        }
        private decimal _precio_entrada;
        /// <summary>
        /// Precio de entrada a almacén de un producto.
        /// </summary>
        public decimal precio_entrada
        {
            get { return _precio_entrada; }
        }
        private decimal _precio_salida;
        /// <summary>
        /// Precio de salida de almacén de un producto.
        /// </summary>
        public decimal precio_salida
        {
            get { return _precio_salida; }
        }
        private decimal _precio_pesos;
        /// <summary>
        /// Precio total del producto (con o sin aplicación de tipo cambio).
        /// </summary>
        public decimal precio_pesos
        {
            get { return _precio_pesos; }
        }
        private int _id_inventario_entrada;
        /// <summary>
        /// Obtiene el Id del elemento del inventario con el cual se incorporó el producto a las existencias.
        /// </summary>
        public int id_inventario_entrada { get { return this._id_inventario_entrada; } }
        private bool _habilitar;
        /// <summary>
        /// Permite cambiar el estado de un registro habilitado o deshabilitado.
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por default que inicializa a valores predeterminados los atributos.
        /// </summary>
        public Inventario()
        {
            this._id_inventario = 0;
            this._id_producto = 0;
            this._cantidad = 0.0m;
            this._id_entrada_salida = 0;
            this._id_estatus = 0;
            this._lote = "";
            this._serie = "";
            this._fecha_caducidad = DateTime.MinValue;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._id_inventario_origen = 0;
            this._posicion_x = "";
            this._posicion_y = "";
            this._posicion_z = "";
            this._precio_entrada = 0.0m;
            this._precio_salida = 0.0m;
            this._precio_pesos = 0.0m;
            this._id_inventario_entrada = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un id_inventario solicitado
        /// </summary>
        /// <param name="id_inventario">Id que permite identificar un registro de inventario</param>
        public Inventario(int id_inventario)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_inventario);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase.
        /// </summary>
        ~Inventario()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que realiza la busuqeda de un registros a partir de un id_inventario dado. 
        /// </summary>
        /// <param name="id_inventario">Id de Registro inventario</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_inventario)
        {
            //Crea el objeto retorno
            bool retorno = false;
            //Crea el arreglo param que almacena los datos necesarios para realizar la consulta de un registro a la tabla inventario.
            object[] param = { 3, id_inventario, 0, 0.0, 0, 0, "", "", null, 0, 0, 0, 0, 0, 0, 0.0, 0.0, 0.0, 0, 0, false, "", "" };
            //Asigana al dataset el resultado de invocal al método EjecutaProcAlmacenadoDataSet
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del DS (Que existan o que no sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas del registro y alamcena el resultado en los atributos de la clase
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_inventario = id_inventario;
                        this._id_producto = Convert.ToInt32(r["IdProducto"]);
                        this._cantidad = Convert.ToDecimal(r["Cantidad"]);
                        this._id_entrada_salida = Convert.ToInt32(r["IdEntradaSalida"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._lote = Convert.ToString(r["Lote"]);
                        this._serie = Convert.ToString(r["Serie"]);
                        DateTime.TryParse(r["FechaCaducidad"].ToString(), out this._fecha_caducidad);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._id_inventario_origen = Convert.ToInt32(r["IdInventarioOrigen"]);
                        this._posicion_x = r["PosicionX"].ToString();
                        this._posicion_y = r["PosicionY"].ToString();
                        this._posicion_z = r["PosicionZ"].ToString();
                        this._precio_entrada = Convert.ToDecimal(r["PrecioEntrada"]);
                        this._precio_salida = Convert.ToDecimal(r["PrecioSalida"]);
                        this._precio_pesos = Convert.ToDecimal(r["PrecioPesos"]);
                        this._id_inventario_entrada = Convert.ToInt32(r["IdInventarioEntrada"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno siempre y cuandos e cumplan la validacion del DS
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los campos de un registro de inventario
        /// </summary>
        /// <param name="id_producto">Permite actualizar el identificador de un producto</param>
        /// <param name="cantidad">Permite actualizar el numero de productos en inventario</param>
        /// <param name="id_entrada_salida">Permite actualizar el identificador de una entrada a una salida y viseversa</param>
        /// <param name="estatus">Permite actualizar el estatus de un producto (Disponible, dañado, caduco)</param>
        /// <param name="serie">Permite actualizar la serie de un producto</param>
        /// <param name="fecha_caducidad">Permite actualizar la fecha de caducidad de un producto</param>
        /// <param name="id_tabla">Permite actualizar el identificador de tabla donde proviene el registro</param>
        /// <param name="id_registro">Permite actualizar el identificador de registro de donde proviene</param>
        /// <param name="id_inventario_origen">Permite actualizar si tiene una derivacion a un registro de inventario previo</param>
        /// <param name="posicion_x">Permite actualizar la ubicación en coordenada x de un producto</param>
        /// <param name="posicion_y">Permite actualizar la ubicación en coordenada y de un producto</param>
        /// <param name="posicion_z">Permite actualizar la ubicación en coordenada z de un producto</param>
        /// <param name="precio_entrada">Permite actualizar e precio de entrada a almacén a un producto</param>
        /// <param name="precio_salida">Permite actualizar el precio de salida de almacén a un producto</param>
        /// <param name="precio_pesos">Permite actualizar el precio en pesos de un producto sea entrada o salida</param>
        /// <param name="id_inventario_entrada">Id de Inventario con el cual entró a las existencias el producto asociado</param>
        /// <param name="id_usuario">Permite identificar al usuario quenrealizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de un registro (Habilitado -Deshabilitado)</param>
        /// <returns></returns>
        private RetornoOperacion editarInventario(int id_producto, decimal cantidad, int id_entrada_salida, Estatus estatus, string lote, string serie, DateTime fecha_caducidad,
                                                  int id_tabla, int id_registro, int id_inventario_origen, string posicion_x, string posicion_y, string posicion_z,
                                                  decimal precio_entrada, decimal precio_salida, decimal precio_pesos, int id_inventario_entrada, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo param que almacena los datos necesarios para la edicion de un registro
            object[] param = { 2, this._id_inventario, id_producto, cantidad, id_entrada_salida, (byte)estatus, lote, serie, Fecha.ConvierteDateTimeObjeto(fecha_caducidad), id_tabla, id_registro,
                               id_inventario_origen, posicion_x, posicion_y, posicion_z, precio_entrada, precio_salida, precio_pesos, id_inventario_entrada, id_usuario, habilitar, "", ""};
            //Asigna al objeto retorno el resultado de invocar al método EjecutaProcAlmacenadoObjeto
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que permite realizar la insercion de registros en la tabla Inventario
        /// </summary>
        /// <param name="id_producto">Inserta el identificador de un producto</param>
        /// <param name="cantidad">Inserta la catidad de producto en almacén</param>
        /// <param name="id_entrada_salida">Inserta el identificador de un registro de tipo Entrada o salida</param>
        /// <param name="estatus">Inserta el estatus de prducto en almacén (Disponible, Dañado, Caduco)</param>
        /// <param name="lote">Lote</param>
        /// <param name="serie">Inserta la serie de un producto que lo hace unico en almacén</param>
        /// <param name="fecha_caducidad">Inserta la fecha de caducidad de un producto en almcen</param>
        /// <param name="id_tabla">Inserta el identificador de la tabla de procedencia de un registro</param>
        /// <param name="id_registro">Inserta el identiicador del registro</param>
        /// <param name="id_inventario_origen">Inserta el identificador de inventario si procede de un inventario</param>
        /// <param name="posicion_x">Inserta la ubicación de un producto en coordenada x</param>
        /// <param name="posicion_y">Inserta la ubicación de un producto en coordenada y</param>
        /// <param name="posicion_z">Inserta la ubicación de un producto en coordenada z</param>
        /// <param name="precio_entrada">Inserta el precio de entrada de un producto</param>
        /// <param name="precio_salida">Inserta el precio de salida de un producto</param>
        /// <param name="precio_pesos">Inserta el precio total del producto</param>
        /// <param name="id_inventario_entrada">Id de Inventario con el cual entró a las existencias el producto asociado</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la inserción del registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarInventario(int id_producto, decimal cantidad, int id_entrada_salida, Estatus estatus, string lote, string serie, DateTime fecha_caducidad,
                                                          int id_tabla, int id_registro, int id_inventario_origen, string posicion_x, string posicion_y, string posicion_z,
                                                          decimal precio_entrada, decimal precio_salida, decimal precio_pesos, int id_inventario_entrada, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo param que almacena los datos necesarios para insertar un registro.
            object[] param ={1, 0, id_producto, cantidad, id_entrada_salida, (byte)estatus, lote, serie, Fecha.ConvierteDateTimeObjeto(fecha_caducidad), id_tabla, id_registro,
                               id_inventario_origen, posicion_x, posicion_y, posicion_z, precio_entrada, precio_salida, precio_pesos, 
                               id_inventario_entrada, id_usuario, true, "", "" };
            //Asigna al objeto retorno el resultado de invocar el método EjecutaProcAlmacenadoObjeto.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Retorna el objeto retorno al método.
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los campos de un registro de inventario
        /// </summary>
        /// <param name="cantidad">Cantidad de producto de este elemento inventario</param>
        /// <param name="serie">Permite actualizar la serie de un producto</param>
        /// <param name="fecha_caducidad">Permite actualizar la fecha de caducidad de un producto</param>
        /// <param name="posicion_x">Permite actualizar la ubicación en coordenada x de un producto</param>
        /// <param name="posicion_y">Permite actualizar la ubicación en coordenada y de un producto</param>
        /// <param name="posicion_z">Permite actualizar la ubicación en coordenada z de un producto</param>
        /// <param name="id_usuario">Permite identificar al usuario que realizo acciones sobre el registro</param>
        public RetornoOperacion EditarInventario(decimal cantidad, string serie, DateTime fecha_caducidad, string posicion_x, string posicion_y, string posicion_z, int id_usuario)
        {
            //Retorna el método invocado editarInventario
            return editarInventario(this._id_producto, cantidad, this._id_entrada_salida, (Estatus)this._id_estatus, this._lote, serie, fecha_caducidad, this._id_tabla,
                                    this._id_registro, this._id_inventario_origen, posicion_x, posicion_y, posicion_z, this._precio_entrada,
                                    this._precio_salida, this._precio_pesos, this._id_inventario_entrada, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar la Serie y/ó la Fecha de Caducidad
        /// </summary>
        /// <param name="lote">Lote del Inventario</param>
        /// <param name="serie">Serie del Invetario</param>
        /// <param name="fecha_caducidad">Fecha de Caducidad</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizarLoteSerieFechaCaducidadInventario(string lote, string serie, DateTime fecha_caducidad, int id_usuario)
        {
            //Retorna el método invocado editarInventario
            return editarInventario(this._id_producto, this._cantidad, this._id_entrada_salida, (Estatus)this._id_estatus, lote, serie, fecha_caducidad, this._id_tabla,
                                    this._id_registro, this._id_inventario_origen, this._posicion_x, this._posicion_y, this._posicion_z, this._precio_entrada,
                                    this._precio_salida, this._precio_pesos, this._id_inventario_entrada, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un registro de inventario
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarInventario(int id_usuario)
        {
            //Retorna el método invocado editarInventario
            return editarInventario(this._id_producto, this._cantidad, this._id_entrada_salida, (Estatus)this._id_estatus, this._lote, this._serie, this._fecha_caducidad, this._id_tabla,
                                    this._id_registro, this._id_inventario_origen, this._posicion_x, this._posicion_y, this._posicion_z, this._precio_entrada,
                                    this._precio_salida, this._precio_pesos, this._id_inventario_entrada, id_usuario, false);
        }
        /// <summary>
        /// Realiza la actualización del estatus del elemento del inventario
        /// </summary>
        /// <param name="estatus">Estatus al que se actualizará el elemento del inventario</param>
        /// <param name="id_usuario">Id de Usuario que Actualiza el estatus del inventario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizarEstatus(Estatus estatus, int id_usuario)
        {
            //Actualizando estatus
            return editarInventario(this._id_producto, this._cantidad, this._id_entrada_salida, estatus, this._lote, this._serie, this._fecha_caducidad, this._id_tabla,
                                    this._id_registro, this._id_inventario_origen, this._posicion_x, this._posicion_y, this._posicion_z, this._precio_entrada,
                                    this._precio_salida, this._precio_pesos, this._id_inventario_entrada, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar el Precio de Salida
        /// </summary>
        /// <param name="precio_salida">Precio de Salida actual de Catalogo de Productos</param>
        /// <param name="id_usuario">Id de Usuario que Actualiza el estatus del inventario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizarPrecioSalida(decimal precio_salida, int id_usuario)
        {
            //Retorna el método invocado editarInventario
            return editarInventario(this._id_producto, this._cantidad, this._id_entrada_salida, (Estatus)this._id_estatus, this._lote, this._serie, this._fecha_caducidad, this._id_tabla,
                                    this._id_registro, this._id_inventario_origen, this._posicion_x, this._posicion_y, this._posicion_z, this._precio_entrada,
                                    precio_salida, this._precio_pesos, this._id_inventario_entrada, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Obtiene la cantidad de existencias del elemento inventario
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerExistencias()
        {
            //Declarando objeto de resultado
            decimal existencias = 0;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 4, this._id_inventario, 0, 0.0, 0, 0, "", "", null, 0, 0, 0, 0, 0, 0, 0.0, 0.0, 0.0, 0, 0, false, "", "" };

            //Generando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Recuperando existencias
                    existencias = Convert.ToDecimal(ds.Tables["Table"].Rows[0][0]);
            }

            //Devolviendo resultado
            return existencias;
        }
        /// <summary>
        /// Valida que se cumpla la expresión: ('existencias' - 'cantidad_solicitada') >= 'cantidad_restante' 
        /// </summary>
        /// <param name="cantidad_solicitada">Cantidad solicitada</param>
        /// <param name="cantidad_restante">Cantidad restante</param>
        /// <returns></returns>
        public RetornoOperacion ValidarExistencias(decimal cantidad_solicitada, decimal cantidad_restante)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Validando que el estatus actual del elemento de inventario permita la acción
            if (this.estatus != Estatus.Desmontado && this.estatus != Estatus.Agotado)
            {
                //Obteniendo existencias del inventario
                decimal existencias = ObtenerExistencias();

                //Si la cantidad solicitada es mayor a la disponible
                if ((existencias - cantidad_solicitada) < cantidad_restante)
                    resultado = new RetornoOperacion(string.Format("Inventario Insuficiente. Cantidad Solicitada: '{0}' Cantidad Existente: '{1}' Cantidad Restante: '{2}'", cantidad_solicitada, existencias, cantidad_restante));
            }
            //Si el estatus no es válido para la acción
            else
                resultado = new RetornoOperacion(string.Format("El estatus actual del elemento de inventario es '{0}', no es posible realizar alguna acción sobre el mismo.", this.estatus));

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Valida que se cumpla la expresión: ('existencias' - 'cantidad_solicitada') >= 0 
        /// </summary>
        /// <param name="cantidad_solicitada">Cantidad solicitada</param>
        /// <param name="cantidad_restante">Cantidad restante</param>
        /// <returns></returns>
        public RetornoOperacion ValidarExistencias(decimal cantidad_solicitada)
        {
            //Devolviendo resultado
            return ValidarExistencias(cantidad_solicitada, 0);
        }
        /// <summary>
        /// Realiza el incremento de existencias del inventario de un producto señalado, mediante la inserción de una línea de entrada al inventario.
        /// </summary>
        /// <param name="id_producto">Id de Producto que ingresa al inventario</param>
        /// <param name="cantidad">Cantidad del producto que ingresa</param>
        /// <param name="id_entrada">Id de Registro entrada al que se asocia el nuevo elemento de inventario</param>
        /// <param name="lote">Lote</param>
        /// <param name="serie">Número de serie para identificar este nuevo elemento</param>
        /// <param name="fecha_caducidad">Fecha de Caducidad del elemento</param>
        /// <param name="id_tabla">Id de la Tabla de Origen (entidad) a la que debe vincularse</param>
        /// <param name="id_registro">Id de Registro (perteneciente a la tabla) al que se vincula</param>
        /// <param name="id_inventario_origen">Id de elemento del Inventario del que se desprende el incremento del producto en inventario</param>
        /// <param name="posicion_x">Posición física (en eje X) del elemento de inventario dentro del almacén</param>
        /// <param name="posicion_y">Posición física (en eje Y) del elemento de inventario dentro del almacén</param>
        /// <param name="posicion_z">Posición física (en eje Z) del elemento de inventario dentro del almacén</param>        
        /// <param name="precio_entrada">Precio de entrada por unidad (acorde a la moneda utilizada en su entrada)</param>
        /// <param name="precio_pesos">Precio de entrada por unidad espresado en moneda nacional</param>
        /// <param name="id_usuario">Id de Usuario que incrementa el inventario</param>
        /// <returns></returns>
        public static RetornoOperacion IncrementarExistencias(int id_producto, decimal cantidad, int id_entrada, string lote, string serie, DateTime fecha_caducidad,
                                                          int id_tabla, int id_registro, int id_inventario_origen, string posicion_x, string posicion_y, string posicion_z,
                                                          decimal precio_entrada, decimal precio_pesos, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declarando variable para alamacenar id de registro insertado en inventario
            int id_nuevo_inventario = 0;

            //Creando entorno transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Insertando nueva línea de inventario
                resultado = InsertarInventario(id_producto, cantidad, id_entrada, Estatus.Disponible, lote, serie, fecha_caducidad, id_tabla, id_registro, id_inventario_origen,
                                    posicion_x, posicion_y, posicion_z, precio_entrada, 0, precio_pesos, 0, id_usuario);

                //Si se insertó el nuevo elemento de inventario
                if (resultado.OperacionExitosa)
                {
                    //Almacenando id de nuevo elemento inventario
                    id_nuevo_inventario = resultado.IdRegistro;

                    //Instanciando registro de entrada
                    using (EntradaSalida entrada = new EntradaSalida(id_entrada))
                    {
                        //Si se localiza la información de entrada
                        if (entrada.id_entrada_salida > 0)
                        {
                            //Instanciando resumen de inventario
                            using (InventarioResumen resumen = new InventarioResumen(id_producto, entrada.id_almacen))
                            {
                                //Si se encontró información de resumen
                                if (resumen.id_inventario_resumen > 0)
                                    //Actualizando con datos de nuevo elemento inventario
                                    resultado = resumen.ActualizarExistenciasProducto(cantidad, entrada.fecha_entrada_salida, id_nuevo_inventario, id_usuario);
                                //Si no hay información aún (nuevo producto ingresado al alamcén)
                                else
                                    //Insertando nuevo resumen del producto
                                    resultado = InventarioResumen.InsertarInventarioResumen(id_nuevo_inventario, id_producto, entrada.id_almacen, cantidad, entrada.fecha_entrada_salida, id_usuario);

                                //Si hay errores en la actualización de resumen
                                if (!resultado.OperacionExitosa)
                                {
                                    //Instanciando producto y almacén de interés
                                    using (Producto producto = new Producto(id_producto))
                                    using (Almacen almacen = new Almacen(entrada.id_almacen))
                                        //Personalizando error
                                        resultado = new RetornoOperacion(string.Format("Error al actualizar existencias totales (Prod. '{0}' Alm. '{1}').", producto.descripcion, almacen.descripcion));
                                }
                            }
                        }
                        else
                            resultado = new RetornoOperacion("No fue posible recuperar la información de entrada al inventario.");
                    }
                }
                //Si no se pudo insertar la línea de inventario
                else
                {
                    //Instanciando producto
                    using (Producto producto = new Producto(id_producto))
                        resultado = new RetornoOperacion(string.Format("Error al registrar nueva línea en inventario. Prod.: '{0}' Serie: '{1}' F.Cad.: '{2:dd/MM/yyyy}'", producto.descripcion, serie, fecha_caducidad));
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando cambios
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la reducción de existencias de este elemento del inventario, mediante la generación de una línea de salida de inventario.
        /// </summary>
        /// <param name="cantidad">Cantidad que será extraida del inventario (valor sin signo)</param>
        /// <param name="id_salida">Id de Registro Salida al que se asocia el movimiento del inventario</param>
        /// <param name="id_tabla">Id de la Tabla de Origen (entidad) a la que debe vincularse</param>
        /// <param name="id_registro">Id de Registro (perteneciente a la tabla) al que se vincula</param>
        /// <param name="precio_salida">Precio de Salida utilizado en la transacción que provoca la reducción del inventario</param>
        /// <param name="id_usuario">Id de Usuario que reduce el inventario</param>
        /// <returns></returns>
        public RetornoOperacion ReducirExistencias(decimal cantidad, int id_salida, int id_tabla, int id_registro, decimal precio_salida, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creando entorno transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que la cantidad de existencias sea suficiente para reducir el inventario
                resultado = ValidarExistencias(cantidad);

                //Si hay suficiente cantidad en inventario
                if (resultado.OperacionExitosa)
                {
                    //Instanciando registros de entrada y salida al que se asocia
                    using (EntradaSalida entrada = new EntradaSalida(this._id_entrada_salida),
                         salida = new EntradaSalida(id_entrada_salida))
                    {
                        //Si se localizó el registro entrada
                        if (entrada.id_entrada_salida > 0)
                        {
                            //Si el registro actual está asociado a una entrada
                            if (entrada.tipo_operacion == EntradaSalida.TipoOperacion.Entrada)
                            {
                                //Si se localizó el registro salida
                                if (salida.id_entrada_salida > 0)
                                {
                                    //Instanciando almacen y producto utilizados
                                    using (Almacen almacen = new Almacen())
                                    {
                                        using (Producto producto = new Producto(this._id_producto))
                                        {
                                            //Declarando auxiliar de precio en pesos
                                            decimal precio_salida_pesos = 0;

                                            //Insertando nueva linea de inventario
                                            resultado = InsertarInventario(this._id_producto, cantidad * -1, id_salida, this.estatus, this._lote, this._serie, this._fecha_caducidad,
                                                                        id_tabla, id_registro, this._id_inventario_origen, this._posicion_x, this._posicion_y, this._posicion_z, 0,
                                                                        precio_salida, precio_salida_pesos, this._id_inventario, id_usuario);

                                            //Si se actualizó correctamente
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Instanciando el resumen del inventario
                                                using (InventarioResumen resumen = new InventarioResumen(this._id_producto, salida.id_almacen))
                                                {
                                                    //Si se localizó el resumen de invetario del producto
                                                    if (resumen.id_inventario_resumen > 0)
                                                    {
                                                        //Afectando el resumen de inventario
                                                        resultado = resumen.ActualizarExistenciasProducto(cantidad * -1, salida.fecha_entrada_salida, resultado.IdRegistro, id_usuario);

                                                        //Si no hay errores
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Comprobando existencias al momento en este elemento de inventario
                                                            if (ObtenerExistencias() == 0)
                                                            {
                                                                //Si no hay existencias se actualiza estatus de elemento inventario actual
                                                                resultado = ActualizarEstatus(Estatus.Agotado, id_usuario);

                                                                //Si hay error en la actualización de estatus
                                                                if (!resultado.OperacionExitosa)
                                                                    resultado = new RetornoOperacion(string.Format("Error al actualizar '{0}' a estatus 'Agotado'.", producto.descripcion));
                                                            }
                                                        }
                                                        //Si hay errores durante la actualización de existencias de producto
                                                        else
                                                            //Personalizando error
                                                            resultado = new RetornoOperacion(string.Format("Error al actualizar existencias totales (Prod. '{0}' Alm. '{1}').", producto.descripcion, almacen.descripcion));
                                                    }
                                                    //Si no se encontró el resumen del producto
                                                    else
                                                        resultado = new RetornoOperacion(string.Format("No fue posible recuperar la información de existencias (Prod. '{0}' Alm. '{1}').", producto.descripcion, almacen.descripcion));
                                                }
                                            }
                                            //Si no se realiza la reducción del inventario para el producto solicitado
                                            else
                                                resultado = new RetornoOperacion(string.Format("Error al reducir existencias del inventario (Prod. '{0}'): {1}", producto.descripcion, resultado.Mensaje));
                                        }
                                    }
                                }
                                //Si no se recuperó la información de salida de inventario
                                else
                                    resultado = new RetornoOperacion("No fue posible recuperar la información de salida del inventario.");
                            }
                            //Si no está asociado a una entrada al inventario
                            else
                                resultado = new RetornoOperacion("No es posible reducir este elemento, este movimiento del inventario no correcponde a una Entrada.");
                        }
                        //Si no se recuperó la información de entrada de inventario
                        else
                            resultado = new RetornoOperacion("No fue posible recuperar la información de entrada al inventario.");
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando cambios
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la reducción de existencias de este elemento del inventario, mediante la generación de una línea de salida de inventario.
        /// </summary>
        /// <param name="cantidad">Cantidad que será extraida del inventario (valor sin signo)</param>
        /// <param name="id_salida">Id de Registro Salida al que se asocia el movimiento del inventario</param>
        /// <param name="id_tabla">Id de la Tabla de Origen (entidad) a la que debe vincularse</param>
        /// <param name="id_registro">Id de Registro (perteneciente a la tabla) al que se vincula</param>
        /// <param name="id_usuario">Id de Usuario que reduce el inventario</param>
        /// <returns></returns>
        public RetornoOperacion ReducirExistencias(decimal cantidad, int id_salida, int id_tabla, int id_registro, int id_usuario)
        {
            //Instanciando producto para obtener el precio de salida actual
            using (Producto producto = new Producto(this._id_producto))
                //Afectando al inventario
                return ReducirExistencias(cantidad, id_salida, id_tabla, id_registro, producto.precio_salida, id_usuario);
        }
        /// <summary>
        /// Realiza la reducción de existencias de este elemento del inventario, mediante la generación de una línea de salida de inventario.
        /// </summary>
        /// <param name="id_producto">Id de Producto que será afectado en existencias</param>
        /// <param name="cantidad_solicitada">Cantidad que será extraida del inventario (valor sin signo)</param>
        /// <param name="id_almacen">Id de Almacén de donde se requiere reducir las existencias</param>
        /// <param name="id_salida">Id de Registro Salida al que se asocia el movimiento del inventario</param>
        /// <param name="id_tabla">Id de la Tabla de Origen (entidad) a la que debe vincularse</param>
        /// <param name="id_registro">Id de Registro (perteneciente a la tabla) al que se vincula</param>
        /// <param name="precio_salida">Precio de Salida utilizado en la transacción que provoca la reducción del inventario</param>
        /// <param name="id_usuario">Id de Usuario que reduce el inventario</param>
        /// <returns></returns>
        public static RetornoOperacion ReducirExistencias(int id_producto, decimal cantidad_solicitada, int id_almacen, int id_salida, int id_tabla, int id_registro, decimal precio_salida, int id_usuario)
        { 
            //Declarando objeto de retorno sin error
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Creando entorno transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando producto
                using (Producto producto = new Producto(id_producto))
                {
                    //Validando existencias de producto en almacén
                    using (DataTable mitExistencias = Reportes.CargaExistenciasProducto(id_producto, id_almacen))
                    {
                        //Si hay existencias
                        if (mitExistencias != null)
                        {
                            //Obteniendo existencias totales del producto solicitado
                            decimal existencias = (from DataRow r in mitExistencias.Rows
                                                   select r.Field<decimal>("CantExistencia")).Sum();

                            //Verificando que la cantidad solicitada pueda ser reducida (comparando contra existencias)
                            if (cantidad_solicitada <= existencias)
                            {
                                //Declarando auxiliar de unidades tomadas de existencias
                                decimal acumuladas = 0;

                                //Por cada elemento consultado del inventario
                                foreach (DataRow rInv in mitExistencias.Rows)
                                {
                                    //Instanciando elemento del inventario
                                    using (Inventario inv = new Inventario(Convert.ToInt32(rInv["Id"])))
                                    {
                                        //Obteniendo la cantidad a reducir
                                        decimal cantidadInventaro = (cantidad_solicitada - acumuladas) >= Convert.ToDecimal(rInv["CantExistencia"]) ? Convert.ToDecimal(rInv["CantExistencia"]) : (cantidad_solicitada - acumuladas);
                                        //Reduciendo la cantidad existente
                                        resultado = inv.ReducirExistencias(cantidadInventaro, id_salida, id_tabla, id_registro, precio_salida, id_usuario);

                                        //Si no hay errores
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizando indicador de elementos acumulados (reducidos)
                                            acumuladas += cantidadInventaro;

                                            //Si se han retirado del inventario suficientes elementos
                                            if (acumuladas == cantidad_solicitada)
                                                //Terminando iteraciones
                                                break;
                                        }
                                        //Si existe algún error al reducir de inventario 
                                        else
                                            //Terminando iteraciones
                                            break;
                                    }
                                }
                            }
                            //Si no hay suficientes existencias
                            else
                            {
                                //Instanciando unidad de medida del producto
                                using (Global.UnidadMedida um = new Global.UnidadMedida(producto.id_unidad))
                                    resultado = new RetornoOperacion(string.Format("Existencias insuficientes Prod. '{0}'. En Inventario: {0} {1}.", producto.descripcion, existencias, um.descripcion));
                            }
                        }
                        //Si no hay en existencia
                        else
                            resultado = new RetornoOperacion(string.Format("El producto '{0}' se encuentra Agotado.", producto.descripcion));
                    }
                }

                //Si no hay errores, se confirmas cambios realizados
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la reducción de existencias de este elemento del inventario, mediante la generación de una línea de salida de inventario.
        /// </summary>
        /// <param name="id_producto">Id de Producto que será afectado en existencias</param>
        /// <param name="cantidad_solicitada">Cantidad que será extraida del inventario (valor sin signo)</param>
        /// <param name="id_almacen">Id de Almacén de donde se requiere reducir las existencias</param>
        /// <param name="id_salida">Id de Registro Salida al que se asocia el movimiento del inventario</param>
        /// <param name="id_tabla">Id de la Tabla de Origen (entidad) a la que debe vincularse</param>
        /// <param name="id_registro">Id de Registro (perteneciente a la tabla) al que se vincula</param>
        /// <param name="id_usuario">Id de Usuario que reduce el inventario</param>
        /// <returns></returns>
        public static RetornoOperacion ReducirExistencias(int id_producto, decimal cantidad_solicitada, int id_almacen, int id_salida, int id_tabla, int id_registro, int id_usuario)
        {
            //Instanciando producto
            using (Producto producto = new Producto(id_producto))
                //Realizando redicción de inventario
                return ReducirExistencias(id_producto, cantidad_solicitada, id_almacen, id_salida, id_tabla, id_registro, producto.precio_salida, id_usuario);
        }
        /// <summary>
        /// Realiza la separación del elemento de inventario en 2 entidades. Se crea una nueva entidad con la cantidad especificada, dejando al elemento actual del inventario con la cantidad restante
        /// </summary>
        /// <param name="cantidad">Cantidad de elementos que tendrá la nueva entidad</param>
        /// <param name="id_usuario">Id de usuario que realiza la acción</param>
        /// <returns></returns>
        public RetornoOperacion SepararInventario(decimal cantidad, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            int id_nuevo_inventario = 0;

            //Creando entorno transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que la cantidad de existencias sea suficiente para reducir el inventario
                resultado = ValidarExistencias(cantidad, 1);

                //Si hay suficiente cantidad en inventario
                if (resultado.OperacionExitosa)
                {
                    //Creando nuevo elemento de inventario con la cantidad solicitada
                    resultado = InsertarInventario(this._id_producto, cantidad, this._id_entrada_salida, this.estatus, this._lote, this.serie, this._fecha_caducidad,
                                            this._id_tabla, this._id_registro, this._id_inventario, this._posicion_x, this._posicion_y, this._posicion_z,
                                            this._precio_entrada, this._precio_salida, this._precio_pesos, this._id_inventario_entrada, id_usuario);

                    //Si no hay errores de creación de nuevo elemento inventario
                    if (resultado.OperacionExitosa)
                    {
                        //Guarando id de nuevo registro
                        id_nuevo_inventario = resultado.IdRegistro;

                        //Actualizando cantidad de producto en el elemento de inventario actual
                        resultado = EditarInventario(this._cantidad - cantidad, this._serie, this._fecha_caducidad, this._posicion_x, this._posicion_y, this._posicion_z, id_usuario);
                        //Si hay algún error
                        if (!resultado.OperacionExitosa)
                            resultado = new RetornoOperacion(string.Format("Error al actualizar cantidad original del elemento de inventario. Cant. Original: '{0}' Cant. Nueva: '{1}'.", this._cantidad, cantidad));
                    }
                    else
                        resultado = new RetornoOperacion(string.Format("Error al crear nuevo elemento de inventario con cantidad '{0}'.", cantidad));
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Asignando Id de nuevo inventario creado
                    resultado = new RetornoOperacion(id_nuevo_inventario);
                    //Confirmando cambios realizados
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Extrae la 'cantidad_elementos' disponible(s) en este elemento del inventario, creando un nuevo elemento con el producto de: 'cantidad_elementos' * 'cantidad_contenido'
        /// </summary>
        /// <param name="cantidad_elementos">Cantidad de elementos de este inventario que serán afectados (Ej. El inventario posee 3 cajas, la cantidad para extraer su contenido puede ser un valor de 1 a 3)</param>
        /// <param name="id_usuario">Id de Usuario que extrae el contenido del elemento de inventario</param>
        /// <returns></returns>
        public RetornoOperacion ExtraerContenido(decimal cantidad_elementos, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creando entorno transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando existencias del elemento inventario
                resultado = ValidarExistencias(cantidad_elementos);

                //Si hay elementos suficientes
                if (resultado.OperacionExitosa)
                {
                    //Determinando si es necesario separar el contenido del elemento antes de extraer (en base a las existencias totales)
                    decimal existencias = ObtenerExistencias();

                    //Si las existencias están por debajo de la cantidad total del elemento inventario (ya tuvo salidas) o 
                    //Si las existencias iguales a la cantidad de este elemento (no hay salidas aún) y la cantidad solicitada es menor a las existencias
                    if (existencias < this._cantidad || (existencias == this._cantidad && cantidad_elementos < existencias))
                    {
                        //Separando contenido de elemento de inventario
                        resultado = SepararInventario(cantidad_elementos, id_usuario);

                        //Si se separó correctamente
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciando nuevo elemento creado por la separación del elemento actual
                            using (Inventario nvo_inventario = new Inventario(resultado.IdRegistro))
                                //Se extrae todo el contenido del elemento separado
                                resultado = nvo_inventario.ExtraerContenido(id_usuario);
                        }
                    }
                    //Si aún no hay salidas de este elemento y la cantidad de elementos solicitados es igual a las existencias
                    else if (existencias == this._cantidad)
                        //Se realiza extracción de contenido
                        resultado = ExtraerContenido(id_usuario);
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando cambios
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Extrae el contenido de este elemento del inventario, creando un nuevo elemento con el total de las unidades contenidas por pieza de este elemento
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que extrae el contenido del elemento de inventario</param>
        /// <returns></returns>
        public RetornoOperacion ExtraerContenido(int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que el estatus actual del elemento de inventario permita la acción
            if (this.estatus != Estatus.Desmontado && this.estatus != Estatus.Agotado)
            {
                //Creando entorno transaccional
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Se instancía el producto desde catálogo
                    using (Producto producto = new Producto(this._id_producto))
                    {
                        //Si se localiza el producto
                        if (producto.id_producto > 0)
                        {
                            //Si tiene producto contenido
                            if (producto.id_producto_contenido > 0)
                            {
                                //Obteniendo existencias de producto
                                decimal existencias = ObtenerExistencias();

                                //Calculando cantidad a extraer
                                decimal cantidad = existencias * producto.cantidad_contenido;
                                //Calculando precio de entrada, salida y en pesos
                                decimal precio_e = this._precio_entrada / producto.cantidad_contenido;
                                decimal precio_p = this._precio_pesos / producto.cantidad_contenido;

                                //Creando el nuevo elemento del inventario
                                resultado = IncrementarExistencias(producto.id_producto_contenido, cantidad, this._id_entrada_salida,
                                                    this._lote, this.serie, this.fecha_caducidad, this._id_tabla, this._id_registro, this._id_inventario,
                                                    this._posicion_x, this._posicion_y, this._posicion_z, precio_e, precio_p, id_usuario);

                                //Si se insertó correctamente
                                if (resultado.OperacionExitosa)
                                {
                                    //Actualizando estatus de registro inventario actual
                                    resultado = ActualizarEstatus(Estatus.Desmontado, id_usuario);

                                    //Si no hay errores de actualización de estatus, se actualizan existencias en resumen de inventario
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Instanciando entrada al almacen
                                        using (EntradaSalida entrada = new EntradaSalida(this._id_entrada_salida))
                                        {
                                            //Obteniendo el almacén al que se realizó la entrada
                                            using (Almacen almacen = new Almacen(entrada.id_almacen))
                                            {
                                                //Instanciando el resumen del inventario original
                                                using (InventarioResumen resumen = new InventarioResumen(this._id_producto, almacen.id_almacen))
                                                {
                                                    //Si se localizó el resumen de invetario del producto
                                                    if (resumen.id_inventario_resumen > 0)
                                                    {
                                                        //Afectando el resumen de inventario
                                                        resultado = resumen.ActualizarExistenciasProducto(this._cantidad * -1, entrada.fecha_entrada_salida, this._id_inventario, id_usuario);

                                                        //Si hay errores de actualización de total
                                                        if (!resultado.OperacionExitosa)
                                                            //Personalizando error
                                                            resultado = new RetornoOperacion(string.Format("Error al actualizar existencias totales (Prod. '{0}' Alm. '{1}').", producto.descripcion, almacen.descripcion));
                                                    }
                                                    //Si no se encontró el resumen del producto
                                                    else
                                                        resultado = new RetornoOperacion(string.Format("No fue posible recuperar la información de existencias (Prod. '{0}' Alm. '{1}').", producto.descripcion, almacen.descripcion));
                                                }
                                            }
                                        }
                                    }
                                    //Si hay algún error
                                    else
                                        //Personalizando error
                                        resultado = new RetornoOperacion("Error al actualizar estatus de inventario origen.");
                                }
                                //Si no se insertó el nuevo inventario
                                else
                                {
                                    //Instanciando nuevo producto
                                    using (Producto nvo_producto = new Producto(producto.id_producto_contenido))
                                        resultado = new RetornoOperacion(string.Format("Error al extraer Producto '{0}'.", nvo_producto.descripcion));
                                }
                            }
                            //Si no hay producto contenido
                            else
                                resultado = new RetornoOperacion(string.Format("El producto '{0}' no tiene algún producto contenido que extraer.", producto.descripcion));
                        }
                        //Si no se localiza el producto
                        else
                            resultado = new RetornoOperacion(string.Format("No fue posible localizar el Producto ID '{0}'.", this._id_producto));
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Confirmando cambios
                        scope.Complete();
                }
            }
            //Si el estatus no es válido para la acción
            else
                resultado = new RetornoOperacion(string.Format("El estatus actual del elemento de inventario es '{0}', no es posible realizar esta acción.", this.estatus));

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Obtener los Registros del Inventario de un Producto
        /// </summary>
        /// <param name="id_producto">Producto Solicitado</param>
        /// <returns></returns>
        public static DataTable ObtieneProductoInvetario(int id_producto)
        {
            //Declarando Objeto de Retorno
            DataTable dtInventarioProducto = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 5, 0, id_producto, 0.00, 0, 0, "", "", null, 0, 0, 0, 0, 0, 0, 0.00, 0.0, 0.00, 0, 0, false, "", "" };

            //Generando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Recuperando existencias
                    dtInventarioProducto = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtInventarioProducto;
        }
        /// <summary>
        /// Método encargado de Obtener los Registros de Inventario de un Detalle de Requisición
        /// </summary>
        /// <param name="id_detalle_requisicion">Detalle de Requisición</param>
        /// <returns></returns>
        public static DataTable ObtieneInventarioRequisicionDetalle(int id_detalle_requisicion)
        {
            //Declarando Objeto de Retorno
            DataTable dtInventario = null;

            //Construyendo arreglo de valores para generación de consulta
            object[] param = { 6, 0, 0, 0.00, 0, 0, "", "", null, 0, id_detalle_requisicion, 0, 0, 0, 0, 0.00, 0.0, 0.00, 0, 0, false, "", "" };

            //Generando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Recuperando existencias
                    dtInventario = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtInventario;
        }

        #endregion
    }
}
