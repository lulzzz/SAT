using SAT_CL.FacturacionElectronica;
using System;
using TSDK.Base;

namespace SAT.FacturacionElectronica
{
    public partial class DescargaZip : System.Web.UI.Page
    {
        /// <summary>
        /// Evento ejecutado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Verifica si se produce un Postback
            if (!Page.IsPostBack)
            {
                inicializaForma();
            }
        }
        /// <summary>
        /// Método encargado de Descarga el Archivo
        /// </summary>
        private void inicializaForma()
        {   //Instanciando Link de Descarga
            using (LinkDescarga link = new LinkDescarga(Convert.ToInt32(Request.QueryString["id"])))
            {   //Decrementa 1 a las descargas restantes
                link.EditaLinkDescarga(link.idContacto, link.fechaGeneracion, link.fechaCaducidad, link.descargasRestantes - 1, link.requierePDF, link.id_usuario);
                //Generando el Script de cierre de ventana
                string script_ventana = "<script language= \"javascript\" type=\"text/javascript\">window.close(); window.opener.focus();</script>";
                //Si no se ha registrado el script en la ventana de apretura
                if (!Page.ClientScript.IsStartupScriptRegistered("CierraVentana"))
                    //Registrando nuevo script
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CierraVentana", script_ventana);
                //Descargando Archivo de Bytes
                Archivo.DescargaArchivo((byte[])Session["ZIP"], "FacturasCFDI.zip", Archivo.ContentType.binary_octetStream);
            }
        }
    }
}