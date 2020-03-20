using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Linq;
using System.Transactions;

namespace SAT_CL.ControlEvidencia
{   
    /// <summary>
    /// Clase encargada de todas las operaciones de las Hojas de Instrucción
    /// </summary>
    public class HojaInstruccion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_hoja_instruccion_thi";

        private int _id_hoja_instruccion;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Hoja de Instrucción
        /// </summary>
        public int id_hoja_instruccion
        {
            get { return this._id_hoja_instruccion; }
        }

        private string _descripcion_hoja_instruccion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción de la Hoja de Instrucción
        /// </summary>
        public string descripcion_hoja_instruccion
        {
            get { return this._descripcion_hoja_instruccion; }
        }

        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compania del Emisor
        /// </summary>
        public int id_compania_emisor
        {
            get { return this._id_compania_emisor; }
        }

        private int _id_cliente_receptor;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Cliente Receptor
        /// </summary>
        public int id_cliente_receptor
        {
            get { return this._id_cliente_receptor; }
        }

        private int _id_remitente;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Remitente
        /// </summary>
        public int id_remitente
        {
            get { return this._id_remitente; }
        }

        private int _id_destinatario;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Destinatario
        /// </summary>
        public int id_destinatario
        {
            get { return this._id_destinatario; }
        }

