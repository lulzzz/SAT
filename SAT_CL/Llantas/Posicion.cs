using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    public class Posicion : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla posicion
        /// </summary>
        private static string nom_sp = "llantas.sp_posicion_tp";

        private int _id_posicion;
        /// <summary>
        /// Identifica un registro de posicion
        /// </summary>
        public int id_posicion
        {
            get { return _id_posicion; }
        }
        private int _id_eje;
        /// <summary>
        /// Identifica a un eje
        /// </summary>
        public int id_eje
        {
            get { return _id_eje; }
        }
        private string _descripcion;
        /// <summary>
        /// Nombre o caracteristicas que identifica a una posición
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private decimal _coordenada_x;
        /// <summary>
        /// Almacena la ubicación en el eje x
        /// </summary>
        public decimal coordenada_x
        {
            get { return _coordenada_x; }
        }
        private decimal _coordenada_y;
        /// <summary>
        /// Almacena la ubicación en el eje y
        /// </summary>
        public decimal coordenada_y
        {
            get { return _coordenada_y; }
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
        /// Cosntructor que inicializa los atributos en cero
        /// </summary>
        public Posicion()
        {
            this._id_posicion = 0;
            this._id_eje = 0;
            this._descripcion = "";
            this._coordenada_x = 0;
            this._coordenada_y = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicaliza los atributos a partir de un registro de posicion
        /// </summary>
        /// <param name="id_posicion">Identifica al registro de posicion</param>
        public Posicion(int id_posicion)
        {
            //Invoca al método que busca y asigna valores a los atributos 
            cargaAtributos(id_posicion);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Posicion()
        {
            Dispose(false);
        }
        #endregion

        #region Método Privado
        /// <summary>
        /// Método que busca y asigna a los atributos un registro de posición
        /// </summary>
        /// <param name="id_posicion">Identifica el registro de posición</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_posicion)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro de posicion
            object[] param = { 3, id_posicion, 0, "", 0, 0, 0, false, "", "" };
            //Invoca al método que realiza la busuqeda de un registro de posición
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas del dataset y el resultado lo alamcena en los atributos de la clase
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_posicion = id_posicion;
                        this._id_eje = Convert.ToInt32(r["IdEje"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._coordenada_x = Convert.ToDecimal(r["CoordenadaX"]);
                        this._coordenada_y = Convert.ToDecimal(r["CoordenadaY"]);
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
        /// Método que actualiza un registro de posicion
        /// </summary>
        /// <param name="id_eje">Actualiza el identificador de un eje</param>
        /// <param name="descripcion">Actualiza el nombre que identifica a una posición</param>
        /// <param name="coordenada_x">Actuliza la coordenada x de ubiciación de la posición</param>
        /// <param name="coordenada_y">Actualiza la coordena y de ubicación de la posición</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualizaciójn del registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro de posición</param>
        /// <returns></returns>
        private RetornoOperacion editarPosicion(int id_eje, string descripcion, decimal coordenada_x, decimal coordenada_y, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la actualizacion de un registro de posicion
            object[] param = { 2, this._id_posicion, id_eje, descripcion, coordenada_x, coordenada_y, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método que realiza la actualización de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Método Publico
        /// <summary>
        /// Método que inserta un registro de posicion
        /// </summary>
        /// <param name="id_eje">Inserta el identificador de un eje</param>
        /// <param name="descripcion">Inserta el nombre que identifica a una posición</param>
        /// <param name="coordenada_x">Inserta la coordenada x de ubiciación de la posición</param>
        /// <param name="coordenada_y">Inserta la coordena y de ubicación de la posición</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la actualizaciójn del registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarPosicion(int id_eje, string descripcion, decimal coordenada_x, decimal coordenada_y, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la inserción de un registro de posicion
            object[] param = { 1, 0, id_eje, descripcion, coordenada_x, coordenada_y, id_usuario, true, "", "" };
            //Asigna al objeto retorno el resultado del método que realiza la inserción de un registro posición
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de posicion
        /// </summary>
        /// <param name="id_eje">Actualiza el identificador de un eje</param>
        /// <param name="descripcion">Actualiza el nombre que identifica a una posición</param>
        /// <param name="coordenada_x">Actuliza la coordenada x de ubiciación de la posición</param>
        /// <param name="coordenada_y">Actualiza la coordena y de ubicación de la posición</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualizaciójn del registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarPosición(int id_eje, string descripcion, decimal coordenada_x, decimal coordenada_y, int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro
            return this.editarPosicion(id_eje, descripcion, coordenada_x, coordenada_y, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro (Habilitado-Disponible / Deshabilitado-NoDisponible)
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realiza acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion Deshabilitar(int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro
            return this.editarPosicion(this._id_eje, this._descripcion, this._coordenada_x, this._coordenada_y, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPosicion()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_posicion);
        }
        #endregion
    }
}
