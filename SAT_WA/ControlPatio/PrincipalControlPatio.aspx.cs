using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SAT_CL.ControlPatio;

namespace SAT.ControlPatio
{
    public partial class PrincipalControlPatio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            if( !Page.IsPostBack)
                //inicializamos la pagina
                inicializaPagina();

        }

        protected void lnkDireccionaModulo_Click(object sender, EventArgs e)
        {
            //Obtenemos el Nombre de Comando del LinkButton
            string destino = ((LinkButton)sender).CommandName;
            //Direccionamos a la pagina destino
            Response.Redirect(destino);
        }

        /// <summary>
        /// Evento disparado al seleccionar un nuevo patio 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {            
            inicializaIndicadoresUnidad();
        }

        /// <summary>
        /// Evento disparado al dar click en el boton actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            inicializaIndicadoresUnidad();
        }

        /// <summary>
        /// Inicializamos la pagina 
        /// </summary>
        public void inicializaPagina()
        {
            //Cargamos los catalogo existentes en la pagina
            cargaCatalogo();
            inicializaIndicadoresUnidad();
        }

        /// <summary>
        /// Metodo encargado de cargar los catalogos existentes en la pagina
        /// </summary>
        private void cargaCatalogo()
        {
            //Obteniendo Instancia de Clase
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
            {   //Cargando los Patios por Compania
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Ninguno", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", 0, "");
                //Asignando Patio por Defecto
                ddlPatio.SelectedValue = up.id_patio.ToString();                
            }
        }

        /// <summary>
        /// Inicializamo los indicadores relacionados con unidades dentro de patio
        /// </summary>
        public void inicializaIndicadoresUnidad()
        { 
            //Inicializamos los indicadores de unidad en la pagina
            using(DataTable t = SAT_CL.ControlPatio.DetalleAccesoPatio.retornaIndicadoresUnidades(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(t))
                    //Si el origen es valido recorremos la tabla 
                    foreach(DataRow r in t.Rows)
                    {
                        lblUnidades.Text = r["Unidades"].ToString();
                        lblTiempo.Text = r["Tiempo"].ToString();
                        lblEntradaSalida.Text = r["EntradasSalidas"].ToString();
                        
                    }
            }
        }

        

        
        
    }
}