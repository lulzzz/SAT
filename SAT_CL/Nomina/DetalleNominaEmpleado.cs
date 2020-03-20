using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.Nomina
{
    /// <summary>
    /// Clase encargada de Todas las Operaciones de 
    /// </summary>
    public class DetalleNominaEmpleado:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que alamcena el nombre del store procedure de la tabla detalle nomina empleado
        /// </summary>
        private static string nom_sp = "nomina.sp_detalle_nomina_empleado_tdne";
        private int _id_detalle_nomina_empleado;
        /// <summary>
        /// Atributo que almacena el identificador de un detalle de nomina de un empleado
        /// </summary>
        public int id_detalle_nomina_empleado
        {
            get { return _id_detalle_nomina_empleado; }
        }
        private int _id_nomina_empleado;
        /// <summary>
        /// Atributo que almacena el identificador de la nomina de un empleado
        /// </summary>
        public int id_nomina_empleado
        {
            get { return _id_nomina_empleado; }
        }
        private int _id_tipo_pago;
        /// <summary>
        /// Atributo que alamcena los diferentes tipos de pago de nomina a un empleado
        /// </summary>
        public int id_tipo_pago
        {
            get { return _id_tipo_pago; }
        }
        private decimal _importe_gravado;
        /// <summary>
        /// Atributo qeu almacena la cantidad monetaria de impuestos gravados de nomina de un empleado
        /// </summary>
        public decimal importe_gravado
        {
            get { return _importe_gravado; }
        }
        private decimal _importe_exento;
        /// <summary>
        /// Atributo que almacena la cantidad monetaria de impuestos exento de nomina de un empleado 
        /// </summary>
        public decimal importe_exento
        {
            get { return _importe_exento; }

        }
        private bool _habilitar;
        /// <summary>
        /// Atributo que alamcena el estado de habilitación de un registro (habilitado/deshabilitado)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor default que inicializa los atributos  a un valor 0.
        /// </summary>
        public DetalleNominaEmpleado()
        {
            this._id_detalle_nomina_empleado = 0;
            this._id_nomina_empleado = 0;
            this._id_tipo_pago = 0;
            this._importe_exento = 0.0m;
            this._importe_gravado = 0.0m;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de la busqueda de un registro.
        /// </summary>
        /// <param name="id_detalle_nomina_empleado">Identificador que permite buscar un registro</param>
        public DetalleNominaEmpleado(int id_detalle_nomina_empleado)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_detalle_nomina_empleado);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase detalle nomina empleado
        /// </summary>
        ~DetalleNominaEmpleado()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de registros e inicializa los atributos con el resultado de la busqueda.
        /// </summary>
        /// <param name="id_detalle_nomina_empleado">Identificador del registro que se consultara</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_detalle_nomina_empleado)
        {
            //Creación de objeto retorno
            bool retorno = false;
            //Creación del arreglo necesario para alamcenar los parametros necesario para la consulta de registros a la base d e datos.
            object[] param = { 3, id_detalle_nomina_empleado, 0, 0, 0.0m, 0.0m, 0, false, "", "" };
            //Creación de la variable tipo tabla DS que alamcena el resultado de la invocación del método EjecutaProcAlmacenadoDataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida si existen datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas y almacena los datos en la variable r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_detalle_nomina_empleado = id_detalle_nomina_empleado;
                        this._id_nomina_empleado = Convert.ToInt32(r["IdNominaEmpleado"]);
                        this._id_tipo_pago = Convert.ToInt32(r["IdTipoPago"]);
                        this._importe_gravado = Convert.ToDecimal(r["ImporteGravado"]);
                        this._importe_exento = Convert.ToDecimal(r["ImporteExento"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la validación de los datos.
                    retorno = true;
                }
            }
            //Retornal el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los campos de un registro detalle nomina empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Permite actualizar el identificador de nomina de un empleado</param>
        /// <param name="id_tipo_pago">Permite actualizar el tipo de pago de nomina</param>
        /// <param name="importe_gravado">Pemite actualizar la cantidad monetaria de impueto gravado</param>
        /// <param name="importe_exento">Pemite actualizar la cantidad monetaria de impueto exento</param>
        /// <param name="id_usuario">Permite actualizar al usuario que realiza acciones sobre el registro</param>
        /// <param name="habilitar">Permite cambiar el estado de un registro (Habilitado/Deshabilitado)</param>
        /// <returns></returns>
        private RetornoOperacion editarDetalleNominaEmpleado(int id_nomina_empleado, int id_tipo_pago, decimal importe_gravado, decimal importe_exento, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo necesario para alamcenar los parametros necesario para la consulta de registros a la base d e datos.
            object[] param = { 2, this._id_detalle_nomina_empleado, id_nomina_empleado, id_tipo_pago, importe_gravado, importe_exento, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;

        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que permite insertar los campos de un registro detalle nomina empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Permite insertar el identificador de nomina de un empleado</param>
        /// <param name="id_tipo_pago">Permite insertar el tipo de pago de nomina</param>
        /// <param name="importe_gravado">Pemite insertat la cantidad monetaria de impueto gravado</param>
        /// <param name="importe_exento">Pemite insertar la cantidad monetaria de impueto exento</param>
        /// <param name="id_usuario">Permite insertar al usuario que realizo el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarDetalleNominaEmpleado(int id_nomina_empleado, int id_tipo_pago, decimal importe_gravado, decimal importe_exento, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idDetaleNominaEmpleado = 0;
            
            //Declarando Bloque Transaccional
            using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Nómina Empleado
                using (NominaEmpleado ne = new NominaEmpleado(id_nomina_empleado))
                {
                    //Validando que exista la Nomina
                    if (ne.habilitar)
                    {
                        //Validando que no Exista el Comprobante
                        if (ne.id_comprobante == 0)
                        {
                            //Creación del arreglo que almacena los parametros de actualización del registro
                            object[] param = { 1, 0, id_nomina_empleado, id_tipo_pago, importe_gravado, importe_exento, id_usuario, true, "", "" };

                            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
                            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

                            //Validando Operación Exitosa
                            if (retorno.OperacionExitosa)
                            {
                                //Asignando Valor
                                idDetaleNominaEmpleado = retorno.IdRegistro;

                                //Obteniendo Totales
                                using (DataTable dtTotalesEmpleado = NominaEmpleado.ObtieneTotalesEmpleado(id_nomina_empleado))
                                {
                                    //Validando que existen los Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesEmpleado))
                                    {
                                        //Inicializando Ciclo
                                        foreach (DataRow dr in dtTotalesEmpleado.Rows)
                                        {
                                            //Actualizando Totales de Nomina de Empleado
                                            retorno = ne.ActualizaTotalesNominaEmpleado(Convert.ToDecimal(dr["TotalGravadoPercepcion"]), Convert.ToDecimal(dr["TotalGravadoDeduccion"]),
                                                            Convert.ToDecimal(dr["TotalExentoPercepcion"]), Convert.ToDecimal(dr["TotalExentoDeduccion"]), id_usuario);
                                            break;
                                        }

                                        //Validando que la Operación fue Exitosa
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Instanciando Detalle en Retorno
                                            retorno = new RetornoOperacion(idDetaleNominaEmpleado);

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede Acceder a la Nómina del Empleado");
                }
            }            
            
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los campos de un registro detalle nomina empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Permite actualizar el identificador de nomina de un empleado</param>
        /// <param name="id_tipo_pago">Permite actualizar el tipo de pago de nomina</param>
        /// <param name="importe_gravado">Pemite actualizar la cantidad monetaria de impueto gravado</param>
        /// <param name="importe_exento">Pemite actualizar la cantidad monetaria de impueto exento</param>
        /// <param name="id_usuario">Permite actualizar al usuario que realiza acciones sobre el registro</param>
        public RetornoOperacion EditarDetalleNominaEmpleado(int id_nomina_empleado, int id_tipo_pago, decimal importe_gravado, decimal importe_exento, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idDetaleNominaEmpleado = 0;
            
            //Declarando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Nómina Empleado
                using (NominaEmpleado ne = new NominaEmpleado(this._id_nomina_empleado))
                {
                    //Validando que exista la Nomina
                    if (ne.habilitar)
                    {
                        //Validando que no Exista el Comprobante
                        if (ne.id_comprobante == 0)
                        {
                            //Invoca y retorna el método editarDetalleNominaEmpleado
                            retorno = this.editarDetalleNominaEmpleado(id_nomina_empleado, id_tipo_pago, importe_gravado, importe_exento, id_usuario, this._habilitar);

                            //Validando Operación Exitosa
                            if (retorno.OperacionExitosa)
                            {
                                //Asignando Valor
                                idDetaleNominaEmpleado = retorno.IdRegistro;

                                //Obteniendo Totales
                                using (DataTable dtTotalesEmpleado = NominaEmpleado.ObtieneTotalesEmpleado(this._id_nomina_empleado))
                                {
                                    //Validando que existen los Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesEmpleado))
                                    {
                                        //Inicializando Ciclo
                                        foreach (DataRow dr in dtTotalesEmpleado.Rows)
                                        {
                                            //Actualizando Totales de Nomina de Empleado
                                            retorno = ne.ActualizaTotalesNominaEmpleado(Convert.ToDecimal(dr["TotalGravadoPercepcion"]), Convert.ToDecimal(dr["TotalGravadoDeduccion"]),
                                                            Convert.ToDecimal(dr["TotalExentoPercepcion"]), Convert.ToDecimal(dr["TotalExentoDeduccion"]), id_usuario);
                                            break;
                                        }

                                        //Validando que la Operación fue Exitosa
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Instanciando Detalle en Retorno
                                            retorno = new RetornoOperacion(idDetaleNominaEmpleado);

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede Acceder a la Nómina del Empleado");
                }
            }

            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite cambiar el estado de un registro
        /// </summary>
        /// <param name="id_usuario">>Permite Identificar al ultimo usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarDetalleNominaEmpleado(int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idDetaleNominaEmpleado = 0;
            
            //Declarando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Nómina Empleado
                using (NominaEmpleado ne = new NominaEmpleado(this._id_nomina_empleado))
                {
                    //Validando que exista la Nomina
                    if (ne.habilitar)
                    {
                        //Validando que no Exista el Comprobante
                        if (ne.id_comprobante == 0)
                        {
                            //Invoca y retrona el método editarDEtalleNominaempleado
                            retorno = this.editarDetalleNominaEmpleado(this._id_nomina_empleado, this.id_tipo_pago, this.importe_gravado, this.importe_exento, id_usuario, false);

                            //Validando Operación Exitosa
                            if (retorno.OperacionExitosa)
                            {
                                //Asignando Valor
                                idDetaleNominaEmpleado = retorno.IdRegistro;

                                //Obteniendo Totales
                                using (DataTable dtTotalesEmpleado = NominaEmpleado.ObtieneTotalesEmpleado(this._id_nomina_empleado))
                                {
                                    //Validando que existen los Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesEmpleado))
                                    {
                                        //Inicializando Ciclo
                                        foreach (DataRow dr in dtTotalesEmpleado.Rows)
                                        {
                                            //Actualizando Totales de Nomina de Empleado
                                            retorno = ne.ActualizaTotalesNominaEmpleado(Convert.ToDecimal(dr["TotalGravadoPercepcion"]), Convert.ToDecimal(dr["TotalGravadoDeduccion"]),
                                                            Convert.ToDecimal(dr["TotalExentoPercepcion"]), Convert.ToDecimal(dr["TotalExentoDeduccion"]), id_usuario);
                                            break;
                                        }

                                        //Validando que la Operación fue Exitosa
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Instanciando Detalle en Retorno
                                            retorno = new RetornoOperacion(idDetaleNominaEmpleado);

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede Acceder a la Nómina del Empleado");
                }
            }

            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles de Nomina del Empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Nomina del Empleado</param>
        /// <param name="id_concepto_SAT">Concepto Nomina SAT</param>
        /// <param name="tipo_aplicacion">Tipo de Aplicación (Percepción, Deducción, Bonificación)</param>
        /// <returns></returns>
        public static DataTable ObtieneDetalleNominaEmpleado(int id_nomina_empleado, int id_concepto_SAT, Liquidacion.TipoCobroRecurrente.TipoAplicacion tipo_aplicacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetallesNomina = null;

            //Creación del arreglo que almacena los parametros de actualización del registro
            object[] param = { 4, (int)tipo_aplicacion, id_nomina_empleado, id_concepto_SAT, 0.00M, 0.00M, 0, false, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtDetallesNomina = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtDetallesNomina;
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles de la Nomina del Empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Nomina del Empleado</param>
        /// <returns></returns>
        public static DataTable ObtieneDetalleNominaEmpleado(int id_nomina_empleado)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetallesNomina = null;

            //Creación del arreglo que almacena los parametros de actualización del registro
            object[] param = { 5, 0, id_nomina_empleado, 0, 0.00M, 0.00M, 0, false, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtDetallesNomina = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtDetallesNomina;
        }

        #endregion
    }
}
