using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Transportistas del Patio
    /// </summary>
    public class PatioTransportista : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo que permite privado al cual se le Asigna el nombre del SP de la tabla
        /// </summary>
        private static string nom_param = "control_patio.sp_transportista_patio_ttp";

        private int _id_transportista_patio;
        /// <summary>
        /// Id que coresponde al trasportista de patio
        /// </summary>
        public int id_transportista_patio
        {
            get { return _id_transportista_patio; }
        }
 
        private string _nombre;
        /// <summary>
        /// Nombre que corresponde al nombre del transportista
        /// </summary>
        public string nombre
        {
            get { return _nombre; }
        }
  
        private int _id_patio ;
        /// <summary>
        /// Id que corresponde al patio transportista
        /// </summary>
        public int id_patio
        {
            get { return _id_patio; }
        }
  
        private bool _habilitar;
        /// <summary>
        ///Condicion que Establece un estatus de habilitación del registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
       
        #endregion 

        #region Constructores

        /// <summary>
        /// Constructor Default que Asigna valores de inicio a los atributos privados
        /// </summary>
        public PatioTransportista() 
        {
            this._id_transportista_patio = 0;
            this._nombre = "";
            this._id_patio = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que permite buscar registros dado un id
        /// </summary>
        /// <param name="id_transportista_patio">Dato que permite inicializar la  busqueda de un registro</param>
        public PatioTransportista(int id_transportista_patio)
        {
                //Invocación del método privado carga atributos
                cargaAtributoInstancia(id_transportista_patio);
        }
        
        #endregion 

        #region Destructor

         /// <summary>
         /// Destructor de la clase
         /// </summary>
        ~PatioTransportista()
         {
           Dispose(false);
         }

        #endregion

        #region Métodos Privado

        /// <summary>
        /// Método privado que carga atributos dado un Id 
        /// </summary>
        /// <param name="id_transportista_patio">Permite realizar la busqueda de los registros</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_transportista_patio)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            // Creación y Asignacion de valores al Arreglo, necesarios para el SP de la tabla
            object[] param = { 3, id_transportista_patio, "", 0, 0, false, "", "" };
            //Invocación al Store Procedure
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_param, param))
            {
                //Validación de los datos del DataSet con los valores a buscar
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas del Dataset, y almacenados en r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_transportista_patio = id_transportista_patio;
                        _nombre = Convert.ToString(r["Nombre"]);
                        _id_patio = Convert.ToInt32(r["IdPatio"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno, siempre y cuando se cumpla la validación de datos
                    retorno = true;
                }
            }
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método privado que permite actualizar registros  de Patio Transportista
        /// </summary>
        /// <param name="nombre"> Permite Actualizar el campo nombre de Patio Taansportista</param>
        /// <param name="id_patio">Permite Actualizar el campo id_patio </param>
        /// <param name="id_usuario">Permite Actualizar el campo id_usuario</param>
        /// <param name="habilitar">Permite Actualizar el campo habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarPatioTransportista (string nombre, int id_patio, int id_usuario, bool habilitar)
        {
                   //Creación del objeto retorno
                   RetornoOperacion retorno = new RetornoOperacion();
                   //Creación y Asiganación de Valores al Arreglo, necesarios para el SP de la tabla
                   object[] param = {2, this.id_transportista_patio, nombre, id_patio,id_usuario,habilitar,"","" };
                   //Asignación de valores al Objeto retorno 
                   retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_param, param);
                   //retorno del resultado al método
                   return retorno;
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método que permite insertar registros en patio transportista
        /// </summary>
        /// <param name="nombre">Permite Insertar un valor en el campo nombre de Patio Transportista </param>
        /// <param name="id_patio">Permite Insertar un valor en el campo id_patio de Patio Transportista</param>
        /// <param name="id_usuario">Permite Insertar un valor en el campo id_usuario de Patio Transportista</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarPatioTransportista(string nombre, int id_patio, int id_usuario)
        {
                //Creación del objeto retorno
                RetornoOperacion retorno = new RetornoOperacion();
                //Creación y Asignación del Arreglo, Necesarios para el SP de la tabla
                object[] param = {1, 0, nombre,id_patio,id_usuario,true,"","" };
                //Asignación de valores al objeto retorno 
                retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_param, param);
                //Retorno del resultado al método
                return retorno;
        }

        /// <summary>
        /// Método que permite editar registros de Patio Transportista
        /// </summary>
        /// <param name="nombre">Permite Actualizar el campo nombre</param>
        /// <param name="id_patio">Permite Actualizar el campo  id_patio</param>
        /// <param name="id_usuario">Permite Actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarPatioTransportista(string nombre, int id_patio, int id_usuario)
        {
                //retorna un valor al método 
                return this.editarPatioTransportista(nombre, id_patio, id_usuario, this._habilitar);
        }
        /// <summary>
        ///Método que permite Modificar el estado de un registro 
        /// </summary>
        /// <param name="id_usuario">Permite Actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarPatioTransportista(int id_usuario)
        {
                //Retorna un valor al método
                return this.editarPatioTransportista(this.nombre, this.id_patio, id_usuario, false);
        }

        #endregion

    }
}
