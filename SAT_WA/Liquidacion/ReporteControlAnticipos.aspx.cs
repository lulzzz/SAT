using SAT_CL.Liquidacion;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Liquidacion
{
    public partial class ReporteControlAnticipos : System.Web.UI.Page
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            //Validando Datos del Cliente
            int idCliente = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1));
            if (idCliente > 0)
            {
                //Cargando Maestros
                using (DataTable dtMaestros = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(191, "Todos", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", idCliente, ""))
                {
                    if (Validacion.ValidaOrigenDatos(dtMaestros))
                    {
                        Controles.CargaDropDownList(ddlViajeMaestro, dtMaestros, "id", "descripcion");
                    }
                    else
                        Controles.InicializaDropDownList(ddlViajeMaestro, "El Cliente no posee Viajes Maestros");
                }
            }
            else
                Controles.InicializaDropDownList(ddlViajeMaestro, "El Cliente no posee Viajes Maestros");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaControlAnticipos();
        }
        /// <summary>
        /// Evento Producido al Exportar un Reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch (lnk.CommandName)
            {
                case "ControlAnticipos":
                    //Exportando Contenido
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
                    break;
            }
        }

        #region Eventos GridView "Saldos Detalle"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvControlAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);

            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos al GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvControlAnticipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Fila Enlazada
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Origen de la Fila
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                //Obteniendo Versión
                string version = rowView["VersionCFDI"].ToString().Equals("") ? "0" : rowView["VersionCFDI"].ToString();

                //Validando Versión
                if (version.Equals("3.3"))

                    //Asignando Color de Fuente
                    e.Row.Cells[24].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0404B4");
                else if (version.Equals("3.2"))

                    //Asignando Color de Fuente
                    e.Row.Cells[24].ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF760A");
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvControlAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvControlAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);

            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvControlAnticipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvControlAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);

            //Invocando Método de Suma
            sumaTotales();
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
            Controles.InicializaGridview(gvControlAnticipos);

            //Invocando Método de Carga
            cargaCatalogos();

            //Poner el cursor en el primer control
            txtCliente.Focus();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 3182);
            Controles.InicializaDropDownList(ddlViajeMaestro, "El Cliente no posee Viajes Maestros");
        }
        /// <summary>
        /// Método encargado de Buscar los Saldos Detalle
        /// </summary>
        private void buscaControlAnticipos()
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

            //Obteniendo Fechas
            DateTime fec_iniD = DateTime.MinValue, fec_finD = DateTime.MinValue;

            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtControlAnticipos = Reportes.ObtieneReporteControlAnticipos(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(ddlViajeMaestro.SelectedValue), fec_ini, fec_fin, 
                                                  Convert.ToString(Cadena.RegresaElementoNoVacio(txtNoServicio.Text, "")), Convert.ToString(Cadena.RegresaElementoNoVacio(txtSerie.Text, "")), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                                  Convert.ToInt32(Cadena.RegresaElementoNoVacio(txtFolio.Text, "0")), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtClienteP.Text, "ID:", 1)), Convert.ToString(Cadena.RegresaElementoNoVacio(txtFolioP.Text, "0")), 
                                                  Convert.ToString(Cadena.RegresaElementoNoVacio(txtSerieP.Text, "0")), Cadena.RegresaElementoNoVacio(txtUUID.Text, "")))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtControlAnticipos))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvControlAnticipos, dtControlAnticipos, "Id", "", true, 1);
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtControlAnticipos, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvControlAnticipos);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }

            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Método encargado de Sumar los Totales
        /// </summary>
        private void sumaTotales()
        {
            //Validando que exista la Suma
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvControlAnticipos.FooterRow.Cells[8].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SubTotal)", "")));
                gvControlAnticipos.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Trasladado)", "")));
                gvControlAnticipos.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Retenido)", "")));
                gvControlAnticipos.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
                gvControlAnticipos.FooterRow.Cells[16].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalCFDI)", "")));
                gvControlAnticipos.FooterRow.Cells[18].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalAnticiposCC)", "")));
                gvControlAnticipos.FooterRow.Cells[19].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalAnticiposSC)", "")));
                gvControlAnticipos.FooterRow.Cells[20].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalAnticiposRes)", "")));
                gvControlAnticipos.FooterRow.Cells[24].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalFiniquitos)", "")));
            }
            else
            {
                //Mostrando Totales
                gvControlAnticipos.FooterRow.Cells[8].Text =
                gvControlAnticipos.FooterRow.Cells[9].Text =
                gvControlAnticipos.FooterRow.Cells[10].Text =
                gvControlAnticipos.FooterRow.Cells[11].Text =
                gvControlAnticipos.FooterRow.Cells[16].Text =
                gvControlAnticipos.FooterRow.Cells[18].Text =
                gvControlAnticipos.FooterRow.Cells[19].Text =
                gvControlAnticipos.FooterRow.Cells[20].Text =
                gvControlAnticipos.FooterRow.Cells[24].Text = string.Format("{0:C2}", 0);
            }
        }
        #endregion
    }
}