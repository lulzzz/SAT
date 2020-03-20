using System;
using System.Web.UI.WebControls;
namespace SAT.FacturacionElectronica33
{
    public partial class PrincipalFacturacionElectronica33 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicar seguridad a la página
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "Content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        }
        protected void lnkDireccionaModulo_Click(object sender, EventArgs e)
        {
            //Obtener el nombre del comando del LinkButton
            string destino = ((LinkButton)sender).CommandName;
            //Direccionar a la página destino
            Response.Redirect(destino);
        }
    }
}