using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// Clase encargada de Todas las Operaciones de los Detalles de Requisición
    /// </summary>
    public class RequisicionDetalle : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los posibles estatus del detalle
        /// </summary>
        public enum EstatusDetalle
        {
            /// <summary>
            /// Sólo se ha capturado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Está pendiente de autorización para su abastecimiento
            /// </summary>
            PorAutorizar = 2,
            /// <summary>
            /// Se encuentra pendiente de abastecimiento
            /// </summary>
            Solicitado = 3,
            /// <summary>
            /// Se ha abastecido de forma parcial, puede continuar siendo abastecido
            /// </summary>
            AbastecidoParcial = 4,
            /// <summary>
            /// Se ha terminado de abastecer (parcial o totalmente)
            /// </summary>
            Cerrado = 5,
            /// <summary>
            /// No llegará a ser abastecido
            /// </summary>
            Cancelado = 6
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure principal, utilizado en la clase.
        /// </summary>
        private static string _nombreStoredProcedure = "almacen.sp_requisicion_detalle_trd";
        private int _idDetalleRequisicion;
        /// <summary>
        /// Obtiene el Id de registro del detalle de la instancia.
        /// </summary>
        public int IdDetalleRequisicion { get { return this._idDetalleRequisicion; } }
        private int _idRequisicion;
        /// <summary>
        /// Obtiene el Id de Requisisción a la que pertenece el detalle.
        /// </summary>
        public int IdRequisicion { get { return this._idRequisicion; } }
        private int _idEstatus;
        /// <summary>
        /// Obtiene el Id de Estatus actual de la instancia.
        /// </summary>
        public int IdEstatus { get { return this._idEstatus; } }
        private decimal _cantidad;
        /// <summary>
        /// Obtiene la cantidad de material o producto del detalle de la instancia.
        /// </summary>
        public decimal Cantidad { get { return this._cantidad; } }
        private int _idUnidadMedida;
        /// <summary>
        /// Obtiene el Id de Unidad de Medida del material o producto del detalle de la instancia.
        /// </summary>
        public int IdUnidadMedida { get { return this._idUnidadMedida; } }
        private int _idProducto;
        /// <summary>
        /// Obtiene el Id de Producto del detalle de requisición representado por la instancia.
        /// </summary>
        public int IdProducto { get { return this._idProducto; } }
        private string _codigoProducto;
        /// <summary>
        /// Obtiene el código que describe al material o producto del detalle de la instancia.
        /// </summary>
        public string CodigoProducto { get { return this._codigoProducto; } }
        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del detalle representado por la instancia.
        /// </summary>
        public bool Habilitar { get { return this._habilitar; } }

        #endregion

        #region Contructores

        /// <summary>
        /// Genera una nuevainstancia del tipo "clRequisicionDetalle" del módulo "almacen" por Defecto
        /// </summary>
        public RequisicionDetalle()
        {
            //Asignando los atributos
            this._idRequisicion = 0;
            this._idEstatus = 0;
            this._cantidad = 0;
            this._idUnidadMedida = 0;
            this._idProducto = 0;
            this._codigoProducto = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Genera una nuevainstancia del tipo "clRequisicionDetalle" del módulo "almacen" dado u registro
        /// </summary>
        /// <param name="idDetalleRequisicion">Id de Detalle de Requisición</param>
        public RequisicionDetalle(int idDetalleRequisicion)
        {
            //Asignando el resto de los atributos de la instancia
            cargaAtributosInstancia(idDetalleRequisicion);
        }

        #endregion

        #region Destructor

        ~RequisicionDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Inicializa los atributos de la instancia
        /// </summary>
        /// <param name="id_detalle_requisicion">Id de </param>
        private bool cargaAtributosInstancia(int id_detalle_requisicion)
        {
            //Declarando Objeto de Retorno
            bool result = false;
            
            //Inicialziando arreglo de parámetros para ejecución del Stored Procedure
            object[] parametros = { 3, id_detalle_requisicion, 0, 0, 0, 0, 0, "", 0, false, "", "" };

            //Realizando la solicitud hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreStoredProcedure, parametros))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Para cada registro devuelto
                    foreach (DataRow f in ds.Tables["Table"].Rows)
                    {
                        //Asignando los atributos
                        this._idDetalleRequisicion = id_detalle_requisicion;
                        this._idRequisicion = f.Field<int>("IdRequisicion");
                        this._idEstatus = f.Field<int>("IdEstatus");
                        this._cantidad = f.Field<decimal>("Cantidad");
                        this._idUnidadMedida = f.Field<int>("IdUnidadMedida");
                        this._idProducto = f.Field<int>("IdProducto");
                        this._codigoProducto = f.Field<string>("CodigoProducto");
                        this._habilitar = f.Field<bool>("Habilitar");
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Realiza la edición del registro detalle de requisición representado por la instancia
        /// </summary>
        /// <param name="idEstatus">Id de Estatus</param>
        /// <param name="cantidad">Cantidad de material o producto</param>
        /// <param name="idUnidadMedida">Id de Unidad de medida del material o producto</param>
        /// <param name="idProducto">Id de Producto</param>
        /// <param name="codigoProducto">Código de identificación del material o producto</param>
        /// <param name="idUsuario">Id de Usuario que edita el registro</param>
        /// <param name="habilitar">Valor de habilitación del registro</param>
        /// <returns></returns>
        private RetornoOperacion editaDetalleRequisicion(int idEstatus, decimal cantidad, int idUnidadMedida, int idProducto, string codigoProducto, int idUsuario, bool habilitar)
        {
            //Declarando la variable de retorno
            RetornoOperacion registro = new RetornoOperacion();

            //Inicialziando los parametros para ejecución del Stored Procedure
            object[] parametros = { 2, this._idDetalleRequisicion, this._idRequisicion, idEstatus, cantidad, idUnidadMedida, idProducto, codigoProducto, idUsuario, habilitar, "", "" };

            //Realizando la edición del regsitro
            registro = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombreStoredProcedure, parametros);

            //Devolviendo el Id del registro editado
            return registro;
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
            object[] param = { 6, 0, this._idRequisicion, 0, 0, 0, 0, "", 0, false, "", "" };

            //Realizando la solicitud hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreStoredProcedure, param))
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

        #region Métodos Públicos

        /// <summary>
        /// Inserta un nuevo registro detalle de requisición en BD
        /// </summary>
        /// <param name="idRequisicion">Id de requisisción</param>
        /// <param name="cantidad">Cantidad de Material o Producto</param>
        /// <param name="idUnidadMedida">Id de Unidad de Medida</param>
        /// <param name="idProducto">Id de Producto</param>
        /// <param name="codigoProducto">Código de identificación del producto</param>
        /// <param name="idUsuario">Id de Usuario que registra</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDetalleRequisicion(int idRequisicion, decimal cantidad, int idUnidadMedida, int idProducto, string codigoProducto, int idUsuario)
        {
            //Declarando variable de retorno
            RetornoOperacion registro;

            //Inicializando parámetros de inserción
            object[] parametros = { 1, 0, idRequisicion, 1, cantidad, idUnidadMedida, idProducto, codigoProducto, idUsuario, true, "", "" };

            //Realziando la inserción del nuevo registro
            registro = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombreStoredProcedure, parametros);

            //Devolvinedo variable con el Id de registro insertado
            return registro;
        }
        /// <summary>
        /// Edita los datos principales del registro detalle de requisición representado por la instancia
        /// </summary>
        /// <param name="estatus">Estatus del detalle</param>
        /// <param name="cantidad">Cantidad de Material o Producto</param>
        /// <param name="idUnidadMedida">Id de Unidad de Medida</param>
        /// <param name="idProducto">Id de Producto</param>
        /// <param name="codigoProducto">Código de identificación del producto</param>
        /// <param name="idUsuario">Id de Usuario que registra</param>
        /// <returns></returns>
        public RetornoOperacion EditaDetalleRequisicion(EstatusDetalle estatus, decimal cantidad, int idUnidadMedida, int idProducto, string codigoProducto, int idUsuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que el Estatus se encuentre Registrado
            if ((EstatusDetalle)this._idEstatus == EstatusDetalle.Registrado)

                //Realizando la edición del registro
                result = editaDetalleRequisicion((int)estatus, cantidad, idUnidadMedida, idProducto, codigoProducto, idUsuario, this._habilitar);
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Estatus debe de estar en 'Registrado' para poder Editar");

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Edita el estatus del registro detalle requisición representado por la instancia
        /// </summary>
        ///<param name="estatus">Estatus del Detalle de Requisicion</param>
        /// <param name="idUsuario">Id de Usuario que registra</param>
        /// <returns></returns>
        public RetornoOperacion EditaEstatus(EstatusDetalle estatus, int idUsuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizando la edición del registro
                result = editaDetalleRequisicion((int)estatus, this._cantidad, this._idUnidadMedida, this._idProducto, this._codigoProducto, idUsuario, true);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Instanciando Requisición
                    using (Requisicion requisicion = new Requisicion(this._idRequisicion))
                    {
                        //Validando que Exista la Requisición
                        if (requisicion.habilitar)

                            //Actualizando Estatus de la Requisición
                            result = requisicion.ActualizaEstatusRequisicion((Requisicion.Estatus)this.obtieneEstatusRequisicion(), idUsuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No existe la Requisición");

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Detalle
                            result = new RetornoOperacion(this._idDetalleRequisicion);

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
        /// Deshabilita el registro detalle de requisición representado por la instancia
        /// </summary>
        /// <param name="idUsuario">Id de Usuario que registra</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDetalleRequisicion(int idUsuario)
        {
            //Realizando la edición del registro
            return editaDetalleRequisicion(this._idEstatus, this._cantidad, this._idUnidadMedida, this._idProducto, this._codigoProducto, idUsuario, false);
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles de la Requisición
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesRequisicion(int id_requisicion)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;

            //Inicializando parámetros de inserción
            object[] parametros = { 4, 0, id_requisicion, 1, 0, 0, 0, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreStoredProcedure, parametros))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtDetalles = ds.Tables["Table"];
            }

            //Devovliendo Objeto de Retorno
            return dtDetalles;
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles de la Requisición para Surtirlas
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesRequisicionSurtido(int id_requisicion)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;

            //Inicializando parámetros de inserción
            object[] parametros = { 5, 0, id_requisicion, 0, 0, 0, 0, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreStoredProcedure, parametros))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtDetalles = ds.Tables["Table"];
            }

            //Devovliendo Objeto de Retorno
            return dtDetalles;
        }
        /// <summary>
        /// Realiza la copia de un registro detalle de requisición solicitado
        /// </summary>
        /// <param name="idDetalleRequisicion">Id de Detalle de Requisición a copiar</param>
        /// <param name="idRequisicion">Id de Requisición a la que se asociará</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion CopiaDetalleRequisicion(int idDetalleRequisicion, int idRequisicion, int idUsuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Realziando la instanciación del registro a copiar
            using (RequisicionDetalle d = new RequisicionDetalle(idDetalleRequisicion))
            {
                //Si el detalle existe
                if (d.IdDetalleRequisicion > 0)
                    //Insertando el nuevo registro
                    resultado = InsertaDetalleRequisicion(idRequisicion, d.Cantidad, d.IdUnidadMedida, d.IdProducto, d.CodigoProducto, idUsuario);
            }

            //Retornando resultado obtenido
            return resultado;
        }

        /// <summary>
        /// Obtiene las refacciones consumidas ligadas a una Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo"></param>
        /// <returns></returns>
        public static DataTable ObtieneRefaccionesConsumidas(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;

            //Inicializando parámetros de inserción
            object[] parametros = { 7, 0, 0, 0, 0, 0, 0, "", 0, false, id_orden_trabajo, "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreStoredProcedure, parametros))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtDetalles = ds.Tables["Table"];
            }

            //Devovliendo Objeto de Retorno
            return dtDetalles;
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles de la Requisición para formato de impresión de axejit
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesRequisicionAxejit(int id_requisicion)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;

            //Inicializando parámetros de inserción
            object[] parametros = { 8, 0, id_requisicion, 1, 0, 0, 0, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombreStoredProcedure, parametros))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtDetalles = ds.Tables["Table"];
            }

            //Devovliendo Objeto de Retorno
            return dtDetalles;
        }
        #endregion
    }
}
