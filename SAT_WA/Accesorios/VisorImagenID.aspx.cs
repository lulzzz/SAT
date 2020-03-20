using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Net;
using TSDK.Base;

namespace SAT.Accesorios
{
    public partial class VisorImagenID : System.Web.UI.Page
    {
        #region Enumeraciones

        /// <summary>
        /// Indica los posibles origenes desde donde puede ser cargada una imagen
        /// </summary>
        private enum tipo_carga
        {
            /// <summary>
            /// URL de la imagen
            /// </summary>
            url,
            /// <summary>
            /// Ruta física del archivo de imagen a cargar
            /// </summary>
            archivo
        }
        /// <summary>
        /// Tipos de escalas disponibles
        /// </summary>
        private enum tipo_escala
        {
            /// <summary>
            /// Sin escala (imagen al original)
            /// </summary>
            sin_escala,
            /// <summary>
            /// Porcentaje 0 a 100
            /// </summary>
            porcentual,
            /// <summary>
            /// Medidas alto y ancho
            /// </summary>
            pixcel
        }

        #endregion

        /// <summary>
        /// Evento disparado al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            creaImagen();
        }

        #region Métodos

        /// <summary>
        /// Valida y recupera los parámetros pasados por la URL
        /// </summary>
        /// <param name="t_carga">Tipo de carga de imagen</param>
        /// <param name="url">URL de imagen o ruta de archivo</param>
        /// <param name="t_escala">Tipo de escalado</param>
        /// <param name="porcentaje_escala">Porcentaje de escalado (t_escala=porcentual)</param>
        /// <param name="alto">Valor de Altura(t_escala=pixcel)</param>
        /// <param name="ancho">Valor de Anchura(t_escala=pixcel)</param>
        private bool recuperaVariablesQueryString(out tipo_carga t_carga, out string url, out tipo_escala t_escala, out int porcentaje_escala, out int alto, out int ancho)
        {
            //Inicializando valores de salida
            bool resultado = false;
            t_carga = tipo_carga.url;
            t_escala = tipo_escala.sin_escala;
            porcentaje_escala = 100;
            alto = ancho = 0;
            url = Server.MapPath("~/Image/noDisponible.jpg");

            //Validando existencia de variables principales de imagen
            if (Request.QueryString["t_carga"] != null &&
                Request.QueryString["url"] != null)
            {
                t_carga = Request.QueryString["t_carga"].ToString() == "url" ? tipo_carga.url : tipo_carga.archivo;
                if (Request.QueryString["url"].ToString() != "")
                    url = Request.QueryString["url"].ToString();

                //Verificando si se ha solicitado escalar la imagen
                if (Request.QueryString["t_escala"] != null)
                {
                    //Tipo de escala
                    switch (Request.QueryString["t_escala"].ToString())
                    {
                        case "porcentual":
                            //Validando porcentaje de escala
                            if (Request.QueryString["p_escala"] != null)
                            {
                                t_escala = tipo_escala.porcentual;
                                Int32.TryParse(Request.QueryString["p_escala"].ToString(), out porcentaje_escala);
                            }
                            break;
                        case "pixcel":
                            //Confitmando variables de altura y anchura
                            if (Request.QueryString["alto"] != null &&
                                Request.QueryString["ancho"] != null)
                            {
                                alto = Convert.ToInt32(Request.QueryString["alto"].ToString());
                                ancho = Convert.ToInt32(Request.QueryString["ancho"].ToString());
                                t_escala = tipo_escala.pixcel;
                            }
                            break;
                        default:
                            t_escala = tipo_escala.sin_escala;
                            porcentaje_escala = 100;
                            break;
                    }
                }

                //Si existe un nombre de archivo
                if (!string.IsNullOrEmpty(url))
                    resultado = true;
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Construye la imagen y la envía al flujo de respuesta web
        /// </summary>
        private void creaImagen()
        {
            //Validando que l.os parámetros requeridos hayan sido enviados a la forma
            tipo_carga t_carga = tipo_carga.url;
            string url = "";
            tipo_escala t_escala = tipo_escala.sin_escala;
            int porcentaje = 0, alto = 0, ancho = 0;

            if (recuperaVariablesQueryString(out t_carga, out url, out t_escala, out porcentaje, out alto, out ancho))
            {
                //Definiendo arreglo de bytes
                byte[] bytes = null;
                Image imagen_escala = new Bitmap(1, 1);

                //Determinando el tipo de carga de imagen a realizar
                switch (t_carga)
                {
                    case tipo_carga.url:
                        //Creando cliente web
                        var webClient = new WebClient();
                        //Descargando los datos de imagen
                        bytes = webClient.DownloadData(url);
                        //ALmacenando en flujo temporal
                        using (MemoryStream ms = new System.IO.MemoryStream(bytes))
                        {
                            imagen_escala = Image.FromStream(ms);
                        }

                        break;
                    case tipo_carga.archivo:
                        //Creando imagen original
                        using (Image img = new Bitmap(url))
                        {
                            //Realizando escala de imagen según se requiera
                            switch (t_escala)
                            {
                                case tipo_escala.sin_escala:
                                    imagen_escala = new Bitmap(img);
                                    break;
                                case tipo_escala.porcentual:
                                    imagen_escala = new Bitmap(img, new Size((img.Size.Width * porcentaje) / 100, (img.Size.Height * porcentaje) / 100));
                                    break;
                                case tipo_escala.pixcel:
                                    imagen_escala = new Bitmap(img, new Size(ancho, alto));
                                    break;
                            }
                        }
                        break;
                }

                //Creando flujo para contener imagen resultante
                using (MemoryStream ms = new System.IO.MemoryStream())
                {
                    imagen_escala.Save(ms, ImageFormat.Jpeg);
                    bytes = Flujo.ConvierteFlujoABytes(ms);
                }

                try
                {
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Asignando tipo de imagen
                    Response.ContentType = "jpeg";
                    Response.AddHeader("content-disposition", "attachment;filename=Imagen");
                    Response.BinaryWrite(bytes);
                }
                //Si ocurre un error mostramos imagen no disponible
                catch (Exception)
                {
                    //Mostrando imagen de no disponible
                    Response.Redirect("~/Image/noDisponible.jpg");
                }
                //Liberando recursos
                finally
                {
                    Response.Flush();
                    Response.End();
                }
            }
        }
        #endregion
    }
}