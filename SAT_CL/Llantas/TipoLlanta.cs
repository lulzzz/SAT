using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que Consulta, Actualiza e Inserta registros de Tipo Llanta
    /// </summary>
    public class TipoLlanta:Disposable
    {
        #region Atributos
        /// <summary>
        /// Almacena el nombre del store procedure de la tabla tipo llanta
        /// </summary>
        private static string nom_sp = "llantas.sp_tipo_llanta_ttll";
        private int _id_tipo_llanta;
        /// <summary>
        /// Identifica el registro de tipo llanta
        /// </summary>
        public int id_tipo_llanta
        {
            get { return _id_tipo_llanta; }
        }
        private int _id_marca;
        /// <summary>
        /// Identifica a una marca de llanta
        /// </summary>
        public int id_marca
        {
            get { return _id_marca; }
        }
        private string _modelo;
        /// <summary>
        /// Número que describe la forma de una llanta
        /// </summary>
        public string modelo
        {
            get { return _modelo; }
        }
        private byte _id_tipo;
        /// <summary>
        /// Tipo de Llanta (P-Pasajeros,LT-Camionetas Ligeras, T-Refaccion Temporal,ST- Remolque especial)
        /// </summary>
        public byte id_tipo
        {
            get { return _id_tipo; }
        }
        private int _ancho_seccio;
        /// <summary>
        /// Medida de un costado a otro de la llanta
        /// </summary>
        public int ancho_seccio
        {
            get { return _ancho_seccio; }
        }
        private int _relacion_aspecto;
        /// <summary>
        /// Altura del costado con el ancho de la llanta.
        /// </summary>
        public int relacion_aspecto
        {
            get { return _relacion_aspecto; }
        }
        private byte _id_tipo_construccion;
        /// <summary>
        /// Dirección de las cuerdas de las capas de una llanta (R-Radios, D-Diagonal, B-Construcción en Cinturon)
        /// </summary>
        public byte id_tipo_construccion
        {
            get { return _id_tipo_construccion; }
        }
        private int _diametro_exterior;
        /// <summary>
        /// Diametro de medida del rin
        /// </summary>
        public int diametro_exterior
        {
            get { return _diametro_exterior; }
        }
        private string _indice_carga;
        /// <summary>
        /// Capacidad de carga de la llanta
        /// </summary>
        public string indice_carga
        {
            get { return _indice_carga; }
        }
        private string _rango_velocidad;
        /// <summary>
        /// Rango de velocidad acorde al peso de carga
        /// </summary>
        public string rango_velocidad
        {
            get { return _rango_velocidad; }
        }
        private string _aplicacion;
        /// <summary>
        /// Uso de la llanta (Cumple con los estandares de llanta para lodo y nieve de la RubberManufacturersAssociation ejemplo)
        /// </summary>
        public string aplicacion
        {
            get { return _aplicacion; }
        }
        private int _medida;
        /// <summary>
        /// Calcula la medida de una llanta 
        /// </summary>
        public int medida
        {
            get { return _medida; }
        }
        private string _utqg;
        /// <summary>
        /// Información sobre la llanta (Desgaste de Piso,Tracción y Temperatura)
        /// </summary>
        public string utqg
        {
            get { return _utqg; }
        }
        private int _maxima_presion;
        /// <summary>
        /// Nivel de aire maxima de una llanta
        /// </summary>
        public int maxima_presion
        {
            get { return _maxima_presion; }
        }
        private int _minima_presion;
        /// <summary>
        /// Nivel de aire minima de una llanta
        /// </summary>
        public int minima_presion
        {
            get { return _minima_presion; }
        }
        private int _no_capas;
        /// <summary>
        /// Número de capas de una llanta
        /// </summary>
        public int no_capas
        {
            get { return _no_capas; }
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
        public TipoLlanta()
        {
            this._id_tipo_llanta = 0;
            this._id_marca = 0;
            this._modelo = "";
            this._id_tipo = 0;
            this._ancho_seccio = 0;
            this._relacion_aspecto = 0;
            this._id_tipo_construccion = 0;
            this._diametro_exterior = 0;
            this._indice_carga = "";
            this._rango_velocidad = "";
            this._aplicacion = "";
            this._medida = 0;
            this._utqg = "";
            this._maxima_presion = 0;
            this._minima_presion = 0;
            this._no_capas = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro
        /// </summary>
        /// <param name="id_tipo_llanta">Identifica el regsitro a asignar a los atributos</param>
        public TipoLlanta(int id_tipo_llanta)
        {
            //Invoca al método que realiza la busqueda y asignación de un registro a los atributos
            cargaAtributos(id_tipo_llanta);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~TipoLlanta()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro de tipo llanta y asigna el resultado a los atributos de la clase
        /// </summary>
        /// <param name="id_tipo_llanta">Identificador de un registro de tipo llanta</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_tipo_llanta)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda del regsitro de tipo llanta
            object[] param = { 3, id_tipo_llanta, 0, "", 0, 0, 0, 0, 0, "", "", "", 0, "", 0, 0, 0, 0, false, "", "" };
            //Invoca al método que realiza la busqueda del registro y los almacena en el dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset(que no sean nulos)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos los valores de dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_tipo_llanta = id_tipo_llanta;
                        this._id_marca = Convert.ToInt32(r["IdMarca"]);
                        this._modelo = Convert.ToString(r["Modelo"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._ancho_seccio = Convert.ToInt32(r["AnchoSeccion"]);
                        this._relacion_aspecto = Convert.ToInt32(r["RelacionAspecto"]);
                        this._id_tipo_construccion = Convert.ToByte(r["IdTipoConstruccion"]);
                        this._diametro_exterior = Convert.ToInt32(r["DiametroExterior"]);
                        this._indice_carga = Convert.ToString(r["IndiceCarga"]);
                        this._rango_velocidad = Convert.ToString(r["Rango"]);
                        this._aplicacion = Convert.ToString(r["Aplicacion"]);
                        this._medida = Convert.ToInt32(r["Medida"]);
                        this._utqg = Convert.ToString(r["UTQG"]);
                        this._maxima_presion = Convert.ToInt32(r["MaximaPresion"]);
                        this._minima_presion = Convert.ToInt32(r["MinimaPresion"]);
                        this._no_capas = Convert.ToInt32(r["NoCapas"]);
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
        /// Método que actualiza un registro de tipo llanta
        /// </summary>
        /// <param name="id_marca">Actualiza la marca de la llanta</param>
        /// <param name="modelo">Actualiza el modelo de la llanta</param>
        /// <param name="id_tipo">Actualiza el tipo de empleo de la llanta (P-Pasajeros,LT-Camionetas Ligeras, T-Refaccion Temporal,ST- Remolque especial)</param>
        /// <param name="ancho_seccio">Actualiza la anchura del costado con el ancho de la llanta</param>
        /// <param name="relacion_aspecto">Actualiza el altura del costado con el ancho de la llanta</param>
        /// <param name="id_tipo_construccion">Actualiza  Dirección de las cuerdas de las capas de una llanta (R-Radios, D-Diagonal, B-Construcción en Cinturon)</param>
        /// <param name="diametro_exterior">Actualiza el diametro de rin de una llanta</param>
        /// <param name="indice_carga">Actualiza los indices de carga de una llanta</param>
        /// <param name="rango_velocidad">Actualiza el rango de velocidad de una llanta</param>
        /// <param name="aplicacion">Actualiza Uso de la llanta (Cumple con los estandares de llanta para lodo y nieve de la RubberManufacturersAssociation ejemplo)</param>
        /// <param name="utqg">Actualiza la información sobre la llanta (Desgaste de Piso,Tracción y Temperatura)</param>
        /// <param name="maxima_presion">Actualiza la máxima presión de aire de una llanta</param>
        /// <param name="minima_presion">Actualiza la mínima presión de aire de una llanta</param>
        /// <param name="no_capas">Actualiza el número de capas que esta hecha una llanta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del regsitro</param>
        /// <param name="habilitar">Actualiza el estado de uso de la llanta (Habilitado - Disponible, Deshabilitado - No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarTipoLlanta(int id_marca, string modelo, byte id_tipo, int ancho_seccio, int relacion_aspecto, byte id_tipo_construccion, int diametro_exterior,
                                                    string indice_carga, string rango_velocidad, string aplicacion, string utqg, int maxima_presion, int minima_presion,
                                                    int no_capas, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar el registro
            object[] param = { 2, this._id_tipo_llanta, id_marca, modelo, id_tipo, ancho_seccio, relacion_aspecto, id_tipo_construccion, diametro_exterior, indice_carga, 
                                 rango_velocidad, aplicacion, 0, utqg, maxima_presion, minima_presion, no_capas, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método que realiza la actualización del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta un registro de tipo llanta
        /// </summary>
        /// <param name="id_marca">Inserta la marca de la llanta</param>
        /// <param name="modelo">Inserta el modelo de la llanta</param>
        /// <param name="id_tipo">Inserta el tipo de empleo de la llanta (P-Pasajeros,LT-Camionetas Ligeras, T-Refaccion Temporal,ST- Remolque especial)</param>
        /// <param name="ancho_seccio">Inserta la anchura del costado con el ancho de la llanta</param>
        /// <param name="relacion_aspecto">Inserta el altura del costado con el ancho de la llanta</param>
        /// <param name="id_tipo_construccion">Inserta  Dirección de las cuerdas de las capas de una llanta (R-Radios, D-Diagonal, B-Construcción en Cinturon)</param>
        /// <param name="diametro_exterior">Inserta el diametro de rin de una llanta</param>
        /// <param name="indice_carga">Inserta los indices de carga de una llanta</param>
        /// <param name="rango_velocidad">Inserta el rango de velocidad de una llanta</param>
        /// <param name="aplicacion">Inserta Uso de la llanta (Cumple con los estandares de llanta para lodo y nieve de la RubberManufacturersAssociation ejemplo)</param>
        /// <param name="utqg">Inserta la información sobre la llanta (Desgaste de Piso,Tracción y Temperatura)</param>
        /// <param name="maxima_presion">Inserta la máxima presión de aire de una llanta</param>
        /// <param name="minima_presion">Inserta la mínima presión de aire de una llanta</param>
        /// <param name="no_capas">Inserta el número de capas que esta hecha una llanta</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la actualización del regsitro</param>
        /// <param name="habilitar">Inserta el estado de uso de la llanta (Habilitado - Disponible, Deshabilitado - No Disponible)</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarTipoLlanta(int id_marca, string modelo, byte id_tipo, int ancho_seccio, int relacion_aspecto, byte id_tipo_construccion, int diametro_exterior,
                                                    string indice_carga, string rango_velocidad, string aplicacion, string utqg, int maxima_presion, int minima_presion,
                                                    int no_capas, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para insertar el registro
            object[] param = { 1, 0, id_marca, modelo, id_tipo, ancho_seccio, relacion_aspecto, id_tipo_construccion, diametro_exterior, indice_carga, 
                                 rango_velocidad, aplicacion, 0, utqg, maxima_presion, minima_presion, no_capas, id_usuario, true, "", "" };
            //Asigna al objeto retorno el resultado del método que realiza la inserción del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza un registro de tipo llanta
        /// </summary>
        /// <param name="id_marca">Actualiza la marca de la llanta</param>
        /// <param name="modelo">Actualiza el modelo de la llanta</param>
        /// <param name="id_tipo">Actualiza el tipo de empleo de la llanta (P-Pasajeros,LT-Camionetas Ligeras, T-Refaccion Temporal,ST- Remolque especial)</param>
        /// <param name="ancho_seccio">Actualiza la anchura del costado con el ancho de la llanta</param>
        /// <param name="relacion_aspecto">Actualiza el altura del costado con el ancho de la llanta</param>
        /// <param name="id_tipo_construccion">Actualiza  Dirección de las cuerdas de las capas de una llanta (R-Radios, D-Diagonal, B-Construcción en Cinturon)</param>
        /// <param name="diametro_exterior">Actualiza el diametro de rin de una llanta</param>
        /// <param name="indice_carga">Actualiza los indices de carga de una llanta</param>
        /// <param name="rango_velocidad">Actualiza el rango de velocidad de una llanta</param>
        /// <param name="aplicacion">Actualiza Uso de la llanta (Cumple con los estandares de llanta para lodo y nieve de la RubberManufacturersAssociation ejemplo)</param>
        /// <param name="utqg">Actualiza la información sobre la llanta (Desgaste de Piso,Tracción y Temperatura)</param>
        /// <param name="maxima_presion">Actualiza la maxima presión de aire de una llanta</param>
        /// <param name="minima_presion">Actualiza la minima presión de aire de una llanta</param>
        /// <param name="no_capas">Actualiza el numero de capas que esta hecha una llanta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del regsitro</param>
        /// <returns></returns>
        public RetornoOperacion EditarTipoLllanta(int id_marca, string modelo, byte id_tipo, int ancho_seccio, int relacion_aspecto, byte id_tipo_construccion, int diametro_exterior,
                                                    string indice_carga, string rango_velocidad, string aplicacion, string utqg, int maxima_presion, int minima_presion,
                                                    int no_capas, int id_usuario)
        {
            //Retorna el método que realiza la actualización
            return this.editarTipoLlanta(id_marca, modelo, id_tipo, ancho_seccio, relacion_aspecto, id_tipo_construccion, diametro_exterior, indice_carga, rango_velocidad, aplicacion, utqg, maxima_presion, minima_presion, no_capas, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita el registro de tipo Llanta
        /// </summary>
        /// <param name="id_usuario">Identificador del usuario que deshabilito el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarTipoLlanta(int id_usuario)
        {
            //Retorna el método que realiza la actualización
            return this.editarTipoLlanta(this._id_marca, this._modelo, id_tipo, this._ancho_seccio, this._relacion_aspecto, this._id_tipo_construccion, this._diametro_exterior, this._indice_carga, this._rango_velocidad, this._aplicacion, this._utqg, this._maxima_presion, this._minima_presion, this._no_capas, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTipoLlanta()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_tipo_llanta);
        }
        #endregion
    }
}
