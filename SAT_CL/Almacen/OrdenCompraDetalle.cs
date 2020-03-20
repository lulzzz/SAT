using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
namespace SAT_CL.Almacen
{
    /// <summary>
    /// Clase que eprmite la insercion, edición y DEshabilitar registros de la tabla OrdenCompraDetalle
    /// </summary>
    public class OrdenCompraDetalle : Disposable
    {
        #region Enumeracion

        /// <summary>
        /// Enumeracion de estatus de una orden de compra
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Esatus de la orden como primera instancia registrada
            /// </summary>
            Registrada = 1,
            /// <summary>
            /// Estatus de una orden de compra solicitada
            /// </summary>
            Solicitada = 2,
            /// <summary>
            /// Estatus de una orden de compra abastecida Parcialmente
            /// </summary>
            AbastecidaParcial = 3,
            /// <summary>
            /// Estatus de una orden de compra cerrada
            /// </summary>
            Cerrada = 4,
            /// <summary>
            /// Estatus de una orden de compra cancelada
            /// </summary>
            Cancelada = 5            
        };

        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla orden compra detalle
        /// </summary>
        private static string nom_sp = "almacen.sp_orden_compra_detalle_tocd";
        private int _id_orden_compra_detalle;
        /// <summary>
        /// Id que permite identificar el desglose de datos de una orden de compra
        /// </summary>
        public int id_orden_compra_detalle
        {
            get { return _id_orden_compra_detalle; }
        }
        private int _id_orden_compra;
        /// <summary>
        /// Id que permite realizar la relación del encabezado de una orden e compra con su desglose.
        /// </summary>
        public int id_orden_compra
        {
            get { return _id_orden_compra; }
        }
        private int _id_producto;
        /// <summary>
        /// Id que permite identificar a un producto
        /// </summary>
        public int id_producto
        {
            get { return _id_producto; }
        }
        private decimal _cantidad;
        /// <summary>
        /// Permite describir la cantidad de piezas de un producto 
        /// </summary>
        public decimal cantidad
        {
            get { return _cantidad; }
        }
        private decimal _precio_unitario;
        /// <summary>
        /// Permite describir el precio unitario de un producto 
        /// </summary>
        public decimal precio_unitario
        {
            get { return _precio_unitario; }
        }
        /// <summary>
        /// Atributo que permite acceder a los elementos de la enumeracion Estatus
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this.id_estatus; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Id que permite identificar el estado de una orden de compra(registrada, solicitada, cancelada, cerrada y Abastecida Prancial)
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        private bool _habilitar;
        /// <summary>
        /// Permite modificar el estado de habilitación de un registro 
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Cosntructor default de la clase orden de compra detalle
        /// </summary>
        public OrdenCompraDetalle()
        {
            this._id_orden_compra_detalle = 0;
            this._id_orden_compra = 0;
            this._id_producto = 0;
            this._cantidad = 0.0M;
            this._precio_unitario = 0.0M;
            this._id_estatus = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que permite inicializar los valores de los atributos a partir de un registro de orden de compra detalle
        /// </summary>
        /// <param name="id_orden_compra_detalle">Id que sirve como referencia para la asignación de valores a los atributos.</param>
        public OrdenCompraDetalle(int id_orden_compra_detalle)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_orden_compra_detalle);
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destrucotr de la clase
        /// </summary>
        ~OrdenCompraDetalle()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro a partir de un id_orden_compra_detalle
        /// </summary>
        /// <param name="id_orden_compra_detalle">Id que permite la referencia de registros para su asignación a los atributos</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_orden_compra_detalle)
        {
            //Creacion del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para el sp 
            object[] param = { 3, id_orden_compra_detalle, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Invoca y asigna los valores del arreglo y el atributo con el nombre del sp al metodo encargado de realizar las transacciones a la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos obtenidos de la transaccion, que existan y qe no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas del Dataset y asigna los valores a los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_orden_compra_detalle = id_orden_compra_detalle;
                        this._id_orden_compra = Convert.ToInt32(r["IdOrdenCompra"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);                        
                        this._cantidad = Convert.ToDecimal(r["Cantidad"]);
                        this._precio_unitario = Convert.ToDecimal(r["PrecioUnitario"]);
                        this._id_producto = Convert.ToInt32(r["IdProducto"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la sentencia
                    retorno = true;
                }
            }
            //Retorno al método el objeto retorno
            return retorno;
        }

        /// <summary>
        /// Método que permite la actualización de datos de un registro de Orden de compra detalle
        /// </summary>
        /// <param name="id_orden_compra">Id que permite actualizar el identificador de una orden de compra</param>        
        /// <param name="estatus">Permite actualizar el estado de una orden de compra (registrada, solicitada, cancelada, cerrada y Abastecida Prancial)</param>
        /// <param name="cantidad">Permite actualizar la cantidad de piezas de un prodcutos </param>
        /// <param name="id_producto">Id que pemite actualizar el idntificador del producto de una orden de compra</param>        
        /// <param name="id_usuario">Id que permite actualizar el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Permite actualizar el estado de habilitación de un registro </param>
        /// <returns></returns>
        private RetornoOperacion editarOrdenCompraDetalle(int id_orden_compra, Estatus estatus, decimal cantidad, decimal precio_unitario, int id_producto, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo de tipo objeto que almacena los valores de los parametros del sp de la tabla Orden Compra.
            object[] param = { 2, this._id_orden_compra_detalle, id_orden_compra, (byte)estatus, cantidad, precio_unitario, id_producto, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Obtiene el Estatus de la Requisición según sus Detalles
        /// </summary>
        /// <returns></returns>
        private int obtieneEstatusRequisicion()
        {
            //Declarando Objeto de Retorno
            int idEstatus = 0;

            //Inicialziando los parametros para ejecución del Stored Procedure
            object[] param = { 5, 0, this._id_orden_compra, 0, 0, 0, 0, 0, false, "", "" };

            //Realizando la solicitud hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Para cada registro devuelto
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Asignando Estatus Obtenido
                        idEstatus = Convert.ToInt32(dr["IdEstatus"]);
                }
            }

            //Devolviendo Resultado Obtenido
            return idEstatus;
        }

        #endregion

        #region Método Públicos

        /// <summary>
        /// Método que permite la insercion de un registro en la tabla orden de compra detalle
        /// </summary>
        /// <param name="id_orden_compra">Id que permite la insercion del identificador de una orden de compra</param>
        /// <param name="cantidad">Permite insertar la cantidad de piezas de un producto</param>
        /// <param name="id_producto">Id que permite la inserción del identificador de un producto</param>       
        /// <param name="id_usuario">Permite insertar el identificador del usuario que realizo la ultima acción sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarOrdenCompraDetalle(int id_orden_compra, decimal cantidad, decimal precio_unitario, int id_producto, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idDetalleOC = 0;
            
            //Creación del arreglo de tipo objeto que alamcena el valor de los parametros necesarios para el sp de la tabla OrdenCompra.
            object[] param = { 1, 0, id_orden_compra, (byte)Estatus.Registrada, cantidad, precio_unitario, id_producto, id_usuario, true, "", "" };

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Asignacion al objeto retorno del arreglo y el atributo con el nombre del sp, para la transacción a la base de datos.
                retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

                //Obteniendo Id de Detalle
                idDetalleOC = retorno.IdRegistro;

                //Validando Operación Exitosa
                if (retorno.OperacionExitosa)
                {
                    //Obteniendo Totales
                    using (DataTable dtTotales = ObtieneTotalesOrdenCompra(id_orden_compra))
                    {
                        //Validando que existan Totales
                        if (Validacion.ValidaOrigenDatos(dtTotales))
                        {
                            //Recorriendo Totales
                            foreach (DataRow dr in dtTotales.Rows)
                            {
                                //Instanciando Orden de Compra
                                using (OrdenCompra oc = new OrdenCompra(id_orden_compra))
                                {
                                    //Validando que Exista la Orden
                                    if (oc.habilitar)

                                        //Actualizando Totales de la Orden de Compra
                                        retorno = oc.ActualizaTotalesOrdenCompra(Convert.ToDecimal(dr["SubTotal"]), Convert.ToDecimal(dr["Trasladado"]),
                                                                                Convert.ToDecimal(dr["Retenido"]), Convert.ToDecimal(dr["Total"]),
                                                                                id_usuario);
                                    else
                                        //Instanciando Excepción
                                        retorno = new RetornoOperacion("No se puede Acceder a la Orden de Compra");
                                }

                                //Terminando Ciclo
                                break;
                            }
                        }
                    }
                }

                //Validando Operación Exitosa
                if (retorno.OperacionExitosa)
                {
                    //Instanciando Detalle
                    retorno = new RetornoOperacion(idDetalleOC);
                    
                    //Completando Transacción
                    trans.Complete();
                }
            }
            
            //Retrono del objeto retrono al método
            return retorno;
        }
        /// <summary>
        /// Método que permite la actualización de datos de un registro de Orden de compra detalle
        /// </summary>
        /// <param name="id_orden_compra">Id que permite actualizar el identificador de una orden de compra</param>
        /// <param name="estatus">Permite actualizar el estado de una orden de compra (registrada, solicitada, cancelada, cerrada y Abastecida Prancial)</param>
        /// <param name="cantidad">Permite actualizar la cantidad de piezas de un prodcutos </param>
        /// <param name="id_producto">Id que pemite actualizar el idntificador del producto de una orden de compra</param>        
        /// <param name="id_usuario">Id que permite actualizar el identificador del usuario que realizo acciones sobre el registro</param>
        public RetornoOperacion EditarOrdenCompraDetalle(int id_orden_compra, Estatus estatus, decimal cantidad, decimal precio_unitario, int id_producto, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que este Registrado
            if (this.estatus == Estatus.Registrada || this.estatus == Estatus.Solicitada)
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Invoca y retorna el método editarOrdenCompraDetalle
                    result = this.editarOrdenCompraDetalle(id_orden_compra, estatus, cantidad, precio_unitario, id_producto, id_usuario, this._habilitar);

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Totales
                        using (DataTable dtTotales = ObtieneTotalesOrdenCompra(id_orden_compra))
                        {
                            //Validando que existan Totales
                            if (Validacion.ValidaOrigenDatos(dtTotales))
                            {
                                //Recorriendo Totales
                                foreach (DataRow dr in dtTotales.Rows)
                                {
                                    //Instanciando Orden de Compra
                                    using (OrdenCompra oc = new OrdenCompra(id_orden_compra))
                                    {
                                        //Validando que Exista la Orden
                                        if (oc.habilitar)

                                            //Actualizando Totales de la Orden de Compra
                                            result = oc.ActualizaTotalesOrdenCompra(Convert.ToDecimal(dr["SubTotal"]), Convert.ToDecimal(dr["Trasladado"]),
                                                                                    Convert.ToDecimal(dr["Retenido"]), Convert.ToDecimal(dr["Total"]),
                                                                                    id_usuario);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No se puede Acceder a la Orden de Compra");
                                    }

                                    //Terminando Ciclo
                                    break;
                                }
                            }
                        }
                    }

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Detalle
                        result = new RetornoOperacion(this._id_orden_compra_detalle);

