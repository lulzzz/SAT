using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TSDK.Base;
using System.Linq;
using TSDK.Datos;
using SAT_CL.Despacho;

namespace SAT_CL.Documentacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones de los Productos de los Servicios
    /// </summary>
    public class ServicioProducto : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Tipo de evento de producto a servicio
        /// </summary>
        public enum TipoEventoServicioProducto
        {
            /// <summary>
            /// Carga de producto
            /// </summary>
            CargarProducto = 1,
            /// <summary>
            /// Descarga de producto
            /// </summary>
            DescargarProducto = 2
            //Importante: No se implementa de modo general el tipo 3 (Descarga Especial) 
            //debido a que se sustituye en BD por tipo 2 en algunas consultas que así lo requieran

        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "documentacion.sp_servicio_producto_tsp";

        private int _id_servicio_producto;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Producto del Servicio
        /// </summary>
        public int id_servicio_producto
        {
            get { return this._id_servicio_producto; }
        }

        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Servicio
        /// </summary>
        public int id_servicio
        {
            get { return this._id_servicio; }
        }

        private int _id_parada;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Parada
        /// </summary>
        public int id_parada
        {
            get { return this._id_parada; }
        }

        private byte _id_tipo;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Tipo
        /// </summary>
        public byte id_tipo
        {
            get { return this._id_tipo; }
        }

        private int _id_producto;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Producto
        /// </summary>
        public int id_producto
        {
            get { return this._id_producto; }
        }

        private decimal _cantidad;
        /// <summary>
        /// Atributo encargado de almacenar la Cantidad
        /// </summary>
        public decimal cantidad
        {
            get { return this._cantidad; }
        }

        private int _id_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Unidad
        /// </summary>
        public int id_unidad
        {
            get { return this._id_unidad; }
        }

        private decimal _peso;
        /// <summary>
        /// Atributo encargado de almacenar el Peso
        /// </summary>
        public decimal peso
        {
            get { return this._peso; }
        }

        private int _id_unidad_peso;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Unidad de Peso
        /// </summary>
        public int id_unidad_peso
        {
            get { return this._id_unidad_peso; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ServicioProducto()
        {   //Invocando Método de Actualización
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ServicioProducto(int id_registro)
        {   //Invocando Método de Actualización
            cargaAtributosInstancia(id_registro);
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id de Registro con Transacción SQL
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="trans">Transacción SQL</param>
        public ServicioProducto(int id_registro, SqlTransaction trans)
        {   //Invocando Método de Actualización con Transacción SQL
            cargaAtributosInstancia(id_registro, trans);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ServicioProducto()
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
            this._id_servicio_producto = 0;
            this._id_servicio = 0;
            this._id_parada = 0;
            this._id_tipo = 0;
            this._id_producto = 0;
            this._cantidad = 0;
            this._id_unidad = 0;
            this._peso = 0;
            this._id_unidad_peso = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la Tabla contenga registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada una de las Celdas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_servicio_producto = id_registro;
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_parada = Convert.ToInt32(dr["IdParada"]);
                        this._id_tipo = Convert.ToByte(dr["IdTipo"]);
                        this._id_producto = Convert.ToInt32(dr["IdProducto"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._peso = Convert.ToDecimal(dr["Peso"]);
                        this._id_unidad_peso = Convert.ToInt32(dr["IdUnidadPeso"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }//Asignando Objeto a Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro con Transacción SQL
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="trans">Transacción SQL</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro, SqlTransaction trans)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param, trans))
            {   //Validando que la Tabla contenga registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada una de las Celdas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_servicio_producto = id_registro;
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_parada = Convert.ToInt32(dr["IdParada"]);
                        this._id_tipo = Convert.ToByte(dr["IdTipo"]);
                        this._id_producto = Convert.ToInt32(dr["IdProducto"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._peso = Convert.ToDecimal(dr["Peso"]);
                        this._id_unidad_peso = Convert.ToInt32(dr["IdUnidadPeso"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }//Asignando Objeto a Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_tipo">Id de Tipo</param>
        /// <param name="id_producto">Id de Producto</param>
        /// <param name="cantidad">Cantidad del Producto</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="peso">Peso</param>
        /// <param name="id_unidad_peso">Id de Unidad de Peso</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_servicio, int id_parada, byte id_tipo, int id_producto,
                                                    decimal cantidad, int id_unidad, decimal peso, int id_unidad_peso,
                                                    int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de parametros
            object[] param = { 2, this._id_servicio_producto, id_servicio, id_parada, id_tipo, id_producto, cantidad, 
                               id_unidad, peso, id_unidad_peso, id_usuario, habilitar, "",	"" };
            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD con Transacción SQL
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_tipo">Id de Tipo</param>
        /// <param name="id_producto">Id de Producto</param>
        /// <param name="cantidad">Cantidad del Producto</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="peso">Peso</param>
        /// <param name="id_unidad_peso">Id de Unidad de Peso</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <param name="trans">Transacción SQL</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_servicio, int id_parada, byte id_tipo, int id_producto,
                                                    decimal cantidad, int id_unidad, decimal peso, int id_unidad_peso,
                                                    int id_usuario, bool habilitar, SqlTransaction trans)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de parametros
            object[] param = { 2, this._id_servicio_producto, id_servicio, id_parada, id_tipo, id_producto, cantidad, 
                               id_unidad, peso, id_unidad_peso, id_usuario, habilitar, "",	"" };
            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param, trans);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar Productos de Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_tipo">Id de Tipo</param>
        /// <param name="id_producto">Id de Producto</param>
        /// <param name="cantidad">Cantidad del Producto</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="peso">Peso</param>
        /// <param name="id_unidad_peso">Id de Unidad de Peso</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioProducto(int id_servicio, int id_parada, byte id_tipo, int id_producto,
                                                               decimal cantidad, int id_unidad, decimal peso, int id_unidad_peso,
                                                               int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validamos que exista la Parada
            if (Parada.ValidaExistenciaParada(id_parada))
            {
                //Validamos que exista el evento
                if (ParadaEvento.ValidaExistenciaEvento(id_parada, id_tipo))
                {
                    //Armando arreglo de parametros
                    object[] param = { 1, 0, id_servicio, id_parada, id_tipo, id_producto, cantidad, 
                               id_unidad, peso, id_unidad_peso, id_usuario, true, "",	"" };
                    //Obteniendo resultado del SP
                    result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                }
                else
                {
                    //Establecemos Mensaje
                    result = new RetornoOperacion("El evento no se encuentra activo.");
                }
            }
            else
            {
                //Establecemos Mensaje
                result = new RetornoOperacion("La parada no se encuentra activa.");
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Público encargado de Insertar Productos de Servicio con Transacción SQL
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_tipo">Id de Tipo</param>
        /// <param name="id_producto">Id de Producto</param>
        /// <param name="cantidad">Cantidad del Producto</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="peso">Peso</param>
        /// <param name="id_unidad_peso">Id de Unidad de Peso</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="trans">Transacción SQL</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioProducto(int id_servicio, int id_parada, byte id_tipo, int id_producto,
                                                               decimal cantidad, int id_unidad, decimal peso, int id_unidad_peso,
                                                               int id_usuario, SqlTransaction trans)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de parametros
            object[] param = { 1, 0, id_servicio, id_parada, id_tipo, id_producto, cantidad, 
                               id_unidad, peso, id_unidad_peso, id_usuario, true, "",	"" };
            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param, trans);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar Productos de Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_tipo">Id de Tipo</param>
        /// <param name="id_producto">Id de Producto</param>
        /// <param name="cantidad">Cantidad del Producto</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="peso">Peso</param>
        /// <param name="id_unidad_peso">Id de Unidad de Peso</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioProducto(int id_servicio, int id_parada, byte id_tipo, int id_producto,
                                                      decimal cantidad, int id_unidad, decimal peso, int id_unidad_peso,
                                                      int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_servicio, id_parada, id_tipo, id_producto, cantidad,
                               id_unidad, peso, id_unidad_peso, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Editar Productos de Servicio con Transacción SQL
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_tipo">Id de Tipo</param>
        /// <param name="id_producto">Id de Producto</param>
        /// <param name="cantidad">Cantidad del Producto</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="peso">Peso</param>
        /// <param name="id_unidad_peso">Id de Unidad de Peso</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="trans">Transacción SQL</param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioProducto(int id_servicio, int id_parada, byte id_tipo, int id_producto,
                                                      decimal cantidad, int id_unidad, decimal peso, int id_unidad_peso,
                                                      int id_usuario, SqlTransaction trans)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_servicio, id_parada, id_tipo, id_producto, cantidad,
                               id_unidad, peso, id_unidad_peso, id_usuario, this._habilitar, trans);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar Productos de Servicio
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioProducto(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_servicio, this._id_parada, this._id_tipo, id_producto, this._cantidad,
                               id_unidad, peso, id_unidad_peso, id_usuario, false);
        }
        /// <summary>
        /// Deshabilita Productos de una parada solicitada
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaProductosParada(int id_parada, int id_usuario)
        {
            //Declaramos resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_parada);

            //Inicializando bloque transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos Productos asociados a la parada
                using (DataTable mitProductos = CargaProductosParadaParaDeshabilitacion(id_parada))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mitProductos))
                    {
                        //Recorremos cada uno de los Productos
                        foreach (DataRow r in mitProductos.Rows)
                        {
                            //Instanciamos cada uno de los Productos
                            using (ServicioProducto objServicioProducto = new ServicioProducto(r.Field<int>("Id")))
                                //Deshabilitamos cada uno de los Productos
                                resultado = objServicioProducto.DeshabilitaServicioProducto(id_usuario);

                            //Si hay errores de deshabilitación
                            if (!resultado.OperacionExitosa)
                                break;
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando transacción
                    scope.Complete();
            }

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar Productos de Servicio con Transacción SQL
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaServiciosProductos(int id_servicio, int id_usuario)
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Productos ligando el Id Servicio
            using (DataTable mitProductos = CargaProductosServicioParaDeshabilitacion(id_servicio))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitProductos))
                {
                    //Recorremos cada uno de los Productos
                    foreach (DataRow r in mitProductos.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos cada uno de los Productos
                            using (ServicioProducto objServicioProducto = new ServicioProducto(r.Field<int>("Id")))
                            {
                                //Deshabilitamos cada uno de los Productos
                                resultado = objServicioProducto.DeshabilitaServicioProducto(id_usuario);
                            }
                        }
                        else
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método Público encargado de Actualizar el Producto del Servicio
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicioProducto()
        {   //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_servicio_producto);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Producto del Servicio con Transacción SQL
        /// </summary>
        /// <param name="trans">Transacción SQL</param>
        /// <returns></returns>
        public bool ActualizaServicioProducto(SqlTransaction trans)
        {   //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_servicio_producto, trans);
        }
        /// <summary>
        /// Método Público encargado de Obtener los Productos Ligados a un Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneProductosServicio(int id_servicio)
        {   //Declarando Tabla de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando resultado Obtenido
                    dt = ds.Tables["Table"];
            }//Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        ///Recupera los Productos Ligados a una parada específica
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <returns></returns>
        public static DataTable ObtieneProductosParada(int id_parada)
        {   //Declarando Tabla de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 6, 0, 0, id_parada, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando resultado Obtenido
                    dt = ds.Tables["Table"];
            }//Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Obtiene el Total de Productos ligado a un Id de Parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static int ObtieneTotalProductosDeParada(int id_parada)
        {
            //Declaramos Resultados
            int Total = 0;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, id_parada, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    Total = (from DataRow r in ds.Tables[0].Rows
                             select Convert.ToInt32(r["Total"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return Total;
        }
        /// <summary>
        /// Recupera la sumatoria total de los productos medidos por cantidad de piezas, asociados al servicio solicitado
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static decimal ObtieneTotalCantidadServicio(int id_servicio)
        {
            //Obteniendo todos los productos del servicio
            using (DataTable productos = ServicioProducto.ObtieneProductosServicio(id_servicio))
            {
                //Devolviendo sumatoria de cantidades de interés (Unidades de conteo y carga de producto)
                return (from DataRow p in productos.Rows
                        where p.Field<byte>("IdTipoUniCant") == 5 && p.Field<byte>("IdTipo") == 1
                        select p.Field<decimal>("Cantidad")).DefaultIfEmpty().Sum();
            }
        }
        /// <summary>
        /// Recupera la sumatoria total de los productos medidos por peso, asociados al servicio solicitado
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa (Determina la conversión de unidades a las predeterminadas para una base de tarifa)</param>
        /// <returns></returns>
        public static decimal ObtieneTotalPesoServicio(int id_servicio, int id_base_tarifa)
        {
            //Declarando objeto de retorno
            decimal peso = 0;

            //Instanciando base de tarifa
            using (Tarifas.BaseTarifa base_tarifa = new Tarifas.BaseTarifa(id_base_tarifa))
            {
                //Obteniendo todos los productos del servicio
                using (DataTable productos = ServicioProducto.ObtieneProductosServicio(id_servicio))
                {
                    //Validando la existencia del origen de datos
                    if (Validacion.ValidaOrigenDatos(productos))
                    {
                        //Para cada uno de los elemento
                        foreach (DataRow p in (from DataRow p in productos.Rows
                                               where p.Field<byte>("IdTipoUniPeso") == 2 && p.Field<byte>("IdTipo") == 1
                                               select p).DefaultIfEmpty())
                        {
                            //Si no es un elemento vacío
                            if (p != null)
                            {
                                //Determinando si es requerido realizar una conversión de unidades
                                if (Convert.ToInt32(p["IdUniPeso"]) != base_tarifa.id_unidad_medida)
                                    //Realizando la conversión de unidad
                                    peso += Global.UnidadMedidaConversion.ConvierteCantidadUnidadMedida(Convert.ToInt32(p["IdUniPeso"]), base_tarifa.id_unidad_medida, Convert.ToDecimal(p["Peso"]));
                                //Si no se requiere convertir
                                else
                                    peso += Convert.ToDecimal(p["Peso"]);
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return peso;
        }
        /// <summary>
        /// Recupera la sumatoria total de los productos medidos por volúmen, asociados al servicio solicitado
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa (Determina la conversión de unidades a las predeterminadas para una base de tarifa)</param>
        /// <returns></returns>
        public static decimal ObtieneTotalVolumenServicio(int id_servicio, int id_base_tarifa)
        {
            //Declarando objeto de retorno
            decimal volumen = 0;

            //Instanciando base de tarifa
            using (Tarifas.BaseTarifa base_tarifa = new Tarifas.BaseTarifa(id_base_tarifa))
            {
                //Obteniendo todos los productos del servicio
                using (DataTable productos = ServicioProducto.ObtieneProductosServicio(id_servicio))
                {
                    //Validando la existencia del origen de datos
                    if (Validacion.ValidaOrigenDatos(productos))
                    {
                        //Para cada uno de los elemento
                        foreach (DataRow p in (from DataRow p in productos.Rows
                                               where p.Field<byte>("IdTipoUniPeso") == 4 && p.Field<byte>("IdTipo") == 1
                                               select p).DefaultIfEmpty())
                        {
                            //Si no es un elemento vacío
                            if (p != null)
                            {
                                //Determinando si es requerido realizar una conversión de unidades
                                if (Convert.ToInt32(p["IdUniPeso"]) != base_tarifa.id_unidad_medida)
                                    //Realizando la conversión de unidad
                                    volumen += Global.UnidadMedidaConversion.ConvierteCantidadUnidadMedida(Convert.ToInt32(p["IdUniPeso"]), base_tarifa.id_unidad_medida, Convert.ToDecimal(p["Peso"]));
                                //Si no se requiere convertir
                                else
                                    volumen += Convert.ToDecimal(p["Peso"]);
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return volumen;
        }
        /// <summary>
        /// Recupera el id de producto que fue añadido como primer elemento cargado en el servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static int ObtieneProductoPrincipal(int id_servicio)
        {
            //Obteniendo todos los productos del servicio
            using (DataTable productos = ServicioProducto.ObtieneProductosServicio(id_servicio))
            {
                //Devolviendo sumatoria de cantidades de interés (Unidades de medición de volumen y carga de producto)
                return (from DataRow p in productos.Rows
                        where p.Field<byte>("IdTipo") == 1
                        orderby p.Field<int>("Id")
                        select p.Field<int>("IdProducto")).FirstOrDefault();
            }
        }

        /// <summary>
        /// Método Público encargado de Obtener los Productos Ligados a un Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaProductosServicioParaDeshabilitacion(int id_servicio)
        {   //Declarando Tabla de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 7, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando resultado Obtenido
                    dt = ds.Tables["Table"];
            }//Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        ///Obtiene los Productos Ligados a una Parada indicada
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <returns></returns>
        public static DataTable CargaProductosParadaParaDeshabilitacion(int id_parada)
        {   //Declarando Tabla de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 10, 0, 0, id_parada, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando resultado Obtenido
                    dt = ds.Tables["Table"];
            }//Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Obtiene el Total de Productos ligado a un Id de Parada y un Tipo de evento
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_tipo_evento">Id Tipo Evento correspondiente a la parada</param>
        /// <returns></returns>
        public static int ObtieneTotalProductosDeParadaEvento(int id_parada, byte id_tipo_evento)
        {
            //Declaramos Resultados
            int Total = 0;

            //Armando Arreglo de Parametros
            object[] param = { 8, 0, 0, id_parada, id_tipo_evento, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    Total = (from DataRow r in ds.Tables[0].Rows
                             select Convert.ToInt32(r["Total"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return Total;
        }

        /// <summary>
        /// Obtiene los productos con saldo en peso, cantidad o volúmen del servicio y hasta la parada indicada
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="secuencia_parada">Secuencia de parada hasta la que se contabilizarán los productos cargados y/o descargados</param>
        /// <returns></returns>
        public static DataTable ObtieneSaldoProductoServicio(int id_servicio, decimal secuencia_parada)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 9, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, false, secuencia_parada, "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existen registros coincidentes
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Recupera el primer producto encontrado con saldo de carga / descarga del mismo servicio, hasta la parada señalada
        /// </summary>
        /// <param name="id_parada">Id de Parada final de la búsqueda</param>
        /// <returns></returns>
        public static int ObtieneProductoPrincipalParada(int id_parada)
        {
            //Declarando objeto de retorno
            int id_producto = 0;

            //Instanciando parada de interés
            using (Parada parada = new Parada(id_parada))
            {
                //Si la parada existe y es de un servicio
                if (parada.habilitar && parada.secuencia_parada_servicio > 0)
                {
                    //Obteniendo todos los productos del servicio hasta la parada señalada
                    using (DataTable productos = ObtieneSaldoProductoServicio(parada.id_servicio, parada.id_parada))
                    {
                        //Obteniendo y devolviendo el primer producto por descargar en la parada
                        id_producto = (from DataRow p in productos.Rows
                                       orderby p.Field<int>("Id")
                                       select p.Field<int>("IdProducto")).FirstOrDefault();
                    }
                }
            }

            //Devolviendo resultado
            return id_producto;
        }
        /// <summary>
        /// Recupera la sumatoria total de los productos del mismo servicio, medidos por cantidad de piezas (con saldo de carga / descarga), hasta la parada indicada
        /// </summary>
        /// <param name="id_parada">Id de Parada final de la sumatoria</param>
        /// <returns></returns>
        public static decimal ObtieneTotalCantidadParada(int id_parada)
        {
            //Declarando objeto de retorno
            decimal cantidad = 0;

            //Instanciando parada de interés
            using (Parada parada = new Parada(id_parada))
            {
                //Si la parada existe y es de un servicio
                if (parada.habilitar && parada.secuencia_parada_servicio > 0)
                {
                    //Obteniendo todos los productos del servicio hasta la parada señalada
                    using (DataTable productos = ObtieneSaldoProductoServicio(parada.id_servicio, parada.id_parada))
                    {
                        //Devolviendo sumatoria de cantidades de interés (Unidades de conteo y carga de producto)
                        cantidad = (from DataRow p in productos.Rows
                                    where p.Field<byte>("IdTipoUniCant") == 5
                                    select p.Field<decimal>("Cantidad")).DefaultIfEmpty().Sum();
                    }
                }
            }

            //Devolviendo resultado
            return cantidad;
        }
        /// <summary>
        /// Recupera la sumatoria total de los productos del mismo servicio, medidos por peso, hasta la parada solicitada
        /// </summary>
        /// <param name="id_parada">Id de Parada final de la sumatoria</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa (Determina la conversión de unidades a las predeterminadas para una base de tarifa)</param>
        /// <returns></returns>
        public static decimal ObtieneTotalPesoParada(int id_parada, int id_base_tarifa)
        {
            //Declarando objeto de retorno
            decimal peso = 0;

            //Instanciando base de tarifa
            using (TarifasPago.BaseTarifa base_tarifa = new TarifasPago.BaseTarifa(id_base_tarifa))
            {
                //Instanciando parada de interés
                using (Parada parada = new Parada(id_parada))
                {
                    //Si la parada existe y es de un servicio
                    if (parada.habilitar && parada.secuencia_parada_servicio > 0)
                    {
                        //Obteniendo todos los productos del servicio hasta la parada señalada
                        using (DataTable productos = ObtieneSaldoProductoServicio(parada.id_servicio, parada.secuencia_parada_servicio))
                        {
                            //Validando la existencia del origen de datos
                            if (Validacion.ValidaOrigenDatos(productos))
                            {
                                //Para cada uno de los elemento
                                foreach (DataRow p in (from DataRow p in productos.Rows
                                                       where p.Field<byte>("IdTipoUniPeso") == 2
                                                       select p).DefaultIfEmpty())
                                {
                                    //Si no es un elemento vacío
                                    if (p != null)
                                    {
                                        //Determinando si es requerido realizar una conversión de unidades
                                        if (Convert.ToInt32(p["IdUniPeso"]) != base_tarifa.id_unidad_medida)
                                            //Realizando la conversión de unidad
                                            peso += Global.UnidadMedidaConversion.ConvierteCantidadUnidadMedida(Convert.ToInt32(p["IdUniPeso"]), base_tarifa.id_unidad_medida, Convert.ToDecimal(p["Peso"]));
                                        //Si no se requiere convertir
                                        else
                                            peso += Convert.ToDecimal(p["Peso"]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return peso;
        }
        /// <summary>
        /// Recupera la sumatoria total de los productos del mismo servicio, medidos por volúmen hasta la parada solicitada
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa (Determina la conversión de unidades a las predeterminadas para una base de tarifa)</param>
        /// <returns></returns>
        public static decimal ObtieneTotalVolumenParada(int id_parada, int id_base_tarifa)
        {
            //Declarando objeto de retorno
            decimal volumen = 0;

            //Instanciando base de tarifa
            using (Tarifas.BaseTarifa base_tarifa = new Tarifas.BaseTarifa(id_base_tarifa))
            {
                //Instanciando parada de interés
                using (Parada parada = new Parada(id_parada))
                {
                    //Si la parada existe y es de un servicio
                    if (parada.habilitar && parada.secuencia_parada_servicio > 0)
                    {
                        //Obteniendo todos los productos del servicio
                        using (DataTable productos = ObtieneSaldoProductoServicio(parada.id_servicio, parada.secuencia_parada_servicio))
                        {
                            //Validando la existencia del origen de datos
                            if (Validacion.ValidaOrigenDatos(productos))
                            {
                                //Para cada uno de los elemento
                                foreach (DataRow p in (from DataRow p in productos.Rows
                                                       where p.Field<byte>("IdTipoUniPeso") == 4
                                                       select p).DefaultIfEmpty())
                                {
                                    //Si no es un elemento vacío
                                    if (p != null)
                                    {
                                        //Determinando si es requerido realizar una conversión de unidades
                                        if (Convert.ToInt32(p["IdUniPeso"]) != base_tarifa.id_unidad_medida)
                                            //Realizando la conversión de unidad
                                            volumen += Global.UnidadMedidaConversion.ConvierteCantidadUnidadMedida(Convert.ToInt32(p["IdUniPeso"]), base_tarifa.id_unidad_medida, Convert.ToDecimal(p["Peso"]));
                                        //Si no se requiere convertir
                                        else
                                            volumen += Convert.ToDecimal(p["Peso"]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return volumen;
        }
        /// <summary>
        /// Método encargado de Obtener los Productos de Carga
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneProductosCarga(int id_servicio)
        {
            //Declarando Tabla de Retorno
            DataTable dt = null;

            //Armando Arreglo de Parametros
            object[] param = { 10, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que la tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando resultado Obtenido
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Realiza la copia de los productos de una parada hacia la parada y servicio indicados
        /// </summary>        
        /// <param name="id_parada_origen">Id de Parada de Origen</param>
        /// <param name="id_servicio">Id de Servicio de destino</param>
        /// <param name="id_parada_destino">Id de Parada a la que serán copiados los productos</param>
        /// <returns></returns>
        public static RetornoOperacion CopiaProductosParadaServicio(int id_servicio, int id_parada_origen, int id_parada_destino, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_parada_destino);

            //Inicializando transacción
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando productos asociados al servicio maestro y la parada
                using (DataTable tblProductos = ServicioProducto.ObtieneProductosParada(id_parada_origen))
                {
                    //Validando la existencia de productos
                    if (Validacion.ValidaOrigenDatos(tblProductos))
                    {
                        //Para cada producto
                        foreach (DataRow rProducto in tblProductos.Rows)
                        {
                            //Copiando productos al servicio recién copiado
                            resultado = ServicioProducto.InsertaServicioProducto(id_servicio, id_parada_destino, Convert.ToByte(rProducto["IdTipo"]),
                                                                Convert.ToInt32(rProducto["IdProducto"]), Convert.ToDecimal(rProducto["Cantidad"]),
                                                                Convert.ToInt32(rProducto["IdUniCant"]), Convert.ToDecimal(rProducto["Peso"]),
                                                                Convert.ToInt32(rProducto["IdUniPeso"]), id_usuario);

                            //Si existe algún error
                            if (!resultado.OperacionExitosa)
                            {
                                //Personalizando error
                                resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar producto ({0}){1}: {2}", rProducto["Id"], rProducto["Producto"], resultado.Mensaje), resultado.OperacionExitosa);
                                //Se interrumpe el ciclo de copia de productos
                                break;
                            }
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Se confirman cambios realziados
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la carga de los eventos y productos asociados a una parada, con el formato requerido para visualización en control de usuario Documentación
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <returns></returns>
        public static DataSet CargaEventosYProductosParadaVisualizacionControlDocumentacion(int id_parada)
        { 
            //Creando conjunto de parámetros de consulta
            object[] param = { 11, 0, 0, id_parada, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Buscando elementos coincidentes
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                //Devolviendo resultado
                return ds;
        }

       /// <summary>
        /// Obtien el Id Producto Servicio de la Primer Parada del servicio
       /// </summary>
       /// <param name="id_servicio">Id Servicio</param>
       /// <returns></returns>
        public static int ObtieneProductoParadaInicial(int id_servicio)
        {
            //Declaramos Variables
            int Id = 0;
            //Creando conjunto de parámetros de consulta
            object[] param = { 12, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Realizando consulta y devolviendo resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    Id = (from DataRow r in ds.Tables[0].Rows
                               select Convert.ToInt32(r["Id"])).FirstOrDefault();

                }
                //Devolviendo resultado
                return Id;
            }
        }
    
        #endregion
    }
}
