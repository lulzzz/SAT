using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// 
    /// </summary>
    public class Departamento :Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo privado al cual se le asigna el nombre del SP de la tabla
        /// </summary>
        private static string  nom_sp="global.sp_departamento_td";
 
        private int _id_departamento;
        /// <summary>
        /// Id que corresponde a departamento
        /// </summary>
        public int id_departamento
        {
            get { return _id_departamento; }
        }
        
        private string _nombre;
        /// <summary>
        ///Nombre que corresponde a departamento
        /// </summary>
        public string nombre
        {
            get { return _nombre; }
        }
 
        private int _id_empleado_responsable;
        /// <summary>
        /// Id que corresponde al empleado responsables
        /// </summary>
        public int id_departamento_responsable
        {
            get { return _id_empleado_responsable; }
        }
  
        private bool _bit_administrador;
        /// <summary>
        /// Identificador que corresponde al administrador
        /// </summary>
        public bool bit_administrador
        {
            get { return _bit_administrador; }
        }

        private bool _habilitar;
        /// <summary>
        /// Estado de un registro que corresponde al departamento
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion 

        #region Constructores

        /// <summary>
        /// Constructor por default que inicializa los atributos privados
        /// </summary>
        public Departamento()
        {
            this._id_departamento = 0;
            this._nombre = "";
            this._id_empleado_responsable = 0;
            this._bit_administrador = false;
            this._habilitar= false;

        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_departamento">Permite buscar y asignar  registro al objeto  dado un valor</param>
        public Departamento(int id_departamento)
        {
        //Invocacion al metodo privado de carga de atributos
            cargaAtributosInstancia(id_departamento);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Departamento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privado

        /// <summary>
        /// Método que permite actualizar campos de un registro departamento
        /// </summary>
        /// <param name="nombre">Permite actualizar el campo nombre del departamento</param>
        /// <param name="id_empleado_responsable">Permite actualizar el campo id_empleado_responsable del departamento</param>
        /// <param name="bit_administrador">Permite actualizar el campo bit_administrador del departamento</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario del departamento</param>
        /// <param name="habilitar">Permite actualizar el campo habilitar del departamento</param>
        /// <returns></returns>
        private RetornoOperacion editaDepartamento(string nombre, int id_empleado_responsable, bool bit_administrador, int id_usuario, bool habilitar ) 
        {
            //Creación del objeto retorno 
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el SP de la tabla.
            object[] param = {2, this.id_departamento,nombre, id_empleado_responsable, bit_administrador,id_usuario,habilitar,"","" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno de valor al método
            return retorno;
        }

        /// <summary>
        /// Método privado que carga los atributos de un registro de departamento
        /// </summary>
        /// <param name="id_departamento">Id que que inicializara la busqueda y carga de atributos de los registros de departamento </param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_departamento)
        {
            //Declaración del objeto de retorno
            bool retorno = false;
            //Creación y Aignación de un arreglo con los valores necesarios para el SP de la tabla
            object[] param = { 3, id_departamento, "", 0, false, 0, false, "", "" };

            //Invocación del SP 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos del DataSet con el registro a buscar
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //recorrido de las filas y almacena los valores en la r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_departamento = id_departamento;
                        _nombre = Convert.ToString(r["Nombre"]);
                        _id_empleado_responsable = Convert.ToInt32(r["IdEmpleadoResponsable"]);
                        _bit_administrador = Convert.ToBoolean(r["Administrador"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno, solo si se cumplen las sentencias de validacion de datos
                    retorno = true;
                }
            }
            //retorno del resultado al método
            return retorno;
        }
        #endregion 

        #region Métodos Publicos

        /// <summary>
        /// Método que me permite insertar datos a departamento
        /// </summary>
        /// <param name="nombre">Valor que se inserta en el campo nombre</param>
        /// <param name="id_empleado_responsable">Valor que se inserta en el campo id_empleado_responsable</param>
        /// <param name="bit_administrador">Valor que se inserta en el campo bit_administrador</param>
        /// <param name="id_usuario">Valor que se inserta en el campo id_usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarDepartamento(string nombre, int id_empleado_responsable,bool bit_administrador, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del Arreglo con los datos necesarios para el SP de la tabla
            object[] param = { 1, 0, nombre, id_empleado_responsable, bit_administrador, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno de resultado al método
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los campos de un registro departamento
        /// </summary>
        /// <param name="nombre">Permite actualizar el campo nombre</param>
        /// <param name="id_empleado_responsable">Permite actualizar el campo id_empleado_responsable</param>
        /// <param name="bit_administrador">Permite actualizar el campo bit_administrador</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarDepartamento(string nombre, int id_empleado_responsable, bool bit_administrador, int id_usuario)
        {
            //Invoca y retorna un resultado  al método privado editaDepartamento
            return this.editaDepartamento(nombre, id_empleado_responsable, bit_administrador,id_usuario, this._habilitar);

        }

        /// <summary>
        /// Método que me permite cambiar el estado de un registro (Habilitar/Deshabilitar)
        /// </summary>
        /// <param name="id_usuario">Permite Actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarDepartamento(int id_usuario) 
        {
            //Invoca y Retorna un resultado al método privado editaDepartamento
            return this.editaDepartamento(this.nombre, this._id_empleado_responsable, this.bit_administrador, id_usuario, false);
         
        }

        #endregion

    }
}
