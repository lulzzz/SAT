using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que permite Editar, Consultar e Insertar registros en la tabla Configuración
    /// </summary>
    public class Configuracion:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla configuración
        /// </summary>
        private static string nom_sp = "llantas.sp_configuracion_tc";

        private int _id_configuracion;
        /// <summary>
        /// Almacena el identificador del registro de configuración
        /// </summary>
        public int id_configuracion
        {
            get { return _id_configuracion; }
        }
        private string _descripcion;
        /// <summary>
        /// Nombre o caracteristicas que permite identificar la configuración
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private int _no_ejes;
        /// <summary>
        /// Número total de ejes de una unidad
        /// </summary>
        public int no_ejes
        {
            get { return _no_ejes; }
        }
        private int _no_posiciones;
        /// <summary>
        /// Número total de posiciones de una unidad
        /// </summary>
        public int no_posiciones
        {
            get { return _no_posiciones; }
        }
        private bool _habilitar;
        /// <summary>
        /// Estado de uso del registro (Habilitar-Disponible / Deshabilitar-NoDisponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// 
        /// Constructor que inicializa los atributos de la clase en cero
        /// </summary>
        public Configuracion()
        {
            this._id_configuracion = 0;
            this._descripcion = "";
            this._no_ejes = 0;
            this._no_posiciones = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicaliza los atributos a partir de un registro de configuración
        /// </summary>
        /// <param name="id_configuracion">Identificador del registro</param>
        public Configuracion(int id_configuracion)
        {
            //Invoca al método que realiza la asignación y busqueda del registro
            cargaAtributos(id_configuracion);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~Configuracion()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que consulta y asigna el resultado a los atributos de la clase
        /// </summary>
        /// <param name="id_configuracion">Id que sirve como referencia para la busuqeda de un registro Configuración</param>
        /// <returns></returns>
        public bool cargaAtributos(int id_configuracion)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda del registro de configuración
            object[] param = { 3, id_configuracion, "", 0, 0, 0, false, "", "" };
            //Instancia a la clase que realiza la busqueda del registro de Configuración
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del datase(que no sean  nulos)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos de la clase los valores del dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_configuracion = id_configuracion;
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._no_ejes = Convert.ToInt32(r["NoEjes"]);
                        this._no_posiciones = Convert.ToInt32(r["NoPosiciones"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorno al método del objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de configuración
        /// </summary>
        /// <param name="descripcion">Actualiza la descripción de la configuración.</param>
        /// <param name="no_ejes">Actualiza la cantidad de ejes de la configuración</param>
        /// <param name="no_posiciones">Actualiza la cantidad de posiciones de una configuración</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realiza la actualización del registro de configuración</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitar-Disponible / Deshabilitar-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarConfiguracion(string descripcion, int no_ejes, int no_posiciones, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar el registro de configuración
            object[] param = { 2, this._id_configuracion, descripcion, no_ejes, no_posiciones, id_usuario, habilitar, "", "" };
            //Invoca y asigna al objeto retorno el método que realiza la actualización del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta un registro de configuración
        /// </summary>
        /// <param name="descripcion">Inserta la descripción de la configuración.</param>
        /// <param name="no_ejes">Inserta la cantidad de ejes de la configuración</param>
        /// <param name="no_posiciones">Inserta la cantidad de posiciones de una configuración</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realiza la actualización del registro de configuración</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarConfiguracion(string descripcion, int no_ejes, int no_posiciones, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para insertar un registro de configuración
            object[] param = { 1, 0, descripcion, no_ejes, no_posiciones, id_usuario, true, "", "" };
            //Invoca y asigna al objeto retorno el método que realiza la actualización de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de configuración
        /// </summary>
        /// <param name="descripcion">Actualiza la descripción de la configuración.</param>
        /// <param name="no_ejes">Actualiza la cantidad de ejes de la configuración</param>
        /// <param name="no_posiciones">Actualiza la cantidad de posiciones de una configuración</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realiza la actualización del registro de configuración</param>
        /// <returns></returns>
        public RetornoOperacion EditarConfiguracion(string descripcion, int no_ejes, int no_posiciones, int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro
            return this.editarConfiguracion(descripcion, no_ejes, no_posiciones, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro  de Habilitar-Disponible a Deshabilitar-NoDisponible
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que deshabilito el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarConfiguracion(int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro
            return this.editarConfiguracion(this._descripcion, this._no_ejes, this._no_posiciones, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaConfiguracion()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_configuracion);
        }
        #endregion
    }
}
