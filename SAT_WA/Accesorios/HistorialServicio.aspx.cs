using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.Accesorios
{
    public partial class HistorialServicio : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido el Generarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
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
            int idCompaniaEmisora = Convert.ToInt32(Request.QueryString["idRegistro"]);

            //Inicializando Control de Usuario
            wucHistorialViajes.InicializaHistorialViajes(idCompaniaEmisora);
        }

        #endregion
    }
}