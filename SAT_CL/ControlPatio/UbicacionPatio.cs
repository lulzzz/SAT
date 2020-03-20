using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{   
    /// <summary>
    /// Clase encargada de las Operaciones correspondientes a las Ubicaciones de los Patios
    /// </summary>
    public class UbicacionPatio : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo  nom_sp con el nombre del sp_ de la tabla
        /// </summary>
        private static string nom_sp = "control_patio.sp_ubicacion_patio";

        private int _id_ubicacion_patio;
        /// <summary>
        /// ID que corresponde a la ubicacion patio del objeto
        /// </summary>
        public int id_ubicacion_patio
        {
            get { return _id_ubicacion_patio; }
        }

        private int _id_ubicacion;
        /// <summary>
        /// Id que corresponde a la ubicacion del objeto
        /// </summary>
        public int id_ubicacion
        {
            get { return _id_ubicacion; }
        }

        private string _nombre_corto;
        /// <summary>
        /// Nombre que corresponde al nombre corto del objeto
        /// </summary>
        public string nombre_corto
        {
            get { return _nombre_corto; }
        }

        private byte _id_tipo_patio;
        /// <summary>
        /// Id que corresponde al tipo patio del objeto
        /// </summary>
        public byte id_tipo_patio
        {
            get { return _id_tipo_patio; }
        }

        private int _id_compania_propietario;
        /// <summary>
        /// Id que corresponde a la compañia propietario del objeto
        /// </summary>
        public int id_compania_propietario
        {
            get { return _id_compania_propietario; }
        }

        private int _tiempo_limite;
        /// <summary>
        /// tiempo limite que corresponde a la ubicacion patio
        /// </summary>
        public int tiempo_limite
        {
            get { return _tiempo_limite; }
        }

        private bool _habilitar;
        /// <summary>
        /// Habilitar que corresponde al objeto
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que inicializa los atributos por default
        /// </summary>
        public UbicacionPatio()
        {
            //asignacion a mis atributos de valores de inicializacion 
            this._id_ubicacion_patio = 0;
            this._id_ubicacion = 0;
            this._nombre_corto = "";
            this._id_tipo_patio = 0;
            this._id_compania_propietario = 0;
            this._tiempo_limite = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que asignarn a los atributos datos existentes acuerdo al id
        /// </summary>
        /// <param name="id_ubicacion_patio"> Id con el cual se inicializara la busqueda  de los valores de mi objeto</param>
        public UbicacionPatio(int id_ubicacion_patio)
        {
            //invocación del método privado carga Atributo
            cargaAtributoInstancia(id_ubicacion_patio);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_ubicacion"></param>
        /// <param name="id_compania_emisora"></param>
        public UbicacionPatio(int id_ubicacion, int id_compania_emisora)
        {
            //invocación del método privado carga Atributo
            cargaAtributoInstancia(id_ubicacion, id_compania_emisora);
        }

        #endregion

        #region Destructor
        
        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~UbicacionPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        
        /// <summary>
        /// Método privado que permite cargar a los atributos registros dado un id de busqueda
        /// </summary>
        /// <param name="id_ubicacion_patio">Id que permite realizar la busqueda de registros </param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_ubicacion_patio)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            //Declaración y Asignación a un Arreglo con los parametro necesarios a utilizar  en el Store Procedure
            object[] param = { 3, id_ubicacion_patio, 0, "", 0, 0, 0, 0, false, "", "" };
            //Invocación del Store Procedure
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validar los datos de la tabla DataSet con los del Store Procedure
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido del las filas de la tabla del DataSet y las almacena en r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_ubicacion_patio = id_ubicacion_patio;
                        _id_ubicacion = Convert.ToInt32(r["IdUbicacionPatio"]);
                        _nombre_corto = Convert.ToString(r["NombreCorto"]);
                        _id_tipo_patio = Convert.ToByte(r["TipoPatio"]);
                        _id_compania_propietario = Convert.ToInt32(r["CompaniaPropietario"]);
                        _tiempo_limite = Convert.ToInt32(r["TiempoLimite"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                    }
                    //Cambio de valor al objeto solo si se cumple la validacion de los datos
                    retorno = true;
                }
            }
            //Retorno  del resultado al metodo
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_ubicacion"></param>
        /// <param name="id_compania_emisora"></param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_ubicacion, int id_compania_emisora)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            //Declaración y Asignación a un Arreglo con los parametro necesarios a utilizar  en el Store Procedure
            object[] param = { 4, 0, id_ubicacion, "", 0, id_compania_emisora, 0, 0, false, "", "" };
            //Invocación del Store Procedure
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validar los datos de la tabla DataSet con los del Store Procedure
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido del las filas de la tabla del DataSet y las almacena en r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_ubicacion_patio = Convert.ToInt32(r["Id"]);
                        _id_ubicacion = Convert.ToInt32(r["IdUbicacionPatio"]);
                        _nombre_corto = Convert.ToString(r["NombreCorto"]);
                        _id_tipo_patio = Convert.ToByte(r["TipoPatio"]);
                        _id_compania_propietario = Convert.ToInt32(r["CompaniaPropietario"]);
                        _tiempo_limite = Convert.ToInt32(r["TiempoLimite"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                    }
                    //Cambio de valor al objeto solo si se cumple la validacion de los datos
                    retorno = true;
                }
            }
            //Retorno  del resultado al metodo
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar registros a Ubicacion Patio
        /// </summary>
        /// <param name="id_ubicacion">Permite actualizar el id_ubicacion del registro</param>
        /// <param name="nombre_corto">Permite actualizr el nombre_corto del registro</param>
        /// <param name="id_tipo_patio">Permite actualizar el id_tipo_patio del registro</param>
        /// <param name="id_compania_propietario">Permite actualizar el id_compania_propietario del registro</param>
        /// <param name="tiempo_limite">permite actualizar el Tiempo Libre del registro</param>
        /// <param name="id_usuario">Permite actualizar el id_usuario del registro</param>
        /// <param name="habilitar">Permite actualizar habilitar del registro</param>
        /// <returns></returns>
        private RetornoOperacion editaUbicacionPatio(int id_ubicacion, string nombre_corto, int id_tipo_patio, int id_compania_propietario, int tiempo_limite, int id_usuario, bool habilitar)
        {
            //Creación del objeto  Retorno Operacion
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignacion de valores al Arreglo de tipo objeto
            object[] param = { 2, this.id_ubicacion_patio, id_ubicacion, nombre_corto, id_tipo_patio, id_compania_propietario, tiempo_limite, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Rertorna el resultado del método
            return retorno;
        }
        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método que inserta registros a ubicacion patio
        /// </summary>
        /// <param name="id_ubicacion"> Valor que se inserta en id_ubicacion</param>
        /// <param name="nombre_corto">Valor que se  inserta en nombre_corto</param>
        /// <param name="id_tipo_patio">valor que se inserta en id_tipo_patio</param>
        /// <param name="id_compania_propietario">Valor que se inserta en id_compania_propietario</param>
        /// <param name="tiempo_limite">permite actualizar el Tiempo Libre del registro</param>
        /// <param name="id_usuario">Valor que se inserta en id_usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarUbicacionPatio(int id_ubicacion, string nombre_corto, int id_tipo_patio, int id_compania_propietario, int tiempo_limite, int id_usuario)
        {
            //Creación del objeto de la clase RetornoOperacion
            RetornoOperacion retorno = new RetornoOperacion();
            //Arreglo que me permite almacenar los datos necesarios para el Store Procedure
            object[] param = { 1, 0, id_ubicacion, nombre_corto, id_tipo_patio, id_compania_propietario, tiempo_limite, id_usuario, true, "", "" };
            //Asignación  de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //retorno del resultado al metodo
            return retorno;
        }

        /// <summary>
        /// Método que permite editar registros ubicacion patio
        /// </summary>
        /// <param name="id_ubicacion"> Permite actualizar el id_ubicacion</param>
        /// <param name="nombre_corto">Permite actualizar el nombre_corto</param>
        /// <param name="id_tipo_patio">Permite actualizar el id_tipo_patio</param>
        /// <param name="id_compania_propietario">Permite actualizar el id_compania_propietario</param>
        /// <param name="tiempo_limite">permite actualizar el Tiempo Libre del registro</param>
        /// <param name="id_usuario">Permite actualizar el id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaUbicacionPatio(int id_ubicacion, string nombre_corto, int id_tipo_patio, int id_compania_propietario, int tiempo_limite, int id_usuario)
        {
            //Retorna e Invoca al Método Privado editaUbicacionPatio
            return this.editaUbicacionPatio(id_ubicacion, nombre_corto, id_tipo_patio, id_compania_propietario, tiempo_limite, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Método que permite Deshabilitar un registro Ubicacion Patio
        /// </summary>
        /// <param name="id_usuario"> Permite actualizar el campo id_usuario del registro Ubicacion Patio </param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUbicacionPatio(int id_usuario)
        {
            //Retorna e Invoca al Método Privado edita UbicacionPatio
            return this.editaUbicacionPatio(this.id_ubicacion, this.nombre_corto, this.id_tipo_patio, this.id_compania_propietario, this.tiempo_limite, id_usuario, false);
        }

        #endregion
    }
}
