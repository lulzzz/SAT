using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;

namespace SAT_CL.ControlEvidencia
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones de los Documentos de las Hojas de Instrucciones
    /// </summary>
    public class HojaInstruccionDocumento : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_hoja_instruccion_documento_thid";

        private int _id_hoja_instruccion_documento;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Documento de la Hoja de Instrucción
        /// </summary>
        public int id_hoja_instruccion_documento
        {
            get { return this._id_hoja_instruccion_documento; }
        }

        private int _id_hoja_instruccion;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Hoja de Instrucción
        /// </summary>
        public int id_hoja_instruccion
        {
            get { return this._id_hoja_instruccion; }
        }
        
        private int _id_tipo_documento;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Tipo de Documento
        /// </summary>
        public int id_tipo_documento
        {
            get { return this._id_tipo_documento; }
        }
        
        private bool _bit_evidencia;
        /// <summary>
        /// Atributo encargado de Almacenar el Bit de la Evidencia
        /// </summary>
        public bool bit_evidencia
        {
            get { return this._bit_evidencia; }
        }
        
        private int _id_tipo_evento;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Tipo de Evento
        /// </summary>
        public int id_tipo_evento
        {
            get { return this._id_tipo_evento; }
        }
        
        private int _id_recepcion_entrega;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Recepción de Entrega
        /// </summary>
        public int id_recepcion_entrega
        {
            get { return this._id_recepcion_entrega; }
        }
        
        private int _id_copia_original;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Copia Original
        /// </summary>
        public int id_copia_original
        {
            get { return this._id_copia_original; }
        }
        
        private bool _bit_sello;
        /// <summary>
        /// Atributo encargado de Almacenar el Bit del Sello
        /// </summary>
        public bool bit_sello
        {
            get { return this._bit_sello; }
        }
        
        private string _observaciones;
        /// <summary>
        /// Atributo encargado de Almacenar las Observaciones
        /// </summary>
        public string observaciones
        {
            get { return this._observaciones; }
        }
        
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Contructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public HojaInstruccionDocumento()
        {   //Invoncando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public HojaInstruccionDocumento(int id_registro)
        {   //Invoncando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~HojaInstruccionDocumento()
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
            this._id_hoja_instruccion_documento = 0;
            this._id_hoja_instruccion = 0;
	        this._id_tipo_documento = 0;
	        this._bit_evidencia = false;
	        this._id_tipo_evento = 0;
	        this._id_recepcion_entrega = 0;
            this._id_copia_original = 0;
            this._bit_sello = false;
	        this._observaciones = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Variable de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, 0, false, 0, 0, 0, false, "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que la Tabla contenga Registros
                if(Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada Fila de la Tabla
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_hoja_instruccion_documento = id_registro;
                        this._id_hoja_instruccion = Convert.ToInt32(dr["IdHojaInstruccion"]);
                        this._id_tipo_documento = Convert.ToInt32(dr["IdTipoDocumento"]);
                        this._bit_evidencia = Convert.ToBoolean(dr["BitEvidencia"]);
                        this._id_tipo_evento = Convert.ToInt32(dr["IdTipoEvento"]);
                        this._id_recepcion_entrega = Convert.ToInt32(dr["IdRecepcionEntrega"]);
                        this._id_copia_original = Convert.ToInt32(dr["IdCopiaOriginal"]);
                        this._bit_sello = Convert.ToBoolean(dr["BitSello"]);
                        this._observaciones = dr["Observaciones"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Asignando Resultado Positivo
                    result = true;
                }
            }//Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en la BD
        /// </summary>
        /// <param name="id_hoja_instruccion">Hoja de Instrucción</param>
        /// <param name="id_tipo_documento">Id Tipo del Documento</param>
        /// <param name="bit_evidencia">Bit de Evidencia</param>
        /// <param name="id_tipo_evento">Id de Tipo de Evento</param>
        /// <param name="id_recepcion_entrega">Id Recepción de Entrega</param>
        /// <param name="id_copia_original">Id Copia Original</param>
        /// <param name="bit_sello">Bit del Sello</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_hoja_instruccion, int id_tipo_documento,
	                            bool bit_evidencia, int id_tipo_evento, int id_recepcion_entrega,
                                int id_copia_original, bool bit_sello, string observaciones,
                                int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2,this._id_hoja_instruccion_documento, id_hoja_instruccion,
	                           id_tipo_documento, bit_evidencia,	id_tipo_evento,
	                           id_recepcion_entrega, id_copia_original, bit_sello,
	                           observaciones, id_usuario, habilitar, "",	"" };
            //Obteniendo resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Registros
        /// </summary>
        /// <param name="id_hoja_instruccion">Hoja de Instrucción</param>
        /// <param name="id_tipo_documento">Id Tipo del Documento</param>
        /// <param name="bit_evidencia">Bit de Evidencia</param>
        /// <param name="id_tipo_evento">Id de Tipo de Evento</param>
        /// <param name="id_recepcion_entrega">Id Recepción de Entrega</param>
        /// <param name="id_copia_original">Id Copia Original</param>
        /// <param name="bit_sello">Bit del Sello</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarHojaInstruccionDocumento(int id_hoja_instruccion, int id_tipo_documento,
                                bool bit_evidencia, int id_tipo_evento, int id_recepcion_entrega,
                                int id_copia_original, bool bit_sello, string observaciones,
                                int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1,0, id_hoja_instruccion,
	                           id_tipo_documento, bit_evidencia,	id_tipo_evento,
	                           id_recepcion_entrega, id_copia_original, bit_sello,
	                           observaciones, id_usuario, true, "", "" };
            //Obteniendo resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Público encargado de Editar los Registros
        /// </summary>
        /// <param name="id_hoja_instruccion">Hoja de Instrucción</param>
        /// <param name="id_tipo_documento">Id Tipo del Documento</param>
        /// <param name="bit_evidencia">Bit de Evidencia</param>
        /// <param name="id_tipo_evento">Id de Tipo de Evento</param>
        /// <param name="id_recepcion_entrega">Id Recepción de Entrega</param>
        /// <param name="id_copia_original">Id Copia Original</param>
        /// <param name="bit_sello">Bit del Sello</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarHojaInstruccionDocumento(int id_hoja_instruccion, int id_tipo_documento,
                                bool bit_evidencia, int id_tipo_evento, int id_recepcion_entrega,
                                int id_copia_original, bool bit_sello, string observaciones,
                                int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_hoja_instruccion,
                               id_tipo_documento, bit_evidencia, id_tipo_evento,
                               id_recepcion_entrega, id_copia_original, bit_sello,
                               observaciones, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar los Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaHojaInstruccionDocumento(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_hoja_instruccion,
                               this._id_tipo_documento, this._bit_evidencia, this._id_tipo_evento,
                               this._id_recepcion_entrega, this._id_copia_original, this._bit_sello,
                               this._observaciones, id_usuario, false);
        }

        /// <summary>
        /// Deshabilita todos los documentos ligados a la HI indicada
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de Hoja de Instrucciones</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaHojaInstruccionDocumentos(int id_hoja_instruccion, int id_usuario)
        { 
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_hoja_instruccion);

            //Cargando documentos
            using (DataTable mitDoc = ObtieneHojaInstruccionDocumentos(id_hoja_instruccion))
            {
                //Si existen resultados
                if (Validacion.ValidaOrigenDatos(mitDoc))
                {
                    //Para cada uno de los registros
                    foreach (DataRow d in mitDoc.Rows)
                    {
                        //instanciando registro
                        using (HojaInstruccionDocumento hid = new HojaInstruccionDocumento(Convert.ToInt32(d["Id"])))
                        {
                            //Si el registro existe
                            if (hid.id_hoja_instruccion_documento > 0)
                                //Deshabilitando Documento
                                resultado = hid.DeshabilitaHojaInstruccionDocumento(id_usuario);
                            else
                                resultado = new RetornoOperacion(string.Format("Documento 'ID: {0}' no encontrado.", Convert.ToInt32(d["Id"])));
                        }

                        //Si existe error
                        if (!resultado.OperacionExitosa)
                            //Saliendo de ciclo
                            break;
                    }
                }
            }


            //Devolviendo resultado
            return resultado;
        }


        /// <summary>
        /// Realiza la carga de todas las imagenes de documentos ligados a un Id de Hoja de Instrucciones de los segmentos
        /// </summary>
        /// <param name="id_servicio_control_evidencia">Id Servicio Control Evidencia</param>
        /// <returns></returns>
        public static DataTable ObtieneHojasDeInstruccionesDocumentosImagenes(int id_servicio_control_evidencia)
        { 
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 5, 0, 0, 0, false, 0, 0, 0, false, "", 0, false, id_servicio_control_evidencia, "" };

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
        /// Realiza la carga de todos los documentos ligados a un Id de Hoja de Instrucciones
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de HI</param>
        /// <returns></returns>
        public static DataTable ObtieneHojaInstruccionDocumentos(int id_hoja_instruccion)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 4, 0, id_hoja_instruccion, 0, false, 0, 0, 0, false, "", 0, false, "", "" };

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
        /// Carga la descripción de los documentos incluyendo la imagen correspondiente ligado a una hoja de Instrucción
        /// </summary>
        /// <param name="id_hoja_instruccion"></param>
        /// <returns></returns>
        public static DataTable CargaHojaInstruccionDocumentoParaImpresion(int id_hoja_instruccion)
        {

            //Declarando arreglo de parámetros de consulta
            object[] param = { 6, 0, id_hoja_instruccion, 0, false, 0, 0, 0, false, "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {
                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Atributos dado un Id
        /// </summary>
        /// <returns></returns>
        public bool ActualizaHojaInstruccionDocumento()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_hoja_instruccion_documento);
        }

        /// <summary>
        /// Realiza la carga de todas las imagenes de documentos ligando una Hoja de Instruccion
        /// </summary>
        /// <param name="id_hoja_instruccion">Id hoja de Instrucción</param>
        /// <returns></returns>
        public static DataTable ObtieneHojaInstruccionDocumentosImagenes(int id_hoja_instruccion)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 7, 0, id_hoja_instruccion, 0, false, 0, 0, 0, false, "", 0, false, "", "" };

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
        /// Realiza la carga de todos los Documentos ligados a una Hoja de Instrucción
        /// </summary>
        /// <param name="id_hoja_instruccion">Id hoja de Instrucción</param>
        /// <returns></returns>
        public static DataTable ObtieneDocumentosHIImpresion(int id_hoja_instruccion)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 8, 0, id_hoja_instruccion, 0, false, 0, 0, 0, false, "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                
                    //Devolviendo Resultado Obtenido
                    mit = ds.Tables["Table"];
                else
                {
                    //Creando "Tabla"
                    mit = new DataTable("Table");

                    //Añadiendo Columnas
                    mit.Columns.Add("Documento");
                    mit.Columns.Add("Evento");
                    mit.Columns.Add("Accion");
                    mit.Columns.Add("Formato");
                    mit.Columns.Add("Sello");
                    mit.Columns.Add("Observacion");
                    mit.Columns.Add("URL");

                    //Añadiendo Fila
                    mit.Rows.Add("", "", "", "", "", "", "");
                }

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Realiza la carga de todas las imagenes de documentos ligando una Hoja de Instruccion
        /// </summary>
        /// <param name="id_hoja_instruccion_documento">Documento de Hoja de Instrucción</param>
        /// <returns></returns>
        public static DataTable ObtieneImagenesHojaInstruccionDocumentos(int id_hoja_instruccion_documento)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 9, id_hoja_instruccion_documento, 0, 0, false, 0, 0, 0, false, "", 0, false, "", "" };

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

        #endregion
    }
}
