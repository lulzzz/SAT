using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones correspondientes a los Accesos a Patios
    /// </summary>
    public class AccesoPatio : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Tipo de Acceso
        /// </summary>
        public enum TipoAcceso
        {   /// <summary>
            /// Expresa que el Acceso al Patio es de tipo de Entrada
            /// </summary>
            Entrada = 1,
            /// <summary>
            /// Expresa que el Acceso al Patio es de tipo de Salida
            /// </summary>
            Salida
        }
        /// <summary>
        /// Enumeración que expresa el Tipo de Actualización
        /// </summary>
        public enum TipoActualizacion
        {   /// <summary>
            /// Expresa que la actualización fue atravez de la Aplicación Web
            /// </summary>
            Web = 1,
            /// <summary>
            /// Expresa que la actualización fue atravez de la Aplicación Mobil
            /// </summary>
            Mobil
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_acceso_patio_tap";
        
        private int _id_acceso_patio;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Acceso al Patio
        /// </summary>
        public int id_acceso_patio { get { return this._id_acceso_patio; } }
        private int _id_ubicacion_patio;
        /// <summary>
        /// Atributo encargado de Almacenar la Ubicación del Patio
        /// </summary>
        public int id_ubicacion_patio { get { return this._id_ubicacion_patio; } }
        private int _id_entidad_acceso;
        /// <summary>
        /// Atributo encargado de Almacenar la Entidad del Acceso al Patio
        /// </summary>
        public int id_entidad_acceso { get { return this._id_entidad_acceso; } }
        private byte _id_tipo_actualizacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Actualización
        /// </summary>
        public byte id_tipo_actualizacion { get { return this._id_tipo_actualizacion; } }
        /// <summary>
        /// Atributo encargado de Almacenar la Enumeración del Tipo de Actualización
        /// </summary>
        public TipoActualizacion tipo_actualizacion { get { return (TipoActualizacion)this._id_tipo_actualizacion; } }
        private byte _id_tipo_acceso;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Acceso
        /// </summary>
        public byte id_tipo_acceso { get { return this._id_tipo_acceso; } }
        /// <summary>
        /// Atributo encargado de Almacenar la Enumeración del Tipo de Acceso
        /// </summary>
        public TipoAcceso tipo_acceso { get { return (TipoAcceso)this._id_tipo_acceso; } }
        private DateTime _fecha_acceso;
        /// <summary>
        /// Atributo encargado de Almacenar la Fecha de Acceso
        /// </summary>
        public DateTime fecha_acceso { get { return this._fecha_acceso; } }
        private string _referencia;
        /// <summary>
        /// Atributo encargado de Almacenar la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public AccesoPatio()
        {   //Asignando Valores
            this._id_acceso_patio = 0;
            this._id_ubicacion_patio = 0;
            this._id_entidad_acceso = 0;
            this._id_tipo_actualizacion = 0;
            this._id_tipo_acceso = 0;
            this._fecha_acceso = DateTime.MinValue;
            this._referencia = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Contructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_acceso_patio"></param>
        public AccesoPatio(int id_acceso_patio)
        {   //Asignando Valores
            cargaAtributosInstancia(id_acceso_patio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~AccesoPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_acceso_patio">Id de Acceso de Patio</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_acceso_patio)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_acceso_patio, 0, 0, 0, 0, null, "", 0, false, "", "" };
            //Instanciando Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Filas
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_acceso_patio = id_acceso_patio;
                        this._id_ubicacion_patio = Convert.ToInt32(dr["IdUbicacionPatio"]);
                        this._id_entidad_acceso = Convert.ToInt32(dr["IdEntidadAcceso"]);
                        this._id_tipo_actualizacion = Convert.ToByte(dr["IdTipoActualizacion"]);
                        this._id_tipo_acceso = Convert.ToByte(dr["IdTipoAcceso"]);
                        DateTime.TryParse(dr["FechaAcceso"].ToString(), out this._fecha_acceso);
                        this._referencia = dr["Referencia"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Retorno Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="id_entidad_acceso">Entidad de Acceso</param>
        /// <param name="id_tipo_actuallizacion">Tipo de Actualización</param>
        /// <param name="id_tipo_acceso">Tipo de Acceso</param>
        /// <param name="fecha_acceso">Fecha del Acceso</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_ubicacion_patio, int id_entidad_acceso, byte id_tipo_actuallizacion, byte id_tipo_acceso, 
                                                    DateTime fecha_acceso, string referencia, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_acceso_patio, id_ubicacion_patio, id_entidad_acceso, id_tipo_actuallizacion, id_tipo_acceso, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_acceso), referencia, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Accesos a Patios
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="id_entidad_acceso">Entidad de Acceso</param>
        /// <param name="tipo_actualizacion">Tipo de Actualización</param>
        /// <param name="tipo_acceso">Tipo de Acceso</param>
        /// <param name="fecha_acceso">Fecha del Acceso</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAccesoPatio(int id_ubicacion_patio, int id_entidad_acceso, TipoActualizacion tipo_actualizacion, TipoAcceso tipo_acceso, 
                                                    DateTime fecha_acceso, string referencia, int id_usuario)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que la Fecha Ingresada sea Menor a la Fecha Actual 
            if (DateTime.Compare(fecha_acceso, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()) <= 0)
            {
                //Armando Arreglo de Parametros
                object[] param = { 1, 0, id_ubicacion_patio, id_entidad_acceso, (byte)tipo_actualizacion,  (byte)tipo_acceso, 
                                   TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_acceso), referencia, id_usuario, true, "", "" };
                
                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            }
            else
                //Instanciando Excepcion
                result = new RetornoOperacion("La Fecha Ingresada debe ser Menor a la Fecha Actual");
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Accesos a Patios
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="id_entidad_acceso">Entidad de Acceso</param>
        /// <param name="tipo_actualizacion">Tipo de Actualización</param>
        /// <param name="tipo_acceso">Tipo de Acceso</param>
        /// <param name="fecha_acceso">Fecha del Acceso</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaAccesoPatio(int id_ubicacion_patio, int id_entidad_acceso, TipoActualizacion tipo_actualizacion, TipoAcceso tipo_acceso,
                                                    DateTime fecha_acceso, string referencia, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_ubicacion_patio, id_entidad_acceso, (byte)tipo_actualizacion, (byte)tipo_acceso, 
                                fecha_acceso, referencia, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Accesos a Patios
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAccesoPatio(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_ubicacion_patio, this._id_entidad_acceso, this._id_tipo_actualizacion, this._id_tipo_acceso, 
                                this._fecha_acceso, this._referencia, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Accesos a Patios
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAccesoPatio()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_acceso_patio);
        }

        #endregion
    }
}
