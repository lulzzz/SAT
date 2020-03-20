using System;
using TSDK.Base;
using System.Data;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones de relacionadas con 
    /// </summary>
    public class AsignacionTipoServicio : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo privado estático que almacena el nombre del Store Procedure de la tabla
        /// </summary>
        private static string nom_sp = "global.sp_asignacion_tipo_servicio_tats";
      
        private int _id_asignacion_tipo_servicio;
        /// <summary>
        /// Id que corresponde a la asignación de tipo servicio
        /// </summary>
        public int id_asignacion_tipo_servicio
        {
            get { return _id_asignacion_tipo_servicio; }
        }
        private int _id_proveedor;
        /// <summary>
        /// Id que corresponde al proveedor
        /// </summary>
        public int id_proveedor
        {
            get { return _id_proveedor; }
        }
        private int _id_tipo_servicio;
        /// <summary>
        /// Id que corresponde al tipo de servicio
        /// </summary>
        public int id_tipo_servicio
        {
            get { return _id_tipo_servicio; }
        }
        private bool _habilitar;
        /// <summary>
        /// Corresponde al Estatus de habilitación de un registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por Default que inicializa a los atributos
        /// </summary>
        public AsignacionTipoServicio()
        {
            this._id_asignacion_tipo_servicio = 0;
            this._id_proveedor = 0;
            this._id_tipo_servicio = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_asignacion_tipo_servicio">Id con el cual se inicializara la busqueda de los valores de mi objeto </param>
        public AsignacionTipoServicio(int id_asignacion_tipo_servicio) 
        {
            //Invocacion del método privado de Carga de atributos
            cargaAtributoInstancia(id_asignacion_tipo_servicio);
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Proveedor y un Tipo de Servicio
        /// </summary>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_tipo_servicio">Tipo de Servicio</param>
        public AsignacionTipoServicio(int id_proveedor, int id_tipo_servicio)
        {
            //Invocacion del método privado de Carga de atributos
            cargaAtributoInstancia(id_proveedor, id_tipo_servicio);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AsignacionTipoServicio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro 
        /// </summary>
        /// <param name="id_asignacion_tipo_servicio">Campo que me permite realizar la busqueda de un registro </param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_asignacion_tipo_servicio)
        {
            //Declarando al Objeto de Retorno
            bool retorno = false;
            //Creación y asignación de valores a un arreglo, necesarios para el SP_ de la tabla
            object[] param = { 3, id_asignacion_tipo_servicio, 0, 0, 0, false, "", "" };
            //Invocación al Store Procedure
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos del DataSet con los de Store Procedure de la Tabla
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas del la tabla del DataSet, almacenadas en r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_asignacion_tipo_servicio = id_asignacion_tipo_servicio;
                        _id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        _id_tipo_servicio = Convert.ToInt32(r["IdTipoServicio"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno siempre y cuando se cumpla la sentencia de validadcion de datos
                    retorno = true;
                }
            }
            //Retorno del resultado al método
            return retorno;
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Proveedor y un Tipo de Servicio
        /// </summary>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_tipo_servicio">Tipo de Servicio</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_proveedor, int id_tipo_servicio)
        {
            //Declarando al Objeto de Retorno
            bool retorno = false;
            //Creación y asignación de valores a un arreglo, necesarios para el SP_ de la tabla
            object[] param = { 4, 0, id_proveedor, id_tipo_servicio, 0, false, "", "" };
            //Invocación al Store Procedure
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos del DataSet con los de Store Procedure de la Tabla
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas del la tabla del DataSet, almacenadas en r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_asignacion_tipo_servicio = id_asignacion_tipo_servicio;
                        _id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        _id_tipo_servicio = Convert.ToInt32(r["IdTipoServicio"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno siempre y cuando se cumpla la sentencia de validadcion de datos
                    retorno = true;
                }
            }
            //Retorno del resultado al método
            return retorno;
        }
        /// <summary>
        /// Método privado que permite actualizar registros de Asignación Tipo Servicio
        /// </summary>
        /// <param name="id_proveedor">Permite actualizar el campo id_proveedor</param>
        /// <param name="id_tipo_servicio">Permite actualizar el campo id_tipo_servicio</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <param name="habilitar">Permite actualizar el estado del registro Habilitado </param>
        /// <returns></returns>
        private RetornoOperacion editarAsignacionTipoServicio(int id_proveedor, int id_tipo_servicio, int id_usuario, bool habilitar)
        {
            // Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al Arreglo, necesarios para el SP_ de la tabla
            object[] param = { 2,this.id_asignacion_tipo_servicio, id_proveedor, id_tipo_servicio, id_usuario, habilitar,"","" };
            //Asignación de valor al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Devuelve el resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método que inserta datos a Asignación Tipo Servicio
        /// </summary>
        /// <param name="id_proveedor">Valor que se inserta en el campo id_proveedor </param>
        /// <param name="id_tipo_servicio">Valor que se inserta en el campo id_tipo_servicio</param>
        /// <param name="id_usuario">Valor que se inserta en el campo id_usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarAsignacionTipoServicio(int id_proveedor, int id_tipo_servicio, int id_usuario) 
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            // Creación y Asignación de valores al Arreglos, necesario para el SP_ de la tabla
            object[] param = {1, 0, id_proveedor, id_tipo_servicio,id_usuario,true,"",""};
            //Asignación de valores al objeto retorno 
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Devuelve el resultado al método
            return retorno;            
        }

        /// <summary>
        /// Método que permite actualizar los registros de Asignación Tipo Servicio
        /// </summary>
        /// <param name="id_proveedor"> Permite actualizar el campo id_proveedor</param>
        /// <param name="id_tipo_servicio"> Permite actualizar el campo id_tipo_servicio</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarAsignacionTipoServicio(int id_proveedor, int id_tipo_servicio, int id_usuario) 
        {
            // invoca y retorna el resultado del metodo privado edita Asignación Tipo Servicio
            return this.editarAsignacionTipoServicio(id_proveedor, id_tipo_servicio, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que modifica el estado de un registro
        /// </summary>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarAsignacionTipoServicio(int id_usuario) 
        {
            // Invoca y Retorna el resultado del método privado editaAsignaciónTipoServicio
            return this.editarAsignacionTipoServicio(this.id_proveedor, this.id_tipo_servicio, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Obtener las Asignaciones por Proveedor
        /// </summary>
        /// <param name="id_proveedor">Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneAsignacionesPorProveedor(int id_proveedor)
        {
            //Declarando Objeto de Retorno
            DataTable dtAsignaciones = null;

            // Creación y Asignación de valores al Arreglos, necesario para el SP_ de la tabla
            object[] param = { 4, 0, id_proveedor, 0, 0, false, "", "" };

            //Ejecutando SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado
                    dtAsignaciones = ds.Tables["Table"];
            }

            //Devolviendo Asignaciones
            return dtAsignaciones;
        }

        #endregion
    }
}
