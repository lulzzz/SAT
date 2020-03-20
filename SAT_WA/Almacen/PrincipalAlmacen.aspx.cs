using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAT.Almacen
{
    public partial class PrincipalAlmacen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad a la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

        }
        protected void lnkDireccionaModulo_Click(object sender, EventArgs e)
        {
            //Obtiene el Nombre de Comando del LinkButton
            string destino = ((LinkButton)sender).CommandName;
            //Direccionamos a la página destino
            Response.Redirect(destino);

        }
    }
}