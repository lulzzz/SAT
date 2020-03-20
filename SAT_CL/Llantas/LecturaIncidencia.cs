using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que permite Consultar, Insertar y Actualizar registros de Lectura Incidencia
    /// </summary>
    public class LecturaIncidencia : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure
        /// </summary>
        private static string nom_sp = "llantas.sp_lectura_incidencia_tli";

        private int _id_lectura_incidencia;
        /// <summary>
        /// Almacena el registro de lectura incidencia
        /// </summary>
        public int id_lectura_incidencia
        {
            get { return _id_lectura_incidencia; }
        }
        private int _id_lectura;
        /// <summary>
        /// Almacena el identificador de una lectura de llanta
        /// </summary>
        public int id_lectura
        {
            get { return _id_lectura; }
        }
        private byte _id_tipo_incidencia;
        /// <summary>
        /// Almacena el tipo de incidencia que pueda presentar una llanta (Chipote, Ponchadura, etc.)
        /// </summary>
        public byte id_tipo_incidencia
        {
            get { return _id_tipo_incidencia; }
        }
        private int _id_operador;
        /// <summary>
        /// Operador al cual se le presento la incidencia
        /// </summary>
        public int id_operador
        {
            get { return _id_operador; }
        }
        private string _responsable;
        /// <summary>
        /// Persona responsable de la incidencia
        /// </summary>
        public string responsable
        {
            get { return _responsable; }
        }
        private string _observacion;
        /// <summary>
        /// Datos descriptivos sobre la incidencia
        /// </summary>
        public string observacion
        {
            get { return _observacion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Habilita o deshabilita el uso de un registro (Habilitado-Disponible / Deshabilitado-NoDisponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa en ceros los atributos de la clase
        /// </summary>
        public LecturaIncidencia()
        {
            this._id_lectura_incidencia = 0;
            this._id_lectura = 0;
            this._id_tipo_incidencia = 0;
            this._id_operador = 0;
            this._responsable = "";
            this._observacion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro
        /// </summary>
        /// <param name="id_lectura_incidencia">Identificador del registro de lectura incidencia</param>
        public LecturaIncidencia(int id_lectura_incidencia)
        {
            //Invoca al método que realiza la consulta y asignación de valores a los atributos de la clase
            cargaAtributos(id_lectura_incidencia);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~LecturaIncidencia()
        {
            Dispose(false);
        }
        #endregion

        #region Método Privados
        /// <summary>
        /// Método que consulta e inicializa los atributos de la clase a partir de un registro de Lectura Incidencia
        /// </summary>
        /// <param name="id_lectura_incidencia">Identificador del regsitro de Lectura Incidencia</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_lectura_incidencia)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de una lectura incidencia
            object[] param = { 3, id_lectura_incidencia, 0, 0, 0, "", "", 0, false, "", "" };
            //Invoa y asigna al dataset el método que realiza la busqueda de una Lectura Incidencia
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que no sean nulos)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna los campos del registro encontrado del dataset a los atributos de la clase
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_lectura_incidencia = id_lectura_incidencia;
                        this._id_lectura = Convert.ToInt32(r["IdLectura"]);
                        this._id_tipo_incidencia = Convert.ToByte(r["IdTipoIncidencia"]);
                        this._id_operador = Convert.ToInt32(r["IdOperador"]);
                        this._responsable = Convert.ToString(r["Responsable"]);
                        this._observacion = Convert.ToString(r["Observacion"]);
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
        /// Método que actualiza una lectura incidencia 
        /// </summary>
        /// <param name="id_lectura">Actualiza el identificador de una lectura de llanta</param>
        /// <param name="id_tipo_incidencia">Actualiza el tipo de incidencia (Ponchadura, Chipote,etc.)</param>
        /// <param name="id_operador">Actualiza el identificador del operador a cargo de la unidad</param>
        /// <param name="responsable">Actualiza nombre del responsable </param>
        /// <param name="observacion">Actualiza las observaciones realizadas sobre la incidencia</param>
        /// <param name="id_usuario">Actualiza al usuario que realizó la ultima actualización sobrel el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del regsitro (Habilitado-Disponible / Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarLecturaIncidencia(int id_lectura, byte id_tipo_incidencia, int id_operador, string responsable, string observacion, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar un  registro de lectura incidencia
            object[] param = { 2, this._id_lectura_incidencia, id_lectura, id_tipo_incidencia, id_operador, responsable, observacion, id_usuario, habilitar, "", "" };
            //Invcoa y asigna al objeto retorno el método que realiza la actualización de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el valor del objeto retorno
            return retorno;
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta un registro de lectura incidencia 
        /// </summary>
        /// <param name="id_lectura">Inserta el identificador de una lectura de llanta</param>
        /// <param name="id_tipo_incidencia">Inserta el tipo de incidencia (Ponchadura, Chipote,etc.)</param>
        /// <param name="id_operador">Inserta el identificador del operador a cargo de la unidad</param>
        /// <param name="responsable">Inserta el  nombre del responsable </param>
        /// <param name="observacion">Inserta las observaciones realizadas sobre la incidencia</param>
        /// <param name="id_usuario">Inserta el al usuario que realizo la ultima actualización sobrel el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarLecturaIncidencia(int id_lectura, byte id_tipo_incidencia, int id_operador, string responsable, string observacion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para insertar los campos de un  registro de lectura incidencia
            object[] param = { 1, 0, id_lectura, id_tipo_incidencia, id_operador, responsable, observacion, id_usuario, true, "", "" };
            //Invcoa y asigna al objeto retorno el método que realiza la inserción de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el valor del objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de lectura incidencia 
        /// </summary>
        /// <param name="id_lectura">Actualiza el identificador de una lectura de llanta</param>
        /// <param name="id_tipo_incidencia">Actualiza el tipo de incidencia (Ponchadura, Chipote,etc.)</param>
        /// <param name="id_operador">Actualiza el identificador del operador a cargo de la unidad</param>
        /// <param name="responsable">Actualiza nombre del responsable </param>
        /// <param name="observacion">Actualiza las observaciones realizadas sobre la incidencia</param>
        /// <param name="id_usuario">Actualiza al usuario que realizo la ultima actualización sobrel el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarLecturaIncidencia(int id_lectura, byte id_tipo_incidencia, int id_operador, string responsable, string observacion, int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro
            return this.editarLecturaIncidencia(id_lectura, id_tipo_incidencia, id_operador, responsable, observacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita el uso de un registro de Habilitado-Disponible a Deshabilitado-NoDisponible
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario responsable de esta acción</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarLecturaIncidencia(int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro
            return this.editarLecturaIncidencia(this._id_lectura, this._id_tipo_incidencia, this._id_operador, this._responsable, this._observacion, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaLecturaIncedencia()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_lectura_incidencia);
        }
        #endregion
    }
}
