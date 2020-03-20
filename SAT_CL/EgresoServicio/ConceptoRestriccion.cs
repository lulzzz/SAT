using SAT_CL.Global;
using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.EgresoServicio
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones de las Restricciones de los Conceptos
    /// </summary>
    public class ConceptoRestriccion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "egresos_servicio.sp_concepto_restriccion_tcr";

        private int _id_concepto_restriccion;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Restricción del Concepto
        /// </summary>
        public int id_concepto_restriccion { get { return this._id_concepto_restriccion; } }
        private int _id_concepto;
        /// <summary>
        /// Atributo encargado de Almacenar el Concepto
        /// </summary>
        public int id_concepto { get { return this._id_concepto; } }
        private int _id_compania_receptor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Receptora
        /// </summary>
        public int id_compania_receptor { get { return this._id_compania_receptor; } }
        private decimal _minimo_monto;
        /// <summary>
        /// Atributo encargado de Almacenar valor Minimo del Monto
        /// </summary>
        public decimal minimo_monto { get { return this._minimo_monto; } }
        private decimal _maximo_monto;
        /// <summary>
        /// Atributo encargado de Almacenar  valor Maximo del Monto
        /// </summary>
        public decimal maximo_monto { get { return this._maximo_monto; } }
        private byte _incidencia_servicio;
        /// <summary>
        /// Atributo encargado de Almacenar el Limite de Incidencia por Montos
        /// </summary>
        public byte incidencia_servicio { get { return this._incidencia_servicio; } }
        private byte _incidencia_movimiento;
        /// <summary>
        /// Atributo encargado de Almacenar el Limite de Incidencia por Movimientos
        /// </summary>
        public byte incidencia_movimiento { get { return this._incidencia_movimiento; } }
        private byte _id_tipo_calculo;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Calculo
        /// </summary>
        public byte id_tipo_calculo { get { return this._id_tipo_calculo; } }
        private int _id_periodo;
        /// <summary>
        /// Atributo encargado de Almacenar el Periodo
        /// </summary>
        public int id_periodo { get { return this._id_periodo; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private TimeSpan _hora_inicio;
        /// <summary>
        /// Atributo encargado de Almacenar la Hora de Inicio
        /// </summary>
        public TimeSpan hora_inicio { get { return this._hora_inicio; } }
        private TimeSpan _hora_fin;
        /// <summary>
        /// Atributo encargado de Almacenar la Hora de Fin
        /// </summary>
        public TimeSpan hora_fin { get { return this._hora_fin; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que Inicializa los Atributos por Defecto
        /// </summary>
        public ConceptoRestriccion()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor que Inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Registro</param>
        public ConceptoRestriccion(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ConceptoRestriccion()
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
            this._id_concepto_restriccion = 0;
            this._id_concepto = 0;
            this._id_compania_receptor = 0;
            this._minimo_monto = 0;
            this._maximo_monto = 0;
            this._incidencia_servicio = 0;
            this._incidencia_movimiento = 0;
            this._id_tipo_calculo = 0;
            this._id_periodo = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._hora_inicio = TimeSpan.MinValue;
            this._hora_fin = TimeSpan.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, 0, 0, 0, null, null, null, null, 0, true, "", "" };
            //Instanciando Objeto de parametros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_concepto_restriccion = id_registro;
                        this._id_concepto = Convert.ToInt32(dr["IdConcepto"]);
                        this._id_compania_receptor = Convert.ToInt32(dr["IdCompaniaReceptor"]);
                        this._minimo_monto = Convert.ToDecimal(dr["MinimoMonto"]);
                        this._maximo_monto = Convert.ToDecimal(dr["MaximoMonto"]);
                        this._incidencia_servicio = Convert.ToByte(dr["IncidenciaServicio"]);
                        this._incidencia_movimiento = Convert.ToByte(dr["IncidenciaMovimiento"]);
                        this._id_tipo_calculo = Convert.ToByte(dr["IdTipoCalculo"]);
                        this._id_periodo = Convert.ToInt32(dr["IdPeriodo"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        TimeSpan.TryParse(dr["HoraInicio"].ToString(), out this._hora_inicio);
                        TimeSpan.TryParse(dr["HoraFin"].ToString(), out this._hora_fin);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando resultado Positivo
                    result = true;
                }
            }
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_concepto">Concepto Ligado a la Restricción</param>
        /// <param name="id_compania_receptor">Compania Receptora</param>
        /// <param name="minimo_monto">Cantidad Minima del Monto</param>
        /// <param name="maximo_monto">Cantidad Maxima del Monto</param>
        /// <param name="incidencia_servicio">Limite de Incidencias por Servicio</param>
        /// <param name="incidencia_movimiento">Limite de Incidencias por Movimiento</param>
        /// <param name="id_tipo_calculo">Tipo de Calculo</param>
        /// <param name="id_periodo">Periodo</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="hora_inicio">Hora de Inicio</param>
        /// <param name="hora_fin">Hora de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_concepto, int id_compania_receptor, decimal minimo_monto, decimal maximo_monto, 
                                               byte incidencia_servicio, byte incidencia_movimiento, byte id_tipo_calculo, int id_periodo, DateTime fecha_inicio, 
                                               DateTime fecha_fin, TimeSpan hora_inicio, TimeSpan hora_fin, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_concepto_restriccion, id_concepto, id_compania_receptor, minimo_monto, maximo_monto, 
                               incidencia_servicio, incidencia_movimiento, id_tipo_calculo, id_periodo, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), TSDK.Base.Fecha.ConvierteTimeSpanObjeto(hora_inicio), 
                               TSDK.Base.Fecha.ConvierteTimeSpanObjeto(hora_fin), id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Restricciones de los Conceptos
        /// </summary>
        /// <param name="id_concepto">Concepto Ligado a la Restricción</param>
        /// <param name="id_compania_receptor">Compania Receptora</param>
        /// <param name="minimo_monto">Cantidad Minima del Monto</param>
        /// <param name="maximo_monto">Cantidad Maxima del Monto</param>
        /// <param name="incidencia_servicio">Limite de Incidencias por Servicio</param>
        /// <param name="incidencia_movimiento">Limite de Incidencias por Movimiento</param>
        /// <param name="id_tipo_calculo">Tipo de Calculo</param>
        /// <param name="id_periodo">Periodo</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="hora_inicio">Hora de Inicio</param>
        /// <param name="hora_fin">Hora de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaConceptoRestriccion(int id_concepto, int id_compania_receptor, decimal minimo_monto, decimal maximo_monto,
                                               byte incidencia_servicio, byte incidencia_movimiento, byte id_tipo_calculo, int id_periodo, DateTime fecha_inicio,
                                               DateTime fecha_fin, TimeSpan hora_inicio, TimeSpan hora_fin, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_concepto, id_compania_receptor, minimo_monto, maximo_monto, incidencia_servicio, incidencia_movimiento, 
                               id_tipo_calculo, id_periodo, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               TSDK.Base.Fecha.ConvierteTimeSpanObjeto(hora_inicio), TSDK.Base.Fecha.ConvierteTimeSpanObjeto(hora_fin), id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Restricciones de los Conceptos
        /// </summary>
        /// <param name="id_concepto">Concepto Ligado a la Restricción</param>
        /// <param name="id_compania_receptor">Compania Receptora</param>
        /// <param name="minimo_monto">Cantidad Minima del Monto</param>
        /// <param name="maximo_monto">Cantidad Maxima del Monto</param>
        /// <param name="incidencia_servicio">Limite de Incidencias por Servicio</param>
        /// <param name="incidencia_movimiento">Limite de Incidencias por Movimiento</param>
        /// <param name="id_tipo_calculo">Tipo de Calculo</param>
        /// <param name="id_periodo">Periodo</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="hora_inicio">Hora de Inicio</param>
        /// <param name="hora_fin">Hora de Fin</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaConceptoRestriccion(int id_concepto, int id_compania_receptor, decimal minimo_monto, decimal maximo_monto,
                                               byte incidencia_servicio, byte incidencia_movimiento, byte id_tipo_calculo, int id_periodo, DateTime fecha_inicio,
                                               DateTime fecha_fin, TimeSpan hora_inicio, TimeSpan hora_fin, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_concepto, id_compania_receptor, minimo_monto, maximo_monto,
                               incidencia_servicio, incidencia_movimiento, id_tipo_calculo, id_periodo, fecha_inicio, fecha_fin,
                               hora_inicio, hora_fin, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Restricciones de los Conceptos
        /// </summary>
        /// <param name="id_usuario">id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaConceptoRestriccion(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_concepto, this._id_compania_receptor, this._minimo_monto, this._maximo_monto,
                               this._incidencia_servicio, this._incidencia_movimiento, this._id_tipo_calculo, this._id_periodo, this._fecha_inicio, this._fecha_fin,
                               this._hora_inicio, this._hora_fin, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar las Restricciones de los Conceptos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaConceptoRestriccion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_concepto_restriccion);
        }

        /// <summary>
        /// Obtiene el Id de Concepto Restriccion coincidente con la clasificación del Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_concepto">Id Concepto</param>
        /// <returns></returns>
        public static int ObtieneConceptoRestriccionCoincidenteClasificacion(int id_servicio, int id_concepto)
        {
            //Asignando parámetros de salida
           int id_concepto_restriccion = 0;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_concepto, 0, 0, 0, 0, 0, 0, 0, null, null, null, null, 0, true, id_servicio, "" };

            //Realziando la consulta de conteos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obteniendo el Id Concepto Restriccion
                    id_concepto_restriccion = ds.Tables["Table"].Rows[0].Field<int>("IdConceptoRestriccion");
                }
            }
            //Devolvemos Resultado 
            return id_concepto_restriccion;
        }

        /// <summary>
        /// Verifica el periodo de validéz de un concepto y determina si es posible solicitar un depósito en un fecha determinada.
        /// </summary>
        /// <param name="fecha_solicitud">Fecha de solicitud del depósito</param>
        /// <returns></returns>
        public RetornoOperacion ValidaFechaConceptoRestriccion(DateTime fecha_solicitud)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
             
               //Validamos Rango de Fechas
            resultado = Fecha.ValidaFechaEnRango(this._fecha_inicio, fecha_solicitud.Date, this._fecha_fin, "El depósito '{1}' solo esta diponible a partir de la fecha'{0}' y '{2}'");

                //Validamos Resultado
               if (resultado.OperacionExitosa)
               {
                   //Validamos Rango de Hora
                   resultado = Fecha.ValidaTimeSpanEnRango(this._hora_inicio, fecha_solicitud.TimeOfDay, this._hora_fin, "El depósito '{1}' solo esta diponible a las '{0}' y '{2}'");

                   //Instanciamos Periodo
                   using (Periodo objPeriodo = new Periodo(this._id_periodo))
                   {
                       //Validamos Resultado
                       if (resultado.OperacionExitosa)
                       {
                           if (objPeriodo.id_periodo > 0)
                           {
                               //Validamos Dia
                               resultado = objPeriodo.ValidaDiaEnPeriodo(fecha_solicitud.DayOfWeek);
                           }
                       }
                   }
               }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene el Id de Concepto Restriccion coincidente con un Id Concepto
        /// </summary>
        /// <param name="id_concepto">Id Concepto</param>
        /// <returns></returns>
        public static int ObtieneConceptoRestriccionConcepto( int id_concepto)
        {
            //Asignando parámetros de salida
            int id_concepto_restriccion = 0;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_concepto, 0, 0, 0, 0, 0, 0, 0, null, null, null, null, 0, true, "", "" };

            //Realziando la consulta de conteos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obteniendo el Id Concepto Restriccion
                    id_concepto_restriccion = ds.Tables["Table"].Rows[0].Field<int>("Id");
                }
            }
            //Devolvemos Resultado 
            return id_concepto_restriccion;
        }

        /// <summary>
        /// Obtiene los Concepto Restriccion ligando una Compañia
        /// </summary>
        /// <param name="id_compania">Id Compañia</param>
        /// <returns></returns>
        public static DataTable  ObtieneConceptoRestriccion(int id_compania)
        {
            //Asignando parámetros de salida
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, null, null, 0, true, id_compania, "" };

            //Realziando la consulta de conteos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obteniendo el Id Concepto Restriccion
                    mit = ds.Tables["Table"];
                }
            }
            //Devolvemos Resultado 
            return mit;
        }
        #endregion
    }
}
