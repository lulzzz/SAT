using System;
using System.Data;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// 
    /// </summary>
    public class Requisicion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración encargada de Describir el Estatus
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// La Requisición sólo se ha capturado
            /// </summary>
            Registrada = 1,
            /// <summary>
            /// La requisición está pendiente de autorización para su abastecimiento
            /// </summary>
            PorAutorizar = 2,
            /// <summary>
            /// La requisición se encuentra pendiente de abastecimiento
            /// </summary>
            Solicitada = 3,
            /// <summary>
            /// La requisición se ha abastecido de forma parcial, puede continuar siendo abastecida
            /// </summary>
            AbastecidaParcial = 4,
            /// <summary>
            /// La requisición se ha terminado de abastecer (parcial o totalmente)
            /// </summary>
            Cerrada = 5,            
            /// <summary>
            /// La requisición no llegará a ser abastecida
            /// </summary>
            Cancelada = 6
        }
        /// <summary>
        /// Enumeración encargada de Describir el Tipo
        /// </summary>
        public enum Tipo
        {
            Maestro = 1,
            Trabajo
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "almacen.sp_requisicion_tr";

        private int _id_requisicion;
        /// <summary>
        /// Atributo que Almacena la Requisición
        /// </summary>
        public int id_requisicion { get { return this._id_requisicion; } }
        private int _no_requisicion;
        /// <summary>
        /// Atributo que Almacena el No. de Requisición
        /// </summary>
        public int no_requisicion { get { return this._no_requisicion; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo que Almacena la Compania Emisora
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo que Almacena el Servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo que Almacena el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo que Almacena el Estatus (Enumeración)
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private string _referencia;
        /// <summary>
        /// Atributo que Almacena la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private byte _id_tipo;
        /// <summary>
        /// Atributo que Almacena el Tipo
        /// </summary>
        public byte id_tipo { get { return this._id_tipo; } }
        /// <summary>
        /// Atributo que Almacena el Tipo (Enumeración)
        /// </summary>
        public Tipo tipo { get { return (Tipo)this._id_tipo; } }
        private int _id_almacen;
        /// <summary>
        /// Atributo que Almacena el Almacen
        /// </summary>
        public int id_almacen { get { return this._id_almacen; } }
        private int _id_usuario_solicitante;
        /// <summary>
        /// Atributo que Almacena el Usuario Solicitante
        /// </summary>
        public int id_usuario_solicitante { get { return this._id_usuario_solicitante; } }
        private DateTime _fecha_solitud;
        /// <summary>
        /// Atributo que Almacena la Fecha de Solicitud
        /// </summary>
        public DateTime fecha_solitud { get { return this._fecha_solitud; } }
        private DateTime _fecha_entrega_requerida;
        /// <summary>
        /// Atributo que Almacena la Fecha de Entrega Requerida
        /// </summary>
        public DateTime fecha_entrega_requerida { get { return this._fecha_entrega_requerida; } }
        private DateTime _fecha_entrega;
        /// <summary>
        /// Atributo que Almacena la Fecha de Entrega
        /// </summary>
        public DateTime fecha_entrega { get { return this._fecha_entrega; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que Almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que Inicializa los Atributos de la Clase por Defecto
        /// </summary>
        public Requisicion()
        {
            //Asignando Valores
            this._id_requisicion = 0;
            this._no_requisicion = 0;
            this._id_compania_emisora = 0;
            this._id_servicio = 0;
            this._id_estatus = 0;
            this._referencia = "";
            this._id_tipo = 0;
            this._id_almacen = 0;
            this._id_usuario_solicitante = 0;
            this._fecha_solitud = DateTime.MinValue;
            this._fecha_entrega_requerida = DateTime.MinValue;
            this._fecha_entrega = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que Inicializa los Atributos de la Clase dado un Registro
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        public Requisicion(int id_requisicion)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_requisicion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Requisicion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos de la Clase dado un Registro
        /// </summary>
        /// <param name="id_requisicion">Requisición</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_requisicion)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_requisicion, 0, 0, 0, 0, "", 0, 0, 0, null, null, null, 0, false, "", "" };

            //Ejecutando SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existen Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Ciclo
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_requisicion = Convert.ToInt32(dr["Id"]);
                        this._no_requisicion = Convert.ToInt32(dr["NoRequisicion"]);
                        this._id_compania_emisora = Convert.ToInt32(dr["IdCompaniaEmisora"]);
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._referencia = dr["Referencia"].ToString();
                        this._id_tipo = Convert.ToByte(dr["IdTipo"]);
                        this._id_almacen = Convert.ToInt32(dr["IdAlmacen"]);
                        this._id_usuario_solicitante = Convert.ToInt32(dr["IdUsuarioSolicitante"]);
                        DateTime.TryParse(dr["FechaSolicitud"].ToString(), out this._fecha_solitud);
                        DateTime.TryParse(dr["FechaEntregaRequerida"].ToString(), out this._fecha_entrega_requerida);
                        DateTime.TryParse(dr["FechaEntrega"].ToString(), out this._fecha_entrega);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
            }

            //Devolviendo Resultado Positivo
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="no_requisicion">Número de Requisición</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo">Tipo</param>
        /// <param name="id_almacen">Almacen</param>
        /// <param name="id_usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_solitud">Fecha de Solicitud</param>
        /// <param name="fecha_entrega_requerida">Fecha de Entrega Requerida</param>
        /// <param name="fecha_entrega">Fecha de Entrega</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int no_requisicion, int id_compania_emisora, int id_servicio, byte id_estatus, string referencia, byte id_tipo, 
                                                      int id_almacen, int id_usuario_solicitante, DateTime fecha_solitud, DateTime fecha_entrega_requerida, DateTime fecha_entrega, 
                                                      int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_requisicion, no_requisicion, id_compania_emisora, id_servicio, id_estatus, referencia, id_tipo, id_almacen, id_usuario_solicitante, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solitud), 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_entrega_requerida), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_entrega), id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Requisiciones
        /// </summary>
        /// <param name="no_requisicion">Número de Requisición</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo">Tipo</param>
        /// <param name="id_almacen">Almacen</param>
        /// <param name="id_usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_solitud">Fecha de Solicitud</param>
        /// <param name="fecha_entrega_requerida">Fecha de Entrega Requerida</param>
        /// <param name="fecha_entrega">Fecha de Entrega</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaRequisicion(int no_requisicion, int id_compania_emisora, int id_servicio, byte id_estatus, string referencia, byte id_tipo, 
                                                      int id_almacen, int id_usuario_solicitante, DateTime fecha_solitud, DateTime fecha_entrega_requerida, DateTime fecha_entrega,
                                                      int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, no_requisicion, id_compania_emisora, id_servicio, id_estatus, referencia, id_tipo, id_almacen, id_usuario_solicitante, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_solitud), 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_entrega_requerida), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_entrega), id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Requisiciones
        /// </summary>
        /// <param name="no_requisicion">Número de Requisición</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_servicio"></param>
        /// <param name="estatus">Estatus</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_tipo">Tipo</param>
        /// <param name="id_almacen">Almacen</param>
        /// <param name="id_usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_solitud">Fecha de Solicitud</param>
        /// <param name="fecha_entrega_requerida">Fecha de Entrega Requerida</param>
        /// <param name="fecha_entrega">Fecha de Entrega</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaRequisicion(int no_requisicion, int id_compania_emisora, int id_servicio, Estatus estatus, string referencia, byte id_tipo, int id_almacen,
                                                 int id_usuario_solicitante, DateTime fecha_solitud, DateTime fecha_entrega_requerida, DateTime fecha_entrega,
                                                 int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando que el Estatus este en Capturado
            if ((Estatus)this._id_estatus == Estatus.Registrada)

                //Devolviendo Resultado Obtenido
                result = this.actualizaRegistrosBD(no_requisicion, id_compania_emisora, id_servicio, (byte)estatus, referencia, id_tipo, id_almacen, id_usuario_solicitante, fecha_solitud,
                                   fecha_entrega_requerida, fecha_entrega, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                result = new RetornoOperacion(string.Format("El Estatus '{0}' no permite Editar la Requisición", (Estatus)this._id_estatus));

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Deshabilitar las Requisiciones
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaRequisicion(int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Inicia la transaccion 
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Devolviendo Resultado Obtenido
                retorno = this.actualizaRegistrosBD(this._no_requisicion, this._id_compania_emisora, this._id_servicio, this._id_estatus, this._referencia, this._id_tipo, this._id_almacen, this._id_usuario_solicitante, this._fecha_solitud,
                                   this._fecha_entrega_requerida, this._fecha_entrega, id_usuario, false);

                //Validando Operación Exitosa
                if (retorno.OperacionExitosa)
                {
                    //Obteniendo Detalles de la Requisición
                    using (DataTable dtDetalleReq = SAT_CL.Almacen.RequisicionDetalle.ObtieneDetallesRequisicion(this._id_requisicion))
                    {
                        //Valida los datos del datatable
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalleReq))
                        {
                            //Recorre las filas de la tabla
                            foreach (DataRow r in dtDetalleReq.Rows)
                            {
                                //Invoca a la clase RequisicionDetalle
                                using (SAT_CL.Almacen.RequisicionDetalle reqDet = new SAT_CL.Almacen.RequisicionDetalle(Convert.ToInt32(r["NoDetalle"])))
                                    //Asigna al objeto retorno el resultado del método invocado DeshabilitaDetalleRequisicion().
                                    retorno = reqDet.DeshabilitaDetalleRequisicion(id_usuario);
                                //Valida si la acción se realizo correctamente y  niega la operaci´on para que elimine todos los registros del data table.
                                if (!retorno.OperacionExitosa)
                                    //Cuando el retorno sea false agrega un break.
                                    break;
                            }
                        }
                    }
                }


                //Valida la Transaccion
                if (retorno.OperacionExitosa)

                    //Invoca al método Complete().
                    trans.Complete();
            }

            //Regresa el resultado al método
            return retorno;
        }
        /// <summary>
        /// Método encargado de Actualizar la Carga de los Atributos de la Requisición
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRequisicion()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_requisicion);
        }
        /// <summary>
        /// Método encargado de Solicitar una Requisición
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion SolicitaRequisicion(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando Tipo de Requisición
                if ((Tipo)this._id_tipo == Tipo.Trabajo)
                {
                    //Validando que el Estatus sea Registrado
                    if ((Estatus)this._id_estatus == Estatus.Registrada)
                    {
                        //Devolviendo Resultado Obtenido
                        result = this.actualizaRegistrosBD(this._no_requisicion, this._id_compania_emisora, this._id_servicio, (byte)Estatus.Solicitada, 
                                           this._referencia, this._id_tipo, this._id_almacen, this._id_usuario_solicitante, this._fecha_solitud,
                                           this._fecha_entrega_requerida, this._fecha_entrega, id_usuario, this._habilitar);

                        //Validando que la Operación haya sido exitosa
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Detalles
                            using (DataTable dtDetalles = RequisicionDetalle.ObtieneDetallesRequisicion(this._id_requisicion))
                            {
                                //Validando que Existan los Detalles
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                                {
                                    //Recorriendo Registros
                                    foreach (DataRow dr in dtDetalles.Rows)
                                    {
                                        //Instanciando Detalle de Requisición
                                        using (RequisicionDetalle rd = new RequisicionDetalle(Convert.ToInt32(dr["NoDetalle"])))
                                        {
                                            //Validando que exista
                                            if (rd.IdDetalleRequisicion > 0)
                                            {
                                                //Actualizando Estatus
                                                result = rd.EditaEstatus(RequisicionDetalle.EstatusDetalle.Solicitado, id_usuario);

                                                //Validando que la Operación no fuese Exitosa
                                                if (!result.OperacionExitosa)
                                                    //Terminando Ciclo
                                                    break;
                                            }
                                            else
                                                //Terminando Ciclo
                                                break;
                                        }
                                    }

                                    //Validando que la Operación fuese Exitosa
                                    if (result.OperacionExitosa)
                                    {   //Completando Transacción
                                        trans.Complete();

                                        //Instanciando Requisición
                                        result = new RetornoOperacion(this._id_requisicion);
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Requisición no tiene Detalles");
                            }
                        }
                    }
                    else
                    {
                        //Validando Estatus para Excepción
                        switch((Estatus)this._id_estatus)
                        {
                            case Estatus.Solicitada:
                                {
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Requisición ya ha sido Solicitada");
                                    break;
                                }
                            case Estatus.PorAutorizar:
                                {
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Requisición se encuentra Por Autorizar");
                                    break;
                                }
                            case Estatus.AbastecidaParcial:
                                {
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Requisición se encuentra Abastecida Parcialmente");
                                    break;
                                }
                            case Estatus.Cerrada:
                                {
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Requisición se encuentra Cerrada");
                                    break;
                                }
                            case Estatus.Cancelada:
                                {
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Requisición se encuentra Cancelada");
                                    break;
                                }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No puede solicitar una Requisición Maestra");
            }

            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus de la Requisición y de los Detalles Ligados
        /// </summary>
        /// <param name="estatus">Estatus de la Requisición</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusRequisicion(Estatus estatus, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Devolviendo Resultado Obtenido
            result = this.actualizaRegistrosBD(this._no_requisicion, this._id_compania_emisora, this._id_servicio, (byte)estatus, this._referencia, 
                                               this._id_tipo, this._id_almacen, this._id_usuario_solicitante, this._fecha_solitud, this._fecha_entrega_requerida, 
                                               estatus == Estatus.Cerrada ? Fecha.ObtieneFechaEstandarMexicoCentro() : this._fecha_entrega, id_usuario, this._habilitar);

            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus de la Requisición y de los Detalles Ligados
        /// </summary>
        /// <param name="estatus">Estatus de la Requisición</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusRequisicionDetalles(Estatus estatus, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Estatus General de los Detalles
            RequisicionDetalle.EstatusDetalle estatus_det = (RequisicionDetalle.EstatusDetalle)((byte)estatus);

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Devolviendo Resultado Obtenido
                result = this.actualizaRegistrosBD(this._no_requisicion, this._id_compania_emisora, this._id_servicio, (byte)estatus, 
                                                   this._referencia, this._id_tipo, this._id_almacen, this._id_usuario_solicitante, this._fecha_solitud,
                                                   this._fecha_entrega_requerida, estatus == Estatus.Cerrada ? Fecha.ObtieneFechaEstandarMexicoCentro() : this._fecha_entrega, id_usuario, this._habilitar);

                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                {
                    //Obteniendo Detalles
                    using (DataTable dtDetalles = RequisicionDetalle.ObtieneDetallesRequisicion(this._id_requisicion))
                    {
                        //Validando que Existan los Detalles
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Registros
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Detalle de Requisición
                                using (RequisicionDetalle rd = new RequisicionDetalle(Convert.ToInt32(dr["NoDetalle"])))
                                {
                                    //Validando que exista
                                    if (rd.IdDetalleRequisicion > 0)
                                    {
                                        //Actualizando Estatus
                                        result = rd.EditaEstatus(estatus_det, id_usuario);

                                        //Validando que la Operación no fuese Exitosa
                                        if (!result.OperacionExitosa)
                                            //Terminando Ciclo
                                            break;
                                    }
                                    else
                                        //Terminando Ciclo
                                        break;
                                }
                            }

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {   
                                //Instanciando Requisición
                                result = new RetornoOperacion(this._id_requisicion);

                                //Completando Transacción
                                trans.Complete();
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("La Requisición no tiene Detalles");
                    }
                }
            }

            //Devolviendo Objeto de Retorno
            return result;
        }

        /// <summary>
        /// Método encargado de Cancelar una Requisición
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion CancelaRequisicion(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("El estatus de la Requisición no permite su Cancelación");
            
            //Validamos Estatus de la Requición sea Solicitada
            if((Estatus) this._id_estatus == Estatus.Solicitada)
            {
                //Actualizamos Estatus
                resultado = ActualizaEstatusRequisicionDetalles(Estatus.Cancelada, id_usuario);
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Obtener el Total de las Refacciones de la Requisición
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static decimal ObtieneTotalRefaccionRequisicion(int id_orden_trabajo)
        {
            //Declarando Variable de Retorno
            decimal total_requisicion = 0.00M;

            //Armando Arreglo de Parametros
            object[] param = { 4, id_orden_trabajo, 0, 0, 0, 0, "", 0, 0, 0, null, null, null, 0, false, "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Obteniendo Valor
                        decimal.TryParse(dr["TotalRequisicion"].ToString(), out total_requisicion);

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return total_requisicion;
        }
        /// <summary>
        /// Método encargado de Obtener las Requisiciones ligadas a una Orden de Trabajo
        /// </summary>
        /// <param name="id_orden_trabajo">Orden de Trabajo</param>
        /// <returns></returns>
        public static DataTable ObtieneRequisicionesOrdenTrabajo(int id_orden_trabajo)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, id_orden_trabajo, 0, 0, 0, 0, "", 0, 0, 0, null, null, null, 0, false, "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtRequisiciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisiciones;
        }
        /// <summary>
        /// Método encargado de Obtener las Requisiciones Ligadas a un Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneRequisicionesServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dtRequisiciones = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, 0, 0, 0, id_servicio, 0, "", 0, 0, 0, null, null, null, 0, false, "", "" };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtRequisiciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtRequisiciones;
        }
        /// <summary>
        /// Método encargado de Agregar un Servicio a la Requisición
        /// </summary>
        /// <param name="id_servicio">Servicio por Agregar</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion AgregaServicioRequisicion(int id_servicio, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Devolviendo Resultado Obtenido
            result = this.actualizaRegistrosBD(this._no_requisicion, this._id_compania_emisora, id_servicio, this._id_estatus, 
                                               this._referencia, this._id_tipo, this._id_almacen, this._id_usuario_solicitante, this._fecha_solitud,
                                               this._fecha_entrega_requerida, this._fecha_entrega, id_usuario, this._habilitar);

            //Devolviendo Resultado Positivo
            return result;
        }
        /// <summary>
        /// Método encargado Eliminar el Servicio de la Requisición
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EliminaServicioRequisicion(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Devolviendo Resultado Obtenido
            result = this.actualizaRegistrosBD(this._no_requisicion, this._id_compania_emisora, 0, this._id_estatus,
                                               this._referencia, this._id_tipo, this._id_almacen, this._id_usuario_solicitante, this._fecha_solitud,
                                               this._fecha_entrega_requerida, this._fecha_entrega, id_usuario, this._habilitar);

            //Devolviendo Resultado Positivo
            return result;
        }

        #endregion
    }
}
