using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL;

namespace SAT.Accesorios
{
    public partial class ImpresionDocumentos : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando que se produjo un PostBack
            if (!(Page.IsPostBack))
                //Invocando Método de Inicialización
                inicializaPagina();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Obteniendo Valores
            int idCompania = Convert.ToInt32(Request.QueryString["idRegistro"]);
            int idUsuario = Convert.ToInt32(Request.QueryString["idUsuario"]);
            //Inicializando Control de Usuario
            ucImpresionDocumentos.InicializaControlUsuario(idCompania, idUsuario);
        }
        #endregion
    }
}