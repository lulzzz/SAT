using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using SAT_CL.Despacho;
using System.Transactions;

namespace SAT_CL.ControlEvidencia
{
    /// <summary>
    /// Inplementa una Interfaz Servicio Control Evidencia
    /// </summary>
    public class ServicioControlEvidencia : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_servicio_control_evidencia_tvce";

        private int _id_servicio_control_evidencia;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Control Evidencia del servicio
        /// </summary>
        public int id_servicio_control_evidencia
        {
            get { return this._id_servicio_control_evidencia; }
        }

		private int _id_servicio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del servicio
        /// </summary>
        public int id_servicio
        {
            get { return this._id_servicio; }
        }
		
        private byte _id_estatus_documentos;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de los Documentos
        /// </summary>
        public EstatusServicioControlEvidencias id_estatus_documentos
        {
            get { return (EstatusServicioControlEvidencias)this._id_estatus_documentos; }
        }

        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio
        {
            get { return this._fecha_inicio; }
        }
		
        private DateTime _fecha_termino;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Termino
        /// </summary>
        public DateTime fecha_termino
        {
            get { return this._fecha_termino; }
        }
		
        
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de Habilitado
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Enumeraciones

        /// <summary>
        /// Define los estatus que puede tener el control de evidencias de servicio
        /// </summary>
        public enum EstatusServicioControlEvidencias
        {
            /// <summary>
            /// Documentos recibidos en terminal de cobro
            /// </summary>
            Recibido = 1,
            /// <summary>
            /// Documentos recibido en terminal distinta a la de cobro
            /// </summary>
            Recibido_Reenvio,
            /// <summary>
            /// Documentos en transito (dentro de paquete)
            /// </summary>
            Transito,
            /// <summary>
            /// Documentos en aclaración (No se ha localizado fisicamente en el paquetes de envío o no se han recibido algunos documentos en ninguna terminal)
            /// </summary>
            En_Aclaracion,
            /// <summary>
            /// No se ha recibido ningún documento
            /// </summary>
            No_Recibidos,
            /// <summary>
            /// Solo se han Digitalizados Imagenes
            /// </summary>
            Cancelado,
            /// <summary>
            /// Solo se han Cancelados
            /// </summary>
            Imagen_Digitalizada =7
        }

