using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de todas la Operaciones de los Tipos de Archivo
    /// </summary>
    public class ArchivoTipo : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los distintos contenidos de un archivo de descarga
        /// </summary>
        public enum ContentType
        {
            /// <summary>
            /// Texto con formato HTML
            /// </summary>
            text_HTML = 1,
            /// <summary>
            /// Texto Plano
            /// </summary>
            text_Plain = 2,
            /// <summary>
            /// Imagen en formato JPEG
            /// </summary>
            image_JPEG = 3,
            /// <summary>
            /// Documento PDF
            /// </summary>
            application_PDF = 4,
            /// <summary>
            /// Flujo de archivo binario
            /// </summary>
            binary_octetStream = 5,
            /// <summary>
            /// Documneto Excel
            /// </summary>
            ms_excel = 6,
            /// <summary>
            /// Flujo de archivo generado por alguna aplicación específica
            /// </summary>
            application_octetStream = 7,
            /// <summary>
            /// Archivo XML
            /// </summary>
            text_xml = 8
        }
        /// <summary>
        /// Define los distintos contenidos de un archivo de descarga
        /// </summary>
        public enum Unidad
        {
            /// <summary>
            /// Capadidad en  bits
            /// </summary>
            bit = 1,
            /// <summary>
            /// Capacidad en bytes
            /// </summary>
            bytes = 2,
            /// <summary>
            /// Capacidad en kilobyte 
            /// </summary>
            KB = 3,
            /// <summary>
            /// Capacidad en megabytes 
            /// </summary>
            MB = 4,
            /// <summary>
            /// Capacidad en gigabyte 
            /// </summary>
            GB = 5,
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del stored procedure encargado de realizar las acciones en BD
        /// </summary>
        public const string _nom_sp = "global.sp_archivo_tipo_tat";

        private int _id_archivo_tipo;
        /// <summary>
        /// Atributo encargado de almacenar el Id Principal del Tipo de Archivo
        /// </summary>
        public int id_archivo_tipo { get { return this._id_archivo_tipo; } }
        private string _extension;
        /// <summary>
        /// Atributo encargado de almacenar la Extension del Archivo
        /// </summary>
        public string extension { get { return this._extension; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción del Archivo
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private decimal _tamano_archivo;
        /// <summary>
        /// Atributo encargado de almacenar el Tamaño del Archivo
        /// </summary>
        public decimal tamano_archivo { get { return this._tamano_archivo; } }
        private byte _id_content_type;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo del Archivo
        /// </summary>
        public byte id_content_type { get { return this._id_content_type; } }
        /// <summary>
        /// Atributo encargado de almacenar el Tipo del Archivo (Enumeración)
        /// </summary>
        public ContentType content_type { get { return (ContentType)this._id_content_type; } }
        private byte _id_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Unidad
        /// </summary>
        public byte id_unidad { get { return this._id_unidad; } }
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Unidad (Enumeración)
        /// </summary>
        public Unidad unidad { get { return (Unidad)this._id_unidad; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encaragdo de Inicializar los Atributos por Defecto
        /// </summary>
        public ArchivoTipo()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encaragdo de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro"></param>
        public ArchivoTipo(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ArchivoTipo()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_archivo_tipo = 0;
            this._extension = "";
            this._descripcion = "";
            this._tamano_archivo = 0;
            this._id_content_type = 0;
            this._id_unidad = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Registro
        /// </summary>
        /// <param name="id_archivo_tipo"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_archivo_tipo)
        {   //Declarando variable de retorno
            bool resultado = false;
            //Inicializamos el arreglo de parametros
            object[] param = { 3, id_archivo_tipo, "", "", 0, 0, 0, 0, false, "", "" };
            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {   //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_archivo_tipo = Convert.ToInt32(r["Id"]);
                        this._extension = r["Extension"].ToString();
                        this._descripcion = r["Descripcion"].ToString();
                        this._tamano_archivo = Convert.ToDecimal(r["TamanoArchivo"]);
                        this._id_content_type = Convert.ToByte(r["ContentType"]);
                        this._id_unidad = Convert.ToByte(r["Unidad"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    resultado = true;
                }
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="extension">Extensión del Archivo</param>
        /// <param name="descripcion">Descripcion del Archivo</param>
        /// <param name="tamano_archivo">Tamaño del Archivo</param>
        /// <param name="id_content_type">Tipo de Contenido del Archivo</param>
        /// <param name="id_unidad">Tipo de Unidad del Archivo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaArchivoTipo(string extension, string descripcion, decimal tamano_archivo, 
                                                        byte id_content_type, byte id_unidad, int id_usuario, bool habilitar)
        {   //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_archivo_tipo, extension, descripcion, tamano_archivo, id_content_type,
                                 id_unidad, id_usuario, habilitar,"", "" };
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Tipos de Archivo
        /// </summary>
        /// <param name="extension">Extensión del Archivo</param>
        /// <param name="descripcion">Descripcion del Archivo</param>
        /// <param name="tamano_archivo">Tamaño del Archivo</param>
        /// <param name="id_content_type">Tipo de Contenido del Archivo</param>
        /// <param name="id_unidad">Tipo de Unidad del Archivo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaArchivoTipo(string extension, string descripcion, decimal tamano_archivo, 
                                                            ContentType id_content_type, Unidad id_unidad, int id_usuario)
        {   //Inicializando arreglo de parámetros
            object[] param = { 1, 0, extension, descripcion, tamano_archivo, (byte)id_content_type,
                                (byte)id_unidad, id_usuario, true,"", "" };
            //Realizamos la inserción del registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
        }
        /// <summary>
        /// Método Público encargado de Editar los Tipos de Archivo
        /// </summary>
        /// <param name="extension">Extensión del Archivo</param>
        /// <param name="descripcion">Descripcion del Archivo</param>
        /// <param name="tamano_archivo">Tamaño del Archivo</param>
        /// <param name="id_content_type">Tipo de Contenido del Archivo</param>
        /// <param name="id_unidad">Tipo de Unidad del Archivo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaArchivoTipo(string extension, string descripcion, decimal tamano_archivo, 
                                                    ContentType id_content_type, Unidad id_unidad,
                                                    int id_usuario)
        {   //Invocando Método de Actualización
            return actualizaArchivoTipo(extension, descripcion, tamano_archivo, (byte)id_content_type,
                                (byte)id_unidad, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Tipos de Archivo
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaArchivoTipo(int id_usuario)
        {   //Invocando Método de Actualización
            return actualizaArchivoTipo(this._extension, this._descripcion, this._tamano_archivo, this._id_content_type,
                                        this._id_unidad, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Tipo de Archivo
        /// </summary>
        /// <returns></returns>
        public bool ActualizaArchivoTipo()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_archivo_tipo);
        }

        #endregion
    }
}
