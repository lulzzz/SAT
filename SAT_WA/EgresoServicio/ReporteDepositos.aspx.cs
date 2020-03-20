using SAT_CL.CXP;
using SAT_CL.EgresoServicio;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Liquidacion;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Xml;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.EgresoServicio
{
    public partial class ReporteDepositos : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Generado al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Si no es una recarga de página
            if (!this.IsPostBack)
            {
                //Inicializando  la forma
                inicializaForma();
                //Asignamos Focus
                txtNoDeposito.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;

            }
        }
        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
        {
            //Obteniendo Archivo Codificado en UTF8
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] archivoXML = utf8.GetBytes(context.ToString());

            //Declarando Documento XML
            XmlDocument doc = new XmlDocument();

            //Obteniendo XML en cadena
            using (MemoryStream ms = new MemoryStream(archivoXML))

                //Cargando Documento XML
                doc.Load(ms);

            //Guardando en sesión el objeto creado
            System.Web.HttpContext.Current.Session["XML"] = doc;
            System.Web.HttpContext.Current.Session["XMLFileName"] = file_name;
        }
        /// <summary>
        /// Evento Generado al dar clic en Limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //Inicializa Controles
            inicializaControles();
            //Inicializando GridView
            Controles.InicializaGridview(gvDepositos);
        }
        /// <summary>
        /// Evento generado al dar clic en Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Cargando Depositos
            cargaDepositos();
        }
        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentana_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando Comando
            switch (lnk.CommandName)
            {
                case "FacturasLigadas":
                    {
                        //Abriendo Ventana Modal
                        ScriptServer.AlternarVentana(lnk, lnk.GetType(), "Facturas Ligadas", "contenedorVentanaFacturasLigadas", "ventanaFacturasLigadas");
                        break;
                    }
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvDepositos);
            //Actualizando Reporte
            upgvDepositos.Update();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtUnidad_TextChanged(object sender, EventArgs e)
        {
            string term = txtUnidad.Text;

            txtOperador.Text = term;
        }

        #region Eventos Depositos

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDepositos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Registros           
            Controles.CambiaTamañoPaginaGridView(gvDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoDepositos.SelectedValue), true, 1);

            //Suma Totales
            sumaTotales();
        }

        /// <summary>
        /// Evento Producido al Cambiar el Orden de los Datos del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvDepositos.DataKeys.Count > 0)
            {

                //Cambiando Ordenamiento
                lblOrdenarDepositos.Text = Controles.CambiaSortExpressionGridView(gvDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

                //Suma Totales
                sumaTotales();
            }
        }

        /// <summary>
        /// Evento Generado al cambiar indice de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando paginacion al Grid View
            Controles.CambiaIndicePaginaGridView(gvDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);

            //Suma Totales
            sumaTotales();
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
                case "Depositos":
                    //Exporta Grid View
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
                    break;
                case "Comprobaciones":
                    //Exporta Grid View
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
                    break;
                case "FacturasComp":
                    //Exporta Grid View
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "");
                    break;
            }
        }

        /// <summary>
        /// Evento generado al dar Click en el Link Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClik_Bitacora(object sender, EventArgs e)
        {
            //Validamos Registros
            if (gvDepositos.DataKeys.Count > 0)
            {
                //Selecionamo Fila
                Controles.SeleccionaFila(gvDepositos, sender, "lnk", false);

                //Inicializa Bitácora
                inicializaBitacora(gvDepositos.SelectedDataKey.Value.ToString(), "51", "Depósitos");

                //Carga Grafica
                Controles.CargaGrafica(ChtDepositos, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "Estatus", "Total", true);
              
            }
        }

        /// <summary>
        /// Evento producido al marcar o desmarcar el filtrado por fechas de Registró
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Si se marca la búsqueda por Fechas
            if (!(chkRangoFechas.Checked))
            {
                //Inicializamos Controles de Fechas
                DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
                DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
                txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy") + " 00:01";
                txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";

            }
            //Habilitación de cajas de texto para fecha
            txtFechaInicio.Enabled = txtFechaFin.Enabled = chkRangoFechas.Checked;
        }

        /// <summary>
        /// Evento producido al marcar o desmarcar el filtrado por fechas de  Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechasD_CheckedChanged(object sender, EventArgs e)
        {
            //Si se marca la búsqueda por Fechas
            if (!(chkRangoFechasD.Checked))
            {
                //Inicialozamos Controles de Fechas
                DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
                DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
                txtFechaInicioD.Text = primerdia.ToString("dd/MM/yyyy") + " 00:01";
                txtFechaFinD.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";

            }
            //Habilitación de cajas de texto para fecha
            txtFechaInicioD.Enabled = txtFechaFinD.Enabled = chkRangoFechasD.Checked;
        }
        /// <summary>
        /// Click en botón del GV de Depósitos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDeposito_Click(object sender, EventArgs e)
        {
            //validando existencia de registros
            if (gvDepositos.DataKeys.Count > 0)
            {
                //Determinando que botón fue pulsado
                LinkButton lkb = (LinkButton)sender;

                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvDepositos, sender, "lnk", false);

                //Instanciando Deposito
                using (Deposito dep = new Deposito(Convert.ToInt32(gvDepositos.SelectedDataKey["Id"])))
                {
                    //Determinando que comando será ejecutado
                    switch (lkb.CommandName)
                    {
                        case "Eliminar":
                            //Eliminando depósito
                            eliminaDeposito();
                            break;
                        case "AltaComprobacion":
                            {
                                //Validando Estatus
                                if ((DetalleLiquidacion.Estatus)dep.objDetalleLiquidacion.id_estatus_liquidacion == DetalleLiquidacion.Estatus.Registrado)
                                {
                                    //Validando estatus Por Depositar
                                    if (dep.Estatus == Deposito.EstatusDeposito.PorLiquidar)
                                    {
                                        //Inicializando Indices
                                        Controles.InicializaIndices(gvComprobaciones);

                                        //Validando que el Movimiento 
                                        if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(dep.objDetalleLiquidacion.id_movimiento))
                                        {
                                            //Inicializando Control
                                            inicializaControlComprobaciones(0, Convert.ToInt32(gvDepositos.SelectedDataKey["Id"]));

                                            //Mostrando Ventana de Alta de Comprobaciones
                                            gestionaVentanas(lkb, "AltaComprobaciones");
                                        }
                                        else
                                            //Mostrando Notificación
                                            ScriptServer.MuestraNotificacion(this, "El Movimiento ha sido Pagado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                    else
                                        //Mostrando Notificación
                                        ScriptServer.MuestraNotificacion(this, "No es posible hacer una Comprobación sobre un Anticipo no Depositado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                else
                                    //Mostrando Notificación
                                    ScriptServer.MuestraNotificacion(this, "El Deposito se encuentra Liquidado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                break;
                            }
                        case "VerComprobacion":
                            {
                                //Inicializando Indices
                                Controles.InicializaIndices(gvComprobaciones);

                                //Cargando las Comprobaciones por Deposito
                                cargaComprobacionesDeposito(Convert.ToInt32(gvDepositos.SelectedDataKey["Id"]));

                                //Mostrando Ventana de Alta de Comprobaciones
                                gestionaVentanas(lkb, "Comprobaciones");
                                break;
                            }
                    }
                }
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

            //Invocando Método de Gestión
            gestionaVentanas(lnk, lnk.CommandName);

            //Cargamos Depósitos
            cargaDepositos();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Facturas Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkFacturasProveedor_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvDepositos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDepositos, sender, "lnk", false);

                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;

                //Invocando Método de Facturas Ligadas
                obtieneFacturasLigadas(Convert.ToInt32(gvDepositos.SelectedDataKey["Id"]));

                //Abriendo Ventana Modal
                ScriptServer.AlternarVentana(lnk, lnk.GetType(), "Facturas Ligadas", "contenedorVentanaFacturasLigadas", "ventanaFacturasLigadas");
            }
        }

        #endregion

        #region Eventos Comprobaciones

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Comprobación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarComprobacion_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaComprobacion();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar Comprobación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarComprobacion_Click(object sender, EventArgs e)
        {
            //Inicializando Controles
            inicializaControlComprobaciones(0, 0);

            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarComprobacion, upbtnCancelarComprobacion.GetType(), "VentanaComprobacion", "contenedorVentanaComprobaciones", "ventanaComprobaciones");
        }

        #region Eventos GridView "Comprobaciones"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoComp.SelectedValue));

            //Invocando Método de Suma
            sumaTotalComprobaciones();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoComp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);

            //Invocando Método de Suma
            sumaTotalComprobaciones();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);

            //Invocando Método de Suma
            sumaTotalComprobaciones();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditaComp_Click(object sender, EventArgs e)
        {
            //Validando que existan 
            if (gvComprobaciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvComprobaciones, sender, "lnk", false);

                //Instanciando Comprobación
                using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(gvComprobaciones.SelectedDataKey["Id"])))
                {
                    //Validando Estatus de la Liquidación
                    if ((DetalleLiquidacion.Estatus)cmp.objDetalleComprobacion.id_estatus_liquidacion == DetalleLiquidacion.Estatus.Registrado)
                    {
                        //Validando que el Movimiento 
                        if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(cmp.objDetalleComprobacion.id_movimiento))
                        {
                            //Inicializando Controles
                            inicializaControlComprobaciones(cmp.id_comprobacion, 0);

                            //Gestionando Ventanas
                            gestionaVentanas(gvComprobaciones, "Comprobaciones");
                            gestionaVentanas(gvComprobaciones, "AltaComprobaciones");
                        }
                        else
                            //Mostrando Notificación
                            ScriptServer.MuestraNotificacion(this, "El Movimiento ha sido Pagado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(this, "La Comprobación esta Liquidada", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminaComp_Click(object sender, EventArgs e)
        {
            //Validando que existan 
            if (gvComprobaciones.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Declarando Variable Auxiliar
                int idCmp = 0;

                //Seleccionando Fila
                Controles.SeleccionaFila(gvComprobaciones, sender, "lnk", false);

                //Transacción
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando Comprobación
                    using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(gvComprobaciones.SelectedDataKey["Id"])))
                    {
                        //Validando que exista la Comprobación
                        if (cmp.id_comprobacion > 0)
                        {
                            //Validando estatus de la Comprobación
                            if ((DetalleLiquidacion.Estatus)cmp.objDetalleComprobacion.id_estatus_liquidacion == DetalleLiquidacion.Estatus.Registrado)
                            {
                                //Validando que el Movimiento 
                                if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(cmp.objDetalleComprobacion.id_movimiento))
                                {
                                    //Deshabilitando el Registro
                                    result = cmp.DeshabilitaComprobacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Operación Exitosa?
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Comprobación
                                        idCmp = result.IdRegistro;

                                        //Obteniendo Facturas
                                        using (DataTable dtFacturas = FacturadoProveedorRelacion.ObtieneFacturasComprobacion(idCmp))
                                        {
                                            //Validando que existan registros
                                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturas))
                                            {
                                                //Recorriendo Filas
                                                foreach (DataRow dr in dtFacturas.Rows)
                                                {
                                                    //Instanciando Fatura
                                                    using (FacturadoProveedorRelacion cf = new FacturadoProveedorRelacion(Convert.ToInt32(dr["IdFacturaRelacion"])))
                                                    {
                                                        //Validando que Exista
                                                        if (cf.id_factura_proveedor_relacion > 0)
                                                        {
                                                            //Deshabilita Factura
                                                            result = cf.DeshabilitarFacturaPoveedorRelacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Operación Exitosa?
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Instanciando Factura
                                                                using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(dr["IdFactura"])))
                                                                {
                                                                    //Deshabilitando Factura
                                                                    result = fp.DeshabilitaFacturadoProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                    //Validando la Operación de la Transacción
                                                                    if (!result.OperacionExitosa)
                                                                        //Terminando Ciclo
                                                                        break;
                                                                }
                                                            }
                                                            else
                                                                //Terminando Ciclo
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                //Instanciando Comprobación
                                                result = new RetornoOperacion(idCmp);

                                            //Operación Exitosa?
                                            if (result.OperacionExitosa)

                                                //Instanciando Comprobación
                                                result = new RetornoOperacion(idCmp);

                                            //Inicializando Indices
                                            TSDK.ASP.Controles.InicializaIndices(gvFacturasComprobacion);
                                        }
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Movimiento ha sido Pagado");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("La Comprobación se encuentra Liquidada");

                            //Validando que la Operación haya sido exitosa
                            if (result.OperacionExitosa)

                                //Completando transacción
                                trans.Complete();
                        }
                    }
                }

                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                {
                    //Cargando Catalogos
                    cargaComprobacionesDeposito(Convert.ToInt32(gvDepositos.SelectedDataKey["Id"]));

                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
                }

                //Mostrando Mensaje
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #region Eventos GridView "Facturas Comprobación"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoFacComp.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasComprobacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoFacComp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasComprobacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasComprobacion.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasComprobacion, sender, "lnk", false);

                //Inicializando Transacción
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando Factura
                    using (FacturadoProveedorRelacion cfp = new FacturadoProveedorRelacion(Convert.ToInt32(gvFacturasComprobacion.SelectedDataKey["IdFacturaRelacion"])))
                    {
                        //Validando que exista la Factura
                        if (cfp.habilitar)
                        {
                            //Deshabilitando Factura
                            result = cfp.DeshabilitarFacturaPoveedorRelacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando la Operación de la Transacción
                            if (result.OperacionExitosa)
                            {
                                //Instanciando Factura
                                using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(gvFacturasComprobacion.SelectedDataKey["IdFactura"])))
                                {
                                    //Deshabilitando Factura
                                    result = fp.DeshabilitaFacturadoProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando la Operación de la Transacción
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Comprobación
                                        using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text)))
                                        {
                                            //Validando que exista la Comprobación
                                            if (cmp.id_comprobacion > 0)
                                            {
                                                //Actualizando el Total de las Comprobaciones
                                                result = cmp.ActualizaTotalComprobacion(FacturadoProveedorRelacion.ObtieneTotalFacturasComprobacion(cmp.id_comprobacion), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando la Operación de la Transacción
                                                if (result.OperacionExitosa)
                                                {
                                                    //Cargando Facturas
                                                    cargaFacturasComprobacion(cmp.id_comprobacion);

                                                    //Completando Transacción
                                                    trans.Complete();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Mostrando Notificación
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                //Actualizando Control
                upgvFacturasComprobacion.Update();
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFactura_Click(object sender, EventArgs e)
        {
            //Guardando Factura
            guardaFacturaXML();
        }

        #endregion

        #endregion

        #region Eventos GridView "Factura Proveedor"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoFL.SelectedValue), true, 1);

            //Invocando Método de Suma
            sumaFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            //Asignando Expresión de Ordenamiento
            lblOrdenadoGrid.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, true, 1);

            //Invocando Método de Suma
            sumaFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño de la Página del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex, true, 1);

            //Invocando Método de Suma
            sumaFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFL_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), "Id");
        }

        #endregion

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
            TSDK.ASP.Controles.InicializaGridview(gvDepositos);
        }

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "TODOS", 45);
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDepositos, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacComp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoComp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFL, "", 26);
            //Cargando Catalogo Conceptos Comprobación
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 38, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Catalogo de Conceptos de Deposito
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConceptoDeposito, 71, "TODOS", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControles()
        {
            txtNoDeposito.Text = "";
            ddlEstatus.SelectedValue = "0";
            txtUnidad.Text = "";
            txtOperador.Text = "";
            txtTercero.Text = "";
            txtIdentificador.Text = "";
            txtNoServicio.Text = "";
            txtNoServicio.Text = "";
            chkRangoFechas.Checked = false;
            chkRangoFechasD.Checked = false;
            rdbTodos.Checked = true;
            rdbTodosComprobacion.Checked = true;
            DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
            DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
            txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy") + " 00:01";
            txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";
            txtFechaInicioD.Text = primerdia.ToString("dd/MM/yyyy") + " 00:01";
            txtFechaFinD.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";
        }


        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/EgresoServicio/ReporteDepositos.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=600,height=550";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora de Servicio", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de cargar los Depositos
        /// </summary>
        private void cargaDepositos()
        {
            //Declaramos variables de Fechas 
            DateTime fechaInicioRegistro = DateTime.MinValue, fechaFinRegistro = DateTime.MinValue, fechaInicioDeposito = DateTime.MinValue, fechaFinDeposito = DateTime.MinValue;

            int efectivo = 0;
            int comprobacion = 0;
            //De acuerdo al chek box de fechas de Registró
            if (chkRangoFechas.Checked)
            {
                //Declaramos variables de Fechas de Registró
                fechaInicioRegistro = Convert.ToDateTime(txtFechaInicio.Text);
                fechaFinRegistro = Convert.ToDateTime(txtFechaFin.Text);
            }

            //De acuerdo al chek box de fechas de Depósito
            if (chkRangoFechasD.Checked)
            {
                //Declaramos variables de Fechas de Depósito
                fechaInicioDeposito = Convert.ToDateTime(txtFechaInicioD.Text);
                fechaFinDeposito = Convert.ToDateTime(txtFechaFinD.Text);
            }

            //validando Busqueda por efectivo
            if (rdbEfectivo.Checked)
                efectivo = 1;

            //Valiodando Búsqueda Sin Comprobación
            if (rdbNo.Checked)
                comprobacion = 1;
            //validando Busqueda por  Transferencia
            if (rdbTranseferencia.Checked)
                efectivo = 2;

            //Validamos Búsqueda por Comprobación
            if (rdbSi.Checked)
                comprobacion = 2;

            //Inicializando indices de selección
            Controles.InicializaIndices(gvDepositos);

            //Obtenemos Depósito
            using (DataSet  ds = SAT_CL.EgresoServicio.Reportes.ReporteDepositos(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToInt32(Cadena.VerificaCadenaVacia(txtNoDeposito.Text, "0")), Convert.ToByte(ddlEstatus.SelectedValue) ,
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUnidad.Text, ":", 1, "0")),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtOperador.Text, ":", 1, "0")),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTercero.Text, ":", 1, "0")),
                                                                    Cadena.VerificaCadenaVacia(txtIdentificador.Text, "0"),
                                                                    efectivo, Cadena.VerificaCadenaVacia(txtNoServicio.Text, "0"),
                                                                    fechaInicioRegistro, fechaFinRegistro, fechaInicioDeposito, fechaFinDeposito,
                                                                    0, 
                                                                    comprobacion, Convert.ToInt32(ddlConceptoDeposito.SelectedValue),txtCartaPorte.Text,txtReferenciaViaje.Text))
            {
                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table") && Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvDepositos, ds.Tables["Table"], "Id", "", true, 1);
                    Controles.CargaGridView(gvResumen, ds.Tables["Table1"], "Estatus", "", true, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                    //Carga grafica
                    Controles.CargaGrafica(ChtDepositos, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                          "Estatus", "Total", true);
                    gvResumen.FooterRow.Cells[2].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvDepositos);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
                //Suma Totales al pie
                sumaTotales();
                //Limpiamos Etiqueta
                lblError.Text = "";
            }

        }
        /// <summary>
        /// Elimina el depósito seleccionado
        /// </summary>
        private void eliminaDeposito()
        { 
            //Instanciando depósito
            using (SAT_CL.EgresoServicio.Deposito deposito = new SAT_CL.EgresoServicio.Deposito(Convert.ToInt32(gvDepositos.SelectedDataKey["Id"])))
            { 
                //Realizando borrado
                RetornoOperacion resultado = deposito.DeshabilitaDeposito(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Si no hay errores
                if (resultado.OperacionExitosa)
                //Actualizando lista de depósitos
                    cargaDepositos();

                //Mostrando resultado
                ScriptServer.MuestraNotificacion(gvDepositos, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
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
                gvDepositos.FooterRow.Cells[3].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Monto)", "")));
                gvDepositos.FooterRow.Cells[4].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoComprobacion)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvDepositos.FooterRow.Cells[3].Text = string.Format("{0:C2}", 0);
                gvDepositos.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaTotalesFacturasLigadas()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvDepositos.FooterRow.Cells[3].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Monto)", "")));
                gvDepositos.FooterRow.Cells[4].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoComprobacion)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvDepositos.FooterRow.Cells[3].Text = string.Format("{0:C2}", 0);
                gvDepositos.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
            }
        }

        #region Métodos Comprobaciones

        /// <summary>
        /// Método Privado encargado de Inicializar el Control de Comprobaciones
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobacion</param>
        /// <param name="id_deposito">Referencia del Deposito</param>
        private void inicializaControlComprobaciones(int id_comprobacion, int id_deposito)
        {
            //Instanciando Comprobación
            using (Comprobacion cmp = new Comprobacion(id_comprobacion))

            //Instanciando Deposito
            using (Deposito dep = new Deposito(id_deposito))
            {
                //Validar que exista la Comprobación
                if (cmp.id_comprobacion > 0)
                {
                    //Asignando Valores
                    lblIdComprobacion.Text = cmp.id_comprobacion.ToString();
                    ddlConcepto.SelectedValue = cmp.id_concepto_comprobacion.ToString();
                    txtObservacion.Text = cmp.observacion_comprobacion;
                    txtValorUnitario.Text = cmp.objDetalleComprobacion.monto.ToString();
                }
                else
                {
                    //Asignando Valores
                    lblIdComprobacion.Text = "Por Asignar";
                    txtObservacion.Text = "";

                    //Validando que exista el Depsoito
                    if (dep.id_deposito > 0)
                    {
                        //Concatenando Valor al Control TextBox
                        txtValorUnitario.Text = dep.objDetalleLiquidacion.monto.ToString();
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 39, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", dep.id_concepto, "");
                    }
                    else
                    {   //Limpiando Control
                        txtValorUnitario.Text = "";
                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 38, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                    }
                    //Habilitando Control
                    txtValorUnitario.Enabled = true;
                    //Inicializando indices
                    Controles.InicializaIndices(gvComprobaciones);
                }

                //Cargando Facturas
                cargaFacturasComprobacion(id_comprobacion);

                //Limpiando Mensaje
                lblErrorComprobacion.Text = "";

                //Actualizando Controles
                uplblIdComprobacion.Update();
                uptxtObservacion.Update();
                uptxtValorUnitario.Update();
                uplblErrorComprobacion.Update();
            }
        }
        /// <summary>
        /// Método encargado de Cargar las Comprobaciones dado un Deposito
        /// </summary>
        /// <param name="id_deposito"></param>
        private void cargaComprobacionesDeposito(int id_deposito)
        {
            //Obteniendo Comprobaciones
            using (DataTable dtComprobaciones = SAT_CL.Liquidacion.Comprobacion.ObtieneComprobacionesDeposito(id_deposito))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobaciones))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvComprobaciones, dtComprobaciones, "Id", "", true, 1);
                    //Añadiendo tabla a Session
                    TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtComprobaciones, "Table1");
                    //Mostrando Totales
                    gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Monto)", "")).ToString();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
                    //Eliminando Tabla de Session
                    TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    //Mostrando Totales
                    gvComprobaciones.FooterRow.Cells[4].Text = "0.00";
                }
            }
        }
        /// <summary>
        /// Método encargado de Sumar el Total de las Comprobaciones
        /// </summary>
        private void sumaTotalComprobaciones()
        {
            //Validando que existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table1"))

                //Mostrando Totales
                gvComprobaciones.FooterRow.Cells[4].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Monto)", "")));
            else
                //Mostrando Totales
                gvComprobaciones.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
        }
        /// <summary>
        /// Método encargado de Gestionar la Ventana Modales de Comprobación
        /// </summary>
        /// <param name="sender">Control que dispara el Evento</param>
        /// <param name="command">Comando a Ejecutar</param>
        private void gestionaVentanas(Control sender, string command)
        {
            //Validando Comando
            switch(command)
            {
                case "Comprobaciones":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, "Comprobaciones", "contenedorVentanaComprobaciones", "ventanaComprobaciones");
                        break;
                    }
                case "AltaComprobaciones":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, "AltaComprobaciones", "contenedorVentanaAltaComprobaciones", "ventanaAltaComprobaciones");
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar la Comprobación
        /// </summary>
        private void guardaComprobacion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Deposito
            int idDeposito = Convert.ToInt32(gvDepositos.SelectedIndex == -1 ? 0 : gvDepositos.SelectedDataKey["Id"]);

            //Instanciando Deposito
            using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(idDeposito))
            {
                //Validando que no exista la Comprobación
                if (gvComprobaciones.SelectedIndex == -1)
                
                    //Insertando Comprobación
                    result = Comprobacion.InsertaComprobacion(idDeposito, Convert.ToInt32(ddlConcepto.SelectedValue), 0, txtObservacion.Text, false, 0,
                                                                DetalleLiquidacion.Estatus.Registrado, dep.objDetalleLiquidacion.id_unidad, dep.objDetalleLiquidacion.id_operador,
                                                                dep.objDetalleLiquidacion.id_proveedor_compania, dep.id_deposito > 0 ? dep.objDetalleLiquidacion.id_servicio : 0,
                                                                dep.id_deposito > 0 ? dep.objDetalleLiquidacion.id_movimiento : 0, dep.objDetalleLiquidacion.fecha_liquidacion, 
                                                                dep.objDetalleLiquidacion.id_liquidacion, 1, Convert.ToDecimal(txtValorUnitario.Text),
                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                
                else
                {
                    //Instanciando Comprobación
                    using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(gvComprobaciones.SelectedDataKey["Id"])))
                    {
                        //Validando que exista el Registro
                        if (cmp.id_comprobacion > 0)
                        
                            //Editando Comprobación
                            result = cmp.EditaComprobacion(cmp.id_deposito, Convert.ToInt32(ddlConcepto.SelectedValue), 0, txtObservacion.Text,
                                                    cmp.bit_transferencia, cmp.id_transferencia, (DetalleLiquidacion.Estatus)cmp.objDetalleComprobacion.id_estatus_liquidacion,
                                                    cmp.objDetalleComprobacion.id_unidad, cmp.objDetalleComprobacion.id_operador, cmp.objDetalleComprobacion.id_proveedor_compania,
                                                    cmp.objDetalleComprobacion.id_servicio, cmp.objDetalleComprobacion.id_movimiento, cmp.objDetalleComprobacion.fecha_liquidacion,
                                                    cmp.objDetalleComprobacion.id_liquidacion, cmp.objDetalleComprobacion.cantidad, Convert.ToDecimal(txtValorUnitario.Text == "" ? "0" : txtValorUnitario.Text),
                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }

                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)

                    //Inicializando Controles
                    inicializaControlComprobaciones(result.IdRegistro, idDeposito);

                //Mostrando Mensaje de la Operacion
                lblErrorComprobacion.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar las Facturas de la Comprobación
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobación</param>
        private void cargaFacturasComprobacion(int id_comprobacion)
        {
            //Obteniendo Facturas
            using (DataTable dtFacturas = FacturadoProveedorRelacion.ObtieneFacturasComprobacion(id_comprobacion))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvFacturasComprobacion, dtFacturas, "IdFactura-IdFacturaRelacion", "", true, 2);

                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table2");

                    //Deshabilitando Control
                    txtValorUnitario.Enabled = false;
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvFacturasComprobacion);

                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");

                    //Deshabilitando Control
                    txtValorUnitario.Enabled = true;
                }

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasComprobacion);
            }

            //Actualizando Control
            upgvFacturasComprobacion.Update();
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de los Conceptos de la Factura
        /// </summary>
        /// <param name="cfdi"></param>
        /// <param name="concepto"></param>
        /// <param name="tasa_imp_tras"></param>
        /// <param name="tasa_imp_ret"></param>
        /// <param name="imp_ret"></param>
        /// <param name="imp_tras"></param>
        private void obtieneCantidadesConcepto(XmlDocument cfdi, XmlNode concepto, out decimal tasa_imp_tras, out decimal tasa_imp_ret)
        {
            //Validación de Retenciones
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
            {
                //Asignando Valores
                tasa_imp_ret = (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) / Convert.ToDecimal(concepto.Attributes["importe"].Value)) * 100;
            }
            else
                //Asignando Valores
                tasa_imp_ret = 0;
            //Validación de Traslados
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
            {
                //Obteniendo Valor de la Tasa
                string tasaImpTrasladado = cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"]["cfdi:Traslado"].Attributes["tasa"].Value;

                //Remplazando Puntos Decimales
                tasaImpTrasladado = tasaImpTrasladado.Replace(".", "|");

                //Validando que exista un valor despues del Punto decimal
                if (Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1)) > 0.00M)

                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1));
                else
                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 0));
            }
            else
                //Asignando Valores
                tasa_imp_tras = 0;
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de la Factura
        /// </summary>
        /// <param name="document">Factura XML</param>
        /// <param name="total_p">Total en Pesos</param>
        /// <param name="subtotal_p">Subtotal en Pesos</param>
        /// <param name="descuento_p">Descuento en Pesos</param>
        /// <param name="traslado_p">Importe Trasladado en Pesos</param>
        /// <param name="retenido_p">Importe Retenido en Pesos</param>
        /// <param name="monto_tc">Monto del Tipo de Cambio</param>
        private void obtieneCantidades(XmlDocument document, out decimal total_p, out decimal subtotal_p, out decimal descuento_p, out decimal traslado_p, out decimal retenido_p, out decimal monto_tc)
        {
            //Validando si existe el Tipo de Cambio
            if (document.DocumentElement.Attributes["TipoCambio"] == null)
            {
                //Asignando Atributo Obligatorios
                monto_tc = 1;
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value);
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value);
                traslado_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0;
                retenido_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0;

                //Asignando Atributos Opcionales
                descuento_p = document.DocumentElement.Attributes["descuento"] == null ? 0 :
                    Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value);
            }
            else
            {
                //Asignando Atributo Obligatorios
                monto_tc = Convert.ToDecimal(document.DocumentElement.Attributes["TipoCambio"].Value);
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value) * monto_tc;
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value) * monto_tc;
                traslado_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0) * monto_tc;
                retenido_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0) * monto_tc;

                //Asignando Atributos Opcionales
                descuento_p = (document.DocumentElement.Attributes["descuento"] == null ? 0 : Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value)) * monto_tc;
            }
        }
        /// <summary>
        /// Método privado encargado de Validar la Factura en formato XML
        /// </summary>
        /// <param name="mensaje">Mensaje de Operación</param>
        /// <returns></returns>
        private bool validaFacturaXML(out string mensaje)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Limpiando Mensaje
            mensaje = "";

            //Validando que exista un Archivo en Sessión
            if (Session["XML"] != null)
            {
                //Declarando Documento XML
                XmlDocument doc = (XmlDocument)Session["XML"];

                //Validando que exista el Documento
                if (doc != null)
                {
                    try
                    {   //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Declarando Variable Auxiliar
                            int idEmisor = 0;

                            //Validando que exista la Compania
                            if (emi.id_compania_emisor_receptor > 0)

                                //Asignando Emisor
                                idEmisor = emi.id_compania_emisor_receptor;

                            //Si no existe
                            else
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultado = new RetornoOperacion();

                                //Insertando Compania
                                resultado = SAT_CL.Global.CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto("", doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(),
                                                                            doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(), 0, false, false, true, 0, "", "", "", 0, 0, 0, 0,
                                                                            "FACTURAS DE PROVEEDOR", "", 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Asignando Registro
                                idEmisor = resultado.IdRegistro;
                            }

                            //Validando que el RFC sea igual
                            if (idEmisor > 0)
                            {
                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que el RFC sea igual
                                    if (cer.rfc.ToUpper() == doc.DocumentElement["cfdi:Receptor"].Attributes["rfc"].Value.ToUpper())
                                    {
                                        //Instanciando XSD de validación
                                        using (EsquemasFacturacion ef = new EsquemasFacturacion(doc["cfdi:Comprobante"].Attributes["version"].Value))
                                        {
                                            //Validando que exista el XSD
                                            if (ef.id_esquema_facturacion != 0)
                                            {
                                                //Declarando variables Auxiliares
                                                bool addenda;

                                                //Obteniendo XSD
                                                string[] esquemas = EsquemasFacturacion.CargaEsquemasPadreYComplementosCFDI(ef.version, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out addenda);

                                                //Validando que Exista una Addenda
                                                if (doc.DocumentElement["cfdi:Addenda"] != null)

                                                    //Removiendo Addendas
                                                    doc.DocumentElement["cfdi:Addenda"].RemoveAll();

                                                //Obteniendo Validación
                                                result = TSDK.Base.Xml.ValidaXMLSchema(doc.InnerXml, esquemas, out mensaje);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Mostrando el Mensaje
                                        mensaje = "El RFC de la factura no coincide con el Receptor";

                                        //Asignando Negativa el Objeto de retorno
                                        result = false;
                                    }
                                }
                            }
                            else
                            {
                                //Mostrando el Mensaje
                                mensaje = "El RFC de la factura no coincide con el Emisor";

                                //Asignando Negativa el Objeto de retorno
                                result = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Mostrando Mensaje
                        mensaje = e.Message;
                    }
                }
                else//Mensaje de Error
                    mensaje = "No se ha podido cargar el Archivo";
            }
            else//Mensaje de Error
                mensaje = "No se ha podido localizar el Archivo";

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Guardar la Factura en XML
        /// </summary>
        private void guardaFacturaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion resultado = new RetornoOperacion();

            //Declarando Objeto de Mensaje
            string mensaje = "";

            //Declarando Variable para Factura
            int idFactura = 0;

            //Instanciando Comprobacion
            using (SAT_CL.Liquidacion.Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text == "Por Asignar" ? "0" : lblIdComprobacion.Text)))
            {
                //Validando la Comprobación
                if (cmp.id_comprobacion > 0)
                {
                    //Inicializando transacción
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Declarando Documento XML
                        XmlDocument doc = (XmlDocument)Session["XML"];

                        //Validando que exista una Factura
                        if (doc != null)
                        {
                            //Recuperando tabla temporal
                            if (validaFacturaXML(out mensaje))
                            {
                                //Declarando variables de Montos
                                decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

                                //Obteniendo Valores
                                obtieneCantidades(doc, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

                                //Instanciando Emisor-Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Emisor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que Exista el Emisor
                                    if (emisor.id_compania_emisor_receptor != 0)
                                    {
                                        //Instanciando Emisor-Compania
                                        using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                        {
                                            //Validando que coincida el RFC
                                            if (receptor.rfc.Equals(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value))
                                            {
                                                //Declarando Variables Auxiliares
                                                decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                                /** Retenciones **/
                                                //Validando que no exista el Nodo
                                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] == null)
                                                {
                                                    //Validando que existan Retenciones
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
                                                    {
                                                        //Validando que existan Nodos
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"].ChildNodes.Count > 0)
                                                        {
                                                            //Recorriendo Retenciones
                                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"])
                                                            {
                                                                //Sumando Impuestos Retenidos
                                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Asignando Total de Impuestos
                                                    totalImpRetenidos = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value);


                                                /** Traslados **/
                                                //Validando que no exista el Nodo
                                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] == null)
                                                {
                                                    //Validando que existan Traslados
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
                                                    {
                                                        //Validando que existan Nodos
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"].ChildNodes.Count > 0)
                                                        {
                                                            //Recorriendo Traslados
                                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"])
                                                            {
                                                                //Sumando Impuestos Trasladados
                                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Asignando Total de Impuestos
                                                    totalImpTrasladados = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value);
                                                
                                                //Insertando factura
                                                resultado = FacturadoProveedor.InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
                                                                                    cmp.objDetalleComprobacion.id_servicio, doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                                                    doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                                                    doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                                                    Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI,
                                                                                    "I", 1, (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
                                                                                    0, 0, 0, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value), Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value),
                                                                                    doc["cfdi:Comprobante"].Attributes["descuentos"] == null ? 0 : Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["descuentos"].Value),
                                                                                    totalImpTrasladados, totalImpRetenidos,
                                                                                    doc["cfdi:Comprobante"].Attributes["Moneda"] == null ? "" : doc["cfdi:Comprobante"].Attributes["Moneda"].Value,
                                                                                    monto_tc, Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), total_p, subtotal_p, descuento_p, traslado_p,
                                                                                    retenido_p, false, DateTime.MinValue, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value),
                                                                                    doc["cfdi:Comprobante"].Attributes["condicionesDePago"] == null ? "" : doc["cfdi:Comprobante"].Attributes["condicionesDePago"].Value,
                                                                                    emisor.dias_credito, 1, (byte)FacturadoProveedor.EstatusValidacion.ValidacionSintactica, "",
                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }
                                            else
                                                //Instanciando Excepcion
                                                resultado = new RetornoOperacion("La Compania Receptora no esta registrada");
                                        }
                                    }
                                    else
                                        //Instanciando Excepcion
                                        resultado = new RetornoOperacion("El Compania Proveedora no esta registrado");
                                }

                                //Validando que se inserto la Factura
                                if (resultado.OperacionExitosa)
                                {
                                    //Obteniendo Factura
                                    idFactura = resultado.IdRegistro;

                                    //Obteniendo Nodos de Concepto
                                    XmlNodeList xmlNL = doc.GetElementsByTagName("cfdi:Concepto");

                                    //Declarando Variables Auxiliares
                                    decimal tasa_imp_ret, tasa_imp_tras;
                                    bool res = true;
                                    int contador = 0;

                                    //Recorriendo cada 
                                    while (res)
                                    {
                                        //Obteniendo Concepto
                                        XmlNode node = xmlNL[contador];

                                        //Obteniendo Cantidades del Concepto
                                        obtieneCantidadesConcepto(doc, node, out tasa_imp_tras, out tasa_imp_ret);

                                        //Insertando Cocepto de Factura
                                        resultado = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(node.Attributes["cantidad"].Value),
                                                                node.Attributes["unidad"] == null ? "" : node.Attributes["unidad"].Value, node.Attributes["noIdentificacion"] == null ? "" : node.Attributes["noIdentificacion"].Value,
                                                                node.Attributes["descripcion"].Value, 0, Convert.ToDecimal(node.Attributes["valorUnitario"] == null ? "1" : node.Attributes["valorUnitario"].Value),
                                                                Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value),
                                                                Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value) * monto_tc,
                                                                tasa_imp_ret, tasa_imp_tras, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Incrementando Contador
                                        contador++;

                                        //Obteniendo resultado del Ciclo
                                        res = contador >= xmlNL.Count ? false : resultado.OperacionExitosa;
                                    }
                                    //Validando resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Declarando Variables Auxiliares
                                        string ruta;

                                        //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                        ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + Session["XMLFileName"].ToString());

                                        //Insertamos Registro
                                        resultado = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + Session["XMLFileName"].ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                            Encoding.UTF8.GetBytes(doc.OuterXml), ruta);
                                    }
                                }

                                //Validando la Operación de la Transacción
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertando Comprobacion de Factura
                                    resultado = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, 104, Convert.ToInt32(lblIdComprobacion.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando la Operación de la Transacción
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Actualizando el Total de las Comprobaciones
                                        resultado = cmp.ActualizaTotalComprobacion(FacturadoProveedorRelacion.ObtieneTotalFacturasComprobacion(cmp.id_comprobacion), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando la Operación de la Transacción
                                        if (resultado.OperacionExitosa)

                                            //Completando Transacción
                                            trans.Complete();
                                    }
                                }
                            }
                            else
                            {
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion(mensaje);

                                //Limpiando Session
                                Session["XML"] = null;
                            }
                        }
                        else
                        {
                            //Instanciando Excepcion
                            resultado = new RetornoOperacion("No existe la Factura");

                            //Limpiando Session
                            Session["XML"] = null;
                        }
                    }
                    

                    //Validando que la Operación haya Sido Exitosa
                    if (resultado.OperacionExitosa)

                        //Inicializando Controles
                        inicializaControlComprobaciones(Convert.ToInt32(lblIdComprobacion.Text), 0);
                }
                else
                {
                    //Instanciando Excepcion
                    resultado = new RetornoOperacion("No existe la Comprobación");

                    //Limpiando Session
                    Session["XML"] = null;
                }
            }

            //Mostrando Mensaje de la Operación
            lblErrorComprobacion.Text = resultado.Mensaje;
        }

        #endregion

        #region Métodos Facturas Ligadas

        /// <summary>
        /// Método encargado de Obtener las Facturas Ligadas
        /// </summary>
        private void obtieneFacturasLigadas(int id_deposito)
        {
            //Obtiene Facturas Ligadas
            using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedor.ObtieneFacturasPorDeposito(id_deposito))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturasLigadas, dtFacturasLigadas, "Id", lblOrdenadoGrid.Text, true, 1);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLigadas, "Table3");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturasLigadas);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }

                //Invocando Método de Suma
                sumaFacturasLigadas();
            }
        }
        /// <summary>
        /// Método encargado de Sumar las Facturas Ligadas
        /// </summary>
        private void sumaFacturasLigadas()
        {
            //Validando si existen Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3")))
            {
                //Asignando Valores
                gvFacturasLigadas.FooterRow.Cells[6].Text = string.Format("{0:C2}", Convert.ToDecimal(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").Compute("SUM(SubTotal)", "")));
                gvFacturasLigadas.FooterRow.Cells[7].Text = string.Format("{0:C2}", Convert.ToDecimal(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").Compute("SUM(ImpTrasladado)", "")));
                gvFacturasLigadas.FooterRow.Cells[8].Text = string.Format("{0:C2}", Convert.ToDecimal(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").Compute("SUM(ImpRetenido)", "")));
                gvFacturasLigadas.FooterRow.Cells[9].Text = string.Format("{0:C2}", Convert.ToDecimal(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").Compute("SUM(Total)", "")));
            }
            else
            {
                //Asignando Valores
                gvFacturasLigadas.FooterRow.Cells[6].Text = 
                gvFacturasLigadas.FooterRow.Cells[7].Text = 
                gvFacturasLigadas.FooterRow.Cells[8].Text = 
                gvFacturasLigadas.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
            }
        }

        #endregion

        #endregion
    }
}