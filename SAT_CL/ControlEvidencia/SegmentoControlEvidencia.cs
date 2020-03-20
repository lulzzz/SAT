using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using SAT_CL.Despacho;
using System.Linq;
using System.Transactions;

namespace SAT_CL.ControlEvidencia
{
  /// <summary>
    /// Clase Encargada de las Operaciones de los Segmnetos de Control de Evidencia
  /// </summary>
  public class SegmentoControlEvidencia : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_segmento_control_evidencia_tsce";

        private int _id_segmento_control_evidencia;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Control Evidencia del segmento
        /// </summary>
        public int id_segmento_control_evidencia
        {
            get { return this._id_segmento_control_evidencia; }
        }
        private int _id_servicio_contorl_evidencia;
        /// <summary>
        /// Atributo encargado de almacenar el Id servicio Control Evidencia
        /// </summary>
        public int id_servicio_contorl_evidencia
        {
            get { return this._id_servicio_contorl_evidencia; }
        }

        private int _id_segmento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del segmento
        /// </summary>
        public int id_segmento
        {
            get { return this._id_segmento; }
        }

        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus 
        /// </summary>
        public EstatusSegmentoControlEvidencias id_estatus
        {
            get { return (EstatusSegmentoControlEvidencias)this._id_estatus; }
        }

        private int _id_hoja_instruccion;
        /// <summary>
        /// Atributo encargado de almacenar la Hoja Instrucci
        /// </summary>
        public int id_hoja_instruccion
        {
            get { return this._id_hoja_instruccion; }
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
        /// Define los estatus que puede tener el control de evidencias de segmento
        /// </summary>
        public enum EstatusSegmentoControlEvidencias
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
            No_Recibidos = 5,
            /// <summary>
            /// Cancelados (Edición de paradas).
            /// </summary>
            Cancelado = 6,
             /// <summary>
            /// Solo se han Digitalizados Imagenes
            /// </summary>
            Imagen_Digitalizada =7
        }