        /// <summary>
        /// Define los Tipos de Consulta
        /// </summary>
        public enum TipoConsulta
        {
            /// <summary>
            /// Id Servicio Control Evidencia
            /// </summary>
            IdServicioControlEvidencia = 1,
            /// <summary>
            /// Id Servicio
            /// </summary>
            IdServicio,
        }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Valores por Default
        /// </summary>
        public ServicioControlEvidencia()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Valores dado un Id de Registro
        /// </summary>
        /// <param name="tipo">Tipo Consulta</param>
        /// <param name="id_registro">Id de Registro</param>
        public ServicioControlEvidencia(TipoConsulta tipo, int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(tipo, id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ServicioControlEvidencia()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Default
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Inicializando Valores
            this._id_servicio_control_evidencia = 0;
            this._id_servicio = 0;
            this._id_estatus_documentos = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_termino = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="tipo">Tipo</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(TipoConsulta tipo, int id_registro)
        {
              //Declaramos Variables  para ejecutar la onsulta
              int tipo_consulta = 6;
              int id_servicio_control_evidencia = 0;
              int id_servicio = id_registro;

            //Si el Yipo es Is Servicio Control Evidencia
            if(TipoConsulta.IdServicioControlEvidencia == tipo)
            {
                tipo_consulta = 3;
                id_servicio_control_evidencia = id_registro;
                id_servicio = 0;
            }
            
            //Declarando variable de Retorno
            bool result = false;
            //Declarando Arreglo de Parametros
            object[] param = { tipo_consulta, id_servicio_control_evidencia, id_servicio, 0, null, null, 0,  false, "", "" };
            //Obteniendo resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que el DataSet contenga Registros
                if(Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo las Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_servicio_control_evidencia = Convert.ToInt32(dr["Id"]);
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_estatus_documentos = Convert.ToByte(dr["IdEstatusDocumento"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out _fecha_inicio);
                        DateTime.TryParse(dr["FechaTermino"].ToString(), out _fecha_termino);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Cambiando valor de Variable de Retorno 
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }



        /// <summary>
        /// Método Privado encargado de Actualizar los Valores del Registro
        /// </summary>
        /// <param name="id_servicio">Id de servicio</param>
        /// <param name="id_estatus_documentos">Estatus de los Documentos</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_termino">Fecha de Termino</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitado</param>
        /// <returns></returns>
        private RetornoOperacion actualizaAtributos(int id_servicio, EstatusServicioControlEvidencias id_estatus_documentos,
                                                        DateTime fecha_inicio, DateTime fecha_termino,
                                                        int id_usuario,
                                                        bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Arreglo de Parametros del SP
            object[] param = { 2, this._id_servicio_control_evidencia, id_servicio, (EstatusServicioControlEvidencias)id_estatus_documentos,
                                            Fecha.ConvierteDateTimeObjeto(fecha_inicio), Fecha.ConvierteDateTimeObjeto(fecha_termino), 
                                            id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }
        
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar nuevos Registros
        /// </summary>
        /// <param name="id_servicio">Id de servicio</param>
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioControlEvidencia(int id_servicio, DateTime fecha_inicio, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();
            int idServicioControlEvidencia = 0;

             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Insertamos Servicio Control Evidencia

                //Declarando Arreglo de Parametros del SP
                object[] param = { 1, 0, id_servicio, (byte)EstatusServicioControlEvidencias.No_Recibidos, fecha_inicio, null, 
                                 id_usuario, true, "", "" };
                //Obteniendo Resultado del SP
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                //Asignamos IdServicioControlEvidencia
                idServicioControlEvidencia = resultado.IdRegistro;

                //Si se realizo la Insercción correctamente
                if (resultado.OperacionExitosa)
                {
                    //Cargamos Segmentos Obteniendo HI
                    using (DataTable mit = SegmentoCarga.CargaSegmentosObteniendoHI(id_servicio, id_usuario))
                    {
                        //Validamos Origen de Datos
                        if (Validacion.ValidaOrigenDatos(mit))
                        {
                            //Obtenemos cada uno de los Registros
                            foreach (DataRow r in mit.Rows)
                            {
                                //Si el resultado es exitoso
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertamos Segmento Control Evidencia
                                    resultado = SegmentoControlEvidencia.InsertaSegmentoControlEvidencia(idServicioControlEvidencia,r.Field<int>("IdSegmento"), r.Field<int>("HI"), id_usuario);
                                }
                                else
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Validamos Transacción
                    scope.Complete();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Método Público encargado de Editar Registros
        /// </summary>
        /// <param name="id_servicio">Id de servicio</param>
        /// <param name="id_estatus_documentos">Estatus de los Documentos</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_termino">Fecha de Termino</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioControlEvidencia(int id_servicio, EstatusServicioControlEvidencias id_estatus_documentos,
                                                        DateTime fecha_inicio, DateTime fecha_termino,
                                                        int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaAtributos(id_servicio, id_estatus_documentos, fecha_inicio, fecha_termino,
                                          id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Eliminar Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioControlEvidencia(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaAtributos(this._id_servicio, this.id_estatus_documentos, this._fecha_inicio, 
                                           this._fecha_termino, id_usuario, false);
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <param name="tipo">Tipo Consulta</param>
        /// <param name="id_registro">Id Registro</param>
        /// <returns></returns>
        public bool ActualizaServicioControlEvidencia(TipoConsulta tipo, int id_registro)
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(tipo, id_registro);
        }
        /// <summary>
        /// Método encargado de actualizar el estatus del control de evidencia del viaje, así como las fechas de inicio y fin de proceso.
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusGeneralServicioControlEvidencia(int id_usuario)
        {
            //Definiendo objetos de estatus de documentos
            bool recibido, recibido_reenvio, transito, en_aclaracion, no_recibido, imagen_digitalizada;
            //Definiendo variable de estatus por asignar
            EstatusServicioControlEvidencias nuevo_estatus = EstatusServicioControlEvidencias.No_Recibidos;
            //Definiendo fecha de inicio y fin de proceso de control de evidencia
            DateTime inicio_control = Fecha.EsFechaMinima(this._fecha_inicio) ? TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro() : this._fecha_inicio, fin_control = this._fecha_termino;

            //Cargando los estatus de documentos
             SegmentoControlEvidencia.CargaEstatusSegmentoControlEvidencia(this._id_servicio, out recibido, out recibido_reenvio, out transito, out en_aclaracion, out no_recibido, out imagen_digitalizada);

            //Determinando estatus de viaje
            //Filtrando solo recibidos
            if (recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & !imagen_digitalizada)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.Recibido;
                fin_control = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            }
            //Filtrando solo recibidos con Imagen Digitalizada
            else if (recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.Imagen_Digitalizada;
                fin_control = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            }
            //Solo recibido con reenvio
            else if (!recibido & recibido_reenvio & !transito & !en_aclaracion & !no_recibido & !imagen_digitalizada)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.Recibido_Reenvio;
            }
            //Solo recibido con reenvio con Imagen Digitalizada
            else if (!recibido & recibido_reenvio & !transito & !en_aclaracion & !no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.Imagen_Digitalizada;
            }
            //Solo no recibido
            else if (!recibido & !recibido_reenvio & !transito & !en_aclaracion & no_recibido & !imagen_digitalizada)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.No_Recibidos;
            }
            //Solo no recibido con Imagen Digitalizada
            else if (!recibido & !recibido_reenvio & !transito & !en_aclaracion & no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.Imagen_Digitalizada;
            }
            //Solo Iamgen Digitalizada
            else if (!recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.Imagen_Digitalizada;
            }
            //Cualquier combinación con estatus en aclaración, tomará este estatus directamente
            else if (en_aclaracion)
            {
                nuevo_estatus = EstatusServicioControlEvidencias.En_Aclaracion;
            }
            //Para el resto de posibilidades
            else
            {
                //Si existen documentos en transito, se asigna estatus directo
                if (transito)
                {
                    nuevo_estatus = EstatusServicioControlEvidencias.Transito;
                }
                //Si no hay transito, en este nivel solo habrá aclaraciones
                else
                {
                    nuevo_estatus = EstatusServicioControlEvidencias.En_Aclaracion;
                }
            }

            //Invocando Método de Actualización
            return this.actualizaAtributos(this._id_servicio, nuevo_estatus, inicio_control, fin_control,
                                           id_usuario, this._habilitar);
        }


                
        /// <summary>
        /// Método Público encargado de Obtener los servicios que no estan ligados a un Control de Evidencia
        /// </summary>
        /// <param name="id_alterno">Id ALterno de servicio</param>
        /// <param name="id_compania">Id de Compania</param>
        /// <returns></returns>
        public static DataTable ObtieneServiciosSinControlEvidencia(string id_alterno, int id_compania)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Declarando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, null, null, 0, 0, false, id_alterno.ToString(), id_compania.ToString() };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando si la Tabla contiene registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Recuperando Tabla
                    dt =  OrigenDatos.RecuperaDataTableDataSet(ds, "Table");
            }//Devolviendo Resultado Obtenido
            return dt;
        }
        
        #endregion
    }
}
