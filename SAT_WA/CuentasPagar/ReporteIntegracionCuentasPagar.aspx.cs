using SAT_CL;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasPagar
{
    public partial class ReporteIntefracionCuentasPagar : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Carga de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es una recarga de página
            if (!IsPostBack)
            {
                //Cargando el catálogo de tamañó de gv
                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstatusFactura, 99, "");
                //Cargando el catálogo de tamañó de gv
                CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 3182);
                //Cargando GV vacío
                TSDK.ASP.Controles.InicializaGridview(gvFacturasProveedor);
                //Focus en primer control
                txtProveedor.Focus();
            }
        }
        /// <summary>
        /// Click sobre botón buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargaReporteIntegracion();
        }
        /// <summary>
        /// Cambio de cantidad de registros por página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvFacturasProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue) * 10, true, 2);
        }
        /// <summary>
        /// Click en botón exportar a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "*".ToCharArray());
        }
        /// <summary>
        /// Cambio de página activa del GV de facturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasProveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvFacturasProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Enlace a datos de cada fila del GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasProveedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si hay registros
            if (gvFacturasProveedor.DataKeys.Count > 0)
            {
                //Si es una fila de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow r = ((DataRowView)e.Row.DataItem).Row;

                    //Recuperando referencia de link de saldo
                    using (LinkButton lkbSaldo = (LinkButton)e.Row.FindControl("lkbAplicacionesFactura"))
                    {
                        //Si poseé un UUID
                        if (r.Field<string>("UUID") != "")
                        {
                            //Aplicando estilo de factura
                            e.Row.CssClass = "gridviewrowgroupheader";
                            lkbSaldo.Enabled = true;
                        }
                        else if (r.Field<string>("CPorte") == "Totales:")
                        {
                            e.Row.CssClass = "gridviewrowgroupfooter";
                            lkbSaldo.Enabled = false;
                        }
                        else
                        {
                            e.Row.CssClass = "gridviewrow";
                            lkbSaldo.Enabled = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el "Monto Aplicado"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAplicaciones_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasProveedor.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasProveedor, sender, "lnk", false);

                //Actualizando texto de encabezado de ventana
                lblVentanaFacturasFichas.Text = string.Format("Aplicaciones y Relaciones de la Factura '{0}'", gvFacturasProveedor.SelectedDataKey["SerieFolio"]);

                //Mostrando Ventana
                ScriptServer.AlternarVentana(this, this.GetType(), "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");

                //Invocando Método de Carga
                buscaFichasAplicadas(Convert.ToInt32(gvFacturasProveedor.SelectedDataKey["IdFactura"]));
            }
        }
        /// <summary>
        /// Click en botón cerrar aplicaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarFF_Click(object sender, EventArgs e)
        {
            //Ocultando Ventana Modal
            ScriptServer.AlternarVentana(this, "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturasProveedor);
        }
        protected void lnkExportarFF_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }

        protected void gvFichasFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFF.Text = Controles.CambiaSortExpressionGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        protected void gvFichasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);

            //Invocando Método de Suma
            sumaFichasFacturas();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Realiza la búsqueda de facturas y su integración de acuerdo a los filtros definidos
        /// </summary>
        private void cargaReporteIntegracion()
        {
            //Determinando rango de fechas solicitado
            DateTime fInicio = chkIncluir.Checked ? Fecha.ConvierteStringDateTime(txtFecIni.Text) : DateTime.MinValue;
            DateTime fFin = chkIncluir.Checked ? Fecha.ConvierteStringDateTime(txtFecFin.Text) : DateTime.MinValue;

            //Obteniendo estatus de pago
            string ids_estatus_pago = Controles.RegresaSelectedValuesListBox(ddlEstatusFactura, "{0},", true, false);

            //Realizando consulta de facturas de proveedor
            using (DataTable mitOriginal = SAT_CL.CXP.Reportes.ObtieneIntegracionFacturasProveedor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),
                txtSerie.Text, txtFolio.Text, txtUUID.Text, fInicio, fFin, txtNoServicio.Text, txtNoViaje.Text, txtCartaPorte.Text, txtNoAnticipo.Text, txtNoLiquidacion.Text, ids_estatus_pago.Length > 1 ? ids_estatus_pago.Substring(0, ids_estatus_pago.Length - 1) : ""))
            {
                //Definiendo auxiliar de origen de datos con formato
                DataTable mit = null;

                //Si hay resultados
                if (mitOriginal != null)
                {
                    //Copiando esquema
                    mit = mitOriginal.Clone();

                    //Declarando auxilizar de identificación de factura nueva
                    string UUID = ""; int idFactura = 0;
                    decimal montoAnt = 0, montoLiq = 0, montoOtr = 0;
                    decimal montoFac = 0, montoApl = 0, montoPen = 0, saldo = 0;
                    //Para cada linea encontrada
                    foreach (DataRow row in mitOriginal.Rows)
                    {
                        //Si la factura es distinta al auxiliar
                        if (row.Field<string>("UUID") != UUID)
                        {
                            //Insertando fila con totales de aplicaciones
                            if (UUID != "")
                            {
                                mit.Rows.Add(idFactura, "", "", "", "", DBNull.Value, "", "", "Totales:", 0, montoAnt, "", 0, montoLiq, "", montoOtr,
                                            "", montoFac, montoApl, montoPen, saldo);

                                //Limpiando elementos auxiliares de montos de aplicación
                                montoAnt = montoLiq = montoOtr = 0;
                            }

                            //Añadiendo linea a origen de datos alterno
                            mit.ImportRow(row);

                            //Preservando nuevo UUID
                            UUID = row.Field<string>("UUID");
                            idFactura = row.Field<int>("IdFactura");

                            //Asignando totales de factura
                            montoFac = Convert.ToDecimal(row["MontoFactura"]);
                            montoApl = Convert.ToDecimal(row["MontoAplicado"]);
                            montoPen = Convert.ToDecimal(row["MontoProgramado"]);
                            saldo = Convert.ToDecimal(row["Saldo"]);

                            //Incrementando total de aplicaciones
                            montoAnt += Convert.ToDecimal(row["MontoAnticipo"]);
                            montoLiq += Convert.ToDecimal(row["MontoLiquidacion"]);
                            montoOtr += Convert.ToDecimal(row["MontoOtros"]);
                        }
                        else
                        {
                            //Añadiendo linea a origen de datos alterno
                            mit.Rows.Add(idFactura, "", "", "", "", DBNull.Value, row["NoServicio"], row["NoViaje"],
                                                                                    row["CPorte"], row["NoAnticipo"], row["MontoAnticipo"], row["EstAnticipo"], row["NoLiquidacion"],
                                                                                    row["MontoLiquidacion"], row["EstLiquidacion"], row["MontoOtros"], "", 0, 0, 0, 0);
                            //Incrementando total de aplicaciones
                            montoAnt += Convert.ToDecimal(row["MontoAnticipo"]);
                            montoLiq += Convert.ToDecimal(row["MontoLiquidacion"]);
                            montoOtr += Convert.ToDecimal(row["MontoOtros"]);
                        }
                    }

                    //Si hubo registros
                    if (mit.Rows.Count > 0)
                    {
                        //Insertando suma total de último conjunto
                        mit.Rows.Add(idFactura, "", "", "", "", DBNull.Value, "", "", "Totales:", 0, montoAnt, "", 0, montoLiq, "", montoOtr,
                            "", montoFac, montoApl, montoPen, saldo);
                    }

                    //Guardando origen de datos
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                //Si no los hay
                else
                    //Borrando origen de datos
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");

                //Llenando gv con resultados
                Controles.CargaGridView(gvFacturasProveedor, mit, "IdFactura-SerieFolio", "", true, 2);
            }
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Fichas Aplicadas
        /// </summary>
        /// <param name="id_factura"></param>
        private void buscaFichasAplicadas(int id_factura)
        {
            //Obteniendo Reporte
            using (DataTable dtFichasFacturas = SAT_CL.CXP.FacturadoProveedor.ObtieneAplicacionesRelacionFacturasProveedor(id_factura))
            {
                //Cargando GridView
                Controles.CargaGridView(gvFichasFacturas, dtFichasFacturas, "Id-IdEntidad-IdRegistro", "", true, 1);

                //Validando que Existen Registros
                if (dtFichasFacturas != null)
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table1");
                else
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// Método 
        /// </summary>
        private void sumaFichasFacturas()
        {
            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = "0.00";
        }

        #endregion
                
    }
}