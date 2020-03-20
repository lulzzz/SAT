using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace SAT.WebHandlers
{
    /// <summary>
    /// Descripción breve de VisorImagen
    /// </summary>
    public class VisorImagen : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //Si existe un arreglo de bytes en sesión
            if (context.Session["Dibujo"] != null)
            {
                try
                {
                    //Convirtiendo valor en arreglo de bytes
                    Byte[] bytes = (Byte[])context.Session["Dibujo"];
                    context.Response.Buffer = true;
                    context.Response.Charset = "";
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Limpiamos la respuesta
                    context.Response.Clear();
                    //Asignando tipo de imagen
                    context.Response.ContentType = "image/jpeg";
                    //Asignamos el encabezado
                    context.Response.AddHeader("content-disposition", "attachment;filename=" + "Imagen");

                    MemoryStream ms = new MemoryStream(bytes);
                    Bitmap bmp = (Bitmap)System.Drawing.Bitmap.FromStream(ms);
                    bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                    //Response.BinaryWrite(bytes);
                }
                //Si ocurre un error mostramos imagen no disponible
                catch
                {
                    //Mostrando imagen de no disponible
                    context.Response.Redirect("~/Image/no_disponible.jpg");
                }
                //Liberando recursos
                finally
                {
                    context.Response.Flush();
                    context.Response.End();
                }
            }
            //Si no existe sesión
            else
                //Mostrando imagen de no disponible
                context.Response.Redirect("~/Image/no_disponible.jpg");
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