using SAT_CL.Global;
using System;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_WCF
{
    /// <summary>
    /// Servicio encargado de todas las Funciones y/ó Operaciones que requiera la Aplicación Móvil
    /// </summary>
    public class HerramientasMovil : IHerramientasMovil
    {
        /// <summary>
        /// Obtiene el catálogo solicitado a partir de su id y los criterios deseados
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string CargaCatalogo(int id_catalogo, string opcion_inicial)
        {
            //Invocando método base
            return CargaCatalogoParametros(id_catalogo, opcion_inicial, 0, "", 0, "");
        }
        /// <summary>
        /// Obtiene el catálogo solicitado a partir de su id y los criterios deseados
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <param name="param1">Parámetro 1</param>
        /// <param name="param2">Parámetro 2</param>
        /// <param name="param3">Parámetro 3</param>
        /// <param name="param4">Parámetro 4</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string CargaCatalogoParametros(int id_catalogo, string opcion_inicial, int param1, string param2, int param3, string param4)
        {
            //Declarando objeto de retorno
            string resultado = "";

            //Validando que el parámetro id_catalogo se haya especificado, para evitar consultas no necesarias
            if (id_catalogo > 0)
            {
                //Consultando catálogo solicitado
                using (DataTable mit = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(id_catalogo, opcion_inicial, param1, param2, param3, param4))
                {
                    //Validando que existan registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de obtener el catálogo clasificado como general, solicitado a partir de su id
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string CargaCatalogoGeneral(int id_catalogo, string opcion_inicial)
        {
            //Declarando objeto de retorno
            string resultado = "";

            //Validando que el parámetro id_catalogo se haya especificado, para evitar consultas no necesarias
            if (id_catalogo > 0)
            {
                //Consultando catálogo solicitado
                using (DataTable mit = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(1, opcion_inicial, id_catalogo, 0, 0, 0, "", 0, ""))
                {
                    //Validando que existan registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene el catálogo clasificado como general, solicitado a partir de su id y un valor de un catálogo superior
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <param name="valor_superior">Valor del catálogo superior</param>
        /// <returns></returns>
        public string CargaCatalogoGeneralParametros(int id_catalogo, string opcion_inicial, int valor_superior)
        {
            //Declarando objeto de retorno
            string resultado = "";

            //Validando que el parámetro id_catalogo se haya especificado, para evitar consultas no necesarias
            if (id_catalogo > 0)
            {
                //Consultando catálogo solicitado
                using (DataTable mit = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(2, opcion_inicial, id_catalogo, 0, valor_superior, 0, "", 0, ""))
                {
                    //Validando que existan registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Cargar las Posibles Sugerencias
        /// </summary>
        /// <param name="contextKey">Indica el Catalogo a Obtener</param>
        /// <param name="prefix">Contiene el Indicio de las Opciones a Elegir</param>
        /// <param name="complement1">Indica el Primer complemento en caso de requerirse</param>
        /// <param name="complement2">Indica el Segundo complemento en caso de requerirse</param>
        /// <param name="complement3">Indica el Tercer complemento en caso de requerirse</param>
        /// <returns></returns>
        public string CargaConsultaSugerencia(int contextKey, string prefix, string complement1, string complement2, string complement3)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Declarando Tabla de Sugerencias
            using (DataTable dtSugerencias = new DataTable("Table"))
            {
                //Creando Columnas
                dtSugerencias.Columns.Add("id", typeof(int));
                dtSugerencias.Columns.Add("descripcion", typeof(string));

                //Declarando Arreglo de Parametros
                object[] param = { contextKey, prefix, complement1, complement2, complement3 };

                //Ejecutando Consulta
                using (DataSet ds = SAT_CL.CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cargaConsultaSugerencia", param))
                {
                    //Validando que exista una tabla
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo cada fila
                        foreach (DataRow dr in ds.Tables["Table"].Rows)

                            //Añadiendo el Registro a la Tabla
                            dtSugerencias.Rows.Add(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(dr["descripcion"].ToString(), "ID:", 1)),
                                                    TSDK.Base.Cadena.RegresaCadenaSeparada(dr["descripcion"].ToString(), "ID:", 0));

                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            dtSugerencias.WriteXml(s);

                            //Obteniendo DataSet en XML
                            XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                            //Añadiendo Elemento al nodo de Publicaciones
                            document.Add(dataTableElement);
                        }
                    }
                }
            }

            //Devolviendo resultado
            return document.ToString();
        }
        /// <summary>
        /// Método Público encaragdo de Guardar los Archivos de cada Registro
        /// </summary>
        /// <param name="id_registro">Parametro que hace Referencia al Registro</param>
        /// <param name="id_tabla">Parametro que hace Referencia a la Tabla</param>
        /// <param name="id_compania">Compañia Emisora</param>
        /// <param name="id_archivo_tipo_configuracion">Tipo de Configuración del Archivo</param>
        /// <param name="referencia">Referencia del Archivo (Nombre)</param>
        /// <param name="archivo">Archivo en Formato Base 64</param>
        /// <param name="id_sesion">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string GuardaArchivoRegistro(int id_registro, int id_tabla, int id_compania, int id_archivo_tipo_configuracion, string referencia, string archivo, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = null;

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
            {
                //Validando que la Sesión este Activa
                if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                {
                    //Instanciando el Tipo de Configuración del Archivo
                    using (ArchivoTipoConfiguracion atc = new ArchivoTipoConfiguracion(id_archivo_tipo_configuracion))
                    {
                        //Validando si existe la Configuración
                        if (atc.id_archivo_tipo_configuracion > 0)
                        {
                            //Declarando Arreglo de Bytes
                            byte[] fileArray = Convert.FromBase64String(archivo);

                            //Validando que existan
                            if (fileArray != null)
                            {
                                //Instanciando Tipo del Archivo
                                using (ArchivoTipo at = new ArchivoTipo(atc.id_archivo_tipo))
                                {
                                    //Validando que exista el Tipo
                                    if (at.id_archivo_tipo > 0)
                                    {
                                        //Declaramos variable para almacenar ruta
                                        string ruta = "";
                                        //Armando ruta de guardado físico de archivo
                                        //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                        ruta = string.Format(@"{0}{1}\{2}-{3}{4}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), id_tabla.ToString("0000"), id_registro.ToString("0000000"), DateTime.Now.ToString("yyMMddHHmmss"), at.extension);

                                        try
                                        {
                                            //Insertamos Registro
                                            resultado = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(id_tabla, id_registro, atc.id_archivo_tipo_configuracion, referencia.ToUpper(), user_sesion.id_usuario, fileArray, ruta);
                                            using (EventLog eventLog = new EventLog("Application"))
                                            {
                                                eventLog.Source = "Application";
                                                eventLog.WriteEntry(string.Format("{0} - {1}", resultado.Mensaje, ruta), EventLogEntryType.Information, 666, 1);
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            //Instanciando Excepción
                                            resultado = new RetornoOperacion(e.Message);
                                            using (EventLog eventLog = new EventLog("Application"))
                                            {
                                                eventLog.Source = "Application";
                                                eventLog.WriteEntry(string.Format("{0} - {1}", resultado.Mensaje, ruta), EventLogEntryType.Information, 666, 1);
                                            }
                                            
                                        }
                                    }
                                    else
                                        //Instanciando Excepcion
                                        resultado = new RetornoOperacion("No se encontro el tipo del Archivo");
                                }
                            }
                            else
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion("No existe el Archivo");
                        }
                        else
                            //Instanciando Excepcion
                            resultado = new RetornoOperacion("No se encontraron datos complementarios Detalle Archivo");
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("La Sesión no esta Activa");
            }

            //Devolviendo resultado Obtenido
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Actualizar el Archivo.
        /// </summary>
        /// <param name="id_archivo_registro">Parametro que hace referencia al Registro del Archivo</param>
        /// <param name="referencia">Referencia del Archivo (Nombre)</param>
        /// <param name="archivo">Archivo en Formato Base 64</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns>Retorno Operación en Formato XML</returns>
        public string ActualizaArchivoRegistro(int id_archivo_registro, string referencia, string archivo, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = null;

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
            {
                //Validando que la Sesión este Activa
                if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                {
                    //Instanciando Archivo Registro
                    using (ArchivoRegistro archivoRegistro = new ArchivoRegistro(id_archivo_registro))
                    {
                        //Validando que exista el Archivo
                        if (archivoRegistro.habilitar)
                        {
                            //Instanciando Referencias
                            using (ArchivoTipoConfiguracion atc = new ArchivoTipoConfiguracion(archivoRegistro.id_archivo_tipo_configuracion))
                            using (ArchivoTipo at = new ArchivoTipo(atc.id_archivo_tipo))
                            {
                                //Declarando Arreglo de Bytes
                                byte[] fileArray = Convert.FromBase64String(archivo);

                                //Validando que existan
                                if (fileArray != null)
                                {
                                    //Declaramos variable para almacenar ruta
                                    string ruta = "";

                                    //Armando ruta de guardado físico de archivo
                                    //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                    ruta = string.Format(@"{0}{1}\{2}-{3}{4}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), archivoRegistro.id_tabla.ToString("0000"), archivoRegistro.id_registro.ToString("0000000"), Fecha.ObtieneFechaEstandarMexicoCentro().ToString("yyMMddHHmmss"), at.extension);

                                    try
                                    {
                                        //Actualizando Archivo
                                        resultado = archivoRegistro.ActualizaArchivoRegistro(ruta, referencia.ToUpper(), user_sesion.id_usuario, fileArray);
                                        using (EventLog eventLog = new EventLog("Application"))
                                        {
                                            eventLog.Source = "Application";
                                            eventLog.WriteEntry(string.Format("{0} - {1}", resultado.Mensaje, ruta), EventLogEntryType.Information, 666, 1);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion(e.Message);
                                        using (EventLog eventLog = new EventLog("Application"))
                                        {
                                            eventLog.Source = "Application";
                                            eventLog.WriteEntry(string.Format("{0} - {1}", resultado.Mensaje, ruta), EventLogEntryType.Information, 666, 1);
                                        }
                                    }
                                }
                                else
                                    //Instanciando Excepcion
                                    resultado = new RetornoOperacion("No existe el Archivo");
                            }
                        }
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No existe el Registro del Archivo");
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("La Sesión no esta Activa");
            }

            //Devolviendo resultado Obtenido
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Eliminar el Archivo Registro
        /// </summary>
        /// <param name="id_archivo_registro">Registro del Archivo</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string EliminaArchivoRegistro(int id_archivo_registro, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = null;

            //Instanciando la Sesión de Usuario
            using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
            {
                //Validando que la Sesión este Activa
                if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                {
                    //Instanciando Archivo Registro
                    using (ArchivoRegistro archivoRegistro = new ArchivoRegistro(id_archivo_registro))
                    {
                        //Validando que exista el Archivo
                        if (archivoRegistro.habilitar)
                        {
                            try
                            {
                                //Deshabilitando Registro
                                resultado = archivoRegistro.DeshabilitaArchivoRegistro(user_sesion.id_usuario);
                                using (EventLog eventLog = new EventLog("Application"))
                                {
                                    eventLog.Source = "Application";
                                    eventLog.WriteEntry(string.Format("{1} - {0}", resultado.Mensaje, resultado.OperacionExitosa), EventLogEntryType.Information, 666, 1);
                                }
                            }
                            catch (Exception ex)
                            {
                                resultado = new RetornoOperacion(ex.Message);
                                using (EventLog eventLog = new EventLog("Application"))
                                {
                                    eventLog.Source = "Application";
                                    eventLog.WriteEntry(string.Format("{1} - {0}", resultado.Mensaje, resultado.OperacionExitosa), EventLogEntryType.Information, 666, 1);
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No existe el Registro del Archivo");
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("La Sesión no esta Activa");
            }

            //Devolviendo resultado Obtenido
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Obtener los Registros de Archivos Registro
        /// </summary>
        /// <param name="id_tabla">Tabla</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Tipo de Configuración de Archivo</param>
        /// <returns></returns>
        public string CargaArchivosRegistro(string id_tabla, string id_registro, string id_archivo_tipo_configuracion)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("ArchivosRegistro"));

            //Obteniendo Archivos
            using (DataTable dtArchivosRegistro = SAT_CL.Global.ArchivoRegistro.ObtieneArchivoRegistro(Convert.ToInt32(id_tabla),
                                                    Convert.ToInt32(id_registro), Convert.ToInt32(id_archivo_tipo_configuracion)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtArchivosRegistro))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtArchivosRegistro.WriteXml(s, XmlWriteMode.WriteSchema);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document.Element("ArchivosRegistro").Add(dataTableElement);
                    }
                }
                else
                    //Añadiendo Resultado por Defecto
                    document.Element("ArchivosRegistro").Add(new XElement("NewDataSet"));
            }

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }
        /// <summary>
        /// Método encargado de Obtener el Archivo en Formato Base64 de un Registro
        /// </summary>
        /// <param name="id_archivo_registro">Registro del Archivo</param>
        /// <returns>Archivo en Formato XML</returns>
        public string ObtieneArchivoRegistroBase64(int id_archivo_registro)
        {
            //Declarando Objeto de Retorno
            XDocument document = new XDocument();

            //Añadiendo Nodo Principal
            document.Add(new XElement("ArchivoRegistro"));

            //Declarando Elemento por Añadir
            XElement archivoBase64;
            XElement extension;
            XElement nombreArchivo;

            //Declarando Variables Auxiliares
            byte[] archivoBytes = null;
            string base64 = "", ext = "", name = "";

            //Instanciando Archivo Registro
            using (ArchivoRegistro archivo_registro = new ArchivoRegistro(id_archivo_registro))
            {
                //Validando que exista el Archivo
                if (archivo_registro.habilitar)
                {
                    //Instanciando Archivo Tipo Configuración
                    using (ArchivoTipoConfiguracion atc = new ArchivoTipoConfiguracion(archivo_registro.id_archivo_tipo_configuracion))
                    {
                        //Validando que exista la Configuración del Tipo de Archivo
                        if (atc.habilitar)
                        {
                            //Instanciando Tipo de Archivo
                            using (ArchivoTipo at = new ArchivoTipo(atc.id_archivo_tipo))
                            {
                                //Validando que exista el Tipo de Archivo
                                if (at.habilitar)
                                {
                                    try
                                    {
                                        //Obteniendo Arreglo de Bytes de la Ruta
                                        archivoBytes = System.IO.File.ReadAllBytes(archivo_registro.url);

                                        //Convirtiendo Archivo a Base 64
                                        base64 = Convert.ToBase64String(archivoBytes);
                                    }
                                    catch { }

                                    //Asignando Valores
                                    ext = at.extension;
                                    name = archivo_registro.referencia;
                                }
                            }
                        }
                    }
                }

                //Inicializando Elemento
                extension = new XElement("Extension", ext);
                nombreArchivo = new XElement("NombreArchivo", name);
                archivoBase64 = new XElement("StreamBase64", base64);

            }

            //Añadiendo a Nodo Principal
            document.Element("ArchivoRegistro").Add(extension);
            document.Element("ArchivoRegistro").Add(nombreArchivo);
            document.Element("ArchivoRegistro").Add(archivoBase64);

            //Devolviendo Resultado Obtenido
            return document.ToString();
        }

    }
}
