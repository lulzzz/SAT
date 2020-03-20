using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Liquidacion
{
    public partial class ReporteLiquidacion : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento generado al cargar la Pagina
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
                txtNoLiquidacion.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }

        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);

            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvLiquidacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewLiquidacion.SelectedValue), true, 3);

            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Liquidacion a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelLiquidacion_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdAccesorio", "IdTipoEvento");

        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewLiquidacion.Text = Controles.CambiaSortExpressionGridView(gvLiquidacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);

            //Suma Totales
            sumaTotales();
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
                txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy") + " 00:01";
                txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";

            }
            //Habilitación de cajas de texto para fecha
            txtFechaInicio.Enabled = txtFechaFin.Enabled = chkRangoFechas.Checked;
        }

        /// <summary>
        /// Evento generado al buscar las Liquidaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Carga las Liquidaciones
            cargaLiquidaciones();
        }

        /// <summary>
        /// Evento generado al dar click en la Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvLiquidacion, sender, "lnk", false);
                //Inicializamos Bitacora
                inicializaBitacora(Convert.ToInt32(gvLiquidacion.SelectedDataKey["Id"]), 82, "Liquidación");

                //Carga grafica
                Controles.CargaGrafica(ChtLiquidaciones, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                      "Estatus", "Total", true);
               
            }
        }

        /// <summary>
        /// Evento producido al cambiar el tipo de asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoAsignacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializando GridViews
            Controles.InicializaGridview(gvResumen);
            Controles.InicializaGridview(gvLiquidacion);


            //Eliminando Tablas del DataSet de Session
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");

            //Limpia grafica
            ChtLiquidaciones.Series.Clear();
            //Limpiamos controles
            txtValor.Text = "";
            //Declaramos variable tipo de autocomplete
            string id = "";
            //Validamos Tipo de Asignación para mostrar el control correspondiente
            switch (ddlTipoAsignacion.SelectedValue)
            {
                //Unidad
                case "1":
                    //Asiganamos variable correspondientes
                    lblValor.Text = "Unidad";
                    id = "12";
                    //Inicializa DropDown List
                    Controles.InicializaDropDownList(ddlTipoOperador, "Ninguno");
                 break;
                //Operador
                case "2":
                    //Asiganamos variable correspondientes
                    lblValor.Text = "Operador";
                    id = "11";
                    //Cargando DropDownList
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOperador, "", 3146);
                    break;
                //Tercero
                case "3":
                    //Asiganamos variable correspondientes
                    lblValor.Text = "Tercero";
                    id = "17";
                    //Inicializa DropDown List
                    Controles.InicializaDropDownList(ddlTipoOperador, "Ninguno");
                    break;
            }

            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + txtValor.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=" + id + @" &param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upddlTipoAsignacion, upddlTipoAsignacion.GetType(), "AutocompleteRecursos", script, false);

            //Control Valor de acuerdo al Tipo de Asignación
            if(ddlTipoAsignacion.SelectedValue =="0")
            {
                //Deshabilitamos Control
                txtValor.Enabled = false;
            }
            else
            {
                //Habilitamos Control
                txtValor.Enabled = true;
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
            TSDK.ASP.Controles.InicializaGridview(gvLiquidacion);
        }
        /// <summary>
        /// 
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {

            //Cargando Catalogos Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "TODOS", 65);
            //Cargando Catalogo Tipo Asignación
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoAsignacion, "TODOS", 2127);
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewLiquidacion, "", 26);
            //Inicializa DropDown List
            Controles.InicializaDropDownList(ddlTipoOperador, "Ninguno");
        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControles()
        {
            txtNoLiquidacion.Text = "";
            ddlEstatus.SelectedValue = "0";
            txtValor.Text = "";
            ddlTipoAsignacion.SelectedValue = "0";
            txtValor.Enabled = false;
            chkRangoFechas.Checked = false;
            DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
            DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
            txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy") + " 00:01";
            txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/ReporteLiquidacion.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }

         /// <summary>
        /// Método encargado de cargar las Liquidaciones
        /// </summary>
        private void cargaLiquidaciones()
        {
            //Declaramos variables de Fechas 
            DateTime fechaInicio = DateTime.MinValue, fechaFin = DateTime.MinValue;
            int id_operador = 0, id_unidad = 0, id_tercero = 0;
            //De acuerdo al chek box de fechas de Liquidación
            if (chkRangoFechas.Checked)
            {
                //Declaramos variables de Fechas de Registró
                fechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                fechaFin = Convert.ToDateTime(txtFechaFin.Text);
            }

            //Validamos Tipo de Asignación
            switch (ddlTipoAsignacion.SelectedValue)
            {
                //Validamos Selección
                //Unidad
                case "1":
                    id_unidad = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtValor.Text, ":", 1, "0"));
                    break;
                //Operador
                case "2":
                    id_operador = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtValor.Text, ":", 1, "0"));
                    break;
                //Tercero
                case "3":
                    id_tercero = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtValor.Text, ":", 1, "0"));
                    break;

            }

            //Obtenemos Depósito
            using (DataSet ds = SAT_CL.Liquidacion.Reportes.ReporteLiquidaciones(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                   Convert.ToInt32(Cadena.VerificaCadenaVacia(txtNoLiquidacion.Text, "0")), id_unidad, id_operador, id_tercero, Convert.ToByte(ddlEstatus.SelectedValue),
                                                                     fechaInicio, fechaFin, Convert.ToByte(ddlTipoAsignacion.SelectedValue), Convert.ToByte(ddlTipoOperador.SelectedValue)))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table") && Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvLiquidacion, ds.Tables["Table"], "Id-NoLiquidacion", "", true, 3);
                    Controles.CargaGridView(gvResumen, ds.Tables["Table1"], "Estatus", "", true, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                    //Carga grafica
                    Controles.CargaGrafica(ChtLiquidaciones, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                          "Estatus", "Total", true);

                    gvResumen.FooterRow.Cells[2].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")).ToString();

                }
                else
                {   
                    //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvLiquidacion);

                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
                //Suma Totales
                sumaTotales();
            }

        }


        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaTotales()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvLiquidacion.FooterRow.Cells[7].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Diesel)", "")));
                gvLiquidacion.FooterRow.Cells[8].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SueldoFijo)", "")));
                gvLiquidacion.FooterRow.Cells[9].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(PagoViajes)", "")));
                gvLiquidacion.FooterRow.Cells[10].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(OtrosConceptos)", "")));
                gvLiquidacion.FooterRow.Cells[11].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Estadias)", "")));
                gvLiquidacion.FooterRow.Cells[12].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(BonoSemanal)", "")));
                gvLiquidacion.FooterRow.Cells[13].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SubsidioAlEmpleado)", "")));
                gvLiquidacion.FooterRow.Cells[14].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Comprobaciones)", "")));
                gvLiquidacion.FooterRow.Cells[15].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Depositos)", "")));
                gvLiquidacion.FooterRow.Cells[16].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(IMSS)", "")));
                gvLiquidacion.FooterRow.Cells[17].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(ISR)", "")));
                gvLiquidacion.FooterRow.Cells[18].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(INFONAVIT)", "")));
                gvLiquidacion.FooterRow.Cells[19].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Prestamo)", "")));
                gvLiquidacion.FooterRow.Cells[20].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Deducciones)", "")));
                gvLiquidacion.FooterRow.Cells[21].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")));
                gvLiquidacion.FooterRow.Cells[22].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(NominaBasica)", "")));
                gvLiquidacion.FooterRow.Cells[23].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(NominaComplementaria)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvLiquidacion.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[8].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[10].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[11].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[12].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[13].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[14].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[15].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[16].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[17].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[18].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[19].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[20].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[21].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[22].Text = string.Format("{0:C2}", 0);
                gvLiquidacion.FooterRow.Cells[23].Text = string.Format("{0:C2}", 0);
            }
        }
   #endregion



       

    }
}
