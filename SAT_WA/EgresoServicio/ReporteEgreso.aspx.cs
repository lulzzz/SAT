using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.EgresoServicio
{
    public partial class ReporteEgreso : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento generado al cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando  la forma
                inicializaForma();
                //Asignamos Focus
                txtNoEgreso.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }
        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEgreso_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvEgreso, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);

            //Mostrando Totales
            sumaTotalesReporte();
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEgreso_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            Controles.CambiaTamañoPaginaGridView(gvEgreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewEgreso.SelectedValue), true, 3);

            //Mostrando Totales
            sumaTotalesReporte();
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Liquidacion a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelEgreso_Onclick(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdAccesorio", "IdTipoEvento");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEgreso_Onsorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblCriterioGridViewEgreso.Text = Controles.CambiaSortExpressionGridView(gvEgreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);

            //Mostrando Totales
            sumaTotalesReporte();
        }
        /// <summary>
        /// Evento producido al marcar o desmarcar el filtrado por fechas de  Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Si se marca la búsqueda por Fechas
            if (!(chkRangoFechas.Checked))
            {
                //Inicialozamos Controles de Fechas
                DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
                DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
                txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy");
                txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy");
            }

            //Habilitación de cajas de texto para fecha
            txtFechaInicio.Enabled = txtFechaFin.Enabled = chkRangoFechas.Checked;
        }
        /// <summary>
        /// Evento generado al buscar los Egresos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Carga las Egresos
            cargaEgresos();
        }
        /// <summary>
        /// Evento generado al dar click en la Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvEgreso.DataKeys.Count > 0)
            {
                //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvEgreso, sender, "lnk", false);
                //Inicializamos Bitacora
                inicializaBitacora(Convert.ToInt32(gvEgreso.SelectedValue), 101, "Egreso");
                //Carga grafica
                Controles.CargaGrafica(ChtEgreso, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                      "Concepto", "Total", true);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga controles
            cargaCatalogos();
            //Inicializa Controles
            inicializaControles();
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvEgreso);
        }
        /// <summary>
        /// 
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstatus, 59, "TODOS");
            //Cargando Catalogo Cuenta Origen 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuentaOrigen, 58, "TODOS", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Catalogo Concepto 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 57, "TODOS", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewEgreso, "", 26);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControles()
        {
            //Asignando Valores
            txtNoEgreso.Text = "";
            ddlCuentaOrigen.SelectedValue = "";
            txtBeneficiario.Text = "";
            ddlConcepto.SelectedValue = "";
            ddlEstatus.SelectedValue = "0";
            chkRangoFechas.Checked = false;
            DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
            DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
            txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy");
            txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// Configura y muestra ventana de bitácora de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla (Titulo de bitácora)</param>
        private void inicializaBitacora(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/EgresoServicio/ReporteEgreso.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de cargar los Egresos
        /// </summary>
        private void cargaEgresos()
        {
            //Declaramos variables de Fechas 
            DateTime fechaInicio = DateTime.MinValue, fechaFin = DateTime.MinValue;
            //De acuerdo al chek box de fechas de Liquidación
            if (chkRangoFechas.Checked)
            {
                //Declaramos variables de Fechas de Registró
                fechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                fechaFin = Convert.ToDateTime(txtFechaFin.Text);
            }

            //Obtenemos Depósito
            using (DataSet ds = SAT_CL.Bancos.Reporte.ReporteEgresos(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                   Convert.ToInt32(Cadena.VerificaCadenaVacia(txtNoEgreso.Text, "0")), Convert.ToInt32(ddlConcepto.SelectedValue), txtBeneficiario.Text,
                                                                   Convert.ToInt32(ddlCuentaOrigen.SelectedValue), Convert.ToByte(ddlEstatus.SelectedValue),
                                                                     fechaInicio, fechaFin))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table") && Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvEgreso, ds.Tables["Table"], "Id", "", true, 3);
                    Controles.CargaGridView(gvResumen, ds.Tables["Table1"], "Concepto", "", true, 1);
                    //Añadiendo Tablas al DataSet de Session 
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                    //Carga grafica
                    Controles.CargaGrafica(ChtEgreso, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                          "Concepto", "Total", true);
                    //Calculamos Total
                    gvResumen.FooterRow.Cells[1].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {
                    //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvEgreso);

                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }

                //Mostrando Totales
                sumaTotalesReporte();
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales del GridView
        /// </summary>
        private void sumaTotalesReporte()
        {
            //Validando que existe la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Calculamos Totales
                gvEgreso.FooterRow.Cells[8].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Monto)", "")));
                gvEgreso.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoNacional)", "")));
            }
            else
            {
                //Calculamos Totales
                gvEgreso.FooterRow.Cells[8].Text =
                gvEgreso.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
            }
        }

        #endregion
    }
}