        private int _id_terminal_cobro;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Terminal de Cobro
        /// </summary>
        public int id_terminal_cobro
        {
            get { return this._id_terminal_cobro; }
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

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos pro Default
        /// </summary>
        public HojaInstruccion()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public HojaInstruccion(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }


        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~HojaInstruccion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   
           
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
            object[] param = { 3, id_registro, "", 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Tabla de Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {   //Validando que la Tabla contenga Registros
                if(Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada fila de la Tabla
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_hoja_instruccion = id_registro;
                        this._descripcion_hoja_instruccion = dr["DescripcionHojaInstruccion"].ToString();
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_cliente_receptor = Convert.ToInt32(dr["IdClienteReceptor"]);
                        this._id_remitente = Convert.ToInt32(dr["IdRemitente"]);
                        this._id_destinatario = Convert.ToInt32(dr["IdDestinatario"]);
                        this._id_terminal_cobro = Convert.ToInt32(dr["IdTerminalCobro"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Asignando Positiva la variable de Retorno
                    result = true;
                }
            }//Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="descripcion_hoja_instruccion">Descripción de la Hoja de Instrucción</param>
        /// <param name="id_compania_emisor">Id de Compania del Emisor</param>
        /// <param name="id_cliente_receptor">Id de Cliente del Receptor</param>
        /// <param name="id_remitente">Id del Remitente</param>
        /// <param name="id_destinatario">Id del Destinatario</param>
        /// <param name="id_terminal_cobro">Id de la Terminal de Cobro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitado</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion_hoja_instruccion,
                        int id_compania_emisor,	int id_cliente_receptor, int id_remitente, 
                        int id_destinatario, int id_terminal_cobro, int id_usuario,
                        bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2,this._id_hoja_instruccion,descripcion_hoja_instruccion,id_compania_emisor,id_cliente_receptor,
                                 id_remitente,id_destinatario,id_terminal_cobro,id_usuario,
                                 habilitar,"","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Variable de Retorno
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar registros
        /// </summary>
        /// <param name="descripcion_hoja_instruccion">Descripción de la Hoja de Instrucción</param>
        /// <param name="id_compania_emisor">Id de Compania del Emisor</param>
        /// <param name="id_cliente_receptor">Id de Cliente del Receptor</param>
        /// <param name="id_remitente">Id del Remitente</param>
        /// <param name="id_destinatario">Id del Destinatario</param>
        /// <param name="id_terminal_cobro">Id de la Terminal de Cobro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarHojaInstruccion(string descripcion_hoja_instruccion,
                        int id_compania_emisor, int id_cliente_receptor, int id_remitente,
                        int id_destinatario, int id_terminal_cobro, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1,0,descripcion_hoja_instruccion,id_compania_emisor,id_cliente_receptor,
                                 id_remitente,id_destinatario,id_terminal_cobro,id_usuario,
                                 true,"","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Variable de Retorno
            return result;
        }

        /// <summary>
        /// Método Público encargado de Editar registros
        /// </summary>
        /// <param name="descripcion_hoja_instruccion">Descripción de la Hoja de Instrucción</param>
        /// <param name="id_compania_emisor">Id de Compania del Emisor</param>
        /// <param name="id_cliente_receptor">Id de Cliente del Receptor</param>
        /// <param name="id_remitente">Id del Remitente</param>
        /// <param name="id_destinatario">Id del Destinatario</param>
        /// <param name="id_terminal_cobro">Id de la Terminal de Cobro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarHojaInstruccion(string descripcion_hoja_instruccion,
                        int id_compania_emisor, int id_cliente_receptor, int id_remitente,
                        int id_destinatario, int id_terminal_cobro, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion_hoja_instruccion, id_compania_emisor, id_cliente_receptor,
                                 id_remitente, id_destinatario, id_terminal_cobro, id_usuario,
                                 this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar los Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaHojaInstruccion(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion_hoja_instruccion, this._id_compania_emisor, this._id_cliente_receptor,
                                 this._id_remitente, this._id_destinatario, this._id_terminal_cobro, id_usuario,
                                 false);
        }



        /// <summary>
        /// Método Público encargado de Deshabilitar la HI y sus dependencias (documentos y accesorios)
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaHojaInstruccionCascada(int id_usuario)
        {
            //Definiendo objeto de resultado (sin errores)
            RetornoOperacion resultado = new RetornoOperacion(this._id_hoja_instruccion);
             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Deshabilitando documentos
                resultado = HojaInstruccionDocumento.DeshabilitaHojaInstruccionDocumentos(this._id_hoja_instruccion, id_usuario);

                //Si no existen errores
                if (resultado.OperacionExitosa)
                    //Deshabilitando accesorios
                    resultado = HojaInstruccionAccesorio.DeshabilitaHojaInstruccionAccesorios(this._id_hoja_instruccion, id_usuario);

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Deshabilitando HI
                    resultado = DeshabilitaHojaInstruccion(id_usuario);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Validamos Transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaHojaInstruccion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_hoja_instruccion);
        }

        /// <summary>
        /// Realiza la copia
        /// </summary>
        /// <param name="id_hoja_instruccion_original">Id de HI a copiar</param>
        /// <param name="descripcion_hoja_instruccion">Descripción de la Hoja de Instrucción</param>
        /// <param name="id_compania_emisor">Id de Compania del Emisor</param>
        /// <param name="id_cliente_receptor">Id de Cliente del Receptor</param>
        /// <param name="id_remitente">Id del Remitente</param>
        /// <param name="id_destinatario">Id del Destinatario</param>
        /// <param name="id_terminal_cobro">Id de la Terminal de Cobro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion CopiaHojaInstruccion(int id_hoja_instruccion_original, string descripcion_hoja_instruccion,
                        int id_compania_emisor, int id_cliente_receptor, int id_remitente,
                        int id_destinatario, int id_terminal_cobro, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

               //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizando la inserción del nuevo registro HI
                resultado = InsertarHojaInstruccion(descripcion_hoja_instruccion, id_compania_emisor, id_cliente_receptor, id_remitente, id_destinatario, id_terminal_cobro, id_usuario);

                //Conservando Id de registro
                int id_hoja_instruccion_copia = resultado.IdRegistro;

                //Si no existe ningún error
                if (resultado.OperacionExitosa)
                {
                    
                    //Cargando Documentos de la HI
                    using (DataTable documentos = HojaInstruccionDocumento.ObtieneHojaInstruccionDocumentos(id_hoja_instruccion_original))
                    {
                        //SI existen registros que copiar
                        if (Validacion.ValidaOrigenDatos(documentos))
                        {
                            //Para cada uno de los documentos
                            foreach (DataRow d in documentos.Rows)
                            {
                                //Insertando un nuevo documento a la nueva HI
                                resultado = HojaInstruccionDocumento.InsertarHojaInstruccionDocumento(id_hoja_instruccion_copia, Convert.ToInt32(d["IdTipoDocumento"]), Convert.ToBoolean(d["*Evidencia"]),
                                                                Convert.ToInt32(d["IdTipoEvento"]), Convert.ToInt32(d["IdRecepcionEntrega"]), Convert.ToInt32(d["IdCopiaOriginal"]),
                                                                Convert.ToBoolean(d["*Sello"]), d["Observacion"].ToString(), id_usuario);

                                //Si existe error
                                if (!resultado.OperacionExitosa)
                                    //Saliendo del ciclo 
                                    break;
                            }

                            //Si no existen errores
                            if (resultado.OperacionExitosa)
                            {
                                //Cargando accesorios
                                using (DataTable accesorios = HojaInstruccionAccesorio.ObtieneHojaInstruccionAccesorios(id_hoja_instruccion_original))
                                {
                                    //SI existen registros que copiar
                                    if (Validacion.ValidaOrigenDatos(accesorios))
                                    {
                                        //Para cada uno de los accesorios
                                        foreach (DataRow a in accesorios.Rows)
                                        {
                                            //Insertando un nuevo documento a la nueva HI
                                            resultado = HojaInstruccionAccesorio.InsertarHojaInstruccionAccesorio(id_hoja_instruccion_copia, Convert.ToInt32(a["IdAccesorio"]), Convert.ToInt32(a["IdTipoEvento"]),
                                                                a["Observacion"].ToString(), id_usuario);

                                            //Si existe error
                                            if (!resultado.OperacionExitosa)
                                                //Saliendo del ciclo 
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                        
                }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Reasignando Id de HI al resultado
                        resultado = new RetornoOperacion(id_hoja_instruccion_copia);
                        //Validamos Transacción
                        scope.Complete();
                    }
            }

            //Devolviendo resultado 
            return resultado;
        }

        /// <summary>
        /// Regresa  un Id de Hoja de Instrucción con la mejor coincidencia de Id Compañia, Id Cliente, Id Remitente, Id Destinatario
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_cliente"></param>
        /// <param name="id_remitente"></param>
        /// <param name="id_destinatario"></param>
        /// <returns></returns>
        public static int CargaHojaInstruccion(int id_compania, int id_cliente, int id_remitente, int id_destinatario)
        {
            //Declaramos variables
            int idHojaInstruccion = 0;

            //Armando Objeto de Parametros    
            object[] param = { 4, 0, "", id_compania , id_cliente, id_remitente, id_destinatario, 0, 0, false, "", "" };
           
            //Carga Detalles
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {
                idHojaInstruccion = (from DataRow r in mit.Rows
                                     select r.Field<int>("IdHojaInstruccion")).First();
            }

            return idHojaInstruccion;
        }

        /// <summary>
        /// Carga Referencias (Observaciones) de la Hoja de Instrucción 
        /// </summary>
        /// <param name="id_hoja_instruccion"></param>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static DataTable CargaReferenciasParaImpresion(int id_hoja_instruccion, int id_compania)
        {
            //Armando Objeto de Parametros    
            object[] param = { 5, id_hoja_instruccion, "", 0 , 0, 0, 0, 0, 0, false, id_compania, "" };
      

            //Realizando carga de registros
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {
                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Ruta de los Mapas ligado a la Hoja de Instrucción
        /// </summary>
        /// <param name="id_hoja_instruccion"></param>
        /// <returns></returns>
        public static DataTable CargaRutaMapas(int id_hoja_instruccion)
        {
            //Armando Objeto de Parametros    
            object[] param = { 6, id_hoja_instruccion, "", 0, 0, 0, 0, 0, 0, false, "", "" };


            //Realizando carga de registros
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de Obtener los Datos de la Hoja de Impresión
        /// </summary>
        /// <param name="id_segmento">Segmento del Servicio</param>
        /// <returns></returns>
        public static DataTable CargaImpresionHojaInstruccion(int id_segmento)
        {
            //Declarando Objeto de Retorno
            DataTable dtImpresion = null;
            
            //Armando Objeto de Parametros    
            object[] param = { 7, 0, "", 0, 0, 0, 0, 0, 0, false, id_segmento.ToString(), "" };

            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtImpresion = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtImpresion;
        }
        /// <summary>
        /// Método encargado de Obtener los Mapas de Carga y Descarga de la Hoja de Impresión
        /// </summary>
        /// <param name="id_hoja_instruccion">Hoja de Instrucción</param>
        /// <returns></returns>
        public static DataTable CargaMapasHojaInstruccion(int id_hoja_instruccion)
        {
            //Declarando Objeto de Retorno
            DataTable dtImpresion = null;

            //Armando Objeto de Parametros    
            object[] param = { 8, id_hoja_instruccion, "", 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtImpresion = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtImpresion;
        }

        #endregion
    }
}
