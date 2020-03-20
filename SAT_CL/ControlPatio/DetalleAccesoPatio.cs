using System;
using System.Data;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{
    /// <summary>
    /// Clase encargada de todas las Operaciones correspondientes a los Detalles de los Accesos a Patios
    /// </summary>
    public class DetalleAccesoPatio : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Estatus de Acceso en Tiempo Real de la Entidad
        /// </summary>
        public enum EstatusAcceso
        {   /// <summary>
            /// Expresa que la Entidad del Acceso esta Dentro del Patio
            /// </summary>
            Dentro = 1,
            /// <summary>
            /// Expresa que la Entidad del Acceso esta Fuera del Patio
            /// </summary>
            Fuera
        }
        /// <summary>
        /// Enumeración que expresa el Estatus de la Entidad en el Patio
        /// </summary>
        public enum EstatusPatio
        {   /// <summary>
            /// Expresa que el Estatus de la Entidad del Acceso esta sin Proceso Asignado
            /// </summary>
            SinAsignacion = 1,
            /// <summary>
            /// Expresa que el Estatus de la Entidad del Acceso esta en Proceso de Carga
            /// </summary>
            Carga,
            /// <summary>
            /// Expresa que el Estatus de la Entidad del Acceso esta en Proceso de Descarga
            /// </summary>
            Descarga,
            /// <summary>
            /// Expresa que el Estatus de la Entidad del Acceso esta en Proceso de Estacionado
            /// </summary>
            Estacionado
        }
        
        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_detalle_acceso_patio_tdap";

        private int _id_detalle_acceso_patio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Detalle de Acceso al Patio
        /// </summary>
        public int id_detalle_acceso_patio { get { return this._id_detalle_acceso_patio; } }
        private int _id_acceso_entrada;
        /// <summary>
        /// Atributo encargado de almacenar el Acceso de Entrada
        /// </summary>
        public int id_acceso_entrada { get { return this._id_acceso_entrada; } }
        private int _id_acceso_salida;
        /// <summary>
        /// Atributo encargado de almacenar  el Acceso de Salida
        /// </summary>
        public int id_acceso_salida { get { return this._id_acceso_salida; } }
        private int _id_transportista;
        /// <summary>
        /// Atributo encargado de almacenar el Transportista
        /// </summary>
        public int id_transportista { get { return this._id_transportista; } }
        private byte _id_estatus_acceso;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de Acceso
        /// </summary>
        public byte id_estatus_acceso { get { return this._id_estatus_acceso; } }
        private byte _id_estatus_patio;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus en Patio
        /// </summary>
        public byte id_estatus_patio { get { return this._id_estatus_patio; } }
        private int _id_entidad_cajon;
        /// <summary>
        /// Atributo encargado de almacenar el Cajon Actual
        /// </summary>
        public int id_entidad_cajon { get { return this._id_entidad_cajon; } }
        private int _id_entidad_anden;
        /// <summary>
        /// Atributo encargado de almacenar el Anden Actual
        /// </summary>
        public int id_entidad_anden { get { return this._id_entidad_anden; } }
        private bool _bit_cargado;
        /// <summary>
        /// Atributo encargado de almacenar el Indicador de Carga
        /// </summary>
        public bool bit_cargado { get { return this._bit_cargado; } }
        private DateTime _fecha_estatus_patio;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Actualización del Estatus en Patio
        /// </summary>
        public DateTime fecha_estatus_patio { get { return this._fecha_estatus_patio; } }
        private byte _id_tipo_detalle_acceso;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Detalle de Acceso 
        /// </summary>
        public byte id_tipo_detalle_acceso { get { return this._id_tipo_detalle_acceso; } }
        private string _descripcion_detalle_acceso;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción del Detalle de Acceso
        /// </summary>
        public string descripcion_detalle_acceso { get { return this._descripcion_detalle_acceso; } }
        private string _identificacion_detalle_acceso;
        /// <summary>
        /// Atributo encargado de almacenar la Identificación del Detalle de Acceso
        /// </summary>
        public string identificacion_detalle_acceso { get { return this._identificacion_detalle_acceso; } }
        private int _id_unidad_operador;
        /// <summary>
        /// Atributo encargado de almacenar la Referencia de la Entidad del Operador o la Unidad
        /// </summary>
        public int id_unidad_operador { get { return this._id_unidad_operador; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private byte[] _row_version;
        /// <summary>
        /// Atributo encargado de Almacenar la Versión del Registro
        /// </summary>
        public byte[] row_version { get { return this._row_version; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public DetalleAccesoPatio()
        {   //Asignando Valores
            this._id_detalle_acceso_patio = 0;
            this._id_acceso_entrada = 0;
            this._id_acceso_salida = 0;
            this._id_transportista = 0;
            this._id_estatus_acceso = 0;
            this._id_estatus_patio = 0;
            this._id_entidad_cajon = 0;
            this._id_entidad_anden = 0;
            this._bit_cargado = false;
            this._fecha_estatus_patio = DateTime.MinValue;
            this._id_tipo_detalle_acceso = 0;
            this._descripcion_detalle_acceso = "";
            this._identificacion_detalle_acceso = "";
            this._id_unidad_operador = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_detalle_acceso_patio">Id del Detalle de Acceso a Patio</param>
        public DetalleAccesoPatio(int id_detalle_acceso_patio)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_detalle_acceso_patio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~DetalleAccesoPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos 
        /// </summary>
        /// <param name="id_detalle_acceso_patio">Id de Detalle de Acceso a Patio</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_detalle_acceso_patio)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_detalle_acceso_patio, 0, 0, 0, 0, 0, 0, 0, false, null, 0, "", "", 0, 0, false, null, "", "" };
            //Instanciando Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_detalle_acceso_patio = id_detalle_acceso_patio;
                        this._id_acceso_entrada = Convert.ToInt32(dr["IdAccesoEntrada"]);
                        this._id_acceso_salida = Convert.ToInt32(dr["IdAccesoSalida"]);
                        this._id_transportista = Convert.ToInt32(dr["IdTransportista"]);
                        this._id_estatus_acceso = Convert.ToByte(dr["IdEstatusAcceso"]);
                        this._id_estatus_patio = Convert.ToByte(dr["IdEstatusPatio"]);
                        this._id_entidad_cajon = Convert.ToInt32(dr["IdEntidadCajon"]);
                        this._id_entidad_anden = Convert.ToInt32(dr["IdEntidadAnden"]);
                        this._bit_cargado = Convert.ToBoolean(dr["BitCargado"]);
                        DateTime.TryParse(dr["FechaEstatusPatio"].ToString(), out this._fecha_estatus_patio);
                        this._id_tipo_detalle_acceso = Convert.ToByte(dr["IdTipoDetalleAcceso"]);
                        this._descripcion_detalle_acceso = dr["DescripcionDetalleAcceso"].ToString();
                        this._identificacion_detalle_acceso = dr["IdentificacionDetalleAcceso"].ToString();
                        this._id_unidad_operador = Convert.ToInt32(dr["IdUnidadOperador"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._row_version = (byte[])dr["RowVersion"];
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Validar si el Registro se puede Modificar
        /// </summary>
        /// <returns></returns>
        private bool validaRegistro()
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 4, id_detalle_acceso_patio, 0, 0, 0, 0, 0, 0, 0, false, null, 0, "", "", 0, 0, false, this._row_version, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Validación en Positivo
                    result = true;
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_acceso_entrada">Acceso de Entrada</param>
        /// <param name="id_acceso_salida">Acceso de Salida</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="estatus_acceso">Estatus de Acceso</param>
        /// <param name="estatus_patio">Estatus en Patio</param>
        /// <param name="id_entidad_cajon">Cajon Actual</param>
        /// <param name="id_entidad_anden">Anden Actual</param>
        /// <param name="bit_cargado">Indicador de Carga de Unidad</param>
        /// <param name="fecha_estatus_patio">Fecha de Actualización de Estatus en Patio</param>
        /// <param name="id_tipo_detalle_acceso">Tipo de Detalle de Acceso</param>
        /// <param name="descripcion_detalle_acceso">Descripción de Detalle de Acceso</param>
        /// <param name="identificacion_detalle_acceso">Identificación de Detalle de Acceso</param>
        /// <param name="id_unidad_operador">Referencia de Unidad / Operador</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_acceso_entrada, int id_acceso_salida, int id_transportista, EstatusAcceso estatus_acceso, 
                                       EstatusPatio estatus_patio, int id_entidad_cajon, int id_entidad_anden, bool bit_cargado, DateTime fecha_estatus_patio, 
                                       byte id_tipo_detalle_acceso, string descripcion_detalle_acceso, string identificacion_detalle_acceso, int id_unidad_operador, 
                                       int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que el Registro se pueda Editar
            if (validaRegistro())
            {   //Armando Arreglo de Parametros
                object[] param = { 2, this._id_detalle_acceso_patio, id_acceso_entrada, id_acceso_salida, id_transportista, (byte)estatus_acceso, 
                               (byte)estatus_patio, id_entidad_cajon, id_entidad_anden, bit_cargado, fecha_estatus_patio, 
                               id_tipo_detalle_acceso, descripcion_detalle_acceso, identificacion_detalle_acceso, id_unidad_operador, 
                               id_usuario, habilitar, this._row_version, "", "" };
                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }
            else//Instanciando Excepcion
                result = new RetornoOperacion("El registro fue modificado en BD desde la última vez que fue consultado.");
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Detalles de Acceso a Patio
        /// </summary>
        /// <param name="id_acceso_entrada">Acceso de Entrada</param>
        /// <param name="id_acceso_salida">Acceso de Salida</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="id_entidad_cajon">Cajon Actual</param>
        /// <param name="id_entidad_anden">Anden Actual</param>
        /// <param name="bit_cargado">Indicador de Carga de Unidad</param>
        /// <param name="fecha_estatus_patio">Fecha de Actualización de Estatus en Patio</param>
        /// <param name="tipo_detalle_acceso">Tipo de Detalle de Acceso</param>
        /// <param name="descripcion_detalle_acceso">Descripción de Detalle de Acceso</param>
        /// <param name="identificacion_detalle_acceso">Identificación de Detalle de Acceso</param>
        /// <param name="id_unidad_operador">Referencia de Unidad / Operador</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDetalleAccesoPatio(int id_acceso_entrada, int id_acceso_salida, int id_transportista, 
                                       int id_entidad_cajon, int id_entidad_anden, bool bit_cargado, DateTime fecha_estatus_patio,
                                       byte tipo_detalle_acceso, string descripcion_detalle_acceso, string identificacion_detalle_acceso, int id_unidad_operador,
                                       int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_acceso_entrada, id_acceso_salida, id_transportista, (byte)EstatusAcceso.Dentro, 
                               (byte)EstatusPatio.SinAsignacion, id_entidad_cajon, id_entidad_anden, bit_cargado, fecha_estatus_patio, 
                               (byte)tipo_detalle_acceso, descripcion_detalle_acceso, identificacion_detalle_acceso, id_unidad_operador, 
                               id_usuario, true, null, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Detalles de Acceso a Patio
        /// </summary>
        /// <param name="id_acceso_entrada">Acceso de Entrada</param>
        /// <param name="id_acceso_salida">Acceso de Salida</param>
        /// <param name="id_transportista">Transportista</param>
        /// <param name="estatus_acceso">Estatus de Acceso</param>
        /// <param name="estatus_patio">Estatus en Patio</param>
        /// <param name="id_entidad_cajon">Cajon Actual</param>
        /// <param name="id_entidad_anden">Anden Actual</param>
        /// <param name="bit_cargado">Indicador de Carga de Unidad</param>
        /// <param name="fecha_estatus_patio">Fecha de Actualización de Estatus en Patio</param>
        /// <param name="id_tipo_detalle_acceso">Tipo de Detalle de Acceso</param>
        /// <param name="descripcion_detalle_acceso">Descripción de Detalle de Acceso</param>
        /// <param name="identificacion_detalle_acceso">Identificación de Detalle de Acceso</param>
        /// <param name="id_unidad_operador">Referencia de Unidad / Operador</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaDetalleAccesoPatio(int id_acceso_entrada, int id_acceso_salida, int id_transportista, EstatusAcceso estatus_acceso,
                                       EstatusPatio estatus_patio, int id_entidad_cajon, int id_entidad_anden, bool bit_cargado, DateTime fecha_estatus_patio,
                                       byte id_tipo_detalle_acceso, string descripcion_detalle_acceso, string identificacion_detalle_acceso, int id_unidad_operador,
                                       int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_acceso_entrada, id_acceso_salida, id_transportista, estatus_acceso, 
                               estatus_patio, id_entidad_cajon, id_entidad_anden, bit_cargado, fecha_estatus_patio,
                               id_tipo_detalle_acceso, descripcion_detalle_acceso, identificacion_detalle_acceso, id_unidad_operador,
                               id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Dar Salida al Detalle del Operador o Unidad
        /// </summary>
        /// <param name="id_acceso_salida">Acceso de Salida</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaSalidaDetalle(int id_acceso_salida, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Acceso de Salida
                using (AccesoPatio ap = new AccesoPatio(id_acceso_salida))
                {   //Validando que exista el Acceso de Patio de Salida
                    if (ap.id_acceso_patio > 0)
                    {
                        //Obteniendo Fecha Maxima de los Eventos del Detalle
                        DateTime fec_max = ObtieneFechaMaximaEvento(this._id_detalle_acceso_patio);

                        //Validando que la fecha de Salida del Acceso sea Superior a la del Evento
                        if (DateTime.Compare(ap.fecha_acceso, fec_max) >= 0)
                        {
                            //Invocando Método de Actualización
                            result = this.actualizaRegistros(this._id_acceso_entrada, id_acceso_salida, this._id_transportista, EstatusAcceso.Fuera,
                                               EstatusPatio.SinAsignacion, 0, 0, this._bit_cargado, ap.fecha_acceso,
                                               this._id_tipo_detalle_acceso, this._descripcion_detalle_acceso, this._identificacion_detalle_acceso, this._id_unidad_operador,
                                               id_usuario, this._habilitar);
                            //Validando que la Operación fuese exitosa
                            if (result.OperacionExitosa)
                            {   //Obteniendo Eventos
                                using (DataTable dtEventos = EventoDetalleAcceso.ObtieneEventosDetalle(this._id_detalle_acceso_patio))
                                {   //Validando que existen Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEventos))
                                    {
                                        //Recorriendo Filas
                                        foreach (DataRow dr in dtEventos.Rows)
                                        {   //Instanciando Evento
                                            using (EventoDetalleAcceso eda = new EventoDetalleAcceso(Convert.ToInt32(dr["Id"])))
                                            {   //Validando que exista el Evento
                                                if (eda.id_evento_detalle_acceso > 0)
                                                    //Terminando Evento
                                                    result = eda.TerminaEventoDetalle(ap.fecha_acceso, EventoDetalleAcceso.TipoActualizacion.Web, id_usuario);
                                                else//Instanciando Excepcion
                                                    result = new RetornoOperacion("No se puede Terminar el Evento");
                                                //Validando que haya terminado el Evento
                                                if (result.OperacionExitosa)
                                                {   //Instanciando Entidad de Patio
                                                    using (EntidadPatio ep = new EntidadPatio(eda.id_entidad_patio))
                                                    {   //Validando que Exista la 
                                                        if (ep.id_entidad_patio > 0)
                                                            //Actualiza Evento Actual
                                                            result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Vacio, EntidadPatio.EstatusCarga.Ninguno,
                                                                        ap.fecha_acceso, 0, id_usuario);
                                                        else//Instanciando Excepcion
                                                            result = new RetornoOperacion("No se puede Actualizar la Entidad");
                                                        //Si la Operación no fue Exitosa
                                                        if (!result.OperacionExitosa)
                                                            //Finalizando Ciclo
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Validando que la Operación sea Exitosa
                                    if (result.OperacionExitosa)
                                        //Completando Transacción
                                        trans.Complete();
                                }
                            }
                        }
                        else
                            //Instanciando Excepcion
                            result = new RetornoOperacion("La Fecha de Salida debe ser superior a la del Ultimo Evento");
                    }
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("No existe el acceso de Salida");
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_acceso_salida"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaSalidaDetalleMobil(int id_acceso_salida, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {   
                //Instanciando Acceso de Salida
                using (AccesoPatio ap = new AccesoPatio(id_acceso_salida))
                {   //Validando que exista el Acceso de Patio de Salida
                    if (ap.id_acceso_patio > 0)
                    {
                        //Obteniendo Fecha Maxima de los Eventos del Detalle
                        DateTime fec_max = ObtieneFechaMaximaEvento(this._id_detalle_acceso_patio);

                        //Validando que la fecha de Salida del Acceso sea Superior a la del Evento
                        if (DateTime.Compare(ap.fecha_acceso, fec_max) >= 0)
                        {
                            //Invocando Método de Actualización
                            result = this.actualizaRegistros(this._id_acceso_entrada, id_acceso_salida, this._id_transportista, EstatusAcceso.Fuera,
                                               EstatusPatio.SinAsignacion, 0, 0, this._bit_cargado, ap.fecha_acceso,
                                               this._id_tipo_detalle_acceso, this._descripcion_detalle_acceso, this._identificacion_detalle_acceso, this._id_unidad_operador,
                                               id_usuario, this._habilitar);
                            //Validando que la Operación fuese exitosa
                            if (result.OperacionExitosa)
                            {   //Obteniendo Eventos
                                using (DataTable dtEventos = EventoDetalleAcceso.ObtieneEventosDetalle(this._id_detalle_acceso_patio))
                                {   //Validando que existen Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEventos))
                                    {
                                        //Recorriendo Filas
                                        foreach (DataRow dr in dtEventos.Rows)
                                        {   //Instanciando Evento
                                            using (EventoDetalleAcceso eda = new EventoDetalleAcceso(Convert.ToInt32(dr["Id"])))
                                            {   //Validando que exista el Evento
                                                if (eda.id_evento_detalle_acceso > 0)
                                                    //Terminando Evento
                                                    result = eda.TerminaEventoDetalle(ap.fecha_acceso, EventoDetalleAcceso.TipoActualizacion.Mobil, id_usuario);
                                                else//Instanciando Excepcion
                                                    result = new RetornoOperacion("No se puede Terminar el Evento");
                                                //Validando que haya terminado el Evento
                                                if (result.OperacionExitosa)
                                                {   //Instanciando Entidad de Patio
                                                    using (EntidadPatio ep = new EntidadPatio(eda.id_entidad_patio))
                                                    {   //Validando que Exista la 
                                                        if (ep.id_entidad_patio > 0)
                                                            //Actualiza Evento Actual
                                                            result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Vacio, EntidadPatio.EstatusCarga.Ninguno,
                                                                        ap.fecha_acceso, 0, id_usuario);
                                                        else//Instanciando Excepcion
                                                            result = new RetornoOperacion("No se puede Actualizar la Entidad");
                                                        //Si la Operación no fue Exitosa
                                                        if (!result.OperacionExitosa)
                                                            //Finalizando Ciclo
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Validando que la Operación sea Exitosa
                                    if (result.OperacionExitosa)
                                        //Completando Transacción
                                        trans.Complete();
                                }
                            }
                        }
                        else
                            //Instanciando Excepcion
                            result = new RetornoOperacion("La Fecha de Salida debe ser superior a la del Ultimo Evento");
                    } 
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("No existe el acceso de Salida");
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Detalles de Acceso a Patio
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDetalleAccesoPatio(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_acceso_entrada, this._id_acceso_salida, this._id_transportista, (EstatusAcceso)this._id_estatus_acceso,
                               (EstatusPatio)this._id_estatus_patio, this._id_entidad_cajon, this._id_entidad_anden, this._bit_cargado, this._fecha_estatus_patio,
                               this._id_tipo_detalle_acceso, this._descripcion_detalle_acceso, this._identificacion_detalle_acceso, this._id_unidad_operador,
                               id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Anden actual y su Carga
        /// </summary>
        /// <param name="id_entidad_anden">Anden Actual</param>
        /// <param name="bit_cargado">Indicador de Carga</param>
        /// <param name="estatus_patio">Estatus en Patio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaAndenCargaDetalle(int id_entidad_anden, bool bit_cargado, EstatusPatio estatus_patio, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_acceso_entrada, this._id_acceso_salida, this._id_transportista, (EstatusAcceso)this._id_estatus_acceso,
                               estatus_patio, 0, id_entidad_anden, bit_cargado, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                               this._id_tipo_detalle_acceso, this._descripcion_detalle_acceso, this._identificacion_detalle_acceso, this._id_unidad_operador,
                               id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Cajon actual y su Carga
        /// </summary>
        /// <param name="id_entidad_cajon">Cajon Actual</param>
        /// <param name="bit_cargado">Indicador de Carga</param>
        /// <param name="estatus_patio">Estatus en Patio</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCajonCargaDetalle(int id_entidad_cajon, bool bit_cargado, EstatusPatio estatus_patio, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_acceso_entrada, this._id_acceso_salida, this._id_transportista, (EstatusAcceso)this._id_estatus_acceso,
                               estatus_patio, id_entidad_cajon, 0, bit_cargado, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                               this._id_tipo_detalle_acceso, this._descripcion_detalle_acceso, this._identificacion_detalle_acceso, this._id_unidad_operador,
                               id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de los Detalles de Acceso a Patio
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDetalleAccesoPatio()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_detalle_acceso_patio);
        }

        /// <summary>
        /// Metodo que regresa los indicadores relacionados con las unidades en patio
        /// </summary>
        /// <param name="id_patio"></param>
        /// <returns></returns>
        public static DataTable retornaIndicadoresUnidades(int id_patio)
        {
            //Inicializamos los parametros de entrada del store procedure
            object[] parametros = { 5, 0, 0, 0, 0, (byte)1,(byte)1, 0, 0, 0, null,(byte)1, "", "", 0,0, true, null, id_patio, "" };
            //Ejecutamos el procedimiento almacenado
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros).Tables[0])
                return mit;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades ligadas al Acceso
        /// </summary>
        /// <param name="id_acceso_entrada"></param>
        /// <param name="id_detalle_acceso"></param>
        /// <returns></returns>
        public static DataTable ObtieneUnidadesAcceso(int id_acceso_entrada, int id_detalle_acceso)
        {  //Declarando Objeto de Retorno
            DataTable dtUnidadesAcceso = null;
            //Armando Arreglo de Parametros
            object[] param = { 6, id_detalle_acceso, id_acceso_entrada, 0, 0, 0, 0, 0, 0, false, null, 
                               0, "", "", 0, 0, false, null, "", "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtUnidadesAcceso = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtUnidadesAcceso;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades ligadas al Acceso
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneUnidadesEstatus(int id_ubicacion_patio)
        {  //Declarando Objeto de Retorno
            DataTable dtUnidadesAcceso = null;
            //Armando Arreglo de Parametros
            object[] param = { 7, 0, 0, 0, 0, 0, 0, 0, 0, false, null, 
                               0, "", "", 0, 0, false, null, id_ubicacion_patio.ToString(), "" };
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtUnidadesAcceso = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtUnidadesAcceso;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades en un Periodo de Tiempo
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public static DataSet ObtienePeriodoUnidades(int id_ubicacion_patio)
        {  
            //Armando Arreglo de Parametros
            object[] param = { 8, 0, 0, 0, 0, 0, 0, 0, 0, false, null, 
                               0, "", "", 0, 0, false, null, id_ubicacion_patio.ToString(), "" };
            //Instanciando Registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param);           
        }
        /// <summary>
        /// Método Público encargado de las Unidades por su Rango de Estancia
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneUnidadesRangoEstancia(int id_ubicacion_patio)
        {   
            //Declarando Objeto de Retorno
            DataTable dtUnidades = null;
            
            //Armando Arreglo de Parametros
            object[] param = { 9, 0, 0, 0, 0, 0, 0, 0, 0, false, null, 0, "", "", 0, 0, false, null, id_ubicacion_patio.ToString(), "" };
            
            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Resultado Obtenido
                    dtUnidades = ds.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades por su Estatus de Acceso
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public static DataTable ObtieneEntradasSalidas(int id_ubicacion_patio)
        {
            //Declarando Objeto de Retorno
            DataTable dtUnidades = null;

            //Armando Arreglo de Parametros
            object[] param = { 10, 0, 0, 0, 0, 0, 0, 0, 0, false, null, 0, "", "", 0, 0, false, null, id_ubicacion_patio.ToString(), "" };

            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtUnidades = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método encargado de Obtener las Entradas y Salidas por Hora
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public static DataSet ObtieneESHora(int id_ubicacion_patio)
        {
            //Armando Arreglo de Parametros
            object[] param = { 11, 0, 0, 0, 0, 0, 0, 0, 0, false, null, 
                               0, "", "", 0, 0, false, null, id_ubicacion_patio.ToString(), "" };
            //Instanciando Registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param);
        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades que tienen un Evento Pendiente
        /// </summary>
        /// <param name="id_ubicacion_patio"></param>
        /// <returns></returns>
        public static DataTable ObtieneUnidadesEventoPendientes(int id_ubicacion_patio)
        {
            //Declarando Objeto de Retorno
            DataTable dtUnidades = null;

            //Armando Arreglo de Parametros
            object[] param = { 12, 0, 0, 0, 0, 0, 0, 0, 0, false, null, 0, "", "", 0, 0, false, null, id_ubicacion_patio.ToString(), "" };

            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtUnidades = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades que tienen Evidencias
        /// </summary>
        /// <param name="id_detalle_acceso"></param>
        /// <returns></returns>
        public static DataTable ObtieneImagenesUnidades(int id_detalle_acceso)
        {
            //Declarando Objeto de Retorno
            DataTable dtUnidades = null;

            //Armando Arreglo de Parametros
            object[] param = { 13, id_detalle_acceso, 0, 0, 0, 0, 0, 0, 0, false, null, 0, "", "", 0, 0, false, null, "", "" };

            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtUnidades = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtUnidades;
        }
        /// <summary>
        /// Método Público encargado de Obtener la Fecha Maxima del Evento de un Detalle
        /// </summary>
        /// <param name="id_detalle_acceso">Detalle de Acceso</param>
        /// <returns></returns>
        public static DateTime ObtieneFechaMaximaEvento(int id_detalle_acceso)
        {
            //Declarando Objeto de Retorno
            DateTime fecha_max = DateTime.MinValue;

            //Armando Arreglo de Parametros
            object[] param = { 14, id_detalle_acceso, 0, 0, 0, 0, 0, 0, 0, false, null, 0, "", "", 0, 0, false, null, "", "" };

            //Instanciando Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Asignando Resultado Obtenido
                        DateTime.TryParse(dr["FechaMaxima"].ToString(), out fecha_max);
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return fecha_max;
        }


        #endregion
    }
}
