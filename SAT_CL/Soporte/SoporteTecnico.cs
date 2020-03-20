using System;
using System.Data;
using TSDK.Base;
using System.Configuration;


namespace SAT_CL.Soporte
{
    public class SoporteTecnico : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los posibles etstaus de una Orden de Trabajo
        /// </summary>
        public enum EstatusSoporteTecnico
        {
            /// <summary>
            /// Estatus que indica que la orden esta en progreso
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Estatus que indica que la orden esta activa sin embargo todas las actividades estan pausadas
            /// </summary>
            Inicio,
            /// <summary>
            /// Estatus que indica que la orden esta terminada
            /// </summary>
            Cancelado,
            /// <summary>
            /// Estatus que indica que la orden esta terminada
            /// </summary>
            Terminado

        }
        /// <summary>
        /// Define los tipos existentes de Orden de TRabajo
        /// </summary>
        public enum TipoSoporteTecnico
        {
            /// <summary>
            /// Tipo estandar de orden de trabajo
            /// </summary>
            Estandar = 1
        }

        #endregion
       
        #region Atributos
        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "soporte.sp_soporte_tecnico_tst";
        private int _id_soporte_tecnico;
        /// <summary>
        /// Atributo encargado de almacenar el Id Soporte Tecnico
        /// </summary>
        public int id_soporte_tecnico { get { return this._id_soporte_tecnico; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo encargado de almacenar el Id Compañia Emisora
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        private int _no_consecutivo_compania;
        /// <summary>
        /// Atributo que almacena el No Consecutivo de Compañia
        /// </summary>
        public int no_consecutivo_compania { get { return this._no_consecutivo_compania; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Id Estatus 
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        private int _id_usuario_asistente;
        /// <summary>
        /// Atributo encargado de almacenar Id Usuario Asistente
        /// </summary>
        public int id_usuario_asistente { get { return this._id_usuario_asistente; } }
        private string _usuario_solicitante;
        /// <summary>
        /// Atributo encargado de almacenar al usuario solicitante
        /// </summary>
        public string usuario_solicitante { get { return this._usuario_solicitante; } }
        private DateTime _fecha_inicio_general;
        /// <summary>
        /// Atributo encargado de almacenar Fecha Inicio General
        /// </summary>
        public DateTime fecha_inicio_general { get { return this._fecha_inicio_general; } }
        private DateTime _fecha_termino_general;
        /// <summary>
        /// Atributo encargado de almacenar Fecha Termino General
        /// </summary>
        public DateTime fecha_termino_general { get { return this._fecha_termino_general; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        ///<summary>
        /// Atributos para consulta
        private byte _id_soporte;
        /// <summary>
        /// Atributo encargado de almacenar el Id Soporte
        /// </summary>
        public byte id_soporte { get { return this._id_soporte; } }

        private string _observacion;
        /// <summary>
        /// Atributo encargado de almacenar observacion 
        /// </summary>
        public string observacion { get { return this._observacion; } }
        ///// <summary>
        /// Atributo que almacena 
        /// </summary>
        public EstatusSoporteTecnico EstatusSoporte { get { return (EstatusSoporteTecnico)_id_estatus; } }






        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public SoporteTecnico()
        {
            //Asignando Valores
            this._id_soporte_tecnico = 0;
            this._id_compania_emisora = 0;
            this._no_consecutivo_compania = 0;
            this._id_estatus = 0;
            this._id_usuario_asistente = 0;
            this._usuario_solicitante = "";
            this._fecha_inicio_general = DateTime.MinValue;
            this._fecha_termino_general = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_soporte_tecnico">Id de Registro</param>
        public SoporteTecnico(int id_soporte_tecnico)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_soporte_tecnico);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~SoporteTecnico()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Registro
        /// </summary>
        /// <param name="id_soporte_tecnico">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_soporte_tecnico)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de parametros
            object[] param = { 3, id_soporte_tecnico, 0, 0, 0, 0, "", null, null, 0, false, "", "", "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_soporte_tecnico = id_soporte_tecnico;
                        this._id_compania_emisora = Convert.ToInt32(dr["IdCompaniaEmisora"]);
                        this._no_consecutivo_compania = Convert.ToInt32(dr["NoConsecutivoCompania"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_usuario_asistente = Convert.ToInt32(dr["IdUsuarioAsistente"]);
                        this._usuario_solicitante = dr["UsuarioSolicitante"].ToString();
                        DateTime.TryParse(dr["FechaInicioGeneral"].ToString(), out this._fecha_inicio_general);
                        DateTime.TryParse(dr["FechaTerminoGeneral"].ToString(), out this._fecha_termino_general);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo resultado obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario Soporte Tecnico</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_compania_emisora, int no_consecutivo_compania, byte id_estatus, int id_usuario_asistente, string usuario_solicitante,
                                                     DateTime fecha_inicio_general, DateTime fecha_termino_general, int id_usuario, bool habilitar)
        {   //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2, this._id_soporte_tecnico, id_compania_emisora, no_consecutivo_compania, id_estatus, id_usuario_asistente, usuario_solicitante,
                                  fecha_inicio_general, fecha_termino_general, id_usuario, habilitar, "", "", "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

      
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario Soporte Tecnico</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroFecha(int id_compania_emisora, int no_consecutivo_compania, byte id_estatus, int id_usuario_asistente, string usuario_solicitante,
                                                     DateTime fecha_inicio_general,int id_usuario, bool habilitar)
        {   //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 5, this._id_soporte_tecnico, id_compania_emisora, no_consecutivo_compania, id_estatus, id_usuario_asistente, usuario_solicitante,
                                  fecha_inicio_general, null, id_usuario, habilitar, "", "", "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar soporte tecnico
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario Soporte Tecnico</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSoporteTecnico(int id_compania_emisora, int no_consecutivo_compania, byte id_estatus, int id_usuario_asistente, string usuario_solicitante,
                                                     DateTime fecha_inicio_general, int id_usuario)
        {
            //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1, 0, id_compania_emisora, no_consecutivo_compania, id_estatus, id_usuario_asistente,  usuario_solicitante, fecha_inicio_general,
                              null, id_usuario, true, "", "", "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar Soporte Tecnico
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario del Soporte Tecnico</param>
        /// <returns></returns>
        public RetornoOperacion EditaSoporteTecnico(int id_compania_emisora, int no_consecutivo_compania, byte id_estatus, int id_usuario_asistente, string usuario_solicitante,
                                                    DateTime fecha_inicio_general, DateTime fecha_termino_general, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroBD(id_compania_emisora, no_consecutivo_compania, id_estatus, id_usuario_asistente, usuario_solicitante, fecha_inicio_general,
                               fecha_termino_general, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Editar Soporte Tecnico
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario del Soporte Tecnico</param>
        /// <returns></returns>
        public RetornoOperacion EditaSoporteTecnicoFechaF(int id_compania_emisora, int no_consecutivo_compania, byte id_estatus, int id_usuario_asistente, string usuario_solicitante,
                                                    DateTime fecha_inicio_general, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroFecha(id_compania_emisora, no_consecutivo_compania, id_estatus, id_usuario_asistente, usuario_solicitante, fecha_inicio_general,
                                               id_usuario, this._habilitar);
        }



        /// <summary>
        /// Método Público encargado de Editar Soporte Tecnico
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario del Soporte Tecnico</param>
        /// <returns></returns>
        public RetornoOperacion TerminaSoporteTecnico(DateTime fecha_termino_general, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroBD(this._id_compania_emisora, this._no_consecutivo_compania,3, this._id_usuario_asistente, this._usuario_solicitante, this._fecha_inicio_general,
                               fecha_termino_general, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar el Soporte Tecnico
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaSoporteTecnico(int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroBD(this._id_compania_emisora, this._no_consecutivo_compania, this._id_estatus, this._id_usuario_asistente, this._usuario_solicitante, this._fecha_inicio_general,
                                           this._fecha_termino_general, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos del Soporte Tecnico
        /// </summary>
        /// <returns></returns>
        public bool ActualizaSoporteTecnico()
        {
            //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_soporte_tecnico);
        }


        /// <summary>
        /// Método Público encargado de Insertar soporte tecnico
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario Soporte Tecnico</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSoporteTecnico(int id_compania_emisora, int no_consecutivo_compania, byte id_estatus, int id_usuario_asistente, string usuario_solicitante,
                                                     DateTime fecha_inicio_general, DateTime fecha_termino_general, int id_usuario)
        {
            //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1, 0, id_compania_emisora, no_consecutivo_compania, id_estatus, id_usuario_asistente,  usuario_solicitante, fecha_inicio_general,
                              fecha_termino_general, id_usuario, true, "", "", "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado Obtener soporte detalles
        /// </summary>
        /// <param name="id_soporte_tecnico_detalle">Detalle de Orden de Compra</param>
        /// <returns></returns>
        public static DataTable ObtieneSoporte(string solicitante, int id_compania, int tipo, DateTime fecha_inicio, DateTime fecha_fin, string observacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtSoporteTecnico = null;
            
            //Creación del arreglo necesario para la obtención de datos del sp de la tabla orden compra detalle
            object[] param = { 6, 0, id_compania, 0, 0, 0, solicitante, null, null, 0, false, observacion, tipo, fecha_inicio == DateTime.MinValue ? null : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), fecha_fin == DateTime.MinValue ? null : fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) };
            //Realiza la transaccion con la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asigna a la tabla los valores encontrados
                    dtSoporteTecnico = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtSoporteTecnico;
        }
        #endregion
    }
}
