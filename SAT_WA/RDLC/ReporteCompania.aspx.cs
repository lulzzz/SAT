using Microsoft.Reporting.WebForms;
using SAT_CL;
using SAT_CL.Seguridad;
using System;
using System.Configuration;

namespace SAT.RDLC
{
    public partial class ReporteCompania : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es un postback
            if (!IsPostBack)
                //Inicializando contenido de página
                inicializaForma();
        }
        /// <summary>
        /// Cambio en la selección del reporte a visualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determinando si se debe mostrar algún reporte
            if (ddlReporte.SelectedValue != "0")
            {
                //Instanciando reporte seleccionado
                using (SAT_CL.Global.CompaniaReporte reporte = new SAT_CL.Global.CompaniaReporte(Convert.ToInt32(ddlReporte.SelectedValue)))
                {
                    //Crea Variable que alamcena el identificador de una compania
                    int idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
                    //ASigna al reporte la variable compania
                    rvSSRS.ServerReport.ReportPath = reporte.url_reporte_ssrs;
                    ReportParameter param = new ReportParameter("param1", Convert.ToString(idCompania));
                    rvSSRS.ServerReport.SetParameters(new ReportParameter[] { param });                    

                   // rvSSRS.ServerReport.ReportPath = string.Format("{0}&Param1={1}", reporte.url_reporte_ssrs, idCompania);
                }
            }
            else
                //Mostrando reporte en blanco
                rvSSRS.ServerReport.ReportPath = "";
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializando contenido de GV
        /// </summary>
        private void inicializaForma()
        {
            //Cargando lista de reportes de la compañía
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlReporte, 70, "- Seleccione un Reporte -", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

            /*** CONFIGURANDO VISOR DE REPORTE ***/
            //Indicando el servidor al que será conectado el visor
            rvSSRS.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ServidorSSRS"]);
            //Credenciales de Autenticación en servidor de reportes
            rvSSRS.ServerReport.ReportServerCredentials = new SAT_CL.Seguridad.CredencialServidorReportes(ConfigurationManager.AppSettings["UsuarioSSRS"],
                                                ConfigurationManager.AppSettings["ContrasenaSSRS"], ConfigurationManager.AppSettings["DominioSSRS"]);            
        }
                
        #endregion
        
    }
}