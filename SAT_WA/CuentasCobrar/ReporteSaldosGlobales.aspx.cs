using SAT_CL.CXC;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;

namespace SAT.CuentasCobrar
{
    public partial class ReporteSaldosGlobales : System.Web.UI.Page 
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se produjo un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaSaldosGlobales();
        }

        #region Eventos GridView "Saldos Globales"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Saldos Globales"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvSaldosGlobales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));

            //Invocando Método de Suma
            sumaTotalesGlobales();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Saldos Globales"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Saldos Globales"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSaldosGlobales_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvSaldosGlobales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

            //Invocando Método de Suma
            sumaTotalesGlobales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Saldos Globales"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSaldosGlobales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvSaldosGlobales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);

            //Invocando Método de Suma
            sumaTotalesGlobales();
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Mostrando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Inicializando GridView
            Controles.InicializaGridview(gvSaldosGlobales);

            //Invocando Método de Carga
            cargaCatalogos();

            //Poner el cursor en el primer control
            txtFecIni.Focus();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
        }
        /// <summary>
        /// Método encargado de Buscar los Saldos Globales
        /// </summary>
        private void buscaSaldosGlobales()
        {
            //Obteniendo Fechas
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;
            
            //Validando que se Incluyan las Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }
            
            //Obteniendo Reporte de Saldos Globales
            using(DataTable dtSaldosGlobales = Reporte.ObtieneReporteSaldoGlobal(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), fec_ini, fec_fin, Convert.ToByte(chkFacturacinE.Checked)))
            {
                //Validando que existan Registros
                if(Validacion.ValidaOrigenDatos(dtSaldosGlobales))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvSaldosGlobales, dtSaldosGlobales, "Cliente", "", true, 0);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSaldosGlobales, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvSaldosGlobales);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Invocando Método de Suma
                sumaTotalesGlobales();
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales en el Pie de GridView
        /// </summary>
        private void sumaTotalesGlobales()
        {
            //Validando que existan Registros
            if(TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Sumando Totales
                gvSaldosGlobales.FooterRow.Cells[1].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalSinProceso)", "")));
                gvSaldosGlobales.FooterRow.Cells[2].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalProcesoActual)", "")));
                gvSaldosGlobales.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalProcesoTerminado)", "")));
                gvSaldosGlobales.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalFE)", "")));
                gvSaldosGlobales.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalFacturado)", "")));
                gvSaldosGlobales.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalPagosAplicados)", "")));
                gvSaldosGlobales.FooterRow.Cells[7].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoActual)", "")));
            }
            else
            {
                //Sumando Totales
                gvSaldosGlobales.FooterRow.Cells[1].Text = 
                gvSaldosGlobales.FooterRow.Cells[2].Text = 
                gvSaldosGlobales.FooterRow.Cells[3].Text = 
                gvSaldosGlobales.FooterRow.Cells[4].Text = 
                gvSaldosGlobales.FooterRow.Cells[5].Text = 
                gvSaldosGlobales.FooterRow.Cells[6].Text = 
                gvSaldosGlobales.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
            }
        }

        #endregion
    }
}