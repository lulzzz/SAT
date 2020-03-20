using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Linq;
using System.Transactions;
using SAT_CL.Global;

namespace SAT_CL.ControlEvidencia
{
    /// <summary>
    /// Clase Encargada de las Operaciones de los Documentos de Control de Evidencia
    /// </summary>
    public class ControlEvidenciaDocumento : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_control_evidencia_documento_tced";

        private int _id_control_evidencia_documento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Control del Documento de Evidencia
        /// </summary>
        public int id_control_evidencia_documento
        {
            get { return this._id_control_evidencia_documento; }
        }

        private int _id_servicio_control_evidencia;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Control de Evidencia
        /// </summary>
        public int id_servicio_control_evidencia
        {
            get { return this._id_servicio_control_evidencia; }
        }

        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del servicio
        /// </summary>
        public int servicio
        {
            get { return this._id_servicio; }
        }
        private int _id_segmento_control_evidencia;
        /// <summary>
        /// Atributo encargado de almacenar el Id  Segmento Control Evidencia
        /// </summary>
        public int id_segmento_control_evidencia
        {
            get { return this._id_segmento_control_evidencia; }
        }
        private int _id_segmento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Segmento
        /// </summary>
        public int id_segmento
        {
            get { return this._id_segmento; }
        }
        private byte _id_tipo_documento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Tipo de Documento
        /// </summary>
        public byte id_tipo_documento
        {
            get { return this._id_tipo_documento; }
        }

        private byte _id_estatus_documento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Estatus del Documento
        /// </summary>
        public EstatusDocumento id_estatus_documento
        {
            get { return (EstatusDocumento)this._id_estatus_documento; }
        }

        private int _id_hoja_instruccion_documento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Viaje
        /// </summary>
        public int id_hoja_instruccion_documento
        {
            get { return this._id_hoja_instruccion_documento; }
        }

        private int _terminal_recepcion;
        /// <summary>
        /// Atributo encargado de almacenar la Terminal de Recepción
        /// </summary>
        public int terminal_recepcion
        {
            get { return this._terminal_recepcion; }
        }

        private DateTime _fecha_recepcion;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Recepción
        /// </summary>
        public DateTime fecha_recepcion
        {
            get { return this._fecha_recepcion; }
        }

        private int _terminal_cobro;
        /// <summary>
        /// Atributo encargado de almacenar la Terminal de Cobro
        /// </summary>
        public int terminal_cobro
        {
            get { return this._terminal_cobro; }
        }

        private bool _bit_original;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Original
        /// </summary>
        public bool bit_original
        {
            get { return this._bit_original; }
        }

        private bool _bit_copia;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Copia
        /// </summary>
        public bool bit_copia
        {
            get { return this._bit_copia; }
        }

        private bool _bit_sello;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Sello
        /// </summary>
        public bool bit_sello
        {
            get { return this._bit_sello; }
        }

        private int _id_imagen;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Imagen
        /// </summary>
        public int id_imagen
        {
            get { return this._id_imagen; }
        }

