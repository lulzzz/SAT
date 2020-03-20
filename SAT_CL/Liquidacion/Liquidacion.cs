using SAT_CL.Bancos;
using SAT_CL.Despacho;
using SAT_CL.EgresoServicio;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.FacturacionElectronica;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con la Liquidación
    /// </summary>
    public class Liquidacion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que indica el Estatus de la Liquidación
        /// </summary>
        public enum Estatus
        {   /// <summary>
            /// Estatus que indica que la Liquidación ha sido Registrada
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Estatus que indica que la Liquidación ha sido Liquidada
            /// </summary>
            Liquidado,
            /// <summary>
            /// Estatus que indica que la Liquidación ha sido Transferida
            /// </summary>
            Transferido,
            /// <summary>
            /// Estatus que indica que la Liquidación ha sido Depositada
            /// </summary>
            Depositado
        }
        /// <summary>
        /// Enumeración que indica el Tipo de Asignación
        /// </summary>
        public enum TipoAsignacion
        {   /// <summary>
            /// Expresa que la Entidad de Asignación es un Operador
            /// </summary>
            Unidad = 1,
            /// <summary>
            /// Expresa que la Entidad de Asignación es un Unidad
            /// </summary>
            Operador,
            /// <summary>
            /// Expresa que la Entidad de Asignación es un Proveedor
            /// </summary>
            Proveedor
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_liquidacion_tl";

        private int _id_liquidacion;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Liquidación
        /// </summary>
        public int id_liquidacion { get { return this._id_liquidacion; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo encargado de almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        private int _no_liquidacion;
        /// <summary>
        /// Atributo encargado de almacenar el No. de Liquidación
        /// </summary>
        public int no_liquidacion { get { return this._no_liquidacion; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de la Liquidación
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de la Liquidación según su Enumeración
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private byte _id_tipo_asignacion;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Asignación
        /// </summary>
        public byte id_tipo_asignacion { get { return this._id_tipo_asignacion; } }
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Asignación según su Enumeración
        /// </summary>
        public TipoAsignacion tipo_asignacion { get { return (TipoAsignacion)this._id_tipo_asignacion; } }
        private int _id_operador;
        /// <summary>
        /// Atributo encargado de almacenar el Operador
        /// </summary>
        public int id_operador { get { return this._id_operador; } }
        private int _id_unidad;
        /// <summary>
        /// Atributo encargado de almacenar la Unidad
        /// </summary>
        public int id_unidad { get { return this._id_unidad; } }
        private int _id_proveedor;
        /// <summary>
        /// Atributo encargado de almacenar el Proveedor
        /// </summary>
        public int id_proveedor { get { return this._id_proveedor; } }
        private DateTime _fecha_liquidacion;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Liquidación
        /// </summary>
        public DateTime fecha_liquidacion { get { return this._fecha_liquidacion; } }
        private decimal _total_salario;
        /// <summary>
        /// Atributo encargado de almacenar el Total del Salario
        /// </summary>
        public decimal total_salario { get { return this._total_salario; } }
        private decimal _total_deducciones;
        /// <summary>
        /// Atributo encargado de almacenar el Total de Deducciones
        /// </summary>
        public decimal total_deducciones { get { return this._total_deducciones; } }
        private decimal _total_sueldo;
        /// <summary>
        /// Atributo encargado de almacenar el Total del Sueldo
        /// </summary>
        public decimal total_sueldo { get { return this._total_sueldo; } }
        private decimal _total_anticipos;
        /// <summary>
        /// Atributo encargado de almacenar el Total de los Anticipos
        /// </summary>
        public decimal total_anticipos { get { return this._total_anticipos; } }
        private decimal _total_comprobaciones;
        /// <summary>
        /// Atributo encargado de almacenar el Total de Comprobaciones
        /// </summary>
        public decimal total_comprobaciones { get { return this._total_comprobaciones; } }
        private decimal _total_descuentos;
        /// <summary>
        /// Atributo encargado de almacenar el Total de los Descuentos
        /// </summary>
        public decimal total_descuentos { get { return this._total_descuentos; } }
        private decimal _total_alcance;
        /// <summary>
        /// Atributo encargado de almacenar el Total del Alcance
        /// </summary>
        public decimal total_alcance { get { return this._total_alcance; } }
        private int _dias_pagados;
        /// <summary>
        /// Atributo encargado de almacenar los Dias Pagados
        /// </summary>
        public int dias_pagados { get { return this._dias_pagados; } }
        private bool _bit_transferencia;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de Transferencia
        /// </summary>
        public bool bit_transferencia { get { return this._bit_transferencia; } }
        private int _id_transferencia;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Transferencia
        /// </summary>
        public int id_transferencia { get { return this._id_transferencia; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private byte[] _row_version;
        /// <summary>
        /// Atributo encargado de almacenar la Version del Registro
        /// </summary>
        public byte[] row_version { get { return this._row_version; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Liquidacion()
        {   //Asignando Valores
            this._id_liquidacion = 0;
            this._id_compania_emisora = 0;
            this._no_liquidacion = 0;
            this._id_estatus = 0;
            this._id_tipo_asignacion = 0;
            this._id_operador = 0;
            this._id_unidad = 0;
            this._id_proveedor = 0;
            this._fecha_liquidacion = DateTime.MinValue;
            this._total_salario = 0;
            this._total_deducciones = 0;
            this._total_sueldo = 0;
            this._total_anticipos = 0;
            this._total_comprobaciones = 0;
            this._total_descuentos = 0;
            this._total_alcance = 0;
            this._dias_pagados = 0;
            this._bit_transferencia = false;
            this._id_transferencia = 0;
            this._habilitar = false;
            this._row_version = null;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_liquidacion">Id de Liquidación</param>
        public Liquidacion(int id_liquidacion)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_liquidacion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Liquidacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_liquidacion">Id de Liquidación</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_liquidacion)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_liquidacion, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, false, null, "", "" };
            //Instanciando 
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_liquidacion = id_liquidacion;
                        this._id_compania_emisora = Convert.ToInt32(dr["IdCompaniaEmisora"]);
                        this._no_liquidacion = Convert.ToInt32(dr["NoLiquidacion"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_tipo_asignacion = Convert.ToByte(dr["IdTipoAsignacion"]);
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._id_proveedor = Convert.ToInt32(dr["IdProveedor"]);
                        DateTime.TryParse(dr["FechaLiquidacion"].ToString(), out this._fecha_liquidacion);
                        this._total_salario = Convert.ToDecimal(dr["TotalSalario"]);
                        this._total_deducciones = Convert.ToDecimal(dr["TotalDeducciones"]);
                        this._total_sueldo = Convert.ToDecimal(dr["TotalSueldo"]);
                        this._total_anticipos = Convert.ToDecimal(dr["TotalAnticipos"]);
                        this._total_comprobaciones = Convert.ToDecimal(dr["TotalComprobaciones"]);
                        this._total_descuentos = Convert.ToDecimal(dr["TotalDescuentos"]);
                        this._total_alcance = Convert.ToDecimal(dr["TotalAlcance"]);
                        this._dias_pagados = Convert.ToInt32(dr["DiasPagados"]);
                        this._bit_transferencia = Convert.ToBoolean(dr["BitTransferencia"]);
                        this._id_transferencia = Convert.ToInt32(dr["IdTransferencia"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._row_version = (byte[])dr["RowVersion"];
                    }
                    //Asignando Retorno Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="no_liquidacion">No. Liquidación</param>
        /// <param name="estatus">Estatus de la Liquidación</param>
        /// <param name="tipo_asignacion">Tipo de Entidad de Asignación</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <param name="total_salario">Total del Salario</param>
        /// <param name="total_deducciones">Total de Deducciones</param>
        /// <param name="total_sueldo">Total de Sueldo</param>
        /// <param name="total_anticipos">Total de Anticipos</param>
        /// <param name="total_comprobaciones">Total de Comprobaciones</param>
        /// <param name="total_descuentos">Total de Descuentos</param>
        /// <param name="total_alcance">Total de Alcance</param>
        /// <param name="dias_pagados">Dias Pagados</param>
        /// <param name="bit_transferencia">Bit de Transferencia</param>
        /// <param name="id_transferencia">Id de Transferencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_compania_emisora, int no_liquidacion, Liquidacion.Estatus estatus, TipoAsignacion tipo_asignacion, int id_operador, int id_unidad, int id_proveedor, 
                                                    DateTime fecha_liquidacion, decimal total_salario, decimal total_deducciones, decimal total_sueldo, decimal total_anticipos,
                                                    decimal total_comprobaciones, decimal total_descuentos, decimal total_alcance, int dias_pagados, bool bit_transferencia, int id_transferencia,
                                                    int id_usuario, bool habilitar)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando versión actual de registro
            if (validarVersion())
            {
                //Armando Arreglo de Parametros
                object[] param = { 2, this._id_liquidacion, id_compania_emisora, no_liquidacion, (byte)estatus, (byte)tipo_asignacion, id_operador, id_unidad, id_proveedor, 
                                   fecha_liquidacion, total_salario, total_deducciones, total_sueldo, total_anticipos, total_comprobaciones, 
                                   total_descuentos, total_alcance, dias_pagados,  bit_transferencia, id_transferencia, id_usuario, habilitar, this._row_version, "", "" };

                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }
            //De lo contrario
            else
                result = new RetornoOperacion("El registro fue modificado en BD desde la última vez que fue consultado.");
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Valida que la versión actual del registro en BD y la contenida en este objeto sea la misma
        /// </summary>
        /// <returns></returns>
        private bool validarVersion()
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Declarando arreglo de parámetros para consulta en BD
            object[] param = { 6, this._id_liquidacion, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, false, this._row_version, "", "" };

            //Realizando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Indicando asignación correcta de atributos
                    resultado = true;
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Liquidaciones
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="no_liquidacion">No. Liquidación</param>
        /// <param name="tipo_asignacion">Tipo de Entidad de Asignación</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <param name="total_salario">Total del Salario</param>
        /// <param name="total_deducciones">Total de Deducciones</param>
        /// <param name="total_sueldo">Total de Sueldo</param>
        /// <param name="total_anticipos">Total de Anticipos</param>
        /// <param name="total_comprobaciones">Total de Comprobaciones</param>
        /// <param name="total_descuentos">Total de Descuentos</param>
        /// <param name="total_alcance">Total de Alcance</param>
        /// <param name="dias_pagados">Dias Pagados</param>
        /// <param name="bit_transferencia">Bit de Transferencia</param>
        /// <param name="id_transferencia">Id de Transferencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaLiquidacion(int id_compania_emisora, int no_liquidacion, TipoAsignacion tipo_asignacion, int id_operador, int id_unidad, int id_proveedor,
                                                    DateTime fecha_liquidacion, decimal total_salario, decimal total_deducciones, decimal total_sueldo, decimal total_anticipos,
                                                    decimal total_comprobaciones, decimal total_descuentos, decimal total_alcance, int dias_pagados, bool bit_transferencia, int id_transferencia, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania_emisora, no_liquidacion, (byte)Liquidacion.Estatus.Registrado, (byte)tipo_asignacion, id_operador, id_unidad, id_proveedor, 
                               fecha_liquidacion, total_salario, total_deducciones, total_sueldo, total_anticipos, total_comprobaciones, 
                               total_descuentos, total_alcance, dias_pagados, bit_transferencia, id_transferencia, id_usuario, true, null, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Liquidaciones
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="no_liquidacion">No. Liquidación</param>
        /// <param name="estatus">Estatus de la Liquidación</param>
        /// <param name="tipo_asignacion">Tipo de Entidad de Asignación</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <param name="total_salario">Total del Salario</param>
        /// <param name="total_deducciones">Total de Deducciones</param>
        /// <param name="total_sueldo">Total de Sueldo</param>
        /// <param name="total_anticipos">Total de Anticipos</param>
        /// <param name="total_comprobaciones">Total de Comprobaciones</param>
        /// <param name="total_descuentos">Total de Descuentos</param>
        /// <param name="total_alcance">Total de Alcance</param>
        /// <param name="dias_pagados">Dias Pagados</param>
        /// <param name="bit_transferencia">Bit de Transferencia</param>
        /// <param name="id_transferencia">Id de Transferencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaLiquidacion(int id_compania_emisora, int no_liquidacion, Estatus estatus, TipoAsignacion tipo_asignacion, int id_operador, int id_unidad, int id_proveedor,
                                                 DateTime fecha_liquidacion, decimal total_salario, decimal total_deducciones, decimal total_sueldo, decimal total_anticipos,
                                                 decimal total_comprobaciones, decimal total_descuentos, decimal total_alcance, int dias_pagados, bool bit_transferencia, int id_transferencia, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_compania_emisora, no_liquidacion, estatus, tipo_asignacion, id_operador, id_unidad, id_proveedor,
                               fecha_liquidacion, total_salario, total_deducciones, total_sueldo, total_anticipos, total_comprobaciones,
                               total_descuentos, total_alcance, dias_pagados, bit_transferencia, id_transferencia, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Actualizamos la Liquidación a Depositado
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusADepositado(int id_usuario)
        {                        
                return this.actualizaRegistros(this._id_compania_emisora, this._no_liquidacion, Liquidacion.Estatus.Depositado, (TipoAsignacion)this._id_tipo_asignacion,
                                              this._id_operador, this._id_unidad, this._id_proveedor, this._fecha_liquidacion, this._total_salario, this._total_deducciones,
                                              this._total_sueldo, this._total_anticipos, this._total_comprobaciones, this._total_descuentos, this._total_alcance, this._dias_pagados, 
                                              this._bit_transferencia, this._id_transferencia,                                              id_usuario, this._habilitar);           
        }
        /// <summary>
        /// Método encargado de Validar
        /// </summary>
        /// <returns></returns>
        public bool ValidaPagosLiquidacion()
        {
            //Declarando Objeto de retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 12, id_liquidacion, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, false, null, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo cada Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Si el Indicador es 0 no existen Pagos
                        result = Convert.ToInt32(dr["Indicador"]) == 0 ? false : true;

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Cerrar la Liquidación
        /// </summary>
        /// <param name="total_salario">Monto Total Referente al Salario</param>
        /// <param name="total_deducciones">Monto Total Referente a las Deducciones</param>
        /// <param name="total_sueldo">Monto Total Referente al Sueldo</param>
        /// <param name="total_anticipos">Monto Total Referente a los Anticipos</param>
        /// <param name="total_comprobaciones">Monto Total Referente a las Comprobaciones</param>
        /// <param name="total_descuentos">Monto Total Referente a los Descuentos</param>
        /// <param name="total_alcance">Monto Total Referente al Alcance de la Liquidación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion CierraLiquidacion(decimal total_salario, decimal total_deducciones, decimal total_sueldo, decimal total_anticipos, decimal total_comprobaciones,
                                                 decimal total_descuentos, decimal total_alcance, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            TimeSpan dias_pagados_actualizados = new TimeSpan(0, 0, 0);
            DateTime fecha = DateTime.MinValue;
            bool cargos_recurrentes = false;
            
            //Inicializando la Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo Entidad
                int id_entidad = (TipoAsignacion)this.id_tipo_asignacion == TipoAsignacion.Operador ? this._id_operador : (TipoAsignacion)this.id_tipo_asignacion == TipoAsignacion.Unidad ? this._id_unidad : this._id_proveedor;

                //Obteniendo la Ultima Fecha de una Liquidación Anterior
                fecha = Liquidacion.ObtieneUltimaFechaLiquidacion(this._id_compania_emisora, (TipoAsignacion)this._id_tipo_asignacion, id_entidad);

                //Validando que exista una Fecha de Ultima Liquidación
                if (fecha != DateTime.MinValue)
                    //Obtiene Dias pagados
                    dias_pagados_actualizados = this._fecha_liquidacion - fecha;
                else
                {
                    //Obteniendo Primer Viaje
                    fecha = SAT_CL.Despacho.Reporte.ObtieneFechaPrimerViajeAsignado(id_entidad, (int)this.id_tipo_asignacion);
                    //Validando que exista una Fecha del Primer Viaje
                    if (fecha != DateTime.MinValue)
                        //Obtiene Dias pagados
                        dias_pagados_actualizados = this._fecha_liquidacion - fecha;
                }

                //Invocando Método de Actualización
                result = this.actualizaRegistros(this.id_compania_emisora, this._no_liquidacion, Estatus.Liquidado, this.tipo_asignacion, this.id_operador, this.id_unidad, this.id_proveedor,
                                   this.fecha_liquidacion, total_salario, total_deducciones, total_sueldo, total_anticipos, total_comprobaciones,
                                   total_descuentos, total_alcance, dias_pagados_actualizados.Days, this._bit_transferencia, this._id_transferencia, id_usuario, this._habilitar);

                //Validando que se haya Actualizado el Encabezado de la Liquidación
                if (result.OperacionExitosa)
                {
                    //Obteniendos Cargos Recurrentes
                    using (DataTable dtCargosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneCobrosRecurrentesEntidad(this._id_tipo_asignacion, this._id_unidad, this._id_operador, this._id_proveedor, this._id_compania_emisora, this._fecha_liquidacion))
                    {
                        //Validando que existan los Cargos recurrentes
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCargosRecurrentes))
                        {
                            //Recorriendo los Registros
                            foreach (DataRow dr in dtCargosRecurrentes.Rows)
                            {
                                //Instanciando Cobro Recurrente
                                using (SAT_CL.Liquidacion.CobroRecurrente cr = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando que exista 
                                    if (cr.id_cobro_recurrente > 0)
                                    {
                                        //Actualizando Total y asignando Valor de la Operación
                                        result = cr.ActualizaTotalCobroRecurrente(this._id_liquidacion, this._fecha_liquidacion, this._id_unidad, this._id_operador, this._id_proveedor, id_usuario);

                                        //Validando que se haya actualizado Bien
                                        if (!result.OperacionExitosa)

                                            //Terminando Ciclo
                                            break;
                                    }
                                }
                            }

                            //Si hay Cargos Recurrentes
                            cargos_recurrentes = true;
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion(1, "No existen Cargos Recurrentes", true);
                    }
                }

                //Validando que se hayan Actualizado los Cobros Recurrentes de la Liquidación
                if (result.OperacionExitosa)
                {
                    //Obteniendo Tipo de Asignación
                    MovimientoAsignacionRecurso.Tipo tipo_asignacion = (TipoAsignacion)this._id_tipo_asignacion == TipoAsignacion.Operador ? MovimientoAsignacionRecurso.Tipo.Operador : (TipoAsignacion)this._id_tipo_asignacion == TipoAsignacion.Unidad ? MovimientoAsignacionRecurso.Tipo.Unidad : MovimientoAsignacionRecurso.Tipo.Tercero;

                    //Obteniendo Movimientos
                    using (DataSet ds = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneMovimientosYDetallesPorLiquidacion(this._id_liquidacion, DetalleLiquidacion.Estatus.Registrado, id_entidad, (byte)tipo_asignacion))
                    {
                        //Validando que existan Movimientos
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table") && TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table1"))
                        {
                            //Recorriendo Movimientos
                            foreach (DataRow drMov in ds.Tables["Table"].Rows)
                            {
                                //Validando que exista un Movimiento
                                if (Convert.ToInt32(drMov["IdMovimiento"]) > 0)
                                {
                                    //Obteniendo Asignacion de Recursos
                                    using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Convert.ToInt32(drMov["IdMovimiento"]), MovimientoAsignacionRecurso.Estatus.Terminado, tipo_asignacion, id_entidad))
                                    {
                                        //Validando que existe la Asignación del Recurso
                                        if (mar.id_movimiento_asignacion_recurso > 0)
                                        {
                                            //Actualizando Estatus a "Liquidado"
                                            result = mar.ActualizaEstatusMovimientoAsignacionRecurso(MovimientoAsignacionRecurso.Estatus.Liquidado, id_usuario);

                                            //Validando que la operación haya sido Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Obteniendo Asignación de Diesel
                                                using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselMovimiento(Convert.ToInt32(drMov["IdMovimiento"]), this._id_liquidacion, id_entidad, this._id_tipo_asignacion))
                                                {
                                                    //Validando que existan Registros
                                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                                                    {
                                                        //Recorriendo Ciclo
                                                        foreach (DataRow drDiesel in dtDiesel.Rows)
                                                        {
                                                            //Instanciando Vale de Diesel
                                                            using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new AsignacionDiesel(Convert.ToInt32(drDiesel["Id"])))
                                                            {
                                                                //Validando que exista el Vale
                                                                if (ad.id_asignacion_diesel > 0)
                                                                {
                                                                    //Liquidando Vale
                                                                    result = ad.LiquidaValeDiesel(id_liquidacion, this._fecha_liquidacion, id_usuario);

                                                                    //Validando la Operación
                                                                    if (!result.OperacionExitosa)
                                                                        //Terminando Ciclo
                                                                        break;
                                                                }
                                                                else
                                                                {
                                                                    //Instanciando Excepción
                                                                    result = new RetornoOperacion("Vale inaccesible, Imposible su Liquidación");
                                                                    //Terminando Ciclo
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Instanciando Excepcion
                                                        result = new RetornoOperacion(1, "No existen Vales de Diesel", true);
                                                }

                                                //Validando que la operación haya sido Exitosa
                                                if (!result.OperacionExitosa)
                                                    //Finalizando Ciclo
                                                    break;
                                            }
                                            else
                                                //Finalizando Ciclo
                                                break;
                                        }
                                        else
                                            //Finalizando Ciclo
                                            break;
                                    }
                                }
                            }

                            //Recorriendo Detalles de Liquidación
                            foreach (DataRow drDL in ds.Tables["Table1"].Rows)
                            {
                                //Instanciando Detalle de Liquidación
                                using (DetalleLiquidacion dl = new DetalleLiquidacion(Convert.ToInt32(drDL["Id"])))
                                {
                                    //Validando que exista el Detalle de Liquidación
                                    if (dl.id_detalle_liquidacion > 0)
                                    {
                                        //Validando Entidad
                                        if (dl.id_tabla == 51)
                                        {
                                            //Instanciando Deposito
                                            using (Deposito dep = new Deposito(dl.id_registro))
                                            {
                                                //Validando que este en el Estatus Correcto
                                                if (dep.habilitar && dep.Estatus == Deposito.EstatusDeposito.PorLiquidar)

                                                    //Liquida Detalle
                                                    result = dl.LiquidaDetalle(this._id_liquidacion, this._fecha_liquidacion, id_usuario);
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion(string.Format("El Deposito '{0}' no esta Depositado", dep.no_deposito));
                                            }
                                        }
                                        else
                                            //Liquida Detalle
                                            result = dl.LiquidaDetalle(this._id_liquidacion, this._fecha_liquidacion, id_usuario);
                                        

                                        //Validando que se haya Realizado la Operación exitosamente
                                        if (!result.OperacionExitosa)
                                            //Terminando Ciclo
                                            break;
                                    }
                                    else
                                        //Terminando Ciclo
                                        break;
                                }
                            }
                        }
                        else
                            //Instanciando Excepcion
                            result = new RetornoOperacion(0, "No existen Movimientos y/ó Cargos Recurrentes en la Liquidación", cargos_recurrentes);

                        //Validando que las Operaciones de los Movimientos y Detalles haya sido Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Invocando Método de Actualización
                            this.ActualizaLiquidacion();

                            //TO DO: Método que Inserta el Registro Egreso-Ingreso
                            result = EgresoIngreso.InsertaPagoLiquidacion(this._id_compania_emisora, this._id_liquidacion, this._total_alcance, this._total_alcance, this._fecha_liquidacion.Date, id_usuario);
                        }

                        //Validando que la Operación del Egreso e Ingreso haya sido Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Liqudiación
                            result = new RetornoOperacion(this._id_liquidacion);

                            //Completando Transacción
                            trans.Complete();
                        }
                    }

                }

            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Público encargado de Abrir la Liquidación
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion AbreLiquidacion(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando el Estatus de la Liquidación
                if ((Estatus)this._id_estatus == Estatus.Liquidado)
                {
                    //Obteniendo Entidad
                    int id_entidad = (TipoAsignacion)this._id_tipo_asignacion == TipoAsignacion.Operador ? this._id_operador : (TipoAsignacion)this._id_tipo_asignacion == TipoAsignacion.Unidad ? this._id_unidad : this._id_proveedor;
                    MovimientoAsignacionRecurso.Tipo tipo_asignacion = (TipoAsignacion)this._id_tipo_asignacion == TipoAsignacion.Operador ? MovimientoAsignacionRecurso.Tipo.Operador : (TipoAsignacion)this._id_tipo_asignacion == TipoAsignacion.Unidad ? MovimientoAsignacionRecurso.Tipo.Unidad : MovimientoAsignacionRecurso.Tipo.Tercero;

                    //Invocando Método de Actualización
                    result = this.actualizaRegistros(this.id_compania_emisora, this._no_liquidacion, Estatus.Registrado, this.tipo_asignacion, this.id_operador, this.id_unidad, this.id_proveedor,
                                       this.fecha_liquidacion, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 
                                       this._dias_pagados, this._bit_transferencia, this._id_transferencia, id_usuario, this._habilitar);

                    //Validando que se haya actualizado el Encabezado de la Liquidación
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Detalles
                        using (DataSet ds = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneMovimientosYDetallesPorLiquidacion(id_liquidacion, DetalleLiquidacion.Estatus.Liquidado, id_entidad, (byte)tipo_asignacion))
                        {
                            //Validando que existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                            {
                                //Recorriendo Movimientos
                                foreach (DataRow dr in ds.Tables["Table"].Rows)
                                {
                                    //Validando que exista un Movimiento
                                    if (Convert.ToInt32(dr["IdMovimiento"]) > 0)
                                    {
                                        //Obteniendo Asignacion de Recursos
                                        using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Convert.ToInt32(dr["IdMovimiento"]), MovimientoAsignacionRecurso.Estatus.Liquidado, tipo_asignacion, id_entidad))
                                        {
                                            //Validando que existe la Asignación del Recurso
                                            if (mar.id_movimiento_asignacion_recurso > 0)
                                            {
                                                //Actualizando Estatus a "Liquidado"
                                                result = mar.ActualizaEstatusMovimientoAsignacionRecurso(MovimientoAsignacionRecurso.Estatus.Terminado, id_usuario);

                                                //Validando que la operación haya sido Exitosa
                                                if (!result.OperacionExitosa)
                                                    //Finalizando Ciclo
                                                    break;

                                            }
                                            else
                                                //Finalizando Ciclo
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Liquidación
                                result = new RetornoOperacion(this.id_liquidacion);

                            //Validando que se hayan actualizado los Movimientos
                            if (result.OperacionExitosa)
                            {
                                //Validando que existan Detalles de Liquidación
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table1"))
                                {
                                    //Recorriendo Detalles de Liquidación
                                    foreach (DataRow dr in ds.Tables["Table1"].Rows)
                                    {
                                        //Instanciando Detalle
                                        using (DetalleLiquidacion dl = new DetalleLiquidacion(Convert.ToInt32(dr["Id"])))
                                        {
                                            //Validando que exista el Registro
                                            if (dl.id_detalle_liquidacion > 0)
                                            {
                                                //Validando que el Detalle Venga de un Cobro Recurrente
                                                if (dl.id_tabla == 77)
                                                {
                                                    //Instanciando Cobro Recurrente
                                                    using (SAT_CL.Liquidacion.CobroRecurrente cr = new SAT_CL.Liquidacion.CobroRecurrente(dl.id_registro))
                                                    {
                                                        //Validando que exista el cobro recurrente
                                                        if (cr.id_cobro_recurrente > 0)

                                                            //Regresando Total del Cobro Recurrente
                                                            result = cr.RegresaTotalCobroRecurrente(id_usuario);
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("No se pudo acceder al Cobro Recurrente");
                                                    }

                                                    //Validando que el Detalle de Cobro se haya Deshabilitado
                                                    if (result.OperacionExitosa)

                                                        //Invoca Método de Deshabilitación del Detalle
                                                        result = dl.DeshabilitaDetalleLiquidacion(id_usuario);
                                                }
                                                else
                                                    //Invoca Método de Apertura de la Liquidación
                                                    result = dl.AbreLiquidacionDetalle(id_usuario);

                                                //Validando que la Operacion haya sido Exitosa
                                                if (!result.OperacionExitosa)
                                                    //Terminando Ciclo
                                                    break;
                                            }
                                            else
                                            {
                                                //Instanciando Excepcion
                                                result = new RetornoOperacion("No se puede acceder al Detalle de Liquidación");
                                                //Termiando Ciclo
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion(this._id_liquidacion);
                            }

                                //Validando que las Operaciones de los Movimientos y Detalles haya sido Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Validando que la Operación del Egreso e Ingreso haya sido Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Instanciando Liqudiación
                                    result = new RetornoOperacion(this._id_liquidacion);

                                    //Completando Transacción
                                    trans.Complete();
                                }
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Liquidación esta en Estatus " + ((Estatus)this._id_estatus).ToString());
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Liquidaciones
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaLiquidacion(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //

            
            
            //Invocando Método de Actualización
            result = this.actualizaRegistros(this._id_compania_emisora, this._no_liquidacion, this.estatus, this.tipo_asignacion, this._id_operador, this._id_unidad, this._id_proveedor,
                               this._fecha_liquidacion, this._total_salario, this._total_deducciones, this._total_sueldo, this._total_anticipos, this._total_comprobaciones,
                               this._total_descuentos, this._total_alcance, this._dias_pagados, this._bit_transferencia, this._id_transferencia, id_usuario, false);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Actualizar la Liquidación
        /// </summary>
        /// <returns></returns>
        public bool ActualizaLiquidacion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_liquidacion);
        }
        /// <summary>
        /// Método Público encargado de Obtener las Liquidaciones dada una Entidad especifica
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="tipo_asignacion">Tipo de Asignación</param>
        /// <param name="id_entidad">Entidad</param>
        /// <returns></returns>
        public static DataTable ObtieneLiquidacionesEntidad(int id_compania_emisora, TipoAsignacion tipo_asignacion, int id_entidad)
        {   //Declarando Objeto de Retorno
            DataTable dtLiquidaciones = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_compania_emisora, 0, 0, (byte)tipo_asignacion, id_entidad, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, true, null, "", "" };
            //Obteniendo Liquidaciones
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtLiquidaciones = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtLiquidaciones;
        }
        /// <summary>
        /// Método Público encargado de Obtener la Fecha de la Ultima Liquidacion dada una Entidad especifica
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="tipo_asignacion">Tipo de Asignación</param>
        /// <param name="id_entidad">Entidad</param>
        /// <returns></returns>
        public static DateTime ObtieneUltimaFechaLiquidacion(int id_compania_emisora, TipoAsignacion tipo_asignacion, int id_entidad)
        {   //Declarando Objeto de Retorno
            DateTime fecha_ultima_liq = DateTime.MinValue;
            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_compania_emisora, 0, 0, (byte)tipo_asignacion, id_entidad, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, true, null, "", "" };
            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        //Asignando Valor Obtenido
                        DateTime.TryParse(dr["FechaUltimaLiquidacion"].ToString(), out fecha_ultima_liq);
                }
            }
            //Devolviendo reusltado Obtenido
            return fecha_ultima_liq;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Montos Totales de la Liquidación 
        /// </summary>
        /// <param name="id_liquidacion">Liquidación</param>
        /// <param name="tipo_asignacion">Tipo de Asignación de la Liquidación</param>
        /// <param name="id_unidad">Unidad de la Liquidación</param>
        /// <param name="id_operador">Operador de la Liquidación</param>
        /// <param name="id_proveedor">Proveedor de la Liquidación</param>
        /// <param name="id_compania_emisora">Compania Emisora de la Liquidación</param>
        /// <returns></returns>
        public static DataTable ObtieneMontosTotalesLiquidacion(int id_liquidacion, TipoAsignacion tipo_asignacion, int id_unidad, int id_operador, int id_proveedor, int id_compania_emisora)
        {
            //Declarando Objeto de Retorno
            DataTable dtLiquidaciones = null;
            
            //Armando Arreglo de Parametros
            object[] param = { 7, id_liquidacion, id_compania_emisora, 0, 0, (byte)tipo_asignacion, id_operador, id_unidad, id_proveedor, null, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, true, null, "", "" };
            
            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Valor Obtenido
                    dtLiquidaciones = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtLiquidaciones;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Liquidaciones por Depositar
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneLiquidacionesPorDepositar(int id_compania_emisora)
        {
            //Declarando Objeto de Retorno
            DataTable dtLiquidaciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 8, 0, id_compania_emisora, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0, true, null, "", "" };

            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtLiquidaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtLiquidaciones;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Liquidaciones Depositadas
        /// </summary>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneLiquidacionesDepositadas(int id_operador, int id_unidad, int id_proveedor, int id_compania_emisora)
        {
            //Declarando Objeto de Retorno
            DataTable dtLiquidaciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 9, 0, id_compania_emisora, 0, 0, 0, id_operador, id_unidad, id_proveedor, null, 0, 0, 0, 0, 0, 0, 0, 0, 
                               false, 0, 0, true, null, "", "" };

            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtLiquidaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtLiquidaciones;
        }
        /// <summary>
        /// Obtiene información de la liquidación para exportación a Factura Electrónica
        /// </summary>
        /// <param name="id_liquidacion">Id Liquidación</param>
        /// <param name="id_metodo_pago">Método de Pafo</param>
        /// <returns></returns>
        public static DataSet ObtienesDatosLiquidacionFacturaElectronica(int id_liquidacion, int id_metodo_pago)
        {
            //Declarando Objeto de Retorno
            DataSet dsLiquidacion = null;

            //Armando Arreglo de Parametros
            object[] param = { 10, id_liquidacion, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 
                               false, 0, 0, true, null, id_metodo_pago, "" };

            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))

                    //Asignando Valor Obtenido
                    dsLiquidacion = ds;
            }

            //Devolviendo Resultado Obtenido
            return dsLiquidacion;
        }

        /// <summary>
        /// Carga Armado XML de Recibo de Nómina
        /// </summary>
        /// <param name="id_liquidacion"></param>
        /// <param name="periocidad_pago"></param>
        /// <returns></returns>
        public static DataSet CargaArmadoXMLReciboNomina(int id_liquidacion, string periocidad_pago)
        {
            //Declarando Objeto de Retorno
            DataSet dsLiquidacion = null;

            //Armando Arreglo de Parametros
            object[] param = { 11, id_liquidacion, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 
                               false, 0, 0, true, null, periocidad_pago, 0 };

            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))

                    //Asignando Valor Obtenido
                    dsLiquidacion = ds;
            }

            //Devolviendo Resultado Obtenido
            return dsLiquidacion;
        }
        
 
        public static DateTime ObtienePenultimaLiquidacion(int id_operador, int id_unidad, int id_proveedor, int id_tipo_entidad, int id_liquidacion)
        {
            //Declarando Objeto de Retorno
            DateTime fecha_penultima_liquidacion = DateTime.MinValue;

            //Armando Arreglo de Parametros
            object[] param = { 13, id_liquidacion, 0, 0, 0, id_tipo_entidad, id_operador, id_unidad, id_proveedor, null, 0, 0, 0, 0, 0, 0, 0, 0, 
                               false, 0, 0, true, null, "", 0 };

            //Obteniendo Liquidaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Asignando Fecha
                        DateTime.TryParse(dr["FechaLiquidacion"].ToString(), out fecha_penultima_liquidacion);
                }
            }

            //Devolviendo Resultado Obtenido
            return fecha_penultima_liquidacion;
        }


        #endregion
    }
}
