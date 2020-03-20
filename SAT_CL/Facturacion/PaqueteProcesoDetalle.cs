using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using SAT_CL.Tarifas;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Documentacion;
using SAT_CL.Global;
using TSDK.Datos;
using System.Transactions;

namespace SAT_CL.Facturacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Detalles de los Procesos
    /// </summary>
    public class PaqueteProcesoDetalle : Disposable
    {
        #region Enumeraciones
        
        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "facturacion.sp_paquete_proceso_detalle_tppd";
        
        private int _id_paquete_proceso_detalle;
        /// <summary>
        /// Atributo que almacena el Id del Detalle del Proceso del Paquete
        /// </summary>
        public int id_paquete_proceso_detalle { get { return this._id_paquete_proceso_detalle; } }
        private int _id_paquete_proceso;
        /// <summary>
        /// Atributo que almacena el Encabezado del Proceso del Paquete
        /// </summary>
        public int id_paquete_proceso { get { return this._id_paquete_proceso; } }
        private int _id_facturado;
        /// <summary>
        /// Atributo que almacena la Factura del Detalle
        /// </summary>
        public int id_facturado { get { return this._id_facturado; } }
        private int _id_facturado_detalle;
        /// <summary>
        /// Atributo que almacena el Id del Detalle del Proceso del Paquete
        /// </summary>
        public int id_facturado_detalle { get { return this._id_facturado_detalle; } }
        private bool _no_entregado;
        /// <summary>
        /// Atributo que almacena el Indicador de Entrega
        /// </summary>
        public bool no_entregado { get { return this._no_entregado; } }
        private bool _rechazado;
        /// <summary>
        /// Atributo que almacena el Indicador de Rechazo
        /// </summary>
        public bool rechazado { get { return this._rechazado; } }
        private DateTime _fecha_actualizacion;
        /// <summary>
        /// Atributo que almacena la Fecha de Actualización
        /// </summary>
        public DateTime fecha_actualizacion { get { return this._fecha_actualizacion; } }
        private string _observacion;
        /// <summary>
        /// Atributo que almacena la Observación
        /// </summary>
        public string observacion { get { return this._observacion; } }
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
        public PaqueteProcesoDetalle()
        {
            //Asignando Valores
            this._id_paquete_proceso_detalle = 0;
            this._id_paquete_proceso = 0;
            this._id_facturado = 0;
            this._id_facturado_detalle = 0;
            this._no_entregado = false;
            this._rechazado = false;
            this._fecha_actualizacion = DateTime.MinValue;
            this._observacion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Contructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_paquete_proceso_detalle">Detalle de Proceso</param>
        public PaqueteProcesoDetalle(int id_paquete_proceso_detalle)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_paquete_proceso_detalle);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~PaqueteProcesoDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos de la Clase dado un Registro
        /// </summary>
        /// <param name="id_paquete_proceso_detalle">Proceso de Paquete</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_paquete_proceso_detalle)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_paquete_proceso_detalle, 0, 0, 0, false, false, null, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada uno de los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_paquete_proceso_detalle = id_paquete_proceso_detalle;
                        this._id_paquete_proceso = Convert.ToInt32(dr["IdPaqueteProceso"]);
                        this._id_facturado = Convert.ToInt32(dr["IdFacturado"]);
                        this._id_facturado_detalle = Convert.ToInt32(dr["IdFacturadoDetalle"]);
                        this._no_entregado = Convert.ToBoolean(dr["NoEntregado"]);
                        this._rechazado = Convert.ToBoolean(dr["Rechazado"]);
                        DateTime.TryParse(dr["FechaActualizacion"].ToString(), out this._fecha_actualizacion);
                        this._observacion = dr["Observacion"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_paquete_proceso">Proceso del Paquete</param>
        /// <param name="id_facturado">Factura</param>
        /// <param name="id_facturado_detalle">Detalle de la Factura</param>
        /// <param name="no_entregado">Indicador de Entrega</param>
        /// <param name="rechazado">Indicador de Rechazo</param>
        /// <param name="fecha_actualizacion">Fecha de Actualización</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_paquete_proceso, int id_facturado, int id_facturado_detalle, bool no_entregado, bool rechazado, 
                                        DateTime fecha_actualizacion, string observacion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_paquete_proceso_detalle, id_paquete_proceso, id_facturado, id_facturado_detalle, no_entregado, rechazado, 
                                 TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_actualizacion), observacion, id_usuario, habilitar, "", "" };

            //Validando que su Proceso no se encuentre Terminado
            if (!this._no_entregado && !this._rechazado)

                //Realizando Actualización
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Proceso de esta Factura esta Terminado, Imposible su Eliminación");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar la Combinación de Factura y Concepto
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <param name="id_detalle_factura">Concepto</param>
        /// <returns></returns>
        private static bool ValidaFacturaConcepto(int id_factura, int id_detalle_factura)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 10, 0, 0, id_factura, id_detalle_factura, false, false, null, "", 0, false, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Resultados
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Asignando Indicador
                        result = Convert.ToInt32(dr["Indicador"]) == 1 ? true : false;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Detalles del Proceso de Paquetes
        /// </summary>
        /// <param name="id_paquete_proceso">Proceso del Paquete</param>
        /// <param name="id_facturado">Factura</param>
        /// <param name="id_facturado_detalle">Detalle de la Factura</param>
        /// <param name="no_entregado">Indicador de Entrega</param>
        /// <param name="rechazado">Indicador de Rechazo</param>
        /// <param name="fecha_actualizacion">Fecha de Actualización</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPaqueteProcesoDetalle(int id_paquete_proceso, int id_facturado, int id_facturado_detalle, bool no_entregado, bool rechazado,
                                        DateTime fecha_actualizacion, string observacion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_paquete_proceso, id_facturado, id_facturado_detalle, no_entregado, rechazado, 
                                 TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_actualizacion), observacion, id_usuario, true, "", "" };

            //Validando la Factura y el Concepto
            if (!PaqueteProcesoDetalle.ValidaFacturaConcepto(id_facturado, id_facturado_detalle))

                //Realizando Actualización
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            else
                //Instanciando Excepción
                result = new RetornoOperacion("La Factura y/o Concepto no estan Disponibles");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Detalles del Proceso de Paquetes
        /// </summary>
        /// <param name="tipo_proceso">Tipo de Proceso</param>
        /// <param name="estatus">Estatus del Proceso</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="consecutivo_compania">Consecutivo por Compania</param>
        /// <param name="id_cliente">Cliente Receptor</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario_responsable">Usuario Responsable del Proceso</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaPaqueteProcesoDetalle(int id_paquete_proceso, int id_facturado, int id_facturado_detalle, bool no_entregado, bool rechazado,
                                        DateTime fecha_actualizacion, string observacion, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(id_paquete_proceso, id_facturado, id_facturado_detalle, no_entregado, rechazado,
                                 fecha_actualizacion, observacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Detalles del Proceso de Paquetes
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPaqueteProcesoDetalle(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(this._id_paquete_proceso, this._id_facturado, this._id_facturado_detalle, this._no_entregado, this._rechazado,
                                     this._fecha_actualizacion, this._observacion, id_usuario, false);
        }

        /// <summary>
        /// Método encargado de Deshabilitar los Detalles del Proceso de Paquetes
        /// </summary>
        /// <param name="no_entregado">Indicador de Falta de Entrega del Detalle</param>
        /// <param name="rechazado">Indicador de Rechazo del Detalle</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaProcesoDetalle(bool no_entregado, bool rechazado, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(this._id_paquete_proceso, this._id_facturado, this._id_facturado_detalle, no_entregado, rechazado,
                                 this._fecha_actualizacion, this._observacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPaqueteProcesoDetalle()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_paquete_proceso);
        }
        /// <summary>
        /// Método encargado de Cargar las Facturas Disponibles
        /// </summary>
        /// <param name="id_paquete">Referencia al Proceso del Paquete</param>
        /// <param name="id_cliente">Cliente al que pertenecen las Facturas</param>
        /// <param name="id_compania">Compania a la que Pertenecen las Facturas</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="carta_porte">Carta Porte</param>
        /// <returns></returns>
        public static DataTable CargaFacturasDisponibles(int id_paquete, int id_cliente, int id_compania, string referencia, string carta_porte)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturasDisponibles = null;

            //Armando arreglo de parametros
            object[] param = { 4, id_paquete, id_cliente, id_compania, 0, false, false, null, "", 0, false, referencia, carta_porte };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacturasDisponibles = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacturasDisponibles;
        }
        /// <summary>
        /// Método Público encargado de Cargar los Conceptos Disponibles de la Factura
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <param name="id_cliente">Cliente al que pertenece la Factura</param>
        /// <returns></returns>
        public static DataTable CargaConceptosDisponibleFactura(int id_factura, int id_cliente)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando arreglo de parametros
            object[] param = { 5, 0, 0, id_factura, id_cliente, false, false, null, "", 0, false, "", "" };

            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Público encargado de Cargar los Conceptos Ligados de la Factura
        /// </summary>
        /// <param name="id_paquete_proceso">Factura Global</param>
        /// <param name="id_factura">Factura</param>
        /// <param name="id_cliente">Cliente al que pertenece la Factura</param>
        /// <returns></returns>
        public static DataTable CargaConceptosLigadosFactura(int id_paquete_proceso, int id_factura, int id_cliente)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Armando arreglo de parametros
            object[] param = { 6, 0, id_paquete_proceso, id_factura, id_cliente, false, false, null, "", 0, false, "", "" };

            //Obteniendo Reporte de Conceptos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dt = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Totales del Paquete
        /// </summary>
        /// <param name="id_paquete_proceso">Proceso del Paquete</param>
        /// <returns></returns>
        public static DataTable ObtieneTotalesProcesoPaquete(int id_paquete_proceso)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 7, 0, id_paquete_proceso, 0, 0, false, false, null, "", 0, false, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Método encargado de Obtener las relación de las Facturas dada una Factura y un Paquete
        /// </summary>
        /// <param name="id_factura">Factura Actual</param>
        /// <param name="id_paquete_proceso">Factura Global Actual</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturacionPaqueteProceso(int id_factura, int id_paquete_proceso)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 8, 0, id_paquete_proceso, id_factura, 0, false, false, null, "", 0, false, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Método encargado de Obtener la Relación de los Paquetes dada la Referencia del Paquete
        /// </summary>
        /// <param name="id_paquete_proceso">Paquete Actual</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturacionPaqueteProceso(int id_paquete_proceso)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 9, 0, id_paquete_proceso, 0, 0, false, false, null, "", 0, false, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Método encargado de Cargar las Facturas Ligadas a un Proceso de Revisión
        /// </summary>
        /// <param name="id_paquete">Referencia al Proceso del Paquete</param>
        /// <returns></returns>
        public static DataTable CargaFacturasLigadas(int id_paquete)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturasDisponibles = null;
            
            //Armando arreglo de parametros
            object[] param = { 11, id_paquete, 0, 0, 0, false, false, null, "", 0, false, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacturasDisponibles = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacturasDisponibles;
        }

        #endregion
    }
}
