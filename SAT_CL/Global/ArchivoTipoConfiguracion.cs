using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones de la Configuración de los Tipos de Archivo
    /// </summary>
    public class ArchivoTipoConfiguracion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Define el nombre del stored procedure encargado de realizar las acciones en BD
        /// </summary>
        public const string _nom_sp = "global.sp_archivo_tipo_configuracion_tda";

        private int _id_archivo_tipo_configuracion;
        /// <summary>
        /// Atributo encargado de almacenar la Configuración del Tipo de Archivo
        /// </summary>
        public int id_archivo_tipo_configuracion { get { return this._id_archivo_tipo_configuracion; } }
        private int _id_archivo_tipo;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Archivo
        /// </summary>
        public int id_archivo_tipo { get { return this._id_archivo_tipo; } }
        private int _id_compania;
        /// <summary>
        /// Atributo encargado de almacenar la Compañia
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción del Archivo
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private int _max_permitido;
        /// <summary>
        /// Atributo encargado de almacenar el Valor Máximo del Archivo
        /// </summary>
        public int max_permitido { get { return this._max_permitido; } }
        private bool _bit_editable;
        /// <summary>
        /// Atributo encargado de almacenar la Propiedad Editar del Archivo
        /// </summary>
        public bool bit_editable { get { return this._bit_editable; } }
        private bool _bit_eliminar;
        /// <summary>
        /// Atributo encargado de almacenar la Propiedad Eliminar del Archivo
        /// </summary>
        public bool bit_eliminar { get { return this._bit_eliminar; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Tabla del Archivo
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ArchivoTipoConfiguracion()
        {   
            //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ArchivoTipoConfiguracion(int id_registro)
        {   
            //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dados el Tipo de Archivo y la Compania
        /// </summary>
        /// <param name="id_archivo_tipo">Tipo de Archivo</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_tabla">Tabla</param>
        public ArchivoTipoConfiguracion(int id_archivo_tipo, int id_compania, int id_tabla)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_archivo_tipo, id_compania, id_tabla);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ArchivoTipoConfiguracion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando variable de retorno
            bool result = false;
            //Inicializamos el arreglo de parametros
            object[] param = { 3, id_registro, 0,0, "", 0, false, false, 0, 0, false, "", "" };
            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {   //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_archivo_tipo_configuracion = Convert.ToInt32(r["Id"]);
                        this._id_archivo_tipo = Convert.ToInt32(r["IdTipoArchivo"]);
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._descripcion = r["Descripcion"].ToString();
                        this._max_permitido = Convert.ToInt32(r["MaximoPermitido"]);
                        this._bit_editable = Convert.ToBoolean(r["BitEditable"]);
                        this._bit_eliminar = Convert.ToBoolean(r["BitEliminar"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo resultado
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dados el Tipo de Archivo y la Compania
        /// </summary>
        /// <param name="id_archivo_tipo">Tipo de Archivo</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_tabla">Tabla</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_archivo_tipo, int id_compania, int id_tabla)
        {
            //Declarando variable de retorno
            bool result = false;

            //Inicializamos el arreglo de parametros
            object[] param = { 5, 0, id_archivo_tipo, id_compania, "", 0, false, false, id_tabla, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_archivo_tipo_configuracion = Convert.ToInt32(r["Id"]);
                        this._id_archivo_tipo = Convert.ToInt32(r["IdTipoArchivo"]);
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._descripcion = r["Descripcion"].ToString();
                        this._max_permitido = Convert.ToInt32(r["MaximoPermitido"]);
                        this._bit_editable = Convert.ToBoolean(r["BitEditable"]);
                        this._bit_eliminar = Convert.ToBoolean(r["BitEliminar"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }

                    //Asignando Valor Positivo
                    result = true;
                }
            }

            //Devolviendo resultado
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_archivo_tipo">Id de Tipo de Archivo</param>
        /// <param name="id_compania">Id Compañia</param>
        /// <param name="descripcion">Descripción del Tipo</param>
        /// <param name="max_permitido">Maximo Tamaño permitido</param>
        /// <param name="bit_editable">Propiedad Editable</param>
        /// <param name="bit_eliminar">Propiedad Eliminar</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_archivo_tipo, int id_compania, string descripcion, int max_permitido,
                                                    bool bit_editable, bool bit_eliminar, int id_tabla, int id_usuario,
                                                    bool habilitar)
        {   //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_archivo_tipo_configuracion, id_compania, id_archivo_tipo, descripcion, max_permitido, bit_editable,
                                 bit_eliminar,id_tabla,id_usuario, habilitar,"", "" };
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        ///  Método Público encargado de Insertar un Detalle Archivo
        /// </summary>
        /// <param name="id_archivo_tipo">Id de Tipo de Archivo</param>
        /// <param name="id_compania">Id Compañia</param>
        /// <param name="descripcion">Descripción del Tipo</param>
        /// <param name="max_permitido">Maximo Tamaño permitido</param>
        /// <param name="bit_editable">Propiedad Editable</param>
        /// <param name="bit_eliminar">Propiedad Eliminar</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaArchivoTipoConfiguracion(int id_archivo_tipo, int id_compania, string descripcion, int max_permitido,
                                                        bool bit_editable, bool bit_eliminar, int id_tabla, int id_usuario)
        {   //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_archivo_tipo, id_compania, descripcion, max_permitido, bit_editable,
                                 bit_eliminar, id_tabla, id_usuario, true, "", "" };
            //Realizamos la inserción del registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
        }
        /// <summary>
        /// Método Público encargado de Editar un Detalle Archivo
        /// </summary>
        /// <param name="id_archivo_tipo">Id de Tipo de Archivo</param>
        /// <param name="id_compania">Id Compañia</param>
        /// <param name="descripcion">Descripción del Tipo</param>
        /// <param name="max_permitido">Maximo Tamaño permitido</param>
        /// <param name="bit_editable">Propiedad Editable</param>
        /// <param name="bit_eliminar">Propiedad Eliminar</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaArchivoTipoConfiguracion(int id_archivo_tipo, int id_compania, string descripcion, int max_permitido,
                                                        bool bit_editable, bool bit_eliminar, int id_tabla, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(id_archivo_tipo, id_compania, descripcion, max_permitido, bit_editable,
                                 bit_eliminar, id_tabla, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar Detalle Archivo
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaArchivoTipoConfiguracion(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistro(this._id_archivo_tipo, this._id_compania, this._descripcion, this._max_permitido, this._bit_editable,
                                      this._bit_eliminar, this.id_tabla, id_usuario, false);

        }
        /// <summary>
        /// Método Público encargado de Actualizar la Configuración del Archivo
        /// </summary>
        /// <returns></returns>
        public bool ActualizaArchivoConfiguracion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_archivo_tipo_configuracion);
        }
        /// <summary>
        /// Método Público encargado de Cargar el Detalle de Archivo ligado a un id Tipo Archivo
        /// </summary>
        /// <param name="id_archivo_tipo">Id Archivo Tipo</param>
        /// <returns></returns>
        public static DataTable CargaArchivoTipoConfiguracion(int id_archivo_tipo)
        {   //Declaramos objeto Retorno
            DataTable mit = null;
            //Inicializamos el arreglo de parametros
            object[] param = { 4, 0, id_archivo_tipo, 0, "", 0, false, false, 0, false, "", "" };
            //Carga Detalles
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Si existe el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor
                    mit = ds.Tables["Table"];
            }
            //Devolviendo resultado
            return mit;
        }

        #endregion
    }
}
