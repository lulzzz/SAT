using System;
using System.Collections.Generic;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Despacho;
using SAT_CL.Global;
using System.Transactions;
using System.Linq;
using System.Xml.Linq;
using SAT_CL.Facturacion;
using SAT_CL.ControlEvidencia;

namespace SAT_CL.Documentacion
{
    /// <summary>
    /// Proporciona los medios para la adminsitración de Servicios
    /// </summary>
    public partial class Servicio : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los posibles estatus existentes para una entidad Servicio
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// El servicio se ha registrado, pero no se le ha asignado recursos
            /// </summary>
            Documentado = 1,
            /// <summary>
            /// El servicio se ha iniciado
            /// </summary>
            Iniciado = 2,
            /// <summary>
            /// El servicio se ha terminado
            /// </summary>
            Terminado = 3,
            /// <summary>
            /// El servicio se ha cancelado
            /// </summary>
            Cancelado = 4
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure de la clase
        /// </summary>
        private static string _nombre_stored_procedure = "documentacion.sp_servicio_ts";

        private int _id_servicio;
        /// <summary>
        /// Obtiene el Id de Servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private string _prefijo_servicio;
        /// <summary>
        /// Obtiene el prefijo del servicio
        /// </summary>
        public string prefijo_servicio { get { return this._prefijo_servicio; } }
        private int _consecutivo_compania_emisor;
        /// <summary>
        /// Obtiene el número consecutivo del servicio para su compañía
        /// </summary>
        public int consecutivo_compania_emisor { get { return this._consecutivo_compania_emisor; } }
        private string _no_servicio;
        /// <summary>
        /// Obtiene el Número de Servicio asignado (prefijo_servicio + consecutivo_compania_emisor)
        /// </summary>
        public string no_servicio { get { return this._no_servicio; } }
        private int _id_origen_datos;
        /// <summary>
        /// Obtiene el Id de Origen de datos del Servicio (de dónde proviene)
        /// </summary>
        public int id_origen_datos { get { return this._id_origen_datos; } }
        private int _id_servicio_base;
        /// <summary>
        /// Obtiene el Id de Servicio Base del que fue obtenido este servicio
        /// </summary>
        public int id_servicio_base { get { return this._id_servicio_base; } }
        private bool _bit_servicio_base;
        /// <summary>
        /// Indica si el servicio fue obtenido desde un servicio base
        /// </summary>
        public bool bit_servicio_base { get { return this._bit_servicio_base; } }
        private int _id_cotizacion;
        /// <summary>
        /// Obtiene el Id de Cotización
        /// </summary>
        public int id_cotizacion { get { return this._id_cotizacion; } }
        private byte _id_estatus;
        /// <summary>
        /// Obtiene el Estatus actual del servicio
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Obtiene el Id de Compañía Emisora
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }
        private int _id_cliente_receptor;
        /// <summary>
        /// Obtiene el Id de Cliente Receptor del Servicio
        /// </summary>
        public int id_cliente_receptor { get { return this._id_cliente_receptor; } }
        private int _id_ubicacion_carga;
        /// <summary>
        /// Id de Ubicación de Carga
        /// </summary>
        public int id_ubicacion_carga { get { return this._id_ubicacion_carga; } }
        private DateTime _cita_carga;
        /// <summary>
        /// Obtiene la fecha de la cita de carga
        /// </summary>
        public DateTime cita_carga { get { return this._cita_carga; } }
        private int _id_ubicacion_descarga;
        /// <summary>
        /// Obtiene el Id de la Ubicación de descarga
        /// </summary>
        public int id_ubicacion_descarga { get { return this._id_ubicacion_descarga; } }
        private DateTime _cita_descarga;
        /// <summary>
        /// Obtiene la fecha de la cita de descarga
        /// </summary>
        public DateTime cita_descarga { get { return this._cita_descarga; } }
        private string _porte;
        /// <summary>
        /// Obtiene el Número de Carta Porte
        /// </summary>
        public string porte { get { return this._porte; } }
        private string _referencia_cliente;
        /// <summary>
        /// Obtiene el Número de referencia de servicio utilizado por el cliente
        /// </summary>
        public string referencia_cliente { get { return this._referencia_cliente; } }
        private DateTime _fecha_documentacion;
        /// <summary>
        /// Obtiene la fecha de documentación del servicio
        /// </summary>
        public DateTime fecha_documentacion { get { return this._fecha_documentacion; } }
        private string _observacion_servicio;
        /// <summary>
        /// Obtiene la observación general sobre el servicio
        /// </summary>
        public string observacion_servicio { get { return this._observacion_servicio; } }
        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del servicio
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private byte[] _row_version;
        /// <summary>
        /// Obtiene la versión del registro en este objeto
        /// </summary>
        public byte[] row_version { get { return this._row_version; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una instancia con valores predeterminados en atributos
        /// </summary>
        protected Servicio()
        {
        }
        /// <summary>
        ///  Inicializa una instancia con valores de atributos definidos por el Id de registro solicitado
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        public Servicio(int id_servicio)
        {
            cargarAtributosInstancia(id_servicio);
        }
        /// <summary>
        ///  Inicializa una instancia con valores de atributos definidos por el Número de servicio y Compañía solicitados
        /// </summary>
        /// <param name="no_servicio">Número de servicio</param>
        /// <param name="id_compania_emisor">Id de Compañía emisora</param>
        public Servicio(string no_servicio, int id_compania_emisor)
        {
            cargarAtributosInstancia(no_servicio, id_compania_emisor);
        }
        /// <summary>
        ///  Inicializa una instancia con valores de atributos definidos por el Número de servicio y Compañía solicitados
        /// </summary>
        /// <param name="no_servicio">Número de servicio</param>
        /// <param name="id_compania_emisor">Id de Compañía emisora</param>
        public Servicio(string no_servicio, string no_viaje, int id_compania_emisor)
        {
            cargarAtributosInstancia(no_servicio, no_viaje, id_compania_emisor);
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Servicio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la carga de los atributos de la instancia
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        private bool cargarAtributosInstancia(int id_servicio)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Declarando arreglo de parámetros para consulta en BD
            object[] param = { 3, id_servicio, "", 0, "", 0, 0, false, 0, 0, 0, 0, 0, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando entre resultados devueltos
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores de atributos
                        this._id_servicio = Convert.ToInt32(r["Id"]);
                        this._prefijo_servicio = r["Prefijo"].ToString();
                        this._consecutivo_compania_emisor = Convert.ToInt32(r["Consecutivo"]);
                        this._no_servicio = r["NoServicio"].ToString();
                        this._id_origen_datos = Convert.ToInt32(r["IdOrigenDatos"]);
                        this._id_servicio_base = Convert.ToInt32(r["IdServicioBase"]);
                        this._bit_servicio_base = Convert.ToBoolean(r["BitServicioBase"]);
                        this._id_cotizacion = Convert.ToInt32(r["IdCotizacion"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._id_cliente_receptor = Convert.ToInt32(r["IdClienteReceptor"]);
                        this._id_ubicacion_carga = Convert.ToInt32(r["IdUbicacionCarga"]);
                        this._cita_carga = Convert.ToDateTime(r["CitaCarga"]);
                        this._id_ubicacion_descarga = Convert.ToInt32(r["IdUbicacionDescarga"]);
                        this._cita_descarga = Convert.ToDateTime(r["CitaDescarga"]);
                        this._porte = r["Porte"].ToString();
                        this._referencia_cliente = r["ReferenciaCliente"].ToString();
                        this._fecha_documentacion = Convert.ToDateTime(r["FechaDocumentacion"]);
                        this._observacion_servicio = r["ObservacionServicio"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                        this._row_version = (byte[])r["RowVersion"];

                        //Indicando asignación correcta de atributos
                        resultado = true;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la carga de los atributos de la instancia
        /// </summary>
        /// <param name="no_servicio">Id de Servicio</param>
        /// <param name="id_compania_emisor">Id de Compañía emisara</param>
        /// <returns></returns>
        private bool cargarAtributosInstancia(string no_servicio, int id_compania_emisor)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Declarando arreglo de parámetros para consulta en BD
            object[] param = { 5, 0, "", 0, no_servicio, 0, 0, false, 0, 0, id_compania_emisor, 0, 0, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando entre resultados devueltos
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores de atributos
                        this._id_servicio = Convert.ToInt32(r["Id"]);
                        this._prefijo_servicio = r["Prefijo"].ToString();
                        this._consecutivo_compania_emisor = Convert.ToInt32(r["Consecutivo"]);
                        this._no_servicio = r["NoServicio"].ToString();
                        this._id_origen_datos = Convert.ToInt32(r["IdOrigenDatos"]);
                        this._id_servicio_base = Convert.ToInt32(r["IdServicioBase"]);
                        this._bit_servicio_base = Convert.ToBoolean(r["BitServicioBase"]);
                        this._id_cotizacion = Convert.ToInt32(r["IdCotizacion"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._id_cliente_receptor = Convert.ToInt32(r["IdClienteReceptor"]);
                        this._id_ubicacion_carga = Convert.ToInt32(r["IdUbicacionCarga"]);
                        this._cita_carga = Convert.ToDateTime(r["CitaCarga"]);
                        this._id_ubicacion_descarga = Convert.ToInt32(r["IdUbicacionDescarga"]);
                        this._cita_descarga = Convert.ToDateTime(r["CitaDescarga"]);
                        this._porte = r["Porte"].ToString();
                        this._referencia_cliente = r["ReferenciaCliente"].ToString();
                        this._fecha_documentacion = Convert.ToDateTime(r["FechaDocumentacion"]);
                        this._observacion_servicio = r["ObservacionServicio"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                        this._row_version = (byte[])r["RowVersion"];

                        //Indicando asignación correcta de atributos
                        resultado = true;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la carga de los atributos de la instancia
        /// </summary>
        /// <param name="no_servicio">Id de Servicio</param>
        /// <param name="id_compania_emisor">Id de Compañía emisara</param>
        /// <returns></returns>
        private bool cargarAtributosInstancia(string no_servicio, string no_viaje, int id_compania_emisor)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

                //Declarando arreglo de parámetros para consulta en BD
                object[] param = { 13, 0, "", 0, no_servicio, 0, 0, false, 0, 0, id_compania_emisor, 0, 0, null, 0, null, "", no_viaje, null, "", 0, false, null, "", "" };

                //Realizando consulta
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
                {
                    //Validando el origen de datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Iterando entre resultados devueltos
                        foreach (DataRow r in ds.Tables["Table"].Rows)
                        {
                            //Asignando valores de atributos
                            this._id_servicio = Convert.ToInt32(r["Id"]);
                            this._prefijo_servicio = r["Prefijo"].ToString();
                            this._consecutivo_compania_emisor = Convert.ToInt32(r["Consecutivo"]);
                            this._no_servicio = r["NoServicio"].ToString();
                            this._id_origen_datos = Convert.ToInt32(r["IdOrigenDatos"]);
                            this._id_servicio_base = Convert.ToInt32(r["IdServicioBase"]);
                            this._bit_servicio_base = Convert.ToBoolean(r["BitServicioBase"]);
                            this._id_cotizacion = Convert.ToInt32(r["IdCotizacion"]);
                            this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                            this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                            this._id_cliente_receptor = Convert.ToInt32(r["IdClienteReceptor"]);
                            this._id_ubicacion_carga = Convert.ToInt32(r["IdUbicacionCarga"]);
                            this._cita_carga = Convert.ToDateTime(r["CitaCarga"]);
                            this._id_ubicacion_descarga = Convert.ToInt32(r["IdUbicacionDescarga"]);
                            this._cita_descarga = Convert.ToDateTime(r["CitaDescarga"]);
                            this._porte = r["Porte"].ToString();
                            this._referencia_cliente = r["ReferenciaCliente"].ToString();
                            this._fecha_documentacion = Convert.ToDateTime(r["FechaDocumentacion"]);
                            this._observacion_servicio = r["ObservacionServicio"].ToString();
                            this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                            this._row_version = (byte[])r["RowVersion"];

                            //Indicando asignación correcta de atributos
                            resultado = true;
                        }
                    }
                }
            

            //Devolviendo resultado
            return resultado;
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
            object[] param = { 4, this._id_servicio, "", 0, "", 0, 0, false, 0, 0, 0, 0, 0, null, 0, null, "", "", null, "", 0, false, this._row_version, "", "" };

            //Realizando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Indicando asignación correcta de atributos
                    resultado = true;
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de los valores de atributos del registro
        /// </summary>
        /// <param name="prefijo_servicio">Prefijo de servicio</param>
        /// <param name="consecutivo_compania_emisor">Número consecutivo ligado al prefijo del servicio</param>
        /// <param name="id_origen_datos">Id de Origen de Datos</param>
        /// <param name="id_servicio_base">Id de servicio base en el cual tiene origen este servicio</param>
        /// <param name="bit_servicio_base">Un Valor 'True' Indica que el Servicio proviene de un servicio base</param>
        /// <param name="id_cotizacion">Id de Cotización que dio origen al servicio</param>
        /// <param name="estatus">Estatus actual del servicio</param>
        /// <param name="id_compania_emisor">Id de Compañía que genera el servicio</param>
        /// <param name="id_cliente_receptor">Id de Cliente que solicita el servicio</param>
        /// <param name="id_ubicacion_carga">Id de Ubicación donde se realiza la carga</param>
        /// <param name="cita_carga">Fecha de la cita de carga</param>
        /// <param name="id_ubicacion_descarga">Id de la Ubicación dónde se realiza la descarga</param>
        /// <param name="cita_descarga">Fecha de la cita de descarga</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="referencia_cliente">Número de referencia de servicio del cliente</param>
        /// <param name="fecha_documentacion">Fecha de documentación</param>
        /// <param name="observacion_servicio">Observación general del servicio</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <param name="habilitar">Valor de habilitación del registro</param>
        /// <returns></returns>
        private RetornoOperacion editarServicio(string prefijo_servicio, int consecutivo_compania_emisor, int id_origen_datos, int id_servicio_base, bool bit_servicio_base, int id_cotizacion,
                                        Estatus estatus, int id_compania_emisor, int id_cliente_receptor, int id_ubicacion_carga, DateTime cita_carga, int id_ubicacion_descarga, DateTime cita_descarga,
                                        string porte, string referencia_cliente, DateTime fecha_documentacion, string observacion_servicio, int id_usuario, bool habilitar)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando versión actual de registro
            if (validarVersion())
            {
                //Definiendo parámetros de actualización
                object[] param = { 2, this._id_servicio, prefijo_servicio, consecutivo_compania_emisor, this._no_servicio, id_origen_datos, id_servicio_base, bit_servicio_base, id_cotizacion, (byte)estatus, 
                                     id_compania_emisor, id_cliente_receptor, id_ubicacion_carga, cita_carga, id_ubicacion_descarga, cita_descarga, porte, referencia_cliente, fecha_documentacion, 
                                     observacion_servicio, id_usuario, habilitar, this._row_version, "", "" };

                //Realizando actualización
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            }
            //De lo contrario
            else
                resultado = new RetornoOperacion("El registro fue modificado en BD desde la última vez que fue consultado.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la validación de una referencia de número de viaje o que el campo referencia cliente del encabezado de servicio se encuentre asignado
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validarNoViajeCliente()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);

            //Validando necesidad de asignación de No. de Viaje de cliente antes de terminar servicio (configuración de compañía)
            string noViajeObligatorio = Referencia.CargaReferencia(this.id_compania_emisor, 25, ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 25, "No. Viaje Obligatorio", 0, "Configuración Operativa"));

            //Si es requerida esta validación (acorde al perfil de la compañía)
            if (Convert.ToBoolean(noViajeObligatorio == "" ? "false" : noViajeObligatorio))
            {
                //Verificando que la referencia de viaje o encabezado de servicio esté asignada
                if (this._referencia_cliente == "" && Referencia.CargaReferencia(this.id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "No. Viaje", 0, "Referencia de Viaje")) == "")
                    //Marcando el error
                    resultado = new RetornoOperacion("El Número de Viaje del Cliente se encuentra sin asignar.");
            }

            //Devolvicneo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Validar el Costo del Viaje antes de Terminar el Viaje
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaCostoViaje()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);

            //Validando necesidad de asignación de No. de Viaje de cliente antes de terminar servicio (configuración de compañía)
            string costoObligatorio = Referencia.CargaReferencia(this.id_compania_emisor, 25, ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 25, "Costo de Viaje Obligatorio", 0, "Configuración Operativa"));

            //Si es requerida esta validación (acorde al perfil de la compañía)
            if (Convert.ToBoolean(costoObligatorio == "" ? "false" : costoObligatorio))
            {
                //Instanciando Facturado
                using (Facturado fac = Facturado.ObtieneFacturaServicio(this._id_servicio))
                {
                    //Validando Que exista un Costo
                    if (!(fac.total_factura > 0 && fac.total_factura_pesos > 0))
                        
                        //Marcando el error
                        resultado = new RetornoOperacion("El Viaje aún no tiene un costo definido");
                }
            }

            //Devolvicneo resultado
            return resultado;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Realiza la inserción de un nuevo registro servicio
        /// </summary>
        /// <param name="prefijo_servicio">Prefijo de servicio</param>
        /// <param name="id_origen_datos">Id de Origen de Datos</param>
        /// <param name="id_servicio_base">Id de servicio base en el cual tiene origen este servicio</param>
        /// <param name="bit_servicio_base">Un Valor 'True' Indica que el Servicio proviene de un servicio base</param>
        /// <param name="id_cotizacion">Id de Cotización que dio origen al servicio</param>
        /// <param name="id_compania_emisor">Id de Compañía que genera el servicio</param>
        /// <param name="id_cliente_receptor">Id de Cliente que solicita el servicio</param>
        /// <param name="id_ubicacion_carga">Id de Ubicación donde se realiza la carga</param>
        /// <param name="cita_carga">Fecha de la cita de carga</param>
        /// <param name="id_ubicacion_descarga">Id de la Ubicación dónde se realiza la descarga</param>
        /// <param name="cita_descarga">Fecha de la cita de descarga</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="referencia_cliente">Número de referencia de servicio del cliente</param>
        /// <param name="fecha_documentacion">Fecha de documentación</param>
        /// <param name="observacion_servicio">Observación general del servicio</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarServicio(string prefijo_servicio, int id_origen_datos, int id_servicio_base, bool bit_servicio_base, int id_cotizacion,
                                        int id_compania_emisor, int id_cliente_receptor, int id_ubicacion_carga, DateTime cita_carga, int id_ubicacion_descarga, DateTime cita_descarga,
                                        string porte, string referencia_cliente, DateTime fecha_documentacion, string observacion_servicio, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que la cita de descarga sea mayor a la de carga
            if (cita_descarga.CompareTo(cita_carga) > 0)
            {
                //Definiendo parámetros de actualización
                object[] param = { 1, 0, prefijo_servicio, 0, "", id_origen_datos, id_servicio_base, bit_servicio_base, id_cotizacion, (byte)Estatus.Documentado, 
                                     id_compania_emisor, id_cliente_receptor, id_ubicacion_carga, cita_carga, id_ubicacion_descarga, cita_descarga, porte, referencia_cliente, fecha_documentacion, 
                                     observacion_servicio, id_usuario, true, null, "", "" };

                //Realizando actualización
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            }
            else
                resultado = new RetornoOperacion(string.Format("La cita de la parada final '{0:dd/MM/yyyy HH:mm}' debe ser mayor a '{1:dd/MM/yyyy HH:mm}'", cita_descarga, cita_carga));
            
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la inserción de un nuevo registro servicio
        /// </summary>
        /// <param name="prefijo_servicio">Prefijo de servicio</param>
        /// <param name="estatus">Estatus del Servicio</param>
        /// <param name="id_origen_datos">Id de Origen de Datos</param>
        /// <param name="id_servicio_base">Id de servicio base en el cual tiene origen este servicio</param>
        /// <param name="bit_servicio_base">Un Valor 'True' Indica que el Servicio proviene de un servicio base</param>
        /// <param name="id_cotizacion">Id de Cotización que dio origen al servicio</param>
        /// <param name="id_compania_emisor">Id de Compañía que genera el servicio</param>
        /// <param name="id_cliente_receptor">Id de Cliente que solicita el servicio</param>
        /// <param name="id_ubicacion_carga">Id de Ubicación donde se realiza la carga</param>
        /// <param name="cita_carga">Fecha de la cita de carga</param>
        /// <param name="id_ubicacion_descarga">Id de la Ubicación dónde se realiza la descarga</param>
        /// <param name="cita_descarga">Fecha de la cita de descarga</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="referencia_cliente">Número de referencia de servicio del cliente</param>
        /// <param name="fecha_documentacion">Fecha de documentación</param>
        /// <param name="observacion_servicio">Observación general del servicio</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarServicio(string prefijo_servicio, Estatus estatus, int id_origen_datos, int id_servicio_base, bool bit_servicio_base, int id_cotizacion,
                                    int id_compania_emisor, int id_cliente_receptor, int id_ubicacion_carga, DateTime cita_carga, int id_ubicacion_descarga, DateTime cita_descarga,
                                    string porte, string referencia_cliente, DateTime fecha_documentacion, string observacion_servicio, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Definiendo parámetros de actualización
            object[] param = { 1, 0, prefijo_servicio, 0, "", id_origen_datos, id_servicio_base, bit_servicio_base, id_cotizacion, estatus, 
                                     id_compania_emisor, id_cliente_receptor, id_ubicacion_carga, cita_carga, id_ubicacion_descarga, cita_descarga, porte, referencia_cliente, fecha_documentacion, 
                                     observacion_servicio, id_usuario, true, null, "", "" };

            //Realizando actualización
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de los datos generales del servicio
        /// </summary>
        /// <param name="id_cliente_receptor">Id de Cliente que solicita el servicio</param>
        /// <param name="id_ubicacion_carga">Id de Ubicación donde se realiza la carga</param>
        /// <param name="cita_carga">Fecha de la cita de carga</param>
        /// <param name="id_ubicacion_descarga">Id de la Ubicación dónde se realiza la descarga</param>
        /// <param name="cita_descarga">Fecha de la cita de descarga</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="referencia_cliente">Número de referencia de servicio del cliente</param>
        /// <param name="observacion_servicio">Observación general del servicio</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditarServicio(int id_cliente_receptor, int id_ubicacion_carga, DateTime cita_carga, int id_ubicacion_descarga,
                                            DateTime cita_descarga, string porte, string referencia_cliente, string observacion_servicio, int id_usuario)
        {
            //Realizando edición
            return editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                            this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, id_cliente_receptor, id_ubicacion_carga, cita_carga,
                            id_ubicacion_descarga, cita_descarga, porte, referencia_cliente, this._fecha_documentacion, observacion_servicio, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Editamos Refrencia del Cliente
        /// </summary>
        /// <param name="referencia_cliente"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditarServicio(string referencia_cliente, int id_usuario)
        {
            //Realizando edición
            return editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                            this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                            this._id_ubicacion_descarga, this._cita_descarga, this._porte, referencia_cliente, this._fecha_documentacion, this._observacion_servicio, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método encargado de Sincronizar la referencia de Viaje
        /// </summary>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizacionReferenciaViaje(string referencia, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);
            //Obtenemos Refernacia a de Numero de Viaje del Servicio
            int idReferencia = Referencia.CargaRegistroReferencia(this._id_compania_emisor.ToString(), 1,this._id_servicio , "Referencia de Viaje", "No. Viaje");

            //Validamos Existencia de Viaje
            if (idReferencia > 0)
            {
                //Instanciamos Referencia
                using (SAT_CL.Global.Referencia objReferencia = new SAT_CL.Global.Referencia(idReferencia))
                {
                    if (objReferencia.habilitar)
                    {
                        //De acuerdo a la Refrencia Actual del Viaje
                        if (!string.IsNullOrEmpty(referencia))
                            //Editamos Referencia
                            resultado = SAT_CL.Global.Referencia.EditaReferencia(idReferencia, referencia.Trim(), id_usuario);

                        else if (string.IsNullOrEmpty(referencia))
                            //Eliminamos Referencia
                            resultado = SAT_CL.Global.Referencia.EliminaReferencia(idReferencia, id_usuario);
                    }
                    else
                        resultado = new RetornoOperacion("No se puede recuperar la Referencia 'No. Viaje'");
                }
            }
            else
            {
                //Validamos que Exista Referencia
                if (!string.IsNullOrEmpty(referencia))
                    //Insertando Referencia de Viaje
                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(this._id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(this.id_compania_emisor, 1, "No. Viaje", 0, "Referencia de Viaje"),
                                                            referencia.Trim(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
            }
            
            //Validamos Resultado
            if(resultado.OperacionExitosa)
                //Asignando Resultado positivo
                resultado = new RetornoOperacion(this._id_servicio);
          
            //Devolvicneo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Editar el Cliente
        /// </summary>
        /// <param name="id_cliente_receptor">Id Cliente</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarServicio(int id_cliente_receptor , int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado =  new RetornoOperacion();
            //Instanciamos Factura
            SAT_CL.Facturacion.Facturado objFacturado =  Facturado.ObtieneFacturaServicio(this._id_servicio);

             //Validamos que no se encuentre Facturado
              resultado = objFacturado.ValidaFacturaElectronica();

              //Validamos Resultado
              if (resultado.OperacionExitosa)
              {
                  //Validamos asignaición a la Factura Global
                  resultado = objFacturado.ValidaFacturaGlobal();
                  //Validamos Resultado
                  if (resultado.OperacionExitosa)
                  {
                      //Realizando edición
                      resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                                      this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                      this._id_ubicacion_descarga, this._cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion, this._observacion_servicio, id_usuario, this._habilitar);
                  }
                  else
                      resultado = new RetornoOperacion("El servicio se encuentra en una Fcatura Global.");
              }
            else
                  resultado = new RetornoOperacion("El Servicio se encuentra Fcaturado.");

            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la edición de los datos de Referencia del Servicio
        /// </summary>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="referencia_cliente">Número de referencia de servicio del cliente</param>
        /// <param name="observacion_servicio">Observación general del servicio</param>
        /// <param name="id_cliente">Id de Cliente</param>
        /// <param name="resultadoCliente">Resultado de la Ediciónn de Cliente</param>
        /// <param name="resultadoReferenciaPrincipal">Resultado de la Referencia Actual</param>
        /// <param name="resultadoPorte">Resultado de Otras Observaciones</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaReferenciaServicio(string porte, string referencia_cliente, string observacion_servicio, int id_cliente, out RetornoOperacion resultadoCliente,
                                                            out RetornoOperacion resultadoReferenciaPrincipal, out RetornoOperacion resultadoPorte, int id_usuario)
        {
            //Declaramos Objeto Resultado para la Edición de Cliente
             resultadoCliente = new RetornoOperacion();
            //Declaramos Objeto Resultado para Edición de la Referencia Principal
             resultadoReferenciaPrincipal = new RetornoOperacion();
            //Declaramos Objeto Resultado para la Carta Porte
             resultadoPorte = new RetornoOperacion();
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Editamos Cliente
            resultadoCliente = EditarServicio(id_cliente, id_usuario);
            
            //Actualizamos Atributos
            if(ActualizaServicio())
            {
                //Realizando edición
                resultadoPorte = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                                this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                this._id_ubicacion_descarga, this._cita_descarga, porte, this._referencia_cliente, this._fecha_documentacion, observacion_servicio, id_usuario, this._habilitar);
               
            }
            
            //Actualizamos Atributo
            if (ActualizaServicio())
            {
                 //Creamos la transacción 
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Actualizamos Refrencias de Servicio
                    resultadoReferenciaPrincipal = ActualizacionReferenciaViaje(referencia_cliente, id_usuario);
                    //Validamos Resultado
                    if (resultadoReferenciaPrincipal.OperacionExitosa)
                    {
                        //Realizando edición
                        resultadoReferenciaPrincipal = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                                        this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                        this._id_ubicacion_descarga, this._cita_descarga, this._porte, referencia_cliente, this._fecha_documentacion, this._observacion_servicio, id_usuario, this._habilitar);


                    }
                    //Validamos Resultado
                    if(resultadoReferenciaPrincipal.OperacionExitosa)
                    {
                        //Finalizamos Transacción
                        scope.Complete();
                    }
                }
            }
            //Editamos Resultados
            resultadoCliente = new RetornoOperacion(string.Format("Cliente: {0}", resultadoCliente.Mensaje), resultadoCliente.OperacionExitosa);
            resultadoReferenciaPrincipal = new RetornoOperacion(string.Format("No Viaje: {0}", resultadoReferenciaPrincipal.Mensaje), resultadoReferenciaPrincipal.OperacionExitosa);
            resultadoPorte = new RetornoOperacion(string.Format("CartaPorte/Observación: {0}", resultadoPorte.Mensaje), resultadoPorte.OperacionExitosa);
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de los datos del Enzabezado del Servicio
        /// </summary>
        /// <param name="id_cliente">Cliente del Servicio</param>
        /// <param name="porte">Número de Carta Porte</param>
        /// <param name="referencia_cliente">Número de referencia de servicio del cliente</param>
        /// <param name="observacion_servicio">Observación general del servicio</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEncabezadoServicio(int id_cliente, string porte, string referencia_cliente, string observacion_servicio, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizando edición
                resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                                this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, id_cliente, this._id_ubicacion_carga, this._cita_carga,
                                this._id_ubicacion_descarga, this._cita_descarga, porte, referencia_cliente, this._fecha_documentacion, observacion_servicio, id_usuario, this._habilitar);

                //Validando Operación Exitosa
                if (resultado.OperacionExitosa)
                {
                    //Validando que exista una Referencia de Cliente
                    if (!referencia_cliente.Equals(""))
                    {
                        //Cargando Referencias
                        using (DataTable dtNoViaje = SAT_CL.Global.Referencia.CargaReferencias(this._id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "No. Viaje", 0, "Referencia de Viaje")))
                        {
                            //Validando que existe la Referencia
                            if (Validacion.ValidaOrigenDatos(dtNoViaje))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtNoViaje.Rows)
                                {
                                    //Instanciando No. de Viaje
                                    using (SAT_CL.Global.Referencia no_viaje = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que este habilitada
                                        if (no_viaje.habilitar)

                                            //Editando Referencia de Viaje
                                            resultado = SAT_CL.Global.Referencia.EditaReferencia(no_viaje.id_referencia, referencia_cliente, id_usuario);
                                        else
                                            //Instanciando Excepción
                                            resultado = new RetornoOperacion("No existe la Referencia 'No. Viaje'");
                                    }
                                }
                            }
                            else
                                //Insertando Referencia de Viaje
                                resultado = SAT_CL.Global.Referencia.InsertaReferencia(this._id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "No. Viaje", 0, "Referencia de Viaje"),
                                        referencia_cliente, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                        }
                    }
                    else
                        //Instanciando Resultado Positivo
                        resultado = new RetornoOperacion(this._id_servicio);
                }

                //Validando Operación Final
                if (resultado.OperacionExitosa)
                {
                    //Instanciando Resultado Positivo
                    resultado = new RetornoOperacion(this._id_servicio);

                    //Completando Transacción
                    scope.Complete();
                }

            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Cancela el servicio
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="motivo">Motivo de Cancelación</param>
        /// <returns></returns>
        public RetornoOperacion CancelaServicio(int id_usuario, string motivo)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Estatus del Servicio
                if ((Estatus)this._id_estatus == Estatus.Documentado)
                {
                    //Validando que no existan asignaciones de recurso pendientes
                    using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.CargaAsignacionesServicio(this._id_servicio))
                    {
                        //Si no hay asignaciones activas (Registradas, Iniciadas o Terminadas)
                        if (mitAsignaciones == null)
                        {
                            //Obtenemos Factura ligada al Servicio
                            Facturado objFactura = Facturado.ObtieneFacturaServicio(this._id_servicio);

                            //Validando que exista la Factura
                            if (objFactura.habilitar)
                            {
                                //Cancelamos Facura
                                resultado = objFactura.CancelaFactura(id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertamos Referencia de Cancelación 
                                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(this._id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "Motivo de la Cancelación", 0, "Referencia de Viaje"),
                                                               motivo, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                                    
                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Devolviendo resultado
                                        resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                                                            this._id_cotizacion, Estatus.Cancelado, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                                            this._id_ubicacion_descarga, this._cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion,
                                                            this._observacion_servicio, id_usuario, this._habilitar);

                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Obteniendo Referencias Unicas
                                            using (DataTable dt = Referencia.CargaReferenciasUnicasServicio(this._id_servicio))
                                            {
                                                //Validando Referencias
                                                if (Validacion.ValidaOrigenDatos(dt))
                                                {
                                                    //Recorriendo Ciclo
                                                    foreach (DataRow dr in dt.Rows)
                                                    {
                                                        //Instanciando Referencia
                                                        using (Referencia rf = new Referencia(Convert.ToInt32(dr["Id"])))
                                                        {
                                                            //Validando que exista la Referencia
                                                            if (rf.habilitar)
                                                            {
                                                                //Editando Referencia
                                                                resultado = Referencia.EditaReferencia(rf.id_referencia, rf.valor + string.Format(" [CANCELADO {0:yyyyMMdd HH:mm}]", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()), id_usuario);

                                                                //Si la Operación no fue Exitosa
                                                                if (!resultado.OperacionExitosa)

                                                                    //Terminando Ciclo
                                                                    break;
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                resultado = new RetornoOperacion("No se puede acceder a la Referencia");
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Servicio
                                                    resultado = new RetornoOperacion(this._id_servicio);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No se puede acceder a la Factura del Servicio");
                        }
                        //Si hay asignacione pendientes
                        else
                            resultado = new RetornoOperacion("Este servicio ya tiene recursos asignados, es necesario liberarlos antes de Cancelar.");
                    }
                }
                else
                    //Establecemos mensaje Error
                    resultado = new RetornoOperacion("El estatus del servicio no permite su cancelación.");


                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Instanciando Servicio
                    resultado = new RetornoOperacion(this._id_servicio);

                    //Completamos transacción
                    scope.Complete();
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Actualizamos Estatus del Servicio
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusServicio(Estatus estatus, int id_usuario)
        {
            //Devolviendo resultado
            return editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos,
                                 this._id_servicio_base, this._bit_servicio_base, this._id_cotizacion, estatus, this._id_compania_emisor,
                                 this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga, this._id_ubicacion_descarga,
                                 this._cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion, this._observacion_servicio,
                                 id_usuario, this._habilitar);
        }

        /// <summary>
        /// Deshabilita el registro
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarServicio(int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando estatus actual (sólo si está documentado)
            if (this.estatus == Estatus.Documentado)
            {
                //Inicializando bloque transaccional
                using (TransactionScope transaccion = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {                    
                    //Deshabilitando productos
                    resultado = ServicioProducto.DeshabilitaServiciosProductos(this._id_servicio, id_usuario);

                    //Validamos resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Deshabilitando Paradas y Eventos
                        resultado = Parada.DeshabilitaParadas(this._id_servicio, id_usuario);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Deshabilitando segmentos
                            resultado = SegmentoCarga.DeshabilitaSegmentosDeCarga(this._id_servicio, id_servicio);

                            //Validamos Segmento
                            if (resultado.OperacionExitosa)
                            {
                                //Deshabilitando movimientos 
                                resultado = Movimiento.DeshabilitaMovimientos(this.id_servicio, id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Deshabilitando Clasificación
                                    using (Clasificacion objClasificacion = new Clasificacion(1, this._id_servicio, 0))
                                    {
                                        //Validamos que exista la Clasificación
                                        if (objClasificacion.id_clasificacion > 0)
                                        {
                                            //Deshabilitamos Clasificación
                                            resultado = objClasificacion.DeshabilitaClasificacion(id_usuario);
                                        }

                                    }
                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Deshabilitando Requerimientos
                                        resultado = ServicioRequerimiento.DeshabilitaRequerimientos(this._id_servicio, id_usuario);
                                        
                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Obtenemos Factura ligada al Servicio
                                            Facturado objFactura = Facturado.ObtieneFacturaServicio(this._id_servicio);

                                            //Validamos que exista la factura
                                            if (objFactura.id_factura > 0)
                                            {
                                                //Deshabilitando Facturación 
                                                resultado = objFactura.DeshabilitaFactura(id_usuario);
                                            }

                                            //Validamos Resultado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Deshabilitamos Refrencias
                                                resultado = Referencia.EliminaReferenciasNoEditable(1, this._id_servicio, id_usuario);
                                            }

                                            //Si no hay errores haste este punto
                                            if (resultado.OperacionExitosa)
                                                //Realizando actualización y almacenando resultado
                                                resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                                                                    this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                                                    this._id_ubicacion_descarga, this._cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion,
                                                                    this._observacion_servicio, id_usuario, false);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Si no existen errores
                    if (resultado.OperacionExitosa)
                        //Confirmando acciones de transacción
                        transaccion.Complete();
                }
            }
            //De lo contrario
            else
                resultado = new RetornoOperacion("No es posible eliminar el servicio, su estatus actual es :" + this.estatus.ToString());

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Convierte un servicio común a un servicio maestro, siempre que este se encuentre en estatus "Documentado"
        /// </summary>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion HacerMaestro(int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que el servicio aún no sea maestro
            if (!this._bit_servicio_base)
            {
                //Validando estatus actual (sólo si está documentado)
                if (this.estatus == Estatus.Documentado)
                {
                    //Validando que no existan asignaciones ed recurso
                    if (MovimientoAsignacionRecurso.CargaAsignacionesServicio(this._id_servicio) == null)
                    {
                        //Realizando actualización y almacenando resultado
                        resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, true,
                                                        this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                                        this._id_ubicacion_descarga, this._cita_descarga, "", this._referencia_cliente, this._fecha_documentacion,
                                                        this._observacion_servicio, id_usuario, this._habilitar);
                    }
                    //Si ya hay asignaciones, se señala el error
                    else
                        resultado = new RetornoOperacion("Existen recursos asignados en algún movimiento de este servicio. No es posible marcar el servicio como 'Maestro'.");
                }
                else
                    resultado = new RetornoOperacion("No es posible marcar el servicio como 'Maestro', su estatus actual es :" + this.estatus.ToString());
            }
            //De lo contrario
            else
                resultado = new RetornoOperacion("Este ya es un Servicio 'Maestro'.");

            //Devolviendo resultado
            return resultado;
        }        
        /// <summary>
        /// Realiza la copia del servicio, siempre que el servicio esté marcado como Maestro
        /// </summary>
        /// <param name="id_usuario">Id de usuario que realiza la copia</param>
        /// <returns></returns>
        public RetornoOperacion CopiarServicio(int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que el servicio sea maestro
            if (this._bit_servicio_base)
            {
                //Inicializando bloque transaccional
                using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Declarando variable principal, la mantendrá el nuevo Id de servicio disponible
                    int id_servicio = 0;

                    //Cargando las paradas asociadas al servicio maestro
                    using (DataTable tblParadas = Parada.CargaParadasParaCopia(this._id_servicio))
                    {
                        //Validando existencia de paradas para copia
                        if (Validacion.ValidaOrigenDatos(tblParadas))
                        {
                            //Seleccionando primer par de paradas del servicio
                            DataRow p1 = (from DataRow r in tblParadas.Rows
                                          where Convert.ToInt32(r["Secuencia"]) == 1
                                          select r).FirstOrDefault();
                            DataRow pU = (from DataRow r in tblParadas.Rows
                                          orderby Convert.ToInt32(r["Secuencia"]) descending
                                          select r).FirstOrDefault();

                            //Obteniendo valores para fecha inicial y final (primer y segunda cita respectivamente)
                            double minutos_incremento = Convert.ToDateTime(pU["CitaParada"]).Subtract(Convert.ToDateTime(p1["CitaParada"])).TotalMinutes;
                            DateTime fecha_primer_cita = Fecha.ObtieneFechaEstandarMexicoCentro(),
                                    fecha_ultima_cita = fecha_primer_cita.AddMinutes(minutos_incremento * (int)Convert.ToInt32(pU["Secuencia"]));

                            //Insertando nuevo encabezado de servicio
                            resultado = InsertarServicio(this._prefijo_servicio, this._id_origen_datos, this._id_servicio, false, this._id_cotizacion, this._id_compania_emisor,
                                                this._id_cliente_receptor, Convert.ToInt32(p1["IdUbicacion"]), fecha_primer_cita, Convert.ToInt32(pU["IdUbicacion"]), fecha_ultima_cita, "", "",
                                                Fecha.ObtieneFechaEstandarMexicoCentro(), "", id_usuario);

                            //Validando Operación Exitosa
                            if (resultado.OperacionExitosa)
                            {
                                //Conservando Id de Servicio nuevo
                                id_servicio = resultado.IdRegistro;

                                //Inicializando información de paradas, segmentos y movimientos de viaje
                                resultado = Parada.InsertaServicio(id_servicio, Convert.ToInt32(p1["IdUbicacion"]), fecha_primer_cita,
                                                                   Convert.ToInt32(pU["IdUbicacion"]), fecha_ultima_cita,
                                                                   0, this._id_compania_emisor, id_usuario, true);

                                //Si no hay error al registrar nuevo servicio
                                if (resultado.OperacionExitosa)
                                {
                                    //Declarando Contador
                                    int count = 2;

                                    //Recorriendo Paradas
                                    foreach (DataRow stop in tblParadas.Rows)
                                    {
                                        //Validando que no sea ni la PRIMERA ni la ULTIMA
                                        if (Convert.ToInt32(stop["Secuencia"]) != Convert.ToInt32(p1["Secuencia"]) &&
                                            Convert.ToInt32(stop["Secuencia"]) != Convert.ToInt32(pU["Secuencia"]))
                                        {
                                            //Instanciando Parada
                                            using (Parada st = new Parada(Convert.ToInt32(stop["Id"])))
                                            {
                                                //Validando que exista
                                                if (st.habilitar)
                                                {
                                                    //Insertando Paradas
                                                    resultado = Parada.NuevaParadaDocumentacion(id_servicio, st.secuencia_parada_servicio, st.Tipo, st.id_ubicacion, st.geo_ubicacion,
                                                        st.secuencia_parada_servicio == 1 ? fecha_primer_cita : fecha_primer_cita.AddMinutes(Convert.ToDouble(minutos_incremento * (int)st.secuencia_parada_servicio)),
                                                                        0, this._id_compania_emisor, count, id_usuario);

                                                    //Validando Errores
                                                    if (!resultado.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;

                                                    //Incrementando Contador
                                                    count++;
                                                }
                                                else
                                                {
                                                    //Instanciando Excepción
                                                    resultado = new RetornoOperacion("No se puede recuperar la Parada");
                                                    //Terminando Ciclo
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                            resultado = new RetornoOperacion("No pudieron ser recuperadas las paradas del servicio origen de la copia.");
                    }

                    //Si no hay errores encontrados (paradas, eventos y productos)
                    if (resultado.OperacionExitosa)
                    {
                        //Buscando la clasificación del servicio
                        using (Global.Clasificacion clasif = new Global.Clasificacion(1, this._id_servicio, 0))
                        {
                            //Si existe una clasificación
                            if (clasif.id_clasificacion > 0)
                            {
                                //Copiando clasificación
                                resultado = Global.Clasificacion.InsertaClasificacion(1, id_servicio, clasif.id_tipo, clasif.id_flota,
                                                                        clasif.id_region, clasif.id_ubicacion_terminal, clasif.id_tipo_servicio,
                                                                        clasif.id_alcance_servicio, clasif.id_detalle_negocio, clasif.id_clasificacion1,
                                                                        clasif.id_clasificacion2, id_usuario);

                                //Personalizando resultado en caso de error
                                if (!resultado.OperacionExitosa)
                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Clasificación ({0}); {1}", clasif.id_clasificacion, resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                    }

                    //Si no hay errores encontrados (paradas, eventos, productos y clasificación)
                    if (resultado.OperacionExitosa)
                    {
                        //Buscando requerimientos aplicables al servicio maestro
                        using (DataTable tblRequerimientos = ServicioRequerimiento.CargaRequerimientosServicio(this._id_servicio))
                        {
                            //Validando origen de datos
                            if (Validacion.ValidaOrigenDatos(tblRequerimientos))
                            {
                                //Para cada requerimiento encontrado
                                foreach (DataRow rRequerimiento in tblRequerimientos.Rows)
                                {
                                    //Insertando copia de requerimiento
                                    resultado = ServicioRequerimiento.InsertaServicioRequerimiento(Convert.ToInt32(rRequerimiento["IdRequerimiento"]), id_servicio, Convert.ToInt32(rRequerimiento["IdTabla"]),
                                                                            Convert.ToInt32(rRequerimiento["IdRegistro"]), rRequerimiento["Descripcion"].ToString(), Convert.ToInt32(rRequerimiento["IdTablaObjetivo"]),
                                                                            Convert.ToInt32(rRequerimiento["IdCampoObjetivo"]), rRequerimiento["ValorObjetivo"].ToString(), rRequerimiento["Condicion"].ToString(), id_usuario);

                                    //Si existe algún error
                                    if (!resultado.OperacionExitosa)
                                    {
                                        //Personalizando error
                                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Requerimiento ({0}){1}; {2}", rRequerimiento["Id"], rRequerimiento["Descripcion"], resultado.Mensaje), resultado.OperacionExitosa);
                                        //Se interrumpe el ciclo de copia de productos
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //Si no hay errores encontrados (paradas, eventos, productos, clasificación y requerimiento)
                    if (resultado.OperacionExitosa)
                    {
                        //Cargando datos de facturación existentes (encabezado)
                        using (Facturacion.Facturado factura = Facturacion.Facturado.ObtieneFacturaServicio(this._id_servicio))
                        {
                            //Si la facturación existe
                            if (factura.id_factura > 0)
                            {
                                //Realizando copia a nuevo servicio
                                resultado = Facturacion.Facturado.InsertaFactura(id_servicio, id_usuario);

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Guardando Id de Factura
                                    int id_factura = resultado.IdRegistro;
                                    //Cargando los detalles de la factura
                                    using (DataTable tblCargos = Facturacion.FacturadoConcepto.ObtieneConceptosFactura(factura.id_factura))
                                    {
                                        //Verificando que existen detalles por copiar
                                        if (Validacion.ValidaOrigenDatos(tblCargos))
                                        {
                                            //Para cada cargo (detalle de factura)
                                            foreach (DataRow rCargo in tblCargos.Rows)
                                            {
                                                //Realizando copia de detalle
                                                resultado = Facturacion.FacturadoConcepto.InsertaFacturaConcepto(id_factura, Convert.ToDecimal(rCargo["Cantidad"]), Convert.ToByte(rCargo["IdUnidad"]),
                                                                                                rCargo["Identificador"].ToString(), Convert.ToInt32(rCargo["IdConceptoCobro"]), Convert.ToDecimal(rCargo["ValorUnitario"]),
                                                                                                Convert.ToDecimal(rCargo["ImportePesos"]), Convert.ToByte(rCargo["IdImpuestoRetenido"]), Convert.ToDecimal(rCargo["TasaImpuestoRetenido"]), Convert.ToByte(rCargo["IdImpuestoTrasladado"]), Convert.ToDecimal(rCargo["TasaImpuestoTrasladado"]),
                                                                                                Convert.ToInt32(rCargo["IdCargoRecurrente"]), id_usuario);
                                                //Si existe algún error
                                                if (!resultado.OperacionExitosa)
                                                {
                                                    //Personalizando error
                                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Cargo ({0}){1}; {2}", rCargo["Id"], rCargo["ConceptoCobro"], resultado.Mensaje), resultado.OperacionExitosa);
                                                    //Se interrumpe el ciclo de copia de productos
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    //Personalizando error
                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Encabezado de Factura ({0}){1}; {2}", factura.id_factura, factura.no_factura, resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Finalizando transacción
                        transaccion.Complete();
                        //Crenado resultado con Id de Servicio nuevo
                        resultado = new RetornoOperacion(id_servicio);
                    }
                }
            }
            //De lo contrario
            else
                resultado = new RetornoOperacion("El servicio no puede ser copiado, no es un servicio maestro.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la copia del servicio, siempre que el servicio esté marcado como Maestro
        /// </summary>
        /// <param name="cita_carga_copia">Fecha de cita de la primer parada</param>
        /// <param name="cita_descarga_copia">Fecha de cita de la última parada</param>
        /// <param name="no_viaje">Número de Viaje del Cliente al que se hace el servicio (Referencia y Ref Cliente del Encabezado)</param>
        /// <param name="no_confirmacion">Número de Confirmación de Viaje (Referencia)</param>
        /// <param name="id_usuario">Id de usuario que realiza la copia</param>
        /// <returns></returns>
        public RetornoOperacion CopiarServicio(DateTime cita_carga_copia, DateTime cita_descarga_copia, string no_viaje, string no_confirmacion, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que el servicio sea maestro
            if (this._bit_servicio_base)
            {
                //Inicializando bloque transaccional
                using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Declarando variable principal, la mantendrá el nuevo Id de servicio disponible
                    int id_servicio = 0;

                    //Cargando las paradas asociadas al servicio maestro
                    using (DataTable tblParadas = Parada.CargaParadasParaCopia(this._id_servicio))
                    {
                        //Validando existencia de paradas para copia
                        if (Validacion.ValidaOrigenDatos(tblParadas))
                        {
                            //Seleccionando primer par de paradas del servicio
                            DataRow p1 = (from DataRow r in tblParadas.Rows
                                          where Convert.ToInt32(r["Secuencia"]) == 1
                                          select r).FirstOrDefault();
                            DataRow pU = (from DataRow r in tblParadas.Rows
                                          orderby Convert.ToInt32(r["Secuencia"]) descending
                                          select r).FirstOrDefault();

                            //Determinando número de paradas totales para indicar el incremeneto de tiempo (minutos) entre cada parada comprendida entre la cita de carga y descarga
                            decimal total_paradas_intermedias = tblParadas.Rows.Count - 2;
                            decimal minutos_incremento = Convert.ToDecimal(cita_descarga_copia.Subtract(cita_carga_copia).TotalMinutes) / tblParadas.Rows.Count;

                            //Validando que el incremento entre paradas sea de al menos 1 min.
                            if (minutos_incremento >= 1)
                            {
                                //Insertando nuevo encabezado de servicio
                                resultado = InsertarServicio(this._prefijo_servicio, this._id_origen_datos, this._id_servicio, false, this._id_cotizacion, this._id_compania_emisor,
                                                    this._id_cliente_receptor, Convert.ToInt32(p1["IdUbicacion"]), cita_carga_copia, Convert.ToInt32(pU["IdUbicacion"]), cita_descarga_copia, "", "",
                                                    Fecha.ObtieneFechaEstandarMexicoCentro(), "", id_usuario);

                                //Validando Operación Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Conservando Id de Servicio nuevo
                                    id_servicio = resultado.IdRegistro;

                                    //Inicializando información de paradas, segmentos y movimientos de viaje
                                    //Mandamos id parada para obtner los eventos
                                    resultado = Parada.InsertaServicio(id_servicio, Convert.ToInt32(p1["IdUbicacion"]), cita_carga_copia,
                                                                       Convert.ToInt32(pU["IdUbicacion"]), cita_descarga_copia,
                                                                       0, this._id_compania_emisor, id_usuario, true, Convert.ToInt32(p1["Id"]), Convert.ToInt32(pU["Id"]));

                                    //Si no hay error al registrar nuevo servicio
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Declarando Contador
                                        int count = 1;

                                        //Recorriendo Paradas
                                        foreach (DataRow stop in tblParadas.Rows)
                                        {
                                            //Validando que no sea ni la PRIMERA ni la ULTIMA
                                            if (Convert.ToInt32(stop["Secuencia"]) != Convert.ToInt32(p1["Secuencia"]) &&
                                                Convert.ToInt32(stop["Secuencia"]) != Convert.ToInt32(pU["Secuencia"]))
                                            {
                                                //Instanciando Parada
                                                using (Parada st = new Parada(Convert.ToInt32(stop["Id"])))
                                                {
                                                    //Validando que exista
                                                    if (st.habilitar)
                                                    {
                                                        //Insertando Paradas
                                                        resultado = Parada.NuevaParadaDocumentacion(id_servicio, st.secuencia_parada_servicio, st.Tipo, st.id_ubicacion, st.geo_ubicacion,
                                                            st.secuencia_parada_servicio == 1 ? cita_carga_copia : cita_carga_copia.AddMinutes(Convert.ToDouble(minutos_incremento * count)),
                                                                            0, this._id_compania_emisor, count, id_usuario, Convert.ToInt32(stop["Id"]));

                                                        //Validando Errores
                                                        if (!resultado.OperacionExitosa)

                                                            //Terminando Ciclo
                                                            break;
                                                    }
                                                    else
                                                    {
                                                        //Instanciando Excepción
                                                        resultado = new RetornoOperacion("No se puede recuperar la Parada");
                                                        //Terminando Ciclo
                                                        break;
                                                    }
                                                }
                                            }

                                            //Incrementando Contador
                                            count++;
                                        }
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion("El intervalo entre cada cita del nuevo servicio no puede ser menor a '1 min'. Ajuste la fecha de la última cita.");
                        }
                        else
                            resultado = new RetornoOperacion("No pudieron ser recuperadas las paradas del servicio origen de la copia.");
                    }

                    //Si no hay errores encontrados (paradas, eventos y productos)
                    if (resultado.OperacionExitosa)
                    {
                        //Buscando la clasificación del servicio
                        using (Global.Clasificacion clasif = new Global.Clasificacion(1, this._id_servicio, 0))
                        {
                            //Si existe una clasificación
                            if (clasif.id_clasificacion > 0)
                            {
                                //Copiando clasificación
                                resultado = Global.Clasificacion.InsertaClasificacion(1, id_servicio, clasif.id_tipo, clasif.id_flota,
                                                                        clasif.id_region, clasif.id_ubicacion_terminal, clasif.id_tipo_servicio,
                                                                        clasif.id_alcance_servicio, clasif.id_detalle_negocio, clasif.id_clasificacion1,
                                                                        clasif.id_clasificacion2, id_usuario);

                                //Personalizando resultado en caso de error
                                if (!resultado.OperacionExitosa)
                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Clasificación ({0}); {1}", clasif.id_clasificacion, resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                    }

                    //Si no hay errores encontrados (paradas, eventos, productos y clasificación)
                    if (resultado.OperacionExitosa)
                    {
                        //Buscando requerimientos aplicables al servicio maestro
                        using (DataTable tblRequerimientos = ServicioRequerimiento.CargaRequerimientosServicio(this._id_servicio))
                        {
                            //Validando origen de datos
                            if (Validacion.ValidaOrigenDatos(tblRequerimientos))
                            {
                                //Para cada requerimiento encontrado
                                foreach (DataRow rRequerimiento in tblRequerimientos.Rows)
                                {
                                    //Insertando copia de requerimiento
                                    resultado = ServicioRequerimiento.InsertaServicioRequerimiento(Convert.ToInt32(rRequerimiento["IdRequerimiento"]), id_servicio, Convert.ToInt32(rRequerimiento["IdTabla"]),
                                                                            Convert.ToInt32(rRequerimiento["IdRegistro"]), rRequerimiento["Descripcion"].ToString(), Convert.ToInt32(rRequerimiento["IdTablaObjetivo"]),
                                                                            Convert.ToInt32(rRequerimiento["IdCampoObjetivo"]), rRequerimiento["ValorObjetivo"].ToString(), rRequerimiento["Condicion"].ToString(), id_usuario);

                                    //Si existe algún error
                                    if (!resultado.OperacionExitosa)
                                    {
                                        //Personalizando error
                                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Requerimiento ({0}){1}; {2}", rRequerimiento["Id"], rRequerimiento["Descripcion"], resultado.Mensaje), resultado.OperacionExitosa);
                                        //Se interrumpe el ciclo de copia de productos
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //Si no hay errores encontrados (paradas, eventos, productos, clasificación y requerimiento)
                    if (resultado.OperacionExitosa)
                    {
                        //Cargando datos de facturación existentes (encabezado)
                        using (Facturacion.Facturado factura = Facturacion.Facturado.ObtieneFacturaServicio(this._id_servicio))
                        {
                            //Si la facturación existe
                            if (factura.id_factura > 0)
                            {
                                //Realizando copia a nuevo servicio
                                resultado = Facturacion.Facturado.InsertaFactura(id_servicio, id_usuario);

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Guardando Id de Factura
                                    int id_factura = resultado.IdRegistro;
                                    //Cargando los detalles de la factura
                                    using (DataTable tblCargos = Facturacion.FacturadoConcepto.ObtieneConceptosFactura(factura.id_factura))
                                    {
                                        //Verificando que existen detalles por copiar
                                        if (Validacion.ValidaOrigenDatos(tblCargos))
                                        {
                                            //Para cada cargo (detalle de factura)
                                            foreach (DataRow rCargo in tblCargos.Rows)
                                            {
                                                //Realizando copia de detalle
                                                resultado = Facturacion.FacturadoConcepto.InsertaFacturaConcepto(id_factura, Convert.ToDecimal(rCargo["Cantidad"]), Convert.ToByte(rCargo["IdUnidad"]),
                                                                                                rCargo["Identificador"].ToString(),Convert.ToInt32(rCargo["IdConceptoCobro"]), Convert.ToDecimal(rCargo["ValorUnitario"]),
                                                                                                Convert.ToDecimal(rCargo["ImportePesos"]), Convert.ToByte(rCargo["IdImpuestoRetenido"]), Convert.ToDecimal(rCargo["TasaImpuestoRetenido"]), Convert.ToByte(rCargo["IdImpuestoTrasladado"]), Convert.ToDecimal(rCargo["TasaImpuestoTrasladado"]),
                                                                                                Convert.ToInt32(rCargo["IdCargoRecurrente"]), id_usuario);
                                                //Si existe algún error
                                                if (!resultado.OperacionExitosa)
                                                {
                                                    //Personalizando error
                                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Cargo ({0}){1}; {2}", rCargo["Id"], rCargo["ConceptoCobro"], resultado.Mensaje), resultado.OperacionExitosa);
                                                    //Se interrumpe el ciclo de copia de productos
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    //Personalizando error
                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Encabezado de Factura ({0}){1}; {2}", factura.id_factura, factura.no_factura, resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                    }

                    //Si no hay errores de copia en información operativa
                    if (resultado.OperacionExitosa)
                    {
                        //Obteniendo id de tipo de referencias
                        int id_tipo_no_viaje = ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "No. Viaje", 0, "Referencia de Viaje");
                        int id_tipo_confirmacion = ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "Confirmación", 0, "Referencia de Viaje");

                        //Realizando inserción de referencias principales
                        //Si se especificó número de viaje (Ref Cliente)
                        if (no_viaje.Trim() != "" && id_tipo_no_viaje > 0)
                            //Insertando referencia de viaje
                            resultado = Referencia.InsertaReferencia(id_servicio, 1, id_tipo_no_viaje, no_viaje, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                        else if (no_viaje.Trim() != "")
                            resultado = new RetornoOperacion("No pudo ser localizado el Tipo de Referencia 'No. Viaje' para esta compañía.");

                        //Si no hay errorres
                        if (resultado.OperacionExitosa && no_confirmacion.Trim() != "" && id_tipo_confirmacion > 0)
                            //Insertando confirmación
                            resultado = Referencia.InsertaReferencia(id_servicio, 1, id_tipo_confirmacion, no_confirmacion, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                        else if (resultado.OperacionExitosa && no_confirmacion.Trim() != "")
                            resultado = new RetornoOperacion("No pudo ser localizado el Tipo de Referencia 'Confirmación' para esta compañía.");
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Finalizando transacción
                        transaccion.Complete();
                        //Crenado resultado con Id de Servicio nuevo
                        resultado = new RetornoOperacion(id_servicio);
                    }
                }
            }
            //De lo contrario
            else
                resultado = new RetornoOperacion("El servicio no puede ser copiado, no es un servicio maestro.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la copia del servicio, siempre que el servicio esté marcado como Maestro
        /// </summary>
        /// <param name="cita_carga_copia">Fecha de cita de la primer parada</param>
        /// <param name="cita_descarga_copia">Fecha de cita de la última parada</param>
        /// <param name="no_viaje">Número de Viaje del Cliente al que se hace el servicio (Referencia y Ref Cliente del Encabezado)</param>
        /// <param name="no_confirmacion">Número de Confirmación de Viaje (Referencia)</param>
        /// <param name="id_producto_carga">Id de Producto de Carga que va a sustituir el conjunto de productos definidos en el servicio maestro</param>
        /// <param name="cantidad_producto">Cantidad de producto</param>
        /// <param name="id_unidad_cantidad_producto">Unidad de conteo de producto</param>
        /// <param name="peso_producto">Peso del producto</param>
        /// <param name="id_unidad_peso_producto">Unidad de peso del producto</param>
        /// <param name="id_usuario">Id de usuario que realiza la copia</param>
        /// <returns></returns>
        public RetornoOperacion CopiarServicio(DateTime cita_carga_copia, DateTime cita_descarga_copia, string no_viaje, string no_confirmacion, int id_producto_carga,
                                        decimal cantidad_producto, byte id_unidad_cantidad_producto, decimal peso_producto, byte id_unidad_peso_producto, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que el servicio sea maestro
            if (this._bit_servicio_base)
            {
                //Inicializando bloque transaccional
                using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Declarando variable principal, la mantendrá el nuevo Id de servicio disponible
                    int id_servicio = 0;

                    //Cargando las paradas asociadas al servicio maestro
                    using (DataTable tblParadas = Parada.CargaParadasParaCopia(this._id_servicio))
                    {
                        //Validando existencia de paradas para copia
                        if (Validacion.ValidaOrigenDatos(tblParadas))
                        {
                            //Seleccionando primer par de paradas del servicio
                            DataRow p1 = (from DataRow r in tblParadas.Rows
                                          where Convert.ToInt32(r["Secuencia"]) == 1
                                          select r).FirstOrDefault();
                            DataRow pU = (from DataRow r in tblParadas.Rows
                                          orderby Convert.ToInt32(r["Secuencia"]) descending
                                          select r).FirstOrDefault();

                            //Determinando número de paradas totales para indicar el incremeneto de tiempo (minutos) entre cada parada comprendida entre la cita de carga y descarga
                            decimal total_paradas_intermedias = tblParadas.Rows.Count - 2;
                            decimal minutos_incremento = Convert.ToDecimal(cita_descarga_copia.Subtract(cita_carga_copia).TotalMinutes) / (total_paradas_intermedias + 1);

                            //Validando que el incremento entre paradas sea de al menos 1 min.
                            if (minutos_incremento >= 1)
                            {
                                //Obteniendo valores para fecha inicial y final (primer y segunda cita respectivamente)
                                DateTime fecha_primer_cita = cita_carga_copia,
                                        fecha_ultima_cita = cita_carga_copia.AddMinutes(Convert.ToDouble(minutos_incremento));

                                //Insertando nuevo encabezado de servicio
                                resultado = InsertarServicio(this._prefijo_servicio, this._id_origen_datos, this._id_servicio, false, this._id_cotizacion, this._id_compania_emisor,
                                                    this._id_cliente_receptor, Convert.ToInt32(p1["IdUbicacion"]), fecha_primer_cita, Convert.ToInt32(pU["IdUbicacion"]), fecha_ultima_cita, "", "",
                                                    Fecha.ObtieneFechaEstandarMexicoCentro(), "", id_usuario);

                                //Validando Operación Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Conservando Id de Servicio nuevo
                                    id_servicio = resultado.IdRegistro;

                                    //Inicializando información de paradas, segmentos y movimientos de viaje
                                    resultado = Parada.InsertaServicio(id_servicio, Convert.ToInt32(p1["IdUbicacion"]), fecha_primer_cita,
                                                                       Convert.ToInt32(pU["IdUbicacion"]), fecha_ultima_cita,
                                                                       0, this._id_compania_emisor, id_usuario, true);

                                    //Si no hay error al registrar nuevo servicio
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Declarando Contador
                                        int count = 2;

                                        //Recorriendo Paradas
                                        foreach (DataRow stop in tblParadas.Rows)
                                        {
                                            //Validando que no sea ni la PRIMERA ni la ULTIMA
                                            if (Convert.ToInt32(stop["Secuencia"]) != Convert.ToInt32(p1["Secuencia"]) &&
                                                Convert.ToInt32(stop["Secuencia"]) != Convert.ToInt32(pU["Secuencia"]))
                                            {
                                                //Instanciando Parada
                                                using (Parada st = new Parada(Convert.ToInt32(stop["Id"])))
                                                {
                                                    //Validando que exista
                                                    if (st.habilitar)
                                                    {
                                                        //Insertando Paradas
                                                        resultado = Parada.NuevaParadaDocumentacion(id_servicio, st.secuencia_parada_servicio, st.Tipo, st.id_ubicacion, st.geo_ubicacion,
                                                            st.secuencia_parada_servicio == 1 ? fecha_primer_cita : fecha_primer_cita.AddMinutes(Convert.ToDouble(minutos_incremento * (int)st.secuencia_parada_servicio)),
                                                                            0, this._id_compania_emisor, count, id_usuario);

                                                        //Validando Errores
                                                        if (!resultado.OperacionExitosa)

                                                            //Terminando Ciclo
                                                            break;

                                                        //Incrementando Contador
                                                        count++;
                                                    }
                                                    else
                                                    {
                                                        //Instanciando Excepción
                                                        resultado = new RetornoOperacion("No se puede recuperar la Parada");
                                                        //Terminando Ciclo
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion("El intervalo entre cada cita del nuevo servicio no puede ser menor a '1 min'. Ajuste la fecha de la última cita.");
                        }
                        else
                            resultado = new RetornoOperacion("No pudieron ser recuperadas las paradas del servicio origen de la copia.");
                    }

                    //Si no hay errores encontrados (paradas, eventos y productos)
                    if (resultado.OperacionExitosa)
                    {
                        //Buscando la clasificación del servicio
                        using (Global.Clasificacion clasif = new Global.Clasificacion(1, this._id_servicio, 0))
                        {
                            //Si existe una clasificación
                            if (clasif.id_clasificacion > 0)
                            {
                                //Copiando clasificación
                                resultado = Global.Clasificacion.InsertaClasificacion(1, id_servicio, clasif.id_tipo, clasif.id_flota,
                                                                        clasif.id_region, clasif.id_ubicacion_terminal, clasif.id_tipo_servicio,
                                                                        clasif.id_alcance_servicio, clasif.id_detalle_negocio, clasif.id_clasificacion1,
                                                                        clasif.id_clasificacion2, id_usuario);

                                //Personalizando resultado en caso de error
                                if (!resultado.OperacionExitosa)
                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Clasificación ({0}); {1}", clasif.id_clasificacion, resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                    }

                    //Si no hay errores encontrados (paradas, eventos, productos y clasificación)
                    if (resultado.OperacionExitosa)
                    {
                        //Buscando requerimientos aplicables al servicio maestro
                        using (DataTable tblRequerimientos = ServicioRequerimiento.CargaRequerimientosServicio(this._id_servicio))
                        {
                            //Validando origen de datos
                            if (Validacion.ValidaOrigenDatos(tblRequerimientos))
                            {
                                //Para cada requerimiento encontrado
                                foreach (DataRow rRequerimiento in tblRequerimientos.Rows)
                                {
                                    //Insertando copia de requerimiento
                                    resultado = ServicioRequerimiento.InsertaServicioRequerimiento(Convert.ToInt32(rRequerimiento["IdRequerimiento"]), id_servicio, Convert.ToInt32(rRequerimiento["IdTabla"]),
                                                                            Convert.ToInt32(rRequerimiento["IdRegistro"]), rRequerimiento["Descripcion"].ToString(), Convert.ToInt32(rRequerimiento["IdTablaObjetivo"]),
                                                                            Convert.ToInt32(rRequerimiento["IdCampoObjetivo"]), rRequerimiento["ValorObjetivo"].ToString(), rRequerimiento["Condicion"].ToString(), id_usuario);

                                    //Si existe algún error
                                    if (!resultado.OperacionExitosa)
                                    {
                                        //Personalizando error
                                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Requerimiento ({0}){1}; {2}", rRequerimiento["Id"], rRequerimiento["Descripcion"], resultado.Mensaje), resultado.OperacionExitosa);
                                        //Se interrumpe el ciclo de copia de productos
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //Si no hay errores encontrados (paradas, eventos, productos, clasificación y requerimiento)
                    if (resultado.OperacionExitosa)
                    {
                        //Cargando datos de facturación existentes (encabezado)
                        using (Facturacion.Facturado factura = Facturacion.Facturado.ObtieneFacturaServicio(this._id_servicio))
                        {
                            //Si la facturación existe
                            if (factura.id_factura > 0)
                            {
                                //Realizando copia a nuevo servicio
                                resultado = Facturacion.Facturado.InsertaFactura(id_servicio, id_usuario);

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Guardando Id de Factura
                                    int id_factura = resultado.IdRegistro;
                                    //Cargando los detalles de la factura
                                    using (DataTable tblCargos = Facturacion.FacturadoConcepto.ObtieneConceptosFactura(factura.id_factura))
                                    {
                                        //Verificando que existen detalles por copiar
                                        if (Validacion.ValidaOrigenDatos(tblCargos))
                                        {
                                            //Para cada cargo (detalle de factura)
                                            foreach (DataRow rCargo in tblCargos.Rows)
                                            {
                                                //Realizando copia de detalle
                                                resultado = Facturacion.FacturadoConcepto.InsertaFacturaConcepto(id_factura, Convert.ToDecimal(rCargo["Cantidad"]), Convert.ToByte(rCargo["IdUnidad"]),
                                                                                                rCargo["Identificador"].ToString(), Convert.ToInt32(rCargo["IdConceptoCobro"]), Convert.ToDecimal(rCargo["ValorUnitario"]),
                                                                                                Convert.ToDecimal(rCargo["ImportePesos"]), Convert.ToByte(rCargo["IdImpuestoRetenido"]), Convert.ToDecimal(rCargo["TasaImpuestoRetenido"]), Convert.ToByte(rCargo["IdImpuestoTrasladado"]), Convert.ToDecimal(rCargo["TasaImpuestoTrasladado"]),
                                                                                                Convert.ToInt32(rCargo["IdCargoRecurrente"]), id_usuario);
                                                //Si existe algún error
                                                if (!resultado.OperacionExitosa)
                                                {
                                                    //Personalizando error
                                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Cargo ({0}){1}; {2}", rCargo["Id"], rCargo["ConceptoCobro"], resultado.Mensaje), resultado.OperacionExitosa);
                                                    //Se interrumpe el ciclo de copia de productos
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    //Personalizando error
                                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar Encabezado de Factura ({0}){1}; {2}", factura.id_factura, factura.no_factura, resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                    }

                    //Si no hay errores de copia en información operativa
                    if (resultado.OperacionExitosa)
                    {
                        //Obteniendo id de tipo de referencias
                        int id_tipo_no_viaje = ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "No. Viaje", 0, "Referencia de Viaje");
                        int id_tipo_confirmacion = ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "Confirmación", 0, "Referencia de Viaje");

                        //Realizando inserción de referencias principales
                        //Si se especificó número de viaje (Ref Cliente)
                        if (no_viaje.Trim() != "" && id_tipo_no_viaje > 0)
                            //Insertando referencia de viaje
                            resultado = Referencia.InsertaReferencia(id_servicio, 1, id_tipo_no_viaje, no_viaje, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                        else if (no_viaje.Trim() != "")
                            resultado = new RetornoOperacion("No pudo ser localizado el Tipo de Referencia 'No. Viaje' para esta compañía.");

                        //Si no hay errorres
                        if (resultado.OperacionExitosa && no_confirmacion.Trim() != "" && id_tipo_confirmacion > 0)
                            //Insertando confirmación
                            resultado = Referencia.InsertaReferencia(id_servicio, 1, id_tipo_confirmacion, no_confirmacion, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                        else if (resultado.OperacionExitosa && no_confirmacion.Trim() != "")
                            resultado = new RetornoOperacion("No pudo ser localizado el Tipo de Referencia 'Confirmación' para esta compañía.");
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Finalizando transacción
                        transaccion.Complete();
                        //Crenado resultado con Id de Servicio nuevo
                        resultado = new RetornoOperacion(id_servicio);
                    }
                }
            }
            //De lo contrario
            else
                resultado = new RetornoOperacion("El servicio no puede ser copiado, no es un servicio maestro.");

            //Devolviendo resultado
            return resultado;
        }



        /// <summary>
        ///  Método encargado de Copiar un Servicio ligado a una Publicación
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="no_viaje">No Viaje</param>
        /// <param name="no_confirmacion">No Confirmación</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_producto_carga">Id Producto Carga</param>
        /// <param name="cantidad_producto">Cantidad Producto</param>
        /// <param name="peso_producto">Peso Ptroducto</param>
        /// <param name="total_cobro">Total Cobro</param>
        /// <param name="id_concepto_cobro">Id Concepto Cobro</param>
        /// <param name="mit_paradas">Paradas</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion CopiarServicioPublicacion(int id_compania_emisor, int id_cliente, string no_viaje, string no_confirmacion, string observacion, int id_producto_carga,
                                    decimal cantidad_producto, decimal peso_producto, decimal total_cobro, int id_concepto_cobro,  DataTable mit_paradas, int id_unidad, out int id_movimiento, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            id_movimiento = 0;
            //Inicializando bloque transaccional
            using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando variable principal, la mantendrá el nuevo Id de servicio disponible
                int id_servicio = 0;

                //Validando existencia de paradas 
                if (Validacion.ValidaOrigenDatos(mit_paradas))
                {
                    //Seleccionando primer par de paradas del servicio
                    DataRow p1 = (from DataRow r in mit_paradas.Rows
                                  orderby Convert.ToDecimal(r["noSecuencia"]) ascending
                                  select r).FirstOrDefault();
                    DataRow p2 = (from DataRow r in mit_paradas.Rows
                                  orderby Convert.ToDecimal(r["noSecuencia"]) descending
                                  select r).FirstOrDefault();


                    //Insertando nuevo encabezado de servicio
                    resultado = InsertarServicio("", 2, 0, false, 0, id_compania_emisor, id_cliente,Convert.ToInt32(p1["idUbicacionDestino"]), Convert.ToDateTime(p1["cita"]),
                                            Convert.ToInt32(p2["idUbicacionDestino"]),Convert.ToDateTime(p2["cita"]), "", no_viaje, Fecha.ObtieneFechaEstandarMexicoCentro(), observacion,
                                             id_usuario);

                    //Si no hay error al registrar nuevo servicio
                    if (resultado.OperacionExitosa)
                    {
                        //Conservando Id de Servicio nuevo
                        id_servicio = resultado.IdRegistro;

                        //Insertammos las paradas de servicio
                        resultado = Parada.InsertaParadasInicialesServicio(id_servicio, Convert.ToInt32(p1["idUbicacionDestino"]), Convert.ToDateTime(p1["cita"]),Convert.ToInt32(p2["idUbicacionDestino"]), Convert.ToDateTime(p2["cita"]), 0, id_compania_emisor, id_usuario);

                        //Obrenemos Movimiento
                     using(Movimiento objMovimiento = new Movimiento(id_servicio, 1))
                        {
                         //Establecemos Id Movimiento
                            id_movimiento = objMovimiento.id_movimiento;
                        }
                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //Recuperando primer parada del servicio nuevo
                            using (Parada paradaNueva = new Parada(1, id_servicio))
                            {
                                //Copiando eventos de parada
                                resultado = ParadaEvento.InsertaParadaEvento(id_servicio, paradaNueva.id_parada, 0, Convert.ToInt32(p1["idTipoEvento"]), id_usuario);
                                //Si no hay problemas de copia
                                if (resultado.OperacionExitosa)
                                {
                                    //SE SOBREESCRIBE PRODUCTO DE PARADA DE CARGA
                                    resultado = ServicioProducto.InsertaServicioProducto(id_servicio, paradaNueva.id_parada, 1, id_producto_carga, cantidad_producto,
                                                                                    15, peso_producto, 18, id_usuario);
                                }
                            }
                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                            {
                                //Recuperando segunda parada del servicio nuevo
                                using (Parada paradaNueva = new Parada(2, id_servicio))
                                {
                                    //Copiando eventos de parada
                                    resultado = ParadaEvento.InsertaParadaEvento(id_servicio, paradaNueva.id_parada, 0, Convert.ToInt32(p2["idTipoEvento"]), id_usuario);

                                    //Si es servicio solo posee 2 paradass
                                    if (mit_paradas.Rows.Count <= 2)
                                    {
                                        //SE AGREGA PRODUCTO DE PARADA DE DESCARGA
                                        resultado = ServicioProducto.InsertaServicioProducto(id_servicio, paradaNueva.id_parada,2, id_producto_carga,  cantidad_producto,
                                                                                        15, -peso_producto, 18, id_usuario);
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion(string.Format("Error al copiar productos de parada  (1): {0}", resultado.Mensaje));

                            //Si no hay problemas en la copia de ambas paradas
                            if (resultado.OperacionExitosa)
                            {
                                //Si es necesario copiar más paradas (el servicio posee más de dos)
                                if (mit_paradas.Rows.Count > 2)
                                {
                                    //Realizando ciclo de inserción
                                    foreach (DataRow p in (from DataRow r in mit_paradas.Rows
                                                           where Convert.ToDecimal(r["noSecuencia"]) != Convert.ToDecimal(p1["noSecuencia"]) && Convert.ToDecimal(r["noSecuencia"]) !=  Convert.ToDecimal(p2["noSecuencia"])
                                                           orderby Convert.ToDecimal(r["noSecuencia"] ) ascending
                                                           select r).DefaultIfEmpty())
                                    {

                                        //Registrando nueva parada
                                        resultado = Parada.NuevaParadaServicio(id_servicio, Convert.ToDecimal(p["noSecuencia"]), Parada.TipoParada.Operativa,
                                                                   Convert.ToInt32(p["idUbicacionDestino"]), Microsoft.SqlServer.Types.SqlGeography.Null,
                                                                   Convert.ToDateTime(p["cita"]), 0, id_usuario);

                                        //Si no hay error al insertar parada (y sus segmentos y movimientos)
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Recuperando Id de Parada
                                            int id_parada = resultado.IdRegistro;

                                            //Copiando eventos de parada
                                            resultado = ParadaEvento.InsertaParadaEvento(id_servicio, id_parada, 0, Convert.ToInt32(p["idTipoEvento"]), id_usuario);

                                            //DONE: SE OMITE COPIA DE PRODUCTOS DEBIDO A SOBREESCRITURA DE PRODUCTO DE CARGA
                                        }

                                        //Si hay error en algún punto
                                        if (!resultado.OperacionExitosa)
                                        {
                                            //Personalizando error
                                            resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar parada ({0}){1}: {2}", p["Secuencia"], p["Descripcion"], resultado.Mensaje), resultado.OperacionExitosa);
                                            //Se interrumpe el ciclo de copia de paradas
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al copiar primer segmento de servicio: {0}", resultado.Mensaje), resultado.OperacionExitosa);
                        }
                        else
                            resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Error al registrar encabezado de servicio: {0}", resultado.Mensaje), resultado.OperacionExitosa);


                        //Si no hay errores encontrados (paradas, eventos y productos)
                        if (resultado.OperacionExitosa)
                        {

                            //Insertamos clasificación
                            resultado = Global.Clasificacion.InsertaClasificacion(1, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_usuario);

                        }

                        //Si no hay errores encontrados (paradas, eventos, productos, clasificación )
                        if (resultado.OperacionExitosa)
                        {
                            //Insertaamos Factura
                            resultado = Facturacion.Facturado.InsertaFactura(id_servicio, id_usuario);

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                            {
                                //Guardando Id de Factura
                                int id_factura = resultado.IdRegistro;

                                //Realizando copia de detalle
                                resultado = Facturacion.FacturadoConcepto.InsertaFacturaConcepto(id_factura, 1, 2, "", id_concepto_cobro,total_cobro, 2, 4, 3, 16,
                                                                                                0, id_usuario);

                            }
                        }

                        //Si no hay errores de copia en información operativa
                        if (resultado.OperacionExitosa)
                        {
                            //Obteniendo id de tipo de referencias
                            int id_tipo_no_viaje = ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania_emisor, 1, "No. Viaje", 0, "Referencia de Viaje");
                            int id_tipo_confirmacion = ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania_emisor, 1, "Confirmación", 0, "Referencia de Viaje");

                            //Realizando inserción de referencias principales
                            //Si se especificó número de viaje (Ref Cliente)
                            if (no_viaje.Trim() != "" && id_tipo_no_viaje > 0)
                                //Insertando referencia de viaje
                                resultado = Referencia.InsertaReferencia(id_servicio, 1, id_tipo_no_viaje, no_viaje, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                            else if (no_viaje.Trim() != "")
                                resultado = new RetornoOperacion("No pudo ser localizado el Tipo de Referencia 'No. Viaje' para esta compañía.");

                            //Si no hay errorres
                            if (resultado.OperacionExitosa && no_confirmacion.Trim() != "" && id_tipo_confirmacion > 0)
                                //Insertando confirmación
                                resultado = Referencia.InsertaReferencia(id_servicio, 1, id_tipo_confirmacion, no_confirmacion, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                            else if (resultado.OperacionExitosa && no_confirmacion.Trim() != "")
                                resultado = new RetornoOperacion("No pudo ser localizado el Tipo de Referencia 'Confirmación' para esta compañía.");
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //Finalizando transacción
                            transaccion.Complete();
                            //Crenado resultado con Id de Servicio nuevo
                            resultado = new RetornoOperacion(id_servicio);
                        }
                    }

                }
            }

            //Devolviendo resultado
            return resultado;
        }
 
        
        /// <summary>
        /// Actualiza la fecha de cita de carga del servicio
        /// </summary>
        /// <param name="cita_carga">Cita de carga</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCitaCarga(DateTime cita_carga, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizando edición de cita de carga
                resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                                this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, cita_carga,
                                this._id_ubicacion_descarga, this._cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion, this._observacion_servicio, id_usuario, this._habilitar);
                if (resultado.OperacionExitosa)
                {
                    using (ServicioImportacionDetalle sid = ServicioImportacionDetalle.ObtieneDetalleServicio(this._id_servicio))
                    {
                        if (sid.habilitar)
                            //Actualizando Cita de Carga
                            resultado = sid.ActualizaServicioProduccion(this._id_servicio, cita_carga, sid.cita_descarga,
                                                    sid.operador, sid.unidad, sid.no_viaje, id_usuario);
                        if (resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion(this._id_servicio);
                            scope.Complete();
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Actualiza la fecha de cita de descarga del servicio
        /// </summary>
        /// <param name="cita_descarga">Cita de descarga</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCitaDescarga(DateTime cita_descarga, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizando edición de cita de carga
                resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos, this._id_servicio_base, this._bit_servicio_base,
                            this._id_cotizacion, (Estatus)this._id_estatus, this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                            this._id_ubicacion_descarga, cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion, this._observacion_servicio, id_usuario, this._habilitar);
                if (resultado.OperacionExitosa)
                {
                    using (ServicioImportacionDetalle sid = ServicioImportacionDetalle.ObtieneDetalleServicio(this._id_servicio))
                    {
                        if (sid.habilitar)
                            //Actualizando Cita de Carga
                            resultado = sid.ActualizaServicioProduccion(this._id_servicio, sid.cita_carga, cita_descarga,
                                                    sid.operador, sid.unidad, sid.no_viaje, id_usuario);
                        if (resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion(this._id_servicio);
                            scope.Complete();
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Busca servicios maestros que coincidan con los criterios solicitados
        /// </summary>
        /// <param name="id_compania">Id de Compañía</param>
        /// <param name="id_cliente">Id de Cliente</param>
        /// <param name="id_ubicacion_origen">Id de Origen del servicio</param>
        /// <param name="id_ubicacion_destino">Id de Destino del servicio</param>
        /// <returns></returns>
        public static DataTable CargaServiciosMaestros(int id_compania, int id_cliente, int id_ubicacion_origen, int id_ubicacion_destino)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 6, 0, "", 0, "", 0, 0, true, 0, 0, id_compania, id_cliente, id_ubicacion_origen, null, id_ubicacion_destino, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Metodo encargado de cargar los indicadores relacionados con los servicios documentados
        /// </summary>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static DataTable CargaIndicadoresServicios(int id_compania)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 7, 0, "", 0, "", 0, 0, true, 0, 0, id_compania, 0, 0, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Terminamos Servicio
        /// </summary>
        /// <param name="tipo_actualizacion_salida">Tipo de actualización de Salida de la Parada (Manual, GPS)</param>
        /// <param name="tipo_actualizacion_fin">Tipo actualización fin del evento (Manual, GPS)</param>
        /// <param name="id_usuario">Id usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaServicio(DateTime fecha_termino, Parada.TipoActualizacionSalida tipo_actualizacion_salida, ParadaEvento.TipoActualizacionFin tipo_actualizacion_fin, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos Estatus del Servicio
            if ((Estatus)this._id_estatus == Estatus.Iniciado)
            {
                //Instanciamos última parada del servicio en cuestión
                    using (Parada objParada = new Parada(Parada.ObtieneUltimaParada(this._id_servicio)))
                    {
                        //Validamos Ultima Parada
                        if (objParada.habilitar)
                        {
                            //Validamos que la parada se encuentre Iniciada
                            if ((Parada.EstatusParada)objParada.id_estatus_parada == Parada.EstatusParada.Iniciado)
                            {
                                //Validando asignación de referencia o número de viaje en encabezado
                                    resultado = validarNoViajeCliente();

                                    //Si no hay problemas con validación de viaje de cliente
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Validando asignación de referencia o número de viaje en encabezado
                                        resultado = validaCostoViaje();

                                        //Si no hay problemas con validación de costo de viaje
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Creamos la transacción 
                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {
                                                //Terminamos Parada
                                                resultado = objParada.TerminaParada(fecha_termino, tipo_actualizacion_salida, tipo_actualizacion_fin, 0, id_usuario);

                                                //Validamos Termino de la Parada
                                                if (resultado.OperacionExitosa && objParada.ActualizaParada())
                                                {
                                                    //Obtenemos Último Movimiento
                                                    using (Movimiento objMovimiento = new Movimiento(Movimiento.BuscamosMovimientoParadaDestino(this._id_servicio, objParada.id_parada)))
                                                    {
                                                        //Si el movimiento fue localizado
                                                        if (objMovimiento.habilitar && objParada.Estatus == Parada.EstatusParada.Terminado)
                                                            
                                                            //Actualizando estatus de recursos utilizados en el último movimiento
                                                            resultado = MovimientoAsignacionRecurso.ActualizaEstatusRecursosTerminadosADisponibleTerminoServicio(objMovimiento.id_movimiento, objParada.id_parada, fecha_termino, id_usuario);
                                                        else
                                                            //Si no se localizó el movimiento
                                                            resultado = new RetornoOperacion("Último Movimiento no encontrado o la Ultima parada no ha finalizado.");
                                                    }
                                                    
                                                    //Validamos Actualiación de Id de parada en estancias
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validamos que exista un Servicio Control Evidencia
                                                        using (ServicioControlEvidencia objServicioControl = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, this._id_servicio))
                                                        {
                                                            //Validmos que exista el Servicio Control Evidencia
                                                            if (objServicioControl.id_servicio_control_evidencia <= 0)
                                                            
                                                                //Insertamos Servicio Control Evidencia
                                                                resultado = ServicioControlEvidencia.InsertaServicioControlEvidencia(this._id_servicio, fecha_termino, id_usuario);
                                                        }

                                                        //Validamos Resultado
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Instanciamos Servicio despacho
                                                            using (ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                                                            {
                                                                //Actualizamos Valores Generales
                                                                resultado = objServicioDespacho.ActualizaParadaDestinoServicio(objParada.id_parada, fecha_termino, id_usuario);
                                                                
                                                                //Validamos Actualización
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Validando que el Servicio se pueda terminar
                                                                    resultado = Parada.ValidaTerminoServicio(this._id_servicio);

                                                                    //Si no hay paradas Pendientes por Terminar a excepción de la Ultima
                                                                    if (resultado.OperacionExitosa)
                                                                    
                                                                        //Editamos Estatus Servicio a Terminado
                                                                        resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos,
                                                                                                  this._id_servicio_base, this._bit_servicio_base, this._id_cotizacion, Estatus.Terminado,
                                                                                                  this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                                                                                  this._id_ubicacion_descarga, this._cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion,
                                                                                                  this._observacion_servicio, id_usuario, this._habilitar);
                                                                    
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            //////Validamos que se realiza la operaciones anteriores para poder mandar los datos WEBSERVICE
                                            //if (resultado.OperacionExitosa)
                                            //{
                                            //    //metodo para verificar si cuenta con el proveedor 
                                            //    resultado = SAT_CL.Monitoreo.ProveedorWSUnidad.ObtieneDespachoActualUnidad(this._id_servicio, 0,1,0);
                                            //    resultado = new RetornoOperacion(true);
                                            //}
                                        else
                                                    resultado = new RetornoOperacion(string.Format("Error al terminar última parada del servicio: {0}", resultado.Mensaje));

                                                //Validamos Resultado
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Instanciando retorno en Servicio
                                                    resultado = new RetornoOperacion(this._id_servicio);
                                                    //Completando Transacción
                                                    scope.Complete();
                                                }
                                            }
                                        }
                                    }
                            }
                            else
                                //Establecemos mensaje error
                                resultado = new RetornoOperacion("Ingrese la Fecha de Llegada de la última Parada.");
                        }
                        else
                            //Establecemos mensaje error
                            resultado = new RetornoOperacion("No se encontró datos complementarios de la última Parada.");
                    }
            }
            else
                //Establecemos errror
                resultado = new RetornoOperacion("El estatus del Servicio no permite su edición.");

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la re apertura del Servicio para su modificación
        /// </summary>
        /// <param name="id_usuario">Id usuario</param>
        /// <returns></returns>
        public RetornoOperacion ReversaTerminaServicio(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos Estatus del Servicio
            if ((Estatus)this._id_estatus == Estatus.Terminado)
            {
                //Instanciamos última parada del servicio en cuestión
                using (Parada objParada = new Parada(Parada.ObtieneUltimaParada(this._id_servicio)))
                {
                    //Validamos Ultima Parada
                    if (objParada.id_parada > 0)
                    {
                        //Validamos que la parada se encuentre Terminada
                        if ((Parada.EstatusParada)objParada.id_estatus_parada == Parada.EstatusParada.Terminado)
                        {
                            //Obteniendo el ultimo movimiento asociado al servicio (donde la ultima parada sea el destino)
                            using (Movimiento movimientoFinal = new Movimiento(Movimiento.BuscamosMovimientoParadaDestino(this._id_servicio, objParada.id_parada)))
                            {
                                //Si el movimiento fue encontrado
                                if (movimientoFinal.id_movimiento > 0)
                                {
                                    //Validando que el movimiento cuente con recursos disponibles en su última parada
                                    resultado = MovimientoAsignacionRecurso.ValidaRecursosParaReversaTerminaMovimiento(movimientoFinal.id_movimiento);

                                    //Si los recursos están disponibles
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Creamos la transacción 
                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {
                                            //Iniciando la Parada
                                            resultado = objParada.IniciaParada(objParada.fecha_llegada, objParada.TipoActualizacionLlegada_, objParada.id_razon_llegada_tarde, id_usuario);

                                            //Validamos Reinicio de la Parada
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Obteniendo las asignaciones terminadas de el movimiento final
                                                using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.CargaAsignacionesTerminadas(movimientoFinal.id_movimiento))
                                                {
                                                    //Si hay asignaciones de recurso
                                                    if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                                                    {
                                                        //Para cada una de las asignaciones
                                                        foreach (DataRow r in mitAsignaciones.Rows)
                                                        {
                                                            //Determinando el tipo de asignación
                                                            switch ((MovimientoAsignacionRecurso.Tipo)Convert.ToInt32(r["TipoAsignacion"]))
                                                            {
                                                                //Unidades
                                                                case MovimientoAsignacionRecurso.Tipo.Unidad:
                                                                    //Instanciando la unidad 
                                                                    using (Unidad unidad = new Unidad(Convert.ToInt32(r["IdRecursoAsignado"])))
                                                                    {
                                                                        //Si la unidad fue recuperada
                                                                        if (unidad.id_unidad > 0)
                                                                        {
                                                                            //Instanciando la estancia actual del recurso
                                                                            using (EstanciaUnidad estanciaActual = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(unidad.id_unidad)))
                                                                            {
                                                                                //Si la estancia fue localizada
                                                                                if (estanciaActual.id_estancia_unidad > 0)
                                                                                    //Actualziando Id de Parada en la estancia (parada final del servicio)
                                                                                    resultado = estanciaActual.CambiaParadaEstanciaUnidad(objParada.id_parada, id_usuario);
                                                                                //Si no se encontró la estancia
                                                                                else
                                                                                    resultado = new RetornoOperacion(string.Format("Estancia actual de la unidad '{0}' no encontrada.", unidad.numero_unidad));

                                                                                //Si la estancia fue actualizada correctamente
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Actualizando estatus de unidad
                                                                                    resultado = unidad.ActualizaEstatusUnidad(Unidad.Estatus.ParadaOcupado, id_usuario);
                                                                                    //Si se actualizó el estatus de la unidad
                                                                                    if (resultado.OperacionExitosa && unidad.ActualizaAtributosInstancia())
                                                                                        //Actualizando estancia de la unidad
                                                                                        resultado = unidad.ActualizaEstanciaYMovimiento(estanciaActual.id_estancia_unidad, 0, objParada.fecha_llegada, id_usuario);
                                                                                    else
                                                                                        resultado = new RetornoOperacion(string.Format("Error al actualizar estatus de unidad '{0}': {1}", unidad.numero_unidad, resultado.Mensaje));
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                            //Si no se localizó la unidad
                                                                            resultado = new RetornoOperacion(string.Format("No pudo ser recuperada la información de la unidad Id '{0}'.", r["IdRecursoAsignado"]));
                                                                    }
                                                                    break;
                                                                //Operadores
                                                                case MovimientoAsignacionRecurso.Tipo.Operador:
                                                                    //Instanciando operador
                                                                    using (Operador operador = new Operador(Convert.ToInt32(r["IdRecursoAsignado"])))
                                                                    {
                                                                        //Si el operador fue localizado
                                                                        if (operador.id_operador > 0)
                                                                        {
                                                                            //Actualizando su estatus
                                                                            resultado = operador.ActualizaEstatus(Operador.Estatus.Ocupado, id_usuario);
                                                                            //Si se actualizó correctamente el estatus
                                                                            if (resultado.OperacionExitosa && operador.ActualizaAtributosInstancia())
                                                                                //Actualizando parada actual
                                                                                resultado = operador.ActualizaParadaYMovimiento(objParada.id_parada, 0, objParada.fecha_llegada, id_usuario);
                                                                            else
                                                                                resultado = new RetornoOperacion(string.Format("Error al actualizar estatus de operador '{0}': {1}", operador.nombre, resultado.Mensaje));
                                                                        }
                                                                        //Si no se localizó
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("No pudo ser recuperada la información del operador Id '{0}'.", r["IdRecursoAsignado"]));
                                                                    }
                                                                    break;
                                                            }

                                                            //Si existe algún error
                                                            if (!resultado.OperacionExitosa)
                                                                break;
                                                        }
                                                    }
                                                    //De lo contrario
                                                    else
                                                        resultado = new RetornoOperacion("No se recuperaron las asignaciones terminadas del último movimiento.");
                                                }

                                                //Validamos Actualiación de Iestancias y uso de operadores y unidades
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Instanciamos Servicio despacho
                                                    using (ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                                                    {
                                                        //Actualizamos Valores Generales de despacho (sin parada destino)
                                                        resultado = objServicioDespacho.ActualizaParadaDestinoServicio(0, DateTime.MinValue, id_usuario);
                                                        //Validamos Actualización
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Editamos Estatus Servicio a Iniciado
                                                            resultado = editarServicio(this._prefijo_servicio, this._consecutivo_compania_emisor, this._id_origen_datos,
                                                                                      this._id_servicio_base, this._bit_servicio_base, this._id_cotizacion, Estatus.Iniciado,
                                                                                      this._id_compania_emisor, this._id_cliente_receptor, this._id_ubicacion_carga, this._cita_carga,
                                                                                      this._id_ubicacion_descarga, this._cita_descarga, this._porte, this._referencia_cliente, this._fecha_documentacion,
                                                                                      this._observacion_servicio, id_usuario, this._habilitar);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                resultado = new RetornoOperacion(string.Format("Error al terminar última parada del servicio: {0}", resultado.Mensaje));

                                            //Validamos Resultado
                                            if (resultado.OperacionExitosa)
                                                scope.Complete();
                                        }
                                    }
                                }
                                //Si el movimiento final no se encontró
                                else
                                    resultado = new RetornoOperacion("Movimiento final no localizado.");
                            }
                        }
                        else
                            //Establecemos mensaje error
                            resultado = new RetornoOperacion("La parada final del servicio no se encuentra terminada.");
                    }
                    else
                        //Establecemos mensaje error
                        resultado = new RetornoOperacion("No se encontró datos complementarios de la última Parada del Servicio.");
                }

            }
            else
                //Establecemos errror
                resultado = new RetornoOperacion("Esta operación no es válida debido al estatus actual del servicio.");

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Calculamos Kilometraje del  Servicio
        /// </summary>
        /// <param name="id_usuario">Id Servicio</param>
        /// <returns></returns>
        public RetornoOperacion CalculaKilometrajeServicio(int id_usuario)
         {
            //Declaramos Variables
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos Movimientos
                using (DataTable mit = Movimiento.CargaMovimientos(this._id_servicio))
                {
                    //Validando origen de datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recorremos cada una de los movimientos
                        foreach (DataRow r in mit.Rows)
                        {
                            //Validamos Kilometraje
                            if (resultado.OperacionExitosa)
                                //Calculamos Kilometraje
                                resultado = Movimiento.CalculaKilometrajeMovimiento(Convert.ToInt32(r["Id"]), id_usuario);
                            else
                                //Salimos del Ciclo
                                break;
                        }

                        //Validamos calculo de kilometraje de los movimientos
                        if (resultado.OperacionExitosa)
                        {
                            //Declaramos Vaiables para Obtener Kilometraje
                            decimal total_kms_reales = 0;

                            //Obtenemos Kilometraje Total
                            Movimiento.ObtieneKilometrajeTotal(this._id_servicio, out total_kms_reales);

                            //Declaramos Variables
                            decimal total_kms_recorridos = 0; decimal total_kms_cargados_recorridos = 0; decimal total_kms_vacios_recorridos = 0; decimal total_kms_troncos_recorridos = 0;

                            //Si el servicio  ya se encuentra Iniciado
                            if ((Servicio.Estatus)this._id_estatus == Estatus.Iniciado)
                            {
                                //Obtenemos Kilometraje de Movimientos Terminados
                                Movimiento.ObtieneKilometrajeTotalRecorrido(this._id_servicio, out total_kms_recorridos, out total_kms_cargados_recorridos,
                                                                         out total_kms_vacios_recorridos, out total_kms_troncos_recorridos);
                            }
                            //Instanciamos Sevicio Despacho a Partir del Id de Servicio
                            using (ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                            {
                                //Validamos Existencia de Servicio Despacho
                                if (objServicioDespacho.id_servicio_despacho == 0)
                                {
                                    //Insertamos Servicio Despacho 
                                    resultado = ServicioDespacho.InsertaServicioDespacho(this._id_servicio, DateTime.MinValue, DateTime.MinValue,
                                                                                        0, 0, 0, 0, total_kms_reales, 0, 0, 0, 0, 0, 0, 0, 0, id_usuario);
                                }
                                else
                                {
                                    //Editamos Servicio Despacho
                                    resultado = objServicioDespacho.EditaServicioDespacho(objServicioDespacho.id_servicio, objServicioDespacho.fecha_inicio, objServicioDespacho.fecha_fin,
                                                                                          objServicioDespacho.id_parada_origen, objServicioDespacho.id_parada_destino, objServicioDespacho.id_parada_carga_inicio,
                                                                                          objServicioDespacho.id_parada_carga_fin, total_kms_reales, total_kms_recorridos,
                                                                                          total_kms_cargados_recorridos, total_kms_vacios_recorridos,
                                                                                          total_kms_troncos_recorridos, objServicioDespacho.id_unidad_motriz_principal, objServicioDespacho.id_unidad_arrastre1,
                                                                                           objServicioDespacho.id_unidad_arrastre2, objServicioDespacho.id_tercero, id_usuario);
                                }
                                //Si la Edición de Servicio Despacho
                                if (resultado.OperacionExitosa)
                                {
                                    //Devolvemos Id de Servicio
                                    resultado = new RetornoOperacion(this._id_servicio);
                                }
                            }
                        }
                    }
                }

                //Si el resultado es exitoso
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Calculamos Kilometraje del Servicio cuando se realiza la modificacion del kilometraje de un movimiento
        /// </summary>
        /// <param name="id_usuario">Id Servicio</param>
        /// <returns></returns>
        public RetornoOperacion CalculaKilometrajeServicio(int id_movimiento, int id_usuario)
        {
            //Declaramos Variables
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Calculamos Kilometraje
                resultado = Movimiento.ActualizaKilometrajeMovimiento(id_movimiento, id_usuario);

                //Validamos calculo de kilometraje del Movimiento
                if (resultado.OperacionExitosa)
                {
                    //Declaramos Vaiables para Obtener Kilometraje
                    decimal total_kms_reales = 0;

                    //Obtenemos Kilometraje Total
                    Movimiento.ObtieneKilometrajeTotal(this._id_servicio, out total_kms_reales);

                    //Declaramos Variables
                    decimal total_kms_recorridos = 0; decimal total_kms_cargados_recorridos = 0; decimal total_kms_vacios_recorridos = 0; decimal total_kms_troncos_recorridos = 0;

                    //Si el servicio  ya se encuentra Iniciado
                    if ((Servicio.Estatus)this._id_estatus == Estatus.Iniciado)
                    {
                        //Obtenemos Kilometraje de Movimientos Terminados
                        Movimiento.ObtieneKilometrajeTotalRecorrido(this._id_servicio, out total_kms_recorridos, out total_kms_cargados_recorridos,
                                                                    out total_kms_vacios_recorridos, out total_kms_troncos_recorridos);
                    }
                    //Instanciamos Servicio Despacho a Partir del Id de Servicio
                    using (ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                    {
                        //Validamos Existencia de Servicio Despacho
                        if (objServicioDespacho.id_servicio_despacho == 0)
                        {
                            //Insertamos Servicio Despacho 
                            resultado = ServicioDespacho.InsertaServicioDespacho(this._id_servicio, DateTime.MinValue, DateTime.MinValue,
                                                                                0, 0, 0, 0, total_kms_reales, 0, 0, 0, 0, 0, 0, 0, 0, id_usuario);
                        }
                        else
                        {
                            //Editamos Servicio Despacho
                            resultado = objServicioDespacho.EditaServicioDespacho(objServicioDespacho.id_servicio, objServicioDespacho.fecha_inicio, objServicioDespacho.fecha_fin,
                                                                                    objServicioDespacho.id_parada_origen, objServicioDespacho.id_parada_destino, objServicioDespacho.id_parada_carga_inicio,
                                                                                    objServicioDespacho.id_parada_carga_fin, total_kms_reales, total_kms_recorridos,
                                                                                    total_kms_cargados_recorridos, total_kms_vacios_recorridos,
                                                                                    total_kms_troncos_recorridos, objServicioDespacho.id_unidad_motriz_principal, objServicioDespacho.id_unidad_arrastre1,
                                                                                    objServicioDespacho.id_unidad_arrastre2, objServicioDespacho.id_tercero, id_usuario);
                        }
                        //Si la Edición de Servicio Despacho
                        if (resultado.OperacionExitosa)
                        {
                            //Devolvemos Id de Servicio
                            resultado = new RetornoOperacion(this._id_servicio);
                        }
                    }
                }

                //Si el resultado es exitoso
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Obtenemos Resultado
            return resultado;
        }
        
        /// <summary>
        /// Determina los criterios de búsqueda definidos para aplicación de una tarifa sobre un servicio en particular (valores de columna y fila)
        /// </summary>
        /// <param name="columna">Criterio de filtrdo sobre columnas de matriz de tarifa</param>
        /// <param name="fila">Criterio de filtrado sobre filas de matriz de tarifa</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa</param>
        /// <param name="descripcion_columna">Rótulo de Columna</param>
        /// <param name="descripcion_fila">Rótulo de Fila</param>
        /// <param name="operador_columna">Operador de búsqueda en columna</param>
        /// <param name="operador_fila">Operador de búsqueda en fila</param>
        public RetornoOperacion ExtraeCriteriosMatrizTarifa(Tarifas.Tarifa.CriterioMatrizTarifa columna, Tarifas.Tarifa.CriterioMatrizTarifa fila, int id_base_tarifa, out string descripcion_columna, out string descripcion_fila, out string operador_columna, out string operador_fila)
        {
            //Declarando objeto de resultado, sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);

            //Asignando valores por default a parámetros de salida
            descripcion_columna = descripcion_fila = operador_columna = operador_fila = "";

            //Determinando el tipo de filtrado columna aplicable
            switch (columna)
            {
                case Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                    //Obteniendo el producto correspondiente
                    int id_producto = ServicioProducto.ObtieneProductoPrincipal(this._id_servicio);

                    //Si el producto existe
                    if (id_producto > 0)
                        descripcion_columna = id_producto.ToString();
                    else
                        resultado = new RetornoOperacion("No existe ningún producto asignado al servicio, no se puede aplicar tarifa por este criterio.");

                    //Asignando operador de comparación
                    operador_columna = "=";
                    break;
                case Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                    using (SAT_CL.Global.Ubicacion ubicacion = new Global.Ubicacion(this._id_ubicacion_descarga))
                        descripcion_columna = ubicacion.id_ciudad.ToString();
                    operador_columna = "=";
                    break;
                case Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                    descripcion_columna = this._id_ubicacion_descarga.ToString();
                    operador_columna = "=";
                    break;
                case Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                    using (ServicioDespacho total = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                    {
                        descripcion_columna = total.ObtieneKilometrajeCobro().ToString();
                        operador_columna = "<=";

                        //Si no hay kilometros
                        if (total.kms_asignados <= 0)
                            resultado = new RetornoOperacion("El kilometraje calculado es igual a 0, no se puede aplicar tarifa por este criterio.");
                    }
                    break;
                case Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                    descripcion_columna = Parada.ObtieneTotalParadas(this._id_servicio).ToString();
                    operador_columna = "<=";
                    break;
                case Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                    //Obteniendo volumen total
                    decimal volumen = ServicioProducto.ObtieneTotalVolumenServicio(this._id_servicio, id_base_tarifa);

                    //Si el volumen es mayor a 0
                    if (volumen > 0)
                        descripcion_columna = volumen.ToString();
                    else
                        resultado = new RetornoOperacion("El volúmen transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<=";
                    break;
                case Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                    //Obteniendo el peso total
                    decimal peso = ServicioProducto.ObtieneTotalPesoServicio(this._id_servicio, id_base_tarifa);

                    //Si el peso es mayor a 0
                    if (peso > 0)
                        descripcion_columna = peso.ToString();
                    else
                        resultado = new RetornoOperacion("El peso transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<=";
                    break;
                case Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                    //Obteniendo el conteo total de productos
                    decimal cantidad = ServicioProducto.ObtieneTotalCantidadServicio(this._id_servicio);

                    //Si la cantidad es mayor a 0
                    if (cantidad > 0)
                        descripcion_columna = cantidad.ToString();
                    else
                        resultado = new RetornoOperacion("El conteo transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<="; break;
                default:
                    descripcion_columna = "0";
                    operador_columna = "=";
                    break;
            }

            //Si no hay error de búsqueda de criterios hasta este punto
            if (resultado.OperacionExitosa)
            {
                //Determinando el tipo de filtrado de fila aplicable
                switch (fila)
                {
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                        //Obteniendo el producto correspondiente
                        int id_producto = ServicioProducto.ObtieneProductoPrincipal(this._id_servicio);

                        //Si el producto existe
                        if (id_producto > 0)
                            descripcion_fila = id_producto.ToString();
                        else
                            resultado = new RetornoOperacion("No existe ningún producto asignado al servicio, no se puede aplicar tarifa por este criterio.");

                        //Asignando operador de comparación
                        operador_fila = "=";
                        break;
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                        using (SAT_CL.Global.Ubicacion ubicacion = new Global.Ubicacion(this._id_ubicacion_carga))
                            descripcion_fila = ubicacion.id_ciudad.ToString();
                        operador_fila = "=";
                        break;
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                        descripcion_fila = this._id_ubicacion_carga.ToString();
                        operador_fila = "=";
                        break;
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                        using (ServicioDespacho total = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                        {
                            descripcion_fila = total.kms_asignados.ToString();
                            operador_fila = "<=";

                            //Si no hay kilometros
                            if (total.kms_asignados <= 0)
                                resultado = new RetornoOperacion("El kilometraje calculado es igual a 0, no se puede aplicar tarifa por este criterio.");
                        }
                        break;
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                        descripcion_fila = Parada.ObtieneTotalParadas(this._id_servicio).ToString();
                        operador_fila = "<=";
                        break;
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                        //Obteniendo volumen total
                        decimal volumen = ServicioProducto.ObtieneTotalVolumenServicio(this._id_servicio, id_base_tarifa);

                        //Si el volumen es mayor a 0
                        if (volumen > 0)
                            descripcion_fila = volumen.ToString();
                        else
                            resultado = new RetornoOperacion("El volúmen transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                        //Obteniendo el peso total
                        decimal peso = ServicioProducto.ObtieneTotalPesoServicio(this._id_servicio, id_base_tarifa);

                        //Si el peso es mayor a 0
                        if (peso > 0)
                            descripcion_fila = peso.ToString();
                        else
                            resultado = new RetornoOperacion("El peso transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    case Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                        //Obteniendo el conteo total de productos
                        decimal cantidad = ServicioProducto.ObtieneTotalCantidadServicio(this._id_servicio);

                        //Si la cantidad es mayor a 0
                        if (cantidad > 0)
                            descripcion_fila = cantidad.ToString();
                        else
                            resultado = new RetornoOperacion("El conteo transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    default:
                        descripcion_fila = "0";
                        operador_fila = "=";
                        break;
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Determina los criterios de búsqueda definidos para aplicación de una tarifa de pago sobre un servicio en particular (valores de columna y fila)
        /// </summary>
        /// <param name="columna">Criterio de filtrdo sobre columnas de matriz de tarifa</param>
        /// <param name="fila">Criterio de filtrado sobre filas de matriz de tarifa</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa</param>
        /// <param name="descripcion_columna">Rótulo de Columna</param>
        /// <param name="descripcion_fila">Rótulo de Fila</param>
        /// <param name="operador_columna">Operador de búsqueda en columna</param>
        /// <param name="operador_fila">Operador de búsqueda en fila</param>
        public RetornoOperacion ExtraeCriteriosMatrizTarifaPago(TarifasPago.Tarifa.CriterioMatrizTarifa columna, TarifasPago.Tarifa.CriterioMatrizTarifa fila, int id_base_tarifa, out string descripcion_columna, out string descripcion_fila, out string operador_columna, out string operador_fila)
        {
            //Declarando objeto de resultado, sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);

            //Asignando valores por default a parámetros de salida
            descripcion_columna = descripcion_fila = operador_columna = operador_fila = "";

            //Determinando el tipo de filtrado columna aplicable
            switch (columna)
            {
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                    //Obteniendo el producto correspondiente
                    int id_producto = ServicioProducto.ObtieneProductoPrincipal(this._id_servicio);

                    //Si el producto existe
                    if (id_producto > 0)
                        descripcion_columna = id_producto.ToString();
                    else
                        resultado = new RetornoOperacion("No existe ningún producto asignado al servicio, no se puede aplicar tarifa por este criterio.");

                    //Asignando operador de comparación
                    operador_columna = "=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                    using (SAT_CL.Global.Ubicacion ubicacion = new Global.Ubicacion(this._id_ubicacion_descarga))
                        descripcion_columna = ubicacion.id_ciudad.ToString();
                    operador_columna = "=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                    descripcion_columna = this._id_ubicacion_descarga.ToString();
                    operador_columna = "=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                    using (ServicioDespacho total = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                    {
                        descripcion_columna = total.ObtieneKilometrajePago().ToString();
                        operador_columna = "<=";

                        //Si no hay kilometros
                        if (total.kms_asignados <= 0)
                            resultado = new RetornoOperacion("El kilometraje calculado es igual a 0, no se puede aplicar tarifa por este criterio.");
                    }
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                    descripcion_columna = Parada.ObtieneTotalParadas(this._id_servicio).ToString();
                    operador_columna = "<=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                    //Obteniendo volumen total
                    decimal volumen = ServicioProducto.ObtieneTotalVolumenServicio(this._id_servicio, id_base_tarifa);

                    //Si el volumen es mayor a 0
                    if (volumen > 0)
                        descripcion_columna = volumen.ToString();
                    else
                        resultado = new RetornoOperacion("El volúmen transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                    //Obteniendo el peso total
                    decimal peso = ServicioProducto.ObtieneTotalPesoServicio(this._id_servicio, id_base_tarifa);

                    //Si el peso es mayor a 0
                    if (peso > 0)
                        descripcion_columna = peso.ToString();
                    else
                        resultado = new RetornoOperacion("El peso transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                    //Obteniendo el conteo total de productos
                    decimal cantidad = ServicioProducto.ObtieneTotalCantidadServicio(this._id_servicio);

                    //Si la cantidad es mayor a 0
                    if (cantidad > 0)
                        descripcion_columna = cantidad.ToString();
                    else
                        resultado = new RetornoOperacion("El conteo transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<="; break;
                default:
                    descripcion_columna = "0";
                    operador_columna = "=";
                    break;
            }

            //Si no hay error de búsqueda de criterios hasta este punto
            if (resultado.OperacionExitosa)
            {
                //Determinando el tipo de filtrado de fila aplicable
                switch (fila)
                {
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                        //Obteniendo el producto correspondiente
                        int id_producto = ServicioProducto.ObtieneProductoPrincipal(this._id_servicio);

                        //Si el producto existe
                        if (id_producto > 0)
                            descripcion_fila = id_producto.ToString();
                        else
                            resultado = new RetornoOperacion("No existe ningún producto asignado al servicio, no se puede aplicar tarifa por este criterio.");

                        //Asignando operador de comparación
                        operador_fila = "=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                        using (SAT_CL.Global.Ubicacion ubicacion = new Global.Ubicacion(this._id_ubicacion_carga))
                            descripcion_fila = ubicacion.id_ciudad.ToString();
                        operador_fila = "=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                        descripcion_fila = this._id_ubicacion_carga.ToString();
                        operador_fila = "=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                        using (ServicioDespacho total = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                        {
                            descripcion_fila = total.kms_asignados.ToString();
                            operador_fila = "<=";

                            //Si no hay kilometros
                            if (total.kms_asignados <= 0)
                                resultado = new RetornoOperacion("El kilometraje calculado es igual a 0, no se puede aplicar tarifa por este criterio.");
                        }
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                        descripcion_fila = Parada.ObtieneTotalParadas(this._id_servicio).ToString();
                        operador_fila = "<=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                        //Obteniendo volumen total
                        decimal volumen = ServicioProducto.ObtieneTotalVolumenServicio(this._id_servicio, id_base_tarifa);

                        //Si el volumen es mayor a 0
                        if (volumen > 0)
                            descripcion_fila = volumen.ToString();
                        else
                            resultado = new RetornoOperacion("El volúmen transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                        //Obteniendo el peso total
                        decimal peso = ServicioProducto.ObtieneTotalPesoServicio(this._id_servicio, id_base_tarifa);

                        //Si el peso es mayor a 0
                        if (peso > 0)
                            descripcion_fila = peso.ToString();
                        else
                            resultado = new RetornoOperacion("El peso transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                        //Obteniendo el conteo total de productos
                        decimal cantidad = ServicioProducto.ObtieneTotalCantidadServicio(this._id_servicio);

                        //Si la cantidad es mayor a 0
                        if (cantidad > 0)
                            descripcion_fila = cantidad.ToString();
                        else
                            resultado = new RetornoOperacion("El conteo transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    default:
                        descripcion_fila = "0";
                        operador_fila = "=";
                        break;
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene de acuerdo a la unidad de medida predeterminada de la base de tarifa, la cantidad de elementos a considerar para la aplicación del flete al servicio
        /// </summary>
        /// <param name="id_base_tarifa">Base usada en la tarifa </param>
        /// <param name="id_tipo_cargo_base_tarifa">Id de Tipo de Cargo predeterminado para registrar el cálculo principal de la tarifa</param>
        public decimal ExtraeCriterioBaseTarifa(int id_base_tarifa, out int id_tipo_cargo_base_tarifa)
        {
            //Declarando objeto de retorno
            decimal cantidad = 0;

            //Usando la base de tarifa aplicable
            using (Tarifas.BaseTarifa base_tarifa = new Tarifas.BaseTarifa(id_base_tarifa))
            {
                //Obteniendo el rubro del servicio a cuantificar
                switch (base_tarifa.base_tarifa)
                {
                    case Tarifas.BaseTarifa.Base.Distancia:
                        //Obteniendo totales de servicio
                        using (ServicioDespacho total = new ServicioDespacho( ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                            cantidad = total.ObtieneKilometrajeCobro();
                        break;
                    case Tarifas.BaseTarifa.Base.Fijo:
                        //Cantidad Fija a 1
                        cantidad = 1;
                        break;
                    case Tarifas.BaseTarifa.Base.Peso:
                        cantidad = ServicioProducto.ObtieneTotalPesoServicio(this._id_servicio, id_base_tarifa);
                        break;
                    case Tarifas.BaseTarifa.Base.Volumen:
                        cantidad = ServicioProducto.ObtieneTotalVolumenServicio(this._id_servicio, id_base_tarifa);
                        break;
                    case Tarifas.BaseTarifa.Base.Tiempo:
                        break;
                }

                //Buscando el tipo de cargo predeterminado para la base y la compañía solictante
                using (Tarifas.TipoCargo tc = Tarifas.TipoCargo.ObtieneTipoCargoBaseTarifa(this._id_compania_emisor, id_base_tarifa))
                    id_tipo_cargo_base_tarifa = tc.id_tipo_cargo;
            }

            //Devolviendo resultado
            return cantidad;
        }
        /// <summary>
        /// Obtiene de acuerdo a la unidad de medida predeterminada de la base de tarifa, la cantidad de elementos a considerar para el pago del servicio
        /// </summary>
        /// <param name="id_base_tarifa">Base usada en la tarifa </param>
        /// <param name="id_tipo_cargo_base_tarifa">Id de Tipo de Cargo predeterminado para registrar el cálculo principal de la tarifa</param>
        public decimal ExtraeCriterioBaseTarifaPago(int id_base_tarifa, out int id_tipo_cargo_base_tarifa)
        {
            //Declarando objeto de retorno
            decimal cantidad = 0;

            //Usando la base de tarifa aplicable
            using (TarifasPago.BaseTarifa base_tarifa = new TarifasPago.BaseTarifa(id_base_tarifa))
            {
                //Obteniendo el rubro del servicio a cuantificar
                switch (base_tarifa.base_tarifa)
                {
                    case TarifasPago.BaseTarifa.Base.Distancia:
                        //Obteniendo totales de servicio
                        using (ServicioDespacho total = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                            cantidad = total.kms_asignados;
                        break;
                    case TarifasPago.BaseTarifa.Base.Fijo:
                        //Cantidad Fija a 1
                        cantidad = 1;
                        break;
                    case TarifasPago.BaseTarifa.Base.Peso:
                        cantidad = ServicioProducto.ObtieneTotalPesoServicio(this._id_servicio, id_base_tarifa);
                        break;
                    case TarifasPago.BaseTarifa.Base.Volumen:
                        cantidad = ServicioProducto.ObtieneTotalVolumenServicio(this._id_servicio, id_base_tarifa);
                        break;
                    case TarifasPago.BaseTarifa.Base.Tiempo:
                        break;
                }

                //Buscando el tipo de cargo predeterminado para la base y la compañía solictante
                using (Liquidacion.TipoPago tp = Liquidacion.TipoPago.ObtieneTipoPagoBaseTarifa(this._id_compania_emisor, id_base_tarifa))
                    id_tipo_cargo_base_tarifa = tp.id_tipo_pago;
            }

            //Devolviendo resultado
            return cantidad;
        }
        /// <summary>
        /// Método  encargado de Actualizar los atributos del Servicio
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicio()
        {
            return this.cargarAtributosInstancia(this._id_servicio);
        }

       
        /// <summary>
        /// Metodo encargado de cargar los datos requeridos para la impresión de carta porte
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        public static DataTable CargaDatosPorte(int id_servicio)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 8, id_servicio, "", 0, "", 0, 0, true, 0, 0, 0, 0, 0, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Metodo encargado de cargar los datos referentes a paradas
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        public static DataTable CargaDatosParadas(int id_servicio)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 9, id_servicio, "", 0, "", 0, 0, true, 0, 0, 0, 0, 0, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Metodo encargado de cargar los datos referentes  a la Llegeda de la Parada
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada">Parada Actual</param>
        /// <returns></returns>
        public static DataTable ObtieneInformacionLlegaParadaEMail(int id_servicio, int id_parada)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 10, id_servicio, "", 0, "", 0, 0, true, 0, 0, 0, 0, 0, null, 0, null, "", "", null, "", 0, false, null, id_parada, "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// Metodo encargado de cargar los datos referentes a la Salida de la Parada
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada">Parada Actual</param>
        /// <returns></returns>
        public static DataTable ObtieneInformacionSalidaParadaEMail(int id_servicio, int id_parada)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 11, id_servicio, "", 0, "", 0, 0, true, 0, 0, 0, 0, 0, null, 0, null, "", "", null, "", 0, false, null, id_parada, "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Metodo Encargado de Mostrar los datos requeridos para impresion de Carta Porte-Traslado
        /// con regularizacion de 16/12/2015
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        public static DataTable CargaDatosPorteRegularizacion16122015(int id_servicio)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 12, id_servicio, "", 0, "", 0, 0, true, 0, 0, 0, 0, 0, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Metodo Encargado de Mostrar los datos requeridos para impresion de Carta Porte-Traslado
        /// con regularizacion de 16/12/2015
        /// </summary>
        /// <param name="id_servicio"></param>
        /// 
        /// <returns></returns>
        public static DataTable CargaDatosPorteRegularizacion16122015(int id_servicio, int id_operador, int id_unidad_motriz, int id_unidad_arrastre1, int id_unidad_arrastre2)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 12, id_servicio, "", id_unidad_motriz, "", 0, 0, true, 0, 0, id_unidad_arrastre1, id_unidad_arrastre2, id_operador, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Definiendo objeto de resultado
                DataTable mit = null;

                //Si existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de validar la especificación de la Impresión de la Carta Porte
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        public static RetornoOperacion ValidaImpresionEspecificaCartaPorte(int id_servicio)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion(id_servicio);

            //Instanciando Servicio
            using (Servicio serv = new Servicio(id_servicio))
            {
                //Validando Servicio
                if (serv.habilitar)
                {
                    //Obteniendo los Recursos de Terceros
                    using (DataTable dtTerceros = CapaNegocio.m_capaNegocio.CargaCatalogo(190, "", serv.id_servicio, "", 0, ""))
                    {
                        //Si existe un recurso de Terceros
                        if (Validacion.ValidaOrigenDatos(dtTerceros))

                            //Instanciando Excepción
                            retorno = new RetornoOperacion("El Servicio es despachado por un Tercero, no requiere parametros de impresión.");
                        else
                        {
                            //Obteniendo los Recursos de Operadores
                            using (DataTable dtOperadores = CapaNegocio.m_capaNegocio.CargaCatalogo(188, "", serv.id_servicio, "", 0, ""))
                            {
                                //Validando Existencia de Operadores
                                if (Validacion.ValidaOrigenDatos(dtOperadores))
                                {
                                    //Validando que existan más de un Operador
                                    if (dtOperadores.Rows.Count > 1)

                                        //Instanciando Retorno Positivo
                                        retorno = new RetornoOperacion(serv.id_servicio, "El Servicio tiene mas de un Operador.", true);
                                    else
                                        //Instanciando Excepción
                                        retorno = new RetornoOperacion("El Servicio solo tiene un Operador.");
                                }
                                else
                                    //Instanciando Excepción
                                    retorno = new RetornoOperacion("No hay Operadores Asignados.");

                                //Validando Operación
                                if (!retorno.OperacionExitosa)
                                {
                                    //Obteniendo los Recursos de Unidades Motrices
                                    using (DataTable dtUnidadesMotrices = CapaNegocio.m_capaNegocio.CargaCatalogo(189, "", serv.id_servicio, "", 1, ""))
                                    {
                                        //Validando Existencia de varias Unidades
                                        if (Validacion.ValidaOrigenDatos(dtUnidadesMotrices))
                                        {
                                            //Validando que existan más de una Unidad Motriz
                                            if (dtUnidadesMotrices.Rows.Count > 1)

                                                //Instanciando Retorno Positivo
                                                retorno = new RetornoOperacion(serv.id_servicio, "El Servicio tiene mas de una Unidad motriz.", true);
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion("El Servicio solo tiene una Unidad Motriz.");
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No hay Unidades Motrices Asignadas.");

                                        //Validando Operación
                                        if (!retorno.OperacionExitosa)
                                        {
                                            //Obteniendo los Recursos de Unidades de Arrastre
                                            using (DataTable dtUnidadesArrastres = CapaNegocio.m_capaNegocio.CargaCatalogo(189, "", serv.id_servicio, "", 2, ""))
                                            {
                                                //Validando Existencia de varias Unidades
                                                if (Validacion.ValidaOrigenDatos(dtUnidadesArrastres))
                                                {
                                                    //Validando que existan más de una Unidad de Arrastre
                                                    if (dtUnidadesArrastres.Rows.Count > 2)

                                                        //Instanciando Retorno Positivo
                                                        retorno = new RetornoOperacion(serv.id_servicio, "El Servicio tiene mas de una Unidad de Arrastre.", true);
                                                    else
                                                        //Instanciando Excepción
                                                        retorno = new RetornoOperacion("El Servicio solo tiene una Unidad de Arrastre.");
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    retorno = new RetornoOperacion("No hay Unidades de Arrastre Asignadas.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el Servicio.");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion TerminoAutomaticoServicioTercero(int id_servicio, int id_usuario, bool termino_auto)
        {
            RetornoOperacion retorno = new RetornoOperacion();

            using (Servicio serv = new Servicio(id_servicio))
            {
                if (serv.habilitar)
                {
                    //Inicializando Bloque Transacional
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Obteniendo Paradas
                        int id_ultima_parada = Despacho.Parada.ObtieneUltimaParada(serv.id_servicio);
                        using (DataTable dtParadas = Despacho.Parada.CargaParadasParaVisualizacion(id_servicio))
                        {
                            if (Validacion.ValidaOrigenDatos(dtParadas))
                            {
                                foreach (DataRow dr in dtParadas.Rows)
                                {
                                    using (Parada parada = new Parada(Convert.ToInt32(dr["Id"])))
                                    {
                                        if (parada.habilitar)
                                        {
                                            if (parada.Estatus == Parada.EstatusParada.Registrado)
                                            {
                                                retorno = parada.ActualizarFechaLlegada(parada.cita_parada, Parada.TipoActualizacionLlegada.Manual,
                                                            0, EstanciaUnidad.TipoActualizacionInicio.Manual, id_usuario);
                                                if (retorno.OperacionExitosa && parada.ActualizaParada())
                                                {
                                                    if (parada.id_parada == id_ultima_parada && serv.ActualizaServicio())
                                                    {
                                                        if(termino_auto)
                                                        retorno = serv.TerminaServicio(parada.cita_parada.AddMinutes(4), Parada.TipoActualizacionSalida.Manual,
                                                                            ParadaEvento.TipoActualizacionFin.SinActualizar, id_usuario);
                                                    }
                                                    else
                                                    {
                                                        DateTime fecha_salida = DateTime.MinValue;
                                                        using (Parada parada_proxima = new Parada(parada.secuencia_parada_servicio + 1, serv.id_servicio))
                                                        {
                                                            if (parada_proxima.habilitar)
                                                                fecha_salida = parada.cita_parada.AddMinutes((parada_proxima.cita_parada - parada.cita_parada).TotalMinutes / 2);
                                                            else
                                                                retorno = new RetornoOperacion("No se puede recuperar la siguiente parada");

                                                            if (fecha_salida != DateTime.MinValue)
                                                                retorno = parada.ActualizarFechaSalida(fecha_salida, Parada.TipoActualizacionSalida.Manual,
                                                                            EstanciaUnidad.TipoActualizacionFin.Manual, ParadaEvento.TipoActualizacionFin.SinActualizar,
                                                                            0, id_usuario);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                retorno = new RetornoOperacion("La Parada esta Terminada, imposible su Edición");
                                        }
                                        else
                                            retorno = new RetornoOperacion("No se puede recuperar la Parada");
                                    }

                                    //Terminando Ciclo en caso de Error
                                    if (!retorno.OperacionExitosa)
                                        break;
                                }
                            }
                            else
                                retorno = new RetornoOperacion("No se pueden recuperar las Parada del Servicio");

                            if (retorno.OperacionExitosa)
                            {
                                retorno = new RetornoOperacion(serv.id_servicio);
                                scope.Complete();
                            }
                        }
                    }
                }
                else
                    retorno = new RetornoOperacion("No se puede recuperar el Servicio");
            }

            return retorno;
        }
        /// <summary>
        /// Metodo Encargado de Mostrar los datos requeridos para impresion de Carta Porte-Traslado
        /// con regularizacion de 16/12/2015
        /// </summary>
        /// <param name="id_servicio"></param>
        /// 
        /// <returns></returns>
        public static DataSet CargaDatosPorteViajera(int id_servicio, int id_operador, int id_unidad_motriz, int id_unidad_arrastre1, int id_unidad_arrastre2)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 14, id_servicio, "", id_unidad_motriz, "", 0, 0, true, 0, 0, id_unidad_arrastre1, id_unidad_arrastre2, id_operador, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            //Devolviendo resultado
             return ds;
        }
        /// <summary>
        /// Metodo Encargado de Mostrar los datos requeridos para impresion de Carta Porte-Traslado
        /// con regularizacion de 16/12/2015
        /// </summary>
        /// <param name="id_servicio"></param>
        /// 
        /// <returns></returns>
        public static DataSet CargaDatosHojaInstruccion(int id_servicio, int id_operador, int id_unidad_motriz, int id_unidad_arrastre1, int id_unidad_arrastre2)
        {
            //Armando conjunto de criterios de búsqueda
            object[] param = { 15, id_servicio, "", id_unidad_motriz, "", 0, 0, true, 0, 0, id_unidad_arrastre1, id_unidad_arrastre2, id_operador, null, 0, null, "", "", null, "", 0, false, null, "", "" };

            //Realizando búsqueda de servicios
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
                //Devolviendo resultado
                return ds;
        }
        #endregion
    }
}
