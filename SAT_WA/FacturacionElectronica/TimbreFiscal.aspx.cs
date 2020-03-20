using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAT_CL.FacturacionElectronica;

namespace SAT.FacturacionElectronica
{
    public partial class TimbreFiscal : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento gernerado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Si no es una Recargar de Pagina
            if (!Page.IsPostBack)
            {   //Carga la Pagina
                inicializaPagina();
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores de la Pagina
        /// </summary>
        private void inicializaPagina()
        {   //Validando si el QueryString contiene datos
            if (Request.QueryString["id_comprobante"] != null)
            {   //Convertir parametro a Entero
                int id_comprobante = Convert.ToInt32(Request.QueryString["id_comprobante"]);
                //Validando Id de Comprobante
                if (id_comprobante != 0)
                {   //Instancia de Objeto
                    using (SAT_CL.FacturacionElectronica.TimbreFiscalDigital tfd = SAT_CL.FacturacionElectronica.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(id_comprobante))
                    {   //Asignando Valores 
                        lblID.Text = tfd.id_comprobante.ToString();
                        txtUUID.Text = tfd.UUID;
                        txtVersion.Text = tfd.version;
                        txtSelloCFD.Text = tfd.sello_CFD;
                        txtSelloSAT.Text = tfd.sello_SAT;
                        txtNoCer.Text = tfd.no_certificado;
                        txtFecTim.Text = tfd.fecha_timbrado.ToString("yyyy/MM/dd HH:mm");
                        //Label de Error
                        lblError.Text = (id_comprobante != 0) ? "" : "Registro No Encontrado";
                    }
                }
            }
            else
                lblError.Text = "Registro No Encontrado";
        }

        #endregion

    }
}