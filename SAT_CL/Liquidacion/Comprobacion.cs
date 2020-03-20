using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using SAT_CL.EgresoServicio;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con las Comprobaciones
    /// </summary>
    public class Comprobacion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de alamcenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_comprobacion_tc";

        private int _id_comprobacion;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente a la Comprobación
        /// </summary>
        public int id_comprobacion { get { return this._id_comprobacion; } }
        private int _id_deposito;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente al Deposito
        /// </summary>
        public int id_deposito { get { return this._id_deposito; } }
        private int _id_concepto_comprobacion;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente al Concepto de la Comprobación
        /// </summary>
        public int id_concepto_comprobacion { get { return this._id_concepto_comprobacion; } }
        private int _id_autorizacion_comprobacion;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente a la Autorización de la Comprobación
        /// </summary>
        public int id_autorizacion_comprobacion { get { return this._id_autorizacion_comprobacion; } }
        private string _observacion_comprobacion;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente a las Observaciones
        /// </summary>
        public string observacion_comprobacion { get { return this._observacion_comprobacion; } }
        private bool _bit_transferencia;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente al Estatus de Transferencia
        /// </summary>
        public bool bit_transferencia { get { return this._bit_transferencia; } }
        private int _id_transferencia;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente a la Transferencia
        /// </summary>
        public int id_transferencia { get { return this._id_transferencia; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente al Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private DetalleLiquidacion _objDetalleComprobacion;
        /// <summary>
        /// Atributo encargado de guardar el Detalle de la Liquidación Ligado a la Comprobación
        /// </summary>
        public DetalleLiquidacion objDetalleComprobacion { get { return this._objDetalleComprobacion; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos de la Clase por Defecto
        /// </summary>
        public Comprobacion()
        {
            //Asignando Valores
            this._id_comprobacion = 0;
            this._id_deposito = 0;
            this._id_concepto_comprobacion = 0;
            this._id_autorizacion_comprobacion = 0;
            this._observacion_comprobacion = "";
            this._bit_transferencia = false;
            this._id_transferencia = 0;
            this._habilitar = false;
            this._objDetalleComprobacion = new DetalleLiquidacion();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos de la Clase dado un Registro
        /// </summary>
        /// <param name="id_comprobacion">Id de Comprobación</param>
        public Comprobacion(int id_comprobacion)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_comprobacion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Comprobacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_comprobacion">Id de Comprobación</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_comprobacion)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_comprobacion, 0, 0, 0, "", false, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_comprobacion = Convert.ToInt32(dr["Id"]);
                        this._id_deposito = Convert.ToInt32(dr["IdDeposito"]);
                        this._id_concepto_comprobacion = Convert.ToInt32(dr["IdConceptoComprobacion"]);
                        this._id_autorizacion_comprobacion = Convert.ToInt32(dr["IdAutorizacion"]);
                        this._observacion_comprobacion = dr["ObservacionComprobacion"].ToString();
                        this._bit_transferencia = Convert.ToBoolean(dr["BitTransferencia"]);
                        this._id_transferencia = Convert.ToInt32(dr["IdTransferencia"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._objDetalleComprobacion = new DetalleLiquidacion(id_comprobacion, 104);
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros
        /// </summary>
        /// <param name="id_deposito">Deposito de la Comprobación</param>
        /// <param name="id_concepto_comprobacion">Concepto de la Comprobación</param>
        /// <param name="id_autorizacion_comprobacion">Autorización de la Comprobación</param>
        /// <param name="observacion_comprobacion">Observación de la Comprobación</param>
        /// <param name="bit_transferencia">Estatus de Transferencia</param>
        /// <param name="id_transferencia">Referencia de la Transferencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_deposito, int id_concepto_comprobacion, int id_autorizacion_comprobacion, string observacion_comprobacion, 
                                                   bool bit_transferencia, int id_transferencia, DetalleLiquidacion.Estatus estatus, int id_unidad, int id_operador, int id_proveedor, int id_servicio, int id_movimiento,
                                                   DateTime fecha_liquidacion, int id_liquidacion, decimal cantidad, decimal valor_unitario, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transaccion
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Armando Arreglo de Parametros
                object[] param = { 2, this._id_comprobacion, id_deposito, id_concepto_comprobacion, id_autorizacion_comprobacion, observacion_comprobacion, bit_transferencia, 
                               id_transferencia, id_usuario, habilitar, "", "" };

                //Ejecutando resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                {
                    //Obteniendo Comprobación
                    int idComprobacion = result.IdRegistro;

                    //Instanciando Detalle de la Liquidación
                    using(DetalleLiquidacion dl = new DetalleLiquidacion(idComprobacion, 104))
                    {
                        //Validando que exista el Registro
                        if (dl.id_detalle_liquidacion > 0)
                        {
                            //Validando que sea una Edicion
                            if (habilitar)

                                //Insertando Detalle de Liquidación
                                result = dl.EditaDetalleLiquidacion(104, idComprobacion, (byte)estatus, id_unidad, id_operador, id_proveedor, id_servicio,
                                                id_movimiento, fecha_liquidacion, id_liquidacion, cantidad, 0, valor_unitario, id_usuario);
                            
                            //Validando que sea una Deshabilitación
                            else
                                //Deshabilitando Detalle de Liquidación
                                result = dl.DeshabilitaDetalleLiquidacion(id_usuario);
                        }

                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede Acceder el Detalle, Imposible su Edición");
                    }

                    //Validando que la Operación haya sido exitosa
                    if (result.OperacionExitosa)
                    {
                        //Completando Transacción
                        trans.Complete();

                        //Instanciando la Comprobación
                        result = new RetornoOperacion(idComprobacion);
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Comprobaciones
        /// </summary>
        /// <param name="id_deposito">Deposito de la Comprobación</param>
        /// <param name="id_concepto_comprobacion">Concepto de la Comprobación</param>
        /// <param name="id_autorizacion_comprobacion">Autorización de la Comprobación</param>
        /// <param name="observacion_comprobacion">Observación de la Comprobación</param>
        /// <param name="bit_transferencia">Estatus de Transferencia</param>
        /// <param name="id_transferencia">Referencia de la Transferencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaComprobacion(int id_deposito, int id_concepto_comprobacion, int id_autorizacion_comprobacion, string observacion_comprobacion,
                                                   bool bit_transferencia, int id_transferencia, SAT_CL.EgresoServicio.DetalleLiquidacion.Estatus estatus, int id_unidad, int id_operador, int id_proveedor, int id_servicio, int id_movimiento,
                                                   DateTime fecha_liquidacion, int id_liquidacion, decimal cantidad, decimal valor_unitario, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transaccion
            using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Deposito
                using (Deposito dep = new Deposito(id_deposito))
                {
                    //Validando que Exista el Deposito
                    if (dep.habilitar)
                    {
                        //Validando Estatus
                        if(dep.Estatus == Deposito.EstatusDeposito.PorLiquidar)

                            //Instanciando Positivo
                            result = new RetornoOperacion(0, "", true);
                        else
                            //Instanciando Negativo
                            result = new RetornoOperacion(string.Format("El Estatus '{0}' del Deposito no permite su Comprobación", dep.Estatus));
                    }
                    else
                        //Instanciando Positivo
                        result = new RetornoOperacion(0, "", true);

                    //Validando Resultado del Deposito
                    if (result.OperacionExitosa)
                    {
                        //Armando Arreglo de Parametros
                        object[] param = { 1, 0, id_deposito, id_concepto_comprobacion, id_autorizacion_comprobacion, observacion_comprobacion, bit_transferencia, 
                               id_transferencia, id_usuario, true, "", "" };

                        //Obteniendo Comprobación
                        int idComprobacion = 0;

                        //Ejecutando resultado del SP
                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                        //Validando que la Operación haya sido exitosa
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Comprobación
                            idComprobacion = result.IdRegistro;

                            //Insertando Detalle de Liquidación
                            result = SAT_CL.EgresoServicio.DetalleLiquidacion.InsertaDetalleLiquidacion(104, idComprobacion, id_unidad, id_operador, id_proveedor,
                                                id_servicio, id_movimiento, id_liquidacion, cantidad, 0, valor_unitario, id_usuario);

                            //Validando que la Operación haya sido exitosa
                            if (result.OperacionExitosa)
                            {
                                //Instanciando Deposito (Detalle Liquidación)
                                using (SAT_CL.EgresoServicio.DetalleLiquidacion det = new SAT_CL.EgresoServicio.DetalleLiquidacion(id_deposito, 51))
                                {
                                    //Validando que Exista el Deposito
                                    if (det.id_detalle_liquidacion > 0)
                                    {
                                        //Agregando Referencia de la Liquidación al Detalle del Deposito
                                        result = det.EditaDetalleLiquidacion(det.id_tabla, det.id_registro, det.id_estatus_liquidacion, det.id_unidad,
                                                    det.id_operador, det.id_proveedor_compania, det.id_servicio, det.id_movimiento, det.fecha_liquidacion,
                                                    id_liquidacion, det.cantidad, det.id_unidad_medida, det.valor_unitario, id_usuario);


                                    }
                                    else
                                        //Si es una Comprobación sin Deposito
                                        result = new RetornoOperacion(idComprobacion);

                                    //Validando que se añadio el Deposito a la Liqudiación
                                    if (result.OperacionExitosa)
                                    {
                                        //Completando Transacción
                                        trans.Complete();

                                        //Instanciando Resultado de COmprobacion
                                        result = new RetornoOperacion(idComprobacion);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Comprobaciones
        /// </summary>
        /// <param name="id_deposito">Deposito de la Comprobación</param>
        /// <param name="id_concepto_comprobacion">Concepto de la Comprobación</param>
        /// <param name="id_autorizacion_comprobacion">Autorización de la Comprobación</param>
        /// <param name="observacion_comprobacion">Observación de la Comprobación</param>
        /// <param name="bit_transferencia">Estatus de Transferencia</param>
        /// <param name="id_transferencia">Referencia de la Transferencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaComprobacion(int id_deposito, int id_concepto_comprobacion, int id_autorizacion_comprobacion, string observacion_comprobacion,
                                                   bool bit_transferencia, int id_transferencia, SAT_CL.EgresoServicio.DetalleLiquidacion.Estatus estatus, int id_unidad, int id_operador, int id_proveedor, int id_servicio, int id_movimiento,
                                                   DateTime fecha_liquidacion, int id_liquidacion, decimal cantidad, decimal valor_unitario, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(id_deposito, id_concepto_comprobacion, id_autorizacion_comprobacion, observacion_comprobacion, 
                                          bit_transferencia, id_transferencia, estatus, id_unidad, id_operador, id_proveedor, id_servicio, id_movimiento,
                                          fecha_liquidacion, id_liquidacion, cantidad, valor_unitario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Comprobaciones
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaComprobacion(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(this._id_deposito, this._id_concepto_comprobacion, this._id_autorizacion_comprobacion, this._observacion_comprobacion,
                                          this._bit_transferencia, this._id_transferencia, (DetalleLiquidacion.Estatus)this._objDetalleComprobacion.id_estatus_liquidacion, this._objDetalleComprobacion.id_unidad, this._objDetalleComprobacion.id_operador,
                                          this._objDetalleComprobacion.id_proveedor_compania, this._objDetalleComprobacion.id_servicio, this._objDetalleComprobacion.id_movimiento,
                                          this._objDetalleComprobacion.fecha_liquidacion, this._objDetalleComprobacion.id_liquidacion, this._objDetalleComprobacion.cantidad, this._objDetalleComprobacion.valor_unitario, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de la Comprobación
        /// </summary>
        /// <returns></returns>
        public bool ActualizaComprobacion()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_comprobacion);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Total de la Comprobación
        /// </summary>
        /// <param name="valor_total">Valor Total de la Comprobación</param>
        /// <param name="id_usuario">Usuario que Actualiza la Comprobación</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTotalComprobacion(decimal valor_total, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(this._id_deposito, this._id_concepto_comprobacion, this._id_autorizacion_comprobacion, this._observacion_comprobacion,
                                          this._bit_transferencia, this._id_transferencia, (DetalleLiquidacion.Estatus)this._objDetalleComprobacion.id_estatus_liquidacion, this._objDetalleComprobacion.id_unidad, this._objDetalleComprobacion.id_operador,
                                          this._objDetalleComprobacion.id_proveedor_compania, this._objDetalleComprobacion.id_servicio, this._objDetalleComprobacion.id_movimiento,
                                          this._objDetalleComprobacion.fecha_liquidacion, this._objDetalleComprobacion.id_liquidacion, this._objDetalleComprobacion.cantidad, valor_total, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Plúbico encargado de Obtener las Comprobaciones Ligadas a un Movimiento
        /// </summary>
        /// <param name="id_movimiento">Referencia al Movimiento</param>
        /// <param name="id_liquidacion">Referencia a la Liquidación</param>
        /// <returns></returns>
        public static DataTable ObtieneComprobacionesMovimiento(int id_movimiento, int id_liquidacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtComprobaciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, 0, "", false, 0, 0, false, id_movimiento.ToString(), id_liquidacion.ToString() };

            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtComprobaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtComprobaciones;
        }
        /// <summary>
        /// Método encargado de Obtener las Comprobaciones Ligadas a un Deposito
        /// </summary>
        /// <param name="id_deposito">Deposito de la Comprobación</param>
        /// <returns></returns>
        public static DataTable ObtieneComprobacionesDeposito(int id_deposito)
        {
            //Declarando Objeto de Retorno
            DataTable dtComprobaciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_deposito, 0, 0, "", false, 0, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtComprobaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtComprobaciones;
        }
        /// <summary>
        /// Método encargado de Actualizar la Liquidación de la Comprobación
        /// </summary>
        /// <param name="id_liquidacion">Liquidación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaComprobacionLiquidacion(int id_liquidacion, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(this._id_deposito, this._id_concepto_comprobacion, this._id_autorizacion_comprobacion, this._observacion_comprobacion,
                                          this._bit_transferencia, this._id_transferencia, (DetalleLiquidacion.Estatus)objDetalleComprobacion.id_estatus_liquidacion, objDetalleComprobacion.id_unidad, 
                                          objDetalleComprobacion.id_operador, objDetalleComprobacion.id_proveedor_compania, objDetalleComprobacion.id_servicio, objDetalleComprobacion.id_movimiento,
                                          objDetalleComprobacion.fecha_liquidacion, id_liquidacion, objDetalleComprobacion.cantidad, objDetalleComprobacion.valor_unitario, id_usuario, this._habilitar);
        }

        #endregion
    }
}