                        //Completando Transacción
                        trans.Complete();
                    }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Detalle debe de estar en 'Registrado' o 'Solicitado' para su Edición");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que permite cambiar el estado de habilitación de un registro
        /// </summary>
        /// <param name="id_usuario">Permite insertar el identificador del usuario que realizo la ultima acción sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarOrdenCompraDetalle(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando que este Registrado
            if (this.estatus == Estatus.Registrada || this.estatus == Estatus.Solicitada)
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Invoca y retorna el método editaOrdenCompraDetalle
                    result = this.editarOrdenCompraDetalle(this.id_orden_compra, this.estatus, this.cantidad, this._precio_unitario, this.id_producto, id_usuario, false);

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Totales
                        using (DataTable dtTotales = ObtieneTotalesOrdenCompra(this.id_orden_compra))
                        {
                            //Validando que existan Totales
                            if (Validacion.ValidaOrigenDatos(dtTotales))
                            {
                                //Recorriendo Totales
                                foreach (DataRow dr in dtTotales.Rows)
                                {
                                    //Instanciando Orden de Compra
                                    using (OrdenCompra oc = new OrdenCompra(this.id_orden_compra))
                                    {
                                        //Validando que Exista la Orden
                                        if (oc.habilitar)

                                            //Actualizando Totales de la Orden de Compra
                                            result = oc.ActualizaTotalesOrdenCompra(Convert.ToDecimal(dr["SubTotal"]), Convert.ToDecimal(dr["Trasladado"]),
                                                                                    Convert.ToDecimal(dr["Retenido"]), Convert.ToDecimal(dr["Total"]),
                                                                                    id_usuario);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No se puede Acceder a la Orden de Compra");
                                    }

                                    //Terminando Ciclo
                                    break;
                                }
                            }
                        }
                    }

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Detalle
                        result = new RetornoOperacion(this._id_orden_compra_detalle);

                        //Completando Transacción
                        trans.Complete();
                    }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Detalle debe de estar en 'Registrado' o 'Solicitado' para su Eliminación");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método qeu realiza la consulta de registro dado un id_compra y el resultado los almacena en una tabla
        /// </summary>
        /// <param name="id_orden_compra">Id que sirve como referencia para la busqueda de registros</param>
        /// <returns></returns>
        public static DataTable CargaDetallesOrdenCompra(int id_orden_compra)
        {
            //Creación de la tabla OrdenCompradetalle
            DataTable dtOrdenCompraDetalle = null;
            //Creación del arreglo necesario para la obtención de datos del sp de la tabla orden compra detalle
            object[] param = { 4, 0, id_orden_compra, 0, 0, 0, 0, 0, false, "", "" };
            //Realiza la transaccion con la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla los valores encontrados
                    dtOrdenCompraDetalle = DS.Tables["Table"];
            }

            //Devuelve el resultado al método
            return dtOrdenCompraDetalle;
        }
        /// <summary>
        /// Método que permite la actualización el estatus de la orden de compra detalle
        /// </summary>
        /// <param name="estatus">Permite modificar el estatus de la orden de compra detalle</param>
        /// <param name="id_usuario">Permite modificar el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusDetalle(Estatus estatus, int id_usuario)
        {
            //Invoca y retorna el método editarOrdenCompraDetalle
            return this.editarOrdenCompraDetalle(this._id_orden_compra, estatus, this._cantidad, this._precio_unitario, this._id_producto, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus de la Orden de Compra y su Encabezado
        /// </summary>
        /// <param name="estatus">Estatus del Detalle</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusOrdenCompraDetalle(Estatus estatus, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invoca y retorna el método editarOrdenCompraDetalle
                result = this.editarOrdenCompraDetalle(this._id_orden_compra, estatus, this._cantidad, this._precio_unitario, this.id_producto, id_usuario, this._habilitar);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Declarando Estatus
                    OrdenCompra.Estatus estatusOC = (OrdenCompra.Estatus)this.obtieneEstatusRequisicion();

                    //Instanciando Orden de Compra
                    using (OrdenCompra oc = new OrdenCompra(this._id_orden_compra))
                    {
                        //Validando que exista la Orden
                        if (oc.habilitar)
                        
                            //Actualizando Estatus
                            result = oc.ActualizaEstatusOrdenCompra(estatusOC, id_usuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede acceder a la Orden de Compra");

                        //Validando Operaciones Exitosas
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Detalle de OC
                            result = new RetornoOperacion(this._id_orden_compra_detalle);

                            //Completando Transacción
                            trans.Complete();
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener los Totales de la Orden de Compra
        /// </summary>
        /// <param name="id_orden_compra">Orden de Compra</param>
        /// <returns></returns>
        public static DataTable ObtieneTotalesOrdenCompra(int id_orden_compra)
        {
            //Declarando Objeto de Retorno
            DataTable dtTotales = null;

            //Creación del arreglo necesario para la obtención de datos del sp de la tabla orden compra detalle
            object[] param = { 6, 0, id_orden_compra, 0, 0, 0, 0, 0, false, "", "" };

            //Realiza la transaccion con la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    
                    //Asigna a la tabla los valores encontrados
                    dtTotales = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtTotales;
        }
        /// <summary>
        /// Método encargado de Obtener los Totales de la Orden de Compra
        /// </summary>
        /// <param name="id_orden_compra_detalle">Detalle de Orden de Compra</param>
        /// <returns></returns>
        public static DataTable ObtieneDetalleInventario(int id_orden_compra_detalle)
        {
            //Declarando Objeto de Retorno
            DataTable dtInventario = null;

            //Creación del arreglo necesario para la obtención de datos del sp de la tabla orden compra detalle
            object[] param = { 7, id_orden_compra_detalle, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Realiza la transaccion con la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asigna a la tabla los valores encontrados
                    dtInventario = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtInventario;
        }
        /// <summary>
        /// Método encargado de Obtener los detalles de una orden de compra
        /// </summary>
        /// <param name="id_orden_compra_detalle">Orden de Compra</param>
        /// <returns></returns>
        public static DataTable ObtieneDetalleEtiquetasEmbarque(int id_orden_compra)
        {
            //Declarando Objeto de Retorno
            DataTable dtEtiqueta = null;

            //Creación del arreglo necesario para la obtención de datos del sp de la tabla orden compra detalle
            object[] param = { 8, 0, id_orden_compra, 0, 0, 0, 0, 0, false, "", "" };

            //Realiza la transaccion con la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asigna a la tabla los valores encontrados
                    dtEtiqueta = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtEtiqueta;
        }
        /// <summary>
        /// Método qeu realiza la consulta de registro dado un id_compra y el resultado los almacena en una tabla
        /// </summary>
        /// <param name="id_orden_compra">Id que sirve como referencia para la busqueda de registros</param>
        /// <returns></returns>
        public static DataTable OrdenCompraImpresionDetalles(int id_orden_compra)
        {
            //Creación de la tabla OrdenCompradetalle
            DataTable dtOrdenCompraDetalle = null;
            //Creación del arreglo necesario para la obtención de datos del sp de la tabla orden compra detalle
            object[] param = { 9, 0, id_orden_compra, 0, 0, 0, 0, 0, false, "", "" };
            //Realiza la transaccion con la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla los valores encontrados
                    dtOrdenCompraDetalle = DS.Tables["Table"];
            }

            //Devuelve el resultado al método
            return dtOrdenCompraDetalle;
        }

        #endregion
    }
}
