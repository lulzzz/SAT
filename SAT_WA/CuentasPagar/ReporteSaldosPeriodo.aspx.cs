using SAT_CL.CXP;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;

namespace SAT.CuentasPagar
{
    public partial class ReporteSaldosPeriodo : System.Web.UI.Page 
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
            buscaSaldosPeriodo();
        }

        #region Eventos GridView "Saldos Periodo"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Saldos Periodo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvSaldosPeriodo, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));

            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Sumando Totales
                gvSaldosPeriodo.FooterRow.Cells[1].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo15Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[2].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo30Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo45Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoMayor45)", "")));
                gvSaldosPeriodo.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoTotal)", "")));
            }
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Saldos Periodo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Saldos Periodo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSaldosPeriodo_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvSaldosPeriodo, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Sumando Totales
                gvSaldosPeriodo.FooterRow.Cells[1].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo15Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[2].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo30Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo45Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoMayor45)", "")));
                gvSaldosPeriodo.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoTotal)", "")));
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Saldos Periodo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSaldosPeriodo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvSaldosPeriodo, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);

            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Sumando Totales
                gvSaldosPeriodo.FooterRow.Cells[1].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo15Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[2].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo30Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo45Dias)", "")));
                gvSaldosPeriodo.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoMayor45)", "")));
                gvSaldosPeriodo.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoTotal)", "")));
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializando GridView
            Controles.InicializaGridview(gvSaldosPeriodo);

            //Invocando Método de Carga
            cargaCatalogos();
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
        /// Método encargado de Buscar los Saldos Periodo
        /// </summary>
        private void buscaSaldosPeriodo()
        {
            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtSaldosPeriodo = Reportes.ObtieneReporteSaldosPeriodo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1))))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtSaldosPeriodo))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvSaldosPeriodo, dtSaldosPeriodo, "Cliente", "", true, 0);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSaldosPeriodo, "Table");

                    //Sumando Totales
                    gvSaldosPeriodo.FooterRow.Cells[1].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo15Dias)", "")));
                    gvSaldosPeriodo.FooterRow.Cells[2].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo30Dias)", "")));
                    gvSaldosPeriodo.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Saldo45Dias)", "")));
                    gvSaldosPeriodo.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoMayor45)", "")));
                    gvSaldosPeriodo.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoTotal)", "")));
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvSaldosPeriodo);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");

                    //Sumando Totales
                    gvSaldosPeriodo.FooterRow.Cells[1].Text =
                    gvSaldosPeriodo.FooterRow.Cells[2].Text =
                    gvSaldosPeriodo.FooterRow.Cells[3].Text =
                    gvSaldosPeriodo.FooterRow.Cells[4].Text =
                    gvSaldosPeriodo.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
                }
            }
        }

        #endregion
    }
}