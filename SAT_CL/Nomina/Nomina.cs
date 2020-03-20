using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Nomina
{
    /// <summary>
    /// Clase encargada de Todas las Operaciones de 
    /// </summary>
    public class Nomina : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "nomina.sp_nomina_tn";

        private int _id_nomina;
        /// <summary>
        /// Atributo que almacena el Registro de Nomina
        /// </summary>
        public int id_nomina { get { return this._id_nomina; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo que almacena la Compania Emisora de la Nomina
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        private int _no_consecutivo;
        /// <summary>
        /// Atributo que almacena el No. Consecutivo por Compania
        /// </summary>
        public int no_consecutivo { get { return this._no_consecutivo; } }
        private int _id_nomina_origen;
        /// <summary>
        /// Atributo que almacena la Nomina de Origen
        /// </summary>
        public int id_nomina_origen { get { return this._id_nomina_origen; } }
        private DateTime _fecha_pago;
        /// <summary>
        /// Atributo que almacena la Fecha de Pago
        /// </summary>
        public DateTime fecha_pago { get { return this._fecha_pago; } }
        private DateTime _fecha_inicio_pago;
        /// <summary>
        /// Atributo que almacena la Fecha de Inicio de Pago
        /// </summary>
        public DateTime fecha_inicio_pago { get { return this._fecha_inicio_pago; } }
        private DateTime _fecha_fin_pago;
        /// <summary>
        /// Atributo que almacena la Fecha de Fin de Pago
        /// </summary>
        public DateTime fecha_fin_pago { get { return this._fecha_fin_pago; } }
        private DateTime _fecha_nomina;
        /// <summary>
        /// Atributo que almacena la Fecha de la Nomina
        /// </summary>
        public DateTime fecha_nomina { get { return this._fecha_nomina; } }
        private int _dias_pago;
        /// <summary>
        /// Atributo que almacena los Dias de Pago
        /// </summary>
        public int dias_pago { get { return this._dias_pago; } }
        private int _id_sucursal;
        /// <summary>
        /// Atributo que almacena la Sucursal
        /// </summary>
        public int id_sucursal { get { return this._id_sucursal; } }
        private byte _id_periodicidad_pago;
        /// <summary>
        /// Atributo que almacena la Periodicidad de Pago
        /// </summary>
        public byte id_periodicidad_pago { get { return this._id_periodicidad_pago; } }
        private byte _id_metodo_pago;
        /// <summary>
        /// Atributo que almacena el Método de Pago
        /// </summary>
        public byte id_metodo_pago { get { return this._id_metodo_pago; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Nomina()
        {
            //Asignando Atributos
            this._id_nomina = 0;
            this._id_compania_emisora = 0;
            this._no_consecutivo = 0;
            this._id_nomina_origen = 0;
            this._fecha_pago = DateTime.MinValue;
            this._fecha_inicio_pago = DateTime.MinValue;
            this._fecha_fin_pago = DateTime.MinValue;
            this._fecha_nomina = DateTime.MinValue;
            this._dias_pago = 0;
            this._id_sucursal = 0;
            this._id_periodicidad_pago = 0;
            this._id_metodo_pago = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_nomina">Id de Nomina</param>
        public Nomina(int id_nomina)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_nomina);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Nomina()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos
        /// </summary>
        /// <param name="id_nomina"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_nomina)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_nomina, 0, 0, 0, null, null, null, null, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo Cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_nomina = id_nomina;
                        this._id_compania_emisora = Convert.ToInt32(dr["IdCompaniaEmisora"]);
                        this._no_consecutivo = Convert.ToInt32(dr["NoConsecutivo"]);
                        this._id_nomina_origen = Convert.ToInt32(dr["IdNominaOrigen"]);
                        DateTime.TryParse(dr["FechaPago"].ToString(), out this._fecha_pago);
                        DateTime.TryParse(dr["FechaInicioPago"].ToString(), out this._fecha_inicio_pago);
                        DateTime.TryParse(dr["FechaFinPago"].ToString(), out this._fecha_fin_pago);
                        DateTime.TryParse(dr["FechaNomina"].ToString(), out this._fecha_nomina);
                        this._dias_pago = Convert.ToInt32(dr["DiasPago"]);
                        this._id_sucursal = Convert.ToInt32(dr["IdSucursal"]);
                        this._id_periodicidad_pago = Convert.ToByte(dr["IdPeriodicidadPago"]);
                        this._id_metodo_pago = Convert.ToByte(dr["IdMetodoPago"]);
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
        /// Método encargado de Actualizar los Atributos en BD
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="no_consecutivo">No. Consecutivo por Compania</param>
        /// <param name="id_nomina_origen">Nomina de Origen</param>
        /// <param name="fecha_pago">Fecha de Pago</param>
        /// <param name="fecha_inicio_pago">Fecha de Inicio del Pago</param>
        /// <param name="fecha_fin_pago">Fecha de de Fin del Pago</param>
        /// <param name="fecha_nomina">Fecha de Nomina</param>
        /// <param name="dias_pago">Dias de Pago</param>
        /// <param name="id_sucursal">Sucursal</param>
        /// <param name="id_periodicidad_pago">Periodicidad de Pago(Quincenal, Semanal, Mensual, etc...)</param>
        /// <param name="id_metodo_pago">Método de Pago(Efectivo, Transferencia, Cheque, etc...)</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaAtributosBD(int id_compania_emisora, int no_consecutivo, int id_nomina_origen, DateTime fecha_pago, 
                                                DateTime fecha_inicio_pago, DateTime fecha_fin_pago, DateTime fecha_nomina, int dias_pago, 
                                                int id_sucursal, byte id_periodicidad_pago, byte id_metodo_pago, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_nomina, id_compania_emisora, no_consecutivo, id_nomina_origen, fecha_pago, fecha_inicio_pago, 
                                 fecha_fin_pago, fecha_nomina, dias_pago, id_sucursal, id_periodicidad_pago, id_metodo_pago, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar si la Nomina Tiene Nominas de Empleado Timbradas
        /// </summary>
        /// <param name="id_nomina">Nomina</param>
        /// <returns></returns>
        private RetornoOperacion validaNominaTimbrada(int id_nomina)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 4, id_nomina, 0, 0, 0, null, null, null, null, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Validando si esta Timbrada
                        if (Convert.ToInt32(dr["Validacion"]) > 0)

                            //Instanciando Excepción
                            result = new RetornoOperacion(this._id_nomina, "La Nomina ha sido Timbrada", true);
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar la Nomina
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="no_consecutivo">No. Consecutivo por Compania</param>
        /// <param name="id_nomina_origen">Nomina de Origen</param>
        /// <param name="fecha_pago">Fecha de Pago</param>
        /// <param name="fecha_inicio_pago">Fecha de Inicio del Pago</param>
        /// <param name="fecha_fin_pago">Fecha de de Fin del Pago</param>
        /// <param name="fecha_nomina">Fecha de Nomina</param>
        /// <param name="dias_pago">Dias de Pago</param>
        /// <param name="id_sucursal">Sucursal</param>
        /// <param name="id_periodicidad_pago">Periodicidad de Pago(Quincenal, Semanal, Mensual, etc...)</param>
        /// <param name="id_metodo_pago">Método de Pago(Efectivo, Transferencia, Cheque, etc...)</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaNomina(int id_compania_emisora, int no_consecutivo, int id_nomina_origen, DateTime fecha_pago,
                                                DateTime fecha_inicio_pago, DateTime fecha_fin_pago, DateTime fecha_nomina, int dias_pago,
                                                int id_sucursal, byte id_periodicidad_pago, byte id_metodo_pago, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania_emisora, no_consecutivo, id_nomina_origen, fecha_pago, fecha_inicio_pago, fecha_fin_pago, 
                                 fecha_nomina, dias_pago, id_sucursal, id_periodicidad_pago, id_metodo_pago, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar la Nomina
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="no_consecutivo">No. Consecutivo por Compania</param>
        /// <param name="id_nomina_origen">Nomina de Origen</param>
        /// <param name="fecha_pago">Fecha de Pago</param>
        /// <param name="fecha_inicio_pago">Fecha de Inicio del Pago</param>
        /// <param name="fecha_fin_pago">Fecha de de Fin del Pago</param>
        /// <param name="fecha_nomina">Fecha de Nomina</param>
        /// <param name="dias_pago">Dias de Pago</param>
        /// <param name="id_sucursal">Sucursal</param>
        /// <param name="id_periodicidad_pago">Periodicidad de Pago(Quincenal, Semanal, Mensual, etc...)</param>
        /// <param name="id_metodo_pago">Método de Pago(Efectivo, Transferencia, Cheque, etc...)</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaNomina(int id_compania_emisora, int no_consecutivo, int id_nomina_origen, DateTime fecha_pago,
                                                DateTime fecha_inicio_pago, DateTime fecha_fin_pago, DateTime fecha_nomina, int dias_pago,
                                                int id_sucursal, byte id_periodicidad_pago, byte id_metodo_pago, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Nomina Timbrada
            result = this.validaNominaTimbrada(this._id_nomina);
            
            //Operación Exitosa?
            if (!result.OperacionExitosa)

                //Devolviendo Resultado Obtenido
                result = this.actualizaAtributosBD(id_compania_emisora, no_consecutivo, id_nomina_origen, fecha_pago, fecha_inicio_pago, fecha_fin_pago,
                                     fecha_nomina, dias_pago, id_sucursal, id_periodicidad_pago, id_metodo_pago, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                result = new RetornoOperacion(result.Mensaje);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Deshabilitar la Nomina
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaNomina(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Ambiente Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando Nomina Timbrada
                result = this.validaNominaTimbrada(this._id_nomina);

                //Operación Exitosa?
                if (!result.OperacionExitosa)
                {
                    //Obteniendo Nomina de Empleados
                    using(DataTable dtNominasEmpleado = NominaEmpleado.ObtieneNominasEmpleado(this._id_nomina))
                    {
                        //Validando que existan Nominas de Empleados
                        if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtNominasEmpleado))
                        {
                            //Recorriendo Nomina de Empleados
                            foreach(DataRow dr in dtNominasEmpleado.Rows)
                            {
                                //Instanciando Nomina de Empleados
                                using(NominaEmpleado ne = new NominaEmpleado(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando que exista el Registro
                                    if (ne.habilitar)
                                    {
                                        //Deshabilitando Nomina de Empleado
                                        result = ne.DeshabilitaNominaEmpleado(id_usuario);

                                        //Si la Operación no fue Correcta
                                        if (!result.OperacionExitosa)

                                            //Terminando Ciclo
                                            break;
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Nomina
                            result = new RetornoOperacion(this._id_nomina);
                    }

                    //Validando Operación
                    if (result.OperacionExitosa)
                    {
                        //Devolviendo Resultado Obtenido
                        result = this.actualizaAtributosBD(this._id_compania_emisora, this._no_consecutivo, this._id_nomina_origen, this._fecha_pago, this._fecha_inicio_pago, this._fecha_fin_pago,
                                             this._fecha_nomina, this._dias_pago, this._id_sucursal, this._id_periodicidad_pago, this._id_metodo_pago, id_usuario, false);

                        //Validando Operación
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion(result.Mensaje);
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Valores
        /// </summary>
        /// <returns></returns>
        public bool ActualizaNomina()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_nomina);
        }
        /// <summary>
        /// Método encargado de Copiar la Nomina
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion CopiaNomina(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            int idNomina = 0, idNominaEmpleado = 0;

            //Declarando Ambiente Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Armando Arreglo de Parametros
                object[] param = { 1, 0, this._id_compania_emisora, 0, this._id_nomina, null, null, null, null, 
                                 this._dias_pago, this._id_sucursal, this._id_periodicidad_pago, this._id_metodo_pago, id_usuario, true, "", "" };

                //Ejecutando SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                //Validando Operación Correcta
                if (result.OperacionExitosa)
                {
                    //Guardando Nomina
                    idNomina = result.IdRegistro;

                    //Obteniendo Nominas de Empleado
                    using(DataTable dtNominaEmpleado = NominaEmpleado.ObtieneNominasEmpleado(this._id_nomina))
                    {
                        //Validando que Existen Nominas
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtNominaEmpleado))
                        {
                            //Iniciando Ciclo de Nomina de Empleados
                            foreach(DataRow drNE in dtNominaEmpleado.Rows)
                            {
                                //Instanciando Nomina de Empleado
                                using(NominaEmpleado ne = new NominaEmpleado(Convert.ToInt32(drNE["Id"])))
                                {
                                    //Validando que existe el Registro
                                    if (ne.habilitar)
                                    {
                                        //Insertando Nomina de Empleado
                                        result = NominaEmpleado.InsertaNominaEmpleado(idNomina, ne.id_empleado, 0, 0, ne.total_gravado_percepcion, ne.total_gravado_deduccion,
                                                    ne.total_exento_percepcion, ne.total_exento_deduccion, ne.total_percepcion, ne.total_deduccion, id_usuario);

                                        //Validando Operación Correcta
                                        if (result.OperacionExitosa)
                                        {
                                            //Guardando Nomina
                                            idNominaEmpleado = result.IdRegistro;

                                            //Obteniendo Detalles
                                            using (DataTable dtDetallesEmp = DetalleNominaEmpleado.ObtieneDetalleNominaEmpleado(ne.id_nomina_empleado))
                                            {
                                                //Validando que Existen Nominas
                                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesEmp))
                                                {
                                                    //Iniciando Ciclo de Detalle
                                                    foreach (DataRow drDE in dtDetallesEmp.Rows)
                                                    {
                                                        //Instanciando Detalle
                                                        using (DetalleNominaEmpleado dne = new DetalleNominaEmpleado(Convert.ToInt32(drDE["Id"])))
                                                        {
                                                            //Validando Registro
                                                            if (dne.habilitar)
                                                            {
                                                                //Insertando Detalle
                                                                result = DetalleNominaEmpleado.InsertarDetalleNominaEmpleado(idNominaEmpleado, dne.id_tipo_pago,
                                                                                                dne.importe_gravado, dne.importe_exento, id_usuario);

                                                                //Si la Operación no fue Exitosa
                                                                if(!result.OperacionExitosa)

                                                                    //Terminando Ciclo
                                                                    break;
                                                            }
                                                            else
                                                            {
                                                                //Instanciando Nomina
                                                                result = new RetornoOperacion("No Existe el Detalle de la Nomina");

                                                                //Terminando Ciclo
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Nomina
                                                    result = new RetornoOperacion(idNomina);
                                            }

                                            //Operación Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Obteniendo Detalles
                                                using (DataTable dtNominaOtrosEmp = NominaOtros.ObtieneNominaOtros(ne.id_nomina_empleado))
                                                {
                                                    //Validando que Existen Nominas
                                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtNominaOtrosEmp))
                                                    {
                                                        //Iniciando Ciclo de Detalle
                                                        foreach (DataRow drNO in dtNominaOtrosEmp.Rows)
                                                        {
                                                            //Instanciando Nomina Otros
                                                            using (NominaOtros no = new NominaOtros(Convert.ToInt32(drNO["Id"])))
                                                            {
                                                                //Validando Registro
                                                                if (no.habilitar)
                                                                {
                                                                    //Insertando Detalle
                                                                    result = NominaOtros.InsertarNominaOtros(idNominaEmpleado,(NominaOtros.TipoNominaOtros)no.id_tipo, no.dias,(NominaOtros.SubTipo) no.id_subtipo, no.importe_gravado, no.importe_exento, no.cantidad, id_usuario);

                                                                    //Si la Operación no fue Exitosa
                                                                    if (!result.OperacionExitosa)

                                                                        //Terminando Ciclo
                                                                        break;
                                                                }
                                                                else
                                                                {
                                                                    //Instanciando Nomina
                                                                    result = new RetornoOperacion("No Existe la Nomina de Otros");

                                                                    //Terminando Ciclo
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Instanciando Nomina
                                                        result = new RetornoOperacion(idNomina);
                                                }
                                            }
                                        }
                                        else
                                            //Terminando Ciclo
                                            break;
                                    }
                                    else
                                    {   
                                        //Instanciando Nomina
                                        result = new RetornoOperacion("No Existe la Nómina del Empleado");

                                        //Terminando Ciclo
                                        break;
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Nomina
                            result = new RetornoOperacion(idNomina);

                        //Validando Operaciones
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Nomina
                            result = new RetornoOperacion(idNomina);

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
        /// Método encargado de Validar la Nomina
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidaNominaTimbrada()
        {
            //Invocando Método de Validación
            return this.validaNominaTimbrada(this._id_nomina);
        }


        /// <summary>
        /// Método encargado de Timbrar Toda la Nómina
        /// </summary>
        /// <param name="serie">Serie</param>
        /// <param name="id_cuenta_pago">Cuenta Pago del Emisor</param>
        /// <param name="ruta_xslr_co">Ruta para la cadena Original en Linea</param>
        /// <param name="ruta_xslr_co_local">Ruta para la Cadena Original desconectado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TimbraNomina_V3_2(string serie, int id_cuenta_pago, string ruta_xslr_co, string ruta_xslr_co_local, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion result = new RetornoOperacion(0);
            //Declaramos Objeto Resultado
            RetornoOperacion ResultadoMensaje = new RetornoOperacion(0);

            //Guardamos Mensaje
            string mensaje = "";
            //Instanciando Nomina de Empleados
            using (DataTable dtNominaEmpleados = SAT_CL.Nomina.NominaEmpleado.ObtieneNominasEmpleadoRegistrados(this._id_nomina))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtNominaEmpleados))
                {
                    //Recorriendo Nominas de Empleados
                    foreach (DataRow dr in dtNominaEmpleados.Rows)
                    {
                        //Instanciando Nómina Empleado
                        using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(dr["Id"])))
                        {
                            //Validando que exista el Registro
                            if (ne.habilitar)
                            {
                                //Timbrando Nómina del Empleado
                                result = ne.ImportaTimbraNominaEmpleadoComprobante_V3_2(serie, id_cuenta_pago, ruta_xslr_co, ruta_xslr_co_local, id_usuario);
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No Existe la Nómina del Empleado");

                        }
                        //Construyendo mensaje de este Timbrado de Nómina de Empleado
                        ResultadoMensaje = new RetornoOperacion(mensaje += string.Format("{0}, ", result.Mensaje), true);

                    }
                }
            }
            //Devolvemos Resultado
            return ResultadoMensaje;
        }


        public RetornoOperacion EnviaEmailCFDINomina()
        { 
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Devolviendo resultado
            return resultado;
        }


        #endregion
    }
}
