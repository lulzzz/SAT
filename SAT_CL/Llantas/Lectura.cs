using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que permite realizar las acciones de Consulta, Actualización e Inserción de registros de tipo lectura llanta
    /// </summary>
    public class Lectura:Disposable
    {
        #region Enumeracion
        /// <summary>
        /// Enumera los estados de una llanta presentada en la lectura de una llanta
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Cuando en la lectura se marca el estado de la llanta en buena condición
            /// </summary>
            BuenaCondicion = 1,
            /// <summary>
            /// Cuando en la lectura se marca el estado de la llanta como dañada
            /// </summary>
            Dañada
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla lectura
        /// </summary>
        private static string nom_sp = "llantas.sp_lectura_tl";

        private int _id_lectura;
        /// <summary>
        /// Almacena el identificador de un registro de lectura de llanta
        /// </summary>
        public int id_lectura
        {
            get { return _id_lectura; }
        }
        private int _id_llanta;
        /// <summary>
        /// Identifica a una llanta
        /// </summary>
        public int id_llanta
        {
            get { return _id_llanta; }
        }
        private int _id_posicion;
        /// <summary>
        /// Identifica la posicion de la llanta a al cual se le esta aplicando una lectura
        /// </summary>
        public int id_posicion
        {
            get { return _id_posicion; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Estatus de una llanta (Buena Condicion o Dañada)
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        /// <summary>
        /// Permite tener acceso a la enumeración de los estatus de una llanta
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this._id_estatus; }
        }
        private int _profundidad;
        /// <summary>
        /// Permite medir el desgaste que tiene una llanta por lectura
        /// </summary>
        public int profundidad
        {
            get { return _profundidad; }
        }
        private decimal _presion;
        /// <summary>
        /// Permite medir los niveles de presion de aire de una llanta
        /// </summary>
        public decimal presion
        {
            get { return _presion; }
        }
        private decimal _km_odometro;
        /// <summary>
        /// Kilometros recorridos de una llanta 
        /// </summary>
        public decimal km_odometro
        {
            get { return _km_odometro; }
        }
        private string _observaciones;
        /// <summary>
        /// Datos sobre la lectura de la llanta
        /// </summary>
        public string observaciones
        {
            get { return _observaciones; }
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
        /// Constructor que inicializa los atributos de la clase en cero
        /// </summary>
        public Lectura()
        {
            this._id_lectura = 0;
            this._id_llanta = 0;
            this._id_posicion = 0;
            this._id_estatus = 0;
            this._profundidad = 0;
            this._presion = 0;
            this._km_odometro = 0;
            this._observaciones = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de la consulta de un registro de lectura
        /// </summary>
        /// <param name="id_lectura">Identificador que sirve como referencia para la consulta de un registro de lectura</param>
        public Lectura(int id_lectura)
        {
            //Invoca al método que realiza la busqueda y asignación de valores a los atributos de la clase
            cargaAtributos(id_lectura);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~Lectura()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la consulta de una lectura y asigna el resultado a los atributos de la clase
        /// </summary>
        /// <param name="id_lectura">Identificador de una lectura</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_lectura)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta de un registro de lectura
            object[] param = { 3, id_lectura, 0, 0, 0, 0, 0, 0, "", 0, false, "", "" };
            //Invoca y asigna al dataset el método que realiza la busqueda del registro de Lectura
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que no sean nulos)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna  a los atributos el regsitro del dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_lectura = id_lectura;
                        this._id_llanta = Convert.ToInt32(r["IdLlanta"]);
                        this._id_posicion = Convert.ToInt32(r["IdPosicion"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._profundidad = Convert.ToInt32(r["Profundidad"]);
                        this._presion = Convert.ToDecimal(r["Presion"]);
                        this._km_odometro = Convert.ToDecimal(r["KMOdometro"]);
                        this._observaciones = Convert.ToString(r["Observaciones"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno.
            return retorno;
        }
        /// <summary>
        /// Método que actualiza a una lectura de llanta
        /// </summary>
        /// <param name="id_llanta">Actualiza la llanta</param>
        /// <param name="id_posicion">Actualiza la posicion de una llanta</param>
        /// <param name="estatus">Actualiza el estatus de una llanta (Dañada o en Buena condicion)</param>
        /// <param name="profundidad">Actualiza la medida de desgaste de una llanta</param>
        /// <param name="presion">Actualiza los niveles de aire de una llanta</param>
        /// <param name="km_odometro">Actualiza el kilometraje recorrido de una llanta</param>
        /// <param name="observaciones">Actualiza los comentarios acerca de la lectura de la llanta </param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo las actualizaciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso de un registro (Habilitado - Disponible, Deshabilitado - No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarLectura(int id_llanta, int id_posicion, Estatus estatus, int profundidad, decimal presion, decimal km_odometro, string observaciones, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la actulización de un registro
            object[] param = { 2, this._id_lectura, id_llanta, id_posicion, (Estatus)estatus, profundidad, presion, km_odometro, observaciones, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método que realiza la actualización del registro lectura
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que Inserta un registro de lectura
        /// </summary>
        /// <param name="id_llanta">Inserta una llanta</param>
        /// <param name="id_posicion">Inserta la posicion de una llanta</param>
        /// <param name="estatus">Inserta el estatus de una llanta (Dañada o en Buena condicion)</param>
        /// <param name="profundidad">Inserta la medida de desgaste de una llanta</param>
        /// <param name="presion">Inserta los niveles de aire de una llanta</param>
        /// <param name="km_odometro">Inserta el kilometraje recorrido de una llanta</param>
        /// <param name="observaciones">Inserta los comentarios acerca de la lectura de la llanta </param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo las actualizaciones sobre el registro</param>        
        /// <returns></returns>
        public static RetornoOperacion InsertarLectura(int id_llanta, int id_posicion, Estatus estatus, int profundidad, decimal presion, decimal km_odometro, string observaciones, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la inserción de un registro
            object[] param = { 1, 0, id_llanta, id_posicion, (Estatus)estatus, profundidad, presion, km_odometro, observaciones, id_usuario, true, "", "" };
            //Asigana la objeto retorno resultado del método que realiza la insercion del registro lectura
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza a un registro de lectura
        /// </summary>
        /// <param name="id_llanta">Actualiza la llanta</param>
        /// <param name="id_posicion">Actualiza la posicion de una llanta</param>
        /// <param name="estatus">Actualiza el estatus de una llanta (Dañada o en Buena condicion)</param>
        /// <param name="profundidad">Actualiza la medida de desgaste de una llanta</param>
        /// <param name="presion">Actualiza los niveles de aire de una llanta</param>
        /// <param name="km_odometro">Actualiza el kilometraje recorrido de una llanta</param>
        /// <param name="observaciones">Actualiza los comentarios acerca de la lectura de la llanta </param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo las actualizaciones sobre el registro</param>        
        /// <returns></returns>
        public RetornoOperacion EditarLectura(int id_llanta, int id_posicion, Estatus estatus, int profundidad, decimal presion, decimal km_odometro, string observaciones, int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro de lectura
            return this.editarLectura(id_llanta, id_posicion, (Estatus)estatus, profundidad, presion, km_odometro, observaciones, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro de Habilitado - Disponible a Deshabilitado - No Disponible)
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo esta acción</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarLectura(int id_usuario)
        {
            //Retorna el método que realiza la actualización de un registro de lectura
            return this.editarLectura(this._id_llanta, this._id_posicion, (Estatus)this._id_estatus, this._profundidad, this._presion, this._km_odometro, this._observaciones, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaLectura()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_lectura);
        }
        #endregion
    }
}
