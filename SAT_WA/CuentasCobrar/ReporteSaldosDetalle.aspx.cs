using SAT_CL.CXC;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasCobrar
{
    public partial class ReporteSaldosDetalle : System.Web.UI.Page 
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
            buscaSaldosDetalle();
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
                case "SaldoDetalle":
                    //Exportando Contenido
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "NoFactura");
                    break;
                case "FichasFacturas":
                    //Exportando Contenido
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
                    break;
                case "Devolucion":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Cerrar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Cerrando ventana modal 
            gestionaVentanas(lnk, lnk.CommandName);

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvSaldosDetalle);

            //Actualizando Reporte
            upgvSaldosDetalle.Update();
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
            Controles.CambiaTamañoPaginaGridView(gvSaldosDetalle, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);

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
        protected void gvSaldosDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvSaldosDetalle_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvSaldosDetalle, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);

            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSaldosDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvSaldosDetalle, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);

            //Invocando Método de Suma
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Editar Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerConceptos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSaldosDetalle, sender, "lnk", false);

                //Validando que exista una Factura Seleccionada
                if (Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"]) > 0)
                {
                    //Inicializando Control
                    ucFacturadoConcepto.InicializaControl(Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"]));

                    //Abriendo Ventana Modal
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvSaldosDetalle, upgvSaldosDetalle.GetType(), "Edicion Conceptos", "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbFichasAplicadas_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSaldosDetalle, sender, "lnk", false);

                //Mostrando Ventana
                ScriptServer.AlternarVentana(this, this.GetType(), "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");

                //Obteniendo Reporte
                using (DataTable dtFichasFacturas = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(0, Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"])))
                {
                    //Validando que Existen Registros
                    if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvFichasFacturas, dtFichasFacturas, "Id", "", true, 1);

                        //Añadiendo Tabla a Session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table1");

                        //Mostrando Totales
                        gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
                    }
                    else
                    {
                        //Inicializando GridView
                        Controles.InicializaGridview(gvFichasFacturas);

                        //Eliminando Tabla de Session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");

                        //Mostrando Totales
                        gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Clic en el Link de Devoluciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerDevoluciones_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSaldosDetalle, sender, "lnk", false);

                //Instanciando Factura
                using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"])))
                {
                    //Validando que Exista la Factura
                    if (fac.habilitar)
                    {
                        //Validando que exista el Servicio
                        if (fac.id_servicio > 0)
                        {
                            //Obteniendo Devoluciones
                            using (DataTable dtDevoluciones = SAT_CL.Despacho.DevolucionFaltante.ObtieneDevolucionesServicio(fac.id_servicio))
                            {
                                //Validando que Existan Registros
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDevoluciones))
                                {
                                    //Cargando GridView
                                    Controles.CargaGridView(gvDevoluciones, dtDevoluciones, "Id-IdDevolucion", lblOrdenadoDev.Text, true, 1);

                                    //Añadiendo Tabla a Sesión
                                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDevoluciones, "Table2");

                                    //Mostrando Ventana
                                    ScriptServer.AlternarVentana(this, this.GetType(), "VentanaDevoluciones", "contenedorVentanaResumenDevoluciones", "ventanaResumenDevoluciones");
                                }
                                else
                                {
                                    //Inicializando GridView
                                    Controles.InicializaGridview(gvDevoluciones);

                                    //Eliminando Tabla de Sesión
                                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                                    
                                    //Mostrando Excepción
                                    ScriptServer.MuestraNotificacion(this, "No hay Devoluciones", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(this, "No es una Factura de Servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Clic en el Link de Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkServicio_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSaldosDetalle, sender, "lnk", false);

                //Instanciando Factura
                using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"])))
                {
                    //Validando que Exista la Factura
                    if (fac.habilitar)
                    {
                        //Validando que exista el Servicio
                        if (fac.id_servicio > 0)
                        {
                            //Obteniendo Control
                            LinkButton lkb = (LinkButton)sender;
                            
                            //Obteniendo Información de Viaje
                            using(DataTable dtInfoServicio = SAT_CL.Despacho.ServicioDespacho.ObtieneInformacionViaje(fac.id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            {
                                //Validando que existan Servicios
                                if (Validacion.ValidaOrigenDatos(dtInfoServicio))
                                {
                                    //Cargando GridView
                                    Controles.CargaGridView(gvInfoViaje, dtInfoServicio, "IdServicio", lblOrdenadoServicio.Text, true, 1);

                                    //Añadiendo Resultado a Sesión
                                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtInfoServicio, "Table3");
                                }
                                else
                                {
                                    //Inicializando GridView
                                    Controles.InicializaGridview(gvInfoViaje);

                                    //Eliminando Resultado de Sesión
                                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                                }
                            }

                            //Mostrando Ventana
                            gestionaVentanas(lkb, "Servicio");
                        }
                        else
                            //Mostrando Error
                            ScriptServer.MuestraNotificacion(this, "La Factura no tiene Servicio", ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Error
                        ScriptServer.MuestraNotificacion(this, "No existe la Factura", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #region Eventos GridView "Fichas Aplicadas"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFF_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFF.Text = Controles.CambiaSortExpressionGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFF_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFF.SelectedValue));

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
        }

        #endregion

        #region Eventos GridView "Devoluciones"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoDev.Text = Controles.CambiaSortExpressionGridView(gvDevoluciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvDevoluciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvDevoluciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoDev.SelectedValue));
        }

        #endregion

        #region Eventos GridView "Información Viaje"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Información Viaje"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño
            Controles.CambiaTamañoPaginaGridView(gvInfoViaje, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoServicio.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Información Viaje"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInfoViaje_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            Controles.CambiaSortExpressionGridView(gvInfoViaje, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Información Viaje"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInfoViaje_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Paginación
            Controles.CambiaIndicePaginaGridView(gvInfoViaje, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
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
            Controles.InicializaGridview(gvSaldosDetalle);

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
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFF, "", 3182);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDev, "", 3182);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServicio, "", 3182);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(lbxEstatusCobro, "", 2129);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 3, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 4, "");
        }
        /// <summary>
        /// Método encargado de Buscar los Saldos Detalle
        /// </summary>
        private void buscaSaldosDetalle()
        {
            //Obteniendo Fechas
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;

            //Obteniendo Filtro de Facturación Electronica
            byte facturacionElectronica = (byte)(rbTodos.Checked ? 0 : (rbSi.Checked ? 1 : (rbNo.Checked ? 2 : 0)));

            //Validando que se Incluyan las Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }

            //Obteniendo estatus de pago
            string id_estatus_cobro = Controles.RegresaSelectedValuesListBox(lbxEstatusCobro, "{0},", true, false);

            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtSaldosDetalle = Reporte.ObtieneReporteSaldoDetalle(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), fec_ini, fec_fin, txtReferencia.Text,
                                                        chkIndicadorServicio.Checked, chkSinProceso.Checked, chkProcesoActual.Checked, chkProcesoTerminado.Checked,
                                                        facturacionElectronica, Convert.ToInt32(Cadena.VerificaCadenaVacia(txtFolio.Text, "0")), id_estatus_cobro.Length > 1 ? id_estatus_cobro.Substring(0, id_estatus_cobro.Length - 1) : "",
                                                        txtNoServicio.Text))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtSaldosDetalle))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvSaldosDetalle, dtSaldosDetalle, "NoFactura", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSaldosDetalle, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvSaldosDetalle);

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
            if(TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvSaldosDetalle.FooterRow.Cells[31].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SubTotal)", "")));
                gvSaldosDetalle.FooterRow.Cells[32].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Trasladado)", "")));
                gvSaldosDetalle.FooterRow.Cells[33].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Retenido)", "")));
                gvSaldosDetalle.FooterRow.Cells[34].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
                gvSaldosDetalle.FooterRow.Cells[35].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoAplicado)", "")));
                gvSaldosDetalle.FooterRow.Cells[36].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoActual)", "")));
            }
            else
            {
                //Mostrando Totales
                gvSaldosDetalle.FooterRow.Cells[31].Text = 
                gvSaldosDetalle.FooterRow.Cells[32].Text = 
                gvSaldosDetalle.FooterRow.Cells[33].Text = 
                gvSaldosDetalle.FooterRow.Cells[34].Text = 
                gvSaldosDetalle.FooterRow.Cells[35].Text = 
                gvSaldosDetalle.FooterRow.Cells[36].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender">Control que Ejecuta la Acción</param>
        /// <param name="nombre_ventana">Nombre de la Ventana</param>
        private void gestionaVentanas(Control sender, string nombre_ventana)
        {
            //Validando Nombre
            switch (nombre_ventana)
            {
                case "EdicionConceptos":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "EdicionConceptos", "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");
                    break;
                case "FichasFacturas":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
                    break;
                case "Devoluciones":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "Devoluciones", "contenedorVentanaResumenDevoluciones", "ventanaResumenDevoluciones");
                    break;
                case "Servicio":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "Servicios", "contenedorVentanaInformacionViaje", "ventanaInformacionViaje");
                    break;
            }
        }

        #endregion
    }
}