using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de las operaciones correspondientes a las Ciudades
    /// </summary>
    public class Ciudad : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_ciudad_tc";

        private int _id_ciudad;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Ciudad
        /// </summary>
        public int id_ciudad { get { return this._id_ciudad; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción de la Ciudad
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private byte _id_estado;
        /// <summary>
        /// Atributo encargado de almacenar el Estado de la Ciudad
        /// </summary>
        public byte id_estado { get { return this._id_estado; } }
        private string _estado;
        /// <summary>
        /// Atributo encargado de almacenar el Nombre del Estado de la Ciudad
        /// </summary>
        public string estado { get { return this._estado; } }
        private string _pais;
        /// <summary>
        /// Atributo encargado de almacenar el Nombre del Pais al que pertenece la Ciudad
        /// </summary>
        public string pais { get { return this._pais; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Costructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Ciudad()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Costructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public Ciudad(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Ciudad()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_ciudad = 0;
            this._descripcion = "";
            this._id_estado = 0;
            this._estado = "";
            this._pais = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Declarando Objeto de Retorno
            object[] param = { 3, id_registro, "", 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada Fila
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_ciudad = id_registro;
                        this._descripcion = dr["Descripcion"].ToString();
                        this._id_estado = Convert.ToByte(dr["IdEstado"]);
                        this._estado = dr["Estado"].ToString();
                        this._pais = dr["Pais"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
                //Asignando Resultado Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="descripcion">Descripción de la Ciudad</param>
        /// <param name="id_estado">Referencia del Estado de la Ciudad</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion, byte id_estado, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Objeto de Retorno
            object[] param = { 2, this._id_ciudad, descripcion, id_estado, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Ciudades
        /// </summary>
        /// <param name="descripcion">Descripción de la Ciudad</param>
        /// <param name="id_estado">Referencia del Estado de la Ciudad</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCiudad(string descripcion, byte id_estado, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Objeto de Retorno
            object[] param = { 1, 0, descripcion, id_estado, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Ciudades
        /// </summary>
        /// <param name="descripcion">Descripción de la Ciudad</param>
        /// <param name="id_estado">Referencia del Estado de la Ciudad</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaCiudad(string descripcion, byte id_estado, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion, id_estado, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Ciudades
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCiudad(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion, this._id_estado, id_usuario, false);
        }

        /// <summary>
        /// Método encargado de retornar la descripción de la ciuadad en un formato especificado.
        /// </summary>
        /// <param name="mensaje">>Mensaje a retornar. Puede utilizar comodines de sustitución para incluir los valores de la ciudad con el siguiente orden: {0} ciudad, {1} estado y {2} pais</param>
        /// <returns></returns>
        public override string ToString()
        {  

            return  string.Format("({0}, {1})", this.descripcion.ToUpper(), this._estado.ToUpper());
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de la Ciudad
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCiudad()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_ciudad);
        }

        #endregion
    }
}