        private string _referencia_imagen;
        /// <summary>
        /// Atributo encargado de almacenar la Referencia de la Imagen
        /// </summary>
        public string referencia_imagen
        {
            get { return this._referencia_imagen; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }


        #endregion

        #region Enumeraciones

        /// <summary>
        /// Define los estatus existentes que puede tener un documento
        /// </summary>
        public enum EstatusDocumento
        {
            /// <summary>
            /// Documento recibido en terminal de cobro
            /// </summary>
            Recibido = 1,
            /// <summary>
            /// Documento recibido en terminal distinta a la de cobro
            /// </summary>
            Recibido_Reenvio,
            /// <summary>
            /// Documento en transito (dentro de paquete)
            /// </summary>
            Transito,
            /// <summary>
            /// Documento en aclaración (No se ha localizado fisicamente en el paquete actual)
            /// </summary>
            En_Aclaracion,
            /// <summary>
            /// Documento no recibido (no aplicable directamente a un registro, pero es requerido para actualización de dependencias)
            /// </summary>
            No_Recibido,
            /// <summary>
            /// Documento Cancelado (por motivos de insercción de paradas se re realiza la cancelación correspondiente)
            /// </summary>
            Cancelado,
             /// <summary>
            /// Solo se han Digitalizados Imagenes
            /// </summary>
            Imagen_Digitalizada =7
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ControlEvidenciaDocumento()
        {   //Invocando Método de carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ControlEvidenciaDocumento(int id_registro)
        {   //Invocando Método de carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ControlEvidenciaDocumento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Inicializando Valores
            this._id_control_evidencia_documento = 0;
            this._id_servicio_control_evidencia = 0;
            this._id_servicio = 0;
            this._id_segmento = 0;
            this._id_segmento_control_evidencia = 0;
            this._id_tipo_documento = 0;
            this._id_estatus_documento = 0;
            this._id_hoja_instruccion_documento = 0;
            this._terminal_recepcion = 0;
            this._fecha_recepcion = DateTime.MinValue;
            this._terminal_cobro = 0;
            this._bit_original = false;
            this._bit_copia = false;
            this._bit_sello = false;
            this._id_imagen = 0;
            this._referencia_imagen = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando variable de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, false, false, false, 0, "", 0, false, "", "" };
            //Obteniendo Tabla de Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la Tabla contenga Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo las Filas de la Tabla
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_control_evidencia_documento = id_registro;
                        this._id_servicio_control_evidencia = Convert.ToInt32(dr["IdServicioControlEvidencia"]);
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_segmento_control_evidencia = Convert.ToInt32(dr["IdSegmentoControlEvidencia"]);
                        this._id_segmento = Convert.ToInt32(dr["IdSegmento"]);
                        this._id_tipo_documento = Convert.ToByte(dr["IdTipoDocumento"]);
                        this._id_estatus_documento = Convert.ToByte(dr["IdEstatusDocumento"]);
                        this._id_hoja_instruccion_documento = Convert.ToInt32(dr["IdHojaInstruccionDocumento"]);
                        this._terminal_recepcion = Convert.ToInt32(dr["TerminalRecepcion"]);
                        DateTime.TryParse(dr["FechaRecepcion"].ToString(), out _fecha_recepcion);
                        this._terminal_cobro = Convert.ToInt32(dr["TerminalCobro"]);
                        this._bit_original = Convert.ToBoolean(dr["Original"]);
                        this._bit_copia = Convert.ToBoolean(dr["Copia"]);
                        this._bit_sello = Convert.ToBoolean(dr["Sello"]);
                        this._id_imagen = Convert.ToInt32(dr["IdImagen"]);
                        this._referencia_imagen = dr["ReferenciaImagen"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Asignando Variable a Positivo
                    result = true;
                }
            }//Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de las Actualizacion en la BD
        /// </summary>
        /// <param name="id_servicio_control_evidencia">Id de Servicio Control de Evidencia</param>
        /// <param name="id_servicio">Id de Sevicio</param>
        /// <param name="id_segmento_control_evidencia">Id Segmento Control Evidencia</param>
        /// <param name="id_segmento">Id Segmento</param>
        /// <param name="id_tipo_documento">Id de Tipo de Documento</param>
        /// <param name="id_estatus_documento">Estatus del Documento</param>
        /// <param name="id_hoja_instruccion_documento">Hoja de Instrucción</param>
        /// <param name="terminal_recepcion">Terminal de Recepción</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="terminal_cobro">Terminal de Cobro</param>
        /// <param name="bit_original">Origina</param>
        /// <param name="bit_copia">Copia</param>
        /// <param name="bit_sello">Sello</param>
        /// <param name="id_imagen">Id de Imagen</param>
        /// <param name="referencia_imagen">Referencia de Imagen</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitado</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_servicio_control_evidencia, int id_servicio, int id_segmento_control_evidencia, int id_segmento, 
                                                byte id_tipo_documento, EstatusDocumento id_estatus_documento, int id_hoja_instruccion_documento, int terminal_recepcion,
                                                DateTime fecha_recepcion, int terminal_cobro, bool bit_original,
                                                bool bit_copia, bool bit_sello, int id_imagen, string referencia_imagen,
                                                int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Arreglo de Parametros
            object[] param = { 2,this._id_control_evidencia_documento,id_servicio_control_evidencia,id_servicio, id_segmento_control_evidencia, id_segmento,  id_tipo_documento,
                                 (byte)id_estatus_documento,id_hoja_instruccion_documento,terminal_recepcion,
                               Fecha.ConvierteDateTimeObjeto(fecha_recepcion),
                                 terminal_cobro,bit_original,bit_copia,bit_sello,id_imagen,referencia_imagen,
                                 id_usuario,habilitar,"","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado
            return result;
        }
      

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de la Insercion de los Registros
        /// </summary>
        /// <param name="id_servicio_control_evidencia">Id Sevicio Control de Evidencia</param>
        /// <param name="id_servicio">Id de Servcio</param>
        /// <param name="id_segmento_control_evidencia">Id Segmento Control Evidencia</param>
        /// <param name="id_segmento">Id Segmento</param>
        /// <param name="id_tipo_documento">Id de Tipo de Documento</param>
        /// <param name="id_estatus_documento">Estatus del Documento</param>
        /// <param name="id_hoja_instruccion_documento">Hoja de Instrucción</param>
        /// <param name="terminal_recepcion">Terminal de Recepción</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="terminal_cobro">Terminal de Cobro</param>
        /// <param name="bit_original">Origina</param>
        /// <param name="bit_copia">Copia</param>
        /// <param name="bit_sello">Sello</param>
        /// <param name="id_imagen">Id de Imagen</param>
        /// <param name="referencia_imagen">Referencia de Imagen</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaControlEvidenciaDocumento(int id_servicio_control_evidencia, int id_servicio, int id_segmento_control_evidencia, int id_segmento, byte id_tipo_documento,
                                                EstatusDocumento id_estatus_documento, int id_hoja_instruccion_documento, int terminal_recepcion,
                                                DateTime fecha_recepcion, int terminal_cobro, bool bit_original,
                                                bool bit_copia, bool bit_sello, int id_imagen, string referencia_imagen,
                                                int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando Arreglo de Parametros
                object[] param = { 1,0, id_servicio_control_evidencia,id_servicio,id_segmento_control_evidencia, id_segmento, id_tipo_documento,
                                 (byte)id_estatus_documento,id_hoja_instruccion_documento,terminal_recepcion,
                                 Fecha.ConvierteDateTimeObjeto(fecha_recepcion),
                                 terminal_cobro,bit_original,bit_copia,bit_sello,id_imagen,referencia_imagen,
                                 id_usuario,true,"","" };

                //Declarando Variable Auxiliar
                int id_control_evidencia = 0;

                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                //Si se Inserto Correctamente Control Evidencia Documento
                if (result.OperacionExitosa)
                {
                    //Asignando Control Evidencia
                    id_control_evidencia = result.IdRegistro;
                    
                    //Instanciamos SegmentoControlEvidencia
                    using (SegmentoControlEvidencia sce = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmentoControlEvidencia, id_segmento_control_evidencia))
                    {
                        //Actualizamos Estatus
                        result = sce.ActualizaEstatusSegmentoControlEvidenciaSegmento(id_usuario);
                    }
                }
                //Validamos Resultado
                if (result.OperacionExitosa)
                {
                    //Actualizamos Estatus
                    result = new RetornoOperacion(id_control_evidencia);
                        
                    //Validamos Transacción
                    scope.Complete();
                }
            }
            //Devolviendo Resultado
            return result;
        }
        /// <summary>
        /// Método Público encargado de la Edicion de los Registros
        /// </summary>
        /// <param name="id_servicio_control_evidencia">Id Servicio  de Control de Evidencia</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_segmento_control_evidencia">Id Segmento Control Evidencia</param>
        /// <param name="id_segmento">Id Segmento</param>
        /// <param name="id_tipo_documento">Id de Tipo de Documento</param>
        /// <param name="id_estatus_documento">Estatus del Documento</param>
        /// <param name="id_hoja_instruccion_documento">Hoja de Instrucción</param>
        /// <param name="terminal_recepcion">Terminal de Recepción</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="terminal_cobro">Terminal de Cobro</param>
        /// <param name="bit_original">Origina</param>
        /// <param name="bit_copia">Copia</param>
        /// <param name="bit_sello">Sello</param>
        /// <param name="id_imagen">Id de Imagen</param>
        /// <param name="referencia_imagen">Referencia de Imagen</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaControlEvidenciaDocumento(int id_servicio_control_evidencia, int id_servicio, int id_segmento_control_evidencia, int id_segmento, byte id_tipo_documento,
                                                EstatusDocumento id_estatus_documento, int id_hoja_instruccion_documento, int terminal_recepcion,
                                                DateTime fecha_recepcion, int terminal_cobro, bool bit_original,
                                                bool bit_copia, bool bit_sello, int id_imagen, string referencia_imagen,
                                                int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_servicio_control_evidencia, id_servicio, id_segmento_control_evidencia, id_segmento,id_tipo_documento,
                                 id_estatus_documento, id_hoja_instruccion_documento, terminal_recepcion,
                                 fecha_recepcion, terminal_cobro, bit_original, bit_copia, bit_sello,
                                 id_imagen, referencia_imagen, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Ediitamos Control Evidencia Documento
        /// </summary>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaControlEvidenciaDocumentoRecibido(DateTime fecha_recepcion, int id_terminal_recepcion, int id_terminal_cobro, EstatusDocumento estatus, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion result = new RetornoOperacion();
            int id_control_evidencia = 0;
             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {                

                //Invocando Método de Actualización
                result = this.actualizaRegistros(this._id_servicio_control_evidencia, this._id_servicio, this._id_segmento_control_evidencia, this._id_segmento, this._id_tipo_documento,
                                     estatus, this._id_hoja_instruccion_documento, id_terminal_recepcion,
                                     fecha_recepcion, id_terminal_cobro, this._bit_original, this._bit_copia, this._bit_sello,
                                     this._id_imagen, this._referencia_imagen, id_usuario, this._habilitar);

                //Si se Inserto Correctamente Control Evidencia Documento
                if (result.OperacionExitosa)
                {
                    //Asignando Control Evidencia
                    id_control_evidencia = result.IdRegistro;

                    //Instanciamos SegmentoControlEvidencia
                    using (SegmentoControlEvidencia sce = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmentoControlEvidencia, id_segmento_control_evidencia))
                    {
                        //Actualizamos Estatus
                        result = sce.ActualizaEstatusSegmentoControlEvidenciaSegmento(id_usuario);
                    }
                }
                //Validamos Resultado
                if (result.OperacionExitosa)
                {
                    //Actualizamos Estatus
                    result = new RetornoOperacion(id_control_evidencia);

                    //Validamos Transacción
                    scope.Complete();
                }
            }
            //Declaramos Objeto resultado
            return result;
        }
        /// <summary>
        /// Método Público encargado de la Eliminacion de los Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaControlEvidenciaDocumento(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_servicio_control_evidencia, this._id_servicio, this._id_segmento_control_evidencia, this._id_segmento, this._id_tipo_documento,
                                 this.id_estatus_documento, this._id_hoja_instruccion_documento, this._terminal_recepcion,
                                 this._fecha_recepcion, this._terminal_cobro, this._bit_original, this._bit_copia, this._bit_sello,
                                 this._id_imagen, this._referencia_imagen, id_usuario, false);
        }

        /// <summary>
        /// Método Público encargado de la Eliminacion de los Registros
        /// </summary>
        /// <returns></returns>
        public bool ActualizaControlEvidenciaDocumento()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_control_evidencia_documento);
        }




        /// <summary>
        /// Actualizamos Estatus de Control Evidencia Documento
        /// </summary>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusControlEvidenciaDocumento(EstatusDocumento estatus, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos Variable para fecha de recepción del Documento
            DateTime fecha_recepcion = this._fecha_recepcion;

            //Declaramos IdControl Evidencia
            int IdControl = 0;

            //Validamos que no se encuentre cancelado el Documento
            if((EstatusDocumento)this._id_estatus_documento != EstatusDocumento.Cancelado)
            { 

                //Validamos que el estatus sea Recibido o Recibido con Reención para la asignación de la fecha de recepción
                if (estatus == EstatusDocumento.Recibido || estatus == EstatusDocumento.Recibido_Reenvio)
                {
                    fecha_recepcion = Fecha.ObtieneFechaEstandarMexicoCentro();
                }

                //Actualizamos Estatus del Documento
                resultado = this.actualizaRegistros(this._id_servicio_control_evidencia, this._id_servicio, this._id_segmento_control_evidencia, this._id_segmento, this._id_tipo_documento, estatus, this._id_hoja_instruccion_documento,
                                                    this._terminal_recepcion, this._fecha_recepcion, this._terminal_cobro, this._bit_original,
                                                    this._bit_copia, this._bit_sello, this._id_imagen, this._referencia_imagen, id_usuario, this._habilitar);
                //Establecemos Id del Control Evidencia
                IdControl = resultado.IdRegistro;

                //Si se actualizo Correctamente el Registro
                if (resultado.OperacionExitosa)
                {
                    //Instanciamos Viaje Control Evidencia 
                    using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmentoControlEvidencia, this._id_segmento_control_evidencia))
                    {
                        //Actualizamos Viaje Control Evidencia 
                        resultado = objSegmentoControlEvidencia.ActualizaEstatusSegmentoControlEvidenciaSegmento(id_usuario);
                        //Sila actualización fue correcta
                        if (resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion(IdControl);
                        }

                    }
                }
                
            }
            return resultado;

        }
        /// <summary>
        /// Actualizamos Estatus de Control Evidencia Documento
        /// </summary>
        /// <param name="id_terminal_cobro"></param>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusControlEvidenciaDocumento(int id_terminal_cobro, EstatusDocumento estatus, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos Variable para fecha de recepción del Documento
            DateTime fecha_recepcion = this._fecha_recepcion;

            //Declaramos IdControl Evidencia
            int IdControl = 0;

            //Validamos que no se encuentre cancelado el Documento
            if ((EstatusDocumento)this._id_estatus_documento != EstatusDocumento.Cancelado)
            {

                //Validamos que el estatus sea Recibido o Recibido con Reención para la asignación de la fecha de recepción
                if (estatus == EstatusDocumento.Recibido || estatus == EstatusDocumento.Recibido_Reenvio)
                {
                    fecha_recepcion = Fecha.ObtieneFechaEstandarMexicoCentro();
                }

                //Actualizamos Estatus del Documento
                resultado = this.actualizaRegistros(this._id_servicio_control_evidencia, this._id_servicio, this._id_segmento_control_evidencia, this._id_segmento, this._id_tipo_documento, estatus, this._id_hoja_instruccion_documento,
                                                    this._terminal_recepcion, this._fecha_recepcion, id_terminal_cobro, this._bit_original,
                                                    this._bit_copia, this._bit_sello, this._id_imagen, this._referencia_imagen, id_usuario, this._habilitar);
                //Establecemos Id del Control Evidencia
                IdControl = resultado.IdRegistro;

                //Si se actualizo Correctamente el Registro
                if (resultado.OperacionExitosa)
                {
                    //Instanciamos Viaje Control Evidencia 
                    using (SegmentoControlEvidencia objSegmentoControlEvidencia = new SegmentoControlEvidencia(SegmentoControlEvidencia.TipoConsulta.IdSegmentoControlEvidencia, this._id_segmento_control_evidencia))
                    {
                        //Actualizamos Viaje Control Evidencia 
                        resultado = objSegmentoControlEvidencia.ActualizaEstatusSegmentoControlEvidenciaSegmento(id_usuario);
                        //Sila actualización fue correcta
                        if (resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion(IdControl);
                        }

                    }
                }

            }
            return resultado;

        }
        /// <summary>
        /// Carga el listado de estatus de documentos de un viaje y los asigna a los parámetros de salida para su uso posterior
        /// </summary>
        /// <param name="id_control_evidencia_segmento">Id de control de evidencia</param>
        /// <param name="recibido">Documentos recibidos</param>
        /// <param name="recibido_reenvio">Documentos recibidos con reenvio</param>
        /// <param name="transito">Documentos en transito</param>
        /// <param name="en_acalaracion">Documentos en aclaración</param>
        /// <param name="no_recibido">Documentos no recibidos</param>
        /// <param name="cancelado">Documentos Cancelados</param>
        /// <param name="imagen_digitalizada">Documentos Con Imagen Digitalizada</param>
        public static void CargaEstatusDocumentosControlEvidencia(int id_control_evidencia_segmento, out bool recibido, out bool recibido_reenvio,
            out bool transito, out bool en_acalaracion, out bool no_recibido, out bool cancelado, out bool imagen_digitalizada)
        {
            //Asignando parametros de salida
            recibido = recibido_reenvio = transito = en_acalaracion = cancelado = imagen_digitalizada= false;
            no_recibido = true;

            //Definiendo arreglo de parámetros para consulta
            object[] param = { 4, 0, 0, 0, id_control_evidencia_segmento, 0, 0, 0,0,0, null, 0, false, false, false, 0, "", 0, false, "", "" };

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
                    cancelado = (from DataRow r in ds.Tables["Table"].Rows
                                   where Convert.ToByte(r["IdEstatus"]) == 6
                                   select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                    imagen_digitalizada = (from DataRow r in ds.Tables["Table"].Rows
                                 where Convert.ToByte(r["IdEstatus"]) == 7
                                 select Convert.ToBoolean(r["Documentos"])).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Recupera aquellos documentos de una compañía o emisor que deben ser enviados a su lugar de cobro (destino), desde un lugar de recepción(origen).
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía - Emisor</param>
        /// <param name="id_lugar_origen">Id de Lugar de Origen</param>
        /// <param name="id_lugar_destino">Id de Lugar de Destino</param>
        /// <returns></returns>
        public static DataTable CargaDocumentosEnvioPendiente(int id_compania_emisor, int id_lugar_origen, int id_lugar_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando arreglo de valores para filtrado de consulta a realizar
            object[] param = { 5, 0, 0, 0, 0, 0, 0, 0, 0, id_lugar_origen, null, id_lugar_destino, false, false, false, 0, "", 0, false, id_compania_emisor, "" };

            //Realizando consulta requerida
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existen resultados 
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando valor a retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        
        /// <summary>
        /// Recupera los documentos de unun registro control evidencia solicitado
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>    
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <returns></returns>
        public static DataTable CargaDocumentosControlEvidencia(int id_servicio, int id_operador, int id_unidad)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando arreglo de valores para filtrado de consulta a realizar
            object[] param = { 6, 0, 0, id_servicio, 0, 0, 0, 0, 0, 0, null, 0, false, false, false, 0, "", 0, false, id_operador, id_unidad};

            //Realizando consulta requerida
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existen resultados 
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando valor a retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
 
        /// <summary>
        /// Realiza la carga de todas las imagenes de documentos ligados a un Id de Control de Evidencia
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable ObtieneControlEvidenciaDocumentosImagenes(int id_servicio)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 7, 0, 0, id_servicio, 0, 0, 0 ,0, 0, 0, null, 0, false, false, false, 0, "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Realiza la carga de todas las imagenes de documentos ligados a un Segmento
        /// </summary>
        /// <param name="id_servicio">Servicio o Viaje</param>
        /// <param name="id_segmento">Segmento del Viaje</param>
        /// <returns></returns>
        public static DataTable ObtieneControlEvidenciaDocumentosImagenes(int id_servicio, int id_segmento)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 8, 0, 0, id_servicio, 0, id_segmento, 0, 0, 0, 0, null, 0, false, false, false, 0, "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        
        /// <summary>
        /// Realiza la carga de todas documentos ligados a un Segmento Control Evidencia
        /// </summary>
        /// <param name="id_segmento_control_evidencia">Segmento Control Evidencia</param>
        /// <returns></returns>
        public static DataTable CargaDocumentosControlEvidencia(int id_segmento_control_evidencia)
        {
            //Definiendo objeto de resultado
            DataTable mit = null; 

            //Declarando arreglo de parámetros de consulta
            object[] param = { 9, 0, 0, 0,  id_segmento_control_evidencia, 0, 0, 0, 0, 0, null, 0, false, false, false, 0, "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

       /// <summary>
       /// Cancela Control Evidencia Documento
       /// </summary>
       /// <param name="id_segmento_control_evidencia">Id Segmento Control Evidencia Documento</param>
       /// <param name="objSegmentoControEvlidencia">Objeto Segmento Control Evidencia</param>
       /// <param name="id_usuario">Id Usuario</param>
       /// <returns></returns>
       public static RetornoOperacion CancelaControlEvidenciaDocumento(int id_segmento_control_evidencia, SegmentoControlEvidencia objSegmentoControEvlidencia, int id_usuario)
        {
            //Objeto Resultado
            RetornoOperacion res = new RetornoOperacion(0);
             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos Evidencias de Documento
                using (DataTable mitDocumento = CargaDocumentosControlEvidencia(id_segmento_control_evidencia))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mitDocumento))
                    {
                        //Recorremos Cada uno de los Documento
                        foreach (DataRow r in mitDocumento.Rows)
                        {
                            //Instanciamos Documento
                            using (ControlEvidenciaDocumento doc = new ControlEvidenciaDocumento(r.Field<int>("Id")))
                            {
                                //Actualizamos Estatus
                                res = doc.ActualizaEstatusControlEvidenciaDocumento(ControlEvidenciaDocumento.EstatusDocumento.Cancelado, id_usuario);

                                //Validamos Resultado
                                if (res.OperacionExitosa)
                                {
                                    //Cargamos detalle del paquete correspondiente al Documento
                                    using (DataTable mit = PaqueteEnvioDocumento.ObtieneDetallesPaqueteDocumento(doc.id_control_evidencia_documento))
                                    {
                                        //Validamos Origen de Datos
                                        if (Validacion.ValidaOrigenDatos(mit))
                                        {
                                            //Recorremos cada uno de los paquetes
                                            foreach (DataRow p in mit.Rows)
                                            {
                                                //Insertamos Refrencia
                                                res = Referencia.InsertaReferencia(p.Field<int>("Id"), 41, 2070, "Reacomodo de los documentos a causa de la edición del segmento", Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);

                                                //Validamos Resultado
                                                if (!res.OperacionExitosa)
                                                {
                                                    //Salimos del ciclo
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Validamos Resultado
                            if (!res.OperacionExitosa)
                            {
                                //Salimos del ciclo
                                break;
                            }
                        }
                    }
                    //Validamos Resultado
                    if (res.OperacionExitosa)
                    {
                        //Cancelamos Segmento
                        res = objSegmentoControEvlidencia.EditaEstatusSegmentoControlEvidencia(SegmentoControlEvidencia.EstatusSegmentoControlEvidencias.Cancelado, id_usuario);
                    }
                }
                //Validamos Resultado
                if(res.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
            //Devolvemos resultado
          return res;

        }
        #endregion
    }
}
