using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que Actualiza, Consulta e Inserta registros de Llanta Actividad
    /// </summary>
    public class LlantaActividad: Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que alamcena el nombre del store procedure de la tabla llanta actividad
        /// </summary>
        private static string nom_sp = "llantas.sp_llanta_actividad_tlla";

        private int _id_llanta_actividad;
        /// <summary>
        /// Almacena el identificador de la llanta actividadd
        /// </summary>
        public int id_llanta_actividad
        {
            get { return _id_llanta_actividad; }
        }
        private int _id_llanta;
        /// <summary>
        /// Almacena el identificador de una llanta
        /// </summary>
        public int id_llanta
        {
            get { return _id_llanta; }
        }
        private int _id_actividad;
        /// <summary>
        /// Almacena el identificador de una actividad
        /// </summary>
        public int id_actividad
        {
            get { return _id_actividad; }
        }
        private DateTime _fecha;
        /// <summary>
        /// Almacena la fecha de la actividad de llanta
        /// </summary>
        public DateTime fecha
        {
            get { return _fecha; }
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
        /// Constructor que inicializa los atributos en cero
        /// </summary>
        public LlantaActividad()
        {
            this._id_llanta = 0;
            this._id_actividad = 0;
            this._fecha = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos de la clase a partir de la consulta de un registro
        /// </summary>
        /// <param name="id_llanta_actividad">Identifica al registro de llanta actividad</param>
        public LlantaActividad(int id_llanta_actividad)
        {
            //Invoca al método que busca y asigna a los atributos un registro de llanta Actividad
            cargaAtributos(id_llanta_actividad);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~LlantaActividad()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que busca y asigna a los atributos los valores de un registro Llanta Actividad
        /// </summary>
        /// <param name="id_llanta_actividad"></param>
        /// <returns></returns>
        private bool cargaAtributos(int id_llanta_actividad)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 3, id_llanta_actividad, 0, 0, null, 0, false, "", "" };
            //Instancia al método que realiza la busqueda de un registro de LlantaActividad
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos los campos del dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_llanta_actividad = id_llanta_actividad;
                        this._id_llanta = Convert.ToInt32(r["IdLlanta"]);
                        this._id_actividad = Convert.ToInt32(r["IdActividad"]);
                        this._fecha = Convert.ToDateTime(r["Fecha"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Devuelve al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de llanta actividad
        /// </summary>
        /// <param name="id_llanta">Actualiza una llanta</param>
        /// <param name="id_actividad">Actualiza una actividad</param>
        /// <param name="fecha">Actualiza la fecha de la actividad referente a una llanta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro de llanta Actividad</param>
        /// <param name="habilitar">Actualiza el estado de uso del regsitro (Habilitado-Disponible / Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarLlantaActividad(int id_llanta, int id_actividad, DateTime fecha, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar un registro de Llanta Actividad
            object[] param = { 2, this._id_llanta_actividad, id_llanta, id_actividad, fecha, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método que realiza la actualización de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método que inserta un registro de llanta actividad
        /// </summary>
        /// <param name="id_llanta">Inserta una llanta</param>
        /// <param name="id_actividad">Inserta una actividad</param>
        /// <param name="fecha">Inserta la fecha de la actividad referente a una llanta</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la actualización del registro de llanta Actividad</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarLlantaActividad(int id_llanta, int id_actividad, DateTime fecha, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para insertar un registro de Llanta Actividad
            object[] param = { 1, 0, id_llanta, id_actividad, fecha, id_usuario, true, "", "" };
            //Asigna al objeto retorno el resultado del método que realiza la inserción de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de llanta actividad
        /// </summary>
        /// <param name="id_llanta">Actualiza una llanta</param>
        /// <param name="id_actividad">Actualiza una actividad</param>
        /// <param name="fecha">Actualiza la fecha de la actividad referente a una llanta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro de llanta Actividad</param>
        /// <returns></returns>
        public RetornoOperacion EditarLlantaActividad(int id_llanta, int id_actividad, DateTime fecha, int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro Llantaactividad
            return this.editarLlantaActividad(id_llanta, id_actividad, fecha, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que Deshabilita el uso de un registro de llanta actividad
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo la acción de deshabilitar el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarLlantaActividad(int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro Llantaactividad
            return this.editarLlantaActividad(this._id_llanta, this._id_actividad, this._fecha, id_usuario, false);
        }

        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaLlantaActividad()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_llanta_actividad);
        }
        #endregion
    }
}
