using SAT_CL.CXC;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasCobrar
{
    public partial class AplicacionPago : System.Web.UI.Page 
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
            //Validando que se haya producido un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar Fichas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaFichasIngreso();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFacturas_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaFacturas();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aplicar Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAplicarFacturas_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            string[] result_msn = new string[1];
            int contador = 0;
            decimal monto_aplicado = 0.00M;
            SAT_CL.Bancos.EgresoIngreso.Estatus estatus;
            SAT_CL.Facturacion.Facturado.EstatusFactura estatusFactura;
            decimal tol_mon = 0.00M;
            
            //Obteniendo Facturas Seleccionadas
            GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvFacturas, "chkVariosFactura");

            //Validando que Existan Registros
            if (gvrs.Length > 0)
            {
                //Creando Arreglo Dinamicamente
                result_msn = new string[gvrs.Length];

                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validando que este seleccionado una Ficha de Ingreso
                    if (gvFichasIngreso.SelectedIndex != -1)
                    {
                        //Instanciando Ficha de Ingreso
                        using (SAT_CL.Bancos.EgresoIngreso ficha_ingreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvFichasIngreso.SelectedDataKey["Id"])))
                        {
                            //Validando que existe el Registro
                            if (ficha_ingreso.id_egreso_ingreso > 0)
                            {
                                try
                                {
                                    //Obteniendo Montos
                                    tol_mon = Convert.ToDecimal(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Monto Tolerancia Saldo Pago Cliente", ficha_ingreso.id_compania));
                                }
                                catch { tol_mon = 0.00M; }

                                //Obteniendo Estatus
                                estatus = ficha_ingreso.estatus;

                                //Recorriendo Filas
                                foreach (GridViewRow gvr in gvrs)
                                {
                                    //Seleccionando Fila
                                    gvFacturas.SelectedIndex = gvr.RowIndex;

                                    //Validando que el Monto no sea 0
                                    if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) != 0.00M)
                                    {
                                        //Insertando Ficha de Ingreso Aplicada
                                        result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(9, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), Convert.ToInt32(gvFichasIngreso.SelectedDataKey["Id"]),
                                                            Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), false, 0, false, 0,
                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando Factura
                                            using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                                            {
                                                //Validando que exista la Factura
                                                if (fac.id_factura > 0)
                                                {
                                                    //Validando que sea la misma moneda
                                                    if (fac.moneda == ficha_ingreso.id_moneda)
                                                    {
                                                        //Calculando Estatus de la Factura
                                                        decimal diff = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPendiente"]) - Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]);
                                                        estatusFactura = diff > 0 ? (diff <= tol_mon ? SAT_CL.Facturacion.Facturado.EstatusFactura.Liquidada : SAT_CL.Facturacion.Facturado.EstatusFactura.AplicadaParcial) : SAT_CL.Facturacion.Facturado.EstatusFactura.Liquidada;

                                                        //Actualizando Estatus de la Factura
                                                        result = fac.ActualizaEstatusFactura(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("Las Monedas de la Ficha y la Factura no coinciden");
                                                }
                                            }

                                            //Validando que la Operación fuese Exitosa
                                            if(result.OperacionExitosa)
                                            {
                                                //Guardando Mensaje de la Operación
                                                result_msn[contador] = "* La Factura " + gvFacturas.SelectedDataKey["Id"].ToString() + ": Ha sido aplicada Exitosamente";
                                                //Incrementando Contador
                                                contador++;
                                            }
                                            else
                                            {
                                                //Mostrando Mensaje de Operación
                                                ScriptServer.MuestraNotificacion(this.Page, "Error en la Factura " + gvFacturas.SelectedDataKey["Id"].ToString() + ": " + result.Mensaje, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                //Creando Arreglo
                                                result_msn = new string[1];
                                                result_msn[0] = "Error en la Factura " + gvFacturas.SelectedDataKey["Id"].ToString() + ": " + result.Mensaje;

                                                //Terminando Ciclo
                                                break;
                                            }
                                        }
                                    }
                                }

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Recorriendo Filas
                                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                                        //Sumando Monto por Aplicar
                                        monto_aplicado = monto_aplicado + Convert.ToDecimal(dr["MontoPorAplicar"]);

                                    //Validando que la Ficha tiene saldo
                                    if (monto_aplicado == Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]))

                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.Aplicada;
                                    else
                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.AplicadaParcial;
                                    
                                    //Validando que la Operación fuese Exitosa
                                    if(result.OperacionExitosa)
                                    
                                        //Actualizando Estatus de la Ficha de Ingreso
                                        result = ficha_ingreso.ActualizaFichaIngresoEstatus(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            else
                            
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe la Ficha de Ingreso");
                            
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No hay una Fichas de Ingreso Seleccionada");

                    //Validando que las Operaciones fuesen Exitosas
                    if (result.OperacionExitosa)
                    {
                        //Invocando Método de Busqueda de Fichas de Ingreso
                        buscaFichasIngreso();

                        //Invocando Método de Busqueda de Facturas
                        buscaFacturas();

                        //lblErrorFicha.Text = string.Format("* La Ficha {0}: Ha sido actualizada",result.IdRegistro);
                        //lblErrorFactura.Text = string.Join("<br/>", result_msn);

                        //Mostrando Saldos
                        actualizaTotales();

                        //Completando Transacción
                        trans.Complete();

                        //Mostrando Mensaje de Operación
                        //ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            else
                //Mostrando Mensaje de Operación
                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No hay Facturas Seleccionadas"), ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Validando que existan Mensajes
            if (result_msn.Length > 0)
            {
                //Recorriendo Errores
                foreach (string mensaje in result_msn)
                {
                    //Validando Error
                    if (mensaje.Contains("Error"))

                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(this, mensaje, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    else
                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(this, mensaje, ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #region Eventos GridView "Fichas de Ingreso"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Fichas de Ingreso"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFI.SelectedValue), true, 2);

            //Inicializando Indices
            Controles.InicializaIndices(gvFichasIngreso);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                    //Editando el Registro
                    dr["MontoPorAplicar"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                //Invocando Método de Suma
                sumaTotalesFacturas();

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
                //Sumando Totales
                sumaTotalesPieFI();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Fichas de Ingreso"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Fichas de Ingreso"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasIngreso_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFI.Text = Controles.CambiaSortExpressionGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);

            //Inicializando Indices
            Controles.InicializaIndices(gvFichasIngreso);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                    //Editando el Registro
                    dr["MontoPorAplicar"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
                //Sumando Totales
                sumaTotalesPieFI();

            //Invocando Método de Suma
            sumaTotalesFacturas();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas de Ingreso"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasIngreso_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);

            //Inicializando Indices
            Controles.InicializaIndices(gvFichasIngreso);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                    //Editando el Registro
                    dr["MontoPorAplicar"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
                //Sumando Totales
                sumaTotalesPieFI();

            //Invocando Método de Suma
            sumaTotalesFacturas();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Enlace de datos de fila de GV Fichas de Ingreso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasIngreso_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si hay elementos que enlazar
            if (gvFichasIngreso.DataKeys.Count > 0)
            {
                //Si la fila es de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando referencia de cambio de estatus
                    using (LinkButton lnkEstatus = (LinkButton)e.Row.FindControl("lnkEstatusFI"))
                    {
                        //Recuperando datos de la fila
                        DataRow r = ((DataRowView)e.Row.DataItem).Row;
                        //Si el estatus de la FI es Aplicado Parcial
                        if ((SAT_CL.Bancos.EgresoIngreso.Estatus)Convert.ToByte(r["IdEstatus"]) == SAT_CL.Bancos.EgresoIngreso.Estatus.AplicadaParcial)
                            //Habilitando opción de cambio de estatus
                            lnkEstatus.Enabled = true;
                        else
                            //Deshabilitando opción de cambio de estatus
                            lnkEstatus.Enabled = false;
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Registro del GridView "Fichas de Ingreso"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros 
            if (gvFichasIngreso.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFichasIngreso, sender, "lnk", false);

                //Mostrando el Saldos
                actualizaTotales();

                //Validando que existan Registros
                if(Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                {
                    //Recorriendo Filas
                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                        //Editando el Registro
                        dr["MontoPorAplicar"] = "0.00";

                    //Aceptando Cambios
                    ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                    //Cargando GridView
                    Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                    //Invocando Método de Suma
                    sumaTotalesFacturas();

                    //Desmarcando todas las Filas
                    Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");

                    //Invocando Método de Suma
                    sumaTotalesFacturas();
                }
                
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturas);
            }
        }
        /// <summary>
        /// Click en el elemento Estatus de la FI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEstatusFI_Click(object sender, EventArgs e)
        {
            //Seleccionando fila
            Controles.SeleccionaFila(gvFichasIngreso, sender, "lnk", false);
            LinkButton lnk = (LinkButton)sender;
            //Abriendo dialogo de confirmación de cambio de estatus
            alternaVentanaModal(lnk.CommandName, lnk);
        }
        /// <summary>
        /// Evento Producido al Seleccionar el Monto Aplicado del GridView "Fichas de Ingreso"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerFacturasAplicadas_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvFichasIngreso.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFichasIngreso, sender, "lnk", false);

                //Personalizando Mensaje
                lblVentanaFacturasFichas.Text = "Facturas Aplicadas";

                //Cambiando Encabezado del GridView
                gvFichasFacturas.Columns[1].HeaderText = "Serie-Folio";
                upgvFichasFacturas.Update();

                //Invocando Método de Busqueda
                buscaFichasAplicadas(Convert.ToInt32(gvFichasIngreso.SelectedDataKey["Id"]),0);

                //Mostrando Ventana
                ScriptServer.AlternarVentana(upgvFichasIngreso, upgvFichasIngreso.GetType(), "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
            }
        }

        #endregion

        #region Eventos GridView "Facturas"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                    //Editando el Registro
                    dr["MontoPorAplicar"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                //Cambiando tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFacturas.SelectedValue), true, 2);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }
            else
            {
                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }

            //Invocando Método de Suma
            sumaTotalesFacturas();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacturas_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                    //Editando el Registro
                    dr["MontoPorAplicar"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                //Cambiando Expresión del Ordenamiento
                lblOrdenadoFacturas.Text = Controles.CambiaSortExpressionGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }
            else
            {
                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }

            //Invocando Método de Suma
            sumaTotalesFacturas();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                    //Editando el Registro
                    dr["MontoPorAplicar"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 2);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }
            else
            {
                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }

            //Invocando Método de Suma
            sumaTotalesFacturas();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Marcar un Registro del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosFactura_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvFacturas.DataKeys.Count > 0)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)sender;
                    
                //Validando que exista una Ficha Seleccionada
                if (gvFichasIngreso.SelectedIndex != -1)
                {
                    //Declarando variable que Guardara las Filas Seleccionadas
                    int[] indices_chk = new int[1];
                    
                    //Validando Id del Control
                    switch (chk.ID)
                    {
                        case "chkTodosFactura":
                            {
                                //Validando que se haya Marcado
                                if (chk.Checked)
                                {
                                    //Recorriendo Filas
                                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                                        //Editando el Registro
                                        dr["MontoPorAplicar"] = "0.00";

                                    //Aceptando Cambios
                                    ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                                    //Cargando GridView
                                    Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);
                                    
                                    //Recorriendo Filas
                                    foreach (GridViewRow gvr in gvFacturas.Rows)
                                    {
                                        //Obteniendo Fila Actual
                                        CheckBox chkFila = (CheckBox)gvr.FindControl("chkVariosFactura");

                                        //Validando que Exista el Control
                                        if (chkFila != null)
                                        {
                                            //Marcando el Control
                                            chkFila.Checked = true;

                                            //Invocando Método de Configuración
                                            configuraRegistroFactura(chkFila, out indices_chk);
                                        }
                                    }
                                }
                                else
                                {
                                    //Recorriendo Filas
                                    foreach(DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)
                                    
                                        //Editando el Registro
                                        dr["MontoPorAplicar"] = "0.00";
                                    
                                    //Aceptando Cambios
                                    ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                                    //Cargando GridView
                                    Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);
                                    
                                    //Desmarcando todas las Filas
                                    Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", chk.Checked);
                                }

                                //Obteniendo Control de Encabezado
                                CheckBox chkEncabezado = (CheckBox)gvFacturas.HeaderRow.FindControl("chkTodosFactura");

                                //Validando que Exista el Control
                                if (chkEncabezado != null)

                                    //Marcando Control de Encabezado
                                    chkEncabezado.Checked = chk.Checked;

                                break;
                            }
                        case "chkVariosFactura":
                            {
                                //Invocando Método de Configuración
                                configuraRegistroFactura(sender, out indices_chk);

                                break;
                            }
                    }

                    //Invocando Método de Suma
                    sumaTotalesFacturas();

                    //Mostrando Saldos
                    actualizaTotales();
                }
                else
                {
                    //Deshabilitando Control
                    chk.Checked = false;

                    //Mostrando Error
                    lblErrorFactura.Text = "* Debe Seleccionar una Ficha de Ingreso";
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Monto de un Registro del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCambiar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturas, sender, "lnk", false);

                //Obteniendo Control
                TextBox txt = (TextBox)gvFacturas.SelectedRow.FindControl("txtMXA");

                //Declarando Variables Auxiliares
                int[] indices_chk = new int[1];
                int contador = 0;

                //Validando el Comando del Control
                switch(((LinkButton)sender).CommandName)
                {
                    case "Cambiar":
                        {
                            //Validando que exista el Control
                            if (txt != null)
                            {
                                //Habilitando el Control
                                txt.Enabled = true;

                                //Obteniendo Control
                                LinkButton lnk = (LinkButton)sender;

                                //Configurando Control
                                lnk.Text = lnk.CommandName = "Guardar";
                            }

                            break;
                        }
                    case "Guardar":
                        {
                            //Validando que exista el Control
                            if (txt != null)
                            {
                                //Declarando Variable Auxiliar
                                bool value = true;
                                
                                //Recorriendo Registros
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString()))
                                {
                                    //Validando que el Valor Ingresado no supera al Permitido
                                    value = Convert.ToDecimal(dr["MPA2"]) >= Convert.ToDecimal(txt.Text == "" ? "0" : txt.Text) ? true : false;

                                    //Realizando Validación
                                    if (value)

                                        //Actualizando Registro
                                        dr["MontoPorAplicar"] = string.Format("{0:0.00}", txt.Text == "" ? "0" : txt.Text);
                                    else
                                    {
                                        //Mostrando Monto Maximo Permitido
                                        txt.Text = string.Format("{0:0.00}", dr["MontoPorAplicar"]);
                                        
                                        //Instanciando Excepción
                                        lblErrorFactura.Text = string.Format("La Cantidad excede el Monto de {0:0.00}", dr["MPA2"]);
                                    }

                                    //Obteniendo Control
                                    LinkButton lnk = (LinkButton)sender;

                                    //Deshabilitando el Control
                                    txt.Enabled = false;

                                    //Configurando Control
                                    lnk.Text = lnk.CommandName = "Cambiar";
                                }
                                
                                //Actualizando Cambios
                                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                                //Obtenemos Filas Seleccionadas
                                GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvFacturas, "chkVariosFactura");

                                //Validando que Existan Filas
                                if (gvrs.Length > 0)
                                {
                                    //Obteniendo Indices
                                    indices_chk = new int[gvrs.Length];

                                    //Recorriendo Filas
                                    foreach (GridViewRow gvr in gvrs)
                                    {
                                        //Guardando Indice
                                        indices_chk[contador] = gvr.RowIndex;

                                        //Incrementando Contador
                                        contador++;
                                    }
                                }
                                else
                                    //Borrando Arreglo
                                    indices_chk = null;

                                //Cargando GridView
                                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                                //Validando que Existan Indices
                                if (indices_chk.Length > 0)
                                {
                                    //Creando Ciclo
                                    foreach (int indice in indices_chk)
                                    {
                                        //Seleccionando Indice
                                        gvFacturas.SelectedIndex = indice;

                                        //Obteniendo Control
                                        CheckBox chkFila = (CheckBox)gvFacturas.SelectedRow.FindControl("chkVariosFactura");

                                        //Validando que exista el Control
                                        if (chkFila != null)
                                        {
                                            //Marcando Control
                                            chkFila.Checked = true;

                                            //Obteniendo Control
                                            LinkButton lnk = (LinkButton)gvFacturas.SelectedRow.FindControl("lnkCambiar");

                                            //Validando que exista el Control
                                            if (lnk != null)

                                                //Habilitando Control
                                                lnk.Enabled = true;
                                        }
                                    }
                                }

                                //Inicializando INdices
                                Controles.InicializaIndices(gvFacturas);
                            }
                            break;
                        }
                }

                //Actualizando Totales
                actualizaTotales();

                //Invocando Método de Suma
                sumaTotalesFacturas();
            }
        }
        /// <summary>
        /// Evento Producido al Marcar un Registro del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAplicar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Validando que este seleccionado una Ficha de Ingreso
                if (gvFichasIngreso.SelectedIndex != -1)
                {
                    //Instanciando Ficha de Ingreso
                    using (SAT_CL.Bancos.EgresoIngreso ficha_ingreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvFichasIngreso.SelectedDataKey["Id"])))
                    {
                        //Validando que existe el Registro
                        if (ficha_ingreso.id_egreso_ingreso > 0)
                        {
                            //Seleccionando Fila
                            Controles.SeleccionaFila(gvFacturas, sender, "lnk", false);

                            //Declarando Variables Auxiliares
                            decimal monto_x_aplicar = 0.00M;
                            SAT_CL.Bancos.EgresoIngreso.Estatus estatus = ficha_ingreso.estatus;
                            SAT_CL.Facturacion.Facturado.EstatusFactura estatusFactura;

                            //Declarando Objeto de Retorno
                            RetornoOperacion result = new RetornoOperacion();

                            //Validando que el Monto Disponible sea Mayor o Igual al Monto Pendiente
                            if (Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]) >= Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPendiente"]))
                            {
                                //Validando que 
                                if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) > 0.00M)

                                    //Actualizando Registros
                                    monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]);
                                else
                                    //Actualizando Registros
                                    monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPendiente"]);
                            }
                            else
                            {   
                                //Validando que el Monto por Aplicar sea Menor que el Monto Disponible
                                if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) <= Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]))
                                {
                                    //Validando que 
                                    if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) > 0.00M)

                                        //Actualizando Registros
                                        monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]);
                                    else
                                        //Actualizando Registros
                                        monto_x_aplicar = Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]);
                                }
                                else
                                    //Actualizando Registros
                                    monto_x_aplicar = Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]);
                            }

                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Insertando Ficha de Ingreso Aplicada
                                result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(9, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), Convert.ToInt32(gvFichasIngreso.SelectedDataKey["Id"]),
                                                    monto_x_aplicar, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), false, 0, false, 0,
                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Validando que la Ficha tiene saldo
                                    if (monto_x_aplicar == Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]))

                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.Aplicada;
                                    else
                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.AplicadaParcial;

                                    //Actualizando Estatus de la Ficha de Ingreso
                                    result = ficha_ingreso.ActualizaFichaIngresoEstatus(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Operación fuese Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Mensaje de Operación
                                        result = new RetornoOperacion(string.Format("La Ficha {0}: ha sido Actualizada", gvFichasIngreso.SelectedDataKey["Id"]));

                                        //Mostrando Mensaje de Operación
                                        lblErrorFicha.Text = result.Mensaje;

                                        //Instanciando Factura
                                        using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                                        {
                                            //Validando que exista la Factura
                                            if (fac.id_factura > 0)
                                            {
                                                //Calculando Estatus de la Factura
                                                estatusFactura = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPendiente"]) - monto_x_aplicar > 0 ? SAT_CL.Facturacion.Facturado.EstatusFactura.AplicadaParcial : SAT_CL.Facturacion.Facturado.EstatusFactura.Liquidada;

                                                //Actualizando Estatus de la Factura
                                                result = fac.ActualizaEstatusFactura(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación Fuese Exitosa
                                                if(result.OperacionExitosa)
                                                {
                                                    //Instanciando Mensaje de Operación
                                                    result = new RetornoOperacion(string.Format("La Factura {0}: ha sido Aplicada Exitosamente", fac.id_factura));

                                                    //Invocando Método de Busqueda de Fichas de Ingreso
                                                    buscaFichasIngreso();

                                                    //Invocando Método de Busqueda de Facturas
                                                    buscaFacturas();

                                                    //Inicializando Indices
                                                    Controles.InicializaIndices(gvFacturas);

                                                    //Completando Transacción
                                                    trans.Complete();
                                                }
                                            }
                                        }
                                    }

                                    //Mostrando Mensaje de Operación
                                    lblErrorFactura.Text = result.Mensaje;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar el Monto Aplicado del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerFichasAplicadas_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturas, sender, "lnk", false);

                //Personalizando Mensaje
                lblVentanaFacturasFichas.Text = "Fichas Aplicadas";

                //Invocando Método de Busqueda
                buscaFichasAplicadas(0, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]));
                
                //Cambiando Encabezado del GridView
                gvFichasFacturas.Columns[1].HeaderText = "No. Ficha";
                upgvFichasFacturas.Update();

                //Mostrando Ventana
                ScriptServer.AlternarVentana(upgvFacturas, upgvFacturas.GetType(), "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
            }
        }

        #endregion

        #region Eventos GridView "Fichas Facturas"

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
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
            lblOrdenadoFF.Text = Controles.CambiaSortExpressionGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 1);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
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
            Controles.CambiaTamañoPaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoFF.SelectedValue), true, 1);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
        }
        /// <summary>
        /// Evento Producido al Eliminar la Aplicación del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarAplicacion_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvFichasFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFichasFacturas, sender, "lnk", false);
                SAT_CL.Facturacion.Facturado.EstatusFactura estatusFactura;

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Declarando Variable Auxiliar
                SAT_CL.Bancos.EgresoIngreso.Estatus estatus;
                int id_ficha_ingreso = 0;
                int id_factura = 0;

                //Instanciando Aplicación de la Ficha de Ingreso
                using (SAT_CL.CXC.FichaIngresoAplicacion fia = new FichaIngresoAplicacion(Convert.ToInt32(gvFichasFacturas.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Registro
                    if (fia.id_ficha_ingreso_aplicacion > 0)
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Deshabilitando Aplicación de la Ficha de Ingreso
                            result = fia.DeshabilitarFichaIngresoAplicacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Instanciando la Ficha de Ingreso
                                using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(fia.id_egreso_ingreso))
                                {
                                    //Validando que exista la Ficha de Ingreso
                                    if (ei.id_egreso_ingreso > 0)
                                    {
                                        //Asignando Estatus
                                        estatus = ei.estatus;

                                        //Validando que existan Registros
                                        if (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows.Count - 1 > 0)

                                            //Asignando Estatus a Aplciada Parcial
                                            estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.AplicadaParcial;

                                        else
                                            //Asignando Estatus A Capturada
                                            estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.Capturada;

                                        //Actualizando Ficha de Ingreso
                                        result = ei.ActualizaFichaIngresoEstatus(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando Factura
                                            using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(fia.id_registro)))
                                            {
                                                //Validando que exista la Factura
                                                if (fac.id_factura > 0)
                                                {
                                                    //Calculando Estatus de la Factura
                                                    estatusFactura = SAT_CL.Facturacion.Facturado.ObtieneMontoPendienteAplicacion(fia.id_registro) == fac.total_factura ? SAT_CL.Facturacion.Facturado.EstatusFactura.Registrada : SAT_CL.Facturacion.Facturado.EstatusFactura.AplicadaParcial;

                                                    //Actualizando Estatus de la Factura
                                                    result = fac.ActualizaEstatusFactura(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando que la Operación fuese Exitosa
                                                    if (result.OperacionExitosa)

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
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se puede Acceder a la Ficha de Ingreso");

                    //Validando que la Operación fuese Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Validando la Operación
                        switch (lblVentanaFacturasFichas.Text)
                        {
                            case "Facturas Aplicadas":
                                {
                                    //Asignando Valores
                                    id_ficha_ingreso = fia.id_egreso_ingreso;
                                    id_factura = 0;
                                    result = new RetornoOperacion(string.Format("La Factura {0}: Ha sido Desaplicada por el monto de {1:C2}", fia.id_registro, fia.monto_aplicado));
                                    break;
                                }
                            case "Fichas Aplicadas":
                                {
                                    //Asignando Valores
                                    id_ficha_ingreso = 0;
                                    id_factura = fia.id_registro;

                                    //Instanciando Egreso Ingreso
                                    using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(fia.id_egreso_ingreso))
                                    {
                                        //Instanciando Ficha
                                        result = new RetornoOperacion(string.Format("La Ficha {0}: Ha sido Desaplicada por el monto de {1:C2}", ei.secuencia_compania, fia.monto_aplicado));
                                    }
                                    
                                    break;
                                }
                        }

                        //Invocando Métodos de Busqueda
                        buscaFichasAplicadas(id_ficha_ingreso, id_factura);
                        buscaFichasIngreso();
                        buscaFacturas();

                        //Inicializando Indices
                        Controles.InicializaIndices(gvFichasFacturas);
                    }

                    //Mostrando Mensaje de Error
                    lblErrorFF.Text = result.Mensaje;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFF_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarFF_Click(object sender, EventArgs e)
        {
            //Invocando Métodos de Busqueda
            buscaFichasIngreso();
            buscaFacturas();

            //Inicializando Indices
            Controles.InicializaIndices(gvFacturas);
            Controles.InicializaIndices(gvFichasIngreso);

            //Limpiando Controles
            lblErrorFactura.Text =
            lblErrorFicha.Text =
            lblErrorFF.Text = "";

            //Mostrando Ventana
            ScriptServer.AlternarVentana(uplnkCerrarFF, uplnkCerrarFF.GetType(), "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Forma
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();

            //Mostrando Saldos
            actualizaTotales();

            //Inicializando GridViews
            Controles.InicializaGridview(gvFichasIngreso);
            Controles.InicializaGridview(gvFacturas);

            //Poner el cursor en el primer control
            txtCliente.Focus();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFI, "", 3182);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacturas, "", 3182);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFF, "", 3182);
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Fichas de Ingreso
        /// </summary>
        private void buscaFichasIngreso()
        {
            //Obteniendo Valor
            using (DataTable dtFichasIngreso = SAT_CL.Bancos.EgresoIngreso.ObtieneFichasIngresoConSaldo(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), 
                  ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(txtNoFicha.Text.Equals("") ? "0" : txtNoFicha.Text), txtReferencia.Text))
            {
                //Validando que existan Registros
                if(Validacion.ValidaOrigenDatos(dtFichasIngreso))
                {
                    //Validando que exista una Ficha Seleccionada
                    if (gvFichasIngreso.SelectedIndex != -1)

                        //Marcando Fila
                        TSDK.ASP.Controles.MarcaFila(gvFichasIngreso, gvFichasIngreso.SelectedDataKey["Id"].ToString(), "Id", "Id-IdMoneda-MontoDisponible", dtFichasIngreso, lblOrdenadoFI.Text, Convert.ToInt32(ddlTamanoFI.SelectedValue), true, 2);
                    else
                        //Cargando GridView
                        Controles.CargaGridView(gvFichasIngreso, dtFichasIngreso, "Id-IdMoneda-MontoDisponible", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasIngreso, "Table");
                    //Realizando calculo de totales al pie
                    sumaTotalesPieFI();
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFichasIngreso);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");

                    //Sumando Totales
                    gvFichasIngreso.FooterRow.Cells[5].Text = 
                    gvFichasIngreso.FooterRow.Cells[6].Text = 
                    gvFichasIngreso.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
                }

                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");

                //Invocando Método de Suma de Fichas
                sumaFichasTotales();
            }
        }
        /// <summary>
        /// Coloca la sumatoria al pie en el GV de FI
        /// </summary>
        private void sumaTotalesPieFI()
        {
            //Sumando Totales
            gvFichasIngreso.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
            gvFichasIngreso.FooterRow.Cells[7].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoAplicado)", "")));
            gvFichasIngreso.FooterRow.Cells[8].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoDisponible)", "")));
        }
        /// <summary>
        /// Método encargado de Sumar las Fichas de Ingreso por Estatus
        /// </summary>
        private void sumaFichasTotales()
        {
            //Validando Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Obteniendo Fichas
                int capturadas = (from DataRow fi in ((DataSet)Session["DS"]).Tables["Table"].Rows
                                  where fi["Estatus"].ToString().Equals("Capturada")
                                  select fi).Count();
                int a_parciales = (from DataRow fi in ((DataSet)Session["DS"]).Tables["Table"].Rows
                                   where fi["Estatus"].ToString().Equals("Aplicada Parcial")
                                   select fi).Count();

                //Asignando Valores
                lblCapturadas.Text = capturadas.ToString();
                lblAplicadasPar.Text = a_parciales.ToString();
                lblTotFichas.Text = (capturadas + a_parciales).ToString();
            }
            else
            {
                //Asignando Valores
                lblCapturadas.Text =
                lblAplicadasPar.Text =
                lblTotFichas.Text = "0";
            }
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Facturas
        /// </summary>
        private void buscaFacturas()
        {
            //Obteniendo Moneda
            int idMoneda = gvFichasIngreso.SelectedIndex != -1 ? Convert.ToInt32(gvFichasIngreso.SelectedDataKey["IdMoneda"]) : 0;

            //Obteniendo Valor
            using (DataTable dtFacturas = SAT_CL.Facturacion.Reporte.ObtieneFacturasPorPagar(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), txtFacturaGlobal.Text, txtUUID.Text, txtReferencia.Text,
                        Convert.ToInt32(Cadena.VerificaCadenaVacia(txtFolio.Text, "0")), idMoneda, chkSoloTimbradas.Checked))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturas, dtFacturas, "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }

            //Invocando Método de Suma
            sumaTotalesFacturas();
        }
        /// <summary>
        /// Método encargado de Sumar los Totales de las Facturas
        /// </summary>
        private void sumaTotalesFacturas()
        {
            //Validando que exista el Origend e Datos
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Sumando Totales
                gvFacturas.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(SubTotal)", "")));
                gvFacturas.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Trasladado)", "")));
                gvFacturas.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Retenido)", "")));
                gvFacturas.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoTotal)", "")));
                gvFacturas.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
                gvFacturas.FooterRow.Cells[15].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPendiente)", "")));
                gvFacturas.FooterRow.Cells[16].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPorAplicar)", "")));
            }
            else
            {
                //Sumando Totales
                gvFacturas.FooterRow.Cells[10].Text =
                gvFacturas.FooterRow.Cells[11].Text =
                gvFacturas.FooterRow.Cells[12].Text =
                gvFacturas.FooterRow.Cells[13].Text =
                gvFacturas.FooterRow.Cells[14].Text =
                gvFacturas.FooterRow.Cells[15].Text =
                gvFacturas.FooterRow.Cells[16].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método Privado encargado de la Configuración de los Registros de las Ffacturas
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="indices_chk">Indice de Controles</param>
        private void configuraRegistroFactura(object sender, out int[] indices_chk)
        {
            //Inicializando Variables
            indices_chk = new int[1];
            int contador = 0;
            bool indicador = false;
            
            //Declarando Variables Auxiliares
            decimal monto_sumatoria = 0.00M;
            decimal monto_disponible = 0.00M;

            //Obteniendo Control
            CheckBox chk = (CheckBox)sender;

            //Validando que el Control haya sido Marcado
            if (chk.Checked)
            {
                //Validando que exista la Ficha de Ingreso
                using (SAT_CL.Bancos.EgresoIngreso ficha = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvFichasIngreso.SelectedDataKey["Id"])))
                {
                    //Validando que exista la Ficha de Ingreso
                    if (ficha.id_egreso_ingreso > 0)
                    {
                        //Desmarcando Control para excluirlo del Calculo
                        chk.Checked = false;
                        
                        //Obteniendo las Otras Filas Seleccionadas
                        GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturas, "chkVariosFactura");

                        //Marcando Control para continuar la Operación
                        chk.Checked = true;

                        //Declarando Variables Auxiliares
                        indices_chk = new int[1];

                        //Validando que existan Filas Selecionadas
                        if (gvr.Length > 0)
                        {
                            //Creando Arreglo de Forma Dinamica
                            indices_chk = new int[gvr.Length + 1];

                            //Recorriendo Registros
                            foreach (GridViewRow gv in gvr)
                            {
                                //Obteniendo Indice Actual
                                gvFacturas.SelectedIndex =
                                indices_chk[contador] = gv.RowIndex;

                                //Añadiendo el Monto Pendiente a la Sumatoria
                                monto_sumatoria = monto_sumatoria + Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]);

                                //Incrementando Contador
                                contador++;
                            }
                        }

                        //Seleccionando Fila
                        Controles.SeleccionaFila(gvFacturas, sender, "chk", false);

                        //Guardando Indice Actual
                        indices_chk[contador] = gvFacturas.SelectedIndex;

                        //Calculando Monto Disponible
                        monto_disponible = (Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]) - monto_sumatoria) < 0 ? 0 : (Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]) - monto_sumatoria);
                        
                        //Obteniendo Resultado del Indicador
                        indicador = monto_disponible > 0 ? true : false;

                        //Obteniendo Fila por Editar
                        DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString() + " ");

                        //Recorriendo Registro Encontrado
                        foreach (DataRow dr in drEdit)
                        {
                            //Validando que el Monto Disponible sea Mayor o Igual al Monto Pendiente
                            if (monto_disponible >= Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPendiente"]))
                            {
                                //Actualizando Registros
                                dr["MontoPorAplicar"] = string.Format("{0:0.00}", gvFacturas.SelectedDataKey["MontoPendiente"]);
                                dr["MPA2"] = string.Format("{0:0.00}", gvFacturas.SelectedDataKey["MontoPendiente"]);
                            }
                            else
                            {   
                                //Actualizando Registros
                                dr["MontoPorAplicar"] = string.Format("{0:0.00}", monto_disponible);
                                dr["MPA2"] = string.Format("{0:0.00}", monto_disponible);
                            }
                        }

                        //Aceptando Cambios
                        ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();
                    }
                }
            }
            else
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturas, sender, "chk", false);

                //Obteniendo Fila por Editar
                DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString());

                //Recorriendo Registro Encontrado
                foreach (DataRow dr in drEdit)
                {
                    //Actualizando Registro
                    dr["MontoPorAplicar"] = "0.00";
                    dr["MPA2"] = "0.00";
                }

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                //Obtenemos Filas Seleccionadas
                GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvFacturas, "chkVariosFactura");

                //Validando que Existan Filas
                if (gvrs.Length > 0)
                {
                    //Creando Arreglo Dinamico
                    indices_chk = new int[gvrs.Length];
                    
                    //Recorriendo Filas
                    foreach (GridViewRow gvr in gvrs)
                    {
                        //Guardando Indice
                        indices_chk[contador] = gvr.RowIndex;

                        //Incrementando Contador
                        contador++;
                    }
                }
                else
                    //Borrando Arreglo
                    indices_chk = null;
            }

            //Cargando GrdiView
            Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-MontoPendiente-MontoPorAplicar", "", true, 2);

            //Validando que Existan Indices
            if (indices_chk != null)
            {
                //Validando que Existan Indices
                if (indices_chk.Length > 0)
                {
                    //Creando Ciclo
                    foreach (int indice in indices_chk)
                    {
                        //Seleccionando Indice
                        gvFacturas.SelectedIndex = indice;

                        //Obteniendo Control
                        CheckBox chkFila = (CheckBox)gvFacturas.SelectedRow.FindControl("chkVariosFactura");

                        //Validando que exista el Control
                        if (chkFila != null)
                        {
                            //Marcando Control
                            chkFila.Checked = true;

                            //Obteniendo Control
                            LinkButton lnk = (LinkButton)gvFacturas.SelectedRow.FindControl("lnkCambiar");

                            //Validando que exista el Control
                            if (lnk != null)

                                //Habilitando Control
                                lnk.Enabled = indicador ? chk.Checked : false;
                        }
                    }
                }
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvFacturas);
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Fichas Aplicadas
        /// </summary>
        /// <param name="id_ficha_ingreso"></param>
        /// <param name="id_factura"></param>
        private void buscaFichasAplicadas(int id_ficha_ingreso, int id_factura)
        {
            //Obteniendo Reporte
            using (DataTable dtFichasFacturas = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(id_ficha_ingreso, id_factura))
            {
                //Validando que Existen Registros
                if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFichasFacturas, dtFichasFacturas, "Id", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table2");

                    //Mostrando Totales
                    gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFichasFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");

                    //Mostrando Totales
                    gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
                }
            }
        }
        /// <summary>
        /// Método encargado de Actualizar los Totales
        /// </summary>
        private void actualizaTotales()
        {
            //Validando que una Ficha de Ingreso este Seleccionada
            if (gvFichasIngreso.SelectedIndex != -1)
            {
                //Asignando Saldo Disponible de la Ficha
                lblSaldoFI.Text = string.Format("{0:C2}", gvFichasIngreso.SelectedDataKey["MontoDisponible"]);
                
                //Validando que Existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                {
                    //Mostrando el Saldo por Aplicar
                    lblPorAplicar.Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPorAplicar)", "")));
                    lblSaldoFinal.Text = string.Format("{0:C2}", Convert.ToDecimal(gvFichasIngreso.SelectedDataKey["MontoDisponible"]) - Convert.ToDecimal((((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPorAplicar)", ""))));
                }
                else
                {
                    //Mostrando el Saldo en Ceros
                    lblPorAplicar.Text =
                    lblSaldoFinal.Text = string.Format("{0:C2}", 0);
                }
            }
            else
            {
                //Mostrando valores Iniciales
                lblSaldoFI.Text =
                lblPorAplicar.Text =
                lblSaldoFinal.Text = string.Format("{0:C2}", 0);
            }
        }

        #endregion

        #region Modal Cambio Estatus FI

        /// <summary>
        /// Click en botón de cierre de dialogo de cambio de estatus de FI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Cerrando ventana modal
            alternaVentanaModal("CambiarEstatusFI", lnkCerrarConfirmacionCambiarEstatus);
        }
        /// <summary>
        /// Click en botón Aceptar Cambio de estatus de FI (Pago Aplicado)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCambiarEstatusFI_Click(object sender, EventArgs e)
        {
            //Realizando actualización
            actualizarEstatusAplicadoFI();

        }
        /// <summary>
        /// Cambia el estatus de la FI (Pago de Cliente a 'Aplicado')
        /// </summary>
        private void actualizarEstatusAplicadoFI()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando registro seleccionado 
            using (SAT_CL.Bancos.EgresoIngreso ingreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvFichasIngreso.SelectedDataKey["Id"])))
            {
                //Si el registro existe
                if (ingreso.habilitar)
                    //Realizando actualización de estatus
                    resultado = ingreso.ActualizarEstatusManualFichaIngresoAplicado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Ocultando ventana modal
                alternaVentanaModal("CambiarEstatusFI", btnAceptarCambiarEstatusFI);
                //Recargando contenido de ventana
                buscaFichasIngreso();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarCambiarEstatusFI, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "CambiarEstatusFI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionCambioEstatusFI", "confirmacionCambioEstatusFI");
                    break;
            }
        }

        #endregion
    }
}