using SAT_CL.EgresoServicio;
using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Pagos de los Movimientos
    /// </summary>
    public class PagoMovimiento : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_pago_movimiento_tpm";

        private int _id_pago_movimiento;
        /// <summary>
        /// Atributo encaragdo de almacenar el Id del Pago del Movimiento
        /// </summary>
        public int id_pago_movimiento { get { return this._id_pago_movimiento; } }
        private int _id_pago;
        /// <summary>
        /// Atributo encaragdo de almacenar el Id del Pago
        /// </summary>
        public int id_pago { get { return this._id_pago; } }
        private int _id_movimiento;
        /// <summary>
        /// Atributo encaragdo de almacenar el Id del Movimiento
        /// </summary>
        public int id_movimiento { get { return this._id_movimiento; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encaragdo de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public PagoMovimiento()
        {   //Asignando Valores
            this._id_pago_movimiento = 0;
            this._id_pago = 0;
            this._id_movimiento = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_pago_movimiento">Id de Pago del Movimiento</param>
        public PagoMovimiento(int id_pago_movimiento)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_pago_movimiento);
        }

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_pago">Pago</param>
        /// <param name="id_movimiento">Movimiento</param>
        public PagoMovimiento(int id_pago, int id_movimiento)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_pago, id_movimiento);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~PagoMovimiento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_pago_movimiento">Id de Pago del Movimiento</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_pago_movimiento)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_pago_movimiento, 0, 0, 0, false, "", "" };
            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_pago_movimiento = id_pago_movimiento;
                        this._id_pago = Convert.ToInt32(dr["IdPago"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);                        
                    }
                    //Asignando Retorno Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_pago">Pago</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_pago, int id_movimiento)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_pago, id_movimiento, 0, false, "", "" };
            //Obteniendo Registro del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_pago_movimiento = Convert.ToInt32(dr["Id"]);
                        this._id_pago = Convert.ToInt32(dr["IdPago"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Retorno Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_pago">Id de Pago</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_pago, int id_movimiento, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_pago_movimiento, id_pago, id_movimiento, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Pagos de los Movimiento
        /// </summary>
        /// <param name="id_pago">Id de Pago</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPagoMovimiento(int id_pago, int id_movimiento, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_pago, id_movimiento, id_usuario, true, "", "" };

            //Instanciando Pago
            using(Pago pay = new Pago(id_pago))
            {
                //Validando que exista el Pago
                if(pay.habilitar)
                {
                    //Instanciando Liquidación
                    using(Liquidacion liq = new Liquidacion(pay.objDetallePago.id_liquidacion))
                    {
                        //Validando que exista la Liquidación
                        if (liq.habilitar)
                        {
                            //Obteniendo Entidad
                            int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                            //Validando que no existan Depositos Pendientes
                            if (!DetalleLiquidacion.ValidaDepositosPendientesMovimiento(id_movimiento, liq.id_estatus, liq.id_tipo_asignacion, id_entidad))

                                //Obteniendo Resultado del SP
                                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion(-3, "El Movimiento tiene Depositos Pendientes", false);
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No Existe la Liquidación");
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe el Pago");
            }
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Pagos de los Movimiento
        /// </summary>
        /// <param name="id_pago">Id de Pago</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaPagoMovimiento(int id_pago, int id_movimiento, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_pago, id_movimiento, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Pagos de los Movimientos
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPagoMovimiento(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_pago, this._id_movimiento, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Pagos de los Movimientos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPagoMovimiento()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_pago_movimiento);
        }

        /// <summary>
        /// Mètodo encargado de validar que existan Pagos Ligados a Un Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static bool ValidaPagoMovimiento(int id_movimiento)
        {   
            //Declarando variable 
            bool validaPago = false;
            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, id_movimiento, 0, true, "", "" };
            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignamos Valor
                    validaPago = true;
            }
            //Devolviendo Resultado Obtenido
            return validaPago;
        }
        /// <summary>
        /// Método encargado de Obtener la Liga de los Movimientos de un Pago
        /// </summary>
        /// <param name="id_pago">Pago</param>
        /// <returns></returns>
        public static DataTable ObtienePagosMovimiento(int id_pago)
        {
            //Declarando Objeto de Retorno
            DataTable dtPagos = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, 0, id_pago, 0, 0, true, "", "" };
            
            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignamos Valor
                    dtPagos = ds.Tables["Table"];
            }

            //Devolviendo resultado Obtenido
            return dtPagos;
        }

        #endregion
    }
}
