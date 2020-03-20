using FEv32 = SAT_CL.FacturacionElectronica;
using FEv33 = SAT_CL.FacturacionElectronica33;
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
    public class NominaEmpleado : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que define a los Estatus de la Nomina del Empleado
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Estatus Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Estatus Timbrado
            /// </summary>
            Timbrado,
            /// <summary>
            /// Estatus Cancelado
            /// </summary>
            Cancelado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "nomina.sp_nomina_empleado_tne";

        private int _id_nomina_empleado;
        /// <summary>
        /// Atributo que almacena el Identificador de la Nomina del Empleado
        /// </summary>
        public int id_nomina_empleado { get { return this._id_nomina_empleado; } }
        private int _id_nomina;
        /// <summary>
        /// Atributo que almacena la Nomina
        /// </summary>
        public int id_nomina { get { return this._id_nomina; } }
        private int _id_empleado;
        /// <summary>
        /// Atributo que almacena el Empleado de la Nomina
        /// </summary>
        public int id_empleado { get { return this._id_empleado; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo que almacena el Estatus de la Nomina
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo que almacena el Estatus de la Nomina (Enumeración)
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private int _id_comprobante;
        /// <summary>
        /// Atributo que almacena el Comprobante
        /// </summary>
        public int id_comprobante { get { return this._id_comprobante; } }
        private int _id_comprobante33;
        /// <summary>
        /// Atributo que almacena el Comprobante v3.3
        /// </summary>
        public int id_comprobante33 { get { return this._id_comprobante33; } }
        private decimal _total_gravado_percepcion;
        /// <summary>
        /// Atributo que almacena el Total Gravado de la Percepción
        /// </summary>
        public decimal total_gravado_percepcion { get { return this._total_gravado_percepcion; } }
        private decimal _total_gravado_deduccion;
        /// <summary>
        /// Atributo que almacena el Total Gravado de la Deducción
        /// </summary>
        public decimal total_gravado_deduccion { get { return this._total_gravado_deduccion; } }
        private decimal _total_exento_percepcion;
        /// <summary>
        /// Atributo que almacena el Total Exento de la Percepción
        /// </summary>
        public decimal total_exento_percepcion { get { return this._total_exento_percepcion; } }
        private decimal _total_exento_deduccion;
        /// <summary>
        /// Atributo que almacena el Total Exento de la Deducción
        /// </summary>
        public decimal total_exento_deduccion { get { return this._total_exento_deduccion; } }
        private decimal _total_percepcion;
        /// <summary>
        /// Atributo que almacena el Total de la Percepción
        /// </summary>
        public decimal total_percepcion { get { return this._total_percepcion; } }
        private decimal _total_deduccion;
        /// <summary>
        /// Atributo que almacena el Total de la Deducción
        /// </summary>
        public decimal total_deduccion { get { return this._total_deduccion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defectos
        /// </summary>
        public NominaEmpleado()
        {
            //Asignando Atributos
            this._id_nomina_empleado = 0;
            this._id_nomina = 0;
            this._id_empleado = 0;
            this._id_estatus = 0;
            this._id_comprobante = 0;
            this._id_comprobante33 = 0;
            this._total_gravado_percepcion = 0.00M;
            this._total_gravado_deduccion = 0.00M;
            this._total_exento_percepcion = 0.00M;
            this._total_exento_deduccion = 0.00M;
            this._total_percepcion = 0.00M;
            this._total_deduccion = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_nomina_empleado"></param>
        public NominaEmpleado(int id_nomina_empleado)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_nomina_empleado);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~NominaEmpleado()
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
        private bool cargaAtributosInstancia(int id_nomina_empleado)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_nomina_empleado, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

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
                        this._id_nomina_empleado = id_nomina_empleado;
                        this._id_nomina = Convert.ToInt32(dr["IdNomina"]);
                        this._id_empleado = Convert.ToInt32(dr["IdEmpleado"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._id_comprobante33 = Convert.ToInt32(dr["IdComprobante33"]);
                        this._total_gravado_percepcion = Convert.ToDecimal(dr["TotalGravadoPercepcion"]);
                        this._total_gravado_deduccion = Convert.ToDecimal(dr["TotalGravadoDeduccion"]);
                        this._total_exento_percepcion = Convert.ToDecimal(dr["TotalExentoPercepcion"]);
                        this._total_exento_deduccion = Convert.ToDecimal(dr["TotalExentoDeduccion"]);
                        this._total_percepcion = Convert.ToDecimal(dr["TotalPercepcion"]);
                        this._total_deduccion = Convert.ToDecimal(dr["TotalDeduccion"]);
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
        /// <param name="id_nomina">Nomina</param>
        /// <param name="id_empleado">Empleado de la Nomina</param>
        /// <param name="id_estatus">Estatus de la Nomina del Empleado</param>
        /// <param name="id_comprobante">Comprobante (CFDI)</param>
        /// <param name="total_gravado_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_gravado_deduccion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_deduccion">Total Gravado de la Percepción</param>
        /// <param name="total_percepcion">Total de la Percepción</param>
        /// <param name="total_deduccion">Total Gravado de la Percepción</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaAtributosBD(int id_nomina, int id_empleado, byte id_estatus, int id_comprobante, int id_comprobante33, decimal total_gravado_percepcion, 
                                        decimal total_gravado_deduccion, decimal total_exento_percepcion, decimal total_exento_deduccion, decimal total_percepcion, 
                                        decimal total_deduccion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_nomina_empleado, id_nomina, id_empleado, id_estatus, id_comprobante, id_comprobante33, 
                               total_gravado_percepcion, total_gravado_deduccion, total_exento_percepcion, total_exento_deduccion, 
                               total_percepcion, total_deduccion, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar la Nomina del Empleado
        /// </summary>
        /// <param name="id_nomina">Nomina</param>
        /// <param name="id_empleado">Empleado de la Nomina</param>
        /// <param name="id_comprobante">Comprobante (CFDI)</param>
        /// <param name="total_gravado_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_gravado_deduccion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_deduccion">Total Gravado de la Percepción</param>
        /// <param name="total_percepcion">Total de la Percepción</param>
        /// <param name="total_deduccion">Total Gravado de la Percepción</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaNominaEmpleado(int id_nomina, int id_empleado, int id_comprobante, int id_comprobante33, decimal total_gravado_percepcion,
                                        decimal total_gravado_deduccion, decimal total_exento_percepcion, decimal total_exento_deduccion, decimal total_percepcion,
                                        decimal total_deduccion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_nomina, id_empleado, Estatus.Registrado, id_comprobante, id_comprobante33, total_gravado_percepcion, total_gravado_deduccion, 
                                 total_exento_percepcion, total_exento_deduccion, total_percepcion, total_deduccion, id_usuario, true, "", "" };

            //Instanciando Nomina
            using (Nomina nomina = new Nomina(id_nomina))
            {
                //Validando Nomina
                if(nomina.habilitar)
                {
                    //Validando Nomina Timbrada
                    result = nomina.ValidaNominaTimbrada();

                    //Operación Exitosa?
                    if(!result.OperacionExitosa)

                        //Ejecutando SP
                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar la Nomina del Empleado
        /// </summary>
        /// <param name="id_nomina">Nomina</param>
        /// <param name="id_empleado">Empleado de la Nomina</param>
        /// <param name="estatus">Estatus de la Nomina del Empleado</param>
        /// <param name="id_comprobante">Comprobante (CFDI)</param>
        /// <param name="total_gravado_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_gravado_deduccion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_deduccion">Total Gravado de la Percepción</param>
        /// <param name="total_percepcion">Total de la Percepción</param>
        /// <param name="total_deduccion">Total Gravado de la Percepción</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaNominaEmpleado(int id_nomina, int id_empleado, Estatus estatus, int id_comprobante, int id_comprobante33, decimal total_gravado_percepcion,
                                        decimal total_gravado_deduccion, decimal total_exento_percepcion, decimal total_exento_deduccion, decimal total_percepcion,
                                        decimal total_deduccion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando que exista un Comprobante
            if (!(this._id_comprobante != 0 && this.estatus == Estatus.Timbrado))

                //Devolviendo Resultado Obtenido
                result = this.actualizaAtributosBD(id_nomina, id_empleado, (byte)estatus, id_comprobante, id_comprobante33, total_gravado_percepcion, total_gravado_deduccion,
                                     total_exento_percepcion, total_exento_deduccion, total_percepcion, total_deduccion, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                result = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Totales del Empleado
        /// </summary>
        /// <param name="total_gravado_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_gravado_deduccion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_percepcion">Total Gravado de la Percepción</param>
        /// <param name="total_exento_deduccion">Total Gravado de la Percepción</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTotalesNominaEmpleado(decimal total_gravado_percepcion, decimal total_gravado_deduccion, 
                                                        decimal total_exento_percepcion, decimal total_exento_deduccion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que exista un Comprobante
            if (!(this._id_comprobante != 0 && this._id_comprobante33 != 0 && this.estatus == Estatus.Timbrado))

                //Devolviendo Resultado Obtenido
                return this.actualizaAtributosBD(this._id_nomina, this._id_empleado, this._id_estatus, this._id_comprobante, this._id_comprobante33, total_gravado_percepcion, total_gravado_deduccion,
                                     total_exento_percepcion, total_exento_deduccion, this._total_percepcion, this._total_deduccion, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                result = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        // Método encargado de Actualizar el Estatus de la Nómina de Empleado
        /// </summary>
        /// <param name="estatus">Estatus de la Nómina Empleado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatus(Estatus estatus, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Declarando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Devolviendo Resultado Obtenido
                result = this.actualizaAtributosBD(this._id_nomina, this._id_empleado, (byte)estatus, this._id_comprobante, this._id_comprobante33, this._total_gravado_percepcion, this._total_gravado_deduccion,
                                        this._total_exento_percepcion, this._total_exento_deduccion, this._total_percepcion, this._total_deduccion, id_usuario, this._habilitar);

                //Operación Exitosa?
                if (result.OperacionExitosa)
                {
                    //Validando estatus Ingresado
                    if (estatus == Estatus.Cancelado)
                    {
                        //Validando Comprobante 3.3
                        if (this._id_comprobante33 > 0)
                        {
                            //Instanciando Compania
                            using (FEv33.Comprobante cmp = new FEv33.Comprobante(this._id_comprobante33))
                            {
                                //Validando Comprobante
                                if (cmp.habilitar)
                                {
                                    //Aztualizamos el Estatus del Comprobante a Cancelado
                                    //result = cmp.CancelaComprobante(id_usuario);
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se puede recuperar el Comprobante v3.3");
                            }
                        }
                        //Validando Comprobante 3.2
                        else if (this._id_comprobante > 0)
                        {
                            //Instanciando Compania
                            using (FEv32.Comprobante cmp = new FEv32.Comprobante(this._id_comprobante))
                            {
                                //Validando Comprobante
                                if (cmp.habilitar)

                                    //Aztualizamos el Estatus del Comprobante a Cancelado
                                    result = cmp.CancelaComprobante(id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se puede recuperar el Comprobante");
                            }
                        }
                    }

                    //Operación Exitosa?
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Nomina Empleado
                        result = new RetornoOperacion(this._id_nomina_empleado);

                        //Completando Transacción
                        trans.Complete();
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Deshabilitar la Nomina
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaNominaEmpleado(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Ambiente Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que exista un Comprobante
                if (!(this._id_comprobante != 0 && this._id_comprobante33 != 0 && this.estatus == Estatus.Timbrado))
                {
                    //Obteniendo Detalles
                    using (DataTable dtDetallesEmp = DetalleNominaEmpleado.ObtieneDetalleNominaEmpleado(this._id_nomina_empleado))
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
                                        result = dne.DeshabilitarDetalleNominaEmpleado(id_usuario);

                                        //Si la Operación no fue Exitosa
                                        if (!result.OperacionExitosa)

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
                            result = new RetornoOperacion(this._id_nomina_empleado);

                        //Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Detalles
                            using (DataTable dtNominaOtrosEmp = NominaOtros.ObtieneNominaOtros(this._id_nomina_empleado))
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
                                                result = no.DeshabilitarNominaOtros(id_usuario);

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
                                    result = new RetornoOperacion(this._id_nomina_empleado);
                            }
                        }

                        //Validando Operaciones
                        if (result.OperacionExitosa)
                        {
                            //Devolviendo Resultado Obtenido
                            result = this.actualizaAtributosBD(this._id_nomina, this._id_empleado, this._id_estatus, this._id_comprobante, this._id_comprobante33, this._total_gravado_percepcion, this._total_gravado_deduccion,
                                                 this._total_exento_percepcion, this._total_exento_deduccion, this._total_percepcion, this._total_deduccion, id_usuario, false);
                            
                            //Validando Operaciones
                            if (result.OperacionExitosa)
                                
                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                    
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Valores
        /// </summary>
        /// <returns></returns>
        public bool ActualizaNominaEmpleado()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_nomina_empleado);
        }
        /// <summary>
        /// Método encargado de Obtener las Nominas de los Empleados dada un Nomina
        /// </summary>
        /// <param name="id_nomina">Nomina</param>
        /// <returns></returns>
        public static DataTable ObtieneNominasEmpleado(int id_nomina)
        {
            //Declarando Objeto de Retorno
            DataTable dtNominasEmpleados = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_nomina, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtNominasEmpleados = ds.Tables["Table"];
            }

            //Devolviendo Resultado
            return dtNominasEmpleados;
        }
        /// <summary>
        /// Método encargado de Obtener los Totales del Empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Nomina del Empleado</param>
        /// <returns></returns>
        public static DataTable ObtieneTotalesEmpleado(int id_nomina_empleado)
        {
            //Declarando Objeto de Retorno
            DataTable dtNominasEmpleados = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, id_nomina_empleado, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtNominasEmpleados = ds.Tables["Table"];
            }

            //Devolviendo Resultado
            return dtNominasEmpleados;
        }

        /// <summary>
        /// Método encargado de Obtener los Datos de la Nomina de Empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Id Nomina Empleado</param>
        /// <param name="id_cuenta_pago">Cuenta de Pago</param>
        /// <returns></returns>
        public static DataSet ObtienesDatosFacturaElectronica(int id_nomina_empleado, int id_cuenta_pago)
        {
            //Declarando Objeto de Retorno
            DataSet dsNomina = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, id_nomina_empleado, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, id_cuenta_pago, "" };

            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))

                    //Asignando Valor Obtenido
                    dsNomina = ds;
            }

            //Devolviendo Resultado Obtenido
            return dsNomina;
        }

        /// <summary>
        /// Carga Armado XML de Recibo de Nómina
        /// </summary>
        /// <param name="id_nomina_empleado"> Id Nómina del Empleado</param>
        /// <param name="id_cuenta_pago">Cuenta pago</param>
        /// <returns></returns>
        public static DataSet CargaArmadoXMLReciboNomina(int id_nomina_empleado, int id_cuenta_pago)
        {
            //Declarando Objeto de Retorno
            DataSet dsNomina = null;

            //Armando Arreglo de Parametros
            object[] param = { 7, id_nomina_empleado, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, id_cuenta_pago, "" };

            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))

                    //Asignando Valor Obtenido
                    dsNomina = ds;
            }

            //Devolviendo Resultado Obtenido
            return dsNomina;
        }

        /// <summary>
        /// Método encargado de Obtener las Nominas de los Empleados en estatus Registrado para su Timbrado
        /// </summary>
        /// <param name="id_nomina">Nomina</param>
        /// <returns></returns>
        public static DataTable ObtieneNominasEmpleadoRegistrados(int id_nomina)
        {
            //Declarando Objeto de Retorno
            DataTable dtNominasEmpleados = null;

            //Armando Arreglo de Parametros
            object[] param = { 8, 0, id_nomina, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtNominasEmpleados = ds.Tables["Table"];
            }

            //Devolviendo Resultado
            return dtNominasEmpleados;
        }
        /// <summary>
        /// Importa Y Timbra Liquidación
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="ruta_xslr_co"></param>
        /// <param name="ruta_xslr_co_local"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ImportaTimbraNominaEmpleadoComprobante_V3_2(string serie, int id_cuenta_pago, string ruta_xslr_co, string ruta_xslr_co_local, int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Estatus de la Nómina 
                if ((Estatus)this._id_estatus == Estatus.Registrado)
                {

                    //Obtenemos los Datos de Nomina Empleado de acuerdo al esquema de Facturación Electrónica
                    using (DataSet dsNomina = ObtienesDatosFacturaElectronica(this._id_nomina_empleado, id_cuenta_pago), dsArmadoXML = CargaArmadoXMLReciboNomina(this._id_nomina_empleado, id_cuenta_pago))
                    {
                        //Validamos Registros
                        if (Validacion.ValidaOrigenDatos(dsNomina))
                        {
                            //Intsanciamos Nómina
                            using (Nomina objNomina = new Nomina(this._id_nomina))
                            {
                                //Importamos Factura a Factura Electrónica
                                resultado = FEv32.Comprobante.ImportaReciboNomina_V3_2(dsNomina.Tables["Table"], objNomina.id_compania_emisora, objNomina.id_sucursal, dsNomina.Tables["Table1"], null, dsNomina.Tables["Table3"], dsNomina.Tables["Table2"], dsNomina.Tables["Table5"],
                                                                                id_usuario);
                            }
                            //Validamos Resultaod de Timbrado
                            if (resultado.OperacionExitosa)
                            {
                                //Instaciamos Comprobante
                                using (FEv32.Comprobante objComprobante = new FEv32.Comprobante(resultado.IdRegistro))
                                {
                                    //Validamos Id Comprobante
                                    if (objComprobante.id_comprobante > 0)
                                    {
                                        //Actualizmos Id de Comprobantes
                                        resultado = EditaNominaEmpleado(this._id_nomina, this._id_empleado, (Estatus)this._id_estatus, resultado.IdRegistro, this._id_comprobante33, this._total_gravado_percepcion,
                                                                      this._total_gravado_deduccion, this._total_exento_percepcion, this._total_exento_deduccion, this._total_percepcion, this._total_deduccion,
                                                                      id_usuario);
                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Timbramos
                                            resultado = objComprobante.TimbraReciboNomina_V3_2(serie, id_usuario, ruta_xslr_co, ruta_xslr_co_local, false, dsArmadoXML.Tables["Table"],
                                                dsArmadoXML.Tables["Table2"], dsArmadoXML.Tables["Table1"], dsArmadoXML.Tables["Table4"], dsArmadoXML.Tables["Table3"], dsArmadoXML.Tables["Table5"], dsArmadoXML.Tables["Table6"]);

                                            //Actualizamos Estatus de la Nómina de Empleado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Recargamos Atributos
                                                if (this.ActualizaNominaEmpleado())
                                                {
                                                    //Actualizamos Estatus
                                                    resultado = ActualizaEstatus(Estatus.Timbrado, id_usuario);
                                                    //Validamos Resultado
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        resultado = new RetornoOperacion(this._id_nomina_empleado, "La Nómina del Empleado se ha Timbrado " + objComprobante.serie + objComprobante.folio.ToString(), true);
                                                        //Finalizamos transacción
                                                        scope.Complete();
                                                    }
                                                }
                                                else
                                                //Mostramos Mensaje Error
                                                resultado = new RetornoOperacion("Error al refrescar Atributos.");
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            resultado = new RetornoOperacion("No se encontró Información para exportación de la FE.");
                        }
                    }
                }
                else
                {
                    //Mostramos Mensaje Error
                    resultado = new RetornoOperacion("El estatus del Nómina no permite su edición.");
                }

            }
            //Devolvemos Resultado
            return resultado;
        }

        #endregion
    }
}
