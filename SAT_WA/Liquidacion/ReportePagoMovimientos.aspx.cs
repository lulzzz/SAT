using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Liquidacion
{
    public partial class ReportePagoMovimientos : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Carga de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando  PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando  la forma
                inicializaForma();
                //Asignamos Focus
                txtOperador.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }
        }
        /// <summary>
        /// Cambio de valor para uso de fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            habilitaFiltroFecha();
        }
        /// <summary>
        /// Cambio de página en gv de resumen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando indice de página
            Controles.CambiaIndicePaginaGridView(gvResumen, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 0);
            //Asignando totales al pie
            asignaTotalesPieResumen();
        }
        /// <summary>
        /// Click en botón exportar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelLiquidacion_Click(object sender, EventArgs e)
        {
            //Validando existencia de datos
            if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table", "Table1"))
            {
                //Obteniendo bytes de excel
                byte[] excel = Excel.BytesDataSetExcel((DataSet)Session["DS"], "Resumen", "Detalle");
                //Descargando el archivo resultante
                Archivo.DescargaArchivo(excel, "Excel.xls", Archivo.ContentType.ms_excel);
            }
        }
        /// <summary>
        /// Click en botón buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscaMovimientos();
        }
        /// <summary>
        /// Cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewLiquidacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño de página
            Controles.CambiaTamañoPaginaGridView(gvLiquidacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamañoGridViewLiquidacion.SelectedValue), true, 3);
            //Colocando totales al pie
            asignaTotalesPieDetalle();
        }
        /// <summary>
        /// Cambio de indice de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambio de indice de página
            Controles.CambiaIndicePaginaGridView(gvLiquidacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 3);
            //Colocando totales al pie
            asignaTotalesPieDetalle();
        }
        /// <summary>
        /// Cambio de criterio de orden de registros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_Onsorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando criterio de orden
            lblCriterioGridViewLiquidacion.Text = Controles.CambiaSortExpressionGridView(gvLiquidacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 3);
            //Colocando totales al pie
            asignaTotalesPieDetalle();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link de Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferencias_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvLiquidacion, sender, "lnk", false);

                //Validando que exista el Servicio
                if (Convert.ToInt32(gvLiquidacion.SelectedDataKey["IdServicio"]) != 0)
                {
                    //Inicializando Control de Referencias
                    ucReferenciasViaje.InicializaControl(Convert.ToInt32(gvLiquidacion.SelectedDataKey["IdServicio"]));

                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(gvLiquidacion, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
                }
            }
        }
        /// <summary>
        /// Evento Disparado al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarReferencias_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana Modal
            ScriptServer.AlternarVentana(lnkCerrarReferencias, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");

            //Inicializando Indices
            Controles.InicializaIndices(gvLiquidacion);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de los controles
        /// </summary>
        private void inicializaForma()
        {
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewLiquidacion, "", 26);
            //Habilitando controles de fecha
            habilitaFiltroFecha();
            //Inicializando GridView
            Controles.InicializaGridview(gvResumen);
            Controles.InicializaGridview(gvLiquidacion);
        }
        /// <summary>
        /// Habilita la selección de fecha de filtrado
        /// </summary>
        private void habilitaFiltroFecha()
        {
            //Aplicando habilitación de controles
            txtFechaFin.Enabled =
                    txtFechaInicio.Enabled = chkRangoFechas.Checked;

            //Estableciendo texto inicial
            DateTime inicio = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(-7);
            txtFechaInicio.Text = inicio.ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = inicio.AddDays(8).AddMinutes(-1).ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Realiza la búsqueda de registros
        /// </summary>
        private void buscaMovimientos()
        {
            //Declaramos variables de Fechas 
            DateTime fechaInicio = DateTime.MinValue, fechaFin = DateTime.MinValue;
            //De acuerdo al chek box de fechas de búsqueda
            if (chkRangoFechas.Checked)
            {
                //Declaramos variables de Fechas de Registró
                fechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                fechaFin = Convert.ToDateTime(txtFechaFin.Text);
            }

            //Obtenemos Depósito
            using (DataSet ds = SAT_CL.Liquidacion.Reportes.ReportePagoMovimientosOperador(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                   fechaInicio, fechaFin,Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, "ID:", 1))))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table", "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvResumen, ds.Tables["Table"], "Operador", "", true, 0);
                    Controles.CargaGridView(gvLiquidacion, ds.Tables["Table1"], "IdMovimiento-IdServicio", lblCriterioGridViewLiquidacion.Text, true, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                    //Colocando totales al pie
                    asignaTotalesPieResumen();
                    asignaTotalesPieDetalle();
                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvLiquidacion);

                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Realiza la sumatorias de columnas con cantidades monetarias y actualiza el resultado de cada una de ellas en la fila al pie de GV
        /// </summary>
        private void asignaTotalesPieResumen()
        {
            //Colocando totales de gv
            gvResumen.FooterRow.Cells[1].Text = string.Format("{0}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(NoServicios)", "")));
            gvResumen.FooterRow.Cells[2].Text = string.Format("{0}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(NoMovimientos)", "")));
            gvResumen.FooterRow.Cells[3].Text = string.Format("{0:F2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Litros)", "")));
            gvResumen.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Diesel)", "")));
            gvResumen.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Anticipos)", "")));
            gvResumen.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Comprobaciones)", "")));
            gvResumen.FooterRow.Cells[7].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Pagos)", "")));
        }
        /// <summary>
        /// Realiza la sumatorias de columnas con cantidades monetarias y actualiza el resultado de cada una de ellas en la fila al pie de GV
        /// </summary>
        private void asignaTotalesPieDetalle()
        {
            //Colocando totales de gv
            gvLiquidacion.FooterRow.Cells[6].Text = string.Format("{0}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Kms)", "")));
            gvLiquidacion.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")));
            gvLiquidacion.FooterRow.Cells[14].Text = string.Format("{0:F2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Litros)", "")));
            gvLiquidacion.FooterRow.Cells[15].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Diesel)", "")));
            gvLiquidacion.FooterRow.Cells[16].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Anticipos)", "")));
            gvLiquidacion.FooterRow.Cells[17].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Comprobaciones)", "")));
            gvLiquidacion.FooterRow.Cells[18].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Pagos)", "")));
        }

        #endregion
    }
}