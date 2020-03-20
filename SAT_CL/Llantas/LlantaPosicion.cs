using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que realiza las acciones de Lectura Edición e Inserción de registros en la tabla LlantasPosición 
    /// </summary>
    public class LlantaPosicion : Disposable
    {
        #region Enumeración
        /// <summary>
        /// Enumera los estado de una posición llanta
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// La posición de la llanta puede estar activa
            /// </summary>
            Activo =1,
            /// <summary>
            /// La posición de la llnata puede estar inactiva
            /// </summary>
            Inactivo
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla
        /// </summary>
        private static string nom_sp = "llantas.sp_llanta_posicion_tllp";

        private int _id_llanta_posicion;
        /// <summary>
        /// Almacena el identificador de la posición de una llanta
        /// </summary>
        public int id_llanta_posicion
        {
            get { return _id_llanta_posicion; }
        }
        private int _id_unidad;
        /// <summary>
        /// Almacena la unidad
        /// </summary>
        public int id_unidad
        {
            get { return _id_unidad; }
        }
        private int _id_llanta;
        /// <summary>
        /// Almacena la llanta de la unidad
        /// </summary>
        public int id_llanta
        {
            get { return _id_llanta; }            
        }
        private int _id_posicion;
        /// <summary>
        /// Almacena la posición de la llanta de la unidad
        /// </summary>
        public int id_posicion
        {
            get { return _id_posicion; }            
        }
        private byte _id_estatus;
        /// <summary>
        /// Almacena el estatus de posicion de la llanta (Activa / Inactiva)
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }            
        }
        /// <summary>
        /// Permite tener acceso a la enumeración de los estatus de una posición de la llanta
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this._id_estatus; }
        }

        private DateTime _fecha_montaje;
        /// <summary>
        /// Fecha en la que se monta una llanta a una posición
        /// </summary>
        public DateTime fecha_montaje
        {
            get { return _fecha_montaje; }
        }
        private int _id_actividad;
        /// <summary>
        /// Almacena la actividad de montaje de llanta de la unidad
        /// </summary>
        public int id_actividad
        {
            get { return _id_actividad; }
        }
        private DateTime _fecha_desmontaje;
        /// <summary>
        /// Fecha en la que se desmanta la llanta de la unidad
        /// </summary>
        public DateTime fecha_desmontaje
        {
            get { return _fecha_desmontaje; }            
        }
        private int _id_actividad_desmontaje;
        /// <summary>
        /// Almacena la actividad de desmontaje de llanta de la unidad
        /// </summary>
        public int id_actividad_desmontaje
        {
            get { return _id_actividad_desmontaje; }            
        }
        private string _razon_desmontaje;
        /// <summary>
        /// Almacena la razon por la cual se desmonto una llanta de la unidad
        /// </summary>
        public string razon_desmontaje
        {
            get { return _razon_desmontaje; }           
        }
        private bool _habilitar;
        /// <summary>
        /// Habilita o Deshabilita un registro para su uso en el sistema
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion
        
        #region Constructores
        /// <summary>
        /// Constructr que inicializa los atributos a cero
        /// </summary>
        public LlantaPosicion()
        {
            this._id_llanta_posicion = 0;
            this._id_unidad = 0;
            this._id_llanta = 0;
            this._id_posicion = 0;
            this._id_estatus = 0;
            this._fecha_montaje = DateTime.MinValue;
            this._id_actividad = 0;
            this._fecha_desmontaje = DateTime.MinValue;
            this._id_actividad_desmontaje = 0;
            this._razon_desmontaje = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de la consulta de un regsitro
        /// </summary>
        /// <param name="id_llanta_posicion">Id que sirve como referencia para la consulta y asignación de valores a los atributos</param>
        public LlantaPosicion(int id_llanta_posicion)
        {
            //Invoca al metodo que asigna valores a los atributos 
            cargaAtributos(id_llanta_posicion);
        }
        #endregion
        
        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~LlantaPosicion()
        {
            Dispose(false);
        }
        #endregion
        
        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busuqeda de un registro y almacena el resultado en los atributos de la clase
        /// </summary>
        /// <param name="id_llanta_posicion">Id del registro de llanta posición a buscar</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_llanta_posicion)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos para realizar la consulta del registro
            object[] param = { 3, id_llanta_posicion, 0, 0, 0, 0, null, 0, null, 0, "", 0, false, "", "" };
            //Realiza la instancia al método que realiza la busqueda del registro y el resultado lo almacena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan datos en el dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorer y asigna los valores del dataset a los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_llanta_posicion = id_llanta_posicion;
                        this._id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        this._id_llanta = Convert.ToInt32(r["IdLlanta"]);
                        this._id_posicion = Convert.ToInt32(r["IdPosicion"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._fecha_montaje = Convert.ToDateTime(r["FechaMontaje"]);
                        this._id_actividad = Convert.ToInt32(r["IdActividad"]);
                        this._fecha_desmontaje = Convert.ToDateTime(r["FechaDesmontaje"]);
                        this._id_actividad_desmontaje = Convert.ToInt32(r["IdActividadDesmontaje"]);
                        this._razon_desmontaje= Convert.ToString(r["RazonDesmontaje"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de Llanta Posición
        /// </summary>
        /// <param name="id_unidad">Actualiza el identificador de una unidad</param>
        /// <param name="id_llanta">Actualiza el identificador de una llanta</param>
        /// <param name="id_posicion">Actualiza el identificador de una posición</param>
        /// <param name="estatus">Actualiza el estatus de una Posició de la llanta (Activa / Inactiva)</param>
        /// <param name="fecha_montaje">Actualiza la fecha en la que fue montada la llanta</param>
        /// <param name="id_actividad">Actualiza el identificador de la actividad de montaje de la llanta</param>
        /// <param name="fecha_desmontaje">Actializa la fecha en que fue desmontada la llanta</param>
        /// <param name="id_actividad_desmontaje">Actualiza el identificador de la actividad de desmontaje de la unidad </param>
        /// <param name="razon_desmontaje">Actualiza el motivo por el cual se desmonto una llanta</param>
        /// <param name="id_usuario">Actualiza identificador del usuario que realizo la actualización de campos del registro llanta Posición </param>
        /// <param name="habilitar">Actualiza el estado de uso de registro (Habilitado-Disponible/ Deshabiilitado - No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarLlantaPosicion(int id_unidad, int id_llanta, int id_posicion, Estatus estatus, DateTime fecha_montaje, int id_actividad, 
                                                       DateTime fecha_desmontaje, int id_actividad_desmontaje, string razon_desmontaje, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creacion del arreglo que almacena los datos necesarios para reaizar la actualización de los campos del registro
            object[] param = { 2, this._id_llanta_posicion, id_unidad, id_llanta, id_posicion, (Estatus)estatus, fecha_montaje, id_actividad, fecha_desmontaje, id_actividad_desmontaje, razon_desmontaje, id_usuario, habilitar, "", "" };
            //Instancia a la clase que realiza la actualización de los campos del registro y el resultado lo almacena en el objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion
        
        #region Método Públicos
        /// <summary>
        /// Método que actualiza los campos de un registro de Llanta Posición
        /// </summary>
        /// <param name="id_unidad">Inserta el identificador de una unidad</param>
        /// <param name="id_llanta">Inserta el identificador de una llanta</param>
        /// <param name="id_posicion">Inserta el identificador de una posición</param>
        /// <param name="estatus">Inserta el estatus de una Posició de la llanta (Activa / Inactiva)</param>
        /// <param name="fecha_montaje">Inserta la fecha en la que fue montada la llanta</param>
        /// <param name="id_actividad">Inserta el identificador de la actividad de montaje de la llanta</param>
        /// <param name="fecha_desmontaje">Inserta la fecha en que fue desmontada la llanta</param>
        /// <param name="id_actividad_desmontaje">Inserta el identificador de la actividad de desmontaje de la unidad </param>
        /// <param name="razon_desmontaje">Inserta el motivo por el cual se desmonto una llanta</param>
        /// <param name="id_usuario">Inserta identificador del usuario que realizo la actualización de campos del registro llanta Posición </param>
        /// <param name="habilitar">Inserta el estado de uso de registro (Habilitado-Disponible/ Deshabiilitado - No Disponible)</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarLlantaPosicion(int id_unidad, int id_llanta, int id_posicion, Estatus estatus, DateTime fecha_montaje, int id_actividad,
                                                       DateTime fecha_desmontaje, int id_actividad_desmontaje, string razon_desmontaje, int id_usuario )
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creacion del arreglo que almacena los datos necesarios para reaizar la insercion de los campos del registro
            object[] param = { 1, 0, id_unidad, id_llanta, id_posicion, (Estatus)estatus, fecha_montaje, id_actividad, fecha_desmontaje, id_actividad_desmontaje, razon_desmontaje, id_usuario, true, "", "" };
            //RInstancia a la clase que realiza la insercion de los campos del registro y el resultado lo almacena en el objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de Llanta Posición
        /// </summary>
        /// <param name="id_unidad">Actualiza el identificador de una unidad</param>
        /// <param name="id_llanta">Actualiza el identificador de una llanta</param>
        /// <param name="id_posicion">Actualiza el identificador de una posición</param>
        /// <param name="estatus">Actualiza el estatus de una Posició de la llanta (Activa / Inactiva)</param>
        /// <param name="fecha_montaje">Actualiza la fecha en la que fue montada la llanta</param>
        /// <param name="id_actividad">Actualiza el identificador de la actividad de montaje de la llanta</param>
        /// <param name="fecha_desmontaje">Actializa la fecha en que fue desmontada la llanta</param>
        /// <param name="id_actividad_desmontaje">Actualiza el identificador de la actividad de desmontaje de la unidad </param>
        /// <param name="razon_desmontaje">Actualiza el motivo por el cual se desmonto una llanta</param>
        /// <param name="id_usuario">Actualiza identificador del usuario que realizo la actualización de campos del registro llanta Posición </param>
        /// <returns></returns>
        public RetornoOperacion EditarLlantaPosición(int id_unidad, int id_llanta, int id_posicion, Estatus estatus, DateTime fecha_montaje, int id_actividad,
                                                       DateTime fecha_desmontaje, int id_actividad_desmontaje, string razon_desmontaje, int id_usuario)
        {
            //invoca al método que actualiza los campos de un registro
            return this.editarLlantaPosicion(id_unidad, id_llanta, id_posicion, (Estatus)estatus, fecha_desmontaje, id_actividad, fecha_desmontaje, id_actividad_desmontaje, razon_desmontaje, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia de el estado del registro
        /// </summary>
        /// <param name="id_usuario">Identifica al usuarioq eu cambio el estado del registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarLlantaPosición(int id_usuario)
        {
            //invoca al método que actualiza los campos de un registro
            return this.editarLlantaPosicion(this._id_unidad, this._id_llanta, this._id_posicion, (Estatus)this._id_estatus, this._fecha_desmontaje, this._id_actividad, this._fecha_desmontaje, this._id_actividad_desmontaje, this._razon_desmontaje, id_usuario, false);
        }

        /// <summary>
        /// Carga Posiciones Actuales de Una Unidad
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <returns></returns>
        public static DataTable CargaPosicionesUnidad(int id_unidad)
        {
            //Creación del arreglo que almacena los datos para realizar la consulta del registro
            object[] param = { 4, 0, id_unidad, 0, 0, 0, null, 0, null, 0, "", 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Definiendo objeto de retorno
                DataTable mit = null;

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
