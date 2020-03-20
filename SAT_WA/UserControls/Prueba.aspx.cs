using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Datos;
using TSDK.ASP;
using TSDK.Base;
using System.Linq;
using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using SAT_CL.Facturacion;
using SAT_CL.Documentacion;
using Microsoft.SqlServer.Types;
using System.Web.Hosting;

namespace SAT.UserControls
{
    public partial class Prueba : System.Web.UI.Page
    {

        /// <summary>
        /// Evento generado al cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!Page.IsPostBack)
            {
                wucPublicacionUnidad.InicializaControl(16976);
               
            }
                
            
           
        }






   

        
    }
}
