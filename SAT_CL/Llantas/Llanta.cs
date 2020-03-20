using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que permite Insertr, Actualizar y Consultar registros de una llanta
    /// </summary>
    public class Llanta:Disposable
    {
        #region Enumeración
        /// <summary>
        /// Enumeración de los estatus de una llanta
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Una llanta puede estar Montada
            /// </summary>
            Montada = 1,
            /// <summary>
            /// Una llanta puede estar desmontada
            /// </summary>
            Desmontada,
            /// <summary>
            /// Una llanta puede estar en almacen
            /// </summary>
            Almacen
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Almacena el nombre del store procedure de la tabla llanta
        /// </summary>
        private static string nom_sp = "llantas.sp_llanta_tll";

        private int _id_llanta;
        /// <summary>
        /// Almacena el identificador de un registro de llanta
        /// </summary>
        public int id_llanta
        {
            get { return _id_llanta; }
        }
        private int _id_compania;
        /// <summary>
        /// Identifica a que compañia pertenece el regsitro de llanta
        /// </summary>
        public int id_compania
        {
            get { return _id_compania; }
        }
        private int _id_tipo_llanta;
        /// <summary>
        /// Identifica el tipo de una llanta
        /// </summary>
        public int id_tipo_llanta
        {
            get { return _id_tipo_llanta; }
        }
        private string _serie_fabricante;
        /// <summary>
        /// Número consecutivo de fabrica de una llanta
        /// </summary>
        public string serie_fabricante
        {
            get { return _serie_fabricante; }
        }
        private string _no_llanta;
        /// <summary>
        /// Número consecutivo que permite identificar a una llanta de las demás
        /// </summary>
        public string no_llanta
        {
            get { return _no_llanta; }
        }
        private string _rfdi;
        /// <summary>
        /// Llave o chip que identifica a una llanta
        /// </summary>
        public string rfdi
        {
            get { return _rfdi; }
        }
        private string _dot;
        /// <summary>
        /// Código que almacena los datos de fabricación de una llanta
        /// </summary>
        public string dot
        {
            get { return _dot; }
        }
        private int _no_renovacion;
        /// <summary>
        /// Cantidad de renovaciones que ha tenido una llanta
        /// </summary>
        public int no_renovacion
        {
            get { return _no_renovacion; }
        }
        private int _mm_inicio;
        /// <summary>
        /// Medida en milimetros inicial de una llanta
        /// </summary>
        public int mm_inicio
        {
            get { return _mm_inicio; }
        }
        private int _mm_actuales;
        /// <summary>
        /// Medida en milimetros actuales de una llanta
        /// </summary>
        public int mm_actuales
        {
            get { return _mm_actuales; }
        }
        private DateTime _fecha_lectura;
        /// <summary>
        /// Almacena la fecha en la que se realizo un lectura de llantas
        /// </summary>
        public DateTime fecha_lectura
        {
            get { return _fecha_lectura; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Estatus de una llanta (Almacen, Montada, Desmontada)
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        /// <summary>
        /// Permite el acceso a la enumeración de los estus de la llanta
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this._id_estatus; }
        }
        private bool _habilitar;
        /// <summary>
        /// Define la disponiblidad de uso de un registro (Habilitado - Disponible, Deshabilitado - No Disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializa los atributos de la clase en cero
        /// </summary>
        public Llanta()
        {
            this._id_llanta = 0;
            this._id_compania = 0;
            this._id_tipo_llanta = 0;
            this._serie_fabricante = "";
            this._no_llanta = "";
            this._rfdi = "";
            this._dot = "";
            this._no_renovacion = 0;
            this._mm_inicio = 0;
            this._mm_actuales = 0;
            this._fecha_lectura = DateTime.MinValue;
            this._id_estatus = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro
        /// </summary>
        /// <param name="id_llanta">Identificador de un registro de Llanta</param>
        public Llanta(int id_llanta)
        {
            //Invoca al método que realiza la busqueda y asignación de valores a los atributos
            cargaAtributos(id_llanta);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Llanta()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda y asignación de valores a los atributos
        /// </summary>
        /// <param name="id_llanta">Identificador de un registro de llanta, Sirve como referencia para iniciar la busqueda</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_llanta)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro de llnata
            object[] param = { 3, id_llanta, 0, 0, "", "", "", "", 0, 0, 0, null, 0, 0, false, "", "" };
            //Invoca y asigna al dataset el método que realiza la busuqeda de un regsitro de llanta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los valores del dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos los campos del dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_llanta = id_llanta;
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._id_tipo_llanta = Convert.ToInt32(r["IdTipoLlanta"]);
                        this._serie_fabricante = Convert.ToString(r["SerieFabricante"]);
                        this._no_llanta = Convert.ToString(r["NoLlanta"]);
                        this._rfdi = Convert.ToString(r["RFDI"]);
                        this._dot = Convert.ToString(r["DOT"]);
                        this._no_renovacion = Convert.ToInt32(r["NoRenovacion"]);
                        this._mm_inicio = Convert.ToInt32(r["MMIinicio"]);
                        this._mm_actuales = Convert.ToInt32(r["MMActuales"]);
                        this._fecha_lectura = Convert.ToDateTime(r["FechaLectura"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno
                    retorno = true;
                }
            }
            //retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de llanta
        /// </summary>
        /// <param name="id_compania">Actualiza el identificador de una compañia</param>
        /// <param name="id_tipo_llanta">Actaliza el tipo de llanta</param>
        /// <param name="serie_fabricante">Actualiza la serie de fabrica de una llanta</param>
        /// <param name="no_llanta">Actualiza el numero que identifica a una llanta</param>
        /// <param name="rfdi">Actualiza el RFDI</param>
        /// <param name="dot">Actualiza el dot</param>
        /// <param name="no_renovacion">Actualiza la cantidad de renovaciones que ha tenido una llanta</param>
        /// <param name="mm_inicio">Actaliza los milimetros de medida inicial de una llanta</param>
        /// <param name="mm_actuales">Actaliza los milimetros de medida actuales de una llanta</param>
        /// <param name="fecha_lectura">Actualiza la fecha de lectura de una llanta</param>
        /// <param name="estatus">Actualiza el estado de una llanta (Almacen, Montada, Desmontada)</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro Habilitado - Disponible, Deshabilitado - No Disponible</param>
        /// <returns></returns>
        private RetornoOperacion editarLlanta(int id_compania, int id_tipo_llanta, string serie_fabricante, string no_llanta, string rfdi, string dot, int no_renovacion,
                                              int mm_inicio, int mm_actuales, DateTime fecha_lectura, Estatus estatus, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la actualización del registro
            object[] param = { 2, this._id_llanta, id_compania, id_tipo_llanta, serie_fabricante, no_llanta, rfdi, dot, no_renovacion, mm_inicio, mm_actuales, 
                               fecha_lectura, (Estatus)estatus, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el método que realiza la actualización del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que Inserta un registro de llanta
        /// </summary>
        /// <param name="id_compania">Inserta el identificador de una compañia</param>
        /// <param name="id_tipo_llanta">Inserta el tipo de llanta</param>
        /// <param name="serie_fabricante">Inserta la serie de fabrica de una llanta</param>
        /// <param name="no_llanta">Inserta el numero que identifica a una llanta</param>
        /// <param name="rfdi">Inserta el RFDI</param>
        /// <param name="dot">Inserta el dot</param>
        /// <param name="no_renovacion">Inserta la cantidad de renovaciones que ha tenido una llanta</param>
        /// <param name="mm_inicio">Inserta los milimetros de medida inicial de una llanta</param>
        /// <param name="mm_actuales">Inserta los milimetros de medida actuales de una llanta</param>
        /// <param name="fecha_lectura">Inserta la fecha de lectura de una llanta</param>
        /// <param name="estatus">Inserta es estado de una llanta (Almacen, Montada, Desmontada)</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarLlanta(int id_compania, int id_tipo_llanta, string serie_fabricante, string no_llanta, string rfdi, string dot, int no_renovacion,
                                                      int mm_inicio, int mm_actuales, DateTime fecha_lectura, Estatus estatus, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la inserción del registro
            object[] param = { 1, 0, id_compania, id_tipo_llanta, serie_fabricante, no_llanta, rfdi, dot, no_renovacion, mm_inicio, mm_actuales, 
                               fecha_lectura, (Estatus)estatus, id_usuario, true, "", "" };
            //Asigna al objeto retorno el método que realiza la Inserción del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de llanta
        /// </summary>
        /// <param name="id_compania">Actualiza el identificador de una compañia</param>
        /// <param name="id_tipo_llanta">Actaliza el tipo de llanta</param>
        /// <param name="serie_fabricante">Actualiza la serie de fabrica de una llanta</param>
        /// <param name="no_llanta">Actualiza el numero que identifica a una llanta</param>
        /// <param name="rfdi">Actualiza el RFDI</param>
        /// <param name="dot">Actualiza el dot</param>
        /// <param name="no_renovacion">Actualiza la cantidad de renovaciones que ha tenido una llanta</param>
        /// <param name="mm_inicio">Actaliza los milimetros de medida inicial de una llanta</param>
        /// <param name="mm_actuales">Actaliza los milimetros de medida actuales de una llanta</param>
        /// <param name="fecha_lectura">Actualiza la fecha de lectura de una llanta</param>
        /// <param name="estatus">Actualiza es estado de una llanta (Almacen, Montada, Desmontada)</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarLlantas(int id_compania, int id_tipo_llanta, string serie_fabricante, string no_llanta, string rfdi, string dot, int no_renovacion,
                                               int mm_inicio, int mm_actuales, DateTime fecha_lectura, Estatus estatus, int id_usuario)
        {
            //Retorna el método que actualiza los registros de una llanta
            return this.editarLlanta(id_compania, id_tipo_llanta, serie_fabricante, no_llanta, rfdi, dot, no_renovacion, mm_inicio, mm_actuales, fecha_lectura, (Estatus)estatus, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita el uso de un  registro
        /// </summary>
        /// <param name="id_usuario"> Identifica al usuario responsable de deshabilitar el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarLlantas(int id_usuario)
        {
            //Retorna el método que actualiza los registros de una llanta
            return this.editarLlanta(this._id_compania, this._id_tipo_llanta, this._serie_fabricante, this._no_llanta, this._rfdi, this._dot, this._no_renovacion, this._mm_inicio, this._mm_actuales, this._fecha_lectura, (Estatus)this._id_estatus, id_usuario, false);
        }
        
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaLlanta()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_llanta);
        }

        #endregion
    }
}
