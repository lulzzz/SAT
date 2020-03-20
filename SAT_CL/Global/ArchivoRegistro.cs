using System;
using System.Data;
using TSDK.Base;
using System.IO;
using System.Transactions;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las Operaciones de los Registros de los Archivos
    /// </summary>
    public class ArchivoRegistro : Disposable
    {   
        #region Atributos

        /// <summary>
        /// Define el nombre del stored procedure encargado de realizar las acciones en BD
        /// </summary>
        public static string _nom_sp = "global.sp_archivo_registro_tar";

        private int _id_archivo_registro;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Registro del Archivo
        /// </summary>
        public int id_archivo_registro { get { return this._id_archivo_registro; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private int _id_archivo_tipo_configuracion;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Configuración del Archivo
        /// </summary>
        public int id_archivo_tipo_configuracion { get { return this._id_archivo_tipo_configuracion; } }
        private string _url;
        /// <summary>
        /// Atributo encargado de almacenar la URL del registro del Archivo
        /// </summary>
        public string url { get { return this._url; } }
        private string _referencia;
        /// <summary>
        /// Atributo encargado de almacenar la Referencia del registro del Archivo
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Cosntructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ArchivoRegistro()
        {   //Invocando Método de carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Cosntructor de la Clase encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ArchivoRegistro(int id_registro)
        {   //Invocando Método de carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ArchivoRegistro()
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
            this._id_archivo_registro = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._id_archivo_tipo_configuracion = 0;
            this._url = "";
            this._referencia = "";
            this._habilitar = false;
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
            object[] param = { 3, id_registro, 0, 0, 0, "", "", 0, false, "", "" };
            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {   //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_archivo_registro = id_registro;
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._id_archivo_tipo_configuracion = Convert.ToInt32(r["IdDetalleArchivo"]);
                        this._url = r["Url"].ToString();
                        this._referencia = r["Referencia"].ToString(); ;
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]); ;
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
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Id de Tipo de COnfiguración de Archivo</param>
        /// <param name="url">URL de Ubicación del Archivo</param>
        /// <param name="referencia">Referencia del Archivo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_tabla, int id_registro, int id_archivo_tipo_configuracion, string url,
                                                    string referencia, int id_usuario, bool habilitar)
        {   //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_archivo_registro,id_tabla, id_registro, id_archivo_tipo_configuracion, url,
                                  referencia, id_usuario, habilitar, "", ""};
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);                
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar un Archivo Registro, guardando el archivo de forma física en la ubicación indicada
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Id de Tipo de Archivo configurado</param>
        /// <param name="referencia">Referencia descriptiva</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="archivo">Arreglo de bytes con contenido del archivo</param>
        /// <param name="ruta_archivo_con_extension">Ruta de archivo con extensión</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaArchivoRegistro(int id_tabla, int id_registro, int id_archivo_tipo_configuracion,
                                                        string referencia, int id_usuario, byte[] archivo, string ruta_archivo_con_extension)
        {   
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos el Detalle Archivo
            using (ArchivoTipoConfiguracion objDetalle = new ArchivoTipoConfiguracion(id_archivo_tipo_configuracion))
                //Validamos que exista objeto
                if (objDetalle.id_archivo_tipo_configuracion > 0)
                {   
                    //Instanciamos Tipo de Archivo
                    using (ArchivoTipo objTipo = new ArchivoTipo(objDetalle.id_archivo_tipo))
                    {   
                        //Validamos que exista objeto
                        if (objTipo.id_archivo_tipo > 0)
                        {   
                            //Validamos extension
                            if (objTipo.extension.ToLower() == ("." + TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.RegresaCadenaSeparada(TSDK.Base.Cadena.InvierteCadena(ruta_archivo_con_extension), '.', 0))).ToLower())
                            {
                                //Convertimos tamaño de Archivo bytes
                                decimal unidad = (decimal)TSDK.Base.Archivo.ConvierteUnidadaBytes((double)objTipo.tamano_archivo, (TSDK.Base.Archivo.UnidadInformacion)objTipo.id_unidad);
                                //Si el tamaño de archivo es menor al establecido
                                if (archivo.LongLength <= unidad)
                                {
                                    //Guardamos el Archivo en la Ruta Especifica
                                    resultado = Archivo.GuardaArchivoCreandoRuta(archivo, ruta_archivo_con_extension, false);

                                    //Si se Guarda Correctamente
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Guardamos Refistro en Base de Datos
                                        //Inicializando arreglo de parámetros
                                        object[] param = { 1, 0,id_tabla, id_registro, id_archivo_tipo_configuracion, ruta_archivo_con_extension,
                                                    referencia, id_usuario, true, "", ""};
                                        //Realizamos la inserción del registro
                                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                                        //Validamos Guardado en Base de Datos, si no se guardó correctamente
                                        if (!resultado.OperacionExitosa)
                                            //Eliminamos  Archivo Creado y el directorio, si es que ya no tiene contenido
                                            TSDK.Base.Archivo.EliminaArchivo(ruta_archivo_con_extension, true);
                                    }
                                }
                                else//Mostramos Error
                                    resultado = new RetornoOperacion(string.Format("El tamaño de archivo excede el permitido: {0} Bytes", archivo.LongLength));
                            }
                            else//Mostramos Error
                                resultado = new RetornoOperacion("Extensión de Archivo invalida");
                        }
                        else//Mostramos Error
                            resultado = new RetornoOperacion("No se encontraron datos complementarios Tipo Archivo");
                    }
                }
                else//Mostramos Error
                    resultado = new RetornoOperacion("No se encontraron datos complementarios Detalle Archivo");
            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Editar un Archivo Registro 
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Id de Tipo de Archivo configurado</param>
        /// <param name="url">URL del Archivo</param>
        /// <param name="referencia">Referencia del Archivo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaArchivoRegistro(int id_tabla, int id_registro, int id_archivo_tipo_configuracion, string url,
                                                          string referencia, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tabla, id_registro, id_archivo_tipo_configuracion, url,
                                            referencia, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar el Archivo del Registro, junto con sus Campos
        /// </summary>
        /// <param name="ruta_archivo_con_extension"></param>
        /// <param name="referencia"></param>
        /// <param name="id_usuario"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaArchivoRegistro(string ruta_archivo_con_extension, string referencia, int id_usuario, byte[] archivo)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciamos el Detalle Archivo
            using (ArchivoTipoConfiguracion objDetalle = new ArchivoTipoConfiguracion(this._id_archivo_tipo_configuracion))
            {
                //Validamos que exista objeto
                if (objDetalle.id_archivo_tipo_configuracion > 0)
                {
                    //Instanciamos Tipo de Archivo
                    using (ArchivoTipo objTipo = new ArchivoTipo(objDetalle.id_archivo_tipo))
                    {
                        //Validamos que exista objeto
                        if (objTipo.id_archivo_tipo > 0)
                        {
                            //Validamos extension
                            if (objTipo.extension == "." + TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.RegresaCadenaSeparada(TSDK.Base.Cadena.InvierteCadena(ruta_archivo_con_extension), '.', 0)))
                            {
                                //Convertimos tamaño de Archivo bytes
                                decimal unidad = (decimal)TSDK.Base.Archivo.ConvierteUnidadaBytes((double)objTipo.tamano_archivo, (TSDK.Base.Archivo.UnidadInformacion)objTipo.id_unidad);
                                
                                //Si el tamaño de archivo es menor al establecido
                                if (archivo.LongLength <= unidad)
                                {
                                    //Guardamos el Archivo en la Ruta Especifica
                                    result = Archivo.GuardaArchivoCreandoRuta(archivo, ruta_archivo_con_extension, false);

                                    //Si se Guarda Correctamente
                                    if (result.OperacionExitosa)
                                    {
                                        //Invocando Método de Actualización
                                        result = this.actualizaRegistros(this._id_tabla, this._id_registro, this._id_archivo_tipo_configuracion, ruta_archivo_con_extension,
                                                                        referencia, id_usuario, this._habilitar);

                                        //Validamos Guardado en Base de Datos, si no se guardó correctamente
                                        if (!result.OperacionExitosa)
                                            //Eliminamos  Archivo Creado y el directorio, si es que ya no tiene contenido
                                            TSDK.Base.Archivo.EliminaArchivo(ruta_archivo_con_extension, true);
                                        else
                                            //Eliminamos Archivo Anterior
                                            TSDK.Base.Archivo.EliminaArchivo(this._url, true);
                                    }
                                }
                                else//Mostramos Error
                                    result = new RetornoOperacion(string.Format("El tamaño de archivo excede el permitido: {0} Bytes", archivo.LongLength));
                            }
                            else//Mostramos Error
                                result = new RetornoOperacion("Extensión de Archivo invalida");
                        }
                        else//Mostramos Error
                            result = new RetornoOperacion("No se encontraron datos complementarios Tipo Archivo");
                    }
                }
                else//Mostramos Error
                    result = new RetornoOperacion("No se encontraron datos complementarios Detalle Archivo");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar un Archivo Registro
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaArchivoRegistro(int id_usuario)
        {   
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creando Transacción
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invocando Método de Actualización
                resultado = this.actualizaRegistros(this._id_tabla, this._id_registro, this._id_archivo_tipo_configuracion, this._url,
                                           this._referencia, id_usuario, false);
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                    //Eliminamos  Archivo Creado
                    resultado = TSDK.Base.Archivo.EliminaArchivo(this._url);

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Finalizando transacción
                    scope.Complete();
            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        ///  Carga Archivos Registros
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Id de Tipo de archivo copnfigurado a la tabla</param>
        /// <param name="id_compania">Id Compañia</param>
        /// <returns></returns>
       public static DataTable CargaArchivoRegistro(int id_tabla, int id_registro, int id_archivo_tipo_configuracion, int id_compania)
        {   //Declaramos objeto Retorno
            DataTable mit = null;
            //Inicializamos el arreglo de parametros
            object[] param = { 5, 0, id_tabla, id_registro, id_archivo_tipo_configuracion, "", "", 0, false, id_compania, "" };
            //Carga Detalles
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Si existe el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Carga Numero Maximo  del Nombre del archivo
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_tipo">Id de Tipo</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public static int CargaNombreNumeroMaximoArchivo(int id_tabla, int id_tipo, int id_registro)
        {   //Declaremos Variable
            int NumeroMaximo = 0;
            //Inicializamos el arreglo de parametros
            object[] param = { 6, 0, id_tabla, id_registro, 0, "", "", 0, false, id_tipo, "" };
            //Carga Detalles
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {   //Si existe el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    //Asignamos el Valor
                    NumeroMaximo = Convert.ToInt32(mit.Rows[0].ItemArray[0]);
                //Devolviendo resultado
                return NumeroMaximo;
            }
        }

        /// <summary>
        /// Carga Numero de Archivos Registrados
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int CargaNumeroArchivoCreados(string url)
        {   //Declaremos Variable
            int NumeroMaximo = 0;
            //Inicializamos el arreglo de parametros
            object[] param = { 7, 0, 0, 0, 0, url, "", 0, false, "", "" };
            //Carga Detalles
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {   //Si existe el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    //Asignamos el Valor
                    NumeroMaximo = Convert.ToInt32(mit.Rows[0].ItemArray[0]);
                //Devolviendo resultado
                return NumeroMaximo;
            }
        }
        /// <summary>
        /// Método encargado de Obtener los Registros del Archivo
        /// </summary>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Tipo de Configuración del Archivo</param>
        /// <returns></returns>
        public static DataTable ObtieneArchivoRegistro(int id_tabla, int id_registro, int id_archivo_tipo_configuracion)
        {
            //Declaramos objeto Retorno
            DataTable dtArchivosRegistro = null;

            //Inicializamos el arreglo de parametros
            object[] param = { 8, 0, id_tabla, id_registro, id_archivo_tipo_configuracion, "", "", 0, false, "", "" };
            
            //Carga Detalles
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Si existe el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    
                    //Asignando Valor Obtenido
                    dtArchivosRegistro = ds.Tables["Table"];
            }

            //Devolviendo resultado
            return dtArchivosRegistro;
        }

        #endregion      
        
    }
}
