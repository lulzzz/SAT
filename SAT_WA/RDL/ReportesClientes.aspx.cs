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
        /// <summary>
        /// Token
        /// </summary>
        private string _token;
        /// <summary>
        /// Id Usuario
        /// </summary>
        private int _id_usuario;
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
                if (Request.QueryString["ustk"] != null)
                {
                    //Invoca al método carga los servicios a buscar
                    inicializaForma();
                    //Recupera 
                    recuperaAtributos();
                }

            }

        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Recuperando Atributos
            asignaAtributos();
        }

        /// <summary>
        /// Cambio en la selección del reporte a visualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Asigna Atributos
            asignaAtributos();
            //Instanciando
            using (SAT_CL.Seguridad.UsuarioToken objtoken = new SAT_CL.Seguridad.UsuarioToken(this._token))
            using (SAT_CL.Seguridad.Usuario objUsario = new SAT_CL.Seguridad.Usuario(objtoken.id_usuario_registra))
            using (SAT_CL.Global.CompaniaReporte objReporte = new SAT_CL.Global.CompaniaReporte(Convert.ToInt32(ddlReporte.SelectedValue)))
            {
                if (objtoken.habilitar && objUsario.habilitar && objReporte.habilitar)
                {
                    if (objReporte.habilitar)

                    {
                        //En base al tipo de reporte definido para el botón
                        switch (objReporte.id_tipo_reporte)
                        {
                            case 1:
                                {
                                    rvSSRS.Visible = true;
                                    rvPBI.Visible = false;
                                    rvAND.Visible = false;
                                    //Instanciando reporte seleccionado
                                    using (SAT_CL.Global.CompaniaReporte reporte = new SAT_CL.Global.CompaniaReporte(Convert.ToInt32(ddlReporte.SelectedValue)))
                                    {
                                        /*** CONFIGURANDO VISOR DE REPORTE ***/
                                        //Indicando el servidor al que será conectado el visor
                                        rvSSRS.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ServidorSSRS"]);
                                        //Credenciales de Autenticación en servidor de reportes
                                        rvSSRS.ServerReport.ReportServerCredentials = new SAT_CL.Seguridad.CredencialServidorReportes(ConfigurationManager.AppSettings["UsuarioSSRS"],
                                                                            ConfigurationManager.AppSettings["ContrasenaSSRS"], ConfigurationManager.AppSettings["DominioSSRS"]);
                                        //ASigna al reporte la variable compania
                                        rvSSRS.ServerReport.ReportPath = reporte.url_reporte_ssrs;
                                        ReportParameter param = new ReportParameter("param1", Convert.ToString(objtoken.id_compania));
                                        rvSSRS.ServerReport.SetParameters(new ReportParameter[] { param });
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    rvSSRS.Visible = false;
                                    rvPBI.Visible = false;
                                    rvAND.Visible = false;
                                    break;
                                }
                            case 3:
                                {
                                    rvSSRS.Visible = false;
                                    rvPBI.Visible = true;
                                    rvAND.Visible = false;

                                    break;
                                }
                            case 4:
                                {
                                    rvSSRS.Visible = false;
                                    rvPBI.Visible = false;
                                    rvAND.Visible = true;
                                    break;
                                }
                        }
                    }
                    else
                        Error();

                }
                else
                    Error();
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Inicializando contenido de GV
        /// </summary>
        private void inicializaForma()
        {
            //Recupera token 
            this._token = Convert.ToString(Request.QueryString["ustk"]);
            //Instanciando
            using (SAT_CL.Seguridad.UsuarioToken objtoken = new SAT_CL.Seguridad.UsuarioToken(this._token))
            using (SAT_CL.Seguridad.Usuario objUsario = new SAT_CL.Seguridad.Usuario(objtoken.id_usuario_registra))
            using (SAT_CL.Seguridad.PerfilSeguridadUsuario objperfilactivo = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilActivo(objUsario.id_usuario))
            {
                if (objtoken.habilitar && objUsario.habilitar && objperfilactivo.habilitar)
                {

                    //Cargando lista de reportes de la compañía
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlReporte, 198, "- Seleccione un Reporte -", objtoken.id_compania, Convert.ToString(objperfilactivo.id_perfil), 0, "");
                }
                else
                    Error();
            }
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Recupera token 
            this._token = Convert.ToString(Request.QueryString["ustk"]);
            //Asignando Atributos
            ViewState["_token"] = this._token;
            ViewState["_id_usuario"] = this._id_usuario;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            this._token = Convert.ToString(ViewState["_token"]);
            //Recuperando Atributos
            this._id_usuario = Convert.ToInt32(ViewState["_id_usuario"]);
        }

        private void Error()
        {
            //Obteniendo Acceso por Defecto
            string acceso = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Externa/Login.aspx", "~/Externa/TokenInvalido.aspx");
            if (!acceso.Equals(""))
            {
                //Redireccionando a forma  por Default
                Response.Redirect(acceso);
            }
        }
        #endregion
    }
}