        /// <summary>
        /// Define los Tipos de Consulta
        /// </summary>
        public enum TipoConsulta
        {
            /// <summary>
            /// Id Segmento Control Evidencia
            /// </summary>
            IdSegmentoControlEvidencia = 1,
            /// <summary>
            /// Id Segmento
            /// </summary>
            IdSegmento,
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Valores por Default
        /// </summary>
        public SegmentoControlEvidencia()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Valores dado un Id de Registro
        /// </summary>
        /// <param name="tipo">Tipo de Consulta</param>
        /// <param name="id_registro">Id de Registro</param>
        public SegmentoControlEvidencia(TipoConsulta tipo, int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(tipo, id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~SegmentoControlEvidencia()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Default
        /// </summary>
        private void cargaAtributosInstancia()
        {   
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="tipo">Tipo de Consulta</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(TipoConsulta tipo, int id_registro)
        {
            //Declaramos Variables  para ejecutar la onsulta
            int tipo_consulta = 6;
            int id_segmento_control_evidencia = 0;
            int id_segmento = id_registro;

            //Si el Yipo es Is Servicio Control Evidencia
            if (TipoConsulta.IdSegmentoControlEvidencia == tipo)
            {
                tipo_consulta = 3;
                id_segmento_control_evidencia = id_registro;
                id_segmento = 0;
            }
            //Declarando variable de Retorno
            bool result = false;
            //Declarando Arreglo de Parametros
            object[] param = { tipo_consulta, id_segmento_control_evidencia, 0, id_segmento, 0, 0, 0, false, "", "" };
            //Obteniendo resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que el DataSet contenga Registros
                if(Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo las Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_segmento_control_evidencia = Convert.ToInt32(dr["Id"]);
                        this._id_servicio_contorl_evidencia = Convert.ToInt32(dr["IdServicioControlEvidencia"]);
                        this._id_segmento = Convert.ToInt32(dr["IdSegmento"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_hoja_instruccion = Convert.ToInt32(dr["IdHojaInstruccion"]);
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
        /// <param name="id_segmento">Id de segmento</param>
        /// <param name="id_servicio_control_evidencia">Id Servicio Control Evidencia</param>
        /// <param name="estatus">Estatus </param>
        /// <param name="id_hoja_instruccion">Id Hoja Instruccion</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaAtributos(int id_servicio_control_evidencia, int id_segmento,  EstatusSegmentoControlEvidencias estatus, int id_hoja_instruccion, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Arreglo de Parametros del SP
            object[] param = { 2, this._id_segmento_control_evidencia,id_servicio_contorl_evidencia, id_segmento, estatus,
                                  id_hoja_instruccion, id_usuario, habilitar, "", "" };
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
        /// <param name="id_servicio_control_evidencia">Id Servicio Control Evidencia</param>
        /// <param name="id_segmento">Id de segmento</param>
        /// <param name="id_hoja_instruccion">Hoja Instrucción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSegmentoControlEvidencia(int id_servicio_control_evidencia, int id_segmento, int id_hoja_instruccion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Si cuenta con una Hoja de Instruccón
            if (id_hoja_instruccion != 0)
            {
                //Insertamos Segmento Control Evidencia

                //Declarando Arreglo de Parametros del SP
                object[] param = { 1, 0, id_servicio_control_evidencia, id_segmento, EstatusSegmentoControlEvidencias.No_Recibidos,  id_hoja_instruccion,
                                 id_usuario, true, "", "" };
                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }
            //Si no existe HI
            else
            {
                //Instanciamos Segmento
                using (SegmentoCarga objSegmento = new SegmentoCarga(id_segmento))
                {
                    //Instanciamos Parada Inicio y Parada Fin
                    using (Parada objParadaInicio = new Parada(objSegmento.id_parada_inicio), objParadaFin = new Parada(objSegmento.id_parada_fin))
                    {

                        result = new RetornoOperacion("No se encontró la HI para el siguiente segmento: " + "(Remitente " + objParadaInicio.descripcion + " / Destinatario " + objParadaFin.descripcion + ")");
                    }
                }
            }
            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Público encargado de Editar Registros
        /// </summary>
        /// <param name="id_servicio_control_evidencia">Id Servicio Control Evidencia</param>
        /// <param name="id_segmento">Id de segmento</param>
        /// <param name="estatus">Estatus </param>
        /// <param name="id_hoja_instruccion">Hoja Instruccion</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaSegmentoControlEvidencia(int id_servicio_control_evidencia, int id_segmento, EstatusSegmentoControlEvidencias estatus,
                                                              int id_hoja_instruccion, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaAtributos(id_servicio_contorl_evidencia, id_segmento, estatus, id_hoja_instruccion, id_usuario, true);
        }
        

      /// <summary>
      /// Edita Estatus del Segmento Control Evidencia
      /// </summary>
      /// <param name="estatus"></param>
      /// <param name="id_usuario"></param>
      /// <returns></returns>
        public RetornoOperacion EditaEstatusSegmentoControlEvidencia(EstatusSegmentoControlEvidencias estatus, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaAtributos(this._id_servicio_contorl_evidencia, this._id_segmento, estatus, this._id_hoja_instruccion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Eliminar Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaSegmentoControlEvidencia(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaAtributos(this._id_servicio_contorl_evidencia,this._id_segmento, (EstatusSegmentoControlEvidencias)this._id_estatus, this._id_hoja_instruccion, id_usuario, false);
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <param name="tipo">Tipo de Consulta </param>
        /// <param name="id_regitro">Id Registro</param>
        /// <returns></returns>
        public bool ActualizaServicioControlEvidencia(TipoConsulta tipo, int id_registro)
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(tipo, id_registro);
        }


        /// <summary>
        /// Método encargado de actualizar el estatus del control de evidencia del segmento.
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusSegmentoControlEvidenciaSegmento(int id_usuario)
        {

            //Definiendo objetos de estatus de documentos
            bool recibido, recibido_reenvio, transito, en_aclaracion, no_recibido, cancelado, imagen_digitalizada;
            RetornoOperacion resultado = new RetornoOperacion();
            //Definiendo variable de estatus por asignar
            EstatusSegmentoControlEvidencias nuevo_estatus = EstatusSegmentoControlEvidencias.No_Recibidos;
           
            //Cargando los estatus de documentos
            ControlEvidenciaDocumento.CargaEstatusDocumentosControlEvidencia(this.id_segmento_control_evidencia, out recibido, out recibido_reenvio, out transito, out en_aclaracion, out no_recibido, out cancelado,
                                                                            out imagen_digitalizada);

            //Determinando estatus de viaje
            //Filtrando solo recibidos
            if (recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & !imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Recibido;
            }
            //Filtrando solo recibidos con Imagen Digitalizada
            else if (recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Imagen_Digitalizada;
            }
            //Solo cancelados
            else if (cancelado & !recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & !imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Cancelado;
            }
            //Solo cancelados con Imagen Digitalizada
            else if (cancelado & !recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Cancelado;
            }
            //Solo recibido con reenvio
            else if (!recibido & recibido_reenvio & !transito & !en_aclaracion & !no_recibido &!imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Recibido_Reenvio;
            }
            //Solo recibido con reenvio con Imagen Digitalizada
            else if (!recibido & recibido_reenvio & !transito & !en_aclaracion & !no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Imagen_Digitalizada;
            }
            //Solo no recibido
            else if (!recibido & !recibido_reenvio & !transito & !en_aclaracion & no_recibido & !imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.No_Recibidos;
            }
            //Imagen Digitalizada
            else if (!recibido & !recibido_reenvio & !transito & !en_aclaracion & !no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Imagen_Digitalizada;
            }
            //No Recibidos con Imagen Digitalizada
            else if (!recibido & !recibido_reenvio & !transito & !en_aclaracion & no_recibido & imagen_digitalizada)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.Imagen_Digitalizada;
            }
            //Cualquier combinación con estatus en aclaración, tomará este estatus directamente
            else if (en_aclaracion)
            {
                nuevo_estatus = EstatusSegmentoControlEvidencias.En_Aclaracion;
            }
            //Para el resto de posibilidades
            else
            {
                //Si existen documentos en transito, se asigna estatus directo
                if (transito)
                {
                    nuevo_estatus = EstatusSegmentoControlEvidencias.Transito;
                }
                //Si no hay transito, en este nivel solo habrá aclaraciones
                else
                {
                    nuevo_estatus = EstatusSegmentoControlEvidencias.En_Aclaracion;
                }
            }

            //Invocando Método de Actualización
            resultado = this.actualizaAtributos(this._id_servicio_contorl_evidencia, this._id_segmento, nuevo_estatus,
                                           this._id_hoja_instruccion, id_usuario, this._habilitar);

            return resultado;
        }

        /// <summary>
        /// Carga el listado de estatus de segmento de un viaje y los asigna a los parámetros de salida para su uso posterior
        /// </summary>
        /// <param name="id_servicio">Id servicio control de evidencia</param>
        /// <param name="recibido">Documentos recibidos</param>
        /// <param name="recibido_reenvio">Documentos recibidos con reenvio</param>
        /// <param name="transito">Documentos en transito</param>
        /// <param name="en_acalaracion">Documentos en aclaración</param>
        /// <param name="no_recibido">Documentos no recibidos</param>
        /// <param name="imagen_digitalizada">Documentos Con Imagen Digitalizada</param>
        public static void CargaEstatusSegmentoControlEvidencia(int id_servicio, out bool recibido, out bool recibido_reenvio,
            out bool transito, out bool en_acalaracion, out bool no_recibido, out bool imagen_digitalizada)
        {
            //Asignando parametros de salida
            recibido = recibido_reenvio = transito = en_acalaracion = imagen_digitalizada = false;
            no_recibido = true;

            //Declarando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, 0, 0, 0, false, id_servicio, "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando valores de estatus en parámetro de salida
                    recibido = (from DataRow r in ds.Tables["Table"].Rows
                                where Convert.ToByte(r["IdEstatus"]) == 1
                                select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                    recibido_reenvio = (from DataRow r in ds.Tables["Table"].Rows
                                        where Convert.ToByte(r["IdEstatus"]) == 2
                                        select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                    transito = (from DataRow r in ds.Tables["Table"].Rows
                                where Convert.ToByte(r["IdEstatus"]) == 3
                                select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                    en_acalaracion = (from DataRow r in ds.Tables["Table"].Rows
                                      where Convert.ToByte(r["IdEstatus"]) == 4
                                      select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                    no_recibido = (from DataRow r in ds.Tables["Table"].Rows
                                   where Convert.ToByte(r["IdEstatus"]) == 5
                                   select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                    imagen_digitalizada = (from DataRow r in ds.Tables["Table"].Rows
                                   where Convert.ToByte(r["IdEstatus"]) == 7
                                   select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                }
            }
        }

     
          /// <summary>
        /// Método encargado de realizar los modificaciones existentes de evidencias de acuerdo a la insercción de parada en Despacho
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_segmento_anterior">Id Segmento Anterior</param>
        /// <param name="id_segmento_posterior">Id Segemento Posterior</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion SegmentoControlEvidenciaInsertaParada(int id_servicio, int id_segmento_anterior, int id_segmento_posterior, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion res = new RetornoOperacion(0);

            //declaramos avriables para almacenar la HI
            int hi_segmento_anterior = 0;
            int hi_segmento_posterior = 0;

             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio Control Evidencia
                using (ServicioControlEvidencia objServicioCE = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, id_servicio))
                {
                    //Validamos que exista el Servicio Control Evidencia
                    if (objServicioCE.id_servicio_control_evidencia > 0)
                    {
                        //En caso de ser segmento en medio
                        if (id_segmento_posterior != 0 && id_segmento_anterior != 0)
                        {
                            //Obtenemos HI correspondiente al Segmento anterior
                            res = Despacho.SegmentoCarga.ObteniendoHISegmento(id_segmento_anterior);
                            //Validamos que exista HI para el segmento anterior
                            if (res.OperacionExitosa)
                            {
                                //Establcemos valor
                                hi_segmento_anterior = res.IdRegistro;
                                //Obtenemos HI correspondiente al Segmento  posterior
                                res = Despacho.SegmentoCarga.ObteniendoHISegmento(id_segmento_posterior);
                                //Validamos que exista HI para el segmento posterior
                                if (res.OperacionExitosa)
                                {
                                    //Establecemos valor
                                    hi_segmento_posterior = res.IdRegistro;

                                    //Instanciamos Control Evidencia Segmento
                                    using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(TipoConsulta.IdSegmento, id_segmento_anterior))
                                    {
                                        //Si no existe el Segmento Control Eviedncia Documento
                                        if (objSegmentoControlEvidencia.id_segmento_control_evidencia <= 0)
                                        {

                                            //Validamos Resultado
                                            if (res.OperacionExitosa)
                                            {
                                                //Insertamos Segmento Control Evidencia
                                                res = SegmentoControlEvidencia.InsertaSegmentoControlEvidencia(objServicioCE.id_servicio_control_evidencia, id_segmento_anterior, hi_segmento_anterior, id_usuario);
                                            }
                                        }
                                        else
                                        {
                                            //Carga Control Evidencia Documentos ligado al Segmento Control Evidencia Documento
                                            res = ControlEvidenciaDocumento.CancelaControlEvidenciaDocumento(objSegmentoControlEvidencia.id_segmento_control_evidencia, objSegmentoControlEvidencia, id_usuario);

                                            //Validamos Resultado
                                            if (res.OperacionExitosa)
                                            {
                                                //Insertamos Segmento Control Evidencia
                                                res = SegmentoControlEvidencia.InsertaSegmentoControlEvidencia(objServicioCE.id_servicio_control_evidencia, id_segmento_anterior, hi_segmento_anterior, id_usuario);
                                            }

                                        }
                                        //Insertamos Segmento Control Evidencia para el posterior Segmento
                                        //Validamos Resultado
                                        if (res.OperacionExitosa)
                                        {
                                            //Insertamos Segmento Control Evidencia
                                            res = SegmentoControlEvidencia.InsertaSegmentoControlEvidencia(objServicioCE.id_servicio_control_evidencia, id_segmento_posterior, hi_segmento_posterior, id_usuario);
                                        }
                                    }
                                }
                            }

                        }
                            //En caso de ser Segmento al final
                        else if (id_segmento_posterior == 0)
                        {
                            //Obtenemos HI correspondiente al Segmento  posterior
                            res = Despacho.SegmentoCarga.ObteniendoHISegmento(id_segmento_anterior);
                            //Validamos Resultado
                            if (res.OperacionExitosa)
                                //Establcemos HI
                                hi_segmento_anterior = res.IdRegistro;
                            //Validamos que exista el Servicio Control Evidencia
                            if (objServicioCE.id_servicio_control_evidencia > 0)
                            {

                                //Insertamos Segmento Control Evidencia
                                res = SegmentoControlEvidencia.InsertaSegmentoControlEvidencia(objServicioCE.id_servicio_control_evidencia, id_segmento_anterior, hi_segmento_anterior, id_usuario);

                            }
                        }
                        //Validamos Resultado
                        if (res.OperacionExitosa)
                        {
                            //anteriorizamos Estatus del Servicio Control Evidencia
                            res = objServicioCE.ActualizaEstatusGeneralServicioControlEvidencia(id_usuario);

                        }
                    }
                }
                //Validamos resultado
                if(res.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }
            }

            //Devolvemos resultado
            return res;
        }

      /// <summary>
      /// Método encargado de eralizar las ediciones de las evidencias de acuerdo a la deshabilitacion de parada en despacho
      /// </summary>
      /// <param name="id_servicio"></param>
      /// <param name="id_segmento_anterior"></param>
      /// <param name="id_segmento_posterior"></param>
      /// <param name="id_usuario"></param>
      /// <returns></returns>
        public static RetornoOperacion SegmentoControlEvidenciaDeshabilitaParada(int id_servicio, int id_segmento_anterior, int id_segmento_posterior, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion res = new RetornoOperacion(0);

            //declaramos avriables para almacenar la HI
            int hi_segmento_segmento_anterior = 0;

              //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio Control Evidencia
                using (ServicioControlEvidencia objServicioCE = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, id_servicio))
                {
                    //Validamos que exista el Servicio Control Evidencia
                    if (objServicioCE.id_servicio_control_evidencia > 0)
                    {
                        //En caso de ser segmento en medio
                        if (id_segmento_posterior != 0 && id_segmento_anterior != 0)
                        {
                            //Establcemos valor
                            hi_segmento_segmento_anterior = res.IdRegistro;
                            //Obtenemos HI correspondiente al Segmento  anterior
                            res = Despacho.SegmentoCarga.ObteniendoHISegmento(id_segmento_anterior);
                            //Validamos que exista HI para el segmento anterior
                            if (res.OperacionExitosa)
                            {
                                //Establecemos valor
                                hi_segmento_segmento_anterior = res.IdRegistro;

                                //Instanciamos Control Evidencia Segmento
                                using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(TipoConsulta.IdSegmento, id_segmento_anterior))
                                {
                                    //Carga Control Evidencia Documentos ligado al Segmento Control Evidencia Documento
                                    res = ControlEvidenciaDocumento.CancelaControlEvidenciaDocumento(objSegmentoControlEvidencia.id_segmento_control_evidencia, objSegmentoControlEvidencia, id_usuario);
                                    //Insertamos Segmento Control Evidencia para el posterior Segmento
                                    //Validamos Resultado
                                    if (res.OperacionExitosa)
                                    {
                                        //Instanciamos Control Evidencia Segmento Posterior
                                        using (SegmentoControlEvidencia objSegmentoControlEvidenciaP = new SegmentoControlEvidencia(TipoConsulta.IdSegmento, id_segmento_posterior))
                                        {

                                            //Carga Control Evidencia Documentos ligado al Segmento Control Evidencia Documento
                                            res = ControlEvidenciaDocumento.CancelaControlEvidenciaDocumento(objSegmentoControlEvidenciaP.id_segmento_control_evidencia, objSegmentoControlEvidenciaP, id_usuario);

                                            //Validamos resulado
                                            if (res.OperacionExitosa)
                                            {
                                                //Insertamos Segmento Control Evidencia
                                                res = SegmentoControlEvidencia.InsertaSegmentoControlEvidencia(objServicioCE.id_servicio_control_evidencia, id_segmento_anterior, hi_segmento_segmento_anterior, id_usuario);
                                            }


                                        }
                                    }
                                }
                            }
                        }
                        //En caso de Ser Segmento Al final
                        else if (id_segmento_posterior == 0)
                        {
                            if (objServicioCE.id_servicio_control_evidencia > 0)
                            {
                                //Instanciamos Control Evidencia Segmento
                                using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(TipoConsulta.IdSegmento, id_segmento_anterior))
                                {

                                    //Carga Control Evidencia Documentos ligado al Segmento Control Evidencia Documento
                                    res = ControlEvidenciaDocumento.CancelaControlEvidenciaDocumento(objSegmentoControlEvidencia.id_segmento_control_evidencia, objSegmentoControlEvidencia, id_usuario);

                                }

                            }
                        }
                        //Validamos Resultado
                        if (res.OperacionExitosa)
                        {
                            //anteriorizamos Estatus del Servicio Control Evidencia
                            res = objServicioCE.ActualizaEstatusGeneralServicioControlEvidencia(id_usuario);

                        }
                    }
                }
                //Validamos Resultado
                if(res.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }
            }

            //Devolvemos resultado
            return res;
        }

        /// <summary>
        /// Carga Resumen de los Segmentos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaResumenSegmentos(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            //Declarando Arreglo de Parametros
            object[] param = { 5, 0, 0, 0, 0, 0, 0, false, id_servicio, "" };

            //Realizando la consulta
            using (DataTable mit1 = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables["Table"])
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(mit1))
                    //Asignando a objeto de retorno
                    mit = mit1;

                //Devolviendo resultado
                return mit;
            }
        }
        #endregion

    }
}
