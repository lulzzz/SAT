using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que permite realizar Actualizaciones, Consulta e Inserción de registros de ejes
    /// </summary>
    public class Eje: Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store producere de la tabla eje
        /// </summary>
        private static string nom_sp = "llantas.sp_eje_te";

        private int _id_eje;
        /// <summary>
        /// Almacena el identificador del registro de un eje
        /// </summary>
        public int id_eje
        {
            get { return _id_eje; }
        }
        private int _id_configuracion;
        /// <summary>
        /// Identificador de una configuración
        /// </summary>
        public int id_configuracion
        {
            get { return _id_configuracion; }
        }
        private string _descripcion;
        /// <summary>
        /// Nombre o caracteristicas que permite identificar a un registro de eje
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private int _no_posiciones;
        /// <summary>
        /// Número de posiciones que puede tener un eje
        /// </summary>
        public int no_posiciones
        {
            get { return _no_posiciones; }
        }
        private bool _habilitar;
        /// <summary>
        /// Estado de uso del registro (Habilitado-Disponible / Deshabilitado-NoDisponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Cosntructor que inicializa los atributos a cero
        /// </summary>
        public Eje()
        {
            this._id_eje = 0;
            this._id_configuracion = 0;
            this._descripcion = "";
            this._no_posiciones = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de la consulta de un eje
        /// </summary>
        /// <param name="id_eje"></param>
        public Eje(int id_eje)
        {
            //Invoca al método que consulta y asigna a los atributos un registro de eje
            cargaAtributos(id_eje);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~Eje()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que consulta un registro de eje y lo alamcena en los atributos de la clase.
        /// </summary>
        /// <param name="id_eje">Identifica al registro de eje</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_eje)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un eje
            object[] param = { 3, id_eje, 0, "", 0, 0, false, "", "" };
            //Instancia a la clase que realiza la consulta de un regsitro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del datase
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos el registro encontrado
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_eje = id_eje;
                        this._id_configuracion = Convert.ToInt32(r["IdConfiguracion"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._no_posiciones = Convert.ToInt32(r["NoPosiciones"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de eje 
        /// </summary>
        /// <param name="id_configuracion">Actualiza el identificador de una configuración.</param>
        /// <param name="descripcion">Actualiza las caracteristicas que identifican a un registro de eje</param>
        /// <param name="no_posiciones">Actualiza la cantidad de posiciones de un eje</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realiza la actualización</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado-Disponible / Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarEje(int id_configuracion, string descripcion, int no_posiciones, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la actuaización del regsitro eje
            object[] param = { 2, this._id_eje, id_configuracion, descripcion, no_posiciones, id_usuario, habilitar, "", "" };
            //Invoca y asigna al objeto retorno el método que realiza la actualización del regsitro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta un registro de eje 
        /// </summary>
        /// <param name="id_configuracion">Inserta el identificador de una configuración.</param>
        /// <param name="descripcion">Inserta las caracteristicas que identifican a un registro de eje</param>
        /// <param name="no_posiciones">Inserta la cantidad de posiciones de un eje</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realiza la inserción</param>       
        /// <returns></returns>
        public static RetornoOperacion InsertarEje(int id_configuracion, string descripcion, int no_posiciones, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la inserción del regsitro eje
            object[] param = { 1, 0, id_configuracion, descripcion, no_posiciones, id_usuario, true, "", "" };
            //Invoca y asigna al objeto retorno el método que realiza la insercion del regsitro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de eje 
        /// </summary>
        /// <param name="id_configuracion">Actualiza el identificador de una configuración.</param>
        /// <param name="descripcion">Actualiza las caracteristicas que identifican a un registro de eje</param>
        /// <param name="no_posiciones">Actualiza la cantidad de posiciones de un eje</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realiza la actualización</param>        
        /// <returns></returns>
        public RetornoOperacion EditarEje(int id_configuracion, string descripcion, int no_posiciones, int id_usuario)
        {
            //Retorna el método que actualiza el registro de eje
            return this.editarEje(id_configuracion, descripcion, no_posiciones, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita el uso de un regsitro
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo la acción de deshabilitar el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaEje(int id_usuario)
        {
            //Retorna el método que actualiza el registro de eje
            return this.editarEje(this._id_configuracion, this._descripcion, this._no_posiciones, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaEje()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_eje);
        }
        #endregion
    }
}
