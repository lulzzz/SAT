using SAT_CL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace SAT.WebHandlers
{
    /// <summary>
    /// Descripción breve de AutoCompleteUI
    /// </summary>
    public class FileUploader : IHttpHandler
    {
        /// <summary>
        /// Método encargado de Recibir la Petición Web
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //Obteniendo Termino a Filtrar
            string term = context.Request["term"] ?? "";

            //Obteniendo Tipo de Completacion
            string tipo = context.Request.QueryString["type"];

            //Validando que existan Archivos
            if (context.Request.Files.Count > 0)
            {
                //Obteniendo Archivos
                HttpFileCollection files = context.Request.Files;

                //Iniciando Ciclo de Archivos
                for (int i = 0; i < files.Count; i++)
                {
                    //Obteniendo Archivo
                    HttpPostedFile file = files[i];

                    //Leyendo el Archivo
                    byte[] fileArray = new byte[file.ContentLength];
                    file.InputStream.Read(fileArray, 0, file.ContentLength);

                    //Invocando Método de Obtención de Archivo
                    CargaArchivoObtenido(tipo, fileArray, context, file.FileName);
                }
            } 
        }
        /// <summary>
        /// Método encargado de Cargar el Archivo Obtenido
        /// </summary>
        /// <param name="tipo">Tipo de Archivo</param>
        /// <param name="archivo"></param>
        public static void CargaArchivoObtenido(string tipo, byte[] archivo, HttpContext context, string nombre_archivo)
        {
            //Validando Tipo
            switch (tipo)
            {
                case "XML":
                    {
                        //Obteniendo Archivo Codificado en UTF8
                        UTF8Encoding utf8 = new UTF8Encoding();
                        
                        //Declarando Documento XML
                        XmlDocument doc = new XmlDocument();

                        try
                        {
                            //Obteniendo XML en cadena
                            using (MemoryStream ms = new MemoryStream(archivo))

                                //Cargando Documento XML
                                doc.Load(ms);

                            //Guardando en sesión el objeto creado
                            System.Web.HttpContext.Current.Session["XML"] = doc;
                            System.Web.HttpContext.Current.Session["XMLFileName"] = nombre_archivo;

                            //Devolviendo Respuesta
                            context.Response.ContentType = "text/plain";
                            context.Response.Write("File Uploaded Successfully!");
                        }
                        catch(Exception e)
                        {
                            //Devolviendo Respuesta
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(e.Message);
                        }

                        
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Obtener las Coincidencias del Catalogo
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static List<string> autoCompleta(string prefix, int contextKey, string complement1, string complement2, string complement3)
        {
            //Declarando Objeto de Retorno
            List<string> optionList = new List<string>();

            //Declarando Objeto de Parametro
            object[] param = { contextKey, prefix, complement1, complement2, complement3 };

            //Ejecutando 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cargaConsultaSugerencia", param))
            {
                //Validando que exista una tabla
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        //Añadiendo Filas a la lista
                        optionList.Add(dr["descripcion"].ToString());
                }
            }

            //Devolviendo Lista
            return optionList;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}