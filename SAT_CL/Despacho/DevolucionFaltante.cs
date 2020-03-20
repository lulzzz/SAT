using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con las Devoluciones
    /// </summary>
    public class DevolucionFaltante : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa la Devolución
        /// </summary>
        public enum TipoDevolucion
        {
            /// <summary>
            /// Expresa la Devolución
            /// </summary>
            Devolucion = 1,
            /// <summary>
            /// Expresa que Existe Producto Faltante
            /// </summary>
            Faltante,
            /// <summary>
            /// Expresa que Existe Producto Sobrante
            /// </summary>
            Sobrante,
            /// <summary>
            /// Expresa que Existe Producto Rechazado
            /// </summary>
            Rechazo
        }
        /// <summary>
        /// Enumeración que expresa la Devolución
        /// </summary>
        public enum EstatusDevolucion
        {
            /// <summary>
            /// Estatus Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Estatus en Proceso
            /// </summary>
            EnProceso,
            /// <summary>
            /// Estatus Terminado
            /// </summary>
            Terminado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "despacho.sp_devolucion_faltante_tdf";

        private int _id_devolucion_faltante;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Devolución
        /// </summary>
        public int id_devolucion_faltante { get { return this._id_devolucion_faltante; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        private int _consecutivo_compania;
        /// <summary>
        /// Atributo encargado de Almacenar el Número Consecutivo por Compania
        /// </summary>
        public int consecutivo_compania { get { return this._consecutivo_compania; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de Almacenar el Servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private int _id_movimiento;
        /// <summary>
        /// Atributo encargado de Almacenar el Movimiento
        /// </summary>
        public int id_movimiento { get { return this._id_movimiento; } }
        private int _id_parada;
        /// <summary>
        /// Atributo encargado de Almacenar la Parada
        /// </summary>
        public int id_parada { get { return this._id_parada; } }
        private byte _id_tipo;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Devolución
        /// </summary>
        public byte id_tipo { get { return this._id_tipo; } }
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Devolución (Enumeración)
        /// </summary>
        public TipoDevolucion tipo { get { return (TipoDevolucion)this._id_tipo; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus de la Devolución
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus de la Devolución
        /// </summary>
        public EstatusDevolucion estatus { get { return (EstatusDevolucion)this._id_estatus; } }
        private DateTime _fecha_captura;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Captura
        /// </summary>
        public DateTime fecha_captura { get { return this._fecha_captura; } }
        private DateTime _fecha_devolucion_faltante;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Devolución
        /// </summary>
        public DateTime fecha_devolucion_faltante { get { return this._fecha_devolucion_faltante; } }
        private string _observacion;
        /// <summary>
        /// Atributo encargado de Almacenar la Observación de la Devolución
        /// </summary>
        public string observacion { get { return this._observacion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Contructores

        /// <summary>
        /// Constructor que Inicializa los Valores por Defecto
        /// </summary>
        public DevolucionFaltante()
        {
            //Asignando Valores
            this._id_devolucion_faltante = 0;
            this._id_compania_emisora = 0;
            this._consecutivo_compania = 0;
            this._id_servicio = 0;
            this._id_movimiento = 0;
            this._id_parada = 0;
            this._id_tipo = 0;
            this._id_estatus = 0;
            this._fecha_captura = DateTime.MinValue;
            this._fecha_devolucion_faltante = DateTime.MinValue;
            this._observacion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que Inicializa los Valores dado un Registro
        /// </summary>
        /// <param name="id_devolucion_faltante">Devolución Faltante</param>
        public DevolucionFaltante(int id_devolucion_faltante)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_devolucion_faltante);
        }
        /// <summary>
        /// Constructor que Inicializa los Valores dado un Registro
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_parada">Parada</param>
        public DevolucionFaltante(int id_servicio, int id_movimiento, int id_parada)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_servicio, id_movimiento, id_parada);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~DevolucionFaltante()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos de la Clase 
        /// </summary>
        /// <param name="id_devolucion_faltante">Devolución Faltante</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_devolucion_faltante)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_devolucion_faltante, 0, 0, 0, 0, 0, 0, 0, null, null, "", 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_devolucion_faltante = id_devolucion_faltante;
                        this._id_compania_emisora = Convert.ToInt32(r["IdCompaniaEmisora"]);
                        this._consecutivo_compania = Convert.ToInt32(r["ConsecutivoCompania"]);
                        this._id_servicio = Convert.ToInt32(r["IdServicio"]);
                        this._id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        this._id_parada = Convert.ToInt32(r["IdParada"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        DateTime.TryParse(r["FechaCaptura"].ToString(), out this._fecha_captura);
                        DateTime.TryParse(r["FechaDevolucionFaltante"].ToString(), out this._fecha_devolucion_faltante);
                        this._observacion = r["Observacion"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Cargar los Atributos de la Clase 
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_parada">Parada</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_servicio, int id_movimiento, int id_parada)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, id_servicio, id_movimiento, id_parada, 0, 0, null, null, "", 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_devolucion_faltante = Convert.ToInt32(r["Id"]);
                        this._id_compania_emisora = Convert.ToInt32(r["IdCompaniaEmisora"]);
                        this._consecutivo_compania = Convert.ToInt32(r["ConsecutivoCompania"]);
                        this._id_servicio = Convert.ToInt32(r["IdServicio"]);
                        this._id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        this._id_parada = Convert.ToInt32(r["IdParada"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        DateTime.TryParse(r["FechaCaptura"].ToString(), out this._fecha_captura);
                        DateTime.TryParse(r["FechaDevolucionFaltante"].ToString(), out this._fecha_devolucion_faltante);
                        this._observacion = r["Observacion"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="consecutivo_compania">Número Consecutivo por Compania</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_parada">Parada</param>
        /// <param name="id_tipo">Tipo</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="fecha_captura">Fecha de Captura</param>
        /// <param name="fecha_devolucion_faltante">Fecha de Devolución</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_compania_emisora, int consecutivo_compania, int id_servicio, int id_movimiento, int id_parada, byte id_tipo, byte id_estatus, 
                                                        DateTime fecha_captura, DateTime fecha_devolucion_faltante, string observacion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_devolucion_faltante, id_compania_emisora, consecutivo_compania, id_servicio, id_movimiento, id_parada, id_tipo, id_estatus, 
                                 fecha_captura, fecha_devolucion_faltante, observacion, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Devoluciones Faltantes
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="consecutivo_compania">Número Consecutivo por Compania</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_parada">Parada</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="fecha_captura">Fecha de Captura</param>
        /// <param name="fecha_devolucion_faltante">Fecha de Devolución</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDevolucionesFaltantes(int id_compania_emisora, int consecutivo_compania, int id_servicio, int id_movimiento, int id_parada, TipoDevolucion tipo, EstatusDevolucion estatus,
                                                        DateTime fecha_captura, DateTime fecha_devolucion_faltante, string observacion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania_emisora, consecutivo_compania, id_servicio, id_movimiento, id_parada, (byte)tipo, (byte)estatus, 
                                 fecha_captura, fecha_devolucion_faltante, observacion, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Devoluciones Faltantes
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="consecutivo_compania">Número Consecutivo por Compania</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_parada">Parada</param>
        /// <param name="tipo">Tipo</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="fecha_captura">Fecha de Captura</param>
        /// <param name="fecha_devolucion_faltante">Fecha de Devolución</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaDevolucionesFaltantes(int id_compania_emisora, int consecutivo_compania, int id_servicio, int id_movimiento, int id_parada, TipoDevolucion tipo, EstatusDevolucion estatus,
                                                        DateTime fecha_captura, DateTime fecha_devolucion_faltante, string observacion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idDevolucion = 0;

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invocando Método de Actualización
                result = this.actualizaRegistrosBD(id_compania_emisora, consecutivo_compania, id_servicio, id_movimiento, id_parada, (byte)tipo, (byte)estatus,
                                                 fecha_captura, fecha_devolucion_faltante, observacion, id_usuario, this._habilitar);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Asignando Devolución
                    idDevolucion = result.IdRegistro;
                    
                    //Validando Estatus
                    if ((EstatusDevolucion)this._id_estatus != estatus)
                    {
                        //Obteniendo Detalles
                        using (DataTable dtDetalles = DevolucionFaltanteDetalle.ObtieneDetallesDevolucion(this._id_devolucion_faltante))
                        {
                            //Validando que Existan Detalles
                            if(Validacion.ValidaOrigenDatos(dtDetalles))
                            {
                                //Recorriendo Detalles
                                foreach(DataRow dr in dtDetalles.Rows)
                                {
                                    //Instanciando Detalles
                                    using (DevolucionFaltanteDetalle detalle = new DevolucionFaltanteDetalle(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista el Detalle
                                        if (detalle.habilitar)
                                        {
                                            //Actualizando Estatus del Detalle
                                            result = detalle.ActualizaEstatusDevolucionFaltanteDetalle((DevolucionFaltanteDetalle.EstatusDevolucionDetalle)((byte)estatus), id_usuario);

                                            //Validando si hubo Error
                                            if (!result.OperacionExitosa)

                                                //Terminando Ciclo
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Instanciando Devolución
                    result = new RetornoOperacion(idDevolucion);

                    //Completando Transacción
                    trans.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus de la Devolución
        /// </summary>
        /// <param name="estatus">Estatus General</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusDevolucionFaltante(EstatusDevolucion estatus, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistrosBD(this._id_compania_emisora, this._consecutivo_compania, this._id_servicio, this._id_movimiento, this._id_parada, this._id_tipo, (byte)estatus,
                                             this._fecha_captura, this._fecha_devolucion_faltante, this._observacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar las Devoluciones Faltantes
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDevolucionesFaltantes(int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistrosBD(this._id_compania_emisora, this._consecutivo_compania, this._id_servicio, this._id_movimiento, this._id_parada, this._id_tipo, this._id_estatus,
                                             this._fecha_captura, this._fecha_devolucion_faltante, this._observacion, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar la Devolución Faltante
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDevolucionFaltante()
        {
            //Invocando Devolución Faltante
            return this.cargaAtributosInstancia(this._id_devolucion_faltante);
        }
        /// <summary>
        /// Método encargado de Obtener las Devoluciones Ligadas a un Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneDevolucionesServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtDevoluciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, 0, id_servicio, 0, 0, 0, 0, null, null, "", 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtDevoluciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtDevoluciones;
        }
        /// <summary>
        /// Método que permite realizar la busqueda de devoluciones para el reporte de impresión de cajas devolución.
        /// </summary>
        /// <param name="id_devolucion_faltante"></param>
        /// <returns></returns>
        public static DataTable CargaImpresionCajaDevolucion(int id_devolucion_faltante) 
        {
            //Creación del datatable dtCajaDevolución
            DataTable dtCajaDevolucion= null;
            //Creación y asignación de parametros al arreglo param
            object[]param={6,id_devolucion_faltante,0,0,0,0,0,0,0,null,null,"",0,false,"",""};
            //Obtiene los datos 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida el origen de los datos, que existan o no sean nulos                
                if(Validacion.ValidaOrigenDatos(DS,"Table"))
                    dtCajaDevolucion= DS.Tables["Table"];
            }
            //Devuelve el datatable con los datos obtenidos al método
            return dtCajaDevolucion;
        }

        #endregion
    }
}
