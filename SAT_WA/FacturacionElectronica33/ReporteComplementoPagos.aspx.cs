using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.FacturacionElectronica33
{
    public partial class ReporteComplementoPagos : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que existe un PostBack
            if (!Page.IsPostBack)
                //Invocando Método de Inicialización
                inicializaPagina();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de cargar la pagina
        /// </summary>
        private void inicializaPagina()
        {
            //Invocando Método de Carga
            cargaCatalogos();
            Controles.InicializaGridview(gvComprobantes);
            txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Método de cargar los catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPago, 185, "-- Selecciona una opción", 0, "", 0, "");
        }
        #endregion

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdU");
        }

        protected void gvComprobantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando criterio de visuzalización
            Controles.CambiaIndicePaginaGridView(gvComprobantes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }

        protected void gvComprobantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Tipo de Registro
                string tipo_registro = ((DataRowView)e.Row.DataItem).Row.ItemArray[0].ToString();

                //Validando el Tipo de Registro
                switch (tipo_registro)
                {
                    //Servicio
                    case "1":
                        {
                            string estatus = ((DataRowView) e.Row.DataItem).Row.ItemArray[6].ToString();
                            if(estatus != "Sustituido")
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_servicio";
                            else
                            {//Rojo
                                e.Row.CssClass = "liquidacion_servicio";
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#D75454");
                                e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                            }
                            break;
                        }
                    //Movimiento
                    case "2":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_movimiento";
                            break;
                        }
                        
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini = DateTime.MinValue;
            DateTime fec_fin = DateTime.MinValue;
            //Incluyendo Fechas
            //if (chkIncluir.Checked)
            //{
                //Obteniendo Fechas
                DateTime.TryParse(txtFechaInicio.Text, out fec_ini);
                DateTime.TryParse(txtFechaFin.Text, out fec_fin);
            //}
            using (DataTable dtComprobantes = SAT_CL.FacturacionElectronica33.Reporte.ObtieneComprobantes(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), txtSerie.Text, txtFolio.Text, txtFicha.Text, fec_ini, fec_fin, Convert.ToInt32(ddlFormaPago.SelectedValue), txtUUID.Text))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobantes))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvComprobantes, dtComprobantes, "SerieFolio", "", true, 1);
                    //Añadiendo a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtComprobantes, "Table");
                }
                else
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvComprobantes);
                    //Eliminando de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        //protected void chkIncluir_CheckedChanged(object sender, EventArgs e)
        //{
        //    //Habilitando Controles
        //    txtFechaInicio.Enabled =
        //    txtFechaFin.Enabled = chkIncluir.Checked;

        //    //Validando que no se Incluyan
        //    if (!chkIncluir.Checked)
        //    {
        //        //Asignando Fechas por Defecto
        //        txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
        //        txtFechaFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        //    }
        //}
    }
}