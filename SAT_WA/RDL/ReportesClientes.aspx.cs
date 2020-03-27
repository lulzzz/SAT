using Microsoft.Reporting.WebForms;
using SAT_CL;
using SAT_CL.Seguridad;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using TSDK.ASP;
using TSDK.Base;
using System.Web.Security;

namespace SAT.Reportes
{
    public partial class ReportesClientes : System.Web.UI.Page
    {
        #region Atributos
        private int _id_compania = 76;
        #endregion

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
            {
                //Inicializa autenticaUsuario
                //autenticaUsuario("test123@tectos.com.mx", "123");
                //Inicializando contenido de página
                inicializaForma();

            }

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
                    //ASigna al reporte la variable compania
                    rvSSRS.ServerReport.ReportPath = reporte.url_reporte_ssrs;
                    ReportParameter param = new ReportParameter("param1", Convert.ToString(this._id_compania));
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
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlReporte, 70, "- Seleccione un Reporte -", this._id_compania, "", 0, "");
            
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