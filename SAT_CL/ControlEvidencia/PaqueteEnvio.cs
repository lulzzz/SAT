using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Linq;
using System.Transactions;
namespace SAT_CL.ControlEvidencia
{
    /// <summary>
    /// Clase encargado de las operaciones de los Envios de Paquetes
    /// </summary>
    public class PaqueteEnvio : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_paquete_envio_tpe";

        private int _id_paquete_envio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Paquete de Envio
        /// </summary>
        public int id_paquete_envio
        {
            get { return this._id_paquete_envio; }
        }

        private int _id_compania_emisor;
        /// <summary>
        /// Obtiene el Id de compañía del paquete
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }

        private int _id_terminal_origen;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Terminal de Origen
        /// </summary>
        public int id_terminal_origen
        {
            get { return this._id_terminal_origen; }
        }

        private int _id_terminal_destino;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Terminal de Destino
        /// </summary>
        public int id_terminal_destino
        {
            get { return this._id_terminal_destino; }
        }

        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus
        /// </summary>
        public EstatusPaqueteEnvio id_estatus
        {
            get { return (EstatusPaqueteEnvio)this._id_estatus; }
        }

        private DateTime _fecha_salida;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Salida
        /// </summary>
        public DateTime fecha_salida
        {
            get { return this._fecha_salida; }
        }

        private DateTime _fecha_llegada;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Llegada
        /// </summary>
        public DateTime fecha_llegada
        {
            get { return this._fecha_llegada; }
        }

        private int _id_medio_envio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Medio de Envio
        /// </summary>
        public int id_medio_envio
        {
            get { return this._id_medio_envio; }
        }

        private string _referencia_envio;
        /// <summary>
        /// Atributo encargado de almacenar la Referencia del Envio
        /// </summary>
        public string referencia_envio
        {
            get { return this._referencia_envio; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Enumeraciones

        /// <summary>
        /// Define los estatus que puede tener el control de evidencias de viaje
        /// </summary>
        public enum EstatusPaqueteEnvio
        {
            /// <summary>
            /// Paquete Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Paquete en transito
            /// </summary>
            Transito,
            /// <summary>
            /// Paquete Recibido
            /// </summary>
            Recibido,
            /// <summary>
            /// Paquete en Aclaración (Existe un paquete documento en aclaración)
            /// </summary>
            En_Aclaracion,
            /// <summary>
            /// No se recibio paquete
            /// </summary>
            No_Recibido,
            /// <summary>
            /// Paquete Parcial(Existe un paquete documento en transito)
            /// </summary>
            Recibido_Parcial,
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Valores por Default
        /// </summary>
        public PaqueteEnvio()
        {   //Invocanco Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Valores dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public PaqueteEnvio(int id_registro)
        {   //Invocanco Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Valores dado un Origen, Destino y Compania
        /// </summary>
        /// <param name="id_origen">Id de Origen</param>
        /// <param name="id_destino">Id de Destino</param>
        /// <param name="id_compania">Id de Compania</param>
        public PaqueteEnvio(int id_origen, int id_destino, int id_compania)
        {   //Invocanco Método de Carga
            cargaAtributosInstancia(id_origen, id_destino, id_compania);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~PaqueteEnvio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando variable de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
            //Obteniedo Tabla de Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos de la Tabla Obtenida
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila de la Tabla
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_paquete_envio = id_registro;
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_terminal_origen = Convert.ToInt32(dr["IdTerminalOrigen"]);
                        this._id_terminal_destino = Convert.ToInt32(dr["IdTerminalDestino"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        DateTime.TryParse(dr["FechaSalida"].ToString(), out _fecha_salida);
                        DateTime.TryParse(dr["FechaLlegada"].ToString(), out _fecha_llegada);
                        this._id_medio_envio = Convert.ToInt32(dr["IdMedioEnvio"]);
                        this._referencia_envio = dr["ReferenciaEnvio"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Asignando Resultado Positivo
                } result = true;
            }//Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Origen, Destino y Compania
        /// </summary>
        /// <param name="id_origen">Id de Origen</param>
        /// <param name="id_destino">Id de Destino</param>
        /// <param name="id_compania">Id de Compania</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_origen, int id_destino, int id_compania)
        {   //Declarando variable de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 5, 0, id_compania, id_origen, id_destino, 0, null, null, 0, "", 0, false, "", "" };
            //Obteniedo Tabla de Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos de la Tabla Obtenida
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila de la Tabla
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_paquete_envio = Convert.ToInt32(dr["Id"]);
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_terminal_origen = Convert.ToInt32(dr["IdTerminalOrigen"]);
                        this._id_terminal_destino = Convert.ToInt32(dr["IdTerminalDestino"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        DateTime.TryParse(dr["FechaSalida"].ToString(), out _fecha_salida);
                        DateTime.TryParse(dr["FechaLlegada"].ToString(), out _fecha_llegada);
                        this._id_medio_envio = Convert.ToInt32(dr["IdMedioEnvio"]);
                        this._referencia_envio = dr["ReferenciaEnvio"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Asignando Resultado Positivo
                } result = true;
            }//Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía Emisor</param>
        /// <param name="id_terminal_origen">Terminal de Origen</param>
        /// <param name="id_terminal_destino">Terminal de Destino</param>
        /// <param name="id_estatus">estatus</param>
        /// <param name="fecha_salida">Fecha de Salida</param>
        /// <param name="fecha_llegada">Fecha de Llegada</param>
        /// <param name="id_medio_envio">Medio de Envio</param>
        /// <param name="referencia_envio">Referencia del Envio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitado</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_compania_emisor, int id_terminal_origen, int id_terminal_destino, EstatusPaqueteEnvio id_estatus,
                                                        DateTime fecha_salida, DateTime fecha_llegada, int id_medio_envio,
                                                        string referencia_envio, int id_usuario, bool habilitar)
        {   //Declarando objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros del SP
            object[] param = { 2, this._id_paquete_envio, id_compania_emisor, id_terminal_origen, id_terminal_destino, (byte)id_estatus, 
                                                       Fecha.ConvierteDateTimeObjeto(fecha_salida), 
                                                       Fecha.ConvierteDateTimeObjeto(fecha_llegada), 
                                                       id_medio_envio, referencia_envio, 
                                                       id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Registros
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía Emisor</param>
        /// <param name="id_terminal_origen">Terminal de Origen</param>
        /// <param name="id_terminal_destino">Terminal de Destino</param>
        /// <param name="id_medio_envio">Medio de Envio</param>
        /// <param name="referencia_envio">Referencia del Envio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPaqueteEnvio(int id_compania_emisor, int id_terminal_origen, int id_terminal_destino,
                                                        int id_medio_envio, string referencia_envio, int id_usuario)
        {   //Declarando objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros del SP
            object[] param = { 1, 0, id_compania_emisor, id_terminal_origen, id_terminal_destino, (byte)EstatusPaqueteEnvio.Registrado, 
                                           null, null, id_medio_envio, referencia_envio, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Público encargado de Editar los Registros
        /// </summary>
        /// <param name="id_medio_envio">Medio de Envio</param>
        /// <param name="referencia_envio">Referencia del Envio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaPaqueteEnvio(int id_medio_envio, string referencia_envio, int id_usuario)
        {   //Invocando y Devolviendo Método de Actualización
            return this.actualizaRegistros(this._id_compania_emisor, this._id_terminal_origen, this._id_terminal_destino, this.id_estatus,
                                           this._fecha_salida, this._fecha_llegada, id_medio_envio,
                                           referencia_envio, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Eliminar los Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPaqueteEnvio(int id_usuario)
        {   //Invocando y Devolviendo Método de Actualización
            return this.actualizaRegistros(this._id_compania_emisor, this._id_terminal_origen, this._id_terminal_destino, (EstatusPaqueteEnvio)this._id_estatus,
                                           this._fecha_salida, this._fecha_llegada, this._id_medio_envio,
                                           this._referencia_envio, id_usuario, false);
        }

        /// <summary>
        /// Método encargado de deshabilitar el registro y sus dependencias
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPaqueteEnvioCascada(int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

            //Validando el estatus del paquete, sólo Registrado puede ser eliminado
            if (this.id_estatus == EstatusPaqueteEnvio.Registrado)
            {
                //Deshabilitando los detalles
                resultado = PaqueteEnvioDocumento.DeshabilitaPaqueteEnvioDocumentos(this._id_paquete_envio, id_usuario);
                //Si se deshabilitaron los detalles
                if (resultado.OperacionExitosa)
                    //Realizando deshabilitación de este registros
                    resultado = DeshabilitaPaqueteEnvio(id_usuario);
            }
            else
            {
                resultado = new RetornoOperacion("Un paquete sólo puede ser eliminado cuando su estatus es 'Registrado'.");
            }
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
        public bool ActualizaPaqueteEnvio()
        {   //Invocando Método de carga
            return this.cargaAtributosInstancia(this._id_paquete_envio);
        }


        /// <summary>
        /// Actualiza el Estatus del Paquete 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusGeneralPaquete(int id_usuario)
        {
            //Declaramos Variable Estatus
            bool Registrado, Transito, Recibido, EnAclaracion, NoRecibido;

            //Declaramos estatus
            EstatusPaqueteEnvio estatus = EstatusPaqueteEnvio.No_Recibido;

            //Cargamos Estatus de los Paquetes 
            PaqueteEnvioDocumento.CargaEstatusPaqueteEnvioDocumento(this._id_paquete_envio, out Registrado, out Transito,
                                                                      out Recibido, out EnAclaracion, out NoRecibido);
            //Determinamos estatua para actualizar el Paquete
            //Si existe por lo menos un documento en aclaración
            if (EnAclaracion)
            {
                estatus = EstatusPaqueteEnvio.En_Aclaracion;

            }
            else
            {
                //Si existe solo documentos Recibidos
                if (!Registrado && !Transito && Recibido && !EnAclaracion && !NoRecibido)
                {
                    estatus = EstatusPaqueteEnvio.Recibido;

                }
                else
                {
                    //Si existe solo documentos en NO Recibidos
                    if (!Registrado && !Transito && !Recibido && !EnAclaracion && NoRecibido)
                    {
                        estatus = EstatusPaqueteEnvio.No_Recibido;

                    }
                    else
                    {
                        //Si existe solo documentos en transito
                        if (Transito)
                        {
                            //Si por lo meno existe un documento Recibido
                            if (Recibido)
                                estatus = EstatusPaqueteEnvio.Recibido_Parcial;
                            else if (Registrado)
                                estatus = EstatusPaqueteEnvio.Registrado;
                            else
                                //Asignamos Estatus a Transito
                                estatus = EstatusPaqueteEnvio.Transito;
                        }
                        else
                        {
                            //Asignamos estatus a Recibido
                            estatus = EstatusPaqueteEnvio.Recibido;
                        }
                    }
                }
            }

            //Declaramos Fecha de llegada y Salida
            DateTime fecha_llegada = this._fecha_llegada;
            DateTime fecha_salida = this._fecha_salida;


            // Validamos si el Estatus  acual es a Registrado y el estatus a asignar  es Transito
            if ((EstatusPaqueteEnvio)this._id_estatus == EstatusPaqueteEnvio.Registrado && estatus == EstatusPaqueteEnvio.Transito)
            {

                //Actualizamos la fecha de salida
                fecha_salida = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            }
            else
            {
                // Validamos que el estatus Actua y por asignar se ha diferente de Registrado
                if ((EstatusPaqueteEnvio)this._id_estatus != EstatusPaqueteEnvio.Registrado && estatus != EstatusPaqueteEnvio.Registrado)
                {

                    //Actualizamos la fecha de llegada              
                    fecha_llegada =Fecha.EsFechaMinima(this._fecha_llegada) ? TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro() : this._fecha_llegada;
                }

            }

            //Actualizamos Estatus del Paquete
            return this.actualizaRegistros(this._id_compania_emisor, this._id_terminal_origen, this._id_terminal_destino, estatus, fecha_salida,
                                          fecha_llegada, this._id_medio_envio, this._referencia_envio, id_usuario, this._habilitar);
     }


        /* /// <summary>
         /// Método Público encargado de Determinar los Detalles ligados a los Paquetes
         /// </summary>
         /// <param name="id_paquete">Id del Paquete</param>
         /// <param name="id_usuario">Id de Usuario</param>
         /// <returns></returns>
         public static cEstatusPaquete(int id_paquete, int id_usuario)
         {   //Declarando Objeto de Retorno
             RetornoOperacion result = new RetornoOperacion();
             //Armando arreglo de parametros
             object[] param = { 4, id_paquete, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
             //Obteniendo Datos Solicitados
             using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
             {   //Validando que la Tabla contenga Registros
                 if (Herramientas.ValidaOrigenDatos(ds, "Table"))
                 {   //Instanciando Paquete de Envio
                     using (PaqueteEnvio pe = new PaqueteEnvio(id_paquete))
                     {   //Declarando variable de contencion de de los Id de Detalles
                         byte[] id_estatus_detalles = new byte[ds.Tables["Table"].Rows.Count];
                         //Indice del recorrido
                         int indice = 0;
                         //Recorriendo los registros de la Tabla
                         foreach (DataRow dr in ds.Tables["Table"].Rows)
                         {   //Almacenando Id's en el Arreglo
                             id_estatus_detalles[indice] = Convert.ToByte(dr["IdEstatus"]);
                             //Incrementando Indice
                             indice++;
                         }
                         //Validando que exista mas de un Detalle
                         if (id_estatus_detalles.Length > 1)
                         {   //COMPARANDO QUE LOS VALORES DEL ARREGLO SEAN IGUALES
                             //Declarando Variable de Resultado de la Comparación
                             bool compare = true;
                             //Declarando Condición de los Estatus
                             Predicate<byte> match;
                             //Indice a Comparar
                             for (int j = 0; j <= (id_estatus_detalles.Length-1); j++)
                             {   //Si son diferentes
                                 if (id_estatus_detalles[0] != id_estatus_detalles[j])
                                 {   //Asignando comparacion en Falso
                                     compare = false;
                                     //Terminando Ciclo for
                                     j = id_estatus_detalles.Length + 1;
                                 }
                             }
                             //Validando si son iguales
                             if (compare)
                                 //Obteniendo Resultado de la Actualización
                                 result = pe.ActualizaEstatusPaqueteEnvio(id_estatus_detalles[0], id_usuario);
                             else
                             {   //Asignando el Predicado para evaluar el Estatus "En Aclaración" ==> 4
                                 match = delegate(byte a) { return a == 4; };
                                 //Validando si existe algún valor que concuerde con el predicado 
                                 if (Array.Exists(id_estatus_detalles, match))
                                     //Obteniendo Resultado de la Actualización
                                     result = pe.ActualizaEstatusPaqueteEnvio((byte)4, id_usuario);
                                 else
                                 {   //Asignando otras Validaciones de Estatus
                                     //match = delegate(byte b) { return b == 2 || b == 5; };
                                     //Validando que Existan los Estatus
                                     //if (Array.Exists(id_estatus_detalles, match))
                                         //Obteniendo Resultado de la Actualización
                                         result = pe.ActualizaEstatusPaqueteEnvio((byte)6, id_usuario);
                                 }
                             }
                         }
                         else if (id_estatus_detalles.Length == 1)
                             //Obteniendo Resultado de la Actualización
                             result = pe.ActualizaEstatusPaqueteEnvio(id_estatus_detalles[0], id_usuario);
                     }
                 }
             }//Devolviendo Resultado Obtenido
             return result;
         }*/

        /// <summary>
        /// Método encargado de cargar los Paquetes ligado a una Compania, Terminal Origen, Terminal Destino
        /// </summary>
        /// <param name="id_Compania"></param>
        /// <param name="id_terminal_origen"></param>
        /// <param name="id_terminal_destino"></param>
        /// <returns></returns>
        public static DataTable CargaPaquetesEnRecepcion(int id_Compania, int id_terminal_origen, int id_terminal_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 6, 0, id_Compania, id_terminal_origen, id_terminal_destino, 0, null, null, 0, "", 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Método encargado de  cargar los Paquetes ligado a una Compania, Terminal Origen, Terminal Destino
        /// </summary>
        /// <param name="id_Compania"></param>
        /// <param name="id_terminal_origen"></param>
        /// <param name="id_terminal_destino"></param>
        /// <returns></returns>
        public static DataTable CargaPaquetes(int id_Compania, int id_terminal_origen, int id_terminal_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 6, 0, id_Compania, id_terminal_origen, id_terminal_destino, 0, null, null, 0, "", 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga las Imagenes de los Documentos ligado a un paquete
        /// </summary>
        /// <param name="id_paquete"></param>
        /// <returns></returns>
        public static DataTable CargaPaquetesDocumentosImagenes(int id_paquete)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 7, id_paquete, 0, 0, 0, 0, null, null, 0, "", 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion
    }
}
