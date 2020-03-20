using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// 
    /// </summary>
    public class DevolucionFaltanteDetalle : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Expresa la Razon del Detalle
        /// </summary>
        public enum RazonDetalle
        {
            /// <summary>
            /// Catalogo
            /// </summary>
            Dano = 1,
            /// <summary>
            /// Producto Faltante
            /// </summary>
            Faltante,
            /// <summary>
            /// Producto Sobrante
            /// </summary>
            Sobrante,
            /// <summary>
            /// PB Incorrecto
            /// </summary>
            PBIncorrecto,
            /// <summary>
            /// No Pedido
            /// </summary>
            NoPedido,
            /// <summary>
            /// Código Cambiado
            /// </summary>
            CodigoCambiado,
            /// <summary>
            /// Ordenes de Compras Saldadas
            /// </summary>
            OrdenesComprasSaldadas,
            /// <summary>
            /// Caducidad
            /// </summary>
            Caducidad
        }
        /// <summary>
        /// Enumeración que expresa la Devolución
        /// </summary>
        public enum EstatusDevolucionDetalle
        {
            /// <summary>
            /// Estatus Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Estatus en Proceso
            /// </summary>
            EnProceso,
            /// <summary>
            /// Estatus Terminado
            /// </summary>
            Terminado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "despacho.sp_devolucion_faltante_detalle_tddf";

        private int _id_devolucion_faltante_detalle;
        /// <summary>
        /// Atributo que Almacena el Id del Detalle de la Devolución
        /// </summary>
        public int id_devolucion_faltante_detalle { get { return this._id_devolucion_faltante_detalle; } }
        private int _id_devolucion_faltante;
        /// <summary>
        /// Atributo que Almacena el Id de la Devolución
        /// </summary>
        public int id_devolucion_faltante { get { return this._id_devolucion_faltante; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo que almacena el Estatus del Detalle
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        private int _id_producto_devolucion;
        /// <summary>
        /// Atributo que Almacena el Producto
        /// </summary>
        public int id_producto_devolucion { get { return this._id_producto_devolucion; } }
        private decimal _cantidad;
        /// <summary>
        /// Atributo que Almacena la Cantidad
        /// </summary>
        public decimal cantidad { get { return this._cantidad; } }
        private byte _id_unidad;
        /// <summary>
        /// Atributo que Almacena la Unidad
        /// </summary>
        public byte id_unidad { get { return this._id_unidad; } }
        private string _codigo_producto;
        /// <summary>
        /// Atributo que Almacena el Código del Producto
        /// </summary>
        public string codigo_producto { get { return this._codigo_producto; } }
        private string _descripcion_producto;
        /// <summary>
        /// Atributo que Almacena la Descripción del Producto
        /// </summary>
        public string descripcion_producto { get { return this._descripcion_producto; } }
        private byte _id_razon_detalle;
        /// <summary>
        /// Atributo que Almacena la Razón del Detalle
        /// </summary>
        public byte id_razon_detalle { get { return this._id_razon_detalle; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que Almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Contructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public DevolucionFaltanteDetalle()
        {
            //Asignando Valores
            this._id_devolucion_faltante_detalle = 0;
            this._id_devolucion_faltante = 0;
            this._id_estatus = 0;
            this._id_producto_devolucion = 0;
            this._cantidad = 0;
            this._id_unidad = 0;
            this._codigo_producto = "";
            this._descripcion_producto = "";
            this._id_razon_detalle = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_devolucion_faltante_detalle">Detalle de la Devolución</param>
        public DevolucionFaltanteDetalle(int id_devolucion_faltante_detalle)
        {
            //Asignando Valores
            cargaAtributosInstancia(id_devolucion_faltante_detalle);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~DevolucionFaltanteDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos de la Clase 
        /// </summary>
        /// <param name="id_devolucion_faltante">Devolución Faltante</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_devolucion_faltante_detalle)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_devolucion_faltante_detalle, 0, 0, 0, 0, 0, "", "", 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_devolucion_faltante_detalle = id_devolucion_faltante_detalle;
                        this._id_devolucion_faltante = Convert.ToInt32(r["IdDevolucionFaltante"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_producto_devolucion = Convert.ToInt32(r["IdProductoDevolucion"]);
                        this._cantidad = Convert.ToDecimal(r["Cantidad"]);
                        this._id_unidad = Convert.ToByte(r["IdUnidad"]);
                        this._codigo_producto = r["CodigoProducto"].ToString();
                        this._descripcion_producto = r["DescripcionProducto"].ToString();
                        this._id_razon_detalle = Convert.ToByte(r["IdRazonDetalle"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_devolucion_faltante">Devolución Faltante</param>
        /// <param name="id_estatus">Estatus del Detalle</param>
        /// <param name="id_producto_devolucion">Producto Devolución</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="codigo_producto">Código del Producto</param>
        /// <param name="descripcion_producto">Descripción del Producto</param>
        /// <param name="id_razon_detalle">Razon del Detalle</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_devolucion_faltante, byte id_estatus, int id_producto_devolucion, decimal cantidad, byte id_unidad, string codigo_producto,
                                                string descripcion_producto, byte id_razon_detalle, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_devolucion_faltante_detalle, id_devolucion_faltante, id_estatus, id_producto_devolucion, cantidad, 
                               id_unidad, codigo_producto, descripcion_producto, id_razon_detalle, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Detalles de la Devolución
        /// </summary>
        /// <param name="id_devolucion_faltante">Devolución Faltante</param>
        /// <param name="id_producto_devolucion">Producto de la Devolución</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="codigo_producto">Código del Producto</param>
        /// <param name="descripcion_producto">Descripción del Producto</param>
        /// <param name="razon_detalle">Razon del Detalle</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDevolucionFaltanteDetalle(int id_devolucion_faltante, int id_producto_devolucion, decimal cantidad, byte id_unidad, string codigo_producto,
                                                string descripcion_producto, RazonDetalle razon_detalle, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_devolucion_faltante, (byte)EstatusDevolucionDetalle.Registrado, id_producto_devolucion, cantidad, 
                               id_unidad, codigo_producto, descripcion_producto, (byte)razon_detalle, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Insertar los Detalles de la Devolución
        /// </summary>
        /// <param name="id_devolucion_faltante">Devolución Faltante</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="estatus">Estatus del Detalle</param>
        /// <param name="id_producto_devolucion">Producto de la Devolución</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="codigo_producto">Código del Producto</param>
        /// <param name="descripcion_producto">Descripción del Producto</param>
        /// <param name="razon_detalle">Razon del Detalle</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaDevolucionFaltanteDetalle(int id_devolucion_faltante, EstatusDevolucionDetalle estatus, int id_producto_devolucion,
                                                decimal cantidad, byte id_unidad, string codigo_producto, string descripcion_producto, RazonDetalle razon_detalle, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable Auxliar
            int id_detalle = 0;
            
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invocando Método de Actualización
                result = this.actualizaRegistrosBD(id_devolucion_faltante, (byte)estatus, id_producto_devolucion, cantidad, id_unidad, codigo_producto,
                                     descripcion_producto, (byte)razon_detalle, id_usuario, this._habilitar);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Obteniendo Detalle
                    id_detalle = result.IdRegistro;
                    
                    //Validando que el Estatus sea Distinto
                    if ((EstatusDevolucionDetalle)this._id_estatus != estatus)
                    {
                        //Instanciando Devolución
                        using (DevolucionFaltante devolucion = new DevolucionFaltante(id_devolucion_faltante))
                        {
                            //Validando existencia
                            if (devolucion.habilitar)

                                //Actualizando Estatus
                                result = devolucion.ActualizaEstatusDevolucionFaltante(ObtieneEstatusDevolucion(id_devolucion_faltante), id_usuario);
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe la Devolución");
                        }
                    }
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Instanciando Registro
                    result = new RetornoOperacion(id_detalle);

                    //Completando transacción
                    trans.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Deshabilitar el Detalle de la Devolución
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDevolucionFaltanteDetalle(int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistrosBD(this._id_devolucion_faltante, this._id_estatus, this._id_producto_devolucion, this._cantidad, this._id_unidad, this._codigo_producto,
                                 this._descripcion_producto, this._id_razon_detalle, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus del Detalle
        /// </summary>
        /// <param name="estatus">Estatus del Detalle</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusDevolucionFaltanteDetalle(EstatusDevolucionDetalle estatus, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistrosBD(this._id_devolucion_faltante, (byte)estatus, this._id_producto_devolucion, this._cantidad, this._id_unidad, this._codigo_producto,
                                 this._descripcion_producto, this._id_razon_detalle, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar el Detalle de la Devolución Faltante
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDevolucionFaltanteDetalle()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_devolucion_faltante_detalle);
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles dada un Devolución
        /// </summary>
        /// <param name="id_devolucion">Devolución</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesDevolucion(int id_devolucion_faltante)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_devolucion_faltante, 0, 0, 0, 0, "", "", 0, 0, false, "", "" };

            //Instanciando Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtDetalles = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtDetalles;
        }
        /// <summary>
        /// Método encargado de Obtener el Estatus General de la Devolución
        /// </summary>
        /// <param name="id_devolucion_faltante">Encabezado de la Devolución</param>
        /// <returns></returns>
        public static DevolucionFaltante.EstatusDevolucion ObtieneEstatusDevolucion(int id_devolucion_faltante)
        {
            //Declarando Objeto de Retorno
            DevolucionFaltante.EstatusDevolucion estatus = DevolucionFaltante.EstatusDevolucion.Registrado;

            //Instanciando Devolución
            using (DevolucionFaltante devolucion = new DevolucionFaltante(id_devolucion_faltante))
            {
                //Armando Arreglo de Parametros
                object[] param = { 5, 0, id_devolucion_faltante, 0, 0, 0, 0, "", "", 0, 0, false, "", "" };

                //Instanciando Reporte
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Fila
                        foreach (DataRow dr in ds.Tables["Table"].Rows)
                        
                            //Asignando Estatus Obtenido
                            estatus = (DevolucionFaltante.EstatusDevolucion)Convert.ToByte(dr["IdEstatus"]);
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return estatus;
        }

        #endregion
    }
}
